using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface SGFilter{
		void Filter(ref List<IInventoryItemInstance> items);
	}
		public class SGNullFilter: SGFilter{
			public void Filter(ref List<IInventoryItemInstance> items){}
		}
		public class SGBowFilter: SGFilter{
			public void Filter(ref List<IInventoryItemInstance> items){
				List<IInventoryItemInstance> res = new List<IInventoryItemInstance>();
				foreach(IInventoryItemInstance item in items){
					if(item is BowInstance)
						res.Add(item);
				}
				items = res;
			}
		}
		public class SGWearFilter: SGFilter{
			public void Filter(ref List<IInventoryItemInstance> items){
				List<IInventoryItemInstance> res = new List<IInventoryItemInstance>();
				foreach(IInventoryItemInstance item in items){
					if(item is WearInstance)
						res.Add(item);
				}
				items = res;
			}
		}
		public class SGCGearsFilter: SGFilter{
			public void Filter(ref List<IInventoryItemInstance> items){
				List<IInventoryItemInstance> res = new List<IInventoryItemInstance>();
				foreach(IInventoryItemInstance item in items){
					if(item is CarriedGearInstance)
						res.Add(item);
				}
				items = res;
			}
		}
		public class SGPartsFilter: SGFilter{
			public void Filter(ref List<IInventoryItemInstance> items){
				List<IInventoryItemInstance> res = new List<IInventoryItemInstance>();
				foreach(IInventoryItemInstance item in items){
					if(item is PartsInstance)
						res.Add(item);
				}
				items = res;
			}
		}
}