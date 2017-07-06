using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SGAcquisitionOrderSorter: SGSorter{
		public void OrderSBsWithRetainedSize(ref List<Slottable> sbs){
			int origCount = sbs.Count;
			List<Slottable> trimmed = sbs;
			this.TrimAndOrderSBs(ref trimmed);
			while(trimmed.Count < origCount){
				trimmed.Add(null);
			}
			sbs = trimmed;
		}
		public void TrimAndOrderSBs(ref List<Slottable> sbs){
			List<Slottable> trimmed = new List<Slottable>();
			foreach(Slottable sb in sbs){
				if(sb != null)
					trimmed.Add(sb);
			}
			List<Slottable> temp = new List<Slottable>();
			Slottable addedMax = null;
			while(temp.Count < trimmed.Count){
				int indexAtMin = -1;
				int addedAO;
				if(addedMax == null) addedAO = -1;
				else addedAO = ((InventoryItemInstance)addedMax.item).AcquisitionOrder;

				for(int i = 0; i < trimmed.Count; i++){
					InventoryItemInstance inst = (InventoryItemInstance)trimmed[i].item;
					if(inst.AcquisitionOrder > addedAO){
						if(indexAtMin == -1 || inst.AcquisitionOrder < ((InventoryItemInstance)trimmed[indexAtMin].item).AcquisitionOrder){
							indexAtMin = i;
						}
					}
				}
				Slottable added = trimmed[indexAtMin];
				temp.Add(added);
				addedMax = added;
			}
			sbs = temp;
		}
	}
}
