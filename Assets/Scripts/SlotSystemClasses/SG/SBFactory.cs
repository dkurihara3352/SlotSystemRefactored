﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class SBFactory : ISBFactory {
		ISlotSystemManager ssm{
			get{
				if(_ssm != null)
					return _ssm;
				else
					throw new System.InvalidOperationException("ssm not set");
			}
		}
			ISlotSystemManager _ssm;
			public void SetSSM(ISlotSystemManager ssm){
				_ssm = ssm;
			}
		public SBFactory(ISlotSystemManager ssm){
			SetSSM(ssm);
		}
		public ISlottable CreateSB(IInventoryItemInstance item){
			GameObject newSBGO = new GameObject("newSBGO");
			Slottable newSB = newSBGO.AddComponent<Slottable>();
			newSB.SetSSM(ssm);
			newSB.InitializeSB(item);
			ISSESelStateHandler sbSelStateHandler = newSB.GetSelStateHandler();
			sbSelStateHandler.Defocus();
			return newSB;
		}
	}
	public interface ISBFactory{
		ISlottable CreateSB(IInventoryItemInstance item);
	}
}
