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
		public BowInstance equippedBowInst{
			get{
				if(focusedSGProvider.focusedSGEBow != null){
					ISlottable sb = focusedSGProvider.focusedSGEBow[0] as ISlottable;
					if(sb != null){
						BowInstance result = sb.item as BowInstance;
						if(result != null) return result;
						throw new System.InvalidOperationException("SlotSystemManager.equippedBowInst: focusedSGEBow's sb item is not set right");
					}
					throw new System.InvalidOperationException("SlotSystemManager.equippedBowInst: focusedSGEBow's indexer not set right");
				}
				throw new System.InvalidOperationException("SlotSystemManager.equippedBowInst: focusedSGEBow is not set");
			}
		}
		public WearInstance equippedWearInst{
			get{
				if(focusedSGProvider.focusedSGEWear != null){
					ISlottable sb = focusedSGProvider.focusedSGEWear[0] as ISlottable;
					if(sb!=null){
						WearInstance result = ((ISlottable)focusedSGProvider.focusedSGEWear[0]).item as WearInstance;
						if(result != null) return result;
						throw new System.InvalidOperationException("SlotSystemManager.equippedWearInst: focusedSGEWear's sb item is not set right");
					}
					throw new System.InvalidOperationException("SlotSystemManager.equippedWearInst: focusedSGEWear's indexer not set right");
				}
				throw new System.InvalidOperationException("SlotSystemManager.equippedWearInst: focusedSGEWear is not set");
			}
		}
		public List<CarriedGearInstance> equippedCarriedGears{
			get{
				if(focusedSGProvider.focusedSGECGears != null){
					List<CarriedGearInstance> result = new List<CarriedGearInstance>();
					foreach(ISlottable sb in focusedSGProvider.focusedSGECGears)
						if(sb != null) result.Add((CarriedGearInstance)sb.item);
					return result;
				}
				throw new System.InvalidOperationException("SlotSystemManager.equippedCGearsInst: focusedSGECGears is not set");
			}
		}
		public List<InventoryItemInstance> allEquippedItems{
			get{
				List<InventoryItemInstance> items = new List<InventoryItemInstance>();
				items.Add(equippedBowInst);
				items.Add(equippedWearInst);
				foreach(CarriedGearInstance cgItem in equippedCarriedGears){
					items.Add(cgItem);
				}
				return items;
			}
		}
		public List<PartsInstance> equippedParts{
			get{
				List<PartsInstance> items = new List<PartsInstance>();
				return items;
			}
		}
	}
	public interface IEquippedProvider{
		BowInstance equippedBowInst{get;}
		WearInstance equippedWearInst{get;}
		List<CarriedGearInstance> equippedCarriedGears{get;}
		List<InventoryItemInstance> allEquippedItems{get;}
		List<PartsInstance> equippedParts{get;}
	}
}
