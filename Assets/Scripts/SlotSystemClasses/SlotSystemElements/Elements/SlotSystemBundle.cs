using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SlotSystemBundle : SlotSystemElement, ISlotSystemBundle{
		public ISlotSystemElement GetFocusedElement(){
			if(_focusedElement == null)
				_focusedElement = initiallyFocusedElement;
			return _focusedElement;
		}
			ISlotSystemElement _focusedElement;
		ISlotSystemElement initiallyFocusedElement{
			get{
				if(m_initiallyFocusedElement == null)
					throw new System.InvalidOperationException("SlotSystemBundle.initiallyFocusedElement: is null, first assing in the inspector");
				else
					return m_initiallyFocusedElement;
			}
		} 
			ISlotSystemElement m_initiallyFocusedElement;
		public void SetFocusedElement(ISlotSystemElement element){
			if(this.Contains(element))
				_focusedElement = element;
			else
				throw new System.InvalidOperationException("SlotSystemBundleMB.SetFocusedBundleElement: trying to set focsed element that is not one of its members");
		}
		public override void SetHierarchy(){
			base.SetHierarchy();
			foreach(var ele in this)
				if(!ele.IsActivatedOnDefault())
					if(GetFocusedElement() == ele)
						ele.SetIsActivatedOnDefault(true);
		}
		public void InspectorSetUp(ISlotSystemElement initFocEle){
			if(!initFocEle.IsActivatedOnDefault())
				initFocEle.SetIsActivatedOnDefault(true);
			m_initiallyFocusedElement = initFocEle;
		}
	}
	public interface ISlotSystemBundle: ISlotSystemElement{
		ISlotSystemElement GetFocusedElement();
		void SetFocusedElement(ISlotSystemElement element);
	}
}
