using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface ISGSorter{
		List<ISlot> OrderedSBsWithoutResize(List<ISlot> source);
		List<ISlot> OrderedAndTrimmedSBs(List<ISlot> soruce);
	}
	public abstract class SGSorter: ISGSorter{
		public virtual List<ISlot> OrderedSBsWithoutResize(List<ISlot> source){
			List<ISlot> result = source;
			int origCount = source.Count;
			result = OrderedAndTrimmedSBs(result);
			while(result.Count < origCount){
				result.Add(null);
			}
			return result;
		}
		public abstract List<ISlot> OrderedAndTrimmedSBs(List<ISlot> source);
		public List<ISlot> TrimmedSBsOrderedByCompairer(List<ISlot> source, Comparer<ISlot> compairer){
			List<ISlot> result = new List<ISlot>();
			foreach(ISlot sb in source){
				if(sb != null)
					result.Add(sb);
			}
			result.Sort(compairer);
			return result;
		}
	}
		public class　SGItemIDSorter: SGSorter{
			public override List<ISlot> OrderedAndTrimmedSBs(List<ISlot> source){
				return TrimmedSBsOrderedByCompairer(source, new ItemIDOrderComparer());
			}
		}
		public class SGInverseItemIDSorter: SGSorter{
			public override List<ISlot> OrderedAndTrimmedSBs(List<ISlot> source){
				List<ISlot> result = new SGItemIDSorter().OrderedAndTrimmedSBs(source);
				result.Reverse();
				return result;
			}
		}
		public class SGAcquisitionOrderSorter: SGSorter{
			public override List<ISlot> OrderedAndTrimmedSBs(List<ISlot> source){
				return TrimmedSBsOrderedByCompairer(source, new AcquisitionOrderComparer());
			}
		}
		public class AcquisitionOrderComparer: Comparer<ISlot>{
			public override int Compare(ISlot x, ISlot y){
				Debug.Assert((x is IInventoryItemSlot));
				Debug.Assert((y is IInventoryItemSlot));
				IInventoryItemSlot invSBX = (IInventoryItemSlot)x;
				IInventoryItemSlot invSBY = (IInventoryItemSlot)y;
				int xAcqOrder = invSBX.AcquisitionOrder();
				int yAcqOrder = invSBY.AcquisitionOrder();
				if(xAcqOrder.Equals(yAcqOrder)) 
					return 0;
				return xAcqOrder.CompareTo(yAcqOrder);
			}
		}
		public class ItemIDOrderComparer: Comparer<ISlot>{
			public override int Compare(ISlot x, ISlot y){
				int xItemID = x.ItemID();
				int yItemID = y.ItemID();

				if(xItemID.Equals(yItemID))
					return new AcquisitionOrderComparer().Compare(x, y);
				return xItemID.CompareTo(yItemID);
			}
		}
}
