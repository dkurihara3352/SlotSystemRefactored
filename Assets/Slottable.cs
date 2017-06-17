using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;
namespace SlotSystem{
	public class Slottable : MonoBehaviour, IComparable<Slottable>, IComparable, StateHandler, SlotSystemElement{
		/*	States	*/
			/*	Selection State	*/
				SBStateEngine SelStateEngine{
					get{
						if(m_selStateEngine == null)
							m_selStateEngine = new SBStateEngine(this);
						return m_selStateEngine;
					}
					}SBStateEngine m_selStateEngine;
					public SBSelectionState CurSelState{
						get{return (SBSelectionState)SelStateEngine.curState;}
					}
					public SBSelectionState PrevSelState{
						get{return (SBSelectionState)SelStateEngine.prevState;}
					}
					public void SetSelState(SBSelectionState selState){
						SelStateEngine.SetState(selState);
					}

				public static SBSelectionState DeactivatedState{
					get{
						if(Slottable.m_deactivatedState != null)
							return Slottable.m_deactivatedState;
						else{
							Slottable.m_deactivatedState = new SBDeactivatedState();	
							return Slottable.m_deactivatedState;
						}
					}
					}static SBSelectionState m_deactivatedState;
				public static SBSelectionState SelectedState{
					get{
						if(Slottable.m_selectedState != null)
							return Slottable.m_selectedState;
						else
							Slottable.m_selectedState = new SBSelectedState();
							return Slottable.m_selectedState;
					}
					}static SBSelectionState m_selectedState;			
				public static SBSelectionState DefocusedState{
					get{
						if(Slottable.m_defocusedState != null)
							return Slottable.m_defocusedState;
						else{
							Slottable.m_defocusedState = new SBDefocusedState();
							return Slottable.m_defocusedState;
						}
					}
					}static SBSelectionState m_defocusedState;
				public static SBSelectionState FocusedState{
						get{
							if(Slottable.m_focusedState != null)
								return Slottable.m_focusedState;
							else{
								Slottable.m_focusedState = new SBFocusedState();
								return Slottable.m_focusedState;
							}
						}
						}static SBSelectionState m_focusedState;
			/*	Action State	*/
				SBStateEngine ActStateEngine{
					get{
						if(m_actStateEngine == null)
							m_actStateEngine = new SBStateEngine(this);
						return m_actStateEngine;
					}
					}SBStateEngine m_actStateEngine;
					public SBActionState CurActState{
						get{return (SBActionState)ActStateEngine.curState;}
					}
					public SBActionState PrevActState{
						get{return (SBActionState)ActStateEngine.prevState;}
					}
					public void SetActState(SBActionState actState){
						ActStateEngine.SetState(actState);
					}
				
				public static SBActionState WaitForActionState{
					get{
						if(m_waitForActionState != null)
							return m_waitForActionState;
						else{
							m_waitForActionState = new WaitForActionState();
							return m_waitForActionState;
						}
					}
					}static SBActionState m_waitForActionState;
				public static SBActionState WaitForPointerUpState{
					get{
						if(m_waitForPointerUpState != null)
							return m_waitForPointerUpState;
						else{
							m_waitForPointerUpState = new WaitForPointerUpState();
							return m_waitForPointerUpState;
						}
					}
					}static SBActionState m_waitForPointerUpState;
				public static SBActionState WaitForPickUpState{
					get{
						if(Slottable.m_waitForPickUpState != null)
							return Slottable.m_waitForPickUpState;
						else{
							Slottable.m_waitForPickUpState = new WaitForPickUpState();
							return Slottable.m_waitForPickUpState;
						}
					}
					}static SBActionState m_waitForPickUpState;
				public static SBActionState WaitForNextTouchState{
					get{
						if(Slottable.m_waitForNextTouchState != null)
							return Slottable.m_waitForNextTouchState;
						else{
							Slottable.m_waitForNextTouchState = new WaitForNextTouchState();
							return Slottable.m_waitForNextTouchState;
						}
					}
					}static SBActionState m_waitForNextTouchState;
				public static SBActionState PickedUpState{
					get{
						if(Slottable.m_pickedUpState != null)
							return Slottable.m_pickedUpState;
						else{
							Slottable.m_pickedUpState = new PickedUpState();
							return Slottable.m_pickedUpState;
						}
					}
					}static SBActionState m_pickedUpState;
				public static SBActionState RemovedState{
					get{
						if(Slottable.m_removedState == null)
							Slottable.m_removedState = new SBRemovedState();
						return Slottable.m_removedState;			
					}
					}static SBActionState m_removedState;
				public static SBActionState AddedState{
					get{
						if(Slottable.m_addedState == null)
							Slottable.m_addedState = new SBAddedState();
						return Slottable.m_addedState;			
					}
					}static SBActionState m_addedState;
				public static SBActionState MoveWithinState{
						get{
							if(Slottable.m_moveWithinState == null)
								Slottable.m_moveWithinState = new SBMoveWithinState();
							return Slottable.m_moveWithinState;			
						}
						}static SBActionState m_moveWithinState;
			/*	Equip State	*/
				SBStateEngine EqpStateEngine{
					get{
						if(m_eqpStateEngine == null)
							m_eqpStateEngine = new SBStateEngine(this);
						return m_eqpStateEngine;
					}
					}SBStateEngine m_eqpStateEngine;
					public SBEquipState CurEqpState{
						get{return (SBEquipState)EqpStateEngine.curState;}
					}
					public SBEquipState PrevEqpState{
						get{return (SBEquipState)EqpStateEngine.prevState;}
					}
					public void SetEqpState(SBEquipState actState){
						EqpStateEngine.SetState(actState);
					}
				
				public static SBEquipState EquippedState{
					get{
						if(m_equippedState != null)
							return m_equippedState;
						else{
							m_equippedState = new SBEquippedState();
							return m_equippedState;
						}
					}
					}static SBEquipState m_equippedState;
				public static SBEquipState UnequippedState{
					get{
						if(m_unequippedState != null)
							return m_unequippedState;
						else{
							m_unequippedState = new SBUnequippedState();
							return m_unequippedState;
						}
					}
					}static SBEquipState m_unequippedState;
		/*	processes	*/
			public SBProcess SelectionProcess{
				get{return m_selectionProcess;}
				}SBProcess m_selectionProcess;
				public void SetAndRunSelectionProcess(SBProcess process){
					if(m_selectionProcess != null)
						m_selectionProcess.Stop();
					m_selectionProcess = process;
					if(m_selectionProcess != null)
						m_selectionProcess.Start();
				}
				public IEnumeratorMock GreyoutCoroutine(){return null;}
				public IEnumeratorMock GreyinCoroutine(){return null;}
				public IEnumeratorMock HighlightCoroutine(){return null;}
				public IEnumeratorMock DehighlightCoroutine(){return null;}
			public SBProcess ActionProcess{
				get{return m_actionProcess;}
				}SBProcess m_actionProcess;
				public void SetAndRunActionProcess(SBProcess process){
					if(m_actionProcess != null)
						m_actionProcess.Stop();
					m_actionProcess = process;
					if(m_actionProcess != null)
						m_actionProcess.Start();
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
			public SBProcess EquipProcess{
				get{return m_equipProcess;}
				}SBProcess m_equipProcess;
				public void SetAndRunEquipProcess(SBProcess process){
					if(m_equipProcess != null)
						m_equipProcess.Stop();
					m_equipProcess = process;
					if(m_equipProcess != null)
						m_equipProcess.Start();
				}
				public IEnumeratorMock UnequipCoroutine(){return null;}
				public IEnumeratorMock EquipCoroutine(){return null;}
		/*	commands	*/
			static SlottableCommand m_tapCommand = new SBTapCommand();
				static public SlottableCommand TapCommand{
					get{return m_tapCommand;}
				}
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
				get{
					if(sg.slottables.Contains(this))
						return sg.slottables.IndexOf(this);
					else
						return -1;
				}
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
				get{return CurSelState == Slottable.FocusedState;}
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
			public SlotGroupManager sgm{
				get{return m_sgm;}
				}SlotGroupManager m_sgm;
				public void SetSGM(SlotGroupManager sgm){
					m_sgm = sgm;
				}
			public void Activate(){}
			public void Deactivate(){}
			public void Focus(){
				SetSelState(Slottable.FocusedState);
			}
			public void Defocus(){
				SetSelState(Slottable.DefocusedState);
			}
			public SlotSystemElement DirectParent(SlotSystemElement element){
				return null;
			}
			public bool ContainsInHierarchy(SlotSystemElement element){
				return false;
			}
		/*	Event methods	*/
			/*	Selection event	*/
				public void OnHoverEnterMock(){
					PointerEventDataMock eventData = new PointerEventDataMock();
					CurSelState.OnHoverEnterMock(this, eventData);
				}
				public void OnHoverExitMock(){
					PointerEventDataMock eventData = new PointerEventDataMock();
					CurSelState.OnHoverExitMock(this, eventData);
				}
			/*	Action Event	*/
				public void OnPointerDownMock(PointerEventDataMock eventDataMock){
					CurActState.OnPointerDownMock(this, eventDataMock);
				}
				public void OnPointerUpMock(PointerEventDataMock eventDataMock){
					CurActState.OnPointerUpMock(this, eventDataMock);
				}
				public void OnDeselectedMock(PointerEventDataMock eventDataMock){
					CurActState.OnDeselectedMock(this, eventDataMock);
				}
				public void OnEndDragMock(PointerEventDataMock eventDataMock){
					CurActState.OnEndDragMock(this, eventDataMock);
				}
		/*	Interface implementation	*/
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
		/*	InstantActions	*/
			public void InstantGreyout(){}
			public void InstantEquipAndGreyout(){}
			public void InstantGreyin(){}
			public void InstantEquipAndGreyin(){}
			public void InstantEquip(){}
			public void InstantUnequip(){}
			public void InstantHighlight(){}
		/*	methods	*/
			public void Initialize(InventoryItemInstanceMock item){
				this.delayed = true;
				this.SetItem(item);
				SelStateEngine.SetState(Slottable.DeactivatedState);
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
				if(ActionProcess.IsRunning)
					ActionProcess.Expire();
			}
			public void UpdateEquipState(){
				if(itemInst.isEquipped) Equip();
				else Unequip();
			}
			public void Reset(){
				ResetAction();
				pickedAmount = 0;
				SetNewSlotID(-2);
			}
			public void ResetAction(){
				SetActState(Slottable.WaitForActionState);
			}
	}

}
