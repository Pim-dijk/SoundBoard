using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfPractice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
    }
}
