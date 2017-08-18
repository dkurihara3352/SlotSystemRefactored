using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections;
using System.Collections.Generic;
using NSubstitute;

namespace SlotSystemTests{
	namespace SGTests{
		[TestFixture]
		[Category("SG")]
		public class SGStatesTests: SlotSystemTest{
			/*	ActState	*/
				[Test]
				public void SGWaitForActionState_EnterState_WhenCalled_SetsSGActProcNull(){
					ISlotGroup mockSG = MakeSubSG();
					SGWaitForActionState sgwfaState = new SGWaitForActionState(mockSG);

					sgwfaState.EnterState();

					mockSG.Received().SetAndRunActProcess(null);
				}
				[Test]
				public void SGRevertState_EnterState_WhenCalled_CallsSGUpdateToRevert(){
					ISlotGroup mockSG = MakeSubSG();
					SGRevertState revState = new SGRevertState(mockSG);

					revState.EnterState();

					mockSG.Received().RevertAndUpdateSBs();
				}
		}
	}
}
