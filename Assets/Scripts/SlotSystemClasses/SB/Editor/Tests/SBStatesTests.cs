using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using Utility;
using System;
namespace SlotSystemTests{
	namespace SBTests{

		[TestFixture]
		public class SBStatesTest: SlotSystemTest{
			/*	SBActStates	*/
				/*	WaitForActionState	*/
					[Test]
					public void WaitForActionState_EnterState_Always_SetsActProcNull(){
						WaitForActionState wfaState = new WaitForActionState();
						ISlottable mockSB = MakeSubSB();

						wfaState.EnterState(mockSB);

						mockSB.Received().SetAndRunActProcess(null);
					}
					[Test]
					public void WaitForActionState_OnPointerUpMock_WhenCalled_CallsSBMethods(){
						WaitForActionState wfaState = new WaitForActionState();
						ISlottable mockSB = MakeSubSB();

						wfaState.OnPointerUpMock(mockSB, new PointerEventDataFake());
						
						Received.InOrder(() => {
							mockSB.Tap();
							mockSB.Reset();
							mockSB.Defocus();
						});
					}
					[Test]
					public void WaitForActionState_OnEndDrag_WhenCalled_CallsSBMethods(){
						WaitForActionState wfaState = new WaitForActionState();
						ISlottable mockSB = MakeSubSB();

						wfaState.OnEndDragMock(mockSB, new PointerEventDataFake());

						Received.InOrder( () => {
							mockSB.Reset();
							mockSB.Defocus();
						});
					}
					[Test]
					public void WaitForActionState_OnPointerDown_IsNotFocused_SetsActStateWFPointerUpState(){
						WaitForActionState wfaState = new WaitForActionState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isFocused.Returns(false);

						wfaState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						mockSB.Received().SetActState(Slottable.waitForPointerUpState);
					}
					[Test]
					public void WaitForActionState_OnPointerDown_IsFocused_SetsComplexStates(){
						WaitForActionState wfaState = new WaitForActionState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isFocused.Returns(true);

						wfaState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						mockSB.Received().SetActState(Slottable.waitForPickUpState);
						mockSB.Received().SetSelState(mockSB.selectedState);
					}

				/*	WaitForPickUpState	*/
					[Test]
					public void WaitForPickUpState_EnterState_WhenCalled_SetsActProcWFPickUpProcess(){
						WaitForPickUpState wfpuState = new WaitForPickUpState();
						ISlottable mockSB = MakeSubSB();

						wfpuState.EnterState(mockSB);

						mockSB.Received().SetAndRunActProcess(Arg.Any<WaitForPickUpProcess>());
					}
					[Test]
					public void WaitForPickUpState_OnPointerUp_WhenCalled_SetsActStateWFNTState(){
						WaitForPickUpState wfpuState = new WaitForPickUpState();
						ISlottable mockSB = MakeSubSB();

						wfpuState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						mockSB.Received().SetActState(Slottable.waitForNextTouchState);
					}
					[Test]
					public void WaitForPickUpState_OnEndDrag_WhenCalled_CallsSBMethods(){
						WaitForPickUpState wfpuState = new WaitForPickUpState();
						ISlottable mockSB = MakeSubSB();

						wfpuState.OnEndDragMock(mockSB, new PointerEventDataFake());

						Received.InOrder(()=> {
							mockSB.Reset();
							mockSB.Focus();
						});
					}
				/*	WaitForPointerUpState	*/
					[Test]
					public void WaitForPointerUpState_EnterState_WhenCalled_SetsSBActProcWFPtUProcess(){
						WaitForPointerUpState wfptuState = new WaitForPointerUpState();
						ISlottable mockSB = MakeSubSB();

						wfptuState.EnterState(mockSB);

						mockSB.Received().SetAndRunActProcess(Arg.Any<WaitForPointerUpProcess>());
					}
					[Test]
					public void WaitForPointerUpState_OnPointerUp_WhenCalled_CallsSB(){
						WaitForPointerUpState wfptuState = new WaitForPointerUpState();
						ISlottable mockSB = MakeSubSB();

						wfptuState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						Received.InOrder(() => {
							mockSB.Tap();
							mockSB.Reset();
							mockSB.Defocus();
						});
					}
					[Test]
					public void WaitForPointerUpState_OnEndDrag_WhenCalled_CallsSB(){
						WaitForPointerUpState wfptuState = new WaitForPointerUpState();
						ISlottable mockSB = MakeSubSB();

						wfptuState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						Received.InOrder(() => {
							mockSB.Reset();
							mockSB.Defocus();
						});
					}
				/*	WaitForNextTouchState	*/
					[Test]
					public void WaitForNextTouchState_EnterState_WhenCalled_SetsSBActProcWFNTProcess(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						ISlottable mockSB = MakeSubSB();

						wfntState.EnterState(mockSB);

						mockSB.Received().SetAndRunActProcess(Arg.Any<WaitForNextTouchProcess>());
					}
					[Test]
					public void WaitForNextTouchState_OnPointerDown_IsPickedUp_SetsActStatePickedUpState(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isPickedUp.Returns(true);

						wfntState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						mockSB.Received().SetActState(Slottable.pickedUpState);
					}
					[Test]
					public void WaitForNextTouchState_OnPointerDown_IsPickedUp_CallSBIncrement(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isPickedUp.Returns(true);

						wfntState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						mockSB.Received().Increment();
					}
					[Test]
					public void WaitForNextTouchState_OnPointerDown_IsNotPickedUp_CallSBPickUp(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isPickedUp.Returns(false);

						wfntState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						mockSB.Received().PickUp();
					}
					[Test]
					public void WaitForNextTouchState_OnDeselected_IsNotPickedUp_CallSB(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isPickedUp.Returns(false);

						wfntState.OnDeselectedMock(mockSB, new PointerEventDataFake());

						Received.InOrder(() => {
							mockSB.Reset();
							mockSB.Focus();
						});
					}
				/*	PickedUpState	*/
					[Test]
					public void PickedUpState_EnterState_WhenCalled_CallSB(){
						PickedUpState puState = new PickedUpState();
						ISlottable mockSB = MakeSubSB();
						ISlotSystemManager stubSSM = MakeSubSSM();
						mockSB.ssm.Returns(stubSSM);

						puState.EnterState(mockSB);

						Received.InOrder(() => {
							mockSB.SetPickedSB();
							mockSB.SetDIcon1();
							mockSB.CreateTAResult();
							mockSB.OnHoverEnterMock();
							mockSB.UpdateTA();
						});
					}
					[Test]
					public void PickedUpState_EnterState_WhenCalled_SetsSSMActStateProbing(){
						PickedUpState puState = new PickedUpState();
						ISlottable mockSB = MakeSubSB();
						ISlotSystemManager stubSSM = MakeSubSSM();
						mockSB.ssm.Returns(stubSSM);

						puState.EnterState(mockSB);

						mockSB.Received().SetSSMActState(SlotSystemManager.ssmProbingState);
					}
					[Test]
					public void PickedUpState_EnterState_WhenCalled_SetsActProcPickedUpProcess(){
						PickedUpState puState = new PickedUpState();
						ISlottable mockSB = MakeSubSB();
						ISlotSystemManager stubSSM = MakeSubSSM();
						mockSB.ssm.Returns(stubSSM);

						puState.EnterState(mockSB);

						mockSB.Received().SetAndRunActProcess(Arg.Any<SBPickedUpProcess>());
					}
					[Test]
					public void PickedUpState_OnDeselected_WhenCalled_CallSB(){
						PickedUpState puState = new PickedUpState();
						ISlottable mockSB = MakeSubSB();

						puState.OnDeselectedMock(mockSB, new PointerEventDataFake());

						Received.InOrder(() => {
							mockSB.Reset();
							mockSB.Focus();
						});
					}
					[Test]
					public void PickedUpState_OnPointerUp_IsHoveredAndIsStackable_SetsActStateWFNTState(){
						PickedUpState puState = new PickedUpState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isHovered.Returns(true);
						mockSB.isStackable.Returns(true);

						puState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						mockSB.Received().SetActState(Slottable.waitForNextTouchState);
					}
					[Test]
					public void PickedUpState_OnPointerUp_NOTIsHoveredAndIsStackable_CallSB(){
						PickedUpState puState = new PickedUpState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isHovered.Returns(false);
						mockSB.isStackable.Returns(true);

						puState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						mockSB.Received().ExecuteTransaction();
					}
					[Test]
					public void PickedUpState_OnEndDrag_WhenCalled_CallSB(){
						PickedUpState puState = new PickedUpState();
						ISlottable mockSB = MakeSubSB();

						puState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						mockSB.Received().ExecuteTransaction();
					}
				/*	SBMoveWithinState	*/
					[Test]
					public void SBMoveWithinState_EnterState_WhenCalled_SetsActProcSBMWProcess(){
						SBMoveWithinState mwState = new SBMoveWithinState();
						ISlottable mockSB = MakeSubSB();

						mwState.EnterState(mockSB);

						mockSB.Received().SetAndRunActProcess(Arg.Any<SBMoveWithinProcess>());
					}
				/*	SBAddedState	*/
					[Test]
					public void SBAddedState_EnterState_WhenCalled_SetsActProcAddedProcess(){
						SBAddedState addedState = new SBAddedState();
						ISlottable mockSB = MakeSubSB();

						addedState.EnterState(mockSB);

						mockSB.Received().SetAndRunActProcess(Arg.Any<SBAddProcess>());
					}
				/*	SBRemovedState	*/
					[Test]
					public void SBRemovedState_EnterState_WhenCalled_SetsActProcRemovedProcess(){
						SBRemovedState remState = new SBRemovedState();
						ISlottable mockSB = MakeSubSB();

						remState.EnterState(mockSB);

						mockSB.Received().SetAndRunActProcess(Arg.Any<SBRemoveProcess>());
					}

			/*	SBEqpStates	*/
				[Test]
				public void SBEquippedState_EnterState_IsPoolAndPrevEqpStateUnequipped_SetsEqpProcEquipProc(){
					SBEquippedState eqState = new SBEquippedState();
					ISlottable mockSB = MakeSubSB();
					mockSB.prevEqpState.Returns(Slottable.unequippedState);
					mockSB.isPool.Returns(true);
					eqState.EnterState(mockSB);

					mockSB.Received().SetAndRunEqpProcess(Arg.Any<SBEquipProcess>());
				}
			/*	SBUnequipState	*/
				[Test]
				public void SBUnequippedState_EnterState_IsPoolAndPrevEqpStateEquipped_SetsEqpProcUnequipProc(){
					SBUnequippedState unequipeedState = new SBUnequippedState();
					ISlottable mockSB = MakeSubSB();
					mockSB.prevEqpState.Returns(Slottable.equippedState);
					mockSB.isPool.Returns(true);

					unequipeedState.EnterState(mockSB);

					mockSB.Received().SetAndRunEqpProcess(Arg.Any<SBUnequipProcess>());
				}
			/*	SBMarkedState	*/
				[Test]
				public void SBMarkedState_EnterState_IsPoolAndPrevEqpStateUnmarked_SetsMrkProcMarkProcess(){
					SBMarkedState markedState = new SBMarkedState();
					ISlottable mockSB = MakeSubSB();
					mockSB.prevMrkState.Returns(Slottable.unmarkedState);
					mockSB.isPool.Returns(true);

					markedState.EnterState(mockSB);

					mockSB.Received().SetAndRunMrkProcess(Arg.Any<SBMarkProcess>());
				}
			/*	SBUnmarkedState	*/
				[Test]
				public void SBUnmarkedState_EnterState_IsPoolAndPrevMrkStateMarked_SetsMrkProcUnmarkProcess(){
					SBUnmarkedState unmarkedState = new SBUnmarkedState();
					ISlottable mockSB = MakeSubSB();
					mockSB.prevMrkState.Returns(Slottable.markedState);
					mockSB.isPool.Returns(true);

					unmarkedState.EnterState(mockSB);

					mockSB.Received().SetAndRunMrkProcess(Arg.Any<SBUnmarkProcess>());
				}
			/*	Helper */
		}
	}
}
