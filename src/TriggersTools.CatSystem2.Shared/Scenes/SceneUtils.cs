using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Scenes.Commands;

namespace TriggersTools.CatSystem2.Scenes {
	public static class SceneUtils {
		#region Constants

		/*public static SceneCommandInfo Voice { get; } = new SceneCommandInfo {
			Id = "pcm",
			Create = s => new SoundPlayCommand(s),
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
		};*/

		private static readonly List<ISceneCommand> commandTypes = new List<ISceneCommand>();

		static SceneUtils() {
			foreach (Type type in typeof(ISceneCommand).Assembly.GetTypes()) {
				if (typeof(ISceneCommand).IsAssignableFrom(type) && !type.IsAbstract) {
					commandTypes.Add((ISceneCommand) Activator.CreateInstance(type));
				}
			}
			commandTypes.Sort(CompareCommandPriority);
		}
		private static int CompareCommandPriority(ISceneCommand a, ISceneCommand b) {
			return b.IsCommandPriority.CompareTo(a.IsCommandPriority);
		}

		#endregion

		#region Fields

		/*private readonly static Dictionary<string, SceneCommandInfo> entries
			= new Dictionary<string, SceneCommandInfo>();

		private readonly static List<SceneCommandInfo> specialEntries = new List<SceneCommandInfo>();*/

		#endregion

		#region Static Constructor

		/*static SceneCommands() {
			foreach (PropertyInfo prop in typeof(SceneCommands).GetProperties()) {
				if (prop.PropertyType == typeof(SceneCommandInfo) && prop.GetMethod != null) {
					SceneCommandInfo commandInfo = (SceneCommandInfo) prop.GetValue(null);
					if (commandInfo.IsSpecialCommand)
						specialEntries.Add(commandInfo);
					else
						entries.Add(commandInfo.Id, commandInfo);
				}
			}
		}*/

		#endregion

		#region CreateCommand

			
		/*public static ISceneCommand CreateCommand(SceneLine catString) {
			string name = catString.CommandName;
			foreach (SceneCommandInfo specialCommandInfo in specialEntries) {
				if (specialCommandInfo.IsCommand(name))
					return specialCommandInfo.Create(catString);
			}
			if (entries.TryGetValue(name, out SceneCommandInfo commandInfo))
				return commandInfo.Create(catString);
			return new SceneCommand(catString);
		}*/
		public static ISceneCommand CreateCommand(string content) {
			string name = content;
			int index = content.IndexOf(' ');
			if (index != -1)
				name = content.Substring(0, index);
			string[] parameters = content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (ISceneCommand cmd in commandTypes) {
				if (cmd.IsCommand(name, content, parameters))
					return (ISceneCommand) Activator.CreateInstance(cmd.GetType(), content);
			}
			return null;
		}

		public static ISceneLine CreateLine(SceneLineType type, string content) {
			switch (type) {
			case SceneLineType.Input:
				return new SceneInput(content);
			case SceneLineType.Page:
				return new ScenePage(content);
			case SceneLineType.Message:
				return new SceneMessage(content);
			case SceneLineType.Name:
				return new SceneName(content);
			case SceneLineType.Command:
				return CreateCommand(content);
			}
			throw new ArgumentException(nameof(type));
		}

		#endregion

		public static bool TryParseInt(string s, out int result) {
			if (s.StartsWith("0x"))
				return int.TryParse(s.Substring(2), NumberStyles.HexNumber, null, out result);
			else if (s.StartsWith("$"))
				return int.TryParse(s.Substring(1), NumberStyles.HexNumber, null, out result);
			return int.TryParse(s, out result);
		}
	}
}
