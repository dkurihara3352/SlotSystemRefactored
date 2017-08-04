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
			public void InspectorSetUp_WhenCalled_SetsBundles(){
				SlotSystemManager ssm = MakeSSM();
					ISlotSystemBundle pBun = MakeSubBundle();
					ISlotSystemBundle eBun = MakeSubBundle();
					IEnumerable<ISlotSystemBundle> gBuns;
						ISlotSystemBundle gBunA = MakeSubBundle();
						ISlotSystemBundle gBunB = MakeSubBundle();
						ISlotSystemBundle gBunC = MakeSubBundle();
						gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};
					
				ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());

				Assert.That(ssm.poolBundle, Is.SameAs(pBun));
				Assert.That(ssm.equipBundle, Is.SameAs(eBun));
				bool equality = ssm.otherBundles.MemberEquals(gBuns);
				Assert.That(equality, Is.True);
				}
			[Test]
			public void InspectorSetUp_Always_SetsTAM(){
				ITransactionManager stubTAM = MakeSubTAM();
				SlotSystemManager ssm  = MakeSSM();

				ssm.InspectorSetUp(MakeSubBundle(), MakeSubBundle(), new ISlotSystemBundle[]{}, stubTAM);

				Assert.That(ssm.tam, Is.SameAs(stubTAM));
			}
			[Test]
			public void Initialize_WhenCalled_CallsSetSSMInHierarchy(){
				SlotSystemManager ssm = MakeSSMWithSelStateHandler();
					ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
				ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
				ssm.SetHierarchy();

				ssm.Initialize();
				
				Assert.That(ssm.ssm, Is.SameAs(ssm));
				pBun.Received().PerformInHierarchy(ssm.SetSSMInH);
				eBun.Received().PerformInHierarchy(ssm.SetSSMInH);
				foreach(var gBun in gBuns)
					gBun.Received().PerformInHierarchy(ssm.SetSSMInH);
				}
			[Test]
			public void Initialize_WhenCalled_CallsPIHInitializeState(){
				SlotSystemManager ssm = MakeSSMWithSelStateHandler();
					ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
				ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
				ssm.SetHierarchy();

				ssm.Initialize();

				Assert.That(ssm.isDeactivated, Is.True);
				Assert.That(ssm.wasSelStateNull, Is.True);
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
				SlotSystemBundle MakeSSBundleWithSelStateHandler(){
					SlotSystemBundle bundle = MakeSSBundle();
					SSEStateHandler handler = new SSEStateHandler();
					bundle.SetSelStateHandler(handler);
					return bundle;
				}
				SlotGroup MakeSGWithSelStateHandler(){
					SlotGroup sg = MakeSG();
					SSEStateHandler handler = new SSEStateHandler();
					sg.SetSelStateHandler(handler);
					return sg;
				}
				[Test]
				public void focusedSGGs_Always_ReturnsAllFocusedSGsInOtherBundles(){
					SlotSystemManager ssm = MakeSSMWithSelStateHandler();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							SlotSystemBundle gBun = MakeSSBundleWithSelStateHandler();//non sub
								SlotGroup sggA = MakeSGWithSelStateHandler();
									sggA.transform.SetParent(gBun.transform);
									sggA.SetElements(new ISlotSystemElement[]{});
									sggA.Focus();
								SlotGroup sggB = MakeSGWithSelStateHandler();
									sggB.SetElements(new ISlotSystemElement[]{});
									sggB.transform.SetParent(gBun.transform);
									sggB.Focus();
								SlotGroup sggC = MakeSGWithSelStateHandler();
									sggC.SetElements(new ISlotSystemElement[]{});
									sggC.transform.SetParent(gBun.transform);
									sggC.Focus();
								gBun.SetHierarchy();
								gBun.Focus();
							gBuns = new ISlotSystemBundle[]{gBun};
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
					List<ISlotGroup> expected = new List<ISlotGroup>(new ISlotGroup[]{sggA, sggB, sggC});

					List<ISlotGroup> actual = ssm.focusedSGGs;

					Assert.That(actual, Is.EqualTo(expected));
				}
				public void focusedSGs_Always_ReturnsSumOfAllFocusedSGs(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
							ISlotGroup sgp = MakeSubSG();
							pBun.focusedElement.Returns(sgp);
						ISlotSystemBundle eBun = MakeSubBundle();
							IEquipmentSet eSet = MakeSubESet();
							 	ISlotGroup sgeBow = MakeSubSG();
								ISlotGroup sgeWear = MakeSubSG();
								ISlotGroup sgeCGears = MakeSubSG();
								IEnumerable<ISlotSystemElement> eSetEles = new ISlotSystemElement[]{sgeBow, sgeWear, sgeCGears};
								eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
							eBun.focusedElement.Returns(eSet);
						IEnumerable<ISlotSystemBundle> gBuns;
							SlotSystemBundle gBun = MakeSSBundle();//non sub
								SlotGroup sggA = MakeSG();
									sggA.transform.SetParent(gBun.transform);
									sggA.SetElements(new ISlotSystemElement[]{});
									sggA.Focus();
								SlotGroup sggB = MakeSG();
									sggB.SetElements(new ISlotSystemElement[]{});
									sggB.transform.SetParent(gBun.transform);
									sggB.Focus();
								SlotGroup sggC = MakeSG();
									sggC.SetElements(new ISlotSystemElement[]{});
									sggC.transform.SetParent(gBun.transform);
									sggC.Focus();
								gBun.SetHierarchy();
								gBun.Focus();
							gBuns = new ISlotSystemBundle[]{gBun};
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
					List<ISlotGroup> expected = new List<ISlotGroup>(new ISlotGroup[]{
						sgp, sgeBow, sgeWear, sgeCGears, sggA, sggB, sggC
					});

					List<ISlotGroup> actual = ssm.focusedSGs;
					
					Assert.That(actual, Is.EqualTo(expected));
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
							IEnumerable<ISlotSystemElement> pBunEles = new ISlotSystemElement[]{
								sgpA, sgpB, sgpC
							};
							pBun.GetEnumerator().Returns(pBunEles.GetEnumerator());
							IPoolInventory pInv = MakeSubPoolInv();
								sgpA.inventory.Returns(pInv);
							pBun.focusedElement.Returns(sgpA);
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
												bowSBE.item.Returns(bowE);
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
												wearSBE.item.Returns(wearE);
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
													shieldSBE.item.Returns(shieldE);
												ISlottable mWeaponSBE = MakeSubSB();
													MeleeWeaponInstance mWeaponE = MakeMeleeWeaponInstance(0);
													mWeaponSBE.item.Returns(mWeaponE);
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
												bowSBE.item.Returns(bowE);
											sgeBow[0].Returns(bowSBE);
											sgeBow.filter.Returns(new SGBowFilter());
										ISlotGroup sgeWear = MakeSubSG();
											ISlottable wearSBE = MakeSubSB();
												WearInstance wearE = MakeWearInstance(0);
												wearSBE.item.Returns(wearE);
											sgeWear[0].Returns(wearSBE);
											sgeWear.filter.Returns(new SGWearFilter());
										ISlotGroup sgeCGears = MakeSubSG();
											IEnumerable<ISlotSystemElement> sgeCGearsEles;
												ISlottable shieldSBE = MakeSubSB();
													ShieldInstance shieldE = MakeShieldInstance(0);
													shieldSBE.item.Returns(shieldE);
												ISlottable mWeaponSBE = MakeSubSB();
													MeleeWeaponInstance mWeaponE = MakeMeleeWeaponInstance(0);
													mWeaponSBE.item.Returns(mWeaponE);
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());

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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
					ssm.SetHierarchy();
					
					List<ISlottable> list = ssm.allSBs;

					pBun.Received().PerformInHierarchy(ssm.AddSBToRes, Arg.Any<List<ISlottable>>());
					eBun.Received().PerformInHierarchy(ssm.AddSBToRes, Arg.Any<List<ISlottable>>());
					foreach(var gBun in gBuns)
						gBun.Received().PerformInHierarchy(ssm.AddSBToRes, Arg.Any<List<ISlottable>>());
					}
			/*	methods	*/
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
											sbeBow.item.Returns(bowE);
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
											sbeWear.item.Returns(wearE);
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
												sbeBow.item.Returns(bow);
											sgeBowEles = new ISlotSystemElement[]{sbeBow};
										// sgeBow.GetEnumerator().Returns(sgeBowEles.GetEnumerator());
										sgeBow[0].Returns(sbeBow);
										sgeBow.filter.Returns(new SGBowFilter());
									ISlotGroup sgeWear = MakeSubSG();
										IEnumerable<ISlotSystemElement> sgeWearEles;
											ISlottable sbeWear = MakeSubSB();
												WearInstance wear = MakeWearInstance(0);
												sbeWear.item.Returns(wear);
											sgeWearEles = new ISlotSystemElement[]{sbeWear};
										// sgeWear.GetEnumerator().Returns(sgeWearEles.GetEnumerator());
										sgeWear[0].Returns(sbeWear);
										sgeWear.filter.Returns(new SGWearFilter());
									ISlotGroup sgeCGears = MakeSubSG();
										IEnumerable<ISlotSystemElement> sgeCGearsEles;
											ISlottable sbeShield = MakeSubSB();
												ShieldInstance shield = MakeShieldInstance(0);
												sbeShield.item.Returns(shield);
											ISlottable sbeMWeapon = MakeSubSB();
												MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
												sbeMWeapon.item.Returns(mWeapon);
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
											sbeBow.item.Returns(bow);
										sgeBow[0].Returns(sbeBow);
										IEquipmentSetInventory eInv = MakeSubEquipInv();
										sgeBow.inventory.Returns(eInv);
										sgeBow.filter.Returns(new SGBowFilter());
									ISlotGroup sgeWear = MakeSubSG();
										ISlottable sbeWear = MakeSubSB();
											WearInstance wear = MakeWearInstance(0);
											sbeWear.item.Returns(wear);
										sgeWear.filter.Returns(new SGWearFilter());
										sgeWear[0].Returns(sbeWear);
									ISlotGroup sgeCGears = MakeSubSG();
										IEnumerable<ISlotSystemElement> sgeCGearsEles;
											ISlottable sbeShield = MakeSubSB();
												ShieldInstance shield = MakeShieldInstance(0);
												sbeShield.item.Returns(shield);
											ISlottable sbeMWeapon = MakeSubSB();
												MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
												sbeMWeapon.item.Returns(mWeapon);
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
												sbeBow.item.Returns(bowE);
										sgeBow[0].Returns(sbeBow);
									ISlotGroup sgeWear = MakeSubSG();
										sgeWear.filter.Returns(new SGWearFilter());
											ISlottable sbeWear = MakeSubSB();
												WearInstance wearE = MakeWearInstance(0);
												sbeWear.item.Returns(wearE);
										sgeWear[0].Returns(sbeWear);
									ISlotGroup sgeCGears = MakeSubSG();
										IEnumerable<ISlotSystemElement> sgeCGearsEles = new ISlotSystemElement[]{};
										sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
										sgeCGears.filter.Returns(new SGCGearsFilter());
									eSetEles = new ISlotSystemElement[]{sgeBow, sgeWear, sgeCGears};
								eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
							eBun.focusedElement.Returns(eSet);
						IEnumerable<ISlotSystemBundle> gBuns = new ISlotSystemBundle[]{};
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
						mockSB.item.Returns(bow);
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
					ISlottable mockSB = MakeSubSB();
						mockSB.sg.Returns(stubSG);
						mockSB.item.Returns(bow);
						mockSB.isPool.Returns(true);
					
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
						mockSB.item.Returns(bow);
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
					ISlottable mockSB = MakeSubSB();
						mockSB.sg.Returns(stubSG);
						mockSB.item.Returns(bow);
						mockSB.isPool.Returns(true);
					
					ssm.Unequip(mockSB, bow);

					Received.InOrder(() => {
						mockSB.ClearCurEqpState();
						mockSB.Unequip();
					});
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
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
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
				[Test]
				public void Focus_WhenCalled_SetsSelStateFocusedState(){
					SlotSystemManager ssm = MakeSSMWithSelStateHandler();

					ssm.Focus();

					Assert.That(ssm.isFocused, Is.True);
					}
				[Test]
				public void Defocus_WhenCalled_SetsSelStateDefocused(){
					SlotSystemManager ssm = MakeSSMWithSelStateHandler();

					ssm.Defocus();

					Assert.That(ssm.isDefocused, Is.True);
					}
				SlotSystemManager MakeSSMWithSelStateHandler(){
					SlotSystemManager ssm = MakeSSM();
					SSEStateHandler handler = new SSEStateHandler();
					ssm.SetSelStateHandler(handler);
					return ssm;
				}
				[Test]
				public void Deactivate_WhenCalled_SetsSelStateDeactivateed(){
					SlotSystemManager ssm = MakeSSMWithSelStateHandler();

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
			/*	SSE imple */
				[Test]
				public void SetHierarchy_isElementsSetUp_DoesNotThrowException(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns = new ISlotSystemBundle[]{};
					ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());

					ssm.SetHierarchy();
				}
				[Test]
				public void SetHiearchy_TransformChildCountLessThan2_ThrowsException(){
					SlotSystemManager ssm = MakeSSM();
					TestSlotSystemElement stubSSE = MakeTestSSE();
						stubSSE.transform.SetParent(ssm.transform);
					
					Exception ex = Assert.Catch<InvalidOperationException>(() => ssm.SetHierarchy());

					Assert.That(ex.Message, Is.StringContaining("there has to be at least two transform children"));
				}
				[Test]
				public void SetHiearchy_ChildWithoutBundle_ThrowsException(){
					SlotSystemManager ssm = MakeSSM();
					TestSlotSystemElement stubSSEA = MakeTestSSE();
						stubSSEA.transform.SetParent(ssm.transform);
					TestSlotSystemElement stubSSEB = MakeTestSSE();
						stubSSEB.transform.SetParent(ssm.transform);
					
					Exception ex = Assert.Catch(() => ssm.SetHierarchy());
					
					Assert.That(ex.Message, Is.StringContaining(("some child does not have ISlotSystemBundle component")));
				}
				[Test]
				public void SetHierarchy_HasValidTransformChildren_SetsThemBundlesAndSetsTheirParentThis(){
					SlotSystemManager ssm = MakeSSM();
						SlotSystemBundle pBun = MakeSSBundle();
							ISlotGroup stubSGP = MakeSubSG();
								PoolInventory stubPInv = new PoolInventory();
								stubSGP.inventory.Returns(stubPInv);
							IEnumerable<ISlotSystemElement> pBunEles = new ISlotSystemElement[]{stubSGP};
							pBun.SetElements(pBunEles);
							pBun.transform.SetParent(ssm.transform);
						SlotSystemBundle eBun = MakeSSBundle();
							IEnumerable<ISlotSystemElement> eBunEles;
								IEquipmentSet stubESet = Substitute.For<IEquipmentSet>();
								eBunEles = new ISlotSystemElement[]{stubESet};
							eBun.SetElements(eBunEles);
							eBun.transform.SetParent(ssm.transform);
						SlotSystemBundle gBunA = MakeSSBundle();
							gBunA.transform.SetParent(ssm.transform);
							gBunA.SetElements(new ISlotSystemElement[]{MakeSubSG()});
						SlotSystemBundle gBunB = MakeSSBundle();
							gBunB.transform.SetParent(ssm.transform);
							gBunB.SetElements(new ISlotSystemElement[]{MakeSubBundle()});
						IEnumerable<ISlotSystemBundle> xGBuns = new ISlotSystemBundle[]{gBunA, gBunB};

					ssm.SetHierarchy();

					Assert.That(ssm.poolBundle, Is.SameAs(pBun));
					Assert.That(ssm.equipBundle, Is.SameAs(eBun));
					Assert.That(ssm.otherBundles, Is.EqualTo(xGBuns));

					Assert.That(pBun.parent, Is.SameAs(ssm));
					Assert.That(eBun.parent, Is.SameAs(ssm));
					Assert.That(gBunA.parent, Is.SameAs(ssm));
					Assert.That(gBunB.parent, Is.SameAs(ssm));
				}
			/*	helper	*/
				public IEnumerable<ISlotSystemElement> ConvertToSSEs<T>(IEnumerable<T> sgs) where T: ISlotSystemElement{
					foreach(var sg in sgs)
						yield return (ISlotSystemElement)sg;
				}
		}
	}
}
