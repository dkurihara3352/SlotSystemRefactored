using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class FillTransaction: AbsSlotSystemTransaction, IFillTransaction{
		ISlottable _pickedSB;
		ISlotGroup _selectedSG;
		ISlotGroup _origSG;
		ITransactionIconHandler iconHandler;
		ISlotsHolder sg2SlotsHolder;
		ISGActStateHandler sg1ActStateHandler;
		ISGActStateHandler sg2ActStateHandler;
		ISGTransactionHandler sg1TAHandler;
		ISGTransactionHandler sg2TAHandler;
		public FillTransaction(ISlottable pickedSB, ISlotGroup selected, ITransactionManager tam):base(tam){
			_pickedSB = pickedSB;
			_selectedSG = selected;
			ISlotGroup sg2 = GetSG2();
			_origSG = _pickedSB.GetSG();
			ISlotGroup sg1 = GetSG1();
			iconHandler = tam.GetIconHandler();
			sg2SlotsHolder = sg2.GetSlotsHolder();
			sg1ActStateHandler = sg1.GetSGActStateHandler();
			sg2ActStateHandler = sg2.GetSGActStateHandler();
			sg1TAHandler = sg1.GetSGTAHandler();
			sg2TAHandler = sg2.GetSGTAHandler();
		}
		public override ISlotGroup GetSG1(){
			return _origSG;
		}
		public override ISlotGroup GetSG2(){
			return _selectedSG;
		}
		public override void Indicate(){}
		public override void Execute(){
			ISlotGroup sg1 = GetSG1();
			ISlotGroup sg2 = GetSG2();
			sg1ActStateHandler.Fill();
			sg2ActStateHandler.Fill();
			iconHandler.SetD1Destination(sg2, sg2SlotsHolder.GetNewSlot(_pickedSB.GetItem()));
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
	public interface IFillTransaction: ISlotSystemTransaction{}
	public class TestFillTransaction: TestTransaction, IFillTransaction{}
}
