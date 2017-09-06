using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public abstract class SlotProcess: UIProcess{
		protected ISlotActStateHandler handler;
		public SlotProcess(IEnumeratorFake coroutine, ISlotActStateHandler handler): base(coroutine){
			this.handler = handler;
		}
	}
	/* Act Proc */
		public interface ISlotActProcess: IUIProcess{}
			
		public class SlotWaitForActionProcess: SlotProcess, ISlotActProcess{
			public SlotWaitForActionProcess(IEnumeratorFake coroutine, ISlotActStateHandler handler): base(coroutine, handler){}
		}
		public class SlotWaitForPickUpProcess: SlotProcess, ISlotActProcess{
			public SlotWaitForPickUpProcess(IEnumeratorFake coroutine, ISlotActStateHandler handler): base(coroutine, handler){}
			public override void Expire(){
				handler.PickUp();
				handler.WaitForAction();
			}
		}
		public class SlotWaitForPointerUpProcess: SlotProcess, ISlotActProcess{
			public SlotWaitForPointerUpProcess(IEnumeratorFake coroutine, ISlotActStateHandler handler): base(coroutine, handler){}
		}
		public class SlotWaitForNextTouchProcess: SlotProcess, ISlotActProcess{
			public SlotWaitForNextTouchProcess(IEnumeratorFake coroutine, ISlotActStateHandler handler): base(coroutine, handler){}
			public override void Expire(){
				handler.PickUp();
				handler.WaitForAction();
			}
		}
}