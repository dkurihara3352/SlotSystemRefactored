using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class BowFake: InventoryItem{
		public BowFake(){
			SetStackability(false);
		}
	}
	public class WearFake: InventoryItem{
		public WearFake(){
			SetStackability(false);
		}
	}
	public abstract class CarriedGearFake: InventoryItem{}
	public class MeleeWeaponFake: CarriedGearFake{
		public MeleeWeaponFake(){
			SetStackability(false);
		}
	}
	public class PackFake: CarriedGearFake{
		public PackFake(){
			SetStackability(false);
		}
	}
	public class QuiverFake: CarriedGearFake{
		public QuiverFake(){
			SetStackability(false);
		}
	}
	public class ShieldFake: CarriedGearFake{
		public ShieldFake(){
			SetStackability(false);
		}
	}
	public class PartsFake: InventoryItem{
		public PartsFake(){
			SetStackability(true);
		}
	}
}
