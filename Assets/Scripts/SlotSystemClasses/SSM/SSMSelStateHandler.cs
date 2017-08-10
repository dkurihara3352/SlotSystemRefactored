using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SSMSelStateHandler : SSESelStateHandler {
		ISlotSystemManager ssm;
		public SSMSelStateHandler(ISlotSystemManager ssm){
			this.ssm = ssm;
		}
		public override void Activate(){
			ssm.UpdateEquipInvAndAllSBsEquipState();
			Focus();
		}
	}
}

