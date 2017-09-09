﻿using UnityEngine;
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
		[TestFixture]
		public class SBActStateHandlerTests: SlotSystemTest {
			[Test]
			public void ExpireProcess_ActProcessNotNull_CallsProcExpire(){
				SBActStateHandler handler = new SBActStateHandler(MakeSubSB(), MakeSubTAM());
					ISBActProcess mockProc = Substitute.For<ISBActProcess>();
					IUIProcessEngine<ISBActProcess> stubEngine = Substitute.For<IUIProcessEngine<ISBActProcess>>();
					stubEngine.GetProcess().Returns(mockProc);
					handler.SetActProcessEngine(stubEngine);

				handler.ExpireActProcess();

				mockProc.Received().Expire();
			}
			[Test]
			public void SetActState_NullAndActProcNotNull_CallsProcEngineSetAndRunNull(){
				IUIProcessEngine<ISBActProcess> mockProcEngine;
				SBActStateHandler handler = MakeActStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.GetProcess().Returns(Substitute.For<ISBActProcess>());
				
				handler.SetActState(null);

				mockProcEngine.Received().SetAndRunProcess((ISBActProcess) null);
			}
			[Test]
			public void SetActState_NotNull_DoesNotCallsProcEngineSetAndRunNull(){
				IUIProcessEngine<ISBActProcess> mockProcEngine;
				SBActStateHandler handler = MakeActStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.GetProcess().Returns(Substitute.For<ISBActProcess>());
				
				handler.SetActState(Substitute.For<ISBActState>());

				mockProcEngine.DidNotReceive().SetAndRunProcess((ISBActProcess) null);
			}
			[Test]
			public void SetActState_NullAndActProcNull_DoesNotCallsProcEngineSetAndRunNull(){
				IUIProcessEngine<ISBActProcess> mockProcEngine;
				SBActStateHandler handler = MakeActStateHandlerWithProcEngine(out mockProcEngine);
				mockProcEngine.GetProcess().Returns((ISBActProcess)null);
				
				handler.SetActState((ISBActState)null);

				mockProcEngine.DidNotReceive().SetAndRunProcess((ISBActProcess) null);
			}
			/* Helper */
				SBActStateHandler MakeActStateHandlerWithProcEngine(out IUIProcessEngine<ISBActProcess> engine){
					SBActStateHandler handler = new SBActStateHandler(MakeSubSB(), MakeSubTAM());
					engine = Substitute.For<IUIProcessEngine<ISBActProcess>>();
					handler.SetActProcessEngine(engine);
					return handler;
				}
		}
	}
}