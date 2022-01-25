using Hmm3Clone.State;
using UnityEngine.Assertions;

namespace Hmm3Clone.Gameplay {
	public class Army {
		UnitStack[] _stacks;
		
		public int ArmyLenght => _stacks.Length;

		public Army(UnitStack[] stacks) {
			_stacks = stacks;
		}

		public bool IsStackEmpty(int stackIndex) {
			Assert.IsTrue(stackIndex >= 0);
			return _stacks[stackIndex] == null;
		}

		public UnitStack GetStack(int stackIndex) {
			Assert.IsTrue(stackIndex >= 0);
			return _stacks[stackIndex];
		}

		public bool AreMergeableStacks(int stackStartIndex, int stackEndIndex) {
			Assert.IsTrue(stackStartIndex >= 0);
			Assert.IsTrue(stackEndIndex   >= 0);
			var sourceStack = GetStack(stackStartIndex);
			var dstStack    = GetStack(stackEndIndex);
			return (sourceStack != null) && (dstStack != null) && sourceStack.Type == dstStack.Type;
		}
		
		public void SwapStack(int stackStartIndex, int stackEndIndex) {
			(_stacks[stackEndIndex], _stacks[stackStartIndex]) = (_stacks[stackStartIndex], _stacks[stackEndIndex]);
		}
		
		public void MergeStack(int stackFromSplitIndex, int stackToSplitIndex) {
			if (!AreMergeableStacks(stackFromSplitIndex, stackToSplitIndex)) {
				return;
			}
			Assert.IsNotNull(_stacks[stackToSplitIndex]);
			Assert.IsNotNull(_stacks[stackFromSplitIndex]);
			var sourceStack = _stacks[stackFromSplitIndex];
			var dstStack    = _stacks[stackToSplitIndex];
			dstStack.Amount              += sourceStack.Amount;
			_stacks[stackFromSplitIndex] =  null;
		}
		
		public void SplitStack(int stackFromSplitIndex, int stackToSplitIndex) {
			Assert.IsNull(_stacks[stackToSplitIndex]);
			Assert.IsNotNull(_stacks[stackFromSplitIndex]);
			var sourceStack         = _stacks[stackFromSplitIndex];
			var splittedStackAmount = sourceStack.Amount / 2;
			if (splittedStackAmount == 0) {
				return;
			}
			_stacks[stackToSplitIndex] =  new UnitStack(sourceStack.Type, splittedStackAmount);
			sourceStack.Amount         -= _stacks[stackToSplitIndex].Amount;
		}
	}
}