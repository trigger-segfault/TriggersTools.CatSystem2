using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JpCmd {
	class Program {
		static void Main(string[] args) {
			Console.OutputEncoding = Encoding.GetEncoding(932);
			ProcessStartInfo startInfo = new ProcessStartInfo {
				UseShellExecute = false,
				FileName = "cmd",
			};
			using (var p = Process.Start(startInfo))
				p.WaitForExit();

		}
	}
}
