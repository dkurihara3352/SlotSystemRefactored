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
		bool IsFillable();
		bool IsSwappable();
		
		void TearDownAsDestSG();
		void SetUpAsDestSG();
		ISlot PickedItemSlot();
		bool HasPickedItemSlot();
		ISlot CalculateDestSlot( ISlot hoveredSlot);
		void SwitchDestinationSlot( ISlot destSlot);
		void Reorder(ISlot toSlot);
	}
	public class SlotGroup : SlotSystemElement, ISlotGroup{
		public SlotGroup(RectTransformFake rectTrans, ISGConstructorArg constArg): base(rectTrans, constArg.UISelStateRepo(), constArg.TapCommand()){
			SSM().InventoryUpdated += OnInventoryUpdated;
			SetIntialSlotCount(constArg.InitSlotCount());
			SetFetchInventoryCommand(constArg.FetchInventoryCommand());
			SetAcceptsItemCommand(constArg.AcceptsItemCommand());
			SetReorderability(constArg.IsReorderable());
			SetIsExchangedOverFilled(constArg.IsExchangedOverFilled());
			SetIsExchangedOverReordered( constArg.IsExchangedOverReordered());
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
		public bool IsExchangedOverFilled(){
			return _isExchangedOverFilled;
		}
			void SetIsExchangedOverFilled(bool toggle){
				_isExchangedOverFilled = toggle;
			}
			bool _isExchangedOverFilled;
		public bool IsExchangedOverReordered(){
			return _isExchangedOverReordered;
		}
			void SetIsExchangedOverReordered( bool toggle){
				_isExchangedOverReordered = toggle;
			}
			bool _isExchangedOverReordered;
		
		public bool IsReorderable(){
			return _isReorderable;
		}
		void SetReorderability(bool on){
			_isReorderable = on;
		}
			bool _isReorderable;
		public virtual bool IsFillable(){
			return HasEmptySlot();
		}
		bool HasEmptySlot(){
			foreach(var slot in Slots())
				if(slot.IsEmpty())
					return true;
			return false;
		}
		public bool IsSwappable(){
			return SwappableSlots().Count == 1;
		}
		public List<ISlot> SwappableSlots(){
			List<ISlot> result = new List<ISlot>();
			ISlotGroup sourceSG = SSM().SourceSG();
			if(sourceSG != this && AcceptsItem( SSM().PickedItem())){
				foreach(ISlot slot in this){
					if(slot != null){
						if(sourceSG.AcceptsItem(slot.Item()))
							result.Add(slot);
					}
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
		public override bool IsHovered(){
			return SSM().HoveredSSE() == this;
		}


		ISlot DestinationSlot(){
			return _destinationSlot;
		}
		public void SwitchDestinationSlot(ISlot destSlot){
			ISlot prevSlot = DestinationSlot();
			if(destSlot != prevSlot){

				_destinationSlot = destSlot;

				if(prevSlot != null)
					prevSlot.TearDownAsTarget();

				ISlot newDestSlot = DestinationSlot();
				if(newDestSlot != null){
					if( newDestSlot.IsIncrementable())
						newDestSlot.SetUpAsIncrementTarget();
					else if( newDestSlot.IsEmpty())
						newDestSlot.SetUpAsFillTarget();
					else{
						if( IsDestSlotSet() && !IsExchangedOverReordered())
							Reorder( destSlot);
						else
							newDestSlot.SetUpAsExchangeTarget();
					}
				}
			}
		}
			ISlot _destinationSlot;
			bool IsDestSlotSet(){
				return DestinationSlot() != null;
			}
		public ISlot CalculateDestSlot( ISlot hoveredSlot){
			ISlot destSlot;
			if( HasIncrementTargetSlot()){
				destSlot = IncrementTargetSlot();
			}else{
				if(IsSwappableAndFillable()){
					if(IsExchangedOverFilled())
						destSlot = SwapTargetSlot();
					else
						destSlot = FillTargetSlot();
				}else{
					if( IsSwappable())
						destSlot = SwapTargetSlot();
					else if( IsFillable())
						destSlot = FillTargetSlot();
					else
						throw new InvalidOperationException("this sg should not be filtered in");
				}
			}
			return destSlot;
		}
		ISlot FirstEmptySlot(){
			foreach(var slot in Slots()){
				if(slot.IsEmpty())
					return slot;
			}
			return null;
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
		public void TearDownAsDestSG(){
			Deselect();
		}
		public void SetUpAsDestSG(){
			Select();
		}
		bool HasIncrementTargetSlot(){
			return IncrementTargetSlot() != null;
		}
		ISlot IncrementTargetSlot(){
			if( HasPickedItemSlot())
				if(PickedItemSlot().IsStackable())
					return PickedItemSlot();
			return null;
		}
		bool IsSwappableAndFillable(){
			return IsSwappable() && IsFillable();
		}
		ISlot SwapTargetSlot(){
			return SwappableSlots()[0];
		}
		ISlot FillTargetSlot(){
			return FirstEmptySlot();
		}
		public void Reorder(ISlot toSlot){
			/*	from PickedItemSlot to toSlot
			*/
		}

		public void Refresh(){
			SwitchDestinationSlot(null);
		}
	}
}
