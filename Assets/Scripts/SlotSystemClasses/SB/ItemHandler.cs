using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class ItemHandler : IItemHandler {
		public ItemHandler(InventoryItemInstance item){
			_item = item;
		}
		public InventoryItemInstance GetItem(){
			if(_item != null)
				return _item;
			else
				throw new InvalidOperationException("item not set");
		}
			InventoryItemInstance _item;
		public void SetItem(InventoryItemInstance item){
			_item = item;
		}
		public int GetPickedAmount(){
			return _pickedAmount;
		}
		public void SetPickedAmount(int amount){
			_pickedAmount = amount;
		}
			int _pickedAmount = 0;
		public virtual bool IsStackable(){
			return GetItem().Item.IsStackable;
		}
		public int GetQuantity(){
			return GetItem().quantity;
		}
		public void SetQuantity(int quant){
			GetItem().quantity = quant;
		}
	}
	public interface IItemHandler{
		InventoryItemInstance GetItem();
		void SetItem(InventoryItemInstance item);
		int GetPickedAmount();
		void SetPickedAmount(int amount);
		bool IsStackable();
		int GetQuantity();
		void SetQuantity(int quant);
	}
}
