using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using NSubstitute;

public class AbsSlotSystemTest{
	protected static ISlotSystemManager MakeSubSSM(){
		return Substitute.For<ISlotSystemManager>();
	}
	protected static ISlotGroup MakeSubSG(){
		return Substitute.For<ISlotGroup>();
	}
	protected static ISlottable MakeSubSB(){
		return Substitute.For<ISlottable>();
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
	protected static ISSMSelProcess MakeSubSSMSelProc(){
		return Substitute.For<ISSMSelProcess>();
	}
	protected static ISSMActProcess MakeSubSSMActProc(){
		return Substitute.For<ISSMActProcess>();
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
	protected static IEnumeratorFake FakeCoroutine(){
		return new IEnumeratorFake();
	}
}
