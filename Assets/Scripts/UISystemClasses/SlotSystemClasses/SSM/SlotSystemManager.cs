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
		ISlotGroup SourceSG();
		void SetPicked(ISlottableItem item, ISlotGroup sourceSG);
		ISlotSystemElement HoveredSSE();
		void SetHoveredSSE(ISlotSystemElement hoveredSSE);

		ISlotGroup DestinationSG();
		void SwitchDestinationSG(ISlotGroup destSG);

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
			void SetPickedItem( ISlottableItem item){
				_pickedItem = item;
			}
				ISlottableItem _pickedItem;
			public int PickedQuantity(){
				return PickedItem().Quantity();
			}
			public ISlotGroup SourceSG(){
				return _sourceSG;
			}
			void SetSourceSG(ISlotGroup sourceSG){
				_sourceSG = sourceSG;
			}
				ISlotGroup _sourceSG;
			public void SetPicked(ISlottableItem pickedItem, ISlotGroup sourceSG){
				SetPickedItem( pickedItem);
				SetSourceSG( sourceSG);
			}

			public ISlotSystemElement HoveredSSE(){
				return _hoveredSSE;
			}
			public void SetHoveredSSE(ISlotSystemElement hoveredSSE){
				ISlotSystemElement prevHovered = HoveredSSE();
				if(prevHovered != hoveredSSE){

					_hoveredSSE = hoveredSSE;

					ISlotSystemElement newHoveredSSE = HoveredSSE();
					if(newHoveredSSE != null){
						if(hoveredSSE is ISlotGroup)
							SwitchDestinationSG((ISlotGroup)hoveredSSE);
						else if(hoveredSSE is ISlot){
							ISlot hoveredSlot = (ISlot)hoveredSSE;
							SwitchDestinationSG( hoveredSlot.SlotGroup());
							ISlot destSlot = DestinationSG().CalculateDestSlot( hoveredSlot);
							DestinationSG().SwitchDestinationSlot( destSlot);
						}
					}
					else
						SetHoveredSSE( SourceSG());
				}
			}
				ISlotSystemElement _hoveredSSE;

			public ISlotGroup DestinationSG(){
				return _destinationSG;
			}
			public void SwitchDestinationSG(ISlotGroup destSG){
				ISlotGroup prevDestSG = DestinationSG();
				if(destSG != prevDestSG){

					_destinationSG = destSG;

					if(prevDestSG != null)
						prevDestSG.TearDownAsDestSG();

					ISlotGroup newDestSG = DestinationSG();

					if(newDestSG != null)
						newDestSG.SetUpAsDestSG();
				}
			}
				ISlotGroup _destinationSG;
			void ResetDestinationSG(){
				_destinationSG = null;
			}

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
