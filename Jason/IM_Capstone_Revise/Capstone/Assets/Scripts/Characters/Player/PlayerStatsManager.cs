using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : StatsManager
{
    [Header("Broacasting on")]
    [SerializeField] private VoidEventChannelSO _updateBarUI = default;

    [Header("Listening on")]
    [SerializeField] private SOEventChannelSO _statsSendingChannel = default;

    [SerializeField] private bool _canRestoreStamina = true;

    public bool CanRestoreStamina { get => _canRestoreStamina; set => _canRestoreStamina = value; }

    public bool updatedFlag = true;

    private void OnEnable()
    {
        InitializeStats();
        _statsSendingChannel.OnEventRaised += InitializeStats;
        StartCoroutine(RestoreStamina());
    }

    IEnumerator RestoreStamina()
    {

        while (true)
        {
            // �ȴ�ֱ�����Իָ�stamina  
            while (!CanRestoreStamina)
            {
                yield return null; // ��ͣЭ�̵�������ʱ�䣬����ʹ��yield return new WaitForSeconds(smallDelay)�����С�ӳ�  
            }
            updatedFlag = false;
            for (int i = 0; i < 15; i++)
            {
                // ��ͣ����  
                yield return new WaitForSeconds(0.1f);
                if (updatedFlag)
                {
                    break;
                }
            }

            // ��ʼ�ָ�stamina��ֱ��������������  
            while (CanRestoreStamina && !updatedFlag)
            {
                // ƽ������stamina���߼�  
                // ����RestoreStamina�����ᴦ��stamina�����ӣ����Ҳ��ᳬ�����ֵ  
                float amountToRestore = CalculateAmountToRestore(); // ����Ҫʵ���������������ÿ��Ҫ�ָ���stamina��  
                currentStatsSO.RestoreStamina(amountToRestore);
                _updateBarUI.RaiseEvent(); // ����UI  

                // �������ǲ�ϣ��ÿ�����Ӷ��ȴ������Ǹ������ӵ����������ȴ�ʱ�䣨����Ϊ�˼����������ֱ�Ӽ�����  
                // ���������Ҫƽ���Ķ���Ч�����������Ҫ���������һЩ�ӳ�  
                // yield return new WaitForSeconds(smallDelay); // smallDelay��һ��С���ӳ�ֵ������0.1f  

                // ����Ƿ���Ȼ���Իָ�stamina�����������������ڲ�ѭ���в��Ǳ���ģ���������ⲿ�������ܸı�������Ӧ�ñ����  
                if (!CanRestoreStamina)
                    break;

                // ע�⣺���RestoreStamina�����ڲ��Ѿ�������stamina�����ֵ���ƣ�������ļ����ܲ��Ǳ����  
                // �����δ�������Ӧ������������߼�����ֹ�������ֵ  

                // һ��С�ӳ���ģ��ƽ�����ӣ���ѡ��  
                yield return new WaitForSeconds(0.1f); // ������Ҫ�������ֵ  
            }

            // ��canRestoreStamina��Ϊ��ʱ��Э�̻�ص����ѭ���Ŀ�ʼ�����ȴ�ֱ�����ٴα�Ϊ��  
        }

    }

    protected void InitializeStats(ScriptableObject statsReceived)
    {
        StatsConfigSO stats = (StatsConfigSO)statsReceived;

        Debug.Log("set to calculated stats");

        CurrentStatsSO.SetMaxHealth(stats.InitialHealth);
        CurrentStatsSO.SetCurrentHealth(stats.InitialHealth);
        CurrentStatsSO.SetCurrentArmor(stats.InitialArmor);
        CurrentStatsSO.SetCurrentMR(stats.InitialMagicResist);
        CurrentStatsSO.SetCurrentMaxJumps(stats.InitialJumpCount);
        CurrentStatsSO.SetCurrentAttack(stats.InitialAttack);
        CurrentStatsSO.SetCurrentAP(stats.InitialAbilityPower);
        CurrentStatsSO.SetCurrentCD(stats.InitialCooldown);
        CurrentStatsSO.SetCurrentAttackSpeed(stats.InitialAttackSpeed);
        CurrentStatsSO.SetCurrentMana(stats.InitialMana);
        CurrentStatsSO.SetCurrentLuck(stats.InitialLuck);
        CurrentStatsSO.SetCurrentStamina(stats.InitialStamina);
        CurrentStatsSO.SetMaxStamina(stats.InitialStamina);
        CurrentStatsSO.SetCurrentTenacity(stats.InitialTenacity);
        CurrentStatsSO.SetCurrentCritChance(stats.InitialCritChance);
        CurrentStatsSO.SetCurrentCritDmg(stats.InitialCritDamage);
        _updateBarUI.RaiseEvent();
    }

    protected override void InitializeStats()
    {
        CurrentStatsSO.SetMaxHealth(statsConfig.InitialHealth);
        CurrentStatsSO.SetCurrentHealth(statsConfig.InitialHealth);
        CurrentStatsSO.SetCurrentArmor(statsConfig.InitialArmor);
        CurrentStatsSO.SetCurrentMR(statsConfig.InitialMagicResist);
        CurrentStatsSO.SetCurrentMaxJumps(statsConfig.InitialJumpCount);
        CurrentStatsSO.SetCurrentAttack(statsConfig.InitialAttack);
        CurrentStatsSO.SetCurrentAP(statsConfig.InitialAbilityPower);
        CurrentStatsSO.SetCurrentCD(statsConfig.InitialCooldown);
        CurrentStatsSO.SetCurrentAttackSpeed(statsConfig.InitialAttackSpeed);
        CurrentStatsSO.SetCurrentMana(statsConfig.InitialMana);
        CurrentStatsSO.SetCurrentLuck(statsConfig.InitialLuck);
        CurrentStatsSO.SetCurrentStamina(statsConfig.InitialStamina);
        CurrentStatsSO.SetMaxStamina(statsConfig.InitialStamina);
        CurrentStatsSO.SetCurrentTenacity(statsConfig.InitialTenacity);
        CurrentStatsSO.SetCurrentCritChance(statsConfig.InitialCritChance);
        CurrentStatsSO.SetCurrentCritDmg(statsConfig.InitialCritDamage);
        _updateBarUI.RaiseEvent();
    }

    private float CalculateAmountToRestore()
    {
        // ����ֻ��һ��ʾ�����������Ҫ���������Ϸ�߼�������������㷽��  
        float maxStamina = currentStatsSO.MaxStamina;
        float amount = Mathf.Max(maxStamina * 0.02f, 1f); // ÿ�����ָ�1�㣬���߻ָ������ֵ  
        return amount;
    }


    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public int GetMaxJumps()
    {
        return currentStatsSO.CurrentMaxJumps;
    }

    public void SpendStamina(float spent)
    {
        currentStatsSO.SpendStamina(spent);
        _updateBarUI.RaiseEvent();
    }

    public void ReturnStamina(int returned)
    {
        currentStatsSO.RestoreStamina(returned);
        _updateBarUI.RaiseEvent();
    }

    public float GetCurrentStamina()
    {
        return currentStatsSO.CurrentStamina;
    }

    public float GetCurrentAttackSpeed()
    {
        return currentStatsSO.CurrentAttackSpeed;
    }

    private void OnDisable()
    {
        _statsSendingChannel.OnEventRaised -= InitializeStats;
    }
}

