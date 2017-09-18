using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
namespace UISystem{
	public interface IHoverIcon{
		ISlottableItem Item();
		int ItemQuantity();
		void WaitForIncrement();
		void GetReadyForIncrement();
		void Dehover();
		void Hover();
	}
	public class HoverIcon : IHoverIcon {
		public HoverIcon( ISlottableItem item){
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

		public int ItemQuantity(){
			return Item().Quantity();
		}


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
