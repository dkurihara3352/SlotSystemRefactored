using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SorterHandler : ISorterHandler {
		public List<ISlottable> GetSortedSBsWithoutResize(List<ISlottable> source){
			return sorter.OrderedSBsWithoutResize(source);
		}
		public List<ISlottable> GetSortedSBsWithResize(List<ISlottable> source){
			return sorter.OrderedAndTrimmedSBs(source);
		}
		public void SetSorter(SGSorter sorter){
			_sorter = sorter;
		}
			public SGSorter sorter{
				get{
					if(_sorter != null)
						return _sorter;
					else
						throw new InvalidOperationException("sorter not set");
				}
			}
			SGSorter _sorter;
		public void SetIsAutoSort(bool on){
			_isAutoSort = on;
		}
		public bool isAutoSort{
			get{return _isAutoSort;}
		}
			protected bool _isAutoSort = true;
	}
	public interface ISorterHandler{
		List<ISlottable> GetSortedSBsWithoutResize(List<ISlottable> source);
		List<ISlottable> GetSortedSBsWithResize(List<ISlottable> source);
		SGSorter sorter{get;}
		void SetSorter(SGSorter sorter);
		void SetIsAutoSort(bool on);
		bool isAutoSort{get;}
	}
}
