using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager
{
	public enum GameMode
	{
		Initialize,
		StageEdit,
		TextureEdit,
		Play
	}

	private static GameMode _gameMode = GameMode.Initialize;
	private static GameMode _lastGameMode = GameMode.Initialize;
	public static GameMode CurrentGameMode
	{
		get
		{
			if (_gameMode == GameMode.Initialize)
				_gameMode = GameMode.StageEdit;
			return _gameMode;
		}
		set
		{
			_lastGameMode = _gameMode;
			_gameMode = value;
		}
	}

	public static void TogglePlayMode()
	{
		if (_gameMode != GameMode.Play)
			CurrentGameMode = GameMode.Play;
		else
			CurrentGameMode = _lastGameMode;
	}

	public static int[] PlayerScores = new int[4];

	public static bool IsLiteBuild
	{
		get
		{
			return Application.platform == RuntimePlatform.WebGLPlayer || QualitySettings.activeColorSpace == ColorSpace.Gamma || MusicManager.TrackCount < 7;
		}
	}
}

