﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;
namespace SlotSystem{
	public class Slottable : AbsSlotSystemElement, IComparable<Slottable>, IComparable{
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
						throw new System.InvalidOperationException("Slottable.SetSelState: something other than SBSelState is beint attempted to be assigned");
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
						throw new System.InvalidOperationException("Slottable.SetActState: something other than SBActionState is being attempted to be assigned");
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
				SSEStateEngine EqpStateEngine{
					get{
						if(m_eqpStateEngine == null)
							m_eqpStateEngine = new SSEStateEngine(this);
						return m_eqpStateEngine;
					}
					}SSEStateEngine m_eqpStateEngine;
					public SSEState CurEqpState{
						get{return (SBEqpState)EqpStateEngine.curState;}
					}
					public SSEState PrevEqpState{
						get{return (SBEqpState)EqpStateEngine.prevState;}
					}
					public void SetEqpState(SSEState state){
						if(state == null || state is SBEqpState)
							EqpStateEngine.SetState(state);
						else
							throw new System.InvalidOperationException("Slottable.SetEqpState: something other than SBEqpState is trying to be assinged");
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
		/*	processes	*/
			/*	Selection Process	*/
				public override SSEProcess selProcess{
					get{return (SBSelProcess)selProcEngine.process;}
				}
				public override void SetAndRunSelProcess(SSEProcess process){
					if(process ==null || process is SBSelProcess)
						selProcEngine.SetAndRunProcess(process);
					else throw new System.InvalidOperationException("Slottable.SetAndRunSelProcess: argument is not of type SBSelProcess");
				}
				public override IEnumeratorMock greyoutCoroutine(){return null;}
				public override IEnumeratorMock greyinCoroutine(){return null;}
				public override IEnumeratorMock highlightCoroutine(){return null;}
				public override IEnumeratorMock dehighlightCoroutine(){return null;}
			/*	Action Process	*/
				public override SSEProcess actProcess{
					get{return (SBActProcess)actProcEngine.process;}
				}
				public override void SetAndRunActProcess(SSEProcess process){
					if(process == null || process is SBActProcess)
						actProcEngine.SetAndRunProcess(process);
					else throw new System.InvalidOperationException("Slottable.SetAndRunActProcess: argument is not of type SBActProcess");
				}
				public IEnumeratorMock WaitForPointerUpCoroutine(){return null;}
				public IEnumeratorMock WaitForPickUpCoroutine(){return null;}
				public IEnumeratorMock PickUpCoroutine(){return null;}
				public IEnumeratorMock WaitForNextTouchCoroutine(){return null;}
				public IEnumeratorMock RemoveCoroutine(){return null;}
				public IEnumeratorMock AddCorouine(){return null;}
				public IEnumeratorMock MoveWithinCoroutine(){
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
					public SBEqpProcess eqpProcess{
						get{return (SBEqpProcess)eqpProcEngine.process;}
					}
					public void SetAndRunEquipProcess(SBEqpProcess process){
						eqpProcEngine.SetAndRunProcess(process);
					}
				public IEnumeratorMock UnequipCoroutine(){return null;}
				public IEnumeratorMock EquipCoroutine(){return null;}
		/*	commands	*/
			static public SlottableCommand TapCommand{
				get{return m_tapCommand;}
				}static SlottableCommand m_tapCommand = new SBTapCommand();
				public void Tap(){
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
			public SlottableItem item{
				get{return m_item;}
				}SlottableItem m_item;
				public void SetItem(SlottableItem item){
					m_item = item;
				}
			public InventoryItemInstanceMock itemInst{
					get{return (InventoryItemInstanceMock)item;}
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
			public SlotGroup sg{
				get{
					return (SlotGroup)parent;
				}
			}
			public bool isPickable{
				get{
					bool result = true;
					if(sg.isPool){
						if(sg.isAutoSort){
							if(isEquipped || itemInst is PartsInstanceMock && !(sg.Filter is SGPartsFilter))
								result = false;
						}
					}
					return result;
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
			public bool isPickedUp{
				get{
					return ssm.pickedSB == this;
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
			public bool isStackable{
				get{return itemInst.Item.IsStackable;}
			}
		/*	SlotSystemElement imple	*/
			/*	fields	*/
				public override SlotSystemElement this[int i]{get{return null;}}
				protected override IEnumerable<SlotSystemElement> elements{
					get{return null;}
				}
				public override string eName{
					get{return Util.SBName(this);}
				}
				public override SlotSystemElement parent{
					get{
						return ssm.FindParent(this);
					}
					set{}
				}
				public override SlotSystemBundle immediateBundle{
					get{
						if(parent == null)
							return null;
						return parent.immediateBundle;
					}
				}
				public void SetSSM(SlotSystemManager ssm){
					this.ssm = ssm;
				}
				public override int level{
					get{return sg.level +1;}
				}
			/*	methods	*/
				public override IEnumerator<SlotSystemElement> GetEnumerator(){
					yield return null;
					}
				public override bool Contains(SlotSystemElement element){
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
				public override bool ContainsInHierarchy(SlotSystemElement element){
					return false;
				}
				public override void PerformInHierarchy(System.Action<SlotSystemElement> act){
					act(this);
				}
				public override void PerformInHierarchy(System.Action<SlotSystemElement, object> act, object obj){
					act(this, obj);
				}
				public override void PerformInHierarchy<T>(System.Action<SlotSystemElement, IList<T>> act, IList<T> obj){
					act(this, obj);
				}
		/*	Event methods	*/
			/*	Selection event	*/
				public void OnHoverEnterMock(){
					PointerEventDataMock eventData = new PointerEventDataMock();
					((SBSelState)curSelState).OnHoverEnterMock(this, eventData);
				}
				public void OnHoverExitMock(){
					PointerEventDataMock eventData = new PointerEventDataMock();
					((SBSelState)curSelState).OnHoverExitMock(this, eventData);
				}
			/*	Action Event	*/
				public void OnPointerDownMock(PointerEventDataMock eventDataMock){
					((SBActState)curActState).OnPointerDownMock(this, eventDataMock);
				}
				public void OnPointerUpMock(PointerEventDataMock eventDataMock){
					((SBActState)curActState).OnPointerUpMock(this, eventDataMock);
				}
				public void OnDeselectedMock(PointerEventDataMock eventDataMock){
					((SBActState)curActState).OnDeselectedMock(this, eventDataMock);
				}
				public void OnEndDragMock(PointerEventDataMock eventDataMock){
					((SBActState)curActState).OnEndDragMock(this, eventDataMock);
				}
		/*	othr Interface implementation	*/
			int IComparable.CompareTo(object other){
				if(!(other is Slottable))
					throw new InvalidOperationException("CompareTo: no a slottable");
				return CompareTo((Slottable)other);
			}
			public int CompareTo(Slottable other){
				return this.item.CompareTo(other.item);
			}
			public static bool operator > (Slottable a, Slottable b){
				return a.CompareTo(b) > 0;
			}
			public static bool operator < (Slottable a, Slottable b){
				return a.CompareTo(b) < 0;
			}
		/*	methods	*/
			public void Initialize(InventoryItemInstanceMock item){
				this.delayed = true;
				this.SetItem(item);
				selStateEngine.SetState(Slottable.sbDeactivatedState);
				actStateEngine.SetState(Slottable.sbWaitForActionState);
				EqpStateEngine.SetState(null);
			}
			public void PickUp(){
				SetActState(Slottable.pickedUpState);
				m_pickedAmount = 1;
			}
			public void Increment(){
				if(m_item.IsStackable && m_item.Quantity > m_pickedAmount){
					m_pickedAmount ++;
				}
			}
			public void ExecuteTransaction(){
				ssm.SetActState(SlotSystemManager.ssmTransactionState);
				ssm.transaction.Execute();
			}
			public void ExpireActionProcess(){
				if(actProcess.isRunning)
					actProcess.Expire();
			}
			public void UpdateEquipState(){
				if(itemInst.isEquipped) Equip();
				else Unequip();
			}
			public void Reset(){
				SetActState(Slottable.sbWaitForActionState);
				pickedAmount = 0;
				SetNewSlotID(-2);
			}
	}

}
