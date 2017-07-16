using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface ISlotSystemTransaction{
		ISlottable targetSB{get;}
		ISlotGroup sg1{get;}
		ISlotGroup sg2{get;}
		List<InventoryItemInstance> moved{get;}
		void Indicate();
		void Execute();
		void OnComplete();
	}
	public class TestTransaction: ISlotSystemTransaction{
		public ISlottable targetSB{get;}
		public ISlotGroup sg1{get;}
		public ISlotGroup sg2{get;}
		public List<InventoryItemInstance> moved{get;}
		public void Indicate(){}
		public void Execute(){}
		public void OnComplete(){}
	}
}
