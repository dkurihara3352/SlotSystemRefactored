using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Utility;
namespace SlotSystem{
	[TestFixture]
	[Category("TAM")]
	public class TransactionCacheTests :SlotSystemTest{
		[TestCaseSource(typeof(PrePickFilterVariousSGTAComboCases))]
		public void IsTransactionGoingToBeRevert_VariousSGTACombo_OutsAccordingly( 
			ISlottable pickedSB,
			List<ISlotGroup> focusedSGs,
			ITransactionFactory taFac,
			bool expected)
		{
			ITransactionManager tam = MakeSubTAM();
			TransactionCache taCache = new TransactionCache(tam);
				tam.focusedSGs.Returns(focusedSGs);
				taCache.SetPickedSB(pickedSB);
				taCache.SetTAFactory(taFac);

			bool result = taCache.IsTransactionGoingToBeRevert(pickedSB);

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
						IHoverable hoverable0_0 = MakeSubHoverableForSubSG(sg0_0);
						IHoverable hoverable00_0 = MakeSubHoverableForSubSB(sb00_0);
						IHoverable hoverable01_0 = MakeSubHoverableForSubSB(sb01_0);
						IHoverable hoverable02_0 = MakeSubHoverableForSubSB(sb02_0);
						IHoverable hoverable1_0 = MakeSubHoverableForSubSG(sg1_0);
						IHoverable hoverable10_0 = MakeSubHoverableForSubSB(sb10_0);
						IHoverable hoverable11_0 = MakeSubHoverableForSubSB(sb11_0);
						IHoverable hoverable12_0 = MakeSubHoverableForSubSB(sb12_0);
						ITransactionFactory taFac_0 = Substitute.For<ITransactionFactory>();
							taFac_0.MakeTransaction(pickedSB_0, hoverable0_0).Returns(Substitute.For<IRevertTransaction>());
								taFac_0.MakeTransaction(pickedSB_0, hoverable00_0).Returns(Substitute.For<IRevertTransaction>());
								taFac_0.MakeTransaction(pickedSB_0, hoverable01_0).Returns(Substitute.For<IRevertTransaction>());
								taFac_0.MakeTransaction(pickedSB_0, hoverable02_0).Returns(Substitute.For<IRevertTransaction>());
							taFac_0.MakeTransaction(pickedSB_0, hoverable1_0).Returns(Substitute.For<IRevertTransaction>());
								taFac_0.MakeTransaction(pickedSB_0, hoverable10_0).Returns(Substitute.For<IRevertTransaction>());
								taFac_0.MakeTransaction(pickedSB_0, hoverable11_0).Returns(Substitute.For<IRevertTransaction>());
								taFac_0.MakeTransaction(pickedSB_0, hoverable12_0).Returns(Substitute.For<IRevertTransaction>());
						nothingButRev_F = new object[]{pickedSB_0, focusedSGs_0, taFac_0, true};
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
							IHoverable hoverable0_1 = MakeSubHoverableForSubSG(sg0_1);
							IHoverable hoverable00_1 = MakeSubHoverableForSubSB(sb00_1);
							IHoverable hoverable01_1 = MakeSubHoverableForSubSB(sb01_1);
							IHoverable hoverable02_1 = MakeSubHoverableForSubSB(sb02_1);
							IHoverable hoverable1_1 = MakeSubHoverableForSubSG(sg1_1);
							IHoverable hoverable10_1 = MakeSubHoverableForSubSB(sb10_1);
							IHoverable hoverable11_1 = MakeSubHoverableForSubSB(sb11_1);
							IHoverable hoverable12_1 = MakeSubHoverableForSubSB(sb12_1);
						ITransactionFactory taFac_1 = Substitute.For<ITransactionFactory>();
							taFac_1.MakeTransaction(pickedSB_1, hoverable0_1).Returns(Substitute.For<IRevertTransaction>());
								taFac_1.MakeTransaction(pickedSB_1, hoverable00_1).Returns(Substitute.For<IRevertTransaction>());
								taFac_1.MakeTransaction(pickedSB_1, hoverable01_1).Returns(Substitute.For<IRevertTransaction>());
								taFac_1.MakeTransaction(pickedSB_1, hoverable02_1).Returns(Substitute.For<IRevertTransaction>());
							taFac_1.MakeTransaction(pickedSB_1, hoverable1_1).Returns(Substitute.For<IRevertTransaction>());
								taFac_1.MakeTransaction(pickedSB_1, hoverable10_1).Returns(Substitute.For<IRevertTransaction>());
								taFac_1.MakeTransaction(pickedSB_1, hoverable11_1).Returns(Substitute.For<IRevertTransaction>());
								taFac_1.MakeTransaction(pickedSB_1, hoverable12_1).Returns(Substitute.For<IReorderTransaction>());
						atLeastOneNonRev_T = new object[]{pickedSB_1, focusedSGs_1, taFac_1, false};
						yield return atLeastOneNonRev_T;
					
				}
			}
		static IHoverable MakeSubHoverableForSubSB(ISlottable sb){
			IHoverable hoverable = Substitute.For<IHoverable>();
			sb.hoverable.Returns(hoverable);
			return hoverable;
		}
		static IHoverable MakeSubHoverableForSubSG(ISlotGroup sg){
			IHoverable hoverable = Substitute.For<IHoverable>();
			sg.hoverable.Returns(hoverable);
			return hoverable;
		}
		[Test]
		public void hovered_ByDefault_IsNull(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());

			Assert.That(taCache.hovered, Is.Null);
		}
		[Test]
		public void SetHovered_hoveredNullToSome_SetsItHovered(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			IHoverable stubHoverable = Substitute.For<IHoverable>();

			taCache.SetHovered(stubHoverable);

			Assert.That(taCache.hovered, Is.SameAs(stubHoverable));
		}
		[Test]
		public void SetHovered_hoveredNullToSome_CallsUpdateFieldsCommandExecute(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
				TransactionCacheCommand mockUpdateFieldsCommand = Substitute.For<TransactionCacheCommand>();
				taCache.SetUpdateFieldsCommand(mockUpdateFieldsCommand);
			IHoverable stubHoverable = Substitute.For<IHoverable>();

			taCache.SetHovered(stubHoverable);

			mockUpdateFieldsCommand.Received().Execute();
		}
		[Test]
		public void SetHovered_SomeToNull_SetsHoveredNull(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			IHoverable stubHoverable = Substitute.For<IHoverable>();
			taCache.SetHovered(stubHoverable);

			taCache.SetHovered(null);

			Assert.That(taCache.hovered, Is.Null);
		}
		[Test]
		public void SetHovered_SomeToNull_DoesNotCallsPrevOnHoverExit(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			IHoverable mockHoverable = Substitute.For<IHoverable>();
			taCache.SetHovered(mockHoverable);

			taCache.SetHovered(null);

			mockHoverable.DidNotReceive().OnHoverExit();
		}
		[Test]
		public void SetHovered_SomeToOther_CallsPrevOnHoverExit(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			IHoverable mockHoverable = Substitute.For<IHoverable>();
			IHoverable newHoverable = Substitute.For<IHoverable>();
			taCache.SetHovered(mockHoverable);

			taCache.SetHovered(newHoverable);

			mockHoverable.Received().OnHoverExit();
		}
		[Test]
		public void SetHovered_SomeToOther_SetsTheOtherHovered(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			IHoverable stubHoverable = Substitute.For<IHoverable>();
			IHoverable newHoverable = Substitute.For<IHoverable>();
			taCache.SetHovered(stubHoverable);

			taCache.SetHovered(newHoverable);

			Assert.That(taCache.hovered, Is.SameAs(newHoverable));
		}
		[Test]
		public void SetHovered_SomeToOther_CallsUpdateFieldsCommandExecute(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
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
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			IHoverable mockHoverable = Substitute.For<IHoverable>();
			taCache.SetHovered(mockHoverable);
			
			taCache.SetHovered(mockHoverable);

			mockHoverable.DidNotReceive().OnHoverExit();
		}
		[Test]
		public void SetTargetSB_FromNullToSome_CallsSBSelect(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			ISlottable mockSB = MakeSubSB();

			taCache.SetTargetSB(mockSB);


			mockSB.Received().Select();
			}
		[Test]
		public void SetTargetSB_FromNullToSome_SetsItTargetSB(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			ISlottable stubSB = MakeSubSB();

			taCache.SetTargetSB(stubSB);

			Assert.That(taCache.targetSB, Is.SameAs(stubSB));
			}
		[Test]
		public void SetTargetSB_FromOtherToSome_CallsSBSelect(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			ISlottable stubSB = MakeSubSB();
			ISlottable mockSB = MakeSubSB();
			taCache.SetTargetSB(stubSB);

			taCache.SetTargetSB(mockSB);
			
			mockSB.Received().Select();
			}
		[Test]
		public void SetTargetSB_FromOtherToSome_SetsItTargetSB(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			ISlottable prevSB = MakeSubSB();
			ISlottable stubSB = MakeSubSB();
			taCache.SetTargetSB(prevSB);

			taCache.SetTargetSB(stubSB);
			
			Assert.That(taCache.targetSB, Is.SameAs(stubSB));
			}
		[Test]
		public void SetTargetSB_FromOtherToSome_CallOtherSBFocus(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			ISlottable mockSB = MakeSubSB();
			ISlottable stubSB = MakeSubSB();
			taCache.SetTargetSB(mockSB);

			taCache.SetTargetSB(stubSB);
			
			mockSB.Received().Focus();
			}
		[Test]
		public void SetTargetSB_SomeToNull_CallSBFocus(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			ISlottable mockSB = MakeSubSB();
			taCache.SetTargetSB(mockSB);

			taCache.SetTargetSB(null);

			mockSB.Received().Focus();
			}
		[Test]
		public void SetTargetSB_SomeToNull_SetsNull(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			ISlottable mockSB = MakeSubSB();
			taCache.SetTargetSB(mockSB);

			taCache.SetTargetSB(null);

			Assert.That(taCache.targetSB, Is.Null);
			}
		[Test]
		public void SetTargetSB_SomeToSame_DoesNotCallSelectTwice(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			ISlottable mockSB = MakeSubSB();
			taCache.SetTargetSB(mockSB);

			taCache.SetTargetSB(mockSB);

			mockSB.Received(1).Select();
			}
		[Test]
		public void SetTargetSB_SomeToSame_DoesNotCallFocus(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			ISlottable mockSB = MakeSubSB();
			taCache.SetTargetSB(mockSB);

			taCache.SetTargetSB(mockSB);

			mockSB.DidNotReceive().Focus();
			}

		[Test]
		public void CreateTransactionResults_WhenCalled_CreateAndStoreSGTAPairInDict(){
			ITransactionManager tam = Substitute.For<ITransactionManager>();
			TransactionCache taCache = new TransactionCache(tam);
				ISlottable pickedSB = MakeSubSB();
				taCache.SetPickedSB(pickedSB);
				ITransactionFactory stubMakeTAFactory = MakeSubTAFactory();
					ISlotGroup sgA = MakeSubSG();
						IEnumerable<ISlotSystemElement> sgAEles = new ISlotSystemElement[]{};
						sgA.GetEnumerator().Returns(sgAEles.GetEnumerator());
						IHoverable hoverableA = Substitute.For<IHoverable>();
						sgA.hoverable.Returns(hoverableA);
					ISlotGroup sgB = MakeSubSG();
						IEnumerable<ISlotSystemElement> sgBEles = new ISlotSystemElement[]{};
						sgB.GetEnumerator().Returns(sgBEles.GetEnumerator());
						IHoverable hoverableB = Substitute.For<IHoverable>();
						sgB.hoverable.Returns(hoverableB);
					ISlotGroup sgC = MakeSubSG();
						IEnumerable<ISlotSystemElement> sgCEles = new ISlotSystemElement[]{};
						sgC.GetEnumerator().Returns(sgCEles.GetEnumerator());
						IHoverable hoverableC = Substitute.For<IHoverable>();
						sgC.hoverable.Returns(hoverableC);
					IRevertTransaction revTA = Substitute.For<IRevertTransaction>();
					IFillTransaction fillTA = Substitute.For<IFillTransaction>();
					IReorderTransaction reoTA = Substitute.For<IReorderTransaction>();
					stubMakeTAFactory.MakeTransaction(pickedSB, hoverableA).Returns(revTA);
					stubMakeTAFactory.MakeTransaction(pickedSB, hoverableB).Returns(fillTA);
					stubMakeTAFactory.MakeTransaction(pickedSB, hoverableC).Returns(reoTA);
					List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sgA, sgB, sgC});
					tam.focusedSGs.Returns(sgsList);
			taCache.SetTAFactory(stubMakeTAFactory);

			taCache.CreateTransactionResults();

			Dictionary<IHoverable, ISlotSystemTransaction> expected = new Dictionary<IHoverable, ISlotSystemTransaction>();
			expected.Add(hoverableA, revTA);
			expected.Add(hoverableB, fillTA);
			expected.Add(hoverableC, reoTA);

			Assert.That(taCache.transactionResults.Count, Is.EqualTo(expected.Count));
			IEnumerator actRator = taCache.transactionResults.GetEnumerator();
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
			TransactionCache taCache = new TransactionCache(tam);
				ISlottable pickedSB = MakeSubSB();
				taCache.SetPickedSB(pickedSB);
					ISlotGroup sg = MakeSubSG();
						IHoverable hoverableSG = Substitute.For<IHoverable>();
						sg.hoverable.Returns(hoverableSG);
						IEnumerable<ISlotSystemElement> sgEles;
							ISlottable sbA = MakeSubSB();
								IHoverable hoverableSBA = Substitute.For<IHoverable>();
								sbA.hoverable.Returns(hoverableSBA);
							ISlottable sbB = MakeSubSB();
								IHoverable hoverableSBB = Substitute.For<IHoverable>();
								sbB.hoverable.Returns(hoverableSBB);
							ISlottable sbC = MakeSubSB();
								IHoverable hoverableSBC = Substitute.For<IHoverable>();
								sbC.hoverable.Returns(hoverableSBC);
							sgEles = new ISlotSystemElement[]{sbA, sbB, sbC};
						sg.GetEnumerator().Returns(sgEles.GetEnumerator());
					IRevertTransaction revTA = Substitute.For<IRevertTransaction>();
					IFillTransaction fillTA = Substitute.For<IFillTransaction>();
					IStackTransaction stackTA = Substitute.For<IStackTransaction>();
				ITransactionFactory stubMakeTAFactory = MakeSubTAFactory();
					stubMakeTAFactory.MakeTransaction(pickedSB, hoverableSG).Returns(revTA);
						stubMakeTAFactory.MakeTransaction(pickedSB, hoverableSBA).Returns(revTA);
						stubMakeTAFactory.MakeTransaction(pickedSB, hoverableSBB).Returns(fillTA);
						stubMakeTAFactory.MakeTransaction(pickedSB, hoverableSBC).Returns(stackTA);
					List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sg});
					tam.focusedSGs.Returns(sgsList);
			taCache.SetTAFactory(stubMakeTAFactory);
			Dictionary<IHoverable, ISlotSystemTransaction> expected = new Dictionary<IHoverable, ISlotSystemTransaction>();
			expected.Add(hoverableSG, revTA);
			expected.Add(hoverableSBA, revTA);
			expected.Add(hoverableSBB, fillTA);
			expected.Add(hoverableSBC, stackTA);

			taCache.CreateTransactionResults();

			Assert.That(taCache.transactionResults.Count, Is.EqualTo(expected.Count));
			IEnumerator actRator = taCache.transactionResults.GetEnumerator();
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
			TransactionCache taCache = new TransactionCache(tam);
				ISlottable pickedSB = MakeSubSB();
				taCache.SetPickedSB(pickedSB);
				ITransactionFactory stubFac = MakeSubTAFactory();
					ISlotGroup sgA = MakeSubSG();
						IEnumerable<ISlotSystemElement> sgAEles = new ISlotSystemElement[]{};
						sgA.GetEnumerator().Returns(sgAEles.GetEnumerator());
						IHoverable hoverable = Substitute.For<IHoverable>();
						sgA.hoverable.Returns(hoverable);
					stubFac.MakeTransaction(pickedSB, hoverable).Returns(ta);
					List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sgA});
					tam.focusedSGs.Returns(sgsList);
			taCache.SetTAFactory(stubFac);

			taCache.CreateTransactionResults();

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
			ITransactionManager tam = Substitute.For<ITransactionManager>();
			TransactionCache taCache = new TransactionCache(tam);
				ISlottable pickedSB = MakeSubSB();
				taCache.SetPickedSB(pickedSB);
				ITransactionFactory stubFac = MakeSubTAFactory();
					ISlotGroup sg = MakeSubSG();
						IHoverable hoverableSG = Substitute.For<IHoverable>();
						sg.hoverable.Returns(hoverableSG);
						IEnumerable<ISlotSystemElement> sgEles;	
							ISlottable sb = MakeSubSB();
							sgEles = new ISlotSystemElement[]{sb};
							IHoverable hoverableSB = Substitute.For<IHoverable>();
							sb.hoverable.Returns(hoverableSB);
						sg.GetEnumerator().Returns(sgEles.GetEnumerator());
					stubFac.MakeTransaction(pickedSB, hoverableSB).Returns(ta);
					stubFac.MakeTransaction(pickedSB, hoverableSG).Returns(Substitute.For<IRevertTransaction>());
					List<ISlotGroup> sgsList = new List<ISlotGroup>(new ISlotGroup[]{sg});
					tam.focusedSGs.Returns(sgsList);
			taCache.SetTAFactory(stubFac);

			taCache.CreateTransactionResults();

			if(ta is IRevertTransaction)
				sb.Received().Defocus();
			else if(ta is IFillTransaction)
				sb.Received().Defocus();
			else
				sb.Received().Focus();
			}
		[Test]
		public void UpdateField_MatchFoundInTAResultsForHovered_SetsFields(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
				Dictionary<IHoverable, ISlotSystemTransaction> taResults = new Dictionary<IHoverable, ISlotSystemTransaction>();
					IHoverable stubHovered = Substitute.For<IHoverable>();
					ISlotSystemTransaction stubTA = Substitute.For<ISlotSystemTransaction>();
						ISlottable stubTargetSB = MakeSubSB();
						stubTA.targetSB.Returns(stubTargetSB);
						List<InventoryItemInstance> stubMoved = new List<InventoryItemInstance>();
						stubTA.moved.Returns(stubMoved);
					taResults.Add(stubHovered, stubTA);
				taCache.SetHovered(stubHovered);
				taCache.SetTransactionResults(taResults);

			taCache.UpdateFields();

			Assert.That(taCache.targetSB, Is.SameAs(stubTargetSB));
			Assert.That(taCache.moved, Is.SameAs(stubMoved));
		}
		[Test]
		public void UpdateField_MatchFoundInTAResultsForHovered_CallsTAMInnerUpdateFields(){
			ITransactionManager mockTAM = MakeSubTAM();
			TransactionCache taCache = new TransactionCache(mockTAM);
				Dictionary<IHoverable, ISlotSystemTransaction> taResults = new Dictionary<IHoverable, ISlotSystemTransaction>();
					IHoverable stubHovered = Substitute.For<IHoverable>();
					ISlotSystemTransaction stubTA = Substitute.For<ISlotSystemTransaction>();
						ISlottable stubTargetSB = MakeSubSB();
						stubTA.targetSB.Returns(stubTargetSB);
						List<InventoryItemInstance> stubMoved = new List<InventoryItemInstance>();
						stubTA.moved.Returns(stubMoved);
					taResults.Add(stubHovered, stubTA);
				taCache.SetHovered(stubHovered);
				taCache.SetTransactionResults(taResults);

			taCache.UpdateFields();

			mockTAM.Received().InnerUpdateFields(stubTA);
		}
		[Test][ExpectedException(typeof(System.InvalidOperationException))]
		public void IsTransactionResultRevertFor_TAResultsNoMatch_ThrowsException(){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
			IHoverable mockHoverable = Substitute.For<IHoverable>();

			taCache.IsCachedTAResultRevert(mockHoverable);
		}
		[TestCaseSource(typeof(IsTransactionResultRevertFor_VariousTAsCases))]
		public void IsTransactionResultRevertFor_VariousTAs_ReturnsAccordingly(ISlotSystemTransaction ta, bool expected){
			TransactionCache taCache = new TransactionCache(MakeSubTAM());
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
