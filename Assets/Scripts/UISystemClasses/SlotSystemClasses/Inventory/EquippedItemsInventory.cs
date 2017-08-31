using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UISystem{
	public class EquippedItemsInventory: Inventory, IEquippedItemsInventory{
		public EquippedItemsInventory(BowInstance initBow, WearInstance initWear, List<CarriedGearInstance> initCGears ,int initCGCount){
			SetEquippedBow(initBow);
			SetEquippedWear(initWear);
			SetEquippableCGearsCount(initCGCount);
			SetEquippedCarriedGears(initCGears);
		}
		public BowInstance EquippedBow(){
			Debug.Assert(_equippedBow != null);
			return _equippedBow;
		}
		public void SetEquippedBow(BowInstance bow){
			_equippedBow = bow;
		}
			BowInstance _equippedBow;
		public WearInstance EquippedWear(){
			Debug.Assert(_equippedWear != null);
			return _equippedWear;
		}
		public void SetEquippedWear(WearInstance wear){
			_equippedWear = wear;
		}
			WearInstance _equippedWear;
		public List<CarriedGearInstance> EquippedCarriedGears(){
			Debug.Assert(_equippedCGears != null);
			return _equippedCGears;
		}
		public void SetEquippedCarriedGears(List<CarriedGearInstance> cGears){
			Debug.Assert(cGears != null);
			List<CarriedGearInstance> adjusted = AdjustedCGears(cGears);
			_equippedCGears = adjusted;
		}
			List<CarriedGearInstance> _equippedCGears = new List<CarriedGearInstance>();
		List<CarriedGearInstance> AdjustedCGears(List<CarriedGearInstance> source){
			if(source.Count > EquippableCGearsCount()){
				List<CarriedGearInstance> result = new List<CarriedGearInstance>();
				for(int i = 0; i < EquippableCGearsCount(); i ++)
					result.Add(source[i]);
				return result;
			}else
				return source;
		}
		
		public override List<Item> Items(){
			List<Item> result = new List<Item>();
			result.Add(_equippedBow);
			result.Add(_equippedWear);
			foreach(CarriedGearInstance inst in _equippedCGears){
				result.Add((Item)inst);
			}
			return result;
		}
 		public int EquippableCGearsCount(){
			return _equippableCGearsCount;
		}
		public void SetEquippableCGearsCount(int count){
			_equippableCGearsCount = count;
		}
			int _equippableCGearsCount;
		public void UpdateInventory(BowInstance equippedBow, WearInstance equippedWear, List<CarriedGearInstance> equippedCGears){
			SetEquippedBow(equippedBow);
			SetEquippedWear(equippedWear);
			SetEquippedCarriedGears(equippedCGears);
		}
		public void UpdateItemsEquipState(){
			foreach(var item in Items())
				item.SetEquippability(true);
		}
	}
	public interface IEquippedItemsInventory: IInventory{
		BowInstance EquippedBow();
		void SetEquippedBow(BowInstance bow);

		WearInstance EquippedWear();
		void SetEquippedWear(WearInstance wear);

		List<CarriedGearInstance> EquippedCarriedGears();
		void SetEquippedCarriedGears(List<CarriedGearInstance> cGears);

		int EquippableCGearsCount();
		void SetEquippableCGearsCount(int count);
		void UpdateInventory(BowInstance equippedBow, WearInstance equippedWear, List<CarriedGearInstance> equippedCGears);
		void UpdateItemsEquipState();
	}
}