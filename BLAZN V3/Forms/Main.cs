using System;
using System.Drawing;
using System.Windows.Forms;
using BLAZN.Global;
using BLAZN.AntiDebug;
using BLAZN.Utilities;
using System.Diagnostics;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace BLAZN
{
    using static Variables;
    using static Booleans;
    using static Offsets;
    using static DMChords;
    public partial class MainForm : Form
    {
        cmem mem = new cmem();
        memory m = new memory();

        public static int[] GunList = new int[] { 1295, 10521, 10498, 46362, 46364, 46392, 46398, 10579, 10570, 10604, 10595, 10534, 10605,
            10492, 1364, 45417, 1259, 45320, 46393, 45413, 45335, 24878, 24621, 10244, 10501, 10494, 10615, 10578, 11638, 228, 301, 272, 346, 368,
            332, 293, 345, 349, 259 };

        public int i1 = 0;
        public int i2 = 0;
        public int i3 = 0;
        public int i4 = 0;

        public int j1 = 0;
        public int j2 = 0;
        public int j3 = 0;
        public int j4 = 0;

        private int basea;

        string pname = Process.GetCurrentProcess().ProcessName;
        char[] letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        Random rand = new Random();

        public MainForm()
        {
            InitializeComponent();
        }

        //
        //*FORM FUNCTIONS*//
        //

        [DllImport("user32.dll")]
        private static extern bool SetWindowText(IntPtr hWnd, string text);

        private void randomAppName()
        {
            foreach (Process process in Process.GetProcessesByName(pname))
            {
                string word = "";
                for (int j = 1; j <= 12; j++)
                {
                    // Pick a random number between 0 and 25
                    int letter_num = rand.Next(0, letters.Length - 1);

                    // Append the letter.
                    word += letters[letter_num];
                }
                SetWindowText(process.MainWindowHandle, word);
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            Variables.lastPointMain = new Point(e.X, e.Y);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - Variables.lastPointMain.X;
                Top += e.Y - Variables.lastPointMain.Y;
            }
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MinimizeBtn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        public void playerUpdate(bool IA, bool GM, bool UP, bool AC, bool RF, bool WPC)
        {
            PlayerInfSwitch.Value = IA;
            PlayerGodmodeSwitch.Value = GM;
            PlayerUPSwitch.Value = UP;
            PlayerACSwitch.Value = AC;
            PlayerRFSwitch.Value = RF;
            PlayerWPCSwitch.Value = WPC;
        }


        //
        //*END OF FORM FUNCTIONS*//
        //


        //*FORM LOAD*//
        private void MainForm_Load(object sender, EventArgs e)
        {
            //*INITIALIZING PANEL LAYOUT*//
            SidePnl.Hide();
            RegisterPnl.Hide();
            GlobalPnl.Hide();
            PlayerPnl.Hide();
            ProPnl.Hide();
            ESPnl.Hide();
            GameAttach.Hide();

            LoginPnl.Show();
            LoginPnl.BringToFront();


            if (!AntiDebugWorker.IsBusy) AntiDebugWorker.RunWorkerAsync();
            if (Properties.Settings.Default.Username != string.Empty)
            {
                UsernameTxt.Text = Properties.Settings.Default.Username;
                PasswordTxt.Text = Properties.Settings.Default.Password;
                RMCheckbox.Checked = true;
            }
        }


        //
        //*END OF FORM LOAD*//
        //


        //*LOGIN API*//
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            login();
        }

        private void RRegister_Click(object sender, EventArgs e)
        {
            if (API.Register(RUsernameTxt.Text, RPasswordTxt.Text, REmailTxt.Text, RKeyTxt.Text))
            {
                //Put code here of what you want to do after successful login
                MessageBox.Show("Register has been successful! Go back to login!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RegisterPnl.Hide();
                LoginPnl.Show();
                LoginPnl.BringToFront();
            }
        }

        private void LRegisterBtn_Click(object sender, EventArgs e)
        {
            RegisterPnl.Show();
            LoginPnl.Hide();
        }

        private void LESBtn_Click(object sender, EventArgs e)
        {
            if (UsernameTxt.Text == "" || PasswordTxt.Text == "")
            {
                MessageBox.Show("You must enter your username and password to extend your subscription.", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ESPnl.Show();
                ESPnl.BringToFront();
                LESBtn.Hide();
                UsernameTxt.Hide();
                LUsernamePicture.Hide();
                LUsernameTxtPnl.Hide();
                PasswordTxt.Hide();
                LPasswordPicture.Hide();
                LPasswordPnl.Hide();
                LRegisterBtn.Hide();
                LoginBtn.Hide();
            }
        }

        private void BTLBtn_Click(object sender, EventArgs e)
        {
            LoginPnl.Show();
            LoginPnl.BringToFront();

            RegisterPnl.Hide();
        }

        private void ESExitBtn_Click(object sender, EventArgs e)
        {
            ESPnl.Hide();
            LESBtn.Show();
            UsernameTxt.Show();
            LUsernamePicture.Show();
            LUsernameTxtPnl.Show();
            PasswordTxt.Show();
            LPasswordPicture.Show();
            LPasswordPnl.Show();
            LRegisterBtn.Show();
            LoginBtn.Show();
        }

        private void ESBtn_Click(object sender, EventArgs e)
        {
            if (API.ExtendSubscription(UsernameTxt.Text, PasswordTxt.Text, ESKeyTxt.Text))
            {
                MessageBox.Show("Successfully extended subscription!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void login()
        {
            if (API.Login(UsernameTxt.Text, PasswordTxt.Text))
            {
                if (User.Rank == "0")
                {
                    MessageBox.Show("Login successful!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoginPnl.Hide();
                    PlayerPnl.Hide();
                    GlobalPnl.Show();
                    SidePnl.Show();

                    if (!BGWorker.IsBusy) BGWorker.RunWorkerAsync();
                    if (!TeleportWorker.IsBusy) TeleportWorker.RunWorkerAsync();
                    if (!AmmoWorker.IsBusy) AmmoWorker.RunWorkerAsync();
                    //*SAVES PASSWORD*//
                    if (RMCheckbox.Checked == true)
                    {
                        Properties.Settings.Default.Username = UsernameTxt.Text;
                        Properties.Settings.Default.Password = PasswordTxt.Text;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.Username = "";
                        Properties.Settings.Default.Password = "";
                        Properties.Settings.Default.Save();
                    }
                }
                else
                {
                    MessageBox.Show("Login successful!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoginPnl.Hide();
                    PlayerPnl.Hide();
                    GlobalPnl.Show();
                    SidePnl.Show();

                    ProSPBtn.Hide();
                    GlobalSPBtn.Location = new Point(0, 68);
                    PlayerPicture.Location = new Point(0, 117);
                    PlayerSelectDropdown.Location = new Point(15, 135);

                    //*SAVES PASSWORD*//
                    if (RMCheckbox.Checked == true)
                    {
                        Properties.Settings.Default.Username = UsernameTxt.Text;
                        Properties.Settings.Default.Password = PasswordTxt.Text;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.Username = "";
                        Properties.Settings.Default.Password = "";
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }

        //
        //*END OF LOGIN API*//
        //


        //*SIDE PANEL*//
        private void GlobalSPBtn_Click(object sender, EventArgs e)
        {
            GlobalPnl.Show();
            GlobalPnl.BringToFront();

            PlayerPnl.Hide();
            ProPnl.Hide();
        }

        private void PlayerSelectDropdown_onItemSelected(object sender, EventArgs e)
        {
            if (PlayerSelectDropdown.selectedIndex == 0)
            {
                P1 = true;
                P2 = false;
                P3 = false;
                P4 = false;
                PlayerPnl.Show();
                PlayerPnl.BringToFront();

                GlobalPnl.Hide();
                ProPnl.Hide();
                playerUpdate(P1IA, P1GM, P1UP, P1AC, P1RF, P1WPC);
            }

            if (PlayerSelectDropdown.selectedIndex == 1)
            {
                P1 = false;
                P2 = true;
                P3 = false;
                P4 = false;
                PlayerPnl.Show();
                PlayerPnl.BringToFront();

                GlobalPnl.Hide();
                ProPnl.Hide();
                playerUpdate(P2IA, P2GM, P2UP, P2AC, P2RF, P2WPC);
            }

            if (PlayerSelectDropdown.selectedIndex == 2)
            {
                P1 = false;
                P2 = false;
                P3 = true;
                P4 = false;
                PlayerPnl.Show();
                PlayerPnl.BringToFront();

                GlobalPnl.Hide();
                ProPnl.Hide();
                playerUpdate(P3IA, P3GM, P3UP, P3AC, P3RF, P3WPC);
            }

            if (PlayerSelectDropdown.selectedIndex == 3)
            {
                P1 = false;
                P2 = false;
                P3 = false;
                P4 = true;
                PlayerPnl.Show();
                PlayerPnl.BringToFront();

                GlobalPnl.Hide();
                ProPnl.Hide();
                playerUpdate(P4IA, P4GM, P4UP, P4AC, P4RF, P4WPC);
            }
        }

        private void ProSPBtn_Click(object sender, EventArgs e)
        {
            ProPnl.Show();
            ProPnl.BringToFront();

            PlayerPnl.Hide();
            GlobalPnl.Hide();
        }


        //
        //*END OF SIDE PANEL*//
        //


        //*GLOBAL PANEL*//
        private void GameXPSlider_ValueChanged(object sender, EventArgs e)
        {
            if (GameXPSlider.Value == 0)
            {
                xpModifier = 1.0f;
            }
            if (GameAttached && User.Username != string.Empty)
            {
                xpModifier = GameXPSlider.Value;
                xpvalue = GameXPSlider.Value;
                xpval = xpvalue.ToString();
                GameXPValue.Text = xpval;

                byte[] tempBuffer2 = new byte[4];
                Buffer.BlockCopy(BitConverter.GetBytes(xpModifier), 0, tempBuffer2, 0, 4);

                bmem.WriteProcessMemory(proc, (IntPtr)(baseAddress.ToInt64() + ZMXPScaleBase.ToInt64() + 0x20), tempBuffer2, 4, out _);
                bmem.WriteProcessMemory(proc, (IntPtr)(baseAddress.ToInt64() + ZMXPScaleBase.ToInt64() + 0x28), tempBuffer2, 4, out _);

            }
        }

        private void WPXPSlider_ValueChanged(object sender, EventArgs e)
        {
            if (WPXPSlider.Value == 0)
            {
                gunXpModifier = 1.0f;
            }
            if (GameAttached && User.Username != string.Empty)
            {
                gunXpModifier = WPXPSlider.Value;
                wpxpvalue = WPXPSlider.Value;
                wpxpval = wpxpvalue.ToString();
                WPXPValue.Text = wpxpval;

                byte[] tempBuffer1 = new byte[4];
                Buffer.BlockCopy(BitConverter.GetBytes(gunXpModifier), 0, tempBuffer1, 0, 4);

                bmem.WriteProcessMemory(proc, (IntPtr)(baseAddress.ToInt64() + ZMXPScaleBase.ToInt64() + 0x30), tempBuffer1, 4, out _);
                bmem.WriteProcessMemory(proc, (IntPtr)(baseAddress.ToInt64() + ZMXPScaleBase.ToInt64() + 0x30), tempBuffer1, 4, out _);
            }
        }


        private void SpeedSlider_ValueChanged(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (SpeedSlider.Value == 0)
                {
                    SpeedSlider.Value = 1;
                }
                playerSpeed = SpeedSlider.Value;
                speedvalue = SpeedSlider.Value;
                speedval = speedvalue.ToString();
                SpeedValue.Text = speedval;
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_RunSpeed), Convert.ToSingle(playerSpeed), 4, out _);
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_RunSpeed), Convert.ToSingle(playerSpeed), 4, out _);
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_RunSpeed), Convert.ToSingle(playerSpeed), 4, out _);
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_RunSpeed), Convert.ToSingle(playerSpeed), 4, out _);

            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SpeedSlider.Value = 0;
            }
        }

        private void JHSlider_ValueChanged(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                jhMod = JHSlider.Value;
                jumpheightvalue = JHSlider.Value;
                jumpheightval = jumpheightvalue.ToString();
                JHValue.Text = jumpheightval;
                m.WriteFloat(m.GetPointer(BaseAddress + JumpHeight, 0x130), JHSlider.Value);

            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                JHSlider.Value = 0;
            }
        }

        private void ALLINFSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (ALLINFSwitch.Value)
                {
                    ammoFrozen = true;
                    P1IA = true;
                    P2IA = true;
                    P3IA = true;
                    P4IA = true;
                    //InfAmmo.Start();
                }
                else
                {
                    P1IA = false;
                    P2IA = false;
                    P3IA = false;
                    P4IA = false;
                    ammoFrozen = false;
                    //InfAmmo.Stop();
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ALLINFSwitch.Value = false;
            }
        }

        private void ALLUPSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (ALLUPSwitch.Value)
                {
                    UnlimitedPoints = true;
                    P1UP = true;
                    P2UP = true;
                    P3UP = true;
                    P4UP = true;
                    UnlPoints.Start();
                }
                else
                {
                    UnlimitedPoints = false;
                    P1UP = false;
                    P2UP = false;
                    P3UP = false;
                    P4UP = false;
                    UnlPoints.Stop();
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ALLUPSwitch.Value = false;
            }
        }

        private void ALLGMSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (ALLGMSwitch.Value)
                {
                    GodMode = true;
                    P1GM = true;
                    P2GM = true;
                    P3GM = true;
                    P4GM = true;
                    //GM.Start();
                }
                else
                {
                    GodMode = false;
                    P1GM = false;
                    P2GM = false;
                    P3GM = false;
                    P4GM = false;
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ALLGMSwitch.Value = false;
            }
        }

        private void ALLRFSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (ALLRFSwitch.Value)
                {
                    RapidFire = true;
                    P1RF = true;
                    P2RF = true;
                    P3RF = true;
                    P4RF = true;
                    RapFire.Start();
                }
                else
                {
                    RapidFire = false;
                    P1RF = false;
                    P2RF = false;
                    P3RF = false;
                    P4RF = false;
                    RapFire.Stop();
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ALLRFSwitch.Value = false;
            }
        }

        private void InstaSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (InstaSwitch.Value)
                {
                    InstaKill = true;
                }
                else
                {
                    InstaKill = false;
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                InstaSwitch.Value = false;
            }
        }


        private void RestartGameBtn_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                string CMDCOMMAND = "full_restart;";
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EndMatchBtn_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                string CMDCOMMAND = "disconnect;";
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FMBBtn_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                string CMDCOMMAND = "magic_chest_movable 0;";
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
            }
            else
            {
                string CMDCOMMAND = "magic_chest_movable 1;";
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //
        //*END OF GLOBAL PANEL*//
        //


        //*PLAYER PANEL*//
        private void PlayerInfSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (P1)
                {
                    if (PlayerInfSwitch.Value)
                    {
                        P1IA = true;
                        P1InfAmmo.Start();
                    }
                    else
                    {
                        P1IA = false;
                        P1InfAmmo.Stop();
                    }
                }

                if (P2)
                {
                    if (PlayerInfSwitch.Value)
                    {
                        P2IA = true;
                        P2InfAmmo.Start();
                    }
                    else
                    {
                        P2IA = false;
                        P2InfAmmo.Stop();
                    }
                }

                if (P3)
                {
                    if (PlayerInfSwitch.Value)
                    {
                        P3IA = true;
                        P3InfAmmo.Start();
                    }
                    else
                    {
                        P3IA = false;
                        P3InfAmmo.Stop();
                    }
                }

                if (P4)
                {
                    if (PlayerInfSwitch.Value)
                    {
                        P4IA = true;
                        P4InfAmmo.Start();
                    }
                    else
                    {
                        P4IA = false;
                        P4InfAmmo.Stop();
                    }
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PlayerInfSwitch.Value = false;
            }
        }

        private void PlayerUPSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (P1)
                {
                    if (PlayerUPSwitch.Value)
                    {
                        P1UP = true;
                        P1UnlPoints.Start();
                    }
                    else
                    {
                        P1UP = false;
                        P1UnlPoints.Stop();
                    }
                }

                if (P2)
                {
                    if (PlayerUPSwitch.Value)
                    {
                        P2UP = true;
                        P2UnlPoints.Start();
                    }
                    else
                    {
                        P2UP = false;
                        P2UnlPoints.Stop();
                    }
                }

                if (P3)
                {
                    if (PlayerUPSwitch.Value)
                    {
                        P3UP = true;
                        P3UnlPoints.Start();
                    }
                    else
                    {
                        P3UP = false;
                        P3UnlPoints.Stop();
                    }
                }

                if (P4)
                {
                    if (PlayerUPSwitch.Value)
                    {
                        P4UP = true;
                        P4UnlPoints.Start();
                    }
                    else
                    {
                        P4UP = false;
                        P4UnlPoints.Stop();
                    }
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PlayerUPSwitch.Value = false;
            }
        }

        private void PlayerGodmodeSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (P1)
                {
                    if (PlayerGodmodeSwitch.Value)
                    {
                        P1GM = true;
                        P1GodMode.Start();
                    }
                    else
                    {
                        P1GM = false;
                    }
                }

                if (P2)
                {
                    if (PlayerGodmodeSwitch.Value)
                    {
                        P2GM = true;
                        P2GodMode.Start();
                    }
                    else
                    {
                        P2GM = false;
                    }
                }

                if (P3)
                {
                    if (PlayerGodmodeSwitch.Value)
                    {
                        P3GM = true;
                        P3GodMode.Start();
                    }
                    else
                    {
                        P3GM = false;
                    }
                }

                if (P4)
                {
                    if (PlayerGodmodeSwitch.Value)
                    {
                        P4GM = true;
                        P4GodMode.Start();
                    }
                    else
                    {
                        P4GM = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PlayerGodmodeSwitch.Value = false;
            }
        }

        private void PlayerACSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (P1)
                {
                    if (PlayerACSwitch.Value)
                    {
                        P1AC = true;
                        P1AlwaysCrit.Start();
                    }
                    else
                    {
                        P1AC = false;
                        P1AlwaysCrit.Stop();
                    }
                }

                if (P2)
                {
                    if (PlayerACSwitch.Value)
                    {
                        P2AC = true;
                        P2AlwaysCrit.Start();
                    }
                    else
                    {
                        P2AC = false;
                        P2AlwaysCrit.Stop();
                    }
                }

                if (P3)
                {
                    if (PlayerACSwitch.Value)
                    {
                        P3AC = true;
                        P3AlwaysCrit.Start();
                    }
                    else
                    {
                        P3AC = false;
                        P3AlwaysCrit.Stop();
                    }
                }

                if (P4)
                {
                    if (PlayerACSwitch.Value)
                    {
                        P4AC = true;
                        P4AlwaysCrit.Start();
                    }
                    else
                    {
                        P4AC = false;
                        P4AlwaysCrit.Stop();
                    }
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PlayerACSwitch.Value = false;
            }
        }

        private void PlayerRFSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (P1)
                {
                    if (PlayerRFSwitch.Value)
                    {
                        P1RF = true;
                        P1RapFire.Start();
                    }
                    else
                    {
                        P1RF = false;
                        P1RapFire.Stop();
                    }
                }

                if (P2)
                {
                    if (PlayerRFSwitch.Value)
                    {
                        P2RF = true;
                        P2RapFire.Start();
                    }
                    else
                    {
                        P2RF = false;
                        P2RapFire.Stop();
                    }
                }

                if (P3)
                {
                    if (PlayerRFSwitch.Value)
                    {
                        P3RF = true;
                        P3RapFire.Start();
                    }
                    else
                    {
                        P3RF = false;
                        P3RapFire.Stop();
                    }
                }

                if (P4)
                {
                    if (PlayerRFSwitch.Value)
                    {
                        P4RF = true;
                        P4RapFire.Start();
                    }
                    else
                    {
                        P4RF = false;
                        P4RapFire.Stop();
                    }
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PlayerRFSwitch.Value = false;
            }
        }

        private void PlayerTPCSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (PlayerTPCSwitch.Value)
                {
                    P1TPC = true;
                    P2TPC = false;
                    P3TPC = false;
                    P4TPC = false;
                    TP1 = false;
                    TP2 = false;
                    TP3 = false;
                    TP4 = false;

                }
                else
                {
                    P1TPC = false;
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PlayerTPCSwitch.Value = false;
            }
        }

        private void WPN1Dropdown_onItemSelected(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (P1)
                {
                    P1WPC = false;
                    PCYC1 = false;
                    ppWPN(0);
                }

                if (P2)
                {
                    P2WPC = false;
                    PCYC2 = false;
                    ppWPN(1);
                }

                if (P3)
                {
                    P3WPC = false;
                    PCYC3 = false;
                    ppWPN(2);
                }

                if (P4)
                {
                    P4WPC = false;
                    PCYC4 = false;
                    ppWPN(3);
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                WPN1Dropdown.selectedIndex = 0;
            }
        }

        private void WPN2Dropdown_onItemSelected(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (P1)
                {
                    P1WPC = false;
                    PCYC1 = false;
                    ppWPN2(0);
                }

                if (P2)
                {
                    P2WPC = false;
                    PCYC2 = false;
                    ppWPN2(1);
                }

                if (P3)
                {
                    P3WPC = false;
                    PCYC3 = false;
                    ppWPN2(2);
                }

                if (P4)
                {
                    P4WPC = false;
                    PCYC4 = false;
                    ppWPN2(3);
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                WPN2Dropdown.selectedIndex = 0;
            }
        }

        private void SetTPBtn_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (P1)
                {
                    TP1 = true;
                    TP2 = false;
                    TP3 = false;
                    TP4 = false;
                    P1TPC = false;
                    P2TPC = false;
                    P3TPC = false;
                    P4TPC = false;
                    PlayerTPCSwitch.Value = false;

                    // create new byte array for player coordinates, reads them, and then sets the XYZ coordinates accordingly
                    byte[] playerCoords = new byte[12];
                    bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(0) + PP_Coords), playerCoords, 12, out _);
                    var origxp1 = BitConverter.ToSingle(playerCoords, 0);
                    var origyp1 = BitConverter.ToSingle(playerCoords, 4);
                    var origzp1 = BitConverter.ToSingle(playerCoords, 8);
                    // updates the current playerposition with a Vector3 created from the xyz coordinates
                    updatedPlayerPos1 = new Vector3((float)Math.Round(origxp1, 4), (float)Math.Round(origyp1, 4), (float)Math.Round(origzp1, 4));
                }

                if (P2)
                {
                    TP2 = true;
                    TP1 = false;
                    TP3 = false;
                    TP4 = false;
                    P1TPC = false;
                    P2TPC = false;
                    P3TPC = false;
                    P4TPC = false;

                    byte[] playerCoords = new byte[12];
                    bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(1) + PP_Coords), playerCoords, 12, out _);
                    var origxp1 = BitConverter.ToSingle(playerCoords, 0);
                    var origyp1 = BitConverter.ToSingle(playerCoords, 4);
                    var origzp1 = BitConverter.ToSingle(playerCoords, 8);
                    // updates the current playerposition with a Vector3 created from the xyz coordinates
                    updatedPlayerPos2 = new Vector3((float)Math.Round(origxp1, 4), (float)Math.Round(origyp1, 4), (float)Math.Round(origzp1, 4));
                }

                if (P3)
                {
                    TP3 = true;
                    TP2 = false;
                    TP1 = false;
                    TP4 = false;
                    P1TPC = false;
                    P2TPC = false;
                    P3TPC = false;
                    P4TPC = false;

                    byte[] playerCoords = new byte[12];
                    bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(2) + PP_Coords), playerCoords, 12, out _);
                    var origxp1 = BitConverter.ToSingle(playerCoords, 0);
                    var origyp1 = BitConverter.ToSingle(playerCoords, 4);
                    var origzp1 = BitConverter.ToSingle(playerCoords, 8);
                    // updates the current playerposition with a Vector3 created from the xyz coordinates
                    updatedPlayerPos3 = new Vector3((float)Math.Round(origxp1, 4), (float)Math.Round(origyp1, 4), (float)Math.Round(origzp1, 4));
                }

                if (P4)
                {
                    TP4 = true;
                    TP2 = false;
                    TP1 = false;
                    TP3 = false;
                    P1TPC = false;
                    P2TPC = false;
                    P3TPC = false;
                    P4TPC = false;

                    byte[] playerCoords = new byte[12];
                    bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(3) + PP_Coords), playerCoords, 12, out _);
                    var origxp1 = BitConverter.ToSingle(playerCoords, 0);
                    var origyp1 = BitConverter.ToSingle(playerCoords, 4);
                    var origzp1 = BitConverter.ToSingle(playerCoords, 8);
                    // updates the current playerposition with a Vector3 created from the xyz coordinates
                    updatedPlayerPos4 = new Vector3((float)Math.Round(origxp1, 4), (float)Math.Round(origyp1, 4), (float)Math.Round(origzp1, 4));
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PlayerWPCSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (P1)
                {
                    if (PlayerWPCSwitch.Value)
                    {
                        P1WPC = true;
                        PCYC1 = false;
                        P1Cycle.Start();
                    }
                    else
                    {
                        P1WPC = false;
                        P1Cycle.Stop();
                    }
                }

                if (P2)
                {
                    if (PlayerWPCSwitch.Value)
                    {
                        P2WPC = true;
                        PCYC2 = false;
                        P2Cycle.Start();
                    }
                    else
                    {
                        P2WPC = false;
                        P1Cycle.Stop();
                    }
                }

                if (P3)
                {
                    if (PlayerWPCSwitch.Value)
                    {
                        P3WPC = true;
                        PCYC3 = false;
                        P3Cycle.Start();
                    }
                    else
                    {
                        P3WPC = false;
                        P3Cycle.Stop();
                    }
                }

                if (P4)
                {
                    if (PlayerWPCSwitch.Value)
                    {
                        P4WPC = true;
                        PCYC4 = false;
                        P4Cycle.Start();
                    }
                    else
                    {
                        P4WPC = false;
                        P4Cycle.Stop();
                    }
                }
            }
        }

        private void PlayerKickBtn_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (P1)
                {
                    string CMDCOMMAND = "kick " + mem.ReadString(PlayerCompPtr2(0) + (long)PC_Name) + ";";
                    mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                    mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                }
                if (P2)
                {
                    string CMDCOMMAND = "kick " + mem.ReadString(PlayerCompPtr2(1) + (long)PC_Name) + ";";
                    mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                    mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                }
                if (P3)
                {
                    string CMDCOMMAND = "kick " + mem.ReadString(PlayerCompPtr2(2) + (long)PC_Name) + ";";
                    mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                    mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                }
                if (P4)
                {
                    string CMDCOMMAND = "kick " + mem.ReadString(PlayerCompPtr2(3) + (long)PC_Name) + ";";
                    mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                    mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //
        //*END OF PLAYER PANEL*//
        //


        //*PRO PANEL*//
        private void OSGSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (OSGSwitch.Value)
                {
                    OSG = true;
                }
                else
                {
                    OSG = false;
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OSGSwitch.Value = false;
            }
        }


        private void P1OSGBtn_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (OSG)
                {
                    var Class_Start = m.GetPointerInt(BaseAddress + CamoBase, new long[] { 0x1122F6 }, 1);

                    for (int i = 0; i < 150; i++)
                    {
                        //player 1
                        this.m.WriteXBytes(Class_Start + (0x80 * (long)i), new byte[] { 0x09, 0xC3 });
                        this.m.WriteXBytes(Class_Start + 0x9 + (0x80 * (long)i), new byte[] { 0x09, 0xC3 });
                        this.m.WriteXBytes(Class_Start + 0x12 + (0x80 * (long)i), new byte[] { 0x09, 0xC3 });
                        this.m.WriteXBytes(Class_Start + 0x1C + (0x80 * (long)i), new byte[] { 0x22 });
                        this.m.WriteXBytes(Class_Start + 0x1D + (0x80 * (long)i), new byte[] { 0x22 });
                        this.m.WriteXBytes(Class_Start + 0x28 + (0x80 * (long)i), new byte[] { 0x18 });
                        this.m.WriteXBytes(Class_Start + 0x36 + (0x80 * (long)i), new byte[] { 0x0E });
                        this.m.WriteXBytes(Class_Start + 0x41 + (0x80 * (long)i), new byte[] { 0x09 });
                        this.m.WriteXBytes(Class_Start + 0x52 + (0x80 * (long)i), new byte[] { 0x09 });
                    }
                }
                else
                {
                    MessageBox.Show("You must turn on One Shot Gold to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                P1ONSG = false;
            }
        }

        private void P2OSGBtn_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (OSG)
                {
                    var Class_Start = m.GetPointerInt(BaseAddress + CamoBase, new long[] { 0x1122F6 }, 1);

                    for (int i = 0; i < 150; i++)
                    {
                        //player 2
                        this.m.WriteXBytes(Class_Start + (client_size * 1) + (0x80 * (long)i), new byte[] { 0xC3, 0x09 });
                        this.m.WriteXBytes(Class_Start + (client_size * 1) + 0x9 + (0x80 * (long)i), new byte[] { 0xC3, 0x09 });
                        this.m.WriteXBytes(Class_Start + (client_size * 1) + 0x12 + (0x80 * (long)i), new byte[] { 0xC3, 0x09 });
                        this.m.WriteXBytes(Class_Start + (client_size * 1) + 0x1C + (0x80 * (long)i), new byte[] { 0x22 });
                        this.m.WriteXBytes(Class_Start + (client_size * 1) + 0x1D + (0x80 * (long)i), new byte[] { 0x22 });
                        this.m.WriteXBytes(Class_Start + (client_size * 1) + 0x28 + (0x80 * (long)i), new byte[] { 0x18 });
                        this.m.WriteXBytes(Class_Start + (client_size * 1) + 0x36 + (0x80 * (long)i), new byte[] { 0x0E });
                        this.m.WriteXBytes(Class_Start + (client_size * 1) + 0x41 + (0x80 * (long)i), new byte[] { 0x09 });
                        this.m.WriteXBytes(Class_Start + (client_size * 1) + 0x52 + (0x80 * (long)i), new byte[] { 0x09 });
                    }
                }
                else
                {
                    MessageBox.Show("You must turn on One Shot Gold to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                P2ONSG = false;
            }
        }

        private void P3OSGBtn_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (OSG)
                {
                    var Class_Start = m.GetPointerInt(BaseAddress + CamoBase, new long[] { 0x1122F6 }, 1);

                    for (int i = 0; i < 150; i++)
                    {
                        //player 2
                        this.m.WriteXBytes(Class_Start + (client_size * 2) + (0x80 * (long)i), new byte[] { 0xC3, 0x09 });
                        this.m.WriteXBytes(Class_Start + (client_size * 2) + 0x9 + (0x80 * (long)i), new byte[] { 0xC3, 0x09 });
                        this.m.WriteXBytes(Class_Start + (client_size * 2) + 0x12 + (0x80 * (long)i), new byte[] { 0xC3, 0x09 });
                        this.m.WriteXBytes(Class_Start + (client_size * 2) + 0x1C + (0x80 * (long)i), new byte[] { 0x22 });
                        this.m.WriteXBytes(Class_Start + (client_size * 2) + 0x1D + (0x80 * (long)i), new byte[] { 0x22 });
                        this.m.WriteXBytes(Class_Start + (client_size * 2) + 0x28 + (0x80 * (long)i), new byte[] { 0x18 });
                        this.m.WriteXBytes(Class_Start + (client_size * 2) + 0x36 + (0x80 * (long)i), new byte[] { 0x0E });
                        this.m.WriteXBytes(Class_Start + (client_size * 2) + 0x41 + (0x80 * (long)i), new byte[] { 0x09 });
                        this.m.WriteXBytes(Class_Start + (client_size * 2) + 0x52 + (0x80 * (long)i), new byte[] { 0x09 });
                    }
                }
                else
                {
                    MessageBox.Show("You must turn on One Shot Gold to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                P3ONSG = false;
            }
        }

        private void P4OSGBtn_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (OSG)
                {
                    var Class_Start = m.GetPointerInt(BaseAddress + CamoBase, new long[] { 0x1122F6 }, 1);

                    for (int i = 0; i < 150; i++)
                    {
                        //player 2
                        this.m.WriteXBytes(Class_Start + (client_size * 3) + (0x80 * (long)i), new byte[] { 0xC3, 0x09 });
                        this.m.WriteXBytes(Class_Start + (client_size * 3) + 0x9 + (0x80 * (long)i), new byte[] { 0xC3, 0x09 });
                        this.m.WriteXBytes(Class_Start + (client_size * 3) + 0x12 + (0x80 * (long)i), new byte[] { 0xC3, 0x09 });
                        this.m.WriteXBytes(Class_Start + (client_size * 3) + 0x1C + (0x80 * (long)i), new byte[] { 0x22 });
                        this.m.WriteXBytes(Class_Start + (client_size * 3) + 0x1D + (0x80 * (long)i), new byte[] { 0x22 });
                        this.m.WriteXBytes(Class_Start + (client_size * 3) + 0x28 + (0x80 * (long)i), new byte[] { 0x18 });
                        this.m.WriteXBytes(Class_Start + (client_size * 3) + 0x36 + (0x80 * (long)i), new byte[] { 0x0E });
                        this.m.WriteXBytes(Class_Start + (client_size * 3) + 0x41 + (0x80 * (long)i), new byte[] { 0x09 });
                        this.m.WriteXBytes(Class_Start + (client_size * 3) + 0x52 + (0x80 * (long)i), new byte[] { 0x09 });
                    }
                }
                else
                {
                    MessageBox.Show("You must turn on One Shot Gold to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                P4ONSG = false;
            }
        }

        //
        //*END OF PRO PANEL*//
        //


        //*WORKERS*//
        private void AntiDebugWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                randomAppName();

                Scanner.ScanAndKill();

                if (DebugProtect1.PerformChecks() == 1)
                {
                    Environment.FailFast("");
                }

                if (DebugProtect2.PerformChecks() == 1)
                {
                    Environment.FailFast("");
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

        private void BGWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    address.addressThread();
                    var gameProcs = Process.GetProcessesByName("BlackOpsColdWar");
                    gameProc = gameProcs[0];
                    gamePID = gameProc.Id;

                    if (gameProcs.Length > 1)
                    {
                        GameAttached = true;
                    }
                    else
                    {
                        GameAttached = false;
                    }

                    if (gamePID > 0)
                    {
                        GameAttached = true;
                    }
                    else
                    {
                        GameAttached = false;
                    }

                    proc = bmem.OpenProcess(bmem.ProcessAccessFlags.All, false, gameProc.Id);

                    if (baseAddress != bmem.GetModuleBaseAddress(gameProc, "BlackOpsColdWar.exe"))
                        baseAddress = bmem.GetModuleBaseAddress(gameProc, "BlackOpsColdWar.exe");

                    if (PlayerCompPtr != bmem.FindDMAAddy(proc, (IntPtr)(baseAddress.ToInt64() + PlayerBase.ToInt64()), new int[] { 0 }))
                        PlayerCompPtr = bmem.FindDMAAddy(proc, (IntPtr)(baseAddress.ToInt64() + PlayerBase.ToInt64()), new int[] { 0 });

                    if (PlayerPedPtr != bmem.FindDMAAddy(proc, (IntPtr)(baseAddress.ToInt64() + PlayerBase.ToInt64() + 0x8), new int[] { 0 }))
                        PlayerPedPtr = bmem.FindDMAAddy(proc, (IntPtr)(baseAddress.ToInt64() + PlayerBase.ToInt64() + 0x8), new int[] { 0 });

                    if (ZMGlobalBase != bmem.FindDMAAddy(proc, (IntPtr)(baseAddress.ToInt64() + PlayerBase.ToInt64() + 0x60), new int[] { 0 }))
                        ZMGlobalBase = bmem.FindDMAAddy(proc, (IntPtr)(baseAddress.ToInt64() + PlayerBase.ToInt64() + 0x60), new int[] { 0 });

                    if (ZMBotBase != bmem.FindDMAAddy(proc, (IntPtr)(baseAddress.ToInt64() + PlayerBase.ToInt64() + 0x68), new int[] { 0 }))
                        ZMBotBase = bmem.FindDMAAddy(proc, (IntPtr)(baseAddress.ToInt64() + PlayerBase.ToInt64()) + 0x68, new int[] { 0 });

                    if (ZMBotBase != (IntPtr)0x0 && ZMBotBase != (IntPtr)0x68 && ZMBotListBase != bmem.FindDMAAddy(proc, ZMBotBase + ZM_Bot_List_Offset, new int[] { 0 }))
                        ZMBotListBase = bmem.FindDMAAddy(proc, ZMBotBase + ZM_Bot_List_Offset, new int[] { 0 });

                    if (GameAttached)
                    {
                        GameAttach.Show();
                    }
                    else
                    {
                        GameAttach.Hide();
                    }

                    string CMDCOMMAND = "phys_gravity 99;";
                    mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                    mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                    mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                    mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);
                    mem.WriteString((long)(IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64()), CMDCOMMAND);
                    mem.WriteBool(((IntPtr)(baseAddress.ToInt64() + CMDBufferBase.ToInt64())) + CMDBB_Exec, true);

                    if (playerSpeed <= 0)
                    {
                        byte[] plrSpd = new byte[4];
                        bmem.ReadProcessMemory(proc, PlayerCompPtr + PC_RunSpeed, plrSpd, 4, out _);
                        SpeedSlider.Value = Convert.ToInt32(BitConverter.ToSingle(plrSpd, 0));
                    }

                    // Player Names
                    if (P1)
                    {
                        string text = mem.ReadString(PlayerCompPtr2(0) + (long)PC_Name) ?? "";
                        if (text != "" && text != "UnnamedPlayer")
                        {
                            PlayerUsernameLabel.Text = text;
                            PlayerUsernameLabel.ForeColor = Color.White;
                        }
                        else
                        {
                            PlayerUsernameLabel.Text = "OFFLINE";
                            PlayerUsernameLabel.ForeColor = Color.White;
                        }
                    }

                    if (P2)
                    {
                        string text2 = mem.ReadString(PlayerCompPtr2(1) + (long)PC_Name) ?? "";
                        if (text2 != "" && text2 != "UnnamedPlayer")
                        {
                            PlayerUsernameLabel.Text = text2;
                            PlayerUsernameLabel.ForeColor = Color.White;
                        }
                        else
                        {
                            PlayerUsernameLabel.Text = "OFFLINE";
                            PlayerUsernameLabel.ForeColor = Color.White;
                        }
                    }

                    if (P3)
                    {
                        string text3 = mem.ReadString(PlayerCompPtr2(2) + (long)PC_Name) ?? "";
                        if (text3 != "" && text3 != "UnnamedPlayer")
                        {
                            PlayerUsernameLabel.Text = text3;
                            PlayerUsernameLabel.ForeColor = Color.White;
                        }
                        else
                        {
                            PlayerUsernameLabel.Text = "OFFLINE";
                            PlayerUsernameLabel.ForeColor = Color.White;
                        }
                    }

                    if (P4)
                    {
                        string text4 = mem.ReadString(PlayerCompPtr2(3) + (long)PC_Name) ?? "";
                        if (text4 != "" && text4 != "UnnamedPlayer")
                        {
                            PlayerUsernameLabel.Text = text4;
                            PlayerUsernameLabel.ForeColor = Color.White;
                        }
                        else
                        {
                            PlayerUsernameLabel.Text = "OFFLINE";
                            PlayerUsernameLabel.ForeColor = Color.White;
                        }
                    }

                    string textA = mem.ReadString(PlayerCompPtr2(0) + (long)PC_Name) ?? "";
                    if (textA != "" && textA != "UnnamedPlayer")
                    {
                        P1OSGBtn.Text = textA;
                    }
                    else
                    {
                        P1OSGBtn.Text = "OFFLINE";
                    }

                    string textB = mem.ReadString(PlayerCompPtr2(1) + (long)PC_Name) ?? "";
                    if (textB != "" && textB != "UnnamedPlayer")
                    {
                        P2OSGBtn.Text = textB;
                    }
                    else
                    {
                        P2OSGBtn.Text = "OFFLINE";
                    }

                    string textC = mem.ReadString(PlayerCompPtr2(2) + (long)PC_Name) ?? "";
                    if (textC != "" && textC != "UnnamedPlayer")
                    {
                        P3OSGBtn.Text = textC;
                    }
                    else
                    {
                        P3OSGBtn.Text = "OFFLINE";
                    }

                    string textD = mem.ReadString(PlayerCompPtr2(3) + (long)PC_Name) ?? "";
                    if (textD != "" && textD != "UnnamedPlayer")
                    {
                        P4OSGBtn.Text = textD;
                    }
                    else
                    {
                        P4OSGBtn.Text = "OFFLINE";
                    }

                    if (InstaKill)
                    {
                        for (int i = 0; i < 90; i++)
                        {
                            if (InstaSwitch.Value)
                            {
                                bmem.WriteProcessMemory(proc, ZMBotListBase + (ZM_Bot_ArraySize_Offset * i) + ZM_Bot_Health, 1, 4, out _);
                                bmem.WriteProcessMemory(proc, ZMBotListBase + (ZM_Bot_ArraySize_Offset * i) + ZM_Bot_MaxHealth, 1, 4, out _);
                            }
                        }
                    }

                    if (GodMode)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_GodMode), 0xA0, 1, out _);
                        }
                    }
                    Thread.Sleep(1);
                }
                catch (Exception)
                {

                }
            }
        }

        private void TeleportWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    if (!GameAttached) continue;

                    if (TP1)
                    {
                        for (int i = 0; i < 90; i++)
                        {
                            bmem.WriteProcessMemory(proc, ZMBotListBase + (ZM_Bot_ArraySize_Offset * i) + ZM_Bot_Coords, updatedPlayerPos1, 12, out _);
                            if (TP1 == false)
                            {
                                break;
                            }
                        }
                    }

                    if (TP2)
                    {
                        for (int i = 0; i < 90; i++)
                        {
                            bmem.WriteProcessMemory(proc, ZMBotListBase + (ZM_Bot_ArraySize_Offset * i) + ZM_Bot_Coords, updatedPlayerPos2, 12, out _);
                            if (TP2 == false)
                            {
                                break;
                            }
                        }
                    }

                    if (TP3)
                    {
                        for (int i = 0; i < 90; i++)
                        {
                            bmem.WriteProcessMemory(proc, ZMBotListBase + (ZM_Bot_ArraySize_Offset * i) + ZM_Bot_Coords, updatedPlayerPos3, 12, out _);
                            if (TP3 == false)
                            {
                                break;
                            }
                        }
                    }

                    if (TP4)
                    {
                        for (int i = 0; i < 90; i++)
                        {
                            bmem.WriteProcessMemory(proc, ZMBotListBase + (ZM_Bot_ArraySize_Offset * i) + ZM_Bot_Coords, updatedPlayerPos4, 12, out _);
                            if (TP4 == false)
                            {
                                break;
                            }
                        }
                    }

                    if (P1TPC)
                    {
                        byte[] playerCoords1 = new byte[12];
                        bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(0) + PP_Coords), playerCoords1, 12, out _);
                        var origxp1 = BitConverter.ToSingle(playerCoords1, 0);
                        var origyp1 = BitConverter.ToSingle(playerCoords1, 4);
                        var origzp1 = BitConverter.ToSingle(playerCoords1, 8);
                        // updates the current playerposition with a Vector3 created from the xyz coordinates
                        updatedPlayerPos1 = new Vector3((float)Math.Round(origxp1, 4), (float)Math.Round(origyp1, 4), (float)Math.Round(origzp1, 4));

                        byte[] enemyPosBuffer = new byte[12];

                        byte[] playerHeadingXY = new byte[4];
                        byte[] playerHeadingZ = new byte[4];
                        bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(0) + PP_Heading_XY), playerHeadingXY, 4, out _);
                        bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(0) + PP_Heading_Z), playerHeadingZ, 4, out _);

                        // some stack overflow magic to get the direction the player is facing and getting a position in front of the player
                        var pitch = -ConvertToRadians(BitConverter.ToSingle(playerHeadingZ, 0));
                        var yaw = ConvertToRadians(BitConverter.ToSingle(playerHeadingXY, 0));
                        var x = Convert.ToSingle(Math.Cos(yaw) * Math.Cos(pitch));
                        var y = Convert.ToSingle(Math.Sin(yaw) * Math.Cos(pitch));
                        var z = Convert.ToSingle(Math.Sin(pitch));

                        // im guessing just a straight up BitConverter.GetBytes could have worked for writing vector3s to memory instead of this kinda messy solution
                        var newEnemyPos = updatedPlayerPos1 + (new Vector3(x, y, z) * zmTeleportDistance);

                        Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPos.X), 0, enemyPosBuffer, 0, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPos.Y), 0, enemyPosBuffer, 4, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPos.Z), 0, enemyPosBuffer, 8, 4);

                        for (int i = 0; i < 90; i++)
                        {
                            bmem.WriteProcessMemory(proc, ZMBotListBase + (ZM_Bot_ArraySize_Offset * i) + ZM_Bot_Coords, enemyPosBuffer, 12, out _);
                            if (P1TPC == false)
                            {
                                break;
                            }
                        }
                    }

                    if (P2TPC)
                    {
                        byte[] playerCoords2 = new byte[12];
                        bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(1) + PP_Coords), playerCoords2, 12, out _);
                        var origxp2 = BitConverter.ToSingle(playerCoords2, 0);
                        var origyp2 = BitConverter.ToSingle(playerCoords2, 4);
                        var origzp2 = BitConverter.ToSingle(playerCoords2, 8);
                        // updates the current playerposition with a Vector3 created from the xyz coordinates
                        updatedPlayerPos2 = new Vector3((float)Math.Round(origxp2, 4), (float)Math.Round(origyp2, 4), (float)Math.Round(origzp2, 4));

                        byte[] enemyPosBufferp2 = new byte[12];

                        byte[] playerHeadingXYp2 = new byte[4];
                        byte[] playerHeadingZp2 = new byte[4];
                        bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(1) + PP_Heading_XY), playerHeadingXYp2, 4, out _);
                        bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(1) + PP_Heading_Z), playerHeadingZp2, 4, out _);

                        // some stack overflow magic to get the direction the player is facing and getting a position in front of the player
                        var pitch = -ConvertToRadians(BitConverter.ToSingle(playerHeadingZp2, 0));
                        var yaw = ConvertToRadians(BitConverter.ToSingle(playerHeadingXYp2, 0));
                        var x = Convert.ToSingle(Math.Cos(yaw) * Math.Cos(pitch));
                        var y = Convert.ToSingle(Math.Sin(yaw) * Math.Cos(pitch));
                        var z = Convert.ToSingle(Math.Sin(pitch));

                        // im guessing just a straight up BitConverter.GetBytes could have worked for writing vector3s to memory instead of this kinda messy solution
                        var newEnemyPosp2 = updatedPlayerPos2 + (new Vector3(x, y, z) * zmTeleportDistance);

                        Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPosp2.X), 0, enemyPosBufferp2, 0, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPosp2.Y), 0, enemyPosBufferp2, 4, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPosp2.Z), 0, enemyPosBufferp2, 8, 4);

                        for (int i = 0; i < 90; i++)
                        {
                            bmem.WriteProcessMemory(proc, ZMBotListBase + (ZM_Bot_ArraySize_Offset * i) + ZM_Bot_Coords, enemyPosBufferp2, 12, out _);
                            if (P2TPC == false)
                            {
                                break;
                            }
                        }
                    }

                    if (P3TPC)
                    {
                        byte[] playerCoords3 = new byte[12];
                        bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(2) + PP_Coords), playerCoords3, 12, out _);
                        var origxp3 = BitConverter.ToSingle(playerCoords3, 0);
                        var origyp3 = BitConverter.ToSingle(playerCoords3, 4);
                        var origzp3 = BitConverter.ToSingle(playerCoords3, 8);
                        // updates the current playerposition with a Vector3 created from the xyz coordinates
                        updatedPlayerPos3 = new Vector3((float)Math.Round(origxp3, 4), (float)Math.Round(origyp3, 4), (float)Math.Round(origzp3, 4));

                        byte[] enemyPosBufferp3 = new byte[12];

                        byte[] playerHeadingXYp3 = new byte[4];
                        byte[] playerHeadingZp3 = new byte[4];
                        bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(2) + PP_Heading_XY), playerHeadingXYp3, 4, out _);
                        bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(2) + PP_Heading_Z), playerHeadingZp3, 4, out _);

                        // some stack overflow magic to get the direction the player is facing and getting a position in front of the player
                        var pitch = -ConvertToRadians(BitConverter.ToSingle(playerHeadingZp3, 0));
                        var yaw = ConvertToRadians(BitConverter.ToSingle(playerHeadingXYp3, 0));
                        var x = Convert.ToSingle(Math.Cos(yaw) * Math.Cos(pitch));
                        var y = Convert.ToSingle(Math.Sin(yaw) * Math.Cos(pitch));
                        var z = Convert.ToSingle(Math.Sin(pitch));

                        // im guessing just a straight up BitConverter.GetBytes could have worked for writing vector3s to memory instead of this kinda messy solution
                        var newEnemyPosp3 = updatedPlayerPos3 + (new Vector3(x, y, z) * zmTeleportDistance);

                        Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPosp3.X), 0, enemyPosBufferp3, 0, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPosp3.Y), 0, enemyPosBufferp3, 4, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPosp3.Z), 0, enemyPosBufferp3, 8, 4);

                        for (int i = 0; i < 90; i++)
                        {
                            bmem.WriteProcessMemory(proc, ZMBotListBase + (ZM_Bot_ArraySize_Offset * i) + ZM_Bot_Coords, enemyPosBufferp3, 12, out _);
                            if (P3TPC == false)
                            {
                                break;
                            }
                        }
                    }

                    if (P4TPC)
                    {
                        byte[] playerCoords4 = new byte[12];
                        bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(3) + PP_Coords), playerCoords4, 12, out _);
                        var origxp4 = BitConverter.ToSingle(playerCoords4, 0);
                        var origyp4 = BitConverter.ToSingle(playerCoords4, 4);
                        var origzp4 = BitConverter.ToSingle(playerCoords4, 8);
                        // updates the current playerposition with a Vector3 created from the xyz coordinates
                        updatedPlayerPos4 = new Vector3((float)Math.Round(origxp4, 4), (float)Math.Round(origyp4, 4), (float)Math.Round(origzp4, 4));

                        byte[] enemyPosBufferp4 = new byte[12];

                        byte[] playerHeadingXYp4 = new byte[4];
                        byte[] playerHeadingZp4 = new byte[4];
                        bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(3) + PP_Heading_XY), playerHeadingXYp4, 4, out _);
                        bmem.ReadProcessMemory(proc, (IntPtr)(PlayerPedPtr2(3) + PP_Heading_Z), playerHeadingZp4, 4, out _);

                        // some stack overflow magic to get the direction the player is facing and getting a position in front of the player
                        var pitch = -ConvertToRadians(BitConverter.ToSingle(playerHeadingZp4, 0));
                        var yaw = ConvertToRadians(BitConverter.ToSingle(playerHeadingXYp4, 0));
                        var x = Convert.ToSingle(Math.Cos(yaw) * Math.Cos(pitch));
                        var y = Convert.ToSingle(Math.Sin(yaw) * Math.Cos(pitch));
                        var z = Convert.ToSingle(Math.Sin(pitch));

                        // im guessing just a straight up BitConverter.GetBytes could have worked for writing vector3s to memory instead of this kinda messy solution
                        var newEnemyPosp4 = updatedPlayerPos4 + (new Vector3(x, y, z) * zmTeleportDistance);

                        Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPosp4.X), 0, enemyPosBufferp4, 0, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPosp4.Y), 0, enemyPosBufferp4, 4, 4);
                        Buffer.BlockCopy(BitConverter.GetBytes(newEnemyPosp4.Z), 0, enemyPosBufferp4, 8, 4);

                        for (int i = 0; i < 90; i++)
                        {
                            bmem.WriteProcessMemory(proc, ZMBotListBase + (ZM_Bot_ArraySize_Offset * i) + ZM_Bot_Coords, enemyPosBufferp4, 12, out _);
                            if (P4TPC == false)
                            {
                                break;
                            }
                        }
                    }

                }
                catch (Exception)
                {

                }
            }
        }

        private void AmmoWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    if (!GameAttached) continue;

                    if (ammoFrozen)
                    {
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_Ammo + (1 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_Ammo + (2 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_Ammo + (3 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_Ammo + (4 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_Ammo + (5 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_Ammo + (6 * 0x4)), 30, 4, out _);

                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_Ammo + (1 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_Ammo + (2 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_Ammo + (3 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_Ammo + (4 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_Ammo + (5 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_Ammo + (6 * 0x4)), 30, 4, out _);

                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_Ammo + (1 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_Ammo + (2 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_Ammo + (3 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_Ammo + (4 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_Ammo + (5 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_Ammo + (6 * 0x4)), 30, 4, out _);

                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_Ammo + (1 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_Ammo + (2 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_Ammo + (3 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_Ammo + (4 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_Ammo + (5 * 0x4)), 30, 4, out _);
                        bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_Ammo + (6 * 0x4)), 30, 4, out _);
                    }

                    Thread.Sleep(1);
                }
                catch (Exception)
                {

                }
            }
        }


        //
        //*END OF WORKERS*//
        //


        //*MORE FUNCTIONS*//
        public void UpdateLabel(Label label, string text, string color = "Black")
        {
            if (InvokeRequired)
            {
                label.Invoke((MethodInvoker)delegate ()
                {
                    label.Text = text;
                    label.ForeColor = Color.FromName(color);
                });
                return;
            }
            label.Text = text;
            label.ForeColor = Color.FromName(color);
        }

        public string ToHex(object num)
        {
            return string.Format("0x{0:X}", num);
        }

        // converts a degree angle to radians, for the tp zombies feature
        public double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        private long PlayerCompPtr2(int index)
        {
            if (mem.IsProcessRunning(TargetProcessName))
            {
                return mem.ReadInt64(Variables.BaseAddress + (long)PlayerBase) + (long)(PC_ArraySize_Offset * index);
            }
            return 0;
        }

        private long PlayerPedPtr2(int index)
        {
            if (mem.IsProcessRunning(TargetProcessName))
            {
                return mem.ReadInt64(Variables.BaseAddress + (long)PlayerBase + 0x8) + (long)(PC_ArraySize_Offset * index);
            }
            return 0;
        }

        public void ppWPN(int i)
        {
            if (GameAttached && User.Username != string.Empty)
            {

                //XM4
                if (WPN1Dropdown.selectedIndex == 3)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 49, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + (1 * 0x4)), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + (1 * 0x8)), 20, 4, out _);
                }

                //AK47
                if (WPN1Dropdown.selectedIndex == 4)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 5, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }


                if (WPN1Dropdown.selectedIndex == 5)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 11, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 6)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 47, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 7)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 26, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 8)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 46, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 11)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 38, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 12)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 54, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 13)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 41, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 14)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 17, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 15)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 14, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 16)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 43, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 19)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 32, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 20)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 35, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 21)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 34, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 22)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 31, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 25)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 2, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 26)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 44, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 27)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 57, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 30)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 28, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 31)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 45, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 32)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 4, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 35)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 15, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 36)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 19, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 37)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 25, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 40)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 33, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 41)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 48, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 42)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 56, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 45)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 21, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 46)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 10, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 47)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 8, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 50)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 12, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 51)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 24, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN1Dropdown.selectedIndex == 52)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID), 27, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
            }
            else
            {
                WPN1Dropdown.selectedIndex = 0;
            }
        }

        public void ppWPN2(int i)
        {
            if (GameAttached && User.Username != string.Empty)
            {

                //XM4
                if (WPN2Dropdown.selectedIndex == 3)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 49, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + (1 * 0x4)), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + (1 * 0x8)), 20, 4, out _);
                }

                //AK47
                if (WPN2Dropdown.selectedIndex == 4)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 5, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }

                //KRIG 6
                if (WPN2Dropdown.selectedIndex == 5)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 11, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 6)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 47, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 7)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 26, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 8)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 46, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 11)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 38, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 12)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 54, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 13)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 41, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 14)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 17, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 15)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 14, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 16)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 43, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 19)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 32, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 20)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 35, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 21)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 34, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 22)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 31, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 25)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 2, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 26)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 44, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 27)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 57, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 30)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 28, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 31)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 45, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 32)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 4, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 35)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 15, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 36)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 19, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 37)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 25, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 40)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 33, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 41)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 48, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 42)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 56, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 45)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 21, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 46)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 10, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 47)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 8, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 50)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 12, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 51)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 24, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
                if (WPN2Dropdown.selectedIndex == 52)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_SetWeaponID) + 0x40, 27, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_Ammo + 0x4), 20, 4, out _);
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(i) + PC_MaxAmmo + 0x8), 20, 4, out _);
                }
            }
            else
            {
                WPN2Dropdown.selectedIndex = 0;
            }
        }

        //
        //*END OF MORE FUNCTIONS*//
        //


        //*TIMERS*//
        private void InfAmmo_Tick(object sender, EventArgs e)
        {

            if (!P1IA || !P2IA || !P3IA || !P4IA)
            {
                ALLINFSwitch.Value = false;
                ammoFrozen = false;
                InfAmmo.Stop();
            }
        }

        private void UnlPoints_Tick(object sender, EventArgs e)
        {

            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_Points), 420420, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_Points), 420420, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_Points), 420420, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_Points), 420420, 4, out _);

            if (!P1UP || !P2UP || !P3UP || !P4UP)
            {
                ALLUPSwitch.Value = false;
                UnlimitedPoints = false;
                UnlPoints.Stop();
            }
        }

        private void GodMode_Tick(object sender, EventArgs e)
        {
            if (GodMode)
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_GodMode), 0xA0, 1, out _);
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_GodMode), 0xA0, 1, out _);
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_GodMode), 0xA0, 1, out _);
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_GodMode), 0xA0, 1, out _);
            }
            else
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_GodMode), 0x20, 1, out _);
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_GodMode), 0x20, 1, out _);
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_GodMode), 0x20, 1, out _);
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_GodMode), 0x20, 1, out _);

            }

            if (!P1GM || !P2GM || !P3GM || !P4GM)
            {
                ALLGMSwitch.Value = false;
                GodMode = false;
                GM.Stop();
            }
        }

        private void RapFire_Tick(object sender, EventArgs e)
        {
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_RapidFire1), 0, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_RapidFire2), 0, 4, out _);

            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_RapidFire1), 0, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_RapidFire2), 0, 4, out _);

            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_RapidFire1), 0, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_RapidFire2), 0, 4, out _);

            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_RapidFire1), 0, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_RapidFire2), 0, 4, out _);

            if (!P1RF || !P2RF || !P3RF || !P4RF)
            {
                ALLRFSwitch.Value = false;
                RapidFire = false;
                RapFire.Stop();
            }

            Thread.Sleep(1);
        }

        private void P1InfAmmo_Tick(object sender, EventArgs e)
        {
            for (int i = 1; i < 6; i++)
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_Ammo + (i * 0x4)), 30, 4, out _);
            }
            Thread.Sleep(1);
        }

        private void P2InfAmmo_Tick(object sender, EventArgs e)
        {
            for (int i = 1; i < 6; i++)
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_Ammo + (i * 0x4)), 30, 4, out _);
            }
            Thread.Sleep(1);
        }

        private void P3InfAmmo_Tick(object sender, EventArgs e)
        {
            for (int i = 1; i < 6; i++)
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_Ammo + (i * 0x4)), 30, 4, out _);
            }
            Thread.Sleep(1);
        }

        private void P4InfAmmo_Tick(object sender, EventArgs e)
        {
            for (int i = 1; i < 6; i++)
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_Ammo + (i * 0x4)), 30, 4, out _);
            }
            Thread.Sleep(1);
        }

        private void P1UnlPoints_Tick(object sender, EventArgs e)
        {
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_Points), 420420, 4, out _);
            Thread.Sleep(1);
        }

        private void P2UnlPoints_Tick(object sender, EventArgs e)
        {
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_Points), 420420, 4, out _);
            Thread.Sleep(1);
        }

        private void P3UnlPoints_Tick(object sender, EventArgs e)
        {
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_Points), 420420, 4, out _);
            Thread.Sleep(1);
        }

        private void P4UnlPoints_Tick(object sender, EventArgs e)
        {
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_Points), 420420, 4, out _);
            Thread.Sleep(1);
        }

        private void P1GM_Tick(object sender, EventArgs e)
        {
            if (P1GM)
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_GodMode), 0xA0, 1, out _);
            }
            else
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_GodMode), 0x20, 1, out _);
            }

            Thread.Sleep(1);
        }

        private void P2GM_Tick(object sender, EventArgs e)
        {
            if (P2GM)
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_GodMode), 0xA0, 1, out _);
            }
            else
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_GodMode), 0x20, 1, out _);
            }
        }

        private void P3GM_Tick(object sender, EventArgs e)
        {
            if (P3GM)
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_GodMode), 0xA0, 1, out _);
            }
            else
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_GodMode), 0x20, 1, out _);
            }
        }

        private void P4GM_Tick(object sender, EventArgs e)
        {
            if (P4GM)
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_GodMode), 0xA0, 1, out _);
            }
            else
            {
                bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_GodMode), 0x20, 1, out _);
            }
        }

        private void P1RapFire_Tick(object sender, EventArgs e)
        {
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_RapidFire1), 0, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_RapidFire2), 0, 4, out _);
            Thread.Sleep(1);
        }

        private void P2RapFire_Tick(object sender, EventArgs e)
        {
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_RapidFire1), 0, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_RapidFire2), 0, 4, out _);
            Thread.Sleep(1);
        }

        private void P3RapFire_Tick(object sender, EventArgs e)
        {
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_RapidFire1), 0, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_RapidFire2), 0, 4, out _);
            Thread.Sleep(1);
        }

        private void P4RapFire_Tick(object sender, EventArgs e)
        {
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_RapidFire1), 0, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_RapidFire2), 0, 4, out _);
            Thread.Sleep(1);
        }

        private void P1AC_Tick(object sender, EventArgs e)
        {
            if (P1AC)
            {
                m.WriteByte(PlayerCompPtr2(0) + PC_CritKill1, 255);
            }
            else
            {
                m.WriteByte(PlayerCompPtr2(0) + PC_CritKill1, 0);
            }
            Thread.Sleep(1);
        }

        private void P2AC_Tick(object sender, EventArgs e)
        {
            if (P2AC)
            {
                m.WriteByte(PlayerCompPtr2(1) + PC_CritKill1, 255);
            }
            else
            {
                m.WriteByte(PlayerCompPtr2(1) + PC_CritKill1, 0);
            }
            Thread.Sleep(1);
        }

        private void P3AC_Tick(object sender, EventArgs e)
        {
            if (P3AC)
            {
                m.WriteByte(PlayerCompPtr2(2) + PC_CritKill1, 255);
            }
            else
            {
                m.WriteByte(PlayerCompPtr2(2) + PC_CritKill1, 0);
            }
            Thread.Sleep(1);
        }

        private void P4AC_Tick(object sender, EventArgs e)
        {
            if (P4AC)
            {
                m.WriteByte(PlayerCompPtr2(3) + PC_CritKill1, 255);
            }
            else
            {
                m.WriteByte(PlayerCompPtr2(3) + PC_CritKill1, 0);
            }
            Thread.Sleep(1);
        }

        private void P1Camo_Tick(object sender, EventArgs e)
        {
            if (OSG)
            {
                for (int i = 0; i < camoarme1.Length; i++)
                {
                    m.WriteInt16(camoarme1[i], 2499);
                }
                for (int j = 0; j < camoarme2.Length; j++)
                {
                    m.WriteInt16(camoarme2[j], 2499);
                }
                for (int k = 0; k < camoarme3.Length; k++)
                {
                    m.WriteInt16(camoarme3[k], 2499);
                }
                for (int l = 0; l < camoarme4.Length; l++)
                {
                    m.WriteByte(camoarme4[l], 9);
                }
                for (int q = 0; q < camoarme5.Length; q++)
                {
                    m.WriteByte(camoarme5[q], 9);
                }
                for (int n = 0; n < camoarme6.Length; n++)
                {
                    m.WriteByte(camoarme6[n], 24);
                }
                for (int num = 0; num < camoarme7.Length; num++)
                {
                    m.WriteByte(camoarme7[num], 14);
                }
                for (int num2 = 0; num2 < camoarmegoldall.Length; num2++)
                {
                    m.WriteByte(camoarmegoldall[num2], 34);
                }
                for (int num3 = 0; num3 < camoarmegoldall2.Length; num3++)
                {
                    m.WriteByte(camoarmegoldall2[num3], 34);
                }

                P1Camo.Stop();
            }
        }

        private void P2Camo_Tick(object sender, EventArgs e)
        {
            if (OSG)
            {
                for (int i = 0; i < p2camoarme1.Length; i++)
                {
                    m.WriteInt16(p2camoarme1[i], 2499);
                }
                for (int j = 0; j < p2camoarme2.Length; j++)
                {
                    m.WriteInt16(p2camoarme2[j], 2499);
                }
                for (int k = 0; k < p2camoarme3.Length; k++)
                {
                    m.WriteInt16(p2camoarme3[k], 2499);
                }
                for (int l = 0; l < p2camoarme4.Length; l++)
                {
                    m.WriteByte(p2camoarme4[l], 9);
                }
                for (int q = 0; q < p2camoarme5.Length; q++)
                {
                    m.WriteByte(p2camoarme5[q], 9);
                }
                for (int n = 0; n < p2camoarme6.Length; n++)
                {
                    m.WriteByte(p2camoarme6[n], 24);
                }
                for (int num = 0; num < p2camoarme7.Length; num++)
                {
                    m.WriteByte(p2camoarme7[num], 14);
                }
                for (int num2 = 0; num2 < p2camoarmegoldall.Length; num2++)
                {
                    m.WriteByte(p2camoarmegoldall[num2], 34);
                }
                for (int num3 = 0; num3 < p2camoarmegoldall2.Length; num3++)
                {
                    m.WriteByte(p2camoarmegoldall2[num3], 34);
                }

                P2Camo.Stop();
            }
        }

        private void P3Camo_Tick(object sender, EventArgs e)
        {
            if (OSG)
            {
                for (int i = 0; i < p4camoarme1.Length; i++)
                {
                    m.WriteInt16(p4camoarme1[i], 2499);
                }
                for (int j = 0; j < p4camoarme2.Length; j++)
                {
                    m.WriteInt16(p4camoarme2[j], 2499);
                }
                for (int k = 0; k < p4camoarme3.Length; k++)
                {
                    m.WriteInt16(p4camoarme3[k], 2499);
                }
                for (int l = 0; l < p4camoarme4.Length; l++)
                {
                    m.WriteByte(p4camoarme4[l], 9);
                }
                for (int q = 0; q < p4camoarme5.Length; q++)
                {
                    m.WriteByte(p4camoarme5[q], 9);
                }
                for (int n = 0; n < p4camoarme6.Length; n++)
                {
                    m.WriteByte(p4camoarme6[n], 24);
                }
                for (int num = 0; num < p4camoarme7.Length; num++)
                {
                    m.WriteByte(p4camoarme7[num], 14);
                }
                for (int num2 = 0; num2 < p4camoarmegoldall.Length; num2++)
                {
                    m.WriteByte(p4camoarmegoldall[num2], 34);
                }
                for (int num3 = 0; num3 < p4camoarmegoldall2.Length; num3++)
                {
                    m.WriteByte(p4camoarmegoldall2[num3], 34);
                }

                P3Camo.Stop();
            }
        }

        private void P4Camo_Tick(object sender, EventArgs e)
        {
            if (OSG)
            {
                for (int i = 0; i < p4camoarme1.Length; i++)
                {
                    m.WriteInt16(p4camoarme1[i], 2499);
                }
                for (int j = 0; j < p4camoarme2.Length; j++)
                {
                    m.WriteInt16(p4camoarme2[j], 2499);
                }
                for (int k = 0; k < p4camoarme3.Length; k++)
                {
                    m.WriteInt16(p4camoarme3[k], 2499);
                }
                for (int l = 0; l < p4camoarme4.Length; l++)
                {
                    m.WriteByte(p4camoarme4[l], 9);
                }
                for (int q = 0; q < p4camoarme5.Length; q++)
                {
                    m.WriteByte(p4camoarme5[q], 9);
                }
                for (int n = 0; n < p4camoarme6.Length; n++)
                {
                    m.WriteByte(p4camoarme6[n], 24);
                }
                for (int num = 0; num < p4camoarme7.Length; num++)
                {
                    m.WriteByte(p4camoarme7[num], 14);
                }
                for (int num2 = 0; num2 < p4camoarmegoldall.Length; num2++)
                {
                    m.WriteByte(p4camoarmegoldall[num2], 34);
                }
                for (int num3 = 0; num3 < p4camoarmegoldall2.Length; num3++)
                {
                    m.WriteByte(p4camoarmegoldall2[num3], 34);
                }

                P4Camo.Stop();
            }
        }

        private void Address_Tick(object sender, EventArgs e)
        {
            if (m.IsOpen())
            {
                process.Interval = 500;
                if (!pos.Enabled)
                {
                    pos.Start();
                }
                if (basea == 0)
                {
                    address.addressThread();
                    basea = 1;
                }
            }
            else
            {
                m.AttackProcess("BlackOpsColdWar");
                process.Interval = 100;
            }
        }

        private void P1Cycle_Tick(object sender, EventArgs e)
        {
            if (P1WPC)
            {
                byte[] pkill = new byte[4];
                bmem.ReadProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_KillCount), pkill, 4, out _);
                int pkills = BitConverter.ToInt32(pkill, 0);

                if (pkills == 0)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_KillCount), 5, 4, out _);
                }
                if (pkills % 5 == 0)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_KillCount), pkills + 1, 4, out _);
                    m.WriteInt64(PlayerCompPtr2(0) + PC_SetWeaponID, GunList[i1]);
                    i1++;
                }
                if (i1 == 37)
                {
                    i1 = 0;
                }
            }
            else
            {
                P1Cycle.Stop();
            }

            Thread.Sleep(1);
        }

        private void P2Cycle_Tick(object sender, EventArgs e)
        {
            if (P2WPC)
            {
                byte[] p2kill = new byte[4];
                bmem.ReadProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_KillCount), p2kill, 4, out _);
                int p2kills = BitConverter.ToInt32(p2kill, 0);

                if (p2kills == 0)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_KillCount), 5, 4, out _);
                }
                if (p2kills % 5 == 0)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_KillCount), p2kills + 1, 4, out _);
                    m.WriteInt64(PlayerCompPtr2(1) + PC_SetWeaponID, GunList[i2]);
                    i2++;
                }
                if (i2 == 37)
                {
                    i2 = 0;
                }
            }
            else
            {
                P2Cycle.Stop();
            }
            Thread.Sleep(1);
        }

        private void P3Cycle_Tick(object sender, EventArgs e)
        {
            if (P3WPC)
            {
                byte[] p3kill = new byte[4];
                bmem.ReadProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_KillCount), p3kill, 4, out _);
                int p3kills = BitConverter.ToInt32(p3kill, 0);

                if (p3kills == 0)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_KillCount), 5, 4, out _);
                }
                if (p3kills % 5 == 0)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_KillCount), p3kills + 1, 4, out _);
                    m.WriteInt64(PlayerCompPtr2(2) + PC_SetWeaponID, GunList[i3]);
                    i3++;
                }
                if (i3 == 37)
                {
                    i3 = 0;
                }
            }
            else
            {
                P3Cycle.Stop();
            }

            Thread.Sleep(1);
        }

        private void P4Cycle_Tick(object sender, EventArgs e)
        {
            if (P4WPC)
            {
                byte[] p4kill = new byte[4];
                bmem.ReadProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_KillCount), p4kill, 4, out _);
                int p4kills = BitConverter.ToInt32(p4kill, 0);

                if (p4kills == 0)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_KillCount), 2, 4, out _);
                }
                if (p4kills % 5 == 0)
                {
                    bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_KillCount), p4kills + 1, 4, out _);
                    m.WriteInt64(PlayerCompPtr2(3) + PC_SetWeaponID, GunList[i4]);
                    i4++;
                }
                if (i4 == 37)
                {
                    i4 = 0;
                }
            }
            else
            {
                P4Cycle.Stop();
            }
            Thread.Sleep(1);
        }

        private void SKIPSwitch_Click(object sender, EventArgs e)
        {
            if (GameAttached)
            {
                if (SKIPSwitch.Value)
                {
                    Skip.Start();
                }
                else
                {
                    Skip.Stop();
                }
            }
            else
            {
                MessageBox.Show("The game must be running to use this feature!", "BLUE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SKIPSwitch.Value = false;
            }
        }

        private void Skip_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < skip.Length; i++)
            {
                this.m.WriteInt32(skip[i], 0);
            }

            for (int i = 0; i < skip2.Length; i++)
            {
                this.m.WriteInt32(skip2[i], 0);
            }

            for (int i = 0; i < skip3.Length; i++)
            {
                this.m.WriteInt32(skip3[i], 0);
            }

            for (int i = 0; i < skip4.Length; i++)
            {
                this.m.WriteInt32(skip4[i], 0);
            }

            for (int i = 0; i < skip5.Length; i++)
            {
                this.m.WriteInt32(skip5[i], 0);
            }

            for (int i = 0; i < skip6.Length; i++)
            {
                this.m.WriteInt32(skip6[i], 0);
            }

            Thread.Sleep(1);
        }

        private void GlobalPnl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DragPnl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PMBtn_Click(object sender, EventArgs e)
        {
            if (SKIP)
            {
                SKIP = true;
                SKIPSwitch.Value = true;
            }
            else
            {
                Skip.Start();
                SKIP = true;
                SKIPSwitch.Value = true;
            }

            playerSpeed = 0f;
            SpeedSlider.Value = 0;
            SpeedValue.Text = "0";

            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_RunSpeed), 0, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_RunSpeed), 0, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_RunSpeed), 0, 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_RunSpeed), 0, 4, out _);

            var origxp1 = jailx;
            var origyp1 = jaily;
            var origzp1 = jailz;
            jails = new Vector3((float)Math.Round(origxp1, 4), (float)Math.Round(origyp1, 4), (float)Math.Round(origzp1, 4));
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(0) + PC_Coords), jails, 12, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(1) + PC_Coords), jails, 12, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(2) + PC_Coords), jails, 12, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(PlayerCompPtr2(3) + PC_Coords), jails, 12, out _);

            float prestigemaster = 1000000.0f;
            float defxp = 1.0f;
            bmem.WriteProcessMemory(proc, (IntPtr)(baseAddress.ToInt64() + ZMXPScaleBase.ToInt64() + 0x20), BitConverter.GetBytes(prestigemaster), 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(baseAddress.ToInt64() + ZMXPScaleBase.ToInt64() + 0x28), BitConverter.GetBytes(prestigemaster), 4, out _);

            Thread.Sleep(3000);

            MessageBox.Show("Setting back to default xp...", "BLAZN");
            bmem.WriteProcessMemory(proc, (IntPtr)(baseAddress.ToInt64() + ZMXPScaleBase.ToInt64() + 0x20), BitConverter.GetBytes(defxp), 4, out _);
            bmem.WriteProcessMemory(proc, (IntPtr)(baseAddress.ToInt64() + ZMXPScaleBase.ToInt64() + 0x28), BitConverter.GetBytes(defxp), 4, out _);
        }

        private void ProPnl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SKIPLabel_Click(object sender, EventArgs e)
        {

        }

        private void OSGLabel_Click(object sender, EventArgs e)
        {

        }

        private void PROLabel_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        //
        //*END OF TIMERS*//
        //
    }
}
