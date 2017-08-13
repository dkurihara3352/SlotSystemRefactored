using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public class SGTransactionHandler : ISGTransactionHandler {
		public ISlotGroup sg;
		IHoverable hoverable;
			public ITransactionCache taCache{
				get{return hoverable.taCache;}
			}
		public ISBHandler sbHandler;
		public ISorterHandler sorterHandler;
		ISlotsHolder slotsHolder;
		public SGTransactionHandler(ISlotGroup sg, ITransactionManager tam){
			this.sg = sg;
			hoverable = sg;
			sbHandler = sg;
			sorterHandler = sg;
			slotsHolder = sg;
			SetSGHandler(tam.sgHandler);
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

			List<ISlottable> newSBs = new List<ISlottable>(sbHandler.slottables);

			if(!sg.isPool){
				if(added != null)
					CreateNewSBAndFill(added.item, newSBs);
				if(removed != null)
					NullifyIndexOf(removed.item, newSBs);
			}
			newSBs = SortedSBsIfAutoSortAccordingToExpandability(newSBs);
			return newSBs;
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
		public List<ISlottable> SwappedNewSBs(){
			ISlottable added = GetAddedForSwap();
			ISlottable removed = GetRemovedForSwap();
			List<ISlottable> newSBs = new List<ISlottable>(sbHandler.slottables);
			
			CreateNewSBAndSwapInList(added, removed, newSBs);

			newSBs = SortedSBsIfAutoSortAccordingToExpandability(newSBs);
			return newSBs;			
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
		void CreateNewSBAndSwapInList(ISlottable added, ISlottable removed, List<ISlottable> list){
			if(!sg.isPool){
				ISlottable newSB = sg.CreateSB(added.item);
				newSB.Unequip();
				list[list.IndexOf(removed)] = newSB;
			}
		}
		public List<ISlottable> AddedNewSBs(){
			List<InventoryItemInstance> added = taCache.moved;
			List<ISlottable> newSBs = new List<ISlottable>(sbHandler.slottables);

			foreach(InventoryItemInstance itemInst in added){
				if(!TryChangeStackableQuantity(newSBs, itemInst, true)){
					CreateNewSBAndFill(itemInst, newSBs);
				}
			}
			newSBs = SortedSBsIfAutoSortAccordingToExpandability(newSBs);
			return newSBs;
		}
		public bool TryChangeStackableQuantity(List<ISlottable> target, InventoryItemInstance item, bool added){
			bool changed = false;
			if(item.IsStackable){
				List<ISlottable> removed = new List<ISlottable>();
				foreach(ISlottable sb in target){
					if(sb != null){
						if(sb.item == item){
							int newQuantity;
							if(added)
								newQuantity = sb.quantity + item.quantity;
							else
								newQuantity = sb.quantity - item.quantity;
							if(newQuantity <= 0)
								removed.Add(sb);
							else
								sb.SetQuantity(newQuantity);
							changed = true;
						}
					}
				}
				foreach(ISlottable sb in removed){
					target[target.IndexOf(sb)] = null;
					sb.Destroy();
				}
			}
			return changed;
		}
		public List<ISlottable> RemovedNewSBs(){
			List<InventoryItemInstance> removed = taCache.moved;
			List<ISlottable> thisNewSBs = sbHandler.slottables;
			
			foreach(InventoryItemInstance item in removed){
				if(!TryChangeStackableQuantity(thisNewSBs, item, false)){
					NullifyIndexOf(item, thisNewSBs);
				}
			}
			thisNewSBs = SortedSBsIfAutoSortAccordingToExpandability(thisNewSBs);
			return thisNewSBs;
		}
		public void OnCompleteSlotMovements(){
			foreach(ISlottable sb in sbHandler.slottables){
				if(sb != null){
					if(sb.isToBeRemoved){
						sb.Destroy();
					}else{
						PutSBInNewSlot(sb);
					}
				}
			}
			slotsHolder.SetSlots(slotsHolder.newSlots);
			SyncSBsToSlots();
		}
			void PutSBInNewSlot(ISlottable sb){
				slotsHolder.newSlots[sb.newSlotID].sb = sb;
			}
		public void SyncSBsToSlots(){
			List<ISlottable> newSBs = new List<ISlottable>();
			foreach(Slot slot in slotsHolder.slots){
				newSBs.Add(slot.sb);
			}
			sbHandler.SetSBs(newSBs);
			foreach(ISlottable sb in sbHandler.slottables){
				if(sb != null)
				sb.SetSlotID(newSBs.IndexOf(sb));
			}
		}
		public void CreateNewSBAndFill(InventoryItemInstance addedItem, List<ISlottable> list){
			ISlottable newSB = sg.CreateSB(addedItem);
			newSB.Unequip();
			list.Fill(newSB);
		}
		public void NullifyIndexOf(InventoryItemInstance removedItem, List<ISlottable> list){
			if(removedItem != null){
				ISlottable rem = null;
				foreach(ISlottable sb in list){
					if(sb != null){
						if(sb.item == removedItem)
							rem = sb;
					}
				}
				list[list.IndexOf(rem)] = null;
			}
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
		void OnCompleteSlotMovements();
		void SyncSBsToSlots();
		void ReportTAComp();
	}
}
