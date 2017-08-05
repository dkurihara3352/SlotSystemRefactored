using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using Utility;
namespace SlotSystemTests{
	namespace SSEElementsTests{
		[TestFixture]
		[Category("OtherElements")]
		public class SlotSystemElementTests: SlotSystemTest{
			/*	Fields	*/
				[TestCase(1)]
				[TestCase(10)]
				[TestCase(100)]
				public void indexer_Valid_ReturnsEnumeratorElement(int count){
					TestSlotSystemElement sse = MakeTestSSE();
						IEnumerable<ISlotSystemElement> elements = CreateSSEs(count);
						List<ISlotSystemElement> list = new List<ISlotSystemElement>(elements);
						sse.SetElements(elements);
					
					for(int i = 0; i< count; i++)
						Assert.That(sse[i], Is.Not.SameAs(list[i]));
					}
				[TestCase(1)]
				[TestCase(10)]
				[TestCase(100)]
				[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
				public void indexer_Invalid_ThrowsException(int count){
					TestSlotSystemElement sse = MakeTestSSE();
						IEnumerable<ISlotSystemElement> elements = CreateSSEs(count);
						sse.SetElements(elements);
					
					ISlotSystemElement invalid = sse[count + 1];
					invalid = null;
					}
				public IEnumerable<ISlotSystemElement> CreateSSEs(int count){
					for(int i = 0; i < count; i++)
						yield return Substitute.For<ISlotSystemElement>();
					}
				
				[Test]
				public void immediateBundle_Various_ReturnsAccordingly(){
					TestSlotSystemElement sse_0 = MakeTestSSE();						
					TestSlotSystemElement sse_1 = MakeTestSSE();
					TestSlotSystemElement sse_2 = MakeTestSSE();
					SlotSystemBundle bundleA = MakeSSBundle();
					TestSlotSystemElement sse_3 = MakeTestSSE();
					SlotSystemBundle bundleB = MakeSSBundle();
					TestSlotSystemElement sse_4 = MakeTestSSE();
					TestSlotSystemElement sse_5 = MakeTestSSE();
						sse_0.SetParent(null);
						sse_1.SetParent(sse_0);
						sse_2.SetParent(sse_1);
						bundleA.SetParent(sse_2);
						sse_3.SetParent(bundleA);
						bundleB.SetParent(sse_3);
						sse_4.SetParent(bundleB);
						sse_5.SetParent(sse_4);
					
					Assert.That(sse_0.immediateBundle, Is.Null);
					Assert.That(sse_1.immediateBundle, Is.Null);
					Assert.That(sse_2.immediateBundle, Is.Null);
					Assert.That(bundleA.immediateBundle, Is.Null);
					Assert.That(sse_3.immediateBundle, Is.SameAs(bundleA));
					Assert.That(bundleB.immediateBundle, Is.SameAs(bundleA));
					Assert.That(sse_4.immediateBundle, Is.SameAs(bundleB));
					Assert.That(sse_5.immediateBundle, Is.SameAs(bundleB));
					}
				[Test]
				public void level_Various_ReturnsAccordingly(){
					TestSlotSystemElement sse_0 = MakeTestSSE();						
					TestSlotSystemElement sse_1 = MakeTestSSE();
					TestSlotSystemElement sse_2 = MakeTestSSE();
					SlotSystemBundle bundleA = MakeSSBundle();
					TestSlotSystemElement sse_3 = MakeTestSSE();
					SlotSystemBundle bundleB = MakeSSBundle();
					TestSlotSystemElement sse_4 = MakeTestSSE();
					TestSlotSystemElement sse_5 = MakeTestSSE();
						sse_0.SetParent(null);
						sse_1.SetParent(sse_0);
						sse_2.SetParent(sse_1);
						bundleA.SetParent(sse_2);
						sse_3.SetParent(bundleA);
						bundleB.SetParent(sse_3);
						sse_4.SetParent(bundleB);
						sse_5.SetParent(sse_4);
					
					Assert.That(sse_0.level, Is.EqualTo(0));
					Assert.That(sse_1.level, Is.EqualTo(1));
					Assert.That(sse_2.level, Is.EqualTo(2));
					Assert.That(bundleA.level, Is.EqualTo(3));
					Assert.That(sse_3.level, Is.EqualTo(4));
					Assert.That(bundleB.level, Is.EqualTo(5));
					Assert.That(sse_4.level, Is.EqualTo(6));
					Assert.That(sse_5.level, Is.EqualTo(7));
					}
				[Test]
				public void level_ParentNull_ReturnsZero(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					Assert.That(testSSE.level, Is.EqualTo(0));
					}
				[Test]
				public void level_OneParent_ReturnsOne(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubEle = MakeTestSSE();
					testSSE.SetParent(stubEle);

					Assert.That(testSSE.level, Is.EqualTo(1));
					}
				[Test]
				public void level_TwoParent_ReturnsTwo(){
					TestSlotSystemElement stubEle_1 = MakeTestSSE();
					TestSlotSystemElement stubEle_2 = MakeTestSSE();
					TestSlotSystemElement testSSE = MakeTestSSE();
					stubEle_2.SetParent(stubEle_1);
					testSSE.SetParent(stubEle_2);

					Assert.That(testSSE.level, Is.EqualTo(2));
					}
				[Test]
				public void isBundleElement_ParentIsBundle_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					SlotSystemBundle stubBundle = MakeSlotSystemBundle();
					testSSE.SetParent(stubBundle);

					Assert.That(testSSE.isBundleElement, Is.True);
					}
				[Test]
				public void isBundleElement_ParentIsNotBundle_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubSSE = MakeTestSSE();
					testSSE.SetParent(stubSSE);

					Assert.That(testSSE.isBundleElement, Is.False);
					}
				[Test]
				public void isFocusedInHierarchy_Various_ReturnsAccordingly(){
					TestSlotSystemElement sseF_0 = MakeTestSSEWithStateHandler();
						TestSlotSystemElement sseF_0_0 = MakeTestSSEWithStateHandler();
							TestSlotSystemElement sseF_0_0_0 = MakeTestSSEWithStateHandler();
								TestSlotSystemElement sseD_0_0_0_0 = MakeTestSSEWithStateHandler();
									TestSlotSystemElement sseF_0_0_0_0_0 = MakeTestSSEWithStateHandler();
								TestSlotSystemElement sseF_0_0_0_1 = MakeTestSSEWithStateHandler();
						TestSlotSystemElement sseD_0_1 = MakeTestSSEWithStateHandler();
							TestSlotSystemElement sseF_0_1_0 = MakeTestSSEWithStateHandler();
								TestSlotSystemElement sseF_0_1_0_0 = MakeTestSSEWithStateHandler();
									TestSlotSystemElement sseF_0_1_0_0_0 = MakeTestSSEWithStateHandler();
					sseF_0.SetParent(null);
						sseF_0_0.SetParent(sseF_0);
							sseF_0_0_0.SetParent(sseF_0_0);
								sseD_0_0_0_0.SetParent(sseF_0_0_0);
									sseF_0_0_0_0_0.SetParent(sseD_0_0_0_0);
								sseF_0_0_0_1.SetParent(sseF_0_0_0);
						sseD_0_1.SetParent(sseF_0);
							sseF_0_1_0.SetParent(sseD_0_1);
								sseF_0_1_0_0.SetParent(sseF_0_1_0);
									sseF_0_1_0_0_0.SetParent(sseF_0_1_0_0);
					sseF_0.Focus();
					sseF_0_0.Focus();
					sseF_0_0_0.Focus();
					sseD_0_0_0_0.Defocus();
					sseF_0_0_0_0_0.Focus();
					sseF_0_0_0_1.Focus();
					sseD_0_1.Defocus();
					sseF_0_1_0.Focus();
					sseF_0_1_0_0.Focus();
					sseF_0_1_0_0_0.Focus();
					IEnumerable<ISlotSystemElement> elements = new ISlotSystemElement[0];
					sseF_0.SetElements(elements);
					sseF_0_0.SetElements(elements);
					sseF_0_0_0.SetElements(elements);
					sseD_0_0_0_0.SetElements(elements);
					sseF_0_0_0_0_0.SetElements(elements);
					sseF_0_0_0_1.SetElements(elements);
					sseD_0_1.SetElements(elements);
					sseF_0_1_0.SetElements(elements);
					sseF_0_1_0_0.SetElements(elements);
					sseF_0_1_0_0_0.SetElements(elements);

					Assert.That(sseF_0.isFocusedInHierarchy, Is.True);
					Assert.That(sseF_0_0.isFocusedInHierarchy, Is.True);
					Assert.That(sseF_0_0_0.isFocusedInHierarchy, Is.True);
					Assert.That(sseD_0_0_0_0.isFocusedInHierarchy, Is.False);
					Assert.That(sseF_0_0_0_0_0.isFocusedInHierarchy, Is.False);
					Assert.That(sseF_0_0_0_1.isFocusedInHierarchy, Is.True);
					Assert.That(sseD_0_1.isFocusedInHierarchy, Is.False);
					Assert.That(sseF_0_1_0.isFocusedInHierarchy, Is.False);
					Assert.That(sseF_0_1_0_0.isFocusedInHierarchy, Is.False);
					Assert.That(sseF_0_1_0_0_0.isFocusedInHierarchy, Is.False);
					
					}
				[Test]
				public void isFocusedInHierarchy_SelfFocusedAndNoParent_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSEWithStateHandler();
					testSSE.Focus();

					Assert.That(testSSE.isFocusedInHierarchy, Is.True);
					}
				[Test]
				public void isFocusedInHierarchy_SelfNotFocused_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSEWithStateHandler();

					Assert.That(testSSE.isFocusedInHierarchy, Is.False);
					}
				[Test]
				public void isFocusedInHierarchy_SelfFocusedParentNotFocused_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSEWithStateHandler();
					TestSlotSystemElement stubPar = MakeTestSSEWithStateHandler();
					testSSE.SetParent(stubPar);
					stubPar.Defocus();
					testSSE.Focus();
					
					Assert.That(testSSE.isFocusedInHierarchy, Is.False);
					}
				[Test]
				public void isFocusedInHierarchy_SelfAndAllParentsFocused_ReturnsTrue(){
					TestSlotSystemElement stubPar1 = MakeTestSSEWithStateHandler();
					TestSlotSystemElement stubPar2 = MakeTestSSEWithStateHandler();
					TestSlotSystemElement stubPar3 = MakeTestSSEWithStateHandler();
					TestSlotSystemElement testSSE = MakeTestSSEWithStateHandler();
					testSSE.SetParent(stubPar3);
					stubPar3.SetElements(new ISlotSystemElement[]{testSSE});
					stubPar3.SetParent(stubPar2);
					stubPar2.SetElements(new ISlotSystemElement[]{stubPar3});
					stubPar2.SetParent(stubPar1);
					stubPar1.SetElements(new ISlotSystemElement[]{stubPar2});
					stubPar1.FocusRecursively();
					
					Assert.That(testSSE.isFocusedInHierarchy, Is.True);
					}
				[Test]
				public void isFocusedInHierarchy_SomeNonfocusedAncester_ReturnsFalse(){
					TestSlotSystemElement stubPar1 = MakeTestSSEWithStateHandler();
					TestSlotSystemElement stubPar2 = MakeTestSSEWithStateHandler();
					TestSlotSystemElement stubPar3 = MakeTestSSEWithStateHandler();
					TestSlotSystemElement testSSE = MakeTestSSEWithStateHandler();
					testSSE.SetParent(stubPar3);
					stubPar3.SetElements(new ISlotSystemElement[]{testSSE});
					stubPar3.SetParent(stubPar2);
					stubPar2.SetElements(new ISlotSystemElement[]{stubPar3});
					stubPar2.SetParent(stubPar1);
					stubPar1.SetElements(new ISlotSystemElement[]{stubPar2});
					stubPar2.Focus();
					
					Assert.That(testSSE.isFocusedInHierarchy, Is.False);
					}
			/*	Methods	*/
				[Test]
				public void SetHierarchy_HasTransformChildrenWithSSE_SetsThemElements(){
					TestSlotSystemElement sse = MakeTestSSE();
						TestSlotSystemElement childA = MakeTestSSE();
							childA.transform.SetParent(sse.transform);
						TestSlotSystemElement childB = MakeTestSSE();
							childB.transform.SetParent(sse.transform);
						TestSlotSystemElement childC = MakeTestSSE();
							childC.transform.SetParent(sse.transform);
						IEnumerable<ISlotSystemElement> expectedEles = new ISlotSystemElement[]{childA, childB, childC};
					
					sse.SetHierarchy();

					bool equality = sse.MemberEquals(expectedEles);
					Assert.That(equality, Is.True);
					}
				[Test]
				public void SetHierarchy_HasTransformChildrenWithSSE_SetsTheirParentThis(){
					TestSlotSystemElement sse = MakeTestSSE();
						TestSlotSystemElement childA = MakeTestSSE();
							childA.transform.SetParent(sse.transform);
						TestSlotSystemElement childB = MakeTestSSE();
							childB.transform.SetParent(sse.transform);
						TestSlotSystemElement childC = MakeTestSSE();
							childC.transform.SetParent(sse.transform);
					
					sse.SetHierarchy();

					Assert.That(childA.parent, Is.SameAs(sse));
					Assert.That(childB.parent, Is.SameAs(sse));
					Assert.That(childC.parent, Is.SameAs(sse));
				}
				[Test]
				public void SetHierarchy_HasTransformChildrenWithoutSSE_SetsElementsEmpty(){
					TestSlotSystemElement sse = MakeTestSSE();
						GameObject childA = MakeGO();
						GameObject childB = MakeGO();
						GameObject childC = MakeGO();
					
					sse.SetHierarchy();

					Assert.That(sse, Is.Empty);
				}
				public GameObject MakeGO(){
					GameObject go = new GameObject("go");
					go.tag = "TestGO";
					return go;
				}
				[Test]
				public void ContainsInHierarchy_Various_ReturnsAccordingly(){
					TestSlotSystemElement sse_0 = MakeTestSSE();
						TestSlotSystemElement sse_0_0 = MakeTestSSE();
							TestSlotSystemElement sse_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_0_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_1 = MakeTestSSE();
						TestSlotSystemElement sse_0_1 = MakeTestSSE();
							TestSlotSystemElement sse_0_1_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_1_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_1_0_0_0 = MakeTestSSE();
					sse_0.SetParent(null);
						sse_0_0.SetParent(sse_0);
							sse_0_0_0.SetParent(sse_0_0);
								sse_0_0_0_0.SetParent(sse_0_0_0);
									sse_0_0_0_0_0.SetParent(sse_0_0_0_0);
								sse_0_0_0_1.SetParent(sse_0_0_0);
						sse_0_1.SetParent(sse_0);
							sse_0_1_0.SetParent(sse_0_1);
								sse_0_1_0_0.SetParent(sse_0_1_0);
									sse_0_1_0_0_0.SetParent(sse_0_1_0_0);
					
					Assert.That(sse_0.ContainsInHierarchy(sse_0_0_0_0_0), Is.True);
						Assert.That(sse_0_0.ContainsInHierarchy(sse_0), Is.False);
						Assert.That(sse_0_0.ContainsInHierarchy(sse_0_1), Is.False);
						Assert.That(sse_0_0.ContainsInHierarchy(sse_0_0_0_0_0), Is.True);
						Assert.That(sse_0_0.ContainsInHierarchy(sse_0_1_0_0), Is.False);
					}
				[Test]
				[ExpectedException(typeof(System.ArgumentNullException))]
				public void ContainsInHierarchy_Null_ThrowsException(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					testSSE.ContainsInHierarchy(null);
					}
				[Test]
				public void ContainsInHierarchy_Self_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					bool result = testSSE.ContainsInHierarchy(testSSE);

					Assert.That(result, Is.False);
					}
				[Test]
				public void ContainsInHierarchy_Parent_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubPar = MakeTestSSE();
					testSSE.SetParent(stubPar);

					bool result = testSSE.ContainsInHierarchy(stubPar);

					Assert.That(result, Is.False);
					}
				[Test]
				public void ContainsInHierarchy_DirectChild_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubChild = MakeTestSSE();
					stubChild.SetParent(testSSE);

					bool result = testSSE.ContainsInHierarchy(stubChild);

					Assert.That(result, Is.True);
					}
				[Test]
				public void ContainsInHierarchy_DistantChild_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubChild = MakeTestSSE();
					TestSlotSystemElement stubGrandChild = MakeTestSSE();
					stubGrandChild.SetParent(stubChild);
					stubChild.SetParent(testSSE);

					bool result = testSSE.ContainsInHierarchy(stubGrandChild);

					Assert.That(result, Is.True);
					}
				[Test]
				public void PerformInHierarchyVer1_Various_PerformsAccordingly(){
					TestSlotSystemElement sse_0 = MakeTestSSE();
						TestSlotSystemElement sse_0_0 = MakeTestSSE();
							TestSlotSystemElement sse_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_0_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_1 = MakeTestSSE();
						TestSlotSystemElement sse_0_1 = MakeTestSSE();
							TestSlotSystemElement sse_0_1_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_1_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_1_0_0_0 = MakeTestSSE();
					sse_0.SetParent(null);
						sse_0_0.SetParent(sse_0);
							sse_0_0_0.SetParent(sse_0_0);
								sse_0_0_0_0.SetParent(sse_0_0_0);
									sse_0_0_0_0_0.SetParent(sse_0_0_0_0);
								sse_0_0_0_1.SetParent(sse_0_0_0);
						sse_0_1.SetParent(sse_0);
							sse_0_1_0.SetParent(sse_0_1);
								sse_0_1_0_0.SetParent(sse_0_1_0);
									sse_0_1_0_0_0.SetParent(sse_0_1_0_0);
					sse_0.SetElements(new ISlotSystemElement[]{sse_0_0, sse_0_1});
						sse_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0});
							sse_0_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0_0, sse_0_0_0_1});
								sse_0_0_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0_0_0});
									sse_0_0_0_0_0.SetElements(new ISlotSystemElement[]{});
								sse_0_0_0_1.SetElements(new ISlotSystemElement[]{});
						sse_0_1.SetElements(new ISlotSystemElement[]{sse_0_1_0});
							sse_0_1_0.SetElements(new ISlotSystemElement[]{sse_0_1_0_0});
								sse_0_1_0_0.SetElements(new ISlotSystemElement[]{sse_0_1_0_0_0});
									sse_0_1_0_0_0.SetElements(new ISlotSystemElement[]{});
					IEnumerable<ISlotSystemElement> down_0 = new ISlotSystemElement[]{
						sse_0, 
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1, 
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_0 = new ISlotSystemElement[]{
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<ISlotSystemElement> down_0_0_0 = new ISlotSystemElement[]{
						sse_0_0_0,
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<ISlotSystemElement> down_0_0_0_0 = new ISlotSystemElement[]{
						sse_0_0_0_0, 
						sse_0_0_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1 = new ISlotSystemElement[]{
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1_0 = new ISlotSystemElement[]{
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1_0_0 = new ISlotSystemElement[]{
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					sse_0.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0_0.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0_0_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_0_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0_0_0.PerformInHierarchy(SetHi);
						Assert.That(sse_0_0_0_0_0.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(e != sse_0_0_0_0_0)
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0_1)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_1).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0_1_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_1_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0_0.PerformInHierarchy(SetHi);
						foreach(TestSlotSystemElement e in down_0_1_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_1_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0_0_0.PerformInHierarchy(SetHi);
						Assert.That(sse_0_1_0_0_0.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(e != sse_0_1_0_0_0)
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					}
					void SetHi(ISlotSystemElement ele){
						TestSlotSystemElement testEle = (TestSlotSystemElement)ele;
						testEle.message = "Hi";
					}
					void ClearHi(IEnumerable<ISlotSystemElement> eles){
						foreach(ISlotSystemElement ele in eles){
							TestSlotSystemElement testEle = (TestSlotSystemElement)ele;
							testEle.message = "";
						}
					}
				[Test]
				public void PerformInHierarchyVer2_Various_PerformsAccordingly(){
					TestSlotSystemElement sse_0 = MakeTestSSE();
						TestSlotSystemElement sse_0_0 = MakeTestSSE();
							TestSlotSystemElement sse_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_0_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_1 = MakeTestSSE();
						TestSlotSystemElement sse_0_1 = MakeTestSSE();
							TestSlotSystemElement sse_0_1_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_1_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_1_0_0_0 = MakeTestSSE();
					sse_0.SetParent(null);
						sse_0_0.SetParent(sse_0);
							sse_0_0_0.SetParent(sse_0_0);
								sse_0_0_0_0.SetParent(sse_0_0_0);
									sse_0_0_0_0_0.SetParent(sse_0_0_0_0);
								sse_0_0_0_1.SetParent(sse_0_0_0);
						sse_0_1.SetParent(sse_0);
							sse_0_1_0.SetParent(sse_0_1);
								sse_0_1_0_0.SetParent(sse_0_1_0);
									sse_0_1_0_0_0.SetParent(sse_0_1_0_0);
					sse_0.SetElements(new ISlotSystemElement[]{sse_0_0, sse_0_1});
						sse_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0});
							sse_0_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0_0, sse_0_0_0_1});
								sse_0_0_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0_0_0});
									sse_0_0_0_0_0.SetElements(new ISlotSystemElement[]{});
								sse_0_0_0_1.SetElements(new ISlotSystemElement[]{});
						sse_0_1.SetElements(new ISlotSystemElement[]{sse_0_1_0});
							sse_0_1_0.SetElements(new ISlotSystemElement[]{sse_0_1_0_0});
								sse_0_1_0_0.SetElements(new ISlotSystemElement[]{sse_0_1_0_0_0});
									sse_0_1_0_0_0.SetElements(new ISlotSystemElement[]{});
					IEnumerable<ISlotSystemElement> down_0 = new ISlotSystemElement[]{
						sse_0, 
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1, 
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_0 = new ISlotSystemElement[]{
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<ISlotSystemElement> down_0_0_0 = new ISlotSystemElement[]{
						sse_0_0_0,
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<ISlotSystemElement> down_0_0_0_0 = new ISlotSystemElement[]{
						sse_0_0_0_0, 
						sse_0_0_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1 = new ISlotSystemElement[]{
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1_0 = new ISlotSystemElement[]{
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1_0_0 = new ISlotSystemElement[]{
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					sse_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0_0_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_0_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0_0_0.PerformInHierarchy(SetHi, "Hi");
						Assert.That(sse_0_0_0_0_0.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(e != sse_0_0_0_0_0)
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0_1)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_1).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0_1_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_1_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestSlotSystemElement e in down_0_1_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(!new List<ISlotSystemElement>(down_0_1_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0_0_0.PerformInHierarchy(SetHi, "Hi");
						Assert.That(sse_0_1_0_0_0.message, Is.StringContaining("Hi"));
						foreach(TestSlotSystemElement e in down_0)
							if(e != sse_0_1_0_0_0)
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					}
					void SetHi(ISlotSystemElement ele, object str){
						string s = (string)str;
						((TestSlotSystemElement)ele).message = s;
					}
				[Test]
				public void PerformInHierarchyVer3_Various_PerformsAccordingly(){
					TestSlotSystemElement sse_0 = MakeTestSSE();
						TestSlotSystemElement sse_0_0 = MakeTestSSE();
							TestSlotSystemElement sse_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_0_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_0_0_1 = MakeTestSSE();
						TestSlotSystemElement sse_0_1 = MakeTestSSE();
							TestSlotSystemElement sse_0_1_0 = MakeTestSSE();
								TestSlotSystemElement sse_0_1_0_0 = MakeTestSSE();
									TestSlotSystemElement sse_0_1_0_0_0 = MakeTestSSE();
					sse_0.SetParent(null);
						sse_0_0.SetParent(sse_0);
							sse_0_0_0.SetParent(sse_0_0);
								sse_0_0_0_0.SetParent(sse_0_0_0);
									sse_0_0_0_0_0.SetParent(sse_0_0_0_0);
								sse_0_0_0_1.SetParent(sse_0_0_0);
						sse_0_1.SetParent(sse_0);
							sse_0_1_0.SetParent(sse_0_1);
								sse_0_1_0_0.SetParent(sse_0_1_0);
									sse_0_1_0_0_0.SetParent(sse_0_1_0_0);
					sse_0.SetElements(new ISlotSystemElement[]{sse_0_0, sse_0_1});
						sse_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0});
							sse_0_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0_0, sse_0_0_0_1});
								sse_0_0_0_0.SetElements(new ISlotSystemElement[]{sse_0_0_0_0_0});
									sse_0_0_0_0_0.SetElements(new ISlotSystemElement[]{});
								sse_0_0_0_1.SetElements(new ISlotSystemElement[]{});
						sse_0_1.SetElements(new ISlotSystemElement[]{sse_0_1_0});
							sse_0_1_0.SetElements(new ISlotSystemElement[]{sse_0_1_0_0});
								sse_0_1_0_0.SetElements(new ISlotSystemElement[]{sse_0_1_0_0_0});
									sse_0_1_0_0_0.SetElements(new ISlotSystemElement[]{});
					IEnumerable<ISlotSystemElement> down_0 = new ISlotSystemElement[]{
						sse_0, 
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1, 
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_0 = new ISlotSystemElement[]{
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<ISlotSystemElement> down_0_0_0 = new ISlotSystemElement[]{
						sse_0_0_0,
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<ISlotSystemElement> down_0_0_0_0 = new ISlotSystemElement[]{
						sse_0_0_0_0, 
						sse_0_0_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1 = new ISlotSystemElement[]{
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1_0 = new ISlotSystemElement[]{
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<ISlotSystemElement> down_0_1_0_0 = new ISlotSystemElement[]{
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					List<ISlotSystemElement> down_0List = new List<ISlotSystemElement>();
						sse_0.PerformInHierarchy(PutInList, down_0List);
						Assert.That(down_0List.MemberEquals(down_0), Is.True);
					List<ISlotSystemElement> down_0_0List = new List<ISlotSystemElement>();
						sse_0_0.PerformInHierarchy(PutInList, down_0_0List);
						Assert.That(down_0_0List.MemberEquals(down_0_0), Is.True);
					List<ISlotSystemElement> down_0_0_0List = new List<ISlotSystemElement>();
						sse_0_0_0.PerformInHierarchy(PutInList, down_0_0_0List);
						Assert.That(down_0_0_0List.MemberEquals(down_0_0_0), Is.True);
					List<ISlotSystemElement> down_0_0_0_0List = new List<ISlotSystemElement>();
						sse_0_0_0_0.PerformInHierarchy(PutInList, down_0_0_0_0List);
						Assert.That(down_0_0_0_0List.MemberEquals(down_0_0_0_0), Is.True);
					List<ISlotSystemElement> down_0_0_0_0_0List = new List<ISlotSystemElement>();
						sse_0_0_0_0_0.PerformInHierarchy(PutInList, down_0_0_0_0_0List);
						Assert.That(down_0_0_0_0_0List.MemberEquals(new ISlotSystemElement[]{sse_0_0_0_0_0}), Is.True);
					List<ISlotSystemElement> down_0_1List = new List<ISlotSystemElement>();
						sse_0_1.PerformInHierarchy(PutInList, down_0_1List);
						Assert.That(down_0_1List.MemberEquals(down_0_1), Is.True);
					List<ISlotSystemElement> down_0_1_0List = new List<ISlotSystemElement>();
						sse_0_1_0.PerformInHierarchy(PutInList, down_0_1_0List);
						Assert.That(down_0_1_0List.MemberEquals(down_0_1_0), Is.True);
					List<ISlotSystemElement> down_0_1_0_0List = new List<ISlotSystemElement>();
						sse_0_1_0_0.PerformInHierarchy(PutInList, down_0_1_0_0List);
						Assert.That(down_0_1_0_0List.MemberEquals(down_0_1_0_0), Is.True);
					List<ISlotSystemElement> down_0_1_0_0_0List = new List<ISlotSystemElement>();
						sse_0_1_0_0_0.PerformInHierarchy(PutInList, down_0_1_0_0_0List);
						Assert.That(down_0_1_0_0_0List.MemberEquals(new ISlotSystemElement[]{sse_0_1_0_0_0}), Is.True);
					}
					void PutInList<ISlotSystemElement>(ISlotSystemElement ele, IList<ISlotSystemElement> list){
						list.Add(ele);
					}
				[Test]
				public void PerformInHierarchyVer1_WhenCalled_PerformActionOnElements(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement mockChild_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_1_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_1_2 = MakeTestSSE();
					TestSlotSystemElement mockChild_2 = MakeTestSSE();
					TestSlotSystemElement mockChild_2_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_2_2 = MakeTestSSE();
					TestSlotSystemElement mockPar = MakeTestSSE();
					mockChild_1.SetElements(new ISlotSystemElement[]{mockChild_1_1, mockChild_1_2});
					mockChild_2.SetElements(new ISlotSystemElement[]{mockChild_2_1, mockChild_2_2});
					testSSE.SetElements(new ISlotSystemElement[]{mockChild_1, mockChild_2});
					mockPar.SetElements(new ISlotSystemElement[]{testSSE});

					testSSE.PerformInHierarchy(x => ((TestSlotSystemElement)x).message = "performed");

					Assert.That(mockPar.message, Is.Empty);
					Assert.That(testSSE.message, Is.StringContaining("performed"));
					Assert.That(mockChild_1.message, Is.StringContaining("performed"));
					Assert.That(mockChild_1_1.message, Is.StringContaining("performed"));
					Assert.That(mockChild_1_2.message, Is.StringContaining("performed"));
					Assert.That(mockChild_2.message, Is.StringContaining("performed"));
					Assert.That(mockChild_2_1.message, Is.StringContaining("performed"));
					Assert.That(mockChild_2_2.message, Is.StringContaining("performed"));
					}
				[Test]
				public void PerformInHierarchyVer2_WhenCalled_PerformActionOnElements(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement mockChild_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_1_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_1_2 = MakeTestSSE();
					TestSlotSystemElement mockChild_2 = MakeTestSSE();
					TestSlotSystemElement mockChild_2_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_2_2 = MakeTestSSE();
					TestSlotSystemElement mockPar = MakeTestSSE();
					mockChild_1.SetElements(new ISlotSystemElement[]{mockChild_1_1, mockChild_1_2});
					mockChild_2.SetElements(new ISlotSystemElement[]{mockChild_2_1, mockChild_2_2});
					testSSE.SetElements(new ISlotSystemElement[]{mockChild_1, mockChild_2});
					mockPar.SetElements(new ISlotSystemElement[]{testSSE});

					System.Action<ISlotSystemElement, object> act = (ISlotSystemElement x, object s) => ((TestSlotSystemElement)x).message = (string)s;
					testSSE.PerformInHierarchy(act, "performed");

					Assert.That(mockPar.message, Is.Empty);
					Assert.That(testSSE.message, Is.StringContaining("performed"));
					Assert.That(mockChild_1.message, Is.StringContaining("performed"));
					Assert.That(mockChild_1_1.message, Is.StringContaining("performed"));
					Assert.That(mockChild_1_2.message, Is.StringContaining("performed"));
					Assert.That(mockChild_2.message, Is.StringContaining("performed"));
					Assert.That(mockChild_2_1.message, Is.StringContaining("performed"));
					Assert.That(mockChild_2_2.message, Is.StringContaining("performed"));
					}
				[Test]
				public void PerformInHierarchyVer3_WhenCalled_PerformActionOnElements(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement mockChild_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_1_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_1_2 = MakeTestSSE();
					TestSlotSystemElement mockChild_2 = MakeTestSSE();
					TestSlotSystemElement mockChild_2_1 = MakeTestSSE();
					TestSlotSystemElement mockChild_2_2 = MakeTestSSE();
					TestSlotSystemElement mockPar = MakeTestSSE();
					mockChild_1.SetElements(new ISlotSystemElement[]{mockChild_1_1, mockChild_1_2});
					mockChild_2.SetElements(new ISlotSystemElement[]{mockChild_2_1, mockChild_2_2});
					testSSE.SetElements(new ISlotSystemElement[]{mockChild_1, mockChild_2});
					mockPar.SetElements(new ISlotSystemElement[]{testSSE});

					System.Action<ISlotSystemElement, IList<string>> act = (ISlotSystemElement x, IList<string> list) => {
						string concat = "";
						foreach(string s in list){
							concat += s;
						}
						((TestSlotSystemElement)x).message = concat;
					};
					List<string> stringList = new List<string>(new string[]{"per", "formed"});
					testSSE.PerformInHierarchy(act, stringList);

					Assert.That(mockPar.message, Is.Empty);
					Assert.That(testSSE.message, Is.StringContaining("performed"));
					Assert.That(mockChild_1.message, Is.StringContaining("performed"));
					Assert.That(mockChild_1_1.message, Is.StringContaining("performed"));
					Assert.That(mockChild_1_2.message, Is.StringContaining("performed"));
					Assert.That(mockChild_2.message, Is.StringContaining("performed"));
					Assert.That(mockChild_2_1.message, Is.StringContaining("performed"));
					Assert.That(mockChild_2_2.message, Is.StringContaining("performed"));
					}
				[Test]
				public void Contains_NoElements_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement stubEle = Substitute.For<ISlotSystemElement>();
					
					Assert.That(testSSE.Contains(stubEle), Is.False);
					}
				[Test]
				public void Contains_NonMember_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement stubMember = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement stubNonMember = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{stubMember});
					
					Assert.That(testSSE.Contains(stubNonMember), Is.False);
					}
				[Test]
				public void Contains_Member_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement stubMember = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{stubMember});
					
					Assert.That(testSSE.Contains(stubMember), Is.True);
					}

			/* Other */
				[TestCaseSource(typeof(isFocusableInHierarchyCases))]
				public void isFocusableInHierarchy_AllAncestorsContainedInBundleFocusedElement_ReturnsTrue(TestSlotSystemElement top, IEnumerable<ISlotSystemElement> xOn, IEnumerable<ISlotSystemElement> xOff){
					foreach(var e in xOn)
						Assert.That(e.isFocusableInHierarchy, Is.True);
					foreach(var e in xOff)
						Assert.That(e.isFocusableInHierarchy, Is.False);
				}
				class isFocusableInHierarchyCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] case0;
							TestSlotSystemElement top_0 = MakeTestSSE();
								SlotSystemBundle bun0_0 = MakeSSBundle();
									TestSlotSystemElement sse00_0 = MakeTestSSE();
									TestSlotSystemElement sse01_0 = MakeTestSSE();//foc
									TestSlotSystemElement sse02_0 = MakeTestSSE();
									SlotSystemBundle bun03_0 = MakeSSBundle();
										TestSlotSystemElement sse030_0 = MakeTestSSE();//foc
										TestSlotSystemElement sse031_0 = MakeTestSSE();
								SlotSystemBundle bun1_0 = MakeSSBundle();
									SlotSystemBundle bun10_0 = MakeSSBundle();
										TestSlotSystemElement sse100_0 = MakeTestSSE();//foc
										TestSlotSystemElement sse101_0 = MakeTestSSE();
									SlotSystemBundle bun11_0 = MakeSSBundle();//foc
										SlotSystemBundle bun110_0 = MakeSSBundle();//foc
											TestSlotSystemElement sse1100_0 = MakeTestSSE();//foc
											TestSlotSystemElement sse1101_0 = MakeTestSSE();
										SlotSystemBundle bun111_0 = MakeSSBundle();
										SlotSystemBundle bun112_0 = MakeSSBundle();
							bun0_0.transform.SetParent(top_0.transform);
								sse00_0.transform.SetParent(bun0_0.transform);
								sse01_0.transform.SetParent(bun0_0.transform);
								sse02_0.transform.SetParent(bun0_0.transform);
								bun03_0.transform.SetParent(bun0_0.transform);
									sse030_0.transform.SetParent(bun03_0.transform);
									sse031_0.transform.SetParent(bun03_0.transform);
							bun1_0.transform.SetParent(top_0.transform);
								bun10_0.transform.SetParent(bun1_0.transform);
									sse100_0.transform.SetParent(bun10_0.transform);
									sse101_0.transform.SetParent(bun10_0.transform);
								bun11_0.transform.SetParent(bun1_0.transform);
									bun110_0.transform.SetParent(bun11_0.transform);
										sse1100_0.transform.SetParent(bun110_0.transform);
										sse1101_0.transform.SetParent(bun110_0.transform);
									bun111_0.transform.SetParent(bun11_0.transform);
									bun112_0.transform.SetParent(bun11_0.transform);

							top_0.SetHierarchyRecursively();

							bun0_0.InspectorSetUp(sse01_0);
							bun03_0.InspectorSetUp(sse030_0);
							bun1_0.InspectorSetUp(bun11_0);
							bun10_0.InspectorSetUp(sse100_0);
							bun11_0.InspectorSetUp(bun110_0);
							bun110_0.InspectorSetUp(sse1100_0);

							IEnumerable<ISlotSystemElement> xOn_0 = new ISlotSystemElement[]{
								top_0,
								bun0_0,
								sse01_0,
								bun1_0,
								bun11_0,
								bun110_0,
								sse1100_0
							};
							IEnumerable<ISlotSystemElement> xOff_0 = new ISlotSystemElement[]{
								sse00_0,
								sse02_0,
								bun03_0,
								sse030_0,
								sse031_0,
								bun10_0,
								sse100_0,
								sse101_0,
								sse1101_0,
								bun111_0,
								bun112_0
							};
							case0 = new object[]{top_0, xOn_0, xOff_0};
							yield return case0;
						object[] case1;
							TestSlotSystemElement top_1 = MakeTestSSE();
								SlotSystemBundle bun0_1 = MakeSSBundle();
									TestSlotSystemElement sse00_1 = MakeTestSSE();
									TestSlotSystemElement sse01_1 = MakeTestSSE();
									TestSlotSystemElement sse02_1 = MakeTestSSE();
									SlotSystemBundle bun03_1 = MakeSSBundle();//foc
										TestSlotSystemElement sse030_1 = MakeTestSSE();//foc
										TestSlotSystemElement sse031_1 = MakeTestSSE();
								SlotSystemBundle bun1_1 = MakeSSBundle();
									SlotSystemBundle bun10_1 = MakeSSBundle();//foc
										TestSlotSystemElement sse100_1 = MakeTestSSE();//foc
										TestSlotSystemElement sse101_1 = MakeTestSSE();
									SlotSystemBundle bun11_1 = MakeSSBundle();
										SlotSystemBundle bun110_1 = MakeSSBundle();//foc
											TestSlotSystemElement sse1100_1 = MakeTestSSE();//foc
											TestSlotSystemElement sse1101_1 = MakeTestSSE();
										SlotSystemBundle bun111_1 = MakeSSBundle();
										SlotSystemBundle bun112_1 = MakeSSBundle();
							bun0_1.transform.SetParent(top_1.transform);
								sse00_1.transform.SetParent(bun0_1.transform);
								sse01_1.transform.SetParent(bun0_1.transform);
								sse02_1.transform.SetParent(bun0_1.transform);
								bun03_1.transform.SetParent(bun0_1.transform);
									sse030_1.transform.SetParent(bun03_1.transform);
									sse031_1.transform.SetParent(bun03_1.transform);
							bun1_1.transform.SetParent(top_1.transform);
								bun10_1.transform.SetParent(bun1_1.transform);
									sse100_1.transform.SetParent(bun10_1.transform);
									sse101_1.transform.SetParent(bun10_1.transform);
								bun11_1.transform.SetParent(bun1_1.transform);
									bun110_1.transform.SetParent(bun11_1.transform);
										sse1100_1.transform.SetParent(bun110_1.transform);
										sse1101_1.transform.SetParent(bun110_1.transform);
									bun111_1.transform.SetParent(bun11_1.transform);
									bun112_1.transform.SetParent(bun11_1.transform);

							top_1.SetHierarchyRecursively();

							bun0_1.InspectorSetUp(bun03_1);
							bun03_1.InspectorSetUp(sse030_1);
							bun1_1.InspectorSetUp(bun10_1);
							bun10_1.InspectorSetUp(sse100_1);
							bun11_1.InspectorSetUp(bun110_1);
							bun110_1.InspectorSetUp(sse1100_1);

							IEnumerable<ISlotSystemElement> xOn_1 = new ISlotSystemElement[]{
								top_1,
								bun0_1,
								bun03_1,
								sse030_1,
								bun1_1,
								bun10_1,
								sse100_1
							};
							IEnumerable<ISlotSystemElement> xOff_1 = new ISlotSystemElement[]{
								sse00_1,
								sse01_1,
								sse02_1,
								sse031_1,
								sse101_1,
								bun11_1,
								bun110_1,
								sse1100_1,
								sse1101_1,
								bun111_1,
								bun112_1
							};
							case1 = new object[]{top_1, xOn_1, xOff_1};
							yield return case1;
					}
				}
				static TestSlotSystemElement MakeTestSSEWithStateHandler(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					SSESelStateHandler handler = new SSESelStateHandler();
					testSSE.SetSelStateHandler(handler);
					return testSSE;
				}
				static SlotSystemBundle MakeSSBundleWithStateHandler(){
					SlotSystemBundle bundle = MakeSSBundle();
					SSESelStateHandler handler = new SSESelStateHandler();
					bundle.SetSelStateHandler(handler);
					return bundle;
				}
				[TestCaseSource(typeof(ActivateRecursivelyCases))]
				public void ActivateRecursively_WhenCalled_FocusIfIsAOFAndFocusable_DefocusIfIsAODAndNotFocusable(TestSlotSystemElement top, IEnumerable<ISlotSystemElement> xFocused, IEnumerable<ISlotSystemElement> xDefocused, IEnumerable<ISlotSystemElement> xDeactivated){

					top.ActivateRecursively();

					foreach(var e in xFocused)
						Assert.That(e.isFocused, Is.True);
					foreach(var e in xDefocused)
						Assert.That(e.isDefocused, Is.True);
					foreach(var e in xDeactivated)
						Assert.That(e.isDeactivated, Is.True);
				}
					class ActivateRecursivelyCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case0;
								TestSlotSystemElement top_0 = MakeTestSSEWithStateHandler();
									SlotSystemBundle bun0_0 = MakeSSBundleWithStateHandler();
										TestSlotSystemElement sse00_0 = MakeTestSSEWithStateHandler();
											SlotSystemBundle bun000_0 = MakeSSBundleWithStateHandler();
												TestSlotSystemElement sse0000_0 = MakeTestSSEWithStateHandler();
												TestSlotSystemElement sse0001_0 = MakeTestSSEWithStateHandler();
											TestSlotSystemElement sse001_0 = MakeTestSSEWithStateHandler();
												TestSlotSystemElement sse0010_0 = MakeTestSSEWithStateHandler();
												TestSlotSystemElement sse0011_0 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse01_0 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse02_0 = MakeTestSSEWithStateHandler();
									SlotSystemBundle bun1_0 = MakeSSBundleWithStateHandler();
										SlotSystemBundle bun10_0 = MakeSSBundleWithStateHandler();
											TestSlotSystemElement sse100_0 = MakeTestSSEWithStateHandler();
											TestSlotSystemElement sse101_0 = MakeTestSSEWithStateHandler();
												SlotSystemBundle bun1010_0 = MakeSSBundleWithStateHandler();
													TestSlotSystemElement sse10100_0 = MakeTestSSEWithStateHandler();
													TestSlotSystemElement sse10101_0 = MakeTestSSEWithStateHandler();
										SlotSystemBundle bun11_0 = MakeSSBundleWithStateHandler();
											TestSlotSystemElement sse110_0 = MakeTestSSEWithStateHandler();
											TestSlotSystemElement sse111_0 = MakeTestSSEWithStateHandler();
									SlotSystemBundle bun2_0 = MakeSSBundleWithStateHandler();
										SlotSystemBundle bun20_0 = MakeSSBundleWithStateHandler();
										SlotSystemBundle bun21_0 = MakeSSBundleWithStateHandler();
									bun0_0.transform.SetParent(top_0.transform);
										sse00_0.transform.SetParent(bun0_0.transform);/* foc */
											bun000_0.transform.SetParent(sse00_0.transform);
												sse0000_0.transform.SetParent(bun000_0.transform);/* foc */
												sse0001_0.transform.SetParent(bun000_0.transform);
											/* !isAOD */
											sse001_0.transform.SetParent(sse00_0.transform);
												sse0010_0.transform.SetParent(sse001_0.transform);
												sse0011_0.transform.SetParent(sse001_0.transform);
											/* ****** */
										sse01_0.transform.SetParent(bun0_0.transform);
										sse02_0.transform.SetParent(bun0_0.transform);
									bun1_0.transform.SetParent(top_0.transform);
										bun10_0.transform.SetParent(bun1_0.transform);/* foc */
											sse100_0.transform.SetParent(bun10_0.transform);
											sse101_0.transform.SetParent(bun10_0.transform);/* foc */
												bun1010_0.transform.SetParent(sse101_0.transform);
													sse10100_0.transform.SetParent(bun1010_0.transform);
													sse10101_0.transform.SetParent(bun1010_0.transform);/* foc */
										/* !isAOD */
										bun11_0.transform.SetParent(bun1_0.transform);
											sse110_0.transform.SetParent(bun11_0.transform);//foc
											sse111_0.transform.SetParent(bun11_0.transform);
										/* ****** */
									/* !isAOD */
									bun2_0.transform.SetParent(top_0.transform);
										bun20_0.transform.SetParent(bun2_0.transform);
										bun21_0.transform.SetParent(bun2_0.transform);//foc
									/* ****** */
								
								top_0.SetHierarchyRecursively();
								top_0.InitializeStatesRecursively();

								bun0_0.InspectorSetUp(sse00_0);
								bun000_0.InspectorSetUp(sse0000_0);
								bun1_0.InspectorSetUp(bun10_0);
								bun10_0.InspectorSetUp(sse101_0);
								bun1010_0.InspectorSetUp(sse10101_0);
								bun11_0.InspectorSetUp(sse110_0);
								bun2_0.InspectorSetUp(bun21_0);

								sse001_0.isActivatedOnDefault = false;
								bun11_0.isActivatedOnDefault = false;
								bun2_0.isActivatedOnDefault = false;

								IEnumerable<ISlotSystemElement> xFocused_0 = new ISlotSystemElement[]{
									top_0,
									bun0_0,
									sse00_0,
									bun000_0,
									sse0000_0,
									bun1_0,
									bun10_0,
									sse101_0,
									bun1010_0,
									sse10101_0
								};
								IEnumerable<ISlotSystemElement> xDefocused_0 = new ISlotSystemElement[]{
									sse0001_0,
									sse01_0,
									sse02_0,
									sse100_0,
									sse10100_0
								};
								IEnumerable<ISlotSystemElement> xDeactivated_0 = new ISlotSystemElement[]{
									sse001_0,
									sse0010_0,
									sse0011_0,
									bun11_0,
									sse110_0,
									sse111_0,
									bun2_0,
									bun20_0,
									bun21_0
								};
								case0 = new object[]{top_0, xFocused_0, xDefocused_0, xDeactivated_0};
								yield return case0;
							object[] case1;
								TestSlotSystemElement top_1 = MakeTestSSEWithStateHandler();
									SlotSystemBundle bun0_1 = MakeSSBundleWithStateHandler();
										TestSlotSystemElement sse00_1 = MakeTestSSEWithStateHandler();
											SlotSystemBundle bun000_1 = MakeSSBundleWithStateHandler();
												TestSlotSystemElement sse0000_1 = MakeTestSSEWithStateHandler();
												TestSlotSystemElement sse0001_1 = MakeTestSSEWithStateHandler();
											TestSlotSystemElement sse001_1 = MakeTestSSEWithStateHandler();
												TestSlotSystemElement sse0010_1 = MakeTestSSEWithStateHandler();
												TestSlotSystemElement sse0011_1 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse01_1 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse02_1 = MakeTestSSEWithStateHandler();
									SlotSystemBundle bun1_1 = MakeSSBundleWithStateHandler();
										SlotSystemBundle bun10_1 = MakeSSBundleWithStateHandler();
											TestSlotSystemElement sse100_1 = MakeTestSSEWithStateHandler();
											TestSlotSystemElement sse101_1 = MakeTestSSEWithStateHandler();
												SlotSystemBundle bun1010_1 = MakeSSBundleWithStateHandler();
													TestSlotSystemElement sse10100_1 = MakeTestSSEWithStateHandler();
													TestSlotSystemElement sse10101_1 = MakeTestSSEWithStateHandler();
										SlotSystemBundle bun11_1 = MakeSSBundleWithStateHandler();
											TestSlotSystemElement sse110_1 = MakeTestSSEWithStateHandler();
											TestSlotSystemElement sse111_1 = MakeTestSSEWithStateHandler();
									SlotSystemBundle bun2_1 = MakeSSBundleWithStateHandler();
										SlotSystemBundle bun20_1 = MakeSSBundleWithStateHandler();
										SlotSystemBundle bun21_1 = MakeSSBundleWithStateHandler();
									/* !isAOD */
									bun0_1.transform.SetParent(top_1.transform);
										sse00_1.transform.SetParent(bun0_1.transform);/* foc */
											bun000_1.transform.SetParent(sse00_1.transform);
												sse0000_1.transform.SetParent(bun000_1.transform);/* foc */
												sse0001_1.transform.SetParent(bun000_1.transform);
											sse001_1.transform.SetParent(sse00_1.transform);
												sse0010_1.transform.SetParent(sse001_1.transform);
												sse0011_1.transform.SetParent(sse001_1.transform);
										sse01_1.transform.SetParent(bun0_1.transform);
										sse02_1.transform.SetParent(bun0_1.transform);
									/******* */
									bun1_1.transform.SetParent(top_1.transform);
										bun10_1.transform.SetParent(bun1_1.transform);/* foc */
											sse100_1.transform.SetParent(bun10_1.transform);
											sse101_1.transform.SetParent(bun10_1.transform);/* foc */
												bun1010_1.transform.SetParent(sse101_1.transform);
													sse10100_1.transform.SetParent(bun1010_1.transform);
													sse10101_1.transform.SetParent(bun1010_1.transform);/* foc */
										/* !isAOD */
										bun11_1.transform.SetParent(bun1_1.transform);
											sse110_1.transform.SetParent(bun11_1.transform);//foc
											sse111_1.transform.SetParent(bun11_1.transform);
										/* ****** */
									/* !isAOD */
									bun2_1.transform.SetParent(top_1.transform);
										bun20_1.transform.SetParent(bun2_1.transform);
										bun21_1.transform.SetParent(bun2_1.transform);//foc
									/* ****** */
								
								top_1.SetHierarchyRecursively();
								top_1.InitializeStatesRecursively();

								bun0_1.InspectorSetUp(sse00_1);
								bun000_1.InspectorSetUp(sse0000_1);
								bun1_1.InspectorSetUp(bun10_1);
								bun10_1.InspectorSetUp(sse101_1);
								bun1010_1.InspectorSetUp(sse10101_1);
								bun11_1.InspectorSetUp(sse110_1);
								bun2_1.InspectorSetUp(bun21_1);

								bun0_1.isActivatedOnDefault = false;
								bun11_1.isActivatedOnDefault = false;
								bun2_1.isActivatedOnDefault = false;

								IEnumerable<ISlotSystemElement> xFocused_1 = new ISlotSystemElement[]{
									top_1,
									bun1_1,
									bun10_1,
									sse101_1,
									bun1010_1,
									sse10101_1
								};
								IEnumerable<ISlotSystemElement> xDefocused_1 = new ISlotSystemElement[]{
									sse100_1,
									sse10100_1
								};
								IEnumerable<ISlotSystemElement> xDeactivated_1 = new ISlotSystemElement[]{
									bun0_1,
									sse00_1,
									bun000_1,
									sse0000_1,
									sse0001_1,
									sse001_1,
									sse0010_1,
									sse0011_1,
									sse01_1,
									sse02_1,
									bun11_1,
									sse110_1,
									sse111_1,
									bun2_1,
									bun20_1,
									bun21_1
								};
								case1 = new object[]{top_1, xFocused_1, xDefocused_1, xDeactivated_1};
								yield return case1;
						}
					}
				[Test]
				public void isActivatedOnDefault_ParentIsNull_ReturnsTrue(){
					TestSlotSystemElement sse = MakeTestSSE();

					Assert.That(sse.isActivatedOnDefault, Is.True);
				}
				[TestCaseSource(typeof(isActivatedOnDefaultCases))]
				public void isActivatedOnDefault_Always_ReturnsAccordingly(TestSlotSystemElement sse, IEnumerable<ISlotSystemElement> xOn, IEnumerable<ISlotSystemElement> xOff){
					sse.SetHierarchyRecursively();

					sse.RecursiveTestMethod();
					
					foreach(ISlotSystemElement ele in xOn)
						Assert.That(ele.isFocused, Is.True);
					foreach(ISlotSystemElement ele in xOff)
						Assert.That(ele.isFocused, Is.False);
					}
					class isActivatedOnDefaultCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case0;
								TestSlotSystemElement sse_0 = MakeTestSSEWithStateHandler();
									TestSlotSystemElement sse0_0 = MakeTestSSEWithStateHandler();//off
										TestSlotSystemElement sse00_0 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse01_0 = MakeTestSSEWithStateHandler();
									TestSlotSystemElement sse1_0 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse10_0 = MakeTestSSEWithStateHandler();
											TestSlotSystemElement sse100_0 = MakeTestSSEWithStateHandler();//off
												TestSlotSystemElement sse1000_0 = MakeTestSSEWithStateHandler();
											TestSlotSystemElement sse101_0 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse11_0 = MakeTestSSEWithStateHandler();
										sse0_0.transform.SetParent(sse_0.transform);
											sse0_0.isActivatedOnDefault = false;
										sse00_0.transform.SetParent(sse0_0.transform);
										sse01_0.transform.SetParent(sse0_0.transform);
										sse1_0.transform.SetParent(sse_0.transform);
										sse10_0.transform.SetParent(sse1_0.transform);
										sse100_0.transform.SetParent(sse10_0.transform);
											sse100_0.isActivatedOnDefault = false;
										sse1000_0.transform.SetParent(sse100_0.transform);
										sse101_0.transform.SetParent(sse10_0.transform);
										sse11_0.transform.SetParent(sse1_0.transform);
								IEnumerable<ISlotSystemElement> xOn_0 = new ISlotSystemElement[]{
									sse_0,
									sse1_0,
									sse10_0,
									sse101_0,
									sse11_0
								};
								IEnumerable<ISlotSystemElement> xOff_0 = new ISlotSystemElement[]{
									sse0_0,
									sse00_0,
									sse01_0,
									sse100_0,
									sse1000_0,
								};
								case0 = new object[]{sse_0, xOn_0, xOff_0};
								yield return case0;
							object[] case1;
								TestSlotSystemElement sse_1 = MakeTestSSEWithStateHandler();//off
									TestSlotSystemElement sse0_1 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse00_1 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse01_1 = MakeTestSSEWithStateHandler();
									TestSlotSystemElement sse1_1 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse10_1 = MakeTestSSEWithStateHandler();
											TestSlotSystemElement sse100_1 = MakeTestSSEWithStateHandler();
												TestSlotSystemElement sse1000_1 = MakeTestSSEWithStateHandler();
											TestSlotSystemElement sse101_1 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse11_1 = MakeTestSSEWithStateHandler();
										sse_1.isActivatedOnDefault = false;
										sse0_1.transform.SetParent(sse_1.transform);
										sse00_1.transform.SetParent(sse0_1.transform);
										sse01_1.transform.SetParent(sse0_1.transform);
										sse1_1.transform.SetParent(sse_1.transform);
										sse10_1.transform.SetParent(sse1_1.transform);
										sse100_1.transform.SetParent(sse10_1.transform);
										sse1000_1.transform.SetParent(sse100_1.transform);
										sse101_1.transform.SetParent(sse10_1.transform);
										sse11_1.transform.SetParent(sse1_1.transform);
								IEnumerable<ISlotSystemElement> xOn_1 = new ISlotSystemElement[]{
								};
								IEnumerable<ISlotSystemElement> xOff_1 = new ISlotSystemElement[]{
									sse_1,
									sse1_1,
									sse10_1,
									sse101_1,
									sse11_1,
									sse0_1,
									sse00_1,
									sse01_1,
									sse100_1,
									sse1000_1,
								};
								case1 = new object[]{sse_1, xOn_1, xOff_1};
								yield return case1;
							object[] case2;
								TestSlotSystemElement sse_2 = MakeTestSSEWithStateHandler();
									TestSlotSystemElement sse0_2 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse00_2 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse01_2 = MakeTestSSEWithStateHandler();
									TestSlotSystemElement sse1_2 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse10_2 = MakeTestSSEWithStateHandler();
											TestSlotSystemElement sse100_2 = MakeTestSSEWithStateHandler();
												TestSlotSystemElement sse1000_2 = MakeTestSSEWithStateHandler();
											TestSlotSystemElement sse101_2 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse11_2 = MakeTestSSEWithStateHandler();
										sse0_2.transform.SetParent(sse_2.transform);
										sse00_2.transform.SetParent(sse0_2.transform);
										sse01_2.transform.SetParent(sse0_2.transform);
										sse1_2.transform.SetParent(sse_2.transform);
										sse10_2.transform.SetParent(sse1_2.transform);
										sse100_2.transform.SetParent(sse10_2.transform);
										sse1000_2.transform.SetParent(sse100_2.transform);
										sse101_2.transform.SetParent(sse10_2.transform);
										sse11_2.transform.SetParent(sse1_2.transform);
								IEnumerable<ISlotSystemElement> xOn_2 = new ISlotSystemElement[]{
									sse_2,
									sse0_2,
									sse00_2,
									sse01_2,
									sse1_2,
									sse10_2,
									sse100_2,
									sse1000_2,
									sse101_2,
									sse11_2
								};
								IEnumerable<ISlotSystemElement> xOff_2 = new ISlotSystemElement[]{
								};
								case2 = new object[]{sse_2, xOn_2, xOff_2};
								yield return case2;
							object[] case3;
								TestSlotSystemElement sse_3 = MakeTestSSEWithStateHandler();
									TestSlotSystemElement sse0_3 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse00_3 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse01_3 = MakeTestSSEWithStateHandler();
									TestSlotSystemElement sse1_3 = MakeTestSSEWithStateHandler();//off
										TestSlotSystemElement sse10_3 = MakeTestSSEWithStateHandler();//on
											TestSlotSystemElement sse100_3 = MakeTestSSEWithStateHandler();//off
												TestSlotSystemElement sse1000_3 = MakeTestSSEWithStateHandler();
											TestSlotSystemElement sse101_3 = MakeTestSSEWithStateHandler();
										TestSlotSystemElement sse11_3 = MakeTestSSEWithStateHandler();
										sse0_3.transform.SetParent(sse_3.transform);
										sse00_3.transform.SetParent(sse0_3.transform);
										sse01_3.transform.SetParent(sse0_3.transform);
										sse1_3.transform.SetParent(sse_3.transform);
											sse1_3.isActivatedOnDefault = false;
										sse10_3.transform.SetParent(sse1_3.transform);
											sse10_3.isActivatedOnDefault = true;
										sse100_3.transform.SetParent(sse10_3.transform);
											sse100_3.isActivatedOnDefault = false;
										sse1000_3.transform.SetParent(sse100_3.transform);
										sse101_3.transform.SetParent(sse10_3.transform);
										sse11_3.transform.SetParent(sse1_3.transform);
								IEnumerable<ISlotSystemElement> xOn_3 = new ISlotSystemElement[]{
									sse_3,
									sse0_3,
									sse00_3,
									sse01_3
								};
								IEnumerable<ISlotSystemElement> xOff_3 = new ISlotSystemElement[]{
									sse1_3,
									sse10_3,
									sse100_3,
									sse1000_3,
									sse101_3,
									sse11_3
								};
								case3 = new object[]{sse_3, xOn_3, xOff_3};
								yield return case3;
						}
					}
			/*	helpers */
				SlotSystemBundle MakeSlotSystemBundle(){
					return new GameObject("ssBundleGO").AddComponent<SlotSystemBundle>();
				}
		}
	}
}
