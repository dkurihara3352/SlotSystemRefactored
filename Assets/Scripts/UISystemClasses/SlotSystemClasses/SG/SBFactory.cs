using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
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
			IUISelStateHandler sbSelStateHandler = newSB.UISelStateHandler();
			sbSelStateHandler.MakeUnselectable();
			return newSB;
		}
		public List<ISlottable> CreateSBs(IEnumerable<IInventoryItemInstance> items){
			List<ISlottable> result = new List<ISlottable>();
			foreach(var item in items)
				if(item != null)
					result.Add(CreateSB(item));
				else
					result.Add(null);
			return result;
		}
	}
	public interface ISBFactory{
		ISlottable CreateSB(IInventoryItemInstance item);
		List<ISlottable> CreateSBs(IEnumerable<IInventoryItemInstance> items);
	}
}
