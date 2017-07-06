using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SGBowFilter: SGFilter{
		public void Filter(ref List<SlottableItem> items){
			List<SlottableItem> res = new List<SlottableItem>();
			foreach(SlottableItem item in items){
				if(item is BowInstance)
					res.Add(item);
			}
			items = res;
		}
	}
}
