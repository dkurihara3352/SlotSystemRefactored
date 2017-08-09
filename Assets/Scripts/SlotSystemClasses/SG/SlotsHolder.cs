using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SlotsHolder : ISlotsHolder {
		public SlotsHolder(ISlotGroup sg){
			this.sg = sg;
			SetNewSlots(new List<Slot>());
			SetSlots(new List<Slot>());
		}
		ISlotGroup sg;
		public Slot GetNewSlot(InventoryItemInstance itemInst){
			int index = -3;
			foreach(ISlottable sb in sg){
				if(sb != null){
					if(sb.item == itemInst)
						index = sb.newSlotID;
				}
			}
			if(index != -3)
				return newSlots[index];
			else 
				return null;
		}
		public List<Slot> newSlots{
			get{
				if(_newSlots != null)
					return _newSlots;
				else
					throw new InvalidOperationException("newSlots not set");
			}
		}
			List<Slot> _newSlots;
			public void SetNewSlots(List<Slot> newSlots){
				_newSlots = newSlots;
			}
		public List<Slot> slots{
			get{
				if(m_slots != null)
					return m_slots;
				else throw new InvalidOperationException("slots not set");
			}
		}
			List<Slot> m_slots;
			public void SetSlots(List<Slot> slots){
				m_slots = slots;
			}
		public bool hasEmptySlot{
			get{
				bool emptyFound = false;
				foreach(Slot slot in slots){
					if(slot.sb == null)
						emptyFound = true;
				}
				return emptyFound;
			}
		}
		public void SetInitSlotsCount(int i){
			_initSlotsCount = i;
		}
			public int initSlotsCount{
				get{
					if(_initSlotsCount != -1)
						return _initSlotsCount;
					else
						throw new InvalidOperationException("initSlotsCount not set");
				}
			}
			int _initSlotsCount = -1;
		public void InitSlots(List<InventoryItemInstance> items){
			List<Slot> newSlots = new List<Slot>();
			int slotCountToCreate = initSlotsCount == 0? items.Count: initSlotsCount;
			for(int i = 0; i <slotCountToCreate; i++){
				Slot newSlot = new Slot();
				newSlots.Add(newSlot);
			}
			SetSlots(newSlots);
		}
		public void PutSBsInSlots(List<ISlottable> sbs){
			foreach(Slot slot in slots){
				slot.sb = sbs[slots.IndexOf(slot)];
			}
		}
	}
	public interface ISlotsHolder{
		List<Slot> slots{get;}
		void SetSlots(List<Slot> slots);
		bool hasEmptySlot{get;}
		Slot GetNewSlot(InventoryItemInstance item);
		List<Slot> newSlots{get;}
		void SetNewSlots(List<Slot> newSlots);
		void SetInitSlotsCount(int count);
		int initSlotsCount{get;}
		void InitSlots(List<InventoryItemInstance> items);
		void PutSBsInSlots(List<ISlottable> sbs);
	}
}
