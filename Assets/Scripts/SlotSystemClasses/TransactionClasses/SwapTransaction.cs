using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SwapTransaction: AbsSlotSystemTransaction{
		public Slottable m_pickedSB;
		public SlotGroup m_origSG;
		public Slottable m_selectedSB;
		public SlotGroup m_selectedSG;
		public SwapTransaction(Slottable pickedSB, Slottable selected){
			m_pickedSB = pickedSB;
			m_selectedSB = selected;
			m_origSG = m_pickedSB.sg;
			m_selectedSG = m_selectedSB.sg;
		}
		public SwapTransaction(SwapTransaction orig){
			this.m_pickedSB = SlotSystemUtil.CloneSB(orig.m_pickedSB);
			this.m_origSG = SlotSystemUtil.CloneSG(orig.m_origSG);
			this.m_selectedSB = SlotSystemUtil.CloneSB(orig.m_selectedSB);
			this.m_selectedSG = SlotSystemUtil.CloneSG(orig.m_selectedSG);
		}
		public override Slottable targetSB{get{return m_selectedSB;}}
		public override SlotGroup sg1{get{return m_origSG;}}
		public override SlotGroup sg2{get{return m_selectedSG;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1.SetActState(SlotGroup.swapState);
			sg2.SetActState(SlotGroup.swapState);
			ssm.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.itemInst));
			DraggedIcon di2 = new DraggedIcon(targetSB);
			ssm.SetDIcon2(di2);
			ssm.dIcon2.SetDestination(sg1, sg1.GetNewSlot(targetSB.itemInst));
			sg1.OnActionExecute();
			sg2.OnActionExecute();
			base.Execute();
		}
		public override void OnComplete(){
			sg1.OnCompleteSlotMovements();
			sg2.OnCompleteSlotMovements();
			base.OnComplete();
		}
	}
}
