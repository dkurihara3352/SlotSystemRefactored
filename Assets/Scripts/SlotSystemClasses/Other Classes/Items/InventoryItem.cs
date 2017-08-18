using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlotSystem{
	public class InventoryItem: IInventoryItem{
		public bool GetIsStackable(){
			return _isStackable;
		}
		public void SetIsStackable(bool stackable){
			_isStackable = stackable;
		}
			bool _isStackable;
		public int GetItemID(){
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
			return GetItemID() == other.GetItemID();
		}

		public override int GetHashCode(){
			return 31 + _itemID.GetHashCode();
		}

		public static bool operator == (InventoryItem a, InventoryItem b){
			return a.GetItemID() == b.GetItemID();
		}

		public static bool operator != (InventoryItem a, InventoryItem b){
			return a.GetItemID() != b.GetItemID();
		}
	}
	public interface IInventoryItem: IEquatable<IInventoryItem>{
		bool GetIsStackable();
		void SetIsStackable(bool stackable);
		int GetItemID();
		void SetItemID(int id);
	}
}