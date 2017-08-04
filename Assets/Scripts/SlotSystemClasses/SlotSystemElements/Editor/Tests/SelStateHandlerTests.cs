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
		public class SelStateHandlerTests {
			[Test]
			public void SelStateFields_ByDefault_AreSetDefault(){
				SSEStateHandler handler = new SSEStateHandler();

				Assert.That(handler.isDeactivated, Is.False);
				Assert.That(handler.isFocused, Is.False);
				Assert.That(handler.isDefocused, Is.False);
				Assert.That(handler.isSelected, Is.False);
			}
			[Test]
			public void Activate_Always_SetsFocused(){
				SSEStateHandler handler = new SSEStateHandler();

				handler.Activate();

				Assert.That(handler.isFocused, Is.True);
			}
			[Test]
			public void Deactivate_WhenCalled_SetsCurSelStateDeactivated(){
				SSEStateHandler handler = new SSEStateHandler();
				
				handler.Deactivate();

				Assert.That(handler.isDeactivated, Is.True);
				Assert.That(handler.isDefocused, Is.False);
				Assert.That(handler.isFocused, Is.False);
				Assert.That(handler.isSelected, Is.False);
			}
			[Test]
			public void Deactivate_WasSelStateNull_DoesNotSetSelProc(){
				SSEStateHandler handler = new SSEStateHandler();

				handler.Deactivate();

				Assert.That(handler.selProcess, Is.Null);
			}
			[Test]
			public void Deactivate_IsNotSelStateInit_SetsSelProcDeactivateProc(){
				SSEStateHandler handler = new SSEStateHandler();
				handler.Defocus();

				handler.Deactivate();

				Assert.That(handler.selProcess, Is.TypeOf(typeof(SSEDeactivateProcess)));
				}
			[Test]
			public void Deactivate_FromNullToDeaToDea_DoesNotSetSelProc(){
				SSEStateHandler handler = new SSEStateHandler();
				handler.Deactivate();

				handler.Deactivate();

				Assert.That(handler.selProcess, Is.Null);
			}
			[Test]
			public void Focus_WhenCalled_SetsCurSelStateFocused(){
				SSEStateHandler handler = new SSEStateHandler();
				
				handler.Focus();

				Assert.That(handler.isDeactivated, Is.False);
				Assert.That(handler.isDefocused, Is.False);
				Assert.That(handler.isFocused, Is.True);
				Assert.That(handler.isSelected, Is.False);
			}

			[Test]
			public void Focus_IsSelStateInit_DoesNotSetSelProc(){
				SSEStateHandler handler = new SSEStateHandler();

				handler.Focus();

				Assert.That(handler.selProcess, Is.Null);
				}
			[Test]
			public void Focus_IsSelStateInit_CallsInstantFocus(){
				SSEStateHandler handler = new SSEStateHandler();
					ISSECommand mockComm = Substitute.For<ISSECommand>();
					IInstantCommands stubInstantCommands = Substitute.For<IInstantCommands>();
					stubInstantCommands.When(x => x.ExecuteInstantFocus()).Do(x => mockComm.Execute());
					handler.SetInstantCommands(stubInstantCommands);

				handler.Focus();

				mockComm.Received().Execute();
				}
			[Test]
			public void Focus_IsNotSelStateInit_SetsSelProcFocus(){
				SSEStateHandler handler = new SSEStateHandler();
				handler.Deactivate();

				handler.Focus();

				Assert.That(handler.selProcess, Is.TypeOf(typeof(SSEFocusProcess)));
				}
			[Test]
			public void Defocus_WhenCalled_SetCurStateToDefocusd(){
				SSEStateHandler handler = new SSEStateHandler();
				
				handler.Defocus();

				Assert.That(handler.isDeactivated, Is.False);
				Assert.That(handler.isDefocused, Is.True);
				Assert.That(handler.isFocused, Is.False);
				Assert.That(handler.isSelected, Is.False);
				}
			[Test]
			public void Defocus_IsSelStateInit_DoesNotSetSelProc(){
				SSEStateHandler handler = new SSEStateHandler();

				handler.Defocus();

				Assert.That(handler.selProcess, Is.Null);
				}
			[Test]
			public void Defocus_IsSelStateInit_CallsInstantDefocus(){
				SSEStateHandler handler = new SSEStateHandler();
					ISSECommand mockComm = Substitute.For<ISSECommand>();
					IInstantCommands stubInstantCommands = Substitute.For<IInstantCommands>();
					stubInstantCommands.When(x => x.ExecuteInstantDefocus()).Do(x => mockComm.Execute());
					handler.SetInstantCommands(stubInstantCommands);

				handler.Defocus();

				mockComm.Received().Execute();
				}
			[Test]
			public void Defocus_IsNotSelStateInit_SetsSelProcDefocus(){
				SSEStateHandler handler = new SSEStateHandler();
				handler.Deactivate();

				handler.Defocus();

				Assert.That(handler.selProcess, Is.TypeOf(typeof(SSEDefocusProcess)));
				}
			[Test]
			public void Select_WhenCalled_SetCurStateToSelected(){
				SSEStateHandler handler = new SSEStateHandler();
				
				handler.Select();

				Assert.That(handler.isDeactivated, Is.False);
				Assert.That(handler.isDefocused, Is.False);
				Assert.That(handler.isFocused, Is.False);
				Assert.That(handler.isSelected, Is.True);
				}
			[Test]
			public void Select_IsSelStateInit_DoesNotSetSelProc(){
				SSEStateHandler handler = new SSEStateHandler();

				handler.Select();

				Assert.That(handler.selProcess, Is.Null);
				}
			[Test]
			public void Select_IsSelStateInit_CallsInstantSelect(){
				SSEStateHandler handler = new SSEStateHandler();
					ISSECommand mockComm = Substitute.For<ISSECommand>();
					IInstantCommands stubInstantCommands = Substitute.For<IInstantCommands>();
					stubInstantCommands.When(x => x.ExecuteInstantSelect()).Do(x => mockComm.Execute());
					handler.SetInstantCommands(stubInstantCommands);

				handler.Select();

				mockComm.Received().Execute();
				}
			[Test]
			public void Select_IsNotSelStateInit_SetsSelProcSelect(){
				SSEStateHandler handler = new SSEStateHandler();
				handler.Deactivate();

				handler.Select();

				Assert.That(handler.selProcess, Is.TypeOf(typeof(SSESelectProcess)));
				}
			[TestCaseSource(typeof(SetAndRunSelProcess_ISSESelProcessOrNullCases))]
			public void SetAndRunSelProcess_ISSESelProcessOrNull_CallsSelProcEngineSAR(ISSESelProcess process){
				SSEStateHandler handler = new SSEStateHandler();
					ISSEProcessEngine<ISSESelProcess> engine = Substitute.For<ISSEProcessEngine<ISSESelProcess>>();
					handler.SetSelProcEngine(engine);
				
				handler.SetAndRunSelProcess(process);

				engine.Received().SetAndRunProcess(process);
			}
			class SetAndRunSelProcess_ISSESelProcessOrNullCases: IEnumerable{
				public IEnumerator GetEnumerator(){
					yield return Substitute.For<ISSESelProcess>();
					yield return null;
				}
			}
		}
	}
}
