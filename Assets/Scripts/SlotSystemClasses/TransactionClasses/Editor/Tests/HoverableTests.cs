using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using SlotSystem;
using Utility;

namespace SlotSystemTests{
	[TestFixture]
	[Category("OtherElements")]
	public class HoverableTests : SlotSystemTest{
		[Test]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverEnter_SSEIsSelStateNull_ThrowsException(){
			Hoverable hoverable = new Hoverable(MakeSubTAC());
				ISSESelStateHandler sseSelStateHandler = Substitute.For<ISSESelStateHandler>();
					sseSelStateHandler.isSelStateNull.Returns(true);
				hoverable.SetSSESelStateHandler(sseSelStateHandler);

			hoverable.OnHoverEnter();
		}
		[Test]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverEnter_SSEIsDeactivated_ThrowsException(){
			Hoverable hoverable = new Hoverable(MakeSubTAC());
				ISSESelStateHandler sseSelStateHandler = Substitute.For<ISSESelStateHandler>();
					sseSelStateHandler.isDeactivated.Returns(true);
				hoverable.SetSSESelStateHandler(sseSelStateHandler);

			hoverable.OnHoverEnter();
		}
		[Test]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverEnter_SSEIsSelected_ThrowsException(){
			Hoverable hoverable = new Hoverable(MakeSubTAC());
				ISSESelStateHandler sseSelStateHandler = Substitute.For<ISSESelStateHandler>();
					sseSelStateHandler.isSelected.Returns(true);
				hoverable.SetSSESelStateHandler(sseSelStateHandler);

			hoverable.OnHoverEnter();
		}
		[Test]
		public void OnHoverEnter_SSEIsFocused_CallsTAMSetHoveredThis(){
			Hoverable hoverable;
					ITransactionCache mockTAC = Substitute.For<ITransactionCache>();
				hoverable = new Hoverable(mockTAC);
					ISSESelStateHandler sseSelStateHandler = Substitute.For<ISSESelStateHandler>();
						sseSelStateHandler.isFocused.Returns(true);
				hoverable.SetSSESelStateHandler(sseSelStateHandler);
			
			hoverable.OnHoverEnter();

			mockTAC.Received().SetHovered(hoverable);
		}
		[Test]
		public void OnHoverEnter_SSEIsDefocused_CallsTAMSetHoveredThis(){
			Hoverable hoverable;
					ITransactionCache mockTAC = Substitute.For<ITransactionCache>();
				hoverable = new Hoverable(mockTAC);
					ISSESelStateHandler sseSelStateHandler = Substitute.For<ISSESelStateHandler>();
						sseSelStateHandler.isDefocused.Returns(true);
					hoverable.SetSSESelStateHandler(sseSelStateHandler);
			
			hoverable.OnHoverEnter();

			mockTAC.Received().SetHovered(hoverable);
		}
		
		[Test][ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverExit_SSEIsSelStateNull_ThrowsException(){
			Hoverable hoverable = new Hoverable(MakeSubTAC());
				ISSESelStateHandler sseSelStateHandler = Substitute.For<ISSESelStateHandler>();
					sseSelStateHandler.isSelStateNull.Returns(true);
				hoverable.SetSSESelStateHandler(sseSelStateHandler);

			hoverable.OnHoverExit();
		}
		[Test][ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverExit_SSEIsDeactivated_ThrowsException(){
			Hoverable hoverable = new Hoverable(MakeSubTAC());
				ISSESelStateHandler sseSelStateHandler = Substitute.For<ISSESelStateHandler>();
					sseSelStateHandler.isDeactivated.Returns(true);
				hoverable.SetSSESelStateHandler(sseSelStateHandler);

			hoverable.OnHoverExit();
		}
		[Test][ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverExit_IsNotHovered_ThrowsException(){
			Hoverable hoverable;
					ITransactionCache stubTAC = Substitute.For<ITransactionCache>();
					stubTAC.hovered.Returns((IHoverable)null);
				hoverable = new Hoverable(stubTAC);
					ISSESelStateHandler sseSelStateHandler = Substitute.For<ISSESelStateHandler>();
						sseSelStateHandler.isDeactivated.Returns(true);
					hoverable.SetSSESelStateHandler(sseSelStateHandler);

			hoverable.OnHoverExit();
		}
		[Test]
		public void OnHoverExit_IsHoveredAndSSEIsNotDeactivated_CallsTAMOnHoverExitNull(){
			Hoverable hoverable;
					ITransactionCache mockTAC = Substitute.For<ITransactionCache>();
				hoverable = new Hoverable(mockTAC);
					mockTAC.hovered.Returns(hoverable);
					ISSESelStateHandler sseSelStateHandler = Substitute.For<ISSESelStateHandler>();
						sseSelStateHandler.isDeactivated.Returns(false);
					hoverable.SetSSESelStateHandler(sseSelStateHandler);
			
			hoverable.OnHoverExit();
			
			mockTAC.Received().SetHovered((IHoverable)null);
		}
	}
}
