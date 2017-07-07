using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SBProcess: AbsSSEProcess{
		public Slottable sb{
			get{return (Slottable)sse;}
		}
	}
		public abstract class SBSelProcess: SBProcess{}
			public class SBGreyinProcess: SBSelProcess{
				public SBGreyinProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBGreyoutProcess: SBSelProcess{
				public SBGreyoutProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBHighlightProcess: SBSelProcess{
				public SBHighlightProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBDehighlightProcess: SBSelProcess{
				public SBDehighlightProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
		public abstract class SBActProcess: SBProcess{}
			public class WaitForPickUpProcess: SBActProcess{
				public WaitForPickUpProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					sb.PickUp();
				}
			}
			public class WaitForPointerUpProcess: SBActProcess{
				public WaitForPointerUpProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					sb.SetSelState(Slottable.sbDefocusedState);
				}
			}
			public class WaitForNextTouchProcess: SBActProcess{
				public WaitForNextTouchProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					if(!sb.isPickedUp){
						sb.Tap();
						sb.Reset();
						sb.Focus();
					}else{
						sb.ExecuteTransaction();
					}
				}
			}
			public class SBPickedUpProcess: SBActProcess{
				public SBPickedUpProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBMoveWithinProcess: SBActProcess{
				public SBMoveWithinProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBAddProcess: SBActProcess{
				public SBAddProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBRemoveProcess: SBActProcess{
				public SBRemoveProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
		public abstract class SBEqpProcess: SBProcess{}
			public class SBEquipProcess: SBEqpProcess{
				public SBEquipProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBUnequipProcess: SBEqpProcess{
				public SBUnequipProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
		public abstract class SBMrkProcess: SBProcess{}
			public class SBMarkProcess: SBMrkProcess{
				public SBMarkProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBUnmarkProcess: SBMrkProcess{
				public SBUnmarkProcess(Slottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
}