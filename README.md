# 🚀 Cross-Site Monitoring Web Application

This is a multi-tier web application designed for comprehensive **Site Monitoring**. It was developed to provide a robust, high-performance, and secure platform for managing and observing site-specific data. This project showcases a practical application of clean architecture patterns in a professional environment.

---

### ✨ Key Features

* **Multi-Tier Architecture:** Clear separation of concerns into Presentation (`Csm.Web`), Service (`Csm.Services`), and Data Access (`CSM.Dal`) layers.
* **Secure Authentication (JWT):** Utilizes **JSON Web Tokens (JWT)** for stateless, secure authentication of API requests.
* **High-Performance Data Access:** Leverages the **Dapper** micro-ORM for fast, efficient, and direct access to the database.
* **Unit of Work Pattern:** Implements the **Unit of Work Pattern** with explicit transaction management to ensure **transaction-based data persistence** and integrity for all complex operations.
* **PostgreSQL Support:** Utilizes the robust **Npgsql** provider for connecting to and interacting with the PostgreSQL database.

---

### ⚙️ Technology Stack

| Component | Technology | Description |
| :--- | :--- | :--- |
| **Frontend/UI** | ASP.NET MVC (C#) | The presentation layer for user interaction. |
| **Backend/API** | C# / .NET | Provides core business logic and services. |
| **Authentication** | **JWT (JSON Web Token)** | Token-based security for API resources. |
| **Database** | PostgreSQL | The relational database used for data storage. |
| **Data Access** | **Dapper** (Micro-ORM) | Used for high-speed data mapping and persistence. |
| **Architecture** | Unit of Work & Repository Patterns | Design patterns ensuring scalable, transaction-based, and testable code. |

---

### 🔐 Authentication

The application uses **JWT Bearer Authentication** to secure the API endpoints that power the ASP.NET MVC frontend.

1.  Upon successful login, the application issues a **JWT**.
2.  This token is then included in the `Authorization` header of subsequent API requests to protected resources.
3.  The backend validates the token's signature, issuer, and expiration date before granting access.

---

### 🛠️ Getting Started

Follow these steps to set up the project locally:

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/utsabshrestha/CrossSiteMonitoring.git](https://github.com/utsabshrestha/CrossSiteMonitoring.git)
    ```
2.  **Prerequisites:**
    * .NET Framework / .NET SDK (Specify version if known)
    * **PostgreSQL** Database Instance
    * Visual Studio (or preferred IDE)

3.  **Database & Connection Configuration:**
    * Update the database connection string (key is typically `Csmdb`) in the configuration file (e.g., `web.config` or `appsettings.json` within the `Csm.Web` project) with your **PostgreSQL** credentials.
    * Ensure the required database schema and tables are created.

4.  **Build and Run:**
    * Open `CrossSiteMonitoringProject.sln` in Visual Studio.
    * Restore NuGet packages.
    * Set the `Csm.Web` project as the startup project and run the application.

---

### 🤝 Contributions & License

This project was developed for internal company use and is maintained as a personal portfolio piece.

The project is released under the **MIT License**.
