using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class UIBundle : UIElement, IUIBundle{
		public IUIElement GetFocusedElement(){
			if(_focusedElement == null)
				_focusedElement = initiallyFocusedElement;
			return _focusedElement;
		}
			IUIElement _focusedElement;
		IUIElement initiallyFocusedElement{
			get{
				if(m_initiallyFocusedElement == null)
					throw new System.InvalidOperationException("SlotSystemBundle.initiallyFocusedElement: is null, first assing in the inspector");
				else
					return m_initiallyFocusedElement;
			}
		} 
			IUIElement m_initiallyFocusedElement;
		public void SetFocusedElement(IUIElement element){
			if(this.Contains(element))
				_focusedElement = element;
			else
				throw new System.InvalidOperationException("SlotSystemBundleMB.SetFocusedBundleElement: trying to set focsed element that is not one of its members");
		}
		public override void SetHierarchy(){
			base.SetHierarchy();
			foreach(var ele in this)
				if(!ele.IsShownOnActivation())
					if(GetFocusedElement() == ele)
						ele.SetIsShownOnActivation(true);
		}
		public void InspectorSetUp(IUIElement initFocEle){
			if(!initFocEle.IsShownOnActivation())
				initFocEle.SetIsShownOnActivation(true);
			m_initiallyFocusedElement = initFocEle;
		}
	}
	public interface IUIBundle: IUIElement{
		IUIElement GetFocusedElement();
		void SetFocusedElement(IUIElement element);
	}
}
