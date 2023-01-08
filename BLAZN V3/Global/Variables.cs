using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BLAZN.Global
{
	public static class Variables
	{
		//*MAIN FORM*//
		public static Point lastPointMain;

		//*MEMORY VARAIBLES*//
		public static string TargetProcessName = "BlackOpsColdWar";
		public static Process gameProc;
		public static int gamePID = 0;
		public static long BaseAddress = 0;
		public static int ProcessID;

		//*EXTERNAL INTPTRS*//
		public static IntPtr proc;
		public static IntPtr baseAddress = IntPtr.Zero;

		//*TOOL VARIABLES*//
		public static Vector3 updatedPlayerPos1 = Vector3.Zero;
		public static Vector3 updatedPlayerPos2 = Vector3.Zero;
		public static Vector3 updatedPlayerPos3 = Vector3.Zero;
		public static Vector3 updatedPlayerPos4 = Vector3.Zero;
		public static Vector3 playerPos = Vector3.Zero;

		//*SINGLES*//
		public static Single playerSpeed = -1f;
		public static Single xpModifier = 1.0f;
		public static Single gunXpModifier = 1.0f;
		public static Single timeScaleMod = 1.0f;
		public static Single jhMod = 1.0f;
		public static int zmTeleportDistance = 150;

		public static float xpvalue;
		public static float wpxpvalue;
		public static float speedvalue;
		public static float jumpheightvalue;

		public static string xpval = xpvalue.ToString();
		public static string wpxpval = wpxpvalue.ToString();
		public static string speedval = speedvalue.ToString();
		public static string jumpheightval = jumpheightvalue.ToString();


		//*DM CHORDS*//
		public static Vector3 spawnpos = Vector3.Zero;
		public static Vector3 mbspawn = Vector3.Zero;
		public static Vector3 mbnacht = Vector3.Zero;
		public static Vector3 mbairplane = Vector3.Zero;
		public static Vector3 mbswamp = Vector3.Zero;
		public static Vector3 mbpr = Vector3.Zero;
		public static Vector3 mbl1 = Vector3.Zero;
		public static Vector3 mbl2 = Vector3.Zero;
		public static Vector3 spawn = Vector3.Zero;
		public static Vector3 airplane = Vector3.Zero;
		public static Vector3 snipers = Vector3.Zero;
		public static Vector3 swamp = Vector3.Zero;
		public static Vector3 pap = Vector3.Zero;
		public static Vector3 powerroom = Vector3.Zero;
		public static Vector3 jails = Vector3.Zero;


		//*CAMOS*//

		public static long camoarmegold;
		public static long camoarmegold2;
		public static long p2camoarmegold;
		public static long p2camoarmegold2;
		public static long p3camoarmegold;
		public static long p3camoarmegold2;
		public static long p4camoarmegold;
		public static long p4camoarmegold2;

		public static long defis2;
		public static long defis3;
		public static long defis4;
		public static long defis5;
		public static long defis6;
		public static long defis7;

		public static long p2defis2;
		public static long p2defis3;
		public static long p2defis4;
		public static long p2defis5;
		public static long p2defis6;
		public static long p2defis7;

		public static long p3defis2;
		public static long p3defis3;
		public static long p3defis4;
		public static long p3defis5;
		public static long p3defis6;
		public static long p3defis7;

		public static long p4defis2;
		public static long p4defis3;
		public static long p4defis4;
		public static long p4defis5;
		public static long p4defis6;
		public static long p4defis7;

		public static long[] camoarme1 = new long[140];
		public static long[] camoarme2 = new long[140];
		public static long[] camoarme3 = new long[140];
		public static long[] camoarme4 = new long[140];
		public static long[] camoarme5 = new long[140];
		public static long[] camoarme6 = new long[140];
		public static long[] camoarme7 = new long[140];
		public static long[] camoarmegoldall = new long[140];
		public static long[] camoarmegoldall2 = new long[140];

		public static long[] p2camoarme1 = new long[140];
		public static long[] p2camoarme2 = new long[140];
		public static long[] p2camoarme3 = new long[140];
		public static long[] p2camoarme4 = new long[140];
		public static long[] p2camoarme5 = new long[140];
		public static long[] p2camoarme6 = new long[140];
		public static long[] p2camoarme7 = new long[140];
		public static long[] p2camoarmegoldall = new long[140];
		public static long[] p2camoarmegoldall2 = new long[140];

		public static long[] p3camoarme1 = new long[140];
		public static long[] p3camoarme2 = new long[140];
		public static long[] p3camoarme3 = new long[140];
		public static long[] p3camoarme4 = new long[140];
		public static long[] p3camoarme5 = new long[140];
		public static long[] p3camoarme6 = new long[140];
		public static long[] p3camoarme7 = new long[140];
		public static long[] p3camoarmegoldall = new long[140];
		public static long[] p3camoarmegoldall2 = new long[140];

		public static long[] p4camoarme1 = new long[140];
		public static long[] p4camoarme2 = new long[140];
		public static long[] p4camoarme3 = new long[140];
		public static long[] p4camoarme4 = new long[140];
		public static long[] p4camoarme5 = new long[140];
		public static long[] p4camoarme6 = new long[140];
		public static long[] p4camoarme7 = new long[140];
		public static long[] p4camoarmegoldall = new long[140];
		public static long[] p4camoarmegoldall2 = new long[140];

		public static long[] camos = new long[140];
		public static long[] p2camos = new long[140];
		public static long[] p3camos = new long[140];
		public static long[] p4camos = new long[140];

		//*SKIP ROUNDS*//
		public static long PPedPtr;

		public static long skipround1;
		public static long skipround2;
		public static long skipround3;
		public static long skipround4;
		public static long skipround5;
		public static long skipround6;

		public static long[] skip = new long[50];
		public static long[] skip2 = new long[50];
		public static long[] skip3 = new long[50];
		public static long[] skip4 = new long[50];
		public static long[] skip5 = new long[50];
		public static long[] skip6 = new long[50];

	}
}
