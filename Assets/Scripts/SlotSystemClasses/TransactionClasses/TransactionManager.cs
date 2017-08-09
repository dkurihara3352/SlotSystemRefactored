using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TransactionManager : ITransactionManager{
		public void ExecuteTransaction(){
			actStateHandler.Transact();
			transaction.Execute();
		}
		public void OnCompleteTransaction(){
			transaction.OnCompleteTransaction();
		}
		public void SetTransaction(ISlotSystemTransaction transaction){
			if(m_transaction != transaction){
				m_transaction = transaction;
				if(m_transaction != null){
					m_transaction.Indicate();
				}
			}
		}
			public ISlotSystemTransaction transaction{
				get{return m_transaction;}
			}
			ISlotSystemTransaction m_transaction;
		public ITransactionFactory taFactory{
			get{
				if(_taFactory == null)
					_taFactory = new TransactionFactory(this);
				return _taFactory;
			}
		}
			ITransactionFactory _taFactory;
		/* Transaction Cache */
			public void ClearFields(){
				sgHandler.SetSG1(null);
				sgHandler.SetSG2(null);
				iconHandler.SetDIcon1(null);
				iconHandler.SetDIcon2(null);
				SetTransaction(null);
			}
			public void UpdateFields(ISlotSystemTransaction ta){
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
					if(m_sortEngine == null)
						m_sortEngine = new SortEngine(this, sgHandler);
					return m_sortEngine;
				}
			}
				ISortEngine m_sortEngine;
				public void SetSortEngine(ISortEngine sortEngine){
					m_sortEngine = sortEngine;
				}
		/* SGHandling */
			public ITransactionSGHandler sgHandler{
				get{
					if(m_sgHandler == null)
						m_sgHandler = new TransactionSGHandler(actStateHandler);
					return m_sgHandler;
				}
			}
				ITransactionSGHandler m_sgHandler;
			public void SetSGHandler(ITransactionSGHandler sgHandler){
				m_sgHandler = sgHandler;
			}
		/* IconHandling */
			public ITransactionIconHandler iconHandler{
				get{
					if(m_iconHandler == null)
						m_iconHandler = new TransactionIconHandler(actStateHandler);
					return m_iconHandler;
				}
			}
				ITransactionIconHandler m_iconHandler;
			public void SetIconHandler(ITransactionIconHandler iconHandler){
				m_iconHandler = iconHandler;
			}
		/* Other */
			public void Refresh(){
				actStateHandler.WaitForAction();
			}
		/* state handler */
			public ITAMActStateHandler actStateHandler{
				get{
					if(_stateHandler != null)
						return _stateHandler;
					else
						throw new InvalidOperationException("tam state handler not set");
				}
			}
				ITAMActStateHandler _stateHandler;
			public void SetActStateHandler(ITAMActStateHandler handler){
				_stateHandler = handler;
			}
			public IEnumeratorFake transactionCoroutine(){
				bool flag = true;
				flag &= sgHandler.sg1Done;
				flag &= sgHandler.sg2Done;
				flag &= iconHandler.dIcon1Done;
				flag &= iconHandler.dIcon2Done;
				if(flag)
					actStateHandler.ExpireActProcess();
				return null;
			}
			public void WaitForAction(){
				actStateHandler.WaitForAction();
			}
	}
	public interface ITransactionManager{
		void ExecuteTransaction();
		void OnCompleteTransaction();
		void SetTransaction(ISlotSystemTransaction transaction);
		void UpdateFields(ISlotSystemTransaction ta);
		void SortSG(ISlotGroup sg, SGSorter sorter);
		void Refresh();
		IEnumeratorFake transactionCoroutine();
		ITransactionFactory taFactory{get;}
		ITAMActStateHandler actStateHandler{get;}
		ITransactionIconHandler iconHandler{get;}
		ITransactionSGHandler sgHandler{get;}
		void WaitForAction();
	}
}
