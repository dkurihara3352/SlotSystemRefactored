using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;

public class AbsSlotSystemTest{
	protected TestSSM MakeFakeSSM(){
		return new GameObject("ssm").AddComponent<TestSSM>();
	}
	protected FakeSB MakeFakeSB(){
		GameObject fakeSBGO = new GameObject("fakeSBGO");
		FakeSB fakeSB = fakeSBGO.AddComponent<FakeSB>();
		return fakeSB;
	}
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
		partsInst.Quantity = quantity;
		return partsInst;
	}
}
