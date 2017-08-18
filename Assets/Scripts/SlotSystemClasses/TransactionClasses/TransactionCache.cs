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
				if(_updateFieldsCommand == null)
					_updateFieldsCommand = new UpdateFieldsCommand(this);
				return _updateFieldsCommand;
			}
		}
			TransactionCacheCommand _updateFieldsCommand;
			public void SetUpdateFieldsCommand(TransactionCacheCommand comm){
				_updateFieldsCommand = comm;
			}		
		public void CreateTransactionResults(){
			Dictionary<IHoverable, ISlotSystemTransaction> result = new Dictionary<IHoverable, ISlotSystemTransaction>();
			foreach(ISlotGroup sg in focusedSGProvider.GetFocusedSGs()){
				ISlottable pickedSB = GetPickedSB();
				ISlotSystemTransaction ta = MakeTransaction(pickedSB, sg);
				ISSESelStateHandler sgSelStateHandler = sg.GetSelStateHandler();
				result.Add(sg, ta);
				if(ta is IRevertTransaction)
					sgSelStateHandler.Defocus();
				else
					sgSelStateHandler.Focus();
				foreach(ISlottable sb in sg){
					if(sb != null){
						ISlotSystemTransaction ta2 = MakeTransaction(pickedSB, sb);
						ISSESelStateHandler sbSelStateHandler = sb.GetSelStateHandler();
						result.Add(sb, ta2);
						if(ta2 is IRevertTransaction || ta2 is IFillTransaction)
							sbSelStateHandler.Defocus();
						else
							sbSelStateHandler.Focus();
					}
				}
			}
			SetTransactionResults(result);
		}
		public bool IsTransactionGoingToBeRevert(ISlottable sb){
			bool res = true;
			foreach(ISlotGroup targetSG in focusedSGProvider.GetFocusedSGs()){
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
		public bool IsCachedTAResultRevert(IHoverable hoverable){
			Dictionary<IHoverable, ISlotSystemTransaction> transactionResults = GetTransactionResults();
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
			get{return tam.GetTAFactory();}
		}
		public Dictionary<IHoverable, ISlotSystemTransaction> GetTransactionResults(){
			if(_transactionResults == null)
				_transactionResults = new Dictionary<IHoverable, ISlotSystemTransaction>();
			return _transactionResults;
		}
		public void SetTransactionResults(Dictionary<IHoverable, ISlotSystemTransaction> results){
			_transactionResults = results;
		}
			Dictionary<IHoverable, ISlotSystemTransaction> _transactionResults;
		public ISlottable GetPickedSB(){
			return _pickedSB;
		}
		public void SetPickedSB(ISlottable sb){
			this._pickedSB = sb;
		}
			ISlottable _pickedSB;

		public ISlottable GetTargetSB(){
			return _targetSB;
		}
		public void SetTargetSB(ISlottable newTargetSB){
			ISlottable curTargetSB = GetTargetSB();
			if(newTargetSB == null || newTargetSB != curTargetSB){
				if(curTargetSB != null){
					ISSESelStateHandler targetSBSelStateHandler = curTargetSB.GetSelStateHandler();
					targetSBSelStateHandler.Focus();
				}
				_targetSB = newTargetSB;
				if(newTargetSB != null){
					ISSESelStateHandler targetSBSelStateHandler = newTargetSB.GetSelStateHandler();
					targetSBSelStateHandler.Select();
				}
			}
		}
			ISlottable _targetSB;
		public IHoverable GetHovered(){
			return _hovered;
		}
		public void SetHovered(IHoverable newHovered){
			IHoverable curHovered = GetHovered();
			if(newHovered == null || newHovered != curHovered){
				if(newHovered != null && curHovered != null){
					curHovered.OnHoverExit();
				}
				_hovered = newHovered;
				if(newHovered != null)
					UpdateFields();
			}
		}
			protected IHoverable _hovered;
		public List<IInventoryItemInstance> GetMoved(){
			return _moved;
		}
		public void SetMoved(List<IInventoryItemInstance> moved){
			this._moved = moved;
		}
			List<IInventoryItemInstance> _moved;
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
		Dictionary<IHoverable, ISlotSystemTransaction> GetTransactionResults();
		ISlotSystemTransaction MakeTransaction(ISlottable pickedSB, IHoverable hovered);
		ISlottable GetPickedSB();
		void SetPickedSB(ISlottable sb);
		ISlottable GetTargetSB();
		void SetTargetSB(ISlottable sb);
		IHoverable GetHovered();
		void SetHovered(IHoverable to);
		List<IInventoryItemInstance> GetMoved();
		void SetMoved(List<IInventoryItemInstance> moved);
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
			Dictionary<IHoverable, ISlotSystemTransaction> transactionResults = taCache.GetTransactionResults();
			if(transactionResults != null){
				ISlotSystemTransaction ta = null;
				if(transactionResults.TryGetValue(taCache.GetHovered(), out ta)){
					taCache.SetTargetSB(ta.GetTargetSB());
					taCache.SetMoved(ta.GetMoved());
					taCache.UpdateFieldsOfTAM(ta);
				}
			}
		}
	}
}

