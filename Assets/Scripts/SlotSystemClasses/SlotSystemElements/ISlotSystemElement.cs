using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public interface ISlotSystemElement: IEnumerable<ISlotSystemElement>, IStateHandler{
		SSEState curSelState{get;}
		SSEState prevSelState{get;}
		SSEState curActState{get;}
		SSEState prevActState{get;}
		void SetAndRunSelProcess(ISSEProcess process);
		void SetAndRunActProcess(ISSEProcess process);
		ISSEProcess selProcess{get;}
		ISSEProcess actProcess{get;}
		IEnumeratorFake greyoutCoroutine();
		IEnumeratorFake greyinCoroutine();
		IEnumeratorFake highlightCoroutine();
		IEnumeratorFake dehighlightCoroutine();
		void InstantGreyin();
		void InstantGreyout();
		void InstantHighlight();
		string eName{get;}
		bool isBundleElement{get;}
		bool isPageElement{get;}
		bool isToggledOn{get;}
		bool isFocused{get;}
		bool isDefocused{get;}
		bool isDeactivated{get;}
		bool isFocusedInHierarchy{get;}
		void Activate();
		void Deactivate();
		void Focus();
		void Defocus();
		ISlotSystemBundle immediateBundle{get;}
		ISlotSystemElement parent{get;}
		void SetParent(ISlotSystemElement par);
		ISlotSystemManager ssm{get;}
		void SetSSM(ISlotSystemElement ssm);
		bool ContainsInHierarchy(ISlotSystemElement ele);
		void PerformInHierarchy(System.Action<ISlotSystemElement> act);
		void PerformInHierarchy(System.Action<ISlotSystemElement, object> act, object obj);
		void PerformInHierarchy<T>(System.Action<ISlotSystemElement, IList<T>> act, IList<T> list);
		int level{get;}
		bool Contains(ISlotSystemElement element);
		ISlotSystemElement this[int i]{get;}
		void ToggleOnPageElement();
	}
}