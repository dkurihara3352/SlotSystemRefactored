using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SGSelStateHandler : SSESelStateHandler {
		ITransactionCache taCache;
		IHoverable hoverable;
		public SGSelStateHandler(ITransactionCache taCache, IHoverable hoverable){
			this.taCache = taCache;
			this.hoverable = hoverable;
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
