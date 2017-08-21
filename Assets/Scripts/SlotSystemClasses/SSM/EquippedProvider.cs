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
			ISlottable sb = focusedSGEBow[0] as ISlottable;
			if(sb != null){
				BowInstance result = sb.GetItem() as BowInstance;
				if(result != null) return result;
				throw new InvalidOperationException("focusedSGEBow's sb item is not set right");
			}
			throw new InvalidOperationException("focusedSGEBow's indexer not set right");
		}
		public WearInstance GetEquippedWearInst(){
			ISlotGroup focusedSGEWear = focusedSGProvider.GetFocusedSGEWear();
			ISlottable sb = focusedSGEWear[0] as ISlottable;
			if(sb!=null){
				WearInstance result = ((ISlottable)focusedSGEWear[0]).GetItem() as WearInstance;
				if(result != null) return result;
				throw new InvalidOperationException("focusedSGEWear's sb item is not set right");
			}
			throw new InvalidOperationException("focusedSGEWear's indexer not set right");
		}
		public List<CarriedGearInstance> GetEquippedCarriedGears(){
			ISlotGroup focusedSGECGears = focusedSGProvider.GetFocusedSGECGears();
			if(focusedSGECGears != null){
				List<CarriedGearInstance> result = new List<CarriedGearInstance>();
				foreach(ISlottable sb in focusedSGECGears)
					if(sb != null) result.Add((CarriedGearInstance)sb.GetItem());
				return result;
			}
			throw new InvalidOperationException("focusedSGECGears is not set");
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
		public bool AllEquippedItemsContain(IInventoryItemInstance item){
			return GetAllEquippedItems().Contains(item);
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
		bool AllEquippedItemsContain(IInventoryItemInstance item);
		List<PartsInstance> GetEquippedParts();
	}
}
