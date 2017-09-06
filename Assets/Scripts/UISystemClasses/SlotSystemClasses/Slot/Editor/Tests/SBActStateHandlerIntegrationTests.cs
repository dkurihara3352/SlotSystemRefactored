using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using UISystem;
using Utility;
namespace SlotSystemTests{
	namespace SlottableTests{
		[TestFixture]
		[Category("Integration")]
		public class SlottableIntegrationTests : SlotSystemTest{
			[Test]
			public void ActStateHandlerStates_ByDefault_AreNull(){
				ISBActStateHandler actStateHandler = new SBActStateHandler(MakeSubSB(), MakeSubTAM());

				Assert.That(actStateHandler.IsActStateNull(), Is.True);
				Assert.That(actStateHandler.WasActStateNull(), Is.True);
			}
			[Test]
			public void processes_ByDefault_AreNull(){
				ISBActStateHandler actStateHandler = new SBActStateHandler(MakeSubSB(), MakeSubTAM());

				Assert.That(actStateHandler.GetActProcess(), Is.Null);
			}
		/* Sequence */
			[Test]
			public void ActStates_EventSequence(){
				SBActStateHandler handler;
					ISlottable sb = Substitute.For<ISlottable>();
						IUISelStateHandler selStateHandler = Substitute.For<IUISelStateHandler>();
						sb.UISelStateHandler().Returns(selStateHandler);
						IItemHandler itemHandler = Substitute.For<IItemHandler>();
						sb.ItemHandler().Returns(itemHandler);
						IHoverable hoverable = Substitute.For<IHoverable>();
						sb.GetHoverable().Returns(hoverable);
					ITransactionManager tam = Substitute.For<ITransactionManager>();
					handler = new SBActStateHandler(sb, tam);
						sb.ActStateHandler().Returns(handler);
				PointerEventDataFake eventData = new PointerEventDataFake();
				/*	
					WFA IsWaitingForAction
					(IsFocused)
					Down IsWFPickUp
					(!(IStackable && IsHovered))
					Exp	IsPickingUp
					(!(IsHovered && IsStackable))
					Up ExecTA
				*/
					handler.WaitForAction();
						Assert.That(handler.IsWaitingForAction(), Is.True);
					
						selStateHandler.IsSelectable().Returns(true);
					handler.OnPointerDown(eventData);
						Assert.That(handler.IsWaitingForPickUp(), Is.True);
					
						itemHandler.IsStackable().Returns(true);
						hoverable.IsHovered().Returns(false);
					handler.ExpireActProcess();
						Assert.That(handler.IsPickingUp(), Is.True);
					
						hoverable.IsHovered().Returns(false);
					handler.OnPointerUp(eventData);
						tam.Received(1).ExecuteTransaction();
				/*
					WFA		IsWaitingForAction
					(IsFocused)
					Down	IsWaitingForPickingUp
					(IsStackable && IsHovered)
					Exp		IsPickingUp
					(IsStackable && IsHovered)
					Up		IsWaitingForNextTouch
					(IsPickedUp)
					Exp		ExecTA
				*/
					handler.WaitForAction();
						Assert.That(handler.IsWaitingForAction(), Is.True);

						selStateHandler.IsSelectable().Returns(true);
					handler.OnPointerDown(eventData);
						Assert.That(handler.IsWaitingForPickUp(), Is.True);
					
						itemHandler.IsStackable().Returns(true);
						hoverable.IsHovered().Returns(true);
					handler.ExpireActProcess();
						Assert.That(handler.IsPickingUp(), Is.True);

						itemHandler.IsStackable().Returns(true);
						hoverable.IsHovered().Returns(true);
					handler.OnPointerUp(eventData);
						Assert.That(handler.IsWaitingForNextTouch(), Is.True);

						sb.IsPickedUp().Returns(true);
					handler.ExpireActProcess();
						tam.Received(2).ExecuteTransaction();
				/*	
					WFA		IsWaitingForAction
					(IsFocused)
					Down	IsWFPickUp
					(IsStackable && IsHovered)
					Exp		IsPickingUp
					(IsStackable && IsHovered)
					Up		IsWaitingForNextTouch
					(IsPickedUp)
					Down	Increment
				*/
					handler.WaitForAction();

						selStateHandler.IsSelectable().Returns(true);
					handler.OnPointerDown(eventData);
						Assert.That(handler.IsWaitingForPickUp(), Is.True);
					
						itemHandler.IsStackable().Returns(true);
						hoverable.IsHovered().Returns(true);
					handler.ExpireActProcess();
						Assert.That(handler.IsPickingUp(), Is.True);

						itemHandler.IsStackable().Returns(true);
						hoverable.IsHovered().Returns(true);
					
					handler.OnPointerUp(eventData);
						Assert.That(handler.IsWaitingForNextTouch(), Is.True);

						sb.IsPickedUp().Returns(true);
					handler.OnPointerDown(eventData);
						sb.Received().Increment();
				/*
					WFA		IsWaitingForAction
					(IsFocused)
					Down	IsWaitingForPickUp
					Up		IsWaitingForNextTouch
					(!IsPickedUp)
					Down	IsPickingUp
				*/
					handler.WaitForAction();

						selStateHandler.IsSelectable().Returns(true);
					handler.OnPointerDown(eventData);
						Assert.That(handler.IsWaitingForPickUp(), Is.True);
					
					handler.OnPointerUp(eventData);
						Assert.That(handler.IsWaitingForNextTouch(), Is.True);

						sb.IsPickedUp().Returns(false);
					handler.OnPointerDown(eventData);
						Assert.That(handler.IsPickingUp(), Is.True);
				/*	
					WFA		IsWaitingForAction
					(!IsFocused)
					Down	IsWaitingForPointerUp
					Up		Tap Refresh Defocus
				*/
					handler.WaitForAction();
						Assert.That(handler.IsWaitingForAction(), Is.True);

						selStateHandler.IsSelectable().Returns(false);
					handler.OnPointerDown(eventData);
						Assert.That(handler.IsWaitingForPointerUp(), Is.True);
					
					handler.OnPointerUp(eventData);
						sb.Received().Tap();
						sb.Received().Refresh();
						selStateHandler.Received().MakeUnselectable();
			}
		/* helpers */
		}
	}
}

