using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class FocusedSGProvider: IFocusedSGProvider{
		ISlotSystemBundle poolBundle{
			get{
				if(_poolBundle != null)
					return _poolBundle;
				else
					throw new InvalidOperationException("poolBundle not set");
			}
		}
			ISlotSystemBundle _poolBundle;
		ISlotSystemBundle equipBundle{
			get{
				if(_equipBundle != null)
					return _equipBundle;
				else
					throw new InvalidOperationException("equipBundle not set");
			}
		}
			ISlotSystemBundle _equipBundle;
		IEnumerable<ISlotSystemBundle> otherBundles;
		ISlotSystemManager ssm;
		public FocusedSGProvider(ISlotSystemManager ssm){
			_poolBundle = ssm.GetPoolBundle();
			_equipBundle = ssm.GetEquipBundle();
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
				ISlotSystemElement focusedEle = poolBundle.GetFocusedElement();
				ISlotGroup result = focusedEle as ISlotGroup;
				if(result != null)
					return result;
				else
					throw new InvalidOperationException("focusedEle not of type ISlotGroup");
			}
		}
		public List<ISlotGroup> focusedSGEs{
			get{
				List<ISlotGroup> result = new List<ISlotGroup>();
					foreach(ISlotSystemElement ele in focusedEqSet){
						if(ele != null)
							result.Add((ISlotGroup)ele);
					}
				if(result.Count != 0)
					return result;
				else
					throw new InvalidOperationException("focusedEqSet is empty");
			}
		}
		public ISlotGroup GetFocusedSGEBow(){
			foreach(ISlotGroup sg in focusedSGEs){
				IFilterHandler filterHandler = sg.GetFilterHandler();
				if(filterHandler.GetFilter() is SGBowFilter)
					return sg;
			}
			throw new InvalidOperationException("SlotSystemManager.focusedSGEBow: there's no sg set with SGBowFilter in focusedSGEs");
		}
		public ISlotGroup GetFocusedSGEWear(){
			foreach(ISlotGroup sg in focusedSGEs){
				IFilterHandler filterHandler = sg.GetFilterHandler();
				if(filterHandler.GetFilter() is SGWearFilter)
					return sg;
			}
			throw new InvalidOperationException("SlotSystemManager.focusedSGEWear: there's no sg set with SGWearFilter in focusedSGEs");
		}
		public ISlotGroup GetFocusedSGECGears(){
			foreach(ISlotGroup sg in focusedSGEs){
				IFilterHandler filterHandler = sg.GetFilterHandler();
				if(filterHandler.GetFilter() is SGCGearsFilter)
					return sg;
			}
			throw new InvalidOperationException("SlotSystemManager.focusedSGECGears: there's no sg set with SGCGearsFilter in focusedSGEs");
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
				ISlotSystemElement focEle = equipBundle.GetFocusedElement();
				IEquipmentSet result = focEle as IEquipmentSet;
				if(result != null) 
					return result;
				throw new InvalidOperationException("SlotSystemManger.focusedEqSet: equipBundle.focusedElement is not of valid type or substitute with null");
				
			}
		}
		public IPoolInventory GetPoolInv(){
			IInventory inventory = focusedSGP.GetInventory();
			if(inventory != null)
				return (IPoolInventory)inventory;
			throw new InvalidOperationException("SlotSystemManager.poolInv: focusedSGP.inventory is not set");
		}
		public IEquipmentSetInventory GetEquipInv(){
			foreach(ISlotGroup sge in focusedSGEs){
				if(sge != null){
					IEquipmentSetInventory result = (IEquipmentSetInventory)sge.GetInventory();
					if(result == null)
						throw new InvalidOperationException("SlotSystemManager.equipInv: someSGEs not set with an inv is found");
					else return result;
				}
			}
			return null;
		}
		public void ChangeEquippableCGearsCount(int i, ISlotGroup targetSG){
			if(!targetSG.IsResizable()){
				if(targetSG.IsFocused() || targetSG.IsDefocused()){
					GetEquipInv().SetEquippableCGearsCount(i);
					targetSG.InitializeItems();
					ssm.UpdateEquipInvAndAllSBsEquipState();
				}
			}else{
				throw new InvalidOperationException("ISlotGroupManager.ChangeEquippableCGearsCount: the targetSG is expandable");
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
