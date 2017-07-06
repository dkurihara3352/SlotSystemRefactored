using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public class SlotGroup : AbsSlotSystemElement{
		/*	states	*/
			/*	Engines	*/
				/*	Selection State	*/
					public override SSEState curSelState{
						get{return (SGSelState)selStateEngine.curState;}
					}
					public override SSEState prevSelState{
						get{return (SGSelState)selStateEngine.prevState;}
					}
					public override void SetSelState(SSEState state){
						if(state == null || state is SGSelState)
							selStateEngine.SetState(state);
						else
							throw new System.InvalidOperationException("SlotGroup.SetSelState: somthing other than SGSelectionState is trying to be assinged");
					}
					public static SGSelState sgDeactivatedState{
						get{
							if(SlotGroup.m_sgDeactivatedState == null)
								m_sgDeactivatedState = new SGDeactivatedState();
							return m_sgDeactivatedState;
						}
						}private static SGSelState m_sgDeactivatedState;
					public static SGSelState sgDefocusedState{
						get{
							if(SlotGroup.m_sgDefocusedState == null)
								m_sgDefocusedState = new SGDefocusedState();
							return m_sgDefocusedState;
						}
						}private static SGSelState m_sgDefocusedState;
					public static SGSelState sgFocusedState{
						get{
							if(SlotGroup.m_sgFocusedState == null)
								m_sgFocusedState = new SGFocusedState();
							return m_sgFocusedState;
						}
						}private static SGSelState m_sgFocusedState;
					public static SGSelState sgSelectedState{
						get{
							if(m_sgSelectedState == null)
								m_sgSelectedState = new SGSelectedState();
							return m_sgSelectedState;
						}
						}private static SGSelState m_sgSelectedState;
				
				/*	Action State	*/
					public override SSEState curActState{
						get{return (SGActState)actStateEngine.curState;}
					}
					public override SSEState prevActState{
						get{return (SGActState)actStateEngine.prevState;}
					}
					public override void SetActState(SSEState state){
						if(state == null || state is SGActState)
							actStateEngine.SetState(state);
						else
							throw new System.InvalidOperationException("SlotGroup.SetActState: somthing other than SGActionState is trying to be assinged");
					}
					public static SGActState sgWaitForActionState{
						get{
							if(m_sgWaitForActionState == null)
								m_sgWaitForActionState = new SGWaitForActionState();
							return m_sgWaitForActionState;
						}
						}private static SGActState m_sgWaitForActionState;
					public static SGActState revertState{
						get{
							if(m_revertState == null)
								m_revertState = new SGRevertState();
							return m_revertState;
						}
						}private static SGActState m_revertState;
					public static SGActState reorderState{
						get{
							if(m_reorderState == null)
								m_reorderState = new SGReorderState();
							return m_reorderState;
						}
						}private static SGActState m_reorderState;
					public static SGActState addState{
						get{
							if(m_addState == null)
								m_addState = new SGAddState();
							return m_addState;
						}
						}private static SGActState m_addState;
					public static SGActState removeState{
						get{
							if(m_removeState == null)
								m_removeState = new SGRemoveState();
							return m_removeState;
						}
						}private static SGActState m_removeState;
					public static SGActState swapState{
						get{
							if(m_swapState == null)
								m_swapState = new SGSwapState();
							return m_swapState;
						}
						}private static SGActState m_swapState;
					public static SGActState fillState{
						get{
							if(m_fillState == null)
								m_fillState = new SGFillState();
							return m_fillState;
						}
						}private static SGActState m_fillState;
					public static SGActState sortState{
						get{
							if(m_sortState == null)
								m_sortState = new SGSortState();
							return m_sortState;
						}
						}private static SGActState m_sortState;			
			/*	process	*/
				/*	Selection Process	*/
					public override SSEProcess selProcess{
						get{return (SGSelProcess)selProcEngine.process;}
					}
					public override void SetAndRunSelProcess(SSEProcess process){
						if(process == null || process is SGSelProcess)
							selProcEngine.SetAndRunProcess(process);
						else throw new System.InvalidOperationException("SlotGroup.SetAndrunSelProcess: argument is not of type SGSelProcess");
					}
					public override IEnumeratorFake greyinCoroutine(){
						return null;
					}
					public override IEnumeratorFake greyoutCoroutine(){
						return null;
					}
					public override IEnumeratorFake highlightCoroutine(){
						return null;
					}
					public override IEnumeratorFake dehighlightCoroutine(){
						return null;
					}
				/*	Action Process	*/
					public override SSEProcess actProcess{
						get{return (SGActProcess)actProcEngine.process;}
					}
					public override void SetAndRunActProcess(SSEProcess process){
						if(process == null || process is SGActProcess)
							actProcEngine.SetAndRunProcess(process);
						else throw new System.InvalidOperationException("SlotGroup.SetAndRunActProcess: argument is not of type SGActProcess");
					}
					public IEnumeratorFake TransactionCoroutine(){
						bool flag = true;
						foreach(Slottable sb in slottables){
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
			
			public virtual bool isPool{
				get{
					return ssm.poolBundle.ContainsInHierarchy(this);
				}
			}
			public virtual bool isSGE{
				get{
					return ssm.equipBundle.ContainsInHierarchy(this);
				}
			}
			public virtual bool isSGG{
				get{
					foreach(SlotSystemBundle gBundle in ssm.otherBundles){
						if(gBundle.ContainsInHierarchy(this))
							return true;
					}
					return false;
				}
			}
			public bool isAutoSort{
				get{return m_isAutoSort;}
				}protected bool m_isAutoSort = true;
				public void ToggleAutoSort(bool on){
					m_isAutoSort = on;
					ssm.Focus();
				}
			List<Slottable> slottables{
				get{
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
			public List<InventoryItemInstance> itemInstances{
				get{
					List<InventoryItemInstance> result = new List<InventoryItemInstance>();
						foreach(Slottable sb in slottables){
							if(sb != null)
								result.Add(sb.itemInst);
							else
								result.Add(null);
						}
					return result;
				}
			}
			public List<InventoryItemInstance> actualItemInsts{
				get{
					List<InventoryItemInstance> result = new List<InventoryItemInstance>();
					foreach(InventoryItemInstance itemInst in itemInstances){
						if(itemInst != null)
							result.Add(itemInst);
					}
					return result;
				}
			}
			public virtual bool isFocusedInBundle{
				get{
					return (ssm.focusedSGP == this || ssm.focusedSGEs.Contains(this));
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
			public virtual List<Slottable> equippedSBs{
				get{
					List<Slottable> result = new List<Slottable>();
					foreach(Slottable sb in slottables){
						if(sb != null && sb.isEquipped)
							result.Add(sb);
					}
					return result;
				}
			}
			public virtual bool isAllTASBsDone{
				get{
					foreach(Slottable sb in slottables){
						if(sb != null){
							if(sb.actProcess  != null)
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

			public override bool isFocused{
				get{return curSelState == SlotGroup.sgFocusedState;}
			}
			public override bool isDefocused{
				get{return curSelState == SlotGroup.sgDefocusedState;}
			}
			public override bool isDeactivated{
				get{return curSelState == SlotGroup.sgDeactivatedState;}
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
					else
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
				public override SlotSystemElement this[int i]{
					get{return slottables[i];}
				}
				protected override IEnumerable<SlotSystemElement> elements{
					get{
						foreach(Slottable sb in slottables){
							yield return (SlotSystemElement)sb;
						}
					}
				}
				public int Count{
					get{return slottables.Count;}
				}
				public override string eName{
					get{
						string res = m_eName;
						if(ssm != null){
							if(isPool) res = Util.Red(res);
							if(isSGE) res = Util.Blue(res);
							if(isSGG) res = Util.Green(res);
						}
						return res;
					}
				}
				public List<Slottable> toList{get{return slottables;}}

			/*	methods	*/
				public int IndexOf(Slottable sb){
					return slottables.IndexOf(sb);
				}
				public override bool Contains(SlotSystemElement element){
					if(element is Slottable)
						return slottables.Contains((Slottable)element);
					return false;
				}
				public override void Focus(){
					SetSelState(SlotGroup.sgFocusedState);
					FocusSBs();
					Reset();
				}
				public void FocusSelf(){
					SetSelState(SlotGroup.sgFocusedState);
				}
				public void FocusSBs(){
					foreach(Slottable sb in slottables){
						if(sb != null){
							sb.SetActState(Slottable.sbWaitForActionState);
							sb.Reset();
							if(sb.passesPrePickFilter)
								sb.Focus();
							else
								sb.Defocus();
						}
					}
				}
				public override void Defocus(){
					SetSelState(SlotGroup.sgDefocusedState);
					DefocusSBs();
					Reset();
				}
				public void DefocusSelf(){
					SetSelState(SlotGroup.sgDefocusedState);
				}
				public void DefocusSBs(){
					foreach(Slottable sb in slottables){
						if(sb != null){
							sb.SetActState(Slottable.sbWaitForActionState);
							sb.Reset();
							sb.Defocus();
						}
					}
				}
				public override void Deactivate(){
					SetSelState(SlotGroup.sgDeactivatedState);
					foreach(Slottable sb in slottables){
						if(sb != null){
							sb.Deactivate();
						}
					}
				}
				public override void PerformInHierarchy(System.Action<SlotSystemElement> act){
					act(this);
					foreach(Slottable sb in slottables){
						if(sb != null)
							sb.PerformInHierarchy(act);
					}
				}
				public override void PerformInHierarchy(System.Action<SlotSystemElement, object> act, object obj){
					act(this, obj);
					foreach(Slottable sb in slottables){
						if(sb != null)
							sb.PerformInHierarchy(act, obj);
					}
				}
				public override void PerformInHierarchy<T>(System.Action<SlotSystemElement, IList<T>> act, IList<T> list){
					act(this, list);
					foreach(Slottable sb in slottables){
						if(sb != null)
							sb.PerformInHierarchy<T>(act, list);
					}
				}
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
				SetSelState(SlotGroup.sgDeactivatedState);
				SetActState(SlotGroup.sgWaitForActionState);
			}
			public Slottable GetSB(InventoryItemInstance itemInst){
				foreach(Slottable sb in slottables){
					if(sb != null){
						if(sb.itemInst == itemInst)
							return sb;
					}
				}
				return null;
			}
			public bool HasItem(InventoryItemInstance invInst){
				bool result = false;
				foreach(Slottable sb in slottables){
					if(sb != null){
						if(sb.itemInst == invInst)
							return true;
					}
				}
				return result;
			}
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
					sb.SetActState(Slottable.moveWithinState);
				}
				foreach(Slottable sb in removed){
					sb.SetNewSlotID(-1);
					sb.SetActState(Slottable.removedState);
				}
				foreach(Slottable sb in added){
					sb.SetNewSlotID(newSBs.IndexOf(sb));
					sb.SetActState(Slottable.addedState);
				}
				List<Slottable> allSBs = new List<Slottable>();
				allSBs.AddRange(slottables);
				allSBs.AddRange(added);
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
			public Slot GetNewSlot(InventoryItemInstance itemInst){
				int index = -3;
				foreach(Slottable sb in slottables){
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
					sb.SetActState(Slottable.moveWithinState);
				}
				foreach(Slottable sb in removed){
					sb.SetNewSlotID(-1);
					sb.SetActState(Slottable.removedState);
				}
				foreach(Slottable sb in added){
					sb.SetNewSlotID(newSBs.IndexOf(sb));
					sb.SetActState(Slottable.addedState);
				}
			}
			public void CheckProcessCompletion(){
				TransactionCoroutine();
			}
			public void OnCompleteSlotMovements(){
				foreach(Slottable sb in slottables){
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
				foreach(Slottable sb in slottables){
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
				SetActState(SlotGroup.sgWaitForActionState);
				SetNewSBs(null);
				SetNewSlots(null);
			}
	}
}
