using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using TimePickerTest.Custom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NullableTimePicker), typeof(TimePickerTest.Droid.Renderers.NullableTimePickerRenderer))]
namespace TimePickerTest.Droid.Renderers
{
    public class NullableTimePickerRenderer : ViewRenderer<NullableTimePicker, EditText>
    {
        private MyTimePickerDialog _dialog;

        public NullableTimePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NullableTimePicker> e)
        {
            base.OnElementChanged(e);

            this.SetNativeControl(new Android.Widget.EditText(Context));
            if (Control == null || e.NewElement == null)
                return;

            var entry = (NullableTimePicker)this.Element;

            this.Control.Click += OnPickerClick;
            this.Control.Text = !entry.NullableTime.HasValue ? entry.Placeholder : Element.Time.ToString(Element.Format);
            this.Control.KeyListener = null;
            this.Control.FocusChange += OnPickerFocusChange;
            this.Control.Enabled = Element.IsEnabled;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Xamarin.Forms.TimePicker.TimeProperty.PropertyName || e.PropertyName == Xamarin.Forms.TimePicker.FormatProperty.PropertyName)
            {
                var entry = (NullableTimePicker)this.Element;

                if (this.Element.Format == entry.Placeholder)
                {
                    this.Control.Text = entry.Placeholder;
                    return;
                }
            }

            base.OnElementPropertyChanged(sender, e);
        }

        private void OnPickerFocusChange(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                ShowTimePicker();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                this.Control.Click -= OnPickerClick;
                this.Control.FocusChange -= OnPickerFocusChange;

                if (_dialog != null)
                {
                    _dialog.Hide();
                    _dialog.Dispose();
                    _dialog = null;
                }
            }

            base.Dispose(disposing);
        }

        private void OnPickerClick(object sender, EventArgs e)
        {
            ShowTimePicker();
        }

        private void SetTime(TimeSpan Time)
        {
            this.Control.Text = Time.ToString(this.Element.Format);
            Element.Time = Time;
        }

        private void ShowTimePicker()
        {
            CreateTimePickerDialog(this.Element.Time.Hours, this.Element.Time.Minutes);
            _dialog.Show();
        }

        private void CreateTimePickerDialog(int hours, int minutes)
        {
            NullableTimePicker view = Element;

            _dialog = new MyTimePickerDialog(Context, (o, e) =>
            {
                view.Time = new TimeSpan(e.HourOfDay, e.Minute, 0);
                ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();

                _dialog = null;
            }, hours, minutes, true);

            _dialog.SetButton("Done", (sender, e) =>
            {
                this.Element.Format = this.Element._originalFormat;
                SetTime(_dialog.SelectedTime);
                this.Element.AssignValue();
            });

            _dialog.SetButton2("Clear", (sender, e) =>
            {
                this.Element.CleanTime();
                Control.Text = this.Element.Format;
            });
        }
    }

    public class MyTimePickerDialog : TimePickerDialog
    {
        public TimeSpan SelectedTime { get; set; }

        public MyTimePickerDialog(Context context, EventHandler<TimeSetEventArgs> callBack, int hourOfDay, int minute, bool is24HourView)
            : base(context, callBack, hourOfDay, minute, is24HourView)
        {
            SelectedTime = new TimeSpan();
        }

        public MyTimePickerDialog(Context context, IOnTimeSetListener listener, int hourOfDay, int minute, bool is24HourView)
            : base(context, listener, hourOfDay, minute, is24HourView)
        {
            SelectedTime = new TimeSpan();
        }

        public MyTimePickerDialog(Context context, int theme, EventHandler<TimeSetEventArgs> callBack, int hourOfDay, int minute, bool is24HourView)
            : base(context, theme, callBack, hourOfDay, minute, is24HourView)
        {
            SelectedTime = new TimeSpan();
        }

        public MyTimePickerDialog(Context context, int themeResId, IOnTimeSetListener listener, int hourOfDay, int minute, bool is24HourView)
            : base(context, themeResId, listener, hourOfDay, minute, is24HourView)
        {
            SelectedTime = new TimeSpan();
        }

        protected MyTimePickerDialog(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            SelectedTime = new TimeSpan();
        }

        public override void OnTimeChanged(Android.Widget.TimePicker view, int hourOfDay, int minute)
        {
            base.OnTimeChanged(view, hourOfDay, minute);

            SelectedTime = new TimeSpan(hourOfDay, minute, 0);
        }
    }
}