using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface IFocusedSGProvider{
		List<ISlotGroup> focusedSGs{get;}
	}
}
