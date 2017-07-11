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
		public class SBStatesTest: AbsSlotSystemTest{
			/*	SBSelStates */
				[Test]
				public void SBSelState_OnHoverEnterMock_WhenCalled_CallsSBSetHovered(){
					FakeSBSelState sbSelState = new FakeSBSelState();
					ISlottable stubSB = MakeSubSB();

					sbSelState.OnHoverEnterMock(stubSB, new PointerEventDataFake());

					stubSB.Received().SetHovered();
				}
				/*	SBDeactivatedState	*/
					[Test]
					public void SBDeactivatedState_EnterState_WhenCalled_SetsSBsSelProcToNull(){
						SBDeactivatedState deactState = new SBDeactivatedState();
						ISlottable mockSB = MakeSubSB();

						deactState.EnterState(mockSB);

						mockSB.Received().SetAndRunSelProcess(null);
					}
				/*	SBFocusedState	*/
					[Test]
					public void SBFocusedState_EnterState_FromSBDeactivated_CallsInstantGreyin(){
						SBFocusedState focState = new SBFocusedState();
						ISlottable mockSB = MakeSubSB();
						mockSB.prevSelState = Slottable.sbDeactivatedState;

						focState.EnterState(mockSB);

						mockSB.Received().InstantGreyin();
					}
					[Test]
					public void SBFocusedState_EnterState_FromSBDeactivated_SetsSelProcNull(){
						SBFocusedState focState = new SBFocusedState();
						ISlottable mockSB = MakeSubSB();
						mockSB.prevSelState = Slottable.sbDeactivatedState;

						focState.EnterState(mockSB);

						mockSB.Received().SetAndRunSelProcess(null);
					}
					[TestCaseSource(typeof(SBFocusedStateEnterStateFromNonDeactivated))]
					public void SBFocusedState_EnterState_FromNonDeactivated_SetsSelProcAccordingly<T>(SBSelState prevState, T sgProc) where T: SGSelProcess{
						SBFocusedState focState = new SBFocusedState();
						ISlottable mockSB = MakeSubSB();
						mockSB.prevSelState = prevState;

						focState.EnterState(mockSB);

						mockSB.Received().SetAndRunSelProcess(Arg.Any<T>());
					}
						class SBFocusedStateEnterStateFromNonDeactivated: IEnumerable{
							public IEnumerator GetEnumerator(){
								object[] case1 = new object[]{
									Slottable.sbDefocusedState, new SBGreyinProcess(MakeSubSB(), FakeCoroutine)
								};
								object[] case2 = new object[]{
									Slottable.sbSelectedState, new SBDehighlightProcess(MakeSubSB(), FakeCoroutine)
								};
								yield return case1;
								yield return case2;
							}
						}
				/*	SBDefocusedState	*/
					[Test]
					public void SBDefocusedState_EnterState_FromSGDeactivated_CallsInstantGreyout(){
						SBDefocusedState defocState = new SBDefocusedState();
						ISlottable mockSB = MakeSubSB();
						mockSB.prevSelState = Slottable.sbDeactivatedState;

						defocState.EnterState(mockSB);

						mockSB.Received().InstantGreyout();
					}
					[Test]
					public void SBDefocusedState_EnterState_FromSBDeactivated_SetsSelProcNull(){
						SBDefocusedState defocState = new SBDefocusedState();
						ISlottable mockSB = MakeSubSB();
						mockSB.prevSelState = Slottable.sbDeactivatedState;
						mockSB.selProcess = MakeSubSBSelProc();

						defocState.EnterState(mockSB);

						mockSB.Received().SetAndRunSelProcess(null);
					}
					[TestCaseSource(typeof(SBDefocusedStateEnterStateFromNonDeactivated))]
					public void SBDefocusedState_EnterState_FromNonDeactivated_SetsSelProcAccordingly<T>(SBSelState prevState, T selProc) where T: SBSelProcess{
						SBDefocusedState focState = new SBDefocusedState();
						ISlottable mockSB = MakeSubSB();
						mockSB.prevSelState = prevState;

						focState.EnterState(mockSB);

						mockSB.Received().SetAndRunSelProcess(Arg.Any<T>());
					}
						class SBDefocusedStateEnterStateFromNonDeactivated: IEnumerable{
							public IEnumerator GetEnumerator(){
								object[] case1 = new object[]{
									Slottable.sbFocusedState, new SBGreyoutProcess(MakeSubSB(), FakeCoroutine)
								};
								object[] case2 = new object[]{
									Slottable.sbSelectedState, new SBGreyoutProcess(MakeSubSB(), FakeCoroutine)
								};
								yield return case1;
								yield return case2;
							}
						}
				/*	SBSelectedState	*/
				[Test]
				public void SBSelectedState_EnterState_FromSGDeactivated_CallsInstantHighlight(){
					SBSelectedState defocState = new SBSelectedState();
					ISlottable mockSB = MakeSubSB();
					mockSB.prevSelState = Slottable.sbDeactivatedState;

					defocState.EnterState(mockSB);

					mockSB.Received().InstantHighlight();
				}
				[Test]
				public void SBSelectedState_EnterState_FromSBDeactivated_SetsSelProcNull(){
					SBSelectedState defocState = new SBSelectedState();
					ISlottable mockSB = MakeSubSB();
					mockSB.prevSelState = Slottable.sbDeactivatedState;

					defocState.EnterState(mockSB);

					mockSB.Received().SetAndRunSelProcess(null);
				}
				[TestCaseSource(typeof(SBSelectedStateEnterStateFromNonDeactivated))]
				public void SBSelectedState_EnterState_FromNonDeactivated_SetsSelProcAccordingly<T>(SBSelState prevState, T selProc) where T: SBSelProcess{
					SBSelectedState focState = new SBSelectedState();
					ISlottable mockSB = MakeSubSB();
					mockSB.prevSelState = prevState;

					focState.EnterState(mockSB);

					mockSB.Received().SetAndRunSelProcess(Arg.Any<T>());
				}
					class SBSelectedStateEnterStateFromNonDeactivated: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case1 = new object[]{
								Slottable.sbFocusedState, new SBHighlightProcess(MakeSubSB(), FakeCoroutine)
							};
							object[] case2 = new object[]{
								Slottable.sbDefocusedState, new SBHighlightProcess(MakeSubSB(), FakeCoroutine)
							};
							yield return case1;
							yield return case2;
						}
					}
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
						mockSB.isFocused = false;

						wfaState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						mockSB.Received().SetActState(Slottable.waitForPointerUpState);
					}
					[Test]
					public void WaitForActionState_OnPointerDown_IsFocused_SetsComplexStates(){
						WaitForActionState wfaState = new WaitForActionState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isFocused = false;

						wfaState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						mockSB.Received().SetActState(Slottable.waitForPickUpState);
						mockSB.Received().SetSelState(Slottable.sbSelectedState);
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

						Assert.That(mockSB.actProcess, Is.TypeOf(typeof(WaitForPointerUpProcess)));
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

						Assert.That(mockSB.actProcess, Is.TypeOf(typeof(WaitForNextTouchProcess)));
						mockSB.Received().SetAndRunActProcess(Arg.Any<WaitForNextTouchProcess>());
					}
					[Test]
					public void WaitForNextTouchState_OnPointerDown_IsPickedUp_SetsActStatePickedUpState(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isPickedUp = true;

						wfntState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						mockSB.Received().SetActState(Slottable.pickedUpState);
					}
					[Test]
					public void WaitForNextTouchState_OnPointerDown_IsPickedUp_CallSBIncrement(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isPickedUp = true;

						wfntState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						mockSB.Received().Increment();
					}
					[Test]
					public void WaitForNextTouchState_OnPointerDown_IsNotPickedUp_CallSBPickUp(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isPickedUp = false;

						wfntState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						mockSB.Received().PickUp();
					}
					[Test]
					public void WaitForNextTouchState_OnDeselected_IsNotPickedUp_CallSB(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isPickedUp = false;

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
						mockSB.ssm = stubSSM;

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
						mockSB.ssm = stubSSM;

						puState.EnterState(mockSB);

						mockSB.Received().SetSSMActState(SlotSystemManager.ssmProbingState);
					}
					[Test]
					public void PickedUpState_EnterState_WhenCalled_SetsActProcPickedUpProcess(){
						PickedUpState puState = new PickedUpState();
						ISlottable mockSB = MakeSubSB();
						ISlotSystemManager stubSSM = MakeSubSSM();
						mockSB.ssm = stubSSM;

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
						mockSB.isHovered = true;
						mockSB.isStackable = true;

						puState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						mockSB.Received().SetActState(Slottable.waitForNextTouchState);
					}
					[Test]
					public void PickedUpState_OnPointerUp_NOTIsHoveredAndIsStackable_CallSB(){
						PickedUpState puState = new PickedUpState();
						ISlottable mockSB = MakeSubSB();
						mockSB.isHovered = false;
						mockSB.isStackable = true;

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
					mockSB.prevEqpState = Slottable.unequippedState;
					mockSB.isPool = true;

					eqState.EnterState(mockSB);

					mockSB.Received().SetAndRunEqpProcess(Arg.Any<SBEquipProcess>());
				}
			/*	SBUnequipState	*/
				[Test]
				public void SBUnequippedState_EnterState_IsPoolAndPrevEqpStateEquipped_SetsEqpProcUnequipProc(){
					SBUnequippedState unequipeedState = new SBUnequippedState();
					ISlottable mockSB = MakeSubSB();
					mockSB.prevEqpState = Slottable.equippedState;
					mockSB.isPool = true;

					unequipeedState.EnterState(mockSB);

					mockSB.Received().SetAndRunEqpProcess(Arg.Any<SBUnequipProcess>());
				}
			/*	SBMarkedState	*/
				[Test]
				public void SBMarkedState_EnterState_IsPoolAndPrevEqpStateUnmarked_SetsMrkProcMarkProcess(){
					SBMarkedState markedState = new SBMarkedState();
					ISlottable mockSB = MakeSubSB();
					mockSB.prevMrkState = Slottable.unmarkedState;
					mockSB.isPool = true;

					markedState.EnterState(mockSB);

					mockSB.Received().SetAndRunMrkProcess(Arg.Any<SBMarkProcess>());
				}
			/*	SBUnmarkedState	*/
				[Test]
				public void SBUnmarkedState_EnterState_IsPoolAndPrevMrkStateMarked_SetsMrkProcUnmarkProcess(){
					SBUnmarkedState unmarkedState = new SBUnmarkedState();
					ISlottable mockSB = MakeSubSB();
					mockSB.prevMrkState = Slottable.markedState;
					mockSB.isPool = true;

					unmarkedState.EnterState(mockSB);

					mockSB.Received().SetAndRunMrkProcess(Arg.Any<SBUnmarkProcess>());
				}
			/*	Helper */
		}
	}
}
