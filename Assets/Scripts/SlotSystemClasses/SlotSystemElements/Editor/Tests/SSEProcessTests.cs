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
				class SetAndRunProcess_VariousCases: IEnumerable{ public IEnumerator GetEnumerator(){
					object[] n_n_n_F_F;
						n_n_n_F_F =  new object[]{
							null, null, null, false, false};
						yield return n_n_n_F_F;
					object[] n_A_A_F_T;
							ISSEProcess procA_c2 = Substitute.For<ISSEProcess>();
							procA_c2.Equals(procA_c2).Returns(true);
						n_A_A_F_T = new object[]{
							null, procA_c2, procA_c2, false, true};
						yield return n_A_A_F_T;
					object[] A_A_A_F_F;
							ISSEProcess procA_c3 = Substitute.For<ISSEProcess>();
							procA_c3.Equals(procA_c3).Returns(true);
						A_A_A_F_F = new object[]{
							procA_c3, procA_c3, procA_c3, false, false};
						yield return A_A_A_F_F;
					object[] B_B_B_F_F;
						ISSEProcess procB_c4 = Substitute.For<ISSEProcess>();
						procB_c4.Equals(procB_c4).Returns(true);

						B_B_B_F_F = new object[]{
							procB_c4, procB_c4, procB_c4, false, false};
						yield return B_B_B_F_F;
					object[] A_n_n_T_F;
							ISSEProcess procA_c5 = Substitute.For<ISSEProcess>();
							procA_c5.Equals(procA_c5).Returns(true);
						A_n_n_T_F =  new object[]{
							procA_c5, null, null, true, false};
						yield return A_n_n_T_F;
					object[] A_B_B_T_T;
							ISSEProcess procA_c6 = Substitute.For<ISSEProcess>();
							ISSEProcess procB_c6 = Substitute.For<ISSEProcess>();
								procA_c6.Equals(procB_c6).Returns(false);
								procA_c6.Equals(procA_c6).Returns(true);
								procB_c6.Equals(procA_c6).Returns(false);
								procB_c6.Equals(procB_c6).Returns(true);
						A_B_B_T_T = new object[]{
							procA_c6, procB_c6, procB_c6, true, true};
						yield return A_B_B_T_T;
					}
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
