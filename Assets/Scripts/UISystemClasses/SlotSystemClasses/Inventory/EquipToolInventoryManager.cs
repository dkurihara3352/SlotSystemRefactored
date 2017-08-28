using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class EquipToolInventoryManager : IEquipToolInventoryManager {
		IPoolInventory poolInventory;
		int equippableCarriedGearCount;
		IEquippedElementsProvider equippedElementsProvider;
		public EquipToolInventoryManager(ISlotSystemPlayerData playerData, IEquippedElementsProvider equippedProvider){
			poolInventory = playerData.GetInventory();
			equippableCarriedGearCount = playerData.GetEquippableCarriedGearsCount();
			equippedElementsProvider = equippedProvider;
		}
		public void InitializeEquipToolInventories(){
			List<IInventoryItemInstance> allItems = poolInventory.GetItems();
			List<IInventoryItemInstance> unequippedItems = new List<IInventoryItemInstance>();
			BowInstance equippedBow = null;
			WearInstance equippedWear = null;
			List<CarriedGearInstance> equippedCGears = new List<CarriedGearInstance>();
			
			foreach(var item in allItems){
				Debug.Assert(item != null);
				if(item.IsEquipped()){
					if(item is BowInstance)
						equippedBow = (BowInstance) item;
					else if(item is WearInstance)
						equippedWear = (WearInstance) item;
					else if(item is CarriedGearInstance)
						equippedCGears.Add((CarriedGearInstance) item);
				}else
					unequippedItems.Add(item);
			}
			CheckEquippedItemsAreSet(equippedBow, equippedWear);
			CreateAndSetEquippedItemsInventory(equippedBow, equippedWear, equippedCGears);
			CreateAndSetUnequippedItemsInventory(unequippedItems);
		}
			void CheckEquippedItemsAreSet(BowInstance equippedBow, WearInstance equippedWear){
				if(equippedBow == null)
					throw new InvalidOperationException("equippedBow is not set");
				if(equippedWear == null)
					throw new InvalidOperationException("equippedWear in not set");
			}
			void CreateAndSetEquippedItemsInventory(BowInstance equippedBow, WearInstance equippedWear, List<CarriedGearInstance> equippedCGears){
				IEquippedItemsInventory equippedItemsInventory = new EquippedItemsInventory(equippedBow, equippedWear, equippedCGears, equippableCarriedGearCount);
				SetEquippedItemsInventory(equippedItemsInventory);
			}
			void CreateAndSetUnequippedItemsInventory(List<IInventoryItemInstance> unequippedItems){
				IUnequippedItemsInventory unequippedItemsInventory = new UnequippedItemsInventory(unequippedItems);
				SetUnequippedItemsInventory(unequippedItemsInventory);
			}
		public IEquippedItemsInventory GetEquippedItemsInventory(){
			Debug.Assert(_equippedItemsInventory != null);
			return _equippedItemsInventory;
		}
		void SetEquippedItemsInventory(IEquippedItemsInventory inv){
			_equippedItemsInventory = inv;
		}
			IEquippedItemsInventory _equippedItemsInventory;
		public IUnequippedItemsInventory GetUnequippedItemsInventory(){
			Debug.Assert(_unequippedItemsInventory != null);
			return _unequippedItemsInventory;
		}
		void SetUnequippedItemsInventory(IUnequippedItemsInventory inv){
			_unequippedItemsInventory = inv;
		}
			IUnequippedItemsInventory _unequippedItemsInventory;
		public void UpdateEquipStatus(){
			UpdateInventory();
			UpdateItemsEquipState();
		}
		void UpdateInventory(){
			BowInstance equippedBow = equippedElementsProvider.GetBowInFocusedSGEBow();
			WearInstance equippedWear = equippedElementsProvider.GetWearInFocusedSGEWear();
			List<CarriedGearInstance> equippedCGears = equippedElementsProvider.GetCGearsInFocusedSGECGears();

			GetEquippedItemsInventory().UpdateInventory(equippedBow, equippedWear, equippedCGears);
			GetUnequippedItemsInventory().UpdateInventory(equippedBow, equippedWear, equippedCGears);
		}
		void UpdateItemsEquipState(){
			GetEquippedItemsInventory().UpdateItemsEquipState();
			GetUnequippedItemsInventory().UpdateItemsEquipState();
		}
	}
	public interface IEquipToolInventoryManager{
		void InitializeEquipToolInventories();
		IUnequippedItemsInventory GetUnequippedItemsInventory();
		IEquippedItemsInventory GetEquippedItemsInventory();
		void UpdateEquipStatus();
	}
}
