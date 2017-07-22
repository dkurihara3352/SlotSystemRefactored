using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using SlotSystem;
namespace SlotSystemTests{
	namespace SSEElementsTests{
		[TestFixture]
		[Category("OtherElements")]
		public class SSEStatesTests : SlotSystemTest{
			[TestCaseSource(typeof(VariousStates_EnterStateCases))]
			public void VariousStates_EnterState_FromNull_CallsInstantMethods(SSEState state){
				ISlotSystemElement mockSSE = MakeSubSSE();
				mockSSE.prevSelState.Returns((ISSESelState)null);
				
				state.EnterState(mockSSE);
				
				if(state == SlotSystemElement.focusedState)
					mockSSE.Received().InstantFocus();
				else if(state == SlotSystemElement.defocusedState)
					mockSSE.Received().InstantDefocus();
				else if(state == SlotSystemElement.selectedState)
					mockSSE.Received().InstantSelect();
			}
				class VariousStates_EnterStateCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						yield return SlotSystemElement.focusedState;
						yield return SlotSystemElement.defocusedState;
						yield return SlotSystemElement.selectedState;
					}
				}
			[TestCaseSource(typeof(Various_EnterState_FromVariousNonNullCases))]
			public void Various_EnterState_FromVariousNonNull_CallsSSESetAndRunProcessAccordingly<T>(ISSESelState fromState, ISSESelState toState, T process) where T: ISSESelProcess{
				ISlotSystemElement mockSSE = MakeSubSSE();
				mockSSE.prevSelState.Returns(fromState);

				toState.EnterState(mockSSE);

				mockSSE.Received(1).SetAndRunSelProcess(Arg.Any<T>());
			}
				class Various_EnterState_FromVariousNonNullCases: IEnumerable{
					ISSESelState deactivated = SlotSystemElement.deactivatedState;
					ISSESelState defocused = SlotSystemElement.defocusedState;
					ISSESelState focused = SlotSystemElement.focusedState;
					ISSESelState selected = SlotSystemElement.selectedState;
					ISSESelProcess deaProc = new SSEDeactivateProcess(MakeSubSSE(), FakeCoroutine);
					ISSESelProcess focProc = new SSEFocusProcess(MakeSubSSE(), FakeCoroutine);
					ISSESelProcess defoProc = new SSEDefocusProcess(MakeSubSSE(), FakeCoroutine);
					ISSESelProcess selectProc = new SSESelectProcess(MakeSubSSE(), FakeCoroutine);
					public IEnumerator GetEnumerator(){
						yield return new object[]{deactivated, deactivated, deaProc};
						yield return new object[]{focused, deactivated, deaProc};
						yield return new object[]{defocused, deactivated, deaProc};
						yield return new object[]{selected, deactivated, deaProc};
						
						yield return new object[]{deactivated, focused, focProc};
						yield return new object[]{focused, focused, focProc};
						yield return new object[]{defocused, focused, focProc};
						yield return new object[]{selected, focused, focProc};
						
						yield return new object[]{deactivated, defocused, defoProc};
						yield return new object[]{focused, defocused, defoProc};
						yield return new object[]{defocused, defocused, defoProc};
						yield return new object[]{selected, defocused, defoProc};
						
						yield return new object[]{deactivated, selected, selectProc};
						yield return new object[]{focused, selected, selectProc};
						yield return new object[]{defocused, selected, selectProc};
						yield return new object[]{selected, selected, selectProc};

					}
				}
		}
	}
}

