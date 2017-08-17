using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class DraggedIcon{
		public InventoryItemInstance item{
			get{return _item;}
			}InventoryItemInstance _item;
		public IconDestination dest{
			get{return m_dest;}
			}IconDestination m_dest;
			public void SetDestination(ISlotGroup sg, Slot slot){
				IconDestination newDest = new IconDestination(sg, slot);
				m_dest = newDest;
			}
		ITransactionIconHandler iconHandler;
		public ISlottable sb{
			get{return _sb;}
		}
			ISlottable _sb;
		public DraggedIcon(ISlottable sb, ITransactionIconHandler iconHandler){
			_sb = sb;
			_item = this.sb.GetItem();
			this.iconHandler = iconHandler;
		}
		public void CompleteMovement(){
			iconHandler.AcceptDITAComp(sb);
		}
	}
}