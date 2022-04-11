using UnityEngine;

namespace Hmm3Clone.Utils {
	public class VectorUtils {
		public static Vector3Int StringToVector3(string sVector)
		{
			// Remove the parentheses
			if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
				sVector = sVector.Substring(1, sVector.Length -2);
			}
 
			// split the items
			string[] sArray = sVector.Split(',');
 
			// store as a Vector3
			Vector3Int result = new Vector3Int(
				int.Parse(sArray[0]),
				int.Parse(sArray[1]),
				int.Parse(sArray[2]));
 
			return result;
		}
	}
}