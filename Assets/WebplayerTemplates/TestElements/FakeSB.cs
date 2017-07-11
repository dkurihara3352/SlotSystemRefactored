using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotSystem;
public class FakeSB : Slottable {
	public string message;
		public override void InstantGreyout(){message = "InstantGreyout called";}
		public override void InstantGreyin(){message = "InstantGreyin called";}
		public override void InstantHighlight(){message = "InstantHighlight called";}
	/*	Selection */
		public override void SetAndRunSelProcess(SSEProcess process){m_selProcess = process;}
			public override SSEProcess selProcess{get{return m_selProcess;}
			}SSEProcess m_selProcess;
		public void SetPrevSelState(SSEState state){m_prevSelState = state;}
			public override SSEState prevSelState{get{return m_prevSelState;}}
			SSEState m_prevSelState;
		public override void SetSelState(SSEState state){m_curSelState = state;}
			public override SSEState curSelState{get{return m_curSelState;}}
			SSEState m_curSelState;
	/*	Action */
		public override void SetAndRunActProcess(SSEProcess process){m_actProcess = process;}
			public override SSEProcess actProcess{get{return m_actProcess;}}
			SSEProcess m_actProcess;
		public void SetPrevActState(SSEState state){m_prevActState = state;}
			public override SSEState curActState{get{return m_curActState;}}
			SSEState m_curActState;
		public override void SetActState(SSEState state){m_curActState = state;}
			public override SSEState prevActState{get{return m_prevActState;}}
			SSEState m_prevActState;
	

	/*	Equip */
		public override void SetAndRunEqpProcess(SSEProcess process){m_eqpProcess = process;}
			public override SSEProcess eqpProcess{get{return m_eqpProcess;}}
			
			SSEProcess m_eqpProcess;
		public void SetPrevEqpState(SBEqpState state){m_prevEqpState = state;}
			public override SSEState prevEqpState{get{return m_prevEqpState;}}
			SBEqpState m_prevEqpState;
		public override void SetEqpState(SSEState state){m_curEqpState = state;}
			public override SSEState curEqpState{get{return m_curEqpState;}}
			SSEState m_curEqpState;
	/*	Mark	*/
		public override void SetAndRunMrkProcess(SSEProcess process){m_mrkProcess = process;}
			public override SSEProcess mrkProcess{get{return m_mrkProcess;}}
			SSEProcess m_mrkProcess;
		public void SetPrevMrkState(SBMrkState state){m_prevMrkState = state;}
			public override SSEState prevMrkState{get{return m_prevMrkState;}}
			SBMrkState m_prevMrkState;
		public override void SetMrkState(SSEState state){m_curMrkState = state;}
			public override SSEState curMrkState{get{return m_curMrkState;}}
			SSEState m_curMrkState;
	public override void SetSSMActState(SSMActState ssmState){m_ssmActStateSet = ssmState;}
		public SSMActState SSMActStateSet{get{return m_ssmActStateSet;}}
		SSMActState m_ssmActStateSet;
	/*	Mehtods */
		public override void Tap(){m_isTapped = true;}
			public bool isTapCalled{get{return m_isTapped;}}
			bool m_isTapped;
		public override void Reset(){m_isReset = true;}
			public bool isResetCalled{get{return m_isReset;}
			}bool m_isReset;
		public override void Focus(){m_isFocusCalled = true;}
			public bool isFocusCalled{get{return m_isFocusCalled;}
			}bool m_isFocusCalled;
		public override void Defocus(){m_isDefocusCalled = true;}
			public bool isDefocusCalled{get{return m_isDefocusCalled;}
			}bool m_isDefocusCalled;
		public override void Increment(){m_isIncrementCalled = true;}
			public bool isIncrementCalled{get{return m_isIncrementCalled;}}
			bool m_isIncrementCalled;
		public override void PickUp(){m_isPickUpCalled = true;}
			public bool isPickUpCalled{get{return m_isPickUpCalled;}}
			bool m_isPickUpCalled;
		public override void SetPickedSB(){m_isSetPickedSBCalled = true;}
			public bool isSetPickedSBCalled{get{return m_isSetPickedSBCalled;}}
			bool m_isSetPickedSBCalled;
		public override void SetDIcon1(){m_isSetDIcon1Called = true;}
			public bool isSetDIcon1Called{get{return m_isSetDIcon1Called;}}
			bool m_isSetDIcon1Called;
		public override void SetDIcon2(){m_isSetDIcon2Called = true;}
			public bool isSetDIcon2Called{get{return m_isSetDIcon2Called;}}
			bool m_isSetDIcon2Called;
		public override void CreateTAResult(){m_isCTRCalled = true;}
			public bool isCTRCalled{get{return m_isCTRCalled;}}
			bool m_isCTRCalled;
		public override void UpdateTA(){m_isUpdateTACalled = true;}
			public bool isUpdateTACalled{get{return m_isUpdateTACalled;}}
			bool m_isUpdateTACalled;
		public override void OnHoverEnterMock(){m_isOnHoverEnterCalled = true;}
			public bool isOnHoverEnterCalled{get{return m_isOnHoverEnterCalled;}}
			bool m_isOnHoverEnterCalled;
		public override void OnHoverExitMock(){m_isOnHoverExitCalled = true;}
			public bool isOnHoverExitCalled{get{return m_isOnHoverExitCalled;}}
			bool m_isOnHoverExitCalled;
		public override void ExecuteTransaction(){m_isExecuteTransactionCalled = true;}
			public bool isExecuteTransactionCalled{get{return m_isExecuteTransactionCalled;}}
			bool m_isExecuteTransactionCalled;
	
	public void ResetCallCheck(){
		m_isTapped = false;
		m_isReset = false;
		m_isFocusCalled = false;
		m_isDefocusCalled = false;
		m_isIncrementCalled = false;
		m_isPickUpCalled = false;
		m_isSetPickedSBCalled = false;
		m_isSetDIcon1Called = false;
		m_isSetDIcon2Called = false;
		m_isCTRCalled = false;
		m_isUpdateTACalled = false;
		m_isOnHoverEnterCalled = false;
		m_isOnHoverExitCalled = false;
		m_isExecuteTransactionCalled = false;
	}
	/*	Properties */
		public override bool isFocused{get{return m_isFocused;}}
			public bool m_isFocused;
			public void SetIsFocused(bool toggle){m_isFocused = toggle;}
		public override bool isPickedUp{get{return m_isPickedUp;}}
			bool m_isPickedUp;
			public void SetIsPickedUp(bool on){m_isPickedUp = on;}
		public override bool isHovered{get{return m_isHovered;}}
			bool m_isHovered;
			public void SetIsHovered(bool on){m_isHovered = on;}
		public override bool isStackable{get{return m_isStackable;}}
			bool m_isStackable;
			public void SetIsStackable(bool on){m_isStackable = on;}
		public override bool isPool{get{return m_isPool;}}
			bool m_isPool;
			public void SetIsPool(bool on){m_isPool = on;}
	
}

