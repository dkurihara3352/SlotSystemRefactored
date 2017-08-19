using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using Utility;
namespace SlotSystemTests{
	namespace SlottableTests{
		[TestFixture]
		public class SBStatesTests: SlotSystemTest {
			/* ActStates */
				/* WaitForActionState */
					[Test]
					public void WaitForActionState_EnterState_WasActStateNull_DoesNotCallASHSetAndRunActStateNull(){
						WaitForActionState wfaState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								actStateHandler.WasActStateNull().Returns(true);
								sb.GetActStateHandler().Returns(actStateHandler);
							wfaState = new WaitForActionState(sb);

						wfaState.EnterState();

						actStateHandler.DidNotReceive().SetAndRunActProcess((ISBActProcess)null);
					}
					[Test]
					public void WaitForActionState_EnterState_WasNotActStateNull_CallsASHSetAndRunActProcessNull(){
						WaitForActionState wfaState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								actStateHandler.WasActStateNull().Returns(false);
								sb.GetActStateHandler().Returns(actStateHandler);
							wfaState = new WaitForActionState(sb);

						wfaState.EnterState();

						actStateHandler.Received().SetAndRunActProcess((ISBActProcess)null);
					}
					[Test]
					public void WaitForActionState_OnPointerDown_IsFocused_CallsDelegatesInTurn(){
						WaitForActionState wfaState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								actStateHandler.WasActStateNull().Returns(true);
								sb.GetActStateHandler().Returns(actStateHandler);
								ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
								selStateHandler.IsFocused().Returns(true);
								sb.GetSelStateHandler().Returns(selStateHandler);
							wfaState = new WaitForActionState(sb);

						wfaState.OnPointerDown();

						Received.InOrder(() => {
							selStateHandler.Select();
							actStateHandler.WaitForPickUp();
						});
					}
				/* WaitForPickUp */
					[Test][ExpectedException(typeof(InvalidOperationException))]
					public void WaitForPickUpState_EnterState_WasNotWaitingForAction_ThrowsException(){
						WaitForPickUpState wfpuState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								actStateHandler.WasWaitingForAction().Returns(false);
							sb.GetActStateHandler().Returns(actStateHandler);
						wfpuState = new WaitForPickUpState(sb);

						wfpuState.EnterState();
					}
					[Test]
					public void WaitForPickUpState_EnterState_WasWaitingForAction_Calls_SetsAndRunsWFPickUpProc(){
						WaitForPickUpState wfpuState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								actStateHandler.WasWaitingForAction().Returns(true);
							sb.GetActStateHandler().Returns(actStateHandler);
						wfpuState = new WaitForPickUpState(sb);

						wfpuState.EnterState();

						actStateHandler.Received().SetAndRunActProcess(Arg.Any<WaitForPickUpProcess>());
					}
					[Test]
					public void WaitForPickUpState_OnPointerUp_Always_Calls_WaitForNetTouch(){
						WaitForPickUpState wfpuState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
							sb.GetActStateHandler().Returns(actStateHandler);
						wfpuState = new WaitForPickUpState(sb);

						wfpuState.OnPointerUp();

						actStateHandler.Received().WaitForNextTouch();
					}
					[Test]
					public void WaitForPickUpState_OnEndDrag_Always_Calls_Refresh_Focus(){
						WaitForPickUpState wfpuState;
							ISlottable sb = MakeSubSB();
								ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
							sb.GetSelStateHandler().Returns(selStateHandler);
						wfpuState = new WaitForPickUpState(sb);

						wfpuState.OnEndDrag();

						Received.InOrder(()=>{
							sb.Refresh();
							selStateHandler.Focus();
						});
					}
				/* WaitForPointerUpState */
					[Test][ExpectedException(typeof(InvalidOperationException))]
					public void WaitForPointerUpState_EnterState_WasNotWaitingForAction_ThrowsException(){
						WaitForPointerUpState wfpuState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								actStateHandler.WasWaitingForAction().Returns(false);
							sb.GetActStateHandler().Returns(actStateHandler);
						wfpuState = new WaitForPointerUpState(sb);

						wfpuState.EnterState();
					}
					[Test]
					public void WaitForPointerUp_EnterState_WasWaitingForAction_Calls_SetAndRunWFFPTUProc(){
						WaitForPointerUpState wfptuState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								actStateHandler.WasWaitingForAction().Returns(false);
							sb.GetActStateHandler().Returns(actStateHandler);
						wfptuState = new WaitForPointerUpState(sb);
						
						wfptuState.EnterState();

						actStateHandler.Received().SetAndRunActProcess(Arg.Any<WaitForPointerUpProcess>());
					}
					[Test]
					public void WaitForPointerUpState_OnPointerUp_Always_Calls_Tap_Refresh_Defocus(){
						WaitForPointerUpState wfptuState;
							ISlottable sb = MakeSubSB();
								ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
							sb.GetSelStateHandler().Returns(selStateHandler);
						wfptuState = new WaitForPointerUpState(sb);

						wfptuState.OnPointerUp();

						Received.InOrder(()=>{
							sb.Tap();
							sb.Refresh();
							selStateHandler.Defocus();
						});
					}
					[Test]
					public void WaitForPointerUpState_OnEndDrag_Always_Calls_Refresh_Defocus(){
						WaitForPointerUpState wfptuState;
							ISlottable sb = MakeSubSB();
								ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
							sb.GetSelStateHandler().Returns(selStateHandler);
						wfptuState = new WaitForPointerUpState(sb);

						wfptuState.OnEndDrag();

						Received.InOrder(()=>{
							sb.Refresh();
							selStateHandler.Defocus();
						});
					}
				/* WaitForNextTouchState */
					[Test][ExpectedException(typeof(InvalidOperationException))]
					public void WaitForNextTouchState_EnterState_InvalidPrevActStates_ThrowsException(){
						WaitForNextTouchState wfntState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler sbActStateHandler = Substitute.For<ISBActStateHandler>();
								sbActStateHandler.WasPickingUp().Returns(false);
								sbActStateHandler.WasWaitingForPickUp().Returns(false);
							sb.GetActStateHandler().Returns(sbActStateHandler);
						wfntState = new WaitForNextTouchState(sb, MakeSubTAM());

						wfntState.EnterState();
					}
					[Test]
					public void WaitForNextTouchState_EnterState_WasPickingUp_Calls_SetAndRunWFNTProc(){
						WaitForNextTouchState wfntState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler sbActStateHandler = Substitute.For<ISBActStateHandler>();
								sbActStateHandler.WasPickingUp().Returns(true);
							sb.GetActStateHandler().Returns(sbActStateHandler);
						wfntState = new WaitForNextTouchState(sb, MakeSubTAM());						
						
						wfntState.EnterState();

						sbActStateHandler.Received().SetAndRunActProcess(Arg.Any<WaitForNextTouchProcess>());
					}
					[Test]
					public void WaitForNextTouchState_EnterState_WasWaitingForPickUp_Calls_SetAndRunWFNTProc(){
						WaitForNextTouchState wfntState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler sbActStateHandler = Substitute.For<ISBActStateHandler>();
								sbActStateHandler.WasWaitingForPickUp().Returns(true);
							sb.GetActStateHandler().Returns(sbActStateHandler);
						wfntState = new WaitForNextTouchState(sb, MakeSubTAM());						
						
						wfntState.EnterState();

						sbActStateHandler.Received().SetAndRunActProcess(Arg.Any<WaitForNextTouchProcess>());
					}
					[Test]
					public void WaitForNextTouchState_OnPointerDown_IsPicked_Calls_PickUP(){
						WaitForNextTouchState wfntState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler sbActStateHandler = Substitute.For<ISBActStateHandler>();
								sb.GetActStateHandler().Returns(sbActStateHandler);
							sb.IsPickedUp().Returns(true);
						wfntState = new WaitForNextTouchState(sb, MakeSubTAM());						
						
						wfntState.OnPointerDown();

						sbActStateHandler.Received().PickUp();
					}
					[Test]
					public void WaitForNextTouchState_OnPointerDown_IsNotPicked_Calls_Increment(){
						WaitForNextTouchState wfntState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler sbActStateHandler = Substitute.For<ISBActStateHandler>();
								sb.GetActStateHandler().Returns(sbActStateHandler);
							sb.IsPickedUp().Returns(false);
						wfntState = new WaitForNextTouchState(sb, MakeSubTAM());						
						
						wfntState.OnPointerDown();

						sb.Received().Increment();
					}
					[Test]
					public void WaitForNextTouchState_OnDeselected_Always_Calls_Refresh_Focus(){
						WaitForNextTouchState wfntState;
							ISlottable sb = MakeSubSB();
								ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
								sb.GetSelStateHandler().Returns(selStateHandler);
						wfntState = new WaitForNextTouchState(sb, MakeSubTAM());						
						
						wfntState.OnPointerDown();

						sb.Received().Refresh();
						selStateHandler.Received().Focus();
					}
					[Test]
					public void PickingUpState_EnterState_InvalidPrevState_ThrowsException(){
						PickingUpState puState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								actStateHandler.WasWaitingForPickUp().Returns(false);
								actStateHandler.WasWaitingForNextTouch().Returns(false);
								sb.GetActStateHandler().Returns(actStateHandler);
							puState = new PickingUpState(sb, MakeSubTAM(), MakeSubTAC());
						
						Exception ex = Assert.Catch<InvalidOperationException>(()=> puState.EnterState());

						Assert.That(ex.Message, Is.StringContaining("cannot enter this state from anything other than WaitForPickUpState or WaitForNextTouchState"));
					}
					[Test]
					public void PickingUpState_EnterState_WasWaitingForPickUp_Calls_Various(){
						PickingUpState puState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
									actStateHandler.WasWaitingForPickUp().Returns(true);
								sb.GetActStateHandler().Returns(actStateHandler);
								IHoverable sbHoverable = Substitute.For<IHoverable>();
								sb.GetHoverable().Returns(sbHoverable);
								ITransactionCache taCache = MakeSubTAC();
								ITransactionManager tam = MakeSubTAM();
									ITransactionIconHandler iconHandler = Substitute.For<ITransactionIconHandler>();
									tam.GetIconHandler().Returns(iconHandler);
									ITAMActStateHandler tamStateHandler = Substitute.For<ITAMActStateHandler>();
									tam.GetActStateHandler().Returns(tamStateHandler);
							puState = new PickingUpState(sb, tam, taCache);
						
						puState.EnterState();

						Received.InOrder(() => {
							sbHoverable.OnHoverEnter();
							taCache.SetPickedSB(sb);
							iconHandler.SetDIcon1(sb);
							tamStateHandler.Probe();
							taCache.CreateTransactionResults();
							taCache.UpdateFields();
							actStateHandler.SetAndRunActProcess(Arg.Any<PickUpProcess>());
						});
					}
					[Test]
					public void PickingUpState_EnterState_WasWaitingForNextTouch_Calls_Various(){
						PickingUpState puState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
									actStateHandler.WasWaitingForNextTouch().Returns(true);
								sb.GetActStateHandler().Returns(actStateHandler);
								IHoverable sbHoverable = Substitute.For<IHoverable>();
								sb.GetHoverable().Returns(sbHoverable);
								ITransactionCache taCache = MakeSubTAC();
								ITransactionManager tam = MakeSubTAM();
									ITransactionIconHandler iconHandler = Substitute.For<ITransactionIconHandler>();
									tam.GetIconHandler().Returns(iconHandler);
									ITAMActStateHandler tamStateHandler = Substitute.For<ITAMActStateHandler>();
									tam.GetActStateHandler().Returns(tamStateHandler);
							puState = new PickingUpState(sb, tam, taCache);
						
						puState.EnterState();

						Received.InOrder(() => {
							sbHoverable.OnHoverEnter();
							taCache.SetPickedSB(sb);
							iconHandler.SetDIcon1(sb);
							tamStateHandler.Probe();
							taCache.CreateTransactionResults();
							taCache.UpdateFields();
							actStateHandler.SetAndRunActProcess(Arg.Any<PickUpProcess>());
						});
					}
					[Test]
					public void PickingUpState_OnDeselected_Always_Calls_Refresh_Focus(){
						PickingUpState puState;
							ISlottable sb = MakeSubSB();
								ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
								sb.GetSelStateHandler().Returns(selStateHandler);
							puState = new PickingUpState(sb, MakeSubTAM(), MakeSubTAC());
						
						puState.OnDeselected();

						Received.InOrder(()=> {
							sb.Refresh();
							selStateHandler.Focus();
						});
					}
					[Test]
					public void PickingUpState_OnPointerUp_IsHoveredAndIsStackable_Calls_WaitForNextTouch(){
						PickingUpState puState;
							ISlottable sb = MakeSubSB();
								IHoverable hoverable = Substitute.For<IHoverable>();
									hoverable.IsHovered().Returns(true);
								sb.GetHoverable().Returns(hoverable);
								IItemHandler itemHandler = Substitute.For<IItemHandler>();
									itemHandler.IsStackable().Returns(true);
								sb.GetItemHandler().Returns(itemHandler);
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								sb.GetActStateHandler().Returns(actStateHandler);
							puState = new PickingUpState(sb, MakeSubTAM(), MakeSubTAC());
						
						puState.OnPointerUp();

						actStateHandler.Received().WaitForNextTouch();
					}
					[Test]
					public void PickingUpState_OnPointerUp_NotIsHoveredAndIsStackable_Calls_ExecuteTransaction(){
						PickingUpState puState;
							ISlottable sb = MakeSubSB();
								IHoverable hoverable = Substitute.For<IHoverable>();
									hoverable.IsHovered().Returns(true);
								sb.GetHoverable().Returns(hoverable);
								IItemHandler itemHandler = Substitute.For<IItemHandler>();
									itemHandler.IsStackable().Returns(false);
								sb.GetItemHandler().Returns(itemHandler);
								ITransactionManager tam = MakeSubTAM();
							puState = new PickingUpState(sb, tam, MakeSubTAC());
						
						puState.OnPointerUp();

						tam.Received().ExecuteTransaction();
					}
					[Test]
					public void PickingUpState_OnEndDrag_Always_Calls_ExecuteTransaction(){
						PickingUpState puState;
							ITransactionManager tam = MakeSubTAM();
							puState = new PickingUpState(MakeSubSB(), tam, MakeSubTAC());
						
						puState.OnEndDrag();

						tam.Received().ExecuteTransaction();
					}
					[Test]
					public void SBAddedState_EnterState_Always_Calls_SetAndRunSBAddProc(){
						SBAddedState addedState;
							ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
							addedState = new SBAddedState(actStateHandler);
						
						addedState.EnterState();

						actStateHandler.Received().SetAndRunActProcess(Arg.Any<SBAddProcess>());
					}
					[Test]
					public void SBRemovedState_EnterState_Always_Calls_SetAndRunSBRemoveProc(){
						SBRemovedState addedState;
							ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
							addedState = new SBRemovedState(actStateHandler);
						
						addedState.EnterState();

						actStateHandler.Received().SetAndRunActProcess(Arg.Any<SBRemoveProcess>());
					}
					[Test]
					public void MoveWithinState_EnterState_Always_Calls_SetAndRunSBMoveWithinProc(){
						MoveWithinState addedState;
							ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
							addedState = new MoveWithinState(actStateHandler);
						
						addedState.EnterState();

						actStateHandler.Received().SetAndRunActProcess(Arg.Any<SBMoveWithinProcess>());
					}
					[Test]
					public void SBEquippedState_EnterState_IsNotHierarchySetUp_DoesNotCall_SetAndRunSBEquipProc(){
						SBEquippedState equipState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(false);
							equipState = new SBEquippedState(sb);

						sb.DidNotReceive().SetAndRunEqpProcess(Arg.Any<SBEquipProcess>());
					}
					[Test]
					public void SBEquippedState_EnterState_IsHierarchySetUpAndIsNotPool_DoesNotCall_SetAndRunSBEquipProc(){
						SBEquippedState equipState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								sb.IsPool().Returns(false);
							equipState = new SBEquippedState(sb);

						sb.DidNotReceive().SetAndRunEqpProcess(Arg.Any<SBEquipProcess>());
					}
					[Test]
					public void SBEquippedState_EnterState_IsHierarchySetUpAndIsPool_Calls_SetAndRunSBEquipProc(){
						SBEquippedState equipState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								sb.IsPool().Returns(true);
							equipState = new SBEquippedState(sb);

						sb.Received().SetAndRunEqpProcess(Arg.Any<SBEquipProcess>());
					}
					[Test]
					public void SBUnequippedState_EnterState_IsNotHierarchySetUp_DoesNotCall_SetAndRunSBUnequipProc(){
						SBUnequippedState unequipState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(false);
							unequipState = new SBUnequippedState(sb);

						sb.DidNotReceive().SetAndRunEqpProcess(Arg.Any<SBUnequipProcess>());
					}
					[Test]
					public void SBUnequippedState_EnterState_IsHierarchySetUpAndIsNotPool_DoesNotCall_SetAndRunSBUnequipProc(){
						SBUnequippedState unequipState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								sb.IsPool().Returns(false);
							unequipState = new SBUnequippedState(sb);

						sb.DidNotReceive().SetAndRunEqpProcess(Arg.Any<SBUnequipProcess>());
					}
					[Test]
					public void SBUnequippedState_EnterState_IsHierarchySetUpAndIsPool_Calls_SetAndRunSBUnequipProc(){
						SBUnequippedState unequipState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								sb.IsPool().Returns(true);
							unequipState = new SBUnequippedState(sb);

						sb.Received().SetAndRunEqpProcess(Arg.Any<SBUnequipProcess>());
					}
					[Test]
					public void SBMarkedState_EnterState_IsNotHierarchySetUp_DoesNotCall_SetAndRunSBMarkProc(){
						SBMarkedState markedState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(false);
							markedState = new SBMarkedState(sb);

						sb.DidNotReceive().SetAndRunMrkProcess(Arg.Any<SBMarkProcess>());
					}
					[Test]
					public void SBMarkedState_EnterState_IsHierarchySetUpAndIsNotPool_DoesNotCall_SetAndRunSBMarkProc(){
						SBMarkedState markedState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								sb.IsPool().Returns(false);
							markedState = new SBMarkedState(sb);

						sb.DidNotReceive().SetAndRunMrkProcess(Arg.Any<SBMarkProcess>());
					}
					[Test]
					public void SBMarkedState_EnterState_IsHierarchySetUpAndIsPool_Calls_SetAndRunSBMarkProc(){
						SBMarkedState markedState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								sb.IsPool().Returns(true);
							markedState = new SBMarkedState(sb);

						sb.Received().SetAndRunMrkProcess(Arg.Any<SBMarkProcess>());
					}
					[Test]
					public void SBUnmarkedState_EnterState_IsNotHierarchySetUp_DoesNotCall_SetAndRunSBUnmarkProc(){
						SBUnmarkedState markedState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(false);
							markedState = new SBUnmarkedState(sb);

						sb.DidNotReceive().SetAndRunMrkProcess(Arg.Any<SBUnmarkProcess>());
					}
					[Test]
					public void SBUnmarkedState_EnterState_IsHierarchySetUpAndIsNotPool_DoesNotCall_SetAndRunSBUnmarkProc(){
						SBUnmarkedState markedState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								sb.IsPool().Returns(false);
							markedState = new SBUnmarkedState(sb);

						sb.DidNotReceive().SetAndRunMrkProcess(Arg.Any<SBUnmarkProcess>());
					}
					[Test]
					public void SBUnmarkedState_EnterState_IsHierarchySetUpAndIsPool_Calls_SetAndRunSBUnmarkProc(){
						SBUnmarkedState markedState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								sb.IsPool().Returns(true);
							markedState = new SBUnmarkedState(sb);

						sb.Received().SetAndRunMrkProcess(Arg.Any<SBUnmarkProcess>());
					}
				/* Sequence */
					[Test]
					public void ActStates_EventSequence(){
						Slottable sb = MakeSB();
								PartsInstance parts = MakePartsInstance(0, 20);
							IItemHandler itemHandler = Substitute.For<IItemHandler>();
								itemHandler.GetItem().Returns(parts);
							IHoverable hoverable = Substitute.For<IHoverable>();
								ITransactionCache mockTAC = Substitute.For<ITransactionCache>();
								hoverable.GetTAC().Returns(mockTAC);
							sb.SetHoverable(hoverable);
							sb.SetItemHandler(itemHandler);
							ISlotSystemManager ssm = Substitute.For<ISlotSystemManager>();
								ITransactionManager mockTAM = Substitute.For<ITransactionManager>();
								ssm.GetTAM().Returns(mockTAM);
							sb.SetSSM(ssm);
							ISBCommand mockTapComm = Substitute.For<ISBCommand>();
							sb.SetTapCommand(mockTapComm);
							ISSESelStateHandler selStateHandler = new SBSelStateHandler(sb);
							sb.SetSelStateHandler(selStateHandler);
							ISBActStateHandler actStateHandler = new SBActStateHandler(sb, mockTAM);
							sb.SetActStateHandler(actStateHandler);
							ISlotHandler slotHandler = Substitute.For<ISlotHandler>();
							sb.SetSlotHandler(slotHandler);
						PointerEventDataFake eventData = new PointerEventDataFake();
						//focused !(stackable && hovered) WFA_down WFPickUp_exp PickingUp_up execTA
								selStateHandler.Focus();
								mockTAC.GetPickedSB().Returns((ISlotSystemElement)null);
								mockTAC.GetHovered().Returns((IHoverable)null);
							sb.WaitForAction();
								Assert.That(sb.IsWaitingForAction(), Is.True);
							
							sb.OnPointerDown(eventData);
								Assert.That(sb.IsWaitingForPickUp(), Is.True);
							
							sb.ExpireActProcess();
								Assert.That(sb.IsPickingUp(), Is.True);
								mockTAC.GetHovered().Returns((IHoverable)null);
							
							sb.OnPointerUp(eventData);
								mockTAM.Received(1).ExecuteTransaction();
						//focused stackable && hovered WFA_down WFPickUp_exp PickingUp_up WFNT_exp execTA
								selStateHandler.Focus();
								// sb.itemHandler.item.Returns(parts);
								itemHandler.IsStackable().Returns(true);
								mockTAC.GetPickedSB().Returns((ISlotSystemElement)null);
								mockTAC.GetHovered().Returns((IHoverable)null);
							sb.WaitForAction();

							sb.OnPointerDown(eventData);
								Assert.That(sb.IsWaitingForPickUp(), Is.True);
							
							sb.ExpireActProcess();
								Assert.That(sb.IsPickingUp(), Is.True);
								mockTAC.GetHovered().Returns(sb.GetHoverable());
								mockTAC.GetPickedSB().Returns(sb);

								sb.GetHoverable().IsHovered().Returns(true);
							sb.OnPointerUp(eventData);
								Assert.That(sb.IsWaitingForNextTouch(), Is.True);
							
							sb.ExpireActProcess();
								mockTAM.Received(2).ExecuteTransaction();
						//focused stackable && hovered WFA_down WFPickUp_exp PickingUp_up WFNT_down increment
								selStateHandler.Focus();
								itemHandler.GetItem().Returns(parts);
								mockTAC.GetPickedSB().Returns((ISlotSystemElement)null);
								mockTAC.GetHovered().Returns((IHoverable)null);
							sb.WaitForAction();

							sb.OnPointerDown(eventData);
								Assert.That(sb.IsWaitingForPickUp(), Is.True);
							
							sb.ExpireActProcess();
								Assert.That(sb.IsPickingUp(), Is.True);
								// Assert.That(sb.GetPickedAmount(), Is.EqualTo(1));
								itemHandler.Received().SetPickedAmount(1);
								mockTAC.GetPickedSB().Returns(sb);
								mockTAC.GetHovered().Returns(sb.GetHoverable());
							
							sb.OnPointerUp(eventData);
								Assert.That(sb.IsWaitingForNextTouch(), Is.True);
							
							sb.OnPointerDown(eventData);
								itemHandler.Received().SetPickedAmount(itemHandler.GetPickedAmount() + 1);
						//defocused WFA_down WFPointerUp_up tap
								selStateHandler.Defocus();
								mockTAC.GetPickedSB().Returns((ISlotSystemElement)null);
								mockTAC.GetHovered().Returns((IHoverable)null);
							sb.WaitForAction();
								Assert.That(selStateHandler.IsDefocused(), Is.True);
								Assert.That(sb.IsWaitingForAction(), Is.True);
							
							sb.OnPointerDown(eventData);
								Assert.That(sb.IsWaitingForPointerUp(), Is.True);
							
							sb.OnPointerUp(eventData);
								mockTapComm.Received(1).Execute(sb);
						//focused WFA_down WFPickUp_up WFNT_down PickingUp pickedAmount 1
								selStateHandler.Focus();
								mockTAC.GetPickedSB().Returns((ISlotSystemElement)null);
								mockTAC.GetHovered().Returns((IHoverable)null);
							sb.WaitForAction();

							sb.OnPointerDown(eventData);
								Assert.That(sb.IsWaitingForPickUp(), Is.True);
							
							sb.OnPointerUp(eventData);
								Assert.That(sb.IsWaitingForNextTouch(), Is.True);
							
							sb.OnPointerDown(eventData);
								Assert.That(sb.IsPickingUp(), Is.True);
								itemHandler.Received().SetPickedAmount(1);
					}
					

		}
	}
}
