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
		ISGActStateHandler sg1ActStateHandler;
		ISGActStateHandler sg2ActStateHandler;
		ISGTransactionHandler sg1TAHandler;
		ISGTransactionHandler sg2TAHandler;
		public SwapTransaction(ISlottable pickedSB, ISlottable selected, ITransactionManager tam): base(tam){
			m_pickedSB = pickedSB;
			m_selectedSB = selected;
			m_origSG = m_pickedSB.GetSG();
			m_selectedSG = m_selectedSB.GetSG();
			iconHandler = tam.iconHandler;
			sg1SlotsHolder = sg1;
			sg2SlotsHolder = sg2;
			sg1ActStateHandler = sg1;
			sg2ActStateHandler = sg2;
			sg1TAHandler = sg1;
			sg2TAHandler = sg2;
		}
		public override ISlottable targetSB{get{return m_selectedSB;}}
		public override ISlotGroup sg1{get{return m_origSG;}}
		public override ISlotGroup sg2{get{return m_selectedSG;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1ActStateHandler.Swap();
			sg2ActStateHandler.Swap();
			iconHandler.SetD1Destination(sg2, sg2SlotsHolder.GetNewSlot(m_pickedSB.GetItem()));
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
