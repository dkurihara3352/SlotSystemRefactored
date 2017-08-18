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

				Assert.That(ssm.GetPoolBundle(), Is.SameAs(pBun));
				Assert.That(ssm.GetEquipBundle(), Is.SameAs(eBun));
				bool equality = ssm.GetOtherBundles().MemberEquals(gBuns);
				Assert.That(equality, Is.True);
				}
			[Test]
			public void InspectorSetUp_Always_SetsTAM(){
				ITransactionManager stubTAM = MakeSubTAM();
				SlotSystemManager ssm  = MakeSSM();

				ssm.InspectorSetUp(MakeSubBundle(), MakeSubBundle(), new ISlotSystemBundle[]{}, stubTAM);

				Assert.That(ssm.GetTAM(), Is.SameAs(stubTAM));
			}
			[Test]
			public void Initialize_WhenCalled_CallsSetSSMInHierarchy(){
				SlotSystemManager ssm = MakeSSM_selStateHandler();
					ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
				ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
				ssm.SetHierarchy();
					IFocusedSGProvider stubFocSGProv = Substitute.For<IFocusedSGProvider>();
				ssm.SetFocusedSGProvider(stubFocSGProv);
				
				ssm.Initialize();
				
				Assert.That(ssm.GetSSM(), Is.SameAs(ssm));
				pBun.Received().PerformInHierarchy(ssm.SetSSMInH);
				eBun.Received().PerformInHierarchy(ssm.SetSSMInH);
				foreach(var gBun in gBuns)
					gBun.Received().PerformInHierarchy(ssm.SetSSMInH);
				}
			[Test]
			public void Initialize_WhenCalled_CallsPIHInitializeState(){
				SlotSystemManager ssm = MakeSSM();
					ISSESelStateHandler ssmSelStateHandler = Substitute.For<ISSESelStateHandler>();
					ssm.SetSelStateHandler(ssmSelStateHandler);
					ISlotSystemBundle pBun = MakeSubBundle();
						ISlotSystemBundle eBun = MakeSubBundle();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle gBunA = MakeSubBundle();
							ISlotSystemBundle gBunB = MakeSubBundle();
							ISlotSystemBundle gBunC = MakeSubBundle();
							gBuns = new ISlotSystemBundle[]{gBunA, gBunB, gBunC};	
				ssm.InspectorSetUp(pBun, eBun, gBuns, MakeSubTAM());
				ssm.SetHierarchy();
					IFocusedSGProvider stubFocSGProv = Substitute.For<IFocusedSGProvider>();
				ssm.SetFocusedSGProvider(stubFocSGProv);

				ssm.Initialize();

				ssmSelStateHandler.Received().Deactivate();
				pBun.Received().PerformInHierarchy(ssm.InitStatesInHi);
				eBun.Received().PerformInHierarchy(ssm.InitStatesInHi);
				foreach(var gBun in gBuns)
					gBun.Received().PerformInHierarchy(ssm.InitStatesInHi);
				}
			/*	fields */
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
			/*	methods	*/
				[Test]
				public void UpdateEquipInvAndAllSBsEquipState_Always_CallsEInvRemoveWithItemNotInAllEquippedItems(){
					SlotSystemManager ssm = MakeSSM();
					IFocusedSGProvider stubSGProv = Substitute.For<IFocusedSGProvider>();
							IEquipmentSetInventory eInv = MakeSubEquipInv();
								IEnumerable<IInventoryItemInstance> eInvEles;
									BowInstance bowR = MakeBowInstance(0);
									WearInstance wearR = MakeWearInstance(0);
									QuiverInstance quiverR = MakeQuiverInstance(0);
									PackInstance packR = MakePackInstance(0);
									eInvEles = new IInventoryItemInstance[]{
										bowR, wearR, quiverR, packR
									};
								eInv.GetItems().Returns(new List<IInventoryItemInstance>(eInvEles));
							stubSGProv.GetEquipInv().Returns(eInv);
							stubSGProv.GetPoolInv().Returns(new PoolInventory());
						ssm.SetFocusedSGProvider(stubSGProv);
					IEquippedProvider stubEquiProv = Substitute.For<IEquippedProvider>();
							BowInstance bowE = MakeBowInstance(0);
							WearInstance wearE = MakeWearInstance(0);
							List<IInventoryItemInstance> allEquippedItems = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
								bowE, wearE
							});
							stubEquiProv.GetAllEquippedItems().Returns(allEquippedItems);
						ssm.SetEquippedProvider(stubEquiProv);
					IAllElementsProvider allEProv = Substitute.For<IAllElementsProvider>();
						allEProv.GetAllSBs().Returns(new List<ISlottable>());
						ssm.SetAllElementsProvider(allEProv);

					ssm.UpdateEquipInvAndAllSBsEquipState();

					eInv.Received().Remove(bowR);
					eInv.Received().Remove(wearR);
					eInv.Received().Remove(quiverR);
					eInv.Received().Remove(packR);
				}
				[Test]
				public void UpdateEquipInvAndAllSBsEquipState_Always_CallsEInvAddWithItemNotInEquipInv(){
					SlotSystemManager ssm = MakeSSM();
					IFocusedSGProvider stubSGProv = Substitute.For<IFocusedSGProvider>();
							IEquipmentSetInventory eInv = MakeSubEquipInv();
								IEnumerable<IInventoryItemInstance> eInvEles = new IInventoryItemInstance[]{};
								eInv.GetItems().Returns(new List<IInventoryItemInstance>(eInvEles));
									BowInstance bowA = MakeBowInstance(0);
									WearInstance wearA = MakeWearInstance(0);
									QuiverInstance quiverA = MakeQuiverInstance(0);
									PackInstance packA = MakePackInstance(0);
							stubSGProv.GetEquipInv().Returns(eInv);
							stubSGProv.GetPoolInv().Returns(new PoolInventory());
						ssm.SetFocusedSGProvider(stubSGProv);
					IEquippedProvider stubEquiProv = Substitute.For<IEquippedProvider>();
							List<IInventoryItemInstance> allEquippedItems = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
								bowA, wearA, quiverA, packA
							});
							stubEquiProv.GetAllEquippedItems().Returns(allEquippedItems);
						ssm.SetEquippedProvider(stubEquiProv);
					IAllElementsProvider allEProv = Substitute.For<IAllElementsProvider>();
						allEProv.GetAllSBs().Returns(new List<ISlottable>());
						ssm.SetAllElementsProvider(allEProv);

					ssm.UpdateEquipInvAndAllSBsEquipState();

					eInv.Received().Add(bowA);
					eInv.Received().Add(wearA);
					eInv.Received().Add(quiverA);
					eInv.Received().Add(packA);
				}
				[Test]
				public void UpdateEquipInvAndAllSBsEquipState_Always_UpdatePoolInvItemInstsEquipStatus(){
					SlotSystemManager ssm = MakeSSM();
					IFocusedSGProvider stubSGProv = Substitute.For<IFocusedSGProvider>();
							IEquipmentSetInventory eInv = MakeSubEquipInv();
								IEnumerable<IInventoryItemInstance> eInvEles;
									BowInstance bowR = MakeBowInstance(0);
									WearInstance wearR = MakeWearInstance(0);
									QuiverInstance quiverR = MakeQuiverInstance(0);
									PackInstance packR = MakePackInstance(0);
									BowInstance bowA = MakeBowInstance(0);
									WearInstance wearA = MakeWearInstance(0);
									QuiverInstance quiverA = MakeQuiverInstance(0);
									PackInstance packA = MakePackInstance(0);
									eInvEles = new IInventoryItemInstance[]{
										bowR,
										wearR,
										quiverR,
										packR,
									};
								eInv.GetItems().Returns(new List<IInventoryItemInstance>(eInvEles));
							stubSGProv.GetEquipInv().Returns(eInv);
							IPoolInventory pInv = MakeSubPoolInv();
								IEnumerable<IInventoryItemInstance> pInvEles = new IInventoryItemInstance[]{
									bowR,
									wearR,
									quiverR,
									packR,
									bowA,
									wearA,
									quiverA,
									packA
								};
								pInv.GetItems().Returns(new List<IInventoryItemInstance>(pInvEles));
							stubSGProv.GetPoolInv().Returns(pInv);
						ssm.SetFocusedSGProvider(stubSGProv);
					IEquippedProvider stubEquiProv = Substitute.For<IEquippedProvider>();
							List<IInventoryItemInstance> allEquippedItems = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
								bowA, wearA, quiverA, packA
							});
							stubEquiProv.GetAllEquippedItems().Returns(allEquippedItems);
							stubEquiProv.GetEquippedBowInst().Returns(bowA);
							stubEquiProv.GetEquippedWearInst().Returns(wearA);
							List<CarriedGearInstance> equippedCGears = new List<CarriedGearInstance>(new CarriedGearInstance[]{
								quiverA, packA
							});
							stubEquiProv.GetEquippedCarriedGears().Returns(equippedCGears);
						ssm.SetEquippedProvider(stubEquiProv);
					IAllElementsProvider allEProv = Substitute.For<IAllElementsProvider>();
						allEProv.GetAllSBs().Returns(new List<ISlottable>());
						ssm.SetAllElementsProvider(allEProv);

					ssm.UpdateEquipInvAndAllSBsEquipState();

					bool flag = true;
					flag &= bowA.GetIsEquipped() == true;
					flag &= wearA.GetIsEquipped() == true;
					flag &= quiverA.GetIsEquipped() == true;
					flag &= packA.GetIsEquipped() == true;
					flag &= bowR.GetIsEquipped() == false;
					flag &= wearR.GetIsEquipped() == false;
					flag &= quiverR.GetIsEquipped() == false;
					flag &= packR.GetIsEquipped() == false;

					Assert.That(flag, Is.True);
				}
				[Test]
				public void RemoveFromEquipInv_Always_CallsEInvRemoveMatches(){
					SlotSystemManager ssm = MakeSSM();
						IFocusedSGProvider sgProvider = Substitute.For<IFocusedSGProvider>();
							IEquipmentSetInventory mockEInv = Substitute.For<IEquipmentSetInventory>();
								IEnumerable<IInventoryItemInstance> eInvEles;
									BowInstance bowE = MakeBowInstance(0);
									WearInstance wearE = MakeWearInstance(0);
									ShieldInstance shieldE = MakeShieldInstance(0);
									MeleeWeaponInstance mWeaponE = MakeMWeaponInstance(0);
									BowInstance bowR = MakeBowInstance(0);
									WearInstance wearR = MakeWearInstance(0);
									QuiverInstance quiverR = MakeQuiverInstance(0);
									PackInstance packR = MakePackInstance(0);
									eInvEles = new IInventoryItemInstance[]{
										bowE,
										wearE,
										shieldE,
										mWeaponE,
										bowR,
										wearR,
										quiverR,
										packR
									};
								mockEInv.GetItems().Returns(new List<IInventoryItemInstance>(eInvEles));
							sgProvider.GetEquipInv().Returns(mockEInv);
						ssm.SetFocusedSGProvider(sgProvider);
						IEquippedProvider stubEquiProv = Substitute.For<IEquippedProvider>();
							List<IInventoryItemInstance> allEquippedItems = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
								bowE, wearE, shieldE, mWeaponE
							});
							stubEquiProv.GetAllEquippedItems().Returns(allEquippedItems);
						ssm.SetEquippedProvider(stubEquiProv);
						IAllElementsProvider allEProv = Substitute.For<IAllElementsProvider>();
						ssm.SetAllElementsProvider(allEProv);
					
					ssm.RemoveFromEquipInv();

					mockEInv.Received().Remove(bowR);
					mockEInv.Received().Remove(wearR);
					mockEInv.Received().Remove(quiverR);
					mockEInv.Received().Remove(packR);
					mockEInv.DidNotReceive().Remove(bowE);
					mockEInv.DidNotReceive().Remove(wearE);
					mockEInv.DidNotReceive().Remove(shieldE);
					mockEInv.DidNotReceive().Remove(mWeaponE);
				}
				[Test]
				public void AddToEquipInv_Always_CallsEInvAddMathes(){
					SlotSystemManager ssm = MakeSSM();
						IFocusedSGProvider sgProvider = Substitute.For<IFocusedSGProvider>();
							IEquipmentSetInventory mockEInv = Substitute.For<IEquipmentSetInventory>();
								IEnumerable<IInventoryItemInstance> eInvEles;
									eInvEles = new IInventoryItemInstance[]{};
								mockEInv.GetItems().Returns(new List<IInventoryItemInstance>(eInvEles));
							sgProvider.GetEquipInv().Returns(mockEInv);
						ssm.SetFocusedSGProvider(sgProvider);
						IEquippedProvider stubEquiProv = Substitute.For<IEquippedProvider>();
								List<IInventoryItemInstance> allEquippedItems;
									BowInstance bowA = MakeBowInstance(0);
									WearInstance wearA = MakeWearInstance(0);
									ShieldInstance shieldA = MakeShieldInstance(0);
									MeleeWeaponInstance mWeaponA = MakeMWeaponInstance(0);
								allEquippedItems = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
									bowA, wearA, shieldA, mWeaponA
								});
							stubEquiProv.GetAllEquippedItems().Returns(allEquippedItems);
						ssm.SetEquippedProvider(stubEquiProv);
						IAllElementsProvider allEProv = Substitute.For<IAllElementsProvider>();
						ssm.SetAllElementsProvider(allEProv);
					
					ssm.AddToEquipInv();

					mockEInv.Received().Add(bowA);
					mockEInv.Received().Add(wearA);
					mockEInv.Received().Add(shieldA);
					mockEInv.Received().Add(mWeaponA);
				}
				[Test]
				public void UpdateAllItemsEquipStatusInPoolInv_Always_CallsItemsInPoolInvAccordingly(){
					SlotSystemManager ssm = MakeSSM();
						IEquippedProvider equipProv = Substitute.For<IEquippedProvider>();
							BowInstance bowE = MakeBowInstance(0);
							WearInstance wearE = MakeWearInstance(0);
							ShieldInstance shieldE = MakeShieldInstance(0);
							MeleeWeaponInstance mWeaponE = MakeMWeaponInstance(0);
							PartsInstance partsE = MakePartsInstance(0, 20);
							equipProv.GetEquippedBowInst().Returns(bowE);
							equipProv.GetEquippedWearInst().Returns(wearE);
							equipProv.GetEquippedCarriedGears().Returns(new List<CarriedGearInstance>(new CarriedGearInstance[]{
								shieldE, mWeaponE
							}));
							equipProv.GetEquippedParts().Returns(new List<PartsInstance>(new PartsInstance[]{
								partsE
							}));
						ssm.SetEquippedProvider(equipProv);
						IPoolInventory stubPInv = Substitute.For<IPoolInventory>();
							IEnumerable<IInventoryItemInstance> pInvEles;
								BowInstance bowU = MakeBowInstance(0);
								WearInstance wearU = MakeWearInstance(0);
								QuiverInstance quiverU = MakeQuiverInstance(0);
								PackInstance packU = MakePackInstance(0);
								PartsInstance partsU = MakePartsInstance(1, 2);
								pInvEles = new IInventoryItemInstance[]{
									bowE,
									wearE,
									shieldE,
									mWeaponE,
									partsE,
									bowU,
									wearU,
									quiverU,
									packU,
									partsU
								};
							stubPInv.GetItems().Returns(new List<IInventoryItemInstance>(pInvEles));
						IFocusedSGProvider stubFocSGProv = Substitute.For<IFocusedSGProvider>();
							stubFocSGProv.GetPoolInv().Returns(stubPInv);
						ssm.SetFocusedSGProvider(stubFocSGProv);
						IAllElementsProvider stubAllEProv = Substitute.For<IAllElementsProvider>();
						ssm.SetAllElementsProvider(stubAllEProv);
					
					ssm.UpdateAllItemsEquipStatusInPoolInv();

					Assert.That(bowE.GetIsEquipped(), Is.True);
					Assert.That(wearE.GetIsEquipped(), Is.True);
					Assert.That(shieldE.GetIsEquipped(), Is.True);
					Assert.That(mWeaponE.GetIsEquipped(), Is.True);
					Assert.That(partsE.GetIsEquipped(), Is.True);
					Assert.That(bowU.GetIsEquipped(), Is.False);
					Assert.That(wearU.GetIsEquipped(), Is.False);
					Assert.That(quiverU.GetIsEquipped(), Is.False);
					Assert.That(packU.GetIsEquipped(), Is.False);
					Assert.That(partsU.GetIsEquipped(), Is.False);
				}
				[Test]
				public void UpdateAllSBsEquipState(){
					SlotSystemManager ssm = MakeSSM();
						IAllElementsProvider stubAllEProv = Substitute.For<IAllElementsProvider>();
							List<ISlottable> allSBs;
								ISlottable sbA = MakeSubSB();
								ISlottable sbB = MakeSubSB();
								ISlottable sbC = MakeSubSB();
							allSBs = new List<ISlottable>(new ISlottable[]{
								sbA, sbB, sbC
							});
							stubAllEProv.GetAllSBs().Returns(allSBs);
						ssm.SetAllElementsProvider(stubAllEProv);
					
					ssm.UpdateAllSBsEquipState();

					sbA.Received().UpdateEquipState();
					sbB.Received().UpdateEquipState();
					sbC.Received().UpdateEquipState();
				}
				[TestCaseSource(typeof(MarkEquippedInPoolCases))]
				public void MarkEquippedInPool_WhenCalled_FindItemInPoolAndSetsIsEquippedAccordingly(
					IEnumerable<IInventoryItemInstance> items, 
					IInventoryItemInstance item, 
					bool equipped, 
					List<IInventoryItemInstance> xEquipped)
				{
					SlotSystemManager ssm = MakeSSM();
						IPoolInventory pInv = MakeSubPoolInv();
							pInv.GetItems().Returns(new List<IInventoryItemInstance>(items));
						IFocusedSGProvider stubFocSGProv = Substitute.For<IFocusedSGProvider>();
							stubFocSGProv.GetPoolInv().Returns(pInv);
						ssm.SetFocusedSGProvider(stubFocSGProv);

					ssm.MarkEquippedInPool(item, equipped);

					foreach(IInventoryItemInstance it in pInv.GetItems())
						if(xEquipped.Contains(it))
							Assert.That(it.GetIsEquipped(), Is.True);
						else
						 	Assert.That(it.GetIsEquipped(), Is.False);
					}
					class MarkEquippedInPoolCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case0;
								BowInstance bow_0 = MakeBowInstance(0);
								WearInstance wear_0 = MakeWearInstance(0);
								ShieldInstance shield_0 = MakeShieldInstance(0);
								MeleeWeaponInstance mWeapon_0 = MakeMWeaponInstance(0);
								QuiverInstance quiver_0 = MakeQuiverInstance(0);
								PackInstance pack_0 = MakePackInstance(0);
								PartsInstance parts_0 = MakePartsInstance(0, 2);
								IEnumerable<IInventoryItemInstance> items_0 = new IInventoryItemInstance[]{
									bow_0, 
									wear_0, 
									shield_0, 
									mWeapon_0, 
									quiver_0, 
									pack_0, 
									parts_0
								};
								IEnumerable<IInventoryItemInstance> equipped_0 = new IInventoryItemInstance[]{
									wear_0, 
									shield_0, 
									mWeapon_0, 
									quiver_0, 
									pack_0, 
									parts_0
								};
									foreach(var sItem in equipped_0)
										((IInventoryItemInstance)sItem).SetIsEquipped(true);
								List<IInventoryItemInstance> xEquipped_0 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
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
								MeleeWeaponInstance mWeapon_1 = MakeMWeaponInstance(0);
								QuiverInstance quiver_1 = MakeQuiverInstance(0);
								PackInstance pack_1 = MakePackInstance(0);
								PartsInstance parts_1 = MakePartsInstance(0, 2);
								IEnumerable<IInventoryItemInstance> items_1 = new IInventoryItemInstance[]{
									bow_1, 
									wear_1, 
									shield_1, 
									mWeapon_1, 
									quiver_1, 
									pack_1, 
									parts_1
								};
								IEnumerable<IInventoryItemInstance> equipped_1 = new IInventoryItemInstance[]{
									bow_1
								};
									foreach(var sItem in equipped_1)
										((IInventoryItemInstance)sItem).SetIsEquipped(true);
								List<IInventoryItemInstance> xEquipped_1 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
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
						stubSG.IsFocusedInHierarchy().Returns(true);
					ISlottable mockSB = MakeSubSB();
						mockSB.GetSG().Returns(stubSG);
						mockSB.GetItem().Returns(bow);
						mockSB.GetNewSlotID().Returns(0);
					
					ssm.Equip(mockSB, bow);

					mockSB.Received().Equip();
				}
				[Test]
				public void Equip_MatchesAndSGNOTFocusedInBundleAndSGIsPool_CallsSBInOrder(){
					SlotSystemManager ssm = MakeSSM();
					BowInstance bow = MakeBowInstance(0);
					ISlotGroup stubSG = MakeSubSG();
						stubSG.IsFocusedInHierarchy().Returns(false);
					ISlottable mockSB = MakeSubSB();
						mockSB.GetSG().Returns(stubSG);
						mockSB.GetItem().Returns(bow);
						mockSB.IsPool().Returns(true);
					
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
						stubSG.IsFocusedInHierarchy().Returns(true);
					ISlottable mockSB = MakeSubSB();
						mockSB.GetSG().Returns(stubSG);
						mockSB.GetItem().Returns(bow);
						mockSB.GetSlotID().Returns(0);
					
					ssm.Unequip(mockSB, bow);

					mockSB.Received().Unequip();
				}
				[Test]
				public void Unequip_MatchesAndSGNOTFocusedInBundleAndSGIsPool_CallsSBInOrder(){
					SlotSystemManager ssm = MakeSSM();
					BowInstance bow = MakeBowInstance(0);
					ISlotGroup stubSG = MakeSubSG();
						stubSG.IsFocusedInHierarchy().Returns(false);
					ISlottable mockSB = MakeSubSB();
						mockSB.GetSG().Returns(stubSG);
						mockSB.GetItem().Returns(bow);
						mockSB.IsPool().Returns(true);
					
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
								stubSGP.GetInventory().Returns(stubPInv);
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

					Assert.That(ssm.GetPoolBundle(), Is.SameAs(pBun));
					Assert.That(ssm.GetEquipBundle(), Is.SameAs(eBun));
					Assert.That(ssm.GetOtherBundles(), Is.EqualTo(xGBuns));

					Assert.That(pBun.GetParent(), Is.SameAs(ssm));
					Assert.That(eBun.GetParent(), Is.SameAs(ssm));
					Assert.That(gBunA.GetParent(), Is.SameAs(ssm));
					Assert.That(gBunB.GetParent(), Is.SameAs(ssm));
				}
			/*	helper	*/
				public IEnumerable<ISlotSystemElement> ConvertToSSEs<T>(IEnumerable<T> sgs) where T: ISlotSystemElement{
					foreach(var sg in sgs)
						yield return (ISlotSystemElement)sg;
				}
				SlotSystemManager MakeSSM_selStateHandler(){
					SlotSystemManager ssm = MakeSSM();
					SSESelStateHandler selStateHandler = new SSESelStateHandler();
					ssm.SetSelStateHandler(selStateHandler);
					return ssm;
				}
				SlotSystemManager MakeSSM_TAM_selStateHandler(){
					SlotSystemManager ssm = MakeSSM();
					ITransactionManager tam = Substitute.For<ITransactionManager>();
					ssm.SetTAM(tam);
					SSESelStateHandler selStateHandler = new SSESelStateHandler();
					ssm.SetSelStateHandler(selStateHandler);
					return ssm;
				}
		}
	}
}
