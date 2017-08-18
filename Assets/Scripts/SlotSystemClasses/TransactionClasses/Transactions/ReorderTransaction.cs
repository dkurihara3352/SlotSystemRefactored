using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class ReorderTransaction: AbsSlotSystemTransaction, IReorderTransaction{
		ISlottable _pickedSB;
		ISlottable _selectedSB;
		ISlotGroup _origSG;
		ITransactionIconHandler iconHandler;
		ISlotsHolder sg1SlotsHolder;
		ISGActStateHandler sg1ActStateHandler;
		ISGTransactionHandler sg1TAHandler;
		public ReorderTransaction(ISlottable pickedSB, ISlottable selected, ITransactionManager tam): base(tam){
			_pickedSB = pickedSB;
			_selectedSB = selected;
			_origSG = _pickedSB.GetSG();
			iconHandler = tam.GetIconHandler();
			sg1SlotsHolder = GetSG1();
			sg1ActStateHandler = GetSG1();
			sg1TAHandler = GetSG1();
		}
		public override ISlottable GetTargetSB(){
			return _selectedSB;
		}
		public override ISlotGroup GetSG1(){
			return _origSG;
		}
		public override void Indicate(){}
		public override void Execute(){
			ISlotGroup sg1 = GetSG1();
			sg1ActStateHandler.Reorder();
			iconHandler.SetD1Destination(sg1, sg1SlotsHolder.GetNewSlot(_pickedSB.GetItem()));
			sg1.OnActionExecute();
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			sg1TAHandler.UpdateSBs();
			base.OnCompleteTransaction();
		}
	}
	public interface IReorderTransaction: ISlotSystemTransaction{}
	public class TestReorderTransaction: TestTransaction, IReorderTransaction{}
}
