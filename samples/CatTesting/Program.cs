using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggersTools.CatSystem2.Patcher;

namespace TriggersTools.CatSystem2.Testing {
	class Program {
		static void Main(string[] args) {
			Console.OutputEncoding = CatUtils.ShiftJIS;
			StringsScraper.ResourceScrape("cs2_open.exe", "strings/cs2");
			Console.WriteLine("FINISHED");
			Console.ReadLine();
			//StringsScraper.BinarySearch("cs2_open.exe", "--全て　表示--");
			//Console.WriteLine("FINISHED");
			//StringsScraper.BinarySearch("cs2_open.exe", "--全て表示--");//596368
			//StringsScraper.BinaryScrape("cs2_open.exe", "strings/cs2", 0x596368, 0x596388);
			CS2Patcher cs2 = new CS2Patcher();
			Console.WriteLine(cs2.Patch("cs2_open.exe", "cs2_open_en.exe"));
			// 0x599500, 0x5997C0
			// 0x5993E0, 0x599470
			// 0x5992AC, 0x599384
			// 0x599204, 0x599220
			// 0x5990E0, 0x5990E4 (?)
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
			// 0x, 0x
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
