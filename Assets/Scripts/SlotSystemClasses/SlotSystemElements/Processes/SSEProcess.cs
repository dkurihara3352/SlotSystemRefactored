using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public interface SSEProcess{
		bool isRunning{get;}
		System.Func<IEnumeratorFake> coroutineFake{set;}
		void Start();
		void Stop();
		void Expire();
	}
}