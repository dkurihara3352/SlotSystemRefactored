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
		public class FilterHandlerTests: SlotSystemTest {
			[TestCaseSource(typeof(AcceptsFilterCases))]
			public void AcceptsFilter_Various_ReturnsAccordingly(SGFilter filter, ISlottable sb, bool expected){
				FilterHandler filterHandler = new FilterHandler();
					filterHandler.SetFilter(filter);

				bool actual = filterHandler.AcceptsFilter(sb);

				Assert.That(actual, Is.EqualTo(expected));
			}
				class AcceptsFilterCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] nullF_bow_T;
							ISlottable bow_0 = MakeSubSBWithItem(MakeBowInstance(0));
							nullF_bow_T = new object[]{new SGNullFilter(), bow_0, true};
							yield return nullF_bow_T;
						object[] nullF_wear_T;
							ISlottable wear_0 = MakeSubSBWithItem(MakeWearInstance(0));
							nullF_wear_T = new object[]{new SGNullFilter(), wear_0, true};
							yield return nullF_wear_T;
						object[] nullF_shield_T;
							ISlottable shield_0 = MakeSubSBWithItem(MakeShieldInstance(0));
							nullF_shield_T = new object[]{new SGNullFilter(), shield_0, true};
							yield return nullF_shield_T;
						object[] nullF_mWeapon_T;
							ISlottable mWeapon_0 = MakeSubSBWithItem(MakeMWeaponInstance(0));
							nullF_mWeapon_T = new object[]{new SGNullFilter(), mWeapon_0, true};
							yield return nullF_mWeapon_T;
						object[] nullF_quiver_T;
							ISlottable quiver_0 = MakeSubSBWithItem(MakeQuiverInstance(0));
							nullF_quiver_T = new object[]{new SGNullFilter(), quiver_0, true};
							yield return nullF_quiver_T;
						object[] nullF_pack_T;
							ISlottable pack_0 = MakeSubSBWithItem(MakePackInstance(0));
							nullF_pack_T = new object[]{new SGNullFilter(), pack_0, true};
							yield return nullF_pack_T;
						object[] nullF_parts_T;
							ISlottable parts_0 = MakeSubSBWithItem(MakePartsInstance(0, 1));
							nullF_parts_T = new object[]{new SGNullFilter(), parts_0, true};
							yield return nullF_parts_T;
						
						object[] bowF_bow_T;
							ISlottable bow_1 = MakeSubSBWithItem(MakeBowInstance(0));
							bowF_bow_T = new object[]{new SGBowFilter(), bow_1, true};
							yield return bowF_bow_T;
						object[] bowF_wear_F;
							ISlottable wear_1 = MakeSubSBWithItem(MakeWearInstance(0));
							bowF_wear_F = new object[]{new SGBowFilter(), wear_1, false};
							yield return bowF_wear_F;
						object[] bowF_shield_F;
							ISlottable shield_1 = MakeSubSBWithItem(MakeShieldInstance(0));
							bowF_shield_F = new object[]{new SGBowFilter(), shield_1, false};
							yield return bowF_shield_F;
						object[] bowF_mWeapon_F;
							ISlottable mWeapon_1 = MakeSubSBWithItem(MakeMWeaponInstance(0));
							bowF_mWeapon_F = new object[]{new SGBowFilter(), mWeapon_1, false};
							yield return bowF_mWeapon_F;
						object[] bowF_quiver_F;
							ISlottable quiver_1 = MakeSubSBWithItem(MakeQuiverInstance(0));
							bowF_quiver_F = new object[]{new SGBowFilter(), quiver_1, false};
							yield return bowF_quiver_F;
						object[] bowF_pack_F;
							ISlottable pack_1 = MakeSubSBWithItem(MakePackInstance(0));
							bowF_pack_F = new object[]{new SGBowFilter(), pack_1, false};
							yield return bowF_pack_F;
						object[] bowF_parts_F;
							ISlottable parts_1 = MakeSubSBWithItem(MakePartsInstance(0, 1));
							bowF_parts_F = new object[]{new SGBowFilter(), parts_1, false};
							yield return bowF_parts_F;
						
						object[] wearF_bow_F;
							ISlottable bow_2 = MakeSubSBWithItem(MakeBowInstance(0));
							wearF_bow_F = new object[]{new SGWearFilter(), bow_2, false};
							yield return wearF_bow_F;
						object[] wearF_wear_T;
							ISlottable wear_2 = MakeSubSBWithItem(MakeWearInstance(0));
							wearF_wear_T = new object[]{new SGWearFilter(), wear_2, true};
							yield return wearF_wear_T;
						object[] wearF_shield_F;
							ISlottable shield_2 = MakeSubSBWithItem(MakeShieldInstance(0));
							wearF_shield_F = new object[]{new SGWearFilter(), shield_2, false};
							yield return wearF_shield_F;
						object[] wearF_mWeapon_F;
							ISlottable mWeapon_2 = MakeSubSBWithItem(MakeMWeaponInstance(0));
							wearF_mWeapon_F = new object[]{new SGWearFilter(), mWeapon_2, false};
							yield return wearF_mWeapon_F;
						object[] wearF_quiver_F;
							ISlottable quiver_2 = MakeSubSBWithItem(MakeQuiverInstance(0));
							wearF_quiver_F = new object[]{new SGWearFilter(), quiver_2, false};
							yield return wearF_quiver_F;
						object[] wearF_pack_F;
							ISlottable pack_2 = MakeSubSBWithItem(MakePackInstance(0));
							wearF_pack_F = new object[]{new SGWearFilter(), pack_2, false};
							yield return wearF_pack_F;
						object[] wearF_parts_F;
							ISlottable parts_2 = MakeSubSBWithItem(MakePartsInstance(0, 1));
							wearF_parts_F = new object[]{new SGWearFilter(), parts_2, false};
							yield return wearF_parts_F;
						
						object[] cGearsF_bow_F;
							ISlottable bow_3 = MakeSubSBWithItem(MakeBowInstance(0));
							cGearsF_bow_F = new object[]{new SGCGearsFilter(), bow_3, false};
							yield return cGearsF_bow_F;
						object[] cGearsF_wear_F;
							ISlottable wear_3 = MakeSubSBWithItem(MakeWearInstance(0));
							cGearsF_wear_F = new object[]{new SGCGearsFilter(), wear_3, false};
							yield return cGearsF_wear_F;
						object[] cGearsF_shield_T;
							ISlottable shield_3 = MakeSubSBWithItem(MakeShieldInstance(0));
							cGearsF_shield_T = new object[]{new SGCGearsFilter(), shield_3, true};
							yield return cGearsF_shield_T;
						object[] cGearsF_mWeapon_T;
							ISlottable mWeapon_3 = MakeSubSBWithItem(MakeMWeaponInstance(0));
							cGearsF_mWeapon_T = new object[]{new SGCGearsFilter(), mWeapon_3, true};
							yield return cGearsF_mWeapon_T;
						object[] cGearsF_quiver_T;
							ISlottable quiver_3 = MakeSubSBWithItem(MakeQuiverInstance(0));
							cGearsF_quiver_T = new object[]{new SGCGearsFilter(), quiver_3, true};
							yield return cGearsF_quiver_T;
						object[] cGearsF_pack_T;
							ISlottable pack_3 = MakeSubSBWithItem(MakePackInstance(0));
							cGearsF_pack_T = new object[]{new SGCGearsFilter(), pack_3, true};
							yield return cGearsF_pack_T;
						object[] cGearsF_parts_F;
							ISlottable parts_3 = MakeSubSBWithItem(MakePartsInstance(0, 1));
							cGearsF_parts_F = new object[]{new SGCGearsFilter(), parts_3, false};
							yield return cGearsF_parts_F;
						
						object[] partsF_bow_F;
							ISlottable bow_4 = MakeSubSBWithItem(MakeBowInstance(0));
							partsF_bow_F = new object[]{new SGPartsFilter(), bow_4, false};
							yield return partsF_bow_F;
						object[] partsF_wear_F;
							ISlottable wear_4 = MakeSubSBWithItem(MakeWearInstance(0));
							partsF_wear_F = new object[]{new SGPartsFilter(), wear_4, false};
							yield return partsF_wear_F;
						object[] partsF_shield_F;
							ISlottable shield_4 = MakeSubSBWithItem(MakeShieldInstance(0));
							partsF_shield_F = new object[]{new SGPartsFilter(), shield_4, false};
							yield return partsF_shield_F;
						object[] partsF_mWeapon_F;
							ISlottable mWeapon_4 = MakeSubSBWithItem(MakeMWeaponInstance(0));
							partsF_mWeapon_F = new object[]{new SGPartsFilter(), mWeapon_4, false};
							yield return partsF_mWeapon_F;
						object[] partsF_quiver_F;
							ISlottable quiver_4 = MakeSubSBWithItem(MakeQuiverInstance(0));
							partsF_quiver_F = new object[]{new SGPartsFilter(), quiver_4, false};
							yield return partsF_quiver_F;
						object[] partsF_pack_F;
							ISlottable pack_4 = MakeSubSBWithItem(MakePackInstance(0));
							partsF_pack_F = new object[]{new SGPartsFilter(), pack_4, false};
							yield return partsF_pack_F;
						object[] partsF_parts_T;
							ISlottable parts_4 = MakeSubSBWithItem(MakePartsInstance(0, 1));
							partsF_parts_T = new object[]{new SGPartsFilter(), parts_4, true};
							yield return partsF_parts_T;
					}
				}
		/* helper */
			static ISlottable MakeSubSBWithItem(IInventoryItemInstance item){
				ISlottable stubSB = MakeSubSB();
					stubSB.GetItem().Returns(item);
				return stubSB;
			}
		}
	}
}

