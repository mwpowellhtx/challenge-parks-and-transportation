using System;
using System.Collections.Generic;
using System.IO;

namespace Transportation
{
    class Program
    {

#if DEVELOP

        private static IEnumerable<string> TestCases
        {
            get
            {
//                // Expected output:
//                // 06:00
//                // 05:04
//                // 05:04
//                // 05:19
//                // 07:14
//                // 07:22
//                // 07:25
//                // 23:19
//                // 23:56
//                yield return @"9
//4 8 3 12 8 2 2 3
//05:04 23:30
//06:00 23:55
//6
//05:00 15
//06:00 6
//08:30 10
//15:00 6
//18:30 10
//21:00 15
//11
//05:55 9 S
//05:00 0 N
//05:04 0 N
//05:05 0 N
//07:09 1 N
//07:17 2 N
//07:20 3 N
//23:10 0 N
//23:47 6 N";

//                // TODO: 23:59 was a typo from their web site
//                // Expected output:
//                // 05:04
//                // 05:04
//                // 23:49
//                yield return @"8
//4 8 3 12 8 2 3
//05:04 02:00
//06:00 02:50
//6
//05:00 15
//06:00 6
//08:30 10
//15:00 6
//21:00 15
//01:00 20
//3
//05:00 0 N
//02:05 0 N
//23:47 0 N";

                ////TODO: estimated output:
                // Expected output:
                // 05:00
                // 05:00
                // 24:00
                yield return @"8
4 8 3 12 8 2 3
05:00 02:00
06:00 02:50
6
05:00 15
06:00 6
08:30 10
15:00 6
21:00 15
01:00 15
3
05:00 0 N
02:05 0 N
23:47 0 N";
            }
        }

#endif

        static void Main(string[] args)
        {

#if DEVELOP

            foreach (var testCase in TestCases)
            {
                using (var reader = new StringReader(testCase))
                {
                    using (new LightRail(reader, Console.Out))
                    {
                        Console.WriteLine("=========================");
                    }
                }
            }

#else

            using (new LightRail(Console.In, Console.Out))
            {
            }

#endif

        }
    }
}
