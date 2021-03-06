﻿using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;

public class SlottableTest {


	GameObject sbGO;
	Slottable sb;

	PointerEventDataMock eventDataMock = new PointerEventDataMock();
	GameObject sgmGO;
	SlotGroupManager sgm;
	GameObject sgpAllGO;
	SlotGroup sgpAll;
	GameObject sgBowGO;
	SlotGroup sgBow;
	GameObject sgWearGO;
	SlotGroup sgWear;
	Slottable defBowBSB;

	[SetUp]
	public void Setup(){
		// InstantiateAndInitialize();
		// CreateAndSetSlottableItem();
		// sb.Delayed = true;
		// sb.FilteredInMock();//SetState(Slottable.FocusedState)


		sgmGO = new GameObject("SlotGroupManager");
		sgm = sgmGO.AddComponent<SlotGroupManager>();
		SGMCommand updateTSCommand = new UpdateTransactionCommand();
		sgm.SetUpdateTransactionCommand(updateTSCommand);
		/*	sgpAll
		*/
			sgpAllGO = new GameObject("PoolSlotGroup");
			sgpAll = sgpAllGO.AddComponent<SlotGroup>();
			sgm.SetSG(sgpAll);
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
			sgm.SetSG(sgBow);
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
			sgm.SetSG(sgWear);
			sgWear.Filter = new SGWearFilter();
			sgWear.Sorter = new SGItemIndexSorter();
			sgWear.UpdateEquipStatusCommand = new UpdateEquipStatusForEquipSGCommand();
			sgWear.SetInventory(equipInventory);
			sgWear.IsShrinkable = false;
			sgWear.IsExpandable = false;

			Slot wearSlot = new Slot();
			wearSlot.Position = Vector2.zero;
			sgWear.Slots.Add(wearSlot);

			
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
			PartsMock crfParts = new PartsMock();
			crfParts.ItemID = 602;
			
			PartsInstanceMock defPartsA = new PartsInstanceMock();
			defPartsA.Item = defParts;
			sgpAll.Inventory.Add(defPartsA);
			PartsInstanceMock defPartsB = new PartsInstanceMock();
			defPartsB.Item = defParts;
			sgpAll.Inventory.Add(defPartsB);
			PartsInstanceMock crfPartsA = new PartsInstanceMock();
			crfPartsA.Item = crfParts;
			sgpAll.Inventory.Add(crfPartsA);

		sgm.InitiallyFocusedSG = sgpAll;
		
				Assert.That(sgm.CurState, Is.EqualTo(SlotGroupManager.DeactivatedState));
				Assert.That(sgm.PrevState, Is.EqualTo(SlotGroupManager.DeactivatedState));
				Assert.That(sgm.SlotGroups.Count, Is.EqualTo(3));
		sgm.Initialize();
		sb = sgpAll.GetSlottable(defBowA);		
		sbGO = sb.gameObject;
			Assert.That(sb.SGM.CurState, Is.EqualTo(SlotGroupManager.DefocusedState));
			Assert.That(sb.SGM.PrevState, Is.EqualTo(SlotGroupManager.DeactivatedState));
		sgm.Focus();

		Assert.That(sb.SGM, Is.EqualTo(sgm));
		Assert.That(sgpAll.SGM, Is.EqualTo(sgm));
		Assert.That(sb.SGM.GetSlotGroup(sb), Is.EqualTo(sgpAll));

		Assert.That(sb.SGM.CurState, Is.EqualTo(SlotGroupManager.FocusedState));
		Assert.That(sb.SGM.PrevState, Is.EqualTo(SlotGroupManager.DefocusedState));

		/*	Assertions
		*/
			Assert.That(sgm.SlotGroups.Count, Is.EqualTo(3));
			Assert.That(sgpAll.CurrentState, Is.EqualTo(SlotGroup.FocusedState));
			Assert.That(sgBow.CurrentState, Is.EqualTo(SlotGroup.FocusedState));
			Assert.That(sgWear.CurrentState, Is.EqualTo(SlotGroup.FocusedState));

			Assert.That(sgBow.Slots.Count, Is.EqualTo(1));
				Assert.That(sgBow.GetSlottable(defBowA).CurState, Is.EqualTo(Slottable.EquippedState));
			Assert.That(sgWear.Slots.Count, Is.EqualTo(1));
				Assert.That(sgWear.GetSlottable(defWearA).CurState, Is.EqualTo(Slottable.EquippedState));
			Assert.That(sgpAll.Slots.Count, Is.EqualTo(9));
				Assert.That(sgpAll.GetSlottable(defBowA).CurState, Is.EqualTo(Slottable.EquippedState));
				Assert.That(sgpAll.GetSlottable(defBowB).CurState, Is.EqualTo(Slottable.FocusedState));
				Assert.That(sgpAll.GetSlottable(crfBowA).CurState, Is.EqualTo(Slottable.FocusedState));
				Assert.That(sgpAll.GetSlottable(defWearA).CurState, Is.EqualTo(Slottable.EquippedState));
				Assert.That(sgpAll.GetSlottable(defWearB).CurState, Is.EqualTo(Slottable.FocusedState));
				Assert.That(sgpAll.GetSlottable(crfWearA).CurState, Is.EqualTo(Slottable.FocusedState));
				Assert.That(sgpAll.GetSlottable(defPartsA).CurState, Is.EqualTo(Slottable.DefocusedState));
				Assert.That(sgpAll.GetSlottable(defPartsB).CurState, Is.EqualTo(Slottable.DefocusedState));
				Assert.That(sgpAll.GetSlottable(crfPartsA).CurState, Is.EqualTo(Slottable.DefocusedState));
		/*
		*/
			defBowBSB = sgpAll.GetSlottable(defBowB);
	}
	/*try this again after everything else*/
	public void TestSBSelectedState(){
		Assert.That(Slottable.SelectedState.GetType(), Is.EqualTo(typeof(SBSelectedState)));
		sb.SetState(Slottable.SelectedState);
		AssertState(sb, Slottable.SelectedState);
	}
	[Test]
	public void TestPickedUpAndSelectedState(){
		/* pick up
		*/
			/*	setup
			*/	
						
				// sb.SetState(Slottable.FocusedState);
				defBowBSB.OnPointerDownMock(eventDataMock);
				AssertState(defBowBSB, Slottable.WaitForPickUpState);
				
				defBowBSB.WaitAndPickUpProcess.Expire();
				
				AssertAction(defBowBSB, "WaitAndPickUpProcess done");
				Assert.That(defBowBSB.WaitAndPickUpProcess.IsRunning, Is.False);
				Assert.That(defBowBSB.WaitAndPickUpProcess.IsExpired, Is.True);
				
				AssertState(defBowBSB, Slottable.PickedUpAndSelectedState);
				
				defBowBSB.SGM.SimSBHover(defBowBSB, eventDataMock);
				defBowBSB.SGM.SimSGHover(defBowBSB.SGM.GetSlotGroup(defBowBSB), eventDataMock);
		/* enter
		*/
			Assert.That(defBowBSB.PickedUpAndSelectedProcess.IsRunning, Is.True);
			Assert.That(defBowBSB.PickedUpAndSelectedProcess.IsExpired, Is.False);
			
			Assert.That(defBowBSB.SGM.CurState, Is.EqualTo(SlotGroupManager.ProbingState));
			Assert.That(defBowBSB.SGM.ProbingStateProcess.IsRunning, Is.True);
			Assert.That(defBowBSB.SGM.ProbingStateProcess.IsExpired, Is.False);
			//SGM fields set properly
				Assert.That(defBowBSB.SGM.SelectedSB, Is.EqualTo(defBowBSB));
				Assert.That(defBowBSB.SGM.SelectedSG, Is.EqualTo(defBowBSB.SGM.GetSlotGroup(defBowBSB)));
				Assert.That(defBowBSB.SGM.PickedSB, Is.EqualTo(defBowBSB));
			//transaction
				Assert.That(defBowBSB.SGM.UpdateTransactionCommand.GetType(), Is.EqualTo(typeof(UpdateTransactionCommand)));
				
				Assert.That(defBowBSB.SGM.Transaction.GetType(), Is.EqualTo(typeof(RevertTransaction)));
			
		/*	post pick filtering
			after this is done then test hovering on defocused entities to validate SGM does not update its Selected fields
		*/
			// Assert.That(sgWear.CurrentState, Is.EqualTo(SlotGroup.DefocusedState));
			// foreach(Slot slot in sgWear.Slots){
			// 	if(slot.Sb != null){
			// 		Assert.That(slot.Sb.CurState, Is.EqualTo(Slottable.DefocusedState));
			// 	}
			// }
		/*	checkTransaction
		*/
		/*	hover and dehover
		*/
		/*	expire
		*/
		// sb.PickedUpAndSelectedProcess.Expire();
		// AssertAction("PickedUpAndSelectedProcess done");
		// Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.False);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.True);
		// AssertState(sb, Slottable.PickedUpAndSelectedState);
		// /*	abort
		// */
		// sb.SetState(Slottable.FocusedState);
		// AssertState(sb, Slottable.FocusedState);
		// sb.OnPointerDownMock(eventDataMock);
		// AssertState(sb, Slottable.WaitForPickUpState);
		// sb.WaitAndPickUpProcess.Expire();
		// AssertAction("WaitAndPickUpProcess done");
		// Assert.That(sb.WaitAndPickUpProcess.IsRunning, Is.False);
		// Assert.That(sb.WaitAndPickUpProcess.IsExpired, Is.True);
		// AssertState(sb, Slottable.PickedUpAndSelectedState);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.True);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.False);

		// sb.SetState(Slottable.FocusedState);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.False);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.False);
		/*	OnDehovered
		*/
		//setting up
			sb.SetState(Slottable.FocusedState);
			AssertState(sb, Slottable.FocusedState);
			sb.OnPointerDownMock(eventDataMock);
			AssertState(sb, Slottable.WaitForPickUpState);
			sb.WaitAndPickUpProcess.Expire();
			AssertAction(sb,"WaitAndPickUpProcess done");
			Assert.That(sb.WaitAndPickUpProcess.IsRunning, Is.False);
			Assert.That(sb.WaitAndPickUpProcess.IsExpired, Is.True);
			AssertState(sb, Slottable.PickedUpAndSelectedState);
			Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.True);
			Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.False);
		//
		// sb.OnDehoveredMock(eventDataMock);
		// AssertState(sb, Slottable.PickedUpAndDeselectedState);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsRunning, Is.False);
		// Assert.That(sb.PickedUpAndSelectedProcess.IsExpired, Is.False);
		// Assert.That(sb.SGM.SelectedSB, Is.Null);
		

	}
	public void TestWaitForPickUpState(){
		/*	entering
		*/
		sb.OnPointerDownMock(eventDataMock);
		AssertState(sb, Slottable.WaitForPickUpState);
		Assert.That(sb.WaitAndPickUpProcess.IsRunning, Is.True);
		Assert.That(sb.WaitAndPickUpProcess.IsExpired, Is.False);
		/*	expire
		*/
		sb.WaitAndPickUpProcess.Expire();
		AssertAction(sb,"WaitAndPickUpProcess done");
		AssertState(sb, Slottable.PickedUpAndSelectedState);
		Assert.That(sb.WaitAndPickUpProcess.IsRunning, Is.False);
		Assert.That(sb.WaitAndPickUpProcess.IsExpired, Is.True);
		/* abort, pointer up
		*/
		sb.SetState(Slottable.FocusedState);
		AssertState(sb, Slottable.FocusedState);
		sb.OnPointerDownMock(eventDataMock);
		AssertState(sb, Slottable.WaitForPickUpState);
		
		sb.OnPointerUpMock(eventDataMock);
		AssertState(sb, Slottable.WaitForNextTouchState);
		Assert.That(sb.WaitAndPickUpProcess.IsRunning, Is.False);
		Assert.That(sb.WaitAndPickUpProcess.IsExpired, Is.False);
		/*	OnEndDrag
		*/
		sb.SetState(Slottable.FocusedState);
		AssertState(sb, Slottable.FocusedState);
		sb.OnPointerDownMock(eventDataMock);
		AssertState(sb, Slottable.WaitForPickUpState);

		sb.OnEndDragMock(eventDataMock);
		AssertState(sb, Slottable.FocusedState);
		Assert.That(sb.WaitAndPickUpProcess.IsRunning, Is.False);
		Assert.That(sb.WaitAndPickUpProcess.IsExpired, Is.False);
		/*	OnPointerExit
		*/
		sb.SetState(Slottable.FocusedState);
		AssertState(sb, Slottable.FocusedState);
		sb.OnPointerDownMock(eventDataMock);
		AssertState(sb, Slottable.WaitForPickUpState);

		sb.OnDehoveredMock(eventDataMock);
		AssertState(sb, Slottable.FocusedState);
		Assert.That(sb.WaitAndPickUpProcess.IsRunning, Is.False);
		Assert.That(sb.WaitAndPickUpProcess.IsExpired, Is.False);
		


	}
	public void TestWaitForPointerUpState(){
		sb.SetState(Slottable.DefocusedState);
		
		sb.OnPointerDownMock(eventDataMock);
		AssertState(sb, Slottable.WaitForPointerUpState);
		Assert.That(sb.WaitAndSetBackToDefocusedStateProcess.IsRunning, Is.True);
		Assert.That(sb.WaitAndSetBackToDefocusedStateProcess.IsExpired, Is.False);
		sb.WaitAndSetBackToDefocusedStateProcess.Expire();
		AssertAction(sb,"WaitAndSetBackToDefocusedStateProcess done");
		Assert.That(sb.WaitAndSetBackToDefocusedStateProcess.IsRunning, Is.False);
		Assert.That(sb.WaitAndSetBackToDefocusedStateProcess.IsExpired, Is.True);
		AssertState(sb, Slottable.DefocusedState);
		
		/* tapping
		*/
		sb.OnPointerDownMock(eventDataMock);
		AssertState(sb, Slottable.WaitForPointerUpState);
		sb.OnPointerUpMock(eventDataMock);
		AssertAction(sb,"tapped");
		AssertState(sb, Slottable.DefocusedState);
		Assert.That(sb.WaitAndSetBackToDefocusedStateProcess.IsRunning, Is.False);
		Assert.That(sb.WaitAndSetBackToDefocusedStateProcess.IsExpired, Is.False);
		/*OnEndDrag
		*/
		sb.OnPointerDownMock(eventDataMock);
		AssertState(sb, Slottable.WaitForPointerUpState);
		sb.OnEndDragMock(eventDataMock);
		AssertState(sb, Slottable.DefocusedState);
		Assert.That(sb.WaitAndSetBackToDefocusedStateProcess.IsRunning, Is.False);
		Assert.That(sb.WaitAndSetBackToDefocusedStateProcess.IsExpired, Is.False);
		
		
	}
	
	public void TestFocusedState(){
		
		// sb.SetState(Slottable.MovingState);
		// sb.SetState(Slottable.WaitForNextTouchState);
		// sb.SetState(Slottable.DeactivatedState);
		// sb.SetState(Slottable.FocusedState);
		
		/*	Graying in
		*/
		/*	enter
		*/
			AssertState(sb, Slottable.FocusedState);
			AssertAction(sb,"InstantGrayin called");
			sb.SetState(Slottable.DefocusedState);
			sb.SetState(Slottable.FocusedState);
			Assert.That(sb.GradualGrayinProcess.IsRunning, Is.True);
			Assert.That(sb.GradualGrayinProcess.IsExpired, Is.False);
			/*	expire
			*/
			sb.GradualGrayinProcess.Expire();
			AssertAction(sb,"GradualGrayinProcess done");
			Assert.That(sb.GradualGrayinProcess.IsRunning, Is.False);
			Assert.That(sb.GradualGrayinProcess.IsExpired, Is.True);

			sb.SetState(Slottable.DefocusedState);
			sb.SetState(Slottable.FocusedState);
			/*	abort
			*/
			sb.SetState(Slottable.SelectedState);
			Assert.That(sb.GradualGrayinProcess.IsRunning, Is.False);
			Assert.That(sb.GradualGrayinProcess.IsExpired, Is.False);
		
		/*	Dehighlighting
		*/
			/* enter
			*/
			sb.SetState(Slottable.FocusedState);
			Assert.That(sb.GradualDehighlightProcess.IsRunning, Is.True);
			Assert.That(sb.GradualDehighlightProcess.IsExpired, Is.False);
			/*	expire
			*/
			sb.GradualDehighlightProcess.Expire();
			AssertAction(sb,"GradualDehighlightProcess done");
			Assert.That(sb.GradualDehighlightProcess.IsRunning, Is.False);
			Assert.That(sb.GradualDehighlightProcess.IsExpired, Is.True);
			sb.SetState(Slottable.SelectedState);
			Assert.That(sb.GradualDehighlightProcess.IsRunning, Is.False);
			Assert.That(sb.GradualDehighlightProcess.IsExpired, Is.True);
			/*	abort
			*/
			sb.SetState(Slottable.FocusedState);
			sb.SetState(Slottable.SelectedState);
			Assert.That(sb.GradualDehighlightProcess.IsRunning, Is.False);
			Assert.That(sb.GradualDehighlightProcess.IsExpired, Is.False);
		
		/*Pointer enter
		*/
		sb.SetState(Slottable.FocusedState);
		sb.GradualDehighlightProcess.Expire();
		
		
		// anotherSb.SetState(Slottable.PickedUpAndSelectedState);
		// eventDataMock.pointerDrag = anotherGO;
		sb.OnHoveredMock(eventDataMock);

		AssertState(sb, Slottable.SelectedState);
		/*	pointer down
		*/
		sb.SetState(Slottable.FocusedState);
		sb.GradualDehighlightProcess.Expire();
		AssertState(sb, Slottable.FocusedState);
		sb.OnPointerDownMock(eventDataMock);
		AssertState(sb, Slottable.WaitForPickUpState);


	}
	public void TestDefocusedState(){
		sb.SetState(Slottable.DefocusedState);
		AssertState(sb, Slottable.DefocusedState);
		Assert.That(sb.GradualGrayoutProcess.IsRunning, Is.True);
		Assert.That(sb.GradualGrayoutProcess.IsExpired, Is.False);
		sb.GradualGrayoutProcess.Expire();
		Assert.That(sb.GradualGrayoutProcess.IsRunning, Is.False);
		Assert.That(sb.GradualGrayoutProcess.IsExpired, Is.True);
		AssertAction(sb,"GradualGrayoutProcess done");
		AssertState(sb, Slottable.DefocusedState);
		sb.SetState(Slottable.DefocusedState);
		Assert.That(sb.GradualGrayoutProcess.IsRunning, Is.False);
		Assert.That(sb.GradualGrayoutProcess.IsExpired, Is.True);
		sb.SetState(Slottable.DeactivatedState);
		sb.SetState(Slottable.DefocusedState);
		AssertState(sb, Slottable.DefocusedState);
		Assert.That(sb.UTLog, Is.EqualTo("InstantGrayout called"));
		Assert.That(sb.GradualGrayoutProcess.IsRunning, Is.False);
		Assert.That(sb.GradualGrayoutProcess.IsExpired, Is.True);
		sb.SetState(Slottable.FocusedState);
		sb.SetState(Slottable.DefocusedState);
		Assert.That(sb.GradualGrayoutProcess.IsRunning, Is.True);
		Assert.That(sb.GradualGrayoutProcess.IsExpired, Is.False);
		sb.SetState(Slottable.DeactivatedState);
		Assert.That(sb.GradualGrayoutProcess.IsRunning, Is.False);
		Assert.That(sb.GradualGrayoutProcess.IsExpired, Is.False);
		AssertState(sb, Slottable.DeactivatedState);
		



		// AssertAction("InstantGrayout called");
		
		// sb.OnPointerDownMock(eventDataMock);
		// AssertState(sb, Slottable.WaitForPointerUpState);
	}
	
	public void TestDeactivatedState(){
		sb.SetState(Slottable.DeactivatedState);
		AssertState(sb, Slottable.DeactivatedState);
		sb.OnPointerDownMock(eventDataMock);
		AssertState(sb, Slottable.DeactivatedState);
		
	}
	
	public void InstantiateAndInitialize(){
		sbGO = new GameObject();
		sb = sbGO.AddComponent<Slottable>();
		sb.Initialize(sgpAll);

	}
	public void CreateAndSetSlottableItem(){
		
		BowMock defBow = new BowMock();
		defBow.ItemID = 0;
		BowInstanceMock defBowA = new BowInstanceMock();
		defBowA.Quantity = 1;
		defBowA.Item = defBow;

		sb.SetSlottableItem(defBowA);
	}
	/* legacy
	*/
		// public void CheckEquippedStateIsAssigned(){
		// 	Assert.That(Slottable.EquippedState, Is.Not.Null);
		// 	Assert.That(Slottable.EquippedState.GetType(), Is.EqualTo(typeof(EquippedState)));
		// }
		// public void SlottableStateTransitionTest(){
		// 	sb.OnPointerDownMock();
		// 	AssertState<WaitForPickUpState>();
		// 	sb.ExpirePickupTimer();
		// 	AssertState<PickedUpState>();
		// 	sb.ReleasedInside = false;
		// 	sb.OnPointerUpMock();
		// 	AssertState<FocusedState>();
		// 	Assert.That(sb.UTLog, Is.EqualTo("Reverted"));
		// 	sb.UTLog = "";

		// 	sb.OnPointerDownMock();
		// 	AssertState<WaitForPickUpState>();
		// 	sb.OnPointerUpMock();
		// 	AssertState<WaitForNextTouchState>();

		// 	sb.OnDeselectedMock();
		// 	Assert.That(sb.UTLog, Is.EqualTo("Canceled"));
		// 	sb.UTLog = "";
		// 	AssertState<FocusedState>();

		// 	sb.OnPointerDownMock();
		// 	AssertState<WaitForPickUpState>();
		// 	sb.OnPointerUpMock();
		// 	AssertState<WaitForNextTouchState>();
		// 	sb.ExpireTapTimer();
		// 	AssertState<FocusedState>();
		// 	Assert.That(sb.UTLog, Is.EqualTo("tapped"));
		// 	sb.UTLog = "";

		// 	sb.OnPointerDownMock();
		// 	AssertState<WaitForPickUpState>();
		// 	sb.OnPointerUpMock();
		// 	AssertState<WaitForNextTouchState>();
		// 	sb.OnPointerDownMock();
		// 	AssertState<PickedUpState>();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(1));

		// 	sb.OnEndDragMock();
		// 	Assert.That(sb.UTLog, Is.EqualTo("Reverted"));
		// 	sb.UTLog = "";
		// 	AssertState<FocusedState>();

		// 	sb.OnPointerDownMock();
		// 	AssertState<WaitForPickUpState>();
		// 	sb.OnEndDragMock();
		// 	AssertCanceled();
		// 	AssertState<FocusedState>();

		// 	sb.OnPointerDownMock();
		// 	AssertState<WaitForPickUpState>();
		// 	sb.OnPointerUpMock();
		// 	AssertState<WaitForNextTouchState>();
		// 	sb.OnPointerDownMock();
		// 	AssertState<PickedUpState>();
		// 	AssertPickQuantity(1);
		// }


		// public void RevertTest(){

		// 	sb.OnPointerDownMock();
		// 	sb.ExpirePickupTimer();
		// 	sb.ReleasedInside = false;
		// 	sb.OnPointerUpMock();
		// 	AssertState<FocusedState>();
		// 	Assert.That(sb.UTLog, Is.EqualTo("Reverted"));
		// 	sb.UTLog = "";

		// 	sb.OnPointerDownMock();
		// 	sb.ExpirePickupTimer();
		// 	sb.ReleasedInside = true;
		// 	sb.ExpireRevertTimer();

		// 	AssertState<FocusedState>();
		// 	Assert.That(sb.UTLog, Is.EqualTo("Reverted"));
		// 	sb.UTLog = "";

		// 	sb.OnPointerDownMock();
		// 	sb.ExpirePickupTimer();
		// 	sb.ReleasedInside = true;//now W4NTWPU
		// 	sb.OnPointerUpMock();
		// 	sb.OnDeselectedMock();
		// 	AssertState<FocusedState>();
		// 	Assert.That(sb.UTLog, Is.EqualTo("Reverted"));
		// 	sb.UTLog = "";
		// }
		// public void SlottableInstantiation(){

		// 	Assert.That(sb, Is.Not.Null);

		// 	// sb.Initialize();
		// 	// Assert.That(sb.IsInitialized, Is.True);
		// }
		// public void SlottableInitialization(){
		
		// 	// sb.Initialize();
			
			
		// 	Assert.That(Slottable.DeactivatedState, Is.Not.Null);
		// 	Assert.That(Slottable.DefocusedState, Is.Not.Null);
		// 	Assert.That(Slottable.FocusedState, Is.Not.Null);
		// 	bool assertBool = sb.CurState.GetType() == typeof(DeactivatedState);
		// 	Assert.That(assertBool, Is.True);

		// 	sb.SetState(Slottable.DefocusedState);
		// 	Assert.That(sb.PrevState.GetType(), Is.EqualTo(typeof(DeactivatedState)));
			
		// 	sb.SetState(Slottable.FocusedState);
		// 	Assert.That(sb.PrevState.GetType(), Is.EqualTo(typeof(DefocusedState)));
			
		// 	sb.SetState(Slottable.DeactivatedState);
		// 	Assert.That(sb.PrevState.GetType(), Is.EqualTo(typeof(FocusedState)));
			
		// 	Assert.That(Slottable.WaitForPickUpState, Is.Not.Null);
		// 	Assert.That(Slottable.WaitForNextTouchState, Is.Not.Null);
		// 	Assert.That(Slottable.PickedUpState, Is.Not.Null);
		// 	Assert.That(Slottable.WaitForNextTouchWhilePUState, Is.Not.Null);
		// 	Assert.That(Slottable.MovingState, Is.Not.Null);
		// }

		
		// public void SlottableOnPointerDown(){
		// 	sb.Delayed = true;
		// 	// sb.SetState(Slottable.FocusedState);
		// 	sb.FilteredInMock();
		// 	sb.OnPointerDownMock();

		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(WaitForPickUpState)));
		// 	Assert.That(sb.IsPickupTimerOn, Is.True);

		// 	sb.OnPointerUpMock();
		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(WaitForNextTouchState)));
		// 	Assert.That(sb.IsPickupTimerOn, Is.False);
		// 	Assert.That(sb.IsTapTimerOn, Is.True);

		// 	// sb.ExpirePickupTimer();
		// 	// Assert.That(sb.CurrentState.GetType(), Is.EqualTo(typeof(PickedUpState)));
		// 	// Assert.That(sb.IsPickupTimerOn, Is.False);
			
		// 	sb.ExpireTapTimer();
		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(FocusedState)));
		// 	Assert.That(sb.IsPickupTimerOn, Is.False);
		// 	Assert.That(sb.IsTapTimerOn, Is.False);
		// 	Assert.That(sb.UTLog, Is.EqualTo("tapped"));
		// 	sb.ClearLog();
		// 	Assert.That(sb.UTLog, Is.EqualTo(""));

		// 	// sb.FingerMoveOverThreshMock();
		// 	// Assert.That(sb.CurrentState.GetType(), Is.EqualTo(typeof(DefocusedState)));
		// 	// Assert.That(sb.IsPickupTimerOn, Is.False);
			
		// 	// sb.Delayed = false;
		// 	// sb.SetState(Slottable.FocusedState);
		// 	// sb.OnPointerDownMock();
		// 	// Assert.That(sb.CurrentState.GetType(), Is.EqualTo(typeof(PickedUpState)));

		// 	// sb.ExpireTimer();
		// 	// Assert.That(sb.CurrentState.GetType(), Is.EqualTo(typeof(DefocusedState)));

		// 	// sb.OnDeselectedMock();
		// 	// Assert.That(sb.CurrentState.GetType(), Is.EqualTo(typeof(DefocusedState)));
		// 	// Assert.That(sb.IsPickupTimerOn, Is.False);
		// }

		
		// public void SlottableMultiTouchTest(){
		// 	sb.Delayed = true;
		// 	sb.FilteredInMock();
		// 	sb.OnPointerDownMock();
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(PickedUpState)));
		// 	Assert.That(sb.IsPickupTimerOn, Is.False);
		// 	Assert.That(sb.IsTapTimerOn, Is.False);
		// 	Assert.That(sb.UTLog, Is.EqualTo(""));
		// }
		
		// public void SbIncrementTest(){
			
		// 	sb.OnPointerDownMock();
		// 	sb.ExpirePickupTimer();
		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(PickedUpState)));
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(1));
		// 	sb.ReleasedInside = true;
		// 	sb.OnPointerUpMock();
		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(WaitForNextTouchWhilePUState)));
		// 	Assert.That(sb.IsRevertTimerOn, Is.True);

		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(PickedUpState)));
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(2));
		// 	Assert.That(sb.UTLog, Is.EqualTo("Incremented"));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(3));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(4));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(5));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(6));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(7));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(8));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(9));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(10));
		// 	sb.OnPointerUpMock();
		// 	sb.OnPointerDownMock();
		// 	Assert.That(sb.PickedAmount, Is.EqualTo(10));
			
		// 	// sb.OnPointerDownMock();
		// 	// Assert.That(sb.PickedAmount, Is.EqualTo(2));
		// }
		

		
		// public void TestSlottableEquality(){
		// 	InventoryItemMock itemA = new InventoryItemMock();
		
		// 	itemA.IsStackable = true;
		// 	itemA.ItemID = 1;

		// 	InventoryItemMock itemB = new InventoryItemMock();
		
		// 	itemB.IsStackable = true;
		// 	itemB.ItemID = 1;

		// 	InventoryItemMock itemC = new InventoryItemMock();
		
		// 	itemC.IsStackable = false;
		// 	itemC.ItemID = 2;

		// 	bool objectEquality = itemA.Equals((object)itemB);
		// 	bool objectInequality = !itemA.Equals((object)itemC);
		// 	bool iEquatableEquality = itemA.Equals(itemB);
		// 	bool iEquatableInequality = !itemB.Equals(itemC);
		// 	bool operatorEquality = (itemA == itemB) && (itemA == itemA) && (itemB == itemB);
		// 	bool operatorInequality = (itemA != itemC) && (itemB != itemC);
		// 	bool hashEquality = (itemA.GetHashCode() == itemB.GetHashCode()) && (itemA.GetHashCode() == itemA.GetHashCode());
		// 	bool hashIequality = itemA.GetHashCode() != itemC.GetHashCode();

		// 	Assert.That(objectEquality, Is.True);
		// 	Assert.That(objectInequality, Is.True);
		// 	Assert.That(iEquatableEquality, Is.True);
		// 	Assert.That(iEquatableInequality, Is.True);
		// 	Assert.That(operatorEquality, Is.True);
		// 	Assert.That(operatorInequality, Is.True);
		// 	Assert.That(hashEquality, Is.True);
		// 	Assert.That(hashIequality, Is.True);
			
		// }

	public void AssertState<T>() where T: SlottableState{
		Assert.That(sb.CurState.GetType(), Is.EqualTo(typeof(T)));
	}
	public void AssertState(Slottable sb, SlottableState sbState){
		Assert.That(sb.CurState, Is.EqualTo(sbState));
	}
	public void AssertAction(Slottable sb, string str){
		Assert.That(sb.UTLog, Is.EqualTo(str));
	}
	public void AssertCanceled(){
		Assert.That(sb.UTLog, Is.EqualTo("Canceled"));
		sb.UTLog = "";
	}
	public void AssertPickQuantity(int quant){
		Assert.That(sb.PickedAmount, Is.EqualTo(quant));
	}
}
