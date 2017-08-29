using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class SlotProcess : UIProcess{
		public SlotProcess(Func<IEnumeratorFake> coroutine): base(coroutine){}
	}
	public interface ISlotActProcess: IUIProcess{}
		public class WaitForPickUpProcess: SlotProcess, ISlotActProcess{
			ISlotActStateHandler actStateHandler;
			public WaitForPickUpProcess(ISlotActStateHandler actStateHandler, System.Func<IEnumeratorFake> coroutine): base(coroutine){
				this.actStateHandler = actStateHandler;
			} 
			public override void Expire(){
				base.Expire();
				actStateHandler.PickUp();
			}
		}
		public class WaitForPointerUpProcess: SlotProcess, ISlotActProcess{
			IUISelStateHandler selStateHandler;
			public WaitForPointerUpProcess(IUISelStateHandler stateHandler, System.Func<IEnumeratorFake> coroutine): base(coroutine){
				selStateHandler = stateHandler;
			}
			public override void Expire(){
				base.Expire();
				selStateHandler.MakeUnselectable();
			}
		}
		public class WaitForNextTouchProcess: SlotProcess, ISlotActProcess{
			ISlot slot;
			IUISelStateHandler selStateHandler;
			public WaitForNextTouchProcess(ISlot slot, System.Func<IEnumeratorFake> coroutine): base(coroutine){
				this.slot = slot;
				this.selStateHandler = slot.UISelStateHandler();
			}
			public override void Expire(){
				base.Expire();
				if(!slot.IsPickedUp()){
					slot.Tap();
					slot.Refresh();
					selStateHandler.MakeSelectable();
				}else{
					
				}
			}
		}
		public class PickUpProcess: SlotProcess, ISlotActProcess{
			public PickUpProcess(System.Func<IEnumeratorFake> coroutine): base(coroutine){
			}
		}
		public class SlotActCoroutineRepo: ISlotActCoroutineRepo{
			public Func<IEnumeratorFake> GetWaitForPointerUpCoroutine(){
				return _waitForPointerUpCoroutine;
			}
				IEnumeratorFake _waitForPointerUpCoroutine(){
					return null;
				}
			public Func<IEnumeratorFake> GetWaitForPickUpCoroutine(){
				return _waitForPickUpCoroutine;
			}
				IEnumeratorFake _waitForPickUpCoroutine(){
					return null;
				}
			public Func<IEnumeratorFake> GetPickUpCoroutine(){
				return _pickUpCoroutine;
			}
				IEnumeratorFake _pickUpCoroutine(){
					return null;
				}
			public Func<IEnumeratorFake> GetWaitForNextTouchCoroutine(){
				return _waitForNextTouchCoroutine;
			}
				IEnumeratorFake _waitForNextTouchCoroutine(){
					return null;
				}
		}
		public interface ISlotActCoroutineRepo{
			Func<IEnumeratorFake> GetWaitForPointerUpCoroutine();
			Func<IEnumeratorFake> GetWaitForPickUpCoroutine();
			Func<IEnumeratorFake> GetPickUpCoroutine();
			Func<IEnumeratorFake> GetWaitForNextTouchCoroutine();
		}
		public class SlotSelCoroutineRepo: IUISelCoroutineRepo{
			public Func<IEnumeratorFake> DeactivateCoroutine(){
				return SlotDeactivateCoroutine;
			}
				IEnumeratorFake SlotDeactivateCoroutine(){
					return null;
				}
			public Func<IEnumeratorFake> MakeUnselectableCoroutine(){
				return SlotMakeUnselectableCoroutine;
			}
				IEnumeratorFake SlotMakeUnselectableCoroutine(){
					return null;
				}
			public Func<IEnumeratorFake> MakeSelectableCoroutine(){
				return SlotMakeSelectableCoroutine;
			}
				IEnumeratorFake SlotMakeSelectableCoroutine(){
					return null;
				}
			public Func<IEnumeratorFake> SelectCoroutine(){
				return SlotSelectCoroutine;
			}
				IEnumeratorFake SlotSelectCoroutine(){
					return null;
				}
		}
}
