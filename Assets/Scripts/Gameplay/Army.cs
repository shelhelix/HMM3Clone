using System;
using System.Collections;
using System.Collections.Generic;
using Hmm3Clone.State;
using UnityEngine.Assertions;

namespace Hmm3Clone.Gameplay {
	public class Army : IEnumerable<UnitStack> {
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
		
		public bool TryMergeWithOtherArmy(Army otherArmy) {
			if (!CanMergeWithOtherArmy(otherArmy)) {
				return false;
			}
			foreach (var stack in otherArmy._stacks) {
				if (stack == null) {
					continue;
				}
				var ourStack = GetOrCreateUnitStack(stack.Type);
				Assert.IsNotNull(ourStack);
				ourStack.Amount += stack.Amount;
			}
			otherArmy.FreeAllStacks();
			return true;
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

		bool CanMergeWithOtherArmy(Army otherArmy) {
			Assert.IsNotNull(otherArmy);
			var allTypes = new HashSet<UnitType>();
			foreach (var stack in _stacks) {
				if (stack == null) {
					continue;
				}
				allTypes.Add(stack.Type);
			}
			foreach (var stack in otherArmy._stacks) {
				if (stack == null) {
					continue;
				}
				allTypes.Add(stack.Type);
			}

			return allTypes.Count <= ArmyLenght;
		}

		void FreeAllStacks() {
			for (var stackIndex = 0; stackIndex < _stacks.Length; stackIndex++) {
				_stacks[stackIndex] = null;
			}
		}
		
		void RemoveStack(int stackIndex) {
			_stacks[stackIndex] = null;
		} 

		bool AreMergeableStacks(UnitStack source, UnitStack dest) {
			return source != null && dest != null && source.Type == dest.Type;
		}

		public IEnumerator<UnitStack> GetEnumerator() {
			return (_stacks as IEnumerable<UnitStack>).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}