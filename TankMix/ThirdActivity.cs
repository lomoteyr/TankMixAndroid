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
using System.Drawing;


namespace TankMix
{
	[Activity (Label = "Tank Information")]			
	public class ThirdActivity : Activity
	{

		string TankCapaUnit;
		string TankRemUnit;

		string TankCapaText;
		string TankRemText;

		string[] myValuesFromSecond;

		//Initialize the passed value
		//variables from first screen
		public string AppVolumeValue;
		public string ProdRateValue;
		public string AdjRateValue;
		public string AppVolumeUnit;
		public string ProdRateUnit;
		public string AdjRateUnit;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.Third);

			//RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

			//Initialize all the UI elements
			EditText editTextTankCapacity = FindViewById<EditText> (Resource.Id.editTextTankCapacity);

			EditText editTextTankRemainder = FindViewById<EditText> (Resource.Id.editTextTankRemainder);

			Spinner spinnerTankCapaUnit = FindViewById<Spinner> (Resource.Id.spinnerTankCapaUnit);

			Spinner spinnerTankRemainderUnit = FindViewById<Spinner> (Resource.Id.spinnerTankRemainderUnit);

			Button buttonResult = FindViewById<Button> (Resource.Id.buttonResult);


			//Populate Spinner Element
			// 1. The Tank Capacity Unit Spinner
			spinnerTankCapaUnit.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinnerTankCapa_ItemSelected);

			var adapterTankCapaUnit = ArrayAdapter.CreateFromResource (
				this, Resource.Array.appTankCapaUnitArray, Android.Resource.Layout.SimpleSpinnerItem);

			adapterTankCapaUnit.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerTankCapaUnit.Adapter = adapterTankCapaUnit;

			// 2. The Tank Remainder Unit Spinner
			spinnerTankRemainderUnit.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinnerTankRem_ItemSelected);

			var adapterTankRemUnit = ArrayAdapter.CreateFromResource (
				this, Resource.Array.appTankRemainderUnitArray, Android.Resource.Layout.SimpleSpinnerItem);

			adapterTankRemUnit.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinnerTankRemainderUnit.Adapter = adapterTankRemUnit;


			//Assign the values from Text Field
			editTextTankCapacity.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {

				TankCapaText = e.Text.ToString ();

			};
			editTextTankRemainder.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {

				TankRemText = e.Text.ToString ();

			};






			//Recieve the values from previous screeen (i.e., the Second Activity)
			myValuesFromSecond = Intent.GetStringArrayExtra ("myValues");


			AppVolumeValue = myValuesFromSecond[0];
			ProdRateValue = myValuesFromSecond[1];
			AdjRateValue = myValuesFromSecond[2];
			AppVolumeUnit = myValuesFromSecond[3];
			ProdRateUnit = myValuesFromSecond[4];
			AdjRateUnit = myValuesFromSecond[5];



			//When button is clicked 
		buttonResult.Click += (sender, e) => {
			//receive values and pass to the third activity


			float a_float;


			//check empty fields
			if (String.IsNullOrEmpty (TankCapaText) || String.IsNullOrEmpty (TankRemText) || String.IsNullOrEmpty (TankCapaUnit)
				|| String.IsNullOrEmpty (TankRemUnit)) {

				new AlertDialog.Builder (this)
						.SetPositiveButton ("OK", (sender1, args) =>
				{
					// User pressed yes
				})
							.SetMessage ("Missing inputs or units")
							.SetTitle ("Error")
							.Show ();


			}

				//check text fields are numeric values
				else if (!float.TryParse (TankCapaText, out a_float) || !float.TryParse (TankRemText, out a_float)) {

				new AlertDialog.Builder (this)
						.SetPositiveButton ("OK", (sender1, args) =>
				{
					// User pressed yes
				})
							.SetMessage ("numeric values only for \n App Vol, Prod Rate and Adj Rate")
							.SetTitle ("Error")
							.Show ();

			}

				//all conditions are met
				else {

				//Convert the string values to float
				double a = Convert.ToDouble (AppVolumeValue);
				double b = Convert.ToDouble (ProdRateValue);
				double c = Convert.ToDouble (AdjRateValue);
				double d = Convert.ToDouble (TankCapaText);
				double f = Convert.ToDouble (TankRemText);

				//declare the variables to calculate
				double NetTankFill;
				double AreaTreatedByNetTankFill;
				double ProductAmount, AreaTByNetTankFill, AdjuvantA, L, AdjuvantAmt;

				double AdjuvantAmount;

				//string values that are displayed on the result page
				string NetTankFillResult;
				string AreaTreatedByNetTankFillResult;
				string ProductAmountResult;
				string AdjuvantAmountResult;

				// This if conditions are to check that the units are serialize such that
				// Imp gpa is Imp gal, L/ha is L, and US gpa is US gal


				//Serialize the Result
				//Change all TankCapaity and Tank Remainder Units to L
				if (AppVolumeUnit.Equals ("L/ha") && TankCapaUnit.Equals ("US gal") && 
					TankRemUnit.Equals ("US gal")) {

					TankCapaUnit = "L";
					TankRemUnit = "L";

					d = d * 3.785;
					f = f * 3.785;

				}

				if (AppVolumeUnit.Equals ("L/ha") && TankCapaUnit.Equals ("Imp gal") && 
					TankRemUnit.Equals ("Imp gal")) {

					TankCapaUnit = "L";
					TankRemUnit = "L";

					d = d * 4.54;
					f = f * 4.54;

				}

				if (AppVolumeUnit.Equals ("L/ha") && TankCapaUnit.Equals ("US gal") && 
					TankRemUnit.Equals ("Imp gal")) {

					TankCapaUnit = "L";
					TankRemUnit = "L";

					d = d * 3.785;
					f = f * 4.54;

				}

				if (AppVolumeUnit.Equals ("L/ha") && TankCapaUnit.Equals ("Imp gal") && 
					TankRemUnit.Equals ("US gal")) {

					TankCapaUnit = "L";
					TankRemUnit = "L";

					d = d * 4.54;
					f = f * 3.785;

				}

				if (AppVolumeUnit.Equals ("L/ha") && TankCapaUnit.Equals ("L") && 
					TankRemUnit.Equals ("Imp gal")) {

					TankRemUnit = "L";

					f = f * 4.54;

				}

				if (AppVolumeUnit.Equals ("L/ha") && TankCapaUnit.Equals ("Imp gal") && 
					TankRemUnit.Equals ("L")) {

					TankCapaUnit = "L";

					d = d * 4.54;

				}

				if (AppVolumeUnit.Equals ("L/ha") && TankCapaUnit.Equals ("US gal") && 
					TankRemUnit.Equals ("L")) {

					TankCapaUnit = "L";

					d = d * 3.785;

				}

				if (AppVolumeUnit.Equals ("L/ha") && TankCapaUnit.Equals ("L") && 
					TankRemUnit.Equals ("US gal")) {

					TankRemUnit = "L";

					f = f * 3.785;

				}

				//Change All Tank Capacity Units to US gallon
				if (AppVolumeUnit.Equals ("US gpa") && TankCapaUnit.Equals ("L") && 
					TankRemUnit.Equals ("L")) {

					TankCapaUnit = "US gal";
					TankRemUnit = "US gal";

					d = d / 3.785;

					f = f / 3.785;

				}

				if (AppVolumeUnit.Equals ("US gpa") && TankCapaUnit.Equals ("Imp gal") && 
					TankRemUnit.Equals ("Imp gal")) {

					TankCapaUnit = "US gal";
					TankRemUnit = "US gal";

					d = d * 1.201;

					f = f * 1.201;

				}

				if (AppVolumeUnit.Equals ("US gpa") && TankCapaUnit.Equals ("US gal") && 
					TankRemUnit.Equals ("L")) {

					TankRemUnit = "US gal";

					f = f / 3.785;

				}

				if (AppVolumeUnit.Equals ("US gpa") && TankCapaUnit.Equals ("US gal") && 
					TankRemUnit.Equals ("Imp gal")) {

					TankRemUnit = "US gal";

					f = f * 1.201;

				}

				if (AppVolumeUnit.Equals ("US gpa") && TankCapaUnit.Equals ("L") && 
					TankRemUnit.Equals ("US gal")) {

					TankCapaUnit = "US gal";

					d = d / 3.785;

				}

				if (AppVolumeUnit.Equals ("US gpa") && TankCapaUnit.Equals ("L") && 
					TankRemUnit.Equals ("Imp gal")) {

					TankCapaUnit = "US gal";
					TankRemUnit = "US gal";

					d = d / 3.785;
					f = f * 1.201;

				}

				if (AppVolumeUnit.Equals ("US gpa") && TankCapaUnit.Equals ("Imp gal") && 
					TankRemUnit.Equals ("L")) {

					TankCapaUnit = "US gal";
					TankRemUnit = "US gal";

					d = d * 1.201;
					f = f / 3.785;

				}

				if (AppVolumeUnit.Equals ("US gpa") && TankCapaUnit.Equals ("Imp gal") && 
					TankRemUnit.Equals ("US gal")) {

					TankCapaUnit = "US gal";

					d = d * 1.201;

				}


				//Convert the other units to Imp gal
				if (AppVolumeUnit.Equals ("Imp gpa") && TankCapaUnit.Equals ("US gal") && 
					TankRemUnit.Equals ("US gal")) {

					TankCapaUnit = "Imp gal";
					TankRemUnit = "Imp gal";

					d = d * 0.8333;
					f = f * 0.8333;

				}

				if (AppVolumeUnit.Equals ("Imp gpa") && TankCapaUnit.Equals ("L") && 
					TankRemUnit.Equals ("L")) {

					TankCapaUnit = "Imp gal";
					TankRemUnit = "Imp gal";

					d = d / 4.54;
					f = f / 4.54;

				}

				if (AppVolumeUnit.Equals ("Imp gpa") && TankCapaUnit.Equals ("US gal") && 
					TankRemUnit.Equals ("L")) {

					TankCapaUnit = "Imp gal";
					TankRemUnit = "Imp gal";

					d = d * 0.8333;
					f = f / 4.54;

				}

				if (AppVolumeUnit.Equals ("Imp gpa") && TankCapaUnit.Equals ("L") && 
					TankRemUnit.Equals ("US gal")) {

					TankCapaUnit = "Imp gal";
					TankRemUnit = "Imp gal";

					d = d / 4.54;
					f = f * 0.8333;

				}

				if (AppVolumeUnit.Equals ("Imp gpa") && TankCapaUnit.Equals ("US gal") && 
					TankRemUnit.Equals ("Imp gal")) {

					TankCapaUnit = "Imp gal";

					d = d * 0.8333;

				}

				if (AppVolumeUnit.Equals ("Imp gpa") && TankCapaUnit.Equals ("Imp gal") && 
					TankRemUnit.Equals ("US gal")) {

					TankRemUnit = "Imp gal";

					f = f * 0.8333;

				}

				if (AppVolumeUnit.Equals ("Imp gpa") && TankCapaUnit.Equals ("L") && 
					TankRemUnit.Equals ("Imp gal")) {

					TankCapaUnit = "Imp gal";

					d = d / 4.54;

				}

				if (AppVolumeUnit.Equals ("Imp gpa") && TankCapaUnit.Equals ("Imp gal") && 
					TankRemUnit.Equals ("L")) {

					TankRemUnit = "Imp gal";

					f = f / 4.54;

				}

				//StartSearch the calculations from here





			//	StartActivity (typeof(ResultActivity));

				//each if condition is to test and compare units and calculate the result

				if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = AreaTByNetTankFill * b;
					ProductAmountResult = ProductAmount + " L";

					AdjuvantAmt = Math.Round (NetTankFill * (c / 100), 2);
					AdjuvantAmountResult = AdjuvantAmt + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);


				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTreatedByNetTankFill = Math.Round (NetTankFill / a, 3);
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTreatedByNetTankFill / b, 2);
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantAmount = Math.Round (AreaTreatedByNetTankFill * c, 2);
					AdjuvantAmountResult = AdjuvantAmount + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTreatedByNetTankFill = Math.Round (NetTankFill / a, 3);
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTreatedByNetTankFill * b, 2);
					ProductAmountResult = ProductAmount + " L";

					AdjuvantAmount = Math.Round (AreaTreatedByNetTankFill * c, 2);
					AdjuvantAmountResult = AdjuvantAmount + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " acres";

					ProductAmount = AreaTreatedByNetTankFill * b;
					ProductAmountResult = ProductAmount + " L";

					AdjuvantAmount = NetTankFill * (c / 100);
					L = Math.Round (AdjuvantAmount * 3.785, 2);
					AdjuvantAmountResult = AdjuvantAmount + " US gal" + " (" + L + "L)";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " acres";

					ProductAmount = AreaTreatedByNetTankFill * b;
					ProductAmountResult = ProductAmount + " gal";

					AdjuvantAmount = NetTankFill * (c / 100);
					L = Math.Round (AdjuvantAmount * 3.785, 2);
					AdjuvantAmountResult = AdjuvantAmount + " US gal" + " (" + L + "L)";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " acres";

					ProductAmount = AreaTreatedByNetTankFill * b;
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantAmount = NetTankFill * (c / 100);
					L = Math.Round (AdjuvantAmount * 3.785, 2);
					AdjuvantAmountResult = AdjuvantAmount + " US gal" + " (" + L + " L)";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " acres";

					ProductAmount = AreaTreatedByNetTankFill * b;
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantAmount = NetTankFill * (c / 100);
					L = Math.Round (AdjuvantAmount * 3.785, 2);
					AdjuvantAmountResult = AdjuvantAmount + " US gal" + " (" + L + "L)";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " acres";

					ProductAmount = AreaTreatedByNetTankFill * b;
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantAmount = NetTankFill * (c / 100);
					L = Math.Round (AdjuvantAmount * 3.785, 2);
					AdjuvantAmountResult = AdjuvantAmount + " US gal" + " (" + L + "L)";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " acres";

					ProductAmount = AreaTreatedByNetTankFill * b;
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantAmount = NetTankFill * (c / 100);
					L = Math.Round (AdjuvantAmount * 3.785, 2);
					AdjuvantAmountResult = AdjuvantAmount + " US gal" + " (" + L + "L)";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " acres";

					ProductAmount = AreaTreatedByNetTankFill * b;
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantAmount = NetTankFill * (c / 100);
					L = Math.Round (AdjuvantAmount * 3.785, 2);
					AdjuvantAmountResult = AdjuvantAmount + " US gal" + " (" + L + "L)";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " acres";

					ProductAmount = AreaTreatedByNetTankFill * 0.405 * b;  //convert the acre to ha
					ProductAmountResult = ProductAmount + " gal";

					AdjuvantAmount = NetTankFill * (c / 100);
					L = Math.Round (AdjuvantAmount * 3.785, 2);
					AdjuvantAmountResult = AdjuvantAmount + " US gal" + " (" + L + "L)";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " acres";

					ProductAmount = (AreaTreatedByNetTankFill * 0.405) * b;  //convert the acre to ha
					ProductAmountResult = ProductAmount + " L";

					AdjuvantAmount = NetTankFill * (c / 100);
					L = Math.Round (AdjuvantAmount * 3.785, 2);
					AdjuvantAmountResult = AdjuvantAmount + " US gal" + " (" + L + "L)";

					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);  //convert the acre to ha
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantAmount = NetTankFill * (c / 100);
					L = Math.Round (AdjuvantAmount * 3.785, 2);
					AdjuvantAmountResult = AdjuvantAmount + " US gal" + " (" + L + "L)";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTreatedByNetTankFill / (0.405 * b), 2);  //convert the acre to ha

					ProductAmountResult = ProductAmount + " cases";

					AdjuvantAmount = NetTankFill * (c / 100);
					AdjuvantAmountResult = AdjuvantAmount + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTreatedByNetTankFill / 0.405) * b, 2);  //convert the acre to ha

					ProductAmountResult = ProductAmount + " L";

					AdjuvantAmount = NetTankFill * (c / 100);
					AdjuvantAmountResult = AdjuvantAmount + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTreatedByNetTankFill / 0.405) * b, 2);  //convert the acre to ha

					ProductAmountResult = ProductAmount + " g";

					AdjuvantAmount = NetTankFill * (c / 100);
					AdjuvantAmountResult = AdjuvantAmount + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTreatedByNetTankFill / 0.405) * b, 2);  //convert the acre to ha

					ProductAmountResult = ProductAmount + " mL";

					AdjuvantAmount = NetTankFill * (c / 100);
					AdjuvantAmountResult = AdjuvantAmount + " L";


					//Pass the results to the output page

					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTreatedByNetTankFill / 0.405) * b, 2);  //convert the acre to ha

					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantAmount = NetTankFill * (c / 100);
					AdjuvantAmountResult = AdjuvantAmount + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTreatedByNetTankFill / 0.405) * b, 2);  //convert the acre to ha

					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantAmount = NetTankFill * (c / 100);
					AdjuvantAmountResult = AdjuvantAmount + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTreatedByNetTankFill / 0.405) * b, 2);  //convert the ha to acre

					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantAmount = NetTankFill * (c / 100);
					AdjuvantAmountResult = AdjuvantAmount + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTreatedByNetTankFill / 0.405) * b, 2);  //convert the ha to acre

					ProductAmountResult = ProductAmount + " oz";

					AdjuvantAmount = NetTankFill * (c / 100);
					AdjuvantAmountResult = AdjuvantAmount + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTreatedByNetTankFill * b, 2);  //convert the acre to ha

					ProductAmountResult = ProductAmount + " g";

					AdjuvantAmount = NetTankFill * (c / 100);
					AdjuvantAmountResult = AdjuvantAmount + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTreatedByNetTankFill = NetTankFill / a;
					AreaTreatedByNetTankFillResult = AreaTreatedByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTreatedByNetTankFill * b, 2);  //convert the acre to ha

					ProductAmountResult = ProductAmount + " mL";

					AdjuvantAmount = NetTankFill * (c / 100);
					AdjuvantAmountResult = AdjuvantAmount + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / (a * 3.785), 2); //convert US gal to L 

					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill / b, 2);  //convert the acre to ha

					ProductAmountResult = ProductAmount + " cases";

					AdjuvantAmt = Math.Round (AreaTByNetTankFill * c, 2);     //cross check with Tom
					AdjuvantAmountResult = AdjuvantAmt + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / (a * 3.785), 2);     //convert US gal to L
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  //convert the acre to ha
					ProductAmountResult = ProductAmount + " L";

					AdjuvantAmt = Math.Round (AreaTByNetTankFill * c, 2);     //cross check with Tom
					AdjuvantAmountResult = AdjuvantAmt + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / (a * 3.785), 2);     //convert US gal to L
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  //convert the acre to ha
					ProductAmountResult = ProductAmount + " g";

					AdjuvantAmt = Math.Round (AreaTByNetTankFill * c, 2);     //cross check with Tom
					AdjuvantAmountResult = AdjuvantAmt + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / (a * 3.785), 2);     //convert US gal to L
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  //convert the acre to ha
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantAmt = Math.Round (AreaTByNetTankFill * c, 2);     //cross check with Tom
					AdjuvantAmountResult = AdjuvantAmt + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / (a * 3.785), 2);     //convert US gal to L
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  //convert the acre to ha
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantAmt = Math.Round (AreaTByNetTankFill * c, 2);     //cross check with Tom
					AdjuvantAmountResult = AdjuvantAmt + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / (a * 3.785), 2);     //convert US gal to L
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  //convert the acre to ha
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantAmt = Math.Round (AreaTByNetTankFill * c, 2);     //cross check with Tom
					AdjuvantAmountResult = AdjuvantAmt + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / (a * 3.785), 2);     //convert US gal to L
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  //convert the acre to ha
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantAmt = Math.Round (AreaTByNetTankFill * c, 2);     //cross check with Tom
					AdjuvantAmountResult = AdjuvantAmt + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / (a * 3.785), 2);     //convert US gal to L
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  //convert the acre to ha
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantAmt = Math.Round (AreaTByNetTankFill * c, 2);     //cross check with Tom
					AdjuvantAmountResult = AdjuvantAmt + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / (a * 3.785), 2);     //convert US gal to L
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);  //convert the acre to ha
					ProductAmountResult = ProductAmount + " L";

					AdjuvantAmt = Math.Round (AreaTByNetTankFill * c, 2);     //cross check with Tom
					AdjuvantAmountResult = AdjuvantAmt + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / (a * 3.785), 2);     //convert US gal to L
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);  //convert the acre to ha
					ProductAmountResult = ProductAmount + " g";

					AdjuvantAmt = Math.Round (AreaTByNetTankFill * c, 2);     //cross check with Tom
					AdjuvantAmountResult = AdjuvantAmt + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / (a * 3.785), 2);     //convert US gal to L
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);  //convert the acre to ha
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantAmt = Math.Round (AreaTByNetTankFill * c, 2);     //cross check with Tom
					AdjuvantAmountResult = AdjuvantAmt + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill / b, 2);  
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);   //convert acre to ha  
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);   //convert acre to ha  
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);   //convert acre to ha  
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill / b, 2);   //convert acre to ha  
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);   //convert acre to ha  
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);   //convert acre to ha  
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);    
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);    
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);    
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);    
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);    
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2); //convert acre to ha   
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2); //convert acre to ha   
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2); //convert acre to ha   
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill / b, 2);   
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);   
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);   
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);   
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);   
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);   
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);   
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);   
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);   //convert acre to ha
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);   //convert acre to ha
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);   //convert acre to ha
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill / b, 2);   //convert acre to ha
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantA = Math.Round ((NetTankFill * (c / 100)), 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " US gal";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill / b, 2);   //convert acre to ha
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);   //convert acre to ha
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page
					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);  //convert acre to ha
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);  //convert acre to ha
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);  //convert acre to ha
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("US gpa") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("US gal") && TankRemUnit.Equals ("US gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " US gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);  //convert acre to ha
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom
					AdjuvantAmountResult = AdjuvantA + " L ghu";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) / b, 2);  // convert ha to acre
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);  // convert ha to acre
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);  // convert ha to acre
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);  // convert ha to acre
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);  // convert ha to acre
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);  // convert ha to acre
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);  // convert ha to acre
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);  // convert ha to acre
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) / b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";

					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) / b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) / b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * (c / 0.405)), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round ((AreaTByNetTankFill / 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("L/ha") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("L") && TankRemUnit.Equals ("L")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " L";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " ha";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round ((AreaTByNetTankFill / 0.405) * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				}

					//lmp ga

					else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill / b, 2);  
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * c), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * c), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * c), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * c), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * c), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * c), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * c), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * c), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);     // convert acre to ha
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * c), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);     // convert acre to ha
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * c), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("L/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);     // convert acre to ha
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round ((AreaTByNetTankFill * c), 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " L";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill / b, 2);     
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page
					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);     // convert acre to ha
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom ()
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);     // convert acre to ha
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom ()
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("mL/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);     // convert acre to ha
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom ()
					AdjuvantAmountResult = AdjuvantA + " mL";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill / b, 2);     
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);     // convert acre to ha
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom ()
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);     // convert acre to ha
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom ()
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("US fl oz/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);     // convert acre to ha
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom ()
					AdjuvantAmountResult = AdjuvantA + " US fl oz";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill / b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " cases";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert acre to ha)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom ()
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom ()
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom ()
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom ()
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom ()
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom ()
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " oz";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom ()
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " L";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " g";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("quart/acre") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);     
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);     // convert ha to acre
					ProductAmountResult = ProductAmount + " mL";

					AdjuvantA = Math.Round (AreaTByNetTankFill * c, 2);    //check from Tom (convert ha to acre)
					AdjuvantAmountResult = AdjuvantA + " US quart";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("acres/case") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill / b, 2);  

					ProductAmountResult = ProductAmount + " cases";

					AdjuvantAmt = Math.Round (NetTankFill * (c / 100), 2);
					AdjuvantAmountResult = AdjuvantAmt + " Imp gal";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("L/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  

					ProductAmountResult = ProductAmount + " L";

					AdjuvantAmt = Math.Round (NetTankFill * (c / 100), 2);
					AdjuvantAmountResult = AdjuvantAmt + " Imp gal";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("g/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  

					ProductAmountResult = ProductAmount + " g";

					AdjuvantAmt = Math.Round (NetTankFill * (c / 100), 2);
					AdjuvantAmountResult = AdjuvantAmt + " Imp gal";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results); 
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("mL/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  

					ProductAmountResult = ProductAmount + " mL";

					AdjuvantAmt = Math.Round (NetTankFill * (c / 100), 2);
					AdjuvantAmountResult = AdjuvantAmt + " Imp gal";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US fl oz/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  

					ProductAmountResult = ProductAmount + " US fl oz";

					AdjuvantAmt = Math.Round (NetTankFill * (c / 100), 2);
					AdjuvantAmountResult = AdjuvantAmt + " Imp gal";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US pint/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  

					ProductAmountResult = ProductAmount + " US pint";

					AdjuvantAmt = Math.Round (NetTankFill * (c / 100), 2);
					AdjuvantAmountResult = AdjuvantAmt + " Imp gal";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("US quart/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  

					ProductAmountResult = ProductAmount + " US quart";

					AdjuvantAmt = Math.Round (NetTankFill * (c / 100), 2);
					AdjuvantAmountResult = AdjuvantAmt + " Imp gal";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("oz/acre") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round (AreaTByNetTankFill * b, 2);  

					ProductAmountResult = ProductAmount + " oz";

					AdjuvantAmt = Math.Round (NetTankFill * (c / 100), 2);
					AdjuvantAmountResult = AdjuvantAmt + " Imp gal";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("g/ha") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);  //convert the acre to ha

					ProductAmountResult = ProductAmount + " g";

					AdjuvantAmt = Math.Round (NetTankFill * (c / 100), 2);
					AdjuvantAmountResult = AdjuvantAmt + " Imp gal";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("mL/ha") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);  //convert the acre to ha

					ProductAmountResult = ProductAmount + " mL";

					AdjuvantAmt = Math.Round (NetTankFill * (c / 100), 2);
					AdjuvantAmountResult = AdjuvantAmt + " Imp gal";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else if (AppVolumeUnit.Equals ("Imp gpa") && ProdRateUnit.Equals ("L/ha") && AdjRateUnit.Equals ("%v/v") && 
					TankCapaUnit.Equals ("Imp gal") && TankRemUnit.Equals ("Imp gal")) {

					NetTankFill = Math.Round (d - f, 3);
					NetTankFillResult = NetTankFill + " Imp gal";

					AreaTByNetTankFill = Math.Round (NetTankFill / a, 2);
					AreaTreatedByNetTankFillResult = AreaTByNetTankFill + " acres";

					ProductAmount = Math.Round ((AreaTByNetTankFill * 0.405) * b, 2);  //convert the acre to ha

					ProductAmountResult = ProductAmount + " L";

					AdjuvantAmt = Math.Round (NetTankFill * (c / 100), 2);
					AdjuvantAmountResult = AdjuvantAmt + " Imp gal";


					//Pass the results to the output page

					string[] allResults = {
						AreaTreatedByNetTankFillResult,
						ProductAmountResult,
						AdjuvantAmountResult,
						NetTankFillResult
					};

					var results = new Intent (this, typeof(ResultActivity));
					results.PutExtra ("myResults", allResults);


					StartActivity (results);  
				} else {

					new AlertDialog.Builder (this)
							.SetPositiveButton ("OK", (sender1, args) =>
					{
						// User pressed yes
					})
								.SetMessage ("Consider revising your units \n Choose units in US gpa, L, acres and also consider choosing the same units for Tank Capacity and Tank Remainder at Fill")
								.SetTitle ("Error")
								.Show ();
							
					}
				

			}



		};



			/*
			editTextTankCapacity.ShouldReturn = (s) => {
				editTextTankCapacity.ResignFirstResponder ();
				return true;
			};

			editTextTankRemainder.ShouldReturn = (s) => {
				editTextTankRemainder.ResignFirstResponder ();
				return true;
			};
                        */
		
		}

		private void spinnerTankCapa_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;

			string toast = string.Format ("{0}", spinner.GetItemAtPosition (e.Position));

			TankCapaUnit = toast;

			//Toast.MakeText (this, toast, ToastLength.Long).Show ();
		}

		private void spinnerTankRem_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;

			string toast = string.Format ("{0}", spinner.GetItemAtPosition (e.Position));

			TankRemUnit = toast;

			//Toast.MakeText (this, toast, ToastLength.Long).Show ();
		}
	}
}

