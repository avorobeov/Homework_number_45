using System;

namespace Homework_number_45
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class Warrior
    {
        public Warrior(string name, int health, int armour, int damage)
        {
            Name = name;
            Health = health;
            Armour = armour;
            Damage = damage;
        }

        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int Armour { get; protected set; }
        public virtual int Damage
        {
            get
            {
                return Damage;
            }
            protected set { }
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage - Armour;

            ShowMessage($"Получил урон - {damage}", ConsoleColor.Red);
        }

        public void ShowInfo()
        {
            ShowMessage($"{Name} HP: {Health} ARMOR : {Armour} DMG: {Damage}");
        }

        protected void ShowMessage(string text, ConsoleColor consoleColor = ConsoleColor.Blue)
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

        public override int Damage
        {
            get
            {
                if (HitsInRow == MaxHitsInRow)
                {
                    HitsInRow = 0;

                    return Damage + Damage;
                }
                else
                {
                    HitsInRow++;

                    return Damage;
                } 
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

        private bool IsDamageSuccessful(double probability)
        {
            Random random = new Random();

            double randomValue = random.NextDouble();

            if (randomValue <= probability)
            {
                return true;
            }
            else
            {
                return false;
            }
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

            ShowMessage($"Восстановил XP - {damage}", ConsoleColor.Cyan);
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
    }

    class Arena
    {
        private Warrior _firstFighter;
        private Warrior _secondFighter;

        public void AssignFighters(Warrior firstFighter, Warrior secondFighter)
        {
            _firstFighter = firstFighter;
            _secondFighter = secondFighter;
        }

        public void StartFight()
        {
            Console.WriteLine("Бой начинается \n\r\nВстречайте  бойцов");

            _firstFighter.ShowInfo();
            _secondFighter.ShowInfo();

            while (_firstFighter.Health > 0 && _secondFighter.Health > 0)
            {
                _firstFighter.TakeDamage(_secondFighter.Damage);
                _secondFighter.TakeDamage((_secondFighter.Damage));
            }

            if (_firstFighter.Health <= 0 && _secondFighter.Health <= 0)
            {
                Console.WriteLine($"\n\n К сожалению мы не смогли определить лучного бойца поединок закончился ничей");
            }
            else if(_secondFighter.Health <= 0)
            {
                ShowTheWinner(_firstFighter.Name);
            }
            else if(_firstFighter.Health <= 0)
            {
                ShowTheWinner(_secondFighter.Name);
            }
        }

        private void ShowTheWinner(string winner)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n\nБоец под именем {winner} одержал победу !!!");
            Console.ResetColor();
        }
    }
}
