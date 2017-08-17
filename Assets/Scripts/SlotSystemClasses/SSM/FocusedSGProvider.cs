using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class FocusedSGProvider: IFocusedSGProvider{
		ISlotSystemBundle poolBundle;
		ISlotSystemBundle equipBundle;
		IEnumerable<ISlotSystemBundle> otherBundles;
		ISlotSystemManager ssm;
		public FocusedSGProvider(ISlotSystemManager ssm){
			this.poolBundle = ssm.poolBundle;
			this.equipBundle = ssm.equipBundle;
			this.otherBundles = ssm.otherBundles;
			this.ssm = ssm;
		}
		public IEnumerable<ISlotGroup> focusedSGs{
			get{
				yield return focusedSGP;
				foreach(var sge in focusedSGEs)
					yield return sge;
				foreach(var sgg in focusedSGGs)
					yield return sgg;
			}
		}
		public ISlotGroup focusedSGP{
			get{
				if(poolBundle != null){
					ISlotSystemElement focusedEle = poolBundle.GetFocusedElement();
					if(focusedEle != null){
						ISlotGroup result = focusedEle as ISlotGroup;
						if(result != null)
							return result;
						else
							throw new System.InvalidOperationException("SlotSystemManger.focusedSGP: poolBundle.focusedElement is not of valid type or substitute with null");
					}else
						throw new System.InvalidOperationException("SlotSystemManager.focusedSGP: poolBundle.focusedElement is not set");
				}else
					throw new System.InvalidOperationException("SlotSystemManager.focusedSGP: poolBundle is not set");
			}
		}
		public List<ISlotGroup> focusedSGEs{
			get{
				if(focusedEqSet != null){
					List<ISlotGroup> result = new List<ISlotGroup>();
						foreach(ISlotSystemElement ele in focusedEqSet){
							if(ele != null)
								result.Add((ISlotGroup)ele);
						}
					if(result.Count != 0)
						return result;
					else
						throw new System.InvalidOperationException("SlotSystemManager.focusedSGEs: focusedEqSet empty");
				}else
					throw new System.InvalidOperationException("SlotSystemManager.focusedSGEs: focusedEqSet not set");
			}
		}
		public ISlotGroup focusedSGEBow{
			get{
				if(focusedSGEs != null){
					foreach(ISlotGroup sg in focusedSGEs){
						if(sg.filter is SGBowFilter)
							return sg;
					}
					throw new System.InvalidOperationException("SlotSystemManager.focusedSGEBow: there's no sg set with SGBowFilter in focusedSGEs");
				}
				throw new System.InvalidOperationException("SlotSystemManager.focusedSGEBow: focusedSGEs not set");
			}
		}
		public ISlotGroup focusedSGEWear{
			get{
				if(focusedSGEs != null){
					foreach(ISlotGroup sg in focusedSGEs){
						if(sg.filter is SGWearFilter)
							return sg;
					}
					throw new System.InvalidOperationException("SlotSystemManager.focusedSGEWear: there's no sg set with SGWearFilter in focusedSGEs");
				}
				throw new System.InvalidOperationException("SlotSystemManager.focusedSGEWear: focusedSGEs not set");
			}
		}
		public ISlotGroup focusedSGECGears{
			get{
				if(focusedSGEs != null){
					foreach(ISlotGroup sg in focusedSGEs){
						if(sg.filter is SGCGearsFilter)
							return sg;
					}
					throw new System.InvalidOperationException("SlotSystemManager.focusedSGECGears: there's no sg set with SGCGearsFilter in focusedSGEs");
				}
				throw new System.InvalidOperationException("SlotSystemManager.focusedSGECGears: focusedSGEs not set");
			}
		}
		public List<ISlotGroup> focusedSGGs{
			get{
				List<ISlotGroup> res = new List<ISlotGroup>();
				foreach(ISlotSystemBundle gBundle in otherBundles){
					gBundle.PerformInHierarchy(AddFocusedSGTo, res);
				}
				return res;
			}
		}
		public void AddFocusedSGTo(ISlotSystemElement ele, IList<ISlotGroup> list){
			if(ele is ISlotGroup && ele.IsFocusedInHierarchy())
				list.Add((ISlotGroup)ele);
		}
		public IEquipmentSet focusedEqSet{
			get{
				if(equipBundle != null){
					ISlotSystemElement focEle = equipBundle.GetFocusedElement();
					if(focEle != null){
						IEquipmentSet result = focEle as IEquipmentSet;
						if(result != null) return result;
						throw new System.InvalidOperationException("SlotSystemManger.focusedEqSet: equipBundle.focusedElement is not of valid type or substitute with null");
					}
					else
						throw new System.InvalidOperationException("SlotSystemManager.focusedEqpSet: equipBundle.focusedElement is not set");
				}else
					throw new System.InvalidOperationException("SlotSystemManager.focusedEqSet: equipBundle is not set");
				
			}
		}
		public IPoolInventory poolInv{
			get{
				if(focusedSGP != null){
					if(focusedSGP.inventory != null)
						return (IPoolInventory)focusedSGP.inventory;
					throw new System.InvalidOperationException("SlotSystemManager.poolInv: focusedSGP.inventory is not set");
				}
				throw new System.InvalidOperationException("SlotSystemManager.poolInv: focusedSGP is not set");
			}
		}
		public IEquipmentSetInventory equipInv{
			get{
				foreach(ISlotGroup sge in focusedSGEs){
					if(sge != null){
						IEquipmentSetInventory result = (IEquipmentSetInventory)sge.inventory;
						if(result == null)
							throw new System.InvalidOperationException("SlotSystemManager.equipInv: someSGEs not set with an inv is found");
						else return result;
					}
				}
				return null;
			}
		}
		public void ChangeEquippableCGearsCount(int i, ISlotGroup targetSG){
			if(!targetSG.isExpandable){
				ISSESelStateHandler targetSGSelStateHandler = targetSG.GetSelStateHandler();
				if(targetSGSelStateHandler.isFocused || targetSGSelStateHandler.isDefocused){
					equipInv.SetEquippableCGearsCount(i);
					targetSG.InitializeItems();
					ssm.UpdateEquipInvAndAllSBsEquipState();
				}
			}else{
				throw new System.InvalidOperationException("ISlotGroupManager.ChangeEquippableCGearsCount: the targetSG is expandable");
			}
		}
	}
	public interface IFocusedSGProvider{
		IEnumerable<ISlotGroup> focusedSGs{get;}
		ISlotGroup focusedSGP{get;}
		List<ISlotGroup> focusedSGEs{get;}
		ISlotGroup focusedSGEBow{get;}
		ISlotGroup focusedSGEWear{get;}
		ISlotGroup focusedSGECGears{get;}
		List<ISlotGroup> focusedSGGs{get;}
		IPoolInventory poolInv{get;}
		IEquipmentSetInventory equipInv{get;}
		void ChangeEquippableCGearsCount(int i, ISlotGroup targetSG);
	}
}
