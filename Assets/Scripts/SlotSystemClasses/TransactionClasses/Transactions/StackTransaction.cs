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
		ISlotsHolder sg2SlotsHolder;
		ISGActStateHandler sg1ActStateHandler;
		ISGActStateHandler sg2ActStateHandler;
		ISGTransactionHandler sg1TAHandler;
		ISGTransactionHandler sg2TAHandler;
		public StackTransaction(ISlottable pickedSB ,ISlottable selected, ITransactionManager tam): base(tam){
			m_pickedSB = pickedSB;
			m_origSG = pickedSB.sg;
			m_selectedSB = selected;
			m_selectedSG = m_selectedSB.sg;
			InventoryItemInstance cache = pickedSB.item;
			cache.quantity = pickedSB.pickedAmount;
			itemCache.Add(cache);
			iconHandler = tam.iconHandler;
			sg2SlotsHolder = sg2;
			sg1ActStateHandler = sg1;
			sg2ActStateHandler = sg2;
			sg1TAHandler = sg1;
			sg2TAHandler = sg2;
		}
		public override ISlottable targetSB{get{return m_selectedSB;}}
		public override ISlotGroup sg1{get{return m_origSG;}}
		public override ISlotGroup sg2{get{return m_selectedSG;}}
		public override List<InventoryItemInstance> moved{get{return itemCache;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1ActStateHandler.Remove();
			sg2ActStateHandler.Add();
			iconHandler.SetD1Destination(sg2, sg2SlotsHolder.GetNewSlot(m_pickedSB.item));
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			sg1TAHandler.UpdateSBs();
			sg2TAHandler.UpdateSBs();
			base.OnCompleteTransaction();
		}
	}
	public interface IStackTransaction:ISlotSystemTransaction{}
	public class TestStackTransaction: TestTransaction, IStackTransaction{}
}
