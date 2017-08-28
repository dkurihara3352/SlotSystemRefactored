using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UISystem{
	public class InventoryItem: IInventoryItem{
		public bool IsStackable(){
			return _isStackable;
		}
		public void SetStackability(bool stackable){
			_isStackable = stackable;
		}
			bool _isStackable;
		public int ItemID(){
			return _itemID;
		}
		public void SetItemID(int id){
			_itemID = id;
		}
			int _itemID;

		public override bool Equals(object other){
			if(!(other is IInventoryItem)) return false;
			else
				return Equals((IInventoryItem)other);
		}
		public bool Equals(IInventoryItem other){
			return ItemID() == other.ItemID();
		}

		public override int GetHashCode(){
			return 31 + _itemID.GetHashCode();
		}

		public static bool operator == (InventoryItem a, InventoryItem b){
			return a.ItemID() == b.ItemID();
		}

		public static bool operator != (InventoryItem a, InventoryItem b){
			return a.ItemID() != b.ItemID();
		}
	}
	public interface IInventoryItem: IEquatable<IInventoryItem>{
		bool IsStackable();
		void SetStackability(bool stackable);
		int ItemID();
		void SetItemID(int id);
	}
}