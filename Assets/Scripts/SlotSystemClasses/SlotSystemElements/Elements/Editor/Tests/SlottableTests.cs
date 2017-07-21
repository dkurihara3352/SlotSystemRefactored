﻿using UnityEngine;
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
			/*	States*/
				/*	SelState	*/
					[Test]
					public void SetSelState_Null_SetsSelStateNull(){
						Slottable sb = MakeSB();
						
						sb.SetSelState(null);

						Assert.That(sb.curSelState, Is.Null);
					}
					[Test]
					public void SetSelState_SBSelState_SetsSelState(){
						Slottable sb = MakeSB();
						SBSelState stubSelState = MakeSubSBSelState();

						sb.SetSelState(stubSelState);

						Assert.That(sb.curSelState, Is.SameAs(stubSelState));
					}
					[TestCaseSource(typeof(SetSelStateInvalidStateCases))]
					public void SetSelState_InvalidState_ThrowsException(SSEState state){
						Slottable sb = MakeSB();

						Exception ex = Assert.Catch<ArgumentException>(() => sb.SetSelState(state));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetSelState: something other than SBSelState is beint attempted to be assigned"));
					}
						class SetSelStateInvalidStateCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBActState();
								yield return MakeSubSBEqpState();
								yield return MakeSubSBMrkState();
								yield return MakeSubSGSelState();
								yield return MakeSubSGActState();
								yield return MakeSubSSMSelState();
								yield return MakeSubSSMActState();
							}
						}
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
					[TestCaseSource(typeof(SetActStateInvalidStateCases))]
					public void SetActState_InvalidState_ThrowsException(SSEState state){
						Slottable sb = MakeSB();

						Exception ex = Assert.Catch<ArgumentException>(() => sb.SetActState(state));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetActState: something other than SBActionState is being attempted to be assigned"));
					}
						class SetActStateInvalidStateCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBSelState();
								yield return MakeSubSBEqpState();
								yield return MakeSubSBMrkState();
								yield return MakeSubSGSelState();
								yield return MakeSubSGActState();
								yield return MakeSubSSMSelState();
								yield return MakeSubSSMActState();
							}
						}
				/*	EqpState	*/
					[Test]
					public void SetEqpState_Null_CallsEqpStateEngineSetState(){
						Slottable sb = MakeSB();
						ISSEStateEngine mockEngine = MakeSubSSEStateEngine();
						sb.SetEqpStateEngine(mockEngine);
						sb.SetEqpState(null);

						mockEngine.Received().SetState(null);
					}
					[Test]
					public void SetEqpState_SBEqpState_CallsEqpStateEngineSetState(){
						Slottable sb = MakeSB();
						ISSEStateEngine mockEngine = MakeSubSSEStateEngine();
						sb.SetEqpStateEngine(mockEngine);
						SBEqpState stubEqpState = MakeSubSBEqpState();
						sb.SetEqpState(stubEqpState);

						mockEngine.Received().SetState(stubEqpState);
					}
					[TestCaseSource(typeof(SetEqpStateInvalidStateCases))]
					public void SetEqpState_InvalidState_ThrowsException(SSEState state){
						Slottable sb = MakeSB();
						ISSEStateEngine mockEngine = MakeSubSSEStateEngine();
						sb.SetEqpStateEngine(mockEngine);

						Exception ex = Assert.Catch<ArgumentException>(() => sb.SetEqpState(state));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetEqpState: something other than SBEqpState is trying to be assinged"));
					}
						class SetEqpStateInvalidStateCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBSelState();
								yield return MakeSubSBActState();
								yield return MakeSubSBMrkState();
								yield return MakeSubSGSelState();
								yield return MakeSubSGActState();
								yield return MakeSubSSMSelState();
								yield return MakeSubSSMActState();
							}
						}
				/*	EqpState	*/
					[Test]
					public void SetMrkState_Null_CallsMrkStateEngineSetState(){
						Slottable sb = MakeSB();
						ISSEStateEngine mockEngine = MakeSubSSEStateEngine();
						sb.SetMrkStateEngine(mockEngine);
						sb.SetMrkState(null);

						mockEngine.Received().SetState(null);
					}
					[Test]
					public void SetMrkState_SBMrkState_CallsMrkStateEngineSetState(){
						Slottable sb = MakeSB();
						ISSEStateEngine mockEngine = MakeSubSSEStateEngine();
						sb.SetMrkStateEngine(mockEngine);
						SBMrkState stubMrkState = MakeSubSBMrkState();
						sb.SetMrkState(stubMrkState);

						mockEngine.Received().SetState(stubMrkState);
					}
					[TestCaseSource(typeof(SetMrkStateInvalidStateCases))]
					public void SetMrkState_InvalidState_ThrowsException(SSEState state){
						Slottable sb = MakeSB();
						ISSEStateEngine mockEngine = MakeSubSSEStateEngine();
						sb.SetMrkStateEngine(mockEngine);

						Exception ex = Assert.Catch<ArgumentException>(() => sb.SetMrkState(state));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetMrkState: something other than SBMrkState is trying to be assinged"));
					}
						class SetMrkStateInvalidStateCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBSelState();
								yield return MakeSubSBActState();
								yield return MakeSubSBEqpState();
								yield return MakeSubSGSelState();
								yield return MakeSubSGActState();
								yield return MakeSubSSMSelState();
								yield return MakeSubSSMActState();
							}
						}
			/*	Process	*/
				/*	SelProc	*/
					[Test]
					public void SetAndRunSelState_Null_SetsSelProcNull(){
						Slottable sb = MakeSB();
						sb.SetAndRunSelProcess(null);

						Assert.That(sb.selProcess, Is.Null);
					}
					[Test]
					public void SetAndRunSelProcess_SBSelProcess_SetsSelProc(){
						Slottable sb = MakeSB();
						ISBSelProcess stubProc = MakeSubSBSelProc();
						sb.SetAndRunSelProcess(stubProc);

						Assert.That(sb.selProcess, Is.SameAs(stubProc));
					}
					[TestCaseSource(typeof(SetAndRunSelProcessInvalidProcessCases))]
					public void SetAndRunSelProcess_InvalidProcess_ThrowsException(ISSEProcess proc){
						Slottable sb = MakeSB();
						
						Exception ex = Assert.Catch<ArgumentException>( () => sb.SetAndRunSelProcess(proc));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetAndRunSelProcess: argument is not of type ISBSelProcess"));
					}
						class SetAndRunSelProcessInvalidProcessCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBActProc();
								yield return MakeSubSBEqpProc();
								yield return MakeSubSBMrkProc();
								yield return MakeSubSGSelProc();
								yield return MakeSubSGActProc();
								yield return MakeSubSSMSelProc();
								yield return MakeSubSSMActProc();
							}
						}
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
					[TestCaseSource(typeof(SetAndRunActProcessInvalidProcessCases))]
					public void SetAndRunActProcess_InvalidProcess_ThrowsException(ISSEProcess proc){
						Slottable sb = MakeSB();
						
						Exception ex = Assert.Catch<ArgumentException>( () => sb.SetAndRunActProcess(proc));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetAndRunActProcess: argument is not of type ISBActProcess"));
					}
						class SetAndRunActProcessInvalidProcessCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBSelProc();
								yield return MakeSubSBEqpProc();
								yield return MakeSubSBMrkProc();
								yield return MakeSubSGSelProc();
								yield return MakeSubSGActProc();
								yield return MakeSubSSMSelProc();
								yield return MakeSubSSMActProc();
							}
						}
				/*	EqpProc	*/
					[Test]
					public void SetAndRunEqpState_Null_CallsEngineWithNull(){
						Slottable sb = MakeSB();
						ISSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.SetEqpProcessEngine(mockEngine);
						sb.SetAndRunEqpProcess(null);

						mockEngine.Received().SetAndRunProcess(null);
					}
					[Test]
					public void SetAndRunEqpProcess_ISBEqpProcess_CallsEqpProcEngineWithTheProc(){
						Slottable sb = MakeSB();
						ISSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.SetEqpProcessEngine(mockEngine);
						ISBEqpProcess stubProc = MakeSubSBEqpProc();
						sb.SetAndRunEqpProcess(stubProc);

						mockEngine.Received().SetAndRunProcess(stubProc);
					}
					[TestCaseSource(typeof(SetAndRunEqpProcessInvalidProcessCases))]
					public void SetAndRunEqpProcess_InvalidProcess_ThrowsException(ISSEProcess proc){
						Slottable sb = MakeSB();
						ISSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.SetEqpProcessEngine(mockEngine);
						
						Exception ex = Assert.Catch<ArgumentException>( () => sb.SetAndRunEqpProcess(proc));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetAndRunEquipProcess: argument is not of type ISBEqpProcess"));
					}
						class SetAndRunEqpProcessInvalidProcessCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBSelProc();
								yield return MakeSubSBActProc();
								yield return MakeSubSBMrkProc();
								yield return MakeSubSGSelProc();
								yield return MakeSubSGActProc();
								yield return MakeSubSSMSelProc();
								yield return MakeSubSSMActProc();
							}
						}
				/*	MrkProc	*/
					[Test]
					public void SetAndRunMrkState_Null_CallsEngineWithNull(){
						Slottable sb = MakeSB();
						ISSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.SetMrkProcessEngine(mockEngine);
						sb.SetAndRunMrkProcess(null);

						mockEngine.Received().SetAndRunProcess(null);
					}
					[Test]
					public void SetAndRunMrkProcess_ISBMrkProcess_CallsMrkProcEngineWithTheProc(){
						Slottable sb = MakeSB();
						ISSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.SetMrkProcessEngine(mockEngine);
						ISBMrkProcess stubProc = MakeSubSBMrkProc();
						sb.SetAndRunMrkProcess(stubProc);

						mockEngine.Received().SetAndRunProcess(stubProc);
					}
					[TestCaseSource(typeof(SetAndRunMrkProcessInvalidProcessCases))]
					public void SetAndRunMrkProcess_InvalidProcess_ThrowsException(ISSEProcess proc){
						Slottable sb = MakeSB();
						ISSEProcessEngine mockEngine = MakeSubSSEProcEngine();
						sb.SetMrkProcessEngine(mockEngine);
						Exception ex = Assert.Catch<ArgumentException>( () => sb.SetAndRunMrkProcess(proc));

						Assert.That(ex.Message, Is.StringContaining("Slottable.SetAndRunEquipProcess: argument is not of type ISBMrkProcess"));
					}
						class SetAndRunMrkProcessInvalidProcessCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return MakeSubSBSelProc();
								yield return MakeSubSBActProc();
								yield return MakeSubSBEqpProc();
								yield return MakeSubSGSelProc();
								yield return MakeSubSGActProc();
								yield return MakeSubSSMSelProc();
								yield return MakeSubSSMActProc();
							}
						}

			/*	Methods	*/
				[Test][Category("Methods")]
				public void InitializeStates_WhenCalled_InitializesStates(){
					Slottable sb = MakeSB();

					sb.InitializeStates();

					Assert.That(sb.curSelState, Is.SameAs(Slottable.sbDeactivatedState));
					Assert.That(sb.prevSelState, Is.SameAs(Slottable.sbDeactivatedState));
					Assert.That(sb.curActState, Is.SameAs(Slottable.sbWaitForActionState));
					Assert.That(sb.prevActState, Is.SameAs(Slottable.sbWaitForActionState));
					Assert.That(sb.curEqpState, Is.Null);
					Assert.That(sb.prevEqpState, Is.Null);
					Assert.That(sb.curMrkState, Is.SameAs(Slottable.unmarkedState));
					Assert.That(sb.prevMrkState, Is.SameAs(Slottable.unmarkedState));
				}
				[Test]
				[Category("Methods")]
				public void Pickup_WhenCalled_SetsPickedUpState(){
					Slottable sb = MakeSB();
					sb.SetSSM(MakeSubSSM());
					sb.SetSelState(MakeSubSBSelState());
					
					sb.PickUp();

					Assert.That(sb.curActState, Is.SameAs(Slottable.pickedUpState));
				}
				[Test]
				[Category("Methods")]
				public void Pickup_WhenCalled_SetsPickedAmountOne(){
					Slottable sb = MakeSB();
					sb.SetSSM(MakeSubSSM());
					sb.SetSelState(MakeSubSBSelState());
					
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
					mockProc.isRunning = true;
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
			ISSEStateEngine MakeSubSSEStateEngine(){
				return Substitute.For<ISSEStateEngine>();
			}
			ISSEProcessEngine MakeSubSSEProcEngine(){
				return Substitute.For<ISSEProcessEngine>();
			}
		}
	}
}
