using System.Collections.Generic;

namespace Hmm3Clone.Utils {
	public static class ZipOperator {
		public static IEnumerable<(T, U)> MyZip<T, U>(this IEnumerable<T> first, IEnumerable<U> second)
		{
			using var firstEnumerator  = first.GetEnumerator();
			using var secondEnumerator = second.GetEnumerator();

			while (firstEnumerator.MoveNext()) {
				if (secondEnumerator.MoveNext()) {
					yield return (firstEnumerator.Current, secondEnumerator.Current);
				}
				else {
					yield return (firstEnumerator.Current, default);
				}
			}
			while (secondEnumerator.MoveNext()) {
				yield return (default, secondEnumerator.Current);
			}
		}
	}
}