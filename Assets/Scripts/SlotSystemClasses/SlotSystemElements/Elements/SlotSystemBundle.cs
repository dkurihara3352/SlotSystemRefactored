using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SlotSystemBundle : SlotSystemElement, ISlotSystemBundle{
		/*	fields	*/
		public ISlotSystemElement focusedElement{
			get{return m_focusedElement;}
			}ISlotSystemElement m_focusedElement = null;
			public void SetFocusedBundleElement(ISlotSystemElement element){
				if(this.Contains(element))
					m_focusedElement = element;
				else
					throw new System.InvalidOperationException("SlotSystemBundleMB.SetFocusedBundleElement: trying to set focsed element that is not one of its members");
			}
		/*	methods	*/
		public void Initialize(string name, IEnumerable<ISlotSystemElement> elements){
			m_eName = SlotSystemUtil.Bold(name);
			m_elements = elements;
			InitializeStates();
		}
	}
	public interface ISlotSystemBundle: ISlotSystemElement{
		ISlotSystemElement focusedElement{get;}
		void SetFocusedBundleElement(ISlotSystemElement element);
	}
}
