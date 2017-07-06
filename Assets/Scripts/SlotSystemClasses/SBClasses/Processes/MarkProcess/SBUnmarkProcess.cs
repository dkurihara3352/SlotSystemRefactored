using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SBUnmarkProcess: SBMrkProcess{
		public SBUnmarkProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
			sse = sb;
			this.coroutineFake = coroutineMock;
		}
	}
}
