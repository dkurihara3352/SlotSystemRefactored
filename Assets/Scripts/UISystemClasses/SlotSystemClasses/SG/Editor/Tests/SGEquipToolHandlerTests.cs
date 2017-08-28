using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using UISystem;
using Utility;
namespace SlotSystemTests{
	namespace SlotGroupTests{
		[TestFixture]
		public class SGEquipToolHandlerTests :SlotSystemTest{
			[TestCaseSource(typeof(EquippedSBsCases))]
			public void GetEquippedSBs_Always_ReturnsAllSBsIsEquipped(List<IUIElement> sbs, List<ISlottable> expected){
				SGEquipToolHandler sgEquipToolHandler;
					ISlotGroup sg = MakeSubSG();
						sg.GetEnumerator().Returns(sbs.GetEnumerator());
					sgEquipToolHandler = new SGEquipToolHandler(sg, Substitute.For<IEquipManager>());

				List<ISlottable> actual = sgEquipToolHandler.GetEquippedSBs();

				bool equality = actual.MemberEquals(expected);
				Assert.That(equality, Is.True);
			}
				class EquippedSBsCases: IEnumerable{
					public IEnumerator GetEnumerator(){
						ISlottable eSBA = MakeSubSB();
						ISlottable eSBB = MakeSubSB();
						ISlottable eSBC = MakeSubSB();
						ISlottable uSBA = MakeSubSB();
						ISlottable uSBB = MakeSubSB();
						ISlottable uSBC = MakeSubSB();
						ISBEquipToolHandler eSBAEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
						ISBEquipToolHandler eSBBEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
						ISBEquipToolHandler eSBCEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
						ISBEquipToolHandler uSBAEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
						ISBEquipToolHandler uSBBEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
						ISBEquipToolHandler uSBCEquipToolHandler = Substitute.For<ISBEquipToolHandler>();
						eSBAEquipToolHandler.IsEquipped().Returns(true);
						eSBBEquipToolHandler.IsEquipped().Returns(true);
						eSBCEquipToolHandler.IsEquipped().Returns(true);
						uSBAEquipToolHandler.IsEquipped().Returns(false);
						uSBBEquipToolHandler.IsEquipped().Returns(false);
						uSBCEquipToolHandler.IsEquipped().Returns(false);
						eSBA.GetToolHandler().Returns(eSBAEquipToolHandler);
						eSBB.GetToolHandler().Returns(eSBBEquipToolHandler);
						eSBC.GetToolHandler().Returns(eSBCEquipToolHandler);
						uSBA.GetToolHandler().Returns(uSBAEquipToolHandler);
						uSBB.GetToolHandler().Returns(uSBBEquipToolHandler);
						uSBC.GetToolHandler().Returns(uSBCEquipToolHandler);
						List<ISlottable> case1SBs = new List<ISlottable>(new ISlottable[]{
							eSBA, eSBB, eSBC, uSBA, uSBB, uSBC
						});
						List<ISlottable> case1Exp = new List<ISlottable>(new ISlottable[]{
							eSBA, eSBB, eSBC
						});
						yield return new object[]{case1SBs, case1Exp};
					}
				}

		}
	}
}
