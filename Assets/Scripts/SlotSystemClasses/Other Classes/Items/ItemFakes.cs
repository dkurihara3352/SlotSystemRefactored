using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class BowFake: InventoryItem{
		public BowFake(){
			IsStackable = false;
		}
	}
	public class WearFake: InventoryItem{
		public WearFake(){
			IsStackable = false;
		}
	}
	public abstract class CarriedGearFake: InventoryItem{}
	public class MeleeWeaponFake: CarriedGearFake{
		public MeleeWeaponFake(){
			IsStackable = false;
		}
	}
	public class PackFake: CarriedGearFake{
		public PackFake(){
			IsStackable = false;
		}
	}
	public class QuiverFake: CarriedGearFake{
		public QuiverFake(){
			IsStackable = false;
		}
	}
	public class ShieldFake: CarriedGearFake{
		public ShieldFake(){
			IsStackable = false;
		}
	}
	public class PartsFake: InventoryItem{
		public PartsFake(){
			IsStackable = true;
		}
	}
}
