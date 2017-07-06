using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SGState: SSEState{
		protected SlotGroup sg{
			get{
				return (SlotGroup)sse;
			}
		}
	}
}
