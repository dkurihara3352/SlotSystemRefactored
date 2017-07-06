using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SSMTransactionProcess: SSMActProcess{
		public SSMTransactionProcess(SlotSystemManager ssm){
			this.sse = ssm;
			this.coroutineFake = ssm.transactionCoroutine;
		}
		public override void Expire(){
			base.Expire();
			ssm.transaction.OnComplete();
		}
	}	
}
