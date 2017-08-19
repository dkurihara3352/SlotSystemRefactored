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
	namespace TransactionManagerTests{
		[TestFixture]
		public class TransactionCacheTests :SlotSystemTest{
			[TestCaseSource(typeof(PrePickFilterVariousSGTAComboCases))]
			public void IsTransactionGoingToBeRevert_VariousSGTACombo_OutsAccordingly( 
				ISlottable pickedSB,
				List<ISlotGroup> focusedSGs,
				ITransactionFactory taFac,
				bool expected)
			{
				IFocusedSGProvider focSGPrv = Substitute.For<IFocusedSGProvider>();
					focSGPrv.GetFocusedSGs().Returns(focusedSGs);
				ITransactionManager stubTAM = MakeSubTAM();
				TransactionCache taCache = new TransactionCache(stubTAM, focSGPrv);
					stubTAM.GetTAFactory().Returns(taFac);

				bool result = taCache.IsTransactionGoingToBeRevert(pickedSB);

				Assert.That(result, Is.EqualTo(expected));
			}
				class PrePickFilterVariousSGTAComboCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] nothingButRev_F;
							ISlottable pickedSB_0 = MakeSubSB();
							List<ISlotGroup> focusedSGs_0;
								ISlotGroup sg0_0 = MakeSubSG();
									IHoverable hoverable0_0 = Substitute.For<IHoverable>();
									sg0_0.GetHoverable().Returns(hoverable0_0);
									IEnumerable<ISlotSystemElement> sg0Eles_0;
										ISlottable sb00_0 = MakeSubSB();
										ISlottable sb01_0 = MakeSubSB();
										ISlottable sb02_0 = MakeSubSB();
										sg0Eles_0 = new ISlotSystemElement[]{sb00_0, sb01_0, sb02_0};
									sg0_0.GetEnumerator().Returns(sg0Eles_0.GetEnumerator());
								ISlotGroup sg1_0 = MakeSubSG();
									IHoverable hoverable1_0 = Substitute.For<IHoverable>();
									sg1_0.GetHoverable().Returns(hoverable1_0);
									IEnumerable<ISlotSystemElement> sg1Eles_0;
										ISlottable sb10_0 = MakeSubSB();
										ISlottable sb11_0 = MakeSubSB();
										ISlottable sb12_0 = MakeSubSB();
										sg1Eles_0 = new ISlotSystemElement[]{sb10_0, sb11_0, sb12_0};
									sg1_0.GetEnumerator().Returns(sg1Eles_0.GetEnumerator());
								focusedSGs_0 = new List<ISlotGroup>(new ISlotGroup[]{sg0_0, sg1_0});
							ITransactionFactory taFac_0 = Substitute.For<ITransactionFactory>();
								taFac_0.MakeTransaction(pickedSB_0, hoverable0_0).Returns(Substitute.For<IRevertTransaction>());
									taFac_0.MakeTransaction(pickedSB_0, sb00_0).Returns(Substitute.For<IRevertTransaction>());
									taFac_0.MakeTransaction(pickedSB_0, sb01_0).Returns(Substitute.For<IRevertTransaction>());
									taFac_0.MakeTransaction(pickedSB_0, sb02_0).Returns(Substitute.For<IRevertTransaction>());
								taFac_0.MakeTransaction(pickedSB_0, hoverable1_0).Returns(Substitute.For<IRevertTransaction>());
									taFac_0.MakeTransaction(pickedSB_0, sb10_0).Returns(Substitute.For<IRevertTransaction>());
									taFac_0.MakeTransaction(pickedSB_0, sb11_0).Returns(Substitute.For<IRevertTransaction>());
									taFac_0.MakeTransaction(pickedSB_0, sb12_0).Returns(Substitute.For<IRevertTransaction>());
							nothingButRev_F = new object[]{pickedSB_0, focusedSGs_0, taFac_0, true};
							yield return nothingButRev_F;

						object[] atLeastOneNonRev_T;
							ISlottable pickedSB_1 = MakeSubSB();
							List<ISlotGroup> focusedSGs_1;
								ISlotGroup sg0_1 = MakeSubSG();
									IHoverable hoverable0_1 = Substitute.For<IHoverable>();
									sg0_1.GetHoverable().Returns(hoverable0_1);
									IEnumerable<ISlotSystemElement> sg0Eles_1;
										ISlottable sb00_1 = MakeSubSB();
										ISlottable sb01_1 = MakeSubSB();
										ISlottable sb02_1 = MakeSubSB();
										sg0Eles_1 = new ISlotSystemElement[]{sb00_1, sb01_1, sb02_1};
									sg0_1.GetEnumerator().Returns(sg0Eles_1.GetEnumerator());
								ISlotGroup sg1_1 = MakeSubSG();
									IHoverable hoverable1_1 = Substitute.For<IHoverable>();
									sg1_1.GetHoverable().Returns(hoverable1_1);
									IEnumerable<ISlotSystemElement> sg1Eles_1;
										ISlottable sb10_1 = MakeSubSB();
										ISlottable sb11_1 = MakeSubSB();
										ISlottable sb12_1 = MakeSubSB();
										sg1Eles_1 = new ISlotSystemElement[]{sb10_1, sb11_1, sb12_1};
									sg1_1.GetEnumerator().Returns(sg1Eles_1.GetEnumerator());
								focusedSGs_1 = new List<ISlotGroup>(new ISlotGroup[]{sg0_1, sg1_1});
							ITransactionFactory taFac_1 = Substitute.For<ITransactionFactory>();
								taFac_1.MakeTransaction(pickedSB_1, hoverable0_1).Returns(Substitute.For<IRevertTransaction>());
									taFac_1.MakeTransaction(pickedSB_1, sb00_1).Returns(Substitute.For<IRevertTransaction>());
									taFac_1.MakeTransaction(pickedSB_1, sb01_1).Returns(Substitute.For<IRevertTransaction>());
									taFac_1.MakeTransaction(pickedSB_1, sb02_1).Returns(Substitute.For<IRevertTransaction>());
								taFac_1.MakeTransaction(pickedSB_1, hoverable1_1).Returns(Substitute.For<IRevertTransaction>());
									taFac_1.MakeTransaction(pickedSB_1, sb10_1).Returns(Substitute.For<IRevertTransaction>());
									taFac_1.MakeTransaction(pickedSB_1, sb11_1).Returns(Substitute.For<IRevertTransaction>());
									taFac_1.MakeTransaction(pickedSB_1, sb12_1).Returns(Substitute.For<IReorderTransaction>());
							atLeastOneNonRev_T = new object[]{pickedSB_1, focusedSGs_1, taFac_1, false};
							yield return atLeastOneNonRev_T;
					}
				}
			[Test]
			public void hovered_ByDefault_IsNull(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());

				Assert.That(taCache.GetHovered(), Is.Null);
			}
			[Test]
			public void SetHovered_hoveredNullToSome_SetsItHovered(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				IHoverable stubHoverable = Substitute.For<IHoverable>();

				taCache.SetHovered(stubHoverable);

				Assert.That(taCache.GetHovered(), Is.SameAs(stubHoverable));
			}
			[Test]
			public void SetHovered_hoveredNullToSome_CallsUpdateFieldsCommandExecute(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
					TransactionCacheCommand mockUpdateFieldsCommand = Substitute.For<TransactionCacheCommand>();
					taCache.SetUpdateFieldsCommand(mockUpdateFieldsCommand);
				IHoverable stubHoverable = Substitute.For<IHoverable>();

				taCache.SetHovered(stubHoverable);

				mockUpdateFieldsCommand.Received().Execute();
			}
			[Test]
			public void SetHovered_SomeToNull_SetsHoveredNull(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				IHoverable stubHoverable = Substitute.For<IHoverable>();
				taCache.SetHovered(stubHoverable);

				taCache.SetHovered(null);

				Assert.That(taCache.GetHovered(), Is.Null);
			}
			[Test]
			public void SetHovered_SomeToNull_DoesNotCallsPrevOnHoverExit(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				IHoverable mockHoverable = Substitute.For<IHoverable>();
				taCache.SetHovered(mockHoverable);

				taCache.SetHovered(null);

				mockHoverable.DidNotReceive().OnHoverExit();
			}
			[Test]
			public void SetHovered_SomeToOther_CallsPrevOnHoverExit(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				IHoverable mockHoverable = Substitute.For<IHoverable>();
				IHoverable newHoverable = Substitute.For<IHoverable>();
				taCache.SetHovered(mockHoverable);

				taCache.SetHovered(newHoverable);

				mockHoverable.Received().OnHoverExit();
			}
			[Test]
			public void SetHovered_SomeToOther_SetsTheOtherHovered(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				IHoverable stubHoverable = Substitute.For<IHoverable>();
				IHoverable newHoverable = Substitute.For<IHoverable>();
				taCache.SetHovered(stubHoverable);

				taCache.SetHovered(newHoverable);

				Assert.That(taCache.GetHovered(), Is.SameAs(newHoverable));
			}
			[Test]
			public void SetHovered_SomeToOther_CallsUpdateFieldsCommandExecute(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
					TransactionCacheCommand mockUpdateFieldsCommand = Substitute.For<TransactionCacheCommand>();
					taCache.SetUpdateFieldsCommand(mockUpdateFieldsCommand);
				IHoverable stubHoverable = Substitute.For<IHoverable>();
				IHoverable newHoverable = Substitute.For<IHoverable>();
				taCache.SetHovered(stubHoverable);

				taCache.SetHovered(newHoverable);

				mockUpdateFieldsCommand.Received().Execute();
			}
			[Test]
			public void SetHovered_SomeToSame_DoesNotCallFormerOnHoverExit(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				IHoverable mockHoverable = Substitute.For<IHoverable>();
				taCache.SetHovered(mockHoverable);
				
				taCache.SetHovered(mockHoverable);

				mockHoverable.DidNotReceive().OnHoverExit();
			}
			[Test]
			public void SetTargetSB_FromNullToSome_CallsSBSelect(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				ISlottable stubSB = MakeSubSB();
					ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
					stubSB.GetSelStateHandler().Returns(selStateHandler);

				taCache.SetTargetSB(stubSB);


				selStateHandler.Received().Select();
			}
			[Test]
			public void SetTargetSB_FromNullToSome_SetsItTargetSB(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				ISlottable stubSB = MakeSubSB();

				taCache.SetTargetSB(stubSB);

				Assert.That(taCache.GetTargetSB(), Is.SameAs(stubSB));
				}
			[Test]
			public void SetTargetSB_FromOtherToSome_CallsSBSelect(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				ISlottable prebSB = MakeSubSB();
				ISlottable stubSB = MakeSubSB();
					ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
					stubSB.GetSelStateHandler().Returns(selStateHandler);
				taCache.SetTargetSB(prebSB);

				taCache.SetTargetSB(stubSB);
				
				selStateHandler.Received().Select();
			}
			[Test]
			public void SetTargetSB_FromOtherToSome_SetsItTargetSB(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				ISlottable prevSB = MakeSubSB();
				ISlottable stubSB = MakeSubSB();
				taCache.SetTargetSB(prevSB);

				taCache.SetTargetSB(stubSB);
				
				Assert.That(taCache.GetTargetSB(), Is.SameAs(stubSB));
				}
			[Test]
			public void SetTargetSB_FromOtherToSome_CallOtherSBFocus(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				ISlottable prevSB = MakeSubSB();
					ISSESelStateHandler prevSBSelStateHandler = Substitute.For<ISSESelStateHandler>();
					prevSB.GetSelStateHandler().Returns(prevSBSelStateHandler);
				ISlottable stubSB = MakeSubSB();
				taCache.SetTargetSB(prevSB);

				taCache.SetTargetSB(stubSB);
				
				prevSBSelStateHandler.Received().Focus();
			}
			[Test]
			public void SetTargetSB_SomeToNull_CallSBFocus(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				ISlottable stubSB = MakeSubSB();
					ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
					stubSB.GetSelStateHandler().Returns(selStateHandler);
				taCache.SetTargetSB(stubSB);

				taCache.SetTargetSB(null);

				selStateHandler.Received().Focus();
			}
			[Test]
			public void SetTargetSB_SomeToNull_SetsNull(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				ISlottable mockSB = MakeSubSB();
				taCache.SetTargetSB(mockSB);

				taCache.SetTargetSB(null);

				Assert.That(taCache.GetTargetSB(), Is.Null);
			}
			[Test]
			public void SetTargetSB_SomeToSame_DoesNotCallSelectTwice(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				ISlottable stubSB = MakeSubSB();
					ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
					stubSB.GetSelStateHandler().Returns(selStateHandler);
				taCache.SetTargetSB(stubSB);

				taCache.SetTargetSB(stubSB);

				selStateHandler.Received(1).Select();
			}
			[Test]
			public void SetTargetSB_SomeToSame_DoesNotCallFocus(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				ISlottable stubSB = MakeSubSB();
					ISSESelStateHandler selStateHandler = Substitute.For<ISSESelStateHandler>();
					stubSB.GetSelStateHandler().Returns(selStateHandler);
				taCache.SetTargetSB(stubSB);

				taCache.SetTargetSB(stubSB);

				selStateHandler.DidNotReceive().Focus();
			}
			[Test]
			public void CreateTransactionResults_WhenCalled_CreateAndStoreSGTAPairInDict(){
				ITransactionManager tam = Substitute.For<ITransactionManager>();
				IFocusedSGProvider focSGPrv = Substitute.For<IFocusedSGProvider>();
				TransactionCache taCache = new TransactionCache(tam, focSGPrv);
					ISlottable pickedSB = MakeSubSB();
					taCache.SetPickedSB(pickedSB);
					ITransactionFactory stubMakeTAFactory = MakeSubTAFactory();
						ISlotGroup sgA = MakeSubSG();
							IEnumerable<ISlotSystemElement> sgAEles = new ISlotSystemElement[]{};
							sgA.GetEnumerator().Returns(sgAEles.GetEnumerator());
							IHoverable hoverableA = Substitute.For<IHoverable>();
							sgA.GetHoverable().Returns(hoverableA);
						ISlotGroup sgB = MakeSubSG();
							IEnumerable<ISlotSystemElement> sgBEles = new ISlotSystemElement[]{};
							sgB.GetEnumerator().Returns(sgBEles.GetEnumerator());
							IHoverable hoverableB = Substitute.For<IHoverable>();
							sgB.GetHoverable().Returns(hoverableB);
						ISlotGroup sgC = MakeSubSG();
							IEnumerable<ISlotSystemElement> sgCEles = new ISlotSystemElement[]{};
							sgC.GetEnumerator().Returns(sgCEles.GetEnumerator());
							IHoverable hoverableC = Substitute.For<IHoverable>();
							sgC.GetHoverable().Returns(hoverableC);
						IRevertTransaction revTA = Substitute.For<IRevertTransaction>();
						IFillTransaction fillTA = Substitute.For<IFillTransaction>();
						IReorderTransaction reoTA = Substitute.For<IReorderTransaction>();
						stubMakeTAFactory.MakeTransaction(pickedSB, hoverableA).Returns(revTA);
						stubMakeTAFactory.MakeTransaction(pickedSB, hoverableB).Returns(fillTA);
						stubMakeTAFactory.MakeTransaction(pickedSB, hoverableC).Returns(reoTA);
						List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sgA, sgB, sgC});
						focSGPrv.GetFocusedSGs().Returns(sgsList);
				tam.GetTAFactory().Returns(stubMakeTAFactory);

				taCache.CreateTransactionResults();

				Dictionary<IHoverable, ISlotSystemTransaction> expected = new Dictionary<IHoverable, ISlotSystemTransaction>();
				expected.Add(hoverableA, revTA);
				expected.Add(hoverableB, fillTA);
				expected.Add(hoverableC, reoTA);

				Assert.That(taCache.GetTransactionResults().Count, Is.EqualTo(expected.Count));
				IEnumerator actRator = taCache.GetTransactionResults().GetEnumerator();
				IEnumerator expRator = expected.GetEnumerator();
				while(actRator.MoveNext() && expRator.MoveNext()){
					KeyValuePair<IHoverable, ISlotSystemTransaction> actPair = (KeyValuePair<IHoverable, ISlotSystemTransaction>)actRator.Current;
					KeyValuePair<IHoverable, ISlotSystemTransaction> expPair = (KeyValuePair<IHoverable, ISlotSystemTransaction>)expRator.Current;
					Assert.That(actPair.Key, Is.SameAs(expPair.Key));
					Assert.That(actPair.Value, Is.TypeOf(expPair.Value.GetType()));
				}
			}
			[Test]
			public void CreateTransactionResults_WhenCalled_CreateAndStoreSBTAPairInDict(){
				ITransactionManager tam = Substitute.For<ITransactionManager>();
				IFocusedSGProvider focSGPrv = Substitute.For<IFocusedSGProvider>();
				TransactionCache taCache = new TransactionCache(tam, focSGPrv);
					ISlottable pickedSB = MakeSubSB();
					taCache.SetPickedSB(pickedSB);
						ISlotGroup sg = MakeSubSG();
							IHoverable hoverable = Substitute.For<IHoverable>();
							sg.GetHoverable().Returns(hoverable);
							IEnumerable<ISlotSystemElement> sgEles;
								ISlottable sbA = MakeSubSB();
								ISlottable sbB = MakeSubSB();
								ISlottable sbC = MakeSubSB();
								sgEles = new ISlotSystemElement[]{sbA, sbB, sbC};
							sg.GetEnumerator().Returns(sgEles.GetEnumerator());
						IRevertTransaction revTA = Substitute.For<IRevertTransaction>();
						IFillTransaction fillTA = Substitute.For<IFillTransaction>();
						IStackTransaction stackTA = Substitute.For<IStackTransaction>();
					ITransactionFactory stubMakeTAFactory = MakeSubTAFactory();
						stubMakeTAFactory.MakeTransaction(pickedSB, hoverable).Returns(revTA);
							stubMakeTAFactory.MakeTransaction(pickedSB, sbA).Returns(revTA);
							stubMakeTAFactory.MakeTransaction(pickedSB, sbB).Returns(fillTA);
							stubMakeTAFactory.MakeTransaction(pickedSB, sbC).Returns(stackTA);
						List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sg});
						focSGPrv.GetFocusedSGs().Returns(sgsList);
				tam.GetTAFactory().Returns(stubMakeTAFactory);
				Dictionary<IHoverable, ISlotSystemTransaction> expected = new Dictionary<IHoverable, ISlotSystemTransaction>();
				expected.Add(hoverable, revTA);
				expected.Add(sbA, revTA);
				expected.Add(sbB, fillTA);
				expected.Add(sbC, stackTA);

				taCache.CreateTransactionResults();

				Assert.That(taCache.GetTransactionResults().Count, Is.EqualTo(expected.Count));
				IEnumerator actRator = taCache.GetTransactionResults().GetEnumerator();
				IEnumerator expRator = expected.GetEnumerator();
				while(actRator.MoveNext() && expRator.MoveNext()){
					KeyValuePair<IHoverable, ISlotSystemTransaction> actPair = (KeyValuePair<IHoverable, ISlotSystemTransaction>)actRator.Current;
					KeyValuePair<IHoverable, ISlotSystemTransaction> expPair = (KeyValuePair<IHoverable, ISlotSystemTransaction>)expRator.Current;
					Assert.That(actPair.Key, Is.SameAs(expPair.Key));
					Assert.That(actPair.Value, Is.TypeOf(expPair.Value.GetType()));
				}
			}
			[TestCaseSource(typeof(CreateTransactionResultsVariousTACases))]
			public void CreateTransactionResults_VariousTA_CallsSGDefocusSelfOrFocusSelfAccordingly(ISlotSystemTransaction ta){
				ITransactionManager tam = Substitute.For<ITransactionManager>();
				IFocusedSGProvider focSGPrv = Substitute.For<IFocusedSGProvider>();
				TransactionCache taCache = new TransactionCache(tam, focSGPrv);
					ISlottable pickedSB = MakeSubSB();
					taCache.SetPickedSB(pickedSB);
					ITransactionFactory stubFac = MakeSubTAFactory();
						ISlotGroup sgA = MakeSubSG();
							IHoverable hoverableA = Substitute.For<IHoverable>();
							sgA.GetHoverable().Returns(hoverableA);
							ISSESelStateHandler sgASelStateHandler = Substitute.For<ISSESelStateHandler>();
							sgA.GetSelStateHandler().Returns(sgASelStateHandler);
							IEnumerable<ISlotSystemElement> sgAEles = new ISlotSystemElement[]{};
							sgA.GetEnumerator().Returns(sgAEles.GetEnumerator());
						stubFac.MakeTransaction(pickedSB, hoverableA).Returns(ta);
						List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sgA});
						focSGPrv.GetFocusedSGs().Returns(sgsList);
				tam.GetTAFactory().Returns(stubFac);

				taCache.CreateTransactionResults();

				if(ta is IRevertTransaction)
					sgASelStateHandler.Received().Defocus();
				else
					sgASelStateHandler.Received().Focus();
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
				ITransactionManager tam = Substitute.For<ITransactionManager>();
				IFocusedSGProvider focSGPrv = Substitute.For<IFocusedSGProvider>();
				TransactionCache taCache = new TransactionCache(tam, focSGPrv);
					ISlottable pickedSB = MakeSubSB();
					taCache.SetPickedSB(pickedSB);
					ITransactionFactory stubFac = MakeSubTAFactory();
						ISlotGroup sg = MakeSubSG();
							IHoverable hoverable = Substitute.For<IHoverable>();
							sg.GetHoverable().Returns(hoverable);
							IEnumerable<ISlotSystemElement> sgEles;	
								ISlottable sb = MakeSubSB();
									ISSESelStateHandler sbSelStateHandler = Substitute.For<ISSESelStateHandler>();
									sb.GetSelStateHandler().Returns(sbSelStateHandler);
								sgEles = new ISlotSystemElement[]{sb};
							sg.GetEnumerator().Returns(sgEles.GetEnumerator());
						stubFac.MakeTransaction(pickedSB, sb).Returns(ta);
						stubFac.MakeTransaction(pickedSB, hoverable).Returns(Substitute.For<IRevertTransaction>());
						List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sg});
						focSGPrv.GetFocusedSGs().Returns(sgsList);
				tam.GetTAFactory().Returns(stubFac);

				taCache.CreateTransactionResults();

				if(ta is IRevertTransaction)
					sbSelStateHandler.Received().Defocus();
				else if(ta is IFillTransaction)
					sbSelStateHandler.Received().Defocus();
				else
					sbSelStateHandler.Received().Focus();
			}
			[Test]
			public void UpdateField_MatchFoundInTAResultsForHovered_SetsFields(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
					Dictionary<IHoverable, ISlotSystemTransaction> taResults = new Dictionary<IHoverable, ISlotSystemTransaction>();
						IHoverable stubHovered = Substitute.For<IHoverable>();
						ISlotSystemTransaction stubTA = Substitute.For<ISlotSystemTransaction>();
							ISlottable stubTargetSB = MakeSubSB();
							stubTA.GetTargetSB().Returns(stubTargetSB);
							List<IInventoryItemInstance> stubMoved = new List<IInventoryItemInstance>();
							stubTA.GetMoved().Returns(stubMoved);
						taResults.Add(stubHovered, stubTA);
					taCache.SetHovered(stubHovered);
					taCache.SetTransactionResults(taResults);

				taCache.UpdateFields();

				Assert.That(taCache.GetTargetSB(), Is.SameAs(stubTargetSB));
				Assert.That(taCache.GetMoved(), Is.SameAs(stubMoved));
			}
			[Test]
			public void UpdateField_MatchFoundInTAResultsForHovered_CallsTAMInnerUpdateFields(){
				ITransactionManager mockTAM = MakeSubTAM();
				TransactionCache taCache = new TransactionCache(mockTAM, MakeSubFocSGPrv());
					Dictionary<IHoverable, ISlotSystemTransaction> taResults = new Dictionary<IHoverable, ISlotSystemTransaction>();
						IHoverable stubHovered = Substitute.For<IHoverable>();
						ISlotSystemTransaction stubTA = Substitute.For<ISlotSystemTransaction>();
							ISlottable stubTargetSB = MakeSubSB();
							stubTA.GetTargetSB().Returns(stubTargetSB);
							List<IInventoryItemInstance> stubMoved = new List<IInventoryItemInstance>();
							stubTA.GetMoved().Returns(stubMoved);
						taResults.Add(stubHovered, stubTA);
					taCache.SetHovered(stubHovered);
					taCache.SetTransactionResults(taResults);

				taCache.UpdateFields();

				mockTAM.Received().UpdateFields(stubTA);
			}
			[Test][ExpectedException(typeof(System.InvalidOperationException))]
			public void IsTransactionResultRevertFor_TAResultsNoMatch_ThrowsException(){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				IHoverable mockHoverable = Substitute.For<IHoverable>();

				taCache.IsCachedTAResultRevert(mockHoverable);
			}
			[TestCaseSource(typeof(IsTransactionResultRevertFor_VariousTAsCases))]
			public void IsTransactionResultRevertFor_VariousTAs_ReturnsAccordingly(ISlotSystemTransaction ta, bool expected){
				TransactionCache taCache = new TransactionCache(MakeSubTAM(), MakeSubFocSGPrv());
				IHoverable mockHoverable = Substitute.For<IHoverable>();
				Dictionary<IHoverable, ISlotSystemTransaction> dict = new Dictionary<IHoverable, ISlotSystemTransaction>();
					dict.Add(mockHoverable, ta);
					taCache.SetTransactionResults(dict);
				bool actual = taCache.IsCachedTAResultRevert(mockHoverable);

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
		}
	}
}
