using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SGSelStateHandler : SSESelStateHandler {
		ITransactionCache taCache;
		IHoverable hoverable;
		ISlotGroup sg;
		public SGSelStateHandler(ISlotGroup sg){
			this.taCache = sg.taCache;
			this.hoverable = sg;
			this.sg = sg;
		}
		public override void Activate(){
			if(taCache.IsCachedTAResultRevert(hoverable) == false)
				Focus();
			else
				Defocus();	
		}
		public override void Deselect(){
			Activate();
		}
		public override void InitializeStates(){
			Deactivate();
			sg.WaitForAction();
		}
	}
}
