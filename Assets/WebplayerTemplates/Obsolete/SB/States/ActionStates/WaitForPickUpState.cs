using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class WaitForPickUpState: SBActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SBActProcess wfpuProcess = new WaitForPickUpProcess(sb, sb.WaitForPickUpCoroutine);
			sb.SetAndRunActProcess(wfpuProcess);
		}
		public override void OnPointerUpMock(Slottable sb, PointerEventDataFake eventDataMock){
			sb.SetActState(Slottable.waitForNextTouchState);
		}
		public override void OnEndDragMock(Slottable sb, PointerEventDataFake eventDataMock){
			sb.Reset();
			sb.Focus();
		}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
		public override void OnDeselectedMock(Slottable slottable, PointerEventDataFake eventDataMock){}
		public override void OnPointerDownMock(Slottable slottable, PointerEventDataFake eventDataMock){}
	}
}