using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher;
using TriggersTools.CatSystem2.Patcher.Patches;

namespace TriggersTools.CatSystem2.Testing {
	class Program {
		static void Main(string[] args) {
			//StringsScraper.BinaryValidate(File.ReadAllLines("strings/cs2/binary_5.txt"));
			CS2Patcher cs2 = new CS2Patcher();
			//Console.WriteLine(cs2.Patch("cs2_open.exe", "cs2_open_en.exe"));
			Console.WriteLine(cs2.Patch("Grisaia2.bin", "Grisaia2_en.bin"));
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
			CS2Patcher cs2 = new CS2Patcher();
			Console.WriteLine(cs2.Patch("cs2_open.exe", "cs2_open_en.exe"));
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
			ACPatcher ac = new ACPatcher();
			Console.WriteLine(ac.Patch("ac.exe", "ac_en.exe"));
			//Console.ReadLine();
			//Console.Clear();
			MCPatcher mc = new MCPatcher();
			Console.WriteLine(mc.Patch("mc.exe", "mc_en.exe"));
			//Console.ReadLine();
			//Console.Clear();
			FESPatcher fes = new FESPatcher();
			Console.WriteLine(fes.Patch("fes.exe", "fes_en.exe"));
			WGCPatcher wgc = new WGCPatcher();
			Console.WriteLine(wgc.Patch("WGC.exe", "WGC_en.exe"));
			Console.ReadLine();
		}
	}
}
