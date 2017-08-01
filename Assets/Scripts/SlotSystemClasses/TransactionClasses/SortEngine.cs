using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SortEngine : ISortEngine{
		ITransactionManager tam;
		public SortEngine(ITransactionManager tam){
			this.tam = tam;
		}
		public void SortSG(ISlotGroup sg, SGSorter sorter){
			ISlotSystemTransaction sortTransaction = sortFA.MakeSortTA(sg, sorter);
			tam.SetTargetSB(sortTransaction.targetSB);
			tam.SetSG1(sortTransaction.sg1);
			tam.SetTransaction(sortTransaction);
			tam.ExecuteTransaction();
		}
		ISortTransactionFactory sortFA{
			get{
				if(m_sortFA == null)
					m_sortFA = new SortTransactionFactory();
				return m_sortFA;
			}
		}
			ISortTransactionFactory m_sortFA;
			public void SetSortTransactionFactory(ISortTransactionFactory fa){
				m_sortFA = fa;
			}
	}
	public interface ISortEngine{
		void SortSG(ISlotGroup sg, SGSorter sorter);
	}
	public class SortTransactionFactory: ISortTransactionFactory{
		public ISortTransaction MakeSortTA(ISlotGroup sg, SGSorter sorter){
			return new SortTransaction(sg, sorter);
		}
	}
	public interface ISortTransactionFactory{
		ISortTransaction MakeSortTA(ISlotGroup sg, SGSorter sorter);
	}
}
