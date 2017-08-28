using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class SlotSystemManager : UIElement, ISlotSystemManager{
		public SlotSystemManager(IUIElement uiElement, RectTransform rectTrans): base(rectTrans){
			this.uiElement = uiElement;
		}
			IUIElement uiElement;
		void Initialize(){
			SetSlotSystemElements();
		}
			void SetSlotSystemElements(){
				List<ISlotSystemElement> result = new List<ISlotSystemElement>();
				uiElement.PerformInHierarchy(AddSSEToList, result);
				_slotSystemElements = result;
			}
			void AddSSEToList(IUIElement uiElement, IList<ISlotSystemElement> list){
				if(uiElement is ISlotSystemElement)
					list.Add((ISlotSystemElement)uiElement);
			}
		public List<ISlotSystemElement> SlotSystemElements(){
			Debug.Assert(_slotSystemElements != null);
			return _slotSystemElements;
		}
			List<ISlotSystemElement> _slotSystemElements;
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
			public ISlotGroup GetHoveredSG(){
				return _hoveredSG;
			}
			public void SetHoveredSG(ISlotGroup hoveredSG){
				ISlotGroup prevSG = GetHoveredSG();
				if(prevSG != hoveredSG){
					_hoveredSG = hoveredSG;
					ISlotGroup newHoveredSG = GetHoveredSG();
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
			public ISlot GetHoveredSlot(){
				return _hoveredSlot;
			}
			public void SetHoveredSlot(ISlot hoveredSlot){
				ISlot prevSlot = GetHoveredSlot();
				if(prevSlot != hoveredSlot){
					_hoveredSlot = hoveredSlot;
					ISlot newHoveredSlot = GetHoveredSlot();
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
	}
	public interface ISlotSystemManager: IUIElement{
		List<ISlotSystemElement> SlotSystemElements();
		void OnSSMFocus(object source, ISlotSystemManager ssm);
		void SetPickedSB(ISlottable sb);
		void SetHoveredSG(ISlotGroup sg);
		void SetHoveredSlot(ISlot slot);
		void Drop();
		event EventHandler<SBEventArgs> SBPickedUp;
		event EventHandler<SGEventArgs> SGHoverEntered;
		event EventHandler<SlotEventArgs> SlotHoverEntered;
		event EventHandler<SBEventArgs> SBDropped;
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
}
