using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class IconDestination{
		public SlotGroup sg{
			get{return m_sg;}
			}SlotGroup m_sg;
			public void SetSG(SlotGroup sg){
				m_sg = sg;
			}
		public Slot slot{
			get{return m_slot;}
			}Slot m_slot;
			public void SetSlot(Slot slot){
				m_slot = slot;
			}
		public IconDestination(SlotGroup sg, Slot slot){
			SetSG(sg); SetSlot(slot);
		}
	}
}