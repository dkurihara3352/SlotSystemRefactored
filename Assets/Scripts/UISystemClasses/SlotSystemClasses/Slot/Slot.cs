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
		void WaitForItemFade();
		void FadeItem( ISlottableItem item);

		
		void SetUpAsIncrementTarget();
		void SetUpAsFillTarget();
		void SetUpAsExchangeTarget();
		void TearDownAsTarget();

		bool IsReadyForExchange();
		void WaitForExchange();
		void GetReadyForExchange();

		void GetReadyForIncrement();
		void WaitForIncrement();

		bool LeavesGhost();
		void Ghostify();
		void Unghostify();
		void Refresh();
		void Destroy();
	}
	public class Slot : SlotSystemElement, ISlot{
		public Slot(RectTransformFake rectTrans, IUISelStateRepo selStateRepo, ITapCommand tapCommand, ISlottableItem item, bool leavesGhost): base(rectTrans, selStateRepo, tapCommand){
			SetItem(item);
			SetActStateEngine( new SlotActStateEngine( this));
			SetFadeStateEngine( new SlotFadeStateEngine());
			InitializeStates();
			SetLeavesGhost(leavesGhost);
		}
		/*	States	*/
			public override void InitializeStates(){
				MakeUnselectable();
				WaitForAction();
				WaitForItemFade();
				WaitForIncrement();
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
				/*	picked quantity here
				*/
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
				int _previewQuantity;
			void UpdatePreviewQuantity(){
				int newPreviewQua = Quantity() - PickedQuantity();
				if(newPreviewQua <= 0)
					IndicateZeroQuantity();
			}
			void IndicateZeroQuantity(){
				if( LeavesGhost())
					Ghostify();
				else
					ChangeItemToEmptyInstantly();
			}

			public void ChangeItemInstantlyTo( ISlottableItem item){
				SetItem( item);
			}
			public void ChangeItemToEmptyInstantly(){
				SetItem( new EmptySlottableItem());
			}
			public void ChangeItemGraduallyTo( ISlottableItem item){
				SetItem( item);
				FadeItem( item);
			}
			ISlotFadeStateEngine FadeStateEngine(){
				return _fadeStateEngine;
			}
			void SetFadeStateEngine( ISlotFadeStateEngine engine){
				_fadeStateEngine = engine;
			}
				ISlotFadeStateEngine _fadeStateEngine;
			public void WaitForItemFade(){
				FadeStateEngine().WaitForItemFade();
			}
			public void FadeItem( ISlottableItem item){
				FadeStateEngine().FadeItem( item);
			}
		/* Others */
			public ISlotGroup SlotGroup(){
				return (ISlotGroup)Parent();
			}
			public void Destroy(){
				if(LeavesGhost()){

				}else{

				}
			}
			public bool LeavesGhost(){
				return _leavesGhost;
			}
			void SetLeavesGhost(bool leaves){
				_leavesGhost = leaves;
			}
				bool _leavesGhost;
			public void Ghostify(){

			}
			public void Unghostify(){
				
			}

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
				ChangeItemToEmptyInstantly();
				Select();
			}

		/* Slot Icon */
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

			public void Refresh(){
				WaitForAction();
				WaitForExchange();
				WaitForIncrement();
				SetHoverIcon( null);
			}
	}
}
