using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public class SlotGroup : SlotSystemElement, ISlotGroup{
		/*	states	*/
			/*	Engines	*/
				/*	Action State	*/
					public virtual ISSEStateEngine<ISGActState> actStateEngine{
						get{
							if(m_actStateEngine == null)
								m_actStateEngine = new SSEStateEngine<ISGActState>(this);
							return m_actStateEngine;
						}
						}ISSEStateEngine<ISGActState> m_actStateEngine;
					public virtual void SetActStateEngine(ISSEStateEngine<ISGActState> engine){m_actStateEngine = engine;}
					public virtual ISGActState curActState{
						get{return actStateEngine.curState;}
					}
					public virtual ISGActState prevActState{
						get{return actStateEngine.prevState;}
					}
					public virtual void SetActState(ISGActState state){
						actStateEngine.SetState(state);
					}
					/* Static states */
						public static ISGActState sgWaitForActionState{
							get{
								if(m_sgWaitForActionState == null)
									m_sgWaitForActionState = new SGWaitForActionState();
								return m_sgWaitForActionState;
							}
							}private static ISGActState m_sgWaitForActionState;
						public static ISGActState revertState{
							get{
								if(m_revertState == null)
									m_revertState = new SGRevertState();
								return m_revertState;
							}
							}private static ISGActState m_revertState;
						public static ISGActState reorderState{
							get{
								if(m_reorderState == null)
									m_reorderState = new SGReorderState();
								return m_reorderState;
							}
							}private static ISGActState m_reorderState;
						public static ISGActState addState{
							get{
								if(m_addState == null)
									m_addState = new SGAddState();
								return m_addState;
							}
							}private static ISGActState m_addState;
						public static ISGActState removeState{
							get{
								if(m_removeState == null)
									m_removeState = new SGRemoveState();
								return m_removeState;
							}
							}private static ISGActState m_removeState;
						public static ISGActState swapState{
							get{
								if(m_swapState == null)
									m_swapState = new SGSwapState();
								return m_swapState;
							}
							}private static ISGActState m_swapState;
						public static ISGActState fillState{
							get{
								if(m_fillState == null)
									m_fillState = new SGFillState();
								return m_fillState;
							}
							}private static ISGActState m_fillState;
						public static ISGActState sortState{
						get{
							if(m_sortState == null)
								m_sortState = new SGSortState();
							return m_sortState;
						}
						}private static ISGActState m_sortState;			
		/*	process	*/
			/*	Selection Process	*/
				/* Coroutine */
					public override IEnumeratorFake deactivateCoroutine(){return null;}
					public override IEnumeratorFake focusCoroutine(){return null;}
					public override IEnumeratorFake defocusCoroutine(){return null;}
					public override IEnumeratorFake selectCoroutine(){return null;}
			/*	Action Process	*/
				public virtual ISSEProcessEngine<ISGActProcess> actProcEngine{
					get{
						if(m_actProcEngine == null)
							m_actProcEngine = new SSEProcessEngine<ISGActProcess>();
						return m_actProcEngine;
					}
					}ISSEProcessEngine<ISGActProcess> m_actProcEngine;
				public virtual void SetActProcEngine(ISSEProcessEngine<ISGActProcess> engine){m_actProcEngine = engine;}
				public virtual ISGActProcess actProcess{
					get{return actProcEngine.process;}
				}
				public virtual void SetAndRunActProcess(ISGActProcess process){
					actProcEngine.SetAndRunProcess(process);
				}
				/* Coroutine */
					public IEnumeratorFake TransactionCoroutine(){
						bool flag = true;
						foreach(ISlottable sb in this){
							if(sb != null)
							flag &= !sb.actProcess.isRunning;
						}
						if(flag){
							actProcess.Expire();
						}
						return null;
					}
		/*	public fields	*/
			public virtual AxisScrollerMock scroller{
				get{return m_scroller;}
				set{m_scroller = value;}
				}AxisScrollerMock m_scroller;
			public virtual Inventory inventory{
				get{return m_inventory;}
				}
				Inventory m_inventory;
				public virtual void SetInventory(Inventory inv){
					m_inventory = inv;
					inv.SetSG(this);
				}
			public virtual bool isShrinkable{get{return m_isShrinkable;}} bool m_isShrinkable;
			public virtual bool isExpandable{get{return m_isExpandable;} }bool m_isExpandable;
			public virtual List<Slot> slots{
				get{
					if(m_slots == null)
						m_slots = new List<Slot>();
					return m_slots;}
				}List<Slot> m_slots;
				public virtual void SetSlots(List<Slot> slots){
					m_slots = slots;
				}
			public virtual List<Slot> newSlots{
				get{return m_newSlots;}
				}List<Slot> m_newSlots;
				public virtual void SetNewSlots(List<Slot> newSlots){
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
					foreach(ISlotSystemBundle gBundle in ssm.otherBundles){
						if(gBundle.ContainsInHierarchy(this))
							return true;
					}
					return false;
				}
			}
			public virtual bool isAutoSort{
				get{return m_isAutoSort;}
				}protected bool m_isAutoSort = true;
				public void ToggleAutoSort(bool on){
					m_isAutoSort = on;
					ssm.Focus();
				}
			List<ISlottable> slottables{
				get{
					return m_slottables;
				}
				}protected List<ISlottable> m_slottables;
				public virtual void SetSBs(List<ISlottable> sbs){
					m_slottables = sbs;
				}
			public virtual List<ISlottable> newSBs{
				get{return m_newSBs;}
				}List<ISlottable> m_newSBs;
				public virtual void SetNewSBs(List<ISlottable> sbs){
					m_newSBs = sbs;
				}
			public virtual bool hasEmptySlot{
				get{
					bool emptyFound = false;
					foreach(Slot slot in slots){
						if(slot.sb == null)
							emptyFound = true;
					}
					return emptyFound;
				}
			}
			public virtual List<ISlottable> equippedSBs{
				get{
					List<ISlottable> result = new List<ISlottable>();
					foreach(ISlottable sb in this){
						if(sb != null && sb.isEquipped)
							result.Add(sb);
					}
					return result;
				}
			}
			public virtual bool isAllSBActProcDone{
				get{
					foreach(ISlottable sb in this){
						if(sb != null){
							if(sb.actProcess  != null)
								if(sb.actProcess.isRunning)
									return false;
						}
					}
					return true;
				}
			}
			public virtual int initSlotsCount{
				get{return m_initSlotsCount;}
				}int m_initSlotsCount;
				public virtual void SetInitSlotsCount(int i){
					m_initSlotsCount = i;
				}

			
		/*	commands methods	*/
			public virtual void InitializeItems(){
				initItemsCommand.Execute(this);
				}
				public SlotGroupCommand initItemsCommand{get{return m_initItemsCommand;}} SlotGroupCommand m_initItemsCommand = new SGInitItemsCommand();
				public void SetInitItemsCommand(ISGInitItemsCommand comm){
					m_initItemsCommand = comm;
				}
			public virtual void OnActionComplete(){
				onActionCompleteCommand.Execute(this);
				}
				public SlotGroupCommand onActionCompleteCommand{get{return m_onActionCompleteCommand;}} SlotGroupCommand m_onActionCompleteCommand;
			public virtual void OnActionExecute(){
				onActionExecuteCommand.Execute(this);
				}
				public SlotGroupCommand onActionExecuteCommand{get{return m_onActionExecuteCommand;}} SlotGroupCommand m_onActionExecuteCommand;

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
			
			public virtual SGSorter sorter{
				get{return m_sorter;}
				}SGSorter m_sorter;
				public virtual void SetSorter(SGSorter sorter){
					m_sorter = sorter;
				}
			public virtual void InstantSort(){
				List<ISlottable> orderedSbs = slottables;
				sorter.OrderSBsWithRetainedSize(ref orderedSbs);
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
			public virtual SGFilter filter{
				get{return m_filter;}
				}SGFilter m_filter;
				public virtual void SetFilter(SGFilter filter){
					m_filter = filter;
				}
			public virtual bool AcceptsFilter(ISlottable pickedSB){
				if(this.filter is SGNullFilter) return true;
				else{
					if(pickedSB.itemInst is BowInstance)
						return this.filter is SGBowFilter;
					else if(pickedSB.itemInst is WearInstance)
						return this.filter is SGWearFilter;
					else if(pickedSB.itemInst is CarriedGearInstance)
						return this.filter is SGCGearsFilter;
					else
						return this.filter is SGPartsFilter;
				}
			}
		/*	events	*/
			public virtual void OnHoverEnterMock(){
				PointerEventDataFake eventData = new PointerEventDataFake();
				curSelState.OnHoverEnterMock(this, eventData);
			}
			public virtual void OnHoverExitMock(){
				PointerEventDataFake eventData = new PointerEventDataFake();
				curSelState.OnHoverExitMock(this, eventData);
			}
		/*	SlotSystemElement implementation	*/
			/* fields	*/
				public override ISlotSystemElement this[int i]{
					get{return slottables[i];}
				}
				public override IEnumerable<ISlotSystemElement> elements{
					get{
						foreach(ISlottable sb in slottables){
							yield return (ISlotSystemElement)sb;
						}
					}
				}
				public override string eName{
					get{
						string res = m_eName;
						if(ssm != null){
							if(isPool) res = SlotSystemUtil.Red(res);
							if(isSGE) res = SlotSystemUtil.Blue(res);
							if(isSGG) res = SlotSystemUtil.Green(res);
						}
						return res;
					}
				}
				public virtual List<ISlottable> toList{get{return slottables;}}

			/*	methods	*/

				public override bool Contains(ISlotSystemElement element){
					if(element is ISlottable)
						return slottables.Contains((ISlottable)element);
					return false;
				}
				public override void Focus(){
					FocusSelf();
					FocusSBs();
					Reset();
				}
				public virtual void FocusSelf(){
					SetSelState(SlotSystemElement.focusedState);
				}
				public virtual void FocusSBs(){
					foreach(ISlottable sb in this){
						if(sb != null){
							sb.Reset();
							if(sb.passesPrePickFilter)
								sb.Focus();
							else
								sb.Defocus();
						}
					}
				}
				public override void Defocus(){
					DefocusSelf();
					DefocusSBs();
					Reset();
				}
				public virtual void DefocusSelf(){
					SetSelState(SlotSystemElement.defocusedState);
				}
				public virtual void DefocusSBs(){
					foreach(ISlottable sb in this){
						if(sb != null){
							sb.Reset();
							sb.Defocus();
						}
					}
				}
				public override void Deactivate(){
					SetSelState(SlotSystemElement.deactivatedState);
					foreach(ISlottable sb in this){
						if(sb != null){
							sb.Deactivate();
						}
					}
				}
				public override void PerformInHierarchy(System.Action<ISlotSystemElement> act){
					act(this);
					foreach(ISlottable sb in this){
						if(sb != null) act(sb);
					}
				}
				public override void PerformInHierarchy(System.Action<ISlotSystemElement, object> act, object obj){
					act(this, obj);
					foreach(ISlottable sb in this){
						if(sb != null) act(sb, obj);
					}
				}
				public override void PerformInHierarchy<T>(System.Action<ISlotSystemElement, IList<T>> act, IList<T> list){
					act(this, list);
					foreach(ISlottable sb in this){
						if(sb != null) act(sb, list);
					}
				}
		/*	methods	*/
			public override void InitializeStates(){
				SetSelState(SlotSystemElement.deactivatedState);
				SetActState(SlotGroup.sgWaitForActionState);
			}
			public void InspectorSetUp(Inventory inv, SGFilter filter, SGSorter sorter, int initSlotsCount){
				SetInventory(inv);
				SetFilter(filter);
				SetSorter(sorter);
				SetInitSlotsCount(initSlotsCount);
				m_isExpandable = initSlotsCount == 0;
			}
			public override void SetElements(){
				InitializeItems();
			}
			public virtual ISlottable GetSB(InventoryItemInstance itemInst){
				foreach(ISlottable sb in this){
					if(sb != null){
						if(sb.itemInst == itemInst)
							return sb;
					}
				}
				return null;
			}
			public virtual bool HasItem(InventoryItemInstance invInst){
				bool result = false;
				foreach(ISlottable sb in this){
					if(sb != null){
						if(sb.itemInst == invInst)
							return true;
					}
				}
				return result;
			}
			public virtual void UpdateSBs(List<ISlottable> newSBs){
				SetNewSBs(newSBs);
				CreateNewSlots();
				SetSBsActStates();
				List<ISlottable> allSBs = new List<ISlottable>(slottables);
					List<ISlottable> added = new List<ISlottable>();
					foreach(var sb in newSBs)
						if(!slottables.Contains(sb)) added.Add(sb);
					allSBs.AddRange(added);
				SetSBs(allSBs);
			}
			public virtual void CreateNewSlots(){
				List<Slot> newSlots = new List<Slot>();
				for(int i = 0; i < newSBs.Count; i++){
					Slot newSlot = new Slot();
					newSlots.Add(newSlot);
				}
				SetNewSlots(newSlots);
			}
			public virtual Slot GetNewSlot(InventoryItemInstance itemInst){
				int index = -3;
				foreach(ISlottable sb in this){
					if(sb != null){
						if(sb.itemInst == itemInst)
							index = sb.newSlotID;
					}
				}
				return newSlots[index];
			}
			public virtual void SetSBsActStates(){
				List<ISlottable> moveWithins = new List<ISlottable>();
				List<ISlottable> removed = new List<ISlottable>();
				List<ISlottable> added = new List<ISlottable>();
				foreach(ISlottable sb in this){
					if(sb != null){
						if(newSBs.Contains(sb))
							moveWithins.Add(sb);
						else
							removed.Add(sb);
					}
				}
				foreach(ISlottable sb in newSBs){
					if(sb != null){
						if(!slottables.Contains(sb))
							added.Add(sb);
					}
				}
				foreach(ISlottable sb in moveWithins){
					sb.SetNewSlotID(newSBs.IndexOf(sb));
					sb.SetActState(Slottable.moveWithinState);
				}
				foreach(ISlottable sb in removed){
					sb.SetNewSlotID(-1);
					sb.SetActState(Slottable.removedState);
				}
				foreach(ISlottable sb in added){
					sb.SetNewSlotID(newSBs.IndexOf(sb));
					sb.SetActState(Slottable.addedState);
				}
			}
			public virtual void OnCompleteSlotMovements(){
				foreach(ISlottable sb in this){
					if(sb != null){
						if(sb.isRemoved){
							sb.Destroy();
						}else{
							newSlots[sb.newSlotID].sb = sb;
						}
					}
				}
				SetSlots(newSlots);
				SyncSBsToSlots();
			}
			public virtual void SyncSBsToSlots(){
				List<ISlottable> newSBs = new List<ISlottable>();
				foreach(Slot slot in slots){
					newSBs.Add(slot.sb);
				}
				SetSBs(newSBs);
				foreach(ISlottable sb in this){
					if(sb != null)
					sb.SetSlotID(newSBs.IndexOf(sb));
				}
			}
			public virtual List<ISlottable> SwappableSBs(ISlottable pickedSB){
				List<ISlottable> result = new List<ISlottable>();
				foreach(ISlottable sb in this){
					if(sb != null){
						if(SlotSystemUtil.AreSwappable(pickedSB, sb))
							result.Add(sb);
					}
				}
				return result;
			}
			public virtual void Reset(){
				SetActState(SlotGroup.sgWaitForActionState);
				SetNewSBs(null);
				SetNewSlots(null);
			}
			public virtual void ReorderAndUpdateSBs(){
				ISlottable sb1 = pickedSB;
				ISlottable sb2 = targetSB;
				List<ISlottable> newSBs = new List<ISlottable>(toList);
				newSBs.Reorder(sb1, sb2);
				UpdateSBs(newSBs);
			}
			public virtual void UpdateToRevert(){
				SetNewSBs(toList);
				CreateNewSlots();
				SetSBsActStates();
			}
			public virtual void SortAndUpdateSBs(){
				List<ISlottable> newSBs = new List<ISlottable>(toList);
				if(isExpandable)
					sorter.TrimAndOrderSBs(ref newSBs);
				else
					sorter.OrderSBsWithRetainedSize(ref newSBs);
				
				UpdateSBs(newSBs);
			}
			public virtual void FillAndUpdateSBs(){
				ISlottable added = GetAddedForFill();
				ISlottable removed = GetRemovedForFill();

				List<ISlottable> newSBs = new List<ISlottable>(toList);

				if(!isPool){
					if(added != null)
						CreateNewSBAndFill(added.itemInst, newSBs);
					if(removed != null)
						NullifyIndexOf(removed.itemInst, newSBs);
				}
				SortContextually(ref newSBs);
				UpdateSBs(newSBs);
			}
				public ISlottable GetAddedForFill(){
					ISlottable added;
					if(ssm.sg1 == (ISlotGroup)this)
						added = null;
					else
						added = ssm.pickedSB;
					return added;
				}
				public ISlottable GetRemovedForFill(){
					ISlottable removed;
					if(ssm.sg1 == (ISlotGroup)this)
						removed = ssm.pickedSB;
					else
						removed = null;
					return removed;
				}
				public void CreateNewSBAndFill(InventoryItemInstance addedItem, List<ISlottable> list){
					GameObject newSBGO = new GameObject("newSBGO");
					ISlottable newSB = newSBGO.AddComponent<Slottable>();
					newSB.SetItem(addedItem);
					newSB.SetSSM(ssm);
					newSB.Defocus();
					newSB.SetEqpState(Slottable.unequippedState);
					list.Fill(newSB);
				}
				public void NullifyIndexOf(InventoryItemInstance removedItem, List<ISlottable> list){
					ISlottable rem = null;
					foreach(ISlottable sb in list){
						if(sb != null){
							if(sb.itemInst == removedItem)
								rem = sb;
						}
					}
					list[list.IndexOf(rem)] = null;
				}
				public void SortContextually(ref List<ISlottable> list){
					if(isAutoSort){
						if(isExpandable)
							sorter.TrimAndOrderSBs(ref list);
						else
							sorter.OrderSBsWithRetainedSize(ref list);
					}
				}
			public virtual void SwapAndUpdateSBs(){
				ISlottable added = GetAddedForSwap();
				ISlottable removed = GetRemovedForSwap();
				List<ISlottable> newSBs = new List<ISlottable>(toList);
				
				CreateNewSBAndSwapInList(added, removed, newSBs);

				SortContextually(ref newSBs);
				UpdateSBs(newSBs);
			}
				public ISlottable GetAddedForSwap(){
					ISlottable added = null;
					if(ssm.sg1 == (ISlotGroup)this)
						added = ssm.targetSB;
					else
						added = ssm.pickedSB;
					return added;
				}
				public ISlottable GetRemovedForSwap(){
					ISlottable removed;
					if(ssm.sg1 == (ISlotGroup)this)
						removed = ssm.pickedSB;
					else
						removed = ssm.targetSB;
					return removed;
				}
				public void CreateNewSBAndSwapInList(ISlottable added, ISlottable removed, List<ISlottable> list){
					if(!isPool){
						GameObject newSBGO = new GameObject("newSBGO");
						ISlottable newSB = newSBGO.AddComponent<Slottable>();
						newSB.SetItem(added.itemInst);
						newSB.SetSSM(ssm);
						newSB.SetEqpState(Slottable.unequippedState);
						newSB.Defocus();
						list[list.IndexOf(removed)] = newSB;
					}
				}
			public virtual void AddAndUpdateSBs(){
				List<InventoryItemInstance> added = ssm.moved;
				List<ISlottable> newSBs = new List<ISlottable>(toList);

				foreach(InventoryItemInstance itemInst in added){
					if(!TryChangeStackableQuantity(newSBs, itemInst, true)){
						CreateNewSBAndFill(itemInst, newSBs);
					}
				}
				SortContextually(ref newSBs);
				UpdateSBs(newSBs);
			}
				public bool TryChangeStackableQuantity(List<ISlottable> target, InventoryItemInstance item, bool added){
					bool changed = false;
					if(item.IsStackable){
						List<ISlottable> removed = new List<ISlottable>();
						foreach(ISlottable sb in target){
							if(sb != null){
								if(sb.itemInst == item){
									int newQuantity;
									if(added)
										newQuantity = sb.quantity + item.quantity;
									else
										newQuantity = sb.quantity - item.quantity;
									if(newQuantity <= 0)
										removed.Add(sb);
									else
										sb.SetQuantity(newQuantity);
									changed = true;
								}
							}
						}
						foreach(ISlottable sb in removed){
							target[target.IndexOf(sb)] = null;
							sb.Destroy();
						}
					}
					return changed;
				}
			public virtual void RemoveAndUpdateSBs(){
				List<InventoryItemInstance> removed = ssm.moved;
				List<ISlottable> thisNewSBs = toList;
				
				foreach(InventoryItemInstance item in removed){
					if(!TryChangeStackableQuantity(thisNewSBs, item, false)){
						NullifyIndexOf(item, thisNewSBs);
					}
				}
				SortContextually(ref thisNewSBs);
				
				UpdateSBs(thisNewSBs);
			}
		/*	Forward	*/
			public virtual void SetHovered(){
				ssm.SetHovered((ISlotGroup)this);
			}
			public virtual ISlottable pickedSB{get{return ssm.pickedSB;}}
			public virtual ISlottable targetSB{get{return ssm.targetSB;}}
			public List<SlottableItem> FilterItem(List<SlottableItem> items){
				filter.Filter(ref items);
				return items;
			}
			public void InitSlots(List<SlottableItem> items){
				List<Slot> newSlots = new List<Slot>();
				int slotCountToCreate = initSlotsCount == 0? items.Count: initSlotsCount;
				for(int i = 0; i <slotCountToCreate; i++){
					Slot newSlot = new Slot();
					newSlots.Add(newSlot);
				}
				SetSlots(newSlots);
			}
			public void InitSBs(List<SlottableItem> items){
				while(slots.Count < items.Count){
					items.RemoveAt(slots.Count);
				}
				foreach(SlottableItem item in items){
					GameObject newSBGO = new GameObject("newSBGO");
					ISlottable newSB = newSBGO.AddComponent<Slottable>();
					newSB.SetItem(item);
					newSB.InitializeStates();
					newSB.SetSSM(ssm);
					slots[items.IndexOf(item)].sb = newSB;
				}
			}
			public void SyncEquipped(InventoryItemInstance item, bool equipped){
				if(equipped)
					inventory.Add(item);
				else
					inventory.Remove(item);
				ssm.MarkEquippedInPool(item, equipped);
				ssm.SetEquippedOnAllSBs(item, equipped);
			}
			public void UpdateEquipStatesOnAll(){
				ssm.UpdateEquipStatesOnAll();
			}
			public void ReportTAComp(){
				ssm.AcceptSGTAComp(this);
			}
	}
	public interface ISlotGroup: ISlotSystemElement{
		/* States and Processes */
			ISSEStateEngine<ISGActState> actStateEngine{get;}
				void SetActStateEngine(ISSEStateEngine<ISGActState> engine);
				void SetActState(ISGActState state);
				ISGActState curActState{get;}
				ISGActState prevActState{get;}
			ISSEProcessEngine<ISGActProcess> actProcEngine{get;}
				void SetActProcEngine(ISSEProcessEngine<ISGActProcess> engine);
				ISGActProcess actProcess{get;}
				void SetAndRunActProcess(ISGActProcess process);
				IEnumeratorFake TransactionCoroutine();
		/*	fields	*/
			AxisScrollerMock scroller{get;}
			Inventory inventory{get;}
				void SetInventory(Inventory inv);
			bool isShrinkable{get;}
			bool isExpandable{get;}
			List<Slot> slots{get;}
				void SetSlots(List<Slot> slots);
			List<Slot> newSlots{get;}
				void SetNewSlots(List<Slot> newSlots);
			bool isPool{get;}
			bool isSGE{get;}
			bool isSGG{get;}
			bool isAutoSort{get;}
				void ToggleAutoSort(bool on);
			void SetSBs(List<ISlottable> sbs);
			List<ISlottable> newSBs{get;}
				void SetNewSBs(List<ISlottable> sbs);
			bool hasEmptySlot{get;}
			List<ISlottable> equippedSBs{get;}
			bool isAllSBActProcDone{get;}
			int initSlotsCount{get;}
				void SetInitSlotsCount(int i);

		/*	commands 	*/
			void InitializeItems();
				SlotGroupCommand initItemsCommand{get;}
			void OnActionComplete();
				SlotGroupCommand onActionCompleteCommand{get;}
			void OnActionExecute();
				SlotGroupCommand onActionExecuteCommand{get;}
		/*	Sorter	*/
			SGSorter sorter{get;}
				void SetSorter(SGSorter sorter);
			void InstantSort();
		/*	Filter	*/
			SGFilter filter{get;}
				void SetFilter(SGFilter filter);
			bool AcceptsFilter(ISlottable pickedSB);
		/*	Events	*/
			void OnHoverEnterMock();
			void OnHoverExitMock();
		/*	SSE */
			List<ISlottable> toList{get;}
			void FocusSelf();
			void FocusSBs();
			void DefocusSelf();
			void DefocusSBs();
		/*	Methods	*/
			ISlottable GetSB(InventoryItemInstance itemInst);
			bool HasItem(InventoryItemInstance invInst);
			void UpdateSBs(List<ISlottable> newSBs);
			void CreateNewSlots();
			Slot GetNewSlot(InventoryItemInstance itemInst);
			void SetSBsActStates();
			void OnCompleteSlotMovements();
			void SyncSBsToSlots();
			List<ISlottable> SwappableSBs(ISlottable pickedSB);
			void Reset();
			void ReorderAndUpdateSBs();
			void UpdateToRevert();
			void SortAndUpdateSBs();
			void FillAndUpdateSBs();
				ISlottable GetAddedForFill();
				ISlottable GetRemovedForFill();
				void CreateNewSBAndFill(InventoryItemInstance added, List<ISlottable> list);
				void NullifyIndexOf(InventoryItemInstance removed, List<ISlottable> list);
				void SortContextually(ref List<ISlottable> list);
			void SwapAndUpdateSBs();
				ISlottable GetAddedForSwap();
				ISlottable GetRemovedForSwap();
				void CreateNewSBAndSwapInList(ISlottable added, ISlottable removed, List<ISlottable> list);
			void AddAndUpdateSBs();
				bool TryChangeStackableQuantity(List<ISlottable> target, InventoryItemInstance addedItem, bool added);
			void RemoveAndUpdateSBs();
		/*	Forward	*/
			void SetHovered();
			ISlottable pickedSB{get;}
			ISlottable targetSB{get;}
			List<SlottableItem> FilterItem(List<SlottableItem> items);
			void InitSlots(List<SlottableItem> items);
			void InitSBs(List<SlottableItem> items);
			void SyncEquipped(InventoryItemInstance item, bool equipped);
			void UpdateEquipStatesOnAll();
			void ReportTAComp();
	}
}
