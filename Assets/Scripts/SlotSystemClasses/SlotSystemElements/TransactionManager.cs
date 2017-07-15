using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface TransactionManager{
		ISlotSystemTransaction transaction{get;}
		void AcceptSGTAComp(ISlotGroup sg);
		void AcceptDITAComp(DraggedIcon di);
		ISlottable pickedSB{get;}
		ISlottable targetSB{get;}
		ISlotGroup sg1{get;}
		ISlotGroup sg2{get;}
		DraggedIcon dIcon1{get;}
		DraggedIcon dIcon2{get;}
		ISlotSystemElement hovered{get;}
		void UpdateTransaction();
		void CreateTransactionResults();
		void ReferToTAAndUpdateSelState(ISlotGroup sg);
		ISlotSystemTransaction GetTransaction(ISlottable pickedSB, ISlotSystemElement hovered);
		
		void SetTransaction(ISlotSystemTransaction transaction);	
		void SetPickedSB(ISlottable sb);
		void SetTargetSB(ISlottable sb);
		void SetSG1(ISlotGroup sg);
		bool sg1Done{get;}
		void SetSG2(ISlotGroup sg);
		bool sg2Done{get;}
		void SetDIcon1(DraggedIcon di);
		bool dIcon1Done{get;}
		void SetDIcon2(DraggedIcon di);
		bool dIcon2Done{get;}
		void SetHovered(ISlotSystemElement ele);
	}
}