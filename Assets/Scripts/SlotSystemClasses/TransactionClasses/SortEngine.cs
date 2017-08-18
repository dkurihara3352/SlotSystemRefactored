﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SortEngine : ISortEngine{
		ITransactionManager tam;
		ITransactionCache taCache;
		ITransactionSGHandler sgHandler;
		public SortEngine(ITransactionManager tam, ITransactionSGHandler sgHandler){
			this.tam = tam;
			this.sgHandler = sgHandler;
		}
		public void SortSG(ISlotGroup sg, SGSorter sorter){
			ISlotSystemTransaction sortTransaction = sortFA.MakeSortTA(sg, sorter);
			sg.GetTAC().SetTargetSB(sortTransaction.targetSB);
			sgHandler.SetSG1(sortTransaction.sg1);
			tam.SetTransaction(sortTransaction);
			tam.ExecuteTransaction();
		}
		ISortTransactionFactory sortFA{
			get{
				if(m_sortFA == null)
					m_sortFA = new SortTransactionFactory(tam);
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
		public SortTransactionFactory(ITransactionManager tam){
			this.tam = tam;
		}
		public ISortTransaction MakeSortTA(ISlotGroup sg, SGSorter sorter){
			return new SortTransaction(sg, sorter, tam);
		}
	}
	public interface ISortTransactionFactory{
		ISortTransaction MakeSortTA(ISlotGroup sg, SGSorter sorter);
	}
}
