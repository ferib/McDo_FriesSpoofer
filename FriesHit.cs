using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using Newtonsoft.Json;
using System.Threading;
using System.Linq;

namespace FriesNetworkSpoofer
{
    class FriesHit
    {
        public RestClient web { get; set; }

        private string gameid64 { get; set; }
        private string somehash64 { get; set; }
        private string userdata64 { get; set; }
        private GameSave currentGame { get; set; }
        private bool isFinished { get; set; }
        private string lastHighScore { get; set; }
        private int linesPrinted { get; set;}
        //private string gameid64 { get; set; }
        //private string gameid64 { get; set; }

        public FriesHit()
        {
            this.isFinished = true;
            this.web = new RestClient("https://mcd-games-api.lwprod.nl/");
            this.web.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148";
            this.linesPrinted = 0;
            //Dennis (default app user data)
            //id: 100000000
            //firstname: Dennis
            //lastname: mcdo
            //email: dennistest@test.com
            //mcdonaldsId: dennistest123423123123145asdfhrty3tw
            //this.gameid64 = "MGJmNDFiNzktOGI4OC00OWVmLWFmMGQtMmM3YTZjZmVlY2U3";
            //this.somehash64 = "OGJlNWZlNzgzNDhkZTU4NWM1YjM3YWQ0MDYzOTZhZTMyYjA4MzJlMjI5MTBmMDkwZWFkY2E1YTdjNzdjNTYyZjEzOTViNGU2ODY0OWVmZGE4MDg2MDhkY2UwMmM3ZjRlODgzYzE3MzhhMjVkNmEyYjM4Y2FjN2NiNGEzOGMyZDk="; //used to verify user?
            //this.userdata64 = "MTAwMDAwMDAw"; //userid i guess?
            //ferib userdata
            this.gameid64 = "MGJmNDFiNzktOGI4OC00OWVmLWFmMGQtMmM3YTZjZmVlY2U3";
            this.somehash64 = "NDZjMzhmNWFiODRlOWU4N2E3NmUyNDhkM2EyNTk3NzFmOTEwYzFkZDRmNjQyYjc4M2E0OThlMjRkOTE5MWJiMDIxZThkN2E3OWRlNzg5NzE4Mzc5NjM1ZDY4MjI0MzJmNGZmZWVhNmMwYzVhYmI1ZDc0Mjk3NWQyZjJjMzE0N2Q="; //token obtained from '/users/register'
            this.userdata64 = "MTAwMDA0NzAzNA=="; //userid i guess?
            //FYI users are devices
        }

        public void printBanner()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@" ___________      .__                ___ ___ .__  __            ___________             .__          __                 ");
            Console.WriteLine(@" \_   _____/______|__| ____   ______/   |   \|__|/  |_          \_   _____/ _____  __ __|  | _____ _/  |_  ___________  ");
            Console.WriteLine(@"  |    __) \_  __ \  |/ __ \ /  ___/    ~    \  \   __\  ______  |    __)_ /     \|  |  \  | \__  \\   __\/  _ \_  __ \ ");
            Console.WriteLine(@"  |     \   |  | \/  \  ___/ \___ \\    Y    /  ||  |   /_____/  |        \  Y Y  \  |  /  |__/ __ \|  | (  <_> )  | \/ ");
            Console.WriteLine(@"  \___  /   |__|  |__|\___  >____  >\___|_  /|__||__|           /_______  /__|_|  /____/|____(____  /__|  \____/|__|    ");
            Console.WriteLine(@"      \/                  \/     \/       \/                            \/      \/                \/                    ");
            Console.WriteLine("                                                                                    McDonalds - FriesHit Packet Emulator ");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void start()
        {
            printBanner();
            DateTime LastGamePlayed = DateTime.MinValue;
            DateTime LastCheck = DateTime.MinValue;
            while(true)
            {

                //#===========================#
                //#      ~ Old Methode ~      #
                //#===========================#


                //if(LastCheck.AddMinutes(1) < DateTime.UtcNow)
                //{
                //    LastCheck = DateTime.UtcNow;
                //    var LeaderbordList = getLeaderbordList(100);
                //    this.lastHighScore = LeaderbordList.topScoreData.Max(x => x.score);
                //    //TopScoreData topscoreUser = LeaderbordList.topScoreData.Find(x => x.score == topscore);
                //    TopScoreData topscoreUser = LeaderbordList.topScoreData.FirstOrDefault();
                //    if(topscoreUser != null && (topscoreUser.firstName != "Ferib" || topscoreUser.lastName != "H"))
                //    {
                //        Console.WriteLine($"{topscoreUser.firstName} {topscoreUser.lastName}. is #1 with {topscoreUser.score}... emulating our score to {Convert.ToInt32(topscoreUser.score)+1}");
                //        updateScore(Convert.ToInt32(topscoreUser.score+1));
                //    }else
                //    {
                //        Console.WriteLine($"We are numer one, Hey! ({topscoreUser.firstName} {topscoreUser.lastName}. {topscoreUser.score})");
                //    }
                //}
                //else
                //{
                //    Random rnd = new Random();
                //    if(rnd.Next(1,20) < 25)
                //    {
                //        decimal factor = rnd.Next(100, 900) / 1000m;
                //        Console.WriteLine($"Random Play to {(Convert.ToInt32(this.lastHighScore) * factor)}");
                //        updateScore((int)(Convert.ToInt32(this.lastHighScore) * factor)); //random play?
                //    }
                //}

                //#===========================#
                //#     ~ 1 Game a Day ~      #
                //#===========================#

                if(LastGamePlayed.Day != DateTime.UtcNow.Day && DateTime.UtcNow.Hour >= 20)
                {
                    //We haven't played today and its after 20:00:00 UTC+0
                    LastGamePlayed = DateTime.UtcNow;
                    Random rnd = new Random();
                    var sleepMin = rnd.Next(0, 31);
                    Console.WriteLine($"Sleeping for {sleepMin} minutes...");
                    Thread.Sleep(sleepMin * 1000 * 60); //delay between 0 ~ 30 minutes

                    //clean mess
                    Console.Clear();
                    printBanner();

                    //get leaderboard data
                    int setScoreTo = rnd.Next(1700, 3500); //fallback when scoreboard info fails, these score are 'above average'
                    var LeaderbordList = getLeaderbordList(10);
                    try
                    {
                        setScoreTo = (Convert.ToInt32(LeaderbordList.topScoreData[7].score) + Convert.ToInt32(LeaderbordList.topScoreData[8].score) + Convert.ToInt32(LeaderbordList.topScoreData[9].score) + 42) / 3;
                    }
                    catch { } //cba error handling this ;_;

                    //emulate score
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Emulating score to {setScoreTo}");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    updateScore(setScoreTo);
                }

                Thread.Sleep(20000);
            }
        }

        public void updateScore(int score, bool finish = true)
        {
            //1: 4
            //2: 7
            //3: 8
            //4: 9
            //5: 10
            //6: 12
            //7: 13
            //8: 14
            //9: 15
            //10: 24
            int[] ScoreByLevelIndex = { 24, 4, 6, 8, 9, 10, 12, 13, 14, 15 };
            //System.Convert.ToBase64String();
            Random rnd = new Random();
            if(!(currentGame != null && currentGame.lives > 0))
            {
                this.isFinished = false;
                currentGame = new GameSave();
                currentGame.lives = 3;
                currentGame.score = 0;
                currentGame.stage = 1;
                currentGame.stageScore = 0;
            }
            
            currentGame.startTime = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000;
            int secondsWait = 8;
            DateTime startTime = DateTime.UtcNow;
           // safeScore(currentGame, (long)(startTime.AddSeconds(secondsWait).Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000);
            bool isRunning = true;
            while (isRunning)
            {
                int addSum = ScoreByLevelIndex[(int)((currentGame.stage % 10))];
                if (currentGame.stage % 10 == 0)
                    addSum += 5;
                else
                    addSum += 2;

                if (currentGame.score + addSum > score)
                {
                    addSum = score - currentGame.score; //makes it exactly 1 higher :D
                    currentGame.score += addSum;
                    currentGame.stageScore = addSum;
                    break;
                }

                currentGame.score += addSum;
                currentGame.stageScore = addSum;
                

                //calc new end time
                if (currentGame.stage % 10 == 0)
                    secondsWait = rnd.Next(11, 25); //stage 10 is a boss, should be harder
                else
                    secondsWait = rnd.Next(3, 8); //first 9 stages are just spamming
                startTime = DateTime.UtcNow;

                //set start time
                currentGame.startTime = (long)(startTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000;

                Thread.Sleep(secondsWait * 1000);
                //Console.ReadKey();
                
                //safeScore(currentGame, (long)(startTime.AddSeconds(secondsWait).Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000);
                safeScore(currentGame, (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000);

                //increase stage by 1 and send packet
                currentGame.stage += 1;
            }
            //kill
            if(finish)
            {
                secondsWait = rnd.Next(10, 30); //we die fast 1-3sec
                currentGame.lives = -1;
                currentGame.startTime = (long)(startTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000;
                startTime = DateTime.UtcNow.AddSeconds(secondsWait / 10f);
                safeScore(currentGame, (long)(startTime.AddSeconds(1).Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000);
                this.isFinished = true;
            }
        }

        public bool safeScore(GameSave gamesave, long endtime)
        {
            string status64 = "MA=="; //aob(0)
            if (gamesave.lives <= 0)
                status64 = "MQ=="; //aob(1)
            bool isok = safeScore(System.Convert.ToBase64String(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(gamesave))),
                System.Convert.ToBase64String(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(gamesave.startTime))),
                System.Convert.ToBase64String(Encoding.ASCII.GetBytes(endtime.ToString())), status64);
#if DEBUG
            Console.WriteLine($"lives: {currentGame.lives}\nscore: {currentGame.score}\nstage: {currentGame.stage}\nstartTime: {currentGame.startTime}\nendTime: {endtime}\n{DateTime.Now.ToString("HH:mm:ss")}");
#endif
#if RELEASE
            char[] lives = {'#', '#', '#'};
            for(int i = currentGame.lives; i < 3; i++)
            {
                lives[i] = '_';
            }

            int linePrint = Console.WindowHeight;
            if (linePrint > 10)
                linePrint = 10;

            if (this.linesPrinted % linePrint == 0 || this.linesPrinted == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"[#########################]:[  score ]:[ stage ]:[ h ]:[     startTime     ]:[      endTime      ]");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            if (isok)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            string output = $"[{DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss")}][Sent]: {currentGame.score.ToString().PadLeft(8)} - {currentGame.stage.ToString().PadLeft(7)} - {new string(lives)} - {new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(currentGame.startTime).ToString("yyyy/MM/dd HH:mm:ss")} - {new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(endtime).ToString("yyyy/MM/dd HH:mm:ss")}";
            Console.WriteLine(output);
            this.linesPrinted++;
#endif
            //Console.WriteLine(gamesave.score);
            return isok;
        }

        public bool safeScore(string scoredata64, string starttime64, string endtime64, string isdead64)
        {
            //Console.WriteLine($"a: {this.gameid64}\nb: {this.somehash64}\nc: {scoredata64}\nd: {starttime64}\ne: {endtime64}\nf: {this.userdata64}\n");
            var request = new RestRequest($"games/saveScore?a={gameid64}&b={somehash64}&c={scoredata64}&d={starttime64}&e={endtime64}&f={userdata64}&g={isdead64}", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            var response = this.web.Execute(request);
#if DEBUG
            Console.WriteLine(response.Content);
#endif
            return response.StatusCode == System.Net.HttpStatusCode.OK;
            //Console.WriteLine(response.Content);
            //return true;
        }

        public Data getLeaderbordList(int limit = 10)
        {
            ///
            ///Console.WriteLine($"a: {this.gameid64}\nb: {this.somehash64}\nc: {scoredata64}\nd: {starttime64}\ne: {endtime64}\nf: {this.userdata64}\n");
            var request = new RestRequest($"games/getTopScores?gameId=0bf41b79-8b88-49ef-af0d-2c7a6cfeece7&limit={limit}", Method.GET);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            var response = this.web.Execute(request);
            Console.WriteLine(response.StatusCode);
            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != "")
                return JsonConvert.DeserializeObject<LeaderbordResponse>(response.Content).data;
            return null;
        }


    }

    public class GameSave
    {
        public int stage { get; set; }
        public int score { get; set; }
        public int lives { get; set; }
        public int stageScore { get; set; }
        public long startTime { get; set; }
    }

    public class TopScoreData
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string score { get; set; }
    }

    public class Data
    {
        public List<TopScoreData> topScoreData { get; set; }
    }

    public class LeaderbordResponse
    {
        public Data data { get; set; }
        public int success { get; set; }
    }
}
