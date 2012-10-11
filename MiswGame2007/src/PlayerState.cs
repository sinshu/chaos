using System;

namespace MiswGame2007
{
    public class PlayerState
    {
        int health;
        Player.Weapon weapon;
        int ammo;

        public PlayerState(int health, Player.Weapon weapon, int ammo)
        {
            this.health = health;
            this.weapon = weapon;
            this.ammo = ammo;
        }

        public int Health
        {
            get
            {
                return health;
            }
        }

        public Player.Weapon Weapon
        {
            get
            {
                return weapon;
            }
        }

        public int Ammo
        {
            get
            {
                return ammo;
            }
        }
    }
}
