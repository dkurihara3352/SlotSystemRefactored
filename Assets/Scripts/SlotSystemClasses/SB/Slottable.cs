using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace SlotSystem{
	public class Slottable : SlotSystemElement, ISlottable{
			public void InitializeSB(IInventoryItemInstance item){
				ITransactionCache tac = GetSSM().GetTAC();
				SetTAC(tac);
				IHoverable hoverable = new Hoverable(tac);
				SetHoverable(hoverable);
				SetTapCommand(new SBTapCommand(this));
				SBPickUpEquipCommand pickUpEquipCommand = GetPickUpEquipCommand(item);
				SetPickUpCommand(pickUpEquipCommand);
				SetItemHandler(new ItemHandler(item));
				SetSlotHandler(new SlotHandler());
				InitializeStateHandlers();
				hoverable.SetSSESelStateHandler(GetSelStateHandler());
			}
		/*	States	*/
			public override void InitializeStates(){
				GetSelStateHandler().Deactivate();
				GetActStateHandler().WaitForAction();
				ClearCurEqpState();
				GetMrkStateHandler().Unmark();
			}
			public void InitializeStateHandlers(){
				_selStateHandler = new SBSelStateHandler(this);
				_actStateHandler = new SBActStateHandler(this, GetSSM().GetTAM());
				_eqpStateHandler = new SBEqpStateHandler(this);
				_mrkStateHandler = new SBMrkStateHandler(this);
			}
			/*	Selection state */
				public override ISSESelStateHandler GetSelStateHandler(){
					if(_selStateHandler != null)
						return _selStateHandler;
					else
						throw new InvalidOperationException("selStateHandler not set");
				}
					ISSESelStateHandler _selStateHandler;
				public override void SetSelStateHandler(ISSESelStateHandler handler){
					_selStateHandler = handler;
				}
			/*	Action State */
				public ISBActStateHandler GetActStateHandler(){
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
					return GetActStateHandler().IsActProcessRunning();
				}
				public void MoveWithin(){
					GetActStateHandler().MoveWithin();
				}
				public void Add(){
					GetActStateHandler().Add();
				}
				public void Remove(){
					GetActStateHandler().Remove();
				}
				public void PickUp(){
					GetActStateHandler().PickUp();
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
			/*	Equip State	*/
				public ISBEqpStateHandler GetEqpStateHandler(){
					if(_eqpStateHandler != null)
						return _eqpStateHandler;
					else
						throw new InvalidOperationException("eqpStateHandler not set");
				}
				public void SetEqpStateHandler(ISBEqpStateHandler handler){
					_eqpStateHandler = handler;
				}
					ISBEqpStateHandler _eqpStateHandler;
				public bool IsEquipped(){
					return GetEqpStateHandler().IsEquipped();
				}
				public bool IsUnequipped(){
					return GetEqpStateHandler().IsUnequipped();
				}
				public void Equip(){
					GetEqpStateHandler().Equip();
				}
				public void Unequip(){
					GetEqpStateHandler().Unequip();
				}
				public void ClearCurEqpState(){
					GetEqpStateHandler().ClearCurEqpState();
				}
			/*	Mark state	*/
				public ISBMrkStateHandler GetMrkStateHandler(){
					if(_mrkStateHandler != null)
						return _mrkStateHandler;
					else
						throw new InvalidOperationException("mrkStateHandler not set");
				}
				public void SetMrkStateHandler(ISBMrkStateHandler mrkStateHandler){
					_mrkStateHandler = mrkStateHandler;
				}
					ISBMrkStateHandler _mrkStateHandler;
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
				GetActStateHandler().SetPickedUpState();
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
				return GetItemHandler().GetItem();
			}
			public int GetAcquisitionOrder(){
				return GetItemHandler().GetAcquisitionOrder();
			}
			public int GetItemID(){
				return GetItemHandler().GetItemID();
			}
			public bool IsStackable(){
				return GetItemHandler().IsStackable();
			}
			public void UpdateEquipState(){
				if(GetItem().IsEquipped()) Equip();
				else Unequip();
			}
		/* SG And Slots */
			public ISlotGroup GetSG(){
				ISlotSystemElement parent = GetParent();
				if(parent != null)
					return parent as ISlotGroup;
				return null;
			}
			public bool IsPool(){
				ISlotGroup sg = GetSG();
				if(sg != null){
					return sg.IsPool();
				}else
					throw new System.InvalidOperationException("Slottable.isPool: sg is not set");
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
				GetActStateHandler().WaitForAction();
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
			protected override IEnumerable<ISlotSystemElement> elements{
				get{return new ISlotSystemElement[]{};}
			}
			public override void SetElements(IEnumerable<ISlotSystemElement> elements){
			}
			public override ISlotSystemElement GetParent(){
				return GetSSM().FindParent(this);
			}
			public override bool Contains(ISlotSystemElement element){
				return false;
			}
			public override bool ContainsInHierarchy(ISlotSystemElement element){
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
	public interface ISlottable: ISlotSystemElement{
			ITransactionCache GetTAC();
			IHoverable GetHoverable();
			IItemHandler GetItemHandler();
				IInventoryItemInstance GetItem();
				int GetAcquisitionOrder();
				int GetItemID();
				bool IsStackable();
			ISBActStateHandler GetActStateHandler();
				bool IsActProcessRunning();
				void PickUp();
				void MoveWithin();
				void Remove();
				void Add();
			ISBEqpStateHandler GetEqpStateHandler();
				bool IsEquipped();
				bool IsUnequipped();
				void Equip();
				void Unequip();
				void ClearCurEqpState();
			ISBMrkStateHandler GetMrkStateHandler();
			ISlotHandler GetSlotHandler();
				bool IsToBeRemoved();
				bool IsToBeAdded();
				void SetSlotID(int id);
				int GetNewSlotID();
				void SetNewSlotID(int id);
		/*	Commands	*/
			void Tap();
		/* Item Handling */
			void UpdateEquipState();
		/* SG And Slot */
			ISlotGroup GetSG();
			bool IsPool();
			bool IsHierarchySetUp();
		/* Transaction */
			bool IsPickedUp();
			bool PassesPrePickFilter();
		/* Other */
			void Increment();
			void Refresh();
			bool ShareSGAndItem(ISlottable other);
			void Destroy();
	}
}
