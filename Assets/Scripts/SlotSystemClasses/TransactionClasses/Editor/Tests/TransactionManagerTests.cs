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
					tam.SetSG1(sg1);
					tam.SetSG2(sg2);
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
				[TestCaseSource(typeof(PrePickFilterVariousSGTAComboCases))]
				public void PrePickFilter_VariousSGTACombo_OutsAccordingly( 
					ISlottable pickedSB,
					List<ISlotGroup> focusedSGs,
					ITransactionFactory taFac,
					bool expected)
				{
					TransactionManager tam = MakeTAM();
						ISlotSystemManager stubSSM = MakeSubSSM();
						stubSSM.focusedSGs.Returns(focusedSGs);
						tam.SetPickedSB(pickedSB);
						tam.SetTAFactory(taFac);
						tam.SetSSM(stubSSM);

					bool result;

					tam.PrePickFilter(pickedSB, out result);

					Assert.That(result, Is.EqualTo(expected));
					}
					class PrePickFilterVariousSGTAComboCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] nothingButRev_F;
								ISlottable pickedSB_0 = MakeSubSB();
								List<ISlotGroup> focusedSGs_0;
									ISlotGroup sg0_0 = MakeSubSG();
										IEnumerable<ISlotSystemElement> sg0Eles_0;
											ISlottable sb00_0 = MakeSubSB();
											ISlottable sb01_0 = MakeSubSB();
											ISlottable sb02_0 = MakeSubSB();
											sg0Eles_0 = new ISlotSystemElement[]{sb00_0, sb01_0, sb02_0};
										sg0_0.GetEnumerator().Returns(sg0Eles_0.GetEnumerator());
									ISlotGroup sg1_0 = MakeSubSG();
										IEnumerable<ISlotSystemElement> sg1Eles_0;
											ISlottable sb10_0 = MakeSubSB();
											ISlottable sb11_0 = MakeSubSB();
											ISlottable sb12_0 = MakeSubSB();
											sg1Eles_0 = new ISlotSystemElement[]{sb10_0, sb11_0, sb12_0};
										sg1_0.GetEnumerator().Returns(sg1Eles_0.GetEnumerator());
									focusedSGs_0 = new List<ISlotGroup>(new ISlotGroup[]{sg0_0, sg1_0});
								ITransactionFactory taFac_0 = Substitute.For<ITransactionFactory>();
									taFac_0.MakeTransaction(pickedSB_0, sg0_0).Returns(Substitute.For<IRevertTransaction>());
										taFac_0.MakeTransaction(pickedSB_0, sb00_0).Returns(Substitute.For<IRevertTransaction>());
										taFac_0.MakeTransaction(pickedSB_0, sb01_0).Returns(Substitute.For<IRevertTransaction>());
										taFac_0.MakeTransaction(pickedSB_0, sb02_0).Returns(Substitute.For<IRevertTransaction>());
									taFac_0.MakeTransaction(pickedSB_0, sg1_0).Returns(Substitute.For<IRevertTransaction>());
										taFac_0.MakeTransaction(pickedSB_0, sb10_0).Returns(Substitute.For<IRevertTransaction>());
										taFac_0.MakeTransaction(pickedSB_0, sb11_0).Returns(Substitute.For<IRevertTransaction>());
										taFac_0.MakeTransaction(pickedSB_0, sb12_0).Returns(Substitute.For<IRevertTransaction>());
								nothingButRev_F = new object[]{pickedSB_0, focusedSGs_0, taFac_0, false};
								yield return nothingButRev_F;
							object[] atLeastOneNonRev_T;
								ISlottable pickedSB_1 = MakeSubSB();
								List<ISlotGroup> focusedSGs_1;
									ISlotGroup sg0_1 = MakeSubSG();
										IEnumerable<ISlotSystemElement> sg0Eles_1;
											ISlottable sb00_1 = MakeSubSB();
											ISlottable sb01_1 = MakeSubSB();
											ISlottable sb02_1 = MakeSubSB();
											sg0Eles_1 = new ISlotSystemElement[]{sb00_1, sb01_1, sb02_1};
										sg0_1.GetEnumerator().Returns(sg0Eles_1.GetEnumerator());
									ISlotGroup sg1_1 = MakeSubSG();
										IEnumerable<ISlotSystemElement> sg1Eles_1;
											ISlottable sb10_1 = MakeSubSB();
											ISlottable sb11_1 = MakeSubSB();
											ISlottable sb12_1 = MakeSubSB();
											sg1Eles_1 = new ISlotSystemElement[]{sb10_1, sb11_1, sb12_1};
										sg1_1.GetEnumerator().Returns(sg1Eles_1.GetEnumerator());
									focusedSGs_1 = new List<ISlotGroup>(new ISlotGroup[]{sg0_1, sg1_1});
								ITransactionFactory taFac_1 = Substitute.For<ITransactionFactory>();
									taFac_1.MakeTransaction(pickedSB_1, sg0_1).Returns(Substitute.For<IRevertTransaction>());
										taFac_1.MakeTransaction(pickedSB_1, sb00_1).Returns(Substitute.For<IRevertTransaction>());
										taFac_1.MakeTransaction(pickedSB_1, sb01_1).Returns(Substitute.For<IRevertTransaction>());
										taFac_1.MakeTransaction(pickedSB_1, sb02_1).Returns(Substitute.For<IRevertTransaction>());
									taFac_1.MakeTransaction(pickedSB_1, sg1_1).Returns(Substitute.For<IRevertTransaction>());
										taFac_1.MakeTransaction(pickedSB_1, sb10_1).Returns(Substitute.For<IRevertTransaction>());
										taFac_1.MakeTransaction(pickedSB_1, sb11_1).Returns(Substitute.For<IRevertTransaction>());
										taFac_1.MakeTransaction(pickedSB_1, sb12_1).Returns(Substitute.For<IReorderTransaction>());
								atLeastOneNonRev_T = new object[]{pickedSB_1, focusedSGs_1, taFac_1, true};
								yield return atLeastOneNonRev_T;
							
						}
					}
			[Test]
			public void SortSG_WhenCalled_CallsSortFAMakeSortTA(){
				TransactionManager tam = MakeTAM();
				ISortTransactionFactory sortFA = Substitute.For<ISortTransactionFactory>();
					ISortTransaction stubSortTA = Substitute.For<ISortTransaction>();
					sortFA.MakeSortTA(Arg.Any<ISlotGroup>(), Arg.Any<SGSorter>()).Returns(stubSortTA);
					tam.SetSortFA(sortFA);
				ISlotGroup sg = MakeSubSG();
				SGSorter sorter = new SGItemIDSorter();

				tam.SortSG(sg, sorter);
				
				sortFA.Received().MakeSortTA(sg, sorter);
				}
			[Test]
			public void SortSG_WhenCalled_CallsTAExecute(){
				TransactionManager tam = MakeTAM();
				ISortTransactionFactory sortFA = Substitute.For<ISortTransactionFactory>();
					ISortTransaction mockTA = Substitute.For<ISortTransaction>();
					sortFA.MakeSortTA(Arg.Any<ISlotGroup>(), Arg.Any<SGSorter>()).Returns(mockTA);
					tam.SetSortFA(sortFA);
				ISlotGroup sg = MakeSubSG();
				SGSorter sorter = new SGItemIDSorter();

				tam.SortSG(sg, sorter);
				
				mockTA.Received().Execute();
				}
			[Test]
			public void SortSG_WhenCalled_UpdateFields(){
				TransactionManager tam = MakeTAM();
				ISortTransactionFactory sortFA = Substitute.For<ISortTransactionFactory>();
					ISortTransaction stubTA = Substitute.For<ISortTransaction>();
						ISlottable tarSB = MakeSubSB();
						ISlotGroup sg1 = MakeSubSG();
						stubTA.targetSB.Returns(tarSB);
						stubTA.sg1.Returns(sg1);
					sortFA.MakeSortTA(Arg.Any<ISlotGroup>(), Arg.Any<SGSorter>()).Returns(stubTA);
					tam.SetSortFA(sortFA);
				ISlotGroup sg = MakeSubSG();
				SGSorter sorter = new SGItemIDSorter();
				SortSGTestCase expected = new SortSGTestCase(tarSB, sg1, stubTA);

				tam.SortSG(sg, sorter);
				
				SortSGTestCase actual = new SortSGTestCase(tam.targetSB, tam.sg1, tam.transaction);
				bool equality = actual.Equals(expected);

				Assert.That(equality,Is.True);
				}
				class SortSGTestCase: IEquatable<SortSGTestCase>{
					public ISlottable targetSB;
					public ISlotGroup sg1;
					public ISlotSystemTransaction ta;
					public SortSGTestCase(ISlottable sb, ISlotGroup sg, ISlotSystemTransaction ta){
						targetSB = sb; sg1 = sg; this.ta = ta;
					}
					public bool Equals(SortSGTestCase other){
						bool flag = true;
						flag &= object.ReferenceEquals(this.targetSB, other.targetSB);
						flag &= object.ReferenceEquals(this.sg1, other.sg1);
						flag &= object.ReferenceEquals(this.ta, other.ta);
						return flag;
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
				public void AcceptsSGTAComp_ValidSG_SetsDone(){
					TransactionManager tam = MakeTAM();
					ISlotGroup stubSG = MakeSubSG();
					tam.SetSG1(stubSG);

					tam.AcceptSGTAComp(stubSG);

					Assert.That(tam.sg2Done, Is.True);
					}
				[Test]
				public void AcceptsDITAComp_ValidDI_SetsDone(){
					TransactionManager tam = MakeTAM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());
					tam.SetDIcon1(stubDI);

					tam.AcceptDITAComp(stubDI);

					Assert.That(tam.dIcon1Done, Is.True);
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
				public void SetSG1_NullToSome_SetsSG1(){
					TransactionManager tam = MakeTAM();
					ISlotGroup stubSG = MakeSubSG();

					tam.SetSG1(stubSG);

					Assert.That(tam.sg1, Is.SameAs(stubSG));
					}
				[Test]
				public void SetSG1_NullToSome_SetsSG1DoneFalse(){
					TransactionManager tam = MakeTAM();
					ISlotGroup stubSG = MakeSubSG();

					tam.SetSG1(stubSG);

					Assert.That(tam.sg1Done, Is.False);
					}
				[Test]
				public void SetSG1_OtherToSome_SetsSG1(){
					TransactionManager tam = MakeTAM();
					ISlotGroup prevSG = MakeSubSG();
						SetUpStubTransactionResults(tam, prevSG);
					ISlotGroup stubSG = MakeSubSG();

					tam.SetSG1(prevSG);
					tam.SetSG1(stubSG);

					Assert.That(tam.sg1, Is.SameAs(stubSG));
					}
				void SetUpStubTransactionResults(TransactionManager tam, ISlotSystemElement sse){
					Dictionary<ISlotSystemElement, ISlotSystemTransaction> taResult = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
					taResult.Add(sse, new RevertTransaction(MakeSubSB()));
					tam.SetTransactionResults(taResult);
				}
				[Test]
				public void SetSG1_OtherToSome_SetsSG1DoneFalse(){
					TransactionManager tam = MakeTAM();
					ISlotGroup prevSG = MakeSubSG();
						SetUpStubTransactionResults(tam, prevSG);
					ISlotGroup stubSG = MakeSubSG();

					tam.SetSG1(prevSG);
					tam.SetSG1(stubSG);

					Assert.That(tam.sg1Done, Is.False);
					}
				[Test]
				public void SetSG1_SomeToNull_SetsSG1Null(){
					TransactionManager tam = MakeTAM();
					ISlotGroup stubSG = MakeSubSG();
						SetUpStubTransactionResults(tam, stubSG);
					tam.SetSG1(stubSG);

					tam.SetSG1(null);

					Assert.That(tam.sg1, Is.Null);
					}
				[Test]
				public void SetSG1_SomeToNull_SetsSG1DoneTrue(){
					TransactionManager tam = MakeTAM();
					ISlotGroup stubSG = MakeSubSG();
						SetUpStubTransactionResults(tam, stubSG);
					tam.SetSG1(stubSG);

					tam.SetSG1(null);

					Assert.That(tam.sg1Done, Is.True);
					}
				[Test]
				public void SetSG2_NullToSome_SetsSG2(){
					TransactionManager tam = MakeTAM();
					ISlotGroup stubSG = MakeSubSG();

					tam.SetSG2(stubSG);

					Assert.That(tam.sg2, Is.SameAs(stubSG));
					}
				[Test]
				public void SetSG2_NullToSome_SetsSG2DoneFalse(){
					TransactionManager tam = MakeTAM();
					ISlotGroup stubSG = MakeSubSG();

					tam.SetSG2(stubSG);

					Assert.That(tam.sg2Done, Is.False);
					}
				[Test]
				public void SetSG2_NullToSome_CallSG2Select(){
					TransactionManager tam = MakeTAM();
					ISlotGroup mockSG = MakeSubSG();

					tam.SetSG2(mockSG);

					mockSG.Received().Select();
					}
				[Test]
				public void SetSG2_OtherToSome_SetsSG2(){
					TransactionManager tam = MakeTAM();
					ISlotGroup prevSG = MakeSubSG();
						SetUpStubTransactionResults(tam, prevSG);
					ISlotGroup stubSG = MakeSubSG();

					tam.SetSG2(prevSG);
					tam.SetSG2(stubSG);

					Assert.That(tam.sg2, Is.SameAs(stubSG));
					}
				[Test]
				public void SetSG2_OtherToSome_SetsSG2DoneFalse(){
					TransactionManager tam = MakeTAM();
					ISlotGroup prevSG = MakeSubSG();
						SetUpStubTransactionResults(tam, prevSG);
					ISlotGroup stubSG = MakeSubSG();

					tam.SetSG2(prevSG);
					tam.SetSG2(stubSG);

					Assert.That(tam.sg2Done, Is.False);
					}
				[Test]
				public void SetSG2_OtherToSome_CallsSG2Select(){
					TransactionManager tam = MakeTAM();
					ISlotGroup prevSG = MakeSubSG();
						SetUpStubTransactionResults(tam, prevSG);
					ISlotGroup mockSG = MakeSubSG();

					tam.SetSG2(prevSG);
					tam.SetSG2(mockSG);

					mockSG.Received().Select();
					}
				[Test]
				public void SetSG2_SomeToNull_SetsSG2Null(){
					TransactionManager tam = MakeTAM();
					ISlotGroup stubSG = MakeSubSG();
						SetUpStubTransactionResults(tam, stubSG);
					tam.SetSG2(stubSG);

					tam.SetSG2(null);

					Assert.That(tam.sg2, Is.Null);
					}
				[Test]
				public void SetSG2_SomeToNull_SetsSG2DoneTrue(){
					TransactionManager tam = MakeTAM();
					ISlotGroup stubSG = MakeSubSG();
						SetUpStubTransactionResults(tam, stubSG);
					tam.SetSG2(stubSG);

					tam.SetSG2(null);

					Assert.That(tam.sg2Done, Is.True);
					}
				[Test]
				public void SetDIcon1_ToNonNull_SetsDIcon1DoneFalse(){
					TransactionManager tam = MakeTAM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());

					tam.SetDIcon1(stubDI);

					Assert.That(tam.dIcon1Done, Is.False);
					}
				[Test]
				public void SetDIcon1_ToNull_SetsDIcon1DoneTrue(){
					TransactionManager tam = MakeTAM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());

					tam.SetDIcon1(stubDI);
					tam.SetDIcon1(null);

					Assert.That(tam.dIcon1Done, Is.True);
					}
				[Test]
				public void SetDIcon2_ToNonNull_SetsDIcon2DoneFalse(){
					TransactionManager tam = MakeTAM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());

					tam.SetDIcon2(stubDI);

					Assert.That(tam.dIcon2Done, Is.False);
					}
				[Test]
				public void SetDIcon2_ToNull_SetsDIcon2DoneTrue(){
					TransactionManager tam = MakeTAM();
					DraggedIcon stubDI = new DraggedIcon(MakeSubSB());

					tam.SetDIcon2(stubDI);
					tam.SetDIcon2(null);

					Assert.That(tam.dIcon2Done, Is.True);
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
				public void SetHovered_WhenCalled_UpdateTransactionFields(){
					TransactionManager tam = MakeTAM();
						ISlotSystemTransaction stubTA = MakeSubTA();
							ISlottable targetSB = MakeSubSB();
							ISlotGroup sg1 = MakeSubSG();
							ISlotGroup sg2 = MakeSubSG();
							stubTA.targetSB.Returns(targetSB);
							stubTA.sg1.Returns(sg1);
							stubTA.sg2.Returns(sg2);
						ISlottable hoveredSB = MakeSubSB();
						Dictionary<ISlotSystemElement, ISlotSystemTransaction> taResults = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
						taResults.Add(hoveredSB, stubTA);
						tam.SetTransactionResults(taResults);
					SetHoveredTestData expected = new SetHoveredTestData(targetSB, sg1, sg2, stubTA);

					tam.SetHovered(hoveredSB);

					SetHoveredTestData actual = new SetHoveredTestData(tam.targetSB, tam.sg1, tam.sg2, tam.transaction);
					bool equality = actual.Equals(expected);
					Assert.That(equality, Is.True);
					}
					class SetHoveredTestData: IEquatable<SetHoveredTestData>{
						public ISlottable targetSB;
						public ISlotGroup sg1;
						public ISlotGroup sg2;
						public ISlotSystemTransaction ta;
						public SetHoveredTestData(ISlottable tSB, ISlotGroup sg1, ISlotGroup sg2, ISlotSystemTransaction ta){
							this.targetSB = tSB;
							this.sg1 = sg1;
							this.sg2 = sg2;
							this.ta = ta;
						}
						public bool Equals(SetHoveredTestData other){
							bool flag = true;
							flag &= object.ReferenceEquals(this.targetSB, other.targetSB);
							flag &= object.ReferenceEquals(this.sg1, other.sg1);
							flag &= object.ReferenceEquals(this.sg2, other.sg2);
							flag &= object.ReferenceEquals(this.ta, other.ta);
							return flag;
						}
					}
				[Test]
				public void CreateTransactionResults_WhenCalled_CreateAndStoreSGTAPairInDict(){
					TransactionManager tam = MakeTAM();
						ISlottable pickedSB = MakeSubSB();
						tam.SetPickedSB(pickedSB);
						ITransactionFactory stubFac = MakeSubTAFactory();
							ISlotGroup sgA = MakeSubSG();
								IEnumerable<ISlotSystemElement> sgAEles = new ISlotSystemElement[]{};
								sgA.GetEnumerator().Returns(sgAEles.GetEnumerator());
							ISlotGroup sgB = MakeSubSG();
								IEnumerable<ISlotSystemElement> sgBEles = new ISlotSystemElement[]{};
								sgB.GetEnumerator().Returns(sgBEles.GetEnumerator());
							ISlotGroup sgC = MakeSubSG();
								IEnumerable<ISlotSystemElement> sgCEles = new ISlotSystemElement[]{};
								sgC.GetEnumerator().Returns(sgCEles.GetEnumerator());
							IRevertTransaction revTA = Substitute.For<IRevertTransaction>();
							IFillTransaction fillTA = Substitute.For<IFillTransaction>();
							IReorderTransaction reoTA = Substitute.For<IReorderTransaction>();
							stubFac.MakeTransaction(pickedSB, sgA).Returns(revTA);
							stubFac.MakeTransaction(pickedSB, sgB).Returns(fillTA);
							stubFac.MakeTransaction(pickedSB, sgC).Returns(reoTA);
							List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sgA, sgB, sgC});
							ISlotSystemManager stubSSM = MakeSubSSM();
							stubSSM.focusedSGs.Returns(sgsList);
					tam.SetTAFactory(stubFac);
					tam.SetSSM(stubSSM);

					tam.CreateTransactionResults();

					Dictionary<ISlotSystemElement, ISlotSystemTransaction> expected = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
					expected.Add(sgA, revTA);
					expected.Add(sgB, fillTA);
					expected.Add(sgC, reoTA);

					Assert.That(tam.transactionResults.Count, Is.EqualTo(expected.Count));
					IEnumerator actRator = tam.transactionResults.GetEnumerator();
					IEnumerator expRator = expected.GetEnumerator();
					while(actRator.MoveNext() && expRator.MoveNext()){
						KeyValuePair<ISlotSystemElement, ISlotSystemTransaction> actPair = (KeyValuePair<ISlotSystemElement, ISlotSystemTransaction>)actRator.Current;
						KeyValuePair<ISlotSystemElement, ISlotSystemTransaction> expPair = (KeyValuePair<ISlotSystemElement, ISlotSystemTransaction>)expRator.Current;
						Assert.That(actPair.Key, Is.SameAs(expPair.Key));
						Assert.That(actPair.Value, Is.TypeOf(expPair.Value.GetType()));
					}
					}
				[Test]
				public void CreateTransactionResults_WhenCalled_CreateAndStoreSBTAPairInDict(){
					TransactionManager tam = MakeTAM();
						ISlottable pickedSB = MakeSubSB();
						tam.SetPickedSB(pickedSB);
						ITransactionFactory stubFac = MakeSubTAFactory();
							ISlotGroup sg = MakeSubSG();
								IEnumerable<ISlotSystemElement> sgEles;
									ISlottable sbA = MakeSubSB();
									ISlottable sbB = MakeSubSB();
									ISlottable sbC = MakeSubSB();
									sgEles = new ISlotSystemElement[]{sbA, sbB, sbC};
								sg.GetEnumerator().Returns(sgEles.GetEnumerator());
							IRevertTransaction revTA = Substitute.For<IRevertTransaction>();
							IFillTransaction fillTA = Substitute.For<IFillTransaction>();
							IStackTransaction stackTA = Substitute.For<IStackTransaction>();
							stubFac.MakeTransaction(pickedSB, sg).Returns(revTA);
								stubFac.MakeTransaction(pickedSB, sbA).Returns(revTA);
								stubFac.MakeTransaction(pickedSB, sbB).Returns(fillTA);
								stubFac.MakeTransaction(pickedSB, sbC).Returns(stackTA);
							List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sg});
							ISlotSystemManager stubSSM = MakeSubSSM();
							stubSSM.focusedSGs.Returns(sgsList);
					tam.SetTAFactory(stubFac);
					tam.SetSSM(stubSSM);

					tam.CreateTransactionResults();

					Dictionary<ISlotSystemElement, ISlotSystemTransaction> expected = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
					expected.Add(sg, revTA);
					expected.Add(sbA, revTA);
					expected.Add(sbB, fillTA);
					expected.Add(sbC, stackTA);

					Assert.That(tam.transactionResults.Count, Is.EqualTo(expected.Count));
					IEnumerator actRator = tam.transactionResults.GetEnumerator();
					IEnumerator expRator = expected.GetEnumerator();
					while(actRator.MoveNext() && expRator.MoveNext()){
						KeyValuePair<ISlotSystemElement, ISlotSystemTransaction> actPair = (KeyValuePair<ISlotSystemElement, ISlotSystemTransaction>)actRator.Current;
						KeyValuePair<ISlotSystemElement, ISlotSystemTransaction> expPair = (KeyValuePair<ISlotSystemElement, ISlotSystemTransaction>)expRator.Current;
						Assert.That(actPair.Key, Is.SameAs(expPair.Key));
						Assert.That(actPair.Value, Is.TypeOf(expPair.Value.GetType()));
					}
					}
				
				[TestCaseSource(typeof(CreateTransactionResultsVariousTACases))]
				public void CreateTransactionResults_VariousTA_CallsSGDefocusSelfOrFocusSelfAccordingly(ISlotSystemTransaction ta){
					TransactionManager tam = MakeTAM();
						ISlottable pickedSB = MakeSubSB();
						tam.SetPickedSB(pickedSB);
						ITransactionFactory stubFac = MakeSubTAFactory();
							ISlotGroup sgA = MakeSubSG();
								IEnumerable<ISlotSystemElement> sgAEles = new ISlotSystemElement[]{};
								sgA.GetEnumerator().Returns(sgAEles.GetEnumerator());
							stubFac.MakeTransaction(pickedSB, sgA).Returns(ta);
							List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sgA});
							ISlotSystemManager stubSSM = MakeSubSSM();
							stubSSM.focusedSGs.Returns(sgsList);
					tam.SetTAFactory(stubFac);
					tam.SetSSM(stubSSM);

					tam.CreateTransactionResults();
					if(ta is IRevertTransaction)
						sgA.Received().DefocusSelf();
					else
						sgA.Received().FocusSelf();
					}
					class CreateTransactionResultsVariousTACases: IEnumerable{
						public IEnumerator GetEnumerator(){
							yield return Substitute.For<IRevertTransaction>();
							yield return Substitute.For<IReorderTransaction>();
							yield return Substitute.For<ISortTransaction>();
							yield return Substitute.For<ISwapTransaction>();
							yield return Substitute.For<IFillTransaction>();
							yield return Substitute.For<IStackTransaction>();
						}
					}
				
				[TestCaseSource(typeof(CreateTransactionResultsVariousTACases))]
				public void CreateTransactionResults_VariousTA_CallsSBFocusOrDefocusAccordingly(ISlotSystemTransaction ta){
					TransactionManager tam = MakeTAM();
						ISlottable pickedSB = MakeSubSB();
						tam.SetPickedSB(pickedSB);
						ITransactionFactory stubFac = MakeSubTAFactory();
							ISlotGroup sg = MakeSubSG();
								IEnumerable<ISlotSystemElement> sgEles;	
									ISlottable sb = MakeSubSB();
									sgEles = new ISlotSystemElement[]{sb};
								sg.GetEnumerator().Returns(sgEles.GetEnumerator());
							stubFac.MakeTransaction(pickedSB, sb).Returns(ta);
							stubFac.MakeTransaction(pickedSB, sg).Returns(Substitute.For<IRevertTransaction>());
							List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sg});
							ISlotSystemManager stubSSM = MakeSubSSM();
							stubSSM.focusedSGs.Returns(sgsList);
					tam.SetTAFactory(stubFac);
					tam.SetSSM(stubSSM);

					tam.CreateTransactionResults();
					if(ta is IRevertTransaction)
						sb.Received().Defocus();
					else if(ta is IFillTransaction)
						sb.Received().Defocus();
					else
						sb.Received().Focus();
					}
				[Test]
				public void UpdateTransaction_WhenCalled_UpdatesFields(){
					TransactionManager tam = MakeTAM();
						ISlotGroup hovered = MakeSubSG();
							tam.SetHovered(hovered);
							ISlotSystemTransaction stubTA = MakeSubTA();
								ISlottable targetSB = MakeSubSB();
								ISlotGroup sg1 = MakeSubSG();
								ISlotGroup sg2 = MakeSubSG();
								stubTA.targetSB.Returns(targetSB);
								stubTA.sg1.Returns(sg1);
								stubTA.sg2.Returns(sg2);
						Dictionary<ISlotSystemElement, ISlotSystemTransaction> dict = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
							dict.Add(hovered, stubTA);
							tam.SetTransactionResults(dict);
					UpdateTransactionTestCase expected = new UpdateTransactionTestCase(targetSB, sg1, sg2, stubTA);
					
					tam.UpdateTransaction();

					UpdateTransactionTestCase actual = new UpdateTransactionTestCase(tam.targetSB, tam.sg1, tam.sg2, tam.transaction);
					bool equality = actual.Equals(expected);

					Assert.That(equality, Is.True);
					}
					class UpdateTransactionTestCase: IEquatable<UpdateTransactionTestCase>{
						public ISlottable targetSB;
						public ISlotGroup sg1;
						public ISlotGroup sg2;
						public ISlotSystemTransaction ta;
						public UpdateTransactionTestCase(ISlottable tSB, ISlotGroup g1, ISlotGroup g2, ISlotSystemTransaction tra){
							targetSB = tSB; sg1 = g1; sg2 = g2; ta = tra;
						}
						public bool Equals(UpdateTransactionTestCase other){
							bool flag = true;
							flag &= object.ReferenceEquals(this.targetSB, other.targetSB);
							flag &= object.ReferenceEquals(this.sg1, other.sg1);
							flag &= object.ReferenceEquals(this.sg2, other.sg2);
							flag &= object.ReferenceEquals(this.ta, other.ta);
							return flag;
						}
					}
				[Test][ExpectedException(typeof(System.InvalidOperationException))]
				public void IsTransactionResultRevertFor_TAResultsNoMatch_ThrowsException(){
					TransactionManager tam = MakeTAM();
					ISlotGroup mockSG = MakeSubSG();

					tam.IsTransactionResultRevertFor(mockSG);
					}
				[TestCaseSource(typeof(IsTransactionResultRevertFor_VariousTAsCases))]
				public void IsTransactionResultRevertFor_VariousTAs_ReturnsAccordingly(ISlotSystemTransaction ta, bool expected){
					ISlotGroup mockSG = MakeSubSG();
					TransactionManager tam = MakeTAM();
					Dictionary<ISlotSystemElement, ISlotSystemTransaction> dict = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
						dict.Add(mockSG, ta);
						tam.SetTransactionResults(dict);
					bool actual = tam.IsTransactionResultRevertFor(mockSG);

					Assert.That(actual, Is.EqualTo(expected));
					
					}
					class IsTransactionResultRevertFor_VariousTAsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] revert_def;
								revert_def = new object[]{Substitute.For<IRevertTransaction>(), true};
								yield return revert_def;
							object[] reorder_foc;
								reorder_foc = new object[]{Substitute.For<IReorderTransaction>(), false};
								yield return reorder_foc;
							object[] sort_foc;
								sort_foc = new object[]{Substitute.For<ISortTransaction>(), false};
								yield return sort_foc;
							object[] fill_foc;
								fill_foc = new object[]{Substitute.For<IFillTransaction>(), false};
								yield return fill_foc;
							object[] swap_foc;
								swap_foc = new object[]{Substitute.For<ISwapTransaction>(), false};
								yield return swap_foc;
							object[] stack_foc;
								stack_foc = new object[]{Substitute.For<IStackTransaction>(), false};
								yield return stack_foc;
						}
					}				
			/* helper */
		}
	}
}

