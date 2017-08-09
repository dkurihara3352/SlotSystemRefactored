using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SlotHandler : ISlotHandler {
		public int slotID{
			get{return m_slotID;}
		}
			int m_slotID = -1;
		public void SetSlotID(int i){
			m_slotID = i;
		}
		public int newSlotID{
			get{return m_newSlotID;}
		}
			int m_newSlotID = -2;
		public void SetNewSlotID(int id){
			m_newSlotID = id;
		}
		public bool isToBeAdded{
			get{return slotID == -1;}
		}
		public bool isToBeRemoved{
			get{return newSlotID == -1;}
		}
	}
	public interface ISlotHandler{
		int slotID{get;}
		void SetSlotID(int i);
		int newSlotID{get;}
		void SetNewSlotID(int id);
		bool isToBeAdded{get;}
		bool isToBeRemoved{get;}
	}
}
