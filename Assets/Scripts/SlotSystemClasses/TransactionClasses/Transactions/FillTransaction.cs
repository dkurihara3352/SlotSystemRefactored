using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class FillTransaction: AbsSlotSystemTransaction, IFillTransaction{
		ISlottable m_pickedSB;
		ISlotGroup m_selectedSG;
		ISlotGroup m_origSG;
		ITransactionIconHandler iconHandler;
		ISlotsHolder sg2SlotsHolder;
		ISGActStateHandler sg1ActStateHandler;
		ISGActStateHandler sg2ActStateHandler;
		ISGTransactionHandler sg1TAHandler;
		ISGTransactionHandler sg2TAHandler;
		public FillTransaction(ISlottable pickedSB, ISlotGroup selected, ITransactionManager tam):base(tam){
			m_pickedSB = pickedSB;
			m_selectedSG = selected;
			m_origSG = m_pickedSB.sg;
			iconHandler = tam.iconHandler;
			sg2SlotsHolder = sg2;
			sg1ActStateHandler = sg1;
			sg2ActStateHandler = sg2;
			sg1TAHandler = sg1;
			sg2TAHandler = sg2;
		}
		public override ISlotGroup sg1{
			get{return m_origSG;}
		}
		public override ISlotGroup sg2{
			get{return m_selectedSG;}
		}
		public override void Indicate(){}
		public override void Execute(){
			sg1ActStateHandler.Fill();
			sg2ActStateHandler.Fill();
			iconHandler.SetD1Destination(sg2, sg2SlotsHolder.GetNewSlot(m_pickedSB.item));
			sg1.OnActionExecute();
			sg2.OnActionExecute();
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			sg1TAHandler.OnCompleteSlotMovements();
			sg2TAHandler.OnCompleteSlotMovements();
			base.OnCompleteTransaction();
		}
	}
	public interface IFillTransaction: ISlotSystemTransaction{}
	public class TestFillTransaction: TestTransaction, IFillTransaction{}
}
