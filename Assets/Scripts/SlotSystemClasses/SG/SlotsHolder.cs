using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SlotsHolder : ISlotsHolder {
		public SlotsHolder(ISlotGroup sg){
			_sg = sg;
			SetNewSlots(new List<Slot>());
			SetSlots(new List<Slot>());
		}
			ISlotGroup _sg;
		public Slot GetNewSlot(IInventoryItemInstance itemInst){
			int index = -3;
			foreach(ISlottable sb in _sg){
				if(sb != null){
					if(sb.GetItem() == itemInst)
						index = sb.GetNewSlotID();
				}
			}
			if(index != -3)
				return GetNewSlots()[index];
			else 
				return null;
		}
		public List<Slot> GetSlots(){
			if(_slots != null)
				return _slots;
			else throw new InvalidOperationException("slots not set");
		}
		public void SetSlots(List<Slot> slots){
			_slots = slots;
		}
			List<Slot> _slots;
		public List<Slot> GetNewSlots(){
			if(_newSlots != null)
				return _newSlots;
			else
				throw new InvalidOperationException("newSlots not set");
		}
		public void SetNewSlots(List<Slot> newSlots){
			_newSlots = newSlots;
		}
			List<Slot> _newSlots;
		public bool HasEmptySlot(){
			foreach(Slot slot in GetSlots()){
				if(slot.sb == null)
					return true;
			}
			return false;
		}
		public void SetInitSlotsCount(int i){
			_initSlotsCount = i;
		}
		public int GetInitSlotsCount(){
			if(_initSlotsCount != -1)
				return _initSlotsCount;
			else
				throw new InvalidOperationException("initSlotsCount not set");
		}
			int _initSlotsCount = -1;
		public void InitSlots(List<IInventoryItemInstance> items){
			int slotCountToCreate = SlotCountsToCreate(items);
			List<Slot> newSlots = CreateSlots(slotCountToCreate);
			SetSlots(newSlots);
		}
			int SlotCountsToCreate(List<IInventoryItemInstance> items){
				int initCount = GetInitSlotsCount();
				return initCount == 0? items.Count: initCount;
			}
			List<Slot> CreateSlots(int count){
				List<Slot> result = new List<Slot>();
				for(int i = 0; i < count; i ++){
					Slot newSlot = new Slot();
					result.Add(newSlot);
				}
				return result;
			}
		public void PutSBsInSlots(List<ISlottable> sbs){
			List<Slot> slots = GetSlots();
			if(slots.Count < sbs.Count)
				throw new InvalidOperationException("not enough slots to accomodate sbs");
			else
				foreach(Slot slot in slots){
					slot.sb = sbs[slots.IndexOf(slot)];
				}
		}
	}
	public interface ISlotsHolder{
		List<Slot> GetSlots();
		void SetSlots(List<Slot> slots);
		bool HasEmptySlot();
		Slot GetNewSlot(IInventoryItemInstance item);
		List<Slot> GetNewSlots();
		void SetNewSlots(List<Slot> newSlots);
		void SetInitSlotsCount(int count);
		int GetInitSlotsCount();
		void InitSlots(List<IInventoryItemInstance> items);
		void PutSBsInSlots(List<ISlottable> sbs);
	}
}
