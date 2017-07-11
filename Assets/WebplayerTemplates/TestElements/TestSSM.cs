using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class TestSSM : SlotSystemManager {
		public Dictionary<ISlotSystemElement, ISlotSystemElement> parentDict = new Dictionary<ISlotSystemElement, ISlotSystemElement>();
		public void AddParentChild(ISlottable sb, ISlotGroup sg){
			parentDict.Add(sb, sg);
		}
		public override ISlotSystemElement FindParent(ISlotSystemElement ele){
			foreach(KeyValuePair<ISlotSystemElement, ISlotSystemElement> pair in parentDict){
				if(pair.Key == ele)
					return pair.Value;
			}
			return null;
		}
		public override void SetHovered(ISlotSystemElement ele){
			m_hovered = ele;
		}
		public override void SetActState(SSEState state){
			m_curActState = (SSMActState)state;
		}
			public SSMActState CurActState{
				get{
					return m_curActState;
				}
			}SSMActState m_curActState;
		public override void SetSelState(SSEState state){
			m_curSelState = (SSMSelState)state;
		}
			public SSMSelState CurSelState{
				get{
					return m_curSelState;
				}
			}SSMSelState m_curSelState;
		public override void SetDIcon1(DraggedIcon dIcon1){
			m_dIcon1 = dIcon1;
		}
			public override DraggedIcon dIcon1{
				get{return m_dIcon1;}
			}DraggedIcon m_dIcon1;
		public override void SetDIcon2(DraggedIcon dIcon2){
			m_dIcon2 = dIcon2;
		}
			public override DraggedIcon dIcon2{
				get{return m_dIcon2;}
			}DraggedIcon m_dIcon2;
		public override void SetPickedSB(ISlottable sb){
			m_pickedSB = sb;
		}
			public override ISlottable pickedSB{
				get{
					return m_pickedSB;
				}
			}ISlottable m_pickedSB;
		public override void CreateTransactionResults(){
			m_isCTRCalled = true;
		}
			public bool IsCTRCalled{
				get{
					return m_isCTRCalled;
				}
			}bool m_isCTRCalled;
		public override void UpdateTransaction(){
			m_isUpdateTaCalled = true;
		}
			public bool IsUpdateTaCalled{
				get{
					return m_isUpdateTaCalled;
				}
			}bool m_isUpdateTaCalled;
		public void ResetCallCheck(){
			m_isCTRCalled = false;
		}
	}
}
