using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace SlotSystem{
	public class Slottable : SlotSystemElement, ISlottable{
		public override void InitializeStates(){
			_selStateHandler.Deactivate();
			WaitForAction();
			ClearCurEqpState();
			Unmark();
		}
		/*	States	*/
			public void InitializeSB(IInventoryItemInstance item){

				SetHoverable(new Hoverable(GetSSM().GetTAC()));
				SetTapCommand(new SBTapCommand());
				SetItemHandler(new ItemHandler(item));
				SetSlotHandler(new SlotHandler());
				InitializeStateHandlers();
				hoverable.SetSSESelStateHandler(_selStateHandler);
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
				public ISBActStateHandler actStateHandler{
					get{
						if(_actStateHandler != null)
							return _actStateHandler;
						else
							throw new InvalidOperationException("actStateHandler not set");
					}
				}
					ISBActStateHandler _actStateHandler;
				public void SetActStateHandler(ISBActStateHandler actStateHandler){
					_actStateHandler = actStateHandler;
				}
				public void ClearCurActState(){
					actStateHandler.ClearCurActState();
				}
					public bool WasActStateNull(){
						return actStateHandler.WasActStateNull();
					}
					public bool IsActStateNull(){
						return actStateHandler.IsActStateNull();
					}
				public void WaitForAction(){
					actStateHandler.WaitForAction();
				}
					public bool IsWaitingForAction(){
						return actStateHandler.IsWaitingForAction();
					}
					public bool WasWaitingForAction(){
						return actStateHandler.WasWaitingForAction();
					}
				public void WaitForPointerUp(){
					actStateHandler.WaitForPointerUp();
				}
					public bool IsWaitingForPointerUp(){
						return actStateHandler.IsWaitingForPointerUp();
					}
					public bool WasWaitingForPointerUp(){
						return actStateHandler.WasWaitingForPointerUp();
					}
				public void WaitForPickUp(){
					actStateHandler.WaitForPickUp();
				}
					public bool IsWaitingForPickUp(){
						return actStateHandler.IsWaitingForPickUp();
					}
					public bool WasWaitingForPickUp(){
						return actStateHandler.WasWaitingForPickUp();
					}
				public void WaitForNextTouch(){
					actStateHandler.WaitForNextTouch();
				}
					public bool IsWaitingForNextTouch(){
						return actStateHandler.IsWaitingForNextTouch();
					}
					public bool WasWaitingForNextTouch(){
						return actStateHandler.WasWaitingForNextTouch();
					}
				public void PickUp(){
					actStateHandler.PickUp();
				}
				public void SetPickedUpState(){
					actStateHandler.SetPickedUpState();
				}
					public bool IsPickingUp(){
						return actStateHandler.IsPickingUp();
					}
					public bool WasPickingUp(){
						return actStateHandler.WasPickingUp();
					}
				public void Remove(){
					actStateHandler.Remove();
				}
					public bool IsRemoving(){
						return actStateHandler.IsRemoving();
					}
					public bool WasRemoving(){
						return actStateHandler.WasRemoving();
					}
				public void Add(){
					actStateHandler.Add();
				}
					public bool IsAdding(){
						return actStateHandler.IsAdding();
					}
					public bool WasAdding(){
						return actStateHandler.WasAdding();
					}
				public void MoveWithin(){
					actStateHandler.MoveWithin();
				}
					public bool IsMovingWithin(){
						return actStateHandler.IsMovingWithin();
					}
					public bool WasMovingWithin(){
						return actStateHandler.WasMovingWithin();
					}
				public ISBActProcess GetActProcess(){
					return actStateHandler.GetActProcess();
				}
				public void SetActProcessEngine(ISSEProcessEngine<ISBActProcess> engine){
					((SBActStateHandler)actStateHandler).SetActProcessEngine(engine);
				}
				public void SetAndRunActProcess(ISBActProcess process){
					actStateHandler.SetAndRunActProcess(process);
				}
				public void ExpireActProcess(){
					actStateHandler.ExpireActProcess();
				}
				public void SetActCoroutineRepo(ISBActCoroutineRepo repo){
					((SBActStateHandler)actStateHandler).SetCoroutineRepo(repo);
				}
				public System.Func<IEnumeratorFake> GetWaitForPointerUpCoroutine(){
					return actStateHandler.GetWaitForPointerUpCoroutine();
				}
				public System.Func<IEnumeratorFake> GetWaitForPickUpCoroutine(){
					return actStateHandler.GetWaitForPickUpCoroutine();
				}
				public System.Func<IEnumeratorFake> GetPickUpCoroutine(){
					return actStateHandler.GetPickUpCoroutine();
				}
				public System.Func<IEnumeratorFake> GetWaitForNextTouchCoroutine(){
					return actStateHandler.GetWaitForNextTouchCoroutine();
				}
				public System.Func<IEnumeratorFake> GetRemoveCoroutine(){
					return actStateHandler.GetRemoveCoroutine();
				}
				public System.Func<IEnumeratorFake> GetAddCoroutine(){
					return actStateHandler.GetAddCoroutine();
				}
				public System.Func<IEnumeratorFake> GetMoveWithinCoroutine(){
					return actStateHandler.GetMoveWithinCoroutine();
				}
				public void OnPointerDown(PointerEventDataFake eventDataMock){
					actStateHandler.OnPointerDown(eventDataMock);
				}
				public void OnPointerUp(PointerEventDataFake eventDataMock){
					actStateHandler.OnPointerUp(eventDataMock);
				}
				public void OnDeselected(PointerEventDataFake eventDataMock){
					actStateHandler.OnDeselected(eventDataMock);
				}
				public void OnEndDrag(PointerEventDataFake eventDataMock){
					actStateHandler.OnEndDrag(eventDataMock);
				}
			/*	Equip State	*/
				ISBEqpStateHandler eqpStateHandler{
					get{
						if(_eqpStateHandler != null)
							return _eqpStateHandler;
						else
							throw new InvalidOperationException("eqpStateHandler not set");
					}
				}
					ISBEqpStateHandler _eqpStateHandler;
				public void SetEqpStateHandler(ISBEqpStateHandler handler){
					_eqpStateHandler = handler;
				}
				public void ClearCurEqpState(){
					eqpStateHandler.ClearCurEqpState();
				}
				public bool IsEqpStateNull(){
					return eqpStateHandler.IsEqpStateNull();
				}
				public bool WasEqpStateNull(){
					return eqpStateHandler.WasEqpStateNull();
				}
				public void Equip(){
					eqpStateHandler.Equip();
				}
				public bool IsEquipped(){
					return eqpStateHandler.IsEquipped();
				}
				public bool WasEquipped(){
					return eqpStateHandler.WasEquipped();
				}
				public void Unequip(){
					eqpStateHandler.Unequip();
				}
				public bool IsUnequipped(){
					return eqpStateHandler.IsUnequipped();
				}
				public bool WasUnequipped(){
					return eqpStateHandler.WasUnequipped();
				}
				public ISBEqpProcess GetEqpProcess(){
					return eqpStateHandler.GetEqpProcess();
				}
				public void SetEqpProcessEngine(ISSEProcessEngine<ISBEqpProcess> engine){
					((SBEqpStateHandler)eqpStateHandler).SetEqpProcessEngine(engine);
				}
				public void SetAndRunEqpProcess(ISBEqpProcess process){
					eqpStateHandler.SetAndRunEqpProcess(process);
				}
				public System.Func<IEnumeratorFake> GetUnequipCoroutine(){
					return eqpStateHandler.GetUnequipCoroutine();
				}
				public System.Func<IEnumeratorFake> GetEquipCoroutine(){
					return eqpStateHandler.GetEquipCoroutine();
				}
			/*	Mark state	*/
				ISBMrkStateHandler mrkStateHandler{
					get{
						if(_mrkStateHandler != null)
							return _mrkStateHandler;
						else
							throw new InvalidOperationException("mrkStateHandler not set");
					}
				}
					ISBMrkStateHandler _mrkStateHandler;
				public void SetMrkStateHandler(ISBMrkStateHandler mrkStateHandler){
					_mrkStateHandler = mrkStateHandler;
				}
				public void ClearCurMrkState(){
					mrkStateHandler.ClearCurMrkState();
				}
				public bool IsMrkStateNull(){
					return mrkStateHandler.IsMrkStateNull();
				}
				public bool WasMrkStateNull(){
					return mrkStateHandler.WasMrkStateNull();
				}
				public void Mark(){
					mrkStateHandler.Mark();
				}
				public bool IsMarked(){
					return mrkStateHandler.IsMarked();
				}
				public bool WasMarked(){
					return mrkStateHandler.WasMarked();
				}
				public void Unmark(){
					mrkStateHandler.Unmark();
				}
				public bool IsUnmarked(){
					return mrkStateHandler.IsUnmarked();
				}
				public bool WasUnmarked(){
					return mrkStateHandler.WasUnmarked();
				}
				public ISBMrkProcess GetMrkProcess(){
					return mrkStateHandler.GetMrkProcess();
				}
				public void SetMrkProcessEngine(ISSEProcessEngine<ISBMrkProcess> engine){
					((SBMrkStateHandler)mrkStateHandler).SetMrkProcessEngine(engine);
				}
				public void SetAndRunMrkProcess(ISBMrkProcess process){
					mrkStateHandler.SetAndRunMrkProcess(process);
				}
				public System.Func<IEnumeratorFake> GetUnmarkCoroutine(){
					return mrkStateHandler.GetUnmarkCoroutine();
				}
				public System.Func<IEnumeratorFake> GetMarkCoroutine(){
					return mrkStateHandler.GetMarkCoroutine();
				}
		/*	commands	*/
			public void Tap(){
				tapCommand.Execute(this);
			}
			public ISBCommand tapCommand{
				get{
					if(_tapCommand != null)
						return _tapCommand;
					else
						throw new InvalidOperationException("tapCommand not set");
				}
			}
				ISBCommand _tapCommand;
			public void SetTapCommand(ISBCommand comm){
				_tapCommand = comm;
			}
		/*	public fields	*/
			public void Increment(){
				SetPickedUpState();
				IncreasePickedAmountUpToQuantity();
			}
			public void IncreasePickedAmountUpToQuantity(){
				if(IsStackable() && GetQuantity() > GetPickedAmount()){
					SetPickedAmount(GetPickedAmount() + 1);
				}
			}
		/* Item Handling */
			public IItemHandler itemHandler{
				get{
					if(_itemHandler != null)
						return _itemHandler;
					else throw new InvalidOperationException("itemHandler not set");
				}
			}
				IItemHandler _itemHandler;
			public void SetItemHandler(IItemHandler itemHandler){
				_itemHandler = itemHandler;
			}
			public IInventoryItemInstance GetItem(){
				return itemHandler.GetItem();
			}
			public void SetItem(IInventoryItemInstance item){
				itemHandler.SetItem(item);
			}
			public int GetPickedAmount(){
				return itemHandler.GetPickedAmount();
			}
			public void SetPickedAmount(int amount){
				itemHandler.SetPickedAmount(amount);
			}
			public bool IsStackable(){
				return itemHandler.IsStackable();
			}
			public int GetQuantity(){
				return itemHandler.GetQuantity();
			}
			public void SetQuantity(int quant){
				itemHandler.SetQuantity(quant);
			}
			public void UpdateEquipState(){
				if(GetItem().GetIsEquipped()) Equip();
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
			public ISlotHandler slotHandler{
				get{
					if(_slotHandler != null)
						return _slotHandler;
					else
						throw new InvalidOperationException("slotHandler not set");
				}
			}
				ISlotHandler _slotHandler;
			public void SetSlotHandler(ISlotHandler slotHandler){
				_slotHandler = slotHandler;
			}
			public int GetSlotID(){
				return slotHandler.GetSlotID();
			}
			public void SetSlotID(int i){
				slotHandler.SetSlotID(i);
			}
			public int GetNewSlotID(){
				return slotHandler.GetNewSlotID();
			}
			public void SetNewSlotID(int id){
				slotHandler.SetNewSlotID(id);
			}
			public bool IsToBeAdded(){
				return slotHandler.IsToBeAdded();
			}
			public bool IsToBeRemoved(){
				return slotHandler.IsToBeRemoved();
			}
		/* Others */
			public void Refresh(){
				WaitForAction();
				itemHandler.SetPickedAmount(0);
				slotHandler.SetNewSlotID(-2);
			}
			public bool ShareSGAndItem(ISlottable other){
				bool flag = true;
				flag &= this.GetSG() == other.GetSG();
				flag &= this.GetItem().Equals(other.GetItem());
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
			public bool IsPickedUp(){
				return GetTAC().GetPickedSB() == (ISlottable)this;
			}
			public bool PassesPrePickFilter(){
				return !GetTAC().IsTransactionGoingToBeRevert(this);
			}
		/* hoverable */
			public IHoverable hoverable{
				get{
					if(_hoverable != null)
						return _hoverable;
					else
						throw new InvalidOperationException("hoverable not set");
				}
			}
				IHoverable _hoverable;
			public void SetHoverable(IHoverable hoverable){
				_hoverable = hoverable;
			}
			public ITransactionCache GetTAC(){
				return hoverable.GetTAC();
			}
			public void SetTACache(ITransactionCache taCache){
				hoverable.SetTACache(taCache);
			}
			public bool IsHovered(){
				return hoverable.IsHovered();
			}
			public void OnHoverEnter(){
				hoverable.OnHoverEnter();
			}
			public void OnHoverExit(){
				hoverable.OnHoverExit();
			}
			public void SetSSESelStateHandler(ISSESelStateHandler handler){
				//going to be removed in due time
			}
	}
	public interface ISlottable: ISlotSystemElement, IHoverable, IItemHandler, ISBActStateHandler, ISBEqpStateHandler, ISBMrkStateHandler, ISlotHandler{
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
