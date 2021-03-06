﻿#if PART2

#if ORIGINAL

using System;

public class LightRailApp
{
    private static int MINUTES_IN_DAY = 24 * 60;

    /**
     * Converts a string in the format hh:mm (of a 24-hour clock)
     * to minutes past midnight.  For example, 01:30 becomes 90.
     * @param hhmm a string in the format hh:mm such as 23:41
     * @return how many minutes past midnight have elapsed
     */
    public static int ToMinutes(string hhmm)
    {
        string[] parts = hhmm.Split(':');
        return int.Parse(parts[0]) * 60 + int.Parse(parts[1]);
    }

    /**
     * Converts a number of minutes past midnight into a String representation
     * in the format hh:mm of a 24-hour clock.  For example, 90 becomes 01:30.
     * @param minutes time past midnight
     * @return the time in format h:mm or hh:mm, such as 23:41
     */
    public static string FormatMinutes(int minutes)
    {
        int h = minutes / 60;
        int m = minutes % 60;
        return (h < 10 ? "0" : "") + h
                + ":" + (m < 10 ? "0" : "") + m;
    }


    /**
     * When the first northbound train leaves the southernmost station,
     * as minutes past midnight.
     */
    private int northFirst;

    /**
     * The latest a northbound train may leave the southernmost station.
     * This does not guarantee a train leaves then--it just means a train
     * won't leave the southernmost station if it's past this time.
     */
    private int northLast;

    /**
     * When the first southbound train leaves the northernmost station,
     * as minutes past midnight.
     */
    private int southFirst;

    /**
     * The latest a southbound train may leave the northernmost station.
     * This does not guarantee a train leaves then--it just means a train
     * won't leave the northernmost station if it's past this time.
     */
    private int southLast;

    /**
     * How long it takes (in minutes) to go between stops i and i + 1, either
     * northbound or southbound.
     */
    private int[] travelTimes;

    /**
     * The starts of time intervals that describe the frequency of trains
     * leaving from the northernmost and southernmost stations.  An interval
     * starts at the ith entry and ends 1 minute earlier than the (i + 1)th entry.
     * The starts are in increasing order.
     */
    private int[] intervalStarts;

    /**
     * How frequently (in minutes) trains leave from the northernmost and southernmost stations
     * during a time interval described by intervalStarts.  That is, between the times
     * intervalStarts[i] and intervalStarts[i + 1] - 1, trains leave every
     * intervalPeriods[i] minutes.  The last element describes the frequency until the last
     * train leaves.
     */
    private int[] intervalPeriods;

    /**
     * @param travelTimes
     *			How long it takes (in minutes) to go between stops i and i +
     *			1, either northbound or southbound.
     * @param intervalStarts
     *			The starts of time intervals that describe the frequency of
     *			trains leaving from the northernmost and southernmost
     *			stations. An interval starts at the ith entry and ends 1
     *			minute earlier than the (i + 1)th entry. The starts should
     *			be ordered chronologically.
     * @param intervalPeriods
     *			How frequently (in minutes) trains leave from the northernmost
     *			and southernmost stations during a time interval described by
     *			intervalStarts. That is, between the times intervalStarts[i]
     *			and intervalStarts[i + 1] - 1, trains leave every
     *			intervalPeriods[i] minutes. The last element describes the
     *			frequency until the last train leaves.
     * @param northFirst
     *			When the first northbound train leaves the southernmost
     *			station, as minutes past midnight.
     * @param northLast
     *			The latest a northbound train may leave the southernmost
     *			station. This does not guarantee a train leaves then--it just
     *			means a train won't leave the southernmost station if it's
     *			past this time.  Expressed as minutes past midnight.
     * @param southFirst
     *			When the first southbound train leaves the northernmost
     *			station, as minutes past midnight.
     * @param southLast
     *			The latest a southbound train may leave the northernmost
     *			station. This does not guarantee a train leaves then--it just
     *			means a train won't leave the northernmost station if it's
     *			past this time.  Expressed as minutes past midnight.
     */
    public LightRailApp(int[] travelTimes, int[] intervalStarts, int[] intervalPeriods,
            int northFirst, int northLast, int southFirst, int southLast)
    {
        this.travelTimes = travelTimes;
        this.intervalStarts = intervalStarts;
        this.intervalPeriods = intervalPeriods;
        this.northFirst = northFirst;
        this.northLast = northLast;
        this.southFirst = southFirst;
        this.southLast = northLast;
    }

    /**
     * Returns the earliest time at or after the given time when a train
     * will arrive at the stop.
     * @param leave the time at or after the train may leave.
     * @param stop which stop to leave from (0 being southernmost)
     * @param north whether the train is northbound (otherwise southbound)
     * @return the earliest time a train will leave at or after the time given.
     */
    public int NextTrain(int leave, int stop, bool north)
    {
        // how many minutes ahead this stop is from the first station
        // (the "first" station is the southernmost station if northbound,
        // and northernomst station if southbound).
        int offset = 0;

        // the earliest departure time of a train at the first station
        int first;

        // the latest possible departure time of a train at the first stop
        int last;
        if (north)
        {
            first = northFirst;
            last = northLast;
            for (int i = 0; i < stop; i++)
            {
                offset += travelTimes[i];
            }
        }
        else
        {
            first = southFirst;
            last = southLast;
            for (int i = travelTimes.Length - 1; i > stop; i--)
            {
                offset += travelTimes[i];
            }
        }

        // normalized leave time--when the rider would want to leave, if they were
        // at the first station
        int normLeave = leave - offset;

        // if outside train operating hours, just return the first train.
        if (normLeave > last || normLeave < first)
        {
            return first + offset;
        }

        // when the desired train leaves the first station
        int trainLeave = first;
        int interval = 0;
        while (trainLeave < normLeave)
        {
            trainLeave += intervalPeriods[interval];
            if (interval + 1 < intervalStarts.Length
                    && trainLeave >= intervalStarts[interval + 1])
            {
                interval++;
            }
        }
        return trainLeave + offset;
    }

    public static void Main()
    {
        // read stop count
        int stopCount = int.Parse(Console.ReadLine());
        int[] stops = new int[stopCount - 1];

        // read stops
        string[] parts = Console.ReadLine().Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            stops[i] = int.Parse(parts[i]);
        }

        // read first and latest departure times, for northbound and southbound
        parts = Console.ReadLine().Split(' ');
        int northFirst = ToMinutes(parts[0]);
        int northLast = ToMinutes(parts[1]);

        parts = Console.ReadLine().Split(' ');
        int southFirst = ToMinutes(parts[0]);
        int southLast = ToMinutes(parts[1]);

        // read intervals and periods
        int intervalCount = int.Parse(Console.ReadLine());
        int[] intervalStarts = new int[intervalCount];
        int[] intervalPeriods = new int[intervalCount];
        for (int i = 0; i < intervalCount; i++)
        {
            parts = Console.ReadLine().Split(' ');
            intervalStarts[i] = ToMinutes(parts[0]);
            intervalPeriods[i] = int.Parse(parts[1]);
        }

        // read query count
        int queryCount = int.Parse(Console.ReadLine());

        // read and process queries
        LightRailApp app = new LightRailApp(stops, intervalStarts, intervalPeriods,
                northFirst, northLast, southFirst, southLast);
        for (int i = 0; i < queryCount; i++)
        {
            parts = Console.ReadLine().Split(' ');
            Console.WriteLine(FormatMinutes(
                    app.NextTrain(ToMinutes(parts[0]), int.Parse(parts[1]), parts[2][0] == 'N')));
        }
    }
}

#endif

#endif
