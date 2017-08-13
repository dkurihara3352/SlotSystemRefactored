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
			SGTransactionHandler MakeSGTAHandlerWithSGHandlerSG1ReturningThisSG(bool thisSG, ISlottable pickedSB, ISlottable targetSB){
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						ITransactionCache taCache = MakeSubTAC();
							taCache.pickedSB.Returns(pickedSB);
							taCache.targetSB.Returns(targetSB);
						sg.taCache.Returns(taCache);
					ITransactionManager tam = MakeSubTAM();
						ITransactionSGHandler sgHandler = Substitute.For<ITransactionSGHandler>();
						if(thisSG)
							sgHandler.sg1.Returns(sg);
						else
							sgHandler.sg1.Returns((ISlotGroup)null);
						tam.sgHandler.Returns(sgHandler);
				sgTAHandler = new SGTransactionHandler(sg, tam);
				return sgTAHandler;
			}
			[Test]
			public void GetAddedForFill_SGHandlerSG1NotThisSG_ReturnsTAPickedSB(){
				SGTransactionHandler sgTAHandler;
					ISlottable pickedSB = MakeSubSB();
					sgTAHandler = MakeSGTAHandlerWithSGHandlerSG1ReturningThisSG(false, pickedSB, null);
				
				ISlottable actual = sgTAHandler.GetAddedForFill();

				Assert.That(actual, Is.SameAs(pickedSB));
			}
			[Test]
			public void GetAddedForFill_SGHandlerSG1IsThisSG_ReturnsNull(){
				SGTransactionHandler sgTAHandler = MakeSGTAHandlerWithSGHandlerSG1ReturningThisSG(true, null, null);
				
				ISlottable actual = sgTAHandler.GetAddedForFill();

				Assert.That(actual, Is.Null);
			}
			[Test]
			public void GetRemovedForFill_SGHandlerSG1NotThisSG_ReturnsTAPickedSB(){
				SGTransactionHandler sgTAHandler;
					ISlottable pickedSB = MakeSubSB();
					sgTAHandler = MakeSGTAHandlerWithSGHandlerSG1ReturningThisSG(false, pickedSB, null);
				
				ISlottable actual = sgTAHandler.GetAddedForFill();

				Assert.That(actual, Is.SameAs(pickedSB));
			}
			[Test]
			public void GetRemovedForFill_SGHandlerSG1IsThis_ReturnsNull(){
				SGTransactionHandler sgTAHandler;
					ISlottable pickedSB = MakeSubSB();
					sgTAHandler = MakeSGTAHandlerWithSGHandlerSG1ReturningThisSG(true, pickedSB, null);
				
				ISlottable actual = sgTAHandler.GetAddedForFill();

				Assert.That(actual, Is.Null);
			}
			[TestCaseSource(typeof(CreateNewSBAndFillCases))]
			public void CreateNewSBAndFill_Always_UpdateList(List<ISlottable> list, ISlottable added, int addedIndex){
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSG();
					ISlotSystemManager ssm = MakeSubSSM();
					sg.SetSSM(ssm);
				sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
				List<ISlottable> targetList = new List<ISlottable>(list);
				
				sgTAHandler.CreateNewSBAndFill(added.item, targetList);

				ISlottable actualAdded = targetList[addedIndex];
				AssertCreatedSB(actualAdded, added.item, ssm);
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
				SGTransactionHandler sgTAHandler = new SGTransactionHandler(MakeSubSG(), MakeSubTAM());
				List<ISlottable> targetList = new List<ISlottable>(list);

				sgTAHandler.NullifyIndexOf(item, targetList);

				bool equality = targetList.MemberEquals(expected);
				Assert.That(equality, Is.True);
			}
				class NullifyIndexOfCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] case0;
							ISlottable bowSB_0 = MakeSubSBWithItem(MakeBowInstance(0));
							ISlottable wearSB_0 = MakeSubSBWithItem(MakeWearInstance(0));
							ISlottable shieldSB_0 = MakeSubSBWithItem(MakeShieldInstance(0));
							ISlottable mWeaponSB_0 = MakeSubSBWithItem(MakeMWeaponInstance(0));
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
							case0 = new object[]{list_0, bowSB_0.item, expected_0};
							yield return case0;
						object[] case1;
							ISlottable bowSB_1 = MakeSubSBWithItem(MakeBowInstance(0));
							ISlottable wearSB_1 = MakeSubSBWithItem(MakeWearInstance(0));
							ISlottable shieldSB_1 = MakeSubSBWithItem(MakeShieldInstance(0));
							ISlottable mWeaponSB_1 = MakeSubSBWithItem(MakeMWeaponInstance(0));
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
			public void AssertCreatedSB(ISlottable sb, InventoryItemInstance addedItem, ISlotSystemManager ssm){
				Assert.That(sb.item, Is.SameAs(addedItem));
				Assert.That(sb.ssm, Is.SameAs(ssm));
				Assert.That(sb.isDefocused, Is.True);
				Assert.That(sb.isUnequipped, Is.True);
				Assert.That(sb, Is.Not.Null.And.InstanceOf(typeof(Slottable)));
			}
			[TestCaseSource(typeof(FilledNewSBsCases))]
			public void FilledNewSBs_Various_CreatesAndReturnsNewSBsAccordingly(List<ISlottable> source, bool isPool, bool isAdded, bool isAutoSort, bool isExpandable, List<ISlottable> expected, int addedIndex, InventoryItemInstance addedItem){
				SGTransactionHandler sgTAHandler;
					ISlottable pickedSB = MakeSubSB();
						pickedSB.item.Returns(addedItem);
					ISlotGroup sg = MakeSGForIntegration(isPool, isAutoSort, isExpandable, new List<ISlottable>(source), new SGItemIDSorter(), null, pickedSB, null);
					ITransactionManager tam = MakeSubTAM();
						ITransactionSGHandler sgHandler = Substitute.For<ITransactionSGHandler>();
						if(isAdded)
							sgHandler.sg1.Returns((ISlotGroup)null);
						else
							sgHandler.sg1.Returns(sg);
						tam.sgHandler.Returns(sgHandler);

				sgTAHandler = new SGTransactionHandler(sg, tam);
				
				List<ISlottable> actual = sgTAHandler.FilledNewSBs();

				List<ISlottable> exp = new List<ISlottable>(expected);
				if(isAdded){
					ISlottable addedSB = actual[addedIndex];
					AssertCreatedSB(addedSB, addedItem, sg.ssm);
					exp[addedIndex] = addedSB;
				}
				bool equality = actual.MemberEquals(exp);
				Assert.That(equality, Is.Not.True);
			}
				class FilledNewSBsCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] noUpdate;
							List<ISlottable> source_0;
								ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_0 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_0 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_0 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_0 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_0 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								ISlottable bow1SB_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								source_0 = new List<ISlottable>(new ISlottable[]{
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
							noUpdate = new object[]{
								source_0, false, false, false, false, source_0, -1, MakeBowInstance(0)
							};
							yield return noUpdate;
						object[] filledNOSort_isPool;
							List<ISlottable> source_1;
								ISlottable partsSB_1 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_1 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_1 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_1 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_1 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_1 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_1 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_1 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								ISlottable bow1SB_1 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								source_1 = new List<ISlottable>(new ISlottable[]{
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
							InventoryItemInstance added_1 = bow1SB_1.item;
							filledNOSort_isPool = new object[]{
								source_1, true, true, false, false, source_1, 3, added_1
							};
							yield return filledNOSort_isPool;
						object[] filledSortNoResize_isPool_isAutoSort;
							List<ISlottable> source_2;
								ISlottable partsSB_2 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_2 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_2 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_2 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_2 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_2 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_2 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_2 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								ISlottable bow1SB_2 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								source_2 = new List<ISlottable>(new ISlottable[]{
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
								List<ISlottable> exp_2 = new List<ISlottable>(new ISlottable[]{
									bow0SB_2,
									bow2SB_2,
									null,
									wearSB_2,
									shieldSB_2,
									mWeaponSB_2,
									quiverSB_2,
									packSB_2,
									partsSB_2,
									null,
									null,
									null,
									null
								});
								InventoryItemInstance added_2 = bow1SB_2.item;
							filledSortNoResize_isPool_isAutoSort = new object[]{
								source_2, true, true, true, false, exp_2, 2, added_2
							};
							yield return filledSortNoResize_isPool_isAutoSort;
						object[] filledSortResize_isPool_isAutoSort_isExpandable;
							List<ISlottable> source_3;
								ISlottable partsSB_3 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_3 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_3 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_3 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_3 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_3 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_3 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_3 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								ISlottable bow1SB_3 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
								source_3 = new List<ISlottable>(new ISlottable[]{
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
								List<ISlottable> exp_3 = new List<ISlottable>(new ISlottable[]{
									bow0SB_3,
									bow2SB_3,
									null,
									wearSB_3,
									shieldSB_3,
									mWeaponSB_3,
									quiverSB_3,
									packSB_3,
									partsSB_3
								});
								InventoryItemInstance added_3 = bow1SB_3.item;
							filledSortResize_isPool_isAutoSort_isExpandable = new object[]{
								source_3, true, true, true, true, exp_3, 2, added_3
							};
							yield return filledSortResize_isPool_isAutoSort_isExpandable;
						object[] nulledNOSort_isPool;
							List<ISlottable> source_4;
								ISlottable partsSB_4 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_4 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_4 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_4 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_4 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_4 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_4 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_4 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								source_4 = new List<ISlottable>(new ISlottable[]{
									partsSB_4,
									bow2SB_4,
									wearSB_4,
									null,
									mWeaponSB_4,
									null,
									null,
									packSB_4,
									null,
									quiverSB_4,
									shieldSB_4,
									bow0SB_4,
									null
								});
								List<ISlottable> exp_4 = new List<ISlottable>(new ISlottable[]{
									partsSB_4,
									bow2SB_4,
									wearSB_4,
									null,
									mWeaponSB_4,
									null,
									null,
									packSB_4,
									null,
									null,// nulled
									shieldSB_4,
									bow0SB_4,
									null
								});
							InventoryItemInstance removed_4 = quiverSB_4.item;
							nulledNOSort_isPool = new object[]{
								source_4, true, false, false, false, exp_4, -1, removed_4
							};
							yield return nulledNOSort_isPool;
						object[] nulledSortNoResize_isPool_isAutoSort;
							List<ISlottable> source_5;
								ISlottable partsSB_5 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_5 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_5 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_5 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_5 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_5 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_5 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_5 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								source_5 = new List<ISlottable>(new ISlottable[]{
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
								List<ISlottable> exp_5 = new List<ISlottable>(new ISlottable[]{
									bow2SB_5,
									wearSB_5,
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
									null
								});
								InventoryItemInstance removed_5 = bow0SB_5.item;
							nulledSortNoResize_isPool_isAutoSort = new object[]{
								source_5, true, false, true, false, exp_5, -1, removed_5
							};
							yield return nulledSortNoResize_isPool_isAutoSort;
						object[] nulledSortResize_isPool_isAutoSort_isExpandable;
							List<ISlottable> source_6;
								ISlottable partsSB_6 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_6 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_6 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_6 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_6 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_6 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_6 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_6 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
								source_6 = new List<ISlottable>(new ISlottable[]{
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
								List<ISlottable> exp_6 = new List<ISlottable>(new ISlottable[]{
									bow2SB_6,
									wearSB_6,
									shieldSB_6,
									mWeaponSB_6,
									quiverSB_6,
									packSB_6,
									partsSB_6
								});
								InventoryItemInstance removed_6 = bow0SB_6.item;
							nulledSortResize_isPool_isAutoSort_isExpandable = new object[]{
								source_6, true, false, true, true, exp_6, -1, removed_6
							};
							yield return nulledSortResize_isPool_isAutoSort_isExpandable;
					}
				}
			[Test]
			public void GetAddedForSwap_SSMSG1This_ReturnsSSMTargetSB(){
				SGTransactionHandler sgTAHandler;
					ISlottable targetSB = MakeSubSB();
				sgTAHandler = MakeSGTAHandlerWithSGHandlerSG1ReturningThisSG(true, null, targetSB);

				ISlottable actual = sgTAHandler.GetAddedForSwap();

				Assert.That(actual, Is.SameAs(targetSB));
			}
			[Test]
			public void GetAddedForSwap_SSMSG1NotThis_ReturnsSSMPickedSB(){
				SGTransactionHandler sgTAHandler;
					ISlottable pickedSB = MakeSubSB();
					sgTAHandler = MakeSGTAHandlerWithSGHandlerSG1ReturningThisSG(false, pickedSB, null);

				ISlottable actual = sgTAHandler.GetAddedForSwap();

				Assert.That(actual, Is.SameAs(pickedSB));
			}
			[Test]
			public void GetRemovedForSwap_SSMSG1This_ReturnsSSMPickedSB(){
				SGTransactionHandler sgTAHandler;
					ISlottable pickedSB = MakeSubSB();
				sgTAHandler = MakeSGTAHandlerWithSGHandlerSG1ReturningThisSG(true, pickedSB, null);

				ISlottable actual = sgTAHandler.GetRemovedForSwap();

				Assert.That(actual, Is.SameAs(pickedSB));
			}
			[Test]
			public void GetRemovedForSwap_SSMSG1NotThis_ReturnsSSMTargetSB(){
				SGTransactionHandler sgTAHandler;
					ISlottable targetSB = MakeSubSB();
				sgTAHandler = MakeSGTAHandlerWithSGHandlerSG1ReturningThisSG(false, null, targetSB);

				ISlottable actual = sgTAHandler.GetRemovedForSwap();

				Assert.That(actual, Is.SameAs(targetSB));
			}
			[TestCaseSource(typeof(SwapAndUpdateSBsCases))]
			public void SwapAndUpadteSBs_Various_SetsNewSBsAccordingly(
				bool isPool, 
				bool isAutoSort, 
				bool isExpandable, 
				SGSorter sorter, 
				bool sg1This,
				ISlottable added, 
				ISlottable removed, 
				List<ISlottable> sbs, 
				List<ISlottable> expected, 
				int indexAtSwapped)
			{
				SGTransactionHandler sgTAHandler = MakeSGTAHandlerWithSGHandlerSG1ReturningThisSG(sg1This, sg1This?removed: added, sg1This?added:removed);
				sgTAHandler.sg.isPool.Returns(isPool);
				sgTAHandler.sg.isAutoSort.Returns(isAutoSort);
				sgTAHandler.sg.isExpandable.Returns(isExpandable);
				sgTAHandler.sorterHandler.sorter.Returns(sorter);
				sgTAHandler.sg.slottables.Returns(sbs);
				
				List<ISlottable> actual = sgTAHandler.SwappedNewSBs();
				
				List<ISlottable> expList = new List<ISlottable>(expected);
				if(!isPool){
					ISlottable actualAdded = actual[indexAtSwapped];
					expList[indexAtSwapped] = actualAdded;
				}
				bool equality = actual.MemberEquals(expList);
				Assert.That(equality, Is.Not.True);
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
				SGTransactionHandler sgTAHandler = new SGTransactionHandler(MakeSubSG(), MakeSubTAM());

				List<ISlottable> actualList = new List<ISlottable>(list);

				bool actual = sgTAHandler.TryChangeStackableQuantity(actualList, delta, added);

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
				SGTransactionHandler sgTAHandler = new SGTransactionHandler(MakeSubSG(), MakeSubTAM());
				List<ISlottable> actualTarget = new List<ISlottable>(sbs);

				sgTAHandler.TryChangeStackableQuantity(actualTarget, addedItem, true);

				ISlottable actualAdded = actualTarget[indexAtAdded];
				actualAdded.Received().SetQuantity(expectedQua);
			}
				class TryChangeStackableQuantity_AddedCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] case0;
							ISlottable bowSB_0 = MakeSubSBWithItem(MakeBowInstance(0));
							ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstance(0, 1));
							ISlottable parts1SB_0 = MakeSubSBWithItem(MakePartsInstance(1, 5));
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
							ISlottable parts1SB_1 = MakeSubSBWithItem(MakePartsInstance(1, 5));
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
				SGTransactionHandler sgTAHandler = new SGTransactionHandler(MakeSubSG(), MakeSubTAM());
				List<ISlottable> actualTarget = new List<ISlottable>(sbs);

				sgTAHandler.TryChangeStackableQuantity(actualTarget, decreasedItem, false);

				ISlottable actualDecreased = actualTarget[indexAtDecreased];
				actualDecreased.Received().SetQuantity(expectedQua);
			}
				class TryChangeStackableQuantity_DecreaseCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] case0;
							ISlottable bowSB_0 = MakeSubSBWithItem(MakeBowInstance(0));
							ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstance(0, 7));
							ISlottable parts1SB_0 = MakeSubSBWithItem(MakePartsInstance(1, 5));
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
							ISlottable parts1SB_1 = MakeSubSBWithItem(MakePartsInstance(1, 5));
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
				SGTransactionHandler sgTAHandler = new SGTransactionHandler(MakeSubSG(), MakeSubTAM());
				List<ISlottable> actualTarget = new List<ISlottable>(sbs);
				ISlottable sbCache = actualTarget[indexAtNullified];

				sgTAHandler.TryChangeStackableQuantity(actualTarget, removedItem, false);

				Assert.That(actualTarget[indexAtNullified], Is.Null);
				sbCache.Received().Destroy();
			}
				class TryChangeStackableQuantity_DecreasedDownToZeroCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] case0;
							ISlottable bowSB_0 = MakeSubSBWithItem(MakeBowInstance(0));
							ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstance(0, 1));
							ISlottable parts1SB_0 = MakeSubSBWithItem(MakePartsInstance(1, 5));
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
							ISlottable parts1SB_1 = MakeSubSBWithItem(MakePartsInstance(1, 5));
							ISlottable wearSB_1 = MakeSubSBWithItem(MakeWearInstance(0));
							List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
								bowSB_1, partsSB_1, wearSB_1, parts1SB_1
							});
							PartsInstance removedParts_1 = MakePartsInstance(1, 5);
							case1 = new object[]{sbs_1, removedParts_1, 3};
							yield return case1;
					}
				}
			public void AssertItemAndQuantityEquality(InventoryItemInstance item, InventoryItemInstance other){
				if(item != null){
					Assert.That(other, Is.Not.Null);
					Assert.That(item.Item.ItemID, Is.EqualTo(other.Item.ItemID));
					Assert.That(item.quantity, Is.EqualTo(other.quantity));
				}else
					Assert.That(other, Is.Null);
			}
			SlotGroup MakeSGForIntegration(bool isPool, bool isAutoSort, bool isExpandable, List<ISlottable> sbs, SGSorter sorter, List<InventoryItemInstance> moved, ISlottable pickedSB, ISlottable targetSB){
				SlotGroup sg = MakeSG();
					ISlotSystemManager ssm = MakeSubSSM();
						ISlotSystemBundle pBun = MakeSubBundle();
							if(isPool)
								pBun.ContainsInHierarchy(sg).Returns(true);
							else
								pBun.ContainsInHierarchy(sg).Returns(false);
						ssm.poolBundle.Returns(pBun);
					sg.SetSSM(ssm);
					ISorterHandler sorterHandler = new SorterHandler();
						sorterHandler.SetIsAutoSort(isAutoSort);
						sorterHandler.SetSorter(sorter);
					sg.SetSorterHandler(sorterHandler);
					ISBHandler sbHandler = Substitute.For<ISBHandler>();
						sbHandler.slottables.Returns(sbs);
					sg.SetSBHandler(sbHandler);
					ITransactionCache taCache = Substitute.For<ITransactionCache>();
						taCache.moved.Returns(moved);
						taCache.pickedSB.Returns(pickedSB);
						taCache.targetSB.Returns(targetSB);
					sg.SetTACache(taCache);
				sg.SetFilterHandler(new FilterHandler());
				sg.SetSlotsHolder(new SlotsHolder(sg));
				sg.InspectorSetUp(MakeSubPoolInv(), Substitute.For<SGFilter>(), sorter, isExpandable?0:10);
				return sg;
			}
			public void AssertItemIDEquality(InventoryItemInstance item, InventoryItemInstance other){
				if(item != null){
					Assert.That(item.Item.ItemID, Is.EqualTo(other.Item.ItemID));
				}else
					Assert.That(other, Is.Null);
			}
			[TestCaseSource(typeof(AddedNewSBs_UnitCases))]
			public void AddedNewSBs_Unit(
				List<InventoryItemInstance> added, 
				List<ISlottable> source, 
				bool isAutoSort, 
				bool isExpandable, 
				SGSorter sorter, 
				List<ISlottable> xIncreased, 
				List<int> xIncreasedQuantity, 
				List<ISlottable> xCreated, 
				List<InventoryItemInstance> xCreatedItems)
			{
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						ITransactionCache taCache = Substitute.For<ITransactionCache>();
						taCache.moved.Returns(added);
						sg.taCache.Returns(taCache);
						List<ISlottable> sbs = new List<ISlottable>(source);
						sg.slottables.Returns(sbs);
						sg.isAutoSort.Returns(isAutoSort);
						sg.sorter.Returns(sorter);
						sg.isExpandable.Returns(isExpandable);
					sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
				
				List<ISlottable> actual = sgTAHandler.AddedNewSBs();

				foreach(var sb in xIncreased)
					sb.Received().SetQuantity(xIncreasedQuantity[xIncreased.IndexOf(sb)]);
				foreach(var sb in xCreated)
					sg.Received().CreateSB(xCreatedItems[xCreated.IndexOf(sb)]);
			}
				class AddedNewSBs_UnitCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] increased;
							List<InventoryItemInstance> added_0;
								PartsInstance parts0a_0 = MakePartsInstance(0, 1);
								PartsInstance parts1a_0 = MakePartsInstance(1, 7);
								PartsInstance parts2a_0 = MakePartsInstance(2, 20);
								PartsInstance parts3a_0 = MakePartsInstance(3, 3);
								added_0 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									parts0a_0,
									parts1a_0,
									parts2a_0,
									parts3a_0
								});
							List<ISlottable> source_0;
								PartsInstance parts0_0 = MakePartsInstance(0, 4);
								PartsInstance parts1_0 = MakePartsInstance(1, 4);
								PartsInstance parts2_0 = MakePartsInstance(2, 4);
								PartsInstance parts3_0 = MakePartsInstance(3, 4);
								ISlottable sb0_0 = MakeSubSBWithItem(parts0_0);
								ISlottable sb1_0 = MakeSubSBWithItem(parts1_0);
								ISlottable sb2_0 = MakeSubSBWithItem(parts2_0);
								ISlottable sb3_0 = MakeSubSBWithItem(parts3_0);
								source_0 = new List<ISlottable>(new ISlottable[]{
									sb0_0,
									sb1_0,
									sb2_0,
									sb3_0
								});
							List<ISlottable> xIncreased_0 = new List<ISlottable>(source_0);
							List<int> xIncreasedQuantity_0 = new List<int>(new int[]{
								5,
								11,
								24,
								7
							});
							List<ISlottable> xCreated_0 = new List<ISlottable>();
							List<InventoryItemInstance> xCreatedItems_0 = new List<InventoryItemInstance>();
							increased = new object[]{
								added_0,
								source_0,
								false,
								false,
								new SGItemIDSorter(),
								xIncreased_0,
								xIncreasedQuantity_0,
								xCreated_0,
								xCreatedItems_0
							};
						yield return increased;
						object[] created;
							List<InventoryItemInstance> added_1;
								BowInstance bow0a_1 = MakeBowInstance(0);
								WearInstance wear0a_1 = MakeWearInstance(0);
								ShieldInstance shield0a_1 = MakeShieldInstance(0);
								QuiverInstance quiver0a_1 = MakeQuiverInstance(0);
								added_1 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									bow0a_1,
									wear0a_1,
									shield0a_1,
									quiver0a_1
								});
							List<ISlottable> source_1;
								PartsInstance parts0_1 = MakePartsInstance(0, 4);
								PartsInstance parts1_1 = MakePartsInstance(1, 4);
								PartsInstance parts2_1 = MakePartsInstance(2, 4);
								PartsInstance parts3_1 = MakePartsInstance(3, 4);
								ISlottable sb0_1 = MakeSubSBWithItem(parts0_1);
								ISlottable sb1_1 = MakeSubSBWithItem(parts1_1);
								ISlottable sb2_1 = MakeSubSBWithItem(parts2_1);
								ISlottable sb3_1 = MakeSubSBWithItem(parts3_1);
								source_1 = new List<ISlottable>(new ISlottable[]{
									sb0_1,
									sb1_1,
									sb2_1,
									sb3_1
								});
							List<ISlottable> xIncreased_1 = new List<ISlottable>();
							List<int> xIncreasedQuantity_1 = new List<int>();
							List<ISlottable> xCreated_1 = new List<ISlottable>(new ISlottable[]{
								MakeSubSBWithItem(bow0a_1),
								MakeSubSBWithItem(wear0a_1),
								MakeSubSBWithItem(shield0a_1),
								MakeSubSBWithItem(quiver0a_1)
							});
							List<InventoryItemInstance> xCreatedItems_1 = new List<InventoryItemInstance>(added_1);
							created = new object[]{
								added_1,
								source_1,
								false,
								false,
								new SGItemIDSorter(),
								xIncreased_1,
								xIncreasedQuantity_1,
								xCreated_1,
								xCreatedItems_1
							};
						yield return created;
						object[] mixed;
							List<InventoryItemInstance> added_2;
								BowInstance bow0a_2 = MakeBowInstance(0);
								WearInstance wear0a_2 = MakeWearInstance(0);
								ShieldInstance shield0a_2 = MakeShieldInstance(0);
								QuiverInstance quiver0a_2 = MakeQuiverInstance(0);
								PartsInstance parts0a_2 = MakePartsInstance(0, 1);
								PartsInstance parts1a_2 = MakePartsInstance(1, 7);
								PartsInstance parts2a_2 = MakePartsInstance(2, 20);
								PartsInstance parts3a_2 = MakePartsInstance(3, 3);
								added_2 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
									bow0a_2,
									wear0a_2,
									shield0a_2,
									quiver0a_2,
									parts0a_2,
									parts1a_2,
									parts2a_2,
									parts3a_2
								});
							List<ISlottable> source_2;
								PartsInstance parts0_2 = MakePartsInstance(0, 4);
								PartsInstance parts1_2 = MakePartsInstance(1, 4);
								PartsInstance parts2_2 = MakePartsInstance(2, 4);
								PartsInstance parts3_2 = MakePartsInstance(3, 4);
								ISlottable sb0_2 = MakeSubSBWithItem(parts0_2);
								ISlottable sb1_2 = MakeSubSBWithItem(parts1_2);
								ISlottable sb2_2 = MakeSubSBWithItem(parts2_2);
								ISlottable sb3_2 = MakeSubSBWithItem(parts3_2);
								source_2 = new List<ISlottable>(new ISlottable[]{
									sb0_2,
									sb1_2,
									sb2_2,
									sb3_2
								});
							List<ISlottable> xIncreased_2 = new List<ISlottable>(source_2);
							List<int> xIncreasedQuantity_2 = new List<int>(new int[]{
								5,
								11,
								24,
								7
							});
							List<ISlottable> xCreated_2 = new List<ISlottable>(new ISlottable[]{
								MakeSubSBWithItem(bow0a_2),
								MakeSubSBWithItem(wear0a_2),
								MakeSubSBWithItem(shield0a_2),
								MakeSubSBWithItem(quiver0a_2)
							});
							List<InventoryItemInstance> xCreatedItems_2 = new List<InventoryItemInstance>(added_2);
							mixed = new object[]{
								added_2,
								source_2,
								false,
								false,
								new SGItemIDSorter(),
								xIncreased_2,
								xIncreasedQuantity_2,
								xCreated_2,
								xCreatedItems_2
							};
						yield return mixed;
					}
				}
			[TestCaseSource(typeof(SortedSBsIfAutoSortAccordingToExpandablityCases))]
			public void SortedSBsIfAutoSortAccordingToExpandablity_Always_ReturnsAccordingly(bool isAutoSort, bool isExpandable, List<ISlottable> source ,List<ISlottable> expected){
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						sg.isExpandable.Returns(isExpandable);
					sg.sorter.Returns(new SGItemIDSorter());
					sg.isAutoSort.Returns(isAutoSort);
					ISorterHandler sorterHandler = new SorterHandler();
						sorterHandler.SetSorter(new SGItemIDSorter());
						List<ISlottable> sortedNonResize = sorterHandler.GetSortedSBsWithoutResize(source);
					sg.GetSortedSBsWithoutResize(source).Returns(sortedNonResize);
						List<ISlottable> sortedResize = sorterHandler.GetSortedSBsWithResize(source);
					sg.GetSortedSBsWithResize(source).Returns(sortedResize);
				sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());

				List<ISlottable> actual = sgTAHandler.SortedSBsIfAutoSortAccordingToExpandability(source);

				bool equality = actual.MemberEquals(expected);
				Assert.That(equality, Is.True);
			}
				class SortedSBsIfAutoSortAccordingToExpandablityCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] noChange_isAutoSortF;
							List<ISlottable> source_0;
								ISlottable sbBow_0 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable sbWear_0 = MakeSubSBWithItem(MakeWearInstance(0));
								ISlottable sbShield_0 = MakeSubSBWithItem(MakeShieldInstance(0));
								ISlottable sbMWeapon_0 = MakeSubSBWithItem(MakeMWeaponInstance(0));
								ISlottable sbQuiver_0 = MakeSubSBWithItem(MakeQuiverInstance(0));
								ISlottable sbPack_0 = MakeSubSBWithItem(MakePackInstance(0));
								source_0 = new List<ISlottable>(new ISlottable[]{
									sbPack_0,
									null,
									null,
									sbQuiver_0,
									sbBow_0,
									null,
									sbWear_0,
									sbMWeapon_0,
									null,
									sbShield_0
								});
							noChange_isAutoSortF = new object[]{
								false,
								false,
								source_0,
								source_0
							};
							yield return noChange_isAutoSortF;
						object[] sortNoResize_isAutoSortT_isExpandableF;
							List<ISlottable> source_1;
								ISlottable sbBow_1 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable sbWear_1 = MakeSubSBWithItem(MakeWearInstance(0));
								ISlottable sbShield_1 = MakeSubSBWithItem(MakeShieldInstance(0));
								ISlottable sbMWeapon_1 = MakeSubSBWithItem(MakeMWeaponInstance(0));
								ISlottable sbQuiver_1 = MakeSubSBWithItem(MakeQuiverInstance(0));
								ISlottable sbPack_1 = MakeSubSBWithItem(MakePackInstance(0));
								source_1 = new List<ISlottable>(new ISlottable[]{
									sbPack_1,
									null,
									null,
									sbQuiver_1,
									sbBow_1,
									null,
									sbWear_1,
									sbMWeapon_1,
									null,
									sbShield_1
								});
								List<ISlottable> exp_1 = new List<ISlottable>(new ISlottable[]{
									sbBow_1,
									sbWear_1,
									sbShield_1,
									sbMWeapon_1,
									sbQuiver_1,
									sbPack_1,
									null,
									null,
									null,
									null
								});
							sortNoResize_isAutoSortT_isExpandableF = new object[]{
								true,
								false,
								source_1,
								exp_1
							};
							yield return sortNoResize_isAutoSortT_isExpandableF;
						object[] sortResize_isAutoSortT_isExpandableT;
							List<ISlottable> source_2;
								ISlottable sbBow_2 = MakeSubSBWithItem(MakeBowInstance(0));
								ISlottable sbWear_2 = MakeSubSBWithItem(MakeWearInstance(0));
								ISlottable sbShield_2 = MakeSubSBWithItem(MakeShieldInstance(0));
								ISlottable sbMWeapon_2 = MakeSubSBWithItem(MakeMWeaponInstance(0));
								ISlottable sbQuiver_2 = MakeSubSBWithItem(MakeQuiverInstance(0));
								ISlottable sbPack_2 = MakeSubSBWithItem(MakePackInstance(0));
								source_2 = new List<ISlottable>(new ISlottable[]{
									sbPack_2,
									null,
									null,
									sbQuiver_2,
									sbBow_2,
									null,
									sbWear_2,
									sbMWeapon_2,
									null,
									sbShield_2
								});
								List<ISlottable> exp_2 = new List<ISlottable>(new ISlottable[]{
									sbBow_2,
									sbWear_2,
									sbShield_2,
									sbMWeapon_2,
									sbQuiver_2,
									sbPack_2
								});
							sortResize_isAutoSortT_isExpandableT = new object[]{
								true,
								true,
								source_2,
								exp_2
							};
							yield return sortResize_isAutoSortT_isExpandableT;
					}
				}
			void DebugSBs(IEnumerable<ISlottable> sbs){
				foreach(var sb in sbs)
					SlotSystemUtil.Stack(sb.item.ToString());
				Debug.Log(SlotSystemUtil.Stacked);
			}
			[TestCaseSource(typeof(AddedNewSBs_VariousCases))]
			public void AddedNewSBs_Various_Various(
				bool isAutoSort, 
				bool isExpandable, 
				SGSorter sorter,  
				List<ISlottable> source, 
				List<InventoryItemInstance> added, 
				List<ISlottable> expected)
			{
				SGTransactionHandler sgTAHandler;
					SlotGroup sg = MakeSGForIntegration(true, isAutoSort, isExpandable, new List<ISlottable>(source), sorter, added, null, null);
					// ISlotGroup sg = MakeSubSG();
					// 	sg.sorter.Returns(sorter);
					// 	sg.isAutoSort.Returns(isAutoSort);
					// 	sg.isExpandable.Returns(isExpandable);
					// 	sg.slottables.Returns(sbs);
					// 	ITransactionCache taCache = Substitute.For<ITransactionCache>();
					// 	taCache.moved.Returns(added);
					// 	sg.taCache.Returns(taCache);
					sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
				List<ISlottable> actual = sgTAHandler.AddedNewSBs();
				
				Assert.That(actual.Count, Is.EqualTo(expected.Count));
				foreach(var sb in actual){
					int index = actual.IndexOf(sb);
					AssertItemAndQuantityEquality(sb.item, expected[index].item);
				}
			}
				class AddedNewSBs_VariousCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] addedNoSort;
							ISlottable bowSB_0 = MakeSBWithItem(MakeBowInstance(0));
							ISlottable wearSB_0 = MakeSBWithItem(MakeWearInstance(0));
							ISlottable shieldSB_0 = MakeSBWithItem(MakeShieldInstance(0));
							ISlottable mWeaponSB_0 = MakeSBWithItem(MakeMWeaponInstance(0));
							ISlottable partsSB_0 = MakeSBWithItem(MakePartsInstance(0, 1));
							ISlottable parts1SB_0 = MakeSBWithItem(MakePartsInstance(1, 2));
							ISlottable bow1SB_a_0 = MakeSBWithItem(MakeBowInstance(1));
							ISlottable quiverSB_a_0 = MakeSBWithItem(MakeQuiverInstance(1));
							ISlottable partsSB_a_0 = MakeSBWithItem(MakePartsInstance(0, 6));
							ISlottable parts1SB_a_0 = MakeSBWithItem(MakePartsInstance(1, 3));
							List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
								shieldSB_0,
								null,
								null,
								null,
								partsSB_0,
								null,
								parts1SB_0,
								wearSB_0,
								mWeaponSB_0,
								null,
								bowSB_0
							});
							SGSorter idSorter_0 = new SGItemIDSorter();
							List<InventoryItemInstance> added_0 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
								bow1SB_a_0.item,
								quiverSB_a_0.item,
								partsSB_a_0.item,
								parts1SB_a_0.item 
							});
							ISlottable addedPartsSB_0 = MakeSBWithItem(MakePartsInstance(0, 7));
							ISlottable addedParts1SB_0 = MakeSBWithItem(MakePartsInstance(1, 5));
							List<ISlottable> exp_0 = new List<ISlottable>(new ISlottable[]{
								shieldSB_0,
								bow1SB_a_0,
								quiverSB_a_0,
								null,
								addedPartsSB_0,
								null,
								addedParts1SB_0,
								wearSB_0,
								mWeaponSB_0,
								null,
								bowSB_0
							});
							addedNoSort = new object[]{
								false, 
								false, 
								idSorter_0, 
								sbs_0, 
								added_0, 
								exp_0
							};
							yield return addedNoSort;
						// object[] addedSortNoResize_isAutoSort;
						// 	ISlottable bowSB_1 = MakeSubSBWithItem(MakeBowInstance(0));
						// 	ISlottable wearSB_1 = MakeSubSBWithItem(MakeWearInstance(0));
						// 	ISlottable shieldSB_1 = MakeSubSBWithItem(MakeShieldInstance(0));
						// 	ISlottable mWeaponSB_1 = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
						// 	ISlottable partsSB_1 = MakeSubSBWithItem(MakePartsInstance(0, 1));
						// 		partsSB_1.quantity.Returns(1);
						// 	ISlottable parts1SB_1 = MakeSubSBWithItem(MakePartsInstance(1, 2));
						// 		parts1SB_1.quantity.Returns(2);
						// 	ISlottable bow1SB_a_1 = MakeSubSBWithItem(MakeBowInstance(1));
						// 	ISlottable quiverSB_a_1 = MakeSubSBWithItem(MakeQuiverInstance(1));
						// 	ISlottable partsSB_a_1 = MakeSubSBWithItem(MakePartsInstance(0, 6));
						// 		partsSB_a_1.quantity.Returns(6);
						// 	ISlottable parts1SB_a_1 = MakeSubSBWithItem(MakePartsInstance(1, 3));
						// 		parts1SB_a_1.quantity.Returns(3);
						// 	List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
						// 		parts1SB_1,
						// 		wearSB_1,
						// 		null,
						// 		mWeaponSB_1,
						// 		null,
						// 		partsSB_1,
						// 		null,
						// 		shieldSB_1,
						// 		bowSB_1,
						// 		null,
						// 		null
						// 	});
						// 	SGSorter idSorter_1 = new SGItemIDSorter();
						// 	List<InventoryItemInstance> added_1 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
						// 		bow1SB_a_1.item,
						// 		quiverSB_a_1.item,
						// 		partsSB_a_1.item,
						// 		parts1SB_a_1.item 
						// 	});
						// 	ISlottable addedPartsSB_1 = MakeSubSBWithItem(MakePartsInstance(0, 8));
						// 	ISlottable addedParts1SB_1 = MakeSubSBWithItem(MakePartsInstance(1, 5));
						// 	List<ISlottable> exp_1 = new List<ISlottable>(new ISlottable[]{
						// 		bowSB_1,
						// 		bow1SB_a_1,
						// 		wearSB_1,
						// 		shieldSB_1,
						// 		mWeaponSB_1,
						// 		quiverSB_a_1,
						// 		addedPartsSB_1,
						// 		addedParts1SB_1,
						// 		null,
						// 		null
						// 	});
						// 	addedSortNoResize_isAutoSort = new object[]{
						// 		true,
						// 		false,
						// 		idSorter_1,
						// 		sbs_1,
						// 		added_1,
						// 		exp_1
						// 	};
						// 	yield return addedSortNoResize_isAutoSort;
						// object[] addedSortResize_isAutoSort_isExpandable;
						// 	ISlottable bowSB_2 = MakeSubSBWithItem(MakeBowInstance(0));
						// 	ISlottable wearSB_2 = MakeSubSBWithItem(MakeWearInstance(0));
						// 	ISlottable shieldSB_2 = MakeSubSBWithItem(MakeShieldInstance(0));
						// 	ISlottable mWeaponSB_2 = MakeSubSBWithItem(MakeMeleeWeaponInstance(0));
						// 	ISlottable partsSB_2 = MakeSubSBWithItem(MakePartsInstance(0, 1));
						// 		partsSB_2.quantity.Returns(1);
						// 	ISlottable parts1SB_2 = MakeSubSBWithItem(MakePartsInstance(1, 2));
						// 		parts1SB_2.quantity.Returns(2);
						// 	ISlottable bow1SB_a_2 = MakeSubSBWithItem(MakeBowInstance(1));
						// 	ISlottable quiverSB_a_2 = MakeSubSBWithItem(MakeQuiverInstance(1));
						// 	ISlottable partsSB_a_2 = MakeSubSBWithItem(MakePartsInstance(0, 6));
						// 		partsSB_a_2.quantity.Returns(6);
						// 	ISlottable parts1SB_a_2 = MakeSubSBWithItem(MakePartsInstance(1, 3));
						// 		parts1SB_a_2.quantity.Returns(3);
						// 	List<ISlottable> sbs_2 = new List<ISlottable>(new ISlottable[]{
						// 		parts1SB_2,
						// 		wearSB_2,
						// 		null,
						// 		mWeaponSB_2,
						// 		null,
						// 		partsSB_2,
						// 		null,
						// 		shieldSB_2,
						// 		bowSB_2,
						// 		null,
						// 		null
						// 	});
						// 	SGSorter idSorter_2 = new SGItemIDSorter();
						// 	List<InventoryItemInstance> added_2 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
						// 		bow1SB_a_2.item,
						// 		quiverSB_a_2.item,
						// 		partsSB_a_2.item,
						// 		parts1SB_a_2.item 
						// 	});
						// 	ISlottable addedPartsSB_2 = MakeSubSBWithItem(MakePartsInstance(0, 8));
						// 	ISlottable addedParts1SB_2 = MakeSubSBWithItem(MakePartsInstance(1, 5));
						// 	List<ISlottable> exp_2 = new List<ISlottable>(new ISlottable[]{
						// 		bowSB_2,
						// 		bow1SB_a_2,
						// 		wearSB_2,
						// 		shieldSB_2,
						// 		mWeaponSB_2,
						// 		quiverSB_a_2,
						// 		addedPartsSB_2,
						// 		addedParts1SB_2
						// 	});
						// 	addedSortResize_isAutoSort_isExpandable = new object[]{
						// 		true,
						// 		true,
						// 		idSorter_2,
						// 		sbs_2,
						// 		added_2,
						// 		exp_2
						// 	};
						// 	yield return addedSortResize_isAutoSort_isExpandable;
					}
				}
			[TestCaseSource(typeof(RemovedNewSBs_VariousCases))]
			public void RemovedNewSBs_Various_Various(
				bool isAutoSort, 
				bool isExpandable, 
				SGSorter sorter,  
				List<ISlottable> source, 
				List<InventoryItemInstance> removed,
				List<ISlottable> expected)
			{
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						sg.sorter.Returns(sorter);
						sg.isAutoSort.Returns(isAutoSort);
						sg.isExpandable.Returns(isExpandable);
						List<ISlottable> sbs = new List<ISlottable>(source);
						sg.slottables.Returns(sbs);
						ITransactionCache taCache = Substitute.For<ITransactionCache>();
						taCache.moved.Returns(removed);
					sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
				
				List<ISlottable> actual = sgTAHandler.RemovedNewSBs();
				
				Assert.That(actual.Count, Is.EqualTo(expected));
				foreach(var sb in actual){
					int index = actual.IndexOf(sb);
					AssertItemAndQuantityEquality(sb.item, expected[index].item);
				}
			}
				class RemovedNewSBs_VariousCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] removedNoSort;
							ISlottable bowSB_0 = MakeSubSBWithItem(MakeBowInstance(0));
							ISlottable wearSB_0 = MakeSubSBWithItem(MakeWearInstance(0));
							ISlottable shieldSB_0 = MakeSubSBWithItem(MakeShieldInstance(0));
							ISlottable mWeaponSB_0 = MakeSubSBWithItem(MakeMWeaponInstance(0));
							ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								partsSB_0.quantity.Returns(1);
							ISlottable parts1SB_0 = MakeSubSBWithItem(MakePartsInstance(1, 2));
								parts1SB_0.quantity.Returns(2);
							ISlottable partsSB_a_0 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								partsSB_a_0.quantity.Returns(1);
							ISlottable parts1SB_a_0 = MakeSubSBWithItem(MakePartsInstance(1, 1));
								parts1SB_a_0.quantity.Returns(1);
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
							List<InventoryItemInstance> removed_0 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
								wearSB_0.item,
								shieldSB_0.item,
								partsSB_a_0.item,
								parts1SB_a_0.item 
							});
							ISlottable removedParts1SB_0 = MakeSubSBWithItem(MakePartsInstance(1, 1));
							List<ISlottable> exp_0 = new List<ISlottable>(new ISlottable[]{
								removedParts1SB_0,
								null,
								null,
								mWeaponSB_0,
								null,
								null,
								null,
								null,
								bowSB_0,
								null,
								null
							});
							removedNoSort = new object[]{
								false, 
								false, 
								idSorter_0, 
								sbs_0, 
								removed_0, 
								exp_0
							};
							yield return removedNoSort;
						object[] removedSortNoResize_isAutoSort;
							ISlottable bowSB_1 = MakeSubSBWithItem(MakeBowInstance(0));
							ISlottable wearSB_1 = MakeSubSBWithItem(MakeWearInstance(0));
							ISlottable shieldSB_1 = MakeSubSBWithItem(MakeShieldInstance(0));
							ISlottable mWeaponSB_1 = MakeSubSBWithItem(MakeMWeaponInstance(0));
							ISlottable partsSB_1 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								partsSB_1.quantity.Returns(1);
							ISlottable parts1SB_1 = MakeSubSBWithItem(MakePartsInstance(1, 2));
								parts1SB_1.quantity.Returns(2);
							ISlottable partsSB_a_1 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								partsSB_a_1.quantity.Returns(1);
							ISlottable parts1SB_a_1 = MakeSubSBWithItem(MakePartsInstance(1, 1));
								parts1SB_a_1.quantity.Returns(1);
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
							List<InventoryItemInstance> removed_1 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
								wearSB_1.item,
								shieldSB_1.item,
								partsSB_a_1.item,
								parts1SB_a_1.item 
							});
							ISlottable removedParts1SB_1 = MakeSubSBWithItem(MakePartsInstance(1, 1));
							List<ISlottable> exp_1 = new List<ISlottable>(new ISlottable[]{
								bowSB_1,
								mWeaponSB_1,
								removedParts1SB_1,
								null,
								null,
								null,
								null,
								null,
								null,
								null,
								null
							});
							removedSortNoResize_isAutoSort = new object[]{
								true, 
								false, 
								idSorter_1, 
								sbs_1, 
								removed_1, 
								exp_1
							};
							yield return removedSortNoResize_isAutoSort;
						object[] removedSortResize_isAutoSort_isExpandable;
							ISlottable bowSB_2 = MakeSubSBWithItem(MakeBowInstance(0));
							ISlottable wearSB_2 = MakeSubSBWithItem(MakeWearInstance(0));
							ISlottable shieldSB_2 = MakeSubSBWithItem(MakeShieldInstance(0));
							ISlottable mWeaponSB_2 = MakeSubSBWithItem(MakeMWeaponInstance(0));
							ISlottable partsSB_2 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								partsSB_2.quantity.Returns(1);
							ISlottable parts1SB_2 = MakeSubSBWithItem(MakePartsInstance(1, 2));
								parts1SB_2.quantity.Returns(2);
							ISlottable partsSB_a_2 = MakeSubSBWithItem(MakePartsInstance(0, 1));
								partsSB_a_2.quantity.Returns(1);
							ISlottable parts1SB_a_2 = MakeSubSBWithItem(MakePartsInstance(1, 1));
								parts1SB_a_2.quantity.Returns(1);
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
							List<InventoryItemInstance> removed_2 = new List<InventoryItemInstance>(new InventoryItemInstance[]{
								wearSB_2.item,
								shieldSB_2.item,
								partsSB_a_2.item,
								parts1SB_a_2.item 
							});
							ISlottable removedParts1SB_2 = MakeSubSBWithItem(MakePartsInstance(1, 1));
							List<ISlottable> exp_2 = new List<ISlottable>(new ISlottable[]{
								bowSB_2,
								mWeaponSB_2,
								removedParts1SB_2
							});
							removedSortResize_isAutoSort_isExpandable = new object[]{
								true, 
								true, 
								idSorter_2, 
								sbs_2, 
								removed_2, 
								exp_2
							};
							yield return removedSortResize_isAutoSort_isExpandable;
					}
				}
			[Test]
			public void SyncSBsToSlots(){
				
			}
		/*	helper */
			static ISlottable MakeSubSBWithItemAndSG(InventoryItemInstance item, SlotGroup sg){
				ISlottable sb = MakeSubSB();
					sb.item.Returns(item);
					sb.sg.Returns(sg);
				return sb;
			}
			static Slottable MakeSBWithItem(InventoryItemInstance item){
				Slottable sb = MakeSB();
				sb.SetItemHandler(new ItemHandler(item));
				return sb;
			}
			static ISlottable MakeSubSBWithItem(InventoryItemInstance item){
				ISlottable sb = MakeSubSB();
					sb.item.Returns(item);
					sb.quantity.Returns(item.quantity);
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
