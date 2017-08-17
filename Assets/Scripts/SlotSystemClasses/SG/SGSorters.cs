﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface ISGSorter{
		List<ISlottable> OrderedSBsWithoutResize(List<ISlottable> source);
		List<ISlottable> OrderedAndTrimmedSBs(List<ISlottable> soruce);
	}
	public abstract class SGSorter: ISGSorter{
		public virtual List<ISlottable> OrderedSBsWithoutResize(List<ISlottable> source){
			List<ISlottable> result = source;
			int origCount = source.Count;
			result = OrderedAndTrimmedSBs(result);
			while(result.Count < origCount){
				result.Add(null);
			}
			return result;
		}
		public abstract List<ISlottable> OrderedAndTrimmedSBs(List<ISlottable> source);
		public List<ISlottable> TrimmedSBsOrderedByCompairer(List<ISlottable> source, Comparer<ISlottable> compairer){
			List<ISlottable> result = new List<ISlottable>();
			foreach(ISlottable sb in source){
				if(sb != null)
					result.Add(sb);
			}
			result.Sort(compairer);
			return result;
		}
	}
		public class　SGItemIDSorter: SGSorter{
			public override List<ISlottable> OrderedAndTrimmedSBs(List<ISlottable> source){
				return TrimmedSBsOrderedByCompairer(source, new ItemIDOrderComparer());
			}
		}
		public class SGInverseItemIDSorter: SGSorter{
			public override List<ISlottable> OrderedAndTrimmedSBs(List<ISlottable> source){
				List<ISlottable> result = new SGItemIDSorter().OrderedAndTrimmedSBs(source);
				result.Reverse();
				return result;
			}
		}
		public class SGAcquisitionOrderSorter: SGSorter{
			public override List<ISlottable> OrderedAndTrimmedSBs(List<ISlottable> source){
				return TrimmedSBsOrderedByCompairer(source, new AcquisitionOrderComparer());
			}
		}
		public class AcquisitionOrderComparer: Comparer<ISlottable>{
			public override int Compare(ISlottable x, ISlottable y){
				if(x.GetItem().AcquisitionOrder.Equals(y.GetItem().AcquisitionOrder)) return 0;
					return x.GetItem().AcquisitionOrder.CompareTo(y.GetItem().AcquisitionOrder);
			}
		}
		public class ItemIDOrderComparer: Comparer<ISlottable>{
			public override int Compare(ISlottable x, ISlottable y){
				if(x.GetItem().Item.ItemID.Equals(y.GetItem().Item.ItemID))
					return new AcquisitionOrderComparer().Compare(x, y);
				return x.GetItem().Item.ItemID.CompareTo(y.GetItem().Item.ItemID);
			}
		}
}
