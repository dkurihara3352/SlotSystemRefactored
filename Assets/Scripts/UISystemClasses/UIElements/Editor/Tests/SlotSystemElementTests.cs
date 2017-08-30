using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using UISystem;
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
					TestUIElement sse = MakeTestSSE();
						IEnumerable<IUIElement> elements = CreateSSEs(count);
						List<IUIElement> list = new List<IUIElement>(elements);
						sse.SetElements(elements);
					
					for(int i = 0; i< count; i++)
						Assert.That(sse[i], Is.Not.SameAs(list[i]));
					}
				[TestCase(1)]
				[TestCase(10)]
				[TestCase(100)]
				[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
				public void indexer_Invalid_ThrowsException(int count){
					TestUIElement sse = MakeTestSSE();
						IEnumerable<IUIElement> elements = CreateSSEs(count);
						sse.SetElements(elements);
					
					IUIElement invalid = sse[count + 1];
					invalid = null;
					}
				public IEnumerable<IUIElement> CreateSSEs(int count){
					for(int i = 0; i < count; i++)
						yield return Substitute.For<IUIElement>();
					}
				
				[Test]
				public void ImmediateBundle_Various_ReturnsAccordingly(){
					TestUIElement sse_0 = MakeTestSSE();						
					TestUIElement sse_1 = MakeTestSSE();
					TestUIElement sse_2 = MakeTestSSE();
					UIBundle bundleA = MakeSSBundle();
					TestUIElement sse_3 = MakeTestSSE();
					UIBundle bundleB = MakeSSBundle();
					TestUIElement sse_4 = MakeTestSSE();
					TestUIElement sse_5 = MakeTestSSE();
						sse_0.SetParent(null);
						sse_1.SetParent(sse_0);
						sse_2.SetParent(sse_1);
						bundleA.SetParent(sse_2);
						sse_3.SetParent(bundleA);
						bundleB.SetParent(sse_3);
						sse_4.SetParent(bundleB);
						sse_5.SetParent(sse_4);
					
					Assert.That(sse_0.ImmediateBundle(), Is.Null);
					Assert.That(sse_1.ImmediateBundle(), Is.Null);
					Assert.That(sse_2.ImmediateBundle(), Is.Null);
					Assert.That(bundleA.ImmediateBundle(), Is.Null);
					Assert.That(sse_3.ImmediateBundle(), Is.SameAs(bundleA));
					Assert.That(bundleB.ImmediateBundle(), Is.SameAs(bundleA));
					Assert.That(sse_4.ImmediateBundle(), Is.SameAs(bundleB));
					Assert.That(sse_5.ImmediateBundle(), Is.SameAs(bundleB));
					}
				[Test]
				public void GetLevel_Various_ReturnsAccordingly(){
					TestUIElement sse_0 = MakeTestSSE();						
					TestUIElement sse_1 = MakeTestSSE();
					TestUIElement sse_2 = MakeTestSSE();
					UIBundle bundleA = MakeSSBundle();
					TestUIElement sse_3 = MakeTestSSE();
					UIBundle bundleB = MakeSSBundle();
					TestUIElement sse_4 = MakeTestSSE();
					TestUIElement sse_5 = MakeTestSSE();
						sse_0.SetParent(null);
						sse_1.SetParent(sse_0);
						sse_2.SetParent(sse_1);
						bundleA.SetParent(sse_2);
						sse_3.SetParent(bundleA);
						bundleB.SetParent(sse_3);
						sse_4.SetParent(bundleB);
						sse_5.SetParent(sse_4);
					
					Assert.That(sse_0.GetLevel(), Is.EqualTo(0));
					Assert.That(sse_1.GetLevel(), Is.EqualTo(1));
					Assert.That(sse_2.GetLevel(), Is.EqualTo(2));
					Assert.That(bundleA.GetLevel(), Is.EqualTo(3));
					Assert.That(sse_3.GetLevel(), Is.EqualTo(4));
					Assert.That(bundleB.GetLevel(), Is.EqualTo(5));
					Assert.That(sse_4.GetLevel(), Is.EqualTo(6));
					Assert.That(sse_5.GetLevel(), Is.EqualTo(7));
				}
				[Test]
				public void GetLevel_ParentNull_ReturnsZero(){
					TestUIElement testSSE = MakeTestSSE();

					Assert.That(testSSE.GetLevel(), Is.EqualTo(0));
				}
				[Test]
				public void GetLevel_OneParent_ReturnsOne(){
					TestUIElement testSSE = MakeTestSSE();
					TestUIElement stubEle = MakeTestSSE();
					testSSE.SetParent(stubEle);

					Assert.That(testSSE.GetLevel(), Is.EqualTo(1));
				}
				[Test]
				public void GetLevel_TwoParent_ReturnsTwo(){
					TestUIElement stubEle_1 = MakeTestSSE();
					TestUIElement stubEle_2 = MakeTestSSE();
					TestUIElement testSSE = MakeTestSSE();
					stubEle_2.SetParent(stubEle_1);
					testSSE.SetParent(stubEle_2);

					Assert.That(testSSE.GetLevel(), Is.EqualTo(2));
				}
				[Test]
				public void isBundleElement_ParentIsBundle_ReturnsTrue(){
					TestUIElement testSSE = MakeTestSSE();
					UIBundle stubBundle = MakeSlotSystemBundle();
					testSSE.SetParent(stubBundle);

					Assert.That(testSSE.isBundleElement, Is.True);
				}
				[Test]
				public void isBundleElement_ParentIsNotBundle_ReturnsFalse(){
					TestUIElement testSSE = MakeTestSSE();
					TestUIElement stubSSE = MakeTestSSE();
					testSSE.SetParent(stubSSE);

					Assert.That(testSSE.isBundleElement, Is.False);
				}
				[Test]
				public void IsFocused_InHierarchy_Various_ReturnsAccordingly(){
					TestUIElement sseF_0 = MakeTestSSEWithStateHandler();
						TestUIElement sseF_0_0 = MakeTestSSEWithStateHandler();
							TestUIElement sseF_0_0_0 = MakeTestSSEWithStateHandler();
								TestUIElement sseD_0_0_0_0 = MakeTestSSEWithStateHandler();
									TestUIElement sseF_0_0_0_0_0 = MakeTestSSEWithStateHandler();
								TestUIElement sseF_0_0_0_1 = MakeTestSSEWithStateHandler();
						TestUIElement sseD_0_1 = MakeTestSSEWithStateHandler();
							TestUIElement sseF_0_1_0 = MakeTestSSEWithStateHandler();
								TestUIElement sseF_0_1_0_0 = MakeTestSSEWithStateHandler();
									TestUIElement sseF_0_1_0_0_0 = MakeTestSSEWithStateHandler();
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
					sseF_0.SetSelStateHandler(new UISelStateHandler());
					sseF_0_0.SetSelStateHandler(new UISelStateHandler());
					sseF_0_0_0.SetSelStateHandler(new UISelStateHandler());
					sseD_0_0_0_0.SetSelStateHandler(new UISelStateHandler());
					sseF_0_0_0_0_0.SetSelStateHandler(new UISelStateHandler());
					sseF_0_0_0_1.SetSelStateHandler(new UISelStateHandler());
					sseD_0_1.SetSelStateHandler(new UISelStateHandler());
					sseF_0_1_0.SetSelStateHandler(new UISelStateHandler());
					sseF_0_1_0_0.SetSelStateHandler(new UISelStateHandler());
					sseF_0_1_0_0_0.SetSelStateHandler(new UISelStateHandler());
					sseF_0.UISelStateHandler().MakeSelectable();
					sseF_0_0.UISelStateHandler().MakeSelectable();
					sseF_0_0_0.UISelStateHandler().MakeSelectable();
					sseD_0_0_0_0.UISelStateHandler().MakeUnselectable();
					sseF_0_0_0_0_0.UISelStateHandler().MakeSelectable();
					sseF_0_0_0_1.UISelStateHandler().MakeSelectable();
					sseD_0_1.UISelStateHandler().MakeUnselectable();
					sseF_0_1_0.UISelStateHandler().MakeSelectable();
					sseF_0_1_0_0.UISelStateHandler().MakeSelectable();
					sseF_0_1_0_0_0.UISelStateHandler().MakeSelectable();
					IEnumerable<IUIElement> elements = new IUIElement[0];
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

					Assert.That(sseF_0.IsFocusedInHierarchy(), Is.True);
					Assert.That(sseF_0_0.IsFocusedInHierarchy(), Is.True);
					Assert.That(sseF_0_0_0.IsFocusedInHierarchy(), Is.True);
					Assert.That(sseD_0_0_0_0.IsFocusedInHierarchy(), Is.False);
					Assert.That(sseF_0_0_0_0_0.IsFocusedInHierarchy(), Is.False);
					Assert.That(sseF_0_0_0_1.IsFocusedInHierarchy(), Is.True);
					Assert.That(sseD_0_1.IsFocusedInHierarchy(), Is.False);
					Assert.That(sseF_0_1_0.IsFocusedInHierarchy(), Is.False);
					Assert.That(sseF_0_1_0_0.IsFocusedInHierarchy(), Is.False);
					Assert.That(sseF_0_1_0_0_0.IsFocusedInHierarchy(), Is.False);
				}
				[Test]
				public void IsFocused_InHierarchy_SelfFocusedAndNoParent_ReturnsTrue(){
					TestUIElement testSSE = MakeTestSSEWithStateHandler();
					IUISelStateHandler selStateHandler = Substitute.For<IUISelStateHandler>();
						selStateHandler.IsSelectable().Returns(true);
						testSSE.SetSelStateHandler(selStateHandler);

					Assert.That(testSSE.IsFocusedInHierarchy(), Is.True);
				}
				[Test]
				public void IsFocused_InHierarchy_SelfNotFocused_ReturnsFalse(){
					TestUIElement testSSE = MakeTestSSEWithStateHandler();

					Assert.That(testSSE.IsFocusedInHierarchy(), Is.False);
				}
				[Test]
				public void IsFocused_InHierarchy_SelfFocusedParentNotFocused_ReturnsFalse(){
					TestUIElement sse = MakeTestSSE();
						IUISelStateHandler sseSelStateHandler = Substitute.For<IUISelStateHandler>();
						sseSelStateHandler.IsSelectable().Returns(true);
						sse.SetSelStateHandler(sseSelStateHandler);
					TestUIElement stubPar = MakeTestSSE();
						IUISelStateHandler parSelStateHandler = Substitute.For<IUISelStateHandler>();
						parSelStateHandler.IsSelectable().Returns(false);
						stubPar.SetSelStateHandler(parSelStateHandler);
					sse.SetParent(stubPar);
					
					Assert.That(sse.IsFocusedInHierarchy(), Is.False);
				}
				[Test]
				public void IsFocused_InHierarchy_SelfAndAllParentsFocused_ReturnsTrue(){
					TestUIElement stubPar1 = MakeTestSSE();
						IUISelStateHandler par1SelStateHandler = Substitute.For<IUISelStateHandler>();
						par1SelStateHandler.IsSelectable().Returns(true);
						stubPar1.SetSelStateHandler(par1SelStateHandler);
					TestUIElement stubPar2 = MakeTestSSE();
						IUISelStateHandler par2SelStateHandler = Substitute.For<IUISelStateHandler>();
						par2SelStateHandler.IsSelectable().Returns(true);
						stubPar2.SetSelStateHandler(par2SelStateHandler);
					TestUIElement stubPar3 = MakeTestSSE();
						IUISelStateHandler par3SelStateHandler = Substitute.For<IUISelStateHandler>();
						par3SelStateHandler.IsSelectable().Returns(true);
						stubPar3.SetSelStateHandler(par3SelStateHandler);
					TestUIElement stubPar4 = MakeTestSSE();
						IUISelStateHandler par4SelStateHandler = Substitute.For<IUISelStateHandler>();
						par4SelStateHandler.IsSelectable().Returns(true);
						stubPar4.SetSelStateHandler(par4SelStateHandler);
					TestUIElement testSSE = MakeTestSSE();
						IUISelStateHandler sseSelStateHandler = Substitute.For<IUISelStateHandler>();
						sseSelStateHandler.IsSelectable().Returns(true);
						testSSE.SetSelStateHandler(sseSelStateHandler);
					stubPar2.SetParent(stubPar1);
					stubPar3.SetParent(stubPar2);
					stubPar4.SetParent(stubPar3);
					testSSE.SetParent(stubPar4);
					
					Assert.That(testSSE.IsFocusedInHierarchy(), Is.True);
				}
				[Test]
				public void IsFocused_InHierarchy_SomeNonfocusedAncester_ReturnsFalse(){
					TestUIElement stubPar1 = MakeTestSSE();
						IUISelStateHandler par1SelStateHandler = Substitute.For<IUISelStateHandler>();
						par1SelStateHandler.IsSelectable().Returns(true);
						stubPar1.SetSelStateHandler(par1SelStateHandler);
					TestUIElement stubPar2 = MakeTestSSE();
						IUISelStateHandler par2SelStateHandler = Substitute.For<IUISelStateHandler>();
						par2SelStateHandler.IsSelectable().Returns(true);
						stubPar2.SetSelStateHandler(par2SelStateHandler);
					TestUIElement stubPar3 = MakeTestSSE();
						IUISelStateHandler par3SelStateHandler = Substitute.For<IUISelStateHandler>();
						par3SelStateHandler.IsSelectable().Returns(false);
						stubPar3.SetSelStateHandler(par3SelStateHandler);
					TestUIElement stubPar4 = MakeTestSSE();
						IUISelStateHandler par4SelStateHandler = Substitute.For<IUISelStateHandler>();
						par4SelStateHandler.IsSelectable().Returns(true);
						stubPar4.SetSelStateHandler(par4SelStateHandler);
					TestUIElement testSSE = MakeTestSSE();
						IUISelStateHandler sseSelStateHandler = Substitute.For<IUISelStateHandler>();
						sseSelStateHandler.IsSelectable().Returns(true);
						testSSE.SetSelStateHandler(sseSelStateHandler);
					stubPar1.SetElements(new IUIElement[]{stubPar2});
					stubPar2.SetElements(new IUIElement[]{stubPar3});
					stubPar2.SetParent(stubPar1);
					stubPar3.SetElements(new IUIElement[]{testSSE});
					stubPar3.SetParent(stubPar2);
					testSSE.SetParent(stubPar3);
					
					Assert.That(testSSE.IsFocusedInHierarchy(), Is.False);
				}
			/*	Methods	*/
				[Test]
				public void SetHierarchy_HasTransformChildrenWithSSE_SetsThemElements(){
					TestUIElement sse = MakeTestSSE();
						TestUIElement childA = MakeTestSSE();
							childA.transform.SetParent(sse.transform);
						TestUIElement childB = MakeTestSSE();
							childB.transform.SetParent(sse.transform);
						TestUIElement childC = MakeTestSSE();
							childC.transform.SetParent(sse.transform);
						IEnumerable<IUIElement> expectedEles = new IUIElement[]{childA, childB, childC};
					
					sse.SetHierarchy();

					bool equality = sse.MemberEquals(expectedEles);
					Assert.That(equality, Is.True);
				}
				[Test]
				public void SetHierarchy_HasTransformChildrenWithSSE_SetsTheirParentThis(){
					TestUIElement sse = MakeTestSSE();
						TestUIElement childA = MakeTestSSE();
							childA.transform.SetParent(sse.transform);
						TestUIElement childB = MakeTestSSE();
							childB.transform.SetParent(sse.transform);
						TestUIElement childC = MakeTestSSE();
							childC.transform.SetParent(sse.transform);
					
					sse.SetHierarchy();

					Assert.That(childA.GetParent(), Is.SameAs(sse));
					Assert.That(childB.GetParent(), Is.SameAs(sse));
					Assert.That(childC.GetParent(), Is.SameAs(sse));
				}
				[Test]
				public void SetHierarchy_HasTransformChildrenWithoutSSE_SetsElementsEmpty(){
					TestUIElement sse = MakeTestSSE();
						GameObject childA = MakeGO();
						GameObject childB = MakeGO();
						GameObject childC = MakeGO();
					
					sse.SetHierarchy();

					Assert.That(sse, Is.Empty);

					GameObject.DestroyImmediate(childA);
					GameObject.DestroyImmediate(childB);
					GameObject.DestroyImmediate(childC);
				}
				public GameObject MakeGO(){
					GameObject go = new GameObject("go");
					go.tag = "TestGO";
					return go;
				}
				[Test]
				public void ContainsInHierarchy_Various_ReturnsAccordingly(){
					TestUIElement sse_0 = MakeTestSSE();
						TestUIElement sse_0_0 = MakeTestSSE();
							TestUIElement sse_0_0_0 = MakeTestSSE();
								TestUIElement sse_0_0_0_0 = MakeTestSSE();
									TestUIElement sse_0_0_0_0_0 = MakeTestSSE();
								TestUIElement sse_0_0_0_1 = MakeTestSSE();
						TestUIElement sse_0_1 = MakeTestSSE();
							TestUIElement sse_0_1_0 = MakeTestSSE();
								TestUIElement sse_0_1_0_0 = MakeTestSSE();
									TestUIElement sse_0_1_0_0_0 = MakeTestSSE();
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
					TestUIElement testSSE = MakeTestSSE();

					testSSE.ContainsInHierarchy(null);
				}
				[Test]
				public void ContainsInHierarchy_Self_ReturnsFalse(){
					TestUIElement testSSE = MakeTestSSE();

					bool result = testSSE.ContainsInHierarchy(testSSE);

					Assert.That(result, Is.False);
				}
				[Test]
				public void ContainsInHierarchy_Parent_ReturnsFalse(){
					TestUIElement testSSE = MakeTestSSE();
					TestUIElement stubPar = MakeTestSSE();
					testSSE.SetParent(stubPar);

					bool result = testSSE.ContainsInHierarchy(stubPar);

					Assert.That(result, Is.False);
				}
				[Test]
				public void ContainsInHierarchy_DirectChild_ReturnsTrue(){
					TestUIElement testSSE = MakeTestSSE();
					TestUIElement stubChild = MakeTestSSE();
					stubChild.SetParent(testSSE);

					bool result = testSSE.ContainsInHierarchy(stubChild);

					Assert.That(result, Is.True);
				}
				[Test]
				public void ContainsInHierarchy_DistantChild_ReturnsTrue(){
					TestUIElement testSSE = MakeTestSSE();
					TestUIElement stubChild = MakeTestSSE();
					TestUIElement stubGrandChild = MakeTestSSE();
					stubGrandChild.SetParent(stubChild);
					stubChild.SetParent(testSSE);

					bool result = testSSE.ContainsInHierarchy(stubGrandChild);

					Assert.That(result, Is.True);
				}
				[Test]
				public void PerformInHierarchyVer1_Various_PerformsAccordingly(){
					TestUIElement sse_0 = MakeTestSSE();
						TestUIElement sse_0_0 = MakeTestSSE();
							TestUIElement sse_0_0_0 = MakeTestSSE();
								TestUIElement sse_0_0_0_0 = MakeTestSSE();
									TestUIElement sse_0_0_0_0_0 = MakeTestSSE();
								TestUIElement sse_0_0_0_1 = MakeTestSSE();
						TestUIElement sse_0_1 = MakeTestSSE();
							TestUIElement sse_0_1_0 = MakeTestSSE();
								TestUIElement sse_0_1_0_0 = MakeTestSSE();
									TestUIElement sse_0_1_0_0_0 = MakeTestSSE();
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
					sse_0.SetElements(new IUIElement[]{sse_0_0, sse_0_1});
						sse_0_0.SetElements(new IUIElement[]{sse_0_0_0});
							sse_0_0_0.SetElements(new IUIElement[]{sse_0_0_0_0, sse_0_0_0_1});
								sse_0_0_0_0.SetElements(new IUIElement[]{sse_0_0_0_0_0});
									sse_0_0_0_0_0.SetElements(new IUIElement[]{});
								sse_0_0_0_1.SetElements(new IUIElement[]{});
						sse_0_1.SetElements(new IUIElement[]{sse_0_1_0});
							sse_0_1_0.SetElements(new IUIElement[]{sse_0_1_0_0});
								sse_0_1_0_0.SetElements(new IUIElement[]{sse_0_1_0_0_0});
									sse_0_1_0_0_0.SetElements(new IUIElement[]{});
					IEnumerable<IUIElement> down_0 = new IUIElement[]{
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
					IEnumerable<IUIElement> down_0_0 = new IUIElement[]{
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<IUIElement> down_0_0_0 = new IUIElement[]{
						sse_0_0_0,
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<IUIElement> down_0_0_0_0 = new IUIElement[]{
						sse_0_0_0_0, 
						sse_0_0_0_0_0
					};
					IEnumerable<IUIElement> down_0_1 = new IUIElement[]{
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<IUIElement> down_0_1_0 = new IUIElement[]{
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<IUIElement> down_0_1_0_0 = new IUIElement[]{
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					sse_0.PerformInHierarchy(SetHi);
						foreach(TestUIElement e in down_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0.PerformInHierarchy(SetHi);
						foreach(TestUIElement e in down_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(!new List<IUIElement>(down_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0.PerformInHierarchy(SetHi);
						foreach(TestUIElement e in down_0_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(!new List<IUIElement>(down_0_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0_0.PerformInHierarchy(SetHi);
						foreach(TestUIElement e in down_0_0_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(!new List<IUIElement>(down_0_0_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0_0_0.PerformInHierarchy(SetHi);
						Assert.That(sse_0_0_0_0_0.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(e != sse_0_0_0_0_0)
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1.PerformInHierarchy(SetHi);
						foreach(TestUIElement e in down_0_1)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(!new List<IUIElement>(down_0_1).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0.PerformInHierarchy(SetHi);
						foreach(TestUIElement e in down_0_1_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(!new List<IUIElement>(down_0_1_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0_0.PerformInHierarchy(SetHi);
						foreach(TestUIElement e in down_0_1_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(!new List<IUIElement>(down_0_1_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0_0_0.PerformInHierarchy(SetHi);
						Assert.That(sse_0_1_0_0_0.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(e != sse_0_1_0_0_0)
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					}
					void SetHi(IUIElement ele){
						TestUIElement testEle = (TestUIElement)ele;
						testEle.message = "Hi";
					}
					void ClearHi(IEnumerable<IUIElement> eles){
						foreach(IUIElement ele in eles){
							TestUIElement testEle = (TestUIElement)ele;
							testEle.message = "";
						}
				}
				[Test]
				public void PerformInHierarchyVer2_Various_PerformsAccordingly(){
					TestUIElement sse_0 = MakeTestSSE();
						TestUIElement sse_0_0 = MakeTestSSE();
							TestUIElement sse_0_0_0 = MakeTestSSE();
								TestUIElement sse_0_0_0_0 = MakeTestSSE();
									TestUIElement sse_0_0_0_0_0 = MakeTestSSE();
								TestUIElement sse_0_0_0_1 = MakeTestSSE();
						TestUIElement sse_0_1 = MakeTestSSE();
							TestUIElement sse_0_1_0 = MakeTestSSE();
								TestUIElement sse_0_1_0_0 = MakeTestSSE();
									TestUIElement sse_0_1_0_0_0 = MakeTestSSE();
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
					sse_0.SetElements(new IUIElement[]{sse_0_0, sse_0_1});
						sse_0_0.SetElements(new IUIElement[]{sse_0_0_0});
							sse_0_0_0.SetElements(new IUIElement[]{sse_0_0_0_0, sse_0_0_0_1});
								sse_0_0_0_0.SetElements(new IUIElement[]{sse_0_0_0_0_0});
									sse_0_0_0_0_0.SetElements(new IUIElement[]{});
								sse_0_0_0_1.SetElements(new IUIElement[]{});
						sse_0_1.SetElements(new IUIElement[]{sse_0_1_0});
							sse_0_1_0.SetElements(new IUIElement[]{sse_0_1_0_0});
								sse_0_1_0_0.SetElements(new IUIElement[]{sse_0_1_0_0_0});
									sse_0_1_0_0_0.SetElements(new IUIElement[]{});
					IEnumerable<IUIElement> down_0 = new IUIElement[]{
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
					IEnumerable<IUIElement> down_0_0 = new IUIElement[]{
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<IUIElement> down_0_0_0 = new IUIElement[]{
						sse_0_0_0,
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<IUIElement> down_0_0_0_0 = new IUIElement[]{
						sse_0_0_0_0, 
						sse_0_0_0_0_0
					};
					IEnumerable<IUIElement> down_0_1 = new IUIElement[]{
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<IUIElement> down_0_1_0 = new IUIElement[]{
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<IUIElement> down_0_1_0_0 = new IUIElement[]{
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					sse_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestUIElement e in down_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestUIElement e in down_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(!new List<IUIElement>(down_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestUIElement e in down_0_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(!new List<IUIElement>(down_0_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestUIElement e in down_0_0_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(!new List<IUIElement>(down_0_0_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_0_0_0_0.PerformInHierarchy(SetHi, "Hi");
						Assert.That(sse_0_0_0_0_0.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(e != sse_0_0_0_0_0)
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1.PerformInHierarchy(SetHi, "Hi");
						foreach(TestUIElement e in down_0_1)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(!new List<IUIElement>(down_0_1).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestUIElement e in down_0_1_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(!new List<IUIElement>(down_0_1_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0_0.PerformInHierarchy(SetHi, "Hi");
						foreach(TestUIElement e in down_0_1_0_0)
							Assert.That(e.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(!new List<IUIElement>(down_0_1_0_0).Contains(e))
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					sse_0_1_0_0_0.PerformInHierarchy(SetHi, "Hi");
						Assert.That(sse_0_1_0_0_0.message, Is.StringContaining("Hi"));
						foreach(TestUIElement e in down_0)
							if(e != sse_0_1_0_0_0)
								Assert.That(e.message, Is.Not.StringContaining("Hi"));
						ClearHi(down_0);
					}
					void SetHi(IUIElement ele, object str){
						string s = (string)str;
						((TestUIElement)ele).message = s;
					}
				[Test]
				public void PerformInHierarchyVer3_Various_PerformsAccordingly(){
					TestUIElement sse_0 = MakeTestSSE();
						TestUIElement sse_0_0 = MakeTestSSE();
							TestUIElement sse_0_0_0 = MakeTestSSE();
								TestUIElement sse_0_0_0_0 = MakeTestSSE();
									TestUIElement sse_0_0_0_0_0 = MakeTestSSE();
								TestUIElement sse_0_0_0_1 = MakeTestSSE();
						TestUIElement sse_0_1 = MakeTestSSE();
							TestUIElement sse_0_1_0 = MakeTestSSE();
								TestUIElement sse_0_1_0_0 = MakeTestSSE();
									TestUIElement sse_0_1_0_0_0 = MakeTestSSE();
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
					sse_0.SetElements(new IUIElement[]{sse_0_0, sse_0_1});
						sse_0_0.SetElements(new IUIElement[]{sse_0_0_0});
							sse_0_0_0.SetElements(new IUIElement[]{sse_0_0_0_0, sse_0_0_0_1});
								sse_0_0_0_0.SetElements(new IUIElement[]{sse_0_0_0_0_0});
									sse_0_0_0_0_0.SetElements(new IUIElement[]{});
								sse_0_0_0_1.SetElements(new IUIElement[]{});
						sse_0_1.SetElements(new IUIElement[]{sse_0_1_0});
							sse_0_1_0.SetElements(new IUIElement[]{sse_0_1_0_0});
								sse_0_1_0_0.SetElements(new IUIElement[]{sse_0_1_0_0_0});
									sse_0_1_0_0_0.SetElements(new IUIElement[]{});
					IEnumerable<IUIElement> down_0 = new IUIElement[]{
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
					IEnumerable<IUIElement> down_0_0 = new IUIElement[]{
						sse_0_0, 
						sse_0_0_0, 
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<IUIElement> down_0_0_0 = new IUIElement[]{
						sse_0_0_0,
						sse_0_0_0_0, 
						sse_0_0_0_0_0, 
						sse_0_0_0_1
					};
					IEnumerable<IUIElement> down_0_0_0_0 = new IUIElement[]{
						sse_0_0_0_0, 
						sse_0_0_0_0_0
					};
					IEnumerable<IUIElement> down_0_1 = new IUIElement[]{
						sse_0_1, 
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<IUIElement> down_0_1_0 = new IUIElement[]{
						sse_0_1_0, 
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					IEnumerable<IUIElement> down_0_1_0_0 = new IUIElement[]{
						sse_0_1_0_0, 
						sse_0_1_0_0_0
					};
					List<IUIElement> down_0List = new List<IUIElement>();
						sse_0.PerformInHierarchy(PutInList, down_0List);
						Assert.That(down_0List.MemberEquals(down_0), Is.True);
					List<IUIElement> down_0_0List = new List<IUIElement>();
						sse_0_0.PerformInHierarchy(PutInList, down_0_0List);
						Assert.That(down_0_0List.MemberEquals(down_0_0), Is.True);
					List<IUIElement> down_0_0_0List = new List<IUIElement>();
						sse_0_0_0.PerformInHierarchy(PutInList, down_0_0_0List);
						Assert.That(down_0_0_0List.MemberEquals(down_0_0_0), Is.True);
					List<IUIElement> down_0_0_0_0List = new List<IUIElement>();
						sse_0_0_0_0.PerformInHierarchy(PutInList, down_0_0_0_0List);
						Assert.That(down_0_0_0_0List.MemberEquals(down_0_0_0_0), Is.True);
					List<IUIElement> down_0_0_0_0_0List = new List<IUIElement>();
						sse_0_0_0_0_0.PerformInHierarchy(PutInList, down_0_0_0_0_0List);
						Assert.That(down_0_0_0_0_0List.MemberEquals(new IUIElement[]{sse_0_0_0_0_0}), Is.True);
					List<IUIElement> down_0_1List = new List<IUIElement>();
						sse_0_1.PerformInHierarchy(PutInList, down_0_1List);
						Assert.That(down_0_1List.MemberEquals(down_0_1), Is.True);
					List<IUIElement> down_0_1_0List = new List<IUIElement>();
						sse_0_1_0.PerformInHierarchy(PutInList, down_0_1_0List);
						Assert.That(down_0_1_0List.MemberEquals(down_0_1_0), Is.True);
					List<IUIElement> down_0_1_0_0List = new List<IUIElement>();
						sse_0_1_0_0.PerformInHierarchy(PutInList, down_0_1_0_0List);
						Assert.That(down_0_1_0_0List.MemberEquals(down_0_1_0_0), Is.True);
					List<IUIElement> down_0_1_0_0_0List = new List<IUIElement>();
						sse_0_1_0_0_0.PerformInHierarchy(PutInList, down_0_1_0_0_0List);
						Assert.That(down_0_1_0_0_0List.MemberEquals(new IUIElement[]{sse_0_1_0_0_0}), Is.True);
					}
					void PutInList<ISlotSystemElement>(ISlotSystemElement ele, IList<ISlotSystemElement> list){
						list.Add(ele);
					}
				[Test]
				public void PerformInHierarchyVer1_WhenCalled_PerformActionOnElements(){
					TestUIElement testSSE = MakeTestSSE();
					TestUIElement mockChild_1 = MakeTestSSE();
					TestUIElement mockChild_1_1 = MakeTestSSE();
					TestUIElement mockChild_1_2 = MakeTestSSE();
					TestUIElement mockChild_2 = MakeTestSSE();
					TestUIElement mockChild_2_1 = MakeTestSSE();
					TestUIElement mockChild_2_2 = MakeTestSSE();
					TestUIElement mockPar = MakeTestSSE();
					mockChild_1.SetElements(new IUIElement[]{mockChild_1_1, mockChild_1_2});
					mockChild_2.SetElements(new IUIElement[]{mockChild_2_1, mockChild_2_2});
					testSSE.SetElements(new IUIElement[]{mockChild_1, mockChild_2});
					mockPar.SetElements(new IUIElement[]{testSSE});

					testSSE.PerformInHierarchy(x => ((TestUIElement)x).message = "performed");

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
					TestUIElement testSSE = MakeTestSSE();
					TestUIElement mockChild_1 = MakeTestSSE();
					TestUIElement mockChild_1_1 = MakeTestSSE();
					TestUIElement mockChild_1_2 = MakeTestSSE();
					TestUIElement mockChild_2 = MakeTestSSE();
					TestUIElement mockChild_2_1 = MakeTestSSE();
					TestUIElement mockChild_2_2 = MakeTestSSE();
					TestUIElement mockPar = MakeTestSSE();
					mockChild_1.SetElements(new IUIElement[]{mockChild_1_1, mockChild_1_2});
					mockChild_2.SetElements(new IUIElement[]{mockChild_2_1, mockChild_2_2});
					testSSE.SetElements(new IUIElement[]{mockChild_1, mockChild_2});
					mockPar.SetElements(new IUIElement[]{testSSE});

					System.Action<IUIElement, object> act = (IUIElement x, object s) => ((TestUIElement)x).message = (string)s;
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
					TestUIElement testSSE = MakeTestSSE();
					TestUIElement mockChild_1 = MakeTestSSE();
					TestUIElement mockChild_1_1 = MakeTestSSE();
					TestUIElement mockChild_1_2 = MakeTestSSE();
					TestUIElement mockChild_2 = MakeTestSSE();
					TestUIElement mockChild_2_1 = MakeTestSSE();
					TestUIElement mockChild_2_2 = MakeTestSSE();
					TestUIElement mockPar = MakeTestSSE();
					mockChild_1.SetElements(new IUIElement[]{mockChild_1_1, mockChild_1_2});
					mockChild_2.SetElements(new IUIElement[]{mockChild_2_1, mockChild_2_2});
					testSSE.SetElements(new IUIElement[]{mockChild_1, mockChild_2});
					mockPar.SetElements(new IUIElement[]{testSSE});

					System.Action<IUIElement, IList<string>> act = (IUIElement x, IList<string> list) => {
						string concat = "";
						foreach(string s in list){
							concat += s;
						}
						((TestUIElement)x).message = concat;
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
					TestUIElement testSSE = MakeTestSSE();
					IUIElement stubEle = Substitute.For<IUIElement>();
					
					Assert.That(testSSE.Contains(stubEle), Is.False);
					}
				[Test]
				public void Contains_NonMember_ReturnsFalse(){
					TestUIElement testSSE = MakeTestSSE();
					IUIElement stubMember = Substitute.For<IUIElement>();
					IUIElement stubNonMember = Substitute.For<IUIElement>();
					testSSE.SetElements(new IUIElement[]{stubMember});
					
					Assert.That(testSSE.Contains(stubNonMember), Is.False);
					}
				[Test]
				public void Contains_Member_ReturnsTrue(){
					TestUIElement testSSE = MakeTestSSE();
					IUIElement stubMember = Substitute.For<IUIElement>();
					testSSE.SetElements(new IUIElement[]{stubMember});
					
					Assert.That(testSSE.Contains(stubMember), Is.True);
					}

			/* Other */
				[TestCaseSource(typeof(isFocusableInHierarchyCases))]
				public void isFocusableInHierarchy_AllAncestorsContainedInBundleFocusedElement_ReturnsTrue(TestUIElement top, IEnumerable<IUIElement> xOn, IEnumerable<IUIElement> xOff){
					foreach(var e in xOn)
						Assert.That(e.IsFocusableInHierarchy(), Is.True);
					foreach(var e in xOff)
						Assert.That(e.IsFocusableInHierarchy(), Is.False);
				}
				class isFocusableInHierarchyCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] case0;
							TestUIElement top_0 = MakeTestSSE();
								UIBundle bun0_0 = MakeSSBundle();
									TestUIElement sse00_0 = MakeTestSSE();
									TestUIElement sse01_0 = MakeTestSSE();//foc
									TestUIElement sse02_0 = MakeTestSSE();
									UIBundle bun03_0 = MakeSSBundle();
										TestUIElement sse030_0 = MakeTestSSE();//foc
										TestUIElement sse031_0 = MakeTestSSE();
								UIBundle bun1_0 = MakeSSBundle();
									UIBundle bun10_0 = MakeSSBundle();
										TestUIElement sse100_0 = MakeTestSSE();//foc
										TestUIElement sse101_0 = MakeTestSSE();
									UIBundle bun11_0 = MakeSSBundle();//foc
										UIBundle bun110_0 = MakeSSBundle();//foc
											TestUIElement sse1100_0 = MakeTestSSE();//foc
											TestUIElement sse1101_0 = MakeTestSSE();
										UIBundle bun111_0 = MakeSSBundle();
										UIBundle bun112_0 = MakeSSBundle();
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

							IEnumerable<IUIElement> xOn_0 = new IUIElement[]{
								top_0,
								bun0_0,
								sse01_0,
								bun1_0,
								bun11_0,
								bun110_0,
								sse1100_0
							};
							IEnumerable<IUIElement> xOff_0 = new IUIElement[]{
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
							TestUIElement top_1 = MakeTestSSE();
								UIBundle bun0_1 = MakeSSBundle();
									TestUIElement sse00_1 = MakeTestSSE();
									TestUIElement sse01_1 = MakeTestSSE();
									TestUIElement sse02_1 = MakeTestSSE();
									UIBundle bun03_1 = MakeSSBundle();//foc
										TestUIElement sse030_1 = MakeTestSSE();//foc
										TestUIElement sse031_1 = MakeTestSSE();
								UIBundle bun1_1 = MakeSSBundle();
									UIBundle bun10_1 = MakeSSBundle();//foc
										TestUIElement sse100_1 = MakeTestSSE();//foc
										TestUIElement sse101_1 = MakeTestSSE();
									UIBundle bun11_1 = MakeSSBundle();
										UIBundle bun110_1 = MakeSSBundle();//foc
											TestUIElement sse1100_1 = MakeTestSSE();//foc
											TestUIElement sse1101_1 = MakeTestSSE();
										UIBundle bun111_1 = MakeSSBundle();
										UIBundle bun112_1 = MakeSSBundle();
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

							IEnumerable<IUIElement> xOn_1 = new IUIElement[]{
								top_1,
								bun0_1,
								bun03_1,
								sse030_1,
								bun1_1,
								bun10_1,
								sse100_1
							};
							IEnumerable<IUIElement> xOff_1 = new IUIElement[]{
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
				static TestUIElement MakeTestSSEWithStateHandler(){
					TestUIElement testSSE = MakeTestSSE();
					UISelStateHandler handler = new UISelStateHandler();
					testSSE.SetSelStateHandler(handler);
					return testSSE;
				}
				static UIBundle MakeSSBundleWithStateHandler(){
					UIBundle bundle = MakeSSBundle();
					UISelStateHandler handler = new UISelStateHandler();
					bundle.SetSelStateHandler(handler);
					return bundle;
				}
				[TestCaseSource(typeof(ActivateRecursivelyCases))]
				public void ActivateRecursively_WhenCalled_FocusIfIsAOFAndFocusable_DefocusIfIsAODAndNotFocusable(TestUIElement top, IEnumerable<IUIElement> xFocused, IEnumerable<IUIElement> xDefocused, IEnumerable<IUIElement> xDeactivated){

					top.ActivateRecursively();

					foreach(var e in xFocused)
						Assert.That(e.UISelStateHandler().IsSelectable(), Is.True);
					foreach(var e in xDefocused)
						Assert.That(e.UISelStateHandler().IsUnselectable(), Is.True);
					foreach(var e in xDeactivated)
						Assert.That(e.UISelStateHandler().IsDeactivated(), Is.True);
				}
					class ActivateRecursivelyCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case0;
								TestUIElement top_0 = MakeTestSSEWithStateHandler();
									UIBundle bun0_0 = MakeSSBundleWithStateHandler();
										TestUIElement sse00_0 = MakeTestSSEWithStateHandler();
											UIBundle bun000_0 = MakeSSBundleWithStateHandler();
												TestUIElement sse0000_0 = MakeTestSSEWithStateHandler();
												TestUIElement sse0001_0 = MakeTestSSEWithStateHandler();
											TestUIElement sse001_0 = MakeTestSSEWithStateHandler();
												TestUIElement sse0010_0 = MakeTestSSEWithStateHandler();
												TestUIElement sse0011_0 = MakeTestSSEWithStateHandler();
										TestUIElement sse01_0 = MakeTestSSEWithStateHandler();
										TestUIElement sse02_0 = MakeTestSSEWithStateHandler();
									UIBundle bun1_0 = MakeSSBundleWithStateHandler();
										UIBundle bun10_0 = MakeSSBundleWithStateHandler();
											TestUIElement sse100_0 = MakeTestSSEWithStateHandler();
											TestUIElement sse101_0 = MakeTestSSEWithStateHandler();
												UIBundle bun1010_0 = MakeSSBundleWithStateHandler();
													TestUIElement sse10100_0 = MakeTestSSEWithStateHandler();
													TestUIElement sse10101_0 = MakeTestSSEWithStateHandler();
										UIBundle bun11_0 = MakeSSBundleWithStateHandler();
											TestUIElement sse110_0 = MakeTestSSEWithStateHandler();
											TestUIElement sse111_0 = MakeTestSSEWithStateHandler();
									UIBundle bun2_0 = MakeSSBundleWithStateHandler();
										UIBundle bun20_0 = MakeSSBundleWithStateHandler();
										UIBundle bun21_0 = MakeSSBundleWithStateHandler();
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

								sse001_0.SetIsActivatedOnDefault(false);
								bun11_0.SetIsActivatedOnDefault(false);
								bun2_0.SetIsActivatedOnDefault(false);

								IEnumerable<IUIElement> xFocused_0 = new IUIElement[]{
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
								IEnumerable<IUIElement> xDefocused_0 = new IUIElement[]{
									sse0001_0,
									sse01_0,
									sse02_0,
									sse100_0,
									sse10100_0
								};
								IEnumerable<IUIElement> xDeactivated_0 = new IUIElement[]{
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
								TestUIElement top_1 = MakeTestSSEWithStateHandler();
									UIBundle bun0_1 = MakeSSBundleWithStateHandler();
										TestUIElement sse00_1 = MakeTestSSEWithStateHandler();
											UIBundle bun000_1 = MakeSSBundleWithStateHandler();
												TestUIElement sse0000_1 = MakeTestSSEWithStateHandler();
												TestUIElement sse0001_1 = MakeTestSSEWithStateHandler();
											TestUIElement sse001_1 = MakeTestSSEWithStateHandler();
												TestUIElement sse0010_1 = MakeTestSSEWithStateHandler();
												TestUIElement sse0011_1 = MakeTestSSEWithStateHandler();
										TestUIElement sse01_1 = MakeTestSSEWithStateHandler();
										TestUIElement sse02_1 = MakeTestSSEWithStateHandler();
									UIBundle bun1_1 = MakeSSBundleWithStateHandler();
										UIBundle bun10_1 = MakeSSBundleWithStateHandler();
											TestUIElement sse100_1 = MakeTestSSEWithStateHandler();
											TestUIElement sse101_1 = MakeTestSSEWithStateHandler();
												UIBundle bun1010_1 = MakeSSBundleWithStateHandler();
													TestUIElement sse10100_1 = MakeTestSSEWithStateHandler();
													TestUIElement sse10101_1 = MakeTestSSEWithStateHandler();
										UIBundle bun11_1 = MakeSSBundleWithStateHandler();
											TestUIElement sse110_1 = MakeTestSSEWithStateHandler();
											TestUIElement sse111_1 = MakeTestSSEWithStateHandler();
									UIBundle bun2_1 = MakeSSBundleWithStateHandler();
										UIBundle bun20_1 = MakeSSBundleWithStateHandler();
										UIBundle bun21_1 = MakeSSBundleWithStateHandler();
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

								bun0_1.SetIsActivatedOnDefault(false);
								bun11_1.SetIsActivatedOnDefault(false);
								bun2_1.SetIsActivatedOnDefault(false);

								IEnumerable<IUIElement> xFocused_1 = new IUIElement[]{
									top_1,
									bun1_1,
									bun10_1,
									sse101_1,
									bun1010_1,
									sse10101_1
								};
								IEnumerable<IUIElement> xDefocused_1 = new IUIElement[]{
									sse100_1,
									sse10100_1
								};
								IEnumerable<IUIElement> xDeactivated_1 = new IUIElement[]{
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
					TestUIElement sse = MakeTestSSE();

					Assert.That(sse.IsActivatedOnDefault(), Is.True);
				}
				[TestCaseSource(typeof(isActivatedOnDefaultCases))]
				public void isActivatedOnDefault_Always_ReturnsAccordingly(TestUIElement sse, IEnumerable<IUIElement> xOn, IEnumerable<IUIElement> xOff){
					sse.SetHierarchyRecursively();

					sse.RecursiveTestMethod();
					
					foreach(IUIElement ele in xOn)
						Assert.That(ele.UISelStateHandler().IsSelectable(), Is.True);
					foreach(IUIElement ele in xOff)
						Assert.That(ele.UISelStateHandler().IsSelectable(), Is.False);
					}
					class isActivatedOnDefaultCases: IEnumerable{
						public IEnumerator GetEnumerator(){
							object[] case0;
								TestUIElement sse_0 = MakeTestSSEWithStateHandler();
									TestUIElement sse0_0 = MakeTestSSEWithStateHandler();//off
										TestUIElement sse00_0 = MakeTestSSEWithStateHandler();
										TestUIElement sse01_0 = MakeTestSSEWithStateHandler();
									TestUIElement sse1_0 = MakeTestSSEWithStateHandler();
										TestUIElement sse10_0 = MakeTestSSEWithStateHandler();
											TestUIElement sse100_0 = MakeTestSSEWithStateHandler();//off
												TestUIElement sse1000_0 = MakeTestSSEWithStateHandler();
											TestUIElement sse101_0 = MakeTestSSEWithStateHandler();
										TestUIElement sse11_0 = MakeTestSSEWithStateHandler();
										sse0_0.transform.SetParent(sse_0.transform);
											sse0_0.SetIsActivatedOnDefault(false);
										sse00_0.transform.SetParent(sse0_0.transform);
										sse01_0.transform.SetParent(sse0_0.transform);
										sse1_0.transform.SetParent(sse_0.transform);
										sse10_0.transform.SetParent(sse1_0.transform);
										sse100_0.transform.SetParent(sse10_0.transform);
											sse100_0.SetIsActivatedOnDefault(false);
										sse1000_0.transform.SetParent(sse100_0.transform);
										sse101_0.transform.SetParent(sse10_0.transform);
										sse11_0.transform.SetParent(sse1_0.transform);
								IEnumerable<IUIElement> xOn_0 = new IUIElement[]{
									sse_0,
									sse1_0,
									sse10_0,
									sse101_0,
									sse11_0
								};
								IEnumerable<IUIElement> xOff_0 = new IUIElement[]{
									sse0_0,
									sse00_0,
									sse01_0,
									sse100_0,
									sse1000_0,
								};
								case0 = new object[]{sse_0, xOn_0, xOff_0};
								yield return case0;
							object[] case1;
								TestUIElement sse_1 = MakeTestSSEWithStateHandler();//off
									TestUIElement sse0_1 = MakeTestSSEWithStateHandler();
										TestUIElement sse00_1 = MakeTestSSEWithStateHandler();
										TestUIElement sse01_1 = MakeTestSSEWithStateHandler();
									TestUIElement sse1_1 = MakeTestSSEWithStateHandler();
										TestUIElement sse10_1 = MakeTestSSEWithStateHandler();
											TestUIElement sse100_1 = MakeTestSSEWithStateHandler();
												TestUIElement sse1000_1 = MakeTestSSEWithStateHandler();
											TestUIElement sse101_1 = MakeTestSSEWithStateHandler();
										TestUIElement sse11_1 = MakeTestSSEWithStateHandler();
										sse_1.SetIsActivatedOnDefault(false);
										sse0_1.transform.SetParent(sse_1.transform);
										sse00_1.transform.SetParent(sse0_1.transform);
										sse01_1.transform.SetParent(sse0_1.transform);
										sse1_1.transform.SetParent(sse_1.transform);
										sse10_1.transform.SetParent(sse1_1.transform);
										sse100_1.transform.SetParent(sse10_1.transform);
										sse1000_1.transform.SetParent(sse100_1.transform);
										sse101_1.transform.SetParent(sse10_1.transform);
										sse11_1.transform.SetParent(sse1_1.transform);
								IEnumerable<IUIElement> xOn_1 = new IUIElement[]{
								};
								IEnumerable<IUIElement> xOff_1 = new IUIElement[]{
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
								TestUIElement sse_2 = MakeTestSSEWithStateHandler();
									TestUIElement sse0_2 = MakeTestSSEWithStateHandler();
										TestUIElement sse00_2 = MakeTestSSEWithStateHandler();
										TestUIElement sse01_2 = MakeTestSSEWithStateHandler();
									TestUIElement sse1_2 = MakeTestSSEWithStateHandler();
										TestUIElement sse10_2 = MakeTestSSEWithStateHandler();
											TestUIElement sse100_2 = MakeTestSSEWithStateHandler();
												TestUIElement sse1000_2 = MakeTestSSEWithStateHandler();
											TestUIElement sse101_2 = MakeTestSSEWithStateHandler();
										TestUIElement sse11_2 = MakeTestSSEWithStateHandler();
										sse0_2.transform.SetParent(sse_2.transform);
										sse00_2.transform.SetParent(sse0_2.transform);
										sse01_2.transform.SetParent(sse0_2.transform);
										sse1_2.transform.SetParent(sse_2.transform);
										sse10_2.transform.SetParent(sse1_2.transform);
										sse100_2.transform.SetParent(sse10_2.transform);
										sse1000_2.transform.SetParent(sse100_2.transform);
										sse101_2.transform.SetParent(sse10_2.transform);
										sse11_2.transform.SetParent(sse1_2.transform);
								IEnumerable<IUIElement> xOn_2 = new IUIElement[]{
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
								IEnumerable<IUIElement> xOff_2 = new IUIElement[]{
								};
								case2 = new object[]{sse_2, xOn_2, xOff_2};
								yield return case2;
							object[] case3;
								TestUIElement sse_3 = MakeTestSSEWithStateHandler();
									TestUIElement sse0_3 = MakeTestSSEWithStateHandler();
										TestUIElement sse00_3 = MakeTestSSEWithStateHandler();
										TestUIElement sse01_3 = MakeTestSSEWithStateHandler();
									TestUIElement sse1_3 = MakeTestSSEWithStateHandler();//off
										TestUIElement sse10_3 = MakeTestSSEWithStateHandler();//on
											TestUIElement sse100_3 = MakeTestSSEWithStateHandler();//off
												TestUIElement sse1000_3 = MakeTestSSEWithStateHandler();
											TestUIElement sse101_3 = MakeTestSSEWithStateHandler();
										TestUIElement sse11_3 = MakeTestSSEWithStateHandler();
										sse0_3.transform.SetParent(sse_3.transform);
										sse00_3.transform.SetParent(sse0_3.transform);
										sse01_3.transform.SetParent(sse0_3.transform);
										sse1_3.transform.SetParent(sse_3.transform);
											sse1_3.SetIsActivatedOnDefault(false);
										sse10_3.transform.SetParent(sse1_3.transform);
											sse10_3.SetIsActivatedOnDefault(true);
										sse100_3.transform.SetParent(sse10_3.transform);
											sse100_3.SetIsActivatedOnDefault(false);
										sse1000_3.transform.SetParent(sse100_3.transform);
										sse101_3.transform.SetParent(sse10_3.transform);
										sse11_3.transform.SetParent(sse1_3.transform);
								IEnumerable<IUIElement> xOn_3 = new IUIElement[]{
									sse_3,
									sse0_3,
									sse00_3,
									sse01_3
								};
								IEnumerable<IUIElement> xOff_3 = new IUIElement[]{
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
				UIBundle MakeSlotSystemBundle(){
					return new GameObject("ssBundleGO").AddComponent<UIBundle>();
				}
		}
	}
}
