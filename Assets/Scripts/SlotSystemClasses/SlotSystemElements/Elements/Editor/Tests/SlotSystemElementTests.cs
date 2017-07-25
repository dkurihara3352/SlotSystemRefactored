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
			/*	State and process */
				/* States */
					[Test]
					public void isFocused_CurSelStateIsFocused_ReturnsTrue(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						testSSE.Focus();

						Assert.That(testSSE.isFocused, Is.True);
						Assert.That(testSSE.isDefocused, Is.False);
						Assert.That(testSSE.isDeactivated, Is.False);
						}
					[Test]
					public void isDefocused_CurSelStateIsDefocused_ReturnsTrue(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						testSSE.Defocus();

						Assert.That(testSSE.isFocused, Is.False);
						Assert.That(testSSE.isDefocused, Is.True);
						Assert.That(testSSE.isDeactivated, Is.False);
						}
					[Test]
					public void isDeactivated_CurSelStateIsDeactivated_ReturnsTrue(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						testSSE.Deactivate();

						Assert.That(testSSE.isFocused, Is.False);
						Assert.That(testSSE.isDefocused, Is.False);
						Assert.That(testSSE.isDeactivated, Is.True);
						}
					[Test]
					public void Deactivate_WhenCalled_SetsCurSelStateDeactivated(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						
						testSSE.Deactivate();

						Assert.That(testSSE.isDeactivated, Is.True);
						}
					[Test]
					public void Focus_WhenCalled_SetsCurSelStateFocused(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						
						testSSE.Focus();

						Assert.That(testSSE.isFocused, Is.True);
						}
					[Test]
					public void Defocus_WhenCalled_SetCurStateToDefocusd(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						
						testSSE.Defocus();

						Assert.That(testSSE.isDefocused, Is.True);
						}
					[Test]
					public void Select_WhenCalled_SetCurStateToSelected(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						
						testSSE.Select();

						Assert.That(testSSE.isSelected, Is.True);
						}
				/* Process */
					[TestCaseSource(typeof(SetAndRunSelProcess_ISSESelProcessOrNullCases))][Category("State and Process")]
					public void SetAndRunSelProcess_ISSESelProcessOrNull_CallsSelProcEngineSAR(ISSESelProcess process){
						TestSlotSystemElement sse = MakeTestSSE();
							ISSEProcessEngine<ISSESelProcess> engine = Substitute.For<ISSEProcessEngine<ISSESelProcess>>();
							sse.SetSelProcEngine(engine);
						
						sse.SetAndRunSelProcess(process);

						engine.Received().SetAndRunProcess(process);
						}
						class SetAndRunSelProcess_ISSESelProcessOrNullCases: IEnumerable{
							public IEnumerator GetEnumerator(){
								yield return Substitute.For<ISSESelProcess>();
								yield return null;
							}
						}
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
				public void isPageElement_ParentIsPage_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemPage stubPage = Substitute.For<ISlotSystemPage>();
					testSSE.SetParent(stubPage);

					Assert.That(testSSE.isPageElement, Is.True);
					}
				[Test]
				public void isPageElement_ParentIsNotPage_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubSSE = MakeTestSSE();
					testSSE.SetParent(stubSSE);

					Assert.That(testSSE.isPageElement, Is.False);
					}
				[Test]
				public void isToggledOn_ParentNotPage_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubSSE = MakeTestSSE();
					testSSE.SetParent(stubSSE);

					Assert.That(testSSE.isToggledOn, Is.False);
					}
				[Test]
				public void isToggledOn_ParentIsPageAndElementToggledOn_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemPage stubPage = Substitute.For<ISlotSystemPage>();
					stubPage.GetPageElement(testSSE).isFocusToggleOn.Returns(true);
					testSSE.SetParent(stubPage);

					Assert.That(testSSE.isToggledOn, Is.True);
					}
				[Test]
				public void isToggledOn_ParentIsPageAndElementToggledOff_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemPage stubPage = Substitute.For<ISlotSystemPage>();
					stubPage.GetPageElement(testSSE).isFocusToggleOn.Returns(false);
					testSSE.SetParent(stubPage);

					Assert.That(testSSE.isToggledOn, Is.False);
					}
				[Test]
				public void isFocusedInHierarchy_Various_ReturnsAccordingly(){
					TestSlotSystemElement sseF_0 = MakeTestSSE();
						TestSlotSystemElement sseF_0_0 = MakeTestSSE();
							TestSlotSystemElement sseF_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sseD_0_0_0_0 = MakeTestSSE();
									TestSlotSystemElement sseF_0_0_0_0_0 = MakeTestSSE();
								TestSlotSystemElement sseF_0_0_0_1 = MakeTestSSE();
						TestSlotSystemElement sseD_0_1 = MakeTestSSE();
							TestSlotSystemElement sseF_0_1_0 = MakeTestSSE();
								TestSlotSystemElement sseF_0_1_0_0 = MakeTestSSE();
									TestSlotSystemElement sseF_0_1_0_0_0 = MakeTestSSE();
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
					TestSlotSystemElement testSSE = MakeTestSSE();
					testSSE.Focus();

					Assert.That(testSSE.isFocusedInHierarchy, Is.True);
					}
				[Test]
				public void isFocusedInHierarchy_SelfNotFocused_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					Assert.That(testSSE.isFocusedInHierarchy, Is.False);
					}
				[Test]
				public void isFocusedInHierarchy_SelfFocusedParentNotFocused_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubPar = MakeTestSSE();
					testSSE.SetParent(stubPar);
					stubPar.Defocus();
					testSSE.Focus();
					
					Assert.That(testSSE.isFocusedInHierarchy, Is.False);
					}
				[Test]
				public void isFocusedInHierarchy_SelfAndAllParentsFocused_ReturnsTrue(){
					TestSlotSystemElement stubPar1 = MakeTestSSE();
					TestSlotSystemElement stubPar2 = MakeTestSSE();
					TestSlotSystemElement stubPar3 = MakeTestSSE();
					TestSlotSystemElement testSSE = MakeTestSSE();
					testSSE.SetParent(stubPar3);
					stubPar3.SetElements(new ISlotSystemElement[]{testSSE});
					stubPar3.SetParent(stubPar2);
					stubPar2.SetElements(new ISlotSystemElement[]{stubPar3});
					stubPar2.SetParent(stubPar1);
					stubPar1.SetElements(new ISlotSystemElement[]{stubPar2});
					// stubPar1.Focus();
					stubPar1.PerformInHierarchy(stubPar1.FocusInHi);
					
					Assert.That(testSSE.isFocusedInHierarchy, Is.True);
					}
				[Test]
				public void isFocusedInHierarchy_SomeNonfocusedAncester_ReturnsFalse(){
					TestSlotSystemElement stubPar1 = MakeTestSSE();
					TestSlotSystemElement stubPar2 = MakeTestSSE();
					TestSlotSystemElement stubPar3 = MakeTestSSE();
					TestSlotSystemElement testSSE = MakeTestSSE();
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
				public void SetElements_WhenCalled_SetsElements(){
					TestSlotSystemElement sse = MakeTestSSE();
						TestSlotSystemElement childA = MakeTestSSE();
							childA.transform.SetParent(sse.transform);
							childA.isToggledOnInPageByDefault = true;
						TestSlotSystemElement childB = MakeTestSSE();
							childB.transform.SetParent(sse.transform);
							childB.isToggledOnInPageByDefault = true;
						TestSlotSystemElement childC = MakeTestSSE();
							childC.transform.SetParent(sse.transform);
							childC.isToggledOnInPageByDefault = true;
						IEnumerable<ISlotSystemElement> expectedEles = new ISlotSystemElement[]{childA, childB, childC};
					
					sse.SetElements();

					bool equality = sse.MemberEquals(expectedEles);
					Assert.That(equality, Is.True);
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
				public void Activate_WhenCalled_CallsChildrensActivate(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement mockChildA = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildB = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildC = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{mockChildA, mockChildB, mockChildC});

					testSSE.Activate();

					mockChildA.Received().Activate();
					mockChildB.Received().Activate();
					mockChildC.Received().Activate();
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
				[TestCase(true)]
				[TestCase(false)]
				public void ToggleOnPageElement_Various_CallsPageEleAccordingly(bool called){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemPage stubPage = Substitute.For<ISlotSystemPage>();
					ISlotSystemPageElement mockPageEle = Substitute.For<ISlotSystemPageElement>();
					mockPageEle.isFocusToggleOn.Returns(!called);
					stubPage.GetPageElement(testSSE).Returns(mockPageEle);
					testSSE.SetParent(stubPage);

					testSSE.ToggleOnPageElement();
					
					if(called)
						mockPageEle.Received().isFocusToggleOn = true;
					else
						mockPageEle.DidNotReceive().isFocusToggleOn = true;
					}
			/*	helpers */
				SlotSystemBundle MakeSlotSystemBundle(){
					return new GameObject("ssBundleGO").AddComponent<SlotSystemBundle>();
				}
		}
	}
}
