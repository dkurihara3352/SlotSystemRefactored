using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class SlotSelStateHandler : UISelStateHandler {
		ISlot slot;
		public SlotSelStateHandler(ISlot slot){
			this.slot = slot;
			SetSelCoroutineRepo(new SlotSelCoroutineRepo());
		}
		public override void MakeSelectable(){
			base.MakeSelectable();
			slot.MakeSBSelectable();
		}
		public override void MakeUnselectable(){
			base.MakeUnselectable();
			slot.MakeSBUnselectable();
		}
		public override void Select(){
			base.Select();
			slot.SelectSB();
		}
	}
}
