using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlotSystem{
	public class EquipmentSetInventory: IEquipmentSetInventory{
		public IEnumerator<InventoryItemInstance> GetEnumerator(){
			foreach(InventoryItemInstance item in m_items){
				yield return item;
			}
			}IEnumerator IEnumerable.GetEnumerator(){
				return GetEnumerator();
			}
		public EquipmentSetInventory(BowInstance initBow, WearInstance initWear, List<CarriedGearInstance> initCGears ,int initCGCount){
			m_equippedBow = initBow;
			m_equippedWear = initWear;
			m_equippedCGears = initCGears;
			SetEquippableCGearsCount(initCGCount);
		}
		public bool Contains(InventoryItemInstance item){
			foreach(InventoryItemInstance it in this){
				if(it == item)
					return true;
			}
			return false;
		}
		public int count{
			get{return m_items.Count;}
		}
		public InventoryItemInstance this[int i]{
			get{return m_items[i];}
		}
		public ISlotGroup sg{get{return m_sg;}}
			ISlotGroup m_sg;
			public void SetSG(ISlotGroup sg){
				m_sg = sg;
			}
		protected BowInstance m_equippedBow;
		protected WearInstance m_equippedWear;
		protected List<CarriedGearInstance> m_equippedCGears = new List<CarriedGearInstance>();
		public int equippableCGearsCount{
			get{return m_equippableCGearsCount;}
			}int m_equippableCGearsCount;
		public void SetEquippableCGearsCount(int num){
			m_equippableCGearsCount = num;
			if(sg != null && sg.filter is SGCGearsFilter && !sg.isExpandable)
			sg.SetInitSlotsCount(num);
		}
		
		protected List<InventoryItemInstance> m_items{
			get{
				List<InventoryItemInstance> result = new List<InventoryItemInstance>();
				if(m_equippedBow != null)
					result.Add(m_equippedBow);
				if(m_equippedWear != null)
					result.Add(m_equippedWear);
				if(m_equippedCGears.Count != 0){
					foreach(CarriedGearInstance inst in m_equippedCGears){
						result.Add((InventoryItemInstance)inst);
					}
				}
				return result;
			}
		}
		public void Add(InventoryItemInstance item){
			if(item != null){
				if(item is BowInstance){
					BowInstance bowInst = (BowInstance)item;
					m_equippedBow = bowInst;
				}	
				else if(item is WearInstance){
					WearInstance wearInst = (WearInstance)item;
					m_equippedWear = wearInst;
				}
				else if(item is CarriedGearInstance){
					if(m_equippedCGears.Count < m_equippableCGearsCount)
						m_equippedCGears.Add((CarriedGearInstance)item);
					else
						throw new InvalidOperationException("EquipmentSetInventory.Add: trying to add a CarriedGear exceeding the maximum allowed count");
				}
			}else
				throw new ArgumentNullException();
		}
		public void Remove(InventoryItemInstance removedItem){
			if(removedItem != null){
				if(removedItem is BowInstance){
					if((BowInstance)removedItem == m_equippedBow)
						m_equippedBow = null;
				}else if(removedItem is WearInstance){
					if((WearInstance)removedItem == m_equippedWear)
						m_equippedWear = null;
				}else if(removedItem is CarriedGearInstance){
					CarriedGearInstance spottedOne = null;
					foreach(CarriedGearInstance cgInst in m_equippedCGears){
						if((CarriedGearInstance)removedItem == cgInst)
							spottedOne = cgInst;
					}
					if(spottedOne != null)
						m_equippedCGears.Remove(spottedOne);
				}
			}else
				throw new ArgumentNullException();
		}
	}
	public interface IEquipmentSetInventory: Inventory{
		int equippableCGearsCount{get;}
		void SetEquippableCGearsCount(int num);
	}
}