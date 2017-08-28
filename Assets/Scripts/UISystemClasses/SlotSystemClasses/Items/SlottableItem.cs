using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UISystem{
	public interface ISlottableItem: IEquatable<ISlottableItem>{
		int Quantity();
		void SetQuantity(int quantity);
		bool IsStackable();
		int ItemID();
	}
}