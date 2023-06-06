using System;
using System.Collections.Generic;

namespace Homework_number_45
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string CommandSelectFighters = "1";
            const string CommandStartFight = "2";
            const string CommandExit = "3";

            List<Warrior> warriors = new List<Warrior> { new Warrior("Warrior", 100, 10, 30),
                                                         new Knight("Knight", 80, 15, 25),
                                                         new Barbarian("Barbarian", 110, 5, 35),
                                                         new Magician("Magician", 90, 5, 45),
                                                         new Oracle("Oracle", 95, 10, 30) };

            Arena arena = new Arena(warriors);

            bool isExit = false;
            string userInput;

            while (isExit == false)
            {
                Console.WriteLine($"\n\nДля того что бы выбрать бойца нажмите: {CommandSelectFighters}\n" +
                                  $"Для того что бы начать поединок нажмите:{CommandStartFight}\n" +
                                  $"Для того что бы выйти нажмите: {CommandExit}\n");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandSelectFighters:
                        arena.SelectFighters();
                        break;

                    case CommandStartFight:
                        arena.StartFight();
                        break;

                    case CommandExit:
                        isExit = true;
                        break;

                    default:
                        Console.WriteLine("Такой комады нет в списке команд!");
                        break;
                }

                Console.WriteLine("Для продолжения ведите любую клавишу...");
                Console.ReadKey();
                Console.Clear();
            }
    }
    }

    class Warrior
    {
        protected int Damage;
        protected int Armour;

        public Warrior(string name, int health, int armour, int damage)
        {
            Name = name;
            Health = health;
            Armour = armour;
            Damage = damage;
        }

        public string Name { get; protected set; }
        public int Health { get; protected set; }
       
        public virtual void Attack(Warrior warrior)
        {
            warrior.TakeDamage(Damage);
        }

        public virtual void TakeDamage(int damage)
        {
            if (damage > 0 && damage > Armour)
            {
                Health -= damage - Armour;
            }
        }

        public virtual Warrior Clone()
        {
            return new Warrior(Name, Health, Armour, Damage);
        }

        public void ShowStats()
        {
            ShowMessage($"{Name} HP: {Health} ARMOR : {Armour} DMG: {Damage}");
        }

        private void ShowMessage(string text, ConsoleColor consoleColor = ConsoleColor.Blue)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }

    class Knight : Warrior
    {
        protected int HitsInRow;
        protected int MaxHitsInRow = 3;

        public Knight(string name, int health, int armour, int damage) : base(name, health, armour, damage)
        {
        }

        public override Warrior Clone()
        {
            return new Knight(Name, Health, Armour, Damage);
        }

        public override void Attack(Warrior warrior)
        {
            if (HitsInRow == MaxHitsInRow)
            {
                HitsInRow = 0;

                warrior.TakeDamage(Damage + Damage);
            }
            else
            {
                HitsInRow++;

                warrior.TakeDamage(Damage);
            }
        }
    }

    class Barbarian : Warrior
    {
        private double _probabilisticBlockingDamage = 0.3;

        public Barbarian(string name, int health, int armour, int damage) : base(name, health, armour, damage)
        {
        }

        public override void TakeDamage(int damage)
        {
            if (IsDamageSuccessful(_probabilisticBlockingDamage) == true)
            {
                base.TakeDamage(damage);
            }
        }

        public override Warrior Clone()
        {
            return new Barbarian(Name, Health, Armour, Damage);
        }

        private bool IsDamageSuccessful(double probability)
        {
            Random random = new Random();

            double randomValue = random.NextDouble();

            return randomValue >= probability;
        }
    }

    class Magician : Warrior
    {
        private int _quantitySelfHeal = 7;

        public Magician(string name, int health, int armour, int damage) : base(name, health, armour, damage)
        {
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            Health += _quantitySelfHeal;
        }

        public override Warrior Clone()
        {
            return new Magician(Name, Health, Armour, Damage);
        }
    }

    class Oracle : Knight
    {
        public Oracle(string name, int health, int armour, int damage) : base(name, health, armour, damage)
        {
        }

        public override void TakeDamage(int damage)
        {
            if (HitsInRow == MaxHitsInRow)
            {
                HitsInRow = 0;
            }
            else
            {
                HitsInRow++;

                base.TakeDamage(damage);
            }
        }

        public override Warrior Clone()
        {
            return new Oracle(Name, Health, Armour, Damage);
        }
    }

    class Arena
    {
        private List<Warrior> _warriors;
        private Warrior _firstFighter;
        private Warrior _secondFighter;
        private bool _isReadyBattle = false;

        public Arena(List<Warrior> warriors)
        {
            _warriors = warriors;
        }

        public void SelectFighters()
        {
            Console.WriteLine("\nДля выбора бойца укажите его номер\n\n");

            for (int i = 0; i < _warriors.Count; i++)
            {
                Console.Write($"{i}) ");
                _warriors[i].ShowStats();
            }

            _firstFighter = _warriors[GetNumber()].Clone();
            _secondFighter = _warriors[GetNumber()].Clone();

            _isReadyBattle = true;
        }

        public void StartFight()
        {
            if (_isReadyBattle == true)
            {
                Console.WriteLine("Бой начинается \n\r\nВстречайте  бойцов");

                _firstFighter.ShowStats();
                _secondFighter.ShowStats();

                while (_firstFighter.Health > 0 && _secondFighter.Health > 0)
                {
                    _firstFighter.Attack(_secondFighter);
                    _secondFighter.Attack(_firstFighter);

                    _firstFighter.ShowStats();
                    _secondFighter.ShowStats();
                }

                _isReadyBattle = false;

                if (_firstFighter.Health <= 0 && _secondFighter.Health <= 0)
                {
                    Console.WriteLine($"\n\n К сожалению мы не смогли определить лучного бойца поединок закончился ничей");
                }
                else if (_secondFighter.Health <= 0)
                {
                    ShowWinner(_firstFighter.Name);
                }
                else if (_firstFighter.Health <= 0)
                {
                    ShowWinner(_secondFighter.Name);
                }
            }
            else
            {
                Console.WriteLine("Вы не выбрали бойцов для поединка");
            }
        }

        private int GetNumber()
        {
            bool isNumber = false;
            string userInput;
            int number = 0;

            while (isNumber == false)
            {
                Console.Write("\nУкажите номер первого бойца: ");
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out number) && number >= 0 && number < _warriors.Count)
                {
                    isNumber = true;
                }
                else
                {
                    Console.WriteLine("Такого бойца нет на арене!");
                }
            }

            return number;
        }

        private void ShowWinner(string winner)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n\nБоец под именем {winner} одержал победу !!!");
            Console.ResetColor();
        }
    }
}
