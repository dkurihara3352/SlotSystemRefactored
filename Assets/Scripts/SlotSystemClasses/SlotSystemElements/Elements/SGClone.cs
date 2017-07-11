using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SGClone: SlotGroup{

		public override bool isPool{
			get{return m_isPool;}
			}bool m_isPool;
		public override bool isSGE{
			get{return m_isSGE;}
			}bool m_isSGE;
		public override bool isSGG{
			get{return m_isSGG;}
			}bool m_isSGG;
		public override bool isFocusedInBundle{
			get{return m_isFocusedInBundle;}
			}bool m_isFocusedInBundle;
		public override List<ISlottable> equippedSBs{
			get{return m_equippedSBs;}
			}List<ISlottable> m_equippedSBs;
		public override bool isAllTASBsDone{
			get{return m_isAllTASBsDone;}
			}bool m_isAllTASBsDone;
		public override bool isFocused{
			get{return m_isFocused;}
			}bool m_isFocused;
		public override bool isDefocused{
			get{return m_isDefocused;}
			}bool m_isDefocused;
		public override bool isDeactivated{
			get{return m_isDeactivated;}
			}bool m_isDeactivated;
		public override string eName{
			get{return m_eName;}
		}
		public void Initialize(ISlotGroup orig){
			this.scroller = orig.scroller;
			SetInventory(orig.inventory);
			this.isShrinkable = orig.isShrinkable;
			this.isExpandable = orig.isExpandable;
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
					slotsClone.Add(newSlot);
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
						sbsClone.Add(sb);
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
						newSbsClone.Add(sb);
					}
				}
				SetNewSBs(newSbsClone);
			m_isFocusedInBundle = orig.isFocusedInBundle;
			List<ISlottable> equippedSBsClone = new List<ISlottable>();
				if(orig.equippedSBs != null)
				foreach(ISlottable sb in orig.equippedSBs){
					ISlottable cloneSB = SlotSystemUtil.CloneSB(sb);
					equippedSBsClone.Add(cloneSB);
				}
				m_equippedSBs = equippedSBsClone;
			m_isAllTASBsDone = orig.isAllTASBsDone;
			SetInitSlotsCount(orig.initSlotsCount);
			this.m_isFocused = orig.isFocused;
			this.m_isDefocused = orig.isDefocused;
			this.m_isDeactivated = orig.isDeactivated;
			SetSorter(orig.Sorter);
			SetFilter(orig.Filter);
			this.m_eName = orig.eName;
			SetSelState(orig.prevSelState);
			SetSelState(orig.curSelState);
			SetActState(orig.prevActState);
			SetActState(orig.curActState);
		}
	}
}
