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
			this.poolBundle = ssm.GetPoolBundle();
			this.equipBundle = ssm.GetEquipBundle();
			this.otherBundles = ssm.GetOtherBundles();
			this.ssm = ssm;
		}
		public IEnumerable<ISlotGroup> GetFocusedSGs(){
			yield return focusedSGP;
			foreach(var sge in focusedSGEs)
				yield return sge;
			foreach(var sgg in focusedSGGs)
				yield return sgg;
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
		public ISlotGroup GetFocusedSGEBow(){
			if(focusedSGEs != null){
				foreach(ISlotGroup sg in focusedSGEs){
					IFilterHandler filterHandler = sg.GetFilterHandler();
					if(filterHandler.GetFilter() is SGBowFilter)
						return sg;
				}
				throw new System.InvalidOperationException("SlotSystemManager.focusedSGEBow: there's no sg set with SGBowFilter in focusedSGEs");
			}
			throw new System.InvalidOperationException("SlotSystemManager.focusedSGEBow: focusedSGEs not set");
		}
		public ISlotGroup GetFocusedSGEWear(){
			if(focusedSGEs != null){
				foreach(ISlotGroup sg in focusedSGEs){
					IFilterHandler filterHandler = sg.GetFilterHandler();
					if(filterHandler.GetFilter() is SGWearFilter)
						return sg;
				}
				throw new System.InvalidOperationException("SlotSystemManager.focusedSGEWear: there's no sg set with SGWearFilter in focusedSGEs");
			}
			throw new System.InvalidOperationException("SlotSystemManager.focusedSGEWear: focusedSGEs not set");
		}
		public ISlotGroup GetFocusedSGECGears(){
			if(focusedSGEs != null){
				foreach(ISlotGroup sg in focusedSGEs){
					IFilterHandler filterHandler = sg.GetFilterHandler();
					if(filterHandler.GetFilter() is SGCGearsFilter)
						return sg;
				}
				throw new System.InvalidOperationException("SlotSystemManager.focusedSGECGears: there's no sg set with SGCGearsFilter in focusedSGEs");
			}
			throw new System.InvalidOperationException("SlotSystemManager.focusedSGECGears: focusedSGEs not set");
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
		public IPoolInventory GetPoolInv(){
			if(focusedSGP != null){
				IInventory inventory = focusedSGP.GetInventory();
				if(inventory != null)
					return (IPoolInventory)inventory;
				throw new System.InvalidOperationException("SlotSystemManager.poolInv: focusedSGP.inventory is not set");
			}
			throw new System.InvalidOperationException("SlotSystemManager.poolInv: focusedSGP is not set");
		}
		public IEquipmentSetInventory GetEquipInv(){
			foreach(ISlotGroup sge in focusedSGEs){
				if(sge != null){
					IEquipmentSetInventory result = (IEquipmentSetInventory)sge.GetInventory();
					if(result == null)
						throw new System.InvalidOperationException("SlotSystemManager.equipInv: someSGEs not set with an inv is found");
					else return result;
				}
			}
			return null;
		}
		public void ChangeEquippableCGearsCount(int i, ISlotGroup targetSG){
			if(!targetSG.IsExpandable()){
				ISSESelStateHandler targetSGSelStateHandler = targetSG.GetSelStateHandler();
				if(targetSGSelStateHandler.IsFocused() || targetSGSelStateHandler.IsDefocused()){
					GetEquipInv().SetEquippableCGearsCount(i);
					targetSG.InitializeItems();
					ssm.UpdateEquipInvAndAllSBsEquipState();
				}
			}else{
				throw new System.InvalidOperationException("ISlotGroupManager.ChangeEquippableCGearsCount: the targetSG is expandable");
			}
		}
	}
	public interface IFocusedSGProvider{
		IEnumerable<ISlotGroup> GetFocusedSGs();
		ISlotGroup GetFocusedSGEBow();
		ISlotGroup GetFocusedSGEWear();
		ISlotGroup GetFocusedSGECGears();
		IPoolInventory GetPoolInv();
		IEquipmentSetInventory GetEquipInv();
		void ChangeEquippableCGearsCount(int i, ISlotGroup targetSG);
	}
}
