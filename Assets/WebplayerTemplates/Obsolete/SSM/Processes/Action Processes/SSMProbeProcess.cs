using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SSMProbeProcess: SSMActProcess{
		public SSMProbeProcess(SlotSystemManager ssm, System.Func<IEnumeratorFake> coroutineMock){
			this.sse = ssm;
			this.coroutineFake = coroutineMock;
		}
	}
}
