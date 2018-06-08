using System;
using Xamarin.Forms;

namespace TimePickerTest.Custom
{
    public class NullableTimePicker : TimePicker
    {
        public event EventHandler<NullableTimeChangedEventArgs> TimeCleared;

        protected virtual void OnTimeCleared(NullableTimeChangedEventArgs e)
        {
            e.NewNullableTime = null;

            TimeCleared?.Invoke(this, e);
        }

        #region Properties

        public string _originalFormat = @"hh\:mm";

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(NullableTimePicker),
            defaultValue: string.Empty);

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }

        public static readonly BindableProperty NullableTimeProperty = BindableProperty.Create(
            nameof(NullableTime),
            typeof(TimeSpan?),
            typeof(NullableTimePicker),
            null,
            defaultBindingMode: BindingMode.TwoWay);

        public TimeSpan? NullableTime
        {
            get { return (TimeSpan?)GetValue(NullableTimeProperty); }
            set { SetValue(NullableTimeProperty, value); UpdateTime(); }
        }

        #endregion Properties

        public NullableTimePicker()
        {
            Format = @"hh\:mm";
        }

        private void UpdateTime()
        {
            if (NullableTime != null)
            {
                if (_originalFormat != null)
                {
                    Format = _originalFormat;
                }
            }
            else
            {
                Format = Placeholder;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null)
            {
                _originalFormat = Format;
                UpdateTime();
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == TimeProperty.PropertyName || (propertyName == IsFocusedProperty.PropertyName && !IsFocused && (Time.ToString("c") == DateTime.Now.ToString("c"))))
            {
                AssignValue();
            }

            if (propertyName == NullableTimeProperty.PropertyName)
            {
                if (NullableTime.HasValue)
                {
                    Time = NullableTime.Value;
                    if (Time.ToString(_originalFormat) == DateTime.Now.Ticks.ToString(_originalFormat))
                    {
                        //this code was done because when date selected is the actual date the "DateProperty" does not raise
                        UpdateTime();
                    }
                }
                else
                {
                    var args = new NullableTimeChangedEventArgs
                    {
                        NewNullableTime = null,
                        OldNullableTime = NullableTime
                    };
                    OnTimeCleared(args);

                    UpdateTime();
                }
            }
        }

        public void CleanTime()
        {
            var args = new NullableTimeChangedEventArgs
            {
                NewNullableTime = null,
                OldNullableTime = NullableTime
            };
            OnTimeCleared(args);

            NullableTime = null;
            UpdateTime();
        }

        public void AssignValue()
        {
            NullableTime = Time;
            UpdateTime();
        }
    }

    public class NullableTimeChangedEventArgs
    {
        public TimeSpan? NewNullableTime { get; set; }

        public TimeSpan? OldNullableTime { get; set; }
    }
}