using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections;
using System.Collections.Generic;
using NSubstitute;

namespace SlotSystemTests{
	namespace SGTests{
		[TestFixture]
		public class SGStatesTests: AbsSlotSystemTest {
			[Test]
			public void SGSelState_OnHoverEnter_WhenCalled_CallSGSetHovered(){
				TestSGSelState sgSelState = new TestSGSelState();
				ISlotGroup mockSG = MakeSubSG();				

				sgSelState.OnHoverEnterMock(mockSG, new PointerEventDataFake());

				mockSG.Received().SetHovered();
			}
			/*	SelStates */
				[TestCaseSource(typeof(VariousSGSelStatesEnterStateCases1))]
				public void VariousSGSelStates_EnteState_FromDeactivated_SetsSGSelProcNull(SGSelState toState){
					ISlotGroup mockSG = MakeSubSG();
					mockSG.prevSelState = SlotGroup.sgDeactivatedState;

					toState.EnterState(mockSG);

					mockSG.Received().SetAndRunSelProcess(null);
				}
					class VariousSGSelStatesEnterStateCases1: IEnumerable{
						public IEnumerator GetEnumerator(){
							yield return SlotGroup.sgFocusedState;
							yield return SlotGroup.sgDefocusedState;
							yield return SlotGroup.sgSelectedState;
						}
					}
				[TestCaseSource(typeof(VariousSGSelStateEnterStateCases2))]
				public void VariousSGSelState_EnterState_FromDeactivatedState_CallsSGInstantMethods(SGSelState toState, InstantMethod instantMethod){
					ISlotGroup mockSG = MakeSubSG();
					mockSG.prevSelState = SlotGroup.sgDeactivatedState;

					toState.EnterState(mockSG);

					switch(instantMethod){
						case InstantMethod.InstantGreyin :
							mockSG.Received().InstantGreyin();
						break;
						case InstantMethod.InstantGreyout :
							mockSG.Received().InstantGreyout();
						break;
						case InstantMethod.InstantHighlight :
							mockSG.Received().InstantHighlight();
						break;
					}
				}
					public enum InstantMethod{
						InstantGreyin,
						InstantGreyout,
						InstantHighlight
					}
					class VariousSGSelStateEnterStateCases2: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case1 = new object[]{
								SlotGroup.sgFocusedState, InstantMethod.InstantGreyin
							};
							yield return case1;

							object[] case2 = new object[]{
								SlotGroup.sgDefocusedState, InstantMethod.InstantGreyout
							};
							yield return case2;

							object[] case3 = new object[]{
								SlotGroup.sgSelectedState, InstantMethod.InstantHighlight
							};
							yield return case3;
						}
					}
				[TestCaseSource(typeof(SGFocusedStateEnterStateCases))]
				public void VariousSGSelState_EnteState_FromVarious_SetsSGSelProcAccordingly<T>(SGSelState fromState, SGSelState toState, T selProc) where T: SGSelProcess{
					ISlotGroup mockSG = MakeSubSG();
					mockSG.prevSelState = fromState;

					toState.EnterState(mockSG);

					mockSG.Received().SetAndRunSelProcess(Arg.Any<T>());
				}
					class SGFocusedStateEnterStateCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case1 = new object[]{
								SlotGroup.sgDefocusedState,
								SlotGroup.sgFocusedState,
								new SGGreyinProcess(MakeSubSG(), FakeCoroutine)
							};
							yield return case1;

							object[] case2 = new object[]{
								SlotGroup.sgSelectedState,
								SlotGroup.sgFocusedState,
								new SGDehighlightProcess(MakeSubSG(), FakeCoroutine)
							};
							yield return case2;

							object[] case3 = new object[]{
								SlotGroup.sgFocusedState,
								SlotGroup.sgDefocusedState,
								new SGGreyoutProcess(MakeSubSG(), FakeCoroutine)
							};
							yield return case3;

							object[] case4 = new object[]{
								SlotGroup.sgSelectedState,
								SlotGroup.sgDefocusedState,
								new SGGreyoutProcess(MakeSubSG(), FakeCoroutine)
							};
							yield return case4;
							
							object[] case5 = new object[]{
								SlotGroup.sgFocusedState,
								SlotGroup.sgSelectedState,
								new SGHighlightProcess(MakeSubSG(), FakeCoroutine)
							};
							yield return case5;
							
							object[] case6 = new object[]{
								SlotGroup.sgDefocusedState,
								SlotGroup.sgSelectedState,
								new SGHighlightProcess(MakeSubSG(), FakeCoroutine)
							};
							yield return case6;
						}
					}
			/*	ActState	*/
				[TestCaseSource(typeof(VariousSGActStateEnterStateCases))]
				public void VariousSGActState_EnterState_FromWFAState_SetsSGActProcSGTransactionProces(SGActState toState){
					ISlotGroup mockSG = MakeSubSG();
					mockSG.prevSelState = SlotGroup.sgWaitForActionState;
					// mockSG.SetToList(new List<Slottable>());
					
					toState.EnterState(mockSG);

					mockSG.Received().SetAndRunActProcess(Arg.Any<SGTransactionProcess>());
				}
					class VariousSGActStateEnterStateCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							yield return SlotGroup.revertState;
							yield return SlotGroup.reorderState;
							yield return SlotGroup.sortState;
							yield return SlotGroup.fillState;
							yield return SlotGroup.swapState;
							yield return SlotGroup.addState;
							yield return SlotGroup.removeState;
						}
					}
				[Test]
				public void SGWaitForActionState_EnterState_WhenCalled_SetsSGActProcNull(){
					SGWaitForActionState sgwfaState = new SGWaitForActionState();
					ISlotGroup mockSG = MakeSubSG();

					sgwfaState.EnterState(mockSG);

					mockSG.Received().SetAndRunActProcess(null);
				}
				[Test]
				public void SGRevertState_EnterState_WhenCalled_CallsSGUpdateToRevert(){
					SGRevertState revState = new SGRevertState();
					ISlotGroup mockSG = MakeSubSG();

					revState.EnterState(mockSG);

					mockSG.Received().UpdateToRevert();

				}
				[Test]
				public void SGReorderState_EnterState_WhenCalled_CallsSGReorderAndUpdateSBs(){
					SGReorderState roState = new SGReorderState();
					ISlotGroup mockSG = MakeSubSG();

					roState.EnterState(mockSG);

					mockSG.Received().ReorderAndUpdateSBs();
				}
				[Test]
				public void SGSortState_EnterState_WhenCalled_CallsSGSortAndUpdateSBs(){
					SGSortState sortState = new SGSortState();
					ISlotGroup mockSG = MakeSubSG();

					sortState.EnterState(mockSG);

					mockSG.Received().SortAndUpdateSBs();
				}
				[Test]
				public void SGFillState_EnterState_WhenCalled_CallsSGFillAndUpdateSBs(){
					SGFillState fillState = new SGFillState();
					ISlotGroup mockSG = MakeSubSG();

					fillState.EnterState(mockSG);

					mockSG.Received().FillAndUpdateSBs();
				}
				[Test]
				public void SGSwapState_EnterState_WhenCalled_CallsSGSwapAndUpdateSBs(){
					SGSwapState fillState = new SGSwapState();
					ISlotGroup mockSG = MakeSubSG();

					fillState.EnterState(mockSG);

					mockSG.Received().SwapAndUpdateSBs();
				}
				[Test]
				public void SGAddState_EnterState_WhenCalled_CallsSGAddAndUpdateSBs(){
					SGAddState fillState = new SGAddState();
					ISlotGroup mockSG = MakeSubSG();

					fillState.EnterState(mockSG);

					mockSG.Received().AddAndUpdateSBs();
				}
				[Test]
				public void SGRemoveState_EnterState_WhenCalled_CallsSGRemoveAndUpdateSBs(){
					SGRemoveState fillState = new SGRemoveState();
					ISlotGroup mockSG = MakeSubSG();

					fillState.EnterState(mockSG);

					mockSG.Received().RemoveAndUpdateSBs();
				}
		}
		/*	helpers */
		class TestSGSelState: SGSelState{}
		class TestSGSelProcess: SGSelProcess{}
		class TestSGActProcess: SGActProcess{}
	}
}
