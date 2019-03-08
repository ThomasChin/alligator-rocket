using System;

using Crawl.Views;
using Xamarin.Forms;
using SQLite;

namespace Crawl
{
	public partial class App : Application
	{
        // Initialize App
		public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

            // Load The Mock Datastore by default
            Crawl.Services.MasterDataStore.ToggleDataStore(Models.DataStoreEnum.Mock);
        }

        // Database Connection
	    static SQLiteAsyncConnection _database;

        // Connect to DB.
        public static SQLiteAsyncConnection Database
	    {
	        get
	        {
	            if (_database == null)
	            {
	                _database = new SQLiteAsyncConnection(DependencyService.Get<IFileHelper>().GetLocalFilePath("SQLLiteDBName.db3"));
	            }
	            return _database;
	        }
	    }
    }
}
