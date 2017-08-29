using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UISystem;
using Utility;
namespace SlotSystemTests{
	namespace SlotGroupTests{
		[TestFixture]
		public class SGTransactionHandlerTests: SlotSystemTest {
			[TestCaseSource(typeof(ReorderedNewSBsCases))]
			public void ReorderedNewSBs_Always_CreateAndReturnsReorderdSBs(ISlottable picked, ISlottable target, List<ISlottable> source, IEnumerable<ISlottable> expected){
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
					sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
					ISBHandler sbHandler = Substitute.For<ISBHandler>();
						sbHandler.GetSBs().Returns(new List<ISlottable>(source));
					sgTAHandler.SetSBHandler(sbHandler);
						ITransactionCache taCache = Substitute.For<ITransactionCache>();
						taCache.GetPickedSB().Returns(picked);
						taCache.GetTargetSB().Returns(target);
						sg.GetTAC().Returns(taCache);
				
				List<ISlottable> actual = sgTAHandler.ReorderedNewSBs();

				Assert.That(actual, Is.EqualTo(expected));
			}
				class ReorderedNewSBsCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData case0;
							ISlottable sb0_0 = MakeSubSB();
							ISlottable sb1_0 = MakeSubSB();
							ISlottable sb2_0 = MakeSubSB();
							ISlottable sb3_0 = MakeSubSB();
							ISlottable sb4_0 = MakeSubSB();
							List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
								sb0_0, 
								sb1_0, 
								sb2_0,//to
								sb3_0, 
								sb4_0//from
							});
							ISlottable picked_0 = sb4_0;
							ISlottable hovered_0 = sb2_0;
							IEnumerable<ISlottable> exp_0 = new ISlottable[]{
								sb0_0,
								sb1_0,
								sb4_0,
								sb2_0,
								sb3_0
							};
							case0 = new TestCaseData(picked_0, hovered_0, sbs_0, exp_0);
							yield return case0.SetName("FromTail_ToMiddle");
						TestCaseData case1;
							ISlottable sb0_1 = MakeSubSB();
							ISlottable sb1_1 = MakeSubSB();
							ISlottable sb2_1 = MakeSubSB();
							ISlottable sb3_1 = MakeSubSB();
							ISlottable sb4_1 = MakeSubSB();
							List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
								sb0_1,//from 
								sb1_1, 
								sb2_1,
								sb3_1, 
								sb4_1//to
							});
							ISlottable picked_1 = sb0_1;
							ISlottable hovered_1 = sb4_1;
							IEnumerable<ISlottable> exp_1 = new ISlottable[]{
								sb1_1,
								sb2_1,
								sb3_1,
								sb4_1,
								sb0_1
							};
							case1 = new TestCaseData(picked_1, hovered_1, sbs_1, exp_1);
							yield return case1.SetName("FromHead_ToTail");
					}
				}				
			[TestCaseSource(typeof(SortedNewSBs_VariousCases))]
			public void SortedNewSBs_Various_CreatesAndReturnsSortedAndResizedSBsAccordingly(bool isExpandable, SGSorter sorter, List<ISlottable> source, List<ISlottable> expected){
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						sg.IsResizable().Returns(isExpandable);
					sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
					ISBHandler sbHandler = Substitute.For<ISBHandler>();
						sbHandler.GetSBs().Returns(new List<ISlottable>(source));
					sgTAHandler.SetSBHandler(sbHandler);
					ISorterHandler sorterHandler = new SorterHandler();
						sorterHandler.SetSorter(sorter);
					sgTAHandler.SetSorterHandler(sorterHandler);
				
				List<ISlottable> actual = sgTAHandler.SortedNewSBs();

				Assert.That(actual, Is.EqualTo(expected));
			}
				class SortedNewSBs_VariousCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData case0;
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
								partsSBB_0
							});
							case0 = new TestCaseData(
								true, 
								new SGItemIDSorter(),
								sbs_0,
								expected_0
							);
							yield return case0.SetName("itemOrder_Resize:	isExpandable = true");
						TestCaseData case1;
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
								bowSBA_1
							});
							case1 = new TestCaseData(
								true,
								new SGInverseItemIDSorter(),
								sbs_1, 
								expected_1
							);
							yield return case1.SetName("inverseIDOrder_Resize:	isExpandable = true");
						TestCaseData case2;
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
								mWeaponSBA_2
							});
							case2 = new TestCaseData(
								true,
								new SGAcquisitionOrderSorter(),
								sbs_2, 
								expected_2
							);
							yield return case2.SetName("acqOrder_Resize:		isExpandable = true");
						TestCaseData case3;
							ISlottable partsSBB_3 = MakeSubSBWithItem(MakePartsInstWithOrder(1, 1, 0));
							ISlottable bowSBA_3 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
							ISlottable wearSBA_3 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
							ISlottable quiverSBA_3 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 3));
							ISlottable bowSBC_3 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 4));
							ISlottable packSBA_3 = MakeSubSBWithItem(MakePackInstWithOrder(0, 5));
							ISlottable shieldSBA_3 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
							ISlottable partsSBA_3 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 7));
							ISlottable bowSBB_3 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
							ISlottable mWeaponSBA_3 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 9));
							List<ISlottable> sbs_3 = new List<ISlottable>(new ISlottable[]{
								quiverSBA_3,
								partsSBB_3,
								null,
								null,
								partsSBA_3,
								bowSBB_3,
								wearSBA_3,
								bowSBA_3,
								packSBA_3,
								null,
								shieldSBA_3,
								bowSBC_3,
								mWeaponSBA_3
							});
							List<ISlottable> expected_3 = new List<ISlottable>(new ISlottable[]{
								bowSBA_3,
								bowSBC_3,
								bowSBB_3,
								wearSBA_3,
								shieldSBA_3,
								mWeaponSBA_3,
								quiverSBA_3,
								packSBA_3,
								partsSBA_3,
								partsSBB_3,
								null,
								null,
								null
							});
							case3 = new TestCaseData(
								false, 
								new SGItemIDSorter(),
								sbs_3,
								expected_3
							);
							yield return case3.SetName("idOrder_NonResize:	isExpandable = false");
						TestCaseData case4;
							ISlottable partsSBB_4 = MakeSubSBWithItem(MakePartsInstWithOrder(1, 1, 0));
							ISlottable bowSBA_4 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
							ISlottable wearSBA_4 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
							ISlottable quiverSBA_4 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 3));
							ISlottable bowSBC_4 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 4));
							ISlottable packSBA_4 = MakeSubSBWithItem(MakePackInstWithOrder(0, 5));
							ISlottable shieldSBA_4 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
							ISlottable partsSBA_4 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 7));
							ISlottable bowSBB_4 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
							ISlottable mWeaponSBA_4 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 9));
							List<ISlottable> sbs_4 = new List<ISlottable>(new ISlottable[]{
								quiverSBA_4,
								partsSBB_4,
								null,
								null,
								partsSBA_4,
								bowSBB_4,
								wearSBA_4,
								bowSBA_4,
								packSBA_4,
								null,
								shieldSBA_4,
								bowSBC_4,
								mWeaponSBA_4
							});
							List<ISlottable> expected_4 = new List<ISlottable>(new ISlottable[]{
								partsSBB_4,
								partsSBA_4,
								packSBA_4,
								quiverSBA_4,
								mWeaponSBA_4,
								shieldSBA_4,
								wearSBA_4,
								bowSBB_4,
								bowSBC_4,
								bowSBA_4,
								null,
								null,
								null
							});
							case4 = new TestCaseData(
								false,
								new SGInverseItemIDSorter(),
								sbs_4, 
								expected_4
							);
							yield return case4.SetName("inverseIDOrder_NoResize:	isExpandable = false");
						TestCaseData case5;
							ISlottable partsSBB_5 = MakeSubSBWithItem(MakePartsInstWithOrder(1, 1, 0));
							ISlottable bowSBA_5 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
							ISlottable wearSBA_5 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
							ISlottable quiverSBA_5 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 3));
							ISlottable bowSBC_5 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 4));
							ISlottable packSBA_5 = MakeSubSBWithItem(MakePackInstWithOrder(0, 5));
							ISlottable shieldSBA_5 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
							ISlottable partsSBA_5 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 7));
							ISlottable bowSBB_5 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 8));
							ISlottable mWeaponSBA_5 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 9));
							List<ISlottable> sbs_5 = new List<ISlottable>(new ISlottable[]{
								quiverSBA_5,
								partsSBB_5,
								null,
								null,
								partsSBA_5,
								bowSBB_5,
								wearSBA_5,
								bowSBA_5,
								packSBA_5,
								null,
								shieldSBA_5,
								bowSBC_5,
								mWeaponSBA_5
							});
							List<ISlottable> expected_5 = new List<ISlottable>(new ISlottable[]{
								partsSBB_5,
								bowSBA_5,
								wearSBA_5,
								quiverSBA_5,
								bowSBC_5,
								packSBA_5,
								shieldSBA_5,
								partsSBA_5,
								bowSBB_5,
								mWeaponSBA_5,
								null,
								null,
								null
							});
							case5 = new TestCaseData(
								false,
								new SGAcquisitionOrderSorter(),
								sbs_5, 
								expected_5
							);
							yield return case5.SetName("acqOrder_NoResize:	isExpandable = false");
					}
				}
			[TestCaseSource(typeof(FilledNewSBsCases))]
			public void FilledNewSBs_Various_CreatesAndReturnsNewSBsAccordingly(List<ISlottable> source, bool isPool, bool isAdded, bool isAutoSort, bool isExpandable, List<ISlottable> expected, int addedIndex, IInventoryItemInstance addedItem){
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						sg.IsResizable().Returns(isExpandable);
						sg.IsPool().Returns(isPool);
					sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
					ISorterHandler sorterHandler = new SorterHandler();
						sorterHandler.SetSorter(new SGItemIDSorter());
						sorterHandler.SetIsAutoSort(isAutoSort);
					sgTAHandler.SetSorterHandler(sorterHandler);
					ISBHandler sbHandler = Substitute.For<ISBHandler>();
						sbHandler.GetSBs().Returns(new List<ISlottable>(source));
					sgTAHandler.SetSBHandler(sbHandler);
					ISlotSystemManager ssm = MakeSubSSM();
					ISBFactory sbFactory = new SBFactory(ssm);
					sgTAHandler.SetSBFactory(sbFactory);
					ITransactionSGHandler sgHandler = Substitute.For<ITransactionSGHandler>();
						if(!isAdded)
							sgHandler.GetSG1().Returns(sg);
						else
							sgHandler.GetSG1().Returns((ISlotGroup)null);
					sgTAHandler.SetSGHandler(sgHandler);
						ITransactionCache taCache = Substitute.For<ITransactionCache>();
						ISlottable pickedSB = MakeSubSBWithItem(addedItem);
						taCache.GetPickedSB().Returns(pickedSB);
						sg.GetTAC().Returns(taCache);
				
				List<ISlottable> actual = sgTAHandler.FilledNewSBs();

				List<ISlottable> exp = new List<ISlottable>(expected);
				if(isAdded){
					ISlottable addedSB = actual[addedIndex];
					Assert.That(addedSB, Is.Not.Null);
					AssertCreatedSB(addedSB, addedItem, ssm);
					exp[addedIndex] = addedSB;
				}
				Assert.That(actual, Is.EqualTo(exp));
			}
				class FilledNewSBsCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData case0;
							List<ISlottable> source_0;
								ISlottable partsSB_0 = MakeSubSBWithItem(MakePartsInstWithOrder(0, 1, 0));
								ISlottable bow2SB_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 1));
								ISlottable wearSB_0 = MakeSubSBWithItem(MakeWearInstWithOrder(0, 2));
								ISlottable mWeaponSB_0 = MakeSubSBWithItem(MakeMeleeWeaponInstWithOrder(0, 3));
								ISlottable packSB_0 = MakeSubSBWithItem(MakePackInstWithOrder(0, 4));
								ISlottable quiverSB_0 = MakeSubSBWithItem(MakeQuiverInstWithOrder(0, 5));
								ISlottable shieldSB_0 = MakeSubSBWithItem(MakeShieldInstWithOrder(0, 6));
								ISlottable bow0SB_0 = MakeSubSBWithItem(MakeBowInstWithOrder(0, 7));
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
							case0 = new TestCaseData(
								source_0, true, false, false, false, source_0, -1, MakeBowInstance(0)
							);
							yield return case0.SetName("NoUpdate:		isPool = true");
						TestCaseData case1;
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
							IInventoryItemInstance added_1 = bow1SB_1.GetItem();
							case1 = new TestCaseData(
								source_1, false, true, false, false, source_1, 3, added_1
							);
							yield return case1.SetName("Filled_NoSort:		isPool = false, isAutoSort = false, sg1 = sg");
						TestCaseData case2;
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
									bow2SB_2,
									bow0SB_2,
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
								IInventoryItemInstance added_2 = bow1SB_2.GetItem();
							case2 = new TestCaseData(
								source_2, false, true, true, false, exp_2, 2, added_2
							);
							yield return case2.SetName("Filled_Sort_NoResize:	isPool = false, isAutoSort = true, isExpandable = false, sg1 = sg");
						TestCaseData case3;
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
									bow2SB_3,
									bow0SB_3,
									null,
									wearSB_3,
									shieldSB_3,
									mWeaponSB_3,
									quiverSB_3,
									packSB_3,
									partsSB_3
								});
								IInventoryItemInstance added_3 = bow1SB_3.GetItem();
							case3 = new TestCaseData(
								source_3, false, true, true, true, exp_3, 2, added_3
							);
							yield return case3.SetName("Filled_Sort_Resize:	isPool = false, isAutoSort = true, isExpandable = true, sg1 = sg");
						TestCaseData case4;
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
							IInventoryItemInstance removed_4 = quiverSB_4.GetItem();
							case4 = new TestCaseData(
								source_4, false, false, false, false, exp_4, -1, removed_4
							);
							yield return case4.SetName("Nulled_NoSort:		isPool = false, isAutoSort = false, sg1 != sg");
						TestCaseData case5;
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
								IInventoryItemInstance removed_5 = bow0SB_5.GetItem();
							case5 = new TestCaseData(
								source_5, false, false, true, false, exp_5, -1, removed_5
							);
							yield return case5.SetName("Nulled_Sort_NoResize:	isPool = false, isAutoSort = true, isExpandable = false, sg1 != sg");
						TestCaseData case6;
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
								IInventoryItemInstance removed_6 = bow0SB_6.GetItem();
							case6 = new TestCaseData(
								source_6, false, false, true, true, exp_6, -1, removed_6
							);
							yield return case6.SetName("Nulled_Sort_Resize:	isPool = false, isAutoSort = true, isExpandable = true, sg1 != sg");
					}
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
			public void CreateNewSBAndFill_Always_UpdateList(List<ISlottable> source, ISlottable added, int addedIndex, List<ISlottable> expected){
				SGTransactionHandler sgTAHandler = new SGTransactionHandler(MakeSubSG(), MakeSubTAM());
					ISBFactory sbFactory;
						ISlotSystemManager ssm = MakeSubSSM();
						sbFactory = new SBFactory(ssm);
					sgTAHandler.SetSBFactory(sbFactory);

				
				List<ISlottable> actual = new List<ISlottable>(source);
				
				sgTAHandler.CreateNewSBAndFill(added.GetItem(), actual);

				ISlottable actualAdded = actual[addedIndex];
				AssertCreatedSB(actualAdded, added.GetItem(), ssm);
				expected[addedIndex] = actualAdded;
				Assert.That(actual, Is.EqualTo(expected));
			}
				class CreateNewSBAndFillCases:IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData case0;
							ISlottable sb0_0 = MakeSubSB();
							ISlottable sb1_0 = MakeSubSB();
							ISlottable sb2_0 = MakeSubSB();
							ISlottable sb3_0 = MakeSubSB();
							ISlottable added_0 = MakeSubSB();
								BowInstance bow_0 = MakeBowInstance(0);
								added_0.GetItem().Returns(bow_0);
							List<ISlottable> list_0 = new List<ISlottable>(new ISlottable[]{
								sb0_0, 
								sb1_0, 
								sb2_0, 
								sb3_0
							});
							List<ISlottable> exp_0 = new List<ISlottable>(new ISlottable[]{
								sb0_0, 
								sb1_0, 
								sb2_0, 
								sb3_0,
								added_0
							});
							case0 = new TestCaseData(
								list_0, added_0, 4, exp_0
							);
							yield return case0.SetName("Contatenated");
						TestCaseData case1;
							ISlottable sb0_1 = MakeSubSB();
							ISlottable sb1_1 = MakeSubSB();
							ISlottable sb2_1 = MakeSubSB();
							ISlottable sb3_1 = MakeSubSB();
							ISlottable added_1 = MakeSubSB();
								BowInstance bow_1 = MakeBowInstance(0);
								added_1.GetItem().Returns(bow_1);
							List<ISlottable> list_1 = new List<ISlottable>(new ISlottable[]{
								null,
								sb0_1, 
								null,
								null,
								sb1_1, 
								sb2_1, 
								sb3_1
							});
							List<ISlottable> exp_1 = new List<ISlottable>(new ISlottable[]{
								added_1,
								sb0_1, 
								null,
								null,
								sb1_1, 
								sb2_1, 
								sb3_1
							});
							case1 = new TestCaseData(list_1, added_1, 0, exp_1);
							yield return case1.SetName("FilledAtHead");
						TestCaseData case2;
							ISlottable sb0_2 = MakeSubSB();
							ISlottable sb1_2 = MakeSubSB();
							ISlottable sb2_2 = MakeSubSB();
							ISlottable sb3_2 = MakeSubSB();
							ISlottable added_2 = MakeSubSB();
								BowInstance bow_2 = MakeBowInstance(0);
								added_2.GetItem().Returns(bow_2);
							List<ISlottable> list_2 = new List<ISlottable>(new ISlottable[]{
								sb0_2, 
								sb1_2, 
								null,
								null,
								sb2_2, 
								null,
								sb3_2
							});
							List<ISlottable> exp_2 = new List<ISlottable>(new ISlottable[]{
								sb0_2, 
								sb1_2, 
								added_2,
								null,
								sb2_2, 
								null,
								sb3_2
							});
							case2 = new TestCaseData(list_2, added_2, 2, exp_2);
							yield return case2.SetName("FilledInTheMiddle");
						TestCaseData case_3;
							ISlottable added_3 = MakeSubSB();
								BowInstance bow_3 = MakeBowInstance(0);
								added_3.GetItem().Returns(bow_3);
							List<ISlottable> list_3 = new List<ISlottable>();
							List<ISlottable> exp_3 = new List<ISlottable>(new ISlottable[]{
								added_3
							});
							case_3 = new TestCaseData(list_3, added_3, 0, exp_3);
							yield return case_3.SetName("FilledInEmptyList");
						TestCaseData case_4;
							ISlottable added_4 = MakeSubSB();
								BowInstance bow_4 = MakeBowInstance(0);
								added_4.GetItem().Returns(bow_4);
							List<ISlottable> list_4 = new List<ISlottable>(new ISlottable[]{
								null,
								null,
								null,
								null,
								null,
								null,
								null
							});
							List<ISlottable> exp_4 = new List<ISlottable>(new ISlottable[]{
								added_4,
								null,
								null,
								null,
								null,
								null,
								null
							});
							case_4 = new TestCaseData(list_4, added_4, 0, exp_4);
							yield return case_4.SetName("FilledInAllNull");
					}
				}
			[TestCaseSource(typeof(NullifyIndexAtCases))]
			public void NullifyIndexAt_Always_FindByItemAndReplaceWithNull(List<ISlottable> source, IInventoryItemInstance item, IEnumerable<ISlottable> expected){
				SGTransactionHandler sgTAHandler = new SGTransactionHandler(MakeSubSG(), MakeSubTAM());
				List<ISlottable> actual = new List<ISlottable>(source);

				sgTAHandler.NullifyIndexAt(item, actual);

				Assert.That(actual, Is.EqualTo(expected));
			}
				class NullifyIndexAtCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData case0;
							ISlottable bowSB_0 = MakeSubSBWithItem(MakeBowInstance(0));
							ISlottable wearSB_0 = MakeSubSBWithItem(MakeWearInstance(0));
							ISlottable shieldSB_0 = MakeSubSBWithItem(MakeShieldInstance(0));
							ISlottable mWeaponSB_0 = MakeSubSBWithItem(MakeMWeaponInstance(0));
							List<ISlottable> list_0 = new List<ISlottable>(new ISlottable[]{
								null,
								bowSB_0,//nulled
								null,
								wearSB_0,
								shieldSB_0,
								null,
								null,
								mWeaponSB_0
							});
							IEnumerable<ISlottable> expected_0 = new ISlottable[]{
								null,
								null,//nulled
								null,
								wearSB_0,
								shieldSB_0,
								null,
								null,
								mWeaponSB_0
							};
							case0 = new TestCaseData(list_0, bowSB_0.GetItem(), expected_0);
							yield return case0.SetName("Valid");
						TestCaseData case1;
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
							case1 = new TestCaseData(list_1, null, expected_1);
							yield return case1.SetName("SilentIgnore: argument null");
					}
				}
			[TestCaseSource(typeof(SwapAndUpdateSBsCases))]
			public void SwappedNewSBs_Various_SetsNewSBsAccordingly(
				bool isPool, 
				bool isAutoSort, 
				bool isExpandable, 
				SGSorter sorter, 
				bool sg1This,
				ISlottable added, 
				ISlottable removed, 
				List<ISlottable> source, 
				List<ISlottable> expected, 
				int indexAtSwapped)
			{
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						sg.IsPool().Returns(isPool);
						sg.IsResizable().Returns(isExpandable);
					sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
					ISBHandler sbHandler = Substitute.For<ISBHandler>();
						sbHandler.GetSBs().Returns(new List<ISlottable>(source));
					sgTAHandler.SetSBHandler(sbHandler);
					ISorterHandler sorterHandler = new SorterHandler();
						sorterHandler.SetSorter(sorter);
						sorterHandler.SetIsAutoSort(isAutoSort);
					sgTAHandler.SetSorterHandler(sorterHandler);
					ISlotSystemManager ssm = MakeSubSSM();
					ISBFactory sbFactory = new SBFactory(ssm);
					sgTAHandler.SetSBFactory(sbFactory);
					ITransactionSGHandler sgHandler = Substitute.For<ITransactionSGHandler>();
						if(sg1This)
							sgHandler.GetSG1().Returns(sg);
						else
							sgHandler.GetSG1().Returns((ISlotGroup)null);
					sgTAHandler.SetSGHandler(sgHandler);
						ITransactionCache taCache = Substitute.For<ITransactionCache>();
						if(sg1This){
							taCache.GetPickedSB().Returns(removed);
							taCache.GetTargetSB().Returns(added);
						}
						else{
							taCache.GetPickedSB().Returns(added);
							taCache.GetTargetSB().Returns(removed);
						}
						sg.GetTAC().Returns(taCache);
				
				List<ISlottable> actual = sgTAHandler.SwappedNewSBs();
				
				List<ISlottable> expList = new List<ISlottable>(expected);
				if(!isPool){
					ISlottable actualAdded = actual[indexAtSwapped];
					expList[indexAtSwapped] = actualAdded;
				}
				Assert.That(actual, Is.EqualTo(expList));
			}
				class SwapAndUpdateSBsCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						SGSorter idSorter = new SGItemIDSorter();
						/* SBs */
						TestCaseData case0;
							ISlottable bowSB_0 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 0));
							ISlottable bow1SB_0 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 1));
							ISlottable bow2SB_0 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 2));
							ISlottable wearSB_0 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 3));
							ISlottable shieldSB_0 = MakeSubSBWithItemQuantitySettable(MakeShieldInstWithOrder(0, 4));
							ISlottable mWeaponSB_0 = MakeSubSBWithItemQuantitySettable(MakeMeleeWeaponInstWithOrder(0, 5));
							ISlottable quiverSB_0 = MakeSubSBWithItemQuantitySettable(MakeQuiverInstWithOrder(0, 6));
							ISlottable packSB_0 = MakeSubSBWithItemQuantitySettable(MakePackInstWithOrder(0, 7));
							ISlottable partsSB_0 = MakeSubSBWithItemQuantitySettable(MakePartsInstWithOrder(0, 1, 8));
							ISlottable wear1SB_0 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 9));
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
							case0 = new TestCaseData(
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
							);
							yield return case0.SetName("NoChange:			isPool = true, isAutoSort = false");
						TestCaseData case1;
							ISlottable bowSB_1 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 0));
							ISlottable bow1SB_1 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 1));
							ISlottable bow2SB_1 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 2));
							ISlottable wearSB_1 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 3));
							ISlottable shieldSB_1 = MakeSubSBWithItemQuantitySettable(MakeShieldInstWithOrder(0, 4));
							ISlottable mWeaponSB_1 = MakeSubSBWithItemQuantitySettable(MakeMeleeWeaponInstWithOrder(0, 5));
							ISlottable quiverSB_1 = MakeSubSBWithItemQuantitySettable(MakeQuiverInstWithOrder(0, 6));
							ISlottable packSB_1 = MakeSubSBWithItemQuantitySettable(MakePackInstWithOrder(0, 7));
							ISlottable partsSB_1 = MakeSubSBWithItemQuantitySettable(MakePartsInstWithOrder(0, 1, 8));
							ISlottable wear1SB_1 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 9));
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
							case1 = new TestCaseData(
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
							);
							yield return case1.SetName("NoSwap_Sort_NoResize:		isPool = true, isAutoSort = true, isExpandable = false");
						TestCaseData case2;
							ISlottable bowSB_2 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 0));
							ISlottable bow1SB_2 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 1));
							ISlottable bow2SB_2 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 2));
							ISlottable wearSB_2 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 3));
							ISlottable shieldSB_2 = MakeSubSBWithItemQuantitySettable(MakeShieldInstWithOrder(0, 4));
							ISlottable mWeaponSB_2 = MakeSubSBWithItemQuantitySettable(MakeMeleeWeaponInstWithOrder(0, 5));
							ISlottable quiverSB_2 = MakeSubSBWithItemQuantitySettable(MakeQuiverInstWithOrder(0, 6));
							ISlottable packSB_2 = MakeSubSBWithItemQuantitySettable(MakePackInstWithOrder(0, 7));
							ISlottable partsSB_2 = MakeSubSBWithItemQuantitySettable(MakePartsInstWithOrder(0, 1, 8));
							ISlottable wear1SB_2 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 9));
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
							case2 = new TestCaseData(
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
							);
							yield return case2.SetName("NoSwap_Sort_Resize:		isPool = true, isAutoSort = true, isExpandable = true");
						TestCaseData case3;
							ISlottable bowSB_3 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 0));
							ISlottable bow1SB_3 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 1));
							ISlottable bow2SB_3 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 2));
							ISlottable wearSB_3 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 3));
							ISlottable shieldSB_3 = MakeSubSBWithItemQuantitySettable(MakeShieldInstWithOrder(0, 4));
							ISlottable mWeaponSB_3 = MakeSubSBWithItemQuantitySettable(MakeMeleeWeaponInstWithOrder(0, 5));
							ISlottable quiverSB_3 = MakeSubSBWithItemQuantitySettable(MakeQuiverInstWithOrder(0, 6));
							ISlottable packSB_3 = MakeSubSBWithItemQuantitySettable(MakePackInstWithOrder(0, 7));
							ISlottable partsSB_3 = MakeSubSBWithItemQuantitySettable(MakePartsInstWithOrder(0, 1, 8));
							ISlottable wear1SB_3 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 9));
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
							case3 = new TestCaseData(
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
							);
							yield return case3.SetName("PickedRemoved_TargetAdded_NoSort:	isPool = false, isAutoSort = false, sg1 != sg");
						TestCaseData case4;
							ISlottable bowSB_4 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 0));
							ISlottable bow1SB_4 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 1));
							ISlottable bow2SB_4 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 2));
							ISlottable wearSB_4 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 3));
							ISlottable shieldSB_4 = MakeSubSBWithItemQuantitySettable(MakeShieldInstWithOrder(0, 4));
							ISlottable mWeaponSB_4 = MakeSubSBWithItemQuantitySettable(MakeMeleeWeaponInstWithOrder(0, 5));
							ISlottable quiverSB_4 = MakeSubSBWithItemQuantitySettable(MakeQuiverInstWithOrder(0, 6));
							ISlottable packSB_4 = MakeSubSBWithItemQuantitySettable(MakePackInstWithOrder(0, 7));
							ISlottable partsSB_4 = MakeSubSBWithItemQuantitySettable(MakePartsInstWithOrder(0, 1, 8));
							ISlottable wear1SB_4 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 9));
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
							case4 = new TestCaseData(
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
							);
							yield return case4.SetName("PickedAdded_TargetRemoved_NoSort:	isPool = false, isAutoSort = false, sg1 != sg");
						TestCaseData case5;
							ISlottable bowSB_5 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 0));
							ISlottable bow1SB_5 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 1));
							ISlottable bow2SB_5 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 2));
							ISlottable wearSB_5 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 3));
							ISlottable shieldSB_5 = MakeSubSBWithItemQuantitySettable(MakeShieldInstWithOrder(0, 4));
							ISlottable mWeaponSB_5 = MakeSubSBWithItemQuantitySettable(MakeMeleeWeaponInstWithOrder(0, 5));
							ISlottable quiverSB_5 = MakeSubSBWithItemQuantitySettable(MakeQuiverInstWithOrder(0, 6));
							ISlottable packSB_5 = MakeSubSBWithItemQuantitySettable(MakePackInstWithOrder(0, 7));
							ISlottable partsSB_5 = MakeSubSBWithItemQuantitySettable(MakePartsInstWithOrder(0, 1, 8));
							ISlottable wear1SB_5 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 9));
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
							case5 = new TestCaseData(
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
							);
							yield return case5.SetName("PickedRemoved_TargetAdded_Sort_NoResize:	isPool = false, isAutoSort = true, isExpandable = false, sg1 = sg");
						TestCaseData case6;
							ISlottable bowSB_6 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 0));
							ISlottable bow1SB_6 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 1));
							ISlottable bow2SB_6 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 2));
							ISlottable wearSB_6 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 3));
							ISlottable shieldSB_6 = MakeSubSBWithItemQuantitySettable(MakeShieldInstWithOrder(0, 4));
							ISlottable mWeaponSB_6 = MakeSubSBWithItemQuantitySettable(MakeMeleeWeaponInstWithOrder(0, 5));
							ISlottable quiverSB_6 = MakeSubSBWithItemQuantitySettable(MakeQuiverInstWithOrder(0, 6));
							ISlottable packSB_6 = MakeSubSBWithItemQuantitySettable(MakePackInstWithOrder(0, 7));
							ISlottable partsSB_6 = MakeSubSBWithItemQuantitySettable(MakePartsInstWithOrder(0, 1, 8));
							ISlottable wear1SB_6 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 9));
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
							case6 = new TestCaseData(
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
							);
							yield return case6.SetName("PickedAdded_TargetRemoved_Sort_NoResize:	isPool = false, isAutoSort = true, isExpandable = false, sg1 != sg");
						TestCaseData case7;
							ISlottable bowSB_7 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 0));
							ISlottable bow1SB_7 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 1));
							ISlottable bow2SB_7 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 2));
							ISlottable wearSB_7 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 3));
							ISlottable shieldSB_7 = MakeSubSBWithItemQuantitySettable(MakeShieldInstWithOrder(0, 4));
							ISlottable mWeaponSB_7 = MakeSubSBWithItemQuantitySettable(MakeMeleeWeaponInstWithOrder(0, 5));
							ISlottable quiverSB_7 = MakeSubSBWithItemQuantitySettable(MakeQuiverInstWithOrder(0, 6));
							ISlottable packSB_7 = MakeSubSBWithItemQuantitySettable(MakePackInstWithOrder(0, 7));
							ISlottable partsSB_7 = MakeSubSBWithItemQuantitySettable(MakePartsInstWithOrder(0, 1, 8));
							ISlottable wear1SB_7 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 9));
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
							case7 = new TestCaseData(
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
							);
							yield return case7.SetName("PickedRemoved_TargetAdded_Sort_Resize:	isPool = false, isAutoSort = true, isExpandable = true, sg1 = sg");
						TestCaseData case8;
							ISlottable bowSB_8 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 0));
							ISlottable bow1SB_8 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 1));
							ISlottable bow2SB_8 = MakeSubSBWithItemQuantitySettable(MakeBowInstWithOrder(0, 2));
							ISlottable wearSB_8 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 3));
							ISlottable shieldSB_8 = MakeSubSBWithItemQuantitySettable(MakeShieldInstWithOrder(0, 4));
							ISlottable mWeaponSB_8 = MakeSubSBWithItemQuantitySettable(MakeMeleeWeaponInstWithOrder(0, 5));
							ISlottable quiverSB_8 = MakeSubSBWithItemQuantitySettable(MakeQuiverInstWithOrder(0, 6));
							ISlottable packSB_8 = MakeSubSBWithItemQuantitySettable(MakePackInstWithOrder(0, 7));
							ISlottable partsSB_8 = MakeSubSBWithItemQuantitySettable(MakePartsInstWithOrder(0, 1, 8));
							ISlottable wear1SB_8 = MakeSubSBWithItemQuantitySettable(MakeWearInstWithOrder(0, 9));
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
							case8 = new TestCaseData(
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
							);
							yield return case8.SetName("PickedAdded_TargetRemoved_Sort_Resize:	isPool = false, isAutoSort = true, isExpandable = true, sg1 != sg");
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
			[TestCaseSource(typeof(AddedNewSBs_VariousCases))]
			public void AddedNewSBs_Various_Various(
				bool isAutoSort, 
				bool isExpandable, 
				SGSorter sorter,  
				List<ISlottable> source, 
				List<IInventoryItemInstance> added,
				List<ISlottable> expected)
			{
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						sg.IsResizable().Returns(isExpandable);
					sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
					ISBHandler sbHandler = Substitute.For<ISBHandler>();
						sbHandler.GetSBs().Returns(new List<ISlottable>(source));
					sgTAHandler.SetSBHandler(sbHandler);
					ISBFactory sbFactory = new SBFactory(MakeSubSSM());
					sgTAHandler.SetSBFactory(sbFactory);
						ITransactionCache taCache = Substitute.For<ITransactionCache>();
						taCache.GetMoved().Returns(added);
						sg.GetTAC().Returns(taCache);
					ISorterHandler sorterHandler = new SorterHandler();
						sorterHandler.SetSorter(sorter);
						sorterHandler.SetIsAutoSort(isAutoSort);
					sgTAHandler.SetSorterHandler(sorterHandler);

					
				List<ISlottable> actual = sgTAHandler.AddedNewSBs();

				AssertSBsItemAndQuantityEquality(actual, expected);

				ResetQuantity(actual);
			}
				class AddedNewSBs_VariousCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData addedNoSort_isPoolF;
							ISlottable bowSB_0 = MakeSubSBWithItemQuantitySettable(MakeBowInstance(0));
							ISlottable wearSB_0 = MakeSubSBWithItemQuantitySettable(MakeWearInstance(0));
							ISlottable shieldSB_0 = MakeSubSBWithItemQuantitySettable(MakeShieldInstance(0));
							ISlottable mWeaponSB_0 = MakeSubSBWithItemQuantitySettable(MakeMWeaponInstance(0));
							ISlottable partsSB_0 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 1));
							ISlottable parts1SB_0 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 2));
							ISlottable bow1SB_a_0 = MakeSubSBWithItemQuantitySettable(MakeBowInstance(1));
							ISlottable quiverSB_a_0 = MakeSubSBWithItemQuantitySettable(MakeQuiverInstance(1));
							ISlottable partsSB_a_0 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 6));
							ISlottable parts1SB_a_0 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 3));
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
							List<IInventoryItemInstance> added_0 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
								bow1SB_a_0.GetItem(),
								quiverSB_a_0.GetItem(),
								partsSB_a_0.GetItem(),
								parts1SB_a_0.GetItem() 
							});
							ISlottable addedPartsSB_0 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 7));
							ISlottable addedParts1SB_0 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 5));
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
							addedNoSort_isPoolF = new TestCaseData(
								false, 
								false, 
								idSorter_0, 
								sbs_0, 
								added_0,
								exp_0
							);
							yield return addedNoSort_isPoolF.SetName("Added_NoSort:		isAutoSort == false");
						TestCaseData addedSortNoResize_isAutoSort;
							ISlottable bowSB_1 = MakeSubSBWithItemQuantitySettable(MakeBowInstance(0));
							ISlottable wearSB_1 = MakeSubSBWithItemQuantitySettable(MakeWearInstance(0));
							ISlottable shieldSB_1 = MakeSubSBWithItemQuantitySettable(MakeShieldInstance(0));
							ISlottable mWeaponSB_1 = MakeSubSBWithItemQuantitySettable(MakeMWeaponInstance(0));
							ISlottable partsSB_1 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 1));
								partsSB_1.ItemHandler().Quantity().Returns(1);
							ISlottable parts1SB_1 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 2));
								parts1SB_1.ItemHandler().Quantity().Returns(2);
							ISlottable bow1SB_a_1 = MakeSubSBWithItemQuantitySettable(MakeBowInstance(1));
							ISlottable quiverSB_a_1 = MakeSubSBWithItemQuantitySettable(MakeQuiverInstance(1));
							ISlottable partsSB_a_1 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 6));
								partsSB_a_1.ItemHandler().Quantity().Returns(6);
							ISlottable parts1SB_a_1 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 3));
								parts1SB_a_1.ItemHandler().Quantity().Returns(3);
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
							List<IInventoryItemInstance> added_1 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
								bow1SB_a_1.GetItem(),
								quiverSB_a_1.GetItem(),
								partsSB_a_1.GetItem(),
								parts1SB_a_1.GetItem() 
							});
							ISlottable addedPartsSB_1 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 7));
							ISlottable addedParts1SB_1 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 5));
							List<ISlottable> exp_1 = new List<ISlottable>(new ISlottable[]{
								bowSB_1,
								bow1SB_a_1,
								wearSB_1,
								shieldSB_1,
								mWeaponSB_1,
								quiverSB_a_1,
								addedPartsSB_1,
								addedParts1SB_1,
								null,
								null,
								null
							});
							addedSortNoResize_isAutoSort = new TestCaseData(
								true,
								false,
								idSorter_1,
								sbs_1,
								added_1,
								exp_1
							);
							yield return addedSortNoResize_isAutoSort.SetName("Added_Sort_NoResize:	isAutoSort == true, isExpandable == false");
						TestCaseData addedSortResize_isAutoSort_isExpandable;
							ISlottable bowSB_2 = MakeSubSBWithItemQuantitySettable(MakeBowInstance(0));
							ISlottable wearSB_2 = MakeSubSBWithItemQuantitySettable(MakeWearInstance(0));
							ISlottable shieldSB_2 = MakeSubSBWithItemQuantitySettable(MakeShieldInstance(0));
							ISlottable mWeaponSB_2 = MakeSubSBWithItemQuantitySettable(MakeMWeaponInstance(0));
							ISlottable partsSB_2 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 1));
								partsSB_2.ItemHandler().Quantity().Returns(1);
							ISlottable parts1SB_2 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 2));
								parts1SB_2.ItemHandler().Quantity().Returns(2);
							ISlottable bow1SB_a_2 = MakeSubSBWithItemQuantitySettable(MakeBowInstance(1));
							ISlottable quiverSB_a_2 = MakeSubSBWithItemQuantitySettable(MakeQuiverInstance(1));
							ISlottable partsSB_a_2 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 6));
								partsSB_a_2.ItemHandler().Quantity().Returns(6);
							ISlottable parts1SB_a_2 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 3));
								parts1SB_a_2.ItemHandler().Quantity().Returns(3);
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
							List<IInventoryItemInstance> added_2 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
								bow1SB_a_2.GetItem(),
								quiverSB_a_2.GetItem(),
								partsSB_a_2.GetItem(),
								parts1SB_a_2.GetItem() 
							});
							ISlottable addedPartsSB_2 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 7));
							ISlottable addedParts1SB_2 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 5));
							List<ISlottable> exp_2 = new List<ISlottable>(new ISlottable[]{
								bowSB_2,
								bow1SB_a_2,
								wearSB_2,
								shieldSB_2,
								mWeaponSB_2,
								quiverSB_a_2,
								addedPartsSB_2,
								addedParts1SB_2
							});
							addedSortResize_isAutoSort_isExpandable = new TestCaseData(
								true,
								true,
								idSorter_2,
								sbs_2,
								added_2,
								exp_2
							);
							yield return addedSortResize_isAutoSort_isExpandable.SetName("Added_Sort_Resize:	isAutoSort == true, isExpandable == true");
					}
				}
			[TestCaseSource(typeof(IncreasedSBsCases))]
			public void IncreasedSBs_Always_IncreaseQuantityOrCreateAndFill(List<IInventoryItemInstance> added, List<ISlottable> source, List<ISlottable> expected){
				SGTransactionHandler sgTAHandler;
					sgTAHandler = new SGTransactionHandler(MakeSubSG(), MakeSubTAM());
					ISBFactory sbFactory = new SBFactory(MakeSubSSM());
					sgTAHandler.SetSBFactory(sbFactory);
				
				List<ISlottable> actual = sgTAHandler.IncreasedSBs(added, source);

				AssertSBsItemAndQuantityEquality(actual, expected);

				ResetQuantity(actual);
			}
				class IncreasedSBsCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData increased;
							PartsInstance parts0_0 = MakePartsInstance(0, 20);
							PartsInstance parts1_0 = MakePartsInstance(1, 4);
							PartsInstance parts2_0 = MakePartsInstance(2, 9);
							PartsInstance parts3_0 = MakePartsInstance(3, 15);
							ISlottable sb0_0 = MakeSubSBWithItemQuantitySettable(parts0_0);
							ISlottable sb1_0 = MakeSubSBWithItemQuantitySettable(parts1_0);
							ISlottable sb2_0 = MakeSubSBWithItemQuantitySettable(parts2_0);
							ISlottable sb3_0 = MakeSubSBWithItemQuantitySettable(parts3_0);
							List<ISlottable> source_0 = new List<ISlottable>(new ISlottable[]{
								sb2_0,
								null,
								null,
								sb0_0,
								sb3_0,
								null,
								null,
								sb1_0,
								null
							});
							PartsInstance partsA0_0 = MakePartsInstance(0, 3);
							PartsInstance partsA1_0 = MakePartsInstance(1, 7);
							PartsInstance partsA2_0 = MakePartsInstance(2, 10);
							PartsInstance partsA3_0 = MakePartsInstance(3, 2);
							List<IInventoryItemInstance> added_0 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
								partsA0_0,
								partsA1_0,
								partsA2_0,
								partsA3_0
							});
							PartsInstance partsAA0_0 = MakePartsInstance(0, 23);
							PartsInstance partsAA1_0 = MakePartsInstance(1, 11);
							PartsInstance partsAA2_0 = MakePartsInstance(2, 19);
							PartsInstance partsAA3_0 = MakePartsInstance(3, 17);
							List<ISlottable> exp_0 = new List<ISlottable>(new ISlottable[]{
								MakeSubSBWithItemQuantitySettable(partsAA2_0),
								null,
								null,
								MakeSubSBWithItemQuantitySettable(partsAA0_0),
								MakeSubSBWithItemQuantitySettable(partsAA3_0),
								null,
								null,
								MakeSubSBWithItemQuantitySettable(partsAA1_0),
								null
							});
							increased = new TestCaseData(
								added_0, source_0, exp_0
							);
							yield return increased.SetName("Increased");
						TestCaseData created;
							BowInstance bow_1 = MakeBowInstance(0);
							WearInstance wear_1 = MakeWearInstance(0);
							ShieldInstance shield_1 = MakeShieldInstance(0);
							MeleeWeaponInstance mWeapon_1 = MakeMWeaponInstance(0);
							QuiverInstance quiver_1 = MakeQuiverInstance(0);
							PackInstance pack_1 = MakePackInstance(0);
							ISlottable bowSB_1 = MakeSubSBWithItemQuantitySettable(bow_1);
							ISlottable wearSB_1 = MakeSubSBWithItemQuantitySettable(wear_1);
							ISlottable shieldSB_1 = MakeSubSBWithItemQuantitySettable(shield_1);
							ISlottable mWeaponSB_1 = MakeSubSBWithItemQuantitySettable(mWeapon_1);
							ISlottable quiverSB_1 = MakeSubSBWithItemQuantitySettable(quiver_1);
							ISlottable packSB_1 = MakeSubSBWithItemQuantitySettable(pack_1);
							List<ISlottable> source_1 = new List<ISlottable>(new ISlottable[]{
								null,
								wearSB_1,
								packSB_1,
								mWeaponSB_1,
								null,
								shieldSB_1,
								bowSB_1,
								null,
								null,
								quiverSB_1
							});
							PackInstance packA_1 = MakePackInstance(0);
							QuiverInstance quiverA_1 = MakeQuiverInstance(0);
							MeleeWeaponInstance mWeaponA_1 = MakeMWeaponInstance(0);
							ShieldInstance shieldA_1 = MakeShieldInstance(0);
							List<IInventoryItemInstance> added_1 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
								packA_1,
								quiverA_1,
								mWeaponA_1,
								shieldA_1
							});
							List<ISlottable> exp_1 = new List<ISlottable>(new ISlottable[]{
								MakeSubSBWithItemQuantitySettable(packA_1),
								wearSB_1,
								packSB_1,
								mWeaponSB_1,
								MakeSubSBWithItemQuantitySettable(quiverA_1),
								shieldSB_1,
								bowSB_1,
								MakeSubSBWithItemQuantitySettable(mWeaponA_1),
								MakeSubSBWithItemQuantitySettable(shieldA_1),
								quiverSB_1
							});
							created = new TestCaseData(
								added_1, source_1, exp_1
							);
							yield return created.SetName("Created");
					}
				}
			[TestCaseSource(typeof(RemovedNewSBs_VariousCases))]
			public void RemovedNewSBs_Various_Various(
				bool isPool,
				bool isAutoSort, 
				bool isExpandable, 
				SGSorter sorter,  
				List<ISlottable> source, 
				List<IInventoryItemInstance> removed,
				List<ISlottable> expected)
			{
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						sg.IsPool().Returns(isPool);
						sg.IsResizable().Returns(isExpandable);
					sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
					ISBHandler sbHandler = Substitute.For<ISBHandler>();
						sbHandler.GetSBs().Returns(new List<ISlottable>(source));
					sgTAHandler.SetSBHandler(sbHandler);
					ISorterHandler sorterHandler = new SorterHandler();
						sorterHandler.SetSorter(sorter);
						sorterHandler.SetIsAutoSort(isAutoSort);
					sgTAHandler.SetSorterHandler(sorterHandler);
						ITransactionCache taCache = Substitute.For<ITransactionCache>();
						taCache.GetMoved().Returns(removed);
						sg.GetTAC().Returns(taCache);
				
				List<ISlottable> actual = sgTAHandler.RemovedNewSBs();
				
				AssertSBsItemAndQuantityEquality(actual, expected);

				ResetQuantity(actual);
			}
				class RemovedNewSBs_VariousCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData removedNoSort;
							ISlottable bowSB_1 = MakeSubSBWithItemQuantitySettable(MakeBowInstance(0));
							ISlottable wearSB_1 = MakeSubSBWithItemQuantitySettable(MakeWearInstance(0));
							ISlottable shieldSB_1 = MakeSubSBWithItemQuantitySettable(MakeShieldInstance(0));
							ISlottable mWeaponSB_1 = MakeSubSBWithItemQuantitySettable(MakeMWeaponInstance(0));
							ISlottable partsSB_1 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 1));
							ISlottable parts1SB_1 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 2));
							ISlottable partsSB_a_1 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 1));
							ISlottable parts1SB_a_1 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 1));
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
							List<IInventoryItemInstance> removed_1 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
								wearSB_1.GetItem(),
								shieldSB_1.GetItem(),
								partsSB_a_1.GetItem(),
								parts1SB_a_1.GetItem() 
							});
							ISlottable removedParts1SB_1 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 1));
							List<ISlottable> exp_1 = new List<ISlottable>(new ISlottable[]{
								removedParts1SB_1,
								null,
								null,
								mWeaponSB_1,
								null,
								null,
								null,
								null,
								bowSB_1,
								null,
								null
							});
							removedNoSort = new TestCaseData(
								false,
								false, 
								false,
								idSorter_1, 
								sbs_1, 
								removed_1, 
								exp_1
							);
							yield return removedNoSort.SetName("Removed_NoSort:		isPool == false, isAutoSort == false");
						TestCaseData removedSortNoResize_isAutoSort;
							ISlottable bowSB_2 = MakeSubSBWithItemQuantitySettable(MakeBowInstance(0));
							ISlottable wearSB_2 = MakeSubSBWithItemQuantitySettable(MakeWearInstance(0));
							ISlottable shieldSB_2 = MakeSubSBWithItemQuantitySettable(MakeShieldInstance(0));
							ISlottable mWeaponSB_2 = MakeSubSBWithItemQuantitySettable(MakeMWeaponInstance(0));
							ISlottable partsSB_2 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 1));
							ISlottable parts1SB_2 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 2));
							ISlottable partsSB_a_2 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 1));
							ISlottable parts1SB_a_2 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 1));
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
							List<IInventoryItemInstance> removed_2 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
								wearSB_2.GetItem(),
								shieldSB_2.GetItem(),
								partsSB_a_2.GetItem(),
								parts1SB_a_2.GetItem() 
							});
							ISlottable removedParts1SB_2 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 1));
							List<ISlottable> exp_2 = new List<ISlottable>(new ISlottable[]{
								bowSB_2,
								mWeaponSB_2,
								removedParts1SB_2,
								null,
								null,
								null,
								null,
								null,
								null,
								null,
								null
							});
							removedSortNoResize_isAutoSort = new TestCaseData(
								false,
								true, 
								false, 
								idSorter_2, 
								sbs_2, 
								removed_2, 
								exp_2
							);
							yield return removedSortNoResize_isAutoSort.SetName("Removed_Sort_NoResize:	isPool == false, isAutoSort == true, isExpandable == false");
						TestCaseData removedSortResize_isAutoSort_isExpandable;
							ISlottable bowSB_3 = MakeSubSBWithItemQuantitySettable(MakeBowInstance(0));
							ISlottable wearSB_3 = MakeSubSBWithItemQuantitySettable(MakeWearInstance(0));
							ISlottable shieldSB_3 = MakeSubSBWithItemQuantitySettable(MakeShieldInstance(0));
							ISlottable mWeaponSB_3 = MakeSubSBWithItemQuantitySettable(MakeMWeaponInstance(0));
							ISlottable partsSB_3 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 1));
							ISlottable parts1SB_3 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 2));
							ISlottable partsSB_a_3 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(0, 1));
							ISlottable parts1SB_a_3 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 1));
							List<ISlottable> sbs_3 = new List<ISlottable>(new ISlottable[]{
								parts1SB_3,
								wearSB_3,
								null,
								mWeaponSB_3,
								null,
								partsSB_3,
								null,
								shieldSB_3,
								bowSB_3,
								null,
								null
							});
							SGSorter idSorter_3 = new SGItemIDSorter();
							List<IInventoryItemInstance> removed_3 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
								wearSB_3.GetItem(),
								shieldSB_3.GetItem(),
								partsSB_a_3.GetItem(),
								parts1SB_a_3.GetItem() 
							});
							ISlottable removedParts1SB_3 = MakeSubSBWithItemQuantitySettable(MakePartsInstance(1, 1));
							List<ISlottable> exp_3 = new List<ISlottable>(new ISlottable[]{
								bowSB_3,
								mWeaponSB_3,
								removedParts1SB_3
							});
							removedSortResize_isAutoSort_isExpandable = new TestCaseData(
								false,
								true, 
								true, 
								idSorter_3, 
								sbs_3, 
								removed_3, 
								exp_3
							);
							yield return removedSortResize_isAutoSort_isExpandable.SetName("Removed_Sort_Resize:	isPool == false, isAutoSort == true, isExpandable == true");
					}
				}
			[TestCaseSource(typeof(UpdateSBsCases))]
			public void UpdateSBs_Always_SetsSBsToNewSlotsAndUpdateSlotIDs(List<ISlottable> source, IEnumerable<ISlottable> expected){
				SGTransactionHandler sgTAHandler = new SGTransactionHandler(MakeSubSG(), MakeSubTAM());
					ISlotsHolder slotsHolder = new SlotsHolder(MakeSubSG());
					sgTAHandler.SetSlotsHolder(slotsHolder);
					ISBHandler sbHandler = new SBHandler();
						sbHandler.SetSBs(new List<ISlottable>(source));
					sgTAHandler.SetSBHandler(sbHandler);
				
				sgTAHandler.UpdateSBs();

				List<ISlottable> actual = ExtractSBsFromSlots(slotsHolder.GetSlots());

				Assert.That(actual, Is.EqualTo(expected));
				foreach(var sb in actual){
					if(sb != null){
						int index = actual.IndexOf(sb);
						sb.Received().SetSlotID(index);
					}
				}
			}
				class UpdateSBsCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData case0;
							ISlottable sb0_0 = MakeSubSB();
							ISlottable sb1_0 = MakeSubSB();
							ISlottable sb2_0 = MakeSubSB();
							ISlottable sb3_0 = MakeSubSB();
							sb0_0.IsToBeRemoved().Returns(false);
							sb1_0.IsToBeRemoved().Returns(false);
							sb2_0.IsToBeRemoved().Returns(false);
							sb3_0.IsToBeRemoved().Returns(false);
							sb0_0.GetNewSlotID().Returns(0);
							sb1_0.GetNewSlotID().Returns(1);
							sb2_0.GetNewSlotID().Returns(2);
							sb3_0.GetNewSlotID().Returns(3);
							List<ISlottable> source_0 = new List<ISlottable>(new ISlottable[]{
								sb1_0,
								SBToBeRemoved(),
								null,
								SBToBeRemoved(),
								sb3_0,
								null,
								sb0_0,
								null,
								sb2_0,
								SBToBeRemoved(),
								null
							});
							IEnumerable<ISlottable> expected_0 = new ISlottable[]{
								sb0_0,
								sb1_0,
								sb2_0,
								sb3_0,
								null,
								null,
								null,
								null,
								null,
								null,
								null

							};
							case0 = new TestCaseData(
								source_0, expected_0
							);
							yield return case0.SetName("Valid");
					}
				}
			[TestCaseSource(typeof(RemoveAndDestroySBsFromCases))]
			public void RemoveAndDestroySBsFrom_Always_CallsTargetSBsDestroy(List<ISlottable> source, IEnumerable<ISlottable> xRemoved, IEnumerable<ISlottable> xNotRemoved){
				SGTransactionHandler sgTAHandler = new SGTransactionHandler(MakeSubSG(), MakeSubTAM());

				sgTAHandler.RemoveAndDestorySBsFrom(new List<ISlottable>(source));

				foreach(var sb in xRemoved)
					sb.Received().Destroy();
				foreach(var sb in xNotRemoved)
					sb.DidNotReceive().Destroy();
			}
				class RemoveAndDestroySBsFromCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData allRemoved;
							ISlottable sb0_0 = MakeSubSB();
							ISlottable sb1_0 = MakeSubSB();
							ISlottable sb2_0 = MakeSubSB();
							ISlottable sb3_0 = MakeSubSB();
							sb0_0.IsToBeRemoved().Returns(true);
							sb1_0.IsToBeRemoved().Returns(true);
							sb2_0.IsToBeRemoved().Returns(true);
							sb3_0.IsToBeRemoved().Returns(true);
							List<ISlottable> source_0 = new List<ISlottable>(new ISlottable[]{
								sb0_0,
								sb1_0,
								sb2_0,
								sb3_0
							});
							IEnumerable<ISlottable> xRemoved_0 = source_0;
							IEnumerable<ISlottable> xNotRemoved_0 = new ISlottable[0];
							allRemoved = new TestCaseData(
								source_0, xRemoved_0, xNotRemoved_0
							);
							yield return allRemoved.SetName("AllRemoved");
						TestCaseData allNotRemoved;
							ISlottable sb0_1 = MakeSubSB();
							ISlottable sb1_1 = MakeSubSB();
							ISlottable sb2_1 = MakeSubSB();
							ISlottable sb3_1 = MakeSubSB();
							sb0_1.IsToBeRemoved().Returns(false);
							sb1_1.IsToBeRemoved().Returns(false);
							sb2_1.IsToBeRemoved().Returns(false);
							sb3_1.IsToBeRemoved().Returns(false);
							List<ISlottable> source_1 = new List<ISlottable>(new ISlottable[]{
								sb0_1,
								sb1_1,
								sb2_1,
								sb3_1
							});
							IEnumerable<ISlottable> xRemoved_1 = new ISlottable[0];
							IEnumerable<ISlottable> xNotRemoved_1 = source_1;
							allNotRemoved = new TestCaseData(
								source_1, xRemoved_1, xNotRemoved_1
							);
							yield return allNotRemoved.SetName("AllNotRemoved");
						TestCaseData mixed;
							ISlottable sb0_2 = MakeSubSB();
							ISlottable sb1_2 = MakeSubSB();
							ISlottable sb2_2 = MakeSubSB();
							ISlottable sb3_2 = MakeSubSB();
							sb0_2.IsToBeRemoved().Returns(false);
							sb1_2.IsToBeRemoved().Returns(true);
							sb2_2.IsToBeRemoved().Returns(true);
							sb3_2.IsToBeRemoved().Returns(false);
							List<ISlottable> source_2 = new List<ISlottable>(new ISlottable[]{
								sb0_2,
								sb1_2,
								sb2_2,
								sb3_2
							});
							IEnumerable<ISlottable> xRemoved_2 = new ISlottable[]{
								sb1_2, sb2_2
							};
							IEnumerable<ISlottable> xNotRemoved_2 = new ISlottable[]{
								sb0_2, sb3_2
							};
							mixed = new TestCaseData(
								source_2, xRemoved_2, xNotRemoved_2
							);
							yield return mixed.SetName("Mixed");
					}
				}
			[TestCaseSource(typeof(RemoveAndDestorySBsFromCasesV2))]
			public void RemoveAndDestorySBsFrom_Always_NullifiesSBsIndexAtRemoved(List<ISlottable> source, IEnumerable<ISlottable> expected){
				SGTransactionHandler sgTAHandler = new SGTransactionHandler(MakeSubSG(), MakeSubTAM());

				sgTAHandler.RemoveAndDestorySBsFrom(source);

				Assert.That(source, Is.EqualTo(expected));
			}
				class RemoveAndDestorySBsFromCasesV2: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData allRemoved;
							List<ISlottable> source_0 = new List<ISlottable>(new ISlottable[]{
								SBToBeRemoved(),
								null,
								null,
								null,
								SBToBeRemoved(),
								SBToBeRemoved(),
								null,
								SBToBeRemoved(),
								null
							});
							IEnumerable<ISlottable> expected_0 = new ISlottable[]{
								null,
								null,
								null,
								null,
								null,
								null,
								null,
								null,
								null
							};
							allRemoved = new TestCaseData(
								source_0, expected_0
							);
							yield return allRemoved.SetName("AllRemoved");
						TestCaseData nonRemoved;
							ISlottable sb0_1 = SBNotToBeRemoved();
							ISlottable sb1_1 = SBNotToBeRemoved();
							ISlottable sb2_1 = SBNotToBeRemoved();
							ISlottable sb3_1 = SBNotToBeRemoved();
							List<ISlottable> source_1 = new List<ISlottable>(new ISlottable[]{
								null,
								sb3_1,
								null,
								null,
								null,
								sb0_1,
								sb1_1,
								null,
								sb2_1,
							});
							IEnumerable<ISlottable> expected_1 = new ISlottable[]{
								null,
								sb3_1,
								null,
								null,
								null,
								sb0_1,
								sb1_1,
								null,
								sb2_1,
							};
							nonRemoved = new TestCaseData(
								source_1, expected_1
							);
							yield return nonRemoved.SetName("AllNotRemoved");
						TestCaseData mixed;
							ISlottable sb0_2 = SBNotToBeRemoved();
							ISlottable sb1_2 = SBNotToBeRemoved();
							ISlottable sb2_2 = SBNotToBeRemoved();
							ISlottable sb3_2 = SBNotToBeRemoved();
							List<ISlottable> source_2 = new List<ISlottable>(new ISlottable[]{
								SBToBeRemoved(),
								sb3_2,
								null,
								null,
								SBToBeRemoved(),
								sb0_2,
								sb1_2,
								SBToBeRemoved(),
								sb2_2,
							});
							IEnumerable<ISlottable> expected_2 = new ISlottable[]{
								null,
								sb3_2,
								null,
								null,
								null,
								sb0_2,
								sb1_2,
								null,
								sb2_2,
							};
							mixed = new TestCaseData(
								source_2, expected_2
							);
							yield return mixed.SetName("Mixed");
					}
				}

			[TestCaseSource(typeof(SortedSBsIfAutoSortAccordingToExpandablityCases))]
			public void SortedSBsIfAutoSortAccordingToExpandablity_Always_ReturnsAccordingly(bool isAutoSort, bool isExpandable, List<ISlottable> source ,List<ISlottable> expected){
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						sg.IsResizable().Returns(isExpandable);
						sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());
					ISorterHandler sorterHandler = new SorterHandler();
						sorterHandler.SetSorter(new SGItemIDSorter());
						sorterHandler.SetIsAutoSort(isAutoSort);
					sgTAHandler.SetSorterHandler(sorterHandler);
					ISBHandler sbHandler = Substitute.For<ISBHandler>();
						sbHandler.GetSBs().Returns(source);
					sgTAHandler.SetSBHandler(sbHandler);

				List<ISlottable> actual = sgTAHandler.SortedSBsIfAutoSortAccordingToExpandability(source);

				Assert.That(actual, Is.EqualTo(expected));
			}
				class SortedSBsIfAutoSortAccordingToExpandablityCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData noChange_isAutoSortF;
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
							noChange_isAutoSortF = new TestCaseData(
								false,
								false,
								source_0,
								source_0
							);
							yield return noChange_isAutoSortF.SetName("NoChange:	isAutoSort = false");
						TestCaseData sortNoResize_isAutoSortT_isExpandableF;
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
							sortNoResize_isAutoSortT_isExpandableF = new TestCaseData(
								true,
								false,
								source_1,
								exp_1
							);
							yield return sortNoResize_isAutoSortT_isExpandableF.SetName("Sort_NoResize:	isAutoSort = true, isExpandable = false");
						TestCaseData sortResize_isAutoSortT_isExpandableT;
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
							sortResize_isAutoSortT_isExpandableT = new TestCaseData(
								true,
								true,
								source_2,
								exp_2
							);
							yield return sortResize_isAutoSortT_isExpandableT.SetName("Sort_Resize:	isAutoSort = True, isExpandable = true");
					}
				}
			[TestCaseSource(typeof(SetSBsFromSlotsAndUpdateSlotIDsCases))]
			public void SetSBsFromSlotsAndUpdateSlotIDs_Always_UpdatesSlottables(List<Slot> slots, List<ISlottable> expected){
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						ISBHandler sbHandler = Substitute.For<ISBHandler>();
						sg.GetSBHandler().Returns(sbHandler);
						sbHandler.GetSBs().Returns(new List<ISlottable>());
						ISlotsHolder slotsHolder = Substitute.For<ISlotsHolder>();
							slotsHolder.GetSlots().Returns(slots);
						sg.GetSlotsHolder().Returns(slotsHolder);
						sbHandler.When(x => x.SetSBs(Arg.Is<List<ISlottable>>(y => y.MemberEquals(expected)))).Do(z => sbHandler.GetSBs().Returns(expected));
				sgTAHandler = new SGTransactionHandler(sg, MakeSubTAM());

				sgTAHandler.SetSBsFromSlotsAndUpdateSlotIDs();

				List<ISlottable> actual = sbHandler.GetSBs();
				Assert.That(actual, Is.EqualTo(expected));
				foreach(var sb in actual)
					sb.Received().SetSlotID(actual.IndexOf(sb));
			}
				class SetSBsFromSlotsAndUpdateSlotIDsCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData case0;
							List<Slot> slots_0;
								ISlottable sb0_0 = MakeSubSB();
								ISlottable sb1_0 = MakeSubSB();
								ISlottable sb2_0 = MakeSubSB();
								ISlottable sb3_0 = MakeSubSB();
								Slot slot0_0 = new Slot();
									slot0_0.sb = sb0_0;
								Slot slot1_0 = new Slot();
									slot1_0.sb = sb1_0;
								Slot slot2_0 = new Slot();
									slot2_0.sb = sb2_0;
								Slot slot3_0 = new Slot();
									slot3_0.sb = sb3_0;
								slots_0 = new List<Slot>(new Slot[]{
									slot0_0,
									slot1_0,
									slot2_0,
									slot3_0
								});
							List<ISlottable> expected_0 = new List<ISlottable>(new ISlottable[]{
									sb0_0,
									sb1_0,
									sb2_0,
									sb3_0
							});
						case0 = new TestCaseData(
							slots_0, expected_0
						);
						yield return case0.SetName("Valid");
					}
				}
		/*	helper */
			static ISlottable SBToBeRemoved(){
				ISlottable sb = MakeSubSB();
				sb.IsToBeRemoved().Returns(true);
				return sb;
			}
			static ISlottable SBNotToBeRemoved(){
				ISlottable sb = MakeSubSB();
				sb.IsToBeRemoved().Returns(false);
				return sb;
			}
			List<ISlottable> ExtractSBsFromSlots(List<Slot> slots){
				List<ISlottable> result = new List<ISlottable>();
				foreach(var slot in slots)
					result.Add(slot.sb);
				return result;
			}
			SGTransactionHandler MakeSGTAHandlerWithSGHandlerSG1ReturningThisSG(bool thisSG, ISlottable pickedSB, ISlottable targetSB){
				SGTransactionHandler sgTAHandler;
					ISlotGroup sg = MakeSubSG();
						ITransactionCache taCache = MakeSubTAC();
							taCache.GetPickedSB().Returns(pickedSB);
							taCache.GetTargetSB().Returns(targetSB);
						sg.GetTAC().Returns(taCache);
					ITransactionManager tam = MakeSubTAM();
						ITransactionSGHandler sgHandler = Substitute.For<ITransactionSGHandler>();
						if(thisSG)
							sgHandler.GetSG1().Returns(sg);
						else
							sgHandler.GetSG1().Returns((ISlotGroup)null);
						tam.GetSGHandler().Returns(sgHandler);
				sgTAHandler = new SGTransactionHandler(sg, tam);
				return sgTAHandler;
			}
			public void AssertItemAndQuantityEquality(ISlottable sb, ISlottable other){
				if(sb != null){
					Assert.That(other, Is.Not.Null);
					Assert.That(sb.GetItem().ItemID(), Is.EqualTo(other.GetItem().ItemID()));
					Assert.That(sb.ItemHandler().Quantity(), Is.EqualTo(other.ItemHandler().Quantity()));
				}else
					Assert.That(other, Is.Null);
			}
			void AssertSBsItemAndQuantityEquality(List<ISlottable> actual, List<ISlottable> expected){
				Assert.That(actual.Count, Is.EqualTo(expected.Count));
				foreach(var sb in actual){
					int index = actual.IndexOf(sb);
					if(sb != null){
						Assert.That(expected[index], Is.Not.Null);
						AssertItemAndQuantityEquality(sb, expected[index]);
					}else{
						Assert.That(expected[index], Is.Null);
					}
				}
			}
			static ISlottable MakeSubSBWithItemQuantitySettable(IInventoryItemInstance item){
				ISlottable sb = MakeSubSB();
					sb.GetAcquisitionOrder().Returns(item.AcquisitionOrder());
					sb.GetItemID().Returns(item.ItemID());
					IItemHandler itemHandler = Substitute.For<IItemHandler>();
					sb.ItemHandler().Returns(itemHandler);
					itemHandler.Item().Returns(item);
					sb.GetItem().Returns(item);
				itemHandler.Quantity().Returns(item.GetQuantity());
				int count = item.GetQuantity();
				itemHandler.SetQuantity(Arg.Do<int>(x => count = x));
				itemHandler.When(x => x.SetQuantity(Arg.Any<int>())).Do(x => itemHandler.Quantity().Returns(count));
				return sb;
			}
			[Test]
			public void TestMakeSubSBWithItemQuantitySettable(){
				ISlottable sb = MakeSubSBWithItemQuantitySettable(MakeBowInstance(0));

				Assert.That(sb.ItemHandler().Quantity(), Is.EqualTo(1));

				sb.ItemHandler().SetQuantity(10);

				Assert.That(sb.ItemHandler().Quantity(), Is.EqualTo(10));

				sb.ItemHandler().SetQuantity(2);

				Assert.That(sb.ItemHandler().Quantity(), Is.EqualTo(2));
			}
			void ResetQuantity(List<ISlottable> sbs){
				foreach(var sb in sbs)
					if(sb != null)
					sb.ItemHandler().SetQuantity(sb.GetItem().GetQuantity());
			}
			static ISlottable MakeSubSBWithItem(IInventoryItemInstance item){
				ISlottable sb = MakeSubSB();
					sb.GetItem().Returns(item);
					sb.GetItemID().Returns(item.ItemID());
					sb.GetAcquisitionOrder().Returns(item.AcquisitionOrder());
					IItemHandler itemHandler = Substitute.For<IItemHandler>();
					sb.ItemHandler().Returns(itemHandler);
					itemHandler.Quantity().Returns(item.GetQuantity());
				return sb;
			}
			void DebugSBs(IEnumerable<ISlottable> sbs){
				foreach(var sb in sbs)
					if(sb != null)
						SlotSystemUtil.Stack(sb.GetItem().ToString());
					else
						SlotSystemUtil.Stack("null");
				Debug.Log(SlotSystemUtil.Stacked);
			}
		}
	}
}
