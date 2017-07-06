using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SSEDehighlightProcess: SSESelProcess{
		public SSEDehighlightProcess(SlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
			this.sse = sse;
			this.coroutineFake = coroutineMock;
		}
	}
}
