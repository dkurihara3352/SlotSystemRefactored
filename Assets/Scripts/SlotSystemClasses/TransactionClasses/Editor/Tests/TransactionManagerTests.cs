using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Utility;
namespace SlotSystemTests{
	namespace ElementsTests{
		[TestFixture]
		[Category("TAM")]
		public class TransactionManagerTests: SlotSystemTest {
			[Test]
			public void Refresh_Always_SetsActStateWFA(){
				TransactionManager tam = MakeTAM();
					TAMActStateHandler stateHandler = new TAMActStateHandler(tam);
					tam.SetActStateHandler(stateHandler);

				tam.Refresh();

				Assert.That(stateHandler.IsWaitingForAction(), Is.True);
				}

			[TestCaseSource(typeof(transactionCoroutineCases))]
			public void transactionCoroutine_WhenAllDone_CallsActProcExpire(DraggedIcon di1, DraggedIcon di2, ISlotGroup sg1, ISlotGroup sg2, bool called){
				TransactionManager tam = MakeTAM();
					TAMActStateHandler tamStateHandler = new TAMActStateHandler(tam);
					tam.SetActStateHandler(tamStateHandler);
				ITransactionSGHandler stubSGHandler = Substitute.For<ITransactionSGHandler>();
					stubSGHandler.IsSG1Done().Returns(sg1 == null);
					stubSGHandler.IsSG2Done().Returns(sg2 == null);
				tam.SetSGHandler(stubSGHandler);
				ITransactionIconHandler stubIconHandler = Substitute.For<ITransactionIconHandler>();
					stubIconHandler.IsDIcon1Done().Returns(di1 == null);
					stubIconHandler.IsDIcon2Done().Returns(di2 == null);
				tam.SetIconHandler(stubIconHandler);
				ITAMActProcess mockProc = Substitute.For<ITAMActProcess>();
				tamStateHandler.SetAndRunActProcess(mockProc);

				tamStateHandler.transactionCoroutine();

				if(called)
					mockProc.Received().Expire();
				else
					mockProc.DidNotReceive().Expire();
				}
				class transactionCoroutineCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] allDone_T;
							allDone_T = new object[]{null, null, null, null, true};
							yield return allDone_T;
						object[] di1NotDone_F;
							ISlottable stubSB_1 = MakeSubSB();
							DraggedIcon di1 = new DraggedIcon(stubSB_1, MakeSubIconHandler());
							di1NotDone_F = new object[]{di1, null, null, null, false};
							yield return di1NotDone_F;
						object[] di2NotDone_F;
							ISlottable stubSB_2 = MakeSubSB();
							DraggedIcon di2 = new DraggedIcon(stubSB_2, MakeSubIconHandler());
							di2NotDone_F = new object[]{null, di2, null, null, false};
							yield return di2NotDone_F;
						object[] sg1NotDone_F;
							ISlotGroup sg1 = MakeSubSG();
							sg1NotDone_F = new object[]{null, null, sg1, null, false};
							yield return sg1NotDone_F;
						object[] sg2NotDone_F;
							ISlotGroup sg2 = MakeSubSG();
							sg2NotDone_F = new object[]{null, null, null, sg2, false};
							yield return sg2NotDone_F;
					}
				}
			[Test]
			public void SetTransaction_PrevTransactionNull_SetsTransaction(){
				TransactionManager tam = MakeTAM();
				ISlotSystemTransaction stubTA = MakeSubTA();

				tam.SetTransaction(stubTA);

				Assert.That(tam.transaction, Is.SameAs(stubTA));
				}
			[Test]
			public void SetTransaction_PrevTransactionNull_CallsTAIndicate(){
				TransactionManager tam = MakeTAM();
				ISlotSystemTransaction mockTA = MakeSubTA();

				tam.SetTransaction(mockTA);

				mockTA.Received().Indicate();
				}
			[Test]
			public void SetTransaction_DiffTA_SetsTransaction(){
				TransactionManager tam = MakeTAM();
				ISlotSystemTransaction prevTA = MakeSubTA();
				ISlotSystemTransaction stubTA = MakeSubTA();
				tam.SetTransaction(prevTA);
				tam.SetTransaction(stubTA);

				Assert.That(tam.transaction, Is.SameAs(stubTA));
				}
			[Test]
			public void SetTransaction_DiffTA_CallsTAIndicate(){
				TransactionManager tam = MakeTAM();
				ISlotSystemTransaction prevTA = MakeSubTA();
				ISlotSystemTransaction mockTA = MakeSubTA();

				tam.SetTransaction(prevTA);
				tam.SetTransaction(mockTA);

				mockTA.Received().Indicate();
				}
			[Test]
			public void SetTransaction_SameTA_DoesNotCallIndicateTwice(){
				TransactionManager tam = MakeTAM();
				ISlotSystemTransaction mockTA = MakeSubTA();

				tam.SetTransaction(mockTA);
				tam.SetTransaction(mockTA);

				mockTA.Received(1).Indicate();
				}
			[Test]
			public void SetTransaction_Null_SetsTANull(){
				TransactionManager tam = MakeTAM();
				ISlotSystemTransaction stubTA = MakeSubTA();

				tam.SetTransaction(stubTA);
				tam.SetTransaction(null);

				Assert.That(tam.transaction, Is.Null);
				}
			[Test]
			public void ExecuteTransaction_Always_SetsActStatTransaction(){
				TransactionManager tam = MakeTAM();
				ISlotSystemTransaction stubTA = MakeSubTA();
				tam.SetTransaction(stubTA);
				TAMActStateHandler tamStateHandler = new TAMActStateHandler(tam);
				tam.SetActStateHandler(tamStateHandler);
				
				tam.ExecuteTransaction();

				Assert.That(tamStateHandler.IsTransacting(), Is.True);
				}
			[Test]
			public void InnerUpdateFields_Always_SetsFields(){
				TransactionManager tam = MakeTAM();
					TAMActStateHandler tamStateHandler = new TAMActStateHandler(tam);
					tam.SetActStateHandler(tamStateHandler);
				ISlotSystemTransaction stubTA = Substitute.For<ISlotSystemTransaction>();
					ISlotGroup stubSG1 = MakeSubSG();
					ISlotGroup stubSG2 = MakeSubSG();
					stubTA.sg1.Returns(stubSG1);
					stubTA.sg2.Returns(stubSG2);
				TransactionSGHandler sgHandler = new TransactionSGHandler(MakeSubTAMStateHandler());
				tam.SetSGHandler(sgHandler);
				
				tam.UpdateFields(stubTA);

				Assert.That(sgHandler.GetSG1(), Is.SameAs(stubSG1));
				Assert.That(sgHandler.GetSG2(), Is.SameAs(stubSG2));
				Assert.That(tam.transaction, Is.SameAs(stubTA));
			}
		}
	}
}

