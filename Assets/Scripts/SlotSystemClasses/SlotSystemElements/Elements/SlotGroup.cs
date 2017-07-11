using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace SlotSystem{
	public class SlotGroup : AbsSlotSystemElement, ISlotGroup{
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
						foreach(ISlottable sb in slottables){
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
			public virtual bool isShrinkable{
				get{return m_isShrinkable;}
				set{m_isShrinkable = value;}
				}bool m_isShrinkable;
			public virtual bool isExpandable{
				get{return m_isExpandable;}
				set{m_isExpandable = value;}
				}bool m_isExpandable;
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
			public List<ISlottable> slottables{
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
			public virtual List<InventoryItemInstance> itemInstances{
				get{
					List<InventoryItemInstance> result = new List<InventoryItemInstance>();
						foreach(ISlottable sb in slottables){
							if(sb != null)
								result.Add(sb.itemInst);
							else
								result.Add(null);
						}
					return result;
				}
			}
			public virtual List<InventoryItemInstance> actualItemInsts{
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
					return (ssm.focusedSGP == (ISlotGroup)this || ssm.focusedSGEs.Contains(this));
				}
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
			public virtual int actualSBsCount{
				get{
					int count = 0;
					foreach(Slot slot in slots){
						if(slot.sb != null)
							count ++;
					}
					return count;
				}
			}
			public virtual List<ISlottable> equippedSBs{
				get{
					List<ISlottable> result = new List<ISlottable>();
					foreach(ISlottable sb in slottables){
						if(sb != null && sb.isEquipped)
							result.Add(sb);
					}
					return result;
				}
			}
			public virtual bool isAllTASBsDone{
				get{
					foreach(ISlottable sb in slottables){
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
			public virtual void InitializeItems(){
				m_initItemsCommand.Execute(this);
				}SlotGroupCommand m_initItemsCommand = new SGInitItemsCommand();
			public virtual void OnActionComplete(){
				m_onActionCompleteCommand.Execute(this);
				}SlotGroupCommand m_onActionCompleteCommand;
			public virtual void OnActionExecute(){
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
			
			public virtual SGSorter Sorter{
				get{return m_sorter;}
				}SGSorter m_sorter;
				public virtual void SetSorter(SGSorter sorter){
					m_sorter = sorter;
				}
			public virtual void InstantSort(){
				List<ISlottable> orderedSbs = slottables;
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
			public virtual SGFilter Filter{
				get{return m_filter;}
				set{m_filter = value;}
				}SGFilter m_filter;
				public virtual void SetFilter(SGFilter filter){
					m_filter = filter;
				}
			public virtual bool AcceptsFilter(ISlottable pickedSB){
				if(this.Filter is SGNullFilter) return true;
				else{
					if(pickedSB.item is BowInstance)
						return this.Filter is SGBowFilter;
					else if(pickedSB.item is WearInstance)
						return this.Filter is SGWearFilter;
					else if(pickedSB.item is CarriedGearInstance)
						return this.Filter is SGCGearsFilter;
					else
						return this.Filter is SGPartsFilter;
				}
			}
		/*	events	*/
			public virtual void OnHoverEnterMock(){
				PointerEventDataFake eventData = new PointerEventDataFake();
				((SGSelState)curSelState).OnHoverEnterMock(this, eventData);
			}
			public virtual void OnHoverExitMock(){
				PointerEventDataFake eventData = new PointerEventDataFake();
				((SGSelState)curSelState).OnHoverExitMock(this, eventData);
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
					SetSelState(SlotGroup.sgFocusedState);
					FocusSBs();
					Reset();
				}
				public virtual void FocusSelf(){
					SetSelState(SlotGroup.sgFocusedState);
				}
				public virtual void FocusSBs(){
					foreach(ISlottable sb in slottables){
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
				public virtual void DefocusSelf(){
					SetSelState(SlotGroup.sgDefocusedState);
				}
				public virtual void DefocusSBs(){
					foreach(ISlottable sb in slottables){
						if(sb != null){
							sb.SetActState(Slottable.sbWaitForActionState);
							sb.Reset();
							sb.Defocus();
						}
					}
				}
				public override void Deactivate(){
					SetSelState(SlotGroup.sgDeactivatedState);
					foreach(ISlottable sb in slottables){
						if(sb != null){
							sb.Deactivate();
						}
					}
				}
				public override void PerformInHierarchy(System.Action<ISlotSystemElement> act){
					act(this);
					foreach(ISlottable sb in slottables){
						if(sb != null)
							sb.PerformInHierarchy(act);
					}
				}
				public override void PerformInHierarchy(System.Action<ISlotSystemElement, object> act, object obj){
					act(this, obj);
					foreach(ISlottable sb in slottables){
						if(sb != null)
							sb.PerformInHierarchy(act, obj);
					}
				}
				public override void PerformInHierarchy<T>(System.Action<ISlotSystemElement, IList<T>> act, IList<T> list){
					act(this, list);
					foreach(ISlottable sb in slottables){
						if(sb != null)
							sb.PerformInHierarchy<T>(act, list);
					}
				}
		/*	methods	*/
			public virtual void Initialize(string name, SGFilter filter, Inventory inv, bool isShrinkable, int initSlotsCount, SlotGroupCommand onActionCompleteCommand, SlotGroupCommand onActionExecuteCommand){
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
			public virtual ISlottable GetSB(InventoryItemInstance itemInst){
				foreach(ISlottable sb in slottables){
					if(sb != null){
						if(sb.itemInst == itemInst)
							return sb;
					}
				}
				return null;
			}
			public virtual bool HasItem(InventoryItemInstance invInst){
				bool result = false;
				foreach(ISlottable sb in slottables){
					if(sb != null){
						if(sb.itemInst == invInst)
							return true;
					}
				}
				return result;
			}
			public virtual void UpdateSBs(List<ISlottable> newSBs){
				/*	Create and set new Slots	*/
					List<Slot> newSlots = new List<Slot>();
					for(int i = 0; i < newSBs.Count; i++){
						Slot newSlot = new Slot();
						newSlots.Add(newSlot);
					}
					SetNewSlots(newSlots);
				/*	Set SBs act states	*/
				List<ISlottable> moveWithins = new List<ISlottable>();
				List<ISlottable> removed = new List<ISlottable>();
				List<ISlottable> added = new List<ISlottable>();
				foreach(ISlottable sb in slottables){
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
				List<ISlottable> allSBs = new List<ISlottable>();
				allSBs.AddRange(slottables);
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
				foreach(ISlottable sb in slottables){
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
				foreach(ISlottable sb in slottables){
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
			public virtual void CheckProcessCompletion(){
				TransactionCoroutine();
			}
			public virtual void OnCompleteSlotMovements(){
				foreach(ISlottable sb in slottables){
					if(sb != null){
						if(sb.newSlotID == -1){
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
				foreach(ISlottable sb in slottables){
					if(sb != null)
					sb.SetSlotID(newSBs.IndexOf(sb));
				}
			}
			public virtual List<ISlottable> SwappableSBs(ISlottable pickedSB){
				List<ISlottable> result = new List<ISlottable>();
				foreach(ISlottable sb in slottables){
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
				UpdateSBs(new List<ISlottable>(toList));
			}
			public virtual void SortAndUpdateSBs(){
				List<ISlottable> newSBs = new List<ISlottable>(toList);
				int origCount = newSBs.Count;
				Sorter.TrimAndOrderSBs(ref newSBs);
				if(!isExpandable){
					while(newSBs.Count <origCount){
						newSBs.Add(null);
					}
				}
				UpdateSBs(newSBs);
			}
			public virtual void FillAndUpdateSBs(){
				ISlottable added;
					if(ssm.transaction.sg1 == (ISlotGroup)this)
						added = null;
					else
						added = ssm.pickedSB;
				ISlottable removed;
					if(ssm.transaction.sg1 == (ISlotGroup)this)
						removed = ssm.pickedSB;
					else
						removed = null;

				List<ISlottable> newSBs = new List<ISlottable>(toList);
				int origCount = newSBs.Count;
				if(!isPool){
					if(added != null){
						GameObject newSBGO = new GameObject("newSBGO");
						ISlottable newSB = newSBGO.AddComponent<Slottable>();
						newSB.Initialize(added.itemInst);
						newSB.SetSSM(ssm);
						newSB.Defocus();
						newSB.SetEqpState(Slottable.unequippedState);
						newSBs.Fill(newSB);
					}
					if(removed != null){
						ISlottable rem = null;
						foreach(ISlottable sb in newSBs){
							if(sb != null){
								if(sb.itemInst == removed.itemInst)
									rem = sb;
							}
						}
						newSBs[newSBs.IndexOf(rem)] = null;
					}
				}
				if(isAutoSort){
					Sorter.TrimAndOrderSBs(ref newSBs);
				}
				if(!isExpandable){
					while(newSBs.Count <origCount){
						newSBs.Add(null);
					}
				}
				UpdateSBs(newSBs);
			}
			public virtual void SwapAndUpdateSBs(){
				ISlottable added;
					if(ssm.transaction.sg1 == (ISlotGroup)this)
						added = ssm.transaction.targetSB;
					else
						added = ssm.pickedSB;
				ISlottable removed;
					if(ssm.transaction.sg1 == (ISlotGroup)this)
						removed = ssm.pickedSB;
					else
						removed = ssm.transaction.targetSB;
				List<ISlottable> newSBs = new List<ISlottable>(toList);
				int origCount = newSBs.Count;
				if(!isPool){
					GameObject newSBGO = new GameObject("newSBGO");
					ISlottable newSB = newSBGO.AddComponent<Slottable>();
					newSB.Initialize(added.itemInst);
					newSB.SetSSM(ssm);
					newSB.SetEqpState(Slottable.unequippedState);
					newSB.Defocus();
					newSBs[newSBs.IndexOf(removed)] = newSB;
				}
				if(isAutoSort){
					Sorter.TrimAndOrderSBs(ref newSBs);
					if(!isExpandable){
						while(newSBs.Count <origCount){
							newSBs.Add(null);
						}
					}
				}
				UpdateSBs(newSBs);
			}
			public virtual void AddAndUpdateSBs(){
				List<InventoryItemInstance> cache = ssm.transaction.moved;
				List<ISlottable> newSBs = toList;
				int origCount = newSBs.Count;
				foreach(InventoryItemInstance itemInst in cache){
					bool found = false;
					foreach(ISlottable sb in newSBs){
						if(sb!= null){
							if(sb.itemInst == itemInst){
								if(itemInst.Item.IsStackable){
									sb.itemInst.Quantity += itemInst.Quantity;
									found = true;
								}
							}
						}
					}
					if(!found){
						GameObject newSBSG = new GameObject("newSBSG");
						ISlottable newSB = newSBSG.AddComponent<Slottable>();
						newSB.Initialize(itemInst);
						newSB.SetSSM(ssm);
						newSB.Defocus();
						newSBs.Fill(newSB);
					}
				}
				if(isAutoSort)
					Sorter.TrimAndOrderSBs(ref newSBs);
				if(!isExpandable){
					while(newSBs.Count <origCount){
						newSBs.Add(null);
					}
				}
				SetNewSBs(newSBs);
				CreateNewSlots();
				SetSBsActStates();
			}
			public virtual void RemoveAndUpdateSBs(){
				List<InventoryItemInstance> cache = ssm.transaction.moved;
				List<ISlottable> newSBs = toList;
				int origCount = newSBs.Count;
				List<ISlottable> removedList = new List<ISlottable>();
				List<ISlottable> nonremoved = new List<ISlottable>();
				foreach(InventoryItemInstance itemInst in cache){
					foreach(ISlottable sb in newSBs){
						if(sb!= null){
							if(sb.itemInst == itemInst){
								if(itemInst.Item.IsStackable){
									sb.itemInst.Quantity -= itemInst.Quantity;
									if(sb.itemInst.Quantity <= 0)
										removedList.Add(sb);
								}else{
									removedList.Add(sb);
								}
							}
						}
					}
				}
				foreach(ISlottable sb in removedList){
					newSBs[newSBs.IndexOf(sb)] = null;
				}
				if(isAutoSort){
					Sorter.TrimAndOrderSBs(ref newSBs);
					if(!isExpandable){
						while(newSBs.Count <origCount){
							newSBs.Add(null);
						}
					}
				}else{
					if(isExpandable)
						newSBs.Trim();
				}
				SetNewSBs(nonremoved);
				CreateNewSlots();
				SetSBsActStates();
			}
		/*	Forward	*/
			public virtual void SetHovered(){
				ssm.SetHovered((ISlotGroup)this);
			}
			public virtual ISlottable pickedSB{get{return ssm.pickedSB;}}
			public virtual ISlottable targetSB{get{return ssm.targetSB;}}
	}
	public interface ISlotGroup: IAbsSlotSystemElement{
		IEnumeratorFake TransactionCoroutine();
		/*	fields	*/
			AxisScrollerMock scroller{get;}
			Inventory inventory{get;}
			void SetInventory(Inventory inv);
			bool isShrinkable{get; set;}
			bool isExpandable{get; set;}
			List<Slot> slots{get;}
			void SetSlots(List<Slot> slots);
			List<Slot> newSlots{get;}
			void SetNewSlots(List<Slot> newSlots);
			bool isPool{get;}
			bool isSGE{get;}
			bool isSGG{get;}
			bool isAutoSort{get;}
			void ToggleAutoSort(bool on);
			List<ISlottable> slottables{get;}
			void SetSBs(List<ISlottable> sbs);
			List<ISlottable> newSBs{get;}
			void SetNewSBs(List<ISlottable> sbs);
			List<InventoryItemInstance> itemInstances{get;}
			List<InventoryItemInstance> actualItemInsts{get;}
			bool isFocusedInBundle{get;}
			bool hasEmptySlot{get;}
			int actualSBsCount{get;}
			List<ISlottable> equippedSBs{get;}
			bool isAllTASBsDone{get;}
			int initSlotsCount{get;}
			void SetInitSlotsCount(int i);

		/*	commands 	*/
			void InitializeItems();
			void OnActionComplete();
			void OnActionExecute();
		/*	Sorter	*/
			SGSorter Sorter{get;}
			void SetSorter(SGSorter sorter);
			void InstantSort();
		/*	Filter	*/
			SGFilter Filter{get;set;}
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
			void Initialize(string name, SGFilter filter, Inventory inv, bool isShrinkable, int initSlotsCount, SlotGroupCommand onActionCompleteCommand, SlotGroupCommand onActionExecuteCommand);
			ISlottable GetSB(InventoryItemInstance itemInst);
			bool HasItem(InventoryItemInstance invInst);
			void UpdateSBs(List<ISlottable> newSBs);
			void CreateNewSlots();
			Slot GetNewSlot(InventoryItemInstance itemInst);
			void SetSBsActStates();
			void CheckProcessCompletion();
			void OnCompleteSlotMovements();
			void SyncSBsToSlots();
			List<ISlottable> SwappableSBs(ISlottable pickedSB);
			void Reset();
			void ReorderAndUpdateSBs();
			void UpdateToRevert();
			void SortAndUpdateSBs();
			void FillAndUpdateSBs();
			void SwapAndUpdateSBs();
			void AddAndUpdateSBs();
			void RemoveAndUpdateSBs();
		/*	Forward	*/
			void SetHovered();
			ISlottable pickedSB{get;}
			ISlottable targetSB{get;}
	}
}
