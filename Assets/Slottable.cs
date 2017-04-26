using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class Slottable : MonoBehaviour{

		
		/* States
		*/
			
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

			static SlottableState m_equippedState;
			public static SlottableState EquippedState{
				get{
					if(Slottable.m_equippedState != null)
						return Slottable.m_equippedState;
					else
						Slottable.m_equippedState = new EquippedState();
						return Slottable.m_equippedState;
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
		/* commands
		*/
			static SlottableCommand m_instantDeactivateCommand = new DefInstantDeactivateCommand();
			public static SlottableCommand InstantDeactivateCommand{
				get{return m_instantDeactivateCommand;}
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
			
			bool m_isPickupTimerOn;
			public bool IsPickupTimerOn{
				get{
					return m_isPickupTimerOn;
				}
				set{
					m_isPickupTimerOn = value;
				}
			}

			bool m_isTapTimerOn;
			public bool IsTapTimerOn{
				get{
					return m_isTapTimerOn;
				}
				set{m_isTapTimerOn = value;}
			}
			bool m_isRevertTimerOn;
			public bool IsRevertTimerOn{
				get{return m_isRevertTimerOn;}
				set{m_isRevertTimerOn = value;}
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

			bool m_releasedInside;
			public bool ReleasedInside{
				get{return m_releasedInside;}
				set{m_releasedInside = value;}
			}

			SlotGroupManager m_sgm;
			public SlotGroupManager SGM{
				get{return m_sgm;}
				set{m_sgm = value;}
			}
			
		/*	processes
		*/
			SBProcess m_gradualGrayoutProcess;
				public SBProcess GradualGrayoutProcess{
					get{return m_gradualGrayoutProcess;}
					set{m_gradualGrayoutProcess = value;}
				}
				public IEnumeratorMock GradualGrayoutCoroutine(){
					return null;
				}
			SBProcess m_gradualHighlightProcess;
				public SBProcess GradualGrayinProcess{
					get{return m_gradualHighlightProcess;}
					set{m_gradualHighlightProcess = value;}
				}
				public IEnumeratorMock GradualHighlightCoroutine(){
					return null;
				}
			SBProcess m_gradualDehighlightProcess;
				public SBProcess GradualDehighlightProcess{
					get{return m_gradualDehighlightProcess;}
					set{m_gradualDehighlightProcess = value;}
				}
				public IEnumeratorMock GradualDehighlightCoroutine(){
					return null;
				}
			SBProcess m_waitAndSetBackToDefocusedStateProcess;
				public SBProcess WaitAndSetBackToDefocusedStateProcess{
					get{return m_waitAndSetBackToDefocusedStateProcess;}
					set{m_waitAndSetBackToDefocusedStateProcess = value;}
				}
				public IEnumeratorMock WaitAndSetBackToDefocusedStateCoroutine(){
					return null;
				}
			SBProcess m_waitAndPickUpProcess;
				public SBProcess WaitAndPickUpProcess{
					get{return m_waitAndPickUpProcess;}
					set{m_waitAndPickUpProcess = value;}
				}
				public IEnumeratorMock WaitAndPickUpCoroutine(){
					return null;
				}
			SBProcess m_pickedUpAndSelectedProcess;
				public SBProcess PickedUpAndSelectedProcess{
					get{return m_pickedUpAndSelectedProcess;}
					set{m_pickedUpAndSelectedProcess = value;}
				}
				public IEnumeratorMock PickedUpAndSelectedCoroutine(){
					return null;
				}
		string m_UTLog = "";
		public string UTLog{
			get{return m_UTLog;}
			set{m_UTLog = value;}
		}
		public void ClearLog(){
			m_UTLog = "";
		}
		public void InitializeProcesses(){	
			this.GradualGrayoutProcess = new GradualGrayoutProcess(this, GradualGrayoutCoroutine);
			this.GradualGrayinProcess = new GradualGrayinProcess(this, GradualHighlightCoroutine);
			this.GradualDehighlightProcess = new GradualDehighlightProcess(this, GradualDehighlightCoroutine);
			this.WaitAndSetBackToDefocusedStateProcess = new WaitAndSetBackToDefocusedStateProcess(this, WaitAndSetBackToDefocusedStateCoroutine);
			this.WaitAndPickUpProcess = new WaitAndPickUpProcess(this, WaitAndPickUpCoroutine);
			this.PickedUpAndSelectedProcess = new PickedUpAndSelectedProcess(this, PickedUpAndSelectedCoroutine);
		}
		public void Initialize(SlotGroup sg){
			InitializeProcesses();
			m_curState = Slottable.DeactivatedState;
			// SetState(Slottable.DeactivatedState);
			m_prevState = Slottable.DeactivatedState;
			this.m_sgm = sg.SGM;
		}

		public void SetState(SlottableState state){
			if(this.m_curState != state){
				this.m_prevState = this.m_curState;
				this.m_curState = state;
				this.m_prevState.ExitState(this);
				this.m_curState.EnterState(this);
			}
		}

		public void Defocus(){
			
		}

		public void OnPointerDownMock(PointerEventDataMock eventDataMock){
			m_curState.OnPointerDownMock(this, eventDataMock);
		}
		public void WaitAndPickupMock(){
			m_isPickupTimerOn = true;
		}
		public void WaitAndTapMock(){
			m_isTapTimerOn = true;
		}
		public void WaitAndRevertMock(){
			m_isRevertTimerOn = true;
		}
		public void OnPointerUpMock(PointerEventDataMock eventDataMock){
			m_curState.OnPointerUpMock(this, eventDataMock);
		}
		public void OnHoveredMock(PointerEventDataMock eventDataMock){
			m_curState.OnHoveredMock(this, eventDataMock);
		}
		public void OnDehoveredMock(PointerEventDataMock eventDataMock){
			m_curState.OnDehoveredMock(this, eventDataMock);
		}
		
		public void ExpirePickupTimer(){
			if(m_isPickupTimerOn){
				m_isPickupTimerOn = false;
				PickUp();
			}
		}
		public void ExpireTapTimer(){
			if(m_isTapTimerOn){
				m_isTapTimerOn = false;
				OnTap();
			}
		}
		public void ExpireRevertTimer(){
			if(m_isRevertTimerOn){
				m_isRevertTimerOn = false;
				Revert();
			}
		}
		public void Cancel(){
			m_UTLog = "Canceled";
			ResetTimers();
			FilteredInMock();
		}
		public void FilteredInMock(){
			SetState(Slottable.FocusedState);
		}
		public void OnDeselectedMock(PointerEventDataMock eventDataMock){
			CurState.OnDeselectedMock(this, eventDataMock);
		}
		public void FingerMoveOverThreshMock(PointerEventDataMock eventDataMock){
			OnEndDragMock(eventDataMock);
		}
		public void OnEndDragMock(PointerEventDataMock eventDataMock){
			m_curState.OnEndDragMock(this, eventDataMock);
		}

		public void OnTap(){
			m_UTLog = "tapped";
			// ResetTimers();
			// FilteredInMock();
		}
		public void PickUp(){
			SetState(Slottable.PickedUpAndSelectedState);
			m_pickedAmount = 1;
		}

		public void SetSlottableItem(SlottableItem item){
			m_item = item;
		}

		public void Revert(){
			m_UTLog = "Reverted";
			ResetTimers();
			FilteredInMock();
		}
		public void Increment(){
			if(m_item.IsStackable && m_item.Quantity > m_pickedAmount){
				m_pickedAmount ++;
				m_UTLog = "Incremented";
			}
		}

		public void ResetTimers(){
			m_isPickupTimerOn = false;
			m_isRevertTimerOn = false;
			m_isTapTimerOn = false;
		}
		public void InstantDeactivate(){
			m_instantDeactivateCommand.Execute(this);
		}
		public void InstantGrayout(){
			m_UTLog = "InstantGrayout called";
		}
		public void InstantGrayin(){
			m_UTLog = "InstantGrayin called";
		}
		public void Deactivate(){}
	}

}
