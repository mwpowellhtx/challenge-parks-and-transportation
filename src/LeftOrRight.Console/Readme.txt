
https://app.devdraft.com/#!/workspace/93170d71-9356-4283-ac5d-e22f91a500c0

Left or Right?

 Code Correctness
 Attention to Detail

There is an amusement park with a multitude of attractions accessible by way of a long walkway that guests follow throughout the park.  The walkway forms a loop and does not have intersections with itself or any other major walkways.  As such, to go from one attraction to another, guests always have a choice between two paths: they may walk clockwise or counterclockwise on the circular walkway. Many park guests like to visit attractions in specific orders (perhaps because of a scheduled promotional event, or to go to specific attractions when they expect queue times to be shorter) and it is not always clear to guests which direction yields the fastest route. You are given how much time it takes to walk between adjacent attractions on the walkway (estimates based on data collected by the park) and asked to make a component of a larger app to help park visitors find the fastest way to travel between attractions.

Example

Time (in minutes) to walk between attractions:

Attraction #    0   1   2   3   4   5   0
Time (minutes)    4   2   5   1   3   8      

Above shows how long it takes to walk between 6 attractions.  According to the list, it takes 5 minutes to walk from attraction 2 to 3 (or 3 to 2), and 8 minutes to walk from attraction 0 to 5.  A guest starting at attraction 0 who wishes to visit attractions 4, 2, and 5, in that order, would walk from 0 to 4 by way of 5 for 11 minutes (as opposed to 1, 2, 3 for 14 minutes), then from 4 to 2 by way of 3 for 6 additional minutes (instead of 5, 0, 1 for 17 minutes), and finally from 2 to 5 by way of 3 and 4 for another 9 minutes (instead of 1 and 0 for 14 minutes), totalling to 26 minutes spent walking.

Input

The input should be read from standard input.  The first line contains a single integer A representing the number of attractions.  The next line contains A (0 < A <= 100) integers, where the i-th integer represents the time, in minutes, it takes for the average guest to walk from the i-th attraction to the (i + 1)-th attraction, or, in the case of the last integer, the time it takes to walk from the last attraction to the first.  A guest walking in the opposite direction takes the same amount of time to walk between the attractions.

The next line contains the integer Q (0 <= Q < 100) representing the number of queries asked by simulated users.  2Q lines follow, meant to be read in groups of 2.  In each group of two lines, the first line contains a single integer D (0 < D <= A) that represents how many attractions the user plans to visit.  The next line contains D space-separated integers, each representing the index of one of the A attractions read before, where the first attraction read in is at index 0.  The user is currently at the first of the D attractions, and will visit the remaining attractions in the order presented.

Output

The output should be written to standard output.  For each of the Q queries, output on a separate line the least number of minutes possibly spent walking on the walkway over the course of visiting the D desired attractions.

Example

Input:

6
4 2 5 1 3 8
3
2
0 1
3
0 5 3
4
0 4 2 5

Output:

4
12
26

Explanation:

This is the same park as the previous example, with the 6 attractions and same times to walk between attractions.  However, now we have 3 guests rather than just one.  The first guest visits 2 attractions by walking from attraction 0 directly to 1 for a total of 4 minutes.  The second guest visits 3 attractions by walking from attraction 0 to 5 in 8 minutes, then from 5 to 3 in 4 minutes for a total of 12 minutes spent walking.  The third guest's movements are described in the previous example.

What metrics are associated with this question?

This question counts towards the code correctness metric.

Why this question?

This problem is a simple problem that demonstrates your ability to analyze a situation and write correct code to address it, which is the first competency companies usually look for.

https://app.devdraft.com/#!/workspace/93170d71-9356-4283-ac5d-e22f91a500c0
