﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _15
{
    /// <summary>
    /// Interaction logic for WinWindow.xaml
    /// </summary>
    public partial class WinWindow : Window
    {
       // string contT=moves.ToString();
        public WinWindow()
        {
            InitializeComponent();
        }

        private void Resultst_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //Resultst.Text= cw.To
        }
    }
}
