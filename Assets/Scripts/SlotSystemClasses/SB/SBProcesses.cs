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
					sb.Defocus();
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
						sb.Refresh();
						sb.Focus();
					}else{
						sb.ExecuteTransaction();
					}
				}
			}
			public class PickUpProcess: SBProcess, ISBActProcess{
				public PickUpProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutineMock){
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
	public class SBCoroutineFactory: ISBCoroutineFactory{
		ISlottable sb;
		public SBCoroutineFactory(ISlottable sb){this.sb = sb;}

		public System.Func<IEnumeratorFake> MakeWaitForPointerUpCoroutine(){
			return DefaultWaitForPointerUpCoroutine;
			}
			IEnumeratorFake DefaultWaitForPointerUpCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeWaitForPickUpCoroutine(){
			return DefaultWaitForPickUpCoroutine;
			}
			IEnumeratorFake DefaultWaitForPickUpCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakePickUpCoroutine(){
			return DefaultPickUpCoroutine;
			}
			IEnumeratorFake DefaultPickUpCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeWaitForNextTouchCoroutine(){
			return DefaultWaitForNextTouchCoroutine;
			}
			IEnumeratorFake DefaultWaitForNextTouchCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeRemoveCoroutine(){
			return DefaultRemoveCoroutine;
			}
			IEnumeratorFake DefaultRemoveCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeAddCoroutine(){
			return DefaultAddCoroutine;
			}
			IEnumeratorFake DefaultAddCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeMoveWithinCoroutine(){
			return DefaultMoveWithinCoroutine;
			}
			IEnumeratorFake DefaultMoveWithinCoroutine(){return null;}

		public System.Func<IEnumeratorFake> MakeEquipCoroutine(){
			return DefaultEquipCoroutine;
			}
			IEnumeratorFake DefaultEquipCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeUnequipCoroutine(){
			return DefaultUnequipCoroutine;
			}
			IEnumeratorFake DefaultUnequipCoroutine(){return null;}

		public System.Func<IEnumeratorFake> MakeMarkCoroutine(){
			return DefaultMarkCoroutine;
			}
			IEnumeratorFake DefaultMarkCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeUnmarkCoroutine(){
			return DefaultUnmarkCoroutine;
			}
			IEnumeratorFake DefaultUnmarkCoroutine(){return null;}
	}
	public interface ISBCoroutineFactory{
		System.Func<IEnumeratorFake> MakeWaitForPointerUpCoroutine();
		System.Func<IEnumeratorFake> MakeWaitForPickUpCoroutine();
		System.Func<IEnumeratorFake> MakePickUpCoroutine();
		System.Func<IEnumeratorFake> MakeWaitForNextTouchCoroutine();
		System.Func<IEnumeratorFake> MakeRemoveCoroutine();
		System.Func<IEnumeratorFake> MakeAddCoroutine();
		System.Func<IEnumeratorFake> MakeMoveWithinCoroutine();

		System.Func<IEnumeratorFake> MakeEquipCoroutine();
		System.Func<IEnumeratorFake> MakeUnequipCoroutine();

		System.Func<IEnumeratorFake> MakeMarkCoroutine();
		System.Func<IEnumeratorFake> MakeUnmarkCoroutine();		
	}
}