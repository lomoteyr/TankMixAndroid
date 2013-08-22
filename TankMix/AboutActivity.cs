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
	[Activity (Label = "About")]			
	public class AboutActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.About);

			// Create your application here

			var editText1 = FindViewById<EditText> (Resource.Id.editText1);





		}
	}
}

