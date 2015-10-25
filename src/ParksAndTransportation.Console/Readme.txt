
https://app.devdraft.com/#!/workspace/93170d71-9356-4283-ac5d-e22f91a500c0

Theme Park Planner

 Algorithmic Problem Solving
 Attention to Detail

There is a successful theme park with a multitude of attractions.  In fact, the park is a victim of its own success, as the more popular attractions can have so many people queuing in line that the queue time (how long it takes for a person to move from the end of the line to leaving the attraction) is substantial, nearing or even surpassing an hour.  This is bad for guests since they don't like waiting around and bad for business since guests aren't spending money while they're waiting in line.

There is hope for optimization.  The queue time of an attraction is not static throughout the day--attractions near a park entrance could be busy in the morning and have shorter queues later, while the big attractions in the back of the park might have shorter queues early in the morning and get punishingly long later in the day.  As a result, the order attractions are visited in makes a difference in how much time is spent queuing.

The park has given you queue time data so you can make an app that helps direct visitors to the attractions they want to visit in the least amount of time.

Input

Input should be read from standard input.  The first line contains two space-separated integers: R and Q, in that order (0 < R < 1000, 1 <= H <= 23).  R is the number of attractions in the park and H is the number of hours the park is open per day.  R lines follow, one for each attraction, where the first of the R lines corresponds to the attraction at index 0 and the last of the R lines corresponds to the attraction at index (R - 1).  The i-th line of the R lines contains H space-separated integers, such that the j-th (0 <= j < H) integer represents the queue time for the i-th attraction during the j-th hour of the park being open (where the park opens at hour 0).

The next line after the R lines contains the integer Q (0 <= Q < 100) representing the number of queries made by park guests.  3Q lines follow, meant to be read in groups of three.  The first line contains the integer T for the number of minutes the park has been open that day before the guest enters (guests enter at minute 0 if they arrive right as the park opens, and cannot enter after the park is closed).  The second of the three lines contains the integer D (0 <= D <= 20) which represents how many attractions the guest desires to visit.  The third of the three lines contains D distinct (meaning a guest does not visit the same attraction more than once) space-separated integers, each representing the index of the desired attraction to visit, where the index corresponds to the R attractions described earlier.

Output

The output should be written to standard output. For each of the Q queries, output on a separate line the least amount of time (in minutes) it could possibly take for the user to visit their desired attractions or "IMPOSSIBLE" if the park closes before the guest could possibly visit all their desired attractions.

Example

Input

5 3
30 60 75
30 15 30
30 45 60
60 45 15
99 62 99
3
0
3
0 1 2
55
1
3
119
1
4

Output

75
50
IMPOSSIBLE

Explanation:

This is a small park that is only open for three hours per day with five attractions.  We examine three guests.  The first guest arrives 0 minutes after the park opens and wants to visit 3 attractions--0, 1, and 2--as fast as possible.  For the first hour the queue times of the desired attractions are all 30 minutes, but during the second hour attraction 1 by far has the fastest line.  The guest visits attraction 0 then 2 (or 2 then 0) for a total of 60 minutes.  60 minutes is the start of the second hour, so the queue time of attraction 1 is now 15 minutes.  The guest visits attraction 1 and is finished after spending 75 minutes in the park.

The second guest only wants to visit one attraction, but arrives 55 minutes after the park's opening.  The guest could queue immediately to attraction 3 and be finished in 60 minutes, but instead waits 5 minutes.  In 5 minutes the time is 60 minutes since the park's opening and the guest queues for attraction 3 when it has a 45 minute queue time.  Afterwards the guest leaves the attraction and is satisfied after only spending 50 minutes in the park.

The third guest also only wants to visit one attraction, but arrives 119 minutes after the park's opening.  It's the last minute of hour 2 so they enqueue attraction 4 for 62 minutes.  Unfortunately the park closes right before the guest is able to visit the attraction (119 + 62 = 181 is greater than 3 * 60); in other words it's impossible for them to visit all their desired attractions.

What metrics are associated with this question?

Developing a well-optimized solution earns points towards the Advanced CS metric.

Why this question?

This question is designed to challenge you to find an optimized solution for a computationally difficult problem.  Some of the companies looking for strong backend developers are looking for this skill.

https://app.devdraft.com/#!/workspace/93170d71-9356-4283-ac5d-e22f91a500c0
