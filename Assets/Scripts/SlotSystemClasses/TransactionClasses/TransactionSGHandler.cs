using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TransactionSGHandler: ITransactionSGHandler{
		ITAMActStateHandler tamStateHandler;
		public TransactionSGHandler(ITAMActStateHandler tamStateHandler){
			this.tamStateHandler = tamStateHandler;
		}
		public void AcceptSGTAComp(ISlotGroup sg){
			if(sg2 != null && sg == sg2) m_sg2Done = true;
			else if(sg1 != null && sg == sg1) m_sg1Done = true;
			if(tamStateHandler.isTransacting){
				tamStateHandler.transactionCoroutine();
			}
		}
		public ISlotGroup sg1{
			get{return m_sg1;}
		}
			ISlotGroup m_sg1;
		public void SetSG1(ISlotGroup to){
			if(to == null || to != sg1){
				if(sg1 != null)
					sg1.Activate();
				this.m_sg1 = to;
				if(sg1 != null)
					m_sg1Done = false;
				else
					m_sg1Done = true;
			}
		}
		public bool sg1Done{
			get{return m_sg1Done;}
		}
			bool m_sg1Done = true;
		public ISlotGroup sg2{
			get{return m_sg2;}
		}
			ISlotGroup m_sg2;
		public void SetSG2(ISlotGroup sg){
			if(sg == null || sg != sg2){
				if(sg2 != null)
					sg2.Activate();
				this.m_sg2 = sg;
				if(sg2 != null)
					sg.Select();
				if(sg2 != null)
					m_sg2Done = false;
				else
					m_sg2Done = true;
			}
		}
		public bool sg2Done{
			get{return m_sg2Done;}
		}
			bool m_sg2Done = true;
	}
	public interface ITransactionSGHandler{
		void AcceptSGTAComp(ISlotGroup sg);
		ISlotGroup sg1{get;}
		void SetSG1(ISlotGroup to);
		bool sg1Done{get;}
		ISlotGroup sg2{get;}
		void SetSG2(ISlotGroup sg);
		bool sg2Done{get;}
	}
}
