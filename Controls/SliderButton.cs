using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace DM_Notes.Controls
{
    public class SliderButton : ToggleButton
    {
        public static readonly DependencyProperty OnLabelProperty =  DependencyProperty.Register("OnLabel", typeof(string), typeof(SliderButton), new PropertyMetadata("An"));
        public string OnLabel
        {
            get => (string)GetValue(OnLabelProperty);
            set => SetValue(OnLabelProperty, value);
        }


        public static readonly DependencyProperty OffLabelProperty = DependencyProperty.Register("OffLabel", typeof(string), typeof(SliderButton), new PropertyMetadata("Aus"));
        public string OffLabel
        {
            get => (string)GetValue(OffLabelProperty);
            set => SetValue(OffLabelProperty, value);
        }


        public SliderButton()
        {
            DefaultStyleKey = typeof(SliderButton);
        }
    }
}
