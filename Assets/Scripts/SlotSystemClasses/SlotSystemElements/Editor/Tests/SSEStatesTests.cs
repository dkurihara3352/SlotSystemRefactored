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
		public class SSEStatesTests : AbsSlotSystemTest{
			[TestCaseSource(typeof(VariousStates_EnterStateCases))]
			public void VariousStates_EnterState_FromDeactivated_CallsInstantMethods(SSEState state){
				ISlotSystemElement mockSSE = MakeSubSSE();
				mockSSE.prevSelState.Returns(AbsSlotSystemElement.deactivatedState);
				
				state.EnterState(mockSSE);

				if(state == AbsSlotSystemElement.focusedState)
					mockSSE.Received().InstantGreyin();
				else if(state == AbsSlotSystemElement.defocusedState)
					mockSSE.Received().InstantGreyout();
				else if(state == AbsSlotSystemElement.selectedState)
					mockSSE.Received().InstantHighlight();
			}
				class VariousStates_EnterStateCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						yield return new object[]{
							AbsSlotSystemElement.focusedState
						};
						yield return new object[]{
							AbsSlotSystemElement.defocusedState
						};
						yield return new object[]{
							AbsSlotSystemElement.selectedState
						};
					}
				}
			[TestCaseSource(typeof(VariousStates_EnterState_FromVariousNonDeactivatedCases))]
			public void VariousNonDeactivatedStates_EnterState_FromVariousNonDeactivated_CallsSSESetAndRunProcessAccordingly<T>(SSEState fromState, SSEState toState, T process) where T: ISSEProcess{
				ISlotSystemElement mockSSE = MakeSubSSE();
				mockSSE.prevSelState.Returns(fromState);

				toState.EnterState(mockSSE);

				mockSSE.Received().SetAndRunSelProcess(Arg.Any<T>());
			}
				class VariousStates_EnterState_FromVariousNonDeactivatedCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						yield return new object[]{
							AbsSlotSystemElement.defocusedState,
							AbsSlotSystemElement.focusedState,
							new SSEGreyinProcess(MakeSubSSE(), FakeCoroutine)
						};
						yield return new object[]{
							AbsSlotSystemElement.defocusedState,
							AbsSlotSystemElement.selectedState,
							new SSEHighlightProcess(MakeSubSSE(), FakeCoroutine)
						};
						yield return new object[]{
							AbsSlotSystemElement.focusedState,
							AbsSlotSystemElement.defocusedState,
							new SSEGreyoutProcess(MakeSubSSE(), FakeCoroutine)
						};
						yield return new object[]{
							AbsSlotSystemElement.focusedState,
							AbsSlotSystemElement.selectedState,
							new SSEHighlightProcess(MakeSubSSE(), FakeCoroutine)
						};
						yield return new object[]{
							AbsSlotSystemElement.selectedState,
							AbsSlotSystemElement.defocusedState,
							new SSEGreyoutProcess(MakeSubSSE(), FakeCoroutine)
						};
						yield return new object[]{
							AbsSlotSystemElement.selectedState,
							AbsSlotSystemElement.focusedState,
							new SSEDehighlightProcess(MakeSubSSE(), FakeCoroutine)
						};
					}
				}
		}
	}
}

