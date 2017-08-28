using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public interface IOnSSMSelectedCommand{
		void Execute(ISlotSystemManager selectedSSM);
	}
	public class OnInventorySystemSSMSelectedCommand: IOnSSMSelectedCommand{
		ISlotSystemManager ssm;
		public OnInventorySystemSSMSelectedCommand(ISlotSystemManager ssm){
			this.ssm = ssm;
		}
		public void Execute(ISlotSystemManager selectedSSM){
			if(selectedSSM is IInventorySystemSSM){
				if(selectedSSM == this.ssm)
					ssm.MakeSelectable();
				else
					ssm.MakeUnselectable();
			}else
				ssm.Deactivate();
		}
	}
}
