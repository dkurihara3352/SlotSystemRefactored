using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class FillTransaction: AbsSlotSystemTransaction{
		public Slottable m_pickedSB;
		public SlotGroup m_selectedSG;
		public SlotGroup m_origSG;
		public FillTransaction(Slottable pickedSB, SlotGroup selected){
			m_pickedSB = pickedSB;
			m_selectedSG = selected;
			m_origSG = m_pickedSB.sg;
		}
		public FillTransaction(FillTransaction orig){
			this.m_pickedSB = SlotSystemUtil.CloneSB(orig.m_pickedSB);
			this.m_selectedSG = SlotSystemUtil.CloneSG(orig.m_selectedSG);
			this.m_origSG = SlotSystemUtil.CloneSG(orig.m_origSG);
		}
		public override SlotGroup sg1{get{return m_origSG;}}
		public override SlotGroup sg2{get{return m_selectedSG;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1.SetActState(SlotGroup.fillState);
			sg2.SetActState(SlotGroup.fillState);
			ssm.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.itemInst));
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
