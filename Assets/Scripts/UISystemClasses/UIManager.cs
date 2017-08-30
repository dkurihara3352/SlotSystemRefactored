using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public class UIManager :UIElement, IUIManager {
		public UIManager(RectTransformFake rectTrans): base(rectTrans){
			SetUIHierarchy();
			SetUIMOnAllElements();
			CollectWidgetUIRoots();
		}
		void SetUIHierarchy(){
			PerformInHierarchy(SetHierarchyRecursively);
		}
			void SetHierarchyRecursively(IUIElement ele){
				ele.SetHierarchy();
			}
		void SetUIMOnAllElements(){
			PerformInHierarchy(SetUIMRecursively);
		}
			void SetUIMRecursively(IUIElement ele){
				ele.SetUIM(this);
			}
		void CollectWidgetUIRoots(){
			List<IWidgetUIRoot> roots = new List<IWidgetUIRoot>();
			PerformInHierarchy(AddWidgetUIRootInList, roots);
			SetWidgetUIRoots(roots);
		}
			void AddWidgetUIRootInList(IUIElement element, IList<IWidgetUIRoot> list){
				if(element is IWidgetUIRoot)
					list.Add((IWidgetUIRoot)element);
			}
		public List<IWidgetUIRoot> WidgetUIRoots(){
			Debug.Assert(_widgetUIRoots != null);
			return _widgetUIRoots;
		}
			List<IWidgetUIRoot> _widgetUIRoots;
		public void SetWidgetUIRoots(List<IWidgetUIRoot> roots){
			_widgetUIRoots = roots;
		}
	}
	public interface IUIManager{
		List<ISlotSystemManager> SlotSystemManagers();
		ISlotSystemManager SelectedSSM();
		List<IWidgetUIRoot> WidgetUIRoots();
		IWidgetUIRoot SelectedWidgetUIRoot();
	}
}
