using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace UISystem{
	public class SBEquipToolHandler : ISBEquipToolHandler {
		public ISlottable GetSB(){
			Debug.Assert(sb != null);
			return sb;
		}
		ISlottable sb;
		ISGEquipToolHandler sgEquipToolHandler;
		public SBEquipToolHandler(ISlottable sb, ISGEquipToolHandler sgEquipToolHandler){
			this.sb = sb;
			this.sgEquipToolHandler = sgEquipToolHandler;
			SetEqpStateHandler(new SBEqpStateHandler(GetSB()));
		}
		public void InitializeStates(){
			ClearCurEqpState();
		}
		public ISBEqpStateHandler GetEqpStateHandler(){
			if(_eqpStateHandler != null)
				return _eqpStateHandler;
			else
				throw new InvalidOperationException("eqpStateHandler not set");
		}
		public void SetEqpStateHandler(ISBEqpStateHandler handler){
			_eqpStateHandler = handler;
		}
			ISBEqpStateHandler _eqpStateHandler;
		public bool IsEquipped(){
			return GetEqpStateHandler().IsEquipped();
		}
		public bool IsUnequipped(){
			return GetEqpStateHandler().IsUnequipped();
		}
		public void Equip(){
			GetEqpStateHandler().Equip();
		}
		public void Unequip(){
			GetEqpStateHandler().Unequip();
		}
		public void ClearCurEqpState(){
			GetEqpStateHandler().ClearCurEqpState();
		}
		public bool IsPool(){
			Debug.Assert(sgEquipToolHandler != null);
			return sgEquipToolHandler.IsPool();
		}
		public void UpdateEquipState(){
			if(IsEquipped()) Equip();
			else Unequip();
		}
	}
	public interface ISBEquipToolHandler: ISBToolHandler{
		ISlottable GetSB();
		ISBEqpStateHandler GetEqpStateHandler();
			bool IsEquipped();
			bool IsUnequipped();
			void Equip();
			void Unequip();
			void ClearCurEqpState();
			void UpdateEquipState();
			bool IsPool();

	}
	public interface ISBToolHandler{
		void InitializeStates();
	}
}
