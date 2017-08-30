using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class SGSelStateHandler : UISelStateHandler {
		ISlotGroup slotGroup;
		public SGSelStateHandler(SGSelCoroutineRepo repo, ISlotGroup sg): base(repo){
			this.slotGroup = sg;
		}
		public override void Activate(){
			base.Activate();
			slotGroup.InitializeOnSlotSystemActivate();
		}
	}
}
