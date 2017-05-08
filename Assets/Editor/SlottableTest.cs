using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections.Generic;
public class SlottableTest {


	
	/*	fields
	*/
		PointerEventDataMock eventData = new PointerEventDataMock();
		GameObject sgmGO;
		SlotGroupManager sgm;
		GameObject sgpAllGO;
		SlotGroup sgpAll;
		GameObject sgBowGO;
		SlotGroup sgBow;
		GameObject sgWearGO;
		SlotGroup sgWear;
		GameObject sgpPartsGO;
		SlotGroup sgpParts;
		Slottable defBowBSB_p;
		Slottable defBowASB_p;
		Slottable crfBowASB_p;
		Slottable defWearBSB_p;
		Slottable defWearASB_p;
		Slottable crfWearASB_p;
		Slottable defPartsSB_p;
		Slottable crfPartsSB_p;
		Slottable defBowASB_e;
		Slottable defWearASB_e;
		Slottable defPartsSB_p2;
		Slottable crfPartsSB_p2;


	/*
	*/
	[SetUp]
	public void Setup(){	
		/*	SGM
		*/
			sgmGO = new GameObject("SlotGroupManager");
			sgm = sgmGO.AddComponent<SlotGroupManager>();
			sgm.SetupCommands();
			// sgm.SetupProcesses();
		/*	SGs
		*/
			/*	sgpAll
			*/
				sgpAllGO = new GameObject("PoolSlotGroup");
				sgpAll = sgpAllGO.AddComponent<SlotGroup>();
				sgpAll.Filter = new SGNullFilter();
				sgpAll.Sorter = new SGItemIndexSorter();
				sgpAll.UpdateEquipStatusCommand = new UpdateEquipStatusForPoolCommmand();
				PoolInventory inventory = new PoolInventory();
				sgpAll.SetInventory(inventory);
				sgpAll.IsShrinkable = true;
				sgpAll.IsExpandable = true;
			/*	sgBow
			*/
				sgBowGO = new GameObject("BowSlotGroup");
				sgBow = sgBowGO.AddComponent<SlotGroup>();
				sgBow.Filter = new SGBowFilter();
				sgBow.Sorter = new SGItemIndexSorter();
				sgBow.UpdateEquipStatusCommand = new UpdateEquipStatusForEquipSGCommand();
				EquipmentSetInventory equipInventory = new EquipmentSetInventory();
				sgBow.SetInventory(equipInventory);
				sgBow.IsShrinkable = false;
				sgBow.IsExpandable = false;

				Slot bowSlot = new Slot();
				bowSlot.Position = Vector2.zero;
				sgBow.Slots.Add(bowSlot);

			/*	sgWear
			*/
				sgWearGO = new GameObject("WearSlotGroup");
				sgWear = sgWearGO.AddComponent<SlotGroup>();
				sgWear.Filter = new SGWearFilter();
				sgWear.Sorter = new SGItemIndexSorter();
				sgWear.UpdateEquipStatusCommand = new UpdateEquipStatusForEquipSGCommand();
				sgWear.SetInventory(equipInventory);
				sgWear.IsShrinkable = false;
				sgWear.IsExpandable = false;

				Slot wearSlot = new Slot();
				wearSlot.Position = Vector2.zero;
				sgWear.Slots.Add(wearSlot);

			/*	sgpParts
			*/	
				sgpPartsGO = new GameObject("PartsSlotGroupPool");
				sgpParts = sgpPartsGO.AddComponent<SlotGroup>();
				sgpParts.Filter = new SGPartsFilter();
				sgpParts.Sorter = new SGItemIndexSorter();
				sgpParts.UpdateEquipStatusCommand = new UpdateEquipStatusForPoolCommmand();
				sgpParts.SetInventory(inventory);
				sgpParts.IsShrinkable = true;
				sgpParts.IsExpandable = true;

				
				
		/*	Items
		*/
			/*	bows
			*/
				BowMock defBow = new BowMock();
				defBow.ItemID = 1;
				BowMock crfBow = new BowMock();
				crfBow.ItemID = 2;
				
				BowInstanceMock defBowA = new BowInstanceMock();//	equipped
				defBowA.Item = defBow;
				sgpAll.Inventory.Add(defBowA);
				sgBow.Inventory.Add(defBowA);
				BowInstanceMock defBowB = new BowInstanceMock();
				defBowB.Item = defBow;
				sgpAll.Inventory.Add(defBowB);
				BowInstanceMock crfBowA = new BowInstanceMock();
				crfBowA.Item = crfBow;
				sgpAll.Inventory.Add(crfBowA);
			/*	wears
			*/
					WearMock defWear = new WearMock();
					defWear.ItemID = 101;
					WearMock crfWear = new WearMock();
					crfWear.ItemID = 102;
					
					WearInstanceMock defWearA = new WearInstanceMock();//	equipped
					defWearA.Item = defWear;
					sgpAll.Inventory.Add(defWearA);
					sgWear.Inventory.Add(defWearA);
					WearInstanceMock defWearB = new WearInstanceMock();
					defWearB.Item = defWear;
					sgpAll.Inventory.Add(defWearB);
					WearInstanceMock crfWearA = new WearInstanceMock();
					crfWearA.Item = crfWear;
					sgpAll.Inventory.Add(crfWearA);
			/*	parts
			*/ 
				PartsMock defParts = new PartsMock();
				defParts.ItemID = 601;
				defParts.IsStackable = true;
				PartsMock crfParts = new PartsMock();
				crfParts.ItemID = 602;
				crfParts.IsStackable = true;
				
				PartsInstanceMock defPartsA = new PartsInstanceMock();
				defPartsA.Item = defParts;
				defPartsA.Quantity = 10;
				sgpAll.Inventory.Add(defPartsA);
				PartsInstanceMock defPartsB = new PartsInstanceMock();
				defPartsB.Item = defParts;
				defPartsB.Quantity = 5;
				sgpAll.Inventory.Add(defPartsB);
				PartsInstanceMock crfPartsA = new PartsInstanceMock();
				crfPartsA.Item = crfParts;
				crfPartsA.Quantity = 3;
				sgpAll.Inventory.Add(crfPartsA);

				Assert.That(defPartsA, Is.EqualTo(defPartsB));
				Assert.That(object.ReferenceEquals(defPartsA, defPartsB), Is.False);
		/**/
		EquipmentSet equipSetA = new EquipmentSet(sgBow, sgWear);
		SlotSystemBundle equipBundle = new SlotSystemBundle();
		equipBundle.Elements.Add(equipSetA);
		equipBundle.SetFocusedBundleElement(equipSetA);
		SlotSystemBundle poolBundle = new SlotSystemBundle();
		poolBundle.Elements.Add(sgpAll);
		poolBundle.Elements.Add(sgpParts);
		poolBundle.SetFocusedBundleElement(sgpAll);
		InventoryManagerPage invManPage = new InventoryManagerPage(poolBundle, equipBundle);
	
		sgm.SetRootPage(invManPage);	
			/*	Assert Setup
				Preinitialized validation
				validate all the required fields are filled within the inspector window or at the time of declaration
			*/
			Assert.That(sgm.UpdateTransactionCommand, Is.Not.Null);
			Assert.That(sgm.PostPickFilterCommand, Is.Not.Null);
			// Assert.That(sgm.PrePickFilterCommand, Is.Not.Null);
			// Assert.That(sgm.ProbingStateProcess, Is.Not.Null);

			Assert.That(sgm.Transaction, Is.Null);

			Assert.That(sgm.PrevState, Is.EqualTo(SlotGroupManager.DeactivatedState));
			Assert.That(sgm.CurState, Is.EqualTo(SlotGroupManager.DeactivatedState));

			Assert.That(sgm.SelectedSB, Is.Null);
			Assert.That(sgm.SelectedSG, Is.Null);
			Assert.That(sgm.PickedSB, Is.Null);

			Assert.That(sgm.RootPage, Is.TypeOf(typeof(InventoryManagerPage)));
			AE(sgm.RootPage.SGM, sgm);
			Assert.That(sgm.RootPage.Elements.Count, Is.EqualTo(2));
			Assert.That(sgm.RootPage.ContainsElement(poolBundle));
			AE(poolBundle.SGM, sgm);
			Assert.That(sgm.RootPage.ContainsElement(equipBundle));
			AE(equipBundle.SGM, sgm);

			Assert.That(poolBundle.Elements.Count, Is.EqualTo(2));
			AB(poolBundle.ContainsElement(sgpAll), true);
			AE(sgpAll.SGM, sgm);
			AB(poolBundle.ContainsElement(sgpParts), true);
			AE(sgpParts.SGM, sgm);
			Assert.That(poolBundle.GetFocusedBundleElement(), Is.EqualTo(sgpAll));

			Assert.That(equipBundle.Elements.Count, Is.EqualTo(1));
			Assert.That(equipBundle.GetFocusedBundleElement(), Is.EqualTo(equipSetA));
			AE(equipSetA.SGM, sgm);

			Assert.That(equipSetA.Elements.Count, Is.EqualTo(2));
			Assert.That(equipSetA.ContainsElement(sgBow), Is.True);
			AE(sgBow.SGM, sgm);
			Assert.That(equipSetA.ContainsElement(sgWear), Is.True);
			AE(sgWear.SGM, sgm);
		

		sgm.Initialize();
			/*	Assert Initialiation
				when the scene is loaded, but not yet focused
			*/
			defBowASB_p = sgpAll.GetSlottable(defBowA);	
			defBowBSB_p = sgpAll.GetSlottable(defBowB);
			crfBowASB_p = sgpAll.GetSlottable(crfBowA);
			defWearASB_p = sgpAll.GetSlottable(defWearA);
			defWearBSB_p = sgpAll.GetSlottable(defWearB);
			crfWearASB_p = sgpAll.GetSlottable(crfWearA);
			defPartsSB_p = sgpAll.GetSlottable(defPartsA);
			crfPartsSB_p = sgpAll.GetSlottable(crfPartsA);
			defBowASB_e = sgBow.GetSlottable(defBowA);
			defWearASB_e = sgWear.GetSlottable(defWearA);
			defPartsSB_p2 = sgpParts.GetSlottable(defPartsA);
			crfPartsSB_p2 = sgpParts.GetSlottable(crfPartsA);

			Assert.That(sgm.CurState, Is.EqualTo(SlotGroupManager.DeactivatedState));
			/*	sgpAll
			*/
			Assert.That(sgpAll.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgpAll.FilteredItems.Count, Is.EqualTo(8));
			Assert.That(sgpAll.FilteredItems, Is.Ordered);
			Assert.That(sgpAll.Slots.Count, Is.EqualTo(8));
			foreach(Slot slot in sgpAll.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
				
				AE(sgm.GetSlotGroup(defBowASB_p), sgpAll);
				AE(sgm.GetSlotGroup(defBowBSB_p), sgpAll);
				AE(sgm.GetSlotGroup(crfBowASB_p), sgpAll);
				AE(sgm.GetSlotGroup(defWearASB_p), sgpAll);
				AE(sgm.GetSlotGroup(defWearBSB_p), sgpAll);
				AE(sgm.GetSlotGroup(crfWearASB_p), sgpAll);
				AE(sgm.GetSlotGroup(defPartsSB_p), sgpAll);
				AE(sgm.GetSlotGroup(crfPartsSB_p), sgpAll);
				
			/*	sgpParts
			*/
			Assert.That(sgpParts.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgpParts.FilteredItems.Count, Is.EqualTo(2));
			Assert.That(sgpParts.FilteredItems, Is.Ordered);
			Assert.That(sgpParts.Slots.Count, Is.EqualTo(2));
			foreach(Slot slot in sgpParts.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
				AE(sgm.GetSlotGroup(defPartsSB_p2), sgpParts);
				AE(sgm.GetSlotGroup(crfPartsSB_p2), sgpParts);
				
			/*	sgBow
			*/
			Assert.That(sgBow.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgBow.FilteredItems.Count, Is.EqualTo(1));
			Assert.That(sgBow.Slots.Count, Is.EqualTo(1));
			foreach(Slot slot in sgBow.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
			AE(sgm.GetSlotGroup(defBowASB_e), sgBow);
			InventoryItemInstanceMock defBowA_e_Inst = (InventoryItemInstanceMock)defBowASB_e.Item;
			AB(defBowA_e_Inst.IsEquipped, true);
			/*	sgWear
			*/
			Assert.That(sgWear.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgWear.FilteredItems.Count, Is.EqualTo(1));
			Assert.That(sgWear.Slots.Count, Is.EqualTo(1));
			foreach(Slot slot in sgWear.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
			AE(sgm.GetSlotGroup(defWearASB_e), sgWear);
			InventoryItemInstanceMock defWearA_e_Inst = (InventoryItemInstanceMock)defWearASB_e.Item;
			AB(defWearA_e_Inst.IsEquipped, true);

		sgm.Focus();
			/*	when the widget gets focus
			*/
			AssertSGMFocused();
			// AssertInstGICalled();
		sgm.Defocus();
			AssertSGMDefocus();
		sgm.Focus();
			AssertSGMFocused();
	}
	[Test]
	public void Test(){
		TestHover();
	}
	
	
	public void TestPostPickFilter(){
		AB(sgpAll.AutoSort, true);
		bool picked;
		bool reverted;
		PickUp(defBowBSB_p, out picked);
			ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
			ASSB(defBowBSB_p, Slottable.PickedUpAndSelectedState);
			ASSB(crfBowASB_p, Slottable.DefocusedState);
			ASSB(defWearASB_p, Slottable.EquippedAndDefocusedState);
			ASSB(defWearBSB_p, Slottable.DefocusedState);
			ASSB(crfWearASB_p, Slottable.DefocusedState);
			ASSB(defPartsSB_p, Slottable.DefocusedState);
			ASSB(crfPartsSB_p, Slottable.DefocusedState);
			ASSB(defBowASB_e, Slottable.EquippedAndDeselectedState);
			AB(sgm.RootPage.EquipBundle.GetFocusedBundleElement().ContainsElement(sgWear), true);
			AB(sgWear.AcceptsFilter(defBowBSB_p), false);
			ASSB(defWearASB_e, Slottable.EquippedAndDefocusedState);
			ASSB(defPartsSB_p2, Slottable.DefocusedState);
			ASSB(crfPartsSB_p2, Slottable.DefocusedState);
		Revert(defBowBSB_p, out reverted);

		AssertSGMFocused();
		sgpAll.AutoSort = false;
		PickUp(defBowBSB_p, out picked);
			ASSB(defBowASB_p, Slottable.EquippedAndDeselectedState);
			ASSB(defBowBSB_p, Slottable.PickedUpAndSelectedState);
			ASSB(crfBowASB_p, Slottable.FocusedState);
			ASSB(defWearASB_p, Slottable.EquippedAndDeselectedState);
			ASSB(defWearBSB_p, Slottable.FocusedState);
			ASSB(crfWearASB_p, Slottable.FocusedState);
			ASSB(defPartsSB_p, Slottable.FocusedState);
			ASSB(crfPartsSB_p, Slottable.FocusedState);
			ASSB(defBowASB_e, Slottable.EquippedAndDeselectedState);
			ASSB(defWearASB_e, Slottable.EquippedAndDefocusedState);
			ASSB(defPartsSB_p2, Slottable.DefocusedState);
			ASSB(crfPartsSB_p2, Slottable.DefocusedState);
		Revert(defBowBSB_p, out reverted);
		AssertSGMFocused();
		
			ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
			ASSB(defBowBSB_p, Slottable.FocusedState);
			ASSB(crfBowASB_p, Slottable.FocusedState);
			ASSB(defWearASB_p, Slottable.EquippedAndDefocusedState);
			ASSB(defWearBSB_p, Slottable.FocusedState);
			ASSB(crfWearASB_p, Slottable.FocusedState);
			ASSB(defPartsSB_p, Slottable.DefocusedState);
			ASSB(crfPartsSB_p, Slottable.DefocusedState);
			ASSB(defBowASB_e, Slottable.EquippedAndDeselectedState);
			ASSB(defWearASB_e, Slottable.EquippedAndDeselectedState);
			ASSB(defPartsSB_p2, Slottable.DefocusedState);
			ASSB(crfPartsSB_p2, Slottable.DefocusedState);

		PickUp(defBowASB_e, out picked);
			ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
			ASSB(defBowBSB_p, Slottable.FocusedState);
			ASSB(crfBowASB_p, Slottable.FocusedState);
			ASSB(defWearASB_p, Slottable.EquippedAndDefocusedState);
			ASSB(defWearBSB_p, Slottable.DefocusedState);
			ASSB(crfWearASB_p, Slottable.DefocusedState);
			ASSB(defPartsSB_p, Slottable.DefocusedState);
			ASSB(crfPartsSB_p, Slottable.DefocusedState);
			ASSB(defBowASB_e, Slottable.PickedUpAndSelectedState);
			ASSB(defWearASB_e, Slottable.EquippedAndDefocusedState);
			ASSB(defPartsSB_p2, Slottable.DefocusedState);
			ASSB(crfPartsSB_p2, Slottable.DefocusedState);
			
		Revert(defBowASB_e, out reverted);
		AssertSGMFocused();


	}
	
	/*	Test hover
	*/
		bool picked;
		Slottable selectedSB;
		SlotGroup selectedSG;
		SlotSystemTransaction transaction;
		public void TestHover(){	
			
			SlotGroup origSG = sgm.GetSlotGroup(defBowBSB_p);
			Slottable targetSB;

			// /*	test reorder
				// */
				// AB(origSG.AutoSort, true);
				// origSG.AutoSort = false;

				// TestSimHover(defBowBSB_p, defWearBSB_p, origSG, out picked, out selectedSBChanged, out selectedSGChanged, out transaction);
				// 	AB(picked, true);
				// 	AB(selectedSBChanged, true);
				// 	AB(selectedSGChanged, false);
				// 	AE(transaction.GetType(), typeof(ReorderTransaction));
				
				// TestSimHover(defBowBSB_p, crfBowASB_p, origSG, out picked, out selectedSBChanged, out selectedSGChanged, out transaction);
				// 	AB(picked, true);
				// 	AB(selectedSBChanged, true);
				// 	AB(selectedSGChanged, false);
				// 	AE(transaction.GetType(), typeof(ReorderTransaction));
			// /*	test swap
				// */
				// TestSimHover(defBowBSB_p, defBowASB_e, sgBow, out picked, out selectedSBChanged, out selectedSGChanged, out transaction);
				// 	AB(picked, true);
				// 	AB(selectedSBChanged, true);
				// 	AB(selectedSGChanged, true);
				// 	AE(transaction.GetType(), typeof(SwapTransaction));
				
				// TestSimHover(defWearBSB_p, defWearASB_e, sgWear, out picked, out selectedSBChanged, out selectedSGChanged, out transaction);
				// 	AB(picked, true);
				// 	AB(selectedSBChanged, true);
				// 	AB(selectedSGChanged, true);
				// 	AE(transaction.GetType(), typeof(SwapTransaction));
			/**/

			AB(origSG.AutoSort, true);
			origSG.AutoSort = false;

			TestSimHoverOnAllSB();
			// TestSimHoverOnAllSB(defBowASB_p);
			// TestSimHoverOnAllSB(defBowBSB_p);
			// TestSimHoverOnAllSB(crfBowASB_p);
			// TestSimHoverOnAllSB(defWearASB_p);
			// TestSimHoverOnAllSB(defWearBSB_p);
			// TestSimHoverOnAllSB(crfWearASB_p);
			// TestSimHoverOnAllSB(defBowASB_e);
			// TestSimHoverOnAllSB(defWearASB_e);
			// TestSimHoverOnAllSB(defBowASB_p);
			
				
			// TestHoverDefBowASBE();
			// TestHoverDefBowASBP();
			// TestHoverDefBowBSBP();
			// TestHoverDefWearASBE();
			//TestHoverPartsInSGPParts();
				
		}
		/*	spot tests hover
		*/
			public void TestHoverDefBowASBE(){
				SlotGroup origSG = sgBow;
				/*	defBowASB_e
				*/
					/*	AutoSort false
					*/
						Slottable targetSB = defBowASB_p;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defBowBSB_p;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, defBowBSB_p);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(SwapTransaction));

						targetSB = crfBowASB_p;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, crfBowASB_p);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(SwapTransaction));

						targetSB = defWearASB_p;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defWearBSB_p;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = crfWearASB_p;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));

						targetSB = defPartsSB_p2;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = crfPartsSB_p2;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defBowASB_e;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, defBowASB_e);
							AE(selectedSG, sgBow);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defWearASB_e;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						sgm.SetFocusedPoolSG(sgpParts);
						
							targetSB = defPartsSB_p2;
							TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
								AB(picked, true);
								AE(selectedSB, null);
								AE(selectedSG, sgpParts);
								AE(transaction.GetType(), typeof(RevertTransaction));
							
							targetSB = crfPartsSB_p2;
							TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
								AB(picked, true);
								AE(selectedSB, null);
								AE(selectedSG, sgpParts);
								AE(transaction.GetType(), typeof(RevertTransaction));
					/*	AutoSort true
					*/
						sgm.SetFocusedPoolSG(sgpAll);
						origSG.AutoSort = true;
						targetSB = defBowASB_p;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defBowBSB_p;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, defBowBSB_p);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(SwapTransaction));

						targetSB = crfBowASB_p;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, crfBowASB_p);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(SwapTransaction));

						targetSB = defWearASB_p;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defWearBSB_p;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = crfWearASB_p;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));

						targetSB = defPartsSB_p2;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = crfPartsSB_p2;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defBowASB_e;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, defBowASB_e);
							AE(selectedSG, sgBow);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defWearASB_e;
						TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						sgm.SetFocusedPoolSG(sgpParts);
						
							targetSB = defPartsSB_p2;
							TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
								AB(picked, true);
								AE(selectedSB, null);
								AE(selectedSG, sgpParts);
								AE(transaction.GetType(), typeof(RevertTransaction));
							
							targetSB = crfPartsSB_p2;
							TestSimHover(defBowASB_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
								AB(picked, true);
								AE(selectedSB, null);
								AE(selectedSG, sgpParts);
								AE(transaction.GetType(), typeof(RevertTransaction));
			}
			public void TestHoverDefBowASBP(){
				Slottable targetSB;
				targetSB = defBowASB_p;
						TestSimHover(defBowASB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(false, null, null, true);
					targetSB = defBowBSB_p;
						TestSimHover(defBowASB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(false, null, null, true);
					targetSB = defBowASB_e;
						TestSimHover(defBowASB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(false, null, null, true);
			}
			public void TestHoverDefBowBSBP(){
				sgpAll.AutoSort = true;
				sgm.SetFocusedPoolSG(sgpAll);
				AssertSGMFocused();

				Slottable targetSB;
				targetSB = defBowASB_p;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				targetSB = defBowBSB_p;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, defBowBSB_p, sgpAll, false);
				targetSB = crfBowASB_p;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				targetSB = defBowASB_e;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<SwapTransaction>(true, defBowASB_e, sgBow, false);
				targetSB = defWearASB_e;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);

				sgpAll.AutoSort = false;

				targetSB = defBowASB_p;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, defBowASB_p, sgpAll, false);
				targetSB = crfBowASB_p;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, crfBowASB_p, sgpAll, false);
				targetSB = defBowBSB_p;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, defBowBSB_p, sgpAll, false);
				targetSB = defWearASB_p;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, defWearASB_p, sgpAll, false);
				targetSB = defWearBSB_p;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, defWearBSB_p, sgpAll, false);
				targetSB = crfWearASB_p;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, crfWearASB_p, sgpAll, false);
				targetSB = defPartsSB_p;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, defPartsSB_p, sgpAll, false);
				targetSB = crfPartsSB_p;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, crfPartsSB_p, sgpAll, false);
				targetSB = defBowASB_e;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<SwapTransaction>(true, defBowASB_e, sgBow, false);
				targetSB = defWearASB_e;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				targetSB = defPartsSB_p2;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				targetSB = crfPartsSB_p2;
					TestSimHover(defBowBSB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);

			}
			public void TestHoverDefWearASBE(){
				sgm.SetFocusedPoolSG(sgpAll);
				AssertSGMFocused();
				Slottable target;

				target = defBowASB_p;
					TestSimHover(defWearASB_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = defBowBSB_p;
					TestSimHover(defWearASB_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = crfBowASB_p;
					TestSimHover(defWearASB_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = defWearASB_p;
					TestSimHover(defWearASB_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = defWearBSB_p;
					TestSimHover(defWearASB_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<SwapTransaction>(true, defWearBSB_p, sgpAll, false);
				target = crfWearASB_p;
					TestSimHover(defWearASB_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<SwapTransaction>(true, crfWearASB_p, sgpAll, false);
				target = defPartsSB_p;
					TestSimHover(defWearASB_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = crfPartsSB_p;
					TestSimHover(defWearASB_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				
				target = defBowASB_e;
					TestSimHover(defWearASB_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				target = defWearASB_e;
					TestSimHover(defWearASB_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, defWearASB_e, sgWear, false);
				target = defPartsSB_p2;
					TestSimHover(defWearASB_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				
				sgm.SetFocusedPoolSG(sgpParts);
				target = defPartsSB_p2;
					TestSimHover(defWearASB_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpParts, false);
				sgm.SetFocusedPoolSG(sgpAll);


			}
			public void TestHoverPartsInSGPParts(){
				sgm.SetFocusedPoolSG(sgpParts);
				AssertSGMFocused();
				Slottable target;

				target = defPartsSB_p2;
					TestSimHover(defPartsSB_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, target, sgpParts, false);
				target = crfPartsSB_p2;
					TestSimHover(defPartsSB_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpParts, false);
				
				sgpParts.AutoSort = false;
				target = defPartsSB_p2;
					TestSimHover(defPartsSB_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, target, sgpParts, false);
				target = crfPartsSB_p2;
					TestSimHover(defPartsSB_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, target, sgpParts, false);
				sgpParts.AutoSort = true;
				
				target = defPartsSB_p;
					TestSimHover(defPartsSB_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				target = defBowASB_e;
					TestSimHover(defPartsSB_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				target = defWearASB_e;
					TestSimHover(defPartsSB_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
			}
		public void AssertSimHover<T>(bool picked, Slottable sb, SlotGroup sg, bool TSNull){
			if(picked) AB(picked, true); else AB(picked, false);
			if(sb != null) AE(selectedSB, sb); else AB(selectedSB == null, true);
			if(sg != null) AE(selectedSG, sg); else AB(selectedSG == null, true);
			if(!TSNull) AE(transaction.GetType(), typeof(T)); else AB(transaction == null, true);
		}
		public void TestSimHoverOnAllSB(){
			foreach(Slottable sb in SlottableList()){
				foreach(Slottable hoveredSB in SlottableList()){
					SlotGroup destSG = sgm.GetSlotGroup(hoveredSB);
					bool picked;
					Slottable selectedSB;
					SlotGroup selectedSG;
					SlotSystemTransaction transaction;
					TestSimHover(sb, hoveredSB, destSG, out picked, out selectedSB, out selectedSG, out transaction);
				}
			}
		}
		public void TestSimHover(Slottable pickedSB, Slottable hoveredSB, SlotGroup hoveredSG, out bool picked, out Slottable selectedSB, out SlotGroup selectedSG, out SlotSystemTransaction transaction){
			SlotGroup origSG = sgm.GetSlotGroup(pickedSB);// == SelectedSG
			
			if(pickedSB.CurState == Slottable.FocusedState || pickedSB.CurState == Slottable.EquippedAndDeselectedState){

				PickUp(pickedSB, out picked);//picked = true;
				ASSB(pickedSB, Slottable.PickedUpAndSelectedState);
				AssertPostPickFilter(pickedSB);
				
				if(hoveredSB != null){
					SlotGroup destSG = sgm.GetSlotGroup(hoveredSB);
					
					if(hoveredSB == pickedSB){
						
						selectedSB = pickedSB;
						selectedSG = origSG;
						sgm.SimSBHover(hoveredSB, eventData);
						ASSB(hoveredSB, Slottable.PickedUpAndSelectedState);
						AE(hoveredSB.CurProcess.GetType(), typeof(PickedUpAndSelectedProcess));
						AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
						transaction = sgm.Transaction;

					}else{//	hover is not null nor same as pickedSb
						if(origSG == destSG){
							selectedSG = origSG;
							// ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
							if(hoveredSB.CurState == Slottable.FocusedState){
								selectedSB = hoveredSB;
								sgm.SimSBHover(hoveredSB, eventData);
								sgm.SimSGHover(hoveredSG, eventData);
								ASSB(hoveredSB, Slottable.SelectedState);
								AE(hoveredSB.CurProcess.GetType(), typeof(GradualHighlightProcess));
								ASSB(pickedSB, Slottable.PickedUpAndDeselectedState);
								AE(pickedSB.CurProcess.GetType(), typeof(GradualDehighlightProcess));

								AE(sgm.Transaction.GetType(), typeof(ReorderTransaction));
								transaction = sgm.Transaction;

								sgm.SimSBHover(pickedSB, eventData);

							}else if(hoveredSB.CurState == Slottable.EquippedAndDeselectedState){
								selectedSB = hoveredSB;
								sgm.SimSBHover(hoveredSB, eventData);
								sgm.SimSGHover(hoveredSG, eventData);
								ASSB(hoveredSB, Slottable.EquippedAndSelectedState);
								AE(hoveredSB.CurProcess.GetType(), typeof(EquipGradualHighlightProcess));
								ASSB(pickedSB, Slottable.PickedUpAndDeselectedState);
								AE(pickedSB.CurProcess.GetType(), typeof(GradualDehighlightProcess));

								AE(sgm.Transaction.GetType(), typeof(ReorderTransaction));
								transaction = sgm.Transaction;

								sgm.SimSBHover(pickedSB, eventData);

							}else{
								selectedSB = null;
								SlottableState preState = hoveredSB.CurState;
								sgm.SimSBHover(hoveredSB, eventData);
								sgm.SimSGHover(hoveredSG, eventData);
								ASSB(hoveredSB, preState);
								ASSB(pickedSB, Slottable.PickedUpAndDeselectedState);
								AE(pickedSB.CurProcess.GetType(), typeof(GradualDehighlightProcess));

								AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
								transaction = sgm.Transaction;

								sgm.SimSBHover(pickedSB, eventData);
							}		
						}else{
							/*	destSG != null, != origSG
								selectedSG == null || == destSG || != origSG
								hoveredSB != null, != pickdSB
								fill, swap, stack
								not revert
							*/
							AE(sgm.SelectedSB, pickedSB);
							AE(sgm.SelectedSG, origSG);
							// ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
							if(hoveredSB.CurState == Slottable.FocusedState || hoveredSB.CurState == Slottable.EquippedAndDeselectedState || hoveredSB.CurState == Slottable.PickedUpAndDeselectedState){
								sgm.SimSBHover(hoveredSB, eventData);
									AE(sgm.SelectedSB, hoveredSB);
									selectedSB = hoveredSB;
									if(hoveredSB.CurState != Slottable.PickedUpAndSelectedState){
										if(hoveredSB.IsEquipped)
											ASSB(hoveredSB, Slottable.EquippedAndSelectedState);
										else
											ASSB(hoveredSB, Slottable.SelectedState);
									}else
										ASSB(hoveredSB, Slottable.PickedUpAndSelectedState);
								
								if(hoveredSB.Item.IsStackable)
									AE(sgm.Transaction.GetType(), typeof(StackTransaction));
								else
									AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
								transaction = sgm.Transaction;
							}else{// hoveredSB not in a state to be hovered
								SlottableState preState = hoveredSB.CurState;
								selectedSB = null;
								sgm.SimSBHover(hoveredSB, eventData);
									AE(sgm.SelectedSB, null);
									ASSB(hoveredSB, preState);
								
								AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
								transaction = sgm.Transaction;
							}


							ASSB(pickedSB, Slottable.PickedUpAndDeselectedState);
							/*	selected, eqSelected, pickSele, 
							*/
							if(hoveredSG.CurState == SlotGroup.FocusedState){
								selectedSG = hoveredSG;
								sgm.SimSGHover(hoveredSG, eventData);
									AE(sgm.SelectedSG, hoveredSG);
									ASSG(hoveredSG, SlotGroup.SelectedState);
							}else{
								selectedSG = null;
								SlotGroupState preState = hoveredSG.CurState;
								sgm.SimSGHover(hoveredSG, eventData);
									AE(sgm.SelectedSG, null);
									ASSG(hoveredSG, preState);
							}

							sgm.SimSBHover(pickedSB, eventData);
							sgm.SimSGHover(origSG, eventData);
						}
					}
				}
				else{//	hoveredSB == null
					selectedSB = null;
					transaction = new RevertTransaction(pickedSB);
					if(hoveredSG == null){
						selectedSG = null;
						/*	revert
						*/
					}else{
						selectedSG = hoveredSG;
						if(hoveredSG == origSG){// same sg, no selectable sb under cursor
							

							/*	revert
							*/
						}else{//	hoveredSG not null nor the same as orig

							
						}
					}
					sgm.SimSBHover(pickedSB, eventData);
					sgm.SimSGHover(origSG, eventData);
				}
				bool reverted;
				Revert(pickedSB, out reverted);
				AB(reverted, true);
				AssertSGMFocused();
				
			}else{//	pickedSB is not in a state to be picked up
				picked = false;
				selectedSB = null;
				selectedSG = null;
				transaction = null;

				if(pickedSB.CurState == Slottable.DefocusedState || pickedSB.CurState == Slottable.EquippedAndDefocusedState){
					pickedSB.OnPointerDownMock(eventData);
					ASSB(pickedSB, Slottable.WaitForPointerUpState);
					pickedSB.OnPointerUpMock(eventData);
					if(pickedSB.IsEquipped)
						ASSB(pickedSB, Slottable.EquippedAndDefocusedState);
					else
						ASSB(pickedSB, Slottable.DefocusedState);
				}else{
					/*	not testable
					*/
					SlottableState preState = pickedSB.CurState;
					pickedSB.OnPointerDownMock(eventData);
					ASSB(pickedSB, preState);
				}
				AssertSGMFocused();
			}
		}
		/*	hovering
		*/
			public void TestPickupAndRevertAll(){
				int hoveredCount = 0;
				TestHoverAll(ref hoveredCount);
				AE(hoveredCount, 6);

				hoveredCount = 0;
				sgm.SetFocusedPoolSG(sgpParts);
				TestHoverAll(ref hoveredCount);
				AE(hoveredCount, 4);

				AB(HoverTestPassed(defBowASB_p), false);
				AB(HoverTestPassed(defBowBSB_p), false);
				AB(HoverTestPassed(crfBowASB_p), false);
				AB(HoverTestPassed(defWearASB_p), false);
				AB(HoverTestPassed(defWearBSB_p), false);
				AB(HoverTestPassed(crfWearASB_p), false);
				AB(HoverTestPassed(defPartsSB_p), false);
				AB(HoverTestPassed(crfPartsSB_p), false);
				AB(HoverTestPassed(defBowASB_e), true);
				AB(HoverTestPassed(defWearASB_e), true);
				AB(HoverTestPassed(defPartsSB_p2), true);
				AB(HoverTestPassed(crfPartsSB_p2), true);
				
				hoveredCount = 0;
				sgm.SetFocusedPoolSG(sgpAll);
				TestHoverAll(ref hoveredCount);
				AE(hoveredCount, 6);
				
				AB(HoverTestPassed(defBowASB_p), false);
				AB(HoverTestPassed(defBowBSB_p), true);
				AB(HoverTestPassed(crfBowASB_p), true);
				AB(HoverTestPassed(defWearASB_p), false);
				AB(HoverTestPassed(defWearBSB_p), true);
				AB(HoverTestPassed(crfWearASB_p), true);
				AB(HoverTestPassed(defPartsSB_p), false);
				AB(HoverTestPassed(crfPartsSB_p), false);
				AB(HoverTestPassed(defBowASB_e), true);
				AB(HoverTestPassed(defWearASB_e), true);
				AB(HoverTestPassed(defPartsSB_p2), false);
				AB(HoverTestPassed(crfPartsSB_p2), false);
			}
			public bool HoverTestPassed(Slottable sb){
				bool result = false;
				TestHoverSequence(sb, out result);
				return result;
			}
			public void TestHoverAll(ref int count){
				
				foreach(Slottable sb in SlottableList()){
					bool hovered = false;
					TestHoverSequence(sb, out hovered);
					if(hovered)
						count ++;
				}

			}
			public void TestHoverSequence(Slottable sb, out bool hovered){
				bool pickedUp = false;
				bool reverted = false;
				PickUp(sb, out pickedUp);
				// TestHover(sb, ref pickedUp);
				Revert(sb, out reverted);
				hovered = pickedUp && reverted;
			}
			// public void TestHover(Slottable sb, ref bool picked){
				// 	if(picked){
				// 		ASSB(sb, Slottable.PickedUpAndSelectedState);
						
				// 		// sb.OnEndDragMock(eventData);
						
				// 		ASSB(sb, Slottable.PickedUpAndSelectedState);
				// 	}
				// }
			public void PickUp(Slottable sb, out bool pickedUp){	
				AssertSGMFocused();
				if(sb.CurState == Slottable.FocusedState || sb.CurState == Slottable.EquippedAndDeselectedState){
					
					sb.OnPointerDownMock(eventData);
					ASSB(sb, Slottable.WaitForPickUpState);
					sb.CurProcess.Expire();
					pickedUp = true;
					ASSB(sb, Slottable.PickedUpAndSelectedState);
					AssertPostPickFilter(sb);
				}else{
					pickedUp = false;
				}
			}
			public void Revert(Slottable sb, out bool reverted){
				if(sb.CurState == Slottable.PickedUpAndSelectedState){
					sb.OnPointerUpMock(eventData);
					if(sb.Item.IsStackable){
						ASSB(sb, Slottable.WaitForNextTouchWhilePUState);
						sb.CurProcess.Expire();
					}
						AssertReverting(sb);
					sb.CurProcess.Expire();
					reverted = true;
				}else{
					reverted = false;
				}
				AssertSGMFocused();
			}
	public void TestOnPointerDownOnAllSB(){
		foreach(Slottable sb in SlottableList()){
			TestOnPointerDownSequence(sb);
		}
	}
		
	/*	supportives
	*/
		List<Slottable> SlottableList(){
			List<Slottable> sbList = new List<Slottable>();
			sbList.Add(defBowASB_p);
			sbList.Add(defBowBSB_p);
			sbList.Add(crfBowASB_p);
			sbList.Add(defWearASB_p);
			sbList.Add(defWearBSB_p);
			sbList.Add(crfWearASB_p);
			sbList.Add(defPartsSB_p);
			sbList.Add(crfPartsSB_p);
			sbList.Add(defBowASB_e);
			sbList.Add(defWearASB_e);
			sbList.Add(defPartsSB_p2);
			sbList.Add(crfPartsSB_p2);

			return sbList;
		}
		public void AssertTransactionFieldsAreCleared(Slottable sb){
			AE(sgm.CurState, SlotGroupManager.FocusedState);
			AE(sgm.CurProcess, null);
			AE(sgm.SBA, null);
			AE(sgm.SBB, null);
			AE(sgm.SGA, null);
			AE(sgm.SGB, null);
			AB(sgm.SBADoneTransaction, true);
			AB(sgm.SBBDoneTransaction, true);
			AB(sgm.SGADoneTransaction, true);
			AB(sgm.SGBDoneTransaction, true);

			AE(sb.DestinationSG, null);
			AE(sb.DestinationSlot, null);
			
		}
		public void AssertPostPickFilter(Slottable sb){
			ASSB(sb, Slottable.PickedUpAndSelectedState);
			SlotGroup origSG = sgm.GetSlotGroup(sb);
				ASSG(origSG, SlotGroup.SelectedState);
				
			AE(sgm.CurState, SlotGroupManager.ProbingState);
				AE(sgm.CurProcess.GetType(), typeof(SGMProbingStateProcess));
				AB(sgm.CurProcess.IsRunning, true);
				AB(sgm.CurProcess.IsExpired, false);
				AE(sgm.PickedSB, sb);
				AE(sgm.SelectedSB, sb);
				AE(sgm.SelectedSG, origSG);
			AE(sgm.Transaction.GetType(), typeof(RevertTransaction));

			SlotSystemBundle poolBundle = sgm.RootPage.PoolBundle;
			SlotSystemBundle equipBundle = sgm.RootPage.EquipBundle;
			if(poolBundle.ContainsElement(origSG)){// in the same bundle
				foreach(SlotSystemElement ele in poolBundle.Elements){
					if(ele is SlotGroup){
						SlotGroup sg = (SlotGroup)ele;
						if(sg != origSG){
							ASSG(sg, SlotGroup.DefocusedState);
							/*	sbs
							*/
								foreach(Slot slot in sg.Slots){
									if(slot.Sb.IsEquipped)
										ASSB(slot.Sb, Slottable.EquippedAndDefocusedState);
									else
										ASSB(slot.Sb, Slottable.DefocusedState);
								}
						}else{//	origSG is poolSGs memeber
							foreach(Slot slot in sg.Slots){
								if(slot.Sb == sb)
									ASSB(slot.Sb, Slottable.PickedUpAndSelectedState);
								else if(sg.AutoSort){
									if(slot.Sb.IsEquipped)
										ASSB(slot.Sb, Slottable.EquippedAndDefocusedState);
									else
										ASSB(slot.Sb, Slottable.DefocusedState);
								}else{//	non auto sort
									if(slot.Sb.IsEquipped)
										ASSB(slot.Sb, Slottable.EquippedAndDeselectedState);
									else
										ASSB(slot.Sb, Slottable.FocusedState);
								}
							}
						}
					}
				}
			}else{//	origSg not found in poolBundle
				for(int i = 0; i < equipBundle.Elements.Count; i++){
					EquipmentSet equipSet = (EquipmentSet)equipBundle.Elements[i];
					if(equipSet.ContainsElement(origSG)){// this page contains the origSG
						foreach(SlotSystemElement ele in equipSet.Elements){
							SlotGroup sg = (SlotGroup)ele;
							if(sg != origSG){// on the same page
								if(sg.AcceptsFilter(sb)){
									ASSG(sg, SlotGroup.FocusedState);
									ASSB(sg.Slots[0].Sb, Slottable.EquippedAndDeselectedState);
								}else{
									ASSG(sg, SlotGroup.DefocusedState);
									ASSB(sg.Slots[0].Sb, Slottable.EquippedAndDefocusedState);
								}
							}
						}
					}else{//	this page does not contain the origSG
						foreach(SlotSystemElement ele in equipSet.Elements){
							SlotGroup sg = (SlotGroup)ele;
							ASSG(sg, SlotGroup.DefocusedState);
							foreach(Slot slot in sg.Slots){
								if(slot.Sb != null){
									ASSB(slot.Sb, Slottable.EquippedAndDefocusedState);
								}
							}
						}
					}
				}
			}
		}
		public void AssertSGMFocused(){
			AssertTransactionFieldsAreClearedOnAllSB();
			AE(sgm.CurState, SlotGroupManager.FocusedState);
			if(sgm.RootPage.PoolBundle.GetFocusedBundleElement() == (SlotSystemElement)sgpAll){
				AE(sgpAll.CurState, SlotGroup.FocusedState);
					AB(defBowASB_p.IsEquipped, true);
					ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowBSB_p, Slottable.FocusedState);
					ASSB(crfBowASB_p, Slottable.FocusedState);
					AB(defWearASB_p.IsEquipped, true);
					ASSB(defWearASB_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearBSB_p, Slottable.FocusedState);
					ASSB(crfWearASB_p, Slottable.FocusedState);
					ASSB(defPartsSB_p, Slottable.DefocusedState);
					ASSB(crfPartsSB_p, Slottable.DefocusedState);
				AE(sgpParts.CurState, SlotGroup.DefocusedState);
					ASSB(defPartsSB_p2, Slottable.DefocusedState);
					ASSB(crfPartsSB_p2, Slottable.DefocusedState);
			}else{
				AE(sgm.RootPage.PoolBundle.GetFocusedBundleElement(), sgpParts);
				AE(sgpAll.CurState, SlotGroup.DefocusedState);
					AB(defBowASB_p.IsEquipped, true);
					ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowBSB_p, Slottable.DefocusedState);
					ASSB(crfBowASB_p, Slottable.DefocusedState);
					AB(defWearASB_p.IsEquipped, true);
					ASSB(defWearASB_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearBSB_p, Slottable.DefocusedState);
					ASSB(crfWearASB_p, Slottable.DefocusedState);
					ASSB(defPartsSB_p, Slottable.DefocusedState);
					ASSB(crfPartsSB_p, Slottable.DefocusedState);
				AE(sgpParts.CurState, SlotGroup.FocusedState);
					ASSB(defPartsSB_p2, Slottable.FocusedState);
					ASSB(crfPartsSB_p2, Slottable.FocusedState);
			}
			AE(sgBow.CurState, SlotGroup.FocusedState);
				AB(defBowASB_e.IsEquipped, true);
				ASSB(defBowASB_e, Slottable.EquippedAndDeselectedState);
			AE(sgWear.CurState, SlotGroup.FocusedState);
				AB(defWearASB_e.IsEquipped, true);
				ASSB(defWearASB_e, Slottable.EquippedAndDeselectedState);
		}
		public void AssertSGMDefocus(){
			AE(sgm.CurState, SlotGroupManager.DefocusedState);

			AE(sgpAll.CurState, SlotGroup.DefocusedState);
				ASSB(defBowASB_p, Slottable.EquippedAndDefocusedState);
				ASSB(defBowBSB_p, Slottable.DefocusedState);
				ASSB(crfBowASB_p, Slottable.DefocusedState);
				ASSB(defWearASB_p, Slottable.EquippedAndDefocusedState);
				ASSB(defWearBSB_p, Slottable.DefocusedState);
				ASSB(crfWearASB_p, Slottable.DefocusedState);

				AB(((InventoryItemInstanceMock)defPartsSB_p.Item).IsEquipped, false);
				AE(sgm.GetSlotGroup(defPartsSB_p), sgpAll);

				ASSB(defPartsSB_p, Slottable.DefocusedState);
				ASSB(crfPartsSB_p, Slottable.DefocusedState);
			AE(sgpParts.CurState, SlotGroup.DefocusedState);
				ASSB(defPartsSB_p2, Slottable.DefocusedState);
				ASSB(crfPartsSB_p2, Slottable.DefocusedState);
			AE(sgBow.CurState, SlotGroup.DefocusedState);
				ASSB(defBowASB_e, Slottable.EquippedAndDefocusedState);
			AE(sgWear.CurState, SlotGroup.DefocusedState);
				ASSB(defWearASB_e, Slottable.EquippedAndDefocusedState);

		}
		public void AssertTransactionFieldsAreClearedOnAllSB(){
			foreach(Slottable sb in SlottableList()){
				AssertTransactionFieldsAreCleared(sb);
			}
		}
	/*	states test
	*/
		public void TestOnPointerDownSequence(Slottable sb){
			sgm.RootPage.PoolBundle.SetFocusedBundleElement(sgpParts);
			sgm.Focus();
			AssertSGMFocused();
			TestDeactivatedState(sb);
			TestWaitForPointerUpState(sb);
			TestFocusedState(sb);
			TestEqDeselectedState(sb);
			TestWaitForPickUpState(sb);
			TestPickedUpAndSelectedState(sb);
			TestWaitForNextTouchWhilePUState(sb);
			TestDefocusedStates(sb);
			TestWaitForNextTouchState(sb);

			sgm.RootPage.PoolBundle.SetFocusedBundleElement(sgpAll);		
			sgm.Focus();
			AssertSGMFocused();
			TestDeactivatedState(sb);
			TestWaitForPointerUpState(sb);
			TestFocusedState(sb);
			TestEqDeselectedState(sb);
			TestWaitForPickUpState(sb);
			TestPickedUpAndSelectedState(sb);
			TestWaitForNextTouchWhilePUState(sb);
			TestDefocusedStates(sb);
			TestWaitForNextTouchState(sb);
		}
		public void TestDeactivatedState(Slottable sb){
			
			sgm.Deactivate();

			ASSB(sb, Slottable.DeactivatedState);
			ASSG(sgm.GetSlotGroup(sb), SlotGroup.DeactivatedState);

			ASSB(sb, Slottable.DeactivatedState);

			sb.OnPointerDownMock(eventData);

			ASSB(sb, Slottable.DeactivatedState);
			AE(sb.CurProcess, null);
			
			sb.OnPointerUpMock(eventData);
			ASSB(sb, Slottable.DeactivatedState);
			AE(sb.CurProcess, null);

			sgm.Focus();
			AssertSGMFocused();
		}
		public void TestWaitForPointerUpState(Slottable sb){
			sgm.Defocus();
			AssertSGMDefocus();
			sb.OnPointerDownMock(eventData);


			ASSB(sb, Slottable.WaitForPointerUpState);
			AE(sb.CurProcess.GetType(), typeof(WaitForPointerUpProcess));
			AB(sb.CurProcess.IsRunning, true);
			AB(sb.CurProcess.IsExpired, false);
			/*	OnEndDrag
			*/
				sb.OnEndDragMock(eventData);
				if(sb.IsEquipped){
					ASSB(sb, Slottable.EquippedAndDefocusedState);
					AE(sb.CurProcess, null);
				}else{
					ASSB(sb, Slottable.DefocusedState);
					AE(sb.CurProcess, null);
				}
			
			sb.OnPointerDownMock(eventData);

			/*	OnPointerUp
			*/
				AB(sb.Tapped, false);
				sb.OnPointerUpMock(eventData);
				AB(sb.Tapped, true);
				sb.Tapped = false;
				if(sb.IsEquipped){
					ASSB(sb, Slottable.EquippedAndDefocusedState);
					AE(sb.CurProcess, null);
				}else{
					ASSB(sb, Slottable.DefocusedState);
					AE(sb.CurProcess, null);
				}
			sgm.Focus();

		}
		public void TestFocusedState(Slottable sb){
			sgm.Deactivate();
			sgm.Focus();
			if(sb.CurState == Slottable.FocusedState){
				AE(sb.CurProcess, null);
			}
			sgm.Defocus();
			sgm.Focus();
			if(sb.CurState == Slottable.FocusedState){
				AE(sb.CurProcess.GetType(), typeof(GradualGrayinProcess));
				AB(sb.CurProcess.IsRunning, true);
				AB(sb.CurProcess.IsExpired, false);

				sb.OnPointerDownMock(eventData);
					ASSB(sb,Slottable.WaitForPickUpState);
					AE(sb.CurProcess.GetType(), typeof(WaitForPickUpProcess));
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
				
				// sb.CurProcess.Expire();
				// 	ASSB(sb, Slottable.PickedUpAndSelectedState);
				sb.OnPointerUpMock(eventData);
				if(sb.Item.IsStackable){
					ASSB(sb, Slottable.WaitForNextTouchState);
					AE(sb.CurProcess.GetType(), typeof(WaitForNextTouchProcess));
					AB(sb.Tapped, false);
					sb.CurProcess.Expire();
					AB(sb.Tapped, true);
					sb.Tapped = false;
					ASSB(sb, Slottable.FocusedState);
				}else{
					ASSB(sb, Slottable.FocusedState);
					AB(sb.Tapped, true);
					sb.Tapped = false;
				}
			}
		}
		public void TestEqDeselectedState(Slottable sb){
			sgm.Deactivate();
			sgm.Focus();
			if(sb.CurState == Slottable.EquippedAndDeselectedState){
				AE(sb.CurProcess, null);
			}
			sgm.Defocus();
			sgm.Focus();
			if(sb.CurState == Slottable.EquippedAndDeselectedState){
				AE(sb.CurProcess.GetType(), typeof(EquipGradualGrayinProcess));
				AB(sb.CurProcess.IsRunning, true);
				AB(sb.CurProcess.IsExpired, false);

				sb.OnPointerDownMock(eventData);
					ASSB(sb,Slottable.WaitForPickUpState);
					AE(sb.CurProcess.GetType(), typeof(WaitForPickUpProcess));
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
				
				// sb.CurProcess.Expire();
				// 	ASSB(sb, Slottable.PickedUpAndSelectedState);
				sb.OnPointerUpMock(eventData);
				if(sb.Item.IsStackable){
					
					ASSB(sb, Slottable.WaitForNextTouchState);
					AE(sb.CurProcess.GetType(), typeof(WaitForNextTouchProcess));
					
					sb.CurProcess.Expire();
					AB(sb.Tapped, true);
					sb.Tapped = false;
					ASSB(sb, Slottable.EquippedAndDeselectedState);
				}else{
					ASSB(sb, Slottable.EquippedAndDeselectedState);
					AB(sb.Tapped, true);
					sb.Tapped = false;
				}
			}
		}
		public void TestWaitForPickUpState(Slottable sb){
		AssertSGMFocused();

			if(sb.CurState == Slottable.FocusedState || sb.CurState == Slottable.EquippedAndDeselectedState){
				sb.OnPointerDownMock(eventData);
				ASSB(sb, Slottable.WaitForPickUpState);
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
				/*	1.0	Expire
				*/
				sb.CurProcess.Expire();
					ASSB(sb, Slottable.PickedUpAndSelectedState);
					AssertPostPickFilter(sb);
					AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
				/**/
				sb.OnPointerUpMock(eventData);
				
				if(sb.Item.IsStackable){
					ASSB(sb, Slottable.WaitForNextTouchWhilePUState);
					AE(sb.CurProcess.GetType(), typeof(WaitForNextTouchWhilePUProcess));
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
					/**/
					sb.CurProcess.Expire();

					AssertReverting(sb);
					
					ASSB(sb, Slottable.MovingState);
					AE(sb.CurProcess.GetType(), typeof(MoveProcess));
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
				}else{
					// AB(sgm.CurProcess != null, true);
					AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
					AssertReverting(sb);

					ASSB(sb, Slottable.MovingState);
					AE(sb.CurProcess.GetType(), typeof(MoveProcess));
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);

				}

				sb.CurProcess.Expire();
				AssertTransactionFieldsAreCleared(sb);
				if(sb.IsEquipped)
					ASSB(sb, Slottable.EquippedAndDeselectedState);
				else
					ASSB(sb, Slottable.FocusedState);
				AssertSGMFocused();

				/*	1.1 OnEndDrag
				*/	
				sb.OnPointerDownMock(eventData);
				ASSB(sb, Slottable.WaitForPickUpState);
				sb.OnEndDragMock(eventData);
					if(sb.IsEquipped)
						ASSB(sb, Slottable.EquippedAndDeselectedState);
					else
						ASSB(sb, Slottable.FocusedState);
				AssertSGMFocused();
				/*	1.2	OnPointerUp
				*/
				sb.OnPointerDownMock(eventData);
				ASSB(sb, Slottable.WaitForPickUpState);
				sb.OnPointerUpMock(eventData);
				if(sb.Item.IsStackable){
					ASSB(sb, Slottable.WaitForNextTouchState);
					AB(sb.Tapped, false);
					sb.CurProcess.Expire();
					AB(sb.Tapped, true);
					sb.Tapped = false;

				}else{
					AB(sb.Tapped, true);
					sb.Tapped = false;
					if(sb.IsEquipped)
						ASSB(sb, Slottable.EquippedAndDeselectedState);
					else
						ASSB(sb, Slottable.FocusedState);
				}
				AssertSGMFocused();
				/*	
				*/

			}
		}
			public void AssertReverting(Slottable sb){
				AE(sgm.CurState, SlotGroupManager.PerformingTransactionState);
				AE(sgm.CurProcess.GetType(), typeof(SGMRevertTransactionProcess));
				AB(sgm.CurProcess.IsRunning, true);
				AB(sgm.CurProcess.IsExpired, false);
				AE(sgm.SBA, sb);
				AE(sgm.SBB, null);
				AE(sgm.SGA, null);
				AE(sgm.SGB, null);
				AB(sgm.SBADoneTransaction, false);
				AB(sgm.SBBDoneTransaction, true);
				AB(sgm.SGADoneTransaction, true);
				AB(sgm.SGBDoneTransaction, true);
				
				AE(sb.DestinationSG, sgm.GetSlotGroup(sb));
				AB(sb.DestinationSlot != null, true);
			}
		public void TestPickedUpAndSelectedState(Slottable sb){
			if(sb.CurState == Slottable.FocusedState || sb.CurState == Slottable.EquippedAndDeselectedState){
				sb.OnPointerDownMock(eventData);
				ASSB(sb, Slottable.WaitForPickUpState);
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
				
				sb.CurProcess.Expire();
					ASSB(sb, Slottable.PickedUpAndSelectedState);
					AssertPostPickFilter(sb);
					AE(sgm.Transaction.GetType(), typeof(RevertTransaction));
				/*	1.0 OnPointerUp --> TestWaitForPickUpState
				*/
				/*	1.1 OnEndDrag
				*/
				sb.OnEndDragMock(eventData);
					ASSB(sb, Slottable.MovingState);
					AssertReverting(sb);
					sb.CurProcess.Expire();
					AssertSGMFocused();
			}
		}
		public void TestWaitForNextTouchWhilePUState(Slottable sb){

			if(sb.CurState == Slottable.FocusedState || sb.CurState == Slottable.EquippedAndDeselectedState){
				sb.OnPointerDownMock(eventData);
				ASSB(sb, Slottable.WaitForPickUpState);
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
				
				sb.CurProcess.Expire();
					ASSB(sb, Slottable.PickedUpAndSelectedState);
					AssertPostPickFilter(sb);
					AE(sgm.Transaction.GetType(), typeof(RevertTransaction));

				sb.OnPointerUpMock(eventData);
				
				if(sb.Item.IsStackable){
					ASSB(sb, Slottable.WaitForNextTouchWhilePUState);
					AE(sb.CurProcess.GetType(), typeof(WaitForNextTouchWhilePUProcess));
					AB(sb.CurProcess.IsRunning, true);
					AB(sb.CurProcess.IsExpired, false);
					/*	1.0 Expire --> TestWaitForPickUpState
					*/
					AE(sb.PickedAmount, 1);
					/*	1.1	OnPointerDwon
					*/
					sb.OnPointerDownMock(eventData);
					ASSB(sb, Slottable.PickedUpAndSelectedState);
					AE(sb.PickedAmount, 2);

					sb.OnPointerUpMock(eventData);
					ASSB(sb, Slottable.WaitForNextTouchWhilePUState);
					/*	1.2 OnDeselect
					*/
					sb.OnDeselectedMock(eventData);
					ASSB(sb, Slottable.MovingState);
					AssertReverting(sb);
					sb.CurProcess.Expire();
					AssertSGMFocused();
					/*	1.3
					*/
					
				}else{
					AssertReverting(sb);
					sb.CurProcess.Expire();
					AssertSGMFocused();
				}
			}
		}
		public void TestDefocusedStates(Slottable sb){
			sgm.Defocus();
			if(sb.IsEquipped)
				ASSB(sb, Slottable.EquippedAndDefocusedState);
			else
				ASSB(sb, Slottable.DefocusedState);
			
			sb.OnPointerDownMock(eventData);
				ASSB(sb, Slottable.WaitForPointerUpState);
			sb.OnPointerUpMock(eventData);
				AssertSGMDefocus();
				AE(sb.Tapped, true);
				sb.Tapped = false;
			
			sgm.Focus();
		}
		public void TestWaitForNextTouchState(Slottable sb){
			if(sb.CurState == Slottable.FocusedState || sb.CurState == Slottable.EquippedAndDeselectedState){

				sb.OnPointerDownMock(eventData);
				sb.OnPointerUpMock(eventData);

				if(sb.Item.IsStackable){
					ASSB(sb, Slottable.WaitForNextTouchState);
					/*	1.0 Expire
					*/
					sb.CurProcess.Expire();
						// if(sb.IsEquipped)
						// 	ASSB(sb, Slottable.MovingState);
						// else
							ASSB(sb, Slottable.FocusedState);
						AB(sb.Tapped, true);
						sb.Tapped = false;
					/*	1.1 OnPointerDown
					*/
					sb.OnPointerDownMock(eventData);
					sb.OnPointerUpMock(eventData);
					
					sb.OnPointerDownMock(eventData);
						ASSB(sb, Slottable.PickedUpAndSelectedState);
						AE(sb.PickedAmount, 1);
						AE(sb.CurProcess.GetType(), typeof(PickedUpAndSelectedProcess));
						AB(sb.CurProcess.IsRunning, true);
						AB(sb.CurProcess.IsExpired, false);
						AssertPostPickFilter(sb);
					
					sb.OnPointerUpMock(eventData);
						ASSB(sb, Slottable.WaitForNextTouchWhilePUState);
					sb.CurProcess.Expire();
						ASSB(sb, Slottable.MovingState);
					sb.CurProcess.Expire();
					/*	1.2 OnDeselected
					*/
					sb.OnPointerDownMock(eventData);
					sb.OnPointerUpMock(eventData);
					
					sb.OnDeselectedMock(eventData);
						AssertTransactionFieldsAreCleared(sb);
						if(sb == defBowASB_p || sb == defBowASB_e || sb == defWearASB_p || sb == defWearASB_e)
							AB(sb.IsEquipped, true);
						// if(sb.IsEquipped)
						// 	ASSB(sb, Slottable.MovingState);
						// else
							ASSB(sb, Slottable.FocusedState);
				}else{//	non stackable
					AB(sb.Tapped, true);
					sb.Tapped = false;
				}
			}
			AssertSGMFocused();
		}
		
	/*
	*/
	/*	Assertions
	*/
		public void AssertState<T>() where T: SlottableState{
			Assert.That(defBowASB_p.CurState.GetType(), Is.EqualTo(typeof(T)));
		}
		public void AssertState(Slottable sb, SlottableState sbState){
			Assert.That(sb.CurState, Is.EqualTo(sbState));
		}
		public void AssertPickQuantity(int quant){
			Assert.That(defBowASB_p.PickedAmount, Is.EqualTo(quant));
		}
		public void AE(object inspected, object expected){
			Assert.That(inspected, Is.EqualTo(expected));
		}
		public void AB(bool inspectedBool, bool value){
			if(value)
				Assert.That(inspectedBool, Is.True);
			else
				Assert.That(inspectedBool, Is.False);
		}
		public void AB(object inspected, bool isNull){
			if(isNull)
				Assert.That(inspected, Is.Null);
			else
				Assert.That(inspected, Is.Not.Null);
		}
		public void ASSB(Slottable sb, SlottableState state){
			AE(sb.CurState, state);
		}
		public void ASSG(SlotGroup sg, SlotGroupState state){
			AE(sg.CurState, state);
		}
}
