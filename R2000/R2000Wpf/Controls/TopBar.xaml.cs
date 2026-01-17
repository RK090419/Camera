using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using R2000Wpf.Resources;

namespace R2000Wpf.Controls
{

    public partial class TopBar : UserControl
    {
        public TopBar()
        {
            InitializeComponent();

            LangCommand = new RelayCommand(() =>
            {
                var defaultCulture = new CultureInfo("en-US");
                var hebrewCulture = new CultureInfo("he-IL");

                var current = CultureInfo.CurrentUICulture;

                LocalizedStrings.Instance.ChangeCulture(current.Name == defaultCulture.Name ?
                    hebrewCulture : defaultCulture);
                current = CultureInfo.CurrentUICulture;
            });

        }


        public ICommand? LangCommand
        {
            get { return (ICommand?)GetValue(LangCommandProperty); }
            set { SetValue(LangCommandProperty, value); }
        }

        public static readonly DependencyProperty LangCommandProperty =
            DependencyProperty.Register("LangCommand", typeof(ICommand), typeof(TopBar));


    }
}
