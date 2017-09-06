using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UISystem{
	public interface IInventorySystemItem: ISlottableItem{
		InventoryItem InventoryItem();
		void SetInventoryItem(InventoryItem item);
		int AcquisitionOrder();
		void SetAcquisitionOrder(int order);
		bool IsEquipped();
		void SetEquippability(bool equipped);
	}
	public class InventoryItemInstance: IInventorySystemItem{
		public InventoryItem InventoryItem(){
			return _inventoryItem;
		}
		public void SetInventoryItem(InventoryItem item){
			_inventoryItem = item;
		}
			InventoryItem _inventoryItem;
		public int AcquisitionOrder(){
			return _acquisitionOrder;
		}
		public void SetAcquisitionOrder(int order){
			_acquisitionOrder = order;
		}
			int _acquisitionOrder;
		public int ItemID(){
			return InventoryItem().ItemID();
		}
		public bool IsEquipped(){
			return _isEquipped;
		}
		public void SetEquippability(bool equipped){
			_isEquipped = equipped;
		}
			bool _isEquipped = false;
		public bool GetIsMarked(){
			return _isMarked;
		}
		public void SetIsMarked(bool marked){
			_isMarked = marked;
		}
			bool _isMarked = false;
		public int Quantity(){
			return _quantity;
		}
		public void SetQuantity(int quantity){
			_quantity = quantity;
		}
			int _quantity;
		public bool IsStackable(){
			return InventoryItem().IsStackable();
		}
		public override bool Equals(object other){
			if(!(other is ISlottableItem))
				return false;
			return Equals((ISlottableItem)other);
		}
		public bool Equals(ISlottableItem other){
			if(!(other is IInventorySystemItem))
				return false;
			IInventorySystemItem otherInst = (IInventorySystemItem)other;
			if(IsStackable())
				return InventoryItem().Equals(otherInst.InventoryItem());
			else
				return object.ReferenceEquals(this, other);
		}
		public override int GetHashCode(){
			return ItemID().GetHashCode() + 31;
		}
		public static bool operator == (InventoryItemInstance a, InventoryItemInstance b){
			if(object.ReferenceEquals(a, null)){
				return object.ReferenceEquals(b, null);
			}
			if(object.ReferenceEquals(b, null)){
				return object.ReferenceEquals(a, null);
			}
			return a.Equals(b);
		}
		public static bool operator != (InventoryItemInstance a, InventoryItemInstance b){
			if(object.ReferenceEquals(a, null)){
				return !object.ReferenceEquals(b, null);
			}
			if(object.ReferenceEquals(b, null)){
				return !object.ReferenceEquals(a, null);
			}
			return !(a == b);
		}
	}
}