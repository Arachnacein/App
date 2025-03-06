<h3 align="center">I will be spending Christmas writing unit tests. 🎄💻 </h3>
<h1 align="center"> ExpenseApp</h1>
<p align="">
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Since I started working, I noticed that it’s beneficial dividing income into specific categories. I divided them into 'savings', 'daily expenses', and those related to 'entertainment'.
  This allowed me to manage my budget more effectively, and the savings I generated helped build a financial cushion. In the beginning, I did this on paper. 
  In fact, long before I started working, I would note my expenses on sheets of paper, which often got lost. That's why I decided to switch to electronic record-keeping. 
  I chose Excel spreadsheets for this purpose. However, I didn't really like it due to its clunky appearance and lack of functionalities I wanted.
</p>
<p align="">
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;In the meantime, I decided to start designing my own application. It was supposed to automate the processes I previously had to calculate and manage manually.
  Additionally, I wanted it to be simple to use and include the functionalities I needed. The best part is that I can design and implement features by myself. 
  Without much hesitation, I began creating this application.
</p>
<br>
<p align="center">
  <img src="https://github.com/user-attachments/assets/61bcef4b-c00f-4546-8b88-b5fef3cad395" style="border-radius:15px;">
</p>

<br>
<h2 align="center"> How it works </h2>
<p align="">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;In the 'Budget' tab, you can view your monthly expenses. It's best to start by adding the income for the current month. This includes any funds added to your budget, such as a salary, winnings from a bet, or even money found on the street. When adding the first income for the month, the system will prompt you to select a pattern for distributing your income. Choose the one that suits you best. If none of the available patterns fit, you can create a new one in the options window. After successfully adding the income, the application will automatically allocate it to various categories according to the selected pattern and update the data in the tables. You can now start adding individual transactions. (Transactions can also be added without selecting a pattern or having any income for the month. In that case, only the transaction itself will be visible without a percentage allocation.) If you make a mistake with a category, you can easily move the transaction between columns. When you hover over a transaction with the mouse, the date and the description you entered will appear after a moment. If you click on the transaction, a window will open where you can edit or delete it.
</p>


<br>
<h2 align="center"> Architecture </h2>
<p align="">
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;The architecture used in this application is based on Clean Architecture, which has become the standard for developing WebAPIs. 
  It separates business logic from databases and user interfaces, creating a layered structure where each layer communicates with the others in a well-defined manner.
   <br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Additionally, the application is built using a distributed architecture, specifically microservices. It is divided into modules, such as the API responsible for business logic related to transactions and budgeting, and the graphical user interface. Thesemodules 
  are connected by an API Gateway, which routes requests between the modules. My assumption is that each group of functionalities related to a single aspect will be implemented as a separate microservice.
</p>
<br>
<h3 align="center">How architecture was changing</h3>
<p>
   Firstly, the application had three microservices: 
  budgetapi, apigateway, and GUI. In the future, microservices responsible for the calendar, user and payments will be added.
</p>
<br>
<p align="center">
  <img src="https://github.com/user-attachments/assets/3df263dc-c8b6-472e-9d22-79521cbb3045">
</p>
<br>
<p align="">
	Currently, I added IdentityApi that manages user actions. This WebApi works together with Keycloak API. Keycloak is an open-source identity and access 
	management platform. All user-related requests, such as login, registration, token retrieval, and fetching user resources, go through IdentityApi, 
	which redirects these requests to Keycloak.
</p>
<br>
<p align="center">
	<img src="https://github.com/user-attachments/assets/1dd99578-8045-4595-b69c-d7def4ad583d">
</p>



<br><br>
<h2 align="center"> Screenshots </h2><br>
<p align="center">
  <img src="https://github.com/user-attachments/assets/c77b88b4-3a76-4a07-b70b-ecccdb6c9e9b">
  <img src="https://github.com/user-attachments/assets/3b16c174-0c13-4cf3-b98c-6dc6778ab35d">
  <img src="https://github.com/user-attachments/assets/2b4283c6-75e0-4854-b191-1e4ca9a62743">
  <img src="https://github.com/user-attachments/assets/3f6856fb-b394-4458-be47-8a90e16b2c08">
  <img src="https://github.com/user-attachments/assets/a37be03e-b789-47ad-8fca-eb0721aff4e1" style="width:450px;height:450px;">
  <img src="https://github.com/user-attachments/assets/19aff8be-b687-4ad6-bfb1-1c7948409e1a" style="width:450px;height:450px;">
  <img src="https://github.com/user-attachments/assets/ee76c1dc-6edd-4b90-9c1b-5c33003d6ddf" style="width:70%;height:70%;">

</p>

<br>
<h2 align="center"> Tech stack </h2>
<div align="center">
	<code><img width="50" src="https://user-images.githubusercontent.com/25181517/121405754-b4f48f80-c95d-11eb-8893-fc325bde617f.png" alt=".NET Core" title=".NET Core"/></code>
	<code><img width="50" src="https://user-images.githubusercontent.com/25181517/192107858-fe19f043-c502-4009-8c47-476fc89718ad.png" alt="REST" title="REST"/></code>
	<code><img width="80" src="https://github.com/user-attachments/assets/d58f38d7-ec29-4343-b06d-81d21668c758" alt="Keycloak" title="Keycloak" /></code>
	<code><img width="50" src="https://user-images.githubusercontent.com/25181517/192108372-f71d70ac-7ae6-4c0d-8395-51d8870c2ef0.png" alt="Git" title="Git"/></code>
	<code><img width="50" src="https://user-images.githubusercontent.com/25181517/117207330-263ba280-adf4-11eb-9b97-0ac5b40bc3be.png" alt="Docker" title="Docker"/></code>
	<code><img width="50" src="https://user-images.githubusercontent.com/25181517/192108374-8da61ba1-99ec-41d7-80b8-fb2f7c0a4948.png" alt="GitHub" title="GitHub"/></code>
	<code><img width="50" src="https://github.com/marwin1991/profile-technology-icons/assets/19180175/3b371807-db7c-45b4-8720-c0cfc901680a" alt="MSSQL" title="MSSQL"/></code>
	<code><img width="50" src="https://user-images.githubusercontent.com/25181517/192109061-e138ca71-337c-4019-8d42-4792fdaa7128.png" alt="Postman" title="Postman"/></code>
	<code><img width="50" src="https://user-images.githubusercontent.com/25181517/192158954-f88b5814-d510-4564-b285-dff7d6400dad.png" alt="HTML" title="HTML"/></code>
	<code><img width="50" src="https://user-images.githubusercontent.com/25181517/183898674-75a4a1b1-f960-4ea9-abcb-637170a00a75.png" alt="CSS" title="CSS"/></code>
	<code><img width="50" src="https://user-images.githubusercontent.com/25181517/121405384-444d7300-c95d-11eb-959f-913020d3bf90.png" alt="C#" title="C#"/></code>
	<code><img width="50" src="https://user-images.githubusercontent.com/25181517/186884150-05e9ff6d-340e-4802-9533-2c3f02363ee3.png" alt="Windows" title="Windows"/></code>
</div>

<br>
<h2 align="center"> Milestones </h2>
<p>
  🔜 Add Recurring transactions. <br>
  * Add pdf summary. <br>
  * Add Healtch checks.<br>
  * Add 2FA <br>
  * Add saving goals feature. <br>
  * Add calendar with notifications.<br>
  * Add payment feature. <br>
  ✅ Add Admin Panel. <br>
  ⛔ Add authorization. => Keycloak BUG🐛 <br>
  ✅ Add email verification. => Completed but Keycloak BUG 🐛<br> 
  ✅ Add Unit test. <br>
  ✅ Add Login and Registration. <br>
  ✅ Add Internationalization.<br>
  ✅ Add searching fields for transactions and incomes. <br>
  ✅ Add statistics summary.<br>
  ✅ Change logic by introducing the CQRS architectural pattern. <br>
  ✅ Add income preview. <br>
  ✅ Add patterns preview. <br>
  ✅ Add forms validators. <br>
  ✅ Add dark theme. <br>
  ✅ Add main page carousel. <br>
  ✅ Add dialogs. <br>
  ✅ Add budget gui page. <br>
  ✅ Add income api (controllers, services, respositories, mappers ect.). <br>
  ✅ Add transactions api (controllers, services, respositories, mappers ect.). <br>
  ✅ Add patterns and monthpatterns api (controllers, services, respositories, mappers ect.). <br>
  ✅ Set project architecture.<br>
  ❔❔ Mobile app. <br>
</p>
<center> <img src="https://github.com/user-attachments/assets/1c97b389-d3b9-45f2-9cae-022b50ac28cc" />
<img src="https://github.com/user-attachments/assets/cb09970b-3cc5-4962-bf91-220a1abf56d9" width="265" height="70" /></center>



