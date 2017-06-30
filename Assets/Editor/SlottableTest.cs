using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using SlotSystem;
using System.Collections.Generic;
using Utility;
public class SlottableTest{
	public enum TestElement{SB, SG, SSM, TA}
	public abstract class SlotSystemTestResult{
		public bool isPAS; public bool isTAS;
		public Slottable testSB;
		public SlotSystemElement hovered;
		public bool HasSameFields(SlotSystemTestResult other){
			bool flag = true;
				flag &= AreEquivalent(testSB, other.testSB);
				flag &= AreEquivalent(hovered, other.hovered);
			return flag;
		}
		public abstract bool HasSameValue(SlotSystemTestResult other);
		protected bool AreEquivalent(object a, object b){
			bool flag = true;
			if(a != null && b != null){
				if(a is Slottable)
					if(b is Slottable)
						flag &= Util.SBsShareSGAndItem((Slottable)a, (Slottable)b);
					else return false;
				else if(a is SSEProcess || a is SlotSystemTransaction)
					flag &= a.GetType() == b.GetType();
				else
					flag &= a == b;
			}else if (!(a == null && b == null)) return false;
			return flag;
		}
		public virtual string debugString{
			get{
				string hoveredStr = "null";
				if(hovered != null)
					if(hovered is Slottable)
						hoveredStr = Util.SBofSG((Slottable)hovered);
					else
						hoveredStr = hovered.eName;
				return
					"testSB: " + (testSB == null? "null": Util.SBofSG(testSB)) + 
					", hovered: " + hoveredStr
					;
			}
		}
		public string asString{
			get{
				return "isPAS: " + (isPAS?Util.Blue("true"):Util.Red("false")) + 
					", isTAS: " + (isTAS?Util.Blue("true"):Util.Red("false"));
			}
		}
		public virtual string fullDebugString{
			get{
				return asString + "	" + debugString;
			}
		}
		}
		public class SSETestResult: SlotSystemTestResult{
			public SSEState curSelState;
			public SSEState prevSelState;
			public SSEProcess selProcess;
			public SSEState curActState;
			public SSEState prevActState;
			public SSEProcess actProcess;
			public bool isActProcessRunning;
			public SSETestResult(bool isPAS, bool isTAS, Slottable testSB, SlotSystemElement hovered, SSEState curSel, SSEState prevSel, SSEProcess selProc, SSEState curAct, SSEState prevAct, SSEProcess actProc, bool isRunning){
				this.isPAS = isPAS;
				this.isTAS = isTAS;
				this.testSB = Util.CloneSB(testSB);
				if(hovered is Slottable)
					this.hovered = Util.CloneSB((Slottable)hovered);
				else
					this.hovered = hovered;
				this.curSelState = curSel;
				this.prevSelState = prevSel;
				this.selProcess = selProc;
				this.curActState = curAct;
				this.prevActState = prevAct;
				this.actProcess = actProc;
				this.isActProcessRunning = isRunning;
			}
			public override bool HasSameValue(SlotSystemTestResult other){
				if(other is SSETestResult){
					if(!HasSameFields(other)) return false;
					bool flag = true;
						SSETestResult otherSSERes = (SSETestResult)other;
						flag &= AreEquivalent(this.curSelState, otherSSERes.curSelState);
						flag &= AreEquivalent(this.prevSelState, otherSSERes.prevSelState);
						flag &= AreEquivalent(this.selProcess, otherSSERes.selProcess);
						flag &= AreEquivalent(this.curActState, otherSSERes.curActState);
						flag &= AreEquivalent(this.prevActState, otherSSERes.prevActState);
						flag &= AreEquivalent(this.actProcess, otherSSERes.actProcess);
						flag &= this.isActProcessRunning == otherSSERes.isActProcessRunning;
					return flag;
				}else{
					return false;
				}
			}
			public override string debugString{
				get{
					return base.debugString + " " +
					"Sel from " + Util.SSEStateName((SSESelState)prevSelState)+ 
					" to " + Util.SSEStateNamePlain((SSESelState)curSelState) + 
					", selProc: " + (selProcess == null? "null": Util.SSEProcessName((SSESelProcess)selProcess)) +
					", Act from " + Util.SSEStateName((SSEActState)prevActState) + 
					" to " + Util.SSEStateNamePlain((SSEActState)curActState) + 
					", actProc: " + (actProcess == null?"null": Util.SSEProcessName((SSEActProcess)actProcess)) + 
					", running?: " + (isActProcessRunning?Util.Blue("true"):Util.Red("false"));
				}
			}
		}
		public class SSMTestResult: SlotSystemTestResult{
			public SSEState curSelState;
			public SSEState prevSelState;
			public SSEProcess selProcess;
			public SSEState curActState;
			public SSEState prevActState;
			public SSEProcess actProcess;
			public bool isActProcessRunning;
			public SlotSystemTransaction ta;
			public Slottable pickedSB;
			public Slottable targetSB;
			public Slottable di1SB;
			public Slottable di2SB;
			public SlotGroup sg1;
			public SlotGroup sg2;
			public bool di1Done;
			public bool di2Done;
			public bool sg1Done;
			public bool sg2Done;
			public SSMTestResult(
				bool isPAS, bool isTAS,
				Slottable testSB, SlotSystemElement hovered,
				SSEState curSel, SSEState prevSel, SSEProcess selProc, 
				SSEState curAct, SSEState prevAct, SSEProcess actProc, bool isRunning,
				SlotSystemTransaction ta,
				Slottable pickedSB, Slottable targetSB, Slottable di1SB, Slottable di2SB, SlotGroup sg1, SlotGroup sg2,
				bool di1Done, bool di2Done, bool sg1Done, bool sg2Done){
				this.isPAS = isPAS;
				this.isTAS = isTAS;
				this.testSB = Util.CloneSB(testSB);
				if(hovered is Slottable)
					this.hovered = Util.CloneSB((Slottable)hovered);
				else
					this.hovered = hovered;
				this.curSelState = curSel;
				this.prevSelState = prevSel;
				this.selProcess = selProc;
				this.curActState = curAct;
				this.prevActState = prevAct;
				this.actProcess = actProc;
				this.isActProcessRunning = isRunning;
				this.ta = ta;
				this.pickedSB = Util.CloneSB(pickedSB);
				this.targetSB = Util.CloneSB(targetSB);
				this.di1SB = Util.CloneSB(di1SB);
				this.di2SB = Util.CloneSB(di2SB);
				this.sg1 = sg1;
				this.sg2 = sg2;
				this.di1Done = di1Done;
				this.di2Done = di2Done;
				this.sg1Done = sg1Done;
				this.sg2Done = sg2Done;
			}
			public override string debugString{
				get{
					return base.debugString + " " +
					"Sel from " + Util.SSMStateNamePlain((SSMSelState)prevSelState)+ 
					" to " + Util.SSMStateName((SSMSelState)curSelState) + 
					", selProc: " + (selProcess == null? "null": Util.SSMProcessName((SSMSelProcess)selProcess)) +
					", Act from " + Util.SSMStateNamePlain((SSMActState)prevActState) + 
					" to " + Util.SSMStateName((SSMActState)curActState) + 
					", actProc: " + (actProcess == null?"null": Util.SSMProcessName((SSMActProcess)actProcess)) + 
					", running?: " + (isActProcessRunning?Util.Blue("true"):Util.Red("false")) +
					", TA: " + Util.TransactionName(ta) + 
					", pSB: " + Util.SBofSG(pickedSB) +
					", tSB: " + Util.SBofSG(targetSB) +
					", di1: " + Util.SBofSG(di1SB) + 
					", di2: " + Util.SBofSG(di2SB) + 
					", sg1: " + (sg1 == null? "null": sg1.eName) + 
					", sg2: " + (sg2 == null? "null": sg2.eName) + 
					", di1Done? " + (di1Done?Util.Blue("true"): Util.Red("false")) +
					", di2Done? " + (di2Done?Util.Blue("true"): Util.Red("false")) +
					", sg1Done? " + (sg1Done?Util.Blue("true"): Util.Red("false")) +
					", sg2Done? " + (sg2Done?Util.Blue("true"): Util.Red("false"))
					; 
				}
			}
			public override bool HasSameValue(SlotSystemTestResult other){
				if(other is SSMTestResult){
					if(!HasSameFields(other)) return false;
					bool flag = true;
						SSMTestResult otherSSMRes = (SSMTestResult)other;
						flag &= AreEquivalent(this.curSelState, otherSSMRes.curSelState);
						flag &= AreEquivalent(this.prevSelState, otherSSMRes.prevSelState);
						flag &= AreEquivalent(this.selProcess, otherSSMRes.selProcess);
						flag &= AreEquivalent(this.curActState, otherSSMRes.curActState);
						flag &= AreEquivalent(this.prevActState, otherSSMRes.prevActState);
						flag &= AreEquivalent(this.actProcess, otherSSMRes.actProcess);
						flag &= this.isActProcessRunning == otherSSMRes.isActProcessRunning;
						flag &= AreEquivalent(this.ta, otherSSMRes.ta);
						flag &= AreEquivalent(this.pickedSB, otherSSMRes.pickedSB);
						flag &= AreEquivalent(this.targetSB, otherSSMRes.targetSB);
						flag &= AreEquivalent(this.di1SB, otherSSMRes.di1SB);
						flag &= AreEquivalent(this.di2SB, otherSSMRes.di2SB);
						flag &= AreEquivalent(this.sg1, otherSSMRes.sg1);
						flag &= AreEquivalent(this.sg2, otherSSMRes.sg2);
						flag &= this.di1Done == otherSSMRes.di1Done;
						flag &= this.di2Done == otherSSMRes.di2Done;
						flag &= this.sg1Done == otherSSMRes.sg1Done;
						flag &= this.sg2Done == otherSSMRes.sg2Done;
					return flag;
				}else{
					return false;
				}
			}
		}
		public class SGTestResult: SlotSystemTestResult{
			public SSEState curSelState;
			public SSEState prevSelState;
			public SSEProcess selProcess;
			public SSEState curActState;
			public SSEState prevActState;
			public SSEProcess actProcess;
			public bool isActProcessRunning;
			public SGTestResult(
				bool isPAS, bool isTAS,
				Slottable testSB, SlotSystemElement hovered,
				SSEState curSel, SSEState prevSel, SSEProcess selProc, SSEState curAct, SSEState prevAct, SSEProcess actProc, bool isRunning){
				this.isPAS = isPAS;
				this.isTAS = isTAS;
				this.testSB = Util.CloneSB(testSB);
				if(hovered is Slottable)
					this.hovered = Util.CloneSB((Slottable)hovered);
				else
					this.hovered = hovered;
				this.curSelState = curSel;
				this.prevSelState = prevSel;
				this.selProcess = selProc;
				this.curActState = curAct;
				this.prevActState = prevAct;
				this.actProcess = actProc;
				this.isActProcessRunning = isRunning;
			}
			public override string debugString{
				get{
					return base.debugString + " " +
					"Sel from " + Util.SGStateNamePlain((SGSelState)prevSelState)+ 
					" to " + Util.SGStateName((SGSelState)curSelState) + 
					", selProc: " + (selProcess == null? "null": Util.SGProcessName((SGSelProcess)selProcess)) +
					", Act from " + Util.SGStateNamePlain((SGActState)prevActState) + 
					" to " + Util.SGStateName((SGActState)curActState) + 
					", actProc: " + (actProcess == null?"null": Util.SGProcessName((SGActProcess)actProcess)) + 
					", running?: " + (isActProcessRunning?Util.Blue("true"):Util.Red("false"));
				}
			}
			public override bool HasSameValue(SlotSystemTestResult other){
				if(other is SGTestResult){
					if(!HasSameFields(other)) return false;
					bool flag = true;
						SGTestResult otherSGRes = (SGTestResult)other;
						flag &= AreEquivalent(this.curSelState, otherSGRes.curSelState);
						flag &= AreEquivalent(this.prevSelState, otherSGRes.prevSelState);
						flag &= AreEquivalent(this.selProcess, otherSGRes.selProcess);
						flag &= AreEquivalent(this.curActState, otherSGRes.curActState);
						flag &= AreEquivalent(this.prevActState, otherSGRes.prevActState);
						flag &= AreEquivalent(this.actProcess, otherSGRes.actProcess);
						flag &= this.isActProcessRunning == otherSGRes.isActProcessRunning;
					return flag;
				}else{
					return false;
				}
			}
		}
		public class SBTestResult: SlotSystemTestResult{
			public SSEState curSelState;
			public SSEState prevSelState;
			public SSEProcess selProcess;
			public SSEState curActState;
			public SSEState prevActState;
			public SSEProcess actProcess;
			public bool isActProcessRunning;
			public SSEState curEqpState;
			public SSEState prevEqpState;
			public SSEProcess eqpProcess;
			public int slotID;
			public int newSlotID;
			public SBTestResult(
				bool isPAS, bool isTAS,
				Slottable testSB, SlotSystemElement hovered,
				SSEState curSel, SSEState prevSel, SSEProcess selProc,
				SSEState curAct, SSEState prevAct, SSEProcess actProc, bool isRunning, 
				SSEState curEqp, SSEState prevEqp, SSEProcess eqpProc,
				int slotID, int newSlotID){
				this.isPAS = isPAS;
				this.isTAS = isTAS;
				this.testSB = Util.CloneSB(testSB);
				if(hovered is Slottable)
					this.hovered = Util.CloneSB((Slottable)hovered);
				else
					this.hovered = hovered;
				this.curSelState = curSel;
				this.prevSelState = prevSel;
				this.selProcess = selProc;
				this.curActState = curAct;
				this.prevActState = prevAct;
				this.actProcess = actProc;
				this.isActProcessRunning = isRunning;
				this.curEqpState = curEqp;
				this.prevEqpState = prevEqp;
				this.eqpProcess = eqpProc;
				this.slotID = slotID;
				this.newSlotID = newSlotID;
			}
			public override string debugString{
				get{
					return base.debugString + " " +
					"Sel from " + Util.SBStateNamePlain((SBSelState)prevSelState)+ 
					" to " + Util.SBStateName((SBSelState)curSelState) + 
					", selProc: " + (selProcess == null? "null": Util.SBProcessName((SBSelProcess)selProcess)) +
					", Act from " + Util.SBStateNamePlain((SBActState)prevActState) + 
					" to " + Util.SBStateName((SBActState)curActState) + 
					", actProc: " + (actProcess == null?"null": Util.SBProcessName((SBActProcess)actProcess)) + 
					", running?: " + (isActProcessRunning?Util.Blue("true"):Util.Red("false")) +
					", Eqp from " + Util.SBStateNamePlain((SBEqpState)prevEqpState) + 
					" to " + Util.SBStateName((SBEqpState)curEqpState) + 
					", eqpProc: " + (eqpProcess == null?"null": Util.SBProcessName((SBEqpProcess)eqpProcess)) + 
					", slotID from " + slotID.ToString() +
					" to " + newSlotID.ToString()
					;
				}
			}
			public override bool HasSameValue(SlotSystemTestResult other){
				if(other is SBTestResult){
					if(!HasSameFields(other)) return false;
					bool flag = true;
						SBTestResult otherSBRes = (SBTestResult)other;
						flag &= AreEquivalent(this.curSelState, otherSBRes.curSelState);
						flag &= AreEquivalent(this.prevSelState, otherSBRes.prevSelState);
						flag &= AreEquivalent(this.selProcess, otherSBRes.selProcess);
						flag &= AreEquivalent(this.curActState, otherSBRes.curActState);
						flag &= AreEquivalent(this.prevActState, otherSBRes.prevActState);
						flag &= AreEquivalent(this.actProcess, otherSBRes.actProcess);
						flag &= AreEquivalent(this.curEqpState, otherSBRes.curEqpState);
						flag &= AreEquivalent(this.prevEqpState, otherSBRes.prevEqpState);
						flag &= AreEquivalent(this.eqpProcess, otherSBRes.eqpProcess);
						flag &= this.slotID == otherSBRes.slotID;
						flag &= this.newSlotID == otherSBRes.newSlotID;
						flag &= this.isActProcessRunning == otherSBRes.isActProcessRunning;
					return flag;
				}else{
					return false;
				}
			}
		}
		public class TATestResult: SlotSystemTestResult{
			Slottable targetSB;
			SlotGroup sg1;
			SlotGroup sg2;
			System.Type taType;
			SlotSystemTransaction ta;
			public TATestResult(bool isPAS, bool isTAS, Slottable testSB, SlotSystemElement hovered){
				this.isPAS = isPAS;
				this.isTAS = isTAS;
				this.testSB = Util.CloneSB(testSB);
				if(hovered is Slottable)
					this.hovered = Util.CloneSB((Slottable)hovered);
				else
					this.hovered = hovered;
				this.ta = testSB.ssm.GetTransaction(testSB, hovered);
				this.targetSB = Util.CloneSB(ta.targetSB);
				this.sg1 = ta.sg1;
				this.sg2 = ta.sg2;
				taType = ta.GetType();
			}
			public override bool HasSameValue(SlotSystemTestResult other){
				if(other is TATestResult){
					if(!HasSameFields(other)) return false;
					bool flag = true;
						TATestResult otherTARes = (TATestResult)other;
						flag &= AreEquivalent(this.targetSB, otherTARes.targetSB);
						flag &= AreEquivalent(this.sg1, otherTARes.sg1);
						flag &= AreEquivalent(this.sg2, otherTARes.sg2);
						flag &= AreEquivalent(this.taType, otherTARes.taType);
					return flag;
				}else{
					return false;
				}
			}
			public override string debugString{
				get{
					return base.debugString + " " +
					Util.TransactionName(ta) + 
					", testSB: " + Util.SBofSG(testSB) +
					", targetSB: " + Util.SBofSG(targetSB) +
					", sg1: " + (sg1 == null? "null": sg1.eName) + 
					", sg2: " + (sg2 == null? "null": sg2.eName)
					;
				}
			}
		}
			List<SlotSystemTestResult> testResults = new List<SlotSystemTestResult>();
			List<List<SlotSystemTestResult>> testResultBundles{
				get{
					List<List<SlotSystemTestResult>> result = new List<List<SlotSystemTestResult>>();
					foreach(SlotSystemTestResult res in testResults){
						bool found = false;
						foreach(List<SlotSystemTestResult> bundle in result){
							if(res.HasSameFields(bundle[0]))
								found = true;
						}
						if(!found){
							List<SlotSystemTestResult> bundle = new List<SlotSystemTestResult>();
							bundle.Add(res);
							foreach(SlotSystemTestResult res2 in testResults){
								if(res != res2)
									if(res.HasSameFields(res2)){
										bundle.Add(res2);
									}
							}
							result.Add(bundle);
						}
					}
					return result;
				}
			}
			public void PrintTestResult(){
				foreach(List<SlotSystemTestResult> bundle in testResultBundles){
					if(HasAllSameValue(bundle)){
						Debug.Log(bundle[0].debugString);
					}else{
						foreach(SlotSystemTestResult res in bundle){
							Debug.Log(res.fullDebugString);
						}
					}
				}
				ClearCResults();
				}
				bool HasAllSameValue(List<SlotSystemTestResult> bundle){
					bool flag = true;
					SlotSystemTestResult prevTr = null;
					foreach(SlotSystemTestResult res in bundle){
						flag &= (prevTr == null? true: res.HasSameValue(prevTr));
						prevTr = res;
					}
					return flag;
				}
				public void ClearCResults(){
					testResults.Clear();
				}
			public void Capture(SlotSystemManager ssm, Slottable testSB, SlotSystemElement hovered, bool isPAS, bool isTAS, TestElement ele){
					SlotSystemTestResult res = null;
				if(ele == TestElement.SB){
					if(hovered is Slottable){
						Slottable hovSB = (Slottable)hovered;
						res = new SBTestResult(isPAS, isTAS, testSB, hovSB, hovSB.curSelState, hovSB.prevSelState, hovSB.selProcess, hovSB.curActState, hovSB.prevActState, hovSB.actProcess, hovSB.actProcess == null? false: hovSB.actProcess.isRunning, hovSB.curEqpState, hovSB.prevEqpState, hovSB.eqpProcess, hovSB.slotID, hovSB.newSlotID);
					}else
						throw new System.InvalidOperationException("SlottableTest.Capture: hovered is not of type Slottable");
				}
				if(ele == TestElement.SG){
					if(hovered is SlotGroup){
						SlotGroup hovSG = (SlotGroup)hovered;
						res = new SGTestResult(isPAS, isTAS, testSB, hovered, hovSG.curSelState, hovSG.prevSelState, hovSG.selProcess, hovSG.curActState, hovSG.prevActState, hovSG.actProcess, hovSG.actProcess == null? false: hovSG.actProcess.isRunning);
					}else
						throw new System.InvalidOperationException("SlottableTest.Capture: hovered is not of type SlotGroup");
				}
				if(ele == TestElement.SSM)
					res = new SSMTestResult(isPAS, isTAS, testSB, hovered, ssm.curSelState, ssm.prevSelState, ssm.selProcess, ssm.curActState, ssm.prevActState, ssm.actProcess, ssm.actProcess == null?false: ssm.actProcess.isRunning, ssm.transaction, ssm.pickedSB, ssm.targetSB, ssm.dIcon1 == null? null: ssm.dIcon1.sb, ssm.dIcon2 == null? null: ssm.dIcon2.sb, ssm.sg1, ssm.sg2, ssm.dIcon1Done, ssm.dIcon2Done, ssm.sg1Done, ssm.sg2Done);
				if(ele == TestElement.TA)
					res = new TATestResult(isPAS, isTAS, testSB, hovered);
				testResults.Add(res);
			}
			public void Print(SlotSystemManager ssm, Slottable testSB, SlotSystemElement hovered, bool isPAS, bool isTAS, TestElement ele){
					SlotSystemTestResult res = null;
				if(ele == TestElement.SB){
					if(hovered is Slottable){
						Slottable hovSB = (Slottable)hovered;
						res = new SBTestResult(isPAS, isTAS, testSB, hovSB, hovSB.curSelState, hovSB.prevSelState, hovSB.selProcess, hovSB.curActState, hovSB.prevActState, hovSB.actProcess, hovSB.actProcess == null? false: hovSB.actProcess.isRunning, hovSB.curEqpState, hovSB.prevEqpState, hovSB.eqpProcess, hovSB.slotID, hovSB.newSlotID);
					}else
						throw new System.InvalidOperationException("SlottableTest.Capture: hovered is not of type Slottable");
				}
				if(ele == TestElement.SG){
					if(hovered is SlotGroup){
						SlotGroup hovSG = (SlotGroup)hovered;
						res = new SGTestResult(isPAS, isTAS, testSB, hovered, hovSG.curSelState, hovSG.prevSelState, hovSG.selProcess, hovSG.curActState, hovSG.prevActState, hovSG.actProcess, hovSG.actProcess == null? false: hovSG.actProcess.isRunning);
					}else
						throw new System.InvalidOperationException("SlottableTest.Capture: hovered is not of type SlotGroup");
				}
				if(ele == TestElement.SSM)
					res = new SSMTestResult(isPAS, isTAS, testSB, hovered, ssm.curSelState, ssm.prevSelState, ssm.selProcess, ssm.curActState, ssm.prevActState, ssm.actProcess, ssm.actProcess == null?false: ssm.actProcess.isRunning, ssm.transaction, ssm.pickedSB, ssm.targetSB, ssm.dIcon1 == null? null: ssm.dIcon1.sb, ssm.dIcon2 == null? null: ssm.dIcon2.sb, ssm.sg1, ssm.sg2, ssm.dIcon1Done, ssm.dIcon2Done, ssm.sg1Done, ssm.sg2Done);
				if(ele == TestElement.TA)
					res = new TATestResult(isPAS, isTAS, testSB, hovered);
				Debug.Log(res.fullDebugString);
			}
	/*	fields 	*/
		/*	other	*/
			bool picked;
			PointerEventDataMock eventData = new PointerEventDataMock();
		/*	ssm */
			GameObject ssmGO;
			SlotSystemManager ssm;
		/*	sgs	*/
			/*	pool	*/
				GameObject sgpAllGO;
				SlotGroup sgpAll;
				GameObject sgpPartsGO;
				SlotGroup sgpParts;
				GameObject sgpBowGO;
				SlotGroup sgpBow;
				GameObject sgpWearGO;
				SlotGroup sgpWear;
				GameObject sgpCGearsGO;
				SlotGroup sgpCGears;
			/*	Equip */
				GameObject sgBowGO;
				SlotGroup sgeBow;
				GameObject sgWearGO;
				SlotGroup sgeWear;
				GameObject sgCGearsGO;
				SlotGroup sgeCGears;
			/*	generic	*/
				GameObject sggGO_12;
				SlotGroup sgg_12;
				GameObject sggGO_111;
				SlotGroup sgg_111;
				GameObject sggGO_112;
				SlotGroup sgg_112;
				GameObject sggGO_21;
				SlotGroup sgg_21;
				GameObject sggGO_22;
				SlotGroup sgg_22;
				GameObject sggGO_23;
				SlotGroup sgg_23;
				GameObject sggGO_24;
				SlotGroup sgg_24;
				GameObject sggGO_2511;
				SlotGroup sgg_2511;
				GameObject sggGO_2512;
				SlotGroup sgg_2512;
				GameObject sggGO_2521;
				SlotGroup sgg_2521;
				GameObject sggGO_2522;
				SlotGroup sgg_2522;
		/*	items	*/
			/*	sbp	*/
			Slottable defBowA_p;
			Slottable defBowB_p;
			Slottable crfBowA_p;
			Slottable defWearA_p;
			Slottable defWearB_p;
			Slottable crfWearA_p;
			Slottable defShieldA_p;
			Slottable crfShieldA_p;
			Slottable defMWeaponA_p;
			Slottable crfMWeaponA_p;
			Slottable defQuiverA_p;
			Slottable defPackA_p;
			Slottable defParts_p;
			Slottable crfParts_p;
			
			Slottable defBowA_p2;
			Slottable defBowB_p2;
			Slottable crfBowA_p2;
			Slottable defWearA_p2;
			Slottable defWearB_p2;
			Slottable crfWearA_p2;
			Slottable defShieldA_p2;
			Slottable crfShieldA_p2;
			Slottable defMWeaponA_p2;
			Slottable crfMWeaponA_p2;
			Slottable defQuiverA_p2;
			Slottable defPackA_p2;
			Slottable defParts_p2;
			Slottable crfParts_p2;
			/*	sbe	*/
			Slottable defBowA_e;
			Slottable defWearA_e;
			Slottable defShieldA_e;
			Slottable defMWeaponA_e;
		/*	inventories	*/
			PoolInventory poolInv;
			EquipmentSetInventory equipInv;
			GenericInventory genInv;
		
	[SetUp]
	public void Setup(){
		/*	Items	*/
			/*	bows	*/
				BowMock defBow = new BowMock();
				defBow.ItemID = 0;
				BowMock crfBow = new BowMock();
				crfBow.ItemID = 1;
				BowInstanceMock defBowA = new BowInstanceMock();//	equipped
				defBowA.Item = defBow;
				BowInstanceMock defBowB = new BowInstanceMock();
				defBowB.Item = defBow;
				BowInstanceMock crfBowA = new BowInstanceMock();
				crfBowA.Item = crfBow;
			/*	wears	*/
				WearMock defWear = new WearMock();
				defWear.ItemID = 100;
				WearMock crfWear = new WearMock();
				crfWear.ItemID = 101;
				WearInstanceMock defWearA = new WearInstanceMock();//	equipped
				defWearA.Item = defWear;
				WearInstanceMock defWearB = new WearInstanceMock();
				defWearB.Item = defWear;
				WearInstanceMock crfWearA = new WearInstanceMock();
				crfWearA.Item = crfWear;
			/*	parts	*/
				PartsMock defParts = new PartsMock();
				defParts.ItemID = 600;
				defParts.IsStackable = true;
				PartsMock crfParts = new PartsMock();
				crfParts.ItemID = 601;
				crfParts.IsStackable = true;
				
				PartsInstanceMock defPartsA = new PartsInstanceMock();
				defPartsA.Item = defParts;
				defPartsA.Quantity = 10;
				PartsInstanceMock defPartsB = new PartsInstanceMock();
				defPartsB.Item = defParts;
				defPartsB.Quantity = 5;
				PartsInstanceMock crfPartsA = new PartsInstanceMock();
				crfPartsA.Item = crfParts;
				crfPartsA.Quantity = 3;
				Assert.That(defPartsA, Is.EqualTo(defPartsB));
				Assert.That(object.ReferenceEquals(defPartsA, defPartsB), Is.False);
			/*	carried gears	*/
				ShieldMock defShield = new ShieldMock();
				defShield.ItemID = 200;
				AB(defShield.IsStackable, false);
				
				ShieldInstanceMock defShieldA = new ShieldInstanceMock();
				defShieldA.Item = defShield;
				AE(defShieldA.Quantity, 1);
				
				ShieldMock crfShield = new ShieldMock();
				crfShield.ItemID = 201;
				AB(crfShield.IsStackable, false);

				ShieldInstanceMock crfShieldA = new ShieldInstanceMock();
				crfShieldA.Item = crfShield;
				AE(crfShieldA.Quantity, 1);

				MeleeWeaponMock defMWeapon = new MeleeWeaponMock();
				defMWeapon.ItemID = 300;
				MeleeWeaponMock crfMWeapon = new MeleeWeaponMock();
				crfMWeapon.ItemID = 301;
				MeleeWeaponInstanceMock defMWeaponA = new MeleeWeaponInstanceMock();
				defMWeaponA.Item = defMWeapon;
				MeleeWeaponInstanceMock crfMWeaponA = new MeleeWeaponInstanceMock();
				crfMWeaponA.Item = crfMWeapon;

				QuiverMock defQuiver = new QuiverMock();
				defQuiver.ItemID = 400;
				QuiverInstanceMock defQuiverA = new QuiverInstanceMock();
				defQuiverA.Item = defQuiver;

				PackMock defPack = new PackMock();
				defPack.ItemID = 500;
				PackInstanceMock defPackA = new PackInstanceMock();
				defPackA.Item = defPack;
		/*	Inventory	*/
			/*	pool inv	*/
				List<SlottableItem> items = new List<SlottableItem>();
					items.Add(defBowA);
					items.Add(defBowB);
					items.Add(crfBowA);
					items.Add(defWearA);
					items.Add(defWearB);
					items.Add(crfWearA);
					items.Add(defShieldA);
					items.Add(defMWeaponA);
					items.Add(defPartsA);
					items.Add(defPartsB);
					items.Add(crfPartsA);
					items.Add(crfShieldA);
					items.Add(crfMWeaponA);
					items.Add(defQuiverA);
					items.Add(defPackA);
				poolInv = new PoolInventory();
					foreach(SlottableItem item in items){
						poolInv.Add(item);
					}
						AE(poolInv.count, 14);
						int id = 0;
						foreach(SlottableItem item in poolInv){
							object.ReferenceEquals(item, items[id]);
							id ++;
							AB(poolInv.Contains(item), true);
						}
			/*	equip inv	*/
				List<CarriedGearInstanceMock> initCGears = new List<CarriedGearInstanceMock>();
				initCGears.Add(defShieldA);
				initCGears.Add(defMWeaponA);
				equipInv = new EquipmentSetInventory(defBowA, defWearA, initCGears, 4);
					List<SlottableItem> cgItems = new List<SlottableItem>();
						cgItems.Add(defBowA);
						cgItems.Add(defWearA);
						cgItems.Add(defShieldA);
						cgItems.Add(defMWeaponA);
					AE(equipInv.count, 4);
					id = 0;
					foreach(SlottableItem item in equipInv){
						object.ReferenceEquals(item, cgItems[id]);
						id++;
						AB(equipInv.Contains(item), true);
					}
			/*	generic inventory	*/
				genInv = new GenericInventory();
					AE(genInv.count, 0);
		/*	SGs	*/
			/*	sgpAll	*/
				sgpAllGO = new GameObject("PoolSlotGroup");
				sgpAll = sgpAllGO.AddComponent<SlotGroup>();
				sgpAll.Initialize("sgpAll",SlotGroup.NullFilter, poolInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
					ASSG(sgpAll,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpAll.isShrinkable, true);
					AB(sgpAll.isExpandable, true);
					AssertSGCounts(sgpAll,
						14,//slots
						14,//items
						14);//sbs
			/*	sgpBow	*/
				sgpBowGO = new GameObject("sgpBowGO");
				sgpBow = sgpBowGO.AddComponent<SlotGroup>();
				sgpBow.Initialize("sgpBow" ,SlotGroup.BowFilter, poolInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
					ASSG(sgpBow,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpBow.isShrinkable, true);
					AB(sgpBow.isExpandable, true);
					AssertSGCounts(sgpBow,
						3,//slots
						3,//items
						3);//sbs
			/*	sgpWear	*/
				sgpWearGO = new GameObject("sgpWearGO");
				sgpWear = sgpWearGO.AddComponent<SlotGroup>();
				sgpWear.Initialize("sgpWear", SlotGroup.WearFilter, poolInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
					ASSG(sgpWear,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpWear.isShrinkable, true);
					AB(sgpWear.isExpandable, true);
					AssertSGCounts(sgpWear,
						3,//slots
						3,//items
						3);//sbs
			/*	sgpCGears	*/
				sgpCGearsGO = new GameObject("sgpCGearsGO");
				sgpCGears = sgpCGearsGO.AddComponent<SlotGroup>();
				sgpCGears.Initialize("sgpCGears", SlotGroup.CGearsFilter, poolInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
					ASSG(sgpCGears,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpCGears.isShrinkable, true);
					AB(sgpCGears.isExpandable, true);
					AssertSGCounts(sgpCGears,
						6,//slots
						6,//items
						6);//sbs
			/*	sgpParts	*/
				sgpPartsGO = new GameObject("PartsSlotGroupPool");
				sgpParts = sgpPartsGO.AddComponent<SlotGroup>();
				sgpParts.Initialize("sgpParts", SlotGroup.PartsFilter, poolInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
					ASSG(sgpParts,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgpParts.isShrinkable, true);
					AB(sgpParts.isExpandable, true);
					AssertSGCounts(sgpParts,
						2,//slots
						2,//items
						2);//sbs
			/*	sgBow	*/
				sgBowGO = new GameObject("BowSlotGroup");
				sgeBow = sgBowGO.AddComponent<SlotGroup>();
				sgeBow.Initialize("sgeBow", SlotGroup.BowFilter, equipInv, false, 1, SlotGroup.emptyCommand, SlotGroup.updateEquipAtExecutionCommand);
					ASSG(sgeBow,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgeBow.isShrinkable, false);
					AB(sgeBow.isExpandable, false);
					AssertSGCounts(sgeBow,
						1,//slots
						1,//items
						1);//sbs
			/*	sgWear	*/
				sgWearGO = new GameObject("WearSlotGroup");
				sgeWear = sgWearGO.AddComponent<SlotGroup>();
				sgeWear.Initialize("sgeWear", SlotGroup.WearFilter, equipInv, false, 1, SlotGroup.emptyCommand, SlotGroup.updateEquipAtExecutionCommand);
					ASSG(sgeWear,
						SGDeactivated, SGDeactivated, null,
						SGWFA, SGWFA, null, false);
					AB(sgeWear.isShrinkable, false);
					AB(sgeWear.isExpandable, false);
					AssertSGCounts(sgeWear,
						1,//slots
						1,//items
						1);//sbs
			/*	sgCGears	*/
				sgCGearsGO = new GameObject("CarriedGearsSG");
				sgeCGears = sgCGearsGO.AddComponent<SlotGroup>();
				equipInv.SetEquippableCGearsCount(4);
				int slotCount = equipInv.equippableCGearsCount;
				sgeCGears.Initialize("sgeCGears", SlotGroup.CGearsFilter, equipInv, true, slotCount, SlotGroup.emptyCommand, SlotGroup.updateEquipAtExecutionCommand);
					ASSG(sgeCGears,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AB(sgeCGears.isShrinkable, true);
						AB(sgeCGears.isExpandable, false);
						AssertSGCounts(sgeCGears,
							4,//slots
							2,//items
							2);//sbs
			/*	sgGen	*/
				/*	sgg_12	*/
					sggGO_12 = new GameObject("sggGO_12");
					sgg_12 = sggGO_12.AddComponent<SlotGroup>();
					sgg_12.Initialize("sgg_12", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_12,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_12.isShrinkable, true);
						AE(sgg_12.isExpandable, true);
						AssertSGCounts(sgg_12, 0, 0, 0);
				/*	sgg_111	*/
					sggGO_111 = new GameObject("sggGO_111");
					sgg_111 = sggGO_111.AddComponent<SlotGroup>();
					sgg_111.Initialize("sgg_111", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_111,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_111.isShrinkable, true);
						AE(sgg_111.isExpandable, true);
						AssertSGCounts(sgg_111, 0, 0, 0);
				/*	sgg_112	*/
					sggGO_112 = new GameObject("sggGO_112");
					sgg_112 = sggGO_112.AddComponent<SlotGroup>();
					sgg_112.Initialize("sgg_112", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_112,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_112.isShrinkable, true);
						AE(sgg_112.isExpandable, true);
						AssertSGCounts(sgg_112, 0, 0, 0);
				/*	sgg_21	*/
					sggGO_21 = new GameObject("sggGO_21");
					sgg_21 = sggGO_21.AddComponent<SlotGroup>();
					sgg_21.Initialize("sgg_21", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_21,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_21.isShrinkable, true);
						AE(sgg_21.isExpandable, true);
						AssertSGCounts(sgg_21, 0, 0, 0);
				/*	sgg_22	*/
					sggGO_22 = new GameObject("sggGO_22");
					sgg_22 = sggGO_22.AddComponent<SlotGroup>();
					sgg_22.Initialize("sgg_22", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_22,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_22.isShrinkable, true);
						AE(sgg_22.isExpandable, true);
						AssertSGCounts(sgg_22, 0, 0, 0);
				/*	sgg_23	*/
					sggGO_23 = new GameObject("sggGO_23");
					sgg_23 = sggGO_23.AddComponent<SlotGroup>();
					sgg_23.Initialize("sgg_23", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_23,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_23.isShrinkable, true);
						AE(sgg_23.isExpandable, true);
						AssertSGCounts(sgg_23, 0, 0, 0);
				/*	sgg_24	*/
					sggGO_24 = new GameObject("sgg_24GO");
					sgg_24 = sggGO_24.AddComponent<SlotGroup>();
					sgg_24.Initialize("sgg_24", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_24,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_24.isShrinkable, true);
						AE(sgg_24.isExpandable, true);
						AssertSGCounts(sgg_24, 0, 0, 0);
				/*	sgg_2511	*/
					sggGO_2511 = new GameObject("sgg_2511GO");
					sgg_2511 = sggGO_2511.AddComponent<SlotGroup>();
					sgg_2511.Initialize("sgg_2511", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_2511,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_2511.isShrinkable, true);
						AE(sgg_2511.isExpandable, true);
						AssertSGCounts(sgg_2511, 0, 0, 0);
				/*	sgg_2512	*/
					sggGO_2512 = new GameObject("sgg_2512GO");
					sgg_2512 = sggGO_2512.AddComponent<SlotGroup>();
					sgg_2512.Initialize("sgg_2512", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_2512,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_2512.isShrinkable, true);
						AE(sgg_2512.isExpandable, true);
						AssertSGCounts(sgg_2512, 0, 0, 0);
				/*	sgg_2521	*/
					sggGO_2521 = new GameObject("sgg_2521GO");
					sgg_2521 = sggGO_2521.AddComponent<SlotGroup>();
					sgg_2521.Initialize("sgg_2521", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_2521,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_2521.isShrinkable, true);
						AE(sgg_2521.isExpandable, true);
						AssertSGCounts(sgg_2521, 0, 0, 0);
				/*	sgg_2522	*/
					sggGO_2522 = new GameObject("sgg_2522GO");
					sgg_2522 = sggGO_2522.AddComponent<SlotGroup>();
					sgg_2522.Initialize("sgg_2522", SlotGroup.NullFilter, genInv, true, 0, SlotGroup.emptyCommand, SlotGroup.emptyCommand);
						ASSG(sgg_2522,
							SGDeactivated, SGDeactivated, null,
							SGWFA, SGWFA, null, false);
						AE(sgg_2522.isShrinkable, true);
						AE(sgg_2522.isExpandable, true);
						AssertSGCounts(sgg_2522, 0, 0, 0);
		/*	SBs	*/
			defBowA_p = sgpAll.GetSB(defBowA);
			defBowB_p = sgpAll.GetSB(defBowB);
			crfBowA_p = sgpAll.GetSB(crfBowA);
			defWearA_p = sgpAll.GetSB(defWearA);
			defWearB_p = sgpAll.GetSB(defWearB);
			crfWearA_p = sgpAll.GetSB(crfWearA);
			defShieldA_p = sgpAll.GetSB(defShieldA);
			crfShieldA_p = sgpAll.GetSB(crfShieldA);
			defMWeaponA_p =	sgpAll.GetSB(defMWeaponA); 
			crfMWeaponA_p =	sgpAll.GetSB(crfMWeaponA);
			defQuiverA_p = sgpAll.GetSB(defQuiverA);
			defPackA_p = sgpAll.GetSB(defPackA);
			defParts_p = sgpAll.GetSB(defPartsA);
			crfParts_p = sgpAll.GetSB(crfPartsA);

			defBowA_p2 = sgpBow.GetSB(defBowA);
			defBowB_p2 = sgpBow.GetSB(defBowB);
			crfBowA_p2 = sgpBow.GetSB(crfBowA);
			defWearA_p2 = sgpWear.GetSB(defWearA);
			defWearB_p2 = sgpWear.GetSB(defWearB);
			crfWearA_p2 = sgpWear.GetSB(crfWearA);
			defShieldA_p2 = sgpCGears.GetSB(defShieldA);
			crfShieldA_p2 = sgpCGears.GetSB(crfShieldA);
			defMWeaponA_p2 = sgpCGears.GetSB(defMWeaponA); 
			crfMWeaponA_p2 = sgpCGears.GetSB(crfMWeaponA);
			defQuiverA_p2 = sgpCGears.GetSB(defQuiverA);
			defPackA_p2 = sgpCGears.GetSB(defPackA);
			defParts_p2 = sgpParts.GetSB(defPartsA);
			crfParts_p2 = sgpParts.GetSB(crfPartsA);

			defBowA_e = sgeBow.GetSB(defBowA);
			defWearA_e = sgeWear.GetSB(defWearA);
			defShieldA_e = sgeCGears.GetSB(defShieldA);
			defMWeaponA_e = sgeCGears.GetSB(defMWeaponA);
		/*	SSM	*/
			ssmGO = new GameObject("ssmGO");
			ssm = ssmGO.AddComponent<SlotSystemManager>();
			/*	equip bundle	*/
				/*	page elements	*/
					SlotSystemPageElement sgeBowPE = new SlotSystemPageElement(sgeBow, true);
					SlotSystemPageElement sgeWearPE = new SlotSystemPageElement(sgeWear, true);
					SlotSystemPageElement sgeCGearsPE = new SlotSystemPageElement(sgeCGears, true);
				GameObject eSetGO = new GameObject("eSetGO");
				EquipmentSet equipSetA = eSetGO.AddComponent<EquipmentSet>();
				equipSetA.Initialize(sgeBowPE, sgeWearPE, sgeCGearsPE);
				IEnumerable<SlotSystemElement> eBunEles = new SlotSystemElement[]{equipSetA};
				GameObject eBunGO = new GameObject("eBunGO");
				SlotSystemBundle equipBundle = eBunGO.AddComponent<SlotSystemBundle>();
				equipBundle.Initialize("eBundle", eBunEles);
				equipBundle.SetFocusedBundleElement(equipSetA);
				SlotSystemPageElement equipBundlePE = new SlotSystemPageElement(equipBundle, true);
			/*	pool bundle	*/
				IEnumerable<SlotSystemElement> pBunEles = new SlotSystemElement[]{sgpAll, sgpBow, sgpWear, sgpCGears, sgpParts};
				GameObject pBunGO = new GameObject("pBunGO");
				SlotSystemBundle poolBundle = pBunGO.AddComponent<SlotSystemBundle>();
				poolBundle.Initialize("pBundle", pBunEles);
				poolBundle.SetFocusedBundleElement(sgpAll);
				SlotSystemPageElement poolBundlePE = new SlotSystemPageElement(poolBundle, true);
			/*	generic pages	*/
				/*	gPag_11	*/
						SlotSystemPageElement sgg_111PE = new SlotSystemPageElement(sgg_111, true);
						SlotSystemPageElement sgg_112PE = new SlotSystemPageElement(sgg_112, false);
					IEnumerable<SlotSystemPageElement> gPageEles_11 = new SlotSystemPageElement[]{sgg_111PE, sgg_112PE};
					GameObject gPageGO_11 = new GameObject("gPageGO_11");
					GenericPage gPage_11 = gPageGO_11.AddComponent<GenericPage>();
					gPage_11.Initialize("gPage_11", gPageEles_11);
				/*	gPag_251	*/
						SlotSystemPageElement sgg_2511PE = new SlotSystemPageElement(sgg_2511, true);
						SlotSystemPageElement sgg_2512PE = new SlotSystemPageElement(sgg_2512, false);
					IEnumerable<SlotSystemPageElement> gPageEles_251 = new SlotSystemPageElement[]{sgg_2511PE, sgg_2512PE};
					GameObject gPageGO_251 = new GameObject("gPageGO_251");
					GenericPage gPage_251 = gPageGO_251.AddComponent<GenericPage>();
					gPage_251.Initialize("gPage_251", gPageEles_251);
				/*	gPag_252	*/
						SlotSystemPageElement sgg_2521PE = new SlotSystemPageElement(sgg_2521, false);
						SlotSystemPageElement sgg_2522PE = new SlotSystemPageElement(sgg_2522, true);
					IEnumerable<SlotSystemPageElement> gPageEles_252 = new SlotSystemPageElement[]{sgg_2521PE, sgg_2522PE};
					GameObject gPageGO_252 = new GameObject("gPageGO_252");
					GenericPage gPage_252 = gPageGO_252.AddComponent<GenericPage>();
					gPage_252.Initialize("gPage_252", gPageEles_252);
				
			/*	generic bundle	*/
				/*	gBun_1	*/
					IEnumerable<SlotSystemElement> gBunEles_1 = new SlotSystemElement[]{gPage_11, sgg_12};
					GameObject gBunGO_1 = new GameObject("gBunGO_1");
					SlotSystemBundle gBundle_1 = gBunGO_1.AddComponent<SlotSystemBundle>();
					gBundle_1.Initialize("gBundle_1", gBunEles_1);
					gBundle_1.SetFocusedBundleElement(gPage_11);
				/*	gBun_25	*/
					IEnumerable<SlotSystemElement> gBunEles_25 = new SlotSystemElement[]{gPage_251, gPage_252};
					GameObject gBunGO_25 = new GameObject("gBunGO_25");
					SlotSystemBundle gBundle_25 = gBunGO_25.AddComponent<SlotSystemBundle>();
					gBundle_25.Initialize("gBundle_25", gBunEles_25);
					gBundle_25.SetFocusedBundleElement(gPage_251);
				/*	gBun_2	*/
					IEnumerable<SlotSystemElement> gBunEles_2 = new SlotSystemElement[]{sgg_21, sgg_22, sgg_23, sgg_24, gBundle_25};
					GameObject gBunGO_2 = new GameObject("gBunGO_2");
					SlotSystemBundle gBundle_2 = gBunGO_2.AddComponent<SlotSystemBundle>();
					gBundle_2.Initialize("gBundle_2", gBunEles_2);
					gBundle_2.SetFocusedBundleElement(sgg_21);
			/*	generic Bundles	*/
				IEnumerable<SlotSystemBundle> gBundles = new SlotSystemBundle[]{gBundle_1, gBundle_2};
				SlotSystemPageElement gBundle_1PE = new SlotSystemPageElement(gBundle_1, true);
				SlotSystemPageElement gBundle_2PE = new SlotSystemPageElement(gBundle_2, false);
				IEnumerable<SlotSystemPageElement> gBundlesPE = new SlotSystemPageElement[]{gBundle_1PE, gBundle_2PE};
			
		ssm.Initialize(poolBundlePE, equipBundlePE, gBundlesPE);
			AssertSlotSystemSSMSetRight(ssm);
			AssertParenthood();
			AssertImmediateBundle();
			AssertContainInHierarchy();
			AssertBundlesPagesAndSGsMembership();
			AssertSBsMembership();
			AssertInitialize();
		ssm.Activate();
		AssertFocused();
	}
	[Test]
	public void TestAll(){
		// CheckTransacitonWithSBSpecifiedOnAll();
		// CheckTransactionOnAllSG();
		// TestAllTACheck();
		/*	TAs	*/
			// TestVolSortOnAll();
			// TestRevertOnAll();
			// TestReorderOnAll();
			// TestFillOnAll();
			// TestSwapOnAll();
			// TestSwapSelectively(sgpAll, sgeBow);
			// TestFillSelectively(sgpAll, sgg_111);
		TestLambda();
	}
	/*	Lambda Expression	*/
		public void TestLambda(){
			// Transformer<int> sqr =  x => x * x;
			System.Func<int, int> sqr =  x => x * x;
			AE(sqr(3), 9);
			System.Func<int, int ,int> mult = (x, y) => x * y;
			AE(mult(10, 5), 50);
			int someInt = 1;
			System.Func<int, int ,int> complex = (i, j) => {
				int sum = i + j;
				int prod = i * j;
				return (int)sum * prod - someInt;
			};
			someInt = 100;
			Debug.Log("complex: " + complex(sqr(2), mult(2, 2)).ToString());
		}
	/*	Event and Delegate examples */
		// public delegate int Transformer(int i);
		public delegate T Transformer<T>(T arg);
		public int Square(int i){
			return i*i;
		}
		public int Cube(int i){
			return i* i * i;
		}
		public float Cube(float f){
			return f * f;
		}
		// public void TransformAll(IList<int> ints, Transformer<int> t){
		// 	int count = 0;
		// 	foreach(int i in ints){
		// 		ints[count++] = t(i);
		// 	}
		// }
		// public void TransformAll<T>(IList<T> list, Transformer<T> t){
		// 	int count = 0;
		// 	foreach(var i in list){
		// 		list[count++] = t(i);
		// 	}
		// }
		public delegate void ValueChangedEventHandler<T>(object source, T eventArgs) where T: System.EventArgs;
		public class ValueChangedEventArgs: System.EventArgs{
			public readonly float oldValue;
			public readonly float newValue;
			public ValueChangedEventArgs(float oldVal, float newVal){
				this.oldValue = oldVal;
				this.newValue = newVal;
			}
		}
		public class ValueChangeBroadcaster{
			public event ValueChangedEventHandler<ValueChangedEventArgs> handler;
			public float value{
				get{return m_value;}
				set{
					if(m_value != value){
						ValueChangedEventArgs eventArgs = new ValueChangedEventArgs(m_value, value);
						OnValueChange(eventArgs);
					}
					m_value = value;
				}
				}float m_value = 0f;
			public void OnValueChange(ValueChangedEventArgs e){
				if(handler != null) handler(this, e);
			}
		}
		public class ValueChangeSubscriber{
			public ValueChangeSubscriber(string name, ValueChangeBroadcaster caster){
				this.name = name;
				caster.handler += Print;
				this.caster = caster;
			}
			~ValueChangeSubscriber(){
				caster.handler -= Print;
			}
			ValueChangeBroadcaster caster; 
			string name = "";
			void Print(object source, ValueChangedEventArgs eventArgs){
				Debug.Log(name + " received value changed event call: from " + eventArgs.oldValue.ToString() + " to " + eventArgs.newValue.ToString());
			}
		}
		public void TransformAll<T>(IList<T> list, System.Func<T, T> func){
			int count = 0;
			foreach(var i in list){
				list[count++] = func(i);
			}
		}

		delegate void Printer(string str);
		public void PrintRed(string str){
			Debug.Log(Util.Red(str));
		}
		public void PrintBlue(string str){
			Debug.Log(Util.Blue(str));
		}
		public void TestEventAndDelegate(){
			// int[] ints = new int[]{1, 2, 3, 4};
			// TransformAll(ints, Cube);
			// foreach(int i in ints){
			// 	Debug.Log(i.ToString());
			// }
			// float[] floats = new float[]{.2f, .3f, .4f};
			// TransformAll(floats, Cube);
			// foreach(float f in floats){
			// 	Debug.Log(f.ToString());
			// }
			ValueChangeBroadcaster broadcaster = new ValueChangeBroadcaster();
			ValueChangeSubscriber listenerA = new ValueChangeSubscriber("listenerA", broadcaster);
			ValueChangeSubscriber listenerB = new ValueChangeSubscriber("listenerB", broadcaster);

			broadcaster.value = 2f;
			broadcaster.value = 3.3f;
			broadcaster.value = 122f;

			
		}
	/*	ssm features test */
		public void TestSSMPointFocusAll(){
			PerformOnAllSGAfterFocusing(TestSSMPointFocusAll);
			}
			public void TestSSMPointFocusAll(SlotGroup sg, bool isPAS){
				ssm.PointFocus(sg);
				AssertFocused();
				string ASstring = "isPAS? " + (isPAS?Util.Blue("true"):Util.Red("false"));
				Debug.Log(Util.Bold(ASstring +" "+  sg.eName + " is point focused"));
				PrintSystemHierarchyDetailed(ssm);
			}
		public void TestPrePickFilteringV2(){
			foreach(Slottable sb in ssm.allSBs){
				if(sb.sg.isFocusedInHierarchy){
					bool isFilteredIn;
					ssm.PrePickFilter(sb, out isFilteredIn);
					Debug.Log(Util.SBofSG(sb) + " is filtered? " + (isFilteredIn?Util.Blue("true"): Util.Red("false")));
				}
			}
		}
		public void TestPrePickFiltering(){
			foreach(Slottable sb in ssm.allSBs){
				Dictionary<SlotSystemElement, SlotSystemTransaction> taDict = new Dictionary<SlotSystemElement, SlotSystemTransaction>();
				CheckAndAddNonReverts(sb, taDict);
				foreach(KeyValuePair<SlotSystemElement, SlotSystemTransaction> taPair in taDict){
					SlotSystemTransaction ta = taPair.Value;
					SlotSystemElement ele = taPair.Key;
					string eleName = ele.eName;
					if(ele is Slottable)
						eleName = Util.SBofSG((Slottable)ele);
					string stacked = Util.TransactionName(ta) + " on " + eleName;
					Util.Stack(stacked);
				}
				Debug.Log(Util.SBofSG(sb)+ " 's non-revert tas: " + Util.Stacked);
			}
			}
			public void CheckAndAddNonReverts(Slottable testSB, Dictionary<SlotSystemElement,SlotSystemTransaction> result){
				foreach(SlotGroup sg in ssm.focusedSGs){
					SlotSystemTransaction ta = ssm.GetTransaction(testSB, sg);
					if(!(ta is RevertTransaction)){
						result.Add(sg, ta);
					}
					foreach(Slottable targetSB in sg){
						if(targetSB != null){
							SlotSystemTransaction ta2 = ssm.GetTransaction(testSB, targetSB);
							if(!(ta2 is RevertTransaction)){
								result.Add(targetSB, ta2);
							}
						}
					}
				}
			}
		
	/*	SlotSystem testing*/
		public void TestFindAndFocusElement(){
			ssm.PerformInHierarchy(AssertFocusInBundle);
		}
			public void AssertFocusInBundle(SlotSystemElement target){
				ssm.FindAndFocusInBundle(target);
				AssertFocused();
			}
		public void AssertContainInHierarchy(){
			SlotSystemBundle pBun = ssm.poolBundle;
			ANotNull(pBun);
			List<SlotSystemElement> positives = new List<SlotSystemElement>();
			positives.Add(ssm);
			AssertCIHonAllAndNotInOther(positives, pBun);
			positives.Add(pBun);
			foreach(SlotSystemElement ele in pBun){
				SlotGroup sg = (SlotGroup)ele;
				AssertCIHonAllAndNotInOther(positives, sg);
				positives.Add(sg);
				foreach(Slottable sb in sg){
					AssertCIHonAllAndNotInOther(positives, sb);
				}
				positives.Remove(sg);
			}
			positives.Remove(pBun);
			SlotSystemBundle eBun = ssm.equipBundle;
			ANotNull(eBun);
			AssertCIHonAllAndNotInOther(positives, eBun);
			positives.Add(eBun);
			foreach(EquipmentSet eSet in eBun){
				AssertCIHonAllAndNotInOther(positives, eSet);
				positives.Add(eSet);
				foreach(SlotSystemElement ele in eSet){
					SlotGroup sg = (SlotGroup)ele;
					AssertCIHonAllAndNotInOther(positives, sg);
					positives.Add(sg);
					foreach(Slottable sb in sg){
						if(sb!= null)
						AssertCIHonAllAndNotInOther(positives, sb);
					}
					positives.Remove(sg);
				}
				positives.Remove(eSet);
			}
			positives.Remove(eBun);
			SlotSystemBundle gBun;
			foreach(SlotSystemBundle gB in ssm.otherBundles){
				gBun = gB;
				ANotNull(gBun);
				AssertCIHonAllAndNotInOther(positives, gBun);
				positives.Add(gBun);
				foreach(SlotSystemElement ele in gBun){
					if(ele is GenericPage){
						GenericPage gPage = (GenericPage)ele;
						AssertCIHonAllAndNotInOther(positives, gPage);
						positives.Add(gPage);
						foreach(SlotSystemElement ele2 in gPage){
							SlotGroup sg = (SlotGroup)ele2;
							AssertCIHonAllAndNotInOther(positives, sg);
							positives.Add(sg);
							foreach(Slottable sb in sg){
								if(sb!= null)
									AssertCIHonAllAndNotInOther(positives, sb);
							}
							positives.Remove(sg);
						}
						positives.Remove(gPage);
					}else if(ele is SlotGroup){
						SlotGroup sg = (SlotGroup)ele;
						AssertCIHonAllAndNotInOther(positives, sg);
						positives.Add(sg);
						foreach(Slottable sb in sg){
							if(sb!= null)
								AssertCIHonAllAndNotInOther(positives, sb);
						}
						positives.Remove(sg);
					}
				}
				positives.Remove(gBun);
			}
		}
			public void AssertCIHonAllAndNotInOther(IList<SlotSystemElement> positives, SlotSystemElement target){
				CheckCIH(ssm, positives, target);
				SlotSystemBundle pBun = ssm.poolBundle;
				CheckCIH(pBun, positives, target);
				foreach(SlotSystemElement ele in pBun){
					SlotGroup sg = (SlotGroup)ele;
					CheckCIH(sg, positives, target);
					foreach(Slottable sb in sg){
						CheckCIH(sb, positives, target);
					}
				}
				SlotSystemBundle eBun = ssm.equipBundle;
				CheckCIH(eBun, positives, target);
				foreach(EquipmentSet eSet in eBun){
					CheckCIH(eSet, positives, target);
					foreach(SlotSystemElement ele in eSet){
						SlotGroup sg = (SlotGroup)ele;
						CheckCIH(sg, positives, target);
						foreach(Slottable sb in sg){
							if(sb!= null)
								CheckCIH(sb, positives, target);
						}
					}
				}
				SlotSystemBundle gBun;
				foreach(SlotSystemBundle gB in ssm.otherBundles){
					gBun = gB;
					CheckCIH(gBun, positives, target);
					foreach(SlotSystemElement ele in gBun){
						if(ele is GenericPage){
							GenericPage gPage = (GenericPage)ele;
							CheckCIH(gPage, positives, target);
							foreach(SlotSystemElement ele2 in gPage){
								SlotGroup sg = (SlotGroup)ele2;
								CheckCIH(sg, positives, target);
								foreach(Slottable sb in sg){
									if(sb!= null)
										CheckCIH(sb, positives, target);
								}
							}
						}else if(ele is SlotGroup){
							SlotGroup sg = (SlotGroup)ele;
							CheckCIH(sg, positives, target);
							foreach(Slottable sb in sg){
								if(sb!= null)
									CheckCIH(sb, positives, target);
							}
						}
					}
				}
			}
			public void CheckCIH(SlotSystemElement subject, IList<SlotSystemElement> positives, SlotSystemElement target){
				if(positives.Contains(subject))
					AB(subject.ContainsInHierarchy(target), true);
				else
					AB(subject.ContainsInHierarchy(target), false);
			}
			public void PrintCIH(SlotSystemElement ele){
				Debug.Log(Util.Bold("Target: ") + ele.eName);
				ssm.PerformInHierarchy(CheckAndPrintCIH, ele);
			}
				public void CheckAndPrintCIH(SlotSystemElement ele, object obj){
					SlotSystemElement target = (SlotSystemElement)obj;
					string res = Indent(ele.level) + ele.eName;
					if(ele.ContainsInHierarchy(target))
						res += Util.Blue(": Contains");
					else
						res += Util.Red(": NOT Contains");
					Debug.Log(res);
				}
		public void AssertParenthood(){
			SlotSystemBundle pBun = ssm.poolBundle;
			SlotSystemBundle eBun = ssm.equipBundle;
			SlotSystemBundle gBun;
			ANotNull(pBun);
			ANotNull(eBun);
			ANull(ssm.parent);
			AE(pBun.parent, ssm);
			foreach(SlotSystemElement ele in pBun){
				SlotGroup sg = (SlotGroup)ele;
				AE(sg.parent, pBun);
				foreach(Slottable sb in sg){
					AE(sb.parent, sg);
				}
			}
			AE(eBun.parent, ssm);
			foreach(EquipmentSet eSet in eBun){
				AE(eSet.parent, eBun);
				foreach(SlotSystemElement ele in eSet){
					SlotGroup sg = (SlotGroup)ele;
					AE(sg.parent, eSet);
					foreach(Slottable sb in sg){
						if(sb!= null)
						AE(sb.parent, sg);
					}
				}
			}
			foreach(SlotSystemBundle gB in ssm.otherBundles){
				gBun = gB;
				AE(gBun.parent, ssm);
				foreach(SlotSystemElement ele in gBun){
					if(ele is GenericPage){
						GenericPage gPage = (GenericPage)ele;
						AE(gPage.parent, gBun);
						foreach(SlotSystemElement ele2 in gPage){
							SlotGroup sg = (SlotGroup)ele2;
							AE(sg.parent, gPage);
							foreach(Slottable sb in sg){
								if(sb!= null)
								AE(sb.parent, sg);
							}
						}
					}else if(ele is SlotGroup){
						SlotGroup sg = (SlotGroup)ele;
						AE(sg.parent, gBun);
						foreach(Slottable sb in sg){
							if(sb!= null)
							AE(sb.parent, sg);
						}
					}
				}
			}
		}
		public void AssertImmediateBundle(){
			SlotSystemBundle pBun = ssm.poolBundle;
			SlotSystemBundle eBun = ssm.equipBundle;
			SlotSystemBundle gBun;
			ANotNull(pBun);
			ANotNull(eBun);
			ANull(ssm.immediateBundle);
			ANull(pBun.immediateBundle);
			foreach(SlotSystemElement ele in pBun){
				SlotGroup sg = (SlotGroup)ele;
				AE(sg.immediateBundle, pBun);
				foreach(Slottable sb in sg){
					AE(sb.immediateBundle, pBun);
				}
			}
			ANull(eBun.immediateBundle);
			foreach(EquipmentSet eSet in eBun){
				AE(eSet.immediateBundle, eBun);
				foreach(SlotSystemElement ele in eSet){
					SlotGroup sg = (SlotGroup)ele;
					AE(sg.immediateBundle, eBun);
					foreach(Slottable sb in sg){
						if(sb!= null)
						AE(sb.immediateBundle, eBun);
					}
				}
			}
			foreach(SlotSystemBundle gB in ssm.otherBundles){
				gBun = gB;
				ANull(gBun.immediateBundle);
				foreach(SlotSystemElement ele in gBun){
					if(ele is GenericPage){
						GenericPage gPage = (GenericPage)ele;
						AE(gPage.immediateBundle, gBun);
						foreach(SlotSystemElement ele2 in gPage){
							SlotGroup sg = (SlotGroup)ele2;
							AE(sg.immediateBundle, gBun);
							foreach(Slottable sb in sg){
								if(sb!= null)
								AE(sb.immediateBundle, gBun);
							}
						}
					}else if(ele is SlotGroup){
						SlotGroup sg = (SlotGroup)ele;
						AE(sg.immediateBundle, gBun);
						foreach(Slottable sb in sg){
							if(sb!= null)
							AE(sb.immediateBundle, gBun);
						}
					}
				}
			}
		}
		public void PrintParent(SlotSystemElement ele){
			string parentName = ele.parent == null?"null":ele.parent.eName;
			Debug.Log(Indent(ele.level) + ele.eName + "'s parent is " + parentName);	
		}
		public void AssertInitiallyFocusedBundles(){
			ssm.poolBundle.PerformInHierarchy(AssertFocusedSelfAndBelow);
			ssm.equipBundle.PerformInHierarchy(AssertFocusedSelfAndBelow);
			foreach(SlotSystemBundle bundle in ssm.otherBundles){
				if(bundle != null)
					bundle.PerformInHierarchy(AssertDefocusedSelfAndBelow);
			}
			PrintSystemHierarchyDetailed(ssm);
		}
		public void TestSlotSystemActivateDeactivate(){
			ssm.Deactivate();
				Debug.Log(Util.Bold(Util.Yamabuki("Root deactivated")));
				PrintSystemHierarchyDetailed(ssm);
			ssm.Activate();
				Debug.Log(Util.Bold(Util.Yamabuki("Root activated")));
				AssertFocused();
				PrintSystemHierarchyDetailed(ssm);
			ssm.poolBundle.Defocus();
				Debug.Log(Util.Bold(Util.Yamabuki("Pool defocused")));
				PrintSystemHierarchyDetailed(ssm);
				ssm.poolBundle.PerformInHierarchy(AssertDefocusedSelfAndBelow);
			ssm.equipBundle.Defocus();
				Debug.Log(Util.Bold(Util.Yamabuki("Equip defocused")));
				PrintSystemHierarchyDetailed(ssm);
				ssm.equipBundle.PerformInHierarchy(AssertDefocusedSelfAndBelow);
			ssm.Activate();
				Debug.Log(Util.Bold(Util.Yamabuki("root activated")));
				PrintSystemHierarchyDetailed(ssm);
				AssertFocused();
		}
		public void AssertFocusedSelfAndBelow(SlotSystemElement ele){
			if(ele is SlotGroup){
				SlotGroup sg = (SlotGroup)ele;
				ASGReset(sg);
				if(ssm.poolBundle.ContainsInHierarchy(sg)){//	if sgp
					if(sg == ssm.focusedSGP){
						ASSG(sg,
							null, SGFocused, null,
							null, SGWFA, null, false);
						foreach(Slottable sb in sg){
							if(sb != null){
								AE(sb.ssm, ssm);
								ASBReset(sb);
								if(!sg.isAutoSort){
										ASSB_s(sb, SBFocused, SBWFA);
								}else{
									if(sb.item is PartsInstanceMock && !(sg.Filter is SGPartsFilter))
										ASSB_s(sb, SBDefocused, SBWFA);
									else
										if(sb.isEquipped)
											ASSB_s(sb, SBDefocused, SBWFA);
										else{
											ASSB_s(sb, SBFocused, SBWFA);
										}
								}
							}
						}
					}else{/*  sg not focused	*/
						ASSG(sg,
							null, SGDefocused, null,
							null, SGWFA, null, false);
						foreach(Slottable sb in sg){
							if(sb != null){
								AE(sb.ssm, ssm);
								ASBReset(sb);
								ASSB_s(sb, SBDefocused, SBWFA);
							}
						}
					}
				}else if(ssm.equipBundle.ContainsInHierarchy(sg)){//	if sge
					if(ssm.focusedSGEs.Contains(sg)){
						ASSG(sg,
							null, SGFocused, null,
							null, SGWFA, null, false);
						foreach(Slottable sbe in sg){
							if(sbe != null){
								AE(sbe.ssm, ssm);
								ASBReset(sbe);
								ASSB_s(sbe, SBFocused, SBWFA);
							}
						}
					}else{
						ASSG(sg,
							null, SGFocused, null,
							null, SGWFA, null, false);
						foreach(Slottable sbe in sg){
							if(sbe != null){
								AE(sbe.ssm, ssm);
								ASBReset(sbe);
								ASSB_s(sbe, SBDefocused, SBWFA);
							}
						}
					}
				}
			}else if(ele is Slottable){
				Slottable sb = (Slottable)ele;
				SlotGroup sg = sb.sg;
				ASBReset(sb);
				if(ssm.poolBundle.ContainsInHierarchy(sg)){
					if(sg == ssm.focusedSGP){
						if(!sg.isAutoSort){
							ASSB_s(sb, SBFocused, SBWFA);
						}else{
							if(sb.item is PartsInstanceMock && !(sg.Filter is SGPartsFilter))
								ASSB_s(sb, SBDefocused, SBWFA);
							else
								if(sb.isEquipped)
									ASSB_s(sb, SBDefocused, SBWFA);
								else{
									ASSB_s(sb, SBFocused, SBWFA);
								}
						}
					}else{
						ASSB_s(sb, SBDefocused, SBWFA);
					}
				}else if(ssm.equipBundle.ContainsInHierarchy(sg)){
					if(ssm.focusedSGEs.Contains(sg)){
						ASSB_s(sb, SBFocused, SBWFA);
					}else{
						ASSB_s(sb, SBDefocused, SBWFA);
					}
				}
			}
		}
		public void AssertDefocusedSelfAndBelow(SlotSystemElement ele){
			ele.PerformInHierarchy(AssertDefocusedDown);
			}
			public void AssertDefocusedDown(SlotSystemElement element){
				AB(element.isDefocused, true);
			}
		public void AssertSlotSystemSSMSetRight(SlotSystemManager ssm){
			AE(ssm.ssm, ssm);
			AE(ssm.poolBundle.ssm, ssm);
				foreach(SlotSystemElement ele in ssm.poolBundle){
					SlotGroup sg = (SlotGroup)ele;
					AE(sg.ssm, ssm);
					foreach(Slottable sb in sg){
						if(sb != null)
							AE(sb.ssm, ssm);
					}
				}
			AE(ssm.equipBundle.ssm, ssm);
				foreach(SlotSystemElement ele in ssm.equipBundle){
					EquipmentSet eSet = (EquipmentSet)ele;
					AE(eSet.ssm, ssm);
					foreach(SlotSystemElement ele2 in eSet){
						SlotGroup sg = (SlotGroup)ele2;
						AE(sg.ssm, ssm);
						foreach(Slottable sb in sg){
							if(sb != null)
								AE(sb.ssm, ssm);
						}
					}
				}
			foreach(SlotSystemBundle gBundle in ssm.otherBundles){
				AE(gBundle.ssm, ssm);
				foreach(var ele in gBundle){
					if(ele is GenericPage){
						GenericPage gPage = (GenericPage)ele;
						AE(ele.ssm, ssm);
						foreach(var e in gPage){
							SlotGroup sg = (SlotGroup)e;
							AE(sg.ssm, ssm);
						}
					}else if(ele is SlotGroup){
							SlotGroup sg = (SlotGroup)ele;
							AE(sg.ssm, ssm);
					}
				}
			}		
		}
		public string Indent(int level){
			string res = "";
			for(int i = 0; i < level; i++){
				res += "	" ;
			}
			return res;
		}
		public void PrintElementSimple(SlotSystemElement ele){
			string eleName = ele.eName;
			Debug.Log(Indent(ele.level) + eleName);
		}
		public void PrintElementInDetail(SlotSystemElement ele){
			string eleName = ele.eName;
			if(ele is SlotGroup)
				eleName = Util.SGDebug((SlotGroup)ele);
			else if(ele is Slottable)
				eleName = Util.SBDebug((Slottable)ele);
			else if(ele is SlotSystemManager)
				eleName = Util.SSMDebug((SlotSystemManager)ele);
			else
				eleName = Util.SSEDebug(ele);
			Debug.Log(Indent(ele.level) + eleName);
		}
		public void PrintSystemHierarchyDetailed(SlotSystemElement ele){
			ele.PerformInHierarchy(PrintElementInDetail);
		}
		public void PrintSystemHierarchySimple(SlotSystemElement ele){
			ele.PerformInHierarchy(PrintElementSimple);
		}
	/*	SGs testing	*/
		public void TestSGECorrespondence(){
			ssm.PointFocus(sgpAll);
				sgpAll.ToggleAutoSort(true);
					sgeBow.ToggleAutoSort(true);
					sgeWear.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpAll true, sge true"));
					TestEquippingFromTo(sgpAll, sgeBow);
					TestEquippingFromTo(sgpAll, sgeWear);
					sgeBow.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpAll true, sgBow false"));
					TestEquippingFromTo(sgpAll, sgeBow);
					sgeWear.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpAll true, sgWear false"));
					TestEquippingFromTo(sgpAll, sgeWear);
				sgpAll.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpAll false, sges false"));
					TestEquippingFromTo(sgpAll, sgeBow);
					TestEquippingFromTo(sgpAll, sgeWear);
					sgeBow.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpAll false, sgBow true"));
					TestEquippingFromTo(sgpAll, sgeBow);
					sgeWear.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpAll false, sgWear true"));
					TestEquippingFromTo(sgpAll, sgeWear);
			ssm.PointFocus(sgpBow);
				sgpBow.ToggleAutoSort(true);
				sgeBow.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpBow true, sgBow true"));
					TestEquippingFromTo(sgpBow, sgeBow);
				sgeBow.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpBow true, sgBow false"));
					TestEquippingFromTo(sgpBow, sgeBow);
				sgpBow.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpBow false, sgBow false"));
					TestEquippingFromTo(sgpBow, sgeBow);
				sgeBow.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpBow false, sgBow true"));
					TestEquippingFromTo(sgpBow, sgeBow);
			ssm.PointFocus(sgpWear);
				sgpWear.ToggleAutoSort(true);
				sgeWear.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpWear true, sgWear true"));
					TestEquippingFromTo(sgpWear, sgeWear);
				sgeWear.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpWear true, sgWear false"));
					TestEquippingFromTo(sgpWear, sgeWear);
				sgpWear.ToggleAutoSort(false);
					Debug.Log(Util.Bold("sgpWear false, sgWear false"));
					TestEquippingFromTo(sgpWear, sgeWear);
				sgeWear.ToggleAutoSort(true);
					Debug.Log(Util.Bold("sgpWear false, sgWear true"));
					TestEquippingFromTo(sgpWear, sgeWear);
			}public void TestEquippingFromTo(SlotGroup sgp, SlotGroup sge){
				foreach(Slottable sb in transactableSBs(sgp, sge, typeof(SwapTransaction))){
					// Print(sb);
					InventoryItemInstanceMock testItem = sb.itemInst;
					InventoryItemInstanceMock swapItem = ssm.GetTransaction(sb, sge).targetSB.itemInst;
					AssertFocused();
					Swap(sb, sgp, sge);
					Print((Slottable)sge[0]);
					PrintItemsArray(equipInv);
					AssertFocused();
					/*	reverse	*/
					Swap(sge.GetSB(testItem), sge, sgp.GetSB(swapItem));
					AssertFocused();
				}
				foreach(Slottable sbe in sge){
					if(sbe != null){
						foreach(Slottable sbp in transactableSBs(sgp, sbe, typeof(SwapTransaction))){
							InventoryItemInstanceMock testItem = sbp.itemInst;
							InventoryItemInstanceMock swapItem = sbe.itemInst;
							Slottable testSbe = sge.GetSB(swapItem);
								AssertFocused();
							Swap(sbp, sgp, testSbe);
								Print((Slottable)sge[0]);
								PrintItemsArray(equipInv);
							/*	reverse	*/
							Swap(sge.GetSB(testItem), sge, sgp.GetSB(swapItem));
								AssertFocused();
						}
					}
				}
			}
		public void TestSGCGearsCorrespondence(){
				ssm.PointFocus(sgpAll);
				sgpAll.ToggleAutoSort(true);
				sgeCGears.ToggleAutoSort(true);
			TestEquippingSGCGearsFrom(sgpAll);
				sgeCGears.ToggleAutoSort(false);
			TestEquippingSGCGearsFrom(sgpAll);
				sgpAll.ToggleAutoSort(false);
			TestEquippingSGCGearsFrom(sgpAll);
				sgeCGears.ToggleAutoSort(true);
			TestEquippingSGCGearsFrom(sgpAll);
				
				ssm.PointFocus(sgpCGears);
				sgpCGears.ToggleAutoSort(true);
				sgeCGears.ToggleAutoSort(true);
			TestEquippingSGCGearsFrom(sgpCGears);
				sgeCGears.ToggleAutoSort(false);
			TestEquippingSGCGearsFrom(sgpCGears);
				sgpCGears.ToggleAutoSort(false);
			TestEquippingSGCGearsFrom(sgpCGears);
				sgeCGears.ToggleAutoSort(true);
			TestEquippingSGCGearsFrom(sgpCGears);
			}public void TestEquippingSGCGearsFrom(SlotGroup sgp){
				ClearSGCGearsTo(sgp);
				/*	increasing	*/
					for(int newSlotCount = 1; newSlotCount < 5; newSlotCount++){
						ssm.ChangeEquippableCGearsCount(newSlotCount, sgeCGears);
						AssertSGCounts(sgeCGears, newSlotCount, 0, 0);
						AssertFocused();
						foreach(IEnumerable<Slottable> sbsCombo in possibleSBsCombos(newSlotCount, transactableSBs(sgp, sgeCGears, typeof(FillTransaction)))){
							foreach(Slottable sb in sbsCombo){
								Fill(sb, sgp, sgeCGears);
							}
							AE(transactableSBs(sgp, sgeCGears, typeof(FillTransaction)).Count, 0);
							int count = newSlotCount -1;
							/*	reducing the slots count while there's still some sbs left, down to 1	*/
								while(count > 0){
									ssm.ChangeEquippableCGearsCount(count, sgeCGears);
									count --;
								}
								AE(sgeCGears.slots.Count, 1);
								ssm.ChangeEquippableCGearsCount(newSlotCount, sgeCGears);
								ClearSGCGearsTo(sgp);
							/*	refilling	*/
								foreach(Slottable sb in sbsCombo){
									Fill(sb, sgp, sgeCGears);
								}
							/*	empty the slots	*/
								foreach(Slottable sb in sgeCGears){
									if(sb != null)
										Fill(sb, sgeCGears, sgp);
								}
								AE(sgeCGears.actualSBsCount, 0);
						}
					}
				/*	decreasing	*/
					for(int newSlotCount = 4; newSlotCount > 0; newSlotCount--){
						ssm.ChangeEquippableCGearsCount(newSlotCount, sgeCGears);
						AssertSGCounts(sgeCGears, newSlotCount, 0, 0);
						AssertFocused();
						foreach(IEnumerable<Slottable> sbsCombo in possibleSBsCombos(newSlotCount, transactableSBs(sgp, sgeCGears, typeof(FillTransaction)))){
							foreach(Slottable sb in sbsCombo){
								Fill(sb, sgp, sgeCGears);
							}
							AE(transactableSBs(sgp, sgeCGears, typeof(FillTransaction)).Count, 0);
							foreach(Slottable sb in sgeCGears){
								if(sb != null)
									Fill(sb, sgeCGears, sgp);
							}
							AE(sgeCGears.actualSBsCount, 0);
						}
					}
			}
	/*	test support fields	*/
		List<Slottable> allSBsList{
			get{
				List<Slottable> sbs = new List<Slottable>();
				sbs.AddRange(sbpList);
				sbs.AddRange(sbp2List);
				sbs.AddRange(sbeList);
				return sbs;
			}
		}
		List<Slottable> sbpList{
			get{
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_p);
				sbs.Add(defBowB_p);
				sbs.Add(crfBowA_p);
				sbs.Add(defWearA_p);
				sbs.Add(defWearB_p);
				sbs.Add(crfWearA_p);
				sbs.Add(defShieldA_p);
				sbs.Add(crfShieldA_p);
				sbs.Add(defMWeaponA_p);
				sbs.Add(crfMWeaponA_p);
				sbs.Add(defParts_p);
				sbs.Add(crfParts_p);
				sbs.Add(defQuiverA_p);
				sbs.Add(defPackA_p);
				return sbs;
			}
		}
		List<Slottable> sbp2List{
			get{
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_p2);
				sbs.Add(defBowB_p2);
				sbs.Add(crfBowA_p2);
				sbs.Add(defWearA_p2);
				sbs.Add(defWearB_p2);
				sbs.Add(crfWearA_p2);
				sbs.Add(defShieldA_p2);
				sbs.Add(crfShieldA_p2);
				sbs.Add(defMWeaponA_p2);
				sbs.Add(crfMWeaponA_p2);
				sbs.Add(defQuiverA_p2);
				sbs.Add(defPackA_p2);
				sbs.Add(defParts_p2);
				sbs.Add(crfParts_p2);
				return sbs;
			}
		}
		List<Slottable> sbeList{
			get{
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_e);
				sbs.Add(defWearA_e);
				sbs.Add(defShieldA_e);
				sbs.Add(defMWeaponA_e);
				return sbs;
			}
		}
		public List<Slottable> testSBs{
			get{
				List<Slottable> res = new List<Slottable>();
					res.Add(defBowA_p);
					res.Add(crfBowA_p);
					res.Add(defBowB_p);
					res.Add(defWearA_p);
					res.Add(crfWearA_p);
					res.Add(defWearB_p);
					res.Add(defShieldA_p);
					res.Add(crfShieldA_p);
					res.Add(defMWeaponA_p);
					res.Add(crfMWeaponA_p);
					res.Add(defQuiverA_p);
					res.Add(defPackA_p);
					res.Add(defParts_p);
					res.Add(crfParts_p);
				return res;
			}
		}
		SlotGroup origSGCache;
	/*	setup	*/
		public void AssertInitialize(){
			ASSSM(ssm,
				null, null, null, null, null, null, null,
				SSMDeactivated, SSMDeactivated, null,
				SSMWFA, SSMWFA, null,
				null, true, true, true, true);
			foreach(SlotGroup sg in ssm.allSGs){
				ASSG(sg,
					SGDeactivated, SGDeactivated, null,
					SGWFA, SGWFA, null, false);
					ASGReset(sg);
				foreach(Slottable sb in sg){
					if(sb != null){
						ASSB(sb,
							SBDeactivated, SBDeactivated, null,
							SBWFA, SBWFA, null, false,
							null, null, null,
							SBUnmarked, SBUnmarked, null);
						ANull(sb.curEqpState);
						ANull(sb.prevEqpState);
					}
				}
			}
		}
		public void AssertBundlesPagesAndSGsMembership(){
			ANotNull(ssm);
			AE(ssm.allSGs.Count, 19);
				AB(ssm.allSGs.Contains(sgpAll), true);
				AB(ssm.allSGs.Contains(sgpBow), true);
				AB(ssm.allSGs.Contains(sgpWear), true);
				AB(ssm.allSGs.Contains(sgpCGears), true);
				AB(ssm.allSGs.Contains(sgpParts), true);
				AB(ssm.allSGs.Contains(sgeBow), true);
				AB(ssm.allSGs.Contains(sgeWear), true);
				AB(ssm.allSGs.Contains(sgeCGears), true);
				AB(ssm.allSGs.Contains(sgg_12), true);
				AB(ssm.allSGs.Contains(sgg_111), true);
				AB(ssm.allSGs.Contains(sgg_112), true);
				AB(ssm.allSGs.Contains(sgg_21), true);
				AB(ssm.allSGs.Contains(sgg_22), true);
				AB(ssm.allSGs.Contains(sgg_23), true);
				AB(ssm.allSGs.Contains(sgg_24), true);
				AB(ssm.allSGs.Contains(sgg_2511), true);
				AB(ssm.allSGs.Contains(sgg_2512), true);
				AB(ssm.allSGs.Contains(sgg_2521), true);
				AB(ssm.allSGs.Contains(sgg_2522), true);
			AE(ssm.focusedSGs.Count, 5);
				AB(ssm.focusedSGs.Contains(sgpAll), true);
				AB(ssm.focusedSGs.Contains(sgeBow), true);
				AB(ssm.focusedSGs.Contains(sgeWear), true);
				AB(ssm.focusedSGs.Contains(sgeCGears), true);
				AB(ssm.focusedSGs.Contains(sgg_111), true);
				// AB(ssm.focusedSGs.Contains(sgg_112), true);
				// AB(ssm.focusedSGs.Contains(sgg_21), true);
			/*	pool	*/
			ANotNull(ssm.poolBundle);
			AE(ssm.allSGPs.Count, 5);
				AB(ssm.allSGPs.Contains(sgpAll), true);
				AB(ssm.allSGPs.Contains(sgpBow), true);
				AB(ssm.allSGPs.Contains(sgpWear), true);
				AB(ssm.allSGPs.Contains(sgpCGears), true);
				AB(ssm.allSGPs.Contains(sgpParts), true);
				AE(sgpAll.parent, ssm.poolBundle);
				AE(sgpBow.parent, ssm.poolBundle);
				AE(sgpWear.parent, ssm.poolBundle);
				AE(sgpCGears.parent, ssm.poolBundle);
				AE(sgpParts.parent, ssm.poolBundle);
			AB(sgpAll.isPool, true);
			AB(sgpBow.isPool, true);
			AB(sgpWear.isPool, true);
			AB(sgpCGears.isPool, true);
			AB(sgpParts.isPool, true);
			AB(sgpAll.isSGE, false);
			AB(sgpBow.isSGE, false);
			AB(sgpWear.isSGE, false);
			AB(sgpCGears.isSGE, false);
			AB(sgpParts.isSGE, false);
			AB(sgpAll.isSGG, false);
			AB(sgpBow.isSGG, false);
			AB(sgpWear.isSGG, false);
			AB(sgpCGears.isSGG, false);
			AB(sgpParts.isSGG, false);
			AE(ssm.focusedSGP, sgpAll);
			/*	equip	*/
			ANotNull(ssm.equipBundle);
			AE(ssm.allSGEs.Count, 3);
			AE(ssm.focusedSGEs.Count, 3);
				AB(ssm.focusedSGEs.Contains(sgeBow), true);
				AB(ssm.focusedSGEs.Contains(sgeWear), true);
				AB(ssm.focusedSGEs.Contains(sgeCGears), true);
				AE(ssm.focusedEqSet, ssm.equipBundle.focusedElement);
				AE(sgeBow.parent, ssm.focusedEqSet);
				AE(sgeWear.parent, ssm.focusedEqSet);
				AE(sgeCGears.parent, ssm.focusedEqSet);
			AB(sgeBow.isPool, false);
			AB(sgeWear.isPool, false);
			AB(sgeCGears.isPool, false);
			AB(sgeBow.isSGE, true);
			AB(sgeWear.isSGE, true);
			AB(sgeCGears.isSGE, true);
			AB(sgeBow.isSGG, false);
			AB(sgeWear.isSGG, false);
			AB(sgeCGears.isSGG, false);
			/*	generic	*/
				ANotNull(ssm.otherBundles);
				AE(ssm.allSGGs.Count, 11);
					AB(ssm.allSGGs.Contains(sgg_111), true);
					AB(ssm.allSGGs.Contains(sgg_112), true);
					AB(ssm.allSGGs.Contains(sgg_12), true);
					AB(ssm.allSGGs.Contains(sgg_21), true);
					AB(ssm.allSGGs.Contains(sgg_22), true);
					AB(ssm.allSGGs.Contains(sgg_23), true);
					AB(ssm.allSGGs.Contains(sgg_24), true);
					AB(ssm.allSGGs.Contains(sgg_2511), true);
					AB(ssm.allSGGs.Contains(sgg_2512), true);
					AB(ssm.allSGGs.Contains(sgg_2521), true);
					AB(ssm.allSGGs.Contains(sgg_2522), true);
				AE(ssm.focusedSGGs.Count, 1);
					AB(ssm.focusedSGGs.Contains(sgg_111), true);
					// AB(ssm.focusedSGGs.Contains(sgg_112), true);
					// AB(ssm.focusedSGGs.Contains(sgg_21), true);
				AE(sgg_111.parent.eName, Util.Bold("gPage_11"));
				AE(sgg_112.parent.eName, Util.Bold("gPage_11"));
				AE(sgg_12.parent.eName, Util.Bold("gBundle_1"));
				AE(sgg_21.parent.eName, Util.Bold("gBundle_2"));
				AE(sgg_22.parent.eName, Util.Bold("gBundle_2"));
				AE(sgg_23.parent.eName, Util.Bold("gBundle_2"));
				AE(sgg_24.parent.eName, Util.Bold("gBundle_2"));
				AE(sgg_2511.parent.eName, Util.Bold("gPage_251"));
				AE(sgg_2512.parent.eName, Util.Bold("gPage_251"));
				AE(sgg_2521.parent.eName, Util.Bold("gPage_252"));
				AE(sgg_2522.parent.eName, Util.Bold("gPage_252"));
				AB(sgg_111.isPool, false);
				AB(sgg_112.isPool, false);
				AB(sgg_12.isPool, false);
				AB(sgg_21.isPool, false);
				AB(sgg_22.isPool, false);
				AB(sgg_23.isPool, false);
				AB(sgg_24.isPool, false);
				AB(sgg_2511.isPool, false);
				AB(sgg_2512.isPool, false);
				AB(sgg_2521.isPool, false);
				AB(sgg_2522.isPool, false);
				AB(sgg_111.isSGE, false);
				AB(sgg_112.isSGE, false);
				AB(sgg_12.isSGE, false);
				AB(sgg_21.isSGE, false);
				AB(sgg_22.isSGE, false);
				AB(sgg_23.isSGE, false);
				AB(sgg_24.isSGE, false);
				AB(sgg_2511.isSGE, false);
				AB(sgg_2512.isSGE, false);
				AB(sgg_2521.isSGE, false);
				AB(sgg_2522.isSGE, false);
				AB(sgg_111.isSGG, true);
				AB(sgg_112.isSGG, true);
				AB(sgg_12.isSGG, true);
				AB(sgg_21.isSGG, true);
				AB(sgg_22.isSGG, true);
				AB(sgg_23.isSGG, true);
				AB(sgg_24.isSGG, true);
				AB(sgg_2511.isSGG, true);
				AB(sgg_2512.isSGG, true);
				AB(sgg_2521.isSGG, true);
				AB(sgg_2522.isSGG, true);
		}
		public void AssertSBsMembership(){
			foreach(Slottable sbp in sbpList){
				AE(sbp.sg, sgpAll);
				AE(ssm.ContainsInHierarchy(sbp), true);
				AE(ssm.poolBundle.ContainsInHierarchy(sbp), true);
				AE(ssm.equipBundle.ContainsInHierarchy(sbp), false);

				AE(sgpAll.ContainsInHierarchy(sbp), true);
				AE(sgpBow.ContainsInHierarchy(sbp), false);
				AE(sgpWear.ContainsInHierarchy(sbp), false);
				AE(sgpCGears.ContainsInHierarchy(sbp), false);
				AE(sgpParts.ContainsInHierarchy(sbp), false);
			}
			foreach(Slottable sbp2 in sbp2List){
				AE(ssm.ContainsInHierarchy(sbp2), true);
				AE(ssm.poolBundle.ContainsInHierarchy(sbp2), true);
				AE(sgpAll.ContainsInHierarchy(sbp2), false);
				if(sbp2.itemInst is BowInstanceMock){
					AE(sbp2.sg, sgpBow);
					AE(sgpBow.ContainsInHierarchy(sbp2), true);
					AE(sgpWear.ContainsInHierarchy(sbp2), false);
					AE(sgpCGears.ContainsInHierarchy(sbp2), false);
					AE(sgpParts.ContainsInHierarchy(sbp2), false);
				}else if(sbp2.itemInst is WearInstanceMock){
					AE(sbp2.sg, sgpWear);
					AE(sgpBow.ContainsInHierarchy(sbp2), false);
					AE(sgpWear.ContainsInHierarchy(sbp2), true);
					AE(sgpCGears.ContainsInHierarchy(sbp2), false);
					AE(sgpParts.ContainsInHierarchy(sbp2), false);
				}else if(sbp2.itemInst is CarriedGearInstanceMock){
					AE(sbp2.sg, sgpCGears);
					AE(sgpBow.ContainsInHierarchy(sbp2), false);
					AE(sgpWear.ContainsInHierarchy(sbp2), false);
					AE(sgpCGears.ContainsInHierarchy(sbp2), true);
					AE(sgpParts.ContainsInHierarchy(sbp2), false);
				}else if(sbp2.itemInst is PartsInstanceMock){
					AE(sbp2.sg, sgpParts);
					AE(sgpBow.ContainsInHierarchy(sbp2), false);
					AE(sgpWear.ContainsInHierarchy(sbp2), false);
					AE(sgpCGears.ContainsInHierarchy(sbp2), false);
					AE(sgpParts.ContainsInHierarchy(sbp2), true);
				}
			}
			foreach(Slottable sbe in sbeList){
				AE(ssm.ContainsInHierarchy(sbe), true);
				AE(ssm.poolBundle.ContainsInHierarchy(sbe), false);
				AE(ssm.equipBundle.ContainsInHierarchy(sbe), true);
				foreach(EquipmentSet eSet in ssm.equipmentSets){
					if(eSet == ssm.focusedEqSet)
						AE(eSet.ContainsInHierarchy(sbe), true);
					else
						AE(eSet.ContainsInHierarchy(sbe), false);
				}
				if(sbe.itemInst is BowInstanceMock){
					AE(sbe.sg, sgeBow);
					AE(sgeBow.ContainsInHierarchy(sbe), true);
					AE(sgeWear.ContainsInHierarchy(sbe), false);
					AE(sgeCGears.ContainsInHierarchy(sbe), false);
				}
				if(sbe.itemInst is WearInstanceMock){
					AE(sbe.sg, sgeWear);
					AE(sgeBow.ContainsInHierarchy(sbe), false);
					AE(sgeWear.ContainsInHierarchy(sbe), true);
					AE(sgeCGears.ContainsInHierarchy(sbe), false);
				}
				if(sbe.itemInst is CarriedGearInstanceMock){
					AE(sbe.sg, sgeCGears);
					AE(sgeBow.ContainsInHierarchy(sbe), false);
					AE(sgeWear.ContainsInHierarchy(sbe), false);
					AE(sgeCGears.ContainsInHierarchy(sbe), true);
				}
			}
			
		}
	/*	Test Transaction on All	*/
		public void TestSwapOnAll(){
			PerformOnAllSBs(CrossTestSwap);
			PrintTestResult();
			}
			public void CrossTestSwap(Slottable sb, bool isPAS){
				CrossTestSG(TestSwap, sb, isPAS);
				origSGCache = null;
			}
			public void TestSwap(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				SlotGroup origSG = testSB.sg;
				if(origSG != null)
					origSGCache = origSG;
				else
					origSG = origSGCache;
				InventoryItemInstanceMock testItem = testSB.itemInst;
				testSB = origSG.GetSB(testItem);
				if(testSB.isFocused){
					SlotSystemTransaction ta = ssm.GetTransaction(testSB, tarSG);
					if(ta is SwapTransaction){
						Capture(ssm, testSB, tarSG, isPAS, isTAS, TestElement.SG);
						InventoryItemInstanceMock swapItem = ta.targetSB.itemInst;
						Swap(testSB, origSG, tarSG);
						/*	reverse	*/
						Swap(origSG.GetSB(swapItem), origSG, tarSG);
					}
					foreach(Slottable tarSB in tarSG){
						if(tarSB != null){
							testSB = origSG.GetSB(testItem);
							SlotSystemTransaction ta2 = ssm.GetTransaction(testSB, tarSB);
							if(ta2 is SwapTransaction){
								Capture(ssm, testSB, tarSB, isPAS, isTAS, TestElement.SB);
								InventoryItemInstanceMock tarItem = tarSB.itemInst;
								Swap(testSB, origSG, tarSB);
								/*	reverse	*/
								Swap(tarSG.GetSB(testItem), tarSG, origSG.GetSB(tarItem));
							}
						}
					}
				}
			}
		public void TestSwapSelectively(SlotGroup origSG, SlotGroup tarSG){
			PerformOnAllSBsSelectively(CrossTestSwapSelectively, origSG, tarSG);
			PrintTestResult();
			}
			public void CrossTestSwapSelectively(Slottable sb, bool isPAS, SlotGroup tarSG){
				CrossTestSGSelectively(TestSwap, sb, isPAS, tarSG);
			}

		public void TestFillOnAll(){
			PerformOnAllSBs(CrossTestFill);
			PrintTestResult();
			}public void CrossTestFill(Slottable sb, bool isPAS){
				CrossTestSG(TestFill, sb, isPAS);
				origSGCache = null;
			}
			public void TestFill(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				InventoryItemInstanceMock testItem = testSB.itemInst;
				SlotGroup origSG = testSB.sg;
				if(origSG != null)
					origSGCache = origSG;
				else
					origSG = origSGCache;
				testSB = origSG.GetSB(testItem);
				if(testSB.isFocused){
					if(ssm.GetTransaction(testSB, tarSG).GetType() == typeof(FillTransaction)){
						AssertFocused();
						Capture(ssm, testSB, tarSG, isPAS, isTAS, TestElement.SG);
						/*	on SG	*/
							Fill(testSB, origSG, tarSG);
						/*	reverse	*/
							Slottable sbInTarSG = tarSG.GetSB(testItem);
							if(ssm.GetTransaction(sbInTarSG, origSG).GetType() == typeof(FillTransaction))
								Fill(tarSG.GetSB(testItem), tarSG, origSG);
					}
					foreach(Slottable tarSB in tarSG){
						testSB = origSG.GetSB(testItem);
						if(tarSB != null){
							if(ssm.GetTransaction(testSB, tarSB).GetType() == typeof(FillTransaction)){
								Capture(ssm, testSB, tarSB, isPAS, isTAS, TestElement.SB);
								/*	OnSB	*/
									Fill(testSB, origSG, tarSB);
								/*	reverse	*/
									Slottable sbInTarSG = tarSG.GetSB(testItem);
									if(ssm.GetTransaction(sbInTarSG, origSG).GetType() == typeof(FillTransaction))
										Fill(tarSG.GetSB(testItem), tarSG, origSG);
							}
						}
					}
				}
			}
			public void TestFillSelectively(SlotGroup origSG, SlotGroup tarSG){
			PerformOnAllSBsSelectively(CrossTestFillSelectively, origSG, tarSG);
			PrintTestResult();
			}
			public void CrossTestFillSelectively(Slottable sb, bool isPAS, SlotGroup tarSG){
				CrossTestSGSelectively(TestFill, sb, isPAS, tarSG);
			}
		public void TestVolSortOnAll(){
			PerformOnAllSGAfterFocusing(TestVolSort);
			PrintTestResult();
			}
			public void TestVolSort(SlotGroup sg, bool isPAS){
				if(!sg.isAutoSort){
					ssm.SortSG(sg, SlotGroup.InverseItemIDSorter);
						if(sg.actProcess != null && sg.actProcess.isRunning){
							ASSSM(ssm,
								null, null, sg, null, null, null, null, 
								SSMDeactivated, SSMFocused, null,
								SSMWFA, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(SortTransaction), true, true, false, true);
							CompleteAllSBActProcesses(sg);
						}
							AssertFocused();
							AssertSBsSorted(sg, SlotGroup.InverseItemIDSorter);
							Capture(ssm, null, sg, isPAS, false, TestElement.SG);

					ssm.SortSG(sg, SlotGroup.AcquisitionOrderSorter);
						if(sg.actProcess != null && sg.actProcess.isRunning){
							ASSSM(ssm,
								null, null, sg, null, null, null, null, 
								SSMDeactivated, SSMFocused, null,
								SSMWFA, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(SortTransaction), true, true, false, true);
							CompleteAllSBActProcesses(sg);
						}
							AssertFocused();
							AssertSBsSorted(sg, SlotGroup.AcquisitionOrderSorter);
							Capture(ssm, null, sg, isPAS, false, TestElement.SG);
					
					ssm.SortSG(sg, SlotGroup.ItemIDSorter);
						if(sg.actProcess != null && sg.actProcess.isRunning){
								ASSSM(ssm,
									null, null, sg, null, null, null, null, 
									SSMDeactivated, SSMFocused, null,
									SSMWFA, SSMTransaction, typeof(SSMTransactionProcess),
									typeof(SortTransaction), true, true, false, true);
							CompleteAllSBActProcesses(sg);
						}
							AssertFocused();
							AssertSBsSorted(sg, SlotGroup.ItemIDSorter);
							Capture(ssm, null, sg, isPAS, false, TestElement.SG);
				}
			}
		public void TestReorderOnAll(){
			PerformOnAllSBs(CrossTestReorder);
			PrintTestResult();
			}
			public void CrossTestReorder(Slottable testSB, bool isPAS){
				CrossTestSG(TestReorder, testSB ,isPAS);
			}
			public void TestReorder(SlotGroup targetSG, Slottable testSB, bool isPAS, bool isTAS){
				if(testSB.isFocused){
					SlotGroup origSG = testSB.sg;
					int initID = origSG.toList.IndexOf(testSB);
					foreach(Slottable targetSB in targetSG){
						if(ssm.GetTransaction(testSB, targetSB).GetType() == typeof(ReorderTransaction)){
								Capture(ssm, testSB, targetSB, isPAS, isTAS, TestElement.SB);
								Reorder(testSB, targetSB);
							/*	reverse	*/
								Reorder(testSB, (Slottable)origSG[initID]);
						}
					}
				}
			}
		public void TestRevertOnAll(){
			PerformOnAllSBs(CrossTestRevert);
			PrintTestResult();
			}
			public void CrossTestRevert(Slottable pickedSB, bool isPAS){
				CrossTestSG(TestRevert, pickedSB, isPAS);
			}
			public void TestRevert(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				if(testSB.isFocused){
					if(ssm.GetTransaction(testSB, tarSG).GetType() == typeof(RevertTransaction)){
						Capture(ssm, testSB, tarSG, isPAS, isTAS, TestElement.SG);
						Revert(testSB, tarSG);
					}
					foreach(Slottable tarSB in tarSG){
						if(tarSB != null){
							if(ssm.GetTransaction(testSB, tarSB).GetType() == typeof(RevertTransaction)){
								Capture(ssm, testSB, tarSB, isPAS, isTAS, TestElement.SB);
								Revert(testSB, tarSB);
							}
						}
					}
				}
			}
	/*	Transaction test preliminary	*/
		public void AssertEquippedOnAll(){
			AE(ssm.equippedBowInst, sgeBow.slots[0].sb.itemInst);
			AE(ssm.equippedWearInst, sgeWear.slots[0].sb.itemInst);
			AssertEquipped(ssm.equippedBowInst);
			AssertEquipped(ssm.equippedWearInst);
			AECGears(ssm.equippedCarriedGears, ssm.poolInv, ssm.equipInv);
		}
		public void TestReorderSBsMethod(){
			List<Slottable> sbs = testSBs;
			PrintSBsArray(sbs);
			sbs.Shuffle();
			PrintSBsArray(sbs);
			sbs.Reorder(sbs[0], sbs[3]);
			PrintSBsArray(sbs);
			sbs.Reorder(sbs[sbs.Count - 1], sbs[0]);
			PrintSBsArray(sbs);
		}
		public void TestDraggedIconOnAll(){
			PerformOnAllSBs(TestDraggedIcon);
			}
			public void TestDraggedIcon(Slottable sb, bool isPAS){
				DraggedIcon di = new DraggedIcon(sb);
				ssm.SetDIcon1(di);
				ssm.SetTransaction(new EmptyTransaction());
				ssm.transaction.Execute();
				ASSSM(ssm,
					null, null, null, null, sb, null, null, 
					SSMDeactivated, SSMFocused, null,
					SSMWFA, SSMTransaction, typeof(SSMTransactionProcess),
					typeof(EmptyTransaction), false, true, true, true);
				di.CompleteMovement();
				ASSSM(ssm,
					null, null, null, null, null, null, null, 
					SSMDeactivated, SSMFocused, null,
					SSMTransaction, SSMWFA, null,
					null, true, true, true, true);
			}
		public void TestAcceptSGTACompOnAll(){
			PerformOnAllSGAfterFocusing(TestAcceptSGTAComp);
			}
			public void TestAcceptSGTAComp(SlotGroup sg, bool isPAS){
				AssertFocused();
				ssm.SetSG1(sg);
				ASSSM(ssm,
					null, null, sg, null, null, null, null,
					SSMDeactivated, SSMFocused, null,
					null, SSMWFA, null,
					null, true, true, false, true);
				ssm.SetTransaction(new EmptyTransaction());
				ssm.transaction.Execute();
				ASSSM(ssm,
					null, null, sg, null, null, null, null,
					SSMDeactivated, SSMFocused, null,
					SSMWFA, SSMTransaction, typeof(SSMTransactionProcess),
					typeof(EmptyTransaction), true, true, false, true);
				ssm.AcceptSGTAComp(sg);
				ASSSM(ssm,
					null, null, null, null, null, null, null,
					SSMDeactivated, SSMFocused, null,
					SSMTransaction, SSMWFA, null,
					null, true, true, true, true);
			}
		public void TestSSMStateTransition(){
			/*	Selecttion state */
					ASSSM(ssm,
					null, null, null, null, null, null, null, 
					SSMDeactivated, SSMFocused, null,
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
				ssm.SetSelState(SlotSystemManager.ssmDefocusedState);
					ASSSM(ssm,
					null, null, null, null, null, null, null, 
					SSMFocused, SSMDefocused, typeof(SSMGreyoutProcess),
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
				ssm.SetSelState(SlotSystemManager.ssmFocusedState);
					ASSSM(ssm,
					null, null, null, null, null, null, null, 
					SSMDefocused, SSMFocused, typeof(SSMGreyinProcess),
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
				ssm.SetSelState(SlotSystemManager.ssmDeactivatedState);
					ASSSM(ssm,
					null, null, null, null, null, null, null, 
					SSMFocused, SSMDeactivated, null,
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
				ssm.SetSelState(SlotSystemManager.ssmDefocusedState);
					ASSSM(ssm,
					null, null, null, null, null, null, null, 
					SSMDeactivated, SSMDefocused, null,
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
				ssm.SetSelState(SlotSystemManager.ssmDeactivatedState);
					ASSSM(ssm,
					null, null, null, null, null, null, null, 
					SSMDefocused, SSMDeactivated, null,
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
				ssm.SetSelState(SlotSystemManager.ssmFocusedState);
					ASSSM(ssm,
					null, null, null, null, null, null, null, 
					SSMDeactivated, SSMFocused, null,
					SSMWFA, SSMWFA, null,
					null, true, true, true, true);
			/*	Action state */	// maybe after transaction is done 
				// 	ASSSM(sgm,
				// 	SSMDeactivated, SSMFocused, null,
				// 	SSMWFA, SSMWFA, null,
				// 	null, null,
				// 	true, true, true, true);
				// sgm.SetActState(SlotGroupManager.ProbingState)
		}
		public void TestSBStateTransitionOnAll(){
			PerformOnAllSBs(TestSBStateTransition);
			PrintTestResult();
		}
			public void TestSBStateTransition(Slottable sb, bool isPAS){
				/*	SelState */
					if(isPAS){
						if(sb.sg.isPool){
							if(sb.isEquipped || (sb.itemInst is PartsInstanceMock && !(sb.sg.Filter is SGPartsFilter))){
								if(sb.prevSelState == SBDeactivated){
									ASBSelState(sb, SBDeactivated, SBDefocused, null);

								}else
									ASBSelState(sb, SBFocused, SBDefocused, typeof(SBGreyoutProcess));

								sb.SetSelState(SBSelected);
									ASBSelState(sb, SBDefocused, SBSelected, typeof(SBHighlightProcess));
								sb.SetSelState(SBDefocused);
									ASBSelState(sb, SBSelected, SBDefocused, typeof(SBGreyoutProcess));
								sb.SetSelState(SBFocused);
									ASBSelState(sb, SBDefocused, SBFocused, typeof(SBGreyinProcess));
								return;
							}
						}
					}
					if(sb.prevSelState == SBDeactivated)
						ASBSelState(sb, SBDeactivated, SBFocused, null);
					else
						ASBSelState(sb, SBDefocused, SBFocused, typeof(SBGreyinProcess));
					sb.SetSelState(SBSelected);
						ASBSelState(sb, SBFocused, SBSelected, typeof(SBHighlightProcess));
					sb.SetSelState(SBFocused);
						ASBSelState(sb, SBSelected, SBFocused, typeof(SBDehighlightProcess));
					sb.SetSelState(SBDeactivated);
						ASBSelState(sb, SBFocused, SBDeactivated, null);
					sb.SetSelState(SBFocused);
						ASBSelState(sb, SBDeactivated, SBFocused, null);
					sb.SetSelState(SBDeactivated);
						ASBSelState(sb, SBFocused, SBDeactivated, null);
					sb.SetSelState(SBDefocused);
						ASBSelState(sb, SBDeactivated, SBDefocused, null);
					sb.SetSelState(SBDeactivated);
						ASBSelState(sb, SBDefocused, SBDeactivated, null);
					sb.SetSelState(SBSelected);
						ASBSelState(sb, SBDeactivated, SBSelected, null);
					sb.SetSelState(SBDeactivated);
						ASBSelState(sb, SBSelected, SBDeactivated, null);
				/*	Equip State	*/
				if(sb.sg.isPool){
					if(sb.isEquipped){
							ASBEqpState(sb, SBEquipped, SBEquipped, null);
						sb.SetEqpState(SBUnequipped);
							ASBEqpState(sb, SBEquipped, SBUnequipped, typeof(SBUnequipProcess));
						sb.SetEqpState(SBEquipped);
							ASBEqpState(sb, SBUnequipped, SBEquipped, typeof(SBEquipProcess));
					}
					else{
						if(sb.prevEqpState == SBUnequipped)
							ASBEqpState(sb, SBUnequipped, SBUnequipped, null);
						else
							ASBEqpState(sb, SBEquipped, SBUnequipped, typeof(SBUnequipProcess));

						sb.SetEqpState(SBEquipped);
							ASBEqpState(sb, SBUnequipped, SBEquipped, typeof(SBEquipProcess));
						sb.SetEqpState(SBUnequipped);
							ASBEqpState(sb, SBEquipped, SBUnequipped, typeof(SBUnequipProcess));
					}
				}else{
					sb.SetEqpState(SBUnequipped);
						ASBEqpState(sb, SBEquipped, SBUnequipped, null);
					sb.SetEqpState(SBEquipped);
						ASBEqpState(sb, SBUnequipped, SBEquipped, null);
				}
			}
		public void TestPickUpTransitionOnAll(){
			PerformOnAllSBs(TestPickUpTransition);
		}
			public void TestPickUpTransition(Slottable sb, bool isPAS){
					AssertFocused();
					SlotGroup origSG = sb.sg;
				if(sb.isFocused){
					/*	tap	*/
						sb.OnPointerDownMock(eventData);
							ASSSM(ssm,
								null, null, null, null, null, null, null,
								SSMDeactivated, SSMFocused, null,
								null, SSMWFA, null,
								null, true, true, true, true);
							ASSG(origSG,
								null, SGFocused, null,
								null, SGWFA, null, false);
							ASSB(sb,
								SBFocused, SBSelected, typeof(SBHighlightProcess),
								SBWFA, SBWFPickUp, typeof(WaitForPickUpProcess), true,
								null, null, null,
								null, null, null);
						sb.OnPointerUpMock(eventData);
							ASSB(sb,
								SBFocused, SBSelected, typeof(SBHighlightProcess),
								SBWFPickUp, SBWFNT, typeof(WaitForNextTouchProcess), true,
								null, null, null,
								null, null, null);
						sb.actProcess.Expire();
							ASSB(sb,
								SBSelected, SBFocused, typeof(SBDehighlightProcess),
								SBWFNT, SBWFA, null, false,
								null, null, null,
								null, null, null);
						AssertFocused();
					/*	multi tap -> pickup	*/
						sb.OnPointerDownMock(eventData);
						sb.OnPointerUpMock(eventData);
						sb.OnPointerDownMock(eventData);
							ASSSM(ssm,
								sb, null, null, null, sb, null, sb,
								SSMDeactivated, SSMFocused, null,
								SSMWFA, SSMProbing, typeof(SSMProbeProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								null, SGWFA, null, false);
							ASSB(sb,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFNT, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null,
								null, null, null);
						sb.OnPointerUpMock(eventData);
						if(sb.isStackable){
							ASSB(sb,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBPickedUp, SBWFNT, typeof(WaitForNextTouchProcess), true,
								null, null, null,
								null, null, null);
							sb.actProcess.Expire();
						}
							/*reverting*/
							ASSSM(ssm,
								sb, null, null, null, sb, null, sb,
								SSMDeactivated, SSMFocused, null,
								SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								SGWFA, SGRevert, typeof(SGTransactionProcess), false);
							ASSB(sb,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								sb.isStackable?SBWFNT:SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), false,
								null, null, null,
								null, null, null);
						ssm.dIcon1.CompleteMovement();
						AssertFocused();
					/*	pickup -> release -> expire to revert	*/
						PickUp(sb, out picked);
						LetGo();
							ASSB(sb,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								sb.isStackable?SBWFNT:SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), false,
								null, null, null,
								null, null, null);
						ssm.dIcon1.CompleteMovement();
						AssertFocused();
					/* pickup -> release -> touch	*/
						if(sb.isStackable){
							PickUp(sb, out picked);
							sb.OnPointerUpMock(eventData);
							sb.OnPointerDownMock(eventData);
							LetGo();
							ssm.dIcon1.CompleteMovement();
							AssertFocused();
						}
				}else{
					sb.OnPointerDownMock(eventData);
						ASSSM(ssm,
							null, null, null, null, null, null, null,
							SSMDeactivated, SSMFocused, null,
							null, SSMWFA, null,
							null, true, true, true, true);
						ASSB(sb,
							null, SBDefocused, null,
							SBWFA, SBWFPointerUp, typeof(WaitForPointerUpProcess), true,
							null,null,null,
							null, null, null);
					sb.OnPointerUpMock(eventData);
					AssertFocused();
				}
			}
		
		public void CheckTransacitonWithSBSpecifiedOnAll(){
			PerformOnAllSBs(CheckTransactionWithSB);
			PrintTestResult();
			}public void CheckTransactionWithSB(Slottable sb, bool isPickedAS){
				CrossTestSG(CrossCheckTransactionWithSB, sb, isPickedAS);
			}
			public void CrossCheckTransactionWithSB(SlotGroup tarSG, Slottable pickedSB, bool isPAS, bool isTAS){
					foreach(Slottable tarSB in tarSG){
						if(tarSB != null){
							Capture(tarSB.ssm, pickedSB, tarSB, isPAS, isTAS, TestElement.TA);
						}
					}
			}
		public void CheckShrinkableAndExpandableOnAllSGs(){
			foreach(SlotGroup sg in ssm.allSGs){
				string shrinkStr = " Shrinkable: " + sg.isShrinkable;
				if(sg.isShrinkable)	shrinkStr = Blue(shrinkStr);
				else shrinkStr = Red(shrinkStr);
				string expandStr = " Expandable: " + sg.isExpandable;
				if(sg.isExpandable)	expandStr = Blue(expandStr);
				else expandStr = Red(expandStr);
				Debug.Log(sg.eName + shrinkStr + ", " + expandStr);
			}
		}
		public void CheckTransactionOnAllSG(){
			PerformOnAllSBs(CrossCheckTransaction);
			PrintTestResult();
			}public void CrossCheckTransaction(Slottable sb, bool isPickedAS){
				CrossTestSG(CheckTransaction, sb, isPickedAS);
			}
			public void CheckTransaction(SlotGroup sg, Slottable sb, bool isPAS ,bool isTAS){
				if(sb.isFocused){
					Capture(sg.ssm, sb, sg, isPAS, isTAS, TestElement.TA);
				}
			}
	/*	thorough testing utility	*/
		public List<IEnumerable<Slottable>> possibleSBsCombos(int subsetCount, List<Slottable> sbs){
			List<IEnumerable<Slottable>> result = new List<IEnumerable<Slottable>>();
			foreach(IEnumerable<Slottable> combo in ListMethods.Combinations<Slottable>(subsetCount, sbs)){
				List<IEnumerable<Slottable>> perms = ListMethods.Permutations<Slottable>(combo);
				result.AddRange(perms);
			}
			return result;
			}public void TestPermutation(){
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_p);
				sbs.Add(defWearA_p);
				sbs.Add(defShieldA_p);
				sbs.Add(defQuiverA_p);
				sbs.Add(defParts_p);
				List<IEnumerable<Slottable>> sbsPerms = ListMethods.Permutations<Slottable>(sbs);
				foreach(IEnumerable<Slottable> sbList in sbsPerms){
					PrintSBsArray(sbList);
				}
			}
			public void TestCombination(){
				List<Slottable> sbs = new List<Slottable>();
				sbs.Add(defBowA_p);
				sbs.Add(defWearA_p);
				sbs.Add(defQuiverA_p);
				sbs.Add(defMWeaponA_p);
				sbs.Add(defParts_p);
				
				List<IEnumerable<Slottable>> possibleCombos = possibleSBsCombos(4, sbs);
				foreach(IEnumerable<Slottable> combo in possibleCombos)
					PrintSBsArray(combo);
			}
		public List<Slottable> transactableSBs(SlotGroup origSG, SlotSystemElement hovered, System.Type ta){
			List<Slottable> result = new List<Slottable>();
			foreach(Slottable sb in origSG){
				if(sb != null && sb.isFocused){
					if(ssm.GetTransaction(sb, hovered).GetType() == ta){
						result.Add(sb);
					}
				}
			}
			return result;
		}
		public void PerformOnAllSBs(System.Action<Slottable, bool> act){
			foreach(SlotGroup sg in ssm.allSGs){
				ssm.PointFocus(sg);
				sg.ToggleAutoSort(true);
				foreach(Slottable sb in sg){
					if(sb != null)
						act(sb, true);
				}
				sg.ToggleAutoSort(false);
				foreach(Slottable sb in sg){
					if(sb != null)
						act(sb, false);
				}
			}
		}
		public void PerformOnAllSBsSelectively(System.Action<Slottable, bool, SlotGroup> act, SlotGroup origSG, SlotGroup tarSG){
			foreach(SlotGroup sg in ssm.allSGs){
				if(sg == origSG){
					ssm.PointFocus(sg);
					sg.ToggleAutoSort(true);
					foreach(Slottable sb in sg){
						if(sb != null)
							act(sb, true, tarSG);
					}
					sg.ToggleAutoSort(false);
					foreach(Slottable sb in sg){
						if(sb != null)
							act(sb, false, tarSG);
					}
				}
			}
		}
		public void PerformOnAllSGAfterFocusing(System.Action<SlotGroup, bool> act){
			foreach(SlotGroup sg in ssm.allSGs){
				ssm.PointFocus(sg);
				sg.ToggleAutoSort(true);
				act(sg, true);
				sg.ToggleAutoSort(false);
				act(sg, false);
			}
		}
		public void CrossTestSG(System.Action<SlotGroup, Slottable, bool, bool> act, Slottable sb, bool isPAS){
			if(sb.sg.isPool){
				act(sb.sg, sb, isPAS, isPAS);
				/*
					no toggling since it is handled in the tester side, not int this testee side
				*/
				foreach(EquipmentSet eSet in ssm.equipmentSets){
					ssm.PointFocus(eSet);
					foreach(SlotGroup sge in ssm.focusedSGEs){
						sge.ToggleAutoSort(true);
						act(sge, sb, isPAS, true);
						sge.ToggleAutoSort(false);
						act(sge, sb, isPAS, false);
					}
				}
				foreach(SlotGroup sgg in ssm.allSGGs){
					ssm.PointFocus(sgg);
					sgg.ToggleAutoSort(true);
					act(sgg, sb, isPAS, true);
					sgg.ToggleAutoSort(false);
					act(sgg, sb, isPAS, false);
				}
			}else if(sb.sg.isSGE){
				foreach(SlotGroup sgp in ssm.allSGPs){
					ssm.PointFocus(sgp);
					sgp.ToggleAutoSort(true);
					act(sgp, sb, isPAS, true);
					sgp.ToggleAutoSort(false);
					act(sgp, sb, isPAS, false);
				}
				ssm.PointFocus(sgpAll);
				foreach(SlotGroup sge in ssm.focusedSGEs){
					if(sge == sb.sg){
						act(sge, sb, isPAS, isPAS);
					}else{
						sge.ToggleAutoSort(true);
						act(sge, sb, isPAS, true);
						sge.ToggleAutoSort(false);
						act(sge, sb, isPAS, false);
					}
				}
				foreach(SlotGroup sgg in ssm.allSGGs){
					ssm.PointFocus(sgg);
					sgg.ToggleAutoSort(true);
					act(sgg, sb, isPAS, true);
					sgg.ToggleAutoSort(false);
					act(sgg, sb, isPAS, false);
				}
			}
		}
		public void TestCrossTestSG(){
			PerformOnAllSBs(CrossTestCrossTest);
			PrintTestResult();
			}
			public void CrossTestCrossTest(Slottable sb, bool isPAS){
				CrossTestSG(TestCrossTest, sb, isPAS);
			}
			public void TestCrossTest(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				Capture(ssm, testSB, tarSG, isPAS, isTAS, TestElement.SG);
			}
		public void CrossTestSGSelectively(System.Action<SlotGroup, Slottable, bool, bool> act, Slottable sb, bool isPAS, SlotGroup tarSG){
			if(sb.sg.isPool){
				if(sb.sg == tarSG)
					act(sb.sg, sb, isPAS, isPAS);
				if(tarSG.isSGE)
					foreach(EquipmentSet eSet in ssm.equipmentSets){
						ssm.PointFocus(eSet);
						foreach(SlotGroup sge in ssm.focusedSGEs){
							if(sge == tarSG){
								sge.ToggleAutoSort(true);
								act(sge, sb, isPAS, true);
								sge.ToggleAutoSort(false);
								act(sge, sb, isPAS, false);
							}
						}
					}
				if(tarSG.isSGG)
					foreach(SlotGroup sgg in ssm.allSGGs){
						if(sgg == tarSG){
							ssm.PointFocus(sgg);
							sgg.ToggleAutoSort(true);
							act(sgg, sb, isPAS, true);
							sgg.ToggleAutoSort(false);
							act(sgg, sb, isPAS, false);
						}
					}
			}else if(sb.sg.isSGE){
				if(tarSG.isPool)
					foreach(SlotGroup sgp in ssm.allSGPs){
						if(sgp == tarSG){
							ssm.PointFocus(sgp);
							sgp.ToggleAutoSort(true);
							act(sgp, sb, isPAS, true);
							sgp.ToggleAutoSort(false);
							act(sgp, sb, isPAS, false);
						}
					}
				ssm.PointFocus(sgpAll);
				if(tarSG.isSGE)
					foreach(SlotGroup sge in ssm.focusedSGEs){
						if(sge == tarSG){
							sge.ToggleAutoSort(true);
							act(sge, sb, isPAS, true);
							sge.ToggleAutoSort(false);
							act(sge, sb, isPAS, false);
						}
					}
				if(tarSG.isSGG)
					foreach(SlotGroup sgg in ssm.allSGGs){
						if(sgg == tarSG){
							ssm.PointFocus(sgg);
							sgg.ToggleAutoSort(true);
							act(sgg, sb, isPAS, true);
							sgg.ToggleAutoSort(false);
							act(sgg, sb, isPAS, false);
						}
					}
			}else if(sb.sg.isSGG){
				if(sb.sg == tarSG)
					act(sb.sg, sb, isPAS, isPAS);
				if(tarSG.isPool)
					foreach(SlotGroup sgp in ssm.allSGPs){
						if(sgp == tarSG){
							ssm.PointFocus(sgp);
							sgp.ToggleAutoSort(true);
							act(sgp, sb, isPAS, true);
							sgp.ToggleAutoSort(false);
							act(sgp, sb, isPAS, false);
						}
					}
				ssm.PointFocus(sgpAll);
				if(tarSG.isSGE)
					foreach(EquipmentSet eSet in ssm.equipmentSets){
						ssm.PointFocus(eSet);
						foreach(SlotGroup sge in ssm.focusedSGEs){
							if(tarSG == sge){
								sge.ToggleAutoSort(true);
								act(sge, sb, isPAS, true);
								sge.ToggleAutoSort(false);
								act(sge, sb, isPAS, false);
							}
						}
					}
			}
		}
		public void PerformOnAllSGs(System.Action<SlotGroup> act){
			foreach(SlotGroup sgp in ssm.allSGs){
				act(sgp);
			}
		}
	/*	actions	*/
		public void ClearSGCGearsTo(SlotGroup sgp){
			foreach(Slottable sb in sgeCGears){
				if(sb != null){
					Fill(sb, sgeCGears, sgp);
				}
			}
			AssertSGCounts(sgeCGears, ssm.equipInv.equippableCGearsCount, 0, 0);
			AssertFocused();
		}
		public void Swap(Slottable testSB, SlotGroup origSG, SlotSystemElement hovered){
				if(testSB.isFocused){
					SlotSystemTransaction ta = ssm.GetTransaction(testSB, hovered);
					bool isOnSG = hovered is SlotGroup;
					bool isOnSB = hovered is Slottable;
					Slottable hovSB = isOnSG?null:(Slottable)hovered;
					SlotGroup hovSG = isOnSG?(SlotGroup)hovered:null;
					if(ta.GetType() == typeof(SwapTransaction)){
						Slottable targetSB = isOnSB?hovSB:ta.targetSB;
						SlotGroup targetSG = isOnSG?hovSG:ta.sg2;
						AssertFocused();
							ASSSM(ssm,
								null, null, null, null, null, null, null,
								null, SSMFocused, null,
								null, SSMWFA, null,
								null, true, true, true, true);
							ASSG(origSG,
								null, SGFocused, null,
								null, SGWFA, null, false);
							ASSB(testSB,
								null, SBFocused, null,
								null, SBWFA, null, false,
								null, null, null,
								null, null, null);
						PickUp(testSB, out picked);
							ASSSM(ssm,
								testSB, null, null, null, testSB, null, testSB,
								null, SSMFocused, null,
								SSMWFA, SSMProbing, typeof(SSMProbeProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								null, SGWFA, null, false);
							ASSB(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null);
						if(isOnSG)
							SimHover(targetSG);
						else if(isOnSB)
							SimHover(targetSB);
							ASSSM(ssm,
								testSB, targetSB, origSG, targetSG, testSB, null/*null until execution*/, hovered,
								null, SSMFocused, null,
								SSMWFA, SSMProbing, typeof(SSMProbeProcess),
								typeof(SwapTransaction), false, true/* */, false, false);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								null, SGWFA, null, false);
							ASSG(targetSG,
								null, SGSelected, typeof(SGHighlightProcess),
								null, SGWFA, null, false);
							ASSB(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null);
							ASSB(targetSB,
								null, SBSelected, typeof(SBHighlightProcess),
								null, SBWFA, null, false,
								null, null, null);
						LetGo();
							ASSSM(ssm,
								testSB, targetSB, origSG, targetSG, testSB, targetSB, hovered,
								null, SSMFocused, null,
								SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(SwapTransaction), false, false, origSG.isAllTASBsDone?true:false, targetSG.isAllTASBsDone?true:false);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								SGWFA, SGSwap, typeof(SGTransactionProcess), origSG.isAllTASBsDone?false:true);
							ASSG(targetSG,
								null, SGSelected, typeof(SGHighlightProcess),
								SGWFA, SGSwap, typeof(SGTransactionProcess), targetSG.isAllTASBsDone?false:true);
							ASBSelState(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess));
								if(origSG.isPool)
									ASBActState(testSB,
										SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), testSB.actProcess.isRunning);
								else
									ASBActState(testSB,
										SBPickedUp, SBRemove, typeof(SBRemoveProcess), true);
								if(targetSG.isSGE)
									ASBEqpState(testSB,
										SBUnequipped, SBEquipped, typeof(SBEquipProcess));
							ASBSelState(targetSB,
								null, SBSelected, typeof(SBHighlightProcess));
								if(targetSG.isPool)
									ASBActState(targetSB,
										SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), targetSB.actProcess.isRunning);
								else
									ASBActState(targetSB,
										SBWFA, SBRemove, typeof(SBRemoveProcess), true);
							Slottable tarSBinOrigSG = origSG.GetSB(targetSB.itemInst);
							if(origSG.isPool)
								ASSB(tarSBinOrigSG,
									null, origSG.isAutoSort?SBDefocused:SBFocused, null,
									SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), tarSBinOrigSG.actProcess.isRunning,
									SBEquipped, SBUnequipped, typeof(SBUnequipProcess));
							else
								ASSB(tarSBinOrigSG,
									null, SBDefocused, null,
									SBWFA, SBAdd, typeof(SBAddProcess), true,
									SBUnequipped, SBEquipped, null);
							Slottable testSBinTarSG = targetSG.GetSB(testSB.itemInst);
							if(targetSG.isPool)/* 	unequipped	*/
								ASSB(testSBinTarSG,
									null, SBDefocused, null,
									SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), testSBinTarSG.actProcess.isRunning,
									SBEquipped, SBUnequipped, typeof(SBUnequipProcess));
							else/*	newly created	*/
								ASSB(testSBinTarSG,
									SBDeactivated, SBDefocused, null,
									SBWFA, SBAdd, typeof(SBAddProcess), true,
									SBUnequipped, SBEquipped, null);
						if(!origSG.isAllTASBsDone)
							CompleteAllSBActProcesses(origSG);
						if(!targetSG.isAllTASBsDone)
							CompleteAllSBActProcesses(targetSG);
						ssm.dIcon1.CompleteMovement();
						ssm.dIcon2.CompleteMovement();
						AssertFocused();
					}else
						throw new System.InvalidOperationException("SlottableTest.Swap: given combination of arguments does not result in SwapTransaction");
				}else
					throw new System.InvalidOperationException("SlottableTest.Swap: testSB is not pickable");
			}public void TestSwapShortcut(){
			PerformOnAllSBs(CrossTestSwapShortcut);
			PrintTestResult();
			}
			public void CrossTestSwapShortcut(Slottable sb, bool isPAS){
				CrossTestSG(TestSwapShortcut, sb, isPAS);
				origSGCache = null;
			}
			public void TestSwapShortcut(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				InventoryItemInstanceMock testItem = testSB.itemInst;
				SlotGroup origSG = testSB.sg;
				if(origSG != null)
					origSGCache = origSG;
				else
					origSG = origSGCache;
				testSB = origSG.GetSB(testItem);
				if(testSB.isFocused){
					SlotSystemTransaction ta = ssm.GetTransaction(testSB, tarSG);
					if(ta.GetType() == typeof(SwapTransaction)){
						InventoryItemInstanceMock swapItem = ta.targetSB.itemInst;
						Swap(testSB, origSG, tarSG);
						/*	reverse */
						Swap(origSG.GetSB(swapItem), origSG, tarSG);
					}
				}
				foreach(Slottable tarSB in tarSG){
					if(tarSB != null){
						testSB = origSG.GetSB(testItem);
						if(testSB.isFocused){
							SlotSystemTransaction ta = ssm.GetTransaction(testSB, tarSB);
							if(ta.GetType() == typeof(SwapTransaction)){
								Capture(ssm, testSB, tarSB, isPAS, isTAS, TestElement.SB);
								InventoryItemInstanceMock swapItem = tarSB.itemInst;
								Swap(testSB, origSG, tarSB);
								/*	reverse	*/
								Swap(tarSG.GetSB(testItem), tarSG, origSG.GetSB(swapItem));
							}
						}
					}
				}
			}

		public void Fill(Slottable testSB, SlotGroup origSG, SlotSystemElement hovered){
			if(testSB.isFocused){
				Slottable hovSB = hovered is Slottable?(Slottable)hovered: null;
				SlotGroup hovSG = hovered is SlotGroup?(SlotGroup)hovered: null;
				bool isOnSG = hovSG != null && hovSB == null;
				bool isOnSB = hovSB != null && hovSG == null;
				if(isOnSG || isOnSB){
					SlotSystemTransaction ta = ssm.GetTransaction(testSB, hovered);
					if(ta.GetType() == typeof(FillTransaction)){
						SlotGroup targetSG = isOnSG?hovSG:ta.sg2;
						AssertFocused();
							ASSSM(ssm,
								null, null, null, null, null, null, null,
								null, SSMFocused, null,
								null, SSMWFA, null,
								null, true, true, true, true);
							ASSG(origSG,
								null, SGFocused, null,
								null, SGWFA, null, false);
							ASSB(testSB,
								null, SBFocused, null,
								null, SBWFA, null, false,
								null, null, null);
						PickUp(testSB, out picked);
							ASSSM(ssm,
								testSB, null, null, null, testSB, null, testSB,
								null, SSMFocused, null,
								SSMWFA, SSMProbing, typeof(SSMProbeProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								null, SGWFA, null, false);
								Print(ssm);
							ASSG(targetSG,
								null, SGFocused, null,
								null, SGWFA, null, false);
							ASSB(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null);
							if(isOnSB && hovSB != testSB)
							ASSB(hovSB,
								null, SBDefocused, null,
								null, SBWFA, null, false,
								null, null, null);
						if(hovSG != null)
							SimHover(hovSG);
						else if(hovSB != null){
							SimHover(hovSB);
						}
							ASSSM(ssm,
								testSB, null, origSG, targetSG, testSB, null, hovered,
								null, SSMFocused, null,
								SSMWFA, SSMProbing, typeof(SSMProbeProcess),
								typeof(FillTransaction), false, true, false, false);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								null, SGWFA, null, false);
							ASSG(targetSG,
								SGFocused, SGSelected, typeof(SGHighlightProcess),
								null, SGWFA, null, false);
							ASSB(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null);
							if(isOnSB && hovSB != testSB)
							ASSB(hovSB,
								null, SBDefocused, null,
								null, SBWFA, null, false,
								null, null, null);
						LetGo();
							ASSSM(ssm,
								testSB, null, origSG, targetSG, testSB, null, hovered,
								null, SSMFocused, null,
								SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(FillTransaction), false, true, origSG.isAllTASBsDone?true:false, targetSG.isAllTASBsDone?true:false);
							ASSG(origSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								SGWFA, SGFill, typeof(SGTransactionProcess), !origSG.isAllTASBsDone);
							ASSG(targetSG,
								SGFocused, SGSelected, typeof(SGHighlightProcess),
								SGWFA, SGFill, typeof(SGTransactionProcess), !targetSG.isAllTASBsDone);
							ASBSelState(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess));
								if(origSG.isPool){
									ASBActState(testSB,
										SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), testSB.actProcess.isRunning);
								}else{
									ASBActState(testSB,
										SBPickedUp, SBRemove, typeof(SBRemoveProcess), true);
								}
							Slottable testSBinTarSG = targetSG.GetSB(testSB.itemInst);
							ASBSelState(testSBinTarSG,
								null, SBDefocused, null);
								if(targetSG.isPool){
									ASBActState(testSBinTarSG,
										SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), testSBinTarSG.actProcess.isRunning);
								}else{
									ASBActState(testSBinTarSG,
										SBWFA, SBAdd, typeof(SBAddProcess), true);
								}
							if(isOnSB && hovSB != testSB)
							ASSB(hovSB,
								null, SBDefocused, null,
								SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), hovSB.actProcess.isRunning,
								null, null, null);
						if(!origSG.isAllTASBsDone)
							CompleteAllSBActProcesses(origSG);
							ASSSM(ssm,
								testSB, null, origSG, targetSG, testSB, null, hovered,
								null, SSMFocused, null,
								SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(FillTransaction), false, true, true, targetSG.isAllTASBsDone?true:false);
						if(!targetSG.isAllTASBsDone)
							CompleteAllSBActProcesses(targetSG);
							ASSSM(ssm,
								testSB, null, origSG, targetSG, testSB, null, hovered,
								null, SSMFocused, null,
								SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(FillTransaction), false, true, true, true);
						ssm.dIcon1.CompleteMovement();
						AssertFocused();
					}else
						throw new System.InvalidOperationException("SlottableTest.Fill: given combination of arguments does not result in FillTransaction");
				}else
					throw new System.InvalidOperationException("SlottableTest.Fill: tarSG and tarSB not supplied correctly");
			}else
				throw new System.InvalidOperationException("SlottableTest.Fill: testSB is not pickable");
			}public void TestFillShortcut(){
				PerformOnAllSBs(CrossTestFillShortcut);
				PrintTestResult();
			}
			public void CrossTestFillShortcut(Slottable sb, bool isPAS){
				CrossTestSG(TestFillShortcut, sb, isPAS);
				origSGCache = null;
			}
			public void TestFillShortcut(SlotGroup tarSG, Slottable testSB, bool isPAS, bool isTAS){
				InventoryItemInstanceMock testItem = testSB.itemInst;
				SlotGroup origSG = testSB.sg;
				if(origSG != null)
					origSGCache = origSG;
				else
					origSG = origSGCache;
				testSB = origSG.GetSB(testItem);
				if(testSB.isFocused){
					if(ssm.GetTransaction(testSB, tarSG).GetType() == typeof(FillTransaction)){
						Fill(testSB, origSG, tarSG);
						/*	rev */
						Fill(tarSG.GetSB(testItem), tarSG, origSG);
					}
					foreach(Slottable tarSB in tarSG){
						if(tarSB != null){
							testSB = origSG.GetSB(testItem);
							if(ssm.GetTransaction(testSB, tarSB).GetType() == typeof(FillTransaction)){
								Fill(testSB, origSG, tarSB);
								/*	rev */
								Fill(tarSG.GetSB(testItem), tarSG, origSG);
							}
						}
					}
				}
			}

		public void PickUp(Slottable sb, out bool pickedUp){
			SlotGroup origSG = sb.sg;
			AssertFocused();
			if(sb.isFocused){
				sb.OnPointerDownMock(eventData);
					ASSSM(sb.ssm,
						null, null, null, null, null, null, null, 
						null, SSMFocused, null,
						null, SSMWFA, null,
						null, true, true, true, true);
					ASSG(origSG,
						null, SGFocused, null,
						null, SGWFA, null, false);
					ASSB(sb,
						SBFocused, SBSelected, typeof(SBHighlightProcess),
						SBWFA, SBWFPickUp, typeof(WaitForPickUpProcess), true, 
						null, null, null);
				sb.actProcess.Expire();
					ASSSM(sb.ssm,
						sb, null, null, null, sb, null, sb, 
						null, SSMFocused, null,
						SSMWFA, SSMProbing, typeof(SSMProbeProcess),
						typeof(RevertTransaction), false, true, true, true);
					ASSG(origSG,
						SGFocused, SGDefocused, typeof(SGGreyoutProcess),
						null, SGWFA, null, false);
					ASSB(sb,
						SBSelected, SBDefocused, typeof(SBGreyoutProcess),
						SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true, 
						null, null, null);
				pickedUp = true;
			}else{
				pickedUp = false;
			}
		}
		public void PointerUp(){
			ssm.pickedSB.OnPointerUpMock(eventData);
		}
		public void LetGo(){
			Slottable pickedSB = ssm.pickedSB;
				ASSMActState(ssm, SSMWFA, SSMProbing, typeof(SSMProbeProcess));
				ASSG(pickedSB.sg,
					SGFocused, SGDefocused, typeof(SGGreyoutProcess),
					null, SGWFA, null, false);
				ASSB(pickedSB,
					SBSelected, SBDefocused, typeof(SBGreyoutProcess),
					null, SBPickedUp, null, false,
					null, null, null);
			pickedSB.OnPointerUpMock(eventData);
			if(ssm.pickedSB.curActState == Slottable.waitForNextTouchState){
				ASSMActState(ssm, SSMWFA, SSMProbing, typeof(SSMProbeProcess));
				ASSG(pickedSB.sg,
					SGFocused, SGDefocused, typeof(SGGreyoutProcess),
					null, SGWFA, null, false);
				ASSB(pickedSB,
					SBSelected, SBDefocused, typeof(SBGreyoutProcess),
					SBPickedUp, SBWFNT, typeof(WaitForNextTouchProcess), true,
					null, null, null);
				ssm.pickedSB.actProcess.Expire();
			}
				ASSMActState(ssm, SSMProbing, SSMTransaction, typeof(SSMTransactionProcess));
				ASGSelState(pickedSB.sg,
					SGFocused, SGDefocused, typeof(SGGreyoutProcess));
		}
		public void CompleteAllSBActProcesses(SlotGroup sg){
			foreach(Slottable sb in sg){
				if(sb != null){
					if(sb.actProcess.GetType() == typeof(SBRemoveProcess) ||
					sb.actProcess.GetType() == typeof(SBAddProcess) ||
					sb.actProcess.GetType() == typeof(SBMoveWithinProcess))
						if(sb.actProcess.isRunning)
							sb.actProcess.Expire();
				}
			}
			sg.CheckProcessCompletion();
		}
		public void SimHover(SlotSystemElement hovered){
			/*	revised version
					sgm.SetHovered(sb, sg);
						=> update hovered fields
					sgm.UpdateTransaction();
						=> update target fields
							=> thus the selection states of prev and current tartgets
			*/
			/*	in actual implementation, this method is called whenever either sb or sg's boarder is crossed
			*/
			/*	no need to create transactions since they're already created at pickup and stashed
			*/
			ssm.SetHovered(hovered);
			ssm.UpdateTransaction();
		}
		public void Reorder(Slottable testSB, Slottable hovSB){
			if(testSB.isFocused){
				SlotSystemTransaction ta = ssm.GetTransaction(testSB, hovSB);
				if(ta.GetType() == typeof(ReorderTransaction)){
					SlotGroup origSG = testSB.sg;
						AssertFocused();
						AE(hovSB, ta.targetSB);
					PickUp(testSB, out picked);
						ASSSM(ssm,
							testSB, null, null, null, testSB, null, testSB, 
							SSMDeactivated, SSMFocused, null,
							SSMWFA, SSMProbing, typeof(SSMProbeProcess),
							typeof(RevertTransaction), false, true, true, true);
						ASSG(origSG,
							SGFocused, SGDefocused, typeof(SGGreyoutProcess),
							null, SGWFA, null, false);
						ASSB(testSB,
							SBSelected, SBDefocused, typeof(SBGreyoutProcess),
							SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
							null, null, null);
						ASSB(hovSB,
							null, SBFocused, null,
							null, SBWFA, null, true,
							null, null, null);
					SimHover(hovSB);
						ASSSM(ssm,
							testSB, hovSB, origSG, null, testSB, null, hovSB, 
							SSMDeactivated, SSMFocused, null,
							SSMWFA, SSMProbing, typeof(SSMProbeProcess),
							typeof(ReorderTransaction), false, true, false, true);
						ASSG(origSG,
							SGFocused, SGDefocused, typeof(SGGreyoutProcess),
							null, SGWFA, null, false);
						ASSB(testSB,
							SBSelected, SBDefocused, typeof(SBGreyoutProcess),
							SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
							null, null, null);
						ASSB(hovSB,
							SBFocused, SBSelected, typeof(SBHighlightProcess),
							null, SBWFA, null, true,
							null, null, null);
					LetGo();
						ASSSM(ssm,
							testSB, hovSB, origSG, null, testSB, null, hovSB, 
							SSMDeactivated, SSMFocused, null,
							SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
							typeof(ReorderTransaction), false, true, false, true);
						ASSG(origSG,
							SGFocused, SGDefocused, typeof(SGGreyoutProcess),
							SGWFA, SGReorder, typeof(SGTransactionProcess), true);
						ASSB(testSB,
							SBSelected, SBDefocused, typeof(SBGreyoutProcess),
							SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), true,
							null, null, null);
						ASSB(hovSB,
							SBFocused, SBSelected, typeof(SBHighlightProcess),
							SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), true,
							null, null, null);
					CompleteAllSBActProcesses(origSG);
						ASSSM(ssm,
							testSB, hovSB, origSG, null, testSB, null, hovSB, 
							SSMDeactivated, SSMFocused, null,
							SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
							typeof(ReorderTransaction), false, true, true, true);
					ssm.dIcon1.CompleteMovement();
						AssertFocused();
				}else
					throw new System.InvalidOperationException("SlottableTest.Reorder: given argumant comination does not yield Reorder Transaction");
			}else
				throw new System.InvalidOperationException("SlottableTest.Reorder: testSB not pickable");
		}
		public void Revert(Slottable testSB, SlotSystemElement hovered){
			if(testSB.isFocused){
				SlotSystemTransaction ta = ssm.GetTransaction(testSB, hovered);
				if(ta.GetType() == typeof(RevertTransaction)){
					SlotGroup origSG = testSB.sg;
					Slottable hovSB = hovered is Slottable?(Slottable)hovered: null;
					SlotGroup hovSG = hovered is SlotGroup?(SlotGroup)hovered: null;
					bool isOnSG = hovSG != null && hovSB ==null;
					bool isOnSB = hovSB != null && hovSG == null;
					if(isOnSG || isOnSB){
						SlotGroup targetSG = origSG;
						/* there's no sg1 in ta, Revert is kinda special in this respect */
							AssertFocused();
							ASSSM(testSB.ssm,
								null, null, null, null, null, null, null, 
								SSMDeactivated, SSMFocused, null,
								null, SSMWFA, null,
								null, true, true, true, true);
						PickUp(testSB, out picked);
							ASSSM(testSB.ssm,
								testSB, null, null, null, testSB, null, testSB,
								SSMDeactivated, SSMFocused, null,
								SSMWFA, SSMProbing, typeof(SSMProbeProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSB(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null);
							ASSG(targetSG,
								null, SGDefocused, null,
								null, SGWFA, null, false);
							if(isOnSG && hovSG != origSG)
							ASSG(hovSG,
								null, SGDefocused, null,
								null, SGWFA, null, false);
							if(isOnSB && hovSB != testSB)
							ASSB(hovSB,
								null, SBDefocused, null,
								null, SBWFA, null, false,
								null, null, null);
						SimHover(hovered);
							ASSSM(testSB.ssm,
								testSB, null, null, null, testSB, null, hovered, 
								SSMDeactivated, SSMFocused, null,
								SSMWFA, SSMProbing, typeof(SSMProbeProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(targetSG,
								null, SGDefocused, null,
								null, SGWFA, null, false);
							ASSB(testSB,
								SBSelected, SBDefocused, typeof(SBGreyoutProcess),
								SBWFPickUp, SBPickedUp, typeof(SBPickedUpProcess), true,
								null, null, null);
							if(isOnSG && hovSG != origSG)
							ASSG(hovSG,
								null, SGDefocused, null,
								null, SGWFA, null, false);
							if(isOnSB && hovSB != testSB)
							ASSB(hovSB,
								null, SBDefocused, null,
								null, SBWFA, null, false,
								null, null, null);
						LetGo();
							ASSSM(testSB.ssm,
								testSB, null, null, null, testSB, null, hovered, 
								SSMDeactivated, SSMFocused, null,
								SSMProbing, SSMTransaction, typeof(SSMTransactionProcess),
								typeof(RevertTransaction), false, true, true, true);
							ASSG(targetSG,
								SGFocused, SGDefocused, typeof(SGGreyoutProcess),
								SGWFA, SGRevert, typeof(SGTransactionProcess), false);
							ASBSelState(testSB, SBSelected, SBDefocused, typeof(SBGreyoutProcess));
							if(isOnSB && testSB.isStackable && testSB == hovSB){
								ASBActState(testSB, SBWFNT, SBMoveWithin, typeof(SBMoveWithinProcess), testSB.actProcess.isRunning);
							}else
								ASBActState(testSB, SBPickedUp, SBMoveWithin, typeof(SBMoveWithinProcess), testSB.actProcess.isRunning);
							if(isOnSG && hovSG != origSG)
							ASSG(hovSG,
								null, SGDefocused, null,
								null, SGWFA, null, false);
							if(isOnSB && hovSB != testSB && hovSB.sg == origSG)
							ASSB(hovSB,
								null, SBDefocused, null,
								SBWFA, SBMoveWithin, typeof(SBMoveWithinProcess), hovSB.actProcess.isRunning,
								null, null, null);
						ssm.dIcon1.CompleteMovement();
							AssertFocused();
					}else
						throw new System.InvalidOperationException("SlottableTest.Revert: tarSG and tarSB not set correctly (either both are present or are missing)");
				}else
					throw new System.InvalidOperationException("SlottableTest.Revert: given combination of arguments does not result in Revert Transaction");
			}else
				throw new System.InvalidOperationException("SlottableTest.Revert: testSB not pickable");
		}
	/*	Assertions	*/
		/*	SSM	*/
			public void ASSMSelState(SlotSystemManager ssm, SSMSelState prev, SSMSelState cur, System.Type selProcT){
				AE(ssm.curSelState, cur);
				if(prev != null){
					AE(ssm.prevSelState, prev);
					if(selProcT != null)
						AE(ssm.selProcess.GetType(), selProcT);
					else
						ANull(ssm.selProcess);
				}
			}
			public void ASSMActState(SlotSystemManager ssm, SSMActState prev, SSMActState cur, System.Type actProcT){
				AE(ssm.curActState, cur);
				if(prev != null){
					AE(ssm.prevActState, prev);
					if(actProcT != null)
						AE(ssm.actProcess.GetType(), actProcT);
					else
						ANull(ssm.actProcess);
				}
			}
			public void ASSSM(SlotSystemManager ssm,
				Slottable pickedSB, Slottable targetSB, SlotGroup sg1, SlotGroup sg2, 
				Slottable di1SB, Slottable di2SB, SlotSystemElement hovered,
				SSMSelState prevSel, SSMSelState curSel, System.Type selProcT,
				SSMActState prevAct, SSMActState curAct, System.Type actProcT,
				System.Type taType, bool d1Done, bool d2Done, bool sg1Done, bool sg2Done){
				AE(ssm.pickedSB, pickedSB);
				AE(ssm.targetSB, targetSB);
				AE(ssm.sg1, sg1);
				AE(ssm.sg2, sg2);
				if(di1SB != null)
					AE(ssm.dIcon1.item, di1SB.itemInst);
				else
					AE(ssm.dIcon1, null);
				if(di2SB != null)
					AE(ssm.dIcon2.item, di2SB.itemInst);
				else
					AE(ssm.dIcon2, null);
				AE(ssm.hovered, hovered);
				ASSMSelState(ssm, prevSel, curSel, selProcT);
				ASSMActState(ssm, prevAct, curAct, actProcT);
				if(taType == null) ANull(ssm.transaction);
				else AE(ssm.transaction.GetType(), taType);
				AE(ssm.dIcon1Done, d1Done);
				AE(ssm.dIcon2Done, d2Done);
				AE(ssm.sg1Done, sg1Done);
				AE(ssm.sg2Done, sg2Done);
			}
			public void ASSSM(SlotSystemManager ssm,
				Slottable pickedSB, Slottable targetSB, SlotGroup sg1, SlotGroup sg2, 
				DraggedIcon di1, DraggedIcon di2, SlotSystemElement hovered,
				SSMSelState prevSel, SSMSelState curSel, System.Type selProcT,
				SSMActState prevAct, SSMActState curAct, System.Type actProcT){
				AE(ssm.pickedSB, pickedSB);
				AE(ssm.targetSB, targetSB);
				AE(ssm.sg1, sg1);
				AE(ssm.sg2, sg2);
				AE(ssm.dIcon1, di1);
				AE(ssm.dIcon2, di2);
				AE(ssm.hovered, hovered);
				ASSMSelState(ssm, prevSel, curSel, selProcT);
				ASSMActState(ssm, prevAct, curAct, actProcT);
			}
		/*	SG	*/
			public void ASGSelState(SlotGroup sg, SGSelState prev, SGSelState cur, System.Type procT){
				AE(sg.curSelState, cur);
				if(prev != null){
					AE(sg.prevSelState, prev);
					if(procT != null)
						AE(sg.selProcess.GetType(), procT);
					else
						ANull(sg.selProcess);
				}
			}
			public void ASGActState(SlotGroup sg, SGActState prev, SGActState cur, System.Type procT, bool isRunning){
				AE(sg.curActState, cur);
				if(prev != null){
					AE(sg.prevActState, prev);
					if(procT != null){
						AE(sg.actProcess.GetType(), procT);
						AE(sg.actProcess.isRunning, isRunning);
					}
					else
						ANull(sg.actProcess);
				}
			}
			public void AssertSGCounts(SlotGroup sg, int slotsC, int itemC, int sbsC){
				AE(sg.slots.Count, slotsC);
				AE(sg.actualItemInsts.Count, itemC);
				AE(sg.actualSBsCount, sbsC);
			}
			public void AssertSBsSorted(SlotGroup sg, SGSorter sorter){
				List<Slottable> sbs = sg.toList;
				sorter.OrderSBsWithRetainedSize(ref sbs);
				AE(sbs.Count, sg.Count);
				for(int i = 0; i < sg.Count; i++){
					AE((Slottable)sg[i], sbs[i]);
				}
			}
			public void ASSG(SlotGroup sg, SGSelState prevSel, SGSelState curSel, System.Type selProcT, SGActState prevAct, SGActState curAct, System.Type actProcT, bool isRunning){
				ASGSelState(sg, prevSel, curSel, selProcT);
				ASGActState(sg, prevAct, curAct, actProcT, isRunning);
			}
			public void ASGReset(SlotGroup sg){
				ASGActState(sg, null, SGWFA, null, false);
				AE(sg.newSBs, null);
				AE(sg.newSlots, null);
			}
		/*	SB	*/
			public void ASSB(Slottable sb,
			SBSelState prevSel, SBSelState curSel , System.Type selProcT,
			SBActState prevAct, SBActState curAct, System.Type actProcT, bool isRunning,
			SBEqpState prevEqp, SBEqpState curEqp, System.Type eqpProcT,
			SBMrkState prevMrk, SBMrkState curMrk, System.Type mrkProcT){
				ASBSelState(sb, prevSel, curSel, selProcT);
				ASBActState(sb, prevAct, curAct, actProcT, isRunning);
				if(curEqp != null)
					ASBEqpState(sb, prevEqp, curEqp, eqpProcT);
				if(curMrk != null)
					ASBMrkState(sb, prevMrk, curMrk, mrkProcT);
			}
			public void ASSB(Slottable sb,
			SBSelState prevSel, SBSelState curSel , System.Type selProcT,
			SBActState prevAct, SBActState curAct, System.Type actProcT, bool isRunning,
			SBEqpState prevEqp, SBEqpState curEqp, System.Type eqpProcT){
				ASBSelState(sb, prevSel, curSel, selProcT);
				ASBActState(sb, prevAct, curAct, actProcT, isRunning);
				if(curEqp != null)
					ASBEqpState(sb, prevEqp, curEqp, eqpProcT);
			}
			public void ASSB(Slottable sb,
			SBSelState prevSel, SBSelState curSel , System.Type selProcT,
			SBActState prevAct, SBActState curAct, System.Type actProcT, bool isRunning){
				ASBSelState(sb, prevSel, curSel, selProcT);
				ASBActState(sb, prevAct, curAct, actProcT, isRunning);
			}
			public void ASSB_s(Slottable sb, SBSelState selState, SBActState actState){
				AE(sb.curSelState, selState);
				AE(sb.curActState, actState);
			}
			public void ASBSelState(Slottable sb, SBSelState prev, SBSelState cur, System.Type procT){
				if(prev != null){
					AE(sb.prevSelState, prev);
					if(procT != null)
						AE(sb.selProcess.GetType(), procT);
					else
						ANull(sb.selProcess);
				}
				AE(sb.curSelState, cur);
			}
			public void ASBActState(Slottable sb, SBActState prev, SBActState cur, System.Type procT, bool isRunning){
				if(prev != null){
					AE(sb.prevActState, prev);
					if(procT != null){
						AE(sb.actProcess.GetType(), procT);
						AE(sb.actProcess.isRunning, isRunning);
					}
					else
						ANull(sb.actProcess);
				}
				AE(sb.curActState, cur);
			}
			public void ASBEqpState(Slottable sb, SBEqpState prev, SBEqpState cur, System.Type procT){
				if(prev != null){
					AE(sb.prevEqpState, prev);
					if(procT != null)
						AE(sb.eqpProcess.GetType(), procT);
					else
						ANull(sb.eqpProcess);
				}
				AE(sb.curEqpState, cur);
			}
			public void ASBMrkState(Slottable sb, SBMrkState prev, SBMrkState cur, System.Type procT){
				if(prev != null){
					AE(sb.prevMrkState, prev);
					if(procT != null)
						AE(sb.eqpProcess.GetType(), procT);
					else
						ANull(sb.eqpProcess);
				}
				AE(sb.curMrkState, cur);
			}
			public void ASBReset(Slottable sb){
				ASBActState(sb, null, SBWFA, null, false);
				AE(sb.pickedAmount, 0);
				AE(sb.newSlotID, -2);
			}
		/*	other	*/
			public void ANotNull(object obj){
				// Assert.That(obj, Is.Not.Null);
				AB(obj != null, true);
			}
			public void ANull(object obj){
				AB(obj == null, true);
			}
			public void AE(object inspected, object expected){
				if(expected != null)
					Assert.That(inspected, Is.EqualTo(expected));
				else
					ANull(inspected);
			}
			public void AB(bool inspectedBool, bool value){
				if(value)
					Assert.That(inspectedBool, Is.True);
				else
					Assert.That(inspectedBool, Is.False);
			}
			public void AssertEquipped(InventoryItemInstanceMock itemInst){
				if(itemInst is BowInstanceMock || itemInst is WearInstanceMock){
					/*	Bow or Wear?	*/
					bool isBow = false;
					if(itemInst is BowInstanceMock)
						isBow = true;
					System.Type typeToCheck;
					InventoryItemInstanceMock ssmEquipped;
					Slottable sbe;
					SlotGroup sge;
					SlotGroup sgp;
					if(isBow){
						typeToCheck = typeof(BowInstanceMock);
						ssmEquipped = ssm.equippedBowInst;
						sge = sgeBow;
						sgp = sgpBow;
					}else{
						typeToCheck = typeof(WearInstanceMock);
						ssmEquipped = ssm.equippedWearInst;
						sge = sgeWear;
						sgp = sgpWear;
					}
					foreach(Slottable sbp in sgpAll){
						if(sbp != null){
							InventoryItemInstanceMock sbpItem = sbp.itemInst;
							if(sbpItem.GetType() == typeToCheck)
							{
								if(sgpAll.isFocusedInHierarchy){
									bool isFilteredIn = false;
									ssm.PrePickFilter(sbp, out isFilteredIn);
									if(isFilteredIn)
										ASSB(sbp,
											null, SBFocused, null,
											null, SBWFA, null, false, 
											null, sbp.isEquipped?SBEquipped:SBUnequipped, null,
											null, null, null);
									else
										ASSB(sbp,
											null, SBDefocused, null,
											null, SBWFA, null, false,
											null, sbp.isEquipped?SBEquipped:SBUnequipped, null,
											null, null, null);
								}else{
									ASSB(sbp,
										null, SBDefocused, null,
										null, SBWFA, null, false,
										null, sbp.isEquipped?SBEquipped:SBUnequipped, null,
										null, null, null);
								}
								if(sbpItem == itemInst){
									AB(sbp.isEquipped, true);
									AB(sgpAll.equippedSBs.Contains(sbp), true);
									AE(ssmEquipped, sbpItem);
									sbe = sge.GetSB(sbpItem);
									AB(sbe != null, true);
									AE(sge.equippedSBs.Count, 1);
									AB(sge.equippedSBs.Contains(sbe), true);
									bool isSBEFiltered;
									ssm.PrePickFilter(sbe, out isSBEFiltered);
									ASSB(sbe,
										null, isSBEFiltered?SBFocused:SBDefocused, null,
										null, SBWFA, null, false, 
										null, SBEquipped, null,
										null, null, null);
								}else{// deemed not the equipped bow/wear
									AB(sbp.isEquipped, false);
									AB(sgpAll.equippedSBs.Contains(sbp), false);
									AB(ssmEquipped != sbpItem, true);
									ANull(sge.GetSB(sbpItem));
								}
							}
						}
					}
					foreach(Slottable sbp in sgp){
						if(sbp != null){
							InventoryItemInstanceMock sbpItem = sbp.itemInst;
							if(sgp.isFocusedInHierarchy){
								bool isFilteredIn = false;
								ssm.PrePickFilter(sbp, out isFilteredIn);
								if(isFilteredIn)
									ASSB(sbp,
										null, SBFocused, null,
										null, SBWFA, null, false, 
										null, sbp.isEquipped?SBEquipped: SBUnequipped, null,
										null, null, null);
								else
									ASSB(sbp,
										null, SBDefocused, null,
										null, SBWFA, null, false,
										null, sbp.isEquipped?SBEquipped: SBUnequipped, null,
										null, null, null);
							}else{
								ASSB(sbp,
									null, SBDefocused, null,
									null, SBWFA, null, false,
									null, sbp.isEquipped?SBEquipped: SBUnequipped, null,
									null, null, null);
							}
							if(sbpItem == itemInst){
								AB(sbp.isEquipped, true);
								AB(sgp.equippedSBs.Contains(sbp), true);
								AE(ssmEquipped, sbpItem);
								sbe = sge.GetSB(sbpItem);
								AB(sbe != null, true);
								AE(sge.equippedSBs.Count, 1);
								AB(sge.equippedSBs.Contains(sbe), true);
							}else{/* deemed not equipped	*/
								AB(sbp.isEquipped, false);
									AB(sgp.equippedSBs.Contains(sbp), false);
									AB(ssmEquipped != sbpItem, true);
									ANull(sge.GetSB(sbpItem));
							}
						}
					}
				}
			}
			public void AECGears(List<CarriedGearInstanceMock> items, PoolInventory poolInv, EquipmentSetInventory equipInv){
				if(items != null){
					AE(ssm.equippedCarriedGears.Count, items.Count);
					AE(sgpAll.equippedSBs.Count, items.Count + 2);
					AE(sgpCGears.equippedSBs.Count, items.Count);
					AE(sgeCGears.equippedSBs.Count, items.Count);
					foreach(CarriedGearInstanceMock item in items){
						AB(ssm.equippedCarriedGears.Contains(item), true);
						AB(sgpAll.equippedSBs.Contains(sgpAll.GetSB(item)), true);
						ANull(sgpBow.GetSB(item));
						ANull(sgpWear.GetSB(item));
						ANull(sgpParts.GetSB(item));
						AB(sgpCGears.equippedSBs.Contains(sgpCGears.GetSB(item)), true);
						ANull(sgeBow.GetSB(item));
						ANull(sgeWear.GetSB(item));
						AB(sgeCGears.equippedSBs.Contains(sgeCGears.GetSB(item)), true);
					}
				}
				foreach(InventoryItemInstanceMock itemInInv in poolInv){
					if(itemInInv is CarriedGearInstanceMock){
						if(items.Contains((CarriedGearInstanceMock)itemInInv)){
							AB(itemInInv.isEquipped, true);
							AB(sgpAll.GetSB(itemInInv).isEquipped, true);
							AB(sgpCGears.GetSB(itemInInv).isEquipped, true);
						}else{//deemed not equipped
							AB(itemInInv.isEquipped, false);
							AB(sgpAll.GetSB(itemInInv).isEquipped, false);
							AB(sgpCGears.GetSB(itemInInv).isEquipped, false);
						}
					}
				}
				foreach(CarriedGearInstanceMock item in items){
					if(item != null){
						AE(equipInv.Contains(item), true);
					}
				}
				foreach(InventoryItemInstanceMock itemInInv in equipInv){
					if(itemInInv is CarriedGearInstanceMock){
						AB(itemInInv.isEquipped, true);
						AE(items.Contains((CarriedGearInstanceMock)itemInInv), true);
						AB(sgeCGears.GetSB(itemInInv).isEquipped, true);
					}
				}
			}
			public void AECGears(Slottable cg1, Slottable cg2, Slottable cg3, Slottable cg4){
				List<CarriedGearInstanceMock> checkedList = new List<CarriedGearInstanceMock>();
				if(cg1 != null)
					checkedList.Add((CarriedGearInstanceMock)cg1.item);
				else
					checkedList.Add(null);
				if(cg2 != null)
					checkedList.Add((CarriedGearInstanceMock)cg2.item);
				else
					checkedList.Add(null);
				if(cg3 != null)
					checkedList.Add((CarriedGearInstanceMock)cg3.item);
				else
					checkedList.Add(null);
				if(cg4 != null)
					checkedList.Add((CarriedGearInstanceMock)cg4.item);
				else
					checkedList.Add(null);
				
				int allowedCount = ((EquipmentSetInventory)sgeCGears.inventory).equippableCGearsCount;
				
				for(int i = 0; i < 4; i++){
					if(i +1 > allowedCount)
						if(checkedList[i] != null)
							throw new System.InvalidOperationException("Slottable at index " + i + " is not checked since it exceeds the max slot count");
				}
				for(int i = 0; i < allowedCount; i++){
					Slottable sb = sgeCGears.slots[i].sb;
					if(sb != null)
						AE(sb.item, checkedList[i]);
					else
						Assert.That(checkedList[i], Is.Null);
				}
				foreach(SlotGroup sgp in ssm.allSGPs){
					if(sgp.Filter is SGNullFilter || sgp.Filter is SGCGearsFilter){
						foreach(Slottable sbp in sgp){
							if(sbp != null){
								if(sbp.itemInst is CarriedGearInstanceMock){
									if(checkedList.Contains((CarriedGearInstanceMock)sbp.itemInst)){
										if(sgp == ssm.focusedSGP){
											if(sgp.isAutoSort){
												ASSB(sbp,
													null, SBDefocused, null,
													null, SBWFA, null, false,
													SBUnequipped, SBEquipped, typeof(SBEquipProcess),
													null, null, null);
											}else{
												ASSB(sbp,
													null, SBFocused, null,
													null, SBWFA, null, false,
													SBUnequipped, SBEquipped, typeof(SBEquipProcess),
													null, null, null);
											}
										}else{
											ASSB(sbp,
												null, SBDefocused, null,
												null, SBWFA, null, false,
												SBUnequipped, SBEquipped, typeof(SBEquipProcess),
												null, null, null);
										}
									}else{	/* deemed not equipped */
										if(sgp == ssm.focusedSGP){
											ASSB(sbp,
												null, SBFocused, null,
												null, SBWFA, null, false,
												null, SBUnequipped, null,
												null, null, null);
										}else{
											ASSB(sbp,
												null, SBDefocused, null,
												null, SBWFA, null, false,
												null, SBUnequipped, null,
												null, null, null);
										}
									}
								}
							}
						}
					}
				}
			}
			public void AssertFocused(){
				if(ssm.equipBundle.isToggledOn)
					AssertEquippedOnAll();
				ssm.PerformInHierarchy(AssertFocusedInner);
				}
				public void AssertFocusedInner(SlotSystemElement element){
					if(element is SlotSystemManager){
						ASSSM((SlotSystemManager)element,
							null, null, null, null, null, null, null,
							null, SSMFocused, null,
							null, SSMWFA, null,
							null, true, true, true, true);
					}else if(element.isPageElement){
						if(element is SlotGroup)
							ASGReset((SlotGroup)element);
						SlotSystemPage parentPage = (SlotSystemPage)element.parent;
						if(parentPage.isFocusedInHierarchy){
							SlotSystemPageElement pageElement = parentPage.GetPageElement(element);
							if(pageElement.isFocusToggleOn){
								/*	deemed focused	*/
								AB(element.isFocused, true);
								if(element is SlotGroup)
									ASSG((SlotGroup)element,
										null, SGFocused, null,
										null, SGWFA, null, false);
							}else{
								/*	deemed defocused */
								AB(element.isDefocused, true);
								if(element is SlotGroup)
									ASSG((SlotGroup)element,
										null, SGDefocused, null,
										null, SGWFA, null, false);
							}
						}else{
							AssertDefocusedSelfAndBelow(parentPage);
						}
					}
					if(element.isBundleElement){
						if(element is SlotGroup)
							ASGReset((SlotGroup)element);
						SlotSystemBundle parentBundle = (SlotSystemBundle)element.parent;
						if(parentBundle.isFocusedInHierarchy){
							if(parentBundle.focusedElement == element){
								/*	deemed focused	*/
								AB(element.isFocused, true);
								if(element is SlotGroup)
									ASSG((SlotGroup)element,
										null, SGFocused, null,
										null, SGWFA, null, false);
							}else{
								AB(element.isDefocused, true);
								/*	defocused	*/
								if(element is SlotGroup)
									ASSG((SlotGroup)element,
										null, SGDefocused, null,
										null, SGWFA, null, false);
							}
						}else{
							AssertDefocusedSelfAndBelow(parentBundle);
						}
					}
					if(element is Slottable){
						Slottable sb = (Slottable)element;
						ASBReset(sb);
						if(sb.sg.isFocusedInHierarchy){
							bool isFilteredIn = false;
							ssm.PrePickFilter(sb, out isFilteredIn);
							if(isFilteredIn)
								ASSB_s(sb, SBFocused, SBWFA);
							else
								ASSB_s(sb, SBDefocused, SBWFA);
						}else{
							ASSB_s(sb, SBDefocused, SBWFA);
						}
					}
				}
	/*	shortcut	*/
		/*	Debug	*/
			public void PrintSBsArray(IEnumerable<Slottable> sbs){
				foreach(Slottable sb in sbs){
					if(sb != null)
						Util.Stack(Util.SBName(sb));
					else
						Util.Stack("null");
				}
				string str = Util.Stacked;
				Debug.Log("SBs: " + str);
			}
			public void PrintItemsArray(IEnumerable<SlottableItem> items){
				foreach(SlottableItem item in items){
					if(item is InventoryItemInstanceMock){
						InventoryItemInstanceMock itemInst = (InventoryItemInstanceMock)item;
						if(itemInst != null)
							Util.Stack(Util.ItemInstName(itemInst));
						else
							Util.Stack("null");
					}
				}
				string str = Util.Stacked;
				Debug.Log("itemInsts: " + str);
			}
			public void PrintSBs(List<Slottable> sbs){
				foreach(Slottable sb in sbs){
					Debug.Log(Util.SBDebug(sb));
				}
			}
			public void Print(Slottable sb){
				Debug.Log(Util.SBDebug(sb));
			}
			public void Print(SlotGroup sg){
				Debug.Log(Util.SGDebug(sg));
			}
			public void Print(SlotSystemManager ssm){
				Debug.Log(Util.SSMDebug(ssm));
			}
			public string Name(Slottable sb){
				return Util.SBName(sb);
			}
			public string Bold(string str){
				return Util.Bold(str);
			}
			public string Red(string str){
				return Util.Red(str);
			}
			public string Blue(string str){
				return Util.Blue(str);
			}
			public string Green(string str){
				return Util.Green(str);
			}
			public string Ciel(string str){
				return Util.Ciel(str);
			}
			public string Aqua(string str){
				return Util.Aqua(str);
			}
			public string Forest(string str){
				return Util.Forest(str);
			}
			public string Brown(string str){
				return Util.Brown(str);
			}
			public string Terra(string str){
				return Util.Terra(str);
			}
			public string Berry(string str){
				return Util.Berry(str);
			}
			public string Violet(string str){
				return Util.Violet(str);
			}

		/*	SSM	*/
			SSMSelState SSMDeactivated{
				get{return SlotSystemManager.ssmDeactivatedState;}
			}
			SSMSelState SSMDefocused{
				get{return SlotSystemManager.ssmDefocusedState;}
			}
			SSMSelState SSMFocused{
				get{return SlotSystemManager.ssmFocusedState;}
			}
			SSMActState SSMWFA{
				get{return SlotSystemManager.ssmWaitForActionState;}
			}
			SSMActState SSMProbing{
				get{return SlotSystemManager.ssmProbingState;}
			}
			SSMActState SSMTransaction{
				get{return SlotSystemManager.ssmTransactionState;}
			}
		/*	SG	*/
			SGSelState SGFocused{
				get{return SlotGroup.sgFocusedState;}
			}
			SGSelState SGDefocused{
				get{return SlotGroup.sgDefocusedState;}
			}
			SGSelState SGDeactivated{
				get{return SlotGroup.sgDeactivatedState;}
			}
			SGSelState SGSelected{
				get{return SlotGroup.sgSelectedState;}
			}
			SGActState SGWFA{
				get{return SlotGroup.sgWaitForActionState;}
			}
			SGActState SGRevert{
				get{return SlotGroup.revertState;}
			}
			SGActState SGReorder{
				get{return SlotGroup.reorderState;}
			}
			SGActState SGFill{
				get{return SlotGroup.fillState;}
			}
			SGActState SGSwap{
				get{return SlotGroup.swapState;}
			}
			SGActState SGAdd{
				get{return SlotGroup.addState;}
			}
			SGActState SGRemove{
				get{return SlotGroup.removeState;}
			}
		/*	SB states shortcut	*/
			SBSelState SBFocused{
				get{return Slottable.sbFocusedState;}
			}
			SBSelState SBDefocused{
				get{return Slottable.sbDefocusedState;}
			}
			SBSelState SBDeactivated{
				get{return Slottable.sbDeactivatedState;}
			}
			SBSelState SBSelected{
				get{return Slottable.sbSelectedState;}
			}
			SBActState SBWFA{
				get{return Slottable.sbWaitForActionState;}
			}
			SBActState SBWFPickUp{
				get{return Slottable.waitForPickUpState;}
			}
			SBActState SBWFPointerUp{
				get{return Slottable.waitForPointerUpState;}
			}
			SBActState SBPickedUp{
				get{return Slottable.pickedUpState;}
			}
			SBActState SBWFNT{
				get{return Slottable.waitForNextTouchState;}
			}
			SBActState SBAdd{
				get{return Slottable.addedState;}
			}
			SBActState SBRemove{
				get{return Slottable.removedState;}
			}
			SBActState SBMoveWithin{
				get{return Slottable.moveWithinState;}
			}
			SBEqpState SBEquipped{
				get{return Slottable.equippedState;}
			}
			SBEqpState SBUnequipped{
				get{return Slottable.unequippedState;}
			}
			SBMrkState SBMarked{
				get{return Slottable.markedState;}
			}
			SBMrkState SBUnmarked{
				get{return Slottable.unmarkedState;}
			}

}
