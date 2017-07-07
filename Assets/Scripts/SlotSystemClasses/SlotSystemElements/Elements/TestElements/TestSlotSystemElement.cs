using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TestSlotSystemElement: AbsSlotSystemElement{
		protected override IEnumerable<SlotSystemElement> elements{get{return m_elements;}}
		IEnumerable<SlotSystemElement> m_elements;
		public void SetElements(IEnumerable<SlotSystemElement> elements){
			m_elements = elements;
		}
		public string message = "";
		public override void InstantGreyout(){
			message = "InstantGreyout called";
		}
		public override void InstantGreyin(){
			message = "InstantGreyin called";
		}
		public override void InstantHighlight(){
			message = "InstantHighlight called";
		}
	}
}
