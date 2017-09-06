using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IPositionSBsCommand{
		void Execute();
	}
	public class PositionSlotsCommand: IPositionSBsCommand{
		IResizableSG sg;
		public PositionSlotsCommand(IResizableSG sg){
			this.sg = sg;
		}
		public void Execute(){

		}
	}
	public interface IOnInventoryUpdatedCommand{
		void Execute(IInventory inventory);
	}
	public class OnInventoryUpdatedCommand: IOnInventoryUpdatedCommand{
		IResizableSG sg;
		public OnInventoryUpdatedCommand(IResizableSG sg){
			this.sg = sg;
		}
		public void Execute(IInventory inventory){

		}
	}
	public interface IAcceptsItemCommand{
		bool AcceptsItem(ISlottableItem item);
	}
	public class EmptyAcceptsItemCommand: IAcceptsItemCommand{
		public bool AcceptsItem(ISlottableItem item){
			return false;
		}
	}
	public interface IFetchInventoryCommand{
		IInventory FetchInventory();
	}
	public class FetchEquippedItemsInventory{
		ISlotSystemManager ssm;
		public FetchEquippedItemsInventory(IResizableSG sg){
			this.ssm = sg.SSM();
		}
		public IInventory FetchInventory(){
			IInventoryManager inventoryManager = ssm.InventoryManager();
			Debug.Assert(inventoryManager != null && (inventoryManager is IEquipToolInventoryManager));
			IEquipToolInventoryManager equipToolInvManager = (IEquipToolInventoryManager)inventoryManager;
			IEquippedItemsInventory equippedItemInventory = equipToolInvManager.EquippedItemsInventory();
			return equippedItemInventory;
		}
	}
	public class FetchUnequippedItemsInventory{
		ISlotSystemManager ssm;
		public FetchUnequippedItemsInventory(IResizableSG sg){
			this.ssm = sg.SSM();
		}
		public IInventory FetchInventory(){
			IInventoryManager inventoryManager = ssm.InventoryManager();
			Debug.Assert(inventoryManager != null && (inventoryManager is IEquipToolInventoryManager));
			IEquipToolInventoryManager equipToolInvManager = (IEquipToolInventoryManager)inventoryManager;
			IUnequippedItemsInventory unequippedItemInventory = equipToolInvManager.UnequippedItemsInventory();
			return unequippedItemInventory;
		}
	}

}
