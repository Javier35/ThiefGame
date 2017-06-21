using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	public static Inventory _inventory;
	public Text livesCounterText;
	public Text moneyCounterText;

	int lives = 5;
	double money = 0;


	void Awake()
	{
		//loading function of the saved inventory items
	}

	void Start()
	{
		_inventory = this;
		_inventory.SetLivesCounterText (_inventory.lives);
		_inventory.SetMoneyCounterText(_inventory.money);
	}

	public static void GainALife(){
		_inventory.lives++;
		_inventory.SetLivesCounterText (_inventory.lives);
	}

	public static void LoseALife(){
		_inventory.lives--;
		_inventory.SetLivesCounterText (_inventory.lives);
	}

	public static void GainMoney(double gainedMoney){
		_inventory.money += gainedMoney;
		_inventory.SetMoneyCounterText (_inventory.money);
	}

	public static int GetLives(){
		return _inventory.lives;
	}

	public static double GetMoney(){
		return _inventory.money;
	}

	private void SetLivesCounterText(int livesNum){
//		_inventory.livesCounterText.text = "x" + livesNum;
	}

	private void SetMoneyCounterText(double moneyNum){
//		_inventory.moneyCounterText.text = ": " + moneyNum;
	}
}
