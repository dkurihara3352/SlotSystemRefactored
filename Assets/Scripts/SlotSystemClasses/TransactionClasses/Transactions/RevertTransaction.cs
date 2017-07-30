using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class RevertTransaction: AbsSlotSystemTransaction, IRevertTransaction{
		public ISlottable m_pickedSB;
		public ISlotGroup m_origSG;
		public RevertTransaction(ISlottable pickedSB){
			m_pickedSB = pickedSB;
			m_origSG = m_pickedSB.sg;
		}
		public RevertTransaction(RevertTransaction orig){
			this.m_pickedSB = SlotSystemUtil.CloneSB(orig.m_pickedSB);
			this.m_origSG = SlotSystemUtil.CloneSG(orig.m_origSG);
		}
		public override void Indicate(){}
		public override void Execute(){
			m_origSG.Revert();
			tam.dIcon1.SetDestination(m_origSG, m_origSG.GetNewSlot(m_pickedSB.itemInst));
			m_origSG.OnActionExecute();
			base.Execute();
		}
		public override void OnComplete(){
			m_origSG.OnCompleteSlotMovements();
			base.OnComplete();
		}
	}
	public interface IRevertTransaction: ISlotSystemTransaction{}
	public class TestRevertTransaction: TestTransaction, IRevertTransaction{}
}
