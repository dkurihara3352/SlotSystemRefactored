using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace SlotSystem{
	public class Slottable : SlotSystemElement, ISlottable{
		/*	States	*/
			/*	Action State	*/
				public ISSEStateEngine<ISBActState> actStateEngine{
					get{
						if(m_actStateEngine == null)
							m_actStateEngine = new SSEStateEngine<ISBActState>(this);
						return m_actStateEngine;
					}
					}ISSEStateEngine<ISBActState> m_actStateEngine;
				public void SetActStateEngine(ISSEStateEngine<ISBActState> engine){m_actStateEngine = engine;}
				public virtual ISBActState curActState{
					get{return actStateEngine.curState;}
				}
				public virtual ISBActState prevActState{
					get{return actStateEngine.prevState;}
				}
				public virtual void SetActState(ISBActState state){
					actStateEngine.SetState(state);
				}
				/* static states */
					public static ISBActState sbWaitForActionState{
						get{
							if(m_sbWaitForActionState == null)
								m_sbWaitForActionState = new WaitForActionState();
							return m_sbWaitForActionState;
						}
						}static ISBActState m_sbWaitForActionState;
					public static ISBActState waitForPointerUpState{
						get{
							if(m_waitForPointerUpState == null)
								m_waitForPointerUpState = new WaitForPointerUpState();
							return m_waitForPointerUpState;
						}
						}static ISBActState m_waitForPointerUpState;
					public static ISBActState waitForPickUpState{
						get{
							if(m_waitForPickUpState == null)
								m_waitForPickUpState = new WaitForPickUpState();
							return m_waitForPickUpState;
						}
						}static ISBActState m_waitForPickUpState;
					public static ISBActState waitForNextTouchState{
						get{
							if(m_waitForNextTouchState == null)
								m_waitForNextTouchState = new WaitForNextTouchState();
							return m_waitForNextTouchState;
						}
						}static ISBActState m_waitForNextTouchState;
					public static ISBActState pickedUpState{
						get{
							if(m_pickedUpState == null)
								m_pickedUpState = new PickedUpState();
							return m_pickedUpState;
						}
						}static ISBActState m_pickedUpState;
					public static ISBActState removedState{
						get{
							if(m_removedState == null)
								m_removedState = new SBRemovedState();
							return m_removedState;			
						}
						}static ISBActState m_removedState;
					public static ISBActState addedState{
						get{
							if(m_addedState == null)
								m_addedState = new SBAddedState();
							return m_addedState;			
						}
						}static ISBActState m_addedState;
					public static ISBActState moveWithinState{
							get{
								if(m_moveWithinState == null)
									m_moveWithinState = new SBMoveWithinState();
								return m_moveWithinState;			
							}
							}static ISBActState m_moveWithinState;
			/*	Equip State	*/
				public ISSEStateEngine<ISBEqpState> eqpStateEngine{
					get{
						if(m_eqpStateEngine == null)
							m_eqpStateEngine = new SSEStateEngine<ISBEqpState>(this);
						return m_eqpStateEngine;
					}
					}ISSEStateEngine<ISBEqpState> m_eqpStateEngine;
				public void SetEqpStateEngine(ISSEStateEngine<ISBEqpState> engine){m_eqpStateEngine = engine;}
				public virtual ISBEqpState curEqpState{
					get{return eqpStateEngine.curState;}
				}
				public virtual ISBEqpState prevEqpState{
					get{return eqpStateEngine.prevState;}
				}
				public virtual void SetEqpState(ISBEqpState state){
					eqpStateEngine.SetState(state);
				}
				/* Static states */
					public static ISBEqpState equippedState{
						get{
							if(m_equippedState == null)
								m_equippedState = new SBEquippedState();
							return m_equippedState;
							}
						}static ISBEqpState m_equippedState;
					public static ISBEqpState unequippedState{
					get{
						if(m_unequippedState == null)
							m_unequippedState = new SBUnequippedState();
						return m_unequippedState;
					}
					}static ISBEqpState m_unequippedState;
			/*	Mark state	*/
				public ISSEStateEngine<ISBMrkState> mrkStateEngine{
					get{
						if(m_markStateEngine == null)
							m_markStateEngine = new SSEStateEngine<ISBMrkState>(this);
						return m_markStateEngine;
					}
					}ISSEStateEngine<ISBMrkState> m_markStateEngine;
				public void SetMrkStateEngine(ISSEStateEngine<ISBMrkState> engine){m_markStateEngine = engine;}
				public virtual ISBMrkState curMrkState{
					get{return mrkStateEngine.curState;}
				}
				public virtual ISBMrkState prevMrkState{
					get{return mrkStateEngine.prevState;}
				}
				public virtual void SetMrkState(ISBMrkState state){
					mrkStateEngine.SetState(state);
				}
				/* Static states */
					public static ISBMrkState markedState{
						get{
							if(m_markedState == null)
								m_markedState = new SBMarkedState();
							return m_markedState;
						}
						}static ISBMrkState m_markedState;
					public static ISBMrkState unmarkedState{
						get{
							if(m_unmarkedState == null)
								m_unmarkedState = new SBUnmarkedState();
							return m_unmarkedState;
						}
						}static ISBMrkState m_unmarkedState;
		/*	processes	*/
			/*	Selection Process	*/
				/* Coroutines */
				public override IEnumeratorFake deactivateCoroutine(){return null;}
				public override IEnumeratorFake focusCoroutine(){return null;}
				public override IEnumeratorFake defocusCoroutine(){return null;}
				public override IEnumeratorFake selectCoroutine(){return null;}
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
				/* Coroutine */
					public IEnumeratorFake WaitForPointerUpCoroutine(){return null;}
					public IEnumeratorFake WaitForPickUpCoroutine(){return null;}
					public IEnumeratorFake PickUpCoroutine(){return null;}
					public IEnumeratorFake WaitForNextTouchCoroutine(){return null;}
					public IEnumeratorFake RemoveCoroutine(){return null;}
					public IEnumeratorFake AddCorouine(){return null;}
					public IEnumeratorFake MoveWithinCoroutine(){
						if(slotID == newSlotID)
							ExpireActionProcess();
						return null;
					}
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
				/* Coroutine */
					public IEnumeratorFake UnequipCoroutine(){return null;}
					public IEnumeratorFake EquipCoroutine(){return null;}
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
				/* Coroutine */
					public IEnumeratorFake unmarkCoroutine(){return null;}
					public IEnumeratorFake markCoroutine(){return null;}
		/*	commands	*/
			static public SlottableCommand TapCommand{
				get{return m_tapCommand;}
				}static SlottableCommand m_tapCommand = new SBTapCommand();
				public virtual void Tap(){
					m_tapCommand.Execute(this);
				}
		/*	public fields	*/
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
			public bool isAdded{get{return slotID == -1;}}
			public bool isRemoved{get{return newSlotID == -1;}}
			public ISlotGroup sg{
				get{
					return (ISlotGroup)parent;
				}
			}
			public override bool isFocused{
				get{return curSelState == SlotSystemElement.focusedState;}
			}
			public override bool isDefocused{
				get{return curSelState == SlotSystemElement.defocusedState;}
			}
			public override bool isDeactivated{
				get{return curSelState == SlotSystemElement.deactivatedState;}
			}
			public virtual bool isPickedUp{
				get{
					return ssm.pickedSB == (ISlottable)this;
				}
			}
			public bool isEquipped{
				get{ return itemInst.isEquipped;}
				}public void Equip(){
					SetEqpState(Slottable.equippedState);
				}
				public void Unequip(){
					SetEqpState(Slottable.unequippedState);
				}
			public bool isMarked{
				get{return itemInst.isMarked;}
				}public void Mark(){
					SetMrkState(Slottable.markedState);
				}
				public void Unmark(){
					SetMrkState(Slottable.unmarkedState);
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
						return ssm.FindParent(this);
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
				public override void Activate(){}
				public override void Deactivate(){
					SetSelState(SlotSystemElement.deactivatedState);
				}
				public override void Focus(){
					SetSelState(SlotSystemElement.focusedState);
				}
				public override void Defocus(){
					SetSelState(SlotSystemElement.defocusedState);
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
			/*	Selection event	*/
				public virtual void OnHoverEnterMock(){
					PointerEventDataFake eventData = new PointerEventDataFake();
					curSelState.OnHoverEnterMock(this, eventData);
				}
				public virtual void OnHoverExitMock(){
					PointerEventDataFake eventData = new PointerEventDataFake();
					curSelState.OnHoverExitMock(this, eventData);
				}
			/*	Action Event	*/
				public void OnPointerDownMock(PointerEventDataFake eventDataMock){
					((SBActState)curActState).OnPointerDownMock(this, eventDataMock);
				}
				public void OnPointerUpMock(PointerEventDataFake eventDataMock){
					((SBActState)curActState).OnPointerUpMock(this, eventDataMock);
				}
				public void OnDeselectedMock(PointerEventDataFake eventDataMock){
					((SBActState)curActState).OnDeselectedMock(this, eventDataMock);
				}
				public void OnEndDragMock(PointerEventDataFake eventDataMock){
					((SBActState)curActState).OnEndDragMock(this, eventDataMock);
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
				SetSelState(SlotSystemElement.deactivatedState);
				SetActState(Slottable.sbWaitForActionState);
				SetEqpState(null);
				SetMrkState(Slottable.unmarkedState);
			}
			public override void SetElements(){}
			public virtual void PickUp(){
				SetActState(Slottable.pickedUpState);
				m_pickedAmount = 1;
			}
			public virtual void Increment(){
				if(m_item.IsStackable && quantity > m_pickedAmount){
					m_pickedAmount ++;
				}
			}
			public virtual void ExecuteTransaction(){
				ssm.ExecuteTransaction();
			}
			public void ExpireActionProcess(){
				if(actProcess.isRunning)
					actProcess.Expire();
			}
			public void UpdateEquipState(){
				if(itemInst.isEquipped) Equip();
				else Unequip();
			}
			public virtual void Reset(){
				SetActState(Slottable.sbWaitForActionState);
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
				ssm.SetPickedSB((ISlottable)this);
			}
			public virtual void SetSSMActState(ISSMActState ssmState){
				ssm.SetActState(ssmState);
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
			public virtual bool isPool{get{return sg.isPool;}}
			public void SetHovered(){
				ssm.SetHovered((ISlottable)this);
			}
	}
	public interface ISlottable: ISlotSystemElement ,IComparable<ISlottable>, IComparable{
		/*	States and Processes	*/
			/* States */
				ISSEStateEngine<ISBActState> actStateEngine{get;}
					void SetActStateEngine(ISSEStateEngine<ISBActState> engine);
					ISBActState curActState{get;}
					ISBActState prevActState{get;}
					void SetActState(ISBActState state);
				ISSEStateEngine<ISBEqpState> eqpStateEngine{get;}
					void SetEqpStateEngine(ISSEStateEngine<ISBEqpState> engine);
					ISBEqpState curEqpState{get;}
					ISBEqpState prevEqpState{get;}
					void SetEqpState(ISBEqpState state);
				ISSEStateEngine<ISBMrkState> mrkStateEngine{get;}
					void SetMrkStateEngine(ISSEStateEngine<ISBMrkState> engine);
					ISBMrkState curMrkState{get;}
					ISBMrkState prevMrkState{get;}
					void SetMrkState(ISBMrkState state);
			/* Process */
				ISSEProcessEngine<ISBActProcess> actProcEngine{get;}
					void SetActProcessEngine(ISSEProcessEngine<ISBActProcess> engine);
					ISBActProcess actProcess{get;}
					void SetAndRunActProcess(ISBActProcess process);
						IEnumeratorFake WaitForPointerUpCoroutine();
						IEnumeratorFake WaitForPickUpCoroutine();
						IEnumeratorFake PickUpCoroutine();
						IEnumeratorFake WaitForNextTouchCoroutine();
						IEnumeratorFake RemoveCoroutine();
						IEnumeratorFake AddCorouine();
						IEnumeratorFake MoveWithinCoroutine();
				ISSEProcessEngine<ISBEqpProcess> eqpProcEngine{get;}
					void SetEqpProcessEngine(ISSEProcessEngine<ISBEqpProcess> engine);
					ISBEqpProcess eqpProcess{get;}
					void SetAndRunEqpProcess(ISBEqpProcess process);
						IEnumeratorFake UnequipCoroutine();
						IEnumeratorFake EquipCoroutine();
				ISSEProcessEngine<ISBMrkProcess> mrkProcEngine{get;}
					void SetMrkProcessEngine(ISSEProcessEngine<ISBMrkProcess> engine);
					ISBMrkProcess mrkProcess{get;}
					void SetAndRunMrkProcess(ISBMrkProcess process);
						IEnumeratorFake unmarkCoroutine();
						IEnumeratorFake markCoroutine();
		/*	Commands	*/
			void Tap();
		/*	fields	*/
			bool delayed{get;set;}
			int pickedAmount{get;set;}
			void SetItem(SlottableItem item);
			InventoryItemInstance itemInst{get;}
			int slotID{get;}
			void SetSlotID(int i);
			int newSlotID{get;}
			void SetNewSlotID(int id);
			bool isAdded{get;}
			bool isRemoved{get;}
			ISlotGroup sg{get;}
			bool isPickedUp{get;}
			bool isEquipped{get;}
			void Equip();
			void Unequip();
			bool isMarked{get;}
			void Mark();
			void Unmark();
			bool isStackable{get;}
			bool passesPrePickFilter{get;}
			int quantity{get;}
			void SetQuantity(int quant);
		/*	Event Methods	*/
			void OnHoverEnterMock();
			void OnHoverExitMock();
			void OnPointerDownMock(PointerEventDataFake eventDataMock);
			void OnPointerUpMock(PointerEventDataFake eventDataMock);
			void OnDeselectedMock(PointerEventDataFake eventDataMock);
			void OnEndDragMock(PointerEventDataFake eventDataMock);


		/*	Methods	*/
			void PickUp();
			void Increment();
			void ExecuteTransaction();
			void ExpireActionProcess();
			void UpdateEquipState();
			void Reset();
			bool ShareSGAndItem(ISlottable other);
			bool HaveCommonItemFamily(ISlottable other);
			void Destroy();
		/*	Forward	*/
			void SetPickedSB();
			void SetSSMActState(ISSMActState ssmState);
			void SetDIcon1();
			void SetDIcon2();
			void CreateTAResult();
			void UpdateTA();
			bool isHovered{get;}
			bool isPool{get;}
			void SetHovered();
	}
}
