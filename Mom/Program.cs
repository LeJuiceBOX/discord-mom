using System;
using System.Threading.Tasks;

namespace Mom {
	class Program {
		public static async Task Main(string[] args)
			=> await Startup.RunAsync(args);
	}
}
