using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace UISystem{
	public class Hoverable : IHoverable{
		public Hoverable(ISlotSystemElement sse){
			SetSSE(sse);
			SetSSM(sse.GetSSM());
		}
		public void SetSSM(ISlotSystemManager ssm){
			_ssm = ssm;
		}
		ISlotSystemManager _ssm;
		public ISlotSystemManager GetSSM(){
			return _ssm;
		}
		public void SetSSE(ISlotSystemElement sse){
			_sse = sse;
		}
		ISlotSystemElement _sse;
		public virtual void OnHoverEnter(){
			if(!_sse.IsSelStateNull()){
				if(!_sse.IsDeactivated() && !_sse.IsSelected())
					SetHovered();
				else
					throw new InvalidOperationException("sse needs to be activated and deselected");
			}else
				throw new InvalidOperationException("sse sel state not set");
		}
		public virtual void OnHoverExit(){
			if(!_sse.IsSelStateNull()){
				if(!_sse.IsDeactivated()){
					if(IsHovered())
						UnsetHovered();
					else
						throw new InvalidOperationException("hoverable is not set hovered");
				}else
					throw new InvalidOperationException("sse is deactivated");
			}else
				throw new InvalidOperationException("sse' sel state not set");
		}
		public virtual bool IsHovered(){
			return GetSSM().GetHovered() == this;
		}
		void SetHovered(){
			GetSSM().SetHovered(this);
		}
		void UnsetHovered(){
			GetSSM().SetHovered(null);
		}
	}
	public interface IHoverable{
		bool IsHovered();
		void OnHoverEnter();
		void OnHoverExit();
	}
}
