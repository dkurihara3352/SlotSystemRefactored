using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class TransactionCache : ITransactionCache {
		ITransactionManager tam;
		public TransactionCache(ITransactionManager tam){
			this.tam = tam;
		}
		public virtual void UpdateFields(){
			updateFieldsCommand.Execute();
		}
		public TransactionCacheCommand updateFieldsCommand{
			get{
				if(m_updateFieldsCommand == null)
					m_updateFieldsCommand = new UpdateFieldsCommand(this);
				return m_updateFieldsCommand;
			}
		}
			TransactionCacheCommand m_updateFieldsCommand;
			public void SetUpdateFieldsCommand(TransactionCacheCommand comm){
				m_updateFieldsCommand = comm;
			}		
		public virtual void CreateTransactionResults(){
			Dictionary<ISlotSystemElement, ISlotSystemTransaction> result = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
			foreach(ISlotGroup sg in tam.focusedSGs){
				ISlotSystemTransaction ta = MakeTransaction(pickedSB, sg);
				result.Add(sg, ta);
				if(ta is IRevertTransaction)
					sg.DefocusSelf();
				else
					sg.FocusSelf();
				foreach(ISlottable sb in sg){
					if(sb != null){
						ISlotSystemTransaction ta2 = MakeTransaction(pickedSB, sb);
						result.Add(sb, ta2);
						if(ta2 is IRevertTransaction || ta2 is IFillTransaction)
							sb.Defocus();
						else
							sb.Focus();
					}
				}
			}
			SetTransactionResults(result);
		}
		public bool IsTransactionGoingToBeRevert(ISlottable sb){
			bool res = true;
			foreach(ISlotGroup targetSG in tam.focusedSGs){
				ISlotSystemTransaction ta = MakeTransaction(sb, targetSG);
				if(ta == null)
					throw new System.InvalidOperationException("SlotSystemManager.PrePickFilter: given hoveredSSE does not yield any transaction. something's wrong baby");
				else{
					if(!(ta is IRevertTransaction)){
						res = false; break;
					}
				}
				foreach(ISlottable targetSB in targetSG){
					if(sb != null)
						if(!(MakeTransaction(sb, targetSB) is IRevertTransaction)){
							res = false; break;
						}
				}
			}
			return res;
		}
		public bool IsCachedTAResultRevert(ISlotSystemElement sse){
			if(transactionResults != null){
				ISlotSystemTransaction ta = null;
				if(transactionResults.TryGetValue(sse, out ta)){
					if(ta is IRevertTransaction)
						return true;
					else
						return false;
				}
				else
					throw new System.InvalidOperationException("Transaction not found");
			}else
				throw new System.InvalidOperationException("TransactionResults not set");
		}		
		public ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, ISlotSystemElement hovered){
			return taFactory.MakeTransaction(pickedSB, hovered);
		}
		public ITransactionFactory taFactory{
			get{
				if(m_taFactory == null)
					m_taFactory = new TransactionFactory();
				return m_taFactory;
			}
		}
			ITransactionFactory m_taFactory;
			public void SetTAFactory(ITransactionFactory taFac){m_taFactory = taFac;}
		public Dictionary<ISlotSystemElement, ISlotSystemTransaction> transactionResults{
			get{
					if(m_transactionResults == null)
						m_transactionResults = new Dictionary<ISlotSystemElement, ISlotSystemTransaction>();
					return m_transactionResults;
			}
		}
			Dictionary<ISlotSystemElement, ISlotSystemTransaction> m_transactionResults;
			public void SetTransactionResults(Dictionary<ISlotSystemElement, ISlotSystemTransaction> results){
				m_transactionResults = results;
			}
		public virtual ISlottable pickedSB{
			get{
				return m_pickedSB;
			}
		}
				ISlottable m_pickedSB;
				public virtual void SetPickedSB(ISlottable sb){
					this.m_pickedSB = sb;
				}

		public ISlottable targetSB{
			get{return m_targetSB;
			}
		}
			ISlottable m_targetSB;
			public void SetTargetSB(ISlottable sb){
				if(sb == null || sb != targetSB){
					if(targetSB != null)
						targetSB.Focus();
					this.m_targetSB = sb;
					if(targetSB != null)
						targetSB.Select();
				}
			}
		public ISlotSystemElement hovered{
			get{return m_hovered;
			}
		}
			protected ISlotSystemElement m_hovered;
			public virtual void SetHovered(ISlotSystemElement to){
				if(to == null || to != hovered){
					if(to != null && hovered != null){
						hovered.OnHoverExit();
					}
					m_hovered = to;
					if(hovered != null)
						UpdateFields();
				}
			}
		public virtual List<InventoryItemInstance> moved{
			get{
				return m_moved;
			}
		}
			List<InventoryItemInstance> m_moved;
			public virtual void SetMoved(List<InventoryItemInstance> moved){
				this.m_moved = moved;
			}
		public void ClearFields(){
			SetPickedSB(null);
			SetTargetSB(null);
			SetHovered(null);
		}
		public void InnerUpdateFieldsOfTAM(ISlotSystemTransaction ta){
			tam.InnerUpdateFields(ta);
		}
	}
	public interface ITransactionCache{
		void UpdateFields();
		void ClearFields();
		bool IsTransactionGoingToBeRevert(ISlottable sb);
		bool IsCachedTAResultRevert(IHoverable hoverable);
		void CreateTransactionResults();
		Dictionary<IHoverable, ISlotSystemTransaction> transactionResults{get;}
		ITransactionFactory taFactory{get;}
		ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, IHoverable hovered);
		ISlottable pickedSB{get;}
		void SetPickedSB(ISlottable sb);
		ISlottable targetSB{get;}
		void SetTargetSB(ISlottable sb);
		IHoverable hovered{get;}
		void SetHovered(IHoverable to);
		List<InventoryItemInstance> moved{get;}
		void SetMoved(List<InventoryItemInstance> moved);
		void InnerUpdateFieldsOfTAM(ISlotSystemTransaction ta);
	}
	public interface TransactionCacheCommand{
		void Execute();
	}
	public class UpdateFieldsCommand: TransactionCacheCommand{
		ITransactionCache taCache;
		public UpdateFieldsCommand(ITransactionCache taCache){
			this.taCache = taCache;
		}
		public void Execute(){
			if(taCache.transactionResults != null){
				ISlotSystemTransaction ta = null;
				if(taCache.transactionResults.TryGetValue(taCache.hovered, out ta)){
					taCache.SetTargetSB(ta.targetSB);
					taCache.SetMoved(ta.moved);
					taCache.InnerUpdateFieldsOfTAM(ta);
				}
			}
		}
	}
}

