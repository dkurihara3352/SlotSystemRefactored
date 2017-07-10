using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections;
using System.Collections.Generic;

namespace SlotSystemTests{
	namespace SGTests{
		[TestFixture]
		public class SGStatesTests: AbsSlotSystemTest {
			[Test]
			public void SGSelState_OnHoverEnter_WhenCalled_CallSGSetHovered(){
				TestSGSelState sgSelState = new TestSGSelState();
				FakeSG mockSG = MakeFakeSG();
				mockSG.ResetCallChecks();

				sgSelState.OnHoverEnterMock(mockSG, new PointerEventDataFake());

				Assert.That(mockSG.IsSetHoveredCalled, Is.True);

				mockSG.ResetCallChecks();
			}
			/*	SelStates */
				[TestCaseSource(typeof(VariousSGSelStatesEnterStateCases1))]
				public void VariousSGSelStates_EnteState_FromDeactivated_SetsSGSelProcNull(SGSelState toState){
					FakeSG mockSG = MakeFakeSG();
					mockSG.SetPrevSelState(SlotGroup.sgDeactivatedState);
					mockSG.SetAndRunSelProcess(new TestSGSelProcess());

					toState.EnterState(mockSG);

					Assert.That(mockSG.selProcess, Is.Null);
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
					FakeSG mockSG = MakeFakeSG();
					mockSG.SetPrevSelState(SlotGroup.sgDeactivatedState);
					mockSG.ResetCallChecks();

					toState.EnterState(mockSG);
					bool targetCallCheck = false;
					switch(instantMethod){
						case InstantMethod.InstantGreyin :
							targetCallCheck = mockSG.IsInstantGreyinCalled;
						break;
						case InstantMethod.InstantGreyout :
							targetCallCheck = mockSG.IsInstantGreyoutCalled;
						break;
						case InstantMethod.InstantHighlight :
							targetCallCheck = mockSG.IsInstantHighlightCalled;
						break;
					}
					Assert.That(targetCallCheck, Is.True);
					mockSG.ResetCallChecks();
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
				public void VariousSGSelState_EnteState_FromVarious_SetsSGSelProcAccordingly(SGSelState fromState, SGSelState toState, System.Type procType){
					FakeSG mockSG = MakeFakeSG();
					mockSG.SetPrevSelState(fromState);

					toState.EnterState(mockSG);

					Assert.That(mockSG.selProcess, Is.TypeOf(procType));
				}
					class SGFocusedStateEnterStateCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case1 = new object[]{
								SlotGroup.sgDefocusedState,
								SlotGroup.sgFocusedState,
								typeof(SGGreyinProcess)
							};
							yield return case1;

							object[] case2 = new object[]{
								SlotGroup.sgSelectedState,
								SlotGroup.sgFocusedState,
								typeof(SGDehighlightProcess)
							};
							yield return case2;

							object[] case3 = new object[]{
								SlotGroup.sgFocusedState,
								SlotGroup.sgDefocusedState,
								typeof(SGGreyoutProcess)
							};
							yield return case3;

							object[] case4 = new object[]{
								SlotGroup.sgSelectedState,
								SlotGroup.sgDefocusedState,
								typeof(SGGreyoutProcess)
							};
							yield return case4;
							
							object[] case5 = new object[]{
								SlotGroup.sgFocusedState,
								SlotGroup.sgSelectedState,
								typeof(SGHighlightProcess)
							};
							yield return case5;
							
							object[] case6 = new object[]{
								SlotGroup.sgDefocusedState,
								SlotGroup.sgSelectedState,
								typeof(SGHighlightProcess)
							};
							yield return case6;
						}
					}
			/*	ActState	*/
				[TestCaseSource(typeof(VariousSGActStateEnterStateCases))]
				public void VariousSGActState_EnterState_FromWFAState_SetsSGActProcSGTransactionProces(SGActState toState){
					FakeSG mockSG = MakeFakeSG();
					mockSG.SetPrevActState(SlotGroup.sgWaitForActionState);
					mockSG.SetToList(new List<Slottable>());
					
					toState.EnterState(mockSG);

					Assert.That(mockSG.actProcess, Is.TypeOf(typeof(SGTransactionProcess)));
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
					FakeSG mockSG = MakeFakeSG();
					mockSG.SetAndRunActProcess(new TestSGActProcess());

					sgwfaState.EnterState(mockSG);

					Assert.That(mockSG.actProcess, Is.Null);
				}
				[Test]
				public void SGRevertState_EnterState_WhenCalled_CallsSGUpdateToRevert(){
					SGRevertState revState = new SGRevertState();
					FakeSG mockSG = MakeFakeSG();
					mockSG.ResetCallChecks();

					revState.EnterState(mockSG);

					Assert.That(mockSG.IsUpdateToRevertCalled, Is.True);

					mockSG.ResetCallChecks();
				}
				[Test]
				public void SGReorderState_EnterState_WhenCalled_CallsSGReorderAndUpdateSBs(){
					SGReorderState roState = new SGReorderState();
					FakeSG mockSG = MakeFakeSG();
					mockSG.ResetCallChecks();

					roState.EnterState(mockSG);

					Assert.That(mockSG.IsReorderAndUpdateSBsCalled, Is.True);

					mockSG.ResetCallChecks();
				}
				[Test]
				public void SGSortState_EnterState_WhenCalled_CallsSGSortAndUpdateSBs(){
					SGSortState sortState = new SGSortState();
					FakeSG mockSG = MakeFakeSG();
					mockSG.ResetCallChecks();

					sortState.EnterState(mockSG);

					Assert.That(mockSG.IsSortAndUpdateSBsCalled, Is.True);

					mockSG.ResetCallChecks();
				}
				[Test]
				public void SGFillState_EnterState_WhenCalled_CallsSGFillAndUpdateSBs(){
					SGFillState fillState = new SGFillState();
					FakeSG mockSG = MakeFakeSG();
					mockSG.ResetCallChecks();

					fillState.EnterState(mockSG);

					Assert.That(mockSG.IsFillAndUpdateSBsCalled, Is.True);

					mockSG.ResetCallChecks();
				}
				[Test]
				public void SGSwapState_EnterState_WhenCalled_CallsSGSwapAndUpdateSBs(){
					SGSwapState fillState = new SGSwapState();
					FakeSG mockSG = MakeFakeSG();
					mockSG.ResetCallChecks();

					fillState.EnterState(mockSG);

					Assert.That(mockSG.IsSwapAndUpdateSBsCalled, Is.True);

					mockSG.ResetCallChecks();
				}
				[Test]
				public void SGAddState_EnterState_WhenCalled_CallsSGAddAndUpdateSBs(){
					SGAddState fillState = new SGAddState();
					FakeSG mockSG = MakeFakeSG();
					mockSG.ResetCallChecks();

					fillState.EnterState(mockSG);

					Assert.That(mockSG.IsAddAndUpdateSBsCalled, Is.True);

					mockSG.ResetCallChecks();
				}
				[Test]
				public void SGRemoveState_EnterState_WhenCalled_CallsSGRemoveAndUpdateSBs(){
					SGRemoveState fillState = new SGRemoveState();
					FakeSG mockSG = MakeFakeSG();
					mockSG.ResetCallChecks();

					fillState.EnterState(mockSG);

					Assert.That(mockSG.IsRemoveAndUpdateSBsCalled, Is.True);

					mockSG.ResetCallChecks();
				}
		}
		/*	helpers */
		class TestSGSelState: SGSelState{}
		class TestSGSelProcess: SGSelProcess{}
		class TestSGActProcess: SGActProcess{}
	}
}
