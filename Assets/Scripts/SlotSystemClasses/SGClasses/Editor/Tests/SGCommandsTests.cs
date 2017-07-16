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
		public class SGCommandsTests: AbsSlotSystemTest {

			[Test]
			public void SGInitItemsCommand_Execute_IsNotAutoSort_CallsSGVariousButInstantSort(){
				SGInitItemsCommand comm = new SGInitItemsCommand();
				ISlotGroup mockSG = MakeSubSG();
				Inventory stubInv = Substitute.For<Inventory>();
				mockSG.inventory.Returns(stubInv);
				mockSG.isAutoSort.Returns(false);
				BowInstance bow = MakeBowInstance(0);
				WearInstance wear = MakeWearInstance(0);
				IEnumerable<InventoryItemInstance> items = new InventoryItemInstance[]{bow, wear};
				stubInv.GetEnumerator().Returns(SlottableItemRator(items));
				List<SlottableItem> list = new List<SlottableItem>(new SlottableItem[]{ bow, wear});
				mockSG.FilterItem(Arg.Is<List<SlottableItem>>( x => x.Contains(bow) && x.Contains(wear))).Returns(list);
				
				comm.Execute(mockSG);

				Received.InOrder(() => {
					mockSG.FilterItem(Arg.Is<List<SlottableItem>>(x => x.Contains(bow) && x.Contains(wear)));
					mockSG.InitSlots(Arg.Is<List<SlottableItem>>(x => x.Contains(bow) && x.Contains(wear)));
					mockSG.InitSBs(Arg.Is<List<SlottableItem>>(x => x.Contains(bow) && x.Contains(wear)));
					mockSG.SyncSBsToSlots();
				});
				mockSG.DidNotReceive().InstantSort();
			}
				IEnumerator<SlottableItem> SlottableItemRator(IEnumerable<InventoryItemInstance> items){
					foreach(var item in items)
						yield return (SlottableItem)item;
				}
			[Test]
			public void SGInitItemsCommand_Execute_SGIsAutoSort_CallsSGInstantSort(){
				SGInitItemsCommand comm = new SGInitItemsCommand();
				ISlotGroup mockSG = MakeSubSG();
				mockSG.isAutoSort.Returns(true);
				comm.Execute(mockSG);

				mockSG.Received().InstantSort();
			}
			[Test]
			public void SGUpdateEquipAtExecutionCommand_Execute_VariousSBIDs_CallsSGAccordingly(){
				SGUpdateEquipAtExecutionCommand comm = new SGUpdateEquipAtExecutionCommand();
				ISlotGroup mockSG = MakeSubSG();
				ISlottable stubSB_A = MakeSubSB();
					BowInstance bow = MakeBowInstance(0);
					stubSB_A.newSlotID.Returns(-1);
					stubSB_A.itemInst.Returns(bow);
				ISlottable stubSB_B = MakeSubSB();
					WearInstance wear = MakeWearInstance(0);
					stubSB_B.slotID.Returns(-1);
					stubSB_B.itemInst.Returns(wear);
				ISlottable stubSB_C = MakeSubSB();
					ShieldInstance shield = MakeShieldInstance(0);
					stubSB_C.slotID.Returns(0);
					stubSB_C.itemInst.Returns(shield);
				List<ISlottable> sbs = new List<ISlottable>(new ISlottable[]{stubSB_A, stubSB_B, stubSB_C});
				mockSG.GetEnumerator().Returns(SSERator(sbs));

				comm.Execute(mockSG);

				mockSG.Received().SyncEquipped(Arg.Is<InventoryItemInstance>(bow), false);
				mockSG.Received().SyncEquipped(Arg.Is<InventoryItemInstance>(wear), true);
				mockSG.DidNotReceive().SyncEquipped(Arg.Is<InventoryItemInstance>(shield), Arg.Any<bool>());
			}
				IEnumerator<ISlotSystemElement> SSERator(IEnumerable<ISlottable> sbs){
					foreach(var sb in sbs) yield return sb;
				}
		}
	}
}
