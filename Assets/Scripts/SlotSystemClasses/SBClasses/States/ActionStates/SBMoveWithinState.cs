using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class SBMoveWithinState: SBActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SBMoveWithinProcess process = new SBMoveWithinProcess(sb, sb.MoveWithinCoroutine);
			sb.SetAndRunActProcess(process);
		}
		/*	*/
			public override void ExitState(StateHandler sh){
				base.ExitState(sh);
			}
			public override void OnPointerDownMock(Slottable sb, PointerEventDataFake eventDataMock){}
			public override void OnPointerUpMock(Slottable sb, PointerEventDataFake eventDataMock){}
			public override void OnDeselectedMock(Slottable sb, PointerEventDataFake eventDataMock){}
			public override void OnEndDragMock(Slottable sb, PointerEventDataFake eventDataMock){}
	}
}