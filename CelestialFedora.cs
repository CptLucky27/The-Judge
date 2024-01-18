using System;
using System.Reflection;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;

public class CelestialFedora : PassiveItem
{
    private static Hook healthHaverStartHook;

    public static void Init()
    {
        string itemName = "Celestial Fedora";
        string resourceName = "TheJudgeModule/Resources/Celestial_Fedora_sprite";

        GameObject obj = new GameObject(itemName);
        var item = obj.AddComponent<CelestialFedora>();

        ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

        string shortDesc = "Smooth Judgement";
        string longDesc = "This celestial fedora breaks the boundaries of mortal constraints, allowing you to deal unlimited damage to bosses.";

        ItemBuilder.SetupItem(item, shortDesc, longDesc, "cpt");
        item.quality = PickupObject.ItemQuality.S;
    }

    public override void Pickup(PlayerController player)
    {
        base.Pickup(player);
        EnableBossDpsCapModification();
    }

    public override DebrisObject Drop(PlayerController player)
    {
        DisableBossDpsCapModification();
        return base.Drop(player);
    }

    public override void OnDestroy()
    {
        DisableBossDpsCapModification();
        base.OnDestroy();
    }

    private static void EnableBossDpsCapModification()
    {
        if (healthHaverStartHook == null)
        {
            healthHaverStartHook = new Hook(
                typeof(HealthHaver).GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic),
                typeof(CelestialFedora).GetMethod("ModifiedHealthHaverStart")
            );
        }
    }

    private static void DisableBossDpsCapModification()
    {
        if (healthHaverStartHook != null)
        {
            healthHaverStartHook.Dispose();
            healthHaverStartHook = null;
        }
    }

    public static void ModifiedHealthHaverStart(Action<HealthHaver> orig, HealthHaver self)
    {
        orig(self);
        if (self.IsBoss)
        {
            FieldInfo m_damageCapField = typeof(HealthHaver).GetField("m_damageCap", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo m_bossDpsCapField = typeof(HealthHaver).GetField("m_bossDpsCap", BindingFlags.Instance | BindingFlags.NonPublic);

            m_damageCapField?.SetValue(self, -1f);
            m_bossDpsCapField?.SetValue(self, -1f);
        }
    }
}
