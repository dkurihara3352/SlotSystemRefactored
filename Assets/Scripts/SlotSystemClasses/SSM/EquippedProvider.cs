using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{	
	public class EquippedProvider : IEquippedProvider {
		IFocusedSGProvider focusedSGProvider;
		public EquippedProvider(IFocusedSGProvider focusedSGProvider){
			this.focusedSGProvider = focusedSGProvider;
		}
		public BowInstance GetEquippedBowInst(){
			ISlotGroup focusedSGEBow = focusedSGProvider.GetFocusedSGEBow();
			if(focusedSGEBow != null){
				ISlottable sb = focusedSGEBow[0] as ISlottable;
				if(sb != null){
					BowInstance result = sb.GetItem() as BowInstance;
					if(result != null) return result;
					throw new System.InvalidOperationException("SlotSystemManager.equippedBowInst: focusedSGEBow's sb item is not set right");
				}
				throw new System.InvalidOperationException("SlotSystemManager.equippedBowInst: focusedSGEBow's indexer not set right");
			}
			throw new System.InvalidOperationException("SlotSystemManager.equippedBowInst: focusedSGEBow is not set");
		}
		public WearInstance GetEquippedWearInst(){
			ISlotGroup focusedSGEWear = focusedSGProvider.GetFocusedSGEWear();
			if(focusedSGEWear != null){
				ISlottable sb = focusedSGEWear[0] as ISlottable;
				if(sb!=null){
					WearInstance result = ((ISlottable)focusedSGEWear[0]).GetItem() as WearInstance;
					if(result != null) return result;
					throw new System.InvalidOperationException("SlotSystemManager.equippedWearInst: focusedSGEWear's sb item is not set right");
				}
				throw new System.InvalidOperationException("SlotSystemManager.equippedWearInst: focusedSGEWear's indexer not set right");
			}
			throw new System.InvalidOperationException("SlotSystemManager.equippedWearInst: focusedSGEWear is not set");
		}
		public List<CarriedGearInstance> GetEquippedCarriedGears(){
			ISlotGroup focusedSGECGears = focusedSGProvider.GetFocusedSGECGears();
			if(focusedSGECGears != null){
				List<CarriedGearInstance> result = new List<CarriedGearInstance>();
				foreach(ISlottable sb in focusedSGECGears)
					if(sb != null) result.Add((CarriedGearInstance)sb.GetItem());
				return result;
			}
			throw new System.InvalidOperationException("SlotSystemManager.equippedCGearsInst: focusedSGECGears is not set");
		}
		public List<IInventoryItemInstance> GetAllEquippedItems(){
			List<IInventoryItemInstance> items = new List<IInventoryItemInstance>();
			items.Add(GetEquippedBowInst());
			items.Add(GetEquippedWearInst());
			foreach(CarriedGearInstance cgItem in GetEquippedCarriedGears()){
				items.Add(cgItem);
			}
			return items;
		}
		public List<PartsInstance> GetEquippedParts(){
			List<PartsInstance> items = new List<PartsInstance>();
			return items;
		}
	}
	public interface IEquippedProvider{
		BowInstance GetEquippedBowInst();
		WearInstance GetEquippedWearInst();
		List<CarriedGearInstance> GetEquippedCarriedGears();
		List<IInventoryItemInstance> GetAllEquippedItems();
		List<PartsInstance> GetEquippedParts();
	}
}
