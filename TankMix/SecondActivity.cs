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
using Mono;

namespace TankMix
{
	[Activity (Label = "Product Information")]			
	public class SecondActivity : Activity
	{

		string AppVolUnit;
		string ProdRateUnit;
		string AdjRateUnit;

		string AppVolText;
		string ProdRateText;
		string AdjRateText;



		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.Second);

			//RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

			//Initialize the elements

			var editTextAppVol = FindViewById<EditText> (Resource.Id.editTextAppVol);

			var editTextProdRate = FindViewById<EditText> (Resource.Id.editTextProdRate);

			var editTextAdjRate = FindViewById<EditText> (Resource.Id.editTextAdjRate);


			Spinner spinnerAppUnit = FindViewById<Spinner> (Resource.Id.spinnerAppUnit);

			Spinner spinnerProdUnit = FindViewById<Spinner> (Resource.Id.spinnerProdUnit);

			Spinner spinnerAdjUnit = FindViewById<Spinner> (Resource.Id.spinnerAdjUnit);


			Button buttonTankInfo = FindViewById<Button> (Resource.Id.buttonTankIfo);


			//Populate Spinner Element

			// 1. The Application Unit Spinner
			spinnerAppUnit.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinner_ItemSelected);

			var adapter = ArrayAdapter.CreateFromResource (
				this, Resource.Array.appUnitArray, Android.Resource.Layout.SimpleSpinnerItem);

			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerAppUnit.Adapter = adapter;

			// 2. The Application Unit Spinner
			spinnerProdUnit.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinnerProdUnit_ItemSelected);

			var adapterProdUnit = ArrayAdapter.CreateFromResource (
				this, Resource.Array.prodUnitArray, Android.Resource.Layout.SimpleSpinnerItem);

			adapterProdUnit.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerProdUnit.Adapter = adapterProdUnit;

			// 3. The Application Unit Spinner
			spinnerAdjUnit.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinnerAdjUnit_ItemSelected);

			var adapterAdjUnit = ArrayAdapter.CreateFromResource (
				this, Resource.Array.adjUnitArray, Android.Resource.Layout.SimpleSpinnerItem);

			adapterAdjUnit.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerAdjUnit.Adapter = adapterAdjUnit;


			//Assign the values from Text Field

			editTextAppVol.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {

				AppVolText = e.Text.ToString ();

			};
			editTextProdRate.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {

				ProdRateText = e.Text.ToString ();

			};
			editTextAdjRate.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {

				AdjRateText = e.Text.ToString ();

			};




			//When Button is clicked

			buttonTankInfo.Click += (sender, e) => {
				//receive values and pass to the third activity
				float a;
				float zero;



				//check empty fields
				if (String.IsNullOrEmpty(AppVolText) || String.IsNullOrEmpty(ProdRateText) || String.IsNullOrEmpty(AdjRateText)
				    || String.IsNullOrEmpty(AppVolUnit) || String.IsNullOrEmpty(ProdRateUnit) 
				    || String.IsNullOrEmpty(AdjRateUnit)){

					new AlertDialog.Builder(this)
						.SetPositiveButton("OK", (sender1, args) =>
						                   {
							// User pressed yes
						})
							.SetMessage("Missing inputs or units")
							.SetTitle("Error")
							.Show();


				}

				//check text fields are numeric values
				else if (!float.TryParse(AppVolText, out a) || !float.TryParse(ProdRateText, out a) 
				         || !float.TryParse(AdjRateText, out a)) {

					new AlertDialog.Builder(this)
						.SetPositiveButton("OK", (sender1, args) =>
						                   {
							// User pressed yes
						})
							.SetMessage("numeric values only for \n App Vol, Prod Rate and Adj Rate")
							.SetTitle("Error")
							.Show();
				
				} 

				//Check the inputs are not zero.
				else if (float.TryParse(AppVolText, out zero) && zero == 0) {

					new AlertDialog.Builder(this)
						.SetPositiveButton("OK", (sender1, args) =>
						                   {
							// User pressed yes
						})
							.SetMessage("Application Volume cannot be zero")
							.SetTitle("Error")
							.Show();

				}

				//when all conditions are met
				else {

					string[] allValues = {AppVolText, ProdRateText, AdjRateText, AppVolUnit, ProdRateUnit, AdjRateUnit};

					var third = new Intent (this, typeof (ThirdActivity));
					third.PutExtra ("myValues", allValues);


					StartActivity (third);


				}

		
		};



		}

		private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;

			string toast = string.Format ("{0}", spinner.GetItemAtPosition (e.Position));

			AppVolUnit = toast;

			//Toast.MakeText (this, toast, ToastLength.Long).Show ();
		}

		private void spinnerProdUnit_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;

			string toast = string.Format ("{0}", spinner.GetItemAtPosition (e.Position));

			ProdRateUnit = toast;

			//Toast.MakeText (this, toast, ToastLength.Long).Show ();
		}

		private void spinnerAdjUnit_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;

			string toast = string.Format ("{0}", spinner.GetItemAtPosition (e.Position));

			AdjRateUnit = toast;

			//Toast.MakeText (this, toast, ToastLength.Long).Show ();
		}
	}
}

