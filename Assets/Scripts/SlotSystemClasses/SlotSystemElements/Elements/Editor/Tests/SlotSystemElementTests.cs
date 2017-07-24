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
					[Category("State and Process")]
					public void selProcess_ByDefault_IsNull(){
						TestSlotSystemElement testSSE = MakeTestSSE();
						
						Assert.That(testSSE.selProcess, Is.Null);
					}
					/*	SelStates	*/
						[TestCaseSource(typeof(SetSelState_NullOrSSESelStateCases))][Category("State and Process")]
						public void SetSelState_NullOrSSESelState_CallsSSEStateEngineSetState(ISSESelState state){
							TestSlotSystemElement sse = MakeTestSSE();
								ISSEStateEngine<ISSESelState> engine = Substitute.For<ISSEStateEngine<ISSESelState>>();
								sse.SetSelStateEngine(engine);
							
							sse.SetSelState(state);

							engine.Received().SetState(state);
						}
							class SetSelState_NullOrSSESelStateCases: IEnumerable{
								public IEnumerator GetEnumerator(){
									yield return null;
									yield return Substitute.For<ISSESelState>();
								}
							}
						[TestCaseSource(typeof(FromToProcCases))][Category("State and Process")]
						public void SetSelState_Various_SetsSelProcAccordingly(
							TestSlotSystemElement sse, 
							ISSESelState from, 
							ISSESelState to, 
							ISSEProcess proc,
							InstantMethods method)
						{
							string expected = "default";
							switch(method){
								case InstantMethods.none: expected = ""; break;
								case InstantMethods.focus: expected = "InstantFocus called"; break;
								case InstantMethods.defocus: expected = "InstantDefocus called"; break;
								case InstantMethods.select: expected = "InstantSelect called"; break;
								default: break;
							}
							sse.SetSelState(from);

							sse.SetSelState(to);

							ISSEProcess actualProc = sse.selProcess;
								if(proc != null)
									Assert.That(actualProc, Is.TypeOf(proc.GetType()));
								else
									Assert.That(actualProc, Is.Null);
							string actualMethod = sse.message;
								Assert.That(actualMethod, Is.StringContaining(expected));
						}
							class FromToProcCases: IEnumerable{
									ISSEProcess deaProc = new SSEDeactivateProcess(MakeSubSSE(), FakeCoroutine);
									ISSEProcess focProc = new SSEFocusProcess(MakeSubSSE(), FakeCoroutine);
									ISSEProcess defProc = new SSEDefocusProcess(MakeSubSSE(), FakeCoroutine);
									ISSEProcess selProc = new SSESelectProcess(MakeSubSSE(), FakeCoroutine);
								public IEnumerator GetEnumerator(){
									object[] n_n_n_none;
										TestSlotSystemElement sse_0 = MakeTestSSE();
										n_n_n_none = new object[]{sse_0, null, null, null, InstantMethods.none};
										yield return n_n_n_none;
									object[] n_dea_n_none;
										TestSlotSystemElement sse_1 = MakeTestSSE();
										ISSESelState dea_1 = Substitute.For<ISSESelState>();
										sse_1.deactivatedState.Returns(dea_1);
										n_dea_n_none = new object[]{sse_1, null, dea_1, null, InstantMethods.none};
										yield return n_dea_n_none;
									object[] n_foc_n_foc;
										TestSlotSystemElement sse_2 = MakeTestSSE();
										ISSESelState foc_2 = Substitute.For<ISSESelState>();
										sse_2.focusedState.Returns(foc_2);
										n_foc_n_foc = new object[]{sse_2, null, foc_2, null, InstantMethods.focus};
										yield return n_foc_n_foc;
									object[] n_def_n_def;
										TestSlotSystemElement sse_3 = MakeTestSSE();
										ISSESelState def_3 = Substitute.For<ISSESelState>();
										sse_3.defocusedState.Returns(def_3);
										n_def_n_def = new object[]{sse_3, null, def_3, null, InstantMethods.defocus};
										yield return n_def_n_def;
									object[] n_sel_n_sel;
										TestSlotSystemElement sse_4 = MakeTestSSE();
										ISSESelState sel_4 = Substitute.For<ISSESelState>();
										sse_4.selectedState.Returns(sel_4);
										n_sel_n_sel = new object[]{sse_4, null, sel_4, null, InstantMethods.select};
										yield return n_sel_n_sel;
									
									object[] dea_n_n_none;
										TestSlotSystemElement sse_5 = MakeTestSSE();
										ISSESelState dea_5 = Substitute.For<ISSESelState>();
										sse_5.deactivatedState.Returns(dea_5);
										dea_n_n_none = new object[]{sse_5, dea_5, null, null, InstantMethods.none};
										yield return dea_n_n_none;
									object[] dea_dea_n_none;
										TestSlotSystemElement sse_6 = MakeTestSSE();
										ISSESelState dea_6 = Substitute.For<ISSESelState>();
										sse_6.deactivatedState.Returns(dea_6);
										dea_dea_n_none = new object[]{sse_6, dea_6, dea_6, null, InstantMethods.none};
										yield return dea_dea_n_none;
									object[] dea_foc_foc_none;
										TestSlotSystemElement sse_7 = MakeTestSSE();
										ISSESelState dea_7 = Substitute.For<ISSESelState>();
										ISSESelState foc_7 = Substitute.For<ISSESelState>();
										sse_7.deactivatedState.Returns(dea_7);
										sse_7.focusedState.Returns(foc_7);
										dea_foc_foc_none = new object[]{sse_7, dea_7, foc_7, focProc, InstantMethods.none};
										yield return dea_foc_foc_none;
									object[] dea_def_def_none;
										TestSlotSystemElement sse_8 = MakeTestSSE();
										ISSESelState dea_8 = Substitute.For<ISSESelState>();
										ISSESelState def_8 = Substitute.For<ISSESelState>();
										sse_8.deactivatedState.Returns(dea_8);
										sse_8.defocusedState.Returns(def_8);
										dea_def_def_none = new object[]{sse_8, dea_8, def_8, defProc, InstantMethods.none};
										yield return dea_def_def_none;
									object[] dea_sel_sel_none;
										TestSlotSystemElement sse_9 = MakeTestSSE();
										ISSESelState dea_9 = Substitute.For<ISSESelState>();
										ISSESelState sel_9 = Substitute.For<ISSESelState>();
										sse_9.deactivatedState.Returns(dea_9);
										sse_9.selectedState.Returns(sel_9);
										dea_sel_sel_none = new object[]{sse_9, dea_9, sel_9, selProc, InstantMethods.none};
										yield return dea_sel_sel_none;
									
									object[] foc_n_n_none;
										TestSlotSystemElement sse_10 = MakeTestSSE();
										ISSESelState foc_10 = Substitute.For<ISSESelState>();
										sse_10.focusedState.Returns(foc_10);
										foc_n_n_none = new object[]{sse_10, foc_10, null, null, InstantMethods.none};
										yield return foc_n_n_none;
									object[] foc_dea_dea_none;
										TestSlotSystemElement sse_11 = MakeTestSSE();
										ISSESelState foc_11 = Substitute.For<ISSESelState>();
										ISSESelState dea_11 = Substitute.For<ISSESelState>();
										sse_11.focusedState.Returns(foc_11);
										sse_11.deactivatedState.Returns(dea_11);
										foc_dea_dea_none = new object[]{sse_11, foc_11, dea_11, deaProc, InstantMethods.none};
										yield return foc_dea_dea_none;
									object[] foc_foc_n_none;
										TestSlotSystemElement sse_12 = MakeTestSSE();
										ISSESelState foc_12 = Substitute.For<ISSESelState>();
										sse_12.focusedState.Returns(foc_12);
										foc_foc_n_none = new object[]{sse_12, foc_12, foc_12, null, InstantMethods.none};
										yield return foc_foc_n_none;
									object[] foc_def_def_none;
										TestSlotSystemElement sse_13 = MakeTestSSE();
										ISSESelState foc_13 = Substitute.For<ISSESelState>();
										ISSESelState def_13 = Substitute.For<ISSESelState>();
										sse_13.focusedState.Returns(foc_13);
										sse_13.defocusedState.Returns(def_13);
										foc_def_def_none = new object[]{sse_13, foc_13, def_13, defProc, InstantMethods.none};
										yield return foc_def_def_none;
									object[] foc_sel_sel_none;
										TestSlotSystemElement sse_14 = MakeTestSSE();
										ISSESelState foc_14 = Substitute.For<ISSESelState>();
										ISSESelState sel_14 = Substitute.For<ISSESelState>();
										sse_14.focusedState.Returns(foc_14);
										sse_14.selectedState.Returns(sel_14);
										foc_sel_sel_none = new object[]{sse_14, foc_14, sel_14, selProc, InstantMethods.none};
										yield return foc_sel_sel_none;
									
									object[] def_n_n_none;
										TestSlotSystemElement sse_15 = MakeTestSSE();
										ISSESelState def_15 = Substitute.For<ISSESelState>();
										sse_15.defocusedState.Returns(def_15);
										def_n_n_none = new object[]{sse_15, def_15, null, null, InstantMethods.none};
										yield return def_n_n_none;
									object[] def_dea_dea_none;
										TestSlotSystemElement sse_16 = MakeTestSSE();
										ISSESelState def_16 = Substitute.For<ISSESelState>();
										ISSESelState dea_16 = Substitute.For<ISSESelState>();
										sse_16.defocusedState.Returns(def_16);
										sse_16.deactivatedState.Returns(dea_16);
										def_dea_dea_none = new object[]{sse_16, def_16, dea_16, deaProc, InstantMethods.none};
										yield return def_dea_dea_none;
									object[] def_foc_foc_none;
										TestSlotSystemElement sse_17 = MakeTestSSE();
										ISSESelState def_17 = Substitute.For<ISSESelState>();
										ISSESelState foc_17 = Substitute.For<ISSESelState>();
										sse_17.defocusedState.Returns(def_17);
										sse_17.focusedState.Returns(foc_17);
										def_foc_foc_none = new object[]{sse_17, def_17, foc_17, focProc, InstantMethods.none};
										yield return def_foc_foc_none;
									object[] def_def_n_none;
										TestSlotSystemElement sse_18 = MakeTestSSE();
										ISSESelState def_18 = Substitute.For<ISSESelState>();
										sse_18.defocusedState.Returns(def_18);
										def_def_n_none = new object[]{sse_18, def_18, def_18, null, InstantMethods.none};
										yield return def_def_n_none;
									object[] def_sel_sel_none;
										TestSlotSystemElement sse_19 = MakeTestSSE();
										ISSESelState def_19 = Substitute.For<ISSESelState>();
										ISSESelState sel_19 = Substitute.For<ISSESelState>();
										sse_19.defocusedState.Returns(def_19);
										sse_19.selectedState.Returns(sel_19);
										def_sel_sel_none = new object[]{sse_19, def_19, sel_19, selProc, InstantMethods.none};
										yield return def_sel_sel_none;
									
									object[] sel_n_n_none;
										TestSlotSystemElement sse_20 = MakeTestSSE();
										ISSESelState sel_20 = Substitute.For<ISSESelState>();
										sse_20.selectedState.Returns(sel_20);
										sel_n_n_none = new object[]{sse_20, sel_20, null, null, InstantMethods.none};
										yield return sel_n_n_none;
									object[] sel_dea_dea_none;
										TestSlotSystemElement sse_21 = MakeTestSSE();
										ISSESelState sel_21 = Substitute.For<ISSESelState>();
										ISSESelState dea_21 = Substitute.For<ISSESelState>();
										sse_21.selectedState.Returns(sel_21);
										sse_21.deactivatedState.Returns(dea_21);
										sel_dea_dea_none = new object[]{sse_21, sel_21, dea_21, deaProc, InstantMethods.none};
										yield return sel_dea_dea_none;
									object[] sel_foc_foc_none;
										TestSlotSystemElement sse_22 = MakeTestSSE();
										ISSESelState sel_22 = Substitute.For<ISSESelState>();
										ISSESelState foc_22 = Substitute.For<ISSESelState>();
										sse_22.selectedState.Returns(sel_22);
										sse_22.focusedState.Returns(foc_22);
										sel_foc_foc_none = new object[]{sse_22, sel_22, foc_22, focProc, InstantMethods.none};
										yield return sel_foc_foc_none;
									object[] sel_def_def_none;
										TestSlotSystemElement sse_23 = MakeTestSSE();
										ISSESelState sel_23 = Substitute.For<ISSESelState>();
										ISSESelState def_23 = Substitute.For<ISSESelState>();
										sse_23.selectedState.Returns(sel_23);
										sse_23.defocusedState.Returns(def_23);
										sel_def_def_none = new object[]{sse_23, sel_23, def_23, defProc, InstantMethods.none};
										yield return sel_def_def_none;
									object[] sel_sel_n_none;
										TestSlotSystemElement sse_24 = MakeTestSSE();
										ISSESelState sel_24 = Substitute.For<ISSESelState>();
										sse_24.selectedState.Returns(sel_24);
										sel_sel_n_none = new object[]{sse_24, sel_24, sel_24, null, InstantMethods.none};
										yield return sel_sel_n_none;
									
								}
							}
							public enum InstantMethods{none, focus, defocus, select};
						
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
				[Category("Fields")]
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
				[Category("Fields")]
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
				[Category("Fields")]
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
				[Test][Category("Fields")]
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
				[Category("Fields")]
				public void level_ParentNull_ReturnsZero(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					Assert.That(testSSE.level, Is.EqualTo(0));
				}
				[Test]
				[Category("Fields")]
				public void level_OneParent_ReturnsOne(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubEle = MakeTestSSE();
					testSSE.SetParent(stubEle);

					Assert.That(testSSE.level, Is.EqualTo(1));
				}
				[Test]
				[Category("Fields")]
				public void level_TwoParent_ReturnsTwo(){
					TestSlotSystemElement stubEle_1 = MakeTestSSE();
					TestSlotSystemElement stubEle_2 = MakeTestSSE();
					TestSlotSystemElement testSSE = MakeTestSSE();
					stubEle_2.SetParent(stubEle_1);
					testSSE.SetParent(stubEle_2);

					Assert.That(testSSE.level, Is.EqualTo(2));
				}
				[Test]
				[Category("Fields")]
				public void isBundleElement_ParentIsBundle_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					SlotSystemBundle stubBundle = MakeSlotSystemBundle();
					testSSE.SetParent(stubBundle);

					Assert.That(testSSE.isBundleElement, Is.True);
				}
				[Test]
				[Category("Fields")]
				public void isBundleElement_ParentIsNotBundle_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubSSE = MakeTestSSE();
					testSSE.SetParent(stubSSE);

					Assert.That(testSSE.isBundleElement, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isPageElement_ParentIsPage_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemPage stubPage = Substitute.For<ISlotSystemPage>();
					testSSE.SetParent(stubPage);

					Assert.That(testSSE.isPageElement, Is.True);
				}
				[Test]
				[Category("Fields")]
				public void isPageElement_ParentIsNotPage_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubSSE = MakeTestSSE();
					testSSE.SetParent(stubSSE);

					Assert.That(testSSE.isPageElement, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isToggledOn_ParentNotPage_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubSSE = MakeTestSSE();
					testSSE.SetParent(stubSSE);

					Assert.That(testSSE.isToggledOn, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isToggledOn_ParentIsPageAndElementToggledOn_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemPage stubPage = Substitute.For<ISlotSystemPage>();
					stubPage.GetPageElement(testSSE).isFocusToggleOn.Returns(true);
					testSSE.SetParent(stubPage);

					Assert.That(testSSE.isToggledOn, Is.True);
				}
				[Test]
				[Category("Fields")]
				public void isToggledOn_ParentIsPageAndElementToggledOff_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemPage stubPage = Substitute.For<ISlotSystemPage>();
					stubPage.GetPageElement(testSSE).isFocusToggleOn.Returns(false);
					testSSE.SetParent(stubPage);

					Assert.That(testSSE.isToggledOn, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isFocused_CurSelStateIsFocused_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					testSSE.SetSelState(testSSE.focusedState);

					Assert.That(testSSE.isFocused, Is.True);
					Assert.That(testSSE.isDefocused, Is.False);
					Assert.That(testSSE.isDeactivated, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isDefocused_CurSelStateIsDefocused_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					testSSE.SetSelState(testSSE.defocusedState);

					Assert.That(testSSE.isFocused, Is.False);
					Assert.That(testSSE.isDefocused, Is.True);
					Assert.That(testSSE.isDeactivated, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isDeactivated_CurSelStateIsDeactivated_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					testSSE.SetSelState(testSSE.deactivatedState);

					Assert.That(testSSE.isFocused, Is.False);
					Assert.That(testSSE.isDefocused, Is.False);
					Assert.That(testSSE.isDeactivated, Is.True);
				}
				[Test][Category("Fields")]
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
				[Category("Fields")]
				public void isFocusedInHierarchy_SelfFocusedAndNoParent_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					testSSE.Focus();

					Assert.That(testSSE.isFocusedInHierarchy, Is.True);
				}
				[Test]
				[Category("Fields")]
				public void isFocusedInHierarchy_SelfNotFocused_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					Assert.That(testSSE.isFocusedInHierarchy, Is.False);
				}
				[Test]
				[Category("Fields")]
				public void isFocusedInHierarchy_SelfFocusedParentNotFocused_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubPar = MakeTestSSE();
					testSSE.SetParent(stubPar);
					stubPar.Defocus();
					testSSE.Focus();
					
					Assert.That(testSSE.isFocusedInHierarchy, Is.False);
				}
				[Test]
				[Category("Fields")]
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
					stubPar1.Focus();
					
					Assert.That(testSSE.isFocusedInHierarchy, Is.True);
				}
				[Test]
				[Category("Fields")]
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
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
				public void InitializeStates_WhenCalled_CallsStateEnginesSetState(){
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
						ISSEStateEngine<ISSESelState> mockSelStateEg = Substitute.For<ISSEStateEngine<ISSESelState>>();
							sse.SetSelStateEngine(mockSelStateEg);
					
					sse.InitializeStates();

					mockSelStateEg.Received().SetState(sse.deactivatedState);
				}
				[Test][Category("Methods")]
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
				[Category("Methods")]
				[ExpectedException(typeof(System.ArgumentNullException))]
				public void ContainsInHierarchy_Null_ThrowsException(){
					TestSlotSystemElement testSSE = MakeTestSSE();

					testSSE.ContainsInHierarchy(null);
				}
				[Test]
				[Category("Methods")]
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
				[Category("Methods")]
				public void ContainsInHierarchy_DirectChild_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					TestSlotSystemElement stubChild = MakeTestSSE();
					stubChild.SetParent(testSSE);

					bool result = testSSE.ContainsInHierarchy(stubChild);

					Assert.That(result, Is.True);
				}
				[Test]
				[Category("Methods")]
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
				[Category("Methods")]
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
				[Category("Methods")]
				public void Deactivate_WhenCalled_CallsChildrensDeactivate(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement mockChildA = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildB = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildC = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{mockChildA, mockChildB, mockChildC});

					testSSE.Deactivate();

					mockChildA.Received().Deactivate();
					mockChildB.Received().Deactivate();
					mockChildC.Received().Deactivate();
				}
				[Test]
				[Category("Methods")]
				public void Deactivate_WhenCalled_SetCurStateToDeactivated(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					
					testSSE.Deactivate();

					Assert.That(testSSE.curSelState, Is.SameAs(testSSE.deactivatedState));
				}
				[Test]
				[Category("Methods")]
				public void Focus_WhenCalled_CallsChildrensFocus(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement mockChildA = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildB = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildC = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{mockChildA, mockChildB, mockChildC});

					testSSE.Focus();

					mockChildA.Received().Focus();
					mockChildB.Received().Focus();
					mockChildC.Received().Focus();
				}
				[Test]
				[Category("Methods")]
				public void Focus_WhenCalled_SetCurStateToFocusd(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					
					testSSE.Focus();

					Assert.That(testSSE.curSelState, Is.SameAs(testSSE.focusedState));
				}
				[Test]
				[Category("Methods")]
				public void Defocus_WhenCalled_CallsChildrensDefocus(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement mockChildA = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildB = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement mockChildC = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{mockChildA, mockChildB, mockChildC});

					testSSE.Defocus();

					mockChildA.Received().Defocus();
					mockChildB.Received().Defocus();
					mockChildC.Received().Defocus();
				}
				[Test]
				[Category("Methods")]
				public void Defocus_WhenCalled_SetCurStateToDefocusd(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					
					testSSE.Defocus();

					Assert.That(testSSE.curSelState, Is.SameAs(testSSE.defocusedState));
				}
				[Test][Category("Methods")]
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
				[Test][Category("Methods")]
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
								[Test][Category("Methods")]
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
				[Category("Methods")]
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
				[Category("Methods")]
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
				[Category("Methods")]
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
				[Category("Methods")]
				public void Contains_NoElements_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement stubEle = Substitute.For<ISlotSystemElement>();
					
					Assert.That(testSSE.Contains(stubEle), Is.False);
				}
				[Test]
				[Category("Methods")]
				public void Contains_NonMember_ReturnsFalse(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement stubMember = Substitute.For<ISlotSystemElement>();
					ISlotSystemElement stubNonMember = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{stubMember});
					
					Assert.That(testSSE.Contains(stubNonMember), Is.False);
				}
				[Test]
				[Category("Methods")]
				public void Contains_Member_ReturnsTrue(){
					TestSlotSystemElement testSSE = MakeTestSSE();
					ISlotSystemElement stubMember = Substitute.For<ISlotSystemElement>();
					testSSE.SetElements(new ISlotSystemElement[]{stubMember});
					
					Assert.That(testSSE.Contains(stubMember), Is.True);
				}
				[TestCase(true)]
				[TestCase(false)]
				[Category("Methods")]
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
