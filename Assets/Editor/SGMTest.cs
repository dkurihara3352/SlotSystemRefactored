using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections.Generic;
public class SGMTest {

	GameObject sgmGO;
	SlotGroupManager sgm;
	GameObject sgpAllGO;
	SlotGroup sgpAll;
	GameObject sgpBowGO;
	SlotGroup sgpBow;
	GameObject sgpWearGO;
	SlotGroup sgpWear;
	GameObject sgBowGO;
	SlotGroup sgBow;
	GameObject sgWearGO;
	SlotGroup sgWear;

	GameObject scrollerGO;
	AxisScrollerMock scroller;
	BowMock defBow;
	BowMock crfBow;
	WearMock defWear;
	WearMock crfWear;
	PartsMock defParts;
	PartsMock crfParts;
	BowInstanceMock defBowA;
	BowInstanceMock defBowB;
	BowInstanceMock crfBowA;
	WearInstanceMock defWearA;
	WearInstanceMock crfWearA;
	WearInstanceMock crfWearB;
	PartsInstanceMock defPartsA;
	PartsInstanceMock defPartsB;
	PartsInstanceMock crfPartsA;


	// [SetUp]
	public void SGMSetup(){
		/* setting up

		*/
			InstantiateAndSetupSGs();
			CheckSGMIsAssigned();
			SetupScroller();
			CheckWakeupCommandAssigned();
			AssignFilters();
			AssignSorter();
			CheckCreateSlotsCommandAssigned();
			CheckCreateSbsCommandAssigned();
			AssignUpdateEquipStatusCommand();
			SetInventory();
			SetupInventoryItemInstances();
			AddItemsToInventory();
			SetSGFields();
			SetSGMFields();
			ValidateSlotsAreSetForNonExpandable();
			CheckInitItemsCommandsAreSet();
			CheckInvItemsAreNotEquipped();
		/*	Initialization
			performed when snece loads
		*/
			sgm.Initialize();
			
			CheckItemsAreFilteredIn();
			CheckItemsAreOrdered();
			CheckSlotsAreReady();		
			CheckSlottablesAreCreatedAndInvInstAssigned();
			CheckItemsEquipStatus();
			// CheckEquippedSbsStates();
			
		/*	Focus
			performed when the widget gets focused
		*/
			CheckSGStatesInitialized();
			CheckSbsStatesInitialized();
			CheckInitiallyFocusedSGisAssignedInSGM();
			CheckFocusCommandsAreAssigned();
			CheckDefocusCommandsAreAssigned();

			sgm.Focus();
			
			CheckSGStatesUpdated();
			CheckSbsStatesUpdated();
		/*	switching pool sg
		*/
			TestSwitchingPoolSG();
		/*	prepick filter
		*/
	}
	
	public void Test(){}

	public void CheckInitiallyFocusedSGisAssignedInSGM(){
		Assert.That(sgm.InitiallyFocusedSG, Is.EqualTo(sgpAll));
	}
	public void CheckFocusCommandsAreAssigned(){
		Assert.That(sgpAll.UpdateSbStateCommand.GetType(), Is.EqualTo(typeof(SGUpdateSbStateCommand)));
		Assert.That(sgpBow.UpdateSbStateCommand.GetType(), Is.EqualTo(typeof(SGUpdateSbStateCommand)));
		Assert.That(sgWear.UpdateSbStateCommand.GetType(), Is.EqualTo(typeof(SGUpdateSbStateCommand)));
		Assert.That(sgBow.UpdateSbStateCommand.GetType(), Is.EqualTo(typeof(SGUpdateSbStateCommand)));
		Assert.That(sgWear.UpdateSbStateCommand.GetType(), Is.EqualTo(typeof(SGUpdateSbStateCommand)));
	}
	public void CheckDefocusCommandsAreAssigned(){
		Assert.That(sgpAll.DefocusCommand.GetType(), Is.EqualTo(typeof(SGDefocusCommand)));
		Assert.That(sgpBow.DefocusCommand.GetType(), Is.EqualTo(typeof(SGDefocusCommand)));
		Assert.That(sgpWear.DefocusCommand.GetType(), Is.EqualTo(typeof(SGDefocusCommand)));
		Assert.That(sgBow.DefocusCommand.GetType(), Is.EqualTo(typeof(SGDefocusCommand)));
		Assert.That(sgWear.DefocusCommand.GetType(), Is.EqualTo(typeof(SGDefocusCommand)));
	}
	public void TestSwitchingPoolSG(){
		sgpBow.Focus();
		AssertSGFocused(sgpBow);
		AssertSGDefocused(sgpAll);
		AssertSGDefocused(sgpWear);
		sgpWear.Focus();
		AssertSGFocused(sgpWear);
		AssertSGDefocused(sgpBow);
		AssertSGDefocused(sgpAll);
		sgpAll.Focus();
		AssertSGFocused(sgpAll);
		AssertSGDefocused(sgpBow);
		AssertSGDefocused(sgpWear);
		
		sgBow.Defocus();
		AssertSGDefocused(sgBow);
		sgWear.Defocus();
		AssertSGDefocused(sgWear);
		sgBow.Focus();
		AssertSGFocused(sgBow);
		sgWear.Focus();
		AssertSGFocused(sgWear);
		
	}
	
	public void CheckSbsStatesUpdated(){
		AssertSlottableStateAfterFocused(sgpAll);
		AssertSlottableStateAfterFocused(sgpBow);
		AssertSlottableStateAfterFocused(sgpWear);
		AssertSlottableStateAfterFocused(sgBow);
		AssertSlottableStateAfterFocused(sgWear);
		
	}
	public void CheckSbsStatesInitialized(){
		foreach(Slot slot in sgpAll.Slots){
			if(slot.Sb != null){
				
				AssertSBState(slot.Sb, Slottable.DeactivatedState);
			}
		}
		foreach(Slot slot in sgpBow.Slots){
			if(slot.Sb != null){
				
				AssertSBState(slot.Sb, Slottable.DeactivatedState);
			}
		}
		foreach(Slot slot in sgpWear.Slots){
			if(slot.Sb != null){
				
				AssertSBState(slot.Sb, Slottable.DeactivatedState);
			}
		}
		foreach(Slot slot in sgBow.Slots){
			if(slot.Sb != null){
				
				AssertSBState(slot.Sb, Slottable.DeactivatedState);
			}
		}
		foreach(Slot slot in sgWear.Slots){
			if(slot.Sb != null){
				
				AssertSBState(slot.Sb, Slottable.DeactivatedState);
			}
		}
		
	}
	
	public void CheckSGStatesInitialized(){
	
		AssertSGState(sgpAll, SlotGroup.DeactivatedState);
		AssertSGState(sgpBow, SlotGroup.DeactivatedState);
		AssertSGState(sgpWear, SlotGroup.DeactivatedState);
		AssertSGState(sgBow, SlotGroup.DeactivatedState);
		AssertSGState(sgpWear, SlotGroup.DeactivatedState);
	}	
	public void CheckSGStatesUpdated(){
		
		Assert.That(sgpAll.CurState, Is.EqualTo(SlotGroup.FocusedState));
		
		Assert.That(sgpBow.CurState, Is.EqualTo(SlotGroup.DefocusedState));
		
		Assert.That(sgpWear.CurState, Is.EqualTo(SlotGroup.DefocusedState));
		
		Assert.That(sgBow.CurState, Is.EqualTo(SlotGroup.FocusedState));
		
		Assert.That(sgWear.CurState, Is.EqualTo(SlotGroup.FocusedState));
	}
	public void CheckSGMIsAssigned(){
		Assert.That(sgpAll.SGM, Is.Not.Null);
		Assert.That(sgpBow.SGM, Is.Not.Null);
		Assert.That(sgpWear.SGM, Is.Not.Null);
		Assert.That(sgBow.SGM, Is.Not.Null);
		Assert.That(sgWear.SGM, Is.Not.Null);
	}
	
	public void CheckEquippedSbsStates(){
		Assert.That(sgpAll.Slots, Is.Not.Null);
		foreach(Slot slot in sgpAll.Slots){
			InventoryItemInstanceMock invInst = (InventoryItemInstanceMock)slot.Sb.Item;
			if(invInst.IsEquipped)
				AssertSBState(slot.Sb, Slottable.EquippedAndDeselectedState);
			else
				AssertSBState(slot.Sb, Slottable.DeactivatedState);
		}
		foreach(Slot slot in sgpBow.Slots){
			InventoryItemInstanceMock invInst = (InventoryItemInstanceMock)slot.Sb.Item;
			if(invInst.IsEquipped)
				AssertSBState(slot.Sb, Slottable.EquippedAndDeselectedState);
			else
				AssertSBState(slot.Sb, Slottable.DeactivatedState);
		}
		foreach(Slot slot in sgpWear.Slots){
			InventoryItemInstanceMock invInst = (InventoryItemInstanceMock)slot.Sb.Item;
			if(invInst.IsEquipped)
				AssertSBState(slot.Sb, Slottable.EquippedAndDeselectedState);
			else
				AssertSBState(slot.Sb, Slottable.DeactivatedState);
		}
		foreach(Slot slot in sgBow.Slots){
			InventoryItemInstanceMock invInst = (InventoryItemInstanceMock)slot.Sb.Item;
			if(invInst.IsEquipped)
				AssertSBState(slot.Sb, Slottable.EquippedAndDeselectedState);
			else
				AssertSBState(slot.Sb, Slottable.DeactivatedState);
		}
		foreach(Slot slot in sgWear.Slots){
			InventoryItemInstanceMock invInst = (InventoryItemInstanceMock)slot.Sb.Item;
			if(invInst.IsEquipped)
				AssertSBState(slot.Sb, Slottable.EquippedAndDeselectedState);
			else
				AssertSBState(slot.Sb, Slottable.DeactivatedState);
		}
	}
	public void CheckSlottablesStateReflectEquippedStatus(){

	}
	public void AssignUpdateEquipStatusCommand(){
		sgpAll.UpdateEquipStatusCommand = new UpdateEquipStatusForPoolCommmand();
		sgpBow.UpdateEquipStatusCommand = new UpdateEquipStatusForPoolCommmand();
		sgpWear.UpdateEquipStatusCommand = new UpdateEquipStatusForPoolCommmand();
		sgBow.UpdateEquipStatusCommand = new UpdateEquipStatusForEquipSGCommand();
		sgWear.UpdateEquipStatusCommand = new UpdateEquipStatusForEquipSGCommand();

		Assert.That(sgpAll.UpdateEquipStatusCommand.GetType(), Is.EqualTo(typeof(UpdateEquipStatusForPoolCommmand)));
		Assert.That(sgpBow.UpdateEquipStatusCommand.GetType(), Is.EqualTo(typeof(UpdateEquipStatusForPoolCommmand)));
		Assert.That(sgpWear.UpdateEquipStatusCommand.GetType(), Is.EqualTo(typeof(UpdateEquipStatusForPoolCommmand)));
		Assert.That(sgBow.UpdateEquipStatusCommand.GetType(), Is.EqualTo(typeof(UpdateEquipStatusForEquipSGCommand)));
		Assert.That(sgWear.UpdateEquipStatusCommand.GetType(), Is.EqualTo(typeof(UpdateEquipStatusForEquipSGCommand)));
	}
	
	public void CheckInvItemsAreNotEquipped(){
		Assert.That(defBowA.IsEquipped, Is.False);
		Assert.That(defBowB.IsEquipped, Is.False);
		Assert.That(crfBowA.IsEquipped, Is.False);
		Assert.That(defWearA.IsEquipped, Is.False);
		Assert.That(crfWearA.IsEquipped, Is.False);
		Assert.That(crfWearB.IsEquipped, Is.False);
	}
	
	public void CheckItemsEquipStatus(){
		// check proper UpdateEquipStatusCommand is assigned for each sgs before checking this
		InventoryItemInstanceMock equippedBowInst = (InventoryItemInstanceMock)sgBow.Slots[0].Sb.Item;
		Assert.That(equippedBowInst.IsEquipped, Is.True);
		InventoryItemInstanceMock equippedWearInst = (InventoryItemInstanceMock)sgWear.Slots[0].Sb.Item;
		Assert.That(equippedWearInst.IsEquipped, Is.True);

		foreach(Slot slot in sgpAll.Slots){
			InventoryItemInstanceMock invInst = (InventoryItemInstanceMock)slot.Sb.Item;
			if(object.ReferenceEquals(equippedBowInst, invInst) || object.ReferenceEquals(equippedWearInst,invInst))
				Assert.That(((InventoryItemInstanceMock)slot.Sb.Item).IsEquipped, Is.True);
			else
				Assert.That(((InventoryItemInstanceMock)slot.Sb.Item).IsEquipped, Is.False);
		}
	}
	
	public void CheckSlottablesAreCreatedAndInvInstAssigned(){
		
		AssertSlottables(sgpAll);
		AssertSlottables(sgpBow);
		AssertSlottables(sgpWear);
		AssertSlottables(sgBow);
		AssertSlottables(sgWear);
	}
	public void CheckCreateSbsCommandAssigned(){
		Assert.That(sgpAll.CreateSbsCommand.GetType(), Is.EqualTo(typeof(ConcCreateSbsCommand)));
		Assert.That(sgpBow.CreateSbsCommand.GetType(), Is.EqualTo(typeof(ConcCreateSbsCommand)));
		Assert.That(sgpWear.CreateSbsCommand.GetType(), Is.EqualTo(typeof(ConcCreateSbsCommand)));
		Assert.That(sgBow.CreateSbsCommand.GetType(), Is.EqualTo(typeof(ConcCreateSbsCommand)));
		Assert.That(sgWear.CreateSbsCommand.GetType(), Is.EqualTo(typeof(ConcCreateSbsCommand)));
	}
	
	public void CheckSlotsAreReady(){
		Assert.That(sgpAll.Slots.Count, Is.EqualTo(9));
		Assert.That(sgpBow.Slots.Count, Is.EqualTo(3));
		Assert.That(sgpWear.Slots.Count, Is.EqualTo(3));
		Assert.That(sgBow.Slots.Count, Is.EqualTo(1));
		Assert.That(sgWear.Slots.Count, Is.EqualTo(1));

	}	
	public void CheckCreateSlotsCommandAssigned(){

		Assert.That(sgpAll.CreateSlotsCommand.GetType(), Is.EqualTo(typeof(ConcCreateSlotsCommand)));
		Assert.That(sgpBow.CreateSlotsCommand.GetType(), Is.EqualTo(typeof(ConcCreateSlotsCommand)));
		Assert.That(sgpWear.CreateSlotsCommand.GetType(), Is.EqualTo(typeof(ConcCreateSlotsCommand)));
		Assert.That(sgBow.CreateSlotsCommand.GetType(), Is.EqualTo(typeof(ConcCreateSlotsCommand)));
		Assert.That(sgWear.CreateSlotsCommand.GetType(), Is.EqualTo(typeof(ConcCreateSlotsCommand)));
	}
	
	public void CheckItemsAreOrdered(){
		Assert.That(sgpAll.ItemInstances, Is.Ordered);
		Assert.That(sgpBow.ItemInstances, Is.Ordered);
		Assert.That(sgpWear.ItemInstances, Is.Ordered);
		Assert.That(sgBow.ItemInstances, Is.Ordered);
		Assert.That(sgWear.ItemInstances, Is.Ordered);
	}
	public void AssignSorter(){
		SGItemIndexSorter indexSorter = new SGItemIndexSorter();
		sgpAll.Sorter = indexSorter;
		sgpBow.Sorter = indexSorter;
		sgpWear.Sorter = indexSorter;
		sgBow.Sorter = indexSorter;
		sgWear.Sorter = indexSorter;

		Assert.That(sgpAll.Sorter, Is.EqualTo(indexSorter));
		Assert.That(sgpBow.Sorter, Is.EqualTo(indexSorter));
		Assert.That(sgpWear.Sorter, Is.EqualTo(indexSorter));
		Assert.That(sgBow.Sorter, Is.EqualTo(indexSorter));
		Assert.That(sgWear.Sorter, Is.EqualTo(indexSorter));
	}
	
	public void CheckItemsAreFilteredIn(){

		Assert.That(sgpAll.ItemInstances.Count, Is.EqualTo(9));
		
		Assert.That(sgpBow.ItemInstances.Count, Is.EqualTo(3));
		Assert.That(sgpWear.ItemInstances.Count, Is.EqualTo(3));
		
		Assert.That(sgBow.ItemInstances.Count, Is.EqualTo(1));
		Assert.That(sgWear.ItemInstances.Count, Is.EqualTo(1));


	}
	
	public void CheckSlottableItemsOrdering(){
	
		Assert.That(defBowA.CompareTo(defBowB), Is.EqualTo(0));
		Assert.That(defBowA.CompareTo(crfBowA), Is.LessThan(0));

		Assert.That(defWearA.CompareTo(crfWearA), Is.LessThan(0));
		Assert.That(crfWearA.CompareTo(crfWearB), Is.EqualTo(0));

		Assert.That(defPartsA.CompareTo(defPartsB), Is.EqualTo(0));
		Assert.That(defPartsA.CompareTo(crfPartsA), Is.LessThan(0));


		Assert.That(defBowA.CompareTo(crfBowA), Is.LessThan(0));
		Assert.That(crfBowA.CompareTo(defWearA), Is.LessThan(0));
		Assert.That(defWearA.CompareTo(crfWearA), Is.LessThan(0));
		Assert.That(crfWearA.CompareTo(defPartsA), Is.LessThan(0));
		Assert.That(defPartsA.CompareTo(crfPartsA), Is.LessThan(0));


	}
	
	public void CheckInitItemsCommandsAreSet(){

		AssertCommand<SGInitItemsCommand>(sgpAll.InitItemsCommand);
		AssertCommand<SGInitItemsCommand>(sgpBow.InitItemsCommand);
		AssertCommand<SGInitItemsCommand>(sgpWear.InitItemsCommand);
		AssertCommand<SGInitItemsCommand>(sgBow.InitItemsCommand);
		AssertCommand<SGInitItemsCommand>(sgWear.InitItemsCommand);
	}
	public void SetSGFields(){

		sgpAll.IsShrinkable = true;
		sgpBow.IsShrinkable = true;
		sgpWear.IsShrinkable = true;
		sgBow.IsShrinkable = false;
		sgWear.IsShrinkable = false;

		sgpAll.IsExpandable = true;
		sgpBow.IsExpandable = true;
		sgpWear.IsExpandable = true;
		sgBow.IsExpandable = false;
		sgWear.IsExpandable = false;
	}
	public void ValidateSlotsAreSetForNonExpandable(){

		Slot newBowSlot = new Slot();
		newBowSlot.Position = Vector2.zero;
		Slot newWearSlot = new Slot();
		newWearSlot.Position = Vector2.zero;
		Assert.That(sgBow.Slots.Count, Is.EqualTo(0));
		Assert.That(sgWear.Slots.Count, Is.EqualTo(0));

		sgBow.Slots.Add(newBowSlot);
		sgWear.Slots.Add(newWearSlot);

		Assert.That(sgBow.Slots.Count, Is.EqualTo(1));
		Assert.That(sgWear.Slots.Count, Is.EqualTo(1));	
	}
	
	public void AddItemsToInventory(){
		sgpAll.Inventory.Add(crfBowA);
		sgpAll.Inventory.Add(defBowA);
		sgpAll.Inventory.Add(defBowB);
		sgpAll.Inventory.Add(defWearA);
		sgpAll.Inventory.Add(crfWearB);
		sgpAll.Inventory.Add(crfWearA);
		sgpAll.Inventory.Add(defPartsA);
		sgpAll.Inventory.Add(crfPartsA);
		sgpAll.Inventory.Add(defPartsB);

		sgWear.Inventory.Add(defWearA);
		sgBow.Inventory.Add(defBowA);

		Assert.That(sgpBow.Inventory.Items.Count, Is.EqualTo(9));
		Assert.That(sgpWear.Inventory.Items.Count, Is.EqualTo(9));
		Assert.That(sgWear.Inventory.Items.Count, Is.EqualTo(2));

	}
	public void SetupInventoryItemInstances(){
		defBow = new BowMock();
		defBow.ItemID = 0;

		crfBow = new BowMock();
		crfBow.ItemID = 1;

		defWear = new WearMock();
		defWear.ItemID = 100;

		crfWear = new WearMock();
		crfWear.ItemID = 101;

		defParts = new PartsMock();
		defParts.ItemID = 600;

		crfParts = new PartsMock();
		crfParts.ItemID = 601;

		defBowA = new BowInstanceMock();
		defBowA.Item = defBow;

		defBowB = new BowInstanceMock();
		defBowB.Item = defBow;

		crfBowA = new BowInstanceMock();
		crfBowA.Item = crfBow;

		defWearA = new WearInstanceMock();
		defWearA.Item = defWear;

		crfWearA = new WearInstanceMock();
		crfWearA.Item = crfWear;

		crfWearB = new WearInstanceMock();
		crfWearB.Item = crfWear;

		defPartsA = new PartsInstanceMock();
		defPartsA.Quantity = 10;
		defPartsA.Item = defParts;
		
		defPartsB = new PartsInstanceMock();
		defPartsB.Quantity = 10;
		defPartsB.Item = defParts;

		crfPartsA = new PartsInstanceMock();
		crfPartsA.Quantity = 10;
		crfPartsA.Item = crfParts;

		Assert.That(defBowA.Quantity, Is.EqualTo(1));
		Assert.That(defBowB.Quantity, Is.EqualTo(1));
		Assert.That(crfBowA.Quantity, Is.EqualTo(1));
		Assert.That(defWearA.Quantity, Is.EqualTo(1));
		Assert.That(crfWearA.Quantity, Is.EqualTo(1));
		Assert.That(crfWearB.Quantity, Is.EqualTo(1));
		Assert.That(defPartsA.Quantity, Is.EqualTo(10));
		Assert.That(defPartsB.Quantity, Is.EqualTo(10));
		Assert.That(crfPartsA.Quantity, Is.EqualTo(10));

		
		bool objectEquality = ((object)(defBowA.Item)).Equals((object)(defBowB.Item));//object.Equals
		bool invItemEquality = defBowA.Item.Equals(defBowB.Item);//InventoryItem equality
		bool invItemInstInequality = !defBowA.Equals(defBowB);
		bool invItemInstEquality = defPartsA.Equals(defPartsB);
		bool partsInequality = defPartsA.Equals(crfPartsA);
		Assert.That(objectEquality, Is.True);
		Assert.That(invItemEquality, Is.True);
		Assert.That(invItemInstInequality, Is.True);
		Assert.That(invItemInstEquality, Is.True);
		Assert.That(partsInequality, Is.False);
		

	}
	
	public void InstantiateAndSetupSGs(){

		sgmGO = new GameObject();
		sgm = sgmGO.AddComponent<SlotGroupManager>();
		Assert.That(sgm, Is.Not.Null);

		sgpAllGO = new GameObject();
		sgpAll = sgpAllGO.AddComponent<SlotGroup>();
		sgpBowGO = new GameObject();
		sgpBow = sgpBowGO.AddComponent<SlotGroup>();
		sgpWearGO = new GameObject();
		sgpWear = sgpWearGO.AddComponent<SlotGroup>();
		sgBowGO = new GameObject();
		sgBow = sgBowGO.AddComponent<SlotGroup>();
		sgWearGO = new GameObject();
		sgWear = sgWearGO.AddComponent<SlotGroup>();
		Assert.That(sgpAll, Is.Not.Null);
		Assert.That(sgpBow, Is.Not.Null);
		Assert.That(sgpWear, Is.Not.Null);
		Assert.That(sgBow, Is.Not.Null);
		Assert.That(sgWear, Is.Not.Null);
		
		sgm.SetSG(sgpAll);
		sgm.SetSG(sgpBow);
		sgm.SetSG(sgpWear);
		sgm.SetSG(sgBow);
		sgm.SetSG(sgWear);

		Assert.That(sgm.SlotGroups.Contains(sgpAll), Is.True);
		Assert.That(sgm.SlotGroups.Contains(sgpBow), Is.True);
		Assert.That(sgm.SlotGroups.Contains(sgpWear), Is.True);
		Assert.That(sgm.SlotGroups.Contains(sgBow), Is.True);
		Assert.That(sgm.SlotGroups.Contains(sgWear), Is.True);

		AssertSGState<SGDeactivatedState>(sgpAll);	
		AssertSGState<SGDeactivatedState>(sgpBow);
		AssertSGState<SGDeactivatedState>(sgpWear);
		AssertSGState<SGDeactivatedState>(sgBow);
		AssertSGState<SGDeactivatedState>(sgWear);

	}
	public void SetupScroller(){

		scrollerGO = new GameObject();
		scroller = scrollerGO.AddComponent<AxisScrollerMock>();
		
		sgpAll.Scroller = scroller;
		sgpBow.Scroller = scroller;
		sgpWear.Scroller = scroller;
		
		Assert.That(sgpAll.Scroller, Is.Not.Null);
		Assert.That(sgpBow.Scroller, Is.Not.Null);
		Assert.That(sgpWear.Scroller, Is.Not.Null);
		Assert.That(sgBow.Scroller, Is.Null);
		Assert.That(sgWear.Scroller, Is.Null);
	}
	public void CheckWakeupCommandAssigned(){

		Assert.That(sgpAll.WakeUpCommand.GetType(), Is.EqualTo(typeof(SGWakeupCommand)));
		Assert.That(sgpBow.WakeUpCommand.GetType(), Is.EqualTo(typeof(SGWakeupCommand)));
		Assert.That(sgpWear.WakeUpCommand.GetType(), Is.EqualTo(typeof(SGWakeupCommand)));
		Assert.That(sgBow.WakeUpCommand.GetType(), Is.EqualTo(typeof(SGWakeupCommand)));
		Assert.That(sgWear.WakeUpCommand.GetType(), Is.EqualTo(typeof(SGWakeupCommand)));
	}
	public void AssignFilters(){
		sgpAll.Filter = new SGNullFilter();
		sgpBow.Filter = new SGBowFilter();
		sgpWear.Filter = new SGWearFilter();
		sgBow.Filter = new SGBowFilter();
		sgWear.Filter = new SGWearFilter();
	}
	
	public void SetInventory(){

			PoolInventory poolInventory = new PoolInventory();
			sgpAll.SetInventory(poolInventory);
			sgpBow.SetInventory(poolInventory);
			sgpWear.SetInventory(poolInventory);

			EquipmentSetInventory equipmentSetA = new EquipmentSetInventory();
			sgBow.SetInventory(equipmentSetA);
			sgWear.SetInventory(equipmentSetA);

			Assert.That(sgpAll.Inventory, Is.Not.Null);
			Assert.That(sgpBow.Inventory, Is.Not.Null);
			Assert.That(sgpWear.Inventory, Is.Not.Null);
			Assert.That(sgBow.Inventory, Is.Not.Null);
			Assert.That(sgWear.Inventory, Is.Not.Null);

			Assert.That(sgpAll.Inventory.GetType(), Is.EqualTo(typeof(PoolInventory)));
			Assert.That(sgpBow.Inventory.GetType(), Is.EqualTo(typeof(PoolInventory)));
			Assert.That(sgpWear.Inventory.GetType(), Is.EqualTo(typeof(PoolInventory)));
			Assert.That(sgBow.Inventory.GetType(), Is.EqualTo(typeof(EquipmentSetInventory)));
			Assert.That(sgWear.Inventory.GetType(), Is.EqualTo(typeof(EquipmentSetInventory)));
	}

	public void WakeupElements(){
		//Activation
		
		Assert.That(sgpAll.Filter, Is.Not.Null);
		Assert.That(sgpBow.Filter, Is.Not.Null);
		Assert.That(sgpWear.Filter, Is.Not.Null);
		Assert.That(sgBow.Filter, Is.Not.Null);
		Assert.That(sgWear.Filter, Is.Not.Null);

		Assert.That(sgpAll.Filter.GetType(), Is.EqualTo(typeof(SGNullFilter)));
		Assert.That(sgpBow.Filter.GetType(), Is.EqualTo(typeof(SGBowFilter)));
		Assert.That(sgpWear.Filter.GetType(), Is.EqualTo(typeof(SGWearFilter)));
		Assert.That(sgBow.Filter.GetType(), Is.EqualTo(typeof(SGBowFilter)));
		Assert.That(sgWear.Filter.GetType(), Is.EqualTo(typeof(SGWearFilter)));

		sgm.Focus();//this is called when elements the sgm holds are about to start interaction

		AssertSGState<SGFocusedState>(sgpAll);
		AssertSGState<SGDefocusedState>(sgpBow);
		AssertSGState<SGDefocusedState>(sgpWear);
		AssertSGState<SGFocusedState>(sgBow);
		AssertSGState<SGFocusedState>(sgWear);
		
		
	}
	public void SetSGMFields(){
		sgm.InitiallyFocusedSG = sgpAll;
	}
	
	/* Assert Methods
	*/
		public void AssertSGState<T>(SlotGroup sg) where T: SlotGroupState{
			Assert.That(sg.CurState.GetType(), Is.EqualTo(typeof(T)));
		}
		public void AssertLog(MonoBehaviour logger, string str){
			if(logger is SlotGroupManager){
				SlotGroupManager sgm = (SlotGroupManager)logger;
				Assert.That(sgm.UTLog, Is.EqualTo(str));
			}else if(logger is SlotGroup){
				SlotGroup sg = (SlotGroup)logger;
				Assert.That(sg.UTLog, Is.EqualTo(str));
			}
			else return;
			
		}
		public void AssertItemQuantity(SlottableItem item, int quantity){
			Assert.That(item.Quantity, Is.EqualTo(quantity));
		}
		
		public void AssertSlotsCount(SlotGroup sg, int count){
			Assert.That(sg.Slots.Count, Is.EqualTo(count));
		}
		public void AssertCommand<T>(SlotGroupCommand sgCommand) where T: SlotGroupCommand{
			Assert.That(sgCommand.GetType(), Is.EqualTo(typeof(T)));
		}
		public void AssertSlottables(SlotGroup sg){
			for(int i = 0; i < sg.ItemInstances.Count; i++){
				Assert.That(sg.Slots[i].Sb, Is.Not.Null);
				Assert.That(sg.Slots[i].Sb.Item, Is.EqualTo(sg.ItemInstances[i]));
			}
		}
		public void AssertSlottableStateAfterFocused(SlotGroup sg){
			foreach(Slot slot in sg.Slots){
			if(slot.Sb != null){
				if(sg.CurState == SlotGroup.DefocusedState){
					Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DefocusedState));
				}else if(sg.CurState == SlotGroup.FocusedState){

					InventoryItemInstanceMock invItem = (InventoryItemInstanceMock)slot.Sb.Item;
					if(invItem.IsEquipped)
						Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.EquippedAndDeselectedState));
					else{
						if(invItem.Item is BowMock)
							Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.FocusedState));
						else if(invItem.Item is WearMock)
							Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.FocusedState));
						else if(invItem.Item is PartsMock)
							if(sg.Filter is SGPartsFilter)
								Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.FocusedState));
							else
								Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DefocusedState));
						}
					}
				}
			}
		}
		public void AssertSGState(SlotGroup sg, SlotGroupState sgState){
			Assert.That(sg.CurState, Is.EqualTo(sgState));
		}
		public void AssertSBState(Slottable sb, SlottableState sbState){
			Assert.That(sb.CurState, Is.EqualTo(sbState));
		}
		public void AssertSGFocused(SlotGroup sg){
			AssertSGState(sg, SlotGroup.FocusedState);
			foreach(Slot slot in sg.Slots){
				if(slot.Sb != null){
					InventoryItemInstanceMock invItem = (InventoryItemInstanceMock)slot.Sb.Item;
					if(invItem.IsEquipped)
						AssertSBState(slot.Sb, Slottable.EquippedAndDeselectedState);
					else if(!(sg.Filter is SGPartsFilter) && invItem is PartsInstanceMock)
						AssertSBState(slot.Sb, Slottable.DefocusedState);
					else
						AssertSBState(slot.Sb, Slottable.FocusedState);
				}
			}
		}
		public void AssertSGDefocused(SlotGroup sg){
			AssertSGState(sg, SlotGroup.DefocusedState);
			foreach(Slot slot in sg.Slots){
				if(slot.Sb != null){
					AssertSBState(slot.Sb, Slottable.DefocusedState);
				}
			}
		}
}
