using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface SlotGroupCommand{
		void Execute(SlotGroup Sg);
	}
}
