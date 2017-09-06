using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using UISystem;
using Utility;
namespace SlotSystemTests{
	namespace SlottableTests{
		public class SBEqpStateHandlerTests :SlotSystemTest{
			[Test]
			public void SetEqpState_NullAndEqpProcNotNull_CallsProcEngineSetAndRunNull(){
				IUIProcessEngine<ISBEqpProcess> mockProcEngine;
				SBEqpStateHandler handler = MakeEqpStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.GetProcess().Returns(Substitute.For<ISBEqpProcess>());
				
				handler.SetEqpState(null);

				mockProcEngine.Received().SetAndRunProcess((ISBEqpProcess) null);
			}
			[Test]
			public void SetEqpState_NotNull_DoesNotCallsProcEngineSetAndRunNull(){
				IUIProcessEngine<ISBEqpProcess> mockProcEngine;
				SBEqpStateHandler handler = MakeEqpStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.GetProcess().Returns(Substitute.For<ISBEqpProcess>());
				
				handler.SetEqpState(Substitute.For<ISBEqpState>());

				mockProcEngine.DidNotReceive().SetAndRunProcess((ISBEqpProcess) null);
			}
			[Test]
			public void SetEqpState_NullAndEqpProcNull_DoesNotCallsProcEngineSetAndRunNull(){
				IUIProcessEngine<ISBEqpProcess> mockProcEngine;
				SBEqpStateHandler handler = MakeEqpStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.GetProcess().Returns((ISBEqpProcess)null);
				
				handler.SetEqpState((ISBEqpState)null);

				mockProcEngine.DidNotReceive().SetAndRunProcess((ISBEqpProcess) null);
			}
			SBEqpStateHandler MakeEqpStateHandlerWithProcEngine(out IUIProcessEngine<ISBEqpProcess> engine){
				SBEqpStateHandler handler = new SBEqpStateHandler(MakeSubSB());
				engine = Substitute.For<IUIProcessEngine<ISBEqpProcess>>();
				handler.SetEqpProcessEngine(engine);
				return handler;
			}
		}
	}
}

