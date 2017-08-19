using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class RevertTransaction: AbsSlotSystemTransaction, IRevertTransaction{
		ISlottable _pickedSB;
		ISlotGroup _origSG;
		ISlotsHolder origSGSlotsHolder;
		ITransactionIconHandler iconHandler;
		ISGActStateHandler origSGActStateHandler;
		ISGTransactionHandler origSGTAHandler;
		public RevertTransaction(ISlottable pickedSB, ITransactionManager tam): base(tam){
			_pickedSB = pickedSB;
			_origSG = _pickedSB.GetSG();
			iconHandler = tam.GetIconHandler();
			origSGSlotsHolder = _origSG.GetSlotsHolder();
			origSGActStateHandler = _origSG.GetSGActStateHandler();
			origSGTAHandler = _origSG.GetSGTAHandler();
		}
		public override void Indicate(){}
		public override void Execute(){
			origSGActStateHandler.Revert();
			iconHandler.SetD1Destination(_origSG, origSGSlotsHolder.GetNewSlot(_pickedSB.GetItem()));
			_origSG.OnActionExecute();
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
