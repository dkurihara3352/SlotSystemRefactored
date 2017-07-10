using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class FakeSG : SlotGroup {
		/*	States and Processes	*/
			public override SSEState curSelState{get{return m_curSelState;}}
				private SGSelState m_curSelState;
				public override void SetSelState(SSEState state){m_curSelState = (SGSelState)state;}
			public override SSEState prevSelState{get{return m_prevSelState;}}
				private SGSelState m_prevSelState;
				public void SetPrevSelState(SGSelState state){m_prevSelState = state;}
			public override SSEState curActState{get{return m_curActState;}}
				private SGActState m_curActState;
				public override void SetActState(SSEState state){m_curActState = (SGActState)state;}
			public override SSEState prevActState{get{return m_prevActState;}}
				private SGActState m_prevActState;
				public void SetPrevActState(SGActState state){m_prevActState = state;}
			public override SSEProcess selProcess{get{return m_selProcess;}}
				private SSEProcess m_selProcess;
				public override void SetAndRunSelProcess(SSEProcess process){m_selProcess = process;}
			public override SSEProcess actProcess{get{return m_actProcess;}}
				private SSEProcess m_actProcess;
				public override void SetAndRunActProcess(SSEProcess process){m_actProcess = process;}
		/*	Fields	*/
			public override AxisScrollerMock scroller{get{return m_axisScroller;}}
				AxisScrollerMock m_axisScroller;
				public void SetAxisScroller(object obj){if(obj == null||obj is AxisScrollerMock) m_axisScroller = (AxisScrollerMock) obj;}
			public override Inventory inventory{get{return m_inventory;}}
				Inventory m_inventory;
				public override void SetInventory(Inventory inv){SetInventory((object)inv);}
				public void SetInventory(object obj){if(obj == null || obj is Inventory)m_inventory = (Inventory) obj;}
			public override bool isShrinkable{get{return m_isShrinkable;}}
				bool m_isShrinkable;
				public void SetIsShrinkable(bool bl){m_isShrinkable = bl;}
			public override bool isExpandable{get{return m_isExpandable;}}
				bool m_isExpandable;
				public void SetIsExpandable(bool aScroller){m_isExpandable = aScroller;}
			public override List<Slot> slots{get{return m_slots;}}
				List<Slot> m_slots;
				public override void SetSlots(List<Slot> slots){SetSlots((object)slots);}
				public void SetSlots(object obj){if(obj == null || obj is List<Slot>) m_slots = obj as List<Slot>;}
			public override List<Slot> newSlots{get{return m_newSlots;}}
				List<Slot> m_newSlots;
				public override void SetNewSlots(List<Slot> newSlots){SetNewSlots((object)newSlots);}
				public void SetNewSlots(object obj){if(obj == null || obj is List<Slot>) m_newSlots = obj as List<Slot>;}
			public override bool isPool{get{return m_isPool;}}
				bool m_isPool;
				public void SetIsPool(bool bl){m_isPool = bl;}
			public override bool isSGE{get{return m_isSGE;}}
				bool m_isSGE;
				public void SetIsSGE(bool bl){m_isSGE = bl;}
			public override bool isSGG{get{return m_isSGG;}}
				bool m_isSGG;
				public void SetIsSGG(bool bl){m_isSGG = bl;}
			public override bool isAutoSort{get{return m_isAutoSort;}}
				public void SetIsAutoSort(bool bl){m_isAutoSort = bl;}
			public List<Slottable> GetSBs(){
				return slottables;
			}
				public override void SetSBs(List<Slottable> sbs){SetSBs(sbs);}
				public void SetSBs(object obj){if(obj == null || obj is List<Slottable>) m_slottables = obj as List<Slottable>;}
			public override List<Slottable> newSBs{get{return m_newSBs;}}
				List<Slottable> m_newSBs;
				public override void SetNewSBs(List<Slottable> sbs){SetNewSBs((object)sbs);}
				public void SetNewSBs(object obj){if(obj == null || obj is List<Slottable>) m_newSBs = obj as List<Slottable>;}
			public override List<InventoryItemInstance> itemInstances{get{return m_itemInstances;}}
				List<InventoryItemInstance> m_itemInstances;
				public void SetItemInstances(object obj){if(obj == null || obj is List<InventoryItemInstance>) m_itemInstances = obj as List<InventoryItemInstance>;}
			public override List<InventoryItemInstance> actualItemInsts{get{return m_actualItemInsts;}}
				List<InventoryItemInstance> m_actualItemInsts;
				public void SetActualItemInsts(object obj){if(obj == null || obj is List<InventoryItemInstance>) m_actualItemInsts = obj as List<InventoryItemInstance>;}
			public override bool isFocusedInBundle{get{return m_isFocusedInBundle;}}
				bool m_isFocusedInBundle;
				public void SetIsFocusedInBundle(bool bl){m_isFocusedInBundle = bl;}
			public override bool hasEmptySlot{get{return m_hasEmptySlot;}}
				bool m_hasEmptySlot;
				public void SetHasEmptySlot(bool bl){m_hasEmptySlot = bl;}
			public override int actualSBsCount{get{return m_actualSBsCount;}}
				int m_actualSBsCount = -1;
				public void SetActualSBsCount(int count){m_actualSBsCount = count;}
			public override List<Slottable> equippedSBs{get{return m_equippedSBs;}}
				List<Slottable> m_equippedSBs;
				public void SetEquippedSBs(object obj){if(obj == null || obj is List<Slottable>) m_equippedSBs = obj as List<Slottable>;}
			public override bool isAllTASBsDone{get{return m_isAllTASBsDone;}}
				bool m_isAllTASBsDone;
				public void SetIsAllTASBsDone(bool bl){m_isAllTASBsDone = bl;}
			public override int initSlotsCount{get{return m_initSlotsCount;}}
				int m_initSlotsCount;
				public override void SetInitSlotsCount(int i){m_initSlotsCount = i;}
			public override bool isFocused{get{return m_isFocused;}}
				bool m_isFocused;
				public void SetIsFocused(bool bl){m_isFocused = bl;}
			public override bool isDefocused{get{return m_isDefocused;}}
				bool m_isDefocused;
				public void SetIsDefocused(bool bl){m_isDefocused = bl;}
			public override bool isDeactivated{get{return m_isDeactivated;}}
				bool m_isDeactivated;
				public void SetIsDeactivated(bool bl){m_isDeactivated = bl;}

			public override SGSorter Sorter{get{return m_sorter;}}
				SGSorter m_sorter;
				public override void SetSorter(SGSorter sorter){SetSorter((object)sorter);}
				public void SetSorter(object obj){if(obj == null || obj is SGSorter) m_sorter = (SGSorter) obj;}
			public override SGFilter Filter{get{return m_filter;}}
				SGFilter m_filter;
				public override void SetFilter(SGFilter sorter){SetFilter((object)sorter);}
				public void SetFilter(object obj){if(obj == null || obj is SGFilter) m_filter = (SGFilter) obj;}
			public IEnumerable<SlotSystemElement> GetElements(){return m_elements;}
				IEnumerable<SlotSystemElement> m_elements;
				public void SetElements(object obj){if(obj == null || obj is IEnumerable<SlotSystemElement>) m_elements = obj as IEnumerable<SlotSystemElement>;}
			public override int Count{get{return m_count;}}
				int m_count;
				public void SetCount(int i){m_count = i;}
			public override string eName{get{return m_eName;}}
				public void SetEName(object obj){if(obj == null || obj is string) m_eName = (string) obj;}
			public override List<Slottable> toList{get{return m_toList;}}
				List<Slottable> m_toList;
				public void SetToList(object obj){if(obj == null || obj is List<Slottable>) m_toList = obj as List<Slottable>;}
			public override Slottable pickedSB{get{return m_pickedSB;}}
				Slottable m_pickedSB;
				public void SetPickedSB(object obj){if(obj == null || obj is Slottable) m_pickedSB = (Slottable) obj;}
			public override Slottable targetSB{get{return m_targetSB;}}
				Slottable m_targetSB;
				public void SetTargetSB(object obj){if(obj == null || obj is Slottable) m_targetSB = (Slottable) obj;}

			public void ClearFields(){
				IEnumerable<System.Action<bool>> bools = new System.Action<bool>[]{
					SetIsShrinkable,
					SetIsExpandable,
					SetIsPool,
					SetIsSGE,
					SetIsSGG,
					SetIsAutoSort,
					SetIsFocusedInBundle,
					SetHasEmptySlot,
					SetIsAllTASBsDone,
					SetIsFocused,
					SetIsDefocused,
					SetIsDeactivated,
					SetAcceptsFilter,
					SetContains,
					SetHasItem,
				};
				foreach(var act in bools) act(false);
				IEnumerable<System.Action<object>> objs = new System.Action<object>[]{
					SetAxisScroller,
					SetInventory,
					SetSlots,
					SetNewSlots,
					SetSBs,
					SetNewSBs,
					SetItemInstances,
					SetActualItemInsts,
					SetEquippedSBs,
					SetSorter,
					SetFilter,
					SetEName,
					SetGetSB,
					SetGetNewSlot,
					SetSwappableSBs,
					SetToList,
					SetPickedSB,
					SetTargetSB
				};
				foreach(var act in objs) act(null);
				IEnumerable<System.Action<int>> ints = new System.Action<int>[]{
					SetActualSBsCount,
					SetInitSlotsCount,
					SetCount,
					SetIndexOf,
				};
				foreach(var act in ints) act(-1);
			}
		/*	Methods	*/
			public override void InitializeItems(){m_isInitializeItemsCalled = true;}
				bool m_isInitializeItemsCalled;
				public bool IsInitializeItemsCalled{get{return m_isInitializeItemsCalled;}}
			public override void OnActionComplete(){m_isOnActionCompleteCalled = true;}
				bool m_isOnActionCompleteCalled;
				public bool IsOnActionCompleteCalled{get{return m_isOnActionCompleteCalled;}}
			public override void OnActionExecute(){m_isOnActionExecuteCalled = true;}
				bool m_isOnActionExecuteCalled;
				public bool IsOnActionExecuteCalled{get{return m_isOnActionExecuteCalled;}}
			public override void InstantSort(){m_isInstantSortCalled = true;}
				bool m_isInstantSortCalled;
				public bool IsInstantSortCalled{get{return m_isInstantSortCalled;}}
			public override bool AcceptsFilter(Slottable pickedSB){return m_acceptsFilter;}
				bool m_acceptsFilter;
				public void SetAcceptsFilter(bool bl){m_acceptsFilter = bl;}
			public override void OnHoverEnterMock(){m_isOnHoverEnterMockCalled = true;}
				bool m_isOnHoverEnterMockCalled;
				public bool IsOnHoverEnterMockCalled{get{return m_isOnHoverEnterMockCalled;}}
			public override void OnHoverExitMock(){m_isOnHoverExitMockCalled = true;}
				bool m_isOnHoverExitMockCalled;
				public bool IsOnHoverExitMockCalled{get{return m_isOnHoverExitMockCalled;}}
			public override int IndexOf(Slottable sb){return m_indexOf;}
				int m_indexOf;
				public void SetIndexOf(int i){m_indexOf = i;}
			public override bool Contains(SlotSystemElement element){return m_contains;}
				bool m_contains;
				public void SetContains(bool bl){m_contains = bl;}
			public override void Focus(){m_isFocusCalled = true;}
				bool m_isFocusCalled;
				public bool IsFocusCalled{get{return m_isFocusCalled;}}
			public override void FocusSelf(){m_isFocusSelfCalled = true;}
				bool m_isFocusSelfCalled;
				public bool IsFocusSelfCalled{get{return m_isFocusSelfCalled;}}
			public override void FocusSBs(){m_isFocusSBsCalled = true;}
				bool m_isFocusSBsCalled;
				public bool IsFocusSBsCalled{get{return m_isFocusSBsCalled;}}
			public override void Defocus(){m_isDefocusCalled = true;}
				bool m_isDefocusCalled;
				public bool IsDefocusCalled{get{return m_isDefocusCalled;}}
			public override void DefocusSelf(){m_isDefocusSelfCalled = true;}
				bool m_isDefocusSelfCalled;
				public bool IsDefocusSelfCalled{get{return m_isDefocusSelfCalled;}}
			public override void DefocusSBs(){m_isDefocusSBsCalled = true;}
				bool m_isDefocusSBsCalled;
				public bool IsDefocusSBsCalled{get{return m_isDefocusSBsCalled;}}
			public override void Deactivate(){m_isDeactivateCalled = true;}
				bool m_isDeactivateCalled;
				public bool IsDeactivateCalled{get{return m_isDeactivateCalled;}}
			public override void PerformInHierarchy(System.Action<SlotSystemElement> act){m_isPerformInHierarchyV1Called = true;}
				bool m_isPerformInHierarchyV1Called;
				public bool IsPerformInHierarchyV1Called{get{return m_isPerformInHierarchyV1Called;}}
			public override void PerformInHierarchy(System.Action<SlotSystemElement, object> act, object obj){m_isPerformInHierarchyV2Called = true;}
				bool m_isPerformInHierarchyV2Called;
				public bool IsPerformInHierarchyV2Called{get{return m_isPerformInHierarchyV2Called;}}
			public override void PerformInHierarchy<T>(System.Action<SlotSystemElement, IList<T>> act, IList<T> list){m_isPerformInHierarchyV3Called = true;}
				bool m_isPerformInHierarchyV3Called;
				public bool IsPerformInHierarchyV3Called{get{return m_isPerformInHierarchyV3Called;}}
			

			public override void Initialize(string name, SGFilter filter, Inventory inv, bool isShrinkable, int initSlotsCount, SlotGroupCommand onActionCompleteCommand, SlotGroupCommand onActionExecuteCommand){m_isInitializeCalled = true;}
				bool m_isInitializeCalled;
				public bool IsInitializeCalled{get{return m_isInitializeCalled;}}
			public override Slottable GetSB(InventoryItemInstance itemInst){return m_getSB;}
				Slottable m_getSB;
				public void SetGetSB(object obj){if(obj == null || obj is Slottable)m_getSB = (Slottable) obj;}
			public override bool HasItem(InventoryItemInstance invInst){return m_hasItem;}
				bool m_hasItem;
				public void SetHasItem(bool bl){m_hasItem = bl;}
			public override void UpdateSBs(List<Slottable> newSBs){m_isUpdateSBsCalled = true;}
				bool m_isUpdateSBsCalled;
				public bool IsUpdateSBsCalled{get{return m_isUpdateSBsCalled;}}
			public override void CreateNewSlots(){m_isCreateNewSlotsCalled = true;}
				bool m_isCreateNewSlotsCalled;
				public bool IsCreateNewSlotsCalled{get{return m_isCreateNewSlotsCalled;}}
			public override Slot GetNewSlot(InventoryItemInstance itemInst){return m_getNewSlot;}
				Slot m_getNewSlot;
				public void SetGetNewSlot(object obj){if(obj == null || obj is Slot) m_getNewSlot = (Slot) obj;}
			public override void SetSBsActStates(){m_isSetSBsActStatesCalled = true;}
				bool m_isSetSBsActStatesCalled;
				public bool IsSetSBsActStatesCalled{get{return m_isSetSBsActStatesCalled;}}
			public override void CheckProcessCompletion(){m_isCheckProcessCompletionCalled = true;}
				bool m_isCheckProcessCompletionCalled;
				public bool IsCheckProcessCompletionCalled{get{return m_isCheckProcessCompletionCalled;}}
			public override void OnCompleteSlotMovements(){m_isOnCompleteSlotMovementsCalled = true;}
				bool m_isOnCompleteSlotMovementsCalled;
				public bool IsOnCompleteSlotMovementsCalled{get{return m_isOnCompleteSlotMovementsCalled;}}
			public override void SyncSBsToSlots(){m_isSyncSBsToSlotsCalled = true;}
				bool m_isSyncSBsToSlotsCalled;
				public bool IsSyncSBsToSlotsCalled{get{return m_isSyncSBsToSlotsCalled;}}
			public override List<Slottable> SwappableSBs(Slottable pickedSB){return m_swappableSBs;}
				List<Slottable> m_swappableSBs;
				public void SetSwappableSBs(object obj){if(obj == null || obj is List<Slottable>) m_swappableSBs = obj as List<Slottable>;}
			//
			
			public override void ReorderAndUpdateSBs(){m_isReorderAndUpdateSBsCalled = true;}
				bool m_isReorderAndUpdateSBsCalled;
				public bool IsReorderAndUpdateSBsCalled{get{return m_isReorderAndUpdateSBsCalled;}}
			public override void UpdateToRevert(){m_isUpdateToRevertCalled = true;}
				bool m_isUpdateToRevertCalled;
				public bool IsUpdateToRevertCalled{get{return m_isUpdateToRevertCalled;}}
			public override void SortAndUpdateSBs(){m_isSortAndUpdateSBsCalled = true;}
				bool m_isSortAndUpdateSBsCalled;
				public bool IsSortAndUpdateSBsCalled{get{return m_isSortAndUpdateSBsCalled;}}
			public override void FillAndUpdateSBs(){m_isFillAndUpdateSBsCalled = true;}
				bool m_isFillAndUpdateSBsCalled;
				public bool IsFillAndUpdateSBsCalled{get{return m_isFillAndUpdateSBsCalled;}}
			public override void SwapAndUpdateSBs(){m_isSwapAndUpdateSBsCalled = true;}
				bool m_isSwapAndUpdateSBsCalled;
				public bool IsSwapAndUpdateSBsCalled{get{return m_isSwapAndUpdateSBsCalled;}}
			public override void AddAndUpdateSBs(){m_isAddAndUpdateSBsCalled = true;}
				bool m_isAddAndUpdateSBsCalled;
				public bool IsAddAndUpdateSBsCalled{get{return m_isAddAndUpdateSBsCalled;}}
			public override void RemoveAndUpdateSBs(){m_isRemoveAndUpdateSBsCalled = true;}
				bool m_isRemoveAndUpdateSBsCalled;
				public bool IsRemoveAndUpdateSBsCalled{get{return m_isRemoveAndUpdateSBsCalled;}}
			
			public override void SetHovered(){m_isSetHoveredCalled = true;}
				bool m_isSetHoveredCalled;
				public bool IsSetHoveredCalled{get{return m_isSetHoveredCalled;}}
			public override void InstantGreyin(){m_isInstantGreyinCalled = true;}
				bool m_isInstantGreyinCalled;
				public bool IsInstantGreyinCalled{get{return m_isInstantGreyinCalled;}}
			public override void InstantGreyout(){m_isInstantGreyoutCalled = true;}
				bool m_isInstantGreyoutCalled;
				public bool IsInstantGreyoutCalled{get{return m_isInstantGreyoutCalled;}}
			public override void InstantHighlight(){m_isInstantHighlightCalled = true;}
				bool m_isInstantHighlightCalled;
				public bool IsInstantHighlightCalled{get{return m_isInstantHighlightCalled;}}
			public void ResetCallChecks(){
				bool[] bools = new bool[]{
					m_isInitializeItemsCalled,
					m_isOnActionCompleteCalled,
					m_isOnActionExecuteCalled,
					m_isInstantSortCalled,
					m_isOnHoverEnterMockCalled,
					m_isOnHoverExitMockCalled,
					m_isFocusCalled,
					m_isFocusSelfCalled,
					m_isFocusSBsCalled,
					m_isDefocusCalled,
					m_isDefocusSelfCalled,
					m_isDefocusSBsCalled,
					m_isDeactivateCalled,
					m_isPerformInHierarchyV1Called,
					m_isPerformInHierarchyV2Called,
					m_isPerformInHierarchyV3Called,
					m_isInitializeCalled,
					m_isUpdateSBsCalled,
					m_isCreateNewSlotsCalled,
					m_isSetSBsActStatesCalled,
					m_isCheckProcessCompletionCalled,
					m_isOnCompleteSlotMovementsCalled,
					m_isSyncSBsToSlotsCalled,
					m_isSetHoveredCalled,
					m_isInstantGreyinCalled,
					m_isInstantGreyoutCalled,
					m_isInstantHighlightCalled,
					m_isReorderAndUpdateSBsCalled,
					m_isUpdateToRevertCalled,
					m_isSortAndUpdateSBsCalled,
					m_isFillAndUpdateSBsCalled,
					m_isSwapAndUpdateSBsCalled,
					m_isAddAndUpdateSBsCalled,
					m_isRemoveAndUpdateSBsCalled
				};
				for(int i = 0; i < bools.Length; i ++){
					bools[i] = false;
				}
			}
			
	}
}
