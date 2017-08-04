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
					public IHoverable hovered;
					public DraggedIcon dIcon1;
					public DraggedIcon dIcon2;
					public ISlotSystemTransaction transaction;
					public RefreshTestCase(ISlottable pickedSB, ISlottable targetSB, ISlotGroup sg1, ISlotGroup sg2, IHoverable hovered, DraggedIcon dIcon1, DraggedIcon dIcon2, ISlotSystemTransaction transaction){
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
			[Test]
			public void SetTAMRecursively_Always_SetsIHoverableTAMThis(){
				SlotSystemManager ssm = MakeSSM();
					SlotSystemBundle pBun = MakeSSBundle();
					pBun.transform.SetParent(ssm.transform);
						SlotGroup sgpA = MakeSG();
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
						SlotGroup sgpB = MakeSG();
							sgpB.transform.SetParent(pBun.transform);
						pBun.SetHierarchy();
					SlotSystemBundle eBun = MakeSSBundle();
					eBun.transform.SetParent(ssm.transform);
						EquipmentSet eSetA = MakeEquipmentSet();
						eSetA.transform.SetParent(eBun.transform);
							IEquipmentSetInventory eInv = new EquipmentSetInventory(MakeBowInstance(0), MakeWearInstance(0), new List<CarriedGearInstance>(), 1);
							SlotGroup sgeBow = MakeSG();
								sgeBow.transform.SetParent(eSetA.transform);
								sgeBow.InspectorSetUp(eInv, new SGBowFilter(), new SGItemIDSorter(), 1);
							SlotGroup sgeWear = MakeSG();
								sgeWear.transform.SetParent(eSetA.transform);
								sgeWear.InspectorSetUp(eInv, new SGWearFilter(), new SGItemIDSorter(), 1);
							SlotGroup sgeCGears = MakeSG();
								sgeCGears.transform.SetParent(eSetA.transform);
								sgeCGears.InspectorSetUp(eInv, new SGCGearsFilter(), new SGItemIDSorter(), 1);
							eSetA.SetHierarchy();
						eBun.SetHierarchy();
					SlotSystemBundle gBunA = MakeSSBundle();
					gBunA.transform.SetParent(ssm.transform);
						TestSlotSystemElement ssegA = MakeTestSSE();
						ssegA.transform.SetParent(gBunA.transform);
							SlotGroup sggAA = MakeSG();
							sggAA.transform.SetParent(ssegA.transform);
							SlotGroup sggAB = MakeSG();
							sggAB.transform.SetParent(ssegA.transform);
						ssegA.SetHierarchy();
						SlotSystemBundle gBunAA = MakeSSBundle();
						gBunAA.transform.SetParent(gBunA.transform);
							SlotGroup sggAAA = MakeSG();
							sggAAA.transform.SetParent(gBunAA.transform);
							SlotGroup sggAAB = MakeSG();
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
				ITransactionManager tam = MakeTAM();
					tam.SetSSM(ssm);
				
				tam.SetTAMRecursively();

				Assert.That(sgpA.tam, Is.SameAs(tam));
					Assert.That(bowSBP.tam, Is.SameAs(tam));
					Assert.That(wearSBP.tam, Is.SameAs(tam));
					Assert.That(shieldSBP.tam, Is.SameAs(tam));
					Assert.That(mWeaponSBP.tam, Is.SameAs(tam));
				Assert.That(sgpB.tam, Is.SameAs(tam));
				Assert.That(sgeBow.tam, Is.SameAs(tam));
				Assert.That(sgeWear.tam, Is.SameAs(tam));
				Assert.That(sgeCGears.tam, Is.SameAs(tam));
				Assert.That(sggAA.tam, Is.SameAs(tam));
				Assert.That(sggAB.tam, Is.SameAs(tam));
				Assert.That(sggAAA.tam, Is.SameAs(tam));
				Assert.That(sggAAB.tam, Is.SameAs(tam));
			}
		}
	}
}

