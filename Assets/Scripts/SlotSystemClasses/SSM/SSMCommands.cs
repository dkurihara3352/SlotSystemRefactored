using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public interface ISSMCommand{
		void Execute();
	}
	public abstract class SSMCommand: ISSMCommand{
		protected SlotSystemManager ssm;
		protected IEquippedProvider equippedProvider;
		protected IAllElementsProvider allElesProvider;
		public SSMCommand(SlotSystemManager ssm){
			this.ssm = ssm;
			this.equippedProvider = ssm.GetEquippedProvider();
			this.allElesProvider = ssm.GetAllElementsProvider();
		}
		public abstract void Execute();
	}
	public class UpdateEquipInvAndAllSBsEquipStateCommand: SSMCommand{
		public UpdateEquipInvAndAllSBsEquipStateCommand(SlotSystemManager ssm): base(ssm){
		}
		public override void Execute(){
			ssm.RemoveFromEquipInv();
			ssm.AddToEquipInv();
			ssm.UpdateAllItemsEquipStatusInPoolInv();
			ssm.UpdateAllSBsEquipState();
		}
	}
}
