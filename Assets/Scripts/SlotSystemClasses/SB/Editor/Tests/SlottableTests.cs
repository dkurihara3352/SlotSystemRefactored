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
		public class SlottableTests: SlotSystemTest {			
			/*	Fields	*/
				[Test]
				public void isHierarchySetUp_SGIsSet_ReturnsTrue(){
					Slottable sb = MakeSB();
						ISlotSystemManager ssm = MakeSubSSM();
							ISlotGroup sg = MakeSubSG();
							ssm.FindParent(sb).Returns(sg);
						sb.SetSSM(ssm);

					bool actual = sb.isHierarchySetUp;

					Assert.That(actual, Is.True);
				}
			/*	Methods	*/
				[Test]
				public void IncreasePickedAmountUpToQuantity_isStackable_IncreasePickedAmountUpToQuantity(){
					Slottable sb = MakeSB();
						ItemHandler itemHandler;
							PartsInstance parts = MakePartsInstance(0, 2);
							itemHandler = new ItemHandler(parts);
						sb.SetItemHandler(itemHandler);
					
					sb.IncreasePickedAmountUpToQuantity();
						Assert.That(sb.pickedAmount, Is.EqualTo(1));
					
					sb.IncreasePickedAmountUpToQuantity();
						Assert.That(sb.pickedAmount, Is.EqualTo(2));
					
					sb.IncreasePickedAmountUpToQuantity();
						Assert.That(sb.pickedAmount, Is.EqualTo(2));
				}
				[Test]
				public void IncreasePickedAmountUpToQuantity_isNotStackable_DoesNotIncrease(){
					Slottable sb = MakeSB();
						ItemHandler itemHandler;
							BowInstance bow = MakeBowInstance(0);
							itemHandler = new ItemHandler(bow);
						sb.SetItemHandler(itemHandler);
					
					sb.IncreasePickedAmountUpToQuantity();
						Assert.That(sb.pickedAmount, Is.EqualTo(0));
					
					sb.IncreasePickedAmountUpToQuantity();
						Assert.That(sb.pickedAmount, Is.EqualTo(0));
					
					sb.IncreasePickedAmountUpToQuantity();
						Assert.That(sb.pickedAmount, Is.EqualTo(0));
				}
				[Test]
				public void InitializeStates_Always_InitializesStates(){
					Slottable sb = MakeSBWithRealStateHandlers();

					sb.InitializeStates();

					Assert.That(sb.isDeactivated, Is.True);
					Assert.That(sb.wasSelStateNull, Is.True);
					Assert.That(sb.isWaitingForAction, Is.True);
					Assert.That(sb.wasActStateNull, Is.True);
					Assert.That(sb.isEqpStateNull, Is.True);
					Assert.That(sb.wasEqpStateNull, Is.True);
					Assert.That(sb.isUnmarked, Is.True);
					Assert.That(sb.wasMrkStateNull, Is.True);
					}
				[Test]
				public void Pickup_FromValidPrevActState_SetsPickedUpState(){
					Slottable sb = MakeSBWithRealStateHandlers();
					sb.Focus();
					sb.WaitForAction();
					sb.WaitForPickUp();
					
					sb.PickUp();

					Assert.That(sb.isPickingUp, Is.True);
					}
				[Test]
				public void Pickup_FromValidPrevActState_SetsPickedAmountOne(){
					Slottable sb = MakeSBWithRealStateHandlers();
					sb.Focus();
					sb.WaitForAction();
					sb.WaitForPickUp();
					
					sb.PickUp();

					Assert.That(sb.pickedAmount, Is.EqualTo(1));
				}
				Slottable MakeSBforIncrement(InventoryItemInstance item){
					Slottable sb = MakeSB();
						ISlotSystemManager ssm = MakeSubSSM();
						ITransactionManager stubTAM = MakeSubTAM();
							ITransactionIconHandler subIconHd = MakeSubIconHandler();
							stubTAM.iconHandler.Returns(subIconHd);
							ssm.tam.Returns(stubTAM);
						ITransactionCache stubTAC = MakeSubTAC();
						sb.SetSSM(ssm);
						sb.SetTACache(stubTAC);
						SBSelStateHandler selStateHandler = new SBSelStateHandler(sb);
						sb.SetSelStateHandler(selStateHandler);
						ITAMActStateHandler tamStateHandler = MakeSubTAMStateHandler();
							stubTAM.actStateHandler.Returns(tamStateHandler);
						sb.SetItem(item);
					return sb;
				}
				[Test]
				public void Increment_Always_SetsIsPickingUp(){
					Slottable sb = MakeSBWithRealStateHandlers();
					sb.Focus();
					sb.WaitForAction();
					sb.WaitForPickUp();

					sb.Increment();

					Assert.That(sb.isPickingUp, Is.True);
				}
				[Test]
				public void UpdateEquipState_ItemInstIsEquipped_SetsSBEquipped(){
					Slottable testSB = MakeSBWithRealStateHandlers();
					BowInstance stubBow = MakeBowInstance(0);
						stubBow.isEquipped = true;
						testSB.itemHandler.item.Returns(stubBow);
					ISlotGroup stubSG = MakeSubSG();
					ISlotSystemManager stubSSM = MakeSubSSM();
					stubSSM.FindParent(testSB).Returns(stubSG);
					testSB.SetSSM(stubSSM);

					testSB.UpdateEquipState();

					Assert.That(testSB.isEquipped, Is.True);
					}
				[Test]
				public void UpdateEquipState_ItemInstIsNotEquipped_SetsSBUnequipped(){
					Slottable testSB = MakeSBWithRealStateHandlers();
					BowInstance stubBow = MakeBowInstance(0);
						stubBow.isEquipped = false;
						testSB.itemHandler.item.Returns(stubBow);
					ISlotGroup stubSG = MakeSubSG();
					ISlotSystemManager stubSSM = MakeSubSSM();
					stubSSM.FindParent(testSB).Returns(stubSG);
					testSB.SetSSM(stubSSM);

					testSB.UpdateEquipState();

					Assert.That(testSB.isUnequipped, Is.True);
					}
				[Test]
				public void Refresh_WhenCalled_SetsActStateWFAState(){
					Slottable sb = MakeSBWithRealStateHandlers();
					
					((ISlottable)sb).Refresh();

					Assert.That(sb.isWaitingForAction, Is.True);
					}
				[Test]
				public void Refresh_WhenCalled_SetsPickedAmountZero(){
					Slottable sb = MakeSBWithRealStateHandlers();
					sb.pickedAmount = 10;
					((ISlottable)sb).Refresh();

					Assert.That(sb.pickedAmount, Is.EqualTo(0));
					}
				[Test]
				public void Refresh_WhenCalled_SetsNewSlotIDMinus2(){
					Slottable sb = MakeSBWithRealStateHandlers();
						sb.SetSlotHandler(new SlotHandler());
					sb.SetNewSlotID(3);
					((ISlottable)sb).Refresh();

					Assert.That(sb.newSlotID, Is.EqualTo(-2));
					}
				[TestCaseSource(typeof(ShareSGAndItemCases))]
				public void ShareSGAndItem_VariousCombo_ReturnsAccordingly(ISlotGroup sg, ISlotGroup otherSG, InventoryItemInstance iInst, InventoryItemInstance otherIInst, bool expected){
					Slottable sb = MakeSBWithRealStateHandlers();
					Slottable otherSB = MakeSBWithRealStateHandlers();
					ISlotSystemManager stubSSM = MakeSubSSM();
					sb.SetSSM(stubSSM);
					otherSB.SetSSM(stubSSM);
					stubSSM.FindParent(sb).Returns(sg);
					stubSSM.FindParent(otherSB).Returns(otherSG);
					sb.itemHandler.item.Returns(iInst);
					otherSB.itemHandler.item.Returns(otherIInst);

					Assert.That(sb.ShareSGAndItem(otherSB), Is.EqualTo(expected));
					}
					class ShareSGAndItemCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] sameSG_sameNonStackable_T;
								ISlotGroup sgA_1 = MakeSubSG();
								BowInstance bowA_1 = MakeBowInstance(0);
								sameSG_sameNonStackable_T = new object[]{
									sgA_1, sgA_1, bowA_1, bowA_1, true
								};
								yield return sameSG_sameNonStackable_T;
							object[] sameSG_sameStackable_T;
								ISlotGroup sgA_2 = MakeSubSG();
								PartsInstance partsA_2 = MakePartsInstance(0, 1);
								PartsInstance partsAA_2 = MakePartsInstance(0, 19);
								sameSG_sameStackable_T = new object[]{
									sgA_2, sgA_2, partsA_2, partsAA_2, true
								};
								yield return sameSG_sameStackable_T;
							object[] sameSG_DiffNonStackable_F;
								ISlotGroup sgA_3 = MakeSubSG();
								BowInstance bowA_3 = MakeBowInstance(0);
								BowInstance bowB_3 = MakeBowInstance(1);
								sameSG_DiffNonStackable_F = new object[]{
									sgA_3, sgA_3, bowA_3, bowB_3, false
								};
								yield return sameSG_DiffNonStackable_F;
							object[] sameSG_DiffStackable_F;
								ISlotGroup sgA_4 = MakeSubSG();
								PartsInstance partsA_4 = MakePartsInstance(0, 1);
								PartsInstance partsB_4 = MakePartsInstance(1, 19);
								sameSG_DiffStackable_F = new object[]{
									sgA_4, sgA_4, partsA_4, partsB_4, false
								};
								yield return sameSG_DiffStackable_F;
							object[] diffSG_sameNonStackable_F;
								ISlotGroup sgA_5 = MakeSubSG();
								ISlotGroup sgB_5 = MakeSubSG();
								BowInstance bowA_5 = MakeBowInstance(0);
								BowInstance bowAA_5 = MakeBowInstance(0);
								diffSG_sameNonStackable_F = new object[]{
									sgA_5, sgB_5, bowA_5, bowAA_5, false
								};
								yield return diffSG_sameNonStackable_F;
							object[] diffSG_sameStackable_F;
								ISlotGroup sgA_6 = MakeSubSG();
								ISlotGroup sgB_6 = MakeSubSG();
								PartsInstance partsA_6 = MakePartsInstance(0, 1);
								PartsInstance partsAA_6 = MakePartsInstance(0, 19);
								diffSG_sameStackable_F = new object[]{
									sgA_6, sgB_6, partsA_6, partsAA_6, false
								};
								yield return diffSG_sameStackable_F;
						}
					}			
			/*	helper	*/
				ISSEProcessEngine<ISBEqpProcess> MakeSubSBEqpProcessEngine(){
					return Substitute.For<ISSEProcessEngine<ISBEqpProcess>>();
				}
				ISSEProcessEngine<ISBMrkProcess> MakeSubSBMrkProcessEngine(){
					return Substitute.For<ISSEProcessEngine<ISBMrkProcess>>();
				}
		}
	}
}
