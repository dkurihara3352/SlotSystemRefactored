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
			[Test]
			public void Refresh_WhenCalled_SetsFieldsNull(){
				TransactionManager tam = MakeTAM();
				RefreshTestCase expected = new RefreshTestCase(
					null, null, null, null, null, null, null, null
				);

				tam.Refresh();

				RefreshTestCase actual = new RefreshTestCase(
					tam.pickedSB, tam.targetSB, tam.sg1, tam.sg2, tam.hovered,
					tam.dIcon1, tam.dIcon2, tam.transaction
				);
				bool equality = actual.Equals(expected);
				Assert.That(equality, Is.True);
				}
				class RefreshTestCase: IEquatable<RefreshTestCase>{
					public ISlottable pickedSB;
					public ISlottable targetSB;
					public ISlotGroup sg1;
					public ISlotGroup sg2;
					public ISlotSystemElement hovered;
					public DraggedIcon dIcon1;
					public DraggedIcon dIcon2;
					public ISlotSystemTransaction transaction;
					public RefreshTestCase(ISlottable pickedSB, ISlottable targetSB, ISlotGroup sg1, ISlotGroup sg2, ISlotSystemElement hovered, DraggedIcon dIcon1, DraggedIcon dIcon2, ISlotSystemTransaction transaction){
						this.pickedSB = pickedSB;
						this.targetSB = targetSB;
						this.sg1 = sg1;
						this.sg2 = sg2;
						this.hovered = hovered;
						this.dIcon1 = dIcon1;
						this.dIcon2 = dIcon2;
						this.transaction = transaction;
					}
					public bool Equals(RefreshTestCase other){
						bool flag = true;
						flag &= BothNullOrReferenceEquals(this.pickedSB, other.pickedSB);
						flag &= BothNullOrReferenceEquals(this.targetSB, other.targetSB);
						flag &= BothNullOrReferenceEquals(this.sg1, other.sg1);
						flag &= BothNullOrReferenceEquals(this.sg2, other.sg2);
						flag &= BothNullOrReferenceEquals(this.hovered, other.hovered);
						flag &= BothNullOrReferenceEquals(this.dIcon1, other.dIcon1);
						flag &= BothNullOrReferenceEquals(this.dIcon2, other.dIcon2);
						flag &= BothNullOrReferenceEquals(this.transaction, other.transaction);
						return flag;
					}
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
								DraggedIcon di1 = new DraggedIcon(stubSB_1);
								di1NotDone_F = new object[]{di1, null, null, null, false};
								yield return di1NotDone_F;
							object[] di2NotDone_F;
								ISlottable stubSB_2 = MakeSubSB();
								DraggedIcon di2 = new DraggedIcon(stubSB_2);
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
				public void SetTargetSB_FromNullToSome_CallsSBSelect(){
					TransactionManager tam = MakeTAM();
					ISlottable mockSB = MakeSubSB();

					tam.SetTargetSB(mockSB);


					mockSB.Received().Select();
					}
				[Test]
				public void SetTargetSB_FromNullToSome_SetsItTargetSB(){
					TransactionManager tam = MakeTAM();
					ISlottable stubSB = MakeSubSB();

					tam.SetTargetSB(stubSB);

					Assert.That(tam.targetSB, Is.SameAs(stubSB));
					}
				[Test]
				public void SetTargetSB_FromOtherToSome_CallsSBSelect(){
					TransactionManager tam = MakeTAM();
					ISlottable stubSB = MakeSubSB();
					ISlottable mockSB = MakeSubSB();
					tam.SetTargetSB(stubSB);

					tam.SetTargetSB(mockSB);
					
					mockSB.Received().Select();
					}
				[Test]
				public void SetTargetSB_FromOtherToSome_SetsItTargetSB(){
					TransactionManager tam = MakeTAM();
					ISlottable prevSB = MakeSubSB();
					ISlottable stubSB = MakeSubSB();
					tam.SetTargetSB(prevSB);

					tam.SetTargetSB(stubSB);
					
					Assert.That(tam.targetSB, Is.SameAs(stubSB));
					}
				[Test]
				public void SetTargetSB_FromOtherToSome_CallOtherSBFocus(){
					TransactionManager tam = MakeTAM();
					ISlottable mockSB = MakeSubSB();
					ISlottable stubSB = MakeSubSB();
					tam.SetTargetSB(mockSB);

					tam.SetTargetSB(stubSB);
					
					mockSB.Received().Focus();
					}
				[Test]
				public void SetTargetSB_SomeToNull_CallSBFocus(){
					TransactionManager tam = MakeTAM();
					ISlottable mockSB = MakeSubSB();
					tam.SetTargetSB(mockSB);

					tam.SetTargetSB(null);

					mockSB.Received().Focus();
					}
				[Test]
				public void SetTargetSB_SomeToNull_SetsNull(){
					TransactionManager tam = MakeTAM();
					ISlottable mockSB = MakeSubSB();
					tam.SetTargetSB(mockSB);

					tam.SetTargetSB(null);

					Assert.That(tam.targetSB, Is.Null);
					}
				[Test]
				public void SetTargetSB_SomeToSame_DoesNotCallSelectTwice(){
					TransactionManager tam = MakeTAM();
					ISlottable mockSB = MakeSubSB();
					tam.SetTargetSB(mockSB);

					tam.SetTargetSB(mockSB);

					mockSB.Received(1).Select();
					}
				[Test]
				public void SetTargetSB_SomeToSame_DoesNotCallFocus(){
					TransactionManager tam = MakeTAM();
					ISlottable mockSB = MakeSubSB();
					tam.SetTargetSB(mockSB);

					tam.SetTargetSB(mockSB);

					mockSB.DidNotReceive().Focus();
					}
				[Test]
				public void SetHovered_NullToSB_SetsHovered(){
					TransactionManager tam = MakeTAM();
					ISlottable mockSB = MakeSubSB();

					tam.SetHovered(mockSB);

					Assert.That(tam.hovered, Is.SameAs(mockSB));
					}
				[Test]
				public void SetHovered_SBToNull_DoesNotCallSBOnHoverExit(){
					TransactionManager tam = MakeTAM();
					ISlottable mockSB = MakeSubSB();
					tam.SetHovered(mockSB);

					tam.SetHovered(null);

					mockSB.DidNotReceive().OnHoverExit();
					}
				[Test]
				public void SetHovered_SBToNull_SetsNull(){
					TransactionManager tam = MakeTAM();
					ISlottable mockSB = MakeSubSB();
					tam.SetHovered(mockSB);

					tam.SetHovered(null);

					Assert.That(tam.hovered, Is.Null);
					}
				[Test]
				public void SetHovered_SBToOtherSSE_CallSBOnHoverExit(){
					TransactionManager tam = MakeTAM();
					ISlottable mockSB = MakeSubSB();
					tam.SetHovered(mockSB);

					tam.SetHovered(MakeSubSSE());

					mockSB.Received().OnHoverExit();
					}
				[Test]
				public void SetHovered_SBToSame_DoesNotCallSBOnHoverExit(){
					TransactionManager tam = MakeTAM();
					ISlottable mockSB = MakeSubSB();
					tam.SetHovered(mockSB);

					tam.SetHovered(mockSB);

					mockSB.DidNotReceive().OnHoverExit();
					}
				[Test]
				public void SetHovered_NullToSG_SetsHovered(){
					TransactionManager tam = MakeTAM();
					ISlotGroup mockSG = MakeSubSG();

					tam.SetHovered(mockSG);

					Assert.That(tam.hovered, Is.SameAs(mockSG));
					}
				[Test]
				public void SetHovered_SGToNull_DoesNotCallSGOnHoverExit(){
					TransactionManager tam = MakeTAM();
					ISlotGroup mockSG = MakeSubSG();
					tam.SetHovered(mockSG);

					tam.SetHovered(null);

					mockSG.DidNotReceive().OnHoverExit();
					}
				[Test]
				public void SetHovered_SGToNull_SetsNull(){
					TransactionManager tam = MakeTAM();
					ISlotGroup mockSG = MakeSubSG();
					tam.SetHovered(mockSG);

					tam.SetHovered(null);

					Assert.That(tam.hovered, Is.Null);
					}
				[Test]
				public void SetHovered_SGToOther_CallSGOnHoverExit(){
					TransactionManager tam = MakeTAM();
					ISlotGroup mockSG = MakeSubSG();
					tam.SetHovered(mockSG);

					tam.SetHovered(MakeSubSSE());

					mockSG.Received().OnHoverExit();
					}
				[Test]
				public void SetHovered_SGToSame_DoesNotCallSGOnHoverExit(){
					TransactionManager tam = MakeTAM();
					ISlotGroup mockSG = MakeSubSG();
					tam.SetHovered(mockSG);

					tam.SetHovered(mockSG);

					mockSG.DidNotReceive().OnHoverExit();
					}
				[Test]
				public void InnerUpdateFields_Always_SetsFields(){
					TransactionManager tam = MakeTAM();
					ISlotSystemTransaction stubTA = Substitute.For<ISlotSystemTransaction>();
						ISlotGroup stubSG1 = MakeSubSG();
						ISlotGroup stubSG2 = MakeSubSG();
						stubTA.sg1.Returns(stubSG1);
						stubTA.sg2.Returns(stubSG2);
					
					tam.InnerUpdateFields(stubTA);

					Assert.That(tam.sg1, Is.SameAs(stubSG1));
					Assert.That(tam.sg2, Is.SameAs(stubSG2));
					Assert.That(tam.transaction, Is.SameAs(stubTA));
				}
		}
	}
}

