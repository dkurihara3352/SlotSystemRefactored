using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlotSystem{
	public class InventoryItemInstance: IInventoryItemInstance{
		public InventoryItem GetInventoryItem(){
			return _inventoryItem;
		}
		public void SetInventoryItem(InventoryItem item){
			_inventoryItem = item;
		}
			InventoryItem _inventoryItem;
		public int GetAcquisitionOrder(){
			return _acquisitionOrder;
		}
		public void SetAcquisitionOrder(int order){
			_acquisitionOrder = order;
		}
			int _acquisitionOrder;
		public int GetItemID(){
			return GetInventoryItem().GetItemID();
		}
		public bool IsEquipped(){
			return _isEquipped;
		}
		public void SetIsEquipped(bool equipped){
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
		public int GetQuantity(){
			return _quantity;
		}
		public void SetQuantity(int quantity){
			_quantity = quantity;
		}
			int _quantity;
		public bool IsStackable(){
			return GetInventoryItem().GetIsStackable();
		}
		public override bool Equals(object other){
			if(!(other is SlottableItem))
				return false;
			return Equals((SlottableItem)other);
		}
		public bool Equals(SlottableItem other){
			if(!(other is IInventoryItemInstance))
				return false;
			IInventoryItemInstance otherInst = (IInventoryItemInstance)other;
			if(IsStackable())
				return GetInventoryItem().Equals(otherInst.GetInventoryItem());
			else
				return object.ReferenceEquals(this, other);
		}
		public override int GetHashCode(){
			return GetItemID().GetHashCode() + 31;
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
	public interface IInventoryItemInstance: SlottableItem{
		int GetAcquisitionOrder();
		void SetAcquisitionOrder(int order);
		InventoryItem GetInventoryItem();
		void SetInventoryItem(InventoryItem item);
		int GetItemID();
		bool IsEquipped();
		void SetIsEquipped(bool equipped);
		bool GetIsMarked();
		void SetIsMarked(bool marked);
	}
}