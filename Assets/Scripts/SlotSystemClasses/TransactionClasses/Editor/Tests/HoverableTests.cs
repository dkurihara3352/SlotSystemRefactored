﻿using System.Collections;
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
			Hoverable hoverable;
				ISlotSystemElement sse = MakeSubSSE();
					sse.isSelStateNull.Returns(true);
				hoverable = new Hoverable(sse, MakeSubTAC());

			hoverable.OnHoverEnter();
		}
		[Test]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverEnter_SSEIsDeactivated_ThrowsException(){
			Hoverable hoverable;
				ISlotSystemElement sse = MakeSubSSE();
					sse.isDeactivated.Returns(true);
				hoverable = new Hoverable(sse, MakeSubTAC());

			hoverable.OnHoverEnter();
		}
		[Test]
		[ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverEnter_SSEIsSelected_ThrowsException(){
			Hoverable hoverable;
				ISlotSystemElement sse = MakeSubSSE();
					sse.isSelected.Returns(true);
				hoverable = new Hoverable(sse, MakeSubTAC());

			hoverable.OnHoverEnter();
		}
		[Test]
		public void OnHoverEnter_SSEIsFocused_CallsTAMSetHoveredThis(){
			Hoverable hoverable;
				ISlotSystemElement stubSSE = MakeSubSSE();
					stubSSE.isFocused.Returns(true);
					ITransactionCache mockTAC = MakeSubTAC();
				hoverable = new Hoverable(stubSSE, mockTAC);
			
			hoverable.OnHoverEnter();

			mockTAC.Received().SetHovered(hoverable);
		}
		[Test]
		public void OnHoverEnter_SSEIsDefocused_CallsTAMSetHoveredThis(){
			Hoverable hoverable;
				ISlotSystemElement stubSSE = MakeSubSSE();
					stubSSE.isDefocused.Returns(true);
					ITransactionCache mockTAC = MakeSubTAC();
				hoverable = new Hoverable(stubSSE, mockTAC);
			
			hoverable.OnHoverEnter();

			mockTAC.Received().SetHovered(hoverable);
		}
		
		[Test][ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverExit_SSEIsSelStateNull_ThrowsException(){
			Hoverable hoverable;
				ISlotSystemElement stubSSE = MakeSubSSE();
					stubSSE.isSelStateNull.Returns(true);
			hoverable = new Hoverable(stubSSE, MakeSubTAC());

			hoverable.OnHoverExit();
		}
		[Test][ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverExit_SSEIsDeactivated_ThrowsException(){
			Hoverable hoverable;
				ISlotSystemElement stubSSE = MakeSubSSE();
					stubSSE.isDeactivated.Returns(true);
			hoverable = new Hoverable(stubSSE, MakeSubTAC());

			hoverable.OnHoverExit();
		}
		[Test][ExpectedException(typeof(System.InvalidOperationException))]
		public void OnHoverExit_IsNotHovered_ThrowsException(){
			Hoverable hoverable;
				ISlotSystemElement stubSSE = MakeSubSSE();
					stubSSE.isDeactivated.Returns(false);
					ITransactionCache stubTAC = MakeSubTAC();
				hoverable = new Hoverable(stubSSE, stubTAC);
					stubTAC.hovered.Returns((IHoverable)null);

			hoverable.OnHoverExit();
		}
		[Test]
		public void OnHoverExit_IsHoveredAndSSEIsNotDeactivated_CallsTAMOnHoverExitNull(){
			IHoverable hoverable;
				ISlotSystemElement stubSSE = MakeSubSSE();
					stubSSE.isFocused.Returns(true);
					stubSSE.isDeactivated.Returns(false);
					ITransactionCache mockTAC = MakeSubTAC();
				hoverable = new Hoverable(stubSSE, mockTAC);
					mockTAC.hovered.Returns(hoverable);
			
			hoverable.OnHoverExit();
			
			mockTAC.Received().SetHovered((IHoverable)null);
		}
	}
}