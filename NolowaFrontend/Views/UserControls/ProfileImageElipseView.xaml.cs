﻿using NolowaFrontend.Extensions;
using NolowaFrontend.Models;
using NolowaFrontend.Models.Events;
using NolowaFrontend.Models.Images;
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

namespace NolowaFrontend.Views.UserControls
{
    /// <summary>
    /// ProfileImageElipseView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProfileImageElipseView : UserControl
    {
        //private readonly ProfileImage _profileImage;
        
        /// <summary>
        /// 프로필 클릭 이벤트를 라우티드이벤트로 만들어서 밖으로 버블링시킴
        /// </summary>
        public static readonly RoutedEvent ClickedProfileImageEvent =
            EventManager.RegisterRoutedEvent("ClickedProfileImage", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PostView));

        public event RoutedEventHandler ClickedProfileImage
        {
            add { AddHandler(ClickedProfileImageEvent, value); }
            remove { RemoveHandler(ClickedProfileImageEvent, value); }
        }

        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User", typeof(User), typeof(ProfileImageElipseView), new PropertyMetadata(null));

        public ProfileImageElipseView()
        {
            InitializeComponent();
        }

        private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //RaiseEvent(e);
            if (User.IsNull())
                return;

            var newEventArgs = new ObjectRoutedEventArgs(ClickedProfileImageEvent, User);
            RaiseEvent(newEventArgs);
        }
    }
}
