using UnityEngine;

public static class GameTimeChange
{
    // Start is called before the first frame update
    public static void PauseGame()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    public static void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public static void ChangeGameSpeed(float speed)
    {
        Time.timeScale = speed;
    }
}
