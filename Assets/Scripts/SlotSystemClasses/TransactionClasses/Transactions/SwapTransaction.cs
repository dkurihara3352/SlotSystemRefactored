using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SwapTransaction: AbsSlotSystemTransaction, ISwapTransaction{
		ISlottable _pickedSB;
		ISlotGroup _origSG;
		ISlottable _selectedSB;
		ISlotGroup _selectedSG;
		ITransactionIconHandler iconHandler;
		ISlotsHolder sg1SlotsHolder;
		ISlotsHolder sg2SlotsHolder;
		ISGActStateHandler sg1ActStateHandler;
		ISGActStateHandler sg2ActStateHandler;
		ISGTransactionHandler sg1TAHandler;
		ISGTransactionHandler sg2TAHandler;
		public SwapTransaction(ISlottable pickedSB, ISlottable selected, ITransactionManager tam): base(tam){
			_pickedSB = pickedSB;
			_selectedSB = selected;
			_origSG = _pickedSB.GetSG();
			ISlotGroup sg1 = GetSG1();
			_selectedSG = _selectedSB.GetSG();
			ISlotGroup sg2 = GetSG2();
			iconHandler = tam.GetIconHandler();
			sg1SlotsHolder = sg1.GetSlotsHolder();
			sg2SlotsHolder = sg2.GetSlotsHolder();
			sg1ActStateHandler = sg1.GetSGActStateHandler();
			sg2ActStateHandler = sg2.GetSGActStateHandler();
			sg1TAHandler = sg1.GetSGTAHandler();
			sg2TAHandler = sg2.GetSGTAHandler();
		}
		public override ISlottable GetTargetSB(){
			return _selectedSB;
		}
		public override ISlotGroup GetSG1(){
			return _origSG;
		}
		public override ISlotGroup GetSG2(){
			return _selectedSG;
		}
		public override void Indicate(){}
		public override void Execute(){
			ISlottable targetSB = GetTargetSB();
			ISlotGroup sg1 = GetSG1();
			ISlotGroup sg2 = GetSG2();
			sg1ActStateHandler.Swap();
			sg2ActStateHandler.Swap();
			iconHandler.SetD1Destination(sg2, sg2SlotsHolder.GetNewSlot(_pickedSB.GetItem()));
			iconHandler.SetDIcon2(targetSB);
			iconHandler.SetD2Destination(sg1, sg1SlotsHolder.GetNewSlot(targetSB.GetItem()));
			sg1.OnActionExecute();
			sg2.OnActionExecute();
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			sg1TAHandler.UpdateSBs();
			sg2TAHandler.UpdateSBs();
			base.OnCompleteTransaction();
		}
	}
	public interface ISwapTransaction: ISlotSystemTransaction{}
	public class TestSwapTransaction: TestTransaction, ISwapTransaction{}
}
