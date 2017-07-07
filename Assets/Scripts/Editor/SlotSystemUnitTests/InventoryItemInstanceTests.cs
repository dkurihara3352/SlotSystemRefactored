using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;

[TestFixture]
public class InventoryItemInstanceTests {

	[Test]
	public void Equals_ToSameStackableItem_ReturnsTrue(){
		PartsFake stubParts = MakePartsFake(0);
		PartsInstance partsInstA = MakePartsInstance(stubParts);
		PartsInstance partsInstB = MakePartsInstance(stubParts);
		bool equality = partsInstA == partsInstB;

		Assert.That(equality, Is.True);
	}
	[Test]
	public void objectReferenceEquals_ToSameStackableItem_ReturnsFalse(){
		PartsFake stubParts = MakePartsFake(0);
		PartsInstance partsInstA = MakePartsInstance(stubParts);
		PartsInstance partsInstB = MakePartsInstance(stubParts);
		bool equality = object.ReferenceEquals(partsInstA, partsInstB);

		Assert.That(equality, Is.False);
	}
	[Test]
	public void Equals_ToDifferentStackableItem_ReturnsFalse(){
		PartsFake stubPartsA = MakePartsFake(0);
		PartsFake stubPartsB = MakePartsFake(1);
		PartsInstance partsInstA = MakePartsInstance(stubPartsA);
		PartsInstance partsInstB = MakePartsInstance(stubPartsB);
		bool equality = partsInstA == partsInstB;

		Assert.That(equality, Is.False);
	}
	[Test]
	public void Equals_ToSameNonStackableItem_ReturnsFalse(){
		BowFake stubBow = MakeBowFake(0);
		BowInstance stubBowInstA = MakeBowInstance(stubBow);
		BowInstance stubBowInstB = MakeBowInstance(stubBow);
		bool equality = stubBowInstA == stubBowInstB;

		Assert.That(equality, Is.False);
	}
	[Test]
	public void Equals_ToSelf_ReturnsTrue(){
		BowFake stubBow = MakeBowFake(0);
		BowInstance stubBowInstA = MakeBowInstance(stubBow);
		BowInstance stubBowInstB = stubBowInstA;
		bool equality = stubBowInstA == stubBowInstB;

		Assert.That(equality, Is.True);
	}
	PartsInstance MakePartsInstance(PartsFake parts){
		 PartsInstance result = new PartsInstance();
		 result.Item = parts;
		 return result;
	}
	PartsFake MakePartsFake(int id){
		PartsFake result = new PartsFake();
		result.ItemID = id;
		return result;
	}
	BowInstance MakeBowInstance(BowFake bow){
		BowInstance result = new BowInstance();
		result.Item = bow;
		return result;
	}
	BowFake MakeBowFake(int id){
		BowFake result = new BowFake();
		result.ItemID = id;
		return result;
	}
}
