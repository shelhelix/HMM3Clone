using System.Collections.Generic;
using Hmm3Clone.Utils;

namespace Hmm3Clone {
	public class ActiveData : Singleton<ActiveData> {
		List<object> _objects = new List<object>();

		public void SetData<T>(T obj) where T : class {
			_objects.RemoveAll(x => x is T);
			_objects.Add(obj);
		}

		public T GetData<T>() where T : class {
			return _objects.Find(x => x is T) as T;
		}

		public void RemoveData<T>() where T : class {
			_objects.RemoveAll(x => x is T);
		}
	}
}