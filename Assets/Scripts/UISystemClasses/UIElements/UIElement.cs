using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace UISystem{
	public interface IUIElement: IEnumerable<IUIElement>, IUISystemInputHandler{
		void InitializeStates();
		void SetHierarchy();
		bool IsShownOnActivation();
		void SetIsShownOnActivation(bool shown);
		bool IsFocusedInHierarchy();
		bool IsFocusableInHierarchy();
		void FocusInBundle();
		IUIBundle ImmediateBundle();
		IUIElement Parent();
		void SetParent(IUIElement par);
		void SetUIM(IUIManager uim);
		bool ContainsInHierarchy(IUIElement ele);
		void PerformInHierarchy(System.Action<IUIElement> act);
		void PerformInHierarchy(System.Action<IUIElement, object> act, object obj);
		void PerformInHierarchy<T>(System.Action<IUIElement, IList<T>> act, IList<T> list);
		bool Contains(IUIElement element);
		IUIElement this[int i]{get;}
		int GetLevel();
		IUISelStateHandler SelStateHandler();
			void Activate();
			void Deactivate();
			void MakeSelectable();
			void MakeUnselectable();
			bool IsSelectable();
			void Select();
			bool IsSelected();
			void Deselect();
		ITapStateHandler TapStateHandler();
		void ExecuteTapCommand();
	}
	public class UIElement : IUIElement{
		RectTransformFake rectTransform;
		public UIElement(RectTransformFake rectTrans, IUISelStateRepo selStateRepo, ITapCommand tapCommand){
			rectTransform = rectTrans;
			SetSelStateHandler(new UISelStateHandler(this, selStateRepo));
			SetTapStateHandler(new TapStateHandler(this));
			SetTapCommand(tapCommand);
		}
		/* Sel State Handling */
			public IUISelStateHandler SelStateHandler(){
				Debug.Assert(_selStateHandler != null);
				return _selStateHandler;
			}
			public void SetSelStateHandler(IUISelStateHandler handler){
				_selStateHandler = handler;
			}
				IUISelStateHandler _selStateHandler;
			public void Activate(){
				SelStateHandler().Activate();
				ActivateChildren();
			}
				void ActivateChildren(){
					foreach(var ele in this)
						ele.Activate();
				}
			public void Deactivate(){
				SelStateHandler().Deactivate();
				DeactivateChildren();
			}
				void DeactivateChildren(){
					foreach(var ele in this)
						ele.Deactivate();
				}
			public void MakeSelectable(){
				SelStateHandler().MakeSelectable();
				MakeChildrenSelectable();
			}
				void MakeChildrenSelectable(){
					foreach(var ele in this)
						ele.MakeSelectable();
				}
			public void MakeUnselectable(){
				SelStateHandler().MakeUnselectable();
				MakeChildrenUnselectable();
			}
				void MakeChildrenUnselectable(){
					foreach(var ele in this)
						ele.MakeUnselectable();
				}
			public void Select(){
				SelStateHandler().Select();
			}
				public bool IsSelected(){
					return SelStateHandler().IsSelected();
				}
			public bool IsSelectable(){
				return SelStateHandler().IsSelectable();
			}
			public bool IsUnselectable(){
				return SelStateHandler().IsUnselectable();
			}
			public void Deselect(){
				SelStateHandler().Deselect();
			}
			public virtual void InitializeStates(){
				SelStateHandler().Deactivate();
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
			public bool IsShownOnActivation(){
				IUIElement inspected = Parent();
				while(true){
					if(inspected　== null)
						break;
					if(inspected.IsShownOnActivation())
						inspected = inspected.Parent();
					else
						return false;
				}
				return _isShownOnActivation;
			}
			public void SetIsShownOnActivation(bool shown){
				if(isBundleElement && ImmediateBundle().GetFocusedElement() == (IUIElement)this)
					_isShownOnActivation = true;
				else
					_isShownOnActivation = shown;
			}
				bool _isShownOnActivation;
			public bool IsFocusedInHierarchy(){
				IUIElement inspected = this;
				while(true){
					if(inspected　== null)
						break;
					if(inspected.IsSelectable())
						inspected = inspected.Parent();
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
					return Parent() is IUIBundle;
				}
			}
			public IUIBundle ImmediateBundle(){
				IUIElement parent = Parent();
				if(parent == null)
					return null;
				else if(parent is IUIBundle)
					return (IUIBundle)parent;
				else
					return parent.ImmediateBundle();
			}
			public virtual IUIElement Parent(){
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
					IUIElement testEle = ele.Parent();
					while(true){
						if(testEle == null)
							return false;
						if(testEle == (IUIElement)this)
							return true;
						testEle = testEle.Parent();
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
				IUIElement parent = Parent();
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
		/* Input Handling */
			public ITapStateHandler TapStateHandler(){
				Debug.Assert(_tapStateHandler != null);
				return _tapStateHandler;
			}
			void SetTapStateHandler(ITapStateHandler handler){
				_tapStateHandler = handler;
			}
				ITapStateHandler _tapStateHandler;
			public void ExecuteTapCommand(){
				TapCommand().Execute();
			}
			ITapCommand TapCommand(){
				Debug.Assert(_tapCommand != null);
				return _tapCommand;
			}
			void SetTapCommand(ITapCommand comm){
				_tapCommand = comm;
				TapCommand().SetUIElement(this);
			}
				ITapCommand _tapCommand;
			public virtual void OnPointerDown(){
				TapStateHandler().OnPointerDown();
			}
			public virtual void OnPointerUp(){
				TapStateHandler().OnPointerUp();
			}
			public virtual void OnEndDrag(){
				TapStateHandler().OnEndDrag();
			}
			public virtual void OnDeselected(){
				TapStateHandler().OnDeselected();
			}
	}
}
