using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using System;
using System.Collections;
using System.Collections.Generic;
namespace SlotSystemTests{
	namespace ElementsTests{
		[TestFixture]
		[Category("OtherElements")]
		public class SlotSystemBundleTests: SlotSystemTest {
			[TestCaseSource(typeof(SetFocusedBundleElementMemberCases))]
			public void SetFocusedBundleElement_Member_SetsItAsTheFocused(SlotSystemBundle bun, ISlotSystemElement member){
				bun.SetFocusedBundleElement(member);

				Assert.That(bun.focusedElement, Is.SameAs(member));
			}
				class SetFocusedBundleElementMemberCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] case0;
							SlotSystemBundle bun_0 = MakeSSBundle();
								TestSlotSystemElement sseA_0 = MakeTestSSE();									
								TestSlotSystemElement sseB_0 = MakeTestSSE();
								TestSlotSystemElement sseC_0 = MakeTestSSE();
								sseA_0.transform.SetParent(bun_0.transform);
								sseB_0.transform.SetParent(bun_0.transform);
								sseC_0.transform.SetParent(bun_0.transform);
							bun_0.SetHierarchy();
							case0 = new object[]{bun_0, sseB_0};
							yield return case0;
					}
				}
		}
	}
}

