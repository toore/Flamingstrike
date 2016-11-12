﻿using RISK.UI.WPF.ViewModels;

namespace RISK.UI.WPF.Views
{
    public partial class NotifyView
    {
        public NotifyView()
        {
            InitializeComponent();
        }
    }

    public class NotifyViewModelDesignerData : NotifyViewModel
    {
        //public string Message => "Message displays here";
        //public string ConfirmText => "Confirm";
        //public string AbortText => "Abort";
        public NotifyViewModelDesignerData()
        {
            Message = "Message displays here";
        }
    }
}