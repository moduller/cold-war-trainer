using BLAZN.Utilities;
using System;
using System.Diagnostics;

namespace BLAZN.Global
{

    using static BLAZN.Global.Variables;
    using static BLAZN.Global.Offsets;

    internal class address
    {
        public static void addressThread()
        {
            memory memory = new memory();
            baseadd = memory.GetBaseAddress("BlackOpsColdWar").ToInt64();
            memory.AttackProcess("BlackOpsColdWar");

            PPedPtr = memory.GetPointerInt(baseadd + PB + 0x8, new long[1], 1);

            skipround1 = PPedPtr + SkipRound;
            skipround2 = skipround1 + 1528L;
            skipround3 = skipround1 + 3056L;
            skipround4 = skipround1 + 4584L;
            skipround5 = skipround1 + 6112L;
            skipround6 = skipround1 + 7640L;

            //SKIP ROUNDS//
            for (int b2 = 0; b2 < skip.Length; b2++)
            {
                skip[b2] = skipround1 + ZM_Bot_ArraySize_Offset * (long)b2;
            }

            for (int b3 = 0; b3 < skip2.Length; b3++)
            {
                skip2[b3] = skipround2 + ZM_Bot_ArraySize_Offset * (long)b3;
            }

            for (int b4 = 0; b4 < skip3.Length; b4++)
            {
                skip3[b4] = skipround3 + ZM_Bot_ArraySize_Offset * (long)b4;
            }

            for (int b5 = 0; b5 < skip4.Length; b5++)
            {
                skip4[b5] = skipround4 + ZM_Bot_ArraySize_Offset * (long)b5;
            }

            for (int b6 = 0; b6 < skip5.Length; b6++)
            {
                skip5[b6] = skipround5 + ZM_Bot_ArraySize_Offset * (long)b6;
            }

            for (int b7 = 0; b7 < skip6.Length; b7++)
            {
                skip6[b7] = skipround6 + ZM_Bot_ArraySize_Offset * (long)b7;
            }

        }
        public static long baseadd = 0;
    }
}
