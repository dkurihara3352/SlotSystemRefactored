using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public interface ISlotSystemManager: IUIElement{
		IInventoryManager InventoryManager();
		void InitializeSlotSystemOnActivate();
		List<ISlotSystemElement> SlotSystemElements();
		
		ISlottableItem PickedItem();
		int PickedQuantity();
		void SetPicked(ISlottableItem item, ISlotGroup sourceSG);
		ISlotSystemElement HoveredSSE();
		void SetHoveredSSE(ISlotSystemElement hoveredSSE);
		void SetSourceSGHovered();

		ISlotGroup DestinationSG();
		void SetDestinationSG(ISlotGroup destSG);

		void Drop();
		void UpdateInventory(IInventory inventory);
		event EventHandler<InventoryEventArgs> InventoryUpdated;
		
		void Refresh();
	}
	public interface IInventorySystemSSM{}
	public class SlotSystemManager : UIElement, ISlotSystemManager{
		public SlotSystemManager(RectTransformFake rectTrans, IUISelStateRepo selStateRepo, ITapCommand tapCommand, IInventoryManager invManager): base(rectTrans, selStateRepo, tapCommand){
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
		public void OnSSMSelected(object uiManager, ISlotSystemManager ssm){
			OnSSMSelectedCommand().Execute(ssm);
		}
			IOnSSMSelectedCommand OnSSMSelectedCommand(){
				Debug.Assert(_onSSMSelectedCommand != null);
				return _onSSMSelectedCommand;
			}
			IOnSSMSelectedCommand _onSSMSelectedCommand;



		/* Events and triggers */
			public ISlottableItem PickedItem(){
				return _pickedItem;
			}
			public void SetPicked(ISlottableItem pickedItem, ISlotGroup sourceSG){
				_pickedItem = pickedItem;
				SetSourceSG(sourceSG);
			}
				ISlottableItem _pickedItem;
			public int PickedQuantity(){
				return PickedItem().Quantity();
			}
			ISlotGroup SourceSG(){
				return _sourceSG;
			}
			void SetSourceSG(ISlotGroup sourceSG){
				_sourceSG = sourceSG;
			}
				ISlotGroup _sourceSG;
			
			public ISlotSystemElement HoveredSSE(){
				return _hoveredSSE;
			}
			public void SetHoveredSSE(ISlotSystemElement hoveredSSE){
				ISlotSystemElement prevHovered = HoveredSSE();
				if(prevHovered != hoveredSSE){

					_hoveredSSE = hoveredSSE;

					if(prevHovered != null)
						PerformHoverExitAction(prevHovered);

					ISlotSystemElement newHoveredSSE = HoveredSSE();
					if(newHoveredSSE != null){
						PerformHoverEnterAction(newHoveredSSE);
					}
					else{
						SetSourceSGHovered();
					}
				}
			}
				ISlotSystemElement _hoveredSSE;
			void PerformHoverEnterAction(ISlotSystemElement hoveredSSE){
				if(hoveredSSE is ISlotGroup)
					SetDestinationSG((ISlotGroup)hoveredSSE);
				else if(hoveredSSE is ISlot){
					ISlot hoveredSlot = (ISlot)hoveredSSE;
					SetDestinationSG( hoveredSlot.SlotGroup());
					if(hoveredSlot.IsIncrementable())
						DraggedIcon().GetReadyForIncrement();
					else{
						DestinationSG().EplicitlySpecifyDestSlot(hoveredSlot);
					}
				}
			}
			void PerformHoverExitAction(ISlotSystemElement exitedSSE){
				if(exitedSSE is ISlotGroup)
					return;
				else if( exitedSSE is ISlot){
					ISlot exitedSlot = (ISlot)exitedSSE;
					if(HoveredSSE() == null)
						exitedSlot.WaitForSwap();
					DraggedIcon().WaitForIncrement();
				}
			}
			public void SetSourceSGHovered(){
				SetHoveredSSE( SourceSG());
			}

			public ISlotGroup DestinationSG(){
				return _destinationSG;
			}
			public void SetDestinationSG(ISlotGroup destSG){
				ISlotGroup prevDestSG = DestinationSG();
				if(destSG != prevDestSG){

					_destinationSG = destSG;

					if(prevDestSG != null){
						prevDestSG.Deselect();
						prevDestSG.ReverseImplicitTargetFocus();
					}

					ISlotGroup newDestSG = DestinationSG();

					if(newDestSG != null){
						newDestSG.Select();
						newDestSG.ImplicitlyFocusTargetSlot();
					}
				}
			}
				ISlotGroup _destinationSG;


			public void Drop(){
			}

			public void UpdateInventory(IInventory inventory){
				OnInventoryUpdated(new InventoryEventArgs(inventory));
			}
			public event EventHandler<InventoryEventArgs> InventoryUpdated;
			protected virtual void OnInventoryUpdated(InventoryEventArgs e){
				if(InventoryUpdated != null)
					InventoryUpdated.Invoke(this, e);
			}

		public void Refresh(){
			SetPicked(null, null);
			SetHoveredSSE(null);
		}
	}
	public class InventoryEventArgs: EventArgs{
		public readonly IInventory inventory;
		public InventoryEventArgs(IInventory inventory){
			this.inventory = inventory;
		}
	}
}
