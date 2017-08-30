using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;
namespace UISystem{
	public class SlotGroup : SlotSystemElement, ISlotGroup{
		public SlotGroup(RectTransformFake rectTrans, IFetchInventoryCommand fetchInventoryCommand, IAcceptsItemCommand acceptsItemCommand, ISorterHandler sorterHandler, int initSlotCount, IFilterHandler filterHandler, bool isReorderable): base(rectTrans, new EmptySSECommandsRepo()){
			SSM().InventoryUpdated += OnInventoryUpdated;
			SetIntialSlotCount(initSlotCount);
			SetFetchInventoryCommand(fetchInventoryCommand);
			SetAcceptsItemCommand(acceptsItemCommand);
			SetCreateSlotCommand(new CreateSlotCommand());
			SetPositionSlotsCommand(new PositionSlotsCommand(this));
			SetSorterHandler(sorterHandler);
			SetReorderability(isReorderable);
			SetSelStateHandler(new SGSelStateHandler(new SGSelCoroutineRepo(), this));
			SetActStateHandler(new SGActStateHandler(this));
		}
		public void InitializeOnSlotSystemActivate(){
			SetUpInventory();
			InitializeSlots();
			InitializeSlottables();
		}
		void SetUpInventory(){
			IInventory inventory = FetchInventoryCommand().FetchInventory();
			SetInventory(inventory);
		}
		IFetchInventoryCommand FetchInventoryCommand(){
			Debug.Assert(_fetchInventoryCommand != null);
			return _fetchInventoryCommand;
		}
			IFetchInventoryCommand _fetchInventoryCommand;
		void SetFetchInventoryCommand(IFetchInventoryCommand comm){
			_fetchInventoryCommand = comm;
		}
		void InitializeSlots(){
			List<ISlot> slots = CreateSlots();
			SetSlots(slots);
			PositionSlots();
			UpdateSlotIDs();
		}
		public List<ISlot> Slots(){
			Debug.Assert(_slots != null);
			return _slots;
		}
		void SetSlots(List<ISlot> slots){
			_slots = slots;
		}
			List<ISlot> _slots;
		List<ISlot> CreateSlots(){
			List<ISlot> result;
			if(IsResizable())
				result = CreateSlotsForEachInventoryItems();
			else
				result = CreateSlotsByInitialSlotCount();
			return result;
		}
		List<ISlot> CreateSlotsForEachInventoryItems(){
			List<ISlot> result = new List<ISlot>();
			foreach(var item in Inventory().Items()){
				Slot slot = CreateSlot();
				result.Add(slot);
			}
			return result;
		}
		List<ISlot> CreateSlotsByInitialSlotCount(){
			List<ISlot> result = new List<ISlot>();
			for(int i = 0; i < InitialSlotCount(); i ++)
				result.Add(CreateSlotCommand().CreateSlot());
			return result;
		}
		Slot CreateSlot(){
			return CreateSlotCommand().CreateSlot();
		}
		ICreateSlotCommand CreateSlotCommand(){
			Debug.Assert(_createSlotCommand != null);
			return _createSlotCommand;
		}
			ICreateSlotCommand _createSlotCommand;
		void SetCreateSlotCommand(ICreateSlotCommand comm){
			_createSlotCommand = comm;
		}
		int InitialSlotCount(){
			Debug.Assert(_initialSlotCount != -1);
			return _initialSlotCount;
		}
			int _initialSlotCount = -1;
		void SetIntialSlotCount(int count){
			_initialSlotCount = count;
		}
		void PositionSlots(){
			PositionSlotsCommand().Execute();
		}
		IPositionSlotsCommand PositionSlotsCommand(){
			Debug.Assert(_positionSlotsCommand != null);
			return _positionSlotsCommand;
		}
		void SetPositionSlotsCommand(IPositionSlotsCommand comm){
			_positionSlotsCommand = comm;
		}
			IPositionSlotsCommand _positionSlotsCommand;
		void UpdateSlotIDs(){
			int index = 0;
			foreach(var slot in Slots()){
				slot.SetID(index ++);
			}
		}
		public IInventory Inventory(){
			Debug.Assert(_inventory != null);
			return _inventory;
		}
		public void SetInventory(IInventory inv){
			_inventory = inv;
			inv.SetSG(this);
		}
			IInventory _inventory;
		public void UpdateInventory(){
			SSM().UpdateInventory(Inventory());
		}
		public void OnInventoryUpdated(object ssm, InventoryEventArgs e){
			OnInventoryUpdatedCommand().Execute(e.inventory);
		}
		IOnInventoryUpdatedCommand OnInventoryUpdatedCommand(){
			Debug.Assert(_onInventoryUpdatedCommand != null);
			return _onInventoryUpdatedCommand;
		}
			IOnInventoryUpdatedCommand _onInventoryUpdatedCommand;
		public void InitializeSlottables(){

		}
		public ISorterHandler GetSorterHandler(){
			Debug.Assert(_sorterHandler != null);
			return _sorterHandler;
		}
		public void SetSorterHandler(ISorterHandler sorterHandler){
			_sorterHandler = sorterHandler;
		}
			ISorterHandler _sorterHandler;
		/*	sorter	*/
			public bool IsAutoSort(){
				return GetSorterHandler().IsAutoSort();
			}
			public void InstantSort(){
				
			}
			public void ToggleAutoSort(bool on){
				GetSorterHandler().SetIsAutoSort(on);
				SSM().Refresh();
			}
		/*	SlotSystemElement implementation	*/
			protected override IEnumerable<IUIElement> elements{
				get{
					foreach(ISlot slot in Slots())
						yield return slot;
				}
			}
			public override void SetElements(IEnumerable<IUIElement> elements){
				List<ISlot> slots = new List<ISlot>();
				foreach(var e in elements){
					if(e == null || e is ISlot){
						slots.Add(e as ISlot);
					}else
						throw new System.ArgumentException("parameter needs to be a collection of only ISlot or null");
				}
				SetSlots(slots);
			}
		/*	intrinsic */
		public bool IsResizable(){
			return InitialSlotCount() == 0;
		}
		public bool IsReorderable(){
			return _isReorderable;
		}
		void SetReorderability(bool on){
			_isReorderable = on;
		}
			bool _isReorderable;
		public bool IsReceivable(){
			return IsResizable() || HasEmptySlot();
		}
		bool HasEmptySlot(){
			if(!IsResizable()){
				foreach(var slot in Slots())
					if(slot.IsEmpty())
						return true;
			}
			return false;
		}
		public bool IsSwappable(ISlottable sb){
			return SwappableSBs(sb).Count == 1;
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
		public bool AcceptsItem(ISlottableItem item){
			Debug.Assert(_acceptsItemCommand != null);
			return AcceptsItemCommand().AcceptsItem(item);
		}
		IAcceptsItemCommand AcceptsItemCommand(){
			Debug.Assert(_acceptsItemCommand != null);
			return _acceptsItemCommand;
		}
		void SetAcceptsItemCommand(IAcceptsItemCommand comm){
			_acceptsItemCommand = comm;
		}
			IAcceptsItemCommand _acceptsItemCommand;
		public void Refresh(){

		}
		public ISGActStateHandler ActStateHandler(){
			Debug.Assert(_actStateHandler != null);
			return _actStateHandler;
		}
		void SetActStateHandler(ISGActStateHandler handler){
			_actStateHandler = handler;
		}
			ISGActStateHandler _actStateHandler;
		public void WaitForAction(){
			ActStateHandler().WaitForAction();
		}
		public void Resize(){
			ActStateHandler().Resize();
		}
	}
	public interface ISlotGroup: ISlotSystemElement{
		void InitializeOnSlotSystemActivate();
		ISGActStateHandler ActStateHandler();
			void WaitForAction();
			void Resize();
		ISorterHandler GetSorterHandler();
			void InstantSort();
			void ToggleAutoSort(bool on);
			bool IsAutoSort();
		IInventory Inventory();
		void SetInventory(IInventory inventory);
		void OnInventoryUpdated(object source, InventoryEventArgs e);
		bool AcceptsItem(ISlottableItem item);
		bool IsResizable();
		bool IsReorderable();
		bool IsReceivable();
		bool IsSwappable(ISlottable sb);
	}
}
