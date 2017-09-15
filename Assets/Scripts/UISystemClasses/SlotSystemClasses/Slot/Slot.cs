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
		void Refresh();
		void Destroy();
	}
	public class Slot : SlotSystemElement, ISlot{
		public Slot(RectTransformFake rectTrans, IUISelStateRepo selStateRepo, ITapCommand tapCommand, ISlottableItem item, bool leavesGhost): base(rectTrans, selStateRepo, tapCommand){
			SetItem(item);
			SetActStateEngine( new SlotActStateEngine( this));
			SetFadeStateEngine( new SlotFadeStateEngine( this));
			InitializeStates();
			SetLeavesGhost(leavesGhost);
		}
		/*	States	*/
			public override void InitializeStates(){
				MakeUnselectable();
				WaitForAction();
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
				ISlotIcon draggedIcon = new SlotIcon( Item());
				SetDraggedIcon( draggedIcon);
				PostPickFilter();
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
				return SSM().PickedQuantity();
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
			public bool ShareSGAndItem(ISlot other){
				bool flag = true;
				flag &= SlotGroup() == other.SlotGroup();
				flag &= Item().Equals(other.Item());
				return flag;
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
			public override bool IsHovered(){
				return SSM().HoveredSSE() == this;
			}

			public void TearDownAsTarget(){
				SSM().MakePickedSlotWaitForIncrement();
				WaitForExchange();
				Deselect();
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

			public void WaitForExchange(){
				/*	
					SlotGroup.HideSlot() -->
					SwappedIcon.Dehover()
						Animate the swapped back to slot with swap target id
					upon expiration, 
						wait until slot's HideProcess is over
					upon expiration of HideProcess
						if( !IsReadyForSwap)
							Swap instantly to empty
						if(IsReadyForSwap) -->this case
							Swap instantly to SwappedIcon item

				*/
				DraggedIcon().Dehover();
			}
			void PostPickFilter(){
				
			}
			public void GetReadyForExchange(){
				if( !IsEmpty()){
					SlotIcon exchangeIcon = new SlotIcon( Item());
					SetExchangeIcon(exchangeIcon);
					ExchangeIcon().Hover();
				}
			}
			ISlotIcon ExchangeIcon(){
				return _exchangeIcon;
			}
			void SetExchangeIcon( ISlotIcon icon){
				_exchangeIcon = icon;
			}
				ISlotIcon _exchangeIcon;
			public bool IsReadyForExchange(){
				return ExchangeIcon() != null;
			}


			ISlotIcon DraggedIcon(){
				return _draggedIcon;
			}
			void SetDraggedIcon( ISlotIcon draggedIcon){
				_draggedIcon = draggedIcon;
			}
				ISlotIcon _draggedIcon;
			public void WaitForIncrement(){
				DraggedIcon().WaitForIncrement();
			}
			public void GetReadyForIncrement(){
				DraggedIcon().GetReadyForIncrement();
			}

			public void Refresh(){
				WaitForAction();
				WaitForExchange();
				SetExchangeIcon( null);
				SetDraggedIcon( null);
			}
	}
}
