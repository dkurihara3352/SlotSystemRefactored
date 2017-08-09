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
				ItemHandler itemHandler;
					BowInstance bow = MakeBowInstance(0);
					itemHandler = new ItemHandler(bow);

				Assert.That(itemHandler.item, Is.SameAs(bow));
				Assert.That(itemHandler.pickedAmount, Is.EqualTo(0));
				Assert.That(itemHandler.isStackable, Is.False);
				Assert.That(itemHandler.quantity, Is.EqualTo(1));
			}
		}
	}
}

