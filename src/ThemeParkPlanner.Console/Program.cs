//TODO: convert me
using System;
using System.Collections.Generic;
using System.IO;

namespace ThemeParkPlanner
{
    internal class Program
    {

#if DEVELOP

        private static IEnumerable<string> TestCases
        {
            get
            {
                ////TODO: does take a little longer than expected to complete: could look at a C++ solution instead
                // Expected output:
                // 176
                yield return @"2 9
95 91 88 77 67 53 52 46 46
85 82 72 64 61 52 40 40 28
1
0
2
0 1";

                // Expected output:
                // 75
                // 50
                // IMPOSSIBLE
                yield return @"5 3
30 60 75
30 15 30
30 45 60
60 45 15
99 62 99
3
0
3
0 1 2
55
1
3
119
1
4";
            }
        }

#endif

        private static void Main(string[] args)
        {

#if DEVELOP

            var sep = false;

            foreach (var testCase in TestCases)
            {
                using (var reader = new StringReader(testCase))
                {
                    if (sep)
                        Console.Out.WriteLine("============================================");

                    using (new Planner(reader, Console.Out))
                    {
                    }

                    sep = true;
                }
            }

#else

            using (new Planner(reader, Console.Out))
            {
            }

#endif

        }
    }
}
