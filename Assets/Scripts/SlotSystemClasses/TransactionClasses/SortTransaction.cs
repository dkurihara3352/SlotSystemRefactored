using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SortTransaction: AbsSlotSystemTransaction{
		public SlotGroup m_selectedSG;
		public SGSorter m_sorter;
		public SortTransaction(SlotGroup sg, SGSorter sorter){
			m_selectedSG = sg;
			m_sorter = sorter;
		}
		public SortTransaction(SortTransaction orig){
			this.m_selectedSG = Util.CloneSG(orig.m_selectedSG);
			this.m_sorter = orig.m_sorter;
		}
		public override SlotGroup sg1{get{return m_selectedSG;}}
		public override void Indicate(){}
		public override void Execute(){
			sg1.SetSorter(m_sorter);
			sg1.SetActState(SlotGroup.sortState);
			sg1.OnActionExecute();
			base.Execute();
		}
		public override void OnComplete(){
			sg1.OnCompleteSlotMovements();
			base.OnComplete();
		}
	}
}
