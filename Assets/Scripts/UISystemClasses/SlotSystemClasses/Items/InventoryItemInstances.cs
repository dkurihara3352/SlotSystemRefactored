using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class BowInstance: InventoryItemInstance{
		public BowInstance(BowFake bow){
			SetInventoryItem(bow);
			SetQuantity(1);
		}
		public override bool IsContainedInEquippedItems(IEquippedElementsProvider equipProv){
			BowInstance equippedBow = equipProv.GetBowInFocusedSGEBow();
			if(Equals(equippedBow))
				return true;
			return false;
		}
	}
	public class WearInstance: InventoryItemInstance{
		public WearInstance(WearFake wear){
			SetInventoryItem(wear);
			SetQuantity(1);
		}
		public override bool IsContainedInEquippedItems(IEquippedElementsProvider equipProv){
			WearInstance equippedWear = equipProv.GetWearInFocusedSGEWear();
			if(Equals(equippedWear))
				return true;
			return false;
		}
	}
	public class CarriedGearInstance: InventoryItemInstance{
		public override bool IsContainedInEquippedItems(IEquippedElementsProvider equipProv){
			List<CarriedGearInstance> equippedCarriedGears = equipProv.GetCGearsInFocusedSGECGears();
			if(equippedCarriedGears.Contains(this))
				return true;
			return false;
		}
	}
	public class ShieldInstance: CarriedGearInstance{
		public ShieldInstance(ShieldFake shield){
			SetInventoryItem(shield);
			SetQuantity(1);
		}
	}
	public class MeleeWeaponInstance: CarriedGearInstance{
		public MeleeWeaponInstance(MeleeWeaponFake mWeapon){
			SetInventoryItem(mWeapon);
			SetQuantity(1);
		}
	}
	public class QuiverInstance: CarriedGearInstance{
		public QuiverInstance(QuiverFake quiver){
			SetInventoryItem(quiver);
			SetQuantity(1);
		}
	}
	public class PackInstance: CarriedGearInstance{
		public PackInstance(PackFake pack){
			SetInventoryItem(pack);
			SetQuantity(1);
		}
	}
	public class PartsInstance: InventoryItemInstance{
		public PartsInstance(PartsFake partsFake, int quantity){
			SetInventoryItem(partsFake);
			SetQuantity(quantity);
		}
	}
}
