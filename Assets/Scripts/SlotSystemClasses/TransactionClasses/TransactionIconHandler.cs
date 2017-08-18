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
			DraggedIcon dIcon1 = GetDIcon1();
			DraggedIcon dIcon2 = GetDIcon2();
			if(dIcon2 != null && sb == dIcon2.sb) m_dIcon2Done = true;
			else if(dIcon1 != null && sb == dIcon1.sb) m_dIcon1Done = true;
			if(tamStateHandler.IsTransacting()){
				tamStateHandler.transactionCoroutine();
			}
		}
		public virtual DraggedIcon GetDIcon1(){
			return m_dIcon1;
		}
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
			DraggedIcon m_dIcon1;
		public bool IsDIcon1Done(){
			return m_dIcon1Done;
		}
			bool m_dIcon1Done = true;
		public void SetD1Destination(ISlotGroup sg, Slot slot){
			GetDIcon1().SetDestination(sg, slot);
		}
		public virtual DraggedIcon GetDIcon2(){
			return m_dIcon2;
		}
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
			DraggedIcon m_dIcon2;
		public bool IsDIcon2Done(){
			return m_dIcon2Done;
		}
		public void SetD2Destination(ISlotGroup sg, Slot slot){
			GetDIcon2().SetDestination(sg, slot);
		}
			bool m_dIcon2Done = true;
	}
	public interface ITransactionIconHandler{
		void AcceptDITAComp(ISlottable sb);
		DraggedIcon GetDIcon1();
		void SetDIcon1(ISlottable sb);
		bool IsDIcon1Done();
		void SetD1Destination(ISlotGroup sg, Slot slot);
		DraggedIcon GetDIcon2();
		void SetDIcon2(ISlottable sb);
		bool IsDIcon2Done();
		void SetD2Destination(ISlotGroup sg, Slot slot);
	}
}

