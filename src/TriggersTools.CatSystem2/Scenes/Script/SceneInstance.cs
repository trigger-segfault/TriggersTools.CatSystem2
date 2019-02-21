using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Scenes.Script {
	public sealed class SceneInstance {
		private readonly Dictionary<int, int> variables = new Dictionary<int, int>();
		private readonly int[] playVariables = new int[512];
		private readonly int[] globalVariables = new int[512];
		private readonly Dictionary<int, string> strings = new Dictionary<int, string>();

		public int GetVariable(int index) {
			if (index < 512)
				return playVariables[index];
			else
				return globalVariables[index];
		}
		public void SetVariable(int index, int value) {
			if (index < 512)
				playVariables[index] = value;
			else
				globalVariables[index] = value;
		}
		public string GetString(int index) {
			return strings[index];
		}
		public void SetString(int index, string value) {
			strings[index] = value;
		}
	}
}
