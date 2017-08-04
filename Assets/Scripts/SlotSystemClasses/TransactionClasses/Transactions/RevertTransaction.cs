using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class RevertTransaction: AbsSlotSystemTransaction, IRevertTransaction{
		public ISlottable m_pickedSB;
		public ISlotGroup m_origSG;
		public RevertTransaction(ISlottable pickedSB, ITransactionManager tam): base(tam){
			m_pickedSB = pickedSB;
			m_origSG = m_pickedSB.sg;
		}
		public override void Indicate(){}
		public override void Execute(){
			m_origSG.Revert();
			tam.dIcon1.SetDestination(m_origSG, m_origSG.GetNewSlot(m_pickedSB.item));
			m_origSG.OnActionExecute();
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			m_origSG.OnCompleteSlotMovements();
			base.OnCompleteTransaction();
		}
	}
	public interface IRevertTransaction: ISlotSystemTransaction{}
	public class TestRevertTransaction: TestTransaction, IRevertTransaction{}
}
