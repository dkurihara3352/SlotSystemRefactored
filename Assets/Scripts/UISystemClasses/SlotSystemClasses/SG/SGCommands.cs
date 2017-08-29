using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface ICreateSlotCommand{
		Slot CreateSlot();
	}
	public class CreateSlotCommand: ICreateSlotCommand{
		public Slot CreateSlot(){
			Slot slot = new Slot(new RectTransformFake());
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
	public class SGCommandsRepo: ISGCommandsRepo{
		ISlotGroup sg;
		public SGCommandsRepo(ISlotGroup sg){
			this.sg = sg;
		}
	}
	public interface ISGCommandsRepo{
	}

}
