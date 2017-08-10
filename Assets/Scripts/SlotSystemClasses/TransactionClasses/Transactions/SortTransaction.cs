using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SortTransaction: AbsSlotSystemTransaction, ISortTransaction{
		ISlotGroup m_selectedSG;
		SGSorter m_sorter;
		ISorterHandler sorterHandler;
		ISGActStateHandler actStateHandler;
		ISGTransactionHandler taHandler;
		public SortTransaction(ISlotGroup sg, SGSorter sorter, ITransactionManager tam): base(tam){
			m_selectedSG = sg;
			m_sorter = sorter;
			sorterHandler = sg1;
			actStateHandler = sg1;
			taHandler = sg1;
		}
		public override ISlotGroup sg1{get{return m_selectedSG;}}
		public override void Indicate(){}
		public override void Execute(){
			sorterHandler.SetSorter(m_sorter);
			actStateHandler.Sort();
			sg1.OnActionExecute();
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			taHandler.OnCompleteSlotMovements();
			base.OnCompleteTransaction();
		}
	}
	public interface ISortTransaction: ISlotSystemTransaction{}
	public class TestSortTransaction: TestTransaction, ISortTransaction{}
}
