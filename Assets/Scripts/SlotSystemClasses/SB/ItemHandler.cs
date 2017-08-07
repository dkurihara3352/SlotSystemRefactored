using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class ItemHandler : IItemHandler {
		public InventoryItemInstance item{
			get{
				if(_item != null)
					return _item;
				else
					throw new InvalidOperationException("item not set");
			}
		}
			InventoryItemInstance _item;
		public void SetItem(InventoryItemInstance item){
			_item = item;
		}
		public int pickedAmount{
			get{return m_pickedAmount;}
			set{m_pickedAmount = value;}
		}
			int m_pickedAmount = 0;
		public virtual bool isStackable{
			get{return item.Item.IsStackable;}
		}
		public int quantity{
			get{return item.quantity;}
		}
		public void SetQuantity(int quant){
			item.quantity = quant;
		}
	}
	public interface IItemHandler{
		InventoryItemInstance item{get;}
		void SetItem(InventoryItemInstance item);
		int pickedAmount{get;set;}
		bool isStackable{get;}
		int quantity{get;}
		void SetQuantity(int quant);
	}
}
