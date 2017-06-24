using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class GenericPage : SlotSystemPage{
		protected override IEnumerable<SlotSystemElement> elements{
				get{
					foreach(SlotSystemPageElement pageEle in pageElements){
						yield return pageEle.element;
					}
				}
				}IEnumerable<SlotSystemElement> m_elements;
		public void Initialize(string name, IEnumerable<SlotSystemPageElement> pageEles){
			m_eName = Util.Bold(name);
			m_pageElements = pageEles;
			base.Initialize();
		}
		public override void Focus(){
			SetSelState(AbsSlotSystemElement.focusedState);
			PageFocus();
		}
	}
}
