using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class ReorderTransaction: AbsSlotSystemTransaction, IReorderTransaction{
		ISlottable m_pickedSB;
		ISlottable m_selectedSB;
		ISlotGroup m_origSG;
		ITransactionIconHandler iconHandler;
		ISlotsHolder sg1SlotsHolder;
		ISGActStateHandler sg1ActStateHandler;
		ISGTransactionHandler sg1TAHandler;
		public ReorderTransaction(ISlottable pickedSB, ISlottable selected, ITransactionManager tam): base(tam){
			m_pickedSB = pickedSB;
			m_selectedSB = selected;
			m_origSG = m_pickedSB.sg;
			iconHandler = tam.iconHandler;
			sg1SlotsHolder = sg1;
			sg1ActStateHandler = sg1;
			sg1TAHandler = sg1;
		}
		public override ISlottable targetSB{get{return m_selectedSB;}}
		public override ISlotGroup sg1{get{return m_origSG;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1ActStateHandler.Reorder();
			iconHandler.SetD1Destination(sg1, sg1SlotsHolder.GetNewSlot(m_pickedSB.item));
			sg1.OnActionExecute();
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			sg1TAHandler.OnCompleteSlotMovements();
			base.OnCompleteTransaction();
		}
	}
	public interface IReorderTransaction: ISlotSystemTransaction{}
	public class TestReorderTransaction: TestTransaction, IReorderTransaction{}
}
