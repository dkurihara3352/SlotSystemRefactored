using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SwapTransaction: AbsSlotSystemTransaction, ISwapTransaction{
		public ISlottable m_pickedSB;
		public ISlotGroup m_origSG;
		public ISlottable m_selectedSB;
		public ISlotGroup m_selectedSG;
		public SwapTransaction(ISlottable pickedSB, ISlottable selected){
			m_pickedSB = pickedSB;
			m_selectedSB = selected;
			m_origSG = m_pickedSB.sg;
			m_selectedSG = m_selectedSB.sg;
			tam = m_pickedSB.tam;
		}
		public override ISlottable targetSB{get{return m_selectedSB;}}
		public override ISlotGroup sg1{get{return m_origSG;}}
		public override ISlotGroup sg2{get{return m_selectedSG;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1.Swap();
			sg2.Swap();
			tam.dIcon1.SetDestination(sg2, sg2.GetNewSlot(m_pickedSB.item));
			DraggedIcon di2 = new DraggedIcon(targetSB);
			tam.SetDIcon2(di2);
			tam.dIcon2.SetDestination(sg1, sg1.GetNewSlot(targetSB.item));
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
