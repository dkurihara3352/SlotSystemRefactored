using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UISystem{
	public interface ISGConstructorArg{
		ISSEEventCommandsRepo SSEEventCommandsRepo();
		IFetchInventoryCommand FetchInventoryCommand();
		IAcceptsItemCommand AcceptsItemCommand();
		IPositionSlotsCommand PositionSlotsCommand();
		ISGSorter InitSorter();
		int InitSlotCount();
		bool IsReorderable();
	}
}
