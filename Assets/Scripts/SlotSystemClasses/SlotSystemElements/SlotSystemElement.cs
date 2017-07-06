using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public interface SlotSystemElement: IEnumerable<SlotSystemElement>, StateHandler{
		SSEState curSelState{get;}
		SSEState prevSelState{get;}
		SSEState curActState{get;}
		SSEState prevActState{get;}
		void SetAndRunSelProcess(SSEProcess process);
		void SetAndRunActProcess(SSEProcess process);
		SSEProcess selProcess{get;}
		SSEProcess actProcess{get;}
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
		SlotSystemBundle immediateBundle{get;}
		SlotSystemElement parent{get;set;}
		SlotSystemManager ssm{get;set;}
		bool ContainsInHierarchy(SlotSystemElement ele);
		void PerformInHierarchy(System.Action<SlotSystemElement> act);
		void PerformInHierarchy(System.Action<SlotSystemElement, object> act, object obj);
		void PerformInHierarchy<T>(System.Action<SlotSystemElement, IList<T>> act, IList<T> list);
		int level{get;}
		bool Contains(SlotSystemElement element);
		SlotSystemElement this[int i]{get;}
		void ToggleOnPageElement();
	}
}