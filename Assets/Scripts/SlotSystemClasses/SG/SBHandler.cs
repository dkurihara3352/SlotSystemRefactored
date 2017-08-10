using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SBHandler : ISBHandler {
		public SBHandler(){
			SetSBs(new List<ISlottable>());
			SetNewSBs(new List<ISlottable>());
		}
		public ISlottable GetSB(InventoryItemInstance itemInst){
			foreach(ISlottable sb in slottables){
				if(sb != null)
					if(sb.item == itemInst)
						return sb;
			}
			return null;
		}
		public bool HasItem(InventoryItemInstance itemInst){
			return GetSB(itemInst) != null;
		}
		public List<ISlottable> slottables{
			get{
				if(_slottables != null)
					return _slottables;
				else
					throw new InvalidOperationException("sbs not set");
			}
		}
			List<ISlottable> _slottables;
			public void SetSBs(List<ISlottable> sbs){
				_slottables = sbs;
			}
		public List<ISlottable> newSBs{
			get{
				if(_newSBs != null)
					return _newSBs;
				else
					throw new InvalidOperationException("newSBs not Set");
			}
		}
			List<ISlottable> _newSBs;
			public void SetNewSBs(List<ISlottable> sbs){
				_newSBs = sbs;
			}
		public List<ISlottable> equippedSBs{
			get{
				List<ISlottable> result = new List<ISlottable>();
				foreach(ISlottable sb in slottables){
					if(sb != null && sb.isEquipped)
						result.Add(sb);
				}
				return result;
			}
		}
		public bool isAllSBActProcDone{
			get{
				foreach(ISlottable sb in slottables){
					if(sb != null){
						if(sb.actProcess  != null)
							if(sb.actProcess.isRunning)
								return false;
					}
				}
				return true;
			}
		}
		public void SetSBsActStates(){
			List<ISlottable> moveWithins = new List<ISlottable>();
			List<ISlottable> removed = new List<ISlottable>();
			List<ISlottable> added = new List<ISlottable>();
			foreach(ISlottable sb in slottables){
				if(sb != null){
					if(newSBs.Contains(sb))
						moveWithins.Add(sb);
					else
						removed.Add(sb);
				}
			}
			foreach(ISlottable sb in newSBs){
				if(sb != null){
					if(!slottables.Contains(sb))
						added.Add(sb);
				}
			}
			foreach(ISlottable sb in moveWithins){
				sb.SetNewSlotID(newSBs.IndexOf(sb));
				sb.MoveWithin();
			}
			foreach(ISlottable sb in removed){
				sb.SetNewSlotID(-1);
				sb.Remove();
			}
			foreach(ISlottable sb in added){
				sb.SetNewSlotID(newSBs.IndexOf(sb));
				sb.Add();
			}
		}
	}
	public interface ISBHandler{
		List<ISlottable> slottables{get;}
		void SetSBs(List<ISlottable> sbs);
		List<ISlottable> newSBs{get;}
		void SetNewSBs(List<ISlottable> newSBs);
		void SetSBsActStates();
		ISlottable GetSB(InventoryItemInstance item);
		bool HasItem(InventoryItemInstance item);
		List<ISlottable> equippedSBs{get;}
		bool isAllSBActProcDone{get;}
	}
}
