using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class UIManager : IUIManager {

	}
	public interface IUIManager{
		List<ISlotSystemManager> SlotSystemManagers();
		ISlotSystemManager SelectedSSM();
		List<IWidgetUIRoot> WidgetUIRoots();
		IWidgetUIRoot SelectedWidgetUIRoot();
	}
}
