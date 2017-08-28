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
		public class SelStateHandlerTests {
			[Test]
			public void SelStateFields_ByDefault_AreSetDefault(){
				UISelStateHandler handler = new UISelStateHandler();

				Assert.That(handler.IsDeactivated(), Is.False);
				Assert.That(handler.IsSelectable(), Is.False);
				Assert.That(handler.IsUnselectable(), Is.False);
				Assert.That(handler.IsSelected(), Is.False);
				Assert.That(handler.IsSelStateNull(), Is.True);
				Assert.That(handler.WasSelStateNull(), Is.True);
			}
			[Test]
			public void Activate_Always_SetsFocused(){
				UISelStateHandler handler = new UISelStateHandler();

				handler.Activate();

				Assert.That(handler.IsSelectable(), Is.True);
			}
			[Test]
			public void Deactivate_WhenCalled_SetsCurSelStateDeactivated(){
				UISelStateHandler handler = new UISelStateHandler();
				
				handler.Deactivate();

				Assert.That(handler.IsDeactivated(), Is.True);
				Assert.That(handler.IsUnselectable(), Is.False);
				Assert.That(handler.IsSelectable(), Is.False);
				Assert.That(handler.IsSelected(), Is.False);
			}
			[Test]
			public void Deactivate_WasSelStateNull_DoesNotSetSelProc(){
				UISelStateHandler handler = new UISelStateHandler();

				handler.Deactivate();

				Assert.That(handler.GetSelProcess(), Is.Null);
			}
			[Test]
			public void Deactivate_IsNotSelStateInit_SetsSelProcDeactivateProc(){
				UISelStateHandler handler = new UISelStateHandler();
				handler.MakeUnselectable();

				handler.Deactivate();

				Assert.That(handler.GetSelProcess(), Is.TypeOf(typeof(UIDeactivateProcess)));
				}
			[Test]
			public void Deactivate_FromNullToDeaToDea_DoesNotSetSelProc(){
				UISelStateHandler handler = new UISelStateHandler();
				handler.Deactivate();

				handler.Deactivate();

				Assert.That(handler.GetSelProcess(), Is.Null);
			}
			[Test]
			public void Focus_WhenCalled_SetsCurSelStateFocused(){
				UISelStateHandler handler = new UISelStateHandler();
				
				handler.MakeSelectable();

				Assert.That(handler.IsDeactivated(), Is.False);
				Assert.That(handler.IsUnselectable(), Is.False);
				Assert.That(handler.IsSelectable(), Is.True);
				Assert.That(handler.IsSelected(), Is.False);
			}

			[Test]
			public void Focus_IsSelStateInit_DoesNotSetSelProc(){
				UISelStateHandler handler = new UISelStateHandler();

				handler.MakeSelectable();

				Assert.That(handler.GetSelProcess(), Is.Null);
				}
			[Test]
			public void Focus_IsSelStateInit_CallsInstantFocus(){
				UISelStateHandler handler = new UISelStateHandler();
					IUICommand mockComm = Substitute.For<IUICommand>();
					IInstantCommands stubInstantCommands = Substitute.For<IInstantCommands>();
					stubInstantCommands.When(x => x.ExecuteInstantFocus()).Do(x => mockComm.Execute());
					handler.SetInstantCommands(stubInstantCommands);

				handler.MakeSelectable();

				mockComm.Received().Execute();
				}
			[Test]
			public void Focus_IsNotSelStateInit_SetsSelProcFocus(){
				UISelStateHandler handler = new UISelStateHandler();
				handler.Deactivate();

				handler.MakeSelectable();

				Assert.That(handler.GetSelProcess(), Is.TypeOf(typeof(UIFocusProcess)));
				}
			[Test]
			public void Defocus_WhenCalled_SetCurStateToDefocusd(){
				UISelStateHandler handler = new UISelStateHandler();
				
				handler.MakeUnselectable();

				Assert.That(handler.IsDeactivated(), Is.False);
				Assert.That(handler.IsUnselectable(), Is.True);
				Assert.That(handler.IsSelectable(), Is.False);
				Assert.That(handler.IsSelected(), Is.False);
				}
			[Test]
			public void Defocus_IsSelStateInit_DoesNotSetSelProc(){
				UISelStateHandler handler = new UISelStateHandler();

				handler.MakeUnselectable();

				Assert.That(handler.GetSelProcess(), Is.Null);
				}
			[Test]
			public void Defocus_IsSelStateInit_CallsInstantDefocus(){
				UISelStateHandler handler = new UISelStateHandler();
					IUICommand mockComm = Substitute.For<IUICommand>();
					IInstantCommands stubInstantCommands = Substitute.For<IInstantCommands>();
					stubInstantCommands.When(x => x.ExecuteInstantDefocus()).Do(x => mockComm.Execute());
					handler.SetInstantCommands(stubInstantCommands);

				handler.MakeUnselectable();

				mockComm.Received().Execute();
				}
			[Test]
			public void Defocus_IsNotSelStateInit_SetsSelProcDefocus(){
				UISelStateHandler handler = new UISelStateHandler();
				handler.Deactivate();

				handler.MakeUnselectable();

				Assert.That(handler.GetSelProcess(), Is.TypeOf(typeof(UIDefocusProcess)));
				}
			[Test]
			public void Select_WhenCalled_SetCurStateToSelected(){
				UISelStateHandler handler = new UISelStateHandler();
				
				handler.Select();

				Assert.That(handler.IsDeactivated(), Is.False);
				Assert.That(handler.IsUnselectable(), Is.False);
				Assert.That(handler.IsSelectable(), Is.False);
				Assert.That(handler.IsSelected(), Is.True);
				}
			[Test]
			public void Select_IsSelStateInit_DoesNotSetSelProc(){
				UISelStateHandler handler = new UISelStateHandler();

				handler.Select();

				Assert.That(handler.GetSelProcess(), Is.Null);
				}
			[Test]
			public void Select_IsSelStateInit_CallsInstantSelect(){
				UISelStateHandler handler = new UISelStateHandler();
					IUICommand mockComm = Substitute.For<IUICommand>();
					IInstantCommands stubInstantCommands = Substitute.For<IInstantCommands>();
					stubInstantCommands.When(x => x.ExecuteInstantSelect()).Do(x => mockComm.Execute());
					handler.SetInstantCommands(stubInstantCommands);

				handler.Select();

				mockComm.Received().Execute();
				}
			[Test]
			public void Select_IsNotSelStateInit_SetsSelProcSelect(){
				UISelStateHandler handler = new UISelStateHandler();
				handler.Deactivate();

				handler.Select();

				Assert.That(handler.GetSelProcess(), Is.TypeOf(typeof(UISelectProcess)));
				}
			[TestCaseSource(typeof(SetAndRunSelProcess_ISSESelProcessOrNullCases))]
			public void SetAndRunSelProcess_ISSESelProcessOrNull_CallsSelProcEngineSAR(IUISelProcess process){
				UISelStateHandler handler = new UISelStateHandler();
					IUIProcessEngine<IUISelProcess> engine = Substitute.For<IUIProcessEngine<IUISelProcess>>();
					handler.SetSelProcEngine(engine);
				
				handler.SetAndRunSelProcess(process);

				engine.Received().SetAndRunProcess(process);
			}
			class SetAndRunSelProcess_ISSESelProcessOrNullCases: IEnumerable{
				public IEnumerator GetEnumerator(){
					yield return Substitute.For<IUISelProcess>();
					yield return null;
				}
			}
		}
	}
}
