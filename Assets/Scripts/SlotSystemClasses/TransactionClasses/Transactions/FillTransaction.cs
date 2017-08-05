using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class FillTransaction: AbsSlotSystemTransaction, IFillTransaction{
		ISlottable m_pickedSB;
		ISlotGroup m_selectedSG;
		ISlotGroup m_origSG;
		ITransactionIconHandler iconHandler;
		public FillTransaction(ISlottable pickedSB, ISlotGroup selected, ITransactionManager tam, ITransactionIconHandler iconHandler, ITAMActStateHandler tamStateHandler):base(tam, tamStateHandler){
			m_pickedSB = pickedSB;
			m_selectedSG = selected;
			m_origSG = m_pickedSB.sg;
			this.iconHandler = iconHandler;
		}
		public override ISlotGroup sg1{get{return m_origSG;}}
		public override ISlotGroup sg2{get{return m_selectedSG;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1.Fill();
			sg2.Fill();
			iconHandler.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.item));
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
	public interface IFillTransaction: ISlotSystemTransaction{}
	public class TestFillTransaction: TestTransaction, IFillTransaction{}
}
