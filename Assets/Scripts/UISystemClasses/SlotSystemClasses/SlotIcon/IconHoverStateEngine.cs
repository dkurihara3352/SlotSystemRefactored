using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace UISystem{
	public interface IIconHoverStateEngine{
		void Dehover();
		bool WasDehovering();
		bool IsDehovering();

		void Hover();
		bool WasHovering();
		bool IsHovering();

		void SetAndRunIconProcess( IconProcess process);
		IEnumeratorFake DehoverCoroutine();
		IEnumeratorFake HoverCoroutine();

		void SwapItemBackToOriginal();
	}
	public class IconHoverStateEngine : IIconHoverStateEngine {
		public IconHoverStateEngine( IHoverIcon slotIcon, ISlot slot){
			SetSlotIcon( slotIcon);
			SetSlot( slot);
			SetStateSwitch( new IconHoverStateSwitch());
			InitializeStates();
		}


		IHoverIcon SlotIcon(){
			return _slotIcon;
		}
		void SetSlotIcon( IHoverIcon slotIcon){
			_slotIcon = slotIcon;
		}
			IHoverIcon _slotIcon;
		ISlot Slot(){
			return _slot;
		}
		void SetSlot( ISlot slot){
			_slot = slot;
		}
			ISlot _slot;


		ISwitchableStateSwitch<IIconHoverState> StateSwitch(){
			return _stateSwitch;
		}
		void SetStateSwitch( ISwitchableStateSwitch<IIconHoverState> stateSwitch){
			_stateSwitch = stateSwitch;
		}
		ISwitchableStateSwitch<IIconHoverState> _stateSwitch;


		public void InitializeStates(){
			_dehoveringState = new IconDehoveringState( this);
			_hoveringState = new IconHoveringState( this);
		}


		public void Dehover(){
			StateSwitch().SwitchTo( DehoveringState());
		}
		IIconHoverState DehoveringState(){
			return _dehoveringState;
		}
			IIconHoverState _dehoveringState;
		public bool WasDehovering(){
			return StateSwitch().PrevState() == DehoveringState();
		}
		public bool IsDehovering(){
			return StateSwitch().CurState() == DehoveringState();
		}


		public void Hover(){
			StateSwitch().SwitchTo( HoveringState());
		}
		IIconHoverState HoveringState(){
			return _hoveringState;
		}
			IIconHoverState _hoveringState;
		public bool WasHovering(){
			return StateSwitch().PrevState() == HoveringState();
		}
		public bool IsHovering(){
			return StateSwitch().CurState() == HoveringState();
		}
		

		IUIProcessSwitch<IconProcess> IconProcessSwitch(){
			return _iconProcessSwitch;
		}
		void SetProcessSwitch( IUIProcessSwitch<IconProcess> procSwitch){
			_iconProcessSwitch = procSwitch;
		}
		IUIProcessSwitch<IconProcess> _iconProcessSwitch;
		public void SetAndRunIconProcess( IconProcess process){
			IconProcessSwitch().SetAndRunProcess( process);
		}


		public IEnumeratorFake DehoverCoroutine(){
			return null;
		}
		public IEnumeratorFake HoverCoroutine(){
			return null;
		}


		public void SwapItemBackToOriginal(){
			Slot().ChangeItemInstantlyTo( SlotIcon().Item());
		}
	}
}
