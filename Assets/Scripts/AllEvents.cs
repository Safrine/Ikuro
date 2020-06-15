using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System;

#region GameManager Events
public class GameMenuEvent : SDD.Events.Event
{
}
public class GamePlayEvent : SDD.Events.Event
{
}
public class GamePauseEvent : SDD.Events.Event
{
}
public class GameResumeEvent : SDD.Events.Event
{
}
public class GameOverEvent : SDD.Events.Event
{
}
public class GameVictoryEvent : SDD.Events.Event
{
}
public class GameStatisticsChangedEvent : SDD.Events.Event
{
    public DateTime eBestChrono { get; set; }
    public DateTime eChrono { get; set; }
    public bool eKey { get; set; }
    /*public int eBestScore { get; set; }
	public int eScore { get; set; }
	public int eNLives { get; set; }
	public int eNEnemiesLeftBeforeVictory { get; set; }*/
}
#endregion

#region MenuManager Events
public class EscapeButtonClickedEvent : SDD.Events.Event
{
}
public class PlayButtonClickedEvent : SDD.Events.Event
{
}
public class ResumeButtonClickedEvent : SDD.Events.Event
{
}
public class MainMenuButtonClickedEvent : SDD.Events.Event
{
}

public class QuitButtonClickedEvent : SDD.Events.Event
{
}

public class NextLevelButtonClickedEvent : SDD.Events.Event
{
}

public class TryAgainButtonClickedEvent : SDD.Events.Event
{
}
#endregion

#region Enemy Event
/*public class EnemyHasBeenDestroyedEvent : SDD.Events.Event
{
	public Enemy eEnemy;
}*/
#endregion

#region Player Event
public class PlayerHasBeenDetectedEvent : SDD.Events.Event
{
}

public class PlayerFindKeyEvent : SDD.Events.Event
{
}

public class PlayerFindGoldEvent : SDD.Events.Event
{

}
#endregion

#region Level Events
//public class EnemiesHaveBeenRegisteredEvent : SDD.Events.Event
//{
//	public int eCount;
//}
/*public class AllEnemiesHaveBeenDestroyedEvent : SDD.Events.Event
{
}*/
#endregion