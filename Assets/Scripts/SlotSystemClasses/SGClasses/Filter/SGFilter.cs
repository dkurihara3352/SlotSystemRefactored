using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface SGFilter{
		void Filter(ref List<SlottableItem> items);
	}
}
