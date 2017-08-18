using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class ItemHandler : IItemHandler {
		public ItemHandler(IInventoryItemInstance item){
			_item = item;
		}
		public IInventoryItemInstance GetItem(){
			if(_item != null)
				return _item;
			else
				throw new InvalidOperationException("item not set");
		}
			IInventoryItemInstance _item;
		public void SetItem(IInventoryItemInstance item){
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
			return GetItem().IsStackable();
		}
		public int GetQuantity(){
			return GetItem().GetQuantity();
		}
		public void SetQuantity(int quant){
			GetItem().SetQuantity(quant);
		}
	}
	public interface IItemHandler{
		IInventoryItemInstance GetItem();
		void SetItem(IInventoryItemInstance item);
		int GetPickedAmount();
		void SetPickedAmount(int amount);
		bool IsStackable();
		int GetQuantity();
		void SetQuantity(int quant);
	}
}
