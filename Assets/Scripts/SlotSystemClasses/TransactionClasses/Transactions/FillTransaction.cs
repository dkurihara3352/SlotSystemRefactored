using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class FillTransaction: AbsSlotSystemTransaction, IFillTransaction{
		public ISlottable m_pickedSB;
		public ISlotGroup m_selectedSG;
		public ISlotGroup m_origSG;
		public FillTransaction(ISlottable pickedSB, ISlotGroup selected){
			m_pickedSB = pickedSB;
			m_selectedSG = selected;
			m_origSG = m_pickedSB.sg;
			tam = pickedSB.tam;
		}
		public override ISlotGroup sg1{get{return m_origSG;}}
		public override ISlotGroup sg2{get{return m_selectedSG;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1.Fill();
			sg2.Fill();
			tam.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.item));
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
