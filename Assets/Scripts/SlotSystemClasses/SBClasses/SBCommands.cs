using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface SlottableCommand{
		void Execute(ISlottable sb);
	}
	public class SBTapCommand: SlottableCommand{
		public void Execute(ISlottable sb){

		}
	}
}
