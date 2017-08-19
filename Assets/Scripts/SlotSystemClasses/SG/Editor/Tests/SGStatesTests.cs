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
					ISlotGroup sg = MakeSubSG();
						ISGActStateHandler actStateHandler = Substitute.For<ISGActStateHandler>();
						sg.GetSGActStateHandler().Returns(actStateHandler);
					SGWaitForActionState sgwfaState = new SGWaitForActionState(sg);


					sgwfaState.EnterState();

					actStateHandler.Received().SetAndRunActProcess(null);
				}
				[Test]
				public void SGRevertState_EnterState_WhenCalled_CallsSGUpdateToRevert(){
					ISlotGroup sg = MakeSubSG();
						ISGActStateHandler actStateHandler = Substitute.For<ISGActStateHandler>();
						sg.GetSGActStateHandler().Returns(actStateHandler);
					SGRevertState revState = new SGRevertState(sg);

					revState.EnterState();

					sg.Received().RevertAndUpdateSBs();
				}
		}
	}
}
