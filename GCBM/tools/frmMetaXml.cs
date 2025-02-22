﻿using System;
using System.IO;
using System.Security;
using System.Windows.Forms;
using GCBM.Properties;

namespace GCBM.tools
{
    public partial class frmMetaXml : Form
    {
        #region Main form

        public frmMetaXml()
        {
            InitializeComponent();

            cbAppNoReloadIOS.SelectedIndex = 0;
            cbAppAhbAccess.SelectedIndex = 0;
            cbPortUSB.SelectedIndex = 0;
            cbMountUSB.SelectedIndex = 0;
            lblImageNotFound.Visible = false;
            lblAttentionImageNotFound.Visible = false;
            tbAppName.Enabled = false;
            tbAppDeveloper.Enabled = false;
            tbAppVersion.Enabled = false;
            tbAppReleaseDate.Enabled = false;
            tbAppShortDescription.Enabled = false;
            tbAppLongDescription.Enabled = false;
            cbAppAhbAccess.Enabled = false;
            cbAppNoReloadIOS.Enabled = false;
        }

        #endregion

        #region Notifications

        private void GlobalNotifications(string ex)
        {
            notifyIcon.ShowBalloonTip(10, "GameCube Backup Manager", ex, ToolTipIcon.Info);
        }

        #endregion

        #region Reset

        private void Reset()
        {
            tbAppDeveloper.Text = null;
            tbAppLongDescription.Text = null;
            tbAppName.Text = null;
            tbAppShortDescription.Text = null;
            tbAppVersion.Text = null;
            tbAppReleaseDate.Text = null;
            cbAppNoReloadIOS.SelectedIndex = 0;
            cbAppAhbAccess.SelectedIndex = 0;
            cbPortUSB.SelectedIndex = 0;
            cbMountUSB.SelectedIndex = 0;
            //pbBanner.Image = null;
            pbBanner.Image = Resources.image_not_found_1;
            btnAddBanner.Enabled = false;
            btnDeleteBanner.Enabled = false;
            btnClear.Enabled = false;
            btnSave.Enabled = false;
            lblAttentionImageNotFound.Visible = false;
            lblImageNotFound.Visible = false;
            tbAppName.Enabled = false;
            tbAppDeveloper.Enabled = false;
            tbAppVersion.Enabled = false;
            tbAppReleaseDate.Enabled = false;
            tbAppShortDescription.Enabled = false;
            tbAppLongDescription.Enabled = false;
            cbAppAhbAccess.Enabled = false;
            cbAppNoReloadIOS.Enabled = false;
        }

        #endregion

        #region Read Value XML

        public string ReadValuefromXML(string xmlfile, string start, string end)
        {
            var fileinfo = new FileInfo(ofdXml.FileName);
            var other = fileinfo.DirectoryName;

            if (File.Exists(other + @"\icon.png"))
            {
                pbBanner.LoadAsync(other + @"\icon.png");
                btnAddBanner.Enabled = true;
                btnDeleteBanner.Enabled = true;
                btnClear.Enabled = true;
                btnSave.Enabled = true;
                lblAttentionImageNotFound.Visible = false;
                lblImageNotFound.Visible = false;
            }
            else
            {
                pbBanner.Image = Resources.image_not_found_2;
                btnAddBanner.Enabled = true;
                btnDeleteBanner.Enabled = true;
                btnClear.Enabled = true;
                btnSave.Enabled = true;
                lblAttentionImageNotFound.Visible = true;
                lblImageNotFound.Visible = true;
            }

            tbAppName.Enabled = true;
            tbAppDeveloper.Enabled = true;
            tbAppVersion.Enabled = true;
            tbAppReleaseDate.Enabled = true;
            tbAppShortDescription.Enabled = true;
            tbAppLongDescription.Enabled = true;
            cbAppAhbAccess.Enabled = true;
            cbAppNoReloadIOS.Enabled = true;
            //chkEnableArguments.Enabled = true;

            var startindex = xmlfile.IndexOf(start);
            var endindex = xmlfile.IndexOf(end, startindex);
            var length = endindex - (startindex + start.Length);
            return xmlfile.Substring(startindex + start.Length, length);
        }

        #endregion

        #region Check Everything

        public bool CheckEverything()
        {
            var result = true;
            //if (xmlfilebox.Text == null) result = false; not for all...
            if (tbAppVersion.Text == "") result = false;
            if (tbAppName.Text == "") result = false;
            if (tbAppDeveloper.Text == "") result = false;
            //if (version.Text == "") result = false;
            if (tbAppReleaseDate.Text == "") result = false;
            if (tbAppShortDescription.Text == "") result = false;
            if (tbAppLongDescription.Text == "") result = false;
            //if (cbAppNoReloadIOS.SelectedIndex == 0) result = false;
            //if (cbAppAhbAccess.SelectedIndex == 0) result = false;

            return result;
        }

        #endregion

        #region Generate MetaXML

        public string GenerateMetaXML()
        {
            //char quote = '"';
            var xmlgenerated = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n\t<app version=\"";
            xmlgenerated += "1.0.0\">\r\n\t\t";
            xmlgenerated += "<name>" + tbAppName.Text + "</name>\r\n\t\t";
            xmlgenerated += "<coder>" + tbAppDeveloper.Text + "</coder>\r\n\t\t";
            xmlgenerated += "<version>" + tbAppVersion.Text + "</version>\r\n\t\t";
            xmlgenerated += "<release_date>" + tbAppReleaseDate.Text + "</release_date>\r\n\t\t";

            //if (chkEnableArguments.Checked)
            //{
            //    if (cbPortUSB.SelectedIndex != 0 || cbMountUSB.SelectedIndex != 0 || tbIOS.Text != string.Empty)
            //    {
            //        xmlgenerated += "<arguments>\r\n\t\t";
            //        if (tbIOS.Text != string.Empty)
            //        {
            //            xmlgenerated += "   <arg>--ios=" + tbIOS.Text + "</arg>\r\n\t\t";
            //        }

            //        if (cbPortUSB.SelectedIndex != 0)
            //        {
            //            xmlgenerated += "   <arg>--usbport=" + cbPortUSB.Text + "</arg>\r\n\t\t";
            //        }

            //        if (cbMountUSB.SelectedIndex != 0)
            //        {
            //            xmlgenerated += "   <arg>--mountusb=" + cbMountUSB.Text + "</arg>\r\n\t\t";
            //        }
            //        xmlgenerated += "</arguments>\r\n\t\t";
            //    }
            //}

            if (cbAppNoReloadIOS.SelectedIndex == 1) xmlgenerated += "<no_ios_reload />\r\n\t\t";

            if (cbAppAhbAccess.SelectedIndex == 1) xmlgenerated += "<ahb_access />\r\n\t\t";

            xmlgenerated += "<short_description>" + tbAppShortDescription.Text + "</short_description>\r\n\t\t";
            xmlgenerated += "<long_description>" + tbAppLongDescription.Text + "</long_description>\r\n\t</app>";

            return xmlgenerated;
        }

        #endregion

        #region Buttons

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ofdXml.ShowDialog() == DialogResult.OK)
                try
                {
                    var xmlfile = File.ReadAllText(ofdXml.FileName);
                    //xmlfilebox.Text = openmetaxml.FileName.ToString();
                    //appversion.Text = ReadValuefromXML(xmlfile, "<app version=", ">");
                    //appversion.Text = appversion.Text.Substring(1, appversion.Text.Length - 2);
                    tbAppName.Text = ReadValuefromXML(xmlfile, "<name>", "</name>");
                    tbAppDeveloper.Text = ReadValuefromXML(xmlfile, "<coder>", "</coder>");
                    tbAppVersion.Text = ReadValuefromXML(xmlfile, "<version>", "</version>");
                    tbAppReleaseDate.Text = ReadValuefromXML(xmlfile, "<release_date>", "</release_date>");
                    tbAppShortDescription.Text =
                        ReadValuefromXML(xmlfile, "<short_description>", "</short_description>");
                    tbAppLongDescription.Text = ReadValuefromXML(xmlfile, "<long_description>", "</long_description>");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    MessageBox.Show(Resources.MetaXml_String1 + Environment.NewLine + ex.Message,
                        Resources.MetaXml_String_ERRO,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (SecurityException ex)
                {
                    GlobalNotifications(ex.Message);
                }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var fileinfo = new FileInfo(ofdXml.FileName);
            var other = fileinfo.DirectoryName;
            var xmlFile = @"\meta.xml";

            if (CheckEverything())
            {
                File.WriteAllText(other + xmlFile, GenerateMetaXML());
                // Save PictureBox
                if (pbBanner.Image != null) pbBanner.Image.Save(other + @"\icon.png");

                MessageBox.Show(Resources.MetaXml_String2, Resources.Notice, MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnAddBanner_Click(object sender, EventArgs e)
        {
            if (ofdBanner.ShowDialog() == DialogResult.OK)
                try
                {
                    pbBanner.LoadAsync(ofdBanner.FileName);
                    lblImageNotFound.Visible = false;
                    lblAttentionImageNotFound.Visible = false;
                    btnDeleteBanner.Enabled = true;
                }
                catch (Exception ex)
                {
                    GlobalNotifications(ex.Message);
                }
        }

        private void btnDeleteBanner_Click(object sender, EventArgs e)
        {
            try
            {
                var fileInfo = new FileInfo(ofdXml.FileName);
                var other = fileInfo.DirectoryName;
                var delFile = @"\icon.png";
                File.Delete(other + delFile);
                btnDeleteBanner.Enabled = false;
                pbBanner.Image = null;
                //pbBanner.Image = Properties.Resources.image_not_found_1;
                lblImageNotFound.Visible = false;
                lblAttentionImageNotFound.Visible = false;
            }
            catch (Exception ex)
            {
                GlobalNotifications(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        #endregion
    }
}