using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public class PickedUpState: SBActState{
		public override void EnterState(StateHandler sh){
			base.EnterState(sh);
			sb.ssm.SetPickedSB(sb);
			sb.ssm.SetActState(SlotSystemManager.ssmProbingState);
			DraggedIcon di = new DraggedIcon(sb);
			sb.ssm.SetDIcon1(di);
			sb.ssm.CreateTransactionResultsV2();
			sb.OnHoverEnterMock();
			sb.ssm.UpdateTransaction();
			SBActProcess pickedUpProcess = new SBPickedUpProcess(sb, sb.PickUpCoroutine);
			sb.SetAndRunActProcess(pickedUpProcess);
		}
		public override void OnDeselectedMock(Slottable sb, PointerEventDataFake eventDataMock){
			sb.Reset();
			sb.Focus();
		}
		public override void OnPointerUpMock(Slottable sb, PointerEventDataFake eventDataMock){
			if(sb.ssm.hovered == (SlotSystemElement)sb && sb.isStackable)
				sb.SetActState(Slottable.waitForNextTouchState);
			else
				sb.ExecuteTransaction();
		}
		public override void OnEndDragMock(Slottable sb, PointerEventDataFake eventDataMock){
			sb.ExecuteTransaction();
		}
		/*	undef	*/
			public override void ExitState(StateHandler sh){
				base.ExitState(sh);
			}
			public override void OnPointerDownMock(Slottable sb, PointerEventDataFake eventDataMock){}
	}
}