using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class ResizableSG : SlotGroup, IResizableSG{
		public ResizableSG(RectTransformFake rectTrans, ISGConstructorArg constArg): base(rectTrans, constArg){
			SetActStateHandler(new SGActStateHandler(this));
		}
		protected override List<ISlot> CreateSlots(){
			List<ISlot> result = CreateSlotsForEachInventoryItems();
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
		public IResizableSGActStateHandler ActStateHandler(){
			Debug.Assert(_actStateHandler != null);
			return _actStateHandler;
		}
		void SetActStateHandler(IResizableSGActStateHandler handler){
			_actStateHandler = handler;
		}
		IResizableSGActStateHandler _actStateHandler;
		public void WaitForResize(){
			ActStateHandler().WaitForAction();
		}
		public void Resize(){
			ActStateHandler().Resize();
		}
		public override bool IsReceivable(){
			return true;
		}
	}
	public interface IResizableSG: ISlotGroup{
		IResizableSGActStateHandler ActStateHandler();
			void WaitForResize();
			void Resize();
	}
}
