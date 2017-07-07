using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SGDehighlightProcess: SGSelProcess{
		public SGDehighlightProcess(SlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
			sse = sg;
			this.coroutineFake = coroutineMock;
		}
	}
}
