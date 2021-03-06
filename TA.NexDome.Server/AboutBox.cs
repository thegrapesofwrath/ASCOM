﻿// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Server
    {
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;

    public partial class AboutBox : Form
        {
        public AboutBox()
            {
            InitializeComponent();
            }

        private void label1_Click(object sender, EventArgs e) { }

        private void AboutBox_Load(object sender, EventArgs e)
            {
            var me = Assembly.GetExecutingAssembly();
            var name = me.GetName();
            var driverVersion = name.Version;
            string productVersion = me.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            DriverVersion.Text = driverVersion.ToString();
            InformationalVersion.Text = productVersion;
            }

        private void DriverVersion_Click(object sender, EventArgs e) { }

        private void NavigateToWebPage(object sender, EventArgs e)
            {
            WindowUtils.NavigateToWebDestination(sender);
            }

        private void OkCommand_Click(object sender, EventArgs e)
            {
            Close();
            }
        }
    }