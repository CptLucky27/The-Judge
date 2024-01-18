using BepInEx;
using ItemAPI;
using UnityEngine;

namespace TheJudgeModule
{
    [BepInPlugin("creator.etg.Judge", "The Judge", "1.0.0")]
    [BepInDependency("etgmodding.etg.mtgapi")]
    public class TheJudgeModule : BaseUnityPlugin
    {
        public const string GUID = "creator.etg.TheJudge";
        public const string NAME = "The Judge";
        public const string VERSION = "1.0.0";
        public const string TEXT_COLOR = "#00FFFF";

        public void Awake()
        {
            ItemBuilder.Init();
            CelestialFedora.Init();  // Initialize Celestial Fedora
            Log($"{NAME} v{VERSION} started successfully.", TEXT_COLOR);
        }

        public void Start()
        {
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }

        public void GMStart(GameManager gameManager)
        {
            // Initialization that needs to happen after the game manager starts
            Log($"{NAME} v{VERSION} started successfully.", TEXT_COLOR);
        }

        public static void Log(string text, string color = "#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }
    }
}
