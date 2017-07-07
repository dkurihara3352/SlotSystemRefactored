using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class BowInstance: InventoryItemInstance{
		public BowInstance(){
			this.Quantity = 1;
		}
	}
	public class WearInstance: InventoryItemInstance{
		public WearInstance(){
			this.Quantity = 1;
		}
	}
	public class CarriedGearInstance: InventoryItemInstance{}
	public class ShieldInstance: CarriedGearInstance{
		public ShieldInstance(){
			this.Quantity = 1;
		}
	}
	public class MeleeWeaponInstance: CarriedGearInstance{
		public MeleeWeaponInstance(){
			this.Quantity = 1;
		}
	}
	public class QuiverInstance: CarriedGearInstance{
		public QuiverInstance(){
			this.Quantity = 1;
		}
	}
	public class PackInstance: CarriedGearInstance{
		public PackInstance(){
			this.Quantity = 1;
		}
	}
	public class PartsInstance: InventoryItemInstance{}
}
