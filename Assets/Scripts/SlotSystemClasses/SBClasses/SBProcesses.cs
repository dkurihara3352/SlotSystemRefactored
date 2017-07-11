using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SBProcess: AbsSSEProcess{
		public ISlottable sb{
			get{return (ISlottable)sse;}
		}
	}
		public abstract class SBSelProcess: SBProcess{}
			public class SBGreyinProcess: SBSelProcess{
				public SBGreyinProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBGreyoutProcess: SBSelProcess{
				public SBGreyoutProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBHighlightProcess: SBSelProcess{
				public SBHighlightProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBDehighlightProcess: SBSelProcess{
				public SBDehighlightProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
		public abstract class SBActProcess: SBProcess{}
			public class WaitForPickUpProcess: SBActProcess{
				public WaitForPickUpProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					sb.PickUp();
				}
			}
			public class WaitForPointerUpProcess: SBActProcess{
				public WaitForPointerUpProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					sb.SetSelState(Slottable.sbDefocusedState);
				}
			}
			public class WaitForNextTouchProcess: SBActProcess{
				public WaitForNextTouchProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
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
				public SBPickedUpProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBMoveWithinProcess: SBActProcess{
				public SBMoveWithinProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBAddProcess: SBActProcess{
				public SBAddProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBRemoveProcess: SBActProcess{
				public SBRemoveProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
		public abstract class SBEqpProcess: SBProcess{}
			public class SBEquipProcess: SBEqpProcess{
				public SBEquipProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBUnequipProcess: SBEqpProcess{
				public SBUnequipProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
		public abstract class SBMrkProcess: SBProcess{}
			public class SBMarkProcess: SBMrkProcess{
				public SBMarkProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBUnmarkProcess: SBMrkProcess{
				public SBUnmarkProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
}