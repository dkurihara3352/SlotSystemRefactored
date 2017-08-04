using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotSystem{
	public static class SlotSystemUtil{
		public static bool AreSwappable(ISlottable pickedSB, ISlottable otherSB){
			/*	precondition
					1) they do not share same SG
					2) otherSB.SG accepts pickedSB
					3) not stackable
			*/
			if(pickedSB.sg != otherSB.sg){
				if(otherSB.sg.AcceptsFilter(pickedSB)){
					if(!(pickedSB.item == otherSB.item && pickedSB.item.Item.IsStackable))
						if(pickedSB.sg.AcceptsFilter(otherSB))
						return true;
				}
			}
			return false;
		}
		public static string Red(string str){
			return "<color=#ff0000>" + str + "</color>";
		}
		public static string Blue(string str){
			return "<color=#0000ff>" + str + "</color>";

		}
		public static string Green(string str){
			return "<color=#02B902>" + str + "</color>";
		}
		public static string Ciel(string str){
			return "<color=#11A795>" + str + "</color>";
		}
		public static string Aqua(string str){
			return "<color=#128582>" + str + "</color>";
		}
		public static string Forest(string str){
			return "<color=#046C57>" + str + "</color>";
		}
		public static string Brown(string str){
			return "<color=#805A05>" + str + "</color>";
		}
		public static string Terra(string str){
			return "<color=#EA650F>" + str + "</color>";
		}
		public static string Berry(string str){
			return "<color=#A41565>" + str + "</color>";
		}
		public static string Violet(string str){
			return "<color=#793DBD>" + str + "</color>";
		}
		public static string Khaki(string str){
			return "<color=#747925>" + str + "</color>";
		}
		public static string Midnight(string str){
			return "<color=#1B2768>" + str + "</color>";
		}
		public static string Beni(string str){
			return "<color=#E32791>" + str + "</color>";
		}
		public static string Sangria(string str){
			return "<color=#640A16>" + str + "</color>";
		}
		public static string Yamabuki(string str){
			return "<color=#EAB500>" + str + "</color>";
		}
		public static string Bold(string str){
			return "<b>" + str + "</b>";
		}
		static string m_stacked;
		public static string Stacked{
			get{
				string result = m_stacked;
				m_stacked = "";
				return result;
			}
		}
		public static void Stack(string str){
			m_stacked += str + ", ";
		}
	}
}