using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class StackTransaction: AbsSlotSystemTransaction, IStackTransaction{
		ISlottable _pickedSB;
		ISlotGroup _origSG;
		ISlottable _selectedSB;
		ISlotGroup _selectedSG;
		List<IInventoryItemInstance> itemCache = new List<IInventoryItemInstance>();
		ITransactionIconHandler iconHandler;
		ISlotsHolder sg2SlotsHolder;
		ISGActStateHandler sg1ActStateHandler;
		ISGActStateHandler sg2ActStateHandler;
		ISGTransactionHandler sg1TAHandler;
		ISGTransactionHandler sg2TAHandler;
		public StackTransaction(ISlottable pickedSB ,ISlottable selected, ITransactionManager tam): base(tam){
			_pickedSB = pickedSB;
			_origSG = pickedSB.GetSG();
			ISlotGroup sg1 = GetSG1();
			_selectedSB = selected;
			_selectedSG = _selectedSB.GetSG();
			ISlotGroup sg2 = GetSG2();
			IInventoryItemInstance cache = pickedSB.GetItem();
			cache.SetQuantity(pickedSB.GetItemHandler().GetPickedAmount());
			itemCache.Add(cache);
			iconHandler = tam.GetIconHandler();
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
		public override List<IInventoryItemInstance> GetMoved(){
			return itemCache;
		}
		public override void Indicate(){}
		public override void Execute(){
			sg1ActStateHandler.Remove();
			sg2ActStateHandler.Add();
			iconHandler.SetD1Destination(GetSG2(), sg2SlotsHolder.GetNewSlot(_pickedSB.GetItem()));
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			sg1TAHandler.UpdateSBs();
			sg2TAHandler.UpdateSBs();
			base.OnCompleteTransaction();
		}
	}
	public interface IStackTransaction:ISlotSystemTransaction{}
	public class TestStackTransaction: TestTransaction, IStackTransaction{}
}
