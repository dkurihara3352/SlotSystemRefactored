using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using NSubstitute;

public class SlotSystemTest{
	[TearDown]
	public void CleanupScene(){
		Object[] gos = GameObject.FindGameObjectsWithTag("TestGO");
		foreach(var obj in gos)
			GameObject.DestroyImmediate(obj);
	}
	protected static bool BothNullOrReferenceEquals(object a, object b){
		if(a == null) return b ==null;
		else
			return object.ReferenceEquals(a, b);
	}
	/*	Elements	*/
		protected static TransactionManager MakeTAM(){
			GameObject go = new GameObject("tamGo");
			go.tag = "TestGO";
			return go.AddComponent<TransactionManager>();
		}
		protected static SlotSystemManager MakeSSM(){
			GameObject go = new GameObject("go");
			go.tag = "TestGO";
			SlotSystemManager ssm = go.AddComponent<SlotSystemManager>();
			return ssm;
		}
		protected static ISlotSystemManager MakeSubSSM(){
			return Substitute.For<ISlotSystemManager>();
		}
		protected static EquipmentSet MakeEquipmentSet(){
			GameObject go = new GameObject("eSetGO");
			go.tag = "TestGO";
			EquipmentSet eSet = go.AddComponent<EquipmentSet>();
			return eSet;
		}
		protected static IEquipmentSet MakeSubESet(){
			return Substitute.For<IEquipmentSet>();
		}
		protected static IEquipmentSet MakeSubEquipmentSetInitWithSGs(){
			IEquipmentSet eSet = Substitute.For<IEquipmentSet>();
			ISlotGroup bowSG = MakeSubSGWithEmptySBs();
			ISlotGroup wearSG = MakeSubSGWithEmptySBs();
			ISlotGroup cGearsSG = MakeSubSGWithEmptySBs();
			IEnumerable<ISlotSystemElement> eles = new ISlotSystemElement[]{
				bowSG, wearSG, cGearsSG
			};
			eSet.GetEnumerator().Returns(eles.GetEnumerator());
			eSet.GetEnumerator().Returns(eles.GetEnumerator());
			return eSet;
		}
		protected static ISlotSystemBundle MakeSubBundle(){
			return Substitute.For<ISlotSystemBundle>();
		}
		protected static SlotSystemBundle MakeSSBundle(){
			GameObject go = new GameObject("ssBunGO");
			go.tag = "TestGO";
			SlotSystemBundle ssBun = go.AddComponent<SlotSystemBundle>();
			return ssBun;
		}
		protected static ISlotGroup MakeSubSG(){
			return Substitute.For<ISlotGroup>();
		}
		protected static ISlotGroup MakeSubSGWithEmptySBs(){
			ISlotGroup sg = MakeSubSG();
			List<ISlotSystemElement> eles = new List<ISlotSystemElement>();
			sg.GetEnumerator().Returns(eles.GetEnumerator());
			return sg;
		}
		protected static SlotGroup MakeSG(){
			GameObject go = new GameObject("go");
			go.tag = "TestGO";
			return go.AddComponent<SlotGroup>();
		}
		protected static SlotGroup MakeSGWithEmptySBs(){
			GameObject go = new GameObject("go");
			go.tag = "TestGO";
			SlotGroup sg = go.AddComponent<SlotGroup>();
			sg.SetSBs(new List<ISlottable>());
			return sg;
		}
		protected static Slottable MakeSB(){
			GameObject sbGO = new GameObject("sbGO");
			sbGO.tag = "TestGO";
			Slottable sb = sbGO.AddComponent<Slottable>();
			return sb;
		}
		protected static ISlottable MakeSubSB(){
			return Substitute.For<ISlottable>();
		}
		protected static TestSlotSystemElement MakeTestSSE(){
			GameObject sseGO = new GameObject("sseGO");
			return sseGO.AddComponent<TestSlotSystemElement>();
		}
		protected static ISlotSystemElement MakeSubSSE(){
			return Substitute.For<ISlotSystemElement>();
		}
	/*	Non elements	*/
		protected static ITransactionManager MakeSubTAM(){
			return Substitute.For<ITransactionManager>();
		}
		protected static ITransactionCache MakeSubTAC(){
			return Substitute.For<ITransactionCache>();
		}
		protected static ITransactionIconHandler MakeSubIconHandler(){
			return Substitute.For<ITransactionIconHandler>();
		}
		protected static ITransactionSGHandler MakeSubSGHandler(){
			return Substitute.For<ITransactionSGHandler>();
		}
		protected static ITAMActStateHandler MakeSubTAMStateHandler(){
			return Substitute.For<ITAMActStateHandler>();
		}
		protected static IFocusedSGProvider MakeSubFocSGPrv(){
			return Substitute.For<IFocusedSGProvider>();
		}
		protected static List<Slot> CreateSlots(int count){
			List<Slot> slots = new List<Slot>();
			for(int i = 0; i< count; i++)
				slots.Add(new Slot());
			return slots;
		}
		protected static IEquipmentSetInventory MakeSubEquipInv(){
			return Substitute.For<IEquipmentSetInventory>();
		}
		protected static IPoolInventory MakeSubPoolInv(){
			return Substitute.For<IPoolInventory>();
		}
		/* States */
			protected static SBActState MakeSubSBActState(){
				return Substitute.For<SBActState>();
			}
			protected static SBEqpState MakeSubSBEqpState(){
				return Substitute.For<SBEqpState>();
			}
			protected static SBMrkState MakeSubSBMrkState(){
				return Substitute.For<SBMrkState>();
			}
			protected static SGActState MakeSubSGActState(){
				return Substitute.For<SGActState>();
			}
			protected static TAMActState MakeSubSSMActState(){
				return Substitute.For<TAMActState>();
			}
		/* Process */
			protected static ISBSelProcess MakeSubSBSelProc(){
				return Substitute.For<ISBSelProcess>();
			}
			protected static ISBActProcess MakeSubSBActProc(){
				return Substitute.For<ISBActProcess>();
			}
			protected static ISBEqpProcess MakeSubSBEqpProc(){
				return Substitute.For<ISBEqpProcess>();
			}
			protected static ISBMrkProcess MakeSubSBMrkProc(){
				return Substitute.For<ISBMrkProcess>();
			}
			protected static ISGSelProcess MakeSubSGSelProc(){
				return Substitute.For<ISGSelProcess>();
			}
			protected static ISGActProcess MakeSubSGActProc(){
				return Substitute.For<ISGActProcess>();
			}
			protected static ITAMActProcess MakeSubTAMActProc(){
				return Substitute.For<ITAMActProcess>();
			}
		/* ItemInstance */
		protected static BowInstance MakeBowInstance(int id){
			BowFake bowFake = new BowFake();
			bowFake.ItemID = id;
			BowInstance bowInst = new BowInstance();
			bowInst.Item = bowFake;
			return bowInst;
		}
		protected static WearInstance MakeWearInstance(int id){
			WearFake wearFake = new WearFake();
			wearFake.ItemID = 1000 + id;
			WearInstance wearInst = new WearInstance();
			wearInst.Item = wearFake;
			return wearInst;
		}
		protected static ShieldInstance MakeShieldInstance(int id){
			ShieldFake shieldFake = new ShieldFake();
			shieldFake.ItemID = 2000 + id;
			ShieldInstance shieldInst = new ShieldInstance();
			shieldInst.Item = shieldFake;
			return shieldInst;
		}
		protected static MeleeWeaponInstance MakeMeleeWeaponInstance(int id){
			MeleeWeaponFake mWFake = new MeleeWeaponFake();
			mWFake.ItemID = 3000 + id;
			MeleeWeaponInstance mWInst = new MeleeWeaponInstance();
			mWInst.Item = mWFake;
			return mWInst;
		}
		protected static QuiverInstance MakeQuiverInstance(int id){
			QuiverFake quiverFake = new QuiverFake();
			quiverFake.ItemID = 4000 + id;
			QuiverInstance quiverInst = new QuiverInstance();
			quiverInst.Item = quiverFake;
			return quiverInst;
		}
		protected static PackInstance MakePackInstance(int id){
			PackFake packFake = new PackFake();
			packFake.ItemID = 5000 + id;
			PackInstance packInst = new PackInstance();
			packInst.Item = packFake;
			return packInst;
		}
		protected static PartsInstance MakePartsInstance(int id, int quantity){
			PartsFake partsFake = new PartsFake();
			partsFake.ItemID = 6000 + id;
			PartsInstance partsInst = new PartsInstance();
			partsInst.Item = partsFake;
			partsInst.quantity = quantity;
			return partsInst;
		}
		protected static BowInstance MakeBowInstWithOrder(int id, int order){
			BowInstance bow = MakeBowInstance(id);
			bow.SetAcquisitionOrder(order);
			return bow;
		}
		protected static WearInstance MakeWearInstWithOrder(int id, int order){
			WearInstance wear = MakeWearInstance(id);
			wear.SetAcquisitionOrder(order);
			return wear;
		}
		protected static ShieldInstance MakeShieldInstWithOrder(int id, int order){
			ShieldInstance shield = MakeShieldInstance(id);
			shield.SetAcquisitionOrder(order);
			return shield;
		}
		protected static MeleeWeaponInstance MakeMeleeWeaponInstWithOrder(int id, int order){
			MeleeWeaponInstance mWeapon = MakeMeleeWeaponInstance(id);
			mWeapon.SetAcquisitionOrder(order);
			return mWeapon;
		}
		protected static QuiverInstance MakeQuiverInstWithOrder(int id, int order){
			QuiverInstance quvier = MakeQuiverInstance(id);
			quvier.SetAcquisitionOrder(order);
			return quvier;
		}
		protected static PackInstance MakePackInstWithOrder(int id, int order){
			PackInstance pack = MakePackInstance(id);
			pack.SetAcquisitionOrder(order);
			return pack;
		}
		protected static PartsInstance MakePartsInstWithOrder(int id, int qua, int order){
			PartsInstance parts = MakePartsInstance(id, qua);
			parts.SetAcquisitionOrder(order);
			return parts;
		}
		protected static IEnumeratorFake FakeCoroutine(){
			return new IEnumeratorFake();
		}
		protected static ISlotSystemTransaction MakeSubTA(){
			return Substitute.For<ISlotSystemTransaction>();
		}
		protected static ITransactionFactory MakeSubTAFactory(){
			return Substitute.For<ITransactionFactory>();
		}
		protected static IRevertTransaction MakeTestRevertTA(){
			return new TestRevertTransaction();
		}
		protected static IReorderTransaction MakeTestReorderTA(){
			return new TestReorderTransaction();
		}
		protected static ISortTransaction MakeTestSortTA(){
			return new TestSortTransaction();
		}
		protected static IFillTransaction MakeTestFillTA(){
			return new TestFillTransaction();
		}
		protected static ISwapTransaction MakeTestSwapTA(){
			return new TestSwapTransaction();
		}
		protected static IStackTransaction MakeTestStackTA(){
			return new TestStackTransaction();
		}
}
