using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class Slot: SlotSystemElement, ISlot{
		public Slot(RectTransformFake rectTrans): base(rectTrans){
			
		}
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
		void InitializeSlotID(){}
		void InitializeSelStateHandler(){
			SetSelStateHandler(new SlotSelStateHandler(this));
		}
	}
	public interface ISlot: ISlotSystemElement, IUIElement{
		void MakeSBSelectable();
		void MakeSBUnselectable();
		void SelectSB();
		int GetID();
		void SetID(int id);
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