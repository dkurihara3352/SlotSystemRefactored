using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface SlottableCommand{
		void Execute(Slottable sb);
	}
	public class SBTapCommand: SlottableCommand{
		public void Execute(Slottable sb){

		}
	}
}
