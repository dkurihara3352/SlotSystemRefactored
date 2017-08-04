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
			public void Refresh_WhenCalled_SetsActStateWFA(){
				TransactionManager tam = MakeTAM();

				tam.Refresh();

				Assert.That(tam.isWaitingForAction, Is.True);
				}

			[TestCaseSource(typeof(transactionCoroutineCases))]
			public void transactionCoroutine_WhenAllDone_CallsActProcExpire(DraggedIcon di1, DraggedIcon di2, ISlotGroup sg1, ISlotGroup sg2, bool called){
				TransactionManager tam = MakeTAM();
				tam.SetDIcon1(di1);
				tam.SetDIcon2(di2);
				ITransactionSGHandler stubSGHandler = Substitute.For<ITransactionSGHandler>();
					stubSGHandler.sg1Done.Returns(sg1 == null);
					stubSGHandler.sg2Done.Returns(sg2 == null);
					
				tam.SetSGHandler(stubSGHandler);
				ITAMActProcess mockProc = Substitute.For<ITAMActProcess>();
				tam.SetAndRunActProcess(mockProc);

				tam.transactionCoroutine();

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
							DraggedIcon di1 = new DraggedIcon(stubSB_1, MakeSubTAM());
							di1NotDone_F = new object[]{di1, null, null, null, false};
							yield return di1NotDone_F;
						object[] di2NotDone_F;
							ISlottable stubSB_2 = MakeSubSB();
							DraggedIcon di2 = new DraggedIcon(stubSB_2, MakeSubTAM());
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
			public void ExecuteTransaction_WhenCalled_SetsActStatTransaction(){
				TransactionManager tam = MakeTAM();
				ISlotSystemTransaction stubTA = MakeSubTA();
				tam.SetTransaction(stubTA);
				
				tam.ExecuteTransaction();

				Assert.That(tam.isTransacting, Is.True);
				}
			[Test]
			public void InnerUpdateFields_Always_SetsFields(){
				TransactionManager tam = MakeTAM();
				ISlotSystemTransaction stubTA = Substitute.For<ISlotSystemTransaction>();
					ISlotGroup stubSG1 = MakeSubSG();
					ISlotGroup stubSG2 = MakeSubSG();
					stubTA.sg1.Returns(stubSG1);
					stubTA.sg2.Returns(stubSG2);
				
				tam.UpdateFields(stubTA);

				Assert.That(tam.sg1, Is.SameAs(stubSG1));
				Assert.That(tam.sg2, Is.SameAs(stubSG2));
				Assert.That(tam.transaction, Is.SameAs(stubTA));
			}
		}
	}
}

