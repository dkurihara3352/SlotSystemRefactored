using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class SBSelStateHandler : UISelStateHandler{
		ITransactionCache taCache;
		IHoverable hoverable;
		public  SBSelStateHandler(ISlottable sb){
			taCache = sb.GetTAC();
			hoverable = sb.GetHoverable();
		}
		public override void Activate(){
			if(taCache.IsCachedTAResultRevert(hoverable) == false)
				MakeSelectable();
			else
				MakeUnselectable();	
		}
		public override void Deselect(){
			Activate();
		}
	}
}
