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
		[Category("OtherElements")]
		public class SSEProcessTests: SlotSystemTest {
			[TestCaseSource(typeof(SetAndRunProcess_VariousCases))]
			public void SetAndRunProcess_Various_Various(
				ISSEProcess from, 
				ISSEProcess to, 
				ISSEProcess expectedProcess,
				bool isStopCalled, 
				bool isStartCalled)
			{
				SSEProcessEngine<ISSEProcess> engine = MakeProcessEngineWithDefaultProc(from);
				
				engine.SetAndRunProcess(to);

				if(from != null)
					if(isStopCalled) 
						from.Received().Stop();
					else 
						from.DidNotReceive().Stop();
				if(to != null)
					if(isStartCalled) 
						to.Received().Start();
					else 
						to.DidNotReceive().Start();
				Assert.That(engine.process, Is.SameAs(expectedProcess));
			}
				class SetAndRunProcess_VariousCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						object[] nullToNull;
							nullToNull =  SetAndRunProcess_VariousCase(
								from:null, to:null, 
								expectedProcess:null, 
								isStopCalled:false, isStartCalled:false
							);
							yield return nullToNull;
						object[] nullToSome;
								ISSEProcess procA_c2 = Substitute.For<ISSEProcess>();
								procA_c2.Equals(procA_c2).Returns(true);
							nullToSome = SetAndRunProcess_VariousCase(
								from:null, to:procA_c2, 
								expectedProcess:procA_c2, 
								isStopCalled:false, isStartCalled:true
							);
							yield return nullToSome;
						object[] someToSame;
								ISSEProcess procA_c3 = Substitute.For<ISSEProcess>();
								procA_c3.Equals(procA_c3).Returns(true);
							someToSame = SetAndRunProcess_VariousCase(
								from:procA_c3, to:procA_c3, 
								expectedProcess:procA_c3, 
								isStopCalled:false, isStartCalled:false
							);
							yield return someToSame;
						object[] someToSame2;
							ISSEProcess procB_c4 = Substitute.For<ISSEProcess>();
							procB_c4.Equals(procB_c4).Returns(true);

							someToSame2 = SetAndRunProcess_VariousCase(
								from:procB_c4, to:procB_c4, 
								expectedProcess:procB_c4, 
								isStopCalled:false, isStartCalled:false
							);
							yield return someToSame2;
						object[] someToNull;
								ISSEProcess procA_c5 = Substitute.For<ISSEProcess>();
								procA_c5.Equals(procA_c5).Returns(true);
							someToNull =  SetAndRunProcess_VariousCase(
								from:procA_c5, to:null, 
								expectedProcess:null, 
								isStopCalled:true, isStartCalled:false
							);
							yield return someToNull;
						object[] someToDiff;
								ISSEProcess procA_c6 = Substitute.For<ISSEProcess>();
								ISSEProcess procB_c6 = Substitute.For<ISSEProcess>();
									procA_c6.Equals(procB_c6).Returns(false);
									procA_c6.Equals(procA_c6).Returns(true);
									procB_c6.Equals(procA_c6).Returns(false);
									procB_c6.Equals(procB_c6).Returns(true);
							someToDiff = SetAndRunProcess_VariousCase(
								from:procA_c6, to:procB_c6, 
								expectedProcess:procB_c6, 
								isStopCalled:true, isStartCalled:true
							);
							yield return someToDiff;
					}
				}
				static object[] SetAndRunProcess_VariousCase(
					ISSEProcess from, 
					ISSEProcess to, 
					ISSEProcess expectedProcess,
					bool isStopCalled, 
					bool isStartCalled)
				{
					return new object[]{from, to, expectedProcess, isStopCalled, isStartCalled};
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
			/* Helper */
				SSEProcessEngine<ISSEProcess> MakeProcessEngineWithDefaultProc(ISSEProcess from){
					return new SSEProcessEngine<ISSEProcess>(from);
				}
				System.Func<IEnumeratorFake> MakeCoroutine(){
					return () => {return new IEnumeratorFake();};
				}
				class FakeAbsSSEProcess: SSEProcess{
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
