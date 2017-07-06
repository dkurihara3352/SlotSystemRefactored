using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SBGreyinProcess: SBSelProcess{
		public SBGreyinProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
			sse = sb;
			this.coroutineFake = coroutineMock;
		}
	}
}
