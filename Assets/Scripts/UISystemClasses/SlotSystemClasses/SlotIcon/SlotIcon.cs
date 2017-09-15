using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace UISystem{
	public interface ISlotIcon{
		ISlottableItem Item();
		void WaitForIncrement();
		void GetReadyForIncrement();
		void Dehover();
		void Hover();
	}
	public class SlotIcon : ISlotIcon {
		public SlotIcon( ISlottableItem item){
			SetItem(item);
			SetIncrementStateEngine( new IconIncrementStateEngine( this));
		}
		public ISlottableItem Item(){
			return _item;
		}
		void SetItem( ISlottableItem item){
			_item = item;
		}
		ISlottableItem _item;


		IconIncrementStateEngine IncrementStateEngine(){
			return _incrementStateEngine;
		}
		void SetIncrementStateEngine( IconIncrementStateEngine engine){
			_incrementStateEngine = engine;
		}
		IconIncrementStateEngine _incrementStateEngine;

		public void WaitForIncrement(){
			IncrementStateEngine().WaitForIncrement();
		}
		public void GetReadyForIncrement(){
			IncrementStateEngine().GetReadyForIncrement();
		}


		IconHoverStateEngine HoverStateEngine(){
			return _hoverStateEngine;
		}
		void SetHoverStateEngine( IconHoverStateEngine engine){
			_hoverStateEngine = engine;
		}
		IconHoverStateEngine _hoverStateEngine;

		public void Dehover(){
			HoverStateEngine().Dehover();
		}
		public void Hover(){
			HoverStateEngine().Hover();
		}

		
	}
}
