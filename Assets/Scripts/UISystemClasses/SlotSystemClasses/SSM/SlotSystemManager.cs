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
		ISlot DestinationSlot();
		void SetDestinationSlot(ISlot destSlot);

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
						prevHovered.PerformHoverExitAction();

					ISlotSystemElement newHoveredSSE = HoveredSSE();
					if(newHoveredSSE != null)
						newHoveredSSE.PerformHoverEnterAction();
					else{
						SetSourceSGHovered();
					}
				}
			}
				ISlotSystemElement _hoveredSSE;
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
						prevDestSG.ReduceItem( PickedItem());
					}
					ISlotGroup newDestSG = DestinationSG();
					if(newDestSG != null){
						newDestSG.Select();
						if(newDestSG.IsHovered())
							newDestSG.AddItem( PickedItem());
					}
				}
			}
				ISlotGroup _destinationSG;
			public ISlot DestinationSlot(){
				return _destinationSlot;
			}
			public void SetDestinationSlot(ISlot destSlot){
				ISlot prevDestSlot = DestinationSlot();
				if(destSlot != prevDestSlot){

					_destinationSlot = destSlot;

					if(prevDestSlot != null){
						prevDestSlot.Deselect();
					}
					ISlot newDestSlot = DestinationSlot();
					if(newDestSlot != null){
						newDestSlot.Select();
					}
				}
			}
				ISlot _destinationSlot;


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
