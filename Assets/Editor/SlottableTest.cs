﻿using UnityEngine;
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
		/*	sgs
		*/
			GameObject sgpAllGO;
			SlotGroup sgpAll;
			GameObject sgBowGO;
			SlotGroup sgBow;
			GameObject sgWearGO;
			SlotGroup sgWear;
			GameObject sgpPartsGO;
			SlotGroup sgpParts;
			GameObject sgCGearsGO;
			SlotGroup sgCGears;
		/*	items
		*/
			Slottable defBowB_p;
			Slottable defBowA_p;
			Slottable crfBowA_p;
			Slottable defWearB_p;
			Slottable defWearA_p;
			Slottable crfWearA_p;
			Slottable defParts_p;
			Slottable crfParts_p;
			Slottable defBowA_e;
			Slottable defWearA_e;
			Slottable defParts_p2;
			Slottable crfParts_p2;
			Slottable defShieldA_e;
			Slottable defShieldA_p;
			Slottable defMWeaponA_e;
			Slottable defMWeaponA_p;
			Slottable crfShieldA_e;
			Slottable crfShieldA_p;
			Slottable crfMWeaponA_e;
			Slottable crfMWeaponA_p;

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
			/*	sgCGears
			*/
				sgCGearsGO = new GameObject("CarriedGearsSG");
				sgCGears = sgCGearsGO.AddComponent<SlotGroup>();
				sgCGears.Filter = new SGCarriedGearFilter();
				sgCGears.Sorter = new SGItemIndexSorter();
				sgCGears.UpdateEquipStatusCommand = new UpdateEquipStatusForEquipSGCommand();
				sgCGears.SetInventory(equipInventory);
				sgCGears.IsShrinkable = true;
				sgCGears.IsExpandable = false;

				((EquipmentSetInventory)sgCGears.Inventory).SetEquippableCGearsCount(4);

				sgCGears.Slots = new List<Slot>();
				for(int i = 0; i < ((EquipmentSetInventory)sgCGears.Inventory).EquippableCGearsCount; i++){
					Slot slot = new Slot();
					sgCGears.Slots.Add(slot);
				}
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
		
			/*	carried gears
			*/
				ShieldMock defShield = new ShieldMock();
				defShield.ItemID = 200;
				AB(defShield.IsStackable, false);
				
				ShieldInstanceMock defShieldA = new ShieldInstanceMock();
				defShieldA.Item = defShield;
				AE(defShieldA.Quantity, 1);
				
				ShieldMock crfShield = new ShieldMock();
				crfShield.ItemID = 201;
				AB(crfShield.IsStackable, false);

				ShieldInstanceMock crfShieldA = new ShieldInstanceMock();
				crfShieldA.Item = crfShield;
				AE(crfShieldA.Quantity, 1);

				MeleeWeaponMock defMWeapon = new MeleeWeaponMock();
				defMWeapon.ItemID = 300;
				MeleeWeaponMock crfMWeapon = new MeleeWeaponMock();
				crfMWeapon.ItemID = 301;
				MeleeWeaponInstanceMock defMWeaponA = new MeleeWeaponInstanceMock();
				defMWeaponA.Item = defMWeapon;
				MeleeWeaponInstanceMock crfMWeaponA = new MeleeWeaponInstanceMock();
				crfMWeaponA.Item = crfMWeapon;


				inventory.Add(defShieldA);
				inventory.Add(crfShieldA);
				inventory.Add(defMWeaponA);
				inventory.Add(crfMWeaponA);
				equipInventory.Add(defShieldA);
				equipInventory.Add(defMWeaponA);

		/**/
		EquipmentSet equipSetA = new EquipmentSet(sgBow, sgWear, sgCGears);
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

			Assert.That(equipSetA.Elements.Count, Is.EqualTo(3));
			Assert.That(equipSetA.ContainsElement(sgBow), Is.True);
			AE(sgBow.SGM, sgm);
			Assert.That(equipSetA.ContainsElement(sgWear), Is.True);
			AE(sgWear.SGM, sgm);
			
		

		sgm.Initialize();
			/*	Assert Initialiation
				when the scene is loaded, but not yet focused
			*/
			defBowA_p = sgpAll.GetSlottable(defBowA);	
			defBowB_p = sgpAll.GetSlottable(defBowB);
			crfBowA_p = sgpAll.GetSlottable(crfBowA);
			defWearA_p = sgpAll.GetSlottable(defWearA);
			defWearB_p = sgpAll.GetSlottable(defWearB);
			crfWearA_p = sgpAll.GetSlottable(crfWearA);
			defParts_p = sgpAll.GetSlottable(defPartsA);
			crfParts_p = sgpAll.GetSlottable(crfPartsA);
			defBowA_e = sgBow.GetSlottable(defBowA);
			defWearA_e = sgWear.GetSlottable(defWearA);
			defParts_p2 = sgpParts.GetSlottable(defPartsA);
			crfParts_p2 = sgpParts.GetSlottable(crfPartsA);
			defShieldA_p = sgpAll.GetSlottable(defShieldA);
			crfShieldA_p = sgpAll.GetSlottable(crfShieldA);
			defMWeaponA_p =	sgpAll.GetSlottable(defMWeaponA); 
			crfMWeaponA_p =	sgpAll.GetSlottable(crfMWeaponA);
			defShieldA_e = sgCGears.GetSlottable(defShieldA);
			defMWeaponA_e = sgCGears.GetSlottable(defMWeaponA);

			Assert.That(sgm.CurState, Is.EqualTo(SlotGroupManager.DeactivatedState));
			/*	sgpAll
			*/
			Assert.That(sgpAll.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgpAll.FilteredItems.Count, Is.EqualTo(12));
			Assert.That(sgpAll.FilteredItems, Is.Ordered);
			Assert.That(sgpAll.Slots.Count, Is.EqualTo(12));
			foreach(Slot slot in sgpAll.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
				
				AE(sgm.GetSlotGroup(defBowA_p), sgpAll);
				AE(sgm.GetSlotGroup(defBowB_p), sgpAll);
				AE(sgm.GetSlotGroup(crfBowA_p), sgpAll);
				AE(sgm.GetSlotGroup(defWearA_p), sgpAll);
				AE(sgm.GetSlotGroup(defWearB_p), sgpAll);
				AE(sgm.GetSlotGroup(crfWearA_p), sgpAll);
				AE(sgm.GetSlotGroup(defParts_p), sgpAll);
				AE(sgm.GetSlotGroup(crfParts_p), sgpAll);
				
			/*	sgpParts
			*/
			Assert.That(sgpParts.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgpParts.FilteredItems.Count, Is.EqualTo(2));
			Assert.That(sgpParts.FilteredItems, Is.Ordered);
			Assert.That(sgpParts.Slots.Count, Is.EqualTo(2));
			foreach(Slot slot in sgpParts.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
				AE(sgm.GetSlotGroup(defParts_p2), sgpParts);
				AE(sgm.GetSlotGroup(crfParts_p2), sgpParts);
				
			/*	sgBow
			*/
			Assert.That(sgBow.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgBow.FilteredItems.Count, Is.EqualTo(1));
			Assert.That(sgBow.Slots.Count, Is.EqualTo(1));
			foreach(Slot slot in sgBow.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
			AE(sgm.GetSlotGroup(defBowA_e), sgBow);
			InventoryItemInstanceMock defBowA_e_Inst = (InventoryItemInstanceMock)defBowA_e.Item;
			AB(defBowA_e_Inst.IsEquipped, true);
			/*	sgWear
			*/
			Assert.That(sgWear.CurState, Is.EqualTo(SlotGroup.DeactivatedState));
			Assert.That(sgWear.FilteredItems.Count, Is.EqualTo(1));
			Assert.That(sgWear.Slots.Count, Is.EqualTo(1));
			foreach(Slot slot in sgWear.Slots){
				Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DeactivatedState));
			}
			AE(sgm.GetSlotGroup(defWearA_e), sgWear);
			InventoryItemInstanceMock defWearA_e_Inst = (InventoryItemInstanceMock)defWearA_e.Item;
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
		/*	testing sgCarriedGears
		*/
			/*	setting up
			*/
				AB(sgCGears != null, true);
				AE(sgCGears.Filter.GetType(), typeof(SGCarriedGearFilter));
				AE(sgCGears.Sorter.GetType(), typeof(SGItemIndexSorter));
				AE(sgCGears.UpdateEquipStatusCommand.GetType(), typeof(UpdateEquipStatusForEquipSGCommand));
				AE(sgCGears.Inventory, sgBow.Inventory);
				AB(sgCGears.IsShrinkable, true);
				AB(sgCGears.IsExpandable, false);

				AE(((EquipmentSet)sgm.RootPage.EquipBundle.GetFocusedBundleElement()).Elements.Count, 3);
				AB(sgm.RootPage.EquipBundle.GetFocusedBundleElement().ContainsElement(sgCGears), true);
				AE(sgCGears.Slots.Count, 4);
				AE(sgm.GetEquippedCarriedGears().Count, 2);

				AE(sgpAll.Slots.Count, 12);
				AB(defShieldA_p == null, false);
				AE(sgm.GetSlotGroup(defShieldA_p), sgpAll);
				AB(crfShieldA_p == null, false);
				AE(sgm.GetSlotGroup(crfShieldA_p), sgpAll);
				AB(defMWeaponA_p == null, false);
				AE(sgm.GetSlotGroup(defMWeaponA_p), sgpAll);
				AB(crfMWeaponA_p == null, false);
				AE(sgm.GetSlotGroup(crfMWeaponA_p), sgpAll);
				AB(defShieldA_e == null, false);
				AE(sgm.GetSlotGroup(defShieldA_e), sgCGears);
				AB(defMWeaponA_e == null, false);
				AE(sgm.GetSlotGroup(defMWeaponA_e), sgCGears);
				
				ASSG(sgpAll, SlotGroup.FocusedState);
				ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
				ASSB(crfShieldA_p, Slottable.FocusedState);
				ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
				ASSB(crfMWeaponA_p, Slottable.FocusedState);

				ASSG(sgCGears, SlotGroup.FocusedState);
				ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
				ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);

				sgm.Defocus();

				ASSG(sgpAll, SlotGroup.DefocusedState);
				ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
				ASSB(crfShieldA_p, Slottable.DefocusedState);
				ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
				ASSB(crfMWeaponA_p, Slottable.DefocusedState);

				ASSG(sgCGears, SlotGroup.DefocusedState);
				ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
				ASSB(defMWeaponA_e, Slottable.EquippedAndDefocusedState);

				sgm.Focus();
				sgm.SetFocusedPoolSG(sgpParts);
				ASSG(sgpAll, SlotGroup.DefocusedState);
				ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
				ASSB(crfShieldA_p, Slottable.DefocusedState);
				ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
				ASSB(crfMWeaponA_p, Slottable.DefocusedState);

				ASSG(sgCGears, SlotGroup.FocusedState);
				ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
				ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
				
				sgm.SetFocusedPoolSG(sgpAll);
			/*	
			*/
			TestHover();
	}
	
	/*	Test hover
	*/
		bool picked;
		Slottable selectedSB;
		SlotGroup selectedSG;
		SlotSystemTransaction transaction;
		public void TestHover(){
			
			SlotGroup origSG = sgm.GetSlotGroup(defBowB_p);
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

			// TestPostPickFilter();

			// TestSimHoverOnAllSB();
				// TestSimHoverOnAllSB(defBowASB_p);
				// TestSimHoverOnAllSB(defBowBSB_p);
				// TestSimHoverOnAllSB(crfBowASB_p);
				// TestSimHoverOnAllSB(defWearASB_p);
				// TestSimHoverOnAllSB(defWearBSB_p);
				// TestSimHoverOnAllSB(crfWearASB_p);
				// TestSimHoverOnAllSB(defBowASB_e);
				// TestSimHoverOnAllSB(defWearASB_e);
				// TestSimHoverOnAllSB(defBowASB_p);
			
				
			/*
			*/
			// TestHoverDefBowASBE();
			// TestHoverDefBowASBP();
			// TestHoverDefBowBSBP();
			// TestHoverDefWearASBE();
			//TestHoverPartsInSGPParts();
			// TestHoverDefShieldA();
				
		}
		public void TestPostPickFilter(){
			AB(sgpAll.AutoSort, true);
			bool picked;
			bool reverted;
			PickUp(defBowB_p, out picked);
				ASSG(sgpAll, SlotGroup.SelectedState);
					ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowB_p, Slottable.PickedUpAndSelectedState);
					ASSB(crfBowA_p, Slottable.DefocusedState);
					ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearB_p, Slottable.DefocusedState);
					ASSB(crfWearA_p, Slottable.DefocusedState);
					ASSB(defParts_p, Slottable.DefocusedState);
					ASSB(crfParts_p, Slottable.DefocusedState);

					ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfShieldA_p, Slottable.DefocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfMWeaponA_p, Slottable.DefocusedState);

				ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
				AB(sgm.RootPage.EquipBundle.GetFocusedBundleElement().ContainsElement(sgWear), true);
				AB(sgWear.AcceptsFilter(defBowB_p), false);
				ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
				ASSB(defParts_p2, Slottable.DefocusedState);
				ASSB(crfParts_p2, Slottable.DefocusedState);

				ASSG(sgCGears, SlotGroup.DefocusedState);
					ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
					ASSB(defMWeaponA_e, Slottable.EquippedAndDefocusedState);

			Revert(defBowB_p, out reverted);

			AssertSGMFocused();
			sgpAll.AutoSort = false;
			PickUp(defBowB_p, out picked);
				ASSG(sgpAll, SlotGroup.SelectedState);
					ASSB(defBowA_p, Slottable.EquippedAndDeselectedState);
					ASSB(defBowB_p, Slottable.PickedUpAndSelectedState);
					ASSB(crfBowA_p, Slottable.FocusedState);
					ASSB(defWearA_p, Slottable.EquippedAndDeselectedState);
					ASSB(defWearB_p, Slottable.FocusedState);
					ASSB(crfWearA_p, Slottable.FocusedState);
					ASSB(defParts_p, Slottable.FocusedState);
					ASSB(crfParts_p, Slottable.FocusedState);

					ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);
					ASSB(crfShieldA_p, Slottable.FocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
					ASSB(crfMWeaponA_p, Slottable.FocusedState);

				ASSG(sgBow, SlotGroup.FocusedState);
					ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
				ASSG(sgWear, SlotGroup.DefocusedState);
					ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgCGears, SlotGroup.DefocusedState);
					ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
					ASSB(defMWeaponA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgpParts, SlotGroup.DefocusedState);
					ASSB(defParts_p2, Slottable.DefocusedState);
					ASSB(crfParts_p2, Slottable.DefocusedState);
			Revert(defBowB_p, out reverted);
			AssertSGMFocused();
			//

				ASSG(sgpAll, SlotGroup.FocusedState);
					ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowB_p, Slottable.FocusedState);
					ASSB(crfBowA_p, Slottable.FocusedState);
					ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearB_p, Slottable.FocusedState);
					ASSB(crfWearA_p, Slottable.FocusedState);
					ASSB(defParts_p, Slottable.DefocusedState);
					ASSB(crfParts_p, Slottable.DefocusedState);
					ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfShieldA_p, Slottable.FocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfMWeaponA_p, Slottable.FocusedState);
				ASSG(sgBow, SlotGroup.FocusedState);
					ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
				ASSG(sgWear, SlotGroup.FocusedState);
					ASSB(defWearA_e, Slottable.EquippedAndDeselectedState);
				ASSG(sgCGears, SlotGroup.FocusedState);
					ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
					ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
				ASSG(sgpParts, SlotGroup.DefocusedState);
					ASSB(defParts_p2, Slottable.DefocusedState);
					ASSB(crfParts_p2, Slottable.DefocusedState);
			
			AB(sgpAll.AutoSort, false);
			
			PickUp(defBowA_e, out picked);
			
				ASSG(sgpAll, SlotGroup.FocusedState);
					ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowB_p, Slottable.FocusedState);
					ASSB(crfBowA_p, Slottable.FocusedState);
					ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearB_p, Slottable.DefocusedState);
					ASSB(crfWearA_p, Slottable.DefocusedState);
					ASSB(defParts_p, Slottable.DefocusedState);
					ASSB(crfParts_p, Slottable.DefocusedState);
					ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfShieldA_p, Slottable.DefocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfMWeaponA_p, Slottable.DefocusedState);
				ASSG(sgBow, SlotGroup.SelectedState);
					ASSB(defBowA_e, Slottable.PickedUpAndSelectedState);
				ASSG(sgWear, SlotGroup.DefocusedState);
					ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgCGears, SlotGroup.DefocusedState);
					ASSB(defShieldA_e, Slottable.EquippedAndDefocusedState);
					ASSB(defMWeaponA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgpParts, SlotGroup.DefocusedState);
					ASSB(defParts_p2, Slottable.DefocusedState);
					ASSB(crfParts_p2, Slottable.DefocusedState);
				
			Revert(defBowA_e, out reverted);
			AssertSGMFocused();

			PickUp(defShieldA_p, out picked);
				ASSG(sgpAll, SlotGroup.FocusedState);
				ASSG(sgBow, SlotGroup.FocusedState);
				ASSG(sgWear, SlotGroup.FocusedState);
				ASSG(sgCGears, SlotGroup.FocusedState);
				ASSG(sgpParts, SlotGroup.DefocusedState);
			Revert(defShieldA_p, out reverted);
			AB(picked, false);
			AB(reverted, false);

			PickUp(crfShieldA_p, out picked);
				ASSG(sgpAll, SlotGroup.SelectedState);
					ASSB(defBowA_p, Slottable.EquippedAndDeselectedState);
					ASSB(defBowB_p, Slottable.FocusedState);
					ASSB(crfBowA_p, Slottable.FocusedState);
					ASSB(defWearA_p, Slottable.EquippedAndDeselectedState);
					ASSB(defWearB_p, Slottable.FocusedState);
					ASSB(crfWearA_p, Slottable.FocusedState);
					ASSB(defParts_p, Slottable.FocusedState);
					ASSB(crfParts_p, Slottable.FocusedState);
					ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);
					ASSB(crfShieldA_p, Slottable.PickedUpAndSelectedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDeselectedState);
					ASSB(crfMWeaponA_p, Slottable.FocusedState);
				ASSG(sgBow, SlotGroup.DefocusedState);
					ASSB(defBowA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgWear, SlotGroup.DefocusedState);
					ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgCGears, SlotGroup.FocusedState);
					ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
					ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
				ASSG(sgpParts, SlotGroup.DefocusedState);
					ASSB(defParts_p2, Slottable.DefocusedState);
					ASSB(crfParts_p2, Slottable.DefocusedState);
			Revert(crfShieldA_p, out reverted);
			AB(picked, true);
			AB(reverted, true);
			AssertSGMFocused();

			sgpAll.AutoSort = true;
			sgCGears.AutoSort = false;
			AB(sgCGears.AutoSort, false);
			PickUp(defShieldA_e, out picked);
			AB(picked, true);
				ASSG(sgpAll, SlotGroup.FocusedState);
					ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowB_p, Slottable.DefocusedState);
					ASSB(crfBowA_p, Slottable.DefocusedState);
					ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearB_p, Slottable.DefocusedState);
					ASSB(crfWearA_p, Slottable.DefocusedState);
					ASSB(defParts_p, Slottable.DefocusedState);
					ASSB(crfParts_p, Slottable.DefocusedState);
						AB(object.ReferenceEquals(defShieldA_e.Item, defShieldA_p.Item), true);
						AB(sgm.GetSlotGroup(defShieldA_e).IsShrinkable, true);
						AB(SlotSystem.Utility.HaveCommonItemFamily(defShieldA_e, defShieldA_p), true);
					ASSB(defShieldA_p, Slottable.EquippedAndDeselectedState);//
					ASSB(crfShieldA_p, Slottable.FocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfMWeaponA_p, Slottable.FocusedState);
				ASSG(sgBow, SlotGroup.DefocusedState);
					ASSB(defBowA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgWear, SlotGroup.DefocusedState);
					ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);
				ASSG(sgCGears, SlotGroup.SelectedState);
					ASSB(defShieldA_e, Slottable.PickedUpAndSelectedState);
					ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
				ASSG(sgpParts, SlotGroup.DefocusedState);
					ASSB(defParts_p2, Slottable.DefocusedState);
					ASSB(crfParts_p2, Slottable.DefocusedState);
			AB(reverted, true);
			Revert(defShieldA_e, out reverted);
			AssertSGMFocused();
		}
		/*	spot tests hover
		*/
			public void TestHoverDefShieldA(){
				bool picked;
				bool reverted;
				AssertSGMFocused();
				sgm.SetFocusedPoolSG(sgpParts);
				AssertSGMFocused();
				sgm.SetFocusedPoolSG(sgpAll);
				AssertSGMFocused();

				PickUp(defShieldA_p, out picked);
				Revert(defShieldA_p, out reverted);
				AssertSGMFocused();
				AB(picked, false);
				AB(reverted, false);


				/*	picking crfShieldA_p
				*/
					Slottable pickedSB = crfShieldA_p;
					AB(sgpAll.AutoSort, true);
					Slottable target;
					/*	sgpAll
					*/
						target = defBowA_p;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
						target = pickedSB;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, pickedSB, sgpAll, false);
						
						sgpAll.AutoSort = false;
						
						target = defBowA_p;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<ReorderTransaction>(true, target, sgpAll, false);
						target = defShieldA_p;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<ReorderTransaction>(true, target, sgpAll, false);
						target = pickedSB;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, pickedSB, sgpAll, false);
					/*	equip sgs
					*/
						target = defBowA_e;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, null, null, false);
						target = defShieldA_e;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<SwapTransaction>(true, target, sgCGears, false);
						target = defMWeaponA_e;
							TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<SwapTransaction>(true, target, sgCGears, false);


				
				
				/*	picking defMWeaponA_p
				*/
					pickedSB = defMWeaponA_p;
					target = defBowB_p;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(false, null, null, true);
				/*	picking defMWeaponA_e
				*/
					AB(sgpAll.AutoSort, false);
					AB(sgCGears.AutoSort, true);
					pickedSB = defMWeaponA_e;
					target = defBowB_p;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
					target = defMWeaponA_p;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<UnequipTransaction>(true, defMWeaponA_p, sgpAll, false);
					target = crfShieldA_p;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<SwapTransaction>(true, target, sgpAll, false);
					target = crfMWeaponA_p;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<SwapTransaction>(true, target, sgpAll, false);
					target = defShieldA_p;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
					target = defBowA_e;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(true, null, null, false);
					target = defWearA_e;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(true, null, null, false);
					
					target = defShieldA_e;
						TestSimHover(pickedSB, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(true, null, sgCGears, false);
					
			}
			public void TestHoverDefBowASBE(){
				SlotGroup origSG = sgBow;
				/*	defBowASB_e
				*/
					/*	AutoSort false
					*/
						Slottable targetSB = defBowA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defBowB_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, defBowB_p);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(SwapTransaction));

						targetSB = crfBowA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, crfBowA_p);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(SwapTransaction));

						targetSB = defWearA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defWearB_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = crfWearA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));

						targetSB = defParts_p2;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = crfParts_p2;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defBowA_e;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, defBowA_e);
							AE(selectedSG, sgBow);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defWearA_e;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defShieldA_p;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
						targetSB = crfShieldA_p;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
						targetSB = defShieldA_e;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AssertSimHover<RevertTransaction>(true, null, null, false);
						
						
						sgm.SetFocusedPoolSG(sgpParts);
						
							targetSB = defParts_p2;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
								AB(picked, true);
								AE(selectedSB, null);
								AE(selectedSG, sgpParts);
								AE(transaction.GetType(), typeof(RevertTransaction));
							
							targetSB = crfParts_p2;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
								AB(picked, true);
								AE(selectedSB, null);
								AE(selectedSG, sgpParts);
								AE(transaction.GetType(), typeof(RevertTransaction));
					/*	AutoSort true
					*/
						sgm.SetFocusedPoolSG(sgpAll);
						origSG.AutoSort = true;
						targetSB = defBowA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defBowB_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, defBowB_p);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(SwapTransaction));

						targetSB = crfBowA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, crfBowA_p);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(SwapTransaction));

						targetSB = defWearA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defWearB_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = crfWearA_p;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, sgpAll);
							AE(transaction.GetType(), typeof(RevertTransaction));

						targetSB = defParts_p2;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = crfParts_p2;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defBowA_e;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, defBowA_e);
							AE(selectedSG, sgBow);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						targetSB = defWearA_e;
						TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
							AB(picked, true);
							AE(selectedSB, null);
							AE(selectedSG, null);
							AE(transaction.GetType(), typeof(RevertTransaction));
						
						sgm.SetFocusedPoolSG(sgpParts);
						
							targetSB = defParts_p2;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
								AB(picked, true);
								AE(selectedSB, null);
								AE(selectedSG, sgpParts);
								AE(transaction.GetType(), typeof(RevertTransaction));
							
							targetSB = crfParts_p2;
							TestSimHover(defBowA_e, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
								AB(picked, true);
								AE(selectedSB, null);
								AE(selectedSG, sgpParts);
								AE(transaction.GetType(), typeof(RevertTransaction));
			}
			public void TestHoverDefBowASBP(){
				Slottable targetSB;
				targetSB = defBowA_p;
						TestSimHover(defBowA_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(false, null, null, true);
					targetSB = defBowB_p;
						TestSimHover(defBowA_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(false, null, null, true);
					targetSB = defBowA_e;
						TestSimHover(defBowA_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
						AssertSimHover<RevertTransaction>(false, null, null, true);
			}
			public void TestHoverDefBowBSBP(){
				sgpAll.AutoSort = true;
				sgm.SetFocusedPoolSG(sgpAll);
				AssertSGMFocused();

				Slottable targetSB;
				targetSB = defBowA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				targetSB = defBowB_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, defBowB_p, sgpAll, false);
				targetSB = crfBowA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				targetSB = defBowA_e;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<SwapTransaction>(true, defBowA_e, sgBow, false);
				targetSB = defWearA_e;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				targetSB = defShieldA_e;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				targetSB = defShieldA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				

				sgpAll.AutoSort = false;

				targetSB = defBowA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, defBowA_p, sgpAll, false);
				targetSB = crfBowA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, crfBowA_p, sgpAll, false);
				targetSB = defBowB_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, defBowB_p, sgpAll, false);
				targetSB = defWearA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, defWearA_p, sgpAll, false);
				targetSB = defWearB_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, defWearB_p, sgpAll, false);
				targetSB = crfWearA_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, crfWearA_p, sgpAll, false);
				targetSB = defParts_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, defParts_p, sgpAll, false);
				targetSB = crfParts_p;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, crfParts_p, sgpAll, false);
				targetSB = defBowA_e;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<SwapTransaction>(true, defBowA_e, sgBow, false);
				targetSB = defWearA_e;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				targetSB = defParts_p2;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				targetSB = crfParts_p2;
					TestSimHover(defBowB_p, targetSB, sgm.GetSlotGroup(targetSB), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);

			}
			public void TestHoverDefWearASBE(){
				sgm.SetFocusedPoolSG(sgpAll);
				AssertSGMFocused();
				Slottable target;

				target = defBowA_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = defBowB_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = crfBowA_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = defWearA_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = defWearB_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<SwapTransaction>(true, defWearB_p, sgpAll, false);
				target = crfWearA_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<SwapTransaction>(true, crfWearA_p, sgpAll, false);
				target = defParts_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				target = crfParts_p;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpAll, false);
				
				target = defBowA_e;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				target = defWearA_e;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, defWearA_e, sgWear, false);
				target = defParts_p2;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				
				sgm.SetFocusedPoolSG(sgpParts);
				target = defParts_p2;
					TestSimHover(defWearA_e, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpParts, false);
				sgm.SetFocusedPoolSG(sgpAll);


			}
			public void TestHoverPartsInSGPParts(){
				sgm.SetFocusedPoolSG(sgpParts);
				AssertSGMFocused();
				Slottable target;

				target = defParts_p2;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, target, sgpParts, false);
				target = crfParts_p2;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, sgpParts, false);
				
				sgpParts.AutoSort = false;
				target = defParts_p2;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, target, sgpParts, false);
				target = crfParts_p2;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<ReorderTransaction>(true, target, sgpParts, false);
				sgpParts.AutoSort = true;
				
				target = defParts_p;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				target = defBowA_e;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
					AssertSimHover<RevertTransaction>(true, null, null, false);
				target = defWearA_e;
					TestSimHover(defParts_p2, target, sgm.GetSlotGroup(target), out picked, out selectedSB, out selectedSG, out transaction);
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
								else{
									if(object.ReferenceEquals(hoveredSB.Item, pickedSB.Item) && sgm.GetSlotGroup(hoveredSB).IsShrinkable)
										AE(sgm.Transaction.GetType(), typeof(UnequipTransaction));
									else
										AE(sgm.Transaction.GetType(), typeof(SwapTransaction));
								}
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

				AB(HoverTestPassed(defBowA_p), false);
				AB(HoverTestPassed(defBowB_p), false);
				AB(HoverTestPassed(crfBowA_p), false);
				AB(HoverTestPassed(defWearA_p), false);
				AB(HoverTestPassed(defWearB_p), false);
				AB(HoverTestPassed(crfWearA_p), false);
				AB(HoverTestPassed(defParts_p), false);
				AB(HoverTestPassed(crfParts_p), false);
				AB(HoverTestPassed(defBowA_e), true);
				AB(HoverTestPassed(defWearA_e), true);
				AB(HoverTestPassed(defParts_p2), true);
				AB(HoverTestPassed(crfParts_p2), true);
				
				hoveredCount = 0;
				sgm.SetFocusedPoolSG(sgpAll);
				TestHoverAll(ref hoveredCount);
				AE(hoveredCount, 6);
				
				AB(HoverTestPassed(defBowA_p), false);
				AB(HoverTestPassed(defBowB_p), true);
				AB(HoverTestPassed(crfBowA_p), true);
				AB(HoverTestPassed(defWearA_p), false);
				AB(HoverTestPassed(defWearB_p), true);
				AB(HoverTestPassed(crfWearA_p), true);
				AB(HoverTestPassed(defParts_p), false);
				AB(HoverTestPassed(crfParts_p), false);
				AB(HoverTestPassed(defBowA_e), true);
				AB(HoverTestPassed(defWearA_e), true);
				AB(HoverTestPassed(defParts_p2), false);
				AB(HoverTestPassed(crfParts_p2), false);
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
	
		
	/*	supportives
	*/
		List<Slottable> SlottableList(){
			List<Slottable> sbList = new List<Slottable>();
			sbList.Add(defBowA_p);
			sbList.Add(defBowB_p);
			sbList.Add(crfBowA_p);
			sbList.Add(defWearA_p);
			sbList.Add(defWearB_p);
			sbList.Add(crfWearA_p);
			sbList.Add(defParts_p);
			sbList.Add(crfParts_p);
			sbList.Add(defBowA_e);
			sbList.Add(defWearA_e);
			sbList.Add(defParts_p2);
			sbList.Add(crfParts_p2);
			sbList.Add(defShieldA_p);
			sbList.Add(crfShieldA_p);
			sbList.Add(defMWeaponA_p);
			sbList.Add(crfMWeaponA_p);
			sbList.Add(defShieldA_e);
			sbList.Add(defMWeaponA_e);
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
				ASSG(sgpAll, SlotGroup.FocusedState);
					AB(defBowA_p.IsEquipped, true);
					ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowB_p, Slottable.FocusedState);
					ASSB(crfBowA_p, Slottable.FocusedState);
					AB(defWearA_p.IsEquipped, true);
					ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearB_p, Slottable.FocusedState);
					ASSB(crfWearA_p, Slottable.FocusedState);
					ASSB(defParts_p, Slottable.DefocusedState);
					ASSB(crfParts_p, Slottable.DefocusedState);
					ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfShieldA_p, Slottable.FocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfMWeaponA_p, Slottable.FocusedState);
					
				ASSG(sgpParts, SlotGroup.DefocusedState);
					ASSB(defParts_p2, Slottable.DefocusedState);
					ASSB(crfParts_p2, Slottable.DefocusedState);
			}else{
				AE(sgm.RootPage.PoolBundle.GetFocusedBundleElement(), sgpParts);
				ASSG(sgpAll, SlotGroup.DefocusedState);
					AB(defBowA_p.IsEquipped, true);
					ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defBowB_p, Slottable.DefocusedState);
					ASSB(crfBowA_p, Slottable.DefocusedState);
					AB(defWearA_p.IsEquipped, true);
					ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
					ASSB(defWearB_p, Slottable.DefocusedState);
					ASSB(crfWearA_p, Slottable.DefocusedState);
					ASSB(defParts_p, Slottable.DefocusedState);
					ASSB(crfParts_p, Slottable.DefocusedState);
					ASSB(defShieldA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfShieldA_p, Slottable.DefocusedState);
					ASSB(defMWeaponA_p, Slottable.EquippedAndDefocusedState);
					ASSB(crfMWeaponA_p, Slottable.DefocusedState);
				ASSG(sgpParts, SlotGroup.FocusedState);
					ASSB(defParts_p2, Slottable.FocusedState);
					ASSB(crfParts_p2, Slottable.FocusedState);
			}
			ASSG(sgBow, SlotGroup.FocusedState);
				AB(defBowA_e.IsEquipped, true);
				ASSB(defBowA_e, Slottable.EquippedAndDeselectedState);
			ASSG(sgWear, SlotGroup.FocusedState);
				AB(defWearA_e.IsEquipped, true);
				ASSB(defWearA_e, Slottable.EquippedAndDeselectedState);
			ASSG(sgCGears, SlotGroup.FocusedState);
				AB(defShieldA_e.IsEquipped, true);
				ASSB(defShieldA_e, Slottable.EquippedAndDeselectedState);
				AB(defMWeaponA_e.IsEquipped, true);
				ASSB(defMWeaponA_e, Slottable.EquippedAndDeselectedState);
		}
		public void AssertSGMDefocus(){
			AE(sgm.CurState, SlotGroupManager.DefocusedState);

			AE(sgpAll.CurState, SlotGroup.DefocusedState);
				ASSB(defBowA_p, Slottable.EquippedAndDefocusedState);
				ASSB(defBowB_p, Slottable.DefocusedState);
				ASSB(crfBowA_p, Slottable.DefocusedState);
				ASSB(defWearA_p, Slottable.EquippedAndDefocusedState);
				ASSB(defWearB_p, Slottable.DefocusedState);
				ASSB(crfWearA_p, Slottable.DefocusedState);

				AB(((InventoryItemInstanceMock)defParts_p.Item).IsEquipped, false);
				AE(sgm.GetSlotGroup(defParts_p), sgpAll);

				ASSB(defParts_p, Slottable.DefocusedState);
				ASSB(crfParts_p, Slottable.DefocusedState);
			AE(sgpParts.CurState, SlotGroup.DefocusedState);
				ASSB(defParts_p2, Slottable.DefocusedState);
				ASSB(crfParts_p2, Slottable.DefocusedState);
			AE(sgBow.CurState, SlotGroup.DefocusedState);
				ASSB(defBowA_e, Slottable.EquippedAndDefocusedState);
			AE(sgWear.CurState, SlotGroup.DefocusedState);
				ASSB(defWearA_e, Slottable.EquippedAndDefocusedState);

		}
		public void AssertTransactionFieldsAreClearedOnAllSB(){
			foreach(Slottable sb in SlottableList()){
				AssertTransactionFieldsAreCleared(sb);
			}
		}
	/*	states test
	*/
		public void TestOnPointerDownOnAllSB(){
			foreach(Slottable sb in SlottableList()){
				TestOnPointerDownSequence(sb);
			}
		}
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
						if(sb == defBowA_p || sb == defBowA_e || sb == defWearA_p || sb == defWearA_e)
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
			Assert.That(defBowA_p.CurState.GetType(), Is.EqualTo(typeof(T)));
		}
		public void AssertState(Slottable sb, SlottableState sbState){
			Assert.That(sb.CurState, Is.EqualTo(sbState));
		}
		public void AssertPickQuantity(int quant){
			Assert.That(defBowA_p.PickedAmount, Is.EqualTo(quant));
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
