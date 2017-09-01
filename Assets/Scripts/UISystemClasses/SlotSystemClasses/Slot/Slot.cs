using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public class Slot: SlotSystemElement, ISlot{
		public Slot(RectTransformFake rectTrans, IUISelStateRepo selStateRepo, ISSEEventCommandsRepo eventCommRepo, ITapCommand tapCommand): base(rectTrans, selStateRepo, eventCommRepo){
			SetActStateHandler(new SlotActStateHandler(this));
			SetSelStateHandler(new UISelStateHandler(this, selStateRepo));
			SetTapCommand(tapCommand);
		}

		public ISlotActStateHandler ActStateHandler(){
			Debug.Assert(_actStateHandler != null);
			return _actStateHandler;
		}
		void SetActStateHandler(ISlotActStateHandler handler){
			_actStateHandler = handler;
		}
			ISlotActStateHandler _actStateHandler;
		public void WaitForAction(){
			ActStateHandler().WaitForAction();
		}
		public void WaitForPickUp(){
			ActStateHandler().WaitForPickUp();
		}
		public void WaitForPointerUp(){
			ActStateHandler().WaitForPointerUp();
		}
		public void WaitForNextTouch(){
			ActStateHandler().WaitForNextTouch();
		}
		public void PickUp(){
			ActStateHandler().PickUp();
			SetIsPickedUp(true);
		}
		public void OnPointerDown(){
			ActStateHandler().OnPointerDown();
		}
		public void OnPointerUp(){
			ActStateHandler().OnPointerUp();
		}
		public void OnEndDrag(){
			ActStateHandler().OnEndDrag();
		}
		public void OnDeselected(){
			ActStateHandler().OnDeselected();
		}


		public void Refresh(){
			SetIsPickedUp(false);
			SetPickedAmount(0);
			WaitForAction();
		}
		public void Tap(){
			TapCommand().Execute();
		}
		ITapCommand TapCommand(){
			Debug.Assert(_tapCommand != null);
			return _tapCommand;
		}
			ITapCommand _tapCommand;
		void SetTapCommand(ITapCommand comm){
			_tapCommand = comm;
		}
		public int PickedAmount(){
			return _pickedAmount;
		}
		public void SetPickedAmount(int amount){
			_pickedAmount = amount;
		}
			int _pickedAmount = 0;
		public void Increment(){
			ISlottable sb = Slottable();
			if(sb != null){
				ISlottableItem item = Slottable().Item();
				if(item != null){
					int quantity = item.Quantity();
					if(PickedAmount() < quantity)
						SetPickedAmount(PickedAmount() + 1);
				}
			}
		}
		public void Drop(){
			if(IsPickedUp()){
				SSM().Drop();
			}
		}


		public override void HoverEnter(){
			SSM().SetHoveredSlot(this);
		}
		public override bool IsHovered(){
			return SSM().HoveredSlot() == this;
		}
		public bool HasItemAndIsStackable(){
			ISlottable sb = Slottable();
			if(sb != null)
				return sb.IsStackable();
			return false;
		}
		public bool IsPickedUp(){
			return _isPickedUp;
		}
		void SetIsPickedUp(bool pickedUp){
			_isPickedUp = pickedUp;
		}
			bool _isPickedUp;
		public IResizableSG SlotGroup(){
			Debug.Assert(_slotGroup != null);
			return _slotGroup;
		}
		public void SetSG(IResizableSG sg){
			_slotGroup = sg;
		}
			IResizableSG _slotGroup;
		public ISlottable Slottable(){
			return _slottable;
		}
			ISlottable _slottable;
		void SetSlottable(ISlottable sb){
			_slottable = sb;
		}
		public void MakeSBSelectable(){
			if(!IsEmpty())
				Slottable().MakeSelectable();
		}
		public void MakeSBUnselectable(){
			if(!IsEmpty())
				Slottable().MakeUnselectable();
		}
		public void SelectSB(){
			if(!IsEmpty())
				Slottable().Select();
		}
		public bool IsEmpty(){
			return Slottable() == null;
		}
		public int ID(){
			Debug.Assert(_id != -1);
			return _id;
		}
			int _id = -1;
		public void SetID(int id){
			_id = id;
		}
	}
	public interface ISlot: ISlotSystemElement, IUIElement, IUISystemInputHandler{
		void MakeSBSelectable();
		void MakeSBUnselectable();
		void SelectSB();
		int ID();
		void SetID(int id);
		IResizableSG SlotGroup();
		ISlottable Slottable();
		bool IsEmpty();
		ISlotActStateHandler ActStateHandler();
			void WaitForAction();
			void WaitForPickUp();
			void WaitForPointerUp();
			void WaitForNextTouch();
			void PickUp();
		bool IsPickedUp();
		int PickedAmount();
		bool HasItemAndIsStackable();
		void Drop();
		void Tap();
		void Refresh();
		void Increment();
	}
}