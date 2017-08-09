using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TransactionIconHandler: ITransactionIconHandler{
		ITAMActStateHandler tamStateHandler;
		public TransactionIconHandler(ITAMActStateHandler tamStateHandler){
			this.tamStateHandler = tamStateHandler;
		}
		public void AcceptDITAComp(ISlottable sb){
			if(dIcon2 != null && sb == dIcon2.sb) m_dIcon2Done = true;
			else if(dIcon1 != null && sb == dIcon1.sb) m_dIcon1Done = true;
			if(tamStateHandler.isTransacting){
				tamStateHandler.transactionCoroutine();
			}
		}
		public virtual DraggedIcon dIcon1{
			get{return m_dIcon1;}
		}
			DraggedIcon m_dIcon1;
		public virtual void SetDIcon1(ISlottable sb){
			if(sb != null){
				DraggedIcon di = new DraggedIcon(sb, this);
				m_dIcon1 = di;
			}else
				m_dIcon1 = null;
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
		public virtual void SetDIcon2(ISlottable sb){
			if(sb != null){
				DraggedIcon di = new DraggedIcon(sb, this);
				m_dIcon2 = di;
			}else
				m_dIcon2 = null;
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
		void AcceptDITAComp(ISlottable sb);
		DraggedIcon dIcon1{get;}
		void SetDIcon1(ISlottable sb);
		bool dIcon1Done{get;}
		DraggedIcon dIcon2{get;}
		void SetDIcon2(ISlottable sb);
		bool dIcon2Done{get;}
	}
}

