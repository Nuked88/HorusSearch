using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using System.Windows.Input;
using System.Reflection;
using System.Windows.Threading;

namespace SDKSample
{
    public partial class MediaElementSound : Page
    {

        public delegate void timerTick();
        DispatcherTimer ticks = new DispatcherTimer();
        timerTick tick;
        private void player_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            //Put a breakpoint here if you get errors.
            MessageBox.Show("Error opening video file: " + e.ErrorException.Message);
        }
        private MediaState GetMediaState(MediaElement myMedia)
        {
            FieldInfo hlp = typeof(MediaElement).GetField("_helper", BindingFlags.NonPublic | BindingFlags.Instance);
            object helperObject = hlp.GetValue(myMedia);
            FieldInfo stateField = helperObject.GetType().GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
            MediaState state = (MediaState)stateField.GetValue(helperObject);
            return state;
        }
        // Play the media.
        void OnMouseDownPlayMedia(object sender, MouseButtonEventArgs args)
        {

            // The Play method will begin the media if it is not currently active or
            // resume media if it is paused. This has no effect if the media is
            // already running.
            
            var converter = new ImageSourceConverter();
            if (McMediaElement.Clock != null)
            {
                if (McMediaElement.Clock.CurrentState == ClockState.Active)
                {
                    if (GetMediaState(McMediaElement) == MediaState.Play || McMediaElement.Clock.IsPaused )
                    {
                        playButton.Source = (ImageSource)converter.ConvertFromString("pack://application:,,,/Resources/Images/UI_pause_w.png");
                        McMediaElement.Clock.Controller.Resume();
                    }
                    else
                    {
                        playButton.Source = (ImageSource)converter.ConvertFromString("pack://application:,,,/Resources/Images/UI_play_w.png");
                        McMediaElement.Clock.Controller.Pause();
                    }

                }
            }
            // Initialize the MediaElement property values.
            InitializePropertyValues();
          
        }

        // Pause the media.
        void OnMouseDownPauseMedia(object sender, MouseButtonEventArgs args)
        {

            // The Pause method pauses the media if it is currently running.
            // The Play method can be used to resume.
            McMediaElement.Clock.Controller.Pause();
        }

        // Stop the media.
        void OnMouseDownStopMedia(object sender, MouseButtonEventArgs args)
        {

            // The Stop method stops and resets the media to be played from
            // the beginning.
            McMediaElement.Clock.Controller.Stop();
        }

        // Change the volume of the media.
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            McMediaElement.Volume = (double)volumeSlider.Value;
        }



        // When the media opens, initialize the "Seek To" slider maximum value
        // to the total number of miliseconds in the length of the media clip.
        /*
        private void Element_MediaOpened(object sender, EventArgs e)
        {
            timelineSlider.Maximum = McMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
        }*/

        // When the media playback is finished. Stop() the media to seek to media start.
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            McMediaElement.Stop();
        }

        // Jump to different parts of the media (seek to).
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            int SliderValue = (int)timelineSlider.Value;

            // Overloaded constructor takes the arguments days, hours, minutes, seconds, milliseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            McMediaElement.Position = ts;
        }

        void InitializePropertyValues()
        {
            // Set the media's starting Volume and SpeedRatio to the current value of the
            // their respective slider controls.
            McMediaElement.Volume = (double)volumeSlider.Value;
            timelineSlider.ValueChanged += (o, ev) =>
            {
                    //  McMediaElement.Clock.Controller.Seek(TimeSpan.FromSeconds(timelineSlider.Value), TimeSeekOrigin.BeginTime);
           
            };
            McMediaElement.MediaOpened += (o, ex) =>
            {
           /*     timelineSlider.Maximum = McMediaElement.NaturalDuration.TimeSpan.Seconds;
                McMediaElement.Clock.Controller.Begin();*/
            };

        }
        void ticks_Tick(object sender, object e)
        {
            Dispatcher.Invoke(tick);
        }
        private void McMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {

            ticks.Interval = TimeSpan.FromMilliseconds(1);
            ticks.Tick += ticks_Tick;
            tick = new timerTick(changeStatus);
            ticks.Start();
            var converter = new ImageSourceConverter();
            playButton.Source = (ImageSource)converter.ConvertFromString("pack://application:,,,/Resources/Images/UI_pause_w.png");
            timelineSlider.Maximum = McMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                McMediaElement.Clock.Controller.Begin();
       

        }
        void  changeStatus()
        {
            /* If you want the Slider to Update Regularly Just UnComment the Line Below*/
            timelineSlider.Value = McMediaElement.Position.TotalMilliseconds/1000;
         //   Duration.Text = Milliseconds_to_Minute((long)MediaPlayer.Position.TotalMilliseconds);
        }

        private void TimelineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (timelineSlider.Value > (McMediaElement.Position.TotalMilliseconds / 1000) + 10 || timelineSlider.Value < (McMediaElement.Position.TotalMilliseconds / 1000) - 10) {
                McMediaElement.Clock.Controller.Seek(TimeSpan.FromSeconds(timelineSlider.Value), TimeSeekOrigin.BeginTime);
            }
            
        }
    }
}