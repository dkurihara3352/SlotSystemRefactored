using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public class SlotGroup : MonoBehaviour, SlotSystemElement{
		/*	states	*/
			/*	Engines	*/
				SSEStateEngine SGSelStateEngine{
					get{
						if(m_sgSelStateEngine == null)
							m_sgSelStateEngine = new SSEStateEngine(this);
						return m_sgSelStateEngine;
					}	
					}SSEStateEngine m_sgSelStateEngine;
					public SSEState curSelState{
						get{return (SGSelState)SGSelStateEngine.curState;}
					}
					public SSEState prevSelState{
						get{return (SGSelState)SGSelStateEngine.prevState;}
					}
					public void SetSelState(SSEState state){
						if(state == null || state is SGSelState)
							SGSelStateEngine.SetState(state);
						else
							throw new System.InvalidOperationException("SlotGroup.SetSelState: somthing other than SGSelectionState is trying to be assinged");
					}
					public static SGSelState DeactivatedState{
						get{
							if(SlotGroup.m_deactivatedState == null)
								m_deactivatedState = new SGDeactivatedState();
							return m_deactivatedState;
						}
						}private static SGSelState m_deactivatedState;
					public static SGSelState DefocusedState{
						get{
							if(SlotGroup.m_defocusedState == null)
								m_defocusedState = new SGDefocusedState();
							return m_defocusedState;
						}
						}private static SGSelState m_defocusedState;
					public static SGSelState FocusedState{
						get{
							if(SlotGroup.m_focusedState == null)
								m_focusedState = new SGFocusedState();
							return m_focusedState;
						}
						}private static SGSelState m_focusedState;
					public static SGSelState SelectedState{
						get{
							if(m_selectedState == null)
								m_selectedState = new SGSelectedState();
							return m_selectedState;
						}
						}private static SGSelState m_selectedState;
				SSEStateEngine SGActStateEngine{
					get{
						if(m_sgActStateEngine == null)
							m_sgActStateEngine = new SSEStateEngine(this);
						return m_sgActStateEngine;
					}	
					}SSEStateEngine m_sgActStateEngine;
					public SSEState curActState{
						get{return (SGActState)SGActStateEngine.curState;}
					}
					public SSEState prevActState{
						get{return (SGActState)SGActStateEngine.prevState;}
					}
					public void SetActState(SSEState state){
						if(state == null || state is SGActState)
							SGActStateEngine.SetState(state);
						else
							throw new System.InvalidOperationException("SlotGroup.SetActState: somthing other than SGActionState is trying to be assinged");
					}
					public static SGActState WaitForActionState{
						get{
							if(m_waitForActionState == null)
								m_waitForActionState = new SGWaitForActionState();
							return m_waitForActionState;
						}
						}private static SGActState m_waitForActionState;
					public static SGActState RevertState{
						get{
							if(m_revertState == null)
								m_revertState = new SGRevertState();
							return m_revertState;
						}
						}private static SGActState m_revertState;
					public static SGActState ReorderState{
						get{
							if(m_reorderState == null)
								m_reorderState = new SGReorderState();
							return m_reorderState;
						}
						}private static SGActState m_reorderState;
					public static SGActState AddState{
						get{
							if(m_addState == null)
								m_addState = new SGAddState();
							return m_addState;
						}
						}private static SGActState m_addState;
					public static SGActState RemoveState{
						get{
							if(m_removeState == null)
								m_removeState = new SGRemoveState();
							return m_removeState;
						}
						}private static SGActState m_removeState;
					public static SGActState SwapState{
						get{
							if(m_swapState == null)
								m_swapState = new SGSwapState();
							return m_swapState;
						}
						}private static SGActState m_swapState;
					public static SGActState FillState{
						get{
							if(m_fillState == null)
								m_fillState = new SGFillState();
							return m_fillState;
						}
						}private static SGActState m_fillState;
					public static SGActState SortState{
						get{
							if(m_sortState == null)
								m_sortState = new SGSortState();
							return m_sortState;
						}
						}private static SGActState m_sortState;

			
			/*	process	*/
				public SSEProcessEngine selProcEngine{
					get{
						if(m_selProcEngine == null)
							m_selProcEngine = new SSEProcessEngine();
						return m_selProcEngine;
					}
					}SSEProcessEngine m_selProcEngine;
					public SSEProcess selProcess{
						get{return (SGSelProcess)selProcEngine.process;}
					}
					public void SetAndRunSelProcess(SSEProcess process){
						if(process == null || process is SGSelProcess)
							selProcEngine.SetAndRunProcess(process);
						else throw new System.InvalidOperationException("SlotGroup.SetAndrunSelProcess: argument is not of type SGSelProcess");
					}
					public IEnumeratorMock greyinCoroutine(){
						return null;
					}
					public IEnumeratorMock greyoutCoroutine(){
						return null;
					}
					public IEnumeratorMock highlightCoroutine(){
						return null;
					}
					public IEnumeratorMock dehighlightCoroutine(){
						return null;
					}
				public SSEProcessEngine actProcEngine{
					get{
						if(m_actProcEngine == null)
							m_actProcEngine = new SSEProcessEngine();
						return m_actProcEngine;
					}
					}SSEProcessEngine m_actProcEngine;
					public SSEProcess actProcess{
						get{return (SGActProcess)actProcEngine.process;}
					}
					public void SetAndRunActProcess(SSEProcess process){
						if(process == null || process is SGActProcess)
							actProcEngine.SetAndRunProcess(process);
						else throw new System.InvalidOperationException("SlotGroup.SetAndRunActProcess: argument is not of type SGActProcess");
					}
					public IEnumeratorMock TransactionCoroutine(){
						bool flag = true;
						foreach(Slottable sb in this){
							if(sb != null)
							flag &= !sb.actProcess.isRunning;
						}
						if(flag){
							actProcess.Expire();
						}
						return null;
					}
		/*	public fields	*/
			public AxisScrollerMock scroller{
				get{return m_scroller;}
				set{m_scroller = value;}
				}AxisScrollerMock m_scroller;
			public Inventory inventory{
				get{return m_inventory;}
				}
				Inventory m_inventory;
				public void SetInventory(Inventory inv){
					m_inventory = inv;
					inv.SetSG(this);
				}
			public bool isShrinkable{
				get{return m_isShrinkable;}
				set{m_isShrinkable = value;}
				}bool m_isShrinkable;
			public bool isExpandable{
				get{return m_isExpandable;}
				set{m_isExpandable = value;}
				}bool m_isExpandable;
			public List<Slot> slots{
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
			
			public bool isPool{
				get{
					// return sgm.allSGPs.Contains(this);
					return ((InventoryManagerPage)rootElement).poolBundle.ContainsInHierarchy(this);
				}
			}
			public bool isSGE{
				get{
					// return sgm.allSGEs.Contains(this);
					return ((InventoryManagerPage)rootElement).equipBundle.ContainsInHierarchy(this);
				}
			}
			public bool isSGG{
				get{
					foreach(SlotSystemBundle gBundle in ((InventoryManagerPage)rootElement).otherBundles){
						if(gBundle.ContainsInHierarchy(this))
							return true;
					}
					return false;
				}
			}
			public bool isAutoSort{
				get{return m_autoSort;}
				}bool m_autoSort = true;
				public void ToggleAutoSort(bool on){
					m_autoSort = on;
					sgm.Focus();
				}
			List<Slottable> slottables{
				get{
					// List<Slottable> result = new List<Slottable>();
					// 	foreach(Slot slot in this.slots){
					// 		if(slot.sb != null)
					// 			result.Add(slot.sb);
					// 		else
					// 			result.Add(null);
					// 	}
					// return result;
					return m_slottables;
				}
				}List<Slottable> m_slottables;
				public void SetSBs(List<Slottable> sbs){
					m_slottables = sbs;
				}
			public List<Slottable> newSBs{
				get{return m_newSBs;}
				}List<Slottable> m_newSBs;
				public void SetNewSBs(List<Slottable> sbs){
					m_newSBs = sbs;
				}
			public List<InventoryItemInstanceMock> itemInstances{
				get{
					List<InventoryItemInstanceMock> result = new List<InventoryItemInstanceMock>();
						foreach(Slottable sb in slottables){
							if(sb != null)
								result.Add(sb.itemInst);
							else
								result.Add(null);
						}
					return result;
				}
			}
			public List<InventoryItemInstanceMock> actualItemInsts{
				get{
					List<InventoryItemInstanceMock> result = new List<InventoryItemInstanceMock>();
					foreach(InventoryItemInstanceMock itemInst in itemInstances){
						if(itemInst != null)
							result.Add(itemInst);
					}
					return result;
				}
			}
			public bool isFocusedInBundle{
				get{
					return (sgm.focusedSGP == this || sgm.focusedSGEs.Contains(this));
				}
			}
			public bool hasEmptySlot{
				get{
					bool emptyFound = false;
					foreach(Slot slot in slots){
						if(slot.sb == null)
							emptyFound = true;
					}
					return emptyFound;
				}
			}
			public int actualSBsCount{
				get{
					int count = 0;
					foreach(Slot slot in slots){
						if(slot.sb != null)
							count ++;
					}
					return count;
				}
			}
			public List<Slottable> equippedSBs{
				get{
					List<Slottable> result = new List<Slottable>();
					foreach(Slottable sb in slottables){
						if(sb != null && sb.isEquipped)
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
			public bool isAllTASBsDone{
				get{
					// if(allTASBs != null){
					// }
						foreach(Slottable sb in this){
							if(sb != null){
								if(sb.actProcess.isRunning)
									return false;
							}
						}
					return true;
				}
			}
			public int initSlotsCount{
				get{return m_initSlotsCount;}
				}int m_initSlotsCount;
				public void SetInitSlotsCount(int i){
					m_initSlotsCount = i;
				}
		/*	commands methods	*/
			public void InitializeItems(){
				m_initItemsCommand.Execute(this);
				}SlotGroupCommand m_initItemsCommand = new SGInitItemsCommand();
			public void OnActionComplete(){
				m_onActionCompleteCommand.Execute(this);
				}SlotGroupCommand m_onActionCompleteCommand;
			public void OnActionExecute(){
				m_onActionExecuteCommand.Execute(this);
				}SlotGroupCommand m_onActionExecuteCommand;

			/*	static	commands	*/
				public static SlotGroupCommand updateEquippedStatusCommand{
					get{
						return m_updateEquippedStatusCommand;
					}
					}static SlotGroupCommand m_updateEquippedStatusCommand = new SGUpdateEquipStatusCommand();
				public static SlotGroupCommand emptyCommand{
					get{
						return m_emptyCommand;
					}
					}static SlotGroupCommand m_emptyCommand = new SGEmptyCommand();
				public static SlotGroupCommand updateEquipAtExecutionCommand{
					get{
						return m_updateEquipAtExecutionCommand;
					}
					}static SlotGroupCommand m_updateEquipAtExecutionCommand = new SGUpdateEquipAtExecutionCommand();
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
			public void InstantSort(){
				List<Slottable> orderedSbs = slottables;
				Sorter.OrderSBsWithRetainedSize(ref orderedSbs);
				foreach(Slot slot in slots){
					slot.sb = orderedSbs[slots.IndexOf(slot)];
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
			public bool AcceptsFilter(Slottable pickedSB){
				if(this.Filter is SGNullFilter) return true;
				else{
					if(pickedSB.item is BowInstanceMock)
						return this.Filter is SGBowFilter;
					else if(pickedSB.item is WearInstanceMock)
						return this.Filter is SGWearFilter;
					else if(pickedSB.item is CarriedGearInstanceMock)
						return this.Filter is SGCGearsFilter;
					else// if(pickedSB.Item is PartsInstanceMock)
						return this.Filter is SGPartsFilter;
				}
			}
		/*	events	*/
			public void OnHoverEnterMock(){
				PointerEventDataMock eventData = new PointerEventDataMock();
				((SGSelState)curSelState).OnHoverEnterMock(this, eventData);
			}
			public void OnHoverExitMock(){
				PointerEventDataMock eventData = new PointerEventDataMock();
				((SGSelState)curSelState).OnHoverExitMock(this, eventData);
			}
		/*	SlotSystemElement implementation	*/
			/* fields	*/
				public SlotSystemElement this[int i]{
					get{return slottables[i];}
				}
				public int Count{
					get{return slottables.Count;}
				}
				public string eName{
					get{
						string res = m_eName;
						if(sgm != null){
							if(isPool) res = Util.Red(res);
							if(isSGE) res = Util.Blue(res);
							if(isSGG) res = Util.Green(res);
						}
						return res;
						}
					}string m_eName;
				public List<Slottable> toList{get{return slottables;}}
				public SlotSystemElement parent{
					get{return m_parent;}
					set{m_parent = value;}
					}SlotSystemElement m_parent;
				public SlotSystemBundle immediateBundle{
					get{
						if(parent == null)
							return null;
						if(parent is SlotSystemBundle)
							return (SlotSystemBundle)parent;
						else
							return parent.immediateBundle;
					}
				}
				public SlotGroupManager sgm{
					get{return m_sgm;}
					set{m_sgm = value;}
					}SlotGroupManager m_sgm;

				public int level{
					get{
						if(parent == null) return 0;
						return parent.level + 1;
					}
				}
				public SlotSystemElement rootElement{
					get{return m_rootElement;}
					set{m_rootElement = value;}
					}SlotSystemElement m_rootElement;

			/*	methods	*/
				public IEnumerator<SlotSystemElement> GetEnumerator(){
					foreach(Slottable sb in slottables){
						yield return sb;
					}
					}IEnumerator IEnumerable.GetEnumerator(){
						return GetEnumerator();
					}
				public int IndexOf(Slottable sb){
					return slottables.IndexOf(sb);
				}
				public bool Contains(SlotSystemElement element){
					if(element is Slottable)
						return slottables.Contains((Slottable)element);
					return false;
				}
				public bool ContainsInHierarchy(SlotSystemElement element){
					return FindParentInHierarchy(element) != null;
				}
				public SlotSystemElement FindParentInHierarchy(SlotSystemElement element){
					if(element is Slottable){
						Slottable tarSB = (Slottable)element;
						foreach(Slottable sb in slottables){
							if(sb != null){
								if(sb == tarSB)
									return this;
							}
						}
					}
					return null;
				}
				public void Focus(){
					SetSelState(SlotGroup.FocusedState);
					FocusSBs();
					Reset();
				}
				public void FocusSelf(){
					SetSelState(SlotGroup.FocusedState);
				}
				public void FocusSBs(){
					foreach(Slottable sb in slottables){
						if(sb != null){
							sb.SetActState(Slottable.WaitForActionState);
							sb.Reset();
							if(sb.isPickable)
								sb.Focus();
							else
								sb.Defocus();
						}
					}
				}
				public void Defocus(){
					SetSelState(SlotGroup.DefocusedState);
					DefocusSBs();
					Reset();
				}
				public void DefocusSelf(){
					SetSelState(SlotGroup.DefocusedState);
				}
				public void DefocusSBs(){
					foreach(Slottable sb in slottables){
						if(sb != null){
							sb.SetActState(Slottable.WaitForActionState);
							sb.Reset();
							sb.Defocus();
						}
					}
				}
				public void Activate(){
				}
				public void Deactivate(){
					SetSelState(SlotGroup.DeactivatedState);
					foreach(Slottable sb in slottables){
						if(sb != null){
							sb.SetSelState(Slottable.DeactivatedState);
						}
					}
				}
				public void PerformInHierarchy(System.Action<SlotSystemElement> act){
					act(this);
					foreach(Slottable sb in this){
						if(sb != null)
							sb.PerformInHierarchy(act);
					}
				}
				public void PerformInHierarchy(System.Action<SlotSystemElement, object> act, object obj){
					act(this, obj);
					foreach(Slottable sb in slottables){
						if(sb != null)
							sb.PerformInHierarchy(act, obj);
					}
				}
				public void PerformInHierarchy<T>(System.Action<SlotSystemElement, IList<T>> act, IList<T> list){
					act(this, list);
					foreach(Slottable sb in slottables){
						if(sb != null)
							sb.PerformInHierarchy<T>(act, list);
					}
				}
				public void InstantGreyout(){}
				public void InstantGreyin(){}
				public void InstantHighlight(){}
		/*	methods	*/
			public void Initialize(string name, SGFilter filter, Inventory inv, bool isShrinkable, int initSlotsCount, SlotGroupCommand onActionCompleteCommand, SlotGroupCommand onActionExecuteCommand){
				m_eName = name;
				SetFilter(filter);
				SetSorter(SlotGroup.ItemIDSorter);
				SetInventory(inv);
				m_onActionCompleteCommand = onActionCompleteCommand;
				m_onActionExecuteCommand = onActionExecuteCommand;
				this.isShrinkable = isShrinkable;
				if(initSlotsCount == 0)
					this.isExpandable = true;
				else
					this.isExpandable = false;
				m_initSlotsCount = initSlotsCount;
				InitializeItems();			
				SetSelState(SlotGroup.DeactivatedState);
				SetActState(SlotGroup.WaitForActionState);
			}
			public Slottable GetSB(InventoryItemInstanceMock itemInst){
				foreach(Slottable sb in this.slottables){
					if(sb != null){
						if(sb.itemInst == itemInst)
							return sb;
					}
				}
				if(allTASBs != null){
					foreach(Slottable sb in allTASBs){
						if(sb != null){
							if(sb.itemInst == itemInst)
								return sb;
						}
					}
				}
				return null;
			}
			public bool HasItem(InventoryItemInstanceMock invInst){
				bool result = false;
				foreach(Slottable sb in this.slottables){
					if(sb != null){
						if(sb.itemInst == invInst)
							return true;
					}
				}
				return result;
			}
			// public Slot GetSlot(InventoryItemInstanceMock itemInst){
				// 	foreach(Slot slot in this.slots){
				// 		if(slot.sb != null){
				// 			if(slot.sb.itemInst == itemInst)
				// 				return slot;
				// 		}
				// 	}
				// 	return null;
				// }
			public void UpdateSBs(List<Slottable> newSBs){
				/*	Create and set new Slots	*/
					List<Slot> newSlots = new List<Slot>();
					for(int i = 0; i < newSBs.Count; i++){
						Slot newSlot = new Slot();
						newSlots.Add(newSlot);
					}
					SetNewSlots(newSlots);
				/*	Set SBs act states	*/
				List<Slottable> moveWithins = new List<Slottable>();
				List<Slottable> removed = new List<Slottable>();
				List<Slottable> added = new List<Slottable>();
				foreach(Slottable sb in slottables){
					if(sb != null){
						if(newSBs.Contains(sb))
							moveWithins.Add(sb);
						else
							removed.Add(sb);
					}
				}
				foreach(Slottable sb in newSBs){
					if(sb != null){
						if(!slottables.Contains(sb))
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
				allSBs.AddRange(slottables);
				allSBs.AddRange(added);
				// SetAllTASBs(allSBs);
				SetSBs(allSBs);
			}
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
				foreach(Slottable sb in this){
					if(sb != null){
						if(sb.itemInst == itemInst)
							index = sb.newSlotID;
					}
				}
				return newSlots[index];
			}
			public void SetSBsActStates(){
				List<Slottable> moveWithins = new List<Slottable>();
				List<Slottable> removed = new List<Slottable>();
				List<Slottable> added = new List<Slottable>();
				foreach(Slottable sb in slottables){
					if(sb != null){
						if(newSBs.Contains(sb))
							moveWithins.Add(sb);
						else
							removed.Add(sb);
					}
				}
				foreach(Slottable sb in newSBs){
					if(sb != null){
						if(!slottables.Contains(sb))
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
				allSBs.AddRange(slottables);
				allSBs.AddRange(added);
				SetAllTASBs(allSBs);
			}
			public void CheckProcessCompletion(){
				TransactionCoroutine();
			}
			public void OnCompleteSlotMovements(){
				foreach(Slottable sb in this){
					if(sb != null){
						if(sb.newSlotID == -1){
							GameObject sbGO = sb.gameObject;
							DestroyImmediate(sbGO);
							DestroyImmediate(sb);
						}else{
							newSlots[sb.newSlotID].sb = sb;
						}
					}
				}
				SetSlots(newSlots);
				SyncSBsToSlots();
			}
			public void SyncSBsToSlots(){
				List<Slottable> newSBs = new List<Slottable>();
				foreach(Slot slot in slots){
					newSBs.Add(slot.sb);
				}
				SetSBs(newSBs);
				foreach(Slottable sb in this){
					if(sb != null)
					sb.SetSlotID(newSBs.IndexOf(sb));
				}
			}
			public List<Slottable> SwappableSBs(Slottable pickedSB){
				List<Slottable> result = new List<Slottable>();
				foreach(Slottable sb in slottables){
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
	}
}
