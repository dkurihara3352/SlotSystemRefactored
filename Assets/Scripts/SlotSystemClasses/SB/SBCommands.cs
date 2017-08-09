using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface ISBCommand{
		void Execute(ISlottable sb);
	}
	public class SBTapCommand: ISBCommand{
		public void Execute(ISlottable sb){

		}
	}
}
