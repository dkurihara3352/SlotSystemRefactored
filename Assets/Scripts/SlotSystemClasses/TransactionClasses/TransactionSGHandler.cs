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
			ISlotGroup sg1 = GetSG1();
			ISlotGroup sg2 = GetSG2();
			if(sg2 != null && sg == sg2) _sg2Done = true;
			else if(sg1 != null && sg == sg1) _sg1Done = true;
			if(tamStateHandler.IsTransacting()){
				tamStateHandler.transactionCoroutine();
			}
		}
		public ISlotGroup GetSG1(){
			return _sg1;
		}
			ISlotGroup _sg1;
		public void SetSG1(ISlotGroup newSG1){
			ISlotGroup curSG1 = GetSG1();
			if(newSG1 == null || newSG1 != curSG1){
				if(curSG1 != null){
					ISSESelStateHandler sg1SelStateHandler = curSG1.GetSelStateHandler();
					sg1SelStateHandler.Activate();
				}
				_sg1 = newSG1;
				if(newSG1 != null)
					_sg1Done = false;
				else
					_sg1Done = true;
			}
		}
		public bool IsSG1Done(){
			return _sg1Done;
		}
			bool _sg1Done = true;
		public ISlotGroup GetSG2(){
			return _sg2;
		}
			ISlotGroup _sg2;
		public void SetSG2(ISlotGroup newSG2){
			ISlotGroup curSG2 = GetSG2();
			if(newSG2 == null || newSG2 != curSG2){
				if(curSG2 != null){
					ISSESelStateHandler sg2SelStateHandler = curSG2.GetSelStateHandler();
					sg2SelStateHandler.Activate();
				}
				_sg2 = newSG2;
				if(newSG2 != null){
					ISSESelStateHandler newSG2SelStateHandler = newSG2.GetSelStateHandler();
					newSG2SelStateHandler.Select();
				}
				if(newSG2 != null)
					_sg2Done = false;
				else
					_sg2Done = true;
			}
		}
		public bool IsSG2Done(){
			return _sg2Done;
		}
			bool _sg2Done = true;
	}
	public interface ITransactionSGHandler{
		void AcceptSGTAComp(ISlotGroup sg);
		ISlotGroup GetSG1();
		void SetSG1(ISlotGroup to);
		bool IsSG1Done();
		ISlotGroup GetSG2();
		void SetSG2(ISlotGroup sg);
		bool IsSG2Done();
	}
}
