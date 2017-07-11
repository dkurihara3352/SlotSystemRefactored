using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SlotSystem{
	public interface ISlotSystemElement: IEnumerable<ISlotSystemElement>, IStateHandler{
		SSEState curSelState{get;set;}
		SSEState prevSelState{get;set;}
		SSEState curActState{get;set;}
		SSEState prevActState{get;set;}
		void SetAndRunSelProcess(SSEProcess process);
		void SetAndRunActProcess(SSEProcess process);
		SSEProcess selProcess{get;set;}
		SSEProcess actProcess{get;set;}
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
		bool isFocused{get;set;}
		bool isDefocused{get;set;}
		bool isDeactivated{get;set;}
		bool isFocusedInHierarchy{get;}
		void Activate();
		void Deactivate();
		void Focus();
		void Defocus();
		ISlotSystemBundle immediateBundle{get;}
		ISlotSystemElement parent{get;set;}
		ISlotSystemManager ssm{get;set;}
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