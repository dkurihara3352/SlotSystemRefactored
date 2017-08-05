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
				public void VariousSGActState_EnterState_FromWFAState_SetsSGActProcSGTransactionProces(ISGActStateHandler handler , ISlotGroup mockSG, SGActState toState){
					SGWaitForActionState wfaState = new SGWaitForActionState(handler, mockSG);
						handler.waitForActionState.Returns(wfaState);
					handler.wasWaitingForAction.Returns(true);
					
					toState.EnterState();

					handler.Received().SetAndRunActProcess(Arg.Any<SGTransactionProcess>());
					}
					class VariousSGActStateEnterStateCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] revert;
								ISlotGroup sg_0 = MakeSubSG();
									ISGActStateHandler handler_0 = Substitute.For<ISGActStateHandler>();
									SGRevertState revertState = new SGRevertState(handler_0, sg_0);
									sg_0.revertState.Returns(revertState);
								revert = new object[]{handler_0, sg_0, sg_0.revertState};
								yield return revert;
							object[] reorder;
								ISlotGroup sg_1 = MakeSubSG();
									ISGActStateHandler handler_1 = Substitute.For<ISGActStateHandler>();
									SGReorderState reorderState = new SGReorderState(handler_1, sg_1);
									sg_1.reorderState.Returns(reorderState);
								reorder = new object[]{handler_1, sg_1, sg_1.reorderState};
								yield return reorder;
							object[] sort;
								ISlotGroup sg_2 = MakeSubSG();
									ISGActStateHandler handler_2 = Substitute.For<ISGActStateHandler>();
									SGSortState sortState = new SGSortState(handler_2, sg_2);
									sg_2.sortState.Returns(sortState);
								sort = new object[]{handler_2, sg_2, sg_2.sortState};
								yield return sort;
							object[] fill;
								ISlotGroup sg_3 = MakeSubSG();
									ISGActStateHandler handler_3 = Substitute.For<ISGActStateHandler>();
									SGFillState fillState = new SGFillState(handler_3, sg_3);
									sg_3.fillState.Returns(fillState);
								fill = new object[]{handler_3, sg_3, sg_3.fillState};
								yield return fill;
							object[] swap;
								ISlotGroup sg_4 = MakeSubSG();
									ISGActStateHandler handler_4 = Substitute.For<ISGActStateHandler>();
									SGSwapState swapState = new SGSwapState(handler_4, sg_4);
									sg_4.swapState.Returns(swapState);
								swap = new object[]{handler_4, sg_4, sg_4.swapState};
								yield return swap;
							object[] add;
								ISlotGroup sg_5 = MakeSubSG();
									ISGActStateHandler handler_5 = Substitute.For<ISGActStateHandler>();
									SGAddState addState = new SGAddState(handler_5, sg_5);
									sg_5.addState.Returns(addState);
								add = new object[]{handler_5, sg_5, sg_5.addState};
								yield return add;
							object[] remove;
								ISlotGroup sg_6 = MakeSubSG();
									ISGActStateHandler handler_6 = Substitute.For<ISGActStateHandler>();
									SGRemoveState removeState = new SGRemoveState(handler_6, sg_6);
									sg_6.removeState.Returns(removeState);
								remove = new object[]{sg_6, sg_6.removeState};
								yield return remove;
						}
					}
				[Test]
				public void SGWaitForActionState_EnterState_WhenCalled_SetsSGActProcNull(){
					ISlotGroup mockSG = MakeSubSG();
					ISGActStateHandler handler = Substitute.For<ISGActStateHandler>();
					SGWaitForActionState sgwfaState = new SGWaitForActionState(handler, mockSG);

					sgwfaState.EnterState();

					handler.Received().SetAndRunActProcess(null);
					}
				[Test]
				public void SGRevertState_EnterState_WhenCalled_CallsSGUpdateToRevert(){
					ISlotGroup mockSG = MakeSubSG();
					SGActStateHandler handler = new SGActStateHandler(mockSG);
					SGRevertState revState = new SGRevertState(handler, mockSG);

					revState.EnterState();

					mockSG.Received().RevertAndUpdateSBs();

					}
				[Test]
				public void SGReorderState_EnterState_WhenCalled_CallsSGReorderAndUpdateSBs(){
					ISlotGroup mockSG = MakeSubSG();
					SGActStateHandler handler = new SGActStateHandler(mockSG);
					SGReorderState roState = new SGReorderState(handler, mockSG);

					roState.EnterState();

					mockSG.Received().ReorderAndUpdateSBs();
					}
				[Test]
				public void SGSortState_EnterState_WhenCalled_CallsSGSortAndUpdateSBs(){
					ISlotGroup mockSG = MakeSubSG();
					SGActStateHandler handler = new SGActStateHandler(mockSG);
					SGSortState sortState = new SGSortState(handler, mockSG);

					sortState.EnterState();

					mockSG.Received().SortAndUpdateSBs();
					}
				[Test]
				public void SGFillState_EnterState_WhenCalled_CallsSGFillAndUpdateSBs(){
					ISlotGroup mockSG = MakeSubSG();
					SGActStateHandler handler = new SGActStateHandler(mockSG);
					SGFillState fillState = new SGFillState(handler, mockSG);

					fillState.EnterState();

					mockSG.Received().FillAndUpdateSBs();
					}
				[Test]
				public void SGSwapState_EnterState_WhenCalled_CallsSGSwapAndUpdateSBs(){
					ISlotGroup mockSG = MakeSubSG();
					SGActStateHandler handler = new SGActStateHandler(mockSG);
					SGSwapState fillState = new SGSwapState(handler, mockSG);

					fillState.EnterState();

					mockSG.Received().SwapAndUpdateSBs();
					}
				[Test]
				public void SGAddState_EnterState_WhenCalled_CallsSGAddAndUpdateSBs(){
					ISlotGroup mockSG = MakeSubSG();
					SGActStateHandler handler = new SGActStateHandler(mockSG);
					SGAddState fillState = new SGAddState(handler, mockSG);

					fillState.EnterState();

					mockSG.Received().AddAndUpdateSBs();
					}
				[Test]
				public void SGRemoveState_EnterState_WhenCalled_CallsSGRemoveAndUpdateSBs(){
					ISlotGroup mockSG = MakeSubSG();
					SGActStateHandler handler = new SGActStateHandler(mockSG);
					SGRemoveState fillState = new SGRemoveState(handler, mockSG);

					fillState.EnterState();

					mockSG.Received().RemoveAndUpdateSBs();
					}
		}
	}
}
