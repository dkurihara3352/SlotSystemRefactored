using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections;
using System.Collections.Generic;
using System;
using Utility;
using NSubstitute;
namespace SlotSystemTests{
	namespace SlottableTests{
		[TestFixture]
		public class ItemHandlerTests: SlotSystemTest {
			[Test]
			public void Fields_ByDefault_AreSetToDefault(){
				ItemHandler itemHandler = new ItemHandler();
					BowInstance bow = MakeBowInstance(0);
					itemHandler.SetItem(bow);

				Assert.That(itemHandler.item, Is.SameAs(bow));
				Assert.That(itemHandler.pickedAmount, Is.EqualTo(0));
				Assert.That(itemHandler.isStackable, Is.False);
				Assert.That(itemHandler.quantity, Is.EqualTo(1));
			}
			[Test]
			public void Increment_isStackable_IncreasePickedAmountUpToQuantity(){
				ItemHandler itemHandler = new ItemHandler();
					PartsInstance parts = MakePartsInstance(0, 2);
					itemHandler.SetItem(parts);
				
				itemHandler.Increment();
					Assert.That(itemHandler.pickedAmount, Is.EqualTo(1));
				
				itemHandler.Increment();
					Assert.That(itemHandler.pickedAmount, Is.EqualTo(2));
				
				itemHandler.Increment();
					Assert.That(itemHandler.pickedAmount, Is.EqualTo(2));
			}
			[Test]
			public void Increment_isNotStackable_DoesNotIncrease(){
				ItemHandler itemHandler = new ItemHandler();
					BowInstance bow = MakeBowInstance(0);
					itemHandler.SetItem(bow);
				
				itemHandler.Increment();
					Assert.That(itemHandler.pickedAmount, Is.EqualTo(0));
				
				itemHandler.Increment();
					Assert.That(itemHandler.pickedAmount, Is.EqualTo(0));
				
				itemHandler.Increment();
					Assert.That(itemHandler.pickedAmount, Is.EqualTo(0));
			}
		}
	}
}

