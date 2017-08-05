using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SlotSystem{
	public class TransactionCache : ITransactionCache {
		IFocusedSGProvider focusedSGProvider{
			get{
				if(_focusedSGProvider != null)
					return _focusedSGProvider;
				else
					throw new System.InvalidOperationException("focusedSGProvider not set");
			}
		}
			IFocusedSGProvider _focusedSGProvider;
		ITransactionManager tam{
			get{
				if(_tam != null)
					return _tam;
				else
					throw new System.InvalidOperationException("tam not set");
			}
		}
			ITransactionManager _tam;
		public TransactionCache(ITransactionManager tam, IFocusedSGProvider focusedSGProvider){
			_tam = tam;
			_focusedSGProvider = focusedSGProvider;
		}
		public void UpdateFields(){
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
		public void CreateTransactionResults(){
			Dictionary<IHoverable, ISlotSystemTransaction> result = new Dictionary<IHoverable, ISlotSystemTransaction>();
			foreach(ISlotGroup sg in focusedSGProvider.focusedSGs){
				ISlotSystemTransaction ta = MakeTransaction(pickedSB, sg.hoverable);
				result.Add(sg.hoverable, ta);
				if(ta is IRevertTransaction)
					sg.Defocus();
				else
					sg.Focus();
				foreach(ISlottable sb in sg){
					if(sb != null){
						ISlotSystemTransaction ta2 = MakeTransaction(pickedSB, sb.hoverable);
						result.Add(sb.hoverable, ta2);
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
			foreach(ISlotGroup targetSG in focusedSGProvider.focusedSGs){
				ISlotSystemTransaction ta = MakeTransaction(sb, targetSG.hoverable);
				if(ta == null)
					throw new System.InvalidOperationException("SlotSystemManager.PrePickFilter: given hoveredSSE does not yield any transaction. something's wrong baby");
				else{
					if(!(ta is IRevertTransaction)){
						res = false; break;
					}
				}
				foreach(ISlottable targetSB in targetSG){
					if(sb != null)
						if(!(MakeTransaction(sb, targetSB.hoverable) is IRevertTransaction)){
							res = false; break;
						}
				}
			}
			return res;
		}
		public bool IsCachedTAResultRevert(IHoverable hoverable){
			if(transactionResults != null){
				ISlotSystemTransaction ta = null;
				if(transactionResults.TryGetValue(hoverable, out ta)){
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
		public ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, IHoverable hovered){
			return taFactory.MakeTransaction(pickedSB, hovered);
		}
		public ITransactionFactory taFactory{
			get{return tam.taFactory;}
		}
		public Dictionary<IHoverable, ISlotSystemTransaction> transactionResults{
			get{
					if(m_transactionResults == null)
						m_transactionResults = new Dictionary<IHoverable, ISlotSystemTransaction>();
					return m_transactionResults;
			}
		}
			Dictionary<IHoverable, ISlotSystemTransaction> m_transactionResults;
			public void SetTransactionResults(Dictionary<IHoverable, ISlotSystemTransaction> results){
				m_transactionResults = results;
			}
		public ISlottable pickedSB{
			get{
				return m_pickedSB;
			}
		}
				ISlottable m_pickedSB;
				public void SetPickedSB(ISlottable sb){
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
		public IHoverable hovered{
			get{return m_hovered;
			}
		}
			protected IHoverable m_hovered;
			public void SetHovered(IHoverable to){
				if(to == null || to != hovered){
					if(to != null && hovered != null){
						hovered.OnHoverExit();
					}
					m_hovered = to;
					if(hovered != null)
						UpdateFields();
				}
			}
		public List<InventoryItemInstance> moved{
			get{
				return m_moved;
			}
		}
			List<InventoryItemInstance> m_moved;
			public void SetMoved(List<InventoryItemInstance> moved){
				this.m_moved = moved;
			}
		public void ClearFields(){
			SetPickedSB(null);
			SetTargetSB(null);
			SetHovered(null);
		}
		public void UpdateFieldsOfTAM(ISlotSystemTransaction ta){
			tam.UpdateFields(ta);
		}
	}
	public interface ITransactionCache{
		void UpdateFields();
		void ClearFields();
		bool IsTransactionGoingToBeRevert(ISlottable sb);
		bool IsCachedTAResultRevert(IHoverable hoverable);
		void CreateTransactionResults();
		Dictionary<IHoverable, ISlotSystemTransaction> transactionResults{get;}
		ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, IHoverable hovered);
		ISlottable pickedSB{get;}
		void SetPickedSB(ISlottable sb);
		ISlottable targetSB{get;}
		void SetTargetSB(ISlottable sb);
		IHoverable hovered{get;}
		void SetHovered(IHoverable to);
		List<InventoryItemInstance> moved{get;}
		void SetMoved(List<InventoryItemInstance> moved);
		void UpdateFieldsOfTAM(ISlotSystemTransaction ta);
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
					taCache.UpdateFieldsOfTAM(ta);
				}
			}
		}
	}
}

