using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using System;
using System.Collections;
using System.Collections.Generic;
namespace SlotSystemTests{
	namespace ElementsTests{
		[TestFixture]
		public class SlotSystemManagerTests: AbsSlotSystemTest{
			[Test]
			public void SetCurSSM_WhenCalled_CallsPrevSSMDefocus(){
				SlotSystemManager ssm = MakeSSM();
				ISlotSystemManager mockSSM = MakeSubISSM();
				SlotSystemManager.curSSM = mockSSM;

				ssm.SetCurSSM();

				mockSSM.Received().Defocus();
			}
			[Test]
			public void SetCurSSM_WhenCalled_SetsThisAsCur(){
				SlotSystemManager ssm = MakeSSM();
				ISlotSystemManager stubSSM = MakeSubISSM();
				SlotSystemManager.curSSM = stubSSM;

				ssm.SetCurSSM();

				Assert.That(SlotSystemManager.curSSM, Is.SameAs(ssm));
			}
			[Test]
			public void Initialize_WhenCalled_SetsFields(){
				SlotSystemManager ssm = MakeSSM();
				ISlotSystemPageElement poolBundlePE = MakeSubPageElement();
					ISlotSystemBundle poolBundle = MakeSubBundle();
					poolBundlePE.element.Returns(poolBundle);
				ISlotSystemPageElement equipBundlePE = MakeSubPageElement();
					ISlotSystemBundle equipBundle = MakeSubBundle();
					equipBundlePE.element.Returns(equipBundle);
				IEnumerable<ISlotSystemPageElement> genBunPEs;
					ISlotSystemPageElement genBundleAPE = MakeSubPageElement();
						ISlotSystemBundle genBundleA = MakeSubBundle();
						genBundleAPE.element.Returns(genBundleA);
					ISlotSystemPageElement genBundleBPE = MakeSubPageElement();
						ISlotSystemBundle genBundleB = MakeSubBundle();
						genBundleBPE.element.Returns(genBundleB);
					ISlotSystemPageElement genBundleCPE = MakeSubPageElement();
						ISlotSystemBundle genBundleC = MakeSubBundle();
						genBundleCPE.element.Returns(genBundleC);
					genBunPEs = new ISlotSystemPageElement[]{
						genBundleAPE, genBundleBPE, genBundleCPE
					};
				IEnumerable<ISlotSystemBundle> genBundles = new ISlotSystemBundle[]{
					genBundleA, genBundleB, genBundleC
				};
				IEnumerable<ISlotSystemPageElement> pEles = new ISlotSystemPageElement[]{
					poolBundlePE, equipBundlePE, genBundleAPE, genBundleBPE, genBundleCPE
				};
				Initialize_WhenCalled_SetsFieldsTestCase expected = new Initialize_WhenCalled_SetsFieldsTestCase(poolBundle, equipBundle, genBundles, pEles);
				
				ssm.Initialize(poolBundlePE, equipBundlePE, genBunPEs);

				Initialize_WhenCalled_SetsFieldsTestCase actual = new Initialize_WhenCalled_SetsFieldsTestCase(ssm.poolBundle, ssm.equipBundle, ssm.otherBundles, ssm.pageElements);
				bool equality = actual.Equals(expected);

				Assert.That(equality, Is.True);
			}
				class Initialize_WhenCalled_SetsFieldsTestCase: IEquatable<Initialize_WhenCalled_SetsFieldsTestCase>{
					public ISlotSystemBundle poolBundle;
					public ISlotSystemBundle equipBundle;
					public IEnumerable<ISlotSystemBundle> genBundles;
					public IEnumerable<ISlotSystemPageElement> pageEles;
					public Initialize_WhenCalled_SetsFieldsTestCase(ISlotSystemBundle poolBun, ISlotSystemBundle equipBun, IEnumerable<ISlotSystemBundle> genBuns, IEnumerable<ISlotSystemPageElement> pageEles){
						poolBundle = poolBun;
						equipBundle = equipBun;
						genBundles = genBuns;
						this.pageEles = pageEles;
					}
					public bool Equals(Initialize_WhenCalled_SetsFieldsTestCase other){
						bool flag = true;
						flag &= object.ReferenceEquals(this.poolBundle, other.poolBundle);
						flag &= object.ReferenceEquals(this.equipBundle, other.equipBundle);
						IEnumerator gBunRator = this.genBundles.GetEnumerator();
						IEnumerator otherGBunRator = other.genBundles.GetEnumerator();
						IEnumerator pERator = this.pageEles.GetEnumerator();
						IEnumerator otherPERator = other.pageEles.GetEnumerator();
						while(gBunRator.MoveNext() && otherGBunRator.MoveNext()){
							flag &= object.ReferenceEquals(gBunRator.Current, otherGBunRator.Current);
						}
						while(pERator.MoveNext() && otherPERator.MoveNext()){
							flag &= object.ReferenceEquals(pERator.Current, otherPERator.Current);
						}
						return flag;
					}
				}
			[Test]
			public void Initialize_WhenCalled_CallsSetSSMInHierarchy(){
				SlotSystemManager initSSM = InitializedSSM();
				
				Assert.That(initSSM.ssm, Is.SameAs(initSSM));
				initSSM.poolBundle.Received().PerformInHierarchy(initSSM.SetSSM);
				initSSM.equipBundle.Received().PerformInHierarchy(initSSM.SetSSM);
				foreach(var oBun in initSSM.otherBundles)
					oBun.Received().PerformInHierarchy(initSSM.SetSSM);
			}
			[Test]
			public void Initialize_WhenCalled_CallsSetParentInHierarchy(){
				SlotSystemManager initSSM = InitializedSSM();
				
				Assert.That(initSSM.parent, Is.Null);
				initSSM.poolBundle.Received().PerformInHierarchy(initSSM.SetParent);
				initSSM.equipBundle.Received().PerformInHierarchy(initSSM.SetParent);
				foreach(var oBun in initSSM.otherBundles)
					oBun.Received().PerformInHierarchy(initSSM.SetParent);
			}
			[Test]
			public void Initialize_WhenCalled_SetsSelStateSSMDeactivated(){
				SlotSystemManager initSSM = InitializedSSM();

				Assert.That(initSSM.curSelState, Is.SameAs(SlotSystemManager.ssmDeactivatedState));
			}
			[Test]
			public void Initialize_WhenCalled_SetsActStateSSMWFA(){
				SlotSystemManager initSSM = InitializedSSM();

				Assert.That(initSSM.curActState, Is.SameAs(SlotSystemManager.ssmWaitForActionState));
			}
			/*	managerial */
				/*	fields */
					[Test]
					public void AllSGs_Always_CallsPIHAddInSGListInSequence(){
						ISlotSystemPageElement poolBundlePE = MakeSubPoolBundlePEInitWithSGs();
						ISlotSystemPageElement equipBundlePE = MakeSubPoolBundlePEInitWithSGs();
						IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeSubGenBundlePEsInitWithSGs();
						SlotSystemManager ssm = MakeSSM();
						ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);
						List<ISlotGroup> list = ssm.allSGs;
						Received.InOrder(() => {
							poolBundlePE.element.PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
							equipBundlePE.element.PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
							foreach(var pe in genBundlesPEs){
								pe.element.PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
							}
						});
					}
					[Test]
					public void AddInSGList_WhenCalled_VerifySGsAndStoreThemInTheList(){
						SlotSystemManager ssm = MakeSSM();
						ISlottable sb = MakeSubSB();
						ISlotSystemManager stubSSM = MakeSubISSM();
						ISlotSystemElement ele = MakeSubSSE();
						ISlotGroup sgA = MakeSubSG();
						ISlotSystemPage page = MakeSubSSPage();
						ISlotGroup sgB = MakeSubSG();
						IEnumerable<ISlotSystemElement> sses = new ISlotSystemElement[]{
							sb, stubSSM, ele, sgA, page, ele, sgB
						};
						IEnumerable<ISlotGroup> expected = new ISlotGroup[]{sgA, sgB};
						List<ISlotGroup> sgs = new List<ISlotGroup>();

						foreach(var e in sses)
							ssm.AddInSGList(e, sgs);
						
						Assert.That(sgs, Is.EqualTo(expected));
					}
					[Test]
					public void AllSGPs_Always_CallsPoolBundlePIHAddInSGList(){
						ISlotSystemPageElement poolBundlePE = MakeSubPoolBundlePEInitWithSGs();
						ISlotSystemPageElement equipBundlePE = MakeSubPoolBundlePEInitWithSGs();
						IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeSubGenBundlePEsInitWithSGs();
						SlotSystemManager ssm = MakeSSM();
						ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);

						List<ISlotGroup> list = ssm.allSGPs;
						poolBundlePE.element.Received().PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
					}
					[Test]
					public void AllSGEs_Always_CallsEquipBundlePIHAddINSGList(){
						ISlotSystemPageElement poolBundlePE = MakeSubPoolBundlePEInitWithSGs();
						ISlotSystemPageElement equipBundlePE = MakeSubEquipBundlePEInitWithSGs();
						IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeSubGenBundlePEsInitWithSGs();
						SlotSystemManager ssm = MakeSSM();
						ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);

						List<ISlotGroup> list = ssm.allSGEs;
						ssm.equipBundle.Received().PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
					}
					[Test]
					public void AllSGGs_Always_CallsAllGenBundlesPIHAddInSGList(){
						ISlotSystemPageElement poolBundlePE = MakeSubPoolBundlePEInitWithSGs();
						ISlotSystemPageElement equipBundlePE = MakeSubEquipBundlePEInitWithSGs();
						IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeSubGenBundlePEsInitWithSGs();
						SlotSystemManager ssm = MakeSSM();
						ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);

						List<ISlotGroup> list = ssm.allSGGs;

						foreach(ISlotSystemPageElement gBunPE in genBundlesPEs){
							gBunPE.element.Received().PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
						}
					}
					[Test]
					public void FocusedSGP_PoolBundleToggledOn_ReturnsPoolBundleFocusedElement(){
						ISlotGroup focusedSG = MakeSubSGWithEmptySBs();
						ISlotSystemPageElement poolBundlePE = MakeSubPoolBundlePEInitWithSGs();
							poolBundlePE.element.isToggledOn.Returns(true);
							((ISlotSystemBundle)poolBundlePE.element).focusedElement.Returns(focusedSG);
						ISlotSystemPageElement equipBundlePE = MakeSubEquipBundlePEInitWithSGs();
						IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeSubGenBundlePEsInitWithSGs();
						SlotSystemManager ssm = MakeSSM();
							ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);
						
						ISlotGroup actual = ssm.focusedSGP;

						Assert.That(actual, Is.SameAs(focusedSG));
					}
					[Test]
					public void FocusedEpSet_EquipBundleIsToggledOn_ReturnsEquipBundleFocusedElement(){
						IEquipmentSet focusedESet = MakeSubEquipmentSetInitWithSGs();
						ISlotSystemPageElement poolBundlePE = MakeSubPoolBundlePEInitWithSGs();
						ISlotSystemPageElement equipBundlePE = MakeSubEquipBundlePEInitWithSGs();
							equipBundlePE.element.isToggledOn.Returns(true);
							((ISlotSystemBundle)equipBundlePE.element).focusedElement.Returns(focusedESet);
						IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeSubGenBundlePEsInitWithSGs();
						SlotSystemManager ssm = MakeSSM();
							ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);

						IEquipmentSet actual = ssm.focusedEqSet;

						Assert.That(actual, Is.SameAs(focusedESet));
					}
					[Test]
					public void FocusedSGEs_EquipBundleIsToggledOn_ReturnsFocusedESetSGs(){
						ISlotSystemPageElement poolBundlePE = MakeSubPoolBundlePEInitWithSGs();
						ISlotSystemPageElement equipBundlePE = MakeSubEquipBundlePEInitWithSGs();
							IEquipmentSet focusedESet = MakeSubEquipmentSetInitWithSGs();
							equipBundlePE.element.isToggledOn.Returns(true);
							((ISlotSystemBundle)equipBundlePE.element).focusedElement.Returns(focusedESet);
							IEnumerable<ISlotSystemElement> focusedESetEles;
								ISlotGroup sgeA = MakeSubSGWithEmptySBs();
								ISlotGroup sgeB = MakeSubSGWithEmptySBs();
								ISlotGroup sgeC = MakeSubSGWithEmptySBs();
								focusedESetEles = new ISlotSystemElement[]{
									sgeA, sgeB, sgeC
								};
							focusedESet.GetEnumerator().Returns(focusedESetEles.GetEnumerator());
						IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeSubGenBundlePEsInitWithSGs();
						SlotSystemManager ssm = MakeSSM();
						ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);

						IEnumerable<ISlotGroup> actual = ssm.focusedSGEs;
						Assert.That(actual, Is.EqualTo(focusedESetEles));
					}
					[TestCaseSource(typeof(AddFocusedTo_VariousConfigCases))]
					public void AddFocusedSGTo_VariousConfig_ReturnsAccordingly(ISlotSystemElement ele, IEnumerable<ISlotGroup> expected){
						SlotSystemManager ssm = MakeSSM();
						List<ISlotGroup> list = new List<ISlotGroup>();
						ssm.AddFocusedSGTo(ele, list);

						Assert.That(list, Is.EqualTo(expected));
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
						ISlotSystemPageElement poolBundlePE = MakeSubPoolBundlePEInitWithSGs();
						ISlotSystemPageElement equipBundlePE = MakeSubEquipBundlePEInitWithSGs();
						IEnumerable<ISlotSystemPageElement> genBundlePEs;
							ISlotSystemPageElement genBunAPE = MakeSubGenBundlePEInitWithSGs();
								ISlotSystemBundle genBunA = MakeSubBundle();
								genBunAPE.element.Returns(genBunA);
							ISlotSystemPageElement genBunBPE = MakeSubGenBundlePEInitWithSGs();
								ISlotSystemBundle genBunB = MakeSubBundle();
								genBunBPE.element.Returns(genBunB);
							ISlotSystemPageElement genBunCPE = MakeSubGenBundlePEInitWithSGs();
								ISlotSystemBundle genBunC = MakeSubBundle();
								genBunCPE.element.Returns(genBunC);
						genBundlePEs = new ISlotSystemPageElement[]{
							genBunAPE, genBunBPE, genBunCPE
						};
						IEnumerable<ISlotSystemBundle> buns = new ISlotSystemBundle[]{
							genBunA, genBunB, genBunC
						};
						ssm.Initialize(poolBundlePE, equipBundlePE, genBundlePEs);
						
						List<ISlotGroup> list = ssm.focusedSGGs;
						
						foreach(var bun in buns)
							bun.Received().PerformInHierarchy(ssm.AddFocusedSGTo, Arg.Any<List<ISlotGroup>>());
					}
					[TestCaseSource(typeof(FocusedSGsCases))]
					public void FocusedSGs_Always_ReturnsAllTheFocusedSGsInPBunAndEBunAndCallAllGBunPIHAddFocusedSGTo(ISlotSystemPageElement poolBunPE, ISlotSystemPageElement equipBunPE, IEnumerable<ISlotSystemPageElement> genBunPEs, List<ISlotGroup> expected, IEnumerable<ISlotSystemBundle> gBuns){
						SlotSystemManager ssm = MakeSSM();
						ssm.Initialize(poolBunPE, equipBunPE, genBunPEs);
						ssm.Focus();

						List<ISlotGroup> actual = ssm.focusedSGs;

						Assert.That(actual, Is.EqualTo(expected));
						foreach(var gBun in gBuns)
							gBun.Received().PerformInHierarchy(ssm.AddFocusedSGTo, Arg.Any<List<ISlotGroup>>());
					}
						class FocusedSGsCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								ISlotSystemPageElement pBunPE = MakeSubPageElement();
									ISlotSystemBundle pBun = MakeSubBundle();
									pBunPE.element.Returns(pBun);
										ISlotGroup sgpA = MakeSubSGWithEmptySBs();
										ISlotGroup sgpB = MakeSubSGWithEmptySBs();
										ISlotGroup sgpC = MakeSubSGWithEmptySBs();
										IEnumerable<ISlotSystemElement> pBunEles = new ISlotSystemElement[]{
											sgpA, sgpB, sgpC
										};
										pBun.GetEnumerator().Returns(pBunEles.GetEnumerator());
										pBun.isToggledOn.Returns(true);
										pBun.focusedElement.Returns(sgpA);
								ISlotSystemPageElement eBunPE = MakeSubPageElement();
									ISlotSystemBundle eBun = MakeSubBundle();
										eBun.isToggledOn.Returns(true);
									eBunPE.element.Returns(eBun);
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
								IEnumerable<ISlotSystemPageElement> genBunPEs;
									ISlotSystemPageElement gBunAPE = MakeSubPageElement();
										ISlotSystemBundle genBundleA = MakeSubBundle();
											IEnumerable<ISlotSystemElement> gBunAEles;
												ISlotGroup sggA_A = MakeSubSGWithEmptySBs();
												ISlotGroup sggA_B = MakeSubSGWithEmptySBs();
												ISlotGroup sggA_C = MakeSubSGWithEmptySBs();
												gBunAEles = new ISlotSystemElement[]{
													sggA_A, sggA_B, sggA_C
												};
											genBundleA.GetEnumerator().Returns(gBunAEles.GetEnumerator());
										gBunAPE.element.Returns(genBundleA);
									ISlotSystemPageElement gBunBPE = MakeSubPageElement();
										ISlotSystemBundle genBundleB = MakeSubBundle();
											IEnumerable<ISlotSystemElement> gBunBEles;
												ISlotGroup sggB_A = MakeSubSGWithEmptySBs();
												ISlotGroup sggB_B = MakeSubSGWithEmptySBs();
												ISlotGroup sggB_C = MakeSubSGWithEmptySBs();
												gBunBEles = new ISlotSystemElement[]{
													sggB_A, sggB_B, sggB_C
												};
											genBundleB.GetEnumerator().Returns(gBunBEles.GetEnumerator());
										gBunBPE.element.Returns(genBundleB);
									genBunPEs = new ISlotSystemPageElement[]{
										gBunAPE, gBunBPE
									};
								List<ISlotGroup> case1Exp = new List<ISlotGroup>(new ISlotGroup[]{
									sgpB, sgBowC, sgWearC, sgCGearsC
								});
								IEnumerable<ISlotSystemBundle> gBuns = new ISlotSystemBundle[]{
									genBundleA, genBundleB
								};
								yield return new object[]{
									pBunPE, eBunPE, genBunPEs, case1Exp, gBuns
								};
							}
						}

					[Test]
					public void EquipmentSets_Always_ReturnsEquipBundleElements(){
						SlotSystemManager ssm = MakeSSM();
						ISlotSystemPageElement poolBunPE = MakeSubPoolBundlePEInitWithSGs();
						IEnumerable<ISlotSystemPageElement> genBunPEs = MakeSubGenBundlePEsInitWithSGs();
						ISlotSystemPageElement eBunPE = MakeSubPageElement();
							ISlotSystemBundle equipmentBundle = MakeSubBundle();
								IEnumerable<ISlotSystemElement> eBunEles;
									IEquipmentSet eSetA = AbsSlotSystemTest.MakeSubEquipmentSetInitWithSGs();
									IEquipmentSet eSetB = AbsSlotSystemTest.MakeSubEquipmentSetInitWithSGs();
									IEquipmentSet eSetC = AbsSlotSystemTest.MakeSubEquipmentSetInitWithSGs();						
									eBunEles = new ISlotSystemElement[]{
										eSetA, eSetB, eSetC
									};
								equipmentBundle.GetEnumerator().Returns(eBunEles.GetEnumerator());
							eBunPE.element.Returns(equipmentBundle);
						ssm.Initialize(poolBunPE, eBunPE, genBunPEs);

						List<IEquipmentSet> actual = ssm.equipmentSets;

						Assert.That(actual, Is.EqualTo(eBunEles));
					}
					[Test]
					public void poolInv_Always_ReturnsFocusedSGPsInventory(){
						SlotSystemManager ssm = MakeSSM();
						ISlotSystemPageElement eBunPE = MakeSubEquipBundlePEInitWithSGs();
						IEnumerable<ISlotSystemPageElement> genBunPEs = MakeSubGenBundlePEsInitWithSGs();
						ISlotSystemBundle pBun = MakeSubBundle();
							ISlotGroup sgpA = MakeSubSG();
								sgpA.slottables.Returns(new List<ISlottable>());
							ISlotGroup sgpB = MakeSubSG();
								sgpA.slottables.Returns(new List<ISlottable>());
							ISlotGroup sgpC = MakeSubSG();
								sgpA.slottables.Returns(new List<ISlottable>());
							IPoolInventory pInv = MakeSubPoolInv();
							sgpA.inventory.Returns(pInv);
							pBun.elements.Returns(new ISlotSystemElement[]{
								sgpA, sgpB, sgpC
							});
							pBun.focusedElement.Returns(sgpA);
							ISlotSystemPageElement pBunPE = MakeSubPageElement();
							pBunPE.element.Returns(pBun);
							pBun.isToggledOn.Returns(true);
						ssm.Initialize(pBunPE, eBunPE, genBunPEs);

						IPoolInventory actual = ssm.poolInv;

						Assert.That(actual, Is.SameAs(pInv));
					}
					[Test]
					public void equipInv_Always_ReturnsFocusedSGEsInventory(){
						SlotSystemManager ssm = MakeSSM();
							ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
							IEquipmentSetInventory eInv;
							ISlotSystemPageElement eBunPE = MakeSubEBunPEWithEquipInv(out eInv);
							IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
							ssm.Initialize(pBunPE, eBunPE, gBunPEs);
						
						IEquipmentSetInventory actual = ssm.equipInv;

						Assert.That(actual, Is.SameAs(eInv));
					}
					[Test]
					public void equippedBowInst_Always_ReturnsFocusedSGEWithBowFilterFirtSlotSBItemInst(){
						SlotSystemManager ssm = MakeSSM();
							ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
							IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
							ISlotSystemPageElement eBunPE = MakeSubPageElement();
								ISlotSystemBundle eBun = MakeSubBundle();
								eBun.isToggledOn.Returns(true);
									IEnumerable<ISlotSystemElement> eBunEles;
										IEquipmentSet eSet = MakeSubEquipmentSetInitWithSGs();
											IEnumerable<ISlotSystemElement> eSetEles;
												ISlotGroup sgeBow = MakeSGEBow();
													BowInstance expected = (BowInstance)sgeBow.slots[0].sb.itemInst;
												ISlotGroup sgeWear = MakeSGEWear();
												ISlotGroup sgeCGears = MakeSGECGears();
												eSetEles = new ISlotSystemElement[]{
													sgeBow, sgeWear, sgeCGears
												};
											eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
										eBunEles = new ISlotSystemElement[]{
											eSet
										};
									eBun.GetEnumerator().Returns(eBunEles.GetEnumerator());
									eBun.focusedElement.Returns(eSet);
								eBunPE.element.Returns(eBun);
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);

						BowInstance actual = ssm.equippedBowInst;

						Assert.That(actual, Is.SameAs(expected));
					}
					[Test]
					public void equippedWearInst_Always_ReturnsFocusedSGEWithWearFilterFirtSlotSBItemInst(){
						SlotSystemManager ssm = MakeSSM();
							ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
							IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
							ISlotSystemPageElement eBunPE = MakeSubPageElement();
								ISlotSystemBundle eBun = MakeSubBundle();
								eBun.isToggledOn.Returns(true);
									IEnumerable<ISlotSystemElement> eBunEles;
										IEquipmentSet eSet = MakeSubEquipmentSetInitWithSGs();
											IEnumerable<ISlotSystemElement> eSetEles;
												ISlotGroup sgeBow = MakeSGEBow();
												ISlotGroup sgeWear = MakeSGEWear();
													WearInstance expected = (WearInstance)sgeWear.slots[0].sb.itemInst;
												ISlotGroup sgeCGears = MakeSGECGears();
												eSetEles = new ISlotSystemElement[]{
													sgeBow, sgeWear, sgeCGears
												};
											eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
										eBunEles = new ISlotSystemElement[]{
											eSet
										};
									eBun.GetEnumerator().Returns(eBunEles.GetEnumerator());
									eBun.focusedElement.Returns(eSet);
								eBunPE.element.Returns(eBun);
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);

						WearInstance actual = ssm.equippedWearInst;

						Assert.That(actual, Is.SameAs(expected));
					}
					[Test]
					public void equippedCarriedGears_Always_ReturnsFocusedSGEWithCGFilterAllElements(){
						SlotSystemManager ssm = MakeSSM();
							ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
							IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
							ISlotSystemPageElement eBunPE = MakeSubPageElement();
								ISlotSystemBundle eBun = MakeSubBundle();
								eBun.isToggledOn.Returns(true);
									IEnumerable<ISlotSystemElement> eBunEles;
										IEquipmentSet eSet = MakeSubEquipmentSetInitWithSGs();
											IEnumerable<ISlotSystemElement> eSetEles;
												ISlotGroup sgeBow = MakeSGEBow();
												ISlotGroup sgeWear = MakeSGEWear();
												ISlotGroup sgeCGears = MakeSGECGears();
													List<CarriedGearInstance> expected = new List<CarriedGearInstance>();
													foreach(var ele in sgeCGears)
														expected.Add((CarriedGearInstance)((ISlottable)ele).itemInst);
												eSetEles = new ISlotSystemElement[]{
													sgeBow, sgeWear, sgeCGears
												};
											eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
										eBunEles = new ISlotSystemElement[]{
											eSet
										};
									eBun.GetEnumerator().Returns(eBunEles.GetEnumerator());
									eBun.focusedElement.Returns(eSet);
								eBunPE.element.Returns(eBun);
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);

						List<CarriedGearInstance> actual = ssm.equippedCarriedGears;

						Assert.That(actual, Is.EqualTo(expected));
					}
					[Test]
					public void allEquippedItems_Always_ReturnsSumOfAllThree(){
						SlotSystemManager ssm = MakeSSM();
							ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
							IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
							ISlotGroup sgeBow;
							ISlotGroup sgeWear;
							ISlotGroup sgeCGears;
							ISlotSystemPageElement eBunPE = MakeEBunPEWithSGEs(out sgeBow, out sgeWear, out sgeCGears);
						List<InventoryItemInstance> expected = new List<InventoryItemInstance>();
							expected.Add(sgeBow.slots[0].sb.itemInst);
							expected.Add(sgeWear.slots[0].sb.itemInst);
							foreach(var ele in sgeCGears)
								expected.Add((CarriedGearInstance)((ISlottable)ele).itemInst);
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);

						List<InventoryItemInstance> actual = ssm.allEquippedItems;

						Assert.That(actual, Is.EqualTo(expected));
					}
					[Test]
					public void AddSBsToRes_WhenCalled_FindSBsAndAddThemIntoTheList(){
						SlotSystemManager ssm = MakeSSM();
							ISlotGroup sg = MakeSubSG();
							ISlottable sbA = MakeSubSB();
							ISlotSystemElement ele = Substitute.For<ISlotSystemElement>();
							ISlotSystemPage page = MakeSubSSPage();
							ISlottable sbB = MakeSubSB();
							ISlotSystemBundle bundle = MakeSubBundle();
							IEnumerable<ISlotSystemElement> elements = new ISlotSystemElement[]{
								sg, sbA, ele, page, sbB, bundle
							};
							IEnumerable<ISlottable> expected = new ISlottable[]{
								sbA, sbB
							};
							List<ISlottable> actual = new List<ISlottable>();

							foreach(var e in elements)
								ssm.AddSBToRes(e, actual);
							
							Assert.That(actual, Is.EqualTo(expected));
					}
					[Test]
					public void allSBs_WhenCalled_CallsAllBundlesPIHAddSBtoRes(){
						SlotSystemManager ssm = MakeSSM();
							ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
							ISlotSystemPageElement eBunPE = MakeSubEquipBundlePEInitWithSGs();
							IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);
						
						List<ISlottable> list = ssm.allSBs;

						pBunPE.element.Received().PerformInHierarchy(ssm.AddSBToRes, Arg.Any<List<ISlottable>>());
						eBunPE.element.Received().PerformInHierarchy(ssm.AddSBToRes, Arg.Any<List<ISlottable>>());
						foreach(var pe in gBunPEs)
							pe.element.Received().PerformInHierarchy(ssm.AddSBToRes, Arg.Any<List<ISlottable>>());
					}
				/*	methods	*/
					[Test]
					public void Reset_WhenCalled_SetsActStateWFA(){
						SlotSystemManager ssm = MakeSSM();

						ssm.Reset();

						Assert.That(ssm.curActState, Is.SameAs(SlotSystemManager.ssmWaitForActionState));
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
						SlotSystemManager ssm = InitializedSSM();

						ssm.ResetAndFocus();

						Assert.That(ssm.curSelState, Is.SameAs(SlotSystemManager.ssmFocusedState));
					}
					[Test]
					public void UpdateEquipStatesOnAll_WhenCalled_CallsEInvRemoveWithItemNotInAllEquippedItems(){
						SlotSystemManager ssm = MakeSSM();
							IPoolInventory pInv;
						ISlotSystemPageElement pBunPE = MakePBunWithPoolInv(out pInv);
						IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
						ISlotGroup sgeBow;
						ISlotGroup sgeWear;
						ISlotGroup sgeCGears;
						IEquipmentSetInventory eInv;
						ISlotSystemPageElement eBunPE = MakeEBunPEWithSGEsAndEquipInv(out sgeBow, out sgeWear, out sgeCGears, out eInv);
							ISlottable sbeBow = MakeSubSB();
								BowInstance bow = MakeBowInstance(0);
								sbeBow.itemInst.Returns(bow);
							ISlottable sbeWear = MakeSubSB();
								WearInstance wear = MakeWearInstance(0);
								sbeWear.itemInst.Returns(wear);
							IEnumerable<ISlotSystemElement> sgeCGearsEles;
								ISlottable sbeShield = MakeSubSB();
									ShieldInstance shield = MakeShieldInstance(0);
									sbeShield.itemInst.Returns(shield);
								ISlottable sbeMWeapon = MakeSubSB();
									MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
									sbeMWeapon.itemInst.Returns(mWeapon);
								sgeCGearsEles = new ISlotSystemElement[]{sbeShield, sbeMWeapon};
							List<Slot> bowSlots = new List<Slot>();
								Slot bowSlot = new Slot();
								bowSlot.sb = sbeBow;
								bowSlots.Add(bowSlot);
								sgeBow.slots.Returns(bowSlots);
							List<Slot> wearSlots = new List<Slot>();
								Slot wearSlot = new Slot();
								wearSlot.sb = sbeWear;
								wearSlots.Add(wearSlot);
								sgeWear.slots.Returns(wearSlots);
							sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
						IEnumerable<SlottableItem> eInvEles;
							BowInstance bowR = MakeBowInstance(0);
							WearInstance wearR = MakeWearInstance(0);
							QuiverInstance quiverR = MakeQuiverInstance(0);
							PackInstance packR = MakePackInstance(0);
							eInvEles = new SlottableItem[]{
								bowR, wearR, quiverR, packR
							};
						eInv.GetEnumerator().Returns(eInvEles.GetEnumerator());
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);
						pBunPE.element.isToggledOn.Returns(true);

						ssm.UpdateEquipStatesOnAll();

						eInv.Received().Remove(bowR);
						eInv.Received().Remove(wearR);
						eInv.Received().Remove(quiverR);
						eInv.Received().Remove(packR);
					}
					[Test]
					public void UpdateEquipStatesOnAll_WhenCalled_CallsEInvAddWithItemNotInEquipInv(){
						SlotSystemManager ssm = MakeSSM();
							IPoolInventory pInv;
						ISlotSystemPageElement pBunPE = MakePBunWithPoolInv(out pInv);
						IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
						ISlotGroup sgeBow;
						ISlotGroup sgeWear;
						ISlotGroup sgeCGears;
						IEquipmentSetInventory eInv;
						ISlotSystemPageElement eBunPE = MakeEBunPEWithSGEsAndEquipInv(out sgeBow, out sgeWear, out sgeCGears, out eInv);
							ISlottable sbeBow = MakeSubSB();
								BowInstance bow = MakeBowInstance(0);
								sbeBow.itemInst.Returns(bow);
							ISlottable sbeWear = MakeSubSB();
								WearInstance wear = MakeWearInstance(0);
								sbeWear.itemInst.Returns(wear);
							IEnumerable<ISlotSystemElement> sgeCGearsEles;
								ISlottable sbeShield = MakeSubSB();
									ShieldInstance shield = MakeShieldInstance(0);
									sbeShield.itemInst.Returns(shield);
								ISlottable sbeMWeapon = MakeSubSB();
									MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
									sbeMWeapon.itemInst.Returns(mWeapon);
								sgeCGearsEles = new ISlotSystemElement[]{sbeShield, sbeMWeapon};
							List<Slot> bowSlots = new List<Slot>();
								Slot bowSlot = new Slot();
								bowSlot.sb = sbeBow;
								bowSlots.Add(bowSlot);
								sgeBow.slots.Returns(bowSlots);
							List<Slot> wearSlots = new List<Slot>();
								Slot wearSlot = new Slot();
								wearSlot.sb = sbeWear;
								wearSlots.Add(wearSlot);
								sgeWear.slots.Returns(wearSlots);
							sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
						IEnumerable<SlottableItem> eInvEles;
							BowInstance bowR = MakeBowInstance(0);
							WearInstance wearR = MakeWearInstance(0);
							QuiverInstance quiverR = MakeQuiverInstance(0);
							PackInstance packR = MakePackInstance(0);
							eInvEles = new SlottableItem[]{
								bowR, wearR, quiverR, packR
							};
						eInv.GetEnumerator().Returns(eInvEles.GetEnumerator());
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);
						pBunPE.element.isToggledOn.Returns(true);

						ssm.UpdateEquipStatesOnAll();

						eInv.Received().Add(bow);
						eInv.Received().Add(wear);
						eInv.Received().Add(shield);
						eInv.Received().Add(mWeapon);
					}
					[Test]
					public void UpdateEquipStatesOnAll_WhenCalled_UpdatePoolInvItemInstsEquipStatus(){
						SlotSystemManager ssm = MakeSSM();
							IPoolInventory pInv;
						ISlotSystemPageElement pBunPE = MakePBunWithPoolInv(out pInv);
						IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
						ISlotGroup sgeBow;
						ISlotGroup sgeWear;
						ISlotGroup sgeCGears;
						IEquipmentSetInventory eInv;
						ISlotSystemPageElement eBunPE = MakeEBunPEWithSGEsAndEquipInv(out sgeBow, out sgeWear, out sgeCGears, out eInv);
							ISlottable sbeBow = MakeSubSB();
								BowInstance bow = MakeBowInstance(0);
								sbeBow.itemInst.Returns(bow);
							ISlottable sbeWear = MakeSubSB();
								WearInstance wear = MakeWearInstance(0);
								sbeWear.itemInst.Returns(wear);
							IEnumerable<ISlotSystemElement> sgeCGearsEles;
								ISlottable sbeShield = MakeSubSB();
									ShieldInstance shield = MakeShieldInstance(0);
									sbeShield.itemInst.Returns(shield);
								ISlottable sbeMWeapon = MakeSubSB();
									MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
									sbeMWeapon.itemInst.Returns(mWeapon);
								sgeCGearsEles = new ISlotSystemElement[]{sbeShield, sbeMWeapon};
							List<Slot> bowSlots = new List<Slot>();
								Slot bowSlot = new Slot();
								bowSlot.sb = sbeBow;
								bowSlots.Add(bowSlot);
								sgeBow.slots.Returns(bowSlots);
							List<Slot> wearSlots = new List<Slot>();
								Slot wearSlot = new Slot();
								wearSlot.sb = sbeWear;
								wearSlots.Add(wearSlot);
								sgeWear.slots.Returns(wearSlots);
							sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
						IEnumerable<SlottableItem> eInvEles;
							BowInstance bowR = MakeBowInstance(0);
							WearInstance wearR = MakeWearInstance(0);
							QuiverInstance quiverR = MakeQuiverInstance(0);
							PackInstance packR = MakePackInstance(0);
							eInvEles = new SlottableItem[]{
								bowR, wearR, quiverR, packR
							};
						eInv.GetEnumerator().Returns(eInvEles.GetEnumerator());
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);
						pBunPE.element.isToggledOn.Returns(true);
						IEnumerable<SlottableItem> pInvEles = new SlottableItem[]{
							bow, wear, shield, mWeapon,
							bowR, wearR, quiverR, packR
						};
						pInv.GetEnumerator().Returns(pInvEles.GetEnumerator());

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
					// [Test]
					public void SortSG_WhenCalled_SetsFields(){
						SlotSystemManager ssm = MakeSSM();
						ISlotGroup sg = MakeSubSG();
						SGSorter sorter = Substitute.For<SGSorter>();

						ssm.SortSG(sg, sorter);

						Assert.That(ssm.targetSB, Is.Not.Null);
						Assert.That(ssm.sg1, Is.Not.SameAs(sg));
						Assert.That(ssm.transaction, Is.Not.TypeOf(typeof(SortTransaction)));
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
								IPoolInventory pInv;
							ISlotSystemPageElement pBunPE = MakePBunWithPoolInv(out pInv);
								IEnumerable<SlottableItem> pInvEles = new SlottableItem[]{};
								pInv.GetEnumerator().Returns(pInvEles.GetEnumerator());
								IEquipmentSetInventory eInv;
								pBunPE.element.isToggledOn.Returns(true);
							ISlotSystemPageElement eBunPE = MakeSubEBunPEWithEquipInv(out eInv);
							IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);
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
					public void MarkEquippedInPool_WhenCalled_FindItemInPoolAndSetsIsEquippedAccordingly(IEnumerable<SlottableItem> items, InventoryItemInstance item, bool equipped, InventoryItemInstance expectedItem){
						SlotSystemManager ssm = MakeSSM();
								IPoolInventory pInv;
							ISlotSystemPageElement pBunPE = MakePBunWithPoolInv(out pInv);
								pBunPE.element.isToggledOn.Returns(true);
								pInv.GetEnumerator().Returns(items.GetEnumerator());
							ISlotSystemPageElement eBunPE = MakeSubEquipBundlePEInitWithSGs();
							IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);

						ssm.MarkEquippedInPool(item, equipped);

						Assert.That(expectedItem.isEquipped, Is.EqualTo(equipped));
					}
						class MarkEquippedInPoolCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								BowInstance bow = MakeBowInstance(0);
								WearInstance wear = MakeWearInstance(0);
								ShieldInstance shield = MakeShieldInstance(0);
								MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
								QuiverInstance quiver = MakeQuiverInstance(0);
								PackInstance pack = MakePackInstance(0);
								PartsInstance parts = MakePartsInstance(0, 2);
								IEnumerable<SlottableItem> items = new SlottableItem[]{
									bow, wear, shield, mWeapon, quiver, pack, parts
								};
								yield return new object[]{items, bow, true, bow}; yield return new object[]{items, bow, false, bow};
								yield return new object[]{items, wear, true, wear}; yield return new object[]{items, wear, false, wear};
								yield return new object[]{items, shield, true, shield}; yield return new object[]{items, shield, false, shield};
								yield return new object[]{items, mWeapon, true, mWeapon}; yield return new object[]{items, mWeapon, false, mWeapon};
								yield return new object[]{items, quiver, true, quiver}; yield return new object[]{items, quiver, false, quiver};
								yield return new object[]{items, pack, true, pack}; yield return new object[]{items, pack, false, pack};
								yield return new object[]{items, parts, true, parts}; yield return new object[]{items, parts, false, parts};

							}
						}
					[TestCase(true)]
					[TestCase(false)]
					public void SetEquippedOnAll_WhenCalled_CallsAllBundlesPIHAppropriateMethods(bool equipped){
						SlotSystemManager ssm = MakeSSM();
						ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
						ISlotSystemPageElement eBunPE = MakeSubEquipBundlePEInitWithSGs();
						IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);
						BowInstance bow = MakeBowInstance(0);

						ssm.SetEquippedOnAllSBs(bow, equipped);
						if(equipped){
							pBunPE.element.Received().PerformInHierarchy(ssm.Equip, bow);
							eBunPE.element.Received().PerformInHierarchy(ssm.Equip, bow);
							foreach(var pe in gBunPEs)
								pe.element.Received().PerformInHierarchy(ssm.Equip, bow);
						}else{
							pBunPE.element.Received().PerformInHierarchy(ssm.Unequip, bow);
							eBunPE.element.Received().PerformInHierarchy(ssm.Unequip, bow);
							foreach(var pe in gBunPEs)
								pe.element.Received().PerformInHierarchy(ssm.Unequip, bow);
						}

					}
					[Test]
					public void Equip_MatchesAndSGFocusedInBundleAndSBNewSlotIDNotMinus1_CallsSBSetEqpStateEquippedState(){
						SlotSystemManager ssm = MakeSSM();
						BowInstance bow = MakeBowInstance(0);
						ISlotGroup stubSG = MakeSubSG();
							stubSG.isFocusedInBundle.Returns(true);
						ISlottable mockSB = MakeSubSB();
							mockSB.sg.Returns(stubSG);
							mockSB.itemInst.Returns(bow);
							mockSB.newSlotID.Returns(0);
						
						ssm.Equip(mockSB, bow);

						mockSB.Received().SetEqpState(Slottable.equippedState);
					}
					[Test]
					public void Equip_MatchesAndSGNOTFocusedInBundleAndSGIsPool_CallsSBInOrder(){
						SlotSystemManager ssm = MakeSSM();
						BowInstance bow = MakeBowInstance(0);
						ISlotGroup stubSG = MakeSubSG();
							stubSG.isFocusedInBundle.Returns(false);
							stubSG.isPool.Returns(true);
						ISlottable mockSB = MakeSubSB();
							mockSB.sg.Returns(stubSG);
							mockSB.itemInst.Returns(bow);
						
						ssm.Equip(mockSB, bow);

						Received.InOrder(() => {
							mockSB.SetEqpState((SSEState) null);
							mockSB.SetEqpState(Slottable.equippedState);
						});
					}
					[Test]
					public void Unequip_MatchesAndSGFocusedInBundleAndSBSlotIDNotMinus1_CallsSBSetEqpStateUnequippedState(){
						SlotSystemManager ssm = MakeSSM();
						BowInstance bow = MakeBowInstance(0);
						ISlotGroup stubSG = MakeSubSG();
							stubSG.isFocusedInBundle.Returns(true);
						ISlottable mockSB = MakeSubSB();
							mockSB.sg.Returns(stubSG);
							mockSB.itemInst.Returns(bow);
							mockSB.slotID.Returns(0);
						
						ssm.Unequip(mockSB, bow);

						mockSB.Received().SetEqpState(Slottable.unequippedState);
					}
					[Test]
					public void Unequip_MatchesAndSGNOTFocusedInBundleAndSGIsPool_CallsSBInOrder(){
						SlotSystemManager ssm = MakeSSM();
						BowInstance bow = MakeBowInstance(0);
						ISlotGroup stubSG = MakeSubSG();
							stubSG.isFocusedInBundle.Returns(false);
							stubSG.isPool.Returns(true);
						ISlottable mockSB = MakeSubSB();
							mockSB.sg.Returns(stubSG);
							mockSB.itemInst.Returns(bow);
						
						ssm.Unequip(mockSB, bow);

						Received.InOrder(() => {
							mockSB.SetEqpState((SSEState) null);
							mockSB.SetEqpState(Slottable.unequippedState);
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
								ISlottable stubSBA = MakeSubSB();
								DraggedIcon diA = new DraggedIcon(stubSBA);
								ISlottable stubSBB = MakeSubSB();
								DraggedIcon diB = new DraggedIcon(stubSBB);
								ISlotGroup sgA = MakeSubSG();
								ISlotGroup sgB = MakeSubSG();
								yield return new object[]{null, null, null, null, true};
								yield return new object[]{diA, null, null, null, false};
								yield return new object[]{null, diB, null, null, false};
								yield return new object[]{null, null, sgA, null, false};
								yield return new object[]{null, null, null, sgB, false};
								yield return new object[]{diA, diB, sgA, sgB, false};
							}
						}
					[Test]
					public void FindParent_WhenCalled_CallAllBundlesPIHCheckAndReportParent(){
						SlotSystemManager ssm = MakeSSM();
						ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
						ISlotSystemPageElement eBunPE = MakeSubEquipBundlePEInitWithSGs();
						IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);

						ISlotSystemElement ele = MakeSubSSE();
						ssm.FindParent(ele);

						pBunPE.element.Received().PerformInHierarchy(ssm.CheckAndReportParent, ele);
						eBunPE.element.Received().PerformInHierarchy(ssm.CheckAndReportParent, ele);
						foreach(var pe in gBunPEs)
							pe.element.Received().PerformInHierarchy(ssm.CheckAndReportParent, ele);
					}
					[TestCaseSource(typeof(CheckAndReportParentCases))]
					public void CheckAndReportParent_MatchesAndNonSlottable_SetsSSMFoundParentWithIt(ISlotSystemElement parent, ISlotSystemElement child, bool valid){
						SlotSystemManager ssm = MakeSSM();

						ssm.CheckAndReportParent(parent, child);

						if(valid)
							Assert.That(ssm.foundParent, Is.SameAs(parent));
						else
							Assert.That(ssm.foundParent, Is.Null);
					}
						class CheckAndReportParentCases: IEnumerable{
							public IEnumerator GetEnumerator(){
									ISlotSystemBundle bundle = MakeSubBundle();
									IEnumerable<ISlotSystemElement> bundleEles;
										ISlotSystemPage page = MakeSubSSPage();
										IEnumerable<ISlotSystemElement> pageEles;
											ISlotGroup sgA = MakeSubSG();
											IEnumerable<ISlotSystemElement> sgAEles;
												ISlottable sbA = MakeSubSB();
												ISlottable sbB = MakeSubSB();
												ISlottable sbC = MakeSubSB();
												sgAEles = new ISlotSystemElement[]{sbA, sbB, sbC};
											sgA.GetEnumerator().Returns(sgAEles.GetEnumerator());
											pageEles = new ISlotSystemElement[]{sgA};
										page.GetEnumerator().Returns(pageEles.GetEnumerator());
										bundleEles = new ISlotSystemElement[]{page};
									bundle.GetEnumerator().Returns(bundleEles.GetEnumerator());
								yield return new object[]{bundle, page, true};
								yield return new object[]{bundle, sgA, false};
								yield return new object[]{page, sgA, true};
								yield return new object[]{page, bundle, false};
								yield return new object[]{page, sbA, false};
								yield return new object[]{sgA, sbA, true};
								yield return new object[]{sgA, sbB, true};
								yield return new object[]{sgA, sbC, true};
							}
						}
					[TestCaseSource(typeof(SetParentCases))]
					public void SetParent_NotSBNorSG_SetsAllElementsParAsSelf(ISlotSystemElement parent, IEnumerable<ISlotSystemElement> elements, bool valid){
						SlotSystemManager ssm = MakeSSM();
						
						ssm.SetParent(parent);

						foreach(var ele in elements)
							if(valid)
								ele.Received().parent = parent;
							else
								ele.DidNotReceive().parent = parent;
					}
						class SetParentCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								ISlotSystemBundle bundle = MakeSubBundle();
								IEnumerable<ISlotSystemElement> bundleEles;
									ISlotSystemPage pageA = MakeSubSSPage();
									IEnumerable<ISlotSystemElement> pageAEles;
										ISlotGroup sgA = MakeSubSG();
										IEnumerable<ISlotSystemElement> sgAEles;
											ISlottable sbAA = MakeSubSB();
											ISlottable sbAB = MakeSubSB();
											ISlottable sbAC = MakeSubSB();
											sgAEles = new ISlotSystemElement[]{sbAA, sbAB, sbAC};
										sgA.GetEnumerator().Returns(sgAEles.GetEnumerator());
										ISlotGroup sgA1 = MakeSubSG();
										IEnumerable<ISlotSystemElement> sgA1Eles;
											ISlottable sbA1A = MakeSubSB();
											ISlottable sbA1B = MakeSubSB();
											ISlottable sbA1C = MakeSubSB();
											sgA1Eles = new ISlotSystemElement[]{sbA1A, sbA1B, sbA1C};
										sgA1.GetEnumerator().Returns(sgA1Eles.GetEnumerator());
										pageAEles = new ISlotSystemElement[]{sgA, sgA1};
									pageA.GetEnumerator().Returns(pageAEles.GetEnumerator());
									ISlotSystemPage pageB = MakeSubSSPage();
									IEnumerable<ISlotSystemElement> pageBEles;
										ISlotGroup sgB = MakeSubSG();
										IEnumerable<ISlotSystemElement> sgBEles;
											ISlottable sbB = MakeSubSB();
											ISlottable sbB1 = MakeSubSB();
											ISlottable sbB2 = MakeSubSB();
											sgBEles = new ISlotSystemElement[]{sbB, sbB1, sbB2};
										sgB.GetEnumerator().Returns(sgBEles.GetEnumerator());
										pageBEles = new ISlotSystemElement[]{sgB};
										ISlotGroup sgB1 = MakeSubSG();
										IEnumerable<ISlotSystemElement> sgB1Eles;
											ISlottable sbB1A = MakeSubSB();
											ISlottable sbB1B = MakeSubSB();
											ISlottable sbB1C = MakeSubSB();
											sgB1Eles = new ISlotSystemElement[]{sbB1A, sbB1B, sbB1C};
										sgB1.GetEnumerator().Returns(sgB1Eles.GetEnumerator());
										pageBEles = new ISlotSystemElement[]{sgB, sgB1};
									pageB.GetEnumerator().Returns(pageBEles.GetEnumerator());
									bundleEles = new ISlotSystemElement[]{pageA, pageB};
								bundle.GetEnumerator().Returns(bundleEles.GetEnumerator());

								yield return new object[]{bundle, bundleEles, true};
								yield return new object[]{pageA, pageAEles, true};
								yield return new object[]{pageB, pageBEles, true};
								yield return new object[]{sgA, sgAEles, false};
								yield return new object[]{sgA1, sgA1Eles, false};
								yield return new object[]{sgB, sgBEles, false};
								yield return new object[]{sgB1, sgB1Eles, false};
							}
						}
					[Test]
					public void Focus_WhenCalled_SetsSelStateFocusedState(){
						SlotSystemManager ssm = InitializedSSM();

						ssm.Focus();

						Assert.That(ssm.curSelState, Is.SameAs(SlotSystemManager.ssmFocusedState));
					}
					[TestCase(true, true, true, true, true)]
					[TestCase(false, false, false, false, false)]
					[TestCase(false, true, false, true, true)]
					public void Focus_WhenCalled_CallsPEsAccordingly(bool pBunPEOn, bool eBunPEOn, bool gBunAPEOn, bool gBunBPEOn, bool gBunCPEOn){
						SlotSystemManager ssm = MakeSSM();
							ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
								pBunPE.isFocusToggleOn.Returns(pBunPEOn);
							ISlotSystemPageElement eBunPE = MakeSubEquipBundlePEInitWithSGs();
								eBunPE.isFocusToggleOn.Returns(eBunPEOn);
							IEnumerable<ISlotSystemPageElement> gBunPEs;
								ISlotSystemPageElement gBunAPE = MakeSubGenBundlePEInitWithSGs();
									ISlotSystemBundle gBunA = MakeSubBundle();
									gBunAPE.element.Returns(gBunA);
									gBunAPE.isFocusToggleOn.Returns(gBunAPEOn);
								ISlotSystemPageElement gBunBPE = MakeSubGenBundlePEInitWithSGs();
									ISlotSystemBundle gBunB = MakeSubBundle();
									gBunBPE.element.Returns(gBunB);
									gBunBPE.isFocusToggleOn.Returns(gBunBPEOn);
								ISlotSystemPageElement gBunCPE = MakeSubGenBundlePEInitWithSGs();
									ISlotSystemBundle gBunC = MakeSubBundle();
									gBunCPE.element.Returns(gBunC);
									gBunCPE.isFocusToggleOn.Returns(gBunCPEOn);
								gBunPEs = new ISlotSystemPageElement[]{gBunAPE, gBunBPE, gBunCPE};
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);
							
						ssm.Focus();

						if(pBunPEOn)
							pBunPE.Received().Focus();
						else
							pBunPE.Received().Defocus();
						if(eBunPEOn)
							eBunPE.Received().Focus();
						else
							eBunPE.Received().Defocus();
						if(gBunAPEOn)
							gBunAPE.Received().Focus();
						else
							gBunAPE.Received().Defocus();
						if(gBunBPEOn)
							gBunBPE.Received().Focus();
						else
							gBunBPE.Received().Defocus();
						if(gBunCPEOn)
							gBunCPE.Received().Focus();
						else
							gBunCPE.Received().Defocus();
					}
					[Test]
					public void Defocus_WhenCalled_SetsSelStateDefocused(){
						SlotSystemManager ssm = InitializedSSM();

						ssm.Defocus();

						Assert.That(ssm.curSelState, Is.SameAs(SlotSystemManager.ssmDefocusedState));
					}
					[Test]
					public void Defocus_WhenCalled_CallsAllBundlesDefocus(){
						SlotSystemManager ssm = InitializedSSM();

						ssm.Defocus();

						ssm.poolBundle.Received().Defocus();
						ssm.equipBundle.Received().Defocus();
						foreach(var bun in ssm.otherBundles)
							bun.Received().Defocus();
					}
					[Test]
					public void Deactivate_WhenCalled_SetsSelStateDeactivateed(){
						SlotSystemManager ssm = InitializedSSM();

						ssm.Deactivate();

						Assert.That(ssm.curSelState, Is.SameAs(SlotSystemManager.ssmDeactivatedState));
					}
					[Test]
					public void Deactivate_WhenCalled_CallsAllBundlesDeactivate(){
						SlotSystemManager ssm = InitializedSSM();

						ssm.Deactivate();

						ssm.poolBundle.Received().Deactivate();
						ssm.equipBundle.Received().Deactivate();
						foreach(var bun in ssm.otherBundles)
							bun.Received().Deactivate();
					}
					[TestCase(true, true, true, true, true)]
					[TestCase(false, false, false, false, false)]
					public void Deactivate_WhenCalled_SetsPEsToggleBackToDefault(bool pBunPEOn, bool eBunPEOn, bool gBunAPEOn, bool gBunBPEOn, bool gBunCPEOn){
						SlotSystemManager ssm = MakeSSM();
							ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
								pBunPE.isFocusedOnActivate.Returns(pBunPEOn);
							ISlotSystemPageElement eBunPE = MakeSubEquipBundlePEInitWithSGs();
								eBunPE.isFocusedOnActivate.Returns(eBunPEOn);
							IEnumerable<ISlotSystemPageElement> gBunPEs;
								ISlotSystemPageElement gBunAPE = MakeSubGenBundlePEInitWithSGs();
									ISlotSystemBundle gBunA = MakeSubBundle();
									gBunAPE.element.Returns(gBunA);
									gBunAPE.isFocusedOnActivate.Returns(gBunAPEOn);
								ISlotSystemPageElement gBunBPE = MakeSubGenBundlePEInitWithSGs();
									ISlotSystemBundle gBunB = MakeSubBundle();
									gBunBPE.element.Returns(gBunB);
									gBunBPE.isFocusedOnActivate.Returns(gBunBPEOn);
								ISlotSystemPageElement gBunCPE = MakeSubGenBundlePEInitWithSGs();
									ISlotSystemBundle gBunC = MakeSubBundle();
									gBunCPE.element.Returns(gBunC);
									gBunCPE.isFocusedOnActivate.Returns(gBunCPEOn);
								gBunPEs = new ISlotSystemPageElement[]{gBunAPE, gBunBPE, gBunCPE};
						ssm.Initialize(pBunPE, eBunPE, gBunPEs);

						ssm.Deactivate();

						pBunPE.Received().isFocusToggleOn = pBunPEOn;
						eBunPE.Received().isFocusToggleOn = eBunPEOn;
						gBunAPE.Received().isFocusToggleOn = gBunAPEOn;
						gBunBPE.Received().isFocusToggleOn = gBunBPEOn;
						gBunCPE.Received().isFocusToggleOn = gBunCPEOn;
					}
					[Test]
					[Ignore]
					public void FocusInBundle_sbeBShield_CallsElementsAccordingly(){
						/*	out fields	*/
							ISlotSystemBundle pBun;
								ISlotGroup sgpA;
									ISlottable sbpA0;
									ISlottable sbpA1;
									ISlottable sbpA2;
								ISlotGroup sgpB;
									ISlottable sbpB0;
									ISlottable sbpB1;
									ISlottable sbpB2;
								ISlotGroup sgpC;
									ISlottable sbpC0;
									ISlottable sbpC1;
									ISlottable sbpC2;
							ISlotSystemBundle eBun;
								IEquipmentSet eSetA;
									ISlotGroup sgeABow;
										ISlottable sbeABow;
									ISlotGroup sgeAWear;
										ISlottable sbeAWear;
									ISlotGroup sgeACGears;
										ISlottable sbeAShield;
										ISlottable sbeAMWeapon;
								IEquipmentSet eSetB;
									ISlotGroup sgeBBow;
										ISlottable sbeBBow;
									ISlotGroup sgeBWear;
										ISlottable sbeBWear;
									ISlotGroup sgeBCGears;
										ISlottable sbeBShield;
										ISlottable sbeBMWeapon;
							ISlotSystemBundle gBunA;
								ISlotSystemBundle gBunAA;
									ISlotGroup sggAA0;
									ISlotGroup sggAA1;
									ISlotGroup sggAA2;
								ISlotSystemBundle gBunAB;
									ISlotGroup sggAB0;
									ISlotGroup sggAB1;
									ISlotGroup sggAB2;
							ISlotSystemBundle gBunB;
								ISlotSystemPage pageA;
									ISlotGroup sggPA0;
									ISlotGroup sggPA1;
									ISlotGroup sggPA2;
								ISlotSystemPage pageB;
									ISlotGroup sggPB0;
									ISlotGroup sggPB1;
									ISlotGroup sggPB;
						SlotSystemManager ssm = MakeFullSSM(
							out pBun,
								out sgpA,
									out sbpA0,
									out sbpA1,
									out sbpA2,
								out sgpB,
									out sbpB0,
									out sbpB1,
									out sbpB2,
								out sgpC,
									out sbpC0,
									out sbpC1,
									out sbpC2,
							out eBun,
								out eSetA,
									out sgeABow,
										out sbeABow,
									out sgeAWear,
										out sbeAWear,
									out sgeACGears,
										out sbeAShield,
										out sbeAMWeapon,
								out eSetB,
									out sgeBBow,
										out sbeBBow,
									out sgeBWear,
										out sbeBWear,
									out sgeBCGears,
										out sbeBShield,
										out sbeBMWeapon,
							out gBunA,
								out gBunAA,
									out sggAA0,
									out sggAA1,
									out sggAA2,
								out gBunAB,
									out sggAB0,
									out sggAB1,
									out sggAB2,
							out gBunB,
								out pageA,
									out sggPA0,
									out sggPA1,
									out sggPA2,
								out pageB,
									out sggPB0,
									out sggPB1,
									out sggPB
						);

						ssm.FocusInBundle(sbeBShield, sbeBShield);

						Received.InOrder(() => {
							eBun.SetFocusedBundleElement(eSetB);
							eBun.ToggleOnPageElement();
						});				
					}
					[Test]
					[Ignore]
					public void FocusInBundle_sggPA2_CallsElementsAccordingly(){
						/*	out fields	*/
							ISlotSystemBundle pBun;
								ISlotGroup sgpA;
									ISlottable sbpA0;
									ISlottable sbpA1;
									ISlottable sbpA2;
								ISlotGroup sgpB;
									ISlottable sbpB0;
									ISlottable sbpB1;
									ISlottable sbpB2;
								ISlotGroup sgpC;
									ISlottable sbpC0;
									ISlottable sbpC1;
									ISlottable sbpC2;
							ISlotSystemBundle eBun;
								IEquipmentSet eSetA;
									ISlotGroup sgeABow;
										ISlottable sbeABow;
									ISlotGroup sgeAWear;
										ISlottable sbeAWear;
									ISlotGroup sgeACGears;
										ISlottable sbeAShield;
										ISlottable sbeAMWeapon;
								IEquipmentSet eSetB;
									ISlotGroup sgeBBow;
										ISlottable sbeBBow;
									ISlotGroup sgeBWear;
										ISlottable sbeBWear;
									ISlotGroup sgeBCGears;
										ISlottable sbeBShield;
										ISlottable sbeBMWeapon;
							ISlotSystemBundle gBunA;
								ISlotSystemBundle gBunAA;
									ISlotGroup sggAA0;
									ISlotGroup sggAA1;
									ISlotGroup sggAA2;
								ISlotSystemBundle gBunAB;
									ISlotGroup sggAB0;
									ISlotGroup sggAB1;
									ISlotGroup sggAB2;
							ISlotSystemBundle gBunB;
								ISlotSystemPage pageA;
									ISlotGroup sggPA0;
									ISlotGroup sggPA1;
									ISlotGroup sggPA2;
								ISlotSystemPage pageB;
									ISlotGroup sggPB0;
									ISlotGroup sggPB1;
									ISlotGroup sggPB;
						SlotSystemManager ssm = MakeFullSSM(
							out pBun,
								out sgpA,
									out sbpA0,
									out sbpA1,
									out sbpA2,
								out sgpB,
									out sbpB0,
									out sbpB1,
									out sbpB2,
								out sgpC,
									out sbpC0,
									out sbpC1,
									out sbpC2,
							out eBun,
								out eSetA,
									out sgeABow,
										out sbeABow,
									out sgeAWear,
										out sbeAWear,
									out sgeACGears,
										out sbeAShield,
										out sbeAMWeapon,
								out eSetB,
									out sgeBBow,
										out sbeBBow,
									out sgeBWear,
										out sbeBWear,
									out sgeBCGears,
										out sbeBShield,
										out sbeBMWeapon,
							out gBunA,
								out gBunAA,
									out sggAA0,
									out sggAA1,
									out sggAA2,
								out gBunAB,
									out sggAB0,
									out sggAB1,
									out sggAB2,
							out gBunB,
								out pageA,
									out sggPA0,
									out sggPA1,
									out sggPA2,
								out pageB,
									out sggPB0,
									out sggPB1,
									out sggPB
						);
						ssm.FocusInBundle(sggPA2, sggPA2);
						Received.InOrder(()=>{
							pageA.ContainsInHierarchy(sggPA2);
							gBunB.SetFocusedBundleElement(pageA);
							gBunB.ToggleOnPageElement();
						});
					}
					public SlotSystemManager MakeFullSSM(
						out ISlotSystemBundle pBun, 
							out ISlotGroup sgpA, 
								out ISlottable sbpA0,
								out ISlottable sbpA1,
								out ISlottable sbpA2,	
							out ISlotGroup sgpB,
								out ISlottable sbpB0,
								out ISlottable sbpB1,
								out ISlottable sbpB2,
							out ISlotGroup sgpC,
								out ISlottable sbpC0,
								out ISlottable sbpC1,
								out ISlottable sbpC2,
						out ISlotSystemBundle eBun,
							out IEquipmentSet eSetA,
								out ISlotGroup sgeABow,
									out ISlottable sbeABow,
								out ISlotGroup sgeAWear,
									out ISlottable sbeAWear,
								out ISlotGroup sgeACGears,
									out ISlottable sbeAShield,
									out ISlottable sbeAMWeapon,
							out IEquipmentSet eSetB,
								out ISlotGroup sgeBBow,
									out ISlottable sbeBBow,
								out ISlotGroup sgeBWear,
									out ISlottable sbeBWear,
								out ISlotGroup sgeBCGears,
									out ISlottable sbeBShield,
									out ISlottable sbeBMWeapon,
						out ISlotSystemBundle gBunA,
							out ISlotSystemBundle gBunAA,
								out ISlotGroup sggAA0,
								out ISlotGroup sggAA1,
								out ISlotGroup sggAA2,
							out ISlotSystemBundle gBunAB,
								out ISlotGroup sggAB0,
								out ISlotGroup sggAB1,
								out ISlotGroup sggAB2,
						out ISlotSystemBundle gBunB,
							out ISlotSystemPage pageA,
								out ISlotGroup sggPA0,
								out ISlotGroup sggPA1,
								out ISlotGroup sggPA2,
							out ISlotSystemPage pageB,
								out ISlotGroup sggPB0,
								out ISlotGroup sggPB1,
								out ISlotGroup sggPB2)
					{
						SlotSystemManager ssm = MakeSSM();
							ISlotSystemPageElement pBunPE = MakeSubPageElement();
								pBun = MakeSubBundle();
									IEnumerable<ISlotSystemElement> pBunEles;
										sgpA = MakeSubSG();
											IEnumerable<ISlotSystemElement> sgpAEles;
												sbpA0 = MakeSubSB();
												sbpA1 = MakeSubSB();
												sbpA2 = MakeSubSB();
												sgpAEles = new ISlotSystemElement[]{sbpA0, sbpA1, sbpA2};
											sgpA.GetEnumerator().Returns(sgpAEles.GetEnumerator());
										sgpB = MakeSubSG();
											IEnumerable<ISlotSystemElement> sgpBEles;
												sbpB0 = MakeSubSB();
												sbpB1 = MakeSubSB();
												sbpB2 = MakeSubSB();
												sgpBEles = new ISlotSystemElement[]{sbpB0, sbpB1, sbpB2};
											sgpB.GetEnumerator().Returns(sgpBEles.GetEnumerator());
										sgpC = MakeSubSG();
											IEnumerable<ISlotSystemElement> sgpCEles;
												sbpC0 = MakeSubSB();
												sbpC1 = MakeSubSB();
												sbpC2 = MakeSubSB();
												sgpCEles = new ISlotSystemElement[]{sbpC0, sbpC1, sbpC2};
											sgpC.GetEnumerator().Returns(sgpCEles.GetEnumerator());
										pBunEles = new ISlotSystemElement[]{sgpA, sgpB, sgpC};
										sgpA.immediateBundle.Returns(pBun);
											sgpA.ContainsInHierarchy(sbpA0).Returns(true);
											sgpA.ContainsInHierarchy(sbpA1).Returns(true);
											sgpA.ContainsInHierarchy(sbpA2).Returns(true);
												sbpA0.immediateBundle.Returns(pBun);
												sbpA1.immediateBundle.Returns(pBun);
												sbpA2.immediateBundle.Returns(pBun);
										sgpB.immediateBundle.Returns(pBun);
											sgpB.ContainsInHierarchy(sbpB0).Returns(true);
											sgpB.ContainsInHierarchy(sbpB1).Returns(true);
											sgpB.ContainsInHierarchy(sbpB2).Returns(true);
												sbpB0.immediateBundle.Returns(pBun);
												sbpB1.immediateBundle.Returns(pBun);
												sbpB2.immediateBundle.Returns(pBun);
										sgpC.immediateBundle.Returns(pBun);
											sgpC.ContainsInHierarchy(sbpC0).Returns(true);
											sgpC.ContainsInHierarchy(sbpC1).Returns(true);
											sgpC.ContainsInHierarchy(sbpC2).Returns(true);
												sbpC0.immediateBundle.Returns(pBun);
												sbpC1.immediateBundle.Returns(pBun);
												sbpC2.immediateBundle.Returns(pBun);
										sgpA.isBundleElement.Returns(true);
										sgpB.isBundleElement.Returns(true);
										sgpC.isBundleElement.Returns(true);
									pBun.GetEnumerator().Returns(pBunEles.GetEnumerator());
									pBun.immediateBundle.Returns((ISlotSystemBundle)null);
									pBun.ContainsInHierarchy(sgpA).Returns(true);
										pBun.ContainsInHierarchy(sbpA0).Returns(true);
										pBun.ContainsInHierarchy(sbpA1).Returns(true);
										pBun.ContainsInHierarchy(sbpA2).Returns(true);
									pBun.ContainsInHierarchy(sgpB).Returns(true);
										pBun.ContainsInHierarchy(sbpB0).Returns(true);
										pBun.ContainsInHierarchy(sbpB1).Returns(true);
										pBun.ContainsInHierarchy(sbpB2).Returns(true);
									pBun.ContainsInHierarchy(sgpC).Returns(true);
										pBun.ContainsInHierarchy(sbpC0).Returns(true);
										pBun.ContainsInHierarchy(sbpC1).Returns(true);
										pBun.ContainsInHierarchy(sbpC2).Returns(true);
									pBun.isPageElement.Returns(true);
									pBun.isToggledOn.Returns(false);
								pBunPE.element.Returns(pBun);
							ISlotSystemPageElement eBunPE = MakeSubPageElement();
								eBun = MakeSubBundle();
									IEnumerable<ISlotSystemElement> eBunEles;
										eSetA = MakeSubESet();
											IEnumerable<ISlotSystemElement> eSetAEles;
												sgeABow = MakeSubSG();
													IEnumerable<ISlotSystemElement> sgeABowEles;
														sbeABow = MakeSubSB();
														sgeABowEles = new ISlotSystemElement[]{sgeABow};
													sgeABow.GetEnumerator().Returns(sgeABowEles.GetEnumerator());
												sgeAWear = MakeSubSG();
													IEnumerable<ISlotSystemElement> sgeAWearEles;
														sbeAWear = MakeSubSB();
														sgeAWearEles = new ISlotSystemElement[]{sgeAWear};
													sgeAWear.GetEnumerator().Returns(sgeAWearEles.GetEnumerator());
												sgeACGears = MakeSubSG();
													IEnumerable<ISlotSystemElement> sgeACGearsEles;
														sbeAShield = MakeSubSB();
														sbeAMWeapon = MakeSubSB();
														sgeACGearsEles = new ISlotSystemElement[]{sbeAShield, sbeAMWeapon};
													sgeACGears.GetEnumerator().Returns(sgeACGearsEles.GetEnumerator());
												eSetAEles = new ISlotSystemElement[]{sgeABow, sgeAWear, sgeACGears};
											eSetA.GetEnumerator().Returns(eSetAEles.GetEnumerator());
										eSetB = MakeSubESet();
											IEnumerable<ISlotSystemElement> eSetBEles;
												sgeBBow = MakeSubSG();
													IEnumerable<ISlotSystemElement> sgeBBowEles;
														sbeBBow = MakeSubSB();
														sgeBBowEles = new ISlotSystemElement[]{sgeBBow};
													sgeBBow.GetEnumerator().Returns(sgeBBowEles.GetEnumerator());
												sgeBWear = MakeSubSG();
													IEnumerable<ISlotSystemElement> sgeBWearEles;
														sbeBWear = MakeSubSB();
														sgeBWearEles = new ISlotSystemElement[]{sgeBWear};
													sgeBWear.GetEnumerator().Returns(sgeBWearEles.GetEnumerator());
												sgeBCGears = MakeSubSG();
													IEnumerable<ISlotSystemElement> sgeBCGearsEles;
														sbeBShield = MakeSubSB();
														sbeBMWeapon = MakeSubSB();
														sgeBCGearsEles = new ISlotSystemElement[]{sbeBShield, sbeBMWeapon};
													sgeBCGears.GetEnumerator().Returns(sgeBCGearsEles.GetEnumerator());
												eSetBEles = new ISlotSystemElement[]{sgeBBow, sgeBWear, sgeBCGears};
											eSetB.GetEnumerator().Returns(eSetBEles.GetEnumerator());
										eBunEles = new ISlotSystemElement[]{eSetA, eSetB};
										eSetA.immediateBundle.Returns(eBun);
										eSetA.isBundleElement.Returns(true);
											eSetA.ContainsInHierarchy(sgeABow).Returns(true);
												eSetA.ContainsInHierarchy(sbeABow).Returns(true);
											eSetA.ContainsInHierarchy(sgeAWear).Returns(true);
												eSetA.ContainsInHierarchy(sbeAWear).Returns(true);
											eSetA.ContainsInHierarchy(sgeACGears).Returns(true);
												eSetA.ContainsInHierarchy(sbeAShield).Returns(true);
												eSetA.ContainsInHierarchy(sbeAMWeapon).Returns(true);
											sgeABow.immediateBundle.Returns(eBun);
											sgeABow.ContainsInHierarchy(sgeABow).Returns(true);
												sbeABow.immediateBundle.Returns(eBun);
											sgeAWear.immediateBundle.Returns(eBun);
											sgeAWear.ContainsInHierarchy(sbeAWear).Returns(true);
												sbeAWear.immediateBundle.Returns(eBun);
											sgeACGears.immediateBundle.Returns(eBun);
											sgeACGears.ContainsInHierarchy(sbeAShield).Returns(true);
											sgeACGears.ContainsInHierarchy(sbeAMWeapon).Returns(true);
												sbeAShield.immediateBundle.Returns(eBun);
												sbeAMWeapon.immediateBundle.Returns(eBun);
											sgeABow.isPageElement.Returns(true);
											sgeAWear.isPageElement.Returns(true);
											sgeACGears.isPageElement.Returns(true);
											sgeABow.isToggledOn.Returns(false);
											sgeAWear.isToggledOn.Returns(false);
											sgeACGears.isToggledOn.Returns(false);
										eSetB.immediateBundle.Returns(eBun);
										eSetB.isBundleElement.Returns(true);
											eSetB.ContainsInHierarchy(sgeBBow).Returns(true);
												eSetB.ContainsInHierarchy(sbeBBow).Returns(true);
											eSetB.ContainsInHierarchy(sgeBWear).Returns(true);
												eSetB.ContainsInHierarchy(sbeBWear).Returns(true);
											eSetB.ContainsInHierarchy(sgeBCGears).Returns(true);
												eSetB.ContainsInHierarchy(sbeBShield).Returns(true);
												eSetB.ContainsInHierarchy(sbeBMWeapon).Returns(true);
											sgeBBow.immediateBundle.Returns(eBun);
											sgeBBow.ContainsInHierarchy(sbeBBow).Returns(true);
												sbeBBow.immediateBundle.Returns(eBun);
											sgeBWear.immediateBundle.Returns(eBun);
											sgeBWear.ContainsInHierarchy(sbeBWear).Returns(true);
												sbeBWear.immediateBundle.Returns(eBun);
											sgeBCGears.immediateBundle.Returns(eBun);
											sgeBCGears.ContainsInHierarchy(sbeBShield).Returns(true);
											sgeBCGears.ContainsInHierarchy(sbeBMWeapon).Returns(true);
												sbeBShield.immediateBundle.Returns(eBun);
												sbeBMWeapon.immediateBundle.Returns(eBun);
											sgeBBow.isPageElement.Returns(true);
											sgeBWear.isPageElement.Returns(true);
											sgeBCGears.isPageElement.Returns(true);
											sgeBBow.isToggledOn.Returns(false);
											sgeBWear.isToggledOn.Returns(false);
											sgeBCGears.isToggledOn.Returns(false);
									eBun.GetEnumerator().Returns(eBunEles.GetEnumerator());
									eBun.immediateBundle.Returns((ISlotSystemBundle)null);
									eBun.ContainsInHierarchy(eSetA).Returns(true);
										eBun.ContainsInHierarchy(sgeABow).Returns(true);
											eBun.ContainsInHierarchy(sbeABow).Returns(true);
										eBun.ContainsInHierarchy(sgeAWear).Returns(true);
											eBun.ContainsInHierarchy(sbeAWear).Returns(true);
										eBun.ContainsInHierarchy(sgeACGears).Returns(true);
											eBun.ContainsInHierarchy(sbeAShield).Returns(true);
											eBun.ContainsInHierarchy(sbeAMWeapon).Returns(true);
									eBun.ContainsInHierarchy(eSetB).Returns(true);
										eBun.ContainsInHierarchy(sgeBBow).Returns(true);
											eBun.ContainsInHierarchy(sbeBBow).Returns(true);
										eBun.ContainsInHierarchy(sgeBWear).Returns(true);
											eBun.ContainsInHierarchy(sbeBWear).Returns(true);
										eBun.ContainsInHierarchy(sgeBCGears).Returns(true);
											eBun.ContainsInHierarchy(sbeBShield).Returns(true);
											eBun.ContainsInHierarchy(sbeBMWeapon).Returns(true);
									eBun.isPageElement.Returns(true);
									eBun.isToggledOn.Returns(false);
								eBunPE.element.Returns(eBun);
							IEnumerable<ISlotSystemPageElement> gBunPEs;
								ISlotSystemPageElement gBunAPE = MakeSubPageElement();
									gBunA = MakeSubBundle();
										IEnumerable<ISlotSystemElement> gBunAEles;
											gBunAA = MakeSubBundle();
												IEnumerable<ISlotSystemElement> gBunAAEles;
													sggAA0 = MakeSubSG();
													sggAA1 = MakeSubSG();
													sggAA2 = MakeSubSG();
													gBunAAEles = new ISlotSystemElement[]{sggAA0, sggAA1, sggAA2};
													sggAA0.immediateBundle.Returns(gBunAA);
													sggAA1.immediateBundle.Returns(gBunAA);
													sggAA2.immediateBundle.Returns(gBunAA);
												gBunAA.GetEnumerator().Returns(gBunAAEles.GetEnumerator());
												gBunAA.immediateBundle.Returns(gBunA);
												gBunAA.ContainsInHierarchy(sggAA0).Returns(true);
												gBunAA.ContainsInHierarchy(sggAA1).Returns(true);
												gBunAA.ContainsInHierarchy(sggAA2).Returns(true);
												sggAA0.isBundleElement.Returns(true);
												sggAA1.isBundleElement.Returns(true);
												sggAA2.isBundleElement.Returns(true);
											gBunAA.isBundleElement.Returns(true);
											gBunAB = MakeSubBundle();
												IEnumerable<ISlotSystemElement> gBunABEles;
													sggAB0 = MakeSubSG();
													sggAB1 = MakeSubSG();
													sggAB2 = MakeSubSG();
													gBunABEles = new ISlotSystemElement[]{sggAB0, sggAB1, sggAB2};
													sggAB0.immediateBundle.Returns(gBunAB);
													sggAB1.immediateBundle.Returns(gBunAB);
													sggAB2.immediateBundle.Returns(gBunAB);
												gBunAB.GetEnumerator().Returns(gBunABEles.GetEnumerator());
												gBunAB.immediateBundle.Returns(gBunA);
												gBunAB.ContainsInHierarchy(sggAB0).Returns(true);
												gBunAB.ContainsInHierarchy(sggAB1).Returns(true);
												gBunAB.ContainsInHierarchy(sggAB2).Returns(true);
												sggAB0.isBundleElement.Returns(true);
												sggAB1.isBundleElement.Returns(true);
												sggAB2.isBundleElement.Returns(true);
											gBunAB.isBundleElement.Returns(true);
											gBunAEles = new ISlotSystemElement[]{gBunAA, gBunAB};
										gBunA.GetEnumerator().Returns(gBunAEles.GetEnumerator());
										gBunA.immediateBundle.Returns((ISlotSystemBundle)null);
										gBunA.ContainsInHierarchy(gBunAA).Returns(true);
											gBunA.ContainsInHierarchy(sggAA0).Returns(true);
											gBunA.ContainsInHierarchy(sggAA1).Returns(true);
											gBunA.ContainsInHierarchy(sggAA2).Returns(true);
										gBunA.ContainsInHierarchy(gBunAB).Returns(true);
											gBunA.ContainsInHierarchy(sggAB0).Returns(true);
											gBunA.ContainsInHierarchy(sggAB1).Returns(true);
											gBunA.ContainsInHierarchy(sggAB2).Returns(true);
										gBunA.isPageElement.Returns(true);
										gBunA.isToggledOn.Returns(false);
									gBunAPE.element.Returns(gBunA);
								ISlotSystemPageElement gBunBPE = MakeSubPageElement();
									gBunB = MakeSubBundle();
										IEnumerable<ISlotSystemElement> gBunBEles;
											pageA = MakeSubSSPage();
												IEnumerable<ISlotSystemElement> pageAEles;
													sggPA0 = MakeSubSG();
													sggPA1 = MakeSubSG();
													sggPA2 = MakeSubSG();
													pageAEles = new ISlotSystemElement[]{sggPA0, sggPA1, sggPA2};
												pageA.GetEnumerator().Returns(pageAEles.GetEnumerator());
												pageA.ContainsInHierarchy(sggPA0).Returns(true);
												pageA.ContainsInHierarchy(sggPA1).Returns(true);
												pageA.ContainsInHierarchy(sggPA2).Returns(true);
												sggPA0.isPageElement.Returns(true);
												sggPA1.isPageElement.Returns(true);
												sggPA2.isPageElement.Returns(true);
												sggPA0.isToggledOn.Returns(false);
												sggPA1.isToggledOn.Returns(false);
												sggPA2.isToggledOn.Returns(false);
											pageA.isBundleElement.Returns(true);
											pageB = MakeSubSSPage();
												IEnumerable<ISlotSystemElement> pageBEles;
													sggPB0 = MakeSubSG();
													sggPB1 = MakeSubSG();
													sggPB2 = MakeSubSG();
													pageBEles = new ISlotSystemElement[]{sggPB0, sggPB1, sggPB2};
												pageB.GetEnumerator().Returns(pageBEles.GetEnumerator());
												pageB.ContainsInHierarchy(sggPB0).Returns(true);
												pageB.ContainsInHierarchy(sggPB1).Returns(true);
												pageB.ContainsInHierarchy(sggPB2).Returns(true);
												sggPB0.isPageElement.Returns(true);
												sggPB1.isPageElement.Returns(true);
												sggPB2.isPageElement.Returns(true);
												sggPB0.isToggledOn.Returns(false);
												sggPB1.isToggledOn.Returns(false);
												sggPB2.isToggledOn.Returns(false);
												pageA.immediateBundle.Returns(gBunB);
												sggPA0.immediateBundle.Returns(gBunB);
												sggPA1.immediateBundle.Returns(gBunB);
												sggPA2.immediateBundle.Returns(gBunB);
												pageB.immediateBundle.Returns(gBunB);
												sggPB0.immediateBundle.Returns(gBunB);
												sggPB1.immediateBundle.Returns(gBunB);
												sggPB2.immediateBundle.Returns(gBunB);
											pageB.isBundleElement.Returns(true);
											gBunBEles = new ISlotSystemElement[]{pageA, pageB};
										gBunB.GetEnumerator().Returns(gBunBEles.GetEnumerator());
										gBunB.ContainsInHierarchy(pageA).Returns(true);
											gBunB.ContainsInHierarchy(sggPA0).Returns(true);
											gBunB.ContainsInHierarchy(sggPA1).Returns(true);
											gBunB.ContainsInHierarchy(sggPA2).Returns(true);
										gBunB.ContainsInHierarchy(pageB).Returns(true);
											gBunB.ContainsInHierarchy(sggPB0).Returns(true);
											gBunB.ContainsInHierarchy(sggPB1).Returns(true);
											gBunB.ContainsInHierarchy(sggPB2).Returns(true);
										gBunB.immediateBundle.Returns((ISlotSystemBundle)null);
										gBunB.isPageElement.Returns(true);
										gBunB.isToggledOn.Returns(false);
									gBunBPE.element.Returns(gBunB);
								gBunPEs = new ISlotSystemPageElement[]{gBunAPE, gBunBPE};
							ssm.Initialize(pBunPE, eBunPE, gBunPEs);
						return ssm;
					}
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
					public void AcceptsSGTAComp_ValidSG_SetsDoneAndCallCheck(){
						SlotSystemManager ssm = MakeSSM();
					}
			/*	helper	*/
				public ISlotGroup MakeSGEBow(){
					ISlotGroup sgeBow = MakeSubSGWithEmptySBs();
						List<Slot> slots;
							Slot slot = new Slot();
								ISlottable sbBow = MakeSubSB();
									BowInstance bow = MakeBowInstance(0);
									sbBow.itemInst.Returns(bow);
								slot.sb = sbBow;
							slots = new List<Slot>(new Slot[]{slot});
						sgeBow.slots.Returns(slots);
						sgeBow.Filter.Returns(new SGBowFilter());
					return sgeBow;
				}
				public ISlotGroup MakeSGEWear(){
					ISlotGroup sgeWear = MakeSubSGWithEmptySBs();
						List<Slot> slots;
							Slot slot = new Slot();
								ISlottable sbWear = MakeSubSB();
									WearInstance wear = MakeWearInstance(0);
									sbWear.itemInst.Returns(wear);
								slot.sb = sbWear;
							slots = new List<Slot>(new Slot[]{slot});
						sgeWear.slots.Returns(slots);
						sgeWear.Filter.Returns(new SGWearFilter());
					return sgeWear;
				}
				public ISlotGroup MakeSGECGears(){
					ISlotGroup sgeCGears = MakeSubSGWithEmptySBs();
						IEnumerable<ISlotSystemElement> sgeCGearsEles;
							ISlottable sbShield = MakeSubSB();
								ShieldInstance shield = MakeShieldInstance(0);
								sbShield.itemInst.Returns(shield);
							ISlottable sbMWeapon = MakeSubSB();
								MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
								sbMWeapon.itemInst.Returns(mWeapon);
						sgeCGearsEles = new ISlotSystemElement[]{
							sbShield, sbMWeapon
						};
						sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
						sgeCGears.Filter.Returns(new SGCGearsFilter());
					return sgeCGears;
				}
				public SlotSystemManager MakeSSMInitWithPEs(){
					ISlotSystemPageElement poolBundlePE = MakeSubPoolBundlePEInitWithSGs();
					ISlotSystemPageElement equipBundlePE = MakeSubEquipBundlePEInitWithSGs();
					IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeSubGenBundlePEsInitWithSGs();
					SlotSystemManager ssm = MakeSSM();
					ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);
					return ssm;
				}
				public SlotSystemManager InitializedSSM(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
						ISlotSystemPageElement eBunpE = MakeSubEquipBundlePEInitWithSGs();
						IEnumerable<ISlotSystemPageElement> gBunPEs = MakeSubGenBundlePEsInitWithSGs();
					ssm.Initialize(pBunPE, eBunpE, gBunPEs);
					return ssm;
				}
				public ISlotSystemPageElement MakeSubPoolBundlePEInitWithSGs(){
					ISlotSystemBundle poolBundle = MakeSubBundle();
					ISlotSystemPageElement poolBundlePE = MakeSubPageElement();
					poolBundlePE.element.Returns(poolBundle);
					ISlotGroup sgpA = MakeSubSG();
						sgpA.slottables.Returns(new List<ISlottable>());
					ISlotGroup sgpB = MakeSubSG();
						sgpB.slottables.Returns(new List<ISlottable>());
					ISlotGroup sgpC = MakeSubSG();
						sgpC.slottables.Returns(new List<ISlottable>());
					IEnumerable<ISlotSystemElement> sgs = new ISlotSystemElement[]{
						sgpA, sgpB, sgpC
					};
					poolBundle.elements.Returns(sgs);
					return poolBundlePE;
				}
				public ISlotSystemPageElement MakeSubEquipBundlePEInitWithSGs(){
					ISlotSystemBundle equipBundle = MakeSubBundle();
					ISlotSystemPageElement equipBundlePE = MakeSubPageElement();
					equipBundlePE.element.Returns(equipBundle);
					IEquipmentSet eSetA = MakeSubEquipmentSetInitWithSGs();
					IEquipmentSet eSetB = MakeSubEquipmentSetInitWithSGs();
					IEquipmentSet eSetC = MakeSubEquipmentSetInitWithSGs();
					IEnumerable<ISlotSystemElement> eSets = new ISlotSystemElement[]{
						eSetA, eSetB, eSetC
					};
					equipBundle.elements.Returns(eSets);
					return equipBundlePE;
				}
				public ISlotSystemPageElement MakeSubGenBundlePEInitWithSGs(){
					ISlotSystemBundle genBun = MakeSubBundle();
						ISlotGroup sgg = MakeSubSGWithEmptySBs();
						ISlotGroup sgg1 = MakeSubSGWithEmptySBs();
						ISlotGroup sgg2 = MakeSubSGWithEmptySBs();
						IEnumerable<ISlotSystemElement> gBunEles = new ISlotSystemElement[]{
							sgg, sgg1, sgg2
						};
					genBun.elements.Returns(gBunEles);
					ISlotSystemPageElement genBunPE = MakeSubPageElement();
					genBunPE.element.Returns(genBun);
					return genBunPE;
				}
				public IEnumerable<ISlotSystemPageElement> MakeSubGenBundlePEsInitWithSGs(){
					ISlotSystemPageElement genBunAPE = MakeSubGenBundlePEInitWithSGs();
					ISlotSystemPageElement genBunBPE = MakeSubGenBundlePEInitWithSGs();
					ISlotSystemPageElement genBunCPE = MakeSubGenBundlePEInitWithSGs();
					IEnumerable<ISlotSystemPageElement> genBunPEs = new ISlotSystemPageElement[]{
						genBunAPE, genBunBPE, genBunCPE
					};
					return genBunPEs;
				}
				IEnumerable<ISlotSystemElement> GetAllElementsInPE(ISlotSystemPageElement pe){
					foreach(var ele in ((ISlotSystemBundle)pe.element).elements){
						yield return (ISlotSystemElement)ele;
					}
				}
				IEnumerable<ISlotSystemElement> GetAllSGsInPE(ISlotSystemPageElement pe){
					foreach(var ele in ((ISlotSystemBundle)pe.element).elements){
						if(ele is ISlotGroup)
							yield return ele;
						else if(ele is ISlotSystemPage)
							foreach(var e in ele)
								yield return e;
					}
				}
				public IEnumerable<ISlotSystemElement> GetAllElementsInPEs(IEnumerable<ISlotSystemPageElement> pes){
					foreach(var pe in pes){
						ISlotSystemBundle bundle = (ISlotSystemBundle)pe.element;
						foreach(var e in bundle.elements){
							yield return e;
						}
					}
				}
				public ISlotSystemPageElement MakeESetPEWInitWithSGs(){
					ISlotSystemPageElement eSetPE = MakeSubPageElement();
					IEquipmentSet eSet = MakeSubEquipmentSetInitWithSGs();
					eSetPE.element.Returns(eSet);
					return eSetPE;
				}
				public ISlotSystemPageElement MakePBunWithPoolInv(out IPoolInventory pInv){
					ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
					ISlotGroup focusedSGP = MakeSubSGWithEmptySBs();
					((ISlotSystemBundle)pBunPE.element).focusedElement.Returns(focusedSGP);
					pInv = MakeSubPoolInv();
					focusedSGP.inventory.Returns(pInv);
					IEnumerable<SlottableItem> pInvEles = new SlottableItem[]{};
					pInv.GetEnumerator().Returns(pInvEles.GetEnumerator());
					return pBunPE;
				}
				public ISlotSystemPageElement MakeSubEBunPEWithEquipInv(out IEquipmentSetInventory eInv){
					ISlotSystemPageElement eBunPE = MakeSubPageElement();
						ISlotSystemBundle eBun = MakeSubBundle();
						eBunPE.element.Returns(eBun);
							IEquipmentSet eSet = MakeSubEquipmentSetInitWithSGs();
							eBun.isToggledOn.Returns(true);
							eBun.focusedElement.Returns(eSet);
								IEnumerable<ISlotSystemElement> eSetEles;
									ISlotGroup sgeA = MakeSubSGWithEmptySBs();
										eInv = MakeSubEquipInv();
										sgeA.inventory.Returns(eInv);
									ISlotGroup sgeB = MakeSubSGWithEmptySBs();
									ISlotGroup sgeC = MakeSubSGWithEmptySBs();
									eSetEles = new ISlotSystemElement[]{
										sgeA, sgeB, sgeC
									};
								eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
						return eBunPE;
				}
				public ISlotSystemPageElement MakeEBunPEWithSGEs(out ISlotGroup sgeBow, out ISlotGroup sgeWear, out ISlotGroup sgeCGears){
					ISlotSystemPageElement eBunPE = MakeSubPageElement();
						ISlotSystemBundle eBun = MakeSubBundle();
						eBun.isToggledOn.Returns(true);
							IEnumerable<ISlotSystemElement> eBunEles;
								IEquipmentSet eSet = MakeSubEquipmentSetInitWithSGs();
									IEnumerable<ISlotSystemElement> eSetEles;
										sgeBow = MakeSGEBow();
										sgeWear = MakeSGEWear();
										sgeCGears = MakeSGECGears();
										eSetEles = new ISlotSystemElement[]{
											sgeBow, sgeWear, sgeCGears
										};
									eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
								eBunEles = new ISlotSystemElement[]{
									eSet
								};
							eBun.GetEnumerator().Returns(eBunEles.GetEnumerator());
							eBun.focusedElement.Returns(eSet);
						eBunPE.element.Returns(eBun);
						return eBunPE;
				}
				public ISlotSystemPageElement MakeEBunPEWithSGEsAndEquipInv(out ISlotGroup sgeBow, out ISlotGroup sgeWear, out ISlotGroup sgeCGears, out IEquipmentSetInventory eInv){
					ISlotSystemPageElement eBunPE = MakeSubPageElement();
						ISlotSystemBundle eBun = MakeSubBundle();
						eBun.isToggledOn.Returns(true);
							IEnumerable<ISlotSystemElement> eBunEles;
								IEquipmentSet eSet = MakeSubEquipmentSetInitWithSGs();
									IEnumerable<ISlotSystemElement> eSetEles;
										sgeBow = MakeSGEBow();
										sgeWear = MakeSGEWear();
										sgeCGears = MakeSGECGears();
										eSetEles = new ISlotSystemElement[]{
											sgeBow, sgeWear, sgeCGears
										};
										eInv = MakeSubEquipInv();
										sgeBow.inventory.Returns(eInv);
									eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
								eBunEles = new ISlotSystemElement[]{
									eSet
								};
							eBun.GetEnumerator().Returns(eBunEles.GetEnumerator());
							eBun.focusedElement.Returns(eSet);
						eBunPE.element.Returns(eBun);
						return eBunPE;
				}
		}
	}
}
