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
	[Test]
	public void CompareTo_IIWithGreaterID_ReturnsNegative(){
		BowFake stubBowA = MakeBowFake(0);
		BowFake stubBowB = MakeBowFake(1);
		BowInstance stubBowInstA = MakeBowInstance(stubBowA);
		BowInstance stubBowInstB = MakeBowInstance(stubBowB);

		int result = stubBowInstA.CompareTo(stubBowInstB);

		Assert.That(result, Is.LessThan(0));
	}
	[Test]
	public void CompareTo_IIWithLesserID_ReturnsPositive(){
		BowFake stubBowA = MakeBowFake(1);
		BowFake stubBowB = MakeBowFake(0);
		BowInstance stubBowInstA = MakeBowInstance(stubBowA);
		BowInstance stubBowInstB = MakeBowInstance(stubBowB);

		int result = stubBowInstA.CompareTo(stubBowInstB);

		Assert.That(result, Is.GreaterThan(0));
	}
	[Test]
	public void CompareTo_IIWithSameIDAndOrder_ReturnsZero(){
		BowFake stubBow = MakeBowFake(0);
		BowInstance stubBowInstA = MakeBowInstance(stubBow, 0);
		BowInstance stubBowInstB = MakeBowInstance(stubBow, 0);

		int result = stubBowInstA.CompareTo(stubBowInstB);

		Assert.That(result, Is.EqualTo(0));
	}
	[Test]
	public void CompareTo_IIWithSameIDAndGreaterOrder_ReturnsNegative(){
		BowFake stubBow = MakeBowFake(0);
		BowInstance stubBowInstA = MakeBowInstance(stubBow, 0);
		BowInstance stubBowInstB = MakeBowInstance(stubBow, 1);

		int result = stubBowInstA.CompareTo(stubBowInstB);

		Assert.That(result, Is.LessThan(0));
	}
	[Test]
	public void CompareTo_IIWithSameIDAndLesserOrder_ReturnsPositive(){
		BowFake stubBow = MakeBowFake(0);
		BowInstance stubBowInstA = MakeBowInstance(stubBow, 1);
		BowInstance stubBowInstB = MakeBowInstance(stubBow, 0);

		int result = stubBowInstA.CompareTo(stubBowInstB);

		Assert.That(result, Is.GreaterThan(0));
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
	BowInstance MakeBowInstance(BowFake bow, int acquisitionOrder){
		BowInstance result = new BowInstance();
		result.Item = bow;
		result.SetAcquisitionOrder(acquisitionOrder);
		return result;
	}
	BowFake MakeBowFake(int id){
		BowFake result = new BowFake();
		result.ItemID = id;
		return result;
	}
}
