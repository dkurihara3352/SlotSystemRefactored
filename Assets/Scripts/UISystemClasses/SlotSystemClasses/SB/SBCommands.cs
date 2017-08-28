using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem{
	public interface ISBCommand{
		void Execute();
	}
	public abstract class SBCommand: ISBCommand{
		protected ISlottable sb;
		public SBCommand(ISlottable sb){
			this.sb = sb;
		}
		public abstract void Execute();
	}
	public class SBTapCommand: SBCommand{
		public SBTapCommand(ISlottable sb) :base(sb){
		}
		public override void Execute(){
		}
	}
	public class SBPickUpCommand: SBCommand{
		public SBPickUpCommand(ISlottable sb): base(sb){}
		public override void Execute(){}
	}
	public class SBPickUpEquipCommand: SBPickUpCommand{
		public SBPickUpEquipCommand(ISlottable sb): base(sb){
		}
		public override void Execute(){
		}
	}
	public class SBPickUpEquipBowCommand: SBPickUpEquipCommand{
		public SBPickUpEquipBowCommand(ISlottable sb): base(sb){}
		public override void Execute(){}
	}
	public class SBPickUpEquipWearCommand: SBPickUpEquipCommand{
		public SBPickUpEquipWearCommand(ISlottable sb): base(sb){}
		public override void Execute(){}
	}
	public class SBPickUpEquipCGearsCommand: SBPickUpEquipCommand{
		public SBPickUpEquipCGearsCommand(ISlottable sb): base(sb){}
		public override void Execute(){}
	}
	public class SBPickUpEquipPartsCommand: SBPickUpEquipCommand{
		public SBPickUpEquipPartsCommand(ISlottable sb): base(sb){}
		public override void Execute(){}
	}
}
