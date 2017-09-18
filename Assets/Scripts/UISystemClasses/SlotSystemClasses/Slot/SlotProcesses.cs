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
		}
	/* Act Proc */
		public interface ISlotActProcess: IUIProcess{}
		public abstract class SlotActProcess: UIProcess, ISlotActProcess{
			protected ISlotActStateEngine engine;
			public SlotActProcess(IEnumeratorFake coroutine, ISlotActStateEngine engine): base(coroutine){
				this.engine = engine;
			}
		}
			
		public class SlotWaitForActionProcess: SlotActProcess{
			public SlotWaitForActionProcess(IEnumeratorFake coroutine, ISlotActStateEngine engine): base(coroutine, engine){}
		}
		public class SlotWaitForPickUpProcess: SlotActProcess{
			public SlotWaitForPickUpProcess(IEnumeratorFake coroutine, ISlotActStateEngine engine): base(coroutine, engine){}
			public override void Expire(){
				engine.PickUp();
				engine.WaitForAction();
			}
		}
		public class SlotWaitForPointerUpProcess: SlotActProcess{
			public SlotWaitForPointerUpProcess(IEnumeratorFake coroutine, ISlotActStateEngine engine): base(coroutine, engine){}
		}
		public class SlotWaitForNextTouchProcess: SlotActProcess{
			public SlotWaitForNextTouchProcess(IEnumeratorFake coroutine, ISlotActStateEngine engine): base(coroutine, engine){}
			public override void Expire(){
				engine.PickUp();
				engine.WaitForAction();
			}
		}
	/* FadeProc */
		public interface ISlotFadeProcess: IUIProcess{}
		public abstract class SlotFadeProcess: UIProcess, ISlotFadeProcess{
			protected ISlotFadeStateEngine engine;
			public SlotFadeProcess( IEnumeratorFake coroutine, ISlotFadeStateEngine engine): base( coroutine){
				this.engine = engine;
			}
		}
		public class SlotWaitForItemFadeProcess: SlotFadeProcess{
			public SlotWaitForItemFadeProcess( IEnumeratorFake coroutine, ISlotFadeStateEngine engine): base( coroutine, engine){}
		}
		public class SlotFadeItemProcess: SlotFadeProcess{
			public SlotFadeItemProcess( IEnumeratorFake coroutine, ISlotFadeStateEngine engine): base( coroutine, engine){}
			public override void Expire(){
				engine.WaitForItemFade();
			}
		}
}