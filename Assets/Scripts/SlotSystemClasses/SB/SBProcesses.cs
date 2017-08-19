using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace SlotSystem{
	public abstract class SBProcess: SSEProcess, ISBProcess{
		public SBProcess(Func<IEnumeratorFake> coroutine): base(coroutine){
		}
	}
	public interface ISBProcess: ISSEProcess{
	}
		public interface ISBActProcess: ISBProcess{}
			public class WaitForPickUpProcess: SBProcess, ISBActProcess{
				ISBActStateHandler actStateHandler;
				public WaitForPickUpProcess(ISBActStateHandler actStateHandler, System.Func<IEnumeratorFake> coroutine): base(coroutine){
					this.actStateHandler = actStateHandler;
				} 
				public override void Expire(){
					base.Expire();
					actStateHandler.PickUp();
				}
			}
			public class WaitForPointerUpProcess: SBProcess, ISBActProcess{
				ISSESelStateHandler selStateHandler;
				public WaitForPointerUpProcess(ISSESelStateHandler stateHandler, System.Func<IEnumeratorFake> coroutine): base(coroutine){
					selStateHandler = stateHandler;
				}
				public override void Expire(){
					base.Expire();
					selStateHandler.Defocus();
				}
			}
			public class WaitForNextTouchProcess: SBProcess, ISBActProcess{
				ITransactionManager tam;
				ISlottable sb;
				ISSESelStateHandler selStateHandler;
				public WaitForNextTouchProcess(ISlottable sb, ITransactionManager tam, System.Func<IEnumeratorFake> coroutine): base(coroutine){
					this.tam = tam;
					this.sb = sb;
					this.selStateHandler = sb.GetSelStateHandler();
				}
				public override void Expire(){
					base.Expire();
					if(!sb.IsPickedUp()){
						sb.Tap();
						sb.Refresh();
						selStateHandler.Focus();
					}else{
						tam.ExecuteTransaction();
					}
				}
			}
			public class PickUpProcess: SBProcess, ISBActProcess{
				public PickUpProcess(System.Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
			public class SBMoveWithinProcess: SBProcess, ISBActProcess{
				public SBMoveWithinProcess(System.Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
			public class SBAddProcess: SBProcess, ISBActProcess{
				public SBAddProcess(System.Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
			public class SBRemoveProcess: SBProcess, ISBActProcess{
				public SBRemoveProcess(System.Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
		public interface ISBEqpProcess: ISBProcess{}
			public class SBEquipProcess: SBProcess, ISBEqpProcess{
				public SBEquipProcess(System.Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
			public class SBUnequipProcess: SBProcess, ISBEqpProcess{
				public SBUnequipProcess(System.Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
		public interface ISBMrkProcess: ISBProcess{}
			public class SBMarkProcess: SBProcess, ISBMrkProcess{
				public SBMarkProcess(System.Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
			public class SBUnmarkProcess: SBProcess, ISBMrkProcess{
				public SBUnmarkProcess(System.Func<IEnumeratorFake> coroutine): base(coroutine){
				}
			}
	public class SBActCoroutineRepo: ISBActCoroutineRepo{
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
		public Func<IEnumeratorFake> GetRemoveCoroutine(){
			return _removeCoroutine;
		}
			IEnumeratorFake _removeCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> GetAddCoroutine(){
			return _addCoroutine;
		}
			IEnumeratorFake _addCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> GetMoveWithinCoroutine(){
			return _moveWithinCoroutine;
		}
			IEnumeratorFake _moveWithinCoroutine(){
				return null;
			}
	}
	public interface ISBActCoroutineRepo{
		Func<IEnumeratorFake> GetWaitForPointerUpCoroutine();
		Func<IEnumeratorFake> GetWaitForPickUpCoroutine();
		Func<IEnumeratorFake> GetPickUpCoroutine();
		Func<IEnumeratorFake> GetWaitForNextTouchCoroutine();
		Func<IEnumeratorFake> GetRemoveCoroutine();
		Func<IEnumeratorFake> GetAddCoroutine();
		Func<IEnumeratorFake> GetMoveWithinCoroutine();
	}
	public class SBEqpCoroutineRepo: ISBEqpCoroutineRepo{
		public Func<IEnumeratorFake> GetEquipCoroutine(){
			return _equipCoroutine;
		}
			IEnumeratorFake _equipCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> GetUnequipCoroutine(){
			return _unequipCoroutine;
		}
			IEnumeratorFake _unequipCoroutine(){
				return null;
			}
	}
	public interface ISBEqpCoroutineRepo{
		Func<IEnumeratorFake> GetEquipCoroutine();
		Func<IEnumeratorFake> GetUnequipCoroutine();
	}
	public class SBMrkCoroutineRepo: ISBMrkCoroutineRepo{
		public Func<IEnumeratorFake> GetMarkCoroutine(){
			return _markCoroutine;
		}
			IEnumeratorFake _markCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> GetUnmarkCoroutine(){
			return _unmarkCoroutine;
		}
			IEnumeratorFake _unmarkCoroutine(){
				return null;
			}
	}
	public interface ISBMrkCoroutineRepo{
		Func<IEnumeratorFake> GetMarkCoroutine();
		Func<IEnumeratorFake> GetUnmarkCoroutine();
	}
}