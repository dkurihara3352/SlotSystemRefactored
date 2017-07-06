using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class WaitForNextTouchState: SBActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SBActProcess wfntProcess = new WaitForNextTouchProcess(sb, sb.WaitForNextTouchCoroutine);
			sb.SetAndRunActProcess(wfntProcess);
		}
		public override void OnPointerDownMock(Slottable sb, PointerEventDataFake eventDataMock){
			if(!sb.isPickedUp)
				sb.PickUp();
			else{
				sb.SetActState(Slottable.pickedUpState);
				sb.Increment();
			}
		}
		public override void OnDeselectedMock(Slottable sb, PointerEventDataFake eventDataMock){
			sb.Reset();
			sb.Focus();
		}
		/*	undef	*/
			public override void ExitState(StateHandler sh){
				base.ExitState(sh);
			}
			public override void OnPointerUpMock(Slottable sb, PointerEventDataFake eventDataMock){}
			public override void OnEndDragMock(Slottable sb, PointerEventDataFake eventDataMock){}
	}
}