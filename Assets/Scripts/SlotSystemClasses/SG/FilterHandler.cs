using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{	
	public class FilterHandler : IFilterHandler {
		public List<InventoryItemInstance> FilteredItems(List<InventoryItemInstance> items){
			filter.Filter(ref items);
			return items;
		}
		public SGFilter filter{
			get{
				if(_filter != null)
					return _filter;
				else
					throw new InvalidOperationException("filter not set");
			}
		}
			SGFilter _filter;
			public void SetFilter(SGFilter filter){
				_filter = filter;
			}
		public bool AcceptsFilter(ISlottable pickedSB){
			if(this.filter is SGNullFilter) return true;
			else{
				if(pickedSB.GetItem() is BowInstance)
					return this.filter is SGBowFilter;
				else if(pickedSB.GetItem() is WearInstance)
					return this.filter is SGWearFilter;
				else if(pickedSB.GetItem() is CarriedGearInstance)
					return this.filter is SGCGearsFilter;
				else
					return this.filter is SGPartsFilter;
			}
		}
	}
	public interface IFilterHandler{
		List<InventoryItemInstance> FilteredItems(List<InventoryItemInstance> items);
		SGFilter filter{get;}
		void SetFilter(SGFilter filter);
		bool AcceptsFilter(ISlottable pickedSB);
	}
}

