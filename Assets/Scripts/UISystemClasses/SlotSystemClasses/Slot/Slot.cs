using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace UISystem{
	public interface ISlot: ISlotSystemElement{
		ISlotGroup SlotGroup();

		ISlotActStateHandler ActStateHandler();
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
		bool IsSwappableWith(ISlottableItem item);


		bool IsReadyForSwap();
		void WaitForSwap();
		void GetReadyForSwap(ISlottableItem item);

		bool LeavesGhost();
		void Refresh();
		void Destroy();
	}
	public class Slot : SlotSystemElement, ISlot{
		public Slot(RectTransformFake rectTrans, IUISelStateRepo selStateRepo, ITapCommand tapCommand, ISlottableItem item, bool leavesGhost): base(rectTrans, selStateRepo, tapCommand){
			SetItem(item);
			SetActStateHandler(new SlotActStateHandler(this));
			InitializeStates();
			SetLeavesGhost(leavesGhost);
		}
		/*	States	*/
			public override void InitializeStates(){
				MakeUnselectable();
				WaitForAction();
			}
			/*	Action State */
				public ISlotActStateHandler ActStateHandler(){
					Debug.Assert(_actStateHandler != null);
					return _actStateHandler;
				}
					ISlotActStateHandler _actStateHandler;
				public void SetActStateHandler(ISlotActStateHandler actStateHandler){
					_actStateHandler = actStateHandler;
				}
				public void WaitForAction(){
					ActStateHandler().WaitForAction();
				}
			public void PickUp(){
				CreateDraggedIcon();
				SSM().SetPickedItem( Item());
				PostPickFilter();
			}
			void CreateDraggedIcon(){

			}
			public override void OnPointerDown(){
				base.OnPointerDown();
				ActStateHandler().OnPointerDown();
			}
			public override void OnPointerUp(){
				base.OnPointerUp();
				ActStateHandler().OnPointerUp();
			}
			public override void OnEndDrag(){
				base.OnEndDrag();
				ActStateHandler().OnEndDrag();
			}
			public override void OnDeselected(){
				base.OnDeselected();
				ActStateHandler().OnDeselected();
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

			public void WaitForSwap(){
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
			}
			public void GetReadyForSwap(ISlottableItem swappedItem){
				// SlotGroup.ShowSlot() -->
				DraggedIcon swappedIcon = CreateSwappedIcon(swappedItem);
				SetSwappedIcon(swappedIcon);
				SwappedIcon().Hover();
				
			}
			public bool IsReadyForSwap(){
				return SwappedIcon() != null;
			}


			public void Refresh(){
				WaitForAction();
				WaitForSwap();
			}
	}
}
