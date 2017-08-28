using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UISystem;
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
				ItemHandler itemHandler;
					BowInstance bow = MakeBowInstance(0);
					itemHandler = new ItemHandler(bow);

				Assert.That(itemHandler.Item(), Is.SameAs(bow));
				Assert.That(itemHandler.PickedAmount(), Is.EqualTo(0));
				Assert.That(itemHandler.IsStackable(), Is.False);
				Assert.That(itemHandler.Quantity(), Is.EqualTo(1));
			}
			[Test]
			public void IncreasePickedAmount_itemIsStackable_IncreasesPickedAmountUptoQuantity([NUnit.Framework.Random(1, 10, 1)]int quantity){
				IInventoryItemInstance item = Substitute.For<IInventoryItemInstance>();
					item.IsStackable().Returns(true);
					item.GetQuantity().Returns(quantity);
				ItemHandler itemHandler = new ItemHandler(item);

				for(int i = 0; i < quantity + 1; i ++){
					itemHandler.IncreasePickedAmount();
				}

				Assert.That(itemHandler.Quantity(), Is.EqualTo(quantity));
			}
		}
	}
}

