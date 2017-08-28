using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface IUISystemInputHandler{
		void OnPointerDown();
		void OnPointerUp();
		void OnDeselected();
		void OnEndDrag();
	}
}
