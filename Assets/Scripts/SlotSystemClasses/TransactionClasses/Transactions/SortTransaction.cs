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
		ITransactionCache taCache;
		public SortTransaction(ISlotGroup sg, SGSorter sorter, ITransactionManager tam): base(tam){
			_selectedSG = sg;
			ISlotGroup sg1 = GetSG1();
			_sorter = sorter;
			sorterHandler = sg1.GetSorterHandler();
			actStateHandler = sg1.GetSGActStateHandler();
			taHandler = sg1.GetSGTAHandler();
			taCache = sg.GetTAC();
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
		public void SetTargetSBOnTAC(){
			taCache.SetTargetSB(GetTargetSB());
		}
	}
	public interface ISortTransaction: ISlotSystemTransaction{
		void SetTargetSBOnTAC();
	}
	public class TestSortTransaction: TestTransaction, ISortTransaction{
		public void SetTargetSBOnTAC(){}
	}
}
