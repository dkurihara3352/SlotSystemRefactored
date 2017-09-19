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
	/* ItemVisualUpdateProcs */
		public interface IItemVisualUpdateProcess: IUIProcess{}
		public abstract class ItemVisualUpdateProcess: UIProcess, IItemVisualUpdateProcess{
			protected IItemVisualUpdateEngine engine;
			public ItemVisualUpdateProcess( IEnumeratorFake coroutine, IItemVisualUpdateEngine engine): base( coroutine){
				this.engine = engine;
			}
		}
		public class WaitForItemVisualUpdateProcess: ItemVisualUpdateProcess{
			public WaitForItemVisualUpdateProcess( IEnumeratorFake coroutine, IItemVisualUpdateEngine engine): base( coroutine, engine){}
		}
		public class UpdateItemVisualProcess: ItemVisualUpdateProcess{
			public UpdateItemVisualProcess( IEnumeratorFake coroutine, IItemVisualUpdateEngine engine): base( coroutine, engine){}
			public override void Expire(){
				engine.WaitForItemVisualUpdate();
			}
		}
	
	
	/*	GhostifyProc */
		public interface IGhostificationProcess: IUIProcess{}
		public abstract class GhostificationProcess: UIProcess, IGhostificationProcess{
			protected IGhostificationEngine engine;
			public GhostificationProcess( IEnumeratorFake coroutine, IGhostificationEngine engine): base( coroutine){
				this.engine = engine;
			}
		}

		public class UnghostifyProcess: GhostificationProcess{
			public UnghostifyProcess( IEnumeratorFake coroutine, IGhostificationEngine engine): base( coroutine, engine){}
		}
		public class GhostifyProcess: GhostificationProcess{
			public GhostifyProcess( IEnumeratorFake coroutine, IGhostificationEngine engine): base( coroutine, engine){}
		}


	/* QuantityVisualUpdate Process */
		public interface IQuantityVisualUpdateProcess: IUIProcess{}
		public abstract class QuantityVisualUpdateProcess: UIProcess, IQuantityVisualUpdateProcess{
			protected IQuantityVisualUpdateEngine engine;
			public QuantityVisualUpdateProcess( IEnumeratorFake coroutine, IQuantityVisualUpdateEngine engine): base( coroutine){
				this.engine = engine;
			}
		}
		public class UpdateQuantityVisualProcess: QuantityVisualUpdateProcess{
			public UpdateQuantityVisualProcess( IEnumeratorFake coroutine, IQuantityVisualUpdateEngine engine): base( coroutine, engine){}
		}
}