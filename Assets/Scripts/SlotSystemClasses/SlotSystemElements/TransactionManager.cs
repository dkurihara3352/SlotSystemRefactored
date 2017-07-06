using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface TransactionManager{
		SlotSystemTransaction transaction{get;}
		void AcceptSGTAComp(SlotGroup sg);
		void AcceptDITAComp(DraggedIcon di);
		Slottable pickedSB{get;}
		Slottable targetSB{get;}
		SlotGroup sg1{get;}
		SlotGroup sg2{get;}
		DraggedIcon dIcon1{get;}
		DraggedIcon dIcon2{get;}
		SlotSystemElement hovered{get;}
		void UpdateTransaction();
		SlotSystemTransaction GetTransaction(Slottable pickedSB, SlotSystemElement hovered);
	}
}