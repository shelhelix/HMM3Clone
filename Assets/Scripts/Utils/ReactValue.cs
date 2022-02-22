using System;

namespace Hmm3Clone.Utils {
	public class ReactValue<T> {
		T _value;

		public T Value {
			get => _value;
			set {
				_value = value;
				OnValueChanged?.Invoke(value);
			}
		}
		
		public event Action<T> OnValueChanged;
		
		public static implicit operator T(ReactValue<T> val) {
			return val.Value;
		}
	}
}