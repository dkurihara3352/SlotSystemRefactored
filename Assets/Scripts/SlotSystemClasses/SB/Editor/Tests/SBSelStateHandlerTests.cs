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
	namespace SlottableTests{
		[TestFixture]
		public class SBSelStateHandlerTests: SlotSystemTest {
			[Test]
			public void Activate_TACacheIsTAResultRevertForFalse_SetIsFocused(){
				SBSelStateHandler selStateHandler;
					ISlottable sb = MakeSubSB();
					ITransactionCache taCache = Substitute.For<ITransactionCache>();
						taCache.IsCachedTAResultRevert(sb).Returns(false);
					sb.GetTAC().Returns(taCache);
					selStateHandler = new SBSelStateHandler(sb);
				
				selStateHandler.Activate();

				Assert.That(selStateHandler.IsFocused(), Is.True);
			}
			[Test]
			public void Activate_TACacheIsTAResultRevertForTrue_SetIsDefocused(){
				SBSelStateHandler selStateHandler;
					ISlottable sb = MakeSubSB();
					ITransactionCache taCache = Substitute.For<ITransactionCache>();
						taCache.IsCachedTAResultRevert(sb).Returns(true);
					sb.GetTAC().Returns(taCache);
					selStateHandler = new SBSelStateHandler(sb);
				
				selStateHandler.Activate();

				Assert.That(selStateHandler.IsDefocused(), Is.True);
			}
		}
	}
}
