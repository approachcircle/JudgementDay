using System;
using System.Collections.Generic;
using System.Linq;

namespace JudgementDay;

public static class DecisionManager
{
	public static readonly Dictionary<string, int> DecisionWeights = new()
	{
		{"Saved Earth from total annihilation", 50},
		{"Created world peace, and ended hunger and poverty", 48},
		{"Saved a child from a burning building", 40},
		{"Committed to celibacy, used no foul language and loved thy neighbour", 30},
		{"Fed a stray dog", 9},
		{"Donated a small sum to charity", 10},
		{"Donated masses of money to charity", 32}, // must cancel out stealing from charity
		{"Paid for their friend's coffee", 3},
		{"Always helped with house chores", 8},
		{"Helped an elderly woman across the street", 7},
		{"Returned somebody's dropped wallet", 10},
		{"Held the door open for somebody", 2},
		{"Took friend for dinner at expensive restaurant", 13},
		{"Stopped a bank robbery", 35},
		{"Ended world peace", -50},
		{"Destroyed crucial technological advancements", -44},
		{"Manufactured weapons of mass destruction", -45},
		{"Talked loudly in an enclosed and quiet public space", -3},
		{"Fed a stray dog chocolate and grapes explicitly", -15},
		{"Accidentally broke their neighbour's window", -5},
		{"Stole masses of money from a charity", -32},
		{"Abandoned their pet in public", -25},
		{"Falsely advertised", -12},
		{"Kicked a large rock at a parked car", -14},
		{"Scammed thousands of people using cryptocurrency", -39}
	};

	public static readonly int[] DecisionRarities =
	[
		100, // save total annihilation
		95, // create world peace
		90, // save child in a burning building
		80, // celibacy
		10, // stray dog feed
		8, // charity
		19, // charity large
		5, // friend coffee
		4, // house chores
		12, // woman cross street
		11, // wallet
		4, // door open
		18, // restaurant
		78, // bank robbery
		100, // end world peace
		90, // tech advance destroy
		80, // WMD
		5, // loud in public
		60, // stray dog feed bad
		10, // window broke
		70, // stole charity
		60, // abandon pet
		30, // false advert
		30, // rock car
		40 // crypto scam
	];

	public static Decision GetRandomDecision()
	{
		List<Decision> decisionCandidates = [];
		do
		{
			Random random = new Random();
			for (int i = 0; i < DecisionRarities.Length; i++)
			{
				if (random.Next(0, DecisionRarities[i]) == DecisionRarities[i] / 2)
				{
					decisionCandidates.Add(new Decision
					{
						DecisionDescription = DecisionWeights.Keys.ElementAt(i),
						DecisionRarity = DecisionRarities[i],
						DecisionWeight = DecisionWeights.Values.ElementAt(i)
					});
				}
			}
		} while (decisionCandidates.Count == 0);
		
		return decisionCandidates.MaxBy(decision => decision.DecisionRarity);
	}
}

