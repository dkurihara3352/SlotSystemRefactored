using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Utility;
namespace SlotSystemTests{
	namespace ElementsTests{
		[TestFixture]
		[Category("SSM")]
		public class SlotSystemManagerTests: SlotSystemTest{
			[Test]
			public void SetCurSSM_WhenCalled_CallsPrevSSMDefocus(){
				SlotSystemManager ssm = MakeSSM();
				ISlotSystemManager mockSSM = MakeSubSSM();
				SlotSystemManager.curSSM = mockSSM;

				ssm.SetCurSSM();

				mockSSM.Received().Defocus();
				}
			[Test]
			public void SetCurSSM_WhenCalled_SetsThisAsCur(){
				SlotSystemManager ssm = MakeSSM();
				ISlotSystemManager stubSSM = MakeSubSSM();
				SlotSystemManager.curSSM = stubSSM;

				ssm.SetCurSSM();

				Assert.That(SlotSystemManager.curSSM, Is.SameAs(ssm));
				}
			[Test]
			public void InspectorSetUp_WhenCalled_SetsBundles(){
				SlotSystemManager ssm = MakeSSM();
					ISlotSystemBundle pBun = MakeSubBundle();
					ISlotSystemBundle eBun = MakeSubBundle();
					IEnumerable<ISlotSystemBundle> gBuns;
						ISlotSystemBundle gBunA = MakeSubBundle();
						ISlotSystemBundle gBunB = MakeSubBundle();
						ISlotSystemBundle gBunC = MakeSubBundle();
						gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};
					
				ssm.InspectorSetUp(pBun, eBun, gBuns);

				Assert.That(ssm.poolBundle, Is.SameAs(pBun));
				Assert.That(ssm.equipBundle, Is.SameAs(eBun));
				bool equality = ssm.otherBundles.MemberEquals(gBuns);
				Assert.That(equality, Is.True);
				}
			[Test]
			public void SetElements_Always_DoNothing(){
				SlotSystemManager ssm = MakeSSM();
					TestSlotSystemElement sseA = MakeTestSSE();
						sseA.transform.SetParent(ssm.transform);
					TestSlotSystemElement sseB = MakeTestSSE();
						sseB.transform.SetParent(ssm.transform);
					TestSlotSystemElement sseC = MakeTestSSE();
						sseC.transform.SetParent(ssm.transform);
				IEnumerable<ISlotSystemElement> expected = new ISlotSystemElement[]{null, null};
				ssm.SetHierarchy();
				
				bool equality = ssm.MemberEquals(expected);
				Assert.That(equality, Is.True);
				}
			[Test]
			public void Initialize_WhenCalled_CallsSetSSMInHierarchy(){
				SlotSystemManager ssm = MakeSSM();
					ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
				ssm.InspectorSetUp(pBun, eBun, gBuns);
				ssm.SetHierarchy();

				ssm.Initialize();
				
				Assert.That(ssm.ssm, Is.SameAs(ssm));
				pBun.Received().PerformInHierarchy(ssm.SetSSMInH);
				eBun.Received().PerformInHierarchy(ssm.SetSSMInH);
				foreach(var gBun in gBuns)
					gBun.Received().PerformInHierarchy(ssm.SetSSMInH);
				}
			[Test]
			public void Initialize_WhenCalled_CallsSetParentInHierarchy(){
				SlotSystemManager ssm = MakeSSM();
					ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
				ssm.InspectorSetUp(pBun, eBun, gBuns);
				ssm.SetHierarchy();

				ssm.Initialize();
				
				Assert.That(ssm.parent, Is.Null);
				pBun.Received().PerformInHierarchy(ssm.SetParent);
				eBun.Received().PerformInHierarchy(ssm.SetParent);
				foreach(var gBun in gBuns)
					gBun.Received().PerformInHierarchy(ssm.SetParent);
				}
			[Test]
			public void Initialize_WhenCalled_CallsPIHInitializeState(){
				SlotSystemManager ssm = MakeSSM();
					ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
				ssm.InspectorSetUp(pBun, eBun, gBuns);
				ssm.SetHierarchy();

				ssm.Initialize();

				Assert.That(ssm.isDeactivated, Is.True);
				Assert.That(ssm.isSelStateInit, Is.True);
				Assert.That(ssm.isWaitingForAction, Is.True);
				Assert.That(ssm.isActStateInit, Is.True);
				pBun.Received().PerformInHierarchy(ssm.InitStatesInHi);
				eBun.Received().PerformInHierarchy(ssm.InitStatesInHi);
				foreach(var gBun in gBuns)
					gBun.Received().PerformInHierarchy(ssm.InitStatesInHi);
				}
			/*	fields */
				[Test]
				public void AllSGs_Always_CallsPIHAddInSGListInSequence(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					List<ISlotGroup> list = ssm.allSGs;
					Received.InOrder(() => {
						pBun.PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
						eBun.PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
						foreach(var gBun in gBuns){
							gBun.PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
						}
					});
					}
				[Test]
				public void AddInSGList_WhenCalled_VerifySGsAndStoreThemInTheList(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable sb = MakeSubSB();
					ISlotSystemManager stubSSM = MakeSubSSM();
					ISlotSystemElement ele = MakeSubSSE();
					ISlotGroup sgA = MakeSubSG();
					ISlotGroup sgB = MakeSubSG();
					IEnumerable<ISlotSystemElement> sses = new ISlotSystemElement[]{
						sb, stubSSM, ele, sgA, ele, sgB
					};
					IEnumerable<ISlotGroup> expected = new ISlotGroup[]{sgA, sgB};
					List<ISlotGroup> sgs = new List<ISlotGroup>();

					foreach(var e in sses)
						ssm.AddInSGList(e, sgs);
					
					Assert.That(sgs.MemberEquals(expected), Is.True);
					}
				[Test]
				public void AllSGPs_Always_CallsPoolBundlePIHAddInSGList(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					List<ISlotGroup> list = ssm.allSGPs;
					pBun.Received().PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
					}
				[Test]
				public void AllSGEs_Always_CallsEquipBundlePIHAddINSGList(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					List<ISlotGroup> list = ssm.allSGEs;
					eBun.Received().PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
					}
				[Test]
				public void AllSGGs_Always_CallsAllGenBundlesPIHAddInSGList(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					List<ISlotGroup> list = ssm.allSGGs;

					foreach(var gBun in gBuns){
						gBun.Received().PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
					}
					}
				[Test]
				public void FocusedSGP_PoolBundleToggledOn_ReturnsPoolBundleFocusedElement(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
							ISlotGroup focusedSG = MakeSubSGWithEmptySBs();
								pBun.focusedElement.Returns(focusedSG);
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();
					
					ISlotGroup actual = ssm.focusedSGP;

					Assert.That(actual, Is.SameAs(focusedSG));
					}
				[Test]
				public void FocusedEpSet_EquipBundleIsToggledOn_ReturnsEquipBundleFocusedElement(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
							IEquipmentSet focusedESet = MakeSubEquipmentSetInitWithSGs();
								eBun.focusedElement.Returns(focusedESet);
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					IEquipmentSet actual = ssm.focusedEqSet;

					Assert.That(actual, Is.SameAs(focusedESet));
					}
				[Test]
				public void FocusedSGEs_EquipBundleIsToggledOn_ReturnsFocusedESetSGs(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
							IEquipmentSet focusedESet = MakeSubEquipmentSetInitWithSGs();
								IEnumerable<ISlotSystemElement> focusedESetEles;
									ISlotGroup sgeA = MakeSubSGWithEmptySBs();
									ISlotGroup sgeB = MakeSubSGWithEmptySBs();
									ISlotGroup sgeC = MakeSubSGWithEmptySBs();
									focusedESetEles = new ISlotSystemElement[]{
										sgeA, sgeB, sgeC
									};
								focusedESet.GetEnumerator().Returns(focusedESetEles.GetEnumerator());
							eBun.focusedElement.Returns(focusedESet);
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					IEnumerable<ISlotGroup> actual = ssm.focusedSGEs;
					IEnumerable<ISlotSystemElement> convertedActual = ConvertToSSEs(actual);
					Assert.That(convertedActual.MemberEquals<ISlotSystemElement>(focusedESetEles), Is.True);
					}
				
				[TestCaseSource(typeof(AddFocusedTo_VariousConfigCases))]
				public void AddFocusedSGTo_VariousConfig_ReturnsAccordingly(ISlotSystemElement ele, IEnumerable<ISlotGroup> expected){
					SlotSystemManager ssm = MakeSSM();
					List<ISlotGroup> list = new List<ISlotGroup>();
					ssm.AddFocusedSGTo(ele, list);

					Assert.That(list.MemberEquals(expected), Is.True);
					}
					class AddFocusedTo_VariousConfigCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							/*	No SG	*/
								ISlotSystemElement notSG = MakeSubSSE();
								notSG.isFocused.Returns(true);
								yield return new object[]{notSG, new ISlotGroup[]{}};
							/*	Not FocusedInHierarchy	*/
								ISlotGroup notFocused = MakeSubSG();
								notFocused.isFocusedInHierarchy.Returns(false);
								yield return new object[]{notFocused, new ISlotGroup[]{}};
							/*	Valid	*/
								ISlotGroup validSG = MakeSubSGWithEmptySBs();
								validSG.isFocusedInHierarchy.Returns(true);
								yield return new object[]{validSG, new ISlotGroup[]{validSG}};
								
						}
					}
				[Test]
				public void FocusedSGGs_Always_CallsAllGenBundlesPIHAddFocusedSGTo(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();
					
					List<ISlotGroup> list = ssm.focusedSGGs;
					
					foreach(var gBun in gBuns)
						gBun.Received().PerformInHierarchy(ssm.AddFocusedSGTo, Arg.Any<List<ISlotGroup>>());
					}
				[TestCaseSource(typeof(FocusedSGsCases))]
				public void FocusedSGs_Always_ReturnsAllTheFocusedSGsInPBunAndEBunAndCallAllGBunPIHAddFocusedSGTo(ISlotSystemBundle pBun, ISlotSystemBundle eBun, IEnumerable<ISlotSystemBundle> gBuns, List<ISlotGroup> expected){
					SlotSystemManager ssm = MakeSSM();
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();
					ssm.Focus();

					List<ISlotGroup> actual = ssm.focusedSGs;

					Assert.That(actual.MemberEquals(expected), Is.True);
					foreach(var gBun in gBuns)
						gBun.Received().PerformInHierarchy(ssm.AddFocusedSGTo, Arg.Any<List<ISlotGroup>>());
					}
					class FocusedSGsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlotSystemBundle pBun = MakeSubBundle();
								ISlotGroup sgpA = MakeSubSGWithEmptySBs();
								ISlotGroup sgpB = MakeSubSGWithEmptySBs();
								ISlotGroup sgpC = MakeSubSGWithEmptySBs();
								IEnumerable<ISlotSystemElement> pBunEles = new ISlotSystemElement[]{
									sgpA, sgpB, sgpC
								};
								pBun.GetEnumerator().Returns(pBunEles.GetEnumerator());
								pBun.focusedElement.Returns(sgpA);
							ISlotSystemBundle eBun = MakeSubBundle();
								IEnumerable<ISlotSystemElement> eBunEles;
									IEquipmentSet eSetA = MakeSubEquipmentSetInitWithSGs();
										IEnumerable<ISlotSystemElement> eSetAEles;
											ISlotGroup sgBowA = MakeSubSGWithEmptySBs();		
											ISlotGroup sgWearA = MakeSubSGWithEmptySBs();
											ISlotGroup sgCGearsA = MakeSubSGWithEmptySBs();
										eSetAEles = new ISlotSystemElement[]{
											sgBowA, sgWearA, sgCGearsA
										};
										eSetA.GetEnumerator().Returns(eSetAEles.GetEnumerator());
									IEquipmentSet eSetB = MakeSubEquipmentSetInitWithSGs();
										IEnumerable<ISlotSystemElement> eSetBEles;
											ISlotGroup sgBowB = MakeSubSGWithEmptySBs();		
											ISlotGroup sgWearB = MakeSubSGWithEmptySBs();
											ISlotGroup sgCGearsB = MakeSubSGWithEmptySBs();
										eSetBEles = new ISlotSystemElement[]{
											sgBowB, sgWearB, sgCGearsB
										};
										eSetB.GetEnumerator().Returns(eSetBEles.GetEnumerator());
									IEquipmentSet eSetC = MakeSubEquipmentSetInitWithSGs();
										IEnumerable<ISlotSystemElement> eSetCEles;
											ISlotGroup sgBowC = MakeSubSGWithEmptySBs();		
											ISlotGroup sgWearC = MakeSubSGWithEmptySBs();
											ISlotGroup sgCGearsC = MakeSubSGWithEmptySBs();
										eSetCEles = new ISlotSystemElement[]{
											sgBowC, sgWearC, sgCGearsC
										};
										eSetC.GetEnumerator().Returns(eSetCEles.GetEnumerator());
								eBunEles = new ISlotSystemElement[]{
									eSetA, eSetB, eSetC
								};
								eBun.GetEnumerator().Returns(eBunEles.GetEnumerator());
								eBun.focusedElement.Returns(eSetC);
							IEnumerable<ISlotSystemBundle> gBuns;
								ISlotSystemBundle genBundleA = MakeSubBundle();
									IEnumerable<ISlotSystemElement> gBunAEles;
										ISlotGroup sggA_A = MakeSubSGWithEmptySBs();
										ISlotGroup sggA_B = MakeSubSGWithEmptySBs();
										ISlotGroup sggA_C = MakeSubSGWithEmptySBs();
										gBunAEles = new ISlotSystemElement[]{
											sggA_A, sggA_B, sggA_C
										};
									genBundleA.GetEnumerator().Returns(gBunAEles.GetEnumerator());
								ISlotSystemBundle genBundleB = MakeSubBundle();
									IEnumerable<ISlotSystemElement> gBunBEles;
										ISlotGroup sggB_A = MakeSubSGWithEmptySBs();
										ISlotGroup sggB_B = MakeSubSGWithEmptySBs();
										ISlotGroup sggB_C = MakeSubSGWithEmptySBs();
										gBunBEles = new ISlotSystemElement[]{
											sggB_A, sggB_B, sggB_C
										};
									genBundleB.GetEnumerator().Returns(gBunBEles.GetEnumerator());
								gBuns = new ISlotSystemBundle[]{genBundleA, genBundleB};
							List<ISlotGroup> case1Exp = new List<ISlotGroup>(new ISlotGroup[]{
								sgpA, sgBowC, sgWearC, sgCGearsC
							});
							yield return new object[]{
								pBun, eBun, gBuns, case1Exp
							};
						}
					}
				[Test]
				public void focusedSGs_Always_ReturnsFactoryProduct(){
					SlotSystemManager ssm = MakeSSM();
					IFocusedSGsFactory fac = Substitute.For<IFocusedSGsFactory>();
					ISlotGroup sgA = MakeSubSG();
					ISlotGroup sgB = MakeSubSG();
					ISlotGroup sgC = MakeSubSG();
					List<ISlotGroup> list = new List<ISlotGroup>(new ISlotGroup[]{sgA, sgB , sgC});
					fac.focusedSGs.Returns(list);
					ssm.SetFocusedSGsFactory(fac);

					List<ISlotGroup> actual = ssm.focusedSGs;

					Assert.That(actual.MemberEquals(list), Is.True);
					}
				[Test]
				public void EquipmentSets_Always_ReturnsEquipBundleElements(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
							IEnumerable<ISlotSystemElement> eBunEles;
								IEquipmentSet eSetA = SlotSystemTest.MakeSubEquipmentSetInitWithSGs();
								IEquipmentSet eSetB = SlotSystemTest.MakeSubEquipmentSetInitWithSGs();
								IEquipmentSet eSetC = SlotSystemTest.MakeSubEquipmentSetInitWithSGs();						
								eBunEles = new ISlotSystemElement[]{
									eSetA, eSetB, eSetC
								};
								eBun.GetEnumerator().Returns(eBunEles.GetEnumerator());
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					IEnumerable<ISlotSystemElement> actual = ConvertToSSEs(ssm.equipmentSets);
					Assert.That(actual.MemberEquals(eBunEles), Is.True);
					}
				[Test]
				public void poolInv_Always_ReturnsFocusedSGPsInventory(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
							List<ISlotSystemElement> sbs = new List<ISlotSystemElement>();
								ISlotGroup sgpA = MakeSubSG();
									sgpA.GetEnumerator().Returns(sbs.GetEnumerator());
								ISlotGroup sgpB = MakeSubSG();
									sgpB.GetEnumerator().Returns(sbs.GetEnumerator());
								ISlotGroup sgpC = MakeSubSG();
									sgpC.GetEnumerator().Returns(sbs.GetEnumerator());
							pBun.elements.Returns(new ISlotSystemElement[]{
								sgpA, sgpB, sgpC
							});
							IPoolInventory pInv = MakeSubPoolInv();
								sgpA.inventory.Returns(pInv);
							pBun.focusedElement.Returns(sgpA);
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					IPoolInventory actual = ssm.poolInv;

					Assert.That(actual, Is.SameAs(pInv));
					}
				[Test]
				public void equipInv_Always_ReturnsFocusedSGEsInventory(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
							IEquipmentSet eSet = MakeSubEquipmentSetInitWithSGs();
							eBun.focusedElement.Returns(eSet);
								IEnumerable<ISlotSystemElement> eSetEles;
									ISlotGroup sgeA = MakeSubSGWithEmptySBs();
										IEquipmentSetInventory eInv = MakeSubEquipInv();
										sgeA.inventory.Returns(eInv);
									ISlotGroup sgeB = MakeSubSGWithEmptySBs();
									ISlotGroup sgeC = MakeSubSGWithEmptySBs();
									eSetEles = new ISlotSystemElement[]{
										sgeA, sgeB, sgeC
									};
								eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();
					
					IEquipmentSetInventory actual = ssm.equipInv;

					Assert.That(actual, Is.SameAs(eInv));
					}
				[Test]
				public void equippedBowInst_Always_ReturnsFocusedSGEWithBowFilterFirtSlotSBItemInst(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
							IEnumerable<ISlotSystemElement> eBunEles;
								IEquipmentSet eSet = MakeSubEquipmentSetInitWithSGs();
									IEnumerable<ISlotSystemElement> eSetEles;
										ISlotGroup sgeBow = MakeSubSG();
											ISlottable bowSBE = MakeSubSB();
												BowInstance bowE = MakeBowInstance(0);
												bowSBE.itemInst.Returns(bowE);
												BowInstance expected = bowE;
											sgeBow[0].Returns(bowSBE);
											sgeBow.filter.Returns(new SGBowFilter());
										ISlotGroup sgeWear = MakeSubSG();
										ISlotGroup sgeCGears = MakeSubSG();
										eSetEles = new ISlotSystemElement[]{
											sgeBow, sgeWear, sgeCGears
										};
									eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
								eBunEles = new ISlotSystemElement[]{
									eSet
								};
							eBun.GetEnumerator().Returns(eBunEles.GetEnumerator());
							eBun.focusedElement.Returns(eSet);
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					BowInstance actual = ssm.equippedBowInst;

					Assert.That(actual, Is.SameAs(expected));
					}
				[Test]
				public void equippedWearInst_Always_ReturnsFocusedSGEWithWearFilterFirtSlotSBItemInst(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
							IEnumerable<ISlotSystemElement> eBunEles;
								IEquipmentSet eSet = MakeSubEquipmentSetInitWithSGs();
									IEnumerable<ISlotSystemElement> eSetEles;
										ISlotGroup sgeBow = MakeSubSG();
										ISlotGroup sgeWear = MakeSubSG();
											ISlottable wearSBE = MakeSubSB();
												WearInstance wearE = MakeWearInstance(0);
												wearSBE.itemInst.Returns(wearE);
												WearInstance expected = wearE;
											sgeWear[0].Returns(wearSBE);
											sgeWear.filter.Returns(new SGWearFilter());
										ISlotGroup sgeCGears = MakeSubSG();
										eSetEles = new ISlotSystemElement[]{
											sgeBow, sgeWear, sgeCGears
										};
									eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
								eBunEles = new ISlotSystemElement[]{
									eSet
								};
							eBun.GetEnumerator().Returns(eBunEles.GetEnumerator());
							eBun.focusedElement.Returns(eSet);
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					WearInstance actual = ssm.equippedWearInst;

					Assert.That(actual, Is.SameAs(expected));
					}
				[Test]
				public void equippedCarriedGears_Always_ReturnsFocusedSGEWithCGFilterAllElements(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
							IEnumerable<ISlotSystemElement> eBunEles;
								IEquipmentSet eSet = MakeSubEquipmentSetInitWithSGs();
									IEnumerable<ISlotSystemElement> eSetEles;
										ISlotGroup sgeBow = MakeSubSG();
										ISlotGroup sgeWear = MakeSubSG();
										ISlotGroup sgeCGears = MakeSubSG();
											IEnumerable<ISlotSystemElement> sgeCGearsEles;
												ISlottable shieldSBE = MakeSubSB();
													ShieldInstance shieldE = MakeShieldInstance(0);
													shieldSBE.itemInst.Returns(shieldE);
												ISlottable mWeaponSBE = MakeSubSB();
													MeleeWeaponInstance mWeaponE = MakeMeleeWeaponInstance(0);
													mWeaponSBE.itemInst.Returns(mWeaponE);
												sgeCGearsEles = new ISlotSystemElement[]{shieldSBE, mWeaponSBE};
												List<CarriedGearInstance> expected = new List<CarriedGearInstance>(new CarriedGearInstance[]{shieldE, mWeaponE});
											sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
											sgeCGears.filter.Returns(new SGCGearsFilter());
										eSetEles = new ISlotSystemElement[]{
											sgeBow, sgeWear, sgeCGears
										};
									eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
								eBunEles = new ISlotSystemElement[]{
									eSet
								};
							eBun.GetEnumerator().Returns(eBunEles.GetEnumerator());
							eBun.focusedElement.Returns(eSet);
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					IEnumerable<CarriedGearInstance> actual = ssm.equippedCarriedGears;

					Assert.That(actual.MemberEquals(expected), Is.True);
					}
				[Test]
				public void allEquippedItems_Always_ReturnsSumOfAllThree(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();	
						ISlotSystemBundle eBun = MakeSubBundle();
							IEnumerable<ISlotSystemElement> eBunEles;
								IEquipmentSet eSet = Substitute.For<IEquipmentSet>();
									IEnumerable<ISlotSystemElement> eSetEles;
										ISlotGroup sgeBow = MakeSubSG();
											ISlottable bowSBE = MakeSubSB();
												BowInstance bowE = MakeBowInstance(0);
												bowSBE.itemInst.Returns(bowE);
											sgeBow[0].Returns(bowSBE);
											sgeBow.filter.Returns(new SGBowFilter());
										ISlotGroup sgeWear = MakeSubSG();
											ISlottable wearSBE = MakeSubSB();
												WearInstance wearE = MakeWearInstance(0);
												wearSBE.itemInst.Returns(wearE);
											sgeWear[0].Returns(wearSBE);
											sgeWear.filter.Returns(new SGWearFilter());
										ISlotGroup sgeCGears = MakeSubSG();
											IEnumerable<ISlotSystemElement> sgeCGearsEles;
												ISlottable shieldSBE = MakeSubSB();
													ShieldInstance shieldE = MakeShieldInstance(0);
													shieldSBE.itemInst.Returns(shieldE);
												ISlottable mWeaponSBE = MakeSubSB();
													MeleeWeaponInstance mWeaponE = MakeMeleeWeaponInstance(0);
													mWeaponSBE.itemInst.Returns(mWeaponE);
												sgeCGearsEles = new ISlotSystemElement[]{shieldSBE, mWeaponSBE};
											sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
											sgeCGears.filter.Returns(new SGCGearsFilter());
										eSetEles = new ISlotSystemElement[]{
											sgeBow, sgeWear, sgeCGears
										};
									eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
								eBunEles = new ISlotSystemElement[]{eSet};
							eBun.GetEnumerator().Returns(eBunEles.GetEnumerator());
							eBun.focusedElement.Returns(eSet);
							List<InventoryItemInstance> expected = new List<InventoryItemInstance>(new InventoryItemInstance[]{
								bowE, wearE, shieldE, mWeaponE
							});
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);

					List<InventoryItemInstance> actual = ssm.allEquippedItems;

					Assert.That(actual.MemberEquals(expected), Is.True);
					}
				[Test]
				public void AddSBsToRes_WhenCalled_FindSBsAndAddThemIntoTheList(){
					SlotSystemManager ssm = MakeSSM();
						ISlotGroup sg = MakeSubSG();
						ISlottable sbA = MakeSubSB();
						ISlotSystemElement ele = Substitute.For<ISlotSystemElement>();
						ISlottable sbB = MakeSubSB();
						ISlotSystemBundle bundle = MakeSubBundle();
						IEnumerable<ISlotSystemElement> elements = new ISlotSystemElement[]{
							sg, sbA, ele, sbB, bundle
						};
						IEnumerable<ISlottable> expected = new ISlottable[]{
							sbA, sbB
						};
						List<ISlottable> actual = new List<ISlottable>();

						foreach(var e in elements)
							ssm.AddSBToRes(e, actual);
						
						Assert.That(actual.MemberEquals(expected), Is.True);
					}
				[Test]
				public void allSBs_WhenCalled_CallsAllBundlesPIHAddSBtoRes(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();
					
					List<ISlottable> list = ssm.allSBs;

					pBun.Received().PerformInHierarchy(ssm.AddSBToRes, Arg.Any<List<ISlottable>>());
					eBun.Received().PerformInHierarchy(ssm.AddSBToRes, Arg.Any<List<ISlottable>>());
					foreach(var gBun in gBuns)
						gBun.Received().PerformInHierarchy(ssm.AddSBToRes, Arg.Any<List<ISlottable>>());
					}
				[Test]
				public void FocusedSGsFactory_focusedSGs_Always_ReturnsSumOfCollections(){
					FocusedSGsFactory fac;
						ISlotSystemManager stubSSM = MakeSubSSM();
							ISlotGroup sgp = MakeSubSG();
							stubSSM.focusedSGP.Returns(sgp);
							List<ISlotGroup> sges;
								ISlotGroup sgeA = MakeSubSG();
								ISlotGroup sgeB = MakeSubSG();
								ISlotGroup sgeC = MakeSubSG();
								sges = new List<ISlotGroup>(new ISlotGroup[]{sgeA, sgeB, sgeC});
							List<ISlotGroup> sggs;
								ISlotGroup sggA = MakeSubSG();
								ISlotGroup sggB = MakeSubSG();
								ISlotGroup sggC = MakeSubSG();
								sggs = new List<ISlotGroup>(new ISlotGroup[]{sggA, sggB, sggC});
							stubSSM.focusedSGEs.Returns(sges);
							stubSSM.focusedSGGs.Returns(sggs);
					fac = new FocusedSGsFactory(stubSSM);
					List<ISlotGroup> expected = new List<ISlotGroup>(new ISlotGroup[]{
						sgp, sgeA, sgeB, sgeC, sggA, sggB, sggC
					});

					List<ISlotGroup> actual = fac.focusedSGs;

					Assert.That(actual.MemberEquals(expected), Is.True);
					}
			/*	methods	*/
				[Test]
				public void Reset_WhenCalled_SetsActStateWFA(){
					SlotSystemManager ssm = MakeSSM();

					ssm.Reset();

					Assert.That(ssm.isWaitingForAction, Is.True);
					}
				[Test]
				public void Reset_WhenCalled_SetsFieldsNull(){
					SlotSystemManager ssm = MakeSSM();
					ResetTestCase expected = new ResetTestCase(
						null, null, null, null, null, null, null, null
					);

					ssm.Reset();

					ResetTestCase actual = new ResetTestCase(
						ssm.pickedSB, ssm.targetSB, ssm.sg1, ssm.sg2, ssm.hovered,
						ssm.dIcon1, ssm.dIcon2, ssm.transaction
					);
					bool equality = actual.Equals(expected);
					Assert.That(equality, Is.True);
					}
					class ResetTestCase: IEquatable<ResetTestCase>{
						public ISlottable pickedSB;
						public ISlottable targetSB;
						public ISlotGroup sg1;
						public ISlotGroup sg2;
						public ISlotSystemElement hovered;
						public DraggedIcon dIcon1;
						public DraggedIcon dIcon2;
						public ISlotSystemTransaction transaction;
						public ResetTestCase(ISlottable pickedSB, ISlottable targetSB, ISlotGroup sg1, ISlotGroup sg2, ISlotSystemElement hovered, DraggedIcon dIcon1, DraggedIcon dIcon2, ISlotSystemTransaction transaction){
							this.pickedSB = pickedSB;
							this.targetSB = targetSB;
							this.sg1 = sg1;
							this.sg2 = sg2;
							this.hovered = hovered;
							this.dIcon1 = dIcon1;
							this.dIcon2 = dIcon2;
							this.transaction = transaction;
						}
						public bool Equals(ResetTestCase other){
							bool flag = true;
							flag &= BothNullOrReferenceEquals(this.pickedSB, other.pickedSB);
							flag &= BothNullOrReferenceEquals(this.targetSB, other.targetSB);
							flag &= BothNullOrReferenceEquals(this.sg1, other.sg1);
							flag &= BothNullOrReferenceEquals(this.sg2, other.sg2);
							flag &= BothNullOrReferenceEquals(this.hovered, other.hovered);
							flag &= BothNullOrReferenceEquals(this.dIcon1, other.dIcon1);
							flag &= BothNullOrReferenceEquals(this.dIcon2, other.dIcon2);
							flag &= BothNullOrReferenceEquals(this.transaction, other.transaction);
							return flag;
						}
					}
				[Test]
				public void ResetAndFocus_WhenCalled_SetsSelStateFocused(){
					SlotSystemManager ssm = MakeSSM();

					ssm.ResetAndFocus();

					Assert.That(ssm.isFocused, Is.True);
					}
				[Test]
				public void UpdateEquipStatesOnAll_WhenCalled_CallsEInvRemoveWithItemNotInAllEquippedItems(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
							ISlotGroup sgp = MakeSubSG();
								IPoolInventory pInv = MakeSubPoolInv();
								sgp.inventory.Returns(pInv);
							pBun.focusedElement.Returns(sgp);
						ISlotSystemBundle eBun = MakeSubBundle();
							IEquipmentSet eSet = Substitute.For<IEquipmentSet>();
								IEnumerable<ISlotSystemElement> eSetEles;
									ISlotGroup sgeBow = MakeSubSG();
										ISlottable sbeBow = MakeSubSB();
											BowInstance bowE = MakeBowInstance(0);
											sbeBow.itemInst.Returns(bowE);
										sgeBow[0].Returns(sbeBow);
										IEquipmentSetInventory eInv = MakeSubEquipInv();
										sgeBow.inventory.Returns(eInv);
											IEnumerable<SlottableItem> eInvEles;
												BowInstance bowR = MakeBowInstance(0);
												WearInstance wearR = MakeWearInstance(0);
												QuiverInstance quiverR = MakeQuiverInstance(0);
												PackInstance packR = MakePackInstance(0);
												eInvEles = new SlottableItem[]{
													bowR, wearR, quiverR, packR
												};
											eInv.GetEnumerator().Returns(eInvEles.GetEnumerator());
										sgeBow.filter.Returns(new SGBowFilter());
									ISlotGroup sgeWear = MakeSubSG();
										ISlottable sbeWear = MakeSubSB();
											WearInstance wearE = MakeWearInstance(0);
											sbeWear.itemInst.Returns(wearE);
										sgeWear[0].Returns(sbeWear);
										sgeWear.filter.Returns(new SGWearFilter());
									ISlotGroup sgeCGears = MakeSubSG();
										sgeCGears.filter.Returns(new SGCGearsFilter());
										IEnumerable<ISlotSystemElement> sgeCGearsEles = new ISlotSystemElement[]{};
										sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
								eSetEles = new ISlotSystemElement[]{sgeBow, sgeWear, sgeCGears};
								eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
							eBun.focusedElement.Returns(eSet);
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					ssm.UpdateEquipStatesOnAll();

					eInv.Received().Remove(bowR);
					eInv.Received().Remove(wearR);
					eInv.Received().Remove(quiverR);
					eInv.Received().Remove(packR);
					}
				[Test]
				public void UpdateEquipStatesOnAll_WhenCalled_CallsEInvAddWithItemNotInEquipInv(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
							ISlotGroup sgp = MakeSubSG();
								IPoolInventory pInv = MakeSubPoolInv();
								sgp.inventory.Returns(pInv);
							pBun.focusedElement.Returns(sgp);
						ISlotSystemBundle eBun = MakeSubBundle();
							IEquipmentSet eSet = Substitute.For<IEquipmentSet>();
								IEnumerable<ISlotSystemElement> eSetEles;
									ISlotGroup sgeBow = MakeSubSG();
										IEnumerable<ISlotSystemElement> sgeBowEles;
											ISlottable sbeBow = MakeSubSB();
												IEquipmentSetInventory eInv = MakeSubEquipInv();
														IEnumerable<SlottableItem> eInvEles = new SlottableItem[]{};
													eInv.GetEnumerator().Returns(eInvEles.GetEnumerator());
													sgeBow.inventory.Returns(eInv);
												BowInstance bow = MakeBowInstance(0);
												sbeBow.itemInst.Returns(bow);
											sgeBowEles = new ISlotSystemElement[]{sbeBow};
										// sgeBow.GetEnumerator().Returns(sgeBowEles.GetEnumerator());
										sgeBow[0].Returns(sbeBow);
										sgeBow.filter.Returns(new SGBowFilter());
									ISlotGroup sgeWear = MakeSubSG();
										IEnumerable<ISlotSystemElement> sgeWearEles;
											ISlottable sbeWear = MakeSubSB();
												WearInstance wear = MakeWearInstance(0);
												sbeWear.itemInst.Returns(wear);
											sgeWearEles = new ISlotSystemElement[]{sbeWear};
										// sgeWear.GetEnumerator().Returns(sgeWearEles.GetEnumerator());
										sgeWear[0].Returns(sbeWear);
										sgeWear.filter.Returns(new SGWearFilter());
									ISlotGroup sgeCGears = MakeSubSG();
										IEnumerable<ISlotSystemElement> sgeCGearsEles;
											ISlottable sbeShield = MakeSubSB();
												ShieldInstance shield = MakeShieldInstance(0);
												sbeShield.itemInst.Returns(shield);
											ISlottable sbeMWeapon = MakeSubSB();
												MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
												sbeMWeapon.itemInst.Returns(mWeapon);
											sgeCGearsEles = new ISlotSystemElement[]{sbeShield, sbeMWeapon};
										sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
										sgeCGears.filter.Returns(new SGCGearsFilter());
									eSetEles = new ISlotSystemElement[]{sgeBow, sgeWear, sgeCGears};
								eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
							eBun.focusedElement.Returns(eSet);
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();
					ssm.UpdateEquipStatesOnAll();

					eInv.Received().Add(bow);
					eInv.Received().Add(wear);
					eInv.Received().Add(shield);
					eInv.Received().Add(mWeapon);
					}
				[Test]
				public void UpdateEquipStatesOnAll_WhenCalled_UpdatePoolInvItemInstsEquipStatus(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
							ISlotGroup sgpAll = MakeSubSG();
								IPoolInventory pInv = MakeSubPoolInv();
								sgpAll.inventory.Returns(pInv);
							pBun.focusedElement.Returns(sgpAll);
						IEnumerable<ISlotSystemBundle> gBuns = new ISlotSystemBundle[]{};
						ISlotSystemBundle eBun = MakeSubBundle();
							IEquipmentSet eSet = Substitute.For<IEquipmentSet>();
								IEnumerable<ISlotSystemElement> eSetEles;
									ISlotGroup sgeBow = MakeSubSG();
										ISlottable sbeBow = MakeSubSB();
											BowInstance bow = MakeBowInstance(0);
											sbeBow.itemInst.Returns(bow);
										sgeBow[0].Returns(sbeBow);
										IEquipmentSetInventory eInv = MakeSubEquipInv();
										sgeBow.inventory.Returns(eInv);
										sgeBow.filter.Returns(new SGBowFilter());
									ISlotGroup sgeWear = MakeSubSG();
										ISlottable sbeWear = MakeSubSB();
											WearInstance wear = MakeWearInstance(0);
											sbeWear.itemInst.Returns(wear);
										sgeWear.filter.Returns(new SGWearFilter());
										sgeWear[0].Returns(sbeWear);
									ISlotGroup sgeCGears = MakeSubSG();
										IEnumerable<ISlotSystemElement> sgeCGearsEles;
											ISlottable sbeShield = MakeSubSB();
												ShieldInstance shield = MakeShieldInstance(0);
												sbeShield.itemInst.Returns(shield);
											ISlottable sbeMWeapon = MakeSubSB();
												MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
												sbeMWeapon.itemInst.Returns(mWeapon);
											sgeCGearsEles = new ISlotSystemElement[]{sbeShield, sbeMWeapon};
										sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
										sgeCGears.filter.Returns(new SGCGearsFilter());
									eSetEles = new ISlotSystemElement[]{sgeBow, sgeWear, sgeCGears};
								eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
							eBun.focusedElement.Returns(eSet);
					IEnumerable<SlottableItem> eInvEles;
						BowInstance bowR = MakeBowInstance(0);
						WearInstance wearR = MakeWearInstance(0);
						QuiverInstance quiverR = MakeQuiverInstance(0);
						PackInstance packR = MakePackInstance(0);
						eInvEles = new SlottableItem[]{
							bowR, wearR, quiverR, packR
						};
						eInv.GetEnumerator().Returns(eInvEles.GetEnumerator());
					IEnumerable<SlottableItem> pInvEles = new SlottableItem[]{
						bow, wear, shield, mWeapon,
						bowR, wearR, quiverR, packR
						};
						pInv.GetEnumerator().Returns(pInvEles.GetEnumerator());
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					ssm.UpdateEquipStatesOnAll();

					bool flag = true;
					flag &= bow.isEquipped == true;
					flag &= wear.isEquipped == true;
					flag &= shield.isEquipped == true;
					flag &= mWeapon.isEquipped == true;
					flag &= bowR.isEquipped == false;
					flag &= wearR.isEquipped == false;
					flag &= quiverR.isEquipped == false;
					flag &= packR.isEquipped == false;

					Assert.That(flag, Is.True);
					}
				[Test]
				public void ChangeEquippableCGearsCount_TargetSGIsExpandable_ThrowsException(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup sg = MakeSubSG();
					sg.isExpandable.Returns(true);

					Exception ex = Assert.Catch<InvalidOperationException>(()=> ssm.ChangeEquippableCGearsCount(0, sg));
					
					Assert.That(ex.Message, Is.StringContaining("ISlotGroupManager.ChangeEquippableCGearsCount: the targetSG is expandable"));
					}
				[TestCase(true)]
				[TestCase(false)]
				public void ChangeEquippableCGearsCount_TargetSGIsNOTExpandableAndSGIsFocusedOrDefocused_CallsEInvAndSGInOrder(bool focused){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
							ISlotGroup sgp = MakeSubSG();
							IPoolInventory pInv = MakeSubPoolInv();
								sgp.inventory.Returns(pInv);
							pBun.focusedElement.Returns(sgp);
						ISlotSystemBundle eBun = MakeSubBundle();
							IEquipmentSet eSet = Substitute.For<IEquipmentSet>();
								IEnumerable<ISlotSystemElement> eSetEles;
									ISlotGroup sgeBow = MakeSubSG();
										IEquipmentSetInventory eInv = MakeSubEquipInv();
										sgeBow.inventory.Returns(eInv);
										sgeBow.filter.Returns(new SGBowFilter());
											ISlottable sbeBow = MakeSubSB();
												BowInstance bowE = MakeBowInstance(0);
												sbeBow.itemInst.Returns(bowE);
										sgeBow[0].Returns(sbeBow);
									ISlotGroup sgeWear = MakeSubSG();
										sgeWear.filter.Returns(new SGWearFilter());
											ISlottable sbeWear = MakeSubSB();
												WearInstance wearE = MakeWearInstance(0);
												sbeWear.itemInst.Returns(wearE);
										sgeWear[0].Returns(sbeWear);
									ISlotGroup sgeCGears = MakeSubSG();
										IEnumerable<ISlotSystemElement> sgeCGearsEles = new ISlotSystemElement[]{};
										sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
										sgeCGears.filter.Returns(new SGCGearsFilter());
									eSetEles = new ISlotSystemElement[]{sgeBow, sgeWear, sgeCGears};
								eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
							eBun.focusedElement.Returns(eSet);
						IEnumerable<ISlotSystemBundle> gBuns = new ISlotSystemBundle[]{};
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();
					ISlotGroup sg = MakeSubSG();
						sg.isExpandable.Returns(false);
						sg.isFocused.Returns(focused);
						sg.isDefocused.Returns(!focused);

					ssm.ChangeEquippableCGearsCount(0, sg);

					Received.InOrder(() => {
						eInv.SetEquippableCGearsCount(0);
						sg.InitializeItems();
					});
					}
				[TestCaseSource(typeof(MarkEquippedInPoolCases))]
				public void MarkEquippedInPool_WhenCalled_FindItemInPoolAndSetsIsEquippedAccordingly(
					IEnumerable<SlottableItem> items, 
					InventoryItemInstance item, 
					bool equipped, 
					List<SlottableItem> xEquipped)
				{
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
							IEnumerable<ISlotSystemElement> pBunEles;
								ISlotGroup sgp = MakeSubSG();
									IPoolInventory pInv = MakeSubPoolInv();
										pInv.GetEnumerator().Returns(items.GetEnumerator());
									sgp.inventory.Returns(pInv);
									pBunEles = new ISlotSystemElement[]{sgp};
								pBun.GetEnumerator().Returns(pBunEles.GetEnumerator());
							pBun.focusedElement.Returns(sgp);
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns = new ISlotSystemBundle[]{};
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					ssm.MarkEquippedInPool(item, equipped);

					foreach(InventoryItemInstance it in ssm.poolInv)
						if(xEquipped.Contains(it))
							Assert.That(it.isEquipped, Is.True);
						else
						 	Assert.That(it.isEquipped, Is.False);
					}
					class MarkEquippedInPoolCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case0;
								BowInstance bow_0 = MakeBowInstance(0);
								WearInstance wear_0 = MakeWearInstance(0);
								ShieldInstance shield_0 = MakeShieldInstance(0);
								MeleeWeaponInstance mWeapon_0 = MakeMeleeWeaponInstance(0);
								QuiverInstance quiver_0 = MakeQuiverInstance(0);
								PackInstance pack_0 = MakePackInstance(0);
								PartsInstance parts_0 = MakePartsInstance(0, 2);
								IEnumerable<SlottableItem> items_0 = new SlottableItem[]{
									bow_0, 
									wear_0, 
									shield_0, 
									mWeapon_0, 
									quiver_0, 
									pack_0, 
									parts_0
								};
								IEnumerable<SlottableItem> equipped_0 = new SlottableItem[]{
									wear_0, 
									shield_0, 
									mWeapon_0, 
									quiver_0, 
									pack_0, 
									parts_0
								};
									foreach(var sItem in equipped_0)
										((InventoryItemInstance)sItem).isEquipped = true;
								List<SlottableItem> xEquipped_0 = new List<SlottableItem>(new SlottableItem[]{
									bow_0, 
									wear_0, 
									shield_0, 
									mWeapon_0, 
									quiver_0, 
									pack_0, 
									parts_0
								});
								case0 = new object[]{items_0, bow_0, true, xEquipped_0};
								yield return case0;
							object[] case1;
								BowInstance bow_1 = MakeBowInstance(0);
								WearInstance wear_1 = MakeWearInstance(0);
								ShieldInstance shield_1 = MakeShieldInstance(0);
								MeleeWeaponInstance mWeapon_1 = MakeMeleeWeaponInstance(0);
								QuiverInstance quiver_1 = MakeQuiverInstance(0);
								PackInstance pack_1 = MakePackInstance(0);
								PartsInstance parts_1 = MakePartsInstance(0, 2);
								IEnumerable<SlottableItem> items_1 = new SlottableItem[]{
									bow_1, 
									wear_1, 
									shield_1, 
									mWeapon_1, 
									quiver_1, 
									pack_1, 
									parts_1
								};
								IEnumerable<SlottableItem> equipped_1 = new SlottableItem[]{
									bow_1
								};
									foreach(var sItem in equipped_1)
										((InventoryItemInstance)sItem).isEquipped = true;
								List<SlottableItem> xEquipped_1 = new List<SlottableItem>(new SlottableItem[]{
								});
								case1 = new object[]{items_1, bow_1, false, xEquipped_1};
								yield return case1;
						}
					}
				[TestCase(true)]
				[TestCase(false)]
				public void SetEquippedOnAll_WhenCalled_CallsAllBundlesPIHAppropriateMethods(bool equipped){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();
					BowInstance bow = MakeBowInstance(0);

					ssm.SetEquippedOnAllSBs(bow, equipped);

					if(equipped){
						pBun.Received().PerformInHierarchy(ssm.Equip, bow);
						eBun.Received().PerformInHierarchy(ssm.Equip, bow);
						foreach(var gBun in gBuns)
							gBun.Received().PerformInHierarchy(ssm.Equip, bow);
					}else{
						pBun.Received().PerformInHierarchy(ssm.Unequip, bow);
						eBun.Received().PerformInHierarchy(ssm.Unequip, bow);
						foreach(var gBun in gBuns)
							gBun.Received().PerformInHierarchy(ssm.Unequip, bow);
					}
					}
				[Test]
				public void Equip_MatchesAndSGFocusedInBundleAndSBNewSlotIDNotMinus1_CallsSBEquip(){
					SlotSystemManager ssm = MakeSSM();
					BowInstance bow = MakeBowInstance(0);
					ISlotGroup stubSG = MakeSubSG();
						stubSG.isFocusedInHierarchy.Returns(true);
					ISlottable mockSB = MakeSubSB();
						mockSB.sg.Returns(stubSG);
						mockSB.itemInst.Returns(bow);
						mockSB.newSlotID.Returns(0);
					
					ssm.Equip(mockSB, bow);

					mockSB.Received().Equip();
					}
				[Test]
				public void Equip_MatchesAndSGNOTFocusedInBundleAndSGIsPool_CallsSBInOrder(){
					SlotSystemManager ssm = MakeSSM();
					BowInstance bow = MakeBowInstance(0);
					ISlotGroup stubSG = MakeSubSG();
						stubSG.isFocusedInHierarchy.Returns(false);
						stubSG.isPool.Returns(true);
					ISlottable mockSB = MakeSubSB();
						mockSB.sg.Returns(stubSG);
						mockSB.itemInst.Returns(bow);
					
					ssm.Equip(mockSB, bow);

					Received.InOrder(() => {
						mockSB.ClearCurEqpState();
						mockSB.Equip();
					});
					}
				[Test]
				public void Unequip_MatchesAndSGFocusedInBundleAndSBSlotIDNotMinus1_CallsSBUnequip(){
					SlotSystemManager ssm = MakeSSM();
					BowInstance bow = MakeBowInstance(0);
					ISlotGroup stubSG = MakeSubSG();
						stubSG.isFocusedInHierarchy.Returns(true);
					ISlottable mockSB = MakeSubSB();
						mockSB.sg.Returns(stubSG);
						mockSB.itemInst.Returns(bow);
						mockSB.slotID.Returns(0);
					
					ssm.Unequip(mockSB, bow);

					mockSB.Received().Unequip();
					}
				[Test]
				public void Unequip_MatchesAndSGNOTFocusedInBundleAndSGIsPool_CallsSBInOrder(){
					SlotSystemManager ssm = MakeSSM();
					BowInstance bow = MakeBowInstance(0);
					ISlotGroup stubSG = MakeSubSG();
						stubSG.isFocusedInHierarchy.Returns(false);
						stubSG.isPool.Returns(true);
					ISlottable mockSB = MakeSubSB();
						mockSB.sg.Returns(stubSG);
						mockSB.itemInst.Returns(bow);
					
					ssm.Unequip(mockSB, bow);

					Received.InOrder(() => {
						mockSB.ClearCurEqpState();
						mockSB.Unequip();
					});
					}
				[TestCaseSource(typeof(transactionCoroutineCases))]
				public void transactionCoroutine_WhenAllDone_CallsActProcExpire(DraggedIcon di1, DraggedIcon di2, ISlotGroup sg1, ISlotGroup sg2, bool called){
					SlotSystemManager ssm = MakeSSM();
					ssm.SetDIcon1(di1);
					ssm.SetDIcon2(di2);
					ssm.SetSG1(sg1);
					ssm.SetSG2(sg2);
					ISSMActProcess mockProc = MakeSubSSMActProc();
					ssm.SetAndRunActProcess(mockProc);

					ssm.transactionCoroutine();

					if(called)
						mockProc.Received().Expire();
					else
						mockProc.DidNotReceive().Expire();
					}
					class transactionCoroutineCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] allDone_T;
								allDone_T = new object[]{null, null, null, null, true};
								yield return allDone_T;
							object[] di1NotDone_F;
								ISlottable stubSB_1 = MakeSubSB();
								DraggedIcon di1 = new DraggedIcon(stubSB_1);
								di1NotDone_F = new object[]{di1, null, null, null, false};
								yield return di1NotDone_F;
							object[] di2NotDone_F;
								ISlottable stubSB_2 = MakeSubSB();
								DraggedIcon di2 = new DraggedIcon(stubSB_2);
								di2NotDone_F = new object[]{null, di2, null, null, false};
								yield return di2NotDone_F;
							object[] sg1NotDone_F;
								ISlotGroup sg1 = MakeSubSG();
								sg1NotDone_F = new object[]{null, null, sg1, null, false};
								yield return sg1NotDone_F;
							object[] sg2NotDone_F;
								ISlotGroup sg2 = MakeSubSG();
								sg2NotDone_F = new object[]{null, null, null, sg2, false};
								yield return sg2NotDone_F;
						}
					}
				[Test]
				public void FindParent_WhenCalled_CallAllBundlesPIHCheckAndReportParent(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetHierarchy();

					ISlotSystemElement ele = MakeSubSSE();
					ssm.FindParent(ele);

					pBun.Received().PerformInHierarchy(ssm.CheckAndReportParent, ele);
					eBun.Received().PerformInHierarchy(ssm.CheckAndReportParent, ele);
					foreach(var gBun in gBuns)
						gBun.Received().PerformInHierarchy(ssm.CheckAndReportParent, ele);
					}
				[TestCaseSource(typeof(CheckAndReportParentCases))]
				public void CheckAndReportParent_MatchesAndParentNonSlottable_SetsSSMFoundParentWithIt(ISlotSystemElement parent, ISlotSystemElement child, bool valid){
					SlotSystemManager ssm = MakeSSM();

					ssm.CheckAndReportParent(parent, child);

					if(valid)
						Assert.That(ssm.foundParent, Is.SameAs(parent));
					else
						Assert.That(ssm.foundParent, Is.Null);
					}
					class CheckAndReportParentCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] match_T;
								ISlotSystemBundle bundle_0 = MakeSubBundle();
									IEnumerable<ISlotSystemElement> bundleEles_0;
										ISlotSystemElement sse_0 = MakeSubSSE();
										bundleEles_0 = new ISlotSystemElement[]{sse_0};
									bundle_0.GetEnumerator().Returns(bundleEles_0.GetEnumerator());
								match_T = new object[]{bundle_0, sse_0, true};
								yield return match_T;
							object[] nonDirect_F;
								ISlotSystemBundle bundle_1 = MakeSubBundle();
									IEnumerable<ISlotSystemElement> bundleEles_1;
										ISlotSystemElement sse_1 = MakeSubSSE();
											IEnumerable<ISlotSystemElement> sseEles_1;
												ISlotGroup sg_1 = MakeSubSG();
												sseEles_1 = new ISlotSystemElement[]{sg_1};
											sse_1.GetEnumerator().Returns(sseEles_1.GetEnumerator());
										bundleEles_1 = new ISlotSystemElement[]{sse_1};
									bundle_1.GetEnumerator().Returns(bundleEles_1.GetEnumerator());
								nonDirect_F = new object[]{bundle_1, sg_1, false};
								yield return nonDirect_F;
							object[] inverse_F;
								ISlotSystemBundle bundle_2 = MakeSubBundle();
									IEnumerable<ISlotSystemElement> bundleEles_2;
										ISlotSystemElement sse_2 = MakeSubSSE();
										bundleEles_2 = new ISlotSystemElement[]{sse_2};
									bundle_2.GetEnumerator().Returns(bundleEles_2.GetEnumerator());
								inverse_F = new object[]{sse_2, bundle_2, false};
								yield return inverse_F;
							object[] directChildSG_T;
								ISlotSystemBundle bundle_3 = MakeSubBundle();
									IEnumerable<ISlotSystemElement> bundleEles_3;
										ISlotGroup sg_3 = MakeSubSG();
										bundleEles_3 = new ISlotSystemElement[]{sg_3};
									bundle_3.GetEnumerator().Returns(bundleEles_3.GetEnumerator());
								directChildSG_T = new object[]{bundle_3, sg_3, true};
								yield return directChildSG_T;
							object[] directChildSB_T;
								ISlotGroup sg_4 = MakeSubSG();
									IEnumerable<ISlotSystemElement> sgEles_4;
										ISlottable sb_4 = MakeSubSB();
										sgEles_4 = new ISlotSystemElement[]{sb_4};
									sg_4.GetEnumerator().Returns(sgEles_4.GetEnumerator());
								directChildSB_T = new object[]{sg_4, sb_4, true};
								yield return directChildSB_T;
						}
					}
				[TestCaseSource(typeof(SetParentCases))]
				public void SetParent_ParentNotSBNorSG_SetsAllElementsParAsSelf(ISlotSystemElement parent, IEnumerable<ISlotSystemElement> elements, bool valid){
					SlotSystemManager ssm = MakeSSM();
					
					ssm.SetParent(parent);

					foreach(var ele in elements)
						if(valid)
							ele.Received().SetParent(parent);
						else
							ele.DidNotReceive().SetParent(parent);
					}
					class SetParentCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] bundle_T;
								ISlotSystemBundle bundle_0 = MakeSubBundle();
									IEnumerable<ISlotSystemElement> eles_0;
										ISlotSystemElement sse0_0 = Substitute.For<ISlotSystemElement>();
										ISlotSystemElement sse1_0 = Substitute.For<ISlotSystemElement>();
										ISlotSystemElement sse2_0 = Substitute.For<ISlotSystemElement>();
										ISlotSystemElement sse3_0 = Substitute.For<ISlotSystemElement>();
										eles_0 = new ISlotSystemElement[]{sse0_0, sse1_0, sse2_0, sse3_0};
									bundle_0.GetEnumerator().Returns(eles_0.GetEnumerator());
								bundle_T = new object[]{bundle_0, eles_0, true};
								yield return bundle_T;
							object[] sg_F;
								ISlotGroup sg_2 = Substitute.For<ISlotGroup>();
									IEnumerable<ISlotSystemElement> eles_2;
										ISlotSystemElement sse0_2 = Substitute.For<ISlotSystemElement>();
										ISlotSystemElement sse1_2 = Substitute.For<ISlotSystemElement>();
										ISlotSystemElement sse2_2 = Substitute.For<ISlotSystemElement>();
										ISlotSystemElement sse3_2 = Substitute.For<ISlotSystemElement>();
										eles_2 = new ISlotSystemElement[]{sse0_2, sse1_2, sse2_2, sse3_2};
									sg_2.GetEnumerator().Returns(eles_2.GetEnumerator());
								sg_F = new object[]{sg_2, eles_2, false};
								yield return sg_F;
							object[] sb_F;
								ISlottable sb_3 = Substitute.For<ISlottable>();
									IEnumerable<ISlotSystemElement> eles_3;
										ISlotSystemElement sse0_3 = Substitute.For<ISlotSystemElement>();
										ISlotSystemElement sse1_3 = Substitute.For<ISlotSystemElement>();
										ISlotSystemElement sse2_3 = Substitute.For<ISlotSystemElement>();
										ISlotSystemElement sse3_3 = Substitute.For<ISlotSystemElement>();
										eles_3 = new ISlotSystemElement[]{sse0_3, sse1_3, sse2_3, sse3_3};
									sb_3.GetEnumerator().Returns(eles_3.GetEnumerator());
								sb_F = new object[]{sb_3, eles_3, false};
								yield return sb_F;
						}
					}
				[Test]
				public void Focus_WhenCalled_SetsSelStateFocusedState(){
					SlotSystemManager ssm = MakeSSM();

					ssm.Focus();

					Assert.That(ssm.isFocused, Is.True);
					}
				[Test]
				public void Defocus_WhenCalled_SetsSelStateDefocused(){
					SlotSystemManager ssm = MakeSSM();

					ssm.Defocus();

					Assert.That(ssm.isDefocused, Is.True);
					}
				[Test]
				public void Deactivate_WhenCalled_SetsSelStateDeactivateed(){
					SlotSystemManager ssm = MakeSSM();

					ssm.Deactivate();

					Assert.That(ssm.isDeactivated, Is.True);
					}
				// [Test][Category("Methods")]
				/* Get bakc to this after refactoring isToggleON */
				public void FocusInBundle_Various_CallsElementsAccordingly(){
					// SlotSystemManager ssm = MakeSSM();
					// 	SlotSystemBundle pBun = MakeSSBundle();
					// 		SlotGroup sgpAll = MakeSG();
					// 			PoolInventory pInv = new PoolInventory();
					// 				BowInstance bow = MakeBowInstance(0);

					}
				[TestCaseSource(typeof(PrePickFilterVariousSGTAComboCases))]
				public void PrePickFilter_VariousSGTACombo_OutsAccordingly( 
					ISlottable pickedSB,
					List<ISlotGroup> focusedSGs,
					ITransactionFactory taFac,
					bool expected)
				{
					SlotSystemManager ssm = MakeSSM();
						IFocusedSGsFactory focFac = Substitute.For<IFocusedSGsFactory>();
							focFac.focusedSGs.Returns(focusedSGs);
							ssm.SetPickedSB(pickedSB);
							ssm.SetFocusedSGsFactory(focFac);
							ssm.SetTAFactory(taFac);
					bool result;

					ssm.PrePickFilter(pickedSB, out result);

					Assert.That(result, Is.EqualTo(expected));
					}
					class PrePickFilterVariousSGTAComboCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] nothingButRev_F;
								ISlottable pickedSB_0 = MakeSubSB();
								List<ISlotGroup> focusedSGs_0;
									ISlotGroup sg0_0 = MakeSubSG();
										IEnumerable<ISlotSystemElement> sg0Eles_0;
											ISlottable sb00_0 = MakeSubSB();
											ISlottable sb01_0 = MakeSubSB();
											ISlottable sb02_0 = MakeSubSB();
											sg0Eles_0 = new ISlotSystemElement[]{sb00_0, sb01_0, sb02_0};
										sg0_0.GetEnumerator().Returns(sg0Eles_0.GetEnumerator());
									ISlotGroup sg1_0 = MakeSubSG();
										IEnumerable<ISlotSystemElement> sg1Eles_0;
											ISlottable sb10_0 = MakeSubSB();
											ISlottable sb11_0 = MakeSubSB();
											ISlottable sb12_0 = MakeSubSB();
											sg1Eles_0 = new ISlotSystemElement[]{sb10_0, sb11_0, sb12_0};
										sg1_0.GetEnumerator().Returns(sg1Eles_0.GetEnumerator());
									focusedSGs_0 = new List<ISlotGroup>(new ISlotGroup[]{sg0_0, sg1_0});
								ITransactionFactory taFac_0 = Substitute.For<ITransactionFactory>();
									taFac_0.MakeTransaction(pickedSB_0, sg0_0).Returns(Substitute.For<IRevertTransaction>());
										taFac_0.MakeTransaction(pickedSB_0, sb00_0).Returns(Substitute.For<IRevertTransaction>());
										taFac_0.MakeTransaction(pickedSB_0, sb01_0).Returns(Substitute.For<IRevertTransaction>());
										taFac_0.MakeTransaction(pickedSB_0, sb02_0).Returns(Substitute.For<IRevertTransaction>());
									taFac_0.MakeTransaction(pickedSB_0, sg1_0).Returns(Substitute.For<IRevertTransaction>());
										taFac_0.MakeTransaction(pickedSB_0, sb10_0).Returns(Substitute.For<IRevertTransaction>());
										taFac_0.MakeTransaction(pickedSB_0, sb11_0).Returns(Substitute.For<IRevertTransaction>());
										taFac_0.MakeTransaction(pickedSB_0, sb12_0).Returns(Substitute.For<IRevertTransaction>());
								nothingButRev_F = new object[]{pickedSB_0, focusedSGs_0, taFac_0, false};
								yield return nothingButRev_F;
							object[] atLeastOneNonRev_T;
								ISlottable pickedSB_1 = MakeSubSB();
								List<ISlotGroup> focusedSGs_1;
									ISlotGroup sg0_1 = MakeSubSG();
										IEnumerable<ISlotSystemElement> sg0Eles_1;
											ISlottable sb00_1 = MakeSubSB();
											ISlottable sb01_1 = MakeSubSB();
											ISlottable sb02_1 = MakeSubSB();
											sg0Eles_1 = new ISlotSystemElement[]{sb00_1, sb01_1, sb02_1};
										sg0_1.GetEnumerator().Returns(sg0Eles_1.GetEnumerator());
									ISlotGroup sg1_1 = MakeSubSG();
										IEnumerable<ISlotSystemElement> sg1Eles_1;
											ISlottable sb10_1 = MakeSubSB();
											ISlottable sb11_1 = MakeSubSB();
											ISlottable sb12_1 = MakeSubSB();
											sg1Eles_1 = new ISlotSystemElement[]{sb10_1, sb11_1, sb12_1};
										sg1_1.GetEnumerator().Returns(sg1Eles_1.GetEnumerator());
									focusedSGs_1 = new List<ISlotGroup>(new ISlotGroup[]{sg0_1, sg1_1});
								ITransactionFactory taFac_1 = Substitute.For<ITransactionFactory>();
									taFac_1.MakeTransaction(pickedSB_1, sg0_1).Returns(Substitute.For<IRevertTransaction>());
										taFac_1.MakeTransaction(pickedSB_1, sb00_1).Returns(Substitute.For<IRevertTransaction>());
										taFac_1.MakeTransaction(pickedSB_1, sb01_1).Returns(Substitute.For<IRevertTransaction>());
										taFac_1.MakeTransaction(pickedSB_1, sb02_1).Returns(Substitute.For<IRevertTransaction>());
									taFac_1.MakeTransaction(pickedSB_1, sg1_1).Returns(Substitute.For<IRevertTransaction>());
										taFac_1.MakeTransaction(pickedSB_1, sb10_1).Returns(Substitute.For<IRevertTransaction>());
										taFac_1.MakeTransaction(pickedSB_1, sb11_1).Returns(Substitute.For<IRevertTransaction>());
										taFac_1.MakeTransaction(pickedSB_1, sb12_1).Returns(Substitute.For<IReorderTransaction>());
								atLeastOneNonRev_T = new object[]{pickedSB_1, focusedSGs_1, taFac_1, true};
								yield return atLeastOneNonRev_T;
							
						}
					}
				[Test]
				public void SortSG_WhenCalled_CallsSortFAMakeSortTA(){
					SlotSystemManager ssm = MakeSSM();
					ISortTransactionFactory sortFA = Substitute.For<ISortTransactionFactory>();
						ISortTransaction stubSortTA = Substitute.For<ISortTransaction>();
						sortFA.MakeSortTA(Arg.Any<ISlotGroup>(), Arg.Any<SGSorter>()).Returns(stubSortTA);
						ssm.SetSortFA(sortFA);
					ISlotGroup sg = MakeSubSG();
					SGSorter sorter = new SGItemIDSorter();

					ssm.SortSG(sg, sorter);
					
					sortFA.Received().MakeSortTA(sg, sorter);
					}
				[Test]
				public void SortSG_WhenCalled_CallsTAExecute(){
					SlotSystemManager ssm = MakeSSM();
					ISortTransactionFactory sortFA = Substitute.For<ISortTransactionFactory>();
						ISortTransaction mockTA = Substitute.For<ISortTransaction>();
						sortFA.MakeSortTA(Arg.Any<ISlotGroup>(), Arg.Any<SGSorter>()).Returns(mockTA);
						ssm.SetSortFA(sortFA);
					ISlotGroup sg = MakeSubSG();
					SGSorter sorter = new SGItemIDSorter();

					ssm.SortSG(sg, sorter);
					
					mockTA.Received().Execute();
					}
				[Test]
				public void SortSG_WhenCalled_UpdateFields(){
					SlotSystemManager ssm = MakeSSM();
					ISortTransactionFactory sortFA = Substitute.For<ISortTransactionFactory>();
						ISortTransaction stubTA = Substitute.For<ISortTransaction>();
							ISlottable tarSB = MakeSubSB();
							ISlotGroup sg1 = MakeSubSG();
							stubTA.targetSB.Returns(tarSB);
							stubTA.sg1.Returns(sg1);
						sortFA.MakeSortTA(Arg.Any<ISlotGroup>(), Arg.Any<SGSorter>()).Returns(stubTA);
						ssm.SetSortFA(sortFA);
					ISlotGroup sg = MakeSubSG();
					SGSorter sorter = new SGItemIDSorter();
					SortSGTestCase expected = new SortSGTestCase(tarSB, sg1, stubTA);

					ssm.SortSG(sg, sorter);
					
					SortSGTestCase actual = new SortSGTestCase(ssm.targetSB, ssm.sg1, ssm.transaction);
					bool equality = actual.Equals(expected);

					Assert.That(equality,Is.True);
					}
					class SortSGTestCase: IEquatable<SortSGTestCase>{
						public ISlottable targetSB;
						public ISlotGroup sg1;
						public ISlotSystemTransaction ta;
						public SortSGTestCase(ISlottable sb, ISlotGroup sg, ISlotSystemTransaction ta){
							targetSB = sb; sg1 = sg; this.ta = ta;
						}
						public bool Equals(SortSGTestCase other){
							bool flag = true;
							flag &= object.ReferenceEquals(this.targetSB, other.targetSB);
							flag &= object.ReferenceEquals(this.sg1, other.sg1);
							flag &= object.ReferenceEquals(this.ta, other.ta);
							return flag;
						}
					}
			/*	Transaction */
				[Test]
				public void SetTransaction_PrevTransactionNull_SetsTransaction(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction stubTA = MakeSubTA();

					ssm.SetTransaction(stubTA);

					Assert.That(ssm.transaction, Is.SameAs(stubTA));
					}
				[Test]
				public void SetTransaction_PrevTransactionNull_CallsTAIndicate(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction mockTA = MakeSubTA();

					ssm.SetTransaction(mockTA);

					mockTA.Received().Indicate();
					}
				[Test]
				public void SetTransaction_DiffTA_SetsTransaction(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction prevTA = MakeSubTA();
					ISlotSystemTransaction stubTA = MakeSubTA();
					ssm.SetTransaction(prevTA);
					ssm.SetTransaction(stubTA);

					Assert.That(ssm.transaction, Is.SameAs(stubTA));
					}
				[Test]
				public void SetTransaction_DiffTA_CallsTAIndicate(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction prevTA = MakeSubTA();
					ISlotSystemTransaction mockTA = MakeSubTA();

					ssm.SetTransaction(prevTA);
					ssm.SetTransaction(mockTA);

					mockTA.Received().Indicate();
					}
				[Test]
				public void SetTransaction_SameTA_DoesNotCallIndicateTwice(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction mockTA = MakeSubTA();

					ssm.SetTransaction(mockTA);
					ssm.SetTransaction(mockTA);

					mockTA.Received(1).Indicate();
					}
				[Test]
				public void SetTransaction_Null_SetsTANull(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction stubTA = MakeSubTA();

					ssm.SetTransaction(stubTA);
					ssm.SetTransaction(null);

					Assert.That(ssm.transaction, Is.Null);
					}
				[Test]
				public void AcceptsSGTAComp_ValidSG_SetsDone(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();
					ssm.SetSG1(stubSG);

					ssm.AcceptSGTAComp(stubSG);

					Assert.That(ssm.sg2Done, Is.True);
					}
				[Test]
				public void AcceptsDITAComp_ValidDI_SetsDone(){
					SlotSystemManager ssm = MakeSSM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());
					ssm.SetDIcon1(stubDI);

					ssm.AcceptDITAComp(stubDI);

					Assert.That(ssm.dIcon1Done, Is.True);
					}
				[Test]
				public void ExecuteTransaction_WhenCalled_SetsActStatTransaction(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction stubTA = MakeSubTA();
					ssm.SetTransaction(stubTA);
					
					ssm.ExecuteTransaction();

					Assert.That(ssm.isTransacting, Is.True);
					}
				[Test]
				public void SetTargetSB_FromNullToSome_CallsSBSelect(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();

					ssm.SetTargetSB(mockSB);


					mockSB.Received().Select();
					}
				[Test]
				public void SetTargetSB_FromNullToSome_SetsItTargetSB(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable stubSB = MakeSubSB();

					ssm.SetTargetSB(stubSB);

					Assert.That(ssm.targetSB, Is.SameAs(stubSB));
					}
				[Test]
				public void SetTargetSB_FromOtherToSome_CallsSBSelect(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable stubSB = MakeSubSB();
					ISlottable mockSB = MakeSubSB();
					ssm.SetTargetSB(stubSB);

					ssm.SetTargetSB(mockSB);
					
					mockSB.Received().Select();
					}
				[Test]
				public void SetTargetSB_FromOtherToSome_SetsItTargetSB(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable prevSB = MakeSubSB();
					ISlottable stubSB = MakeSubSB();
					ssm.SetTargetSB(prevSB);

					ssm.SetTargetSB(stubSB);
					
					Assert.That(ssm.targetSB, Is.SameAs(stubSB));
					}
				[Test]
				public void SetTargetSB_FromOtherToSome_CallOtherSBFocus(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ISlottable stubSB = MakeSubSB();
					ssm.SetTargetSB(mockSB);

					ssm.SetTargetSB(stubSB);
					
					mockSB.Received().Focus();
					}
				[Test]
				public void SetTargetSB_SomeToNull_CallSBFocus(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetTargetSB(mockSB);

					ssm.SetTargetSB(null);

					mockSB.Received().Focus();
					}
				[Test]
				public void SetTargetSB_SomeToNull_SetsNull(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetTargetSB(mockSB);

					ssm.SetTargetSB(null);

					Assert.That(ssm.targetSB, Is.Null);
					}
				[Test]
				public void SetTargetSB_SomeToSame_DoesNotCallSelectTwice(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetTargetSB(mockSB);

					ssm.SetTargetSB(mockSB);

					mockSB.Received(1).Select();
					}
				[Test]
				public void SetTargetSB_SomeToSame_DoesNotCallFocus(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetTargetSB(mockSB);

					ssm.SetTargetSB(mockSB);

					mockSB.DidNotReceive().Focus();
					}
				[Test]
				public void SetSG1_NullToSome_SetsSG1(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG1(stubSG);

					Assert.That(ssm.sg1, Is.SameAs(stubSG));
					}
				[Test]
				public void SetSG1_NullToSome_SetsSG1DoneFalse(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG1(stubSG);

					Assert.That(ssm.sg1Done, Is.False);
					}
				[Test]
				public void SetSG1_OtherToSome_SetsSG1(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup prevSG = MakeSubSG();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG1(prevSG);
					ssm.SetSG1(stubSG);

					Assert.That(ssm.sg1, Is.SameAs(stubSG));
					}
				[Test]
				public void SetSG1_OtherToSome_SetsSG1DoneFalse(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup prevSG = MakeSubSG();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG1(prevSG);
					ssm.SetSG1(stubSG);

					Assert.That(ssm.sg1Done, Is.False);
					}
				[Test]
				public void SetSG1_SomeToNull_SetsSG1Null(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();
					ssm.SetSG1(stubSG);

					ssm.SetSG1(null);

					Assert.That(ssm.sg1, Is.Null);
					}
				[Test]
				public void SetSG1_SomeToNull_SetsSG1DoneTrue(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();
					ssm.SetSG1(stubSG);

					ssm.SetSG1(null);

					Assert.That(ssm.sg1Done, Is.True);
					}
				[Test]
				public void SetSG2_NullToSome_SetsSG2(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG2(stubSG);

					Assert.That(ssm.sg2, Is.SameAs(stubSG));
					}
				[Test]
				public void SetSG2_NullToSome_SetsSG2DoneFalse(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG2(stubSG);

					Assert.That(ssm.sg2Done, Is.False);
					}
				[Test]
				public void SetSG2_NullToSome_CallSG2Select(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();

					ssm.SetSG2(mockSG);

					mockSG.Received().Select();
					}
				[Test]
				public void SetSG2_OtherToSome_SetsSG2(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup prevSG = MakeSubSG();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG2(prevSG);
					ssm.SetSG2(stubSG);

					Assert.That(ssm.sg2, Is.SameAs(stubSG));
					}
				[Test]
				public void SetSG2_OtherToSome_SetsSG2DoneFalse(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup prevSG = MakeSubSG();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG2(prevSG);
					ssm.SetSG2(stubSG);

					Assert.That(ssm.sg2Done, Is.False);
					}
				[Test]
				public void SetSG2_OtherToSome_CallsSG2Select(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup prevSG = MakeSubSG();
					ISlotGroup mockSG = MakeSubSG();

					ssm.SetSG2(prevSG);
					ssm.SetSG2(mockSG);

					mockSG.Received().Select();
					}
				[Test]
				public void SetSG2_SomeToNull_SetsSG2Null(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();
					ssm.SetSG2(stubSG);

					ssm.SetSG2(null);

					Assert.That(ssm.sg2, Is.Null);
					}
				[Test]
				public void SetSG2_SomeToNull_SetsSG2DoneTrue(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();
					ssm.SetSG2(stubSG);

					ssm.SetSG2(null);

					Assert.That(ssm.sg2Done, Is.True);
					}
				[Test]
				public void SetDIcon1_ToNonNull_SetsDIcon1DoneFalse(){
					SlotSystemManager ssm = MakeSSM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());

					ssm.SetDIcon1(stubDI);

					Assert.That(ssm.dIcon1Done, Is.False);
					}
				[Test]
				public void SetDIcon1_ToNull_SetsDIcon1DoneTrue(){
					SlotSystemManager ssm = MakeSSM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());

					ssm.SetDIcon1(stubDI);
					ssm.SetDIcon1(null);

					Assert.That(ssm.dIcon1Done, Is.True);
					}
				[Test]
				public void SetDIcon2_ToNonNull_SetsDIcon2DoneFalse(){
					SlotSystemManager ssm = MakeSSM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());

					ssm.SetDIcon2(stubDI);

					Assert.That(ssm.dIcon2Done, Is.False);
					}
				[Test]
				public void SetDIcon2_ToNull_SetsDIcon2DoneTrue(){
					SlotSystemManager ssm = MakeSSM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());

					ssm.SetDIcon2(stubDI);
					ssm.SetDIcon2(null);

					Assert.That(ssm.dIcon2Done, Is.True);
					}
				[Test]
				public void SetHovered_NullToSB_SetsHovered(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();

					ssm.SetHovered(mockSB);

					Assert.That(ssm.hovered, Is.SameAs(mockSB));
					}
				[Test]
				public void SetHovered_SBToNull_CallSBOnHoverExit(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetHovered(mockSB);

					ssm.SetHovered(null);

					mockSB.Received().OnHoverExitMock();
					}
				[Test]
				public void SetHovered_SBToNull_SetsNull(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetHovered(mockSB);

					ssm.SetHovered(null);

					Assert.That(ssm.hovered, Is.Null);
					}
				[Test]
				public void SetHovered_SBToSomeSSE_CallSBOnHoverExit(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetHovered(mockSB);

					ssm.SetHovered(MakeSubSSE());

					mockSB.Received().OnHoverExitMock();
					}
				[Test]
				public void SetHovered_SBToSame_DoesNotCallSBOnHoverExit(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetHovered(mockSB);

					ssm.SetHovered(mockSB);

					mockSB.DidNotReceive().OnHoverExitMock();
					}
				[Test]
				public void SetHovered_NullToSG_SetsHovered(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();

					ssm.SetHovered(mockSG);

					Assert.That(ssm.hovered, Is.SameAs(mockSG));
					}
				[Test]
				public void SetHovered_SGToNull_CallSGOnHoverExit(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();
					ssm.SetHovered(mockSG);

					ssm.SetHovered(null);

					mockSG.Received().OnHoverExitMock();
					}
				[Test]
				public void SetHovered_SGToNull_SetsNull(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();
					ssm.SetHovered(mockSG);

					ssm.SetHovered(null);

					Assert.That(ssm.hovered, Is.Null);
					}
				[Test]
				public void SetHovered_SGToSomeSSE_CallSGOnHoverExit(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();
					ssm.SetHovered(mockSG);

					ssm.SetHovered(MakeSubSSE());

					mockSG.Received().OnHoverExitMock();
					}
				[Test]
				public void SetHovered_SGToSame_DoesNotCallSGOnHoverExit(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();
					ssm.SetHovered(mockSG);

					ssm.SetHovered(mockSG);

					mockSG.DidNotReceive().OnHoverExitMock();
					}
				[Test]
				public void SetHovered_WhenCalled_UpdateTransactionFields(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemTransaction stubTA = MakeSubTA();
							ISlottable targetSB = MakeSubSB();
							ISlotGroup sg1 = MakeSubSG();
							ISlotGroup sg2 = MakeSubSG();
							stubTA.targetSB.Returns(targetSB);
							stubTA.sg1.Returns(sg1);
							stubTA.sg2.Returns(sg2);
						ISlottable hoveredSB = MakeSubSB();
						Dictionary<ISlotSystemElement, ISlotSystemTransaction> taResults = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
						taResults.Add(hoveredSB, stubTA);
						ssm.transactionResults = taResults;
					SetHoveredTestData expected = new SetHoveredTestData(targetSB, sg1, sg2, stubTA);

					ssm.SetHovered(hoveredSB);

					SetHoveredTestData actual = new SetHoveredTestData(ssm.targetSB, ssm.sg1, ssm.sg2, ssm.transaction);
					bool equality = actual.Equals(expected);
					Assert.That(equality, Is.True);
					}
					class SetHoveredTestData: IEquatable<SetHoveredTestData>{
						public ISlottable targetSB;
						public ISlotGroup sg1;
						public ISlotGroup sg2;
						public ISlotSystemTransaction ta;
						public SetHoveredTestData(ISlottable tSB, ISlotGroup sg1, ISlotGroup sg2, ISlotSystemTransaction ta){
							this.targetSB = tSB;
							this.sg1 = sg1;
							this.sg2 = sg2;
							this.ta = ta;
						}
						public bool Equals(SetHoveredTestData other){
							bool flag = true;
							flag &= object.ReferenceEquals(this.targetSB, other.targetSB);
							flag &= object.ReferenceEquals(this.sg1, other.sg1);
							flag &= object.ReferenceEquals(this.sg2, other.sg2);
							flag &= object.ReferenceEquals(this.ta, other.ta);
							return flag;
						}
					}
				[Test]
				public void CreateTransactionResults_WhenCalled_CreateAndStoreSGTAPairInDict(){
					SlotSystemManager ssm = MakeSSM();
						ISlottable pickedSB = MakeSubSB();
						ssm.SetPickedSB(pickedSB);
						ITransactionFactory stubFac = MakeSubTAFactory();
							ISlotGroup sgA = MakeSubSG();
								IEnumerable<ISlotSystemElement> sgAEles = new ISlotSystemElement[]{};
								sgA.GetEnumerator().Returns(sgAEles.GetEnumerator());
							ISlotGroup sgB = MakeSubSG();
								IEnumerable<ISlotSystemElement> sgBEles = new ISlotSystemElement[]{};
								sgB.GetEnumerator().Returns(sgBEles.GetEnumerator());
							ISlotGroup sgC = MakeSubSG();
								IEnumerable<ISlotSystemElement> sgCEles = new ISlotSystemElement[]{};
								sgC.GetEnumerator().Returns(sgCEles.GetEnumerator());
							IRevertTransaction revTA = Substitute.For<IRevertTransaction>();
							IFillTransaction fillTA = Substitute.For<IFillTransaction>();
							IReorderTransaction reoTA = Substitute.For<IReorderTransaction>();
							stubFac.MakeTransaction(pickedSB, sgA).Returns(revTA);
							stubFac.MakeTransaction(pickedSB, sgB).Returns(fillTA);
							stubFac.MakeTransaction(pickedSB, sgC).Returns(reoTA);
						IFocusedSGsFactory focFac = Substitute.For<IFocusedSGsFactory>();
							List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sgA, sgB, sgC});
							focFac.focusedSGs.Returns(sgsList);

					ssm.SetTAFactory(stubFac);
					ssm.SetFocusedSGsFactory(focFac);

					ssm.CreateTransactionResults();

					Dictionary<ISlotSystemElement, ISlotSystemTransaction> expected = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
					expected.Add(sgA, revTA);
					expected.Add(sgB, fillTA);
					expected.Add(sgC, reoTA);

					Assert.That(ssm.transactionResults.Count, Is.EqualTo(expected.Count));
					IEnumerator actRator = ssm.transactionResults.GetEnumerator();
					IEnumerator expRator = expected.GetEnumerator();
					while(actRator.MoveNext() && expRator.MoveNext()){
						KeyValuePair<ISlotSystemElement, ISlotSystemTransaction> actPair = (KeyValuePair<ISlotSystemElement, ISlotSystemTransaction>)actRator.Current;
						KeyValuePair<ISlotSystemElement, ISlotSystemTransaction> expPair = (KeyValuePair<ISlotSystemElement, ISlotSystemTransaction>)expRator.Current;
						Assert.That(actPair.Key, Is.SameAs(expPair.Key));
						Assert.That(actPair.Value, Is.TypeOf(expPair.Value.GetType()));
					}
					}
				[Test]
				public void CreateTransactionResults_WhenCalled_CreateAndStoreSBTAPairInDict(){
					SlotSystemManager ssm = MakeSSM();
						ISlottable pickedSB = MakeSubSB();
						ssm.SetPickedSB(pickedSB);
						ITransactionFactory stubFac = MakeSubTAFactory();
							ISlotGroup sg = MakeSubSG();
								IEnumerable<ISlotSystemElement> sgEles;
									ISlottable sbA = MakeSubSB();
									ISlottable sbB = MakeSubSB();
									ISlottable sbC = MakeSubSB();
									sgEles = new ISlotSystemElement[]{sbA, sbB, sbC};
								sg.GetEnumerator().Returns(sgEles.GetEnumerator());
							IRevertTransaction revTA = Substitute.For<IRevertTransaction>();
							IFillTransaction fillTA = Substitute.For<IFillTransaction>();
							IStackTransaction stackTA = Substitute.For<IStackTransaction>();
							stubFac.MakeTransaction(pickedSB, sg).Returns(revTA);
								stubFac.MakeTransaction(pickedSB, sbA).Returns(revTA);
								stubFac.MakeTransaction(pickedSB, sbB).Returns(fillTA);
								stubFac.MakeTransaction(pickedSB, sbC).Returns(stackTA);
						IFocusedSGsFactory focFac = Substitute.For<IFocusedSGsFactory>();
							List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sg});
							focFac.focusedSGs.Returns(sgsList);
					ssm.SetTAFactory(stubFac);
					ssm.SetFocusedSGsFactory(focFac);

					ssm.CreateTransactionResults();

					Dictionary<ISlotSystemElement, ISlotSystemTransaction> expected = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
					expected.Add(sg, revTA);
					expected.Add(sbA, revTA);
					expected.Add(sbB, fillTA);
					expected.Add(sbC, stackTA);

					Assert.That(ssm.transactionResults.Count, Is.EqualTo(expected.Count));
					IEnumerator actRator = ssm.transactionResults.GetEnumerator();
					IEnumerator expRator = expected.GetEnumerator();
					while(actRator.MoveNext() && expRator.MoveNext()){
						KeyValuePair<ISlotSystemElement, ISlotSystemTransaction> actPair = (KeyValuePair<ISlotSystemElement, ISlotSystemTransaction>)actRator.Current;
						KeyValuePair<ISlotSystemElement, ISlotSystemTransaction> expPair = (KeyValuePair<ISlotSystemElement, ISlotSystemTransaction>)expRator.Current;
						Assert.That(actPair.Key, Is.SameAs(expPair.Key));
						Assert.That(actPair.Value, Is.TypeOf(expPair.Value.GetType()));
					}
					}
				
				[TestCaseSource(typeof(CreateTransactionResultsVariousTACases))]
				public void CreateTransactionResults_VariousTA_CallsSGDefocusSelfOrFocusSelfAccordingly(ISlotSystemTransaction ta){
					SlotSystemManager ssm = MakeSSM();
						ISlottable pickedSB = MakeSubSB();
						ssm.SetPickedSB(pickedSB);
						ITransactionFactory stubFac = MakeSubTAFactory();
							ISlotGroup sgA = MakeSubSG();
								IEnumerable<ISlotSystemElement> sgAEles = new ISlotSystemElement[]{};
								sgA.GetEnumerator().Returns(sgAEles.GetEnumerator());
							stubFac.MakeTransaction(pickedSB, sgA).Returns(ta);
						IFocusedSGsFactory focFac = Substitute.For<IFocusedSGsFactory>();
							List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sgA});
							focFac.focusedSGs.Returns(sgsList);
					ssm.SetTAFactory(stubFac);
					ssm.SetFocusedSGsFactory(focFac);

					ssm.CreateTransactionResults();
					if(ta is IRevertTransaction)
						sgA.Received().DefocusSelf();
					else
						sgA.Received().FocusSelf();
					}
					class CreateTransactionResultsVariousTACases: IEnumerable{
						public IEnumerator GetEnumerator(){
							yield return Substitute.For<IRevertTransaction>();
							yield return Substitute.For<IReorderTransaction>();
							yield return Substitute.For<ISortTransaction>();
							yield return Substitute.For<ISwapTransaction>();
							yield return Substitute.For<IFillTransaction>();
							yield return Substitute.For<IStackTransaction>();
						}
					}
				
				[TestCaseSource(typeof(CreateTransactionResultsVariousTACases))]
				public void CreateTransactionResults_VariousTA_CallsSBFocusOrDefocusAccordingly(ISlotSystemTransaction ta){
					SlotSystemManager ssm = MakeSSM();
						ISlottable pickedSB = MakeSubSB();
						ssm.SetPickedSB(pickedSB);
						ITransactionFactory stubFac = MakeSubTAFactory();
							ISlotGroup sg = MakeSubSG();
								IEnumerable<ISlotSystemElement> sgEles;	
									ISlottable sb = MakeSubSB();
									sgEles = new ISlotSystemElement[]{sb};
								sg.GetEnumerator().Returns(sgEles.GetEnumerator());
							stubFac.MakeTransaction(pickedSB, sb).Returns(ta);
							stubFac.MakeTransaction(pickedSB, sg).Returns(Substitute.For<IRevertTransaction>());
						IFocusedSGsFactory focFac = Substitute.For<IFocusedSGsFactory>();
							List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sg});
							focFac.focusedSGs.Returns(sgsList);
					ssm.SetTAFactory(stubFac);
					ssm.SetFocusedSGsFactory(focFac);

					ssm.CreateTransactionResults();
					if(ta is IRevertTransaction)
						sb.Received().Defocus();
					else if(ta is IFillTransaction)
						sb.Received().Defocus();
					else
						sb.Received().Focus();
					}
				[Test]
				public void UpdateTransaction_WhenCalled_UpdatesFields(){
					SlotSystemManager ssm = MakeSSM();
						ISlotGroup hovered = MakeSubSG();
							ssm.SetHovered(hovered);
							ISlotSystemTransaction stubTA = MakeSubTA();
								ISlottable targetSB = MakeSubSB();
								ISlotGroup sg1 = MakeSubSG();
								ISlotGroup sg2 = MakeSubSG();
								stubTA.targetSB.Returns(targetSB);
								stubTA.sg1.Returns(sg1);
								stubTA.sg2.Returns(sg2);
						Dictionary<ISlotSystemElement, ISlotSystemTransaction> dict = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
							dict.Add(hovered, stubTA);
							ssm.transactionResults = dict;
					UpdateTransactionTestCase expected = new UpdateTransactionTestCase(targetSB, sg1, sg2, stubTA);
					
					ssm.UpdateTransaction();

					UpdateTransactionTestCase actual = new UpdateTransactionTestCase(ssm.targetSB, ssm.sg1, ssm.sg2, ssm.transaction);
					bool equality = actual.Equals(expected);

					Assert.That(equality, Is.True);
					}
					class UpdateTransactionTestCase: IEquatable<UpdateTransactionTestCase>{
						public ISlottable targetSB;
						public ISlotGroup sg1;
						public ISlotGroup sg2;
						public ISlotSystemTransaction ta;
						public UpdateTransactionTestCase(ISlottable tSB, ISlotGroup g1, ISlotGroup g2, ISlotSystemTransaction tra){
							targetSB = tSB; sg1 = g1; sg2 = g2; ta = tra;
						}
						public bool Equals(UpdateTransactionTestCase other){
							bool flag = true;
							flag &= object.ReferenceEquals(this.targetSB, other.targetSB);
							flag &= object.ReferenceEquals(this.sg1, other.sg1);
							flag &= object.ReferenceEquals(this.sg2, other.sg2);
							flag &= object.ReferenceEquals(this.ta, other.ta);
							return flag;
						}
					}
				[Test]
				public void ReferToTAAndUpdateSelState_TAResultsNull_CallsSGFocus(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();

					ssm.ReferToTAAndUpdateSelState(mockSG);

					mockSG.Received().Focus();
					}
				[TestCaseSource(typeof(ReferToTAAndUpdateSelState_VariousTAsCases))]
				public void ReferToTAAndUpdateSelState_VariousTAs_CallsSGSetSelStateAccordingly(ISlotSystemTransaction ta, bool focused){
					ISlotGroup mockSG = MakeSubSG();
					SlotSystemManager ssm = MakeSSM();
					Dictionary<ISlotSystemElement, ISlotSystemTransaction> dict = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
						dict.Add(mockSG, ta);
						ssm.transactionResults = dict;
					ssm.ReferToTAAndUpdateSelState(mockSG);
					if(focused)
						mockSG.Received().Focus();
					else
						mockSG.Received().Defocus();
					}
					class ReferToTAAndUpdateSelState_VariousTAsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] revert_def;
								revert_def = new object[]{Substitute.For<IRevertTransaction>(), false};
								yield return revert_def;
							object[] reorder_foc;
								reorder_foc = new object[]{Substitute.For<IReorderTransaction>(), true};
								yield return reorder_foc;
							object[] sort_foc;
								sort_foc = new object[]{Substitute.For<ISortTransaction>(), true};
								yield return sort_foc;
							object[] fill_foc;
								fill_foc = new object[]{Substitute.For<IFillTransaction>(), true};
								yield return fill_foc;
							object[] swap_foc;
								swap_foc = new object[]{Substitute.For<ISwapTransaction>(), true};
								yield return swap_foc;
							object[] stack_foc;
								stack_foc = new object[]{Substitute.For<IStackTransaction>(), true};
								yield return stack_foc;
						}
					}
			/*	helper	*/
				public IEnumerable<ISlotSystemElement> ConvertToSSEs<T>(IEnumerable<T> sgs) where T: ISlotSystemElement{
					foreach(var sg in sgs)
						yield return (ISlotSystemElement)sg;
				}
		}
	}
}
