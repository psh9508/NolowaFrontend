﻿using NolowaFrontend.Extensions;
using NolowaFrontend.Models;
using NolowaFrontend.Servicies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// SearchView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SearchView : UserControl
    {
        private readonly User _user;
        private readonly ISearchService _searchService;

        public SearchView(User user)
        {
            InitializeComponent();

            _user = user;
            _searchService = new SearchService();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await SetSearchedKeywordAsync();
        }

        public async Task TimerSearchAsync(string text)
        {
            txtSearchResultEmpty.Text = string.Empty;

            if (text.IsNotVaild())
            {
                listboxUsers.ItemsSource = new List<SearchedUser>();
                await SetSearchedKeywordAsync();
                return;
            }

            var response = await _searchService.SearchUser(text);

            if (response.IsSuccess)
            {
                await InsertSearchKeywordAsync(text);

                listboxUsers.ItemsSource = response.ResponseData;

                if (response.ResponseData.Count <= 0)
                {
                    txtSearchResultEmpty.Text = $"\"{text}\" 검색하기";
                    return;
                }
            }
        }        

        private async Task SetSearchedKeywordAsync()
        {
            var response = await _searchService.GetSearchedKeywords(_user.ID);

            if (response.IsSuccess)
                listboxSearchedKeywords.ItemsSource = response.ResponseData;
        }

        private async Task InsertSearchKeywordAsync(string keyword)
        {
            await _searchService.Search(_user.ID, keyword);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await _searchService.DeleteAllSearchedKeywords(_user.ID);

            await SetSearchedKeywordAsync();
        }
    }
}
