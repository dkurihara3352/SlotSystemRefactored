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
		[TestFixture]
		public class SBHandlerTests :SlotSystemTest{
			[TestCaseSource(typeof(GetSBCases))]
			public void GetSB_Match_ReturnsSB(List<ISlottable> sbs, List<IInventoryItemInstance> items){
				SBHandler sbHandler = new SBHandler();
					sbHandler.SetSBs(sbs);

				foreach(var item in items){
					ISlottable actual = sbHandler.GetSB(item);
					Assert.That(actual, Is.SameAs(sbs[items.IndexOf(item)]));
				}
			}
			[TestCaseSource(typeof(GetSBCases))]
			public void GetSB_NonMatch_ReturnsNull(List<ISlottable> sbs, List<IInventoryItemInstance> items){
				SBHandler sbHandler = new SBHandler();
					sbHandler.SetSBs(sbs);
				
				ISlottable actual = sbHandler.GetSB(MakeBowInstance(0));

				Assert.That(actual, Is.Null);
			}
				class GetSBCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] validBow;
							ISlottable bowA0SB_0 = MakeSubSB();
								BowInstance bowA0_0 = MakeBowInstance(0);
								bowA0SB_0.GetItem().Returns(bowA0_0);
							ISlottable wearA0SB_0 = MakeSubSB();
								WearInstance wearA0_0 = MakeWearInstance(0);
								wearA0SB_0.GetItem().Returns(wearA0_0);
							ISlottable shieldA0SB_0 = MakeSubSB();
								ShieldInstance shieldA0_0 = MakeShieldInstance(0);
								shieldA0SB_0.GetItem().Returns(shieldA0_0);
							ISlottable mWeaponA0SB_0 = MakeSubSB();
								MeleeWeaponInstance mWeaponA0_0 = MakeMWeaponInstance(0);
								mWeaponA0SB_0.GetItem().Returns(mWeaponA0_0);
							ISlottable quiverA0SB_0 = MakeSubSB();
								QuiverInstance quiverA0_0 = MakeQuiverInstance(0);
								quiverA0SB_0.GetItem().Returns(quiverA0_0);
							ISlottable packA0SB_0 = MakeSubSB();
								PackInstance packA0_0 = MakePackInstance(0);
								packA0SB_0.GetItem().Returns(packA0_0);
							ISlottable partsA0SB_0 = MakeSubSB();
								PartsInstance partsA0_0 = MakePartsInstance(0,2);
								partsA0SB_0.GetItem().Returns(partsA0_0);
							List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
								bowA0SB_0,
								wearA0SB_0,
								shieldA0SB_0,
								mWeaponA0SB_0,
								quiverA0SB_0,
								packA0SB_0,
								partsA0SB_0
							});
							List<IInventoryItemInstance> items_0 = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{
								bowA0_0,
								wearA0_0,
								shieldA0_0,
								mWeaponA0_0,
								quiverA0_0,
								packA0_0,
								partsA0_0
							});
							validBow = new object[]{sbs_0, items_0};
							yield return validBow;
					}
				}
			[TestCaseSource(typeof(EquippedSBsCases))]
			public void equippedSBs_Always_ReturnsAllSBsIsEquipped(List<ISlottable> sbs, List<ISlottable> expected){
				SBHandler sbHandler = new SBHandler();
					sbHandler.SetSBs(sbs);

				List<ISlottable> actual = sbHandler.GetEquippedSBs();

				bool equality = actual.MemberEquals(expected);
				Assert.That(equality, Is.True);
			}
				class EquippedSBsCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						ISlottable eSBA = MakeSubSB();
							eSBA.IsEquipped().Returns(true);
						ISlottable eSBB = MakeSubSB();
							eSBB.IsEquipped().Returns(true);
						ISlottable eSBC = MakeSubSB();
							eSBC.IsEquipped().Returns(true);
						ISlottable uSBA = MakeSubSB();
							uSBA.IsEquipped().Returns(false);
						ISlottable uSBB = MakeSubSB();
							uSBB.IsEquipped().Returns(false);
						ISlottable uSBC = MakeSubSB();
							uSBC.IsEquipped().Returns(false);
						List<ISlottable> case1SBs = new List<ISlottable>(new ISlottable[]{
							eSBA, eSBB, eSBC, uSBA, uSBB, uSBC
						});
						List<ISlottable> case1Exp = new List<ISlottable>(new ISlottable[]{
							eSBA, eSBB, eSBC
						});
						yield return new object[]{case1SBs, case1Exp};
					}
				}
				[TestCaseSource(typeof(IsAllSBsActProcDoneCases))]
			public void isAllSBsActProcDone_Various_ReturnsAccordingly(List<ISlottable> sbs, bool expected){
				SBHandler sbHandler = new SBHandler();
					sbHandler.SetSBs(sbs);

				bool actual = sbHandler.IsAllSBActProcDone();

				Assert.That(actual, Is.EqualTo(expected));
			}
				class IsAllSBsActProcDoneCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] mixed_F;
							ISlottable vSBA_0 = MakeSBWithActProc(false);
							ISlottable vSBB_0 = MakeSBWithActProc(false);
							ISlottable vSBC_0 = MakeSBWithActProc(false);
							ISlottable iSBA_0 = MakeSBWithActProc(true);
							ISlottable iSBB_0 = MakeSBWithActProc(true);
							ISlottable iSBC_0 = MakeSBWithActProc(true);
							List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
								vSBA_0, vSBB_0, vSBC_0, iSBA_0, iSBB_0, iSBC_0
							});
							mixed_F = new object[]{sbs_0, false};
							yield return mixed_F;
						object[] allDone_T;
							ISlottable vSBA_1 = MakeSBWithActProc(false);
							ISlottable vSBB_1 = MakeSBWithActProc(false);
							ISlottable vSBC_1 = MakeSBWithActProc(false);
							List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
								vSBA_1, vSBB_1, vSBC_1
							});
							allDone_T = new object[]{sbs_1, true};
							yield return allDone_T;
						object[] allNotDone_F;
							ISlottable iSBA_2 = MakeSBWithActProc(true);
							ISlottable iSBB_2 = MakeSBWithActProc(true);
							ISlottable iSBC_2 = MakeSBWithActProc(true);
							List<ISlottable> sbs_2 = new List<ISlottable>(new ISlottable[]{
								iSBA_2, iSBB_2, iSBC_2
							});
							allNotDone_F = new object[]{sbs_2, false};
							yield return allNotDone_F;
					}
				}
			[TestCaseSource(typeof(SetSBsActStatesCases))]
			public void SetSBsActStates_Various_CallsSBsAccordingly(List<ISlottable> sbs, List<ISlottable> newSBs, List<ISlottable> allSBs, List<ISlottable> xMoveWithin, List<ISlottable> xAdded, List<ISlottable> xRemoved){
				SBHandler sbHandler = new SBHandler();
					sbHandler.SetSBs(sbs);
					sbHandler.SetNewSBs(newSBs);

				sbHandler.SetSBsActStates();

				foreach(var sb in xMoveWithin){
					sb.Received().MoveWithin();
					sb.DidNotReceive().Add();
					sb.DidNotReceive().Remove();
				}
				foreach(var sb in xAdded){
					sb.DidNotReceive().MoveWithin();
					sb.Received().Add();
					sb.DidNotReceive().Remove();
				}
				foreach(var sb in xRemoved){
					sb.DidNotReceive().MoveWithin();
					sb.DidNotReceive().Add();
					sb.Received().Remove();
				}
			}
				class SetSBsActStatesCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						ISlottable mSB_0_0 = MakeSubSB();
						ISlottable mSB_1_0 = MakeSubSB();
						ISlottable mSB_2_0 = MakeSubSB();
						ISlottable aSB_0_0 = MakeSubSB();
						ISlottable aSB_1_0 = MakeSubSB();
						ISlottable aSB_2_0 = MakeSubSB();
						ISlottable rSB_0_0 = MakeSubSB();
						ISlottable rSB_1_0 = MakeSubSB();
						ISlottable rSB_2_0 = MakeSubSB();

						List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
							mSB_0_0,
							mSB_1_0,
							mSB_2_0,
							rSB_0_0,
							rSB_1_0,
							rSB_2_0
						});
						List<ISlottable> newSBs_0 = new List<ISlottable>(new ISlottable[]{
							mSB_0_0,
							mSB_1_0,
							mSB_2_0,
							aSB_0_0,
							aSB_1_0,
							aSB_2_0

						});
						List<ISlottable> allSBs_0 = new List<ISlottable>(new ISlottable[]{
							mSB_0_0,
							mSB_1_0,
							mSB_2_0,
							aSB_0_0,
							aSB_1_0,
							aSB_2_0,
							rSB_0_0,
							rSB_1_0,
							rSB_2_0,
						});
						List<ISlottable> xMoveWithin_0 = new List<ISlottable>(new ISlottable[]{
							mSB_0_0,
							mSB_1_0,
							mSB_2_0
						});
						List<ISlottable> xAdded_0 = new List<ISlottable>(new ISlottable[]{
							aSB_0_0,
							aSB_1_0,
							aSB_2_0
						});
						List<ISlottable> xRemoved_0 = new List<ISlottable>(new ISlottable[]{
							rSB_0_0,
							rSB_1_0,
							rSB_2_0
						});
						yield return new object[]{sbs_0, newSBs_0, allSBs_0, xMoveWithin_0, xAdded_0, xRemoved_0};
					}
				}
		/* helpter */
			static ISlottable MakeSBWithActProc(bool isRunning){
				ISlottable sb = MakeSubSB();
					ISSEProcess sbActProc = MakeSubSBActProc();
						sbActProc.IsRunning().Returns(isRunning);
					ISBActStateHandler actStateHandler = Substitute.For<ISBActStateHandler>();
						actStateHandler.GetActProcess().Returns(sbActProc);
					sb.GetActStateHandler().Returns(actStateHandler);
				return sb;
			}
		}
	}
}
