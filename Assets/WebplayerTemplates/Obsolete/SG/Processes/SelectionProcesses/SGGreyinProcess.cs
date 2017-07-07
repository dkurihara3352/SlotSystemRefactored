using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SGGreyinProcess: SGSelProcess{
		public SGGreyinProcess(SlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
			sse = sg;
			this.coroutineFake = coroutineMock;
		}
	}
}
