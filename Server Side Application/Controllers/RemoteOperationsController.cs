using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server_Side_Application.Models;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server_Side_Application.Controllers
{
    [Route("api/RemoteOperations")]
    [ApiController]
    public class RemoteOperationsController : ControllerBase
    {
        // GET: api/<RegisterController>
        [HttpGet]
        [Route("/api/TestRegister")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<RegisterController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RegisterController>
        [Route("/api/DoRegister")]
        [HttpPost]
        public string DoRegister(RegistrationModel myModel)
        {
            if (!ModelState.IsValid)
            {
                return "Error: Incorrect registration values are provided!";
            }

            DataTable dtUsercheck = DbConnection.cmd_SelectQuery("select 1 from Users where Username=@Username", new List<string> { "@Username" }, new List<object> { myModel.UserName });

            if (dtUsercheck.Rows.Count > 0)
            {
                return "Error: This user already exists!";
            }

            bool blResult = DbConnection.cmd_UpdateDeleteQuery("insert into Users (UserName,UserPassword) values (@UserName,@UserPassword)", new List<string> { "@UserName", "@UserPassword" }, new List<object> { myModel.UserName, myModel.Password });

            if (!blResult)
            {
                return "Error: An unknown error has occured please try again!";
            }

            DataTable dtUser = DbConnection.cmd_SelectQuery("select UserId from Users where Username=@Username", new List<string> { "@Username" }, new List<object> { myModel.UserName });

            if (dtUser.Rows.Count == 0)
            {
                return "Error: An unknown error has occured please try again!";
            }

            var vrUserId = dtUser.Rows[0]["UserId"].ToString();

            bool blResult2 = DbConnection.cmd_UpdateDeleteQuery("insert into UserCharacters (UserId) values (@UserId)", new List<string> { "@UserId" }, new List<object> { vrUserId });


            if (blResult2)
            {
                return "Success: You have registered successfully";
            }
            else
            {
                DbConnection.db_Update_Delete_Query("delete from Users where UserId=" + vrUserId);
                return "Error: An unknown error has occured please try again!";
            }

        }

        // POST api/<RegisterController>
        [Route("/api/DoLogin")]
        [HttpPost]
        public string DoLogin(RegistrationModel myModel)
        {
            if (!ModelState.IsValid)
            {
                return "Error: Incorrect login values are provided!";
            }

            DataTable dtUsercheck = DbConnection.cmd_SelectQuery("select UserId,Username from Users where Username=@Username and UserPassword=@UserPassword", new List<string> { "@Username", "UserPassword" }, new List<object> { myModel.UserName, myModel.Password });

            if (dtUsercheck.Rows.Count == 0)
            {
                return "Error: You have entered incorrect username or password!";
            }

            string srAuthCode = Extensions.GetUniqueKey(64);

            var vrUserId = dtUsercheck.Rows[0]["UserId"].ToString();

            DbConnection.db_Update_Delete_Query("delete from UserAuths where UserId=" + vrUserId);

            DbConnection.db_Update_Delete_Query($"insert into UserAuths (AuthCode,UserId) values ('{srAuthCode}',{vrUserId})");

            LoggedUser _loggedUser = new LoggedUser();
            _loggedUser.UserAuthCode = srAuthCode;
            _loggedUser.UserName = dtUsercheck.Rows[0]["Username"].ToString();
            _loggedUser.UserId = Convert.ToInt32(vrUserId);
            _loggedUser.srMsg = "Success: You have successfully loggedin";
            return JsonConvert.SerializeObject(_loggedUser);
        }

        // POST api/<RegisterController>
        [Route("/api/RefreshUserCharacter")]
        [HttpPost]
        public string RefreshUserCharacter(LoggedUser _loggedUser)
        {
            if (!ModelState.IsValid)
            {
                return "Error: Incorrect login values are provided!";
            }

            DataTable dtUserChar = DbConnection.cmd_SelectQuery("select * from UserCharacters where UserId = (select UserId from UserAuths where AuthCode=@AuthCode)", new List<string> { "@AuthCode" },
                new List<object> { _loggedUser.UserAuthCode });

            if (dtUserChar.Rows.Count == 0)
            {
                return "Error: An unknown error has occured please try again!";
            }

            return JsonConvert.SerializeObject(dtUserChar);
        }

        // POST api/<RegisterController>
        [Route("/api/HuntMonster")]
        [HttpPost]
        public string HuntMonster(LoggedUser _loggedUser)
        {
            if (!ModelState.IsValid)
            {
                return "Error: Incorrect login values are provided!";
            }

            DataTable dtUserChar = DbConnection.cmd_SelectQuery("select * from UserCharacters where UserId = (select UserId from UserAuths where AuthCode=@AuthCode)", new List<string> { "@AuthCode" },
                new List<object> { _loggedUser.UserAuthCode });

            if (dtUserChar.Rows.Count == 0)
            {
                return "Error: An unknown error has occured please try again!";
            }

            DataRow drwUser = dtUserChar.Rows[0];

            LoggedUser userInstance = new LoggedUser();
            userInstance.UserId= drwUser["UserId"].toInt();
            userInstance.charInfo.CharExp = drwUser["CharExp"].toInt();
            userInstance.charInfo.CharHp = drwUser["HP"].toInt();
            userInstance.charInfo.CharAttack = drwUser["Attack"].toInt();
            userInstance.charInfo.CharDefense = drwUser["Defense"].toInt();
            userInstance.charInfo.Char_Level = drwUser["Char Level"].toInt();
            userInstance.charInfo.CharId = drwUser["CharacterId"].toInt();

            UserBattle userBattle = new UserBattle();

            Random random = new Random();
            userBattle.irMonsterLevel = random.Next(1, userInstance.charInfo.Char_Level * 5);
            setMonsterStats(userBattle);

            userBattle.lstTurnResults.Add($"Battle is starting with level: {userBattle.irMonsterLevel} and {userBattle.irMonsterHp} HP Monster");

            while (true)
            {
                int irEnemyDmg = 0;
                int irUserDmg = 0;
                bool blUserAttackFirst = true;

                if (userBattle.irMonsterLevel > userInstance.charInfo.Char_Level)
                    blUserAttackFirst = false;


                irEnemyDmg = userBattle.irMonsterAttack - userInstance.charInfo.CharDefense;
                if (irEnemyDmg < 1)
                    irEnemyDmg = 1;

                irUserDmg = userInstance.charInfo.CharAttack - userBattle.irMonsterDefense;
                if (irUserDmg < 1)
                    irUserDmg = 1;

                if (blUserAttackFirst)
                {
                    userBattle.irMonsterHp -= irUserDmg;
                    checkBattleEnded(userBattle, userInstance);
                    if (userBattle.blBattleEnded == false)
                    {
                        userInstance.charInfo.CharHp -= irEnemyDmg;
                    }

                    userBattle.lstTurnResults.Add($"Turn: {userBattle.irTurnCount} - User attacked first and inflicted: {irUserDmg.ToString("0")} damage. Enemy Monster health reduced to : {userBattle.irMonsterHp}");

                    userBattle.lstTurnResults.Add($"Turn: {userBattle.irTurnCount} - Enemy attacked last and inflicted: {irEnemyDmg.ToString("0")} damage. User health reduced to : {userInstance.charInfo.CharHp}");
                }
                else
                {
                    userInstance.charInfo.CharHp -= irEnemyDmg;
                    checkBattleEnded(userBattle, userInstance);
                    if (userBattle.blBattleEnded == false)
                    {
                        userBattle.irMonsterHp -= irUserDmg;
                    }

                    userBattle.lstTurnResults.Add($"Turn: {userBattle.irTurnCount} - Enemy attacked first and inflicted: {irEnemyDmg.ToString("0")} damage. User health reduced to : {userInstance.charInfo.CharHp}");
                    userBattle.lstTurnResults.Add($"Turn: {userBattle.irTurnCount} - User attacked last and inflicted: {irUserDmg.ToString("0")} damage. Enemy Monster health reduced to : {userBattle.irMonsterHp}");
                }

                checkBattleEnded(userBattle, userInstance);

                if (userBattle.blBattleEnded)
                {
                    break;
                }

                userBattle.irTurnCount++;
            }

            userInstance.charInfo.CharExp += userBattle.irGainedExp;
            int irNewCharLevel = userInstance.charInfo.CharExp / 10;

            if (irNewCharLevel > userInstance.charInfo.Char_Level)
            {
                userInstance.charInfo.Char_Level = irNewCharLevel;
                userInstance.charInfo.CharHp = irNewCharLevel * 50;
                userInstance.charInfo.CharAttack = irNewCharLevel * 10;
                userInstance.charInfo.CharDefense = irNewCharLevel * 10;
                userBattle.lstTurnResults.Add($"Your character level has been raised to {userInstance.charInfo.Char_Level.ToString("N0")}");
            }

            DbConnection.db_Update_Delete_Query($"  update UserCharacters set CharExp={userInstance.charInfo.CharExp}, [Char Level]={userInstance.charInfo.Char_Level},Attack={userInstance.charInfo.CharAttack},Defense={userInstance.charInfo.CharDefense},HP={userInstance.charInfo.CharHp} where CharacterId={userInstance.charInfo.CharId}");

            DbConnection.db_Update_Delete_Query($"  insert into UserBattleLogs (UserId,MonsterLevel,UserGainedExp,TurnCount) values ({userInstance.UserId},{userBattle.irMonsterLevel},{userBattle.irGainedExp},{userBattle.irTurnCount})");

            return JsonConvert.SerializeObject(userBattle);
        }

        private void checkBattleEnded(UserBattle userBattle, LoggedUser userInstance)
        {
            if (userBattle.blBattleEnded)
                return;

            if (userBattle.irMonsterHp <= 0)
            {
                userBattle.lstTurnResults.Add($"Your character has defeated the enemy Monster in {userBattle.irTurnCount.ToString("N0")}");
                userBattle.irGainedExp = userBattle.irMonsterLevel * 7;

                userBattle.lstTurnResults.Add($"Your character has gained {userBattle.irGainedExp.ToString("0")} EXP");
                userBattle.blBattleEnded = true;
                userBattle.blBattleWon = true;
            }

            if (userInstance.charInfo.CharHp <= 0)
            {
                userBattle.lstTurnResults.Add($"Your character has been defeated in turn {userBattle.irTurnCount}");
                userBattle.blBattleEnded = true;
            }
        }

        private void setMonsterStats(UserBattle userBattle)
        {
            userBattle.irMonsterAttack = userBattle.irMonsterLevel * 2;
            userBattle.irMonsterHp = userBattle.irMonsterLevel * 3;
            userBattle.irMonsterMaxHP = userBattle.irMonsterHp;
            userBattle.irMonsterDefense = userBattle.irMonsterLevel * 1;
        }
    }
}
