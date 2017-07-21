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
		[TestFixture][Category("SG")]
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
						sg.SetSSM(stubSSM);

					bool actual = sg.isPool;

					Assert.That(actual, Is.True);
				}
				[Test][Category("Fields")]
				public void isSGE_SSMEBunContainsInHierarchyThis_ReturnsTrue(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager stubSSM = MakeSSMWithEBunContaining(sg);
						sg.SetSSM(stubSSM);

					bool actual = sg.isSGE;

					Assert.That(actual, Is.True);
				}
				[Test][Category("Fields")]
				public void isSGG_SSMGBunContainsInHierarchyThis_ReturnsTrue(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager stubSSM = MakeSSMWithGBunContaining(sg);
						sg.SetSSM(stubSSM);

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
				[TestCaseSource(typeof(AcceptsFilterCases))][Category("Methods")]
				public void AcceptsFilter_Various_ReturnsAccordingly(SGFilter filter, ISlottable sb, bool expected){
					SlotGroup sg = MakeSG();
					sg.SetFilter(filter);

					bool actual = sg.AcceptsFilter(sb);

					Assert.That(actual, Is.EqualTo(expected));
				}
					class AcceptsFilterCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable bowSB = MakeSubSB();
								bowSB.itemInst.Returns(MakeBowInstance(0));
							ISlottable wearSB = MakeSubSB();
								wearSB.itemInst.Returns(MakeWearInstance(0));
							ISlottable shieldSB = MakeSubSB();
								shieldSB.itemInst.Returns(MakeShieldInstance(0));
							ISlottable mWeaponSB = MakeSubSB();
								mWeaponSB.itemInst.Returns(MakeMeleeWeaponInstance(0));
							ISlottable quiverSB = MakeSubSB();
								quiverSB.itemInst.Returns(MakeQuiverInstance(0));
							ISlottable packSB = MakeSubSB();
								packSB.itemInst.Returns(MakePackInstance(0));
							ISlottable partsSB = MakeSubSB();
								partsSB.itemInst.Returns(MakePartsInstance(0, 1));
							SGFilter nullFilter = new SGNullFilter();
							SGBowFilter bowFilter = new SGBowFilter();
							SGWearFilter wearFilter = new SGWearFilter();
							SGCGearsFilter cGearsFilter = new SGCGearsFilter();
							SGPartsFilter partsFilter = new SGPartsFilter();
							yield return new object[]{nullFilter, bowSB, true};
							yield return new object[]{nullFilter, wearSB, true};
							yield return new object[]{nullFilter, shieldSB, true};
							yield return new object[]{nullFilter, mWeaponSB, true};
							yield return new object[]{nullFilter, quiverSB, true};
							yield return new object[]{nullFilter, packSB, true};
							yield return new object[]{nullFilter, partsSB, true};

							yield return new object[]{bowFilter, bowSB, true};
							yield return new object[]{bowFilter, wearSB, false};
							yield return new object[]{bowFilter, shieldSB, false};
							yield return new object[]{bowFilter, mWeaponSB, false};
							yield return new object[]{bowFilter, quiverSB, false};
							yield return new object[]{bowFilter, packSB, false};
							yield return new object[]{bowFilter, partsSB, false};
							
							yield return new object[]{wearFilter, bowSB, false};
							yield return new object[]{wearFilter, wearSB, true};
							yield return new object[]{wearFilter, shieldSB, false};
							yield return new object[]{wearFilter, mWeaponSB, false};
							yield return new object[]{wearFilter, quiverSB, false};
							yield return new object[]{wearFilter, packSB, false};
							yield return new object[]{wearFilter, partsSB, false};
							
							yield return new object[]{cGearsFilter, bowSB, false};
							yield return new object[]{cGearsFilter, wearSB, false};
							yield return new object[]{cGearsFilter, shieldSB, true};
							yield return new object[]{cGearsFilter, mWeaponSB, true};
							yield return new object[]{cGearsFilter, quiverSB, true};
							yield return new object[]{cGearsFilter, packSB, true};
							yield return new object[]{cGearsFilter, partsSB, false};
							
							yield return new object[]{partsFilter, bowSB, false};
							yield return new object[]{partsFilter, wearSB, false};
							yield return new object[]{partsFilter, shieldSB, false};
							yield return new object[]{partsFilter, mWeaponSB, false};
							yield return new object[]{partsFilter, quiverSB, false};
							yield return new object[]{partsFilter, packSB, false};
							yield return new object[]{partsFilter, partsSB, true};
						}
					}
				[TestCaseSource(typeof(ContainsCases))][Category("Methods")]
				public void Contains_Vairous_ReturnsAccordingly(List<ISlottable> sbs, ISlotSystemElement ele, bool expected){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);

					bool actual = sg.Contains(ele);

					Assert.That(actual, Is.EqualTo(expected));
				}
					class ContainsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable vSBA = MakeSubSB();
							ISlottable vSBB = MakeSubSB();
							ISlottable vSBC = MakeSubSB();
							ISlottable iSBA = MakeSubSB();
							ISlottable iSBB = MakeSubSB();
							ISlottable iSBC = MakeSubSB();
							List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{
								vSBA, vSBB, vSBC
							});
							yield return new object[]{sbs, vSBA, true};
							yield return new object[]{sbs, vSBB, true};
							yield return new object[]{sbs, vSBC, true};
							yield return new object[]{sbs, iSBA, false};
							yield return new object[]{sbs, iSBB, false};
							yield return new object[]{sbs, iSBC, false};
							yield return new object[]{sbs, MakeSubSG(), false};
							yield return new object[]{sbs, MakeSubSSE(), false};
						}
					}
				[Test][Category("Methods")]
				public void FocusSelf_WhenCalled_SetsSelStateFocused(){
					SlotGroup sg = MakeSG();

					sg.FocusSelf();

					Assert.That(sg.curSelState, Is.SameAs(SlotGroup.sgFocusedState));
				}
				[Test][Category("Methods")]
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
				[TestCaseSource(typeof(FocusSBsVariousCases))][Category("Methods")]
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
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
				public void DefocusSelf_WhenCalled_SetsSelStateDefocused(){
					SlotGroup sg = MakeSG();

					sg.DefocusSelf();

					Assert.That(sg.curSelState, Is.SameAs(SlotGroup.sgDefocusedState));
				}
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
				public void Deactivate_WhenCalled_CallsSBDeactivate(){
					SlotGroup sg = MakeSG();
						List<ISlottable> sbs;
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
							sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC});
						sg.SetSBs(sbs);
					
					sg.Deactivate();

					sbA.Received().Deactivate();
					sbB.Received().Deactivate();
					sbC.Received().Deactivate();
				}
				[Test][Category("Methods")]
				public void PerformInHierarchyV1_WhenCalled_ActsOnSelfAndAllSBs(){
					SlotGroup sg = MakeSG();
						List<ISlottable> sbs;
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
						sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC});
						sg.SetSBs(sbs);
					
					sg.PerformInHierarchy(TestMethodV1);

					Assert.That(sg.curSelState, Is.SameAs(SlotGroup.sgFocusedState));
					sbA.Received().Focus();
					sbB.Received().Focus();
					sbC.Received().Focus();
				}
					void TestMethodV1(ISlotSystemElement ele){
						ele.Focus();
					}
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
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

					Assert.That(list, Is.EqualTo(expected));
				}
					void TestMethodV3(ISlotSystemElement ele, IList<ISlotSystemElement> list){
						list.Add(ele);
					}
				[Test][Category("Methods")]
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

					sg.SetElements();

					List<ISlotSystemElement> actualEles = new List<ISlotSystemElement>(sg);
					Assert.That(actualEles.Count, Is.EqualTo(4));
				}
				[Test][Category("Methods")]
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

					sg.SetElements();

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
				[Test][Category("Methods")]
				public void InitializeState_WhenCalled_InitializesStates(){
					SlotGroup sg = MakeSG();

					sg.InitializeStates();

					Assert.That(sg.curSelState, Is.SameAs(SlotGroup.sgDeactivatedState));
					Assert.That(sg.prevSelState, Is.SameAs(SlotGroup.sgDeactivatedState));
					Assert.That(sg.curActState, Is.SameAs(SlotGroup.sgWaitForActionState));
					Assert.That(sg.prevActState, Is.SameAs(SlotGroup.sgWaitForActionState));
				}
				[TestCase(0)]
				[TestCase(10)]
				[Category("Methods")]
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
				[TestCaseSource(typeof(GetSBCases))][Category("Methods")]
				public void GetSB_Various_ReturnsSBorNull(List<ISlottable> sbs, InventoryItemInstance item, ISlottable expected){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);

					ISlottable actual = sg.GetSB(item);

					Assert.That(actual, Is.SameAs(expected));
				}
					class GetSBCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable vSBA = MakeSubSB();
								BowInstance vSBABow = MakeBowInstance(0);
								vSBA.itemInst.Returns(vSBABow);
							ISlottable vSBB = MakeSubSB();
								BowInstance vSBBBow = MakeBowInstance(0);
								vSBB.itemInst.Returns(vSBBBow);
							ISlottable vSBC = MakeSubSB();
								BowInstance vSBCBow = MakeBowInstance(0);
								vSBC.itemInst.Returns(vSBCBow);
							ISlottable iSBA = MakeSubSB();
								BowInstance iSBABow = MakeBowInstance(0);
								iSBA.itemInst.Returns(iSBABow);
							ISlottable iSBB = MakeSubSB();
								BowInstance iSBBBow = MakeBowInstance(0);
								iSBB.itemInst.Returns(iSBBBow);
							ISlottable iSBC = MakeSubSB();
								BowInstance iSBCBow = MakeBowInstance(0);
								iSBC.itemInst.Returns(iSBCBow);
							List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{vSBA, vSBB, vSBC});
							yield return new object[]{sbs, vSBABow, vSBA};
							yield return new object[]{sbs, vSBBBow, vSBB};
							yield return new object[]{sbs, vSBCBow, vSBC};
							yield return new object[]{sbs, iSBABow, null};
							yield return new object[]{sbs, iSBBBow, null};
							yield return new object[]{sbs, iSBCBow, null};
							
						}
					}
				
				[TestCaseSource(typeof(GetSBCases))][Category("Methods")]
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
				[Category("Methods")]
				public void UpdateSBs_WhenCalled_SetsNewSlotsWithTheSameNumberAsSBs(int count){
					SlotGroup sg = MakeSG();
						List<ISlottable> sbs = new List<ISlottable>();
						sg.SetSBs(sbs);
						List<ISlottable> newSBs = CreateSBs(count);
					
					sg.UpdateSBs(newSBs);

					Assert.That(sg.newSlots.Count, Is.EqualTo(count));
					Assert.That(sg.newSlots, Is.All.InstanceOf(typeof(Slot)));
				}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))][Category("Methods")]
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
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))][Category("Methods")]
				public void UpdateSBs_NewSBsAndSBsContainsSB_CallsSBSetActStateMoveWithin(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
					
					sg.UpdateSBs(newSBs);

					foreach(ISlottable sb in sg)
						if(newSBs.Contains(sb) && sbs.Contains(sb))
							sb.Received().SetActState(Slottable.moveWithinState);
						else
							sb.DidNotReceive().SetActState(Slottable.moveWithinState);
				}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))][Category("Methods")]
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
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))][Category("Methods")]
				public void UpdateSBs_NewSBNotContainsSB_CallsSBSetActStateRemoved(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);

					sg.UpdateSBs(newSBs);

					foreach(ISlottable sb in sg)
						if(!newSBs.Contains(sb))
							sb.Received().SetActState(Slottable.removedState);
						else
							sb.DidNotReceive().SetActState(Slottable.removedState);
				}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))][Category("Methods")]
				public void UpdateSBs_SBsNotContainsSB_CallsSBSetActStateAdded(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);

					sg.UpdateSBs(newSBs);

					foreach(ISlottable sb in sg)
						if(!sbs.Contains(sb))
							sb.Received().SetActState(Slottable.addedState);
						else
							sb.DidNotReceive().SetActState(Slottable.addedState);
				}
				[TestCase(0)]
				[TestCase(1)]
				[TestCase(10)]
				[Category("Methods")]
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
				[TestCaseSource(typeof(GetNewSlotCases))][Category("Methods")]
				public void GetNewSlot_itemFound_ReturnsNewSlot(List<ISlottable> sbs ,List<Slot> newSlots, InventoryItemInstance item, Slot expected){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
						sg.SetNewSlots(newSlots);
					
					Slot actual = sg.GetNewSlot(item);

					Assert.That(actual, Is.SameAs(expected));
				}
					class GetNewSlotCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable sbA = MakeSubSB();
								BowInstance bowSBA = MakeBowInstance(0);
								sbA.newSlotID.Returns(0);
								sbA.itemInst.Returns(bowSBA);
							ISlottable sbB = MakeSubSB();
								WearInstance wearSBB = MakeWearInstance(0);
								sbB.newSlotID.Returns(1);
								sbB.itemInst.Returns(wearSBB);
							ISlottable sbC = MakeSubSB();
								ShieldInstance shieldSBC = MakeShieldInstance(0);
								sbC.newSlotID.Returns(2);
								sbC.itemInst.Returns(shieldSBC);
							ISlottable sbD = MakeSubSB();
								MeleeWeaponInstance mWeaponSBD = MakeMeleeWeaponInstance(0);
								sbD.newSlotID.Returns(3);
								sbD.itemInst.Returns(mWeaponSBD);
							List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC, sbD});
							List<Slot> newSlots = new List<Slot>();
								foreach(var sb in sbs)
									newSlots.Add(new Slot());
							yield return new object[]{sbs, newSlots, bowSBA, newSlots[0]};
							yield return new object[]{sbs, newSlots, wearSBB, newSlots[1]};
							yield return new object[]{sbs, newSlots, shieldSBC, newSlots[2]};
							yield return new object[]{sbs, newSlots, mWeaponSBD, newSlots[3]};

						}
					}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))][Category("Methods")]
				public void SetSBsActStates_NewSbsAndSBsContainsSB_CallsSBSetActStateMoveWithin(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
						sg.SetNewSBs(newSBs);
					List<ISlottable> allSBs = new List<ISlottable>(sbs);
					foreach(var sb in newSBs)
						if(!sbs.Contains(sb)) allSBs.Add(sb);
					sg.SetSBsActStates();

					foreach(var sb in allSBs)
						if(sbs.Contains(sb) && newSBs.Contains(sb))
							sb.Received().SetActState(Slottable.moveWithinState);
						else
							sb.DidNotReceive().SetActState(Slottable.moveWithinState);
				}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))][Category("Methods")]
				public void SetSBsActStates_NewSBsNotContainsSB_CallsSBSetActStateRemoved(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
						sg.SetNewSBs(newSBs);
					List<ISlottable> allSBs = new List<ISlottable>(sbs);
					foreach(var sb in newSBs)
						if(!sbs.Contains(sb)) allSBs.Add(sb);
					sg.SetSBsActStates();

					foreach(var sb in allSBs)
						if(!newSBs.Contains(sb))
							sb.Received().SetActState(Slottable.removedState);
						else
							sb.DidNotReceive().SetActState(Slottable.removedState);
				}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))][Category("Methods")]
				public void SetSBsActStates_SBsNotContainsSB_CallsSBSetActStateAdded(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
						sg.SetNewSBs(newSBs);
					List<ISlottable> allSBs = new List<ISlottable>(sbs);
					foreach(var sb in newSBs)
						if(!sbs.Contains(sb)) allSBs.Add(sb);
					sg.SetSBsActStates();

					foreach(var sb in allSBs)
						if(!sbs.Contains(sb))
							sb.Received().SetActState(Slottable.addedState);
						else
							sb.DidNotReceive().SetActState(Slottable.addedState);
				}
				[TestCaseSource(typeof(SyncSBsToSlotsCases))][Category("Methods")]
				public void SyncSBsToSlots_WhenCalled_SyncSBsToSlots(List<Slot> slots, List<ISlottable> expected){
					SlotGroup sg = MakeSG();
						sg.SetSlots(slots);
					
					sg.SyncSBsToSlots();

					Assert.That(sg.toList, Is.EqualTo(expected));
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
				[Category("Methods")]
				public void OnCompleteSlotMovements_NewSlotIDMinus1_CallsSBDestory(bool removed, bool expected){
					SlotGroup sg = MakeSG();
						List<ISlottable> sbs;
							ISlottable sb = MakeSubSB();
							sb.isRemoved.Returns(removed);
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
				[TestCaseSource(typeof(OnCompleteSlotMovementsCases))][Category("Methods")]
				public void OnCompleteSlotMovements_WhenCalledAfterNewSlotsAreSet_SyncSBsToSlotsWithNewSlotIDs(List<ISlottable> sbs, List<ISlottable> expected){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
						List<Slot> newSlots = new List<Slot>();
							foreach(var sb in sbs)
								newSlots.Add(new Slot());
							sg.SetNewSlots(newSlots);
					
					sg.OnCompleteSlotMovements();

					Assert.That(sg.toList, Is.EqualTo(expected));
					foreach(var sb in sg)
						if(sb != null)
							((ISlottable)sb).Received().SetSlotID(sg.toList.IndexOf((ISlottable)sb));
				}
					class OnCompleteSlotMovementsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable rsbA = MakeSubSB();
								rsbA.newSlotID.Returns(-1);
								rsbA.isRemoved.Returns(true);
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
								rsbB.isRemoved.Returns(true);
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
				[TestCaseSource(typeof(SwappableSBsCases))][Category("Methods")]
				public void SwappableSBs_SBAreSwappable_ReturnsList(SlotGroup sg, ISlottable sb, List<ISlottable> expected){
					List<ISlottable> actual = sg.SwappableSBs(sb);

					Assert.That(actual, Is.EqualTo(expected));
				}
					class SwappableSBsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							SlotGroup sg = MakeSG();
								sg.SetFilter(new SGNullFilter());
								ISlottable bowSBA = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg);
								ISlottable bowSBB = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg);
								ISlottable wearSBA = MakeSubSBWithItemAndSG(MakeWearInstance(0), sg);
								ISlottable shieldSBA = MakeSubSBWithItemAndSG(MakeShieldInstance(0), sg);
								ISlottable mWeaponSBA = MakeSubSBWithItemAndSG(MakeMeleeWeaponInstance(0), sg);
								ISlottable quiverSBA = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg);
								ISlottable packSBA = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg);
								ISlottable partsSBA = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), sg);
								sg.SetSBs(new List<ISlottable>(new ISlottable[]{
									bowSBA, bowSBB, wearSBA, shieldSBA, mWeaponSBA, quiverSBA, packSBA, partsSBA
								}));
							SlotGroup bowSG = MakeSG();
								bowSG.SetFilter(new SGBowFilter());
								ISlottable oBowSBA = MakeSubSBWithItemAndSG(MakeBowInstance(0), bowSG);
								ISlottable oBowSBB = MakeSubSBWithItemAndSG(MakeBowInstance(1), bowSG);
							SlotGroup wearSG = MakeSG();
								wearSG.SetFilter(new SGWearFilter());
								ISlottable oWearSBA = MakeSubSBWithItemAndSG(MakeWearInstance(0), wearSG);
								ISlottable oWearSBB = MakeSubSBWithItemAndSG(MakeWearInstance(1), wearSG);
							SlotGroup cGearsSG = MakeSG();
								cGearsSG.SetFilter(new SGCGearsFilter());
								ISlottable oShieldSBA = MakeSubSBWithItemAndSG(MakeShieldInstance(0), cGearsSG);
								ISlottable oMWeaponSBA = MakeSubSBWithItemAndSG(MakeMeleeWeaponInstance(0), cGearsSG);
							SlotGroup partsSG = MakeSG();
								partsSG.SetFilter(new SGPartsFilter());
								ISlottable oPartsSBA = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), partsSG);
								ISlottable oPartsSBB = MakeSubSBWithItemAndSG(MakePartsInstance(1, 1), partsSG);
							List<ISlottable> empty = new List<ISlottable>();
							yield return new object[]{sg, bowSBA, empty};
							yield return new object[]{sg, bowSBB, empty};
							yield return new object[]{sg, wearSBA, empty};
							yield return new object[]{sg, shieldSBA, empty};
							yield return new object[]{sg, mWeaponSBA, empty};
							yield return new object[]{sg, quiverSBA, empty};
							yield return new object[]{sg, packSBA, empty};
							yield return new object[]{sg, partsSBA, empty};
							yield return new object[]{sg, oBowSBA, new List<ISlottable>(new ISlottable[]{bowSBA, bowSBB})};
							yield return new object[]{sg, oBowSBB, new List<ISlottable>(new ISlottable[]{bowSBA, bowSBB})};
							yield return new object[]{sg, oWearSBA, new List<ISlottable>(new ISlottable[]{wearSBA})};
							yield return new object[]{sg, oWearSBB, new List<ISlottable>(new ISlottable[]{wearSBA})};
							yield return new object[]{sg, oShieldSBA, new List<ISlottable>(new ISlottable[]{shieldSBA, mWeaponSBA, quiverSBA, packSBA})};
							yield return new object[]{sg, oMWeaponSBA, new List<ISlottable>(new ISlottable[]{shieldSBA, mWeaponSBA, quiverSBA, packSBA})};
							yield return new object[]{sg, oPartsSBA, empty};
							yield return new object[]{sg, oPartsSBB, new List<ISlottable>(new ISlottable[]{partsSBA})};
						}
					}
				[Test][Category("Methods")]
				public void Reset_WhenCalled_SetActStateWFA(){
					SlotGroup sg = MakeSG();

					sg.Reset();
					
					Assert.That(sg.curActState, Is.SameAs(SlotGroup.sgWaitForActionState));
				}
				[Test][Category("Methods")]
				public void Reset_WhenCalled_SetsFields(){
					SlotGroup sg = MakeSG();
						sg.SetNewSBs(new List<ISlottable>());
						sg.SetNewSlots(new List<Slot>());

					sg.Reset();

					Assert.That(sg.newSBs, Is.Null);
					Assert.That(sg.newSlots, Is.Null);
				}
				[TestCaseSource(typeof(ReorderAndUpdateSBsCases))][Category("Methods")]
				public void ReorderAndUpdateSBs_WhenCalled_CallsSBsSetNewSlotIDAndSetActStateMoveWithin(ISlottable picked, ISlottable target, List<ISlottable> sbs, Dictionary<ISlottable, int> newSlotIDs){
					ISlotSystemManager ssm = MakeSubSSM();
						ssm.pickedSB.Returns(picked);
						ssm.targetSB.Returns(target);
					SlotGroup sg = MakeSG();
						sg.SetSSM(ssm);
						sg.SetSBs(sbs);
					
					sg.ReorderAndUpdateSBs();

					foreach(ISlottable sb in sg){
						sb.Received().SetActState(Slottable.moveWithinState);
						sb.Received().SetNewSlotID(newSlotIDs[sb]);
					}
				}
					class ReorderAndUpdateSBsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
							ISlottable sbD = MakeSubSB();
							ISlottable sbE = MakeSubSB();
							List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC, sbD, sbE});
							ISlottable case1Picked = sbE;
							ISlottable case1Target = sbC;
							Dictionary<ISlottable, int> case1IDs = new Dictionary<ISlottable,int>();
								case1IDs.Add(sbA, 0);
								case1IDs.Add(sbB, 1);
								case1IDs.Add(sbE, 2);
								case1IDs.Add(sbC, 3);
								case1IDs.Add(sbD, 4);
							yield return new object[]{case1Picked, case1Target, sbs, case1IDs};
							ISlottable case2Picked = sbA;
							ISlottable case2Target = sbD;
							Dictionary<ISlottable, int> case2IDs = new Dictionary<ISlottable,int>();
								case2IDs.Add(sbB, 0);
								case2IDs.Add(sbC, 1);
								case2IDs.Add(sbD, 2);
								case2IDs.Add(sbA, 3);
								case2IDs.Add(sbE, 4);
							yield return new object[]{case2Picked, case2Target, sbs, case2IDs};
						}
					}				
				[Test][Category("Methods")]
				public void UpdateToRevert_WhenCalled_SetsNewSBsWithSBs(){
					List<ISlottable> sbs = CreateSBs(3);
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
					
					sg.UpdateToRevert();

					Assert.That(sg.newSBs, Is.EqualTo(sbs));
				}
				[TestCase(1)]
				[TestCase(3)]
				[TestCase(10)]
				[Category("Methods")]
				public void UpdateToRevert_WhenCalled_SetsNewSlotsBySBsCount(int count){
					List<ISlottable> sbs = CreateSBs(count);
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
					
					sg.UpdateToRevert();

					Assert.That(sg.newSlots.Count, Is.EqualTo(count));
					foreach(var slot in sg.newSlots)
						Assert.That(slot.sb, Is.Null);
				}
				[Test][Category("Methods")]
				public void UpdateToRevert_WhenCalled_DoesNotChangeSBs(){
					List<ISlottable> sbs = CreateSBs(3);
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
					
					sg.UpdateToRevert();

					Assert.That(sg.toList, Is.EqualTo(sbs));
				}
				[Test][Category("Methods")]
				public void UpdateToRevert_WhenCalled_CallSBsSetActStateMoveWithin(){
					List<ISlottable> sbs = CreateSBs(3);
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
					
					sg.UpdateToRevert();

					foreach(ISlottable sb in sg)
						sb.Received().SetActState(Slottable.moveWithinState);
				}
				[TestCaseSource(typeof(SortAndUpdateSBsCases))][Category("Methods")]
				public void SortAndUpdateSBs_SGIsNotExpandable_SortsAndCallsSBsSetNewSlotIDs(SGSorter sorter, List<ISlottable> sbs, List<ISlottable> expOrder){
					SlotGroup sg = MakeSG();
						// sg.Initialize("someSG", new SGNullFilter(), new PoolInventory(), true, 10, new SGEmptyCommand(), new SGEmptyCommand());/* this makes isExpandable false */
						sg.SetSBs(sbs);
						sg.SetSorter(sorter);
					
					sg.SortAndUpdateSBs();

					foreach(ISlottable sb in sg){
						if(sb != null){
							sb.Received().SetNewSlotID(expOrder.IndexOf(sb));
							sb.Received().SetActState(Slottable.moveWithinState);
						}
					}
				}
					class SortAndUpdateSBsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable partsSBB = MakeSubSBWithItem(MakePartsInstWithOrder(1, 1, 0));
							ISlottable bowSBA = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
							ISlottable wearSBA = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
							ISlottable quiverSBA = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 3));
							ISlottable bowSBC = MakeSubSBWithItem(MakeBowInstWithOrder(0, 4));
							ISlottable packSBA = MakeSubSBWithItem(MakePackInstWithOrder(0, 5));
							ISlottable shieldSBA = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
							ISlottable partsSBA = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 7));
							ISlottable bowSBB = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
							ISlottable mWeaponSBA = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 9));

							List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{
								quiverSBA,
								partsSBB,
								null,
								null,
								partsSBA,
								bowSBB,
								wearSBA,
								bowSBA,
								packSBA,
								null,
								shieldSBA,
								bowSBC,
								mWeaponSBA
							});
							List<ISlottable> itemIDOrdered = new List<ISlottable>(new ISlottable[]{
								bowSBA,
								bowSBC,
								bowSBB,
								wearSBA,
								shieldSBA,
								mWeaponSBA,
								quiverSBA,
								packSBA,
								partsSBA,
								partsSBB,
								null,
								null,
								null
							});
							List<ISlottable> invIDOrdered = new List<ISlottable>(new ISlottable[]{
								partsSBB,
								partsSBA,
								packSBA,
								quiverSBA,
								mWeaponSBA,
								shieldSBA,
								wearSBA,
								bowSBB,
								bowSBC,
								bowSBA,
								null,
								null,
								null
							});
							List<ISlottable> acqOrdered = new List<ISlottable>(new ISlottable[]{
								partsSBB,
								bowSBA,
								wearSBA,
								quiverSBA,
								bowSBC,
								packSBA,
								shieldSBA,
								partsSBA,
								bowSBB,
								mWeaponSBA,
								null,
								null,
								null
							});
							yield return new object[]{new SGItemIDSorter(), sbs, itemIDOrdered};
							yield return new object[]{new SGInverseItemIDSorter(), sbs, invIDOrdered};
							yield return new object[]{new SGAcquisitionOrderSorter(), sbs, acqOrdered};
						}
					}
				[TestCaseSource(typeof(SortAndUpdateSBsCases))][Category("Methods")]
				public void SortAndUpdateSBs_SGIsNotExpandable_SGNewSBsRetainsSize(SGSorter sorter, List<ISlottable> sbs, List<ISlottable> expOrder){
					SlotGroup sg = MakeSG();
						sg.SetSBs(sbs);
						sg.SetSorter(sorter);
					
					sg.SortAndUpdateSBs();

					Assert.That(sg.newSBs.Count, Is.EqualTo(sbs.Count));
				}
				[TestCaseSource(typeof(SortAndUpdateSBsCases))][Category("Methods")]
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
				[TestCaseSource(typeof(SortAndUpdateSBsCases))][Category("Methods")]
				public void SortAndUpdateSBs_SGIsExpandable_SortsAndCallsSBsSetNewSlotIDs(SGSorter sorter, List<ISlottable> sbs, List<ISlottable> expOrder){
					SlotGroup sg = MakeSG();
						sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), sorter, 0);
						sg.SetSBs(sbs);
					
					sg.SortAndUpdateSBs();

					foreach(ISlottable sb in sg){
						if(sb != null){
							sb.Received().SetNewSlotID(expOrder.IndexOf(sb));
							sb.Received().SetActState(Slottable.moveWithinState);
						}
					}
				}
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
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
				[TestCaseSource(typeof(CreateNewSBAndFillCases))][Category("Methods")]
				public void CreateNewSBAndFill_WhenCalled_UpdateList(List<ISlottable> list, ISlottable added, int addedIndex){
					SlotGroup sg = MakeSG();
						ISlotSystemManager ssm = MakeSubSSM();
						sg.SetSSM(ssm);
					CreateNewSBAndFillTestData expected = new CreateNewSBAndFillTestData(added.itemInst, ssm, Slottable.sbDefocusedState, Slottable.unequippedState);
					List<ISlottable> targetList = new List<ISlottable>(list);
					sg.CreateNewSBAndFill(added.itemInst, targetList);

					ISlottable actualAdded = targetList[addedIndex];
					CreateNewSBAndFillTestData actual = new CreateNewSBAndFillTestData(actualAdded.itemInst, actualAdded.ssm, actualAdded.curSelState, actualAdded.curEqpState);
					Assert.That(actualAdded, Is.Not.Null.And.InstanceOf(typeof(Slottable)));
					bool equality = actual.Equals(expected);
					Assert.That(equality, Is.True);
				}
					class CreateNewSBAndFillTestData: IEquatable<CreateNewSBAndFillTestData>{
						public InventoryItemInstance item;
						public ISlotSystemManager ssm;
						public SSEState sbSelState;
						public SSEState sbEqpState;
						public CreateNewSBAndFillTestData(InventoryItemInstance item, ISlotSystemManager ssm, SSEState selState, SSEState eqpState){
							this.item = item;
							this.ssm = ssm;
							this.sbSelState = selState;
							this.sbEqpState = eqpState;
						}
						public bool Equals(CreateNewSBAndFillTestData other){
							bool flag = true;
							flag &= object.ReferenceEquals(this.item, other.item);
							flag &= object.ReferenceEquals(this.ssm, other.ssm);
							flag &= object.ReferenceEquals(this.sbSelState, other.sbSelState);
							flag &= object.ReferenceEquals(this.sbEqpState, other.sbEqpState);
							return flag;
						}
						
					}
					class CreateNewSBAndFillCases:IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
							ISlottable sbD = MakeSubSB();
							ISlottable added = MakeSubSB();
								BowInstance bow = MakeBowInstance(0);
								added.itemInst.Returns(bow);
							List<ISlottable> whole = new List<ISlottable>(new ISlottable[]{
								sbA, sbB, sbC, sbD
							});
							List<ISlottable> hasEmptyA = new List<ISlottable>(new ISlottable[]{
								null, sbA, null, null, sbB, sbC, null ,sbD, null
							});
							List<ISlottable> hasEmptyB = new List<ISlottable>(new ISlottable[]{
								sbA, sbB, sbC, null ,sbD, null
							});
							yield return new object[]{whole, added, 4};
							yield return new object[]{hasEmptyA, added, 0};
							yield return new object[]{hasEmptyB, added, 3};
						}
					}
				[TestCaseSource(typeof(NullifyIndexOfCases))][Category("Methods")]
				public void NullifyIndexOf_WhenCalled_FindByItemAndReplaceWithNull(List<ISlottable> list, ISlottable item, int nulledIndex){
					SlotGroup sg = MakeSG();
					List<ISlottable> targetList = new List<ISlottable>(list);

					sg.NullifyIndexOf(item.itemInst, targetList);

					ISlottable actual = targetList[nulledIndex];

					Assert.That(actual, Is.Null);
				}
					class NullifyIndexOfCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable sbA = MakeSubSB();
								BowInstance bowA = MakeBowInstance(0);
								sbA.itemInst.Returns(bowA);
							ISlottable sbB = MakeSubSB();
								WearInstance wearB = MakeWearInstance(0);
								sbB.itemInst.Returns(wearB);
							ISlottable sbC = MakeSubSB();
								ShieldInstance shieldC = MakeShieldInstance(0);
								sbC.itemInst.Returns(shieldC);
							ISlottable sbD = MakeSubSB();
								MeleeWeaponInstance mWeaponD = MakeMeleeWeaponInstance(0);
								sbD.itemInst.Returns(mWeaponD);
							ISlottable rSBA = MakeSubSB();
								rSBA.itemInst.Returns(bowA);
							ISlottable rSBB = MakeSubSB();
								rSBB.itemInst.Returns(wearB);
							ISlottable rSBC = MakeSubSB();
								rSBC.itemInst.Returns(shieldC);
							ISlottable rSBD = MakeSubSB();
								rSBD.itemInst.Returns(mWeaponD);
							List<ISlottable> list = new List<ISlottable>(new ISlottable[]{
								sbA, null, sbB, null, null, sbC, sbD, null
							});
							yield return new object[]{list, rSBA, 0};
							yield return new object[]{list, rSBB, 2};
							yield return new object[]{list, rSBC, 5};
							yield return new object[]{list, rSBD, 6};

						}	
					}
				[TestCaseSource(typeof(FillAndUpdateSBs_SGNotPoolAndSSMSG1NotThisCases))][Category("Methods")]
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
						sg.SetSSM(ssm);
						sg.SetSBs(sbs);
						sg.ToggleAutoSort(isAutoSort);
					FillAndUpdateSBsTestData expected = new FillAndUpdateSBsTestData(ssm, Slottable.sbDefocusedState, Slottable.unequippedState);
					
					sg.FillAndUpdateSBs();

					ISlottable added = sg.toList[sbs.Count];
					Assert.That(added, Is.Not.Null);
					FillAndUpdateSBsTestData actual = new FillAndUpdateSBsTestData(added.ssm, added.curSelState, added.curEqpState);
					bool equality = actual.Equals(expected);
					Assert.That(equality, Is.True);
				}
					class FillAndUpdateSBs_SGNotPoolAndSSMSG1NotThisCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
							ISlottable sbD = MakeSubSB();
							List<ISlottable> whole = new List<ISlottable>(new ISlottable[]{
								sbA, sbB, sbC, sbD
							});
							List<ISlottable> hasEmpty = new List<ISlottable>(new ISlottable[]{
								null, sbA, null, null, sbB, sbC, null, sbD
							});
							yield return new object[]{true, true, whole};
							yield return new object[]{true, false, whole};
							yield return new object[]{false, true, whole};
							yield return new object[]{false, false, whole};
							
							yield return new object[]{true, true, hasEmpty};
							yield return new object[]{true, false, hasEmpty};
							yield return new object[]{false, true, hasEmpty};
							yield return new object[]{false, false, hasEmpty};
						}
					}
					class FillAndUpdateSBsTestData: IEquatable<FillAndUpdateSBsTestData>{
						public ISlotSystemManager ssm;
						public SSEState sbSelState;
						public SSEState sbEqpState;
						public FillAndUpdateSBsTestData(ISlotSystemManager ssm, SSEState sbSelState, SSEState sbEqpState){
							this.ssm = ssm;
							this.sbSelState = sbSelState;
							this.sbEqpState = sbEqpState;
						}
						public bool Equals(FillAndUpdateSBsTestData other){
							bool flag = true;
							flag &= object.ReferenceEquals(this.ssm, other.ssm);
							flag &= object.ReferenceEquals(this.sbSelState, other.sbSelState);
							flag &= object.ReferenceEquals(this.sbEqpState, other.sbEqpState);
							return flag;
						}

					}
				[TestCaseSource(typeof(FillAndUpdateSBs_VariousCases))][Category("Methods")]
				public void FillAndUpdateSBs_Various_SetsNewSBsAccordingly(
					bool added, 
					ISlottable pickedSB, 
					bool isAutoSort, 
					SGSorter sorter, 
					bool isPool, 
					bool isExpandable, 
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
					Assert.That(sg.newSBs, Is.EqualTo(expected));
				}
					class FillAndUpdateSBs_VariousCases:IEnumerable{
						public IEnumerator GetEnumerator(){
							/* Sbs */
								ISlottable partsSBA = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bowSBC = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSBA = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSBA = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSBA = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSBA = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSBA = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bowSBA = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								ISlottable bowSBB = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
							
							List<ISlottable> wholeList = new List<ISlottable>(new ISlottable[]{
								partsSBA, 
								bowSBC, 
								wearSBA, 
								mWeaponSBA, 
								packSBA, 
								quiverSBA, 
								shieldSBA, 
								bowSBA
							});
							List<ISlottable> hasEmptyList = new List<ISlottable>(new ISlottable[]{
								partsSBA,
								bowSBC,
								wearSBA,
								null,
								mWeaponSBA,
								null,
								null,
								packSBA,
								null,
								quiverSBA,
								shieldSBA,
								bowSBA,
								null
							});
							object[] NoAddNoRemNoSort_isPoolTrue;
								NoAddNoRemNoSort_isPoolTrue = new object[]{
									false, bowSBB, false, new SGItemIDSorter(), true, false, wholeList, wholeList, -1
								};
								yield return NoAddNoRemNoSort_isPoolTrue;
							object[] NoAddNoRemSortNoResize_isPoolTrue_isAutoSortTrue_isExpandableFalse;
								List<ISlottable> hasEmptyItemIDNoResize = new List<ISlottable>(){
									bowSBC,
									bowSBA,
									wearSBA,
									shieldSBA,
									mWeaponSBA,
									quiverSBA,
									packSBA,
									partsSBA,
									null,
									null,
									null,
									null,
									null
								};
								NoAddNoRemSortNoResize_isPoolTrue_isAutoSortTrue_isExpandableFalse = new object[]{
									false, bowSBB, true, new SGItemIDSorter(), true, false, hasEmptyList, hasEmptyItemIDNoResize, -1
								};
								yield return NoAddNoRemSortNoResize_isPoolTrue_isAutoSortTrue_isExpandableFalse;
							object[] NoAddNoRemSortResize_isPoolTrue_isAutoSortTrue_isExpandableTrue;
								List<ISlottable> hasEmptyItemIDResize = new List<ISlottable>(){
									bowSBC,
									bowSBA,
									wearSBA,
									shieldSBA,
									mWeaponSBA,
									quiverSBA,
									packSBA,
									partsSBA
								};
								NoAddNoRemSortResize_isPoolTrue_isAutoSortTrue_isExpandableTrue = new object[]{
									false, bowSBB, true, new SGItemIDSorter(), true, true, hasEmptyList, hasEmptyItemIDResize, -1
								};
								yield return NoAddNoRemSortResize_isPoolTrue_isAutoSortTrue_isExpandableTrue;
							object[] AddNoSort_isPoolFalse_isAutoSortFalse;
								List<ISlottable> filledIn = new List<ISlottable>(new ISlottable[]{
									partsSBA,
									bowSBC,
									wearSBA,
									bowSBB,
									mWeaponSBA,
									null,
									null,
									packSBA,
									null,
									quiverSBA,
									shieldSBA,
									bowSBA,
									null
								});
								AddNoSort_isPoolFalse_isAutoSortFalse = new object[]{
									true, bowSBB, false, new SGItemIDSorter(), false, false, hasEmptyList, filledIn, 3
								};
								yield return AddNoSort_isPoolFalse_isAutoSortFalse;
							object[] AddNoSortOnWhole_isPoolFalse_isAutoSortFalse;
								List<ISlottable> wholeAdded = new List<ISlottable>(new ISlottable[]{
									partsSBA, 
									bowSBC, 
									wearSBA, 
									mWeaponSBA, 
									packSBA, 
									quiverSBA, 
									shieldSBA, 
									bowSBA,
									bowSBB
								});
								AddNoSortOnWhole_isPoolFalse_isAutoSortFalse = new object[]{
									true, bowSBB, false, new SGItemIDSorter(), false, false, wholeList, wholeAdded, 8
								};
								yield return AddNoSortOnWhole_isPoolFalse_isAutoSortFalse;
							object[] AddSortNoResize_isPoolFalse_isAutoSortTrue_isExpandableFalse;
								List<ISlottable> filledSortedNoResize = new List<ISlottable>(new ISlottable[]{
									bowSBC,
									bowSBA,
									bowSBB,
									wearSBA,
									shieldSBA,
									mWeaponSBA,
									quiverSBA,
									packSBA,
									partsSBA,
									null,
									null,
									null,
									null
								});
								AddSortNoResize_isPoolFalse_isAutoSortTrue_isExpandableFalse = new object[]{
									true, bowSBB, true, new SGItemIDSorter(), false, false, hasEmptyList, filledSortedNoResize, 2
								};
								yield return AddSortNoResize_isPoolFalse_isAutoSortTrue_isExpandableFalse;
							object[] AddSortResize_isPoolFalse_isAutoSortTrue_isExpandableTrue;
								List<ISlottable> addSortResize = new List<ISlottable>(new ISlottable[]{
									bowSBC,
									bowSBA,
									bowSBB,
									wearSBA,
									shieldSBA,
									mWeaponSBA,
									quiverSBA,
									packSBA,
									partsSBA
								});
								AddSortResize_isPoolFalse_isAutoSortTrue_isExpandableTrue = new object[]{
									true, bowSBB, true, new SGItemIDSorter(), false, true, hasEmptyList, addSortResize, 2
								};
								yield return AddSortResize_isPoolFalse_isAutoSortTrue_isExpandableTrue;
							object[] RemoveNoSort_isPoolFalse_isAutoSortFalse;
								List<ISlottable> removeNoSort = new List<ISlottable>(new ISlottable[]{
									partsSBA,
									bowSBC,
									wearSBA,
									null,
									mWeaponSBA,
									null,
									null,
									packSBA,
									null,
									quiverSBA,
									shieldSBA,
									null,
									null
								});
								RemoveNoSort_isPoolFalse_isAutoSortFalse = new object[]{
									false, bowSBA, false, new SGItemIDSorter(), false, false, hasEmptyList, removeNoSort, -1
								};
								yield return RemoveNoSort_isPoolFalse_isAutoSortFalse;
							object[] RemovSortNoResize_isPoolFalse_isAutoSortTrue_isExpandableFalse;
								List<ISlottable> removeSortNoResize = new List<ISlottable>(new ISlottable[]{
									bowSBC,
									wearSBA,
									shieldSBA,
									mWeaponSBA,
									quiverSBA,
									packSBA,
									partsSBA,
									null,
									null,
									null,
									null,
									null,
									null
								});
								RemovSortNoResize_isPoolFalse_isAutoSortTrue_isExpandableFalse = new object[]{
									false, bowSBA, true, new SGItemIDSorter(), false, false, hasEmptyList, removeSortNoResize, -1
								};
								yield return RemovSortNoResize_isPoolFalse_isAutoSortTrue_isExpandableFalse;
							object[] RemovSortResize_isPoolFalse_isAutoSortTrue_isExpandableTrue;
								List<ISlottable> removeSortResize = new List<ISlottable>(new ISlottable[]{
									bowSBC,
									wearSBA,
									shieldSBA,
									mWeaponSBA,
									quiverSBA,
									packSBA,
									partsSBA,
								});
								RemovSortResize_isPoolFalse_isAutoSortTrue_isExpandableTrue = new object[]{
									false, bowSBA, true, new SGItemIDSorter(), false, true, hasEmptyList, removeSortResize, -1
								};
								yield return RemovSortResize_isPoolFalse_isAutoSortTrue_isExpandableTrue;
						}
					}
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
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
					CreatedNewSBData expected = new CreatedNewSBData(bow, ssm, Slottable.sbDefocusedState, Slottable.unequippedState);
					
					sg.SwapAndUpdateSBs();

					ISlottable actualAdded = sg.toList[sg.toList.Count -1];
					CreatedNewSBData actual = new CreatedNewSBData(actualAdded.itemInst, actualAdded.ssm, actualAdded.curSelState, actualAdded.curEqpState);
					bool equality = actual.Equals(expected);
					Assert.That(equality, Is.True);
				}
				[Test][Category("Methods")]
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
					CreatedNewSBData expected = new CreatedNewSBData(bow, ssm, Slottable.sbDefocusedState, Slottable.unequippedState);
					
					sg.SwapAndUpdateSBs();

					ISlottable actualAdded = sg.toList[sg.toList.Count -1];
					CreatedNewSBData actual = new CreatedNewSBData(actualAdded.itemInst, actualAdded.ssm, actualAdded.curSelState, actualAdded.curEqpState);
					bool equality = actual.Equals(expected);
					Assert.That(equality, Is.True);
				}
					public class CreatedNewSBData: IEquatable<CreatedNewSBData>{
						public InventoryItemInstance item;
						public ISlotSystemManager ssm;
						public SSEState sbSelState;
						public SSEState sbEqpState;
						public CreatedNewSBData(InventoryItemInstance item, ISlotSystemManager ssm, SSEState sbSelState, SSEState sbEqpState){
							this.item = item;
							this.ssm = ssm;
							this.sbSelState = sbSelState;
							this.sbEqpState = sbEqpState;
						}
						public bool Equals(CreatedNewSBData other){
							bool flag = true;
							flag &= object.ReferenceEquals(this.item, other.item);
							flag &= object.ReferenceEquals(this.ssm, other.ssm);
							flag &= object.ReferenceEquals(this.sbSelState, other.sbSelState);
							flag &= object.ReferenceEquals(this.sbEqpState, other.sbEqpState);
							return flag;
						}
					}
				[TestCaseSource(typeof(SwapAndUpdateSBsCases))][Category("Methods")]
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
					
					Assert.That(actual, Is.EqualTo(expList));
				}
					class SwapAndUpdateSBsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							SGSorter idSorter = new SGItemIDSorter();
							/* SBs */
								ISlottable sbBowA = MakeSubSBWithItem(MakeBowInstWithOrder(0, 0));
								ISlottable sbBowB = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable sbBowC = MakeSubSBWithItem(MakeBowInstWithOrder(0, 2));
								ISlottable sbWear = MakeSubSBWithItem(MakeWearInstWithOrder(0, 3));
								ISlottable sbShield = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 4));
								ISlottable sbMWeapon = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 5));
								ISlottable sbQuiver = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 6));
								ISlottable sbPack = MakeSubSBWithItem(MakePackInstWithOrder(0, 7));
								ISlottable sbParts = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 8));
								ISlottable sbWearA = MakeSubSBWithItem(MakeWearInstWithOrder(0, 9));
							
							IList<ISlottable> hasEmpty = new List<ISlottable>(new ISlottable[]{
								null,
								sbQuiver,
								sbBowB,
								null,
								sbBowC,
								sbMWeapon,
								null,
								sbBowA,
								null,
								sbWear,
								null,
								sbShield,
								sbPack,
								null,
								null,
								sbParts
							});
							object[] NoAddNoRemNoSort_isPoolTrue_isAutoSortFalse;
								NoAddNoRemNoSort_isPoolTrue_isAutoSortFalse = new object[]{
									true, false, false, idSorter, true, sbWearA, sbBowC, hasEmpty, hasEmpty, -1
								};
								yield return NoAddNoRemNoSort_isPoolTrue_isAutoSortFalse;
							object[] NoAddNoRemSortNoResize_isPoolTrue_isAutoSortTrue_isExpandableFalse;
								List<ISlottable> noAddNoRemSortNoResize = new List<ISlottable>(new ISlottable[]{
									sbBowA,
									sbBowB,
									sbBowC,
									sbWear,
									sbShield,
									sbMWeapon,
									sbQuiver,
									sbPack,
									sbParts,
									null,
									null,
									null,
									null,
									null,
									null,
									null
								});
								NoAddNoRemSortNoResize_isPoolTrue_isAutoSortTrue_isExpandableFalse = new object[]{
									true, true, false, idSorter, true, sbWearA, sbBowC, hasEmpty, noAddNoRemSortNoResize, -1
								};
								yield return NoAddNoRemSortNoResize_isPoolTrue_isAutoSortTrue_isExpandableFalse;
							object[] NoAddNoRemSortResize_isPoolTrue_isAutoSortTrue_isExpandableTrue;
								List<ISlottable> noAddNoRemSortResize = new List<ISlottable>(new ISlottable[]{
									sbBowA,
									sbBowB,
									sbBowC,
									sbWear,
									sbShield,
									sbMWeapon,
									sbQuiver,
									sbPack,
									sbParts
								});
								NoAddNoRemSortResize_isPoolTrue_isAutoSortTrue_isExpandableTrue = new object[]{
									true, true, true, idSorter, true, sbWearA, sbBowC, hasEmpty, noAddNoRemSortResize, -1
								};
								yield return NoAddNoRemSortResize_isPoolTrue_isAutoSortTrue_isExpandableTrue;
							object[] SG1ThisNoSort_isPoolFalse_isAutoSortFalse;
								IList<ISlottable> SG1ThisNoSort = new List<ISlottable>(new ISlottable[]{
									null,
									sbQuiver,
									sbBowB,
									null,
									sbWearA,
									sbMWeapon,
									null,
									sbBowA,
									null,
									sbWear,
									null,
									sbShield,
									sbPack,
									null,
									null,
									sbParts
								});
								SG1ThisNoSort_isPoolFalse_isAutoSortFalse = new object[]{
									false, false, false, idSorter, true, sbWearA, sbBowC, hasEmpty, SG1ThisNoSort, 4
								};
								yield return SG1ThisNoSort_isPoolFalse_isAutoSortFalse;
							object[] SG1NotThisNoSort_isPoolFalse_isAutoSortFalse;
								SG1NotThisNoSort_isPoolFalse_isAutoSortFalse = new object[]{
									false, false, false, idSorter, false, sbWearA, sbBowC, hasEmpty, SG1ThisNoSort, 4
								};
								yield return SG1NotThisNoSort_isPoolFalse_isAutoSortFalse;
							object[] SG1ThisSortNoResize_isPoolFalse_isAutoSorTrue_isExpandableFalse;
								IList<ISlottable> SG1ThisSortNoResize = new List<ISlottable>(new ISlottable[]{
									sbBowA,
									sbBowB,
									sbWear,
									sbWearA,
									sbShield,
									sbMWeapon,
									sbQuiver,
									sbPack,
									sbParts,
									null,
									null,
									null,
									null,
									null,
									null,
									null
								});
								SG1ThisSortNoResize_isPoolFalse_isAutoSorTrue_isExpandableFalse = new object[]{
									false, true, false, idSorter, true, sbWearA, sbBowC, hasEmpty, SG1ThisSortNoResize, 3
								};
								yield return SG1ThisSortNoResize_isPoolFalse_isAutoSorTrue_isExpandableFalse;
							object[] SG1NotThisSortNoResize_isPoolFalse_isAutoSorTrue_isExpandableFalse;
								SG1NotThisSortNoResize_isPoolFalse_isAutoSorTrue_isExpandableFalse = new object[]{
									false, true, false, idSorter, false, sbWearA, sbBowC, hasEmpty, SG1ThisSortNoResize, 3
								};
								yield return SG1NotThisSortNoResize_isPoolFalse_isAutoSorTrue_isExpandableFalse;
							object[] SG1ThisSortResize_isPoolFalse_isAutoSorTrue_isExpandableTrue;
								IList<ISlottable> SG1ThisSortResize = new List<ISlottable>(new ISlottable[]{
									sbBowA,
									sbBowB,
									sbWear,
									sbWearA,
									sbShield,
									sbMWeapon,
									sbQuiver,
									sbPack,
									sbParts
								});
								SG1ThisSortResize_isPoolFalse_isAutoSorTrue_isExpandableTrue = new object[]{
									false, true, true, idSorter, true, sbWearA, sbBowC, hasEmpty, SG1ThisSortResize, 3
								};
								yield return SG1ThisSortResize_isPoolFalse_isAutoSorTrue_isExpandableTrue;
							object[] SG1NotThisSortResize_isPoolFalse_isAutoSorTrue_isExpandableTrue;
								SG1NotThisSortResize_isPoolFalse_isAutoSorTrue_isExpandableTrue = new object[]{
									false, true, true, idSorter, false, sbWearA, sbBowC, hasEmpty, SG1ThisSortResize, 3
								};
								yield return SG1NotThisSortResize_isPoolFalse_isAutoSorTrue_isExpandableTrue;
						}
					}
				[TestCaseSource(typeof(TryChangeStackableQuantity_MatchAndIsStackableCases))][Category("Methods")]
				public void TryChangeStackableQuantity_Various_ReturnsAccordingly(List<ISlottable> target, InventoryItemInstance delta, bool added, bool expected){
					SlotGroup sg = MakeSG();

					List<ISlottable> actualTarget = new List<ISlottable>(target);

					bool actual = sg.TryChangeStackableQuantity(actualTarget, delta, added);

					Assert.That(actual, Is.EqualTo(expected));
				}
					class TryChangeStackableQuantity_MatchAndIsStackableCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable bowSB = MakeSubSBWithItem(MakeBowInstance(0));
							ISlottable partsSBA = MakeSubSBWithItem(MakePartsInstance(0, 1));
							ISlottable wearSB = MakeSubSBWithItem(MakeWearInstance(0));
							List<ISlottable> target = new List<ISlottable>(new ISlottable[]{
								bowSB, partsSBA, wearSB
							});
							ISlottable partsSBA_changed = MakeSubSBWithItem(MakePartsInstance(0, 3));
							ISlottable partsSBB = MakeSubSBWithItem(MakePartsInstance(1, 2));
							ISlottable bowSB_changed = MakeSubSBWithItem(MakeBowInstance(0));
							yield return new object[]{target, partsSBA_changed.itemInst,true, true};
							yield return new object[]{target, partsSBA_changed.itemInst,false, true};
							yield return new object[]{target, partsSBB.itemInst, true, false};
							yield return new object[]{target, partsSBB.itemInst, false, false};
							yield return new object[]{target, bowSB_changed.itemInst, true, false};
							yield return new object[]{target, bowSB_changed.itemInst, false, false};
						}
					}
				[TestCaseSource(typeof(TryChangeStackableQuantity_AddedCases))][Category("Methods")]
				public void TryChangeStackableQuantity_Added_IncreaseQuantity(List<ISlottable> sbs, InventoryItemInstance addedItem, int indexAtAdded, int expectedQua){
					SlotGroup sg = MakeSG();

					List<ISlottable> actualTarget = new List<ISlottable>(sbs);

					sg.TryChangeStackableQuantity(actualTarget, addedItem, true);

					ISlottable actualAdded = actualTarget[indexAtAdded];
					actualAdded.Received().SetQuantity(expectedQua);
				}
					class TryChangeStackableQuantity_AddedCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable bowSB = MakeSubSBWithItem(MakeBowInstance(0));
							ISlottable partsSBA = MakeSubSBWithItem(MakePartsInstance(0, 1));
								partsSBA.quantity.Returns(1);
							ISlottable partsSBB = MakeSubSBWithItem(MakePartsInstance(1, 5));
								partsSBB.quantity.Returns(5);
							ISlottable wearSB = MakeSubSBWithItem(MakeWearInstance(0));
							List<ISlottable> target = new List<ISlottable>(new ISlottable[]{
								bowSB, partsSBA, wearSB, partsSBB
							});
							ISlottable partsSBA_added = MakeSubSBWithItem(MakePartsInstance(0, 3));
								partsSBA_added.quantity.Returns(3);
							ISlottable partsSBB_added = MakeSubSBWithItem(MakePartsInstance(1, 1));
								partsSBB_added.quantity.Returns(1);
							yield return new object[]{target, partsSBA_added.itemInst, 1, 4};
							yield return new object[]{target, partsSBB_added.itemInst, 3, 6};
						}
					}
				[TestCaseSource(typeof(TryChangeStackableQuantity_DecreaseCases))][Category("Methods")]
				public void TryChangeStackableQuantity_Decrease_DecreasesQuantity(List<ISlottable> sbs, InventoryItemInstance decreasedItem, int indexAtDecreased, int expectedQua){
					SlotGroup sg = MakeSG();
					List<ISlottable> actualTarget = new List<ISlottable>(sbs);

					sg.TryChangeStackableQuantity(actualTarget, decreasedItem, false);

					ISlottable actualDecreased = actualTarget[indexAtDecreased];
					actualDecreased.Received().SetQuantity(expectedQua);
				}
					class TryChangeStackableQuantity_DecreaseCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable bowSB = MakeSubSBWithItem(MakeBowInstance(0));
							ISlottable partsSBA = MakeSubSBWithItem(MakePartsInstance(0, 7));
								partsSBA.quantity.Returns(7);
							ISlottable partsSBB = MakeSubSBWithItem(MakePartsInstance(1, 5));
								partsSBB.quantity.Returns(5);
							ISlottable wearSB = MakeSubSBWithItem(MakeWearInstance(0));
							List<ISlottable> target = new List<ISlottable>(new ISlottable[]{
								bowSB, partsSBA, wearSB, partsSBB
							});
							ISlottable partsSBA_removed = MakeSubSBWithItem(MakePartsInstance(0, 3));
								partsSBA_removed.quantity.Returns(3);
							ISlottable partsSBB_removed = MakeSubSBWithItem(MakePartsInstance(1, 1));
								partsSBB_removed.quantity.Returns(1);
							yield return new object[]{target, partsSBA_removed.itemInst, 1, 4};
							yield return new object[]{target, partsSBB_removed.itemInst, 3, 4};
						}
					}
				[TestCaseSource(typeof(TryChangeStackableQuantity_DecreasedDownToZeroCases))][Category("Methods")]
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
							ISlottable bowSB = MakeSubSBWithItem(MakeBowInstance(0));
							ISlottable partsSBA = MakeSubSBWithItem(MakePartsInstance(0, 1));
								partsSBA.quantity.Returns(1);
							ISlottable partsSBB = MakeSubSBWithItem(MakePartsInstance(1, 5));
								partsSBB.quantity.Returns(5);
							ISlottable wearSB = MakeSubSBWithItem(MakeWearInstance(0));
							List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{
								bowSB, partsSBA, wearSB, partsSBB
							});
							ISlottable partsSBA_removed = MakeSubSBWithItem(MakePartsInstance(0, 1));
								partsSBA_removed.quantity.Returns(1);
							ISlottable partsSBB_removed = MakeSubSBWithItem(MakePartsInstance(1, 5));
								partsSBB_removed.quantity.Returns(5);
							yield return new object[]{sbs, partsSBA_removed.itemInst, 1};
							yield return new object[]{sbs, partsSBB_removed.itemInst, 3};
						}
					}
				[TestCaseSource(typeof(AddAndUpdateSBs_VariousCases))][Category("Methods")]
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
					Assert.That(sg.toList, Is.EqualTo(expectedSBs));
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
					Assert.That(sg.newSBs, Is.EqualTo(expectedNewSBs));
				}
					class AddAndUpdateSBs_VariousCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							/* SBs */
								ISlottable bowSB = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable wearSB = MakeSubSBWithItem(MakeWearInstance(0));
								ISlottable shieldSB = MakeSubSBWithItem(MakeShieldInstance(0));
								ISlottable mWeaponSB = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
								ISlottable partsSBA = MakeSubSBWithItem(MakePartsInstance(0, 1));
									partsSBA.quantity.Returns(1);
								ISlottable partsSBB = MakeSubSBWithItem(MakePartsInstance(1, 2));
									partsSBB.quantity.Returns(2);
								ISlottable bowSBB_added = MakeSubSBWithItem(MakeBowInstance(1));
								ISlottable quiverSB_added = MakeSubSBWithItem(MakeQuiverInstance(1));
								ISlottable partsSBA_added = MakeSubSBWithItem(MakePartsInstance(0, 6));
									partsSBA_added.quantity.Returns(6);
								ISlottable partsSBB_added = MakeSubSBWithItem(MakePartsInstance(1, 3));
									partsSBB_added.quantity.Returns(3);
							SGSorter idSorter = new SGItemIDSorter();
							List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{
								partsSBB,
								wearSB,
								null,
								mWeaponSB,
								null,
								partsSBA,
								null,
								shieldSB,
								bowSB,
								null,
								null
							});
							object[] addedNoSort_isAutoSortFalse;
								List<InventoryItemInstance> case1Added = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									bowSBB_added.itemInst,
									quiverSB_added.itemInst,
									partsSBA_added.itemInst,
									partsSBB_added.itemInst 
								});
								List<ISlottable> case1ExpSBs = new List<ISlottable>(new ISlottable[]{
									partsSBB,
									wearSB,
									null,
									mWeaponSB,
									null,
									partsSBA,
									null,
									shieldSB,
									bowSB,
									null,
									null,
									bowSBB_added,
									quiverSB_added,
								});
								List<ISlottable> case1ExpNewSBs = new List<ISlottable>(new ISlottable[]{
									partsSBB,
									wearSB,
									bowSBB_added,
									mWeaponSB,
									quiverSB_added,
									partsSBA,
									null,
									shieldSB,
									bowSB,
									null,
									null,
								});
								Dictionary<InventoryItemInstance, int> case1SBsIDDict = new Dictionary<InventoryItemInstance, int>();
									case1SBsIDDict.Add(bowSBB_added.itemInst, 11);
									case1SBsIDDict.Add(quiverSB_added.itemInst, 12);
									case1SBsIDDict.Add(partsSBA_added.itemInst, 5);
									case1SBsIDDict.Add(partsSBB_added.itemInst, 0);
								Dictionary<InventoryItemInstance, int> case1NewSBsIDDict = new Dictionary<InventoryItemInstance, int>();
									case1NewSBsIDDict.Add(bowSBB_added.itemInst, 2);
									case1NewSBsIDDict.Add(quiverSB_added.itemInst, 4);
								Dictionary<InventoryItemInstance, int> case1QuaDict = new Dictionary<InventoryItemInstance, int>();
									case1QuaDict.Add(partsSBA_added.itemInst, 7);
									case1QuaDict.Add(partsSBB_added.itemInst, 5);
								addedNoSort_isAutoSortFalse = new object[]{
									false, false, idSorter, sbs, case1Added, case1ExpSBs, case1ExpNewSBs, case1SBsIDDict, case1NewSBsIDDict, case1QuaDict
								};
								yield return addedNoSort_isAutoSortFalse;
							object[] addedSortNoResize_isAutoSortTrue_isExpandableFalse;
								List<ISlottable> case2ExpNewSBs = new List<ISlottable>(new ISlottable[]{
									bowSB,
									bowSBB_added,
									wearSB,
									shieldSB,
									mWeaponSB,
									quiverSB_added,
									partsSBA,
									partsSBB,
									null,
									null,
									null
								});
								Dictionary<InventoryItemInstance, int> case2NewSBsIDDict = new Dictionary<InventoryItemInstance, int>();
									case2NewSBsIDDict.Add(bowSBB_added.itemInst, 1);
									case2NewSBsIDDict.Add(quiverSB_added.itemInst, 5);
								addedSortNoResize_isAutoSortTrue_isExpandableFalse = new object[]{
									true, false, idSorter, sbs, case1Added, case1ExpSBs, case2ExpNewSBs, case1SBsIDDict, case2NewSBsIDDict, case1QuaDict
								};
								yield return addedSortNoResize_isAutoSortTrue_isExpandableFalse;
							object[] addedSortResize_isAutoSortTrue_isExpandableTrue;
								List<ISlottable> case3ExpNewSBs = new List<ISlottable>(new ISlottable[]{
									bowSB,
									bowSBB_added,
									wearSB,
									shieldSB,
									mWeaponSB,
									quiverSB_added,
									partsSBA,
									partsSBB
								});
								addedSortResize_isAutoSortTrue_isExpandableTrue = new object[]{
									true, true, idSorter, sbs, case1Added, case1ExpSBs, case3ExpNewSBs, case1SBsIDDict, case2NewSBsIDDict, case1QuaDict
								};
								yield return addedSortResize_isAutoSortTrue_isExpandableTrue;
						}
					}
				[TestCaseSource(typeof(RemoveAndUpdateSGsCases))][Category("Methods")]
				public void RemoveAndUpdateSGs_Various_DoesNotChangeSBsAndSetsNewSBsAccordingly(
					bool isAutoSort, 
					bool isExpandable, 
					SGSorter sorter, 
					List<ISlottable> sbs, 
					List<InventoryItemInstance> removed, 
					List<ISlottable> expectedNewSBs, 
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

						Assert.That(sg.toList, Is.EqualTo(sbs));
						Assert.That(sg.newSBs, Is.EqualTo(expectedNewSBs));
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
							SGItemIDSorter sorter = new SGItemIDSorter();
							/* SBs */
								ISlottable sbBowA = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable sbBowB = MakeSubSBWithItem(MakeBowInstance(1));
								ISlottable sbWear = MakeSubSBWithItem(MakeWearInstance(0));
								ISlottable sbShield = MakeSubSBWithItem(MakeShieldInstance(0));
								ISlottable sbMWeapon = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
								ISlottable sbQuiver = MakeSubSBWithItem(MakeQuiverInstance(0));
								ISlottable sbPack = MakeSubSBWithItem(MakePackInstance(0));
								ISlottable sbPartsA = MakeSubSBWithItem(MakePartsInstance(0, 5));
									sbPartsA.quantity.Returns(5);
								ISlottable sbPartsB = MakeSubSBWithItem(MakePartsInstance(1, 4));
									sbPartsB.quantity.Returns(4);
								ISlottable sbPartsA_rem = MakeSubSBWithItem(MakePartsInstance(0, 3));
									sbPartsA_rem.quantity.Returns(3);
								ISlottable sbPartsB_rem = MakeSubSBWithItem(MakePartsInstance(1, 4));
									sbPartsB_rem.quantity.Returns(4);
							List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{
								sbShield,
								null,
								sbMWeapon,
								null,
								sbBowB,
								sbPack,
								null,
								sbPartsB,
								sbQuiver,
								sbPartsA,
								null,
								sbBowA,
								null,
								sbWear
							});
							List<InventoryItemInstance> removed = new List<InventoryItemInstance>(new InventoryItemInstance[]{
								sbQuiver.itemInst,
								sbBowA.itemInst,
								sbPartsA_rem.itemInst,
								sbPartsB_rem.itemInst
							});
							List<ISlottable> noSort = new List<ISlottable>(new ISlottable[]{
								sbShield,
								null,
								sbMWeapon,
								null,
								sbBowB,
								sbPack,
								null,
								null,
								null,
								sbPartsA,
								null,
								null,
								null,
								sbWear
							});
							List<ISlottable> sortNoResize = new List<ISlottable>(new ISlottable[]{
								sbBowB,
								sbWear,
								sbShield,
								sbMWeapon,
								sbPack,
								sbPartsA,
								null,
								null,
								null,
								null,
								null,
								null,
								null,
								null
							});
							List<ISlottable> sortResize = new List<ISlottable>(new ISlottable[]{
								sbBowB,
								sbWear,
								sbShield,
								sbMWeapon,
								sbPack,
								sbPartsA
							});
							Dictionary<InventoryItemInstance, int> removedQuantDict = new Dictionary<InventoryItemInstance, int>();
								removedQuantDict.Add(sbPartsA_rem.itemInst, 2);
							object[] noSort_isAutoSortFalse;
								noSort_isAutoSortFalse = new object[]{
									false, false, sorter, sbs, removed, noSort, removedQuantDict 
								};
								yield return noSort_isAutoSortFalse;
							object sortNoResize_isAutoSortTrue_isExpandableFalse;
								sortNoResize_isAutoSortTrue_isExpandableFalse = new object[]{
									true, false, sorter, sbs, removed, sortNoResize, removedQuantDict
								};
								yield return sortNoResize_isAutoSortTrue_isExpandableFalse;
							object sortResize_isAutoSortTrue_isExpandableTrue;
								sortResize_isAutoSortTrue_isExpandableTrue = new object[]{
									true, true, sorter, sbs, removed, sortResize, removedQuantDict
								};
								yield return sortResize_isAutoSortTrue_isExpandableTrue;
						}
					}
				[TestCase(1)]
				[TestCase(10)]
				[Category("Methods")]
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
				[Category("Methods")]
				public void InitSlots_InitSlotsCountZero_SetsSlotsByItemsCount(int itemsCount){
					SlotGroup sg = MakeSG();
						sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), new SGItemIDSorter() , 0);
					
					sg.InitSlots(new List<SlottableItem>(new SlottableItem[itemsCount]));

					int actual = sg.slots.Count;

					Assert.That(actual, Is.EqualTo(itemsCount));
					foreach(Slot slot in sg.slots)
						Assert.That(slot.sb, Is.Null);
				}
				[TestCaseSource(typeof(InitSBs_SlotsNotEnoughCases))][Category("Methods")]
				public void InitSBs_SlotsNotEnough_RemoveNonFittableItems(List<Slot> slots, List<SlottableItem> items, List<SlottableItem> expected){
					SlotGroup sg = MakeSG();
						sg.SetSlots(slots);
					
					sg.InitSBs(items);

					Assert.That(items, Is.EqualTo(expected));
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
				[TestCaseSource(typeof(InitSBs_WhenCalledCases))][Category("Methods")]
				public void InitSBs_WhenCalled_CreatesAndSetsSBsInSlots(ISlotSystemManager ssm ,List<Slot> slots, List<SlottableItem> items, List<CreatedNewSBData> expected){
					SlotGroup sg = MakeSG();
						sg.SetSSM(ssm);
						sg.SetSlots(slots);
					
					sg.InitSBs(items);

					for(int i = 0; i< slots.Count; i++){
						ISlottable sb = slots[i].sb;
						CreatedNewSBData actual = new CreatedNewSBData(sb.itemInst, sb.ssm, Slottable.sbDeactivatedState, Slottable.unequippedState);
						bool equality = actual.Equals(expected[i]);
						Assert.That(equality, Is.True);
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
							SSEState deaState = Slottable.sbDeactivatedState;
							SSEState uneState = Slottable.unequippedState;
							List<CreatedNewSBData> expected;
								CreatedNewSBData bowAData = new CreatedNewSBData(bowA, ssm, deaState, uneState);
								CreatedNewSBData bowBData = new CreatedNewSBData(bowB, ssm, deaState, uneState);
								CreatedNewSBData bowCData = new CreatedNewSBData(bowC, ssm, deaState, uneState);
								CreatedNewSBData bowDData = new CreatedNewSBData(bowD, ssm, deaState, uneState);
								expected = new List<CreatedNewSBData>(new CreatedNewSBData[]{
									bowAData, bowBData, bowCData, bowDData
								});
							yield return new object[]{
								ssm, slots, items, expected
							};
						}
					}
				[Test][Category("Methods")]
				public void empty(){}
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
