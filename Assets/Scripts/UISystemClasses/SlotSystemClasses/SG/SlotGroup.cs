using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using System;
namespace UISystem{
	public interface ISlotGroup: ISlotSystemElement{
		void InitializeOnSlotSystemActivate();
		ISorterHandler GetSorterHandler();
			void InstantSort();
			void ToggleAutoSort(bool on);
			bool IsAutoSort();
		List<ISlot> Slots();
		IInventory Inventory();
		void SetInventory(IInventory inventory);
		void OnInventoryUpdated(object source, InventoryEventArgs e);
		bool AcceptsItem(ISlottableItem item);
		bool IsReorderable();
		bool IsReceivable();
		bool IsSwappable(ISlot sb);

		ISlot PickedItemSlot();
		bool HasPickedItemSlot();
		void AddItem(ISlottableItem item);
		void ReduceItem(ISlottableItem item);
		void Reorder(ISlot toSlot);
	}
	public class SlotGroup : SlotSystemElement, ISlotGroup{
		public SlotGroup(RectTransformFake rectTrans, ISGConstructorArg constArg): base(rectTrans, constArg.UISelStateRepo(), constArg.TapCommand()){
			SSM().InventoryUpdated += OnInventoryUpdated;
			SetIntialSlotCount(constArg.InitSlotCount());
			SetFetchInventoryCommand(constArg.FetchInventoryCommand());
			SetAcceptsItemCommand(constArg.AcceptsItemCommand());
			SetReorderability(constArg.IsReorderable());
			SetPositionSlotsCommand(constArg.PositionSBsCommand());
			SetSorterHandler(new SorterHandler(constArg.InitSorter()));
			SetSelStateHandler(new UISelStateHandler(this, constArg.UISelStateRepo()));
		}
		public void InitializeOnSlotSystemActivate(){
			SetUpInventory();
			InitializeSlots();
		}


		/* inventory */
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



		/* Slots */
		public void InitializeSlots(){

		}
		public List<ISlot> Slots(){
			Debug.Assert(_slots != null);
			return _slots;
		}
		void SetSlots(List<ISlot> slots){
			_slots = slots;
		}
			List<ISlot> _slots;
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
		IPositionSBsCommand PositionSlotsCommand(){
			Debug.Assert(_positionSlotsCommand != null);
			return _positionSlotsCommand;
		}
		void SetPositionSlotsCommand(IPositionSBsCommand comm){
			_positionSlotsCommand = comm;
		}
			IPositionSBsCommand _positionSlotsCommand;



		public ISorterHandler GetSorterHandler(){
			Debug.Assert(_sorterHandler != null);
			return _sorterHandler;
		}
		public void SetSorterHandler(ISorterHandler sorterHandler){
			_sorterHandler = sorterHandler;
		}
			ISorterHandler _sorterHandler;
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
				foreach(ISlot sb in Slots())
					yield return sb;
			}
		}
		public override void SetElements(IEnumerable<IUIElement> elements){
			List<ISlot> slottables = new List<ISlot>();
			foreach(var e in elements){
				if(e == null || e is ISlot){
					slottables.Add(e as ISlot);
				}else
					throw new System.ArgumentException("parameter needs to be a collection of only ISlottable or null");
			}
			SetSlots(slottables);
		}


		/*	intrinsic */
		public bool IsReorderable(){
			return _isReorderable;
		}
		void SetReorderability(bool on){
			_isReorderable = on;
		}
			bool _isReorderable;
		public virtual bool IsReceivable(){
			return HasEmptySlottable();
		}
		bool HasEmptySlottable(){
			foreach(var slottable in Slots())
				if(slottable.IsEmpty())
					return true;
			return false;
		}
		public bool IsSwappable(ISlot sb){
			return SwappableSBs(sb).Count == 1;
		}
		public List<ISlot> SwappableSBs(ISlot pickedSB){
			List<ISlot> result = new List<ISlot>();
			foreach(ISlot sb in this){
				if(sb != null){
					if(SlotSystemUtil.SBsAreSwappable(pickedSB, sb))
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
		public override void PerformHoverEnterAction(){
			SSM().SetDestinationSG(this);
		}
		public override void PerformHoverExitAction(){
			/*	do nothing
			*/
		}
		public override bool IsHovered(){
			return SSM().HoveredSSE() == this;
		}

		
		public ISlot PickedItemSlot(){
			foreach(var slot in Slots()){
				if(slot.Item() == SSM().PickedItem())
					return slot;
			}
			return null;
		}
		public bool HasPickedItemSlot(){
			return PickedItemSlot() != null;
		}
		public void AddItem(ISlottableItem pickedItem){
			/*	Find or Create new Slot
				Update slots indexes
				Make them travel
				Set the slot as destination

				swap or fill
			*/
		}
		public void ReduceItem(ISlottableItem pickedItem){
			/*	Remove PickedItemSlot if quantity is to be zero
				if so, Update slots indexes
				and make them travel
				if pickedItemSlot is ready for swap, make it wait for swap
			*/
		}
		public void Reorder(ISlot toSlot){
			/*	from PickedItemSlot to toSlot
			*/
		}

		public void Refresh(){
		}
	}
}
