using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class AbsSlotSystemTransaction: ISlotSystemTransaction{
		protected ITransactionManager tam = TransactionManager.curTAM;
		protected List<InventoryItemInstance> removed = new List<InventoryItemInstance>();
		protected List<InventoryItemInstance> added = new List<InventoryItemInstance>();
		public virtual ISlottable targetSB{get{return null;}}
		public virtual ISlotGroup sg1{get{return null;}}
		public virtual ISlotGroup sg2{get{return null;}}
		public virtual List<InventoryItemInstance> moved{get{return null;}}
		public virtual void Indicate(){}
		public virtual void Execute(){
			tam.Transact();
		}
		public virtual void OnComplete(){
			tam.Refresh();
		}
	}
}
