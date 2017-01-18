﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Navigation : MonoBehaviour {
	public GameObject login;
	public GameObject register1;
	public GameObject register2;
	public GameObject gameSelection;
	public GameObject navigation;
	public GameObject profile;
	public GameObject statistics;
	public GameObject help;

	public GameObject backButton;

	public Image statisticsImg;
	public Image profileImg;
	public Image helpImg;

	private Sprite statisticsActive;
	private Sprite statisticsInactive;
	private Sprite profileActive;
	private Sprite profileInactive;
	private Sprite helpActive;
	private Sprite helpInactive;

	private readonly string PATH = "Buttons/";

	void Start () {
		statisticsActive = Resources.Load<Sprite> (PATH + "stats_active");
		statisticsInactive = Resources.Load<Sprite> (PATH + "stats");
		profileActive = Resources.Load<Sprite> (PATH + "profile_active");
		profileInactive = Resources.Load<Sprite> (PATH + "profile");
		helpActive = Resources.Load<Sprite> (PATH + "questionmark_active");
		helpInactive = Resources.Load<Sprite> (PATH + "questionmark");

		StartLogin ();
		//StartProfile ();
	}

	private void StartLogin () {
		HideAll ();
		ShowLogin ();
	}

	private void StartProfile () {
		HideAll ();
		ShowProfile ();
	}

	public void HideAll () {
		login.SetActive (false);
		register1.SetActive (false);
		register2.SetActive (false);
		gameSelection.SetActive (false);
		navigation.SetActive (false);
		profile.SetActive (false);
		statistics.SetActive (false);
		help.SetActive (false);
		backButton.SetActive (false);
	}

	/// <summary>
	/// Activates the Login GameObject and deactivates other content
	/// </summary>
	public void ShowLogin () {
		HideAll ();
		login.SetActive (true);
	}

	/// <summary>
	/// Activates the Register GameObject and deactives other content
	/// </summary>
	public void ShowRegister1 () {
		HideAll ();
		register1.SetActive (true);
	}

	/// <summary>
	/// Activates the Register2 GameObject and deactivates other content
	/// </summary>
	public void ShowRegister2() {
		HideAll ();
		register2.SetActive (true);
	}

	public void ShowProfile() {
		HideAll ();
		profile.SetActive (true);
		navigation.SetActive (true);
		backButton.SetActive (true);
		SetProfileActive ();
	}

	public void ShowStatistics() {
		HideAll ();
		statistics.SetActive (true);
		navigation.SetActive (true);
		backButton.SetActive (true);
		SetStatisticsActive ();
	}

	public void ShowHelp () {
		HideAll ();
		help.SetActive (true);
		navigation.SetActive (true);
		backButton.SetActive (true);
		SetHelpActive ();
	}

	public void ShowGameSelection () {
		HideAll ();
		gameSelection.SetActive (true);
		navigation.SetActive (true);
		SetAllActive (false);
	}

	private void SetAllActive (bool active) {
		if (active) {
			statisticsImg.sprite = statisticsActive;
			profileImg.sprite = profileActive;
			helpImg.sprite = helpActive;
		} else {
			statisticsImg.sprite = statisticsInactive;
			profileImg.sprite = profileInactive;
			helpImg.sprite = helpInactive;
		}
	}

	private void SetStatisticsActive () {
		SetAllActive (false);
		statisticsImg.sprite = statisticsActive;
	}

	private void SetProfileActive () {
		SetAllActive (false);
		profileImg.sprite = profileActive;
	}

	private void SetHelpActive () {
		SetAllActive (false);
		helpImg.sprite = helpActive;
	}

	public void LoadBalloonGame() {
		SceneManager.LoadScene("Balloon_Menu");
	}

	public void Logout() {
		Logger.Instance.EndSession ();
		Debug.Log ("Destroy in logout()");
		ShowLogin ();
	}
}
