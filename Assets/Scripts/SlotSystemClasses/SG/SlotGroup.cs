using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;
namespace SlotSystem{
	public class SlotGroup : SlotSystemElement, ISlotGroup{
		public override void InitializeStates(){
			_selStateHandler.Deactivate();
			WaitForAction();
		}
		public void InitializeStateHandlers(){
			SetSelStateHandler(new SGSelStateHandler(GetTAC(), hoverable));
			SetSGActStateHandler(new SGActStateHandler(this));
		}
		public void InitializeSG(){
			ISlotSystemManager ssm = GetSSM();
			SetCommandsRepo(new SGCommandsRepo(this));
			SetSlotsHolder(new SlotsHolder(this));
			SetSBHandler(new SBHandler());
			SetNewSBs(new List<ISlottable>());
			SetHoverable(new Hoverable(ssm.GetTAC()));
			SetSGTAHandler(new SGTransactionHandler(this, ssm.GetTAM()));
			SetSorterHandler(new SorterHandler());
			SetFilterHandler(new FilterHandler());
			SetSBFactory(new SBFactory(ssm));
			InitializeStateHandlers();
			hoverable.SetSSESelStateHandler(_selStateHandler);
		}
		/*	states	*/
			public override ISSESelStateHandler GetSelStateHandler(){
				if(_selStateHandler != null)
					return _selStateHandler;
				else
					throw new InvalidOperationException("selStateHandler not set");
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
				public bool WasActStateNull(){
					return actStateHandler.WasActStateNull();
				}
				public bool IsActStateNull(){
					return actStateHandler.IsActStateNull();
				}
			public void WaitForAction(){
				actStateHandler.WaitForAction();
			}
				public bool IsWaitingForAction(){
					return actStateHandler.IsWaitingForAction();
				}
				public bool WasWaitingForAction(){
					return actStateHandler.WasWaitingForAction();
				}
			public void Revert(){
				actStateHandler.Revert();
			}
				public bool IsReverting(){
					return actStateHandler.IsReverting();
				}
				public bool WasReverting(){
					return actStateHandler.WasReverting();
				}
			public void Reorder(){
				actStateHandler.Reorder();
			}
				public bool IsReordering(){
					return actStateHandler.IsReordering();
				}
				public bool WasReordering(){
					return actStateHandler.WasReordering();
				}
			public void Add(){
				actStateHandler.Add();
			}
				public bool IsAdding(){
					return actStateHandler.IsAdding();
				}
				public bool WasAdding(){
					return actStateHandler.WasAdding();
				}
			public void Remove(){
				actStateHandler.Remove();
			}
				public bool IsRemoving(){
					return actStateHandler.IsRemoving();
				}
				public bool WasRemoving(){
					return actStateHandler.WasRemoving();
				}
			public void Swap(){
				actStateHandler.Swap();
			}
				public bool IsSwapping(){
					return actStateHandler.IsSwapping();
				}
				public bool WasSwapping(){
					return actStateHandler.WasSwapping();
				}
			public void Fill(){
				actStateHandler.Fill();
			}
				public bool IsFilling(){
					return actStateHandler.IsFilling();
				}
				public bool WasFilling(){
					return actStateHandler.WasFilling();
				}
			public void Sort(){
				actStateHandler.Sort();
			}
				public bool IsSorting(){
					return actStateHandler.IsSorting();
				}
				public bool WasSorting(){
					return actStateHandler.WasSorting();
				}
			public void SetAndRunActProcess(ISGActProcess process){
				actStateHandler.SetAndRunActProcess(process);
			}
			public void ExpireActProcess(){
				actStateHandler.ExpireActProcess();
			}
			public ISGActProcess GetActProcess(){
				return actStateHandler.GetActProcess();
			}
			public IEnumeratorFake TransactionCoroutine(){
				bool flag = true;
				foreach(ISlottable sb in this){
					if(sb != null)
					flag &= !sb.GetActProcess().IsRunning();
				}
				if(flag){
					GetActProcess().Expire();
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
			public List<Slot> GetSlots(){
				return slotsHolder.GetSlots();
			}
			public void SetSlots(List<Slot> slots){
				slotsHolder.SetSlots(slots);
			}
			public bool HasEmptySlot(){
				return slotsHolder.HasEmptySlot();
			}
			public List<Slot> GetNewSlots(){
				return slotsHolder.GetNewSlots();
			}
			public void SetNewSlots(List<Slot> newSlots){
				slotsHolder.SetNewSlots(newSlots);
			}
			public Slot GetNewSlot(IInventoryItemInstance item){
				return slotsHolder.GetNewSlot(item);
			}
			public void SetInitSlotsCount(int count){
				slotsHolder.SetInitSlotsCount(count);
			}
			public int GetInitSlotsCount(){
				return slotsHolder.GetInitSlotsCount();
			}
			public void InitSlots(List<IInventoryItemInstance> items){
				slotsHolder.InitSlots(items);
			}
			public void PutSBsInSlots(List<ISlottable> sbs){
				slotsHolder.PutSBsInSlots(sbs);
			}
		/* SB */
			public ISBHandler sbHandler{
				get{
					if(_sbHandler != null)
						return _sbHandler;
					else
						throw new InvalidOperationException("sbHandler not set");
				}
			}
				ISBHandler _sbHandler;
			public void SetSBHandler(ISBHandler sbHandler){
				_sbHandler = sbHandler;
			}
			public List<ISlottable> GetSBs(){
				return sbHandler.GetSBs();
			}
			public void SetSBs(List<ISlottable> sbs){
				sbHandler.SetSBs(sbs);
			}
			public List<ISlottable> GetNewSBs(){
				return sbHandler.GetNewSBs();
			}
			public void SetNewSBs(List<ISlottable> newSBs){
				sbHandler.SetNewSBs(newSBs);
			}
			public void SetSBsActStates(){
				sbHandler.SetSBsActStates();
			}
			public ISlottable GetSB(IInventoryItemInstance item){
				return sbHandler.GetSB(item);
			}
			public bool HasItem(IInventoryItemInstance item){
				return sbHandler.HasItem(item);
			}
			public List<ISlottable> GetEquippedSBs(){
				return sbHandler.GetEquippedSBs();
			}
			public bool IsAllSBActProcDone(){
				return sbHandler.IsAllSBActProcDone();
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
				List<ISlottable> sortedSBs = GetSortedSBsWithoutResize(GetSBs());
				PutSBsInSlots(sortedSBs);
			}
			public void ToggleAutoSort(bool on){
				SetIsAutoSort(on);
				_selStateHandler.Focus();
			}
			public List<ISlottable> GetSortedSBsWithoutResize(List<ISlottable> source){
				return sorterHandler.GetSortedSBsWithoutResize(source);
			}
			public List<ISlottable> GetSortedSBsWithResize(List<ISlottable> source){
				return sorterHandler.GetSortedSBsWithResize(source);
			}
			public SGSorter GetSorter(){
				return sorterHandler.GetSorter();
			}
			public void SetSorter(SGSorter sorter){
				sorterHandler.SetSorter(sorter);
			}
			public void SetIsAutoSort(bool on){
				sorterHandler.SetIsAutoSort(on);
			}
			public bool IsAutoSort(){
				return sorterHandler.IsAutoSort();
			}
		/*	filter	*/
			public IFilterHandler filterHandler{
				get{
					if(_filterHandler != null)
						return _filterHandler;
					else
						throw new InvalidOperationException("filterHandler not set");
				}
			}
				IFilterHandler _filterHandler;
			public void SetFilterHandler(IFilterHandler filterHandler){
				_filterHandler = filterHandler;
			}
			public List<IInventoryItemInstance> FilteredItems(List<IInventoryItemInstance> items){
				return filterHandler.FilteredItems(items);
			}
			public SGFilter GetFilter(){
				return filterHandler.GetFilter();
			}
			public bool AcceptsFilter(ISlottable pickedSB){
				return filterHandler.AcceptsFilter(pickedSB);
			}
			public void SetFilter(SGFilter filter){
				filterHandler.SetFilter(filter);
			}
		/*	SlotSystemElement implementation	*/
			protected override IEnumerable<ISlotSystemElement> elements{
				get{
					foreach(ISlottable sb in GetSBs())
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
				get{return GetSBs();}
			}
			public override bool Contains(ISlotSystemElement element){
				if(element is ISlottable)
					return GetSBs().Contains((ISlottable)element);
				return false;
			}
			public void FocusSBs(){
				foreach(ISlottable sb in this){
					if(sb != null){
						ISSESelStateHandler sbSelStateHandler = sb.GetSelStateHandler();
						sb.Refresh();
						if(sb.PassesPrePickFilter())
							sbSelStateHandler.Focus();
						else
							sbSelStateHandler.Defocus();
					}
				}
			}
			public void DefocusSBs(){
				foreach(ISlottable sb in this){
					if(sb != null){
						ISSESelStateHandler sbSelStateHandler = sb.GetSelStateHandler();
						sb.Refresh();
						sbSelStateHandler.Defocus();
					}
				}
			}
			public override void SetHierarchy(){
				InitializeItems();
			}
		/* Hoverable */
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
			public ITransactionCache GetTAC(){
				return hoverable.GetTAC();
			}
			public void SetTACache(ITransactionCache taCache){
				hoverable.SetTACache(taCache);
			}
			public void OnHoverEnter(){
				hoverable.OnHoverEnter();
			}
			public void OnHoverExit(){
				hoverable.OnHoverExit();
			}
			public bool IsHovered(){
				return hoverable.IsHovered();
			}
			public void SetSSESelStateHandler(ISSESelStateHandler handler){
				//removed
			}
		/*	intrinsic */
			public IInventory GetInventory(){
				if(_inventory != null)
					return _inventory;
				else
					throw new InvalidOperationException("inventory not set");
			}
			public void SetInventory(IInventory inv){
				_inventory = inv;
				inv.SetSG(this);
			}
				IInventory _inventory;
			public bool IsShrinkable(){
				return _isShrinkable;
			}
				bool _isShrinkable;
			public bool IsExpandable(){
				return _isExpandable;
			}
				bool _isExpandable;
			public bool IsPool(){
				ISlotSystemManager ssm = GetSSM();
				return ssm.GetPoolBundle().ContainsInHierarchy(this);
			}
			public bool IsSGE(){
				ISlotSystemManager ssm = GetSSM();
				return ssm.GetEquipBundle().ContainsInHierarchy(this);
			}
			public bool IsSGG(){
				ISlotSystemManager ssm = GetSSM();
				foreach(ISlotSystemBundle gBundle in ssm.GetOtherBundles()){
					if(gBundle.ContainsInHierarchy(this))
						return true;
				}
				return false;
			}
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
			public void InspectorSetUp(IInventory inv, SGFilter filter, SGSorter sorter, int initSlotsCount){
				SetInventory(inv);
				SetFilter(filter);
				SetSorter(sorter);
				SetInitSlotsCount(initSlotsCount);
				_isExpandable = initSlotsCount == 0;
			}
			public void InitializeItems(){
				initItemsCommand.Execute();
			}
				public ISGCommand initItemsCommand{
					get{return commandsRepo.GetInitializeItemsCommand();}
				}
			public void InitSBs(List<IInventoryItemInstance> items){
				List<Slot> slots = GetSlots();
				while(slots.Count < items.Count){
					items.RemoveAt(slots.Count);
				}
				foreach(IInventoryItemInstance item in items){
					ISlottable newSB = CreateSB(item);
					slots[items.IndexOf(item)].sb = newSB;
				}
			}
		/* SBFactory */
			public ISBFactory sbFactory{
				get{
					if(_sbFactory != null)
						return _sbFactory;
					else
						throw new InvalidOperationException("sbFactory not set");
				}
			}
				ISBFactory _sbFactory;
				public void SetSBFactory(ISBFactory sbFactory){
					_sbFactory = sbFactory;
				}
			public ISlottable CreateSB(IInventoryItemInstance item){
				return sbFactory.CreateSB(item);
			}
		/* Transaction */
			public ISGTransactionHandler sgTAHandler{
				get{
					if(_sgTAHandler != null)
						return _sgTAHandler;
					else
						throw new InvalidOperationException("sgTAHandler not set");
				}
			}
				ISGTransactionHandler _sgTAHandler;
			public void SetSGTAHandler(ISGTransactionHandler sgTAHandler){
				_sgTAHandler = sgTAHandler;
			}
			public List<ISlottable> ReorderedNewSBs(){
				return sgTAHandler.ReorderedNewSBs();
			}
			public List<ISlottable> SortedNewSBs(){
				return sgTAHandler.SortedNewSBs();
			}
			public List<ISlottable> FilledNewSBs(){
				return sgTAHandler.FilledNewSBs();
			}
			public List<ISlottable> SwappedNewSBs(){
				return sgTAHandler.SwappedNewSBs();
			}
			public List<ISlottable> AddedNewSBs(){
				return sgTAHandler.AddedNewSBs();
			}
			public List<ISlottable> RemovedNewSBs(){
				return sgTAHandler.RemovedNewSBs();
			}
			public void UpdateSBs(){
				sgTAHandler.UpdateSBs();
			}
			public void SetSBsFromSlotsAndUpdateSlotIDs(){
				sgTAHandler.SetSBsFromSlotsAndUpdateSlotIDs();
			}
			public void ReportTAComp(){
				sgTAHandler.ReportTAComp();
			}
			public void RevertAndUpdateSBs(){
				SetNewSBs(GetSBs());
				SetSBsActStates();
				CreateNewSlots();
			}
			public void ReadySBsForTransaction(List<ISlottable> newSBs){
				SetNewSBs(newSBs);
				SetSBsActStates();
				List<ISlottable> allSBs = AllSBs(GetSBs(), newSBs);
				SetSBs(allSBs);
				CreateNewSlots();
			}
			public List<ISlottable> AllSBs(List<ISlottable> sbs, List<ISlottable> newSBs){
				List<ISlottable> allSBs = new List<ISlottable>(sbs);
				List<ISlottable> added = new List<ISlottable>();
				foreach(var sb in newSBs)
					if(!sbs.Contains(sb)) 
						added.Add(sb);
				allSBs.AddRange(added);
				return allSBs;
			}
			public void CreateNewSlots(){
				List<Slot> newSlots = new List<Slot>();
				for(int i = 0; i < GetNewSBs().Count; i++){
					Slot newSlot = new Slot();
					newSlots.Add(newSlot);
				}
				SetNewSlots(newSlots);
			}
			public void OnActionComplete(){
				onActionCompleteCommand.Execute();
			}
				public ISGCommand onActionCompleteCommand{
					get{return commandsRepo.GetOnActionCompleteCommand();}
				}
			public void UpdateEquipStatesOnAll(){
				ISlotSystemManager ssm = GetSSM();
				ssm.UpdateEquipInvAndAllSBsEquipState();
			}
			public void OnActionExecute(){
				onActionExecuteCommand.Execute();
			}
				public ISGCommand onActionExecuteCommand{
					get{return commandsRepo.GetOnActionExecuteCommand();}
				}
			public void SyncEquipped(IInventoryItemInstance item, bool equipped){
				ISlotSystemManager ssm = GetSSM();
				IInventory inventory = GetInventory();
				if(equipped)
					inventory.Add(item);
				else
					inventory.Remove(item);
				ssm.MarkEquippedInPool(item, equipped);
				ssm.SetEquippedOnAllSBs(item, equipped);
			}
	}
	public interface ISlotGroup: ISlotSystemElement, IHoverable, ISGActStateHandler, ISlotsHolder, ISorterHandler, IFilterHandler, ISBFactory, ISBHandler, ISGTransactionHandler{
		/*	instrinsic	*/
			IInventory GetInventory();
			bool IsShrinkable();
			bool IsExpandable();
			bool IsPool();
			List<ISlottable> SwappableSBs(ISlottable pickedSB);
			void InitializeItems();
			void InitSBs(List<IInventoryItemInstance> items);
		/*	integration	*/
			void InstantSort();
			void ToggleAutoSort(bool on);
		/*	Transaction	*/
			void ReadySBsForTransaction(List<ISlottable> newSBs);
			void RevertAndUpdateSBs();
			void OnActionComplete();
			void UpdateEquipStatesOnAll();
			void OnActionExecute();
			void SyncEquipped(IInventoryItemInstance item, bool equipped);
	}
}
