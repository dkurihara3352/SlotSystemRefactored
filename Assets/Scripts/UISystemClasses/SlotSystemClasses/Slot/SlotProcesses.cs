using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	/* SelProc */
		public class SlotHideProcess: UIProcess, IUISelProcess{
			ISlot slot;
			public SlotHideProcess(IEnumeratorFake coroutine, ISlot slot): base(coroutine){
				this.slot = slot;
			}
			public override void Expire(){
				if(slot.IsReadyForExchange())
					slot.SubstituteWithSlotWithSwappedItem();
				else
					slot.SubstituteWithEmptySlot();
				slot.SetID( InvalidID());
			}
		}
	/* Act Proc */
		public interface ISlotActProcess: IUIProcess{}
		public abstract class SlotActProcess: UIProcess{
			protected ISlotActStateHandler handler;
			public SlotActProcess(IEnumeratorFake coroutine, ISlotActStateHandler handler): base(coroutine){
				this.handler = handler;
			}
		}
			
		public class SlotWaitForActionProcess: SlotActProcess, ISlotActProcess{
			public SlotWaitForActionProcess(IEnumeratorFake coroutine, ISlotActStateHandler handler): base(coroutine, handler){}
		}
		public class SlotWaitForPickUpProcess: SlotActProcess, ISlotActProcess{
			public SlotWaitForPickUpProcess(IEnumeratorFake coroutine, ISlotActStateHandler handler): base(coroutine, handler){}
			public override void Expire(){
				handler.PickUp();
				handler.WaitForAction();
			}
		}
		public class SlotWaitForPointerUpProcess: SlotActProcess, ISlotActProcess{
			public SlotWaitForPointerUpProcess(IEnumeratorFake coroutine, ISlotActStateHandler handler): base(coroutine, handler){}
		}
		public class SlotWaitForNextTouchProcess: SlotActProcess, ISlotActProcess{
			public SlotWaitForNextTouchProcess(IEnumeratorFake coroutine, ISlotActStateHandler handler): base(coroutine, handler){}
			public override void Expire(){
				handler.PickUp();
				handler.WaitForAction();
			}
		}
}