using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] GameDatabase gameDB;
    public GameDatabase GameDB => gameDB;

    public Board board;
    public ObjectController playerController;

    [SerializeField] private EffectPool effectPoolManager;
    public EffectPool EffectManager => effectPoolManager;

    [SerializeField] private ControllerManager controllerManager;
    public ControllerManager ControllerManager => controllerManager;

    [SerializeField] private EquipAttributeManager equipAttributeManager;
    public EquipAttributeManager EquipAttributeManager => equipAttributeManager;

    [SerializeField] private GameObject hudPanel;
    public GameObject HudPanel => hudPanel;


    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
