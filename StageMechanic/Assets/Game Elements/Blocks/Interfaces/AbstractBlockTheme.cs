using UnityEngine;
using System.Collections;

public abstract class AbstractBlockTheme : ScriptableObject
{
	public string Name;

	[Header("Content Warnings")]
	public bool Alcohol;
	public bool Drugs;
	public bool Sexuality;
	public bool IntenseSexuality;
	public bool Gambling;
	public bool MatureLanguage;
	public bool MatureHumor;

	public bool Blood;
	public bool Gore;
	public bool ComicMiscief;
	public bool CartoonViolence;
	public bool FantasyViolence;
	public bool IntenseViolence;
	public bool SexualViolence;
}
