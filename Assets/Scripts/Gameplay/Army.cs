using System;
using Hmm3Clone.State;
using UnityEngine.Assertions;

namespace Hmm3Clone.Gameplay {
	public class Army {
		public const int InvalidStackIndex = -1;
		
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

		public bool AreMergeableStacks(int sourceStackIndex, Army otherArmy, int destStackIndex) {
			Assert.IsNotNull(otherArmy);
			var sourceStack = GetStack(sourceStackIndex);
			var destStack   = otherArmy.GetStack(destStackIndex);
			return AreMergeableStacks(sourceStack, destStack);
		}

		public void SwapStacks(int sourceStackIndex, Army otherArmy, int destStackIndex) {
			Assert.IsNotNull(otherArmy);
			(_stacks[sourceStackIndex], otherArmy._stacks[destStackIndex]) = (otherArmy._stacks[destStackIndex], _stacks[sourceStackIndex]);
		}

		public UnitStack FindStackWithUnits(UnitType unitType) {
			foreach (var unitStack in _stacks) {
				if (unitStack != null && unitStack.Type == unitType) {
					return unitStack;
				}
			}
			return null;
		}
		
		public int GetFreeStackIndex() {
			for (var i = 0; i < _stacks.Length; i++) {
				if (_stacks[i] == null) {
					return i;
				}
			}
			return InvalidStackIndex;
		}

		public UnitStack GetOrCreateUnitStack(UnitType unitType) {
			var existedStack = FindStackWithUnits(unitType);
			if (existedStack != null) {
				return existedStack;
			}

			var freeStackIndex = GetFreeStackIndex();
			if (freeStackIndex == InvalidStackIndex) {
				return null;
			}
			var res = new UnitStack { Type = unitType};
			_stacks[freeStackIndex] = res;
			return res;
		}

		public void MergeStack(int sourceStackIndex, Army otherArmy, int destStackIndex) {
			Assert.IsNotNull(otherArmy);
			var sourceStack = GetStack(sourceStackIndex);
			var destStack   = otherArmy.GetStack(destStackIndex);
			Assert.IsNotNull(sourceStack);
			Assert.IsNotNull(destStack);
			if (!AreMergeableStacks(sourceStack, destStack)) {
				return;
			}
			destStack.Amount += sourceStack.Amount;
			RemoveStack(sourceStackIndex);
		}
		
		public void SplitStack(int stackFromSplitIndex, int stackToSplitIndex) {
			SplitStack(stackFromSplitIndex, this, stackToSplitIndex);
		}

		public void SplitStack(int sourceStackIndex, Army otherArmy, int destStackIndex) {
			Assert.IsNotNull(otherArmy);
			var sourceStack = GetStack(sourceStackIndex);
			var destStack   = otherArmy.GetStack(destStackIndex);
			Assert.IsNotNull(sourceStack);
			Assert.IsNull(destStack);
			var splittedStackAmount = sourceStack.Amount / 2;
			if (splittedStackAmount == 0) {
				return;
			}
			otherArmy._stacks[destStackIndex] =  new UnitStack(sourceStack.Type, splittedStackAmount);
			sourceStack.Amount                -= otherArmy._stacks[destStackIndex].Amount;
		}

		void RemoveStack(int stackIndex) {
			_stacks[stackIndex] = null;
		} 

		bool AreMergeableStacks(UnitStack source, UnitStack dest) {
			return source != null && dest != null && source.Type == dest.Type;
		}
	}
}