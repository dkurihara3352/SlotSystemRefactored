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
			[Test]
			public void InitializeStates(){
				
			}
			/*	Fields	*/
				[Test]
				public void isHierarchySetUp_SGIsSet_ReturnsTrue(){
					Slottable sb = MakeSB();
						ISlotSystemManager ssm = MakeSubSSM();
							ISlotGroup sg = MakeSubSG();
							ssm.FindParent(sb).Returns(sg);
						sb.SetSSM(ssm);

					bool actual = sb.IsHierarchySetUp();

					Assert.That(actual, Is.True);
				}
			/*	Methods	*/
				[Test]
				public void InitializeStates_Always_InitializesStates(){
					Slottable sb = MakeSB();
						IHoverable hoverable = new Hoverable(MakeSubTAC());
						sb.SetHoverable(hoverable);
						ISSESelStateHandler sbSelStateHandler = Substitute.For<ISSESelStateHandler>();
						sb.SetSelStateHandler(sbSelStateHandler);
						hoverable.SetSSESelStateHandler(sbSelStateHandler);
						ISBActStateHandler sbActStateHandler = Substitute.For<ISBActStateHandler>();
						sb.SetActStateHandler(sbActStateHandler);
						IItemHandler itemHandler = Substitute.For<IItemHandler>();
						sb.SetItemHandler(itemHandler);
						ISBEqpStateHandler eqpStateHandler = Substitute.For<ISBEqpStateHandler>();
						sb.SetEqpStateHandler(eqpStateHandler);
						ISBMrkStateHandler mrkStateHandler = Substitute.For<ISBMrkStateHandler>();
						sb.SetMrkStateHandler(mrkStateHandler);
						sb.SetSSM(MakeSubSSM());

					sb.InitializeStates();

					sbSelStateHandler.Received().Deactivate();
					sbActStateHandler.Received().WaitForAction();
					eqpStateHandler.Received().ClearCurEqpState();
					mrkStateHandler.Received().Unmark();
				}
				[Test]
				public void PickUp_Always_CallsDelegateMethods(){
					Slottable sb = MakeSB();
						ISBActStateHandler sbActStateHandler = Substitute.For<ISBActStateHandler>();
						sb.SetActStateHandler(sbActStateHandler);
						IItemHandler itemHandler = Substitute.For<IItemHandler>();
						sb.SetItemHandler(itemHandler);

					sb.PickUp();

					sbActStateHandler.Received().PickUp();
					itemHandler.Received().SetPickedAmount(1);
				}
				[Test]
				public void UpdateEquipState_ItemInstIsEquipped_SetsSBEquipped(){
					Slottable testSB = MakeSBWithRealStateHandlers();
					BowInstance stubBow = MakeBowInstance(0);
						stubBow.SetIsEquipped(true);
						testSB.GetItem().Returns(stubBow);
					ISlotGroup stubSG = MakeSubSG();
					ISlotSystemManager stubSSM = MakeSubSSM();
					stubSSM.FindParent(testSB).Returns(stubSG);
					testSB.SetSSM(stubSSM);

					testSB.UpdateEquipState();

					Assert.That(testSB.IsEquipped(), Is.True);
				}
				[Test]
				public void UpdateEquipState_ItemInstIsNotEquipped_SetsSBUnequipped(){
					Slottable testSB = MakeSBWithRealStateHandlers();
					BowInstance stubBow = MakeBowInstance(0);
						stubBow.SetIsEquipped(false);
						testSB.GetItem().Returns(stubBow);
					ISlotGroup stubSG = MakeSubSG();
					ISlotSystemManager stubSSM = MakeSubSSM();
					stubSSM.FindParent(testSB).Returns(stubSG);
					testSB.SetSSM(stubSSM);

					testSB.UpdateEquipState();

					Assert.That(testSB.IsUnequipped(), Is.True);
				}
				[Test]
				public void Refresh_WhenCalled_CallsDelegatesInSequence(){
					Slottable sb = MakeSB();
						ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
						sb.SetActStateHandler(actStateHandler);
						ISlotHandler slotHandler = Substitute.For<ISlotHandler>();
						sb.SetSlotHandler(slotHandler);
						IItemHandler itemHandler = Substitute.For<IItemHandler>();
						sb.SetItemHandler(itemHandler);
					
					sb.Refresh();

					Received.InOrder(() => {
						actStateHandler.WaitForAction();
						itemHandler.SetPickedAmount(0);
						slotHandler.SetNewSlotID(-2);
					});
				}
				[TestCaseSource(typeof(ShareSGAndItemCases))]
				public void ShareSGAndItem_VariousCombo_ReturnsAccordingly(ISlotGroup sg, ISlotGroup otherSG, IInventoryItemInstance iInst, IInventoryItemInstance otherIInst, bool expected){
					Slottable sb = MakeSBWithRealStateHandlers();
					Slottable otherSB = MakeSBWithRealStateHandlers();
					ISlotSystemManager stubSSM = MakeSubSSM();
					sb.SetSSM(stubSSM);
					otherSB.SetSSM(stubSSM);
					stubSSM.FindParent(sb).Returns(sg);
					stubSSM.FindParent(otherSB).Returns(otherSG);
					IItemHandler itemHandler = Substitute.For<IItemHandler>();
						itemHandler.GetItem().Returns(iInst);
					sb.SetItemHandler(itemHandler);
					IItemHandler otherItemHandler = Substitute.For<IItemHandler>();
						otherItemHandler.GetItem().Returns(otherIInst);
					otherSB.SetItemHandler(otherItemHandler);

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
