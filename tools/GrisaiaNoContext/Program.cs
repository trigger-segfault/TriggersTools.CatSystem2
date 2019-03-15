using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2;
using TriggersTools.CatSystem2.Scenes;
using TriggersTools.CatSystem2.Scenes.Commands;
using TriggersTools.CatSystem2.Scenes.Commands.Sounds;

namespace GrisaiaNoContext {
	class Program {

		public static readonly string TmpDir = Path.Combine(AppContext.BaseDirectory, "tmp");
		public static string GetArchiveDir(KifintArchiveInfo archive) {
			return Path.Combine(TmpDir, archive.FileNameWithoutExtension);
		}
		public static string GetArchiveCompileDir(KifintArchiveInfo archive) {
			return Path.Combine(GetArchiveDir(archive), "compile");
		}

		public static string GetSceneCompilePath(SceneInfo info) {
			return Path.Combine(GetArchiveCompileDir(info.Entry.Kifint), Path.ChangeExtension(info.Entry.FileName, ".txt"));
		}
		public static string GetSceneOutputPath(SceneInfo info) {
			return Path.Combine(GetArchiveDir(info.Entry.Kifint), info.Entry.FileName);
		}
		public static string CopySceneOutput(SceneInfo info) {
			return Path.Combine(GetArchiveCompileDir(info.Entry.Kifint), Path.ChangeExtension(info.Entry.FileName, ".txt"));
		}
		public static void SaveSceneTxt(KeyValuePair<SceneInfo, List<ISceneLine>> pair) {
			new SceneScript(pair.Key.Script.FileName, pair.Value).DecompileToFile(GetSceneCompilePath(pair.Key));
			//File.WriteAllText(GetSceneCompilePath(pair.Key), )
		}

		public static SceneInfo[] GetSceneInfos(KifintLookup sceneLookup, KifintLookup updateLookup) {
			List<SceneInfo> scenes = new List<SceneInfo>();

			foreach (KifintArchive archive in sceneLookup.Kifints.Concat(updateLookup.Kifints)) {
				void AddScene(KifintEntry entry, KifintStream kifintStream) {
					if (entry.Extension == ".cst") {
						var info = new SceneInfo(entry, entry.ExtractScene(kifintStream));
						if (info.Messages.Count != 0)
							scenes.Add(info);
					}
				}
				string backup = $"{archive.FilePath}.bak";
				if (File.Exists(backup))
					File.Copy(backup, archive.FilePath, true);
				string archiveDir = GetArchiveDir(archive);
				Directory.CreateDirectory(archiveDir);
				using (KifintStream kifintStream = new KifintStream()) {
					if (archive.ArchiveType != KifintType.Update) {
						foreach (var entry in archive) {
							if (updateLookup.ContainsKey(entry.FileName))
								continue;
							AddScene(entry, kifintStream);
						}
					}
					else {
						foreach (var entry in archive) {
							AddScene(entry, kifintStream);
						}
					}
				}
			}
			return scenes.ToArray();
		}

		static void Main(string[] args) {

			CatUtils.McPath = "mc_en.exe";

			//List<SceneInfo> sceneInfos = new List<SceneInfo>();
			Dictionary<SceneInfo, List<ISceneLine>> sceneNewLines = new Dictionary<SceneInfo, List<ISceneLine>>();
			Dictionary<string, List<Message>> groupMessages = new Dictionary<string, List<Message>>();
			HashSet<KifintArchive> archivesWithScenes = new HashSet<KifintArchive>();

			//VCodes vcodes = VCodes.Load(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\Grisaia2.bin.bak");
			//Message.Builder b = new Message.Builder(null);
			//var msssg = b.Build();
			//KifintArchive sceneArchive = KifintArchive.LoadArchive(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\scene.int", vcodes.VCode2);

			string grisaiaDir = @"C:\Programs\Games\Frontwing\The Fruit of Grisaia";
			string grisaiaExe = @"Grisaia.exe";
			VCodes vcodes = VCodes.Load(Path.Combine(grisaiaDir, grisaiaExe));

			KifintLookup sceneLookup = KifintArchive.LoadLookup(KifintType.Scene, grisaiaDir, vcodes.VCode2);
			KifintLookup updateLookup = KifintArchive.LoadLookup(KifintType.Update, grisaiaDir, vcodes.VCode2);

			Directory.CreateDirectory(TmpDir);
			bool restored = false;
			var allArchives = sceneLookup.Kifints.Concat(updateLookup.Kifints);
			foreach (KifintArchive archive in allArchives) {
				string backup = $"{archive.FilePath}.bak";
				if (File.Exists(backup)) {
					File.Copy(backup, archive.FilePath, true);
					restored = true;
				}
				else
					File.Copy(archive.FilePath, backup);
			}

			if (restored) {
				sceneLookup = KifintArchive.LoadLookup(KifintType.Scene, grisaiaDir, vcodes.VCode2);
				updateLookup = KifintArchive.LoadLookup(KifintType.Update, grisaiaDir, vcodes.VCode2);
			}

			var sceneInfos = GetSceneInfos(sceneLookup, updateLookup);
			archivesWithScenes = new HashSet<KifintArchive>(sceneInfos.Select(s => s.Entry.Archive));

			foreach (KifintArchive archive in archivesWithScenes) {
				string archiveDir = GetArchiveDir(archive);
				//Directory.CreateDirectory(archiveDir);
				Directory.CreateDirectory(GetArchiveCompileDir(archive));
				foreach (var entry in archive) {
					entry.ExtractToDirectory(archiveDir);
				}
			}


			foreach (var sceneInfo in sceneInfos) {
				sceneNewLines.Add(sceneInfo, sceneInfo.Script.Lines.ToList());
				foreach (Message msg in sceneInfo.Messages) {
					if (!groupMessages.TryGetValue(msg.GroupId, out var group)) {
						group = new List<Message>();
						groupMessages.Add(msg.GroupId, group);
					}
					group.Add(msg);
				}
			}

			/*using (KifintStream kifintStream = new KifintStream()) {
				foreach (KifintEntry entry in sceneArchive) {
					SceneScript sceneScript = entry.ExtractScene(kifintStream);
					sceneNewLines.Add(sceneScript, sceneScript.Lines.ToList());
					SceneInfo sceneInfo = new SceneInfo(sceneScript);
					sceneInfos.Add(sceneScript, sceneInfo);
					foreach (Message msg in sceneInfo.Messages) {
						if (!groupMessages.TryGetValue(msg.GroupId, out var group)) {
							group = new List<Message>();
							groupMessages.Add(msg.GroupId, group);
						}
						group.Add(msg);
					}
				}
			}*/
			string ToVoiceId(SoundPlayCommand cmd) {
				string snd = cmd.Sound;
				int index = snd.IndexOf('_');
				if (index != -1)
					return snd.Substring(0, index).ToLower();
				return string.Empty;
			}

			//string sceneDir2 = @"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\scene";

			Random rng = new Random();
			foreach (var groupPair in groupMessages) {
				string groupId = groupPair.Key;
				var messages = groupPair.Value.ToList();
				var messageSpots = groupPair.Value.ToList();
				while (messages.Count != 0) {
					var msg = messages[messages.Count - 1];
					int spotIndex = rng.Next(messageSpots.Count);
					var spot = messageSpots[spotIndex];
					messages.RemoveAt(messages.Count - 1);
					messageSpots.RemoveAt(spotIndex);
					var newSceneLines = sceneNewLines[spot.Script];
					int index;
					for (int i = spot.Messages.Count - 1; i > 0; i--) {
						newSceneLines.Remove(spot.Messages[i]);
					}
					index = newSceneLines.IndexOf(spot.Messages[0]);
					newSceneLines.RemoveAt(index);
					newSceneLines.Insert(index, msg.CreateMessage());

					if (!msg.IsMonologue) {
						for (int i = spot.Names.Count - 1; i > 0; i--) {
							newSceneLines.Remove(spot.Names[i]);
						}
						index = newSceneLines.IndexOf(spot.Names[0]);
						newSceneLines.RemoveAt(index);
						newSceneLines.Insert(index, msg.CreateName());
					}

					var voices = msg.CreateVoices().ToList();
					for (int i = 0; i < spot.Voices.Count; i++) {
						//newSceneLines.Remove(spot.Voices[i]);
						string id = ToVoiceId(spot.Voices[i]);
						var voice = voices.Find(v => ToVoiceId(v) == id);
						if (voice == null)
							throw new Exception();
						voices.Remove(voice);
						index = newSceneLines.IndexOf(spot.Voices[i]);
						newSceneLines.RemoveAt(index);
						newSceneLines.Insert(index, voice);
					}
				}
			}
			foreach (var scenePair in sceneNewLines) {
				SaveSceneTxt(scenePair);
				/*string fileName = scenePair.Key.FileName;
				var newSceneLines = scenePair.Value;
				SceneScript newScene = new SceneScript(fileName, newSceneLines);
				string decompiled = newScene.Decompile();
				File.WriteAllText(Path.Combine(sceneDir2, Path.ChangeExtension(fileName, ".txt")), decompiled, CatUtils.ShiftJIS);*/
				//newScene.DecompileToFile();
			}


			foreach (KifintArchive archive in archivesWithScenes) {
				string archiveDir = GetArchiveDir(archive);
				string compileDir = GetArchiveCompileDir(archive);
				CatUtils.CompileSceneFiles(Path.Combine(compileDir, "*.txt"), archiveDir);
				Directory.Delete(compileDir, true);
				ProcessStartInfo startInfo = new ProcessStartInfo {
					FileName = "MakeInt.exe",
					Arguments = $"\"{Path.Combine(grisaiaDir, archive.FileName)}\" \"{Path.Combine(archiveDir, "*")}\""
				};
				using (Process p = Process.Start(startInfo)) {
					p.WaitForExit();
					if (p.ExitCode != 0)
						throw new Exception($"{archive.FileName} {p.ExitCode}");
				}
					/*string backup = $"{archive.FilePath}.bak";
				if (File.Exists(backup))
					File.Copy(backup, archive.FilePath, true);
				string archiveDir = GetArchiveDir(archive);
				Directory.CreateDirectory(archiveDir);
				if (archive.ArchiveType != KifintType.Update) {
					foreach (var entry in archive) {
						if (updateLookup.ContainsKey(entry.FileName))
							continue;
						//entry.ExtractToDirectory(archiveDir);
					}
				}*/
			}

			Environment.Exit(0);


			/*string sceneDir = @"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\scene";
			string sceneName = "ama_005.cst";
			string sceneBackup = $"{sceneName}.bak";
			//string sceneFile = Path.Combine(sceneDir, "ama_005.cst");
			SceneScript script = SceneScript.Extract(Path.Combine(sceneDir, sceneBackup));
			List<ISceneLine> lines = new List<ISceneLine>();
			List<ISceneLine> newLines = new List<ISceneLine>();
			lines.AddRange(script.Lines);
			newLines.AddRange(script.Lines);

			//List<CharacterMessage> messages = new List<CharacterMessage>();
			Dictionary<string, List<CharacterMessage>> idMessages = new Dictionary<string, List<CharacterMessage>>();
			CharacterMessage currentMessage = null;
			SceneName currentName = null;
			SceneMessage currentMsg = null;
			SoundPlayCommand currentVoice = null;

			void AddIdMessage(CharacterMessage message) {
				if (!idMessages.TryGetValue(message.GroupId, out var group)) {
					group = new List<CharacterMessage>();
					idMessages.Add(message.GroupId, group);
				}
				group.Add(message);
				//messages.Add(currentMessage);
			}

			for (int i = lines.Count - 1; i >= 0; i--) {
				ISceneLine line = lines[i];
				if (line.Type == SceneLineType.Input || line.Type == SceneLineType.Page) {
					if (currentMessage != null && currentMessage.HasMessage) {
						currentMessage.Finish(currentName, currentMsg, currentVoice);
						AddIdMessage(currentMessage);
						currentMsg = null;
						currentName = null;
						currentVoice = null;
					}
					currentMessage = new CharacterMessage(line);
				}
				else if (currentMessage == null) {
					continue;
				}
				switch (line) {
				case SceneMessage msg:
					if (currentMsg != null)
						newLines.Remove(currentMsg);
					currentMessage.PrependMessage(msg);
					currentMsg = msg;
					break;
				case SceneName name:
					if (currentName != null)
						newLines.Remove(currentName);
					currentMessage.PrependName(name);
					currentName = name;
					break;
				case ISceneCommand cmd:
					if (line is SoundPlayCommand voice && voice.SoundType == SoundType.Pcm) {
						if (currentVoice != null)
							newLines.Remove(currentVoice);
						currentMessage.AddVoiceCommand(voice);
						currentVoice = voice;
					}
					else if (line is ConditionCommand condition) {
						if (condition.Command is SoundPlayCommand play2)
							Console.WriteLine(condition.Content);
					}
					break;
				}
			}
			Random random = new Random();
			foreach (var groupPair in idMessages) {
				string id = groupPair.Key;
				var messages = groupPair.Value.ToList();
				var messageSpots = groupPair.Value.ToList();
				while (messages.Count != 0) {
					var msg = messages[messages.Count - 1];
					int spotIndex = random.Next(messageSpots.Count);
					var spot = messageSpots[spotIndex];
					messages.RemoveAt(messages.Count - 1);
					messageSpots.RemoveAt(spotIndex);
					int index = newLines.IndexOf(spot.NameLine);
					if (index != -1) {
						newLines.RemoveAt(index);
						newLines.Insert(index, new SceneName(msg.NameContent));
					}
					index = newLines.IndexOf(spot.MessageLine);
					if (index != -1) {
						newLines.RemoveAt(index);
						newLines.Insert(index, new SceneMessage(msg.MessageContent));
					}
					index = newLines.IndexOf(spot.VoiceLine);
					if (index != -1) {
						newLines.RemoveAt(index);
						foreach (var voice in msg.VoiceCommands)
							newLines.Insert(index++, new SoundPlayCommand(voice.Content));
					}
				}
			}
			CatUtils.McPath = Path.Combine(AppContext.BaseDirectory, "mc_en.exe");
			SceneScript newScript = new SceneScript(sceneName, newLines);
			int result = CatUtils.CompileSceneScript(newScript.Decompile(), sceneDir);
			Console.WriteLine(result);
			Console.ReadLine();*/
		}
	}
}
