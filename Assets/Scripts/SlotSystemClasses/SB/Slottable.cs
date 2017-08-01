using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace SlotSystem{
	public class Slottable : SlotSystemElement, ISlottable{
		/*	States	*/
			/* Selection */
				public override void Activate(){
					if(tam.IsTransactionResultRevertFor(hoverable) == false)
						Focus();
					else
						Defocus();
				}
				public override void Deselect(){
					Activate();
				}
			/*	Action State	*/
				ISSEStateEngine<ISBActState> actStateEngine{
					get{
						if(m_actStateEngine == null)
							m_actStateEngine = new SSEStateEngine<ISBActState>(this);
						return m_actStateEngine;
					}
				}
					ISSEStateEngine<ISBActState> m_actStateEngine;
				void SetActState(ISBActState state){
					actStateEngine.SetState(state);
					if(state == null && actProcess != null)
						SetAndRunActProcess(null);
				}
				public ISBActState curActState{
					get{return actStateEngine.curState;}
				}
				public ISBActState prevActState{
					get{return actStateEngine.prevState;}
				}
				/* Act states */
					ISBActStateFactory actStateFactory{
						get{
							if(m_actStateFactory == null)
								m_actStateFactory = new SBActStateFactory();
							return m_actStateFactory;
						}
					}
						ISBActStateFactory m_actStateFactory;
						public void SetActStateFactory(ISBActStateFactory factory){
							m_actStateFactory = factory;
						}
					public void ClearCurActState(){
						SetActState(null);
					}
						public bool wasActStateNull{
							get{return prevActState == null;}
						}
						public bool isActStateNull{
							get{return curActState == null;}
						}
					public virtual void WaitForAction(){
						SetActState(waitForActionState);
					}
						public ISBActState waitForActionState{
							get{return actStateFactory.MakeWaitForActionState();}
						}
						public virtual bool isWaitingForAction{
							get{return curActState == waitForActionState;}
						}
						public virtual bool wasWaitingForAction{
							get{return prevActState == waitForActionState;}
						}
					public virtual void WaitForPointerUp(){
						SetActState(waitForPointerUpState);
					}
						public ISBActState waitForPointerUpState{
							get{return actStateFactory.MakeWaitForPointerUpState();}
						}
						public virtual bool isWaitingForPointerUp{
							get{return curActState == waitForPointerUpState;}
						}
						public virtual bool wasWaitingForPointerUp{
							get{return prevActState == waitForPointerUpState;}
						}
					public virtual void WaitForPickUp(){
						SetActState(waitForPickUpState);
					}
						public ISBActState waitForPickUpState{
							get{return actStateFactory.MakeWaitForPickUpState();}
						}
						public virtual bool isWaitingForPickUp{
							get{return curActState == waitForPickUpState;}
						}
						public virtual bool wasWaitingForPickUp{
							get{return prevActState == waitForPickUpState;}
						}
					public virtual void WaitForNextTouch(){
						SetActState(waitForNextTouchState);
					}
						public ISBActState waitForNextTouchState{
							get{return actStateFactory.MakeWaitForNextTouchState();}
						}
						public virtual bool isWaitingForNextTouch{
							get{return curActState == waitForNextTouchState;}
						}
						public virtual bool wasWaitingForNextTouch{
							get{return prevActState == waitForNextTouchState;}
						}
					public virtual void PickUp(){
						SetActState(pickedUpState);
						m_pickedAmount = 1;
					}
						public ISBActState pickedUpState{
							get{return actStateFactory.MakePickingUpState();}
						}
						public virtual bool isPickingUp{
							get{return curActState == pickedUpState;}
						}
						public virtual bool wasPickingUp{
							get{return prevActState == pickedUpState;}
						}
					public virtual void Remove(){
						SetActState(removedState);
					}
						public ISBActState removedState{
							get{return actStateFactory.MakeRemovedState();}
						}
						public virtual bool isRemoving{
							get{return curActState == removedState;}
						}
						public virtual bool wasRemoving{
							get{return prevActState == removedState;}
						}
					public virtual void Add(){
						SetActState(addedState);
					}
						public ISBActState addedState{
							get{return actStateFactory.MakeAddedState();}
						}
						public virtual bool isAdding{
							get{return curActState == addedState;}
						}
						public virtual bool wasAdding{
							get{return prevActState == addedState;}
						}
					public virtual void MoveWithin(){
						SetActState(moveWithinState);
					}
						public ISBActState moveWithinState{
							get{return actStateFactory.MakeMoveWithinState();}
						}
						public virtual bool isMovingWithin{
							get{return curActState == moveWithinState;}
						}
						public virtual bool wasMovingWithin{
							get{return prevActState == moveWithinState;}
						}
			/*	Equip State	*/
				ISSEStateEngine<ISBEqpState> eqpStateEngine{
					get{
						if(m_eqpStateEngine == null)
							m_eqpStateEngine = new SSEStateEngine<ISBEqpState>(this);
						return m_eqpStateEngine;
					}
				}
					ISSEStateEngine<ISBEqpState> m_eqpStateEngine;
				void SetEqpState(ISBEqpState state){
					eqpStateEngine.SetState(state);
					if(state == null && eqpProcess != null)
						SetAndRunEqpProcess(null);
				}
				ISBEqpState curEqpState{
					get{return eqpStateEngine.curState;}
				}
				ISBEqpState prevEqpState{
					get{return eqpStateEngine.prevState;}
				}
				/* Eqp states */
					ISBEqpStateFactory eqpStateFactory{
						get{
							if(m_eqpStateFactory == null)
								m_eqpStateFactory = new SBEqpStateFactory();
							return m_eqpStateFactory;
						}
					}
						ISBEqpStateFactory m_eqpStateFactory;
					public virtual void ClearCurEqpState(){
						SetEqpState(null);
					}
						public virtual bool isEqpStateNull{
							get{return curEqpState == null;}
						}
						public virtual bool wasEqpStateNull{
							get{return prevEqpState == null;}
						}				
					public virtual void Equip(){
						SetEqpState(equippedState);
					}
						public ISBEqpState equippedState{
							get{return eqpStateFactory.MakeEquippedState();}
						}
						public virtual bool isEquipped{
							get{ return curEqpState == equippedState;}
						}
						public virtual bool wasEquipped{
							get{return prevEqpState == equippedState;}
						}
					public virtual void Unequip(){
						SetEqpState(unequippedState);
					}
						public ISBEqpState unequippedState{
							get{return eqpStateFactory.MakeUnequippedState();}
						}
						public virtual bool isUnequipped{
							get{ return curEqpState == unequippedState;}
						}
						public virtual bool wasUnequipped{
							get{ return prevEqpState == unequippedState;}
						}
			/*	Mark state	*/
				ISSEStateEngine<ISBMrkState> mrkStateEngine{
					get{
						if(m_markStateEngine == null)
							m_markStateEngine = new SSEStateEngine<ISBMrkState>(this);
						return m_markStateEngine;
					}
				}
					ISSEStateEngine<ISBMrkState> m_markStateEngine;
				void SetMrkState(ISBMrkState state){
					mrkStateEngine.SetState(state);
					if(state == null && mrkProcess != null)
						SetAndRunMrkProcess(null);
				}
				ISBMrkState curMrkState{
					get{return mrkStateEngine.curState;}
				}
				ISBMrkState prevMrkState{
					get{return mrkStateEngine.prevState;}
				}
				/* Mrk states */
					ISBMrkStateFactory mrkStateFactory{
						get{
							if(m_mrkStateFactory == null)
								m_mrkStateFactory = new SBMrkStateFactory();
							return m_mrkStateFactory;
						}
					}
						ISBMrkStateFactory m_mrkStateFactory;
					public virtual void ClearCurMrkState(){
						SetMrkState(null);
					}
						public virtual bool isMrkStateNull{
							get{return curMrkState == null;}
						}
						public virtual bool wasMrkStateNull{
							get{return prevMrkState == null;}
						}
					public virtual void Mark(){
						SetMrkState(markedState);
					}
						public ISBMrkState markedState{
							get{return mrkStateFactory.MakeMarkedState();}
						}
						public virtual bool isMarked{
							get{return curMrkState == markedState;}
						}
						public virtual bool wasMarked{
							get{return prevMrkState == markedState;}
						}
					public virtual void Unmark(){
						SetMrkState(unmarkedState);
					}
						public ISBMrkState unmarkedState{
							get{return mrkStateFactory.MakeUnmarkedState();}
						}
						public virtual bool isUnmarked{
							get{return curMrkState == unmarkedState;}
						}
						public virtual bool wasUnmarked{
							get{return prevMrkState == markedState;}
						}
		/*	processes	*/
			ISBCoroutineFactory coroutineFactory{
				get{
					if(m_coroutineFactory == null)
						m_coroutineFactory = new SBCoroutineFactory(this);
					return m_coroutineFactory;
				}
			}
				ISBCoroutineFactory m_coroutineFactory;
			public void SetCoroutineFactory(ISBCoroutineFactory factory){
				m_coroutineFactory = factory;
			}
			/*	Action Process	*/
				public virtual ISBActProcess actProcess{
					get{return actProcEngine.process;}
				}
				public virtual void SetAndRunActProcess(ISBActProcess process){
					actProcEngine.SetAndRunProcess(process);
				}
				ISSEProcessEngine<ISBActProcess> actProcEngine{
					get{
						if(m_actProcEngine == null)
							m_actProcEngine = new SSEProcessEngine<ISBActProcess>();
						return m_actProcEngine;
					}
				}
					ISSEProcessEngine<ISBActProcess> m_actProcEngine;
				public virtual void SetActProcessEngine(ISSEProcessEngine<ISBActProcess> engine){
					m_actProcEngine = engine;
				}
				public void ExpireActProcess(){
					if(actProcess != null)
						actProcess.Expire();
				}
				/* Coroutine */
					public System.Func<IEnumeratorFake> waitForPointerUpCoroutine{
						get{return coroutineFactory.MakeWaitForPointerUpCoroutine();}
					}
					public System.Func<IEnumeratorFake> waitForPickUpCoroutine{
						get{return coroutineFactory.MakeWaitForPickUpCoroutine();}
					}
					public System.Func<IEnumeratorFake> pickUpCoroutine{
						get{return coroutineFactory.MakePickUpCoroutine();}
					}
					public System.Func<IEnumeratorFake> waitForNextTouchCoroutine{
						get{return coroutineFactory.MakeWaitForNextTouchCoroutine();}
					}
					public System.Func<IEnumeratorFake> removeCoroutine{
						get{return coroutineFactory.MakeRemoveCoroutine();}
					}
					public System.Func<IEnumeratorFake> addCoroutine{
						get{return coroutineFactory.MakeAddCoroutine();}
					}
					public System.Func<IEnumeratorFake> moveWithinCoroutine{
						get{return coroutineFactory.MakeMoveWithinCoroutine();}
					}
			/*	Equip Process	*/
				public virtual ISBEqpProcess eqpProcess{
					get{return eqpProcEngine.process;}
				}
				public virtual void SetAndRunEqpProcess(ISBEqpProcess process){
					eqpProcEngine.SetAndRunProcess(process);
				}
				ISSEProcessEngine<ISBEqpProcess> eqpProcEngine{
					get{
						if(m_eqpProcEngine == null)
							m_eqpProcEngine = new SSEProcessEngine<ISBEqpProcess>();
						return m_eqpProcEngine;
					}
				}
					ISSEProcessEngine<ISBEqpProcess> m_eqpProcEngine;
				public void SetEqpProcessEngine(ISSEProcessEngine<ISBEqpProcess> engine){
					m_eqpProcEngine = engine;
				}
				/* Coroutine */
					public System.Func<IEnumeratorFake> equipCoroutine{
						get{return coroutineFactory.MakeEquipCoroutine();}
					}
					public System.Func<IEnumeratorFake> unequipCoroutine{
						get{return coroutineFactory.MakeUnequipCoroutine();}
					}
			/*	Mark Process	*/
				public virtual ISBMrkProcess mrkProcess{
					get{return mrkProcEngine.process;}
				}
				public virtual void SetAndRunMrkProcess(ISBMrkProcess process){
					mrkProcEngine.SetAndRunProcess(process);
				}
				ISSEProcessEngine<ISBMrkProcess> mrkProcEngine{
					get{
						if(m_mrkProcEngine == null)
							m_mrkProcEngine = new SSEProcessEngine<ISBMrkProcess>();
						return m_mrkProcEngine;
					}
				}
					ISSEProcessEngine<ISBMrkProcess> m_mrkProcEngine;
				public void SetMrkProcessEngine(ISSEProcessEngine<ISBMrkProcess> engine){
					m_mrkProcEngine = engine;
				}
				/* Coroutine */
					public System.Func<IEnumeratorFake> markCoroutine{
						get{return coroutineFactory.MakeMarkCoroutine();}
					}
					public System.Func<IEnumeratorFake> unmarkCoroutine{
						get{return coroutineFactory.MakeUnmarkCoroutine();}
					}
		/*	commands	*/
			public virtual void Tap(){
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
			public bool isHierarchySetUp{
				get{
					return sg != null;
				}
			}
			public bool delayed{
				get{return m_delayed;}
				set{m_delayed = value;}
			}
				bool m_delayed = true;
			public int pickedAmount{
				get{return m_pickedAmount;}
				set{m_pickedAmount = value;}
			}
				int m_pickedAmount = 0;
			public InventoryItemInstance item{
				get{return m_item;}
			}
				InventoryItemInstance m_item;
			public void SetItem(InventoryItemInstance item){
				m_item = item;
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
			public ISlotGroup sg{
				get{
					if(parent != null)
						return parent as ISlotGroup;
					return null;
				}
			}
			public virtual bool isPickedUp{
				get{
					return tam.pickedSB == (ISlottable)this;
				}
			}
			public virtual bool isStackable{
				get{return item.Item.IsStackable;}
			}
			public bool passesPrePickFilter{
				get{return !tam.IsTransactionGoingToBeRevert(this);}
			}
			public int quantity{
				get{return item.quantity;}
			}
			public void SetQuantity(int quant){
				item.quantity = quant;
			}
		/*	SlotSystemElement imple	*/
			/*	fields	*/
				public override ISlotSystemElement this[int i]{
					get{return null;}
				}
				protected override IEnumerable<ISlotSystemElement> elements{
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
				return this.item.CompareTo(other.item);
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
			public override void SetHierarchy(){
			}
			public virtual void Increment(){
				SetActState(pickedUpState);
				if(isStackable && quantity > m_pickedAmount){
					m_pickedAmount ++;
				}
			}
			public virtual void ExecuteTransaction(){
				tam.ExecuteTransaction();
			}
			public void UpdateEquipState(){
				if(item.isEquipped) Equip();
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
				flag &= this.item == other.item;
				return flag;
			}
			public bool HaveCommonItemFamily(ISlottable other){
				if(this.item is BowInstance)
					return (other.item is BowInstance);
				else if(this.item is WearInstance)
					return (other.item is WearInstance);
				else if(this.item is CarriedGearInstance)
					return (other.item is CarriedGearInstance);
				else if(this.item is PartsInstance)
					return (other.item is PartsInstance);
				else 
					return false;
			}
			public void Destroy(){
				GameObject go = gameObject;
				DestroyImmediate(go);
			}
		/*	Forward	*/
			public virtual void SetPickedSB(){
				if(tam != null){
					tam.SetPickedSB((ISlottable)this);
				}else
					throw new System.InvalidOperationException("Slottable.SetPickedSB: ssm not set");
			}
			public virtual void Probe(){
				tam.Probe();
			}
			public virtual void SetDIcon1(){
				DraggedIcon dIcon = new DraggedIcon(this);
				tam.SetDIcon1(dIcon);
			}
			public virtual void SetDIcon2(){
				DraggedIcon dIcon = new DraggedIcon(this);
				tam.SetDIcon2(dIcon);
			}
			public virtual void CreateTAResult(){
				tam.CreateTransactionResults();
			}
			public virtual void UpdateTA(){
				tam.UpdateFields();
			}
			public virtual bool isPool{
				get{
					if(sg != null){
						return sg.isPool;
					}else
						throw new System.InvalidOperationException("Slottable.isPool: sg is not set");
				}
			}
			public ITransactionManager tam{
				get{
					if(m_tam != null)
						return m_tam;
					else
						throw new InvalidOperationException("tam not set");
				}
			}ITransactionManager m_tam;
			public void SetTAM(ITransactionManager tam){m_tam = tam;}
		/* Hoverable */
			public IHoverable hoverable{
				get{
					if(m_hoverable == null)
						m_hoverable = new Hoverable(this, tam);
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
			public void SetHovered(){
				hoverable.SetHovered();
			}
			public void UnsetHovered(){
				hoverable.UnsetHovered();
			}
	}
	public interface ISlottable: ISlotSystemElement ,IComparable<ISlottable>, IComparable{
		/*	States and Processes	*/
			/* States */
				/* ActStates */
					void ClearCurActState();
						bool wasActStateNull{get;}
						bool isActStateNull{get;}
					void WaitForAction();
						ISBActState waitForActionState{get;}
						bool isWaitingForAction{get;}
						bool wasWaitingForAction{get;}
					void WaitForPointerUp();
						ISBActState waitForPointerUpState{get;}
						bool isWaitingForPointerUp{get;}
						bool wasWaitingForPointerUp{get;}
					void WaitForPickUp();
						ISBActState waitForPickUpState{get;}
						bool isWaitingForPickUp{get;}
						bool wasWaitingForPickUp{get;}
					void WaitForNextTouch();
						ISBActState waitForNextTouchState{get;}
						bool isWaitingForNextTouch{get;}
						bool wasWaitingForNextTouch{get;}
					void PickUp();
						ISBActState pickedUpState{get;}
						bool isPickingUp{get;}
						bool wasPickingUp{get;}
					void Remove();
						ISBActState removedState{get;}
						bool isRemoving{get;}
						bool wasRemoving{get;}
					void Add();
						ISBActState addedState{get;}
						bool isAdding{get;}
						bool wasAdding{get;}
					void MoveWithin();
						ISBActState moveWithinState{get;}
						bool isMovingWithin{get;}
						bool wasMovingWithin{get;}
				/* Eqp States */
					void ClearCurEqpState();
						bool isEqpStateNull{get;}
						bool wasEqpStateNull{get;}
					void Equip();
						ISBEqpState equippedState{get;}
						bool isEquipped{get;}
						bool wasEquipped{get;}
					void Unequip();
						ISBEqpState unequippedState{get;}
						bool isUnequipped{get;}
						bool wasUnequipped{get;}
				/* Mrk States */
					void ClearCurMrkState();
						bool isMrkStateNull{get;}
						bool wasMrkStateNull{get;}
					void Mark();
						ISBMrkState	markedState{get;}
						bool isMarked{get;}
						bool wasMarked{get;}
					void Unmark();
						ISBMrkState unmarkedState{get;}
						bool isUnmarked{get;}
						bool wasUnmarked{get;}
			/* Process */
				ISBActProcess actProcess{get;}
				void SetAndRunActProcess(ISBActProcess process);
					System.Func<IEnumeratorFake> waitForPointerUpCoroutine{get;}
					System.Func<IEnumeratorFake> waitForPickUpCoroutine{get;}
					System.Func<IEnumeratorFake> pickUpCoroutine{get;}
					System.Func<IEnumeratorFake> waitForNextTouchCoroutine{get;}
					System.Func<IEnumeratorFake> removeCoroutine{get;}
					System.Func<IEnumeratorFake> addCoroutine{get;}
					System.Func<IEnumeratorFake> moveWithinCoroutine{get;}
				ISBEqpProcess eqpProcess{get;}
				void SetAndRunEqpProcess(ISBEqpProcess process);
					System.Func<IEnumeratorFake> unequipCoroutine{get;}
					System.Func<IEnumeratorFake> equipCoroutine{get;}
				ISBMrkProcess mrkProcess{get;}
				void SetAndRunMrkProcess(ISBMrkProcess process);
					System.Func<IEnumeratorFake> unmarkCoroutine{get;}
					System.Func<IEnumeratorFake> markCoroutine{get;}
		/*	Commands	*/
			void Tap();
		/*	fields	*/
			bool isHierarchySetUp{get;}
			bool delayed{get;set;}
			int pickedAmount{get;set;}
			void SetItem(InventoryItemInstance item);
			InventoryItemInstance item{get;}
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
			bool isPool{get;}
			void Probe();
			ITransactionManager tam{get;}
			void SetTAM(ITransactionManager tam);
		/* Hoverable */
			IHoverable hoverable{get;}
			bool isHovered{get;}
			void OnHoverEnter();
			void OnHoverExit();
			void SetHovered();
			void UnsetHovered();
	}
}
