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
			public void VariousStates_EnterState_prevNull_CallsInstantMethods(ISlotSystemElement mockSSE, SSEState state){
				mockSSE.wasSelStateNull.Returns(true);
				state.EnterState();
				
				if(state is SSEFocusedState)
					mockSSE.Received().InstantFocus();
				else if(state is SSEDefocusedState)
					mockSSE.Received().InstantDefocus();
				else if(state is SSESelectedState)
					mockSSE.Received().InstantSelect();
			}
				class VariousStates_EnterStateCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] focusedCase;
							ISlotSystemElement mockSSE = MakeSubSSE();
							ISSESelState focusedState = new SSEFocusedState(mockSSE);
						focusedCase = new object[]{mockSSE, focusedState};
							yield return focusedCase;
						
						object[] defocusedCase;
							ISlotSystemElement mockSSE_c2 = MakeSubSSE();
							ISSESelState defocusedState = new SSEDefocusedState(mockSSE_c2);
						defocusedCase = new object[]{mockSSE_c2, defocusedState};
							yield return defocusedCase;
						
						object[] selectedCase;
							ISlotSystemElement mockSSE_c3 = MakeSubSSE();
							ISSESelState selectedState = new SSESelectedState(mockSSE_c3);
						selectedCase = new object[]{mockSSE_c3, selectedState};
							yield return selectedCase;
					}
				}
			[TestCaseSource(typeof(Various_EnterState_FromVariousNonNullCases))]
			public void Various_EnterState_FromVariousNonNull_CallsSSESetAndRunProcessAccordingly<T>(
				ISlotSystemElement mockSSE,
				ISSESelState toState, 
				T process) where T: ISSESelProcess
			{
				toState.EnterState();

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
									ISSESelState dea = new SSEDeactivatedState(mockSSE);
									mockSSE.wasDeactivated.Returns(true);
								dea_dea_dea =  new object[]{mockSSE, dea, deaProc}; 
								yield return dea_dea_dea;
							object[] foc_dea_dea;
									ISlotSystemElement mockSSE_2 = MakeSubSSE();
									ISSESelState dea_2 = new SSEDeactivatedState(mockSSE_2);
									mockSSE_2.wasFocused.Returns(true);
								foc_dea_dea = new object[]{mockSSE_2, dea_2, deaProc}; 
								yield return foc_dea_dea;
							object[] def_dea_dea;
									ISlotSystemElement mockSSE_3 = MakeSubSSE();
									ISSESelState dea_3 = new SSEDeactivatedState(mockSSE_3);
									mockSSE_3.wasDefocused.Returns(true);
								def_dea_dea = new object[]{mockSSE_3, dea_3, deaProc}; 
								yield return def_dea_dea;
							object[] sel_dea_dea;
									ISlotSystemElement mockSSE_4 = MakeSubSSE();
									ISSESelState dea_4 = new SSEDeactivatedState(mockSSE_4);
									mockSSE_4.wasSelected.Returns(true);
								sel_dea_dea = new object[]{mockSSE_4, dea_4, deaProc}; 
								yield return sel_dea_dea;
							

							object[] dea_foc_foc;
									ISlotSystemElement mockSSE_5 = MakeSubSSE();
									ISSESelState foc_5 = new SSEFocusedState(mockSSE_5);
									mockSSE_5.wasDeactivated.Returns(true);
								dea_foc_foc =  new object[]{mockSSE_5, foc_5, focProc}; 
								yield return dea_foc_foc;
							object[] foc_foc_foc;
									ISlotSystemElement mockSSE_6 = MakeSubSSE();
									ISSESelState foc_6 = new SSEFocusedState(mockSSE_6);
									mockSSE_6.wasFocused.Returns(true);
								foc_foc_foc = new object[]{mockSSE_6, foc_6, focProc}; 
								yield return foc_foc_foc;
							object[] def_foc_foc;
									ISlotSystemElement mockSSE_7 = MakeSubSSE();
									ISSESelState foc_7 = new SSEFocusedState(mockSSE_7);
									mockSSE_7.wasDefocused.Returns(true);
								def_foc_foc = new object[]{mockSSE_7, foc_7, focProc}; 
								yield return def_foc_foc;
							object[] sel_foc_foc;
									ISlotSystemElement mockSSE_8 = MakeSubSSE();
									ISSESelState foc_8 = new SSEFocusedState(mockSSE_8);
									mockSSE_8.wasSelected.Returns(true);
								sel_foc_foc = new object[]{mockSSE_8, foc_8, focProc}; 
								yield return sel_foc_foc;

							object[] dea_def_def;
									ISlotSystemElement mockSSE_9 = MakeSubSSE();
									ISSESelState def_9 = new SSEDefocusedState(mockSSE_9);
									mockSSE_9.wasDeactivated.Returns(true);
								dea_def_def =  new object[]{mockSSE_9, def_9, defoProc}; 
								yield return dea_def_def;
							object[] foc_def_def;
									ISlotSystemElement mockSSE_10 = MakeSubSSE();
									ISSESelState def_10 = new SSEDefocusedState(mockSSE_10);
									mockSSE_10.wasFocused.Returns(true);
								foc_def_def = new object[]{mockSSE_10, def_10, defoProc}; 
								yield return foc_def_def;
							object[] def_def_def;
									ISlotSystemElement mockSSE_11 = MakeSubSSE();
									ISSESelState def_11 = new SSEDefocusedState(mockSSE_11);
									mockSSE_11.wasDefocused.Returns(true);
								def_def_def = new object[]{mockSSE_11, def_11, defoProc}; 
								yield return def_def_def;
							object[] sel_def_def;
									ISlotSystemElement mockSSE_12 = MakeSubSSE();
									ISSESelState def_12 = new SSEDefocusedState(mockSSE_12);
									mockSSE_12.wasSelected.Returns(true);
								sel_def_def = new object[]{mockSSE_12, def_12, defoProc}; 
								yield return sel_def_def;

							object[] dea_sel_sel;
									ISlotSystemElement mockSSE_13 = MakeSubSSE();
									ISSESelState sel_13 = new SSESelectedState(mockSSE_13);
									mockSSE_13.wasDeactivated.Returns(true);
								dea_sel_sel =  new object[]{mockSSE_13, sel_13, selectProc}; 
								yield return dea_sel_sel;
							object[] foc_sel_sel;
									ISlotSystemElement mockSSE_14 = MakeSubSSE();
									ISSESelState sel_14 = new SSESelectedState(mockSSE_14);
									mockSSE_14.wasFocused.Returns(true);
								foc_sel_sel = new object[]{mockSSE_14, sel_14, selectProc}; 
								yield return foc_sel_sel;
							object[] def_sel_sel;
									ISlotSystemElement mockSSE_15 = MakeSubSSE();
									ISSESelState sel_15 = new SSESelectedState(mockSSE_15);
									mockSSE_15.wasDefocused.Returns(true);
								def_sel_sel = new object[]{mockSSE_15, sel_15, selectProc}; 
								yield return def_sel_sel;
							object[] sel_sel_sel;
									ISlotSystemElement mockSSE_16 = MakeSubSSE();
									ISSESelState sel_16 = new SSESelectedState(mockSSE_16);
									mockSSE_16.wasSelected.Returns(true);
								sel_sel_sel = new object[]{mockSSE_16, sel_16, selectProc};
								yield return sel_sel_sel;
					}
				}
		}
	}
}

