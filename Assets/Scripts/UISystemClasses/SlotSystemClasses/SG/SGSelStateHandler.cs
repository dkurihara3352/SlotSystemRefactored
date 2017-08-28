using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class SGSelStateHandler : UISelStateHandler {
		ITransactionCache taCache;
		IHoverable hoverable;
		public SGSelStateHandler(ITransactionCache taCache, IHoverable hoverable){
			this.taCache = taCache;
			this.hoverable = hoverable;
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
