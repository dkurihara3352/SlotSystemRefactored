using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace SlotSystem{
	public class Slottable : SlotSystemElement, ISlottable{
		/*	States	*/
			public override void InitializeStates(){
				selStateHandler.InitializeStates();
				WaitForAction();
				ClearCurEqpState();
				Unmark();
			}
			/*	Selection state */
				public override ISSESelStateHandler selStateHandler{
					get{
						if(_selStateHandler == null)
							_selStateHandler = new SBSelStateHandler(this);
						return _selStateHandler;
					}
				}
					ISSESelStateHandler _selStateHandler;
			/*	Action State	*/
				public ISBActStateHandler actStateHandler{
					get{
						if(_actStateHandler == null)
							_actStateHandler = new SBActStateHandler(this);
						return _actStateHandler;
					}
				}
					ISBActStateHandler _actStateHandler;
				public void SetActSTateHandler(ISBActStateHandler actStateHandler){
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
						if(_eqpStateHandler == null)
							_eqpStateHandler = new SBEqpStateHandler(this);
						return _eqpStateHandler;
					}
				}
					ISBEqpStateHandler _eqpStateHandler;
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
						if(_mrkStateHandler == null)
							_mrkStateHandler = new SBMrkStateHandler(this);
						return _mrkStateHandler;
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
			SlottableCommand tapCommand{
				get{
					if(m_tapCommand == null)
						m_tapCommand = new SBTapCommand();
					return m_tapCommand;
				}
			}
				SlottableCommand m_tapCommand = new SBTapCommand();
			public void SetTapCommand(SlottableCommand comm){
				m_tapCommand = comm;
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
				public int slotID{
					get{return m_slotID;}
				}
					int m_slotID = -1;
				public void SetSlotID(int i){
					m_slotID = i;
				}
				public int newSlotID{
					get{return m_newSlotID;}
				}
					int m_newSlotID = -2;
				public void SetNewSlotID(int id){
					m_newSlotID = id;
				}
				public bool isToBeAdded{
					get{return slotID == -1;}
				}
				public bool isToBeRemoved{
					get{return newSlotID == -1;}
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
					SetNewSlotID(-2);
				}
				public bool ShareSGAndItem(ISlottable other){
					bool flag = true;
					flag &= this.sg == other.sg;
					flag &= this.item == other.item;
					return flag;
				}
				public void Destroy(){
					GameObject go = gameObject;
					DestroyImmediate(go);
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
			public ITransactionManager tam{
				get{
					if(m_tam != null)
						return m_tam;
					else
						throw new InvalidOperationException("tam not set");
				}
			}
				ITransactionManager m_tam;
			public void SetTAM(ITransactionManager tam){
				m_tam = tam;
			}
			public void ExecuteTransaction(){
				tam.ExecuteTransaction();
			}
			public ITransactionCache taCache{
				get{
					if(_taCache != null)
						return _taCache;
					else
						throw new InvalidOperationException("taCache not set");
				}
			}
				ITransactionCache _taCache;
			public void SetTACache(ITransactionCache taCache){
				_taCache = taCache;
			}
			public void SetPickedSB(){
				if(taCache != null){
					taCache.SetPickedSB((ISlottable)this);
				}else
					throw new System.InvalidOperationException("Slottable.SetPickedSB: ssm not set");
			}
			public bool isPickedUp{
				get{
					return taCache.pickedSB == (ISlottable)this;
				}
			}
			public bool passesPrePickFilter{
				get{return !taCache.IsTransactionGoingToBeRevert(this);}
			}
			public void CreateTAResult(){
				taCache.CreateTransactionResults();
			}
			public void UpdateTA(){
				taCache.UpdateFields();
			}
			public ITAMActStateHandler tamStateHandler{
				get{
					if(_tamStateHandler != null)
						return _tamStateHandler;
					else
						throw new InvalidOperationException("tamStateHandler not set");
				}
			}
				ITAMActStateHandler _tamStateHandler;
			public void SetTAMStateHandler(ITAMActStateHandler handler){
				_tamStateHandler = handler;
			}
			public void Probe(){
				tamStateHandler.Probe();
			}
			public ITransactionIconHandler iconHandler{
				get{
					if(_iconHandler != null)
						return _iconHandler;
					else	
						throw new InvalidOperationException("iconHandler not set");
				}
			}
				ITransactionIconHandler _iconHandler;
			public void SetIconHandler(ITransactionIconHandler iconHandler){
				_iconHandler = iconHandler;
			}
			public void SetDIcon1(){
				DraggedIcon dIcon = new DraggedIcon(this, iconHandler);
				iconHandler.SetDIcon1(dIcon);
			}
			public void SetDIcon2(){
				DraggedIcon dIcon = new DraggedIcon(this, iconHandler);
				iconHandler.SetDIcon2(dIcon);
			}
			public IHoverable hoverable{
				get{
					if(m_hoverable == null)
						m_hoverable = new Hoverable(this, taCache);
					return m_hoverable;
				}
			}
				IHoverable m_hoverable;
			public void SetHoverable(IHoverable hoverable){
				m_hoverable = hoverable;
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
	public interface ISlottable: ISlotSystemElement, IHoverable, IItemHandler, ISBActStateHandler, ISBEqpStateHandler, ISBMrkStateHandler{
		/*	States and Processes	*/
			/* States */
		/*	Commands	*/
			void Tap();
		/* Item Handling */
			void UpdateEquipState();
		/* SG And Slot */
			ISlotGroup sg{get;}
			bool isPool{get;}
			bool isHierarchySetUp{get;}
			int slotID{get;}
			void SetSlotID(int i);
			int newSlotID{get;}
			void SetNewSlotID(int id);
			bool isToBeAdded{get;}
			bool isToBeRemoved{get;}
		/* Transaction */
			void SetPickedSB();
			void SetDIcon1();
			void SetDIcon2();
			void CreateTAResult();
			void UpdateTA();
			void Probe();
			IHoverable hoverable{get;}
			bool isPickedUp{get;}
			bool passesPrePickFilter{get;}
			void ExecuteTransaction();
		/* Other */
			void Increment();
			bool delayed{get;set;}
			void Refresh();
			bool ShareSGAndItem(ISlottable other);
			void Destroy();
	}
}
