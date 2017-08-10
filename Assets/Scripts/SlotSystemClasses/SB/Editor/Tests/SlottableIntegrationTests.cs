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
	namespace SlottableTests{
		[TestFixture]
		[Category("Integration")]
		public class SlottableIntegrationTests : SlotSystemTest{
			[Test]
			public void SBStates_ByDefault_AreNull(){
				Slottable sb = MakeSBWithRealStateHandlers();
				Assert.That(sb.isActStateNull, Is.True);
				Assert.That(sb.wasActStateNull, Is.True);
				Assert.That(sb.isSelStateNull, Is.True);
				Assert.That(sb.wasSelStateNull, Is.True);
				Assert.That(sb.isEqpStateNull, Is.True);
				Assert.That(sb.wasEqpStateNull, Is.True);
				Assert.That(sb.isMrkStateNull, Is.True);
				Assert.That(sb.wasMrkStateNull, Is.True);
			}
			[Test]
			public void processes_ByDefault_AreNull(){
				Slottable sb = MakeSBWithRealStateHandlers();

				Assert.That(sb.actProcess, Is.Null);
				Assert.That(sb.eqpProcess, Is.Null);
				Assert.That(sb.mrkProcess, Is.Null);
			}
			/* ActStates */
				/* WaitForActionState */
					[Test]
					public void WaitForAction_Always_SetsIsWaitingForAction(){
						Slottable sb = MakeSBWithRealStateHandlers();
						
						sb.WaitForAction();

						Assert.That(sb.isWaitingForAction, Is.True);
						}
					[Test]
					public void WaitForAction_wasActStateNull_DoesNotCallActEngineSetProcNull(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ISSEProcessEngine<ISBActProcess> mockProcEngine = Substitute.For<ISSEProcessEngine<ISBActProcess>>();
							sb.SetActProcessEngine(mockProcEngine);
						sb.WaitForAction();


						mockProcEngine.DidNotReceive().SetAndRunProcess((ISBActProcess)null);
						}
					[Test]
					public void WaitForAction_WasWaitingForAction_DoesNotCallActEngineSetProcNull(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ISSEProcessEngine<ISBActProcess> mockProcEngine = Substitute.For<ISSEProcessEngine<ISBActProcess>>();
							sb.SetActProcessEngine(mockProcEngine);
						sb.WaitForAction();

						sb.WaitForAction();

						mockProcEngine.DidNotReceive().SetAndRunProcess((ISBActProcess)null);
						}
					[Test]
					public void WaitForAction_IsNotActStateInitAndWasNotWFAction_CallsActEngineSetProcNull(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ISSEProcessEngine<ISBActProcess> mockEngine = Substitute.For<ISSEProcessEngine<ISBActProcess>>();
							sb.SetActProcessEngine(mockEngine);
						sb.Add();

						sb.WaitForAction();

						mockEngine.Received().SetAndRunProcess((ISBActProcess)null);
						}
					[Test]
					public void InWaitForAction_OnPointerDown_IsFocused_SetsIsSelected(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.WaitForAction();
						SSESelStateHandler handler = new SSESelStateHandler();
						sb.SetSelStateHandler(handler);
						sb.Focus();

						sb.OnPointerDown(new PointerEventDataFake());

						Assert.That(sb.isSelected, Is.True);
						}
					[Test]
					public void InWaitForAction_OnPointerDown_IsFocused_SetsIsWFPickUp(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.WaitForAction();
						SSESelStateHandler handler = new SSESelStateHandler();
						sb.SetSelStateHandler(handler);
						sb.Focus();

						sb.OnPointerDown(new PointerEventDataFake());

						Assert.That(sb.isWaitingForPickUp, Is.True);
						}
					[Test]
					public void InWaitForAction_OnPointerDown_IsNotFocused_SetsIsWFPointerUp(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.WaitForAction();
						SSESelStateHandler handler = new SSESelStateHandler();
						sb.SetSelStateHandler(handler);

						sb.OnPointerDown(new PointerEventDataFake());

						Assert.That(sb.isWaitingForPointerUp, Is.True);
						}
				/* WaitForPickUp */
					[Test][ExpectedException(typeof(InvalidOperationException))]
					public void WaitForPickUp_WasNotWaitForAction_ThrowsException(){
						Slottable sb = MakeSBWithRealStateHandlers();

						sb.WaitForPickUp();
					}
					[Test]
					public void WaitForPickUp_WasWaitingForAction_SetsIsWaitingForPickUp(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.WaitForAction();

						sb.WaitForPickUp();

						Assert.That(sb.isWaitingForPickUp, Is.True);
						}
					[Test]
					public void WaitForPickUp_WasWaitingForAction_SetsAndRunsWFPickUpProc(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockWFPickUpCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.waitForPickUpCoroutine.Returns(mockWFPickUpCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);
						sb.WaitForAction();

						sb.WaitForPickUp();

						AssertSBActProcessIsSetAndRunning(sb, typeof(WaitForPickUpProcess), mockWFPickUpCoroutine);
						}
					[Test]
					public void ExpireProcess_WaitingForPickUpProcess_SetsIsPickingUp(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();

						sb.ExpireActProcess();

						Assert.That(sb.isPickingUp, Is.True);
						}
					[Test]
					public void ExpireProcess_WaitingForPickUpProcess_SetsPickedAmount1(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();

						sb.ExpireActProcess();

						Assert.That(sb.pickedAmount, Is.EqualTo(1));
						}
					[Test]
					public void InWaitForPickUp_OnPointerUp_Always_SetsIsWaitingForNextTouch(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.WaitForAction();
						sb.WaitForPickUp();

						sb.OnPointerUp(new PointerEventDataFake());

						Assert.That(sb.isWaitingForNextTouch, Is.True);
						}
					[Test]
					public void InWaitForPickUp_OnEndDrag_Always_CallsRefresh(){
						Slottable sb = MakeSBWithRealStateHandlers();
							SetUpForRefreshCall(sb);
							sb.SetSlotHandler(new SlotHandler());
						sb.WaitForAction();
						sb.WaitForPickUp();

						sb.OnEndDrag(new PointerEventDataFake());

						AssertRefreshCalled(sb);
						}
					[Test]
					public void InWaitForPickUp_OnEndDrag_Always_SetsIsFocused(){
						Slottable sb = MakeSBWithRealStateHandlers();
							SSESelStateHandler handler = new SSESelStateHandler();
							sb.SetSelStateHandler(handler);
						sb.WaitForAction();
						sb.WaitForPickUp();

						sb.OnEndDrag(new PointerEventDataFake());

						Assert.That(sb.isFocused, Is.True);
						}
				/* WaitForPointerUpState */
					[Test][ExpectedException(typeof(InvalidOperationException))]
					public void WaitForPointerUp_WasNotWaitingForAction_ThrowsException(){
						Slottable sb = MakeSBWithRealStateHandlers();

						sb.WaitForPointerUp();
					}
					[Test]
					public void WaitForPointerUp_WasWaitingForAction_SetsIsWaitingForPointerUp(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.WaitForAction();

						sb.WaitForPointerUp();

						Assert.That(sb.isWaitingForPointerUp, Is.True);
						}
					[Test]
					public void WaitForPointerUp_WasWaitingForAction_SetsAndRunsWaitForPointerUpProcess(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockWFPointerUpCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.waitForPointerUpCoroutine.Returns(mockWFPointerUpCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);
						sb.WaitForAction();
						
						sb.WaitForPointerUp();

						AssertSBActProcessIsSetAndRunning(sb, typeof(WaitForPointerUpProcess), mockWFPointerUpCoroutine);
						}
					[Test]
					public void ExpireActPorcess_WaitForPointerUpProcess_SetIsDefocused(){
						Slottable sb = MakeSBWithRealStateHandlers();
						SSESelStateHandler handler = new SSESelStateHandler();
						sb.SetSelStateHandler(handler);
						sb.WaitForAction();
						sb.WaitForPointerUp();

						sb.ExpireActProcess();

						Assert.That(sb.isDefocused, Is.True);
						}
					[Test]
					public void InWaitForPointerUp_OnPointerUp_Always_CallsTapCommandExecute(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ISBCommand mockTapComm = Substitute.For<ISBCommand>();
							sb.SetTapCommand(mockTapComm);
							SSESelStateHandler handler = new SSESelStateHandler();
							sb.SetSelStateHandler(handler);
						sb.WaitForAction();
						sb.WaitForPointerUp();

						sb.OnPointerUp(new PointerEventDataFake());

						mockTapComm.Received().Execute(sb);
						}
					[Test]
					public void InWaitForPointerUp_OnPointerUp_Always_CallsRefresh(){
						Slottable sb = MakeSBWithRealStateHandlers();
							SetUpForRefreshCall(sb);
							sb.SetSlotHandler(new SlotHandler());
						sb.WaitForAction();
						sb.WaitForPointerUp();

						sb.OnPointerUp(new PointerEventDataFake());

						AssertRefreshCalled(sb);
						}
					[Test]
					public void InWaitForPointerUp_OnPointerUp_Always_SetsIsDefocused(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.WaitForAction();
						sb.WaitForPointerUp();

						sb.OnPointerUp(new PointerEventDataFake());

						Assert.That(sb.isDefocused, Is.True);
						}
					[Test]
					public void InWaitForPointerUp_OnEndDrag_Always_CallsRefresh(){
						Slottable sb = MakeSBWithRealStateHandlers();
							SetUpForRefreshCall(sb);
							sb.SetSlotHandler(new SlotHandler());
						sb.WaitForAction();
						sb.WaitForPointerUp();

						sb.OnEndDrag(new PointerEventDataFake());

						AssertRefreshCalled(sb);
						}
					[Test]
					public void InWaitForPointerUp_OnEndDrag_Always_SetsIsDefocused(){
						Slottable sb = MakeSBWithRealStateHandlers();
							SSESelStateHandler handler = new SSESelStateHandler();
							sb.SetSelStateHandler(handler);
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
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.PickUp();
						
						sb.WaitForNextTouch();

						Assert.That(sb.isWaitingForNextTouch, Is.True);
						}
					[Test]
					public void WaitForNextTouch_WasWaitingForPickUp_SetsIsWaitingForNextTouch(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.WaitForAction();
						sb.WaitForPickUp();

						sb.WaitForNextTouch();

						Assert.That(sb.isWaitingForNextTouch, Is.True);
						}
					[Test]
					public void WaitForNextTouch_WasPickingUp_SetsAndRunsWaitForNextTouchProcess(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockWFNTCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.waitForNextTouchCoroutine.Returns(mockWFNTCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.PickUp();
						
						sb.WaitForNextTouch();

						AssertSBActProcessIsSetAndRunning(sb, typeof(WaitForNextTouchProcess), mockWFNTCoroutine);
						}
					[Test]
					public void WaitForNextTouch_WasWaitingForPickUp_SetsAndRunsWaitForNextTouchProcess(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockWFNTCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.waitForNextTouchCoroutine.Returns(mockWFNTCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);
						sb.WaitForAction();
						sb.WaitForPickUp();
						
						sb.WaitForNextTouch();

						AssertSBActProcessIsSetAndRunning(sb, typeof(WaitForNextTouchProcess), mockWFNTCoroutine);
						}
					[Test]
					public void ExpireActProcess_WaitForNextTouchProcess_IsNotPickedUp_CallsTapCommandExecute(){
						Slottable sb = MakeSBWithRealStateHandlers();
						ISBCommand mockComm = sb.tapCommand;
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();

						sb.ExpireActProcess();

						mockComm.Received().Execute(sb);
						}
					[Test]
					public void ExpireActProcess_WaitForNextTouchProcess_IsNotPickedUp_CallsRefresh(){
						Slottable sb = MakeSBWithRealStateHandlers();
							SetUpForRefreshCall(sb);
							sb.taCache.pickedSB.Returns((ISlottable)null);
							sb.SetSlotHandler(new SlotHandler());
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();

						sb.ExpireActProcess();

						AssertRefreshCalled(sb);
					}
					[Test]
					public void ExpireActProcess_WaitForNextTouchProcess_IsNotPickedUp_SetsIsFocused(){
						Slottable sb = MakeSBWithRealStateHandlers();
							sb.taCache.pickedSB.Returns((ISlottable)null);
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();

						sb.ExpireActProcess();

						Assert.That(sb.isFocused, Is.True);
						}
					[Test]
					public void ExpireActProcess_WaitForNextTouchProcess_IsPickedUp_CallsSSMExecuteTransaction(){
						Slottable sb = MakeSBWithRealStateHandlers();
							sb.taCache.pickedSB.Returns(sb);
						ITransactionManager mockTAM = sb.ssm.tam;
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();

						sb.ExpireActProcess();

						mockTAM.Received().ExecuteTransaction();
					}
					Slottable MakeSBForInWFNT(bool isPickedUp){
						Slottable sb = MakeSBWithRealStateHandlers();
							if(!isPickedUp)
								sb.taCache.pickedSB.Returns((ISlottable)null);
							else
								sb.taCache.pickedSB.Returns(sb);
						return sb;
					}
					[Test]
					public void InWaitForNextTouch_OnPointerDown_IsNotPickedUp_SetsIsPickingUp(){
						Slottable sb = MakeSBForInWFNT(false);
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();

						sb.OnPointerDown(new PointerEventDataFake());

						Assert.That(sb.isPickingUp, Is.True);
						}
					[Test]
					public void InWaitForNextTouch_OnPointerDown_IsNotPickedUp_SetsPickedAmount1(){
						Slottable sb = MakeSBForInWFNT(false);
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();

						sb.OnPointerDown(new PointerEventDataFake());

						Assert.That(sb.pickedAmount, Is.EqualTo(1));
						}
					[Test]
					public void InWaitForNextTouch_OnPointerDown_IsPickedUp_SetsIsPickingUp(){
						Slottable sb = MakeSBForInWFNT(true);
							BowInstance bow = MakeBowInstance(0);
							sb.SetItem(bow);
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();
						
						sb.OnPointerDown(new PointerEventDataFake());

						Assert.That(sb.isPickingUp, Is.True);
						}
					[Test]
					public void InWaitForNextTouch_OnPointerDown_IsPickedUpIsStackableQuantityGrearterThanPickedAmount_IncrementsPickedAmount(){
						Slottable sb = MakeSBForInWFNT(true);
							ItemHandler itemHandler;
								PartsInstance parts = MakePartsInstance(0, 10);
								itemHandler = new ItemHandler(parts);
								itemHandler.pickedAmount = 8;
							sb.SetItemHandler(itemHandler);
							sb.taCache.pickedSB.Returns(sb);
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();

						sb.OnPointerDown(new PointerEventDataFake());

						Assert.That(sb.pickedAmount, Is.EqualTo(9));
						}
					[Test]
					public void InWaitForNextTouch_OnDeselect_Always_CallsRefresh(){
						Slottable sb = MakeSBWithRealStateHandlers();
							SetUpForRefreshCall(sb);
							sb.SetSlotHandler(new SlotHandler());
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();

						sb.OnDeselected(new PointerEventDataFake());

						AssertRefreshCalled(sb);
						}
					[Test]
					public void InWaitForNextTouch_OnDeselect_Always_SetsIsFocused(){
						Slottable sb = MakeSBWithRealStateHandlers();
							SSESelStateHandler handler = new SSESelStateHandler();
							sb.SetSelStateHandler(handler);
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
							SSESelStateHandler handler = new SSESelStateHandler();
							sb.SetSelStateHandler(handler);

						sb.PickUp();
					}
					Slottable MakeSBForPickUp(){
						Slottable sb = MakeSB();
							ISlotSystemManager ssm = Substitute.For<ISlotSystemManager>();
								ITransactionManager tam = Substitute.For<ITransactionManager>();
									ITransactionIconHandler iconHd = MakeSubIconHandler();
									tam.iconHandler.Returns(iconHd);
								ssm.tam.Returns(tam);
							sb.SetSSM(ssm);
							ITransactionCache stubTAC = MakeSubTAC();
							sb.SetTACache(stubTAC);
							SSESelStateHandler selStateHandler = new SSESelStateHandler();
							sb.SetSelStateHandler(selStateHandler);
							ITAMActStateHandler tamStateHandler = MakeSubTAMStateHandler();
								tam.actStateHandler.Returns(tamStateHandler);
							IItemHandler itemHandler = Substitute.For<IItemHandler>();
							sb.SetItemHandler(itemHandler);
						return sb;
					}
					[Test]
					public void PickUp_WasWaitingForPickUp_SetsIsPickingUp(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						
						sb.PickUp();

						Assert.That(sb.isPickingUp, Is.True);
						}
					[Test]
					public void PickUp_WasWaitingForNextTouch_SetsIsPickingUp(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();
						
						sb.PickUp();

						Assert.That(sb.isPickingUp, Is.True);
						}
					[Test]
					public void PickUp_IsNotCurSelStateNullAndValidPrevActState_CallsHoverableOnHoverEnter(){
						Slottable sb = MakeSBWithRealStateHandlers();
						IHoverable mockHoverable = sb.hoverable;
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.Deactivate();

						sb.PickUp();

						mockHoverable.Received().OnHoverEnter();
						}
					[Test]
					public void PickUp_SSMNotNullAndValidPrevActState_CallsSSMInSequence(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ITransactionCache mockTAC = sb.taCache;
							ITAMActStateHandler tamStateHandler = sb.ssm.tam.actStateHandler;
							ITransactionIconHandler mockIconHandler = sb.ssm.tam.iconHandler;
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						
						sb.PickUp();

						Received.InOrder(()=> {
							mockTAC.SetPickedSB(sb);
							mockIconHandler.SetDIcon1(sb);
							tamStateHandler.Probe();
							mockTAC.CreateTransactionResults();
							mockTAC.UpdateFields();
						});
						}
					[Test]
					public void PickUp_WasWaitingForPickUpOrWaitingForNextTouch_SetsAndRunsPickUpProcess(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockPickUpCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.pickUpCoroutine.Returns(mockPickUpCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						
						sb.PickUp();

						AssertSBActProcessIsSetAndRunning(sb, typeof(PickUpProcess), mockPickUpCoroutine);
						}
					[Test]
					public void InPickingUp_OnDeselected_Always_CallsRefresh(){
						Slottable sb = MakeSBWithRealStateHandlers();
						SetUpForRefreshCall(sb);
						sb.SetSlotHandler(new SlotHandler());
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.PickUp();

						sb.OnDeselected(new PointerEventDataFake());

						AssertRefreshCalled(sb);
						}
					[Test]
					public void InPickingUp_OnDeselected_Always_SetsIsFocused(){
						Slottable sb = MakeSBWithRealStateHandlers();
						SetUpForRefreshCall(sb);
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.PickUp();

						sb.OnDeselected(new PointerEventDataFake());

						Assert.That(sb.isFocused, Is.True);
						}
					[Test]
					public void InPickingUp_OnPointerUp_IsHoveredAndIsStackable_SetsIsWaitingForNextTouch(){
						Slottable sb = MakeSBWithRealStateHandlers();
							sb.itemHandler.isStackable.Returns(true);
							sb.hoverable.isHovered.Returns(true);
						SetUpForRefreshCall(sb);
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.PickUp();

						sb.OnPointerUp(new PointerEventDataFake());

						Assert.That(sb.isWaitingForNextTouch, Is.True);
						}
					[Test]
					public void InPickingUp_OnPointerUp_NotIsHoveredOrNotIsStackable_CallsSSMExecuteTransaction(){
						Slottable sb = MakeSBWithRealStateHandlers();
							sb.hoverable.isHovered.Returns(false);
						ITransactionManager mockTAM = sb.ssm.tam;
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.PickUp();

						sb.OnPointerUp(new PointerEventDataFake());

						mockTAM.Received().ExecuteTransaction();
						}
					[Test]
					public void InPickingUp_OnEndDrag_Always_CallsSSMExecuteTransaction(){
						Slottable sb = MakeSBWithRealStateHandlers();
						ITransactionManager mockTAM = sb.ssm.tam;
						sb.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.PickUp();

						sb.OnEndDrag(new PointerEventDataFake());

						mockTAM.Received().ExecuteTransaction();
						}
				/*  MoveWithinState	*/
					[Test]
					public void MoveWithin_Always_SetsIsMovingWithin(){
						Slottable sb = MakeSBWithRealStateHandlers();

						sb.MoveWithin();

						Assert.That(sb.isMovingWithin, Is.True);
						}
					[Test]
					public void MoveWithin_Always_SetsAndRunsMoveWithinProcess(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockMoveWithinCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.moveWithinCoroutine.Returns(mockMoveWithinCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);

						sb.MoveWithin();

						AssertSBActProcessIsSetAndRunning(sb, typeof(SBMoveWithinProcess), mockMoveWithinCoroutine);
						}
				/*  AddedState	*/
					[Test]
					public void Add_Always_SetsIsAdding(){
						Slottable sb = MakeSBWithRealStateHandlers();

						sb.Add();

						Assert.That(sb.isAdding, Is.True);
						}
					[Test]
					public void Add_Always_SetsAndRunsAddProcess(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockAddCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.addCoroutine.Returns(mockAddCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);

						sb.Add();

						AssertSBActProcessIsSetAndRunning(sb, typeof(SBAddProcess), mockAddCoroutine);
						}
				/*  RemovedState	*/
					[Test]
					public void Remove_Always_SetsIsRemoving(){
						Slottable sb = MakeSBWithRealStateHandlers();

						sb.Remove();

						Assert.That(sb.isRemoving, Is.True);
						}
					[Test]
					public void Remove_Always_SetsAndRunsRemovProcess(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockRemoveCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.removeCoroutine.Returns(mockRemoveCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);

						sb.Remove();

						AssertSBActProcessIsSetAndRunning(sb, typeof(SBRemoveProcess), mockRemoveCoroutine);
						}
				/* Sequence */
					[Test]
					public void ActStates_EventSequence(){
						Slottable sb = MakeSBWithRealStateHandlers();
								PartsInstance parts = MakePartsInstance(0, 20);
							sb.itemHandler.item.Returns(parts);
							ITransactionCache mockTAC = sb.taCache;
							ITransactionManager mockTAM = sb.ssm.tam;
							ISBCommand mockTapComm = sb.tapCommand;
							
						PointerEventDataFake eventData = new PointerEventDataFake();
						//focused !(stackable && hovered) WFA_down WFPickUp_exp PickingUp_up execTA
								sb.Focus();
								mockTAC.pickedSB.Returns((ISlotSystemElement)null);
								mockTAC.hovered.Returns((IHoverable)null);
							sb.WaitForAction();
								Assert.That(sb.isWaitingForAction, Is.True);
							
							sb.OnPointerDown(eventData);
								Assert.That(sb.isWaitingForPickUp, Is.True);
							
							sb.ExpireActProcess();
								Assert.That(sb.isPickingUp, Is.True);
								mockTAC.hovered.Returns((IHoverable)null);
							
							sb.OnPointerUp(eventData);
								mockTAM.Received(1).ExecuteTransaction();
						//focused stackable && hovered WFA_down WFPickUp_exp PickingUp_up WFNT_exp execTA
								sb.Focus();
								// sb.itemHandler.item.Returns(parts);
								sb.itemHandler.isStackable.Returns(true);
								mockTAC.pickedSB.Returns((ISlotSystemElement)null);
								mockTAC.hovered.Returns((IHoverable)null);
							sb.WaitForAction();

							sb.OnPointerDown(eventData);
								Assert.That(sb.isWaitingForPickUp, Is.True);
							
							sb.ExpireActProcess();
								Assert.That(sb.isPickingUp, Is.True);
								mockTAC.hovered.Returns(sb.hoverable);
								mockTAC.pickedSB.Returns(sb);

								sb.hoverable.isHovered.Returns(true);
							sb.OnPointerUp(eventData);
								Assert.That(sb.isWaitingForNextTouch, Is.True);
							
							sb.ExpireActProcess();
								mockTAM.Received(2).ExecuteTransaction();
						//focused stackable && hovered WFA_down WFPickUp_exp PickingUp_up WFNT_down increment
								sb.Focus();
								sb.itemHandler.item.Returns(parts);
								mockTAC.pickedSB.Returns((ISlotSystemElement)null);
								mockTAC.hovered.Returns((IHoverable)null);
							sb.WaitForAction();

							sb.OnPointerDown(eventData);
								Assert.That(sb.isWaitingForPickUp, Is.True);
							
							sb.ExpireActProcess();
								Assert.That(sb.isPickingUp, Is.True);
								Assert.That(sb.pickedAmount, Is.EqualTo(1));
								mockTAC.pickedSB.Returns(sb);
								mockTAC.hovered.Returns(sb.hoverable);
							
							sb.OnPointerUp(eventData);
								Assert.That(sb.isWaitingForNextTouch, Is.True);
							
							sb.OnPointerDown(eventData);
								sb.itemHandler.Received().pickedAmount++;
						//defocused WFA_down WFPointerUp_up tap
								sb.Defocus();
								mockTAC.pickedSB.Returns((ISlotSystemElement)null);
								mockTAC.hovered.Returns((IHoverable)null);
							sb.WaitForAction();
								Assert.That(sb.isDefocused, Is.True);
								Assert.That(sb.isWaitingForAction, Is.True);
							
							sb.OnPointerDown(eventData);
								Assert.That(sb.isWaitingForPointerUp, Is.True);
							
							sb.OnPointerUp(eventData);
								mockTapComm.Received(1).Execute(sb);
						//focused WFA_down WFPickUp_up WFNT_down PickingUp pickedAmount 1
								sb.Focus();
								mockTAC.pickedSB.Returns((ISlotSystemElement)null);
								mockTAC.hovered.Returns((IHoverable)null);
							sb.WaitForAction();

							sb.OnPointerDown(eventData);
								Assert.That(sb.isWaitingForPickUp, Is.True);
							
							sb.OnPointerUp(eventData);
								Assert.That(sb.isWaitingForNextTouch, Is.True);
							
							sb.OnPointerDown(eventData);
								Assert.That(sb.isPickingUp, Is.True);
								Assert.That(sb.pickedAmount, Is.EqualTo(1));
					}
			/* EqpStates */
				/* EquippedState */
					
		/* helpers */

			PointerEventDataFake eventData{get{return new PointerEventDataFake();}}
			void AssertRefreshCalled(ISlottable sb){
				Assert.That(sb.isWaitingForAction, Is.True);
				Assert.That(sb.pickedAmount, Is.EqualTo(0));
				Assert.That(sb.newSlotID, Is.EqualTo(-2));
			}
			void SetUpForRefreshCall(ISlottable sb){
				sb.pickedAmount = 20;
				sb.SetNewSlotID(6);
			}
			public void AssertSBActProcessIsSetAndRunning(ISlottable sb, Type procType, System.Func<IEnumeratorFake> coroutine){
				ISBActProcess actualProc = sb.actProcess;
				Assert.That(actualProc, Is.TypeOf(procType));
				Assert.That(actualProc.isRunning, Is.True);
				coroutine.Received().Invoke();
			}
		}
	}
}

