using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public abstract class SlotSystemElement :MonoBehaviour, ISlotSystemElement{
		/* State Handling */
			public virtual ISSESelStateHandler selStateHandler{
				get{
					if(_selStateHandler == null)
						_selStateHandler = new SSESelStateHandler();
					return _selStateHandler;
				}
			}
				ISSESelStateHandler _selStateHandler;
			public virtual void SetSelStateHandler(ISSESelStateHandler handler){
				_selStateHandler = handler;
			}
			public bool isSelStateNull{
				get{return selStateHandler.isSelStateNull;}
			}
			public bool wasSelStateNull{
				get{return selStateHandler.wasSelStateNull;}
			}
			public void Deactivate(){
				selStateHandler.Deactivate();
			}
				public bool isDeactivated{
					get{return selStateHandler.isDeactivated;}
				}
				public bool wasDeactivated{
					get{return selStateHandler.wasDeactivated;}
				}
			public void Focus(){
				selStateHandler.Focus();
			}
				public bool isFocused{
					get{return selStateHandler.isFocused;}
				}
				public bool wasFocused{
					get{return selStateHandler.wasFocused;}
				}
			public void Defocus(){
				selStateHandler.Defocus();
			}
				public bool isDefocused{
					get{return selStateHandler.isDefocused;}
				}
				public bool wasDefocused{
					get{return selStateHandler.wasDefocused;}
				}
			public void Select(){
				selStateHandler.Select();
			}
				public bool isSelected{
					get{return selStateHandler.isSelected;}
				}
				public bool wasSelected{
					get{return selStateHandler.wasSelected;}
				}
			public void Activate(){
				selStateHandler.Activate();
			}
			public void Deselect(){
				selStateHandler.Deselect();
			}
			public virtual void InitializeStates(){
				Deactivate();
			}
			public void SetAndRunSelProcess(ISSESelProcess process){
				selStateHandler.SetAndRunSelProcess(process);
			}
			public System.Func<IEnumeratorFake> deactivateCoroutine{
				get{return selStateHandler.deactivateCoroutine;}
			}
			public System.Func<IEnumeratorFake> focusCoroutine{
				get{return selStateHandler.focusCoroutine;}
			}
			public System.Func<IEnumeratorFake> defocusCoroutine{
				get{return selStateHandler.defocusCoroutine;}
			}
			public System.Func<IEnumeratorFake> selectCoroutine{
				get{return selStateHandler.selectCoroutine;}
			}
			public void InstantFocus(){
				selStateHandler.InstantFocus();
			}
			public void InstantDefocus(){
				selStateHandler.InstantDefocus();
			}
			public void InstantSelect(){
				selStateHandler.InstantSelect();
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
			public bool isActivatedOnDefault{
				get{
					ISlotSystemElement inspected = parent;
					while(true){
						if(inspected　== null)
							break;
						if(inspected.isActivatedOnDefault)
							inspected = inspected.parent;
						else
							return false;
					}
					return m_isActivatedOnDefault;
				}
				set{
					if(isBundleElement && immediateBundle.focusedElement == (ISlotSystemElement)this)
						m_isActivatedOnDefault = true;
					else
						m_isActivatedOnDefault = value;
					}
			}
				bool m_isActivatedOnDefault = true;
			public void ActivateRecursively(){
				PerformInHierarchy(ActivateInHi);
			}
				void ActivateInHi(ISlotSystemElement ele){
					if(ele.isActivatedOnDefault){
						if(ele.isFocusableInHierarchy)
							ele.Activate();
						else
							ele.Defocus();
					}
				}
			public bool isFocusedInHierarchy{
				get{
					ISlotSystemElement inspected = this;
					while(true){
						if(inspected　== null)
							break;
						if(inspected.isFocused)
							inspected = inspected.parent;
						else
							return false;
					}
					return true;
				}
			}			
			public bool isFocusableInHierarchy{
				get{
					ISlotSystemElement inspected = this;
					while(true){
						if(inspected.immediateBundle == null)
							break;
						if(inspected.immediateBundle.focusedElement == inspected || inspected.immediateBundle.focusedElement.ContainsInHierarchy(inspected))
							inspected = inspected.immediateBundle;
						else
							return false;
					}
					return true;
				}
			}
			public bool isBundleElement{
				get{
					return parent is ISlotSystemBundle;
				}
			}
			public ISlotSystemBundle immediateBundle{
				get{
					if(parent == null)
						return null;
					else if(parent is ISlotSystemBundle)
						return (ISlotSystemBundle)parent;
					else
						return parent.immediateBundle;
				}
			}
			public virtual ISlotSystemElement parent{
				get{return m_parent;}
			}
				ISlotSystemElement m_parent;
			public void SetParent(ISlotSystemElement par){
				m_parent = par;
			}
			public ISlotSystemManager ssm{
				get{
					if(m_ssm != null)
						return m_ssm;
					else
						throw new InvalidOperationException("ssm not set");
				}
			}
				ISlotSystemManager m_ssm;
			public void SetSSM(ISlotSystemElement ssm){
				m_ssm = (ISlotSystemManager)ssm;
			}
			public virtual bool ContainsInHierarchy(ISlotSystemElement ele){
				if(ele != null){
					ISlotSystemElement testEle = ele.parent;
					while(true){
						if(testEle == null)
							return false;
						if(testEle == (ISlotSystemElement)this)
							return true;
						testEle = testEle.parent;
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
			public int level{
				get{
					if(parent == null)
						return 0;
					return parent.level + 1;
				}
			}
			public IEnumerator<ISlotSystemElement> GetEnumerator(){
				foreach(ISlotSystemElement ele in elements)
					yield return ele;
			}
				IEnumerator IEnumerable.GetEnumerator(){
					return GetEnumerator();
				}	
	}
	public interface ISlotSystemElement: IEnumerable<ISlotSystemElement>, ISSESelStateHandler{
		void InitializeStates();
		void SetHierarchy();
		bool isActivatedOnDefault{get;set;}
		bool isFocusedInHierarchy{get;}
		bool isFocusableInHierarchy{get;}
		ISlotSystemBundle immediateBundle{get;}
		ISlotSystemElement parent{get;}
		void SetParent(ISlotSystemElement par);
		ISlotSystemManager ssm{get;}
		void SetSSM(ISlotSystemElement ssm);
		bool ContainsInHierarchy(ISlotSystemElement ele);
		void PerformInHierarchy(System.Action<ISlotSystemElement> act);
		void PerformInHierarchy(System.Action<ISlotSystemElement, object> act, object obj);
		void PerformInHierarchy<T>(System.Action<ISlotSystemElement, IList<T>> act, IList<T> list);
		bool Contains(ISlotSystemElement element);
		ISlotSystemElement this[int i]{get;}
		int level{get;}
		ISSESelStateHandler selStateHandler{get;}
	}
}
