﻿using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using System;
using System.Collections;
using System.Collections.Generic;
namespace SlotSystemTests{
	namespace SGTests{
		[TestFixture]
		[Category("SG")]
		public class SGSorterTests: SlotSystemTest {

			[TestCaseSource(typeof(VariousSGSorterOSBWRSCases))]
			public void VariousSGSorter_OrderSBsWithRetainedSize_WhenCalled_SetsSBsAccordingly(SGSorter targetSorter,List<ISlottable> original, List<ISlottable> expected){
				SGSorter sorter = targetSorter;
				List<ISlottable> list = new List<ISlottable>(original);

				sorter.OrderSBsWithRetainedSize(ref list);

				Assert.That(list, Is.EqualTo(expected));
				}
				class VariousSGSorterOSBWRSCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] itemIDSorter;
							ISlottable bowA0SB_0 = MakeSB();
								BowInstance bowA0_0 = MakeBowInstance(0);
								bowA0SB_0.SetItem(bowA0_0);
							ISlottable bowA1SB_0 = MakeSB();
								BowInstance bowA1_0 = MakeBowInstance(0);
								bowA1SB_0.SetItem(bowA1_0);
							ISlottable wearA0SB_0 = MakeSB();
								WearInstance wearA0_0 = MakeWearInstance(0);
								wearA0SB_0.SetItem(wearA0_0);
							ISlottable shieldA0SB_0 = MakeSB();
								ShieldInstance shieldA0_0 = MakeShieldInstance(0);
								shieldA0SB_0.SetItem(shieldA0_0);
							ISlottable mWeaponA0SB_0 = MakeSB();
								MeleeWeaponInstance mWeaponA0_0 = MakeMeleeWeaponInstance(0);
								mWeaponA0SB_0.SetItem(mWeaponA0_0);
							ISlottable quiverA0SB_0 = MakeSB();
								QuiverInstance quiverA0_0 = MakeQuiverInstance(0);
								quiverA0SB_0.SetItem(quiverA0_0);
							ISlottable packA0SB_0 = MakeSB();
								PackInstance packA0_0 = MakePackInstance(0);
								packA0SB_0.SetItem(packA0_0);
							ISlottable partsA0SB_0 = MakeSB();
								PartsInstance partsA0_0 = MakePartsInstance(0, 1);
								partsA0SB_0.SetItem(partsA0_0);
							wearA0_0.SetAcquisitionOrder(0);
							bowA0_0.SetAcquisitionOrder(1);
							partsA0_0.SetAcquisitionOrder(2);
							mWeaponA0_0.SetAcquisitionOrder(3);
							packA0_0.SetAcquisitionOrder(4);
							quiverA0_0.SetAcquisitionOrder(5);
							shieldA0_0.SetAcquisitionOrder(6);
							bowA1_0.SetAcquisitionOrder(7);
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
							ISlottable bowA0SB_1 = MakeSB();
								BowInstance bowA0_1 = MakeBowInstance(0);
								bowA0SB_1.SetItem(bowA0_1);
							ISlottable bowA1SB_1 = MakeSB();
								BowInstance bowA1_1 = MakeBowInstance(0);
								bowA1SB_1.SetItem(bowA1_1);
							ISlottable wearA0SB_1 = MakeSB();
								WearInstance wearA0_1 = MakeWearInstance(0);
								wearA0SB_1.SetItem(wearA0_1);
							ISlottable shieldA0SB_1 = MakeSB();
								ShieldInstance shieldA0_1 = MakeShieldInstance(0);
								shieldA0SB_1.SetItem(shieldA0_1);
							ISlottable mWeaponA0SB_1 = MakeSB();
								MeleeWeaponInstance mWeaponA0_1 = MakeMeleeWeaponInstance(0);
								mWeaponA0SB_1.SetItem(mWeaponA0_1);
							ISlottable quiverA0SB_1 = MakeSB();
								QuiverInstance quiverA0_1 = MakeQuiverInstance(0);
								quiverA0SB_1.SetItem(quiverA0_1);
							ISlottable packA0SB_1 = MakeSB();
								PackInstance packA0_1 = MakePackInstance(0);
								packA0SB_1.SetItem(packA0_1);
							ISlottable partsA0SB_1 = MakeSB();
								PartsInstance partsA0_1 = MakePartsInstance(0, 1);
								partsA0SB_1.SetItem(partsA0_1);
							wearA0_1.SetAcquisitionOrder(0);
							bowA0_1.SetAcquisitionOrder(1);
							partsA0_1.SetAcquisitionOrder(2);
							mWeaponA0_1.SetAcquisitionOrder(3);
							packA0_1.SetAcquisitionOrder(4);
							quiverA0_1.SetAcquisitionOrder(5);
							shieldA0_1.SetAcquisitionOrder(6);
							bowA1_1.SetAcquisitionOrder(7);
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
							ISlottable bowA0SB_2 = MakeSB();
								BowInstance bowA0_2 = MakeBowInstance(0);
								bowA0SB_2.SetItem(bowA0_2);
							ISlottable bowA1SB_2 = MakeSB();
								BowInstance bowA1_2 = MakeBowInstance(0);
								bowA1SB_2.SetItem(bowA1_2);
							ISlottable wearA0SB_2 = MakeSB();
								WearInstance wearA0_2 = MakeWearInstance(0);
								wearA0SB_2.SetItem(wearA0_2);
							ISlottable shieldA0SB_2 = MakeSB();
								ShieldInstance shieldA0_2 = MakeShieldInstance(0);
								shieldA0SB_2.SetItem(shieldA0_2);
							ISlottable mWeaponA0SB_2 = MakeSB();
								MeleeWeaponInstance mWeaponA0_2 = MakeMeleeWeaponInstance(0);
								mWeaponA0SB_2.SetItem(mWeaponA0_2);
							ISlottable quiverA0SB_2 = MakeSB();
								QuiverInstance quiverA0_2 = MakeQuiverInstance(0);
								quiverA0SB_2.SetItem(quiverA0_2);
							ISlottable packA0SB_2 = MakeSB();
								PackInstance packA0_2 = MakePackInstance(0);
								packA0SB_2.SetItem(packA0_2);
							ISlottable partsA0SB_2 = MakeSB();
								PartsInstance partsA0_2 = MakePartsInstance(0, 1);
								partsA0SB_2.SetItem(partsA0_2);
							wearA0_2.SetAcquisitionOrder(0);
							bowA0_2.SetAcquisitionOrder(1);
							partsA0_2.SetAcquisitionOrder(2);
							mWeaponA0_2.SetAcquisitionOrder(3);
							packA0_2.SetAcquisitionOrder(4);
							quiverA0_2.SetAcquisitionOrder(5);
							shieldA0_2.SetAcquisitionOrder(6);
							bowA1_2.SetAcquisitionOrder(7);
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
			public void VariousSGSorter_TrimAndOrderSBs_WhenCalled_SetsSBsAccordingly(SGSorter targetSorter,List<ISlottable> original, List<ISlottable> expected){
				SGSorter sorter = targetSorter;
				List<ISlottable> list = new List<ISlottable>(original);

				sorter.TrimAndOrderSBs(ref list);

				Assert.That(list, Is.EqualTo(expected));
				}
				class VariousSGSorterTrimCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] itemIDSorter;
							ISlottable bowA0SB_0 = MakeSB();
								BowInstance bowA0_0 = MakeBowInstance(0);
								bowA0SB_0.SetItem(bowA0_0);
							ISlottable bowA1SB_0 = MakeSB();
								BowInstance bowA1_0 = MakeBowInstance(0);
								bowA1SB_0.SetItem(bowA1_0);
							ISlottable wearA0SB_0 = MakeSB();
								WearInstance wearA0_0 = MakeWearInstance(0);
								wearA0SB_0.SetItem(wearA0_0);
							ISlottable shieldA0SB_0 = MakeSB();
								ShieldInstance shieldA0_0 = MakeShieldInstance(0);
								shieldA0SB_0.SetItem(shieldA0_0);
							ISlottable mWeaponA0SB_0 = MakeSB();
								MeleeWeaponInstance mWeaponA0_0 = MakeMeleeWeaponInstance(0);
								mWeaponA0SB_0.SetItem(mWeaponA0_0);
							ISlottable quiverA0SB_0 = MakeSB();
								QuiverInstance quiverA0_0 = MakeQuiverInstance(0);
								quiverA0SB_0.SetItem(quiverA0_0);
							ISlottable packA0SB_0 = MakeSB();
								PackInstance packA0_0 = MakePackInstance(0);
								packA0SB_0.SetItem(packA0_0);
							ISlottable partsA0SB_0 = MakeSB();
								PartsInstance partsA0_0 = MakePartsInstance(0, 1);
								partsA0SB_0.SetItem(partsA0_0);
							wearA0_0.SetAcquisitionOrder(0);
							bowA0_0.SetAcquisitionOrder(1);
							partsA0_0.SetAcquisitionOrder(2);
							mWeaponA0_0.SetAcquisitionOrder(3);
							packA0_0.SetAcquisitionOrder(4);
							quiverA0_0.SetAcquisitionOrder(5);
							shieldA0_0.SetAcquisitionOrder(6);
							bowA1_0.SetAcquisitionOrder(7);
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
							ISlottable bowA0SB_1 = MakeSB();
								BowInstance bowA0_1 = MakeBowInstance(0);
								bowA0SB_1.SetItem(bowA0_1);
							ISlottable bowA1SB_1 = MakeSB();
								BowInstance bowA1_1 = MakeBowInstance(0);
								bowA1SB_1.SetItem(bowA1_1);
							ISlottable wearA0SB_1 = MakeSB();
								WearInstance wearA0_1 = MakeWearInstance(0);
								wearA0SB_1.SetItem(wearA0_1);
							ISlottable shieldA0SB_1 = MakeSB();
								ShieldInstance shieldA0_1 = MakeShieldInstance(0);
								shieldA0SB_1.SetItem(shieldA0_1);
							ISlottable mWeaponA0SB_1 = MakeSB();
								MeleeWeaponInstance mWeaponA0_1 = MakeMeleeWeaponInstance(0);
								mWeaponA0SB_1.SetItem(mWeaponA0_1);
							ISlottable quiverA0SB_1 = MakeSB();
								QuiverInstance quiverA0_1 = MakeQuiverInstance(0);
								quiverA0SB_1.SetItem(quiverA0_1);
							ISlottable packA0SB_1 = MakeSB();
								PackInstance packA0_1 = MakePackInstance(0);
								packA0SB_1.SetItem(packA0_1);
							ISlottable partsA0SB_1 = MakeSB();
								PartsInstance partsA0_1 = MakePartsInstance(0, 1);
								partsA0SB_1.SetItem(partsA0_1);
							wearA0_1.SetAcquisitionOrder(0);
							bowA0_1.SetAcquisitionOrder(1);
							partsA0_1.SetAcquisitionOrder(2);
							mWeaponA0_1.SetAcquisitionOrder(3);
							packA0_1.SetAcquisitionOrder(4);
							quiverA0_1.SetAcquisitionOrder(5);
							shieldA0_1.SetAcquisitionOrder(6);
							bowA1_1.SetAcquisitionOrder(7);
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
							ISlottable bowA0SB_2 = MakeSB();
								BowInstance bowA0_2 = MakeBowInstance(0);
								bowA0SB_2.SetItem(bowA0_2);
							ISlottable bowA1SB_2 = MakeSB();
								BowInstance bowA1_2 = MakeBowInstance(0);
								bowA1SB_2.SetItem(bowA1_2);
							ISlottable wearA0SB_2 = MakeSB();
								WearInstance wearA0_2 = MakeWearInstance(0);
								wearA0SB_2.SetItem(wearA0_2);
							ISlottable shieldA0SB_2 = MakeSB();
								ShieldInstance shieldA0_2 = MakeShieldInstance(0);
								shieldA0SB_2.SetItem(shieldA0_2);
							ISlottable mWeaponA0SB_2 = MakeSB();
								MeleeWeaponInstance mWeaponA0_2 = MakeMeleeWeaponInstance(0);
								mWeaponA0SB_2.SetItem(mWeaponA0_2);
							ISlottable quiverA0SB_2 = MakeSB();
								QuiverInstance quiverA0_2 = MakeQuiverInstance(0);
								quiverA0SB_2.SetItem(quiverA0_2);
							ISlottable packA0SB_2 = MakeSB();
								PackInstance packA0_2 = MakePackInstance(0);
								packA0SB_2.SetItem(packA0_2);
							ISlottable partsA0SB_2 = MakeSB();
								PartsInstance partsA0_2 = MakePartsInstance(0, 1);
								partsA0SB_2.SetItem(partsA0_2);
							wearA0_2.SetAcquisitionOrder(0);
							bowA0_2.SetAcquisitionOrder(1);
							partsA0_2.SetAcquisitionOrder(2);
							mWeaponA0_2.SetAcquisitionOrder(3);
							packA0_2.SetAcquisitionOrder(4);
							quiverA0_2.SetAcquisitionOrder(5);
							shieldA0_2.SetAcquisitionOrder(6);
							bowA1_2.SetAcquisitionOrder(7);
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
		}
	}
}
