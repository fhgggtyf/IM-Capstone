using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueManager : MonoBehaviour
{

    [SerializeField] private GameStateSO _gameState = default;
    [SerializeField] private List<PrologueSectionSO> _sections;
    private int _index = 0;


    //[Header("Broadcasting on")]


    private void Start()
    {
        StartPrologue();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
    }

    public void NextSection() => PlaySection(_index + 1);

    void PlaySection(int index)
    {
        if (index >= _sections.Count)
        {
            //EndPrologue();
            return;
        }

        _index = index;
        //_sections[index].Play(this); // Pass manager to callback
    }


    private void StartPrologue()
    {
        _gameState.UpdateGameState(GameState.Prologue);
        PlaySection(0);
    }

}
