using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour 
{
	public const string SFX_CANCEL_ITEM = "cancelItem";
	public const string SFX_FLAMINGO = "flamingo";
	public const string SFX_GATOR = "gator";
	public const string SFX_PLACE_ITEM = "placeItem";
	public const string SFX_POSSUM = "possum";
	public const string SFX_SELECT_ITEM = "selectItem";

	public static SoundManager instance;
	
	public bool isMusicEnabled = true;
	public bool isSFXEnabled = true;
	
	public void PlayMusic(string name)
	{
		PlayMusic(name, false);
	}
	
	public void PlayMusic(string name, bool forceRestart)
	{
		bool shouldStartNew = false;
		// If there is current music and either forcing restart or it is not the same as requested
		if (null != mCurrentMusic && (forceRestart || mCurrentMusic.name != name))
		{
			mCurrentMusic.Stop();
			Destroy(mCurrentMusic.gameObject);
			shouldStartNew = true;
		}
		// If there is no current music
		else if (null == mCurrentMusic)
		{
			shouldStartNew = true;
		}
		
		AudioSource musicSource = mMusicByName[name];
		// if requested music found and should start new music
		if (null != musicSource && shouldStartNew)
		{
			mCurrentMusic = Instantiate(musicSource) as AudioSource;
			DontDestroyOnLoad(mCurrentMusic);
			mCurrentMusic.name = musicSource.name;
			if (isMusicEnabled)
			{
				mCurrentMusic.Play();
			}
		}
		// If should start new but source was null
		else if (shouldStartNew)
		{
			Debug.LogError(new System.Text.StringBuilder("Could not find music source with name: ").Append(name).ToString());
		}
	}
	
	public AudioSource PlaySfx(string name)
	{
		return PlaySfx(name, false);
	}
	
	public AudioSource PlaySfx(string name, bool loop)
	{
		AudioSource currentSfx = null;
		if (isSFXEnabled)
		{
			// if no current source for that sfx exists
			if (!mCurrentSfx.ContainsKey(name))
			{
				AudioSource sfxSource = mSfxByName[name];
				if (null != sfxSource)
				{
					currentSfx = Instantiate(sfxSource) as AudioSource;
					DontDestroyOnLoad(currentSfx);
					currentSfx.name = sfxSource.name;
					
					mCurrentSfx[name] = currentSfx;
				}
				// source was null
				else
				{
					Debug.LogError(new System.Text.StringBuilder()
					               .Append("Could not find sfx source with name: ").Append(name).ToString());
				}
			}
			else
			{
				currentSfx = mCurrentSfx[name];
			}
			
			if (null != currentSfx)
			{
				currentSfx.Play();
				currentSfx.loop = loop;
			}
			else
			{
				Debug.LogError(new System.Text.StringBuilder()
				               .Append("Could not find valid current sfx with name: ").Append(name).ToString());
			}
		}
		return currentSfx;
	}
	
	public UnityEngine.Audio.AudioMixer audioMixer { get { return m_audioMixer; } }
	
	private void ClearSfx()
	{
		foreach (AudioSource currentSfx in mCurrentSfx.Values)
		{
			currentSfx.Stop();
			Destroy(currentSfx.gameObject);
		}
		mCurrentSfx.Clear();
	}
	
	private void Awake()
	{		
		DontDestroyOnLoad(gameObject);

		instance = this;
		
		mMusicByName = new Dictionary<string, AudioSource>();
		foreach (AudioSource musicSource in mMusicSources)
		{
			mMusicByName[musicSource.name] = musicSource;
		}
		
		mSfxByName = new Dictionary<string, AudioSource>();
		foreach (AudioSource sfxSource in mSfxSources)
		{
			mSfxByName[sfxSource.name] = sfxSource;
		}
		mCurrentSfx = new Dictionary<string, AudioSource>();
	}
	
	// Use this for initialization
	private void Start()
	{
		
	}
	
	// Update is called once per frame
	private void Update()
	{
		
	}
	
	private void Destroy()
	{
		
	}
	
	[SerializeField]
	private List<AudioSource> mMusicSources;
	[SerializeField]
	private List<AudioSource> mSfxSources;
	[SerializeField]
	private UnityEngine.Audio.AudioMixer m_audioMixer;
	
	private Dictionary<string, AudioSource> mMusicByName;
	private AudioSource mCurrentMusic;
	
	private Dictionary<string, AudioSource> mSfxByName;
	private Dictionary<string, AudioSource> mCurrentSfx;
}