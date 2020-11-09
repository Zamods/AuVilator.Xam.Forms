
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuVilator.Xam.Forms.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FlatIconButton : ContentView
	{
		public FlatIconButton ()
		{
			InitializeComponent ();
		}

        public GridLength ButtonHeight
        {
            get => FlatButtonHeight.Height;
            set => FlatButtonHeight.Height = value;
        }

        //Prefer Auto
        public GridLength ButtonIconWidth
        {
            get => FlatButtonIconWidth.Width;
            set => FlatButtonIconWidth.Width = value;
        }

        //Prefer *
        public GridLength ButtonLabelWidth
        {
            get => FlatButtonLabelWidth.Width;
            set => FlatButtonLabelWidth.Width = value;
        }

        public ImageSource ButtonImageSource
        {
            get => FlatButtonImage.Source;
            set => FlatButtonImage.Source = value;
        }

        //Prefer Aspect Fit
        public Aspect ButtonImageAspect
        {
            get => FlatButtonImage.Aspect;
            set => FlatButtonImage.Aspect = value;
        }

        //Prefer (10,0,10,0)
        public Thickness ButtonImageMargin
        {
            get => FlatButtonImage.Margin;
            set => FlatButtonImage.Margin = value;
        }

        public string ButtonLabelText
        {
            get => FlatButtonLabel.Text;
            set => FlatButtonLabel.Text = value;
        }

        //Prefer (10,0,0,0)
        public Thickness ButtonLabelMargin
        {
            get => FlatButtonLabel.Margin;
            set => FlatButtonLabel.Margin = value;
        }

        public LayoutOptions ButtonLabelHorizontalOptions
        {
            get => FlatButtonLabel.HorizontalOptions;
            set => FlatButtonLabel.HorizontalOptions = value;
        }

        public LayoutOptions ButtonLabelVerticalOptions
        {
            get => FlatButtonLabel.VerticalOptions;
            set => FlatButtonLabel.VerticalOptions = value;
        }

        public string ButtonLabelFontSize
        {

            get
            {

                return FlatButtonLabel.FontSize.ToString();
            }
            set
            {
                FontSizeConverter fontSizeConverter = new FontSizeConverter();
                FlatButtonLabel.FontSize = (double)fontSizeConverter.ConvertFromInvariantString(value);
            }
        }

        public Color ButtonLabelFontColor
        {
            get => FlatButtonLabel.TextColor;
            set => FlatButtonLabel.TextColor = value;
        }

        public Color ButtonColor
        {
            get => FlatButton.BackgroundColor;
            set => FlatButton.BackgroundColor = value;
        }
    }
}