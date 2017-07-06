using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SGHighlightProcess: SGSelProcess{
		public SGHighlightProcess(SlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
			sse = sg;
			this.coroutineFake = coroutineMock;
		}
	}
}
