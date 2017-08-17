using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
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
			TestSlotSystemElement sse = MakeTestSSE();
				SlotSystemBundle bun0 = MakeSSBundle();
					TestSlotSystemElement sse00 = MakeTestSSE();// !isAOD, focusedEle
					TestSlotSystemElement sse01 = MakeTestSSE();
					TestSlotSystemElement sse02 = MakeTestSSE();
				SlotSystemBundle bun1 = MakeSSBundle();

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
