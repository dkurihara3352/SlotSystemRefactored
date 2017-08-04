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
		[Category("SG")]
		public class SGStatesTests: SlotSystemTest{
			/*	ActState	*/
				[TestCaseSource(typeof(VariousSGActStateEnterStateCases))]
				public void VariousSGActState_EnterState_FromWFAState_SetsSGActProcSGTransactionProces(ISlotGroup mockSG, SGActState toState){
					SGWaitForActionState wfaState = new SGWaitForActionState(mockSG);
						mockSG.waitForActionState.Returns(wfaState);
					mockSG.wasWaitingForAction.Returns(true);
					
					toState.EnterState();

					mockSG.Received().SetAndRunActProcess(Arg.Any<SGTransactionProcess>());
					}
					class VariousSGActStateEnterStateCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] revert;
								ISlotGroup sg_0 = MakeSubSG();
									SGRevertState revertState = new SGRevertState(sg_0);
									sg_0.revertState.Returns(revertState);
								revert = new object[]{sg_0, sg_0.revertState};
								yield return revert;
							object[] reorder;
								ISlotGroup sg_1 = MakeSubSG();
									SGReorderState reorderState = new SGReorderState(sg_1);
									sg_1.reorderState.Returns(reorderState);
								reorder = new object[]{sg_1, sg_1.reorderState};
								yield return reorder;
							object[] sort;
								ISlotGroup sg_2 = MakeSubSG();
									SGSortState sortState = new SGSortState(sg_2);
									sg_2.sortState.Returns(sortState);
								sort = new object[]{sg_2, sg_2.sortState};
								yield return sort;
							object[] fill;
								ISlotGroup sg_3 = MakeSubSG();
									SGFillState fillState = new SGFillState(sg_3);
									sg_3.fillState.Returns(fillState);
								fill = new object[]{sg_3, sg_3.fillState};
								yield return fill;
							object[] swap;
								ISlotGroup sg_4 = MakeSubSG();
									SGSwapState swapState = new SGSwapState(sg_4);
									sg_4.swapState.Returns(swapState);
								swap = new object[]{sg_4, sg_4.swapState};
								yield return swap;
							object[] add;
								ISlotGroup sg_5 = MakeSubSG();
									SGAddState addState = new SGAddState(sg_5);
									sg_5.addState.Returns(addState);
								add = new object[]{sg_5, sg_5.addState};
								yield return add;
							object[] remove;
								ISlotGroup sg_6 = MakeSubSG();
									SGRemoveState removeState = new SGRemoveState(sg_6);
									sg_6.removeState.Returns(removeState);
								remove = new object[]{sg_6, sg_6.removeState};
								yield return remove;
						}
					}
				[Test]
				public void SGWaitForActionState_EnterState_WhenCalled_SetsSGActProcNull(){
					ISlotGroup mockSG = MakeSubSG();
					SGWaitForActionState sgwfaState = new SGWaitForActionState(mockSG);

					sgwfaState.EnterState();

					mockSG.Received().SetAndRunActProcess(null);
					}
				[Test]
				public void SGRevertState_EnterState_WhenCalled_CallsSGUpdateToRevert(){
					ISlotGroup mockSG = MakeSubSG();
					SGRevertState revState = new SGRevertState(mockSG);

					revState.EnterState();

					mockSG.Received().RevertAndUpdateSBs();

					}
				[Test]
				public void SGReorderState_EnterState_WhenCalled_CallsSGReorderAndUpdateSBs(){
					ISlotGroup mockSG = MakeSubSG();
					SGReorderState roState = new SGReorderState(mockSG);

					roState.EnterState();

					mockSG.Received().ReorderAndUpdateSBs();
					}
				[Test]
				public void SGSortState_EnterState_WhenCalled_CallsSGSortAndUpdateSBs(){
					ISlotGroup mockSG = MakeSubSG();
					SGSortState sortState = new SGSortState(mockSG);

					sortState.EnterState();

					mockSG.Received().SortAndUpdateSBs();
					}
				[Test]
				public void SGFillState_EnterState_WhenCalled_CallsSGFillAndUpdateSBs(){
					ISlotGroup mockSG = MakeSubSG();
					SGFillState fillState = new SGFillState(mockSG);

					fillState.EnterState();

					mockSG.Received().FillAndUpdateSBs();
					}
				[Test]
				public void SGSwapState_EnterState_WhenCalled_CallsSGSwapAndUpdateSBs(){
					ISlotGroup mockSG = MakeSubSG();
					SGSwapState fillState = new SGSwapState(mockSG);

					fillState.EnterState();

					mockSG.Received().SwapAndUpdateSBs();
					}
				[Test]
				public void SGAddState_EnterState_WhenCalled_CallsSGAddAndUpdateSBs(){
					ISlotGroup mockSG = MakeSubSG();
					SGAddState fillState = new SGAddState(mockSG);

					fillState.EnterState();

					mockSG.Received().AddAndUpdateSBs();
					}
				[Test]
				public void SGRemoveState_EnterState_WhenCalled_CallsSGRemoveAndUpdateSBs(){
					ISlotGroup mockSG = MakeSubSG();
					SGRemoveState fillState = new SGRemoveState(mockSG);

					fillState.EnterState();

					mockSG.Received().RemoveAndUpdateSBs();
					}
		}
	}
}
