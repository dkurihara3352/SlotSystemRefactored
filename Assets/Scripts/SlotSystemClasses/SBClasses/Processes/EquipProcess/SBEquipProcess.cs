using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SBEquipProcess: SBEqpProcess{
		public SBEquipProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
			sse = sb;
			this.coroutineFake = coroutineMock;
		}
	}
}