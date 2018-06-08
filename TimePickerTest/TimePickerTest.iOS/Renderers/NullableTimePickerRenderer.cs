using System;
using System.Collections.Generic;
using System.ComponentModel;
using UIKit;
using TimePickerTest.Custom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NullableTimePicker), typeof(VeloGuideApp.iOS.Renderers.NullableTimePickerRenderer))]

namespace VeloGuideApp.iOS.Renderers
{
    public class NullableTimePickerRenderer : TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && this.Control != null)
            {
                this.AddClearButton();

                this.Control.BorderStyle = UITextBorderStyle.RoundedRect;
                //Control.Layer.BorderColor = UIColor.LightGray.CGColor;
                //Control.Layer.BorderWidth = 1;

                var entry = (NullableTimePicker)this.Element;
                if (!entry.NullableTime.HasValue)
                {
                    this.Control.Text = entry.Placeholder;
                }

                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    this.Control.Font = UIFont.SystemFontOfSize(25);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Check if the property we are updating is the format property
            if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
            {
                var entry = (NullableTimePicker)this.Element;

                // If we are updating the format to the Placeholder then just update the text and return
                if (this.Element.Format == entry.Placeholder)
                {
                    this.Control.Text = entry.Placeholder;
                    return;
                }
            }

            base.OnElementPropertyChanged(sender, e);
        }

        private void AddClearButton()
        {
            var originalToolbar = this.Control.InputAccessoryView as UIToolbar;

            if (originalToolbar != null && originalToolbar.Items.Length <= 2)
            {
                var clearButton = new UIBarButtonItem("Clear", UIBarButtonItemStyle.Plain, ((sender, ev) =>
                {
                    NullableTimePicker baseDatePicker = this.Element as NullableTimePicker;
                    this.Element.Unfocus();
                    this.Element.Time = DateTime.Now.TimeOfDay;
                    baseDatePicker.CleanTime();
                }));

                var newItems = new List<UIBarButtonItem>();
                foreach (var item in originalToolbar.Items)
                {
                    newItems.Add(item);
                }

                newItems.Insert(0, clearButton);

                originalToolbar.Items = newItems.ToArray();
                originalToolbar.SetNeedsDisplay();
            }
        }
    }
}