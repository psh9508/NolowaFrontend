﻿using NolowaFrontend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NolowaFrontend.Views.MainViews
{
    /// <summary>
    /// DirectMessageSendView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DirectMessageSendView : UserControl
    {
        public DirectMessageSendView()
        {
            InitializeComponent();
        }

        private void _this_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = (DirectMessageSendVM)this.DataContext;

            vm.GetNewMessage += () =>  {
                lstDialog.ScrollIntoView(lstDialog.Items[lstDialog.Items.Count - 1]);
            };
        }
    }
}
