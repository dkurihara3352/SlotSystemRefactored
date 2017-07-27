using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SlotSystemBundle : SlotSystemElement, ISlotSystemBundle{
		public void InspectorSetUp(ISlotSystemElement initFocEle){
			if(!initFocEle.isActivatedOnDefault)
				initFocEle.isActivatedOnDefault = true;
			m_initiallyFocusedElement = initFocEle;
		}
		public override void SetHierarchy(){
			base.SetHierarchy();
			foreach(var ele in this)
				if(!ele.isActivatedOnDefault)
					if(focusedElement == ele)
						ele.isActivatedOnDefault = true;
		}
		public ISlotSystemElement initiallyFocusedElement{
			get{
				if(m_initiallyFocusedElement == null)
					throw new System.InvalidOperationException("SlotSystemBundle.initiallyFocusedElement: is null, first assing in the inspector");
				else
					return m_initiallyFocusedElement;
				}
			} 
			ISlotSystemElement m_initiallyFocusedElement;
		public ISlotSystemElement focusedElement{
			get{
				if(m_focusedElement == null)
					m_focusedElement = initiallyFocusedElement;
				return m_focusedElement;
			}
			}ISlotSystemElement m_focusedElement;
			public void SetFocusedBundleElement(ISlotSystemElement element){
				if(this.Contains(element))
					m_focusedElement = element;
				else
					throw new System.InvalidOperationException("SlotSystemBundleMB.SetFocusedBundleElement: trying to set focsed element that is not one of its members");
			}
		
	}
	public interface ISlotSystemBundle: ISlotSystemElement{
		ISlotSystemElement initiallyFocusedElement{get;}
		ISlotSystemElement focusedElement{get;}
		void SetFocusedBundleElement(ISlotSystemElement element);
	}
}
