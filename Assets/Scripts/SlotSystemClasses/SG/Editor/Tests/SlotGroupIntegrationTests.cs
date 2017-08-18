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
	namespace SlotGroupTests{
		[TestFixture]
		[Category("Integration")]
		public class SlotGroupIntegrationTests: SlotSystemTest {
			[TestCaseSource(typeof(InstantSortCases))]
			public void InstantSort_Always_ReorderSlotSBs(List<ISlottable> sbs, SGSorter sorter, List<ISlottable> expected){
				List<ISlottable> source = new List<ISlottable>(sbs);
				SlotGroup sg = MakeSG();
					sg.SetSorterHandler(new SorterHandler());
					sg.SetSBHandler(new SBHandler());
					sg.SetSlotsHolder(new SlotsHolder(sg));
				sg.SetSorter(sorter);
				sg.SetSBs(source);
				List<Slot> slots = CreateSlots(source.Count);
				sg.SetSlots(slots);

				sg.InstantSort();

				List<ISlottable> actual = new List<ISlottable>();
				foreach(var slot in sg.GetSlots())
					actual.Add(slot.sb);
				bool equality = actual.MemberEquals(expected);
				Assert.That(equality, Is.True);
			}
				class InstantSortCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] itemIDSorter;
							PartsInstance partsB_0 = MakePartsInstance(1, 1);
							BowInstance bowA_0 = MakeBowInstance(0);
							ShieldInstance shieldA_0 = MakeShieldInstance(0);
							PartsInstance partsA_0 = MakePartsInstance(0, 2);
							BowInstance bowB_0 = MakeBowInstance(1);
							WearInstance wearA_0 = MakeWearInstance(0);
							QuiverInstance quiverA_0 = MakeQuiverInstance(0);
							MeleeWeaponInstance mWeaponA_0 = MakeMWeaponInstance(0);
							PackInstance packA_0 = MakePackInstance(0);
							WearInstance wearB_0 = MakeWearInstance(1);
							partsB_0.SetAcquisitionOrder(0);
							bowA_0.SetAcquisitionOrder(1);
							shieldA_0.SetAcquisitionOrder(2);
							partsA_0.SetAcquisitionOrder(3);
							bowB_0.SetAcquisitionOrder(4);
							wearA_0.SetAcquisitionOrder(5);
							quiverA_0.SetAcquisitionOrder(6);
							mWeaponA_0.SetAcquisitionOrder(7);
							packA_0.SetAcquisitionOrder(8);
							wearB_0.SetAcquisitionOrder(9);
							ISlottable partsBSB_0 = MakeSubSB();
								partsBSB_0.GetItem().Returns(partsB_0);
							ISlottable bowASB_0 = MakeSubSB();
								bowASB_0.GetItem().Returns(bowA_0);
							ISlottable shieldASB_0 = MakeSubSB();
								shieldASB_0.GetItem().Returns(shieldA_0);
							ISlottable partsASB_0 = MakeSubSB();
								partsASB_0.GetItem().Returns(partsA_0);
							ISlottable bowBSB_0 = MakeSubSB();
								bowBSB_0.GetItem().Returns(bowB_0);
							ISlottable wearASB_0 = MakeSubSB();
								wearASB_0.GetItem().Returns(wearA_0);
							ISlottable quiverASB_0 = MakeSubSB();
								quiverASB_0.GetItem().Returns(quiverA_0);
							ISlottable mWeaponASB_0 = MakeSubSB();
								mWeaponASB_0.GetItem().Returns(mWeaponA_0);
							ISlottable packASB_0 = MakeSubSB();
								packASB_0.GetItem().Returns(packA_0);
							ISlottable wearBSB_0 = MakeSubSB();
								wearBSB_0.GetItem().Returns(wearA_0);
							List<ISlottable> sbs_0 = new List<ISlottable>(new ISlottable[]{
								partsBSB_0, 
								bowASB_0, 
								shieldASB_0, 
								partsASB_0, 
								bowBSB_0, 
								wearASB_0, 
								quiverASB_0, 
								mWeaponASB_0, 
								packASB_0, 
								wearBSB_0
							});
							List<ISlottable> expected_0 = new List<ISlottable>(new ISlottable[]{
								bowASB_0,
								bowBSB_0,
								wearASB_0,
								wearBSB_0,
								shieldASB_0,
								mWeaponASB_0,
								quiverASB_0,
								packASB_0,
								partsASB_0,
								partsBSB_0
							});
							itemIDSorter = new object[]{sbs_0, new SGItemIDSorter(), expected_0};
							yield return itemIDSorter;
						object[] inverseIDSorter;
							PartsInstance partsB_1 = MakePartsInstance(1, 1);
							BowInstance bowA_1 = MakeBowInstance(0);
							ShieldInstance shieldA_1 = MakeShieldInstance(0);
							PartsInstance partsA_1 = MakePartsInstance(0, 2);
							BowInstance bowB_1 = MakeBowInstance(1);
							WearInstance wearA_1 = MakeWearInstance(0);
							QuiverInstance quiverA_1 = MakeQuiverInstance(0);
							MeleeWeaponInstance mWeaponA_1 = MakeMWeaponInstance(0);
							PackInstance packA_1 = MakePackInstance(0);
							WearInstance wearB_1 = MakeWearInstance(1);
							partsB_1.SetAcquisitionOrder(0);
							bowA_1.SetAcquisitionOrder(1);
							shieldA_1.SetAcquisitionOrder(2);
							partsA_1.SetAcquisitionOrder(3);
							bowB_1.SetAcquisitionOrder(4);
							wearA_1.SetAcquisitionOrder(5);
							quiverA_1.SetAcquisitionOrder(6);
							mWeaponA_1.SetAcquisitionOrder(7);
							packA_1.SetAcquisitionOrder(8);
							wearB_1.SetAcquisitionOrder(9);
							ISlottable partsBSB_1 = MakeSubSB();
								partsBSB_1.GetItem().Returns(partsB_1);
							ISlottable bowASB_1 = MakeSubSB();
								bowASB_1.GetItem().Returns(bowA_1);
							ISlottable shieldASB_1 = MakeSubSB();
								shieldASB_1.GetItem().Returns(shieldA_1);
							ISlottable partsASB_1 = MakeSubSB();
								partsASB_1.GetItem().Returns(partsA_1);
							ISlottable bowBSB_1 = MakeSubSB();
								bowBSB_1.GetItem().Returns(bowB_1);
							ISlottable wearASB_1 = MakeSubSB();
								wearASB_1.GetItem().Returns(wearA_1);
							ISlottable quiverASB_1 = MakeSubSB();
								quiverASB_1.GetItem().Returns(quiverA_1);
							ISlottable mWeaponASB_1 = MakeSubSB();
								mWeaponASB_1.GetItem().Returns(mWeaponA_1);
							ISlottable packASB_1 = MakeSubSB();
								packASB_1.GetItem().Returns(packA_1);
							ISlottable wearBSB_1 = MakeSubSB();
								wearBSB_1.GetItem().Returns(wearA_1);
							List<ISlottable> sbs_1 = new List<ISlottable>(new ISlottable[]{
								partsBSB_1, 
								bowASB_1, 
								shieldASB_1, 
								partsASB_1, 
								bowBSB_1, 
								wearASB_1, 
								quiverASB_1, 
								mWeaponASB_1, 
								packASB_1, 
								wearBSB_1
							});
							List<ISlottable> expected_1 = new List<ISlottable>(new ISlottable[]{
								partsBSB_1,
								partsASB_1,
								packASB_1,
								quiverASB_1,
								mWeaponASB_1,
								shieldASB_1,
								wearBSB_1,
								wearASB_1,
								bowBSB_1,
								bowASB_1
							});
							inverseIDSorter = new object[]{sbs_1, new SGInverseItemIDSorter(), expected_1};
							yield return inverseIDSorter;
						object[] acqOrderSorter;
							PartsInstance partsB_2 = MakePartsInstance(1, 1);
							BowInstance bowA_2 = MakeBowInstance(0);
							ShieldInstance shieldA_2 = MakeShieldInstance(0);
							PartsInstance partsA_2 = MakePartsInstance(0, 2);
							BowInstance bowB_2 = MakeBowInstance(1);
							WearInstance wearA_2 = MakeWearInstance(0);
							QuiverInstance quiverA_2 = MakeQuiverInstance(0);
							MeleeWeaponInstance mWeaponA_2 = MakeMWeaponInstance(0);
							PackInstance packA_2 = MakePackInstance(0);
							WearInstance wearB_2 = MakeWearInstance(1);
							partsB_2.SetAcquisitionOrder(0);
							bowA_2.SetAcquisitionOrder(1);
							shieldA_2.SetAcquisitionOrder(2);
							partsA_2.SetAcquisitionOrder(3);
							bowB_2.SetAcquisitionOrder(4);
							wearA_2.SetAcquisitionOrder(5);
							quiverA_2.SetAcquisitionOrder(6);
							mWeaponA_2.SetAcquisitionOrder(7);
							packA_2.SetAcquisitionOrder(8);
							wearB_2.SetAcquisitionOrder(9);
							ISlottable partsBSB_2 = MakeSubSB();
								partsBSB_2.GetItem().Returns(partsB_2);
							ISlottable bowASB_2 = MakeSubSB();
								bowASB_2.GetItem().Returns(bowA_2);
							ISlottable shieldASB_2 = MakeSubSB();
								shieldASB_2.GetItem().Returns(shieldA_2);
							ISlottable partsASB_2 = MakeSubSB();
								partsASB_2.GetItem().Returns(partsA_2);
							ISlottable bowBSB_2 = MakeSubSB();
								bowBSB_2.GetItem().Returns(bowB_2);
							ISlottable wearASB_2 = MakeSubSB();
								wearASB_2.GetItem().Returns(wearA_2);
							ISlottable quiverASB_2 = MakeSubSB();
								quiverASB_2.GetItem().Returns(quiverA_2);
							ISlottable mWeaponASB_2 = MakeSubSB();
								mWeaponASB_2.GetItem().Returns(mWeaponA_2);
							ISlottable packASB_2 = MakeSubSB();
								packASB_2.GetItem().Returns(packA_2);
							ISlottable wearBSB_2 = MakeSubSB();
								wearBSB_2.GetItem().Returns(wearB_2);
							List<ISlottable> sbs_2 = new List<ISlottable>(new ISlottable[]{
								partsBSB_2, 
								bowASB_2, 
								shieldASB_2, 
								partsASB_2, 
								bowBSB_2, 
								wearASB_2, 
								quiverASB_2, 
								mWeaponASB_2, 
								packASB_2, 
								wearBSB_2
							});
							List<ISlottable> expected_2 = new List<ISlottable>(new ISlottable[]{
								partsBSB_2,
								bowASB_2,
								shieldASB_2,
								partsASB_2,
								bowBSB_2,
								wearASB_2,
								quiverASB_2,
								mWeaponASB_2,
								packASB_2,
								wearBSB_2
							});
							acqOrderSorter = new object[]{sbs_2, new SGAcquisitionOrderSorter(), expected_2};
							yield return acqOrderSorter;
					}
				}
			[Test]
			public void SetHierarchy_Always_SetsElements(){
				SlotGroup sg = MakeSG();
					GenericInventory inventory = new GenericInventory();
						BowInstance bow = MakeBowInstance(0);
						WearInstance wear = MakeWearInstance(0);
						ShieldInstance shield = MakeShieldInstance(0);
						MeleeWeaponInstance mWeapon = MakeMWeaponInstance(0);
						inventory.Add(bow);
						inventory.Add(wear);
						inventory.Add(shield);
						inventory.Add(mWeapon);
				sg.SetSBHandler(new SBHandler());
				ISlotsHolder slotsHolder = new SlotsHolder(sg);
					slotsHolder.SetInitSlotsCount(0);
				sg.SetSlotsHolder(slotsHolder);
				sg.SetCommandsRepo(new SGCommandsRepo(sg));
				sg.SetSBFactory(new SBFactory(MakeSubSSM()));
				ISorterHandler sorterHandler = new SorterHandler();
					sorterHandler.SetSorter(new SGItemIDSorter());
				sg.SetSorterHandler(sorterHandler);
				IFilterHandler filterHandler = new FilterHandler();
					filterHandler.SetFilter(new SGNullFilter());
				sg.SetFilterHandler(filterHandler);
				sg.SetSGTAHandler(new SGTransactionHandler(sg, MakeSubTAM()));
				sg.SetInventory(inventory);
				
				sg.SetHierarchy();

				List<ISlotSystemElement> actualEles = new List<ISlotSystemElement>(sg);
				Assert.That(actualEles.Count, Is.EqualTo(4));
			}
			[Test]
			public void SetHierarchy_Always_SetsSBsAndSlots(){
				SlotGroup sg = MakeSG();
					GenericInventory inventory = new GenericInventory();
						BowInstance bow = MakeBowInstance(0);
						WearInstance wear = MakeWearInstance(0);
						ShieldInstance shield = MakeShieldInstance(0);
						MeleeWeaponInstance mWeapon = MakeMWeaponInstance(0);
						inventory.Add(bow);
						inventory.Add(wear);
						inventory.Add(shield);
						inventory.Add(mWeapon);
				sg.SetSBHandler(new SBHandler());
				ISlotsHolder slotsHolder = new SlotsHolder(sg);
					slotsHolder.SetInitSlotsCount(0);
				sg.SetSlotsHolder(slotsHolder);
				sg.SetCommandsRepo(new SGCommandsRepo(sg));
				sg.SetSBFactory(new SBFactory(MakeSubSSM()));
				ISorterHandler sorterHandler = new SorterHandler();
					sorterHandler.SetSorter(new SGItemIDSorter());
				sg.SetSorterHandler(sorterHandler);
				IFilterHandler filterHandler = new FilterHandler();
					filterHandler.SetFilter(new SGNullFilter());
				sg.SetFilterHandler(filterHandler);
				sg.SetSGTAHandler(new SGTransactionHandler(sg, MakeSubTAM()));
				sg.SetInventory(inventory);

				sg.SetHierarchy();

				ISlottable bowSB = sg.GetSB(bow);
				ISlottable wearSB = sg.GetSB(wear);
				ISlottable shieldSB = sg.GetSB(shield);
				ISlottable mWeaponSB = sg.GetSB(mWeapon);
				IEnumerable<ISlottable> actualSBs = new ISlottable[]{bowSB, wearSB, shieldSB, mWeaponSB};
				Assert.That(actualSBs, Is.All.Not.Null);
				Assert.That(sg.GetSlots().Count, Is.EqualTo(4));
				int count = 0;
				foreach(ISlottable sb in sg)
					Assert.That(sb.GetSlotID(), Is.EqualTo(count ++));
			}
		}
	}
}
