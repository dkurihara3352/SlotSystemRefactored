using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public interface IEquippableSlot: IInventoryItemSlot{
		ISBEqpStateHandler EqpStateHandler();
		bool IsEquipped();
		bool IsUnequipped();
		void Equip();
		void Unequip();
		void ClearCurEqpState();
		void UpdateEquipState();
	}
	public class EquippableSlot : InventoryItemSlot, IEquippableSlot {
		public EquippableSlot(RectTransformFake rectTrans, ISBSelStateRepo selStateRepo, ITapCommand tapCommand, ISSEEventCommandsRepo eventCommandsRepo, InventoryItemInstance itemInstance, bool leavesGhost): base(rectTrans, selStateRepo, tapCommand, eventCommandsRepo, itemInstance, leavesGhost){
			SetEqpStateHandler(new SBEqpStateHandler(this));
		}
		public ISBEqpStateHandler EqpStateHandler(){
			Debug.Assert(_eqpStateHandler != null);
			return _eqpStateHandler;
		}
		public void SetEqpStateHandler(ISBEqpStateHandler handler){
			_eqpStateHandler = handler;
		}
			ISBEqpStateHandler _eqpStateHandler;
		public bool IsEquipped(){
			return EqpStateHandler().IsEquipped();
		}
		public bool IsUnequipped(){
			return EqpStateHandler().IsUnequipped();
		}
		public void Equip(){
			EqpStateHandler().Equip();
		}
		public void Unequip(){
			EqpStateHandler().Unequip();
		}
		public void ClearCurEqpState(){
			EqpStateHandler().ClearCurEqpState();
		}
		public void UpdateEquipState(){
			if(IsEquipped()) Equip();
			else Unequip();
		}
	}
}
