using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TestSlotSystemPage: SlotSystemPage{
		protected override IEnumerable<SlotSystemElement> elements{get{return m_elements;}}
		IEnumerable<SlotSystemElement> m_elements;
		Dictionary<SlotSystemElement, SlotSystemPageElement> pageElementDict = new Dictionary<SlotSystemElement, SlotSystemPageElement>();
		public void AddPageElement(SlotSystemElement element, SlotSystemPageElement pageElement){
			pageElementDict.Add(element, pageElement);
		}
		public override SlotSystemPageElement GetPageElement(SlotSystemElement element){
			foreach(KeyValuePair<SlotSystemElement, SlotSystemPageElement> pair in pageElementDict){
				if(pair.Key == element) return pair.Value;
			}
			return null;
		}
	}
}
