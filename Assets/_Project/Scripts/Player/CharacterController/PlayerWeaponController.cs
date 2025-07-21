using Entity;
using EventBus;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Weapon;
public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] protected InputReader inputReader;
    [SerializeField] protected List<WeaponBase> weapons = new();
    [SerializeField] protected List<Transform> ammoDisplays = new();
    [SerializeField] protected TextMeshProUGUI healthText;
    protected List<System.Action<float>> ammoValueChangedHandlers = new();
    private void Awake()
    {
        if (!EventBus<OnHealthUpdated>.AddActions(transform.GetInstanceID(), UpdateHealth))
        {
            Debug.LogError($"{this} unable to add action to OnHealthUpdated EventBus. Adding new binding.");
            EventBus<OnHealthUpdated>.AddBinding(transform.GetInstanceID());
            EventBus<OnHealthUpdated>.AddActions(transform.GetInstanceID(), UpdateHealth);
        }
    }
    private void OnEnable()
    {
        if (!EventBus<OnHealthUpdated>.AddActions(transform.GetInstanceID(), UpdateHealth))
        {
            Debug.LogError($"{this} unable to add action to OnHealthUpdated EventBus. Adding new binding.");
            EventBus<OnHealthUpdated>.AddBinding(transform.GetInstanceID());
            EventBus<OnHealthUpdated>.AddActions(transform.GetInstanceID(), UpdateHealth);
        }
        ConnectEvents();
        ResetWeapons();

    }
    void ConnectEvents()
    {
        inputReader.Weapon += OnWeapon;
        for (int i = 0; i < weapons.Count; i++)
        {
            int index = i;
            System.Action<float> handler = (value) =>
            {
                ammoDisplays[index].localScale = new Vector3(value,
                    ammoDisplays[index].localScale.y,
                    ammoDisplays[index].localScale.z);
            };
            weapons[i].OnAmmoValueChanged += handler;
            ammoValueChangedHandlers.Add(handler);
        }
    }
    void UpdateHealth(OnHealthUpdated @event)
    {
        healthText.text = @event.EntityBase.CurrentHealth.ToString() + " HP";
    }
    void DisconnectEvents()
    {
        inputReader.Weapon -= OnWeapon;
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].OnAmmoValueChanged -= ammoValueChangedHandlers[i];
        }
    }
    private void OnDisable()
    {
        DisconnectEvents();
    }
    protected void OnDestroy()
    {
        OnDisable();
    }
    public void ResetWeapons()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].ResetWeapon();
        }
    }
    protected void OnWeapon(InputAction.CallbackContext context, int weapon)
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        if (weapon < 0 || weapon >= weapons.Count)
        {
            Debug.LogError($"Attempting to use nonexistant weapon {weapon}.");
            return;
        }
        if (context.started) return;
        if (context.canceled && context.interaction is TapInteraction) return;
        if (context.performed)
        {
            if (context.interaction is TapInteraction)
            {
                //a tap
                //Debug.Log("Tap");
                weapons[weapon].AltFire();
            }
            else if (context.interaction is HoldInteraction)
            {
                //holding fire button
                //Debug.Log("Hold");
                weapons[weapon].StartFiring();
            }
            return;
        }
        //Debug.Log("Stop");
        weapons[weapon].StopFiring();
        //let go of the fire button
    }
}
