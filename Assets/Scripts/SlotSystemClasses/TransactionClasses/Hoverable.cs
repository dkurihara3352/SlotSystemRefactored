using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace SlotSystem{
	public class Hoverable : IHoverable{
		ISlotSystemElement sse;
		ITransactionManager tam;
		public Hoverable(ISlotSystemElement sse, ITransactionManager tam){
			this.sse = sse;
			this.tam = tam;
		}
		public virtual void OnHoverEnter(){
			if(!sse.isDeactivated && !sse.isSelected)
				SetHovered();
		}
		public virtual void OnHoverExit(){
			if(!sse.isDeactivated)
				if(isHovered)
					UnsetHovered();
				else
					throw new InvalidOperationException("hoverable is not set hovered");
		}
		public virtual bool isHovered{
			get{return tam.hovered == (ISlotSystemElement)this;}
		}
		public void SetHovered(){
			tam.SetHovered(this);
		}
		public void UnsetHovered(){
			tam.SetHovered(null);
		}
	}
	public interface IHoverable{
		bool isHovered{get;}
		void OnHoverEnter();
		void OnHoverExit();
		void SetHovered();
		void UnsetHovered();
	}
}
