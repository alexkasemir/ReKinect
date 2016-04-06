﻿
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
    using System.Windows.Media.Effects;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.ComponentModel;
    using Microsoft.Kinect;
    using System.Windows.Threading;

    namespace WpfApplication2
    {
        /// <summary>
        /// Interaction logic for Window1.xaml
        /// </summary>
        public partial class InterviewWindow : Window, INotifyPropertyChanged
        {
            /// <summary> Active Kinect sensor </summary>
            private KinectSensor kinectSensor = null;

            /// <summary> Array for the bodies (Kinect will track up to 6 people simultaneously) </summary>
            private Body[] bodies = null;

            /// <summary> Reader for body frames </summary>
            private BodyFrameReader bodyFrameReader = null;

            /// <summary> Current status text to display </summary>
            private string statusText = null;

            private string[] questions = new string[] { "Hello?", "What?", "Am?", "I?", "Doing?", "Here?" };
            private int question_index;

            /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
            private GestureDetector gestureDetector = null;

            private DispatcherTimer timer;
            private int counter = 0;
            private bool paused = false;

            public InterviewWindow()
            {
                question_index = 0;

                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();


                // only one sensor is currently supported
                this.kinectSensor = KinectSensor.GetDefault();

                // set IsAvailableChanged event notifier
                this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

                // open the sensor
                this.kinectSensor.Open();

                // set the status text
                this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                                : Properties.Resources.NoSensorStatusText;

                // open the reader for the body frames
                this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

                // set the BodyFramedArrived event notifier
                this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;
            
                // initialize the MainWindow
                this.InitializeComponent();

                // set our data context objects for display in UI
                //this.DataContext = this;

                // create a gesture detector for each body (6 bodies => 6 detectors) and create content controls to display results in the UI
                GestureResultView result = new GestureResultView(0, false, false, 0.0f);
                GestureDetector detector = new GestureDetector(this.kinectSensor, result);
                this.gestureDetector = detector;
                // split gesture results across the first two columns of the content grid
                //ContentControl contentControl = new ContentControl();
                //contentControl.Content = this.gestureDetector.GestureResultView;
                //Grid.SetColumn(contentControl, 0);
                //Grid.SetRow(contentControl, 0);
                //this.contentGrid.Children.Add(contentControl);
            }

            private void Window_KeyDown(object sender, KeyEventArgs e)
            {
                if(e.Key == Key.Space)
                {
                    if (!paused)
                    {
                        var blur = new BlurEffect();
                        var current = this.Background;
                        var blurRadius = 5;
                        this.Background = new SolidColorBrush(Colors.DarkGray);
                        this.Effect = blur;
                        popup.Visibility = System.Windows.Visibility.Visible;
                        popup.IsOpen = true;
                        timer.IsEnabled = false;
                        paused = true;
                    }
                    else
                    {
                        this.Effect = null;
                        popup.IsOpen = false;
                        timer.IsEnabled = true;
                        paused = false;
                        this.Background = new SolidColorBrush(Colors.White);
                    }
                }
            }

            void timer_Tick(object sender, EventArgs e)
            {
                //lblTime.Content = counter;
                counter++;
            }

            /// <summary>
            /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary>
            /// Gets or sets the current status text to display
            /// </summary>
            public string StatusText
            {
                get
                {
                    return this.statusText;
                }

                set
                {
                    if (this.statusText != value)
                    {
                        this.statusText = value;

                        // notify any bound elements that the text has changed
                        if (this.PropertyChanged != null)
                        {
                            this.PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                        }
                    }
                }
            }

            /// <summary>
            /// Execute shutdown tasks
            /// </summary>
            /// <param name="sender">object sending the event</param>
            /// <param name="e">event arguments</param>
            private void MainWindow_Closing(object sender, CancelEventArgs e)
            {
                if (this.bodyFrameReader != null)
                {
                    // BodyFrameReader is IDisposable
                    this.bodyFrameReader.FrameArrived -= this.Reader_BodyFrameArrived;
                    this.bodyFrameReader.Dispose();
                    this.bodyFrameReader = null;
                }

                if (this.gestureDetector != null)
                {
                    // The GestureDetector contains disposable members (VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader)
                    this.gestureDetector.Dispose();
                }

                if (this.kinectSensor != null)
                {
                    this.kinectSensor.IsAvailableChanged -= this.Sensor_IsAvailableChanged;
                    this.kinectSensor.Close();
                    this.kinectSensor = null;
                }
            }

            /// <summary>
            /// Handles the event when the sensor becomes unavailable (e.g. paused, closed, unplugged).
            /// </summary>
            /// <param name="sender">object sending the event</param>
            /// <param name="e">event arguments</param>
            private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
            {
                // on failure, set the status text
                this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                                : Properties.Resources.SensorNotAvailableStatusText;
            }

            /// <summary>
            /// Handles the body frame data arriving from the sensor and updates the associated gesture detector object for each body
            /// </summary>
            /// <param name="sender">object sending the event</param>
            /// <param name="e">event arguments</param>
            private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
            {
                System.Diagnostics.Debug.WriteLine("hello");
                bool dataReceived = false;

                using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
                {
                    if (bodyFrame != null)
                    {
                        if (this.bodies == null)
                        {
                            // creates an array of 6 bodies, which is the max number of bodies that Kinect can track simultaneously
                            this.bodies = new Body[bodyFrame.BodyCount];
                        }

                        // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                        // As long as those body objects are not disposed and not set to null in the array,
                        // those body objects will be re-used.
                        bodyFrame.GetAndRefreshBodyData(this.bodies);
                        dataReceived = true;
                    }
                }

                if (dataReceived)
                {

                    // we may have lost/acquired bodies, so update the corresponding gesture detectors
                    if (this.bodies != null)
                    {
                        // loop through all bodies to see if any of the gesture detectors need to be updated
                        int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
                        for (int i = 0; i < maxBodies; ++i)
                        {
                            Body body = this.bodies[i];
                            ulong trackingId = body.TrackingId;

                            // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                            if (trackingId != this.gestureDetector.TrackingId && body.IsTracked)
                            {
                                this.gestureDetector.TrackingId = trackingId;

                                // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                                // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                                this.gestureDetector.IsPaused = trackingId == 0;
                            }
                        }
                    }
                    int num_detected = this.gestureDetector.num_gestures_detected;
                    switch (num_detected)
                    {
                        case 0:
                            this.rect_feedback.Fill = new SolidColorBrush(Colors.LawnGreen);
                            break;
                        case 1:
                            this.rect_feedback.Fill = new SolidColorBrush(Colors.Wheat);
                            break;
                        case 2:
                            this.rect_feedback.Fill = new SolidColorBrush(Colors.Coral);
                            break;
                        case 3:
                            this.rect_feedback.Fill = new SolidColorBrush(Colors.Tomato);
                            break;
                    }
                }
            }



        private void button_end_interview_click(object sender, RoutedEventArgs e)
        {
            ScoreWindow window = new ScoreWindow();
            window.Width = this.ActualWidth;
            window.Height = this.ActualHeight;
            if (this.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Maximized;
            }
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.setScore(this.gestureDetector.GestureResultView.Overall_Score);
            this.Close();
            window.Show();
        }

        private void button_next_question_click(object sender, RoutedEventArgs e)
        {
            question_index++;
            if (question_index == questions.Length)
            {
                this.button_end_interview_click(this, null);
            }
            else
            {
                //question_index = question_index % questions.Length;
                this.label.Content = questions[question_index];
            }
        }
    }
}
