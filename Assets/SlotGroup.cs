using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class SlotGroup : MonoBehaviour, SlotSystemElement {
		string m_UTLog = "";
		public string UTLog{
			get{return m_UTLog;}
			set{m_UTLog = value;}
		}
	
		/*	states
		*/
			public void SetState(SlotGroupState state){
				m_prevState = m_curState;
				m_curState = state;
				if(m_curState != m_prevState){
					m_prevState.ExitState(this);
					m_curState.EnterState(this);
				}	
			}	
			SlotGroupState m_curState;
			public SlotGroupState CurState{
				get{
					if(m_curState == null)
						m_curState = SlotGroup.DeactivatedState;
					return m_curState;}
			}
			SlotGroupState m_prevState;
			public SlotGroupState PrevState{
				get{
					if(m_prevState == null)
						m_prevState = SlotGroup.DeactivatedState;
					return m_prevState;
				}
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
			private static SlotGroupState m_performingTransactionState;
			public static SlotGroupState PerformingTransactionState{
				get{
					if(m_performingTransactionState == null)
						m_performingTransactionState = new SGPerformingTransactionState();
					return m_performingTransactionState;
				}
			}
			private static SlotGroupState m_sortingState;
			public static SlotGroupState SortingState{
				get{
					if(m_sortingState == null)
						m_sortingState = new SGSortingState();
					return m_sortingState;
				}
			}

		/*	public fields
		*/
			AxisScrollerMock m_scroller;
			public AxisScrollerMock Scroller{
				get{return m_scroller;}
				set{m_scroller = value;}
			}
			Inventory m_inventory;
				public Inventory Inventory{
					get{return m_inventory;}
				}
				public void SetInventory(Inventory inv){
					m_inventory = inv;
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
			List<SlottableItem> m_filteredItems;
				public List<SlottableItem> FilteredItems{
					get{return m_filteredItems;}
				}
				public void SetFilteredItems(List<SlottableItem> filteredItems){
					m_filteredItems = filteredItems;
				}
			SlotGroupManager m_sgm;
			public SlotGroupManager SGM{
				get{return m_sgm;}
				set{m_sgm = value;}
			}
			public bool IsPool{
				get{
					return SGM.RootPage.PoolBundle.ContainsElement(this);
				}
			}
			bool m_autoSort = true;
			public bool IsAutoSort{
				get{return m_autoSort;}
			}
			public void ToggleAutoSort(bool on){
				m_autoSort = on;
				SGM.Focus();
			}
		
		/* commands
		*/
			SlotGroupCommand m_wakeUpCommand = new SGWakeupCommand();
				public SlotGroupCommand WakeUpCommand{
					get{return m_wakeUpCommand;}
					set{m_wakeUpCommand = value;}
				}
				public void WakeUp(){
					m_wakeUpCommand.Execute(this);
				}
			SlotGroupCommand m_initItemsCommand = new SGInitItemsCommand();
				public SlotGroupCommand InitItemsCommand{
					get{return m_initItemsCommand;}
					set{m_initItemsCommand = value;}
				}
				public void InitializeItems(){
					m_initItemsCommand.Execute(this);
				}
			SlotGroupCommand m_createSlotsCommand = new ConcCreateSlotsCommand();
				public SlotGroupCommand CreateSlotsCommand{
					get{return m_createSlotsCommand;}
					set{m_createSlotsCommand = value;}
				}
				public void CreateSlots(){
					m_createSlotsCommand.Execute(this);
				}
			SlotGroupCommand m_createSbsCommand = new ConcCreateSbsCommand();
				public SlotGroupCommand CreateSbsCommand{
					get{return m_createSbsCommand;}
					
				}
				public void CreateSlottables(){
					m_createSbsCommand.Execute(this);
				}
			
			SlotGroupCommand m_focusCommand = new SGFocusCommandV2();
				public SlotGroupCommand FocusCommand{
					get{return m_focusCommand;}
					set{m_focusCommand = value;}
				}
				public void Focus(){
					m_focusCommand.Execute(this);
				}
			SlotGroupCommand m_updateSbStateCommand = new UpdateSbStateCommandV2();
				public SlotGroupCommand UpdateSbStateCommand{
					get{return m_updateSbStateCommand;}
					set{m_updateSbStateCommand = value;}
				}
				public void UpdateSbState(){
					m_updateSbStateCommand.Execute(this);
				}
			SlotGroupCommand m_defocusCommand = new SGDefocusCommandV2();
				public SlotGroupCommand DefocusCommand{
					get{return m_defocusCommand;}
					set{m_defocusCommand = value;}
				}
				public void Defocus(){
					m_defocusCommand.Execute(this);
				}
			
			
			
		/*	sorter
		*/
			static SGSorter m_itemIDSorter = new SGItemIDSorter();
			public static SGSorter ItemIDSorter{
				get{
					return m_itemIDSorter;
				}
			}
			static SGSorter m_inverseItemIDSorter = new SGInverseItemIDSorter();
			public static SGSorter InverseItemIDSorter{
				get{
					return m_inverseItemIDSorter;
				}
			}
			static SGSorter m_acquisitionOrderSorter = new SGAcquisitionOrderSorter();
			public static SGSorter AcquisitionOrderSorter{
				get{
					return m_acquisitionOrderSorter;
				}
			}
			
			SGSorter m_sorter;
				public SGSorter Sorter{
					get{return m_sorter;}
				}
				public void SetSorter(SGSorter sorter){
					m_sorter = sorter;
				}
			List<Slottable> ReorderedSBs;
			// public void SetReorderedSBs(Slottable picked, Slottable hovered){
			// 	List<Slottable> result = new List<Slottable>();
			// 	foreach(Slot slot in Slots){
			// 		result.Add(slot.Sb);
			// 	}
			// 	int pickedId = result.IndexOf(picked);
			// 	int hoveredId = result.IndexOf(hovered);
			// 	result[pickedId] = hovered;
			// 	result[hoveredId] = picked;
			// 	this.ReorderedSBs = result;
			// }
			public void SetReorderedSBs(Slottable picked, Slottable hovered){
				List<Slottable> result = new List<Slottable>();
				foreach(Slot slot in Slots){
					result.Add(slot.Sb);
				}
				int pickedId = result.IndexOf(picked);
				int hoveredId = result.IndexOf(hovered);
				Slottable pickedOrig =  result[pickedId];
				if(pickedId < hoveredId){
					for (int i = 0; i < result.Count; i++)
					{
						if(i >= pickedId && i < hoveredId){
							result[i] = result[i + 1];
						}
					}
				}else{
					for(int i = result.Count - 1; i >= 0; i --){
						if(i > hoveredId && i <= pickedId){
							result[i] = result[i - 1];
						}
					}
				}
				result[hoveredId] = pickedOrig;
				this.ReorderedSBs = result;
			}
			public List<Slottable> OrderedSbs(){
				if(SGM.Transaction != null && SGM.Transaction.GetType()== typeof(ReorderTransaction)){
					// List<Slottable> result = ReorderedSBs;
					// // ReorderedSBs = null;
					// return result;
					return ReorderedSBs;
				}
				return Sorter.OrderedSbs(this);
			}
			public void InstantSort(){
				List<Slottable> newSlotOrderedSbs = OrderedSbs();
				List<Slottable> sbs = new List<Slottable>();
				foreach(Slot slot in Slots){
					if(slot.Sb != null)
						sbs.Add(slot.Sb);
					slot.Sb = null;
				}
				for(int i = 0; i < newSlotOrderedSbs.Count; i++){
					Slots[i].Sb = newSlotOrderedSbs[i];
				}
			}
		/*	filter
		*/
			SGFilter m_filter;
				public SGFilter Filter{
					get{return m_filter;}
					set{m_filter = value;}
				}
				public void FilterItems(){
					m_filter.Execute(this);
				}
		/*	events
		*/
			public void OnHoverEnterMock(PointerEventDataMock eventData){
				CurState.OnHoverEnterMock(this, eventData);
			}
			public void OnHoverExitMock(PointerEventDataMock eventData){
				CurState.OnHoverExitMock(this, eventData);
			}
		/*	process
		*/
			SGProcess m_curProcess;
			public SGProcess CurProcess{
				get{return m_curProcess;}
			}
			public void SetAndRun(SGProcess process){
				if(m_curProcess != null)
					m_curProcess.Stop();
				m_curProcess = process;
				if(m_curProcess != null)
					m_curProcess.Start();
			}
		/*	coroutines
		*/
			public IEnumeratorMock GreyoutCoroutine(){
				return null;
			}
			public IEnumeratorMock DehighlightCoroutine(){
				return null;
			}
			public IEnumeratorMock HighlightCoroutine(){
				return null;
			}
			public IEnumeratorMock GreyinCoroutine(){
				return null;
			}
			public IEnumeratorMock InstantGreyoutCoroutine(){
				return null;
			}
			public IEnumeratorMock InstantGreyinCoroutine(){
				return null;
			}
			public IEnumeratorMock UpdateTransactionCoroutine(){
				return null;
			}
			public IEnumeratorMock WaitForAllSlotMovementsDone(){
				bool flag = true;
				foreach(SlotMovement sm in slotMovements){
					flag &= sm.Completed;
				}
				if(flag){
					CurProcess.Expire();
				}
				return null;
			}
		/*	SlotMovement	*/
			List<SlotMovement> slotMovements;
			public List<SlotMovement> SlotMovements{
				get{
					if(slotMovements == null)
						slotMovements = new List<SlotMovement>();
					return slotMovements;
				}
			}
			public void AddSlotMovement(SlotMovement sm){
				if(slotMovements == null)
					slotMovements = new List<SlotMovement>();
				slotMovements.Add(sm);
			}
			public SlotMovement GetSlotMovement(Slottable sb){
				foreach(SlotMovement sm in SlotMovements){
					if(sm.SB == sb)
						return sm;
				}
				return null;
			}

		public void Activate(){
			InitializeItems();
		}
		public void Deactivate(){
			SetState(SlotGroup.DeactivatedState);
			foreach(Slot slot in Slots){
				if(slot.Sb != null){
					slot.Sb.SetState(Slottable.DeactivatedState);
				}
			}
		}
		
		public Slottable GetSlottable(SlottableItem itemInst){
			foreach(Slot slot in this.Slots){
				if(slot.Sb != null){
					InventoryItemInstanceMock invItemInst = (InventoryItemInstanceMock)slot.Sb.Item;
					if(itemInst.IsStackable){
						if(invItemInst == (InventoryItemInstanceMock)itemInst)
							return slot.Sb;
					}else if(object.ReferenceEquals(itemInst, invItemInst))
						return slot.Sb;
				}
			}
			return null;
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
		public SlotGroup GetSlotGroup(Slottable sb){
			return null;
		}
		public bool ContainsElement(SlotSystemElement element){
			return false;
		}
		public void FocusSBs(){
			foreach(Slot slot in Slots){
				Slottable sb = slot.Sb;
				if(sb != null){
					if(!this.IsAutoSort){
						sb.Focus();
						continue;
					}else{
						if(this.IsPool){
							if(sb.Item is PartsInstanceMock && !(this.Filter is SGPartsFilter)){
								sb.Defocus();
								continue;
							}else{
								if(sb.IsEquipped){
									sb.Defocus();
									continue;
								}
							}
						}
					}
					sb.Focus();
				}
			}
		}
		public void DefocusSBs(){
			foreach(Slot slot in Slots){
				if(slot.Sb != null)
					slot.Sb.Defocus();
			}
		}
		public bool AcceptsFilter(Slottable pickedSB){
			if(this.Filter is SGNullFilter) return true;
			else{
				if(pickedSB.Item is BowInstanceMock)
					return this.Filter is SGBowFilter;
				else if(pickedSB.Item is WearInstanceMock)
					return this.Filter is SGWearFilter;
				else if(pickedSB.Item is CarriedGearInstanceMock)
					return this.Filter is SGCarriedGearFilter;
				else// if(pickedSB.Item is PartsInstanceMock)
					return this.Filter is SGPartsFilter;
			}
		}
		public Slot GetSlot(Slottable sb){
			foreach(Slot slot in this.Slots){
				if(slot.Sb != null && slot.Sb == sb)
					return slot;
			}
			return null;
		}
		public Slot GetNextEmptySlot(){
			if(IsExpandable){
				Slot newSlot = new Slot();
				Slots.Add(newSlot);
				return newSlot;
			}else{
				foreach(Slot slot in Slots){
					if(slot.Sb == null)
						return slot;
				}
			}
			return null;
		}
		public void TransactionUpdate(Slottable added, Slottable removed){
			// if(added == null && removed == null)
			// 	SetState(SlotGroup.SortingState);
			// else
			// 	SetState(SlotGroup.PerformingTransactionState);
			if(SGM.Transaction.GetType() == typeof(SortTransaction))
				SetState(SlotGroup.SortingState);
			else{
				if(!m_autoSort)
					SGM.CompleteTransactionOnSG(this);
				else
					SetState(SlotGroup.SortingState);
			}
			
			/*	removal
			*/
			if(removed != null && GetSlottable(removed.Item) != null){
				EquipmentSet focusedEquipSet = (EquipmentSet)SGM.RootPage.EquipBundle.GetFocusedBundleElement();
				if(focusedEquipSet.ContainsElement(this))
					// Inventory.Items.Remove(removed.Item);
					Inventory.RemoveItem(removed.Item);
			}
			/*	addition
			*/
			if(added != null && !Inventory.Items.Contains(added.Item)){
				Inventory.AddItem(added.Item);
			}
		}
			
		public void TransactionUpdateV2(Slottable added, Slottable removed){
			/*	add/remove InventoryItem and Slottable
				do not destroy the removed Slottable yet (it remain in situ)
				then sort
				destruction is handled in Sb's removeingProcess's expiration

				if sg is Pool then Inventory addition and removal are exempted
				so are Slottable addition/removal
			*/
			/*	Addition and Removal  */
				Slot swapSlot = null;
				if(SGM.GetFocusedPoolSG() != this){
					/*	remove
					*/
					if(removed != null && Inventory.Items.Contains(removed.Item) && GetSlottable(removed.Item) != null){
						Inventory.RemoveItem(removed.Item);
						Slot slot = GetSlot(removed);
						SGM.removedSB = slot.Sb;
						slot.Sb = null;
						if(added != null)
							swapSlot = slot;
					}
					/*	add
					*/
					if(added != null && !Inventory.Items.Contains(added.Item) && GetSlottable(added.Item) == null){
						Inventory.AddItem(added.Item);
						/*	SB	*/
						GameObject newSBGO = new GameObject("newSBGO");
						Slottable newSB = newSBGO.AddComponent<Slottable>();
						InventoryItemInstanceMock item = (InventoryItemInstanceMock)added.Item;
						/*	slot	*/
						Slot slot = null;
						if(removed != null)
							slot = swapSlot;
						else
							slot = GetNextEmptySlot();
						/*	assemble	*/
						newSB.Initialize(this, true, item);
						slot.Sb = newSB;
					}
				}
			/*	Sorting  */
						
				if(SGM.Transaction.GetType() == typeof(SortTransaction) || SGM.Transaction.GetType() == typeof(ReorderTransaction)){
					SetState(SlotGroup.SortingState);
				}else{
					if(!m_autoSort){
						SGM.CompleteTransactionOnSG(this);
					}
					else
						SetState(SlotGroup.SortingState);
				}
		}
		public void RemoveSB(Slottable sb){
			/*	sb-slot relation stays intact until process is completed
			*/
			foreach(Slot slot in Slots){
				if(slot.Sb != null){
					if(slot.Sb == sb){
						if(SGM.PickedSB == sb)
							SGM.SetPickedSB(null);
						else if(SGM.SelectedSB == sb)
							SGM.SetSelectedSB(null);
						slot.Sb = null;
						DestroyImmediate(sb.gameObject);
						DestroyImmediate(sb);
					}
				}
			}
		}
		public void AddSB(ref Slot toSlot){
			/*	needs to be called after removing, when there's something to remove
			*/
			GameObject addedSBGO = new GameObject("newSBGO");
			Slottable addedSB = addedSBGO.AddComponent<Slottable>();
			InventoryItemInstanceMock item = null;
			foreach(InventoryItemInstanceMock it in Inventory.Items){
				if((Filter is SGBowFilter && it is BowInstanceMock) || (Filter is SGWearFilter && it is WearInstanceMock) || (Filter is SGCarriedGearFilter && it is CarriedGearInstanceMock)){
					bool found = false;
					foreach(Slot slot in Slots){
						if(slot.Sb != null){
							InventoryItemInstanceMock sbItem = (InventoryItemInstanceMock)slot.Sb.Item;
							if(sbItem == it)
								found = true;
						}
					}
					if(!found)
						item = it;
				}
			}
			if(item == null){
				throw new System.InvalidOperationException("a slottable with specified inventory item already exist.");
			}else{
				addedSB.Initialize(this, true, item);
				toSlot.Sb = addedSB;
			}
		}
	}
}
