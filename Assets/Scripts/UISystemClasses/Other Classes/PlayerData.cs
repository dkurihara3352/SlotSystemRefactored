using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class FakeSlotSystemPlayerData: ISlotSystemPlayerData{
		public int GetEquippableCarriedGearsCount(){
			return 0;
		}
		public IPoolInventory GetInventory(){
			return new PoolInventory();
		}
	}
	public interface ISlotSystemPlayerData{
		int GetEquippableCarriedGearsCount();
		IPoolInventory GetInventory();
	}
}
