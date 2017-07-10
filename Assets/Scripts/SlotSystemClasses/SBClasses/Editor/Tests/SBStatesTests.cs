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
				public void SBSelState_OnHoverEnterMock_WhenCalled_SetsSSMsHoveredWithSB(){
					FakeSBSelState sbSelState = new FakeSBSelState();
					FakeSB stubSB = MakeFakeSB();
					TestSSM mockSSM = MakeFakeSSM();
					stubSB.SetSSM(mockSSM);

					sbSelState.OnHoverEnterMock(stubSB, new PointerEventDataFake());

					Assert.That(mockSSM.hovered, Is.SameAs(stubSB));
				}
				/*	SBDeactivatedState	*/
					[Test]
					public void SBDeactivatedState_EnterState_WhenCalled_SetsSBsSelProcToNull(){
						SBDeactivatedState deactState = new SBDeactivatedState();
						FakeSB mockSB = MakeFakeSB();
						FakeSBSelState stubState = new FakeSBSelState();
						mockSB.SetSelState(stubState);

						deactState.EnterState(mockSB);

						Assert.That(mockSB.selProcess, Is.Null);
					}
				/*	SBFocusedState	*/
					[Test]
					public void SBFocusedState_EnterState_FromSBDeactivated_CallsInstantGreyin(){
						SBFocusedState focState = new SBFocusedState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetPrevSelState(Slottable.sbDeactivatedState);

						focState.EnterState(mockSB);

						Assert.That(mockSB.message, Is.StringContaining("InstantGreyin called"));
					}
					[Test]
					public void SBFocusedState_EnterState_FromSBDeactivated_SetsSelProcNull(){
						SBFocusedState focState = new SBFocusedState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetPrevSelState(Slottable.sbDeactivatedState);
						FakeSBselProcess stubSBSelProc = new FakeSBselProcess();
						mockSB.SetAndRunSelProcess((SSEProcess)stubSBSelProc);

						focState.EnterState(mockSB);

						Assert.That(mockSB.selProcess, Is.Null);
					}
					[TestCaseSource(typeof(SBFocusedStateEnterStateFromNonDeactivated))]
					public void SBFocusedState_EnterState_FromNonDeactivated_SetsSelProcAccordingly(SBSelState prevState, Type procType){
						SBFocusedState focState = new SBFocusedState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetPrevSelState(prevState);
						FakeSBselProcess stubSBSelProc = new FakeSBselProcess();
						mockSB.SetAndRunSelProcess((SSEProcess)stubSBSelProc);

						focState.EnterState(mockSB);

						Assert.That(mockSB.selProcess, Is.TypeOf(procType));

					}
						class SBFocusedStateEnterStateFromNonDeactivated: IEnumerable{
							public IEnumerator GetEnumerator(){
								object[] case1 = new object[]{
									Slottable.sbDefocusedState, typeof(SBGreyinProcess)
								};
								object[] case2 = new object[]{
									Slottable.sbSelectedState, typeof(SBDehighlightProcess)
								};
								yield return case1;
								yield return case2;
							}
						}
				/*	SBDefocusedState	*/
					[Test]
					public void SBDefocusedState_EnterState_FromSGDeactivated_CallsInstantGreyout(){
						SBDefocusedState defocState = new SBDefocusedState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetPrevSelState(Slottable.sbDeactivatedState);

						defocState.EnterState(mockSB);

						Assert.That(mockSB.message, Is.StringContaining("InstantGreyout called"));
					}
					[Test]
					public void SBDefocusedState_EnterState_FromSBDeactivated_SetsSelProcNull(){
						SBDefocusedState defocState = new SBDefocusedState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetPrevSelState(Slottable.sbDeactivatedState);
						FakeSBselProcess stubSBSelProc = new FakeSBselProcess();
						mockSB.SetAndRunSelProcess((SSEProcess)stubSBSelProc);

						defocState.EnterState(mockSB);

						Assert.That(mockSB.selProcess, Is.Null);
					}
					[TestCaseSource(typeof(SBDefocusedStateEnterStateFromNonDeactivated))]
					public void SBDefocusedState_EnterState_FromNonDeactivated_SetsSelProcAccordingly(SBSelState prevState, Type procType){
						SBDefocusedState focState = new SBDefocusedState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetPrevSelState(prevState);
						FakeSBselProcess stubSBSelProc = new FakeSBselProcess();
						mockSB.SetAndRunSelProcess((SSEProcess)stubSBSelProc);

						focState.EnterState(mockSB);

						Assert.That(mockSB.selProcess, Is.TypeOf(procType));

					}
						class SBDefocusedStateEnterStateFromNonDeactivated: IEnumerable{
							public IEnumerator GetEnumerator(){
								object[] case1 = new object[]{
									Slottable.sbFocusedState, typeof(SBGreyoutProcess)
								};
								object[] case2 = new object[]{
									Slottable.sbSelectedState, typeof(SBGreyoutProcess)
								};
								yield return case1;
								yield return case2;
							}
						}
				/*	SBSelectedState	*/
				[Test]
				public void SBSelectedState_EnterState_FromSGDeactivated_CallsInstantHighlight(){
					SBSelectedState defocState = new SBSelectedState();
					FakeSB mockSB = MakeFakeSB();
					mockSB.SetPrevSelState(Slottable.sbDeactivatedState);

					defocState.EnterState(mockSB);

					Assert.That(mockSB.message, Is.StringContaining("InstantHighlight called"));
				}
				[Test]
				public void SBSelectedState_EnterState_FromSBDeactivated_SetsSelProcNull(){
					SBSelectedState defocState = new SBSelectedState();
					FakeSB mockSB = MakeFakeSB();
					mockSB.SetPrevSelState(Slottable.sbDeactivatedState);
					FakeSBselProcess stubSBSelProc = new FakeSBselProcess();
					mockSB.SetAndRunSelProcess((SSEProcess)stubSBSelProc);

					defocState.EnterState(mockSB);

					Assert.That(mockSB.selProcess, Is.Null);
				}
				[TestCaseSource(typeof(SBSelectedStateEnterStateFromNonDeactivated))]
				public void SBSelectedState_EnterState_FromNonDeactivated_SetsSelProcAccordingly(SBSelState prevState, Type procType){
					SBSelectedState focState = new SBSelectedState();
					FakeSB mockSB = MakeFakeSB();
					mockSB.SetPrevSelState(prevState);
					FakeSBselProcess stubSBSelProc = new FakeSBselProcess();
					mockSB.SetAndRunSelProcess((SSEProcess)stubSBSelProc);

					focState.EnterState(mockSB);

					Assert.That(mockSB.selProcess, Is.TypeOf(procType));

				}
					class SBSelectedStateEnterStateFromNonDeactivated: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case1 = new object[]{
								Slottable.sbFocusedState, typeof(SBHighlightProcess)
							};
							object[] case2 = new object[]{
								Slottable.sbDefocusedState, typeof(SBHighlightProcess)
							};
							yield return case1;
							yield return case2;
						}
					}
			/*	SBActStates	*/
				/*	WaitForActionState	*/
					[Test]
					public void WaitForActionState_EnterState_Always_SetsSelProcNull(){
						WaitForActionState wfaState = new WaitForActionState();
						FakeSB mockSB = MakeFakeSB();
						FakeSBactProcess stubActProc = new FakeSBactProcess();
						mockSB.SetAndRunActProcess(stubActProc);

						wfaState.EnterState(mockSB);

						Assert.That(mockSB.actProcess, Is.Null);
					}
					[Test]
					public void WaitForActionState_OnPointerUpMock_WhenCalled_CallsSBMethods(){
						WaitForActionState wfaState = new WaitForActionState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.ResetCallCheck();

						wfaState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.isTapCalled, Is.True);
						Assert.That(mockSB.isResetCalled, Is.True);
						Assert.That(mockSB.isDefocusCalled, Is.True);
						
						mockSB.ResetCallCheck();
					}
					[Test]
					public void WaitForActionState_OnEndDrag_WhenCalled_CallsSBMethods(){
						WaitForActionState wfaState = new WaitForActionState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.ResetCallCheck();

						wfaState.OnEndDragMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.isResetCalled, Is.True);
						Assert.That(mockSB.isDefocusCalled, Is.True);

						mockSB.ResetCallCheck();
					}
					[Test]
					public void WaitForActionState_OnPointerDown_IsNotFocused_SetsActStateWFPointerUpState(){
						WaitForActionState wfaState = new WaitForActionState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetIsFocused(false);

						wfaState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.curActState, Is.SameAs(Slottable.waitForPointerUpState));
					}
					[Test]
					public void WaitForActionState_OnPointerDown_IsFocused_SetsComplexStates(){
						WaitForActionState wfaState = new WaitForActionState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetIsFocused(true);

						wfaState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.curActState, Is.SameAs(Slottable.waitForPickUpState));
						Assert.That(mockSB.curSelState, Is.SameAs(Slottable.sbSelectedState));
					}

				/*	WaitForPickUpState	*/
					[Test]
					public void WaitForPickUpState_EnterState_WhenCalled_SetsActProcWFPickUpProcess(){
						WaitForPickUpState wfpuState = new WaitForPickUpState();
						FakeSB mockSB = MakeFakeSB();

						wfpuState.EnterState(mockSB);

						Assert.That(mockSB.actProcess, Is.TypeOf(typeof(WaitForPickUpProcess)));
					}
					[Test]
					public void WaitForPickUpState_OnPointerUp_WhenCalled_SetsActStateWFNTState(){
						WaitForPickUpState wfpuState = new WaitForPickUpState();
						FakeSB mockSB = MakeFakeSB();

						wfpuState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.curActState, Is.SameAs(Slottable.waitForNextTouchState));
					}
					[Test]
					public void WaitForPickUpState_OnEndDrag_WhenCalled_CallsSBMethods(){
						WaitForPickUpState wfpuState = new WaitForPickUpState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.ResetCallCheck();

						wfpuState.OnEndDragMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.isResetCalled, Is.True);
						Assert.That(mockSB.isFocusCalled, Is.True);

						mockSB.ResetCallCheck();
					}
				/*	WaitForPointerUpState	*/
					[Test]
					public void WaitForPointerUpState_EnterState_WhenCalled_SetsSBActProcWFPtUProcess(){
						WaitForPointerUpState wfptuState = new WaitForPointerUpState();
						FakeSB mockSB = MakeFakeSB();

						wfptuState.EnterState(mockSB);

						Assert.That(mockSB.actProcess, Is.TypeOf(typeof(WaitForPointerUpProcess)));
					}
					[Test]
					public void WaitForPointerUpState_OnPointerUp_WhenCalled_CallsSB(){
						WaitForPointerUpState wfptuState = new WaitForPointerUpState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.ResetCallCheck();

						wfptuState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.isTapCalled, Is.True);
						Assert.That(mockSB.isResetCalled, Is.True);
						Assert.That(mockSB.isDefocusCalled, Is.True);

						mockSB.ResetCallCheck();
					}
					[Test]
					public void WaitForPointerUpState_OnEndDrag_WhenCalled_CallsSB(){
						WaitForPointerUpState wfptuState = new WaitForPointerUpState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.ResetCallCheck();

						wfptuState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.isResetCalled, Is.True);
						Assert.That(mockSB.isDefocusCalled, Is.True);

						mockSB.ResetCallCheck();
					}
				/*	WaitForNextTouchState	*/
					[Test]
					public void WaitForNextTouchState_EnterState_WhenCalled_SetsSBActProcWFNTProcess(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						FakeSB mockSB = MakeFakeSB();

						wfntState.EnterState(mockSB);

						Assert.That(mockSB.actProcess, Is.TypeOf(typeof(WaitForNextTouchProcess)));
					}
					[Test]
					public void WaitForNextTouchState_OnPointerDown_IsPickedUp_SetsActStatePickedUpState(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetIsPickedUp(true);

						wfntState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.curActState, Is.SameAs(Slottable.pickedUpState));
					}
					[Test]
					public void WaitForNextTouchState_OnPointerDown_IsPickedUp_CallSBIncrement(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetIsPickedUp(true);
						mockSB.ResetCallCheck();

						wfntState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.isIncrementCalled, Is.True);
						
						mockSB.ResetCallCheck();
					}
					[Test]
					public void WaitForNextTouchState_OnPointerDown_IsNotPickedUp_CallSBPickUp(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetIsPickedUp(false);
						mockSB.ResetCallCheck();

						wfntState.OnPointerDownMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.isPickUpCalled, Is.True);
						
						mockSB.ResetCallCheck();
					}
					[Test]
					public void WaitForNextTouchState_OnDeselected_WhenCalled_CallSB(){
						WaitForNextTouchState wfntState = new WaitForNextTouchState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetIsPickedUp(false);
						mockSB.ResetCallCheck();

						wfntState.OnDeselectedMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.isResetCalled, Is.True);
						Assert.That(mockSB.isFocusCalled, Is.True);
						
						mockSB.ResetCallCheck();
					}
				/*	PickedUpState	*/
					[Test]
					public void PickedUpState_EnterState_WhenCalled_CallSB(){
						PickedUpState puState = new PickedUpState();
						FakeSB mockSB = MakeFakeSB();
						TestSSM stubSSM = MakeFakeSSM();
						mockSB.SetSSM(stubSSM);
						mockSB.ResetCallCheck();

						puState.EnterState(mockSB);

						IEnumerable<bool> callChecksNoArg = new bool[]{
							mockSB.isSetPickedSBCalled,
							mockSB.isSetDIcon1Called,
							mockSB.isCTRCalled,
							mockSB.isOnHoverEnterCalled,
							mockSB.isUpdateTACalled
						};
						Assert.That(callChecksNoArg, Is.All.True);
						
						mockSB.ResetCallCheck();
					}
					[Test]
					public void PickedUpState_EnterState_WhenCalled_SetsSSMActStateProbing(){
						PickedUpState puState = new PickedUpState();
						FakeSB mockSB = MakeFakeSB();
						TestSSM stubSSM = MakeFakeSSM();
						mockSB.SetSSM(stubSSM);

						puState.EnterState(mockSB);

						Assert.That(mockSB.SSMActStateSet, Is.SameAs(SlotSystemManager.ssmProbingState));
					}
					[Test]
					public void PickedUpState_EnterState_WhenCalled_SetsActProcPickedUpProcess(){
						PickedUpState puState = new PickedUpState();
						FakeSB mockSB = MakeFakeSB();
						TestSSM stubSSM = MakeFakeSSM();
						mockSB.SetSSM(stubSSM);

						puState.EnterState(mockSB);

						Assert.That(mockSB.actProcess, Is.TypeOf(typeof(SBPickedUpProcess)));
					}
					[Test]
					public void PickedUpState_OnDeselected_WhenCalled_CallSB(){
						PickedUpState puState = new PickedUpState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.ResetCallCheck();

						puState.OnDeselectedMock(mockSB, new PointerEventDataFake());

						IEnumerable<bool> checkCalls = new bool[]{
							mockSB.isResetCalled,
							mockSB.isFocusCalled
						};

						Assert.That(checkCalls, Is.All.True);
						
						mockSB.ResetCallCheck();
					}
					[Test]
					public void PickedUpState_OnPointerUp_IsHoveredAndIsStackable_SetsActStateWFNTState(){
						PickedUpState puState = new PickedUpState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetIsHovered(true);
						mockSB.SetIsStackable(true);

						puState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.curActState, Is.SameAs(Slottable.waitForNextTouchState));
					}
					[Test]
					public void PickedUpState_OnPointerUp_NOTIsHoveredAndIsStackable_CallSB(){
						PickedUpState puState = new PickedUpState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.SetIsHovered(false);
						mockSB.SetIsStackable(true);
						mockSB.ResetCallCheck();

						puState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.isExecuteTransactionCalled, Is.True);
						
						mockSB.ResetCallCheck();
					}
					[Test]
					public void PickedUpState_OnEndDrag_WhenCalled_CallSB(){
						PickedUpState puState = new PickedUpState();
						FakeSB mockSB = MakeFakeSB();
						mockSB.ResetCallCheck();

						puState.OnPointerUpMock(mockSB, new PointerEventDataFake());

						Assert.That(mockSB.isExecuteTransactionCalled, Is.True);
						
						mockSB.ResetCallCheck();
					}
				/*	SBMoveWithinState	*/
					[Test]
					public void SBMoveWithinState_EnterState_WhenCalled_SetsActProcSBMWProcess(){
						SBMoveWithinState mwState = new SBMoveWithinState();
						FakeSB mockSB = MakeFakeSB();

						mwState.EnterState(mockSB);

						Assert.That(mockSB.actProcess, Is.TypeOf(typeof(SBMoveWithinProcess)));
					}
				/*	SBAddedState	*/
					[Test]
					public void SBAddedState_EnterState_WhenCalled_SetsActProcAddedProcess(){
						SBAddedState addedState = new SBAddedState();
						FakeSB mockSB = MakeFakeSB();

						addedState.EnterState(mockSB);

						Assert.That(mockSB.actProcess, Is.TypeOf(typeof(SBAddProcess)));
					}
				/*	SBRemovedState	*/
					[Test]
					public void SBRemovedState_EnterState_WhenCalled_SetsActProcRemovedProcess(){
						SBRemovedState remState = new SBRemovedState();
						FakeSB mockSB = MakeFakeSB();

						remState.EnterState(mockSB);

						Assert.That(mockSB.actProcess, Is.TypeOf(typeof(SBRemoveProcess)));
					}

			/*	SBEqpStates	*/
				[Test]
				public void SBEquippedState_EnterState_IsPoolAndPrevEqpStateUnequipped_SetsEqpProcEquipProc(){
					SBEquippedState eqState = new SBEquippedState();
					FakeSB mockSB = MakeFakeSB();
					mockSB.SetPrevEqpState(Slottable.unequippedState);
					mockSB.SetIsPool(true);

					eqState.EnterState(mockSB);

					Assert.That(mockSB.eqpProcess, Is.TypeOf(typeof(SBEquipProcess)));
				}
			/*	SBUnequipState	*/
				[Test]
				public void SBUnequippedState_EnterState_IsPoolAndPrevEqpStateEquipped_SetsEqpProcUnequipProc(){
					SBUnequippedState unequipeedState = new SBUnequippedState();
					FakeSB mockSB = MakeFakeSB();
					mockSB.SetPrevEqpState(Slottable.equippedState);
					mockSB.SetIsPool(true);

					unequipeedState.EnterState(mockSB);

					Assert.That(mockSB.eqpProcess, Is.TypeOf(typeof(SBUnequipProcess)));
				}
			/*	SBMarkedState	*/
				[Test]
				public void SBMarkedState_EnterState_IsPoolAndPrevEqpStateUnmarked_SetsMrkProcMarkProcess(){
					SBMarkedState markedState = new SBMarkedState();
					FakeSB mockSB = MakeFakeSB();
					mockSB.SetPrevMrkState(Slottable.unmarkedState);
					mockSB.SetIsPool(true);

					markedState.EnterState(mockSB);

					Assert.That(mockSB.mrkProcess, Is.TypeOf(typeof(SBMarkProcess)));
				}
			/*	SBUnmarkedState	*/
				[Test]
				public void SBUnmarkedState_EnterState_IsPoolAndPrevMrkStateMarked_SetsMrkProcUnmarkProcess(){
					SBUnmarkedState unmarkedState = new SBUnmarkedState();
					FakeSB mockSB = MakeFakeSB();
					mockSB.SetPrevMrkState(Slottable.markedState);
					mockSB.SetIsPool(true);

					unmarkedState.EnterState(mockSB);

					Assert.That(mockSB.mrkProcess, Is.TypeOf(typeof(SBUnmarkProcess)));
				}
			/*	Helper */
				class FakeSBSelState: SBSelState{}
				class FakeSSEState: SSEState{}
				class FakeSBProcess: SBProcess{}
				class FakeSBselProcess: SBSelProcess{}
				class FakeSBactProcess: SBActProcess{}
				
		}
	}
}
