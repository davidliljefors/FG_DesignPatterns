using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreUI : MonoBehaviour, IDisposable
{
    private Text m_Text;
    private ICharacter m_Player;

    void Start()
    {
        m_Text = GetComponent<Text>();
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<ICharacter>();
        UpdateText(m_Player.Health);
        m_Player.OnHealthChanged += UpdateText;
    }

    void UpdateText(int value)
    {
        m_Text.text = "Health : "+value.ToString();
    }

    public void Dispose()
    {
        m_Player.OnHealthChanged -= UpdateText;
    }
}
