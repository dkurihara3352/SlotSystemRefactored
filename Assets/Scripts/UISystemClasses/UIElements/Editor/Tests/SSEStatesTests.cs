﻿using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UISystem;
namespace SlotSystemTests{
	namespace SSEElementsTests{
		[TestFixture]
		[Category("OtherElements")]
		public class SSEStatesTests : SlotSystemTest{
			[TestCaseSource(typeof(VariousStates_EnterStateCases))]
			public void VariousStates_EnterState_prevNull_CallsInstantMethods(IUISelStateHandler selStateHandler, UIState state){
				selStateHandler.WasSelStateNull().Returns(true);
				state.EnterState();
				
				if(state is UISelectableState)
					selStateHandler.Received().MakeSelectableInstantly();
				else if(state is UIUnselectableState)
					selStateHandler.Received().MakeUnselectableInstantly();
				else if(state is UISelectedState)
					selStateHandler.Received().SelectInstantly();
			}
				class VariousStates_EnterStateCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						TestCaseData case_0;
							IUISelStateHandler selStateHandler_0 = Substitute.For<IUISelStateHandler>();
							IUISelState focusedState_0 = new UISelectableState(selStateHandler_0);
							case_0 = new TestCaseData(
								selStateHandler_0, focusedState_0
							);
							yield return case_0.SetName("FromFocused");
						TestCaseData case_1;
							IUISelStateHandler selStateHandler_1 = Substitute.For<IUISelStateHandler>();
							IUISelState focusedState_1 = new UIUnselectableState(selStateHandler_1);
							case_1 = new TestCaseData(
								selStateHandler_1, focusedState_1
							);
							yield return case_1.SetName("FromDefocused");
						TestCaseData case_2;
							IUISelStateHandler selStateHandler_2 = Substitute.For<IUISelStateHandler>();
							IUISelState focusedState_2 = new UISelectedState(selStateHandler_2);
							case_2 = new TestCaseData(
								selStateHandler_2, focusedState_2
							);
							yield return case_2.SetName("FromSelected");
					}
				}
			[TestCaseSource(typeof(Various_EnterState_FromVariousNonNullCases))]
			public void Various_EnterState_FromVariousNonNull_CallsSSESetAndRunProcessAccordingly<T>(
				IUISelStateHandler selStateHandler,
				IUISelState toState, 
				T process) where T: IUISelProcess
			{
				toState.EnterState();

				selStateHandler.Received().SetAndRunSelProcess(Arg.Any<T>());
			}
				class Various_EnterState_FromVariousNonNullCases: IEnumerable{
					UIDeactivateProcess deaProc = new UIDeactivateProcess(FakeCoroutine);
					UIFocusProcess focProc = new UIFocusProcess(FakeCoroutine);
					UIDefocusProcess defoProc = new UIDefocusProcess(FakeCoroutine);
					UISelectProcess selectProc = new UISelectProcess(FakeCoroutine);
					public IEnumerator GetEnumerator(){
							TestCaseData dea_dea_dea;
									IUISelStateHandler selStateHandler = Substitute.For<IUISelStateHandler>();
									IUISelState dea = new UIDeactivatedState(selStateHandler);
									selStateHandler.WasDeactivated().Returns(true);
								dea_dea_dea =  new TestCaseData(selStateHandler, dea, deaProc); 
								yield return dea_dea_dea.SetName("dea_dea_dea");
							TestCaseData foc_dea_dea;
									IUISelStateHandler selStateHandler_2 = Substitute.For<IUISelStateHandler>();
									IUISelState dea_2 = new UIDeactivatedState(selStateHandler_2);
									selStateHandler_2.WasFocused().Returns(true);
								foc_dea_dea = new TestCaseData(selStateHandler_2, dea_2, deaProc); 
								yield return foc_dea_dea.SetName("foc_dea_dea");
							TestCaseData def_dea_dea;
									IUISelStateHandler selStateHandler_3 = Substitute.For<IUISelStateHandler>();
									IUISelState dea_3 = new UIDeactivatedState(selStateHandler_3);
									selStateHandler_3.WasDefocused().Returns(true);
								def_dea_dea = new TestCaseData(selStateHandler_3, dea_3, deaProc); 
								yield return def_dea_dea.SetName("def_dea_dea");
							TestCaseData sel_dea_dea;
									IUISelStateHandler selStateHandler_4 = Substitute.For<IUISelStateHandler>();
									IUISelState dea_4 = new UIDeactivatedState(selStateHandler_4);
									selStateHandler_4.WasSelected().Returns(true);
								sel_dea_dea = new TestCaseData(selStateHandler_4, dea_4, deaProc); 
								yield return sel_dea_dea.SetName("sel_dea_dea");
							

							TestCaseData dea_foc_foc;
									IUISelStateHandler selStateHandler_5 = Substitute.For<IUISelStateHandler>();
									IUISelState foc_5 = new UISelectableState(selStateHandler_5);
									selStateHandler_5.WasDeactivated().Returns(true);
								dea_foc_foc =  new TestCaseData(selStateHandler_5, foc_5, focProc); 
								yield return dea_foc_foc.SetName("dea_foc_foc");
							TestCaseData foc_foc_foc;
									IUISelStateHandler selStateHandler_6 = Substitute.For<IUISelStateHandler>();
									IUISelState foc_6 = new UISelectableState(selStateHandler_6);
									selStateHandler_6.WasFocused().Returns(true);
								foc_foc_foc = new TestCaseData(selStateHandler_6, foc_6, focProc); 
								yield return foc_foc_foc.SetName("foc_foc_foc");
							TestCaseData def_foc_foc;
									IUISelStateHandler selStateHandler_7 = Substitute.For<IUISelStateHandler>();
									IUISelState foc_7 = new UISelectableState(selStateHandler_7);
									selStateHandler_7.WasDefocused().Returns(true);
								def_foc_foc = new TestCaseData(selStateHandler_7, foc_7, focProc); 
								yield return def_foc_foc.SetName("def_foc_foc");
							TestCaseData sel_foc_foc;
									IUISelStateHandler selStateHandler_8 = Substitute.For<IUISelStateHandler>();
									IUISelState foc_8 = new UISelectableState(selStateHandler_8);
									selStateHandler_8.WasSelected().Returns(true);
								sel_foc_foc = new TestCaseData(selStateHandler_8, foc_8, focProc); 
								yield return sel_foc_foc.SetName("sel_foc_foc");

							TestCaseData dea_def_def;
									IUISelStateHandler selStateHandler_9 = Substitute.For<IUISelStateHandler>();
									IUISelState def_9 = new UIUnselectableState(selStateHandler_9);
									selStateHandler_9.WasDeactivated().Returns(true);
								dea_def_def =  new TestCaseData(selStateHandler_9, def_9, defoProc); 
								yield return dea_def_def.SetName("dea_def_def");
							TestCaseData foc_def_def;
									IUISelStateHandler selStateHandler_10 = Substitute.For<IUISelStateHandler>();
									IUISelState def_10 = new UIUnselectableState(selStateHandler_10);
									selStateHandler_10.WasFocused().Returns(true);
								foc_def_def = new TestCaseData(selStateHandler_10, def_10, defoProc); 
								yield return foc_def_def.SetName("foc_def_def");
							TestCaseData def_def_def;
									IUISelStateHandler selStateHandler_11 = Substitute.For<IUISelStateHandler>();
									IUISelState def_11 = new UIUnselectableState(selStateHandler_11);
									selStateHandler_11.WasDefocused().Returns(true);
								def_def_def = new TestCaseData(selStateHandler_11, def_11, defoProc); 
								yield return def_def_def.SetName("def_def_def");
							TestCaseData sel_def_def;
									IUISelStateHandler selStateHandler_12 = Substitute.For<IUISelStateHandler>();
									IUISelState def_12 = new UIUnselectableState(selStateHandler_12);
									selStateHandler_12.WasSelected().Returns(true);
								sel_def_def = new TestCaseData(selStateHandler_12, def_12, defoProc); 
								yield return sel_def_def.SetName("sel_def_def");

							TestCaseData dea_sel_sel;
									IUISelStateHandler selStateHandler_13 = Substitute.For<IUISelStateHandler>();
									IUISelState sel_13 = new UISelectedState(selStateHandler_13);
									selStateHandler_13.WasDeactivated().Returns(true);
								dea_sel_sel =  new TestCaseData(selStateHandler_13, sel_13, selectProc); 
								yield return dea_sel_sel.SetName("dea_sel_sel");
							TestCaseData foc_sel_sel;
									IUISelStateHandler selStateHandler_14 = Substitute.For<IUISelStateHandler>();
									IUISelState sel_14 = new UISelectedState(selStateHandler_14);
									selStateHandler_14.WasFocused().Returns(true);
								foc_sel_sel = new TestCaseData(selStateHandler_14, sel_14, selectProc); 
								yield return foc_sel_sel.SetName("foc_sel_sel");
							TestCaseData def_sel_sel;
									IUISelStateHandler selStateHandler_15 = Substitute.For<IUISelStateHandler>();
									IUISelState sel_15 = new UISelectedState(selStateHandler_15);
									selStateHandler_15.WasDefocused().Returns(true);
								def_sel_sel = new TestCaseData(selStateHandler_15, sel_15, selectProc); 
								yield return def_sel_sel.SetName("def_sel_sel");
							TestCaseData sel_sel_sel;
									IUISelStateHandler selStateHandler_16 = Substitute.For<IUISelStateHandler>();
									IUISelState sel_16 = new UISelectedState(selStateHandler_16);
									selStateHandler_16.WasSelected().Returns(true);
								sel_sel_sel = new TestCaseData(selStateHandler_16, sel_16, selectProc);
								yield return sel_sel_sel.SetName("sel_sel_sel");
					}
				}
		}
	}
}
