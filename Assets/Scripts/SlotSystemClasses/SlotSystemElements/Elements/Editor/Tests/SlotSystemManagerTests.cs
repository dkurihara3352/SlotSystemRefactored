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
						ISlotSystemManager stubSSM = MakeSubSSM();
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
							public SlotSystemTransaction transaction;
							public ResetTestCase(ISlottable pickedSB, ISlottable targetSB, ISlotGroup sg1, ISlotGroup sg2, ISlotSystemElement hovered, DraggedIcon dIcon1, DraggedIcon dIcon2, SlotSystemTransaction transaction){
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
						ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
							ISlotGroup focusedSGP = MakeSubSGWithEmptySBs();
							((ISlotSystemBundle)pBunPE.element).focusedElement.Returns(focusedSGP);
							IPoolInventory pInv = MakeSubPoolInv();
							focusedSGP.inventory.Returns(pInv);
							IEnumerable<SlottableItem> pInvEles = new SlottableItem[]{};
							pInv.GetEnumerator().Returns(pInvEles.GetEnumerator());
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
						ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
							ISlotGroup focusedSGP = MakeSubSGWithEmptySBs();
							((ISlotSystemBundle)pBunPE.element).focusedElement.Returns(focusedSGP);
							IPoolInventory pInv = MakeSubPoolInv();
							focusedSGP.inventory.Returns(pInv);
							IEnumerable<SlottableItem> pInvEles = new SlottableItem[]{};
							pInv.GetEnumerator().Returns(pInvEles.GetEnumerator());
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
						ISlotSystemPageElement pBunPE = MakeSubPoolBundlePEInitWithSGs();
							ISlotGroup focusedSGP = MakeSubSGWithEmptySBs();
							((ISlotSystemBundle)pBunPE.element).focusedElement.Returns(focusedSGP);
							IPoolInventory pInv = MakeSubPoolInv();
							focusedSGP.inventory.Returns(pInv);
							// IEnumerable<SlottableItem> pInvEles = new SlottableItem[]{};
							// pInv.GetEnumerator().Returns(pInvEles.GetEnumerator());
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
					// [Test] untestable for now...
					public void SortSG_WhenCalled_SetsFields(){
						SlotSystemManager ssm = MakeSSM();
						ISlotGroup sg = MakeSubSG();
						SGSorter sorter = Substitute.For<SGSorter>();

						ssm.SortSG(sg, sorter);

						Assert.That(ssm.targetSB, Is.Not.Null);
						Assert.That(ssm.sg1, Is.Not.SameAs(sg));
						Assert.That(ssm.transaction, Is.Not.TypeOf(typeof(SortTransaction)));
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
		}
	}
}
