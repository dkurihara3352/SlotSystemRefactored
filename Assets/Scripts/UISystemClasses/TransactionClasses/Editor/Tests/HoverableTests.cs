﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using UISystem;
using Utility;

namespace SlotSystemTests{
	[TestFixture]
	[Category("OtherElements")]
	public class HoverableTests : SlotSystemTest{
		[Test]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverEnter_SSEIsSelStateNull_ThrowsException(){
			Hoverable hoverable = new Hoverable(MakeSubTAC());
				IUISelStateHandler sseSelStateHandler = Substitute.For<IUISelStateHandler>();
					sseSelStateHandler.IsSelStateNull().Returns(true);
				hoverable.SetSSESelStateHandler(sseSelStateHandler);

			hoverable.OnHoverEnter();
		}
		[Test]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverEnter_SSEIsDeactivated_ThrowsException(){
			Hoverable hoverable = new Hoverable(MakeSubTAC());
				IUISelStateHandler sseSelStateHandler = Substitute.For<IUISelStateHandler>();
					sseSelStateHandler.IsDeactivated().Returns(true);
				hoverable.SetSSESelStateHandler(sseSelStateHandler);

			hoverable.OnHoverEnter();
		}
		[Test]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverEnter_SSEIsSelected_ThrowsException(){
			Hoverable hoverable = new Hoverable(MakeSubTAC());
				IUISelStateHandler sseSelStateHandler = Substitute.For<IUISelStateHandler>();
					sseSelStateHandler.IsSelected().Returns(true);
				hoverable.SetSSESelStateHandler(sseSelStateHandler);

			hoverable.OnHoverEnter();
		}
		[Test]
		public void OnHoverEnter_SSEIsFocused_CallsTAMSetHoveredThis(){
			Hoverable hoverable;
					ITransactionCache mockTAC = Substitute.For<ITransactionCache>();
				hoverable = new Hoverable(mockTAC);
					IUISelStateHandler sseSelStateHandler = Substitute.For<IUISelStateHandler>();
						sseSelStateHandler.IsSelectable().Returns(true);
				hoverable.SetSSESelStateHandler(sseSelStateHandler);
			
			hoverable.OnHoverEnter();

			mockTAC.Received().SetHovered(hoverable);
		}
		[Test]
		public void OnHoverEnter_SSEIsDefocused_CallsTAMSetHoveredThis(){
			Hoverable hoverable;
					ITransactionCache mockTAC = Substitute.For<ITransactionCache>();
				hoverable = new Hoverable(mockTAC);
					IUISelStateHandler sseSelStateHandler = Substitute.For<IUISelStateHandler>();
						sseSelStateHandler.IsUnselectable().Returns(true);
					hoverable.SetSSESelStateHandler(sseSelStateHandler);
			
			hoverable.OnHoverEnter();

			mockTAC.Received().SetHovered(hoverable);
		}
		
		[Test][ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverExit_SSEIsSelStateNull_ThrowsException(){
			Hoverable hoverable = new Hoverable(MakeSubTAC());
				IUISelStateHandler sseSelStateHandler = Substitute.For<IUISelStateHandler>();
					sseSelStateHandler.IsSelStateNull().Returns(true);
				hoverable.SetSSESelStateHandler(sseSelStateHandler);

			hoverable.OnHoverExit();
		}
		[Test][ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverExit_SSEIsDeactivated_ThrowsException(){
			Hoverable hoverable = new Hoverable(MakeSubTAC());
				IUISelStateHandler sseSelStateHandler = Substitute.For<IUISelStateHandler>();
					sseSelStateHandler.IsDeactivated().Returns(true);
				hoverable.SetSSESelStateHandler(sseSelStateHandler);

			hoverable.OnHoverExit();
		}
		[Test][ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverExit_IsNotHovered_ThrowsException(){
			Hoverable hoverable;
					ITransactionCache stubTAC = Substitute.For<ITransactionCache>();
					stubTAC.GetHovered().Returns((IHoverable)null);
				hoverable = new Hoverable(stubTAC);
					IUISelStateHandler sseSelStateHandler = Substitute.For<IUISelStateHandler>();
						sseSelStateHandler.IsDeactivated().Returns(true);
					hoverable.SetSSESelStateHandler(sseSelStateHandler);

			hoverable.OnHoverExit();
		}
		[Test]
		public void OnHoverExit_IsHoveredAndSSEIsNotDeactivated_CallsTAMOnHoverExitNull(){
			Hoverable hoverable;
					ITransactionCache mockTAC = Substitute.For<ITransactionCache>();
				hoverable = new Hoverable(mockTAC);
					mockTAC.GetHovered().Returns(hoverable);
					IUISelStateHandler sseSelStateHandler = Substitute.For<IUISelStateHandler>();
						sseSelStateHandler.IsDeactivated().Returns(false);
					hoverable.SetSSESelStateHandler(sseSelStateHandler);
			
			hoverable.OnHoverExit();
			
			mockTAC.Received().SetHovered((IHoverable)null);
		}
	}
}