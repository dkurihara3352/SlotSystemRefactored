using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SSEGreyinProcess: SSESelProcess{
		public SSEGreyinProcess(SlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
			this.sse = sse;
			this.coroutineFake = coroutineMock;
		}
	}
}