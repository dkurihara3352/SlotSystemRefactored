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
			if(!sse.isSelStateNull){
				if(!sse.isDeactivated && !sse.isSelected)
					SetHovered();
				else
					throw new InvalidOperationException("sse needs to be activated and deselected");
			}else
				throw new InvalidOperationException("sse sel state not set");
		}
		public virtual void OnHoverExit(){
			if(!sse.isSelStateNull){
				if(!sse.isDeactivated){
					if(isHovered)
						UnsetHovered();
					else
						throw new InvalidOperationException("hoverable is not set hovered");
				}else
					throw new InvalidOperationException("sse is deactivated");
			}else
				throw new InvalidOperationException("sse' sel state not set");
		}
		public virtual bool isHovered{
			get{return tam.hovered == this;}
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
