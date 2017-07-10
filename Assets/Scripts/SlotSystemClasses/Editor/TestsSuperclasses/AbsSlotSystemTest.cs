using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using NSubstitute;

public class AbsSlotSystemTest{
	protected TestSSM MakeFakeSSM(){
		return new GameObject("ssm").AddComponent<TestSSM>();
	}
	protected FakeSB MakeFakeSB(){
		GameObject fakeSBGO = new GameObject("fakeSBGO");
		FakeSB fakeSB = fakeSBGO.AddComponent<FakeSB>();
		return fakeSB;
	}
	protected FakeSG MakeFakeSG(){
		return new GameObject("fakeSGGO").AddComponent<FakeSG>();
	}
	protected class FakeSBSelState: SBSelState{}
	protected static SBSelState MakeSubSBSelState(){
		return Substitute.For<SBSelState>();
	}
	protected static SBActState MakeSubSBActState(){
		return Substitute.For<SBActState>();
	}
	protected static SBEqpState MakeSubSBEqpState(){
		return Substitute.For<SBEqpState>();
	}
	protected static SBMrkState MakeSubSBMrkState(){
		return Substitute.For<SBMrkState>();
	}
	protected static SGSelState MakeSubSGSelState(){
		return Substitute.For<SGSelState>();
	}
	protected static SGActState MakeSubSGActState(){
		return Substitute.For<SGActState>();
	}
	protected static SSMSelState MakeSubSSMSelState(){
		return Substitute.For<SSMSelState>();
	}
	protected static SSMActState MakeSubSSMActState(){
		return Substitute.For<SSMActState>();
	}
	protected static SBSelProcess MakeSubSBSelProc(){
		return Substitute.For<SBSelProcess>();
	}
	protected static SBActProcess MakeSubSBActProc(){
		return Substitute.For<SBActProcess>();
	}
	protected static SBEqpProcess MakeSubSBEqpProc(){
		return Substitute.For<SBEqpProcess>();
	}
	protected static SBMrkProcess MakeSubSBMrkProc(){
		return Substitute.For<SBMrkProcess>();
	}
	protected static SGSelProcess MakeSubSGSelProc(){
		return Substitute.For<SGSelProcess>();
	}
	protected static SGActProcess MakeSubSGActProc(){
		return Substitute.For<SGActProcess>();
	}
	protected static SSMSelProcess MakeSubSSMSelProc(){
		return Substitute.For<SSMSelProcess>();
	}
	protected static SSMActProcess MakeSubSSMActProc(){
		return Substitute.For<SSMActProcess>();
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
	protected IEnumeratorFake FakeCoroutine(){
		return new IEnumeratorFake();
	}
}
