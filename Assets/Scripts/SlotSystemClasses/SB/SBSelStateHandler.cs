using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SBSelStateHandler : SSESelStateHandler{
		ITransactionCache taCache;
		IHoverable hoverable;
		public  SBSelStateHandler(ISlottable sb){
			IHoverable hoverable = sb.GetHoverable();
			this.taCache = hoverable.GetTAC();
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
