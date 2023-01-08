using System;

namespace BLAZN.Global
{
    public static class Offsets
    {

        //*BASE OFFSETS (NEEDS SIGS)*//
        public static IntPtr PlayerBase = (IntPtr)0x109BCEA0;
        public static IntPtr ZMXPScaleBase = (IntPtr)0x109E4E98;
        public static IntPtr WPXPScaleBase = (IntPtr)0x109E4E98;
        public static IntPtr CMDBufferBase = (IntPtr)0x121AA390;
        public static long JumpHeight = 0x10AB3BD8;

        //DO NOT FORGET TO CHANGE THESE//
        public static long CamoBase = 0x10DF1880;
        public static long PB = 0x109E4E98; //< PLAYERBASE

        public static IntPtr PlayerCompPtr, PlayerPedPtr, ZMGlobalBase, ZMBotBase, ZMBotListBase, ZMXPScalePtr, JumpHeightPtr, GunAddress;
        public static long DefPtr, P2DefPtr, P3DefPtr, P4DefPtr;
        public const int SkipRound = 0x333D8;
        public const int CMDBB_Exec = -0x1B;

        //*PLAYERCOMPPTR OFFSETS*//
        public const int PC_ArraySize_Offset = 0xB940;
        public const int PC_CurrentUsedWeaponID = 0x28;
        public const int PC_SetWeaponID = 0xB0; // +(1-5 * 0x40 for WP2 to WP6)
        public const int PC_SetWeaponID2 = 0xF0;
        public const int PC_InfraredVision = 0xE66; // (byte) On=0x10|Off=0x0
        public const int PC_GodMode = 0xE67; // (byte) On=0xA0|Off=0x20
        public const int PC_Coords = 0xDE8;
        public const int PC_RapidFire1 = 0xE6C;
        public const int PC_RapidFire2 = 0xE80;
        public const int PC_MaxAmmo = 0x1360; // +(1-5 * 0x8 for WP1 to WP6)
        public const int PC_Ammo = 0x13D4; // +(1-5 * 0x4 for WP1 to WP6)
        public const int PC_Points = 0x5D14;
        public const int PC_Name = 0x5C0A;
        public const int PC_RunSpeed = 0x5C60;
        public const int PC_ClanTags = 0x605C;
        public const int PC_ReadyState1 = 0xE8;
        public const int PC_TeamID = 0x220;
        public const int PC_NumShots = 0xFE4;
        public const int PC_KillCount = 0x5D38;

        public const int client_size = 0x24FFD;

        public const int PC_Crit = 0x10D6;

        public const int PC_CritKill1 = 0x10D6;
        public const int PC_CritKill2 = 0x10D2;
        public const int PC_CritKill3 = 0x10E4;
        public const int PC_CritKill4 = 0x10E8;
        public const int PC_CritKill5 = 0x10C4;
        public const int PC_CritKill6 = 0x10C8;
        public const int PC_CritKill7 = 0x10D4;
        public const int PC_CritKill8 = 0x10D6;

        //*SKIPROUNDPTR OFFSETS*//
        public const int SkipRoundOne = 0x5F8;
        public const int SkipRoundTwo = 0xBF0;
        public const int SkipRoundThree = 0x11E8;
        public const int SkipRoundFour = 0x17E0;
        public const int SkipRoundFive = 0x5F8;
        public const int SkipRoundSix = 0x1DD8;

        //*PLAYERPEDPTR OFFSETS*//
        public const int PP_ArraySize_Offset = 0x5F8;
        public const int PP_Health = 0x398;
        public const int PP_MaxHealth = 0x39C;
        public const int PP_Coords = 0x2D4;
        public const int PP_Heading_Z = 0x34;
        public const int PP_Heading_XY = 0x38;

        //*ZMGLOBAL & BOTBASE OFFSETS*//
        public const int ZM_Global_ZombiesIgnoreAll = 0x14;
        public const int ZM_Global_ZMLeftCount = 0x3C;
        public const int ZM_Bot_List_Offset = 0x8;
        public const int ZM_Bot_ArraySize_Offset = 0x5F8;

        public const int ZM_Bot_Health = 0x398;
        public const int ZM_Bot_MaxHealth = 0x39C;
        public const int ZM_Bot_Coords = 0x2D4;

        //*XP OFFSETS*//
        public const int XPEP_Offset = 0x08;
        public const int XPUNK01_Offset = 0x24;
        public const int XPEP_RealAdd_Offset = 0x28;
        public const int XPUNK03_Offset = 0x2c;
        public const int XPGun_Offset = 0x10;
        public const int XPUNK04_Offset = 0x34;
        public const int XPUNK05_Offset = 0x38;
        public const int XPUNK06_Offset = 0x3c;
        public const int XPUNK07_Offset = 0x40;
        public const int XPUNK08_Offset = 0x44;
        public const int XPUNK09_Offset = 0x48;
        public const int XPUNK10_Offset = 0x4C;
    }
}
