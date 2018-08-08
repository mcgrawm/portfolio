using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsetFinder : MonoBehaviour {

	private int[] array1 = {1,2,3};			// answer:2
	private int[] array2 = {1,3,4,6,7,9}; 	// answer:3

	private int[] array3 = {1,4,5,6,7,7};  	// answer:2

	// Use this for initialization
	void Start () {
		
	}
	
	private int FindSplits(int[] array) {
		int maxSplits = 1;
		// sum of all ints in array
		int sum = 0;
		for(int i=0; i < array.Length; i++) {
			sum += array[i];
		}

		// n is the number of splits we are investigtaing
		for(int n = array.Length; n > 1; n--)
		{
			// is sum evenly divisible by n?
			if(sum % n == 0) {
				// what then is the sum of each split to test for?
				int splitSum = sum/n;

				// TODO find out if we can break array into n sets eac sumnming splitSum
				bool canSplit = false;
				
				// work goes here

				/*
				I'm admittedly a bit stumped. I think I'm on the right track, but am not 100% sure.
				At this point in the code, we are trying to determine if the array of ints can be split into n subsets, each summing exactly splitSum.
				If we can find one combination of n subsets that each have this sum (from any number of array elements), we can immediately report back,
				since we're asked to find the highest number of splits, and we're already counting backward from the theorhetical maximum split count.
				But how to cycle through all possible combinations of splits? In a large set of elements, this will be many, and I'm missing the logic
				on how to do that. I feel it might be simpler, but at this point in the day I would admit I can't be personally certain.

				Option 1 : Continue noodling. This is not unacceptable practice, but a person has to watch their spin time - especially in a time crunch
				Option 2 : Google / Reddit'ing : the art of consulting the static and dynamic internets for advice, examples, or an answers. Mileage might vary.
				Option 3 : Conuslt the person(s) nearby. Colleagues often enjoy helping! Sometime they're busy - be aware, considerate, and respectful

				At my previous place of employment, lunchtimes often consistented of intereting conversations, boardgames, and lunch-and-learns.
				In that environment, I would call an ad-hoc, opt-in ProblemJam (name negotiable) over lunch, near a dry-earse surface.
				I would invite colleagues to noodle over the problem together. I would half expect some brilliant math type to squash it immediately, 
				but if not, it would get some brain juices going and might yield an asnwer or at least some progress. 
				I might choose to reward the participants with candy, or internal company tokens, as a kind of problem-solving bounty. 
				This might start an interesting office trend...

				*/ 




				// if we can split the array into n sets that each sum splitSum, then set splits to n, and break out/return
				if(canSplit) {
					maxSplits = n; 
					break;
				}
		
				// if we can't, we just fall through to next lower n, and try again...

			}
		}

		// eventually, we return the maximum number of splits
		return maxSplits;
	}

}
