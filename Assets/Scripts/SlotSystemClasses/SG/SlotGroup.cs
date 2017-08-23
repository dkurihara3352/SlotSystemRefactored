using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;
namespace SlotSystem{
	public class SlotGroup : SlotSystemElement, ISlotGroup{
		public override void InitializeStates(){
			_selStateHandler.Deactivate();
			GetSGActStateHandler().WaitForAction();
		}
		public void InitializeStateHandlers(){
			IHoverable hoverable = GetHoverable();
			SetSelStateHandler(new SGSelStateHandler(GetTAC(), hoverable));
			SetSGActStateHandler(new SGActStateHandler(this));
		}
		public void InitializeSG(){
			ISlotSystemManager ssm = GetSSM();
			SetCommandsRepo(new SGCommandsRepo(this));
			SetSlotsHolder(new SlotsHolder(this));
			SetSBHandler(new SBHandler());
			SetHoverable(new Hoverable(ssm.GetTAC()));
			SetSGTAHandler(new SGTransactionHandler(this, ssm.GetTAM()));
			SetSorterHandler(new SorterHandler());
			SetFilterHandler(new FilterHandler());
			SetSBFactory(new SBFactory(ssm));
			InitializeStateHandlers();
			GetHoverable().SetSSESelStateHandler(_selStateHandler);
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
			public ISGActStateHandler GetSGActStateHandler(){
				if(_actStateHandler != null)
					return _actStateHandler;
				else
					throw new System.InvalidOperationException("actStateHandler not set");
			}
				ISGActStateHandler _actStateHandler;
			public void SetSGActStateHandler(ISGActStateHandler handler){
				_actStateHandler = handler;
			}
			public IEnumeratorFake TransactionCoroutine(){
				bool flag = true;
				foreach(ISlottable sb in this){
					if(sb != null)
					flag &= !sb.IsActProcessRunning();
				}
				if(flag){
					GetSGActStateHandler().ExpireActProcess();
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
			public ISlotsHolder GetSlotsHolder(){
				if(_slotsHolder != null)
					return _slotsHolder;
				else
					throw new InvalidOperationException("slotsHolder not set");
			}
				ISlotsHolder _slotsHolder;
			public void SetSlotsHolder(ISlotsHolder slotsHolder){
				_slotsHolder = slotsHolder;
			}
			public bool HasEmptySlot(){
				return GetSlotsHolder().HasEmptySlot();
			}
		/* SB */
			public ISBHandler GetSBHandler(){
				if(_sbHandler != null)
					return _sbHandler;
				else
					throw new InvalidOperationException("sbHandler not set");
			}
			public void SetSBHandler(ISBHandler sbHandler){
				_sbHandler = sbHandler;
			}
				ISBHandler _sbHandler;
			public ISlottable GetSB(IInventoryItemInstance item){
				return GetSBHandler().GetSB(item);
			}
			public bool HasItem(IInventoryItemInstance item){
				return GetSBHandler().HasItem(item);
			}
		/*	sorter	*/
			public ISorterHandler GetSorterHandler(){
				if(_sorterHandler != null)
					return _sorterHandler;
				else
					throw new InvalidOperationException("sorterHandler not set");
			}
			public void SetSorterHandler(ISorterHandler sorterHandler){
				_sorterHandler = sorterHandler;
			}
				ISorterHandler _sorterHandler;
			public bool IsAutoSort(){
				return GetSorterHandler().IsAutoSort();
			}
			public void InstantSort(){
				List<ISlottable> sbs = GetSBHandler().GetSBs();
				List<ISlottable> sortedSBs = GetSorterHandler().GetSortedSBsWithoutResize(sbs);
				GetSlotsHolder().PutSBsInSlots(sortedSBs);
			}
			public void ToggleAutoSort(bool on){
				GetSorterHandler().SetIsAutoSort(on);
				GetSelStateHandler().Focus();
			}
		/*	filter	*/
			public IFilterHandler GetFilterHandler(){
				if(_filterHandler != null)
					return _filterHandler;
				else
					throw new InvalidOperationException("filterHandler not set");
			}
			public void SetFilterHandler(IFilterHandler filterHandler){
				_filterHandler = filterHandler;
			}
				IFilterHandler _filterHandler;
			public bool AcceptsFilter(ISlottable sb){
				return GetFilterHandler().AcceptsFilter(sb);
			}
		/*	SlotSystemElement implementation	*/
			protected override IEnumerable<ISlotSystemElement> elements{
				get{
					foreach(ISlottable sb in GetSBHandler().GetSBs())
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
				GetSBHandler().SetSBs(sbs);
			}
			public override bool Contains(ISlotSystemElement element){
				List<ISlottable> sbs = GetSBHandler().GetSBs();
				if(element is ISlottable)
					return sbs.Contains((ISlottable)element);
				return false;
			}
			public void FocusSBs(){
				foreach(ISlottable sb in this){
					if(sb != null){
						sb.Refresh();
						if(sb.PassesPrePickFilter())
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
			public override void SetHierarchy(){
				InitializeItems();
			}
		/* Hoverable */
			public IHoverable GetHoverable(){
				if(_hoverable != null)
					return _hoverable;
				else
					throw new InvalidOperationException("hoverable not set");
			}
			public void SetHoverable(IHoverable hoverable){
				_hoverable = hoverable;
			}
				IHoverable _hoverable;
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
			public bool AllowsOneWayTransaction(){
				return _isShrinkable;
			}
				bool _isShrinkable;
			public bool IsResizable(){
				return _isExpandable;
			}
				bool _isExpandable;
			public bool IsPool(){
				return GetSSM().PoolBundleContains(this);
			}
			public bool IsSGE(){
				return GetSSM().EquipBundleContains(this);
			}
			public bool IsSGG(){
				return GetSSM().OtherBundlesContain(this);
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
				GetSGActStateHandler().WaitForAction();
				GetSBHandler().SetNewSBs(new List<ISlottable>());
				GetSlotsHolder().SetNewSlots(new List<Slot>());
			}
			public void InspectorSetUp(IInventory inv, SGFilter filter, SGSorter sorter, int initSlotsCount){
				SetInventory(inv);
				GetFilterHandler().SetFilter(filter);
				GetSorterHandler().SetSorter(sorter);
				GetSlotsHolder().SetInitSlotsCount(initSlotsCount);
				_isExpandable = initSlotsCount == 0;
			}
			public void InitializeItems(){
				initItemsCommand.Execute();
			}
				public ISGCommand initItemsCommand{
					get{return commandsRepo.GetInitializeItemsCommand();}
				}
			public void InitSBs(List<IInventoryItemInstance> items){
				ISlotsHolder slotsHolder = GetSlotsHolder();
				slotsHolder.MakeSureSlotsAreReady(items);
				List<ISlottable> newSBs = GetSBFactory().CreateSBs(items);
				slotsHolder.PutSBsInSlots(newSBs);
			}

		/* SBFactory */
			public ISBFactory GetSBFactory(){
				if(_sbFactory != null)
					return _sbFactory;
				else
					throw new InvalidOperationException("sbFactory not set");
			}
			public void SetSBFactory(ISBFactory sbFactory){
				_sbFactory = sbFactory;
			}
				ISBFactory _sbFactory;
		/* Transaction */
			public ITransactionCache GetTAC(){
				if(_taCache != null)
					return _taCache;
				else
					throw new InvalidOperationException("taCache not set");
			}
			public void SetTAC(ITransactionCache taCache){
				_taCache = taCache;
			}
				ITransactionCache _taCache;
			public ISGTransactionHandler GetSGTAHandler(){
				if(_sgTAHandler != null)
					return _sgTAHandler;
				else
					throw new InvalidOperationException("sgTAHandler not set");
			}
			public void SetSGTAHandler(ISGTransactionHandler sgTAHandler){
				_sgTAHandler = sgTAHandler;
			}
				ISGTransactionHandler _sgTAHandler;
			public void RevertAndUpdateSBs(){
				ISBHandler sbHandler = GetSBHandler();
				sbHandler.SetNewSBs(sbHandler.GetSBs());
				sbHandler.SetSBsActStates();
				
				CreateAndSetNewSlots(sbHandler.GetNewSBsCount());
			}
			public void ReadySBsForTransaction(List<ISlottable> newSBs){
				ISBHandler sbHandler = GetSBHandler();
				sbHandler.SetNewSBs(newSBs);
				sbHandler.SetSBsActStates();
				List<ISlottable> allSBs = GetAllSBs(sbHandler.GetSBs(), newSBs);
				sbHandler.SetSBs(allSBs);

				CreateAndSetNewSlots(newSBs.Count);
			}
			public List<ISlottable> GetAllSBs(List<ISlottable> sbs, List<ISlottable> newSBs){
				List<ISlottable> allSBs = new List<ISlottable>(sbs);
				List<ISlottable> added = new List<ISlottable>();
				foreach(var sb in newSBs)
					if(!sbs.Contains(sb)) 
						added.Add(sb);
				allSBs.AddRange(added);
				return allSBs;
			}
			public void CreateAndSetNewSlots(int count){
				ISlotsHolder slotsHolder = GetSlotsHolder();
				List<Slot> newSlots = slotsHolder.CreateSlots(count);
				slotsHolder.SetNewSlots(newSlots);
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
	public interface ISlotGroup: ISlotSystemElement{
			ITransactionCache GetTAC();
			ISGActStateHandler GetSGActStateHandler();
			IHoverable GetHoverable();
			ISlotsHolder GetSlotsHolder();
				bool HasEmptySlot();
			ISorterHandler GetSorterHandler();
				bool IsAutoSort();
			IFilterHandler GetFilterHandler();
				bool AcceptsFilter(ISlottable sb);
			ISBFactory GetSBFactory();
			ISBHandler GetSBHandler();
				ISlottable GetSB(IInventoryItemInstance item);
				bool HasItem(IInventoryItemInstance item);
			ISGTransactionHandler GetSGTAHandler();
		/*	instrinsic	*/
			IInventory GetInventory();
			bool AllowsOneWayTransaction();
			bool IsResizable();
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
