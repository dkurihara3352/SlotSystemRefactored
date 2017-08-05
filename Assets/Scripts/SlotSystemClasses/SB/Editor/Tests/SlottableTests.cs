using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections;
using System.Collections.Generic;
using System;
using Utility;
using NSubstitute;

namespace SlotSystemTests{
	namespace ElementsTests{
		[TestFixture]
		[Category("SB")]
		public class SlottableTests: SlotSystemTest {
			/* States */
				[Test]
				public void Activate_SSMIsTAResultRevertForFalse_SetIsFocused(){
					Slottable sb = MakeSB();
						ITransactionCache stubTAC = MakeSubTAC();
						IHoverable stubHoverable = Substitute.For<IHoverable>();
						stubTAC.IsCachedTAResultRevert(stubHoverable).Returns(false);
						sb.SetTACache(stubTAC);
						sb.SetHoverable(stubHoverable);
						SSESelStateHandler handler = new SSESelStateHandler();
						sb.SetSelStateHandler(handler);
					
					sb.Activate();

					Assert.That(sb.isFocused, Is.True);
				}
				[Test]
				public void Activate_SSMIsTAResultRevertForTrue_SetIsDefocused(){
					Slottable sb = MakeSB();
						ITransactionCache stubTAC = MakeSubTAC();
						IHoverable stubHoverable = Substitute.For<IHoverable>();
						stubTAC.IsCachedTAResultRevert(stubHoverable).Returns(true);
						sb.SetTACache(stubTAC);
						sb.SetHoverable(stubHoverable);
						SSESelStateHandler handler = new SSESelStateHandler();
						sb.SetSelStateHandler(handler);
					
					sb.Activate();

					Assert.That(sb.isDefocused, Is.True);
				}
			/*	Process	*/
				/*	ActProc	*/
					[Test]
					public void SetAndRunActState_Null_SetsActProcNull(){
						Slottable sb = MakeSB();
						
						sb.SetAndRunActProcess(null);

						Assert.That(sb.actProcess, Is.Null);
						}
					[Test]
					public void SetAndRunActProcess_ISBActProcess_SetsActProc(){
						Slottable sb = MakeSB();
						
						ISBActProcess stubProc = MakeSubSBActProc();
						sb.SetAndRunActProcess(stubProc);

						Assert.That(sb.actProcess, Is.SameAs(stubProc));
						}
				/*	EqpProc	*/
					[Test]
					public void SetAndRunEqpState_Null_CallsEngineWithNull(){
						Slottable sb = MakeSB();
						ISSEProcessEngine<ISBEqpProcess> mockEngine = MakeSubSBEqpProcessEngine();
						sb.SetEqpProcessEngine(mockEngine);
						sb.SetAndRunEqpProcess(null);

						mockEngine.Received().SetAndRunProcess(null);
						}
					[Test]
					public void SetAndRunEqpProcess_ISBEqpProcess_CallsEqpProcEngineWithTheProc(){
						Slottable sb = MakeSB();
						ISSEProcessEngine<ISBEqpProcess> mockEngine = MakeSubSBEqpProcessEngine();
						sb.SetEqpProcessEngine(mockEngine);
						ISBEqpProcess stubProc = MakeSubSBEqpProc();
						sb.SetAndRunEqpProcess(stubProc);

						mockEngine.Received().SetAndRunProcess(stubProc);
						}
				/*	MrkProc	*/
					[Test]
					public void SetAndRunMrkState_Null_CallsEngineWithNull(){
						Slottable sb = MakeSB();
						ISSEProcessEngine<ISBMrkProcess> mockEngine = MakeSubSBMrkProcessEngine();
						sb.SetMrkProcessEngine(mockEngine);
						sb.SetAndRunMrkProcess(null);

						mockEngine.Received().SetAndRunProcess(null);
						}
					[Test]
					public void SetAndRunMrkProcess_ISBMrkProcess_CallsMrkProcEngineWithTheProc(){
						Slottable sb = MakeSB();
						ISSEProcessEngine<ISBMrkProcess> mockEngine = MakeSubSBMrkProcessEngine();
						sb.SetMrkProcessEngine(mockEngine);
						ISBMrkProcess stubProc = MakeSubSBMrkProc();
						sb.SetAndRunMrkProcess(stubProc);

						mockEngine.Received().SetAndRunProcess(stubProc);
						}
			
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
				public void InitializeStates_Always_InitializesStates(){
					Slottable sb = MakeSB();
					ISlotSystemManager ssm = MakeSubSSM();
						ISlotGroup sg = MakeSubSG();
						ssm.FindParent(sb).Returns(sg);
					sb.SetSSM(ssm);
					SSESelStateHandler handler = new SSESelStateHandler();
					sb.SetSelStateHandler(handler);

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
				Slottable MakeSBForPickUp(){
					Slottable sb = MakeSB();
						ITransactionManager stubTAM = Substitute.For<ITransactionManager>();
						sb.SetTAM(stubTAM);
						ITransactionCache stubTAC = MakeSubTAC();
						sb.SetTACache(stubTAC);
						SSESelStateHandler selStateHd = new SSESelStateHandler();
						sb.SetSelStateHandler(selStateHd);
						ITransactionIconHandler iconHd = MakeSubIconHandler();
						sb.SetIconHandler(iconHd);
						ITAMActStateHandler tamStateHandler = MakeSubTAMStateHandler();
						sb.SetTAMStateHandler(tamStateHandler);
					return sb;
				}
				[Test]
				public void Pickup_FromValidPrevActState_SetsPickedUpState(){
					Slottable sb = MakeSBForPickUp();
					sb.Focus();
					sb.WaitForAction();
					sb.WaitForPickUp();
					
					sb.PickUp();

					Assert.That(sb.isPickingUp, Is.True);
					}
				[Test]
				public void Pickup_FromValidPrevActState_SetsPickedAmountOne(){
					Slottable sb = MakeSBForPickUp();
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
						ITransactionCache stubTAC = MakeSubTAC();
						sb.SetSSM(ssm);
						sb.SetTAM(stubTAM);
						sb.SetTACache(stubTAC);
						SSESelStateHandler selStateHandler = new SSESelStateHandler();
						sb.SetSelStateHandler(selStateHandler);
						ITransactionIconHandler subIconHd = MakeSubIconHandler();
						sb.SetIconHandler(subIconHd);
						ITAMActStateHandler tamStateHandler = MakeSubTAMStateHandler();
						sb.SetTAMStateHandler(tamStateHandler);
						sb.SetItem(item);
					return sb;
				}
				[TestCaseSource(typeof(IncrementCases))]
				public void Increment_StackableToMoreThanQuantityAndValidPrevActStateAndIsActivated_IncrementsPickedAmountUpToQuanityNoMoreThanQuantity(InventoryItemInstance item, int expected){
					Slottable sb = MakeSBforIncrement(item);
					sb.Focus();
					sb.WaitForAction();
					sb.WaitForPickUp();

					for(int i =0; i< expected *2; i++){
						sb.Increment();
					}

					Assert.That(sb.pickedAmount, Is.EqualTo(expected));
					}
					class IncrementCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case1 = new object[]{
								MakePartsInstance(0, 2),
								2
							};
							yield return case1;

							object[] case2 = new object[]{
								MakePartsInstance(0, 10),
								10
							};
							yield return case2;
						}
					}
				[TestCaseSource(typeof(IncrementNonStackableCases))]
				public void Increment_NonStackableAndAfterSSMAndIsActivatedAndFromValidPrevActState_DoesNotIncrement(InventoryItemInstance item){
					Slottable sb = MakeSBforIncrement(item);
					sb.Focus();
					sb.WaitForAction();
					sb.WaitForPickUp();

					for(int i = 0; i < 10; i++)
						sb.Increment();

					Assert.That(sb.pickedAmount, Is.EqualTo(0));
					}
					class IncrementNonStackableCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							yield return MakeBowInstance(0);
							yield return MakeWearInstance(0);
							yield return MakeShieldInstance(0);
							yield return MakeMeleeWeaponInstance(0);
							yield return MakeQuiverInstance(0);
							yield return MakePackInstance(0);
						}
					}
				[Test]
				public void ExecuteTransaction_WhenCalled_CallsSSMExecuteTransaction(){
					ITransactionManager mockTAM = Substitute.For<ITransactionManager>();
					Slottable stubSB = MakeSB();
					stubSB.SetTAM(mockTAM);

					stubSB.ExecuteTransaction();

					mockTAM.Received().ExecuteTransaction();
					}
				[Test]
				public void ExpireActionProcess_actProcIsRunning_CallsActProcExpire(){
					ISBActProcess mockProc = Substitute.For<ISBActProcess>();
					mockProc.isRunning.Returns(true);
					Slottable stubSB = MakeSB();
					stubSB.SetAndRunActProcess(mockProc);

					stubSB.ExpireActProcess();

					mockProc.Received().Expire();
					}
				[Test]
				public void UpdateEquipState_ItemInstIsEquipped_SetsSBEquipped(){
					Slottable testSB = MakeSB();
					BowInstance stubBow = MakeBowInstance(0);
					stubBow.isEquipped = true;
					testSB.SetItem(stubBow);
					ISlotGroup stubSG = MakeSubSG();
					ISlotSystemManager stubSSM = MakeSubSSM();
					stubSSM.FindParent(testSB).Returns(stubSG);
					testSB.SetSSM(stubSSM);

					testSB.UpdateEquipState();

					Assert.That(testSB.isEquipped, Is.True);
					}
				[Test]
				public void UpdateEquipState_ItemInstIsNotEquipped_SetsSBUnequipped(){
					Slottable testSB = MakeSB();
					BowInstance stubBow = MakeBowInstance(0);
					stubBow.isEquipped = false;
					testSB.SetItem(stubBow);
					ISlotGroup stubSG = MakeSubSG();
					ISlotSystemManager stubSSM = MakeSubSSM();
					stubSSM.FindParent(testSB).Returns(stubSG);
					testSB.SetSSM(stubSSM);

					testSB.UpdateEquipState();

					Assert.That(testSB.isUnequipped, Is.True);
					}
				[Test]
				public void Refresh_WhenCalled_SetsActStateWFAState(){
					Slottable sb = MakeSB();
					
					((ISlottable)sb).Refresh();

					Assert.That(sb.isWaitingForAction, Is.True);
					}
				[Test]
				public void Refresh_WhenCalled_SetsPickedAmountZero(){
					Slottable sb = MakeSB();
					sb.pickedAmount = 10;
					((ISlottable)sb).Refresh();

					Assert.That(sb.pickedAmount, Is.EqualTo(0));
					}
				[Test]
				public void Refresh_WhenCalled_SetsNewSlotIDMinus2(){
					Slottable sb = MakeSB();
					sb.SetNewSlotID(3);
					((ISlottable)sb).Refresh();

					Assert.That(sb.newSlotID, Is.EqualTo(-2));
					}
				[TestCaseSource(typeof(ShareSGAndItemCases))]
				public void ShareSGAndItem_VariousCombo_ReturnsAccordingly(ISlotGroup sg, ISlotGroup otherSG, InventoryItemInstance iInst, InventoryItemInstance otherIInst, bool expected){
					Slottable sb = MakeSB();
					Slottable otherSB = MakeSB();
					ISlotSystemManager stubSSM = MakeSubSSM();
					sb.SetSSM(stubSSM);
					otherSB.SetSSM(stubSSM);
					stubSSM.FindParent(sb).Returns(sg);
					stubSSM.FindParent(otherSB).Returns(otherSG);
					sb.SetItem(iInst);
					otherSB.SetItem(otherIInst);

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
