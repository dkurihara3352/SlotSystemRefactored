using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class ReorderTransaction: AbsSlotSystemTransaction, IReorderTransaction{
		public ISlottable m_pickedSB;
		public ISlottable m_selectedSB;
		public ISlotGroup m_origSG;
		public ReorderTransaction(ISlottable pickedSB, ISlottable selected){
			m_pickedSB = pickedSB;
			m_selectedSB = selected;
			m_origSG = m_pickedSB.sg;
			tam = pickedSB.tam;
		}
		public override ISlottable targetSB{get{return m_selectedSB;}}
		public override ISlotGroup sg1{get{return m_origSG;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1.Reorder();
			tam.dIcon1.SetDestination(sg1, sg1.GetNewSlot(m_pickedSB.item));
			sg1.OnActionExecute();
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			sg1.OnCompleteSlotMovements();
			base.OnCompleteTransaction();
		}
	}
	public interface IReorderTransaction: ISlotSystemTransaction{}
	public class TestReorderTransaction: TestTransaction, IReorderTransaction{}
}
