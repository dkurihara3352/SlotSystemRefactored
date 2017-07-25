using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SGClone: SlotGroup{
		public override bool isWaitingForAction{get{return m_isWaitingForAction;}} bool m_isWaitingForAction;
		public override bool isReverting{get{return m_isReverting;}} bool m_isReverting;
		public override bool isReordering{get{return m_isReordering;}} bool m_isReordering;
		public override bool isAdding{get{return m_isAdding;}} bool m_isAdding;
		public override bool isRemoving{get{return m_isRemoving;}} bool m_isRemoving;
		public override bool isSwapping{get{return m_isSwapping;}} bool m_isSwapping;
		public override bool isFilling{get{return m_isFilling;}} bool m_isFilling;
		public override bool isSorting{get{return m_isSorting;}} bool m_isSorting;
		public override bool wasWaitingForAction{get{return m_wasWaitingForAction;}} bool m_wasWaitingForAction;
		public override bool wasReverting{get{return m_wasReverting;}} bool m_wasReverting;
		public override bool wasReordering{get{return m_wasReordering;}} bool m_wasReordering;
		public override bool wasAdding{get{return m_wasAdding;}} bool m_wasAdding;
		public override bool wasRemoving{get{return m_wasRemoving;}} bool m_wasRemoving;
		public override bool wasSwapping{get{return m_wasSwapping;}} bool m_wasSwapping;
		public override bool wasFilling{get{return m_wasFilling;}} bool m_wasFilling;
		public override bool wasSorting{get{return m_wasSorting;}} bool m_wasSorting;

		public override bool isDeactivated{get{return m_isDeactivated;}} bool m_isDeactivated;
		public override bool isFocused{get{return m_isFocused;}} bool m_isFocused;
		public override bool isDefocused{get{return m_isDefocused;}} bool m_isDefocused;
		public override bool isSelected{get{return m_isSelected;}} bool m_isSelected;
		public override bool wasDeactivated{get{return m_wasDeactivated;}} bool m_wasDeactivated;
		public override bool wasFocused{get{return m_wasFocused;}} bool m_wasFocused;
		public override bool wasDefocused{get{return m_wasDefocused;}} bool m_wasDefocused;
		public override bool wasSelected{get{return m_wasSelected;}} bool m_wasSelected;
		public override bool isPool{
			get{return m_isPool;}
			}bool m_isPool;
		public override bool isSGE{
			get{return m_isSGE;}
			}bool m_isSGE;
		public override bool isSGG{
			get{return m_isSGG;}
			}bool m_isSGG;
		
		public override List<ISlottable> equippedSBs{
			get{return m_equippedSBs;}
			}List<ISlottable> m_equippedSBs;
		public override bool isAllSBActProcDone{
			get{return m_isAllTASBsDone;}
			}bool m_isAllTASBsDone;
		public override string eName{
			get{return m_eName;}
			}
		public override bool isShrinkable{
			get{return m_isShrinkable;}
			}bool m_isShrinkable;
		public override bool isExpandable{
			get{return m_isExpandable;}
			}bool m_isExpandable;
		public void Initialize(ISlotGroup orig){
			this.scroller = orig.scroller;
			SetInventory(orig.inventory);
			m_isShrinkable = orig.isShrinkable;
			m_isExpandable = orig.isExpandable;
			List<Slot> slotsClone = new List<Slot>();
				foreach(Slot oSlot in orig.slots){
					Slot newSlot = new Slot();
					newSlot.sb = oSlot.sb;
					slotsClone.Add(newSlot);
				}
				SetSlots(slotsClone);
			List<Slot> newSlotsClone = new List<Slot>();
				if(orig.newSlots != null)
				foreach(Slot oSlot in orig.newSlots){
					Slot newSlot = new Slot();
					newSlot.sb = oSlot.sb;
					newSlotsClone.Add(newSlot);
				}
				SetNewSlots(slotsClone);
			m_isPool = orig.isPool;
			m_isSGE = orig.isSGE;
			m_isSGG = orig.isSGG;
			m_isAutoSort = orig.isAutoSort;
			List<ISlottable> sbsClone = new List<ISlottable>();
				foreach(ISlottable sb in orig){
					if(sb == null)
						sbsClone.Add(null);
					else{
						ISlottable cloneSB = SlotSystemUtil.CloneSB(sb);
						sbsClone.Add(cloneSB);
					}
				}
				SetSBs(sbsClone);
			List<ISlottable> newSbsClone = new List<ISlottable>();
				if(orig.newSBs != null)
				foreach(ISlottable sb in orig.newSBs){
					if(sb == null)
						newSbsClone.Add(null);
					else{
						ISlottable cloneSB = SlotSystemUtil.CloneSB(sb);
						newSbsClone.Add(cloneSB);
					}
				}
				SetNewSBs(newSbsClone);
			List<ISlottable> equippedSBsClone = new List<ISlottable>();
				if(orig.equippedSBs != null)
				foreach(ISlottable sb in orig.equippedSBs){
					ISlottable cloneSB = SlotSystemUtil.CloneSB(sb);
					equippedSBsClone.Add(cloneSB);
				}
				m_equippedSBs = equippedSBsClone;
			m_isAllTASBsDone = orig.isAllSBActProcDone;
			SetInitSlotsCount(orig.initSlotsCount);
			this.m_isFocused = orig.isFocused;
			this.m_isDefocused = orig.isDefocused;
			this.m_isDeactivated = orig.isDeactivated;
			SetSorter(orig.sorter);
			SetFilter(orig.filter);
			this.m_eName = orig.eName;
			m_isDeactivated = orig.isDeactivated;
			m_isFocused = orig.isFocused;
			m_isDefocused = orig.isDefocused;
			m_isSelected = orig.isSelected;
			
			m_wasDeactivated = orig.wasDeactivated;
			m_wasFocused = orig.wasFocused;
			m_wasDefocused = orig.wasDefocused;
			m_wasSelected = orig.wasSelected;
			
			m_isWaitingForAction = orig.isWaitingForAction;
			m_isReverting = orig.isReverting;
			m_isReordering = orig.isReordering;
			m_isAdding = orig.isAdding;
			m_isRemoving = orig.isRemoving;
			m_isSwapping = orig.isSwapping;
			m_isFilling = orig.isFilling;
			m_isSorting = orig.isSorting;
			m_wasWaitingForAction = orig.wasWaitingForAction;
			m_wasReverting = orig.wasReverting;
			m_wasReordering = orig.wasReordering;
			m_wasAdding = orig.wasAdding;
			m_wasRemoving = orig.wasRemoving;
			m_wasSwapping = orig.wasSwapping;
			m_wasFilling = orig.wasFilling;
			m_wasSorting = orig.wasSorting;
			
		}
	}
}
