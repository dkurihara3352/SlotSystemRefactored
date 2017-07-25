using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class GenericPage : SlotSystemPage, ISlotSystemPage{
		public override IEnumerable<ISlotSystemElement> elements{
			get{
				foreach(ISlotSystemPageElement pageEle in pageElements){
					yield return pageEle.element;
				}
			}
		}
		public void Initialize(string name, IEnumerable<ISlotSystemPageElement> pageEles){
			m_eName = SlotSystemUtil.Bold(name);
			m_pageElements = pageEles;
			InitializeStates();
		}
		public override void SetElements(){
			List<ISlotSystemElement> elements = new List<ISlotSystemElement>();
			List<ISlotSystemPageElement> pEles = new List<ISlotSystemPageElement>();
			for(int i =0; i< transform.childCount; i++){
				ISlotSystemElement element = transform.GetChild(i).GetComponent<ISlotSystemElement>();
					elements.Add(element);
				SlotSystemPageElement pEle = new SlotSystemPageElement(element, element.isToggledOnInPageByDefault);
					pEles.Add(pEle);
			}
			m_elements = elements;
			m_pageElements = pEles;
		}
		public override void Focus(){
			base.Focus();
			PageFocus();
		}
		public override void Deactivate(){
			base.Deactivate();
			ToggleBack();
		}
	}
}
