using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using SlotSystem;
using Utility;
namespace SlotSystemTests{
	[TestFixture]
	[Category("Integration")]
	public class SlottableIntegrationTests : SlotSystemTest{
		/* ActStates */
				[Test]
				public void areActStatesNull_ByDefault_IsTrue(){
					Slottable sb = MakeSB();
					Assert.That(sb.isCurActStateNull, Is.True);
					Assert.That(sb.isPrevActStateNull, Is.True);
					Assert.That(sb.isCurSelStateNull, Is.True);
					Assert.That(sb.isPrevSelStateNull, Is.True);
					Assert.That(sb.isCurEqpStateNull, Is.True);
					Assert.That(sb.isPrevEqpStateNull, Is.True);
					Assert.That(sb.isCurMrkStateNull, Is.True);
					Assert.That(sb.isPrevMrkStateNull, Is.True);
				}
				[Test]
				public void processes_ByDefault_isNull(){
					Slottable sb = MakeSB();

					Assert.That(sb.selProcess, Is.Null);
					Assert.That(sb.actProcess, Is.Null);
					Assert.That(sb.eqpProcess, Is.Null);
					Assert.That(sb.mrkProcess, Is.Null);
				}
			/* WaitForActionState */
				[Test]
				public void WaitForAction_Always_SetsIsWaitingForAction(){
					Slottable sb = MakeSB();
					
					sb.WaitForAction();

					Assert.That(sb.isWaitingForAction, Is.True);
					}
				[Test]
				public void WaitForAction_IsPrevActStateNull_DoesNotCallActEngineSetProcNull(){
					Slottable sb = MakeSB();
						ISSEProcessEngine<ISBActProcess> mockProcEngine = Substitute.For<ISSEProcessEngine<ISBActProcess>>();
						sb.SetActProcessEngine(mockProcEngine);
					sb.WaitForAction();


					mockProcEngine.DidNotReceive().SetAndRunProcess((ISBActProcess)null);
					}
				[Test]
				public void WaitForAction_WasWaitingForAction_DoesNotCallActEngineSetProcNull(){
					Slottable sb = MakeSB();
						ISSEProcessEngine<ISBActProcess> mockProcEngine = Substitute.For<ISSEProcessEngine<ISBActProcess>>();
						sb.SetActProcessEngine(mockProcEngine);
					sb.WaitForAction();

					sb.WaitForAction();

					mockProcEngine.DidNotReceive().SetAndRunProcess((ISBActProcess)null);
					}
				[Test]
				public void WaitForAction_IsNotActStateInitAndWasNotWFAction_CallsActEngineSetProcNull(){
					Slottable sb = MakeSB();
						ISSEProcessEngine<ISBActProcess> mockEngine = Substitute.For<ISSEProcessEngine<ISBActProcess>>();
						sb.SetActProcessEngine(mockEngine);
					sb.Add();

					sb.WaitForAction();

					mockEngine.Received().SetAndRunProcess((ISBActProcess)null);
					}
				[Test]
				public void InWaitForAction_OnPointerDown_IsFocused_SetsIsSelected(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.Focus();

					sb.OnPointerDown(new PointerEventDataFake());

					Assert.That(sb.isSelected, Is.True);
					}
				[Test]
				public void InWaitForAction_OnPointerDown_IsFocused_SetsIsWFPickUp(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.Focus();

					sb.OnPointerDown(new PointerEventDataFake());

					Assert.That(sb.isWaitingForPickUp, Is.True);
					}
				[Test]
				public void InWaitForAction_OnPointerDown_IsNotFocused_SetsIsWFPointerUp(){
					Slottable sb = MakeSB();
					sb.WaitForAction();

					sb.OnPointerDown(new PointerEventDataFake());

					Assert.That(sb.isWaitingForPointerUp, Is.True);
					}
			/* WaitForPickUp */
				[Test][ExpectedException(typeof(InvalidOperationException))]
				public void WaitForPickUp_WasNotWaitForAction_ThrowsException(){
					Slottable sb = MakeSB();

					sb.WaitForPickUp();
				}
				[Test]
				public void WaitForPickUp_WasWaitingForAction_SetsIsWaitingForPickUp(){
					Slottable sb = MakeSB();
					sb.WaitForAction();

					sb.WaitForPickUp();

					Assert.That(sb.isWaitingForPickUp, Is.True);
					}
				[Test]
				public void WaitForPickUp_WasWaitingForAction_SetsAndRunsWFPickUpProc(){
					Slottable sb = MakeSB();
						System.Func<IEnumeratorFake> mockCor = Substitute.For<System.Func<IEnumeratorFake>>();
						ISBCoroutineFactory mockCorFac = Substitute.For<ISBCoroutineFactory>();
						mockCorFac.MakeWaitForPickUpCoroutine().Returns(mockCor);
						sb.SetCoroutineFactory(mockCorFac);
					sb.WaitForAction();

					sb.WaitForPickUp();

					ISBActProcess actProc = sb.actProcess;
					Assert.That(actProc, Is.TypeOf(typeof(WaitForPickUpProcess)));
					Assert.That(actProc.sse, Is.SameAs(sb));
					mockCor.Received().Invoke();
					Assert.That(actProc.isRunning, Is.True);
					}
				[Test]
				public void ExpireProcess_WaitingForPickUpProcess_SetsIsPickingUp(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.WaitForPickUp();

					sb.ExpireActProcess();

					Assert.That(sb.isPickingUp, Is.True);
					}
				[Test]
				public void ExpireProcess_WaitingForPickUpProcess_SetsPickedAmount1(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.WaitForPickUp();

					sb.ExpireActProcess();

					Assert.That(sb.pickedAmount, Is.EqualTo(1));
					}
				[Test]
				public void InWaitForPickUp_OnPointerUp_Always_SetsIsWaitingForNextTouch(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.WaitForPickUp();

					sb.OnPointerUp(new PointerEventDataFake());

					Assert.That(sb.isWaitingForNextTouch, Is.True);
					}
				[Test]
				public void InWaitForPickUp_OnEndDrag_Always_CallsRefresh(){
					Slottable sb = MakeSB();
						SetUpForRefreshCall(sb);
					sb.WaitForAction();
					sb.WaitForPickUp();

					sb.OnEndDrag(new PointerEventDataFake());

					AssertRefreshCalled(sb);
					}
				[Test]
				public void InWaitForPickUp_OnEndDrag_Always_SetsIsFocused(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.WaitForPickUp();

					sb.OnEndDrag(new PointerEventDataFake());

					Assert.That(sb.isFocused, Is.True);
					}
			/* WaitForPointerUpState */
				[Test][ExpectedException(typeof(InvalidOperationException))]
				public void WaitForPointerUp_WasNotWaitingForAction_ThrowsException(){
					Slottable sb = MakeSB();

					sb.WaitForPointerUp();
				}
				[Test]
				public void WaitForPointerUp_WasWaitingForAction_SetsIsWaitingForPointerUp(){
					Slottable sb = MakeSB();
					sb.WaitForAction();

					sb.WaitForPointerUp();

					Assert.That(sb.isWaitingForPointerUp, Is.True);
					}
				[Test]
				public void WaitForPointerUp_WasWaitingForAction_SetsAndRunsWaitForPointerUpProcess(){
					Slottable sb = MakeSB();
						System.Func<IEnumeratorFake> mockCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
						ISBCoroutineFactory mockCorFac = Substitute.For<ISBCoroutineFactory>();
						mockCorFac.MakeWaitForPointerUpCoroutine().Returns(mockCoroutine);
						sb.SetCoroutineFactory(mockCorFac);
					sb.WaitForAction();
					
					sb.WaitForPointerUp();

					ISBActProcess actualProc = sb.actProcess;
					Assert.That(actualProc, Is.TypeOf(typeof(WaitForPointerUpProcess)));
					Assert.That(actualProc.sse, Is.SameAs(sb));
					mockCoroutine.Received().Invoke();
					Assert.That(actualProc.isRunning, Is.True);
					}
				[Test]
				public void ExpireActPorcess_WaitForPointerUpProcess_SetIsDefocused(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.WaitForPointerUp();

					sb.ExpireActProcess();

					Assert.That(sb.isDefocused, Is.True);
					}
				[Test]
				public void InWaitForPointerUp_OnPointerUp_Always_CallsTapCommandExecute(){
					Slottable sb = MakeSB();
						SlottableCommand mockTapComm = Substitute.For<SlottableCommand>();
						sb.SetTapCommand(mockTapComm);
					sb.WaitForAction();
					sb.WaitForPointerUp();

					sb.OnPointerUp(new PointerEventDataFake());

					mockTapComm.Received().Execute(sb);
					}
				[Test]
				public void InWaitForPointerUp_OnPointerUp_Always_CallsRefresh(){
					Slottable sb = MakeSB();
						SetUpForRefreshCall(sb);
					sb.WaitForAction();
					sb.WaitForPointerUp();

					sb.OnPointerUp(new PointerEventDataFake());

					AssertRefreshCalled(sb);
					}
				[Test]
				public void InWaitForPointerUp_OnPointerUp_Always_SetsIsDefocused(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.WaitForPointerUp();

					sb.OnPointerUp(new PointerEventDataFake());

					Assert.That(sb.isDefocused, Is.True);
					}
				[Test]
				public void InWaitForPointerUp_OnEndDrag_Always_CallsRefresh(){
					Slottable sb = MakeSB();
						SetUpForRefreshCall(sb);
					sb.WaitForAction();
					sb.WaitForPointerUp();

					sb.OnEndDrag(new PointerEventDataFake());

					AssertRefreshCalled(sb);
					}
				[Test]
				public void InWaitForPointerUp_OnEndDrag_Always_SetsIsDefocused(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.WaitForPointerUp();

					sb.OnEndDrag(new PointerEventDataFake());

					Assert.That(sb.isDefocused, Is.True);
					}
			/* WaitForNextTouchState */
				[Test][ExpectedException(typeof(InvalidOperationException))]
				public void WaitForNextTouch_WasNotPickingUpNorWaitingForPickUp_ThrowsException(){
					Slottable sb = MakeSB();

					sb.WaitForNextTouch();
				}
				[Test]
				public void WaitForNextTouch_WasPickingUp_SetsIsWaitingForNextTouch(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.PickUp();
					
					sb.WaitForNextTouch();

					Assert.That(sb.isWaitingForNextTouch, Is.True);
					}
				[Test]
				public void WaitForNextTouch_WasWaitingForPickUp_SetsIsWaitingForNextTouch(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.WaitForPickUp();

					sb.WaitForNextTouch();

					Assert.That(sb.isWaitingForNextTouch, Is.True);
					}
				[Test]
				public void WaitForNextTouch_WasPickingUp_SetsAndRunsWaitForNextTouchProcess(){
					Slottable sb = MakeSB();
						System.Func<IEnumeratorFake> mockCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
						ISBCoroutineFactory stubCorFac = Substitute.For<ISBCoroutineFactory>();
						stubCorFac.MakeWaitForNextTouchCoroutine().Returns(mockCoroutine);
						sb.SetCoroutineFactory(stubCorFac);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.PickUp();
					
					sb.WaitForNextTouch();

					ISBActProcess actualProc = sb.actProcess;
					Assert.That(actualProc, Is.TypeOf(typeof(WaitForNextTouchProcess)));
					Assert.That(actualProc.sse, Is.SameAs(sb));
					mockCoroutine.Received().Invoke();
					Assert.That(actualProc.isRunning, Is.True);
					}
				[Test]
				public void WaitForNextTouch_WasWaitingForPickUp_SetsAndRunsWaitForNextTouchProcess(){
					Slottable sb = MakeSB();
						System.Func<IEnumeratorFake> mockCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
						ISBCoroutineFactory stubCorFac = Substitute.For<ISBCoroutineFactory>();
						stubCorFac.MakeWaitForNextTouchCoroutine().Returns(mockCoroutine);
						sb.SetCoroutineFactory(stubCorFac);
					sb.WaitForAction();
					sb.WaitForPickUp();
					
					sb.WaitForNextTouch();

					ISBActProcess actualProc = sb.actProcess;
					Assert.That(actualProc, Is.TypeOf(typeof(WaitForNextTouchProcess)));
					Assert.That(actualProc.sse, Is.SameAs(sb));
					mockCoroutine.Received().Invoke();
					Assert.That(actualProc.isRunning, Is.True);
					}
				[Test]
				public void ExpireActProcess_WaitForNextTouchProcess_IsNotPickedUp_CallsTapCommandExecute(){
					Slottable sb = MakeSB();
						SlottableCommand mockComm = Substitute.For<SlottableCommand>();
						sb.SetTapCommand(mockComm);
						ISlotSystemManager mockSSM = MakeSubSSM();
						mockSSM.pickedSB.Returns((ISlottable)null);
						sb.SetSSM(mockSSM);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.WaitForNextTouch();

					sb.ExpireActProcess();

					mockComm.Received().Execute(sb);
					}
				[Test]
				public void ExpireActProcess_WaitForNextTouchProcess_IsNotPickedUp_CallsRefresh(){
					Slottable sb = MakeSB();
						SetUpForRefreshCall(sb);
						ISlotSystemManager mockSSM = MakeSubSSM();
						mockSSM.pickedSB.Returns((ISlottable)null);
						sb.SetSSM(mockSSM);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.WaitForNextTouch();

					sb.ExpireActProcess();

					AssertRefreshCalled(sb);
					}
				[Test]
				public void ExpireActProcess_WaitForNextTouchProcess_IsNotPickedUp_SetsIsFocused(){
					Slottable sb = MakeSB();
						ISlotSystemManager mockSSM = MakeSubSSM();
						mockSSM.pickedSB.Returns((ISlottable)null);
						sb.SetSSM(mockSSM);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.WaitForNextTouch();

					sb.ExpireActProcess();

					Assert.That(sb.isFocused, Is.True);
					}
				[Test]
				public void ExpireActProcess_WaitForNextTouchProcess_IsPickedUp_CallsSSMExecuteTransaction(){
					Slottable sb = MakeSB();
						ISlotSystemManager mockSSM = MakeSubSSM();
						mockSSM.pickedSB.Returns(sb);
						sb.SetSSM(mockSSM);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.WaitForNextTouch();

					sb.ExpireActProcess();

					mockSSM.Received().ExecuteTransaction();
					}
				[Test]
				public void InWaitForNextTouch_OnPointerDown_IsNotPickedUp_SetsIsPickingUp(){
					Slottable sb = MakeSB();
						ISlotSystemManager ssm = MakeSubSSM();
						ssm.pickedSB.Returns((ISlottable)null);
						sb.SetSSM(ssm);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.WaitForNextTouch();

					sb.OnPointerDown(new PointerEventDataFake());

					Assert.That(sb.isPickingUp, Is.True);
					}
				[Test]
				public void InWaitForNextTouch_OnPointerDown_IsNotPickedUp_SetsPickedAmount1(){
					Slottable sb = MakeSB();
						ISlotSystemManager ssm = MakeSubSSM();
						ssm.pickedSB.Returns((ISlottable)null);
						sb.SetSSM(ssm);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.WaitForNextTouch();

					sb.OnPointerDown(new PointerEventDataFake());

					Assert.That(sb.pickedAmount, Is.EqualTo(1));
					}
				[Test]
				public void InWaitForNextTouch_OnPointerDown_IsPickedUp_SetsIsPickingUp(){
					Slottable sb = MakeSB();
						ISlotSystemManager ssm = MakeSubSSM();
						ssm.pickedSB.Returns(sb);
						sb.SetSSM(ssm);
						BowInstance bow = MakeBowInstance(0);
						sb.SetItem(bow);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.WaitForNextTouch();
					
					sb.OnPointerDown(new PointerEventDataFake());

					Assert.That(sb.isPickingUp, Is.True);
					}
				[Test]
				public void InWaitForNextTouch_OnPointerDown_IsPickedUpIsStackableQuantityGrearterThanPickedAmount_IncrementsPickedAmount(){
					Slottable sb = MakeSB();
						ISlotSystemManager ssm = MakeSubSSM();
						ssm.pickedSB.Returns(sb);
						sb.SetSSM(ssm);
						PartsInstance parts = MakePartsInstance(0, 10);
						sb.SetItem(parts);
						sb.pickedAmount = 8;
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.WaitForNextTouch();

					sb.OnPointerDown(new PointerEventDataFake());

					Assert.That(sb.pickedAmount, Is.EqualTo(9));
					}
				[Test]
				public void InWaitForNextTouch_OnDeselect_Always_CallsRefresh(){
					Slottable sb = MakeSB();
						SetUpForRefreshCall(sb);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.WaitForNextTouch();

					sb.OnDeselected(new PointerEventDataFake());

					AssertRefreshCalled(sb);
					}
				[Test]
				public void InWaitForNextTouch_OnDeselect_Always_SetsIsFocused(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.WaitForNextTouch();

					sb.OnDeselected(new PointerEventDataFake());

					Assert.That(sb.isFocused, Is.True);
					}
			/* PickingUpState */
				[Test][ExpectedException(typeof(InvalidOperationException))]
				public void PickUp_WasNotWaitingForPickUpNorWaitingForNextTouch_ThrowsException(){
					Slottable sb = MakeSB();

					sb.PickUp();
				}
				[Test]
				public void PickUp_WasWaitingForPickUp_SetsIsPickingUp(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.WaitForPickUp();
					
					sb.PickUp();

					Assert.That(sb.isPickingUp, Is.True);
					}
				[Test]
				public void PickUp_WasWaitingForNextTouch_SetsIsPickingUp(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.WaitForNextTouch();
					
					sb.PickUp();

					Assert.That(sb.isPickingUp, Is.True);
					}
				[Test]
				public void PickUp_IsNotCurSelStateNullAndValidPrevActState_CallsOnHoverEnterCommandExecute(){
					Slottable sb = MakeSB();
						ISSECommand mockComm = Substitute.For<ISSECommand>();
						sb.SetOnHoverEnterCommand(mockComm);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.Deactivate();

					sb.PickUp();

					mockComm.Received().Execute();
					}
				[Test]
				public void PickUp_SSMNotNullAndValidPrevActState_CallsSSMInSequence(){
					Slottable sb = MakeSB();
						ISlotSystemManager ssm = MakeSubSSM();
						sb.SetSSM(ssm);
					sb.WaitForAction();
					sb.WaitForPickUp();
					
					sb.PickUp();

					Received.InOrder(()=> {
						ssm.SetPickedSB(sb);
						ssm.Probe();
						ssm.SetDIcon1(Arg.Any<DraggedIcon>());
						ssm.CreateTransactionResults();
						ssm.UpdateTransaction();
					});
					}
				[Test]
				public void PickUp_WasWaitingForPickUpOrWaitingForNextTouch_SetsAndRunsPickUpProcess(){
					Slottable sb = MakeSB();
						System.Func<IEnumeratorFake> mockCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
						ISBCoroutineFactory stubCorFac = Substitute.For<ISBCoroutineFactory>();
						stubCorFac.MakePickUpCoroutine().Returns(mockCoroutine);
						sb.SetCoroutineFactory(stubCorFac);
					sb.WaitForAction();
					sb.WaitForPickUp();
					
					sb.PickUp();

					ISBActProcess actualProc = sb.actProcess;
					Assert.That(actualProc, Is.TypeOf(typeof(PickUpProcess)));
					Assert.That(actualProc.sse, Is.SameAs(sb));
					mockCoroutine.Received().Invoke();
					Assert.That(actualProc.isRunning, Is.True);
					}
				[Test]
				public void InPickingUp_OnDeselected_Always_CallsRefresh(){
					Slottable sb = MakeSB();
						SetUpForRefreshCall(sb);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.PickUp();

					sb.OnDeselected(new PointerEventDataFake());

					AssertRefreshCalled(sb);
					}
				[Test]
				public void InPickingUp_OnDeselected_Always_SetsIsFocused(){
					Slottable sb = MakeSB();
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.PickUp();

					sb.OnDeselected(new PointerEventDataFake());

					Assert.That(sb.isFocused, Is.True);
					}
				[Test]
				public void InPickingUp_OnPointerUp_IsHoveredAndIsStackable_SetsIsWaitingForNextTouch(){
					Slottable sb = MakeSB();
						ISlotSystemManager ssm = MakeSubSSM();
						ssm.hovered.Returns(sb);
						PartsInstance parts = MakePartsInstance(0, 2);
						sb.SetItem(parts);
						sb.SetSSM(ssm);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.PickUp();

					sb.OnPointerUp(new PointerEventDataFake());

					Assert.That(sb.isWaitingForNextTouch, Is.True);
					}
				[Test]
				public void InPickingUp_OnPointerUp_NotIsHoveredOrNotIsStackable_CallsSSMExecuteTransaction(){
					Slottable sb = MakeSB();
						ISlotSystemManager ssm = MakeSubSSM();
						ssm.hovered.Returns(sb);
						BowInstance bow = MakeBowInstance(0);
						sb.SetItem(bow);
						sb.SetSSM(ssm);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.PickUp();

					sb.OnPointerUp(new PointerEventDataFake());

					ssm.Received().ExecuteTransaction();
					}
				[Test]
				public void InPickingUp_OnEndDrag_Always_CallsSSMExecuteTransaction(){
					Slottable sb = MakeSB();
						ISlotSystemManager ssm = MakeSubSSM();
						sb.SetSSM(ssm);
					sb.WaitForAction();
					sb.WaitForPickUp();
					sb.PickUp();

					sb.OnEndDrag(new PointerEventDataFake());

					ssm.Received().ExecuteTransaction();
					}
			/*  MoveWithinState	*/
				[Test]
				public void MoveWithin_Always_SetsIsMovingWithin(){
					Slottable sb = MakeSB();

					sb.MoveWithin();

					Assert.That(sb.isMovingWithin, Is.True);
					}
				[Test]
				public void MoveWithin_Always_SetsAndRunsMoveWithinProcess(){
					Slottable sb = MakeSB();
						System.Func<IEnumeratorFake> mockCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
						ISBCoroutineFactory stubCorFac = Substitute.For<ISBCoroutineFactory>();
						stubCorFac.MakeMoveWithinCoroutine().Returns(mockCoroutine);
						sb.SetCoroutineFactory(stubCorFac);

					sb.MoveWithin();

					ISBActProcess actualProc = sb.actProcess;
					Assert.That(actualProc, Is.TypeOf(typeof(SBMoveWithinProcess)));
					Assert.That(actualProc.sse, Is.SameAs(sb));
					Assert.That(actualProc.isRunning, Is.True);
					mockCoroutine.Received().Invoke();
					}
			/*  AddedState	*/
				[Test]
				public void Add_Always_SetsIsAdding(){
					Slottable sb = MakeSB();

					sb.Add();

					Assert.That(sb.isAdding, Is.True);
					}
				[Test]
				public void Add_Always_SetsAndRunsAddProcess(){
					Slottable sb = MakeSB();
						System.Func<IEnumeratorFake> mockCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
						ISBCoroutineFactory stubCorFac = Substitute.For<ISBCoroutineFactory>();
						stubCorFac.MakeAddCoroutine().Returns(mockCoroutine);
						sb.SetCoroutineFactory(stubCorFac);

					sb.Add();

					ISBActProcess actualProc = sb.actProcess;
					Assert.That(actualProc, Is.TypeOf(typeof(SBAddProcess)));
					Assert.That(actualProc.sse, Is.SameAs(sb));
					Assert.That(actualProc.isRunning, Is.True);
					mockCoroutine.Received().Invoke();
					}
			/*  RemovedState	*/
				[Test]
				public void Remov_Always_SetsIsRemoving(){
					Slottable sb = MakeSB();

					sb.Remove();

					Assert.That(sb.isRemoving, Is.True);
					}
				[Test]
				public void Remov_Always_SetsAndRunsRemovProcess(){
					Slottable sb = MakeSB();
						System.Func<IEnumeratorFake> mockCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
						ISBCoroutineFactory stubCorFac = Substitute.For<ISBCoroutineFactory>();
						stubCorFac.MakeRemoveCoroutine().Returns(mockCoroutine);
						sb.SetCoroutineFactory(stubCorFac);

					sb.Remove();

					ISBActProcess actualProc = sb.actProcess;
					Assert.That(actualProc, Is.TypeOf(typeof(SBRemoveProcess)));
					Assert.That(actualProc.sse, Is.SameAs(sb));
					Assert.That(actualProc.isRunning, Is.True);
					mockCoroutine.Received().Invoke();
					}
			/* Sequence */
				[Test]
				public void ActStates_EventSequence(){
					Slottable sb = MakeSB();
						PartsInstance parts = MakePartsInstance(0, 20);
						BowInstance bow = MakeBowInstance(0);
						sb.SetItem(parts);
						ISlotSystemManager mockSSM = MakeSubSSM();
						sb.SetSSM(mockSSM);
						SlottableCommand mockTapComm = Substitute.For<SlottableCommand>();
						sb.SetTapCommand(mockTapComm);
					PointerEventDataFake eventData = new PointerEventDataFake();
					//focused !(stackable && hovered) WFA_down WFPickUp_exp PickingUp_up execTA
							sb.Focus();
							mockSSM.pickedSB.Returns((ISlotSystemElement)null);
							mockSSM.hovered.Returns((ISlotSystemElement)null);
						sb.WaitForAction();
							Assert.That(sb.isWaitingForAction, Is.True);
						
						sb.OnPointerDown(eventData);
							Assert.That(sb.isWaitingForPickUp, Is.True);
						
						sb.ExpireActProcess();
							Assert.That(sb.isPickingUp, Is.True);
							mockSSM.hovered.Returns((ISlotSystemElement)null);
						
						sb.OnPointerUp(eventData);
							mockSSM.Received(1).ExecuteTransaction();
					//focused stackable && hovered WFA_down WFPickUp_exp PickingUp_up WFNT_exp execTA
							sb.Focus();
							sb.SetItem(parts);
							mockSSM.pickedSB.Returns((ISlotSystemElement)null);
							mockSSM.hovered.Returns((ISlotSystemElement)null);
						sb.WaitForAction();

						sb.OnPointerDown(eventData);
							Assert.That(sb.isWaitingForPickUp, Is.True);
						
						sb.ExpireActProcess();
							Assert.That(sb.isPickingUp, Is.True);
							mockSSM.hovered.Returns(sb);
							mockSSM.pickedSB.Returns(sb);
						
						sb.OnPointerUp(eventData);
							Assert.That(sb.isWaitingForNextTouch, Is.True);
						
						sb.ExpireActProcess();
							mockSSM.Received(2).ExecuteTransaction();
					//focused stackable && hovered WFA_down WFPickUp_exp PickingUp_up WFNT_down increment
							sb.Focus();
							sb.SetItem(parts);
							mockSSM.pickedSB.Returns((ISlotSystemElement)null);
							mockSSM.hovered.Returns((ISlotSystemElement)null);
						sb.WaitForAction();

						sb.OnPointerDown(eventData);
							Assert.That(sb.isWaitingForPickUp, Is.True);
						
						sb.ExpireActProcess();
							Assert.That(sb.isPickingUp, Is.True);
							Assert.That(sb.pickedAmount, Is.EqualTo(1));
							mockSSM.pickedSB.Returns(sb);
							mockSSM.hovered.Returns(sb);
						
						sb.OnPointerUp(eventData);
							Assert.That(sb.isWaitingForNextTouch, Is.True);
						
						sb.OnPointerDown(eventData);
							Assert.That(sb.pickedAmount, Is.EqualTo(2));
					//defocused WFA_down WFPointerUp_up tap
							sb.Defocus();
							mockSSM.pickedSB.Returns((ISlotSystemElement)null);
							mockSSM.hovered.Returns((ISlotSystemElement)null);
						sb.WaitForAction();
							Assert.That(sb.isDefocused, Is.True);
							Assert.That(sb.isWaitingForAction, Is.True);
						
						sb.OnPointerDown(eventData);
							Assert.That(sb.isWaitingForPointerUp, Is.True);
						
						sb.OnPointerUp(eventData);
							mockTapComm.Received(1).Execute(sb);
					//focused WFA_down WFPickUp_up WFNT_down PickingUp pickedAmount 1
							sb.Focus();
							mockSSM.pickedSB.Returns((ISlotSystemElement)null);
							mockSSM.hovered.Returns((ISlotSystemElement)null);
						sb.WaitForAction();

						sb.OnPointerDown(eventData);
							Assert.That(sb.isWaitingForPickUp, Is.True);
						
						sb.OnPointerUp(eventData);
							Assert.That(sb.isWaitingForNextTouch, Is.True);
						
						sb.OnPointerDown(eventData);
							Assert.That(sb.isPickingUp, Is.True);
							Assert.That(sb.pickedAmount, Is.EqualTo(1));
					//
				}
		/* EqpStates */
			/* EquippedState */
				
	/* helpers */
		void AssertRefreshCalled(ISlottable sb){
			Assert.That(sb.isWaitingForAction, Is.True);
			Assert.That(sb.pickedAmount, Is.EqualTo(0));
			Assert.That(sb.newSlotID, Is.EqualTo(-2));
		}
		void SetUpForRefreshCall(ISlottable sb){
			sb.pickedAmount = 20;
			sb.SetNewSlotID(6);
		}
	}
}

