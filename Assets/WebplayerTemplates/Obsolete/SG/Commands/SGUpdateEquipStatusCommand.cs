using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SGUpdateEquipStatusCommand: SlotGroupCommand{
		public void Execute(SlotGroup sg){
			sg.ssm.UpdateEquipStatesOnAll();
		}
	}
}
