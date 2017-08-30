using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class SSMSelStateHandler : UISelStateHandler {
		ISlotSystemManager ssm;
		public SSMSelStateHandler(SSMSelCoroutineRepo repo, ISlotSystemManager ssm): base(repo){
			this.ssm = ssm;
		}
		public override void Activate(){
			base.Activate();
			ssm.InitializeSlotSystemOnActivate();
		}
	}
}
