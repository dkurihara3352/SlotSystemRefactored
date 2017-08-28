using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace UISystem{
	public class Slottable : ISlottable{
		public void InitializeSB(IInventoryItemInstance item){
			SetItemHandler(new ItemHandler(item));
			SetSlotHandler(new SlotHandler());
			InitializeStateHandlers();
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
			public override void InitializeStates(){
				UISelStateHandler().Deactivate();
				ActStateHandler().WaitForAction();
				GetToolHandler().InitializeStates();
			}
			public void InitializeStateHandlers(){
				_selStateHandler = new SBSelStateHandler(this);
				_actStateHandler = new SBActStateHandler(this);
			}
			/*	Selection state */
				public override IUISelStateHandler UISelStateHandler(){
					if(_selStateHandler != null)
						return _selStateHandler;
					else
						throw new InvalidOperationException("selStateHandler not set");
				}
					IUISelStateHandler _selStateHandler;
				public override void SetSelStateHandler(IUISelStateHandler handler){
					_selStateHandler = handler;
				}
			/*	Action State */
				public ISBActStateHandler ActStateHandler(){
					if(_actStateHandler != null)
						return _actStateHandler;
					else
						throw new InvalidOperationException("actStateHandler not set");
				}
				public void SetActStateHandler(ISBActStateHandler actStateHandler){
					_actStateHandler = actStateHandler;
				}
					ISBActStateHandler _actStateHandler;
				public bool IsActProcessRunning(){
					return ActStateHandler().IsActProcessRunning();
				}
				public void MoveWithin(){
					ActStateHandler().MoveWithin();
				}
				public void Add(){
					ActStateHandler().Add();
				}
				public void Remove(){
					ActStateHandler().Remove();
				}
				public void PickUp(){
					GetItemHandler().SetPickedAmount(1);
					GetPickUpCommand().Execute();
				}
				public ISBCommand GetPickUpCommand(){
					Debug.Assert(_pickUpCommand != null);
					return _pickUpCommand;
				}
					ISBCommand _pickUpCommand;
				public void SetPickUpCommand(ISBCommand pickUpCommand){
					_pickUpCommand = pickUpCommand;
				}
				public SBPickUpEquipCommand GetPickUpEquipCommand(IInventoryItemInstance item){
					if(item is BowInstance)
						return new SBPickUpEquipBowCommand(this);
					else if(item is WearInstance)
						return new SBPickUpEquipWearCommand(this);
					else if(item is CarriedGearInstance)
						return new SBPickUpEquipCGearsCommand(this);
					else if (item is PartsInstance)
						return new SBPickUpEquipPartsCommand(this);
					else
						return new SBPickUpEquipCommand(this);
				}
		/*	commands	*/
			public void Tap(){
				GetTapCommand().Execute();
			}
			public ISBCommand GetTapCommand(){
				if(_tapCommand != null)
					return _tapCommand;
				else
					throw new InvalidOperationException("tapCommand not set");
			}
				ISBCommand _tapCommand;
			public void SetTapCommand(ISBCommand comm){
				_tapCommand = comm;
			}
		/*	public fields	*/
			public void Increment(){
				ActStateHandler().SetPickedUpState();
				GetItemHandler().IncreasePickedAmount();
			}
		/* Item Handling */
			public IItemHandler GetItemHandler(){
				if(_itemHandler != null)
					return _itemHandler;
				else throw new InvalidOperationException("itemHandler not set");
			}
			public void SetItemHandler(IItemHandler itemHandler){
				_itemHandler = itemHandler;
			}
				IItemHandler _itemHandler;
			public IInventoryItemInstance GetItem(){
				return GetItemHandler().Item();
			}
			public int GetAcquisitionOrder(){
				return GetItemHandler().GetAcquisitionOrder();
			}
			public int GetItemID(){
				return GetItemHandler().ItemID();
			}
			public bool IsStackable(){
				return GetItemHandler().IsStackable();
			}
		/* SG And Slots */
			public ISlotGroup GetSG(){
				IUIElement parent = GetParent();
				if(parent != null)
					return parent as ISlotGroup;
				return null;
			}
			public bool IsHierarchySetUp(){
				return GetSG() != null;
			}
		/* slotHandling */
			public ISlotHandler GetSlotHandler(){
				if(_slotHandler != null)
					return _slotHandler;
				else
					throw new InvalidOperationException("slotHandler not set");
			}
			public void SetSlotHandler(ISlotHandler slotHandler){
				_slotHandler = slotHandler;
			}
				ISlotHandler _slotHandler;
			public bool IsToBeAdded(){
				return GetSlotHandler().IsToBeAdded();
			}
			public bool IsToBeRemoved(){
				return GetSlotHandler().IsToBeRemoved();
			}
			public void SetSlotID(int id){
				GetSlotHandler().SetSlotID(id);
			}
			public int GetNewSlotID(){
				return GetSlotHandler().GetNewSlotID();
			}
			public void SetNewSlotID(int id){
				GetSlotHandler().SetNewSlotID(id);
			}
		/* Others */
			public void Refresh(){
				ActStateHandler().WaitForAction();
				GetItemHandler().SetPickedAmount(0);
				SetNewSlotID(-2);
			}
			public bool ShareSGAndItem(ISlottable other){
				bool flag = true;
				flag &= GetSG() == other.GetSG();
				flag &= GetItem().Equals(other.GetItem());
				return flag;
			}
			public void Destroy(){
				// GameObject go = gameObject;
				// DestroyImmediate(go);
			}
		/*	SlotSystemElement imple and overrides	*/
			protected override IEnumerable<IUIElement> elements{
				get{return new IUIElement[]{};}
			}
			public override void SetElements(IEnumerable<IUIElement> elements){
			}
			public override IUIElement GetParent(){
				return GetSSM().FindParent(this);
			}
			public override bool Contains(IUIElement element){
				return false;
			}
			public override bool ContainsInHierarchy(IUIElement element){
				return false;
			}
		/*	Transaction	*/
			public ITransactionCache GetTAC(){
				if(_taCache != null)
					return _taCache;
				else
					throw new InvalidOperationException("taCache not set");
			}
			public void SetTAC(ITransactionCache taCache){
				_taCache = taCache;
			}
				ITransactionCache _taCache;
			public bool IsPickedUp(){
				return GetTAC().GetPickedSB() == (ISlottable)this;
			}
			public bool PassesPrePickFilter(){
				return !GetTAC().IsTransactionGoingToBeRevert(this);
			}
		/* hoverable */
			public IHoverable GetHoverable(){
				if(_hoverable != null)
					return _hoverable;
				else
					throw new InvalidOperationException("hoverable not set");
			}
			public void SetHoverable(IHoverable hoverable){
				_hoverable = hoverable;
			}
				IHoverable _hoverable;
	}
	public interface ISlottable{
		ISBToolHandler GetToolHandler();
		void SetToolHandler(ISBToolHandler handler);
		IItemHandler GetItemHandler();
			IInventoryItemInstance GetItem();
			int GetAcquisitionOrder();
			int GetItemID();
			bool IsStackable();
		ISBActStateHandler ActStateHandler();
			bool IsActProcessRunning();
			void WaitForAction();
			void PickUp();
			void Travel(ISlotGroup sg, ISlot slot);
			void SlotIn();
			void Remove();
			void Add();
		ISlotHandler GetSlotHandler();
			bool IsToBeRemoved();
			bool IsToBeAdded();
			ISlot Slot();
			void SetSlot(ISlot slot);
		ISlotGroup GetSG();
		/* Other */
		void Refresh();
		void Destroy();
	}
}
