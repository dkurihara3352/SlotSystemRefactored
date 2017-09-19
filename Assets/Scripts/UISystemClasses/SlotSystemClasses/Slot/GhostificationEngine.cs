using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace UISystem{
	public interface IGhostificationEngine{
		void Unghostify();
		void Ghostify();
		bool IsUnghostified();
		bool IsGhostified();

		void SetAndRunGhostificationProcess( IGhostificationProcess process);
		void ExpireProcess();
		IEnumeratorFake UnghostifyCoroutine();
		IEnumeratorFake GhostifyCoroutine();
	}
	public class GhostificationEngine : IGhostificationEngine {

		public GhostificationEngine(){
			InitializeState();
			_stateSwitch = new UIStateSwitch<IGhostificationState>();
			_processSwitch = new UIProcessSwitch<IGhostificationProcess>();
		}
		void InitializeState(){
			_ghostifiedState = new GhostifiedState( this);
			_unghostifiedState = new UnghostifiedState( this);
		}



		IUIStateSwitch<IGhostificationState> StateSwitch(){
			return _stateSwitch;
		}
		IUIStateSwitch<IGhostificationState> _stateSwitch;


		public void Unghostify(){
			StateSwitch().SwitchTo( UnghostifiedState());
		}
		public bool IsUnghostified(){
			return StateSwitch().CurState() == UnghostifiedState();
		}
		IGhostificationState UnghostifiedState(){
			return _unghostifiedState;
		}
		IGhostificationState _unghostifiedState;


		public void Ghostify(){
			StateSwitch().SwitchTo( GhostifiedState());
		}
		public bool IsGhostified(){
			return StateSwitch().CurState() == GhostifiedState();
		}
		IGhostificationState GhostifiedState(){
			return _ghostifiedState;
		}
		IGhostificationState _ghostifiedState;


		IUIProcessSwitch< IGhostificationProcess> ProcessSwitch(){
			return _processSwitch;
		}
		IUIProcessSwitch< IGhostificationProcess> _processSwitch;
		public void SetAndRunGhostificationProcess( IGhostificationProcess process){
			ProcessSwitch().SetAndRunProcess( process);
		}
		public void ExpireProcess(){
			ProcessSwitch().ExpireProcess();
		}


		public IEnumeratorFake UnghostifyCoroutine(){
			return null;
		}
		public IEnumeratorFake GhostifyCoroutine(){
			return null;
		}
	}
}

