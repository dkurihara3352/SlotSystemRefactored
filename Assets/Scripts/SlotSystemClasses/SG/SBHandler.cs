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
		public ISlottable GetSB(IInventoryItemInstance itemInst){
			foreach(ISlottable sb in GetSBs()){
				if(sb != null)
					if(sb.GetItem() == itemInst)
						return sb;
			}
			return null;
		}
		public bool HasItem(IInventoryItemInstance itemInst){
			return GetSB(itemInst) != null;
		}
		public List<ISlottable> GetSBs(){
			if(_slottables != null)
				return _slottables;
			else
				throw new InvalidOperationException("sbs not set");
		}
		public void SetSBs(List<ISlottable> sbs){
			_slottables = sbs;
		}
			List<ISlottable> _slottables;
		public List<ISlottable> GetNewSBs(){
			if(_newSBs != null)
				return _newSBs;
			else
				throw new InvalidOperationException("newSBs not Set");
		}
		public void SetNewSBs(List<ISlottable> sbs){
			_newSBs = sbs;
		}
			List<ISlottable> _newSBs;
		public List<ISlottable> GetEquippedSBs(){
			List<ISlottable> result = new List<ISlottable>();
			foreach(ISlottable sb in GetSBs()){
				if(sb != null && sb.IsEquipped())
					result.Add(sb);
			}
			return result;
		}
		public bool IsAllSBActProcDone(){
			foreach(ISlottable sb in GetSBs()){
				if(sb != null){
					ISBActProcess actProcess = sb.GetActProcess();
					if(actProcess  != null)
						if(actProcess.IsRunning())
							return false;
				}
			}
			return true;
		}
		public void SetSBsActStates(){
			List<ISlottable> moveWithins = new List<ISlottable>();
			List<ISlottable> removed = new List<ISlottable>();
			List<ISlottable> added = new List<ISlottable>();
			List<ISlottable> slottables = GetSBs();
			List<ISlottable> newSBs = GetNewSBs();
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
		List<ISlottable> GetSBs();
		void SetSBs(List<ISlottable> sbs);
		List<ISlottable> GetNewSBs();
		void SetNewSBs(List<ISlottable> newSBs);
		void SetSBsActStates();
		ISlottable GetSB(IInventoryItemInstance item);
		bool HasItem(IInventoryItemInstance item);
		List<ISlottable> GetEquippedSBs();
		bool IsAllSBActProcDone();
	}
}
