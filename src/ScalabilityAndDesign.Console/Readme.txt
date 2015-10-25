
https://app.devdraft.com/#!/workspace/93170d71-9356-4283-ac5d-e22f91a500c0

Why am I being asked these questions?

These questions don't correspond to any metrics, but your answers to them will be presented to
companies directly for their review. 

If you complete them, these questions help us show a more complete picture of your skillset,
demonstrating to companies that you can design and scale systems in addition to possessing the
coding competencies you've demonstrated through other sections.

System Design

Let's expand on the light rail app from part 2. Imagine that your MPV (minimal valuable product)
was successful and the city wants you to cover more routes, including bus routes. Specifically,
you need to ensure that 1) your system can handle at least 1000 routes with different schedules
and 2) all passengers can access the service from any device (including mobile) and get a response
within 500 msec. Consider this to be your service-level agreement (SLA).

At a high level, describe how you would design and build such a system. Be specific in terms of
calculating expected throughput in order to meet the SLA requirements, and how you would scale the
system to meet the requirements. Think about how would you take advantage of multiple machines
when scaling the system. Specifically, you may distribute the load accross the machines in
whatever manner you prefer.

Feel free to make reasonable assumptions; the details are deliberately underspecified. You can
focus your discussion on the parts that you think are the most interesting.

Disclaimer: the sole purpose of this information is to showcase your skills to prospective
employers. DevDraft will not use this information to build an actual product.

==================================================================================================

With the rework of the spaghetti code, now Conductor seems like a logical point of contact where Patron services may perform Route Scheduling.

First, there's nothing there to my knowledge preventing Conductors from conducting scheduling business in a completely threaded manner.

Additionally, such services could easily live on one or several load-balanced machines for maximum throughput.

Perhaps designating some range of Conductors, kinds of services, etc, depending on the business goals at hand.

The thing we will want to be a little careful of is coordinaring Patron itineraries in the face of scheduling conflicts. It shouldn't happen, but anything is possible.

With speed also being key, would seriously consider implementations in performance oriented languages, such as C++.



Attach Diagram (optional)

Feel free to provide a URL to a hand drawn diagram that you used for System Design challenge. We
will show cases it to the employers along with the solution. Please verify that the URL is
accessible.

==================================================================================================

N/A: I would go with Visio, or any number of readily available UML diagraming tools. Visio is typically available via corporate Office plans.




Feedback

We strive to make taking these challenges as enjoyable as possible for developers.  Please let us
know how you enjoyed your DevDraft challenge experience, what you liked, and anything you would
like to see improved.  Thank you for participating.

==================================================================================================

Zero-indexing seems to be a common occurrence; should know that by now. Nevertheless, I appreciate your availability for Q&A throughout the challenge.



Answer Submitted

Your answer has been successfully submitted on Sun Oct 25 2015 09:20:21 GMT-0400 (Eastern Standard Time). Please make sure to submit answers to all parts of the challenge that you complete in order to receive full scorecard.

Your answer has been successfully submitted!
Challenge: https://app.devdraft.com/workspace/93170d71-9356-4283-ac5d-e22f91a500c0
Part: 3
Confirmation number: 9f6cf73a-3f7c-4ddb-9d63-6e5bfb7cdb4d


https://app.devdraft.com/#!/workspace/93170d71-9356-4283-ac5d-e22f91a500c0
