﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class Slottable : MonoBehaviour{

		
		/* States
		*/	
			public void SetState(SlottableState state){
				if(this.m_curState != state){
					this.m_prevState = this.m_curState;
					this.m_curState = state;
					this.m_prevState.ExitState(this);
					this.m_curState.EnterState(this);
				}
			}	
			static SlottableState m_deactivatedState;
				public static SlottableState DeactivatedState{
					get{
						if(Slottable.m_deactivatedState != null)
							return Slottable.m_deactivatedState;
						else{
							Slottable.m_deactivatedState = new DeactivatedState();	
							return Slottable.m_deactivatedState;
						}
					}
				}
			
			static SlottableState m_defocusedState;
				public static SlottableState DefocusedState{
					get{
						if(Slottable.m_defocusedState != null)
							return Slottable.m_defocusedState;
						else{
							Slottable.m_defocusedState = new DefocusedState();
							return Slottable.m_defocusedState;
						}
					}
				}
			static SlottableState m_focusedState;
				public static SlottableState FocusedState{
					get{
						if(Slottable.m_focusedState != null)
							return Slottable.m_focusedState;
						else{
							Slottable.m_focusedState = new FocusedState();
							return Slottable.m_focusedState;
						}
					}
				}
			static SlottableState m_waitForPointerUpState;
				public static SlottableState WaitForPointerUpState{
					get{
						if(m_waitForPointerUpState != null)
							return m_waitForPointerUpState;
						else{
							m_waitForPointerUpState = new WaitForPointerUpState();
							return m_waitForPointerUpState;
						}
					}
				}

			static SlottableState m_waitForPickUpState;
				public static SlottableState WaitForPickUpState{
					get{
						if(Slottable.m_waitForPickUpState != null)
							return Slottable.m_waitForPickUpState;
						else{
							Slottable.m_waitForPickUpState = new WaitForPickUpState();
							return Slottable.m_waitForPickUpState;
						}
					}
				}
			static SlottableState m_waitForNextTouchState;
				public static SlottableState WaitForNextTouchState{
					get{
						if(Slottable.m_waitForNextTouchState != null)
							return Slottable.m_waitForNextTouchState;
						else{
							Slottable.m_waitForNextTouchState = new WaitForNextTouchState();
							return Slottable.m_waitForNextTouchState;
						}
					}
				}
			static SlottableState m_pickedUpAndSelectedState;
				public static SlottableState PickedUpAndSelectedState{
					get{
						if(Slottable.m_pickedUpAndSelectedState != null)
							return Slottable.m_pickedUpAndSelectedState;
						else{
							Slottable.m_pickedUpAndSelectedState = new PickedUpAndSelectedState();
							return Slottable.m_pickedUpAndSelectedState;
						}
					}
				}
			static SlottableState m_pickedUpAndDeselectedState;
				public static SlottableState PickedUpAndDeselectedState{
					get{
						if(Slottable.m_pickedUpAndDeselectedState != null)
							return Slottable.m_pickedUpAndDeselectedState;
						else{
							Slottable.m_pickedUpAndDeselectedState = new PickedUpAndDeselectedState();
							return Slottable.m_pickedUpAndDeselectedState;
						}
					}
				}
			static SlottableState m_waitForNextTouchWhilePUState;
				public static SlottableState WaitForNextTouchWhilePUState{
					get{
						if(Slottable.m_waitForNextTouchWhilePUState != null)
							return Slottable.m_waitForNextTouchWhilePUState;
						else{
							Slottable.m_waitForNextTouchWhilePUState = new WaitForNextTouchWhilePUState();
							return Slottable.m_waitForNextTouchWhilePUState;
						}
					}
				}
			static SlottableState m_movingState;
				public static SlottableState MovingState{
					get{
						if(Slottable.m_movingState != null)
							return Slottable.m_movingState;
						else{
							Slottable.m_movingState = new MovingState();
							return Slottable.m_movingState;
						}
					}
				}

			static SlottableState m_equippedAndDeselectedState;
				public static SlottableState EquippedAndDeselectedState{
					get{
						if(Slottable.m_equippedAndDeselectedState != null)
							return Slottable.m_equippedAndDeselectedState;
						else
							Slottable.m_equippedAndDeselectedState = new EquippedAndDeselectedState();
							return Slottable.m_equippedAndDeselectedState;
					}
				}
			static SlottableState m_equippedAndSelectedState;
				public static SlottableState EquippedAndSelectedState{
					get{
						if(Slottable.m_equippedAndSelectedState != null)
							return Slottable.m_equippedAndSelectedState;
						else
							Slottable.m_equippedAndSelectedState = new EquippedAndSelectedState();
							return Slottable.m_equippedAndSelectedState;
					}
				}
			static SlottableState m_equippedAndDefocusedState;
				public static SlottableState EquippedAndDefocusedState{
					get{
						if(Slottable.m_equippedAndDefocusedState != null)
							return Slottable.m_equippedAndDefocusedState;
						else
							Slottable.m_equippedAndDefocusedState = new EquippedAndDefocusedState();
							return Slottable.m_equippedAndDefocusedState;
					}
				}

			static SlottableState m_selectedState;
				public static SlottableState SelectedState{
					get{
						if(Slottable.m_selectedState != null)
							return Slottable.m_selectedState;
						else
							Slottable.m_selectedState = new SBSelectedState();
							return Slottable.m_selectedState;
					}
				}
			// static SlottableState m_revertingState;
				// public static SlottableState RevertingState{
				// 	get{
				// 		if(Slottable.m_revertingState != null)
				// 			return Slottable.m_revertingState;
				// 		else
				// 			Slottable.m_revertingState = new SBRevertingState();
				// 			return Slottable.m_revertingState;
				// 	}
				// }
		
		/* commands
		*/
			static SlottableCommand m_instantDeactivateCommand = new DefInstantDeactivateCommand();
				public static SlottableCommand InstantDeactivateCommand{
					get{return m_instantDeactivateCommand;}
				}
			static SlottableCommand m_tapCommand = new SBTapCommand();
				static public SlottableCommand TapCommand{
					get{return m_tapCommand;}
				}
		/* public fields
		*/			
			SlottableState m_curState;
				public SlottableState CurState{
					get{return m_curState;}
				}
			SlottableState m_prevState;
				public SlottableState PrevState{
					get{return m_prevState;}
				}
			bool m_delayed = true;
				public bool Delayed{
					get{return m_delayed;}
					set{m_delayed = value;}
				}	
			int m_pickedAmount = 0;
				public int PickedAmount{
					get{return m_pickedAmount;}
					set{m_pickedAmount = value;}
				}
			SlottableItem m_item;
				public SlottableItem Item{
					get{return m_item;}
				}
				public void SetItem(SlottableItem item){
					m_item = item;
				}
			SlotGroupManager m_sgm;
				public SlotGroupManager SGM{
					get{return m_sgm;}
					set{m_sgm = value;}
				}
			bool m_isEquipped;
				public bool IsEquipped{
					get{
						InventoryItemInstanceMock invInst = (InventoryItemInstanceMock)m_item;
						return invInst.IsEquipped;
					}
				}
			SlotGroup m_destinationSG;
				public SlotGroup DestinationSG{
					get{
						return m_destinationSG;
					}
				}
			Slot m_destinationSlot;
				public Slot DestinationSlot{
					get{return m_destinationSlot;}
				}
		/*	processes
		*/
			SBProcess m_curProcess;
			public SBProcess CurProcess{
				get{return m_curProcess;}
			}
			public void SetAndRun(SBProcess process){
				if(m_curProcess != null)
					m_curProcess.Stop();
				m_curProcess = process;
				if(m_curProcess != null)
					m_curProcess.Start();
			}
			/*	coroutines
			*/
				public IEnumeratorMock GradualGrayoutCoroutine(){
					return null;
				}
				public IEnumeratorMock EquipGradualGrayoutCoroutine(){
					return null;
				}
				public IEnumeratorMock GradualGrayinCoroutine(){
					return null;
				}
				public IEnumeratorMock EquipGradualGrayinCoroutine(){
					return null;
				}
				public IEnumeratorMock GradualHighlightCoroutine(){
					return null;
				}
				public IEnumeratorMock EquipGradualHighlightCoroutine(){
					return null;
				}
				public IEnumeratorMock GradualDehighlightCoroutine(){
					return null;
				}
				public IEnumeratorMock EquipGradualDehighlightCoroutine(){
					return null;
				}
				public IEnumeratorMock WaitForPointerUpCoroutine(){
					return null;
				}
				public IEnumeratorMock WaitForPickUpCoroutine(){
					return null;
				}
				public IEnumeratorMock PickedUpAndSelectedCoroutine(){
					return null;
				}
				public IEnumeratorMock PickedUpAndDeselectedCoroutine(){
					return null;
				}
				// public IEnumeratorMock RevertingStateCoroutine(){
				// 	return null;
				// }
				public IEnumeratorMock MoveCoroutine(){
					return null;
				}
				public IEnumeratorMock WaitForNextTouchWhilePUCoroutine(){
					return null;
				}
				public IEnumeratorMock WaitForNextTouchCoroutine(){
					return null;
				}
		/*	Event methods
		*/
			public void OnPointerDownMock(PointerEventDataMock eventDataMock){
				m_curState.OnPointerDownMock(this, eventDataMock);
			}
			public void OnPointerUpMock(PointerEventDataMock eventDataMock){
				m_curState.OnPointerUpMock(this, eventDataMock);
			}
			public void OnHoverEnterMock(PointerEventDataMock eventDataMock){
				m_curState.OnHoverEnterMock(this, eventDataMock);
			}
			public void OnHoverExitMock(PointerEventDataMock eventDataMock){
				m_curState.OnHoverExitMock(this, eventDataMock);
			}
			public void OnDeselectedMock(PointerEventDataMock eventDataMock){
				CurState.OnDeselectedMock(this, eventDataMock);
			}
			public void OnEndDragMock(PointerEventDataMock eventDataMock){
				m_curState.OnEndDragMock(this, eventDataMock);
			}
			public void Focus(){
				m_curState.Focus(this);
			}
			public void Defocus(){
				m_curState.Defocus(this);
			}
		/**/
		
		public void Initialize(SlotGroup sg){
			m_curState = Slottable.DeactivatedState;
			m_prevState = Slottable.DeactivatedState;
			this.m_sgm = sg.SGM;
		}
		

		public void Tap(){
			m_tapCommand.Execute(this);
			Tapped = true;
		}
			public bool Tapped = false;
		public void PickUp(){
			SetState(Slottable.PickedUpAndSelectedState);
			m_pickedAmount = 1;
		}

		public void Increment(){
			if(m_item.IsStackable && m_item.Quantity > m_pickedAmount){
				m_pickedAmount ++;
			}
		}
		public void InstantDeactivate(){
			m_instantDeactivateCommand.Execute(this);
		}
		public void InstantGrayout(){
			IsInstGOCalled = true;
		}
			public bool IsInstGOCalled = false;
		public void InstantEquipGrayout(){
			IsInstGOCalled = true;
		}
			public bool IsInstEqGOCalled = false;
		public void InstantGrayin(){
			IsInstGICalled = true;
		}
			public bool IsInstGICalled = false;
		public void InstantEquipGrayin(){
			IsInstEqGICalled = true;
		}
			public bool IsInstEqGICalled = false;
		public void Deactivate(){}
		public void ExecuteTransaction(){
			SGM.Transaction.Execute();
		}
		public void Move(SlotGroup sg, Slot slot){
			SetDestination(sg, slot);
			this.SetState(Slottable.MovingState);
		}
		public void SetDestination(SlotGroup sg, Slot slot){
			this.m_destinationSG = sg;
			this.m_destinationSlot = slot;
		}
		public void ClearDestination(){
			this.m_destinationSG = null;
			this.m_destinationSlot = null;
		}
	}

}
