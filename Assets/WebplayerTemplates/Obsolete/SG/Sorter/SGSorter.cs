using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface SGSorter{
		void OrderSBsWithRetainedSize(ref List<Slottable> sbs);
		void TrimAndOrderSBs(ref List<Slottable> sbs);
	}	
}
