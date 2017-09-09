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
		
		void EplicitlySpecifyDestSlot(ISlot hoveredSlot);
		ISlot PickedItemSlot();
		bool HasPickedItemSlot();
		void ImplicitlyFocusTargetSlot();
		void ReverseImplicitTargetFocus();
		void Reorder(ISlot toSlot);
	}
	public class SlotGroup : SlotSystemElement, ISlotGroup{
		public SlotGroup(RectTransformFake rectTrans, ISGConstructorArg constArg): base(rectTrans, constArg.UISelStateRepo(), constArg.TapCommand()){
			SSM().InventoryUpdated += OnInventoryUpdated;
			SetIntialSlotCount(constArg.InitSlotCount());
			SetFetchInventoryCommand(constArg.FetchInventoryCommand());
			SetAcceptsItemCommand(constArg.AcceptsItemCommand());
			SetReorderability(constArg.IsReorderable());
			SetIsSwappedOverFilled(constArg.IsSwappedOverFilled());
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
		public bool IsSwappedOverFilled(){
			return _isSwappedOverFilled;
		}
		void SetIsSwappedOverFilled(bool swappedOverFilled){
			_isSwappedOverFilled = swappedOverFilled;
		}
			bool _isSwappedOverFilled;
		public bool IsReorderable(){
			return _isReorderable;
		}
		void SetReorderability(bool on){
			_isReorderable = on;
		}
			bool _isReorderable;
		public virtual bool IsFillable(){
			return HasEmptySlottable();
		}
		bool HasEmptySlottable(){
			foreach(var slottable in Slots())
				if(slottable.IsEmpty())
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
		void SwitchDestinationSlot(ISlot destSlot){
			ISlot prevSlot = DestinationSlot();
			if(destSlot != prevSlot){

				_destinationSlot = destSlot;
				if(prevSlot != null)
					prevSlot.Deselect();
				ISlot newDestSlot = DestinationSlot();
				if(newDestSlot != null)
					newDestSlot.Select();
			}
		}
			ISlot _destinationSlot;
		public virtual void SetUpPickedItemSlotOnPickUp(){
			/*	called when pick up and if is filtered in
				prepare a pickedItemSlot and keep it hidden behind
			*/
			if( !HasPickedItemSlot()){
				ISlot newPickedItemSlot = CreateSlot();
				newPickedItemSlot.SetItem( SSM().PickedItem());
				Debug.Assert(newPickedItemSlot.IsDeactivated() == true);
				HideSlot( newPickedItemSlot);
				newPickedItemSlot.SetID( InvalidID());
				newPickedItemSlot.SetSlotGroup(this);
			}
		}
		ISlot GetFirstEmptySlot(){
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
		void ShowSlots(List<ISlot> slots){

		}
		void ShowSlot(ISlot slot, int index){
			ISlot slotAtIndex = Slots()[index];
			if(slotAtIndex.IsEmpty()){
				//do nothing
			}else{
				slot.GetReadyForSwap( slotAtIndex.Item());
				SubstituteWithEmpty( index);
			}
			slot.SetID( index);
			slot.Show();
		}
		void HideSlots(List<ISlot> slots){

		}
		void HideSlot(ISlot slot){
			/*	Sync Icon dehovering and slot icon shrinking animation
			*/
			if( slot.IsReadyForSwap())
				slot.WaitForSwap();
			slot.Hide();
		}

		public void EplicitlySpecifyDestSlot(ISlot hoveredSlot){
			if(HasPickedItemSlot()){
				SwitchDestinationSlot( PickedItemSlot());
				if(DestinationSlot().IsReadyForSwap()){
					SwitchSwapTarget(hoveredSlot);
				}else
					Reorder(hoveredSlot);
			}else{
				SetUpPickedItemSlotOnPickUp();
				ShowSlot( PickedItemSlot(), hoveredSlot.SlotID());
			}
		}
		void SwitchSwapTarget(ISlot hoveredSlot){
			/*	need revision
			*/
			DestinationSlot().WaitForSwap();
			hoveredSlot.GetReadyForSwap();
		}

		public void ImplicitlyFocusTargetSlot(){
			if( HasIncrementTargetSlot()){
				SwitchDestinationSlot( IncrementTargetSlot());
			}else{
				int showIndex;
				if(IsSwappableAndFillable()){
					if(IsSwappedOverFilled())
						showIndex = SwapTargetSlot().SlotID();
					else
						showIndex = FillTargetSlot().SlotID();
				}else{
					if( IsSwappable())
						showIndex = SwapTargetSlot().SlotID();
					else if( IsFillable())
						showIndex = FillTargetSlot().SlotID();
					else
						throw new InvalidOperationException("this sg should not be filtered in");
				}
				SwitchDestinationSlot( PickedItemSlot());
				ShowSlot( PickedItemSlot(), showIndex);
			}
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
		public void ReverseImplicitTargetFocus(){
			DefocusIncrementTargetSlot();
			DefocusSwapTargetSlot();
			DefocusFillTargetSlot();

			/*	Remove PickedItemSlot if quantity is to be zero
				if so, Update slots indexes
				and make them travel
				if pickedItemSlot is ready for swap, make it wait for swap
			*/
		}
		void DefocusIncrementTargetSlot(){
			if(DestinationSlot() == IncrementTargetSlot()){
				HideSlot( DestinationSlot());
				SwitchDestinationSlot(null);
			}
		}
		void DefocusSwapTargetSlot(){
			if( DestinationSlot().IsReadyForSwap()){
				HideSlot( DestinationSlot());
				SwapTargetSlot().WaitForSwap();
				SwitchDestinationSlot( null);
			}
		}
		void DefocusFillTargetSlo(){
			// if( DestinationSlot() == FillTargetSlot())
		}
		void ReverseSwap(){
			ISlot destSlot = DestinationSlot();
			if(destSlot.IsReadyForSwap()){
				destSlot.WaitForSwap();
			}
			SwitchDestinationSlot(null);
		}
		void ReverseFill(){
			ISlot destSlot = DestinationSlot();
			if(!destSlot.IsReadyForSwap()){
				if(destSlot.PreviewQuantity() <= 0){
					HideSlot(destSlot);
				}
			}
			SwitchDestinationSlot(null);
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
