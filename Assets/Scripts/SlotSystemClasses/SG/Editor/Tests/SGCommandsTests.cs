using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using System;
using System.Collections;
using System.Collections.Generic;
namespace SlotSystemTests{
	namespace SGTests{
		[TestFixture]
		[Category("SG")]
		public class SGCommandsTests: SlotSystemTest {

			[Test]
			public void SGInitItemsCommand_Execute_IsNotAutoSort_CallsSGVariousButInstantSort(){
				SGInitItemsCommand comm;
					ISlotGroup mockSG = MakeSubSG();
						ISlotsHolder slotsHolder = Substitute.For<ISlotsHolder>();
						mockSG.GetSlotsHolder().Returns(slotsHolder);
						ISorterHandler sorterHandler = Substitute.For<ISorterHandler>();
							sorterHandler.IsAutoSort().Returns(false);
						IInventory stubInv = Substitute.For<IInventory>();
						mockSG.GetInventory().Returns(stubInv);
						IEnumerable<IInventoryItemInstance> items;
							BowInstance bow = MakeBowInstance(0);
							WearInstance wear = MakeWearInstance(0);
							items = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{bow, wear});
							stubInv.GetItems().Returns(items);
						List<IInventoryItemInstance> list = new List<IInventoryItemInstance>(new IInventoryItemInstance[]{ bow, wear});
						IFilterHandler filterHandler = Substitute.For<IFilterHandler>();
							filterHandler.FilteredItems(Arg.Is<List<IInventoryItemInstance>>( x => x.Contains(bow) && x.Contains(wear))).Returns(list);
						mockSG.GetFilterHandler().Returns(filterHandler);
						ISGTransactionHandler sgTAHandler = Substitute.For<ISGTransactionHandler>();
						mockSG.GetSGTAHandler().Returns(sgTAHandler);
					comm = new SGInitItemsCommand(mockSG);
				
				comm.Execute();

				Received.InOrder(() => {
					filterHandler.FilteredItems(Arg.Is<List<IInventoryItemInstance>>(x => x.Contains(bow) && x.Contains(wear)));
					slotsHolder.InitSlots(Arg.Is<List<IInventoryItemInstance>>(x => x.Contains(bow) && x.Contains(wear)));
					mockSG.InitSBs(Arg.Is<List<IInventoryItemInstance>>(x => x.Contains(bow) && x.Contains(wear)));
					sgTAHandler.SetSBsFromSlotsAndUpdateSlotIDs();
				});
				mockSG.DidNotReceive().InstantSort();
				}

			[Test]
			public void SGInitItemsCommand_Execute_SGIsAutoSort_CallsSGInstantSort(){
				ISlotGroup mockSG = MakeSubSG();
					IInventory stubInv = Substitute.For<IInventory>();
						stubInv.GetItems().Returns(new List<IInventoryItemInstance>());
					mockSG.GetInventory().Returns(stubInv);
					ISorterHandler sorterHandler = Substitute.For<ISorterHandler>();
						sorterHandler.IsAutoSort().Returns(true);
					mockSG.GetSorterHandler().Returns(sorterHandler);
				SGInitItemsCommand comm = new SGInitItemsCommand(mockSG);
				comm.Execute();

				mockSG.Received().InstantSort();
			}
			[Test]
			public void SGUpdateEquipAtExecutionCommand_Execute_VariousSBIDs_CallsSGAccordingly(){
				ISlotGroup mockSG = MakeSubSG();
				SGUpdateEquipAtExecutionCommand comm = new SGUpdateEquipAtExecutionCommand(mockSG);
					IEnumerable<ISlotSystemElement> sbs;
						ISlottable stubSB_A = MakeSubSB();
							BowInstance bow = MakeBowInstance(0);
							stubSB_A.GetNewSlotID().Returns(-1);
							stubSB_A.GetItem().Returns(bow);
							stubSB_A.IsToBeRemoved().Returns(true);
						ISlottable stubSB_B = MakeSubSB();
							WearInstance wear = MakeWearInstance(0);
							stubSB_B.GetSlotID().Returns(-1);
							stubSB_B.GetItem().Returns(wear);
							stubSB_B.IsToBeAdded().Returns(true);
						ISlottable stubSB_C = MakeSubSB();
							ShieldInstance shield = MakeShieldInstance(0);
							stubSB_C.GetSlotID().Returns(0);
							stubSB_C.GetItem().Returns(shield);
						sbs = new ISlotSystemElement[]{stubSB_A, stubSB_B, stubSB_C};
				mockSG.GetEnumerator().Returns(sbs.GetEnumerator());

				comm.Execute();

				mockSG.Received().SyncEquipped(Arg.Is<IInventoryItemInstance>(bow), false);
				mockSG.Received().SyncEquipped(Arg.Is<IInventoryItemInstance>(wear), true);
				mockSG.DidNotReceive().SyncEquipped(Arg.Is<IInventoryItemInstance>(shield), Arg.Any<bool>());
			}
		}
	}
}
