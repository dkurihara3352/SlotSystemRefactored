using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SortTransaction: AbsSlotSystemTransaction, ISortTransaction{
		ISlotGroup _selectedSG;
		SGSorter _sorter;
		ISorterHandler sorterHandler;
		ISGActStateHandler actStateHandler;
		ISGTransactionHandler taHandler;
		public SortTransaction(ISlotGroup sg, SGSorter sorter, ITransactionManager tam): base(tam){
			_selectedSG = sg;
			_sorter = sorter;
			sorterHandler = GetSG1();
			actStateHandler = GetSG1();
			taHandler = GetSG1();
		}
		public override ISlotGroup GetSG1(){
			return _selectedSG;
		}
		public override void Indicate(){}
		public override void Execute(){
			sorterHandler.SetSorter(_sorter);
			actStateHandler.Sort();
			GetSG1().OnActionExecute();
			base.Execute();
		}
		public override void OnCompleteTransaction(){
			taHandler.UpdateSBs();
			base.OnCompleteTransaction();
		}
	}
	public interface ISortTransaction: ISlotSystemTransaction{}
	public class TestSortTransaction: TestTransaction, ISortTransaction{}
}
