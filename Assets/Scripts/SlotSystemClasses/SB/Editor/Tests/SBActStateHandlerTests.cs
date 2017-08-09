using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using Utility;
namespace SlotSystemTests{
	namespace SlottableTests{
		[TestFixture]
		public class SBActStateHandlerTests: SlotSystemTest {
			[Test]
			public void ExpireProcess_ActProcessNotNull_CallsProcExpire(){
				SBActStateHandler handler = new SBActStateHandler(MakeSubSB(), MakeSubTAM());
					ISBActProcess mockProc = Substitute.For<ISBActProcess>();
					ISSEProcessEngine<ISBActProcess> stubEngine = Substitute.For<ISSEProcessEngine<ISBActProcess>>();
					stubEngine.process.Returns(mockProc);
					handler.SetActProcessEngine(stubEngine);

				handler.ExpireActProcess();

				mockProc.Received().Expire();
			}
			[Test]
			public void SetActState_NullAndActProcNotNull_CallsProcEngineSetAndRunNull(){
				ISSEProcessEngine<ISBActProcess> mockProcEngine;
				SBActStateHandler handler = MakeActStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.process.Returns(Substitute.For<ISBActProcess>());
				
				handler.SetActState(null);

				mockProcEngine.Received().SetAndRunProcess((ISBActProcess) null);
			}
			[Test]
			public void SetActState_NotNull_DoesNotCallsProcEngineSetAndRunNull(){
				ISSEProcessEngine<ISBActProcess> mockProcEngine;
				SBActStateHandler handler = MakeActStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.process.Returns(Substitute.For<ISBActProcess>());
				
				handler.SetActState(Substitute.For<ISBActState>());

				mockProcEngine.DidNotReceive().SetAndRunProcess((ISBActProcess) null);
			}
			[Test]
			public void SetActState_NullAndActProcNull_DoesNotCallsProcEngineSetAndRunNull(){
				ISSEProcessEngine<ISBActProcess> mockProcEngine;
				SBActStateHandler handler = MakeActStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.process.Returns((ISBActProcess)null);
				
				handler.SetActState((ISBActState)null);

				mockProcEngine.DidNotReceive().SetAndRunProcess((ISBActProcess) null);
			}
			SBActStateHandler MakeActStateHandlerWithProcEngine(out ISSEProcessEngine<ISBActProcess> engine){
				SBActStateHandler handler = new SBActStateHandler(MakeSubSB(), MakeSubTAM());
				engine = Substitute.For<ISSEProcessEngine<ISBActProcess>>();
				handler.SetActProcessEngine(engine);
				return handler;
			}
		}
	}
}
