using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Utility;
namespace SlotSystemTests{
	[TestFixture]
	[Category("TAM")]
	public class SortEngineTests : SlotSystemTest{
			[Test]
			public void SortSG_WhenCalled_CallsSortFAMakeSortTA(){
				SortEngine sortEngine = new SortEngine(MakeSubTAM(), MakeSubSGHandler());
				ISortTransactionFactory sortFA = Substitute.For<ISortTransactionFactory>();
					ISortTransaction stubSortTA = Substitute.For<ISortTransaction>();
					sortFA.MakeSortTA(Arg.Any<ISlotGroup>(), Arg.Any<SGSorter>()).Returns(stubSortTA);
					sortEngine.SetSortTransactionFactory(sortFA);
				ISlotGroup sg = MakeSubSG();
				SGSorter sorter = new SGItemIDSorter();

				sortEngine.SortSG(sg, sorter);
				
				sortFA.Received().MakeSortTA(sg, sorter);
				}
			[Test]
			public void SortSG_WhenCalled_CallsTAMMethods(){
				ITransactionManager mockTAM = MakeSubTAM();
				ITransactionCache mockTAC = MakeSubTAC();
				ITransactionSGHandler mockSGHd = MakeSubSGHandler();
				SortEngine sortEngine = new SortEngine(mockTAM, mockSGHd);
					ISlotGroup stubSG = MakeSubSG();
						IHoverable hoverable = Substitute.For<IHoverable>();
						stubSG.GetHoverable().Returns(hoverable);
						stubSG.GetTAC().Returns(mockTAC);
					SGSorter stubSorter = Substitute.For<SGSorter>();
					ISortTransactionFactory stubSortTAFactory = Substitute.For<ISortTransactionFactory>();
						ISortTransaction stubTA = Substitute.For<ISortTransaction>();
							ISlottable stubTargetSB = MakeSubSB();
							ISlotGroup stubSG1 = MakeSubSG();
							stubTA.GetTargetSB().Returns(stubTargetSB);
							stubTA.GetSG1().Returns(stubSG1);
					stubSortTAFactory.MakeSortTA(stubSG, stubSorter).Returns(stubTA);
					sortEngine.SetSortTransactionFactory(stubSortTAFactory);
				
				sortEngine.SortSG(stubSG, stubSorter);

				Received.InOrder(()=>{
					stubTA.SetTargetSBOnTAC();
					mockSGHd.SetSG1(stubSG1);
					mockTAM.SetTransaction(stubTA);
					mockTAM.ExecuteTransaction();
				});
			}	
	}
}
