using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class　SGItemIDSorter: SGSorter{
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
			trimmed.Sort();
			sbs = trimmed;
		}
	}
}
