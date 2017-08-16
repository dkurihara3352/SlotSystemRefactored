using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class RevertTransaction: AbsSlotSystemTransaction, IRevertTransaction{
		ISlottable m_pickedSB;
		ISlotGroup m_origSG;
		ISlotsHolder origSGSlotsHolder;
		ITransactionIconHandler iconHandler;
		ISGActStateHandler origSGActStateHandler;
		ISGTransactionHandler origSGTAHandler;
		public RevertTransaction(ISlottable pickedSB, ITransactionManager tam): base(tam){
			m_pickedSB = pickedSB;
			m_origSG = m_pickedSB.sg;
			iconHandler = tam.iconHandler;
			origSGSlotsHolder = m_origSG;
			origSGActStateHandler = m_origSG;
			origSGTAHandler = m_origSG;
		}
		public override void Indicate(){}
		public override void Execute(){
			origSGActStateHandler.Revert();
			iconHandler.SetD1Destination(m_origSG, origSGSlotsHolder.GetNewSlot(m_pickedSB.item));
			m_origSG.OnActionExecute();
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			origSGTAHandler.UpdateSBs();
			base.OnCompleteTransaction();
		}
	}
	public interface IRevertTransaction: ISlotSystemTransaction{}
	public class TestRevertTransaction: TestTransaction, IRevertTransaction{}
}
