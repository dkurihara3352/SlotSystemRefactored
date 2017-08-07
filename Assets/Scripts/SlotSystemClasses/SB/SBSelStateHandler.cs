using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SBSelStateHandler : SSESelStateHandler{
		ITransactionCache taCache;
		IHoverable hoverable;
		ISlottable sb;
		public  SBSelStateHandler(ISlottable sb){
			this.taCache = sb.taCache;
			this.hoverable = sb.hoverable;
			this.sb = sb;
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
			sb.WaitForAction();
			sb.ClearCurEqpState();
			sb.Unmark();
		}
	}
}
