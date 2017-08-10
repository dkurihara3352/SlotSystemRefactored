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
	namespace SlotGroupTests{
		[TestFixture]
		public class SGTransactionHandlerTests: SlotSystemTest {
			[TestCaseSource(typeof(OnCompleteSlotMovementsCases))]
			public void OnCompleteSlotMovements_SBIsToBeRemoved_CallsSBDestroy(IEnumerable<ISlottable> sbs, IEnumerable<ISlottable> xRemoved, IEnumerable<ISlottable> xNotRemoved){
				SGTransactionHandler sgTAHandler;
					ISlotGroup stubSG = MakeSubSG();
					stubSG.slottables.Returns(sbs);
					sgTAHandler = new SGTransactionHandler(stubSG, MakeSubTAM());
				
				sgTAHandler.OnCompleteSlotMovements();

				foreach(var sb in xRemoved)
					sb.DidNotReceive().Destroy();
				foreach(var sb in xNotRemoved)
					sb.Received().Destroy();
			}
				class OnCompleteSlotMovementsCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						ISlottable rSB_0 = MakeSubSB();
						ISlottable rSB_1 = MakeSubSB();
						ISlottable rSB_2 = MakeSubSB();
						ISlottable nrSB_0 = MakeSubSB();
						ISlottable nrSB_1 = MakeSubSB();
						ISlottable nrSB_2 = MakeSubSB();
						rSB_0.isToBeRemoved.Returns(true);
						rSB_1.isToBeRemoved.Returns(true);
						rSB_2.isToBeRemoved.Returns(true);
						nrSB_0.isToBeRemoved.Returns(false);
						nrSB_1.isToBeRemoved.Returns(false);
						nrSB_2.isToBeRemoved.Returns(false);
						IEnumerable<ISlottable> all = new ISlottable[]{
							rSB_0,
							rSB_1,
							rSB_2,
							nrSB_0,
							nrSB_1,
							nrSB_2
						};
						IEnumerable<ISlottable> xRemoved = new ISlottable[]{
							rSB_0,
							rSB_1,
							rSB_2
						};
						IEnumerable<ISlottable> xNotRemoved = new ISlottable[]{
							nrSB_0,
							nrSB_1,
							nrSB_2
						};
						yield return new object[]{all, xRemoved, xNotRemoved};
					}
				}
			[TestCaseSource(typeof(OnCompleteSlotMovementsV2Cases))]
			public void OnCompleteSlotMovements_SBNotIsToBeRemoved_PutThemInNewSlot(List<ISlottable> sbs, List<ISlottable> expected){
				SGTransactionHandler sgTAHandler;
					ISlotGroup stubSG = MakeSubSG();
						List<Slot> newSlots = CreateSlots(sbs.Count);
						stubSG.newSlots.Returns(newSlots);
					sgTAHandler = new SGTransactionHandler(stubSG, MakeSubTAM());
				
				sgTAHandler.OnCompleteSlotMovements();

				List<ISlottable> actual = CreateSBsFromSlots(newSlots);
				bool equality = actual.MemberEquals(expected);
				Assert.That(equality, Is.Not.True);
			}
				class OnCompleteSlotMovementsV2Cases: IEnumerable{
					public IEnumerator GetEnumerator(){
						ISlottable sb_0 = MakeSubSB();
						ISlottable sb_1 = MakeSubSB();
						ISlottable sb_2 = MakeSubSB();
						ISlottable sb_3 = MakeSubSB();
						ISlottable sb_4 = MakeSubSB();
						ISlottable sb_5 = MakeSubSB();
						sb_0.newSlotID.Returns(0);
						sb_1.newSlotID.Returns(1);
						sb_2.newSlotID.Returns(2);
						sb_3.newSlotID.Returns(3);
						sb_4.newSlotID.Returns(4);
						sb_5.newSlotID.Returns(5);
						List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{
							sb_4,
							sb_3,
							sb_2,
							sb_5,
							sb_1,
							sb_0
						});
						List<ISlottable> expected = new List<ISlottable>(new ISlottable[]{
							sb_0,
							sb_1,
							sb_2,
							sb_3,
							sb_4,
							sb_5
						});
						yield return new object[]{sbs, expected};
					}
				}
			List<ISlottable> CreateSBsFromSlots(List<Slot> slots){
				List<ISlottable> result = new List<ISlottable>();
				foreach(var slot in slots)
					result.Add(slot.sb);
				return result;
			}
			[TestCaseSource(typeof(OnCompleteSlotMovementsV3Cases))]
			public void OnCompleteSlotMovements_Always_SyncSBsToSlotsWithNewSlotIDs(List<ISlottable> sbs, List<ISlottable> expected){
				SGTransactionHandler sgTAHandler;
					ISlotGroup stubSG = MakeSubSG();
						stubSG.slottables.Returns(sbs);
					List<Slot> newSlots = CreateSlots(sbs.Count);
						stubSG.newSlots.Returns(newSlots);
					sgTAHandler = new SGTransactionHandler(stubSG, MakeSubTAM());
				
				sgTAHandler.OnCompleteSlotMovements();

				List<ISlottable> actual = CreateSBsFromSlots(newSlots);
				bool equality = actual.MemberEquals(expected);
				Assert.That(equality, Is.Not.True);
				foreach(var sb in actual)
					if(sb != null)
						((ISlottable)sb).Received().SetSlotID(actual.IndexOf((ISlottable)sb));
			}
				class OnCompleteSlotMovementsV3Cases: IEnumerable{
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
			
			[TestCaseSource(typeof(ReorderedNewSBsCases))]
			public void ReorderedNewSBs_Always_CreateAndReturnsReorderdSBs(ISlottable picked, ISlottable target, List<ISlottable> sbs, IEnumerable<ISlottable> expected){
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						ITransactionCache tac = Substitute.For<ITransactionCache>();
						tac.pickedSB.Returns(picked);
						tac.targetSB.Returns(target);
					sg.taCache.Returns(tac);
					sg.slottables.Returns(sbs);
				sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
				
				List<ISlottable> actual = sgTAHandler.ReorderedNewSBs();

				bool equality = actual.MemberEquals(expected);
				Assert.That(equality, Is.Not.True);
			}
				class ReorderedNewSBsCases: IEnumerable{
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
							IEnumerable<ISlottable> exp_0 = new ISlottable[]{
								sb0_0,
								sb1_0,
								sb4_0,
								sb2_0,
								sb3_0
							};
							case0 = new object[]{picked_0, hovered_0, sbs_0, exp_0};
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
							IEnumerable<ISlottable> exp_1 = new ISlottable[]{
								sb1_1,
								sb2_1,
								sb3_1,
								sb4_1,
								sb0_1
							};
							case1 = new object[]{picked_1, hovered_1, sbs_1, exp_1};
							yield return case1;
					}
				}				
			[TestCaseSource(typeof(SortedNewSBsCases))]
			public void SortAndUpdateSBs_SGIsExpandable_CreatesAndReturnsSortedAndResizedSBs(SGSorter sorter, List<ISlottable> sbs, List<ISlottable> expected){
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						SorterHandler sorterHandler = new SorterHandler();
						sorterHandler.SetSorter(sorter);
						List<ISlottable> sortedAndResizedSBs = sorterHandler.GetSortedSBsWithResize(sbs);
						sg.GetSortedSBsWithResize(sbs).Returns(sortedAndResizedSBs);
						sg.slottables.Returns(sbs);
						sg.isExpandable.Returns(true);
					sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
				
				List<ISlottable> actual = sgTAHandler.SortedNewSBs();

				List<ISlottable> thisExpected = TrimmedSBs(expected);
				bool equality = actual.MemberEquals(expected);
				Assert.That(equality, Is.Not.True);
			}
			List<ISlottable> TrimmedSBs(IEnumerable<ISlottable> source){
				List<ISlottable> result = new List<ISlottable>();
				foreach(var sb in source)
					if(sb != null)
						result.Add(sb);
				return result;
			}
				class SortedNewSBsCases: IEnumerable{
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
			[TestCaseSource(typeof(SortedNewSBsCases))]
			public void SortedNewSBs_SGIsNotExpandable_CreatesAndReturnsSortedAndNonResizedSBs(SGSorter sorter, List<ISlottable> sbs, List<ISlottable> exp){
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						SorterHandler sorterHandler = new SorterHandler();
						sorterHandler.SetSorter(sorter);
						List<ISlottable> sortedAndResizedSBs = sorterHandler.GetSortedSBsWithResize(sbs);
						sg.GetSortedSBsWithResize(sbs).Returns(sortedAndResizedSBs);
						sg.slottables.Returns(sbs);
						sg.isExpandable.Returns(false);
					sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
				
				List<ISlottable> actual = sgTAHandler.SortedNewSBs();

				bool equality = actual.MemberEquals(exp);
				Assert.That(equality, Is.Not.True);
			}
			[Test]
			public void GetAddedForFill_SSMSG1NotThis_ReturnsSSMPickedSB(){
				SlotGroup sg = MakeSGInitWithSubs();
					ISlottable pickedSB = MakeSubSB();
					sg.taCache.pickedSB.Returns(pickedSB);
					ISlotGroup otherSG = MakeSubSG();
					sg.sgHandler.sg1.Returns(otherSG);
				
				ISlottable actual = sg.GetAddedForFill();

				Assert.That(actual, Is.SameAs(pickedSB));
			}
			[Test]
			public void GetAddedForFill_SSMSG1IsThis_ReturnsNull(){
				SlotGroup sg = MakeSGInitWithSubs();
						ISlottable pickedSB = MakeSubSB();
						sg.taCache.pickedSB.Returns(pickedSB);
						sg.sgHandler.sg1.Returns(sg);
				
				ISlottable actual = sg.GetAddedForFill();

				Assert.That(actual, Is.Null);
			}
			[Test]
			public void GetRemovedForFill_SSMSG1NotThis_ReturnsNull(){
				SlotGroup sg = MakeSGInitWithSubs();
					ISlottable pickedSB = MakeSubSB();
					sg.taCache.pickedSB.Returns(pickedSB);
					ISlotGroup otherSG = MakeSubSG();
					sg.sgHandler.sg1.Returns(otherSG);
				
				ISlottable actual = sg.GetRemovedForFill();

				Assert.That(actual, Is.Null);
			}
			[Test]
			public void GetRemovedForFill_SSMSG1IsThis_RetunsSSMPickedSB(){
				SlotGroup sg = MakeSGInitWithSubs();
					ISlottable pickedSB = MakeSubSB();
					sg.taCache.pickedSB.Returns(pickedSB);
					sg.sgHandler.sg1.Returns(sg);
				
				ISlottable actual = sg.GetRemovedForFill();

				Assert.That(actual, Is.SameAs(pickedSB));
			}
			[TestCaseSource(typeof(CreateNewSBAndFillCases))]
			public void CreateNewSBAndFill_Always_UpdateList(List<ISlottable> list, ISlottable added, int addedIndex){
				SlotGroup sg = MakeSGInitWithSubs();
					sg.ssm.FindParent(Arg.Any<ISlottable>()).Returns(sg);
				List<ISlottable> targetList = new List<ISlottable>(list);
				
				sg.CreateNewSBAndFill(added.item, targetList);

				ISlottable actualAdded = targetList[addedIndex];
				Assert.That(actualAdded.item, Is.SameAs(added.item));
				Assert.That(actualAdded.ssm, Is.SameAs(sg.ssm));
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
								added_0.item.Returns(bow_0);
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
								added_1.item.Returns(bow_1);
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
								added_2.item.Returns(bow_2);
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
			public void NullifyIndexOf_Always_FindByItemAndReplaceWithNull(List<ISlottable> list, InventoryItemInstance item, IEnumerable<ISlottable> expected){
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
								bowSB_0.item.Returns(bow_0);
							ISlottable wearSB_0 = MakeSubSB();
								WearInstance wear_0 = MakeWearInstance(0);
								wearSB_0.item.Returns(wear_0);
							ISlottable shieldSB_0 = MakeSubSB();
								ShieldInstance shield_0 = MakeShieldInstance(0);
								shieldSB_0.item.Returns(shield_0);
							ISlottable mWeaponSB_0 = MakeSubSB();
								MeleeWeaponInstance mWeapon_0 = MakeMeleeWeaponInstance(0);
								mWeaponSB_0.item.Returns(mWeapon_0);
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
								bowSB_1.item.Returns(bow_1);
							ISlottable wearSB_1 = MakeSubSB();
								WearInstance wear_1 = MakeWearInstance(0);
								wearSB_1.item.Returns(wear_1);
							ISlottable shieldSB_1 = MakeSubSB();
								ShieldInstance shield_1 = MakeShieldInstance(0);
								shieldSB_1.item.Returns(shield_1);
							ISlottable mWeaponSB_1 = MakeSubSB();
								MeleeWeaponInstance mWeapon_1 = MakeMeleeWeaponInstance(0);
								mWeaponSB_1.item.Returns(mWeapon_1);
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
				SlotGroup sg = MakeSGInitWithSubs();
					sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), Substitute.For<SGSorter>(), isExpandable?0: 10);
					ISlotGroup otherSG = MakeSubSG();
						sg.sgHandler.sg1.Returns(otherSG);
						ISlottable pickedSB = MakeSubSB();
							BowInstance bow = MakeBowInstance(0);
							pickedSB.item.Returns(bow);
							sg.taCache.pickedSB.Returns(pickedSB);
							ISlotSystemBundle poolBundle = MakeSubBundle();
								poolBundle.ContainsInHierarchy(sg).Returns(false);
							sg.ssm.poolBundle.Returns(poolBundle);
						sg.ssm.FindParent(Arg.Any<ISlottable>()).Returns(sg);
					sg.SetSBHandler(new SBHandler());
					sg.SetSBs(sbs);
					sg.ToggleAutoSort(isAutoSort);
					sg.SetNewSBs(new List<ISlottable>());
				
				sg.FillAndUpdateSBs();

				ISlottable added = sg.toList[sbs.Count];
				Assert.That(added, Is.Not.Null);
				Assert.That(added.ssm, Is.SameAs(sg.ssm));
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
				SlotGroup sg = MakeSGInitWithSubs();
					ISlotGroup otherSG = MakeSubSG();
						if(added)
							sg.sgHandler.sg1.Returns(otherSG);
						else
							sg.sgHandler.sg1.Returns(sg);
						sg.taCache.pickedSB.Returns(pickedSB);
							ISlotSystemBundle poolBundle = MakeSubBundle();
								if(isPool)
									poolBundle.ContainsInHierarchy(sg).Returns(true);
								else
									poolBundle.ContainsInHierarchy(sg).Returns(false);
						sg.ssm.poolBundle.Returns(poolBundle);
						sg.ssm.FindParent(Arg.Any<ISlottable>()).Returns(sg);
					sg.SetSorterHandler(new SorterHandler());
					sg.SetFilterHandler(new FilterHandler());
				sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), sorter, isExpandable?0: 10);
				sg.SetSBHandler(new SBHandler());
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
				SlotGroup sg = MakeSGInitWithSubs();
					ISlottable targetSB = MakeSubSB();
					sg.taCache.targetSB.Returns(targetSB);
					sg.sgHandler.sg1.Returns(sg);

				ISlottable actual = sg.GetAddedForSwap();

				Assert.That(actual, Is.SameAs(targetSB));
				}
			[Test]
			public void GetAddedForSwap_SSMSG1NotThis_ReturnsSSMPickedSB(){
				SlotGroup sg = MakeSGInitWithSubs();
					ISlottable pickedSB = MakeSubSB();
					sg.taCache.pickedSB.Returns(pickedSB);
					ISlotGroup otherSG = MakeSubSG();
					sg.sgHandler.sg1.Returns(otherSG);

				ISlottable actual = sg.GetAddedForSwap();

				Assert.That(actual, Is.SameAs(pickedSB));
				}
			[Test]
			public void GetRemovedForSwap_SSMSG1This_ReturnsSSMPickedSB(){
				SlotGroup sg = MakeSGInitWithSubs();
					ISlottable pickedSB = MakeSubSB();
					sg.taCache.pickedSB.Returns(pickedSB);
					sg.sgHandler.sg1.Returns(sg);

				ISlottable actual = sg.GetRemovedForSwap();

				Assert.That(actual, Is.SameAs(pickedSB));
				}
			[Test]
			public void GetRemovedForSwap_SSMSG1NotThis_ReturnsSSMTargetSB(){
				SlotGroup sg = MakeSGInitWithSubs();
					ISlottable targetSB = MakeSubSB();
					sg.taCache.targetSB.Returns(targetSB);
					ISlotGroup otherSG = MakeSubSG();
					sg.sgHandler.sg1.Returns(otherSG);

				ISlottable actual = sg.GetRemovedForSwap();

				Assert.That(actual, Is.SameAs(targetSB));
				}
			[Test]
			public void SwapAndUpdateSBs_SSMSG1This_CreateAndAddNewSBFromSSMTargetSBToTheEndOfSBs(){
				SlotGroup sg = MakeSGInitWithSubs();
						ISlottable targetSB = MakeSubSB();
							BowInstance bow = MakeBowInstance(0);
							targetSB.item.Returns(bow);
						ISlottable pickedSB = MakeSubSB();
						List<ISlottable> sbs = CreateSBs(3);
							sbs.Add(pickedSB);
						sg.taCache.targetSB.Returns(targetSB);
						sg.taCache.pickedSB.Returns(pickedSB);
						sg.sgHandler.sg1.Returns(sg);
					sg.SetSBs(sbs);
					sg.ToggleAutoSort(false);
				
				sg.SwapAndUpdateSBs();

				ISlottable actualAdded = sg.toList[sg.toList.Count -1];
				Assert.That(actualAdded.item, Is.SameAs(bow));
				Assert.That(actualAdded.ssm, Is.SameAs(sg.ssm));
				Assert.That(actualAdded.isDefocused, Is.True);
				Assert.That(actualAdded.isUnequipped, Is.True);
				}
			[Test]
			public void SwapAndUpdateSBs_SSMSG1NotThis_CreateAndAddNewSBFromSSMPickedSBToTheEndOfSBs(){
				SlotGroup sg = MakeSGInitWithSubs();
						ISlottable targetSB = MakeSubSB();
						List<ISlottable> sbs = CreateSBs(3);
							sbs.Add(targetSB);
						ISlottable pickedSB = MakeSubSB();
							BowInstance bow = MakeBowInstance(0);
							pickedSB.item.Returns(bow);
						sg.taCache.targetSB.Returns(targetSB);
						sg.taCache.pickedSB.Returns(pickedSB);
						ISlotGroup otherSG = MakeSubSG();
						sg.sgHandler.sg1.Returns(otherSG);
					sg.SetSBs(sbs);
					sg.ToggleAutoSort(false);
				
				sg.SwapAndUpdateSBs();	

				ISlottable actualAdded = sg.toList[sg.toList.Count -1];
				Assert.That(actualAdded.item, Is.SameAs(bow));
				Assert.That(actualAdded.ssm, Is.SameAs(sg.ssm));
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
				SlotGroup sg = MakeSGInitWithSubs();
					sg.SetSorterHandler(new SorterHandler());
					sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), sorter, isExpandable?0: 10);
						ISlottable targetSB = MakeSubSB();
						sg.taCache.targetSB.Returns(targetSB);
						ISlotGroup otherSG = MakeSubSG();
						if(sg1This){
							sg.sgHandler.sg1.Returns(sg);
							sg.taCache.targetSB.Returns(added);
							sg.taCache.pickedSB.Returns(removed);
						}
						else{
							sg.sgHandler.sg1.Returns(otherSG);
							sg.taCache.targetSB.Returns(removed);
							sg.taCache.pickedSB.Returns(added);
						}
						ISlotSystemBundle pBun = MakeSubBundle();
							pBun.ContainsInHierarchy(sg).Returns(isPool);
						sg.ssm.poolBundle.Returns(pBun);
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
					SlotGroup sg = MakeSGInitWithSubs();
					sg.SetSorterHandler(new SorterHandler());
					sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), sorter, isExpandable?0: 20);
					sg.taCache.moved.Returns(added);
					sg.ssm.FindParent(Arg.Any<ISlottable>()).Returns(sg);
					sg.SetSBs(sbs);
					sg.ToggleAutoSort(isAutoSort);
					
					sg.AddAndUpdateSBs();
					
					foreach(ISlottable sb in sg){
						if(sb != null){
							int id;
							if(sbsAddedIDDict.TryGetValue(sb.item, out id))
								expectedSBs[id] = sb;
						}
					}
					bool equality = sg.toList.MemberEquals(expectedSBs);
					Assert.That(equality, Is.True);
					foreach(ISlottable sb in sg){
						if(sb != null){
							int qua;
							if(quantDict.TryGetValue(sb.item, out qua)){
								sb.Received().SetQuantity(qua);
							}
						}
					}
					foreach(ISlottable sb in sg.newSBs){
						if(sb != null){
							int id;
							if(newSBsAddedIDDict.TryGetValue(sb.item, out id)){
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
								bow1SB_a_0.item,
								quiverSB_a_0.item,
								partsSB_a_0.item,
								parts1SB_a_0.item 
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
								sbsIDs_0.Add(bow1SB_a_0.item, 11);
								sbsIDs_0.Add(quiverSB_a_0.item, 12);
								sbsIDs_0.Add(partsSB_a_0.item, 5);
								sbsIDs_0.Add(parts1SB_a_0.item, 0);
							Dictionary<InventoryItemInstance, int> newSBsIDs_0 = new Dictionary<InventoryItemInstance, int>();
								newSBsIDs_0.Add(bow1SB_a_0.item, 2);
								newSBsIDs_0.Add(quiverSB_a_0.item, 4);
							Dictionary<InventoryItemInstance, int> quants_0 = new Dictionary<InventoryItemInstance, int>();
								quants_0.Add(partsSB_a_0.item, 7);
								quants_0.Add(parts1SB_a_0.item, 5);
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
								bow1SB_a_1.item,
								quiverSB_a_1.item,
								partsSB_a_1.item,
								parts1SB_a_1.item 
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
								sbsIDs_1.Add(bow1SB_a_1.item, 11);
								sbsIDs_1.Add(quiverSB_a_1.item, 12);
								sbsIDs_1.Add(partsSB_a_1.item, 5);
								sbsIDs_1.Add(parts1SB_a_1.item, 0);
							Dictionary<InventoryItemInstance, int> quants_1 = new Dictionary<InventoryItemInstance, int>();
								quants_1.Add(partsSB_a_1.item, 7);
								quants_1.Add(parts1SB_a_1.item, 5);
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
								newSBsIDs_1.Add(bow1SB_a_1.item, 1);
								newSBsIDs_1.Add(quiverSB_a_1.item, 5);
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
								bow1SB_a_2.item,
								quiverSB_a_2.item,
								partsSB_a_2.item,
								parts1SB_a_2.item 
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
								sbsIDs_2.Add(bow1SB_a_2.item, 11);
								sbsIDs_2.Add(quiverSB_a_2.item, 12);
								sbsIDs_2.Add(partsSB_a_2.item, 5);
								sbsIDs_2.Add(parts1SB_a_2.item, 0);
							Dictionary<InventoryItemInstance, int> quants_2 = new Dictionary<InventoryItemInstance, int>();
								quants_2.Add(partsSB_a_2.item, 7);
								quants_2.Add(parts1SB_a_2.item, 5);
							Dictionary<InventoryItemInstance, int> newSBsIDs_2 = new Dictionary<InventoryItemInstance, int>();
								newSBsIDs_2.Add(bow1SB_a_2.item, 1);
								newSBsIDs_2.Add(quiverSB_a_2.item, 5);
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
					SlotGroup sg = MakeSGInitWithSubs();
						sg.SetSorterHandler(new SorterHandler());
						sg.InspectorSetUp(new GenericInventory(), new SGNullFilter(), sorter, isExpandable?0: 20);
						sg.taCache.moved.Returns(removed);
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
								if(removedQuantDict.TryGetValue(sb.item, out quantity)){
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
								quiverSB_0.item,
								bowSB_0.item,
								partsSB_r_0.item,
								parts1SB_r_0.item
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
								removedQuantDict_0.Add(partsSB_r_0.item, 2);
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
								quiverSB_1.item,
								bowSB_1.item,
								partsSB_r_1.item,
								parts1SB_r_1.item
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
								removedQuantDict_1.Add(partsSB_r_1.item, 2);
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
								quiverSB_2.item,
								bowSB_2.item,
								partsSB_r_2.item,
								parts1SB_r_2.item
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
								removedQuantDict_2.Add(partsSB_r_2.item, 2);
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
		/*	helper */
			static ISlottable MakeSubSBWithItemAndSG(InventoryItemInstance item, SlotGroup sg){
				ISlottable sb = MakeSubSB();
					sb.item.Returns(item);
					sb.sg.Returns(sg);
				return sb;
			}
			static ISlottable MakeSubSBWithItem(InventoryItemInstance item){
				ISlottable sb = MakeSubSB();
					sb.item.Returns(item);
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
