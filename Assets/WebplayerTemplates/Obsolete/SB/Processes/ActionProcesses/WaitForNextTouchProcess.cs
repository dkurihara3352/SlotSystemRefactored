using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class WaitForNextTouchProcess: SBActProcess{
		public WaitForNextTouchProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
			sse = sb;
			this.coroutineFake = coroutineMock;
		}
		public override void Expire(){
			base.Expire();
			if(!sb.isPickedUp){
				sb.Tap();
				sb.Reset();
				sb.Focus();
			}else{
				sb.ExecuteTransaction();
			}
		}
	}
}