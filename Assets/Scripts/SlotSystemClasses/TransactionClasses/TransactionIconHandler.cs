using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TransactionIconHandler: ITransactionIconHandler{
		ITAMActStateHandler tamStateHandler;
		public TransactionIconHandler(ITAMActStateHandler tamStateHandler){
			this.tamStateHandler = tamStateHandler;
		}
		public void AcceptDITAComp(DraggedIcon di){
			if(dIcon2 != null && di == dIcon2) m_dIcon2Done = true;
			else if(dIcon1 != null && di == dIcon1) m_dIcon1Done = true;
			if(tamStateHandler.isTransacting){
				tamStateHandler.transactionCoroutine();
			}
		}
		public virtual DraggedIcon dIcon1{
			get{return m_dIcon1;}
		}
			DraggedIcon m_dIcon1;
		public virtual void SetDIcon1(DraggedIcon di){
			m_dIcon1 = di;
			if(m_dIcon1 == null)
				m_dIcon1Done = true;
			else
				m_dIcon1Done = false;
		}
		public bool dIcon1Done{
			get{return m_dIcon1Done;}
		}
			bool m_dIcon1Done = true;

		public virtual DraggedIcon dIcon2{
			get{return m_dIcon2;}
		}
			DraggedIcon m_dIcon2;
		public virtual void SetDIcon2(DraggedIcon di){
			m_dIcon2 = di;
			if(m_dIcon2 == null)
				m_dIcon2Done = true;
			else
				m_dIcon2Done = false;
		}
		public bool dIcon2Done{
			get{return m_dIcon2Done;}
		}
			bool m_dIcon2Done = true;
	}
	public interface ITransactionIconHandler{
		void AcceptDITAComp(DraggedIcon di);
		DraggedIcon dIcon1{get;}
		void SetDIcon1(DraggedIcon di);
		bool dIcon1Done{get;}
		DraggedIcon dIcon2{get;}
		void SetDIcon2(DraggedIcon di);
		bool dIcon2Done{get;}
	}
}

