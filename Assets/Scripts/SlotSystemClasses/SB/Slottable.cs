using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace SlotSystem{
	public class Slottable : SlotSystemElement, ISlottable{
		public override void InitializeStates(){
			Deactivate();
			WaitForAction();
			ClearCurEqpState();
			Unmark();
		}
		/*	States	*/
			public void InitializeSB(InventoryItemInstance item){
				SetHoverable(new Hoverable(this, ssm.taCache));
				SetTapCommand(new SBTapCommand());
				SetItemHandler(new ItemHandler(item));
				SetSlotHandler(new SlotHandler());
				InitializeStateHandlers();
			}
			public void InitializeStateHandlers(){
				_selStateHandler = new SBSelStateHandler(this);
				_actStateHandler = new SBActStateHandler(this, ssm.tam);
				_eqpStateHandler = new SBEqpStateHandler(this);
				_mrkStateHandler = new SBMrkStateHandler(this);
			}
			/*	Selection state */
				public override ISSESelStateHandler selStateHandler{
					get{
						if(_selStateHandler != null)
							return _selStateHandler;
						else
							throw new InvalidOperationException("selStateHandler not set");
					}
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
					public bool wasActStateNull{
						get{return actStateHandler.wasActStateNull;}
					}
					public bool isActStateNull{
						get{return actStateHandler.isActStateNull;}
					}
				public void WaitForAction(){
					actStateHandler.WaitForAction();
				}
					public ISBActState waitForActionState{
						get{return actStateHandler.waitForActionState;}
					}
					public bool isWaitingForAction{
						get{return actStateHandler.isWaitingForAction;}
					}
					public bool wasWaitingForAction{
						get{return actStateHandler.wasWaitingForAction;}
					}
				public void WaitForPointerUp(){
					actStateHandler.WaitForPointerUp();
				}
					public ISBActState waitForPointerUpState{
						get{return actStateHandler.waitForPointerUpState;}
					}
					public bool isWaitingForPointerUp{
						get{return actStateHandler.isWaitingForPointerUp;}
					}
					public bool wasWaitingForPointerUp{
						get{return actStateHandler.wasWaitingForPointerUp;}
					}
				public void WaitForPickUp(){
					actStateHandler.WaitForPickUp();
				}
					public ISBActState waitForPickUpState{
						get{return actStateHandler.waitForPickUpState;}
					}
					public bool isWaitingForPickUp{
						get{return actStateHandler.isWaitingForPickUp;}
					}
					public bool wasWaitingForPickUp{
						get{return actStateHandler.wasWaitingForPickUp;}
					}
				public void WaitForNextTouch(){
					actStateHandler.WaitForNextTouch();
				}
					public ISBActState waitForNextTouchState{
						get{return actStateHandler.waitForNextTouchState;}
					}
					public bool isWaitingForNextTouch{
						get{return actStateHandler.isWaitingForNextTouch;}
					}
					public bool wasWaitingForNextTouch{
						get{return actStateHandler.wasWaitingForNextTouch;}
					}
				public void PickUp(){
					actStateHandler.PickUp();
				}
				public void SetPickedUpState(){
					actStateHandler.SetPickedUpState();
				}
					public ISBActState pickedUpState{
						get{return actStateHandler.pickedUpState;}
					}
					public bool isPickingUp{
						get{return actStateHandler.isPickingUp;}
					}
					public bool wasPickingUp{
						get{return actStateHandler.wasPickingUp;}
					}
				public void Remove(){
					actStateHandler.Remove();
				}
					public ISBActState removedState{
						get{return actStateHandler.removedState;}
					}
					public bool isRemoving{
						get{return actStateHandler.isRemoving;}
					}
					public bool wasRemoving{
						get{return actStateHandler.wasRemoving;}
					}
				public void Add(){
					actStateHandler.Add();
				}
					public ISBActState addedState{
						get{return actStateHandler.addedState;}
					}
					public bool isAdding{
						get{return actStateHandler.isAdding;}
					}
					public bool wasAdding{
						get{return actStateHandler.wasAdding;}
					}
				public void MoveWithin(){
					actStateHandler.MoveWithin();
				}
					public ISBActState moveWithinState{
						get{return actStateHandler.moveWithinState;}
					}
					public bool isMovingWithin{
						get{return actStateHandler.isMovingWithin;}
					}
					public bool wasMovingWithin{
						get{return actStateHandler.wasMovingWithin;}
					}
				public ISBActProcess actProcess{
					get{return actStateHandler.actProcess;}
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
				public System.Func<IEnumeratorFake> waitForPointerUpCoroutine{
					get{return actStateHandler.waitForPointerUpCoroutine;}
				}
				public System.Func<IEnumeratorFake> waitForPickUpCoroutine{
					get{return actStateHandler.waitForPickUpCoroutine;}
				}
				public System.Func<IEnumeratorFake> pickUpCoroutine{
					get{return actStateHandler.pickUpCoroutine;}
				}
				public System.Func<IEnumeratorFake> waitForNextTouchCoroutine{
					get{return actStateHandler.waitForNextTouchCoroutine;}
				}
				public System.Func<IEnumeratorFake> removeCoroutine{
					get{return actStateHandler.removeCoroutine;}
				}
				public System.Func<IEnumeratorFake> addCoroutine{
					get{return actStateHandler.addCoroutine;}
				}
				public System.Func<IEnumeratorFake> moveWithinCoroutine{
					get{return actStateHandler.moveWithinCoroutine;}
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
				public bool isEqpStateNull{
					get{return eqpStateHandler.isEqpStateNull;}
				}
				public bool wasEqpStateNull{
					get{return eqpStateHandler.wasEqpStateNull;}
				}
				public void Equip(){
					eqpStateHandler.Equip();
				}
				public ISBEqpState equippedState{
					get{return eqpStateHandler.equippedState;}
				}
				public bool isEquipped{
					get{return eqpStateHandler.isEquipped;}
				}
				public bool wasEquipped{
					get{return eqpStateHandler.wasEquipped;}
				}
				public void Unequip(){
					eqpStateHandler.Unequip();
				}
				public ISBEqpState unequippedState{
					get{return eqpStateHandler.unequippedState;}
				}
				public bool isUnequipped{
					get{return eqpStateHandler.isUnequipped;}
				}
				public bool wasUnequipped{
					get{return eqpStateHandler.wasUnequipped;}
				}
				public ISBEqpProcess eqpProcess{
					get{return eqpStateHandler.eqpProcess;}
				}
				public void SetEqpProcessEngine(ISSEProcessEngine<ISBEqpProcess> engine){
					((SBEqpStateHandler)eqpStateHandler).SetEqpProcessEngine(engine);
				}
				public void SetAndRunEqpProcess(ISBEqpProcess process){
					eqpStateHandler.SetAndRunEqpProcess(process);
				}
				public System.Func<IEnumeratorFake> unequipCoroutine{
					get{return eqpStateHandler.unequipCoroutine;}
				}
				public System.Func<IEnumeratorFake> equipCoroutine{
					get{return eqpStateHandler.equipCoroutine;}
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
				public bool isMrkStateNull{
					get{return mrkStateHandler.isMrkStateNull;}
				}
				public bool wasMrkStateNull{
					get{return mrkStateHandler.wasMrkStateNull;}
				}
				public void Mark(){
					mrkStateHandler.Mark();
				}
				public ISBMrkState	markedState{
					get{return mrkStateHandler.markedState;}
				}
				public bool isMarked{
					get{return mrkStateHandler.isMarked;}
				}
				public bool wasMarked{
					get{return mrkStateHandler.wasMarked;}
				}
				public void Unmark(){
					mrkStateHandler.Unmark();
				}
				public ISBMrkState unmarkedState{
					get{return mrkStateHandler.unmarkedState;}
				}
				public bool isUnmarked{
					get{return mrkStateHandler.isUnmarked;}
				}
				public bool wasUnmarked{
					get{return mrkStateHandler.wasUnmarked;}
				}
				public ISBMrkProcess mrkProcess{
					get{return mrkStateHandler.mrkProcess;}
				}
				public void SetMrkProcessEngine(ISSEProcessEngine<ISBMrkProcess> engine){
					((SBMrkStateHandler)mrkStateHandler).SetMrkProcessEngine(engine);
				}
				public void SetAndRunMrkProcess(ISBMrkProcess process){
					mrkStateHandler.SetAndRunMrkProcess(process);
				}
				public System.Func<IEnumeratorFake> unmarkCoroutine{
					get{return mrkStateHandler.unmarkCoroutine;}
				}
				public System.Func<IEnumeratorFake> markCoroutine{
					get{return mrkStateHandler.markCoroutine;}
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
				if(isStackable && quantity > pickedAmount){
					pickedAmount ++;
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
			public InventoryItemInstance item{
				get{return itemHandler.item;}
			}
			public void SetItem(InventoryItemInstance item){
				itemHandler.SetItem(item);
			}
			public int pickedAmount{
				get{return itemHandler.pickedAmount;}
				set{itemHandler.pickedAmount = value;}
			}
			public bool isStackable{
				get{return itemHandler.isStackable;}
			}
			public int quantity{
				get{return itemHandler.quantity;}
			}
			public void SetQuantity(int quant){
				itemHandler.SetQuantity(quant);
			}
			public void UpdateEquipState(){
				if(item.isEquipped) Equip();
				else Unequip();
			}
		/* SG And Slots */
			public ISlotGroup sg{
				get{
					if(parent != null)
						return parent as ISlotGroup;
					return null;
				}
			}
			public virtual bool isPool{
				get{
					if(sg != null){
						return sg.isPool;
					}else
						throw new System.InvalidOperationException("Slottable.isPool: sg is not set");
				}
			}
			public bool isHierarchySetUp{
				get{
					return sg != null;
				}
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
			public int slotID{
				get{return slotHandler.slotID;}
			}
			public void SetSlotID(int i){
				slotHandler.SetSlotID(i);
			}
			public int newSlotID{
				get{return slotHandler.newSlotID;}
			}
			public void SetNewSlotID(int id){
				slotHandler.SetNewSlotID(id);
			}
			public bool isToBeAdded{
				get{return slotHandler.isToBeAdded;}
			}
			public bool isToBeRemoved{
				get{return slotHandler.isToBeRemoved;}
			}
		/* Others */
			public bool delayed{
				get{return m_delayed;}
				set{m_delayed = value;}
			}
				bool m_delayed = true;
			public void Refresh(){
				WaitForAction();
				itemHandler.pickedAmount = 0;
				slotHandler.SetNewSlotID(-2);
			}
			public bool ShareSGAndItem(ISlottable other){
				bool flag = true;
				flag &= this.sg == other.sg;
				flag &= this.item == other.item;
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
			public override ISlotSystemElement parent{
				get{
					if(ssm != null)
						return ssm.FindParent(this);
					else
						throw new System.InvalidOperationException("Slottable.parent: ssm is not set");
				}
			}
			public override bool Contains(ISlotSystemElement element){
				return false;
			}
			public override bool ContainsInHierarchy(ISlotSystemElement element){
				return false;
			}
		/*	Transaction	*/
			public bool isPickedUp{
				get{
					return taCache.pickedSB == (ISlottable)this;
				}
			}
			public bool passesPrePickFilter{
				get{return !taCache.IsTransactionGoingToBeRevert(this);}
			}
		/* hoverable */
			public IHoverable hoverable{
				get{
					if(m_hoverable != null)
						return m_hoverable;
					else
						throw new InvalidOperationException("hoverable not set");
				}
			}
				IHoverable m_hoverable;
			public void SetHoverable(IHoverable hoverable){
				m_hoverable = hoverable;
			}
			public ITransactionCache taCache{
				get{return hoverable.taCache;}
			}
			public void SetTACache(ITransactionCache taCache){
				hoverable.SetTACache(taCache);
			}
			public bool isHovered{
				get{return hoverable.isHovered;}
			}
			public void OnHoverEnter(){
				hoverable.OnHoverEnter();
			}
			public void OnHoverExit(){
				hoverable.OnHoverExit();
			}
	}
	public interface ISlottable: ISlotSystemElement, IHoverable, IItemHandler, ISBActStateHandler, ISBEqpStateHandler, ISBMrkStateHandler, ISlotHandler{
		/*	Commands	*/
			void Tap();
		/* Item Handling */
			void UpdateEquipState();
		/* SG And Slot */
			ISlotGroup sg{get;}
			bool isPool{get;}
			bool isHierarchySetUp{get;}
		/* Transaction */
			bool isPickedUp{get;}
			bool passesPrePickFilter{get;}
		/* Other */
			void Increment();
			bool delayed{get;set;}
			void Refresh();
			bool ShareSGAndItem(ISlottable other);
			void Destroy();
	}
}
