using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public interface ISlotFadeStateEngine{
		void WaitForItemFade();
		void FadeItem( ISlottableItem item);
		IEnumeratorFake FadeItemCoroutine( ISlottableItem item);
	}


	public class SlotFadeStateEngine : MonoBehaviour {
		public SlotFadeStateEngine(){
			SetStateSwitch( new UIStateSwitch<ISlotFadeState>());
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

		}

	}
}
