using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class SorterHandler : ISorterHandler {
		public List<ISlottable> GetSortedSBsWithoutResize(List<ISlottable> source){
			return GetSorter().OrderedSBsWithoutResize(source);
		}
		public List<ISlottable> GetSortedSBsWithResize(List<ISlottable> source){
			return GetSorter().OrderedAndTrimmedSBs(source);
		}
		public void SetSorter(SGSorter sorter){
			_sorter = sorter;
		}
		public SGSorter GetSorter(){
			if(_sorter != null)
				return _sorter;
			else
				throw new InvalidOperationException("sorter not set");
		}
			SGSorter _sorter;
		public void SetIsAutoSort(bool on){
			_isAutoSort = on;
		}
		public bool IsAutoSort(){
			return _isAutoSort;
		}
			protected bool _isAutoSort = true;
	}
	public interface ISorterHandler{
		List<ISlottable> GetSortedSBsWithoutResize(List<ISlottable> source);
		List<ISlottable> GetSortedSBsWithResize(List<ISlottable> source);
		SGSorter GetSorter();
		void SetSorter(SGSorter sorter);
		void SetIsAutoSort(bool on);
		bool IsAutoSort();
	}
}
