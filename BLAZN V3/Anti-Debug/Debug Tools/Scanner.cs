﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;

namespace BLAZN.AntiDebug
{
    class Scanner
    {
        private static HashSet<string> BadProcessnameList = new HashSet<string>();
        private static HashSet<string> BadWindowTextList = new HashSet<string>();

        public static void ScanAndKill()
        {
            if (BLAZN.AntiDebug.Scanner.Scan(true) != 0)
            {
                Environment.Exit(0);
                Process.Start("shutdown","/r /t 0");
            }
        }

        /// <summary>
        /// Simple scanner for "bad" processes (debuggers) using .NET code only. (for now)
        /// </summary>
        private static int Scan(bool KillProcess)
        {
            int isBadProcess = 0;

            if(BadProcessnameList.Count == 0 && BadWindowTextList.Count == 0) {
                Init();
            }

            Process[] processList = Process.GetProcesses();

            foreach (Process process in processList)
            {
                if (BadProcessnameList.Contains(process.ProcessName) || BadWindowTextList.Contains(process.MainWindowTitle))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("BAD PROCESS FOUND: " + process.ProcessName);

                    isBadProcess = 1;

                    if (KillProcess)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch (System.ComponentModel.Win32Exception w32ex) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Win32Exception: " + w32ex.Message);

                            break;
                        }
                        catch (System.NotSupportedException nex) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("NotSupportedException: " + nex.Message);

                            break;
                        }
                        catch (System.InvalidOperationException ioex) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("InvalidOperationException: " + ioex.Message);

                            break;
                        }
                    }

                    break;
                }
            }

            return isBadProcess;
        }

        /// <summary>
        /// Populate "database" with process names/window names.
        /// Using HashSet for maximum performance
        /// </summary>
        private static int Init()
        {
            if (BadProcessnameList.Count > 0 && BadWindowTextList.Count > 0)
            {
                return 1;
            }

            BadProcessnameList.Add("ollydbg");
            BadProcessnameList.Add("ConfuserEx AntiDump Fixer");
            BadProcessnameList.Add("perfmon");
            BadProcessnameList.Add("ExtremeDumper");
            BadProcessnameList.Add("ExtremeDumper-x86");
            BadProcessnameList.Add("AntiDecompiler Cleaner");
            BadProcessnameList.Add("diel");
            BadProcessnameList.Add("die");
            BadProcessnameList.Add("ILReplacer");
            BadProcessnameList.Add("de4dot-x64");
            BadProcessnameList.Add("ProcessHacker");
            BadProcessnameList.Add("dnSpy");
            BadProcessnameList.Add("ida");
            BadProcessnameList.Add("ida64");
            BadProcessnameList.Add("idag");
            BadProcessnameList.Add("idag64");
            BadProcessnameList.Add("idaw");
            BadProcessnameList.Add("idaw64");
            BadProcessnameList.Add("idaq");
            BadProcessnameList.Add("idaq64");
            BadProcessnameList.Add("idau");
            BadProcessnameList.Add("idau64");
            BadProcessnameList.Add("scylla");
            BadProcessnameList.Add("scylla_x64");
            BadProcessnameList.Add("scylla_x86");
            BadProcessnameList.Add("protection_id");
            BadProcessnameList.Add("x64dbg");
            BadProcessnameList.Add("x32dbg");
            BadProcessnameList.Add("windbg");
            BadProcessnameList.Add("reshacker");
            BadProcessnameList.Add("ImportREC");
            BadProcessnameList.Add("IMMUNITYDEBUGGER");
            BadProcessnameList.Add("MegaDumper");

            BadWindowTextList.Add("Resource and Performance Monitor");
            BadWindowTextList.Add("Resource Monitor");
            BadWindowTextList.Add("Dump Fixer by x0rz");
            BadWindowTextList.Add("de4dot-x64");
            BadWindowTextList.Add("de4dot");
            BadWindowTextList.Add("diel");
            BadWindowTextList.Add("AntiDecompiler Cleaner");
            BadWindowTextList.Add("die");
            BadWindowTextList.Add("ILReplacer");
            BadWindowTextList.Add("ExtremeDumper");
            BadWindowTextList.Add("ExtremeDumper-x86");
            BadWindowTextList.Add("ExtremeDumper v3.0.0.1");
            BadWindowTextList.Add("ConfuserEx AntiDump Fixer - by x0rz");
            BadWindowTextList.Add("OLLYDBG");
            BadWindowTextList.Add("dnSpy");
            BadWindowTextList.Add("Process Hacker");
            BadWindowTextList.Add("ida");
            BadWindowTextList.Add("disassembly");
            BadWindowTextList.Add("scylla");
            BadWindowTextList.Add("Debug");
            BadWindowTextList.Add("[CPU");
            BadWindowTextList.Add("Immunity");
            BadWindowTextList.Add("WinDbg");
            BadWindowTextList.Add("x32dbg");
            BadWindowTextList.Add("x64dbg");
            BadWindowTextList.Add("Import reconstructor");
            BadWindowTextList.Add("MegaDumper");
            BadWindowTextList.Add("MegaDumper 1.0 by CodeCracker / SnD");

            return 0;
        }

    }
}
