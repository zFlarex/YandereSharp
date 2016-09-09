//Author: zFlarex <https://zflarex.pw>

using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Yandere.Simulator.Weapons;
using System.Collections.Generic;

namespace Yandere.Simulator.NPC
{
    struct NPCJson
    {
        public string Name;
        public Gender Gender;
        public Personality Personality;
    }

    class NPCManager : MonoBehaviour
    {
        private List<GameObject> _studentArray = new List<GameObject>();
        private List<GameObject> _teacherArray = new List<GameObject>();

        public List<GameObject> Students { get { return _studentArray; } }
        public List<GameObject> Teachers { get { return _teacherArray; } }

        [Header("NPC Prefabs:")]
        public GameObject _studentPrefab;
        public GameObject _teacherPrefab;

        #region Spawning NPC methods
        private void SpawnStudent(NPCJson npcJson)
        {
            if (_studentPrefab == null)
            {
                Debug.LogErrorFormat("[{0}]: {1}", "NPCManager", "The student prefab has not been set.");
                return;
            }

            GameObject studentObject = (GameObject)Instantiate(_studentPrefab, new Vector3(_studentArray.Count, 0.5f, _studentArray.Count), Quaternion.identity, gameObject.transform.GetChild(0));
            Student studentComponent = studentObject.GetComponent<Student>();

            studentObject.name = npcJson.Name;

            studentComponent.Name = npcJson.Name;
            studentComponent.Gender = npcJson.Gender;
            studentComponent.Personality = npcJson.Personality;

            _studentArray.Add(studentObject);
        }

        private void SpawnTeacher(NPCJson npcJson)
        {
            if (_teacherPrefab == null)
            {
                Debug.LogErrorFormat("[{0}]: {1}", "NPCManager", "The teacher prefab has not been set.");
                return;
            }

            GameObject teacherObject = (GameObject)Instantiate(_teacherPrefab, new Vector3(-_teacherArray.Count, 0.5f, -_teacherArray.Count), Quaternion.identity, gameObject.transform.GetChild(1));
            Teacher teacherComponent = teacherObject.GetComponent<Teacher>();

            teacherObject.name = npcJson.Name;

            teacherComponent.Name = npcJson.Name;
            teacherComponent.Gender = npcJson.Gender;
            teacherComponent.Personality = npcJson.Personality;

            _teacherArray.Add(teacherObject);
        }
        #endregion

        private void Start()
        {
            JObject JNPC = JObject.Parse(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "NPCs.json")));

            foreach (var npcSection in JNPC) //Key is the array name. Value is the array value.
            {
                Debug.LogFormat("Started loading NPC type: '{0}'.", npcSection.Key);

                foreach (var npcJson in npcSection.Value)
                {
                    switch (npcSection.Key)
                    {
                        case "Students":
                            SpawnStudent(npcJson.ToObject<NPCJson>());
                            break;

                        case "Teachers":
                            SpawnTeacher(npcJson.ToObject<NPCJson>());
                            break;

                        case "Rivals":
                            //TODO: Implement rivals.
                            break;

                        default:
                            Debug.LogErrorFormat("'{0}' has no case block to handle it!", npcSection.Key);
                            break;
                    }
                }
            }

            //Testing the weapon seen event.
            Student witnessStudent = _studentArray[0].GetComponent<Student>();
            Student victimStudent = _studentArray[1].GetComponent<Student>();
            Student weaponizedStudent = _studentArray[2].GetComponent<Student>();

            witnessStudent.RaiseWeaponSeenEvent<Knife>(new Knife()); // A witness is seeing the knife.
            victimStudent.RaiseWeaponSeenEvent<Knife>(new Knife()); // The guy who'll be dead is seeing the knife.

            //Testing the murder event.
            witnessStudent.RaiseMurderEvent<Student, Student>(victimStudent, weaponizedStudent); // The witness saw the victim get killed by the person with the knife.
        }
    }
}