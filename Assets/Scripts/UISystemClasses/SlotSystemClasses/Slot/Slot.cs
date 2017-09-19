using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace UISystem{
	public interface ISlot: ISlotSystemElement{
		ISlotGroup SlotGroup();

		ISlotActStateEngine ActStateEngine();
		void WaitForAction();
		void PickUp();

		ISlottableItem Item();
		bool IsEmpty();
		void SetItem(ISlottableItem item);
		int ItemID();
		bool IsStackable();
		bool IsIncrementable();
		int Quantity();
		void SetQuantity(int quantity);
		int PickedQuantity();

		int PreviewQuantity();

		void ChangeItemInstantlyTo( ISlottableItem item);
		void ChangeItemGraduallyTo( ISlottableItem item);
		
		void SetUpAsIncrementTarget();
		void SetUpAsFillTarget();
		void SetUpAsExchangeTarget();
		void TearDownAsTarget();

		void GetReadyForIncrement();
		void WaitForIncrement();

		void Refresh();
		void Destroy();
	}
	public class Slot : SlotSystemElement, ISlot{
		public Slot(RectTransformFake rectTrans, IUISelStateRepo selStateRepo, ITapCommand tapCommand, ISlottableItem item, bool leavesGhost): base(rectTrans, selStateRepo, tapCommand){
			SetItem(item);
			SetActStateEngine( new SlotActStateEngine( this));
			SetItemVisualUpdateEngine( new ItemVisualUpdateEngine());
			SetGhostificationEngine( new GhostificationEngine());
			SetQuantityVisualUpdateEngine( new QuantityVisualUpdateEngine());
			InitializeStates();
			SetLeavesGhost(leavesGhost);
		}
		public override void InitializeStates(){
			MakeUnselectable();
			WaitForAction();
			WaitForItemVisualUpdate();
			WaitForIncrement();
			WaitForQuantityVisualUpdate();
			Unghostify();
		}
		public void Refresh(){
			WaitForAction();
			WaitForExchange();
			WaitForIncrement();
			WaitForItemVisualUpdate();
			WaitForQuantityVisualUpdate();
			SetHoverIcon( null);
			Unghostify();
		}
		public ISlotGroup SlotGroup(){
			return (ISlotGroup)Parent();
		}
		public void Destroy(){
			if(LeavesGhost()){

			}else{

			}
		}
		/*	Action State */
			public ISlotActStateEngine ActStateEngine(){
				Debug.Assert(_actStateEngine != null);
				return _actStateEngine;
			}
				ISlotActStateEngine _actStateEngine;
			public void SetActStateEngine(ISlotActStateEngine actStateEngine){
				_actStateEngine = actStateEngine;
			}
			public void WaitForAction(){
				ActStateEngine().WaitForAction();
			}
			public void PickUp(){
				SSM().SetPicked( this);
				SSM().PostPickFilter();
				ISlottableItem slotIconItem = CreateSlotIconItem(Item(), 1);
				IHoverIcon draggedIcon = new HoverIcon( slotIconItem);
				SetHoverIcon( draggedIcon);
				HoverIcon().Hover();
				UpdatePreviewQuantity();
			}
			protected virtual ISlottableItem CreateSlotIconItem( ISlottableItem item, int quantity){
				ISlottableItem newItem = new SlottableItem( quantity, item.IsStackable(), item.ItemID());
				return newItem;
			}
			public override void OnPointerDown(){
				base.OnPointerDown();
				ActStateEngine().OnPointerDown();
			}
			public override void OnPointerUp(){
				base.OnPointerUp();
				ActStateEngine().OnPointerUp();
			}
			public override void OnEndDrag(){
				base.OnEndDrag();
				ActStateEngine().OnEndDrag();
			}
			public override void OnDeselected(){
				base.OnDeselected();
				ActStateEngine().OnDeselected();
			}


		/* Item Handling */
			public ISlottableItem Item(){
				Debug.Assert(_item != null);
				return _item;
			}
				ISlottableItem _item;
			public void SetItem(ISlottableItem item){
				_item = item;
			}
			public int PickedQuantity(){
				if(HoverIcon() != null)
					return HoverIcon().ItemQuantity();
				else
					return 0;
			}
			public virtual bool IsStackable(){
				return Item().IsStackable();
			}
			public bool IsIncrementable(){
				return Item() == SSM().PickedItem() && IsStackable();
			}
			public int Quantity(){
				return Item().Quantity();
			}
			public void SetQuantity(int quant){
				Item().SetQuantity(quant);
			}
			public int ItemID(){
				return Item().ItemID();
			}
			public bool IsEmpty(){
				return (Item() is EmptySlottableItem);
			}


			public int PreviewQuantity(){
				return _previewQuantity;
			}
			void SetPreveiewQuantity( int previewQua){
				_previewQuantity = previewQua;
			}
				int _previewQuantity;
			void UpdatePreviewQuantity(){
				int newPreviewQua = Quantity() - PickedQuantity();
				SetPreveiewQuantity( newPreviewQua);
				if( PreviewQuantity() > 0)
					UpdateQuantityVisual();
				else
					IndicateZeroQuantity();
			}


		/* Quantity Visual Update  */
			IQuantityVisualUpdateEngine QuantityVisualUpdateEngine(){
				return _quantityVisualUpdateEngine;
			}
			void SetQuantityVisualUpdateEngine( IQuantityVisualUpdateEngine engine){
				_quantityVisualUpdateEngine = engine;
			}
			IQuantityVisualUpdateEngine _quantityVisualUpdateEngine;
			void WaitForQuantityVisualUpdate(){
				QuantityVisualUpdateEngine().WaitForQuantityVisualUpdate();
			}
			void UpdateQuantityVisual(){
				QuantityVisualUpdateEngine().UpdateQuantityVisual( PreviewQuantity());
			}
			void IndicateZeroQuantity(){
				if( LeavesGhost())
					Ghostify();
				else
					ChangeItemInstantlyToEmpty();
			}


		/* Item Visual Update */

			public void ChangeItemInstantlyTo( ISlottableItem item){
				SetItem( item);
				UpdateItemVisual( item);
				ItemVisualUpdateEngine().ExpireProcess();
			}
			public void ChangeItemInstantlyToEmpty(){
				EmptySlottableItem emptyItem = new EmptySlottableItem();
				SetItem( emptyItem);
				UpdateItemVisual( emptyItem);
				ItemVisualUpdateEngine().ExpireProcess();
			}
			public void ChangeItemGraduallyTo( ISlottableItem item){
				SetItem( item);
				UpdateItemVisual( item);
			}
			IItemVisualUpdateEngine ItemVisualUpdateEngine(){
				return _itemVisualUpdateEngine;
			}
			void SetItemVisualUpdateEngine( IItemVisualUpdateEngine engine){
				_itemVisualUpdateEngine = engine;
			}
				IItemVisualUpdateEngine _itemVisualUpdateEngine;
			public void WaitForItemVisualUpdate(){
				ItemVisualUpdateEngine().WaitForItemVisualUpdate();
			}
			public void UpdateItemVisual( ISlottableItem item){
				ItemVisualUpdateEngine().UpdateItemVisual( item);
			}
		
		
		/* Ghostification */
			IGhostificationEngine GhostificationEngine(){
				return _ghostificationEngine;
			}
			void SetGhostificationEngine( IGhostificationEngine engine){
				_ghostificationEngine = engine;
			}
			IGhostificationEngine _ghostificationEngine;
			public bool LeavesGhost(){
				return _leavesGhost;
			}
			void SetLeavesGhost(bool leaves){
				_leavesGhost = leaves;
			}
				bool _leavesGhost;
			public void Unghostify(){
				GhostificationEngine().Unghostify();
			}
			public void Ghostify(){
				GhostificationEngine().Ghostify();
			}


		/* Transaction */
			public void TearDownAsTarget(){
				SSM().MakePickedSlotWaitForIncrement();
				WaitForExchange();
				Deselect();
				if( SSM().DestinationSG() != SlotGroup()){
					if( !LeavesGhost()){
						Hide();
						SlotGroup().Reindex();
					}
				}
			}
			public void SetUpAsIncrementTarget(){
				SSM().GetPickedSlotReadyForIncrement();
				Select();
			}
			public void SetUpAsFillTarget(){
				Select();
			}
			public void SetUpAsExchangeTarget(){
				GetReadyForExchange();
				ChangeItemInstantlyToEmpty();
				Select();
			}

		/* Hover Icon */
			IHoverIcon HoverIcon(){
				return _hoverIcon;
			}
			void SetHoverIcon( IHoverIcon icon){
				_hoverIcon = icon;
			}
				IHoverIcon _hoverIcon;	
			public void WaitForExchange(){
				HoverIcon().Dehover();
			}
			public void GetReadyForExchange(){
				if( !IsEmpty()){
					ISlottableItem exchangeIconItem = CreateSlotIconItem( Item(), 1);
					HoverIcon exchangeIcon = new HoverIcon( exchangeIconItem);
					SetHoverIcon(exchangeIcon);
					HoverIcon().Hover();
					UpdatePreviewQuantity();
				}
			}
			public bool IsReadyForExchange(){
				return SSM().PickedSlot() != this && HoverIcon() != null;
			}
			public void WaitForIncrement(){
				HoverIcon().WaitForIncrement();
			}
			public void GetReadyForIncrement(){
				HoverIcon().GetReadyForIncrement();
			}
	}
}
