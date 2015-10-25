namespace LeftOrRight
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class Program
    {
        private static IEnumerable<string> TestCases
        {
            get
            {
                // Expected output:
                // 4
                // 12
                // 26
                yield return @"6
4 2 5 1 3 8
3
2
0 1
3
0 5 3
4
0 4 2 5";

                // Expected output:
                // 38
                // 35
                yield return @"7
5 4 6 3 7 2 8
2
4
0 4 6 3
4
0 5 1 3";
            }
        }

        static void Main(string[] args)
        {

#if DEVELOP

            foreach (var testCase in TestCases)
            {
                using (var reader = new StringReader(testCase))
                {
                    using (new LeftOrRightChallenge(reader, Console.Out))
                    {
                    }
                }
            }

#else

            using (new LeftOrRightChallenge(Console.In, Console.Out))
            {
            }

#endif

        }
    }
}
