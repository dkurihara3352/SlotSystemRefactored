using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace UISystem{
	public class Slottable : ISlottable{
		public void InitializeSB(ISlot slot, IInventoryItemInstance item){
			SetItemHandler(new ItemHandler(item));
			SetSlot(slot);
			InitializeStateHandlers();
			InitializeStates();
		}
		public ISBToolHandler GetToolHandler(){
			Debug.Assert(_sbToolHandler != null);
			return _sbToolHandler;
		}
			ISBToolHandler _sbToolHandler;
		void SetToolHandler(ISBToolHandler toolHandler){
			_sbToolHandler = toolHandler;
		}
		/*	States	*/
			public void InitializeStates(){
				MakeUnselectable();
				WaitForAction();
				GetToolHandler().InitializeStates();
			}
			public void InitializeStateHandlers(){
				_selStateHandler = new SBSelStateHandler();
				_actStateHandler = new SBActStateHandler(this);
			}
			/*	SB Selection state */
				ISBSelStateHandler SelStateHandler(){
					Debug.Assert(_selStateHandler != null);
					return _selStateHandler;
				}
					ISBSelStateHandler _selStateHandler;
				public void MakeSelectable(){
					SelStateHandler().MakeSelectable();
				}
				public void MakeUnselectable(){
					SelStateHandler().MakeUnselectable();
				}
				public void Select(){
					SelStateHandler().Select();
				}
			/*	Action State */
				ISBActStateHandler ActStateHandler(){
					Debug.Assert(_actStateHandler != null);
					return _actStateHandler;
				}
					ISBActStateHandler _actStateHandler;
				public void SetActStateHandler(ISBActStateHandler actStateHandler){
					_actStateHandler = actStateHandler;
				}
				public void WaitForAction(){
					ActStateHandler().WaitForAction();
				}
				public void Travel(ISlotGroup slotGroup, ISlot slot){
					ActStateHandler().Travel(slotGroup, slot);
				}
				public void Lift(){
					ActStateHandler().Lift();
				}
				public void Land(){
					ActStateHandler().Land();
				}
				public void Appear(){
					ActStateHandler().Appear();
				}
				public void Disappear(){
					ActStateHandler().Disappear();
				}
				public bool IsActProcessRunning(){
					return ActStateHandler().IsActProcessRunning();
				}
				public void ExpireActProcess(){
					ActStateHandler().ExpireActProcess();
				}
		/* Item Handling */
			public IItemHandler ItemHandler(){
				if(_itemHandler != null)
					return _itemHandler;
				else throw new InvalidOperationException("itemHandler not set");
			}
			public void SetItemHandler(IItemHandler itemHandler){
				_itemHandler = itemHandler;
			}
				IItemHandler _itemHandler;
			public ISlottableItem GetItem(){
				return ItemHandler().Item();
			}
			public int GetItemID(){
				return ItemHandler().ItemID();
			}
			public bool IsStackable(){
				return ItemHandler().IsStackable();
			}
			public void Increment(){
				ItemHandler().IncreasePickedAmount();
			}
		/* Others */
			public ISlot Slot(){
				return _slot;
			}
			public void SetSlot(ISlot slot){
				_slot = slot;
			}
				ISlot _slot;
			public ISlotGroup SlotGroup(){
				Slot().SlotGroup();
			}
			public void Refresh(){
				ActStateHandler().WaitForAction();
				ItemHandler().SetPickedAmount(0);
			}
			public bool ShareSGAndItem(ISlottable other){
				bool flag = true;
				flag &= SlotGroup() == other.SlotGroup();
				flag &= GetItem().Equals(other.GetItem());
				return flag;
			}
			public void Destroy(){
			}
	}
	public interface ISlottable{
		ISBSelStateHandler SelStateHandler();
			void MakeSelectable();
			void MakeUnselectable();
			void Select();
		ISBActStateHandler ActStateHandler();
			void WaitForAction();
			void Travel(ISlotGroup slotGroup, ISlot slot);
			void Lift();
			void Land();
			void Appear();
			void Disappear();
			bool IsActProcessRunning();
			void ExpireActProcess();
		ISBToolHandler GetToolHandler();
		void SetToolHandler(ISBToolHandler handler);
		IItemHandler ItemHandler();
			IInventoryItemInstance GetItem();
			int GetItemID();
			bool IsStackable();
		/* Other */
		ISlot Slot();
		void SetSlot(ISlot slot);
		ISlotGroup SlotGroup();
		void Refresh();
		void Destroy();
	}
}
