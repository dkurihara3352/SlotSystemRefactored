using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SGTransactionProcess: SGActProcess{
		public SGTransactionProcess(SlotGroup sg, System.Func<IEnumeratorFake> coroutineMock){
			sse = sg;
			this.coroutineFake = coroutineMock;
		}
		public override void Expire(){
			base.Expire();
			sg.ssm.AcceptSGTAComp(sg);
		}
	}
}
