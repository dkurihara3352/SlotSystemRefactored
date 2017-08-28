using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using UISystem;
using Utility;
using System;
using System.Collections;
using System.Collections.Generic;
namespace SlotSystemTests{
	[TestFixture]
	[Category("Integration")]
	public class SlotSystemIntegrationTest: SlotSystemTest {
		[Test]
		public void SlotSystemBundle_initiallyFocusedElement_NotIsActivatedOnDefault_SetsIsAODTrue(){
			TestUIElement sse = MakeTestSSE();
				UIBundle bun0 = MakeSSBundle();
					TestUIElement sse00 = MakeTestSSE();// !isAOD, focusedEle
					TestUIElement sse01 = MakeTestSSE();
					TestUIElement sse02 = MakeTestSSE();
				UIBundle bun1 = MakeSSBundle();

				bun0.transform.SetParent(sse.transform);
					sse00.transform.SetParent(bun0.transform);
					sse01.transform.SetParent(bun0.transform);
					sse02.transform.SetParent(bun0.transform);
				bun1.transform.SetParent(sse.transform);

				bun0.InspectorSetUp(sse00);

				sse00.SetIsActivatedOnDefault(false);

				sse.SetHierarchyRecursively();
				
				Assert.That(sse00.IsActivatedOnDefault(), Is.Not.False);
		}
	}
}
