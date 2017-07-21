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
		public class SSEStatesTests : AbsSlotSystemTest{
			[TestCaseSource(typeof(VariousStates_EnterStateCases))]
			public void VariousStates_EnterState_FromNull_CallsInstantMethods(SSEState state){
				ISlotSystemElement mockSSE = MakeSubSSE();
				mockSSE.prevSelState.Returns((ISSESelState)null);
				
				state.EnterState(mockSSE);
				
				if(state == AbsSlotSystemElement.focusedState)
					mockSSE.Received().InstantFocus();
				else if(state == AbsSlotSystemElement.defocusedState)
					mockSSE.Received().InstantDefocus();
				else if(state == AbsSlotSystemElement.selectedState)
					mockSSE.Received().InstantSelect();
			}
				class VariousStates_EnterStateCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						yield return AbsSlotSystemElement.focusedState;
						yield return AbsSlotSystemElement.defocusedState;
						yield return AbsSlotSystemElement.selectedState;
					}
				}
			[TestCaseSource(typeof(Various_EnterState_FromVariousNonNullCases))]
			public void Various_EnterState_FromVariousNonNull_CallsSSESetAndRunProcessAccordingly<T>(ISSESelState fromState, ISSESelState toState, T process) where T: ISSEProcess{
				ISlotSystemElement mockSSE = MakeSubSSE();
				mockSSE.prevSelState.Returns(fromState);

				toState.EnterState(mockSSE);

				mockSSE.Received(1).SetAndRunSelProcess(Arg.Any<T>());
			}
				class Various_EnterState_FromVariousNonNullCases: IEnumerable{
					SSEState deactivated = AbsSlotSystemElement.deactivatedState;
					SSEState defocused = AbsSlotSystemElement.defocusedState;
					SSEState focused = AbsSlotSystemElement.focusedState;
					SSEState selected = AbsSlotSystemElement.selectedState;
					ISSEProcess deaProc = new SSEDeactivateProcess(MakeSubSSE(), FakeCoroutine);
					ISSEProcess focProc = new SSEFocusProcess(MakeSubSSE(), FakeCoroutine);
					ISSEProcess defoProc = new SSEDefocusProcess(MakeSubSSE(), FakeCoroutine);
					ISSEProcess selectProc = new SSESelectProcess(MakeSubSSE(), FakeCoroutine);
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

