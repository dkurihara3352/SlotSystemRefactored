using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class BowInstance: InventoryItemInstance{
		public BowInstance(){
			this.SetQuantity(1);
		}
		public override bool IsContainedInEquippedItems(IEquippedProvider equipProv){
			BowInstance equippedBow = equipProv.GetEquippedBowInst();
			if(Equals(equippedBow))
				return true;
			return false;
		}
	}
	public class WearInstance: InventoryItemInstance{
		public WearInstance(){
			this.SetQuantity(1);
		}
		public override bool IsContainedInEquippedItems(IEquippedProvider equipProv){
			WearInstance equippedWear = equipProv.GetEquippedWearInst();
			if(Equals(equippedWear))
				return true;
			return false;
		}
	}
	public class CarriedGearInstance: InventoryItemInstance{
		public override bool IsContainedInEquippedItems(IEquippedProvider equipProv){
			List<CarriedGearInstance> equippedCarriedGears = equipProv.GetEquippedCarriedGears();
			if(equippedCarriedGears.Contains(this))
				return true;
			return false;
		}
	}
	public class ShieldInstance: CarriedGearInstance{
		public ShieldInstance(){
			this.SetQuantity(1);
		}
	}
	public class MeleeWeaponInstance: CarriedGearInstance{
		public MeleeWeaponInstance(){
			this.SetQuantity(1);
		}
	}
	public class QuiverInstance: CarriedGearInstance{
		public QuiverInstance(){
			this.SetQuantity(1);
		}
	}
	public class PackInstance: CarriedGearInstance{
		public PackInstance(){
			this.SetQuantity(1);
		}
	}
	public class PartsInstance: InventoryItemInstance{
		public override bool IsContainedInEquippedItems(IEquippedProvider equipProv){
			List<PartsInstance> equippedParts = equipProv.GetEquippedParts();
			if(equippedParts.Contains(this))
				return true;
			else
				return false;
		}
	}
}
