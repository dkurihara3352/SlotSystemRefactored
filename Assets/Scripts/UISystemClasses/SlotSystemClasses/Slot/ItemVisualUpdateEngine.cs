using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public interface IItemVisualUpdateEngine{
		void WaitForItemVisualUpdate();
		bool IsWaitingForItemVisualUpdate();
		void UpdateItemVisual( ISlottableItem item);
		bool IsUpdatingItemVisual();

		void SetAndRunItemVisualUpdateProcess( IItemVisualUpdateProcess process);

		IEnumeratorFake WaitForItemVisualUpdateCoroutine();
		IEnumeratorFake UpdateItemVisualCoroutine();

		void ExpireProcess();
	}


	public class ItemVisualUpdateEngine : IItemVisualUpdateEngine {
		public ItemVisualUpdateEngine(){
			SetStateSwitch( new UIStateSwitch<IItemVisualUpdateState>());
			SetProcessSwitch( new UIProcessSwitch<IItemVisualUpdateProcess>());
			InitializeStates();
		}


		IUIStateSwitch<IItemVisualUpdateState> StateSwitch(){
			return stateSwitch;
		}
		void SetStateSwitch( IUIStateSwitch<IItemVisualUpdateState> stateSwitch){
			this.stateSwitch = stateSwitch;
		}
		IUIStateSwitch<IItemVisualUpdateState> stateSwitch;


		void InitializeStates(){
			SetWaitingForItemVisualUpdateState( new WaitingForItemVisualUpdateState( this));
			SetUpdateingItemVisualState( new UpdatingItemVisualState( this));
		}
		

		IItemVisualUpdateState WaitingForItemVisualUpdateState(){
			return _waitingForItemVisualUpdateState;
		}
		IItemVisualUpdateState _waitingForItemVisualUpdateState;
		void SetWaitingForItemVisualUpdateState( IItemVisualUpdateState state){
			_waitingForItemVisualUpdateState = state;
		}
		public void WaitForItemVisualUpdate(){
			StateSwitch().SwitchTo( WaitingForItemVisualUpdateState());
		}
		public bool IsWaitingForItemVisualUpdate(){
			return StateSwitch().CurState() == WaitingForItemVisualUpdateState();
		}


		IItemVisualUpdateState UpdatingItemVisualState(){
			return _updatingItemVisualState;
		}
		IItemVisualUpdateState _updatingItemVisualState;
		void SetUpdateingItemVisualState( IItemVisualUpdateState state){
			_updatingItemVisualState = state;
		}
		public void UpdateItemVisual( ISlottableItem item){
			SetTargetItem( item);
			StateSwitch().SwitchTo( UpdatingItemVisualState());
		}
		public bool IsUpdatingItemVisual(){
			return StateSwitch().CurState() == UpdatingItemVisualState();
		}



		IUIProcessSwitch<IItemVisualUpdateProcess> ProcessSwitch(){
			return _processSwitch;
		}
		void SetProcessSwitch( IUIProcessSwitch<IItemVisualUpdateProcess> procSwitch){
			_processSwitch = procSwitch;
		}
		IUIProcessSwitch<IItemVisualUpdateProcess> _processSwitch;
		public void SetAndRunItemVisualUpdateProcess( IItemVisualUpdateProcess process){
			ProcessSwitch().SetAndRunProcess( process);
		}
		public void ExpireProcess(){
			ProcessSwitch().ExpireProcess();
		}


		ISlottableItem TargetItem(){
			return _targetItem;
		}
		void SetTargetItem( ISlottableItem item){
			_targetItem = item;
		}
		ISlottableItem _targetItem;

		public IEnumeratorFake WaitForItemVisualUpdateCoroutine(){
			return null;
		}
		public IEnumeratorFake UpdateItemVisualCoroutine(){
			//Use TargetItem to do something
			return null;
		}
	}
}
