//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SDKTemplate
{
    public sealed partial class Scenario1_GetAndSetPin : Page
    {
        // Use GPIO pin 5 to set values
        private const int LED_1 = 2; // physical pin 3
        private const int LED_2 = 10; // physical pin 19
        private const int BUTTON1 = 8; // physical pin 24
        private const int BUTTON2 = 7; // physical pin 26

        private GpioPin led1;
        private GpioPin led2;
        private GpioPin button1;
        private GpioPin button2;

        // private Boolean runExperiment = false;

        public Scenario1_GetAndSetPin()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            StopScenario();
        }

        void StartScenario()
        {
            var gpio = GpioController.GetDefault();

            // Set up our GPIO pin for setting values.
            // If this next line crashes with a NullReferenceException,
            // then the problem is that there is no GPIO controller on the device.
            led1 = gpio.OpenPin(LED_1);
            led2 = gpio.OpenPin(LED_2);
            button1 = gpio.OpenPin(BUTTON1);
            button2 = gpio.OpenPin(BUTTON2);

            // Configure pin for output.
            led1.SetDriveMode(GpioPinDriveMode.Output);
            led2.SetDriveMode(GpioPinDriveMode.Output);
            button1.SetDriveMode(GpioPinDriveMode.Input);
            button2.SetDriveMode(GpioPinDriveMode.Input);
        }

        void StopScenario()
        {
            // Release the GPIO pin.
            if (led1 != null && led2!= null && button1 != null && button2 != null)
            {
                led1.Dispose();
                led2.Dispose();
                button1.Dispose();
                button2.Dispose();
                led1 = null;
                led2 = null;
                button1 = null;
                button2 = null;
            }
        }

        void StartStopScenario()
        {
            if (led1 != null && led2 != null)
            {
                StopScenario();
                StartStopButton.Content = "Start";
                ScenarioControls.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                StartScenario();
                StartStopButton.Content = "Stop";
                ScenarioControls.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void SetLed1High()
        {
            // Set the pin value to High.
            led1.Write(GpioPinValue.High);
        }

        private void SetLed1Low()
        {
            // Set the pin value to Low.
            led1.Write(GpioPinValue.Low);
        }


        private void SetLed2High()
        {
            // Set the pin value to High.
            led2.Write(GpioPinValue.High);
        }

        private void SetLed2Low()
        {
            // Set the pin value to Low.
            led2.Write(GpioPinValue.Low);
        }

        private void GetPinValue()
        {
            // Change the GUI to reflect the current pin value.
            string led1_result = "LED1 is: " + led1.Read().ToString();
            string led2_result = "LED2 is: " + led2.Read().ToString();

            CurrentPinValue.Text = "\n" + led1_result + "\n" + led2_result;
        }

        private void Experiment() 
        {                                   
            while (true){
                // exit if both buttons pressed
                if (button1.Read() == GpioPinValue.Low && button2.Read() == GpioPinValue.Low)
                {
                    break;
                }
                // button1 pressed -> turn LED_1 ON
                else if (button1.Read() == GpioPinValue.Low && button2.Read() == GpioPinValue.High)
                {
                    led1.Write(GpioPinValue.Low);
                    led2.Write(GpioPinValue.High);
                }
                // button2 pressed -> turn LED_2 ON
                else if (button1.Read() == GpioPinValue.High && button2.Read() == GpioPinValue.Low)
                {
                    led1.Write(GpioPinValue.High);
                    led2.Write(GpioPinValue.Low);
                }
                // no button pressed
                else
                {
                    led1.Write(GpioPinValue.High);
                    led2.Write(GpioPinValue.High);
                }                
            }
        }
       
    }
}

