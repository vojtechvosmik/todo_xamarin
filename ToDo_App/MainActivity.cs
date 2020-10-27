using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Content;
using System;
using ToDo_App.Helper;
using System.Collections.Generic;

namespace ToDo_App
{
    [Activity(
        MainLauncher = true,
        Label = "todo listík pro pana Olžbuta", 
        Theme = "@style/AppTheme"
    )]
    public class MainActivity : AppCompatActivity
    {
        CustomAdapter losAdapteros;
        ListView seznamTasku;
        EditText edittexticek;
        DbHelper databazicka;


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_item, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId){
                case Resource.Id.action_add:
                    edittexticek = new EditText(this);
                    Android.Support.V7.App.AlertDialog dialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                        .SetTitle("přidat úkol")
                        .SetMessage("napiste co chcete udelat:")
                        .SetPositiveButton("pridat", OkAction)
                        .SetView(edittexticek)
                        .SetNegativeButton("zrusit", CancelAction)
                        .Create();
                    dialog.Show();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void OkAction(object sender, DialogClickEventArgs e)
        {
            string task = edittexticek.Text;
            databazicka.InsertNewTask(task);
            LoadTaskList();
        }

        private void CancelAction(object sender, DialogClickEventArgs e)
        {
        }

        public void LoadTaskList()
        {
            List<string> taskList = databazicka.getTaskList();
            losAdapteros = new CustomAdapter(this, taskList, databazicka);
            seznamTasku.Adapter = losAdapteros;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            databazicka = new DbHelper(this);
            seznamTasku = FindViewById<ListView>(Resource.Id.listTask);

            LoadTaskList();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}