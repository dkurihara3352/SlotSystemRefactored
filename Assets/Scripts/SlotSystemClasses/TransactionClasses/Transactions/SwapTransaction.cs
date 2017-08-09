using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SwapTransaction: AbsSlotSystemTransaction, ISwapTransaction{
		ISlottable m_pickedSB;
		ISlotGroup m_origSG;
		ISlottable m_selectedSB;
		ISlotGroup m_selectedSG;
		ITransactionIconHandler iconHandler;
		ISlotsHolder sg1SlotsHolder;
		ISlotsHolder sg2SlotsHolder;
		public SwapTransaction(ISlottable pickedSB, ISlottable selected, ITransactionManager tam): base(tam){
			m_pickedSB = pickedSB;
			m_selectedSB = selected;
			m_origSG = m_pickedSB.sg;
			m_selectedSG = m_selectedSB.sg;
			this.iconHandler = tam.iconHandler;
			sg1SlotsHolder = sg1;
			sg2SlotsHolder = sg2;
		}
		public override ISlottable targetSB{get{return m_selectedSB;}}
		public override ISlotGroup sg1{get{return m_origSG;}}
		public override ISlotGroup sg2{get{return m_selectedSG;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1.Swap();
			sg2.Swap();
			iconHandler.dIcon1.SetDestination(sg2, sg2SlotsHolder.GetNewSlot(m_pickedSB.item));
			iconHandler.SetDIcon2(targetSB);
			iconHandler.dIcon2.SetDestination(sg1, sg1SlotsHolder.GetNewSlot(targetSB.item));
			sg1.OnActionExecute();
			sg2.OnActionExecute();
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			sg1.OnCompleteSlotMovements();
			sg2.OnCompleteSlotMovements();
			base.OnCompleteTransaction();
		}
	}
	public interface ISwapTransaction: ISlotSystemTransaction{}
	public class TestSwapTransaction: TestTransaction, ISwapTransaction{}
}
