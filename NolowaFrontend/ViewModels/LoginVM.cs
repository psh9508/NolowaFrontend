﻿using NolowaFrontend.Servicies;
using NolowaFrontend.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NolowaFrontend.ViewModels
{
    public class LoginVM : ViewModelBase
    {
        private readonly IUserService _service;

        private ICommand loginCommand;

        public ICommand LoginCommand
        {
            get
            {
                return GetRelayCommand(ref loginCommand, async x => {
                    var args = (object[])x;

                    var email = (string)args[0];
                    var password = (string)args[1];

                    var data = await _service.Login(new Models.User() {
                        Name = email,
                        Password = password,
                    });

                    if(data != null)
                        MessageBox.Show("로그인 성공");
                    else
                        MessageBox.Show("로그인 실패");
                });
            }
        }

        public LoginVM()
        {
            _service = new UserService();
        }
    }
}