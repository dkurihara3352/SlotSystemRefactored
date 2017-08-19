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
						wearSBE.GetItem().Returns(wearE);
						WearInstance expected = wearE;
					sgeWear[0].Returns(wearSBE);
					IFilterHandler filterHandler = Substitute.For<IFilterHandler>();
					sgeWear.GetFilterHandler().Returns(filterHandler);
					filterHandler.GetFilter().Returns(new SGWearFilter());
				IFocusedSGProvider stubFocSGProv = Substitute.For<IFocusedSGProvider>();
					stubFocSGProv.GetFocusedSGEWear().Returns(sgeWear);
				equiProv = new EquippedProvider(stubFocSGProv);

			WearInstance actual = equiProv.GetEquippedWearInst();

			Assert.That(actual, Is.SameAs(expected));
		}
		[Test]
		public void equippedCarriedGears_Always_ReturnsFocusedSGEWithCGFilterAllElements(){
			EquippedProvider equiProv;
				ISlotGroup sgeCGears = MakeSubSG();
					IEnumerable<ISlotSystemElement> sgeCGearsEles;
						ISlottable shieldSBE = MakeSubSB();
							ShieldInstance shieldE = MakeShieldInstance(0);
							shieldSBE.GetItem().Returns(shieldE);
						ISlottable mWeaponSBE = MakeSubSB();
							MeleeWeaponInstance mWeaponE = MakeMWeaponInstance(0);
							mWeaponSBE.GetItem().Returns(mWeaponE);
						sgeCGearsEles = new ISlotSystemElement[]{shieldSBE, mWeaponSBE};
						List<CarriedGearInstance> expected = new List<CarriedGearInstance>(new CarriedGearInstance[]{shieldE, mWeaponE});
					sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
					IFilterHandler filterHandler = Substitute.For<IFilterHandler>();
						filterHandler.GetFilter().Returns(new SGCGearsFilter());
					sgeCGears.GetFilterHandler().Returns(filterHandler);
				IFocusedSGProvider stubFocSGProv = Substitute.For<IFocusedSGProvider>();
				stubFocSGProv.GetFocusedSGECGears().Returns(sgeCGears);
				equiProv = new EquippedProvider(stubFocSGProv);

			IEnumerable<CarriedGearInstance> actual = equiProv.GetEquippedCarriedGears();

			Assert.That(actual.MemberEquals(expected), Is.True);
		}
		[Test]
		public void allEquippedItems_Always_ReturnsSumOfAllThree(){
			EquippedProvider equiProv;
				IFocusedSGProvider stubFocSGProv = Substitute.For<IFocusedSGProvider>();
					ISlotGroup sgeBow = MakeSubSG();
						ISlottable bowSBE = MakeSubSB();
							BowInstance bowE = MakeBowInstance(0);
							bowSBE.GetItem().Returns(bowE);
						sgeBow[0].Returns(bowSBE);
						IFilterHandler sgeBowFilterHandler = Substitute.For<IFilterHandler>();
							sgeBowFilterHandler.GetFilter().Returns(new SGBowFilter());
						sgeBow.GetFilterHandler().Returns(sgeBowFilterHandler);
					ISlotGroup sgeWear = MakeSubSG();
						ISlottable wearSBE = MakeSubSB();
							WearInstance wearE = MakeWearInstance(0);
							wearSBE.GetItem().Returns(wearE);
						sgeWear[0].Returns(wearSBE);
						IFilterHandler sgeWearFilterHandler = Substitute.For<IFilterHandler>();
							sgeWearFilterHandler.GetFilter().Returns(new SGWearFilter());
						sgeWear.GetFilterHandler().Returns(sgeWearFilterHandler);
					ISlotGroup sgeCGears = MakeSubSG();
						IEnumerable<ISlotSystemElement> sgeCGearsEles;
							ISlottable shieldSBE = MakeSubSB();
								ShieldInstance shieldE = MakeShieldInstance(0);
								shieldSBE.GetItem().Returns(shieldE);
							ISlottable mWeaponSBE = MakeSubSB();
								MeleeWeaponInstance mWeaponE = MakeMWeaponInstance(0);
								mWeaponSBE.GetItem().Returns(mWeaponE);
							sgeCGearsEles = new ISlotSystemElement[]{shieldSBE, mWeaponSBE};
						sgeCGears.GetEnumerator().Returns(sgeCGearsEles.GetEnumerator());
						IFilterHandler sgeCGearsFilterHandler = Substitute.For<IFilterHandler>();
							sgeCGearsFilterHandler.GetFilter().Returns(new SGCGearsFilter());
						sgeCGears.GetFilterHandler().Returns(sgeCGearsFilterHandler);
					stubFocSGProv.GetFocusedSGEBow().Returns(sgeBow);
					stubFocSGProv.GetFocusedSGEWear().Returns(sgeWear);
					stubFocSGProv.GetFocusedSGECGears().Returns(sgeCGears);
				equiProv = new EquippedProvider(stubFocSGProv);
				List<IInventoryItemInstance> expected = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
					bowE, wearE, shieldE, mWeaponE
				});

			List<IInventoryItemInstance> actual = equiProv.GetAllEquippedItems();

			Assert.That(actual.MemberEquals(expected), Is.True);
		}
	}
}
