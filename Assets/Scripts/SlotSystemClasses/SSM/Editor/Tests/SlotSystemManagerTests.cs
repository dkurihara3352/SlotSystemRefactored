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
			[Test][Category("Methods")]
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
				Assert.That(ssm.otherBundles, Is.EqualTo(gBuns));
			}
			[Test][Category("Methods")]
			public void SetElements_AfterInspectorSetUp_SetsPEs(){
				SlotSystemManager ssm = MakeSSM();
				ISlotSystemBundle pBun = MakeSubBundle();
					ISlotSystemBundle eBun = MakeSubBundle();
					IEnumerable<ISlotSystemBundle> gBuns;
						ISlotSystemBundle gBunA = MakeSubBundle();
						ISlotSystemBundle gBunB = MakeSubBundle();
						ISlotSystemBundle gBunC = MakeSubBundle();
						gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
				ssm.InspectorSetUp(pBun, eBun, gBuns);

				ssm.SetElements();

				ISlotSystemPageElement pBunPE = ssm.GetPageElement(pBun);
				ISlotSystemPageElement eBunPE = ssm.GetPageElement(eBun);
				List<ISlotSystemPageElement> gBunPEs = new List<ISlotSystemPageElement>();
				foreach(var gBun in gBuns)
					gBunPEs.Add(ssm.GetPageElement(gBun));
				Assert.That(pBunPE.element, Is.SameAs(pBun));
				Assert.That(eBunPE.element, Is.SameAs(eBun));
				Assert.That(gBunPEs.Count, Is.EqualTo(3));
				List<ISlotSystemElement> gBunsList = new List<ISlotSystemElement>();
					foreach(var gBunPE in gBunPEs)
						gBunsList.Add(gBunPE.element);
				Assert.That(gBunsList, Is.EqualTo(gBuns));
			}
			[Test][Category("Methods")]
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
				ssm.SetElements();

				ssm.Initialize();
				
				Assert.That(ssm.ssm, Is.SameAs(ssm));
				pBun.Received().PerformInHierarchy(ssm.SetSSMInH);
				eBun.Received().PerformInHierarchy(ssm.SetSSMInH);
				foreach(var gBun in gBuns)
					gBun.Received().PerformInHierarchy(ssm.SetSSMInH);
			}
			[Test][Category("Methods")]
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
				ssm.SetElements();

				ssm.Initialize();
				
				Assert.That(ssm.parent, Is.Null);
				pBun.Received().PerformInHierarchy(ssm.SetParent);
				eBun.Received().PerformInHierarchy(ssm.SetParent);
				foreach(var gBun in gBuns)
					gBun.Received().PerformInHierarchy(ssm.SetParent);
			}
			[Test][Category("Methods")]
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
				ssm.SetElements();

				ssm.Initialize();

				Assert.That(ssm.curSelState, Is.SameAs(AbsSlotSystemElement.deactivatedState));
				Assert.That(ssm.prevSelState, Is.Null);
				Assert.That(ssm.curActState, Is.SameAs(SlotSystemManager.ssmWaitForActionState));
				Assert.That(ssm.prevActState, Is.Null);
				pBun.Received().PerformInHierarchy(ssm.InitStatesInHi);
				eBun.Received().PerformInHierarchy(ssm.InitStatesInHi);
				foreach(var gBun in gBuns)
					gBun.Received().PerformInHierarchy(ssm.InitStatesInHi);
			}
			/*	fields */
				[Test][Category("Fields")]
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
					ssm.SetElements();

					List<ISlotGroup> list = ssm.allSGs;
					Received.InOrder(() => {
						pBun.PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
						eBun.PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
						foreach(var gBun in gBuns){
							gBun.PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
						}
					});
				}
				[Test][Category("Fields")]
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
				[Test][Category("Fields")]
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
					ssm.SetElements();

					List<ISlotGroup> list = ssm.allSGPs;
					pBun.Received().PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
				}
				[Test][Category("Fields")]
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
					ssm.SetElements();

					List<ISlotGroup> list = ssm.allSGEs;
					eBun.Received().PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
				}
				[Test][Category("Fields")]
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
					ssm.SetElements();

					List<ISlotGroup> list = ssm.allSGGs;

					foreach(var gBun in gBuns){
						gBun.Received().PerformInHierarchy(ssm.AddInSGList, Arg.Any<List<ISlotGroup>>());
					}
				}
				[Test][Category("Fields")]
				public void FocusedSGP_PoolBundleToggledOn_ReturnsPoolBundleFocusedElement(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
							ISlotGroup focusedSG = MakeSubSGWithEmptySBs();
								pBun.isToggledOn.Returns(true);
								pBun.focusedElement.Returns(focusedSG);
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();
					
					ISlotGroup actual = ssm.focusedSGP;

					Assert.That(actual, Is.SameAs(focusedSG));
				}
				[Test][Category("Fields")]
				public void FocusedEpSet_EquipBundleIsToggledOn_ReturnsEquipBundleFocusedElement(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
							IEquipmentSet focusedESet = MakeSubEquipmentSetInitWithSGs();
								eBun.isToggledOn.Returns(true);
								eBun.focusedElement.Returns(focusedESet);
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();

					IEquipmentSet actual = ssm.focusedEqSet;

					Assert.That(actual, Is.SameAs(focusedESet));
				}
				[Test][Category("Fields")]
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
							eBun.isToggledOn.Returns(true);
							eBun.focusedElement.Returns(focusedESet);
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();

					IEnumerable<ISlotGroup> actual = ssm.focusedSGEs;
					Assert.That(actual, Is.EqualTo(focusedESetEles));
				}
				[TestCaseSource(typeof(AddFocusedTo_VariousConfigCases))][Category("Fields")]
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
				[Test][Category("Fields")]
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
					ssm.SetElements();
					
					List<ISlotGroup> list = ssm.focusedSGGs;
					
					foreach(var gBun in gBuns)
						gBun.Received().PerformInHierarchy(ssm.AddFocusedSGTo, Arg.Any<List<ISlotGroup>>());
				}
				[TestCaseSource(typeof(FocusedSGsCases))][Category("Fields")]
				public void FocusedSGs_Always_ReturnsAllTheFocusedSGsInPBunAndEBunAndCallAllGBunPIHAddFocusedSGTo(ISlotSystemBundle pBun, ISlotSystemBundle eBun, IEnumerable<ISlotSystemBundle> gBuns, List<ISlotGroup> expected){
					SlotSystemManager ssm = MakeSSM();
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();
					ssm.Focus();

					List<ISlotGroup> actual = ssm.focusedSGs;

					Assert.That(actual, Is.EqualTo(expected));
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
								pBun.isToggledOn.Returns(true);
								pBun.focusedElement.Returns(sgpA);
							ISlotSystemBundle eBun = MakeSubBundle();
								eBun.isToggledOn.Returns(true);
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
								sgpB, sgBowC, sgWearC, sgCGearsC
							});
							yield return new object[]{
								pBun, eBun, gBuns, case1Exp
							};
						}
					}
				[Test][Category("Fields")]
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

					Assert.That(actual, Is.EqualTo(list));
				}
				[Test][Category("Fields")]
				public void EquipmentSets_Always_ReturnsEquipBundleElements(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
							IEnumerable<ISlotSystemElement> eBunEles;
								IEquipmentSet eSetA = AbsSlotSystemTest.MakeSubEquipmentSetInitWithSGs();
								IEquipmentSet eSetB = AbsSlotSystemTest.MakeSubEquipmentSetInitWithSGs();
								IEquipmentSet eSetC = AbsSlotSystemTest.MakeSubEquipmentSetInitWithSGs();						
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
					ssm.SetElements();

					List<IEquipmentSet> actual = ssm.equipmentSets;

					Assert.That(actual, Is.EqualTo(eBunEles));
				}
				[Test][Category("Fields")]
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
							pBun.isToggledOn.Returns(true);
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();

					IPoolInventory actual = ssm.poolInv;

					Assert.That(actual, Is.SameAs(pInv));
				}
				[Test][Category("Fields")]
				public void equipInv_Always_ReturnsFocusedSGEsInventory(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						IEquipmentSetInventory eInv;
						ISlotSystemPageElement eBunPE = MakeSubEBunPEWithEquipInv(out eInv);
						ISlotSystemBundle eBun = (ISlotSystemBundle)eBunPE.element;
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();
					
					IEquipmentSetInventory actual = ssm.equipInv;

					Assert.That(actual, Is.SameAs(eInv));
				}
				[Test][Category("Fields")]
				public void equippedBowInst_Always_ReturnsFocusedSGEWithBowFilterFirtSlotSBItemInst(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
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
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();

					BowInstance actual = ssm.equippedBowInst;

					Assert.That(actual, Is.SameAs(expected));
				}
				[Test][Category("Fields")]
				public void equippedWearInst_Always_ReturnsFocusedSGEWithWearFilterFirtSlotSBItemInst(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
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
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();

					WearInstance actual = ssm.equippedWearInst;

					Assert.That(actual, Is.SameAs(expected));
				}
				[Test][Category("Fields")]
				public void equippedCarriedGears_Always_ReturnsFocusedSGEWithCGFilterAllElements(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
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
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();

					List<CarriedGearInstance> actual = ssm.equippedCarriedGears;

					Assert.That(actual, Is.EqualTo(expected));
				}
				[Test][Category("Fields")]
				public void allEquippedItems_Always_ReturnsSumOfAllThree(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun;
							ISlotGroup sgeBow;
							ISlotGroup sgeWear;
							ISlotGroup sgeCGears;
							ISlotSystemPageElement eBunPE = MakeEBunPEWithSGEs(out sgeBow, out sgeWear, out sgeCGears);
							List<InventoryItemInstance> expected = new List<InventoryItemInstance>();
								expected.Add(sgeBow.slots[0].sb.itemInst);
								expected.Add(sgeWear.slots[0].sb.itemInst);
								foreach(var ele in sgeCGears)
									expected.Add((CarriedGearInstance)((ISlottable)ele).itemInst);
							eBun = (ISlotSystemBundle)eBunPE.element;
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();

					List<InventoryItemInstance> actual = ssm.allEquippedItems;

					Assert.That(actual, Is.EqualTo(expected));
				}
				[Test][Category("Fields")]
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
				[Test][Category("Fields")]
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
					ssm.SetElements();
					
					List<ISlottable> list = ssm.allSBs;

					pBun.Received().PerformInHierarchy(ssm.AddSBToRes, Arg.Any<List<ISlottable>>());
					eBun.Received().PerformInHierarchy(ssm.AddSBToRes, Arg.Any<List<ISlottable>>());
					foreach(var gBun in gBuns)
						gBun.Received().PerformInHierarchy(ssm.AddSBToRes, Arg.Any<List<ISlottable>>());
				}
				[Test][Category("Factory")]
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

					Assert.That(actual, Is.EqualTo(expected));
				}
			/*	methods	*/
				[Test][Category("Methods")]
				public void Reset_WhenCalled_SetsActStateWFA(){
					SlotSystemManager ssm = MakeSSM();

					ssm.Reset();

					Assert.That(ssm.curActState, Is.SameAs(SlotSystemManager.ssmWaitForActionState));
				}
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
				public void ResetAndFocus_WhenCalled_SetsSelStateFocused(){
					SlotSystemManager ssm = MakeSetUpSSM();

					ssm.ResetAndFocus();

					Assert.That(ssm.curSelState, Is.SameAs(AbsSlotSystemElement.focusedState));
				}
				[Test][Category("Methods")]
				public void UpdateEquipStatesOnAll_WhenCalled_CallsEInvRemoveWithItemNotInAllEquippedItems(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun;
							IPoolInventory pInv;
							ISlotSystemPageElement pBunPE = MakePBunWithPoolInv(out pInv);
							pBun = (ISlotSystemBundle)pBunPE.element;
						ISlotSystemBundle eBun;
							ISlotGroup sgeBow;
							ISlotGroup sgeWear;
							ISlotGroup sgeCGears;
							IEquipmentSetInventory eInv;
							ISlotSystemPageElement eBunPE = MakeEBunPEWithSGEsAndEquipInv(out sgeBow, out sgeWear, out sgeCGears, out eInv);
							eBun = (ISlotSystemBundle)eBunPE.element;
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
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();
						pBun.isToggledOn.Returns(true);

					ssm.UpdateEquipStatesOnAll();

					eInv.Received().Remove(bowR);
					eInv.Received().Remove(wearR);
					eInv.Received().Remove(quiverR);
					eInv.Received().Remove(packR);
				}
				[Test][Category("Methods")]
				public void UpdateEquipStatesOnAll_WhenCalled_CallsEInvAddWithItemNotInEquipInv(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun;
							IPoolInventory pInv;
							ISlotSystemPageElement pBunPE = MakePBunWithPoolInv(out pInv);
							pBun = (ISlotSystemBundle)pBunPE.element;
						ISlotSystemBundle eBun;
							ISlotGroup sgeBow;
							ISlotGroup sgeWear;
							ISlotGroup sgeCGears;
							IEquipmentSetInventory eInv;
							ISlotSystemPageElement eBunPE = MakeEBunPEWithSGEsAndEquipInv(out sgeBow, out sgeWear, out sgeCGears, out eInv);
								eBun = (ISlotSystemBundle)eBunPE.element;
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
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();
						pBun.isToggledOn.Returns(true);

					ssm.UpdateEquipStatesOnAll();

					eInv.Received().Add(bow);
					eInv.Received().Add(wear);
					eInv.Received().Add(shield);
					eInv.Received().Add(mWeapon);
				}
				[Test][Category("Methods")]
				public void UpdateEquipStatesOnAll_WhenCalled_UpdatePoolInvItemInstsEquipStatus(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemBundle pBun;
						IPoolInventory pInv;
						ISlotSystemPageElement pBunPE = MakePBunWithPoolInv(out pInv);
						pBun = (ISlotSystemBundle)pBunPE.element;
					IEnumerable<ISlotSystemBundle> gBuns = MakeSubGenBundlesWithSGGs();
					ISlotSystemBundle eBun;
						ISlotGroup sgeBow;
						ISlotGroup sgeWear;
						ISlotGroup sgeCGears;
						IEquipmentSetInventory eInv;
						ISlotSystemPageElement eBunPE = MakeEBunPEWithSGEsAndEquipInv(out sgeBow, out sgeWear, out sgeCGears, out eInv);
						eBun = (ISlotSystemBundle)eBunPE.element;
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
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();
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
				[Test][Category("Methods")]
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
						ISlotSystemBundle pBun;
							IPoolInventory pInv;
							ISlotSystemPageElement pBunPE = MakePBunWithPoolInv(out pInv);
							pBun = (ISlotSystemBundle)pBunPE.element;
							IEnumerable<SlottableItem> pInvEles = new SlottableItem[]{};
							pInv.GetEnumerator().Returns(pInvEles.GetEnumerator());
						ISlotSystemBundle eBun;
							IEquipmentSetInventory eInv;
							ISlotSystemPageElement eBunPE = MakeSubEBunPEWithEquipInv(out eInv);
							eBun = (ISlotSystemBundle)eBunPE.element;
						IEnumerable<ISlotSystemBundle> gBuns = MakeSubGenBundlesWithSGGs();
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();
						pBun.isToggledOn.Returns(true);
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
						ISlotSystemBundle pBun;
							IPoolInventory pInv;
							ISlotSystemPageElement pBunPE = MakePBunWithPoolInv(out pInv);
							pBun = (ISlotSystemBundle)pBunPE.element;
							pBun.isToggledOn.Returns(true);
							pInv.GetEnumerator().Returns(items.GetEnumerator());
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns = MakeSubGenBundlesWithSGGs();
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();

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
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns = MakeSubGenBundlesWithSGGs();
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();
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
				[Test][Category("Methods")]
				public void Equip_MatchesAndSGFocusedInBundleAndSBNewSlotIDNotMinus1_CallsSBSetEqpStateEquippedState(){
					SlotSystemManager ssm = MakeSSM();
					BowInstance bow = MakeBowInstance(0);
					ISlotGroup stubSG = MakeSubSG();
						stubSG.isFocusedInHierarchy.Returns(true);
					ISlottable mockSB = MakeSubSB();
						mockSB.sg.Returns(stubSG);
						mockSB.itemInst.Returns(bow);
						mockSB.newSlotID.Returns(0);
					
					ssm.Equip(mockSB, bow);

					mockSB.Received().SetEqpState(Slottable.equippedState);
				}
				[Test][Category("Methods")]
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
						mockSB.SetEqpState((ISBEqpState)null);
						mockSB.SetEqpState(Slottable.equippedState);
					});
				}
				[Test][Category("Methods")]
				public void Unequip_MatchesAndSGFocusedInBundleAndSBSlotIDNotMinus1_CallsSBSetEqpStateUnequippedState(){
					SlotSystemManager ssm = MakeSSM();
					BowInstance bow = MakeBowInstance(0);
					ISlotGroup stubSG = MakeSubSG();
						stubSG.isFocusedInHierarchy.Returns(true);
					ISlottable mockSB = MakeSubSB();
						mockSB.sg.Returns(stubSG);
						mockSB.itemInst.Returns(bow);
						mockSB.slotID.Returns(0);
					
					ssm.Unequip(mockSB, bow);

					mockSB.Received().SetEqpState(Slottable.unequippedState);
				}
				[Test][Category("Methods")]
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
						mockSB.SetEqpState((ISBEqpState)null);
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
				[Test][Category("Methods")]
				public void FindParent_WhenCalled_CallAllBundlesPIHCheckAndReportParent(){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns = MakeSubGenBundlesWithSGGs();
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();

					ISlotSystemElement ele = MakeSubSSE();
					ssm.FindParent(ele);

					pBun.Received().PerformInHierarchy(ssm.CheckAndReportParent, ele);
					eBun.Received().PerformInHierarchy(ssm.CheckAndReportParent, ele);
					foreach(var gBun in gBuns)
						gBun.Received().PerformInHierarchy(ssm.CheckAndReportParent, ele);
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
							ele.Received().SetParent(parent);
						else
							ele.DidNotReceive().SetParent(parent);
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
				[Test][Category("Methods")]
				public void Focus_WhenCalled_SetsSelStateFocusedState(){
					SlotSystemManager ssm = MakeSetUpSSM();

					ssm.Focus();

					Assert.That(ssm.curSelState, Is.SameAs(AbsSlotSystemElement.focusedState));
				}
				[TestCase(true, true, true, true, true)]
				[TestCase(false, false, false, false, false)]
				[TestCase(false, true, false, true, true)]
				public void Focus_WhenCalled_CallsPEsAccordingly(bool pBunPEOn, bool eBunPEOn, bool gBunAPEOn, bool gBunBPEOn, bool gBunCPEOn){
					SlotSystemManager ssm = MakeSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();
					ISlotSystemPageElement pBunPE = ssm.GetPageElement(pBun);
					ISlotSystemPageElement eBunPE = ssm.GetPageElement(eBun);
					ISlotSystemPageElement gBunAPE = ssm.GetPageElement(gBunA);
					ISlotSystemPageElement gBunBPE = ssm.GetPageElement(gBunB);
					ISlotSystemPageElement gBunCPE = ssm.GetPageElement(gBunC);
						pBunPE.isFocusToggleOn = pBunPEOn;
						eBunPE.isFocusToggleOn = eBunPEOn;
						gBunAPE.isFocusToggleOn = gBunAPEOn;
						gBunBPE.isFocusToggleOn = gBunBPEOn;
						gBunCPE.isFocusToggleOn = gBunCPEOn;
						
					ssm.Focus();

					if(pBunPEOn)
						pBun.Received().Focus();
					else
						pBun.Received().Defocus();
					if(eBunPEOn)
						eBun.Received().Focus();
					else
						eBun.Received().Defocus();
					if(gBunAPEOn)
						gBunA.Received().Focus();
					else
						gBunA.Received().Defocus();
					if(gBunBPEOn)
						gBunB.Received().Focus();
					else
						gBunB.Received().Defocus();
					if(gBunCPEOn)
						gBunC.Received().Focus();
					else
						gBunC.Received().Defocus();
				}
				[Test][Category("Methods")]
				public void Defocus_WhenCalled_SetsSelStateDefocused(){
					SlotSystemManager ssm = MakeSetUpSSM();

					ssm.Defocus();

					Assert.That(ssm.curSelState, Is.SameAs(AbsSlotSystemElement.defocusedState));
				}
				[Test][Category("Methods")]
				public void Defocus_WhenCalled_CallsAllBundlesDefocus(){
					SlotSystemManager ssm = MakeSetUpSSM();

					ssm.Defocus();

					ssm.poolBundle.Received().Defocus();
					ssm.equipBundle.Received().Defocus();
					foreach(var bun in ssm.otherBundles)
						bun.Received().Defocus();
				}
				[Test][Category("Methods")]
				public void Deactivate_WhenCalled_SetsSelStateDeactivateed(){
					SlotSystemManager ssm = MakeSetUpSSM();

					ssm.Deactivate();

					Assert.That(ssm.curSelState, Is.SameAs(AbsSlotSystemElement.deactivatedState));
				}
				[Test][Category("Methods")]
				public void Deactivate_WhenCalled_CallsAllBundlesDeactivate(){
					SlotSystemManager ssm = MakeSetUpSSM();

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
						ISlotSystemBundle pBun = MakeSubBundle();
							pBun.isToggledOnInPageByDefault.Returns(pBunPEOn);
						ISlotSystemBundle eBun = MakeSubBundle();
							eBun.isToggledOnInPageByDefault.Returns(eBunPEOn);
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
								gBunA.isToggledOnInPageByDefault.Returns(gBunAPEOn);
							ISlotSystemBundle gBunB = MakeSubBundle();
								gBunB.isToggledOnInPageByDefault.Returns(gBunBPEOn);
							ISlotSystemBundle gBunC = MakeSubBundle();
								gBunC.isToggledOnInPageByDefault.Returns(gBunCPEOn);
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();
					ISlotSystemPageElement pBunPE = ssm.GetPageElement(pBun);
					ISlotSystemPageElement eBunPE = ssm.GetPageElement(eBun);
					ISlotSystemPageElement gBunAPE = ssm.GetPageElement(gBunA);
					ISlotSystemPageElement gBunBPE = ssm.GetPageElement(gBunB);
					ISlotSystemPageElement gBunCPE = ssm.GetPageElement(gBunC);

					ssm.Deactivate();

					Assert.That(pBunPE.isFocusToggleOn, Is.EqualTo(pBunPEOn));
					Assert.That(eBunPE.isFocusToggleOn, Is.EqualTo(eBunPEOn));
					Assert.That(gBunAPE.isFocusToggleOn, Is.EqualTo(gBunAPEOn));
					Assert.That(gBunBPE.isFocusToggleOn, Is.EqualTo(gBunBPEOn));
					Assert.That(gBunCPE.isFocusToggleOn, Is.EqualTo(gBunCPEOn));
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
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
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
						IEnumerable<ISlotSystemBundle> gBuns;
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
							
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB};
						ssm.InspectorSetUp(pBun, eBun, gBuns);
						ssm.SetElements();
					return ssm;
				}
				[TestCaseSource(typeof(PrePickFilterVariousSGTAComboCases))][Category("Methods")]
				public void PrePickFilter_VariousSGTACombo_OutsAccordingly(ISlotSystemTransaction ta, ISlotSystemTransaction sbTA, bool expected){
					SlotSystemManager ssm = MakeSSM();
						ISlottable pickedSB = MakeSubSB();
						IFocusedSGsFactory focFac = Substitute.For<IFocusedSGsFactory>();
							ISlotGroup sg = MakeSubSG();
								IEnumerable<ISlotSystemElement> sgEles;
									ISlottable sb = MakeSubSB();
									sgEles = new ISlotSystemElement[]{sb};
									sg.GetEnumerator().Returns(sgEles.GetEnumerator());
							List<ISlotGroup> sgs = new List<ISlotGroup>(new ISlotGroup[]{sg});
							focFac.focusedSGs.Returns(sgs);
							ssm.SetFocusedSGsFactory(focFac);
						ITransactionFactory taFac = MakeSubTAFactory();
							taFac.MakeTransaction(pickedSB, sg).Returns(ta);
							taFac.MakeTransaction(pickedSB, sb).Returns(sbTA);
							ssm.SetTAFactory(taFac);
					bool result;

					ssm.PrePickFilter(pickedSB, out result);

					Assert.That(result, Is.EqualTo(expected));
				}
					class PrePickFilterVariousSGTAComboCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							IRevertTransaction revTA =  Substitute.For<IRevertTransaction>();
							IReorderTransaction reoTA =  Substitute.For<IReorderTransaction>();
							ISortTransaction sortTA =  Substitute.For<ISortTransaction>();
							IFillTransaction fillTA =  Substitute.For<IFillTransaction>();
							ISwapTransaction swapTA =  Substitute.For<ISwapTransaction>();
							IStackTransaction stackTA =  Substitute.For<IStackTransaction>();
							yield return new object[]{revTA, revTA, false};
							yield return new object[]{revTA, fillTA, true};
							yield return new object[]{revTA, reoTA, true};
							yield return new object[]{reoTA, fillTA, true};
							yield return new object[]{sortTA, fillTA, true};
							yield return new object[]{fillTA, fillTA, true};
							yield return new object[]{swapTA, fillTA, true};
							yield return new object[]{stackTA, fillTA, true};
						}
					}
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
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
				[Test][Category("Transaction")]
				public void SetTransaction_PrevTransactionNull_SetsTransaction(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction stubTA = MakeSubTA();

					ssm.SetTransaction(stubTA);

					Assert.That(ssm.transaction, Is.SameAs(stubTA));
				}
				[Test][Category("Transaction")]
				public void SetTransaction_PrevTransactionNull_CallsTAIndicate(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction mockTA = MakeSubTA();

					ssm.SetTransaction(mockTA);

					mockTA.Received().Indicate();
				}
				[Test][Category("Transaction")]
				public void SetTransaction_DiffTA_SetsTransaction(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction prevTA = MakeSubTA();
					ISlotSystemTransaction stubTA = MakeSubTA();
					ssm.SetTransaction(prevTA);
					ssm.SetTransaction(stubTA);

					Assert.That(ssm.transaction, Is.SameAs(stubTA));
				}
				[Test][Category("Transaction")]
				public void SetTransaction_DiffTA_CallsTAIndicate(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction prevTA = MakeSubTA();
					ISlotSystemTransaction mockTA = MakeSubTA();

					ssm.SetTransaction(prevTA);
					ssm.SetTransaction(mockTA);

					mockTA.Received().Indicate();
				}
				[Test][Category("Transaction")]
				public void SetTransaction_SameTA_DoesNotCallIndicateTwice(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction mockTA = MakeSubTA();

					ssm.SetTransaction(mockTA);
					ssm.SetTransaction(mockTA);

					mockTA.Received(1).Indicate();
				}
				[Test][Category("Transaction")]
				public void SetTransaction_Null_SetsTANull(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction stubTA = MakeSubTA();

					ssm.SetTransaction(stubTA);
					ssm.SetTransaction(null);

					Assert.That(ssm.transaction, Is.Null);
				}
				[Test][Category("Transaction")]
				public void AcceptsSGTAComp_ValidSG_SetsDone(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();
					ssm.SetSG1(stubSG);

					ssm.AcceptSGTAComp(stubSG);

					Assert.That(ssm.sg2Done, Is.True);
				}
				[Test][Category("Transaction")]
				public void AcceptsDITAComp_ValidDI_SetsDone(){
					SlotSystemManager ssm = MakeSSM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());
					ssm.SetDIcon1(stubDI);

					ssm.AcceptDITAComp(stubDI);

					Assert.That(ssm.dIcon1Done, Is.True);
				}
				[Test][Category("Transaction")]
				public void ExecuteTransaction_WhenCalled_SetsActStatTransaction(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemTransaction stubTA = MakeSubTA();
					ssm.SetTransaction(stubTA);
					
					ssm.ExecuteTransaction();

					Assert.That(ssm.curActState, Is.SameAs(SlotSystemManager.ssmTransactionState));
				}
				[Test][Category("Transaction")]
				public void SetTargetSB_FromNullToSome_CallsSBSetSelStateSelected(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();

					ssm.SetTargetSB(mockSB);

					mockSB.Received().SetSelState(AbsSlotSystemElement.selectedState);
				}
				[Test][Category("Transaction")]
				public void SetTargetSB_FromNullToSome_SetsItTargetSB(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable stubSB = MakeSubSB();

					ssm.SetTargetSB(stubSB);

					Assert.That(ssm.targetSB, Is.SameAs(stubSB));
				}
				[Test][Category("Transaction")]
				public void SetTargetSB_FromOtherToSome_CallsSBSetSelStateSelected(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable stubSB = MakeSubSB();
					ISlottable mockSB = MakeSubSB();
					ssm.SetTargetSB(stubSB);

					ssm.SetTargetSB(mockSB);
					
					mockSB.Received().SetSelState(AbsSlotSystemElement.selectedState);
				}
				[Test][Category("Transaction")]
				public void SetTargetSB_FromOtherToSome_SetsItTargetSB(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable prevSB = MakeSubSB();
					ISlottable stubSB = MakeSubSB();
					ssm.SetTargetSB(prevSB);

					ssm.SetTargetSB(stubSB);
					
					Assert.That(ssm.targetSB, Is.SameAs(stubSB));
				}
				[Test][Category("Transaction")]
				public void SetTargetSB_FromOtherToSome_CallOtherSBSetSelStateFocused(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ISlottable stubSB = MakeSubSB();
					ssm.SetTargetSB(mockSB);

					ssm.SetTargetSB(stubSB);
					
					mockSB.Received().SetSelState(AbsSlotSystemElement.focusedState);
				}
				[Test][Category("Transaction")]
				public void SetTargetSB_SomeToNull_CallSBSetSelStateFocused(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetTargetSB(mockSB);

					ssm.SetTargetSB(null);

					mockSB.Received().SetSelState(AbsSlotSystemElement.focusedState);
				}
				[Test][Category("Transaction")]
				public void SetTargetSB_SomeToNull_SetsNull(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetTargetSB(mockSB);

					ssm.SetTargetSB(null);

					Assert.That(ssm.targetSB, Is.Null);
				}
				[Test][Category("Transaction")]
				public void SetTargetSB_SomeToSame_DoesNotCallSetSelStateTwice(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetTargetSB(mockSB);

					ssm.SetTargetSB(mockSB);

					mockSB.Received(1).SetSelState(AbsSlotSystemElement.selectedState);
				}
				[Test][Category("Transaction")]
				public void SetTargetSB_SomeToSame_DoesNotCallSetSelStateFocused(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetTargetSB(mockSB);

					ssm.SetTargetSB(mockSB);

					mockSB.DidNotReceive().SetSelState(AbsSlotSystemElement.focusedState);
				}
				[Test][Category("Transaction")]
				public void SetSG1_NullToSome_SetsSG1(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG1(stubSG);

					Assert.That(ssm.sg1, Is.SameAs(stubSG));
				}
				[Test][Category("Transaction")]
				public void SetSG1_NullToSome_SetsSG1DoneFalse(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG1(stubSG);

					Assert.That(ssm.sg1Done, Is.False);
				}
				[Test][Category("Transaction")]
				public void SetSG1_OtherToSome_SetsSG1(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup prevSG = MakeSubSG();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG1(prevSG);
					ssm.SetSG1(stubSG);

					Assert.That(ssm.sg1, Is.SameAs(stubSG));
				}
				[Test][Category("Transaction")]
				public void SetSG1_OtherToSome_SetsSG1DoneFalse(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup prevSG = MakeSubSG();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG1(prevSG);
					ssm.SetSG1(stubSG);

					Assert.That(ssm.sg1Done, Is.False);
				}
				[Test][Category("Transaction")]
				public void SetSG1_SomeToNull_SetsSG1Null(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();
					ssm.SetSG1(stubSG);

					ssm.SetSG1(null);

					Assert.That(ssm.sg1, Is.Null);
				}
				[Test][Category("Transaction")]
				public void SetSG1_SomeToNull_SetsSG1DoneTrue(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();
					ssm.SetSG1(stubSG);

					ssm.SetSG1(null);

					Assert.That(ssm.sg1Done, Is.True);
				}
				[Test][Category("Transaction")]
				public void SetSG2_NullToSome_SetsSG2(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG2(stubSG);

					Assert.That(ssm.sg2, Is.SameAs(stubSG));
				}
				[Test][Category("Transaction")]
				public void SetSG2_NullToSome_SetsSG2DoneFalse(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG2(stubSG);

					Assert.That(ssm.sg2Done, Is.False);
				}
				[Test][Category("Transaction")]
				public void SetSG2_NullToSome_CallSG2SetSelStateSelected(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();

					ssm.SetSG2(mockSG);

					mockSG.Received().SetSelState(AbsSlotSystemElement.selectedState);
				}
				[Test][Category("Transaction")]
				public void SetSG2_OtherToSome_SetsSG2(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup prevSG = MakeSubSG();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG2(prevSG);
					ssm.SetSG2(stubSG);

					Assert.That(ssm.sg2, Is.SameAs(stubSG));
				}
				[Test][Category("Transaction")]
				public void SetSG2_OtherToSome_SetsSG2DoneFalse(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup prevSG = MakeSubSG();
					ISlotGroup stubSG = MakeSubSG();

					ssm.SetSG2(prevSG);
					ssm.SetSG2(stubSG);

					Assert.That(ssm.sg2Done, Is.False);
				}
				[Test][Category("Transaction")]
				public void SetSG2_OtherToSome_CallsSG2SetSelStateSelected(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup prevSG = MakeSubSG();
					ISlotGroup mockSG = MakeSubSG();

					ssm.SetSG2(prevSG);
					ssm.SetSG2(mockSG);

					mockSG.Received().SetSelState(AbsSlotSystemElement.selectedState);
				}
				[Test][Category("Transaction")]
				public void SetSG2_SomeToNull_SetsSG2Null(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();
					ssm.SetSG2(stubSG);

					ssm.SetSG2(null);

					Assert.That(ssm.sg2, Is.Null);
				}
				[Test][Category("Transaction")]
				public void SetSG2_SomeToNull_SetsSG2DoneTrue(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup stubSG = MakeSubSG();
					ssm.SetSG2(stubSG);

					ssm.SetSG2(null);

					Assert.That(ssm.sg2Done, Is.True);
				}
				[Test][Category("Transaction")]
				public void SetDIcon1_ToNonNull_SetsDIcon1DoneFalse(){
					SlotSystemManager ssm = MakeSSM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());

					ssm.SetDIcon1(stubDI);

					Assert.That(ssm.dIcon1Done, Is.False);
				}
				[Test][Category("Transaction")]
				public void SetDIcon1_ToNull_SetsDIcon1DoneTrue(){
					SlotSystemManager ssm = MakeSSM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());

					ssm.SetDIcon1(stubDI);
					ssm.SetDIcon1(null);

					Assert.That(ssm.dIcon1Done, Is.True);
				}
				[Test][Category("Transaction")]
				public void SetDIcon2_ToNonNull_SetsDIcon2DoneFalse(){
					SlotSystemManager ssm = MakeSSM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());

					ssm.SetDIcon2(stubDI);

					Assert.That(ssm.dIcon2Done, Is.False);
				}
				[Test][Category("Transaction")]
				public void SetDIcon2_ToNull_SetsDIcon2DoneTrue(){
					SlotSystemManager ssm = MakeSSM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());

					ssm.SetDIcon2(stubDI);
					ssm.SetDIcon2(null);

					Assert.That(ssm.dIcon2Done, Is.True);
				}
				[Test][Category("Transaction")]
				public void SetHovered_NullToSB_SetsHovered(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();

					ssm.SetHovered(mockSB);

					Assert.That(ssm.hovered, Is.SameAs(mockSB));
				}
				[Test][Category("Transaction")]
				public void SetHovered_SBToNull_CallSBOnHoverExit(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetHovered(mockSB);

					ssm.SetHovered(null);

					mockSB.Received().OnHoverExitMock();
				}
				[Test][Category("Transaction")]
				public void SetHovered_SBToNull_SetsNull(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetHovered(mockSB);

					ssm.SetHovered(null);

					Assert.That(ssm.hovered, Is.Null);
				}
				[Test][Category("Transaction")]
				public void SetHovered_SBToSomeSSE_CallSBOnHoverExit(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetHovered(mockSB);

					ssm.SetHovered(MakeSubSSE());

					mockSB.Received().OnHoverExitMock();
				}
				[Test][Category("Transaction")]
				public void SetHovered_SBToSame_DoesNotCallSBOnHoverExit(){
					SlotSystemManager ssm = MakeSSM();
					ISlottable mockSB = MakeSubSB();
					ssm.SetHovered(mockSB);

					ssm.SetHovered(mockSB);

					mockSB.DidNotReceive().OnHoverExitMock();
				}
				[Test][Category("Transaction")]
				public void SetHovered_NullToSG_SetsHovered(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();

					ssm.SetHovered(mockSG);

					Assert.That(ssm.hovered, Is.SameAs(mockSG));
				}
				[Test][Category("Transaction")]
				public void SetHovered_SGToNull_CallSGOnHoverExit(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();
					ssm.SetHovered(mockSG);

					ssm.SetHovered(null);

					mockSG.Received().OnHoverExitMock();
				}
				[Test][Category("Transaction")]
				public void SetHovered_SGToNull_SetsNull(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();
					ssm.SetHovered(mockSG);

					ssm.SetHovered(null);

					Assert.That(ssm.hovered, Is.Null);
				}
				[Test][Category("Transaction")]
				public void SetHovered_SGToSomeSSE_CallSGOnHoverExit(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();
					ssm.SetHovered(mockSG);

					ssm.SetHovered(MakeSubSSE());

					mockSG.Received().OnHoverExitMock();
				}
				[Test][Category("Transaction")]
				public void SetHovered_SGToSame_DoesNotCallSGOnHoverExit(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();
					ssm.SetHovered(mockSG);

					ssm.SetHovered(mockSG);

					mockSG.DidNotReceive().OnHoverExitMock();
				}
				[Test][Category("Transaction")]
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
				[Test][Category("Transaction")]
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

					Assert.That(ssm.transactionResults, Is.EqualTo(expected));
				}
				[Test][Category("Transaction")]
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

					Assert.That(ssm.transactionResults, Is.EqualTo(expected));
				}
				[Category("Transaction")]
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
				[Category("Transaction")]
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
				[Test][Category("Transaction")]
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
				[Test][Category("Transaction")]
				public void ReferToTAAndUpdateSelState_TAResultsNull_CallsSGSetSelStateFocused(){
					SlotSystemManager ssm = MakeSSM();
					ISlotGroup mockSG = MakeSubSG();

					ssm.ReferToTAAndUpdateSelState(mockSG);

					mockSG.Received().SetSelState(AbsSlotSystemElement.focusedState);
				}
				[TestCaseSource(typeof(ReferToTAAndUpdateSelState_VariousTAsCases))][Category("Transaction")]
				public void ReferToTAAndUpdateSelState_VariousTAs_CallsSGSetSelStateAccordingly(ISlotSystemTransaction ta, ISSESelState state){
					SlotSystemManager ssm = MakeSSM();
						ISlotGroup mockSG = MakeSubSG();
					Dictionary<ISlotSystemElement, ISlotSystemTransaction> dict = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
						dict.Add(mockSG, ta);
						ssm.transactionResults = dict;
					ssm.ReferToTAAndUpdateSelState(mockSG);

					mockSG.Received().SetSelState(state);
				}
					class ReferToTAAndUpdateSelState_VariousTAsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							yield return new object[]{Substitute.For<IRevertTransaction>(), AbsSlotSystemElement.defocusedState};
							yield return new object[]{Substitute.For<IReorderTransaction>(), AbsSlotSystemElement.focusedState};
							yield return new object[]{Substitute.For<ISortTransaction>(), AbsSlotSystemElement.focusedState};
							yield return new object[]{Substitute.For<IFillTransaction>(), AbsSlotSystemElement.focusedState};
							yield return new object[]{Substitute.For<ISwapTransaction>(), AbsSlotSystemElement.focusedState};
							yield return new object[]{Substitute.For<IStackTransaction>(), AbsSlotSystemElement.focusedState};
						}
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
						sgeBow.filter.Returns(new SGBowFilter());
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
						sgeWear.filter.Returns(new SGWearFilter());
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
						sgeCGears.filter.Returns(new SGCGearsFilter());
					return sgeCGears;
				}
				public SlotSystemManager MakeSetUpSSM(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemBundle pBun = MakeSubPoolBundleWithSGPs();
					ISlotSystemBundle eBun = MakeSubEquipBundleWithESets();
					IEnumerable<ISlotSystemBundle> gBuns;
						ISlotSystemBundle gBunA = MakeSubGenBundleWithSGGs();
						ISlotSystemBundle gBunB = MakeSubGenBundleWithSGGs();
						ISlotSystemBundle gBunC = MakeSubGenBundleWithSGGs();
						gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
					ssm.InspectorSetUp(pBun, eBun, gBuns);
					ssm.SetElements();
					return ssm;
				}
				public ISlotSystemBundle MakeSubPoolBundleWithSGPs(){
					ISlotSystemBundle poolBundle = MakeSubBundle();
						ISlotGroup sgpA = MakeSubSG();
							IEnumerable<ISlotSystemElement> emptyEles = new ISlotSystemElement[]{};
							sgpA.GetEnumerator().Returns(emptyEles.GetEnumerator());
						ISlotGroup sgpB = MakeSubSG();
							sgpB.GetEnumerator().Returns(emptyEles.GetEnumerator());
						ISlotGroup sgpC = MakeSubSG();
							sgpC.GetEnumerator().Returns(emptyEles.GetEnumerator());
					IEnumerable<ISlotSystemElement> sgs = new ISlotSystemElement[]{
						sgpA, sgpB, sgpC
					};
					poolBundle.elements.Returns(sgs);
					return poolBundle;
				}
				public ISlotSystemPageElement MakeSubPoolBundlePEInitWithSGs(){
					ISlotSystemBundle poolBundle = MakeSubBundle();
					ISlotSystemPageElement poolBundlePE = MakeSubPageElement();
					poolBundlePE.element.Returns(poolBundle);
					ISlotGroup sgpA = MakeSubSG();
						IEnumerable<ISlotSystemElement> emptyEles = new ISlotSystemElement[]{};
						sgpA.GetEnumerator().Returns(emptyEles.GetEnumerator());
					ISlotGroup sgpB = MakeSubSG();
						sgpB.GetEnumerator().Returns(emptyEles.GetEnumerator());
					ISlotGroup sgpC = MakeSubSG();
						sgpC.GetEnumerator().Returns(emptyEles.GetEnumerator());
					IEnumerable<ISlotSystemElement> sgs = new ISlotSystemElement[]{
						sgpA, sgpB, sgpC
					};
					poolBundle.elements.Returns(sgs);
					return poolBundlePE;
				}
				public ISlotSystemBundle MakeSubEquipBundleWithESets(){
					ISlotSystemBundle equipBundle = MakeSubBundle();
						IEquipmentSet eSetA = MakeSubEquipmentSetInitWithSGs();
						IEquipmentSet eSetB = MakeSubEquipmentSetInitWithSGs();
						IEquipmentSet eSetC = MakeSubEquipmentSetInitWithSGs();
						IEnumerable<ISlotSystemElement> eSets = new ISlotSystemElement[]{
							eSetA, eSetB, eSetC
						};
					equipBundle.elements.Returns(eSets);
					return equipBundle;
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
				public ISlotSystemBundle MakeSubGenBundleWithSGGs(){
					ISlotSystemBundle genBun = MakeSubBundle();
						ISlotGroup sgg = MakeSubSGWithEmptySBs();
						ISlotGroup sgg1 = MakeSubSGWithEmptySBs();
						ISlotGroup sgg2 = MakeSubSGWithEmptySBs();
						IEnumerable<ISlotSystemElement> gBunEles = new ISlotSystemElement[]{
							sgg, sgg1, sgg2
						};
					genBun.elements.Returns(gBunEles);
					return genBun;
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
				public IEnumerable<ISlotSystemBundle> MakeSubGenBundlesWithSGGs(){
					IEnumerable<ISlotSystemBundle> gBuns;
						ISlotSystemBundle gBunA = MakeSubGenBundleWithSGGs();
						ISlotSystemBundle gBunB = MakeSubGenBundleWithSGGs();
						ISlotSystemBundle gBunC = MakeSubGenBundleWithSGGs();
						gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};
					return gBuns;
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
