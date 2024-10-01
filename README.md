<h1 align="center"> ExpenseApp</h1>
<p align="">
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Since I started working, I noticed that it’s beneficial dividing income into specific categories. I divided them into 'savings', 'daily expenses', and those related to 'entertainment'.
  This allowed me to manage my budget more effectively, and the savings I generated helped build a financial cushion. In the beginning, I did this on paper. 
  In fact, long before I started working, I would note my expenses on sheets of paper, which often got lost. That's why I decided to switch to electronic record-keeping. 
  I chose Excel spreadsheets for this purpose. However, I didn't really like it due to its clunky appearance and lack of functionalities I wanted.
</p>
<p align="">
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;In the meantime, I decided to start designing my own application. It was supposed to automate the processes I previously had to calculate and manage manually.
  Additionally, I wanted it to be simple to use and include the functionalities I needed. The best part is that I can design and implement features myself. 
  Without much hesitation, I began creating this application.
</p>
<br>
<p align="center">
  <img src="https://github.com/user-attachments/assets/fba767e5-cd58-4c3d-8f9b-715de93393d7" style="border-radius:15px;">
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
  Additionally, the application is built using a distributed architecture, specifically microservices. It is divided into modules, such as the API responsible for business logic related to transactions and budgeting, and the graphical user interface. Thesemodules 
  are connected by an API Gateway, which routes requests between the modules. 
  My assumption is that each group of functionalities related to a single aspect will be implemented as a separate microservice. Currently, the application has three microservices: 
  budgetapi, apigateway, and GUI. In the future, microservices responsible for the calendar and payments will be added.
  
</p>
<p align="center">
  <img src="https://github.com/user-attachments/assets/3df263dc-c8b6-472e-9d22-79521cbb3045">
</p>

