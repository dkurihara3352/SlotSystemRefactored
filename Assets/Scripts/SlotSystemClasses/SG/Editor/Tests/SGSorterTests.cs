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
	namespace SGTests{
		[TestFixture]
		[Category("SG")]
		public class SGSorterTests: SlotSystemTest {

			[TestCaseSource(typeof(VariousSGSorterOSBWRSCases))]
			public void VariousSGSorter_OrderSBsWithRetainedSize_WhenCalled_SetsSBsAccordingly(SGSorter targetSorter,List<ISlottable> source, List<ISlottable> expected){
				SGSorter sorter = targetSorter;

				List<ISlottable> actual = sorter.OrderedSBsWithoutResize(source);

				Assert.That(actual, Is.EqualTo(expected));
			}
				class VariousSGSorterOSBWRSCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] itemIDSorter;
							ISlottable bowA0SB_0;
								BowInstance bowA0_0 = MakeBowInstance(0);
								bowA0SB_0 = MakeSubSBWithItem(bowA0_0);
							ISlottable bowA1SB_0;
								BowInstance bowA1_0 = MakeBowInstance(0);
								bowA1SB_0 = MakeSubSBWithItem(bowA1_0);
							ISlottable wearA0SB_0;
								WearInstance wearA0_0 = MakeWearInstance(0);
								wearA0SB_0 = MakeSubSBWithItem(wearA0_0);
							ISlottable shieldA0SB_0;
								ShieldInstance shieldA0_0 = MakeShieldInstance(0);
								shieldA0SB_0 = MakeSubSBWithItem(shieldA0_0);
							ISlottable mWeaponA0SB_0;
								MeleeWeaponInstance mWeaponA0_0 = MakeMWeaponInstance(0);
								mWeaponA0SB_0 = MakeSubSBWithItem(mWeaponA0_0);
							ISlottable quiverA0SB_0;
								QuiverInstance quiverA0_0 = MakeQuiverInstance(0);
								quiverA0SB_0 = MakeSubSBWithItem(quiverA0_0);
							ISlottable packA0SB_0;
								PackInstance packA0_0 = MakePackInstance(0);
								packA0SB_0 = MakeSubSBWithItem(packA0_0);
							ISlottable partsA0SB_0;
								PartsInstance partsA0_0 = MakePartsInstance(0, 1);
								partsA0SB_0 = MakeSubSBWithItem(partsA0_0);
							wearA0SB_0.GetAcquisitionOrder().Returns(0);
							bowA0SB_0.GetAcquisitionOrder().Returns(1);
							partsA0SB_0.GetAcquisitionOrder().Returns(2);
							mWeaponA0SB_0.GetAcquisitionOrder().Returns(3);
							packA0SB_0.GetAcquisitionOrder().Returns(4);
							quiverA0SB_0.GetAcquisitionOrder().Returns(5);
							shieldA0SB_0.GetAcquisitionOrder().Returns(6);
							bowA1SB_0.GetAcquisitionOrder().Returns(7);
							List<ISlottable> list_0 = new List<ISlottable>(new ISlottable[]{
								null,
								wearA0SB_0,
								bowA0SB_0,
								partsA0SB_0,
								null,
								null,
								mWeaponA0SB_0,
								packA0SB_0,
								null,
								quiverA0SB_0,
								shieldA0SB_0,
								null,
								bowA1SB_0
							});
							List<ISlottable> expected_0 = new List<ISlottable>(new ISlottable[]{
								bowA0SB_0,
								bowA1SB_0,
								wearA0SB_0,
								shieldA0SB_0,
								mWeaponA0SB_0,
								quiverA0SB_0,
								packA0SB_0,
								partsA0SB_0,
								null,
								null,
								null,
								null,
								null
							});
							itemIDSorter = new object[]{new SGItemIDSorter(), list_0, expected_0};
							yield return itemIDSorter;
						object[] inverseIDSorter;
							ISlottable bowA0SB_1;
								BowInstance bowA0_1 = MakeBowInstance(0);
								bowA0SB_1 = MakeSubSBWithItem(bowA0_1);
							ISlottable bowA1SB_1;
								BowInstance bowA1_1 = MakeBowInstance(0);
								bowA1SB_1 = MakeSubSBWithItem(bowA1_1);
							ISlottable wearA0SB_1;
								WearInstance wearA0_1 = MakeWearInstance(0);
								wearA0SB_1 = MakeSubSBWithItem(wearA0_1);
							ISlottable shieldA0SB_1;
								ShieldInstance shieldA0_1 = MakeShieldInstance(0);
								shieldA0SB_1 = MakeSubSBWithItem(shieldA0_1);
							ISlottable mWeaponA0SB_1;
								MeleeWeaponInstance mWeaponA0_1 = MakeMWeaponInstance(0);
								mWeaponA0SB_1 = MakeSubSBWithItem(mWeaponA0_1);
							ISlottable quiverA0SB_1;
								QuiverInstance quiverA0_1 = MakeQuiverInstance(0);
								quiverA0SB_1 = MakeSubSBWithItem(quiverA0_1);
							ISlottable packA0SB_1;
								PackInstance packA0_1 = MakePackInstance(0);
								packA0SB_1 = MakeSubSBWithItem(packA0_1);
							ISlottable partsA0SB_1;
								PartsInstance partsA0_1 = MakePartsInstance(0, 1);
								partsA0SB_1 = MakeSubSBWithItem(partsA0_1);
							wearA0SB_1.GetAcquisitionOrder().Returns(0);
							bowA0SB_1.GetAcquisitionOrder().Returns(1);
							partsA0SB_1.GetAcquisitionOrder().Returns(2);
							mWeaponA0SB_1.GetAcquisitionOrder().Returns(3);
							packA0SB_1.GetAcquisitionOrder().Returns(4);
							quiverA0SB_1.GetAcquisitionOrder().Returns(5);
							shieldA0SB_1.GetAcquisitionOrder().Returns(6);
							bowA1SB_1.GetAcquisitionOrder().Returns(7);
							List<ISlottable> list_1 = new List<ISlottable>(new ISlottable[]{
								null,
								wearA0SB_1,
								bowA0SB_1,
								partsA0SB_1,
								null,
								null,
								mWeaponA0SB_1,
								packA0SB_1,
								null,
								quiverA0SB_1,
								shieldA0SB_1,
								null,
								bowA1SB_1
							});
							List<ISlottable> expected_1 = new List<ISlottable>(new ISlottable[]{
								partsA0SB_1,
								packA0SB_1,
								quiverA0SB_1,
								mWeaponA0SB_1,
								shieldA0SB_1,
								wearA0SB_1,
								bowA1SB_1,
								bowA0SB_1,
								null,
								null,
								null,
								null,
								null
							});
							inverseIDSorter = new object[]{new SGInverseItemIDSorter(), list_1, expected_1};
							yield return inverseIDSorter;
						object[] acqOrderSorter;
							ISlottable bowA0SB_2;
								BowInstance bowA0_2 = MakeBowInstance(0);
								bowA0SB_2 = MakeSubSBWithItem(bowA0_2);
							ISlottable bowA1SB_2;
								BowInstance bowA1_2 = MakeBowInstance(0);
								bowA1SB_2 = MakeSubSBWithItem(bowA1_2);
							ISlottable wearA0SB_2;
								WearInstance wearA0_2 = MakeWearInstance(0);
								wearA0SB_2 = MakeSubSBWithItem(wearA0_2);
							ISlottable shieldA0SB_2;
								ShieldInstance shieldA0_2 = MakeShieldInstance(0);
								shieldA0SB_2 = MakeSubSBWithItem(shieldA0_2);
							ISlottable mWeaponA0SB_2;
								MeleeWeaponInstance mWeaponA0_2 = MakeMWeaponInstance(0);
								mWeaponA0SB_2 = MakeSubSBWithItem(mWeaponA0_2);
							ISlottable quiverA0SB_2;
								QuiverInstance quiverA0_2 = MakeQuiverInstance(0);
								quiverA0SB_2 = MakeSubSBWithItem(quiverA0_2);
							ISlottable packA0SB_2;
								PackInstance packA0_2 = MakePackInstance(0);
								packA0SB_2 = MakeSubSBWithItem(packA0_2);
							ISlottable partsA0SB_2;
								PartsInstance partsA0_2 = MakePartsInstance(0, 1);
								partsA0SB_2 = MakeSubSBWithItem(partsA0_2);
							wearA0SB_2.GetAcquisitionOrder().Returns(0);
							bowA0SB_2.GetAcquisitionOrder().Returns(1);
							partsA0SB_2.GetAcquisitionOrder().Returns(2);
							mWeaponA0SB_2.GetAcquisitionOrder().Returns(3);
							packA0SB_2.GetAcquisitionOrder().Returns(4);
							quiverA0SB_2.GetAcquisitionOrder().Returns(5);
							shieldA0SB_2.GetAcquisitionOrder().Returns(6);
							bowA1SB_2.GetAcquisitionOrder().Returns(7);
							List<ISlottable> list_2 = new List<ISlottable>(new ISlottable[]{
								null,
								wearA0SB_2,
								bowA0SB_2,
								partsA0SB_2,
								null,
								null,
								mWeaponA0SB_2,
								packA0SB_2,
								null,
								quiverA0SB_2,
								shieldA0SB_2,
								null,
								bowA1SB_2
							});
							List<ISlottable> expected_2 = new List<ISlottable>(new ISlottable[]{
								wearA0SB_2,
								bowA0SB_2,
								partsA0SB_2,
								mWeaponA0SB_2,
								packA0SB_2,
								quiverA0SB_2,
								shieldA0SB_2,
								bowA1SB_2,
								null,
								null,
								null,
								null,
								null
							});
							acqOrderSorter = new object[]{new SGAcquisitionOrderSorter(), list_2, expected_2};
							yield return acqOrderSorter;
					}
				}
			[TestCaseSource(typeof(VariousSGSorterTrimCases))]
			public void VariousSGSorter_TrimAndOrderSBs_WhenCalled_SetsSBsAccordingly(SGSorter targetSorter,List<ISlottable> source, List<ISlottable> expected){
				SGSorter sorter = targetSorter;

				List<ISlottable> actual = sorter.OrderedAndTrimmedSBs(source);

				Assert.That(actual, Is.EqualTo(expected));
			}
				class VariousSGSorterTrimCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] itemIDSorter;
							ISlottable bowA0SB_0;
								BowInstance bowA0_0 = MakeBowInstance(0);
								bowA0SB_0 = MakeSubSBWithItem(bowA0_0);
							ISlottable bowA1SB_0;
								BowInstance bowA1_0 = MakeBowInstance(0);
								bowA1SB_0 = MakeSubSBWithItem(bowA1_0);
							ISlottable wearA0SB_0;
								WearInstance wearA0_0 = MakeWearInstance(0);
								wearA0SB_0 = MakeSubSBWithItem(wearA0_0);
							ISlottable shieldA0SB_0;
								ShieldInstance shieldA0_0 = MakeShieldInstance(0);
								shieldA0SB_0 = MakeSubSBWithItem(shieldA0_0);
							ISlottable mWeaponA0SB_0;
								MeleeWeaponInstance mWeaponA0_0 = MakeMWeaponInstance(0);
								mWeaponA0SB_0 = MakeSubSBWithItem(mWeaponA0_0);
							ISlottable quiverA0SB_0;
								QuiverInstance quiverA0_0 = MakeQuiverInstance(0);
								quiverA0SB_0 = MakeSubSBWithItem(quiverA0_0);
							ISlottable packA0SB_0;
								PackInstance packA0_0 = MakePackInstance(0);
								packA0SB_0 = MakeSubSBWithItem(packA0_0);
							ISlottable partsA0SB_0;
								PartsInstance partsA0_0 = MakePartsInstance(0, 1);
								partsA0SB_0 = MakeSubSBWithItem(partsA0_0);
							wearA0SB_0.GetAcquisitionOrder().Returns(0);
							bowA0SB_0.GetAcquisitionOrder().Returns(1);
							partsA0SB_0.GetAcquisitionOrder().Returns(2);
							mWeaponA0SB_0.GetAcquisitionOrder().Returns(3);
							packA0SB_0.GetAcquisitionOrder().Returns(4);
							quiverA0SB_0.GetAcquisitionOrder().Returns(5);
							shieldA0SB_0.GetAcquisitionOrder().Returns(6);
							bowA1SB_0.GetAcquisitionOrder().Returns(7);
							List<ISlottable> list_0 = new List<ISlottable>(new ISlottable[]{
								null,
								wearA0SB_0,
								bowA0SB_0,
								partsA0SB_0,
								null,
								null,
								mWeaponA0SB_0,
								packA0SB_0,
								null,
								quiverA0SB_0,
								shieldA0SB_0,
								null,
								bowA1SB_0
							});
							List<ISlottable> expected_0 = new List<ISlottable>(new ISlottable[]{
								bowA0SB_0,
								bowA1SB_0,
								wearA0SB_0,
								shieldA0SB_0,
								mWeaponA0SB_0,
								quiverA0SB_0,
								packA0SB_0,
								partsA0SB_0
							});
							itemIDSorter = new object[]{new SGItemIDSorter(), list_0, expected_0};
							yield return itemIDSorter;
						object[] inverseIDSorter;
							ISlottable bowA0SB_1;
								BowInstance bowA0_1 = MakeBowInstance(0);
								bowA0SB_1 = MakeSubSBWithItem(bowA0_1);
							ISlottable bowA1SB_1;
								BowInstance bowA1_1 = MakeBowInstance(0);
								bowA1SB_1 = MakeSubSBWithItem(bowA1_1);
							ISlottable wearA0SB_1;
								WearInstance wearA0_1 = MakeWearInstance(0);
								wearA0SB_1 = MakeSubSBWithItem(wearA0_1);
							ISlottable shieldA0SB_1;
								ShieldInstance shieldA0_1 = MakeShieldInstance(0);
								shieldA0SB_1 = MakeSubSBWithItem(shieldA0_1);
							ISlottable mWeaponA0SB_1;
								MeleeWeaponInstance mWeaponA0_1 = MakeMWeaponInstance(0);
								mWeaponA0SB_1 = MakeSubSBWithItem(mWeaponA0_1);
							ISlottable quiverA0SB_1;
								QuiverInstance quiverA0_1 = MakeQuiverInstance(0);
								quiverA0SB_1 = MakeSubSBWithItem(quiverA0_1);
							ISlottable packA0SB_1;
								PackInstance packA0_1 = MakePackInstance(0);
								packA0SB_1 = MakeSubSBWithItem(packA0_1);
							ISlottable partsA0SB_1;
								PartsInstance partsA0_1 = MakePartsInstance(0, 1);
								partsA0SB_1 = MakeSubSBWithItem(partsA0_1);
							wearA0SB_1.GetAcquisitionOrder().Returns(0);
							bowA0SB_1.GetAcquisitionOrder().Returns(1);
							partsA0SB_1.GetAcquisitionOrder().Returns(2);
							mWeaponA0SB_1.GetAcquisitionOrder().Returns(3);
							packA0SB_1.GetAcquisitionOrder().Returns(4);
							quiverA0SB_1.GetAcquisitionOrder().Returns(5);
							shieldA0SB_1.GetAcquisitionOrder().Returns(6);
							bowA1SB_1.GetAcquisitionOrder().Returns(7);
							List<ISlottable> list_1 = new List<ISlottable>(new ISlottable[]{
								null,
								wearA0SB_1,
								bowA0SB_1,
								partsA0SB_1,
								null,
								null,
								mWeaponA0SB_1,
								packA0SB_1,
								null,
								quiverA0SB_1,
								shieldA0SB_1,
								null,
								bowA1SB_1
							});
							List<ISlottable> expected_1 = new List<ISlottable>(new ISlottable[]{
								partsA0SB_1,
								packA0SB_1,
								quiverA0SB_1,
								mWeaponA0SB_1,
								shieldA0SB_1,
								wearA0SB_1,
								bowA1SB_1,
								bowA0SB_1
							});
							inverseIDSorter = new object[]{new SGInverseItemIDSorter(), list_1, expected_1};
							yield return inverseIDSorter;
						object[] acqOrderSorter;
							ISlottable bowA0SB_2;
								BowInstance bowA0_2 = MakeBowInstance(0);
								bowA0SB_2 = MakeSubSBWithItem(bowA0_2);
							ISlottable bowA1SB_2;
								BowInstance bowA1_2 = MakeBowInstance(0);
								bowA1SB_2 = MakeSubSBWithItem(bowA1_2);
							ISlottable wearA0SB_2;
								WearInstance wearA0_2 = MakeWearInstance(0);
								wearA0SB_2 = MakeSubSBWithItem(wearA0_2);
							ISlottable shieldA0SB_2;
								ShieldInstance shieldA0_2 = MakeShieldInstance(0);
								shieldA0SB_2 = MakeSubSBWithItem(shieldA0_2);
							ISlottable mWeaponA0SB_2;
								MeleeWeaponInstance mWeaponA0_2 = MakeMWeaponInstance(0);
								mWeaponA0SB_2 = MakeSubSBWithItem(mWeaponA0_2);
							ISlottable quiverA0SB_2;
								QuiverInstance quiverA0_2 = MakeQuiverInstance(0);
								quiverA0SB_2 = MakeSubSBWithItem(quiverA0_2);
							ISlottable packA0SB_2;
								PackInstance packA0_2 = MakePackInstance(0);
								packA0SB_2 = MakeSubSBWithItem(packA0_2);
							ISlottable partsA0SB_2;
								PartsInstance partsA0_2 = MakePartsInstance(0, 1);
								partsA0SB_2 = MakeSubSBWithItem(partsA0_2);
							wearA0SB_2.GetAcquisitionOrder().Returns(0);
							bowA0SB_2.GetAcquisitionOrder().Returns(1);
							partsA0SB_2.GetAcquisitionOrder().Returns(2);
							mWeaponA0SB_2.GetAcquisitionOrder().Returns(3);
							packA0SB_2.GetAcquisitionOrder().Returns(4);
							quiverA0SB_2.GetAcquisitionOrder().Returns(5);
							shieldA0SB_2.GetAcquisitionOrder().Returns(6);
							bowA1SB_2.GetAcquisitionOrder().Returns(7);
							List<ISlottable> list_2 = new List<ISlottable>(new ISlottable[]{
								null,
								wearA0SB_2,
								bowA0SB_2,
								partsA0SB_2,
								null,
								null,
								mWeaponA0SB_2,
								packA0SB_2,
								null,
								quiverA0SB_2,
								shieldA0SB_2,
								null,
								bowA1SB_2
							});
							List<ISlottable> expected_2 = new List<ISlottable>(new ISlottable[]{
								wearA0SB_2,
								bowA0SB_2,
								partsA0SB_2,
								mWeaponA0SB_2,
								packA0SB_2,
								quiverA0SB_2,
								shieldA0SB_2,
								bowA1SB_2
							});
							acqOrderSorter = new object[]{new SGAcquisitionOrderSorter(), list_2, expected_2};
							yield return acqOrderSorter;
					}
				}
			/* helper */
				static ISlottable MakeSubSBWithItem(IInventoryItemInstance item){
					ISlottable sb = MakeSubSB();
						sb.GetItem().Returns(item);
						sb.GetItemID().Returns(item.GetItemID());
					return sb;
				}
		}
	}
}
