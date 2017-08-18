using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class BowFake: InventoryItem{
		public BowFake(){
			SetIsStackable(false);
		}
	}
	public class WearFake: InventoryItem{
		public WearFake(){
			SetIsStackable(false);
		}
	}
	public abstract class CarriedGearFake: InventoryItem{}
	public class MeleeWeaponFake: CarriedGearFake{
		public MeleeWeaponFake(){
			SetIsStackable(false);
		}
	}
	public class PackFake: CarriedGearFake{
		public PackFake(){
			SetIsStackable(false);
		}
	}
	public class QuiverFake: CarriedGearFake{
		public QuiverFake(){
			SetIsStackable(false);
		}
	}
	public class ShieldFake: CarriedGearFake{
		public ShieldFake(){
			SetIsStackable(false);
		}
	}
	public class PartsFake: InventoryItem{
		public PartsFake(){
			SetIsStackable(true);
		}
	}
}
