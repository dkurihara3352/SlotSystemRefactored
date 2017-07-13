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
				ISlotSystemPageElement genBundleAPE = MakeSubPageElement();
					ISlotSystemBundle genBundleA = MakeSubBundle();
					genBundleAPE.element.Returns(genBundleA);
				ISlotSystemPageElement genBundleBPE = MakeSubPageElement();
					ISlotSystemBundle genBundleB = MakeSubBundle();
					genBundleBPE.element.Returns(genBundleB);
				ISlotSystemPageElement genBundleCPE = MakeSubPageElement();
					ISlotSystemBundle genBundleC = MakeSubBundle();
					genBundleCPE.element.Returns(genBundleC);
				IEnumerable<ISlotSystemPageElement> genBunPEs = new ISlotSystemPageElement[]{
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
			[Test]
			public void allSGs_Always_ReturnsCollection(){
				ISlotSystemPageElement poolBundlePE = MakePoolBundlePEInitWithSGs();
				ISlotSystemPageElement equipBundlePE = MakePoolBundlePEInitWithSGs();
				IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeGenBundlePEsInitWithSGs();
				SlotSystemManager ssm = MakeSSM();
				ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);
				List<ISlotSystemElement> expected = new List<ISlotSystemElement>();
				foreach(var ele in GetAllElementsInPE(poolBundlePE))
					expected.Add(ele);
				foreach(var ele in GetAllElementsInPE(equipBundlePE))
					expected.Add(ele);
				foreach(var ele in GetAllElementsInPEs(genBundlesPEs))
					expected.Add(ele);
				
				Assert.That(ssm.allSGs, Is.EqualTo(expected));
				
			}
			[Test]
			public void AddInSGList_WhenCalled_VerifySGsAndStoreThemInTheList(){
				SlotSystemManager ssm = MakeSSM();
				ISlottable sb = MakeSubSB();
				ISlotSystemManager stubSSM = MakeSubSSM();
				ISlotSystemElement ele = MakeSubSSE();
				ISlotGroup sgA = MakeSubSG();
				TestSlotSystemPage page = MakeTestSSPage();
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
			public void AllSGPs_Always_ReturnsAllSGsInPoolBundle(){
				ISlotSystemPageElement poolBundlePE = MakePoolBundlePEInitWithSGs();
				ISlotSystemPageElement equipBundlePE = MakePoolBundlePEInitWithSGs();
				IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeGenBundlePEsInitWithSGs();
				SlotSystemManager ssm = MakeSSM();
				ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);

				IEnumerable<ISlotSystemElement> expected = GetAllElementsInPE(poolBundlePE);
				Assert.That(ssm.allSGPs, Is.EqualTo(expected));
			}
			[Test]
			public void AllSGEs_Always_ReturnsAllSGsInEquipBundle(){
				ISlotSystemPageElement poolBundlePE = MakePoolBundlePEInitWithSGs();
				ISlotSystemPageElement equipBundlePE = MakeEquipBundlePEInitWithSGs();
				IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeGenBundlePEsInitWithSGs();
				SlotSystemManager ssm = MakeSSM();
				ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);

				// IEnumerable<ISlotSystemElement> expected = GetAllElementsInPE(equipBundlePE);
				IEnumerable<ISlotSystemElement> expected = GetAllSGsInPE(equipBundlePE);
				Assert.That(ssm.allSGEs, Is.EqualTo(expected));
			}
			[Test]
			public void AllSGGs_Always_ReturnsAllSGsInGenBundles(){
				ISlotSystemPageElement poolBundlePE = MakePoolBundlePEInitWithSGs();
				ISlotSystemPageElement equipBundlePE = MakeEquipBundlePEInitWithSGs();
				IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeGenBundlePEsInitWithSGs();
				SlotSystemManager ssm = MakeSSM();
				ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);

				IEnumerable<ISlotSystemElement> expected = GetAllElementsInPEs(genBundlesPEs);

				Assert.That(ssm.allSGGs, Is.EqualTo(expected));
			}
			[TestCaseSource(typeof(FocusedSGP_PoolBundleToggledOnCases))]
			public void FocusedSGP_PoolBundleToggledOn_ReturnsPoolBundleFocusedElement(IEnumerable<ISlotSystemElement> sgs, ISlotGroup expected){
				SlotSystemBundle poolBundle = MakeSSBundle();
					ISlotSystemPageElement poolBundlePE = MakeSubPageElement();
					poolBundlePE.element.Returns(poolBundle);
					poolBundle.Initialize("pBun", sgs);
					poolBundle.SetFocusedBundleElement(expected);
				ISlotSystemPageElement equipBundlePE = MakeEquipBundlePEInitWithSGs();
				IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeGenBundlePEsInitWithSGs();
				SlotSystemManager ssm = MakeSSM();
				ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);
				ssm.poolBundle.ToggleOnPageElement();
				
				ISlotGroup actual = ssm.focusedSGP;

				Assert.That(actual, Is.SameAs(expected));
			}
				class FocusedSGP_PoolBundleToggledOnCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						ISlotGroup sgpA = MakeSG();
						sgpA.SetSBs(new List<ISlottable>());
						ISlotGroup sgpB = MakeSG();
							sgpB.SetSBs(new List<ISlottable>());
						ISlotGroup sgpC = MakeSG();
							sgpC.SetSBs(new List<ISlottable>());
						IEnumerable<ISlotSystemElement> sgs = new ISlotSystemElement[]{
							sgpA, sgpB, sgpC
						};
						yield return new object[]{sgs, sgpA};
						yield return new object[]{sgs, sgpB};
						yield return new object[]{sgs, sgpC};
					}
				}
			[TestCaseSource(typeof(FocusedEpSet_EquipBundleIsToggledOnCases))]
			public void FocusedEpSet_EquipBundleIsToggledOn_ReturnsEquipBundleFocusedElement(IEnumerable<ISlotSystemElement> eSets, IEquipmentSet expected){
				ISlotSystemPageElement poolBundlePE = MakePoolBundlePEInitWithSGs();
				SlotSystemBundle equipBundle = MakeSSBundle();
					ISlotSystemPageElement equipBundlePE = MakeSubPageElement();
					equipBundlePE.element.Returns(equipBundle);
					equipBundle.Initialize("eBun", eSets);
				IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeGenBundlePEsInitWithSGs();
				SlotSystemManager ssm = MakeSSM();
				ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);
				ssm.equipBundle.ToggleOnPageElement();
				ssm.equipBundle.SetFocusedBundleElement(expected);

				IEquipmentSet actual = ssm.focusedEqSet;

				Assert.That(actual, Is.SameAs(expected));
			}
				class FocusedEpSet_EquipBundleIsToggledOnCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						EquipmentSet eSetA = MakeEquipmentSetInitWithSGs();
						EquipmentSet eSetB = MakeEquipmentSetInitWithSGs();
						EquipmentSet eSetC = MakeEquipmentSetInitWithSGs();
						IEnumerable<ISlotSystemElement> eSets = new ISlotSystemElement[]{
							eSetA, eSetB, eSetC
						};
						yield return new object[]{eSets, eSetA};
						yield return new object[]{eSets, eSetB};
						yield return new object[]{eSets, eSetC};
					}
				}
			[TestCaseSource(typeof(FocusedSGEs_EquipBundleIsToggledOnCases))]
			public void FocusedSGEs_EquipBundleIsToggledOn_ReturnsFocusedESetSGs(IEnumerable<ISlotSystemElement> eSets, IEquipmentSet focusedSet, IEnumerable<ISlotGroup> expected){
				ISlotSystemPageElement poolBundlePE = MakePoolBundlePEInitWithSGs();
				SlotSystemBundle equipBundle = MakeSSBundle();
					ISlotSystemPageElement equipBundlePE = MakeSubPageElement();
					equipBundlePE.element.Returns(equipBundle);
					equipBundle.Initialize("eBun", eSets);
				IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeGenBundlePEsInitWithSGs();
				SlotSystemManager ssm = MakeSSM();
				ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);
				ssm.equipBundle.ToggleOnPageElement();
				ssm.equipBundle.SetFocusedBundleElement(focusedSet);

				IEnumerable<ISlotGroup> actual = ssm.focusedSGEs;
				Assert.That(actual, Is.EqualTo(expected));
			}
				class FocusedSGEs_EquipBundleIsToggledOnCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						EquipmentSet eSetA = MakeEquipmentSetInitWithSGs();
						EquipmentSet eSetB = MakeEquipmentSetInitWithSGs();
						EquipmentSet eSetC = MakeEquipmentSetInitWithSGs();
						IEnumerable<ISlotSystemElement> eSets = new ISlotSystemElement[]{
							eSetA, eSetB, eSetC
						};
						yield return new object[]{eSets, eSetA, SSEsToSGs(eSetA.elements)};
						yield return new object[]{eSets, eSetB, SSEsToSGs(eSetB.elements)};
						yield return new object[]{eSets, eSetC, SSEsToSGs(eSetC.elements)};
					}
				}
					static IEnumerable<ISlotGroup> SSEsToSGs(IEnumerable<ISlotSystemElement> eles){
						foreach(var ele in eles)
							yield return (ISlotGroup)ele;
					}
			//
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
						/*	Not Focused	*/
							ISlotGroup notFocused = MakeSubSG();
							notFocused.isFocused.Returns(false);
							yield return new object[]{notFocused, new ISlotGroup[]{}};
						/*	isPageElement	*/
							SlotGroup peSGA = MakeSGWithEmptySBs();
								SlotSystemPageElement peSGAPE = MakePageElement(peSGA, true);
							SlotGroup peSGB = MakeSGWithEmptySBs();
								SlotSystemPageElement peSGBPE = MakePageElement(peSGB, false);
							SlotGroup peSGC = MakeSGWithEmptySBs();
								SlotSystemPageElement peSGCPE = MakePageElement(peSGC, false);
							GenericPage page = MakeGenPage();
							IEnumerable<ISlotSystemPageElement> pEles = new ISlotSystemPageElement[]{
								peSGAPE, peSGBPE, peSGCPE
							};
							page.Initialize("genP", pEles);
							page.Focus();
							yield return new object[]{peSGA, new ISlotGroup[]{peSGA}};
							yield return new object[]{peSGB, new ISlotGroup[]{}};
							yield return new object[]{peSGC, new ISlotGroup[]{}};
						/*	bundle ele*/
							SlotGroup buSGA = MakeSGWithEmptySBs();
							SlotGroup buSGB = MakeSGWithEmptySBs();
							SlotGroup buSGC = MakeSGWithEmptySBs();
							SlotSystemBundle bundle = MakeSSBundle();
							IEnumerable<ISlotSystemElement> eles = new ISlotSystemElement[]{
								buSGA, buSGB, buSGC
							};
							bundle.Initialize("bun", eles);
							bundle.SetFocusedBundleElement(buSGB);
							bundle.Focus();
							yield return new object[]{buSGA, new ISlotGroup[]{}};
							yield return new object[]{buSGB, new ISlotGroup[]{buSGB}};
							yield return new object[]{buSGC, new ISlotGroup[]{}};
					}
				}
			[Test]
			public void FocusedSGGs_Always_ReturnsFocusedSGsInGenBundles(){
				
			}
			/*	helper	*/
				public SlotSystemManager MakeSSMInitWithPEs(){
					ISlotSystemPageElement poolBundlePE = MakePoolBundlePEInitWithSGs();
					ISlotSystemPageElement equipBundlePE = MakeEquipBundlePEInitWithSGs();
					IEnumerable<ISlotSystemPageElement> genBundlesPEs = MakeGenBundlePEsInitWithSGs();
					SlotSystemManager ssm = MakeSSM();
					ssm.Initialize(poolBundlePE, equipBundlePE, genBundlesPEs);
					return ssm;
				}
				public SlotSystemManager InitializedSSM(){
					SlotSystemManager ssm = MakeSSM();
					ISlotSystemPageElement poolBunPE = MakeSubPageElement();
						ISlotSystemBundle poolBundle = MakeSubBundle();
						poolBunPE.element.Returns(poolBundle);
					ISlotSystemPageElement equipBunPE = MakeSubPageElement();
						ISlotSystemBundle equipBundle = MakeSubBundle();
						equipBunPE.element.Returns(equipBundle);
					ISlotSystemPageElement genBundleAPE = MakeSubPageElement();
						ISlotSystemBundle genBundleA = MakeSubBundle();
						genBundleAPE.element.Returns(genBundleA);
					ISlotSystemPageElement genBundleBPE = MakeSubPageElement();
						ISlotSystemBundle genBundleB = MakeSubBundle();
						genBundleBPE.element.Returns(genBundleB);
					ISlotSystemPageElement genBundleCPE = MakeSubPageElement();
						ISlotSystemBundle genBundleC = MakeSubBundle();
						genBundleCPE.element.Returns(genBundleC);
					IEnumerable<ISlotSystemPageElement> genBunPEs = new ISlotSystemPageElement[]{
						genBundleAPE, genBundleBPE, genBundleCPE
					};
					ssm.Initialize(poolBunPE, equipBunPE, genBunPEs);
					return ssm;
				}
				public ISlotSystemPageElement MakePoolBundlePEInitWithSGs(){
					SlotSystemBundle poolBundle = MakeSSBundle();
					ISlotSystemPageElement poolBundlePE = MakeSubPageElement();
					poolBundlePE.element.Returns(poolBundle);
					ISlotGroup sgpA = MakeSG();
						sgpA.SetSBs(new List<ISlottable>());
					ISlotGroup sgpB = MakeSG();
						sgpB.SetSBs(new List<ISlottable>());
					ISlotGroup sgpC = MakeSG();
						sgpC.SetSBs(new List<ISlottable>());
					IEnumerable<ISlotSystemElement> sgs = new ISlotSystemElement[]{
						sgpA, sgpB, sgpC
					};
					poolBundle.Initialize("pBun", sgs);
					return poolBundlePE;
				}
				public ISlotSystemPageElement MakeEquipBundlePEInitWithSGs(){
					SlotSystemBundle equipBundle = MakeSSBundle();
					ISlotSystemPageElement equipBundlePE = MakeSubPageElement();
					equipBundlePE.element.Returns(equipBundle);
					EquipmentSet eSetA = MakeEquipmentSetInitWithSGs();
					EquipmentSet eSetB = MakeEquipmentSetInitWithSGs();
					EquipmentSet eSetC = MakeEquipmentSetInitWithSGs();
					IEnumerable<ISlotSystemElement> eSets = new ISlotSystemElement[]{
						eSetA, eSetB, eSetC
					};
					equipBundle.Initialize("eBun", eSets);
					return equipBundlePE;
				}
				public IEnumerable<ISlotSystemPageElement> MakeGenBundlePEsInitWithSGs(){
					SlotSystemBundle genBunA = MakeSSBundle();
						SlotGroup sggA = MakeSG();
							sggA.SetSBs(new List<ISlottable>());
						SlotGroup sggA1 = MakeSG();
							sggA1.SetSBs(new List<ISlottable>());
						SlotGroup sggA2 = MakeSG();
							sggA2.SetSBs(new List<ISlottable>());
						IEnumerable<ISlotSystemElement> gBunAEles = new ISlotSystemElement[]{
							sggA, sggA1, sggA2
						};
						genBunA.Initialize("gBunA", gBunAEles);
						ISlotSystemPageElement genBunAPE = MakeSubPageElement();
						genBunAPE.element.Returns(genBunA);
					SlotSystemBundle genBunB = MakeSSBundle();
						SlotGroup sggB = MakeSG();
							sggB.SetSBs(new List<ISlottable>());
						SlotGroup sggB1 = MakeSG();
							sggB1.SetSBs(new List<ISlottable>());
						SlotGroup sggB2 = MakeSG();
							sggB2.SetSBs(new List<ISlottable>());
						IEnumerable<ISlotSystemElement> gBunBEles = new ISlotSystemElement[]{
							sggB, sggB1, sggB2
						};
						genBunB.Initialize("gBunB", gBunBEles);
						ISlotSystemPageElement genBunBPE = MakeSubPageElement();
						genBunBPE.element.Returns(genBunB);
					SlotSystemBundle genBunC = MakeSSBundle();
						SlotGroup sggC = MakeSG();
							sggC.SetSBs(new List<ISlottable>());
						SlotGroup sggC1 = MakeSG();
							sggC1.SetSBs(new List<ISlottable>());
						SlotGroup sggC2 = MakeSG();
							sggC2.SetSBs(new List<ISlottable>());
						IEnumerable<ISlotSystemElement> gBunCEles = new ISlotSystemElement[]{
							sggC, sggC1, sggC2
						};
						ISlotSystemPageElement genBunCPE = MakeSubPageElement();
						genBunCPE.element.Returns(genBunC);
						genBunC.Initialize("gBunC", gBunCEles);
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
				protected static EquipmentSet MakeEquipmentSetInitWithSGs(){
					SlotGroup bowSG = MakeSG();
						bowSG.SetSBs(new List<ISlottable>());
					SlotGroup wearSG = MakeSG();
						wearSG.SetSBs(new List<ISlottable>());
					SlotGroup cGearsSG = MakeSG();
						cGearsSG.SetSBs(new List<ISlottable>());
					ISlotSystemPageElement bowSGPE = MakeSubPageElement();
						bowSGPE.element.Returns(bowSG);
					ISlotSystemPageElement wearSGPE = MakeSubPageElement();
						wearSGPE.element.Returns(wearSG);
					ISlotSystemPageElement cGearsSGPE = MakeSubPageElement();
						cGearsSGPE.element.Returns(cGearsSG);
					EquipmentSet eSet = MakeEquipmentSet();
					eSet.Initialize(bowSGPE, wearSGPE, cGearsSGPE);
					return eSet;
				}
				public ISlotSystemPageElement MakeESetPEWInitWithSGs(){
					ISlotSystemPageElement eSetPE = MakeSubPageElement();
					EquipmentSet eSet = MakeEquipmentSetInitWithSGs();
					eSetPE.element.Returns(eSet);
					return eSetPE;
				}
		}
	}
}
