using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SBProcess: SSEProcess, ISBProcess{
		public ISlottable sb{
			get{return (ISlottable)sse;}
			set{}
		}
	}
	public interface ISBProcess: ISSEProcess{
		ISlottable sb{get;set;}
	}
		public interface ISBSelProcess: ISSEProcess{}
			public class SBGreyinProcess: SBProcess, ISBSelProcess{
				public SBGreyinProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBGreyoutProcess: SBProcess, ISBSelProcess{
				public SBGreyoutProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBHighlightProcess: SBProcess, ISBSelProcess{
				public SBHighlightProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBDehighlightProcess: SBProcess, ISBSelProcess{
				public SBDehighlightProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
		public interface ISBActProcess: ISBProcess{}
			public class WaitForPickUpProcess: SBProcess, ISBActProcess{
				public WaitForPickUpProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					sb.PickUp();
				}
			}
			public class WaitForPointerUpProcess: SBProcess, ISBActProcess{
				public WaitForPointerUpProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
				public override void Expire(){
					base.Expire();
					sb.SetSelState(SlotSystemElement.defocusedState);
				}
			}
			public class WaitForNextTouchProcess: SBProcess, ISBActProcess{
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
			public class SBPickedUpProcess: SBProcess, ISBActProcess{
				public SBPickedUpProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBMoveWithinProcess: SBProcess, ISBActProcess{
				public SBMoveWithinProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBAddProcess: SBProcess, ISBActProcess{
				public SBAddProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBRemoveProcess: SBProcess, ISBActProcess{
				public SBRemoveProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
		public interface ISBEqpProcess: ISBProcess{}
			public class SBEquipProcess: SBProcess, ISBEqpProcess{
				public SBEquipProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBUnequipProcess: SBProcess, ISBEqpProcess{
				public SBUnequipProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
		public interface ISBMrkProcess: ISBProcess{}
			public class SBMarkProcess: SBProcess, ISBMrkProcess{
				public SBMarkProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
			public class SBUnmarkProcess: SBProcess, ISBMrkProcess{
				public SBUnmarkProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
					sse = sb;
					this.coroutineFake = coroutineMock;
				}
			}
}