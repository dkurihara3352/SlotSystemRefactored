using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class ResizableSG : SlotGroup, IResizableSG{
		public ResizableSG(RectTransformFake rectTrans, ISGConstructorArg constArg): base(rectTrans, constArg){
			SetActStateHandler(new ResizableSGActStateEngine(this));
		}
		public IResizableSGActStateEngine ActStateHandler(){
			Debug.Assert(_actStateHandler != null);
			return _actStateHandler;
		}
		void SetActStateHandler(IResizableSGActStateEngine handler){
			_actStateHandler = handler;
		}
		IResizableSGActStateEngine _actStateHandler;
		public void WaitForResize(){
			ActStateHandler().WaitForAction();
		}
		public void Resize(){
			ActStateHandler().Resize();
		}
		public override bool IsFillable(){
			return true;
		}
		public override void SetUpPickedItemSlotOnPickUp(){
			
		}
		protected override void SetFillTargetSlotAsDestination(){
			/*	Get the empty Slot
				Switch its item from empty to PickedItem
				if auto reorder
					reindex and travel
				set it as destination
			*/
			ISlot emptySlot = EmptySlot();
			emptySlot.SwitchItemTo( SSM().PickedItem());
			if(IsAutoReorderEnabled())
				Reindex();
			SSM().SetDestinationSlot( emptySlot);
		}
		public override void ReverseImplicitTargetFocus(){

		}
	}
	public interface IResizableSG: ISlotGroup{
		IResizableSGActStateEngine ActStateHandler();
			void WaitForResize();
			void Resize();
	}
}
