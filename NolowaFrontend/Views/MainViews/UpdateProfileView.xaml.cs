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
    /// UpdateProfileView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UpdateProfileView : UserControl
    {       
        public UpdateProfileView()
        {
            InitializeComponent();

            // 호출하는 ProfileView에서 명시적으로 세팅
            //this.DataContext = new UpdateProfileVM();
        }
    }
}
