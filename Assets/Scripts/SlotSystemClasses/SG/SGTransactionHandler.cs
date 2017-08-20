using System;
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
			get{return sg.GetTAC();}
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
			SetHoverable(sg.GetHoverable());
			SetSBHandler(sg.GetSBHandler());
			SetSorterHandler(sg.GetSorterHandler());
			SetSlotsHolder(sg.GetSlotsHolder());
			SetSBFactory(sg.GetSBFactory());
			SetSGHandler(tam.GetSGHandler());
		}
		public List<ISlottable> ReorderedNewSBs(){
			List<ISlottable> result = new List<ISlottable>(sbHandler.GetSBs());
			ISlottable sb1 = taCache.GetPickedSB();
			ISlottable sb2 = taCache.GetTargetSB();
			result.Reorder(sb1, sb2);
			return result;
		}
		public List<ISlottable> SortedNewSBs(){
			List<ISlottable> result;
			if(sg.IsExpandable())
				result = sorterHandler.GetSortedSBsWithResize(sbHandler.GetSBs());
			else
				result = sorterHandler.GetSortedSBsWithoutResize(sbHandler.GetSBs());
			return result;
		}
		public List<ISlottable> FilledNewSBs(){
			ISlottable added = GetAddedForFill();
			ISlottable removed = GetRemovedForFill();

			List<ISlottable> result = FilledOrNulledSBs(sbHandler.GetSBs(), added, removed);

			result = SortedSBsIfAutoSortAccordingToExpandability(result);
			return result;
		}
			List<ISlottable> FilledOrNulledSBs(List<ISlottable> source, ISlottable added, ISlottable removed){
				List<ISlottable> result = new List<ISlottable>(source);
				if(!sg.IsPool()){
					if(added != null)
						CreateNewSBAndFill(added.GetItem(), result);
					if(removed != null)
						NullifyIndexAt(removed.GetItem(), result);
				}
				return result;
			}
			public ISlottable GetAddedForFill(){
				ISlottable added;
				if(sgHandler.GetSG1() == sg)
					added = null;
				else
					added = taCache.GetPickedSB();
				return added;
			}
			public ISlottable GetRemovedForFill(){
				ISlottable removed;
				if(sgHandler.GetSG1() == sg)
					removed = taCache.GetPickedSB();
				else
					removed = null;
				return removed;
			}
		public void CreateNewSBAndFill(IInventoryItemInstance addedItem, List<ISlottable> list){
			ISlottable newSB = sbFactory.CreateSB(addedItem);
			newSB.Unequip();
			list.Fill(newSB);
		}
		public void NullifyIndexAt(IInventoryItemInstance removedItem, List<ISlottable> list){
			if(removedItem != null){
				ISlottable rem = null;
				foreach(ISlottable sb in list){
					if(sb != null){
						if(sb.GetItem() == removedItem)
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

			List<ISlottable> result = SwappedList(added, removed, sbHandler.GetSBs());

			result = SortedSBsIfAutoSortAccordingToExpandability(result);
			return result;			
		}
			public ISlottable GetAddedForSwap(){
				ISlottable added = null;
				if(sgHandler.GetSG1() == sg)
					added = taCache.GetTargetSB();
				else
					added = taCache.GetPickedSB();
				return added;
			}
			public ISlottable GetRemovedForSwap(){
				ISlottable removed = null;
				if(sgHandler.GetSG1() == sg)
					removed = taCache.GetPickedSB();
				else
					removed = taCache.GetTargetSB();
				return removed;
			}
			List<ISlottable> SwappedList(ISlottable added, ISlottable removed, List<ISlottable> source){
				List<ISlottable> result = new List<ISlottable>(source);
				if(!sg.IsPool()){
					ISlottable newSB = sbFactory.CreateSB(added.GetItem());
					newSB.Unequip();
					result[result.IndexOf(removed)] = newSB;
				}
				return result;
			}
		public List<ISlottable> AddedNewSBs(){
			List<IInventoryItemInstance> added = taCache.GetMoved();

			List<ISlottable> result = IncreasedSBs(added, sbHandler.GetSBs());

			result = SortedSBsIfAutoSortAccordingToExpandability(result);
			return result;
		}
			public List<ISlottable> IncreasedSBs(List<IInventoryItemInstance> added, List<ISlottable> source){
				List<ISlottable> result = new List<ISlottable>(source);
				foreach(IInventoryItemInstance item in added){
					if(item.IsStackable())
						IncreaseQuantityOfSBInList(result, item);
					else
						CreateNewSBAndFill(item, result);
				}
				return result;
			}
			public void IncreaseQuantityOfSBInList(List<ISlottable> sbs, IInventoryItemInstance item){
				if(!item.IsStackable())
					throw new ArgumentException("item is not stackable");
				else{
					List<IItemHandler> itemHandlers = ExtractItemHandlers(sbs);
					bool found = false;
					foreach(var itemHandler in itemHandlers){
						if(itemHandler != null){
							if(itemHandler.GetItem().Equals(item)){
								int newQuantity = itemHandler.GetQuantity() + item.GetQuantity();
								itemHandler.SetQuantity(newQuantity);
								found = true;
								return;
							}
						}
					}
					if(!found)
						throw new InvalidOperationException("sbs does not contain matching sb");
				}
			}
			List<IItemHandler> ExtractItemHandlers(IEnumerable<ISlottable> sbs){
				List<IItemHandler> result = new List<IItemHandler>();
				foreach(ISlottable sb in sbs)
					if(sb != null)
						result.Add(sb.GetItemHandler());
					else
						result.Add(null);
				return result;
			}
		public List<ISlottable> RemovedNewSBs(){
			List<IInventoryItemInstance> removed = taCache.GetMoved();

			List<ISlottable> result = DecreasedSBs(removed, sbHandler.GetSBs());
			
			result = SortedSBsIfAutoSortAccordingToExpandability(result);
			return result;
		}
			List<ISlottable> DecreasedSBs(List<IInventoryItemInstance> removed, List<ISlottable> source){
				List<ISlottable> result = new List<ISlottable>(source);
				foreach(IInventoryItemInstance item in removed)
					if(item.IsStackable())
						DecreaseQuantityOfSBInList(result, item);
					else
						NullifyIndexAt(item, result);
				return result;
			}
			public void DecreaseQuantityOfSBInList(List<ISlottable> sbs, IInventoryItemInstance item){
				if(!item.IsStackable())
					throw new ArgumentException("item is not stackable");
				else{
					bool found = false;
					ISlottable removed = null;
					foreach(var sb in sbs)
						if(sb != null){
							IItemHandler itemHandler = sb.GetItemHandler();
							if(sb.GetItem().Equals(item)){
								found = true;
								int newQuantity = itemHandler.GetQuantity() - item.GetQuantity();
								if(newQuantity <= 0){
									removed = sb;
									break;
								}else
									itemHandler.SetQuantity(newQuantity);
									return;
							}
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
			UpdateSlotIDs(sbHandler.GetSBs());
		}
			List<ISlottable> ExtractSBsFromSlots(){
				List<ISlottable> result = new List<ISlottable>();
				foreach(Slot slot in slotsHolder.GetSlots()){
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
			MakeSureNewSlotsAreReady(sbHandler.GetSBs());
			RemoveAndDestorySBsFrom(sbHandler.GetSBs());
			PutSBsInNewSlot(sbHandler.GetSBs());
			SetNewSlotsToSlots();
			SetSBsFromSlotsAndUpdateSlotIDs();
		}
			void MakeSureNewSlotsAreReady(List<ISlottable> sbs){
				int count = sbs.Count;
				List<Slot> newSlots = slotsHolder.GetNewSlots();
				if(newSlots == null || newSlots.Count < count)
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
					if(sb != null && sb.IsToBeRemoved()){
						int index = source.IndexOf(sb);
						source[index] = null;
						sb.Destroy();
					}
				}
			}
			void PutSBsInNewSlot(IEnumerable<ISlottable> source){
				foreach(var sb in source)
					if(sb != null && !sb.IsToBeRemoved())
						slotsHolder.GetNewSlots()[sb.GetNewSlotID()].sb = sb;
			}
			void SetNewSlotsToSlots(){
				slotsHolder.SetSlots(slotsHolder.GetNewSlots());
			}
		public List<ISlottable> SortedSBsIfAutoSortAccordingToExpandability(List<ISlottable> source){
			if(!sorterHandler.IsAutoSort())
				return source;
			else{
				if(sg.IsExpandable())
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
