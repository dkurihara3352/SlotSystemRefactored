using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class AllElementsProvider : IAllElementsProvider {
		ISlotSystemManager ssm;
		public AllElementsProvider(ISlotSystemManager ssm){
			this.ssm = ssm;
		}
		public List<ISlotGroup> allSGs{
			get{
				List<ISlotGroup> result = new List<ISlotGroup>();
				result.AddRange(allSGPs);
				result.AddRange(allSGEs);
				result.AddRange(allSGGs);
				return result;
			}
		}
		public List<ISlotGroup> allSGPs
		{
			get{
				List<ISlotGroup> result = new List<ISlotGroup>();
				ssm.poolBundle.PerformInHierarchy(AddInSGList, result);
				return result;
			}
		}
		public List<ISlotGroup> allSGEs{
			get{
				List<ISlotGroup> result = new List<ISlotGroup>();
				ssm.equipBundle.PerformInHierarchy(AddInSGList, result);
				return result;
			}
		}
		public List<ISlotGroup> allSGGs{
			get{
				List<ISlotGroup> result = new List<ISlotGroup>();
				foreach(ISlotSystemBundle gBun in ssm.otherBundles){
					gBun.PerformInHierarchy(AddInSGList, result);
				}
				return result;
			}
		}
		public void AddInSGList(ISlotSystemElement ele, IList<ISlotGroup> sgs){
			if(ele is ISlotGroup)
			sgs.Add((ISlotGroup)ele);
		}
		public List<ISlottable> allSBs{
			get{
				List<ISlottable> res = new List<ISlottable>();
				ssm.PerformInHierarchy(AddSBToRes, res);
				return res;
			}
		}
		public void AddSBToRes(ISlotSystemElement ele, IList<ISlottable> list){
			if(ele is ISlottable)
				list.Add((ISlottable)ele);
		}
	}
	public interface IAllElementsProvider{
		List<ISlotGroup> allSGs{get;}
		List<ISlottable> allSBs{get;}
	}
}
