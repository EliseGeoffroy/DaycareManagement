# DaycareManagement

This is a console application designed for managing a daycare center, offering two main services:
- Child Registration:
    *  Data is entered by the user through a form with data validation.
    *  The user can add trusted individuals to a contact list associated with the child.
    *  Depending on their age, the child is added to a group.
      _ If no places are available for their age group, the registration is canceled.
      _ If it is the last available spot, an event is sent to notify that there are no more spots for a child of the same age category.
    *  When exiting the service, if URLs for profile photos have been added, they are downloaded asynchronously.
- Generation of a Group Summary in HTML Format:
    *  The user selects the group for which they want a summary.
    *  The application generates an HTML file with the group name, the name and photo of each educator, the number of children and the group's capacity, and the name, photo, and birthday of each child.
    *  
Data persistence is achieved through JSON files (introduction of SQL databases planned for a future release).
