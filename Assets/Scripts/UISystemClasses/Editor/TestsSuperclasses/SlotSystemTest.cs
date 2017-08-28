using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UISystem;
using NSubstitute;

public class SlotSystemTest{
	[TearDown]
	public void CleanupScene(){
		Object[] gos = GameObject.FindGameObjectsWithTag("TestGO");
		foreach(var obj in gos)
			GameObject.DestroyImmediate(obj);
	}
	public void AssertCreatedSB(ISlottable sb, IInventoryItemInstance addedItem, ISlotSystemManager ssm){
		Assert.That(sb.GetItem(), Is.SameAs(addedItem));
		Assert.That(sb.GetSSM(), Is.SameAs(ssm));
		IUISelStateHandler sbSelStateHandler = sb.UISelStateHandler();
		Assert.That(sbSelStateHandler.IsUnselectable(), Is.True);
		Assert.That(sb, Is.Not.Null.And.InstanceOf(typeof(Slottable)));
	}
	/*	Elements	*/
		protected static SlotGroup MakeSG_FilterHandler_RSBHandler(){
			SlotGroup sg = MakeSG();
			sg.SetSBHandler(new SBHandler());
			sg.SetFilterHandler(new FilterHandler());
			return sg;
		}
		protected static SlotGroup MakeSG_ISBHandler(List<ISlottable> sbs){
			SlotGroup sg = MakeSG();
				ISBHandler sbHandler = Substitute.For<ISBHandler>();
				sbHandler.GetSBs().Returns(sbs);
			sg.SetSBHandler(sbHandler);

			return sg;
		}
		protected static TransactionManager MakeTAM(){
			return new TransactionManager();
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
			IEnumerable<IUIElement> eles = new IUIElement[]{
				bowSG, wearSG, cGearsSG
			};
			eSet.GetEnumerator().Returns(eles.GetEnumerator());
			eSet.GetEnumerator().Returns(eles.GetEnumerator());
			return eSet;
		}
		protected static IUIBundle MakeSubBundle(){
			return Substitute.For<IUIBundle>();
		}
		protected static UIBundle MakeSSBundle(){
			GameObject go = new GameObject("ssBunGO");
			go.tag = "TestGO";
			UIBundle ssBun = go.AddComponent<UIBundle>();
			return ssBun;
		}
		protected static ISlotGroup MakeSubSG(){
			return Substitute.For<ISlotGroup>();
		}
		protected static ISlotGroup MakeSubSGWithEmptySBs(){
			ISlotGroup sg = MakeSubSG();
			List<IUIElement> eles = new List<IUIElement>();
			sg.GetEnumerator().Returns(eles.GetEnumerator());
			return sg;
		}
		protected static SlotGroup MakeSG(){
			GameObject go = new GameObject("go");
			go.tag = "TestGO";
			return go.AddComponent<SlotGroup>();
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
		protected static TestUIElement MakeTestSSE(){
			GameObject sseGO = new GameObject("sseGO");
			return sseGO.AddComponent<TestUIElement>();
		}
		protected static IUIElement MakeSubSSE(){
			return Substitute.For<IUIElement>();
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
		protected static List<Slot> CreateSlots(int count){
			List<Slot> slots = new List<Slot>();
			for(int i = 0; i< count; i++)
				slots.Add(new Slot());
			return slots;
		}
		protected static IEquippedItemsInventory MakeSubEquipInv(){
			return Substitute.For<IEquippedItemsInventory>();
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
			protected static SGActState MakeSubSGActState(){
				return Substitute.For<SGActState>();
			}
			protected static TAMActState MakeSubSSMActState(){
				return Substitute.For<TAMActState>();
			}
		/* Process */
			protected static ISBActProcess MakeSubSBActProc(){
				return Substitute.For<ISBActProcess>();
			}
			protected static ISBEqpProcess MakeSubSBEqpProc(){
				return Substitute.For<ISBEqpProcess>();
			}
			protected static ISBMrkProcess MakeSubSBMrkProc(){
				return Substitute.For<ISBMrkProcess>();
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
			bowFake.SetItemID(id);
			BowInstance bowInst = new BowInstance();
			bowInst.SetInventoryItem(bowFake);
			return bowInst;
		}
		protected static WearInstance MakeWearInstance(int id){
			WearFake wearFake = new WearFake();
			wearFake.SetItemID(1000 + id);
			WearInstance wearInst = new WearInstance();
			wearInst.SetInventoryItem(wearFake);
			return wearInst;
		}
		protected static ShieldInstance MakeShieldInstance(int id){
			ShieldFake shieldFake = new ShieldFake();
			shieldFake.SetItemID(2000 + id);
			ShieldInstance shieldInst = new ShieldInstance();
			shieldInst.SetInventoryItem(shieldFake);
			return shieldInst;
		}
		protected static MeleeWeaponInstance MakeMWeaponInstance(int id){
			MeleeWeaponFake mWFake = new MeleeWeaponFake();
			mWFake.SetItemID(3000 + id);
			MeleeWeaponInstance mWInst = new MeleeWeaponInstance();
			mWInst.SetInventoryItem(mWFake);
			return mWInst;
		}
		protected static QuiverInstance MakeQuiverInstance(int id){
			QuiverFake quiverFake = new QuiverFake();
			quiverFake.SetItemID(4000 + id);
			QuiverInstance quiverInst = new QuiverInstance();
			quiverInst.SetInventoryItem(quiverFake);
			return quiverInst;
		}
		protected static PackInstance MakePackInstance(int id){
			PackFake packFake = new PackFake();
			packFake.SetItemID(5000 + id);
			PackInstance packInst = new PackInstance();
			packInst.SetInventoryItem(packFake);
			return packInst;
		}
		protected static PartsInstance MakePartsInstance(int id, int quantity){
			PartsFake partsFake = new PartsFake();
			partsFake.SetItemID(6000 + id);
			PartsInstance partsInst = new PartsInstance();
			partsInst.SetInventoryItem(partsFake);
			partsInst.SetQuantity(quantity);
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
			MeleeWeaponInstance mWeapon = MakeMWeaponInstance(id);
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
