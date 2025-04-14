using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Delegation : MonoBehaviour
{
    private Func<int, int, int> _mathFunction;
    private Predicate<int> _isDeadCheck;
    private Action debugLogOutput;

    [SerializeField] private int _currentHealth = 0;

    delegate void ShootWeaponDelegate();

    private ShootWeaponDelegate _shootPrimary;
    private ShootWeaponDelegate _shootSecondary;

    private Action _shootWeapon;
    
    void Start()
    {
        debugLogOutput = () => Debug.Log("Hello");
        debugLogOutput += () => Debug.Log("How are you today?");
        debugLogOutput += () => Debug.Log("I'm fine thank you very much");
        debugLogOutput();

        _shootPrimary = ShootHitscan;
    }
    
    

    void ShootHitscan()
    {
        Debug.Log("Shooting hitscan");
    }

    void FunctionDelegate()
    {
        int randomSelection = Random.Range(0, 2);

        _mathFunction = randomSelection == 0 ? Addition : Subtraction;
        int result = _mathFunction(20, 10);
        Debug.Log("Result of math: " + result);
    }

    void PredicateDelegateAndLambda()
    {
        _isDeadCheck = currentHealth => currentHealth <= 0;
        Debug.Log("Character is dead: " + _isDeadCheck(_currentHealth));
    }

    int Addition(int a, int b)
    {
        return a + b;
    }

    int Subtraction(int a, int b)
    {
        return a - b;
    }
}