﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlotSystem{
	public class EquipmentSetInventory: Inventory{
		public IEnumerator<SlottableItem> GetEnumerator(){
			foreach(SlottableItem item in m_items){
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
		public bool Contains(SlottableItem item){
			foreach(SlottableItem it in this){
				if(it == item)
					return true;
			}
			return false;
		}
		public int count{
			get{return m_items.Count;}
		}
		public SlottableItem this[int i]{
			get{return m_items[i];}
		}
		public SlotGroup sg{get{return m_sg;}}
			SlotGroup m_sg;
			public void SetSG(SlotGroup sg){
				m_sg = sg;
			}
		BowInstance m_equippedBow;
		WearInstance m_equippedWear;
		List<CarriedGearInstance> m_equippedCGears = new List<CarriedGearInstance>();
		public int equippableCGearsCount{
			get{return m_equippableCGearsCount;}
			}int m_equippableCGearsCount;
		public void SetEquippableCGearsCount(int num){
			m_equippableCGearsCount = num;
			if(sg != null && sg.Filter is SGCGearsFilter && !sg.isExpandable)
			sg.SetInitSlotsCount(num);
		}
		
		List<SlottableItem> m_items{
			get{
				List<SlottableItem> result = new List<SlottableItem>();
				if(m_equippedBow != null)
					result.Add(m_equippedBow);
				if(m_equippedWear != null)
					result.Add(m_equippedWear);
				if(m_equippedCGears.Count != 0){
					foreach(CarriedGearInstance inst in m_equippedCGears){
						result.Add((SlottableItem)inst);
					}
				}
				return result;
			}
		}
		public void Add(SlottableItem item){
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
						throw new InvalidOperationException("trying to add a CarriedGear exceeding the maximum allowed count");
				}
			}
		}
		public void Remove(SlottableItem removedItem){
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
			}
		}
	}
}