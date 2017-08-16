using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlotSystem{
	public class InventoryItemInstance: SlottableItem{
		InventoryItem m_item;
		public InventoryItem Item{
			get{return m_item;}
			set{m_item = value;}
		}
		int m_quantity;
		public int quantity{
			get{return m_quantity;}
			set{m_quantity = value;}
		}
		int m_acquisitionOrder;
		public int AcquisitionOrder{
			get{return m_acquisitionOrder;}
		}
		public void SetAcquisitionOrder(int id){
			m_acquisitionOrder = id;
		}
		bool m_isStackable;
		public bool isStackable{
			get{
				return m_item.IsStackable;
			}
		}
		bool m_isEquipped = false;
		public bool isEquipped{
			get{return m_isEquipped;}
			set{m_isEquipped = value;}
		}
		bool m_isMarked = false;
		public bool isMarked{
			get{return m_isMarked;}
			set{m_isMarked = value;}
		}
		public override bool Equals(object other){
			if(!(other is InventoryItemInstance))
				return false;
			return Equals((SlottableItem)other);
		}
		public bool Equals(SlottableItem other){
			if(!(other is InventoryItemInstance))
				return false;
			InventoryItemInstance otherInst = (InventoryItemInstance)other;
			if(m_item.IsStackable)
				return m_item.Equals(otherInst.Item);
			else
				return object.ReferenceEquals(this, other);
		}
		public override int GetHashCode(){
			return m_item.ItemID.GetHashCode() + 31;
		}
		public static bool operator ==(InventoryItemInstance a, InventoryItemInstance b){
			if(object.ReferenceEquals(a, null)){
				return !object.ReferenceEquals(b, null);
			}
			if(object.ReferenceEquals(b, null)){
				return !object.ReferenceEquals(a, null);
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
		int IComparable.CompareTo(object other){
			if(!(other is SlottableItem))
				throw new InvalidOperationException("System.Object.CompareTo: not a SlottableItem");
			return CompareTo((SlottableItem)other);
		}
		public int CompareTo(SlottableItem other){
			if(!(other is InventoryItemInstance))
				throw new InvalidOperationException("System.Object.CompareTo: not an InventoryItemInstance");
			InventoryItemInstance otherInst = (InventoryItemInstance)other;

			int result = m_item.ItemID.CompareTo(otherInst.Item.ItemID);
			if(result == 0)
				result = this.AcquisitionOrder.CompareTo(otherInst.AcquisitionOrder);
			
			return result;
		}
	}
}