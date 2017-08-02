using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class ItemHandler : IItemHandler {
		public ItemHandler(){
		}
		public InventoryItemInstance item{
			get{return m_item;}
		}
			InventoryItemInstance m_item;
		public void SetItem(InventoryItemInstance item){
			m_item = item;
		}
		public void IncreasePickedAmountWithinQuanity(){
			if(isStackable && quantity > m_pickedAmount){
				m_pickedAmount ++;
			}
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
		void IncreasePickedAmountWithinQuanity();
		InventoryItemInstance item{get;}
		void SetItem(InventoryItemInstance item);
		int pickedAmount{get;set;}
		bool isStackable{get;}
		int quantity{get;}
		void SetQuantity(int quant);
	}
}
