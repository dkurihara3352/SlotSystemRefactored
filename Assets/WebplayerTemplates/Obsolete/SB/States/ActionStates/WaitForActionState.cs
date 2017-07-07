using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class WaitForActionState: SBActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			SBActProcess process = null;
			sb.SetAndRunActProcess(process);
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
		public override void OnPointerDownMock(Slottable sb, PointerEventDataFake eventDataMock){
			if(sb.isFocused){
				sb.SetSelState(Slottable.sbSelectedState);
				sb.SetActState(Slottable.waitForPickUpState);
			}
			else
				sb.SetActState(Slottable.waitForPointerUpState);
		}
		public override void OnDeselectedMock(Slottable sb, PointerEventDataFake eventDataMock){}
		public override void ExitState(StateHandler sh){
			base.ExitState(sh);
		}
	}
}