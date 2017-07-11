using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TestSlotSystemPage: SlotSystemPage{
		public override IEnumerable<ISlotSystemElement> elements{get{return m_elements;}}
		IEnumerable<ISlotSystemElement> m_elements;
		Dictionary<ISlotSystemElement, ISlotSystemPageElement> pageElementDict = new Dictionary<ISlotSystemElement, ISlotSystemPageElement>();
		public void AddPageElement(ISlotSystemElement element, ISlotSystemPageElement pageElement){
			pageElementDict.Add(element, pageElement);
		}
		public override ISlotSystemPageElement GetPageElement(ISlotSystemElement element){
			foreach(KeyValuePair<ISlotSystemElement, ISlotSystemPageElement> pair in pageElementDict){
				if(pair.Key == element) return pair.Value;
			}
			return null;
		}
	}
}
