using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface SGSorter{
		void OrderSBsWithRetainedSize(ref List<ISlottable> sbs);
		void TrimAndOrderSBs(ref List<ISlottable> sbs);
	}
		public class　SGItemIDSorter: SGSorter{
			public void OrderSBsWithRetainedSize(ref List<ISlottable> sbs){
				int origCount = sbs.Count;
				List<ISlottable> trimmed = sbs;
				this.TrimAndOrderSBs(ref trimmed);
				while(trimmed.Count < origCount){
					trimmed.Add(null);
				}
				sbs = trimmed;
			}
			public void TrimAndOrderSBs(ref List<ISlottable> sbs){
				List<ISlottable> trimmed = new List<ISlottable>();
				foreach(ISlottable sb in sbs){
					if(sb != null)
						trimmed.Add(sb);
				}
				trimmed.Sort();
				sbs = trimmed;
			}
		}
		public class SGInverseItemIDSorter: SGSorter{
			public void OrderSBsWithRetainedSize(ref List<ISlottable> sbs){
				int origCount = sbs.Count;
				List<ISlottable> trimmed = sbs;
				this.TrimAndOrderSBs(ref trimmed);
				while(trimmed.Count < origCount){
					trimmed.Add(null);
				}
				sbs = trimmed;
			}
			public void TrimAndOrderSBs(ref List<ISlottable> sbs){
				List<ISlottable> trimmed = new List<ISlottable>();
				foreach(ISlottable sb in sbs){
					if(sb != null)
						trimmed.Add(sb);
				}
				trimmed.Sort();
				trimmed.Reverse();
				sbs = trimmed;
			}
		}
		public class SGAcquisitionOrderSorter: SGSorter{
			public void OrderSBsWithRetainedSize(ref List<ISlottable> sbs){
				int origCount = sbs.Count;
				List<ISlottable> trimmed = sbs;
				this.TrimAndOrderSBs(ref trimmed);
				while(trimmed.Count < origCount){
					trimmed.Add(null);
				}
				sbs = trimmed;
			}
			public void TrimAndOrderSBs(ref List<ISlottable> sbs){
				List<ISlottable> trimmed = new List<ISlottable>();
				foreach(ISlottable sb in sbs){
					if(sb != null)
						trimmed.Add(sb);
				}
				List<ISlottable> temp = new List<ISlottable>();
				ISlottable addedMax = null;
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
					ISlottable added = trimmed[indexAtMin];
					temp.Add(added);
					addedMax = added;
				}
				sbs = temp;
			}
		}
}
