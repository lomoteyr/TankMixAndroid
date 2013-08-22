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

namespace TankMix
{
	[Activity (Label = "Tank Mix Result")]			
	public class ResultActivity : Activity
	{

		public string AreaTreatedByNTFDisplay;
		public string ProductAmountDisplay;
		public string AdjuvantAmountDisplay;
		public string NetTankFillDisplay;

		string[] myValuesFromThird;



		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here

			//RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;



			SetContentView (Resource.Layout.Result);

			var editTextAreaNTF = FindViewById<EditText> (Resource.Id.editTextAreaNTF);

			var editTextProdAmount = FindViewById<EditText> (Resource.Id.editTextProdAmount);

			var editTextAdjAmt = FindViewById<EditText> (Resource.Id.editTextAdjAmt);

			var editTextNetTF = FindViewById<EditText> (Resource.Id.editTextNetTF);


			//Recieve the values from previous screeen (i.e., the Second Activity)
			myValuesFromThird = Intent.GetStringArrayExtra ("myResults");


			AreaTreatedByNTFDisplay = myValuesFromThird[0];
			ProductAmountDisplay = myValuesFromThird[1];
			AdjuvantAmountDisplay = myValuesFromThird[2];
			NetTankFillDisplay = myValuesFromThird[3];

			//write the values to screen

			//1. The Area Treated by NTF

			editTextAreaNTF.Text = AreaTreatedByNTFDisplay;

			//2. The Product Amount

			editTextProdAmount.Text = ProductAmountDisplay;

			//3. Adjuvant Amount

			editTextAdjAmt.Text = AdjuvantAmountDisplay;

			//4. Net tank Fill

			editTextNetTF.Text = NetTankFillDisplay;


		}
	}
}

