using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class ItemHandler : IItemHandler {
		public ItemHandler(ISlottableItem item){
			_item = item;
		}
		public ISlottableItem Item(){
			if(_item != null)
				return _item;
			else
				throw new InvalidOperationException("item not set");
		}
			ISlottableItem _item;
		public void SetItem(ISlottableItem item){
			_item = item;
		}
		public int PickedAmount(){
			return _pickedAmount;
		}
		public void SetPickedAmount(int amount){
			_pickedAmount = amount;
		}
			int _pickedAmount = 0;
		public void IncreasePickedAmount(){
			int pickedAmount = PickedAmount();
			if(IsStackable() && Quantity() > pickedAmount){
				SetPickedAmount(pickedAmount + 1);
			}
		}
		public virtual bool IsStackable(){
			return Item().IsStackable();
		}
		public int Quantity(){
			return Item().Quantity();
		}
		public void SetQuantity(int quant){
			Item().SetQuantity(quant);
		}
		public int ItemID(){
			return Item().ItemID();
		}
	}
	public interface IItemHandler{
		ISlottableItem Item();
		void SetItem(ISlottableItem item);
		int PickedAmount();
		void SetPickedAmount(int amount);
		void IncreasePickedAmount();
		bool IsStackable();
		int Quantity();
		void SetQuantity(int quant);
		int ItemID();
	}
}
