using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace UISystem{
	public class UIElement : IUIElement{
		RectTransformFake rectTransform;
		public UIElement(RectTransformFake rectTrans){
			rectTransform = rectTrans;
		}
		/* State Handling */
			public virtual IUISelStateHandler UISelStateHandler(){
				if(_selStateHandler != null)
					return _selStateHandler;
				else
					throw new InvalidOperationException("selStateHandler not set");
			}
			public virtual void SetSelStateHandler(IUISelStateHandler handler){
				_selStateHandler = handler;
			}
				IUISelStateHandler _selStateHandler;
			public void Activate(){
				UISelStateHandler().Activate();
			}
			public void Deactivate(){
				UISelStateHandler().Deactivate();
			}
			public void MakeSelectable(){
				UISelStateHandler().MakeSelectable();
			}
			public void MakeUnselectable(){
				UISelStateHandler().MakeUnselectable();
			}
			public bool IsSelectable(){
				return UISelStateHandler().IsSelectable();
			}
			public bool IsUnselectable(){
				return UISelStateHandler().IsUnselectable();
			}
			public void Select(){
				UISelStateHandler().Select();
			}
			public virtual void InitializeStates(){
				UISelStateHandler().Deactivate();
			}
			public void InitializeSSE(){
				SetSelStateHandler(new UISelStateHandler());
			}
		/*	public fields	*/
			public virtual void SetHierarchy(){
				List<IUIElement> elements = new List<IUIElement>();
				for(int i = 0; i < rectTransform.childCount; i++){
					IUIElement ele = rectTransform.GetChild(i).GetComponent<IUIElement>();
					if(ele != null){
						elements.Add(ele);
					}
				}
				_elements = elements;
				foreach(var e in this)
					e.SetParent(this);
			}
			public bool IsActivatedOnDefault(){
				IUIElement inspected = GetParent();
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
				if(isBundleElement && ImmediateBundle().GetFocusedElement() == (IUIElement)this)
					_isActivatedOnDefault = true;
				else
					_isActivatedOnDefault = activated;
			}
				bool _isActivatedOnDefault = true;
			public void ActivateRecursively(){
				PerformInHierarchy(ActivateInHi);
			}
				void ActivateInHi(IUIElement ele){
					if(ele.IsActivatedOnDefault()){
						if(ele.IsFocusableInHierarchy())
							ele.Activate();
						else
							ele.MakeUnselectable();
					}
				}
			public bool IsFocusedInHierarchy(){
				IUIElement inspected = this;
				while(true){
					if(inspected　== null)
						break;
					if(inspected.IsSelectable())
						inspected = inspected.GetParent();
					else
						return false;
				}
				return true;
			}
			public bool IsFocusableInHierarchy(){
				IUIElement inspected = this;
				while(true){
					IUIBundle inspectedImmBun = inspected.ImmediateBundle();
					if(inspectedImmBun == null)
						break;
					IUIElement inspImmBunFocusedEle = inspectedImmBun.GetFocusedElement();
					if(inspImmBunFocusedEle == inspected || inspImmBunFocusedEle.ContainsInHierarchy(inspected))
						inspected = inspectedImmBun;
					else
						return false;
				}
				return true;
			}
			public void FocusInBundle(){
				IUIElement tested = this;
				while(true){
					IUIBundle immBundle = tested.ImmediateBundle();
					if(immBundle == null)
						break;
					IUIElement containingEle = null;
					foreach(IUIElement e in immBundle){
						if(e.ContainsInHierarchy(tested) || e == tested)
							containingEle = e;
					}
					immBundle.FocusElement(containingEle);
					tested = tested.ImmediateBundle();
				}
				_selStateHandler.MakeSelectable();
			}
			public bool isBundleElement{
				get{
					return GetParent() is IUIBundle;
				}
			}
			public IUIBundle ImmediateBundle(){
				IUIElement parent = GetParent();
				if(parent == null)
					return null;
				else if(parent is IUIBundle)
					return (IUIBundle)parent;
				else
					return parent.ImmediateBundle();
			}
			public virtual IUIElement GetParent(){
				return _parent;
			}
				IUIElement _parent;
			public void SetParent(IUIElement par){
				_parent = par;
			}
			public void SetUIM(IUIManager uim){
				_uim = uim;
			}
			IUIManager _uim;
			public virtual bool ContainsInHierarchy(IUIElement ele){
				if(ele != null){
					IUIElement testEle = ele.GetParent();
					while(true){
						if(testEle == null)
							return false;
						if(testEle == (IUIElement)this)
							return true;
						testEle = testEle.GetParent();
					}
				}
				throw new System.ArgumentNullException();
			}
			public void PerformInHierarchy(System.Action<IUIElement> act){
				act(this);
				if(elements != null)
					foreach(IUIElement ele in this){
						if(ele != null)
							ele.PerformInHierarchy(act);
					}
			}
			public void PerformInHierarchy(System.Action<IUIElement, object> act, object obj){
				act(this, obj);
				if(elements != null)
					foreach(IUIElement ele in this){
						if(ele != null)
							ele.PerformInHierarchy(act, obj);
					}
			}
			public void PerformInHierarchy<T>(System.Action<IUIElement, IList<T>> act, IList<T> list){
				act(this, list);
				if(elements != null)
					foreach(IUIElement ele in this){
						if(ele != null)
							ele.PerformInHierarchy<T>(act, list);
					}
			}
			public virtual bool Contains(IUIElement element){
				if(elements != null)
					foreach(IUIElement ele in elements){
						if(ele != null && ele == element)
							return true;
					}
				return false;
			}
			public virtual void SetElements(IEnumerable<IUIElement> elements){
				_elements = elements;
			}
			public IUIElement this[int i]{
				get{
					int id = 0;
					foreach(var ele in elements){
						if(id++ == i)
							return ele;	
					}
					throw new System.ArgumentOutOfRangeException("AbsSlotSysElement.indexer: argument out of range");
				}
			}
			protected virtual IEnumerable<IUIElement> elements{
				get{
					if(_elements == null)
						_elements = new IUIElement[]{};
					return _elements;
				}
			}
				IEnumerable<IUIElement> _elements;
			public int GetLevel(){
				IUIElement parent = GetParent();
				if(parent == null)
					return 0;
				return parent.GetLevel() + 1;
			}
			public IEnumerator<IUIElement> GetEnumerator(){
				foreach(IUIElement ele in elements)
					yield return ele;
			}
				IEnumerator IEnumerable.GetEnumerator(){
					return GetEnumerator();
				}	
	}
	public interface IUIElement: IEnumerable<IUIElement>{
		void InitializeStates();
		void SetHierarchy();
		bool IsActivatedOnDefault();
		void SetIsActivatedOnDefault(bool activated);
		bool IsFocusedInHierarchy();
		bool IsFocusableInHierarchy();
		void FocusInBundle();
		IUIBundle ImmediateBundle();
		IUIElement GetParent();
		void SetParent(IUIElement par);
		void SetUIM(IUIManager uim);
		bool ContainsInHierarchy(IUIElement ele);
		void PerformInHierarchy(System.Action<IUIElement> act);
		void PerformInHierarchy(System.Action<IUIElement, object> act, object obj);
		void PerformInHierarchy<T>(System.Action<IUIElement, IList<T>> act, IList<T> list);
		bool Contains(IUIElement element);
		IUIElement this[int i]{get;}
		int GetLevel();
		IUISelStateHandler UISelStateHandler();
		void Activate();
		void Deactivate();
		void MakeSelectable();
		void MakeUnselectable();
		bool IsSelectable();
		bool IsUnselectable();
		void Select();
	}
}
