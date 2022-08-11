namespace Server_Side_Application.Models
{
    public class UserBattle
    {
        public int irMonsterLevel { get; set; }
        public int irMonsterHp { get; set; }

        public int irMonsterAttack { get; set; }

        public int irMonsterDefense { get; set; }

        public List<string> lstTurnResults { get; set; }

        public int irGainedExp { get; set; } = 0;

        public bool blBattleWon { get; set; } = false;

        public int irTurnCount { get; set; }   

        public bool blBattleEnded { get; set; } = false;
    }
}
