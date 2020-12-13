using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eScoreEvent
{
    draw,
    mine,
    mineGold,
    gameWin,
    gameLoss
}

public class ScoreManager : MonoBehaviour {
    static private ScoreManager S;

    static public int SCORE_FROM_PREV_ROUND = 0;
    static public int GAME_SCORE = 0;
    static public int GAME_COUNT = 1;

    [Header("Set Dynamically")]
    // Fields to track score info
    public int chain = -1;
    public int scoreRun = -1;
    public int score = 35;

    private void Awake()
    {
        if (S == null)
        {
            S = this; // Set the private singleton
        }
        else
        {
            Debug.LogError("ERROR: ScoreManager.Awake(): S is already set!");
        }

        // Check for a high score in PlayerPrefs

        if (PlayerPrefs.HasKey("gameCount"))
        {
            GAME_COUNT = PlayerPrefs.GetInt("gameCount");
			print("EE");
			print(GAME_COUNT);
        }
        if (PlayerPrefs.HasKey("GameScore"))
        {
            GAME_SCORE = PlayerPrefs.GetInt("GameScore");
        }
        // Add the score from last round, which will be >0 if it was a win
        score += SCORE_FROM_PREV_ROUND;
        // And reset the SCORE_FROM_PREV_ROUND
        SCORE_FROM_PREV_ROUND = 0;
    }

    static public void EVENT(eScoreEvent evt)
    {
        try
        {
            // try-catch stops an error from breaking your program
            S.Event(evt);
        }
        catch(System.NullReferenceException nre)
        {
            Debug.LogError("ScoreManager:EVENT() called while S=null.\n" + nre);
        }
    }

    private void Event(eScoreEvent evt)
    {
        switch (evt)
        {
            // Same things need to happen whether it's a draw, a win, or a lose 
            case eScoreEvent.draw: // Drawing a card
            case eScoreEvent.gameWin: // Won the round
            case eScoreEvent.gameLoss: // Lost the round
                //chain = 0; // resets the score chain
                //score += scoreRun; // add scoreRun to total score
                //scoreRun = -1; // reset scoreRun
                break;

            case eScoreEvent.mine: // Remove a mine card 
               // chain++; // increase the score chain
                //scoreRun += chain; // add score for this card to run
				score = score - 1;
                break;
        }
	
        // This second switch statement handles round wins and losses
        switch (evt)
        {
			
            case eScoreEvent.gameWin:
                // If it's a win, add the score to the next round
                // static fields are NOT reset by SceneManager.LoadScene()
				int a = PlayerPrefs.GetInt("gameCount");
				int b = PlayerPrefs.GetInt("GameScore");
				if (a < 9){
					a += 1;
					b += score;
					PlayerPrefs.SetInt("gameCount", a);
					PlayerPrefs.SetInt("GameScore", b);
					PlayerPrefs.Save();
				} else if (a == 9){
					PlayerPrefs.SetInt("gameCount", 1);
					PlayerPrefs.SetInt("GameScore", 0);
					PlayerPrefs.Save();
				};
                break;

            case eScoreEvent.gameLoss:
                // If it's a loss, check against the high score
				int c = PlayerPrefs.GetInt("gameCount");
				int d = PlayerPrefs.GetInt("GameScore");
				if (c < 9){
					c += 1;
					d += score;
					PlayerPrefs.SetInt("gameCount", c);
					PlayerPrefs.SetInt("GameScore", d);
					PlayerPrefs.Save();
				} else if (c == 9){
					PlayerPrefs.SetInt("gameCount", 1);
					PlayerPrefs.SetInt("GameScore", 0);
					PlayerPrefs.Save();
				};


           //     if (HIGH_SCORE <= score)
          //      {
           //         print("You got the high score! High score: " + score);
              //      HIGH_SCORE = score;
            //        PlayerPrefs.SetInt("ProspectorHighScore", score);
              //  }
         //       else
       //         {
      //              print("Your final score for the game was: " + score);
       //         }
                break;

            default:
                print("score: " + score + "  scoreRun:" + scoreRun + "  chain:" + chain);
                break;
        }
    }

    static public int CHAIN {  get { return S.chain; } }
    static public int SCORE {  get { return S.score; } }
    static public int SCORE_RUN {  get { return S.scoreRun; } }
}
