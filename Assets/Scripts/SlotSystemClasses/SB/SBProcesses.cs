using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace SlotSystem{
	public abstract class SBProcess: SSEProcess, ISBProcess{
		protected ISlottable sb;
		public SBProcess(ISlottable sb, Func<IEnumeratorFake> coroutine): base(coroutine){
			this.sb = sb;
		}
	}
	public interface ISBProcess: ISSEProcess{
	}
		public interface ISBActProcess: ISBProcess{}
			public class WaitForPickUpProcess: SBProcess, ISBActProcess{
				public WaitForPickUpProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutine): base(sb, coroutine){
				}
				public override void Expire(){
					base.Expire();
					sb.PickUp();
				}
			}
			public class WaitForPointerUpProcess: SBProcess, ISBActProcess{
				public WaitForPointerUpProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutine): base(sb, coroutine){
				}
				public override void Expire(){
					base.Expire();
					sb.Defocus();
				}
			}
			public class WaitForNextTouchProcess: SBProcess, ISBActProcess{
				ITransactionManager tam;
				public WaitForNextTouchProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutine, ITransactionManager tam): base(sb, coroutine){
					this.tam = tam;
				}
				public override void Expire(){
					base.Expire();
					if(!sb.isPickedUp){
						sb.Tap();
						sb.Refresh();
						sb.Focus();
					}else{
						tam.ExecuteTransaction();
					}
				}
			}
			public class PickUpProcess: SBProcess, ISBActProcess{
				public PickUpProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutine): base(sb, coroutine){
				}
			}
			public class SBMoveWithinProcess: SBProcess, ISBActProcess{
				public SBMoveWithinProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutine): base(sb, coroutine){
				}
			}
			public class SBAddProcess: SBProcess, ISBActProcess{
				public SBAddProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutine): base(sb, coroutine){
				}
			}
			public class SBRemoveProcess: SBProcess, ISBActProcess{
				public SBRemoveProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutine): base(sb, coroutine){
				}
			}
		public interface ISBEqpProcess: ISBProcess{}
			public class SBEquipProcess: SBProcess, ISBEqpProcess{
				public SBEquipProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutine): base(sb, coroutine){
				}
			}
			public class SBUnequipProcess: SBProcess, ISBEqpProcess{
				public SBUnequipProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutine): base(sb, coroutine){
				}
			}
		public interface ISBMrkProcess: ISBProcess{}
			public class SBMarkProcess: SBProcess, ISBMrkProcess{
				public SBMarkProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutine): base(sb, coroutine){
				}
			}
			public class SBUnmarkProcess: SBProcess, ISBMrkProcess{
				public SBUnmarkProcess(ISlottable sb, System.Func<IEnumeratorFake> coroutine): base(sb, coroutine){
				}
			}
	public class SBActCoroutineRepo: ISBActCoroutineRepo{
		public Func<IEnumeratorFake> waitForPointerUpCoroutine{
			get{return _waitForPointerUpCoroutine;}
		}
			IEnumeratorFake _waitForPointerUpCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> waitForPickUpCoroutine{
			get{return _waitForPickUpCoroutine;}
		}
			IEnumeratorFake _waitForPickUpCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> pickUpCoroutine{
			get{return _pickUpCoroutine;}
		}
			IEnumeratorFake _pickUpCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> waitForNextTouchCoroutine{
			get{return _waitForNextTouchCoroutine;}
		}
			IEnumeratorFake _waitForNextTouchCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> removeCoroutine{
			get{return _removeCoroutine;}
		}
			IEnumeratorFake _removeCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> addCoroutine{
			get{return _addCoroutine;}
		}
			IEnumeratorFake _addCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> moveWithinCoroutine{
			get{return _moveWithinCoroutine;}
		}
			IEnumeratorFake _moveWithinCoroutine(){
				return null;
			}
	}
	public interface ISBActCoroutineRepo{
		Func<IEnumeratorFake> waitForPointerUpCoroutine{get;}
		Func<IEnumeratorFake> waitForPickUpCoroutine{get;}
		Func<IEnumeratorFake> pickUpCoroutine{get;}
		Func<IEnumeratorFake> waitForNextTouchCoroutine{get;}
		Func<IEnumeratorFake> removeCoroutine{get;}
		Func<IEnumeratorFake> addCoroutine{get;}
		Func<IEnumeratorFake> moveWithinCoroutine{get;}
	}
	public class SBEqpCoroutineRepo: ISBEqpCoroutineRepo{
		public Func<IEnumeratorFake> equipCoroutine{
			get{return _equipCoroutine;}
		}
			IEnumeratorFake _equipCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> unequipCoroutine{
			get{return _unequipCoroutine;}
		}
			IEnumeratorFake _unequipCoroutine(){
				return null;
			}
	}
	public interface ISBEqpCoroutineRepo{
		Func<IEnumeratorFake> equipCoroutine{get;}
		Func<IEnumeratorFake> unequipCoroutine{get;}
	}
	public class SBMrkCoroutineRepo: ISBMrkCoroutineRepo{
		public Func<IEnumeratorFake> markCoroutine{
			get{return _markCoroutine;}
		}
			IEnumeratorFake _markCoroutine(){
				return null;
			}
		public Func<IEnumeratorFake> unmarkCoroutine{
			get{return _unmarkCoroutine;}
		}
			IEnumeratorFake _unmarkCoroutine(){
				return null;
			}
	}
	public interface ISBMrkCoroutineRepo{
		Func<IEnumeratorFake> markCoroutine{get;}
		Func<IEnumeratorFake> unmarkCoroutine{get;}
	}
}