
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using ZXing;

namespace MonoAndroidDemo
{
	[Activity (Label = "Write:")]			
	public class EncoderActivity : Activity 
	{
		public static BarcodeFormat CurrentFormat = BarcodeFormat.QR_CODE;

		Android.Graphics.Bitmap bitmap = null;

		Button buttonGenerate;
		Button buttonSaveToGallery;
		EditText textValue;
		ImageView imageBarcode;
		TextView textViewEncoderMsg;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			this.SetContentView(Resource.Layout.EncoderActivityLayout);


			buttonGenerate = this.FindViewById<Button>(Resource.Id.buttonEncoderGenerate);
			buttonSaveToGallery = this.FindViewById<Button>(Resource.Id.buttonEncoderSaveBarcode);
			textValue = this.FindViewById<EditText>(Resource.Id.textEncoderValue);
			imageBarcode = this.FindViewById<ImageView>(Resource.Id.imageViewEncoderBarcode);
			textViewEncoderMsg = this.FindViewById<TextView>(Resource.Id.textViewEncoderMsg);

			this.buttonSaveToGallery.Enabled = false;
			this.Title = "Write: " + CurrentFormat.ToString();

			buttonGenerate.Click += (sender, e) => {

				var value = string.Empty;

				this.RunOnUiThread(() => { value = this.textValue.Text; });

				try
				{
					var writer = new BarcodeWriter();
					writer.Format = CurrentFormat; 
					bitmap = writer.Write(value);

					this.RunOnUiThread(() => 
					{
						this.imageBarcode.SetImageDrawable(new Android.Graphics.Drawables.BitmapDrawable(bitmap));
						this.buttonSaveToGallery.Enabled = true;
					});
				}
				catch (Exception ex)
				{
					bitmap = null;
					this.buttonSaveToGallery.Enabled = false;
					this.RunOnUiThread(() => this.textViewEncoderMsg.Text = ex.ToString());
				}
			};

			buttonSaveToGallery.Click += (sender, e) => {
				Android.Provider.MediaStore.Images.Media.InsertImage(this.ContentResolver, bitmap, "Zxing.Net: " + CurrentFormat.ToString(), ""); 
			};


		}
	}
}

