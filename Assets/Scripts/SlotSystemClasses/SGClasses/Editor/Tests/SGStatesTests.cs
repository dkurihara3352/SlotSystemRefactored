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
		public class SGStatesTests: AbsSlotSystemTest{
			/*	ActState	*/
				[TestCaseSource(typeof(VariousSGActStateEnterStateCases))]
				public void VariousSGActState_EnterState_FromWFAState_SetsSGActProcSGTransactionProces(SGActState toState){
					ISlotGroup mockSG = MakeSubSG();
					mockSG.prevActState.Returns(SlotGroup.sgWaitForActionState);
					
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
	}
}
