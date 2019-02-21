using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TriggersTools.CatSystem2.Scenes.Commands {
	public static class SceneCommands {
		#region Constants

		public static SceneCommandInfo Voice { get; } = new SceneCommandInfo {
			Id = "pcm",
			Create = s => new VoiceCommand(s),
		};
		public static SceneCommandInfo SceneTitle { get; } = new SceneCommandInfo {
			Id = "scene",
			Create = s => new SceneTitleCommand(s),
		};
		public static SceneCommandInfo String { get; } = new SceneCommandInfo {
			Id = "str",
			Create = s => new StringCommand(s),
		};
		public static SceneCommandInfo Condition { get; } = new SceneCommandInfo {
			Id = "if",
			Create = s => new ConditionCommand(s),
			IsCatCommand = n => n.StartsWith("if"),
			IsSpecialCommand = true,
		};
		public static SceneCommandInfo Variable { get; } = new SceneCommandInfo {
			Id = "variable",
			Create = s => new VariableCommand(s),
			IsCatCommand = n => n.StartsWith("#"),
			IsSpecialCommand = true,
		};

		#endregion

		#region Fields

		private readonly static Dictionary<string, SceneCommandInfo> entries
			= new Dictionary<string, SceneCommandInfo>();

		private readonly static List<SceneCommandInfo> specialEntries = new List<SceneCommandInfo>();

		#endregion

		#region Static Constructor

		static SceneCommands() {
			foreach (PropertyInfo prop in typeof(SceneCommands).GetProperties()) {
				if (prop.PropertyType == typeof(SceneCommandInfo) && prop.GetMethod != null) {
					SceneCommandInfo commandInfo = (SceneCommandInfo) prop.GetValue(null);
					if (commandInfo.IsSpecialCommand)
						specialEntries.Add(commandInfo);
					else
						entries.Add(commandInfo.Id, commandInfo);
				}
			}
		}

		#endregion

		#region CreateCommand

		public static ISceneCommand CreateCommand(SceneLine catString) {
			string name = catString.CommandName;
			foreach (SceneCommandInfo specialCommandInfo in specialEntries) {
				if (specialCommandInfo.IsCommand(name))
					return specialCommandInfo.Create(catString);
			}
			if (entries.TryGetValue(name, out SceneCommandInfo commandInfo))
				return commandInfo.Create(catString);
			return new SceneCommand(catString);
		}

		#endregion
	}
}
