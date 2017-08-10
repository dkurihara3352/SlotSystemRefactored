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
	namespace SlotGroupTests{
		[TestFixture]
		public class SGActStateHandlerTests: SlotSystemTest {
			[Test]
			public void SetActState_ToNullAndActProcNotNull_CallsActProcEngineSetNull(){
				SGActStateHandler actStateHandler = new SGActStateHandler(MakeSubSG());
					ISSEProcessEngine<ISGActProcess> mockProcEngine = Substitute.For<ISSEProcessEngine<ISGActProcess>>();
					actStateHandler.SetActProcEngine(mockProcEngine);
					mockProcEngine.process.Returns(Substitute.For<ISGActProcess>());

				actStateHandler.SetActState(null);

				mockProcEngine.Received().SetAndRunProcess((ISGActProcess)null);
			}
		}
	}
}
