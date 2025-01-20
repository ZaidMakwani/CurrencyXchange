---What this app is about?
--
CurrencyXchange is an application that allows users to create a wallet and perform transactions using them.
It performs Create / Read / Update User’s Balance / Wallet.
It also has an analytical feature that allows the users to fetch the details of avg Profit and loss for the money transferred by all users 
It also add money to the user's wallet with the selected currency of the users and exchange the rate based on the amount being added to the wallet


--To run the project in an application
--
A script file has been attached which when executed will create a new database along with the schema and data.
once the script is executed configure the connection string in the appsetting.json accordingly

--assumptions made while developing the Application
--
Every user is an admin and can modify the details on the fly

--Additional Information
--
DbFirst approach have been used with entityframework core (ORM).
ApiLayer is being used for getting the real time currency exchange rates

