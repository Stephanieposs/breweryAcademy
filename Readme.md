## Brewery Academy

This project is destinated to APIs communication to create an ecosystem between Yard Management System, Warehouse Management System and a SAP.

We organized it in 3 different APIs and communicate between them.

The SAP process the Invoices, verifies if the Invoice exists and after it is processed the Status changes to Inactive.

The YMS receivs es the informations from the truck, call SAP to verify if the Invoice exists and then send the information to WMS.

The WMS receives the informations from YMS and apply it in the Stock, after all processes it send to SAP to change the Invoice status.

Developers: Stephanie Possamai, Bianca Lancer Peres and Gabriel Fernandes.
