using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using Utility;
namespace SlotSystemTests{
	namespace SlotGroupTests{
		[TestFixture]
		public class SGSelStateHandlerTests: SlotSystemTest {
			[Test]
			public void Activate_TACacheResultRevertFalse_SetsIsFocused(){
				SGSelStateHandler selStateHandler;
					ISlotGroup stubSG = MakeSubSG();
						ITransactionCache stubTACache = MakeSubTAC();
						stubTACache.IsCachedTAResultRevert(stubSG).Returns(false);
					stubSG.taCache.Returns(stubTACache);
				selStateHandler = new SGSelStateHandler(stubSG);

				selStateHandler.Activate();

				Assert.That(selStateHandler.isFocused, Is.True);
			}
			[Test]
			public void Activate_TACacheResultRevertTrue_SetsIsDefocused(){
				SGSelStateHandler selStateHandler;
					ISlotGroup stubSG = MakeSubSG();
						ITransactionCache stubTACache = MakeSubTAC();
						stubTACache.IsCachedTAResultRevert(stubSG).Returns(true);
					stubSG.taCache.Returns(stubTACache);
				selStateHandler = new SGSelStateHandler(stubSG);

				selStateHandler.Activate();

				Assert.That(selStateHandler.isDefocused, Is.True);
			}
		}
	}
}
