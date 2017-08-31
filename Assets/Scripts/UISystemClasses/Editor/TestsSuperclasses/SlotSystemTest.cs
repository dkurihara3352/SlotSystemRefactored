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
	/*	Elements	*/
		protected static ISlotSystemManager MakeSubSSM(){
			return Substitute.For<ISlotSystemManager>();
		}
		protected static IEquipmentSet MakeSubESet(){
			return Substitute.For<IEquipmentSet>();
		}
		protected static IUIBundle MakeSubBundle(){
			return Substitute.For<IUIBundle>();
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
		protected static ISlottable MakeSubSB(){
			return Substitute.For<ISlottable>();
		}
		protected static IUIElement MakeSubUIE(){
			return Substitute.For<IUIElement>();
		}
	/*	Non elements	*/
		protected static IInventory MakeSubInv(){
			return Substitute.For<IInventory>();
		}
		protected static IEquippedItemsInventory MakeSubEquipInv(){
			return Substitute.For<IEquippedItemsInventory>();
		}
		protected static IPoolInventory MakeSubPoolInv(){
			return Substitute.For<IPoolInventory>();
		}
	/* Items */
		const int bowIDBase = 0;
		const int wearIDBase = 1000;
		const int shieldIDBase = 2000;
		const int mWeaponIDBase = 3000;
		const int quiverIDBase = 4000;
		const int packIDBase = 5000;
		const int partsIDBase = 6000;
		protected static BowInstance MakeBowInstance(int id){
			BowFake bowFake = new BowFake();
			bowFake.SetItemID(bowIDBase + id);
			BowInstance bowInst = new BowInstance(bowFake);
			return bowInst;
		}
		protected static WearInstance MakeWearInstance(int id){
			WearFake wearFake = new WearFake();
			wearFake.SetItemID(wearIDBase + id);
			WearInstance wearInst = new WearInstance(wearFake);
			return wearInst;
		}
		protected static ShieldInstance MakeShieldInstance(int id){
			ShieldFake shieldFake = new ShieldFake();
			shieldFake.SetItemID(shieldIDBase + id);
			ShieldInstance shieldInst = new ShieldInstance(shieldFake);
			return shieldInst;
		}
		protected static MeleeWeaponInstance MakeMWeaponInstance(int id){
			MeleeWeaponFake mWFake = new MeleeWeaponFake();
			mWFake.SetItemID(mWeaponIDBase + id);
			MeleeWeaponInstance mWInst = new MeleeWeaponInstance(mWFake);
			return mWInst;
		}
		protected static QuiverInstance MakeQuiverInstance(int id){
			QuiverFake quiverFake = new QuiverFake();
			quiverFake.SetItemID(quiverIDBase + id);
			QuiverInstance quiverInst = new QuiverInstance(quiverFake);
			return quiverInst;
		}
		protected static PackInstance MakePackInstance(int id){
			PackFake packFake = new PackFake();
			packFake.SetItemID(packIDBase + id);
			PackInstance packInst = new PackInstance(packFake);
			return packInst;
		}
		protected static PartsInstance MakePartsInstance(int id, int quantity){
			PartsFake partsFake = new PartsFake();
			partsFake.SetItemID(partsIDBase + id);
			PartsInstance partsInst = new PartsInstance(partsFake, quantity);
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
}
