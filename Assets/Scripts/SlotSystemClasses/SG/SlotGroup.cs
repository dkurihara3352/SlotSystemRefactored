using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;
namespace SlotSystem{
	public class SlotGroup : SlotSystemElement, ISlotGroup{
		public void InitializeStateHandlers(){
			SetSelStateHandler(new SGSelStateHandler(this));
			SetSGActStateHandler(new SGActStateHandler(this));
		}
		public void InitializeSG(){
			SetCommandsRepo(new SGCommandsRepo(this));
			SetSlotsHolder(new SlotsHolder(this));
			SetNewSBs(new List<ISlottable>());
			SetTACache(ssm.taCache);
			SetHoverable(new Hoverable(this, taCache));
			SetSGHandler(ssm.tam.sgHandler);
			SetSorterHandler(new SorterHandler());
			InitializeStateHandlers();
		}
		/*	states	*/
			public override ISSESelStateHandler selStateHandler{
				get{
					if(_selStateHandler != null)
						return _selStateHandler;
					else
						throw new InvalidOperationException("selStateHandler not set");
				}
			}
				ISSESelStateHandler _selStateHandler;
			public override void SetSelStateHandler(ISSESelStateHandler handler){
				_selStateHandler = handler;
			}
			ISGActStateHandler actStateHandler{
				get{
					if(_actStateHandler != null)
						return _actStateHandler;
					else
						throw new System.InvalidOperationException("actStateHandler not set");
				}
			}
				ISGActStateHandler _actStateHandler;
			public void SetSGActStateHandler(ISGActStateHandler handler){
				_actStateHandler = handler;
			}
			public void ClearCurActState(){
				actStateHandler.ClearCurActState();
			}
				public bool wasActStateNull{
					get{return actStateHandler.wasActStateNull;}
				}
				public bool isActStateNull{
					get{return actStateHandler.isActStateNull;}
				}
			public ISGActState waitForActionState{
				get{return actStateHandler.waitForActionState;}
			}
				public void WaitForAction(){
					actStateHandler.WaitForAction();
				}
				public bool isWaitingForAction{
					get{return actStateHandler.isWaitingForAction;}
				}
				public bool wasWaitingForAction{
					get{return actStateHandler.wasWaitingForAction;}
				}
			public ISGActState revertState{
				get{return actStateHandler.revertState;}
			}
				public void Revert(){
					actStateHandler.Revert();
				}
				public bool isReverting{
					get{return actStateHandler.isReverting;}
				}
				public bool wasReverting{
					get{return actStateHandler.wasReverting;}
				}
			public ISGActState reorderState{
				get{return actStateHandler.reorderState;}
			}
				public void Reorder(){
					actStateHandler.Reorder();
				}
				public bool isReordering{
					get{return actStateHandler.isReordering;}
				}
				public bool wasReordering{
					get{return actStateHandler.wasReordering;}
				}
			public ISGActState addState{
				get{return actStateHandler.addState;}
			}
				public void Add(){
					actStateHandler.Add();
				}
				public bool isAdding{
					get{return actStateHandler.isAdding;}
				}
				public bool wasAdding{
					get{return actStateHandler.wasAdding;}
				}
			public ISGActState removeState{
				get{return actStateHandler.removeState;}
			}
				public void Remove(){
					actStateHandler.Remove();
				}
				public bool isRemoving{
					get{return actStateHandler.isRemoving;}
				}
				public bool wasRemoving{
					get{return actStateHandler.wasRemoving;}
				}
			public ISGActState swapState{
				get{return actStateHandler.swapState;}
			}
				public void Swap(){
					actStateHandler.Swap();
				}
				public bool isSwapping{
					get{return actStateHandler.isSwapping;}
				}
				public bool wasSwapping{
					get{return actStateHandler.wasSwapping;}
				}
			public ISGActState fillState{
				get{return actStateHandler.fillState;}
			}
				public void Fill(){
					actStateHandler.Fill();
				}
				public bool isFilling{
					get{return actStateHandler.isFilling;}
				}
				public bool wasFilling{
					get{return actStateHandler.wasFilling;}
				}
			public ISGActState sortState{
				get{return actStateHandler.sortState;}
			}
				public void Sort(){
					actStateHandler.Sort();
				}
				public bool isSorting{
					get{return actStateHandler.isSorting;}
				}
				public bool wasSorting{
					get{return actStateHandler.wasSorting;}
				}
			public void SetAndRunActProcess(ISGActProcess process){
				actStateHandler.SetAndRunActProcess(process);
			}
			public void ExpireActProcess(){
				actStateHandler.ExpireActProcess();
			}
			public ISGActProcess actProcess{
				get{return actStateHandler.actProcess;}
			}
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
		/*  Commands	*/
			ISGCommandsRepo commandsRepo{
				get{
					if(_commandsRepo != null)
						return _commandsRepo;
					else 
						throw new InvalidOperationException("commandsRepo not set");
				}
			}
				ISGCommandsRepo _commandsRepo;
			public void SetCommandsRepo(ISGCommandsRepo repo){
				_commandsRepo = repo;
			}
		/*	public fields	*/
			public Inventory inventory{
				get{
					if(_inventory != null)
						return _inventory;
					else
						throw new InvalidOperationException("inventory not set");
				}
			}
				Inventory _inventory;
				void SetInventory(Inventory inv){
					_inventory = inv;
					inv.SetSG(this);
				}
			public bool isShrinkable{
				get{return m_isShrinkable;}
			}
				bool m_isShrinkable;
			public bool isExpandable{
				get{return m_isExpandable;}
			}
				bool m_isExpandable;
		/* Hierarchy */
			public bool isPool{
				get{
					return ssm.poolBundle.ContainsInHierarchy(this);
				}
			}
			public bool isSGE{
				get{
					return ssm.equipBundle.ContainsInHierarchy(this);
				}
			}
			public bool isSGG{
				get{
					foreach(ISlotSystemBundle gBundle in ssm.otherBundles){
						if(gBundle.ContainsInHierarchy(this))
							return true;
					}
					return false;
				}
			}
		/* Slots */
			public ISlotsHolder slotsHolder{
				get{
					if(_slotsHolder != null)
						return _slotsHolder;
					else
						throw new InvalidOperationException("slotsHolder not set");
				}
			}
				ISlotsHolder _slotsHolder;
			public void SetSlotsHolder(ISlotsHolder slotsHolder){
				_slotsHolder = slotsHolder;
			}
			public List<Slot> slots{
				get{return slotsHolder.slots;}
			}
			public void SetSlots(List<Slot> slots){
				slotsHolder.SetSlots(slots);
			}
			public bool hasEmptySlot{
				get{return slotsHolder.hasEmptySlot;}
			}
			public List<Slot> newSlots{
				get{return slotsHolder.newSlots;}
			}
			public void SetNewSlots(List<Slot> newSlots){
				slotsHolder.SetNewSlots(newSlots);
			}
			public Slot GetNewSlot(InventoryItemInstance item){
				return slotsHolder.GetNewSlot(item);
			}
			public void SetInitSlotsCount(int count){
				slotsHolder.SetInitSlotsCount(count);
			}
			public int initSlotsCount{
				get{return slotsHolder.initSlotsCount;}
			}
			public void InitSlots(List<InventoryItemInstance> items){
				slotsHolder.InitSlots(items);
			}
			public void PutSBsInSlots(List<ISlottable> sbs){
				slotsHolder.PutSBsInSlots(sbs);
			}
		/* SB */
			public ISlottable GetSB(InventoryItemInstance itemInst){
				foreach(ISlottable sb in this){
					if(sb != null)
						if(sb.item == itemInst)
							return sb;
				}
				return null;
			}
			public bool HasItem(InventoryItemInstance itemInst){
				return GetSB(itemInst) != null;
			}
			List<ISlottable> slottables{
				get{
					if(_slottables != null)
						return _slottables;
					else
						throw new InvalidOperationException("sbs not set");
				}
			}
				List<ISlottable> _slottables;
				public void SetSBs(List<ISlottable> sbs){
					_slottables = sbs;
				}
			public List<ISlottable> newSBs{
				get{
					if(_newSBs != null)
						return _newSBs;
					else
						throw new InvalidOperationException("newSBs not Set");
				}
			}
				List<ISlottable> _newSBs;
				public void SetNewSBs(List<ISlottable> sbs){
					_newSBs = sbs;
				}
			public List<ISlottable> equippedSBs{
				get{
					List<ISlottable> result = new List<ISlottable>();
					foreach(ISlottable sb in this){
						if(sb != null && sb.isEquipped)
							result.Add(sb);
					}
					return result;
				}
			}
			public bool isAllSBActProcDone{
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
		/*	sorter	*/
			public ISorterHandler sorterHandler{
				get{
					if(_sorterHandler != null)
						return _sorterHandler;
					else
						throw new InvalidOperationException("sorterHandler not set");
				}
			}
				ISorterHandler _sorterHandler;
			public void SetSorterHandler(ISorterHandler sorterHandler){
				_sorterHandler = sorterHandler;
			}
			public void InstantSort(){
				List<ISlottable> sortedSBs = GetSortedSBsWithoutResize(slottables);
				PutSBsInSlots(sortedSBs);
			}
			public void ToggleAutoSort(bool on){
				SetIsAutoSort(on);
				Focus();
			}
			public List<ISlottable> GetSortedSBsWithoutResize(List<ISlottable> source){
				return sorterHandler.GetSortedSBsWithoutResize(source);
			}
			public List<ISlottable> GetSortedSBsWithResize(List<ISlottable> source){
				return sorterHandler.GetSortedSBsWithResize(source);
			}
			public void SetSorter(SGSorter sorter){
				sorterHandler.SetSorter(sorter);
			}
			public void SetIsAutoSort(bool on){
				sorterHandler.SetIsAutoSort(on);
			}
			public bool isAutoSort{
				get{return sorterHandler.isAutoSort;}
			}
		/*	filter	*/
			public List<InventoryItemInstance> FilterItem(List<InventoryItemInstance> items){
				filter.Filter(ref items);
				return items;
			}
			public SGFilter filter{
				get{
					if(_filter != null)
						return _filter;
					else
						throw new InvalidOperationException("filter not set");
				}
			}
				SGFilter _filter;
				public void SetFilter(SGFilter filter){
					_filter = filter;
				}
			public bool AcceptsFilter(ISlottable pickedSB){
				if(this.filter is SGNullFilter) return true;
				else{
					if(pickedSB.item is BowInstance)
						return this.filter is SGBowFilter;
					else if(pickedSB.item is WearInstance)
						return this.filter is SGWearFilter;
					else if(pickedSB.item is CarriedGearInstance)
						return this.filter is SGCGearsFilter;
					else
						return this.filter is SGPartsFilter;
				}
			}
		/*	SlotSystemElement implementation	*/
			protected override IEnumerable<ISlotSystemElement> elements{
				get{
					foreach(ISlottable sb in slottables)
						yield return (ISlotSystemElement)sb;
				}
			}
			public override void SetElements(IEnumerable<ISlotSystemElement> elements){
				List<ISlottable> sbs = new List<ISlottable>();
				foreach(var e in elements){
					if(e == null || e is ISlottable){
						sbs.Add(e as ISlottable);
					}else
						throw new System.ArgumentException("parameter needs to be a collection of only ISlottable or null");
				}
				SetSBs(sbs);
			}
			public List<ISlottable> toList{
				get{return slottables;}
			}
			public override bool Contains(ISlotSystemElement element){
				if(element is ISlottable)
					return slottables.Contains((ISlottable)element);
				return false;
			}
			public void FocusSBs(){
				foreach(ISlottable sb in this){
					if(sb != null){
						sb.Refresh();
						if(sb.passesPrePickFilter)
							sb.Focus();
						else
							sb.Defocus();
					}
				}
			}
			public void DefocusSBs(){
				foreach(ISlottable sb in this){
					if(sb != null){
						sb.Refresh();
						sb.Defocus();
					}
				}
			}
		/* Transaction */
			public ITransactionSGHandler sgHandler{
				get{
					if(_sgHandler != null)
						return _sgHandler;
					else
						throw new InvalidOperationException("sgHandler not set");
				}
			}
				ITransactionSGHandler _sgHandler;
			public void SetSGHandler(ITransactionSGHandler sgHandler){
				_sgHandler = sgHandler;
			}
			public void ReorderAndUpdateSBs(){
				ISlottable sb1 = taCache.pickedSB;
				ISlottable sb2 = taCache.targetSB;
				List<ISlottable> newSBs = new List<ISlottable>(toList);
				newSBs.Reorder(sb1, sb2);
				UpdateSBs(newSBs);
			}
			public void RevertAndUpdateSBs(){
				SetNewSBs(toList);
				CreateNewSlots();
				SetSBsActStates();
			}
			public void SortAndUpdateSBs(){
				List<ISlottable> newSBs;
				if(isExpandable)
					newSBs = GetSortedSBsWithResize(toList);
				else
					newSBs = GetSortedSBsWithoutResize(toList);
				
				UpdateSBs(newSBs);
			}
			public void FillAndUpdateSBs(){
				ISlottable added = GetAddedForFill();
				ISlottable removed = GetRemovedForFill();

				List<ISlottable> newSBs = new List<ISlottable>(toList);

				if(!isPool){
					if(added != null)
						CreateNewSBAndFill(added.item, newSBs);
					if(removed != null)
						NullifyIndexOf(removed.item, newSBs);
				}
				newSBs = SortedSBsIfAutoSortAccordingToExpandability(newSBs);
				UpdateSBs(newSBs);
			}
			public ISlottable GetAddedForFill(){
				ISlottable added;
				if(sgHandler.sg1 == (ISlotGroup)this)
					added = null;
				else
					added = taCache.pickedSB;
				return added;
			}
			public ISlottable GetRemovedForFill(){
				ISlottable removed;
				if(sgHandler.sg1 == (ISlotGroup)this)
					removed = taCache.pickedSB;
				else
					removed = null;
				return removed;
			}
			public void SwapAndUpdateSBs(){
				ISlottable added = GetAddedForSwap();
				ISlottable removed = GetRemovedForSwap();
				List<ISlottable> newSBs = new List<ISlottable>(toList);
				
				CreateNewSBAndSwapInList(added, removed, newSBs);

				newSBs = SortedSBsIfAutoSortAccordingToExpandability(newSBs);
				UpdateSBs(newSBs);
			}
			public ISlottable GetAddedForSwap(){
				ISlottable added = null;
				if(sgHandler.sg1 == (ISlotGroup)this)
					added = taCache.targetSB;
				else
					added = taCache.pickedSB;
				return added;
			}
			public ISlottable GetRemovedForSwap(){
				ISlottable removed;
				if(sgHandler.sg1 == (ISlotGroup)this)
					removed = taCache.pickedSB;
				else
					removed = taCache.targetSB;
				return removed;
			}
			void CreateNewSBAndSwapInList(ISlottable added, ISlottable removed, List<ISlottable> list){
				if(!isPool){
					Slottable newSB = CreateSB(added.item);
					newSB.Unequip();
					list[list.IndexOf(removed)] = newSB;
				}
			}
			public void AddAndUpdateSBs(){
				List<InventoryItemInstance> added = taCache.moved;
				List<ISlottable> newSBs = new List<ISlottable>(toList);

				foreach(InventoryItemInstance itemInst in added){
					if(!TryChangeStackableQuantity(newSBs, itemInst, true)){
						CreateNewSBAndFill(itemInst, newSBs);
					}
				}
				newSBs = SortedSBsIfAutoSortAccordingToExpandability(newSBs);
				UpdateSBs(newSBs);
			}
			public bool TryChangeStackableQuantity(List<ISlottable> target, InventoryItemInstance item, bool added){
				bool changed = false;
				if(item.IsStackable){
					List<ISlottable> removed = new List<ISlottable>();
					foreach(ISlottable sb in target){
						if(sb != null){
							if(sb.item == item){
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
			public void RemoveAndUpdateSBs(){
				List<InventoryItemInstance> removed = taCache.moved;
				List<ISlottable> thisNewSBs = toList;
				
				foreach(InventoryItemInstance item in removed){
					if(!TryChangeStackableQuantity(thisNewSBs, item, false)){
						NullifyIndexOf(item, thisNewSBs);
					}
				}
				thisNewSBs = SortedSBsIfAutoSortAccordingToExpandability(thisNewSBs);
				
				UpdateSBs(thisNewSBs);
			}
			//Transaction Utility
			public Slottable CreateSB(InventoryItemInstance item){
				GameObject newSBGO = new GameObject("newSBGO");
				Slottable newSB = newSBGO.AddComponent<Slottable>();
				newSB.SetSSM(ssm);
				newSB.InitializeSB(item);
				newSB.Defocus();
				return newSB;
			}
			public void UpdateSBs(List<ISlottable> newSBs){
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
			public void CreateNewSlots(){
				List<Slot> newSlots = new List<Slot>();
				for(int i = 0; i < newSBs.Count; i++){
					Slot newSlot = new Slot();
					newSlots.Add(newSlot);
				}
				SetNewSlots(newSlots);
			}
			public void SetSBsActStates(){
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
					sb.MoveWithin();
				}
				foreach(ISlottable sb in removed){
					sb.SetNewSlotID(-1);
					sb.Remove();
				}
				foreach(ISlottable sb in added){
					sb.SetNewSlotID(newSBs.IndexOf(sb));
					sb.Add();
				}
			}
			public void OnCompleteSlotMovements(){
				foreach(ISlottable sb in this){
					if(sb != null){
						if(sb.isToBeRemoved){
							sb.Destroy();
						}else{
							newSlots[sb.newSlotID].sb = sb;
						}
					}
				}
				SetSlots(newSlots);
				SyncSBsToSlots();
			}
			public void SyncSBsToSlots(){
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
			public void CreateNewSBAndFill(InventoryItemInstance addedItem, List<ISlottable> list){
				Slottable newSB = CreateSB(addedItem);
				newSB.Unequip();
				list.Fill(newSB);
			}
			public void NullifyIndexOf(InventoryItemInstance removedItem, List<ISlottable> list){
				ISlottable rem = null;
				foreach(ISlottable sb in list){
					if(sb != null){
						if(sb.item == removedItem)
							rem = sb;
					}
				}
				list[list.IndexOf(rem)] = null;
			}
			List<ISlottable> SortedSBsIfAutoSortAccordingToExpandability(List<ISlottable> source){
				List<ISlottable> result = source;
				if(isAutoSort){
					if(isExpandable)
						result = GetSortedSBsWithResize(result);
					else
						result = GetSortedSBsWithoutResize(result);
				}
				if(result == null)
					return source;
				return result;
			}
			public void OnActionComplete(){
				onActionCompleteCommand.Execute();
			}
				public ISGCommand onActionCompleteCommand{
					get{return commandsRepo.onActionCompleteCommand;}
				}
			public void UpdateEquipStatesOnAll(){
				ssm.UpdateEquipInvAndAllSBsEquipState();
			}
			public void OnActionExecute(){
				onActionExecuteCommand.Execute();
			}
				public ISGCommand onActionExecuteCommand{
					get{return commandsRepo.onActionExecuteCommand;}
				}
			public void SyncEquipped(InventoryItemInstance item, bool equipped){
				if(equipped)
					inventory.Add(item);
				else
					inventory.Remove(item);
				ssm.MarkEquippedInPool(item, equipped);
				ssm.SetEquippedOnAllSBs(item, equipped);
			}
			public void ReportTAComp(){
				sgHandler.AcceptSGTAComp(this);
			}
		/* Other */
			public List<ISlottable> SwappableSBs(ISlottable pickedSB){
				List<ISlottable> result = new List<ISlottable>();
				foreach(ISlottable sb in this){
					if(sb != null){
						if(SlotSystemUtil.AreSwappable(pickedSB, sb))
							result.Add(sb);
					}
				}
				return result;
			}
			public void Refresh(){
				WaitForAction();
				SetNewSBs(new List<ISlottable>());
				SetNewSlots(new List<Slot>());
			}
			public void InspectorSetUp(Inventory inv, SGFilter filter, SGSorter sorter, int initSlotsCount){
				SetInventory(inv);
				SetFilter(filter);
				SetSorter(sorter);
				SetInitSlotsCount(initSlotsCount);
				m_isExpandable = initSlotsCount == 0;
			}
			public override void SetHierarchy(){
				InitializeItems();
			}
			public void InitializeItems(){
				initItemsCommand.Execute();
			}
				public ISGCommand initItemsCommand{
					get{return commandsRepo.initializeItemsCommand;}
				}
			public void InitSBs(List<InventoryItemInstance> items){
				while(slots.Count < items.Count){
					items.RemoveAt(slots.Count);
				}
				foreach(InventoryItemInstance item in items){
					ISlottable newSB = CreateSB(item);
					slots[items.IndexOf(item)].sb = newSB;
				}
			}
		/* Hoverable */
			public ITransactionCache taCache{
				get{
					if(_taCache != null)
						return _taCache;
					else
						throw new System.InvalidOperationException("taCache is not set");
				}
			}
				ITransactionCache _taCache;
			public void SetTACache(ITransactionCache taCache){
				_taCache = taCache;
			}
			public IHoverable hoverable{
				get{
					if(_hoverable != null)
						return _hoverable;
					else
						throw new InvalidOperationException("hoverable not set");
				}
			}
				IHoverable _hoverable;
			public void SetHoverable(IHoverable hoverable){
				_hoverable = hoverable;
			}
			public void OnHoverEnter(){
				hoverable.OnHoverEnter();
			}
			public void OnHoverExit(){
				hoverable.OnHoverExit();
			}
			public bool isHovered{
				get{return hoverable.isHovered;}
			}
	}
	public interface ISlotGroup: ISlotSystemElement, IHoverable, ISGActStateHandler, ISlotsHolder, ISorterHandler{
		/*	instrinsic	*/
			Inventory inventory{get;}
			bool isShrinkable{get;}
			bool isExpandable{get;}
			bool isPool{get;}
			ISlottable GetSB(InventoryItemInstance itemInst);
			bool HasItem(InventoryItemInstance invInst);
			void SetSBs(List<ISlottable> sbs);
			List<ISlottable> SwappableSBs(ISlottable pickedSB);
			void InitializeItems();
			void InitSBs(List<InventoryItemInstance> items);
		/*	Sorter	*/
			void InstantSort();
		/*	Filter	*/
			List<InventoryItemInstance> FilterItem(List<InventoryItemInstance> items);
			SGFilter filter{get;}
			bool AcceptsFilter(ISlottable pickedSB);
		/*	Transaction	*/
			ITransactionSGHandler sgHandler{get;}
			void ReorderAndUpdateSBs();
			void RevertAndUpdateSBs();
			void SortAndUpdateSBs();
			void FillAndUpdateSBs();
			void SwapAndUpdateSBs();
			void AddAndUpdateSBs();
			void RemoveAndUpdateSBs();
			void OnCompleteSlotMovements();
			void SyncSBsToSlots();
			void OnActionComplete();
			void UpdateEquipStatesOnAll();
			void OnActionExecute();
			void SyncEquipped(InventoryItemInstance item, bool equipped);
			void ReportTAComp();
	}
}
