using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using Utility;
using UISystem;
namespace SlotSystemTests{
	namespace SlottableTests{
		[TestFixture]
		public class SBCommandsTests: SlotSystemTest{
			[Test]
			public void SBPickUpEquipBowCommand_Execute_Always_Calls_FocusSGEBow(){
			}
			[Test]
			public void SBPickUpEquipWearCommand_Execute_Always_Calls_FocusSGEWear(){
			}
			[Test]
			public void SBPickUpEquipCGearsCommand_Execute_Always_Calls_FocusSGECGears(){
			}
		}
	}
}
