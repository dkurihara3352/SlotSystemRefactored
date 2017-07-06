using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlotSystem{
	public class InventoryItem: IEquatable<InventoryItem>, IComparable, IComparable<InventoryItem>{
		bool m_isStackable;
		public bool IsStackable{
			get{return m_isStackable;}
			set{m_isStackable = value;}
		}

		int m_itemId;
		public int ItemID{
			get{return m_itemId;}
			set{m_itemId = value;}
		}

		public override bool Equals(object other){
			if(!(other is InventoryItem)) return false;
			else
				return Equals((InventoryItem)other);
		}
		public bool Equals(InventoryItem other){
			return m_itemId == other.ItemID;
		}

		public override int GetHashCode(){
			return 31 + m_itemId.GetHashCode();
		}

		public static bool operator == (InventoryItem a, InventoryItem b){
			return a.ItemID == b.ItemID;
		}

		public static bool operator != (InventoryItem a, InventoryItem b){
			return a.ItemID != b.ItemID;
		}
		int IComparable.CompareTo(object other){
			if(!(other is InventoryItem))
				throw new InvalidOperationException("Compare To: not a InventoryItemMock");
			return CompareTo((InventoryItem)other);
		}
		public int CompareTo(InventoryItem other){
			if(!(other is InventoryItem))
				throw new InvalidOperationException("Compare To: not a InventoryItemMock");
			
			return this.m_itemId.CompareTo(other.ItemID);
		}
		public static bool operator > (InventoryItem a, InventoryItem b){
			return a.CompareTo(b) > 0;
		}
		public static bool operator < (InventoryItem a, InventoryItem b){
			return a.CompareTo(b) < 0;
		}
	}
}