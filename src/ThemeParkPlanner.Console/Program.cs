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

            foreach (var testCase in TestCases)
            {
                using (var reader = new StringReader(testCase))
                {
                    using (new Planner(reader, Console.Out))
                    {
                    }
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
