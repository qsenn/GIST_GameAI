using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public static int Money;
	public int startMoney = 400;
	public static int Tower1 = 0;
	public static int Tower2 = 0;
	public static int Tower3 = 0;
	public static int Tower1_up = 0;
	public static int Tower2_up = 0;
	public static int Tower3_up = 0;
	public static int TotalBudget;


	public static int Lives;
	public static int startLives = 20;

	public static int Rounds;

	void Start ()
	{
		Money = startMoney;
		Lives = startLives;
		TotalBudget = Money;

		Rounds = 0;
	}

	void Update(){
		TotalBudget = Money + Tower1 * 100 + Tower2 * 250 + Tower3 * 350 + Tower1_up * 160 + Tower2_up * 400 + Tower3_up * 600;
	}

}
