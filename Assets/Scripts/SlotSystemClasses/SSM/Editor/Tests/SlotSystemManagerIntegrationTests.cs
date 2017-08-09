using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using SlotSystem;
using Utility;
namespace SlotSystemTests{
	[TestFixture]
	[Category("Integration")]
	public class SlotSystemManagerIntegrationTests: SlotSystemTest {
			[Test]
			public void SetTACacheRecursively_Always_SetsIHoverableTACache(){
				SlotSystemManager ssm = MakeSSM();
					SlotSystemBundle pBun = MakeSSBundle();
					pBun.transform.SetParent(ssm.transform);
						SlotGroup sgpA = MakeSGInitWithSubsAndRealCommandsAndRealSlotsHolder();
							sgpA.transform.SetParent(pBun.transform);
							PoolInventory pInv = new PoolInventory();
								BowInstance bowA = MakeBowInstance(0);
								WearInstance wearA = MakeWearInstance(0);
								ShieldInstance shieldA = MakeShieldInstance(0);
								MeleeWeaponInstance mWeaponA = MakeMeleeWeaponInstance(0);
								pInv.Add(bowA);
								pInv.Add(wearA);
								pInv.Add(shieldA);
								pInv.Add(mWeaponA);
							sgpA.InspectorSetUp(pInv, new SGNullFilter(), new SGItemIDSorter(), 0);
							sgpA.SetHierarchy();
							IEnumerable<ISlotSystemElement> xSGPAEles;
								ISlottable bowSBP = sgpA.GetSB(bowA);
								ISlottable wearSBP = sgpA.GetSB(wearA);
								ISlottable shieldSBP = sgpA.GetSB(shieldA);
								ISlottable mWeaponSBP = sgpA.GetSB(mWeaponA);
								xSGPAEles = new ISlotSystemElement[]{bowSBP, wearSBP, shieldSBP, mWeaponSBP};
						SlotGroup sgpB = MakeSGInitWithSubsAndRealCommandsAndRealSlotsHolder();
							sgpB.transform.SetParent(pBun.transform);
						pBun.SetHierarchy();
					SlotSystemBundle eBun = MakeSSBundle();
					eBun.transform.SetParent(ssm.transform);
						EquipmentSet eSetA = MakeEquipmentSet();
						eSetA.transform.SetParent(eBun.transform);
							IEquipmentSetInventory eInv = new EquipmentSetInventory(MakeBowInstance(0), MakeWearInstance(0), new List<CarriedGearInstance>(), 1);
							SlotGroup sgeBow = MakeSGInitWithSubsAndRealCommandsAndRealSlotsHolder();
								sgeBow.transform.SetParent(eSetA.transform);
								sgeBow.InspectorSetUp(eInv, new SGBowFilter(), new SGItemIDSorter(), 1);
							SlotGroup sgeWear = MakeSGInitWithSubsAndRealCommandsAndRealSlotsHolder();
								sgeWear.transform.SetParent(eSetA.transform);
								sgeWear.InspectorSetUp(eInv, new SGWearFilter(), new SGItemIDSorter(), 1);
							SlotGroup sgeCGears = MakeSGInitWithSubsAndRealCommandsAndRealSlotsHolder();
								sgeCGears.transform.SetParent(eSetA.transform);
								sgeCGears.InspectorSetUp(eInv, new SGCGearsFilter(), new SGItemIDSorter(), 1);
							eSetA.SetHierarchy();
						eBun.SetHierarchy();
					SlotSystemBundle gBunA = MakeSSBundle();
					gBunA.transform.SetParent(ssm.transform);
						TestSlotSystemElement ssegA = MakeTestSSE();
						ssegA.transform.SetParent(gBunA.transform);
							SlotGroup sggAA = MakeSGInitWithSubsAndRealCommandsAndRealSlotsHolder();
							sggAA.transform.SetParent(ssegA.transform);
							SlotGroup sggAB = MakeSGInitWithSubsAndRealCommandsAndRealSlotsHolder();
							sggAB.transform.SetParent(ssegA.transform);
						ssegA.SetHierarchy();
						SlotSystemBundle gBunAA = MakeSSBundle();
						gBunAA.transform.SetParent(gBunA.transform);
							SlotGroup sggAAA = MakeSGInitWithSubsAndRealCommandsAndRealSlotsHolder();
							sggAAA.transform.SetParent(gBunAA.transform);
							SlotGroup sggAAB = MakeSGInitWithSubsAndRealCommandsAndRealSlotsHolder();
							sggAAB.transform.SetParent(gBunAA.transform);
						gBunAA.SetHierarchy();
					gBunA.SetHierarchy();
				ssm.SetHierarchy();
					IEnumerable<ISlotSystemElement> xSSMEles = new ISlotSystemElement[]{pBun, eBun, gBunA};
					Assert.That(ssm.MemberEquals(xSSMEles), Is.True);
					IEnumerable<ISlotSystemElement> xPBunEles = new ISlotSystemElement[]{sgpA, sgpB};
					Assert.That(pBun.MemberEquals(xPBunEles), Is.True);
					Assert.That(sgpA.MemberEquals(xSGPAEles), Is.True);
					IEnumerable<ISlotSystemElement> xEBunEles = new ISlotSystemElement[]{eSetA};
					Assert.That(eBun.MemberEquals(xEBunEles), Is.True);
					IEnumerable<ISlotSystemElement> xESetAEles = new ISlotSystemElement[]{sgeBow, sgeWear, sgeCGears};
					Assert.That(eSetA.MemberEquals(xESetAEles), Is.True);
					IEnumerable<ISlotSystemElement> xGBunAEles = new ISlotSystemElement[]{ssegA, gBunAA};
					Assert.That(gBunA.MemberEquals(xGBunAEles), Is.True);
					IEnumerable<ISlotSystemElement> xSSEGAEles = new ISlotSystemElement[]{sggAA, sggAB};
					Assert.That(ssegA.MemberEquals(xSSEGAEles), Is.True);
					IEnumerable<ISlotSystemElement> xGBunAAEles = new ISlotSystemElement[]{sggAAA, sggAAB};
					Assert.That(gBunAA.MemberEquals(xGBunAAEles), Is.True);
				ITransactionCache stubTAC = MakeSubTAC();
				ssm.SetTACache(stubTAC);
				
				ssm.SetTACacheRecursively();

				Assert.That(sgpA.taCache, Is.SameAs(stubTAC));
					Assert.That(bowSBP.taCache, Is.SameAs(stubTAC));
					Assert.That(wearSBP.taCache, Is.SameAs(stubTAC));
					Assert.That(shieldSBP.taCache, Is.SameAs(stubTAC));
					Assert.That(mWeaponSBP.taCache, Is.SameAs(stubTAC));
				Assert.That(sgpB.taCache, Is.SameAs(stubTAC));
				Assert.That(sgeBow.taCache, Is.SameAs(stubTAC));
				Assert.That(sgeWear.taCache, Is.SameAs(stubTAC));
				Assert.That(sgeCGears.taCache, Is.SameAs(stubTAC));
				Assert.That(sggAA.taCache, Is.SameAs(stubTAC));
				Assert.That(sggAB.taCache, Is.SameAs(stubTAC));
				Assert.That(sggAAA.taCache, Is.SameAs(stubTAC));
				Assert.That(sggAAB.taCache, Is.SameAs(stubTAC));
			}
		/* helper */
	}
}

