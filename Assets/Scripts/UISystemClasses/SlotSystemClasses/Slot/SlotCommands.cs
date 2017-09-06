using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class SlotSSEEventCommandsRepo: SSEEventCommandsRepo{
		ISlot slot;
		public SlotSSEEventCommandsRepo(ISlot slot){
			this.slot = slot;
			InitializeCommands();
		}
		public override void InitializeCommands(){
			SetOnItemPickedUpCommand(new OnItemPickedUpCommand_Slot(slot));
			SetOnSBHoverEnteredCommand(new OnSlotHoverEnteredCommand_Slot(slot));
			SetOnSlotHoverEnteredCommand(new OnSlotHoverEnteredCommand_Slot(slot));
			SetOnSGHoverEnteredCommand(new OnSGHoverEnteredCommand_SB(slot));
			SetOnItemDroppedCommand(new OnItemDroppedCommand_Slot(slot));
		}
		public abstract class SSEEventArgsCommand_Slot: ISSEEventArgsCommand{
			protected ISlot slot;
			protected ISlotGroup slotGroup;
			protected ISlottableItem pickedItem;
			protected ISlotGroup hoveredSG;
			protected ISlot hoveredSlot;
			public SSEEventArgsCommand_Slot(ISlot slot){
				this.slot = slot;
			}
			public virtual void Execute(SSEEventArgs e){
				UpdateField(e);
			}
			void UpdateField(SSEEventArgs e){
				slotGroup = slot.SlotGroup();
				pickedItem = e.pickedItem;
				hoveredSG = e.hoveredSG;
				hoveredSlot = e.hoveredSlot;
			}
			protected bool SBIsInHoveredSG(){
				return slot.SlotGroup() == hoveredSG;
			}
			protected bool SlotIsHovered(){
				return this.slot == hoveredSlot;
			}
			protected bool SlotHasPickedItem(){
				return slot.Item() == pickedItem;
			}
		}
		public class OnItemPickedUpCommand_Slot: SSEEventArgsCommand_Slot{
			public OnItemPickedUpCommand_Slot(ISlot slot): base(slot){
			}
			public override void Execute(SSEEventArgs e){
			}
		}
		public class OnSlotHoverEnteredCommand_Slot: SSEEventArgsCommand_Slot{
			public OnSlotHoverEnteredCommand_Slot(ISlot slot): base(slot){
			}
			public override void Execute(SSEEventArgs e){
				if(SlotIsHovered()){
					slot.Select();
					if(SlotHasPickedItem()){
						slot.IncreaseQuantityBy(pickedItem);
					}else{
						/* Reorder */
						slotGroup.InsertItemAt(pickedItem, slot);
					}
				}else{
					if(slot.IsSelected())
						slot.Deselect();
				}
			}
		}
		public class OnSGHoverEnteredCommand_SB: ISSEEventArgsCommand{
			ISlot sb;
			public OnSGHoverEnteredCommand_SB(ISlot sb){
				this.sb = sb;
			}
			public void Execute(SSEEventArgs e){
			}
		}
		public class OnItemDroppedCommand_Slot: ISSEEventArgsCommand{
			ISlot sb;
			public OnItemDroppedCommand_Slot(ISlot sb){
				this.sb = sb;
			}
			public void Execute(SSEEventArgs e){
			}
		}
	}
}
