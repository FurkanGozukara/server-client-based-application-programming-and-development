namespace Server_Side_Application.Models
{
    public class LoggedUser
    {
        public string UserName { get; set; }    
        public int UserId { get; set; }   

        public string UserAuthCode { get; set; }

        public UserCharacterInfo charInfo = new UserCharacterInfo();
        public string srMsg { get; set; }

    }

    public class UserCharacterInfo
    {
        public int CharId    { get; set; }

        public int CharHp { get; set; }
        public int CharAttack { get; set; }
        public int CharDefense { get; set; }
        public int Char_Level { get; set; }
        public int CharExp { get; set; }


    }
}
