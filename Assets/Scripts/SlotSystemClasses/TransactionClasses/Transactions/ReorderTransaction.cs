using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class ReorderTransaction: AbsSlotSystemTransaction, IReorderTransaction{
		public ISlottable m_pickedSB;
		public ISlottable m_selectedSB;
		public ISlotGroup m_origSG;
		public ReorderTransaction(ISlottable pickedSB, ISlottable selected){
			m_pickedSB = pickedSB;
			m_selectedSB = selected;
			m_origSG = m_pickedSB.sg;
		}
		public ReorderTransaction(ReorderTransaction orig){
			this.m_pickedSB = SlotSystemUtil.CloneSB(orig.m_pickedSB);
			this.m_selectedSB = SlotSystemUtil.CloneSB(orig.m_selectedSB);
			this.m_origSG = SlotSystemUtil.CloneSG(orig.m_origSG);
		}
		public override ISlottable targetSB{get{return m_selectedSB;}}
		public override ISlotGroup sg1{get{return m_origSG;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1.Reorder();
			ssm.dIcon1.SetDestination(sg1, sg1.GetNewSlot(m_pickedSB.itemInst));
			sg1.OnActionExecute();
			base.Execute();
		}
		public override void OnComplete(){
			sg1.OnCompleteSlotMovements();
			base.OnComplete();
		}
	}
	public interface IReorderTransaction: ISlotSystemTransaction{}
	public class TestReorderTransaction: TestTransaction, IReorderTransaction{}
}
