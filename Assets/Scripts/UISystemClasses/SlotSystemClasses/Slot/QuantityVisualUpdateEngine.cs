using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace UISystem{
	public interface IQuantityVisualUpdateEngine{
		void WaitForQuantityVisualUpdate();
		void UpdateQuantityVisual(int newQuantity);
		bool IsWaitingForQuantityVisualUpdate();
		bool IsUpdatingQuantityVisual();

		void SetAndRunQuantityVisualUpdateProcess( IQuantityVisualUpdateProcess process);
		void ExpireProcess();
		IEnumeratorFake UpdateQuantityVisualCoroutine();
	}
	public class QuantityVisualUpdateEngine : IQuantityVisualUpdateEngine {

		public QuantityVisualUpdateEngine(){
			_stateSwitch = new UIStateSwitch<IQuantityVisualUpdateState>();
			InitializeStates();
			SetPrevQuantity( 0);
			SetCurQuantity( 0);
		}
		void InitializeStates(){
			_waitingForQuantityVisualUpdateState = new WaitingForQuantityVisualUpdateState( this);
			_updatingQuantityVisualState = new UpdatingQuantityVisualState( this);
		}


		IUIStateSwitch<IQuantityVisualUpdateState> StateSwitch(){
			return _stateSwitch;
		}
		IUIStateSwitch<IQuantityVisualUpdateState> _stateSwitch;


		public void WaitForQuantityVisualUpdate(){
			StateSwitch().SwitchTo( WaitingForQuantityVisualUpdateState());
		}
		public bool IsWaitingForQuantityVisualUpdate(){
			return StateSwitch().CurState() == WaitingForQuantityVisualUpdateState();
		}
		IQuantityVisualUpdateState WaitingForQuantityVisualUpdateState(){
			return _waitingForQuantityVisualUpdateState;
		}
		IQuantityVisualUpdateState _waitingForQuantityVisualUpdateState;


		public void UpdateQuantityVisual( int newQuantity){
			SetPrevQuantity( CurQuantity());
			SetCurQuantity( newQuantity);
			StateSwitch().SwitchTo( UpdatingQuantityVisualState
			());
		}
		public bool IsUpdatingQuantityVisual(){
			return StateSwitch().CurState() == UpdatingQuantityVisualState();
		}
		IQuantityVisualUpdateState UpdatingQuantityVisualState(){
			return _updatingQuantityVisualState;
		}
		IQuantityVisualUpdateState _updatingQuantityVisualState;



		IUIProcessSwitch<IQuantityVisualUpdateProcess> ProcessSwitch(){
			return _processSwitch;
		}
		IUIProcessSwitch<IQuantityVisualUpdateProcess> _processSwitch;
		public void SetAndRunQuantityVisualUpdateProcess( IQuantityVisualUpdateProcess process){
			ProcessSwitch().SetAndRunProcess( process);
		}
		public void ExpireProcess(){
			ProcessSwitch().ExpireProcess();
		}

		/* Quantity */
		int PrevQuantity(){
			return _prevQuantity;
		}
		void SetPrevQuantity(int qua){
			_prevQuantity = qua;
		}
		int _prevQuantity;

		int CurQuantity(){
			return _curQuantity;
		}
		void SetCurQuantity(int qua){
			_curQuantity = qua;
		}
		int _curQuantity;

		public IEnumeratorFake UpdateQuantityVisualCoroutine(){
			// do something with PrevQuantity and CurQuantity
			return null;
		}
	}
}

