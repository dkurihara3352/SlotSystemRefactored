using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using Utility;
namespace SlotSystemTests{
	namespace ElementsTests{
		[TestFixture][Category("SG")]
		public class SlotGroupTests: SlotSystemTest{
			[Test]
			public void Activate_WhenCalled_ReferToTAAndFocusOrDefocus(){
				SlotGroup sg = MakeSG();
					ISlotSystemManager ssm = Substitute.For<ISlotSystemManager>();
					ssm.When(x => x.ReferToTAAndUpdateSelState(sg)).Do(x => {sg.Focus();});
					sg.SetSSM(ssm);
				
				sg.Activate();

				Assert.That(sg.isFocused, Is.True);
			}
			[Test]
			public void TransactionCoroutine_AllSBsNotRunning_CallsActProcessExpire(){
				SlotGroup sg = MakeSG();
				List<ISlottable> sbs;
					ISlottable sbA = MakeSBWithActProc(false);
					ISlottable sbB = MakeSBWithActProc(false);
					ISlottable sbC = MakeSBWithActProc(false);
					sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC});
				sg.SetSBs(sbs);
				ISGActProcess sgActProc = MakeSubSGActProc();
				sg.SetAndRunActProcess(sgActProc);

				sg.TransactionCoroutine();
				
				sgActProc.Received().Expire();
				}
			/*	Fields	*/
				[Test]
				public void isPool_SSMPBunContainsInHierarchyThis_ReturnsTrue(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager stubSSM = MakeSSMWithPBunContaining(sg);
						sg.SetSSM(stubSSM);

					bool actual = sg.isPool;

					Assert.That(actual, Is.True);
					}
				[Test]
				public void isSGE_SSMEBunContainsInHierarchyThis_ReturnsTrue(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager stubSSM = MakeSSMWithEBunContaining(sg);
						sg.SetSSM(stubSSM);

					bool actual = sg.isSGE;

					Assert.That(actual, Is.True);
					}
				[Test]
				public void isSGG_SSMGBunContainsInHierarchyThis_ReturnsTrue(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager stubSSM = MakeSSMWithGBunContaining(sg);
						sg.SetSSM(stubSSM);

					bool actual = sg.isSGG;

					Assert.That(actual, Is.True);
					}
				[Test]
				public void hasEmptySlot_SlotsWithNullMember_ReturnsTrue(){
					SlotGroup sg = MakeSG();
					List<Slot> slots;
						Slot slotA = new Slot();
							ISlottable sbA = MakeSubSB();
							slotA.sb = sbA;
						Slot slotB = new Slot();
							ISlottable sbB = MakeSubSB();
							slotB.sb = sbB;
						Slot slotC = new Slot();
							slotC.sb = null;
						slots = new List<Slot>(new Slot[]{slotA, slotB, slotC});
						sg.SetSlots(slots);

					bool actual = sg.hasEmptySlot;

					Assert.That(actual, Is.True);
					}
				[Test]
				public void hasEmptySlot_SlotsWithNoNullMember_ReturnsFalse(){
					SlotGroup sg = MakeSG();
					List<Slot> slots;
						Slot slotA = new Slot();
							ISlottable sbA = MakeSubSB();
							slotA.sb = sbA;
						Slot slotB = new Slot();
							ISlottable sbB = MakeSubSB();
							slotB.sb = sbB;
						Slot slotC = new Slot();
							ISlottable sbC = MakeSubSB();
							slotC.sb = sbC;
						slots = new List<Slot>(new Slot[]{slotA, slotB, slotC});
						sg.SetSlots(slots);

					bool actual = sg.hasEmptySlot;

					Assert.That(actual, Is.False);
					}
				[TestCaseSource(typeof(EquippedSBsCases))]
				public void equippedSBs_WhenCalled_ReturnsAllSBsIsEquipped(List<ISlottable> sbs, List<ISlottable> expected){
					SlotGroup sg = MakeSG();
					sg.SetSBs(sbs);

					List<ISlottable> actual = sg.equippedSBs;

					bool equality = actual.MemberEquals(expected);
					Assert.That(equality, Is.True);
					}
					class EquippedSBsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable eSBA = MakeSubSB();
								eSBA.isEquipped.Returns(true);
							ISlottable eSBB = MakeSubSB();
								eSBB.isEquipped.Returns(true);
							ISlottable eSBC = MakeSubSB();
								eSBC.isEquipped.Returns(true);
							ISlottable uSBA = MakeSubSB();
								uSBA.isEquipped.Returns(false);
							ISlottable uSBB = MakeSubSB();
								uSBB.isEquipped.Returns(false);
							ISlottable uSBC = MakeSubSB();
								uSBC.isEquipped.Returns(false);
							List<ISlottable> case1SBs = new List<ISlottable>(new ISlottable[]{
								eSBA, eSBB, eSBC, uSBA, uSBB, uSBC
							});
							List<ISlottable> case1Exp = new List<ISlottable>(new ISlottable[]{
								eSBA, eSBB, eSBC
							});
							yield return new object[]{case1SBs, case1Exp};
						}
					}
				[TestCaseSource(typeof(IsAllSBsActProcDoneCases))]
				public void isAllSBsActProcDone_Various_ReturnsAccordingly(List<ISlottable> sbs, bool expected){
					SlotGroup sg = MakeSG();
					sg.SetSBs(sbs);

					bool actual = sg.isAllSBActProcDone;

					Assert.That(actual, Is.EqualTo(expected));
					}
					class IsAllSBsActProcDoneCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] mixed_F;
								ISlottable vSBA_0 = MakeSBWithActProc(false);
								ISlottable vSBB_0 = MakeSBWithActProc(false);
								ISlottable vSBC_0 = MakeSBWithActProc(false);
								ISlottable iSBA_0 = MakeSBWithActProc(true);
								ISlottable iSBB_0 = MakeSBWithActProc(true);
								ISlottable iSBC_0 = MakeSBWithActProc(true);
								List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
									vSBA_0, vSBB_0, vSBC_0, iSBA_0, iSBB_0, iSBC_0
								});
								mixed_F = new object[]{sbs_0, false};
								yield return mixed_F;
							object[] allDone_T;
								ISlottable vSBA_1 = MakeSBWithActProc(false);
								ISlottable vSBB_1 = MakeSBWithActProc(false);
								ISlottable vSBC_1 = MakeSBWithActProc(false);
								List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
									vSBA_1, vSBB_1, vSBC_1
								});
								allDone_T = new object[]{sbs_1, true};
								yield return allDone_T;
							object[] allNotDone_F;
								ISlottable iSBA_2 = MakeSBWithActProc(true);
								ISlottable iSBB_2 = MakeSBWithActProc(true);
								ISlottable iSBC_2 = MakeSBWithActProc(true);
								List<ISlottable> sbs_2 = new List<ISlottable>(new ISlottable[]{
									iSBA_2, iSBB_2, iSBC_2
								});
								allNotDone_F = new object[]{sbs_2, false};
								yield return allNotDone_F;
						}
					}
			/*	Methods	*/
				[TestCaseSource(typeof(InstantSortCases))]
				public void InstantSort_WhenCalled_ReorderSlotSBs(List<ISlottable> sbs, SGSorter sorter, List<ISlottable> expected){
					List<ISlottable> orig = new List<ISlottable>(sbs);
					SlotGroup sg = MakeSG();
					sg.SetSorter(sorter);
					sg.SetSBs(orig);
					List<Slot> slots = new List<Slot>();
						for(int i = 0; i< sbs.Count; i++)
							slots.Add(new Slot());
					sg.SetSlots(slots);

					sg.InstantSort();

					List<ISlottable> actual = new List<ISlottable>();
					foreach(var slot in sg.slots)
						actual.Add(slot.sb);
					bool equality = actual.MemberEquals(expected);
					Assert.That(equality, Is.True);
					}
					class InstantSortCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] itemIDSorter;
								PartsInstance partsB_0 = MakePartsInstance(1, 1);
								BowInstance bowA_0 = MakeBowInstance(0);
								ShieldInstance shieldA_0 = MakeShieldInstance(0);
								PartsInstance partsA_0 = MakePartsInstance(0, 2);
								BowInstance bowB_0 = MakeBowInstance(1);
								WearInstance wearA_0 = MakeWearInstance(0);
								QuiverInstance quiverA_0 = MakeQuiverInstance(0);
								MeleeWeaponInstance mWeaponA_0 = MakeMeleeWeaponInstance(0);
								PackInstance packA_0 = MakePackInstance(0);
								WearInstance wearB_0 = MakeWearInstance(1);
								partsB_0.SetAcquisitionOrder(0);
								bowA_0.SetAcquisitionOrder(1);
								shieldA_0.SetAcquisitionOrder(2);
								partsA_0.SetAcquisitionOrder(3);
								bowB_0.SetAcquisitionOrder(4);
								wearA_0.SetAcquisitionOrder(5);
								quiverA_0.SetAcquisitionOrder(6);
								mWeaponA_0.SetAcquisitionOrder(7);
								packA_0.SetAcquisitionOrder(8);
								wearB_0.SetAcquisitionOrder(9);
								ISlottable partsBSB_0 = MakeSB();
									partsBSB_0.SetItem(partsB_0);
								ISlottable bowASB_0 = MakeSB();
									bowASB_0.SetItem(bowA_0);
								ISlottable shieldASB_0 = MakeSB();
									shieldASB_0.SetItem(shieldA_0);
								ISlottable partsASB_0 = MakeSB();
									partsASB_0.SetItem(partsA_0);
								ISlottable bowBSB_0 = MakeSB();
									bowBSB_0.SetItem(bowB_0);
								ISlottable wearASB_0 = MakeSB();
									wearASB_0.SetItem(wearA_0);
								ISlottable quiverASB_0 = MakeSB();
									quiverASB_0.SetItem(quiverA_0);
								ISlottable mWeaponASB_0 = MakeSB();
									mWeaponASB_0.SetItem(mWeaponA_0);
								ISlottable packASB_0 = MakeSB();
									packASB_0.SetItem(packA_0);
								ISlottable wearBSB_0 = MakeSB();
									wearBSB_0.SetItem(wearA_0);
								List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
									partsBSB_0, 
									bowASB_0, 
									shieldASB_0, 
									partsASB_0, 
									bowBSB_0, 
									wearASB_0, 
									quiverASB_0, 
									mWeaponASB_0, 
									packASB_0, 
									wearBSB_0
								});
								List<ISlottable> expected_0 = new List<ISlottable>(new ISlottable[]{
									bowASB_0,
									bowBSB_0,
									wearASB_0,
									wearBSB_0,
									shieldASB_0,
									mWeaponASB_0,
									quiverASB_0,
									packASB_0,
									partsASB_0,
									partsBSB_0
								});
								itemIDSorter = new object[]{sbs_0, new SGItemIDSorter(), expected_0};
								yield return itemIDSorter;
							object[] inverseIDSorter;
								PartsInstance partsB_1 = MakePartsInstance(1, 1);
								BowInstance bowA_1 = MakeBowInstance(0);
								ShieldInstance shieldA_1 = MakeShieldInstance(0);
								PartsInstance partsA_1 = MakePartsInstance(0, 2);
								BowInstance bowB_1 = MakeBowInstance(1);
								WearInstance wearA_1 = MakeWearInstance(0);
								QuiverInstance quiverA_1 = MakeQuiverInstance(0);
								MeleeWeaponInstance mWeaponA_1 = MakeMeleeWeaponInstance(0);
								PackInstance packA_1 = MakePackInstance(0);
								WearInstance wearB_1 = MakeWearInstance(1);
								partsB_1.SetAcquisitionOrder(0);
								bowA_1.SetAcquisitionOrder(1);
								shieldA_1.SetAcquisitionOrder(2);
								partsA_1.SetAcquisitionOrder(3);
								bowB_1.SetAcquisitionOrder(4);
								wearA_1.SetAcquisitionOrder(5);
								quiverA_1.SetAcquisitionOrder(6);
								mWeaponA_1.SetAcquisitionOrder(7);
								packA_1.SetAcquisitionOrder(8);
								wearB_1.SetAcquisitionOrder(9);
								ISlottable partsBSB_1 = MakeSB();
									partsBSB_1.SetItem(partsB_1);
								ISlottable bowASB_1 = MakeSB();
									bowASB_1.SetItem(bowA_1);
								ISlottable shieldASB_1 = MakeSB();
									shieldASB_1.SetItem(shieldA_1);
								ISlottable partsASB_1 = MakeSB();
									partsASB_1.SetItem(partsA_1);
								ISlottable bowBSB_1 = MakeSB();
									bowBSB_1.SetItem(bowB_1);
								ISlottable wearASB_1 = MakeSB();
									wearASB_1.SetItem(wearA_1);
								ISlottable quiverASB_1 = MakeSB();
									quiverASB_1.SetItem(quiverA_1);
								ISlottable mWeaponASB_1 = MakeSB();
									mWeaponASB_1.SetItem(mWeaponA_1);
								ISlottable packASB_1 = MakeSB();
									packASB_1.SetItem(packA_1);
								ISlottable wearBSB_1 = MakeSB();
									wearBSB_1.SetItem(wearA_1);
								List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
									partsBSB_1, 
									bowASB_1, 
									shieldASB_1, 
									partsASB_1, 
									bowBSB_1, 
									wearASB_1, 
									quiverASB_1, 
									mWeaponASB_1, 
									packASB_1, 
									wearBSB_1
								});
								List<ISlottable> expected_1 = new List<ISlottable>(new ISlottable[]{
									partsBSB_1,
									partsASB_1,
									packASB_1,
									quiverASB_1,
									mWeaponASB_1,
									shieldASB_1,
									wearBSB_1,
									wearASB_1,
									bowBSB_1,
									bowASB_1
								});
								inverseIDSorter = new object[]{sbs_1, new SGInverseItemIDSorter(), expected_1};
								yield return inverseIDSorter;
							object[] acqOrderSorter;
								PartsInstance partsB_2 = MakePartsInstance(1, 1);
								BowInstance bowA_2 = MakeBowInstance(0);
								ShieldInstance shieldA_2 = MakeShieldInstance(0);
								PartsInstance partsA_2 = MakePartsInstance(0, 2);
								BowInstance bowB_2 = MakeBowInstance(1);
								WearInstance wearA_2 = MakeWearInstance(0);
								QuiverInstance quiverA_2 = MakeQuiverInstance(0);
								MeleeWeaponInstance mWeaponA_2 = MakeMeleeWeaponInstance(0);
								PackInstance packA_2 = MakePackInstance(0);
								WearInstance wearB_2 = MakeWearInstance(1);
								partsB_2.SetAcquisitionOrder(0);
								bowA_2.SetAcquisitionOrder(1);
								shieldA_2.SetAcquisitionOrder(2);
								partsA_2.SetAcquisitionOrder(3);
								bowB_2.SetAcquisitionOrder(4);
								wearA_2.SetAcquisitionOrder(5);
								quiverA_2.SetAcquisitionOrder(6);
								mWeaponA_2.SetAcquisitionOrder(7);
								packA_2.SetAcquisitionOrder(8);
								wearB_2.SetAcquisitionOrder(9);
								ISlottable partsBSB_2 = MakeSB();
									partsBSB_2.SetItem(partsB_2);
								ISlottable bowASB_2 = MakeSB();
									bowASB_2.SetItem(bowA_2);
								ISlottable shieldASB_2 = MakeSB();
									shieldASB_2.SetItem(shieldA_2);
								ISlottable partsASB_2 = MakeSB();
									partsASB_2.SetItem(partsA_2);
								ISlottable bowBSB_2 = MakeSB();
									bowBSB_2.SetItem(bowB_2);
								ISlottable wearASB_2 = MakeSB();
									wearASB_2.SetItem(wearA_2);
								ISlottable quiverASB_2 = MakeSB();
									quiverASB_2.SetItem(quiverA_2);
								ISlottable mWeaponASB_2 = MakeSB();
									mWeaponASB_2.SetItem(mWeaponA_2);
								ISlottable packASB_2 = MakeSB();
									packASB_2.SetItem(packA_2);
								ISlottable wearBSB_2 = MakeSB();
									wearBSB_2.SetItem(wearB_2);
								List<ISlottable> sbs_2 = new List<ISlottable>(new ISlottable[]{
									partsBSB_2, 
									bowASB_2, 
									shieldASB_2, 
									partsASB_2, 
									bowBSB_2, 
									wearASB_2, 
									quiverASB_2, 
									mWeaponASB_2, 
									packASB_2, 
									wearBSB_2
								});
								List<ISlottable> expected_2 = new List<ISlottable>(new ISlottable[]{
									partsBSB_2,
									bowASB_2,
									shieldASB_2,
									partsASB_2,
									bowBSB_2,
									wearASB_2,
									quiverASB_2,
									mWeaponASB_2,
									packASB_2,
									wearBSB_2
								});
								acqOrderSorter = new object[]{sbs_2, new SGAcquisitionOrderSorter(), expected_2};
								yield return acqOrderSorter;
						}
					}
				[TestCaseSource(typeof(AcceptsFilterCases))]
				public void AcceptsFilter_Various_ReturnsAccordingly(SGFilter filter, ISlottable sb, bool expected){
					SlotGroup sg = MakeSG();
					sg.SetFilter(filter);

					bool actual = sg.AcceptsFilter(sb);

					Assert.That(actual, Is.EqualTo(expected));
					}
					class AcceptsFilterCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] nullF_bow_T;
								ISlottable bow_0 = MakeSubSBWithItem(MakeBowInstance(0));
								nullF_bow_T = new object[]{new SGNullFilter(), bow_0, true};
								yield return nullF_bow_T;
							object[] nullF_wear_T;
								ISlottable wear_0 = MakeSubSBWithItem(MakeWearInstance(0));
								nullF_wear_T = new object[]{new SGNullFilter(), wear_0, true};
								yield return nullF_wear_T;
							object[] nullF_shield_T;
								ISlottable shield_0 = MakeSubSBWithItem(MakeShieldInstance(0));
								nullF_shield_T = new object[]{new SGNullFilter(), shield_0, true};
								yield return nullF_shield_T;
							object[] nullF_mWeapon_T;
								ISlottable mWeapon_0 = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
								nullF_mWeapon_T = new object[]{new SGNullFilter(), mWeapon_0, true};
								yield return nullF_mWeapon_T;
							object[] nullF_quiver_T;
								ISlottable quiver_0 = MakeSubSBWithItem(MakeQuiverInstance(0));
								nullF_quiver_T = new object[]{new SGNullFilter(), quiver_0, true};
								yield return nullF_quiver_T;
							object[] nullF_pack_T;
								ISlottable pack_0 = MakeSubSBWithItem(MakePackInstance(0));
								nullF_pack_T = new object[]{new SGNullFilter(), pack_0, true};
								yield return nullF_pack_T;
							object[] nullF_parts_T;
								ISlottable parts_0 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								nullF_parts_T = new object[]{new SGNullFilter(), parts_0, true};
								yield return nullF_parts_T;
							
							object[] bowF_bow_T;
								ISlottable bow_1 = MakeSubSBWithItem(MakeBowInstance(0));
								bowF_bow_T = new object[]{new SGBowFilter(), bow_1, true};
								yield return bowF_bow_T;
							object[] bowF_wear_F;
								ISlottable wear_1 = MakeSubSBWithItem(MakeWearInstance(0));
								bowF_wear_F = new object[]{new SGBowFilter(), wear_1, false};
								yield return bowF_wear_F;
							object[] bowF_shield_F;
								ISlottable shield_1 = MakeSubSBWithItem(MakeShieldInstance(0));
								bowF_shield_F = new object[]{new SGBowFilter(), shield_1, false};
								yield return bowF_shield_F;
							object[] bowF_mWeapon_F;
								ISlottable mWeapon_1 = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
								bowF_mWeapon_F = new object[]{new SGBowFilter(), mWeapon_1, false};
								yield return bowF_mWeapon_F;
							object[] bowF_quiver_F;
								ISlottable quiver_1 = MakeSubSBWithItem(MakeQuiverInstance(0));
								bowF_quiver_F = new object[]{new SGBowFilter(), quiver_1, false};
								yield return bowF_quiver_F;
							object[] bowF_pack_F;
								ISlottable pack_1 = MakeSubSBWithItem(MakePackInstance(0));
								bowF_pack_F = new object[]{new SGBowFilter(), pack_1, false};
								yield return bowF_pack_F;
							object[] bowF_parts_F;
								ISlottable parts_1 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								bowF_parts_F = new object[]{new SGBowFilter(), parts_1, false};
								yield return bowF_parts_F;
							
							object[] wearF_bow_F;
								ISlottable bow_2 = MakeSubSBWithItem(MakeBowInstance(0));
								wearF_bow_F = new object[]{new SGWearFilter(), bow_2, false};
								yield return wearF_bow_F;
							object[] wearF_wear_T;
								ISlottable wear_2 = MakeSubSBWithItem(MakeWearInstance(0));
								wearF_wear_T = new object[]{new SGWearFilter(), wear_2, true};
								yield return wearF_wear_T;
							object[] wearF_shield_F;
								ISlottable shield_2 = MakeSubSBWithItem(MakeShieldInstance(0));
								wearF_shield_F = new object[]{new SGWearFilter(), shield_2, false};
								yield return wearF_shield_F;
							object[] wearF_mWeapon_F;
								ISlottable mWeapon_2 = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
								wearF_mWeapon_F = new object[]{new SGWearFilter(), mWeapon_2, false};
								yield return wearF_mWeapon_F;
							object[] wearF_quiver_F;
								ISlottable quiver_2 = MakeSubSBWithItem(MakeQuiverInstance(0));
								wearF_quiver_F = new object[]{new SGWearFilter(), quiver_2, false};
								yield return wearF_quiver_F;
							object[] wearF_pack_F;
								ISlottable pack_2 = MakeSubSBWithItem(MakePackInstance(0));
								wearF_pack_F = new object[]{new SGWearFilter(), pack_2, false};
								yield return wearF_pack_F;
							object[] wearF_parts_F;
								ISlottable parts_2 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								wearF_parts_F = new object[]{new SGWearFilter(), parts_2, false};
								yield return wearF_parts_F;
							
							object[] cGearsF_bow_F;
								ISlottable bow_3 = MakeSubSBWithItem(MakeBowInstance(0));
								cGearsF_bow_F = new object[]{new SGCGearsFilter(), bow_3, false};
								yield return cGearsF_bow_F;
							object[] cGearsF_wear_F;
								ISlottable wear_3 = MakeSubSBWithItem(MakeWearInstance(0));
								cGearsF_wear_F = new object[]{new SGCGearsFilter(), wear_3, false};
								yield return cGearsF_wear_F;
							object[] cGearsF_shield_T;
								ISlottable shield_3 = MakeSubSBWithItem(MakeShieldInstance(0));
								cGearsF_shield_T = new object[]{new SGCGearsFilter(), shield_3, true};
								yield return cGearsF_shield_T;
							object[] cGearsF_mWeapon_T;
								ISlottable mWeapon_3 = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
								cGearsF_mWeapon_T = new object[]{new SGCGearsFilter(), mWeapon_3, true};
								yield return cGearsF_mWeapon_T;
							object[] cGearsF_quiver_T;
								ISlottable quiver_3 = MakeSubSBWithItem(MakeQuiverInstance(0));
								cGearsF_quiver_T = new object[]{new SGCGearsFilter(), quiver_3, true};
								yield return cGearsF_quiver_T;
							object[] cGearsF_pack_T;
								ISlottable pack_3 = MakeSubSBWithItem(MakePackInstance(0));
								cGearsF_pack_T = new object[]{new SGCGearsFilter(), pack_3, true};
								yield return cGearsF_pack_T;
							object[] cGearsF_parts_F;
								ISlottable parts_3 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								cGearsF_parts_F = new object[]{new SGCGearsFilter(), parts_3, false};
								yield return cGearsF_parts_F;
							
							object[] partsF_bow_F;
								ISlottable bow_4 = MakeSubSBWithItem(MakeBowInstance(0));
								partsF_bow_F = new object[]{new SGPartsFilter(), bow_4, false};
								yield return partsF_bow_F;
							object[] partsF_wear_F;
								ISlottable wear_4 = MakeSubSBWithItem(MakeWearInstance(0));
								partsF_wear_F = new object[]{new SGPartsFilter(), wear_4, false};
								yield return partsF_wear_F;
							object[] partsF_shield_F;
								ISlottable shield_4 = MakeSubSBWithItem(MakeShieldInstance(0));
								partsF_shield_F = new object[]{new SGPartsFilter(), shield_4, false};
								yield return partsF_shield_F;
							object[] partsF_mWeapon_F;
								ISlottable mWeapon_4 = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
								partsF_mWeapon_F = new object[]{new SGPartsFilter(), mWeapon_4, false};
								yield return partsF_mWeapon_F;
							object[] partsF_quiver_F;
								ISlottable quiver_4 = MakeSubSBWithItem(MakeQuiverInstance(0));
								partsF_quiver_F = new object[]{new SGPartsFilter(), quiver_4, false};
								yield return partsF_quiver_F;
							object[] partsF_pack_F;
								ISlottable pack_4 = MakeSubSBWithItem(MakePackInstance(0));
								partsF_pack_F = new object[]{new SGPartsFilter(), pack_4, false};
								yield return partsF_pack_F;
							object[] partsF_parts_T;
								ISlottable parts_4 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								partsF_parts_T = new object[]{new SGPartsFilter(), parts_4, true};
								yield return partsF_parts_T;
						}
					}
				[TestCaseSource(typeof(ContainsCases))]
				public void Contains_Vairous_ReturnsAccordingly(List<ISlottable> sbs, IEnumerable<ISlottable> expValid, IEnumerable<ISlottable> expInvalid){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);

					foreach(var sb in expValid)
						Assert.That(sg.Contains(sb), Is.True);
					foreach(var sb in expInvalid)
						Assert.That(sg.Contains(sb), Is.False);
					}
					class ContainsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object case0;
								ISlottable vSB0_0 = MakeSubSB();
								ISlottable vSB1_0 = MakeSubSB();
								ISlottable vSB2_0 = MakeSubSB();
								ISlottable iSB0_0 = MakeSubSB();
								ISlottable iSB1_0 = MakeSubSB();
								ISlottable iSB2_0 = MakeSubSB();
								List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
									vSB0_0, vSB1_0, vSB2_0
								});
								List<ISlottable> valid_0 = new List<ISlottable>(new ISlottable[]{
									vSB0_0, vSB1_0, vSB2_0
								});
								List<ISlottable> invalid_0 = new List<ISlottable>(new ISlottable[]{
									iSB0_0, iSB1_0, iSB2_0
								});
								case0 = new object[]{sbs_0, valid_0, invalid_0};
								yield return case0;
						}
					}
				[Test]
				public void FocusSelf_WhenCalled_SetsSelStateFocused(){
					SlotGroup sg = MakeSG();

					sg.FocusSelf();

					Assert.That(sg.isFocused, Is.True);
					}
				[Test]
				public void FocusSBs_WhenCalled_CallsSBReset(){
					SlotGroup sg = MakeSG();
						List<ISlottable> sbs;
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
							sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC});
						sg.SetSBs(sbs);
					
					sg.FocusSBs();

					sbA.Received().Reset();
					sbB.Received().Reset();
					sbC.Received().Reset();
					}
				[TestCaseSource(typeof(FocusSBsVariousCases))]
				public void FocusSBs_Various_CallsSBFocusOrDefocus(List<ISlottable> sbs, Dictionary<ISlotSystemElement, bool> dict){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);

					sg.FocusSBs();

					foreach(var e in sg){
						if(dict[e])
							e.Received().Focus();
						else
							e.Received().Defocus();
					}
					}
					class FocusSBsVariousCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable fSBA = MakeSubSB();
								fSBA.passesPrePickFilter.Returns(true);
							ISlottable fSBB = MakeSubSB();
								fSBB.passesPrePickFilter.Returns(true);
							ISlottable fSBC = MakeSubSB();
								fSBC.passesPrePickFilter.Returns(true);
							ISlottable dSBA = MakeSubSB();
								dSBA.passesPrePickFilter.Returns(false);
							ISlottable dSBB = MakeSubSB();
								dSBB.passesPrePickFilter.Returns(false);
							ISlottable dSBC = MakeSubSB();
								dSBC.passesPrePickFilter.Returns(false);
							List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{
								fSBA, fSBB, fSBC, dSBA, dSBB, dSBC
							});
							Dictionary<ISlotSystemElement, bool> dict = new Dictionary<ISlotSystemElement, bool>();
								dict.Add(fSBA, true);
								dict.Add(fSBB, true);
								dict.Add(fSBC, true);
								dict.Add(dSBA, false);
								dict.Add(dSBB, false);
								dict.Add(dSBC, false);
							yield return new object[]{ sbs, dict};
						}
					}
				[Test]
				public void DefocusSBs_WhenCalled_CallsSBReset(){
					SlotGroup sg = MakeSG();
						List<ISlottable> sbs;
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
							sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC});
						sg.SetSBs(sbs);
					
					sg.DefocusSBs();

					sbA.Received().Reset();
					sbB.Received().Reset();
					sbC.Received().Reset();
					}
				[Test]
				public void DefocusSelf_WhenCalled_SetsSelStateDefocused(){
					SlotGroup sg = MakeSG();

					sg.DefocusSelf();

					Assert.That(sg.isDefocused, Is.True);
					}
				[Test]
				public void DefocusSBs_WhenCalled_CallsSBDefocus(){
					SlotGroup sg = MakeSG();
						List<ISlottable> sbs;
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
							sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC});
						sg.SetSBs(sbs);
					
					sg.DefocusSBs();

					sbA.Received().Defocus();
					sbB.Received().Defocus();
					sbC.Received().Defocus();
					}
				
				
				[Test]
				public void PerformInHierarchyV1_WhenCalled_ActsOnSelfAndAllSBs(){
					SlotGroup sg = MakeSG();
						List<ISlottable> sbs;
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
						sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC});
						sg.SetSBs(sbs);
					
					sg.PerformInHierarchy(TestMethodV1);

					Assert.That(sg.isFocused, Is.True);
					sbA.Received().Focus();
					sbB.Received().Focus();
					sbC.Received().Focus();
					}
					void TestMethodV1(ISlotSystemElement ele){
						ele.Focus();
					}
				[Test]
				public void PerformInHierarchyV2_WhenCalled_ActsOnSelfAndAllSBs(){
					SlotGroup sg = MakeSG();
						List<ISlottable> sbs;
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
						sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC});
						sg.SetSBs(sbs);
					ISlotSystemElement stubEle = MakeSubSSE();
					sg.PerformInHierarchy(TestMethodV2, stubEle);

					Assert.That(sg.parent, Is.SameAs(stubEle));
					sbA.Received().SetParent(stubEle);
					sbB.Received().SetParent(stubEle);
					sbC.Received().SetParent(stubEle);
					}
					void TestMethodV2(ISlotSystemElement ele, object o){
						ISlotSystemElement e = (ISlotSystemElement)o;
						ele.SetParent(e);
					}
				[Test]
				public void PerformInHierarchyV3_WhenCalled_ActsOnSelfAndAllSBs(){
					SlotGroup sg = MakeSG();
						List<ISlottable> sbs;
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
						sbs = new List<ISlottable>(new ISlottable[]{sbA, null, null, sbB, sbC});
						sg.SetSBs(sbs);
					List<ISlotSystemElement> list = new List<ISlotSystemElement>();
					List<ISlotSystemElement> expected = new List<ISlotSystemElement>(new ISlotSystemElement[]{
						sg, sbA, sbB, sbC
					});
					
					sg.PerformInHierarchy(TestMethodV3, list);

					bool equality = list.MemberEquals(expected);
					Assert.That(equality, Is.True);
					}
					void TestMethodV3(ISlotSystemElement ele, IList<ISlotSystemElement> list){
						list.Add(ele);
					}
				[Test]
				public void SetElements_WhenCalled_SetsElements(){
					SlotGroup sg = MakeSG();
						GenericInventory inventory = new GenericInventory();
							BowInstance bow = MakeBowInstance(0);
							WearInstance wear = MakeWearInstance(0);
							ShieldInstance shield = MakeShieldInstance(0);
							MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
							inventory.Add(bow);
							inventory.Add(wear);
							inventory.Add(shield);
							inventory.Add(mWeapon);
						SGFilter nullFilter = new SGNullFilter();
						SGSorter idSorter = new SGItemIDSorter();
					sg.InspectorSetUp(inventory, nullFilter, idSorter, 0);

					sg.SetHierarchy();

					List<ISlotSystemElement> actualEles = new List<ISlotSystemElement>(sg);
					Assert.That(actualEles.Count, Is.EqualTo(4));
					}
				[Test]
				public void SetElements_WhenCalled_SetsSBsAndSlots(){
					SlotGroup sg = MakeSG();
						GenericInventory inventory = new GenericInventory();
							BowInstance bow = MakeBowInstance(0);
							WearInstance wear = MakeWearInstance(0);
							ShieldInstance shield = MakeShieldInstance(0);
							MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
							inventory.Add(bow);
							inventory.Add(wear);
							inventory.Add(shield);
							inventory.Add(mWeapon);
						SGFilter nullFilter = new SGNullFilter();
						SGSorter idSorter = new SGItemIDSorter();
					sg.InspectorSetUp(inventory, nullFilter, idSorter, 0);

					sg.SetHierarchy();

					ISlottable bowSB = sg.GetSB(bow);
					ISlottable wearSB = sg.GetSB(wear);
					ISlottable shieldSB = sg.GetSB(shield);
					ISlottable mWeaponSB = sg.GetSB(mWeapon);
					IEnumerable<ISlottable> actualSBs = new ISlottable[]{bowSB, wearSB, shieldSB, mWeaponSB};
					Assert.That(actualSBs, Is.All.Not.Null);
					Assert.That(sg.slots.Count, Is.EqualTo(4));
					int count = 0;
					foreach(ISlottable sb in sg)
						Assert.That(sb.slotID, Is.EqualTo(count ++));
					}
				[Test]
				public void InitializeState_WhenCalled_InitializesStates(){
					SlotGroup sg = MakeSG();

					sg.InitializeStates();

					Assert.That(sg.isDeactivated, Is.True);
					Assert.That(sg.isSelStateInit, Is.True);
					Assert.That(sg.isWaitingForAction, Is.True);
					Assert.That(sg.isActStateInit, Is.True);
					}
				[TestCase(0)]
				[TestCase(10)]
				public void InspectorSetUp_WhenCalled_SetsFields(int initSlotsCount){
					SlotGroup sg = MakeSG();
						GenericInventory genInv = Substitute.For<GenericInventory>();
						SGFilter filter = new SGNullFilter();
						SGSorter sorter = new SGItemIDSorter();
					
					sg.InspectorSetUp(genInv, filter, sorter, initSlotsCount);

					Assert.That(sg.inventory, Is.SameAs(genInv));
					Assert.That(sg.filter, Is.SameAs(filter));
					Assert.That(sg.sorter, Is.SameAs(sorter));
					Assert.That(sg.initSlotsCount, Is.EqualTo(initSlotsCount));
					Assert.That(sg.isExpandable, Is.EqualTo(initSlotsCount == 0));
					}
				class SGSetUpFieldsData: IEquatable<SGSetUpFieldsData>{
					public string name;
					public SGFilter filter;
					public Inventory inventory;
					public SlotGroupCommand OnACompComm;
					public SlotGroupCommand oAExecComm;
					public bool isShrinkable;
					public int initSlotsCount;
					public bool isExpandable;
					public SSEState sgSelState;
					public SSEState sgActState;
					public SGSetUpFieldsData(string name, SGFilter filter, Inventory inventory, SlotGroupCommand OnACompComm, SlotGroupCommand oAExecComm, bool isShrinkable, int initSlotsCount, bool isExpandable, SSEState sgSelState, SSEState sgActState){
						this.name = name;
						this.filter = filter;
						this.inventory = inventory;
						this.OnACompComm = OnACompComm;
						this.oAExecComm = oAExecComm;
						this.isShrinkable = isShrinkable;
						this.initSlotsCount = initSlotsCount;
						this.isExpandable = isExpandable;
						this.sgSelState = sgSelState;
						this.sgActState = sgActState;
					}
					public bool Equals(SGSetUpFieldsData other){
						bool flag = true;
							flag &= object.ReferenceEquals(this.filter, other.filter);
							flag &= object.ReferenceEquals(this.inventory, other.inventory);
							flag &= object.ReferenceEquals(this.OnACompComm, other.OnACompComm);
							flag &= object.ReferenceEquals(this.oAExecComm, other.oAExecComm);
							flag &= this.isShrinkable == other.isShrinkable;
							flag &= this.initSlotsCount == other.initSlotsCount;
							flag &= this.isExpandable == other.isExpandable;
							flag &= object.ReferenceEquals(this.sgSelState, other.sgSelState);
							flag &= object.ReferenceEquals(this.sgActState, other.sgActState);
						return flag;
					}

				}
				[TestCaseSource(typeof(GetSBCases))]
				public void GetSB_Various_ReturnsSBorNull(List<ISlottable> sbs, InventoryItemInstance item, ISlottable expected){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);

					ISlottable actual = sg.GetSB(item);

					Assert.That(actual, Is.SameAs(expected));
					}
					class GetSBCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] validBow;
								ISlottable bowA0SB_0 = MakeSubSB();
									BowInstance bowA0_0 = MakeBowInstance(0);
									bowA0SB_0.itemInst.Returns(bowA0_0);
								ISlottable wearA0SB_0 = MakeSubSB();
									WearInstance wearA0_0 = MakeWearInstance(0);
									wearA0SB_0.itemInst.Returns(wearA0_0);
								ISlottable shieldA0SB_0 = MakeSubSB();
									ShieldInstance shieldA0_0 = MakeShieldInstance(0);
									shieldA0SB_0.itemInst.Returns(shieldA0_0);
								ISlottable mWeaponA0SB_0 = MakeSubSB();
									MeleeWeaponInstance mWeaponA0_0 = MakeMeleeWeaponInstance(0);
									mWeaponA0SB_0.itemInst.Returns(mWeaponA0_0);
								ISlottable quiverA0SB_0 = MakeSubSB();
									QuiverInstance quiverA0_0 = MakeQuiverInstance(0);
									quiverA0SB_0.itemInst.Returns(quiverA0_0);
								ISlottable packA0SB_0 = MakeSubSB();
									PackInstance packA0_0 = MakePackInstance(0);
									packA0SB_0.itemInst.Returns(packA0_0);
								ISlottable partsA0SB_0 = MakeSubSB();
									PartsInstance partsA0_0 = MakePartsInstance(0,2);
									partsA0SB_0.itemInst.Returns(partsA0_0);
								List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
									bowA0SB_0,
									wearA0SB_0,
									shieldA0SB_0,
									mWeaponA0SB_0,
									quiverA0SB_0,
									packA0SB_0,
									partsA0SB_0
								});
								validBow = new object[]{sbs_0, bowA0_0, bowA0SB_0};
								yield return validBow;
							object[] validWear;
								ISlottable bowA0SB_1 = MakeSubSB();
									BowInstance bowA0_1 = MakeBowInstance(0);
									bowA0SB_1.itemInst.Returns(bowA0_1);
								ISlottable wearA0SB_1 = MakeSubSB();
									WearInstance wearA0_1 = MakeWearInstance(0);
									wearA0SB_1.itemInst.Returns(wearA0_1);
								ISlottable shieldA0SB_1 = MakeSubSB();
									ShieldInstance shieldA0_1 = MakeShieldInstance(0);
									shieldA0SB_1.itemInst.Returns(shieldA0_1);
								ISlottable mWeaponA0SB_1 = MakeSubSB();
									MeleeWeaponInstance mWeaponA0_1 = MakeMeleeWeaponInstance(0);
									mWeaponA0SB_1.itemInst.Returns(mWeaponA0_1);
								ISlottable quiverA0SB_1 = MakeSubSB();
									QuiverInstance quiverA0_1 = MakeQuiverInstance(0);
									quiverA0SB_1.itemInst.Returns(quiverA0_1);
								ISlottable packA0SB_1 = MakeSubSB();
									PackInstance packA0_1 = MakePackInstance(0);
									packA0SB_1.itemInst.Returns(packA0_1);
								ISlottable partsA0SB_1 = MakeSubSB();
									PartsInstance partsA0_1 = MakePartsInstance(0,2);
									partsA0SB_1.itemInst.Returns(partsA0_1);
								List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
									bowA0SB_1,
									wearA0SB_1,
									shieldA0SB_1,
									mWeaponA0SB_1,
									quiverA0SB_1,
									packA0SB_1,
									partsA0SB_1
								});
								validWear = new object[]{sbs_1, wearA0_1, wearA0SB_1};
								yield return validWear;
							object[] validShield;
								ISlottable bowA0SB_2 = MakeSubSB();
									BowInstance bowA0_2 = MakeBowInstance(0);
									bowA0SB_2.itemInst.Returns(bowA0_2);
								ISlottable wearA0SB_2 = MakeSubSB();
									WearInstance wearA0_2 = MakeWearInstance(0);
									wearA0SB_2.itemInst.Returns(wearA0_2);
								ISlottable shieldA0SB_2 = MakeSubSB();
									ShieldInstance shieldA0_2 = MakeShieldInstance(0);
									shieldA0SB_2.itemInst.Returns(shieldA0_2);
								ISlottable mWeaponA0SB_2 = MakeSubSB();
									MeleeWeaponInstance mWeaponA0_2 = MakeMeleeWeaponInstance(0);
									mWeaponA0SB_2.itemInst.Returns(mWeaponA0_2);
								ISlottable quiverA0SB_2 = MakeSubSB();
									QuiverInstance quiverA0_2 = MakeQuiverInstance(0);
									quiverA0SB_2.itemInst.Returns(quiverA0_2);
								ISlottable packA0SB_2 = MakeSubSB();
									PackInstance packA0_2 = MakePackInstance(0);
									packA0SB_2.itemInst.Returns(packA0_2);
								ISlottable partsA0SB_2 = MakeSubSB();
									PartsInstance partsA0_2 = MakePartsInstance(0,2);
									partsA0SB_2.itemInst.Returns(partsA0_2);
								List<ISlottable> sbs_2 = new List<ISlottable>(new ISlottable[]{
									bowA0SB_2,
									wearA0SB_2,
									shieldA0SB_2,
									mWeaponA0SB_2,
									quiverA0SB_2,
									packA0SB_2,
									partsA0SB_2
								});
								validShield = new object[]{sbs_2, shieldA0_2, shieldA0SB_2};
								yield return validShield;
							object[] validMWeapon;
								ISlottable bowA0SB_3 = MakeSubSB();
									BowInstance bowA0_3 = MakeBowInstance(0);
									bowA0SB_3.itemInst.Returns(bowA0_3);
								ISlottable wearA0SB_3 = MakeSubSB();
									WearInstance wearA0_3 = MakeWearInstance(0);
									wearA0SB_3.itemInst.Returns(wearA0_3);
								ISlottable shieldA0SB_3 = MakeSubSB();
									ShieldInstance shieldA0_3 = MakeShieldInstance(0);
									shieldA0SB_3.itemInst.Returns(shieldA0_3);
								ISlottable mWeaponA0SB_3 = MakeSubSB();
									MeleeWeaponInstance mWeaponA0_3 = MakeMeleeWeaponInstance(0);
									mWeaponA0SB_3.itemInst.Returns(mWeaponA0_3);
								ISlottable quiverA0SB_3 = MakeSubSB();
									QuiverInstance quiverA0_3 = MakeQuiverInstance(0);
									quiverA0SB_3.itemInst.Returns(quiverA0_3);
								ISlottable packA0SB_3 = MakeSubSB();
									PackInstance packA0_3 = MakePackInstance(0);
									packA0SB_3.itemInst.Returns(packA0_3);
								ISlottable partsA0SB_3 = MakeSubSB();
									PartsInstance partsA0_3 = MakePartsInstance(0,2);
									partsA0SB_3.itemInst.Returns(partsA0_3);
								List<ISlottable> sbs_3 = new List<ISlottable>(new ISlottable[]{
									bowA0SB_3,
									wearA0SB_3,
									shieldA0SB_3,
									mWeaponA0SB_3,
									quiverA0SB_3,
									packA0SB_3,
									partsA0SB_3
								});
								validMWeapon = new object[]{sbs_3, mWeaponA0_3, mWeaponA0SB_3};
								yield return validMWeapon;
							object[] validQuiver;
								ISlottable bowA0SB_4 = MakeSubSB();
									BowInstance bowA0_4 = MakeBowInstance(0);
									bowA0SB_4.itemInst.Returns(bowA0_4);
								ISlottable wearA0SB_4 = MakeSubSB();
									WearInstance wearA0_4 = MakeWearInstance(0);
									wearA0SB_4.itemInst.Returns(wearA0_4);
								ISlottable shieldA0SB_4 = MakeSubSB();
									ShieldInstance shieldA0_4 = MakeShieldInstance(0);
									shieldA0SB_4.itemInst.Returns(shieldA0_4);
								ISlottable mWeaponA0SB_4 = MakeSubSB();
									MeleeWeaponInstance mWeaponA0_4 = MakeMeleeWeaponInstance(0);
									mWeaponA0SB_4.itemInst.Returns(mWeaponA0_4);
								ISlottable quiverA0SB_4 = MakeSubSB();
									QuiverInstance quiverA0_4 = MakeQuiverInstance(0);
									quiverA0SB_4.itemInst.Returns(quiverA0_4);
								ISlottable packA0SB_4 = MakeSubSB();
									PackInstance packA0_4 = MakePackInstance(0);
									packA0SB_4.itemInst.Returns(packA0_4);
								ISlottable partsA0SB_4 = MakeSubSB();
									PartsInstance partsA0_4 = MakePartsInstance(0,2);
									partsA0SB_4.itemInst.Returns(partsA0_4);
								List<ISlottable> sbs_4 = new List<ISlottable>(new ISlottable[]{
									bowA0SB_4,
									wearA0SB_4,
									shieldA0SB_4,
									mWeaponA0SB_4,
									quiverA0SB_4,
									packA0SB_4,
									partsA0SB_4
								});
								validQuiver = new object[]{sbs_4, quiverA0_4, quiverA0SB_4};
								yield return validQuiver;
							object[] validPack;
								ISlottable bowA0SB_5 = MakeSubSB();
									BowInstance bowA0_5 = MakeBowInstance(0);
									bowA0SB_5.itemInst.Returns(bowA0_5);
								ISlottable wearA0SB_5 = MakeSubSB();
									WearInstance wearA0_5 = MakeWearInstance(0);
									wearA0SB_5.itemInst.Returns(wearA0_5);
								ISlottable shieldA0SB_5 = MakeSubSB();
									ShieldInstance shieldA0_5 = MakeShieldInstance(0);
									shieldA0SB_5.itemInst.Returns(shieldA0_5);
								ISlottable mWeaponA0SB_5 = MakeSubSB();
									MeleeWeaponInstance mWeaponA0_5 = MakeMeleeWeaponInstance(0);
									mWeaponA0SB_5.itemInst.Returns(mWeaponA0_5);
								ISlottable quiverA0SB_5 = MakeSubSB();
									QuiverInstance quiverA0_5 = MakeQuiverInstance(0);
									quiverA0SB_5.itemInst.Returns(quiverA0_5);
								ISlottable packA0SB_5 = MakeSubSB();
									PackInstance packA0_5 = MakePackInstance(0);
									packA0SB_5.itemInst.Returns(packA0_5);
								ISlottable partsA0SB_5 = MakeSubSB();
									PartsInstance partsA0_5 = MakePartsInstance(0,2);
									partsA0SB_5.itemInst.Returns(partsA0_5);
								List<ISlottable> sbs_5 = new List<ISlottable>(new ISlottable[]{
									bowA0SB_5,
									wearA0SB_5,
									shieldA0SB_5,
									mWeaponA0SB_5,
									quiverA0SB_5,
									packA0SB_5,
									partsA0SB_5
								});
								validPack = new object[]{sbs_5, packA0_5, packA0SB_5};
								yield return validPack;
							object[] validParts;
								ISlottable bowA0SB_6 = MakeSubSB();
									BowInstance bowA0_6 = MakeBowInstance(0);
									bowA0SB_6.itemInst.Returns(bowA0_6);
								ISlottable wearA0SB_6 = MakeSubSB();
									WearInstance wearA0_6 = MakeWearInstance(0);
									wearA0SB_6.itemInst.Returns(wearA0_6);
								ISlottable shieldA0SB_6 = MakeSubSB();
									ShieldInstance shieldA0_6 = MakeShieldInstance(0);
									shieldA0SB_6.itemInst.Returns(shieldA0_6);
								ISlottable mWeaponA0SB_6 = MakeSubSB();
									MeleeWeaponInstance mWeaponA0_6 = MakeMeleeWeaponInstance(0);
									mWeaponA0SB_6.itemInst.Returns(mWeaponA0_6);
								ISlottable quiverA0SB_6 = MakeSubSB();
									QuiverInstance quiverA0_6 = MakeQuiverInstance(0);
									quiverA0SB_6.itemInst.Returns(quiverA0_6);
								ISlottable packA0SB_6 = MakeSubSB();
									PackInstance packA0_6 = MakePackInstance(0);
									packA0SB_6.itemInst.Returns(packA0_6);
								ISlottable partsA0SB_6 = MakeSubSB();
									PartsInstance partsA0_6 = MakePartsInstance(0,2);
									partsA0SB_6.itemInst.Returns(partsA0_6);
								List<ISlottable> sbs_6 = new List<ISlottable>(new ISlottable[]{
									bowA0SB_6,
									wearA0SB_6,
									shieldA0SB_6,
									mWeaponA0SB_6,
									quiverA0SB_6,
									packA0SB_6,
									partsA0SB_6
								});
								validParts = new object[]{sbs_6, partsA0_6, partsA0SB_6};
								yield return validParts;
							
							object[] invalid;
								ISlottable bowA0SB_7 = MakeSubSB();
									BowInstance bowA0_7 = MakeBowInstance(0);
									bowA0SB_7.itemInst.Returns(bowA0_7);
								ISlottable wearA0SB_7 = MakeSubSB();
									WearInstance wearA0_7 = MakeWearInstance(0);
									wearA0SB_7.itemInst.Returns(wearA0_7);
								ISlottable shieldA0SB_7 = MakeSubSB();
									ShieldInstance shieldA0_7 = MakeShieldInstance(0);
									shieldA0SB_7.itemInst.Returns(shieldA0_7);
								ISlottable mWeaponA0SB_7 = MakeSubSB();
									MeleeWeaponInstance mWeaponA0_7 = MakeMeleeWeaponInstance(0);
									mWeaponA0SB_7.itemInst.Returns(mWeaponA0_7);
								ISlottable quiverA0SB_7 = MakeSubSB();
									QuiverInstance quiverA0_7 = MakeQuiverInstance(0);
									quiverA0SB_7.itemInst.Returns(quiverA0_7);
								ISlottable packA0SB_7 = MakeSubSB();
									PackInstance packA0_7 = MakePackInstance(0);
									packA0SB_7.itemInst.Returns(packA0_7);
								ISlottable partsA0SB_7 = MakeSubSB();
									PartsInstance partsA0_7 = MakePartsInstance(0,2);
									partsA0SB_7.itemInst.Returns(partsA0_7);
								List<ISlottable> sbs_7 = new List<ISlottable>(new ISlottable[]{
									bowA0SB_7,
									wearA0SB_7,
									shieldA0SB_7,
									mWeaponA0SB_7,
									quiverA0SB_7,
									packA0SB_7,
									partsA0SB_7
								});
								invalid = new object[]{sbs_7, MakePartsInstance(1, 2), null};
								yield return invalid;
							
						}
					}
				
				[TestCaseSource(typeof(GetSBCases))]
				public void HasItem_Various_ReturnsTrueOrFalse(List<ISlottable> sbs, InventoryItemInstance item, ISlottable returnedSB){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
					bool expected = returnedSB ==null? false: true;

					bool actual = sg.HasItem(item);

					Assert.That(actual, Is.EqualTo(expected));
					}
				[TestCase(1)]
				[TestCase(10)]
				[TestCase(0)]
				public void UpdateSBs_WhenCalled_SetsNewSlotsWithTheSameNumberAsSBs(int count){
					SlotGroup sg = MakeSG();
						List<ISlottable> sbs = new List<ISlottable>();
						sg.SetSBs(sbs);
						List<ISlottable> newSBs = CreateSBs(count);
					
					sg.UpdateSBs(newSBs);

					Assert.That(sg.newSlots.Count, Is.EqualTo(count));
					Assert.That(sg.newSlots, Is.All.InstanceOf(typeof(Slot)));
					}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))]
				public void UpdateSBs_NewSBsContainsSB_CallsSetNewSlotIDWithNewSBsID(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
					
					sg.UpdateSBs(newSBs);

					foreach(ISlottable sb in sg)
						if(newSBs.Contains(sb))
							sb.Received().SetNewSlotID(newSBs.IndexOf(sb));
					}
					class UpdateSBsNewSBsContainsSBCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable oSBA = MakeSubSB();
							ISlottable oSBB = MakeSubSB();
							ISlottable oSBC = MakeSubSB();
							ISlottable aSBA = MakeSubSB();
							ISlottable aSBB = MakeSubSB();
							ISlottable aSBC = MakeSubSB();
							ISlottable rSBA = MakeSubSB();
							ISlottable rSBB = MakeSubSB();
							ISlottable rSBC = MakeSubSB();
							List<ISlottable> case1SBs = new List<ISlottable>(new ISlottable[]{
								oSBA, oSBB, oSBC, rSBA, rSBB, rSBC
							});
							List<ISlottable> case1NewSBs = new List<ISlottable>(new ISlottable[]{
								oSBB,
								aSBA,
								oSBC,
								aSBB,
								oSBA,
								aSBC
							});
							yield return new object[]{case1SBs, case1NewSBs};
						}
					}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))]
				public void UpdateSBs_NewSBsAndSBsContainsSB_CallsSBMoveWithin(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
					
					sg.UpdateSBs(newSBs);

					foreach(ISlottable sb in sg)
						if(newSBs.Contains(sb) && sbs.Contains(sb))
							sb.Received().MoveWithin();
						else
							sb.DidNotReceive().MoveWithin();
					}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))]
				public void UpdateSBs_NewSBNotContainsSB_CallsSetNewSlotIDMinus1(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);

					sg.UpdateSBs(newSBs);

					foreach(ISlottable sb in sg)
						if(!newSBs.Contains(sb))
							sb.Received().SetNewSlotID(-1);
						else
							sb.DidNotReceive().SetNewSlotID(-1);
					}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))]
				public void UpdateSBs_NewSBNotContainsSB_CallsSBRemove(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);

					sg.UpdateSBs(newSBs);

					foreach(ISlottable sb in sg)
						if(!newSBs.Contains(sb))
							sb.Received().Remove();
						else
							sb.DidNotReceive().Remove();
					}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))]
				public void UpdateSBs_SBsNotContainsSB_CallsSBAdd(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);

					sg.UpdateSBs(newSBs);

					foreach(ISlottable sb in sg)
						if(!sbs.Contains(sb))
							sb.Received().Add();
						else
							sb.DidNotReceive().Add();
					}
				[TestCase(0)]
				[TestCase(1)]
				[TestCase(10)]
				public void CreateNewSlots_WhenCalled_SetsNewSlotsWithSameCountAsNewSBs(int count){
					SlotGroup sg = MakeSG();
						List<ISlottable> sbs = new List<ISlottable>();
						sg.SetSBs(sbs);
						List<ISlottable> newSBs = CreateSBs(count);
						sg.SetNewSBs(newSBs);
					
					sg.CreateNewSlots();

					Assert.That(sg.newSlots.Count, Is.EqualTo(count));
					foreach(var slot in sg.newSlots)
						Assert.That(slot.sb, Is.Null);
					}
				[TestCaseSource(typeof(GetNewSlotCases))]
				public void GetNewSlot_itemFound_ReturnsNewSlot(List<ISlottable> sbs ,List<Slot> newSlots, InventoryItemInstance item, Slot expected){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
						sg.SetNewSlots(newSlots);
					
					Slot actual = sg.GetNewSlot(item);

					Assert.That(actual, Is.SameAs(expected));
					}
					class GetNewSlotCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] valid0;
								ISlottable bowSB_0 = MakeSubSB();
									BowInstance bow_0 = MakeBowInstance(0);
									bowSB_0.newSlotID.Returns(0);
									bowSB_0.itemInst.Returns(bow_0);
								ISlottable wearSB_0 = MakeSubSB();
									WearInstance wear_0 = MakeWearInstance(0);
									wearSB_0.newSlotID.Returns(1);
									wearSB_0.itemInst.Returns(wear_0);
								ISlottable shieldSB_0 = MakeSubSB();
									ShieldInstance shield_0 = MakeShieldInstance(0);
									shieldSB_0.newSlotID.Returns(2);
									shieldSB_0.itemInst.Returns(shield_0);
								ISlottable mWeaponSB_0 = MakeSubSB();
									MeleeWeaponInstance mWeapon_0 = MakeMeleeWeaponInstance(0);
									mWeaponSB_0.newSlotID.Returns(3);
									mWeaponSB_0.itemInst.Returns(mWeapon_0);
								List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{bowSB_0, wearSB_0, shieldSB_0, mWeaponSB_0});
								List<Slot> newSlots_0 = new List<Slot>();
									foreach(var sb in sbs_0)
										newSlots_0.Add(new Slot());
								valid0 = new object[]{sbs_0, newSlots_0, bow_0, newSlots_0[0]};
								yield return valid0;
							object[] valid1;
								ISlottable bowSB_1 = MakeSubSB();
									BowInstance bow_1 = MakeBowInstance(0);
									bowSB_1.newSlotID.Returns(0);
									bowSB_1.itemInst.Returns(bow_1);
								ISlottable wearSB_1 = MakeSubSB();
									WearInstance wear_1 = MakeWearInstance(0);
									wearSB_1.newSlotID.Returns(1);
									wearSB_1.itemInst.Returns(wear_1);
								ISlottable shieldSB_1 = MakeSubSB();
									ShieldInstance shield_1 = MakeShieldInstance(0);
									shieldSB_1.newSlotID.Returns(2);
									shieldSB_1.itemInst.Returns(shield_1);
								ISlottable mWeaponSB_1 = MakeSubSB();
									MeleeWeaponInstance mWeapon_1 = MakeMeleeWeaponInstance(0);
									mWeaponSB_1.newSlotID.Returns(3);
									mWeaponSB_1.itemInst.Returns(mWeapon_1);
								List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{bowSB_1, wearSB_1, shieldSB_1, mWeaponSB_1});
								List<Slot> newSlots_1 = new List<Slot>();
									foreach(var sb in sbs_1)
										newSlots_1.Add(new Slot());
								valid1 = new object[]{sbs_1, newSlots_1, wear_1, newSlots_1[1]};
								yield return valid1;
							object[] valid2;
								ISlottable bowSB_2 = MakeSubSB();
									BowInstance bow_2 = MakeBowInstance(0);
									bowSB_2.newSlotID.Returns(0);
									bowSB_2.itemInst.Returns(bow_2);
								ISlottable wearSB_2 = MakeSubSB();
									WearInstance wear_2 = MakeWearInstance(0);
									wearSB_2.newSlotID.Returns(1);
									wearSB_2.itemInst.Returns(wear_2);
								ISlottable shieldSB_2 = MakeSubSB();
									ShieldInstance shield_2 = MakeShieldInstance(0);
									shieldSB_2.newSlotID.Returns(2);
									shieldSB_2.itemInst.Returns(shield_2);
								ISlottable mWeaponSB_2 = MakeSubSB();
									MeleeWeaponInstance mWeapon_2 = MakeMeleeWeaponInstance(0);
									mWeaponSB_2.newSlotID.Returns(3);
									mWeaponSB_2.itemInst.Returns(mWeapon_2);
								List<ISlottable> sbs_2 = new List<ISlottable>(new ISlottable[]{bowSB_2, wearSB_2, shieldSB_2, mWeaponSB_2});
								List<Slot> newSlots_2 = new List<Slot>();
									foreach(var sb in sbs_2)
										newSlots_2.Add(new Slot());
								valid2 = new object[]{sbs_2, newSlots_2, shield_2, newSlots_2[2]};
								yield return valid2;
							object[] valid3;
								ISlottable bowSB_3 = MakeSubSB();
									BowInstance bow_3 = MakeBowInstance(0);
									bowSB_3.newSlotID.Returns(0);
									bowSB_3.itemInst.Returns(bow_3);
								ISlottable wearSB_3 = MakeSubSB();
									WearInstance wear_3 = MakeWearInstance(0);
									wearSB_3.newSlotID.Returns(1);
									wearSB_3.itemInst.Returns(wear_3);
								ISlottable shieldSB_3 = MakeSubSB();
									ShieldInstance shield_3 = MakeShieldInstance(0);
									shieldSB_3.newSlotID.Returns(2);
									shieldSB_3.itemInst.Returns(shield_3);
								ISlottable mWeaponSB_3 = MakeSubSB();
									MeleeWeaponInstance mWeapon_3 = MakeMeleeWeaponInstance(0);
									mWeaponSB_3.newSlotID.Returns(3);
									mWeaponSB_3.itemInst.Returns(mWeapon_3);
								List<ISlottable> sbs_3 = new List<ISlottable>(new ISlottable[]{bowSB_3, wearSB_3, shieldSB_3, mWeaponSB_3});
								List<Slot> newSlots_3 = new List<Slot>();
									foreach(var sb in sbs_3)
										newSlots_3.Add(new Slot());
								valid3 = new object[]{sbs_3, newSlots_3, mWeapon_3, newSlots_3[3]};
								yield return valid3;
							object[] invalid;
								ISlottable bowSB_4 = MakeSubSB();
									BowInstance bow_4 = MakeBowInstance(0);
									bowSB_4.newSlotID.Returns(0);
									bowSB_4.itemInst.Returns(bow_4);
								ISlottable wearSB_4 = MakeSubSB();
									WearInstance wear_4 = MakeWearInstance(0);
									wearSB_4.newSlotID.Returns(1);
									wearSB_4.itemInst.Returns(wear_4);
								ISlottable shieldSB_4 = MakeSubSB();
									ShieldInstance shield_4 = MakeShieldInstance(0);
									shieldSB_4.newSlotID.Returns(2);
									shieldSB_4.itemInst.Returns(shield_4);
								ISlottable mWeaponSB_4 = MakeSubSB();
									MeleeWeaponInstance mWeapon_4 = MakeMeleeWeaponInstance(0);
									mWeaponSB_4.newSlotID.Returns(3);
									mWeaponSB_4.itemInst.Returns(mWeapon_4);
								List<ISlottable> sbs_4 = new List<ISlottable>(new ISlottable[]{bowSB_4, wearSB_4, shieldSB_4, mWeaponSB_4});
								List<Slot> newSlots_4 = new List<Slot>();
									foreach(var sb in sbs_4)
										newSlots_4.Add(new Slot());
								invalid = new object[]{sbs_4, newSlots_4, MakeBowInstance(0), null};
								yield return invalid;
						}
					}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))]
				public void SetSBsActStates_NewSbsAndSBsContainsSB_CallsSBMoveWithin(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
						sg.SetNewSBs(newSBs);
					List<ISlottable> allSBs = new List<ISlottable>(sbs);
					foreach(var sb in newSBs)
						if(!sbs.Contains(sb)) allSBs.Add(sb);
					sg.SetSBsActStates();

					foreach(var sb in allSBs)
						if(sbs.Contains(sb) && newSBs.Contains(sb))
							sb.Received().MoveWithin();
						else
							sb.DidNotReceive().MoveWithin();
					}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))]
				public void SetSBsActStates_NewSBsNotContainsSB_CallsSBRemoved(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
						sg.SetNewSBs(newSBs);
					List<ISlottable> allSBs = new List<ISlottable>(sbs);
					foreach(var sb in newSBs)
						if(!sbs.Contains(sb)) allSBs.Add(sb);
					sg.SetSBsActStates();

					foreach(var sb in allSBs)
						if(!newSBs.Contains(sb))
							sb.Received().Remove();
						else
							sb.DidNotReceive().Remove();
					}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))]
				public void SetSBsActStates_SBsNotContainsSB_CallsSBAdd(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
						sg.SetNewSBs(newSBs);
					List<ISlottable> allSBs = new List<ISlottable>(sbs);
					foreach(var sb in newSBs)
						if(!sbs.Contains(sb)) allSBs.Add(sb);
					sg.SetSBsActStates();

					foreach(var sb in allSBs)
						if(!sbs.Contains(sb))
							sb.Received().Add();
						else
							sb.DidNotReceive().Add();
					}
				[TestCaseSource(typeof(SyncSBsToSlotsCases))]
				public void SyncSBsToSlots_WhenCalled_SyncSBsToSlots(List<Slot> slots, List<ISlottable> expected){
					SlotGroup sg = MakeSG();
						sg.SetSlots(slots);
					
					sg.SyncSBsToSlots();

					bool equality = sg.toList.MemberEquals(expected);
					Assert.That(equality, Is.True);
					foreach(ISlottable sb in sg){
						if(sb != null)
							sb.Received().SetSlotID(sg.toList.IndexOf(sb));
					}
					}
					class SyncSBsToSlotsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							List<Slot> slots;
								Slot slotA = new Slot();
									ISlottable sbA = MakeSubSB();
									slotA.sb = sbA;
								Slot slotNA = new Slot();
								Slot slotB = new Slot();
									ISlottable sbB = MakeSubSB();
									slotB.sb = sbB;
								Slot slotC = new Slot();
									ISlottable sbC = MakeSubSB();
									slotC.sb = sbC;
								Slot slotNB = new Slot();
								Slot slotNC = new Slot();
								Slot slotD = new Slot();
									ISlottable sbD = MakeSubSB();
									slotD.sb = sbD;
								Slot slotE = new Slot();
									ISlottable sbE = MakeSubSB();
									slotE.sb = sbE;
								slots = new List<Slot>(new Slot[]{slotA, slotNA, slotB, slotC, slotNB, slotNC, slotD, slotE});
							List<ISlottable> expected = new List<ISlottable>(new ISlottable[]{
								sbA, null, sbB, sbC, null, null, sbD, sbE
							});
							yield return new object[]{slots, expected};
						}
					}
				[TestCase(true, true)]
				[TestCase(false, false)]
				public void OnCompleteSlotMovements_NewSlotIDMinus1_CallsSBDestory(bool removed, bool expected){
					SlotGroup sg = MakeSG();
						List<ISlottable> sbs;
							ISlottable sb = MakeSubSB();
							sb.isToBeRemoved.Returns(removed);
						sbs = new List<ISlottable>(new ISlottable[]{sb});
						sg.SetSBs(sbs);
						List<Slot> newSlots = new List<Slot>(new Slot[]{ new Slot(), new Slot()});
						sg.SetNewSlots(newSlots);
					
					sg.OnCompleteSlotMovements();

					if(expected)
						sb.Received().Destroy();
					else
						sb.DidNotReceive().Destroy();
					}
				[TestCaseSource(typeof(OnCompleteSlotMovementsCases))]
				public void OnCompleteSlotMovements_WhenCalledAfterNewSlotsAreSet_SyncSBsToSlotsWithNewSlotIDs(List<ISlottable> sbs, List<ISlottable> expected){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
						List<Slot> newSlots = new List<Slot>();
							foreach(var sb in sbs)
								newSlots.Add(new Slot());
							sg.SetNewSlots(newSlots);
					
					sg.OnCompleteSlotMovements();

					bool equality = sg.toList.MemberEquals(expected);
					Assert.That(equality, Is.True);
					foreach(var sb in sg)
						if(sb != null)
							((ISlottable)sb).Received().SetSlotID(sg.toList.IndexOf((ISlottable)sb));
					}
					class OnCompleteSlotMovementsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable rsbA = MakeSubSB();
								rsbA.newSlotID.Returns(-1);
								rsbA.isToBeRemoved.Returns(true);
							ISlottable sbA = MakeSubSB();
								sbA.newSlotID.Returns(0);
								//null
							ISlottable sbB = MakeSubSB();
								sbB.newSlotID.Returns(1);
							ISlottable sbC = MakeSubSB();
								sbC.newSlotID.Returns(2);
								//null
							ISlottable rsbB = MakeSubSB();
								rsbB.newSlotID.Returns(-1);
								rsbB.isToBeRemoved.Returns(true);
								//null
							ISlottable sbD = MakeSubSB();
								sbD.newSlotID.Returns(3);
							List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{
								rsbA,
								sbC,
								null,
								sbA,
								null,
								sbB,
								sbD,
								null,
								rsbB
							});
							List<ISlottable> expected = new List<ISlottable>(new ISlottable[]{
								sbA, sbB, sbC, sbD, null, null, null, null, null
							});	
							yield return new object[]{sbs, expected};
						}
					}
				[TestCaseSource(typeof(SwappableSBsCases))]
				public void SwappableSBs_SBAreSwappable_ReturnsList(SlotGroup sg, ISlottable sb, List<ISlottable> expected){
					List<ISlottable> actual = sg.SwappableSBs(sb);

					bool equality = actual.MemberEquals(expected);
					Assert.That(equality, Is.True);
					}
					class SwappableSBsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] sameSG_empty;
								SlotGroup sg_0 = MakeSG();
									sg_0.SetFilter(new SGNullFilter());
									ISlottable bow0SB_0 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_0);
									ISlottable bow1SB_0 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_0);
									ISlottable wearSB_0 = MakeSubSBWithItemAndSG(MakeWearInstance(0), sg_0);
									ISlottable shieldSB_0 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), sg_0);
									ISlottable mWeaponSB_0 = MakeSubSBWithItemAndSG(MakeMeleeWeaponInstance(0), sg_0);
									ISlottable quiverSB_0 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_0);
									ISlottable packSB_0 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_0);
									ISlottable partsSB_0 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), sg_0);
									sg_0.SetSBs(new List<ISlottable>(new ISlottable[]{
										bow0SB_0, bow1SB_0, wearSB_0, shieldSB_0, mWeaponSB_0, quiverSB_0, packSB_0, partsSB_0
									}));
								sameSG_empty = new object[]{sg_0, shieldSB_0, new List<ISlottable>()};
								yield return sameSG_empty;
							object[] withBows;
								SlotGroup sg_1 = MakeSG();
									sg_1.SetFilter(new SGNullFilter());
									ISlottable bow0SB_1 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_1);
									ISlottable bow1SB_1 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_1);
									ISlottable wearSB_1 = MakeSubSBWithItemAndSG(MakeWearInstance(0), sg_1);
									ISlottable shieldSB_1 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), sg_1);
									ISlottable mWeaponSB_1 = MakeSubSBWithItemAndSG(MakeMeleeWeaponInstance(0), sg_1);
									ISlottable quiverSB_1 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_1);
									ISlottable packSB_1 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_1);
									ISlottable partsSB_1 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), sg_1);
									sg_1.SetSBs(new List<ISlottable>(new ISlottable[]{
										bow0SB_1, bow1SB_1, wearSB_1, shieldSB_1, mWeaponSB_1, quiverSB_1, packSB_1, partsSB_1
									}));
								SlotGroup bowSG_1 = MakeSG();
									bowSG_1.SetFilter(new SGBowFilter());
									ISlottable oBow0SB_1 = MakeSubSBWithItemAndSG(MakeBowInstance(0), bowSG_1);
								withBows = new object[]{sg_1, oBow0SB_1, new List<ISlottable>(new ISlottable[]{bow0SB_1, bow1SB_1})};
								yield return withBows;
							object[] withWears;
								SlotGroup sg_2 = MakeSG();
									sg_2.SetFilter(new SGNullFilter());
									ISlottable bow0SB_2 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_2);
									ISlottable bow1SB_2 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_2);
									ISlottable wearSB_2 = MakeSubSBWithItemAndSG(MakeWearInstance(0), sg_2);
									ISlottable shieldSB_2 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), sg_2);
									ISlottable mWeaponSB_2 = MakeSubSBWithItemAndSG(MakeMeleeWeaponInstance(0), sg_2);
									ISlottable quiverSB_2 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_2);
									ISlottable packSB_2 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_2);
									ISlottable partsSB_2 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), sg_2);
									sg_2.SetSBs(new List<ISlottable>(new ISlottable[]{
										bow0SB_2, bow1SB_2, wearSB_2, shieldSB_2, mWeaponSB_2, quiverSB_2, packSB_2, partsSB_2
									}));
								SlotGroup wearSG_2 = MakeSG();
									wearSG_2.SetFilter(new SGWearFilter());
									ISlottable oWear0SB_2 = MakeSubSBWithItemAndSG(MakeWearInstance(0), wearSG_2);
								withWears = new object[]{sg_2, oWear0SB_2, new List<ISlottable>(new ISlottable[]{wearSB_2})};
								yield return withWears;
							object[] withCGears;
								SlotGroup sg_3 = MakeSG();
									sg_3.SetFilter(new SGNullFilter());
									ISlottable bow0SB_3 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_3);
									ISlottable bow1SB_3 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_3);
									ISlottable wearSB_3 = MakeSubSBWithItemAndSG(MakeWearInstance(0), sg_3);
									ISlottable shieldSB_3 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), sg_3);
									ISlottable mWeaponSB_3 = MakeSubSBWithItemAndSG(MakeMeleeWeaponInstance(0), sg_3);
									ISlottable quiverSB_3 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_3);
									ISlottable packSB_3 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_3);
									ISlottable partsSB_3 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), sg_3);
									sg_3.SetSBs(new List<ISlottable>(new ISlottable[]{
										bow0SB_3, bow1SB_3, wearSB_3, shieldSB_3, mWeaponSB_3, quiverSB_3, packSB_3, partsSB_3
									}));
								SlotGroup cGearsSG_3 = MakeSG();
									cGearsSG_3.SetFilter(new SGCGearsFilter());
									ISlottable oShieldSB_3 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), cGearsSG_3);
								withCGears = new object[]{sg_3, oShieldSB_3, new List<ISlottable>(new ISlottable[]{shieldSB_3, mWeaponSB_3, quiverSB_3, packSB_3})};
								yield return withCGears;
							object[] sameParts_empty;
								SlotGroup sg_4 = MakeSG();
									sg_4.SetFilter(new SGNullFilter());
									ISlottable bow0SB_4 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_4);
									ISlottable bow1SB_4 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_4);
									ISlottable wearSB_4 = MakeSubSBWithItemAndSG(MakeWearInstance(0), sg_4);
									ISlottable shieldSB_4 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), sg_4);
									ISlottable mWeaponSB_4 = MakeSubSBWithItemAndSG(MakeMeleeWeaponInstance(0), sg_4);
									ISlottable quiverSB_4 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_4);
									ISlottable packSB_4 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_4);
									ISlottable partsSB_4 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), sg_4);
									sg_4.SetSBs(new List<ISlottable>(new ISlottable[]{
										bow0SB_4, bow1SB_4, wearSB_4, shieldSB_4, mWeaponSB_4, quiverSB_4, packSB_4, partsSB_4
									}));
								SlotGroup partsSG_4 = MakeSG();
									partsSG_4.SetFilter(new SGPartsFilter());
									ISlottable oPartsSB_4 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 4), partsSG_4);
								sameParts_empty = new object[]{sg_4, oPartsSB_4, new List<ISlottable>(new ISlottable[]{})};
								yield return sameParts_empty;
							object[] withOtherParts;
								SlotGroup sg_5 = MakeSG();
									sg_5.SetFilter(new SGNullFilter());
									ISlottable bow0SB_5 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_5);
									ISlottable bow1SB_5 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_5);
									ISlottable wearSB_5 = MakeSubSBWithItemAndSG(MakeWearInstance(0), sg_5);
									ISlottable shieldSB_5 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), sg_5);
									ISlottable mWeaponSB_5 = MakeSubSBWithItemAndSG(MakeMeleeWeaponInstance(0), sg_5);
									ISlottable quiverSB_5 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_5);
									ISlottable packSB_5 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_5);
									ISlottable partsSB_5 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), sg_5);
									sg_5.SetSBs(new List<ISlottable>(new ISlottable[]{
										bow0SB_5, bow1SB_5, wearSB_5, shieldSB_5, mWeaponSB_5, quiverSB_5, packSB_5, partsSB_5
									}));
								SlotGroup partsSG_5 = MakeSG();
									partsSG_5.SetFilter(new SGPartsFilter());
									ISlottable oPartsSB_5 = MakeSubSBWithItemAndSG(MakePartsInstance(1, 4), partsSG_5);
								withOtherParts = new object[]{sg_5, oPartsSB_5, new List<ISlottable>(new ISlottable[]{partsSB_5})};
								yield return withOtherParts;
						}
					}
				[Test]
				public void Reset_WhenCalled_SetActStateWFA(){
					SlotGroup sg = MakeSG();

					sg.Reset();
					
					Assert.That(sg.isWaitingForAction, Is.True);
					}
				[Test]
				public void Reset_WhenCalled_SetsFields(){
					SlotGroup sg = MakeSG();
						sg.SetNewSBs(new List<ISlottable>());
						sg.SetNewSlots(new List<Slot>());

					sg.Reset();

					Assert.That(sg.newSBs, Is.Null);
					Assert.That(sg.newSlots, Is.Null);
					}
				[TestCaseSource(typeof(ReorderAndUpdateSBsCases))]
				public void ReorderAndUpdateSBs_WhenCalled_CallsSBsSetNewSlotIDAndMoveWithin(ISlottable picked, ISlottable target, List<ISlottable> sbs, Dictionary<ISlottable, int> newSlotIDs){
					ISlotSystemManager ssm = MakeSubSSM();
						ssm.pickedSB.Returns(picked);
						ssm.targetSB.Returns(target);
					SlotGroup sg = MakeSG();
						sg.SetSSM(ssm);
						sg.SetSBs(sbs);
					
					sg.ReorderAndUpdateSBs();

					foreach(ISlottable sb in sg){
						sb.Received().MoveWithin();
						sb.Received().SetNewSlotID(newSlotIDs[sb]);
					}
					}
					class ReorderAndUpdateSBsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case0;
								ISlottable sb0_0 = MakeSubSB();
								ISlottable sb1_0 = MakeSubSB();
								ISlottable sb2_0 = MakeSubSB();
								ISlottable sb3_0 = MakeSubSB();
								ISlottable sb4_0 = MakeSubSB();
								List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{sb0_0, sb1_0, sb2_0, sb3_0, sb4_0});
								ISlottable picked_0 = sb4_0;
								ISlottable hovered_0 = sb2_0;
								Dictionary<ISlottable, int> ids_0 = new Dictionary<ISlottable,int>();
									ids_0.Add(sb0_0, 0);
									ids_0.Add(sb1_0, 1);
									ids_0.Add(sb4_0, 2);
									ids_0.Add(sb2_0, 3);
									ids_0.Add(sb3_0, 4);
								case0 = new object[]{picked_0, hovered_0, sbs_0, ids_0};
								yield return case0;
							object[] case1;
								ISlottable sb0_1 = MakeSubSB();
								ISlottable sb1_1 = MakeSubSB();
								ISlottable sb2_1 = MakeSubSB();
								ISlottable sb3_1 = MakeSubSB();
								ISlottable sb4_1 = MakeSubSB();
								List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{sb0_1, sb1_1, sb2_1, sb3_1, sb4_1});
								ISlottable picked_1 = sb0_1;
								ISlottable hovered_1 = sb4_1;
								Dictionary<ISlottable, int> ids_1 = new Dictionary<ISlottable,int>();
									ids_1.Add(sb1_1, 0);
									ids_1.Add(sb2_1, 1);
									ids_1.Add(sb3_1, 2);
									ids_1.Add(sb4_1, 3);
									ids_1.Add(sb0_1, 4);
								case1 = new object[]{picked_1, hovered_1, sbs_1, ids_1};
								yield return case1;
						}
					}				
				[Test]
				public void UpdateToRevert_WhenCalled_SetsNewSBsWithSBs(){
					List<ISlottable> sbs = CreateSBs(3);
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
					
					sg.UpdateToRevert();
					
					bool equality = sg.newSBs.MemberEquals(sbs);
					Assert.That(equality, Is.True);
					}
				[TestCase(1)]
				[TestCase(3)]
				[TestCase(10)]
				public void UpdateToRevert_WhenCalled_SetsNewSlotsBySBsCount(int count){
					List<ISlottable> sbs = CreateSBs(count);
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
					
					sg.UpdateToRevert();

					Assert.That(sg.newSlots.Count, Is.EqualTo(count));
					foreach(var slot in sg.newSlots)
						Assert.That(slot.sb, Is.Null);
					}
				[Test]
				public void UpdateToRevert_WhenCalled_DoesNotChangeSBs(){
					List<ISlottable> sbs = CreateSBs(3);
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
					
					sg.UpdateToRevert();
					
					bool equality = sg.toList.MemberEquals(sbs);
					Assert.That(equality, Is.True);
					}
				[Test]
				public void UpdateToRevert_WhenCalled_CallSBsMoveWithin(){
					List<ISlottable> sbs = CreateSBs(3);
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
					
					sg.UpdateToRevert();

					foreach(ISlottable sb in sg)
						sb.Received().MoveWithin();
					}
				[TestCaseSource(typeof(SortAndUpdateSBsCases))]
				public void SortAndUpdateSBs_SGIsNotExpandable_SortsAndCallsSBsSetNewSlotIDsAndMoveWithin(SGSorter sorter, List<ISlottable> sbs, List<ISlottable> expOrder){
					SlotGroup sg = MakeSG();
						// sg.Initialize("someSG", new SGNullFilter(), new PoolInventory(), true, 10, new SGEmptyCommand(), new SGEmptyCommand());/* this makes isExpandable false */
						sg.SetSBs(sbs);
						sg.SetSorter(sorter);
					
					sg.SortAndUpdateSBs();

					foreach(ISlottable sb in sg){
						if(sb != null){
							sb.Received().SetNewSlotID(expOrder.IndexOf(sb));
							sb.Received().MoveWithin();
						}
					}
					}
					class SortAndUpdateSBsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] itemIDOrder;
								ISlottable partsSBB_0 = MakeSubSBWithItem(MakePartsInstWithOrder(1, 1, 0));
								ISlottable bowSBA_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSBA_0 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable quiverSBA_0 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 3));
								ISlottable bowSBC_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 4));
								ISlottable packSBA_0 = MakeSubSBWithItem(MakePackInstWithOrder(0, 5));
								ISlottable shieldSBA_0 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable partsSBA_0 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 7));
								ISlottable bowSBB_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								ISlottable mWeaponSBA_0 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 9));
								List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
									quiverSBA_0,
									partsSBB_0,
									null,
									null,
									partsSBA_0,
									bowSBB_0,
									wearSBA_0,
									bowSBA_0,
									packSBA_0,
									null,
									shieldSBA_0,
									bowSBC_0,
									mWeaponSBA_0
								});
								List<ISlottable> expected_0 = new List<ISlottable>(new ISlottable[]{
									bowSBA_0,
									bowSBC_0,
									bowSBB_0,
									wearSBA_0,
									shieldSBA_0,
									mWeaponSBA_0,
									quiverSBA_0,
									packSBA_0,
									partsSBA_0,
									partsSBB_0,
									null,
									null,
									null
								});
								itemIDOrder = new object[]{new SGItemIDSorter() ,sbs_0, expected_0};
								yield return itemIDOrder;
							object[] inverseIDOrder;
								ISlottable partsSBB_1 = MakeSubSBWithItem(MakePartsInstWithOrder(1, 1, 0));
								ISlottable bowSBA_1 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSBA_1 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable quiverSBA_1 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 3));
								ISlottable bowSBC_1 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 4));
								ISlottable packSBA_1 = MakeSubSBWithItem(MakePackInstWithOrder(0, 5));
								ISlottable shieldSBA_1 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable partsSBA_1 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 7));
								ISlottable bowSBB_1 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								ISlottable mWeaponSBA_1 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 9));
								List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
									quiverSBA_1,
									partsSBB_1,
									null,
									null,
									partsSBA_1,
									bowSBB_1,
									wearSBA_1,
									bowSBA_1,
									packSBA_1,
									null,
									shieldSBA_1,
									bowSBC_1,
									mWeaponSBA_1
								});
								List<ISlottable> expected_1 = new List<ISlottable>(new ISlottable[]{
									partsSBB_1,
									partsSBA_1,
									packSBA_1,
									quiverSBA_1,
									mWeaponSBA_1,
									shieldSBA_1,
									wearSBA_1,
									bowSBB_1,
									bowSBC_1,
									bowSBA_1,
									null,
									null,
									null
								});
								inverseIDOrder = new object[]{new SGInverseItemIDSorter() ,sbs_1, expected_1};
								yield return inverseIDOrder;
							object[] acquisitionOrder;
								ISlottable partsSBB_2 = MakeSubSBWithItem(MakePartsInstWithOrder(1, 1, 0));
								ISlottable bowSBA_2 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSBA_2 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable quiverSBA_2 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 3));
								ISlottable bowSBC_2 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 4));
								ISlottable packSBA_2 = MakeSubSBWithItem(MakePackInstWithOrder(0, 5));
								ISlottable shieldSBA_2 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable partsSBA_2 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 7));
								ISlottable bowSBB_2 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								ISlottable mWeaponSBA_2 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 9));
								List<ISlottable> sbs_2 = new List<ISlottable>(new ISlottable[]{
									quiverSBA_2,
									partsSBB_2,
									null,
									null,
									partsSBA_2,
									bowSBB_2,
									wearSBA_2,
									bowSBA_2,
									packSBA_2,
									null,
									shieldSBA_2,
									bowSBC_2,
									mWeaponSBA_2
								});
								List<ISlottable> expected_2 = new List<ISlottable>(new ISlottable[]{
									partsSBB_2,
									bowSBA_2,
									wearSBA_2,
									quiverSBA_2,
									bowSBC_2,
									packSBA_2,
									shieldSBA_2,
									partsSBA_2,
									bowSBB_2,
									mWeaponSBA_2,
									null,
									null,
									null
								});
								acquisitionOrder = new object[]{new SGAcquisitionOrderSorter() ,sbs_2, expected_2};
								yield return acquisitionOrder;
						}
					}
				[TestCaseSource(typeof(SortAndUpdateSBsCases))]
				public void SortAndUpdateSBs_SGIsNotExpandable_SGNewSBsRetainsSize(SGSorter sorter, List<ISlottable> sbs, List<ISlottable> expOrder){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
						sg.SetSorter(sorter);
					
					sg.SortAndUpdateSBs();

					Assert.That(sg.newSBs.Count, Is.EqualTo(sbs.Count));
					}
				[TestCaseSource(typeof(SortAndUpdateSBsCases))]
				public void SortAndUpdateSBs_SGIsExpandableAndContainsNull_SGnewSBsSizeShrinks(SGSorter sorter, List<ISlottable> sbs, List<ISlottable> expOrder){
					SlotGroup sg = MakeSG();
						sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), sorter, 0);
						sg.SetSBs(sbs);
						sg.SetSorter(sorter);
					
					sg.SortAndUpdateSBs();
					
					int nullCount = 0;
						foreach(var sb in sbs)
							if(sb ==null) nullCount++;
					Assert.That(sg.newSBs.Count, Is.EqualTo(sbs.Count - nullCount));
					}
				[TestCaseSource(typeof(SortAndUpdateSBsCases))]
				public void SortAndUpdateSBs_SGIsExpandable_SortsAndCallsSBsSetNewSlotIDsAndMoveWithin(SGSorter sorter, List<ISlottable> sbs, List<ISlottable> expOrder){
					SlotGroup sg = MakeSG();
						sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), sorter, 0);
						sg.SetSBs(sbs);
					
					sg.SortAndUpdateSBs();

					foreach(ISlottable sb in sg){
						if(sb != null){
							sb.Received().SetNewSlotID(expOrder.IndexOf(sb));
							sb.Received().MoveWithin();
						}
					}
					}
				[Test]
				public void GetAddedForFill_SSMSG1NotThis_ReturnsSSMPickedSB(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager ssm = MakeSubSSM();
							ISlottable pickedSB = MakeSubSB();
							ssm.pickedSB.Returns(pickedSB);
							ISlotGroup otherSG = MakeSubSG();
							ssm.sg1.Returns(otherSG);
						sg.SetSSM(ssm);
					
					ISlottable actual = sg.GetAddedForFill();

					Assert.That(actual, Is.SameAs(pickedSB));
					}
				[Test]
				public void GetAddedForFill_SSMSG1IsThis_ReturnsNull(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager ssm = MakeSubSSM();
							ISlottable pickedSB = MakeSubSB();
							ssm.pickedSB.Returns(pickedSB);
							ssm.sg1.Returns(sg);
						sg.SetSSM(ssm);
					
					ISlottable actual = sg.GetAddedForFill();

					Assert.That(actual, Is.Null);
					}
				[Test]
				public void GetRemovedForFill_SSMSG1NotThis_ReturnsNull(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager ssm = MakeSubSSM();
							ISlottable pickedSB = MakeSubSB();
							ssm.pickedSB.Returns(pickedSB);
							ISlotGroup otherSG = MakeSubSG();
							ssm.sg1.Returns(otherSG);
						sg.SetSSM(ssm);
					
					ISlottable actual = sg.GetRemovedForFill();

					Assert.That(actual, Is.Null);
					}
				[Test]
				public void GetRemovedForFill_SSMSG1IsThis_RetunsSSMPickedSB(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager ssm = MakeSubSSM();
							ISlottable pickedSB = MakeSubSB();
							ssm.pickedSB.Returns(pickedSB);
							ssm.sg1.Returns(sg);
						sg.SetSSM(ssm);
					
					ISlottable actual = sg.GetRemovedForFill();

					Assert.That(actual, Is.SameAs(pickedSB));
					}
				[TestCaseSource(typeof(CreateNewSBAndFillCases))]
				public void CreateNewSBAndFill_WhenCalled_UpdateList(List<ISlottable> list, ISlottable added, int addedIndex){
					SlotGroup sg = MakeSG();
						ISlotSystemManager ssm = MakeSubSSM();
						sg.SetSSM(ssm);
						ssm.FindParent(Arg.Any<ISlottable>()).Returns(sg);
					List<ISlottable> targetList = new List<ISlottable>(list);
					sg.CreateNewSBAndFill(added.itemInst, targetList);

					ISlottable actualAdded = targetList[addedIndex];
					Assert.That(actualAdded.itemInst, Is.SameAs(added.itemInst));
					Assert.That(actualAdded.ssm, Is.SameAs(ssm));
					Assert.That(actualAdded.isDefocused, Is.True);
					Assert.That(actualAdded.isUnequipped, Is.True);
					Assert.That(actualAdded, Is.Not.Null.And.InstanceOf(typeof(Slottable)));
					}
					class CreateNewSBAndFillCases:IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] concatnated;
								ISlottable sb0_0 = MakeSubSB();
								ISlottable sb1_0 = MakeSubSB();
								ISlottable sb2_0 = MakeSubSB();
								ISlottable sb3_0 = MakeSubSB();
								ISlottable added_0 = MakeSubSB();
									BowInstance bow_0 = MakeBowInstance(0);
									added_0.itemInst.Returns(bow_0);
								List<ISlottable> list_0 = new List<ISlottable>(new ISlottable[]{
									sb0_0, 
									sb1_0, 
									sb2_0, 
									sb3_0
								});
								concatnated = new object[]{list_0, added_0, 4};
								yield return concatnated;
							object[] filled;
								ISlottable sb0_1 = MakeSubSB();
								ISlottable sb1_1 = MakeSubSB();
								ISlottable sb2_1 = MakeSubSB();
								ISlottable sb3_1 = MakeSubSB();
								ISlottable added_1 = MakeSubSB();
									BowInstance bow_1 = MakeBowInstance(0);
									added_1.itemInst.Returns(bow_1);
								List<ISlottable> list_1 = new List<ISlottable>(new ISlottable[]{
									null,
									sb0_1, 
									null,
									null,
									sb1_1, 
									sb2_1, 
									sb3_1
								});
								filled = new object[]{list_1, added_1, 0};
								yield return filled;
							object[] filledV2;
								ISlottable sb0_2 = MakeSubSB();
								ISlottable sb1_2 = MakeSubSB();
								ISlottable sb2_2 = MakeSubSB();
								ISlottable sb3_2 = MakeSubSB();
								ISlottable added_2 = MakeSubSB();
									BowInstance bow_2 = MakeBowInstance(0);
									added_2.itemInst.Returns(bow_2);
								List<ISlottable> list_2 = new List<ISlottable>(new ISlottable[]{
									sb0_2, 
									sb1_2, 
									null,
									null,
									sb2_2, 
									null,
									sb3_2
								});
								filledV2 = new object[]{list_2, added_2, 2};
								yield return filledV2;
						}
					}
				[TestCaseSource(typeof(NullifyIndexOfCases))]
				public void NullifyIndexOf_WhenCalled_FindByItemAndReplaceWithNull(List<ISlottable> list, InventoryItemInstance item, IEnumerable<ISlottable> expected){
					SlotGroup sg = MakeSG();
					List<ISlottable> targetList = new List<ISlottable>(list);

					sg.NullifyIndexOf(item, targetList);

					bool equality = targetList.MemberEquals(expected);
					Assert.That(equality, Is.True);
					}
					class NullifyIndexOfCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case0;
								ISlottable bowSB_0 = MakeSubSB();
									BowInstance bow_0 = MakeBowInstance(0);
									bowSB_0.itemInst.Returns(bow_0);
								ISlottable wearSB_0 = MakeSubSB();
									WearInstance wear_0 = MakeWearInstance(0);
									wearSB_0.itemInst.Returns(wear_0);
								ISlottable shieldSB_0 = MakeSubSB();
									ShieldInstance shield_0 = MakeShieldInstance(0);
									shieldSB_0.itemInst.Returns(shield_0);
								ISlottable mWeaponSB_0 = MakeSubSB();
									MeleeWeaponInstance mWeapon_0 = MakeMeleeWeaponInstance(0);
									mWeaponSB_0.itemInst.Returns(mWeapon_0);
								List<ISlottable> list_0 = new List<ISlottable>(new ISlottable[]{
									null,
									bowSB_0,
									null,
									wearSB_0,
									shieldSB_0,
									null,
									null,
									mWeaponSB_0
								});
								IEnumerable<ISlottable> expected_0 = new ISlottable[]{
									null,
									null,
									null,
									wearSB_0,
									shieldSB_0,
									null,
									null,
									mWeaponSB_0
								};
								case0 = new object[]{list_0, bow_0, expected_0};
								yield return case0;
							object[] case1;
								ISlottable bowSB_1 = MakeSubSB();
									BowInstance bow_1 = MakeBowInstance(0);
									bowSB_1.itemInst.Returns(bow_1);
								ISlottable wearSB_1 = MakeSubSB();
									WearInstance wear_1 = MakeWearInstance(0);
									wearSB_1.itemInst.Returns(wear_1);
								ISlottable shieldSB_1 = MakeSubSB();
									ShieldInstance shield_1 = MakeShieldInstance(0);
									shieldSB_1.itemInst.Returns(shield_1);
								ISlottable mWeaponSB_1 = MakeSubSB();
									MeleeWeaponInstance mWeapon_1 = MakeMeleeWeaponInstance(0);
									mWeaponSB_1.itemInst.Returns(mWeapon_1);
								List<ISlottable> list_1 = new List<ISlottable>(new ISlottable[]{
									null,
									bowSB_1,
									null,
									wearSB_1,
									shieldSB_1,
									null,
									null,
									mWeaponSB_1
								});
								IEnumerable<ISlottable> expected_1 = new ISlottable[]{
									null,
									bowSB_1,
									null,
									wearSB_1,
									shieldSB_1,
									null,
									null,
									mWeaponSB_1
								};
								case1 = new object[]{list_1, null, expected_1};
								yield return case1;
							

						}	
					}
				[TestCaseSource(typeof(FillAndUpdateSBs_SGNotPoolAndSSMSG1NotThisCases))]
				public void FillAndUpdateSBs_AddedNotNull_CreateAndSetsNewSBAndAddItToTheEndOfSGSBs(bool isExpandable, bool isAutoSort, List<ISlottable> sbs){
					SlotGroup sg = MakeSG();
						sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), Substitute.For<SGSorter>(), isExpandable?0: 10);
						ISlotGroup otherSG = MakeSubSG();
						ISlotSystemManager ssm = MakeSubSSM();
							ssm.sg1.Returns(otherSG);
							ISlottable pickedSB = MakeSubSB();
								BowInstance bow = MakeBowInstance(0);
								pickedSB.itemInst.Returns(bow);
								ssm.pickedSB.Returns(pickedSB);
								ISlotSystemBundle poolBundle = MakeSubBundle();
									poolBundle.ContainsInHierarchy(sg).Returns(false);
								ssm.poolBundle.Returns(poolBundle);
							ssm.FindParent(Arg.Any<ISlottable>()).Returns(sg);
						sg.SetSSM(ssm);
						sg.SetSBs(sbs);
						sg.ToggleAutoSort(isAutoSort);
					
					sg.FillAndUpdateSBs();

					ISlottable added = sg.toList[sbs.Count];
					Assert.That(added, Is.Not.Null);
					Assert.That(added.ssm, Is.SameAs(ssm));
					Assert.That(added.isDefocused, Is.True);
					Assert.That(added.isUnequipped, Is.True);
					}
					class FillAndUpdateSBs_SGNotPoolAndSSMSG1NotThisCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] T_T_whole;
								ISlottable sb0_0 = MakeSubSB();
								ISlottable sb1_0 = MakeSubSB();
								ISlottable sb2_0 = MakeSubSB();
								ISlottable sb3_0 = MakeSubSB();
								List<ISlottable> whole_0 = new List<ISlottable>(new ISlottable[]{
									sb0_0, 
									sb1_0, 
									sb2_0, 
									sb3_0
								});
								T_T_whole = new object[]{true, true, whole_0};
								yield return T_T_whole;
							object[] T_F_whole;
								ISlottable sb0_1 = MakeSubSB();
								ISlottable sb1_1 = MakeSubSB();
								ISlottable sb2_1 = MakeSubSB();
								ISlottable sb3_1 = MakeSubSB();
								List<ISlottable> whole_1 = new List<ISlottable>(new ISlottable[]{
									sb0_1, 
									sb1_1, 
									sb2_1, 
									sb3_1
								});
								T_F_whole = new object[]{true, false, whole_1};
								yield return T_F_whole;
							object[] F_T_whole;
								ISlottable sb0_2 = MakeSubSB();
								ISlottable sb1_2 = MakeSubSB();
								ISlottable sb2_2 = MakeSubSB();
								ISlottable sb3_2 = MakeSubSB();
								List<ISlottable> whole_2 = new List<ISlottable>(new ISlottable[]{
									sb0_2, 
									sb1_2, 
									sb2_2, 
									sb3_2
								});
								F_T_whole = new object[]{false, true, whole_2};
								yield return F_T_whole;
							object[] F_F_whole;
								ISlottable sb0_3 = MakeSubSB();
								ISlottable sb1_3 = MakeSubSB();
								ISlottable sb2_3 = MakeSubSB();
								ISlottable sb3_3 = MakeSubSB();
								List<ISlottable> whole_3 = new List<ISlottable>(new ISlottable[]{
									sb0_3, 
									sb1_3, 
									sb2_3, 
									sb3_3
								});
								F_F_whole = new object[]{false, false, whole_3};
								yield return F_F_whole;
							object[] T_T_hasEmpty;
								ISlottable sb0_4 = MakeSubSB();
								ISlottable sb1_4 = MakeSubSB();
								ISlottable sb2_4 = MakeSubSB();
								ISlottable sb3_4 = MakeSubSB();
								List<ISlottable> hasEmpty_4 = new List<ISlottable>(new ISlottable[]{
									null,
									sb0_4, 
									sb1_4, 
									null,
									null,
									sb2_4, 
									null,
									sb3_4,
									null
								});
								T_T_hasEmpty = new object[]{true, true, hasEmpty_4};
								yield return T_T_hasEmpty;
							object[] T_F_hasEmpty;
								ISlottable sb0_5 = MakeSubSB();
								ISlottable sb1_5 = MakeSubSB();
								ISlottable sb2_5 = MakeSubSB();
								ISlottable sb3_5 = MakeSubSB();
								List<ISlottable> hasEmpty_5 = new List<ISlottable>(new ISlottable[]{
									null,
									sb0_5, 
									sb1_5, 
									null,
									null,
									sb2_5, 
									null,
									sb3_5,
									null
								});
								T_F_hasEmpty = new object[]{true, false, hasEmpty_5};
								yield return T_F_hasEmpty;
							object[] F_T_hasEmpty;
								ISlottable sb0_6 = MakeSubSB();
								ISlottable sb1_6 = MakeSubSB();
								ISlottable sb2_6 = MakeSubSB();
								ISlottable sb3_6 = MakeSubSB();
								List<ISlottable> hasEmpty_6 = new List<ISlottable>(new ISlottable[]{
									null,
									sb0_6, 
									sb1_6, 
									null,
									null,
									sb2_6, 
									null,
									sb3_6,
									null
								});
								F_T_hasEmpty = new object[]{false, true, hasEmpty_6};
								yield return F_T_hasEmpty;
							object[] F_F_hasEmpty;
								ISlottable sb0_7 = MakeSubSB();
								ISlottable sb1_7 = MakeSubSB();
								ISlottable sb2_7 = MakeSubSB();
								ISlottable sb3_7 = MakeSubSB();
								List<ISlottable> hasEmpty_7 = new List<ISlottable>(new ISlottable[]{
									null,
									sb0_7, 
									sb1_7, 
									null,
									null,
									sb2_7, 
									null,
									sb3_7,
									null
								});
								F_F_hasEmpty = new object[]{false, false, hasEmpty_7};
								yield return F_F_hasEmpty;
						}
					}
				[TestCaseSource(typeof(FillAndUpdateSBs_VariousCases))]
				public void FillAndUpdateSBs_Various_SetsNewSBsAccordingly(
					bool added,
					bool isPool, 
					bool isAutoSort, 
					bool isExpandable, 
					ISlottable pickedSB, 
					SGSorter sorter, 
					List<ISlottable> sbs, 
					List<ISlottable> expNewSBs, 
					int idAtAdded)
				{
					SlotGroup sg = MakeSG();
						ISlotGroup otherSG = MakeSubSG();
						ISlotSystemManager ssm = MakeSubSSM();
							if(added)
								ssm.sg1.Returns(otherSG);
							else
								ssm.sg1.Returns(sg);
							ssm.pickedSB.Returns(pickedSB);
								ISlotSystemBundle poolBundle = MakeSubBundle();
									if(isPool)
										poolBundle.ContainsInHierarchy(sg).Returns(true);
									else
										poolBundle.ContainsInHierarchy(sg).Returns(false);
							ssm.poolBundle.Returns(poolBundle);
							ssm.FindParent(Arg.Any<ISlottable>()).Returns(sg);
					sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), sorter, isExpandable?0: 10);
					sg.SetSSM(ssm);
					sg.SetSBs(sbs);
					sg.ToggleAutoSort(isAutoSort);

					sg.FillAndUpdateSBs();

					List<ISlottable> expected = new List<ISlottable>(expNewSBs);
					if(added){
						ISlottable addedActual = sg.toList[sbs.Count];
						expected[idAtAdded] = addedActual;
					}
					bool equality = sg.newSBs.MemberEquals(expected);
					Assert.That(equality, Is.True);
					}
					class FillAndUpdateSBs_VariousCases:IEnumerable{
						public IEnumerator GetEnumerator(){
							/* Sbs */
							object[] NoAddNoRemNoSort_isPoolTrue;
								ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_0 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_0 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_0 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_0 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_0 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								ISlottable bow1SB_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								List<ISlottable> list_0 = new List<ISlottable>(new ISlottable[]{
									partsSB_0,
									bow2SB_0,
									wearSB_0,
									null,
									mWeaponSB_0,
									null,
									null,
									packSB_0,
									null,
									quiverSB_0,
									shieldSB_0,
									bow0SB_0,
									null
								});
								NoAddNoRemNoSort_isPoolTrue = new object[]{
									false, 
									true, 
									false, 
									false, 
									bow1SB_0, 
									new SGItemIDSorter(), 
									list_0,
									list_0,
									-1
								};
								yield return NoAddNoRemNoSort_isPoolTrue;
							object[] NoAddNoRemSortNoResize_isPoolTrue_isAutoSortTrue_isExpandableFalse;
								ISlottable partsSB_1 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_1 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_1 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_1 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_1 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_1 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_1 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_1 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								ISlottable bow1SB_1 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								List<ISlottable> list_1 = new List<ISlottable>(new ISlottable[]{
									partsSB_1,
									bow2SB_1,
									wearSB_1,
									null,
									mWeaponSB_1,
									null,
									null,
									packSB_1,
									null,
									quiverSB_1,
									shieldSB_1,
									bow0SB_1,
									null
								});
								List<ISlottable> expected_1 = new List<ISlottable>(){
									bow2SB_1,
									bow0SB_1,
									wearSB_1,
									shieldSB_1,
									mWeaponSB_1,
									quiverSB_1,
									packSB_1,
									partsSB_1,
									null,
									null,
									null,
									null,
									null
								};
								NoAddNoRemSortNoResize_isPoolTrue_isAutoSortTrue_isExpandableFalse = new object[]{
									false, 
									true, 
									true, 
									false, 
									bow1SB_1, 
									new SGItemIDSorter(), 
									list_1, 
									expected_1, 
									-1
								};
								yield return NoAddNoRemSortNoResize_isPoolTrue_isAutoSortTrue_isExpandableFalse;
							object[] NoAddNoRemSortResize_isPoolTrue_isAutoSortTrue_isExpandableTrue;
								ISlottable partsSB_2 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_2 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_2 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_2 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_2 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_2 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_2 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_2 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								ISlottable bow1SB_2 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								List<ISlottable> list_2 = new List<ISlottable>(new ISlottable[]{
									partsSB_2,
									bow2SB_2,
									wearSB_2,
									null,
									mWeaponSB_2,
									null,
									null,
									packSB_2,
									null,
									quiverSB_2,
									shieldSB_2,
									bow0SB_2,
									null
								});
								List<ISlottable> expected_2 = new List<ISlottable>(){
									bow2SB_2,
									bow0SB_2,
									wearSB_2,
									shieldSB_2,
									mWeaponSB_2,
									quiverSB_2,
									packSB_2,
									partsSB_2
								};
								NoAddNoRemSortResize_isPoolTrue_isAutoSortTrue_isExpandableTrue = new object[]{
									false, 
									true, 
									true, 
									true, 
									bow1SB_2, 
									new SGItemIDSorter(), 
									list_2, 
									expected_2, 
									-1
								};
								yield return NoAddNoRemSortResize_isPoolTrue_isAutoSortTrue_isExpandableTrue;
							object[] AddNoSort_isPoolFalse_isAutoSortFalse;
								ISlottable partsSB_3 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_3 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_3 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_3 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_3 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_3 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_3 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_3 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								ISlottable bow1SB_3 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								List<ISlottable> list_3 = new List<ISlottable>(new ISlottable[]{
									partsSB_3,
									bow2SB_3,
									wearSB_3,
									null,
									mWeaponSB_3,
									null,
									null,
									packSB_3,
									null,
									quiverSB_3,
									shieldSB_3,
									bow0SB_3,
									null
								});
								List<ISlottable> expected_3 = new List<ISlottable>(new ISlottable[]{
									partsSB_3,
									bow2SB_3,
									wearSB_3,
									bow1SB_3,
									mWeaponSB_3,
									null,
									null,
									packSB_3,
									null,
									quiverSB_3,
									shieldSB_3,
									bow0SB_3,
									null
								});
								AddNoSort_isPoolFalse_isAutoSortFalse = new object[]{
									true, 
									false, 
									false, 
									false, 
									bow1SB_3, 
									new SGItemIDSorter(), 
									list_3, 
									expected_3, 
									3
								};
								yield return AddNoSort_isPoolFalse_isAutoSortFalse;
							object[] AddNoSortOnWhole_isPoolFalse_isAutoSortFalse;
								ISlottable partsSB_4 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_4 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_4 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_4 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_4 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_4 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_4 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_4 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								ISlottable bow1SB_4 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								List<ISlottable> list_4 = new List<ISlottable>(new ISlottable[]{
									partsSB_4,
									bow2SB_4,
									wearSB_4,
									mWeaponSB_4,
									packSB_4,
									quiverSB_4,
									shieldSB_4,
									bow0SB_4
								});
								List<ISlottable> expected_4 = new List<ISlottable>(new ISlottable[]{
									partsSB_4, 
									bow2SB_4, 
									wearSB_4, 
									mWeaponSB_4, 
									packSB_4, 
									quiverSB_4, 
									shieldSB_4, 
									bow0SB_4,
									bow1SB_4
								});
								AddNoSortOnWhole_isPoolFalse_isAutoSortFalse = new object[]{
									true, 
									false, 
									false, 
									false, 
									bow1SB_4, 
									new SGItemIDSorter(), 
									list_4, 
									expected_4, 
									8
								};
								yield return AddNoSortOnWhole_isPoolFalse_isAutoSortFalse;
							object[] AddSortNoResize_isPoolFalse_isAutoSortTrue_isExpandableFalse;
								ISlottable partsSB_5 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_5 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_5 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_5 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_5 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_5 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_5 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_5 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								ISlottable bow1SB_5 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								List<ISlottable> list_5 = new List<ISlottable>(new ISlottable[]{
									partsSB_5,
									bow2SB_5,
									wearSB_5,
									null,
									mWeaponSB_5,
									null,
									null,
									packSB_5,
									null,
									quiverSB_5,
									shieldSB_5,
									bow0SB_5,
									null
								});
								List<ISlottable> expected_5 = new List<ISlottable>(new ISlottable[]{
									bow2SB_5,
									bow0SB_5,
									bow1SB_5,
									wearSB_5,
									shieldSB_5,
									mWeaponSB_5,
									quiverSB_5,
									packSB_5,
									partsSB_5,
									null,
									null,
									null,
									null
								});
								AddSortNoResize_isPoolFalse_isAutoSortTrue_isExpandableFalse = new object[]{
									true, 
									false, 
									true, 
									false, 
									bow1SB_5, 
									new SGItemIDSorter(), 
									list_5, 
									expected_5, 
									2
								};
								yield return AddSortNoResize_isPoolFalse_isAutoSortTrue_isExpandableFalse;
							object[] AddSortResize_isPoolFalse_isAutoSortTrue_isExpandableTrue;
								ISlottable partsSB_6 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_6 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_6 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_6 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_6 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_6 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_6 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_6 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								ISlottable bow1SB_6 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								List<ISlottable> list_6 = new List<ISlottable>(new ISlottable[]{
									partsSB_6,
									bow2SB_6,
									wearSB_6,
									null,
									mWeaponSB_6,
									null,
									null,
									packSB_6,
									null,
									quiverSB_6,
									shieldSB_6,
									bow0SB_6,
									null
								});
								List<ISlottable> expected_6 = new List<ISlottable>(new ISlottable[]{
									bow2SB_6,
									bow0SB_6,
									bow1SB_6,
									wearSB_6,
									shieldSB_6,
									mWeaponSB_6,
									quiverSB_6,
									packSB_6,
									partsSB_6
								});
								AddSortResize_isPoolFalse_isAutoSortTrue_isExpandableTrue = new object[]{
									true, 
									false, 
									true, 
									true, 
									bow1SB_6, 
									new SGItemIDSorter(), 
									list_6, 
									expected_6, 
									2
								};
								yield return AddSortResize_isPoolFalse_isAutoSortTrue_isExpandableTrue;
							object[] RemoveNoSort_isPoolFalse_isAutoSortFalse;
								ISlottable partsSB_7 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_7 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_7 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_7 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_7 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_7 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_7 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_7 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								List<ISlottable> list_7 = new List<ISlottable>(new ISlottable[]{
									partsSB_7,
									bow2SB_7,
									wearSB_7,
									null,
									mWeaponSB_7,
									null,
									null,
									packSB_7,
									null,
									quiverSB_7,
									shieldSB_7,
									bow0SB_7,
									null
								});
								List<ISlottable> expected_7 = new List<ISlottable>(new ISlottable[]{
									partsSB_7,
									bow2SB_7,
									wearSB_7,
									null,
									mWeaponSB_7,
									null,
									null,
									packSB_7,
									null,
									quiverSB_7,
									shieldSB_7,
									null,
									null
								});
								RemoveNoSort_isPoolFalse_isAutoSortFalse = new object[]{
									false, 
									false, 
									false, 
									false, 
									bow0SB_7, 
									new SGItemIDSorter(), 
									list_7, 
									expected_7, 
									-1
								};
								yield return RemoveNoSort_isPoolFalse_isAutoSortFalse;
							object[] RemovSortNoResize_isPoolFalse_isAutoSortTrue_isExpandableFalse;
								ISlottable partsSB_8 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_8 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_8 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_8 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_8 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_8 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_8 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_8 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								List<ISlottable> list_8 = new List<ISlottable>(new ISlottable[]{
									partsSB_8,
									bow2SB_8,
									wearSB_8,
									null,
									mWeaponSB_8,
									null,
									null,
									packSB_8,
									null,
									quiverSB_8,
									shieldSB_8,
									bow0SB_8,
									null
								});
								List<ISlottable> expected_8 = new List<ISlottable>(new ISlottable[]{
									bow2SB_8,
									wearSB_8,
									shieldSB_8,
									mWeaponSB_8,
									quiverSB_8,
									packSB_8,
									partsSB_8,
									null,
									null,
									null,
									null,
									null,
									null
								});
								RemovSortNoResize_isPoolFalse_isAutoSortTrue_isExpandableFalse = new object[]{
									false, 
									false, 
									true, 
									false, 
									bow0SB_8, 
									new SGItemIDSorter(), 
									list_8, 
									expected_8, 
									-1
								};
								yield return RemovSortNoResize_isPoolFalse_isAutoSortTrue_isExpandableFalse;
							object[] RemovSortResize_isPoolFalse_isAutoSortTrue_isExpandableTrue;
								ISlottable partsSB_9 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_9 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_9 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_9 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_9 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_9 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_9 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_9 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								List<ISlottable> list_9 = new List<ISlottable>(new ISlottable[]{
									partsSB_9,
									bow2SB_9,
									wearSB_9,
									null,
									mWeaponSB_9,
									null,
									null,
									packSB_9,
									null,
									quiverSB_9,
									shieldSB_9,
									bow0SB_9,
									null
								});
								List<ISlottable> expected_9 = new List<ISlottable>(new ISlottable[]{
									bow2SB_9,
									wearSB_9,
									shieldSB_9,
									mWeaponSB_9,
									quiverSB_9,
									packSB_9,
									partsSB_9,
								});
								RemovSortResize_isPoolFalse_isAutoSortTrue_isExpandableTrue = new object[]{
									false, 
									false, 
									true, 
									true, 
									bow0SB_9, 
									new SGItemIDSorter(), 
									list_9, 
									expected_9, 
									-1
								};
								yield return RemovSortResize_isPoolFalse_isAutoSortTrue_isExpandableTrue;
						}
					}
				[Test]
				public void GetAddedForSwap_SSMSG1This_ReturnsSSMTargetSB(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager ssm = MakeSubSSM();
							ISlottable targetSB = MakeSubSB();
							ssm.targetSB.Returns(targetSB);
							ssm.sg1.Returns(sg);
						sg.SetSSM(ssm);

					ISlottable actual = sg.GetAddedForSwap();

					Assert.That(actual, Is.SameAs(targetSB));
					}
				[Test]
				public void GetAddedForSwap_SSMSG1NotThis_ReturnsSSMPickedSB(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager ssm = MakeSubSSM();
							ISlottable pickedSB = MakeSubSB();
							ssm.pickedSB.Returns(pickedSB);
							ISlotGroup otherSG = MakeSubSG();
							ssm.sg1.Returns(otherSG);
						sg.SetSSM(ssm);

					ISlottable actual = sg.GetAddedForSwap();

					Assert.That(actual, Is.SameAs(pickedSB));
					}
				[Test]
				public void GetRemovedForSwap_SSMSG1This_ReturnsSSMPickedSB(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager ssm = MakeSubSSM();
							ISlottable pickedSB = MakeSubSB();
							ssm.pickedSB.Returns(pickedSB);
							ssm.sg1.Returns(sg);
						sg.SetSSM(ssm);

					ISlottable actual = sg.GetRemovedForSwap();

					Assert.That(actual, Is.SameAs(pickedSB));
					}
				[Test]
				public void GetRemovedForSwap_SSMSG1NotThis_ReturnsSSMTargetSB(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager ssm = MakeSubSSM();
							ISlottable targetSB = MakeSubSB();
							ssm.targetSB.Returns(targetSB);
							ISlotGroup otherSG = MakeSubSG();
							ssm.sg1.Returns(otherSG);
						sg.SetSSM(ssm);

					ISlottable actual = sg.GetRemovedForSwap();

					Assert.That(actual, Is.SameAs(targetSB));
					}
				[Test]
				public void SwapAndUpdateSBs_SSMSG1This_CreateAndAddNewSBFromSSMTargetSBToTheEndOfSBs(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager ssm = MakeSubSSM();
							ISlottable targetSB = MakeSubSB();
								BowInstance bow = MakeBowInstance(0);
								targetSB.itemInst.Returns(bow);
							ISlottable pickedSB = MakeSubSB();
							List<ISlottable> sbs = CreateSBs(3);
								sbs.Add(pickedSB);
							ssm.targetSB.Returns(targetSB);
							ssm.pickedSB.Returns(pickedSB);
							ssm.sg1.Returns(sg);
						sg.SetSSM(ssm);
						sg.SetSBs(sbs);
						sg.ToggleAutoSort(false);
					
					sg.SwapAndUpdateSBs();

					ISlottable actualAdded = sg.toList[sg.toList.Count -1];
					Assert.That(actualAdded.itemInst, Is.SameAs(bow));
					Assert.That(actualAdded.ssm, Is.SameAs(ssm));
					Assert.That(actualAdded.isDefocused, Is.True);
					Assert.That(actualAdded.isUnequipped, Is.True);
					}
				[Test]
				public void SwapAndUpdateSBs_SSMSG1NotThis_CreateAndAddNewSBFromSSMPickedSBToTheEndOfSBs(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager ssm = MakeSubSSM();
							ISlottable targetSB = MakeSubSB();
							List<ISlottable> sbs = CreateSBs(3);
								sbs.Add(targetSB);
							ISlottable pickedSB = MakeSubSB();
								BowInstance bow = MakeBowInstance(0);
								pickedSB.itemInst.Returns(bow);
							ssm.targetSB.Returns(targetSB);
							ssm.pickedSB.Returns(pickedSB);
							ISlotGroup otherSG = MakeSubSG();
							ssm.sg1.Returns(otherSG);
						sg.SetSSM(ssm);
						sg.SetSBs(sbs);
						sg.ToggleAutoSort(false);
					
					sg.SwapAndUpdateSBs();	

					ISlottable actualAdded = sg.toList[sg.toList.Count -1];
					Assert.That(actualAdded.itemInst, Is.SameAs(bow));
					Assert.That(actualAdded.ssm, Is.SameAs(ssm));
					Assert.That(actualAdded.isDefocused, Is.True);
					Assert.That(actualAdded.isUnequipped, Is.True);
					}
				[TestCaseSource(typeof(SwapAndUpdateSBsCases))]
				public void SwapAndUpadteSBs_Various_SetsNewSBsAccordingly(
					bool isPool, 
					bool isAutoSort, 
					bool isExpandable, 
					SGSorter sorter, 
					bool sg1This,ISlottable added, 
					ISlottable removed, 
					List<ISlottable> sbs, 
					List<ISlottable> expected, 
					int indexAtAdded)
					{
					SlotGroup sg = MakeSG();
						sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), sorter, isExpandable?0: 10);
						ISlotSystemManager ssm = MakeSubSSM();
							ISlottable targetSB = MakeSubSB();
							ssm.targetSB.Returns(targetSB);
							ISlotGroup otherSG = MakeSubSG();
							if(sg1This){
								ssm.sg1.Returns(sg);
								ssm.targetSB.Returns(added);
								ssm.pickedSB.Returns(removed);
							}
							else{
								ssm.sg1.Returns(otherSG);
								ssm.targetSB.Returns(removed);
								ssm.pickedSB.Returns(added);
							}
							ISlotSystemBundle pBun = MakeSubBundle();
								pBun.ContainsInHierarchy(sg).Returns(isPool);
							ssm.poolBundle.Returns(pBun);
						sg.SetSSM(ssm);
						sg.SetSBs(sbs);
						sg.ToggleAutoSort(isAutoSort);
					
					sg.SwapAndUpdateSBs();
					
					List<ISlottable> expList = new List<ISlottable>(expected);
					if(!sg.isPool){
						ISlottable actualAdded = sg.toList[sg.toList.Count -1];
						expList[indexAtAdded] = actualAdded;
					}
					List<ISlottable> actual = sg.newSBs;
					
					bool equality = actual.MemberEquals(expList);
					Assert.That(equality, Is.True);
					}
					class SwapAndUpdateSBsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							SGSorter idSorter = new SGItemIDSorter();
							/* SBs */
							object[] NoAddNoRemNoSort_isPoolTrue_isAutoSortFalse;
								ISlottable bowSB_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 0));
								ISlottable bow1SB_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable bow2SB_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 2));
								ISlottable wearSB_0 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 3));
								ISlottable shieldSB_0 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 4));
								ISlottable mWeaponSB_0 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 5));
								ISlottable quiverSB_0 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 6));
								ISlottable packSB_0 = MakeSubSBWithItem(MakePackInstWithOrder(0, 7));
								ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 8));
								ISlottable wear1SB_0 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 9));
								IList<ISlottable> list_0 = new List<ISlottable>(new ISlottable[]{
									null,
									quiverSB_0,
									bow1SB_0,
									null,
									bow2SB_0,
									mWeaponSB_0,
									null,
									bowSB_0,
									null,
									wearSB_0,
									null,
									shieldSB_0,
									packSB_0,
									null,
									null,
									partsSB_0
								});
								NoAddNoRemNoSort_isPoolTrue_isAutoSortFalse = new object[]{
									true, 
									false, 
									false, 
									idSorter, 
									true, 
									wear1SB_0, 
									bow2SB_0, 
									list_0, 
									list_0, 
									-1
								};
								yield return NoAddNoRemNoSort_isPoolTrue_isAutoSortFalse;
							object[] NoAddNoRemSortNoResize_isPoolTrue_isAutoSortTrue_isExpandableFalse;
								ISlottable bowSB_1 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 0));
								ISlottable bow1SB_1 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable bow2SB_1 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 2));
								ISlottable wearSB_1 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 3));
								ISlottable shieldSB_1 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 4));
								ISlottable mWeaponSB_1 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 5));
								ISlottable quiverSB_1 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 6));
								ISlottable packSB_1 = MakeSubSBWithItem(MakePackInstWithOrder(0, 7));
								ISlottable partsSB_1 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 8));
								ISlottable wear1SB_1 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 9));
								IList<ISlottable> list_1 = new List<ISlottable>(new ISlottable[]{
									null,
									quiverSB_1,
									bow1SB_1,
									null,
									bow2SB_1,
									mWeaponSB_1,
									null,
									bowSB_1,
									null,
									wearSB_1,
									null,
									shieldSB_1,
									packSB_1,
									null,
									null,
									partsSB_1
								});
								List<ISlottable> expected_1 = new List<ISlottable>(new ISlottable[]{
									bowSB_1,
									bow1SB_1,
									bow2SB_1,
									wearSB_1,
									shieldSB_1,
									mWeaponSB_1,
									quiverSB_1,
									packSB_1,
									partsSB_1,
									null,
									null,
									null,
									null,
									null,
									null,
									null
								});
								NoAddNoRemSortNoResize_isPoolTrue_isAutoSortTrue_isExpandableFalse = new object[]{
									true, 
									true, 
									false, 
									idSorter, 
									true, 
									wear1SB_1, 
									bow2SB_1, 
									list_1, 
									expected_1, 
									-1
								};
								yield return NoAddNoRemSortNoResize_isPoolTrue_isAutoSortTrue_isExpandableFalse;
							object[] NoAddNoRemSortResize_isPoolTrue_isAutoSortTrue_isExpandableTrue;
								ISlottable bowSB_2 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 0));
								ISlottable bow1SB_2 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable bow2SB_2 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 2));
								ISlottable wearSB_2 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 3));
								ISlottable shieldSB_2 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 4));
								ISlottable mWeaponSB_2 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 5));
								ISlottable quiverSB_2 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 6));
								ISlottable packSB_2 = MakeSubSBWithItem(MakePackInstWithOrder(0, 7));
								ISlottable partsSB_2 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 8));
								ISlottable wear1SB_2 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 9));
								IList<ISlottable> list_2 = new List<ISlottable>(new ISlottable[]{
									null,
									quiverSB_2,
									bow1SB_2,
									null,
									bow2SB_2,
									mWeaponSB_2,
									null,
									bowSB_2,
									null,
									wearSB_2,
									null,
									shieldSB_2,
									packSB_2,
									null,
									null,
									partsSB_2
								});
								List<ISlottable> expected_2 = new List<ISlottable>(new ISlottable[]{
									bowSB_2,
									bow1SB_2,
									bow2SB_2,
									wearSB_2,
									shieldSB_2,
									mWeaponSB_2,
									quiverSB_2,
									packSB_2,
									partsSB_2
								});
								NoAddNoRemSortResize_isPoolTrue_isAutoSortTrue_isExpandableTrue = new object[]{
									true, 
									true, 
									true, 
									idSorter, 
									true, 
									wear1SB_2, 
									bow2SB_2, 
									list_2, 
									expected_2, 
									-1
								};
								yield return NoAddNoRemSortResize_isPoolTrue_isAutoSortTrue_isExpandableTrue;
							object[] SG1ThisNoSort_isPoolFalse_isAutoSortFalse;
								ISlottable bowSB_3 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 0));
								ISlottable bow1SB_3 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable bow2SB_3 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 2));
								ISlottable wearSB_3 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 3));
								ISlottable shieldSB_3 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 4));
								ISlottable mWeaponSB_3 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 5));
								ISlottable quiverSB_3 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 6));
								ISlottable packSB_3 = MakeSubSBWithItem(MakePackInstWithOrder(0, 7));
								ISlottable partsSB_3 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 8));
								ISlottable wear1SB_3 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 9));
								IList<ISlottable> list_3 = new List<ISlottable>(new ISlottable[]{
									null,
									quiverSB_3,
									bow1SB_3,
									null,
									bow2SB_3,
									mWeaponSB_3,
									null,
									bowSB_3,
									null,
									wearSB_3,
									null,
									shieldSB_3,
									packSB_3,
									null,
									null,
									partsSB_3
								});
								IList<ISlottable> expected_3 = new List<ISlottable>(new ISlottable[]{
									null,
									quiverSB_3,
									bow1SB_3,
									null,
									wear1SB_3,
									mWeaponSB_3,
									null,
									bowSB_3,
									null,
									wearSB_3,
									null,
									shieldSB_3,
									packSB_3,
									null,
									null,
									partsSB_3
								});
								SG1ThisNoSort_isPoolFalse_isAutoSortFalse = new object[]{
									false, 
									false, 
									false, 
									idSorter, 
									true, 
									wear1SB_3, 
									bow2SB_3, 
									list_3, 
									expected_3, 
									4
								};
								yield return SG1ThisNoSort_isPoolFalse_isAutoSortFalse;
							object[] SG1NotThisNoSort_isPoolFalse_isAutoSortFalse;
								ISlottable bowSB_4 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 0));
								ISlottable bow1SB_4 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable bow2SB_4 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 2));
								ISlottable wearSB_4 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 3));
								ISlottable shieldSB_4 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 4));
								ISlottable mWeaponSB_4 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 5));
								ISlottable quiverSB_4 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 6));
								ISlottable packSB_4 = MakeSubSBWithItem(MakePackInstWithOrder(0, 7));
								ISlottable partsSB_4 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 8));
								ISlottable wear1SB_4 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 9));
								IList<ISlottable> list_4 = new List<ISlottable>(new ISlottable[]{
									null,
									quiverSB_4,
									bow1SB_4,
									null,
									bow2SB_4,
									mWeaponSB_4,
									null,
									bowSB_4,
									null,
									wearSB_4,
									null,
									shieldSB_4,
									packSB_4,
									null,
									null,
									partsSB_4
								});
								IList<ISlottable> expected_4 = new List<ISlottable>(new ISlottable[]{
									null,
									quiverSB_4,
									bow1SB_4,
									null,
									wear1SB_4,
									mWeaponSB_4,
									null,
									bowSB_4,
									null,
									wearSB_4,
									null,
									shieldSB_4,
									packSB_4,
									null,
									null,
									partsSB_4
								});
								SG1NotThisNoSort_isPoolFalse_isAutoSortFalse = new object[]{
									false, 
									false, 
									false, 
									idSorter, 
									false, 
									wear1SB_4, 
									bow2SB_4, 
									list_4, 
									expected_4, 
									4
								};
								yield return SG1NotThisNoSort_isPoolFalse_isAutoSortFalse;
							object[] SG1ThisSortNoResize_isPoolFalse_isAutoSorTrue_isExpandableFalse;
								ISlottable bowSB_5 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 0));
								ISlottable bow1SB_5 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable bow2SB_5 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 2));
								ISlottable wearSB_5 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 3));
								ISlottable shieldSB_5 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 4));
								ISlottable mWeaponSB_5 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 5));
								ISlottable quiverSB_5 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 6));
								ISlottable packSB_5 = MakeSubSBWithItem(MakePackInstWithOrder(0, 7));
								ISlottable partsSB_5 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 8));
								ISlottable wear1SB_5 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 9));
								IList<ISlottable> list_5 = new List<ISlottable>(new ISlottable[]{
									null,
									quiverSB_5,
									bow1SB_5,
									null,
									bow2SB_5,
									mWeaponSB_5,
									null,
									bowSB_5,
									null,
									wearSB_5,
									null,
									shieldSB_5,
									packSB_5,
									null,
									null,
									partsSB_5
								});
								IList<ISlottable> expected_5 = new List<ISlottable>(new ISlottable[]{
									bowSB_5,
									bow1SB_5,
									wearSB_5,
									wear1SB_5,
									shieldSB_5,
									mWeaponSB_5,
									quiverSB_5,
									packSB_5,
									partsSB_5,
									null,
									null,
									null,
									null,
									null,
									null,
									null
								});
								SG1ThisSortNoResize_isPoolFalse_isAutoSorTrue_isExpandableFalse = new object[]{
									false, 
									true, 
									false, 
									idSorter, 
									true, 
									wear1SB_5, 
									bow2SB_5, 
									list_5, 
									expected_5, 
									3
								};
								yield return SG1ThisSortNoResize_isPoolFalse_isAutoSorTrue_isExpandableFalse;
							object[] SG1NotThisSortNoResize_isPoolFalse_isAutoSorTrue_isExpandableFalse;
								ISlottable bowSB_6 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 0));
								ISlottable bow1SB_6 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable bow2SB_6 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 2));
								ISlottable wearSB_6 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 3));
								ISlottable shieldSB_6 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 4));
								ISlottable mWeaponSB_6 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 5));
								ISlottable quiverSB_6 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 6));
								ISlottable packSB_6 = MakeSubSBWithItem(MakePackInstWithOrder(0, 7));
								ISlottable partsSB_6 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 8));
								ISlottable wear1SB_6 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 9));
								IList<ISlottable> list_6 = new List<ISlottable>(new ISlottable[]{
									null,
									quiverSB_6,
									bow1SB_6,
									null,
									bow2SB_6,
									mWeaponSB_6,
									null,
									bowSB_6,
									null,
									wearSB_6,
									null,
									shieldSB_6,
									packSB_6,
									null,
									null,
									partsSB_6
								});
								IList<ISlottable> expected_6 = new List<ISlottable>(new ISlottable[]{
									bowSB_6,
									bow1SB_6,
									wearSB_6,
									wear1SB_6,
									shieldSB_6,
									mWeaponSB_6,
									quiverSB_6,
									packSB_6,
									partsSB_6,
									null,
									null,
									null,
									null,
									null,
									null,
									null
								});
								SG1NotThisSortNoResize_isPoolFalse_isAutoSorTrue_isExpandableFalse = new object[]{
									false, 
									true, 
									false, 
									idSorter, 
									false, 
									wear1SB_6, 
									bow2SB_6, 
									list_6, 
									expected_6, 
									3
								};
								yield return SG1NotThisSortNoResize_isPoolFalse_isAutoSorTrue_isExpandableFalse;
							object[] SG1ThisSortResize_isPoolFalse_isAutoSorTrue_isExpandableTrue;
								ISlottable bowSB_7 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 0));
								ISlottable bow1SB_7 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable bow2SB_7 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 2));
								ISlottable wearSB_7 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 3));
								ISlottable shieldSB_7 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 4));
								ISlottable mWeaponSB_7 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 5));
								ISlottable quiverSB_7 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 6));
								ISlottable packSB_7 = MakeSubSBWithItem(MakePackInstWithOrder(0, 7));
								ISlottable partsSB_7 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 8));
								ISlottable wear1SB_7 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 9));
								IList<ISlottable> list_7 = new List<ISlottable>(new ISlottable[]{
									null,
									quiverSB_7,
									bow1SB_7,
									null,
									bow2SB_7,
									mWeaponSB_7,
									null,
									bowSB_7,
									null,
									wearSB_7,
									null,
									shieldSB_7,
									packSB_7,
									null,
									null,
									partsSB_7
								});
								IList<ISlottable> expected_7 = new List<ISlottable>(new ISlottable[]{
									bowSB_7,
									bow1SB_7,
									wearSB_7,
									wear1SB_7,
									shieldSB_7,
									mWeaponSB_7,
									quiverSB_7,
									packSB_7,
									partsSB_7
								});
								SG1ThisSortResize_isPoolFalse_isAutoSorTrue_isExpandableTrue = new object[]{
									false, 
									true, 
									true, 
									idSorter, 
									true, 
									wear1SB_7, 
									bow2SB_7, 
									list_7, 
									expected_7, 
									3
								};
								yield return SG1ThisSortResize_isPoolFalse_isAutoSorTrue_isExpandableTrue;
							object[] SG1NotThisSortResize_isPoolFalse_isAutoSorTrue_isExpandableTrue;
								ISlottable bowSB_8 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 0));
								ISlottable bow1SB_8 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable bow2SB_8 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 2));
								ISlottable wearSB_8 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 3));
								ISlottable shieldSB_8 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 4));
								ISlottable mWeaponSB_8 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 5));
								ISlottable quiverSB_8 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 6));
								ISlottable packSB_8 = MakeSubSBWithItem(MakePackInstWithOrder(0, 7));
								ISlottable partsSB_8 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 8));
								ISlottable wear1SB_8 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 9));
								IList<ISlottable> list_8 = new List<ISlottable>(new ISlottable[]{
									null,
									quiverSB_8,
									bow1SB_8,
									null,
									bow2SB_8,
									mWeaponSB_8,
									null,
									bowSB_8,
									null,
									wearSB_8,
									null,
									shieldSB_8,
									packSB_8,
									null,
									null,
									partsSB_8
								});
								IList<ISlottable> expected_8 = new List<ISlottable>(new ISlottable[]{
									bowSB_8,
									bow1SB_8,
									wearSB_8,
									wear1SB_8,
									shieldSB_8,
									mWeaponSB_8,
									quiverSB_8,
									packSB_8,
									partsSB_8
								});
								SG1NotThisSortResize_isPoolFalse_isAutoSorTrue_isExpandableTrue = new object[]{
									false, 
									true, 
									true, 
									idSorter, 
									false, 
									wear1SB_8, 
									bow2SB_8, 
									list_8, 
									expected_8, 
									3
								};
								yield return SG1NotThisSortResize_isPoolFalse_isAutoSorTrue_isExpandableTrue;
						}
					}
				[TestCaseSource(typeof(TryChangeStackableQuantity_MatchAndIsStackableCases))]
				public void TryChangeStackableQuantity_Various_ReturnsAccordingly(List<ISlottable> list, InventoryItemInstance delta, bool added, bool expected){
					SlotGroup sg = MakeSG();

					List<ISlottable> actualList = new List<ISlottable>(list);

					bool actual = sg.TryChangeStackableQuantity(actualList, delta, added);

					Assert.That(actual, Is.EqualTo(expected));
					}
					class TryChangeStackableQuantity_MatchAndIsStackableCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] match_notStackable_add_F;
								ISlottable bowSB_0 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								ISlottable wearSB_0 = MakeSubSBWithItem(MakeWearInstance(0));
								List<ISlottable> list_0 = new List<ISlottable>(new ISlottable[]{
									bowSB_0, partsSB_0, wearSB_0
								});
								BowInstance bow_d_0 = MakeBowInstance(0);
								match_notStackable_add_F = new object[]{list_0, bow_d_0, true, false};
								yield return match_notStackable_add_F;
							object[] match_notStackable_remove_F;
								ISlottable bowSB_1 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable partsSB_1 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								ISlottable wearSB_1 = MakeSubSBWithItem(MakeWearInstance(0));
								List<ISlottable> list_1 = new List<ISlottable>(new ISlottable[]{
									bowSB_1, partsSB_1, wearSB_1
								});
								BowInstance bow_d_1 = MakeBowInstance(0);
								match_notStackable_remove_F = new object[]{list_1, bow_d_1, true, false};
								yield return match_notStackable_remove_F;
							object[] match_stackable_add_T;
								ISlottable bowSB_2 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable partsSB_2 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								ISlottable wearSB_2 = MakeSubSBWithItem(MakeWearInstance(0));
								List<ISlottable> list_2 = new List<ISlottable>(new ISlottable[]{
									bowSB_2, partsSB_2, wearSB_2
								});
								PartsInstance parts_d_2 = MakePartsInstance(0, 3);
								match_stackable_add_T = new object[]{list_2, parts_d_2, true, true};
								yield return match_stackable_add_T;
							object[] match_stackable_removed_T;
								ISlottable bowSB_3 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable partsSB_3 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								ISlottable wearSB_3 = MakeSubSBWithItem(MakeWearInstance(0));
								List<ISlottable> list_3 = new List<ISlottable>(new ISlottable[]{
									bowSB_3, partsSB_3, wearSB_3
								});
								PartsInstance parts_d_3 = MakePartsInstance(0, 3);
								match_stackable_removed_T = new object[]{list_3, parts_d_3, false, true};
								yield return match_stackable_removed_T;
							object[] nonmatch_stackable_add_F;
								ISlottable bowSB_4 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable partsSB_4 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								ISlottable wearSB_4 = MakeSubSBWithItem(MakeWearInstance(0));
								List<ISlottable> list_4 = new List<ISlottable>(new ISlottable[]{
									bowSB_4, partsSB_4, wearSB_4
								});
								PartsInstance parts1_d_4 = MakePartsInstance(1, 3);
								nonmatch_stackable_add_F = new object[]{list_4, parts1_d_4, true, false};
								yield return nonmatch_stackable_add_F;
							object[] nonmatch_stackable_remove_F;
								ISlottable bowSB_5 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable partsSB_5 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								ISlottable wearSB_5 = MakeSubSBWithItem(MakeWearInstance(0));
								List<ISlottable> list_5 = new List<ISlottable>(new ISlottable[]{
									bowSB_5, partsSB_5, wearSB_5
								});
								PartsInstance parts1_d_5 = MakePartsInstance(1, 3);
								nonmatch_stackable_remove_F = new object[]{list_5, parts1_d_5, false, false};
								yield return nonmatch_stackable_remove_F;
						}
					}
				[TestCaseSource(typeof(TryChangeStackableQuantity_AddedCases))]
				public void TryChangeStackableQuantity_Added_IncreaseQuantity(List<ISlottable> sbs, InventoryItemInstance addedItem, int indexAtAdded, int expectedQua){
					SlotGroup sg = MakeSG();

					List<ISlottable> actualTarget = new List<ISlottable>(sbs);

					sg.TryChangeStackableQuantity(actualTarget, addedItem, true);

					ISlottable actualAdded = actualTarget[indexAtAdded];
					actualAdded.Received().SetQuantity(expectedQua);
					}
					class TryChangeStackableQuantity_AddedCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case0;
								ISlottable bowSB_0 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstance(0, 1));
									partsSB_0.quantity.Returns(1);
								ISlottable parts1SB_0 = MakeSubSBWithItem(MakePartsInstance(1, 5));
									parts1SB_0.quantity.Returns(5);
								ISlottable wearSB_0 = MakeSubSBWithItem(MakeWearInstance(0));
								List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
									bowSB_0, partsSB_0, wearSB_0, parts1SB_0
								});
								PartsInstance addedParts_0 = MakePartsInstance(0, 3);
								case0 = new object[]{sbs_0, addedParts_0, 1, 4};
								yield return case0;
							object[] case1;
								ISlottable bowSB_1 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable partsSB_1 = MakeSubSBWithItem(MakePartsInstance(0, 1));
									partsSB_1.quantity.Returns(1);
								ISlottable parts1SB_1 = MakeSubSBWithItem(MakePartsInstance(1, 5));
									parts1SB_1.quantity.Returns(5);
								ISlottable wearSB_1 = MakeSubSBWithItem(MakeWearInstance(0));
								List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
									bowSB_1, partsSB_1, wearSB_1, parts1SB_1
								});
								PartsInstance addedParts_1 = MakePartsInstance(1, 1);
								case1 = new object[]{sbs_1, addedParts_1, 3, 6};
								yield return case1;
						}
					}
				[TestCaseSource(typeof(TryChangeStackableQuantity_DecreaseCases))]
				public void TryChangeStackableQuantity_Decrease_DecreasesQuantity(List<ISlottable> sbs, InventoryItemInstance decreasedItem, int indexAtDecreased, int expectedQua){
					SlotGroup sg = MakeSG();
					List<ISlottable> actualTarget = new List<ISlottable>(sbs);

					sg.TryChangeStackableQuantity(actualTarget, decreasedItem, false);

					ISlottable actualDecreased = actualTarget[indexAtDecreased];
					actualDecreased.Received().SetQuantity(expectedQua);
					}
					class TryChangeStackableQuantity_DecreaseCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case0;
								ISlottable bowSB_0 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstance(0, 7));
									partsSB_0.quantity.Returns(7);
								ISlottable parts1SB_0 = MakeSubSBWithItem(MakePartsInstance(1, 5));
									parts1SB_0.quantity.Returns(5);
								ISlottable wearSB_0 = MakeSubSBWithItem(MakeWearInstance(0));
								List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
									bowSB_0, partsSB_0, wearSB_0, parts1SB_0
								});
								PartsInstance removedParts_0 = MakePartsInstance(0, 3);
									removedParts_0.quantity = 3;
								case0 = new object[]{sbs_0, removedParts_0, 1, 4};
								yield return case0;
							object[] case1;
								ISlottable bowSB_1 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable partsSB_1 = MakeSubSBWithItem(MakePartsInstance(0, 7));
									partsSB_1.quantity.Returns(7);
								ISlottable parts1SB_1 = MakeSubSBWithItem(MakePartsInstance(1, 5));
									parts1SB_1.quantity.Returns(5);
								ISlottable wearSB_1 = MakeSubSBWithItem(MakeWearInstance(0));
								List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
									bowSB_1, partsSB_1, wearSB_1, parts1SB_1
								});
								PartsInstance removedParts_1 = MakePartsInstance(1, 3);
									removedParts_1.quantity = 1;
								case1 = new object[]{sbs_1, removedParts_1, 3, 4};
								yield return case1;
						}
					}
				[TestCaseSource(typeof(TryChangeStackableQuantity_DecreasedDownToZeroCases))]
				public void TryChangeStackableQuantity_DecreasedDownToZero_NullifiesAtTheIndexAndCallsSBDestory(List<ISlottable> sbs, InventoryItemInstance removedItem, int indexAtNullified){
					SlotGroup sg = MakeSG();
					List<ISlottable> actualTarget = new List<ISlottable>(sbs);
					ISlottable sbCache = actualTarget[indexAtNullified];

					sg.TryChangeStackableQuantity(actualTarget, removedItem, false);

					Assert.That(actualTarget[indexAtNullified], Is.Null);
					sbCache.Received().Destroy();
					}
					class TryChangeStackableQuantity_DecreasedDownToZeroCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case0;
								ISlottable bowSB_0 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstance(0, 1));
									partsSB_0.quantity.Returns(1);
								ISlottable parts1SB_0 = MakeSubSBWithItem(MakePartsInstance(1, 5));
									parts1SB_0.quantity.Returns(5);
								ISlottable wearSB_0 = MakeSubSBWithItem(MakeWearInstance(0));
								List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
									bowSB_0, partsSB_0, wearSB_0, parts1SB_0
								});
								PartsInstance removedParts_0 = MakePartsInstance(0, 1);
								case0 = new object[]{sbs_0, removedParts_0, 1};
								yield return case0;
							object[] case1;
								ISlottable bowSB_1 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable partsSB_1 = MakeSubSBWithItem(MakePartsInstance(0, 1));
									partsSB_1.quantity.Returns(1);
								ISlottable parts1SB_1 = MakeSubSBWithItem(MakePartsInstance(1, 5));
									parts1SB_1.quantity.Returns(5);
								ISlottable wearSB_1 = MakeSubSBWithItem(MakeWearInstance(0));
								List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
									bowSB_1, partsSB_1, wearSB_1, parts1SB_1
								});
								PartsInstance removedParts_1 = MakePartsInstance(1, 5);
								case1 = new object[]{sbs_1, removedParts_1, 3};
								yield return case1;
						}
					}
				[TestCaseSource(typeof(AddAndUpdateSBs_VariousCases))]
				public void AddAndUpdateSBs_Various_Various(
					bool isAutoSort, 
					bool isExpandable, 
					SGSorter sorter,  
					List<ISlottable> sbs, 
					List<InventoryItemInstance> added, 
					List<ISlottable> expSBs, 
					List<ISlottable> expNewSBs, 
					Dictionary<InventoryItemInstance, int> sbsAddedIDDict, 
					Dictionary<InventoryItemInstance, int> newSBsAddedIDDict, 
					Dictionary<InventoryItemInstance, int> quantDict)
					{
						List<ISlottable> expectedSBs = new List<ISlottable>(expSBs);
						List<ISlottable> expectedNewSBs = new List<ISlottable>(expNewSBs);
						SlotGroup sg = MakeSG();
						sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), sorter, isExpandable?0: 20);
							ISlotSystemManager ssm = MakeSubSSM();
								ssm.moved.Returns(added);
								ssm.FindParent(Arg.Any<ISlottable>()).Returns(sg);
							sg.SetSSM(ssm);
							sg.SetSBs(sbs);
							sg.ToggleAutoSort(isAutoSort);
						
						sg.AddAndUpdateSBs();
						
						foreach(ISlottable sb in sg){
							if(sb != null){
								int id;
								if(sbsAddedIDDict.TryGetValue(sb.itemInst, out id))
									expectedSBs[id] = sb;
							}
						}
						bool equality = sg.toList.MemberEquals(expectedSBs);
						Assert.That(equality, Is.True);
						foreach(ISlottable sb in sg){
							if(sb != null){
								int qua;
								if(quantDict.TryGetValue(sb.itemInst, out qua)){
									sb.Received().SetQuantity(qua);
								}
							}
						}
						foreach(ISlottable sb in sg.newSBs){
							if(sb != null){
								int id;
								if(newSBsAddedIDDict.TryGetValue(sb.itemInst, out id)){
									expectedNewSBs[id] = sg.newSBs[sg.newSBs.IndexOf(sb)];
								}
							}
						}
						bool newSBsEquality = sg.newSBs.MemberEquals(expectedNewSBs);
						Assert.That(newSBsEquality, Is.True);
					}
					class AddAndUpdateSBs_VariousCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] addedNoSort_isAutoSortFalse;
								ISlottable bowSB_0 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable wearSB_0 = MakeSubSBWithItem(MakeWearInstance(0));
								ISlottable shieldSB_0 = MakeSubSBWithItem(MakeShieldInstance(0));
								ISlottable mWeaponSB_0 = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
								ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstance(0, 1));
									partsSB_0.quantity.Returns(1);
								ISlottable parts1SB_0 = MakeSubSBWithItem(MakePartsInstance(1, 2));
									parts1SB_0.quantity.Returns(2);
								ISlottable bow1SB_a_0 = MakeSubSBWithItem(MakeBowInstance(1));
								ISlottable quiverSB_a_0 = MakeSubSBWithItem(MakeQuiverInstance(1));
								ISlottable partsSB_a_0 = MakeSubSBWithItem(MakePartsInstance(0, 6));
									partsSB_a_0.quantity.Returns(6);
								ISlottable parts1SB_a_0 = MakeSubSBWithItem(MakePartsInstance(1, 3));
									parts1SB_a_0.quantity.Returns(3);
								List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
									parts1SB_0,
									wearSB_0,
									null,
									mWeaponSB_0,
									null,
									partsSB_0,
									null,
									shieldSB_0,
									bowSB_0,
									null,
									null
								});
								SGSorter idSorter_0 = new SGItemIDSorter();
								List<InventoryItemInstance> added_0 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									bow1SB_a_0.itemInst,
									quiverSB_a_0.itemInst,
									partsSB_a_0.itemInst,
									parts1SB_a_0.itemInst 
								});
								List<ISlottable> xSBs_0 = new List<ISlottable>(new ISlottable[]{
									parts1SB_0,
									wearSB_0,
									null,
									mWeaponSB_0,
									null,
									partsSB_0,
									null,
									shieldSB_0,
									bowSB_0,
									null,
									null,
									bow1SB_a_0,
									quiverSB_a_0,
								});
								List<ISlottable> xNewSBs_0 = new List<ISlottable>(new ISlottable[]{
									parts1SB_0,
									wearSB_0,
									bow1SB_a_0,
									mWeaponSB_0,
									quiverSB_a_0,
									partsSB_0,
									null,
									shieldSB_0,
									bowSB_0,
									null,
									null,
								});
								Dictionary<InventoryItemInstance, int> sbsIDs_0 = new Dictionary<InventoryItemInstance, int>();
									sbsIDs_0.Add(bow1SB_a_0.itemInst, 11);
									sbsIDs_0.Add(quiverSB_a_0.itemInst, 12);
									sbsIDs_0.Add(partsSB_a_0.itemInst, 5);
									sbsIDs_0.Add(parts1SB_a_0.itemInst, 0);
								Dictionary<InventoryItemInstance, int> newSBsIDs_0 = new Dictionary<InventoryItemInstance, int>();
									newSBsIDs_0.Add(bow1SB_a_0.itemInst, 2);
									newSBsIDs_0.Add(quiverSB_a_0.itemInst, 4);
								Dictionary<InventoryItemInstance, int> quants_0 = new Dictionary<InventoryItemInstance, int>();
									quants_0.Add(partsSB_a_0.itemInst, 7);
									quants_0.Add(parts1SB_a_0.itemInst, 5);
								addedNoSort_isAutoSortFalse = new object[]{
									false, 
									false, 
									idSorter_0, 
									sbs_0, 
									added_0, 
									xSBs_0, 
									xNewSBs_0, 
									sbsIDs_0, 
									newSBsIDs_0, 
									quants_0
								};
								yield return addedNoSort_isAutoSortFalse;
							object[] addedSortNoResize_isAutoSortTrue_isExpandableFalse;
								ISlottable bowSB_1 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable wearSB_1 = MakeSubSBWithItem(MakeWearInstance(0));
								ISlottable shieldSB_1 = MakeSubSBWithItem(MakeShieldInstance(0));
								ISlottable mWeaponSB_1 = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
								ISlottable partsSB_1 = MakeSubSBWithItem(MakePartsInstance(0, 1));
									partsSB_1.quantity.Returns(1);
								ISlottable parts1SB_1 = MakeSubSBWithItem(MakePartsInstance(1, 2));
									parts1SB_1.quantity.Returns(2);
								ISlottable bow1SB_a_1 = MakeSubSBWithItem(MakeBowInstance(1));
								ISlottable quiverSB_a_1 = MakeSubSBWithItem(MakeQuiverInstance(1));
								ISlottable partsSB_a_1 = MakeSubSBWithItem(MakePartsInstance(0, 6));
									partsSB_a_1.quantity.Returns(6);
								ISlottable parts1SB_a_1 = MakeSubSBWithItem(MakePartsInstance(1, 3));
									parts1SB_a_1.quantity.Returns(3);
								List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
									parts1SB_1,
									wearSB_1,
									null,
									mWeaponSB_1,
									null,
									partsSB_1,
									null,
									shieldSB_1,
									bowSB_1,
									null,
									null
								});
								SGSorter idSorter_1 = new SGItemIDSorter();
								List<InventoryItemInstance> added_1 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									bow1SB_a_1.itemInst,
									quiverSB_a_1.itemInst,
									partsSB_a_1.itemInst,
									parts1SB_a_1.itemInst 
								});
								List<ISlottable> xSBs_1 = new List<ISlottable>(new ISlottable[]{
									parts1SB_1,
									wearSB_1,
									null,
									mWeaponSB_1,
									null,
									partsSB_1,
									null,
									shieldSB_1,
									bowSB_1,
									null,
									null,
									bow1SB_a_1,
									quiverSB_a_1,
								});
								Dictionary<InventoryItemInstance, int> sbsIDs_1 = new Dictionary<InventoryItemInstance, int>();
									sbsIDs_1.Add(bow1SB_a_1.itemInst, 11);
									sbsIDs_1.Add(quiverSB_a_1.itemInst, 12);
									sbsIDs_1.Add(partsSB_a_1.itemInst, 5);
									sbsIDs_1.Add(parts1SB_a_1.itemInst, 0);
								Dictionary<InventoryItemInstance, int> quants_1 = new Dictionary<InventoryItemInstance, int>();
									quants_1.Add(partsSB_a_1.itemInst, 7);
									quants_1.Add(parts1SB_a_1.itemInst, 5);
								List<ISlottable> xNewSBs_1 = new List<ISlottable>(new ISlottable[]{
									bowSB_1,
									bow1SB_a_1,
									wearSB_1,
									shieldSB_1,
									mWeaponSB_1,
									quiverSB_a_1,
									partsSB_1,
									parts1SB_1,
									null,
									null,
									null
								});
								Dictionary<InventoryItemInstance, int> newSBsIDs_1 = new Dictionary<InventoryItemInstance, int>();
									newSBsIDs_1.Add(bow1SB_a_1.itemInst, 1);
									newSBsIDs_1.Add(quiverSB_a_1.itemInst, 5);
								addedSortNoResize_isAutoSortTrue_isExpandableFalse = new object[]{
									true, 
									false, 
									idSorter_1, 
									sbs_1, 
									added_1, 
									xSBs_1, 
									xNewSBs_1, 
									sbsIDs_1, 
									newSBsIDs_1, 
									quants_1
								};
								yield return addedSortNoResize_isAutoSortTrue_isExpandableFalse;
							object[] addedSortResize_isAutoSortTrue_isExpandableTrue;
								ISlottable bowSB_2 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable wearSB_2 = MakeSubSBWithItem(MakeWearInstance(0));
								ISlottable shieldSB_2 = MakeSubSBWithItem(MakeShieldInstance(0));
								ISlottable mWeaponSB_2 = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
								ISlottable partsSB_2 = MakeSubSBWithItem(MakePartsInstance(0, 1));
									partsSB_2.quantity.Returns(1);
								ISlottable parts1SB_2 = MakeSubSBWithItem(MakePartsInstance(1, 2));
									parts1SB_2.quantity.Returns(2);
								ISlottable bow1SB_a_2 = MakeSubSBWithItem(MakeBowInstance(1));
								ISlottable quiverSB_a_2 = MakeSubSBWithItem(MakeQuiverInstance(1));
								ISlottable partsSB_a_2 = MakeSubSBWithItem(MakePartsInstance(0, 6));
									partsSB_a_2.quantity.Returns(6);
								ISlottable parts1SB_a_2 = MakeSubSBWithItem(MakePartsInstance(1, 3));
									parts1SB_a_2.quantity.Returns(3);
								List<ISlottable> sbs_2 = new List<ISlottable>(new ISlottable[]{
									parts1SB_2,
									wearSB_2,
									null,
									mWeaponSB_2,
									null,
									partsSB_2,
									null,
									shieldSB_2,
									bowSB_2,
									null,
									null
								});
								SGSorter idSorter_2 = new SGItemIDSorter();
								List<InventoryItemInstance> added_2 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									bow1SB_a_2.itemInst,
									quiverSB_a_2.itemInst,
									partsSB_a_2.itemInst,
									parts1SB_a_2.itemInst 
								});
								List<ISlottable> xSBs_2 = new List<ISlottable>(new ISlottable[]{
									parts1SB_2,
									wearSB_2,
									null,
									mWeaponSB_2,
									null,
									partsSB_2,
									null,
									shieldSB_2,
									bowSB_2,
									null,
									null,
									bow1SB_a_2,
									quiverSB_a_2,
								});
								Dictionary<InventoryItemInstance, int> sbsIDs_2 = new Dictionary<InventoryItemInstance, int>();
									sbsIDs_2.Add(bow1SB_a_2.itemInst, 11);
									sbsIDs_2.Add(quiverSB_a_2.itemInst, 12);
									sbsIDs_2.Add(partsSB_a_2.itemInst, 5);
									sbsIDs_2.Add(parts1SB_a_2.itemInst, 0);
								Dictionary<InventoryItemInstance, int> quants_2 = new Dictionary<InventoryItemInstance, int>();
									quants_2.Add(partsSB_a_2.itemInst, 7);
									quants_2.Add(parts1SB_a_2.itemInst, 5);
								Dictionary<InventoryItemInstance, int> newSBsIDs_2 = new Dictionary<InventoryItemInstance, int>();
									newSBsIDs_2.Add(bow1SB_a_2.itemInst, 1);
									newSBsIDs_2.Add(quiverSB_a_2.itemInst, 5);
								List<ISlottable> xNewSBs_2 = new List<ISlottable>(new ISlottable[]{
									bowSB_2,
									bow1SB_a_2,
									wearSB_2,
									shieldSB_2,
									mWeaponSB_2,
									quiverSB_a_2,
									partsSB_2,
									parts1SB_2
								});
								addedSortResize_isAutoSortTrue_isExpandableTrue = new object[]{
									true, 
									true, 
									idSorter_2, 
									sbs_2, 
									added_2, 
									xSBs_2, 
									xNewSBs_2, 
									sbsIDs_2, 
									newSBsIDs_2, 
									quants_2
								};
								yield return addedSortResize_isAutoSortTrue_isExpandableTrue;
						}
					}
				[TestCaseSource(typeof(RemoveAndUpdateSGsCases))]
				public void RemoveAndUpdateSGs_Various_DoesNotChangeSBsAndSetsNewSBsAccordingly(
					bool isAutoSort, 
					bool isExpandable, 
					SGSorter sorter, 
					List<ISlottable> sbs, 
					List<InventoryItemInstance> removed, 
					List<ISlottable> xNewSBs, 
					Dictionary<InventoryItemInstance, int> removedQuantDict)
					{
						SlotGroup sg = MakeSG();
							sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), sorter, isExpandable?0: 20);
								ISlotSystemManager ssm = MakeSubSSM();
									ssm.moved.Returns(removed);
								sg.SetSSM(ssm);
								sg.SetSBs(sbs);
								sg.ToggleAutoSort(isAutoSort);

							sg.RemoveAndUpdateSBs();

							bool sbsEquality = sg.toList.MemberEquals(sbs);
							Assert.That(sbsEquality, Is.True);
							bool newSBsEquality = sg.newSBs.MemberEquals(xNewSBs);
							Assert.That(newSBsEquality, Is.True);
							foreach(ISlottable sb in sbs){
								if(sb != null){
									int quantity;
									if(removedQuantDict.TryGetValue(sb.itemInst, out quantity)){
										sb.Received().SetQuantity(quantity);
									}
								}
							}	
					}
					class RemoveAndUpdateSGsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] noSort_isAutoSortFalse;
								ISlottable bowSB_0 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable bow1SB_0 = MakeSubSBWithItem(MakeBowInstance(1));
								ISlottable wearSB_0 = MakeSubSBWithItem(MakeWearInstance(0));
								ISlottable shieldSB_0 = MakeSubSBWithItem(MakeShieldInstance(0));
								ISlottable mWeaponSB_0 = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
								ISlottable quiverSB_0 = MakeSubSBWithItem(MakeQuiverInstance(0));
								ISlottable packSB_0 = MakeSubSBWithItem(MakePackInstance(0));
								ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstance(0, 5));
									partsSB_0.quantity.Returns(5);
								ISlottable parts1SB_0 = MakeSubSBWithItem(MakePartsInstance(1, 4));
									parts1SB_0.quantity.Returns(4);
								ISlottable partsSB_r_0 = MakeSubSBWithItem(MakePartsInstance(0, 3));
									partsSB_r_0.quantity.Returns(3);
								ISlottable parts1SB_r_0 = MakeSubSBWithItem(MakePartsInstance(1, 4));
									parts1SB_r_0.quantity.Returns(4);
								List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
									shieldSB_0,
									null,
									mWeaponSB_0,
									null,
									bow1SB_0,
									packSB_0,
									null,
									parts1SB_0,
									quiverSB_0,
									partsSB_0,
									null,
									bowSB_0,
									null,
									wearSB_0
								});
								List<InventoryItemInstance> removed_0 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									quiverSB_0.itemInst,
									bowSB_0.itemInst,
									partsSB_r_0.itemInst,
									parts1SB_r_0.itemInst
								});
								List<ISlottable> xNewSBs_0 = new List<ISlottable>(new ISlottable[]{
									shieldSB_0,
									null,
									mWeaponSB_0,
									null,
									bow1SB_0,
									packSB_0,
									null,
									null,
									null,
									partsSB_0,
									null,
									null,
									null,
									wearSB_0
								});
								Dictionary<InventoryItemInstance, int> removedQuantDict_0 = new Dictionary<InventoryItemInstance, int>();
									removedQuantDict_0.Add(partsSB_r_0.itemInst, 2);
									noSort_isAutoSortFalse = new object[]{
										false, 
										false, 
										new SGItemIDSorter(), 
										sbs_0, 
										removed_0, 
										xNewSBs_0, 
										removedQuantDict_0 
									};
								yield return noSort_isAutoSortFalse;
							object[] sortNoResize_isAutoSortTrue_isExpandableFalse;
								ISlottable bowSB_1 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable bow1SB_1 = MakeSubSBWithItem(MakeBowInstance(1));
								ISlottable wearSB_1 = MakeSubSBWithItem(MakeWearInstance(0));
								ISlottable shieldSB_1 = MakeSubSBWithItem(MakeShieldInstance(0));
								ISlottable mWeaponSB_1 = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
								ISlottable quiverSB_1 = MakeSubSBWithItem(MakeQuiverInstance(0));
								ISlottable packSB_1 = MakeSubSBWithItem(MakePackInstance(0));
								ISlottable partsSB_1 = MakeSubSBWithItem(MakePartsInstance(0, 5));
									partsSB_1.quantity.Returns(5);
								ISlottable parts1SB_1 = MakeSubSBWithItem(MakePartsInstance(1, 4));
									parts1SB_1.quantity.Returns(4);
								ISlottable partsSB_r_1 = MakeSubSBWithItem(MakePartsInstance(0, 3));
									partsSB_r_1.quantity.Returns(3);
								ISlottable parts1SB_r_1 = MakeSubSBWithItem(MakePartsInstance(1, 4));
									parts1SB_r_1.quantity.Returns(4);
								List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
									shieldSB_1,
									null,
									mWeaponSB_1,
									null,
									bow1SB_1,
									packSB_1,
									null,
									parts1SB_1,
									quiverSB_1,
									partsSB_1,
									null,
									bowSB_1,
									null,
									wearSB_1
								});
								List<InventoryItemInstance> removed_1 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									quiverSB_1.itemInst,
									bowSB_1.itemInst,
									partsSB_r_1.itemInst,
									parts1SB_r_1.itemInst
								});
								List<ISlottable> xNewSBs_1 = new List<ISlottable>(new ISlottable[]{
									bow1SB_1,
									wearSB_1,
									shieldSB_1,
									mWeaponSB_1,
									packSB_1,
									partsSB_1,
									null,
									null,
									null,
									null,
									null,
									null,
									null,
									null
								});
								Dictionary<InventoryItemInstance, int> removedQuantDict_1 = new Dictionary<InventoryItemInstance, int>();
									removedQuantDict_1.Add(partsSB_r_1.itemInst, 2);
									sortNoResize_isAutoSortTrue_isExpandableFalse = new object[]{
										true, 
										false, 
										new SGItemIDSorter(), 
										sbs_1, 
										removed_1, 
										xNewSBs_1, 
										removedQuantDict_1
									};
								yield return sortNoResize_isAutoSortTrue_isExpandableFalse;
							object[] sortResize_isAutoSortTrue_isExpandableTrue;
								ISlottable bowSB_2 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable bow1SB_2 = MakeSubSBWithItem(MakeBowInstance(1));
								ISlottable wearSB_2 = MakeSubSBWithItem(MakeWearInstance(0));
								ISlottable shieldSB_2 = MakeSubSBWithItem(MakeShieldInstance(0));
								ISlottable mWeaponSB_2 = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
								ISlottable quiverSB_2 = MakeSubSBWithItem(MakeQuiverInstance(0));
								ISlottable packSB_2 = MakeSubSBWithItem(MakePackInstance(0));
								ISlottable partsSB_2 = MakeSubSBWithItem(MakePartsInstance(0, 5));
									partsSB_2.quantity.Returns(5);
								ISlottable parts1SB_2 = MakeSubSBWithItem(MakePartsInstance(1, 4));
									parts1SB_2.quantity.Returns(4);
								ISlottable partsSB_r_2 = MakeSubSBWithItem(MakePartsInstance(0, 3));
									partsSB_r_2.quantity.Returns(3);
								ISlottable parts1SB_r_2 = MakeSubSBWithItem(MakePartsInstance(1, 4));
									parts1SB_r_2.quantity.Returns(4);
								List<ISlottable> sbs_2 = new List<ISlottable>(new ISlottable[]{
									shieldSB_2,
									null,
									mWeaponSB_2,
									null,
									bow1SB_2,
									packSB_2,
									null,
									parts1SB_2,
									quiverSB_2,
									partsSB_2,
									null,
									bowSB_2,
									null,
									wearSB_2
								});
								List<InventoryItemInstance> removed_2 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									quiverSB_2.itemInst,
									bowSB_2.itemInst,
									partsSB_r_2.itemInst,
									parts1SB_r_2.itemInst
								});
								List<ISlottable> xNewSBs_2 = new List<ISlottable>(new ISlottable[]{
									bow1SB_2,
									wearSB_2,
									shieldSB_2,
									mWeaponSB_2,
									packSB_2,
									partsSB_2
								});
								Dictionary<InventoryItemInstance, int> removedQuantDict_2 = new Dictionary<InventoryItemInstance, int>();
									removedQuantDict_2.Add(partsSB_r_2.itemInst, 2);
									sortResize_isAutoSortTrue_isExpandableTrue = new object[]{
										true, 
										true, 
										new SGItemIDSorter(), 
										sbs_2, 
										removed_2, 
										xNewSBs_2, 
										removedQuantDict_2
									};
								yield return sortResize_isAutoSortTrue_isExpandableTrue;
						}
					}
				[TestCase(1)]
				[TestCase(10)]
				public void InitSlots_InitSlotsCountNonZero_SetsSlotsByInitSlotsCount(int initSlotsCount){
					SlotGroup sg = MakeSG();
						sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), new SGItemIDSorter() , initSlotsCount);
					
					sg.InitSlots(new List<SlottableItem>());

					int actual = sg.slots.Count;

					Assert.That(actual, Is.EqualTo(initSlotsCount));
					foreach(Slot slot in sg.slots)
						Assert.That(slot.sb, Is.Null);
					}
				[TestCase(4)]
				[TestCase(40)]
				public void InitSlots_InitSlotsCountZero_SetsSlotsByItemsCount(int itemsCount){
					SlotGroup sg = MakeSG();
						sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), new SGItemIDSorter() , 0);
					
					sg.InitSlots(new List<SlottableItem>(new SlottableItem[itemsCount]));

					int actual = sg.slots.Count;

					Assert.That(actual, Is.EqualTo(itemsCount));
					foreach(Slot slot in sg.slots)
						Assert.That(slot.sb, Is.Null);
					}
				[TestCaseSource(typeof(InitSBs_SlotsNotEnoughCases))]
				public void InitSBs_SlotsNotEnough_RemoveNonFittableItems(List<Slot> slots, List<SlottableItem> items, List<SlottableItem> expected){
					SlotGroup sg = MakeSG();
						sg.SetSlots(slots);
					
					sg.InitSBs(items);

					bool equality = items.MemberEquals(expected);
					Assert.That(equality, Is.True);					
					}
					class InitSBs_SlotsNotEnoughCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							List<Slot> slots = CreateSlots(4);
							List<SlottableItem> items;
								BowInstance bowA = MakeBowInstance(0);
								BowInstance bowA_1 = MakeBowInstance(0);
								BowInstance bowA_2 = MakeBowInstance(0);
								WearInstance wear = MakeWearInstance(0);
								ShieldInstance shield = MakeShieldInstance(0);
								MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
								items = new List<SlottableItem>(new SlottableItem[]{bowA, bowA_1, bowA_2, wear, shield, mWeapon});
							List<SlottableItem> expected = new List<SlottableItem>(new SlottableItem[]{bowA, bowA_1, bowA_2, wear});
							yield return new object[]{slots, items, expected};

						}
					}
				[TestCaseSource(typeof(InitSBs_WhenCalledCases))]
				public void InitSBs_WhenCalled_CreatesAndSetsSBsInSlots(ISlotSystemManager ssm ,List<Slot> slots, List<SlottableItem> items){
					SlotGroup sg = MakeSG();
						sg.SetSSM(ssm);
						sg.SetSlots(slots);
					
					sg.InitSBs(items);

					for(int i = 0; i< slots.Count; i++){
						ISlottable sb = slots[i].sb;
						Assert.That(sb.ssm, Is.SameAs(ssm));
						Assert.That(sb.itemInst, Is.SameAs(items[i]));
						Assert.That(sg.isSelStateInit, Is.True);
						Assert.That(sb.isEquipped, Is.False);
						Assert.That(sb.isUnequipped, Is.False);
					}
					}
					class InitSBs_WhenCalledCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlotSystemManager ssm = MakeSubSSM();
							List<Slot> slots = CreateSlots(4);
							List<SlottableItem> items;
								BowInstance bowA = MakeBowInstance(0);
								BowInstance bowB = MakeBowInstance(1);
								BowInstance bowC = MakeBowInstance(2);
								BowInstance bowD = MakeBowInstance(3);
								items = new List<SlottableItem>(new SlottableItem[]{bowA, bowB, bowC, bowD});
							yield return new object[]{
								ssm, slots, items
							};
						}
					}
			/*	helper */
				static ISlottable MakeSubSBWithItemAndSG(InventoryItemInstance item, SlotGroup sg){
					ISlottable sb = MakeSubSB();
						sb.itemInst.Returns(item);
						sb.sg.Returns(sg);
					return sb;
				}
				static ISlottable MakeSubSBWithItem(InventoryItemInstance item){
					ISlottable sb = MakeSubSB();
						sb.itemInst.Returns(item);
					return sb;
				}
				static ISlottable MakeSBWithItem(InventoryItemInstance item){
					ISlottable sb = MakeSB();
						sb.SetItem(item);
					return sb;
				}
				List<ISlottable> CreateSBs(int count){
					List<ISlottable> sbs = new List<ISlottable>();
					for(int i =0; i< count; i++){
						ISlottable sb = MakeSubSB();
						sbs.Add(sb);
					}
					return sbs;
				}
				static ISlottable MakeSBWithActProc(bool isRunning){
					ISlottable sb = MakeSubSB();
						ISSEProcess sbActProc = MakeSubSBActProc();
							sbActProc.isRunning.Returns(isRunning);
						sb.actProcess.Returns(sbActProc);
					return sb;
				}
				ISlotSystemManager MakeSSMWithPBunContaining(ISlotGroup sg){
					ISlotSystemManager stubSSM = MakeSubSSM();
						ISlotSystemBundle stubPBun = MakeSubBundle();
							stubPBun.ContainsInHierarchy(sg).Returns(true);
						stubSSM.poolBundle.Returns(stubPBun);
					return stubSSM;
				}
				ISlotSystemManager MakeSSMWithEBunContaining(ISlotGroup sg){
					ISlotSystemManager stubSSM = MakeSubSSM();
						ISlotSystemBundle stubEBun = MakeSubBundle();
							stubEBun.ContainsInHierarchy(sg).Returns(true);
						stubSSM.equipBundle.Returns(stubEBun);
					return stubSSM;
				}
				ISlotSystemManager MakeSSMWithGBunContaining(ISlotGroup sg){
					ISlotSystemManager stubSSM = MakeSubSSM();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle stubGBun = MakeSubBundle();
								stubGBun.ContainsInHierarchy(sg).Returns(true);
							gBuns = new ISlotSystemBundle[]{stubGBun};
						stubSSM.otherBundles.Returns(gBuns);
					return stubSSM;
				}
				
		}
	}
}
