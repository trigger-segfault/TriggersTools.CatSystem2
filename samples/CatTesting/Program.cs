using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher;
using TriggersTools.CatSystem2.Patcher.Patches;
using TriggersTools.SharpUtils.IO;
using System.Diagnostics;
using TriggersTools.Resources.Dialog;
using TriggersTools.Resources.Menu;
using TriggersTools.Resources;
using Newtonsoft.Json;
using TriggersTools.CatSystem2.Patcher.Programs.CS2;
using TriggersTools.CatSystem2.Scenes;
using TriggersTools.CatSystem2.Scenes.Commands;

namespace TriggersTools.CatSystem2.Testing {
	class Program {
		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern IntPtr LoadLibrary(string lpFileName);
		static void Main(string[] args) {
			var screen = ScreenScript.Extract(@"C:\Users\Onii-chan\Source\C#\TriggersTools\CatSystem2\samples\CatTesting\bin\Debug\fcgview.fes");
			var screen2 = ScreenScript.Extract(@"C:\Programs\Tools\CatSystem2_v401\system\fes\cgview.fes");
			//SceneScript.DecompileToFile(@)
			string vc2 = string.Empty;
			Stopwatch watchNative = new Stopwatch();
			Stopwatch watchManaged = new Stopwatch();
			Stopwatch watchManaged2 = new Stopwatch();
			VCodes vcodes = VCodes.Load(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\Grisaia2.bin.bak");
			/*using (var stream = File.OpenRead("config.fes")) {
				byte[] dat = new byte[20];
				stream.Position = stream.Length - 1;
				BinaryReader reader = new BinaryReader(stream);
				Console.WriteLine(stream.Read(dat, 0, dat.Length));
				Console.WriteLine(stream.Read(dat, 0, dat.Length));
				stream.Position = stream.Length - 1;
				reader.ReadUInt16();
				Console.ReadLine();
			}*/
			vc2 = vcodes.VCode2;
			/*for (int i = 0; i < 100000; i++) {
				CatDebug.NativeBlowfish = true;
				watchNative.Start();
				vc2 = vcodes.VCode2;
				watchNative.Stop();
				CatDebug.NativeBlowfish = false;
				CatDebug.ManagedBlowfishRound = true;
				watchManaged.Start();
				vc2 = vcodes.VCode2;
				watchManaged.Stop();
				CatDebug.ManagedBlowfishRound = false;
				watchManaged2.Start();
				vc2 = vcodes.VCode2;
				watchManaged2.Stop();
			}
			Console.WriteLine(watchNative.ElapsedMilliseconds);
			Console.WriteLine(watchManaged.ElapsedMilliseconds);
			Console.WriteLine(watchManaged2.ElapsedMilliseconds);*/

			string kajitsuDir = @"C:\Programs\Games\Frontwing\The Fruit of Grisaia";
			string kajitsuExe = "Grisaia.exe";
			VCodes kajitsoVCodes = VCodes.Load(Path.Combine(kajitsuDir, kajitsuExe));
			string hg3Key = "bg15n_d.hg3";
			KifintLookup images = KifintArchive.LoadLookup(KifintType.Image, kajitsuDir, kajitsoVCodes.VCode2);
			Stopwatch watch1 = new Stopwatch();
			Stopwatch watch2 = new Stopwatch();
			Directory.CreateDirectory("hg3");
			CatDebug.SpeedTestHgx = true;
			CatDebug.NativeBlowfish = true;
			//var entr = images[hg3Key];
			Stopwatch w1 = new Stopwatch();
			Stopwatch w2 = new Stopwatch();
			using (KifintStream kifintStream = new KifintStream()) {
				int index = 0;
				foreach (var entry in images.Take(25)) {
					//if (entry.Length < 100000)
					//	continue;
					if (entry.Extension != ".hg3")
						continue;
					//if (entry.FileNameWithoutExtension != "yum011a")
					//	continue;
					//if (index % 25 == 0)
					//	Console.Write($"\r{index}");
					//index++;

					//int loops = 1;// Math.Max(Math.Min(2000, 5 * 26000000 / entry.Length), 5);
					//Console.WriteLine($"{entry} Loops={loops}");

					CatDebug.StreamExtract = false;
					//w1.Start();
					//for (int i = 0; i < loops; i++)
						entry.ExtractHgxAndImages(kifintStream, "hg3", HgxOptions.None);//.CopyTo(Stream.Null);
																						//w1.Stop();
																						//Console.WriteLine($"Extract: {w.ElapsedMilliseconds}ms");

					CatDebug.StreamExtract = true;
					//w2.Start();
					//for (int i = 0; i < loops; i++)
						entry.ExtractHgxAndImages(kifintStream, "hg3", HgxOptions.None);//.CopyTo(Stream.Null);
					//w2.Stop();
					//Console.WriteLine($" Stream: {w.ElapsedMilliseconds}ms");
					//Console.WriteLine();
					//Thread.Sleep(1000);
				}
				foreach (var entry in images) {
					//if (entry.Length < 100000)
					//	continue;
					if (entry.Extension != ".hg3")
						continue;
					//if (entry.FileNameWithoutExtension != "yum011a")
					//	continue;
					if (index % 25 == 0)
						Console.Write($"\r{index}");
					index++;

					int loops = 1;// Math.Max(Math.Min(2000, 5 * 26000000 / entry.Length), 5);
								  //Console.WriteLine($"{entry} Loops={loops}");

					CatDebug.StreamExtract = false;
					w1.Start();
					//for (int i = 0; i < loops; i++)
						entry.ExtractHgxAndImages(kifintStream, "hg3", HgxOptions.None);//.CopyTo(Stream.Null);
					w1.Stop();
					//Console.WriteLine($"Extract: {w.ElapsedMilliseconds}ms");

					CatDebug.StreamExtract = true;
					w2.Start();
					//for (int i = 0; i < loops; i++)
						entry.ExtractHgxAndImages(kifintStream, "hg3", HgxOptions.None);//.CopyTo(Stream.Null);
					w2.Stop();
					//Console.WriteLine($" Stream: {w.ElapsedMilliseconds}ms");
					//Console.WriteLine();
					//Thread.Sleep(1000);
				}
				Console.WriteLine($"\r{index}");
				Console.WriteLine($"Extract: {w1.ElapsedMilliseconds}ms");
				Console.WriteLine($" Stream: {w2.ElapsedMilliseconds}ms");
			}
			/*for (int j = 0; j < 4; j++) {
				CatDebug.ManagedBlowfishRound = true;
				watch1.Reset();
				watch2.Reset();
				for (int i = 0; i < 6; i++) {
					watch1.Start();
					imageLookup[hg3Key].ExtractHg3AndImages("hg3", HgxOptions.None);
					watch1.Stop();
					watch2.Start();
					using (var stream = KifintArchive.ExtractToStream(new KifintStream(), imageLookup[hg3Key]))
						HgxImage.ExtractImages(stream, hg3Key, "hg3", HgxOptions.None);
					watch2.Stop();
				}
				Console.WriteLine(watch1.ElapsedMilliseconds);
				Console.WriteLine(watch2.ElapsedMilliseconds);
				CatDebug.ManagedBlowfishRound = false;
				watch1.Reset();
				watch2.Reset();
				for (int i = 0; i < 6; i++) {
					watch1.Start();
					imageLookup[hg3Key].ExtractHg3AndImages("hg3", HgxOptions.None);
					watch1.Stop();
					watch2.Start();
					using (var stream = KifintArchive.ExtractToStream(new KifintStream(), imageLookup[hg3Key]))
						HgxImage.ExtractImages(stream, hg3Key, "hg3", HgxOptions.None);
					watch2.Stop();
				}
				Console.WriteLine(watch1.ElapsedMilliseconds);
				Console.WriteLine(watch2.ElapsedMilliseconds);
			}*/
			images = null;
			Console.ReadLine();

			using (KifintStream kifintStream = new KifintStream()) {
				KifintLookup movies;

				//CatDebug.NativeBlowfish = true;
				CatDebug.ManagedBlowfishRound = true;
				movies = KifintArchive.LoadLookup(KifintType.Movie, @"C:\Programs\Games\Frontwing\Labyrinth of Grisaia", vc2);
				GC.Collect();
				watchNative.Restart();
				foreach (KifintEntry movie in movies) {
					//movie.ExtractToFile(kifintStream, "mov.mpg");
					movie.ExtractToBytes(kifintStream);
				}
				Console.WriteLine(watchNative.ElapsedMilliseconds);

				//CatDebug.NativeBlowfish = true;
				CatDebug.ManagedBlowfishRound = false;
				movies = KifintArchive.LoadLookup(KifintType.Movie, @"C:\Programs\Games\Frontwing\Labyrinth of Grisaia", vc2);
				GC.Collect();
				watchNative.Restart();
				foreach (KifintEntry movie in movies) {
					//movie.ExtractToFile(kifintStream, "mov.mpg");
					movie.ExtractToBytes(kifintStream);
				}
				Console.WriteLine(watchNative.ElapsedMilliseconds);

				//CatDebug.NativeBlowfish = false;
				CatDebug.ManagedBlowfishRound = false;
				movies = KifintArchive.LoadLookup(KifintType.Movie, @"C:\Programs\Games\Frontwing\Labyrinth of Grisaia", vc2);
				GC.Collect();
				watchManaged.Restart();
				foreach (KifintEntry movie in movies) {
					//movie.ExtractToFile(kifintStream, "mov.mpg");
					movie.ExtractToBytes(kifintStream);
				}
				Console.WriteLine(watchManaged.ElapsedMilliseconds);

				/*CatDebug.NativeBlowfish = true;
				movies = KifintArchive.LoadLookup(KifintType.Movie, @"C:\Programs\Games\Frontwing\Labyrinth of Grisaia", vc2);
				GC.Collect();
				watchNative.Restart();
				foreach (KifintEntry movie in movies) {
					using (var fs = File.Create("mov.mpg"))
						KifintArchive.ExtractToStream(kifintStream, movie, fs);
				}
				Console.WriteLine(watchNative.ElapsedMilliseconds);

				CatDebug.NativeBlowfish = false;
				movies = KifintArchive.LoadLookup(KifintType.Movie, @"C:\Programs\Games\Frontwing\Labyrinth of Grisaia", vc2);
				GC.Collect();
				watchManaged.Restart();
				foreach (KifintEntry movie in movies) {
					using (var fs = File.Create("mov.mpg"))
						KifintArchive.ExtractToStream(kifintStream, movie, fs);
				}
				Console.WriteLine(watchManaged.ElapsedMilliseconds);
				GC.Collect();*/

			}
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			Console.WriteLine("FINISHED");
			Console.ReadLine();
			Random random = new Random();
			const int rngLength = 1024 * 64;
			byte[] rngBytesOrigin = new byte[rngLength];
			byte[] rngBytes = new byte[rngLength];
			byte[] rngUints = new byte[rngLength];
			byte[] rngBytesAdd = new byte[rngLength];
			random.NextBytes(rngBytesOrigin);
			random.NextBytes(rngBytesAdd);
			Buffer.BlockCopy(rngBytesOrigin, 0, rngBytes, 0, rngLength);
			Buffer.BlockCopy(rngBytesOrigin, 0, rngUints, 0, rngLength);
			unchecked {
				unsafe {
					fixed (byte* pRngBytesAdd = rngBytesAdd)
					fixed (byte* pRngBytes = rngUints) {
						uint* pRngUintsAdd = (uint*) pRngBytesAdd;
						uint* pRngUints = (uint*) pRngBytes;
						for (int i = 0; i < rngLength / 4; i++) {
							pRngUints[i] += pRngUintsAdd[i];
						}
						/*for (int i = 0; i < rngLength; i++) {
							pRngBytes[i] += pRngBytesAdd[i];
						}*/
					}
					for (int i = 0; i < rngLength; i++) {
						rngBytes[i] += rngBytesAdd[i];
					}
				}
			}
			/*for (int i = 0; i < rngLength; i++) {
				if (rngBytes[i] != rngUints[i])
					Console.WriteLine(i);
			}*/
			bool isEqual = rngBytes.SequenceEqual(rngUints);

			/*string vcode22 = VCodes.FindVCode2(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\Grisaia2.bin.bak");
			Kifint.DecryptLookup(KifintType.Image, @"C:\Programs\Games\Frontwing\Labyrinth of Grisaia", vcode22);
			Kifint.DecryptLookup(KifintType.Image, @"C:\Programs\Games\Frontwing\Labyrinth of Grisaia", vcode22, csBlowfish: true);
			var ll = Kifint.DecryptLookup(KifintType.Image, @"C:\Programs\Games\Frontwing\Labyrinth of Grisaia", vcode22);
			var lll = Kifint.DecryptLookup(KifintType.Image, @"C:\Programs\Games\Frontwing\Labyrinth of Grisaia", vcode22, csBlowfish: true);
			for (int i = 0; i < ll.Count; i++) {

			}*/
			Directory.CreateDirectory("img2");
			Directory.CreateDirectory("imgv");
			Directory.CreateDirectory("imgnv");
			HgxImage.ExtractImages("sys_flipped.hg3", "imgv", HgxOptions.Flip).SaveJsonToDirectory("imgv");
			HgxImage.ExtractImages("sys_flipped.hg3", "imgnv", HgxOptions.None).SaveJsonToDirectory("imgnv");
			//HgxImage.ExtractImages("img_flipped.hg3", "img2", HgxOptions.None).SaveJsonToDirectory("img2");
			//HgxImage.ExtractImages("img_notflipped.hg3", "img2", HgxOptions.None).SaveJsonToDirectory("img2");
			Console.ReadLine();
			Directory.CreateDirectory("img");
			IEnumerable<string> hg3Files = Directory.GetFiles(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\image", "*.hg3");
			//hg3Files = hg3Files.Take(100);
			int take = 100;
			try {
				foreach (string hg3File in hg3Files.Take(10))
					HgxImage.ExtractImages(hg3File, "img", HgxOptions.None);
				/*Stopwatch hg3Watch = HgxImage.ProcessImageWatch; hg3Watch.Reset();
				for (int i = 0; i < 10; i++) {
					foreach (string hg3File in hg3Files.Take(take))
						HgxImage.ExtractImages(hg3File, "img", false);
					Console.WriteLine(hg3Watch.ElapsedMilliseconds); hg3Watch.Reset();
				}*/
			}
			catch (Exception ex) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex);
			}
			Console.ReadLine();
			Environment.Exit(0);
			HgxImage.ExtractImages(@"C:\Programs\Tools\CatSystem2_v401\tool\img_test2.hg3", ".", HgxOptions.None);
			StringsScraper.BinaryScrape("cs2_open.exe", "strings/null/cs2", 0x5B6458, 0x5B7558);
			Console.ReadLine();
			StringsScraper.BinarySearch("cs2_open.exe", "page");
			//foreach (string file in Directory.GetFiles(@"C:\Programs\Tools\CatSystem2_v401\system\scene", "*.cst")) {
			foreach (string file in Directory.GetFiles(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\scene", "*.cst")) {
				SceneScript scene2 = SceneScript.Extract(file);
				foreach (ISceneLine line in scene2) {
					if (line.Type == SceneLineType.Page) {
						Console.WriteLine(file);
					}
				}
			}
			Console.Write("FINISHED");
			Console.ReadLine();
			using (FileStream fp = File.OpenRead("mc.exe")) {
				BinaryReader reader = new BinaryReader(fp);
				while (!fp.IsEndOfStream()) {
					ushort type = reader.ReadUInt16();
					if (type == 0x0301) {
						Console.WriteLine($"{fp.Position:X8}");
					}
				}
			}
			//byte[] data = File.ReadAllBytes("mc.exe");
			//for (int i = 0; i <)
			StringsScraper.BinarySearch("mc.exe", "page");
			SceneScript scene = SceneScript.Extract(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\scene\ama_006.cst");

			foreach (ISceneLine line in scene) {
				if (line.Type == SceneLineType.Command) {
					var command = (ISceneCommand) line;
				}
			}
			//var h = LoadLibrary(@"C:\Users\Onii-chan\AppData\Local\Temp\TriggersToolsGames\CatSystem2\asmodean.dll");
			//Embedded
			var hg3 = HgxImage.ExtractImages(@"C:\Programs\Tools\CatSystem2_v401\system\image\sys_confirm.hg3", ".", HgxOptions.None);
			string grisaiaInstallDir = @"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)";
			string grisaiaExe = Path.Combine(grisaiaInstallDir, "Grisaia2.bin.bak");
			string grisaiaConfigInt = Path.Combine(grisaiaInstallDir, "config.int");
			//VCodes grisaiaVCodes = VCodes.Load(grisaiaExe);
			//KifintLookup lookup = Kifint.DecryptLookup(KifintType.Config, grisaiaInstallDir, grisaiaVCodes.VCode2);
			//lookup["startup.xml"].ExtractToFile("startup2.xml");
			File.Copy("startup2.xml", "startup.xml", true);
			var patcher = new ResolveXmlCommentErrorsPatch("startup.xml");
			var patcher2 = new CS2XmlDebugPatch("startup.xml", true, "TriggersTools.CatSystem2.Patcher.Resources.CS2");
			patcher.Patch();
			patcher2.Patch();
			Console.WriteLine("FINISHED");
			Console.ReadLine();
			for (int i = 0; i < 10; i++) {
				Stopwatch swatch = Stopwatch.StartNew();
				//var vcodes = VCodes.Load(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\Grisaia2.bin");
				string exeFile = CatUtils.FindCatExecutable(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)");
				Console.WriteLine(swatch.ElapsedMilliseconds);
				Console.WriteLine(exeFile ?? string.Empty);
			}
			Console.ReadLine();
			//vcodes.Save(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\Grisaia2.bin");
			//File.WriteAllText("vcodes.json", JsonConvert.SerializeObject(vcodes, Formatting.Indented));
			//vcodes.KeyCode = vcodes.KeyCode;
			//vcodes.KeyCode = new byte[4] { 0x1, 0x1, 0x1, 0x1 };
			Console.Beep();
			Console.ReadLine();
			using (var ms = new MemoryStream()) {
				BinaryReader reader = new BinaryReader(ms);
				BinaryWriter writer = new BinaryWriter(ms);
				writer.Write(1);
				ms.Position = 0;
				byte[] buffer = reader.ReadBytes(4);
				int[] ints = new int[1];
				Buffer.BlockCopy(buffer, 0, ints, 0, 4);
				Console.WriteLine(ints[0]);
			}
				//ushort us = 1;
			byte[] bs = { 1, 0 };
			byte[] decompressedA;
			//byte[] decompressedB;
			for (int i = 0; i < 3; i++) {
				Stopwatch watch = new Stopwatch();
				using (FileStream fs = File.OpenRead("asa.zt")) {
					BinaryReader reader = new BinaryReader(fs);
					int nextEntry = 0;
					long startPosition = fs.Position;
					//do {
					fs.Position = startPosition + nextEntry;
					startPosition = fs.Position;
					nextEntry = reader.ReadInt32();
					reader.ReadInt32();
					int offsetToData = reader.ReadInt32();
					reader.ReadInt32();
					string fileName = reader.ReadFixedString(256, '\0');
					int reserved = reader.ReadInt32();
					int compressedLength = reader.ReadInt32();
					int decompressedLength = reader.ReadInt32();
					byte[] compressed = reader.ReadBytes(compressedLength);
					watch.Restart();
					byte[] decompressed = new byte[decompressedLength];
					//ZLib.Uncompress(decompressed, ref decompressedLength, compressed, compressedLength);
					decompressedA = decompressed;
					Console.WriteLine(watch.ElapsedMilliseconds);
					/*Console.WriteLine(offsetToData);
					Console.WriteLine($"{fs:Position:X8}");
					Console.WriteLine(decompressedLength);

					Console.WriteLine(decompressedLength);*/

					//File.WriteAllBytes(Path.Combine("ztout/2", fileName), decompressed);

					//} while (nextEntry != 0);
				}
				using (FileStream fs = File.OpenRead("asa.zt")) {
					BinaryReader reader = new BinaryReader(fs);
					int nextEntry = 0;
					long startPosition = fs.Position;
					//do {
					fs.Position = startPosition + nextEntry;
					startPosition = fs.Position;
					nextEntry = reader.ReadInt32();
					reader.ReadInt32();
					int offsetToData = reader.ReadInt32();
					reader.ReadInt32();
					string fileName = reader.ReadFixedString(256, '\0');
					int reserved = reader.ReadInt32();
					int compressedLength = reader.ReadInt32();
					int decompressedLength = reader.ReadInt32();
					byte[] compressed = reader.ReadBytes(compressedLength);
					watch.Restart();
					//decompressedB = null;// ZlibStream.UncompressBuffer(compressed);
					Console.WriteLine(watch.ElapsedMilliseconds);
					/*using (var ms = new MemoryStream())
					using (ZlibStream zs = new ZlibStream(ms, CompressionMode.Decompress)) {
						//byte[] decompressed = new byte[decompressedLength];
						zs.Read(compressed, 0, compressedLength);
						decompressedB = ms.ToArray();
					}*/
					//byte[] decompressed = new byte[decompressedLength];
					/*Console.WriteLine(offsetToData);
					Console.WriteLine($"{fs:Position:X8}");
					Console.WriteLine(decompressedLength);

					//ZLib.Uncompress(decompressed, ref decompressedLength, compressed, compressedLength);
					Console.WriteLine(decompressedLength);

					File.WriteAllBytes(Path.Combine("ztout/2", fileName), decompressed);*/

					//} while (nextEntry != 0);
				}
			}
			//344AC
			//@"C:\Programs\Tools\CatSystem2_v401\psds\BA01_1.hg2"
			Directory.CreateDirectory("hg3");
			HgxImage.Extract(@"C:\Programs\Tools\CatSystem2_v401\tool\img_test2.hg3");
			HgxImage.Extract(@"C:\Programs\Tools\CatSystem2_v401\psds\sys_confirm.hg2");
			var hg3Img = HgxImage.ExtractImages(@"C:\Programs\Tools\CatSystem2_v401\psds\BA01_1.hg3",
				"hg3", HgxOptions.None);
			hg3Img.SaveJsonToDirectory("hg3");
			Console.OutputEncoding = CatUtils.ShiftJIS;
			ZTPatcher zt2 = new ZTPatcher {
				InstallDir = ".",
			};
			zt2.Patch();
			WGCPatcher wgc2 = new WGCPatcher {
				InstallDir = ".",
			};
			wgc2.Patch();
			Console.ReadLine();
			string[] lines = File.ReadAllLines("strings/wgc/binary.txt");
			StringsScraper.BinaryValidate(lines);
			Console.ReadLine();
			lines = File.ReadAllLines("strings/zt/binary.txt");
			StringsScraper.BinaryValidate(lines);
			Console.ReadLine();
			StringsScraper.BinarySearch("WGC.exe", "img_jpg");//256C28, 256C8C
			Console.ReadLine();
			BinaryRange[] rangess = {
				new BinaryRange(0x211DD4, 0x211DEC),
				new BinaryRange(0x211E54, 0x211FA8),
				new BinaryRange(0x212078, 0x212138),
				new BinaryRange(0x212154, 0x2121A0),
				new BinaryRange(0x2122F0, 0x212204),
				new BinaryRange(0x212304, 0x2123C4),
				new BinaryRange(0x2123D8, 0x2123F4),
				new BinaryRange(0x21C300, 0x21C31C),

				new BinaryRange(0x25BAF8, 0x25BD20),
				new BinaryRange(0x25BD60, 0x25BD90),
				new BinaryRange(0x25BDD0, 0x25BDF4),
				new BinaryRange(0x25BE90, 0x25BFC0),
				new BinaryRange(0x25C00C, 0x25C118),
			};
			StringsScraper.BinaryScrape("WGC.exe", "strings/wgc", rangess); //2120A0

			Console.ReadLine();
			StringsScraper.BinaryScrape("ztpack.exe", "strings/zt", 0x344AC, 0x345F8);
			Console.ReadLine();
			/*var resInfo = new ResourceInfo("cs2_open_en.exe");
			File.Copy("cs2_open_en.exe", "cs2_open_en2.exe", true);
			Thread.Sleep(400);
			resInfo.Save("cs2_open_en2.exe");
			var menuRes = new MenuResource("cs2_open.exe", 105, 1041);
			var dialogRes = new DialogResource("cs2_open.exe", 201, 1041);
			var menu = (MenuTemplate) menuRes.Template;
			var dialog = (DialogExTemplate) dialogRes.Template;
			dialog.Caption = "Hello World!";
			dialog.Controls[0].CaptionId = "OK WORLD!";
			dialog.Controls[1].HelpId = 25;
			menu.MenuItems[0].MenuString = "Hello World!";
			menu.MenuItems.Add(new MenuTemplateItemPopup {
				MenuString = "Popup",
				MenuItems = {
					new MenuTemplateItemCommand {
						IsSeparator = true,
					},
					new MenuTemplateItemCommand {
						MenuString = "Do Nothing",
						MenuId = 2555,
					},
				},
			});
			//IntPtr ptr = Marshal.StringToHGlobalUni(null);
			//File.Copy("cs2_open.exe", "cs2_open2.exe", true);
			Thread.Sleep(400);
			Resource.SaveTo("cs2_open_en2.exe", new Resource[] { menuRes, dialogRes });*/

			CS2Patcher cs2Patcher = new CS2Patcher {
				InstallDir = ".",
			};
			Console.WriteLine(cs2Patcher.Patch());
			Console.WriteLine("FINISHED");
			WGCPatcher wgc = new WGCPatcher {
				InstallDir = ".",
			};
			Console.WriteLine(wgc.Patch());
			Console.WriteLine("FINISHED");
			Console.ReadLine();
			//ResourceInfo reInfo = new ResourceInfo();
			//reInfo.Load(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\Grisaia2.bin.bak");
			//Console.Write("");
			/*PsDotNet
			/*using (var s = File.OpenRead(@"C:\Programs\Tools\CatSystem2_v401\psds\CS2用_キャラクター立ち絵2.psd")) {
				BinaryReverseReader2 reader = new BinaryReverseReader2(s);
				while (!s.IsEndOfStream()) {
					int sh = reader.ReadInt32();
					if (sh == 930)
						Console.WriteLine($"{s.Position:X8}");
				}
			}*/

			/*	Console.OutputEncoding = CatUtils.ShiftJIS;
			Document psd = new Document(@"C:\Programs\Tools\CatSystem2_v401\psds\CS2用_キャラクター立ち絵.psd");
			psd.SaveXml("psd.xml", false);
			foreach (var layer in psd.Layers) {
				Console.WriteLine(layer.Name);
			}*/
			//Document psd2 = new Document(@"C:\Programs\Tools\CatSystem2_v401\tool\img_test.psd");
			//psd2.SaveXml("psd.xml", false);
			//Document psd2 = new Document(@"C:\Programs\Tools\CatSystem2_v401\tool\img_test.psd");
			//StringsScraper.BinarySearch("WGC.exe", "選択されたファイルをリストから");
			//StringsScraper.BinaryScrape("WGC.exe", "strings/wgc", 0x211000, 0x212F00);
			//StringsScraper.BinaryScrape("WGC.exe", "strings/wgc", 0x256000, 0x1000000);
			//StringsScraper.BinarySearch("WGC.exe", "mode");//21220C
			//212298
			//string vcode = VCodes.FindVCode(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\Grisaia2.bin.bak");
			string vcode2 = VCodes.FindVCode2(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\Grisaia2.bin.bak");
			var hg = HgxImage.ExtractImages(@"C:\Programs\Tools\CatSystem2_v401\tool\img_test2.hg3", ".", HgxOptions.None);
			hg = HgxImage.Extract(@"C:\Programs\Tools\CatSystem2_v401\system\image\sys_confirm.hg3");
			//int iiii = int.Parse("FFFFFFFF", NumberStyles.HexNumber);
			hg = HgxImage.Extract(@"C:\Programs\Games\Frontwing\Labyrinth of Grisaia - Copy (2)\image\Tmic1cs_6.hg3");
			byte[] zt = File.ReadAllBytes("zt.zt");
			/*using (var ms = new MemoryStream()) {
				BinaryWriter writer = new BinaryWriter(ms);
				writer.Write(zt.Take(0x110).ToArray());
				writer.Write(zt.Skip(0x110).Take(0x194 - 0x110).ToArray());
				ms.Position = 0;
				writer.Write(405);
				writer.Write(0);
				writer.Write()
				ms.Position = 404;
				writer.Write((byte) 0);
				writer.Write(zt.Skip(404).ToArray());
				//ms.Position = 0x110;
				//writer.Write(123233);
				zt = ms.ToArray();
			}*/
			//File.WriteAllBytes("zt2.zt", zt);
			var ztpackage = ZtPackage.ExtractFiles("zt.zt", "ztout");
			Environment.Exit(0);
			/*using (FileStream fs = File.Create("out.zt")) {
				long offset = 0x100000194;
				BinaryWriter writer = new BinaryWriter(fs);
				writer.Write(zt.Take(0x194).ToArray());
				fs.Position = offset;
				writer.Write(zt.Skip(0x194).ToArray());
			}*/
			//StringsScraper.BinaryValidate(File.ReadAllLines("strings/cs2/binary_5.txt"));
			CS2Patcher cs2 = new CS2Patcher {
				InstallDir = ".",
			};
			//Console.WriteLine(cs2.Patch("cs2_open.exe", "cs2_open_en.exe"));
			Console.WriteLine(cs2.Patch());
			Console.WriteLine("FINISHED");
			Console.ReadLine();
			/*using (StreamWriter writer = File.CreateText("ranges_out.txt")) {
				List<int> starts = new List<int>();
				List<int> ends = new List<int>();
				Regex regex = new Regex(@"\t\t// 0x(?'start'[A-Fa-f0-9]+), 0x(?'end'[A-Fa-f0-9]+)");
				foreach (string line in File.ReadAllLines("ranges.txt")) {
					Match match = regex.Match(line);
					if (match.Success) {
						starts.Add(int.Parse(match.Groups["start"].Value, NumberStyles.HexNumber));
						ends.Add(int.Parse(match.Groups["end"].Value, NumberStyles.HexNumber));
						if (starts.Last() >= ends.Last())
							throw new Exception();
					}
				}
				for (int i = 0; i < starts.Count; i++) {
					int start1 = starts[i];
					int end1 = ends[i];
					if (start1 >= end1)
						throw new Exception();
					for (int j = 0; j < starts.Count; j++) {
						if (i == j)
							continue;
						int start2 = starts[j];
						int end2 = ends[j];
						if (start1 >= start2 && start1 <= end2)
							throw new Exception();
						if (end1 >= start2 && end1 <= end2)
							throw new Exception();
					}
				}
				starts.Sort();
				ends.Sort();
				for (int i = 0; i < starts.Count; i++) {

					int start = starts[i];
					int end = ends[i];
					writer.WriteLine($"new BinaryRange(0x{start:X6}, 0x{end:X6}),");
				}
				writer.Flush();
			}
			Console.WriteLine("FINISHED");
			Console.ReadLine();
			CS2Patcher cs2 = new CS2Patcher {
				Executable = "cs2_open.exe",
			};
			Console.WriteLine(cs2.Patch());
			Console.WriteLine("FINISHED");
			Console.ReadLine();*/
			var ranges = new BinaryRange[] {
				new BinaryRange(0x58CFA0, 0x58CFC0),
				new BinaryRange(0x590960, 0x590B7C),
				new BinaryRange(0x590C14, 0x590C38),
				new BinaryRange(0x590CA0, 0x590D9C),
				new BinaryRange(0x590E14, 0x591120),
				new BinaryRange(0x591160, 0x5911BC),
				new BinaryRange(0x5911FC, 0x59122C),
				new BinaryRange(0x59126C, 0x59144C),
				new BinaryRange(0x5914F4, 0x59158C),
				new BinaryRange(0x5915E8, 0x59164C),
				new BinaryRange(0x5917B0, 0x5917E8),
				new BinaryRange(0x591864, 0x591890),
				new BinaryRange(0x591944, 0x5919CC),
				new BinaryRange(0x5919F0, 0x591AE0),
				new BinaryRange(0x591B04, 0x591BA0),
				new BinaryRange(0x591BD4, 0x591C64),
				new BinaryRange(0x591C88, 0x591D18),
				new BinaryRange(0x591D3C, 0x591DD0),
				new BinaryRange(0x591FD8, 0x592000),
				new BinaryRange(0x5920E8, 0x59211C),
				new BinaryRange(0x59259C, 0x59263C),
				new BinaryRange(0x5962C4, 0x596334),
				new BinaryRange(0x596368, 0x596388),
				new BinaryRange(0x5963B8, 0x59645C),
				new BinaryRange(0x596638, 0x5967A4),
				new BinaryRange(0x596970, 0x596C44),
				new BinaryRange(0x596F9C, 0x597334),
				new BinaryRange(0x597380, 0x59754C),
				new BinaryRange(0x59797C, 0x597AF8),
				new BinaryRange(0x597C20, 0x597D84),
				new BinaryRange(0x597DB0, 0x597E10),
				new BinaryRange(0x597E78, 0x597FB4),
				new BinaryRange(0x597FC8, 0x5980EC),
				new BinaryRange(0x598130, 0x5981C0),
				new BinaryRange(0x598270, 0x598328),
				new BinaryRange(0x598370, 0x5988B4),
				new BinaryRange(0x598988, 0x598B14),
				new BinaryRange(0x598BA8, 0x598C04),
				new BinaryRange(0x598DEC, 0x598F20),
				new BinaryRange(0x598F34, 0x5990CC),
				new BinaryRange(0x599204, 0x599220),
				new BinaryRange(0x5992AC, 0x599384),
				new BinaryRange(0x5993E0, 0x599470),
				new BinaryRange(0x599500, 0x5997C0),
				new BinaryRange(0x5AFC20, 0x5AFC48),
				new BinaryRange(0x5AFD00, 0x5AFE24),
				new BinaryRange(0x5B02EC, 0x5B03D0),
				new BinaryRange(0x5B0814, 0x5B0880),
				new BinaryRange(0x5B0A48, 0x5B0AAC),
				new BinaryRange(0x5B25E8, 0x5B27D0),
				new BinaryRange(0x5B2844, 0x5B28DC),
				new BinaryRange(0x5B2BC8, 0x5B2BD8),
				new BinaryRange(0x5B3480, 0x5B34B8),
				new BinaryRange(0x5B3578, 0x5B35B4),
				new BinaryRange(0x5B4324, 0x5B4420),
				new BinaryRange(0x5B4518, 0x5B4534),
				new BinaryRange(0x5B464C, 0x5B4850),
				new BinaryRange(0x5B4FB4, 0x5B50D4),
			};
			/*Console.OutputEncoding = CatUtils.ShiftJIS;
			StringsScraper.ResourceScrape("cs2_open.exe", "strings/cs2");
			Console.WriteLine("FINISHED");
			Console.ReadLine();*/
			StringsScraper.BinaryScrape("cs2_open.exe", "strings/cs2", ranges);
			Console.WriteLine("FINISHED");
			Console.ReadLine();
			//StringsScraper.BinarySearch("cs2_open.exe", "--全て表示--");//596368
			//StringsScraper.BinaryScrape("cs2_open.exe", "strings/cs2", 0x596368, 0x596388);
			// 0x5990E0, 0x5990E4 (?)

			// 0x599500, 0x5997C0
			// 0x5993E0, 0x599470
			// 0x5992AC, 0x599384
			// 0x599204, 0x599220
			// 0x598F34, 0x5990CC
			// 0x598DEC, 0x598F20
			// 0x598BA8, 0x598C04
			// 0x598988, 0x598B14
			// 0x598370, 0x5988B4
			// 0x598270, 0x598328
			// 0x598130, 0x5981C0
			// 0x597FC8, 0x5980EC
			// 0x597E78, 0x597FB4
			// 0x597DB0, 0x597E10
			// 0x597C20, 0x597D84
			// 0x59797C, 0x597AF8
			// 0x597380, 0x59754C
			// 0x596F9C, 0x597334
			// 0x596970, 0x596C44
			// 0x596638, 0x5967A4
			// 0x5963B8, 0x59645C
			// 0x596368, 0x596388
			// 0x5962C4, 0x596334
			// 0x59259C, 0x59263C
			// 0x5920E8, 0x59211C
			// 0x591FD8, 0x592000
			// 0x591D3C, 0x591DD0
			// 0x591C88, 0x591D18
			// 0x591BD4, 0x591C64
			// 0x591B04, 0x591BA0
			// 0x5919F0, 0x591AE0
			// 0x591944, 0x5919CC
			// 0x591864, 0x591890
			// 0x5917B0, 0x5917E8
			// 0x5915E8, 0x59164C
			// 0x5914F4, 0x59158C
			// 0x59126C, 0x59144C
			// 0x5911FC, 0x59122C
			// 0x591160, 0x5911BC
			// 0x590E14, 0x591120
			// 0x590CA0, 0x590D9C
			// 0x590C14, 0x590C38
			// 0x590960, 0x590B7C
			// 0x58CFA0, 0x58CFC0

			// 0x5B4FB4, 0x5B50D4
			// 0x5B464C, 0x5B4850
			// 0x5B4518, 0x5B4534
			// 0x5B4324, 0x5B4420
			// 0x5B3578, 0x5B35B4
			// 0x5B3480, 0x5B34B8
			// 0x5B2BC8, 0x5B2BD8
			// 0x5B2844, 0x5B28DC
			// 0x5B25E8, 0x5B27D0
			// 0x5B0A48, 0x5B0AAC
			// 0x5B0814, 0x5B0880
			// 0x5B02EC, 0x5B03D0
			// 0x5AFD00, 0x5AFE24
			// 0x5AFC20, 0x5AFC48

			// 0x, 0x
			// 0x, 0x
			// 0x, 0x
			// 0x, 0x
			// 0x, 0x
			// 0x, 0x
			// 0x, 0x
			// 0x, 0x
			// 0x, 0x
			// 0x, 0x
			// 0x, 0x
			// 0x, 0x
			// 0x, 0x
			// 0x, 0x
			Console.WriteLine("FINISHED");
			Console.ReadLine();
			/*StringsScraper.BinaryScrape("mc.exe", "strings/mc", 0x1AEE8, 0x1B1F4);
			Console.Read();
			Environment.Exit(0);*/
			//StringsScraper.BinarySearch("fes.exe", "画面スクリプト(fes)コンバータ fes.exe\n");
			//StringsScraper.BinaryScrape("fes.exe", "strings/fes", 0x3D828, 0x3DC68);
			//Console.Beep();
			//Console.ReadLine();
			ACPatcher ac = new ACPatcher {
				InstallDir = ".",
			};
			Console.WriteLine(ac.Patch());
			//Console.ReadLine();
			//Console.Clear();
			MCPatcher mc = new MCPatcher {
				InstallDir = ".",
			};
			Console.WriteLine(mc.Patch());
			//Console.ReadLine();
			//Console.Clear();
			FESPatcher fes = new FESPatcher {
				InstallDir = ".",
			};
			Console.WriteLine(fes.Patch());
			//WGCPatcher wgc = new WGCPatcher {
			//	InstallDir = ".",
			//};
			//Console.WriteLine(wgc.Patch());
			Console.ReadLine();
		}
	}
}
