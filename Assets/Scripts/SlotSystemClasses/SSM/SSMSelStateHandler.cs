using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SSMSelStateHandler : SSESelStateHandler {
		ISlotSystemManager ssm;
		ITransactionManager tam;
		public SSMSelStateHandler(ISlotSystemManager ssm){
			this.ssm = ssm;
			this.tam = ssm.tam;
		}
		public override void InitializeStates(){
			Deactivate();
			tam.WaitForAction();
		}
		public override void Activate(){
			ssm.UpdateEquipInvAndAllSBsEquipState();
			Focus();
		}
	}
}

