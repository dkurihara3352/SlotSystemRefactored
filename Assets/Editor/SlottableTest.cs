using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections.Generic;
using Utility;
public class SlottableTest{
	public enum TestElement{SB, SG, SGM, TA}
	public class SlotSystemTestResult{
		public bool isPAS; public bool isTAS;
		public Slottable testingSB;
		public SlotGroup testTargetSG;
		public Slottable testTargetSB;
		public string Val;
		// public SlotSystemTestResult(bool isPAS, bool isTAS, Slottable pickedSB, SlotGroup hoveredSG,  int i){
			// 	this.testTargetSG = hoveredSG; this.testingSB = pickedSB;
			// 	this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			// 	this.tAS = "tAS: " + (isTAS?Util.Blue("on "):Util.Red("off "));
			// 	this.hoveredName = " on " + Util.SGName(hoveredSG);
			// 		if(hoveredSG.IsPool)
			// 			hoveredName = Util.Red(hoveredName);
			// 		else
			// 			hoveredName = Util.Blue(hoveredName);

			// 	this.pickedSBName = Util.SBName(pickedSB) + " of " + Util.SGName(pickedSB.SG);
			// 		if(pickedSB.SG.IsPool)
			// 			pickedSBName = Util.Red(pickedSBName);
			// 		else
			// 			pickedSBName = Util.Blue(pickedSBName);
			// 	this.Val = i.ToString();
			// }
			// public SlotSystemTestResult(bool isPAS, bool isTAS, Slottable pickedSB, SlotGroup hoveredSG, SlotSystemTransaction ta){
			// 	this.testTargetSG = hoveredSG; this.testingSB = pickedSB;
			// 	this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			// 	this.tAS = "tAS: " + (isTAS?Util.Blue("on "):Util.Red("off "));
			// 	this.hoveredName = " on " + Util.SGName(hoveredSG);
			// 		if(hoveredSG.IsPool)
			// 			hoveredName = Util.Red(hoveredName);
			// 		else
			// 			hoveredName = Util.Blue(hoveredName);

			// 	this.pickedSBName = Util.SBName(pickedSB) + " of " + Util.SGName(pickedSB.SG);
			// 		if(pickedSB.SG.IsPool)
			// 			pickedSBName = Util.Red(pickedSBName);
			// 		else
			// 			pickedSBName = Util.Blue(pickedSBName);
			// 		if(pickedSB.IsEquipped && pickedSB.SG.IsPool)
			// 			pickedSBName = Util.Bold(pickedSBName);
			// 	this.Val = ta.GetType().ToString();
			// 	if(ta.GetType() == typeof(FillEquipTransaction))
			// 		Val = Util.Terra(Val);
			// 	if(ta.GetType() == typeof(SwapTransaction))
			// 		Val = Util.Aqua(Val);
			// 	if(ta.GetType() == typeof(StackTransaction))
			// 		Val = Util.Forest(Val);
			// 	if(ta.GetType() == typeof(ReorderTransaction))
			// 		Val = Util.Berry(Val);
			// 	if(ta.GetType() == typeof(InsertTransaction))
			// 		Val = Util.Violet(Val);
			// }
			// public SlotSystemTestResult(bool isPAS, bool isTAS, Slottable pickedSB, SlotGroup sg, Slottable hoveredSB, SlotSystemTransaction ta){
			// 	this.testTargetSG = sg; this.testingSB = pickedSB; this.testTargetSB = hoveredSB;
			// 	this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			// 	this.tAS = "tAS: " + (isTAS?Util.Blue("on "):Util.Red("off "));

			// 	this.hoveredName = Util.SGName(sg);
			// 	string hoveredSBStr = Util.SBName(hoveredSB);
			// 		if(hoveredSB.SG.IsPool && hoveredSB.IsEquipped)
			// 			hoveredSBStr = Util.Bold(hoveredSBStr);
			// 	hoveredName = " on " + hoveredSBStr + " of " + hoveredName;
			// 		if(sg.IsPool)
			// 			hoveredName = Util.Red(hoveredName);
			// 		else
			// 			hoveredName = Util.Blue(hoveredName);

			// 	this.pickedSBName = Util.SBName(pickedSB) + " of " + Util.SGName(pickedSB.SG);
			// 		if(pickedSB.SG.IsPool)
			// 			pickedSBName = Util.Red(pickedSBName);
			// 		else
			// 			pickedSBName = Util.Blue(pickedSBName);
			// 		if(pickedSB.IsEquipped && pickedSB.SG.IsPool)
			// 			pickedSBName = Util.Bold(pickedSBName);
			// 	this.Val = ta.GetType().ToString();
			// 	if(ta.GetType() == typeof(FillEquipTransaction))
			// 		Val = Util.Terra(Val);
			// 	if(ta.GetType() == typeof(SwapTransaction))
			// 		Val = Util.Aqua(Val);
			// 	if(ta.GetType() == typeof(StackTransaction))
			// 		Val = Util.Forest(Val);
			// 	if(ta.GetType() == typeof(ReorderTransaction))
			// 		Val = Util.Berry(Val);
			// 	if(ta.GetType() == typeof(InsertTransaction))
			// 		Val = Util.Violet(Val);
			// 	if(ta.GetType() == typeof(ReorderInOtherSGTransaction))
			// 		Val = Util.Khaki(Val);
			// }
			// public SlotSystemTestResult(bool isPAS, Slottable pickedSB, bool test){
			// 	this.testingSB = pickedSB;
			// 	this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			// 	this.pickedSBName = Util.SBName(pickedSB) + " of " + Util.SGName(pickedSB.SG);
			// 		if(pickedSB.SG.IsPool)
			// 			pickedSBName = Util.Red(pickedSBName);
			// 		else
			// 			pickedSBName = Util.Blue(pickedSBName);
			// 		if(pickedSB.IsEquipped && pickedSB.SG.IsPool)
			// 			pickedSBName = Util.Bold(pickedSBName);
			// 	this.Val = " isPickable: " + test.ToString();
			// 		if(test) Val = Util.Blue(Val); else Val = Util.Red(Val);
			// }
			// public SlotSystemTestResult(bool isPAS, Slottable pickedSB, SBState state){
			// 	this.testingSB = pickedSB;
			// 	this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			// 	this.pickedSBName = Util.SBName(pickedSB) + " of " + Util.SGName(pickedSB.SG);
			// 		if(pickedSB.SG.IsPool)
			// 			pickedSBName = Util.Red(pickedSBName);
			// 		else
			// 			pickedSBName = Util.Blue(pickedSBName);
			// 		if(pickedSB.IsEquipped && pickedSB.SG.IsPool)
			// 			pickedSBName = Util.Bold(pickedSBName);
			// 	this.Val = state.GetType().ToString();
			// 		if(state is SBFocusedState)
			// 			Val = Util.Terra(Val);
			// 		if(state is SBDefocusedState)
			// 			Val = Util.Ciel(Val);
			// 		if(state is WaitForPointerUpState)
			// 			Val = Util.Aqua(Val);
			// }
			// public SlotSystemTestResult(bool isPAS, Slottable pickedSB, SBSelectionState curSelSt, SBSelectionState prevSelSt, SBProcess selProcess ,SBActionState curActSt, SBActionState prevActSt, SBProcess actProcess){
			// 	this.testingSB = pickedSB;
			// 	this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			// 	this.pickedSBName = Util.SBofSG(pickedSB);

			// 	string curSelStStr = Util.SBStateName(curSelSt);
			// 	string prevSelStStr = Util.SBStateName(prevSelSt);
			// 	string selProcStr;
			// 	if(selProcess == null) selProcStr = "null";
			// 	else selProcStr = Util.SBProcessName(selProcess);
			// 	string curActStStr = Util.SBStateName(curActSt);
			// 	string prevActStStr = Util.SBStateName(prevActSt);
			// 	string actProcStr;
			// 	if(actProcess == null) actProcStr = "null";
			// 	else actProcStr = Util.SBProcessName(actProcess);
				
			// 	this.Val = Util.Bold("Sel: ") + prevSelStStr + " to " + curSelStStr + ", Process: " + selProcStr + Util.Bold(" Act: ") + prevActStStr + " to " + curActStStr + ", Process: " + actProcStr;
			// }
			// public SlotSystemTestResult(bool isPAS, Slottable pickedSB, SGMSelectionState prevSel, SGMSelectionState curSel, SGMProcess selProc, SGMActionState prevAct, SGMActionState curAct, SGMProcess actProc, SlotSystemTransaction ta, bool pSBDone, bool sSBDone, bool oSGDone, bool sSGDone){
			// 	this.testingSB = pickedSB;
			// 	this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
			// 	this.pickedSBName = Util.SBofSG(pickedSB);

			// 	this.Val = Util.SBofSG(pickedSB) + 
			// 		" SGM: Sel from " + Util.SGMStateName(prevSel) + " to " + Util.SGMStateName(curSel) +  ", proc " + Util.SGMProcessName(selProc) + 
			// 		" Act from " + Util.SGMStateName(prevAct) + " to " + Util.SGMStateName(curAct) +
			// 		", proc " + Util.SGMProcessName(actProc) +
			// 		", TA " + Util.TransactionName(ta) + 
			// 		", TAComp: " + 
			// 		pSBDone.ToString() + ", " + 
			// 		sSBDone.ToString() + ", " + 
			// 		oSGDone.ToString() + ", " + 
			// 		sSGDone.ToString();
			// }
			// public SlotSystemTestResult(bool isPAS, SlotGroup sg, SGSelectionState prevSel, SGSelectionState curSel, SGProcess selProc, SGActionState prevAct, SGActionState curAct, SGProcess actProc){
			// 	this.testTargetSG = sg;
			// 	this.hoveredName = Util.SGName(sg);
			// 	this.pAS = "pAS: " + (isPAS?Util.Blue("on "):Util.Red("off "));
				
			// 	this.Val = Util.SGName(sg)+ " Sel from " + Util.SGStateName(prevSel) + " to "+ Util.SGStateName(curSel) + ", proc " + Util.SGProcessName(selProc)+", Act from " + Util.SGStateName(prevAct) + " to " + Util.SGStateName(curAct) + ", proc " + Util.SGProcessName(actProc);

			// }
		public SlotSystemTestResult(bool isPAS, Slottable testSB, bool isPickable){
			this.testingSB = testSB;
			this.isPAS = isPAS;
			this.Val = " isPiackable: " + (isPickable?"True":"False");
		}
		public SlotSystemTestResult(SlotGroupManager sgm, bool isPAS, bool isTAS, Slottable testSB, SlotGroup testTargetSG, Slottable testTargetSB, int i){
			this.isPAS = isPAS; this.isTAS = isTAS;
			this.testingSB = testSB;
			this.testTargetSG = testTargetSG; this.testTargetSB = testTargetSB;
			this.Val = i.ToString();
		}
		/*	thorough ver	*/
			public SlotSystemTestResult(SlotGroupManager sgm, bool isPAS, bool isTAS, 
				Slottable testSB, SlotGroup testTargetSG, Slottable testTargetSB, TestElement ele){
					this.isPAS = isPAS; this.isTAS = isTAS;
					this.testingSB = testSB;
					this.testTargetSG = testTargetSG; this.testTargetSB = testTargetSB;
					string selection = "testSB: " + Util.SBofSG(testSB) + 
					" tarSG: " + (testTargetSG == null? "null": Util.SGName(testTargetSG)) + " " +
					" tarSB: " + (testTargetSB == null? "null": Util.SBofSG(testTargetSB)) + " ";
					if(ele == TestElement.SGM)
						this.Val = selection + Util.SGMDebug(sgm);
					else if(ele == TestElement.SG)
						this.Val = selection + Util.SGDebug(testTargetSG);
					else if(ele == TestElement.SB)
						this.Val = selection + Util.SBDebug(testSB);
					else if(ele == TestElement.TA)
						this.Val = selection + Util.TADebug(testSB, testTargetSG, testTargetSB);
				}
		public bool SameSGSB(SlotSystemTestResult other){
			if(testingSB == null){
				return this.testTargetSG == other.testTargetSG;
			}else{
				if(testTargetSG == null)
					return this.testingSB == other.testingSB;
				else{
					if(testTargetSB == null)
						return this.testTargetSG == other.testTargetSG && this.testingSB == other.testingSB;
					else
						return this.testTargetSG == other.testTargetSG && this.testingSB == other.testingSB && this.testTargetSB == other.testTargetSB;
				}
			}
		}
		
	}
		List<SlotSystemTestResult> testResults = new List<SlotSystemTestResult>();
		List<List<SlotSystemTestResult>> crossResultsBundles{
			get{
				List<List<SlotSystemTestResult>> result = new List<List<SlotSystemTestResult>>();
				foreach(SlotSystemTestResult res in testResults){
					bool found = false;
					foreach(List<SlotSystemTestResult> bundle in result){
						if(res.SameSGSB(bundle[0]))
							found = true;
					}
					if(!found){
						List<SlotSystemTestResult> bundle = new List<SlotSystemTestResult>();
						bundle.Add(res);
						foreach(SlotSystemTestResult res2 in testResults){
							if(res != res2)
								if(res.SameSGSB(res2)){
									bundle.Add(res2);
								}
						}
						result.Add(bundle);
					}
				}
				return result;
			}
		}
		public void PrintTestResult(string suppressedVal){
			foreach(List<SlotSystemTestResult> bundle in crossResultsBundles){
				if(HasAllSameValue(bundle)){
					if(!(suppressedVal != null && bundle[0].Val == suppressedVal))
						Debug.Log(bundle[0].Val);
				}else{
					foreach(SlotSystemTestResult res in bundle){
						if(!(suppressedVal != null && bundle[0].Val == suppressedVal))
							Debug.Log(
								("isPAS: " + (res.isPAS?Util.Blue("On"):Util.Red("Off"))) + 
								(" isTAS: " + (res.isTAS?Util.Blue("On"):Util.Red("Off"))) + "	" +
								res.Val);
					}
				}
			}
			ClearCResults();
		}
			bool HasAllSameValue(List<SlotSystemTestResult> bundle){
				bool flag = true;
				string prevVal = "";
				foreach(SlotSystemTestResult res in bundle){
					flag &= prevVal == ""? true: res.Val == prevVal;
					prevVal = res.Val;
				}
				return flag;
			}
			public void ClearCResults(){
				testResults.Clear();
			}
		public void Capture(SlotGroupManager sgm, Slottable testSB, SlotGroup testTargetSG, Slottable testTargetSB, bool isPAS, bool isTAS, TestElement ele){
			SlotSystemTestResult res = new SlotSystemTestResult(sgm, isPAS, isTAS, testSB, testTargetSG, testTargetSB, ele);
			testResults.Add(res);
		}
		public void Print(string msg, SlotGroupManager sgm, Slottable testSB, SlotGroup testTargetSG, Slottable testTargetSB, bool isPAS, bool isTAS, TestElement ele){
			string ASSTr = "isPAS: " + (isPAS?Util.Blue("On"):Util.Red("Off")) + ", " +
				"isTAS: " + (isTAS?Util.Blue("On"):Util.Red("Off")) + " ";
			string testSBstr = "testSB: " + Util.SBofSG(testSB) + " ";
			string tarSGstr = "";
				if(testTargetSG != null)
					tarSGstr = "tarSG: " + Util.SGName(testTargetSG) + " ";
			string tarSBstr = "";
				if(testTargetSB != null)
					tarSBstr = "tarSB: " + Util.SBofSG(testTargetSB) + " ";
			string selection = testSBstr + tarSGstr + tarSBstr;
			string disp = "";
			if(ele == TestElement.SB)
				disp = Util.SBDebug(testSB);
			else if(ele == TestElement.SG)
				disp = Util.SGDebug(testTargetSG);
			else if(ele == TestElement.SGM)
				disp = Util.SGMDebug(sgm);
			else if(ele == TestElement.TA)
				disp = Util.TADebug(testSB, testTargetSG, testTargetSB);
			Debug.Log(msg + " " + ASSTr + selection +disp);
		}
	/*	fields 	*/
		PointerEventDataMock eventData = new PointerEventDataMock();
		GameObject sgmGO;
		SlotGroupManager sgm;
		/*	sgs	*/
			/*	pool	*/
				GameObject sgpAllGO;
				SlotGroup sgpAll;
				GameObject sgpPartsGO;
				SlotGroup sgpParts;
				GameObject sgpBowGO;
				SlotGroup sgpBow;
				GameObject sgpWearGO;
				SlotGroup sgpWear;
				GameObject sgpCGearsGO;
				SlotGroup sgpCGears;
			/*	Equip */
				GameObject sgBowGO;
				SlotGroup sgBow;
				GameObject sgWearGO;
				SlotGroup sgWear;
				GameObject sgCGearsGO;
				SlotGroup sgCGears;
		/*	items	*/
			/*	sbp	*/
			Slottable defBowA_p;
			Slottable defBowB_p;
			Slottable crfBowA_p;
			Slottable defWearA_p;
			Slottable defWearB_p;
			Slottable crfWearA_p;
			Slottable defShieldA_p;
			Slottable crfShieldA_p;
			Slottable defMWeaponA_p;
			Slottable crfMWeaponA_p;
			Slottable defQuiverA_p;
			Slottable defPackA_p;
			Slottable defParts_p;
			Slottable crfParts_p;
			
			Slottable defBowA_p2;
			Slottable defBowB_p2;
			Slottable crfBowA_p2;
			Slottable defWearA_p2;
			Slottable defWearB_p2;
			Slottable crfWearA_p2;
			Slottable defShieldA_p2;
			Slottable crfShieldA_p2;
			Slottable defMWeaponA_p2;
			Slottable crfMWeaponA_p2;
			Slottable defQuiverA_p2;
			Slottable defPackA_p2;
			Slottable defParts_p2;
			Slottable crfParts_p2;
			
			/*	sbe	*/
			Slottable defBowA_e;
			Slottable defWearA_e;
			Slottable defShieldA_e;
			Slottable defMWeaponA_e;

	
		/*	inventories	*/
		PoolInventory poolInv;
		EquipmentSetInventory equipInv;
		bool picked;
	[SetUp]
	public void Setup(){
		/*	Items	*/
			/*	bows	*/
				BowMock defBow = new BowMock();
				defBow.ItemID = 0;
				BowMock crfBow = new BowMock();
				crfBow.ItemID = 1;
				BowInstanceMock defBowA = new BowInstanceMock();//	equipped
				defBowA.Item = defBow;
				BowInstanceMock defBowB = new BowInstanceMock();
				defBowB.Item = defBow;
				BowInstanceMock crfBowA = new BowInstanceMock();
				crfBowA.Item = crfBow;
			/*	wears	*/
				WearMock defWear = new WearMock();
				defWear.ItemID = 100;
				WearMock crfWear = new WearMock();
				crfWear.ItemID = 101;
				WearInstanceMock defWearA = new WearInstanceMock();//	equipped
				defWearA.Item = defWear;
				WearInstanceMock defWearB = new WearInstanceMock();
				defWearB.Item = defWear;
				WearInstanceMock crfWearA = new WearInstanceMock();
				crfWearA.Item = crfWear;
			/*	parts	*/
				PartsMock defParts = new PartsMock();
				defParts.ItemID = 600;
				defParts.IsStackable = true;
				PartsMock crfParts = new PartsMock();
				crfParts.ItemID = 601;
				crfParts.IsStackable = true;
				
				PartsInstanceMock defPartsA = new PartsInstanceMock();
				defPartsA.Item = defParts;
				defPartsA.Quantity = 10;
				PartsInstanceMock defPartsB = new PartsInstanceMock();
				defPartsB.Item = defParts;
				defPartsB.Quantity = 5;
				PartsInstanceMock crfPartsA = new PartsInstanceMock();
				crfPartsA.Item = crfParts;
				crfPartsA.Quantity = 3;
				Assert.That(defPartsA, Is.EqualTo(defPartsB));
				Assert.That(object.ReferenceEquals(defPartsA, defPartsB), Is.False);
			/*	carried gears	*/
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

				QuiverMock defQuiver = new QuiverMock();
				defQuiver.ItemID = 400;
				QuiverInstanceMock defQuiverA = new QuiverInstanceMock();
				defQuiverA.Item = defQuiver;

				PackMock defPack = new PackMock();
				defPack.ItemID = 500;
				PackInstanceMock defPackA = new PackInstanceMock();
				defPackA.Item = defPack;
		/*	Inventory	*/
			poolInv = new PoolInventory();
				poolInv.AddItem(defBowA);
				poolInv.AddItem(defBowB);
				poolInv.AddItem(crfBowA);
				poolInv.AddItem(defWearA);
				poolInv.AddItem(defWearB);
				poolInv.AddItem(crfWearA);
				poolInv.AddItem(defShieldA);
				poolInv.AddItem(defMWeaponA);
				poolInv.AddItem(defPartsA);
				poolInv.AddItem(defPartsB);
				poolInv.AddItem(crfPartsA);
				poolInv.AddItem(crfShieldA);
				poolInv.AddItem(crfMWeaponA);
				poolInv.AddItem(defQuiverA);
				poolInv.AddItem(defPackA);
				List<CarriedGearInstanceMock> initCGears = new List<CarriedGearInstanceMock>();
				initCGears.Add(defShieldA);
				initCGears.Add(defMWeaponA);
			equipInv = new EquipmentSetInventory(defBowA, defWearA, initCGears, 4);
				AE(poolInv.items.Count, 14);
				AE(equipInv.items.Count, 4);
		/*	SGs	*/
			/*	sgpAll	*/
				sgpAllGO = new GameObject("PoolSlotGroup");
				sgpAll = sgpAllGO.AddComponent<SlotGroup>();
				sgpAll.Initialize(SlotGroup.NullFilter, poolInv, true, 0, SlotGroup.emptyCommand);
					ASSG(sgpAll,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpAll.isShrinkable, true);
					AB(sgpAll.isExpandable, true);
					AssertSGCounts(sgpAll,
						14,//slots
						14,//items
						14);//sbs
			/*	sgpBow	*/
				sgpBowGO = new GameObject("sgpBowGO");
				sgpBow = sgpBowGO.AddComponent<SlotGroup>();
				sgpBow.Initialize(SlotGroup.BowFilter, poolInv, true, 0, SlotGroup.emptyCommand);
					ASSG(sgpBow,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpBow.isShrinkable, true);
					AB(sgpBow.isExpandable, true);
					AssertSGCounts(sgpBow,
						3,//slots
						3,//items
						3);//sbs
			/*	sgpWear	*/
				sgpWearGO = new GameObject("sgpWearGO");
				sgpWear = sgpWearGO.AddComponent<SlotGroup>();
				sgpWear.Initialize(SlotGroup.WearFilter, poolInv, true, 0, SlotGroup.emptyCommand);
					ASSG(sgpWear,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpWear.isShrinkable, true);
					AB(sgpWear.isExpandable, true);
					AssertSGCounts(sgpWear,
						3,//slots
						3,//items
						3);//sbs
			/*	sgpCGears	*/
				sgpCGearsGO = new GameObject("sgpCGearsGO");
				sgpCGears = sgpCGearsGO.AddComponent<SlotGroup>();
				sgpCGears.Initialize(SlotGroup.CGearsFilter, poolInv, true, 0, SlotGroup.emptyCommand);
					ASSG(sgpCGears,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpCGears.isShrinkable, true);
					AB(sgpCGears.isExpandable, true);
					AssertSGCounts(sgpCGears,
						6,//slots
						6,//items
						6);//sbs
			/*	sgpParts	*/
				sgpPartsGO = new GameObject("PartsSlotGroupPool");
				sgpParts = sgpPartsGO.AddComponent<SlotGroup>();
				sgpParts.Initialize(SlotGroup.PartsFilter, poolInv, true, 0, SlotGroup.emptyCommand);
					ASSG(sgpParts,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpParts.isShrinkable, true);
					AB(sgpParts.isExpandable, true);
					AssertSGCounts(sgpParts,
						2,//slots
						2,//items
						2);//sbs
			/*	sgBow	*/
				sgBowGO = new GameObject("BowSlotGroup");
				sgBow = sgBowGO.AddComponent<SlotGroup>();
				sgBow.Initialize(SlotGroup.BowFilter, equipInv, false, 1, SlotGroup.updateEquippedStatusCommand);
					ASSG(sgBow,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgBow.isShrinkable, false);
					AB(sgBow.isExpandable, false);
					AssertSGCounts(sgBow,
						1,//slots
						1,//items
						1);//sbs
			/*	sgWear	*/
				sgWearGO = new GameObject("WearSlotGroup");
				sgWear = sgWearGO.AddComponent<SlotGroup>();
				sgWear.Initialize(SlotGroup.WearFilter, equipInv, false, 1, SlotGroup.updateEquippedStatusCommand);
					ASSG(sgWear,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgWear.isShrinkable, false);
					AB(sgWear.isExpandable, false);
					AssertSGCounts(sgWear,
						1,//slots
						1,//items
						1);//sbs
			/*	sgCGears	*/
				sgCGearsGO = new GameObject("CarriedGearsSG");
				sgCGears = sgCGearsGO.AddComponent<SlotGroup>();
				equipInv.SetEquippableCGearsCount(4);
				int slotCount = equipInv.equippableCGearsCount;
				sgCGears.Initialize(SlotGroup.CGearsFilter, equipInv, true, slotCount, SlotGroup.updateEquippedStatusCommand);
					ASSG(sgCGears,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AB(sgCGears.isShrinkable, true);
						AB(sgCGears.isExpandable, false);
						AssertSGCounts(sgCGears,
							4,//slots
							2,//items
							2);//sbs
		/*	SBs	*/
			defBowA_p = sgpAll.GetSB(defBowA);
			defBowB_p = sgpAll.GetSB(defBowB);
			crfBowA_p = sgpAll.GetSB(crfBowA);
			defWearA_p = sgpAll.GetSB(defWearA);
			defWearB_p = sgpAll.GetSB(defWearB);
			crfWearA_p = sgpAll.GetSB(crfWearA);
			defShieldA_p = sgpAll.GetSB(defShieldA);
			crfShieldA_p = sgpAll.GetSB(crfShieldA);
			defMWeaponA_p =	sgpAll.GetSB(defMWeaponA); 
			crfMWeaponA_p =	sgpAll.GetSB(crfMWeaponA);
			defQuiverA_p = sgpAll.GetSB(defQuiverA);
			defPackA_p = sgpAll.GetSB(defPackA);
			defParts_p = sgpAll.GetSB(defPartsA);
			crfParts_p = sgpAll.GetSB(crfPartsA);

			defBowA_p2 = sgpBow.GetSB(defBowA);
			defBowB_p2 = sgpBow.GetSB(defBowB);
			crfBowA_p2 = sgpBow.GetSB(crfBowA);
			defWearA_p2 = sgpWear.GetSB(defWearA);
			defWearB_p2 = sgpWear.GetSB(defWearB);
			crfWearA_p2 = sgpWear.GetSB(crfWearA);
			defShieldA_p2 = sgpCGears.GetSB(defShieldA);
			crfShieldA_p2 = sgpCGears.GetSB(crfShieldA);
			defMWeaponA_p2 = sgpCGears.GetSB(defMWeaponA); 
			crfMWeaponA_p2 = sgpCGears.GetSB(crfMWeaponA);
			defQuiverA_p2 = sgpCGears.GetSB(defQuiverA);
			defPackA_p2 = sgpCGears.GetSB(defPackA);
			defParts_p2 = sgpParts.GetSB(defPartsA);
			crfParts_p2 = sgpParts.GetSB(crfPartsA);

			defBowA_e = sgBow.GetSB(defBowA);
			defWearA_e = sgWear.GetSB(defWearA);
			defShieldA_e = sgCGears.GetSB(defShieldA);
			defMWeaponA_e = sgCGears.GetSB(defMWeaponA);
		/*	SGM	*/
			sgmGO = new GameObject("SlotGroupManager");
			sgm = sgmGO.AddComponent<SlotGroupManager>();
			EquipmentSet equipSetA = new EquipmentSet(sgBow, sgWear, sgCGears);
			SlotSystemBundle equipBundle = new SlotSystemBundle();
			equipBundle.Elements.Add(equipSetA);
			equipBundle.SetFocusedBundleElement(equipSetA);
			SlotSystemBundle poolBundle = new SlotSystemBundle();
			poolBundle.Elements.Add(sgpAll);
			poolBundle.Elements.Add(sgpParts);
			poolBundle.Elements.Add(sgpBow);
			poolBundle.Elements.Add(sgpWear);
			poolBundle.Elements.Add(sgpCGears);
			poolBundle.SetFocusedBundleElement(sgpAll);
			InventoryManagerPage invManPage = new InventoryManagerPage(poolBundle, equipBundle);
		sgm.Initialize(invManPage);
			AssertBundlesPagesAndSGsMembership();
			AssertSBsMembership();
			AssertInitialize();
		sgm.Activate();
		AssertFocused();
	}
	[Test]
	public void TestAll(){
		//	not gonna use on the second thought
			// TestReorderInOtherAll();
			// TestInsertAll();//	later
		//	not testable for now
			// TestPickUpTransitionOnAll(); //revisit after stack
		// done
			// CheckShrinkableAndExpandableOnAllSGs();
			// CheckPickcableOnAllSB();
			// CheckTransacitonWithSBSpecifiedOnAll();
			// CheckTransactionOnAllSG();
			// TestDraggedIconOnAll();
			// TestAcceptSGTACompOnAll();
			// TestReorderSBsMethod();
			// TestSGActionStateSequence();
			// TestSBStateTransitionOnAll();
			// TestSGMStateTransition();
			// TestRevertOnAllSBs();
			// TestVolSortOnSPGParts();
			// TestVolSortOnAll();
			// TestReorderOnAll();
			// TestFillOnAll();
			// TestSwapOnAll();
			// TestFillShortcut();
			// TestPermutation();
			// TestCombination();
			// TestSGCGearsCorrespondence();
			// AssertInitialize();
			// TestChangingSGCGearsSlotsCountAgain();
			// TestSwapShortcut();
			// TestSGECorrespondence();
		// TestAddAndRemoveAll();
		// TestSGGeneric();
		// TestSlotSystemElement();
	}
	/*	SGs testing	*/
		public void TestSGECorrespondence(){
			sgm.SetFocusedPoolSG(sgpAll);
				sgpAll.ToggleAutoSort(true);
					sgBow.ToggleAutoSort(true);
					sgWear.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpAll true, sge true"));
					TestEquippingFromTo(sgpAll, sgBow);
					TestEquippingFromTo(sgpAll, sgWear);
					sgBow.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpAll true, sgBow false"));
					TestEquippingFromTo(sgpAll, sgBow);
					sgWear.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpAll true, sgWear false"));
					TestEquippingFromTo(sgpAll, sgWear);
				sgpAll.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpAll false, sges false"));
					TestEquippingFromTo(sgpAll, sgBow);
					TestEquippingFromTo(sgpAll, sgWear);
					sgBow.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpAll false, sgBow true"));
					TestEquippingFromTo(sgpAll, sgBow);
					sgWear.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpAll false, sgWear true"));
					TestEquippingFromTo(sgpAll, sgWear);
			sgm.SetFocusedPoolSG(sgpBow);
				sgpBow.ToggleAutoSort(true);
				sgBow.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpBow true, sgBow true"));
					TestEquippingFromTo(sgpBow, sgBow);
				sgBow.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpBow true, sgBow false"));
					TestEquippingFromTo(sgpBow, sgBow);
				sgpBow.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpBow false, sgBow false"));
					TestEquippingFromTo(sgpBow, sgBow);
				sgBow.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpBow false, sgBow true"));
					TestEquippingFromTo(sgpBow, sgBow);
			sgm.SetFocusedPoolSG(sgpWear);
				sgpWear.ToggleAutoSort(true);
				sgWear.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpWear true, sgWear true"));
					TestEquippingFromTo(sgpWear, sgWear);
				sgWear.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpWear true, sgWear false"));
					TestEquippingFromTo(sgpWear, sgWear);
				sgpWear.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpWear false, sgWear false"));
					TestEquippingFromTo(sgpWear, sgWear);
				sgWear.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpWear false, sgWear true"));
					TestEquippingFromTo(sgpWear, sgWear);
			}public void TestEquippingFromTo(SlotGroup sgp, SlotGroup sge){
				foreach(Slottable sb in transactableSBs(sgp, sge, null, typeof(SwapTransaction))){
					// Print(sb);
					InventoryItemInstanceMock testItem = sb.itemInst;
					InventoryItemInstanceMock swapItem = sgm.GetTransaction(sb, sge, null).targetSB.itemInst;
					AssertFocused();
					Swap(sb, sgp, sge, null);
					// Fill(sb, sgp, sge, null);//make this swap
					Print(sge.slottables[0]);
					PrintItemsArray(equipInv.items);
					AssertFocused();
					/*	reverse	*/
					Swap(sge.GetSB(testItem), sge, null, sgp.GetSB(swapItem));
					// Fill(sge.GetSB(testItem), sge, sgp, null);//swap
					AssertFocused();
				}
				foreach(Slottable sbe in sge.slottables){
					if(sbe != null){
						foreach(Slottable sbp in transactableSBs(sgp, null, sbe, typeof(SwapTransaction))){
							InventoryItemInstanceMock testItem = sbp.itemInst;
							InventoryItemInstanceMock swapItem = sbe.itemInst;
							Slottable testSbe = sge.GetSB(swapItem);
								AssertFocused();
							Swap(sbp, sgp, null, testSbe);
								Print(sge.slottables[0]);
								PrintItemsArray(equipInv.items);
							/*	reverse	*/
							Swap(sge.GetSB(testItem), sge, null, sgp.GetSB(swapItem));
								AssertFocused();
						}
					}
				}
			}
		public void TestSGCGearsCorrespondence(){
				sgm.SetFocusedPoolSG(sgpAll);
				sgpAll.ToggleAutoSort(true);
				sgCGears.ToggleAutoSort(true);
			TestEquippingSGCGearsFrom(sgpAll);
				sgCGears.ToggleAutoSort(false);
			TestEquippingSGCGearsFrom(sgpAll);
				sgpAll.ToggleAutoSort(false);
			TestEquippingSGCGearsFrom(sgpAll);
				sgCGears.ToggleAutoSort(true);
			TestEquippingSGCGearsFrom(sgpAll);
				
				sgm.SetFocusedPoolSG(sgpCGears);
				sgpCGears.ToggleAutoSort(true);
				sgCGears.ToggleAutoSort(true);
			TestEquippingSGCGearsFrom(sgpCGears);
				sgCGears.ToggleAutoSort(false);
			TestEquippingSGCGearsFrom(sgpCGears);
				sgpCGears.ToggleAutoSort(false);
			TestEquippingSGCGearsFrom(sgpCGears);
				sgCGears.ToggleAutoSort(true);
			TestEquippingSGCGearsFrom(sgpCGears);
			}public void TestEquippingSGCGearsFrom(SlotGroup sgp){
				ClearSGCGearsTo(sgp);
				/*	increasing	*/
					for(int newSlotCount = 1; newSlotCount < 5; newSlotCount++){
						sgm.ChangeEquippableCGearsCount(newSlotCount, sgCGears);
						AssertSGCounts(sgCGears, newSlotCount, 0, 0);
						AssertFocused();
						// PrintSBsArray(sgCGears.slottables);
						foreach(List<Slottable> sbsCombo in possibleSBsCombos(newSlotCount, transactableSBs(sgp, sgCGears, null, typeof(FillTransaction)))){
							foreach(Slottable sb in sbsCombo){
								Fill(sb, sgp, sgCGears, null);
							}
							AE(transactableSBs(sgp, sgCGears, null, typeof(FillTransaction)).Count, 0);
							int count = newSlotCount -1;
							/*	reducing the slots count while there's still some sbs left, down to 1	*/
								while(count > 0){
									sgm.ChangeEquippableCGearsCount(count, sgCGears);
									// PrintSBsArray(sgCGears.slottables);
									// PrintItemsArray(equipInv.items);
									count --;
								}
								AE(sgCGears.slots.Count, 1);
								sgm.ChangeEquippableCGearsCount(newSlotCount, sgCGears);
								ClearSGCGearsTo(sgp);
							/*	refilling	*/
								foreach(Slottable sb in sbsCombo){
									Fill(sb, sgp, sgCGears, null);
								}
							/*	empty the slots	*/
								foreach(Slottable sb in sgCGears.slottables){
									if(sb != null)
										Fill(sb, sgCGears, sgp, null);
								}
								AE(sgCGears.actualSBsCount, 0);
						}
					}
				/*	decreasing	*/
					for(int newSlotCount = 4; newSlotCount > 0; newSlotCount--){
						sgm.ChangeEquippableCGearsCount(newSlotCount, sgCGears);
						AssertSGCounts(sgCGears, newSlotCount, 0, 0);
						AssertFocused();
						foreach(List<Slottable> sbsCombo in possibleSBsCombos(newSlotCount, transactableSBs(sgp, sgCGears, null, typeof(FillTransaction)))){
							foreach(Slottable sb in sbsCombo){
								Fill(sb, sgp, sgCGears, null);
							}
							AE(transactableSBs(sgp, sgCGears, null, typeof(FillTransaction)).Count, 0);
							foreach(Slottable sb in sgCGears.slottables){
								if(sb != null)
									Fill(sb, sgCGears, sgp, null);
							}
							AE(sgCGears.actualSBsCount, 0);
						}
					}
			}
	/*	test support fields	*/
		List<Slottable> allSBsList{
			get{
				List<Slottable> sbs = new List<Slottable>();
				sbs.AddRange(sbpList);
				sbs.AddRange(sbp2List);
				sbs.AddRange(sbeList);
				return sbs;
			}
		}
		List<Slottable> sbpList{
			get{
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_p);
				sbs.Add(defBowB_p);
				sbs.Add(crfBowA_p);
				sbs.Add(defWearA_p);
				sbs.Add(defWearB_p);
				sbs.Add(crfWearA_p);
				sbs.Add(defShieldA_p);
				sbs.Add(crfShieldA_p);
				sbs.Add(defMWeaponA_p);
				sbs.Add(crfMWeaponA_p);
				sbs.Add(defParts_p);
				sbs.Add(crfParts_p);
				sbs.Add(defQuiverA_p);
				sbs.Add(defPackA_p);
				return sbs;
			}
		}
		List<Slottable> sbp2List{
			get{
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_p2);
				sbs.Add(defBowB_p2);
				sbs.Add(crfBowA_p2);
				sbs.Add(defWearA_p2);
				sbs.Add(defWearB_p2);
				sbs.Add(crfWearA_p2);
				sbs.Add(defShieldA_p2);
				sbs.Add(crfShieldA_p2);
				sbs.Add(defMWeaponA_p2);
				sbs.Add(crfMWeaponA_p2);
				sbs.Add(defQuiverA_p2);
				sbs.Add(defPackA_p2);
				sbs.Add(defParts_p2);
				sbs.Add(crfParts_p2);
				return sbs;
			}
		}
		List<Slottable> sbeList{
			get{
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_e);
				sbs.Add(defWearA_e);
				sbs.Add(defShieldA_e);
				sbs.Add(defMWeaponA_e);
				return sbs;
			}
		}
		public List<Slottable> testSBs{
			get{
				List<Slottable> res = new List<Slottable>();
					res.Add(defBowA_p);
					res.Add(crfBowA_p);
					res.Add(defBowB_p);
					res.Add(defWearA_p);
					res.Add(crfWearA_p);
					res.Add(defWearB_p);
					res.Add(defShieldA_p);
					res.Add(crfShieldA_p);
					res.Add(defMWeaponA_p);
					res.Add(crfMWeaponA_p);
					res.Add(defQuiverA_p);
					res.Add(defPackA_p);
					res.Add(defParts_p);
					res.Add(crfParts_p);
				return res;
			}
		}
		SlotGroup origSGCache;
	/*	working	*/
		public void TestSGGeneric(){
			// AE(sgm.sgBundles.Count, 3);
			// AE(sgm.genBundles.Count, 1);
			// AE(sgm.allSGPs.Count, 5);
			// AE(sgm.allSGEs.Count, 3);
			// AE(sgm.allSGGs.Count, 2);
			// AE(sgm.focusedBundles.Count, 2);
			// AE(sgm.focusedSGGs.Count, 2);
			// sgm.FocusBundle(genBundle);
			// AE(sgm.FocusedBundles.Count, 3);
		}
		public void TestAddAndRemoveAll(){
			PerformOnAllSGAfterFocusing(TestAddAndRemove);
			PrintTestResult(null);
			}
			public void TestAddAndRemove(SlotGroup testSG, bool isPAS){
				List<Slottable> sbs = new List<Slottable>();
				sbs.AddRange(testSG.slottables);
					AssertFocused();
				// sgm.MoveSBs(testSG, null, sbs);
				// 	ASSGM(sgm,
				// 		null, null, testSG, null, null, null, null, null,
				// 		SGMDeactivated, SGMFocused, null,
				// 		SGMWFA, SGMTransaction, typeof(SGMTransactionProcess),
				// 		typeof())
			}

	/*	setup	*/
		public void AssertInitialize(){
			// ANull(SlotGroupManager.CurSGM);
			ASSGM(sgm,
				null, null, null, null, null, null, null, null,
				SGMDeactivated, SGMDeactivated, null,
				SGMWFA, SGMWFA, null,
				null, true, true, true, true);
			foreach(SlotGroup sg in sgm.allSGs){
				ASSG(sg,
					SGDeactivated, SGDeactivated, null,
					SGWFA, SGWFA, null, false);
					ASGReset(sg);
				foreach(Slottable sb in sg.slottables){
					if(sb != null){
						ASSB(sb,
							SBDeactivated, SBDeactivated, null,
							SBWFA, SBWFA, null, false,
							null, null, null);
						ANull(sb.CurEqpState);
						ANull(sb.PrevEqpState);
					}
				}
			}
		}
		public void AssertBundlesPagesAndSGsMembership(){
			ANotNull(sgm.rootPage);
			AE(sgm.allSGs.Count, 8);
				AB(sgm.allSGs.Contains(sgpAll), true);
				AB(sgm.allSGs.Contains(sgpBow), true);
				AB(sgm.allSGs.Contains(sgpWear), true);
				AB(sgm.allSGs.Contains(sgpCGears), true);
				AB(sgm.allSGs.Contains(sgpParts), true);
				AB(sgm.allSGs.Contains(sgBow), true);
				AB(sgm.allSGs.Contains(sgWear), true);
				AB(sgm.allSGs.Contains(sgCGears), true);
			AE(sgm.focusedSGs.Count, 4);
				AB(sgm.focusedSGs.Contains(sgpAll), true);
				AB(sgm.focusedSGs.Contains(sgBow), true);
				AB(sgm.focusedSGs.Contains(sgWear), true);
				AB(sgm.focusedSGs.Contains(sgCGears), true);
			ANotNull(sgm.rootPage.PoolBundle);
			AE(sgm.allSGPs.Count, 5);
				AB(sgm.allSGPs.Contains(sgpAll), true);
				AB(sgm.allSGPs.Contains(sgpBow), true);
				AB(sgm.allSGPs.Contains(sgpWear), true);
				AB(sgm.allSGPs.Contains(sgpCGears), true);
				AB(sgm.allSGPs.Contains(sgpParts), true);
				AE(sgm.rootPage.DirectParent(sgpAll), sgm.rootPage.PoolBundle);
				AE(sgm.rootPage.DirectParent(sgpBow), sgm.rootPage.PoolBundle);
				AE(sgm.rootPage.DirectParent(sgpWear), sgm.rootPage.PoolBundle);
				AE(sgm.rootPage.DirectParent(sgpCGears), sgm.rootPage.PoolBundle);
				AE(sgm.rootPage.DirectParent(sgpParts), sgm.rootPage.PoolBundle);
			AB(sgpAll.isPool, true);
			AB(sgpBow.isPool, true);
			AB(sgpWear.isPool, true);
			AB(sgpCGears.isPool, true);
			AB(sgpParts.isPool, true);
			AB(sgpAll.isSGE, false);
			AB(sgpBow.isSGE, false);
			AB(sgpWear.isSGE, false);
			AB(sgpCGears.isSGE, false);
			AB(sgpParts.isSGE, false);
			AE(sgm.focusedSGP, sgpAll);

			ANotNull(sgm.rootPage.EquipBundle);
			AE(sgm.allSGEs.Count, 3);
			AE(sgm.focusedSGEs.Count, 3);
				AB(sgm.focusedSGEs.Contains(sgBow), true);
				AB(sgm.focusedSGEs.Contains(sgWear), true);
				AB(sgm.focusedSGEs.Contains(sgCGears), true);
				AE(sgm.focusedEqSet, sgm.rootPage.EquipBundle.GetFocusedBundleElement());
				AE(sgm.rootPage.DirectParent(sgBow), sgm.focusedEqSet);
				AE(sgm.rootPage.DirectParent(sgWear), sgm.focusedEqSet);
				AE(sgm.rootPage.DirectParent(sgCGears), sgm.focusedEqSet);
			AB(sgBow.isPool, false);
			AB(sgWear.isPool, false);
			AB(sgCGears.isPool, false);
			AB(sgBow.isSGE, true);
			AB(sgWear.isSGE, true);
			AB(sgCGears.isSGE, true);
		}
		public void AssertSBsMembership(){
			foreach(Slottable sbp in sbpList){
				AE(sgm.GetSG(sbp), sgpAll);
				AE(sbp.sg, sgpAll);
				AE(sgm.rootPage.ContainsInHierarchy(sbp), true);
				AE(sgm.rootPage.PoolBundle.ContainsInHierarchy(sbp), true);
				AE(sgm.rootPage.EquipBundle.ContainsInHierarchy(sbp), false);

				AE(sgpAll.ContainsInHierarchy(sbp), true);
				AE(sgpBow.ContainsInHierarchy(sbp), false);
				AE(sgpWear.ContainsInHierarchy(sbp), false);
				AE(sgpCGears.ContainsInHierarchy(sbp), false);
				AE(sgpParts.ContainsInHierarchy(sbp), false);
			}
			foreach(Slottable sbp2 in sbp2List){
				AE(sgm.rootPage.ContainsInHierarchy(sbp2), true);
				AE(sgm.rootPage.PoolBundle.ContainsInHierarchy(sbp2), true);
				AE(sgpAll.ContainsInHierarchy(sbp2), false);
				if(sbp2.itemInst is BowInstanceMock){
					AE(sgm.GetSG(sbp2), sgpBow);
					AE(sbp2.sg, sgpBow);
					AE(sgpBow.ContainsInHierarchy(sbp2), true);
					AE(sgpWear.ContainsInHierarchy(sbp2), false);
					AE(sgpCGears.ContainsInHierarchy(sbp2), false);
					AE(sgpParts.ContainsInHierarchy(sbp2), false);
				}else if(sbp2.itemInst is WearInstanceMock){
					AE(sgm.GetSG(sbp2), sgpWear);
					AE(sbp2.sg, sgpWear);
					AE(sgpBow.ContainsInHierarchy(sbp2), false);
					AE(sgpWear.ContainsInHierarchy(sbp2), true);
					AE(sgpCGears.ContainsInHierarchy(sbp2), false);
					AE(sgpParts.ContainsInHierarchy(sbp2), false);
				}else if(sbp2.itemInst is CarriedGearInstanceMock){
					AE(sgm.GetSG(sbp2), sgpCGears);
					AE(sbp2.sg, sgpCGears);
					AE(sgpBow.ContainsInHierarchy(sbp2), false);
					AE(sgpWear.ContainsInHierarchy(sbp2), false);
					AE(sgpCGears.ContainsInHierarchy(sbp2), true);
					AE(sgpParts.ContainsInHierarchy(sbp2), false);
				}else if(sbp2.itemInst is PartsInstanceMock){
					AE(sgm.GetSG(sbp2), sgpParts);
					AE(sbp2.sg, sgpParts);
					AE(sgpBow.ContainsInHierarchy(sbp2), false);
					AE(sgpWear.ContainsInHierarchy(sbp2), false);
					AE(sgpCGears.ContainsInHierarchy(sbp2), false);
					AE(sgpParts.ContainsInHierarchy(sbp2), true);
				}
			}
			foreach(Slottable sbe in sbeList){
				AE(sgm.rootPage.ContainsInHierarchy(sbe), true);
				AE(sgm.rootPage.PoolBundle.ContainsInHierarchy(sbe), false);
				AE(sgm.rootPage.EquipBundle.ContainsInHierarchy(sbe), true);
				foreach(EquipmentSet eSet in sgm.equipmentSets){
					if(eSet == sgm.focusedEqSet)
						AE(eSet.ContainsInHierarchy(sbe), true);
					else
						AE(eSet.ContainsInHierarchy(sbe), false);
				}
				if(sbe.itemInst is BowInstanceMock){
					AE(sgm.GetSG(sbe), sgBow);
					AE(sbe.sg, sgBow);
					AE(sgBow.ContainsInHierarchy(sbe), true);
					AE(sgWear.ContainsInHierarchy(sbe), false);
					AE(sgCGears.ContainsInHierarchy(sbe), false);
				}
				if(sbe.itemInst is WearInstanceMock){
					AE(sgm.GetSG(sbe), sgWear);
					AE(sbe.sg, sgWear);
					AE(sgBow.ContainsInHierarchy(sbe), false);
					AE(sgWear.ContainsInHierarchy(sbe), true);
					AE(sgCGears.ContainsInHierarchy(sbe), false);
				}
				if(sbe.itemInst is CarriedGearInstanceMock){
					AE(sgm.GetSG(sbe), sgCGears);
					AE(sbe.sg, sgCGears);
					AE(sgBow.ContainsInHierarchy(sbe), false);
					AE(sgWear.ContainsInHierarchy(sbe), false);
					AE(sgCGears.ContainsInHierarchy(sbe), true);
				}
			}
			
		}
		public void TestSwapOnAll(){
		PerformOnAllSBs(CrossTestSwap);
		PrintTestResult(null);
		}
		public void CrossTestSwap(Slottable sb, bool isPAS){
			CrossTestSGs(TestSwap, sb, isPAS);
			origSGCache = null;
		}
		public void TestSwap(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
			SlotGroup origSG = testSB.sg;
			if(origSG != null)
				origSGCache = origSG;
			else
				origSG = origSGCache;
			InventoryItemInstanceMock pickedItem = testSB.itemInst;
			testSB = origSG.GetSB(pickedItem);
			if(testSB.isPickable){
				SlotSystemTransaction ta = sgm.GetTransaction(testSB, tarSG, null);
				if(ta is SwapTransaction){
						AssertFocused();
						ASSGM(sgm,
							null, null, null, null, null, null, null, null,
							SGMDeactivated, SGMFocused, null,
							null, SGMWFA, null,
							null, true, true, true, true);
						ASSG(origSG,
							null, SGFocused, null,
							null, SGWFA, null, false);
						ASSB(testSB,
							null, SBFocused, null,
							null, SBWFA, null, false,
							null, null, null);
					PickUp(testSB, out picked);
						ASSGM(sgm,
							testSB, null, null, null, testSB, null, null, testSB,
							SGMDeactivated, SGMFocused, null,
							SGMWFA, SGMProbing, typeof(SGMProbeProcess),
							typeof(RevertTransaction), false, true, true, true);
						ASSG(origSG,
							SGFocused, SGDefocused, typeof(SGGreyoutProcess),
							null, SGWFA, null, false);
						ASSB(testSB,
							SBSelected, SBDefocused, typeof(SBGreyoutProcess),
							testSB.isStackable?SBWFNT:SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
							null, null, null);
					SimHover(null, tarSG, eventData);
					Slottable tarSB = sgm.Transaction.targetSB;
					InventoryItemInstanceMock tarItem = tarSB.itemInst;
						ASSGM(sgm,
							testSB, tarSB, origSG, tarSG, testSB, null, tarSG, null,
							SGMDeactivated, SGMFocused, null,
							SGMWFA, SGMProbing, typeof(SGMProbeProcess),
							typeof(SwapTransaction), false, true, false, false);
						ASSG(origSG,
							SGFocused, SGDefocused, typeof(SGGreyoutProcess),
							null, SGWFA, null, false);
						ASSG(tarSG,
							null, SGSelected, typeof(SGHighlightProcess),
							null, SGWFA, null, false);
						ASSB(testSB,
							SBSelected, SBDefocused, typeof(SBGreyoutProcess),
							testSB.isStackable?SBWFNT:SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
							null, null, null);
						ASSB(tarSB,
							null, SBSelected, typeof(SBHighlightProcess),
							null, SBWFA, null, false,
							null, null, null);
					LetGo();
						ASSGM(sgm,
							testSB, tarSB, origSG, tarSG, testSB, tarSB, tarSG, null,
							SGMDeactivated, SGMFocused, null,
							SGMProbing, SGMTransaction, typeof(SGMTransactionProcess),
							typeof(SwapTransaction), false, false, origSG.isPool?true:false, tarSG.isPool?true:false);
						ASSG(origSG,
							SGFocused, SGDefocused, typeof(SGGreyoutProcess),
							SGWFA, SGSwap, typeof(SGTransactionProcess), origSG.isPool?false:true);
						ASSG(tarSG,
							null, SGSelected, typeof(SGHighlightProcess),
							SGWFA, SGSwap, typeof(SGTransactionProcess), tarSG.isPool?false:true);
						ASBSelState(testSB,
							SBSelected, SBDefocused, typeof(SBGreyoutProcess));
							if(origSG.isPool)
								ASBActState(testSB,
								SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), false);
							else
								ASBActState(testSB,
								SBPickedUp, SBRemove, typeof(SBRemoveProcess), true);
						ASBSelState(tarSB,
							null, SBSelected, typeof(SBHighlightProcess));
							if(tarSG.isPool)
								ASBActState(tarSB,
								SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), false);
							else
								ASBActState(tarSB,
								SBWFA, SBRemove, typeof(SBRemoveProcess), true);
					// Capture(sgm, testSB, tarSG, null, isPAS, isTAS, TestElement.SGM);
					if(!origSG.isAllTASBsDone){
						AE(sgm.sg1Done, false);
						CompleteAllSBActProcesses(origSG);
						AE(sgm.sg1Done, true);
					}
					if(!tarSG.isAllTASBsDone){
						AE(sgm.sg2Done, false);
						CompleteAllSBActProcesses(tarSG);
						AE(sgm.sg2Done, true);
					}
						AE(sgm.dIcon1Done, false);
					sgm.dIcon1.CompleteMovement();
						AE(sgm.dIcon1Done, true);
						AE(sgm.dIcon2Done, false);
					sgm.dIcon2.CompleteMovement();
						AE(sgm.dIcon2Done, true);
						AssertFocused();
					/*	reverse	*/
					Slottable testSBinTarSG = tarSG.GetSB(pickedItem);
					Slottable tarSBinOrigSG = origSG.GetSB(tarItem);
					PickUp(testSBinTarSG, out picked);
					SimHover(tarSBinOrigSG, null, eventData);
					LetGo();
					if(!origSG.isAllTASBsDone)
						CompleteAllSBActProcesses(origSG);
					if(!tarSG.isAllTASBsDone)
						CompleteAllSBActProcesses(tarSG);
					sgm.dIcon1.CompleteMovement();
					sgm.dIcon2.CompleteMovement();
						AssertFocused();
				}
				foreach(Slottable tarSB in tarSG.slottables){
					if(tarSB != null){
						testSB = origSG.GetSB(pickedItem);
						SlotSystemTransaction ta2 = sgm.GetTransaction(testSB, null, tarSB);
						if(ta2 is SwapTransaction){
							// Capture(sgm, testSB, null, tarSB, isPAS, isTAS, TestElement.SB);
							InventoryItemInstanceMock tarItem = tarSB.itemInst;
								AssertFocused();
								ASSGM(sgm,
									null, null, null, null, null, null, null, null,
									SGMDeactivated, SGMFocused, null,
									null, SGMWFA, null,
									null, true, true, true, true);
								ASSG(origSG,
									null, SGFocused, null,
									null, SGWFA, null, false);
								ASSB(testSB,
									null, SBFocused, null,
									null, SBWFA, null, false,
									null, null, null);
							PickUp(testSB, out picked);
								ASSGM(sgm,
									testSB, null, null, null, testSB, null, null, testSB,
									SGMDeactivated, SGMFocused, null,
									SGMWFA, SGMProbing, typeof(SGMProbeProcess),
									typeof(RevertTransaction), false, true, true, true);
								ASSG(origSG,
									SGFocused, SGDefocused, typeof(SGGreyoutProcess),
									null, SGWFA, null, false);
								ASSB(testSB,
									SBSelected, SBDefocused, typeof(SBGreyoutProcess),
									testSB.isStackable?SBWFNT:SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
									null, null, null);
							SimHover(tarSB, null, eventData);
								ASSGM(sgm,
									testSB, tarSB, origSG, tarSG, testSB, null, null, tarSB,
									SGMDeactivated, SGMFocused, null,
									SGMWFA, SGMProbing, typeof(SGMProbeProcess),
									typeof(SwapTransaction), false, true, false, false);
								ASSG(origSG,
									SGFocused, SGDefocused, typeof(SGGreyoutProcess),
									null, SGWFA, null, false);
								ASSG(tarSG,
									null, SGSelected, typeof(SGHighlightProcess),
									null, SGWFA, null, false);
								ASSB(testSB,
									SBSelected, SBDefocused, typeof(SBGreyoutProcess),
									testSB.isStackable?SBWFNT:SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
									null, null, null);
								ASSB(tarSB,
									null, SBSelected, typeof(SBHighlightProcess),
									null, SBWFA, null, false,
									null, null, null);
							LetGo();
								ASSGM(sgm,
									testSB, tarSB, origSG, tarSG, testSB, tarSB, null, tarSB,
									SGMDeactivated, SGMFocused, null,
									SGMProbing, SGMTransaction, typeof(SGMTransactionProcess),
									typeof(SwapTransaction), false, false, origSG.isPool?true:false, tarSG.isPool?true:false);
								ASSG(origSG,
									SGFocused, SGDefocused, typeof(SGGreyoutProcess),
									SGWFA, SGSwap, typeof(SGTransactionProcess), origSG.isPool?false:true);
								ASSG(tarSG,
									null, SGSelected, typeof(SGHighlightProcess),
									SGWFA, SGSwap, typeof(SGTransactionProcess), tarSG.isPool?false:true);
								ASBSelState(testSB,
									SBSelected, SBDefocused, typeof(SBGreyoutProcess));
									if(origSG.isPool)
										ASBActState(testSB,
										SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), false);
									else
										ASBActState(testSB,
										SBPickedUp, SBRemove, typeof(SBRemoveProcess), true);
								ASBSelState(tarSB,
									null, SBSelected, typeof(SBHighlightProcess));
									if(tarSG.isPool)
										ASBActState(tarSB,
										SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), false);
									else
										ASBActState(tarSB,
										SBWFA, SBRemove, typeof(SBRemoveProcess), true);
							// Capture(sgm, testSB, tarSG, null, isPAS, isTAS, TestElement.SGM);
								if(!origSG.isAllTASBsDone){
									AE(sgm.sg1Done, false);
									CompleteAllSBActProcesses(origSG);
									AE(sgm.sg1Done, true);
								}
								if(!tarSG.isAllTASBsDone){
									AE(sgm.sg2Done, false);
									CompleteAllSBActProcesses(tarSG);
									AE(sgm.sg2Done, true);
								}
									AE(sgm.dIcon1Done, false);
								sgm.dIcon1.CompleteMovement();
									AE(sgm.dIcon1Done, true);
									AE(sgm.dIcon2Done, false);
								sgm.dIcon2.CompleteMovement();
									AE(sgm.dIcon2Done, true);
									AssertFocused();
							/*	reverse	*/
							Slottable tarSBinOrigSG = origSG.GetSB(tarItem);
							Slottable testSBinTarSG = tarSG.GetSB(pickedItem);
							PickUp(testSBinTarSG, out picked);
							SimHover(tarSBinOrigSG, null, eventData);
							LetGo();
							if(!origSG.isAllTASBsDone)
								CompleteAllSBActProcesses(origSG);
							if(!tarSG.isAllTASBsDone)
								CompleteAllSBActProcesses(tarSG);
							sgm.dIcon1.CompleteMovement();
							sgm.dIcon2.CompleteMovement();
								AssertFocused();
						}
					}
				}
			}
		}
	
	/*	Test Transaction on All	*/
		public void TestFillOnAll(){
			PerformOnAllSBs(CrossTestFill);
			PrintTestResult(null);
			}public void CrossTestFill(Slottable sb, bool isPAS){
				CrossTestSGs(TestFill, sb, isPAS);
				origSGCache = null;
			}
			public void TestFill(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				InventoryItemInstanceMock pickedItem = testSB.itemInst;
				SlotGroup origSG = testSB.sg;
				if(origSG != null)
					origSGCache = origSG;
				else
					origSG = origSGCache;
				testSB = origSG.GetSB(pickedItem);
				if(testSB.isPickable){
					if(sgm.GetTransaction(testSB, tarSG, null).GetType() == typeof(FillTransaction)){
							AssertFocused();
							/*	on SG	*/
								// Capture(sgm, testSB, tarSG, null, isPAS, isTAS, TestElement.SG);
									ASSGM(sgm,
										null, null, null, null, null, null, null, null, 
										SGMDeactivated, SGMFocused, null,
										null, SGMWFA, null, 
										null,
										true, true, true, true);
									ASSG(origSG,
										null, SGFocused, null,
										null, SGWFA, null, false);
									ASSB(testSB,
										null, SBFocused, null,
										null, SBWFA, null, false,
										null, null, null);
								PickUp(testSB, out picked);
									ASSGM(sgm,
										testSB, null, null, null, testSB, null, null, testSB, 
										SGMDeactivated, SGMFocused, null,
										SGMWFA, SGMProbing, typeof(SGMProbeProcess), 
										typeof(RevertTransaction),
										false, true, true, true);
									ASSG(origSG,
										SGFocused, SGDefocused, typeof(SGGreyoutProcess),
										null, SGWFA, null, false);
									ASSB(testSB,
										SBSelected, SBDefocused, typeof(SBGreyoutProcess),
										testSB.isStackable?SBWFNT:SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
										null, null, null);
								SimHover(null, tarSG, eventData);
									ASSGM(sgm,
										testSB, null, origSG, tarSG, testSB, null, tarSG, null,  
										SGMDeactivated, SGMFocused, null,
										SGMWFA, SGMProbing, typeof(SGMProbeProcess), 
										typeof(FillTransaction),
										false, true, false, false);
									ASSG(origSG,
										SGFocused, SGDefocused, typeof(SGGreyoutProcess),
										null, SGWFA, null, false);
									ASSG(tarSG,
										SGFocused, SGSelected, typeof(SGHighlightProcess),
										null, SGWFA, null, false);
									ASSB(testSB,
										SBSelected, SBDefocused, typeof(SBGreyoutProcess),
										testSB.isStackable?SBWFNT:SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
										null, null, null);
								LetGo();
									ASSGM(sgm,
										testSB, null, origSG, tarSG, testSB, null, tarSG, null,  
										SGMDeactivated, SGMFocused, null,
										SGMProbing, SGMTransaction, typeof(SGMTransactionProcess), 
										typeof(FillTransaction),
										false, true, origSG.isAllTASBsDone?true: false, tarSG.isAllTASBsDone?true: false);
									ASSG(origSG,
										SGFocused, SGDefocused, typeof(SGGreyoutProcess),
										SGWFA, SGFill, typeof(SGTransactionProcess), origSG.isPool? false: true);
									ASSG(tarSG,
										SGFocused, SGSelected, typeof(SGHighlightProcess),
										SGWFA, SGFill, typeof(SGTransactionProcess), tarSG.isPool? false: true);
									ASBSelState(testSB,
										SBSelected, SBDefocused, typeof(SBGreyoutProcess));
									if(testSB.sg.isPool)
										ASBActState(testSB,
											testSB.isStackable? SBWFNT: SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), false);
									else
										ASBActState(testSB,
											testSB.isStackable? SBWFNT: SBPickedUp, SBRemove, typeof(SBRemoveProcess), true);
									if(!tarSG.isPool){
										Slottable newSB = tarSG.GetSB(pickedItem);
										ASSB(newSB,
											SBDeactivated, SBDefocused, null,
											SBWFA, SBAdd, typeof(SBAddProcess), true,
											SBUnequipped, SBUnequipped, null);
									}
								sgm.dIcon1.CompleteMovement();
								if(!tarSG.isAllTASBsDone)
									CompleteAllSBActProcesses(tarSG);
								if(!origSG.isAllTASBsDone)
									CompleteAllSBActProcesses(origSG);
									ASSGM(sgm,
										null, null, null, null, null, null, null, null, 
										SGMDeactivated, SGMFocused, null,
										SGMTransaction, SGMWFA, null, 
										null, true, true, true, true);
									ASSG(origSG,
										SGDefocused, SGFocused, typeof(SGGreyinProcess),
										SGFill, SGWFA, null, false);
									ASSG(tarSG,
										SGSelected, SGFocused, typeof(SGDehighlightProcess),
										SGFill, SGWFA, null, false);
									if(origSG.isPool){//else, the sb is gone
											ASBActState(testSB,
												SBMoveWithin, SBWFA, null, false);
										if(!testSB.isPickable)
											ASBSelState(testSB,
												SBSelected, SBDefocused, typeof(SBGreyoutProcess));
										else
											ASBSelState(testSB,
												SBDefocused, SBFocused, typeof(SBGreyinProcess));
									}
									if(!tarSG.isPool){
										Slottable added = tarSG.GetSB(pickedItem);
										ASSB(added,
											SBDefocused, SBFocused, typeof(SBGreyinProcess),
											SBAdd, SBWFA, null, false,
											null, null, null);
									}
									AssertFocused();
							/*	reverse	*/
								Slottable revPickedSB = tarSG.GetSB(pickedItem);
								PickUp(revPickedSB, out picked);
								SimHover(null ,origSG, eventData);
								LetGo();
								if(!origSG.isPool)
									CompleteAllSBActProcesses(origSG);
								if(!tarSG.isPool)
									CompleteAllSBActProcesses(tarSG);
								sgm.dIcon1.CompleteMovement();
									AssertFocused();
					}
					foreach(Slottable tarSB in tarSG.slottables){
						testSB = origSG.GetSB(pickedItem);
						if(tarSB != null){
							if(sgm.GetTransaction(testSB, null, tarSB).GetType() == typeof(FillTransaction)){
								/*	OnSB	*/
									AssertFocused();
										ASSGM(sgm,
											null, null, null, null, null, null, null, null,
											SGMDeactivated, SGMFocused, null,
											null, SGMWFA, null,
											null, true, true, true, true);
										ASSG(origSG,
											null, SGFocused, null,
											null, SGWFA, null, false);
										ASSB(testSB,
											null, SBFocused, null,
											null, SBWFA, null, false,
											null, null, null);
									PickUp(testSB, out picked);
										ASSGM(sgm,
											testSB, null, null, null, testSB, null, null, testSB, 
											SGMDeactivated, SGMFocused, null,
											SGMWFA, SGMProbing, typeof(SGMProbeProcess), 
											typeof(RevertTransaction),
											false, true, true, true);
										ASSG(origSG,
											SGFocused, SGDefocused, typeof(SGGreyoutProcess),
											null, SGWFA, null, false);
										ASSB(testSB,
											SBSelected, SBDefocused, typeof(SBGreyoutProcess),
											testSB.isStackable?SBWFNT:SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
											null, null, null);
									SimHover(tarSB, null, eventData);
										ASSGM(sgm,
											testSB, null, origSG, tarSG, testSB, null, null, tarSB,  
											SGMDeactivated, SGMFocused, null,
											SGMWFA, SGMProbing, typeof(SGMProbeProcess), 
											typeof(FillTransaction),
											false, true, false, false);
										ASSG(origSG,
											SGFocused, SGDefocused, typeof(SGGreyoutProcess),
											null, SGWFA, null, false);
										ASSG(tarSG,
											null, SGSelected, typeof(SGHighlightProcess),
											null, SGWFA, null, false);
										ASSB(tarSB,
											null, SBDefocused, null,
											null, SBWFA, null, false,
											null, null, null);
										ASSB(testSB,
											SBSelected, SBDefocused, typeof(SBGreyoutProcess),
											testSB.isStackable?SBWFNT:SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
											null, null, null);
									LetGo();
										ASSGM(sgm,
											testSB, null, origSG, tarSG, testSB, null, null, tarSB,  
											SGMDeactivated, SGMFocused, null,
											SGMProbing, SGMTransaction, typeof(SGMTransactionProcess), 
											typeof(FillTransaction),
											false, true, origSG.isAllTASBsDone?true: false, tarSG.isAllTASBsDone?true:false);
										ASSG(origSG,
											SGFocused, SGDefocused, typeof(SGGreyoutProcess),
											SGWFA, SGFill, typeof(SGTransactionProcess), origSG.isAllTASBsDone?false:true);
										ASSG(tarSG,
											null, SGSelected, typeof(SGHighlightProcess),
											SGWFA, SGFill, typeof(SGTransactionProcess), tarSG.isAllTASBsDone?false:true);
										ASBSelState(tarSB,
											null, SBDefocused, null);
											if(tarSG.isPool)
												ASBActState(tarSB,
													SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), tarSB.slotID == tarSB.newSlotID?false: true);
											else
												ASBActState(tarSB,
													SBWFA, SBRemove, typeof(SBRemoveProcess), true);
										ASBSelState(testSB,
											SBSelected, SBDefocused, typeof(SBGreyoutProcess));
											if(origSG.isPool)
												ASBActState(testSB,
													SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), tarSB.slotID == tarSB.newSlotID? false: true);
											else
												ASBActState(testSB,
													SBPickedUp, SBRemove, typeof(SBRemoveProcess), true);

									if(!origSG.isAllTASBsDone)
										CompleteAllSBActProcesses(origSG);
									if(!tarSG.isAllTASBsDone)
										CompleteAllSBActProcesses(tarSG);
									sgm.dIcon1.CompleteMovement();
									AssertFocused();
								/*	reverse	*/
									PickUp(tarSG.GetSB(pickedItem), out picked);
									SimHover(null, origSG, eventData);
									LetGo();
									if(!origSG.isAllTASBsDone)
										CompleteAllSBActProcesses(origSG);
									if(!tarSG.isAllTASBsDone)
										CompleteAllSBActProcesses(tarSG);
									sgm.dIcon1.CompleteMovement();
									AssertFocused();
							}
						}
					}
				}
			}
		public void TestVolSortOnAll(){
			PerformOnAllSGAfterFocusing(TestVolSort);
			}
			public void TestVolSort(SlotGroup sg, bool isPAS){
				if(!sg.isAutoSort){
					sgm.SortSG(sg, SlotGroup.InverseItemIDSorter);
						if(sg.ActionProcess != null && sg.ActionProcess.IsRunning){
							ASSGM(sgm,
								null, null, sg, null, null, null, null, null, 
								SGMDeactivated, SGMFocused, null,
								SGMWFA, SGMTransaction, typeof(SGMTransactionProcess),
								typeof(SortTransaction), true, true, false, true);
							CompleteAllSBActProcesses(sg);
						}
							AssertFocused();
							AssertSBsSorted(sg, SlotGroup.InverseItemIDSorter);

					sgm.SortSG(sg, SlotGroup.AcquisitionOrderSorter);
						if(sg.ActionProcess != null && sg.ActionProcess.IsRunning){
							ASSGM(sgm,
								null, null, sg, null, null, null, null, null, 
								SGMDeactivated, SGMFocused, null,
								SGMWFA, SGMTransaction, typeof(SGMTransactionProcess),
								typeof(SortTransaction), true, true, false, true);
							CompleteAllSBActProcesses(sg);
						}
							AssertFocused();
							AssertSBsSorted(sg, SlotGroup.AcquisitionOrderSorter);
					
					sgm.SortSG(sg, SlotGroup.ItemIDSorter);
						if(sg.ActionProcess != null && sg.ActionProcess.IsRunning){
								ASSGM(sgm,
									null, null, sg, null, null, null, null, null, 
									SGMDeactivated, SGMFocused, null,
									SGMWFA, SGMTransaction, typeof(SGMTransactionProcess),
									typeof(SortTransaction), true, true, false, true);
							CompleteAllSBActProcesses(sg);
						}
							AssertFocused();
							AssertSBsSorted(sg, SlotGroup.ItemIDSorter);
				}
			}
		public void TestReorderOnAll(){
			PerformOnAllSBs(CrossTestReorder);
			PrintTestResult(null);
			}
			public void CrossTestReorder(Slottable testSB, bool isPAS){
				CrossTestSGs(TestReorder, testSB ,isPAS);
			}
			public void TestReorder(SlotGroup targetSG, Slottable testSB, bool isPAS, bool isTAS){
				if(testSB.isPickable){
					SlotGroup origSG = testSB.sg;
					foreach(Slottable targetSB in targetSG.slottables){
						if(sgm.GetTransaction(testSB, null, targetSB).GetType() == typeof(ReorderTransaction)){
							Capture(testSB.sgm, testSB, null, targetSB, isPAS, isTAS, TestElement.SB);
							// Print(testSB.SGM, testSB, null, targetSB, isPAS, isTAS, TestElement.SB);
								AssertFocused();
							PickUp(testSB, out picked);
								ASSGM(sgm,
									testSB, null, null, null, testSB, null, null, testSB, 
									SGMDeactivated, SGMFocused, null,
									SGMWFA, SGMProbing, typeof(SGMProbeProcess),
									typeof(RevertTransaction), false, true, true, true);
								ASSG(origSG,
									SGFocused, SGDefocused, typeof(SGGreyoutProcess),
									null, SGWFA, null, false);
							SimHover(targetSB, null, eventData);
								ASSGM(sgm,
									testSB, targetSB, origSG, null, testSB, null, null, targetSB, 
									SGMDeactivated, SGMFocused, null,
									SGMWFA, SGMProbing, typeof(SGMProbeProcess),
									typeof(ReorderTransaction), false, true, false, true);
								ASSG(origSG,
									SGFocused, SGDefocused, typeof(SGGreyoutProcess),
									null, SGWFA, null, false);
								ASSB(testSB,
									SBSelected, SBDefocused, typeof(SBGreyoutProcess),
									SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
									null, null, null);
								ASSB(targetSB,
									SBFocused, SBSelected, typeof(SBHighlightProcess),
									null, SBWFA, null, true,
									null, null, null);
							LetGo();
								ASSGM(sgm,
									testSB, targetSB, origSG, null, testSB, null, null, targetSB, 
									SGMDeactivated, SGMFocused, null,
									SGMProbing, SGMTransaction, typeof(SGMTransactionProcess),
									typeof(ReorderTransaction), false, true, origSG.isAllTASBsDone?true:false, true);
								ASSG(origSG,
									SGFocused, SGDefocused, typeof(SGGreyoutProcess),
									SGWFA, SGReorder, typeof(SGTransactionProcess), true);
								ASSB(testSB,
									SBSelected, SBDefocused, typeof(SBGreyoutProcess),
									SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), true,
									null, null, null);
								ASSB(targetSB,
									SBFocused, SBSelected, typeof(SBHighlightProcess),
									SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), true,
									null, null, null);
							CompleteAllSBActProcesses(origSG);
								ASSGM(sgm,
									testSB, targetSB, origSG, null, testSB, null, null, targetSB, 
									SGMDeactivated, SGMFocused, null,
									SGMProbing, SGMTransaction, typeof(SGMTransactionProcess),
									typeof(ReorderTransaction), false, true, true, true);
							sgm.dIcon1.CompleteMovement();
								AssertFocused();
							/*	reverse	*/
							PickUp(testSB, out picked);
							SimHover(targetSB, null, eventData);
							LetGo();
							CompleteAllSBActProcesses(origSG);
							sgm.dIcon1.CompleteMovement();
							AssertFocused();
						}
					}
				}
			}
		public void TestRevertOnAllSBs(){
			PerformOnAllSBs(CrossTestRevert);
			PrintTestResult(null);
			}
			public void CrossTestRevert(Slottable pickedSB, bool isPAS){
				CrossTestSGs(TestRevert, pickedSB, isPAS);
			}
			public void TestRevert(SlotGroup sg, Slottable pickedSB, bool isPAS, bool isTAS){
				if(pickedSB.isPickable){
					SlotGroup origSG = pickedSB.sg;
					sgm.SetPickedSB(pickedSB);
					SlotSystemTransaction ta = AbsSlotSystemTransaction.GetTransaction(pickedSB, null, sg);
					sgm.ResetAndFocus();
					if(ta is RevertTransaction){
						// Capture(sg.SGM, pickedSB, sg, null, isPAS, isTAS, TestElement.SGM);
							AssertFocused();
							ASSGM(pickedSB.sgm,
								null, null, null, null, null, null, null, null, 
								SGMDeactivated, SGMFocused, null,
								null, SGMWFA, null,
								null, true, true, true, true);
						PickUp(pickedSB, out picked);
							ASSGM(pickedSB.sgm,
								pickedSB, null, null, null, pickedSB, null, null, pickedSB,
								SGMDeactivated, SGMFocused, null,
								SGMWFA, SGMProbing, typeof(SGMProbeProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSB(pickedSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null);
							ASSG(sg,
								null, SGDefocused, null,
								null, SGWFA, null, false);
						SimHover(null, sg, eventData);
							ASSGM(pickedSB.sgm,
								pickedSB, null, null, null, pickedSB, null, sg, null, 
								SGMDeactivated, SGMFocused, null,
								SGMWFA, SGMProbing, typeof(SGMProbeProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(sg,
								null, SGDefocused, null,
								null, SGWFA, null, false);
						LetGo();
							ASSGM(pickedSB.sgm,
								pickedSB, null, null, null, pickedSB, null, sg, null, 
								SGMDeactivated, SGMFocused, null,
								SGMProbing, SGMTransaction, typeof(SGMTransactionProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								SGWFA, SGRevert, typeof(SGTransactionProcess), false);
							ASSB(pickedSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), false,
								null, null, null);
						sgm.dIcon1.CompleteMovement();
						// Print(sg.SGM, pickedSB, sg, null, isPAS, isTAS, TestElement.SGM);
						AssertFocused();
						foreach(Slottable sb in sg.slottables){
							if(sb != null){
								if(sgm.GetTransaction(pickedSB, null, sb).GetType() == typeof(RevertTransaction)){
									// Capture(sgm, pickedSB, null, sb, isPAS, isTAS, TestElement.SGM);
									AssertFocused();
										ASSGM(pickedSB.sgm,
											null, null, null, null, null, null, null, null, 
											SGMDeactivated, SGMFocused, null,
											null, SGMWFA, null,
											null, true, true, true, true);
									PickUp(pickedSB, out picked);
										ASSGM(pickedSB.sgm,
											pickedSB, null, null, null, pickedSB, null, null, pickedSB,
											SGMDeactivated, SGMFocused, null,
											SGMWFA, SGMProbing, typeof(SGMProbeProcess),
											typeof(RevertTransaction), false, true, true, true);
										ASSB(pickedSB,
											SBSelected, SBDefocused, typeof(SBGreyoutProcess),
											SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
											null, null, null);
										ASSG(sg,
											null, SGDefocused, null,
											null, SGWFA, null, false);
									SimHover(null, sg, eventData);
										ASSGM(pickedSB.sgm,
											pickedSB, null, null, null, pickedSB, null, sg, null, 
											SGMDeactivated, SGMFocused, null,
											SGMWFA, SGMProbing, typeof(SGMProbeProcess),
											typeof(RevertTransaction), false, true, true, true);
										ASSG(sg,
											null, SGDefocused, null,
											null, SGWFA, null, false);
									LetGo();
										ASSGM(pickedSB.sgm,
											pickedSB, null, null, null, pickedSB, null, sg, null, 
											SGMDeactivated, SGMFocused, null,
											SGMProbing, SGMTransaction, typeof(SGMTransactionProcess),
											typeof(RevertTransaction), false, true, true, true);
										ASSG(origSG,
											null, SGDefocused, null,
											SGWFA, SGRevert, typeof(SGTransactionProcess), false);
										ASSB(pickedSB,
											SBSelected, SBDefocused, typeof(SBGreyoutProcess),
											SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), false,
											null, null, null);
									// pickedSB.ActionProcess.Expire();
									sgm.dIcon1.CompleteMovement();
								}
							}
						}
					}
				}
			}
	/*	Transaction test preliminary	*/
		public void AssertEquippedOnAll(){
			AE(sgm.equippedBowInst, sgBow.slots[0].sb.itemInst);
			AE(sgm.equippedWearInst, sgWear.slots[0].sb.itemInst);
			AssertEquipped(sgm.equippedBowInst);
			AssertEquipped(sgm.equippedWearInst);
			AECGears(sgm.equippedCarriedGears, sgm.poolInv, sgm.equipInv);
		}
		public void TestSGActionStateSequence(){
			List<Slot> slots = new List<Slot>();
			sgpAll.SetSlots(slots);

			List<Slottable> newSBs = new List<Slottable>();
			newSBs.Add(defBowA_p);
			newSBs.Add(defWearA_p);
			newSBs.Add(null);
			newSBs.Add(defShieldA_p);
			sgpAll.SetNewSBs(newSBs);
			sgpAll.CreateNewSlots();
			sgpAll.SetSBsActStates();
			foreach(Slottable sb in sgpAll.allTASBs){
				if(sb != null)
				Debug.Log(Util.SBDebug(sb));
			}
			PrintSBsArray(sgpAll.newSBs);
			sgpAll.OnCompleteSlotMovementsV2();
			
			newSBs.Clear();
			newSBs.Add(null);
			newSBs.Add(defWearA_p);
			newSBs.Add(defShieldA_p);
			newSBs.Add(defBowA_p);
			sgpAll.SetNewSBs(newSBs);
			sgpAll.CreateNewSlots();
			sgpAll.SetSBsActStates();
			foreach(Slottable sb in sgpAll.allTASBs){
				if(sb != null)
				Debug.Log(Util.SBDebug(sb));
			}
			PrintSBsArray(sgpAll.newSBs);
			sgpAll.OnCompleteSlotMovementsV2();
			
			newSBs.Clear();
			newSBs.Add(defShieldA_p);
			newSBs.Add(defWearA_p);
			newSBs.Add(null);
			newSBs.Add(null);
			newSBs.Add(crfMWeaponA_p);
			sgpAll.SetNewSBs(newSBs);
			sgpAll.CreateNewSlots();
			sgpAll.SetSBsActStates();
			foreach(Slottable sb in sgpAll.allTASBs){
				if(sb != null)
				Debug.Log(Util.SBDebug(sb));
			}
			PrintSBsArray(sgpAll.newSBs);
			sgpAll.OnCompleteSlotMovementsV2();

			sgpAll.InstantSort();
			PrintSBsArray(sgpAll.slottables);
		}
		public void TestReorderSBsMethod(){
			List<Slottable> sbs = testSBs;
			PrintSBsArray(sbs);
			sbs.Shuffle();
			PrintSBsArray(sbs);
			// Util.ReorderSBs(sbs[0], sbs[3], ref sbs);
			sbs.Reorder(sbs[0], sbs[3]);
			PrintSBsArray(sbs);
			sbs.Reorder(sbs[sbs.Count - 1], sbs[0]);
			PrintSBsArray(sbs);
		}
		public void TestDraggedIconOnAll(){
			PerformOnAllSBs(TestDraggedIcon);
			}
			public void TestDraggedIcon(Slottable sb, bool isPAS){
				DraggedIcon di = new DraggedIcon(sb);
				sgm.SetDIcon1(di);
				sgm.SetTransaction(new EmptyTransaction());
				sgm.Transaction.Execute();
				ASSGM(sgm,
					null, null, null, null, sb, null, null, null, 
					SGMDeactivated, SGMFocused, null,
					SGMWFA, SGMTransaction, typeof(SGMTransactionProcess),
					typeof(EmptyTransaction), false, true, true, true);
				di.CompleteMovement();
				ASSGM(sgm,
					null, null, null, null, null, null, null, null, 
					SGMDeactivated, SGMFocused, null,
					SGMTransaction, SGMWFA, null,
					null, true, true, true, true);
			}
		public void TestAcceptSGTACompOnAll(){
			PerformOnAllSGAfterFocusing(TestAcceptSGTAComp);
			}
			public void TestAcceptSGTAComp(SlotGroup sg, bool isPAS){
				AssertFocused();
				sgm.SetSG1(sg);
				ASSGM(sgm,
					null, null, sg, null, null, null, null, null,
					SGMDeactivated, SGMFocused, null,
					null, SGMWFA, null,
					null, true, true, false, true);
				sgm.SetTransaction(new EmptyTransaction());
				sgm.Transaction.Execute();
				ASSGM(sgm,
					null, null, sg, null, null, null, null, null,
					SGMDeactivated, SGMFocused, null,
					SGMWFA, SGMTransaction, typeof(SGMTransactionProcess),
					typeof(EmptyTransaction), true, true, false, true);
				sgm.AcceptSGTAComp(sg);
				ASSGM(sgm,
					null, null, null, null, null, null, null, null,
					SGMDeactivated, SGMFocused, null,
					SGMTransaction, SGMWFA, null,
					null, true, true, true, true);
			}
		public void TestSGMStateTransition(){
			/*	Selecttion state */
					ASSGM(sgm,
					null, null, null, null, null, null, null, null, 
					SGMDeactivated, SGMFocused, null,
					SGMWFA, SGMWFA, null,
					null, true, true, true, true);
				sgm.SetSelState(SlotGroupManager.DefocusedState);
					ASSGM(sgm,
					null, null, null, null, null, null, null, null, 
					SGMFocused, SGMDefocused, typeof(SGMGreyoutProcess),
					SGMWFA, SGMWFA, null,
					null, true, true, true, true);
				sgm.SetSelState(SlotGroupManager.FocusedState);
					ASSGM(sgm,
					null, null, null, null, null, null, null, null, 
					SGMDefocused, SGMFocused, typeof(SGMGreyinProcess),
					SGMWFA, SGMWFA, null,
					null, true, true, true, true);
				sgm.SetSelState(SlotGroupManager.DeactivatedState);
					ASSGM(sgm,
					null, null, null, null, null, null, null, null, 
					SGMFocused, SGMDeactivated, null,
					SGMWFA, SGMWFA, null,
					null, true, true, true, true);
				sgm.SetSelState(SlotGroupManager.DefocusedState);
					ASSGM(sgm,
					null, null, null, null, null, null, null, null, 
					SGMDeactivated, SGMDefocused, null,
					SGMWFA, SGMWFA, null,
					null, true, true, true, true);
				sgm.SetSelState(SlotGroupManager.DeactivatedState);
					ASSGM(sgm,
					null, null, null, null, null, null, null, null, 
					SGMDefocused, SGMDeactivated, null,
					SGMWFA, SGMWFA, null,
					null, true, true, true, true);
				sgm.SetSelState(SlotGroupManager.FocusedState);
					ASSGM(sgm,
					null, null, null, null, null, null, null, null, 
					SGMDeactivated, SGMFocused, null,
					SGMWFA, SGMWFA, null,
					null, true, true, true, true);
			/*	Action state */	// maybe after transaction is done 
				// 	ASSGM(sgm,
				// 	SGMDeactivated, SGMFocused, null,
				// 	SGMWFA, SGMWFA, null,
				// 	null, null,
				// 	true, true, true, true);
				// sgm.SetActState(SlotGroupManager.ProbingState)
		}
		public void TestSGStateTransitionOnAll(){
			PerformOnAllSGAfterFocusing(TestSGStateTransition);
			PrintTestResult(null);
		}
			public void TestSGStateTransition(SlotGroup sg, bool isPAS){
				// CaptureSGState(sg, isPAS);
				/*	Selection State	*/
					// if(sg.PrevSelState == SGDeactivated)
					// 		ASSG(sg,
					// 			SGDeactivated, SGFocused, null,
					// 			SGWFA, SGWFA, null);
					// else 
					// 		ASSG(sg,
					// 			SGDefocused, SGFocused, typeof(SGGreyinProcess),
					// 			SGWFA, SGWFA, null);

					// 	sg.SetSelState(SGDefocused);
					// 		ASSG(sg,
					// 			SGFocused, SGDefocused, typeof(SGGreyoutProcess),
					// 			SGWFA, SGWFA, null);
					// 	sg.SetSelState(SGFocused);
					// 		ASSG(sg,
					// 			SGDefocused, SGFocused, typeof(SGGreyinProcess),
					// 			SGWFA, SGWFA, null);
					// 	sg.SetSelState(SGSelected);
					// 		ASSG(sg,
					// 			SGFocused, SGSelected, typeof(SGHighlightProcess),
					// 			SGWFA, SGWFA, null);
					// 	sg.SetSelState(SGFocused);
					// 		ASSG(sg,
					// 			SGSelected, SGFocused, typeof(SGDehighlightProcess),
					// 			SGWFA, SGWFA, null);
					// 	sg.SetSelState(SGDeactivated);
					// 		ASSG(sg,
					// 			SGFocused, SGDeactivated, null,
					// 			SGWFA, SGWFA, null);
					// 	sg.SetSelState(SGFocused);
					// 		ASSG(sg,
					// 			SGDeactivated, SGFocused, null,
					// 			SGWFA, SGWFA, null);
				/*	Action state	*/
			}
		public void TestSBStateTransitionOnAll(){
			PerformOnAllSBs(TestSBStateTransition);
			PrintTestResult(null);
		}
			public void TestSBStateTransition(Slottable sb, bool isPAS){
				/*	SelState */
					if(isPAS){
						if(sb.sg.isPool){
							if(sb.isEquipped || (sb.itemInst is PartsInstanceMock && !(sb.sg.Filter is SGPartsFilter))){
								if(sb.PrevSelState == SBDeactivated){
									ASBSelState(sb, SBDeactivated, SBDefocused, null);

								}else
									ASBSelState(sb, SBFocused, SBDefocused, typeof(SBGreyoutProcess));

								sb.SetSelState(SBSelected);
									ASBSelState(sb, SBDefocused, SBSelected, typeof(SBHighlightProcess));
								sb.SetSelState(SBDefocused);
									ASBSelState(sb, SBSelected, SBDefocused, typeof(SBGreyoutProcess));
								sb.SetSelState(SBFocused);
									ASBSelState(sb, SBDefocused, SBFocused, typeof(SBGreyinProcess));
								return;
							}
						}
					}
					if(sb.PrevSelState == SBDeactivated)
						ASBSelState(sb, SBDeactivated, SBFocused, null);
					else
						ASBSelState(sb, SBDefocused, SBFocused, typeof(SBGreyinProcess));
					sb.SetSelState(SBSelected);
						ASBSelState(sb, SBFocused, SBSelected, typeof(SBHighlightProcess));
					sb.SetSelState(SBFocused);
						ASBSelState(sb, SBSelected, SBFocused, typeof(SBDehighlightProcess));
					sb.SetSelState(SBDeactivated);
						ASBSelState(sb, SBFocused, SBDeactivated, null);
					sb.SetSelState(SBFocused);
						ASBSelState(sb, SBDeactivated, SBFocused, null);
					sb.SetSelState(SBDeactivated);
						ASBSelState(sb, SBFocused, SBDeactivated, null);
					sb.SetSelState(SBDefocused);
						ASBSelState(sb, SBDeactivated, SBDefocused, null);
					sb.SetSelState(SBDeactivated);
						ASBSelState(sb, SBDefocused, SBDeactivated, null);
					sb.SetSelState(SBSelected);
						ASBSelState(sb, SBDeactivated, SBSelected, null);
					sb.SetSelState(SBDeactivated);
						ASBSelState(sb, SBSelected, SBDeactivated, null);
				/*	Equip State	*/
				if(sb.sg.isPool){
					if(sb.isEquipped){
							ASBEqpState(sb, SBUnequipped, SBEquipped, typeof(SBEquipProcess));
						sb.SetEqpState(SBUnequipped);
							ASBEqpState(sb, SBEquipped, SBUnequipped, typeof(SBUnequipProcess));
						sb.SetEqpState(SBEquipped);
							ASBEqpState(sb, SBUnequipped, SBEquipped, typeof(SBEquipProcess));
					}
					else{
						if(sb.PrevEqpState == SBUnequipped)
							ASBEqpState(sb, SBUnequipped, SBUnequipped, null);
						else
							ASBEqpState(sb, SBEquipped, SBUnequipped, typeof(SBUnequipProcess));

						sb.SetEqpState(SBEquipped);
							ASBEqpState(sb, SBUnequipped, SBEquipped, typeof(SBEquipProcess));
						sb.SetEqpState(SBUnequipped);
							ASBEqpState(sb, SBEquipped, SBUnequipped, typeof(SBUnequipProcess));
					}
				}else{
						ASBEqpState(sb, SBUnequipped, SBEquipped, null);
					sb.SetEqpState(SBUnequipped);
						ASBEqpState(sb, SBEquipped, SBUnequipped, null);
					sb.SetEqpState(SBEquipped);
						ASBEqpState(sb, SBUnequipped, SBEquipped, null);
				}
			}
		public void TestPickUpTransitionOnAll(){
			PerformOnAllSBs(TestPickUpTransition);
			// PrintTestResult(null);
		}
			public void TestPickUpTransition(Slottable sb, bool isPAS){
					AssertFocused();
					SlotGroup origSG = sb.sg;
				if(sb.isPickable){
					/*	tap	*/
						sb.OnPointerDownMock(eventData);
							ASSGM(sgm,
								null, null, null, null, null, null, null, null,
								SGMDeactivated, SGMFocused, null,
								null, SGMWFA, null,
								null, true, true, true, true);
							ASSG(origSG,
								null, SGFocused, null,
								null, SGWFA, null, false);
							ASSB(sb,
								SBFocused, SBSelected, typeof(SBHighlightProcess),
								SBWFA, SBWFPickUp, typeof(WaitForPickUpProcess), true,
								null, null, null);
						sb.OnPointerUpMock(eventData);
							ASSB(sb,
								SBFocused, SBSelected, typeof(SBHighlightProcess),
								SBWFPickUp, SBWFNT, typeof(WaitForNextTouchProcess), true,
								null, null, null);
						sb.ActionProcess.Expire();
							ASSB(sb,
								SBSelected, SBFocused, typeof(SBDehighlightProcess),
								SBWFNT, SBWFA, null, false,
								null, null, null);
						AssertFocused();
					/*	multi tap -> pickup	*/
						sb.OnPointerDownMock(eventData);
						sb.OnPointerUpMock(eventData);
						sb.OnPointerDownMock(eventData);
							ASSGM(sgm,
								sb, null, null, null, sb, null, null, sb,
								SGMDeactivated, SGMFocused, null,
								SGMWFA, SGMProbing, typeof(SGMProbeProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								null, SGWFA, null, false);
							ASSB(sb,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFNT, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null);							
						sb.OnPointerUpMock(eventData);
						if(sb.isStackable){
							ASSB(sb,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBPickedUp, SBWFNT, typeof(WaitForNextTouchProcess), true,
								null, null, null);
							sb.ActionProcess.Expire();
						}
							/*reverting*/
							ASSGM(sgm,
								sb, null, null, null, sb, null, null, sb,
								SGMDeactivated, SGMFocused, null,
								SGMProbing, SGMTransaction, typeof(SGMTransactionProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								SGWFA, SGRevert, typeof(SGTransactionProcess), false);
							ASSB(sb,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								sb.isStackable?SBWFNT:SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), false,
								null, null, null);
						// sb.ActionProcess.Expire();
						sgm.dIcon1.CompleteMovement();
						AssertFocused();
					/*	pickup -> release -> expire to revert	*/
						PickUp(sb, out picked);
						LetGo();
							ASSB(sb,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								sb.isStackable?SBWFNT:SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), false,
								null, null, null);
						// sb.ActionProcess.Expire();
						sgm.dIcon1.CompleteMovement();
						AssertFocused();
					/* pickup -> release -> touch	*/
						if(sb.isStackable){
							PickUp(sb, out picked);
							sb.OnPointerUpMock(eventData);
							sb.OnPointerDownMock(eventData);
							LetGo();
							// sb.ActionProcess.Expire();
							sgm.dIcon1.CompleteMovement();
							AssertFocused();
						}
				}else{
					sb.OnPointerDownMock(eventData);
						ASSGM(sgm,
							null, null, null, null, null, null, null, null,
							SGMDeactivated, SGMFocused, null,
							null, SGMWFA, null,
							null, true, true, true, true);
						ASSB(sb,
							null, SBDefocused, null,
							SBWFA, SBWFPointerUp, typeof(WaitForPointerUpProcess), true,
							null,null,null);
					sb.OnPointerUpMock(eventData);
					AssertFocused();
				}
			}
		
		public void CheckPickcableOnAllSB(){
			PerformOnAllSBs(CheckPickable);
			// PrintTestResult(null);
			}public void CheckPickable(Slottable sb, bool isPas){
				bool pickable = sb.isPickable;
				Debug.Log(Util.SBofSG(sb)+ " isPickable: " + (pickable?Util.Blue("true"): Util.Red("false")));
				// SlotSystemTestResult res = new SlotSystemTestResult(isPas, sb, pickable);
				// testResults.Add(res);
			}
		public void CheckTransacitonWithSBSpecifiedOnAll(){
			PerformOnAllSBs(CheckTransactionWithSB);
			PrintTestResult(null);
			}public void CheckTransactionWithSB(Slottable sb, bool isPickedAS){
				CrossTestSGs(CrossCheckTransactionWithSB, sb, isPickedAS);
			}
			public void CrossCheckTransactionWithSB(SlotGroup sg, Slottable pickedSB, bool isPickedAS, bool isTargetAS){
				if(pickedSB.isPickable){
					foreach(Slottable sb in sg.slottables){
						if(sb != null){
							Capture(sb.sgm, pickedSB, sg, sb, isPickedAS, isTargetAS, TestElement.TA);
						}
					}
				}
			}
		public void CheckShrinkableAndExpandableOnAllSGs(){
			foreach(SlotGroup sg in sgm.allSGs){
				string shrinkStr = " Shrinkable: " + sg.isShrinkable;
				if(sg.isShrinkable)	shrinkStr = Blue(shrinkStr);
				else shrinkStr = Red(shrinkStr);
				string expandStr = " Expandable: " + sg.isExpandable;
				if(sg.isExpandable)	expandStr = Blue(expandStr);
				else expandStr = Red(expandStr);
				Debug.Log(Name(sg) + shrinkStr + ", " + expandStr);
			}
		}
		public void CheckSwappableOnAll(){
			PerformOnAllSBs(CrossCheckSwappable);
			PrintTestResult(0.ToString());
			}public void CrossCheckSwappable(Slottable sb, bool isPickedAS){
				CrossTestSGs(CheckSwappable, sb, isPickedAS);
			}
			public void CheckSwappable(SlotGroup sg, Slottable sb, bool isPickedAS ,bool isTargetAS){
				int count = sg.SwappableSBs(sb).Count;
				SlotSystemTestResult newRes = new SlotSystemTestResult(sg.sgm, isPickedAS, isTargetAS, sb, sg, null, count);
				testResults.Add(newRes);
			}
		public void CheckTransactionOnAllSG(){
				PerformOnAllSBs(CrossCheckTransaction);
				PrintTestResult(null);
				}public void CrossCheckTransaction(Slottable sb, bool isPickedAS){
					CrossTestSGs(CheckTransaction, sb, isPickedAS);
				}
				public void CheckTransaction(SlotGroup sg, Slottable sb, bool isPAS ,bool isTAS){
					if(sb.isPickable){
						Capture(sg.sgm, sb, sg, null, isPAS, isTAS, TestElement.TA);
					}
				}
	/*	thorough testing utility	*/
		public List<List<Slottable>> possibleSBsCombos(int subsetCount, List<Slottable> sbs){
			List<List<Slottable>> result = new List<List<Slottable>>();
			foreach(List<Slottable> combo in ListMethods.Combinations<Slottable>(subsetCount, sbs)){
				List<List<Slottable>> perms = ListMethods.Permutations<Slottable>(combo);
				result.AddRange(perms);
			}
			return result;
			}public void TestPermutation(){
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_p);
				sbs.Add(defWearA_p);
				sbs.Add(defShieldA_p);
				sbs.Add(defQuiverA_p);
				sbs.Add(defParts_p);
				List<List<Slottable>> sbsPerms = ListMethods.Permutations<Slottable>(sbs);
				foreach(List<Slottable> sbList in sbsPerms){
					PrintSBsArray(sbList);
				}
			}
			public void TestCombination(){
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_p);
				sbs.Add(defWearA_p);
				sbs.Add(defQuiverA_p);
				sbs.Add(defMWeaponA_p);
				sbs.Add(defParts_p);
				
				List<List<Slottable>> possibleCombos = possibleSBsCombos(4, sbs);
				foreach(List<Slottable> combo in possibleCombos)
					PrintSBsArray(combo);
			}
		public List<Slottable> transactableSBs(SlotGroup origSG, SlotGroup tarSG, Slottable tarSB, System.Type ta){
			List<Slottable> result = new List<Slottable>();
			foreach(Slottable sb in origSG.slottables){
				if(sb != null && sb.isPickable){
					if(sgm.GetTransaction(sb, tarSG, tarSB).GetType() == ta){
						result.Add(sb);
					}
				}
			}
			return result;
		}
		public void PerformOnAllSBs(System.Action<Slottable, bool> act){
			foreach(SlotGroup sgp in sgm.allSGPs){
				sgm.SetFocusedPoolSG(sgp);
				sgp.ToggleAutoSort(true);
				foreach(Slottable sb in sgp.slottables){
					if(sb != null){
						act(sb, true);
					}
				}
				sgp.ToggleAutoSort(false);
				foreach(Slottable sb in sgp.slottables){
					if(sb != null){
						act(sb, false);
					}
				}
			}
			foreach(EquipmentSet eSet in sgm.equipmentSets){
				sgm.SetFocusedEquipmentSet(eSet);
				foreach(SlotGroup sge in sgm.focusedSGEs){
					sge.ToggleAutoSort(true);
					foreach(Slottable sb in sge.slottables){
						if(sb != null){
							act(sb, true);
						}
					}
					sge.ToggleAutoSort(false);
					foreach(Slottable sb in sge.slottables){
						if(sb != null){
							act(sb, false);
						}
					}
				}
			}
		}
		public void PerformOnAllSGAfterFocusing(System.Action<SlotGroup, bool> act){
			foreach(SlotGroup sgp in sgm.allSGPs){
				sgm.SetFocusedPoolSG(sgp);
				sgp.ToggleAutoSort(true);
				act(sgp, true);
				sgp.ToggleAutoSort(false);
				act(sgp, false);
			}
			foreach(EquipmentSet eSet in sgm.equipmentSets){
				sgm.SetFocusedEquipmentSet(eSet);
				foreach(SlotGroup sge in sgm.focusedSGEs){
					sge.ToggleAutoSort(true);
					act(sge, true);
					sge.ToggleAutoSort(false);
					act(sge, false);
				}
			}
		}
		public void CrossTestSGs(System.Action<SlotGroup, Slottable, bool, bool> act, Slottable sb, bool isPickedAS){
			if(sb.sg.isPool){
				act(sb.sg, sb, isPickedAS, isPickedAS);
				foreach(EquipmentSet eSet in sgm.equipmentSets){
					sgm.SetFocusedEquipmentSet(eSet);
					foreach(SlotGroup sge in sgm.focusedSGEs){
						sge.ToggleAutoSort(true);
						act(sge, sb, isPickedAS, true);
						sge.ToggleAutoSort(false);
						act(sge, sb, isPickedAS, false);
					}
				}
			}else if(sb.sg.isSGE){
				foreach(SlotGroup sgp in sgm.allSGPs){
					sgm.SetFocusedPoolSG(sgp);
					sgp.ToggleAutoSort(true);
					act(sgp, sb, isPickedAS, true);
					sgp.ToggleAutoSort(false);
					act(sgp, sb, isPickedAS, false);
				}
				foreach(SlotGroup sge in sgm.focusedSGEs){
					sge.ToggleAutoSort(true);
					act(sge, sb, isPickedAS, true);
					sge.ToggleAutoSort(false);
					act(sge, sb, isPickedAS, false);
				}
			}
		}
		public void PerformOnAllSGs(System.Action<SlotGroup> act){
			foreach(SlotGroup sgp in sgm.allSGs){
				act(sgp);
			}
		}
	/*	actions	*/
		public void ClearSGCGearsTo(SlotGroup sgp){
			foreach(Slottable sb in sgCGears.slottables){
				if(sb != null){
					Fill(sb, sgCGears, sgp, null);
				}
			}
			AssertSGCounts(sgCGears, sgm.equipInv.equippableCGearsCount, 0, 0);
			AssertFocused();
		}
		public void Swap(Slottable testSB, SlotGroup origSG, SlotGroup tarSG, Slottable tarSB){
			AssertFocused();
			PickUp(testSB, out picked);
			if(tarSG != null && tarSB == null)
				SimHover(null, tarSG, eventData);
			else if(tarSB != null && tarSG == null)
				SimHover(tarSB, null, eventData);
			else
				throw new System.InvalidOperationException("SlottableTest.Swap: argument not set correctly");
			LetGo();
			if(!origSG.isAllTASBsDone)
				CompleteAllSBActProcesses(origSG);
			if(tarSG == null)
				tarSG = tarSB.sg;
			if(!tarSG.isAllTASBsDone)
				CompleteAllSBActProcesses(tarSG);
			sgm.dIcon1.CompleteMovement();
			sgm.dIcon2.CompleteMovement();
			AssertFocused();
			}public void TestSwapShortcut(){
			PerformOnAllSBs(CrossTestSwapShortcut);
			PrintTestResult(null);
			}
			public void CrossTestSwapShortcut(Slottable sb, bool isPAS){
				CrossTestSGs(TestSwapShortcut, sb, isPAS);
				origSGCache = null;
			}
			public void TestSwapShortcut(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				InventoryItemInstanceMock testItem = testSB.itemInst;
				SlotGroup origSG = testSB.sg;
				if(origSG != null)
					origSGCache = origSG;
				else
					origSG = origSGCache;
				testSB = origSG.GetSB(testItem);
				if(testSB.isPickable){
					SlotSystemTransaction ta = sgm.GetTransaction(testSB, tarSG, null);
					if(ta.GetType() == typeof(SwapTransaction)){
						InventoryItemInstanceMock swapItem = ta.targetSB.itemInst;
						Swap(testSB, origSG, tarSG, null);
						/*	reverse */
						Swap(origSG.GetSB(swapItem), origSG, tarSG, null);
						// Capture(sgm, testSB, tarSG, null, isPAS, isTAS, TestElement.SG);
					}
				}
				foreach(Slottable tarSB in tarSG.slottables){
					if(tarSB != null){
						testSB = origSG.GetSB(testItem);
						if(testSB.isPickable){
							SlotSystemTransaction ta = sgm.GetTransaction(testSB, null, tarSB);
							if(ta.GetType() == typeof(SwapTransaction)){
								Capture(sgm, testSB, null, tarSB, isPAS, isTAS, TestElement.SB);
								InventoryItemInstanceMock swapItem = tarSB.itemInst;
								Swap(testSB, origSG, null, tarSB);
								/*	reverse	*/
								Swap(tarSG.GetSB(testItem), tarSG, null, origSG.GetSB(swapItem));
							}
						}
					}
				}
			}

		public void Fill(Slottable testSB, SlotGroup origSG, SlotGroup tarSG, Slottable tarSB){
			AssertFocused();
			PickUp(testSB, out picked);
			if(tarSG != null)
				SimHover(null, tarSG, eventData);
			else if(tarSB != null){
				SimHover(tarSB, null, eventData);
				tarSG = testSB.sgm.Transaction.sg2;
			}
			LetGo();
			if(!origSG.isAllTASBsDone)
				CompleteAllSBActProcesses(origSG);
			if(!tarSG.isAllTASBsDone)
				CompleteAllSBActProcesses(tarSG);
			sgm.dIcon1.CompleteMovement();
			AssertFocused();
			}public void TestFillShortcut(){
			PerformOnAllSBs(CrossTestFillShortcut);
			PrintTestResult(null);
			}
			public void CrossTestFillShortcut(Slottable sb, bool isPAS){
				CrossTestSGs(TestFillShortcut, sb, isPAS);
				origSGCache = null;
			}
			public void TestFillShortcut(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				InventoryItemInstanceMock testItem = testSB.itemInst;
				SlotGroup origSG = testSB.sg;
				if(origSG != null)
					origSGCache = origSG;
				else
					origSG = origSGCache;
				testSB = origSG.GetSB(testItem);
				if(testSB.isPickable){
					if(sgm.GetTransaction(testSB, tarSG, null).GetType() == typeof(FillTransaction)){
						Fill(testSB, origSG, tarSG, null);
						/*	rev */
						Fill(tarSG.GetSB(testItem), tarSG, origSG, null);
					}
					foreach(Slottable tarSB in tarSG.slottables){
						if(tarSB != null){
							testSB = origSG.GetSB(testItem);
							if(sgm.GetTransaction(testSB, null, tarSB).GetType() == typeof(FillTransaction)){
								Fill(testSB, origSG, null, tarSB);
								/*	rev */
								Fill(tarSG.GetSB(testItem), tarSG, origSG, null);
							}
						}
					}
				}
			}

		public void PickUp(Slottable sb, out bool pickedUp){
			SlotGroup origSG = sb.sg;
			AssertFocused();
			if(sb.isPickable){
				sb.OnPointerDownMock(eventData);
					ASSGM(sb.sgm,
						null, null, null, null, null, null, null, null, 
						null, SGMFocused, null,
						null, SGMWFA, null,
						null, true, true, true, true);
					ASSG(origSG,
						null, SGFocused, null,
						null, SGWFA, null, false);
					ASSB(sb,
						SBFocused, SBSelected, typeof(SBHighlightProcess),
						SBWFA, SBWFPickUp, typeof(WaitForPickUpProcess), true, 
						null, null, null);
				sb.ActionProcess.Expire();
					ASSGM(sb.sgm,
						sb, null, null, null, sb, null, null, sb, 
						null, SGMFocused, null,
						SGMWFA, SGMProbing, typeof(SGMProbeProcess),
						typeof(RevertTransaction), false, true, true, true);
					ASSG(origSG,
						SGFocused, SGDefocused, typeof(SGGreyoutProcess),
						null, SGWFA, null, false);
					ASSB(sb,
						SBSelected, SBDefocused, typeof(SBGreyoutProcess),
						SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true, 
						null, null, null);
				pickedUp = true;
			}else{
				pickedUp = false;
			}
		}
		public void PointerUp(){
			sgm.pickedSB.OnPointerUpMock(eventData);
		}
		public void LetGo(){
			Slottable pickedSB = sgm.pickedSB;
				ASGMActState(sgm, SGMWFA, SGMProbing, typeof(SGMProbeProcess));
				ASSG(pickedSB.sg,
					SGFocused, SGDefocused, typeof(SGGreyoutProcess),
					null, SGWFA, null, false);
				ASSB(pickedSB,
					SBSelected, SBDefocused, typeof(SBGreyoutProcess),
					null, SBPickedUp, null, false,
					null, null, null);
			sgm.pickedSB.OnPointerUpMock(eventData);
			if(sgm.pickedSB.CurActState == Slottable.WaitForNextTouchState){
				ASGMActState(sgm, SGMWFA, SGMProbing, typeof(SGMProbeProcess));
				ASSG(pickedSB.sg,
					SGFocused, SGDefocused, typeof(SGGreyoutProcess),
					null, SGWFA, null, false);
				ASSB(pickedSB,
					SBSelected, SBDefocused, typeof(SBGreyoutProcess),
					SBPickedUp, SBWFNT, typeof(WaitForNextTouchProcess), true,
					null, null, null);
				sgm.pickedSB.ActionProcess.Expire();
			}
				ASGMActState(sgm, SGMProbing, SGMTransaction, typeof(SGMTransactionProcess));
				ASGSelState(pickedSB.sg,
					SGFocused, SGDefocused, typeof(SGGreyoutProcess));
		}
		public void CompleteAllSBActProcesses(SlotGroup sg){
			foreach(Slottable sb in sg.allTASBs){
				if(sb != null){
					if(sb.ActionProcess.GetType() == typeof(SBRemoveProcess) ||
					sb.ActionProcess.GetType() == typeof(SBAddProcess) ||
					sb.ActionProcess.GetType() == typeof(SBMoveWithinProcess))
						if(!sb.ActionProcess.IsExpired)
							sb.ActionProcess.Expire();
				}
				// if(sb != sg.SGM.pickedSB && sb != sg.SGM.sg2){
				// }
			}
			sg.CheckProcessCompletion();
		}
		public void SimHover(Slottable sb, SlotGroup sg, PointerEventDataMock eventData){
			/*	revised version
					sgm.SetHovered(sb, sg);
						=> update hovered fields
					sgm.UpdateTransaction();
						=> update target fields
							=> thus the selection states of prev and current tartgets
			*/
			/*	in actual implementation, this method is called whenever either sb or sg's boarder is crossed
			*/
			sgm.SetHoveredSB(sb); sgm.SetHoveredSG(sg);
			sgm.CreateTransactionResults();
			sgm.UpdateTransaction();
		}
	/*	Assertions	*/
		/*	SGM	*/
			public void ASGMSelState(SlotGroupManager sgm, SGMSelectionState prev, SGMSelectionState cur, System.Type selProcT){
				AE(sgm.CurSelState, cur);
				if(prev != null){
					AE(sgm.PrevSelState, prev);
					if(selProcT != null)
						AE(sgm.SelectionProcess.GetType(), selProcT);
					else
						ANull(sgm.SelectionProcess);
				}
			}
			public void ASGMActState(SlotGroupManager sgm, SGMActionState prev, SGMActionState cur, System.Type actProcT){
				AE(sgm.CurActState, cur);
				if(prev != null){
					AE(sgm.PrevActState, prev);
					if(actProcT != null)
						AE(sgm.ActionProcess.GetType(), actProcT);
					else
						ANull(sgm.ActionProcess);
				}
			}
			// public void ASSGM(SlotGroupManager sgm,
				// 	Slottable pickedSB, SlotGroup hoveredSG, Slottable hoveredSB, SlotGroup targetSG, Slottable targetSB, 
				// 	SGMSelectionState prevSel, SGMSelectionState curSel, System.Type selProcT,
				// 	SGMActionState prevAct, SGMActionState curAct, System.Type actProcT,
				// 	System.Type taType, bool pSBDone, bool sSBDone, bool oSGDone, bool sSGDone){
				// 	AE(sgm.pickedSB, pickedSB);
				// 	AE(sgm.HoveredSG, hoveredSG);
				// 	AE(sgm.HoveredSB, hoveredSB);
				// 	AE(sgm.sg2, targetSG);
				// 	AE(sgm.sg2, targetSB);
				// 	ASGMSelState(sgm, prevSel, curSel, selProcT);
				// 	ASGMActState(sgm, prevAct, curAct, actProcT);
				// 	if(taType == null) ANull(sgm.Transaction);
				// 	else AE(sgm.Transaction.GetType(), taType);
				// 	AE(sgm.pickedSBDone, pSBDone);
				// 	AE(sgm.targetSBdone, sSBDone);
				// 	AE(sgm.sg1Done, oSGDone);
				// 	AE(sgm.sg2Done, sSGDone);
				// }
			public void ASSGM(SlotGroupManager sgm,
				Slottable pickedSB, Slottable targetSB, SlotGroup sg1, SlotGroup sg2, 
				Slottable di1SB, Slottable di2SB, SlotGroup hoveredSG, Slottable hoveredSB,
				SGMSelectionState prevSel, SGMSelectionState curSel, System.Type selProcT,
				SGMActionState prevAct, SGMActionState curAct, System.Type actProcT,
				System.Type taType, bool d1Done, bool d2Done, bool sg1Done, bool sg2Done){
				AE(sgm.pickedSB, pickedSB);
				AE(sgm.targetSB, targetSB);
				AE(sgm.sg1, sg1);
				AE(sgm.sg2, sg2);
				if(di1SB != null)
					AE(sgm.dIcon1.item, di1SB.itemInst);
				else
					AE(sgm.dIcon1, null);
				if(di2SB != null)
					AE(sgm.dIcon2.item, di2SB.itemInst);
				else
					AE(sgm.dIcon2, null);
				AE(sgm.hoveredSG, hoveredSG);
				AE(sgm.hoveredSB, hoveredSB);
				ASGMSelState(sgm, prevSel, curSel, selProcT);
				ASGMActState(sgm, prevAct, curAct, actProcT);
				if(taType == null) ANull(sgm.Transaction);
				else AE(sgm.Transaction.GetType(), taType);
				AE(sgm.dIcon1Done, d1Done);
				AE(sgm.dIcon2Done, d2Done);
				AE(sgm.sg1Done, sg1Done);
				AE(sgm.sg2Done, sg2Done);
			}
			public void ASSGM(SlotGroupManager sgm,
				Slottable pickedSB, Slottable targetSB, SlotGroup sg1, SlotGroup sg2, 
				DraggedIcon di1, DraggedIcon di2, SlotGroup hoveredSG, Slottable hoveredSB,
				SGMSelectionState prevSel, SGMSelectionState curSel, System.Type selProcT,
				SGMActionState prevAct, SGMActionState curAct, System.Type actProcT){
				AE(sgm.pickedSB, pickedSB);
				AE(sgm.targetSB, targetSB);
				AE(sgm.sg1, sg1);
				AE(sgm.sg2, sg2);
				AE(sgm.dIcon1, di1);
				AE(sgm.dIcon2, di2);
				AE(sgm.hoveredSG, hoveredSG);
				AE(sgm.hoveredSB, hoveredSB);
				ASGMSelState(sgm, prevSel, curSel, selProcT);
				ASGMActState(sgm, prevAct, curAct, actProcT);
			}
			// public void ASSGM(SlotGroupManager sgm,
				// 	Slottable pickedSB, SlotGroup hoveredSG, Slottable hoveredSB, SlotGroup targetSG, Slottable targetSB, 
				// 	SGMSelectionState prevSel, SGMSelectionState curSel, System.Type selProcT,
				// 	SGMActionState prevAct, SGMActionState curAct, System.Type actProcT){
				// 	AE(sgm.pickedSB, pickedSB);
				// 	AE(sgm.HoveredSG, hoveredSG);
				// 	AE(sgm.HoveredSB, hoveredSB);
				// 	AE(sgm.sg2, targetSG);
				// 	AE(sgm.sg2, targetSB);
				// 	ASGMSelState(sgm, prevSel, curSel, selProcT);
				// 	ASGMActState(sgm, prevAct, curAct, actProcT);
				// }
		/*	SG	*/
			public void ASGSelState(SlotGroup sg, SGSelectionState prev, SGSelectionState cur, System.Type procT){
				AE(sg.CurSelState, cur);
				if(prev != null){
					AE(sg.PrevSelState, prev);
					if(procT != null)
						AE(sg.SelectionProcess.GetType(), procT);
					else
						ANull(sg.SelectionProcess);
				}
			}
			public void ASGActState(SlotGroup sg, SGActionState prev, SGActionState cur, System.Type procT, bool isRunning){
				AE(sg.CurActState, cur);
				if(prev != null){
					AE(sg.PrevActState, prev);
					if(procT != null){
						AE(sg.ActionProcess.GetType(), procT);
						AE(sg.ActionProcess.IsRunning, isRunning);
					}
					else
						ANull(sg.ActionProcess);
				}
			}
			public void AssertSGCounts(SlotGroup sg, int slotsC, int itemC, int sbsC){
				AE(sg.slots.Count, slotsC);
				AE(sg.actualItemInsts.Count, itemC);
				AE(sg.actualSBsCount, sbsC);
			}
			public void AssertSBsSorted(SlotGroup sg, SGSorter sorter){
				List<Slottable> sbs = sg.slottables;
				sorter.OrderSBsWithRetainedSize(ref sbs);
				AE(sbs.Count, sg.slottables.Count);
				for(int i = 0; i < sg.slottables.Count; i++){
					AE(sg.slottables[i], sbs[i]);
				}
			}
			public void ASSG(SlotGroup sg, SGSelectionState prevSel, SGSelectionState curSel, System.Type selProcT, SGActionState prevAct, SGActionState curAct, System.Type actProcT, bool isRunning){
				ASGSelState(sg, prevSel, curSel, selProcT);
				ASGActState(sg, prevAct, curAct, actProcT, isRunning);
			}
			public void ASGReset(SlotGroup sg){
				ASGActState(sg, null, SGWFA, null, false);
				AE(sg.allTASBs, null);
				AE(sg.newSBs, null);
				AE(sg.newSlots, null);
			}
		/*	SB	*/
			public void ASSB(Slottable sb,
			SBSelectionState prevSel, SBSelectionState curSel , System.Type selProcT,
			SBActionState prevAct, SBActionState curAct, System.Type actProcT, bool isRunning,
			SBEquipState prevEqp, SBEquipState curEqp, System.Type eqpProcT){
				ASBSelState(sb, prevSel, curSel, selProcT);
				ASBActState(sb, prevAct, curAct, actProcT, isRunning);
				if(curEqp != null)
					ASBEqpState(sb, prevEqp, curEqp, eqpProcT);
			}
			public void ASSB_s(Slottable sb, SBSelectionState selState, SBActionState actState){
				AE(sb.CurSelState, selState);
				AE(sb.CurActState, actState);
			}
			public void ASBSelState(Slottable sb, SBSelectionState prev, SBSelectionState cur, System.Type procT){
				if(prev != null){
					AE(sb.PrevSelState, prev);
					if(procT != null)
						AE(sb.SelectionProcess.GetType(), procT);
					else
						ANull(sb.SelectionProcess);
				}
				AE(sb.CurSelState, cur);
			}
			public void ASBActState(Slottable sb, SBActionState prev, SBActionState cur, System.Type procT, bool isRunning){
				if(prev != null){
					AE(sb.PrevActState, prev);
					if(procT != null){
						AE(sb.ActionProcess.GetType(), procT);
						AE(sb.ActionProcess.IsRunning, isRunning);
					}
					else
						ANull(sb.ActionProcess);
				}
				AE(sb.CurActState, cur);
			}
			public void ASBEqpState(Slottable sb, SBEquipState prev, SBEquipState cur, System.Type procT){
				if(prev != null){
					AE(sb.PrevEqpState, prev);
					if(procT != null)
						AE(sb.EquipProcess.GetType(), procT);
					else
						ANull(sb.EquipProcess);
				}
				AE(sb.CurEqpState, cur);
			}
			public void ASBReset(Slottable sb){
				ASBActState(sb, null, SBWFA, null, false);
				AE(sb.pickedAmount, 0);
				AE(sb.newSlotID, -2);
			}
		/*	other	*/
			public void ANotNull(object obj){
				// Assert.That(obj, Is.Not.Null);
				AB(obj != null, true);
			}
			public void ANull(object obj){
				AB(obj == null, true);
			}
			public void AE(object inspected, object expected){
				if(expected != null)
					Assert.That(inspected, Is.EqualTo(expected));
				else
					ANull(inspected);
			}
			public void AB(bool inspectedBool, bool value){
				if(value)
					Assert.That(inspectedBool, Is.True);
				else
					Assert.That(inspectedBool, Is.False);
			}
			public void AssertEquipped(InventoryItemInstanceMock itemInst){
				if(itemInst is BowInstanceMock || itemInst is WearInstanceMock){
					/*	Bow or Wear?	*/
					bool isBow = false;
					if(itemInst is BowInstanceMock)
						isBow = true;
					System.Type typeToCheck;
					InventoryItemInstanceMock sgmEquipped;
					Slottable sbe;
					SlotGroup sge;
					SlotGroup sgp;
					if(isBow){
						typeToCheck = typeof(BowInstanceMock);
						sgmEquipped = sgm.equippedBowInst;
						sge = sgBow;
						sgp = sgpBow;
					}else{
						typeToCheck = typeof(WearInstanceMock);
						sgmEquipped = sgm.equippedWearInst;
						sge = sgWear;
						sgp = sgpWear;
					}
					foreach(Slottable sbp in sgpAll.slottables){
						if(sbp != null){
							InventoryItemInstanceMock sbpItem = sbp.itemInst;
							if(sbpItem.GetType() == typeToCheck)
							{
								if(sbpItem == itemInst){
									AB(sbp.isEquipped, true);
									AB(sgpAll.equippedSBs.Contains(sbp), true);
									AE(sgmEquipped, sbpItem);
									sbe = sge.GetSB(sbpItem);
									AB(sbe != null, true);
									AE(sge.equippedSBs.Count, 1);
									AB(sge.equippedSBs.Contains(sbe), true);
									ASSB(sbe,
										null, SBFocused, null,
										null, SBWFA, null, false, 
										null, SBEquipped, null);
									if(sgpAll == sgm.focusedSGP){
										if(sgpAll.isAutoSort){
											ASSB(sbp,
												null, SBDefocused, null,
												null, SBWFA, null, false,
												null, SBEquipped, null);
										}else{
											ASSB(sbp,
												null, SBFocused, null,
												null, SBWFA, null, false, 
												null, SBEquipped, null);
										}
									}else{
										ASSB(sbp,
											null, SBDefocused, null,
											null, SBWFA, null, false, 
											null, SBEquipped, null);
									}
								}else{// deemed not the equipped bow/wear
									AB(sbp.isEquipped, false);
									AB(sgpAll.equippedSBs.Contains(sbp), false);
									AB(sgmEquipped != sbpItem, true);
									ANull(sge.GetSB(sbpItem));
									if(sgpAll == sgm.focusedSGP){
										ASSB(sbp,
											null, SBFocused, null,
											null, SBWFA, null, false, 
											null, SBUnequipped, null);
									}else{
										ASSB(sbp,
											null, SBDefocused, null,
											null, SBWFA, null, false, 
											null, SBUnequipped,null);
									}
								}
							}
						}
					}
					foreach(Slottable sbp in sgp.slottables){
						if(sbp != null){
							InventoryItemInstanceMock sbpItem = sbp.itemInst;
							if(sbpItem == itemInst){
								AB(sbp.isEquipped, true);
								AB(sgp.equippedSBs.Contains(sbp), true);
								AE(sgmEquipped, sbpItem);
								sbe = sge.GetSB(sbpItem);
								AB(sbe != null, true);
								AE(sge.equippedSBs.Count, 1);
								AB(sge.equippedSBs.Contains(sbe), true);
								if(sgp == sgm.focusedSGP){
									if(sgp.isAutoSort)
										ASSB(sbp,
											null, SBDefocused, null,
											null, SBWFA, null, false,
											null, SBEquipped, null);
									else
										ASSB(sbp,
											null, SBFocused, null,
											null, SBWFA, null, false,
											null, SBEquipped, null);
								}else{
									ASSB(sbp,
										null, SBDefocused, null,
										null, SBWFA, null, false,
										null, SBEquipped, null);
								}
							}else{/* deemed not equipped	*/
								AB(sbp.isEquipped, false);
									AB(sgp.equippedSBs.Contains(sbp), false);
									AB(sgmEquipped != sbpItem, true);
									ANull(sge.GetSB(sbpItem));
								if(sgp == sgm.focusedSGP)
									ASSB(sbp,
										null, SBFocused, null,
										null, SBWFA, null, false,
										null, SBUnequipped, null);
								else
									ASSB(sbp,
										null, SBDefocused, null,
										null, SBWFA, null, false,
										null, SBUnequipped, null);
							}
						}
					}
				}
			}
			public void AECGears(List<CarriedGearInstanceMock> items, PoolInventory poolInv, EquipmentSetInventory equipInv){
				if(items != null){
					AE(sgm.equippedCarriedGears.Count, items.Count);
					AE(sgpAll.equippedSBs.Count, items.Count + 2);
					AE(sgpCGears.equippedSBs.Count, items.Count);
					AE(sgCGears.equippedSBs.Count, items.Count);
					foreach(CarriedGearInstanceMock item in items){
						AB(sgm.equippedCarriedGears.Contains(item), true);
						AB(sgpAll.equippedSBs.Contains(sgpAll.GetSB(item)), true);
						ANull(sgpBow.GetSB(item));
						ANull(sgpWear.GetSB(item));
						ANull(sgpParts.GetSB(item));
						AB(sgpCGears.equippedSBs.Contains(sgpCGears.GetSB(item)), true);
						ANull(sgBow.GetSB(item));
						ANull(sgWear.GetSB(item));
						AB(sgCGears.equippedSBs.Contains(sgCGears.GetSB(item)), true);
					}
				}
				foreach(InventoryItemInstanceMock itemInInv in poolInv.items){
					if(itemInInv is CarriedGearInstanceMock){
						if(items.Contains((CarriedGearInstanceMock)itemInInv)){
							AB(itemInInv.isEquipped, true);
							AB(sgpAll.GetSB(itemInInv).isEquipped, true);
							AB(sgpCGears.GetSB(itemInInv).isEquipped, true);
						}else{//deemed not equipped
							AB(itemInInv.isEquipped, false);
							AB(sgpAll.GetSB(itemInInv).isEquipped, false);
							AB(sgpCGears.GetSB(itemInInv).isEquipped, false);
						}
					}
				}
				foreach(CarriedGearInstanceMock item in items){
					if(item != null){
						AE(equipInv.items.Contains(item), true);
					}
				}
				foreach(InventoryItemInstanceMock itemInInv in equipInv.items){
					if(itemInInv is CarriedGearInstanceMock){
						AB(itemInInv.isEquipped, true);
						AE(items.Contains((CarriedGearInstanceMock)itemInInv), true);
						AB(sgCGears.GetSB(itemInInv).isEquipped, true);
					}
				}
			}
			public void AECGears(Slottable cg1, Slottable cg2, Slottable cg3, Slottable cg4){
				List<CarriedGearInstanceMock> checkedList = new List<CarriedGearInstanceMock>();
				if(cg1 != null)
					checkedList.Add((CarriedGearInstanceMock)cg1.item);
				else
					checkedList.Add(null);
				if(cg2 != null)
					checkedList.Add((CarriedGearInstanceMock)cg2.item);
				else
					checkedList.Add(null);
				if(cg3 != null)
					checkedList.Add((CarriedGearInstanceMock)cg3.item);
				else
					checkedList.Add(null);
				if(cg4 != null)
					checkedList.Add((CarriedGearInstanceMock)cg4.item);
				else
					checkedList.Add(null);
				
				int allowedCount = ((EquipmentSetInventory)sgCGears.inventory).equippableCGearsCount;
				
				for(int i = 0; i < 4; i++){
					if(i +1 > allowedCount)
						if(checkedList[i] != null)
							throw new System.InvalidOperationException("Slottable at index " + i + " is not checked since it exceeds the max slot count");
				}
				for(int i = 0; i < allowedCount; i++){
					Slottable sb = sgCGears.slots[i].sb;
					if(sb != null)
						AE(sb.item, checkedList[i]);
					else
						Assert.That(checkedList[i], Is.Null);
				}
				foreach(SlotGroup sgp in sgm.allSGPs){
					if(sgp.Filter is SGNullFilter || sgp.Filter is SGCGearsFilter){
						foreach(Slottable sbp in sgp.slottables){
							if(sbp != null){
								if(sbp.itemInst is CarriedGearInstanceMock){
									if(checkedList.Contains((CarriedGearInstanceMock)sbp.itemInst)){
										if(sgp == sgm.focusedSGP){
											if(sgp.isAutoSort){
												ASSB(sbp,
													null, SBDefocused, null,
													null, SBWFA, null, false,
													SBUnequipped, SBEquipped, typeof(SBEquipProcess));
											}else{
												ASSB(sbp,
													null, SBFocused, null,
													null, SBWFA, null, false,
													SBUnequipped, SBEquipped, typeof(SBEquipProcess));	
											}
										}else{
											ASSB(sbp,
												null, SBDefocused, null,
												null, SBWFA, null, false,
												SBUnequipped, SBEquipped, typeof(SBEquipProcess));	
										}
									}else{	/* deemed not equipped */
										if(sgp == sgm.focusedSGP){
											ASSB(sbp,
												null, SBFocused, null,
												null, SBWFA, null, false,
												null, SBUnequipped, null);
										}else{
											ASSB(sbp,
												null, SBDefocused, null,
												null, SBWFA, null, false,
												null, SBUnequipped, null);
										}
									}
								}
							}
						}
					}
				}
			}
			public void AssertFocused(){
				AssertEquippedOnAll();
				ASSGM(sgm,
					null, null, null, null, null, null, null, null,
					null, SGMFocused, null,
					null, SGMWFA, null,
					null, true, true, true, true);
				foreach(SlotGroup sgp in sgm.allSGPs){
					ASGReset(sgp);
					if(sgp == sgm.focusedSGP){
						ASSG(sgp,
							null, SGFocused, null,
							null, SGWFA, null, false);
						foreach(Slottable sb in sgp.slottables){
							if(sb != null){
								AE(sb.sgm, sgm);
								ASBReset(sb);
								if(!sgp.isAutoSort){
										ASSB_s(sb, SBFocused, SBWFA);
								}else{
									if(sb.item is PartsInstanceMock && !(sgp.Filter is SGPartsFilter))
										ASSB_s(sb, SBDefocused, SBWFA);
									else
										if(sb.isEquipped)
											ASSB_s(sb, SBDefocused, SBWFA);
										else{
											ASSB_s(sb, SBFocused, SBWFA);
										}
								}
							}
						}
					}else{/*  sgp not focused	*/
						ASSG(sgp,
							null, SGDefocused, null,
							null, SGWFA, null, false);
						foreach(Slottable sb in sgp.slottables){
							if(sb != null){
								AE(sb.sgm, sgm);
								ASBReset(sb);
								ASSB_s(sb, SBDefocused, SBWFA);
							}
						}
					}
				}
				foreach(SlotGroup sge in sgm.allSGEs){
					ASGReset(sge);
					if(sgm.focusedSGEs.Contains(sge)){
						ASSG(sge,
							null, SGFocused, null,
							null, SGWFA, null, false);
						foreach(Slottable sbe in sge.slottables){
							if(sbe != null){
								AE(sbe.sgm, sgm);
								ASBReset(sbe);
								ASSB_s(sbe, SBFocused, SBWFA);
							}
						}
					}else{
						ASSG(sge,
							null, SGFocused, null,
							null, SGWFA, null, false);
						foreach(Slottable sbe in sge.slottables){
							if(sbe != null){
								AE(sbe.sgm, sgm);
								ASBReset(sbe);
								ASSB_s(sbe, SBDefocused, SBWFA);
							}
						}
					}
				}
			}
	/*	shortcut	*/
		/*	Debug	*/
			public void PrintSBsArray(List<Slottable> sbs){
				foreach(Slottable sb in sbs){
					if(sb != null)
						Util.Stack(Util.SBName(sb));
					else
						Util.Stack("null");
				}
				string str = Util.Stacked;
				Debug.Log("SBs: " + str);
			}
			public void PrintItemsArray(List<SlottableItem> items){
				foreach(SlottableItem item in items){
					if(item is InventoryItemInstanceMock){
						InventoryItemInstanceMock itemInst = (InventoryItemInstanceMock)item;
						if(itemInst != null)
							Util.Stack(Util.ItemInstName(itemInst));
						else
							Util.Stack("null");
					}
				}
				string str = Util.Stacked;
				Debug.Log("itemInsts: " + str);
			}
			public void PrintSBs(List<Slottable> sbs){
				foreach(Slottable sb in sbs){
					Debug.Log(Util.SBDebug(sb));
				}
			}
			public void Print(Slottable sb){
				Debug.Log(Util.SBDebug(sb));
			}
			public void Print(SlotGroup sg){
				Debug.Log(Util.SGDebug(sg));
			}
			public void Print(SlotGroupManager sgm){
				Debug.Log(Util.SGMDebug(sgm));
			}
			public string Name(SlotGroup sg){
				return Util.SGName(sg);
			}
			public string Name(Slottable sb){
				return Util.SBName(sb);
			}
			public string Bold(string str){
				return Util.Bold(str);
			}
			public string Red(string str){
				return Util.Red(str);
			}
			public string Blue(string str){
				return Util.Blue(str);
			}
			public string Green(string str){
				return Util.Green(str);
			}
			public string Ciel(string str){
				return Util.Ciel(str);
			}
			public string Aqua(string str){
				return Util.Aqua(str);
			}
			public string Forest(string str){
				return Util.Forest(str);
			}
			public string Brown(string str){
				return Util.Brown(str);
			}
			public string Terra(string str){
				return Util.Terra(str);
			}
			public string Berry(string str){
				return Util.Berry(str);
			}
			public string Violet(string str){
				return Util.Violet(str);
			}
		/*	SGM	*/
			SGMSelectionState SGMDeactivated{
				get{return SlotGroupManager.DeactivatedState;}
			}
			SGMSelectionState SGMDefocused{
				get{return SlotGroupManager.DefocusedState;}
			}
			SGMSelectionState SGMFocused{
				get{return SlotGroupManager.FocusedState;}
			}
			SGMActionState SGMWFA{
				get{return SlotGroupManager.WaitForActionState;}
			}
			SGMActionState SGMProbing{
				get{return SlotGroupManager.ProbingState;}
			}
			SGMActionState SGMTransaction{
				get{return SlotGroupManager.PerformingTransactionState;}
			}
		/*	SG	*/
			SGSelectionState SGFocused{
				get{return SlotGroup.FocusedState;}
			}
			SGSelectionState SGDefocused{
				get{return SlotGroup.DefocusedState;}
			}
			SGSelectionState SGDeactivated{
				get{return SlotGroup.DeactivatedState;}
			}
			SGSelectionState SGSelected{
				get{return SlotGroup.SelectedState;}
			}
			SGActionState SGWFA{
				get{return SlotGroup.WaitForActionState;}
			}
			SGActionState SGRevert{
				get{return SlotGroup.RevertState;}
			}
			SGActionState SGReorder{
				get{return SlotGroup.ReorderState;}
			}
			SGActionState SGFill{
				get{return SlotGroup.FillState;}
			}
			SGActionState SGSwap{
				get{return SlotGroup.SwapState;}
			}
			SGActionState SGAdd{
				get{return SlotGroup.AddState;}
			}
			SGActionState SGRemove{
				get{return SlotGroup.RemoveState;}
			}
		/*	SB states shortcut	*/
			SBSelectionState SBFocused{
				get{return Slottable.FocusedState;}
			}
			SBSelectionState SBDefocused{
				get{return Slottable.DefocusedState;}
			}
			SBSelectionState SBDeactivated{
				get{return Slottable.DeactivatedState;}
			}
			SBSelectionState SBSelected{
				get{return Slottable.SelectedState;}
			}
			SBActionState SBWFA{
				get{return Slottable.WaitForActionState;}
			}
			SBActionState SBWFPickUp{
				get{return Slottable.WaitForPickUpState;}
			}
			SBActionState SBWFPointerUp{
				get{return Slottable.WaitForPointerUpState;}
			}
			SBActionState SBPickedUp{
				get{return Slottable.PickedUpState;}
			}
			SBActionState SBWFNT{
				get{return Slottable.WaitForNextTouchState;}
			}
			SBActionState SBAdd{
				get{return Slottable.AddedState;}
			}
			SBActionState SBRemove{
				get{return Slottable.RemovedState;}
			}
			SBActionState SBMoveWithin{
				get{return Slottable.MoveWithinState;}
			}
			SBEquipState SBEquipped{
				get{return Slottable.EquippedState;}
			}
			SBEquipState SBUnequipped{
				get{return Slottable.UnequippedState;}
			}

}
