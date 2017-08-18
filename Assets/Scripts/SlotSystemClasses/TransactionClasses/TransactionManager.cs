using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TransactionManager : ITransactionManager{
		public void ExecuteTransaction(){
			GetActStateHandler().Transact();
			transaction.Execute();
		}
		public void OnCompleteTransaction(){
			transaction.OnCompleteTransaction();
		}
		public void SetTransaction(ISlotSystemTransaction transaction){
			if(_transaction != transaction){
				_transaction = transaction;
				if(_transaction != null){
					_transaction.Indicate();
				}
			}
		}
			public ISlotSystemTransaction transaction{
				get{return _transaction;}
			}
			ISlotSystemTransaction _transaction;
		public ITransactionFactory GetTAFactory(){
			if(_taFactory == null)
				_taFactory = new TransactionFactory(this);
			return _taFactory;
		}
			ITransactionFactory _taFactory;
		/* Transaction Cache */
			public void ClearFields(){
				ITransactionIconHandler iconHandler = GetIconHandler();
				ITransactionSGHandler sgHandler = GetSGHandler();
				sgHandler.SetSG1(null);
				sgHandler.SetSG2(null);
				iconHandler.SetDIcon1(null);
				iconHandler.SetDIcon2(null);
				SetTransaction(null);
			}
			public void UpdateFields(ISlotSystemTransaction ta){
				ITransactionSGHandler sgHandler = GetSGHandler();
				sgHandler.SetSG1(ta.sg1);
				sgHandler.SetSG2(ta.sg2);
				SetTransaction(ta);
			}
		/* Sort Engine */
			public void SortSG(ISlotGroup sg, SGSorter sorter){
				sortEngine.SortSG(sg, sorter);
			}
			ISortEngine sortEngine{
				get{
					if(_sortEngine == null)
						_sortEngine = new SortEngine(this, GetSGHandler());
					return _sortEngine;
				}
			}
				ISortEngine _sortEngine;
				public void SetSortEngine(ISortEngine sortEngine){
					_sortEngine = sortEngine;
				}
		/* SGHandling */
			public ITransactionSGHandler GetSGHandler(){
				if(_sgHandler == null)
					_sgHandler = new TransactionSGHandler(GetActStateHandler());
				return _sgHandler;
			}
				ITransactionSGHandler _sgHandler;
			public void SetSGHandler(ITransactionSGHandler sgHandler){
				_sgHandler = sgHandler;
			}
		/* IconHandling */
			public ITransactionIconHandler GetIconHandler(){
				if(_iconHandler == null)
					_iconHandler = new TransactionIconHandler(GetActStateHandler());
				return _iconHandler;
			}
			public void SetIconHandler(ITransactionIconHandler iconHandler){
				_iconHandler = iconHandler;
			}
				ITransactionIconHandler _iconHandler;
		/* Other */
			public void Refresh(){
				GetActStateHandler().WaitForAction();
			}
		/* state handler */
			public ITAMActStateHandler GetActStateHandler(){
				if(_stateHandler != null)
					return _stateHandler;
				else
					throw new InvalidOperationException("tam state handler not set");
			}
				ITAMActStateHandler _stateHandler;
			public void SetActStateHandler(ITAMActStateHandler handler){
				_stateHandler = handler;
			}
			public IEnumeratorFake TransactionCoroutine(){
				ITransactionIconHandler iconHandler = GetIconHandler();
				ITransactionSGHandler sgHandler = GetSGHandler();
				bool flag = true;
				flag &= sgHandler.IsSG1Done();
				flag &= sgHandler.IsSG2Done();
				flag &= iconHandler.IsDIcon1Done();
				flag &= iconHandler.IsDIcon2Done();
				if(flag)
					GetActStateHandler().ExpireActProcess();
				return null;
			}
			public void WaitForAction(){
				GetActStateHandler().WaitForAction();
			}
	}
	public interface ITransactionManager{
		void ExecuteTransaction();
		void OnCompleteTransaction();
		void SetTransaction(ISlotSystemTransaction transaction);
		void UpdateFields(ISlotSystemTransaction ta);
		void SortSG(ISlotGroup sg, SGSorter sorter);
		void Refresh();
		IEnumeratorFake TransactionCoroutine();
		ITransactionFactory GetTAFactory();
		ITAMActStateHandler GetActStateHandler();
		ITransactionIconHandler GetIconHandler();
		ITransactionSGHandler GetSGHandler();
		void WaitForAction();
	}
}
