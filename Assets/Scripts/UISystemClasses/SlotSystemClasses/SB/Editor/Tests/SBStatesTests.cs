using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UISystem;
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
								sb.ActStateHandler().Returns(actStateHandler);
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
								sb.ActStateHandler().Returns(actStateHandler);
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
								sb.ActStateHandler().Returns(actStateHandler);
								IUISelStateHandler selStateHandler = Substitute.For<IUISelStateHandler>();
								selStateHandler.IsSelectable().Returns(true);
								sb.UISelStateHandler().Returns(selStateHandler);
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
							sb.ActStateHandler().Returns(actStateHandler);
						wfpuState = new WaitForPickUpState(sb);

						wfpuState.EnterState();
					}
					[Test]
					public void WaitForPickUpState_EnterState_WasWaitingForAction_Calls_SetsAndRunsWFPickUpProc(){
						WaitForPickUpState wfpuState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								actStateHandler.WasWaitingForAction().Returns(true);
							sb.ActStateHandler().Returns(actStateHandler);
						wfpuState = new WaitForPickUpState(sb);

						wfpuState.EnterState();

						actStateHandler.Received().SetAndRunActProcess(Arg.Any<WaitForPickUpProcess>());
					}
					[Test]
					public void WaitForPickUpState_OnPointerUp_Always_Calls_WaitForNetTouch(){
						WaitForPickUpState wfpuState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
							sb.ActStateHandler().Returns(actStateHandler);
						wfpuState = new WaitForPickUpState(sb);

						wfpuState.OnPointerUp();

						actStateHandler.Received().WaitForNextTouch();
					}
					[Test]
					public void WaitForPickUpState_OnEndDrag_Always_Calls_Refresh_Focus(){
						WaitForPickUpState wfpuState;
							ISlottable sb = MakeSubSB();
								IUISelStateHandler selStateHandler = Substitute.For<IUISelStateHandler>();
							sb.UISelStateHandler().Returns(selStateHandler);
						wfpuState = new WaitForPickUpState(sb);

						wfpuState.OnEndDrag();

						Received.InOrder(()=>{
							sb.Refresh();
							selStateHandler.MakeSelectable();
						});
					}
				/* WaitForPointerUpState */
					[Test][ExpectedException(typeof(InvalidOperationException))]
					public void WaitForPointerUpState_EnterState_WasNotWaitingForAction_ThrowsException(){
						WaitForPointerUpState wfpuState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								actStateHandler.WasWaitingForAction().Returns(false);
							sb.ActStateHandler().Returns(actStateHandler);
						wfpuState = new WaitForPointerUpState(sb);

						wfpuState.EnterState();
					}
					[Test]
					public void WaitForPointerUp_EnterState_WasWaitingForAction_Calls_SetAndRunWFFPTUProc(){
						WaitForPointerUpState wfptuState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								actStateHandler.WasWaitingForAction().Returns(true);
							sb.ActStateHandler().Returns(actStateHandler);
						wfptuState = new WaitForPointerUpState(sb);
						
						wfptuState.EnterState();

						actStateHandler.Received().SetAndRunActProcess(Arg.Any<WaitForPointerUpProcess>());
					}
					[Test]
					public void WaitForPointerUpState_OnPointerUp_Always_Calls_Tap_Refresh_Defocus(){
						WaitForPointerUpState wfptuState;
							ISlottable sb = MakeSubSB();
								IUISelStateHandler selStateHandler = Substitute.For<IUISelStateHandler>();
							sb.UISelStateHandler().Returns(selStateHandler);
						wfptuState = new WaitForPointerUpState(sb);

						wfptuState.OnPointerUp();

						Received.InOrder(()=>{
							sb.Tap();
							sb.Refresh();
							selStateHandler.MakeUnselectable();
						});
					}
					[Test]
					public void WaitForPointerUpState_OnEndDrag_Always_Calls_Refresh_Defocus(){
						WaitForPointerUpState wfptuState;
							ISlottable sb = MakeSubSB();
								IUISelStateHandler selStateHandler = Substitute.For<IUISelStateHandler>();
							sb.UISelStateHandler().Returns(selStateHandler);
						wfptuState = new WaitForPointerUpState(sb);

						wfptuState.OnEndDrag();

						Received.InOrder(()=>{
							sb.Refresh();
							selStateHandler.MakeUnselectable();
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
							sb.ActStateHandler().Returns(sbActStateHandler);
						wfntState = new WaitForNextTouchState(sb, MakeSubTAM());

						wfntState.EnterState();
					}
					[Test]
					public void WaitForNextTouchState_EnterState_WasPickingUp_Calls_SetAndRunWFNTProc(){
						WaitForNextTouchState wfntState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler sbActStateHandler = Substitute.For<ISBActStateHandler>();
								sbActStateHandler.WasPickingUp().Returns(true);
							sb.ActStateHandler().Returns(sbActStateHandler);
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
							sb.ActStateHandler().Returns(sbActStateHandler);
						wfntState = new WaitForNextTouchState(sb, MakeSubTAM());						
						
						wfntState.EnterState();

						sbActStateHandler.Received().SetAndRunActProcess(Arg.Any<WaitForNextTouchProcess>());
					}
					[Test]
					public void WaitForNextTouchState_OnPointerDown_IsNotPickedUp_Calls_PickUP(){
						WaitForNextTouchState wfntState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler sbActStateHandler = Substitute.For<ISBActStateHandler>();
								sb.ActStateHandler().Returns(sbActStateHandler);
							sb.IsPickedUp().Returns(false);
						wfntState = new WaitForNextTouchState(sb, MakeSubTAM());						
						
						wfntState.OnPointerDown();

						sbActStateHandler.Received().PickUp();
					}
					[Test]
					public void WaitForNextTouchState_OnPointerDown_IsPickedUp_Calls_Increment(){
						WaitForNextTouchState wfntState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler sbActStateHandler = Substitute.For<ISBActStateHandler>();
								sb.ActStateHandler().Returns(sbActStateHandler);
							sb.IsPickedUp().Returns(true);
						wfntState = new WaitForNextTouchState(sb, MakeSubTAM());						
						
						wfntState.OnPointerDown();

						sb.Received().Increment();
					}
					[Test]
					public void WaitForNextTouchState_OnDeselected_Always_Calls_Refresh_Focus(){
						WaitForNextTouchState wfntState;
							ISlottable sb = MakeSubSB();
								IUISelStateHandler selStateHandler = Substitute.For<IUISelStateHandler>();
								sb.UISelStateHandler().Returns(selStateHandler);
						wfntState = new WaitForNextTouchState(sb, MakeSubTAM());						
						
						wfntState.OnDeselected();

						sb.Received().Refresh();
						selStateHandler.Received().MakeSelectable();
					}
					[Test]
					public void PickingUpState_EnterState_InvalidPrevState_ThrowsException(){
						PickingUpState puState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								actStateHandler.WasWaitingForPickUp().Returns(false);
								actStateHandler.WasWaitingForNextTouch().Returns(false);
								sb.ActStateHandler().Returns(actStateHandler);
							puState = new PickingUpState(sb, MakeSubTAM());
						
						Exception ex = Assert.Catch<InvalidOperationException>(()=> puState.EnterState());

						Assert.That(ex.Message, Is.StringContaining("cannot enter this state from anything other than WaitForPickUpState or WaitForNextTouchState"));
					}
					[Test]
					public void PickingUpState_EnterState_WasWaitingForPickUp_Calls_Various(){
						PickingUpState puState;
							ISlottable sb = MakeSubSB();
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
									actStateHandler.WasWaitingForPickUp().Returns(true);
								sb.ActStateHandler().Returns(actStateHandler);
								IHoverable sbHoverable = Substitute.For<IHoverable>();
								sb.GetHoverable().Returns(sbHoverable);
								ITransactionCache taCache = MakeSubTAC();
								sb.GetTAC().Returns(taCache);
								ITransactionManager tam = MakeSubTAM();
									ITransactionIconHandler iconHandler = Substitute.For<ITransactionIconHandler>();
									tam.GetIconHandler().Returns(iconHandler);
									ITAMActStateHandler tamStateHandler = Substitute.For<ITAMActStateHandler>();
									tam.GetActStateHandler().Returns(tamStateHandler);
							puState = new PickingUpState(sb, tam);
						
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
								sb.ActStateHandler().Returns(actStateHandler);
								IHoverable sbHoverable = Substitute.For<IHoverable>();
								sb.GetHoverable().Returns(sbHoverable);
								ITransactionCache taCache = MakeSubTAC();
								sb.GetTAC().Returns(taCache);
								ITransactionManager tam = MakeSubTAM();
									ITransactionIconHandler iconHandler = Substitute.For<ITransactionIconHandler>();
									tam.GetIconHandler().Returns(iconHandler);
									ITAMActStateHandler tamStateHandler = Substitute.For<ITAMActStateHandler>();
									tam.GetActStateHandler().Returns(tamStateHandler);
							puState = new PickingUpState(sb, tam);
						
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
								IUISelStateHandler selStateHandler = Substitute.For<IUISelStateHandler>();
								sb.UISelStateHandler().Returns(selStateHandler);
							puState = new PickingUpState(sb, MakeSubTAM());
						
						puState.OnDeselected();

						Received.InOrder(()=> {
							sb.Refresh();
							selStateHandler.MakeSelectable();
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
								sb.ItemHandler().Returns(itemHandler);
								ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
								sb.ActStateHandler().Returns(actStateHandler);
							puState = new PickingUpState(sb, MakeSubTAM());
						
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
								sb.ItemHandler().Returns(itemHandler);
								ITransactionManager tam = MakeSubTAM();
							puState = new PickingUpState(sb, tam);
						
						puState.OnPointerUp();

						tam.Received().ExecuteTransaction();
					}
					[Test]
					public void PickingUpState_OnEndDrag_Always_Calls_ExecuteTransaction(){
						PickingUpState puState;
							ITransactionManager tam = MakeSubTAM();
							puState = new PickingUpState(MakeSubSB(), tam);
						
						puState.OnEndDrag();

						tam.Received().ExecuteTransaction();
					}
					[Test]
					public void SBAddedState_EnterState_Always_Calls_SetAndRunSBAddProc(){
						SBAppearingState addedState;
							ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
							addedState = new SBAppearingState(actStateHandler);
						
						addedState.EnterState();

						actStateHandler.Received().SetAndRunActProcess(Arg.Any<SBAppearProcess>());
					}
					[Test]
					public void SBRemovedState_EnterState_Always_Calls_SetAndRunSBRemoveProc(){
						SBDisappearingState addedState;
							ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
							addedState = new SBDisappearingState(actStateHandler);
						
						addedState.EnterState();

						actStateHandler.Received().SetAndRunActProcess(Arg.Any<SBDisappearProcess>());
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
								ISBEqpStateHandler eqpStateHandler = Substitute.For<ISBEqpStateHandler>();
								ISBEquipToolHandler sbEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
									sbEquipToolHandler.GetEqpStateHandler().Returns(eqpStateHandler);
								sb.GetToolHandler().Returns(sbEquipToolHandler);
							equipState = new SBEquippedState(sb);
						
						equipState.EnterState();

						eqpStateHandler.DidNotReceive().SetAndRunEqpProcess(Arg.Any<SBEquipProcess>());
					}
					[Test]
					public void SBEquippedState_EnterState_IsHierarchySetUpAndIsNotPool_DoesNotCall_SetAndRunSBEquipProc(){
						SBEquippedState equipState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								ISBEquipToolHandler sbEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
									sbEquipToolHandler.IsPool().Returns(false);
									ISBEqpStateHandler eqpStateHandler = Substitute.For<ISBEqpStateHandler>();
									sbEquipToolHandler.GetEqpStateHandler().Returns(eqpStateHandler);
								sb.GetToolHandler().Returns(sbEquipToolHandler);
							equipState = new SBEquippedState(sb);
						
						equipState.EnterState();

						eqpStateHandler.DidNotReceive().SetAndRunEqpProcess(Arg.Any<SBEquipProcess>());
					}
					[Test]
					public void SBEquippedState_EnterState_IsHierarchySetUpAndIsPoolAndIsNotUnequipped_DoesNotCall_SetAndRunSBEquipProc(){
						SBEquippedState equipState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								ISBEquipToolHandler sbEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
								sbEquipToolHandler.IsPool().Returns(true);
									ISBEqpStateHandler eqpStateHandler = Substitute.For<ISBEqpStateHandler>();
										eqpStateHandler.IsUnequipped().Returns(false);
									sbEquipToolHandler.GetEqpStateHandler().Returns(eqpStateHandler);
								sb.GetToolHandler().Returns(sbEquipToolHandler);
							equipState = new SBEquippedState(sb);

						equipState.EnterState();

						eqpStateHandler.DidNotReceive().SetAndRunEqpProcess(Arg.Any<SBEquipProcess>());
					}
					[Test]
					public void SBEquippedState_EnterState_IsHierarchySetUpAndIsPoolAndIsUnequipped_Calls_SetAndRunSBEquipProc(){
						SBEquippedState equipState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								ISBEquipToolHandler sbEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
								sbEquipToolHandler.IsPool().Returns(true);
									ISBEqpStateHandler eqpStateHandler = Substitute.For<ISBEqpStateHandler>();
										eqpStateHandler.IsUnequipped().Returns(true);
									sbEquipToolHandler.GetEqpStateHandler().Returns(eqpStateHandler);
								sb.GetToolHandler().Returns(sbEquipToolHandler);
							equipState = new SBEquippedState(sb);

						equipState.EnterState();

						eqpStateHandler.Received().SetAndRunEqpProcess(Arg.Any<SBEquipProcess>());
					}
					[Test]
					public void SBUnequippedState_EnterState_IsNotHierarchySetUp_DoesNotCall_SetAndRunSBUnequipProc(){
						SBUnequippedState unequipState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(false);
								ISBEquipToolHandler sbEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
									ISBEqpStateHandler eqpStateHandler = Substitute.For<ISBEqpStateHandler>();
									sbEquipToolHandler.GetEqpStateHandler().Returns(eqpStateHandler);
								sb.GetToolHandler().Returns(sbEquipToolHandler);
							unequipState = new SBUnequippedState(sb);

						unequipState.EnterState();

						eqpStateHandler.DidNotReceive().SetAndRunEqpProcess(Arg.Any<SBUnequipProcess>());
					}
					[Test]
					public void SBUnequippedState_EnterState_IsHierarchySetUpAndIsNotPool_DoesNotCall_SetAndRunSBUnequipProc(){
						SBUnequippedState unequipState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								ISBEquipToolHandler sbEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
								sbEquipToolHandler.IsPool().Returns(false);
									ISBEqpStateHandler eqpStateHandler = Substitute.For<ISBEqpStateHandler>();
									sbEquipToolHandler.GetEqpStateHandler().Returns(eqpStateHandler);
								sb.GetToolHandler().Returns(sbEquipToolHandler);
							unequipState = new SBUnequippedState(sb);

						unequipState.EnterState();

						eqpStateHandler.DidNotReceive().SetAndRunEqpProcess(Arg.Any<SBUnequipProcess>());
					}
					[Test]
					public void SBUnequippedState_EnterState_IsHierarchySetUpAndIsPoolAndIsNotEquipped_DoesNotCall_SetAndRunSBUnequipProc(){
						SBUnequippedState unequipState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								ISBEquipToolHandler sbEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
								sbEquipToolHandler.IsPool().Returns(true);
									ISBEqpStateHandler eqpStateHandler = Substitute.For<ISBEqpStateHandler>();
										eqpStateHandler.IsEquipped().Returns(false);
									sbEquipToolHandler.GetEqpStateHandler().Returns(eqpStateHandler);
								sb.GetToolHandler().Returns(sbEquipToolHandler);
							unequipState = new SBUnequippedState(sb);

						unequipState.EnterState();

						eqpStateHandler.DidNotReceive().SetAndRunEqpProcess(Arg.Any<SBUnequipProcess>());
					}
					[Test]
					public void SBUnequippedState_EnterState_IsHierarchySetUpAndIsPoolAndIsEquipped_Calls_SetAndRunSBUnequipProc(){
						SBUnequippedState unequipState;
							ISlottable sb = MakeSubSB();
								sb.IsHierarchySetUp().Returns(true);
								ISBEquipToolHandler sbEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
								sbEquipToolHandler.IsPool().Returns(true);
									ISBEqpStateHandler eqpStateHandler = Substitute.For<ISBEqpStateHandler>();
										eqpStateHandler.IsEquipped().Returns(true);
									sbEquipToolHandler.GetEqpStateHandler().Returns(eqpStateHandler);
								sb.GetToolHandler().Returns(sbEquipToolHandler);
							unequipState = new SBUnequippedState(sb);

						unequipState.EnterState();

						eqpStateHandler.Received().SetAndRunEqpProcess(Arg.Any<SBUnequipProcess>());
					}
		}
	}
}
