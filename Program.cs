using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace LiarsDiceEB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player p1 = new Player() { Id = 1, Name = "Player", Personality = new Personality { Player = true, Quickhand = 10, SuperCheeter = true }, Inventory = new Inventory { ExtraDice = new List<Dice>(DiceGenerator.generateT6OnlyHigh(2)) } };
            Player p2 = new Player() { Id = 2, Name = "Datorn", Personality = new Personality { Quickhand = 2 }, Inventory = new Inventory { ExtraDice = new List<Dice>(DiceGenerator.generateT6OnlyHigh(2)) } };
            Player p3 = new Player() { Id = 3, Name = "Fartorn", Personality = new Personality { Quickhand = 7 }, Inventory = new Inventory { ExtraDice = new List<Dice>(DiceGenerator.generateT6OnlyHigh(2)) } };
            Player p4 = new Player() { Id = 4, Name = "Readorn", Personality = new Personality { Suspicious = 1 } };
            Player p7 = new Player() { Id = 777, Name = "777", Personality = new Personality { SuperCheeter = true } };
            //T6
            /*       var dices1 = DiceGenerator.generateT6(4);
                   dices1.AddRange(DiceGenerator.generateT6OnlyHigh(1));
                   var dices2 = DiceGenerator.generateT6(5);
                   var dices3 = DiceGenerator.generateT6(5);
                   var dices4 = DiceGenerator.generateT6(5);
                   var dices7 = DiceGenerator.generateT6(5);*/
            //T20
            /*   var dices1 = DiceGenerator.generateNTX(5, 20);
               var dices2 = DiceGenerator.generateNTX(5, 20);
               var dices3 = DiceGenerator.generateNTX(5, 20);
               var dices4 = DiceGenerator.generateNTX(5, 20);*/

            //  List<Player> list = new List<Player>() { p1, p2, p3 };
            List<LiarsDicePlayer> list = new List<LiarsDicePlayer>();
            LiarsDicePlayer lpl1 = new LiarsDicePlayer() { Player = p1 };
            LiarsDicePlayer lpl2 = new LiarsDicePlayer() { Player = p2 };
            LiarsDicePlayer lpl3 = new LiarsDicePlayer() { Player = p3 };
            LiarsDicePlayer lpl4 = new LiarsDicePlayer() { Player = p4 };
            LiarsDicePlayer lpl7 = new LiarsDicePlayer() { Player = p7 };

            //


            list.Add(lpl1);
            list.Add(lpl2);
            //    list.Add(lpl3);
            list.Add(lpl4);
            list.Add(lpl7);



            LiarsDiceSuperGame ldsg = new LiarsDiceSuperGame() { LiarsDicePlayers = list };

            //Kinda Normal Game
            /*  ldsg.NumberOfEachDice = new List<int>() { 4, 1 };
              ldsg.ListOfDiceFacesUsed = new List<List<int>>() { new List<int>() { 1, 2, 3, 4, 5, 6 }, new List<int> { 6, 5, 4, 4, 5, 6 } };*/


            //Eternal Game with dice and coin and totaly random?
            ldsg.ListOfDiceFacesUsed = new List<List<int>>() { new List<int>() { 1, 2, 3, 4, 5, 6 }, new List<int>() { 1, 6} };
            ldsg.NumberOfEachDice = new List<int>() { 1,1 };
            ldsg.Eternal = true;
            ldsg.RandomDealDice = true;
            //  ldsg.MaxTurns = 3;

            // ldsg.generateNewDicesForPlayers();


            // LiarsDicePlayerLogic.Game(list);

            LiarsDicePlayerLogic.MasterLDGame(ldsg);




        }
    }
    public class Dice
    {
        public List<int> Values { get; set; }
        public Dice(List<int> values)
        {
            Values = values;
        }
    }
    public class LiarsDiceGame
    {
        public int Bet { get; set; }
        public List<LiarsDiceTurn> GameList { get; set; }
    }

    public class LiarsDiceSuperGame
    {
        public bool Eternal { get; set; }
        public List<List<int>> ListOfDiceFacesUsed { get; set; }
        public List<LiarsDicePlayer> LiarsDicePlayers { get; set; }
        public List<int> NumberOfEachDice { get; set; }

        public List<Dice> OrgDiceList { get; set; }
        public List<Player> OrgPlayerList { get; set; }

        public List<Dice> GameBag { get; set; }

        //GameRules
        public bool MoneyGame { get; set; }
        public int MaxTurns { get; set; }
        public int FirstTo { get; set; }

        public bool RandomDealDice { get; set; }

        public LiarsDiceSuperGame()
        {
            ListOfDiceFacesUsed = new List<List<int>>() { new List<int>() { 1, 2, 3, 4, 5, 6 } };
            NumberOfEachDice = new List<int> { 5 };
            Eternal = false;
            FirstTo = 3; //implement func
            MaxTurns = -1; //implement -1
            GameBag = new List<Dice>();
            RandomDealDice = false;

        }
        public void generateNewDicesForPlayers()
        {
            //Gamebag lösningen fördelar inte tärningar av olika ansiktetn jämnt.
            if (GameBag.Count == 0)
            {
                //Fördelar jämnt så länge ingen har fuskat
                GameBag.OrderBy(x => x.Values.ToString());

                if (ListOfDiceFacesUsed.Count == ListOfDiceFacesUsed.Count)
                {

                    //Fördelar alltid till tidigaste spelaren först?
                    foreach (var ldp in LiarsDicePlayers.Where(x => x.Player.Id != 0))
                    {
                        ldp.DiceList = new List<Dice>();
                        for (int jj = 0; jj < NumberOfEachDice.Count; jj++)
                        {
                            for (int ii = 0; ii < NumberOfEachDice[jj]; ii++)
                            {
                                Dice d = new Dice(ListOfDiceFacesUsed[jj]);
                                //    ldp.DiceList.Add(d);
                                GameBag.Add(d);
                            }
                        }

                    }
                }
                //set 1 d6 if not  dicelist and dicefaces are equal length
                else
                {
                    foreach (var ldp in LiarsDicePlayers.Where(x => x.Player.Id != 0))
                    {
                        ldp.DiceList = new List<Dice>();

                        Dice d = new Dice(new List<int> { 1, 2, 3, 4, 5, 6 });
                        // ldp.DiceList.Add(d);
                        GameBag.Add(d);

                    }
                }
            }
            else
            {

            }
            OrgDiceList = GameBag.ToList();
            //blir detta jämnt?
            if (RandomDealDice)
            {
                GameBag.OrderBy(a => Guid.NewGuid()).ToList();
            }
            else
            {
                GameBag.OrderBy(a => Guid.NewGuid()).OrderBy(x => x.Values.ToString()).ToList();
            }
            while (GameBag.Count != 0)
            {
                foreach (var ldp in LiarsDicePlayers.Where(x => x.Player.Id != 0).OrderBy(a=> Guid.NewGuid()))
                {
                    ldp.DiceList.Add(GameBag.FirstOrDefault());
                    GameBag.Remove(GameBag.FirstOrDefault());
                }
            }
           // OrgDiceList = LiarsDicePlayers.FirstOrDefault().DiceList;
           
            OrgPlayerList = LiarsDicePlayers.ConvertAll(z => z.Player).Where(z => z.Id != 0).ToList();
        }
    }

    public class LiarsDiceTurn
    {
        public Player Player { get; set; }
        public int Number { get; set; }
        public int Value { get; set; }
        public bool Bluff { get; set; }

        public LiarsDiceTurn(Player player)
        {
            Bluff = false;
            Player = player;
        }
    }

    public class LiarsDicePlayerLogic
    {
        public static void LiarsDiceTurn()
        {

        }
        public static void QuickdrawPlayer(LiarsDicePlayer p)
        {

            //TBI!!
            Console.WriteLine("Dice in play:");
            var counter = 0;
            foreach (var dice in p.DiceList)
            {
                counter++;
                Console.WriteLine(counter + ": " + String.Join(", ", dice.Values));

            }
            Console.WriteLine("Dice in inventory:");
            counter = 0;
            foreach (var dice in p.Player.Inventory.ExtraDice)
            {
                counter++;
                Console.WriteLine(counter + ": " + String.Join(", ", dice.Values));
            }
            Console.WriteLine($"Swap Dice? (Success rate {p.Player.Personality.Quickhand * 10}%) y/n?"); //add possebility of failure and getting caught
            var boolius = Console.ReadLine();
            if (boolius.ToLower() == "y")
            {
                Console.WriteLine("Swap what dice from play");
                var playD = Console.ReadLine();

                Console.WriteLine("Swap what dice from inventory");
                var inventoryD = Console.ReadLine();


                int intInv = int.TryParse(inventoryD, out intInv) ? intInv : 1;
                int intPla = int.TryParse(inventoryD, out intPla) ? intPla : 1;

                //if(intPla < ) TBI

                var SwapDice = p.DiceList[intPla - 1];
                var bagDice = p.Player.Inventory.ExtraDice[intInv - 1];
                p.DiceList.Add(bagDice);
                p.Player.Inventory.ExtraDice.Add(SwapDice);
                p.DiceList.Remove(SwapDice);              
                p.Player.Inventory.ExtraDice.Remove(bagDice);


            }
            Console.Clear();
        }
        public static LiarsDiceTurn LiarsDicePlayerTurn(LiarsDicePlayer turnPlayer, LiarsDiceGame lgd, int trueTurnCounter, int counter)
        {
            LiarsDiceTurn liarsDiceTurn = new LiarsDiceTurn(turnPlayer.Player);


            Console.WriteLine(turnPlayer.Player.Name);
            var yourtrow = String.Join(", ", turnPlayer.RollList.ConvertAll(x => x.GetFace().ToString()));
            Console.WriteLine(yourtrow);
            var b = "n";
            if (lgd.GameList.LastOrDefault().Player.Id != 0)
            {
                Console.WriteLine("Bluff? Y/N");
                b = Console.ReadLine();
            }
            if (b.ToUpper() == "Y")
            {
                liarsDiceTurn.Bluff = true;

            }
            else
            {
                bool trueBet = false;
                do
                {
                    trueBet = false;
                    Console.WriteLine("How Many?");
                    var x = Console.ReadLine();
                    liarsDiceTurn.Number = int.Parse(x);
                    Console.WriteLine("What value?");
                    var y = Console.ReadLine();
                    liarsDiceTurn.Value = int.Parse(y);
                    if (!ValidLDT(liarsDiceTurn, lgd))
                    {
                        Console.WriteLine("Must be higher than last bid");
                        trueBet = true;
                    }
                } while (trueBet);
            }
            return liarsDiceTurn;
        }
        public static void MasterLDGame(LiarsDiceSuperGame ldsg)
        {

            //GameSet
            var turn = 0;
            while ((ldsg.MaxTurns > turn && ldsg.MaxTurns != -1) || (ldsg.FirstTo > ldsg.LiarsDicePlayers.Max(x => x.Wins) && ldsg.FirstTo != -1))
            {
                turn++;
                ldsg.generateNewDicesForPlayers();

                //Single Game
                while (ldsg.LiarsDicePlayers.Where(p => p.DiceList.Count() != 0).Count() != 1)
                {
                    ldsg.LiarsDicePlayers = ldsg.LiarsDicePlayers.OrderBy(a => Guid.NewGuid()).ToList();

                    var result = Game(ldsg);
                    var loser = result[0];
                    var winner = result[1];

                    var loserDice = loser.DiceList[0];
                    if (loser.Player.Personality.Player)
                    {
                        Console.WriteLine("What dice do you give?");
                        var counter = 0;
                        foreach (var dice in loser.DiceList)
                        {
                            counter++;
                            Console.WriteLine(counter + ": " + String.Join(", ", dice.Values));
                        }
                        var choice = Console.ReadLine();
                        int intPla = int.TryParse(choice, out intPla) ? intPla : 1;
                        if (intPla > loser.DiceList.Count)
                        {
                            intPla = 1;
                        }
                        loserDice = loser.DiceList[intPla - 1];
                        Console.WriteLine("");

                    }
                    if (ldsg.Eternal)
                    {                       
                        winner.DiceList.Add(loserDice);
                    }
                    else
                    {
                        ldsg.GameBag.Add(loserDice);
                    }

                    loser.DiceList.Remove(loserDice);

                    if (loser.Player.Personality.SuperCheeter)
                    {
                        loser.Player.Personality.Suspicious++;
                    }
                    if (loser.DiceList.Count == 0)
                    {
                        // ldsg.LiarsDicePlayers.Remove(loser);
                    }
                }
                var gameWinner = ldsg.LiarsDicePlayers.Where(p => p.DiceList.Count() != 0).FirstOrDefault();
                Console.WriteLine(gameWinner.Player.Name + " Won!");
                ldsg.LiarsDicePlayers.Where(p => p.Player.Id == gameWinner.Player.Id).FirstOrDefault().Wins++;
                //NewlyAdded
                ldsg.GameBag.AddRange(gameWinner.DiceList);
                gameWinner.DiceList.Clear();
                Console.ReadLine();


                //  LiarsDicePlayerLogic.MasterLDGame(ldsg);
            }

            foreach (var p in ldsg.LiarsDicePlayers)
            {
                Console.WriteLine(p.Player.Name + ": " + p.Wins);
            }

        }


        //returns loser
        public static List<LiarsDicePlayer> Game(LiarsDiceSuperGame ldsg)
        {

            var lpl = ldsg.LiarsDicePlayers.Where(p => p.DiceList.Count != 0).ToList();

            string gameString = "";
            LiarsDiceGame ldg = new LiarsDiceGame();
            ldg.Bet = 5; //FOR NOW
            ldg.GameList = new List<LiarsDiceTurn>();
            //  List<List<Roll>> rollsList = new List<List<Roll>>();

            //Real Dices?
            foreach (var p in lpl)
            {
                var fd = DiceGenerator.RollFakeDice(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
                if (p.Player.Personality.Quickhand > fd)
                {
                    if (!p.Player.Personality.Player)
                    {
                        if (p.Player.Inventory.ExtraDice.Count != 0)
                        {
                            var SwapDice = p.DiceList.LastOrDefault();
                            p.DiceList.Remove(SwapDice);
                            var bagDice = p.Player.Inventory.ExtraDice.FirstOrDefault();
                            p.Player.Inventory.ExtraDice.Remove(bagDice);
                            p.DiceList.Add(bagDice);
                            p.Player.Inventory.ExtraDice.Add(SwapDice);
                        }
                    }
                    else
                    {
                        QuickdrawPlayer(p);
                    }
                }
                var dices = DiceGenerator.generateT6(5);
                //  var rolls = DiceGenerator.RollDices(dices);

                var rolls = DiceGenerator.RollTrueDices(p.DiceList);
                p.RollList = rolls;

                //  rollsList.Add(rolls);
            }
            Player player = new Player() { Id = 0, Name = "Example" };
            LiarsDiceTurn ldt = new LiarsDiceTurn(player) { Number = 0, Value = 0 };

            ldg.GameList.Add(ldt);
            var counter = -1;
            var trueTurnCounter = -1;
            while (ldg.GameList.Where(x => x.Bluff == true).Count() == 0)
            {
                counter++;
                trueTurnCounter++;




                if (counter >= lpl.Count)
                {
                    counter = 0;
                }
                var turnPlayer = lpl[counter];

                LiarsDiceTurn liarsDiceTurn = new LiarsDiceTurn(turnPlayer.Player);


                //Show Dice in Game



                //Player tyrn
                if (turnPlayer.Player.Personality.Player)
                {
                    if (turnPlayer.Player.Personality.SuperCheeter)
                    {
                        Console.WriteLine("In a mirror under the table you see:");
                    }

                    foreach (var p in lpl)
                    {
                        var stringus = $"{p.Player.Name}: ";

                        foreach (var d in p.RollList)
                        {
                            stringus += "T" + d.Dice.Values.Count();
                            if (turnPlayer.Player.Personality.SuperCheeter)
                            {
                                stringus += ":" + d.GetReverceFace();
                            }
                            stringus += ", ";
                        }

                        stringus = stringus.Trim(' ');
                        stringus = stringus.Trim(',');
                        Console.WriteLine(stringus);


                    }
                    Console.WriteLine("_______________");


                    liarsDiceTurn = LiarsDicePlayerLogic.LiarsDicePlayerTurn(turnPlayer, ldg, trueTurnCounter, counter);
                }
                else
                {
                    liarsDiceTurn = MasterCPU(turnPlayer, ldg, lpl, ldsg);

                }

                if (liarsDiceTurn.Bluff)
                {
                    gameString += turnPlayer.Player.Name + ": BLUFF!!";
                }
                else
                {
                    gameString += liarsDiceTurn.Player.Name + ": " + liarsDiceTurn.Number + " st " + liarsDiceTurn.Value + "\n";
                }

                Console.Clear();
                Console.WriteLine(gameString);

                ldg.GameList.Add(liarsDiceTurn);


            }
            LiarsDicePlayer loser = new LiarsDicePlayer();
            LiarsDicePlayer winner = new LiarsDicePlayer();
            var correctNum = lpl.Sum(x => x.RollList.Count(y => y.GetFace() == ldg.GameList[trueTurnCounter].Value));
            if (correctNum >= ldg.GameList[trueTurnCounter].Number)
            {
                Console.WriteLine("WRONG! There were: " + correctNum + " st " + ldg.GameList[trueTurnCounter].Value);
                loser = lpl[counter];
                if (counter > 0)
                {
                    counter--;
                }
                else
                {
                    counter = lpl.Count() - 1;
                }
                winner = lpl[counter];
            }
            else
            {
                winner = lpl[counter];
                counter--;
                if (counter < 0)
                {
                    counter = lpl.Count() - 1;
                }

                Console.WriteLine("Correct! There were: " + correctNum + " st " + ldg.GameList[trueTurnCounter].Value);
                loser = lpl[counter];
            }
            foreach (var rolls in lpl)
            {

                var yourtrow = String.Join(", ", rolls.RollList.ConvertAll(x => x.GetFace().ToString()));
                if (loser == rolls)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (winner == rolls && ldsg.Eternal)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(rolls.Player.Name + ": " + yourtrow);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.ReadLine();
            Console.Clear();
            return new List<LiarsDicePlayer>() { loser, winner };

        }
        public static LiarsDiceTurn MasterCPU(LiarsDicePlayer turnPlayer, LiarsDiceGame lgd, List<LiarsDicePlayer> activePlayers, LiarsDiceSuperGame ldsg)
        {
            if (turnPlayer.Player.Id == 4)
            {
                return LiarsDicePlayerLogic.ReadingCPU(turnPlayer, lgd, activePlayers);
            }
            else if (turnPlayer.Player.Personality.SuperCheeter && turnPlayer.Player.Personality.Suspicious < 4)
            {
                return LiarsDicePlayerLogic.SuperCheeterCPU(turnPlayer, lgd, activePlayers,ldsg);
            }
            var d = DiceGenerator.generateT6(1);
            var rd = DiceGenerator.RollTrueDices(d).First();
            if (rd.GetFace() == 1 || lgd.GameList.Count == 1)
            {
                //liarsDiceTurn = LiarsDicePlayerLogic.BadCPU(turnPlayer, lgd);
                return LiarsDicePlayerLogic.CarefullCPU(turnPlayer, lgd, activePlayers);

            }
            else if (rd.GetFace() == 2 || rd.GetFace() == 3)
            {
                return LiarsDicePlayerLogic.ChancyCPU(turnPlayer, lgd, activePlayers);
            }
            else
            {
                //liarsDiceTurn = LiarsDicePlayerLogic.BlufferCPU(turnPlayer.Player, lgd);
                return LiarsDicePlayerLogic.FateBelivingCPU(turnPlayer, lgd, activePlayers);
            }
        }

        public static LiarsDiceTurn BadCPU(LiarsDicePlayer turnPlayer, LiarsDiceGame lgd)
        {

            var valuBonus = 0;
            if (lgd.GameList.LastOrDefault().Value < turnPlayer.DiceList.Max(x => x.Values.Count()))
            {
                valuBonus = 1;
            }
            var numBonus = 1;
            if (valuBonus > 0)
            {
                var intlist = new List<int>() { 0, 0, 1 };
                numBonus = DiceGenerator.RollFakeDice(intlist);
            }

            LiarsDiceTurn ldt = new LiarsDiceTurn(turnPlayer.Player) { Number = lgd.GameList.LastOrDefault().Number + numBonus, Value = lgd.GameList.LastOrDefault().Value + valuBonus };

            return ldt;

        }





        public static LiarsDiceTurn BlufferCPU(Player turnPlayer, LiarsDiceGame lgd)
        {
            LiarsDiceTurn ldt = new LiarsDiceTurn(turnPlayer) { Bluff = true };

            return ldt;

        }

        public static LiarsDiceTurn CarefullCPU(LiarsDicePlayer turnPlayer, LiarsDiceGame lgd, List<LiarsDicePlayer> activePlayers) //CarefullCPU
        {

            LiarsDiceTurn ldt = new LiarsDiceTurn(turnPlayer.Player) { };

            var MostOwnedValueList = turnPlayer.RollList.OrderBy(z => z.GetFace()).GroupBy(z => z.GetFace()).OrderByDescending(z => z.Count());
            //    var yourtrow = String.Join(", ", turnPlayer.RollList.ConvertAll(x => x.Value.ToString()));
            //    Console.WriteLine(MostOwnedValueList.FirstOrDefault().FirstOrDefault().Value);
            //    Console.WriteLine(yourtrow);
            var otherDiceCount = activePlayers.Sum(z => z.RollList.Count()) - turnPlayer.RollList.Count();
            ldt.Value = MostOwnedValueList.FirstOrDefault().FirstOrDefault().GetFace();
            ldt.Number = MostOwnedValueList.FirstOrDefault().Count() + otherDiceCount / turnPlayer.RollList.Max(z => z.Dice.Values.Count);


            if (ValidLDT(ldt, lgd))
            {
                return ldt;
            }
            else
            {
                if (DiceGenerator.RollFakeDice(new List<int> { 0, 0, 1 }) == 0)
                    return BlufferCPU(turnPlayer.Player, lgd);
                else
                    return BadCPU(turnPlayer, lgd);
            }
            // return ldt;
        }
        public static LiarsDiceTurn ReadingCPU(LiarsDicePlayer turnPlayer, LiarsDiceGame lgd, List<LiarsDicePlayer> activePlayers)
        {
            LiarsDiceTurn ldt = new LiarsDiceTurn(turnPlayer.Player) { };
            // all named dices

            //   var valueGroup = lgd.gameList.GroupBy(z => z.Value + "-" + z.Player.Id.ToString()).Where(x=>x.Key != "0-0").ToList();
            var valueGroup = lgd.GameList.GroupBy(z => z.Value).Where(x => x.Key != 0).ToList();

            var lastList = valueGroup.ConvertAll(x => x.LastOrDefault());

            List<RollTrue> otherRolls = new List<RollTrue>();
            var otherDiceCount = activePlayers.Sum(z => z.RollList.Count()) - turnPlayer.RollList.Count();

            foreach (var said in lastList)
            {
                for (var ii = 0; ii < said.Number - otherDiceCount / turnPlayer.RollList.FirstOrDefault().Dice.Values.Count; ii++)
                {
                    List<int> tempList = new List<int> { said.Value };
                    var tempDice = new Dice(tempList);
                    var tempRoll = new RollTrue(tempDice);
                    otherRolls.Add(tempRoll);
                }

            }
            // Console.WriteLine("");
            otherRolls.AddRange(turnPlayer.RollList);

            LiarsDicePlayer localLiarsDicePlayer = new LiarsDicePlayer() { Player = new Player() { Id = turnPlayer.Player.Id, Name = turnPlayer.Player.Name }, RollList = otherRolls, DiceList = turnPlayer.DiceList };

            return CarefullCPU(localLiarsDicePlayer, lgd, activePlayers);

            //   return ldt;


        }

        public static LiarsDiceTurn FateBelivingCPU(LiarsDicePlayer turnPlayer, LiarsDiceGame ldg, List<LiarsDicePlayer> activePlayers)
        {
            LiarsDiceTurn ldt = new LiarsDiceTurn(turnPlayer.Player) { };
            var MostOwnedValueList = turnPlayer.RollList.OrderByDescending(z => z.GetFace()).GroupBy(z => z.GetFace()).OrderByDescending(z => z.Count());
            ldt.Number = MostOwnedValueList.FirstOrDefault().Count();
            ldt.Value = MostOwnedValueList.FirstOrDefault().FirstOrDefault().GetFace();

            if (ValidLDT(ldt, ldg))
            {
                return ldt;
            }
            ldt.Number = ldg.GameList.LastOrDefault().Number;
            if (ValidLDT(ldt, ldg))
            {
                return ldt;
            }
            ldt.Number++;

            return ldt;
        }

        public static LiarsDiceTurn SuperCheeterCPU(LiarsDicePlayer turnPlayer, LiarsDiceGame ldg, List<LiarsDicePlayer> activePlayers, LiarsDiceSuperGame ldsg)
        {
            LiarsDiceTurn ldt = new LiarsDiceTurn(turnPlayer.Player) { };

            var allRolls = new List<RollTrue>();
            foreach (var ap in activePlayers.Where(ap => ap.Player.Id != 0 && ap.Player.Id != turnPlayer.Player.Id))
            {
                // allRolls.AddRange(ap.RollList);
                foreach (var d in ap.RollList)
                {
                    var seenBottom = d.GetReverceFace();

                    // var guessDice = DiceGenerator.generateNTX(1, d.Dice.Values.Count).FirstOrDefault();
                  //  var probDiceList = ldsg.OrgDiceList.GroupBy(z => z.Values.ToString());
                    var probDiceList = ldsg.OrgDiceList.Where(pd=>pd.Values.Contains(seenBottom) && pd.Values.Count == d.Dice.Values.Count);

                    var probDice = probDiceList.ToList().OrderBy(a => Guid.NewGuid()).FirstOrDefault();
                   
                    var guessDice = new Dice(probDice.Values);
                    RollTrue rollTrue = new RollTrue(guessDice);
              //      rollTrue.ValueUp = seenBottom - 1;
                    rollTrue.ValueUp = rollTrue.Dice.Values.IndexOf(seenBottom);
                    var guessFace = rollTrue.GetReverceFace();

                   // rollTrue.ValueUp = guessFace - 1;
                    rollTrue.ValueUp = rollTrue.Dice.Values.IndexOf(guessFace);
                    allRolls.Add(rollTrue);

                }



            }
            allRolls.AddRange(turnPlayer.RollList);
            var tempPlayer = new LiarsDicePlayer() { Player = turnPlayer.Player, DiceList = turnPlayer.DiceList, RollList = allRolls };
            //


            var MostOwnedValueList = allRolls.OrderByDescending(z => z.GetFace()).GroupBy(z => z.GetFace()).OrderByDescending(z => z.Count());
            ldt.Number = MostOwnedValueList.FirstOrDefault().Count();
            ldt.Value = MostOwnedValueList.FirstOrDefault().FirstOrDefault().GetFace();


            //Gen number of guess value 
            var lastValue = ldg.GameList.LastOrDefault().Value;
            var trueNumberOfLastValue = allRolls.Count(x => x.GetFace() == lastValue);

            //
            if (ldg.GameList.LastOrDefault().Number > trueNumberOfLastValue && ldg.GameList.LastOrDefault().Player.Id != 0)
            {
                return BlufferCPU(tempPlayer.Player, ldg);
            }
            else if (ValidLDT(ldt, ldg))
            {
                return ldt;
            }
            ldt.Number = ldg.GameList.LastOrDefault().Number + 1;
            return ldt;

        }

        public static LiarsDiceTurn ChancyCPU(LiarsDicePlayer turnPlayer, LiarsDiceGame ldg, List<LiarsDicePlayer> activePlayers)
        {
            LiarsDiceTurn ldt = new LiarsDiceTurn(turnPlayer.Player) { };

            var MostOwnedValueList = turnPlayer.RollList.OrderByDescending(z => z.GetFace()).GroupBy(z => z.GetFace()).OrderByDescending(z => z.Count());
            Random rnd = new Random();
            var rodman = rnd.Next(MostOwnedValueList.Count());
            for (int ii = rodman; ii >= 0; ii--)
            {
                ldt.Number = rnd.Next(ii) + activePlayers.Where(c => c.Player.Id != turnPlayer.Player.Id).Sum(z => z.DiceList.Count()) / turnPlayer.DiceList.FirstOrDefault().Values.Count();
                ldt.Value = rnd.Next(ii);
                if (ValidLDT(ldt, ldg))
                {
                    return ldt;
                }

            }

            return CarefullCPU(turnPlayer, ldg, activePlayers);
        }


        public static bool ValidLDT(LiarsDiceTurn ldt, LiarsDiceGame ldg)
        {
            var boolus = false;
            if ((ldg.GameList.LastOrDefault().Number <= ldt.Number && ldg.GameList.LastOrDefault().Value < ldt.Value) || ldg.GameList.LastOrDefault().Number < ldt.Number)
            {
                boolus = true;
            }
            return boolus;
        }
    }

    public class DiceGenerator
    {
        public static List<Dice> generateT6(int numbers)
        {
            List<Dice> ld = new List<Dice>();
            for (int i = 0; i < numbers; i++)
            {
                List<int> ints = new List<int>() { 1, 2, 3, 4, 5, 6 };
                Dice dice = new Dice(ints);
                ld.Add(dice);
            }
            return ld;
        }
        public static List<Dice> generateT6OnlyHigh(int numbers)
        {
            List<Dice> ld = new List<Dice>();
            for (int i = 0; i < numbers; i++)
            {
                List<int> ints = new List<int>() { 6, 5, 4, 4, 5, 6 };
                Dice dice = new Dice(ints);
                ld.Add(dice);
            }
            return ld;
        }

        public static List<Dice> generateNTX(int numbers, int sides)
        {
            List<Dice> ld = new List<Dice>();
            for (int i = 0; i < numbers; i++)
            {
                List<int> ints = new List<int>();
                for (int j = 1; j <= sides; j++)
                {
                    ints.Add(j);
                }
                Dice dice = new Dice(ints);
                ld.Add(dice);
            }
            return ld;
        }


        public static List<RollTrue> RollTrueDices(List<Dice> dices)
        {
            Random rnd = new Random();
            List<RollTrue> rolls = new List<RollTrue>();
            foreach (Dice dice in dices)
            {
                var rodman = rnd.Next(dice.Values.Count);
                rolls.Add(new RollTrue(dice) { });
            }
            rolls = rolls.OrderBy(a => Guid.NewGuid()).ToList();
            return rolls;
        }

        public static int RollFakeDice(List<int> dices)
        {
            Random rnd = new Random();

            int roll = 0;

            var rodman = rnd.Next(dices.Count);
            roll = dices[rodman];


            return roll;
        }
    }
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Money { get; set; }
        public Personality Personality { get; set; }
        public Inventory Inventory { get; set; }
    }


    public class RollTrue //Better for cheeting, painting true picture
    {
        public Dice Dice { get; set; }
        public int ValueUp { get; set; }

        public RollTrue(Dice dice)
        {
            Dice = dice;
            Random rnd = new Random();
            ValueUp = rnd.Next(Dice.Values.Count);
        }
        public int GetFace()
        {
            return Dice.Values[ValueUp];
        }
        public int GetReverceFace()
        {

            var revDice = Dice.Values.Reverse<int>().ToList();
            return revDice[ValueUp];
        }
    }
    public class LiarsDicePlayer
    {
        public Player Player { get; set; }
        public List<Dice> DiceList { get; set; }
        public List<RollTrue> RollList { get; set; }
        public int Wins { get; set; }
    }

    public class Personality
    {
        public bool Player { get; set; }
        public bool SuperCheeter { get; set; }
        public bool Carefull { get; set; }
        public int Quickhand { get; set; }
        public int Suspicious { get; set; }
    }
    public class Inventory
    {
        public List<Dice> ExtraDice { get; set; }
    }
}