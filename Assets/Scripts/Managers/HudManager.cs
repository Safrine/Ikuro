using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HudManager : Manager<HudManager> {

    #region Attributs
    [Header("Chrono")]
    [SerializeField] private Text m_BestChrono;
    [SerializeField] private Text m_Chrono;

    [Header("Key")]
    [SerializeField] private Text m_Key;
    #endregion

    #region Initialisation
    protected override IEnumerator InitCoroutine()
	{
		yield break;
	}
    #endregion

    #region MAJ du HUD
    protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
        m_BestChrono.text = e.eBestChrono.ToString(GameManager.dateFormat);
        m_Chrono.text = e.eChrono.ToString(GameManager.dateFormat);
        if (e.eKey)
            m_Key.text = "1 / 1";
        else
            m_Key.text = "0 / 1";
    }
    #endregion
}
