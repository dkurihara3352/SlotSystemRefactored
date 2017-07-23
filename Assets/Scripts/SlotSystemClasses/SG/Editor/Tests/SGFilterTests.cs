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
		[Category("SG")]
		public class SGFilterTests: SlotSystemTest {

			[TestCaseSource(typeof(SGFilterCases))]
			public void SGFilter_Filter_WhenCalled_SetsItemsAccordingly(SGFilter targFilter, List<SlottableItem> list, IEnumerable<SlottableItem> expected){
				SGFilter filter = targFilter;
                List<SlottableItem> target = list;

				filter.Filter(ref target);

				Assert.That(target, Is.EqualTo(expected));
				}
				class SGFilterCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] bowFilter;
							BowInstance bowA0_1 = MakeBowInstance(0);
							BowInstance bowA1_1 = MakeBowInstance(0);
							BowInstance bowA2_1 = MakeBowInstance(0);
							WearInstance wearA0_1 = MakeWearInstance(0);
							WearInstance wearA1_1 = MakeWearInstance(0);
							WearInstance wearA2_1 = MakeWearInstance(0);
							ShieldInstance shieldA0_1 = MakeShieldInstance(0);
							ShieldInstance shieldA1_1 = MakeShieldInstance(0);
							ShieldInstance shieldA2_1 = MakeShieldInstance(0);
							MeleeWeaponInstance mWeaponA0_1 = MakeMeleeWeaponInstance(0);
							MeleeWeaponInstance mWeaponA1_1 = MakeMeleeWeaponInstance(0);
							MeleeWeaponInstance mWeaponA2_1 = MakeMeleeWeaponInstance(0);
							QuiverInstance quiverA0_1 = MakeQuiverInstance(0);
							QuiverInstance quiverA1_1 = MakeQuiverInstance(0);
							QuiverInstance quiverA2_1 = MakeQuiverInstance(0);
							PackInstance packA0_1 = MakePackInstance(0);
							PackInstance packA1_1 = MakePackInstance(0);
							PackInstance packA2_1 = MakePackInstance(0);
							PartsInstance partsA0_1 = MakePartsInstance(0, 1);
							PartsInstance partsA1_1 = MakePartsInstance(0, 1);
							PartsInstance partsA2_1 = MakePartsInstance(0, 1);
							List<SlottableItem> list_1 = new List<SlottableItem>(new SlottableItem[]{
								bowA0_1, bowA1_1, bowA2_1,
								wearA0_1, wearA1_1, wearA2_1,
								shieldA0_1, shieldA1_1, shieldA2_1,
								mWeaponA0_1, mWeaponA1_1, mWeaponA2_1,
								quiverA0_1, quiverA1_1, quiverA2_1,
								packA0_1, packA1_1, packA2_1,
								partsA0_1, partsA1_1, partsA2_1
							});
							IEnumerable<SlottableItem> expected_1 = new SlottableItem[]{
								bowA0_1, bowA1_1, bowA2_1
							};
							bowFilter = new object[]{new SGBowFilter(), list_1, expected_1};
							yield return bowFilter;
						object[] wearFilter;
							BowInstance bowA0_2 = MakeBowInstance(0);
							BowInstance bowA1_2 = MakeBowInstance(0);
							BowInstance bowA2_2 = MakeBowInstance(0);
							WearInstance wearA0_2 = MakeWearInstance(0);
							WearInstance wearA1_2 = MakeWearInstance(0);
							WearInstance wearA2_2 = MakeWearInstance(0);
							ShieldInstance shieldA0_2 = MakeShieldInstance(0);
							ShieldInstance shieldA1_2 = MakeShieldInstance(0);
							ShieldInstance shieldA2_2 = MakeShieldInstance(0);
							MeleeWeaponInstance mWeaponA0_2 = MakeMeleeWeaponInstance(0);
							MeleeWeaponInstance mWeaponA1_2 = MakeMeleeWeaponInstance(0);
							MeleeWeaponInstance mWeaponA2_2 = MakeMeleeWeaponInstance(0);
							QuiverInstance quiverA0_2 = MakeQuiverInstance(0);
							QuiverInstance quiverA1_2 = MakeQuiverInstance(0);
							QuiverInstance quiverA2_2 = MakeQuiverInstance(0);
							PackInstance packA0_2 = MakePackInstance(0);
							PackInstance packA1_2 = MakePackInstance(0);
							PackInstance packA2_2 = MakePackInstance(0);
							PartsInstance partsA0_2 = MakePartsInstance(0, 1);
							PartsInstance partsA1_2 = MakePartsInstance(0, 1);
							PartsInstance partsA2_2 = MakePartsInstance(0, 1);
							List<SlottableItem> list_2 = new List<SlottableItem>(new SlottableItem[]{
								bowA0_2, bowA1_2, bowA2_2,
								wearA0_2, wearA1_2, wearA2_2,
								shieldA0_2, shieldA1_2, shieldA2_2,
								mWeaponA0_2, mWeaponA1_2, mWeaponA2_2,
								quiverA0_2, quiverA1_2, quiverA2_2,
								packA0_2, packA1_2, packA2_2,
								partsA0_2, partsA1_2, partsA2_2
							});
							IEnumerable<SlottableItem> expected_2 = new SlottableItem[]{
								wearA0_2, wearA1_2, wearA2_2
							};
							wearFilter = new object[]{new SGWearFilter(), list_2, expected_2};
							yield return wearFilter;
						object[] cGearsFilter;
							BowInstance bowA0_3 = MakeBowInstance(0);
							BowInstance bowA1_3 = MakeBowInstance(0);
							BowInstance bowA2_3 = MakeBowInstance(0);
							WearInstance wearA0_3 = MakeWearInstance(0);
							WearInstance wearA1_3 = MakeWearInstance(0);
							WearInstance wearA2_3 = MakeWearInstance(0);
							ShieldInstance shieldA0_3 = MakeShieldInstance(0);
							ShieldInstance shieldA1_3 = MakeShieldInstance(0);
							ShieldInstance shieldA2_3 = MakeShieldInstance(0);
							MeleeWeaponInstance mWeaponA0_3 = MakeMeleeWeaponInstance(0);
							MeleeWeaponInstance mWeaponA1_3 = MakeMeleeWeaponInstance(0);
							MeleeWeaponInstance mWeaponA2_3 = MakeMeleeWeaponInstance(0);
							QuiverInstance quiverA0_3 = MakeQuiverInstance(0);
							QuiverInstance quiverA1_3 = MakeQuiverInstance(0);
							QuiverInstance quiverA2_3 = MakeQuiverInstance(0);
							PackInstance packA0_3 = MakePackInstance(0);
							PackInstance packA1_3 = MakePackInstance(0);
							PackInstance packA2_3 = MakePackInstance(0);
							PartsInstance partsA0_3 = MakePartsInstance(0, 1);
							PartsInstance partsA1_3 = MakePartsInstance(0, 1);
							PartsInstance partsA2_3 = MakePartsInstance(0, 1);
							List<SlottableItem> list_3 = new List<SlottableItem>(new SlottableItem[]{
								bowA0_3, bowA1_3, bowA2_3,
								wearA0_3, wearA1_3, wearA2_3,
								shieldA0_3, shieldA1_3, shieldA2_3,
								mWeaponA0_3, mWeaponA1_3, mWeaponA2_3,
								quiverA0_3, quiverA1_3, quiverA2_3,
								packA0_3, packA1_3, packA2_3,
								partsA0_3, partsA1_3, partsA2_3
							});
							IEnumerable<SlottableItem> expected_3 = new SlottableItem[]{
								shieldA0_3, shieldA1_3, shieldA2_3,
								mWeaponA0_3, mWeaponA1_3, mWeaponA2_3,
								quiverA0_3, quiverA1_3, quiverA2_3,
								packA0_3, packA1_3, packA2_3
							};
							cGearsFilter = new object[]{new SGCGearsFilter(), list_3, expected_3};
							yield return cGearsFilter;
						object[] partsFilter;
							BowInstance bowA0_4 = MakeBowInstance(0);
							BowInstance bowA1_4 = MakeBowInstance(0);
							BowInstance bowA2_4 = MakeBowInstance(0);
							WearInstance wearA0_4 = MakeWearInstance(0);
							WearInstance wearA1_4 = MakeWearInstance(0);
							WearInstance wearA2_4 = MakeWearInstance(0);
							ShieldInstance shieldA0_4 = MakeShieldInstance(0);
							ShieldInstance shieldA1_4 = MakeShieldInstance(0);
							ShieldInstance shieldA2_4 = MakeShieldInstance(0);
							MeleeWeaponInstance mWeaponA0_4 = MakeMeleeWeaponInstance(0);
							MeleeWeaponInstance mWeaponA1_4 = MakeMeleeWeaponInstance(0);
							MeleeWeaponInstance mWeaponA2_4 = MakeMeleeWeaponInstance(0);
							QuiverInstance quiverA0_4 = MakeQuiverInstance(0);
							QuiverInstance quiverA1_4 = MakeQuiverInstance(0);
							QuiverInstance quiverA2_4 = MakeQuiverInstance(0);
							PackInstance packA0_4 = MakePackInstance(0);
							PackInstance packA1_4 = MakePackInstance(0);
							PackInstance packA2_4 = MakePackInstance(0);
							PartsInstance partsA0_4 = MakePartsInstance(0, 1);
							PartsInstance partsA1_4 = MakePartsInstance(0, 1);
							PartsInstance partsA2_4 = MakePartsInstance(0, 1);
							List<SlottableItem> list_4 = new List<SlottableItem>(new SlottableItem[]{
								bowA0_4, bowA1_4, bowA2_4,
								wearA0_4, wearA1_4, wearA2_4,
								shieldA0_4, shieldA1_4, shieldA2_4,
								mWeaponA0_4, mWeaponA1_4, mWeaponA2_4,
								quiverA0_4, quiverA1_4, quiverA2_4,
								packA0_4, packA1_4, packA2_4,
								partsA0_4, partsA1_4, partsA2_4
							});
							IEnumerable<SlottableItem> expected_4 = new SlottableItem[]{
								partsA0_4, partsA1_4, partsA2_4
							};
							partsFilter = new object[]{new SGPartsFilter(), list_4, expected_4};
							yield return partsFilter;
					}
				}
		}
	}
}

