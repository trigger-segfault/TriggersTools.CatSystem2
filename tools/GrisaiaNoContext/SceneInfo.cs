using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TriggersTools.CatSystem2;
using TriggersTools.CatSystem2.Scenes;
using TriggersTools.CatSystem2.Scenes.Commands;
using TriggersTools.CatSystem2.Scenes.Commands.Sounds;
using TriggersTools.SharpUtils.Collections;

namespace TriggersTools.CatSystem2.GrisaiaNoContext {
	public class SceneInfo {
		public KifintEntry Entry { get; }
		public CstScene Script { get; }
		public IReadOnlyList<Message> Messages { get; }
		public IReadOnlyList<Choice> Choices { get; }
		//public IReadOnlyList<NovelRange> NovelRanges { get; set; }

		public SceneInfo(KifintEntry entry, CstScene script) {
			Entry = entry;
			Script = script;
			//List<NovelRange> novelRanges = new List<NovelRange>();
			List<Message> messages = new List<Message>();
			List<Choice> choices = new List<Choice>();
			ReadScene(messages, choices);
			//ReadNovelRanges(novelRanges);
			Messages = messages.ToImmutableArrayLW();
			Choices = choices.ToImmutableArrayLW();
			//NovelRanges = novelRanges.ToImmutableArray();
		}
		
		/*private void ReadNovelRanges(List<NovelRange> novelRanges) {
			NovelCommand on = null;
			for (int i = 0; i < Script.Count; i++) {
				if (Script.Lines[i] is NovelCommand novel) {
					if (on == null && novel.State) {
						on = novel;
					}
					else if (on != null && !novel.State) {
						novelRanges.Add(new NovelRange(on, novel));
						on = null;
					}
				}
			}
			if (on != null)
				novelRanges.Add(new NovelRange(on));
		}*/

		private void ReadScene(List<Message> messages, List<Choice> choices) {
			bool mesdraw = true;
			bool novel = false;
			Message.Builder msgBuilder = new Message.Builder(this);
			for (int i = 0; i < Script.Count; i++) {
				ISceneLine line = Script.Lines[i];
				switch (line) {
				case ScenePage _:
				case SceneInput _:
					if (msgBuilder.HasMessage) {
						Message msg = msgBuilder.Build();
						if (mesdraw && msg != null)
							messages.Add(msg);
					}
					break;
				case NovelCommand novelCmd:
					novel = novelCmd.State;
					msgBuilder.IsNovel = novelCmd.State;
					break;
				case MesdrawCommand mesdrawCmd:
					mesdraw = mesdrawCmd.State;
					break;
				case SceneName name:
					msgBuilder.Names.Add(name);
					break;
				case SceneMessage message:
					msgBuilder.Messages.Add(message);
					break;
				case SoundPlayCommand play:
					if (play.SoundType == SoundType.Pcm)
						msgBuilder.Voices.Add(play);
					break;
				case ConditionCommand condition:
					if (condition.Command is SoundPlayCommand conPlay && conPlay.SoundType == SoundType.Pcm) {
						Console.WriteLine(condition.Content);
					}
					break;
				case ChoiceCommand choice:
					if (!JpUtils.Check(choice.ChoiceContent))
						choices.Add(new Choice(this, choice));
					else
						Console.WriteLine("");
					break;
				}
			}
		}
	}

	public class Choice {
		public SceneInfo Script { get; }
		public ChoiceCommand ChoiceCommand { get; }
		public Choice(SceneInfo script, ChoiceCommand choiceCommand) {
			Script = script;
			ChoiceCommand = choiceCommand;
		}

		public ChoiceCommand CreateChoice(Choice choiceToReplace) {
			ChoiceCommand newCmd = choiceToReplace.ChoiceCommand;
			ChoiceCommand oldCmd = ChoiceCommand;
			return new ChoiceCommand(newCmd.ChoiceId, newCmd.ChoiceJump, oldCmd.ChoiceContent);
		}
	}


	public class Message {
		public class Builder {
			public SceneInfo Script { get; }
			public List<SceneName> Names { get; } = new List<SceneName>();
			public List<SceneMessage> Messages { get; } = new List<SceneMessage>();
			public List<SoundPlayCommand> Voices { get; } = new List<SoundPlayCommand>();
			public bool IsNovel { get; set; }

			public bool HasMessage => Messages.Count != 0;
			
			public Builder(SceneInfo script) {
				Script = script;
			}

			public Message Build() {
				//if (!HasMessage)
				//	throw new Exception();
				var voices = Voices.ToList();
				voices.Sort(CompareVoiceIds);
				string novelId = (IsNovel ? "novel   " : string.Empty);
				string nameId = string.Join("", Names.Select(n => n.Content));
				string voiceId = string.Join(" ", voices.Select(ToVoiceId));
				string groupId = $"{novelId}{nameId}  {voiceId}";
				Message m = new Message {
					GroupId = groupId,
					Script = Script,
					Names = Names.ToImmutableArrayLW(),
					Messages = Messages.ToImmutableArrayLW(),
					Voices = Voices.ToImmutableArrayLW(),
				};
				Names.Clear();
				Messages.Clear();
				Voices.Clear();
				string msg = m.CreateMessage().Message;
				if (msg.Length == 0 || msg == "\n")
					return null;
				if (msg == new string('.', msg.Length))
					return null;
				if (JpUtils.Check(msg))
					return null;
				return m;
			}
			private static readonly char[] JpValid = {
				'「', '　', '～',
			};
			private static readonly int[] JpRanges = {
				0x3000, 0x303F,
				0x3040, 0x309F,
				0x30A0, 0x30FF,
				0xFF00, 0xFFEF,
				0x4E00, 0x9FAF,
			};

			private static int CompareVoiceIds(SoundPlayCommand a, SoundPlayCommand b) {
				return string.Compare(ToVoiceId(a), ToVoiceId(b));
			}

			private static string ToVoiceId(SoundPlayCommand cmd) {
				string snd = cmd.Sound;
				int index = snd.IndexOf('_');
				if (index != -1)
					return snd.Substring(0, index).ToLower();
				return string.Empty;
			}
		}
		private Message() {

		}
		/*public Message(SceneScript script, List<SceneName> names, List<SceneMessage> messages, List<SoundPlayCommand> voiceCommands) {
			Script = script;
			Names = names.ToImmutableArray();
			Messages = messages.ToImmutableArray();
			VoiceCommands = voiceCommands.ToImmutableArray();
		}*/
		private const string AtPattern = @"(?<!\\)(?:\\\\)*(?'at'\\@)";
		private static readonly Regex AtRegex = new Regex(AtPattern);
		private string StripAtSigns(string message) {
			StringBuilder str = new StringBuilder(message);
			MatchCollection matches = AtRegex.Matches(message);
			for (int i = matches.Count - 1; i >= 0; i--) {
				Group group = matches[i].Groups["at"];
				str.Remove(group.Index, group.Length);
			}
			return str.ToString();
		}

		public SceneName CreateName() {
			string content = string.Join("", Names.Select(n => n.Content));
			return new SceneName(content);
		}
		public SceneMessage CreateMessage() {
			string content = string.Join("", Messages.Select(m => StripAtSigns(m.Content)));
			return new SceneMessage(content);
		}
		public SoundPlayCommand[] CreateVoices() {
			SoundPlayCommand[] voices = new SoundPlayCommand[Voices.Count];
			for (int i = 0; i < voices.Length; i++)
				voices[i] = new SoundPlayCommand(Voices[i].Content);
			return voices;
		}
		public bool IsMonologue => Names.Count == 0;
		public bool IsSilent => Voices.Count == 0;

		public bool IsNovel { get; private set; }
		public string GroupId { get; private set; }
		public SceneInfo Script { get; private set; }
		public IReadOnlyList<SceneName> Names { get; private set; }
		public IReadOnlyList<SceneMessage> Messages { get; private set; }
		public IReadOnlyList<SoundPlayCommand> Voices { get; private set; }
	}

	/*public struct NovelRange {
		public NovelCommand On { get; }
		public NovelCommand Off { get; }
		public NovelRange(NovelCommand on) {
			On = on;
			Off = null;
		}
		public NovelRange(NovelCommand on, NovelCommand off) {
			On = on;
			Off = off;
		}
		public bool IsUntilEnd => Off == null;
	}*/

	public static class JpUtils {
		
		private static readonly char[] JpValid = {
			'「', '」', '　', '～', '￥', '！',
		};
		private static readonly int[] JpRanges = {
			0x3000, 0x303F,
			0x3040, 0x309F,
			0x30A0, 0x30FF,
			0xFF00, 0xFFEF,
			0x4E00, 0x9FAF,
		};

		public static bool Check(this string s) {
			//if (s.StartsWith("Translation Note"))
			//	return false;
			foreach (char c in s) {
				if (JpValid.Contains(c))
					continue;
				for (int i = 0; i<JpRanges.Length; i+= 2) {
					if (c >= JpRanges[i] && c <= JpRanges[i+1]) {
						Trace.WriteLine(s);
						Trace.WriteLine(c);
						return true;
					}
				}
			}
			return false;
		}
	}

}
