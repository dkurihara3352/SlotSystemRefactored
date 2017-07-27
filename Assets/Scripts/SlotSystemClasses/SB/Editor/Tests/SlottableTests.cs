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
				[TestCase(true)]
				[TestCase(false)]
				public void Activate_WhenCalled_ReferToTAAndFocusOrDefocus(bool focused){
					Slottable sb = MakeSB();
						ISlotSystemManager ssm = Substitute.For<ISlotSystemManager>();
						ssm.When(x => x.ReferToTAAndUpdateSelState(sb)).Do(x => {if(focused) sb.Focus(); else sb.Defocus();});
						sb.SetSSM(ssm);
					
					sb.Activate();
					
					if(focused){
						Assert.That(sb.isFocused, Is.True);
						Assert.That(sb.isDefocused, Is.False);
					}else{
						Assert.That(sb.isFocused, Is.False);
						Assert.That(sb.isDefocused, Is.True);
					}
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
				public void InitializeStates_AfterSGIsSet_InitializesStates(){
					Slottable sb = MakeSB();
					ISlotSystemManager ssm = MakeSubSSM();
						ISlotGroup sg = MakeSubSG();
						ssm.FindParent(sb).Returns(sg);
					sb.SetSSM(ssm);

					sb.InitializeStates();

					Assert.That(sb.isDeactivated, Is.True);
					Assert.That(sb.isSelStateInit, Is.True);
					Assert.That(sb.isWaitingForAction, Is.True);
					Assert.That(sb.isActStateInit, Is.True);
					Assert.That(sb.isEqpStateInit, Is.True);
					Assert.That(sb.isEquipped, Is.False);
					Assert.That(sb.isUnequipped, Is.False);
					Assert.That(sb.isUnmarked, Is.True);
					Assert.That(sb.isMrkStateInit, Is.True);
					}
				[Test]
				public void Pickup_SelStateNotNull_SetsPickedUpState(){
					Slottable sb = MakeSB();
					sb.SetSSM(MakeSubSSM());
					sb.Deactivate();
					
					sb.PickUp();

					Assert.That(sb.isPickingUp, Is.True);
					}
				[Test]
				public void Pickup_SelStateNotNull_SetsPickedAmountOne(){
					Slottable sb = MakeSB();
					sb.SetSSM(MakeSubSSM());
					sb.Deactivate();
					
					sb.PickUp();

					Assert.That(sb.pickedAmount, Is.EqualTo(1));
					}
				[TestCaseSource(typeof(IncrementCases))]
				public void Increment_StackableToMoreThanQuantity_IncrementsPickedAmountUpToQuanityNoMoreThanQuantity(InventoryItemInstance item, int expected){
					Slottable sb = MakeSB();
					sb.SetItem(item);
					ISlotSystemManager ssm = MakeSubSSM();
					sb.SetSSM(ssm);
					sb.Deactivate();

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
				public void Increment_NonStackableAndAfterSSMAndSelStateSet_DoesNotIncrement(InventoryItemInstance item){
					Slottable sb = MakeSB();
					ISlotSystemManager ssm = MakeSubSSM();
					sb.SetSSM(ssm);
					sb.Deactivate();
					sb.SetItem(item);

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
					ISlotSystemManager mockSSM = MakeSubSSM();
					Slottable stubSB = MakeSB();
					stubSB.SetSSM(mockSSM);

					stubSB.ExecuteTransaction();

					mockSSM.Received().ExecuteTransaction();
					}
				[Test]
				public void ExpireActionProcess_actProcIsRunning_CallsActProcExpire(){
					ISBActProcess mockProc = Substitute.For<ISBActProcess>();
					mockProc.isRunning.Returns(true);
					Slottable stubSB = MakeSB();
					stubSB.SetAndRunActProcess(mockProc);

					stubSB.ExpireActionProcess();

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
				public void Reset_WhenCalled_SetsActStateWFAState(){
					Slottable sb = MakeSB();
					
					sb.Reset();

					Assert.That(sb.isWaitingForAction, Is.True);
					}
				[Test]
				public void Reset_WhenCalled_SetsPickedAmountZero(){
					Slottable sb = MakeSB();
					sb.pickedAmount = 10;
					sb.Reset();

					Assert.That(sb.pickedAmount, Is.EqualTo(0));
					}
				[Test]
				public void Reset_WhenCalled_SetsNewSlotIDMinus2(){
					Slottable sb = MakeSB();
					sb.SetNewSlotID(3);
					sb.Reset();

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
				[TestCaseSource(typeof(HaveCommonItemFamilyCases))]
				public void HaveCommonItemFamily_Various_ReturnsAccordingly(InventoryItemInstance iInst, InventoryItemInstance otherIInst, bool expected){
					Slottable sb = MakeSB();
					Slottable otherSB = MakeSB();
					sb.SetItem(iInst);
					otherSB.SetItem(otherIInst);

					Assert.That(sb.HaveCommonItemFamily(otherSB), Is.EqualTo(expected));
					}
					class HaveCommonItemFamilyCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] bow_bow_T;
								BowInstance bow_1 = MakeBowInstance(0);
								bow_bow_T = new object[]{bow_1, bow_1, true};
								yield return bow_bow_T;
							object[] wear_wear_T;
								WearInstance wear_2 = MakeWearInstance(0);
								wear_wear_T = new object[]{wear_2, wear_2, true};
								yield return wear_wear_T;
							object[] shield_mWeapon_T;
								ShieldInstance shield_3 = MakeShieldInstance(0);
								MeleeWeaponInstance mWeapon_3 = MakeMeleeWeaponInstance(0);
								shield_mWeapon_T = new object[]{shield_3, mWeapon_3, true};
								yield return shield_mWeapon_T;
							object[] parts_parts_T;
								PartsInstance partsA_4 = MakePartsInstance(0, 1);
								PartsInstance partsB_4 = MakePartsInstance(1, 3);
								parts_parts_T = new object[]{partsA_4, partsB_4, true};
								yield return parts_parts_T;
							object[] bow_wear_F;
								BowInstance bow_5 = MakeBowInstance(0);
								WearInstance wear_5 = MakeWearInstance(0);
								bow_wear_F = new object[]{bow_5, wear_5, false};
								yield return bow_wear_F;
							object[] wear_quiver_F;
								WearInstance wear_6 = MakeWearInstance(0);
								QuiverInstance quiver_6 = MakeQuiverInstance(0);
								wear_quiver_F = new object[]{wear_6, quiver_6, false};
								yield return wear_quiver_F;
							object[] shield_parts_F;
								ShieldInstance shield_7 = MakeShieldInstance(0);
								PartsInstance parts_7 = MakePartsInstance(0, 1);
								shield_parts_F = new object[]{shield_7, parts_7, false};
								yield return shield_parts_F;
							object[] parts_pack_F;
								PartsInstance partsA_8 = MakePartsInstance(0, 1);
								PackInstance pack_8 = MakePackInstance(0);
								parts_pack_F = new object[]{partsA_8, pack_8, false};
								yield return parts_pack_F;
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
