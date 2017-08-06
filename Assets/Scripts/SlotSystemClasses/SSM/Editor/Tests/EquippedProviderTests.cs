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
	[TestFixture]
	[Category("SSM")]
	public class EquippedProviderTests : SlotSystemTest{

		[Test]
		public void equippedWearInst_Always_ReturnsFocusedSGEWithWearFilterFirtSlotSBItemInst(){
			EquippedProvider equiProv;
				ISlotGroup sgeWear = MakeSubSG();
					ISlottable wearSBE = MakeSubSB();
						WearInstance wearE = MakeWearInstance(0);
						wearSBE.item.Returns(wearE);
						WearInstance expected = wearE;
					sgeWear[0].Returns(wearSBE);
					sgeWear.filter.Returns(new SGWearFilter());
				IFocusedSGProvider stubFocSGProv = Substitute.For<IFocusedSGProvider>();
					stubFocSGProv.focusedSGEWear.Returns(sgeWear);
				equiProv = new EquippedProvider(stubFocSGProv);

			WearInstance actual = equiProv.equippedWearInst;

			Assert.That(actual, Is.SameAs(expected));
		}
		[Test]
		public void equippedCarriedGears_Always_ReturnsFocusedSGEWithCGFilterAllElements(){
			EquippedProvider equiProv;
				ISlotGroup sgeCGears = MakeSubSG();
					IEnumerable<ISlotSystemElement> sgeCGearsEles;
						ISlottable shieldSBE = MakeSubSB();
							ShieldInstance shieldE = MakeShieldInstance(0);
							shieldSBE.item.Returns(shieldE);
						ISlottable mWeaponSBE = MakeSubSB();
							MeleeWeaponInstance mWeaponE = MakeMeleeWeaponInstance(0);
							mWeaponSBE.item.Returns(mWeaponE);
						sgeCGearsEles = new ISlotSystemElement[]{shieldSBE, mWeaponSBE};
						List<CarriedGearInstance> expected = new List<CarriedGearInstance>(new CarriedGearInstance[]{shieldE, mWeaponE});
					sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
					sgeCGears.filter.Returns(new SGCGearsFilter());
				IFocusedSGProvider stubFocSGProv = Substitute.For<IFocusedSGProvider>();
				stubFocSGProv.focusedSGECGears.Returns(sgeCGears);
				equiProv = new EquippedProvider(stubFocSGProv);

			IEnumerable<CarriedGearInstance> actual = equiProv.equippedCarriedGears;

			Assert.That(actual.MemberEquals(expected), Is.True);
		}
		[Test]
		public void allEquippedItems_Always_ReturnsSumOfAllThree(){
			EquippedProvider equiProv;
				IFocusedSGProvider stubFocSGProv = Substitute.For<IFocusedSGProvider>();
					ISlotGroup sgeBow = MakeSubSG();
						ISlottable bowSBE = MakeSubSB();
							BowInstance bowE = MakeBowInstance(0);
							bowSBE.item.Returns(bowE);
						sgeBow[0].Returns(bowSBE);
						sgeBow.filter.Returns(new SGBowFilter());
					ISlotGroup sgeWear = MakeSubSG();
						ISlottable wearSBE = MakeSubSB();
							WearInstance wearE = MakeWearInstance(0);
							wearSBE.item.Returns(wearE);
						sgeWear[0].Returns(wearSBE);
						sgeWear.filter.Returns(new SGWearFilter());
					ISlotGroup sgeCGears = MakeSubSG();
						IEnumerable<ISlotSystemElement> sgeCGearsEles;
							ISlottable shieldSBE = MakeSubSB();
								ShieldInstance shieldE = MakeShieldInstance(0);
								shieldSBE.item.Returns(shieldE);
							ISlottable mWeaponSBE = MakeSubSB();
								MeleeWeaponInstance mWeaponE = MakeMeleeWeaponInstance(0);
								mWeaponSBE.item.Returns(mWeaponE);
							sgeCGearsEles = new ISlotSystemElement[]{shieldSBE, mWeaponSBE};
						sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
						sgeCGears.filter.Returns(new SGCGearsFilter());
					stubFocSGProv.focusedSGEBow.Returns(sgeBow);
					stubFocSGProv.focusedSGEWear.Returns(sgeWear);
					stubFocSGProv.focusedSGECGears.Returns(sgeCGears);
				equiProv = new EquippedProvider(stubFocSGProv);
				List<InventoryItemInstance> expected = new List<InventoryItemInstance>(new InventoryItemInstance[]{
					bowE, wearE, shieldE, mWeaponE
				});

			List<InventoryItemInstance> actual = equiProv.allEquippedItems;

			Assert.That(actual.MemberEquals(expected), Is.True);
		}
	}
}
