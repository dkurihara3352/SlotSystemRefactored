using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using Utility;
namespace SlotSystemTests{
	namespace SlotGroupTests{
		[TestFixture][Category("SG")]
		public class SlotGroupTests: SlotSystemTest{
			[Test]
			public void TransactionCoroutine_AllSBsNotRunning_CallsActProcessExpire(){
				SlotGroup sg = MakeSG();
					ISBHandler sbHandler = Substitute.For<ISBHandler>();
						List<ISlottable> sbs;
							ISlottable sbA = MakeSBWithActProc(false);
							ISlottable sbB = MakeSBWithActProc(false);
							ISlottable sbC = MakeSBWithActProc(false);
							sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC});
						sbHandler.slottables.Returns(sbs);
					sg.SetSBHandler(sbHandler);
					ISGActStateHandler actStateHandler = Substitute.For<ISGActStateHandler>();
							ISGActProcess mockActProc = MakeSubSGActProc();
						actStateHandler.actProcess.Returns(mockActProc);
					sg.SetSGActStateHandler(actStateHandler);
				sg.SetAndRunActProcess(mockActProc);

				sg.TransactionCoroutine();
				
				mockActProc.Received().Expire();
			}
			/*	intrinsic	*/
				[Test]
				public void isPool_SSMPBunContainsInHierarchyThis_ReturnsTrue(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager stubSSM = MakeSSMWithPBunContaining(sg);
						sg.SetSSM(stubSSM);

					bool actual = sg.isPool;

					Assert.That(actual, Is.True);
				}
				[Test]
				public void isSGE_SSMEBunContainsInHierarchyThis_ReturnsTrue(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager stubSSM = MakeSSMWithEBunContaining(sg);
						sg.SetSSM(stubSSM);

					bool actual = sg.isSGE;

					Assert.That(actual, Is.True);
				}
				[Test]
				public void isSGG_SSMGBunContainsInHierarchyThis_ReturnsTrue(){
					SlotGroup sg = MakeSG();
						ISlotSystemManager stubSSM = MakeSSMWithGBunContaining(sg);
						sg.SetSSM(stubSSM);

					bool actual = sg.isSGG;

					Assert.That(actual, Is.True);
				}
				[TestCaseSource(typeof(SwappableSBsCases))]
				public void SwappableSBs_SBAreSwappable_ReturnsList(SlotGroup sg, ISlottable sb, List<ISlottable> expected){
					List<ISlottable> actual = sg.SwappableSBs(sb);

					bool equality = actual.MemberEquals(expected);
					Assert.That(equality, Is.True);
				}
					class SwappableSBsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] sameSG_empty;
								SlotGroup sg_0 = MakeSG();
									sg_0.SetFilterHandler(new FilterHandler());
									sg_0.SetFilter(new SGNullFilter());
									ISlottable bow0SB_0 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_0);
									ISlottable bow1SB_0 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_0);
									ISlottable wearSB_0 = MakeSubSBWithItemAndSG(MakeWearInstance(0), sg_0);
									ISlottable shieldSB_0 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), sg_0);
									ISlottable mWeaponSB_0 = MakeSubSBWithItemAndSG(MakeMWeaponInstance(0), sg_0);
									ISlottable quiverSB_0 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_0);
									ISlottable packSB_0 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_0);
									ISlottable partsSB_0 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), sg_0);
									sg_0.SetSBHandler(new SBHandler());
									sg_0.SetSBs(new List<ISlottable>(new ISlottable[]{
										bow0SB_0, bow1SB_0, wearSB_0, shieldSB_0, mWeaponSB_0, quiverSB_0, packSB_0, partsSB_0
									}));
								sameSG_empty = new object[]{sg_0, shieldSB_0, new List<ISlottable>()};
								yield return sameSG_empty;
							object[] withBows;
								SlotGroup sg_1 = MakeSG();
									sg_1.SetFilterHandler(new FilterHandler());
									sg_1.SetFilter(new SGNullFilter());
									ISlottable bow0SB_1 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_1);
									ISlottable bow1SB_1 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_1);
									ISlottable wearSB_1 = MakeSubSBWithItemAndSG(MakeWearInstance(0), sg_1);
									ISlottable shieldSB_1 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), sg_1);
									ISlottable mWeaponSB_1 = MakeSubSBWithItemAndSG(MakeMWeaponInstance(0), sg_1);
									ISlottable quiverSB_1 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_1);
									ISlottable packSB_1 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_1);
									ISlottable partsSB_1 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), sg_1);
									sg_1.SetSBHandler(new SBHandler());
									sg_1.SetSBs(new List<ISlottable>(new ISlottable[]{
										bow0SB_1, bow1SB_1, wearSB_1, shieldSB_1, mWeaponSB_1, quiverSB_1, packSB_1, partsSB_1
									}));
								SlotGroup bowSG_1 = MakeSG();
									bowSG_1.SetFilterHandler(new FilterHandler());
									bowSG_1.SetFilter(new SGBowFilter());
									ISlottable oBow0SB_1 = MakeSubSBWithItemAndSG(MakeBowInstance(0), bowSG_1);
								withBows = new object[]{sg_1, oBow0SB_1, new List<ISlottable>(new ISlottable[]{bow0SB_1, bow1SB_1})};
								yield return withBows;
							object[] withWears;
								SlotGroup sg_2 = MakeSG();
									sg_2.SetFilterHandler(new FilterHandler());
									sg_2.SetFilter(new SGNullFilter());
									ISlottable bow0SB_2 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_2);
									ISlottable bow1SB_2 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_2);
									ISlottable wearSB_2 = MakeSubSBWithItemAndSG(MakeWearInstance(0), sg_2);
									ISlottable shieldSB_2 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), sg_2);
									ISlottable mWeaponSB_2 = MakeSubSBWithItemAndSG(MakeMWeaponInstance(0), sg_2);
									ISlottable quiverSB_2 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_2);
									ISlottable packSB_2 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_2);
									ISlottable partsSB_2 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), sg_2);
									sg_2.SetSBHandler(new SBHandler());
									sg_2.SetSBs(new List<ISlottable>(new ISlottable[]{
										bow0SB_2, bow1SB_2, wearSB_2, shieldSB_2, mWeaponSB_2, quiverSB_2, packSB_2, partsSB_2
									}));
								SlotGroup wearSG_2 = MakeSG();
									wearSG_2.SetFilterHandler(new FilterHandler());
									wearSG_2.SetFilter(new SGWearFilter());
									ISlottable oWear0SB_2 = MakeSubSBWithItemAndSG(MakeWearInstance(0), wearSG_2);
								withWears = new object[]{sg_2, oWear0SB_2, new List<ISlottable>(new ISlottable[]{wearSB_2})};
								yield return withWears;
							object[] withCGears;
								SlotGroup sg_3 = MakeSG();
									sg_3.SetFilterHandler(new FilterHandler());
									sg_3.SetFilter(new SGNullFilter());
									ISlottable bow0SB_3 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_3);
									ISlottable bow1SB_3 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_3);
									ISlottable wearSB_3 = MakeSubSBWithItemAndSG(MakeWearInstance(0), sg_3);
									ISlottable shieldSB_3 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), sg_3);
									ISlottable mWeaponSB_3 = MakeSubSBWithItemAndSG(MakeMWeaponInstance(0), sg_3);
									ISlottable quiverSB_3 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_3);
									ISlottable packSB_3 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_3);
									ISlottable partsSB_3 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), sg_3);
									sg_3.SetSBHandler(new SBHandler());
									sg_3.SetSBs(new List<ISlottable>(new ISlottable[]{
										bow0SB_3, bow1SB_3, wearSB_3, shieldSB_3, mWeaponSB_3, quiverSB_3, packSB_3, partsSB_3
									}));
								SlotGroup cGearsSG_3 = MakeSG();
									cGearsSG_3.SetFilterHandler(new FilterHandler());
									cGearsSG_3.SetFilter(new SGCGearsFilter());
									ISlottable oShieldSB_3 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), cGearsSG_3);
								withCGears = new object[]{sg_3, oShieldSB_3, new List<ISlottable>(new ISlottable[]{shieldSB_3, mWeaponSB_3, quiverSB_3, packSB_3})};
								yield return withCGears;
							object[] sameParts_empty;
								SlotGroup sg_4 = MakeSG();
									sg_4.SetFilterHandler(new FilterHandler());
									sg_4.SetFilter(new SGNullFilter());
									ISlottable bow0SB_4 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_4);
									ISlottable bow1SB_4 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_4);
									ISlottable wearSB_4 = MakeSubSBWithItemAndSG(MakeWearInstance(0), sg_4);
									ISlottable shieldSB_4 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), sg_4);
									ISlottable mWeaponSB_4 = MakeSubSBWithItemAndSG(MakeMWeaponInstance(0), sg_4);
									ISlottable quiverSB_4 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_4);
									ISlottable packSB_4 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_4);
									ISlottable partsSB_4 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), sg_4);
									sg_4.SetSBHandler(new SBHandler());
									sg_4.SetSBs(new List<ISlottable>(new ISlottable[]{
										bow0SB_4, bow1SB_4, wearSB_4, shieldSB_4, mWeaponSB_4, quiverSB_4, packSB_4, partsSB_4
									}));
								SlotGroup partsSG_4 = MakeSG();
									partsSG_4.SetFilterHandler(new FilterHandler());
									partsSG_4.SetFilter(new SGPartsFilter());
									ISlottable oPartsSB_4 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 4), partsSG_4);
								sameParts_empty = new object[]{sg_4, oPartsSB_4, new List<ISlottable>(new ISlottable[]{})};
								yield return sameParts_empty;
							object[] withOtherParts;
								SlotGroup sg_5 = MakeSG();
									sg_5.SetFilterHandler(new FilterHandler());
									sg_5.SetFilter(new SGNullFilter());
									ISlottable bow0SB_5 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_5);
									ISlottable bow1SB_5 = MakeSubSBWithItemAndSG(MakeBowInstance(0), sg_5);
									ISlottable wearSB_5 = MakeSubSBWithItemAndSG(MakeWearInstance(0), sg_5);
									ISlottable shieldSB_5 = MakeSubSBWithItemAndSG(MakeShieldInstance(0), sg_5);
									ISlottable mWeaponSB_5 = MakeSubSBWithItemAndSG(MakeMWeaponInstance(0), sg_5);
									ISlottable quiverSB_5 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_5);
									ISlottable packSB_5 = MakeSubSBWithItemAndSG(MakeQuiverInstance(0), sg_5);
									ISlottable partsSB_5 = MakeSubSBWithItemAndSG(MakePartsInstance(0, 1), sg_5);
									sg_5.SetSBHandler(new SBHandler());
									sg_5.SetSBs(new List<ISlottable>(new ISlottable[]{
										bow0SB_5, bow1SB_5, wearSB_5, shieldSB_5, mWeaponSB_5, quiverSB_5, packSB_5, partsSB_5
									}));
								SlotGroup partsSG_5 = MakeSG();
									partsSG_5.SetFilterHandler(new FilterHandler());
									partsSG_5.SetFilter(new SGPartsFilter());
									ISlottable oPartsSB_5 = MakeSubSBWithItemAndSG(MakePartsInstance(1, 4), partsSG_5);
								withOtherParts = new object[]{sg_5, oPartsSB_5, new List<ISlottable>(new ISlottable[]{partsSB_5})};
								yield return withOtherParts;
						}
					}
				[Test]
				public void Refresh_Always_SetActStateWFA(){
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						SGActStateHandler handler = new SGActStateHandler(sg);
						sg.SetSGActStateHandler(handler);

					sg.Refresh();
					
					Assert.That(sg.isWaitingForAction, Is.True);
				}
				[Test]
				public void Refresh_Always_SetsFields(){
					SlotGroup sg = MakeSGWithRealStateHandlersAndRealSlotsHolder();

					sg.Refresh();

					Assert.That(sg.newSBs, Is.Empty);
					Assert.That(sg.newSlots, Is.Empty);
				}
			/*	sse override	*/
				[TestCaseSource(typeof(ContainsCases))]
				public void Contains_Vairous_ReturnsAccordingly(List<ISlottable> sbs, IEnumerable<ISlottable> expValid, IEnumerable<ISlottable> expInvalid){
					SlotGroup sg = MakeSG_ISBHandler(sbs);

					foreach(var sb in expValid)
						Assert.That(sg.Contains(sb), Is.True);
					foreach(var sb in expInvalid)
						Assert.That(sg.Contains(sb), Is.False);
				}
					class ContainsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object case0;
								ISlottable vSB0_0 = MakeSubSB();
								ISlottable vSB1_0 = MakeSubSB();
								ISlottable vSB2_0 = MakeSubSB();
								ISlottable iSB0_0 = MakeSubSB();
								ISlottable iSB1_0 = MakeSubSB();
								ISlottable iSB2_0 = MakeSubSB();
								List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
									vSB0_0, vSB1_0, vSB2_0
								});
								List<ISlottable> valid_0 = new List<ISlottable>(new ISlottable[]{
									vSB0_0, vSB1_0, vSB2_0
								});
								List<ISlottable> invalid_0 = new List<ISlottable>(new ISlottable[]{
									iSB0_0, iSB1_0, iSB2_0
								});
								case0 = new object[]{sbs_0, valid_0, invalid_0};
								yield return case0;
						}
					}
				[Test]
				public void FocusSBs_Always_CallsSBRefresh(){
					SlotGroup sg;
						List<ISlottable> sbs;
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
							sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC});
						sg = MakeSG_ISBHandler(sbs);
					
					sg.FocusSBs();

					sbA.Received().Refresh();
					sbB.Received().Refresh();
					sbC.Received().Refresh();
				}
				[TestCaseSource(typeof(FocusSBsVariousCases))]
				public void FocusSBs_Various_CallsSBFocusOrDefocus(List<ISlottable> sbs, Dictionary<ISlotSystemElement, bool> dict){
					SlotGroup sg = MakeSG_ISBHandler(sbs);

					sg.FocusSBs();

					foreach(var e in sg){
						if(dict[e])
							e.Received().Focus();
						else
							e.Received().Defocus();
					}
				}
					class FocusSBsVariousCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable fSBA = MakeSubSB();
								fSBA.passesPrePickFilter.Returns(true);
							ISlottable fSBB = MakeSubSB();
								fSBB.passesPrePickFilter.Returns(true);
							ISlottable fSBC = MakeSubSB();
								fSBC.passesPrePickFilter.Returns(true);
							ISlottable dSBA = MakeSubSB();
								dSBA.passesPrePickFilter.Returns(false);
							ISlottable dSBB = MakeSubSB();
								dSBB.passesPrePickFilter.Returns(false);
							ISlottable dSBC = MakeSubSB();
								dSBC.passesPrePickFilter.Returns(false);
							List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{
								fSBA, fSBB, fSBC, dSBA, dSBB, dSBC
							});
							Dictionary<ISlotSystemElement, bool> dict = new Dictionary<ISlotSystemElement, bool>();
								dict.Add(fSBA, true);
								dict.Add(fSBB, true);
								dict.Add(fSBC, true);
								dict.Add(dSBA, false);
								dict.Add(dSBB, false);
								dict.Add(dSBC, false);
							yield return new object[]{ sbs, dict};
						}
					}
				[Test]
				public void DefocusSBs_Always_CallsSBRefresh(){
					SlotGroup sg;
						List<ISlottable> sbs;
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
							sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC});
						sg = MakeSG_ISBHandler(sbs);
					
					sg.DefocusSBs();

					sbA.Received().Refresh();
					sbB.Received().Refresh();
					sbC.Received().Refresh();
				}
				[Test]
				public void DefocusSBs_Always_CallsSBDefocus(){
					SlotGroup sg;
						List<ISlottable> sbs;
							ISlottable sbA = MakeSubSB();
							ISlottable sbB = MakeSubSB();
							ISlottable sbC = MakeSubSB();
							sbs = new List<ISlottable>(new ISlottable[]{sbA, sbB, sbC});
						sg = MakeSG_ISBHandler(sbs);
					
					sg.DefocusSBs();

					sbA.Received().Defocus();
					sbB.Received().Defocus();
					sbC.Received().Defocus();
					}
				[Test]
				public void InitializeState_Always_InitializesStates(){
					SlotGroup sg = MakeSG_RealStateHandlers_RSBHandler();
					
					sg.InitializeStates();

					Assert.That(sg.isDeactivated, Is.True);
					Assert.That(sg.wasSelStateNull, Is.True);
					Assert.That(sg.isWaitingForAction, Is.True);
					Assert.That(sg.wasActStateNull, Is.True);
					}
				[TestCase(0)]
				[TestCase(10)]
				public void InspectorSetUp_Always_SetsFields(int initSlotsCount){
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						GenericInventory genInv = Substitute.For<GenericInventory>();
						SGFilter filter = new SGNullFilter();
						SGSorter sorter = new SGItemIDSorter();
						SorterHandler sorterHandler = new SorterHandler();
						sg.SetSorterHandler(sorterHandler);
						sg.SetFilterHandler(new FilterHandler());
					
					sg.InspectorSetUp(genInv, filter, sorter, initSlotsCount);

					Assert.That(sg.inventory, Is.SameAs(genInv));
					Assert.That(sg.filter, Is.SameAs(filter));
					Assert.That(sorterHandler.sorter, Is.SameAs(sorter));
					Assert.That(sg.initSlotsCount, Is.EqualTo(initSlotsCount));
					Assert.That(sg.isExpandable, Is.EqualTo(initSlotsCount == 0));
					}
				class SGSetUpFieldsData: IEquatable<SGSetUpFieldsData>{
					public string name;
					public SGFilter filter;
					public Inventory inventory;
					public ISGCommand OnACompComm;
					public ISGCommand oAExecComm;
					public bool isShrinkable;
					public int initSlotsCount;
					public bool isExpandable;
					public SSEState sgSelState;
					public SSEState sgActState;
					public SGSetUpFieldsData(string name, SGFilter filter, Inventory inventory, ISGCommand OnACompComm, ISGCommand oAExecComm, bool isShrinkable, int initSlotsCount, bool isExpandable, SSEState sgSelState, SSEState sgActState){
						this.name = name;
						this.filter = filter;
						this.inventory = inventory;
						this.OnACompComm = OnACompComm;
						this.oAExecComm = oAExecComm;
						this.isShrinkable = isShrinkable;
						this.initSlotsCount = initSlotsCount;
						this.isExpandable = isExpandable;
						this.sgSelState = sgSelState;
						this.sgActState = sgActState;
					}
					public bool Equals(SGSetUpFieldsData other){
						bool flag = true;
							flag &= object.ReferenceEquals(this.filter, other.filter);
							flag &= object.ReferenceEquals(this.inventory, other.inventory);
							flag &= object.ReferenceEquals(this.OnACompComm, other.OnACompComm);
							flag &= object.ReferenceEquals(this.oAExecComm, other.oAExecComm);
							flag &= this.isShrinkable == other.isShrinkable;
							flag &= this.initSlotsCount == other.initSlotsCount;
							flag &= this.isExpandable == other.isExpandable;
							flag &= object.ReferenceEquals(this.sgSelState, other.sgSelState);
							flag &= object.ReferenceEquals(this.sgActState, other.sgActState);
						return flag;
					}
				}
			/* Transaction */
				[TestCase(1)]
				[TestCase(10)]
				[TestCase(0)]
				public void UpdateSBs_Always_SetsNewSlotsWithTheSameNumberAsSBs(int count){
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						List<ISlottable> sbs = new List<ISlottable>();
						sg.SetSBs(sbs);
						List<ISlottable> newSBs = CreateSBs(count);
					
					sg.ReadySBsForTransaction(newSBs);

					Assert.That(sg.newSlots.Count, Is.EqualTo(count));
					Assert.That(sg.newSlots, Is.All.InstanceOf(typeof(Slot)));
				}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))]
				public void UpdateSBs_NewSBsContainsSB_CallsSetNewSlotIDWithNewSBsID(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						sg.SetSBs(sbs);
					
					sg.ReadySBsForTransaction(newSBs);

					foreach(ISlottable sb in sg)
						if(newSBs.Contains(sb))
							sb.Received().SetNewSlotID(newSBs.IndexOf(sb));
				}
					class UpdateSBsNewSBsContainsSBCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlottable oSBA = MakeSubSB();
							ISlottable oSBB = MakeSubSB();
							ISlottable oSBC = MakeSubSB();
							ISlottable aSBA = MakeSubSB();
							ISlottable aSBB = MakeSubSB();
							ISlottable aSBC = MakeSubSB();
							ISlottable rSBA = MakeSubSB();
							ISlottable rSBB = MakeSubSB();
							ISlottable rSBC = MakeSubSB();
							List<ISlottable> case1SBs = new List<ISlottable>(new ISlottable[]{
								oSBA, oSBB, oSBC, rSBA, rSBB, rSBC
							});
							List<ISlottable> case1NewSBs = new List<ISlottable>(new ISlottable[]{
								oSBB,
								aSBA,
								oSBC,
								aSBB,
								oSBA,
								aSBC
							});
							yield return new object[]{case1SBs, case1NewSBs};
						}
					}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))]
				public void UpdateSBs_NewSBsAndSBsContainsSB_CallsSBMoveWithin(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						sg.SetSBs(sbs);
					
					sg.ReadySBsForTransaction(newSBs);

					foreach(ISlottable sb in sg)
						if(newSBs.Contains(sb) && sbs.Contains(sb))
							sb.Received().MoveWithin();
						else
							sb.DidNotReceive().MoveWithin();
				}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))]
				public void UpdateSBs_NewSBNotContainsSB_CallsSetNewSlotIDMinus1(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						sg.SetSBs(sbs);

					sg.ReadySBsForTransaction(newSBs);

					foreach(ISlottable sb in sg)
						if(!newSBs.Contains(sb))
							sb.Received().SetNewSlotID(-1);
						else
							sb.DidNotReceive().SetNewSlotID(-1);
				}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))]
				public void UpdateSBs_NewSBNotContainsSB_CallsSBRemove(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						sg.SetSBs(sbs);

					sg.ReadySBsForTransaction(newSBs);

					foreach(ISlottable sb in sg)
						if(!newSBs.Contains(sb))
							sb.Received().Remove();
						else
							sb.DidNotReceive().Remove();
				}
				[TestCaseSource(typeof(UpdateSBsNewSBsContainsSBCases))]
				public void UpdateSBs_SBsNotContainsSB_CallsSBAdd(List<ISlottable> sbs, List<ISlottable> newSBs){
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						sg.SetSBs(sbs);

					sg.ReadySBsForTransaction(newSBs);

					foreach(ISlottable sb in sg)
						if(!sbs.Contains(sb))
							sb.Received().Add();
						else
							sb.DidNotReceive().Add();
				}
				[TestCase(0)]
				[TestCase(1)]
				[TestCase(10)]
				public void CreateNewSlots_Always_SetsNewSlotsWithSameCountAsNewSBs(int count){
					SlotGroup sg = MakeSGInitWithSubs();
						SlotsHolder slotsHolder = new SlotsHolder(sg);
						sg.SetSlotsHolder(slotsHolder);
						sg.SetSBHandler(new SBHandler());
						sg.SetSBs(new List<ISlottable>());
						List<ISlottable> newSBs = CreateSBs(count);
						sg.SetNewSBs(newSBs);
					
					sg.CreateNewSlots();

					Assert.That(sg.newSlots.Count, Is.EqualTo(count));
					foreach(var slot in sg.newSlots)
						Assert.That(slot.sb, Is.Null);
				}
				[Test]
				public void UpdateToRevert_Always_SetsNewSBsWithSBs(){
					List<ISlottable> sbs = CreateSBs(3);
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						sg.SetSBs(sbs);
					
					sg.RevertAndUpdateSBs();
					
					bool equality = sg.newSBs.MemberEquals(sbs);
					Assert.That(equality, Is.True);
				}
				[TestCase(1)]
				[TestCase(3)]
				[TestCase(10)]
				public void UpdateToRevert_Always_SetsNewSlotsBySBsCount(int count){
					List<ISlottable> sbs = CreateSBs(count);
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						sg.SetSBs(sbs);
					
					sg.RevertAndUpdateSBs();

					Assert.That(sg.newSlots.Count, Is.EqualTo(count));
					foreach(var slot in sg.newSlots)
						Assert.That(slot.sb, Is.Null);
				}
				[Test]
				public void UpdateToRevert_Always_DoesNotChangeSBs(){
					List<ISlottable> sbs = CreateSBs(3);
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						sg.SetSBs(sbs);
					
					sg.RevertAndUpdateSBs();
					
					bool equality = sg.toList.MemberEquals(sbs);
					Assert.That(equality, Is.True);
				}
				[Test]
				public void UpdateToRevert_Always_CallSBsMoveWithin(){
					List<ISlottable> sbs = CreateSBs(3);
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						sg.SetSBs(sbs);
					
					sg.RevertAndUpdateSBs();

					foreach(ISlottable sb in sg)
						sb.Received().MoveWithin();
				}
				[TestCaseSource(typeof(InitSBs_SlotsNotEnoughCases))]
				public void InitSBs_SlotsNotEnough_RemoveNonFittableItems(List<Slot> slots, List<InventoryItemInstance> items, List<InventoryItemInstance> expected){
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						sg.SetSlots(slots);
					
					sg.InitSBs(items);

					bool equality = items.MemberEquals(expected);
					Assert.That(equality, Is.True);					
					}
					class InitSBs_SlotsNotEnoughCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							List<Slot> slots = CreateSlots(4);
							List<InventoryItemInstance> items;
								BowInstance bowA = MakeBowInstance(0);
								BowInstance bowA_1 = MakeBowInstance(0);
								BowInstance bowA_2 = MakeBowInstance(0);
								WearInstance wear = MakeWearInstance(0);
								ShieldInstance shield = MakeShieldInstance(0);
								MeleeWeaponInstance mWeapon = MakeMWeaponInstance(0);
								items = new List<InventoryItemInstance>(new InventoryItemInstance[]{bowA, bowA_1, bowA_2, wear, shield, mWeapon});
							List<InventoryItemInstance> expected = new List<InventoryItemInstance>(new InventoryItemInstance[]{bowA, bowA_1, bowA_2, wear});
							yield return new object[]{slots, items, expected};

						}
					}
				[TestCaseSource(typeof(InitSBs_AlwaysCases))]
				public void InitSBs_Always_CreatesAndSetsSBsInSlots(ISlotSystemManager ssm ,List<Slot> slots, List<InventoryItemInstance> items){
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						sg.SetSSM(ssm);
						sg.SetSlots(slots);
					
					sg.InitSBs(items);

					for(int i = 0; i< slots.Count; i++){
						ISlottable sb = slots[i].sb;
						Assert.That(sb.ssm, Is.SameAs(ssm));
						Assert.That(sb.item, Is.SameAs(items[i]));
						Assert.That(sb.isDefocused, Is.True);
						Assert.That(sb.isEquipped, Is.False);
						Assert.That(sb.isUnequipped, Is.False);
					}
				}
					class InitSBs_AlwaysCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							ISlotSystemManager ssm = MakeSubSSM();
							List<Slot> slots = CreateSlots(4);
							List<InventoryItemInstance> items;
								BowInstance bowA = MakeBowInstance(0);
								BowInstance bowB = MakeBowInstance(1);
								BowInstance bowC = MakeBowInstance(2);
								BowInstance bowD = MakeBowInstance(3);
								items = new List<InventoryItemInstance>(new InventoryItemInstance[]{bowA, bowB, bowC, bowD});
							yield return new object[]{
								ssm, slots, items
							};
						}
					}
				[TestCaseSource(typeof(SyncSBsToSlotsCases))]
				public void SyncSBsToSlots_Always_SyncSBsToSlots(List<Slot> slots, List<ISlottable> expected){
					SlotGroup sg = MakeSGInitWithSubsAndRealSlotsHolder();
						sg.SetSlots(slots);
					
					sg.SetSBsFromSlotsAndUpdateSlotIDs();

					bool equality = sg.toList.MemberEquals(expected);
					Assert.That(equality, Is.True);
					foreach(ISlottable sb in sg){
						if(sb != null)
							sb.Received().SetSlotID(sg.toList.IndexOf(sb));
					}
				}
					class SyncSBsToSlotsCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							List<Slot> slots;
								Slot slotA = new Slot();
									ISlottable sbA = MakeSubSB();
									slotA.sb = sbA;
								Slot slotNA = new Slot();
								Slot slotB = new Slot();
									ISlottable sbB = MakeSubSB();
									slotB.sb = sbB;
								Slot slotC = new Slot();
									ISlottable sbC = MakeSubSB();
									slotC.sb = sbC;
								Slot slotNB = new Slot();
								Slot slotNC = new Slot();
								Slot slotD = new Slot();
									ISlottable sbD = MakeSubSB();
									slotD.sb = sbD;
								Slot slotE = new Slot();
									ISlottable sbE = MakeSubSB();
									slotE.sb = sbE;
								slots = new List<Slot>(new Slot[]{slotA, slotNA, slotB, slotC, slotNB, slotNC, slotD, slotE});
							List<ISlottable> expected = new List<ISlottable>(new ISlottable[]{
								sbA, null, sbB, sbC, null, null, sbD, sbE
							});
							yield return new object[]{slots, expected};
						}
					}
			/*	helper */
				static ISlottable MakeSubSBWithItemAndSG(InventoryItemInstance item, SlotGroup sg){
					ISlottable sb = MakeSubSB();
						sb.item.Returns(item);
						sb.sg.Returns(sg);
					return sb;
				}
				List<ISlottable> CreateSBs(int count){
					List<ISlottable> sbs = new List<ISlottable>();
					for(int i =0; i< count; i++){
						ISlottable sb = MakeSubSB();
						sbs.Add(sb);
					}
					return sbs;
				}
				static ISlottable MakeSBWithActProc(bool isRunning){
					ISlottable sb = MakeSubSB();
						ISSEProcess sbActProc = MakeSubSBActProc();
							sbActProc.isRunning.Returns(isRunning);
						sb.actProcess.Returns(sbActProc);
					return sb;
				}
				ISlotSystemManager MakeSSMWithPBunContaining(ISlotGroup sg){
					ISlotSystemManager stubSSM = MakeSubSSM();
						ISlotSystemBundle stubPBun = MakeSubBundle();
							stubPBun.ContainsInHierarchy(sg).Returns(true);
						stubSSM.poolBundle.Returns(stubPBun);
					return stubSSM;
				}
				ISlotSystemManager MakeSSMWithEBunContaining(ISlotGroup sg){
					ISlotSystemManager stubSSM = MakeSubSSM();
						ISlotSystemBundle stubEBun = MakeSubBundle();
							stubEBun.ContainsInHierarchy(sg).Returns(true);
						stubSSM.equipBundle.Returns(stubEBun);
					return stubSSM;
				}
				ISlotSystemManager MakeSSMWithGBunContaining(ISlotGroup sg){
					ISlotSystemManager stubSSM = MakeSubSSM();
						IEnumerable<ISlotSystemBundle> gBuns;
							ISlotSystemBundle stubGBun = MakeSubBundle();
								stubGBun.ContainsInHierarchy(sg).Returns(true);
							gBuns = new ISlotSystemBundle[]{stubGBun};
						stubSSM.otherBundles.Returns(gBuns);
					return stubSSM;
				}
		}
	}
}
