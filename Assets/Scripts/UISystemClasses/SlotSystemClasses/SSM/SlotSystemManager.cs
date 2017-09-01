using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class SlotSystemManager : UIElement, ISlotSystemManager{
		public SlotSystemManager(RectTransformFake rectTrans, IUISelStateRepo selStateRepo, IInventoryManager invManager): base(rectTrans, selStateRepo){
			SetInventoryManager(invManager);
		}
		public void InitializeSlotSystemOnActivate(){
			SetSlotSystemElementsOnActivate();
		}
		public List<ISlotSystemElement> SlotSystemElements(){
			Debug.Assert(_slotSystemElements != null);
			return _slotSystemElements;
		}
		public void SetSlotSystemElementsOnActivate(){
			List<ISlotSystemElement> result = new List<ISlotSystemElement>();
			PerformInHierarchy(AddSSEToList, result);
			_slotSystemElements = result;
		}
		void AddSSEToList(IUIElement uiElement, IList<ISlotSystemElement> list){
			if(uiElement is ISlotSystemElement)
				list.Add((ISlotSystemElement)uiElement);
		}
			List<ISlotSystemElement> _slotSystemElements;
		List<ISlotGroup> SlotGroups(){
			List<ISlotGroup> result = new List<ISlotGroup>();
			foreach(var sse in SlotSystemElements())
				if(sse is ISlotGroup)
					result.Add((ISlotGroup)sse);
			return result;
		}
		public IInventoryManager InventoryManager(){
			Debug.Assert(_inventoryManager != null);
			return _inventoryManager;
		}
		void SetInventoryManager(IInventoryManager inventoryManager){
			_inventoryManager = inventoryManager;
		}
			IInventoryManager _inventoryManager;
		public ISlottable GetPickedSB(){
			return _pickedSB;
		}
		public void OnSSMSelected(object uiManager, ISlotSystemManager ssm){
			OnSSMSelectedCommand().Execute(ssm);
		}
			IOnSSMSelectedCommand OnSSMSelectedCommand(){
				Debug.Assert(_onSSMSelectedCommand != null);
				return _onSSMSelectedCommand;
			}
			IOnSSMSelectedCommand _onSSMSelectedCommand;
		public void Refresh(){}
		/* Events and triggers */
			public void SetPickedSB(ISlottable pickedSB){
				ISlottable prevPickedSB = GetPickedSB();
				if(prevPickedSB != pickedSB)
					_pickedSB = pickedSB;
				ISlottable newPickedSB = GetPickedSB();
				if(newPickedSB != null)
					OnSBPickedUp(new SBEventArgs(pickedSB));
			}
				ISlottable _pickedSB;
			public event EventHandler<SBEventArgs> SBPickedUp;
			protected virtual void OnSBPickedUp(SBEventArgs e){
				if(SBPickedUp != null)
					SBPickedUp.Invoke(this, e);
			}
			public ISlotGroup HoveredSG(){
				return _hoveredSG;
			}
			public void SetHoveredSG(ISlotGroup hoveredSG){
				ISlotGroup prevSG = HoveredSG();
				if(prevSG != hoveredSG){
					_hoveredSG = hoveredSG;
					ISlotGroup newHoveredSG = HoveredSG();
					if(newHoveredSG != null)
						OnSGHoverEntered(new SGEventArgs(newHoveredSG));
				}
			}
				ISlotGroup _hoveredSG;
			public event EventHandler<SGEventArgs> SGHoverEntered;
			protected virtual void OnSGHoverEntered(SGEventArgs e){
				if(SGHoverEntered != null)
					SGHoverEntered.Invoke(this, e);
			}
			public ISlot HoveredSlot(){
				return _hoveredSlot;
			}
			public void SetHoveredSlot(ISlot hoveredSlot){
				ISlot prevSlot = HoveredSlot();
				if(prevSlot != hoveredSlot){
					_hoveredSlot = hoveredSlot;
					ISlot newHoveredSlot = HoveredSlot();
					if(newHoveredSlot != null)
						OnSlotHoverEntered(new SlotEventArgs(newHoveredSlot));
				}
			}
				ISlot _hoveredSlot;
			public event EventHandler<SlotEventArgs> SlotHoverEntered;
			protected virtual void OnSlotHoverEntered(SlotEventArgs e){
				if(SlotHoverEntered != null)
					SlotHoverEntered.Invoke(this, e);
			}
			public void Drop(){
				OnSBDropped(new SBEventArgs(GetPickedSB()));
			}
			public event EventHandler<SBEventArgs> SBDropped;
			protected virtual void OnSBDropped(SBEventArgs e){
				if(SBDropped != null)
					SBDropped.Invoke(this, e);
			}
			public void UpdateInventory(IInventory inventory){
				OnInventoryUpdated(new InventoryEventArgs(inventory));
			}
			public event EventHandler<InventoryEventArgs> InventoryUpdated;
			protected virtual void OnInventoryUpdated(InventoryEventArgs e){
				if(InventoryUpdated != null)
					InventoryUpdated.Invoke(this, e);
			}
	}
	public interface ISlotSystemManager: IUIElement{
		IInventoryManager InventoryManager();
		void InitializeSlotSystemOnActivate();
		List<ISlotSystemElement> SlotSystemElements();
		void Refresh();
		void SetPickedSB(ISlottable sb);
		ISlotGroup HoveredSG();
		void SetHoveredSG(ISlotGroup sg);
		ISlot HoveredSlot();
		void SetHoveredSlot(ISlot slot);
		void Drop();
		void UpdateInventory(IInventory inventory);
		event EventHandler<SBEventArgs> SBPickedUp;
		event EventHandler<SGEventArgs> SGHoverEntered;
		event EventHandler<SlotEventArgs> SlotHoverEntered;
		event EventHandler<SBEventArgs> SBDropped;
		event EventHandler<InventoryEventArgs> InventoryUpdated;
	}
	public interface IInventorySystemSSM{}
	public class SBEventArgs: EventArgs{
		public readonly ISlottable slottable;
		public SBEventArgs(ISlottable slottable){
			this.slottable = slottable;
		}
	}
	public class SGEventArgs: EventArgs{
		public readonly ISlotGroup slotGroup;
		public SGEventArgs(ISlotGroup sg){
			this.slotGroup = sg;
		}
	}
	public class SlotEventArgs: EventArgs{
		public readonly ISlot slot;
		public SlotEventArgs(ISlot slot){
			this.slot = slot;
		}
	}
	public class InventoryEventArgs: EventArgs{
		public readonly IInventory inventory;
		public InventoryEventArgs(IInventory inventory){
			this.inventory = inventory;
		}
	}
}
