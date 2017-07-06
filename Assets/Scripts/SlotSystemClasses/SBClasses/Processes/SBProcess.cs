using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SBProcess: AbsSSEProcess{
		public Slottable sb{
			get{return (Slottable)sse;}
		}
	}
}
