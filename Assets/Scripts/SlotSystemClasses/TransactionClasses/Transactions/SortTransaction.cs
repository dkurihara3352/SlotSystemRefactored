using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SortTransaction: AbsSlotSystemTransaction, ISortTransaction{
		public ISlotGroup m_selectedSG;
		public SGSorter m_sorter;
		public SortTransaction(ISlotGroup sg, SGSorter sorter){
			m_selectedSG = sg;
			m_sorter = sorter;
			tam = m_selectedSG.tam;
		}
		public override ISlotGroup sg1{get{return m_selectedSG;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1.SetSorter(m_sorter);
			sg1.Sort();
			sg1.OnActionExecute();
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			sg1.OnCompleteSlotMovements();
			base.OnCompleteTransaction();
		}
	}
	public interface ISortTransaction: ISlotSystemTransaction{}
	public class TestSortTransaction: TestTransaction, ISortTransaction{}
}
