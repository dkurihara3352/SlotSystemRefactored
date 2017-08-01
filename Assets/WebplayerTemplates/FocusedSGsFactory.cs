using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class FocusedSGsFactory: IFocusedSGsFactory{
		ISlotSystemManager ssm;
		public FocusedSGsFactory(ISlotSystemManager ssm){this.ssm = ssm;}
		public List<ISlotGroup> focusedSGs{
			get{
				List<ISlotGroup> result = new List<ISlotGroup>();
				result.Add(ssm.focusedSGP);
				result.AddRange(ssm.focusedSGEs);
				result.AddRange(ssm.focusedSGGs);
				return result;
			}
		}
	}
	public interface IFocusedSGsFactory{
		List<ISlotGroup> focusedSGs{get;}
	}
}
