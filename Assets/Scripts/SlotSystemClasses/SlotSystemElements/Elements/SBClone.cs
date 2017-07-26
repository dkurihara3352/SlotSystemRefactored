using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SBClone: Slottable{
		public override ISlotSystemElement parent{
			get{return m_parent;}
		}ISlotSystemElement m_parent;
		public override bool isDeactivated{get{return m_isDeactivated;}} bool m_isDeactivated;
		public override bool isFocused{get{return m_isFocused;}} bool m_isFocused;
		public override bool isDefocused{get{return m_isDefocused;}} bool m_isDefocused;
		public override bool isSelected{get{return m_isSelected;}} bool m_isSelected;
		public override bool wasDeactivated{get{return m_wasDeactivated;}} bool m_wasDeactivated;
		public override bool wasFocused{get{return m_wasFocused;}} bool m_wasFocused;
		public override bool wasDefocused{get{return m_wasDefocused;}} bool m_wasDefocused;
		public override bool wasSelected{get{return m_wasSelected;}} bool m_wasSelected;

		public override bool isWaitingForAction{get{return m_isWaitingForAction;}} bool m_isWaitingForAction;
		public override bool isWaitingForPointerUp{get{return m_isWaitingForPointerUp;}} bool m_isWaitingForPointerUp;
		public override bool isWaitingForPickUp{get{return m_isWaitingForPickUp;}} bool m_isWaitingForPickUp;
		public override bool isWaitingForNextTouch{get{return m_isWaitingForNextTouch;}} bool m_isWaitingForNextTouch;
		public override bool isPickingUp{get{return m_isPickingUp;}} bool m_isPickingUp;
		public override bool isRemoving{get{return m_isRemoving;}} bool m_isRemoving;
		public override bool isAdding{get{return m_isAdding;}} bool m_isAdding;
		public override bool isMovingWithin{get{return m_isMovingWithin;}} bool m_isMovingWithin;
		public override bool wasWaitingForAction{get{return m_wasWaitingForAction;}} bool m_wasWaitingForAction;
		public override bool wasWaitingForPointerUp{get{return m_wasWaitingForPointerUp;}} bool m_wasWaitingForPointerUp;
		public override bool wasWaitingForPickUp{get{return m_wasWaitingForPickUp;}} bool m_wasWaitingForPickUp;
		public override bool wasWaitingForNextTouch{get{return m_wasWaitingForNextTouch;}} bool m_wasWaitingForNextTouch;
		public override bool wasPickingUp{get{return m_wasPickingUp;}} bool m_wasPickingUp;
		public override bool wasRemoving{get{return m_wasRemoving;}} bool m_wasRemoving;
		public override bool wasAdding{get{return m_wasAdding;}} bool m_wasAdding;
		public override bool wasMovingWithin{get{return m_wasMovingWithin;}} bool m_wasMovingWithin;

		public override bool isEquipped{get{return m_isEquipped;}} bool m_isEquipped;
		public override bool isUnequipped{get{return m_isUnequipped;}} bool m_isUnequipped;
		public override bool wasEquipped{get{return m_wasEquipped;}} bool m_wasEquipped;
		public override bool wasUnequipped{get{return m_wasUnequipped;}} bool m_wasUnequipped;
		
		public override bool isMarked{get{return m_isMarked;}} bool m_isMarked;
		public override bool isUnmarked{get{return m_isUnmarked;}} bool m_isUnmarked;
		public override bool wasMarked{get{return m_wasMarked;}} bool m_wasMarked;
		public override bool wasUnmarked{get{return m_wasUnmarked;}} bool m_wasUnmarked;
		
		public void Initialize(ISlottable orig){
			this.delayed = orig.delayed;
			this.SetItem(orig.itemInst);
			m_parent = orig.sg;
			SetSSM(orig.ssm);
			m_isDeactivated = orig.isDeactivated;
			m_isFocused = orig.isFocused;
			m_isDefocused = orig.isDefocused;
			m_isSelected = orig.isSelected;
			m_wasDeactivated = orig.wasDeactivated;
			m_wasFocused = orig.wasFocused;
			m_wasDefocused = orig.wasDefocused;
			m_wasSelected = orig.wasSelected;

			m_isWaitingForAction = orig.isWaitingForAction;
			m_isWaitingForPointerUp = orig.isWaitingForPointerUp;
			m_isWaitingForPickUp = orig.isWaitingForPickUp;
			m_isWaitingForNextTouch = orig.isWaitingForNextTouch;
			m_isPickingUp = orig.isPickingUp;
			m_isRemoving = orig.isRemoving;
			m_isAdding = orig.isAdding;
			m_isMovingWithin = orig.isMovingWithin;
			m_wasWaitingForAction = orig.wasWaitingForAction;
			m_wasWaitingForPointerUp = orig.wasWaitingForPointerUp;
			m_wasWaitingForPickUp = orig.wasWaitingForPickUp;
			m_wasWaitingForNextTouch = orig.wasWaitingForNextTouch;
			m_wasPickingUp = orig.wasPickingUp;
			m_wasRemoving = orig.wasRemoving;
			m_wasAdding = orig.wasAdding;
			m_wasMovingWithin = orig.wasMovingWithin;

			m_isEquipped = orig.isEquipped;
			m_isUnequipped = orig.isUnequipped;
			m_wasEquipped = orig.wasEquipped;
			m_wasUnequipped = orig.wasUnequipped;
			
			m_isMarked = orig.isMarked;
			m_isUnmarked = orig.isUnmarked;
			m_wasMarked = orig.wasMarked;
			m_wasUnmarked = orig.wasUnmarked;
			
			SetSlotID(orig.slotID);
			SetNewSlotID(orig.newSlotID);
		}
	}
}
