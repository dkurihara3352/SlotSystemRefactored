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
				trimmed.Sort(new ItemIDOrderComparer());
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
				trimmed.Sort(new ItemIDOrderComparer());
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
				trimmed.Sort(new AcquisitionOrderComparer());
				sbs = trimmed;
			}
		}
		public class AcquisitionOrderComparer: Comparer<ISlottable>{
			public override int Compare(ISlottable x, ISlottable y){
				if(x.item.AcquisitionOrder.Equals(y.item.AcquisitionOrder)) return 0;
					return x.item.AcquisitionOrder.CompareTo(y.item.AcquisitionOrder);
			}
		}
		public class ItemIDOrderComparer: Comparer<ISlottable>{
			public override int Compare(ISlottable x, ISlottable y){
				if(x.item.Item.ItemID.Equals(y.item.Item.ItemID))
					return new AcquisitionOrderComparer().Compare(x, y);
				return x.item.Item.ItemID.CompareTo(y.item.Item.ItemID);
			}
		}
}
