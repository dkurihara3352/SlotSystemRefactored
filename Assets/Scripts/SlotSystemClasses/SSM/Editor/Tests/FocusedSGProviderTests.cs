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
		public class FocusedSGProviderTests : SlotSystemTest{
			[Test]
			public void FocusedSGP_Always_ReturnsPoolBundleFocusedElement(){
				ISlotSystemManager ssm = MakeSubSSM();
					ISlotSystemBundle pBun = MakeSubBundle();
						ISlotGroup focusedSG = MakeSubSGWithEmptySBs();
							pBun.focusedElement.Returns(focusedSG);
					ssm.poolBundle.Returns(pBun);
					ssm.equipBundle.Returns(MakeSubBundle());
					ssm.otherBundles.Returns(new ISlotSystemBundle[]{});
				FocusedSGProvider focSGPrv = new FocusedSGProvider(ssm);
				
				ISlotGroup actual = focSGPrv.focusedSGP;

				Assert.That(actual, Is.SameAs(focusedSG));
			}
			[Test]
			public void FocusedEpSet_EquipBundleIsToggledOn_ReturnsEquipBundleFocusedElement(){
				ISlotSystemManager ssm = MakeSubSSM();
					ISlotSystemBundle eBun = MakeSubBundle();
					IEquipmentSet stubFocusedESet = Substitute.For<IEquipmentSet>();
						eBun.focusedElement.Returns(stubFocusedESet);
					ssm.poolBundle.Returns(MakeSubBundle());
					ssm.equipBundle.Returns(eBun);
					ssm.otherBundles.Returns(new ISlotSystemBundle[]{});
				FocusedSGProvider focSGPrv = new FocusedSGProvider(ssm);
				IEquipmentSet actual = focSGPrv.focusedEqSet;

				Assert.That(actual, Is.SameAs(stubFocusedESet));
			}
			[Test]
			public void FocusedSGEs_Always_ReturnsFocusedESetSGs(){
				ISlotSystemManager ssm = MakeSubSSM();
					ISlotSystemBundle eBun = MakeSubBundle();
						IEquipmentSet focusedESet = Substitute.For<IEquipmentSet>();
							IEnumerable<ISlotSystemElement> focusedESetEles;
								ISlotGroup sgeA = MakeSubSGWithEmptySBs();
								ISlotGroup sgeB = MakeSubSGWithEmptySBs();
								ISlotGroup sgeC = MakeSubSGWithEmptySBs();
								focusedESetEles = new ISlotSystemElement[]{
									sgeA, sgeB, sgeC
								};
							focusedESet.GetEnumerator().Returns(focusedESetEles.GetEnumerator());
						eBun.focusedElement.Returns(focusedESet);
					ssm.poolBundle.Returns(MakeSubBundle());
					ssm.equipBundle.Returns(eBun);
					ssm.otherBundles.Returns(new ISlotSystemBundle[]{});
				FocusedSGProvider focSGPrv = new FocusedSGProvider(ssm);

				IEnumerable<ISlotGroup> actual = focSGPrv.focusedSGEs;
				IEnumerable<ISlotSystemElement> convertedActual = ConvertToSSEs(actual);
				Assert.That(convertedActual.MemberEquals<ISlotSystemElement>(focusedESetEles), Is.True);
			}
			[TestCaseSource(typeof(AddFocusedTo_VariousConfigCases))]
			public void AddFocusedSGTo_VariousConfig_ReturnsAccordingly(ISlotSystemElement ele, IEnumerable<ISlotGroup> expected){
				FocusedSGProvider focusedSGProvider = new FocusedSGProvider(MakeSubSSM());
				List<ISlotGroup> list = new List<ISlotGroup>();
				focusedSGProvider.AddFocusedSGTo(ele, list);

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
			public void focusedSGGs_Always_ReturnsAllFocusedSGsInOtherBundles(){
				ISlotSystemManager ssm = MakeSubSSM();
					IEnumerable<ISlotSystemBundle> gBuns;
						SlotSystemBundle gBun = MakeSSBundleWithSelStateHandler();//non sub
							SlotGroup sggA = MakeSG();
								sggA.SetSelStateHandler(new SGSelStateHandler(MakeSubTAC(), Substitute.For<IHoverable>()));
								sggA.SetSBHandler(new SBHandler());
								sggA.transform.SetParent(gBun.transform);
								sggA.SetElements(new ISlotSystemElement[]{});
								sggA.Focus();
							SlotGroup sggB = MakeSG();
								sggB.SetSelStateHandler(new SGSelStateHandler(MakeSubTAC(), Substitute.For<IHoverable>()));
								sggB.SetSBHandler(new SBHandler());
								sggB.SetElements(new ISlotSystemElement[]{});
								sggB.transform.SetParent(gBun.transform);
								sggB.Focus();
							SlotGroup sggC = MakeSG();
								sggC.SetSelStateHandler(new SGSelStateHandler(MakeSubTAC(), Substitute.For<IHoverable>()));
								sggC.SetSBHandler(new SBHandler());
								sggC.SetElements(new ISlotSystemElement[]{});
								sggC.transform.SetParent(gBun.transform);
								sggC.Focus();
							gBun.SetHierarchy();
							gBun.Focus();
						gBuns = new ISlotSystemBundle[]{gBun};
					ssm.poolBundle.Returns(MakeSubBundle());
					ssm.equipBundle.Returns(MakeSubBundle());
					ssm.otherBundles.Returns(gBuns);
				FocusedSGProvider focusedSGProvider = new FocusedSGProvider(ssm);
				List<ISlotGroup> expected = new List<ISlotGroup>(new ISlotGroup[]{sggA, sggB, sggC});

				List<ISlotGroup> actual = focusedSGProvider.focusedSGGs;

				Assert.That(actual, Is.EqualTo(expected));
			}
			[Test]
			public void focusedSGs_Always_ReturnsSumOfAllFocusedSGs(){
				ISlotSystemManager ssm = MakeSubSSM();
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
						SlotSystemBundle gBun = MakeSSBundleWithSelStateHandler();//non sub
							SlotGroup sggA = MakeSG();
								sggA.SetSelStateHandler(new SGSelStateHandler(MakeSubTAC(), Substitute.For<IHoverable>()));
								sggA.SetSBHandler(new SBHandler());
								sggA.transform.SetParent(gBun.transform);
								sggA.SetElements(new ISlotSystemElement[]{});
								sggA.Focus();
							SlotGroup sggB = MakeSG();
								sggB.SetSelStateHandler(new SGSelStateHandler(MakeSubTAC(), Substitute.For<IHoverable>()));
								sggB.SetSBHandler(new SBHandler());
								sggB.SetElements(new ISlotSystemElement[]{});
								sggB.transform.SetParent(gBun.transform);
								sggB.Focus();
							SlotGroup sggC = MakeSG();
								sggC.SetSelStateHandler(new SGSelStateHandler(MakeSubTAC(), Substitute.For<IHoverable>()));
								sggC.SetSBHandler(new SBHandler());
								sggC.SetElements(new ISlotSystemElement[]{});
								sggC.transform.SetParent(gBun.transform);
								sggC.Focus();
							gBun.SetHierarchy();
							gBun.Focus();
						gBuns = new ISlotSystemBundle[]{gBun};
					ssm.poolBundle.Returns(pBun);
					ssm.equipBundle.Returns(eBun);
					ssm.otherBundles.Returns(gBuns);
				FocusedSGProvider focusedSGProvider = new FocusedSGProvider(ssm);
				List<ISlotGroup> expected = new List<ISlotGroup>(new ISlotGroup[]{
					sgp, sgeBow, sgeWear, sgeCGears, sggA, sggB, sggC
				});

				IEnumerable<ISlotGroup> actual = focusedSGProvider.focusedSGs;
				
				Assert.That(actual, Is.EqualTo(expected));
			}
			[Test]
			public void equipInv_Always_ReturnsFocusedSGEsInventory(){
				ISlotSystemManager ssm = MakeSubSSM();
					ISlotSystemBundle eBun = MakeSubBundle();
						IEquipmentSet eSet = MakeSubESet();
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
						ssm.equipBundle.Returns(eBun);
				FocusedSGProvider focSGPrv = new FocusedSGProvider(ssm);
				
				IEquipmentSetInventory actual = focSGPrv.equipInv;

				Assert.That(actual, Is.SameAs(eInv));
			}
			[Test]
			public void poolInv_Always_ReturnsFocusedSGPsInventory(){
				FocusedSGProvider sgProvider;
					ISlotSystemManager ssm = MakeSubSSM();
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
						ssm.poolBundle.Returns(pBun);
						ssm.equipBundle.Returns(MakeSubBundle());
						ssm.otherBundles.Returns(new ISlotSystemBundle[]{});
					sgProvider = new FocusedSGProvider(ssm);

				IPoolInventory actual = sgProvider.poolInv;

				Assert.That(actual, Is.SameAs(pInv));
			}
			[Test]
			public void ChangeEquippableCGearsCount_TargetSGIsExpandable_ThrowsException(){
				FocusedSGProvider focSGProv = new FocusedSGProvider(MakeSubSSM());
				ISlotGroup sg = MakeSubSG();
				sg.isExpandable.Returns(true);

				Exception ex = Assert.Catch<InvalidOperationException>(()=> focSGProv.ChangeEquippableCGearsCount(0, sg));
				
				Assert.That(ex.Message, Is.StringContaining("ISlotGroupManager.ChangeEquippableCGearsCount: the targetSG is expandable"));
			}
			[TestCase(true)]
			[TestCase(false)]
			public void ChangeEquippableCGearsCount_TargetSGIsNOTExpandableAndSGIsFocusedOrDefocused_CallsEInvAndSGInOrder(bool focused){
				FocusedSGProvider stubFocSGProv;
					ISlotSystemManager stubSSM = MakeSubSSM();
						ISlotSystemBundle eBun = MakeSubBundle();
							IEquipmentSet eSet = Substitute.For<IEquipmentSet>();
								ISlotGroup sge = MakeSubSG();
									IEquipmentSetInventory eInv = MakeSubEquipInv();
									sge.inventory.Returns(eInv);
								IEnumerable<ISlotSystemElement> eSetEles = new ISlotSystemElement[]{
									sge
								};
							eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
						eBun.focusedElement.Returns(eSet);
					stubSSM.equipBundle.Returns(eBun);
				stubFocSGProv = new FocusedSGProvider(stubSSM);
				ISlotGroup sg = MakeSubSG();
					sg.isExpandable.Returns(false);
					sg.isFocused.Returns(focused);
					sg.isDefocused.Returns(!focused);

				stubFocSGProv.ChangeEquippableCGearsCount(0, sg);

				Received.InOrder(() => {
					eInv.SetEquippableCGearsCount(0);
					sg.InitializeItems();
				});
			}
		/* Test */
			SlotSystemBundle MakeSSBundleWithSelStateHandler(){
				SlotSystemBundle bundle = MakeSSBundle();
				SSESelStateHandler handler = new SSESelStateHandler();
				bundle.SetSelStateHandler(handler);
				return bundle;
			}
			public IEnumerable<ISlotSystemElement> ConvertToSSEs<T>(IEnumerable<T> sgs) where T: ISlotSystemElement{
				foreach(var sg in sgs)
					yield return (ISlotSystemElement)sg;
			}
		}
	}
}
