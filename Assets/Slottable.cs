using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;
namespace SlotSystem{
	public class Slottable : MonoBehaviour, IComparable<Slottable>, IComparable, StateHandler{
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
				// static states
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
				//	static states
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
					public static SBActionState MovingInSGState{
						get{
							if(Slottable.m_movingInSGState == null)
								Slottable.m_movingInSGState = new SBMovingInSGState();
							return Slottable.m_movingInSGState;			
						}
						}static SBActionState m_movingInSGState;
					public static SBActionState RevertingState{
						get{
							if(Slottable.m_revertingState == null)
								Slottable.m_revertingState = new SBRevertingState();
							return Slottable.m_revertingState;			
						}
						}static SBActionState m_revertingState;
					public static SBActionState MovingOutState{
						get{
							if(Slottable.m_movingOutState == null)
								Slottable.m_movingOutState = new SBMovingOutState();
							return Slottable.m_movingOutState;			
						}
						}static SBActionState m_movingOutState;
					public static SBActionState MovingInState{
						get{
							if(Slottable.m_movingInState == null)
								Slottable.m_movingInState = new SBMovingInState();
							return Slottable.m_movingInState;			
						}
						}static SBActionState m_movingInState;
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
				//	static states
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
			/*	coroutine	*/
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
			/*	coroutine */
				public IEnumeratorMock WaitForPointerUpCoroutine(){return null;}
				public IEnumeratorMock WaitForPickUpCoroutine(){return null;}
				public IEnumeratorMock PickedUpCoroutine(){return null;}
				public IEnumeratorMock WaitForNextTouchCoroutine(){return null;}
				public IEnumeratorMock UnpickCoroutine(){return null;}
				public IEnumeratorMock PickUpCoroutine(){return null;}
				public IEnumeratorMock RemovedCoroutine(){return null;}
				public IEnumeratorMock AddedCoroutine(){return null;}
				public IEnumeratorMock MoveInSGCoroutine(){return null;}
				public IEnumeratorMock RevertCoroutine(){return null;}
				public IEnumeratorMock MoveInCoroutine(){return null;}
				public IEnumeratorMock MoveOutCoroutine(){return null;}
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
			/*	coroutine	*/
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
			/*	dump	*/
				// static SlottableCommand m_instantDeactivateCommand = new DefInstantDeactivateCommand();
				// 	public static SlottableCommand InstantDeactivateCommand{
				// 		get{return m_instantDeactivateCommand;}
				// 	}
				// 	public void InstantDeactivate(){
				// 		m_instantDeactivateCommand.Execute(this);
				// 	}
		/*	public fields	*/
			public bool Delayed{
				get{return m_delayed;}
				set{m_delayed = value;}
				}bool m_delayed = true;
			public int PickedAmount{
				get{return m_pickedAmount;}
				set{m_pickedAmount = value;}
				}int m_pickedAmount = 0;
			public SlottableItem Item{
				get{return m_item;}
				}SlottableItem m_item;
				public void SetItem(SlottableItem item){
					m_item = item;
				}
			public InventoryItemInstanceMock ItemInst{
					get{return (InventoryItemInstanceMock)Item;}
				}
			public SlotGroupManager SGM{
				get{return m_sgm;}
				set{m_sgm = value;}
				}SlotGroupManager m_sgm;
			public SlotGroup DestinationSG{
				get{return m_destinationSG;}
				}SlotGroup m_destinationSG;
			public Slot DestinationSlot{
				get{return m_destinationSlot;}
				}Slot m_destinationSlot;
			public int SlotID{
				get{return SG.Slottables.IndexOf(this);}
			}
			public SlotGroup SG{
				get{
					if(m_sg == null){
						m_sg = SGM.GetSlotGroup(this);
					}
					return m_sg;
				}
				}SlotGroup m_sg;
				public void SetSG(SlotGroup sg){
					/*	use this only when creating a new Sb in transaction	*/
					m_sg = sg;
				}
			public bool IsPickable{
				get{
					bool result = true;
					if(SG.IsPool){
						if(SG.IsAutoSort){
							if(IsEquipped || ItemInst is PartsInstanceMock && !(SG.Filter is SGPartsFilter))
								result = false;
						}
					}
					return result;
				}
			}
			public bool IsFocused{
				get{return CurSelState == Slottable.FocusedState;}
			}
			public bool IsPickedUp{
				get{
					return SGM.PickedSB == this;
				}
			}
			public bool IsEquipped{
				get{ return ItemInst.IsEquipped;}
				}public void Equip(){
					SetEqpState(Slottable.EquippedState);
				}
				public void Unequip(){
					SetEqpState(Slottable.UnequippedState);
				}
			public bool IsStackable{
				get{return ItemInst.Item.IsStackable;}
			}
		
		/*	Event methods	*/
			/*	Selection event	*/
				public void Focus(){
					SetActState(Slottable.WaitForActionState);
					if(IsPickable)
						SetSelState(Slottable.FocusedState);
					else
						SetSelState(Slottable.DefocusedState);
				}
				public void Defocus(){
					SetSelState(Slottable.DefocusedState);
				}
				public void OnHoverEnterMock(PointerEventDataMock eventDataMock){
					CurSelState.OnHoverEnterMock(this, eventDataMock);
				}
				public void OnHoverExitMock(PointerEventDataMock eventDataMock){
					CurSelState.OnHoverExitMock(this, eventDataMock);
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
				return this.Item.CompareTo(other.Item);
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
			public void Initialize(SlotGroupManager sgm, SlotGroup sg, bool delayed, InventoryItemInstanceMock item){
				this.m_sgm = sgm;
				SetSG(sg);
				Delayed = delayed;
				this.SetItem(item);
				SelStateEngine.SetState(Slottable.DeactivatedState);
				ActStateEngine.SetState(Slottable.WaitForActionState);
				EqpStateEngine.SetState(Slottable.UnequippedState);
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
			public void Deactivate(){}
			public void ExecuteTransaction(){
				SGM.Transaction.Execute();
			}
			public void MoveDraggedIcon(SlotGroup sg, Slot slot){
				SetDraggedIconDestination(sg, slot);
			}
			public void SetDraggedIconDestination(SlotGroup sg, Slot slot){
				this.m_destinationSG = sg;
				this.m_destinationSlot = slot;
			}
			public void ClearDraggedIconDestination(){
				this.m_destinationSG = null;
				this.m_destinationSlot = null;
			}
			public void GetSlotIndex(out int curID, out int newID){
				SG.GetSlotMovement(this).GetIndex(out curID, out newID);
			}
			public void ExpireActionProcess(){
				if(ActionProcess.IsRunning)
					ActionProcess.Expire();
			}
			public void UpdateEquipState(){
				if(ItemInst.IsEquipped) Equip();
				else Unequip();
			}
	}

}
