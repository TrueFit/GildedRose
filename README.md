cavernous-threshold

(install node)
npm i
ng serve

dev log:

Began about 5:30 pm on 10/1/2021
Stopped at 6:40 pm on 10/1/2021 to attend PR Rams highschool football game (my daughter is in marching band.)
Resumed at about 11:00 pm on 10/1/2021
Stopped at about 1:00 am on 10/2/2021
Resumed at about 4:00 pm on 10/2/2021
Stopped at about 5:30 pm on 10/2/201 (+ 20 min to add the game room component...)

All in, less than 5 hours.

My answer to the first question: Because more "special" rules might occur, one could easily add more processXX methods to enforce those rules. While modifying "processDay" to detect those items and shunt off to the proper sub-process. I'd rather have had a more elegant discriminator method, but my original OOP approach (item interface, with a baseItem implementation and a bunch of subclasses for the specific) ended up being clunky with the state management system.

My answer to the second question: With more time, one could flesh out the unit tests of each component and write unit tests for the inventory service which could ensure that each set of special rules remain within compliance. (Or hire more QA folks...)
