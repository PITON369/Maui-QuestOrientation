﻿namespace MauiQuestOrientation
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
        }
    }
}
