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