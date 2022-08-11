using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server_Side_Application.Models;
using System.Data;

namespace Client_Server_Project
{
    //salted password hashging check videos in the channel
    public static class Extensions
    {
        public static string srGlobalUrl = "http://game.test.com/";

        public static MainWindow mainThis = null;

       public static string ComputeSha256Hash(this string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static LoggedUser userInstance = new LoggedUser();

        public async static Task<bool> refreshUserInstance()
        {
            string srResult = await getCharValues();

            if(srResult.Contains("Error") || srResult.Contains("errors"))
            {
                return false;
            }

            DataRow drwUser = JsonConvert.DeserializeObject<DataTable>(srResult).Rows[0];

            userInstance.charInfo.CharExp = drwUser["CharExp"].toInt();
            userInstance.charInfo.CharHp = drwUser["HP"].toInt();
            userInstance.charInfo.CharAttack = drwUser["Attack"].toInt();
            userInstance.charInfo.CharDefense = drwUser["Defense"].toInt();
            userInstance.charInfo.Char_Level = drwUser["Char Level"].toInt();

            mainThis.lblLoggedUser.Content = "Logged User: " + userInstance.UserId + " - " + userInstance.UserName;

            mainThis.lstBoxUserCharacter.Items.Clear();

            mainThis.lstBoxUserCharacter.Items.Add("Character Exp: " + userInstance.charInfo.CharExp.ToString("N0"));
            mainThis.lstBoxUserCharacter.Items.Add("Character HP: " + userInstance.charInfo.CharHp.ToString("N0"));
            mainThis.lstBoxUserCharacter.Items.Add("Character Attack: " + userInstance.charInfo.CharAttack.ToString("N0"));
            mainThis.lstBoxUserCharacter.Items.Add("Character Defense: " + userInstance.charInfo.CharDefense.ToString("N0"));
            mainThis.lstBoxUserCharacter.Items.Add("Character Level: " + userInstance.charInfo.Char_Level.ToString("N0"));
        

            return true;
        }

        private static async Task<string> getCharValues()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(srGlobalUrl);

                string serializedObject = JsonConvert.SerializeObject(Extensions.userInstance);

                var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                var result = await client.PostAsync("api/RefreshUserCharacter", content);
                string resultContent = await result.Content.ReadAsStringAsync();

                return resultContent;
            }
        }

        public static int toInt(this object data)
        {
            int irResult = 0;
            Int32.TryParse(data.ToString(), out irResult);
            return irResult;
        }
       
    }



}
