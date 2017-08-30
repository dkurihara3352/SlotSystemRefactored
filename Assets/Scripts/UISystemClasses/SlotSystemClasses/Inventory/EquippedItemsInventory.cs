using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UISystem{
	public class EquippedItemsInventory: Inventory, IEquippedItemsInventory{
		public EquippedItemsInventory(BowInstance initBow, WearInstance initWear, List<CarriedGearInstance> initCGears ,int initCGCount){
			_equippedBow = initBow;
			_equippedWear = initWear;
			_equippedCGears = initCGears;
			SetEquippableCGearsCount(initCGCount);
		}
		public override void SetSG(ISlotGroup sg){
			_slotsHolder = sg.GetSlotsHolder();
			_filterHandler = sg.FilterHandler();
		}
		ISlotsHolder slotsHolder{
			get{
				if(_slotsHolder != null)
					return _slotsHolder;
				else
					throw new InvalidOperationException("slotsHolder not set");
			}
		}
			ISlotsHolder _slotsHolder;
		IFilterHandler filterHandler{
			get{
				if(_filterHandler != null)
					return _filterHandler;
				else
					throw new InvalidOperationException("filterHandler not set");
			}
		}
			IFilterHandler _filterHandler;
		public BowInstance GetEquippedBow(){
			Debug.Assert(_equippedBow != null);
			return _equippedBow;
		}
		public void SetEquippedBow(BowInstance bow){
			_equippedBow = bow;
		}
			BowInstance _equippedBow;
		public WearInstance GetEquippedWear(){
			Debug.Assert(_equippedWear != null);
			return _equippedWear;
		}
		public void SetEquippedWear(WearInstance wear){
			_equippedWear = wear;
		}
			WearInstance _equippedWear;
		public List<CarriedGearInstance> GetEquippedCarriedGears(){
			Debug.Assert(_equippedCGears != null);
			return _equippedCGears;
		}
		public void SetEquippedCarriedGears(List<CarriedGearInstance> cGears){
			_equippedCGears = cGears;
		}
			List<CarriedGearInstance> _equippedCGears = new List<CarriedGearInstance>();
		
		public override List<Item> Items(){
			List<Item> result = new List<Item>();
			result.Add(_equippedBow);
			result.Add(_equippedWear);
			foreach(CarriedGearInstance inst in _equippedCGears){
				result.Add((Item)inst);
			}
			return result;
		}
 		public int GetEquippableCGearsCount(){
			return _equippableCGearsCount;
		}
		public void SetEquippableCGearsCount(int num){
			_equippableCGearsCount = num;
			ISlotGroup sg = GetSG();
			if(sg != null && filterHandler.GetFilter() is SGCGearsFilter && !sg.IsResizable())
			slotsHolder.SetInitSlotsCount(num);
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
		BowInstance GetEquippedBow();
		void SetEquippedBow(BowInstance bow);

		WearInstance GetEquippedWear();
		void SetEquippedWear(WearInstance wear);

		List<CarriedGearInstance> GetEquippedCarriedGears();
		void SetEquippedCarriedGears(List<CarriedGearInstance> cGears);

		int GetEquippableCGearsCount();
		void SetEquippableCGearsCount(int num);
		void UpdateInventory(BowInstance equippedBow, WearInstance equippedWear, List<CarriedGearInstance> equippedCGears);
		void UpdateItemsEquipState();
	}
}