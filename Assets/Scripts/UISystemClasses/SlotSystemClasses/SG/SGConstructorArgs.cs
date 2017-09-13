using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public interface ISGConstructorArg{
		IUISelStateRepo UISelStateRepo();
		ITapCommand TapCommand();
		ISSEEventCommandsRepo SSEEventCommandsRepo();
		IFetchInventoryCommand FetchInventoryCommand();
		IAcceptsItemCommand AcceptsItemCommand();
		IPositionSBsCommand PositionSBsCommand();
		ISGSorter InitSorter();
		int InitSlotCount();
		bool IsReorderable();
		bool IsExchangedOverFilled();
		bool IsExchangedOverReordered();
	}
}
