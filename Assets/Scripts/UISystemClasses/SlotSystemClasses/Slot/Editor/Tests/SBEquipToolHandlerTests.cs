using UnityEngine;
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
		public class SBEquipToolHandlerTests : SlotSystemTest{
			[Test]
			public void UpdateEquipState_IsEquipped_Calls_Equip(){
				SBEquipToolHandler sbEquipToolHandler = new SBEquipToolHandler(MakeSubSB(), Substitute.For<ISGEquipToolHandler>());
					ISBEqpStateHandler eqpStateHandler = Substitute.For<ISBEqpStateHandler>();
						eqpStateHandler.IsEquipped().Returns(true);
					sbEquipToolHandler.SetEqpStateHandler(eqpStateHandler);

				sbEquipToolHandler.UpdateEquipState();

				eqpStateHandler.DidNotReceive().Equip();
			}
			[Test]
			public void UpdateEquipState_IsNotEquipped_Calls_Unequip(){
				SBEquipToolHandler sbEquipToolHandler = new SBEquipToolHandler(MakeSubSB(), Substitute.For<ISGEquipToolHandler>());
					ISBEqpStateHandler eqpStateHandler = Substitute.For<ISBEqpStateHandler>();
						eqpStateHandler.IsEquipped().Returns(false);
					sbEquipToolHandler.SetEqpStateHandler(eqpStateHandler);

				sbEquipToolHandler.UpdateEquipState();

				eqpStateHandler.DidNotReceive().Unequip();
			}
		}
	}
}
