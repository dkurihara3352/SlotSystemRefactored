using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	
	public interface ICreateSlotCommand{
		Slot CreateSlot();
	}
	public class CreateSlotCommand: ICreateSlotCommand{
		public Slot CreateSlot(){
			Slot slot = new Slot(new RectTransformFake(), new EmptySSECommandsRepo(), new TapCommand());
			return slot;
		}
	}
	public interface IPositionSlotsCommand{
		void Execute();
	}
	public class PositionSlotsCommand: IPositionSlotsCommand{
		ISlotGroup sg;
		public PositionSlotsCommand(ISlotGroup sg){
			this.sg = sg;
		}
		public void Execute(){

		}
	}
	public interface IOnInventoryUpdatedCommand{
		void Execute(IInventory inventory);
	}
	public class OnInventoryUpdatedCommand: IOnInventoryUpdatedCommand{
		ISlotGroup sg;
		public OnInventoryUpdatedCommand(ISlotGroup sg){
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
		public FetchEquippedItemsInventory(ISlotGroup sg){
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
		public FetchUnequippedItemsInventory(ISlotGroup sg){
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
