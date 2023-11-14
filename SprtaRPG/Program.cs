using System;

namespace SprtaRPG
{
    public class Character
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int AD { get; }
        public int DF { get; }
        public int HP { get; }
        public int Gold { get; }

        public Character(string name, string job, int level, int ad, int df, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            AD = ad;
            DF = df;
            HP = hp;
            Gold = gold;
        }
    }

    public class Item
    {
        public string Name { get; }
        public string Description { get; }
        public int Type { get; }
        public int AD { get; }
        public int DF { get; }
        public int HP { get; }
        public bool UseItem { get; set; }

        public static int ItemCnt = 0;

        public Item(string name, string description, int type, int ad, int df, int hp, bool useitem = false)
        {
            Name = name;
            Description = description;
            Type = type;
            AD = ad;
            DF = df;
            HP = hp;
            UseItem = useitem;
        }

        public void PrintItemStatDescription(bool withNumber = false, int idx = 0)
        {
            //장착관리

            Console.Write("- ");

            if (withNumber)
            {
                Console.Write("{0} ", idx);
            }
            if (UseItem)
            {
                Console.Write("[");
                Console.Write("E");
                Console.Write("]");
            }

            Console.Write(" | ");

            Console.Write($"{Name} - ");

            if (AD != 0) Console.Write($"AD {(AD >= 0 ? "+" : "")}{AD} ");
            if (DF != 0) Console.Write($"DF {(DF >= 0 ? "+" : "")}{DF} ");
            if (HP != 0) Console.Write($"HP {(HP >= 0 ? "+" : "")}{HP}");

            Console.Write(" | ");

            Console.WriteLine(Description);
        }

        public static int GetPrintableLength(string str)
        {
            int length = 0;
            foreach (char c in str)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    length += 2;
                }
                else
                {
                    length += 1;
                }
            }

            return length;
        }

        public static string PadRightForMixedText(string str, int totalLength)
        {
            int currentLength = GetPrintableLength(str);
            int padding = totalLength - currentLength;
            return str.PadRight(str.Length + padding);
        }

    }

    internal class Program
    {
        static Character player;
        static Item[] items;

        static void Main(string[] args)
        {
            //처음 화면
            GameDataSetting();
            PrintStartLogo();
            StartMenu();
        }

        static void GameDataSetting()
        {
            //게임 데이터 세팅
            player = new Character("르탄", "전사", 1, 10, 5, 100, 1500);
            items = new Item[10];
            AddItem(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 0, 5, 0));
            AddItem(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검입니다.", 1, 2, 0, 0));
            AddItem(new Item("활력의 투구", "체력이 좋아지는 게 느껴지는 투구입니다.", 2, 0, 5, 100));
            AddItem(new Item("판금 장화", "어느 게임에선 많이 쓰이는 신발이라고 합니다.", 3, 0, 20, 0));
        }

        static void StartMenu()
        {
            //메인화면
            Console.Clear();

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("");

            switch (CheckValidInput(1, 2))
            {
                case 1:
                    StatusMenu();
                    break;
                case 2:
                    InventoryMenu();
                    break;
            }
        }

        static int CheckValidInput(int min, int max)
        {
            // 입력 키 제한
            int keyInput;
            bool result;
            do
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                result = int.TryParse(Console.ReadLine(), out keyInput);
            } while (result == false || CheckIfValid(keyInput, min, max) == false);

            return keyInput;
        }

        static bool CheckIfValid(int checkable, int min, int max)
        {
            if (min <= checkable && checkable <= max) return true;
            return false;
        }

        static void AddItem(Item item)
        {
            if (Item.ItemCnt == 10) return;
            items[Item.ItemCnt] = item;
            Item.ItemCnt++;
        }

        static void StatusMenu()
        {
            //상태보기
            Console.Clear();

            Console.WriteLine("상태보기");
            Console.WriteLine("캐릭터의 정보를 표시합니다.");

            Console.WriteLine("Lv. {0}", player.Level.ToString("00"));
            Console.WriteLine("");
            Console.WriteLine("{0} ( {1} )", player.Name, player.Job);

            int AddAD = getSumAddAD();
            Console.WriteLine("공격력 : {0}", (player.AD + AddAD).ToString(), AddAD > 0 ? string.Format(" (+{0})", AddAD) : "");

            int AddDF = getSumAddDF();
            Console.WriteLine("방어력 : {0}", (player.DF + AddDF).ToString(), AddDF > 0 ? string.Format(" (+{0})", AddDF) : "");

            int AddHP = getSumAddHP();
            Console.WriteLine("체 력 : {0}", (player.HP + AddHP).ToString(), AddHP > 0 ? string.Format(" (+{0})", AddHP) : "");

            Console.WriteLine("Gold : {0}", player.Gold.ToString());

            Console.WriteLine("");
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("");
            switch (CheckValidInput(0, 0))
            {
                case 0:
                    StartMenu();
                    break;
            }
        }

        private static int getSumAddAD()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (items[i].UseItem) sum += items[i].AD;
            }
            return sum;
        }

        private static int getSumAddDF()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (items[i].UseItem) sum += items[i].DF;
            }
            return sum;
        }

        private static int getSumAddHP()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (items[i].UseItem) sum += items[i].HP;
            }
            return sum;
        }

        static void InventoryMenu()
        {
            Console.Clear();

            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                items[i].PrintItemStatDescription();
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("1. 장착관리");
            Console.WriteLine("");
            switch (CheckValidInput(0, 1))
            {
                case 0:
                    StartMenu();
                    break;
                case 1:
                    EquipMenu();
                    break;
            }
        }

        static void EquipMenu()
        {
            Console.Clear();

            Console.WriteLine("아이템 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("아이템 목록");
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                items[i].PrintItemStatDescription(true, i + 1);
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");

            int keyInput = CheckValidInput(0, Item.ItemCnt);

            switch (keyInput)
            {
                case 0:
                    InventoryMenu();
                    break;
                default:
                    ToggleEquipStatus(keyInput - 1);
                    EquipMenu();
                    break;
            }
        }

        static void ToggleEquipStatus(int idx)
        {
            items[idx].UseItem = !items[idx].UseItem;
        }

        static void PrintStartLogo()
        {
            Console.WriteLine("  _________                        __          __________ __________   ________  ");
            Console.WriteLine(" /   _____/______ _____  _______ _/  |_ _____  \\______   \\\\______   \\ /  _____/  ");
            Console.WriteLine(" ＼_____  \\ \\____ \\\\__  \\ \\_  __ \\\\   __\\\\__  \\  |       _/ |     ___//   \\  ___  ");
            Console.WriteLine(" /        \\|  |_> >/ __ \\_|  | \\/ |  |   / __ \\_|    |   \\ |    |    \\    \\_\\  \\ ");
            Console.WriteLine("/_______  /|   __/(____  /|__|    |__|  (____  /|____|_  / |____|     \\______  / ");
            Console.WriteLine("        \\/ |__|        \\/                    \\/        \\/                    \\/  ");
            Console.WriteLine("");
            Console.WriteLine("=============================================================================");
            Console.WriteLine("                           PRESS ANYKEY TO START                             ");
            Console.WriteLine("=============================================================================");
            Console.ReadKey();
        }
    }


}