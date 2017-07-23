using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public static class SlotSystemUtil{
		public static ISlottable CloneSB(ISlottable orig){
			if(orig != null){
				GameObject cloneGO = new GameObject("cloneGO");
				SBClone clone = cloneGO.AddComponent<SBClone>();
				clone.Initialize(orig);
				return clone;
			}
			return null;
		}
		public static ISlotGroup CloneSG(ISlotGroup orig){
			if(orig != null){
				GameObject cloneSGGO = new GameObject("cloneSGGO");
				SGClone cloneSG = cloneSGGO.AddComponent<SGClone>();
				cloneSG.Initialize(orig);
				return cloneSG;
			}
			return null;
		}
		public static bool AreSwappable(ISlottable pickedSB, ISlottable otherSB){
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
			public static string SSEDebug(ISlotSystemElement sse){
				string res = "";
				string prevSel = SSEStateNamePlain((SSEState)sse.prevSelState);
				string curSel = SSEStateName((SSEState)sse.curSelState);
				string selProc;
					if(sse.selProcess == null)
						selProc = "";
					else
						selProc = SSEProcessName(sse.selProcess) + " running? " + (sse.selProcess.isRunning?Blue("true"):Red("false"));
				res =
					sse.eName + " " +
					Bold("Sel ") + "from " + prevSel + " to " + curSel + " " +
						" proc, " + selProc + ", ";
				return res;
			}
			public static string SSEStateName(SSEState state){
				string res = "";

				if(state is SSEDeactivatedState){
					res = SlotSystemUtil.Red("SSEDeactivated");
				}else if(state is SSEDefocusedState){
					res = SlotSystemUtil.Green("SSEDefocused");
				}else if(state is SSEFocusedState){
					res = SlotSystemUtil.Blue("SSEFocused");
				}else if(state is SSESelectedState){
					res = SlotSystemUtil.Aqua("SSESelected");
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
				}
				return res;
			}
			public static string SSEProcessName(ISSEProcess process){
				string res = "";
				if(process is SSEGreyinProcess)
					res = SlotSystemUtil.Blue("Greyin");
				else if(process is SSEGreyoutProcess)
					res = SlotSystemUtil.Green("Greyout");
				else if(process is SSEHighlightProcess)
					res = SlotSystemUtil.Red("Highlight");
				else if(process is SSEDehighlightProcess)
					res = SlotSystemUtil.Brown("Dehighlight");
				return res;
			}
		/*	SSM	*/
			public static string SSMStateName(SSMState state){
				string res = "";
				if(state is SSMWaitForActionState)
					res = SlotSystemUtil.Green("WaitForAction");
				else if(state is SSMProbingState)
					res = SlotSystemUtil.Ciel("Probing");
				else if(state is SSMTransactionState)
					res = SlotSystemUtil.Terra("Transaction");
				return res;
			}
			public static string SSMStateNamePlain(SSMState state){
				string res = "";
				if(state is SSMWaitForActionState)
					res = "WaitForAction";
				else if(state is SSMProbingState)
					res = "Probing";
				else if(state is SSMTransactionState)
					res = "Transaction";
				return res;
			}
			public static string TransactionName(ISlotSystemTransaction ta){
				string res = "";
				if(ta is IRevertTransaction)
					res = SlotSystemUtil.Red("RevertTA");
				else if(ta is IReorderTransaction)
					res = SlotSystemUtil.Blue("ReorderTA");
				else if(ta is IStackTransaction)
					res = SlotSystemUtil.Aqua("StackTA");
				else if(ta is ISwapTransaction)
					res = SlotSystemUtil.Terra("SwapTA");
				else if(ta is IFillTransaction)
					res = SlotSystemUtil.Forest("FillTA");
				else if(ta is ISortTransaction)
					res = SlotSystemUtil.Khaki("SortTA");
				else if(ta is EmptyTransaction)
					res = SlotSystemUtil.Beni("Empty");
				return res;
			}
			public static string SSMProcessName(ISSMProcess proc){
				string res = "";
				if(proc is SSMGreyinProcess)
					res = SlotSystemUtil.Red("Greyin");
				else if(proc is SSMGreyoutProcess)
					res = SlotSystemUtil.Red("Greyout");
				else if(proc is SSMProbeProcess)
					res = SlotSystemUtil.Red("Probe");
				else if(proc is SSMTransactionProcess)
					res = SlotSystemUtil.Blue("Transaction");
				return res;
			}
			public static string SSMDebug(ISlotSystemManager ssm){
				string res = "";
				string pSB = SlotSystemUtil.SBofSG(ssm.pickedSB);
				string tSB = SlotSystemUtil.SBofSG(ssm.targetSB);
				string hovered = "";
					if(ssm.hovered is ISlottable)
						hovered = SBofSG((ISlottable)ssm.hovered);
					else if(ssm.hovered is ISlotGroup)
						hovered = ssm.hovered.eName;
				string di1;
					if(ssm.dIcon1 == null)
						di1 = "null";
					else
						di1 = SlotSystemUtil.SBofSG(ssm.dIcon1.sb);
				string di2;
					if(ssm.dIcon2 == null)
						di2 = "null";
					else
						di2 = SlotSystemUtil.SBofSG(ssm.dIcon2.sb);
				
				string sg1 = ssm.sg1 == null?"null":ssm.sg1.eName;
				string sg2 = ssm.sg2 == null?"null":ssm.sg2.eName;
				string prevSel = SlotSystemUtil.SSEStateNamePlain((SSEState)ssm.prevSelState);
				string curSel = SlotSystemUtil.SSEStateName((SSEState)ssm.curSelState);
				string selProc;
					if(ssm.selProcess == null)
						selProc = "";
					else
						selProc = SlotSystemUtil.SSMProcessName((ISSMProcess)ssm.selProcess) + " running? " + (ssm.selProcess.isRunning?Blue("true"):Red("false"));
				string prevAct = SlotSystemUtil.SSMStateNamePlain((SSMActState)ssm.prevActState);
				string curAct = SlotSystemUtil.SSMStateName((SSMActState)ssm.curActState);
				string actProc;
					if((ISSMProcess)ssm.actProcess == null)
						actProc = "";
					else
						actProc = SlotSystemUtil.SSMProcessName((ISSMProcess)ssm.actProcess) + " running? " + (ssm.actProcess.isRunning?Blue("true"):Red("false"));
				string ta = SlotSystemUtil.TransactionName(ssm.transaction);
				string d1Done = "d1Done: " + (ssm.dIcon1Done?SlotSystemUtil.Blue("true"):SlotSystemUtil.Red("false"));
				string d2Done = "d2Done: " + (ssm.dIcon2Done?SlotSystemUtil.Blue("true"):SlotSystemUtil.Red("false"));
				string sg1Done = "sg1Done: " + (ssm.sg1Done?SlotSystemUtil.Blue("true"):SlotSystemUtil.Red("false"));
				string sg2Done = "sg2Done: " + (ssm.sg2Done?SlotSystemUtil.Blue("true"):SlotSystemUtil.Red("false"));
				res = SlotSystemUtil.Bold("SSM:") +
						" pSB " + pSB +
						", tSB " + tSB +
						", hovered " + hovered +
						", di1 " + di1 +
						", di2 " + di2 +
						", sg1 " + sg1 +
						", sg2 " + sg2 + ", " +
					SlotSystemUtil.Bold("Sel ") + "from " + prevSel + " to " + curSel + " " +
						"proc " + selProc + ", " +
					SlotSystemUtil.Bold("Act ") + "from " + prevAct + " to " + curAct + " " +
						"proc " + actProc + ", " +
					SlotSystemUtil.Bold("TA ") + ta + ", " + 
					SlotSystemUtil.Bold("TAComp ") + d1Done + " " + d2Done + " " + sg1Done + " " + sg2Done;
				return res;
			}
		/*	SG	*/
			public static string SGStateName(SGState state){
				string res = "";
				if(state is SGWaitForActionState){
					res = SlotSystemUtil.Sangria("SGWFA");
				}else if(state is SGRevertState){
					res = SlotSystemUtil.Sangria("SGRevert");
				}else if(state is SGReorderState){
					res = SlotSystemUtil.Aqua("SGReorder");
				}else if(state is SGFillState){
					res = SlotSystemUtil.Forest("SGFill");
				}else if(state is SGSwapState){
					res = SlotSystemUtil.Berry("SGSwap");
				}else if(state is SGAddState){
					res = SlotSystemUtil.Violet("SGAdd");
				}else if(state is SGRemoveState){
					res = SlotSystemUtil.Khaki("SGRemove");
				}else if(state is SGSortState){
					res = SlotSystemUtil.Midnight("SGSort");
				}
				return res;
			}
			public static string SGStateNamePlain(SGState state){
				string res = "";
				if(state is SGWaitForActionState){
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
			public static string SGProcessName(ISGProcess proc){
				string res = "";
				if(proc is SGGreyinProcess)
					res = SlotSystemUtil.Blue("Greyin");
				else if(proc is SGGreyoutProcess)
					res = SlotSystemUtil.Green("Greyout");
				else if(proc is SGHighlightProcess)
					res = SlotSystemUtil.Red("Highlight");
				else if(proc is SGDehighlightProcess)
					res = SlotSystemUtil.Brown("Dehighlight");
				else if(proc is SGTransactionProcess)
					res = SlotSystemUtil.Khaki("Transaction");
				return res;
			}
			public static string SGDebug(ISlotGroup sg){
				string res = "";
				string prevSel = SSEStateNamePlain((SSEState)sg.prevSelState);
				string curSel = SSEStateName((SSEState)sg.curSelState);
				string selProc;
					if(sg.selProcess == null)
						selProc = "";
					else
						selProc = SGProcessName((ISGProcess)sg.selProcess) + " running? " + (sg.selProcess.isRunning?Blue("true"):Red("false"));
				string prevAct = SGStateNamePlain((SGActState)sg.prevActState);
				string curAct = SGStateName((SGActState)sg.curActState);
				string actProc;
					if(sg.actProcess == null)
						actProc = "";
					else
						actProc = SGProcessName((ISGProcess)sg.actProcess) + " running? " + (sg.actProcess.isRunning?Blue("true"):Red("false"));
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
						case 1000: result = "defWear"; break;
						case 1001: result = "crfWear"; break;
						case 1002: result = "frgWear"; break;
						case 1003: result = "mstWear"; break;
						case 2000: result = "defShield"; break;
						case 2001: result = "crfShield"; break;
						case 2002: result = "frgShield"; break;
						case 2003: result = "mstShield"; break;
						case 3000: result = "defMWeapon"; break;
						case 3001: result = "crfMWeapon"; break;
						case 3002: result = "frgMWeapon"; break;
						case 3003: result = "mstMWeapon"; break;
						case 4000: result = "defQuiver"; break;
						case 4001: result = "crfQuiver"; break;
						case 4002: result = "frgQuiver"; break;
						case 4003: result = "mstQuiver"; break;
						case 5000: result = "defPack"; break;
						case 5001: result = "crfPack"; break;
						case 5002: result = "frgPack"; break;
						case 5003: result = "mstPack"; break;
						case 6000: result = "defParts"; break;
						case 6001: result = "crfParts"; break;
						case 6002: result = "frgParts"; break;
						case 6003: result = "mstParts"; break;
					}
				}
				return result;
			}
			public static string SBName(ISlottable sb){
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
						if(sb.itemInst is BowInstance)
							result = Forest(result);
						if(sb.itemInst is WearInstance)
							result = Sangria(result);
						if(sb.itemInst is CarriedGearInstance)
							result = Terra(result);
						if(sb.itemInst is PartsInstance)
							result = Midnight(result);
					}
				}
				return result;
			}
			public static string SBofSG(ISlottable sb){
				string res = "";
				if(sb != null){
					res = SlotSystemUtil.SBName(sb) + " of " + sb.sg.eName;
					if(sb.isEquipped && sb.sg.isPool)
						res = SlotSystemUtil.Bold(res);
				}
				return res;
			}
			public static string SBStateName(SBState state){
				string result = "";
				if(state is SBActState){
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
				if(state is SBActState){
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
			public static string SBProcessName(ISBProcess process){
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
			public static string SBDebug(ISlottable sb){
				string res = "";
				if(sb == null)
					res = "null";
				else{	
					string sbName = SBofSG(sb);
					string prevSel = SSEStateNamePlain((SSEState)sb.prevSelState);
					string curSel = SSEStateName((SSEState)sb.curSelState);
					string selProc;
						if(sb.selProcess == null)
							selProc = "";
						else
							selProc = SBProcessName((ISBProcess)sb.selProcess) + " running? " + (sb.selProcess.isRunning?Blue("true"):Red("false"));
					string prevAct = SBStateNamePlain((SBActState)sb.prevActState);
					string curAct = SBStateName((SBActState)sb.curActState);
					string actProc;
						if(sb.actProcess == null)
							actProc = "";
						else
							actProc = SBProcessName((ISBProcess)sb.actProcess) + " running? " + (sb.actProcess.isRunning?Blue("true"):Red("false"));
					string prevEqp = SBStateNamePlain((SBEqpState)sb.prevEqpState);
					string curEqp = SBStateName((SBEqpState)sb.curEqpState);
					string eqpProc;
						if(sb.eqpProcess == null)
							eqpProc = "";
						else
							eqpProc = SBProcessName((ISBProcess)sb.eqpProcess) + " running? " + (sb.eqpProcess.isRunning?Blue("true"):Red("false"));
					string prevMrk = SBStateNamePlain((SBMrkState)sb.prevMrkState);
					string curMrk = SBStateName((SBMrkState)sb.curMrkState);
					string mrkProc;
						if(sb.mrkProcess == null)
							mrkProc = "";
						else
							mrkProc = SBProcessName((ISBProcess)sb.mrkProcess) + " running? " + (sb.mrkProcess.isRunning?Blue("true"):Red("false"));
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
			public static string TADebug(ISlottable testSB, ISlotSystemElement hovered){
				ISlotSystemTransaction ta = testSB.ssm.MakeTransaction(testSB, hovered);
				string taStr = TransactionName(ta);
				string taTargetSB = SlotSystemUtil.SBofSG(ta.targetSB);
				string taSG1 = ta.sg1==null?"null":ta.sg1.eName;
				string taSG2 = ta.sg2 == null? "null": ta.sg2.eName;
				return "DebugTarget: " + taStr + " " +
					"targetSB: " + taTargetSB + ", " + 
					"sg1: " + taSG1 + ", " +
					"sg2: " + taSG2
					;
			}
			public static string TADebug(ISlotSystemTransaction ta){
				string taStr = TransactionName(ta);
				string taTargetSB = SlotSystemUtil.SBofSG(ta.targetSB);
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