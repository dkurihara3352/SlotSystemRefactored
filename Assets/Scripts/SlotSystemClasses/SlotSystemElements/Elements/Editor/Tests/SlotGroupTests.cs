using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
namespace SlotSystemTests{
	namespace ElementsTests{
		[TestFixture]
		public class SlotGroupTests: AbsSlotSystemTest{
			[Test]
			public void TransactionCoroutine_AllSBsNotRunning_CallsActProcessExpire(){
				SlotGroup sg = MakeSG();
				List<ISlottable> sbs;
					ISlottable sbA = MakeSBWithActProc(false);
					ISlottable sbB = MakeSBWithActProc(false);
					ISlottable sbC = MakeSBWithActProc(false);
					sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC});
				sg.SetSBs(sbs);
				ISSEProcess sgActProc = MakeSubSGActProc();
				sg.SetAndRunActProcess(sgActProc);

				sg.TransactionCoroutine();
				
				sgActProc.Received().Expire();
			}
			/*	Fields	*/
				[Test][Category("Fields")]
				public void isPool_SSMPBunContainsInHierarchyThis_ReturnsTrue(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager stubSSM = MakeSSMWithPBunContaining(sg);
						sg.ssm = stubSSM;

					bool actual = sg.isPool;

					Assert.That(actual, Is.True);
				}
				[Test][Category("Fields")]
				public void isSGE_SSMEBunContainsInHierarchyThis_ReturnsTrue(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager stubSSM = MakeSSMWithEBunContaining(sg);
						sg.ssm = stubSSM;

					bool actual = sg.isSGE;

					Assert.That(actual, Is.True);
				}
				[Test][Category("Fields")]
				public void isSGG_SSMGBunContainsInHierarchyThis_ReturnsTrue(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager stubSSM = MakeSSMWithGBunContaining(sg);
						sg.ssm = stubSSM;

					bool actual = sg.isSGG;

					Assert.That(actual, Is.True);
				}
				[Test][Category("Fields")]
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
				[Test][Category("Fields")]
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
				[TestCaseSource(typeof(EquippedSBsCases))][Category("Fields")]
				public void equippedSBs_WhenCalled_ReturnsAllSBsIsEquipped(List<ISlottable> sbs, List<ISlottable> expected){
					SlotGroup sg = MakeSG();
					sg.SetSBs(sbs);

					List<ISlottable> actual = sg.equippedSBs;

					Assert.That(actual, Is.EqualTo(expected));
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
				[TestCaseSource(typeof(IsAllSBsActProcDoneCases))][Category("Fields")]
				public void isAllSBsActProcDone_Various_ReturnsAccordingly(List<ISlottable> sbs, bool expected){
					SlotGroup sg = MakeSG();
					sg.SetSBs(sbs);

					bool actual = sg.isAllSBActProcDone;

					Assert.That(actual, Is.EqualTo(expected));
				}
					class IsAllSBsActProcDoneCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable vSBA = MakeSBWithActProc(false);
							ISlottable vSBB = MakeSBWithActProc(false);
							ISlottable vSBC = MakeSBWithActProc(false);
							ISlottable iSBA = MakeSBWithActProc(true);
							ISlottable iSBB = MakeSBWithActProc(true);
							ISlottable iSBC = MakeSBWithActProc(true);
							List<ISlottable> case1SBs = new List<ISlottable>(new ISlottable[]{
								vSBA, vSBB, vSBC, iSBA, iSBB, iSBC
							});
							yield return new object[]{case1SBs, false};
							List<ISlottable> case2SBs = new List<ISlottable>(new ISlottable[]{
								vSBA, vSBB, vSBC
							});
							yield return new object[]{case2SBs, true};
							List<ISlottable> case3SBs = new List<ISlottable>(new ISlottable[]{
								iSBA, iSBB, iSBC
							});
							yield return new object[]{case3SBs, false};
							List<ISlottable> case4SBs = new List<ISlottable>(new ISlottable[]{
								vSBA
							});
							yield return new object[]{case4SBs, true};
							
						}
					}
			/*	Methods	*/
				[TestCaseSource(typeof(InstantSortCases))][Category("Methods")]
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
					foreach(var slot in sg.slots){
						actual.Add(slot.sb);
					}
					Assert.That(actual, Is.EqualTo(expected));
				}
					class InstantSortCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							PartsInstance parts2 = MakePartsInstance(1, 1);
							BowInstance bow = MakeBowInstance(0);
							ShieldInstance shield = MakeShieldInstance(0);
							PartsInstance parts = MakePartsInstance(0, 2);
							BowInstance bow2 = MakeBowInstance(1);
							WearInstance wear = MakeWearInstance(0);
							QuiverInstance quiver = MakeQuiverInstance(0);
							MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
							PackInstance pack = MakePackInstance(0);
							WearInstance wear2 = MakeWearInstance(1);
								parts2.SetAcquisitionOrder(0);
								bow.SetAcquisitionOrder(1);
								shield.SetAcquisitionOrder(2);
								parts.SetAcquisitionOrder(3);
								bow2.SetAcquisitionOrder(4);
								wear.SetAcquisitionOrder(5);
								quiver.SetAcquisitionOrder(6);
								mWeapon.SetAcquisitionOrder(7);
								pack.SetAcquisitionOrder(8);
								wear2.SetAcquisitionOrder(9);
							ISlottable parts2SB = MakeSB();
								parts2SB.SetItem(parts2);
							ISlottable bowSB = MakeSB();
								bowSB.SetItem(bow);
							ISlottable shieldSB = MakeSB();
								shieldSB.SetItem(shield);
							ISlottable partsSB = MakeSB();
								partsSB.SetItem(parts);
							ISlottable bow2SB = MakeSB();
								bow2SB.SetItem(bow2);
							ISlottable wearSB = MakeSB();
								wearSB.SetItem(wear);
							ISlottable quiverSB = MakeSB();
								quiverSB.SetItem(quiver);
							ISlottable mWeaponSB = MakeSB();
								mWeaponSB.SetItem(mWeapon);
							ISlottable packSB = MakeSB();
								packSB.SetItem(pack);
							ISlottable wear2SB = MakeSB();
								wear2SB.SetItem(wear);
							
							List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{
								parts2SB, bowSB, shieldSB, partsSB, bow2SB, wearSB, quiverSB, mWeaponSB, packSB, wear2SB
							});
							List<ISlottable> itemIDCaseExp = new List<ISlottable>(new ISlottable[]{
								bowSB
								,bow2SB
								,wearSB
								,wear2SB
								,shieldSB
								,mWeaponSB
								,quiverSB
								,packSB
								,partsSB
								,parts2SB
							});
							yield return new object[]{sbs, new SGItemIDSorter(), itemIDCaseExp};
							List<ISlottable> inverseItemIDCaseExp = new List<ISlottable>(new ISlottable[]{
								parts2SB
								,partsSB
								,packSB
								,quiverSB
								,mWeaponSB
								,shieldSB
								,wear2SB
								,wearSB
								,bow2SB
								,bowSB
							});
							yield return new object[]{sbs, new SGInverseItemIDSorter(), inverseItemIDCaseExp};
							List<ISlottable> acquisitionOrderCase = new List<ISlottable>(new ISlottable[]{
								parts2SB
								,bowSB
								,shieldSB
								,partsSB
								,bow2SB
								,wearSB
								,quiverSB
								,mWeaponSB
								,packSB
								,wear2SB
							});
							yield return new object[]{sbs, new SGAcquisitionOrderSorter(), acquisitionOrderCase};
							
						}
					}
			/*	helper */
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
