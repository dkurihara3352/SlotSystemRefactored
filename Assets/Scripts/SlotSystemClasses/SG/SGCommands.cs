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
	public class SGCommandsRepo: ISGCommandsRepo{
		ISlotGroup sg;
		public SGCommandsRepo(ISlotGroup sg){
			this.sg = sg;
		}
		public ISGCommand initializeItemsCommand{
			get{				
				if(_initializeItemsCommand == null)
					_initializeItemsCommand = new SGInitItemsCommand(sg);
				return _initializeItemsCommand;
			}
		}
			ISGCommand _initializeItemsCommand;
		public ISGCommand onActionCompleteCommand{
			get{
				if(_onActionCompleteCommand == null)
					_onActionCompleteCommand = new SGEmptyCommand(sg);
				return _onActionCompleteCommand;
			}
		}
			ISGCommand _onActionCompleteCommand;
		public ISGCommand onActionExecuteCommand{
			get{
				if(_onActionExecuteCommand == null)
					_onActionExecuteCommand = new SGUpdateEquipAtExecutionCommand(sg);
				return _onActionExecuteCommand;
			}
		}
			ISGCommand _onActionExecuteCommand;
	}
	public interface ISGCommandsRepo{
		ISGCommand initializeItemsCommand{get;}
		ISGCommand onActionCompleteCommand{get;}
		ISGCommand onActionExecuteCommand{get;}
	}
	public class SGEmptyCommand: SGCommand, ISGEmptyCommand{
		public SGEmptyCommand(ISlotGroup sg): base(sg){}
		public override void Execute(){
		}
	}
	public interface ISGEmptyCommand: ISGCommand{}
	public class SGInitItemsCommand: SGCommand,ISGInitItemsCommand{
		ISlotsHolder slotsHolder;
		IFilterHandler filterHandler;
		ISGTransactionHandler sgTAHandler;
		public SGInitItemsCommand(ISlotGroup sg): base(sg){
			slotsHolder = sg;
			filterHandler = sg;
			sgTAHandler = sg;
		}
		public override void Execute(){
			List<InventoryItemInstance> items = new List<InventoryItemInstance>(sg.inventory);
			items = filterHandler.FilteredItems(items);
			slotsHolder.InitSlots(items);
			sg.InitSBs(items);
			sgTAHandler.SetSBsFromSlotsAndUpdateSlotIDs();
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
					InventoryItemInstance item = sb.GetItem();
					if(sb.IsToBeRemoved()){
						sg.SyncEquipped(item, false);
					}else if(sb.IsToBeAdded()){
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
