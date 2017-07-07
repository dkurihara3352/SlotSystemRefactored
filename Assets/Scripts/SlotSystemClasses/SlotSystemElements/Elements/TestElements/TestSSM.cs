using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TestSSM : SlotSystemManager {
		public Dictionary<SlotSystemElement, SlotSystemElement> parentDict = new Dictionary<SlotSystemElement, SlotSystemElement>();
		public void AddParentChild(Slottable sb, SlotGroup sg){
			parentDict.Add(sb, sg);
		}

		public override SlotSystemElement FindParent(SlotSystemElement ele){
			foreach(KeyValuePair<SlotSystemElement, SlotSystemElement> pair in parentDict){
				if(pair.Key == ele)
					return pair.Value;
			}
			return null;
		}
	}
}
