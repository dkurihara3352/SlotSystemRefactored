using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SlotHandler : ISlotHandler {
		public int GetSlotID(){
			return _slotID;
		}
			int _slotID = -1;
		public void SetSlotID(int i){
			_slotID = i;
		}
		public int GetNewSlotID(){
			return _newSlotID;
		}
			int _newSlotID = -2;
		public void SetNewSlotID(int id){
			_newSlotID = id;
		}
		public bool IsToBeAdded(){
			return GetSlotID() == -1;
		}
		public bool IsToBeRemoved(){
			return GetNewSlotID() == -1;
		}
	}
	public interface ISlotHandler{
		int GetSlotID();
		void SetSlotID(int i);
		int GetNewSlotID();
		void SetNewSlotID(int id);
		bool IsToBeAdded();
		bool IsToBeRemoved();
	}
}
