using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class SGEquipToolHandler : ISGEquipToolHandler {
		IEquipManager equipManager;
		ISlotGroup sg;
		public SGEquipToolHandler(ISlotGroup sg, IEquipManager equipManager){
			this.sg = sg;
			this.equipManager = equipManager;
		}
		public void UpdateEquipStatesOnAll(){
			equipManager.UpdateEquipStatus();
		}
		public void SyncEquipped(IInventoryItemInstance item, bool equipped){
			IInventory inventory = sg.Inventory();
			if(equipped)
				inventory.Add(item);
			else
				inventory.Remove(item);
			equipManager.MarkEquippedInPool(item, equipped);
			equipManager.UpdateEquipStatesOnAllSBs(item, equipped);
		}
		public bool IsPool(){
			return equipManager.UnequipBundleContains(sg);
		}
		public bool IsSGE(){
			return equipManager.EquipBundleContains(sg);
		}
		public bool IsSGG(){
			return equipManager.OtherBundlesContain(sg);
		}
		List<ISBEquipToolHandler> GetSBEquipToolHandlers(){
			List<ISBEquipToolHandler> result = new List<ISBEquipToolHandler>();
			foreach(var ele in sg){
				ISlottable sb = (ISlottable)ele;
				if(sb != null){
					ISBToolHandler sbToolHandler = sb.GetToolHandler();
					if(sbToolHandler is ISBEquipToolHandler)
						result.Add((ISBEquipToolHandler)sbToolHandler);
				}
			}
			return result;
		}
		public List<ISlottable> GetEquippedSBs(){
			List<ISlottable> result = new List<ISlottable>();
			foreach(ISBEquipToolHandler sbEquipHandler in GetSBEquipToolHandlers()){
				if(sbEquipHandler != null && sbEquipHandler.IsEquipped())
					result.Add(sbEquipHandler.GetSB());
			}
			return result;
		}
	}
	public interface ISGEquipToolHandler: ISGToolHandler{
		void UpdateEquipStatesOnAll();
		void SyncEquipped(IInventoryItemInstance item, bool equipped);
		bool IsPool();
		List<ISlottable> GetEquippedSBs();
	}
	public interface ISGToolHandler{
		
	}
}
