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
					ISlotGroup mockSG = MakeSubSG();
				SGInitItemsCommand comm = new SGInitItemsCommand(mockSG);
						Inventory stubInv = Substitute.For<Inventory>();
						mockSG.isAutoSort.Returns(false);
						mockSG.inventory.Returns(stubInv);
						IEnumerable<InventoryItemInstance> items;
							BowInstance bow = MakeBowInstance(0);
							WearInstance wear = MakeWearInstance(0);
							items = new InventoryItemInstance[]{bow, wear};
							stubInv.GetEnumerator().Returns(items.GetEnumerator());
						List<InventoryItemInstance> list = new List<InventoryItemInstance>(new InventoryItemInstance[]{ bow, wear});
						mockSG.FilterItem(Arg.Is<List<InventoryItemInstance>>( x => x.Contains(bow) && x.Contains(wear))).Returns(list);
				
				comm.Execute();

				Received.InOrder(() => {
					mockSG.FilterItem(Arg.Is<List<InventoryItemInstance>>(x => x.Contains(bow) && x.Contains(wear)));
					mockSG.InitSlots(Arg.Is<List<InventoryItemInstance>>(x => x.Contains(bow) && x.Contains(wear)));
					mockSG.InitSBs(Arg.Is<List<InventoryItemInstance>>(x => x.Contains(bow) && x.Contains(wear)));
					mockSG.SyncSBsToSlots();
				});
				mockSG.DidNotReceive().InstantSort();
				}

			[Test]
			public void SGInitItemsCommand_Execute_SGIsAutoSort_CallsSGInstantSort(){
				ISlotGroup mockSG = MakeSubSG();
				SGInitItemsCommand comm = new SGInitItemsCommand(mockSG);
				mockSG.isAutoSort.Returns(true);
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
							stubSB_A.newSlotID.Returns(-1);
							stubSB_A.item.Returns(bow);
							stubSB_A.isToBeRemoved.Returns(true);
						ISlottable stubSB_B = MakeSubSB();
							WearInstance wear = MakeWearInstance(0);
							stubSB_B.slotID.Returns(-1);
							stubSB_B.item.Returns(wear);
							stubSB_B.isToBeAdded.Returns(true);
						ISlottable stubSB_C = MakeSubSB();
							ShieldInstance shield = MakeShieldInstance(0);
							stubSB_C.slotID.Returns(0);
							stubSB_C.item.Returns(shield);
						sbs = new ISlotSystemElement[]{stubSB_A, stubSB_B, stubSB_C};
				mockSG.GetEnumerator().Returns(sbs.GetEnumerator());

				comm.Execute();

				mockSG.Received().SyncEquipped(Arg.Is<InventoryItemInstance>(bow), false);
				mockSG.Received().SyncEquipped(Arg.Is<InventoryItemInstance>(wear), true);
				mockSG.DidNotReceive().SyncEquipped(Arg.Is<InventoryItemInstance>(shield), Arg.Any<bool>());
				}
		}
	}
}
