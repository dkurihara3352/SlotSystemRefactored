using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace SlotSystem{
	public class Hoverable : IHoverable{
		ISSESelStateHandler sseSelStateHandler{
			get{
				if(_sseSelStateHandler != null)
					return _sseSelStateHandler;
				else 
					throw new InvalidOperationException("sseSelStateHandler not set");
			}
		}
			ISSESelStateHandler _sseSelStateHandler;
			public void SetSSESelStateHandler(ISSESelStateHandler handler){
				_sseSelStateHandler = handler;
			}
		public ITransactionCache GetTAC(){
			return _taCache;
		}
		public void SetTACache(ITransactionCache taCache){
			_taCache = taCache;
		}
			ITransactionCache _taCache;
		public Hoverable(ITransactionCache taCache){
			_taCache = taCache;
		}
		public virtual void OnHoverEnter(){
			if(!sseSelStateHandler.IsSelStateNull()){
				if(!sseSelStateHandler.IsDeactivated() && !sseSelStateHandler.IsSelected())
					SetHovered();
				else
					throw new InvalidOperationException("sse needs to be activated and deselected");
			}else
				throw new InvalidOperationException("sse sel state not set");
		}
		public virtual void OnHoverExit(){
			if(!sseSelStateHandler.IsSelStateNull()){
				if(!sseSelStateHandler.IsDeactivated()){
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
			return GetTAC().GetHovered() == this;
		}
		void SetHovered(){
			GetTAC().SetHovered(this);
		}
		void UnsetHovered(){
			GetTAC().SetHovered(null);
		}
	}
	public interface IHoverable{
		void SetTACache(ITransactionCache taCache);
		void SetSSESelStateHandler(ISSESelStateHandler handler);
		bool IsHovered();
		void OnHoverEnter();
		void OnHoverExit();
	}
}
