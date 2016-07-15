Justin Robb
4/29/16
Subscriber Feed Application

Similar to Twitch Alerts, which provides Twitch users with a live feed of followers, this program attempts to give YouTube users the ability to get live notifications of new Subscribers. The idea is to query YouTube data v3 API, and use the data to show live notifications on screen during a stream or event.
Due to limitations with the data that YouTube provides, there are two inherent issues with this kind of program. Firstly, YouTube does not save subscriber information for users without channels. This means that user 1 can subscribe to channel A without issue. Channel A will have one subscriber (user 1). If user 1 has an account on YouTube with no channel (for example they sign in user their Google account info and nothing else), then channel A will have one subscriber but YouTube will not have user 1’s information available. It is impossible to display a notification saying “user 1 has subscribed!” because we do not know user 1’s name, even though we know a subscription exists for channel A. Let’s say user 2 does have a channel. Once user 2 subscribes to channel A, we can immediately pull user 2’s name and display a notification. We know that channel A has two subscribers but we can only view that user 2 is a subscriber from the data provided by YouTube v3 API.
Secondly, YouTube does not provide any way to pull all of a channels new subscribers. It would be nice if there were some RSS subscriber feed to consume, or something similar to get a list of live subscriptions, but no. There isn’t. To work around this, we keep track of both the users who are subscribed when the program first starts, and any who subscribe during execution. Using this set of subscribers, we can continually query the subscribers stored on YouTube and compare the results with our local cache of users who we know have previously subscribed, and if we see a new user we can safely assume that they have just subscribed and show a notification. 

Another issue with testing is that I do not have access to a YouTube account with many subscribers. This test case is important and because the program may require a bit of memory, IO access and notification management. I cannot create this kind of test case myself, and I feel that I am missing out on a huge amount of knowledge and best practices.

As of 7/14/2016, this program was switched to tracking sponsors instead of subscribers. All of the old API code was kept and no other requirement changes were made. 


TODO:
    Make program use my account info when run in Debug mode, but prompt for account info in release mode
    Utalize refresh token so that User does no ave to manually enter access token every start
    Notification customization, like font, color, pictures, border, position ect.
    Status Bar to show events and progress
    Collapsable log
    Resizing events for main form and for notification form
    Menu and menu items
    Collpase to taskbar and minimize options
    Optimize IO (saving and loading local cached users)
    Optimize network use (communication with YouTube API)
    Save Log to file

DONE
    Improve Web Authorization endpoint UI
    Create Local cookie which saves info for persistant storage
    Make notification completely transaparent
    Notification fade out/ in
    Stagger notifications and queue them up to be shown one after another
    Connect to YouTube data v3 API and get all subscribers for channel
    Create OAuth Workflow for YouTube accounts