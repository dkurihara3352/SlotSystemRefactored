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
			public void VariousStates_EnterState_FromNull_CallsInstantMethods(ISlotSystemElement mockSSE, SSEState state){
				mockSSE.prevSelState.Returns((ISSESelState)null);
				
				state.EnterState(mockSSE);
				
				if(state == mockSSE.focusedState)
					mockSSE.Received().InstantFocus();
				else if(state == mockSSE.defocusedState)
					mockSSE.Received().InstantDefocus();
				else if(state == mockSSE.selectedState)
					mockSSE.Received().InstantSelect();
			}
				class VariousStates_EnterStateCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] focusedCase;
							ISlotSystemElement mockSSE = MakeSubSSE();
							ISSESelState focusedState = new SSEFocusedState();
							mockSSE.focusedState.Returns(focusedState);
						focusedCase = new object[]{mockSSE, focusedState};
							yield return focusedCase;
						
						object[] defocusedCase;
							ISlotSystemElement mockSSE_c2 = MakeSubSSE();
							ISSESelState defocusedState = new SSEDefocusedState();
							mockSSE.defocusedState.Returns(defocusedState);
						defocusedCase = new object[]{mockSSE_c2, defocusedState};
							yield return defocusedCase;
						
						object[] selectedCase;
							ISlotSystemElement mockSSE_c3 = MakeSubSSE();
							ISSESelState selectedState = new SSESelectedState();
							mockSSE.selectedState.Returns(selectedState);
						selectedCase = new object[]{mockSSE_c3, selectedState};
							yield return selectedCase;


					}
				}
			[TestCaseSource(typeof(Various_EnterState_FromVariousNonNullCases))]
			public void Various_EnterState_FromVariousNonNull_CallsSSESetAndRunProcessAccordingly<T>(
				ISlotSystemElement mockSSE, 
				ISSESelState fromState, 
				ISSESelState toState, 
				T process) where T: ISSESelProcess
			{
				mockSSE.prevSelState.Returns(fromState);

				toState.EnterState(mockSSE);

				mockSSE.Received().SetAndRunSelProcess(Arg.Any<T>());
			}
				class Various_EnterState_FromVariousNonNullCases: IEnumerable{
					SSEDeactivateProcess deaProc = new SSEDeactivateProcess(MakeSubSSE(), FakeCoroutine);
					SSEFocusProcess focProc = new SSEFocusProcess(MakeSubSSE(), FakeCoroutine);
					SSEDefocusProcess defoProc = new SSEDefocusProcess(MakeSubSSE(), FakeCoroutine);
					SSESelectProcess selectProc = new SSESelectProcess(MakeSubSSE(), FakeCoroutine);
					public IEnumerator GetEnumerator(){
							object[] dea_dea_dea;
									ISlotSystemElement mockSSE = MakeSubSSE();
									ISSESelState dea = new SSEDeactivatedState();
									mockSSE.deactivatedState.Returns(dea);
								dea_dea_dea =  new object[]{mockSSE, mockSSE.deactivatedState, mockSSE.deactivatedState, deaProc}; 
								yield return dea_dea_dea;
							object[] foc_dea_dea;
									ISlotSystemElement mockSSE_2 = MakeSubSSE();
									ISSESelState dea_2 = new SSEDeactivatedState();
									mockSSE_2.deactivatedState.Returns(dea_2);
									ISSESelState foc_2 = new SSEFocusedState();
									mockSSE_2.focusedState.Returns(foc_2);
								foc_dea_dea = new object[]{mockSSE_2, mockSSE_2.focusedState, mockSSE_2.deactivatedState, deaProc}; 
								yield return foc_dea_dea;
							object[] def_dea_dea;
									ISlotSystemElement mockSSE_3 = MakeSubSSE();
									ISSESelState dea_3 = new SSEDeactivatedState();
									mockSSE_3.deactivatedState.Returns(dea_3);
									ISSESelState defoc_3 = new SSEDefocusedState();
									mockSSE_3.defocusedState.Returns(defoc_3);
								def_dea_dea = new object[]{mockSSE_3, mockSSE_3.defocusedState, mockSSE_3.deactivatedState, deaProc}; 
								yield return def_dea_dea;
							object[] sel_dea_dea;
									ISlotSystemElement mockSSE_4 = MakeSubSSE();
									ISSESelState dea_4 = new SSEDeactivatedState();
									mockSSE_4.deactivatedState.Returns(dea_4);
									ISSESelState defoc_4 = new SSEDefocusedState();
									mockSSE_4.defocusedState.Returns(defoc_4);
								sel_dea_dea = new object[]{mockSSE_4, mockSSE_4.defocusedState, mockSSE_4.deactivatedState, deaProc}; 
								yield return sel_dea_dea;
							

							object[] dea_foc_foc;
									ISlotSystemElement mockSSE_5 = MakeSubSSE();
									ISSESelState dea_5 = new SSEDeactivatedState();
									ISSESelState foc_5 = new SSEFocusedState();
									mockSSE_5.deactivatedState.Returns(dea_5);
									mockSSE_5.focusedState.Returns(foc_5);
								dea_foc_foc =  new object[]{mockSSE_5, mockSSE_5.deactivatedState, mockSSE_5.focusedState, focProc}; 
								yield return dea_foc_foc;
							object[] foc_foc_foc;
									ISlotSystemElement mockSSE_6 = MakeSubSSE();
									ISSESelState foc_6 = new SSEFocusedState();
									mockSSE_6.focusedState.Returns(foc_6);
								foc_foc_foc = new object[]{mockSSE_6, mockSSE_6.focusedState, mockSSE_6.focusedState, focProc}; 
								yield return foc_foc_foc;
							object[] def_foc_foc;
									ISlotSystemElement mockSSE_7 = MakeSubSSE();
									ISSESelState defoc_7 = new SSEDefocusedState();
									ISSESelState foc_7 = new SSEFocusedState();
									mockSSE_7.defocusedState.Returns(defoc_7);
									mockSSE_7.focusedState.Returns(foc_7);
								def_foc_foc = new object[]{mockSSE_7, mockSSE_7.defocusedState, mockSSE_7.focusedState, focProc}; 
								yield return def_foc_foc;
							object[] sel_foc_foc;
									ISlotSystemElement mockSSE_8 = MakeSubSSE();
									ISSESelState sel_8 = new SSESelectedState();
									ISSESelState foc_8 = new SSEFocusedState();
									mockSSE_8.selectedState.Returns(sel_8);
									mockSSE_8.focusedState.Returns(foc_8);
								sel_foc_foc = new object[]{mockSSE_8, mockSSE_8.selectedState, mockSSE_8.focusedState, focProc}; 
								yield return sel_foc_foc;

							object[] dea_def_def;
									ISlotSystemElement mockSSE_9 = MakeSubSSE();
									ISSESelState dea_9 = new SSEDeactivatedState();
									ISSESelState def_9 = new SSEDefocusedState();
									mockSSE_9.deactivatedState.Returns(dea_9);
									mockSSE_9.defocusedState.Returns(def_9);
								dea_def_def =  new object[]{mockSSE_9, mockSSE_9.deactivatedState, mockSSE_9.defocusedState, defoProc}; 
								yield return dea_def_def;
							object[] foc_def_def;
									ISlotSystemElement mockSSE_10 = MakeSubSSE();
									ISSESelState foc_10 = new SSEFocusedState();
									ISSESelState def_10 = new SSEDefocusedState();
									mockSSE_10.focusedState.Returns(foc_10);
									mockSSE_10.defocusedState.Returns(def_10);
								foc_def_def = new object[]{mockSSE_10, mockSSE_10.focusedState, mockSSE_10.defocusedState, defoProc}; 
								yield return foc_def_def;
							object[] def_def_def;
									ISlotSystemElement mockSSE_11 = MakeSubSSE();
									ISSESelState defoc_11 = new SSEDefocusedState();
									mockSSE_11.defocusedState.Returns(defoc_11);
								def_def_def = new object[]{mockSSE_11, mockSSE_11.defocusedState, mockSSE_11.defocusedState, defoProc}; 
								yield return def_def_def;
							object[] sel_def_def;
									ISlotSystemElement mockSSE_12 = MakeSubSSE();
									ISSESelState sel_12 = new SSESelectedState();
									ISSESelState def_12 = new SSEDefocusedState();
									mockSSE_12.selectedState.Returns(sel_12);
									mockSSE_12.defocusedState.Returns(def_12);
								sel_def_def = new object[]{mockSSE_12, mockSSE_12.selectedState, mockSSE_12.defocusedState, defoProc}; 
								yield return sel_def_def;

							object[] dea_sel_sel;
									ISlotSystemElement mockSSE_13 = MakeSubSSE();
									ISSESelState dea_13 = new SSEDeactivatedState();
									ISSESelState sel_13 = new SSESelectedState();
									mockSSE_13.deactivatedState.Returns(dea_13);
									mockSSE_13.selectedState.Returns(sel_13);
								dea_sel_sel =  new object[]{mockSSE_13, mockSSE_13.deactivatedState, mockSSE_13.selectedState, selectProc}; 
								yield return dea_sel_sel;
							object[] foc_sel_sel;
									ISlotSystemElement mockSSE_14 = MakeSubSSE();
									ISSESelState foc_14 = new SSEFocusedState();
									ISSESelState sel_14 = new SSESelectedState();
									mockSSE_14.focusedState.Returns(foc_14);
									mockSSE_14.selectedState.Returns(sel_14);
								foc_sel_sel = new object[]{mockSSE_14, mockSSE_14.focusedState, mockSSE_14.selectedState, selectProc}; 
								yield return foc_sel_sel;
							object[] def_sel_sel;
									ISlotSystemElement mockSSE_15 = MakeSubSSE();
									ISSESelState def_15 = new SSEDefocusedState();
									ISSESelState sel_15 = new SSESelectedState();
									mockSSE_15.defocusedState.Returns(def_15);
									mockSSE_15.selectedState.Returns(sel_15);
								def_sel_sel = new object[]{mockSSE_15, mockSSE_15.defocusedState, mockSSE_15.selectedState, selectProc}; 
								yield return def_sel_sel;
							object[] sel_sel_sel;
									ISlotSystemElement mockSSE_16 = MakeSubSSE();
									ISSESelState sel_16 = new SSESelectedState();
									mockSSE_16.selectedState.Returns(sel_16);
								sel_sel_sel = new object[]{mockSSE_16, mockSSE_16.selectedState, mockSSE_16.selectedState, selectProc};
								yield return sel_sel_sel;
					}
				}
		}
	}
}

