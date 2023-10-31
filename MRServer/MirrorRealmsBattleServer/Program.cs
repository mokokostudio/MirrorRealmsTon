using System;
using System.Threading;
using System.Threading.Tasks;

namespace MR.BattleServer {
    class Program {
        static void Main(string[] args) {
            new BattleServer(12345, 12346, 12446);
            Thread.Sleep(int.MaxValue);
        }
    }
}
