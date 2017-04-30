using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SlotGroup : MonoBehaviour {
		string m_UTLog = "";
		public string UTLog{
			get{return m_UTLog;}
			set{m_UTLog = value;}
		}
		/* 	not used
		*/
			// Use this for initialization
			void Start () {
				
			}
			
			// Update is called once per frame
			void Update () {
				
			}
		/*	states
		*/
			SlotGroupState m_curState;
			public SlotGroupState CurState{
				get{
					if(m_curState == null)
						m_curState = DeactivatedState;
					return m_curState;}
			}
			private static SlotGroupState m_deactivatedState;
			public static SlotGroupState DeactivatedState{
				get{
					if(SlotGroup.m_deactivatedState == null)
						m_deactivatedState = new SGDeactivatedState();
					return m_deactivatedState;
				}
			}
			private static SlotGroupState m_defocusedState;
			public static SlotGroupState DefocusedState{
				get{
					if(SlotGroup.m_defocusedState == null)
						m_defocusedState = new SGDefocusedState();
					return m_defocusedState;
				}
			}
			private static SlotGroupState m_focusedState;
			public static SlotGroupState FocusedState{
				get{
					if(SlotGroup.m_focusedState == null)
						m_focusedState = new SGFocusedState();
					return m_focusedState;
				}
			}
			private static SlotGroupState m_selectedState;
			public static SlotGroupState SelectedState{
				get{
					if(m_selectedState == null)
						m_selectedState = new SGSelectedState();
					return m_selectedState;
				}
			}

		/*	public fields
		*/
			
			AxisScrollerMock m_scroller;
			public AxisScrollerMock Scroller{
				get{return m_scroller;}
				set{m_scroller = value;}
			}
			SGFilter m_filter;
			public SGFilter Filter{
				get{return m_filter;}
				set{m_filter = value;}
			}
			SGSorter m_sorter;
			public SGSorter Sorter{
				get{return m_sorter;}
				set{m_sorter = value;}
			}
			
			Inventory m_inventory;
			public Inventory Inventory{
				get{return m_inventory;}
			}
			bool m_isShrinkable;
			public bool IsShrinkable{
				get{return m_isShrinkable;}
				set{m_isShrinkable = value;}
			}
			bool m_isExpandable;
			public bool IsExpandable{
				get{return m_isExpandable;}
				set{m_isExpandable = value;}
			}
			List<Slot> m_slots = new List<Slot>();
			public List<Slot> Slots{
				get{return m_slots;}
				set{m_slots = value;}
			}
			
			List<SlottableItem> m_itemInstances;
			public List<SlottableItem> ItemInstances{
				get{return m_itemInstances;}
			}

			SlotGroupManager m_sgm;
			public SlotGroupManager SGM{
				get{return m_sgm;}
				set{m_sgm = value;}
			}

			public bool IsPool{
				get{
					bool flag = true;
					flag &= IsExpandable;
					flag &= IsShrinkable;
					return flag;
				}
			}
			public bool AutoSort = true;
		/*	state
		*/
		/* commands
		*/	
			SlotGroupCommand m_wakeUpCommand = new SGWakeupCommand();
			public SlotGroupCommand WakeUpCommand{
				get{return m_wakeUpCommand;}
				set{m_wakeUpCommand = value;}
			}
			SlotGroupCommand m_initItemsCommand = new SGInitItemsCommand();
			public SlotGroupCommand InitItemsCommand{
				get{return m_initItemsCommand;}
				set{m_initItemsCommand = value;}
			}
			SlotGroupCommand m_createSlotsCommand = new ConcCreateSlotsCommand();
			public SlotGroupCommand CreateSlotsCommand{
				get{return m_createSlotsCommand;}
				set{m_createSlotsCommand = value;}
			}
			SlotGroupCommand m_createSbsCommand = new ConcCreateSbsCommand();
			public SlotGroupCommand CreateSbsCommand{
				get{return m_createSbsCommand;}
				
			}
			SlotGroupCommand m_updateEquipStatusCommand;
			public SlotGroupCommand UpdateEquipStatusCommand{
				get{return m_updateEquipStatusCommand;}
				set{m_updateEquipStatusCommand = value;}
			}
			SlotGroupCommand m_focusCommand = new SGFocusCommand();
			public SlotGroupCommand FocusCommand{
				get{return m_focusCommand;}
				set{m_focusCommand = value;}
			}
			SlotGroupCommand m_updateSbStateCommand = new SGUpdateSbStateCommand();
			public SlotGroupCommand UpdateSbStateCommand{
				get{return m_updateSbStateCommand;}
				set{m_updateSbStateCommand = value;}
			}
			SlotGroupCommand m_defocusCommand = new SGDefocusCommand();
			public SlotGroupCommand DefocusCommand{
				get{return m_defocusCommand;}
				set{m_defocusCommand = value;}
			}
		public void SetState(SlotGroupState state){
			if(m_curState != state){
				m_curState = state;
				UpdateSbState();
			}
		}
		public void WakeUp(){
			m_wakeUpCommand.Execute(this);
		}
		public void SetInventory(Inventory inv){
			m_inventory = inv;
		}
		public void InitializeItems(){
			m_initItemsCommand.Execute(this);
		}
		public void SetItems(List<SlottableItem> filteredItems){
			m_itemInstances = filteredItems;
		}
		public void FilterItems(){
			m_filter.Execute(this);
		}
		public void SortItems(){
			m_sorter.Execute(this);
		}
		public void CreateSlots(){
			m_createSlotsCommand.Execute(this);
		}
		public void CreateSlottables(){
			m_createSbsCommand.Execute(this);
		}
		public void UpdateEquipStatus(){
			m_updateEquipStatusCommand.Execute(this);
		}
		public void Focus(){
			m_focusCommand.Execute(this);
		}
		public void UpdateSbState(){
			m_updateSbStateCommand.Execute(this);
		}
		public void Defocus(){
			m_defocusCommand.Execute(this);
		}
		public Slottable GetSlottable(InventoryItemInstanceMock itemInst){
			foreach(Slot slot in this.Slots){
				if(slot.Sb != null){
					InventoryItemInstanceMock invItemInst = (InventoryItemInstanceMock)slot.Sb.Item;
					if(itemInst.IsStackable){
						if(invItemInst == itemInst)
							return slot.Sb;
					}else if(object.ReferenceEquals(itemInst, invItemInst))
						return slot.Sb;
				}
			}
			return null;
		}
		public void OnHoverEnterMock(PointerEventDataMock eventData){
			CurState.OnHoverEnterMock(this, eventData);
		}
		public void OnHoverExitMock(PointerEventDataMock eventData){
			CurState.OnHoverExitMock(this, eventData);
		}
		public bool HasItem(InventoryItemInstanceMock invInst){
			bool result = false;
			foreach(Slot slot in this.Slots){
				if(slot.Sb != null){
					if((InventoryItemInstanceMock)slot.Sb.Item == invInst)
						return true;
				}
			}
			return result;
		}
	}
}
