// This file is part of the TA.NexDome.AscomServer project
// Copyright � 2019-2019 Tigra Astronomy, all rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using NLog.Fluent;
using TA.NexDome.Server.Properties;

namespace TA.NexDome.Server
    {
    [ComVisible(false)] // Form not registered for COM!
    public partial class SetupDialogForm : Form
        {
        private ClickCommand updateClickCommand;

        public SetupDialogForm() => InitializeComponent();

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
            => communicationSettingsControl1.Save();

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
            => Close();

        private void BrowseToWebPage(object sender, EventArgs e)
            {
            // Click on a logo event handler
            var control = sender as Control;
            if (control == null)
                {
                Log.Error().Message("Can't browse to web page, event sender is not a WinForms control").Write();
                return;
                }

            string url = control.Tag.ToString();
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                Log.Error().Message("Can't browse to web page, URI not absolute well-formed: {uri}", url).Write();
                return;
                }

            try
                {
                Process.Start(url);
                }
            catch (Win32Exception noBrowser)
                {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
                }
            catch (Exception other)
                {
                MessageBox.Show(other.Message);
                }
            }

        private void SetupDialogForm_Load(object sender, EventArgs e)
            {
            var observableClientStatus = Observable.FromEventPattern<EventArgs>(
                handler => SharedResources.ConnectionManager.ClientStatusChanged += handler,
                handler => SharedResources.ConnectionManager.ClientStatusChanged -= handler);
            updateClickCommand = FirmwareUpdateCommand.AttachCommand(ExecuteFirmwareUpdate, CanUpdateFirmware);
            observableClientStatus.ObserveOn(SynchronizationContext.Current)
                .Subscribe(item => updateClickCommand.CanExecuteChanged());
            int onlineClients = SharedResources.ConnectionManager.OnlineClientCount;
            bool enableDisconnectedControls = onlineClients == 0;
            if (onlineClients == 0)
                {
                communicationSettingsControl1.Enabled = true;
                ConnectionErrorProvider.SetError(communicationSettingsControl1, string.Empty);
                }
            else
                {
                communicationSettingsControl1.Enabled = false;
                ConnectionErrorProvider.SetError(
                    communicationSettingsControl1,
                    "Connection settings cannot be changed while there are connected clients");
                }

            RotatorParametersGroup.Enabled = enableDisconnectedControls;
            ShutterParametersGroup.Enabled = enableDisconnectedControls;
            RotatorRampTimeCurrentValue.Text = RotatorRampTimeTrackBar.Value.ToString();
            RotatorSpeedCurrentValue.Text = RotatorMaximumSpeedTrackBar.Value.ToString();
            ShutterMaximumSpeedCurrentValue.Text = ShutterMaximumSpeedTrackBar.Value.ToString();
            ShutterRampTimeCurrentValue.Text = ShutterAccelerationRampTimeTrackBar.Value.ToString();
            SetControlAppearance();
            }

        private void ConnectionManager_ClientStatusChanged(object sender, EventArgs e) =>
            updateClickCommand.CanExecuteChanged();

        private void ExecuteFirmwareUpdate()
            {
            var updateForm = new FirmwareUpdate();
            updateForm.ShowDialog();
            }

        private bool CanUpdateFirmware() => SharedResources.ConnectionManager.OnlineClientCount == 0;

        private void AboutBox_Click(object sender, EventArgs e)
            {
            using (var aboutBox = new AboutBox()) aboutBox.ShowDialog();
            }

        private void PresetHD6_Click(object sender, EventArgs e)
            {
            FullRotationTimeSeconds.Value = 60;
            ShutterOpenCloseTimeSeconds.Value = 120;
            }

        private void PresetHD10_Click(object sender, EventArgs e)
            {
            FullRotationTimeSeconds.Value = 90;
            ShutterOpenCloseTimeSeconds.Value = 180;
            }

        private void PresetHD15_Click(object sender, EventArgs e)
            {
            FullRotationTimeSeconds.Value = 120;
            ShutterOpenCloseTimeSeconds.Value = 240;
            }

        private void SetControlAppearance() { }

        private void Label5_Click(object sender, EventArgs e) { }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e) { }

        private void Label12_Click(object sender, EventArgs e) { }

        private void Label10_Click(object sender, EventArgs e) { }

        private void Label11_Click(object sender, EventArgs e) { }

        private void Label9_Click(object sender, EventArgs e) { }

        private void RotatorRampTimeTrackBar_Scroll(object sender, EventArgs e) =>
            RotatorRampTimeCurrentValue.Text = RotatorRampTimeTrackBar.Value.ToString();

        private void RotatorMaximumSpeedTrackBar_Scroll(object sender, EventArgs e) =>
            RotatorSpeedCurrentValue.Text = RotatorMaximumSpeedTrackBar.Value.ToString();

        private void ShutterMaximumSpeedTrackBar_Scroll(object sender, EventArgs e) =>
            ShutterMaximumSpeedCurrentValue.Text = ShutterMaximumSpeedTrackBar.Value.ToString();

        private void ShutterAccelerationRampTimeTrackBar_Scroll(object sender, EventArgs e) =>
            ShutterRampTimeCurrentValue.Text = ShutterAccelerationRampTimeTrackBar.Value.ToString();

        private void FirmwareUpdateCommand_Click(object sender, EventArgs e) { }

        private void OnlineHelp_Click(object sender, EventArgs e) { }

        private void LoadDefaultsCommand_Click(object sender, EventArgs e)
            {
            Settings.Default.Reset();
            }
        }
    }