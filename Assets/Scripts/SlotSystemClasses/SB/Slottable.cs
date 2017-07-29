﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace SlotSystem{
	public class Slottable : SlotSystemElement, ISlottable{
		/*	States	*/
			/* Selection */
				public override void Activate(){
					ssm.ReferToTAAndUpdateSelState(this);
				}
			/*	Action State	*/
				ISSEStateEngine<ISBActState> actStateEngine{
					get{
						if(m_actStateEngine == null)
							m_actStateEngine = new SSEStateEngine<ISBActState>(this);
						return m_actStateEngine;
					}
					}ISSEStateEngine<ISBActState> m_actStateEngine;
				public ISBActState curActState{
					get{return actStateEngine.curState;}
				}
				public ISBActState prevActState{
					get{return actStateEngine.prevState;}
				}
				void SetActState(ISBActState state){
					actStateEngine.SetState(state);
					if(state == null && actProcess != null)
						SetAndRunActProcess(null);
				}
				public bool isPrevActStateNull{get{return prevActState == null;}}
				public bool isCurActStateNull{get{return curActState == null;}}
				public  void ClearCurActState(){SetActState(null);}
				/* Act states */
					public ISBActState waitForActionState{
						get{
							if(m_waitForActionState == null)
								m_waitForActionState = new WaitForActionState();
							return m_waitForActionState;
						}
						}ISBActState m_waitForActionState;
						public virtual void WaitForAction(){SetActState(waitForActionState);}
						public virtual bool isWaitingForAction{get{return curActState == waitForActionState;}}
						public virtual bool wasWaitingForAction{get{return prevActState == waitForActionState;}}
					public ISBActState waitForPointerUpState{
						get{
							if(m_waitForPointerUpState == null)
								m_waitForPointerUpState = new WaitForPointerUpState();
							return m_waitForPointerUpState;
						}
						}ISBActState m_waitForPointerUpState;
						public virtual void WaitForPointerUp(){SetActState(waitForPointerUpState);}
						public virtual bool isWaitingForPointerUp{get{return curActState == waitForPointerUpState;}}
						public virtual bool wasWaitingForPointerUp{get{return prevActState == waitForPointerUpState;}}
					public ISBActState waitForPickUpState{
						get{
							if(m_waitForPickUpState == null)
								m_waitForPickUpState = new WaitForPickUpState();
							return m_waitForPickUpState;
						}
						}ISBActState m_waitForPickUpState;
						public virtual void WaitForPickUp(){SetActState(waitForPickUpState);}
						public virtual bool isWaitingForPickUp{get{return curActState == waitForPickUpState;}}
						public virtual bool wasWaitingForPickUp{get{return prevActState == waitForPickUpState;}}
					public ISBActState waitForNextTouchState{
						get{
							if(m_waitForNextTouchState == null)
								m_waitForNextTouchState = new WaitForNextTouchState();
							return m_waitForNextTouchState;
						}
						}ISBActState m_waitForNextTouchState;
						public virtual void WaitForNextTouch(){SetActState(waitForNextTouchState);}
						public virtual bool isWaitingForNextTouch{get{return curActState == waitForNextTouchState;}}
						public virtual bool wasWaitingForNextTouch{get{return prevActState == waitForNextTouchState;}}
					public ISBActState pickedUpState{
						get{
							if(m_pickedUpState == null)
								m_pickedUpState = new PickingUpState();
							return m_pickedUpState;
						}
						}ISBActState m_pickedUpState;
						public virtual void PickUp(){
							SetActState(pickedUpState);
							m_pickedAmount = 1;
						}
						public virtual bool isPickingUp{get{return curActState == pickedUpState;}}
						public virtual bool wasPickingUp{get{return prevActState == pickedUpState;}}
					public ISBActState removedState{
						get{
							if(m_removedState == null)
								m_removedState = new SBRemovedState();
							return m_removedState;			
						}
						}ISBActState m_removedState;
						public virtual void Remove(){SetActState(removedState);}
						public virtual bool isRemoving{get{return curActState == removedState;}}
						public virtual bool wasRemoving{get{return prevActState == removedState;}}
					public ISBActState addedState{
						get{
							if(m_addedState == null)
								m_addedState = new SBAddedState();
							return m_addedState;			
						}
						}ISBActState m_addedState;
						public virtual void Add(){SetActState(addedState);}
						public virtual bool isAdding{get{return curActState == addedState;}}
						public virtual bool wasAdding{get{return prevActState == addedState;}}
					public ISBActState moveWithinState{
						get{
							if(m_moveWithinState == null)
								m_moveWithinState = new MoveWithinState();
							return m_moveWithinState;			
						}
						}ISBActState m_moveWithinState;
						public virtual void MoveWithin(){SetActState(moveWithinState);}
						public virtual bool isMovingWithin{get{return curActState == moveWithinState;}}
						public virtual bool wasMovingWithin{get{return prevActState == moveWithinState;}}
			/*	Equip State	*/
				ISSEStateEngine<ISBEqpState> eqpStateEngine{
					get{
						if(m_eqpStateEngine == null)
							m_eqpStateEngine = new SSEStateEngine<ISBEqpState>(this);
						return m_eqpStateEngine;
					}
					}ISSEStateEngine<ISBEqpState> m_eqpStateEngine;
				void SetEqpStateEngine(ISSEStateEngine<ISBEqpState> engine){m_eqpStateEngine = engine;}
				ISBEqpState curEqpState{
					get{return eqpStateEngine.curState;}
				}
				ISBEqpState prevEqpState{
					get{return eqpStateEngine.prevState;}
				}
				void SetEqpState(ISBEqpState state){
					eqpStateEngine.SetState(state);
					if(state == null && eqpProcess != null)
						SetAndRunEqpProcess(null);
				}
				public virtual bool isCurEqpStateNull{get{return curEqpState == null;}}
				public virtual bool isPrevEqpStateNull{get{return prevEqpState == null;}}
				public virtual void ClearCurEqpState(){SetEqpState(null);}
				
				/* Eqp states */
					public ISBEqpState equippedState{
						get{
							if(m_equippedState == null)
								m_equippedState = new SBEquippedState();
							return m_equippedState;
							}
						}ISBEqpState m_equippedState;
						public virtual void Equip(){SetEqpState(equippedState);}
						public virtual bool isEquipped{get{ return curEqpState == equippedState;}}
						public virtual bool wasEquipped{get{return prevEqpState == equippedState;}}
					public ISBEqpState unequippedState{
						get{
							if(m_unequippedState == null)
								m_unequippedState = new SBUnequippedState();
							return m_unequippedState;
						}
						}ISBEqpState m_unequippedState;
						public virtual void Unequip(){SetEqpState(unequippedState);}
						public virtual bool isUnequipped{get{ return curEqpState == unequippedState;}}
						public virtual bool wasUnequipped{get{ return prevEqpState == unequippedState;}}
			/*	Mark state	*/
				ISSEStateEngine<ISBMrkState> mrkStateEngine{
					get{
						if(m_markStateEngine == null)
							m_markStateEngine = new SSEStateEngine<ISBMrkState>(this);
						return m_markStateEngine;
					}
					}ISSEStateEngine<ISBMrkState> m_markStateEngine;
				void SetMrkStateEngine(ISSEStateEngine<ISBMrkState> engine){m_markStateEngine = engine;}
				ISBMrkState curMrkState{
					get{return mrkStateEngine.curState;}
				}
				ISBMrkState prevMrkState{
					get{return mrkStateEngine.prevState;}
				}
				void SetMrkState(ISBMrkState state){
					mrkStateEngine.SetState(state);
					if(state == null && mrkProcess != null)
						SetAndRunMrkProcess(null);
				}
				public virtual bool isCurMrkStateNull{get{return curMrkState == null;}}
				public virtual bool isPrevMrkStateNull{get{return prevMrkState == null;}}
				public virtual void ClearCurMrkState(){SetMrkState(null);}
				/* Mrk states */
					public ISBMrkState markedState{
						get{
							if(m_markedState == null)
								m_markedState = new SBMarkedState();
							return m_markedState;
						}
						}ISBMrkState m_markedState;
						public virtual void Mark(){SetMrkState(markedState);}
						public virtual bool isMarked{get{return curMrkState == markedState;}}
						public virtual bool wasMarked{get{return prevMrkState == markedState;}}
					public ISBMrkState unmarkedState{
						get{
							if(m_unmarkedState == null)
								m_unmarkedState = new SBUnmarkedState();
							return m_unmarkedState;
						}
						}ISBMrkState m_unmarkedState;
						public virtual void Unmark(){SetMrkState(unmarkedState);}
						public virtual bool isUnmarked{get{return curMrkState == unmarkedState;}}
						public virtual bool wasUnmarked{get{return prevMrkState == markedState;}}
		/*	processes	*/
			ISBCoroutineFactory coroutineFactory{
				get{
					if(m_coroutineFactory == null)
						m_coroutineFactory = new SBCoroutineFactory(this);
					return m_coroutineFactory;
				}
				}ISBCoroutineFactory m_coroutineFactory;
				public void SetCoroutineFactory(ISBCoroutineFactory factory){m_coroutineFactory = factory;}
			/*	Action Process	*/
				public ISSEProcessEngine<ISBActProcess> actProcEngine{
					get{
						if(m_actProcEngine == null)
							m_actProcEngine = new SSEProcessEngine<ISBActProcess>();
						return m_actProcEngine;
					}
					}ISSEProcessEngine<ISBActProcess> m_actProcEngine;
				public virtual void SetActProcessEngine(ISSEProcessEngine<ISBActProcess> engine){m_actProcEngine = engine;}
				public virtual ISBActProcess actProcess{
					get{return actProcEngine.process;}
				}
				public virtual void SetAndRunActProcess(ISBActProcess process){
					actProcEngine.SetAndRunProcess(process);
				}
				public void ExpireActProcess(){if(actProcess != null) actProcess.Expire();}
				/* Coroutine */
					public System.Func<IEnumeratorFake> waitForPointerUpCoroutine{get{return coroutineFactory.MakeWaitForPointerUpCoroutine();}}
					public System.Func<IEnumeratorFake> waitForPickUpCoroutine{get{return coroutineFactory.MakeWaitForPickUpCoroutine();}}
					public System.Func<IEnumeratorFake> pickUpCoroutine{get{return coroutineFactory.MakePickUpCoroutine();}}
					public System.Func<IEnumeratorFake> waitForNextTouchCoroutine{get{return coroutineFactory.MakeWaitForNextTouchCoroutine();}}
					public System.Func<IEnumeratorFake> removeCoroutine{get{return coroutineFactory.MakeRemoveCoroutine();}}
					public System.Func<IEnumeratorFake> addCoroutine{get{return coroutineFactory.MakeAddCoroutine();}}
					public System.Func<IEnumeratorFake> moveWithinCoroutine{get{return coroutineFactory.MakeMoveWithinCoroutine();}}
			/*	Equip Process	*/
				public ISSEProcessEngine<ISBEqpProcess> eqpProcEngine{
					get{
						if(m_eqpProcEngine == null)
							m_eqpProcEngine = new SSEProcessEngine<ISBEqpProcess>();
						return m_eqpProcEngine;
					}
					}ISSEProcessEngine<ISBEqpProcess> m_eqpProcEngine;
				public void SetEqpProcessEngine(ISSEProcessEngine<ISBEqpProcess> engine){m_eqpProcEngine = engine;}
				public virtual ISBEqpProcess eqpProcess{
					get{return eqpProcEngine.process;}
				}
				public virtual void SetAndRunEqpProcess(ISBEqpProcess process){
					eqpProcEngine.SetAndRunProcess(process);
				}
				public void ExpireEqpProcess(){if(eqpProcess != null) eqpProcess.Expire();}
				/* Coroutine */
					public System.Func<IEnumeratorFake> equipCoroutine{get{return coroutineFactory.MakeEquipCoroutine();}}
					public System.Func<IEnumeratorFake> unequipCoroutine{get{return coroutineFactory.MakeUnequipCoroutine();}}
			/*	Mark Process	*/
				public ISSEProcessEngine<ISBMrkProcess> mrkProcEngine{
					get{
						if(m_mrkProcEngine == null)
							m_mrkProcEngine = new SSEProcessEngine<ISBMrkProcess>();
						return m_mrkProcEngine;
					}
					}ISSEProcessEngine<ISBMrkProcess> m_mrkProcEngine;
				public void SetMrkProcessEngine(ISSEProcessEngine<ISBMrkProcess> engine){m_mrkProcEngine = engine;}
				public virtual ISBMrkProcess mrkProcess{
					get{return mrkProcEngine.process;}
				}
				public virtual void SetAndRunMrkProcess(ISBMrkProcess process){
					mrkProcEngine.SetAndRunProcess(process);
				}
				public void ExpireMrkProcess(){if(mrkProcess != null) mrkProcess.Expire();}
				/* Coroutine */
					public System.Func<IEnumeratorFake> markCoroutine{get{return coroutineFactory.MakeMarkCoroutine();}}
					public System.Func<IEnumeratorFake> unmarkCoroutine{get{return coroutineFactory.MakeUnmarkCoroutine();}}
		/*	commands	*/
			public SlottableCommand TapCommand{
				get{
					if(m_tapCommand == null)
						m_tapCommand = new SBTapCommand();
					return m_tapCommand;
				}
				}SlottableCommand m_tapCommand = new SBTapCommand();
				public void SetTapCommand(SlottableCommand comm){m_tapCommand = comm;}
				public virtual void Tap(){
					m_tapCommand.Execute(this);
				}
		/*	public fields	*/
			public bool isHierarchySetUp{
				get{
					return sg != null;
				}
			}
			public bool delayed{
				get{return m_delayed;}
				set{m_delayed = value;}
				}bool m_delayed = true;
			public int pickedAmount{
				get{return m_pickedAmount;}
				set{m_pickedAmount = value;}
				}int m_pickedAmount = 0;
			public InventoryItemInstance itemInst{
					get{return (InventoryItemInstance)item;}
				}
				SlottableItem item{get{return m_item;}} SlottableItem m_item;
				public void SetItem(SlottableItem item){
					m_item = item;
				}
			public int slotID{
				get{return m_slotID;}
				}int m_slotID = -1;
				public void SetSlotID(int i){
					m_slotID = i;
				}
			public int newSlotID{
				get{return m_newSlotID;}
				}int m_newSlotID = -2;
				public void SetNewSlotID(int id){
					m_newSlotID = id;
				}
			public bool isToBeAdded{get{return slotID == -1;}}
			public bool isToBeRemoved{get{return newSlotID == -1;}}
			public ISlotGroup sg{
				get{
					if(parent != null)
						return parent as ISlotGroup;
					return null;
				}
			}
			public virtual bool isPickedUp{
				get{
					return ssm.pickedSB == (ISlottable)this;
				}
			}
			public virtual bool isStackable{
				get{return itemInst.Item.IsStackable;}
			}
			public bool passesPrePickFilter{
				get{
					bool isFilteredIn = false;
					ssm.PrePickFilter(this, out isFilteredIn);
					return isFilteredIn;
				}
			}
			public int quantity{get{return itemInst.quantity;}}
			public void SetQuantity(int quant){
				itemInst.quantity = quant;
			}
		/*	SlotSystemElement imple	*/
			/*	fields	*/
				public override ISlotSystemElement this[int i]{get{return null;}}
				public override IEnumerable<ISlotSystemElement> elements{
					get{return null;}
				}
				public override string eName{
					get{return SlotSystemUtil.SBName(this);}
				}
				public override ISlotSystemElement parent{
					get{
						if(ssm != null)
							return ssm.FindParent(this);
						else
							throw new System.InvalidOperationException("Slottable.parent: ssm is not set");
					}
				}
				public override ISlotSystemBundle immediateBundle{
					get{
						if(parent == null)
							return null;
						return parent.immediateBundle;
					}
				}
				public override int level{
					get{return sg.level +1;}
				}
			/*	methods	*/
				public override IEnumerator<ISlotSystemElement> GetEnumerator(){
					yield return null;
					}
				public override bool Contains(ISlotSystemElement element){
					return false;
				}
				public override bool ContainsInHierarchy(ISlotSystemElement element){
					return false;
				}
				public override void PerformInHierarchy(System.Action<ISlotSystemElement> act){
					act(this);
				}
				public override void PerformInHierarchy(System.Action<ISlotSystemElement, object> act, object obj){
					act(this, obj);
				}
				public override void PerformInHierarchy<T>(System.Action<ISlotSystemElement, IList<T>> act, IList<T> obj){
					act(this, obj);
				}
		/*	Event methods	*/
			/*	Action Event	*/
				public void OnPointerDown(PointerEventDataFake eventDataMock){
					curActState.OnPointerDown(this, eventDataMock);
				}
				public void OnPointerUp(PointerEventDataFake eventDataMock){
					curActState.OnPointerUp(this, eventDataMock);
				}
				public void OnDeselected(PointerEventDataFake eventDataMock){
					curActState.OnDeselected(this, eventDataMock);
				}
				public void OnEndDrag(PointerEventDataFake eventDataMock){
					curActState.OnEndDrag(this, eventDataMock);
				}
		/*	other Interface implementation	*/
			int IComparable.CompareTo(object other){
				if(!(other is ISlottable))
					throw new InvalidOperationException("CompareTo: no a slottable");
				return CompareTo((ISlottable)other);
			}
			public int CompareTo(ISlottable other){
				return this.itemInst.CompareTo(other.itemInst);
			}
			public static bool operator > (Slottable a, ISlottable b){
				return a.CompareTo(b) > 0;
			}
			public static bool operator < (Slottable a, ISlottable b){
				return a.CompareTo(b) < 0;
			}
		/*	methods	*/
			public override void InitializeStates(){
				Deactivate();
				WaitForAction();
				SetEqpState(null);
				SetMrkState(unmarkedState);
			}
			public override void SetHierarchy(){}
			public virtual void Increment(){
				SetActState(pickedUpState);
				if(isStackable && quantity > m_pickedAmount){
					m_pickedAmount ++;
				}
			}
			public virtual void ExecuteTransaction(){
				ssm.ExecuteTransaction();
			}
			public void UpdateEquipState(){
				if(itemInst.isEquipped) Equip();
				else Unequip();
			}
			public void Refresh(){
				WaitForAction();
				pickedAmount = 0;
				SetNewSlotID(-2);
			}

			
			public bool ShareSGAndItem(ISlottable other){
				bool flag = true;
				flag &= this.sg == other.sg;
				flag &= this.itemInst == other.itemInst;
				return flag;
			}
			public bool HaveCommonItemFamily(ISlottable other){
				if(this.itemInst is BowInstance)
					return (other.itemInst is BowInstance);
				else if(this.itemInst is WearInstance)
					return (other.itemInst is WearInstance);
				else if(this.itemInst is CarriedGearInstance)
					return (other.itemInst is CarriedGearInstance);
				else if(this.itemInst is PartsInstance)
					return (other.itemInst is PartsInstance);
				else 
					return false;
			}
			public void Destroy(){
				GameObject go = gameObject;
				DestroyImmediate(go);
			}
		/*	Forward	*/
			public virtual void SetPickedSB(){
				if(ssm != null){
					ssm.SetPickedSB((ISlottable)this);
				}else
					throw new System.InvalidOperationException("Slottable.SetPickedSB: ssm not set");
			}
			public virtual void Probe(){
				ssm.Probe();
			}
			public virtual void SetDIcon1(){
				DraggedIcon dIcon = new DraggedIcon(this);
				ssm.SetDIcon1(dIcon);
			}
			public virtual void SetDIcon2(){
				DraggedIcon dIcon = new DraggedIcon(this);
				ssm.SetDIcon2(dIcon);
			}
			public virtual void CreateTAResult(){
				ssm.CreateTransactionResults();
			}
			public virtual void UpdateTA(){
				ssm.UpdateTransaction();
			}
			public virtual bool isHovered{
				get{return ssm.hovered == (ISlotSystemElement)this;}
			}
			public virtual bool isPool{
				get{
					if(sg != null){
						return sg.isPool;
					}else
						throw new System.InvalidOperationException("Slottable.isPool: sg is not set");
				}
			}
			public void SetHovered(){
				ssm.SetHovered((ISlottable)this);
			}
	}
	public interface ISlottable: ISlotSystemElement ,IComparable<ISlottable>, IComparable{
		/*	States and Processes	*/
			/* States */
				/* ActStates */
					bool isPrevActStateNull{get;}
					bool isCurActStateNull{get;}
					void ClearCurActState();
					ISBActState waitForActionState{get;}
						void WaitForAction();
						bool isWaitingForAction{get;}
						bool wasWaitingForAction{get;}
					ISBActState waitForPointerUpState{get;}
						void WaitForPointerUp();
						bool isWaitingForPointerUp{get;}
						bool wasWaitingForPointerUp{get;}
					ISBActState waitForPickUpState{get;}
						void WaitForPickUp();
						bool isWaitingForPickUp{get;}
						bool wasWaitingForPickUp{get;}
					ISBActState waitForNextTouchState{get;}
						void WaitForNextTouch();
						bool isWaitingForNextTouch{get;}
						bool wasWaitingForNextTouch{get;}
					ISBActState pickedUpState{get;}
						void PickUp();
						bool isPickingUp{get;}
						bool wasPickingUp{get;}
					ISBActState removedState{get;}
						void Remove();
						bool isRemoving{get;}
						bool wasRemoving{get;}
					ISBActState addedState{get;}
						void Add();
						bool isAdding{get;}
						bool wasAdding{get;}
					ISBActState moveWithinState{get;}
						void MoveWithin();
						bool isMovingWithin{get;}
						bool wasMovingWithin{get;}
				/* Eqp States */
					bool isCurEqpStateNull{get;}
					bool isPrevEqpStateNull{get;}
					void ClearCurEqpState();
					ISBEqpState equippedState{get;}
						void Equip();
						bool isEquipped{get;}
						bool wasEquipped{get;}
					ISBEqpState unequippedState{get;}
						void Unequip();
						bool isUnequipped{get;}
						bool wasUnequipped{get;}
				/* Mrk States */
					bool isCurMrkStateNull{get;}
					bool isPrevMrkStateNull{get;}
					void ClearCurMrkState();
					ISBMrkState	markedState{get;}
						void Mark();
						bool isMarked{get;}
						bool wasMarked{get;}
					ISBMrkState unmarkedState{get;}
						void Unmark();
						bool isUnmarked{get;}
						bool wasUnmarked{get;}
			/* Process */
				ISSEProcessEngine<ISBActProcess> actProcEngine{get;}
					void SetActProcessEngine(ISSEProcessEngine<ISBActProcess> engine);
					ISBActProcess actProcess{get;}
					void SetAndRunActProcess(ISBActProcess process);
					void ExpireActProcess();
						System.Func<IEnumeratorFake> waitForPointerUpCoroutine{get;}
						System.Func<IEnumeratorFake> waitForPickUpCoroutine{get;}
						System.Func<IEnumeratorFake> pickUpCoroutine{get;}
						System.Func<IEnumeratorFake> waitForNextTouchCoroutine{get;}
						System.Func<IEnumeratorFake> removeCoroutine{get;}
						System.Func<IEnumeratorFake> addCoroutine{get;}
						System.Func<IEnumeratorFake> moveWithinCoroutine{get;}
				ISSEProcessEngine<ISBEqpProcess> eqpProcEngine{get;}
					void SetEqpProcessEngine(ISSEProcessEngine<ISBEqpProcess> engine);
					ISBEqpProcess eqpProcess{get;}
					void SetAndRunEqpProcess(ISBEqpProcess process);
					void ExpireEqpProcess();
						System.Func<IEnumeratorFake> unequipCoroutine{get;}
						System.Func<IEnumeratorFake> equipCoroutine{get;}

				ISSEProcessEngine<ISBMrkProcess> mrkProcEngine{get;}
					void SetMrkProcessEngine(ISSEProcessEngine<ISBMrkProcess> engine);
					ISBMrkProcess mrkProcess{get;}
					void SetAndRunMrkProcess(ISBMrkProcess process);
					void ExpireMrkProcess();
						System.Func<IEnumeratorFake> unmarkCoroutine{get;}
						System.Func<IEnumeratorFake> markCoroutine{get;}
		/*	Commands	*/
			void Tap();
		/*	fields	*/
			bool isHierarchySetUp{get;}
			bool delayed{get;set;}
			int pickedAmount{get;set;}
			void SetItem(SlottableItem item);
			InventoryItemInstance itemInst{get;}
			int slotID{get;}
			void SetSlotID(int i);
			int newSlotID{get;}
			void SetNewSlotID(int id);
			bool isToBeAdded{get;}
			bool isToBeRemoved{get;}
			ISlotGroup sg{get;}
			bool isPickedUp{get;}
			bool isStackable{get;}
			bool passesPrePickFilter{get;}
			int quantity{get;}
			void SetQuantity(int quant);
		/*	Event Methods	*/
			void OnPointerDown(PointerEventDataFake eventDataMock);
			void OnPointerUp(PointerEventDataFake eventDataMock);
			void OnDeselected(PointerEventDataFake eventDataMock);
			void OnEndDrag(PointerEventDataFake eventDataMock);
		/*	Methods	*/
			void Increment();
			void ExecuteTransaction();
			void UpdateEquipState();
			void Refresh();
			bool ShareSGAndItem(ISlottable other);
			bool HaveCommonItemFamily(ISlottable other);
			void Destroy();
		/*	Forward	*/
			void SetPickedSB();
			void SetDIcon1();
			void SetDIcon2();
			void CreateTAResult();
			void UpdateTA();
			bool isHovered{get;}
			bool isPool{get;}
			void SetHovered();
			void Probe();
	}
	public class SBCoroutineFactory: ISBCoroutineFactory{
		ISlottable sb;
		public SBCoroutineFactory(ISlottable sb){this.sb = sb;}

		public System.Func<IEnumeratorFake> MakeWaitForPointerUpCoroutine(){
			return DefaultWaitForPointerUpCoroutine;
			}
			IEnumeratorFake DefaultWaitForPointerUpCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeWaitForPickUpCoroutine(){
			return DefaultWaitForPickUpCoroutine;
			}
			IEnumeratorFake DefaultWaitForPickUpCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakePickUpCoroutine(){
			return DefaultPickUpCoroutine;
			}
			IEnumeratorFake DefaultPickUpCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeWaitForNextTouchCoroutine(){
			return DefaultWaitForNextTouchCoroutine;
			}
			IEnumeratorFake DefaultWaitForNextTouchCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeRemoveCoroutine(){
			return DefaultRemoveCoroutine;
			}
			IEnumeratorFake DefaultRemoveCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeAddCoroutine(){
			return DefaultAddCoroutine;
			}
			IEnumeratorFake DefaultAddCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeMoveWithinCoroutine(){
			return DefaultMoveWithinCoroutine;
			}
			IEnumeratorFake DefaultMoveWithinCoroutine(){return null;}

		public System.Func<IEnumeratorFake> MakeEquipCoroutine(){
			return DefaultEquipCoroutine;
			}
			IEnumeratorFake DefaultEquipCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeUnequipCoroutine(){
			return DefaultUnequipCoroutine;
			}
			IEnumeratorFake DefaultUnequipCoroutine(){return null;}

		public System.Func<IEnumeratorFake> MakeMarkCoroutine(){
			return DefaultMarkCoroutine;
			}
			IEnumeratorFake DefaultMarkCoroutine(){return null;}
		public System.Func<IEnumeratorFake> MakeUnmarkCoroutine(){
			return DefaultUnmarkCoroutine;
			}
			IEnumeratorFake DefaultUnmarkCoroutine(){return null;}
	}
	public interface ISBCoroutineFactory{
		System.Func<IEnumeratorFake> MakeWaitForPointerUpCoroutine();
		System.Func<IEnumeratorFake> MakeWaitForPickUpCoroutine();
		System.Func<IEnumeratorFake> MakePickUpCoroutine();
		System.Func<IEnumeratorFake> MakeWaitForNextTouchCoroutine();
		System.Func<IEnumeratorFake> MakeRemoveCoroutine();
		System.Func<IEnumeratorFake> MakeAddCoroutine();
		System.Func<IEnumeratorFake> MakeMoveWithinCoroutine();

		System.Func<IEnumeratorFake> MakeEquipCoroutine();
		System.Func<IEnumeratorFake> MakeUnequipCoroutine();

		System.Func<IEnumeratorFake> MakeMarkCoroutine();
		System.Func<IEnumeratorFake> MakeUnmarkCoroutine();		
	}
}
