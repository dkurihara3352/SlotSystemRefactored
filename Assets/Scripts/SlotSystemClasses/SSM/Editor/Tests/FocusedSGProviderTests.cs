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
							pBun.GetFocusedElement().Returns(focusedSG);
					ssm.GetPoolBundle().Returns(pBun);
					ssm.GetEquipBundle().Returns(MakeSubBundle());
					ssm.GetOtherBundles().Returns(new ISlotSystemBundle[]{});
				FocusedSGProvider focSGPrv = new FocusedSGProvider(ssm);
				
				ISlotGroup actual = focSGPrv.focusedSGP;

				Assert.That(actual, Is.SameAs(focusedSG));
			}
			[Test]
			public void FocusedEpSet_EquipBundleIsToggledOn_ReturnsEquipBundleFocusedElement(){
				ISlotSystemManager ssm = MakeSubSSM();
					ISlotSystemBundle eBun = MakeSubBundle();
					IEquipmentSet stubFocusedESet = Substitute.For<IEquipmentSet>();
						eBun.GetFocusedElement().Returns(stubFocusedESet);
					ssm.GetPoolBundle().Returns(MakeSubBundle());
					ssm.GetEquipBundle().Returns(eBun);
					ssm.GetOtherBundles().Returns(new ISlotSystemBundle[]{});
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
						eBun.GetFocusedElement().Returns(focusedESet);
					ssm.GetPoolBundle().Returns(MakeSubBundle());
					ssm.GetEquipBundle().Returns(eBun);
					ssm.GetOtherBundles().Returns(new ISlotSystemBundle[]{});
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
							ISlotSystemElement notSG = MakeSubSSE();
								ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
								selStateHandler.IsFocused().Returns(false);
								notSG.GetSelStateHandler().Returns(selStateHandler);
							yield return new TestCaseData(notSG, new ISlotGroup[]{}).SetName("NoSG");
							ISlotGroup notFocused = MakeSubSG();
							notFocused.IsFocusedInHierarchy().Returns(false);
							yield return new TestCaseData(notFocused, new ISlotGroup[]{}).SetName("NotFocusedInHierarchy");
							ISlotGroup validSG = MakeSubSGWithEmptySBs();
							validSG.IsFocusedInHierarchy().Returns(true);
							yield return new TestCaseData(validSG, new ISlotGroup[]{validSG}).SetName("Valid");
					}
			}
			[Test]
			public void focusedSGGs_Always_ReturnsAllFocusedSGsInOtherBundles(){
				ISlotSystemManager ssm = MakeSubSSM();
					IEnumerable<ISlotSystemBundle> gBuns;
						SlotSystemBundle gBun = MakeSSBundleWithSelStateHandler();//non sub
							ISSESelStateHandler gBunSelStateHandler = Substitute.For<ISSESelStateHandler>();
							gBunSelStateHandler.IsFocused().Returns(true);
						gBun.SetSelStateHandler(gBunSelStateHandler);
							SlotGroup sggA = MakeSG();
								ISSESelStateHandler sggASelStateHandler = Substitute.For<ISSESelStateHandler>();
									sggASelStateHandler.IsFocused().Returns(true);
								sggA.SetSelStateHandler(sggASelStateHandler);
								sggA.SetSBHandler(new SBHandler());
								sggA.transform.SetParent(gBun.transform);
								sggA.SetElements(new ISlotSystemElement[]{});
							SlotGroup sggB = MakeSG();
								ISSESelStateHandler sggBSelStateHandler = Substitute.For<ISSESelStateHandler>();
									sggBSelStateHandler.IsFocused().Returns(true);
								sggB.SetSelStateHandler(sggBSelStateHandler);
								sggB.SetSBHandler(new SBHandler());
								sggB.SetElements(new ISlotSystemElement[]{});
								sggB.transform.SetParent(gBun.transform);
							SlotGroup sggC = MakeSG();
								ISSESelStateHandler sggCSelStateHandler = Substitute.For<ISSESelStateHandler>();
									sggCSelStateHandler.IsFocused().Returns(true);
								sggC.SetSelStateHandler(sggCSelStateHandler);
								sggC.SetSBHandler(new SBHandler());
								sggC.SetElements(new ISlotSystemElement[]{});
								sggC.transform.SetParent(gBun.transform);
							gBun.SetHierarchy();
						gBuns = new ISlotSystemBundle[]{gBun};
					ssm.GetPoolBundle().Returns(MakeSubBundle());
					ssm.GetEquipBundle().Returns(MakeSubBundle());
					ssm.GetOtherBundles().Returns(gBuns);
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
						pBun.GetFocusedElement().Returns(sgp);
					ISlotSystemBundle eBun = MakeSubBundle();
						IEquipmentSet eSet = MakeSubESet();
							ISlotGroup sgeBow = MakeSubSG();
							ISlotGroup sgeWear = MakeSubSG();
							ISlotGroup sgeCGears = MakeSubSG();
							IEnumerable<ISlotSystemElement> eSetEles = new ISlotSystemElement[]{sgeBow, sgeWear, sgeCGears};
							eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
						eBun.GetFocusedElement().Returns(eSet);
					IEnumerable<ISlotSystemBundle> gBuns;
						SlotSystemBundle gBun = MakeSSBundleWithSelStateHandler();//non sub
							ISSESelStateHandler gBunSelStateHandler = Substitute.For<ISSESelStateHandler>();
							gBunSelStateHandler.IsFocused().Returns(true);
						gBun.SetSelStateHandler(gBunSelStateHandler);
							SlotGroup sggA = MakeSG();
								ISSESelStateHandler sggASelStateHandler = Substitute.For<ISSESelStateHandler>();
									sggASelStateHandler.IsFocused().Returns(true);
								sggA.SetSelStateHandler(sggASelStateHandler);
								sggA.SetSBHandler(new SBHandler());
								sggA.transform.SetParent(gBun.transform);
								sggA.SetElements(new ISlotSystemElement[]{});
							SlotGroup sggB = MakeSG();
								ISSESelStateHandler sggBSelStateHandler = Substitute.For<ISSESelStateHandler>();
									sggBSelStateHandler.IsFocused().Returns(true);
								sggB.SetSelStateHandler(sggBSelStateHandler);
								sggB.SetSBHandler(new SBHandler());
								sggB.SetElements(new ISlotSystemElement[]{});
								sggB.transform.SetParent(gBun.transform);
							SlotGroup sggC = MakeSG();
								ISSESelStateHandler sggCSelStateHandler = Substitute.For<ISSESelStateHandler>();
									sggCSelStateHandler.IsFocused().Returns(true);
								sggC.SetSelStateHandler(sggCSelStateHandler);
								sggC.SetSBHandler(new SBHandler());
								sggC.SetElements(new ISlotSystemElement[]{});
								sggC.transform.SetParent(gBun.transform);
							gBun.SetHierarchy();
						gBuns = new ISlotSystemBundle[]{gBun};
					ssm.GetPoolBundle().Returns(MakeSubBundle());
					ssm.GetEquipBundle().Returns(MakeSubBundle());
					ssm.GetOtherBundles().Returns(gBuns);
					ssm.GetPoolBundle().Returns(pBun);
					ssm.GetEquipBundle().Returns(eBun);
					ssm.GetOtherBundles().Returns(gBuns);
				FocusedSGProvider focusedSGProvider = new FocusedSGProvider(ssm);
				List<ISlotGroup> expected = new List<ISlotGroup>(new ISlotGroup[]{
					sgp, sgeBow, sgeWear, sgeCGears, sggA, sggB, sggC
				});

				IEnumerable<ISlotGroup> actual = focusedSGProvider.GetFocusedSGs();
				
				Assert.That(actual, Is.EqualTo(expected));
			}
			[Test]
			public void equipInv_Always_ReturnsFocusedSGEsInventory(){
				ISlotSystemManager ssm = MakeSubSSM();
					ISlotSystemBundle eBun = MakeSubBundle();
						IEquipmentSet eSet = MakeSubESet();
						eBun.GetFocusedElement().Returns(eSet);
							IEnumerable<ISlotSystemElement> eSetEles;
								ISlotGroup sgeA = MakeSubSGWithEmptySBs();
									IEquipmentSetInventory eInv = MakeSubEquipInv();
									sgeA.GetInventory().Returns(eInv);
								ISlotGroup sgeB = MakeSubSGWithEmptySBs();
								ISlotGroup sgeC = MakeSubSGWithEmptySBs();
								eSetEles = new ISlotSystemElement[]{
									sgeA, sgeB, sgeC
								};
							eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
						ssm.GetEquipBundle().Returns(eBun);
				FocusedSGProvider focSGPrv = new FocusedSGProvider(ssm);
				
				IEquipmentSetInventory actual = focSGPrv.GetEquipInv();

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
								sgpA.GetInventory().Returns(pInv);
							pBun.GetFocusedElement().Returns(sgpA);
						ssm.GetPoolBundle().Returns(pBun);
						ssm.GetEquipBundle().Returns(MakeSubBundle());
						ssm.GetOtherBundles().Returns(new ISlotSystemBundle[]{});
					sgProvider = new FocusedSGProvider(ssm);

				IPoolInventory actual = sgProvider.GetPoolInv();

				Assert.That(actual, Is.SameAs(pInv));
			}
			[Test]
			public void ChangeEquippableCGearsCount_TargetSGIsExpandable_ThrowsException(){
				FocusedSGProvider focSGProv = new FocusedSGProvider(MakeSubSSM());
				ISlotGroup sg = MakeSubSG();
				sg.IsExpandable().Returns(true);

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
									sge.GetInventory().Returns(eInv);
								IEnumerable<ISlotSystemElement> eSetEles = new ISlotSystemElement[]{
									sge
								};
							eSet.GetEnumerator().Returns(eSetEles.GetEnumerator());
						eBun.GetFocusedElement().Returns(eSet);
					stubSSM.GetEquipBundle().Returns(eBun);
				stubFocSGProv = new FocusedSGProvider(stubSSM);
				ISlotGroup sg = MakeSubSG();
					sg.IsExpandable().Returns(false);
					ISSESelStateHandler sgSelStateHandler = Substitute.For<ISSESelStateHandler>();
						sgSelStateHandler.IsFocused().Returns(focused);
						sgSelStateHandler.IsDefocused().Returns(!focused);
					sg.GetSelStateHandler().Returns(sgSelStateHandler);

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
