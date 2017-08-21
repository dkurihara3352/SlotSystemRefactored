using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlotSystem{
	public class EquipmentSetInventory: Inventory, IEquipmentSetInventory{
		public EquipmentSetInventory(BowInstance initBow, WearInstance initWear, List<CarriedGearInstance> initCGears ,int initCGCount){
			_equippedBow = initBow;
			_equippedWear = initWear;
			_equippedCGears = initCGears;
			SetEquippableCGearsCount(initCGCount);
		}
		public override void SetSG(ISlotGroup sg){
			_slotsHolder = sg.GetSlotsHolder();
			_filterHandler = sg.GetFilterHandler();
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
			return _equippedBow;
		}
		public void SetEquippedBow(BowInstance bow){
			_equippedBow = bow;
		}
			BowInstance _equippedBow;
		public WearInstance GetEquippedWear(){
			return _equippedWear;
		}
		public void SetEquippedWear(WearInstance wear){
			_equippedWear = wear;
		}
			WearInstance _equippedWear;
		public List<CarriedGearInstance> GetEquippedCarriedGears(){
			return _equippedCGears;
		}
		public void SetEquippedCarriedGears(List<CarriedGearInstance> cGears){
			_equippedCGears = cGears;
		}
			List<CarriedGearInstance> _equippedCGears = new List<CarriedGearInstance>();
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
		
		public override List<IInventoryItemInstance> GetItems(){
			List<IInventoryItemInstance> result = new List<IInventoryItemInstance>();
			if(_equippedBow != null)
				result.Add(_equippedBow);
			if(_equippedWear != null)
				result.Add(_equippedWear);
			if(_equippedCGears.Count != 0){
				foreach(CarriedGearInstance inst in _equippedCGears){
					result.Add((IInventoryItemInstance)inst);
				}
			}
			return result;
		}
		public override void Add(IInventoryItemInstance item){
			if(item != null){
				if(item is BowInstance){
					BowInstance bowInst = (BowInstance)item;
					_equippedBow = bowInst;
				}	
				else if(item is WearInstance){
					WearInstance wearInst = (WearInstance)item;
					_equippedWear = wearInst;
				}
				else if(item is CarriedGearInstance){
					if(_equippedCGears.Count < _equippableCGearsCount)
						_equippedCGears.Add((CarriedGearInstance)item);
					else
						throw new InvalidOperationException("EquipmentSetInventory.Add: trying to add a CarriedGear exceeding the maximum allowed count");
				}
			}else
				throw new ArgumentNullException();
		}
		public override void Remove(IInventoryItemInstance removedItem){
			if(removedItem != null){
				if(removedItem is BowInstance){
					if((BowInstance)removedItem == _equippedBow)
						_equippedBow = null;
				}else if(removedItem is WearInstance){
					if((WearInstance)removedItem == _equippedWear)
						_equippedWear = null;
				}else if(removedItem is CarriedGearInstance){
					CarriedGearInstance spottedOne = null;
					foreach(CarriedGearInstance cgInst in _equippedCGears){
						if((CarriedGearInstance)removedItem == cgInst)
							spottedOne = cgInst;
					}
					if(spottedOne != null)
						_equippedCGears.Remove(spottedOne);
				}
			}else
				throw new ArgumentNullException();
		}
	}
	public interface IEquipmentSetInventory: IInventory{
		BowInstance GetEquippedBow();
		void SetEquippedBow(BowInstance bow);

		WearInstance GetEquippedWear();
		void SetEquippedWear(WearInstance wear);

		List<CarriedGearInstance> GetEquippedCarriedGears();
		void SetEquippedCarriedGears(List<CarriedGearInstance> cGears);

		int GetEquippableCGearsCount();
		void SetEquippableCGearsCount(int num);
	}
}