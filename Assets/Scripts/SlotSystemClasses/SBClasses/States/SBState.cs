using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SBState: SSEState{
		protected Slottable sb{
			get{
				return (Slottable)sse;
			}
		}
	}
}
