using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StorageTestPage : ContentPage
	{
		public StorageTestPage ()
		{
			InitializeComponent ();
		}
        private void CreateEntity_OnClicked(object sender, EventArgs e)
        {
            var number = new Random(DateTime.Now.Millisecond).Next(1, 1000);
            var entity = new TicketDatabase()
            {
                Subject = "Subject " + number,
                Body = "Body " + number,
                LogsFile = Encoding.ASCII.GetBytes("ASDASDASDADAD")
            };
            DataManager.Instance.SaveTicket(entity);

            DisplayAlert("Saved", "Saved", "Ok");
        }

        private void ViewData_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewDataPage());
        }
    }
}