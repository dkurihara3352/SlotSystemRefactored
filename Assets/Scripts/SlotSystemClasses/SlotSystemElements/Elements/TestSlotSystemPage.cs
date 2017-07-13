using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class TestSlotSystemPage : SlotSystemPage {

		public override IEnumerable<ISlotSystemElement> elements{get{return m_elements;}}
			IEnumerable<ISlotSystemElement> m_elements;
			public void SetElements(IEnumerable<ISlotSystemElement> eles){
				m_elements = eles;
			}
		public void SetPageElements(IEnumerable<ISlotSystemPageElement> pEles){
			m_pageElements = pEles;
		}
	}
}
