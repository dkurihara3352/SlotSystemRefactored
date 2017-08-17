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
		public ITransactionCache taCache{
			get{return _taCache;}
		}
			ITransactionCache _taCache;
		public void SetTACache(ITransactionCache taCache){
			_taCache = taCache;
		}
		public Hoverable(ITransactionCache taCache){
			_taCache = taCache;
		}
		public virtual void OnHoverEnter(){
			if(!sseSelStateHandler.isSelStateNull){
				if(!sseSelStateHandler.isDeactivated && !sseSelStateHandler.isSelected)
					SetHovered();
				else
					throw new InvalidOperationException("sse needs to be activated and deselected");
			}else
				throw new InvalidOperationException("sse sel state not set");
		}
		public virtual void OnHoverExit(){
			if(!sseSelStateHandler.isSelStateNull){
				if(!sseSelStateHandler.isDeactivated){
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
			get{return taCache.hovered == this;}
		}
		void SetHovered(){
			taCache.SetHovered(this);
		}
		void UnsetHovered(){
			taCache.SetHovered(null);
		}
	}
	public interface IHoverable{
		ITransactionCache taCache{get;}
		void SetTACache(ITransactionCache taCache);
		void SetSSESelStateHandler(ISSESelStateHandler handler);
		bool isHovered{get;}
		void OnHoverEnter();
		void OnHoverExit();
	}
}
