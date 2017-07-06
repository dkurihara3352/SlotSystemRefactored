using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class DraggedIcon{
		public InventoryItemInstance item{
			get{return m_item;}
			}InventoryItemInstance m_item;
		public IconDestination dest{
			get{return m_dest;}
			}IconDestination m_dest;
			public void SetDestination(SlotGroup sg, Slot slot){
				IconDestination newDest = new IconDestination(sg, slot);
				m_dest = newDest;
			}
		SlotSystemManager m_ssm;
		public Slottable sb{
			get{return m_sb;}
			}Slottable m_sb;
		public DraggedIcon(Slottable sb){
			m_sb = sb;
			m_item = this.sb.itemInst;
			m_ssm = SlotSystemManager.curSSM;
		}
		public void CompleteMovement(){
			m_ssm.AcceptDITAComp(this);
		}
	}
}