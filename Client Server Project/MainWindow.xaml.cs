using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server_Side_Application.Models;

namespace Client_Server_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Extensions.mainThis = this;
            tabPlay.IsEnabled = false;
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var vrResult = await DoRegister();

            if (vrResult.Contains("errors"))
            {
                dynamic result = JObject.Parse(vrResult);
                var vr1 = result.errors;
                MessageBox.Show(vr1);
            }
            else
            {
                MessageBox.Show(vrResult);
            }

            tabLogin.IsSelected = true;
        }

        async private Task<string> DoRegister()
        {
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 4, 0);
               
                RegistrationModel userObject = new RegistrationModel();
                userObject.UserName = txtUserNameRegister.Text;
                userObject.Password = txtPassWordRegister.Password.ToString().ComputeSha256Hash();

             
                client.BaseAddress = new Uri(Extensions.srGlobalUrl);

                string serializedObject = JsonConvert.SerializeObject(userObject);

                // serialize your json using newtonsoft json serializer then add it to the StringContent
                var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                // method address would be like api/callUber:SomePort for example
                var result = await client.PostAsync("api/DoRegister", content);
                string resultContent = await result.Content.ReadAsStringAsync();

                return resultContent;
            }
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var vrResult = await DoLogin();

            if (vrResult.Contains("errors"))
            {
                dynamic result = JObject.Parse(vrResult);
                var vr1 = result.errors;
                MessageBox.Show(vr1);
            }


            if (vrResult.Contains("Success"))
            {
                Extensions.userInstance = JsonConvert.DeserializeObject<LoggedUser>(vrResult);

                await Extensions.refreshUserInstance();

                tabRegister.IsEnabled = false;
                tabLogin.IsEnabled = false;
                tabPlay.IsEnabled = true;
                tabPlay.IsSelected = true;
            }



        }

        async private Task<string> DoLogin()
        {
            using (var client = new HttpClient())
            {
                RegistrationModel userObject = new RegistrationModel();
                userObject.UserName = txtUsernameLogin.Text;
                userObject.Password = txtPasswordLogin.Password.ToString().ComputeSha256Hash();

       
                client.BaseAddress = new Uri(Extensions.srGlobalUrl);

                string serializedObject = JsonConvert.SerializeObject(userObject);

                // serialize your json using newtonsoft json serializer then add it to the StringContent
                var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                // method address would be like api/callUber:SomePort for example
                var result = await client.PostAsync("api/DoLogin", content);
                string resultContent = await result.Content.ReadAsStringAsync();

                return resultContent;
            }


        }

        private async void  btnHuntMonster_Click(object sender, RoutedEventArgs e)
        {
            btnHuntMonster.IsEnabled = false;

            var vrResult = await executeHunting();

            UserBattle battleResult = JsonConvert.DeserializeObject<UserBattle>(vrResult);

            lstBattleResult.ItemsSource = battleResult.lstTurnResults;

            lstBattleResult.SelectedIndex = 0;
           await Extensions.refreshUserInstance();
            btnHuntMonster.IsEnabled = true;
        }

        private static async Task<string> executeHunting()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Extensions.srGlobalUrl);

                string serializedObject = JsonConvert.SerializeObject(Extensions.userInstance);

                var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                var result = await client.PostAsync("api/HuntMonster", content);
                string resultContent = await result.Content.ReadAsStringAsync();

                return resultContent;
            }
        }
    }
}
