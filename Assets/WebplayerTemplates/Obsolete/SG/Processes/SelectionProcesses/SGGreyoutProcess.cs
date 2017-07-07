using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SGGreyoutProcess: SGSelProcess{
		public SGGreyoutProcess(SlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
			sse = sg;
			this.coroutineFake = coroutineMock;
		}
	}
}
