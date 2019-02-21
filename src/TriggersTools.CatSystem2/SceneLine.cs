using System;
using System.Runtime.CompilerServices;
using TriggersTools.CatSystem2.Scenes.Commands;
using TriggersTools.CatSystem2.Structs;
using Newtonsoft.Json;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  The purpose of the string entry for a <see cref="SceneLine"/>.
	/// </summary>
	public enum SceneLineType : ushort {
		/// <summary>
		///  Waits for user input to continue the scene. The string entry for this type is empty. (513)
		/// </summary>
		Input = 0x0201,
		/// <summary>Changes the current dialog. (8193)</summary>
		Message = 0x2001,
		/// <summary>Changes the current character name. (8449)</summary>
		Name = 0x2101,
		/// <summary>Runs a command to change the scene in some way. (12289)</summary>
		Command = 0x3001,
	}
	/// <summary>
	///  A string line in a <see cref="SceneScript"/> file.
	/// </summary>
	public sealed class SceneLine {
		#region Fields

		/// <summary>
		///  Gets the scene script containing this scene line.
		/// </summary>
		[JsonIgnore]
		public SceneScript Scene { get; internal set; }

		/// <summary>
		///  Gets the purpose of the scene line.
		/// </summary>
		[JsonProperty("type")]
		public SceneLineType Type { get; private set; }
		/// <summary>
		///  Gets the string content of the scene line.
		/// </summary>
		[JsonProperty("content")]
		public string Content { get; private set; }
		/// <summary>
		///  The object associated with the scene line, such as command, message, or name.
		/// </summary>
		[JsonIgnore]
		private object obj;

		#endregion

		#region Properties

		/// <summary>
		///  Gets the unescaped message for the line if <see cref="Type"/> is <see cref="SceneLineType.Message"/>.
		/// </summary>
		/// 
		/// <exception cref="InvalidOperationException">
		///  <see cref="Type"/> is not <see cref="SceneLineType.Message"/>.
		/// </exception>
		[JsonIgnore]
		public string Message {
			get {
				ThrowIfNotType(SceneLineType.Message);
				if (obj == null)
					obj = CatUtils.UnescapeMessage(Content, false);
				return (string) obj;
			}
		}
		/// <summary>
		///  Gets the unescaped name for the line if <see cref="Type"/> is <see cref="SceneLineType.Name"/>.
		/// </summary>
		/// 
		/// <exception cref="InvalidOperationException">
		///  <see cref="Type"/> is not <see cref="SceneLineType.Name"/>.
		/// </exception>
		[JsonIgnore]
		public string Name {
			get {
				ThrowIfNotType(SceneLineType.Name);
				if (obj == null)
					obj = CatUtils.UnescapeMessage(Content, true);
				return (string) obj;
			}
		}
		/// <summary>
		///  Gets the command name of the line if <see cref="Type"/> is <see cref="SceneLineType.Command"/>.
		/// </summary>
		/// 
		/// <exception cref="InvalidOperationException">
		///  <see cref="Type"/> is not <see cref="SceneLineType.Command"/>.
		/// </exception>
		[JsonIgnore]
		public string CommandName {
			get {
				ThrowIfNotType(SceneLineType.Command);
				int index = Content.IndexOf(' ');
				if (index == -1)
					return Content;
				return Content.Substring(0, index);
			}
		}
		/// <summary>
		///  Gets the command information of the line if <see cref="Type"/> is <see cref="SceneLineType.Command"/>.
		/// </summary>
		/// 
		/// <exception cref="InvalidOperationException">
		///  <see cref="Type"/> is not <see cref="SceneLineType.Command"/>.
		/// </exception>
		[JsonIgnore]
		public ISceneCommand Command {
			get {
				ThrowIfNotType(SceneLineType.Command);
				if (obj == null)
					obj = SceneCommands.CreateCommand(this);
				return (ISceneCommand) obj;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		///  Constructs an unassigned cat string entry for use with loading via <see cref="Newtonsoft.Json"/>.
		/// </summary>
		public SceneLine() { }
		/// <summary>
		///  Constructs a cat string entry with the specified CATSTR.
		/// </summary>
		/// <param name="catStr">The CATSTR to construct this cat string from.</param>
		internal SceneLine(SCENELINE catStr, SceneScript catScene) {
			Type = (SceneLineType) catStr.Type;
			Content = catStr.Content;
			Scene = catScene;
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the cat string entry.
		/// </summary>
		/// <returns>The string representation of the cat string entry.</returns>
		public override string ToString() {
			string type = $"[{Type.ToString().ToLower()}]";
			if (!string.IsNullOrEmpty(Content))
				return $"{type} {Content}";
			return type;
		}

		#endregion

		#region Private Methods

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ThrowIfNotType(SceneLineType type) {
			if (Type != type)
				throw new InvalidOperationException($"{nameof(SceneLine)} is not of type {type}!");
		}

		#endregion
	}
}
