using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class Slot: SlotSystemElement, ISlot{
		ISlottable m_sb;
		public ISlottable sb{
			get{return m_sb;}
			set{m_sb = value;}
		}
		Vector2 m_position;
		public Vector2 Position{
			get{return m_position;}
			set{m_position = value;}
		}
	}
	public interface ISlot: ISlotSystemElement, IUIElement{
		int GetID();
		ISlotGroup GetSlotGroup();
		ISlottable GetSlottable();
		IHoverable Hoverable();
		IUISystemInputHandler UISystemInputHandler();
		ISlotActStateHandler ActStateHandler();
		IItemHandler ItemHandler();
		bool IsPickedUp();
		void Drag();
		void Drop();
		void Tap();
		void Refresh();
		void Increment();
	}
}