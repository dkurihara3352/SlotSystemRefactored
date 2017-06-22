﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;
namespace SlotSystem{
	public class Slottable : MonoBehaviour, IComparable<Slottable>, IComparable, SlotSystemElement{
		/*	States	*/
			/*	Selection State	*/
				SSEStateEngine selStateEngine{
					get{
						if(m_selStateEngine == null)
							m_selStateEngine = new SSEStateEngine(this);
						return m_selStateEngine;
					}
					}SSEStateEngine m_selStateEngine;
					public SSEState curSelState{
						get{return (SBSelState)selStateEngine.curState;}
					}
					public SSEState prevSelState{
						get{return (SBSelState)selStateEngine.prevState;}
					}
					public void SetSelState(SSEState state){
						if(state == null || state is SBSelState)
							selStateEngine.SetState(state);
						else
							throw new System.InvalidOperationException("Slottable.SetSelState: something other than SBSelectionState is beint attempted to be assigned");
					}
				public static SBSelState DeactivatedState{
					get{
						if(Slottable.m_deactivatedState != null)
							return Slottable.m_deactivatedState;
						else{
							Slottable.m_deactivatedState = new SBDeactivatedState();	
							return Slottable.m_deactivatedState;
						}
					}
					}static SBSelState m_deactivatedState;
				public static SBSelState SelectedState{
					get{
						if(Slottable.m_selectedState != null)
							return Slottable.m_selectedState;
						else
							Slottable.m_selectedState = new SBSelectedState();
							return Slottable.m_selectedState;
					}
					}static SBSelState m_selectedState;			
				public static SBSelState DefocusedState{
					get{
						if(Slottable.m_defocusedState != null)
							return Slottable.m_defocusedState;
						else{
							Slottable.m_defocusedState = new SBDefocusedState();
							return Slottable.m_defocusedState;
						}
					}
					}static SBSelState m_defocusedState;
				public static SBSelState FocusedState{
						get{
							if(Slottable.m_focusedState != null)
								return Slottable.m_focusedState;
							else{
								Slottable.m_focusedState = new SBFocusedState();
								return Slottable.m_focusedState;
							}
						}
						}static SBSelState m_focusedState;
			/*	Action State	*/
				SSEStateEngine ActStateEngine{
					get{
						if(m_actStateEngine == null)
							m_actStateEngine = new SSEStateEngine(this);
						return m_actStateEngine;
					}
					}SSEStateEngine m_actStateEngine;
					public SSEState curActState{
						get{return (SBActState)ActStateEngine.curState;}
					}
					public SSEState prevActState{
						get{return (SBActState)ActStateEngine.prevState;}
					}
					public void SetActState(SSEState state){
						if(state == null || state is SBActState)
							ActStateEngine.SetState(state);
						else
							throw new System.InvalidOperationException("Slottable.SetActState: something other than SBActionState is being attempted to be assigned");
					}
				public static SBActState WaitForActionState{
					get{
						if(m_waitForActionState != null)
							return m_waitForActionState;
						else{
							m_waitForActionState = new WaitForActionState();
							return m_waitForActionState;
						}
					}
					}static SBActState m_waitForActionState;
				public static SBActState WaitForPointerUpState{
					get{
						if(m_waitForPointerUpState != null)
							return m_waitForPointerUpState;
						else{
							m_waitForPointerUpState = new WaitForPointerUpState();
							return m_waitForPointerUpState;
						}
					}
					}static SBActState m_waitForPointerUpState;
				public static SBActState WaitForPickUpState{
					get{
						if(Slottable.m_waitForPickUpState != null)
							return Slottable.m_waitForPickUpState;
						else{
							Slottable.m_waitForPickUpState = new WaitForPickUpState();
							return Slottable.m_waitForPickUpState;
						}
					}
					}static SBActState m_waitForPickUpState;
				public static SBActState WaitForNextTouchState{
					get{
						if(Slottable.m_waitForNextTouchState != null)
							return Slottable.m_waitForNextTouchState;
						else{
							Slottable.m_waitForNextTouchState = new WaitForNextTouchState();
							return Slottable.m_waitForNextTouchState;
						}
					}
					}static SBActState m_waitForNextTouchState;
				public static SBActState PickedUpState{
					get{
						if(Slottable.m_pickedUpState != null)
							return Slottable.m_pickedUpState;
						else{
							Slottable.m_pickedUpState = new PickedUpState();
							return Slottable.m_pickedUpState;
						}
					}
					}static SBActState m_pickedUpState;
				public static SBActState RemovedState{
					get{
						if(Slottable.m_removedState == null)
							Slottable.m_removedState = new SBRemovedState();
						return Slottable.m_removedState;			
					}
					}static SBActState m_removedState;
				public static SBActState AddedState{
					get{
						if(Slottable.m_addedState == null)
							Slottable.m_addedState = new SBAddedState();
						return Slottable.m_addedState;			
					}
					}static SBActState m_addedState;
				public static SBActState MoveWithinState{
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
				public static SBEqpState EquippedState{
					get{
						if(m_equippedState != null)
							return m_equippedState;
						else{
							m_equippedState = new SBEquippedState();
							return m_equippedState;
						}
					}
					}static SBEqpState m_equippedState;
				public static SBEqpState UnequippedState{
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
				public SSEProcessEngine selProcEngine{
					get{
						if(m_processEngine == null)
							m_processEngine = new SSEProcessEngine();
						return m_processEngine;
					}
					}SSEProcessEngine m_processEngine;
					public SSEProcess selProcess{
						get{return (SBSelProcess)selProcEngine.process;}
					}
					public SBSelProcess sbSelProcess{
						get{return (SBSelProcess)selProcess;}
					}
					public void SetAndRunSelProcess(SSEProcess process){
						if(process ==null || process is SBSelProcess)
							selProcEngine.SetAndRunProcess(process);
						else throw new System.InvalidOperationException("Slottable.SetAndRunSelProcess: argument is not of type SBSelProcess");
					}
				public IEnumeratorMock greyoutCoroutine(){return null;}
				public IEnumeratorMock greyinCoroutine(){return null;}
				public IEnumeratorMock highlightCoroutine(){return null;}
				public IEnumeratorMock dehighlightCoroutine(){return null;}
			/*	Action Process	*/
				public SSEProcessEngine actProcEngine{
					get{
						if(m_actProcEngine == null)
							m_actProcEngine = new SSEProcessEngine();
						return m_actProcEngine;}
					}SSEProcessEngine m_actProcEngine;
					public SSEProcess actProcess{
						get{return (SBActProcess)actProcEngine.process;}
					}
					public SBActProcess sbActProcess{
						get{return (SBActProcess)actProcess;}
					}
					public void SetAndRunActProcess(SSEProcess process){
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
					return sgm.GetSG(this);
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
			public bool isFocused{
				get{return curSelState == Slottable.FocusedState;}
			}
			public bool isPickedUp{
				get{
					return sgm.pickedSB == this;
				}
			}
			public bool isEquipped{
				get{ return itemInst.isEquipped;}
				}public void Equip(){
					SetEqpState(Slottable.EquippedState);
				}
				public void Unequip(){
					SetEqpState(Slottable.UnequippedState);
				}
			public bool isStackable{
				get{return itemInst.Item.IsStackable;}
			}
		/*	SlotSystemElement imple	*/
			/*	fields	*/
				public SlotSystemElement this[int i]{get{return null;}}
				public string eName{
					get{return Util.SBName(this);}
				}
				public SlotSystemElement parent{
					get{
						return ((InventoryManagerPage)rootElement).FindParent(this);
					}
					set{}
				}
				public SlotSystemBundle immediateBundle{
					get{
						if(parent == null)
							return null;
						return parent.immediateBundle;
					}
				}
				public SlotGroupManager sgm{
					get{return m_sgm;}
					set{m_sgm = value;}
					}SlotGroupManager m_sgm;
					public void SetSGM(SlotGroupManager sgm){
						m_sgm = sgm;
					}
				public int level{
					get{return sg.level +1;}
				}
				public SlotSystemElement rootElement{
					get{return m_rootElement;}
					set{m_rootElement = value;}
					}SlotSystemElement m_rootElement;
			/*	methods	*/
				public IEnumerator<SlotSystemElement> GetEnumerator(){
					yield return null;
					}IEnumerator IEnumerable.GetEnumerator(){
						return GetEnumerator();
					}
				public bool Contains(SlotSystemElement element){
					return false;
				}
				public void Activate(){}
				public void Deactivate(){}
				public void Focus(){
					SetSelState(Slottable.FocusedState);
				}
				public void Defocus(){
					SetSelState(Slottable.DefocusedState);
				}
				public SlotSystemElement FindParentInHierarchy(SlotSystemElement element){
					return null;
				}
				public bool ContainsInHierarchy(SlotSystemElement element){
					return false;
				}
				public void PerformInHierarchy(System.Action<SlotSystemElement> act){
					act(this);
				}
				public void PerformInHierarchy(System.Action<SlotSystemElement, object> act, object obj){
					act(this, obj);
				}
				public void PerformInHierarchy<T>(System.Action<SlotSystemElement, IList<T>> act, IList<T> obj){
					act(this, obj);
				}
				public void InstantGreyout(){}
				public void InstantGreyin(){}
				public void InstantHighlight(){}
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
				selStateEngine.SetState(Slottable.DeactivatedState);
				ActStateEngine.SetState(Slottable.WaitForActionState);
				EqpStateEngine.SetState(null);
			}
			public void PickUp(){
				SetActState(Slottable.PickedUpState);
				m_pickedAmount = 1;
			}
			public void Increment(){
				if(m_item.IsStackable && m_item.Quantity > m_pickedAmount){
					m_pickedAmount ++;
				}
			}
			public void ExecuteTransaction(){
				sgm.SetActState(SlotGroupManager.PerformingTransactionState);
				sgm.Transaction.Execute();
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
				SetActState(Slottable.WaitForActionState);
				pickedAmount = 0;
				SetNewSlotID(-2);
			}
	}

}
