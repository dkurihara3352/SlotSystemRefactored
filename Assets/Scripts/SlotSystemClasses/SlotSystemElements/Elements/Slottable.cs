using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace SlotSystem{
	public class Slottable : AbsSlotSystemElement, ISlottable{
		/*	States	*/
			/*	Selection State	*/
				public override SSEState curSelState{
					get{return (SBSelState)selStateEngine.curState;}
				}
				public override SSEState prevSelState{
					get{return (SBSelState)selStateEngine.prevState;}
				}
				public override void SetSelState(SSEState state){
					if(state == null || state is SBSelState)
						selStateEngine.SetState(state);
					else
						throw new System.ArgumentException("Slottable.SetSelState: something other than SBSelState is beint attempted to be assigned");
				}
				public static SBSelState sbDeactivatedState{
					get{
						if(Slottable.m_sbDeactivatedState != null)
							return Slottable.m_sbDeactivatedState;
						else{
							Slottable.m_sbDeactivatedState = new SBDeactivatedState();	
							return Slottable.m_sbDeactivatedState;
						}
					}
					}static SBSelState m_sbDeactivatedState;
				public static SBSelState sbSelectedState{
					get{
						if(Slottable.m_sbSelectedState != null)
							return Slottable.m_sbSelectedState;
						else
							Slottable.m_sbSelectedState = new SBSelectedState();
							return Slottable.m_sbSelectedState;
					}
					}static SBSelState m_sbSelectedState;			
				public static SBSelState sbDefocusedState{
					get{
						if(Slottable.m_sbDefocusedState != null)
							return Slottable.m_sbDefocusedState;
						else{
							Slottable.m_sbDefocusedState = new SBDefocusedState();
							return Slottable.m_sbDefocusedState;
						}
					}
					}static SBSelState m_sbDefocusedState;
				public static SBSelState sbFocusedState{
						get{
							if(Slottable.m_sbFocusedState != null)
								return Slottable.m_sbFocusedState;
							else{
								Slottable.m_sbFocusedState = new SBFocusedState();
								return Slottable.m_sbFocusedState;
							}
						}
						}static SBSelState m_sbFocusedState;
			/*	Action State	*/
				public override SSEState curActState{
					get{return (SBActState)actStateEngine.curState;}
				}
				public override SSEState prevActState{
					get{return (SBActState)actStateEngine.prevState;}
				}
				public override void SetActState(SSEState state){
					if(state == null || state is SBActState)
						actStateEngine.SetState(state);
					else
						throw new System.ArgumentException("Slottable.SetActState: something other than SBActionState is being attempted to be assigned");
				}
				public static SBActState sbWaitForActionState{
					get{
						if(m_sbWaitForActionState != null)
							return m_sbWaitForActionState;
						else{
							m_sbWaitForActionState = new WaitForActionState();
							return m_sbWaitForActionState;
						}
					}
					}static SBActState m_sbWaitForActionState;
				public static SBActState waitForPointerUpState{
					get{
						if(m_waitForPointerUpState != null)
							return m_waitForPointerUpState;
						else{
							m_waitForPointerUpState = new WaitForPointerUpState();
							return m_waitForPointerUpState;
						}
					}
					}static SBActState m_waitForPointerUpState;
				public static SBActState waitForPickUpState{
					get{
						if(Slottable.m_waitForPickUpState != null)
							return Slottable.m_waitForPickUpState;
						else{
							Slottable.m_waitForPickUpState = new WaitForPickUpState();
							return Slottable.m_waitForPickUpState;
						}
					}
					}static SBActState m_waitForPickUpState;
				public static SBActState waitForNextTouchState{
					get{
						if(Slottable.m_waitForNextTouchState != null)
							return Slottable.m_waitForNextTouchState;
						else{
							Slottable.m_waitForNextTouchState = new WaitForNextTouchState();
							return Slottable.m_waitForNextTouchState;
						}
					}
					}static SBActState m_waitForNextTouchState;
				public static SBActState pickedUpState{
					get{
						if(Slottable.m_pickedUpState != null)
							return Slottable.m_pickedUpState;
						else{
							Slottable.m_pickedUpState = new PickedUpState();
							return Slottable.m_pickedUpState;
						}
					}
					}static SBActState m_pickedUpState;
				public static SBActState removedState{
					get{
						if(Slottable.m_removedState == null)
							Slottable.m_removedState = new SBRemovedState();
						return Slottable.m_removedState;			
					}
					}static SBActState m_removedState;
				public static SBActState addedState{
					get{
						if(Slottable.m_addedState == null)
							Slottable.m_addedState = new SBAddedState();
						return Slottable.m_addedState;			
					}
					}static SBActState m_addedState;
				public static SBActState moveWithinState{
						get{
							if(Slottable.m_moveWithinState == null)
								Slottable.m_moveWithinState = new SBMoveWithinState();
							return Slottable.m_moveWithinState;			
						}
						}static SBActState m_moveWithinState;
			/*	Equip State	*/
				public SSEStateEngine eqpStateEngine{
					get{
						if(m_eqpStateEngine == null)
							m_eqpStateEngine = new SSEStateEngine(this);
						return m_eqpStateEngine;
					}
					}SSEStateEngine m_eqpStateEngine;
					public void SetEqpStateEngine(SSEStateEngine engine){m_eqpStateEngine = engine;}
					public virtual SSEState curEqpState{
						get{return (SBEqpState)eqpStateEngine.curState;}
					}
					public virtual SSEState prevEqpState{
						get{return (SBEqpState)eqpStateEngine.prevState;}
					}
					public virtual void SetEqpState(SSEState state){
						if(state == null || state is SBEqpState)
							eqpStateEngine.SetState(state);
						else
							throw new System.ArgumentException("Slottable.SetEqpState: something other than SBEqpState is trying to be assinged");
					}
				public static SBEqpState equippedState{
					get{
						if(m_equippedState != null)
							return m_equippedState;
						else{
							m_equippedState = new SBEquippedState();
							return m_equippedState;
						}
					}
					}static SBEqpState m_equippedState;
				public static SBEqpState unequippedState{
					get{
						if(m_unequippedState != null)
							return m_unequippedState;
						else{
							m_unequippedState = new SBUnequippedState();
							return m_unequippedState;
						}
					}
					}static SBEqpState m_unequippedState;
			/*	Mark state	*/
				public SSEStateEngine mrkStateEngine{
					get{
						if(m_markStateEngine == null)
							m_markStateEngine = new SSEStateEngine(this);
						return m_markStateEngine;
					}
					}SSEStateEngine m_markStateEngine;
					public void SetMrkStateEngine(SSEStateEngine engine){m_markStateEngine = engine;}
					public virtual SSEState curMrkState{
						get{return (SBMrkState)mrkStateEngine.curState;}
					}
					public virtual SSEState prevMrkState{
						get{return (SBMrkState)mrkStateEngine.prevState;}
					}
					public virtual void SetMrkState(SSEState state){
						if(state == null || state is SBMrkState)
							mrkStateEngine.SetState(state);
						else
							throw new System.ArgumentException("Slottable.SetMrkState: something other than SBMrkState is trying to be assinged");
					}
				public static SBMrkState markedState{
					get{
						if(m_markedState != null)
							return m_markedState;
						else{
							m_markedState = new SBMarkedState();
							return m_markedState;
						}
					}
					}static SBMrkState m_markedState;
				public static SBMrkState unmarkedState{
					get{
						if(m_unmarkedState != null)
							return m_unmarkedState;
						else{
							m_unmarkedState = new SBUnmarkedState();
							return m_unmarkedState;
						}
					}
					}static SBMrkState m_unmarkedState;
		/*	processes	*/
			/*	Selection Process	*/
				public override ISSEProcess selProcess{
					get{return (ISBSelProcess)selProcEngine.process;}
				}
				public override void SetAndRunSelProcess(ISSEProcess process){
					if(process ==null || process is ISBSelProcess)
						selProcEngine.SetAndRunProcess(process);
					else throw new System.ArgumentException("Slottable.SetAndRunSelProcess: argument is not of type ISBSelProcess");
				}
				public override IEnumeratorFake greyoutCoroutine(){return null;}
				public override IEnumeratorFake greyinCoroutine(){return null;}
				public override IEnumeratorFake highlightCoroutine(){return null;}
				public override IEnumeratorFake dehighlightCoroutine(){return null;}
			/*	Action Process	*/
				public override ISSEProcess actProcess{
					get{return (ISBActProcess)actProcEngine.process;}
				}
				public override void SetAndRunActProcess(ISSEProcess process){
					if(process == null || process is ISBActProcess)
						actProcEngine.SetAndRunProcess(process);
					else throw new System.ArgumentException("Slottable.SetAndRunActProcess: argument is not of type ISBActProcess");
				}
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
				public SSEProcessEngine eqpProcEngine{
					get{
						if(m_eqpProcEngine == null)
							m_eqpProcEngine = new SSEProcessEngine();
						return m_eqpProcEngine;
					}
					}SSEProcessEngine m_eqpProcEngine;
					public void SetEqpProcessEngine(SSEProcessEngine engine){m_eqpProcEngine = engine;}
					public virtual ISSEProcess eqpProcess{
						get{return (ISBEqpProcess)eqpProcEngine.process;}
					}
					public virtual void SetAndRunEqpProcess(ISSEProcess process){
						if(process == null || process is ISBEqpProcess)
							eqpProcEngine.SetAndRunProcess(process);
						else throw new System.ArgumentException("Slottable.SetAndRunEquipProcess: argument is not of type ISBEqpProcess");
					}
				public IEnumeratorFake UnequipCoroutine(){return null;}
				public IEnumeratorFake EquipCoroutine(){return null;}
			/*	Mark Process	*/
				public SSEProcessEngine mrkProcEngine{
					get{
						if(m_mrkProcEngine == null)
							m_mrkProcEngine = new SSEProcessEngine();
						return m_mrkProcEngine;
					}
					}SSEProcessEngine m_mrkProcEngine;
					public void SetMrkProcessEngine(SSEProcessEngine engine){m_mrkProcEngine = engine;}
					public virtual ISSEProcess mrkProcess{
						get{return (ISBMrkProcess)mrkProcEngine.process;}
					}
					public virtual void SetAndRunMrkProcess(ISSEProcess process){
						if(process == null || process is ISBMrkProcess)
							mrkProcEngine.SetAndRunProcess(process);
						else throw new System.ArgumentException("Slottable.SetAndRunEquipProcess: argument is not of type ISBMrkProcess");
					}
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
			public ISlotGroup sg{
				get{
					return (ISlotGroup)parent;
				}
			}
			public override bool isFocused{
				get{return curSelState == Slottable.sbFocusedState;}
			}
			public override bool isDefocused{
				get{return curSelState == Slottable.sbDefocusedState;}
			}
			public override bool isDeactivated{
				get{return curSelState == Slottable.sbDeactivatedState;}
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
					SetSelState(Slottable.sbDeactivatedState);
				}
				public override void Focus(){
					SetSelState(Slottable.sbFocusedState);
				}
				public override void Defocus(){
					SetSelState(Slottable.sbDefocusedState);
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
					((SBSelState)curSelState).OnHoverEnterMock(this, eventData);
				}
				public virtual void OnHoverExitMock(){
					PointerEventDataFake eventData = new PointerEventDataFake();
					((SBSelState)curSelState).OnHoverExitMock(this, eventData);
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
			public void Initialize(InventoryItemInstance item){
				delayed = true;
				SetItem(item);
				SetSelState(Slottable.sbDeactivatedState);
				SetActState(Slottable.sbWaitForActionState);
				SetEqpState(null);
				SetMrkState(Slottable.unmarkedState);
			}
			public virtual void PickUp(){
				SetActState(Slottable.pickedUpState);
				m_pickedAmount = 1;
			}
			public virtual void Increment(){
				if(m_item.IsStackable && m_item.quantity > m_pickedAmount){
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
			public virtual void SetSSMActState(SSMActState ssmState){
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
	public interface ISlottable: IAbsSlotSystemElement ,IComparable<ISlottable>, IComparable{
		/*	States and Processes	*/
			SSEStateEngine eqpStateEngine{get;}
			void SetEqpStateEngine(SSEStateEngine engine);
			SSEState curEqpState{get;}
			SSEState prevEqpState{get;}
			void SetEqpState(SSEState state);
			SSEStateEngine mrkStateEngine{get;}
			void SetMrkStateEngine(SSEStateEngine engine);
			SSEState curMrkState{get;}
			SSEState prevMrkState{get;}
			void SetMrkState(SSEState state);
			IEnumeratorFake WaitForPointerUpCoroutine();
			IEnumeratorFake WaitForPickUpCoroutine();
			IEnumeratorFake PickUpCoroutine();
			IEnumeratorFake WaitForNextTouchCoroutine();
			IEnumeratorFake RemoveCoroutine();
			IEnumeratorFake AddCorouine();
			IEnumeratorFake MoveWithinCoroutine();
			SSEProcessEngine eqpProcEngine{get;}
			void SetEqpProcessEngine(SSEProcessEngine engine);
			ISSEProcess eqpProcess{get;}
			void SetAndRunEqpProcess(ISSEProcess process);
			IEnumeratorFake UnequipCoroutine();
			IEnumeratorFake EquipCoroutine();
			SSEProcessEngine mrkProcEngine{get;}
			void SetMrkProcessEngine(SSEProcessEngine engine);
			ISSEProcess mrkProcess{get;}
			void SetAndRunMrkProcess(ISSEProcess process);
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
			void Initialize(InventoryItemInstance item);
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
			void SetSSMActState(SSMActState ssmState);
			void SetDIcon1();
			void SetDIcon2();
			void CreateTAResult();
			void UpdateTA();
			bool isHovered{get;}
			bool isPool{get;}
			void SetHovered();
	}
}
