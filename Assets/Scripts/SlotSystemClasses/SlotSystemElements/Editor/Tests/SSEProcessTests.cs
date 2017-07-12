using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using SlotSystem;
namespace SlotSystemTests{
	namespace SSEElementsTests{
		[TestFixture]
		public class SSEProcessTests {

			[Test]
			public void proces_ByDefault_IsNull(){
				SSEProcessEngine engine = MakeProcessEngine();

				Assert.That(engine.process, Is.Null);
			}
			[Test]
			public void SetAndRunProcess_NullToAny_SetsProcess(){
				SSEProcessEngine engine = MakeProcessEngine();
				ISSEProcess process = Substitute.For<ISSEProcess>();

				engine.SetAndRunProcess(process);
				
				Assert.That(engine.process, Is.SameAs(process));
			}
			[Test]
			public void SetAndRunProcess_ToAnother_SetsProcess(){
				SSEProcessEngine engine = MakeProcessEngine();
				ISSEProcess another = Substitute.For<ISSEProcess>();

				engine.SetAndRunProcess(another);
				
				Assert.That(engine.process, Is.SameAs(another));
			}
			[Test]
			public void SetAndRunProcess_AnyToNull_SetsNull(){
				SSEProcessEngine engine = MakeProcessEngine();
				ISSEProcess process = Substitute.For<ISSEProcess>();

				engine.SetAndRunProcess(process);
				engine.SetAndRunProcess(null);
				
				Assert.That(engine.process, Is.Null);
			}
			[Test]
			public void SetAndRunProcess_ToAny_StartsProcess(){
				SSEProcessEngine engine = MakeProcessEngine();
				ISSEProcess mockProcess = Substitute.For<ISSEProcess>();

				engine.SetAndRunProcess(mockProcess);
				
				mockProcess.Received().Start();
			}
			[Test]
			public void SetAndRunProcess_AnyToAnother_StopsProcess(){
				SSEProcessEngine engine = MakeProcessEngine();
				ISSEProcess mockProcess = Substitute.For<ISSEProcess>();
				ISSEProcess another = Substitute.For<ISSEProcess>();

				engine.SetAndRunProcess(mockProcess);
				engine.SetAndRunProcess(another);
				
				mockProcess.Received().Stop();
			}
			[Test]
			public void SSEProcessIsRunning_ByDefault_ReturnsFalse(){
				FakeAbsSSEProcess process = MakeProcess(MakeCoroutine());

				Assert.That(process.isRunning, Is.False);
			}
			[Test]
			public void SSEProcessIsRunning_WhenStarted_ReturnsTrue(){
				FakeAbsSSEProcess process = MakeProcess(MakeCoroutine());
				process.Start();

				Assert.That(process.isRunning, Is.True);
			}
			[Test]
			public void SSEProcessIsRunning_WhenStopped_ReturnsFalse(){
				FakeAbsSSEProcess process = MakeProcess(MakeCoroutine());
				process.Start();
				process.Stop();

				Assert.That(process.isRunning, Is.False);
			}
			[Test]
			public void SSEProcessIsRunning_ExpiredWhileRunning_ReturnsFalse(){
				FakeAbsSSEProcess process = MakeProcess(MakeCoroutine());
				process.Start();
				process.Expire();

				Assert.That(process.isRunning, Is.False);
			}
			SSEProcessEngine MakeProcessEngine(){
				return new SSEProcessEngine();
			}
			System.Func<IEnumeratorFake> MakeCoroutine(){
				return () => {return new IEnumeratorFake();};
			}
			class FakeAbsSSEProcess: AbsSSEProcess{
				public FakeAbsSSEProcess(System.Func<IEnumeratorFake> coroutine){
					coroutineFake = coroutine;
				}
			}
			FakeAbsSSEProcess MakeProcess(System.Func<IEnumeratorFake> coroutine){
				return new FakeAbsSSEProcess(coroutine);
			}
		}
	}
}
