using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public class SlotGroup : MonoBehaviour, SlotSystemElement, StateHandler{
		/*	states	*/
			/*	Engines	*/
				SGStateEngine SGSelStateEngine{
					get{
						if(m_sgSelStateEngine == null)
							m_sgSelStateEngine = new SGStateEngine(this);
						return m_sgSelStateEngine;
					}	
					}SGStateEngine m_sgSelStateEngine;
					public SGSelectionState CurSelState{
						get{return (SGSelectionState)SGSelStateEngine.curState;}
					}
					public SGSelectionState PrevSelState{
						get{return (SGSelectionState)SGSelStateEngine.prevState;}
					}
					public void SetSelState(SGSelectionState state){
						SGSelStateEngine.SetState(state);
					}
				SGStateEngine SGActStateEngine{
					get{
						if(m_sgActStateEngine == null)
							m_sgActStateEngine = new SGStateEngine(this);
						return m_sgActStateEngine;
					}	
					}SGStateEngine m_sgActStateEngine;
					public SGActionState CurActState{
						get{return (SGActionState)SGActStateEngine.curState;}
					}
					public SGActionState PrevActState{
						get{return (SGActionState)SGActStateEngine.prevState;}
					}
					public void SetActState(SGActionState state){
						SGActStateEngine.SetState(state);
					}
			/*	static states	*/
				/*	Selection states	*/
					public static SGSelectionState DeactivatedState{
						get{
							if(SlotGroup.m_deactivatedState == null)
								m_deactivatedState = new SGDeactivatedState();
							return m_deactivatedState;
						}
						}private static SGSelectionState m_deactivatedState;
					public static SGSelectionState DefocusedState{
						get{
							if(SlotGroup.m_defocusedState == null)
								m_defocusedState = new SGDefocusedState();
							return m_defocusedState;
						}
						}private static SGSelectionState m_defocusedState;
					public static SGSelectionState FocusedState{
						get{
							if(SlotGroup.m_focusedState == null)
								m_focusedState = new SGFocusedState();
							return m_focusedState;
						}
						}private static SGSelectionState m_focusedState;
					public static SGSelectionState SelectedState{
						get{
							if(m_selectedState == null)
								m_selectedState = new SGSelectedState();
							return m_selectedState;
						}
						}private static SGSelectionState m_selectedState;
				/*	Action States	*/
					public static SGActionState WaitForActionState{
						get{
							if(m_waitForActionState == null)
								m_waitForActionState = new SGWaitForActionState();
							return m_waitForActionState;
						}
						}private static SGActionState m_waitForActionState;
					// public static SGActionState PerformingTransactionState{
						// 	get{
						// 		if(m_performingTransactionState == null)
						// 			m_performingTransactionState = new SGPerformingTransactionState();
						// 		return m_performingTransactionState;
						// 	}
						// 	}private static SGActionState m_performingTransactionState;
					public static SGActionState RevertState{
						get{
							if(m_revertState == null)
								m_revertState = new SGRevertState();
							return m_revertState;
						}
						}private static SGActionState m_revertState;
					public static SGActionState ReorderState{
						get{
							if(m_reorderState == null)
								m_reorderState = new SGReorderState();
							return m_reorderState;
						}
						}private static SGActionState m_reorderState;
					public static SGActionState AddState{
						get{
							if(m_addState == null)
								m_addState = new SGAddState();
							return m_addState;
						}
						}private static SGActionState m_addState;
					public static SGActionState RemoveState{
						get{
							if(m_removeState == null)
								m_removeState = new SGRemoveState();
							return m_removeState;
						}
						}private static SGActionState m_removeState;
					public static SGActionState SwapState{
						get{
							if(m_swapState == null)
								m_swapState = new SGSwapState();
							return m_swapState;
						}
						}private static SGActionState m_swapState;
					public static SGActionState FillState{
						get{
							if(m_fillState == null)
								m_fillState = new SGFillState();
							return m_fillState;
						}
						}private static SGActionState m_fillState;
					public static SGActionState SortState{
						get{
							if(m_sortState == null)
								m_sortState = new SGSortState();
							return m_sortState;
						}
						}private static SGActionState m_sortState;
			/*	process	*/
				public SGProcess SelectionProcess{
					get{return m_selectionProcess;}
					}SGProcess m_selectionProcess;
					public void SetAndRunSelProcess(SGProcess process){
						if(m_selectionProcess != null)
							m_selectionProcess.Stop();
						m_selectionProcess = process;
						if(m_selectionProcess != null)
							m_selectionProcess.Start();
					}
				public SGProcess ActionProcess{
					get{return m_actionProcess;}
					}SGProcess m_actionProcess;
					public void SetAndRunActProcess(SGProcess process){
						if(m_actionProcess != null)
							m_actionProcess.Stop();
						m_actionProcess = process;
						if(m_actionProcess != null)
							m_actionProcess.Start();
					}
		/*	coroutines	*/
			public IEnumeratorMock GreyinCoroutine(){
				return null;
			}
			public IEnumeratorMock GreyoutCoroutine(){
				return null;
			}
			public IEnumeratorMock HighlightCoroutine(){
				return null;
			}
			public IEnumeratorMock DehighlightCoroutine(){
				return null;
			}
			public IEnumeratorMock InstantGreyoutCoroutine(){
				return null;
			}
			public IEnumeratorMock InstantGreyinCoroutine(){
				return null;
			}
			public IEnumeratorMock TransactionCoroutine(){
				bool flag = true;
				if(allTASBs == null/* as in when testing */)
					return null;
				foreach(Slottable sb in allTASBs){
					flag &= sb.ActionProcess.IsExpired;
				}
				if(flag){
					ActionProcess.Expire();
				}
				return null;
			}
			/*	dump	*/
				// public IEnumeratorMock UpdateTransactionCoroutine(){
				// 	return null;
				// }
				// public IEnumeratorMock WaitForAllSlotMovementsDone(){
				// 	bool flag = true;
				// 	foreach(SlotMovement sm in slotMovements){
				// 		flag &= sm.SB.CurProcess.IsExpired;
				// 	}
				// 	if(flag){
				// 		CurProcess.Expire();
				// 	}
				// 	return null;
				// }
		/*	public fields	*/
			public AxisScrollerMock Scroller{
				get{return m_scroller;}
				set{m_scroller = value;}
				}AxisScrollerMock m_scroller;
			public Inventory inventory{
				get{return m_inventory;}
				}
				Inventory m_inventory;
				public void SetInventory(Inventory inv){
					m_inventory = inv;
				}
			public bool IsShrinkable{
				get{return m_isShrinkable;}
				set{m_isShrinkable = value;}
				}bool m_isShrinkable;
			public bool IsExpandable{
				get{return m_isExpandable;}
				set{m_isExpandable = value;}
				}bool m_isExpandable;
			public List<Slot> Slots{
				get{
					if(m_slots == null)
						m_slots = new List<Slot>();
					return m_slots;}
				}List<Slot> m_slots;
				public void SetSlots(List<Slot> slots){
					m_slots = slots;
				}
			public List<Slot> newSlots{
				get{return m_newSlots;}
				}List<Slot> m_newSlots;
				public void SetNewSlots(List<Slot> newSlots){
					m_newSlots = newSlots;
				}
			// public List<SlottableItem> FilteredItems{
				// 	get{return m_filteredItems;}
				// 	}List<SlottableItem> m_filteredItems;
				// 	public void SetFilteredItems(List<SlottableItem> filteredItems){
				// 		m_filteredItems = filteredItems;
				// 	}
			public SlotGroupManager SGM{
				get{return m_sgm;}
				set{m_sgm = value;}
				}SlotGroupManager m_sgm;
			public bool IsPool{
				get{
					return SGM.AllSGPs.Contains(this);
				}
			}
			public bool IsSGE{
				get{
					return SGM.AllSGEs.Contains(this);
				}
			}
			public bool IsAutoSort{
				get{return m_autoSort;}
				}bool m_autoSort = true;
				public void ToggleAutoSort(bool on){
					m_autoSort = on;
					SGM.Focus();
				}
			public List<Slottable> Slottables{
				get{
					List<Slottable> result = new List<Slottable>();
						foreach(Slot slot in this.Slots){
							if(slot.sb != null)
								result.Add(slot.sb);
							else
								result.Add(null);
						}
					return result;
				}
			}
			public List<Slottable> newSBs{
				get{return m_newSBs;}
				}List<Slottable> m_newSBs;
				public void SetNewSBs(List<Slottable> sbs){
					m_newSBs = sbs;
				}
			public List<InventoryItemInstanceMock> ItemInstances{
				get{
					List<InventoryItemInstanceMock> result = new List<InventoryItemInstanceMock>();
						foreach(Slottable sb in Slottables){
							if(sb != null)
								result.Add(sb.ItemInst);
							else
								result.Add(null);
						}
					return result;
				}
			}
			public List<InventoryItemInstanceMock> ActualItemInsts{
				get{
					List<InventoryItemInstanceMock> result = new List<InventoryItemInstanceMock>();
					foreach(InventoryItemInstanceMock itemInst in ItemInstances){
						if(itemInst != null)
							result.Add(itemInst);
					}
					return result;
				}
			}
			public bool IsFocused{
				get{return CurSelState == SlotGroup.FocusedState;}
			}
			public bool HasEmptySlot{
				get{
					bool emptyFound = false;
					foreach(Slot slot in Slots){
						if(slot.sb == null)
							emptyFound = true;
					}
					return emptyFound;
				}
			}
			public int ActualSBsCount{
				get{
					int count = 0;
					foreach(Slot slot in Slots){
						if(slot.sb != null)
							count ++;
					}
					return count;
				}
			}
			public List<Slottable> EquippedSBs{
				get{
					List<Slottable> result = new List<Slottable>();
					foreach(Slottable sb in Slottables){
						if(sb != null && sb.IsEquipped)
							result.Add(sb);
					}
					return result;
				}
			}
			public List<Slottable> allTASBs{
				get{return m_allTASBs;}
				}List<Slottable> m_allTASBs;
				public void SetAllTASBs(List<Slottable> sbs){
					m_allTASBs = sbs;
				}
			public bool IsAllTASBsDone{
				get{
					foreach(Slottable sb in allTASBs){
						if(sb != null){
							if(sb.ActionProcess.IsRunning)
								return false;
						}
					}
					return true;
				}
			}
			public int initSlotsCount{
				get{return m_initSlotsCount;}
				}int m_initSlotsCount;
			// public bool IsSMDone{
				// 	get{
				// 		bool done = true;
				// 		if(SlotMovements != null){
				// 			foreach(SlotMovement sm in SlotMovements){
				// 				if(sm.SB != SGM.pickedSB && sm.SB != SGM.sg2){
				// 					if(sm.SB.ActionProcess != null && sm.SB.ActionProcess.IsRunning)
				// 						return false;
				// 				}
				// 			}
				// 		}
				// 		return done;
				// 	}
				// }
		/*	commands methods	*/
			public void InitializeItems(){
				m_initItemsCommand.Execute(this);
				}SlotGroupCommand m_initItemsCommand = new SGInitItemsCommand();
			// public SlotGroupCommand CreateSlotsCommand{
				// 	get{return m_createSlotsCommand;}
				// 	set{m_createSlotsCommand = value;}
				// 	}SlotGroupCommand m_createSlotsCommand = new ConcCreateSlotsCommand();
				// 	public void CreateSlots(){
				// 		m_createSlotsCommand.Execute(this);
				// 	}
				// public SlotGroupCommand CreateSbsCommand{
				// 	get{return m_createSbsCommand;}
				// 	}SlotGroupCommand m_createSbsCommand = new ConcCreateSbsCommand();
				// 	public void CreateSlottables(){
				// 		m_createSbsCommand.Execute(this);
				// 	}
			/*	dump	*/
				// SlotGroupCommand m_focusCommand = new SGFocusCommandV2();
				// 	public SlotGroupCommand FocusCommand{
				// 		get{return m_focusCommand;}
				// 		set{m_focusCommand = value;}
				// 	}
				// 	public void Focus(){
				// 		m_focusCommand.Execute(this);
				// 	}
				// SlotGroupCommand m_defocusCommand = new SGDefocusCommandV2();
				// 	public SlotGroupCommand DefocusCommand{
				// 		get{return m_defocusCommand;}
				// 		set{m_defocusCommand = value;}
				// 	}
				// 	public void Defocus(){
				// 		m_defocusCommand.Execute(this);
				// 	}
				// SlotGroupCommand m_wakeUpCommand = new SGWakeupCommand();
				// 	public SlotGroupCommand WakeUpCommand{
				// 		get{return m_wakeUpCommand;}
				// 		set{m_wakeUpCommand = value;}
				// 	}
				// 	public void WakeUp(){
				// 		m_wakeUpCommand.Execute(this);
				// 	}
				// SlotGroupCommand m_updateSbStateCommand = new UpdateSbStateCommandV2();
				// 	public SlotGroupCommand UpdateSbStateCommand{
				// 		get{return m_updateSbStateCommand;}
				// 		set{m_updateSbStateCommand = value;}
				// 	}
				// 	public void UpdateSbState(){
				// 		m_updateSbStateCommand.Execute(this);
				// 	}
		/*	sorter	*/
			public static SGSorter ItemIDSorter{
				get{
					return m_itemIDSorter;
				}
				}static SGSorter m_itemIDSorter = new SGItemIDSorter();
			public static SGSorter InverseItemIDSorter{
				get{
					return m_inverseItemIDSorter;
				}
				}static SGSorter m_inverseItemIDSorter = new SGInverseItemIDSorter();
			public static SGSorter AcquisitionOrderSorter{
				get{
					return m_acquisitionOrderSorter;
				}
				}static SGSorter m_acquisitionOrderSorter = new SGAcquisitionOrderSorter();
			
			public SGSorter Sorter{
				get{return m_sorter;}
				}SGSorter m_sorter;
				public void SetSorter(SGSorter sorter){
					m_sorter = sorter;
				}
			// List<Slottable> ReorderedSBs;
			public void ReorderSBs(Slottable picked, Slottable hovered, ref List<Slottable> reorderedSBs){
				List<Slottable> result = new List<Slottable>();
				foreach(Slottable sb in reorderedSBs){
					result.Add(sb);
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
				reorderedSBs = result;
			}
			// public List<Slottable> OrderedSbs(){
				// 	List<Slottable> result = new List<Slottable>();
				// 	if(SGM.Transaction != null && SGM.Transaction.GetType()== typeof(ReorderTransaction)){
				// 		return ReorderedSBs;
				// 	}
				// 	return Sorter.OrderedSBs(this);
				// }
			public void InstantSort(){
				// List<Slottable> newSlotOrderedSbs = OrderedSbs();
					// List<Slottable> sbs = new List<Slottable>();
					// foreach(Slot slot in Slots){
					// 	if(slot.Sb != null)
					// 		sbs.Add(slot.Sb);
					// 	slot.Sb = null;
					// }
					// for(int i = 0; i < newSlotOrderedSbs.Count; i++){
					// 	Slots[i].Sb = newSlotOrderedSbs[i];
					// }
				List<Slottable> origSBs = Slottables;
				Sorter.OrderSBsWithRetainedSize(ref origSBs);
				foreach(Slot slot in Slots){
					slot.sb = origSBs[Slots.IndexOf(slot)];
				}
			}
		/*	filter	*/
			public static SGFilter NullFilter{
				get{
					if(m_nullFilter == null)
						m_nullFilter = new SGNullFilter();
					return m_nullFilter;
				}
				}static SGFilter m_nullFilter;
			public static SGFilter BowFilter{
				get{
					if(m_bowFilter == null)
						m_bowFilter = new SGBowFilter();
					return m_bowFilter;
				}
				}static SGFilter m_bowFilter;
			public static SGFilter WearFilter{
				get{
					if(m_wearFilter == null)
						m_wearFilter = new SGWearFilter();
					return m_wearFilter;
				}
				}static SGFilter m_wearFilter;
			public static SGFilter CGearsFilter{
				get{
					if(m_cGearsFilter == null)
						m_cGearsFilter = new SGCGearsFilter();
					return m_cGearsFilter;
				}
				}static SGFilter m_cGearsFilter;
			public static SGFilter PartsFilter{
				get{
					if(m_partsFilter == null)
						m_partsFilter = new SGPartsFilter();
					return m_partsFilter;
				}
				}static SGFilter m_partsFilter;
			public SGFilter Filter{
				get{return m_filter;}
				}SGFilter m_filter;
				public void SetFilter(SGFilter filter){
					m_filter = filter;
				}
				// public void FilterItems(){
				// 	m_filter.Execute(this);
				// }
			public bool AcceptsFilter(Slottable pickedSB){
				if(this.Filter is SGNullFilter) return true;
				else{
					if(pickedSB.Item is BowInstanceMock)
						return this.Filter is SGBowFilter;
					else if(pickedSB.Item is WearInstanceMock)
						return this.Filter is SGWearFilter;
					else if(pickedSB.Item is CarriedGearInstanceMock)
						return this.Filter is SGCGearsFilter;
					else// if(pickedSB.Item is PartsInstanceMock)
						return this.Filter is SGPartsFilter;
				}
			}
		/*	events	*/
			public void OnHoverEnterMock(){
				PointerEventDataMock eventData = new PointerEventDataMock();
				CurSelState.OnHoverEnterMock(this, eventData);
			}
			public void OnHoverExitMock(){
				PointerEventDataMock eventData = new PointerEventDataMock();
				CurSelState.OnHoverExitMock(this, eventData);
			}
		/*	SlotMovement	*/
			// public List<SlotMovement> SlotMovements{
			// 	get{
			// 		if(m_slotMovements == null)
			// 			m_slotMovements = new List<SlotMovement>();
			// 		return m_slotMovements;
			// 	}
			// 	}List<SlotMovement> m_slotMovements;
			// public void AddSlotMovement(SlotMovement sm){
			// 	if(m_slotMovements == null)
			// 		m_slotMovements = new List<SlotMovement>();
			// 	m_slotMovements.Add(sm);
			// }
			// public SlotMovement GetSlotMovement(Slottable sb){
			// 	foreach(SlotMovement sm in SlotMovements){
			// 		if(sm.SB == sb)
			// 			return sm;
			// 	}
			// 	return null;
			// }
			// public void ExecuteSlotMovements(){
			// 	foreach(SlotMovement sm in SlotMovements)
			// 		sm.Execute();
			// }
		/*	SlotSystemElement implementation	*/
			public SlotGroup GetSlotGroup(Slottable sb){
				/*	this is for SlotSystem implementation	*/
				return null;
			}
			public bool ContainsElement(SlotSystemElement element){
				return false;
			}
			public void Focus(){
				SetSelState(SlotGroup.FocusedState);
				FocusSBs();
				Reset();
			}
			public void FocusSelf(){
				SetSelState(SlotGroup.FocusedState);
			}
			public void Defocus(){
				SetSelState(SlotGroup.DefocusedState);
				DefocusSBs();
				Reset();
			}
			public void DefocusSelf(){
				SetSelState(SlotGroup.DefocusedState);
			}
		/*	methods	*/
			public void Initialize(SGFilter filter, Inventory inv, bool isShrinkable, int initSlotsCount){
				SetFilter(filter);
				SetSorter(SlotGroup.ItemIDSorter);
				SetInventory(inv);
				this.IsShrinkable = isShrinkable;
				if(initSlotsCount == 0)
					this.IsExpandable = true;
				else
					this.IsExpandable = false;
				m_initSlotsCount = initSlotsCount;
				InitializeItems();			
				SetSelState(SlotGroup.DeactivatedState);
				SetActState(SlotGroup.WaitForActionState);
			}
			public void Activate(){
				// InitializeItems();
			}
			public void Deactivate(){
				SetSelState(SlotGroup.DeactivatedState);
				foreach(Slottable sb in Slottables){
					if(sb != null){
						sb.SetSelState(Slottable.DeactivatedState);
					}
				}
			}
			public Slottable GetSB(InventoryItemInstanceMock itemInst){
				foreach(Slottable sb in this.Slottables){
					if(sb != null){
						if(sb.ItemInst == itemInst)
							return sb;
					}
				}
				if(allTASBs != null){
					foreach(Slottable sb in allTASBs){
						if(sb != null){
							if(sb.ItemInst == itemInst)
								return sb;
						}
					}
				}
				return null;
			}
			public bool HasItemCurrently(InventoryItemInstanceMock invInst){
				bool result = false;
				foreach(Slottable sb in this.Slottables){
					if(sb != null){
						if(sb.ItemInst == invInst)
							return true;
					}
				}
				return result;
			}
			public void FocusSBs(){
				foreach(Slottable sb in Slottables){
					if(sb != null){
						sb.SetActState(Slottable.WaitForActionState);
						sb.Reset();
						if(sb.IsPickable)
							sb.Focus();
						else
							sb.Defocus();
					}
				}
			}
			public void DefocusSBs(){
				foreach(Slottable sb in Slottables){
					if(sb != null)
						sb.SetActState(Slottable.WaitForActionState);
						sb.Reset();
						sb.Defocus();
				}
			}
			public Slot GetSlot(InventoryItemInstanceMock itemInst){
				foreach(Slot slot in this.Slots){
					if(slot.sb != null){
						if(slot.sb.ItemInst == itemInst)
							return slot;
					}
				}
				return null;
			}
			// public void UpdateSlots(){
				// 	while(Slots.Count < newSBs.Count){
				// 		Slot newSlot = new Slot();
				// 		Slots.Add(newSlot);
				// 	}
				// }
			public void CreateNewSlots(){
				List<Slot> newSlots = new List<Slot>();
				for(int i = 0; i < newSBs.Count; i++){
					Slot newSlot = new Slot();
					newSlots.Add(newSlot);
				}
				SetNewSlots(newSlots);
			}
			public Slot GetNewSlot(InventoryItemInstanceMock itemInst){
				int index = -3;
				foreach(Slottable sb in newSBs){
					if(sb != null){
						if(sb.ItemInst == itemInst)
							index = newSBs.IndexOf(sb);
					}
				}
				return newSlots[index];
			}
			// public Slot GetNextEmptySlot(){
				// 	if(IsExpandable){
				// 		Slot newSlot = new Slot();
				// 		Slots.Add(newSlot);
				// 		return newSlot;
				// 	}else{
				// 		foreach(Slot slot in Slots){
				// 			if(slot.Sb == null)
				// 				return slot;
				// 		}
				// 	}
				// 	return null;
				// }
			// public void SetAndRunSlotMovements(List<InventoryItemInstanceMock> removed, List<InventoryItemInstanceMock> added){
				// 	List<Slottable> newSBsList = new List<Slottable>();
				// 	/*	remove	and index */
				// 	foreach(Slottable sb in Slottables){
				// 		if(removed != null && sb != null && removed.Contains(sb.ItemInst)){
				// 			if(!IsPool){
				// 				SlotMovement newSM = new SlotMovement(this, sb, sb.SlotID, -2);
				// 			}else{
				// 				SlotMovement newSM = new SlotMovement(this, sb, sb.SlotID, -1);
				// 			}
				// 		}else
				// 			newSBsList.Add(sb);
				// 	}
				// 	/*	scooch (optional)	*/
				// 		List<Slottable> temp = new List<Slottable>();
				// 		foreach(Slottable sb in newSBsList){
				// 			if(sb != null) temp.Add(sb);
				// 		}
				// 		while(temp.Count < newSBsList.Count){
				// 			temp.Add(null);
				// 		}
				// 		newSBsList = temp;
				// 	/*	add	*/
				// 		if(added != null){
				// 			foreach(InventoryItemInstanceMock invInst in added){
				// 				GameObject newSBGO = new GameObject("newSBGO");
				// 				Slottable newSB = newSBGO.AddComponent<Slottable>();
				// 				newSB.Initialize(SGM, this, true, invInst);
				// 				int index = FindNextEmpty(ref newSBsList);
				// 				// newSBsList.Add(newSB);
				// 				newSBsList[index] = newSB;
				// 			}
				// 		}
				// 	/*	sort	*/
				// 	List<Slottable> newListOrdered = new List<Slottable>();
				// 	newListOrdered = newSBsList;
				// 	if(IsAutoSort)
				// 		// Sorter.OrderSBs(ref newListOrdered);
				// 		Sorter.OrderSBsWOSpace(ref newListOrdered);
				// 	/*	index	*/
				// 	foreach(Slottable sb in newSBsList){
				// 		if(sb != null){
				// 			SlotMovement newSM;
				// 			if(added != null && added.Contains(sb.ItemInst)){
				// 				if(!IsPool)
				// 					newSM = new SlotMovement(this, sb, -2, newListOrdered.IndexOf(sb));
				// 				else
				// 					newSM = new SlotMovement(this, sb, -1, newListOrdered.IndexOf(sb));
				// 			}else
				// 				newSM = new SlotMovement(this, sb, sb.SlotID, newListOrdered.IndexOf(sb));
				// 		}
				// 	}
				// 	ExecuteSlotMovements();
				// 	SetActState(SlotGroup.PerformingTransactionState);
				// 	CheckCompletion();
				// }
				// // public void SetAndRunSlotMovementsForReorder(Slottable pickedSB, Slottable hoveredSB){
				// // 	SetReorderedSBs(pickedSB, hoveredSB);
				// // 	List<Slottable> reorderedSB = this.ReorderedSBs;
				// // 	foreach(Slottable sb in Slottables){
				// // 		if(sb != null){
				// // 			SlotMovement sm = new SlotMovement(this, sb, sb.SlotID, reorderedSB.IndexOf(sb));
				// // 		}
				// // 	}
				// // 	ExecuteSlotMovements();
				// // 	SetActState(SlotGroup.PerformingTransactionState);
				// // 	CheckCompletion();
				// // }
				// public void SetAndRunSlotMovementsForSort(){
				// 	List<Slottable> orderedSBs = this.OrderedSbs();
				// 	foreach(Slottable sb in Slottables){
				// 		if(sb != null){
				// 			SlotMovement sm = new SlotMovement(this, sb, sb.SlotID, orderedSBs.IndexOf(sb));
				// 		}
				// 	}
				// 	ExecuteSlotMovements();
				// 	SetActState(SlotGroup.PerformingTransactionState);
				// 	CheckCompletion();
				// }
				// public void SetAndRunSlotmovementsForSwap(Slottable removed, Slottable added){
				// 	if(removed != null && added != null){
				// 		List<Slottable> newSBsList = new List<Slottable>();
				// 		int removedId = -1;
				// 		/*	remove	and index */
				// 		foreach(Slottable sb in Slottables){
				// 			if(sb != null && sb.ItemInst == removed.ItemInst){
				// 				removedId = sb.SlotID;
				// 				newSBsList.Add(null);
				// 				if(!IsPool){
				// 					SlotMovement newSM = new SlotMovement(this, sb, sb.SlotID, -2);
				// 				}else{
				// 					SlotMovement newSM = new SlotMovement(this, sb, sb.SlotID, -1);
				// 				}
				// 			}else
				// 				newSBsList.Add(sb);
				// 		}
				// 		if(removedId == -1)
				// 			throw new System.InvalidOperationException("SetAndRunSlotMovementsForSwap: removed sb not contained in the selectedSG");
				// 		/*	add	*/
				// 			GameObject newSBGO = new GameObject("newSBGO");
				// 			Slottable newSB = newSBGO.AddComponent<Slottable>();
				// 			newSB.Initialize(SGM, this, true, added.ItemInst);
				// 			newSBsList[removedId] = newSB;
				// 		/*	sort	*/
				// 		List<Slottable> newListOrdered = new List<Slottable>();
				// 		newListOrdered = newSBsList;
				// 		if(IsAutoSort)
				// 			Sorter.OrderSBsWOSpace(ref newListOrdered);
				// 		/*	index	*/
				// 		foreach(Slottable sb in newSBsList){
				// 			if(sb != null){
				// 				SlotMovement newSM;
				// 				if(sb.ItemInst == added.ItemInst){
				// 					if(!IsPool)
				// 						newSM = new SlotMovement(this, sb, -2, newListOrdered.IndexOf(sb));
				// 					else
				// 						newSM = new SlotMovement(this, sb, -1, newListOrdered.IndexOf(sb));
				// 				}else
				// 					newSM = new SlotMovement(this, sb, sb.SlotID, newListOrdered.IndexOf(sb));
				// 			}
				// 		}
				// 		ExecuteSlotMovements();	
				// 	}else{
				// 		throw new System.InvalidOperationException("SetAndRunSlotMovementsForSwap: removed nor added sb not assigned properly");
				// 	}
				// }
			public int FindNextEmpty(ref List<Slottable> sbList){
				foreach(Slottable sb in sbList){
					if(sb == null)
						return sbList.IndexOf(sb);
				}
				sbList.Add(null);
				return sbList.Count -1;
			}
			public void SetSBsActStates(){
				List<Slottable> moveWithins = new List<Slottable>();
				List<Slottable> removed = new List<Slottable>();
				List<Slottable> added = new List<Slottable>();
				foreach(Slottable sb in Slottables){
					if(sb != null){
						if(newSBs.Contains(sb))
							moveWithins.Add(sb);
						else
							removed.Add(sb);
					}
				}
				foreach(Slottable sb in newSBs){
					if(sb != null){
						if(!Slottables.Contains(sb))
							added.Add(sb);
					}
				}
				foreach(Slottable sb in moveWithins){
					sb.SetNewSlotID(newSBs.IndexOf(sb));
					sb.SetActState(Slottable.MoveWithinState);
				}
				foreach(Slottable sb in removed){
					sb.SetNewSlotID(-1);
					sb.SetActState(Slottable.RemovedState);
				}
				foreach(Slottable sb in added){
					sb.SetNewSlotID(newSBs.IndexOf(sb));
					sb.SetActState(Slottable.AddedState);
				}
				List<Slottable> allSBs = new List<Slottable>();
				allSBs.AddRange(Slottables);
				allSBs.AddRange(added);
				SetAllTASBs(allSBs);
			}
			// public void CheckCompletion(){
				// 	CheckSBsSlotMovementCompletion();
				// 	CheckProcessCompletion();
				// }
				// public void CheckSBsSlotMovementCompletion(){
				// 	foreach(SlotMovement sm in SlotMovements){
				// 		if(sm.SB.ActionProcess.GetType() == typeof(SBMoveInSGProcess) ||
				// 		sm.SB.ActionProcess.GetType() == typeof(SBRemoveProcess) ||
				// 		sm.SB.ActionProcess.GetType() == typeof(SBAddProcess)){
				// 			int curId; int newId;
				// 			sm.GetIndex(out curId, out newId);
				// 			if(curId == newId)
				// 				sm.SB.ExpireActionProcess();
				// 		}
				// 	}
				// }
			public void CheckProcessCompletion(){
				ActionProcess.Check();
			}
			public void OnCompleteSlotMovementsV2(){
				// foreach(Slot slot in Slots){
				// 	slot.Sb = null;
				// }
				// List<Slot> newSlots = new List<Slot>();
				// for(int i = 0; i<newSBs.Count; i++){
				// 	if(Slots.Count <= i){
				// 		Slot newSlot = new Slot();
				// 		newSlots.Add(newSlot);
				// 	}else
				// 		newSlots.Add(Slots[i]);
				// }
				// foreach(Slot slot in newSlots){
				// 	slot.Sb = newSBs[newSlots.IndexOf(slot)];
				// }
				foreach(Slottable sb in newSBs){
					if(sb != null){
						newSlots[sb.newSlotID].sb = sb;
					}
				}
				SetSlots(newSlots);
			}
			// public void OnCompleteSlotMovements(){
				// 	foreach(Slot slot in Slots){
				// 		slot.Sb = null;
				// 	}
				// 	while(Slots.Count < SlotMovements.Count){
				// 		Slot newSlot = new Slot();
				// 		newSlot.Position = Vector2.zero;
				// 		Slots.Add(newSlot);
				// 	}
				// 	foreach(SlotMovement sm in SlotMovements){
				// 		int curId;
				// 		int newId;
				// 		sm.GetIndex(out curId, out newId);
				// 		if(newId == -1 || newId == -2){
				// 			GameObject go = sm.SB.gameObject;
				// 			DestroyImmediate(go);
				// 			DestroyImmediate(sm.SB);
				// 		}else{
				// 			Slots[newId].Sb = sm.SB;
				// 		}
				// 	}
				// 	if(IsExpandable){
				// 		List<Slot> newSlots = new List<Slot>();
				// 		foreach(Slot slot in Slots){
				// 			if(slot.Sb != null)
				// 				newSlots.Add(slot);
				// 		}
				// 		Slots = newSlots;
				// 	}
				// 	SlotMovements.Clear();
				// }
			// public Slot GetSlotForAdded(Slottable sb){
				// 	Slot slot = null;
				// 	if(SlotMovements.Count == 0){
				// 		return GetSlot(GetSB(sb.ItemInst));
				// 	}else{
				// 		foreach(SlotMovement sm in SlotMovements){
				// 			if(sm.SB.ItemInst == sb.ItemInst){
				// 				int curId; int newId;
				// 				sm.GetIndex(out curId, out newId);
				// 				slot = Slots[newId];
				// 			}
				// 		}
				// 		return slot;
				// 	}
				// }
			public void InstantGreyout(){}
			public void InstantGreyin(){}
			public void InstantHighlight(){}
			// public bool IsFillEquippable(Slottable sb){
				// 	SlotGroup origSG = sb.SG;
				// 	if(!(HasItem(sb.ItemInst) && !IsPool)){
				// 		if(origSG.IsShrinkable){
				// 			if(this != origSG){
				// 				if(IsFocused){
				// 					if(AcceptsFilter(sb)){
				// 						if(IsExpandable){
				// 							return true;
				// 						}else{
				// 							foreach(Slot slot in Slots){
				// 								if(slot.Sb == null)
				// 									return true;
				// 							}
				// 						}
				// 					}
				// 				}
				// 			}
				// 		}
				// 	}
				// 	return false;
				// }
			public List<Slottable> SwappableSBs(Slottable pickedSB){
				List<Slottable> result = new List<Slottable>();
				foreach(Slottable sb in Slottables){
					if(sb != null){
						if(Util.IsSwappable(pickedSB, sb))
							result.Add(sb);
					}
				}
				return result;
			}
			public void Reset(){
				SetActState(SlotGroup.WaitForActionState);
				SetAllTASBs(null);
				SetNewSBs(null);
				SetNewSlots(null);
			}
		/*	dump	*/
			// public void CheckTransactionCompletionOnSBs(){
			// 	bool flag = true;
			// 	foreach(SlotMovement sm in SlotMovements){
			// 		Slottable sb = sm.SB;
			// 		flag |= sb.CurProcess.IsExpired;
			// 	}
			// 	if(flag)
			// 		SGM.CompleteTransactionOnSG(this);
			// }
			// public void StateTransit(){
			// 	foreach(SlotMovement sm in SlotMovements){
			// 		sm.StateTransit();
			// 	}
			// }
			// public void RemoveSB(Slottable sb){
			// 	/*	sb-slot relation stays intact until process is completed
			// 	*/
			// 	foreach(Slot slot in Slots){
			// 		if(slot.Sb != null){
			// 			if(slot.Sb == sb){
			// 				if(SGM.PickedSB == sb)
			// 					SGM.SetPickedSB(null);
			// 				else if(SGM.SelectedSB == sb)
			// 					SGM.SetSelectedSB(null);
			// 				slot.Sb = null;
			// 				DestroyImmediate(sb.gameObject);
			// 				DestroyImmediate(sb);
			// 			}
			// 		}
			// 	}
			// }
			// public void AddSB(ref Slot toSlot){
			// 	/*	needs to be called after removing, when there's something to remove
			// 	*/
			// 	GameObject addedSBGO = new GameObject("newSBGO");
			// 	Slottable addedSB = addedSBGO.AddComponent<Slottable>();
			// 	InventoryItemInstanceMock item = null;
			// 	foreach(InventoryItemInstanceMock it in Inventory.Items){
			// 		if((Filter is SGBowFilter && it is BowInstanceMock) || (Filter is SGWearFilter && it is WearInstanceMock) || (Filter is SGCarriedGearFilter && it is CarriedGearInstanceMock)){
			// 			bool found = false;
			// 			foreach(Slot slot in Slots){
			// 				if(slot.Sb != null){
			// 					InventoryItemInstanceMock sbItem = (InventoryItemInstanceMock)slot.Sb.Item;
			// 					if(sbItem == it)
			// 						found = true;
			// 				}
			// 			}
			// 			if(!found)
			// 				item = it;
			// 		}
			// 	}
			// 	if(item == null){
			// 		throw new System.InvalidOperationException("a slottable with specified inventory item already exist.");
			// 	}else{
			// 		addedSB.Initialize(this.SGM, true, item);
			// 		toSlot.Sb = addedSB;
			// 	}
			// }
			// public void TransactionUpdate(Slottable added, Slottable removed){
			// 	// if(added == null && removed == null)
			// 	// 	SetState(SlotGroup.SortingState);
			// 	// else
			// 	// 	SetState(SlotGroup.PerformingTransactionState);
			// 	if(SGM.Transaction.GetType() == typeof(SortTransaction))
			// 		SetState(SlotGroup.SortingState);
			// 	else{
			// 		if(!m_autoSort)
			// 			SGM.CompleteTransactionOnSG(this);
			// 		else
			// 			SetState(SlotGroup.SortingState);
			// 	}
				
			// 	/*	removal
			// 	*/
			// 	if(removed != null && GetSlottable(removed.Item) != null){
			// 		EquipmentSet focusedEquipSet = (EquipmentSet)SGM.RootPage.EquipBundle.GetFocusedBundleElement();
			// 		if(focusedEquipSet.ContainsElement(this))
			// 			// Inventory.Items.Remove(removed.Item);
			// 			Inventory.RemoveItem(removed.Item);
			// 	}
			// 	/*	addition
			// 	*/
			// 	if(added != null && !Inventory.Items.Contains(added.Item)){
			// 		Inventory.AddItem(added.Item);
			// 	}
			// }
				
			// public void TransactionUpdateV2(Slottable added, Slottable removed){
			// 	/*	add/remove InventoryItem and Slottable
			// 		do not destroy the removed Slottable yet (it remain in situ)
			// 		then sort
			// 		destruction is handled in Sb's removeingProcess's expiration

			// 		if sg is Pool then Inventory addition and removal are exempted
			// 		so are Slottable addition/removal
			// 	*/
			// 	/*	Addition and Removal  */
			// 		Slot swapSlot = null;
			// 		if(SGM.GetFocusedPoolSG() != this){
			// 			/*	remove
			// 			*/
			// 			if(removed != null && Inventory.Items.Contains(removed.Item) && GetSlottable(removed.Item) != null){
			// 				Inventory.RemoveItem(removed.Item);
			// 				Slot slot = GetSlot(removed);
			// 				SGM.removedSB = slot.Sb;
			// 				slot.Sb = null;
			// 				if(added != null)
			// 					swapSlot = slot;
			// 			}
			// 			/*	add
			// 			*/
			// 			if(added != null && !Inventory.Items.Contains(added.Item) && GetSlottable(added.Item) == null){
			// 				Inventory.AddItem(added.Item);
			// 				/*	SB	*/
			// 				GameObject newSBGO = new GameObject("newSBGO");
			// 				Slottable newSB = newSBGO.AddComponent<Slottable>();
			// 				InventoryItemInstanceMock item = (InventoryItemInstanceMock)added.Item;
			// 				/*	slot	*/
			// 				Slot slot = null;
			// 				if(removed != null)
			// 					slot = swapSlot;
			// 				else
			// 					slot = GetNextEmptySlot();
			// 				/*	assemble	*/
			// 				newSB.Initialize(this.SGM, true, item);
			// 				slot.Sb = newSB;
			// 			}
			// 		}
			// 	/*	Sorting  */
							
			// 		if(SGM.Transaction.GetType() == typeof(SortTransaction) || SGM.Transaction.GetType() == typeof(ReorderTransaction)){
			// 			SetState(SlotGroup.SortingState);
			// 		}else{
			// 			if(!m_autoSort){
			// 				SGM.CompleteTransactionOnSG(this);
			// 			}
			// 			else
			// 				SetState(SlotGroup.SortingState);
			// 		}
			// }
	}
}
