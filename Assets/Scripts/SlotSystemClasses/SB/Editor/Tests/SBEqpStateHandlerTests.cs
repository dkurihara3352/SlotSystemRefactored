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
		public class SBEqpStateHandlerTests :SlotSystemTest{
			[Test]
			public void SetEqpState_NullAndEqpProcNotNull_CallsProcEngineSetAndRunNull(){
				ISSEProcessEngine<ISBEqpProcess> mockProcEngine;
				SBEqpStateHandler handler = MakeEqpStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.process.Returns(Substitute.For<ISBEqpProcess>());
				
				handler.SetEqpState(null);

				mockProcEngine.Received().SetAndRunProcess((ISBEqpProcess) null);
			}
			[Test]
			public void SetEqpState_NotNull_DoesNotCallsProcEngineSetAndRunNull(){
				ISSEProcessEngine<ISBEqpProcess> mockProcEngine;
				SBEqpStateHandler handler = MakeEqpStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.process.Returns(Substitute.For<ISBEqpProcess>());
				
				handler.SetEqpState(Substitute.For<ISBEqpState>());

				mockProcEngine.DidNotReceive().SetAndRunProcess((ISBEqpProcess) null);
			}
			[Test]
			public void SetEqpState_NullAndEqpProcNull_DoesNotCallsProcEngineSetAndRunNull(){
				ISSEProcessEngine<ISBEqpProcess> mockProcEngine;
				SBEqpStateHandler handler = MakeEqpStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.process.Returns((ISBEqpProcess)null);
				
				handler.SetEqpState((ISBEqpState)null);

				mockProcEngine.DidNotReceive().SetAndRunProcess((ISBEqpProcess) null);
			}
			SBEqpStateHandler MakeEqpStateHandlerWithProcEngine(out ISSEProcessEngine<ISBEqpProcess> engine){
				SBEqpStateHandler handler = new SBEqpStateHandler(MakeSubSB());
				engine = Substitute.For<ISSEProcessEngine<ISBEqpProcess>>();
				handler.SetEqpProcessEngine(engine);
				return handler;
			}
		}
	}
}

