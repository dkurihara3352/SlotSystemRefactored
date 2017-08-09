using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class RevertTransaction: AbsSlotSystemTransaction, IRevertTransaction{
		ISlottable m_pickedSB;
		ISlotGroup m_origSG;
		ISlotsHolder origSGSlotsHolder;
		ITransactionIconHandler iconHandler;
		public RevertTransaction(ISlottable pickedSB, ITransactionManager tam): base(tam){
			m_pickedSB = pickedSB;
			m_origSG = m_pickedSB.sg;
			this.iconHandler = tam.iconHandler;
			this.origSGSlotsHolder = m_origSG;
		}
		public override void Indicate(){}
		public override void Execute(){
			m_origSG.Revert();
			iconHandler.dIcon1.SetDestination(m_origSG, origSGSlotsHolder.GetNewSlot(m_pickedSB.item));
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
