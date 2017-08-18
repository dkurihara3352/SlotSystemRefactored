using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{	
	public class FilterHandler : IFilterHandler {
		public List<IInventoryItemInstance> FilteredItems(List<IInventoryItemInstance> items){
			GetFilter().Filter(ref items);
			return items;
		}
		public SGFilter GetFilter(){
			if(_filter != null)
				return _filter;
			else
				throw new InvalidOperationException("filter not set");
		}
		public void SetFilter(SGFilter filter){
			_filter = filter;
		}
			SGFilter _filter;
		public bool AcceptsFilter(ISlottable pickedSB){
			SGFilter filter = GetFilter();
			if(filter is SGNullFilter) return true;
			else{
				if(pickedSB.GetItem() is BowInstance)
					return filter is SGBowFilter;
				else if(pickedSB.GetItem() is WearInstance)
					return filter is SGWearFilter;
				else if(pickedSB.GetItem() is CarriedGearInstance)
					return filter is SGCGearsFilter;
				else
					return filter is SGPartsFilter;
			}
		}
	}
	public interface IFilterHandler{
		List<IInventoryItemInstance> FilteredItems(List<IInventoryItemInstance> items);
		SGFilter GetFilter();
		void SetFilter(SGFilter filter);
		bool AcceptsFilter(ISlottable pickedSB);
	}
}

