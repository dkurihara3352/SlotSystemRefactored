using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SBUnequipProcess: SBEqpProcess{
		public SBUnequipProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
			sse = sb;
			this.coroutineFake = coroutineMock;
		}
	}
}