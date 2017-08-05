using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class StackTransaction: AbsSlotSystemTransaction, IStackTransaction{
		ISlottable m_pickedSB;
		ISlotGroup m_origSG;
		ISlottable m_selectedSB;
		ISlotGroup m_selectedSG;
		List<InventoryItemInstance> itemCache = new List<InventoryItemInstance>();
		ITransactionIconHandler iconHandler;
		public StackTransaction(ISlottable pickedSB ,ISlottable selected, ITransactionManager tam): base(tam){
			m_pickedSB = pickedSB;
			m_origSG = pickedSB.sg;
			m_selectedSB = selected;
			m_selectedSG = m_selectedSB.sg;
			InventoryItemInstance cache = pickedSB.item;
			cache.quantity = pickedSB.pickedAmount;
			itemCache.Add(cache);
			this.iconHandler = tam.iconHandler;
		}
		public override ISlottable targetSB{get{return m_selectedSB;}}
		public override ISlotGroup sg1{get{return m_origSG;}}
		public override ISlotGroup sg2{get{return m_selectedSG;}}
		public override List<InventoryItemInstance> moved{get{return itemCache;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1.Remove();
			sg2.Add();
			iconHandler.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.item));
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			sg1.OnCompleteSlotMovements();
			sg2.OnCompleteSlotMovements();
			base.OnCompleteTransaction();
		}
	}
	public interface IStackTransaction:ISlotSystemTransaction{}
	public class TestStackTransaction: TestTransaction, IStackTransaction{}
}
