using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public class Slot{
		Slottable m_sb;
		public Slottable sb{
			get{return m_sb;}
			set{m_sb = value;}
		}
		Vector2 m_position;
		public Vector2 Position{
			get{return m_position;}
			set{m_position = value;}
		}
	}
}