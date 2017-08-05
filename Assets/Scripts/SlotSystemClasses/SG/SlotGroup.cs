﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;
namespace SlotSystem{
	public class SlotGroup : SlotSystemElement, ISlotGroup{
		/*	states	*/
			public override void Activate(){
				if(taCache.IsCachedTAResultRevert(hoverable) == false)
					Focus();
				else
					Defocus();
			}
			public override void Deselect(){
				Activate();
			}
			ISGActStateHandler sgStateHandler{
				get{
					if(_sgStateHandler != null)
						return _sgStateHandler;
					else
						throw new System.InvalidOperationException("sgStateHandler not set");
				}
			}
				ISGActStateHandler _sgStateHandler;
			public void SetSGActStateHandler(ISGActStateHandler handler){
				_sgStateHandler = handler;
			}
		public void ClearCurActState(){
			sgStateHandler.ClearCurActState();
		}
			public bool wasActStateNull{
				get{return sgStateHandler.wasActStateNull;}
			}
			public bool isActStateNull{
				get{return sgStateHandler.isActStateNull;}
			}
		public ISGActState waitForActionState{
			get{return sgStateHandler.waitForActionState;}
		}
			public void WaitForAction(){
				sgStateHandler.WaitForAction();
			}
			public bool isWaitingForAction{
				get{return sgStateHandler.isWaitingForAction;}
			}
			public bool wasWaitingForAction{
				get{return sgStateHandler.wasWaitingForAction;}
			}
		public ISGActState revertState{
			get{return sgStateHandler.revertState;}
		}
			public void Revert(){
				sgStateHandler.Revert();
			}
			public bool isReverting{
				get{return sgStateHandler.isReverting;}
			}
			public bool wasReverting{
				get{return sgStateHandler.wasReverting;}
			}
		public ISGActState reorderState{
			get{return sgStateHandler.reorderState;}
		}
			public void Reorder(){
				sgStateHandler.Reorder();
			}
			public bool isReordering{
				get{return sgStateHandler.isReordering;}
			}
			public bool wasReordering{
				get{return sgStateHandler.wasReordering;}
			}
		public ISGActState addState{
			get{return sgStateHandler.addState;}
		}
			public void Add(){
				sgStateHandler.Add();
			}
			public bool isAdding{
				get{return sgStateHandler.isAdding;}
			}
			public bool wasAdding{
				get{return sgStateHandler.wasAdding;}
			}
		public ISGActState removeState{
			get{return sgStateHandler.removeState;}
		}
			public void Remove(){
				sgStateHandler.Remove();
			}
			public bool isRemoving{
				get{return sgStateHandler.isRemoving;}
			}
			public bool wasRemoving{
				get{return sgStateHandler.wasRemoving;}
			}
		public ISGActState swapState{
			get{return sgStateHandler.swapState;}
		}
			public void Swap(){
				sgStateHandler.Swap();
			}
			public bool isSwapping{
				get{return sgStateHandler.isSwapping;}
			}
			public bool wasSwapping{
				get{return sgStateHandler.wasSwapping;}
			}
		public ISGActState fillState{
			get{return sgStateHandler.fillState;}
		}
			public void Fill(){
				sgStateHandler.Fill();
			}
			public bool isFilling{
				get{return sgStateHandler.isFilling;}
			}
			public bool wasFilling{
				get{return sgStateHandler.wasFilling;}
			}
		public ISGActState sortState{
			get{return sgStateHandler.sortState;}
		}
			public void Sort(){
				sgStateHandler.Sort();
			}
			public bool isSorting{
				get{return sgStateHandler.isSorting;}
			}
			public bool wasSorting{
				get{return sgStateHandler.wasSorting;}
			}
	/* Proc */
		public void SetAndRunActProcess(ISGActProcess process){
			sgStateHandler.SetAndRunActProcess(process);
		}
		public void ExpireActProcess(){
			sgStateHandler.ExpireActProcess();
		}
		public ISGActProcess actProcess{
			get{return sgStateHandler.actProcess;}
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
			ISGCommandsFactory commandsFactory{
				get{
					if(_commandsFactory == null)
						_commandsFactory = new SGCommandsFactory(this);
					return _commandsFactory;
				}
			}
				ISGCommandsFactory _commandsFactory;

		/*	public fields	*/
			public Inventory inventory{
				get{return m_inventory;}
			}
				Inventory m_inventory;
				void SetInventory(Inventory inv){
					m_inventory = inv;
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
			public Slot GetNewSlot(InventoryItemInstance itemInst){
				int index = -3;
				foreach(ISlottable sb in this){
					if(sb != null){
						if(sb.item == itemInst)
							index = sb.newSlotID;
					}
				}
				if(index != -3)
					return newSlots[index];
				else 
					return null;
			}
			public List<Slot> newSlots{
				get{return m_newSlots;}
			}
				List<Slot> m_newSlots;
				public void SetNewSlots(List<Slot> newSlots){
					m_newSlots = newSlots;
				}
			public List<Slot> slots{
				get{
					if(m_slots == null)
						m_slots = new List<Slot>();
					return m_slots;}
			}
				List<Slot> m_slots;
				public void SetSlots(List<Slot> slots){
					m_slots = slots;
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
			public void SetInitSlotsCount(int i){
				m_initSlotsCount = i;
			}
				public int initSlotsCount{
					get{return m_initSlotsCount;}
				}
				int m_initSlotsCount;
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
					if(m_slottables == null)
						m_slottables = new List<ISlottable>();
					return m_slottables;
				}
			}
				List<ISlottable> m_slottables;
				public void SetSBs(List<ISlottable> sbs){
					m_slottables = sbs;
				}
			public List<ISlottable> newSBs{
				get{return m_newSBs;}
			}
				List<ISlottable> m_newSBs;
				public void SetNewSBs(List<ISlottable> sbs){
					m_newSBs = sbs;
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
			public void InstantSort(){
				List<ISlottable> orderedSbs = slottables;
				sorter.OrderSBsWithRetainedSize(ref orderedSbs);
				foreach(Slot slot in slots){
					slot.sb = orderedSbs[slots.IndexOf(slot)];
				}
			}
			public void SetSorter(SGSorter sorter){
				m_sorter = sorter;
			}
				public SGSorter sorter{
					get{return m_sorter;}
				}
				SGSorter m_sorter;
			public void ToggleAutoSort(bool on){
				m_isAutoSort = on;
				ssm.Focus();
			}
			public bool isAutoSort{
				get{return m_isAutoSort;}
			}
				protected bool m_isAutoSort = true;
		/*	filter	*/
			public List<SlottableItem> FilterItem(List<SlottableItem> items){
				filter.Filter(ref items);
				return items;
			}
			public SGFilter filter{
				get{return m_filter;}
			}
				SGFilter m_filter;
				public void SetFilter(SGFilter filter){
					m_filter = filter;
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
			/* fields	*/
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
			/*	methods	*/
				public override void InitializeStates(){
					Deactivate();
					WaitForAction();
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
			ITransactionSGHandler sgHandler{
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
				ISlottable sb1 = pickedSB;
				ISlottable sb2 = targetSB;
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
				List<ISlottable> newSBs = new List<ISlottable>(toList);
				if(isExpandable)
					sorter.TrimAndOrderSBs(ref newSBs);
				else
					sorter.OrderSBsWithRetainedSize(ref newSBs);
				
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
				SortContextually(ref newSBs);
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

				SortContextually(ref newSBs);
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
					GameObject newSBGO = new GameObject("newSBGO");
					Slottable newSB = newSBGO.AddComponent<Slottable>();
					SSESelStateHandler stateHandler = new SSESelStateHandler();
					newSB.SetSelStateHandler(stateHandler);
					newSB.SetItem(added.item);
					newSB.SetSSM(ssm);
					newSB.Unequip();
					newSB.Defocus();
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
				SortContextually(ref newSBs);
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
				SortContextually(ref thisNewSBs);
				
				UpdateSBs(thisNewSBs);
			}
		//Transaction Utility
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
				GameObject newSBGO = new GameObject("newSBGO");
				Slottable newSB = newSBGO.AddComponent<Slottable>();
				SSESelStateHandler stateHandler = new SSESelStateHandler();
				newSB.SetSelStateHandler(stateHandler);
				newSB.SetItem(addedItem);
				newSB.SetSSM(ssm);
				newSB.Defocus();
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
			void SortContextually(ref List<ISlottable> list){
				if(isAutoSort){
					if(isExpandable)
						sorter.TrimAndOrderSBs(ref list);
					else
						sorter.OrderSBsWithRetainedSize(ref list);
				}
			}
			public void OnActionComplete(){
				onActionCompleteCommand.Execute();
			}
				public ISGCommand onActionCompleteCommand{
					get{return commandsFactory.MakeOnActionCompleteCommand();}
				}
			public void UpdateEquipStatesOnAll(){
				ssm.UpdateEquipStatesOnAll();
			}
			public void OnActionExecute(){
				onActionExecuteCommand.Execute();
			}
				public ISGCommand onActionExecuteCommand{
					get{return commandsFactory.MakeOnActionExecuteCommand();}
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
				SetNewSBs(null);
				SetNewSlots(null);
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
					get{return commandsFactory.MakeInitializeItemsCommand();}
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
					newSB.SetItem((InventoryItemInstance)item);
					newSB.SetSSM(ssm);
					slots[items.IndexOf(item)].sb = newSB;
				}
			}
		/* Hoverable */
			public ITransactionManager tam{
				get{
					if(m_tam != null)
						return m_tam;
					else
						throw new System.InvalidOperationException("tam not set");
				}
			}
				ITransactionManager m_tam;
			public void SetTAM(ITransactionManager tam){m_tam = tam;}
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
			public ISlottable pickedSB{
				get{return taCache.pickedSB;}
			}
			public ISlottable targetSB{
				get{return taCache.targetSB;}
			}
			public IHoverable hoverable{
				get{
					if(m_hoverable == null)
						m_hoverable = new Hoverable(this, taCache);
					return m_hoverable;
				}
			}
				IHoverable m_hoverable;
			public void SetHoverable(IHoverable hoverable){
				m_hoverable = hoverable;
			}
			public void SetHovered(){
				taCache.SetHovered(hoverable);
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
	public interface ISlotGroup: ISlotSystemElement, IHoverable, ISGActStateHandler{
		/*	fields	*/
			Inventory inventory{get;}
			bool isShrinkable{get;}
			bool isExpandable{get;}
			bool isPool{get;}
			ISlottable GetSB(InventoryItemInstance itemInst);
			bool HasItem(InventoryItemInstance invInst);
			void SetSBs(List<ISlottable> sbs);
			Slot GetNewSlot(InventoryItemInstance itemInst);
			bool hasEmptySlot{get;}
			void SetInitSlotsCount(int i);
		/*	Sorter	*/
			void SetSorter(SGSorter sorter);
			void InstantSort();
			bool isAutoSort{get;}
		/*	Filter	*/
			List<SlottableItem> FilterItem(List<SlottableItem> items);
			SGFilter filter{get;}
			bool AcceptsFilter(ISlottable pickedSB);
		/*	Transaction	*/
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
		/* Other */
			List<ISlottable> SwappableSBs(ISlottable pickedSB);
			void InitializeItems();
			void InitSlots(List<SlottableItem> items);
			void InitSBs(List<SlottableItem> items);
		/* Hoverable */
			ISlottable pickedSB{get;}
			ISlottable targetSB{get;}
			IHoverable hoverable{get;}
			void SetHovered();
	}
}
