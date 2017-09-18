using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UISystem{
	public interface ISlottableItem: IEquatable<ISlottableItem>{
		int Quantity();
		void SetQuantity(int quantity);
		bool IsStackable();
		int ItemID();
	}
	public class SlottableItem: ISlottableItem{
		public SlottableItem(int quantity, bool isStackable, int itemID){
			SetQuantity( quantity);
			SetIsStackable( isStackable);
			SetItemID( itemID);
		}
		public int Quantity(){
			return _quantity;
		}
		public void SetQuantity(int quantity){
			_quantity = quantity;
		}
			int _quantity;
		public bool IsStackable(){
			return _isStackable;
		}
		void SetIsStackable( bool isStackable){
			_isStackable = isStackable;
		}
		bool _isStackable;
		public int ItemID(){
			return _itemID;
		}
		void SetItemID(int id){
			_itemID = id;
		}
			int _itemID;

 		public override bool Equals(object other){
			if(!(other is ISlottableItem))
				return false;
			else
				return Equals((ISlottableItem)other);
		}
		public bool Equals(ISlottableItem other){
			return this.ItemID().Equals( other.ItemID());
		}
		public override int GetHashCode(){
			return ItemID().GetHashCode() + 31;
		}
		public static bool operator == (SlottableItem a, SlottableItem b){
			if(object.ReferenceEquals(a, null)){
				return object.ReferenceEquals(b, null);
			}
			if(object.ReferenceEquals(b, null)){
				return object.ReferenceEquals(a, null);
			}
			return a.Equals(b);
		}
		public static bool operator != (SlottableItem a, SlottableItem b){
			if(object.ReferenceEquals(a, null)){
				return !object.ReferenceEquals(b, null);
			}
			if(object.ReferenceEquals(b, null)){
				return !object.ReferenceEquals(a, null);
			}
			return !(a == b);
		}
	}
	public class EmptySlottableItem: ISlottableItem{
		public int Quantity(){
			return 0;
		}
		public void SetQuantity(int quantity){
			return;
		}
		public bool IsStackable(){
			return false;
		}
		public int ItemID(){
			return -1;
		}
		public override bool Equals(object other){
			if(!(other is ISlottableItem))
				return false;
			else
				return Equals((ISlottableItem)other);
		}
		public bool Equals(ISlottableItem other){
			return object.ReferenceEquals(this, other);
		}
		public override int GetHashCode(){
			return ItemID().GetHashCode() + 31;
		}
		public static bool operator == (EmptySlottableItem a, EmptySlottableItem b){
			if(object.ReferenceEquals(a, null)){
				return object.ReferenceEquals(b, null);
			}
			if(object.ReferenceEquals(b, null)){
				return object.ReferenceEquals(a, null);
			}
			return a.Equals(b);
		}
		public static bool operator != (EmptySlottableItem a, EmptySlottableItem b){
			if(object.ReferenceEquals(a, null)){
				return !object.ReferenceEquals(b, null);
			}
			if(object.ReferenceEquals(b, null)){
				return !object.ReferenceEquals(a, null);
			}
			return !(a == b);
		}
	}
}