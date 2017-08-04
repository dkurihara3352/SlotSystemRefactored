using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class StackTransaction: AbsSlotSystemTransaction, IStackTransaction{
		public ISlottable m_pickedSB;
		public ISlotGroup m_origSG;
		public ISlottable m_selectedSB;
		public ISlotGroup m_selectedSG;
		public List<InventoryItemInstance> itemCache = new List<InventoryItemInstance>();
		public StackTransaction(ISlottable pickedSB ,ISlottable selected, ITransactionManager tam): base(tam){
			m_pickedSB = pickedSB;
			m_origSG = pickedSB.sg;
			m_selectedSB = selected;
			m_selectedSG = m_selectedSB.sg;
			InventoryItemInstance cache = pickedSB.item;
			cache.quantity = pickedSB.pickedAmount;
			itemCache.Add(cache);
		}
		public override ISlottable targetSB{get{return m_selectedSB;}}
		public override ISlotGroup sg1{get{return m_origSG;}}
		public override ISlotGroup sg2{get{return m_selectedSG;}}
		public override List<InventoryItemInstance> moved{get{return itemCache;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1.Remove();
			sg2.Add();
			tam.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.item));
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
