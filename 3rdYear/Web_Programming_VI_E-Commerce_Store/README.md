## Installation

### Initial Setup
<ul>
  <li>Download the scripts in this github repository (Database.sql & Identity.sql)</li>
  <li>Run the scripts in your editor of choice (SSMS or Azure Data Studio, etc... using your desired name.</li>
  <li>Clone the repository for the project using your editor of choice (Rider, Visual Studio 2022, etc...).</li>
  <li>In rsH60Customer and rsH60Store, npm install the node packages.</li>
</ul>

### Database Setup
<ul>
  <li>Edit the appsettings.json file connection string to use your DB in the rsomers_H60Services project (this is where all the data is being sent and received...) </li>
  <img width="995" alt="image" src="https://github.com/user-attachments/assets/60c8e971-b395-4523-8a92-a6762313a445">
  <li>Still in the Services project, edit the ServicesDBContext.cs Model in the Models folder specifically the connection string again to use your db.</li>
  <img width="1287" alt="image" src="https://github.com/user-attachments/assets/cb32a70d-78f9-4303-a2da-63e45ff864bd">
</ul>

### Identity Setup
<ul>
  <li>In rsH60Store project, in appsettings.json edit the "rsH60StoreContextConnection" to use your own Identity DB you created while running the Identity.sql script.</li>
  <img width="1069" alt="image" src="https://github.com/user-attachments/assets/e0141c26-f5f0-4dac-a4a4-1dc5d8966d9e">
  <li>In rsH60Customer project, in appsettings.json edit the "rsH60StoreContextConnection" to use your own Identity DB you created while running the Identity.sql script.</li>
  <img width="1061" alt="image" src="https://github.com/user-attachments/assets/b6c68ba5-2d46-4d7e-843c-07ebf6b1621f">
</ul>

###
FOR MANAGER PROJECT (REACT SPA)
1. Open the manager folder that's located in the rsH60A03 folder using the IDE of your choice (Mine is WebStorm):
<img width="420" alt="Screenshot 2024-12-11 at 10 35 26 PM" src="https://github.com/user-attachments/assets/977cff07-f244-4ba1-b8de-028d09bd56dd" />

2. Make sure you're in the terminal and "cd" into the "my-manager-app" directory:
<img width="1451" alt="Screenshot 2024-12-11 at 10 38 44 PM" src="https://github.com/user-attachments/assets/d2086cdc-bdd6-4b95-9e2d-78ec105bf009" />
3. (IMPORTANT) "npm install" the node modules packages:
<img width="1458" alt="Screenshot 2024-12-11 at 10 39 23 PM" src="https://github.com/user-attachments/assets/26b10b57-1386-46dc-a61c-1a582b06612d" />
4. To run the app, simply write in the terminal:
<img width="1440" alt="Screenshot 2024-12-11 at 10 40 30 PM" src="https://github.com/user-attachments/assets/b00aedc1-b310-43b0-86c4-6409833f7b9f" />
5. Look at this amazing react app!
<img width="1483" alt="Screenshot 2024-12-11 at 10 41 59 PM" src="https://github.com/user-attachments/assets/2a872bae-3156-407d-964c-e093c08531be" />

### Adding Customers
#### You can add customers by:
<ul>
  <li>As a manager in the store project, go to the customers tab and create a customer like that.</li>
  <img width="1496" alt="Screenshot 2024-12-11 at 10 27 31 PM" src="https://github.com/user-attachments/assets/9092a754-adb3-45d2-b935-d2db9bb6b409" />
  <li>As a customer in the customer project, go to the register tab and create yourself an account to shop!</li>
  <img width="1428" alt="Screenshot 2024-12-11 at 10 26 49 PM" src="https://github.com/user-attachments/assets/c885a2df-5670-4d86-ac78-74c9e09804cc" />
</ul>

### (REFERENCE) SOAP Services:
#### My ways of using the Tax and Credit Card services from CSDEV:
<ul>
  <li>Creating a new customer in the customers project. "1234123412341235" is considered a bad credit card #.</li>
  <img width="1286" alt="Screenshot 2024-12-11 at 10 29 33 PM" src="https://github.com/user-attachments/assets/9b1745d6-8bb3-4ce0-98c8-350da37b40f4" />
  <li>Creating a new customer in the store project. "1234123412341235" is considered a bad credit card #.</li>
  <img width="780" alt="Screenshot 2024-12-11 at 10 31 14 PM" src="https://github.com/user-attachments/assets/d1383500-460e-453f-baaf-a76c5b0fea3d" />
  <li>Taxes are calculated at checkout:</li>
  <img width="1273" alt="Screenshot 2024-12-11 at 10 32 28 PM" src="https://github.com/user-attachments/assets/74373d51-35ff-4482-908f-923152d65e06" />

</ul>




### Data
<p>In the script for the store Database, you have 12 products loaded for you with custom images.</p>
<p>In the script for the Identity Database, you have the three roles loaded for you with Manager, Clerk & Customer.</p>

#### Users
<ul>
  <li>Manager: manager@gmail.com & Password-123</li>
  <li>Clerk: clerk@gmail.com & Password-123</li>
  <li>Customer: customer@gmail.com & Password-123</li>
</ul>

DOCKER (Not implemented)


