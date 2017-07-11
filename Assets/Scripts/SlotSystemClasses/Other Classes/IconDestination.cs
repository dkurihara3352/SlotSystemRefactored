using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class IconDestination{
		public ISlotGroup sg{
			get{return m_sg;}
			}ISlotGroup m_sg;
			public void SetSG(ISlotGroup sg){
				m_sg = sg;
			}
		public Slot slot{
			get{return m_slot;}
			}Slot m_slot;
			public void SetSlot(Slot slot){
				m_slot = slot;
			}
		public IconDestination(ISlotGroup sg, Slot slot){
			SetSG(sg); SetSlot(slot);
		}
	}
}