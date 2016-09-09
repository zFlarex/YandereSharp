//Author: zFlarex <https://zflarex.pw>

using UnityEngine;
using Yandere.Simulator.NPC.AI;
using Yandere.Simulator.NPC.Events;

namespace Yandere.Simulator.NPC
{
    enum Gender { Male, Female }
    enum Personality { None, Other }

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    abstract class NPC : MonoBehaviour
    {
        protected string _npcName;
        protected Gender _npcGender;
        protected NPCMotor _npcMotor;
        protected Personality _npcPersonality;

        public string Name { get { return _npcName; } set { _npcName = value; } }
        public Gender Gender { get { return _npcGender; } set { _npcGender = value; } }
        public NPCMotor Motor { get { return _npcMotor; } set { _npcMotor = value; } }
        public Personality Personality { get { return _npcPersonality; } set { _npcPersonality = value; } }

        public abstract void HandleMurderEvent<TVictim, TMurderer>(object sender, MurderEventArgs<TVictim, TMurderer> e);
        public abstract void HandleWeaponSeenEvent<TWeapon>(object sender, WeaponSeenEventArgs<TWeapon> e);

        public void RaiseMurderEvent<TVictim, TMurderer>(TVictim npcVictim, TMurderer npcMurderer)
        {
            MurderEventRaiser<TVictim, TMurderer> murderEventRaiser = new MurderEventRaiser<TVictim, TMurderer>();
            murderEventRaiser.OnMurderEvent += HandleMurderEvent;
            murderEventRaiser.RaiseMurderEvent(npcVictim, npcMurderer);
        }

        public void RaiseWeaponSeenEvent<TWeapon>(TWeapon weaponSeen)
        {
            WeaponSeenEventRaiser<TWeapon> weaponSeenEventRaiser = new WeaponSeenEventRaiser<TWeapon>();
            weaponSeenEventRaiser.OnWeaponSeenEvent += HandleWeaponSeenEvent;
            weaponSeenEventRaiser.RaiseMurderEvent(weaponSeen); 
        }
    }
}
