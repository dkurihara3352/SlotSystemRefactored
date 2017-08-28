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
		public class SlottableTests: SlotSystemTest {
			[Test]
			public void InitializeSB_Always_PerformsSetUp(){
				Slottable sb = MakeSB();
					ITransactionCache taCache = MakeSubTAC();
					ISlotSystemManager ssm = MakeSubSSM();
						ssm.GetTAC().Returns(taCache);
					sb.SetSSM(ssm);
					IInventoryItemInstance item = Substitute.For<IInventoryItemInstance>();
				
				sb.InitializeSB(item);

				Assert.That(sb.GetTAC(), Is.SameAs(taCache));
				Assert.That((sb.GetHoverable() is Hoverable), Is.True);
				Assert.That((sb.GetTapCommand() is SBTapCommand), Is.True);
				Assert.That((sb.GetPickUpCommand() is SBPickUpCommand), Is.True);
				Assert.That((sb.GetItemHandler() is ItemHandler), Is.True);
				Assert.That((sb.GetSlotHandler() is SlotHandler), Is.True);
				Assert.That((sb.UISelStateHandler() is SBSelStateHandler), Is.True);
				Assert.That((sb.ActStateHandler() is SBActStateHandler), Is.True);
			}
			[TestCaseSource(typeof(SetEquipPickUpCommandCases))]
			public void SetPickUpEquipCommand_Various_Sets_PickUpCommandAccordingly<T>(IInventoryItemInstance item, T command) where T: SBPickUpEquipCommand{
				Slottable sb = MakeSB();
					SBPickUpEquipCommand actual = sb.GetPickUpEquipCommand(item);

				Assert.That(actual, Is.Not.Null);
				Assert.That((actual is T), Is.True);
			}
				class SetEquipPickUpCommandCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						yield return new TestCaseData(MakeBowInstance(0), new SBPickUpEquipBowCommand(MakeSubSB())).SetName("Bow:		SBPickUpEquipBowCommand");
						yield return new TestCaseData(MakeWearInstance(0), new SBPickUpEquipWearCommand(MakeSubSB())).SetName("Wear:		SBPickUpEquipWearCommand");
						yield return new TestCaseData(MakeShieldInstance(0), new SBPickUpEquipCGearsCommand(MakeSubSB())).SetName("Shield:		SBPickUpEquipCGearsCommand");
						yield return new TestCaseData(MakeMWeaponInstance(0), new SBPickUpEquipCGearsCommand(MakeSubSB())).SetName("MWeapon:	SBPickUpEquipCGearsCommand");
						yield return new TestCaseData(MakeQuiverInstance(0), new SBPickUpEquipCGearsCommand(MakeSubSB())).SetName("Quiver:		SBPickUpEquipCGearsCommand");
						yield return new TestCaseData(MakePackInstance(0), new SBPickUpEquipCGearsCommand(MakeSubSB())).SetName("Pack:		SBPickUpEquipCGearsCommand");
						yield return new TestCaseData(MakePartsInstance(0, 2), new SBPickUpEquipPartsCommand(MakeSubSB())).SetName("Parts:		SBPickUpEquipPartsCommand");
					}
				}
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
			[Test]
			public void InitializeStates_Always_InitializesStates(){
				Slottable sb = MakeSB();
					IHoverable hoverable = new Hoverable(MakeSubTAC());
					sb.SetHoverable(hoverable);
					IUISelStateHandler sbSelStateHandler = Substitute.For<IUISelStateHandler>();
					sb.SetSelStateHandler(sbSelStateHandler);
					hoverable.SetSSESelStateHandler(sbSelStateHandler);
					ISBActStateHandler sbActStateHandler = Substitute.For<ISBActStateHandler>();
					sb.SetActStateHandler(sbActStateHandler);
					IItemHandler itemHandler = Substitute.For<IItemHandler>();
					sb.SetItemHandler(itemHandler);
					sb.SetSSM(MakeSubSSM());

				sb.InitializeStates();

				sbSelStateHandler.Received().Deactivate();
				sbActStateHandler.Received().WaitForAction();
			}
			[Test]
			public void PickUp_Always_CallsDelegateMethods(){
				Slottable sb = MakeSB();
					ISBActStateHandler sbActStateHandler = Substitute.For<ISBActStateHandler>();
					sb.SetActStateHandler(sbActStateHandler);
					IItemHandler itemHandler = Substitute.For<IItemHandler>();
					sb.SetItemHandler(itemHandler);
					ISBCommand pickUPCommand = Substitute.For<ISBCommand>();
					sb.SetPickUpCommand(pickUPCommand);

				sb.PickUp();

				sbActStateHandler.Received().PickUp();
				itemHandler.Received().SetPickedAmount(1);
				pickUPCommand.Received().Execute();
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
				Slottable sb = MakeSB();
				Slottable otherSB = MakeSB();
				ISlotSystemManager stubSSM = MakeSubSSM();
				sb.SetSSM(stubSSM);
				otherSB.SetSSM(stubSSM);
				stubSSM.FindParent(sb).Returns(sg);
				stubSSM.FindParent(otherSB).Returns(otherSG);
				IItemHandler itemHandler = Substitute.For<IItemHandler>();
					itemHandler.Item().Returns(iInst);
				sb.SetItemHandler(itemHandler);
				IItemHandler otherItemHandler = Substitute.For<IItemHandler>();
					otherItemHandler.Item().Returns(otherIInst);
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
				IUIProcessEngine<ISBEqpProcess> MakeSubSBEqpProcessEngine(){
					return Substitute.For<IUIProcessEngine<ISBEqpProcess>>();
				}
				IUIProcessEngine<ISBMrkProcess> MakeSubSBMrkProcessEngine(){
					return Substitute.For<IUIProcessEngine<ISBMrkProcess>>();
				}
		}
	}
}
