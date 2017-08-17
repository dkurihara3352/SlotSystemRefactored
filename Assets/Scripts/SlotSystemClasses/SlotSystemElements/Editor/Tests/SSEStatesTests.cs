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
			public void VariousStates_EnterState_prevNull_CallsInstantMethods(ISSESelStateHandler selStateHandler, SSEState state){
				selStateHandler.wasSelStateNull.Returns(true);
				state.EnterState();
				
				if(state is SSEFocusedState)
					selStateHandler.Received().InstantFocus();
				else if(state is SSEDefocusedState)
					selStateHandler.Received().InstantDefocus();
				else if(state is SSESelectedState)
					selStateHandler.Received().InstantSelect();
			}
				class VariousStates_EnterStateCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData case_0;
							ISSESelStateHandler selStateHandler_0 = Substitute.For<ISSESelStateHandler>();
							ISSESelState focusedState_0 = new SSEFocusedState(selStateHandler_0);
							case_0 = new TestCaseData(
								selStateHandler_0, focusedState_0
							);
							yield return case_0.SetName("FromFocused");
						TestCaseData case_1;
							ISSESelStateHandler selStateHandler_1 = Substitute.For<ISSESelStateHandler>();
							ISSESelState focusedState_1 = new SSEDefocusedState(selStateHandler_1);
							case_1 = new TestCaseData(
								selStateHandler_1, focusedState_1
							);
							yield return case_1.SetName("FromDefocused");
						TestCaseData case_2;
							ISSESelStateHandler selStateHandler_2 = Substitute.For<ISSESelStateHandler>();
							ISSESelState focusedState_2 = new SSESelectedState(selStateHandler_2);
							case_2 = new TestCaseData(
								selStateHandler_2, focusedState_2
							);
							yield return case_2.SetName("FromSelected");
					}
				}
			[TestCaseSource(typeof(Various_EnterState_FromVariousNonNullCases))]
			public void Various_EnterState_FromVariousNonNull_CallsSSESetAndRunProcessAccordingly<T>(
				ISSESelStateHandler selStateHandler,
				ISSESelState toState, 
				T process) where T: ISSESelProcess
			{
				toState.EnterState();

				selStateHandler.Received().SetAndRunSelProcess(Arg.Any<T>());
			}
				class Various_EnterState_FromVariousNonNullCases: IEnumerable{
					SSEDeactivateProcess deaProc = new SSEDeactivateProcess(FakeCoroutine);
					SSEFocusProcess focProc = new SSEFocusProcess(FakeCoroutine);
					SSEDefocusProcess defoProc = new SSEDefocusProcess(FakeCoroutine);
					SSESelectProcess selectProc = new SSESelectProcess(FakeCoroutine);
					public IEnumerator GetEnumerator(){
							TestCaseData dea_dea_dea;
									ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
									ISSESelState dea = new SSEDeactivatedState(selStateHandler);
									selStateHandler.wasDeactivated.Returns(true);
								dea_dea_dea =  new TestCaseData(selStateHandler, dea, deaProc); 
								yield return dea_dea_dea.SetName("dea_dea_dea");
							TestCaseData foc_dea_dea;
									ISSESelStateHandler selStateHandler_2 = Substitute.For<ISSESelStateHandler>();
									ISSESelState dea_2 = new SSEDeactivatedState(selStateHandler_2);
									selStateHandler_2.wasFocused.Returns(true);
								foc_dea_dea = new TestCaseData(selStateHandler_2, dea_2, deaProc); 
								yield return foc_dea_dea.SetName("foc_dea_dea");
							TestCaseData def_dea_dea;
									ISSESelStateHandler selStateHandler_3 = Substitute.For<ISSESelStateHandler>();
									ISSESelState dea_3 = new SSEDeactivatedState(selStateHandler_3);
									selStateHandler_3.wasDefocused.Returns(true);
								def_dea_dea = new TestCaseData(selStateHandler_3, dea_3, deaProc); 
								yield return def_dea_dea.SetName("def_dea_dea");
							TestCaseData sel_dea_dea;
									ISSESelStateHandler selStateHandler_4 = Substitute.For<ISSESelStateHandler>();
									ISSESelState dea_4 = new SSEDeactivatedState(selStateHandler_4);
									selStateHandler_4.wasSelected.Returns(true);
								sel_dea_dea = new TestCaseData(selStateHandler_4, dea_4, deaProc); 
								yield return sel_dea_dea.SetName("sel_dea_dea");
							

							TestCaseData dea_foc_foc;
									ISSESelStateHandler selStateHandler_5 = Substitute.For<ISSESelStateHandler>();
									ISSESelState foc_5 = new SSEFocusedState(selStateHandler_5);
									selStateHandler_5.wasDeactivated.Returns(true);
								dea_foc_foc =  new TestCaseData(selStateHandler_5, foc_5, focProc); 
								yield return dea_foc_foc.SetName("dea_foc_foc");
							TestCaseData foc_foc_foc;
									ISSESelStateHandler selStateHandler_6 = Substitute.For<ISSESelStateHandler>();
									ISSESelState foc_6 = new SSEFocusedState(selStateHandler_6);
									selStateHandler_6.wasFocused.Returns(true);
								foc_foc_foc = new TestCaseData(selStateHandler_6, foc_6, focProc); 
								yield return foc_foc_foc.SetName("foc_foc_foc");
							TestCaseData def_foc_foc;
									ISSESelStateHandler selStateHandler_7 = Substitute.For<ISSESelStateHandler>();
									ISSESelState foc_7 = new SSEFocusedState(selStateHandler_7);
									selStateHandler_7.wasDefocused.Returns(true);
								def_foc_foc = new TestCaseData(selStateHandler_7, foc_7, focProc); 
								yield return def_foc_foc.SetName("def_foc_foc");
							TestCaseData sel_foc_foc;
									ISSESelStateHandler selStateHandler_8 = Substitute.For<ISSESelStateHandler>();
									ISSESelState foc_8 = new SSEFocusedState(selStateHandler_8);
									selStateHandler_8.wasSelected.Returns(true);
								sel_foc_foc = new TestCaseData(selStateHandler_8, foc_8, focProc); 
								yield return sel_foc_foc.SetName("sel_foc_foc");

							TestCaseData dea_def_def;
									ISSESelStateHandler selStateHandler_9 = Substitute.For<ISSESelStateHandler>();
									ISSESelState def_9 = new SSEDefocusedState(selStateHandler_9);
									selStateHandler_9.wasDeactivated.Returns(true);
								dea_def_def =  new TestCaseData(selStateHandler_9, def_9, defoProc); 
								yield return dea_def_def.SetName("dea_def_def");
							TestCaseData foc_def_def;
									ISSESelStateHandler selStateHandler_10 = Substitute.For<ISSESelStateHandler>();
									ISSESelState def_10 = new SSEDefocusedState(selStateHandler_10);
									selStateHandler_10.wasFocused.Returns(true);
								foc_def_def = new TestCaseData(selStateHandler_10, def_10, defoProc); 
								yield return foc_def_def.SetName("foc_def_def");
							TestCaseData def_def_def;
									ISSESelStateHandler selStateHandler_11 = Substitute.For<ISSESelStateHandler>();
									ISSESelState def_11 = new SSEDefocusedState(selStateHandler_11);
									selStateHandler_11.wasDefocused.Returns(true);
								def_def_def = new TestCaseData(selStateHandler_11, def_11, defoProc); 
								yield return def_def_def.SetName("def_def_def");
							TestCaseData sel_def_def;
									ISSESelStateHandler selStateHandler_12 = Substitute.For<ISSESelStateHandler>();
									ISSESelState def_12 = new SSEDefocusedState(selStateHandler_12);
									selStateHandler_12.wasSelected.Returns(true);
								sel_def_def = new TestCaseData(selStateHandler_12, def_12, defoProc); 
								yield return sel_def_def.SetName("sel_def_def");

							TestCaseData dea_sel_sel;
									ISSESelStateHandler selStateHandler_13 = Substitute.For<ISSESelStateHandler>();
									ISSESelState sel_13 = new SSESelectedState(selStateHandler_13);
									selStateHandler_13.wasDeactivated.Returns(true);
								dea_sel_sel =  new TestCaseData(selStateHandler_13, sel_13, selectProc); 
								yield return dea_sel_sel.SetName("dea_sel_sel");
							TestCaseData foc_sel_sel;
									ISSESelStateHandler selStateHandler_14 = Substitute.For<ISSESelStateHandler>();
									ISSESelState sel_14 = new SSESelectedState(selStateHandler_14);
									selStateHandler_14.wasFocused.Returns(true);
								foc_sel_sel = new TestCaseData(selStateHandler_14, sel_14, selectProc); 
								yield return foc_sel_sel.SetName("foc_sel_sel");
							TestCaseData def_sel_sel;
									ISSESelStateHandler selStateHandler_15 = Substitute.For<ISSESelStateHandler>();
									ISSESelState sel_15 = new SSESelectedState(selStateHandler_15);
									selStateHandler_15.wasDefocused.Returns(true);
								def_sel_sel = new TestCaseData(selStateHandler_15, sel_15, selectProc); 
								yield return def_sel_sel.SetName("def_sel_sel");
							TestCaseData sel_sel_sel;
									ISSESelStateHandler selStateHandler_16 = Substitute.For<ISSESelStateHandler>();
									ISSESelState sel_16 = new SSESelectedState(selStateHandler_16);
									selStateHandler_16.wasSelected.Returns(true);
								sel_sel_sel = new TestCaseData(selStateHandler_16, sel_16, selectProc);
								yield return sel_sel_sel.SetName("sel_sel_sel");
					}
				}
		}
	}
}

