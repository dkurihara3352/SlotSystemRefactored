using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SortEngine : ISortEngine{
		ITransactionManager tam;
		ITransactionCache taCache;
		ITransactionSGHandler sgHandler;
		ITAMActStateHandler tamStateHandler;
		public SortEngine(ITransactionManager tam, ITransactionSGHandler sgHandler, ITAMActStateHandler tamStateHandler){
			this.tam = tam;
			this.sgHandler = sgHandler;
			this.tamStateHandler = tamStateHandler;
		}
		public void SortSG(ISlotGroup sg, SGSorter sorter){
			ISlotSystemTransaction sortTransaction = sortFA.MakeSortTA(sg, sorter);
			sg.taCache.SetTargetSB(sortTransaction.targetSB);
			sgHandler.SetSG1(sortTransaction.sg1);
			tam.SetTransaction(sortTransaction);
			tam.ExecuteTransaction();
		}
		ISortTransactionFactory sortFA{
			get{
				if(m_sortFA == null)
					m_sortFA = new SortTransactionFactory(tam, tamStateHandler);
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
		ITransactionManager tam;
		ITAMActStateHandler tamStateHandler;
		public SortTransactionFactory(ITransactionManager tam, ITAMActStateHandler tamStateHandler){
			this.tam = tam;
			this.tamStateHandler = tamStateHandler;
		}
		public ISortTransaction MakeSortTA(ISlotGroup sg, SGSorter sorter){
			return new SortTransaction(sg, sorter, tam, tamStateHandler);
		}
	}
	public interface ISortTransactionFactory{
		ISortTransaction MakeSortTA(ISlotGroup sg, SGSorter sorter);
	}
}
