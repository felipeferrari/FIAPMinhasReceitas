﻿using FIAPMinhasReceitas.UWP.Pages;
using FIAPMinhasReceitas.UWP.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FIAPMinhasReceitas.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();

            NavigationService.Frame = ContentFrame;
            NavigationService.Navigated += On_Navigated;
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            NavView.Header = args.InvokedItem is NavigationViewItem ?
                ((NavigationViewItem)args.InvokedItem).Content : (string)args.InvokedItem;

            if (args.IsSettingsInvoked)
            {
                NavigationService.Navigate<ConfiguracoesPage>();
            }
            else
            {
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                NavView_Navigate(item as NavigationViewItem);
            }
        }

        private void NavView_Navigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "receitas":
                    NavigationService.Navigate<ReceitasPage>();
                    break;
                case "timer":
                    NavigationService.Navigate<TimerPage>();
                    break;
            }
        }

        private void NovaReceitaAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            NavView.Header = "Nova Receita";

            NavigationService.Navigate<EditarReceitaPage>();
        }


        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ContentFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;

            if (ContentFrame.SourcePageType == typeof(ConfiguracoesPage))
            {
                NavView.SelectedItem = NavView.SettingsItem as NavigationViewItem;
            }
            else
            {
                Dictionary<Type, string> lookup = new Dictionary<Type, string>()
                {
                    {typeof(ReceitasPage), "receitas"},
                    {typeof(TimerPage), "timer"},
                    {typeof(EditarReceitaPage), ""},
                };

                String stringTag = lookup[ContentFrame.SourcePageType];

                var navItem = NavView.MenuItems.FirstOrDefault(item => item is NavigationViewItem && ((NavigationViewItem)item).Tag.Equals(stringTag)) as NavigationViewItem;

                if (navItem != null)
                {
                    navItem.IsSelected = true;
                }
            }
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            ((NavigationViewItem)NavView.SettingsItem).Content = "Configurações";
        }
    }
}
