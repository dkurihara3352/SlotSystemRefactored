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
			protected ISlotActStateEngine engine;
			public SlotActProcess(IEnumeratorFake coroutine, ISlotActStateEngine engine): base(coroutine){
				this.engine = engine;
			}
		}
			
		public class SlotWaitForActionProcess: SlotActProcess, ISlotActProcess{
			public SlotWaitForActionProcess(IEnumeratorFake coroutine, ISlotActStateEngine engine): base(coroutine, engine){}
		}
		public class SlotWaitForPickUpProcess: SlotActProcess, ISlotActProcess{
			public SlotWaitForPickUpProcess(IEnumeratorFake coroutine, ISlotActStateEngine engine): base(coroutine, engine){}
			public override void Expire(){
				engine.PickUp();
				engine.WaitForAction();
			}
		}
		public class SlotWaitForPointerUpProcess: SlotActProcess, ISlotActProcess{
			public SlotWaitForPointerUpProcess(IEnumeratorFake coroutine, ISlotActStateEngine engine): base(coroutine, engine){}
		}
		public class SlotWaitForNextTouchProcess: SlotActProcess, ISlotActProcess{
			public SlotWaitForNextTouchProcess(IEnumeratorFake coroutine, ISlotActStateEngine engine): base(coroutine, engine){}
			public override void Expire(){
				engine.PickUp();
				engine.WaitForAction();
			}
		}
}