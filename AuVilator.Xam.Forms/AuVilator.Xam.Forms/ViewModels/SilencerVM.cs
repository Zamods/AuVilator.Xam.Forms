using AuVilator.Library.Features;
using AuVilator.Library.Features.Support;
using AuVilator.Library.Wav;
using AuVilator.Xam.Forms.Models;
using AuVilator.Xam.Forms.Page;
using AuVilator.Xam.Forms.Support;
using AuVilator.Xam.Forms.Support.Interface;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AuVilator.Xam.Forms.ViewModels
{
    public class SilencerVM : BaseVM
    {
        private SilencerM _silencerM;
        private ResourceGetter _resourceGetter;
        private bool _isFileLoaded = false;
        private bool _isRunning = false;

        /// <summary>
        /// Its a <typeparamref name="Progress&#60;SilencerProgressModel&#62;"></typeparamref> passed into [AuVilator.Library.Features.Silencer] so application can retreive execution progress and channel it to UI.
        /// </summary>
        public Progress<SilencerProgressModel> ProgessIndicator { get; set; }

        /// <summary>
        /// Its a token passed into [AuVilator.Library.Features.Silencer] so user can cancel the task once executed.
        /// </summary>
        private CancellationTokenSource cTS { get; set; }

        public double StartPositionAmplitude { get => _silencerM.startPositionAmplitude; private set => SetPropertyAndRaise(ref _silencerM.startPositionAmplitude, value, "StartPositionAmplitude"); }
        public double EndPositionAmplitude { get => _silencerM.endPositionAmplitude; private set => SetPropertyAndRaise(ref _silencerM.endPositionAmplitude, value, "EndPositionAmplitude"); }
        public int BinSize { get => _silencerM.binSize; private set => SetPropertyAndRaise(ref _silencerM.binSize, value, "BinSize"); }
        public string SelectedFileName { get => _silencerM.selectedFileName; private set => SetPropertyAndRaise(ref _silencerM.selectedFileName, value, "SelectedFileName"); }
        public string SelectedFilePath { get => _silencerM.selectedFilePath; private set => SetPropertyAndRaise(ref _silencerM.selectedFilePath, value, "SelectedFilePath"); }
        public string SelectedOutputFileName { get => _silencerM.selectedOutputFileName; private set => SetPropertyAndRaise(ref _silencerM.selectedOutputFileName, value, "SelectedOutputFileName"); }
        public string SelectedOutputFilePath { get => _silencerM.selectedOutputFilePath; private set => SetPropertyAndRaise(ref _silencerM.selectedOutputFilePath, value, "SelectedOutputFilePath"); }

        /// <summary>
        /// Tells the state listeners that user have successfully loaded input file.
        /// </summary>
        /// <remarks>
        /// It will allow user to run silencer and make the button on UI available.
        /// </remarks>
        public bool IsFileLoaded { get => IsFileSelected(); private set => SetPropertyAndRaise(ref _isFileLoaded, value, "IsFileLoaded"); }

        /// <summary>
        /// Represents the state of silencer execution.
        /// </summary>
        /// <remarks>
        /// It will change values when user execute the silencer and it is used to allow user to cancel the running task.
        /// </remarks>
        public bool IsRunning { get => IsFileSelected(); private set => SetPropertyAndRaise(ref _isRunning, value, "IsRunning"); }

        public SilencerVM()
        {
            _silencerM = new SilencerM();
            _resourceGetter = new ResourceGetter(CultureInfo.CurrentUICulture);
            cTS = new CancellationTokenSource();
            AbortSilencer = new Command(async () =>
            {
                cTS.Cancel();
                await ResetViewModel();
            });

            RunSilencer = new Command(async () =>
            {
                await SelectSavePathAndFileName();
                if (this.SelectedOutputFilePath != null)
                {
                    await SilenceTheAudio();
                }
            });

            GetFiles = new Command(async () =>
            {
                await LoadFiles();
            });

            OpenSettings = new Command(() =>
            {
                App.Current.MainPage = new SettingsPage();
            });
        }

        /// <summary>
        /// Resets the fields after application is done running the silencer.
        /// </summary>
        /// <remarks>
        /// It will reset regardless of success or failure of silencing task.
        /// </remarks>
        /// <returns>Completed Task.</returns>
        private Task ResetViewModel()
        {
            this.SelectedFileName = null;
            this.SelectedFilePath = null;
            this.SelectedOutputFileName = null;
            this.SelectedOutputFilePath = null;
            this.IsFileLoaded = false;
            this.IsRunning = false;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Does the silencing task by passing in parameters to function in AuVilator.Library
        /// </summary>
        /// <returns>Message Box that tells the fate of execution.</returns>
        private async Task SilenceTheAudio()
        {
            try
            {
                IsRunning = true;
                bool status = await Silencer.PurgeSilence(wavFilePath: this.SelectedFilePath,
                 binSize: this.BinSize,
                 wavOutputFilePath: this.SelectedOutputFilePath,
                 ampEnd: this.EndPositionAmplitude,
                 ampStart: this.StartPositionAmplitude,
                 ProgessIndicator,
                 cTS.Token,
                 CultureInfo.CurrentUICulture
                );
                if (status)
                {
                    string messageBoxText = _resourceGetter.GetSpecifiedResource("SilencerVM_STA_MB_Text");
                    string caption = $"{_resourceGetter.GetSpecifiedResource("SilencerVM_STA_MB_Caption")}\n{SelectedOutputFilePath}";
                    string buttonText = _resourceGetter.GetSpecifiedResource("Button_OK");
                    await ShowAlert(caption, messageBoxText, buttonText);
                };
            }
            catch (Exception ex)
            {
                string messageBoxText = $"{ _resourceGetter.GetSpecifiedResource("SilencerVM_STA_EX_Text")}\n" +
                              $"{ex.Message}";
                string caption = _resourceGetter.GetSpecifiedResource("SilencerVM_STA_EX_Caption");
                string buttonText = _resourceGetter.GetSpecifiedResource("Button_OK");
                await ShowAlert(caption, messageBoxText, buttonText);
            }
            await ResetViewModel();
        }

        /// <summary>
        /// Checks if the file is selected or not.
        /// Used to handle UI button so user can't click if file is not selected.
        /// </summary>
        /// <returns>True [bool] if file is selected or False [bool] if file is not selected.</returns>
        private bool IsFileSelected()
        {
            if (String.IsNullOrEmpty(this.SelectedFilePath))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks the format of file which is expected to be .wav. It checks the header to confirm the file is .wav.
        /// </summary>
        /// <returns>[bool] Task that represents the state of file format.</returns>
        private async Task<bool> CheckFileFormat()
        {
            try
            {
                bool isWav = WavFile.IsWav(this.SelectedFilePath);
                if (isWav == true)
                {
                    //this.SelectedOutputFilePath = $@"{this.SelectedFilePath.Substring(0, this.SelectedFilePath.Length - 4)}_Silenced.wav";
                }
            }
            catch (Exception ex)
            {
                string messageBoxText = $"{ _resourceGetter.GetSpecifiedResource("SilencerVM_CFF_EX_Text_1")} '{this.SelectedFileName}' { _resourceGetter.GetSpecifiedResource("SilencerVM_CFF_EX_Text_2")}\n" +
                              $"{ _resourceGetter.GetSpecifiedResource("SilencerVM_CFF_EX_Text_3")}\n" +
                              $"{_resourceGetter.GetSpecifiedResource("Gen_Error_Stack")}\n" +
                              $"{ex.Message}";
                string caption = _resourceGetter.GetSpecifiedResource("SilencerVM_CFF_EX_Caption");
                string buttonText = _resourceGetter.GetSpecifiedResource("Button_OK");
                await ShowAlert(caption, messageBoxText, buttonText);
                
                this.SelectedFilePath = null;
                this.SelectedFileName = null;
                return false;
                
            }
            return true;
        }

        /// <summary>
        /// Allows user to explicitly set output file name and file path.
        /// </summary>
        /// <returns>Completed Task</returns>
        private async Task SelectSavePathAndFileName()
        {
            try
            {
                /*var _picker = new SaveFileDialog();
                _picker.Filter = $"{_resourceGetter.GetSpecifiedResource("SilencerVM_LF_Select_Picker_Filter_Audio")}(*.wav)|*.wav; | {_resourceGetter.GetSpecifiedResource("SilencerVM_LF_Select_Picker_Filter_All")}(*.*) | *.* ;";
                _picker.Title = _resourceGetter.GetSpecifiedResource("SilencerVM_SF_Save_Picker_Title");
                if (_picker.ShowDialog() == true)
                {
                    this.SelectedOutputFileName = Path.GetFileName(_picker.FileName);
                    this.SelectedOutputFilePath = _picker.FileName;
                }*/
                var fileService = DependencyService.Get<IFileAccess>();
                this.SelectedOutputFilePath = fileService.AcquireFilePath($"{SelectedFileName.Substring(0, SelectedFileName.Length - 4)}_Silenced.wav", Environment.SpecialFolder.MyDocuments);
                this.SelectedOutputFileName = Path.GetFileName(SelectedOutputFilePath);
                /* Just checks if path is selected*/
                if (this.SelectedOutputFilePath.Length < 2)
                {
                    string messageBoxText = _resourceGetter.GetSpecifiedResource("SilencerVM_SF_MB_No_Selected_Text");
                    string caption = _resourceGetter.GetSpecifiedResource("SilencerVM_SF_Save_Picker_Title");
                    string buttonText = _resourceGetter.GetSpecifiedResource("Button_OK");
                    await ShowAlert(caption, messageBoxText, buttonText);
                }
            }
            catch (Exception ex)
            {
                string messageBoxText = $"{_resourceGetter.GetSpecifiedResource("SilencerVM_SF_EX_Text")}\n" +
                              $"{_resourceGetter.GetSpecifiedResource("Gen_Error_Stack")}\n" +
                              $"{ex.Message}";
                string caption = _resourceGetter.GetSpecifiedResource("SilencerVM_SF_EX_Caption");
                string buttonText = _resourceGetter.GetSpecifiedResource("Button_OK");
                await ShowAlert(caption, messageBoxText, buttonText);
            }
        }

        /// <summary>
        /// Allows user to select one file and load it in view model properties.
        /// </summary>
        /// <returns>Task completed</returns>
        /// <remarks>Fills up [SelectedFileName] and [SelectedFilePath] properties if path is selected and format matches with .wav</remarks>
        private async Task LoadFiles()
        {
            try
            {
                var result = await PickAndShow();
               /* _picker.Filter = $"{_resourceGetter.GetSpecifiedResource("SilencerVM_LF_Select_Picker_Filter_Audio")}(*.wav)|*.wav; | {_resourceGetter.GetSpecifiedResource("SilencerVM_LF_Select_Picker_Filter_All")}(*.*) | *.* ;";
                _picker.Title = _resourceGetter.GetSpecifiedResource("SilencerVM_LF_Select_Wav_Title");*/
                if (result.fileName != null)
                {
                    this.SelectedFileName = result.fileName;
                    this.SelectedFilePath = result.filePath;
                    this.IsFileLoaded = await CheckFileFormat();
                }
                /* Just checks if path is selected*/
                if (this.SelectedFilePath.Length < 2)
                {
                    string messageBoxText = _resourceGetter.GetSpecifiedResource("SilencerVM_LF_MB_No_Selected_Text");
                    string caption = _resourceGetter.GetSpecifiedResource("SilencerVM_LF_MB_No_Selected_Caption");
                    string buttonText = _resourceGetter.GetSpecifiedResource("Button_OK");
                    await ShowAlert(caption, messageBoxText, buttonText);
                }
            }
            catch (Exception ex)
            {
                string messageBoxText = $"{ _resourceGetter.GetSpecifiedResource("SilencerVM_LF_EX_Text")}\n" +
                              $"{ _resourceGetter.GetSpecifiedResource("Gen_Error_Stack")}\n" +
                              $"{ex.Message}";
                string caption = _resourceGetter.GetSpecifiedResource("SilencerVM_LF_EX_Caption");
                string buttonText = _resourceGetter.GetSpecifiedResource("Button_OK");
                await ShowAlert(caption, messageBoxText, buttonText);
            }
        }

        private async Task<FileM> PickAndShow()
        {
            try
            {
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    if (result.FileName.EndsWith("wav", StringComparison.OrdinalIgnoreCase))
                    {
                        var copyPath = Path.Combine(FileSystem.CacheDirectory, result.FileName);
                        using (var destination = File.Create(copyPath))
                        using (var source = await result.OpenReadAsync())
                            await source.CopyToAsync(destination);
                        return new FileM()
                        {
                            fileName = result.FileName,
                            filePath = copyPath
                        }; ;
                    } else
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                string messageBoxText = $"{ _resourceGetter.GetSpecifiedResource("SilencerVM_CFF_EX_Text_1")} '{this.SelectedFileName}' { _resourceGetter.GetSpecifiedResource("SilencerVM_CFF_EX_Text_2")}\n" +
              $"{ _resourceGetter.GetSpecifiedResource("SilencerVM_CFF_EX_Text_3")}\n" +
              $"{_resourceGetter.GetSpecifiedResource("Gen_Error_Stack")}\n" +
              $"{ex.Message}";
                string caption = _resourceGetter.GetSpecifiedResource("SilencerVM_CFF_EX_Caption");
                string buttonText = _resourceGetter.GetSpecifiedResource("Button_OK");
                await ShowAlert(caption, messageBoxText, buttonText);
            }
            return new FileM();
        }

        private async Task ShowAlert(string caption, string text, string cancelButton)
        {
            await App.Current.MainPage.DisplayAlert(caption, text, cancelButton);
        }

        /// <summary>
        /// Exposed command to navigate user to settings page
        /// </summary>
        public ICommand OpenSettings { get; private set; }

        /// <summary>
        /// Exposed command to initiate [Task] LoadFiles() from view
        /// </summary>
        public ICommand GetFiles { get; private set; }

        /// <summary>
        /// Exposed command to initiate [Task] ExecuteSilencer() from view
        /// </summary>
        public ICommand RunSilencer { get; private set; }

        /// <summary>
        /// Exposed command to initiate [CancellationTokenSource] to abort the ExecuteSilencer() from view
        /// </summary>
        public ICommand AbortSilencer { get; private set; }
    }

    /// <summary>
    /// Defines the output of the silenced audio.
    /// Left for future implementation
    /// </summary>
    internal enum OutputStyle
    {
        /// <summary>
        /// Compiles the all audio sections in single audio file.
        /// </summary>
        Single,

        /// <summary>
        /// Output all audio sections in individual audio files.
        /// </summary>
        Clipped
    }
}