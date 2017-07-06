using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SlotSystemBundle : AbsSlotSystemElement{
		/*	fields	*/
		protected override IEnumerable<SlotSystemElement> elements{
			get{return m_elements;}
			}IEnumerable<SlotSystemElement> m_elements;

		public SlotSystemElement focusedElement{
			get{return m_focusedElement;}
			}SlotSystemElement m_focusedElement;
			public void SetFocusedBundleElement(SlotSystemElement element){
				if(this.Contains(element))
					m_focusedElement = element;
				else
					throw new System.InvalidOperationException("SlotSystemBundleMB.SetFocusedBundleElement: trying to set focsed element that is not one of its members");
			}
		/*	methods	*/
		public void Initialize(string name, IEnumerable<SlotSystemElement> elements){
			m_eName = SlotSystemUtil.Bold(name);
			m_elements = elements;
			base.Initialize();
		}
		public override void Focus(){
			SetSelState(AbsSlotSystemElement.focusedState);
			if(m_focusedElement != null)
				m_focusedElement.Focus();
			foreach(SlotSystemElement ele in this){
				if(ele != m_focusedElement)
				ele.Defocus();
			}
		}
		public override void Defocus(){
			SetSelState(AbsSlotSystemElement.defocusedState);
			foreach(SlotSystemElement ele in this){
				ele.Defocus();
			}
		}	
	}
}
