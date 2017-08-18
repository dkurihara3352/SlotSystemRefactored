using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class AbsSlotSystemTransaction: ISlotSystemTransaction{
		protected ITransactionManager tam;
		protected ITAMActStateHandler tamStateHandler;
		public AbsSlotSystemTransaction(ITransactionManager tam){
			this.tam = tam;
			this.tamStateHandler = tam.GetActStateHandler();
		}
		public virtual ISlottable GetTargetSB(){
			return null;
		}
		public virtual ISlotGroup GetSG1(){
			return null;
		}
		public virtual ISlotGroup GetSG2(){
			return null;
		}
		public virtual List<IInventoryItemInstance> GetMoved(){
			return null;
		}
		public virtual void Indicate(){}
		public virtual void Execute(){
			tamStateHandler.Transact();
		}
		public virtual void OnCompleteTransaction(){
			tam.Refresh();
		}
	}
}
