
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.IO;
using System;

namespace Jacant
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            EditText txtWeather = FindViewById<EditText>(Resource.Id.TxtWeather);

            // 天气查询结果
            TextView lblCity = FindViewById<TextView>(Resource.Id.LblCityRst);                  // 城市 
            TextView lblCurTmp = FindViewById<TextView>(Resource.Id.LabCurTempRst);             // 当前温度  
            TextView lblWeather = FindViewById<TextView>(Resource.Id.LabWeatherRst);            // 天气
            TextView lblRange = FindViewById<TextView>(Resource.Id.LabRangeRst);                // 范围
            TextView lblUptTime = FindViewById<TextView>(Resource.Id.LabUptTimeRst);            // 更新时间


            button.Click += (sender, e) => {
                HttpHelper helper = new HttpHelper();

                string sUrl = String.Format(@"http://cgi.appx.qq.com/cgi/qqweb/weather/wth/weather.do?
                                            retype=1&city={0}&t={1}",
                                            Uri.EscapeDataString(txtWeather.Text),
                                            DateTime.Now.ToFileTime().ToString());

                try
                {
                    var v = helper.HttpGetRequest(sUrl, null, 10000, null);
                    var rst = new StreamReader(v.GetResponseStream(), System.Text.Encoding.GetEncoding(v.CharacterSet));


                    var vWeather = Newtonsoft.Json.JsonConvert.DeserializeObject<EtWeather>(rst.ReadToEnd());
                    //var vWeather = jss.Deserialize<EtWeather>(rst.ReadToEnd());

                    lblCity.Text = vWeather.city;
                    lblCurTmp.Text = vWeather.now_temperature;
                    lblWeather.Text = vWeather.now_pic_name;
                    lblRange.Text = vWeather.temperature_range;
                    lblUptTime.Text = vWeather.update_time;
                }
                catch (Exception Err)
                {
                    var builder = new Android.App.AlertDialog.Builder(this);
                    builder.SetMessage(Err.Message);
                    builder.SetCancelable(false);
                    builder.SetPositiveButton("OK", delegate { });
                    var dialog = builder.Create();
                    dialog.Show();

                }

            };
        }
    }
}