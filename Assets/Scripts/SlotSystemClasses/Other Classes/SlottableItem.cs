using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlotSystem{
	public interface SlottableItem: IEquatable<SlottableItem>{
		int GetQuantity();
		void SetQuantity(int quantity);
		bool IsStackable();
	}
}