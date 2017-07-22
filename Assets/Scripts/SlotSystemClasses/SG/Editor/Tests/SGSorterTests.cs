using UnityEngine;
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
						ISlottable bowSB_A = MakeSB();
							BowInstance bowA = MakeBowInstance(0);
							bowSB_A.SetItem(bowA);
						ISlottable bowSB_A_1 = MakeSB();
							BowInstance bowA_1 = MakeBowInstance(0);
							bowSB_A_1.SetItem(bowA_1);
						ISlottable wearSB = MakeSB();
							WearInstance wearA = MakeWearInstance(0);
							wearSB.SetItem(wearA);
						ISlottable shieldSB = MakeSB();
							ShieldInstance shieldA = MakeShieldInstance(0);
							shieldSB.SetItem(shieldA);
						ISlottable mWeaponSB = MakeSB();
							MeleeWeaponInstance mWeaponA = MakeMeleeWeaponInstance(0);
							mWeaponSB.SetItem(mWeaponA);
						ISlottable quiverSB = MakeSB();
							QuiverInstance quiverA = MakeQuiverInstance(0);
							quiverSB.SetItem(quiverA);
						ISlottable packSB = MakeSB();
							PackInstance packA = MakePackInstance(0);
							packSB.SetItem(packA);
						ISlottable partsSB = MakeSB();
							PartsInstance partsA = MakePartsInstance(0, 1);
							partsSB.SetItem(partsA);
						wearA.SetAcquisitionOrder(0);
						bowA.SetAcquisitionOrder(1);
						partsA.SetAcquisitionOrder(2);
						mWeaponA.SetAcquisitionOrder(3);
						packA.SetAcquisitionOrder(4);
						quiverA.SetAcquisitionOrder(5);
						shieldA.SetAcquisitionOrder(6);
						bowA_1.SetAcquisitionOrder(7);

						List<ISlottable> listWithHoles = new List<ISlottable>(new ISlottable[]{
							null,
							wearSB,
							bowSB_A,
							partsSB,
							null,
							null,
							mWeaponSB,
							packSB,
							null,
							quiverSB,
							shieldSB,
							null,
							bowSB_A_1
						});
							
						yield return new object[]{
							new SGItemIDSorter(),
							new List<ISlottable>(listWithHoles),
							new List<ISlottable>(new ISlottable[]{
								bowSB_A,
								bowSB_A_1,
								wearSB,
								shieldSB,
								mWeaponSB,
								quiverSB,
								packSB,
								partsSB,
								null,
								null,
								null,
								null,
								null,
							})
						};
						yield return new object[]{
							new SGInverseItemIDSorter(),
							new List<ISlottable>(listWithHoles),
							new List<ISlottable>(new ISlottable[]{
								partsSB,
								packSB,
								quiverSB,
								mWeaponSB,
								shieldSB,
								wearSB,
								bowSB_A_1,
								bowSB_A,
								null,
								null,
								null,
								null,
								null,
							})
						};
						yield return new object[]{
							new SGAcquisitionOrderSorter(),
							new List<ISlottable>(listWithHoles),
							new List<ISlottable>(new ISlottable[]{
								wearSB,
								bowSB_A,
								partsSB,
								mWeaponSB,
								packSB,
								quiverSB,
								shieldSB,
								bowSB_A_1,
								null,
								null,
								null,
								null,
								null,
							})
						};
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
						ISlottable bowSB_A = MakeSB();
							BowInstance bowA = MakeBowInstance(0);
							bowSB_A.SetItem(bowA);
						ISlottable bowSB_A_1 = MakeSB();
							BowInstance bowA_1 = MakeBowInstance(0);
							bowSB_A_1.SetItem(bowA_1);
						ISlottable wearSB = MakeSB();
							WearInstance wearA = MakeWearInstance(0);
							wearSB.SetItem(wearA);
						ISlottable shieldSB = MakeSB();
							ShieldInstance shieldA = MakeShieldInstance(0);
							shieldSB.SetItem(shieldA);
						ISlottable mWeaponSB = MakeSB();
							MeleeWeaponInstance mWeaponA = MakeMeleeWeaponInstance(0);
							mWeaponSB.SetItem(mWeaponA);
						ISlottable quiverSB = MakeSB();
							QuiverInstance quiverA = MakeQuiverInstance(0);
							quiverSB.SetItem(quiverA);
						ISlottable packSB = MakeSB();
							PackInstance packA = MakePackInstance(0);
							packSB.SetItem(packA);
						ISlottable partsSB = MakeSB();
							PartsInstance partsA = MakePartsInstance(0, 1);
							partsSB.SetItem(partsA);
						wearA.SetAcquisitionOrder(0);
						bowA.SetAcquisitionOrder(1);
						partsA.SetAcquisitionOrder(2);
						mWeaponA.SetAcquisitionOrder(3);
						packA.SetAcquisitionOrder(4);
						quiverA.SetAcquisitionOrder(5);
						shieldA.SetAcquisitionOrder(6);
						bowA_1.SetAcquisitionOrder(7);

						List<ISlottable> listWithHoles = new List<ISlottable>(new ISlottable[]{
							null,
							wearSB,
							bowSB_A,
							partsSB,
							null,
							null,
							mWeaponSB,
							packSB,
							null,
							quiverSB,
							shieldSB,
							null,
							bowSB_A_1
						});
							
						yield return new object[]{
							new SGItemIDSorter(),
							new List<ISlottable>(listWithHoles),
							new List<ISlottable>(new ISlottable[]{
								bowSB_A,
								bowSB_A_1,
								wearSB,
								shieldSB,
								mWeaponSB,
								quiverSB,
								packSB,
								partsSB,
							})
						};
						yield return new object[]{
							new SGInverseItemIDSorter(),
							new List<ISlottable>(listWithHoles),
							new List<ISlottable>(new ISlottable[]{
								partsSB,
								packSB,
								quiverSB,
								mWeaponSB,
								shieldSB,
								wearSB,
								bowSB_A_1,
								bowSB_A,
							})
						};
						yield return new object[]{
							new SGAcquisitionOrderSorter(),
							new List<ISlottable>(listWithHoles),
							new List<ISlottable>(new ISlottable[]{
								wearSB,
								bowSB_A,
								partsSB,
								mWeaponSB,
								packSB,
								quiverSB,
								shieldSB,
								bowSB_A_1,
							})
						};
					}
				}
		}
	}
}
