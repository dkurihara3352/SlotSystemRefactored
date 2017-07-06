using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class WaitForPickUpProcess: SBActProcess{
		public WaitForPickUpProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
			sse = sb;
			this.coroutineFake = coroutineMock;
		}
		public override void Expire(){
			base.Expire();
			sb.PickUp();
		}
	}
}
