using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface ISlotSystemTransaction{
		ISlottable GetTargetSB();
		ISlotGroup GetSG1();
		ISlotGroup GetSG2();
		List<IInventoryItemInstance> GetMoved();
		void Indicate();
		void Execute();
		void OnCompleteTransaction();
	}
	public class TestTransaction: ISlotSystemTransaction{
		public ISlottable GetTargetSB(){return null;}
		public ISlotGroup GetSG1(){return null;}
		public ISlotGroup GetSG2(){return null;}
		public List<IInventoryItemInstance> GetMoved(){return null;;}
		public void Indicate(){}
		public void Execute(){}
		public void OnCompleteTransaction(){}
	}
}
