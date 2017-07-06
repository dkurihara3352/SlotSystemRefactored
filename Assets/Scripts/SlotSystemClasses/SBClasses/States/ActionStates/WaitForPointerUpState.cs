using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class WaitForPointerUpState: SBActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SBActProcess wfPtuProcess = new WaitForPointerUpProcess(sb, sb.WaitForPointerUpCoroutine);
			sb.SetAndRunActProcess(wfPtuProcess);
		}
		public override void OnPointerUpMock(Slottable sb, PointerEventDataFake eventDataMock){
			sb.Tap();
			sb.Reset();
			sb.Defocus();
		}
		public override void OnEndDragMock(Slottable sb, PointerEventDataFake eventDataMock){
			sb.Reset();
			sb.Defocus();
		}
		public override void OnDeselectedMock(Slottable sb, PointerEventDataFake eventDataMock){}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
		public override void OnPointerDownMock(Slottable sb, PointerEventDataFake eventDataMock){}
	}
}
