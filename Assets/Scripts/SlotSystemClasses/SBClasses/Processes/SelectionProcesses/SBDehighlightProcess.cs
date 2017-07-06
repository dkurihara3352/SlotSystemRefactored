using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SBDehighlightProcess: SBSelProcess{
		public SBDehighlightProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
			sse = sb;
			this.coroutineFake = coroutineMock;
		}
	}
}