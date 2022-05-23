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
    public IController playerController;

    [SerializeField] private ProjectilePool projectilePoolManager;
    public ProjectilePool ProjectileManager => projectilePoolManager;

    [SerializeField] private EffectPool effectPoolManager;
    public EffectPool EffectManager => effectPoolManager;

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
