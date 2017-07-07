using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class WaitForPointerUpProcess: SBActProcess{
		public WaitForPointerUpProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
			sse = sb;
			this.coroutineFake = coroutineMock;
		}
		public override void Expire(){
			base.Expire();
			sb.SetSelState(Slottable.sbDefocusedState);
		}
	}
}
