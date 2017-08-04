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
			public void SetDestination(ISlotGroup sg, Slot slot){
				IconDestination newDest = new IconDestination(sg, slot);
				m_dest = newDest;
			}
		ITransactionManager tam;
		public ISlottable sb{
			get{return m_sb;}
			}ISlottable m_sb;
		public DraggedIcon(ISlottable sb, ITransactionManager tam){
			m_sb = sb;
			m_item = this.sb.item;
			this.tam = tam;
		}
		public void CompleteMovement(){
			tam.AcceptDITAComp(this);
		}
	}
}