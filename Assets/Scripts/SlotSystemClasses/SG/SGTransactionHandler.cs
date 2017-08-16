﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public class SGTransactionHandler : ISGTransactionHandler {
		public ISlotGroup sg;
		public IHoverable hoverable{
			get{
				if(_hoverable != null)
					return _hoverable;
				else
					throw new InvalidOperationException("hoverable not set");
			}
		}
			IHoverable _hoverable;
			public void SetHoverable(IHoverable hoverable){
				_hoverable = hoverable;
			}
			public ITransactionCache taCache{
				get{return hoverable.taCache;}
			}
		public ISBFactory sbFactory{
			get{
				if(_sbFactory != null)
					return _sbFactory;
				else
					throw new InvalidOperationException("sbFactory not set");
			}
		}
			ISBFactory _sbFactory;
			public void SetSBFactory(ISBFactory sbFactory){
				_sbFactory = sbFactory;
			}
		public ISBHandler sbHandler{
			get{
				if(_sbHandler != null)
					return _sbHandler;
				else
					throw new InvalidOperationException("sbHandler not set");
			}
		}
			ISBHandler _sbHandler;
			public void SetSBHandler(ISBHandler sbHandler){
				_sbHandler = sbHandler;
			}
		public ISorterHandler sorterHandler{
			get{
				if(_sorterHandler != null)
					return _sorterHandler;
				else
					throw new InvalidOperationException("sorterHandler not set");
			}
		}
			ISorterHandler _sorterHandler;
			public void SetSorterHandler(ISorterHandler sorterHandler){
				_sorterHandler = sorterHandler;
			}
		public ISlotsHolder slotsHolder{
			get{
				if(_slotsHolder != null)
					return _slotsHolder;
				else
					throw new InvalidOperationException("slotsHolder not set");
			}
		}
			ISlotsHolder _slotsHolder;
			public void SetSlotsHolder(ISlotsHolder slotsHolder){
				_slotsHolder = slotsHolder;
			}
		public ITransactionSGHandler sgHandler{
			get{
				if(_sgHandler != null)
					return _sgHandler;
				else
					throw new InvalidOperationException("sgHandler not set");
			}
		}
			ITransactionSGHandler _sgHandler;
			public void SetSGHandler(ITransactionSGHandler sgHandler){
				_sgHandler = sgHandler;
			}
		public SGTransactionHandler(ISlotGroup sg, ITransactionManager tam){
			this.sg = sg;
			SetHoverable(sg);
			SetSBHandler(sg);
			SetSorterHandler(sg);
			SetSlotsHolder(sg);
			SetSBFactory(sg);
			SetSGHandler(tam.sgHandler);
		}
		public List<ISlottable> ReorderedNewSBs(){
			List<ISlottable> result = new List<ISlottable>(sbHandler.slottables);
			ISlottable sb1 = taCache.pickedSB;
			ISlottable sb2 = taCache.targetSB;
			result.Reorder(sb1, sb2);
			return result;
		}
		public List<ISlottable> SortedNewSBs(){
			List<ISlottable> result;
			if(sg.isExpandable)
				result = sorterHandler.GetSortedSBsWithResize(sbHandler.slottables);
			else
				result = sorterHandler.GetSortedSBsWithoutResize(sbHandler.slottables);
			return result;
		}
		public List<ISlottable> FilledNewSBs(){
			ISlottable added = GetAddedForFill();
			ISlottable removed = GetRemovedForFill();

			List<ISlottable> result = FilledOrNulledSBs(sbHandler.slottables, added, removed);

			result = SortedSBsIfAutoSortAccordingToExpandability(result);
			return result;
		}
			List<ISlottable> FilledOrNulledSBs(List<ISlottable> source, ISlottable added, ISlottable removed){
				List<ISlottable> result = new List<ISlottable>(source);
				if(!sg.isPool){
					if(added != null)
						CreateNewSBAndFill(added.item, result);
					if(removed != null)
						NullifyIndexAt(removed.item, result);
				}
				return result;
			}
			public ISlottable GetAddedForFill(){
				ISlottable added;
				if(sgHandler.sg1 == sg)
					added = null;
				else
					added = taCache.pickedSB;
				return added;
			}
			public ISlottable GetRemovedForFill(){
				ISlottable removed;
				if(sgHandler.sg1 == sg)
					removed = taCache.pickedSB;
				else
					removed = null;
				return removed;
			}
		public void CreateNewSBAndFill(InventoryItemInstance addedItem, List<ISlottable> list){
			ISlottable newSB = sbFactory.CreateSB(addedItem);
			newSB.Unequip();
			list.Fill(newSB);
		}
		public void NullifyIndexAt(InventoryItemInstance removedItem, List<ISlottable> list){
			if(removedItem != null){
				ISlottable rem = null;
				foreach(ISlottable sb in list){
					if(sb != null){
						if(sb.item == removedItem)
							rem = sb;
					}
				}
				if(rem != null){
					list[list.IndexOf(rem)] = null;
					rem.Destroy();
				}
			}
		}
		public List<ISlottable> SwappedNewSBs(){
			ISlottable added = GetAddedForSwap();
			ISlottable removed = GetRemovedForSwap();

			List<ISlottable> result = SwappedList(added, removed, sbHandler.slottables);

			result = SortedSBsIfAutoSortAccordingToExpandability(result);
			return result;			
		}
			public ISlottable GetAddedForSwap(){
				ISlottable added = null;
				if(sgHandler.sg1 == sg)
					added = taCache.targetSB;
				else
					added = taCache.pickedSB;
				return added;
			}
			public ISlottable GetRemovedForSwap(){
				ISlottable removed = null;
				if(sgHandler.sg1 == sg)
					removed = taCache.pickedSB;
				else
					removed = taCache.targetSB;
				return removed;
			}
			List<ISlottable> SwappedList(ISlottable added, ISlottable removed, List<ISlottable> source){
				List<ISlottable> result = new List<ISlottable>(source);
				if(!sg.isPool){
					ISlottable newSB = sbFactory.CreateSB(added.item);
					newSB.Unequip();
					result[result.IndexOf(removed)] = newSB;
				}
				return result;
			}
		public List<ISlottable> AddedNewSBs(){
			List<InventoryItemInstance> added = taCache.moved;

			List<ISlottable> result = IncreasedSBs(added, sbHandler.slottables);

			result = SortedSBsIfAutoSortAccordingToExpandability(result);
			return result;
		}
			public List<ISlottable> IncreasedSBs(List<InventoryItemInstance> added, List<ISlottable> source){
				List<ISlottable> result = new List<ISlottable>(source);
				foreach(InventoryItemInstance item in added){
					if(item.isStackable)
						IncreaseQuantityOfSBInList(result, item);
					else
						CreateNewSBAndFill(item, result);
				}
				return result;
			}
			public void IncreaseQuantityOfSBInList(List<ISlottable> sbs, InventoryItemInstance item){
				if(!item.isStackable)
					throw new ArgumentException("item is not stackable");
				else{
					bool found = false;
					foreach(var sb in sbs){
						if(sb != null){
							if(sb.item == item){
								int newQuantity = sb.quantity + item.quantity;
								sb.SetQuantity(newQuantity);
								found = true;
								return;
							}
						}
					}
					if(!found)
						throw new InvalidOperationException("sbs does not contain matching sb");
				}
			}
		public List<ISlottable> RemovedNewSBs(){
			List<InventoryItemInstance> removed = taCache.moved;

			List<ISlottable> result = DecreasedSBs(removed, sbHandler.slottables);
			
			result = SortedSBsIfAutoSortAccordingToExpandability(result);
			return result;
		}
			List<ISlottable> DecreasedSBs(List<InventoryItemInstance> removed, List<ISlottable> source){
				List<ISlottable> result = new List<ISlottable>(source);
				foreach(InventoryItemInstance item in removed)
					if(item.isStackable)
						DecreaseQuantityOfSBInList(result, item);
					else
						NullifyIndexAt(item, result);
				return result;
			}
			public void DecreaseQuantityOfSBInList(List<ISlottable> sbs, InventoryItemInstance item){
				if(!item.isStackable)
					throw new ArgumentException("item is not stackable");
				else{
					bool found = false;
					ISlottable removed = null;
					foreach(var sb in sbs)
						if(sb != null)
							if(sb.item == item){
								found = true;
								int newQuantity = sb.quantity - item.quantity;
								if(newQuantity <= 0){
									removed = sb;
									break;
								}else
									sb.SetQuantity(newQuantity);
									return;
							}
					if(removed != null){
						sbs[sbs.IndexOf(removed)] = null;
						removed.Destroy();
					}
					if(!found)
						throw new InvalidOperationException("sbs does not contain matching sb");
				}
			}
		public void SetSBsFromSlotsAndUpdateSlotIDs(){
			List<ISlottable> result = ExtractSBsFromSlots();
			sbHandler.SetSBs(result);
			UpdateSlotIDs(sbHandler.slottables);
		}
			List<ISlottable> ExtractSBsFromSlots(){
				List<ISlottable> result = new List<ISlottable>();
				foreach(Slot slot in slotsHolder.slots){
					result.Add(slot.sb);
				}
				return result;
			}
			void UpdateSlotIDs(List<ISlottable> sbs){
				foreach(ISlottable sb in sbs){
					if(sb != null)
					sb.SetSlotID(sbs.IndexOf(sb));
				}
			}
		public void UpdateSBs(){
			MakeSureNewSlotsAreReady(sbHandler.slottables);
			RemoveAndDestorySBsFrom(sbHandler.slottables);
			PutSBsInNewSlot(sbHandler.slottables);
			SetNewSlotsToSlots();
			SetSBsFromSlotsAndUpdateSlotIDs();
		}
			void MakeSureNewSlotsAreReady(List<ISlottable> sbs){
				int count = sbs.Count;
				if(slotsHolder.newSlots == null || slotsHolder.newSlots.Count < count)
					slotsHolder.SetNewSlots(CreateSlots(count));
			}
			List<Slot> CreateSlots(int count){
				List<Slot> result = new List<Slot>();
				for(int i =0; i< count; i++)
					result.Add(new Slot());
				return result;
			}
			public void RemoveAndDestorySBsFrom(List<ISlottable> source){
				foreach(var sb in source){
					if(sb != null && sb.isToBeRemoved){
						int index = source.IndexOf(sb);
						ISlottable removed = sb;
						source[index] = null;
						sb.Destroy();
					}
				}
			}
			void PutSBsInNewSlot(IEnumerable<ISlottable> source){
				foreach(var sb in source)
					if(sb != null && !sb.isToBeRemoved)
						slotsHolder.newSlots[sb.newSlotID].sb = sb;
			}
			void SetNewSlotsToSlots(){
				slotsHolder.SetSlots(slotsHolder.newSlots);
			}
		public List<ISlottable> SortedSBsIfAutoSortAccordingToExpandability(List<ISlottable> source){
			if(!sorterHandler.isAutoSort)
				return source;
			else{
				if(sg.isExpandable)
					return sorterHandler.GetSortedSBsWithResize(source);
				else
					return sorterHandler.GetSortedSBsWithoutResize(source);
			}
		}
		public void ReportTAComp(){
			sgHandler.AcceptSGTAComp(sg);
		}
	}
	public interface ISGTransactionHandler{
		List<ISlottable> ReorderedNewSBs();
		List<ISlottable> SortedNewSBs();
		List<ISlottable> FilledNewSBs();
		List<ISlottable> SwappedNewSBs();
		List<ISlottable> AddedNewSBs();
		List<ISlottable> RemovedNewSBs();
		void UpdateSBs();
		void SetSBsFromSlotsAndUpdateSlotIDs();
		void ReportTAComp();
	}
}
