using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public interface ISlotFadeStateEngine{
		void WaitForItemFade();
		bool IsWaitingForItemFade();
		void FadeItem( ISlottableItem item);
		bool IsFadingItem();

		void SetAndRunFadeProcess( ISlotFadeProcess process);

		IEnumeratorFake WaitForItemFadeCoroutine();
		IEnumeratorFake FadeItemCoroutine();
	}


	public class SlotFadeStateEngine : ISlotFadeStateEngine {
		public SlotFadeStateEngine(){
			SetStateSwitch( new UIStateSwitch<ISlotFadeState>());
			SetProcessSwitch( new UIProcessSwitch<ISlotFadeProcess>());
			InitializeStates();
		}


		IUIStateSwitch<ISlotFadeState> FadeStateSwitch(){
			return _fadeStateSwitch;
		}
		void SetStateSwitch( IUIStateSwitch<ISlotFadeState> stateSwitch){
			_fadeStateSwitch = stateSwitch;
		}
		IUIStateSwitch<ISlotFadeState> _fadeStateSwitch;


		void InitializeStates(){
			SetWaitingForItemFadeState( new SlotWaitingForItemFadeState( this));
			SetFadingItemState( new SlotFadingItemState( this));
		}
		

		ISlotFadeState WaitingForItemFadeState(){
			return _waitingForItemFadeState;
		}
		ISlotFadeState _waitingForItemFadeState;
		void SetWaitingForItemFadeState( ISlotFadeState state){
			_waitingForItemFadeState = state;
		}
		public void WaitForItemFade(){
			FadeStateSwitch().SwitchTo( WaitingForItemFadeState());
		}
		public bool IsWaitingForItemFade(){
			return FadeStateSwitch().CurState() == WaitingForItemFadeState();
		}


		ISlotFadeState FadingItemState(){
			return _fadingItemState;
		}
		ISlotFadeState _fadingItemState;
		void SetFadingItemState( ISlotFadeState state){
			_fadingItemState = state;
		}
		public void FadeItem( ISlottableItem item){
			SetTargetItem( item);
			FadeStateSwitch().SwitchTo( FadingItemState());
		}
		public bool IsFadingItem(){
			return FadeStateSwitch().CurState() == FadingItemState();
		}



		IUIProcessSwitch<ISlotFadeProcess> ProcessSwitch(){
			return _processSwitch;
		}
		void SetProcessSwitch( IUIProcessSwitch<ISlotFadeProcess> procSwitch){
			_processSwitch = procSwitch;
		}
		IUIProcessSwitch<ISlotFadeProcess> _processSwitch;
		public void SetAndRunFadeProcess( ISlotFadeProcess process){
			ProcessSwitch().SetAndRunProcess( process);
		}



		ISlottableItem TargetItem(){
			return _targetItem;
		}
		void SetTargetItem( ISlottableItem item){
			_targetItem = item;
		}
		ISlottableItem _targetItem;

		public IEnumeratorFake WaitForItemFadeCoroutine(){
			return null;
		}
		public IEnumeratorFake FadeItemCoroutine(){
			//Use TargetItem to do something
			return null;
		}
	}
}
