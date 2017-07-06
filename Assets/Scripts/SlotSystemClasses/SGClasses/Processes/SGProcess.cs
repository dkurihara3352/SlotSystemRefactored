using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SGProcess: AbsSSEProcess{
		public SlotGroup sg{
			get{return (SlotGroup)sse;}
		}
	}
}
