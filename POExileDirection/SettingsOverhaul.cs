﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POExileDirection
{
    public partial class SettingsOverhaul : Form
    {
        #region [[[[[ Global Variables ]]]]]
        // HOT KEYS
        public string keyRemains;
        public string keyJUN;
        public string keyALVA;
        public string keyZANA;
        public string keyHideout;
        public string keySearchbyPosition;
        public string keyEXIT;

        // Timer Color
        public string colorStringRGB1;
        public string colorStringRGB2;
        public string colorStringRGB3;
        public string colorStringRGB4;
        public string colorStringRGB5;

        public string colorStringRGBQ;
        public string colorStringRGBW;
        public string colorStringRGBE;
        public string colorStringRGBR;
        public string colorStringRGBT;

        // TAB INDEX
        public int _nTabIndex = 0;
        #endregion

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        public SettingsOverhaul()
        {
            InitializeComponent();
        }

        private void SettingsOverhaul_Load(object sender, EventArgs e)
        {
            Visible = false;
            this.TopMost = true;

            GetHotkeySettings();
            GetFlaskAndSkllTimerSettings();
            GetTradeNotificationSettings();
            GetOverlaySettings();
            GetHelpSettings();
            GetHallOfFameSettings();

            Visible = true;
        }

        #region [[[[[ Utility Functions ]]]]]
        private Color StringRGBToColor(string color)
        {
            var arrColorFragments = color?.Split(',').Select(sFragment => { _ = int.TryParse(sFragment, out int fragment); return fragment; }).ToArray();

            switch (arrColorFragments?.Length)
            {
                case 3:
                    return Color.FromArgb(255, arrColorFragments[0], arrColorFragments[1], arrColorFragments[2]);
                case 4:
                    return Color.FromArgb(arrColorFragments[0], arrColorFragments[1], arrColorFragments[2], arrColorFragments[3]);
                default:
                    return Color.Transparent;
            }
        }

        private void Set_FlaskImageList()
        {
            try
            {
                // Set ImageList
                var imageList = new ImageList();
                foreach (var obj in NinjaTranslation.FlaskImgPath)
                {
                    try
                    {
                        imageList.Images.Add(Image.FromFile(Application.StartupPath + "\\DeadlyInform\\Flask\\" + obj.Value));
                    }
                    catch (Exception ex)
                    {
                        DeadlyLog4Net._log.Error($"catch {MethodBase.GetCurrentMethod().Name}", ex);
                    }

                }

                listViewFlaskImage.View = View.LargeIcon;
                imageList.ImageSize = new Size(30, 60);
                imageList.ColorDepth = ColorDepth.Depth32Bit;
                this.listViewFlaskImage.LargeImageList = imageList;

                // Add Item
                for (int j = 0; j < imageList.Images.Count; j++)
                {
                    ListViewItem item = new ListViewItem();
                    item.BackColor = Color.FromArgb(39, 44, 56);
                    item.ImageIndex = j;
                    listViewFlaskImage.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                DeadlyLog4Net._log.Error($"catch {MethodBase.GetCurrentMethod().Name}", ex);
            }
        }
        #endregion

        #region [[[[[ GetHotkeySettings() ]]]]]
        private void GetHotkeySettings()
        {
            //TODO : Trade Notification Panel HotKeys.

            // HOTKEYS
            textRemains.Text = keyRemains;
            textJUN.Text = keyJUN;
            textALVA.Text = keyALVA;
            textZANA.Text = keyZANA;
            textHideout.Text = keyHideout;
            textBoxPositionSearch.Text = keySearchbyPosition;
            textBoxEXIT.Text = keyEXIT;

            // HotKey Use
            if (LauncherForm.g_strYNUseRemainingHOTKEY == "Y")
                checkRemaining.Checked = true;
            else
                checkRemaining.Checked = false;
            if (LauncherForm.g_strYNUseSyndicateJUNHOTKEY == "Y")
                checkSyndicateJUN.Checked = true;
            else
                checkSyndicateJUN.Checked = false;
            if (LauncherForm.g_strYNUseIncursionALVAHOTKEY == "Y")
                checkTempleALVA.Checked = true;
            else
                checkTempleALVA.Checked = false;
            if (LauncherForm.g_strYNUseAtlasZANAHOTKEY == "Y")
                checkAtlasZANA.Checked = true;
            else
                checkAtlasZANA.Checked = false;
            if (LauncherForm.g_strYNUseHideoutHOTKEY == "Y")
                checkHideout.Checked = true;
            else
                checkHideout.Checked = false;
            if (LauncherForm.g_strYNUseFindbyPositionHOTKEY == "Y")
                checkFindbyPosition.Checked = true;
            else
                checkFindbyPosition.Checked = false;
            if (LauncherForm.g_strYNUseEmergencyHOTKEY == "Y")
                checkEmergency.Checked = true;
            else
                checkEmergency.Checked = false;
        }
        #endregion

        #region [[[[[ GetFlaskAndSkllTimerSettings() ]]]]]
        private void GetFlaskAndSkllTimerSettings()
        {
            string strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath.ini");

            if (LauncherForm.resolution_width < 1920 && LauncherForm.resolution_height < 1080)
            {
                strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1600_1024.ini");
                if (LauncherForm.resolution_width < 1600 && LauncherForm.resolution_height < 1024)
                    strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1280_768.ini");
                else if (LauncherForm.resolution_width < 1280)
                    strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_LOW.ini");
            }
            else if (LauncherForm.resolution_width > 1920)
                strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_HIGH.ini");

            IniParser parser = new IniParser(strINIPath);

            // FLASK Timer & SKILL Timer
            string sColor1 = string.Empty;
            string sColor2 = string.Empty;
            string sColor3 = string.Empty;
            string sColor4 = string.Empty;
            string sColor5 = string.Empty;

            sColor1 = parser.GetSetting("MISC", "FLASK1COLOR");
            sColor2 = parser.GetSetting("MISC", "FLASK2COLOR");
            sColor3 = parser.GetSetting("MISC", "FLASK3COLOR");
            sColor4 = parser.GetSetting("MISC", "FLASK4COLOR");
            sColor5 = parser.GetSetting("MISC", "FLASK5COLOR");

            colorStringRGB1 = sColor1;
            colorStringRGB2 = sColor2;
            colorStringRGB3 = sColor3;
            colorStringRGB4 = sColor4;
            colorStringRGB5 = sColor5;

            panelCO1.BackColor = StringRGBToColor(sColor1);
            panelCO2.BackColor = StringRGBToColor(sColor2);
            panelCO3.BackColor = StringRGBToColor(sColor3);
            panelCO4.BackColor = StringRGBToColor(sColor4);
            panelCO5.BackColor = StringRGBToColor(sColor5);

            sColor1 = parser.GetSetting("SKILL", "SKILL1COLOR");
            sColor2 = parser.GetSetting("SKILL", "SKILL2COLOR");
            sColor3 = parser.GetSetting("SKILL", "SKILL3COLOR");
            sColor4 = parser.GetSetting("SKILL", "SKILL4COLOR");
            sColor5 = parser.GetSetting("SKILL", "SKILL5COLOR");

            colorStringRGBQ = sColor1;
            colorStringRGBW = sColor2;
            colorStringRGBE = sColor3;
            colorStringRGBR = sColor4;
            colorStringRGBT = sColor5;

            panelQ.BackColor = StringRGBToColor(sColor1);
            panelW.BackColor = StringRGBToColor(sColor2);
            panelE.BackColor = StringRGBToColor(sColor3);
            panelR.BackColor = StringRGBToColor(sColor4);
            panelT.BackColor = StringRGBToColor(sColor5);

            textBoxSEC1.Text = parser.GetSetting("MISC", "FLASKTIME1"); // LauncherForm.g_FlaskTime1
            textBoxSEC2.Text = parser.GetSetting("MISC", "FLASKTIME2"); // LauncherForm.g_FlaskTime2
            textBoxSEC3.Text = parser.GetSetting("MISC", "FLASKTIME3"); // LauncherForm.g_FlaskTime3
            textBoxSEC4.Text = parser.GetSetting("MISC", "FLASKTIME4"); // LauncherForm.g_FlaskTime4
            textBoxSEC5.Text = parser.GetSetting("MISC", "FLASKTIME5"); // LauncherForm.g_FlaskTime5

            labelFL1.Text = ((Keys)LauncherForm.g_Flask1).ToString();
            labelFL2.Text = ((Keys)LauncherForm.g_Flask2).ToString();
            labelFL3.Text = ((Keys)LauncherForm.g_Flask3).ToString();
            labelFL4.Text = ((Keys)LauncherForm.g_Flask4).ToString();
            labelFL5.Text = ((Keys)LauncherForm.g_Flask5).ToString();

            textBoxQ.Text = parser.GetSetting("SKILL", "SKILLTIME1");
            textBoxW.Text = parser.GetSetting("SKILL", "SKILLTIME2");
            textBoxE.Text = parser.GetSetting("SKILL", "SKILLTIME3");
            textBoxR.Text = parser.GetSetting("SKILL", "SKILLTIME4");
            textBoxT.Text = parser.GetSetting("SKILL", "SKILLTIME5");

            lbSkillQ.Text = ((Keys)LauncherForm.g_Skill1).ToString();
            lbSkillW.Text = ((Keys)LauncherForm.g_Skill2).ToString();
            lbSkillE.Text = ((Keys)LauncherForm.g_Skill3).ToString();
            lbSkillR.Text = ((Keys)LauncherForm.g_Skill4).ToString();
            lbSkillT.Text = ((Keys)LauncherForm.g_Skill5).ToString();

            xuiSliderFlaskVolume.Percentage = LauncherForm.g_FlaskTimerVolume;
            labelFlaskVolume.Text = "Volume = " + xuiSliderFlaskVolume.Percentage.ToString();

            Set_FlaskImageList();

            // Flask Image
            pictureFlask1.BackgroundImage = Image.FromFile(Application.StartupPath + "\\DeadlyInform\\Flask\\"
                                    + NinjaTranslation.FlaskImgPath[DeadlyFlaskImage.FlaskImageTimerGetValuebyKey(0)]);
            pictureFlask2.BackgroundImage = Image.FromFile(Application.StartupPath + "\\DeadlyInform\\Flask\\"
                                    + NinjaTranslation.FlaskImgPath[DeadlyFlaskImage.FlaskImageTimerGetValuebyKey(1)]);
            pictureFlask3.BackgroundImage = Image.FromFile(Application.StartupPath + "\\DeadlyInform\\Flask\\"
                                    + NinjaTranslation.FlaskImgPath[DeadlyFlaskImage.FlaskImageTimerGetValuebyKey(2)]);
            pictureFlask4.BackgroundImage = Image.FromFile(Application.StartupPath + "\\DeadlyInform\\Flask\\"
                                    + NinjaTranslation.FlaskImgPath[DeadlyFlaskImage.FlaskImageTimerGetValuebyKey(3)]);
            pictureFlask5.BackgroundImage = Image.FromFile(Application.StartupPath + "\\DeadlyInform\\Flask\\"
                                    + NinjaTranslation.FlaskImgPath[DeadlyFlaskImage.FlaskImageTimerGetValuebyKey(4)]);

            // Use Sound Alert.
            if (LauncherForm.g_strTimerSound1 == "Y")
                checkBox1.Checked = true;
            else
                checkBox1.Checked = false;
        }
        #endregion

        #region [[[[[ GetTradeNotificationSettings() ]]]]]
        private void GetTradeNotificationSettings()
        {
            xuiSlider1.Percentage = LauncherForm.g_NotifyVolume;
            labelVolume.Text = "Volume = " + xuiSlider1.Percentage.ToString();

            //TODO : Check Use Sound Alert.

            string strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath.ini");

            if (LauncherForm.resolution_width < 1920 && LauncherForm.resolution_height < 1080)
            {
                strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1600_1024.ini");
                if (LauncherForm.resolution_width < 1600 && LauncherForm.resolution_height < 1024)
                    strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1280_768.ini");
                else if (LauncherForm.resolution_width < 1280)
                    strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_LOW.ini");
            }
            else if (LauncherForm.resolution_width > 1920)
                strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_HIGH.ini");

            IniParser parser = new IniParser(strINIPath);

            // TRADING PANEL
            textBoxCharacterNick.Text = parser.GetSetting("CHARACTER", "MYNICK");
            if (parser.GetSetting("CHARACTER", "AUTOKICK") == "Y")
                checkBoxAutoKick.Checked = true;
            else
                checkBoxAutoKick.Checked = false;

            try
            {
                List<DeadlyAtlas.NotifyMSGCollection> NotifyMSG =
                    LauncherForm.deadlyInformationData.InformationMSG.NotifyMSG.Where(retval => retval.Id == "THX").ToList();

                textBoxDone.Text = NotifyMSG[0].Msg;
            }
            catch
            {
                textBoxDone.Text = "thanks. gl hf~.";
            }

            try
            {
                List<DeadlyAtlas.NotifyMSGCollection> NotifyMSG =
                    LauncherForm.deadlyInformationData.InformationMSG.NotifyMSG.Where(retval => retval.Id == "WILLING").ToList();

                textBoxResend.Text = NotifyMSG[0].Msg;
            }
            catch
            {
                textBoxResend.Text = "still willing to buy? 'Your Offer : '";
            }

            try
            {
                List<DeadlyAtlas.NotifyMSGCollection> NotifyMSG =
                    LauncherForm.deadlyInformationData.InformationMSG.NotifyMSG.Where(retval => retval.Id == "WAIT").ToList();

                textBoxWait.Text = NotifyMSG[0].Msg;
            }
            catch
            {
                textBoxWait.Text = "wait a sec pls.";
            }

            try
            {
                List<DeadlyAtlas.NotifyMSGCollection> NotifyMSG =
                    LauncherForm.deadlyInformationData.InformationMSG.NotifyMSG.Where(retval => retval.Id == "SOLD").ToList();

                textBoxSold.Text = NotifyMSG[0].Msg;
            }
            catch
            {
                textBoxSold.Text = "sold already. sry.";
            }
        }
        #endregion

        private void GetOverlaySettings()
        {
            ;
        }

        private void GetHelpSettings()
        {
            ;
        }

        private void GetHallOfFameSettings()
        {
            ;
            //labelSupporters.Text = LauncherForm.g_strDonator;
        }

        #region [[[[[ Function : GetSet_Hotkey ]]]]]
        public void GetSet_HotKey(KeyEventArgs e, string strWhich)
        {
            e.SuppressKeyPress = true;  //Suppress the key from being processed by the underlying control.
            if (strWhich == "REMAINS")
                textRemains.Text = string.Empty;
            else if (strWhich == "JUN")
                textJUN.Text = string.Empty;
            else if (strWhich == "ALVA")
                textALVA.Text = string.Empty;
            else if (strWhich == "ZANA")
                textZANA.Text = string.Empty;
            else if (strWhich == "HIDEOUT")
                textHideout.Text = string.Empty;
            else if (strWhich == "SEARCH")
                textBoxPositionSearch.Text = string.Empty;
            else if (strWhich == "EXIT")
                textBoxEXIT.Text = string.Empty;

            //Set the backspace button to specify that the user does not want to use a shortcut.
            if (e.KeyData == Keys.Back)
            {
                return;
            }

            // A modifier is present. Process each modifier.
            // Modifiers are separated by a ",". So we'll split them and write each one to the textbox.
            foreach (string modifier in e.Modifiers.ToString().Split(new Char[] { ';' }))
            {
                if (!modifier.Equals("none", StringComparison.OrdinalIgnoreCase))
                {
                    if (strWhich == "REMAINS")
                        textRemains.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                    else if (strWhich == "JUN")
                        textJUN.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                    else if (strWhich == "ALVA")
                        textALVA.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                    else if (strWhich == "ZANA")
                        textZANA.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                    else if (strWhich == "HIDEOUT")
                        textHideout.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                    else if (strWhich == "SEARCH")
                        textBoxPositionSearch.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                    else if (strWhich == "EXIT")
                        textBoxEXIT.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                }
                else
                {
                    if (strWhich == "REMAINS")
                        textRemains.Text = "NONE;" + e.KeyCode.ToString();
                    else if (strWhich == "JUN")
                        textJUN.Text = "NONE;" + e.KeyCode.ToString();
                    else if (strWhich == "ALVA")
                        textALVA.Text = "NONE;" + e.KeyCode.ToString();
                    else if (strWhich == "ZANA")
                        textZANA.Text = "NONE;" + e.KeyCode.ToString();
                    else if (strWhich == "HIDEOUT")
                        textHideout.Text = "NONE;" + e.KeyCode.ToString();
                    else if (strWhich == "SEARCH")
                        textBoxPositionSearch.Text = "NONE;" + e.KeyCode.ToString();
                    else if (strWhich == "EXIT")
                        textBoxEXIT.Text = "NONE;" + e.KeyCode.ToString();
                }
            }

            if (e.KeyCode == Keys.ShiftKey | e.KeyCode == Keys.ControlKey | e.KeyCode == Keys.Menu)
            {
                if (strWhich == "REMAINS")
                    textRemains.Text = keyRemains;
                else if (strWhich == "JUN")
                    textJUN.Text = keyJUN;
                else if (strWhich == "ALVA")
                    textALVA.Text = keyALVA;
                else if (strWhich == "ZANA")
                    textZANA.Text = keyZANA;
                else if (strWhich == "HIDEOUT")
                    textHideout.Text = keyHideout;
                else if (strWhich == "SEARCH")
                    textBoxPositionSearch.Text = keySearchbyPosition;
                else if (strWhich == "EXIT")
                    textBoxEXIT.Text = keySearchbyPosition;
            }

            keyRemains = textRemains.Text;
            keyJUN = textJUN.Text;
            keyALVA = textALVA.Text;
            keyZANA = textZANA.Text;
            keyHideout = textHideout.Text;
            keySearchbyPosition = textBoxPositionSearch.Text;
            keyEXIT = textBoxEXIT.Text;
        } 
        #endregion

        #region [[[[[ SAVE/CANCEL TAB : Hot Keys  Settings ]]]]]
        private void btnSave_Click(object sender, EventArgs e)
        {
            _nTabIndex = FlatSettingTab.SelectedIndex;

            // Check : HotKey Use
            if (checkRemaining.Checked)
                LauncherForm.g_strYNUseRemainingHOTKEY = "Y";
            else
                LauncherForm.g_strYNUseRemainingHOTKEY = "N";
            if (checkSyndicateJUN.Checked)
                LauncherForm.g_strYNUseSyndicateJUNHOTKEY = "Y";
            else
                LauncherForm.g_strYNUseSyndicateJUNHOTKEY = "N";
            if (checkTempleALVA.Checked)
                LauncherForm.g_strYNUseIncursionALVAHOTKEY = "Y";
            else
                LauncherForm.g_strYNUseIncursionALVAHOTKEY = "N";
            if (checkAtlasZANA.Checked)
                LauncherForm.g_strYNUseAtlasZANAHOTKEY = "Y";
            else
                LauncherForm.g_strYNUseAtlasZANAHOTKEY = "N";
            if (checkHideout.Checked)
                LauncherForm.g_strYNUseHideoutHOTKEY = "Y";
            else
                LauncherForm.g_strYNUseHideoutHOTKEY = "N";
            if (checkFindbyPosition.Checked)
                LauncherForm.g_strYNUseFindbyPositionHOTKEY = "Y";
            else
                LauncherForm.g_strYNUseFindbyPositionHOTKEY = "N";
            if (checkEmergency.Checked)
                LauncherForm.g_strYNUseEmergencyHOTKEY = "Y";
            else
                LauncherForm.g_strYNUseEmergencyHOTKEY = "N";

            btnSave.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnSave.DialogResult = DialogResult.Cancel;
            Close();
        }
        #endregion

        #region [[[[[ HotKey KeyDown ]]]]]
        private void textRemains_KeyDown(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, HOTKEYNAME_STRINGExtensions.ToDescriptionString(HOTKEYNAME_STRING.HOTKEYNAME_REMAINS));
        }

        private void textJUN_KeyDown(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, HOTKEYNAME_STRINGExtensions.ToDescriptionString(HOTKEYNAME_STRING.HOTKEYNAME_JUN));
        }

        private void textALVA_KeyDown(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, HOTKEYNAME_STRINGExtensions.ToDescriptionString(HOTKEYNAME_STRING.HOTKEYNAME_ALVA));
        }

        private void textZANA_KeyDown(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, HOTKEYNAME_STRINGExtensions.ToDescriptionString(HOTKEYNAME_STRING.HOTKEYNAME_ZANA));
        }

        private void textHideout_KeyDown(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, HOTKEYNAME_STRINGExtensions.ToDescriptionString(HOTKEYNAME_STRING.HOTKEYNAME_HIDEOUT));
        }

        private void textBoxPositionSearch_KeyDown(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, HOTKEYNAME_STRINGExtensions.ToDescriptionString(HOTKEYNAME_STRING.HOTKEYNAME_SEARCH));
        }

        private void textBoxEXIT_KeyDown(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, HOTKEYNAME_STRINGExtensions.ToDescriptionString(HOTKEYNAME_STRING.HOTKEYNAME_EXIT));
        }
        #endregion

        #region [[[[[ SAVE/CANCEL TAB : Trade Notification Settings ]]]]]
        private void btnSaveTab2_Click(object sender, EventArgs e)
        {
            // NICKNAME
            if (!String.IsNullOrEmpty(textBoxCharacterNick.Text))
                LauncherForm.g_strMyNickName = textBoxCharacterNick.Text;

            // NOTIFICATION MESSAGE
            if (!String.IsNullOrEmpty(textBoxWait.Text))
                LauncherForm.g_strnotiWAIT = textBoxWait.Text;

            if (!String.IsNullOrEmpty(textBoxSold.Text))
                LauncherForm.g_strnotiSOLD = textBoxSold.Text;

            if (!String.IsNullOrEmpty(textBoxDone.Text))
                LauncherForm.g_strnotiDONE = textBoxDone.Text;

            if (!String.IsNullOrEmpty(textBoxResend.Text))
                LauncherForm.g_strnotiRESEND = textBoxResend.Text;

            if (!String.IsNullOrEmpty(textBoxCustom1.Text))
                LauncherForm.g_strCUSTOM1 = textBoxCustom1.Text;

            if (!String.IsNullOrEmpty(textBoxCustom2.Text))
                LauncherForm.g_strCUSTOM2 = textBoxCustom2.Text;

            if (!String.IsNullOrEmpty(textBoxCustom3.Text))
                LauncherForm.g_strCUSTOM3 = textBoxCustom3.Text;

            // AUTO KICK : THX
            if (checkBoxAutoKick.Checked)
                LauncherForm.g_strTRAutoKick = "Y";
            else
                LauncherForm.g_strTRAutoKick = "N";

            // AUTO KICK : CUSTOM1,2,3
            if (chkCustom1.Checked)
                LauncherForm.g_strTRAutoKickCustom1 = "Y";
            else
                LauncherForm.g_strTRAutoKickCustom1 = "N";
            if (chkCustom2.Checked)
                LauncherForm.g_strTRAutoKickCustom2 = "Y";
            else
                LauncherForm.g_strTRAutoKickCustom2 = "N";
            if (chkCustom3.Checked)
                LauncherForm.g_strTRAutoKickCustom3 = "Y";
            else
                LauncherForm.g_strTRAutoKickCustom3 = "N";

            // TODO : Hot keys... and...


            btnCancelTab2.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelTab2_Click(object sender, EventArgs e)
        {
            btnCancelTab2.DialogResult = DialogResult.Cancel;
            Close();
        } 
        #endregion
    }
}
