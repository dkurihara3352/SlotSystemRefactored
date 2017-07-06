﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

namespace SlotSystem{
	public class SlotSystemClasses{
	}
	
	/*	SSM	Classes	*/
		/*	states	*/
			/*	Selection State	*/
			/*	Action State	*/
		/*	process	*/
			/*	superclasses	*/
			/*	Selection Process	*/
			/*	Action Process	*/
	/*	Transaction classes	*/
		/*	transaction	*/
	/*	SlotGroup Classes	*/
		/*	states	*/
			/*	superclasses	*/
			/*	Selection States	*/
			/*	Action State	*/
		/*	process	*/
			/*	Selecttion Process	*/
			/*	Action Process*/
		/*	commands	*/
		/*	filters	*/
		/*	sorters	*/
	/*	Slottable Classses	*/
		/*	process	*/
			/*	Selecttion process	*/
			/*	Action process	*/
			/*	Equip process*/
			/*	Mark process*/
		/*	states	*/
			/*	superclasses	*/
			/*	SB Selection States	*/
			/*	SB Action States	*/
			/*	SB Equip states	*/
				public class SBUnequippedState: SBEqpState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						if(sb.prevEqpState == null || sb.prevEqpState == Slottable.unequippedState){
							/*	when initialized	*/
							return;
						}
						if(sb.sg.isPool){
							if(sb.prevEqpState != null && sb.prevEqpState == Slottable.equippedState){
								SBEqpProcess process = new SBUnequipProcess(sb, sb.UnequipCoroutine);
								sb.SetAndRunEquipProcess(process);
							}
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
			/*	SB Mark states	*/
				public class SBMarkedState: SBMrkState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						if(sb.sg.isPool){
							if(sb.prevMrkState != null && sb.prevMrkState == Slottable.unmarkedState){
								SBMrkProcess process = new SBMarkProcess(sb, sb.markCoroutine);
								sb.SetAndRunMarkProcess(process);
							}
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
				public class SBUnmarkedState: SBMrkState{
					public override void EnterState(StateHandler sh){
						base.EnterState(sh);
						if(sb.prevMrkState == null || sb.prevMrkState == Slottable.unmarkedState){
							/*	when initialized	*/
							return;
						}
						if(sb.sg.isPool){
							if(sb.prevMrkState != null && sb.prevMrkState == Slottable.markedState){
								SBMrkProcess process = new SBUnmarkProcess(sb, sb.unmarkCoroutine);
								sb.SetAndRunMarkProcess(process);
							}
						}
					}
					public override void ExitState(StateHandler sh){
						base.ExitState(sh);
					}
				}
		/*	commands	*/
			public interface SlottableCommand{
				void Execute(Slottable sb);
			}
			public class SBTapCommand: SlottableCommand{
				public void Execute(Slottable sb){

				}
			}
	/*	Other Classes	*/
		public class Slot{
			Slottable m_sb;
			public Slottable sb{
				get{return m_sb;}
				set{m_sb = value;}
			}
			Vector2 m_position;
			public Vector2 Position{
				get{return m_position;}
				set{m_position = value;}
			}
		}
		public class DraggedIcon{
			public InventoryItemInstance item{
				get{return m_item;}
				}InventoryItemInstance m_item;
			public IconDestination dest{
				get{return m_dest;}
				}IconDestination m_dest;
				public void SetDestination(SlotGroup sg, Slot slot){
					IconDestination newDest = new IconDestination(sg, slot);
					m_dest = newDest;
				}
			SlotSystemManager m_ssm;
			public Slottable sb{
				get{return m_sb;}
				}Slottable m_sb;
			public DraggedIcon(Slottable sb){
				m_sb = sb;
				m_item = this.sb.itemInst;
				m_ssm = SlotSystemManager.curSSM;
			}
			public void CompleteMovement(){
				m_ssm.AcceptDITAComp(this);
			}
		}
		public class IconDestination{
			public SlotGroup sg{
				get{return m_sg;}
				}SlotGroup m_sg;
				public void SetSG(SlotGroup sg){
					m_sg = sg;
				}
			public Slot slot{
				get{return m_slot;}
				}Slot m_slot;
				public void SetSlot(Slot slot){
					m_slot = slot;
				}
			public IconDestination(SlotGroup sg, Slot slot){
				SetSG(sg); SetSlot(slot);
			}
		}
		/*	SlotSystemElements	*/
			/*	states	*/
				/*	superstates	*/
					public class SSEStateEngine: SwitchableStateEngine{
						public SSEStateEngine(SlotSystemElement sse){
							this.handler = sse;
						}
						public void SetState(SSEState state){
							base.SetState(state);
						}
					}
					public abstract class SSEState: SwitchableState{
						protected SlotSystemElement sse;
						public virtual void EnterState(StateHandler handler){
							sse = (SlotSystemElement)handler;
						}
						public virtual void ExitState(StateHandler handler){}
					}					
				/*	Sel States	*/
					public abstract class SSESelState: SSEState{}
					public class SSEDeactivatedState: SSESelState{
						public override void EnterState(StateHandler sh){
							base.EnterState(sh);
							sse.SetAndRunSelProcess(null);
						}
					}
					public class SSEDefocusedState: SSESelState{
						public override void EnterState(StateHandler sh){
							base.EnterState(sh);
							SSEProcess process = null;
							if(sse.prevSelState == AbsSlotSystemElement.deactivatedState){
								process = null;
								sse.InstantGreyout();
							}else if(sse.prevSelState == AbsSlotSystemElement.focusedState)
								process = new SSEGreyoutProcess(sse, sse.greyoutCoroutine);
							else if(sse.prevSelState == AbsSlotSystemElement.selectedState)
								process = new SSEDehighlightProcess(sse, sse.dehighlightCoroutine);
							sse.SetAndRunSelProcess(process);
						}
					}
					public class SSEFocusedState: SSESelState{
						public override void EnterState(StateHandler sh){
							base.EnterState(sh);
							SSEProcess process = null;
							if(sse.prevSelState == AbsSlotSystemElement.deactivatedState){
								process = null;
								sse.InstantGreyin();
							}
							else if(sse.prevSelState == AbsSlotSystemElement.defocusedState)
								process = new SSEGreyinProcess(sse, sse.greyinCoroutine);
							else if(sse.prevSelState == AbsSlotSystemElement.selectedState)
								process = new SSEDehighlightProcess(sse, sse.dehighlightCoroutine);
							sse.SetAndRunSelProcess(process);
						}
					}
					public class SSESelectedState: SSESelState{
						public override void EnterState(StateHandler sh){
							base.EnterState(sh);
							SSEProcess process = null;
							if(sse.prevSelState == AbsSlotSystemElement.deactivatedState){
								sse.InstantHighlight();
							}else if(sse.prevSelState == AbsSlotSystemElement.defocusedState)
								process = new SSEHighlightProcess(sse, sse.highlightCoroutine);
							else if(sse.prevSelState == AbsSlotSystemElement.focusedState)
								process = new SSEHighlightProcess(sse, sse.highlightCoroutine);
							sse.SetAndRunSelProcess(process);
						}
					}
				/*	act state	*/
					public abstract class SSEActState: SSEState{}
					public class SSEWaitForActionState: SSEActState{
						public override void EnterState(StateHandler sh){
							base.EnterState(sh);
							sse.SetAndRunActProcess(null);
						}
					}
			/*	process	*/
				/*	superclasses	*/
					public class SSEProcessEngine{
						public SSEProcess process{
							get{return m_process;}
							}SSEProcess m_process;
						public void SetAndRunProcess(SSEProcess process){
							if(process != null)
								process.Stop();
							m_process = process;
							if(process != null)
								process.Start();
						}
					}
					public interface SSEProcess{
						bool isRunning{get;}
						System.Func<IEnumeratorFake> coroutineFake{set;}
						void Start();
						void Stop();
						void Expire();
					}
					public class AbsSSEProcess: SSEProcess{
						public bool isRunning{
							get{return m_isRunning;}
							} bool m_isRunning;
						public System.Func<IEnumeratorFake> coroutineFake{
							set{m_coroutineMock = value;}
							}System.Func<IEnumeratorFake> m_coroutineMock;
						protected SlotSystemElement sse{
							get{return m_sse;}
							set{m_sse = value;}
							} SlotSystemElement m_sse;
						public virtual void Start(){
							m_isRunning = true;
							m_coroutineMock();
						}
						public virtual void Stop(){
							if(isRunning)
								m_isRunning = false;
						}
						public virtual void Expire(){
							if(isRunning)
								m_isRunning = false;
						}
					}
				/*	sel process	*/
					public abstract class SSESelProcess: AbsSSEProcess{}
						public class SSEGreyoutProcess: SSESelProcess{
							public SSEGreyoutProcess(SlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
								this.sse = sse;
								this.coroutineFake = coroutineMock;
							}
						}
						public class SSEGreyinProcess: SSESelProcess{
							public SSEGreyinProcess(SlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
								this.sse = sse;
								this.coroutineFake = coroutineMock;
							}
						}
						public class SSEHighlightProcess: SSESelProcess{
							public SSEHighlightProcess(SlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
								this.sse = sse;
								this.coroutineFake = coroutineMock;
							}
						}
						public class SSEDehighlightProcess: SSESelProcess{
							public SSEDehighlightProcess(SlotSystemElement sse, System.Func<IEnumeratorFake> coroutineMock){
								this.sse = sse;
								this.coroutineFake = coroutineMock;
							}
						}
				/*	act process	*/
					public abstract class SSEActProcess: AbsSSEProcess{}
			public interface SlotSystemElement: IEnumerable<SlotSystemElement>, StateHandler{
				SSEState curSelState{get;}
				SSEState prevSelState{get;}
				SSEState curActState{get;}
				SSEState prevActState{get;}
				void SetAndRunSelProcess(SSEProcess process);
				void SetAndRunActProcess(SSEProcess process);
				SSEProcess selProcess{get;}
				SSEProcess actProcess{get;}
				IEnumeratorFake greyoutCoroutine();
				IEnumeratorFake greyinCoroutine();
				IEnumeratorFake highlightCoroutine();
				IEnumeratorFake dehighlightCoroutine();
				void InstantGreyin();
				void InstantGreyout();
				void InstantHighlight();
				string eName{get;}
				bool isBundleElement{get;}
				bool isPageElement{get;}
				bool isToggledOn{get;}
				bool isFocused{get;}
				bool isDefocused{get;}
				bool isDeactivated{get;}
				bool isFocusedInHierarchy{get;}
				void Activate();
				void Deactivate();
				void Focus();
				void Defocus();
				SlotSystemBundle immediateBundle{get;}
				SlotSystemElement parent{get;set;}
				SlotSystemManager ssm{get;set;}
				bool ContainsInHierarchy(SlotSystemElement ele);
				void PerformInHierarchy(System.Action<SlotSystemElement> act);
				void PerformInHierarchy(System.Action<SlotSystemElement, object> act, object obj);
				void PerformInHierarchy<T>(System.Action<SlotSystemElement, IList<T>> act, IList<T> list);
				int level{get;}
				bool Contains(SlotSystemElement element);
				SlotSystemElement this[int i]{get;}
				void ToggleOnPageElement();
			}
			public class SlotSystemPageElement{
				public SlotSystemElement element{
					get{return m_element;}
					}SlotSystemElement m_element;
				public bool isFocusedOnActivate{
					get{return m_isFocusedOnActivate;}
					}bool m_isFocusedOnActivate;
				public bool isFocusToggleOn{
					get{return m_isFocusToggleOn;}
					set{m_isFocusToggleOn = value;}
					}bool m_isFocusToggleOn;
				public SlotSystemPageElement(SlotSystemElement element, bool isFocusToggleOn){
					m_element = element;
					m_isFocusToggleOn = isFocusToggleOn;
					m_isFocusedOnActivate = isFocusToggleOn;				
				}
			}
			public interface TransactionManager{
				SlotSystemTransaction transaction{get;}
				void AcceptSGTAComp(SlotGroup sg);
				void AcceptDITAComp(DraggedIcon di);
				Slottable pickedSB{get;}
				Slottable targetSB{get;}
				SlotGroup sg1{get;}
				SlotGroup sg2{get;}
				DraggedIcon dIcon1{get;}
				DraggedIcon dIcon2{get;}
				SlotSystemElement hovered{get;}
				void UpdateTransaction();
				SlotSystemTransaction GetTransaction(Slottable pickedSB, SlotSystemElement hovered);
			}
		/*	Inventory Item	*/
			public interface SlottableItem: IEquatable<SlottableItem>, IComparable<SlottableItem>, IComparable{
				int Quantity{get;}
				bool IsStackable{get;}
			}
			public class InventoryItemInstance: SlottableItem{
				InventoryItem m_item;
				public InventoryItem Item{
					get{return m_item;}
					set{m_item = value;}
				}
				int m_quantity;
				public int Quantity{
					get{return m_quantity;}
					set{m_quantity = value;}
				}
				int m_acquisitionOrder;
				public int AcquisitionOrder{
					get{return m_acquisitionOrder;}
				}
				public void SetAcquisitionOrder(int id){
					m_acquisitionOrder = id;
				}
				bool m_isStackable;
				public bool IsStackable{
					get{
						return m_item.IsStackable;
					}
				}
				bool m_isEquipped = false;
				public bool isEquipped{
					get{return m_isEquipped;}
					set{m_isEquipped = value;}
				}
				bool m_isMarked = false;
				public bool isMarked{
					get{return m_isMarked;}
					set{m_isMarked = value;}
				}
				public override bool Equals(object other){
					if(!(other is InventoryItemInstance))
						return false;
					return Equals((SlottableItem)other);
				}
				public bool Equals(SlottableItem other){
					if(!(other is InventoryItemInstance))
						return false;
					InventoryItemInstance otherInst = (InventoryItemInstance)other;
					if(m_item.IsStackable)
						return m_item.Equals(otherInst.Item);
					else
						return object.ReferenceEquals(this, other);
				}
				public override int GetHashCode(){
					return m_item.ItemID.GetHashCode() + 31;
				}
				public static bool operator ==(InventoryItemInstance a, InventoryItemInstance b){
					return a.Equals(b);
				}
				public static bool operator != (InventoryItemInstance a, InventoryItemInstance b){
					if(object.ReferenceEquals(a, null)){
						return !object.ReferenceEquals(b, null);
					}
					if(object.ReferenceEquals(b, null)){
						return !object.ReferenceEquals(a, null);
					}
					return !(a == b);
				}
				int IComparable.CompareTo(object other){
					if(!(other is SlottableItem))
						throw new InvalidOperationException("System.Object.CompareTo: not a SlottableItem");
					return CompareTo((SlottableItem)other);
				}
				public int CompareTo(SlottableItem other){
					if(!(other is InventoryItemInstance))
						throw new InvalidOperationException("System.Object.CompareTo: not an InventoryItemInstance");
					InventoryItemInstance otherInst = (InventoryItemInstance)other;

					int result = m_item.ItemID.CompareTo(otherInst.Item.ItemID);
					if(result == 0)
						result = this.AcquisitionOrder.CompareTo(otherInst.AcquisitionOrder);
					
					return result;
				}
			}
			public class InventoryItem: IEquatable<InventoryItem>, IComparable, IComparable<InventoryItem>{
				bool m_isStackable;
				public bool IsStackable{
					get{return m_isStackable;}
					set{m_isStackable = value;}
				}

				int m_itemId;
				public int ItemID{
					get{return m_itemId;}
					set{m_itemId = value;}
				}

				public override bool Equals(object other){
					if(!(other is InventoryItem)) return false;
					else
						return Equals((InventoryItem)other);
				}
				public bool Equals(InventoryItem other){
					return m_itemId == other.ItemID;
				}

				public override int GetHashCode(){
					return 31 + m_itemId.GetHashCode();
				}

				public static bool operator == (InventoryItem a, InventoryItem b){
					return a.ItemID == b.ItemID;
				}

				public static bool operator != (InventoryItem a, InventoryItem b){
					return a.ItemID != b.ItemID;
				}
				int IComparable.CompareTo(object other){
					if(!(other is InventoryItem))
						throw new InvalidOperationException("Compare To: not a InventoryItemMock");
					return CompareTo((InventoryItem)other);
				}
				public int CompareTo(InventoryItem other){
					if(!(other is InventoryItem))
						throw new InvalidOperationException("Compare To: not a InventoryItemMock");
					
					return this.m_itemId.CompareTo(other.ItemID);
				}
				public static bool operator > (InventoryItem a, InventoryItem b){
					return a.CompareTo(b) > 0;
				}
				public static bool operator < (InventoryItem a, InventoryItem b){
					return a.CompareTo(b) < 0;
				}
			}
		/*	Inventories	*/
			public interface Inventory: IEnumerable<SlottableItem>{
				void Add(SlottableItem item);
				void Remove(SlottableItem item);
				SlotGroup sg{get;}
				void SetSG(SlotGroup sg);
				SlottableItem this[int i]{get;}
				int count{get;}
				bool Contains(SlottableItem item);
			}
			public class GenericInventory: Inventory{
				public IEnumerator<SlottableItem> GetEnumerator(){
					foreach(SlottableItem item in items){
						yield return item;
					}
					}IEnumerator IEnumerable.GetEnumerator(){
						return GetEnumerator();
					}

				public int count{
					get{return items.Count;}
				}
				public bool Contains(SlottableItem item){
					return items.Contains(item);
				}
				public SlottableItem this[int i]{
					get{return items[i];}
				}
				List<SlottableItem> items = new List<SlottableItem>();
				public void Add(SlottableItem item){
					items.Add(item);
				}
				public void Remove(SlottableItem item){
					items.Remove(item);
				}
				public SlotGroup sg{
					get{return m_sg;}
					}SlotGroup m_sg;
					public void SetSG(SlotGroup sg){
						m_sg = sg;
					}
			}
			public class PoolInventory: Inventory{
				public IEnumerator<SlottableItem> GetEnumerator(){
					foreach(SlottableItem item in m_items){
						yield return item;
					}
					}IEnumerator IEnumerable.GetEnumerator(){
						return GetEnumerator();
					}
				public bool Contains(SlottableItem item){
					return m_items.Contains(item);
				}
				public int count{
					get{return m_items.Count;}
				}
				public SlottableItem this[int i]{
					get{return m_items[i];}
				}
				List<SlottableItem> m_items = new List<SlottableItem>();
				public SlotGroup sg{get{return m_sg;}}
					SlotGroup m_sg;
					public void SetSG(SlotGroup sg){
						m_sg = sg;
					}
				public void Add(SlottableItem item){
					foreach(SlottableItem it in m_items){
						InventoryItemInstance invInst = (InventoryItemInstance)it;
						InventoryItemInstance addedInst = (InventoryItemInstance)item;
						if(invInst == addedInst && invInst.IsStackable){
							invInst.Quantity += addedInst.Quantity;
							return;
						}
					}
					m_items.Add(item);
					IndexItems();
				}
				public void Remove(SlottableItem item){
					SlottableItem itemToRemove = null;
					foreach(SlottableItem it in m_items){
						InventoryItemInstance checkedInst = (InventoryItemInstance)it;
						InventoryItemInstance removedInst = (InventoryItemInstance)item;
						if(checkedInst == removedInst){
							if(!removedInst.IsStackable)
								itemToRemove = it;
							else{
								checkedInst.Quantity -= removedInst.Quantity;
								if(checkedInst.Quantity <= 0)
									itemToRemove = it;
							}
						}
					}
					if(itemToRemove != null)
						m_items.Remove(itemToRemove);
					IndexItems();
				}
				void IndexItems(){
					for(int i = 0; i < m_items.Count; i ++){
						((InventoryItemInstance)m_items[i]).SetAcquisitionOrder(i);
					}
				}
			}
			public class EquipmentSetInventory: Inventory{
				public IEnumerator<SlottableItem> GetEnumerator(){
					foreach(SlottableItem item in m_items){
						yield return item;
					}
					}IEnumerator IEnumerable.GetEnumerator(){
						return GetEnumerator();
					}
				public EquipmentSetInventory(BowInstanceMock initBow, WearInstanceMock initWear, List<CarriedGearInstanceMock> initCGears ,int initCGCount){
					m_equippedBow = initBow;
					m_equippedWear = initWear;
					m_equippedCGears = initCGears;
					SetEquippableCGearsCount(initCGCount);
				}
				public bool Contains(SlottableItem item){
					foreach(SlottableItem it in this){
						if(it == item)
							return true;
					}
					return false;
				}
				public int count{
					get{return m_items.Count;}
				}
				public SlottableItem this[int i]{
					get{return m_items[i];}
				}
				public SlotGroup sg{get{return m_sg;}}
					SlotGroup m_sg;
					public void SetSG(SlotGroup sg){
						m_sg = sg;
					}
				BowInstanceMock m_equippedBow;
				WearInstanceMock m_equippedWear;
				List<CarriedGearInstanceMock> m_equippedCGears = new List<CarriedGearInstanceMock>();
				public int equippableCGearsCount{
					get{return m_equippableCGearsCount;}
					}int m_equippableCGearsCount;
				public void SetEquippableCGearsCount(int num){
					m_equippableCGearsCount = num;
					if(sg != null && sg.Filter is SGCGearsFilter && !sg.isExpandable)
					sg.SetInitSlotsCount(num);
				}
				
				List<SlottableItem> m_items{
					get{
						List<SlottableItem> result = new List<SlottableItem>();
						if(m_equippedBow != null)
							result.Add(m_equippedBow);
						if(m_equippedWear != null)
							result.Add(m_equippedWear);
						if(m_equippedCGears.Count != 0){
							foreach(CarriedGearInstanceMock inst in m_equippedCGears){
								result.Add((SlottableItem)inst);
							}
						}
						return result;
					}
				}
				public void Add(SlottableItem item){
					if(item != null){
						if(item is BowInstanceMock){
							BowInstanceMock bowInst = (BowInstanceMock)item;
							m_equippedBow = bowInst;
						}	
						else if(item is WearInstanceMock){
							WearInstanceMock wearInst = (WearInstanceMock)item;
							m_equippedWear = wearInst;
						}
						else if(item is CarriedGearInstanceMock){
							if(m_equippedCGears.Count < m_equippableCGearsCount)
								m_equippedCGears.Add((CarriedGearInstanceMock)item);
							else
								throw new InvalidOperationException("trying to add a CarriedGear exceeding the maximum allowed count");
						}
					}
				}
				public void Remove(SlottableItem removedItem){
					if(removedItem != null){
						if(removedItem is BowInstanceMock){
							if((BowInstanceMock)removedItem == m_equippedBow)
								m_equippedBow = null;
						}else if(removedItem is WearInstanceMock){
							if((WearInstanceMock)removedItem == m_equippedWear)
								m_equippedWear = null;
						}else if(removedItem is CarriedGearInstanceMock){
							CarriedGearInstanceMock spottedOne = null;
							foreach(CarriedGearInstanceMock cgInst in m_equippedCGears){
								if((CarriedGearInstanceMock)removedItem == cgInst)
									spottedOne = cgInst;
							}
							if(spottedOne != null)
								m_equippedCGears.Remove(spottedOne);
						}
					}
				}
			}
		/*	mock items	*/
			public class BowMock: InventoryItem{
				public BowMock(){
					IsStackable = false;
				}
			}
			public class WearMock: InventoryItem{
				public WearMock(){
					IsStackable = false;
				}
			}
			public abstract class CarriedGearMock: InventoryItem{
			}
			public class ShieldMock: CarriedGearMock{
				public ShieldMock(){
					IsStackable = false;
				}
			}
			public class MeleeWeaponMock: CarriedGearMock{
				public MeleeWeaponMock(){
					IsStackable = false;
				}
			}
			public class QuiverMock: CarriedGearMock{
				public QuiverMock(){
					IsStackable = false;
				}
			}
			public class PackMock: CarriedGearMock{
				public PackMock(){
					IsStackable = false;
				}
			}
			public class PartsMock: InventoryItem{
				public PartsMock(){
					IsStackable = true;
				}
			}
		/*	mock instances	*/
			public class BowInstanceMock: InventoryItemInstance{
				public BowInstanceMock(){
					this.Quantity = 1;
				}
			}
			public class WearInstanceMock: InventoryItemInstance{
				public WearInstanceMock(){
					this.Quantity = 1;
				}
			}
			public class CarriedGearInstanceMock: InventoryItemInstance{}
			public class ShieldInstanceMock: CarriedGearInstanceMock{
				public ShieldInstanceMock(){
					this.Quantity = 1;
				}
			}
			public class MeleeWeaponInstanceMock: CarriedGearInstanceMock{
				public MeleeWeaponInstanceMock(){
					this.Quantity = 1;
				}
			}
			public class QuiverInstanceMock: CarriedGearInstanceMock{
				public QuiverInstanceMock(){
					this.Quantity = 1;
				}
			}
			public class PackInstanceMock: CarriedGearInstanceMock{
				public PackInstanceMock(){
					this.Quantity = 1;
				}
			}
			public class PartsInstanceMock: InventoryItemInstance{}
	/*	utility	*/
		public static class Util{
			public static SlotSystemTransaction CloneTA(SlotSystemTransaction orig){
				SlotSystemTransaction cloneTA = null;
				if(orig is RevertTransaction)
					cloneTA = new RevertTransaction((RevertTransaction)orig);
				if(orig is ReorderTransaction)
					cloneTA = new ReorderTransaction((ReorderTransaction)orig);
				if(orig is StackTransaction)
					cloneTA = new StackTransaction((StackTransaction)orig);
				if(orig is SwapTransaction)
					cloneTA = new SwapTransaction((SwapTransaction)orig);
				if(orig is FillTransaction)
					cloneTA = new FillTransaction((FillTransaction)orig);
				if(orig is SortTransaction)
					cloneTA = new SortTransaction((SortTransaction)orig);
				return cloneTA;
			}
			public static Slottable CloneSB(Slottable orig){
				if(orig != null){
					GameObject cloneGO = new GameObject("cloneGO");
					SBClone clone = cloneGO.AddComponent<SBClone>();
					clone.Initialize(orig);
					return clone;
				}
				return null;
			}
			public static SlotGroup CloneSG(SlotGroup orig){
				if(orig != null){
					GameObject cloneSGGO = new GameObject("cloneSGGO");
					SGClone cloneSG = cloneSGGO.AddComponent<SGClone>();
					cloneSG.Initialize(orig);
					return cloneSG;
				}
				return null;
			}
			public static bool SBsShareSGAndItem(Slottable sbA, Slottable sbB){
				bool flag = true;
				flag &= sbA.sg == sbB.sg;
				flag &= sbA.itemInst == sbB.itemInst;
				return flag;
			}
			public static void Trim(ref List<Slottable> sbs){
				List<Slottable> trimmed = new List<Slottable>();
				foreach(Slottable sb in sbs){
					if(sb != null)
						trimmed.Add(sb);
				}
				sbs = trimmed;
			}
			public static void AddInEmptyOrConcat(ref List<Slottable> sbs, Slottable added){
				foreach(Slottable sb in sbs){
					if(sb == null){
						sbs[sbs.IndexOf(sb)] = added;
						return;
					}
				}
				sbs.Add(added);
			}
			public static bool HaveCommonItemFamily(Slottable sb, Slottable other){
				if(sb.item is BowInstanceMock)
					return (other.item is BowInstanceMock);
				else if(sb.item is WearInstanceMock)
					return (other.item is WearInstanceMock);
				else if(sb.item is CarriedGearInstanceMock)
					return (other.item is CarriedGearInstanceMock);
				else if(sb.item is PartsInstanceMock)
					return (other.item is PartsInstanceMock);
				else
					return false;
			}
			public static bool IsSwappable(Slottable pickedSB, Slottable otherSB){
				/*	precondition
						1) they do not share same SG
						2) otherSB.SG accepts pickedSB
						3) not stackable
				*/
				if(pickedSB.sg != otherSB.sg){
					if(otherSB.sg.AcceptsFilter(pickedSB)){
						if(!(pickedSB.itemInst == otherSB.itemInst && pickedSB.itemInst.Item.IsStackable))
						 if(pickedSB.sg.AcceptsFilter(otherSB))
							return true;
					}
				}
				return false;
			}
			/*	SSE	*/
				public static string SSEDebug(SlotSystemElement sse){
					string res = "";
					string prevSel = SSEStateNamePlain(sse.prevSelState);
					string curSel = SSEStateName(sse.curSelState);
					string selProc;
						if(sse.selProcess == null)
							selProc = "";
						else
							selProc = SSEProcessName(sse.selProcess) + " running? " + (sse.selProcess.isRunning?Blue("true"):Red("false"));
					string prevAct = SSEStateNamePlain(sse.prevActState);
					string curAct = SSEStateName(sse.curActState);
					string actProc;
						if(sse.actProcess == null)
							actProc = "";
						else
							actProc = SSEProcessName(sse.actProcess) + " running " + (sse.actProcess.isRunning?Blue("true"):Red("false"));
					res =
						sse.eName + " " +
						Bold("Sel ") + "from " + prevSel + " to " + curSel + " " +
							" proc, " + selProc + ", " +
						Bold("Act ") + "from " + prevAct + " to " + curAct + " " +
							" proc, " + actProc;
					return res;
				}
				public static string SSEStateName(SSEState state){
					string res = "";

					if(state is SSEDeactivatedState){
						res = Util.Red("SSEDeactivated");
					}else if(state is SSEDefocusedState){
						res = Util.Green("SSEDefocused");
					}else if(state is SSEFocusedState){
						res = Util.Blue("SSEFocused");
					}else if(state is SSESelectedState){
						res = Util.Aqua("SSESelected");
					}else if(state is SSEWaitForActionState){
						res = Util.Sangria("SSEWFA");
					}
					return res;
				}
				public static string SSEStateNamePlain(SSEState state){
					string res = "";

					if(state is SSEDeactivatedState){
						res = "SSEDeactivated";
					}else if(state is SSEDefocusedState){
						res = "SSEDefocused";
					}else if(state is SSEFocusedState){
						res = "SSEFocused";
					}else if(state is SSESelectedState){
						res = "SSESelected";
					}else if(state is SSEWaitForActionState){
						res = "SSEWFA";
					}
					return res;
				}
				public static string SSEProcessName(SSEProcess process){
					string res = "";
					if(process is SSEGreyinProcess)
						res = Util.Blue("Greyin");
					else if(process is SSEGreyoutProcess)
						res = Util.Green("Greyout");
					else if(process is SSEHighlightProcess)
						res = Util.Red("Highlight");
					else if(process is SSEDehighlightProcess)
						res = Util.Brown("Dehighlight");
					return res;
				}
			/*	SSM	*/
				public static string SSMStateName(SSMState state){
					string res = "";
					if(state is SSMDeactivatedState)
						res = Util.Red("Deactivated");
					else if(state is SSMDefocusedState)
						res = Util.Blue("Defocused");
					else if(state is SSMFocusedState)
						res = Util.Green("Focused");
					else if(state is SSMWaitForActionState)
						res = Util.Green("WaitForAction");
					else if(state is SSMProbingState)
						res = Util.Ciel("Probing");
					else if(state is SSMTransactionState)
						res = Util.Terra("Transaction");
					return res;
				}
				public static string SSMStateNamePlain(SSMState state){
					string res = "";
					if(state is SSMDeactivatedState)
						res = "Deactivated";
					else if(state is SSMDefocusedState)
						res = "Defocused";
					else if(state is SSMFocusedState)
						res = "Focused";
					else if(state is SSMWaitForActionState)
						res = "WaitForAction";
					else if(state is SSMProbingState)
						res = "Probing";
					else if(state is SSMTransactionState)
						res = "Transaction";
					return res;
				}
				public static string TransactionName(SlotSystemTransaction ta){
					string res = "";
					if(ta is RevertTransaction)
						res = Util.Red("RevertTA");
					else if(ta is ReorderTransaction)
						res = Util.Blue("ReorderTA");
					else if(ta is StackTransaction)
						res = Util.Aqua("StackTA");
					else if(ta is SwapTransaction)
						res = Util.Terra("SwapTA");
					else if(ta is FillTransaction)
						res = Util.Forest("FillTA");
					else if(ta is SortTransaction)
						res = Util.Khaki("SortTA");
					else if(ta is EmptyTransaction)
						res = Util.Beni("Empty");
					return res;
				}
				public static string SSMProcessName(SSMProcess proc){
					string res = "";
					if(proc is SSMGreyinProcess)
						res = Util.Red("Greyin");
					else if(proc is SSMGreyoutProcess)
						res = Util.Red("Greyout");
					else if(proc is SSMProbeProcess)
						res = Util.Red("Probe");
					else if(proc is SSMTransactionProcess)
						res = Util.Blue("Transaction");
					return res;
				}
				public static string SSMDebug(SlotSystemManager ssm){
					string res = "";
					string pSB = Util.SBofSG(ssm.pickedSB);
					string tSB = Util.SBofSG(ssm.targetSB);
					string hovered = "";
						if(ssm.hovered is Slottable)
							hovered = SBofSG((Slottable)ssm.hovered);
						else if(ssm.hovered is SlotGroup)
							hovered = ssm.hovered.eName;
					string di1;
						if(ssm.dIcon1 == null)
							di1 = "null";
						else
							di1 = Util.SBofSG(ssm.dIcon1.sb);
					string di2;
						if(ssm.dIcon2 == null)
							di2 = "null";
						else
							di2 = Util.SBofSG(ssm.dIcon2.sb);
					
					string sg1 = ssm.sg1 == null?"null":ssm.sg1.eName;
					string sg2 = ssm.sg2 == null?"null":ssm.sg2.eName;
					string prevSel = Util.SSMStateNamePlain((SSMSelState)ssm.prevSelState);
					string curSel = Util.SSMStateName((SSMSelState)ssm.curSelState);
					string selProc;
						if(ssm.selProcess == null)
							selProc = "";
						else
							selProc = Util.SSMProcessName((SSMSelProcess)ssm.selProcess) + " running? " + (ssm.selProcess.isRunning?Blue("true"):Red("false"));
					string prevAct = Util.SSMStateNamePlain((SSMActState)ssm.prevActState);
					string curAct = Util.SSMStateName((SSMActState)ssm.curActState);
					string actProc;
						if((SSMActProcess)ssm.actProcess == null)
							actProc = "";
						else
							actProc = Util.SSMProcessName((SSMActProcess)ssm.actProcess) + " running? " + (ssm.actProcess.isRunning?Blue("true"):Red("false"));
					string ta = Util.TransactionName(ssm.transaction);
					string d1Done = "d1Done: " + (ssm.dIcon1Done?Util.Blue("true"):Util.Red("false"));
					string d2Done = "d2Done: " + (ssm.dIcon2Done?Util.Blue("true"):Util.Red("false"));
					string sg1Done = "sg1Done: " + (ssm.sg1Done?Util.Blue("true"):Util.Red("false"));
					string sg2Done = "sg2Done: " + (ssm.sg2Done?Util.Blue("true"):Util.Red("false"));
					res = Util.Bold("SSM:") +
							" pSB " + pSB +
							", tSB " + tSB +
							", hovered " + hovered +
							", di1 " + di1 +
							", di2 " + di2 +
							", sg1 " + sg1 +
							", sg2 " + sg2 + ", " +
						Util.Bold("Sel ") + "from " + prevSel + " to " + curSel + " " +
							"proc " + selProc + ", " +
						Util.Bold("Act ") + "from " + prevAct + " to " + curAct + " " +
							"proc " + actProc + ", " +
						Util.Bold("TA ") + ta + ", " + 
						Util.Bold("TAComp ") + d1Done + " " + d2Done + " " + sg1Done + " " + sg2Done;
					return res;
				}
			/*	SG	*/
				public static string SGStateName(SGState state){
					string res = "";
					if(state is SGDeactivatedState){
						res = Util.Red("SGDeactivated");
					}else if(state is SGDefocusedState){
						res = Util.Green("SGDefocused");
					}else if(state is SGFocusedState){
						res = Util.Blue("SGFocused");
					}else if(state is SGSelectedState){
						res = Util.Aqua("SGSelected");
					}else if(state is SGWaitForActionState){
						res = Util.Sangria("SGWFA");
					}else if(state is SGRevertState){
						res = Util.Sangria("SGRevert");
					}else if(state is SGReorderState){
						res = Util.Aqua("SGReorder");
					}else if(state is SGFillState){
						res = Util.Forest("SGFill");
					}else if(state is SGSwapState){
						res = Util.Berry("SGSwap");
					}else if(state is SGAddState){
						res = Util.Violet("SGAdd");
					}else if(state is SGRemoveState){
						res = Util.Khaki("SGRemove");
					}else if(state is SGSortState){
						res = Util.Midnight("SGSort");
					}
					return res;
				}
				public static string SGStateNamePlain(SGState state){
					string res = "";
					if(state is SGDeactivatedState){
						res = "SGDeactivated";
					}else if(state is SGDefocusedState){
						res = "SGDefocused";
					}else if(state is SGFocusedState){
						res = "SGFocused";
					}else if(state is SGSelectedState){
						res = "SGSelected";
					}else if(state is SGWaitForActionState){
						res = "SGWFA";
					}else if(state is SGRevertState){
						res = "SGRevert";
					}else if(state is SGReorderState){
						res = "SGReorder";
					}else if(state is SGFillState){
						res = "SGFill";
					}else if(state is SGSwapState){
						res = "SGSwap";
					}else if(state is SGAddState){
						res = "SGAdd";
					}else if(state is SGRemoveState){
						res = "SGRemove";
					}else if(state is SGSortState){
						res = "SGSort";
					}
					return res;
				}
				public static string SGProcessName(SGProcess proc){
					string res = "";
					if(proc is SGGreyinProcess)
						res = Util.Blue("Greyin");
					else if(proc is SGGreyoutProcess)
						res = Util.Green("Greyout");
					else if(proc is SGHighlightProcess)
						res = Util.Red("Highlight");
					else if(proc is SGDehighlightProcess)
						res = Util.Brown("Dehighlight");
					else if(proc is SGTransactionProcess)
						res = Util.Khaki("Transaction");
					return res;
				}
				public static string SGDebug(SlotGroup sg){
					string res = "";
					string prevSel = SGStateNamePlain((SGSelState)sg.prevSelState);
					string curSel = SGStateName((SGSelState)sg.curSelState);
					string selProc;
						if(sg.selProcess == null)
							selProc = "";
						else
							selProc = SGProcessName((SGSelProcess)sg.selProcess) + " running? " + (sg.selProcess.isRunning?Blue("true"):Red("false"));
					string prevAct = SGStateNamePlain((SGActState)sg.prevActState);
					string curAct = SGStateName((SGActState)sg.curActState);
					string actProc;
						if(sg.actProcess == null)
							actProc = "";
						else
							actProc = SGProcessName((SGActProcess)sg.actProcess) + " running? " + (sg.actProcess.isRunning?Blue("true"):Red("false"));
					res =  
						sg.eName + " " +
						Bold("Sel ") + "from " + prevSel + " to " + curSel + " " +
							" proc, " + selProc + ", " +
						Bold("Act ") + "from " + prevAct + " to " + curAct + " " +
							" proc, " + actProc;
					return res;
				}
			/*	SB	*/
				public static string ItemInstName(InventoryItemInstance itemInst){
					string result = "";
					if(itemInst != null){
						switch(itemInst.Item.ItemID){
							case 0:	result = "defBow"; break;
							case 1:	result = "crfBow"; break;
							case 2:	result = "frgBow"; break;
							case 3:	result = "mstBow"; break;
							case 100: result = "defWear"; break;
							case 101: result = "crfWear"; break;
							case 102: result = "frgWear"; break;
							case 103: result = "mstWear"; break;
							case 200: result = "defShield"; break;
							case 201: result = "crfShield"; break;
							case 202: result = "frgShield"; break;
							case 203: result = "mstShield"; break;
							case 300: result = "defMWeapon"; break;
							case 301: result = "crfMWeapon"; break;
							case 302: result = "frgMWeapon"; break;
							case 303: result = "mstMWeapon"; break;
							case 400: result = "defQuiver"; break;
							case 401: result = "crfQuiver"; break;
							case 402: result = "frgQuiver"; break;
							case 403: result = "mstQuiver"; break;
							case 500: result = "defPack"; break;
							case 501: result = "crfPack"; break;
							case 502: result = "frgPack"; break;
							case 503: result = "mstPack"; break;
							case 600: result = "defParts"; break;
							case 601: result = "crfParts"; break;
							case 602: result = "frgParts"; break;
							case 603: result = "mstParts"; break;
						}
					}
					return result;
				}
				public static string SBName(Slottable sb){
					string result = "null";
					if(sb != null){
						result = ItemInstName(sb.itemInst);
						if(SlotSystemManager.curSSM != null){
							List<InventoryItemInstance> sameItemInsts = new List<InventoryItemInstance>();
							foreach(InventoryItemInstance itemInst in SlotSystemManager.curSSM.poolInv){
								if(itemInst.Item == sb.itemInst.Item)
									sameItemInsts.Add(itemInst);
							}
							int index = sameItemInsts.IndexOf(sb.itemInst);
							result += "_"+index.ToString();
							if(sb.itemInst is BowInstanceMock)
								result = Forest(result);
							if(sb.itemInst is WearInstanceMock)
								result = Sangria(result);
							if(sb.itemInst is CarriedGearInstanceMock)
								result = Terra(result);
							if(sb.itemInst is PartsInstanceMock)
								result = Midnight(result);
						}
					}
					return result;
				}
				public static string SBofSG(Slottable sb){
					string res = "";
					if(sb != null){
						res = Util.SBName(sb) + " of " + sb.sg.eName;
						if(sb.isEquipped && sb.sg.isPool)
							res = Util.Bold(res);
					}
					return res;
				}
				public static string SBStateName(SBState state){
					string result = "";
					if(state is SBSelState){
						if(state is SBDeactivatedState)
							result = Red("Deactivated");
						else if(state is SBFocusedState)
							result = Blue("Focused");
						else if(state is SBDefocusedState)
							result = Green("Defocused");
						else if(state is SBSelectedState)
							result = Ciel("Selected");
					}else if(state is SBActState){
						if(state is WaitForActionState)
							result = Aqua("WFAction");
						else if(state is WaitForPointerUpState)
							result = Forest("WFPointerUp");
						else if(state is WaitForPickUpState)
							result = Brown("WFPickUp");
						else if(state is WaitForNextTouchState)
							result = Terra("WFNextTouch");
						else if(state is PickedUpState)
							result = Berry("PickedUp");
						else if(state is SBRemovedState)
							result = Violet("Removed");
						else if(state is SBAddedState)
							result = Khaki("Added");
						else if(state is SBMoveWithinState)
							result = Midnight("MoveWithin");
					}else if(state is SBEqpState){
						if(state is SBEquippedState)
							result = Red("Equipped");
						else if(state is SBUnequippedState)
							result = Blue("Unequipped");
					}else if(state is SBMrkState){
						if(state is SBMarkedState)
							result = Red("Marked");
						else if(state is SBUnmarkedState)
							result = Blue("Unmarked");
					}
					return result;
				}
				public static string SBStateNamePlain(SBState state){
					string result = "";
					if(state is SBSelState){
						if(state is SBDeactivatedState)
							result = "Deactivated";
						else if(state is SBFocusedState)
							result = "Focused";
						else if(state is SBDefocusedState)
							result = "Defocused";
						else if(state is SBSelectedState)
							result = "Selected";
					}else if(state is SBActState){
						if(state is WaitForActionState)
							result = "WFAction";
						else if(state is WaitForPointerUpState)
							result = "WFPointerUp";
						else if(state is WaitForPickUpState)
							result = "WFPickUp";
						else if(state is WaitForNextTouchState)
							result = "WFNextTouch";
						else if(state is PickedUpState)
							result = "PickedUp";
						else if(state is SBRemovedState)
							result = "Removed";
						else if(state is SBAddedState)
							result = "Added";
						else if(state is SBMoveWithinState)
							result = "MoveWithin";
					}else if(state is SBEqpState){
						if(state is SBEquippedState)
							result = "Equipped";
						else if(state is SBUnequippedState)
							result = "Unequipped";
					}else if(state is SBMrkState){
						if(state is SBMarkedState)
							result = "Marked";
						else if(state is SBUnmarkedState)
							result = "Unmarked";
					}
					return result;
				}
				public static string SBProcessName(SBProcess process){
					string res = "";
					if(process is SBGreyoutProcess)
						res = Green("Greyout");
					else if(process is SBGreyinProcess)
						res = Blue("Greyin");
					else if(process is SBHighlightProcess)
						res = Red("Highlight");
					else if(process is SBDehighlightProcess)
						res = Ciel("Dehighlight");
					else if(process is WaitForPointerUpProcess)
						res = Aqua("WFPointerUp");
					else if(process is WaitForPickUpProcess)
						res = Forest("WFPickUp");
					else if(process is SBPickedUpProcess)
						res = Brown("PickedUp");
					else if(process is WaitForNextTouchProcess)
						res = Terra("WFNextTouch");
					else if(process is SBRemoveProcess)
						res = Violet("Removed");
					else if(process is SBAddProcess)
						res = Khaki("Added");
					else if(process is SBMoveWithinProcess)
						res = Midnight("MoveWithin");
					else if(process is SBUnequipProcess)
						res = Red("Unequip");
					else if(process is SBEquipProcess)
						res = Blue("Equipping");
					else if(process is SBUnmarkProcess)
						res = Red("Unmark");
					else if(process is SBMarkProcess)
						res = Blue("Mark");
					return res;
				}
				public static string SBDebug(Slottable sb){
					string res = "";
					if(sb == null)
						res = "null";
					else{	
						string sbName = SBofSG(sb);
						string prevSel = SBStateNamePlain((SBSelState)sb.prevSelState);
						string curSel = SBStateName((SBSelState)sb.curSelState);
						string selProc;
							if(sb.selProcess == null)
								selProc = "";
							else
								selProc = SBProcessName((SBSelProcess)sb.selProcess) + " running? " + (sb.selProcess.isRunning?Blue("true"):Red("false"));
						string prevAct = SBStateNamePlain((SBActState)sb.prevActState);
						string curAct = SBStateName((SBActState)sb.curActState);
						string actProc;
							if(sb.actProcess == null)
								actProc = "";
							else
								actProc = SBProcessName((SBActProcess)sb.actProcess) + " running? " + (sb.actProcess.isRunning?Blue("true"):Red("false"));
						string prevEqp = SBStateNamePlain((SBEqpState)sb.prevEqpState);
						string curEqp = SBStateName((SBEqpState)sb.curEqpState);
						string eqpProc;
							if(sb.eqpProcess == null)
								eqpProc = "";
							else
								eqpProc = SBProcessName((SBEqpProcess)sb.eqpProcess) + " running? " + (sb.eqpProcess.isRunning?Blue("true"):Red("false"));
						string prevMrk = SBStateNamePlain((SBMrkState)sb.prevMrkState);
						string curMrk = SBStateName((SBMrkState)sb.curMrkState);
						string mrkProc;
							if(sb.mrkProcess == null)
								mrkProc = "";
							else
								mrkProc = SBProcessName((SBMrkProcess)sb.mrkProcess) + " running? " + (sb.mrkProcess.isRunning?Blue("true"):Red("false"));
						res = sbName + ": " +
							Bold("Sel ") + " from " + prevSel + " to " + curSel + " proc " + selProc + ", " + 
							Bold("Act ") + " from " + prevAct + " to " + curAct + " proc " + actProc + ", " + 
							Bold("Eqp ") + " from " + prevEqp + " to " + curEqp + " proc " + eqpProc + ", " +
							Bold("Mrk ") + " from " + prevMrk + " to " + curMrk + " proc " + mrkProc + ", " +
							Bold("SlotID: ") + " from " + sb.slotID.ToString() + " to " + sb.newSlotID.ToString() 
							;
					}
					return res;
				}
			/*	Debug	*/
				public static string TADebug(Slottable testSB, SlotSystemElement hovered){
					SlotSystemTransaction ta = testSB.ssm.GetTransaction(testSB, hovered);
					string taStr = TransactionName(ta);
					string taTargetSB = Util.SBofSG(ta.targetSB);
					string taSG1 = ta.sg1==null?"null":ta.sg1.eName;
					string taSG2 = ta.sg2 == null? "null": ta.sg2.eName;
					return "DebugTarget: " + taStr + " " +
						"targetSB: " + taTargetSB + ", " + 
						"sg1: " + taSG1 + ", " +
						"sg2: " + taSG2
						;
				}
				public static string TADebug(SlotSystemTransaction ta){
					string taStr = TransactionName(ta);
					string taTargetSB = Util.SBofSG(ta.targetSB);
					string taSG1 = ta.sg1==null?"null":ta.sg1.eName;
					string taSG2 = ta.sg2 == null? "null": ta.sg2.eName;
					return "DebugTarget: " + taStr + " " +
						"targetSB: " + taTargetSB + ", " + 
						"sg1: " + taSG1 + ", " +
						"sg2: " + taSG2
						;
				}
				public static string Red(string str){
					return "<color=#ff0000>" + str + "</color>";
				}
				public static string Blue(string str){
					return "<color=#0000ff>" + str + "</color>";

				}
				public static string Green(string str){
					return "<color=#02B902>" + str + "</color>";
				}
				public static string Ciel(string str){
					return "<color=#11A795>" + str + "</color>";
				}
				public static string Aqua(string str){
					return "<color=#128582>" + str + "</color>";
				}
				public static string Forest(string str){
					return "<color=#046C57>" + str + "</color>";
				}
				public static string Brown(string str){
					return "<color=#805A05>" + str + "</color>";
				}
				public static string Terra(string str){
					return "<color=#EA650F>" + str + "</color>";
				}
				public static string Berry(string str){
					return "<color=#A41565>" + str + "</color>";
				}
				public static string Violet(string str){
					return "<color=#793DBD>" + str + "</color>";
				}
				public static string Khaki(string str){
					return "<color=#747925>" + str + "</color>";
				}
				public static string Midnight(string str){
					return "<color=#1B2768>" + str + "</color>";
				}
				public static string Beni(string str){
					return "<color=#E32791>" + str + "</color>";
				}
				public static string Sangria(string str){
					return "<color=#640A16>" + str + "</color>";
				}
				public static string Yamabuki(string str){
					return "<color=#EAB500>" + str + "</color>";
				}
				public static string Bold(string str){
					return "<b>" + str + "</b>";
				}
				static string m_stacked;
				public static string Stacked{
					get{
						string result = m_stacked;
						m_stacked = "";
						return result;
					}
				}
				public static void Stack(string str){
					m_stacked += str + ", ";
				}
		}
}
