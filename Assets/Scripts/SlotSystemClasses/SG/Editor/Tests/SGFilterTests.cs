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
		public class SGFilterTests: SlotSystemTest {

			[TestCaseSource(typeof(SGBowFilterCases))]
			public void SGBowFilter_Filter_WhenCalled_SetsItemsAccordingly(SGFilter targFilter, List<SlottableItem> target, List<SlottableItem> expected){
				SGFilter filter = targFilter;
				List<SlottableItem> list = target;

				filter.Filter(ref list);

				Assert.That(list, Is.EqualTo(expected));
			}
				class SGBowFilterCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						BowInstance bow = MakeBowInstance(0);
						BowInstance bow2 = MakeBowInstance(0);
						BowInstance bow3 = MakeBowInstance(0);
						WearInstance wear = MakeWearInstance(0);
						WearInstance wear2 = MakeWearInstance(0);
						WearInstance wear3 = MakeWearInstance(0);
						ShieldInstance shield = MakeShieldInstance(0);
						ShieldInstance shield2 = MakeShieldInstance(0);
						ShieldInstance shield3 = MakeShieldInstance(0);
						MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(0);
						MeleeWeaponInstance mWeapon2 = MakeMeleeWeaponInstance(0);
						MeleeWeaponInstance mWeapon3 = MakeMeleeWeaponInstance(0);
						QuiverInstance quiver = MakeQuiverInstance(0);
						QuiverInstance quiver2 = MakeQuiverInstance(0);
						QuiverInstance quiver3 = MakeQuiverInstance(0);
						PackInstance pack = MakePackInstance(0);
						PackInstance pack2 = MakePackInstance(0);
						PackInstance pack3 = MakePackInstance(0);
						PartsInstance parts = MakePartsInstance(0, 1);
						PartsInstance parts2 = MakePartsInstance(0, 1);
						PartsInstance parts3 = MakePartsInstance(0, 1);
						List<SlottableItem> list = new List<SlottableItem>(new SlottableItem[]{
							bow, bow2, bow3,
							wear, wear2, wear3,
							shield, shield2, shield3,
							mWeapon, mWeapon2, mWeapon3,
							quiver, quiver2, quiver3,
							pack, pack2, pack3,
							parts, parts2, parts3,
						});
						yield return new object[]{
							new SGBowFilter(), list,
							new List<SlottableItem>(new SlottableItem[]{
								bow, bow2, bow3
							})
						};
						yield return new object[]{
							new SGWearFilter(), list,
							new List<SlottableItem>(new SlottableItem[]{
								wear, wear2, wear3
							})
						};
						yield return new object[]{
							new SGCGearsFilter(), list,
							new List<SlottableItem>(new SlottableItem[]{
								shield, shield2, shield3,
								mWeapon, mWeapon2, mWeapon3,
								quiver, quiver2, quiver3,
								pack, pack2, pack3,
							})
						};
						yield return new object[]{
							new SGPartsFilter(), list,
							new List<SlottableItem>(new SlottableItem[]{
								parts, parts2, parts3,
							})
						};
					}
				}
		}
	}
}

