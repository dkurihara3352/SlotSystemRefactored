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
				}IEnumerable<ISlotSystemElement> m_elements;
		public void Initialize(string name, IEnumerable<ISlotSystemPageElement> pageEles){
			m_eName = SlotSystemUtil.Bold(name);
			m_pageElements = pageEles;
			base.Initialize();
		}
		public override void Focus(){
			SetSelState(AbsSlotSystemElement.focusedState);
			PageFocus();
		}
		public override void Deactivate(){
			base.Deactivate();
			ToggleBack();
		}
	}
}
