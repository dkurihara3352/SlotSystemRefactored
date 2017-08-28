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
	namespace SlottableTests{
		[TestFixture]
		public class SBSelStateHandlerTests: SlotSystemTest {
			[Test]
			public void Activate_IsTAResultRevertForFalse_Sets_IsFocused(){
				SBSelStateHandler selStateHandler;
					ISlottable sb = MakeSubSB();
						IHoverable hoverable = Substitute.For<IHoverable>();
						sb.GetHoverable().Returns(hoverable);
					ITransactionCache taCache = Substitute.For<ITransactionCache>();
						taCache.IsCachedTAResultRevert(hoverable).Returns(false);
					sb.GetTAC().Returns(taCache);
					selStateHandler = new SBSelStateHandler(sb);
				
				selStateHandler.Activate();

				Assert.That(selStateHandler.IsSelectable(), Is.True);
			}
			[Test]
			public void Activate_IsTAResultRevertForTrue_Sets_IsDefocused(){
				SBSelStateHandler selStateHandler;
					ISlottable sb = MakeSubSB();
						IHoverable hoverable = Substitute.For<IHoverable>();
						sb.GetHoverable().Returns(hoverable);
					ITransactionCache taCache = Substitute.For<ITransactionCache>();
						taCache.IsCachedTAResultRevert(hoverable).Returns(true);
					sb.GetTAC().Returns(taCache);
					selStateHandler = new SBSelStateHandler(sb);
				
				selStateHandler.Activate();

				Assert.That(selStateHandler.IsUnselectable(), Is.True);
			}
		}
	}
}
