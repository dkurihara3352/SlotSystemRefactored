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
			_origSG = _pickedSB.GetSG();
			iconHandler = tam.GetIconHandler();
			sg2SlotsHolder = GetSG2();
			sg1ActStateHandler = GetSG1();
			sg2ActStateHandler = GetSG2();
			sg1TAHandler = GetSG1();
			sg2TAHandler = GetSG2();
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
