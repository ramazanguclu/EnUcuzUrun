
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices;
using System.Threading;
using System.Windows.Media.Imaging;
using ZXing;
using System.Windows.Threading;


namespace EnUcuzUrun
{
    public partial class Scan : PhoneApplicationPage
    {
        private PhotoCamera _phoneCamera;
        private IBarcodeReader _barcodeReader;
        private DispatcherTimer _scanTimer;
        private WriteableBitmap _previewBuffer;

        public Scan()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Initialize the camera object
            _phoneCamera = new PhotoCamera();
            _phoneCamera.Initialized += cam_Initialized;

            CameraButtons.ShutterKeyHalfPressed += CameraButtons_ShutterKeyHalfPressed;

            //Display the camera feed in the UI
            viewfinderBrush.SetSource(_phoneCamera);


            // This timer will be used to scan the camera buffer every 250ms and scan for any barcodes
            _scanTimer = new DispatcherTimer();
            _scanTimer.Interval = TimeSpan.FromMilliseconds(250);
            _scanTimer.Tick += (o, arg) => ScanForBarcode();

            viewfinderCanvas.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(focus_Tapped);

            base.OnNavigatedTo(e);
        }

        void _phoneCamera_AutoFocusCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(delegate()
            {
                focusBrackets.Visibility = Visibility.Collapsed;
            });
        }

        void focus_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (_phoneCamera != null)
            {
                if (_phoneCamera.IsFocusAtPointSupported == true)
                {
                    // Determine the location of the tap.
                    Point tapLocation = e.GetPosition(viewfinderCanvas);

                    // Position the focus brackets with the estimated offsets.
                    focusBrackets.SetValue(Canvas.LeftProperty, tapLocation.X - 30);
                    focusBrackets.SetValue(Canvas.TopProperty, tapLocation.Y - 28);

                    // Determine the focus point.
                    double focusXPercentage = tapLocation.X / viewfinderCanvas.ActualWidth;
                    double focusYPercentage = tapLocation.Y / viewfinderCanvas.ActualHeight;

                    // Show the focus brackets and focus at point.
                    try
                    {
                        focusBrackets.Visibility = Visibility.Visible;
                        _phoneCamera.FocusAtPoint(focusXPercentage, focusYPercentage);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                  
                }
            }
        }

        void CameraButtons_ShutterKeyHalfPressed(object sender, EventArgs e)
        {
            _phoneCamera.Focus();
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            //we're navigating away from this page, we won't be scanning any barcodes
            _scanTimer.Stop();

            if (_phoneCamera != null)
            {
                // Cleanup
                _phoneCamera.Dispose();
                _phoneCamera.Initialized -= cam_Initialized;
                CameraButtons.ShutterKeyHalfPressed -= CameraButtons_ShutterKeyHalfPressed;
            }
        }

        void cam_Initialized(object sender, Microsoft.Devices.CameraOperationCompletedEventArgs e)
        {
            if (e.Succeeded)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    _phoneCamera.FlashMode = FlashMode.Off;
                    _previewBuffer = new WriteableBitmap((int)_phoneCamera.PreviewResolution.Width, (int)_phoneCamera.PreviewResolution.Height);

                    _barcodeReader = new BarcodeReader();

                    // By default, BarcodeReader will scan every supported barcode type
                    // If we want to limit the type of barcodes our app can read, 
                    // we can do it by adding each format to this list object

                    //var supportedBarcodeFormats = new List<BarcodeFormat>();
                    //supportedBarcodeFormats.Add(BarcodeFormat.QR_CODE);
                    //supportedBarcodeFormats.Add(BarcodeFormat.DATA_MATRIX);
                    //_barcodeReader.PossibleFormats = supportedBarcodeFormats;

                    _barcodeReader.TryHarder = true;

                    _barcodeReader.ResultFound += _bcReader_ResultFound;
                    _scanTimer.Start();
                });
            }
            else
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Unable to initialize the camera");
                    });
            }
        }

        void _bcReader_ResultFound(Result obj)
        {
            // If a new barcode is found, vibrate the device and display the barcode details in the UI
            if (!obj.Text.Equals(tbBarcodeData.Text))
            {
                VibrateController.Default.Start(TimeSpan.FromMilliseconds(100));
                tbBarcodeType.Text = obj.BarcodeFormat.ToString();
                tbBarcodeData.Text = obj.Text;
                tbBarcodeData.Text = "";
                tbBarcodeType.Text = "";

                NavigationService.Navigate(new Uri("/PivotPageUrunKaydet.xaml?barkodtur=" + obj.BarcodeFormat.ToString() + "&barkod=" + obj.Text, UriKind.RelativeOrAbsolute));
            }
        }

        private void ScanForBarcode()
        {
            //grab a camera snapshot
            _phoneCamera.GetPreviewBufferArgb32(_previewBuffer.Pixels);
            _previewBuffer.Invalidate();

            //scan the captured snapshot for barcodes
            //if a barcode is found, the ResultFound event will fire
            _barcodeReader.Decode(_previewBuffer);

        }

    }
}