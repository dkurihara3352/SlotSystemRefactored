using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class BowInstance: InventoryItemInstance{
		public BowInstance(){
			this.quantity = 1;
		}
	}
	public class WearInstance: InventoryItemInstance{
		public WearInstance(){
			this.quantity = 1;
		}
	}
	public class CarriedGearInstance: InventoryItemInstance{}
	public class ShieldInstance: CarriedGearInstance{
		public ShieldInstance(){
			this.quantity = 1;
		}
	}
	public class MeleeWeaponInstance: CarriedGearInstance{
		public MeleeWeaponInstance(){
			this.quantity = 1;
		}
	}
	public class QuiverInstance: CarriedGearInstance{
		public QuiverInstance(){
			this.quantity = 1;
		}
	}
	public class PackInstance: CarriedGearInstance{
		public PackInstance(){
			this.quantity = 1;
		}
	}
	public class PartsInstance: InventoryItemInstance{}
}
