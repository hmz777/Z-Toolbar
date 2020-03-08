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
using System.Windows.Shapes;

namespace Z_Toolbar.UiComponents
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>    
    public partial class CustomMessageBox : Window
    {
        public MessageBoxIcon BoxIcon;

        /// <summary>
        /// Initiate a new instance of the custom message box 
        /// </summary>
        /// <param name="Text">The message to display</param>
        /// <param name="Icon">The message box icon that represents the content</param>
        public CustomMessageBox(string Text, MessageBoxIcon Icon)
        {          
            InitializeComponent();
            this.BoxIcon = Icon;
            this.Text = Text;
        }

        public string Text
        {
            get
            {
                return MessageTextBlock.Text;
            }

            set
            {
                MessageTextBlock.Text = value;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (BoxIcon)
            {
                case MessageBoxIcon.Information:
                    IconImage.Source = new BitmapImage(new Uri("../Resources/information.png", UriKind.Relative));
                    break;
                case MessageBoxIcon.Warning:
                    IconImage.Source = new BitmapImage(new Uri("../Resources/warning.png", UriKind.Relative));
                    break;
                case MessageBoxIcon.Error:
                    IconImage.Source = new BitmapImage(new Uri("../Resources/error.png", UriKind.Relative));
                    break;
                default:
                    IconImage.Source = new BitmapImage(new Uri("../Resources/information.png", UriKind.Relative));
                    break;
            }
        }

        private void window_Initialized(object sender, EventArgs e)
        {

        }

        private void MessageTextBlock_Initialized(object sender, EventArgs e)
        {

        }
    }

    public enum MessageBoxIcon : int
    {
        Information = 1,
        Warning = 2,
        Error = 3
    }
}
