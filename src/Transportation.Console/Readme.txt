
https://app.devdraft.com/#!/workspace/93170d71-9356-4283-ac5d-e22f91a500c0

Transportation

 Debugging
 Attention to Detail

Note

In this debugging challenge, the goal is to remedy the described problem while changing as little code as possible. You're not trying to minimize the number of keystrokes (or something equally silly), but you should only change parts that are incorrect and could be contributing to the described problem.

Select your choice of language in the code editor to receive the initial (buggy) code. Since you're dealing with existing code, the language choice is limited to five of the most common OO languages.



A city has a light rail system and an app that tells customers when the next train will arrive.  The city is planning a change to the rail system and wants you to ensure that the current app can handle the change.

The light rail is laid out in a line oriented north to south and allows passengers to travel both northbound and southbound.  On the line are several stations (stops) where passengers can embark and disembark.  The map of the stops currently looks like this:

Line Map

    stop #:  0   1   2   3   4   5   6   7   8
    minutes:   4   8   3   12  8   2   2   3

Where the stops are labeled 0 - 8 and the numbers are how many minutes it takes to go between two stops and drop off and load passengers, for both northbound trains and southbound trains; for example, it takes 8 minutes to go from stop 1 to stop 2 (or stop 2 to stop 1), and 3 minutes to go from stop 8 to stop 7 (or from stop 7 to stop 8).  The table is also illustrated to the left.

A train that travels northbound leaves from stop 0 and visits each station in increasing order, up to the northernmost stop.  Likewise, a southbound train starts at the northernmost stop and visits each station in decreasing order down to station 0.  For example, a northbound train that leaves stop 0 at 05:04 would arrive at stop 3 at 05:19 and stop 8 at 05:46.

How often northbound trains leave the southernmost station and southbound trains leave the northernmost station is based on the time of day to accomodate heavy commute times.  The frequencies are detailed in the table below:

How frequently trains leave from station 0 (northbound) and station 8 (southbound)

    05:00 - 05:59: every 15 minutes
    06:00 - 08:29: every 6 minutes
    08:30 - 14:59: every 10 minutes
    15:00 - 18:29: every 6 minutes
    18:30 - 20:59: every 10 minutes
    21:00 - every 15 minutes

(The times are given with a 24 hour clock)

For example, suppose a northbound train leaves station 0 at 05:04; the next departures from station 0 would be at 05:19, 05:34, 05:49, 06:04, and 06:10.  Note that once a train leaves its starting station, its times between stops are dictated by the times given in the map of stops and not affected by the frequency table.

The trains don't run for a period at night.  The last northbound train leaves the southernmost station no later than 23:30 (that is not to say the last train leaves at 23:30--it just does not leave after 23:30) and the first northbound train departs at 05:04.  The first southbound train leaves at 06:00 and the last leaves at or before 23:00.  Once the last train leaves its station, it visits all the remaining stations like normal but does not pick up passengers at its final destination.

Your Task:

The city has an app for customers to see the earliest time when the next train departs, given a stop number, desired time to leave, and direction (northbound or southbound).  For example, a rider might want to catch a northbound train from stop 1 at or after 05:15.  The earliest the next train would arrive would be 05:23.  Note that if the rider wanted to catch a train at or after 05:23, the 05:23 train would still be recommended since it is assumed the rider can be at the stop at the given time; but if the rider gave 05:24, the 05:38 train would be recommended.  If a rider wanted to catch a train at a time during the period of night when trains do not run, then the first train of the next day in the desired direction would be recommended.

The app appears to work based on the given test case for the current schedule.  However, the city is planning to add new stops and trains in the near future, as well as have trains leaving their start stations up to the early morning, around 01:00 or 02:00--the details are not yet decided upon--and they have requested that you ensure the code is robust enough to handle the change.  While you test the code be sure to fix any bugs you find.

Input

The input should be read from standard input.  The first line contains a single integer S representing how many stops there are.  The next line contains (S - 1) space-separated integers, where the i-th integer represents the time (in minutes) it takes to travel from the i-th stop to the (i + 1)th stop, or vice-versa.

The next line has the time of day the first northbound train leaves the southernmost station and the latest time of day a northbound train may leave the southernmost station, separated by a space.  The next line has the time of day the first southbound train leaves the northernmost station and the latest time of day a southbound train may leave the northernmost station, separated by a space.

The next line contains a single integer T, the number of time spans that make up the frequency table.  T lines follow, each with a time and integer, separated by a space.  The time is the start of the time span, and the span lasts until the start of the next time span.  The integer is the number of minutes a train must wait at the first station before following a train that left inside the time span.

The next line contains an integer Q, representing the number of queries from riders that will be simulated.

The next Q lines each contain a time of day, stop number, and direction, separated by spaces.  The rider wants to catch a train at or after the given time of day; the stop number is the integer index of the stop the rider wants to catch a train from (where the southernmost stop is stop 0); and the direction is a single capital letter, 'N' for northbound or 'S' for southbound.

Output

The output should be written to standard output.  For each of the Q queries, output on a separate line the earliest time a train will depart at or after the given time, at the given stop, and in the given direction.

Unit Test for Current Schedule

Input:

9
4 8 3 12 8 2 2 3
05:04 23:30
06:00 23:55
6
05:00 15
06:00 6
08:30 10
15:00 6
18:30 10
21:00 15
11
05:55 9 S
05:00 0 N
05:04 0 N
05:05 0 N
07:09 1 N
07:17 2 N
07:20 3 N
23:10 0 N
23:47 6 N

Output:

06:00
05:04
05:04
05:19
07:14
07:22
07:25
23:19
23:56

Explanation:

The first query is an example of a southbound train leaving from the northernmost station.  The next 3 queries show what happens when the desired time to leave gets very close to when an actual train will leave.  The queries at hour 7 follow a train as it moves from stops 1 to 3 (in the output you can see how the differences between the times, :14, :22, :25 match the differences in the Line Map, 8 and 3).  The second to last query shows the final northbound train leaving at 23:19 (15 minutes later is past the latest a north train can leave from stop 0).  The final query shows that a northbound train can arrive to other stops past 23:30, though a northbound train can't leave from stop 0 past 23:30.

Unit Test for Proposed Schedule

Input:

8
4 8 3 12 8 2 3
05:04 02:00
06:00 02:50
6
05:00 15
06:00 6
08:30 10
15:00 6
21:00 15
01:00 20
3
05:00 0 N
02:05 0 N
23:47 0 N

Expected Output:

05:04
05:04
23:59

Actual Output:

05:04
05:04
05:04

Explanation:

Note that in this proposed schedule, the latest a northbound train can leave is 02:00--that means northbound trains start leaving the southernmost station at 05:04 and continue leaving all the way past midnight up to but no later than 02:00 in the morning.  The first and second query show how the first train of the next day is recommended when a rider wants to catch a train after the latest train already passed by.  The third query yields an incorrect answer--it should be 23:59, not 05:04!

What metrics are associated with this question?

This question counts towards the debugging and attention to detail metrics.

Why this question?

Because much of development work involves interacting with existing (sometimes bizarre) code, the ability to understand and refactor the code of other developers is valued by many companies.

Note

Keep in mind you will be asked in the second section what changes you made to the original code.

https://app.devdraft.com/#!/workspace/93170d71-9356-4283-ac5d-e22f91a500c0

