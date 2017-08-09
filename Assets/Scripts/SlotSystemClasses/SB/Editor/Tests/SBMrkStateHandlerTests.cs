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
		public class SBMrkStateHandlerTests: SlotSystemTest{
			[Test]
			public void SetMrkState_NullAndMrkProcNotNull_CallsProcEngineSetAndRunNull(){
				ISSEProcessEngine<ISBMrkProcess> mockProcEngine;
				SBMrkStateHandler handler = MakeMrkStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.process.Returns(Substitute.For<ISBMrkProcess>());
				
				handler.SetMrkState(null);

				mockProcEngine.Received().SetAndRunProcess((ISBMrkProcess) null);
			}
			[Test]
			public void SetMrkState_NotNull_DoesNotCallsProcEngineSetAndRunNull(){
				ISSEProcessEngine<ISBMrkProcess> mockProcEngine;
				SBMrkStateHandler handler = MakeMrkStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.process.Returns(Substitute.For<ISBMrkProcess>());
				
				handler.SetMrkState(Substitute.For<ISBMrkState>());

				mockProcEngine.DidNotReceive().SetAndRunProcess((ISBMrkProcess) null);
			}
			[Test]
			public void SetMrkState_NullAndMrkProcNull_DoesNotCallsProcEngineSetAndRunNull(){
				ISSEProcessEngine<ISBMrkProcess> mockProcEngine;
				SBMrkStateHandler handler = MakeMrkStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.process.Returns((ISBMrkProcess)null);
				
				handler.SetMrkState((ISBMrkState)null);

				mockProcEngine.DidNotReceive().SetAndRunProcess((ISBMrkProcess) null);
			}
			SBMrkStateHandler MakeMrkStateHandlerWithProcEngine(out ISSEProcessEngine<ISBMrkProcess> engine){
				SBMrkStateHandler handler = new SBMrkStateHandler(MakeSubSB());
				engine = Substitute.For<ISSEProcessEngine<ISBMrkProcess>>();
				handler.SetMrkProcessEngine(engine);
				return handler;
			}
		}
	}
}
