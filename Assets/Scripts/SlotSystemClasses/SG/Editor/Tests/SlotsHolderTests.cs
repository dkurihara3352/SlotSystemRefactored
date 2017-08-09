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
		public class SlotsHolderTests: SlotSystemTest {
			[Test]
			public void hasEmptySlot_SlotsWithNullMember_ReturnsTrue(){
				SlotsHolder slotsHolder = new SlotsHolder(MakeSubSG());
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
					slotsHolder.SetSlots(slots);

				bool actual = slotsHolder.hasEmptySlot;

				Assert.That(actual, Is.True);
			}
			[Test]
			public void hasEmptySlot_SlotsWithNoNullMember_ReturnsFalse(){
				SlotsHolder slotsHolder = new SlotsHolder(MakeSubSG());
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
					slotsHolder.SetSlots(slots);

				bool actual = slotsHolder.hasEmptySlot;

				Assert.That(actual, Is.False);
			}
			[TestCaseSource(typeof(GetNewSlotCases))]
			public void GetNewSlot_itemFound_ReturnsNewSlot(List<ISlottable> sbs ,List<Slot> newSlots, InventoryItemInstance item, Slot expected){
				SlotsHolder slotsHolder;
					ISlotGroup sg = MakeSubSG();
					sg.GetEnumerator().Returns(SSEsFromISlottables(sbs).GetEnumerator());
					slotsHolder = new SlotsHolder(sg);
					slotsHolder.SetNewSlots(newSlots);
				
				Slot actual = slotsHolder.GetNewSlot(item);

				Assert.That(actual, Is.SameAs(expected));
			}
				class GetNewSlotCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] valid0;
							ISlottable bowSB_0 = MakeSubSB();
								BowInstance bow_0 = MakeBowInstance(0);
								bowSB_0.newSlotID.Returns(0);
								bowSB_0.item.Returns(bow_0);
							ISlottable wearSB_0 = MakeSubSB();
								WearInstance wear_0 = MakeWearInstance(0);
								wearSB_0.newSlotID.Returns(1);
								wearSB_0.item.Returns(wear_0);
							ISlottable shieldSB_0 = MakeSubSB();
								ShieldInstance shield_0 = MakeShieldInstance(0);
								shieldSB_0.newSlotID.Returns(2);
								shieldSB_0.item.Returns(shield_0);
							ISlottable mWeaponSB_0 = MakeSubSB();
								MeleeWeaponInstance mWeapon_0 = MakeMeleeWeaponInstance(0);
								mWeaponSB_0.newSlotID.Returns(3);
								mWeaponSB_0.item.Returns(mWeapon_0);
							List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{bowSB_0, wearSB_0, shieldSB_0, mWeaponSB_0});
							List<Slot> newSlots_0 = new List<Slot>();
								foreach(var sb in sbs_0)
									newSlots_0.Add(new Slot());
							valid0 = new object[]{sbs_0, newSlots_0, bow_0, newSlots_0[0]};
							yield return valid0;
						object[] valid1;
							ISlottable bowSB_1 = MakeSubSB();
								BowInstance bow_1 = MakeBowInstance(0);
								bowSB_1.newSlotID.Returns(0);
								bowSB_1.item.Returns(bow_1);
							ISlottable wearSB_1 = MakeSubSB();
								WearInstance wear_1 = MakeWearInstance(0);
								wearSB_1.newSlotID.Returns(1);
								wearSB_1.item.Returns(wear_1);
							ISlottable shieldSB_1 = MakeSubSB();
								ShieldInstance shield_1 = MakeShieldInstance(0);
								shieldSB_1.newSlotID.Returns(2);
								shieldSB_1.item.Returns(shield_1);
							ISlottable mWeaponSB_1 = MakeSubSB();
								MeleeWeaponInstance mWeapon_1 = MakeMeleeWeaponInstance(0);
								mWeaponSB_1.newSlotID.Returns(3);
								mWeaponSB_1.item.Returns(mWeapon_1);
							List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{bowSB_1, wearSB_1, shieldSB_1, mWeaponSB_1});
							List<Slot> newSlots_1 = new List<Slot>();
								foreach(var sb in sbs_1)
									newSlots_1.Add(new Slot());
							valid1 = new object[]{sbs_1, newSlots_1, wear_1, newSlots_1[1]};
							yield return valid1;
						object[] valid2;
							ISlottable bowSB_2 = MakeSubSB();
								BowInstance bow_2 = MakeBowInstance(0);
								bowSB_2.newSlotID.Returns(0);
								bowSB_2.item.Returns(bow_2);
							ISlottable wearSB_2 = MakeSubSB();
								WearInstance wear_2 = MakeWearInstance(0);
								wearSB_2.newSlotID.Returns(1);
								wearSB_2.item.Returns(wear_2);
							ISlottable shieldSB_2 = MakeSubSB();
								ShieldInstance shield_2 = MakeShieldInstance(0);
								shieldSB_2.newSlotID.Returns(2);
								shieldSB_2.item.Returns(shield_2);
							ISlottable mWeaponSB_2 = MakeSubSB();
								MeleeWeaponInstance mWeapon_2 = MakeMeleeWeaponInstance(0);
								mWeaponSB_2.newSlotID.Returns(3);
								mWeaponSB_2.item.Returns(mWeapon_2);
							List<ISlottable> sbs_2 = new List<ISlottable>(new ISlottable[]{bowSB_2, wearSB_2, shieldSB_2, mWeaponSB_2});
							List<Slot> newSlots_2 = new List<Slot>();
								foreach(var sb in sbs_2)
									newSlots_2.Add(new Slot());
							valid2 = new object[]{sbs_2, newSlots_2, shield_2, newSlots_2[2]};
							yield return valid2;
						object[] valid3;
							ISlottable bowSB_3 = MakeSubSB();
								BowInstance bow_3 = MakeBowInstance(0);
								bowSB_3.newSlotID.Returns(0);
								bowSB_3.item.Returns(bow_3);
							ISlottable wearSB_3 = MakeSubSB();
								WearInstance wear_3 = MakeWearInstance(0);
								wearSB_3.newSlotID.Returns(1);
								wearSB_3.item.Returns(wear_3);
							ISlottable shieldSB_3 = MakeSubSB();
								ShieldInstance shield_3 = MakeShieldInstance(0);
								shieldSB_3.newSlotID.Returns(2);
								shieldSB_3.item.Returns(shield_3);
							ISlottable mWeaponSB_3 = MakeSubSB();
								MeleeWeaponInstance mWeapon_3 = MakeMeleeWeaponInstance(0);
								mWeaponSB_3.newSlotID.Returns(3);
								mWeaponSB_3.item.Returns(mWeapon_3);
							List<ISlottable> sbs_3 = new List<ISlottable>(new ISlottable[]{bowSB_3, wearSB_3, shieldSB_3, mWeaponSB_3});
							List<Slot> newSlots_3 = new List<Slot>();
								foreach(var sb in sbs_3)
									newSlots_3.Add(new Slot());
							valid3 = new object[]{sbs_3, newSlots_3, mWeapon_3, newSlots_3[3]};
							yield return valid3;
						object[] invalid;
							ISlottable bowSB_4 = MakeSubSB();
								BowInstance bow_4 = MakeBowInstance(0);
								bowSB_4.newSlotID.Returns(0);
								bowSB_4.item.Returns(bow_4);
							ISlottable wearSB_4 = MakeSubSB();
								WearInstance wear_4 = MakeWearInstance(0);
								wearSB_4.newSlotID.Returns(1);
								wearSB_4.item.Returns(wear_4);
							ISlottable shieldSB_4 = MakeSubSB();
								ShieldInstance shield_4 = MakeShieldInstance(0);
								shieldSB_4.newSlotID.Returns(2);
								shieldSB_4.item.Returns(shield_4);
							ISlottable mWeaponSB_4 = MakeSubSB();
								MeleeWeaponInstance mWeapon_4 = MakeMeleeWeaponInstance(0);
								mWeaponSB_4.newSlotID.Returns(3);
								mWeaponSB_4.item.Returns(mWeapon_4);
							List<ISlottable> sbs_4 = new List<ISlottable>(new ISlottable[]{bowSB_4, wearSB_4, shieldSB_4, mWeaponSB_4});
							List<Slot> newSlots_4 = new List<Slot>();
								foreach(var sb in sbs_4)
									newSlots_4.Add(new Slot());
							invalid = new object[]{sbs_4, newSlots_4, MakeBowInstance(0), null};
							yield return invalid;
					}
				}
				IEnumerable<ISlotSystemElement> SSEsFromISlottables(IEnumerable<ISlottable> sbs){
					foreach(ISlottable sb in sbs)
						yield return (ISlotSystemElement)sb;
				}
		}
	}
}
