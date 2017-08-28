using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UISystem;
using Utility;
namespace SlotSystemTests{
	namespace SlotGroupTests{
		[TestFixture]
		public class SGSelStateHandlerTests: SlotSystemTest {
			[Test]
			public void Activate_TACacheResultRevertFalse_SetsIsFocused(){
				SGSelStateHandler selStateHandler;
						ITransactionCache stubTACache = MakeSubTAC();
						IHoverable hoverable = Substitute.For<IHoverable>();
						stubTACache.IsCachedTAResultRevert(hoverable).Returns(false);
				selStateHandler = new SGSelStateHandler(stubTACache, hoverable);

				selStateHandler.Activate();

				Assert.That(selStateHandler.IsSelectable(), Is.True);
			}
			[Test]
			public void Activate_TACacheResultRevertTrue_SetsIsDefocused(){
				SGSelStateHandler selStateHandler;
						ITransactionCache stubTACache = MakeSubTAC();
						IHoverable hoverable = Substitute.For<IHoverable>();
						stubTACache.IsCachedTAResultRevert(hoverable).Returns(true);
				selStateHandler = new SGSelStateHandler(stubTACache, hoverable);

				selStateHandler.Activate();

				Assert.That(selStateHandler.IsUnselectable(), Is.True);
			}
		}
	}
}
