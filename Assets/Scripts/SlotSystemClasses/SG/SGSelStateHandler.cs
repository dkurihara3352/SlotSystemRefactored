using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SGSelStateHandler : SSESelStateHandler {
		ITransactionCache taCache;
		IHoverable hoverable;
		public SGSelStateHandler(ISlotGroup sg){
			this.taCache = sg.taCache;
			this.hoverable = sg;
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
	}
}
