using System.Threading;

namespace Frontend {
    public class Program {
        static void Main(string[] args) {
            new Thread((new LidgrenServer()).Update).Start();
        }
    }
}
