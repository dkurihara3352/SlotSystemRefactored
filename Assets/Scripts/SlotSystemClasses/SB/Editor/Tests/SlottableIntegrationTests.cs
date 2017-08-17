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
				Assert.That(sb.IsActStateNull(), Is.True);
				Assert.That(sb.WasActStateNull(), Is.True);
				Assert.That(sb.IsEqpStateNull(), Is.True);
				Assert.That(sb.WasEqpStateNull(), Is.True);
				Assert.That(sb.IsMrkStateNull(), Is.True);
				Assert.That(sb.WasMrkStateNull(), Is.True);
			}
			[Test]
			public void processes_ByDefault_AreNull(){
				Slottable sb = MakeSBWithRealStateHandlers();

				Assert.That(sb.GetActProcess(), Is.Null);
				Assert.That(sb.GetEqpProcess(), Is.Null);
				Assert.That(sb.GetMrkProcess(), Is.Null);
			}
			/* ActStates */
				/* WaitForActionState */
					[Test]
					public void WaitForAction_Always_SetsIsWaitingForAction(){
						Slottable sb = MakeSBWithRealStateHandlers();
						
						sb.WaitForAction();

						Assert.That(sb.IsWaitingForAction(), Is.True);
						}
					[Test]
					public void WaitForAction_WasActStateNull_DoesNotCallActEngineSetProcNull(){
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
						Slottable sb = MakeSB();
							IHoverable hoverable = new Hoverable(MakeSubTAC());
							sb.SetHoverable(hoverable);
							SSESelStateHandler selStateHandler = new SBSelStateHandler(sb);
							hoverable.SetSSESelStateHandler(selStateHandler);
							sb.SetSelStateHandler(selStateHandler);
							selStateHandler.Focus();
							ISBActStateHandler actStateHandler = new SBActStateHandler(sb, MakeSubTAM());
							sb.SetActStateHandler(actStateHandler);
						sb.WaitForAction();

						sb.OnPointerDown(new PointerEventDataFake());

						Assert.That(selStateHandler.isSelected, Is.True);
					}
					[Test]
					public void InWaitForAction_OnPointerDown_IsFocused_SetsIsWFPickUp(){
						Slottable sb = MakeSB();
							IHoverable hoverable = new Hoverable(MakeSubTAC());
							sb.SetHoverable(hoverable);
							SSESelStateHandler selStateHandler = new SBSelStateHandler(sb);
							hoverable.SetSSESelStateHandler(selStateHandler);
							sb.SetSelStateHandler(selStateHandler);
							selStateHandler.Focus();
							ISBActStateHandler actStateHandler = new SBActStateHandler(sb, MakeSubTAM());
							sb.SetActStateHandler(actStateHandler);
						sb.WaitForAction();

						sb.OnPointerDown(new PointerEventDataFake());

						Assert.That(sb.IsWaitingForPickUp(), Is.True);
					}
					[Test]
					public void InWaitForAction_OnPointerDown_IsNotFocused_SetsIsWFPointerUp(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.WaitForAction();
						SSESelStateHandler handler = new SBSelStateHandler(sb);
						sb.SetSelStateHandler(handler);

						sb.OnPointerDown(new PointerEventDataFake());

						Assert.That(sb.IsWaitingForPointerUp(), Is.True);
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

						Assert.That(sb.IsWaitingForPickUp(), Is.True);
						}
					[Test]
					public void WaitForPickUp_WasWaitingForAction_SetsAndRunsWFPickUpProc(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockWFPickUpCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.GetWaitForPickUpCoroutine().Returns(mockWFPickUpCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);
						sb.WaitForAction();

						sb.WaitForPickUp();

						AssertSBActProcessIsSetAndRunning(sb, typeof(WaitForPickUpProcess), mockWFPickUpCoroutine);
						}
					[Test]
					public void ExpireProcess_WaitingForPickUpProcess_SetsIsPickingUp(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
							selStateHandler.isFocused.Returns(true);
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPickUp();

						sb.ExpireActProcess();

						Assert.That(sb.IsPickingUp(), Is.True);
					}
					[Test]
					public void ExpireProcess_WaitingForPickUpProcess_SetsPickedAmount1(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
							selStateHandler.isFocused.Returns(true);
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPickUp();

						sb.ExpireActProcess();

						sb.itemHandler.Received().SetPickedAmount(1);
					}
					[Test]
					public void InWaitForPickUp_OnPointerUp_Always_SetsIsWaitingForNextTouch(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.WaitForAction();
						sb.WaitForPickUp();

						sb.OnPointerUp(new PointerEventDataFake());

						Assert.That(sb.IsWaitingForNextTouch(), Is.True);
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
							SSESelStateHandler selStateHandler = new SBSelStateHandler(sb);
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPickUp();

						sb.OnEndDrag(new PointerEventDataFake());

						Assert.That(selStateHandler.isFocused, Is.True);
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

						Assert.That(sb.IsWaitingForPointerUp(), Is.True);
						}
					[Test]
					public void WaitForPointerUp_WasWaitingForAction_SetsAndRunsWaitForPointerUpProcess(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockWFPointerUpCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.GetWaitForPointerUpCoroutine().Returns(mockWFPointerUpCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);
						sb.WaitForAction();
						
						sb.WaitForPointerUp();

						AssertSBActProcessIsSetAndRunning(sb, typeof(WaitForPointerUpProcess), mockWFPointerUpCoroutine);
						}
					[Test]
					public void ExpireActPorcess_WaitForPointerUpProcess_SetIsDefocused(){
						Slottable sb = MakeSBWithRealStateHandlers();
						SSESelStateHandler selStateHandler = new SBSelStateHandler(sb);
						sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPointerUp();

						sb.ExpireActProcess();

						Assert.That(selStateHandler.isDefocused, Is.True);
					}
					[Test]
					public void InWaitForPointerUp_OnPointerUp_Always_CallsTapCommandExecute(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ISBCommand mockTapComm = Substitute.For<ISBCommand>();
							sb.SetTapCommand(mockTapComm);
							SSESelStateHandler handler = new SBSelStateHandler(sb);
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
							ISSESelStateHandler selStateHandler = new SBSelStateHandler(sb);
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPointerUp();

						sb.OnPointerUp(new PointerEventDataFake());

						Assert.That(selStateHandler.isDefocused, Is.True);
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
							SSESelStateHandler selStateHandler = new SBSelStateHandler(sb);
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPointerUp();

						sb.OnEndDrag(new PointerEventDataFake());

						Assert.That(selStateHandler.isDefocused, Is.True);
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
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
								selStateHandler.isFocused.Returns(true);
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.PickUp();
						
						sb.WaitForNextTouch();

						Assert.That(sb.IsWaitingForNextTouch(), Is.True);
					}
					[Test]
					public void WaitForNextTouch_WasWaitingForPickUp_SetsIsWaitingForNextTouch(){
						Slottable sb = MakeSBWithRealStateHandlers();
						sb.WaitForAction();
						sb.WaitForPickUp();

						sb.WaitForNextTouch();

						Assert.That(sb.IsWaitingForNextTouch(), Is.True);
						}
					[Test]
					public void WaitForNextTouch_WasPickingUp_SetsAndRunsWaitForNextTouchProcess(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockWFNTCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.GetWaitForNextTouchCoroutine().Returns(mockWFNTCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
							selStateHandler.isFocused.Returns(true);
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
							mockCorRepo.GetWaitForNextTouchCoroutine().Returns(mockWFNTCoroutine);
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
							ISSESelStateHandler selStateHandler = new SBSelStateHandler(sb);
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();

						sb.ExpireActProcess();

						Assert.That(selStateHandler.isFocused, Is.True);
						}
					[Test]
					public void ExpireActProcess_WaitForNextTouchProcess_IsPickedUp_CallsSSMExecuteTransaction(){
						Slottable sb = MakeSBWithRealStateHandlers();
							sb.taCache.pickedSB.Returns(sb);
						ITransactionManager mockTAM = sb.GetSSM().tam;
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
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
							selStateHandler.isFocused.Returns(true);
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();

						sb.OnPointerDown(new PointerEventDataFake());

						Assert.That(sb.IsPickingUp(), Is.True);
					}
					[Test]
					public void InWaitForNextTouch_OnPointerDown_IsNotPickedUp_SetsPickedAmount1(){
						Slottable sb = MakeSBForInWFNT(false);
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
							selStateHandler.isFocused.Returns(true);
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();

						sb.OnPointerDown(new PointerEventDataFake());

						sb.itemHandler.Received().SetPickedAmount(1);
					}
					[Test]
					public void InWaitForNextTouch_OnPointerDown_IsPickedUp_SetsIsPickingUp(){
						Slottable sb = MakeSBForInWFNT(true);
							BowInstance bow = MakeBowInstance(0);
							sb.SetItem(bow);
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
								selStateHandler.isFocused.Returns(true);
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();
						
						sb.OnPointerDown(new PointerEventDataFake());

						Assert.That(sb.IsPickingUp(), Is.True);
						}
					[Test]
					public void InWaitForNextTouch_OnPointerDown_IsPickedUpIsStackableQuantityGrearterThanPickedAmount_IncrementsPickedAmount(){
						Slottable sb = MakeSBForInWFNT(true);
							ItemHandler itemHandler;
								PartsInstance parts = MakePartsInstance(0, 10);
								itemHandler = new ItemHandler(parts);
								itemHandler.SetPickedAmount(8);
							sb.SetItemHandler(itemHandler);
							sb.taCache.pickedSB.Returns(sb);
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
								selStateHandler.isFocused.Returns(true);
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();

						sb.OnPointerDown(new PointerEventDataFake());

						Assert.That(sb.GetPickedAmount(), Is.EqualTo(9));
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
							SSESelStateHandler selStateHandler = new SSESelStateHandler();
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();

						sb.OnDeselected(new PointerEventDataFake());

						Assert.That(selStateHandler.isFocused, Is.True);
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
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
							selStateHandler.isFocused.Returns(true);
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPickUp();
						
						sb.PickUp();

						Assert.That(sb.IsPickingUp(), Is.True);
					}
					[Test]
					public void PickUp_WasWaitingForNextTouch_SetsIsPickingUp(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
							selStateHandler.isFocused.Returns(true);
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.WaitForNextTouch();
						
						sb.PickUp();

						Assert.That(sb.IsPickingUp(), Is.True);
					}
					[Test]
					public void PickUp_IsNotCurSelStateNullAndValidPrevActState_CallsHoverableOnHoverEnter(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ISSESelStateHandler selStateHandler = new SBSelStateHandler(sb);
							sb.SetSelStateHandler(selStateHandler);
						IHoverable mockHoverable = sb.hoverable;
						sb.WaitForAction();
						sb.WaitForPickUp();
						selStateHandler.Deactivate();

						sb.PickUp();

						mockHoverable.Received().OnHoverEnter();
					}
					[Test]
					public void PickUp_SSMNotNullAndValidPrevActState_CallsSSMInSequence(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ITransactionCache mockTAC = sb.taCache;
							ISlotSystemManager ssm = sb.GetSSM();
							ITAMActStateHandler tamStateHandler = ssm.tam.actStateHandler;
							ITransactionIconHandler mockIconHandler = ssm.tam.iconHandler;
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
								selStateHandler.isFocused.Returns(true);
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
							mockCorRepo.GetPickUpCoroutine().Returns(mockPickUpCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
								selStateHandler.isFocused.Returns(true);
							sb.SetSelStateHandler(selStateHandler);
						sb.WaitForAction();
						sb.WaitForPickUp();
						
						sb.PickUp();

						AssertSBActProcessIsSetAndRunning(sb, typeof(PickUpProcess), mockPickUpCoroutine);
					}
					[Test]
					public void InPickingUp_OnDeselected_Always_CallsRefresh(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
							selStateHandler.isFocused.Returns(true);
						sb.SetSelStateHandler(selStateHandler);
						SetUpForRefreshCall(sb);
						sb.SetSlotHandler(new SlotHandler());
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.PickUp();

						sb.OnDeselected(new PointerEventDataFake());

						AssertRefreshCalled(sb);
					}
					[Test]
					public void InPickingUp_OnDeselected_Always_SetsIsFocused(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ISSESelStateHandler selStateHandler = new SBSelStateHandler(sb);
						sb.SetSelStateHandler(selStateHandler);
						SetUpForRefreshCall(sb);
						selStateHandler.Focus();
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.PickUp();

						sb.OnDeselected(new PointerEventDataFake());

						Assert.That(selStateHandler.isFocused, Is.True);
					}
					[Test]
					public void InPickingUp_OnPointerUp_IsHoveredAndIsStackable_SetsIsWaitingForNextTouch(){
						Slottable sb = MakeSBWithRealStateHandlers();
							sb.itemHandler.IsStackable().Returns(true);
							sb.hoverable.isHovered.Returns(true);
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
							selStateHandler.isFocused.Returns(true);
						sb.SetSelStateHandler(selStateHandler);
						SetUpForRefreshCall(sb);
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.PickUp();

						sb.OnPointerUp(new PointerEventDataFake());

						Assert.That(sb.IsWaitingForNextTouch(), Is.True);
					}
					[Test]
					public void InPickingUp_OnPointerUp_NotIsHoveredOrNotIsStackable_CallsSSMExecuteTransaction(){
						Slottable sb = MakeSBWithRealStateHandlers();
							sb.hoverable.isHovered.Returns(false);
						ITransactionManager mockTAM = sb.GetSSM().tam;
						sb.WaitForAction();
						sb.WaitForPickUp();
						sb.PickUp();

						sb.OnPointerUp(new PointerEventDataFake());

						mockTAM.Received().ExecuteTransaction();
						}
					[Test]
					public void InPickingUp_OnEndDrag_Always_CallsSSMExecuteTransaction(){
						Slottable sb = MakeSBWithRealStateHandlers();
							ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
							selStateHandler.isFocused.Returns(true);
						sb.SetSelStateHandler(selStateHandler);
						ITransactionManager mockTAM = sb.GetSSM().tam;
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

						Assert.That(sb.IsMovingWithin(), Is.True);
						}
					[Test]
					public void MoveWithin_Always_SetsAndRunsMoveWithinProcess(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockMoveWithinCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.GetMoveWithinCoroutine().Returns(mockMoveWithinCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);

						sb.MoveWithin();

						AssertSBActProcessIsSetAndRunning(sb, typeof(SBMoveWithinProcess), mockMoveWithinCoroutine);
						}
				/*  AddedState	*/
					[Test]
					public void Add_Always_SetsIsAdding(){
						Slottable sb = MakeSBWithRealStateHandlers();

						sb.Add();

						Assert.That(sb.IsAdding(), Is.True);
						}
					[Test]
					public void Add_Always_SetsAndRunsAddProcess(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockAddCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.GetAddCoroutine().Returns(mockAddCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);

						sb.Add();

						AssertSBActProcessIsSetAndRunning(sb, typeof(SBAddProcess), mockAddCoroutine);
						}
				/*  RemovedState	*/
					[Test]
					public void Remove_Always_SetsIsRemoving(){
						Slottable sb = MakeSBWithRealStateHandlers();

						sb.Remove();

						Assert.That(sb.IsRemoving(), Is.True);
						}
					[Test]
					public void Remove_Always_SetsAndRunsRemovProcess(){
						Slottable sb = MakeSBWithRealStateHandlers();
							System.Func<IEnumeratorFake> mockRemoveCoroutine = Substitute.For<System.Func<IEnumeratorFake>>();
							ISBActCoroutineRepo mockCorRepo = Substitute.For<ISBActCoroutineRepo>();
							mockCorRepo.GetRemoveCoroutine().Returns(mockRemoveCoroutine);
							sb.SetActCoroutineRepo(mockCorRepo);

						sb.Remove();

						AssertSBActProcessIsSetAndRunning(sb, typeof(SBRemoveProcess), mockRemoveCoroutine);
						}
				/* Sequence */
					[Test]
					public void ActStates_EventSequence(){
						Slottable sb = MakeSBWithRealStateHandlers();
								PartsInstance parts = MakePartsInstance(0, 20);
							sb.itemHandler.GetItem().Returns(parts);
							ITransactionCache mockTAC = sb.taCache;
							ITransactionManager mockTAM = sb.GetSSM().tam;
							ISBCommand mockTapComm = sb.tapCommand;
							ISSESelStateHandler selStateHandler = new SBSelStateHandler(sb);
							sb.SetSelStateHandler(selStateHandler);
							
						PointerEventDataFake eventData = new PointerEventDataFake();
						//focused !(stackable && hovered) WFA_down WFPickUp_exp PickingUp_up execTA
								selStateHandler.Focus();
								mockTAC.pickedSB.Returns((ISlotSystemElement)null);
								mockTAC.hovered.Returns((IHoverable)null);
							sb.WaitForAction();
								Assert.That(sb.IsWaitingForAction(), Is.True);
							
							sb.OnPointerDown(eventData);
								Assert.That(sb.IsWaitingForPickUp(), Is.True);
							
							sb.ExpireActProcess();
								Assert.That(sb.IsPickingUp(), Is.True);
								mockTAC.hovered.Returns((IHoverable)null);
							
							sb.OnPointerUp(eventData);
								mockTAM.Received(1).ExecuteTransaction();
						//focused stackable && hovered WFA_down WFPickUp_exp PickingUp_up WFNT_exp execTA
								selStateHandler.Focus();
								// sb.itemHandler.item.Returns(parts);
								sb.itemHandler.IsStackable().Returns(true);
								mockTAC.pickedSB.Returns((ISlotSystemElement)null);
								mockTAC.hovered.Returns((IHoverable)null);
							sb.WaitForAction();

							sb.OnPointerDown(eventData);
								Assert.That(sb.IsWaitingForPickUp(), Is.True);
							
							sb.ExpireActProcess();
								Assert.That(sb.IsPickingUp(), Is.True);
								mockTAC.hovered.Returns(sb.hoverable);
								mockTAC.pickedSB.Returns(sb);

								sb.hoverable.isHovered.Returns(true);
							sb.OnPointerUp(eventData);
								Assert.That(sb.IsWaitingForNextTouch(), Is.True);
							
							sb.ExpireActProcess();
								mockTAM.Received(2).ExecuteTransaction();
						//focused stackable && hovered WFA_down WFPickUp_exp PickingUp_up WFNT_down increment
								selStateHandler.Focus();
								sb.itemHandler.GetItem().Returns(parts);
								mockTAC.pickedSB.Returns((ISlotSystemElement)null);
								mockTAC.hovered.Returns((IHoverable)null);
							sb.WaitForAction();

							sb.OnPointerDown(eventData);
								Assert.That(sb.IsWaitingForPickUp(), Is.True);
							
							sb.ExpireActProcess();
								Assert.That(sb.IsPickingUp(), Is.True);
								// Assert.That(sb.GetPickedAmount(), Is.EqualTo(1));
								sb.itemHandler.Received().SetPickedAmount(1);
								mockTAC.pickedSB.Returns(sb);
								mockTAC.hovered.Returns(sb.hoverable);
							
							sb.OnPointerUp(eventData);
								Assert.That(sb.IsWaitingForNextTouch(), Is.True);
							
							sb.OnPointerDown(eventData);
								sb.itemHandler.Received().SetPickedAmount(sb.itemHandler.GetPickedAmount() + 1);
						//defocused WFA_down WFPointerUp_up tap
								selStateHandler.Defocus();
								mockTAC.pickedSB.Returns((ISlotSystemElement)null);
								mockTAC.hovered.Returns((IHoverable)null);
							sb.WaitForAction();
								Assert.That(selStateHandler.isDefocused, Is.True);
								Assert.That(sb.IsWaitingForAction(), Is.True);
							
							sb.OnPointerDown(eventData);
								Assert.That(sb.IsWaitingForPointerUp(), Is.True);
							
							sb.OnPointerUp(eventData);
								mockTapComm.Received(1).Execute(sb);
						//focused WFA_down WFPickUp_up WFNT_down PickingUp pickedAmount 1
								selStateHandler.Focus();
								mockTAC.pickedSB.Returns((ISlotSystemElement)null);
								mockTAC.hovered.Returns((IHoverable)null);
							sb.WaitForAction();

							sb.OnPointerDown(eventData);
								Assert.That(sb.IsWaitingForPickUp(), Is.True);
							
							sb.OnPointerUp(eventData);
								Assert.That(sb.IsWaitingForNextTouch(), Is.True);
							
							sb.OnPointerDown(eventData);
								Assert.That(sb.IsPickingUp(), Is.True);
								sb.itemHandler.Received().SetPickedAmount(1);
					}
			/* EqpStates */
				/* EquippedState */
					
		/* helpers */

			PointerEventDataFake eventData{get{return new PointerEventDataFake();}}
			void AssertRefreshCalled(ISlottable sb){
				Assert.That(sb.IsWaitingForAction(), Is.True);
				Assert.That(sb.GetPickedAmount(), Is.EqualTo(0));
				Assert.That(sb.GetNewSlotID(), Is.EqualTo(-2));
			}
			void SetUpForRefreshCall(ISlottable sb){
				sb.SetPickedAmount(20);
				sb.SetNewSlotID(6);
			}
			public void AssertSBActProcessIsSetAndRunning(ISlottable sb, Type procType, System.Func<IEnumeratorFake> coroutine){
				ISBActProcess actualProc = sb.GetActProcess();
				Assert.That(actualProc, Is.TypeOf(procType));
				Assert.That(actualProc.IsRunning(), Is.True);
				coroutine.Received().Invoke();
			}
		}
	}
}

