﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wox.Plugin.SimpleClock.Views
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        private string pluginDirectory;
        public SettingsControl()
        {
            InitializeComponent();
        }

        public SettingsControl(string pluginDirectory)
        {
            this.pluginDirectory = pluginDirectory;
            InitializeComponent();
            if (String.IsNullOrEmpty(AlarmTrackProperty))
            {
                AlarmTrackProperty = System.IO.Path.Combine(pluginDirectory, "Sounds\\beepbeep.mp3");
            }
            tbxAudioFilePath.Text = AlarmTrackProperty;
            if (ClockSettingsWrapper.Settings.IsCenter)
            {
                centerRadioButton.IsChecked = true;
            }
            else
            {
                bottomRadioButton.IsChecked = true;
            }
        }

        public string AlarmTrackProperty
        {
            get
            {
                return ClockSettingsWrapper.Settings.AlarmTrackPath;
            }
            set
            {
                if (!File.Exists(value))
                {
                    MessageBox.Show("Error when setting alarm track", "File not found");
                    tbxAudioFilePath.Text = AlarmTrackProperty;
                    return;
                }
                ClockSettingsWrapper.Settings.AlarmTrackPath = value;
            }
        } 

        private void button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.FileName = "Audio track";
            ofd.InitialDirectory = pluginDirectory;
            ofd.Filter = "Audio Files (mp3/wav)|*.mp3;*.wav|All Files|*.*";
            ofd.CheckPathExists = true;
            var res = ofd.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                tbxAudioFilePath.Text = ofd.FileName;
            }
            
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            AlarmTrackProperty = tbxAudioFilePath.Text;
            ClockSettingsWrapper.Settings.IsCenter = centerRadioButton.IsChecked.GetValueOrDefault();
            ClockSettingsWrapper.Storage.Save();
        }
    }
}
