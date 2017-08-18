using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public abstract class SlotSystemElement :MonoBehaviour, ISlotSystemElement{
		/* State Handling */
			public virtual ISSESelStateHandler GetSelStateHandler(){
				if(_selStateHandler != null)
					return _selStateHandler;
				else
					throw new InvalidOperationException("selStateHandler not set");
			}
				ISSESelStateHandler _selStateHandler;
			public virtual void SetSelStateHandler(ISSESelStateHandler handler){
				_selStateHandler = handler;
			}
			public virtual void InitializeStates(){
				GetSelStateHandler().Deactivate();
			}
			public void InitializeSSE(){
				SetSelStateHandler(new SSESelStateHandler());
			}
		/*	public fields	*/
			public virtual void SetHierarchy(){
				List<ISlotSystemElement> elements = new List<ISlotSystemElement>();
				for(int i = 0; i < transform.childCount; i++){
					ISlotSystemElement ele = transform.GetChild(i).GetComponent<ISlotSystemElement>();
					if(ele != null){
						elements.Add(ele);
					}
				}
				m_elements = elements;
				foreach(var e in this)
					e.SetParent(this);
			}
			public bool IsActivatedOnDefault(){
				ISlotSystemElement inspected = GetParent();
				while(true){
					if(inspected　== null)
						break;
					if(inspected.IsActivatedOnDefault())
						inspected = inspected.GetParent();
					else
						return false;
				}
				return _isActivatedOnDefault;
			}
			public void SetIsActivatedOnDefault(bool activated){
				if(isBundleElement && ImmediateBundle().GetFocusedElement() == (ISlotSystemElement)this)
					_isActivatedOnDefault = true;
				else
					_isActivatedOnDefault = activated;
			}
				bool _isActivatedOnDefault = true;
			public void ActivateRecursively(){
				PerformInHierarchy(ActivateInHi);
			}
				void ActivateInHi(ISlotSystemElement ele){
					ISSESelStateHandler selStateHandler = ele.GetSelStateHandler();
					if(ele.IsActivatedOnDefault()){
						if(ele.IsFocusableInHierarchy())
							selStateHandler.Activate();
						else
							selStateHandler.Defocus();
					}
				}
			public bool IsFocusedInHierarchy(){
				ISlotSystemElement inspected = this;
				ISSESelStateHandler selStateHandler = inspected.GetSelStateHandler();
				while(true){
					if(inspected　== null)
						break;
					if(selStateHandler.IsFocused()){
						inspected = inspected.GetParent();
						if(inspected != null)
							selStateHandler = inspected.GetSelStateHandler();
					}
					else
						return false;
				}
				return true;
			}
			public bool IsFocusableInHierarchy(){
				ISlotSystemElement inspected = this;
				while(true){
					ISlotSystemBundle inspectedImmBun = inspected.ImmediateBundle();
					if(inspectedImmBun == null)
						break;
					ISlotSystemElement inspImmBunFocusedEle = inspectedImmBun.GetFocusedElement();
					if(inspImmBunFocusedEle == inspected || inspImmBunFocusedEle.ContainsInHierarchy(inspected))
						inspected = inspectedImmBun;
					else
						return false;
				}
				return true;
			}
			public bool isBundleElement{
				get{
					return GetParent() is ISlotSystemBundle;
				}
			}
			public ISlotSystemBundle ImmediateBundle(){
				ISlotSystemElement parent = GetParent();
				if(parent == null)
					return null;
				else if(parent is ISlotSystemBundle)
					return (ISlotSystemBundle)parent;
				else
					return parent.ImmediateBundle();
			}
			public virtual ISlotSystemElement GetParent(){
				return _parent;
			}
				ISlotSystemElement _parent;
			public void SetParent(ISlotSystemElement par){
				_parent = par;
			}
			public ISlotSystemManager GetSSM(){
				if(_ssm != null)
					return _ssm;
				else
					throw new InvalidOperationException("ssm not set");
			}
				ISlotSystemManager _ssm;
			public void SetSSM(ISlotSystemElement ssm){
				_ssm = (ISlotSystemManager)ssm;
			}
			public virtual bool ContainsInHierarchy(ISlotSystemElement ele){
				if(ele != null){
					ISlotSystemElement testEle = ele.GetParent();
					while(true){
						if(testEle == null)
							return false;
						if(testEle == (ISlotSystemElement)this)
							return true;
						testEle = testEle.GetParent();
					}
				}
				throw new System.ArgumentNullException();
			}
			public void PerformInHierarchy(System.Action<ISlotSystemElement> act){
				act(this);
				if(elements != null)
					foreach(ISlotSystemElement ele in this){
						if(ele != null)
							ele.PerformInHierarchy(act);
					}
			}
			public void PerformInHierarchy(System.Action<ISlotSystemElement, object> act, object obj){
				act(this, obj);
				if(elements != null)
					foreach(ISlotSystemElement ele in this){
						if(ele != null)
							ele.PerformInHierarchy(act, obj);
					}
			}
			public void PerformInHierarchy<T>(System.Action<ISlotSystemElement, IList<T>> act, IList<T> list){
				act(this, list);
				if(elements != null)
					foreach(ISlotSystemElement ele in this){
						if(ele != null)
							ele.PerformInHierarchy<T>(act, list);
					}
			}
			public virtual bool Contains(ISlotSystemElement element){
				if(elements != null)
					foreach(ISlotSystemElement ele in elements){
						if(ele != null && ele == element)
							return true;
					}
				return false;
			}
			public virtual void SetElements(IEnumerable<ISlotSystemElement> elements){
				m_elements = elements;
			}
			public ISlotSystemElement this[int i]{
				get{
					int id = 0;
					foreach(var ele in elements){
						if(id++ == i)
							return ele;	
					}
					throw new System.ArgumentOutOfRangeException("AbsSlotSysElement.indexer: argument out of range");
				}
			}
			protected virtual IEnumerable<ISlotSystemElement> elements{
				get{
					if(m_elements == null)
						m_elements = new ISlotSystemElement[]{};
					return m_elements;
				}
			}
				IEnumerable<ISlotSystemElement> m_elements;
			public int GetLevel(){
				ISlotSystemElement parent = GetParent();
				if(parent == null)
					return 0;
				return parent.GetLevel() + 1;
			}
			public IEnumerator<ISlotSystemElement> GetEnumerator(){
				foreach(ISlotSystemElement ele in elements)
					yield return ele;
			}
				IEnumerator IEnumerable.GetEnumerator(){
					return GetEnumerator();
				}	
	}
	public interface ISlotSystemElement: IEnumerable<ISlotSystemElement>{
		void InitializeStates();
		void SetHierarchy();
		bool IsActivatedOnDefault();
		void SetIsActivatedOnDefault(bool activated);
		bool IsFocusedInHierarchy();
		bool IsFocusableInHierarchy();
		ISlotSystemBundle ImmediateBundle();
		ISlotSystemElement GetParent();
		void SetParent(ISlotSystemElement par);
		ISlotSystemManager GetSSM();
		void SetSSM(ISlotSystemElement ssm);
		bool ContainsInHierarchy(ISlotSystemElement ele);
		void PerformInHierarchy(System.Action<ISlotSystemElement> act);
		void PerformInHierarchy(System.Action<ISlotSystemElement, object> act, object obj);
		void PerformInHierarchy<T>(System.Action<ISlotSystemElement, IList<T>> act, IList<T> list);
		bool Contains(ISlotSystemElement element);
		ISlotSystemElement this[int i]{get;}
		int GetLevel();
		ISSESelStateHandler GetSelStateHandler();
	}
}
