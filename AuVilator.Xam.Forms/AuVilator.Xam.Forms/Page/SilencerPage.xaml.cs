using AuVilator.Library.Features.Support;
using AuVilator.Xam.Forms.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuVilator.Xam.Forms.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SilencerPage : ContentPage
    {
        public SilencerPage()
        {
            InitializeComponent();
            var VM = new SilencerVM();
            this.BindingContext = VM;
            VM.ProgessIndicator = new Progress<SilencerProgressModel>();
            VM.ProgessIndicator.ProgressChanged += SilencerProgress;
        }

        private async void SilencerProgress(object sender, SilencerProgressModel e)
        {
            this.progressText.Text = e.ProgressMessage;
            await progressBar.ProgressTo(e.CompletionPercentage / 100, 500, Easing.SinInOut);
        }

        private void FileSelectionInformationButtonClicked(object sender, EventArgs e)
        {
            var controlVisibility = this.fileSelectInformationSP.IsVisible;
            if (controlVisibility)
            {
                this.fileSelectInformationSP.IsVisible = false;
            }
            else
            {
                this.fileSelectInformationSP.IsVisible = true;
            }
        }

        private void BinSizeSelectionInformationButtonClicked(object sender, EventArgs e)
        {
            var controlVisibility = this.binSizeSelectionInformationSP.IsVisible;
            if (controlVisibility)
            {
                this.binSizeSelectionInformationSP.IsVisible = false;
            }
            else
            {
                this.binSizeSelectionInformationSP.IsVisible = true;
            }
        }

        private void StartAmpPosInformationButtonClicked(object sender, EventArgs e)
        {
            var controlVisibility = this.startAmpPosInformationSP.IsVisible;
            if (controlVisibility)
            {
                this.startAmpPosInformationSP.IsVisible = false;
            }
            else
            {
                this.startAmpPosInformationSP.IsVisible = true;
            }
        }

        private void EndAmpPosInformationButtonClicked(object sender, EventArgs e)
        {
            var controlVisibility = this.endAmpPosInformationSP.IsVisible;
            if (controlVisibility)
            {
                this.endAmpPosInformationSP.IsVisible = false;
            }
            else
            {
                this.endAmpPosInformationSP.IsVisible = true;
            }
        }
    }
}