using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlotSystem{
	public interface SlottableItem: IEquatable<SlottableItem>, IComparable<SlottableItem>, IComparable{
		int quantity{get;}
		bool isStackable{get;}
	}
}