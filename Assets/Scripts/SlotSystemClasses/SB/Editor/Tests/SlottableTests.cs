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
		public class SlottableTests: AbsSlotSystemTest {
				/*	ActState	*/
					[Test]
					public void SetActState_Null_SetsActStateNull(){
						Slottable sb = MakeSB();

						sb.SetActState(null);

						Assert.That(sb.curActState, Is.Null);
					}
					[Test]
					public void SetActState_SBActState_SetsActState(){
						Slottable sb = MakeSB();
						SBActState stubActState = MakeSubSBActState();

						sb.SetActState(stubActState);

						Assert.That(sb.curActState, Is.SameAs(stubActState));
					}
				/*	EqpState	*/
					[Test]
					public void SetEqpState_Null_CallsEqpStateEngineSetState(){
						Slottable sb = MakeSB();
						ISSEStateEngine<ISBEqpState> mockEngine = MakeSubSBEqpStateEngine();
						sb.SetEqpStateEngine(mockEngine);
						sb.SetEqpState(null);

						mockEngine.Received().SetState(null);
					}
					[Test]
					public void SetEqpState_SBEqpState_CallsEqpStateEngineSetState(){
						Slottable sb = MakeSB();
						ISSEStateEngine<ISBEqpState> mockEngine = MakeSubSBEqpStateEngine();
						sb.SetEqpStateEngine(mockEngine);
						SBEqpState stubEqpState = MakeSubSBEqpState();
						sb.SetEqpState(stubEqpState);

						mockEngine.Received().SetState(stubEqpState);
					}
				/*	Mrk State */
					[Test]
					public void SetMrkState_Null_CallsMrkStateEngineSetState(){
						Slottable sb = MakeSB();
						ISSEStateEngine<ISBMrkState> mockEngine = MakeSubSBMrkStateEngine();
						sb.SetMrkStateEngine(mockEngine);
						sb.SetMrkState(null);

						mockEngine.Received().SetState(null);
					}
					[Test]
					public void SetMrkState_SBMrkState_CallsMrkStateEngineSetState(){
						Slottable sb = MakeSB();
						ISSEStateEngine<ISBMrkState> mockEngine = MakeSubSBMrkStateEngine();
						sb.SetMrkStateEngine(mockEngine);
						SBMrkState stubMrkState = MakeSubSBMrkState();
						sb.SetMrkState(stubMrkState);

						mockEngine.Received().SetState(stubMrkState);
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
			/*	Methods	*/
				[Test][Category("Methods")]
				public void InitializeStates_WhenCalled_InitializesStates(){
					Slottable sb = MakeSB();

					sb.InitializeStates();

					Assert.That(sb.curSelState, Is.SameAs(AbsSlotSystemElement.deactivatedState));
					Assert.That(sb.prevSelState, Is.Null);
					Assert.That(sb.curActState, Is.SameAs(Slottable.sbWaitForActionState));
					Assert.That(sb.prevActState, Is.Null);
					Assert.That(sb.curEqpState, Is.Null);
					Assert.That(sb.prevEqpState, Is.Null);
					Assert.That(sb.curMrkState, Is.SameAs(Slottable.unmarkedState));
					Assert.That(sb.prevMrkState, Is.Null);
				}
				[Test]
				[Category("Methods")]
				public void Pickup_WhenCalled_SetsPickedUpState(){
					Slottable sb = MakeSB();
					sb.SetSSM(MakeSubSSM());
					sb.SetSelState(Substitute.For<ISSESelState>());
					
					sb.PickUp();

					Assert.That(sb.curActState, Is.SameAs(Slottable.pickedUpState));
				}
				[Test]
				[Category("Methods")]
				public void Pickup_WhenCalled_SetsPickedAmountOne(){
					Slottable sb = MakeSB();
					sb.SetSSM(MakeSubSSM());
					sb.SetSelState(Substitute.For<ISSESelState>());
					
					sb.PickUp();

					Assert.That(sb.pickedAmount, Is.EqualTo(1));
				}
				[TestCaseSource(typeof(IncrementCases))]
				[Category("Methods")]
				public void Increment_Stackable_IncrementsPickedAmountUpToQuanity(InventoryItemInstance item, int expected){
					Slottable sb = MakeSB();
					
					sb.SetItem(item);

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
				[Category("Methods")]
				public void Increment_NonStackable_DoesNotIncrement(InventoryItemInstance item){
					Slottable sb = MakeSB();
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
				[Category("Methods")]
				public void ExecuteTransaction_WhenCalled_CallsSSMExecuteTransaction(){
					ISlotSystemManager mockSSM = MakeSubSSM();
					Slottable stubSB = MakeSB();
					stubSB.SetSSM(mockSSM);

					stubSB.ExecuteTransaction();

					mockSSM.Received().ExecuteTransaction();
				}
				[Test]
				[Category("Methods")]
				public void ExpireActionProcess_actProcIsRunning_CallsActProcExpire(){
					ISBActProcess mockProc = Substitute.For<ISBActProcess>();
					mockProc.isRunning.Returns(true);
					Slottable stubSB = MakeSB();
					stubSB.SetAndRunActProcess(mockProc);

					stubSB.ExpireActionProcess();

					mockProc.Received().Expire();
				}
				[Test]
				[Category("Methods")]
				public void UpdateEquipState_ItemInstIsEquipped_SetsEqpStateEquipped(){
					Slottable testSB = MakeSB();
					BowInstance stubBow = MakeBowInstance(0);
					stubBow.isEquipped = true;
					testSB.SetItem(stubBow);
					ISlotGroup stubSG = MakeSubSG();
					ISlotSystemManager stubSSM = MakeSubSSM();
					stubSSM.FindParent(testSB).Returns(stubSG);
					testSB.SetSSM(stubSSM);

					testSB.UpdateEquipState();

					Assert.That(testSB.curEqpState, Is.SameAs(Slottable.equippedState));
				}
				[Test]
				[Category("Methods")]
				public void UpdateEquipState_ItemInstIsNotEquipped_SetsEqpStateUnequipped(){
					Slottable testSB = MakeSB();
					BowInstance stubBow = MakeBowInstance(0);
					stubBow.isEquipped = false;
					testSB.SetItem(stubBow);
					ISlotGroup stubSG = MakeSubSG();
					ISlotSystemManager stubSSM = MakeSubSSM();
					stubSSM.FindParent(testSB).Returns(stubSG);
					testSB.SetSSM(stubSSM);

					testSB.UpdateEquipState();

					Assert.That(testSB.curEqpState, Is.SameAs(Slottable.unequippedState));
				}
				[Test]
				[Category("Methods")]
				public void Reset_WhenCalled_SetsActStateWFAState(){
					Slottable sb = MakeSB();
					
					sb.Reset();

					Assert.That(sb.curActState, Is.SameAs(Slottable.sbWaitForActionState));
				}
				[Test]
				[Category("Methods")]
				public void Reset_WhenCalled_SetsPickedAmountZero(){
					Slottable sb = MakeSB();
					sb.pickedAmount = 10;
					sb.Reset();

					Assert.That(sb.pickedAmount, Is.EqualTo(0));
				}
				[Test]
				[Category("Methods")]
				public void Reset_WhenCalled_SetsNewSlotIDMinus2(){
					Slottable sb = MakeSB();
					sb.SetNewSlotID(3);
					sb.Reset();

					Assert.That(sb.newSlotID, Is.EqualTo(-2));
				}
				[TestCaseSource(typeof(ShareSGAndItemCases))]
				[Category("Methods")]
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
							ISlotGroup sgA = MakeSubSG();
							ISlotGroup sgB = MakeSubSG();
							BowInstance bowA = MakeBowInstance(0);
							BowInstance bowA_1 = MakeBowInstance(0);
							PartsInstance partsA = MakePartsInstance(0, 1);
							PartsInstance partsA_1 = MakePartsInstance(0, 2);
							object[] valid1 = new object[]{
								sgA, sgA, bowA, bowA, true
							};
							yield return valid1;
							object[] valid2 = new object[]{
								sgA, sgA, partsA, partsA_1, true
							};
							yield return valid2;
							object[] invalid1 = new object[]{
								sgA, sgA, bowA, bowA_1, false
							};
							yield return invalid1;
							object[] invalid2 = new object[]{
								sgA, sgB, bowA, bowA, false
							};
							yield return invalid2;
						}
					}
				[TestCaseSource(typeof(HaveCommonItemFamilyCases))]
				[Category("Methods")]
				public void HaveCommonItemFamily_Various_ReturnsAccordingly(InventoryItemInstance iInst, InventoryItemInstance otherIInst, bool expected){
					Slottable sb = MakeSB();
					Slottable otherSB = MakeSB();
					sb.SetItem(iInst);
					otherSB.SetItem(otherIInst);

					Assert.That(sb.HaveCommonItemFamily(otherSB), Is.EqualTo(expected));
				}
					class HaveCommonItemFamilyCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							BowInstance bow = MakeBowInstance(0);
							WearInstance wear = MakeWearInstance(0);
							ShieldInstance shield = MakeShieldInstance(0);
							MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
							QuiverInstance quiver = MakeQuiverInstance(0);
							PackInstance pack = MakePackInstance(0);
							PartsInstance parts = MakePartsInstance(0, 1);
							yield return new object[]{bow, bow, true};
							yield return new object[]{wear, wear, true};
							yield return new object[]{shield, shield, true};
							yield return new object[]{mWeapon, mWeapon, true};
							yield return new object[]{quiver, quiver, true};
							yield return new object[]{pack, pack, true};
							yield return new object[]{parts, parts, true};

							yield return new object[]{bow, wear, false};
							yield return new object[]{bow, shield, false};
							yield return new object[]{bow, mWeapon, false};
							yield return new object[]{bow, quiver, false};
							yield return new object[]{bow, pack, false};
							yield return new object[]{bow, parts, false};
						}
					}
			/*	Forward	*/
			/*	helper	*/
			ISSEStateEngine<ISBActState> MakeSubSBActStateEngine(){
				return Substitute.For<ISSEStateEngine<ISBActState>>();
			}
			ISSEStateEngine<ISBEqpState> MakeSubSBEqpStateEngine(){
				return Substitute.For<ISSEStateEngine<ISBEqpState>>();
			}
			ISSEStateEngine<ISBMrkState> MakeSubSBMrkStateEngine(){
				return Substitute.For<ISSEStateEngine<ISBMrkState>>();
			}
			ISSEProcessEngine<ISBActProcess> MakeSubSBActProcessEngine(){
				return Substitute.For<ISSEProcessEngine<ISBActProcess>>();
			}
			ISSEProcessEngine<ISBEqpProcess> MakeSubSBEqpProcessEngine(){
				return Substitute.For<ISSEProcessEngine<ISBEqpProcess>>();
			}
			ISSEProcessEngine<ISBMrkProcess> MakeSubSBMrkProcessEngine(){
				return Substitute.For<ISSEProcessEngine<ISBMrkProcess>>();
			}
		}
	}
}
