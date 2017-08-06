using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public abstract class SGCommand: ISGCommand{
		protected ISlotGroup sg;
		public SGCommand(ISlotGroup sg){
			this.sg = sg;
		}
		public abstract void Execute();

	}
	public interface ISGCommand{
		void Execute();
	}
	public class SGCommandsFactory: ISGCommandsFactory{
		ISlotGroup sg;
		public SGCommandsFactory(ISlotGroup sg){
			this.sg = sg;
		}
		public ISGCommand MakeInitializeItemsCommand(){
			if(_InitializeItemsCommand == null)
				_InitializeItemsCommand = new SGInitItemsCommand(sg);
			return _InitializeItemsCommand;
		}
			ISGCommand _InitializeItemsCommand;
		public ISGCommand MakeOnActionCompleteCommand(){
			if(_OnActionCompleteCommand == null)
				_OnActionCompleteCommand = new SGEmptyCommand(sg);
			return _OnActionCompleteCommand;
		}
			ISGCommand _OnActionCompleteCommand;
		public ISGCommand MakeOnActionExecuteCommand(){
			if(_OnActionExecuteCommand == null)
				_OnActionExecuteCommand = new SGUpdateEquipAtExecutionCommand(sg);
			return _OnActionExecuteCommand;
		}
			ISGCommand _OnActionExecuteCommand;
	}
	public interface ISGCommandsFactory{
		ISGCommand MakeInitializeItemsCommand();
		ISGCommand MakeOnActionCompleteCommand();
		ISGCommand MakeOnActionExecuteCommand();
	}
	public class SGEmptyCommand: SGCommand, ISGEmptyCommand{
		public SGEmptyCommand(ISlotGroup sg): base(sg){}
		public override void Execute(){
		}
	}
	public interface ISGEmptyCommand: ISGCommand{}
	public class SGInitItemsCommand: SGCommand,ISGInitItemsCommand{
		public SGInitItemsCommand(ISlotGroup sg): base(sg){}
		public override void Execute(){
			List<InventoryItemInstance> items = new List<InventoryItemInstance>(sg.inventory);
			items = sg.FilterItem(items);
			sg.InitSlots(items);
			sg.InitSBs(items);
			sg.SyncSBsToSlots();
			if(sg.isAutoSort)
				sg.InstantSort();
		}
	}
		public interface ISGInitItemsCommand: ISGCommand{}
	public class SGUpdateEquipAtExecutionCommand: SGCommand, ISGUpdateEquipAtExeecutionCommand{
		public SGUpdateEquipAtExecutionCommand(ISlotGroup sg): base(sg){}
		public override void Execute(){
			foreach(ISlottable sb in sg){
				if(sb != null){
					InventoryItemInstance item = sb.item;
					if(sb.isToBeRemoved){
						sg.SyncEquipped(item, false);
					}else if(sb.isToBeAdded){
						sg.SyncEquipped(item, true);
					}
				}
			}
		}
	}
		public interface ISGUpdateEquipAtExeecutionCommand: ISGCommand{}
	public class SGUpdateEquipStatusCommand: SGCommand, ISGUpdateEquipStatusCommand{
		public SGUpdateEquipStatusCommand(ISlotGroup sg): base(sg){}
		public override void Execute(){
			sg.UpdateEquipStatesOnAll();
		}
	}
		public interface ISGUpdateEquipStatusCommand: ISGCommand{}
}
