using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class StackTransaction: AbsSlotSystemTransaction{
		public ISlottable m_pickedSB;
		public ISlotGroup m_origSG;
		public ISlottable m_selectedSB;
		public ISlotGroup m_selectedSG;
		public List<InventoryItemInstance> itemCache = new List<InventoryItemInstance>();
		public StackTransaction(ISlottable pickedSB ,ISlottable selected){
			m_pickedSB = pickedSB;
			m_origSG = pickedSB.sg;
			m_selectedSB = selected;
			m_selectedSG = m_selectedSB.sg;
			InventoryItemInstance cache = pickedSB.itemInst;
			cache.Quantity = pickedSB.pickedAmount;
			itemCache.Add(cache);
		}
		public StackTransaction(StackTransaction orig){
			this.m_pickedSB = SlotSystemUtil.CloneSB(orig.m_pickedSB);
			this.m_origSG = SlotSystemUtil.CloneSG(orig.m_origSG);
			this.m_selectedSB = SlotSystemUtil.CloneSB(orig.m_selectedSB);
			this.m_selectedSG = SlotSystemUtil.CloneSG(orig.m_selectedSG);
			InventoryItemInstance item = this.m_pickedSB.itemInst;
			item.Quantity = orig.m_pickedSB.pickedAmount;
			itemCache.Add(item);
		}
		public override ISlottable targetSB{get{return m_selectedSB;}}
		public override ISlotGroup sg1{get{return m_origSG;}}
		public override ISlotGroup sg2{get{return m_selectedSG;}}
		public override List<InventoryItemInstance> moved{get{return itemCache;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1.SetActState(SlotGroup.removeState);
			sg2.SetActState(SlotGroup.addState);
			ssm.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.itemInst));
			base.Execute();
		}
		public override void OnComplete(){
			sg1.OnCompleteSlotMovements();
			sg2.OnCompleteSlotMovements();
			base.OnComplete();
		}
	}
}
