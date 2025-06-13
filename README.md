# NanoTrackingSystem
📋 Quick Brief:
A comprehensive business process automation system that allows organizations to:

Create workflows with multiple approval steps
Assign tasks based on user roles (Admin, Manager, Finance, HR, Employee)
Track processes in real-time with complete audit trail
Validate steps with external API integration
Monitor performance through dashboards and analytics
Tech Stack: .NET 8 Web API + Angular 15 + SQL Server + JWT Authentication + CQRS Pattern

📋 How to Run the Application:
Prerequisites
.NET 8 SDK
Node.js 18+
SQL Server (LocalDB is fine)
Backend Setup

# 1. Clone and navigate to project
# 2. Update connection string in API/appsettings.development.json
# 3. Run database migration (creates tables + seeds data automatically)
# 4. Start the API
# API runs on: https://localhost:7000

Frontend Setup
# 1. Navigate to Angular app
# 2. Install dependencies
--> npm install
# 3. Update API URL in src/environments/environment.ts
apiUrl: 'https://localhost:7000/api/v1'
# 4. Start Angular app
--> ng serve
# App runs on: http://localhost:4200
Note: Database seeding happens automatically on first run - creates all roles and test users.

📋 Business Process Flow
Step 1: Admin Creates Workflow
Admin logs in → Creates "Purchase Approval" workflow:
├── Step 1: Employee submits request
├── Step 2: Manager approves/rejects  
├── Step 3: Finance final approval (with external validation)
└── Step 4: Process completed

Step 2: Employee Starts Process
Employee logs in → Starts new process:
├── Selects "Purchase Approval" workflow
├── System creates process instance
├── First step automatically assigned to employee
└── Employee receives task in "My Tasks"

Step 3: Process Execution
Task Execution Flow:
├── Employee: Submits purchase request details
├── System: Auto-assigns to Manager based on role
├── Manager: Reviews and approves/rejects
├── System: Validates via external API (if required)
├── Finance: Final approval based on validation result
└── System: Completes process, notifies all parties

Step 4: Real-time Tracking
All users can monitor:
├── Dashboard: Shows pending tasks, active processes
├── Process List: Real-time status of all workflows
├── My Tasks: Personal task queue with actions
└── Audit Trail: Complete history of all actions


👥 Test Users (Auto-created during seeding)
Role	   Email	                    Password	                       What They Can Do
Admin	admin@nanohealth.com	        Admin123!	         Create workflows, manage users, full system access
Manager	manager@nanohealth.com	    Manager123!	      Approve requests, view all processes, start workflows
Finance	finance@nanohealth.com	    Finance123!	      Handle financial approvals, budget validations
Employee	employee@nanohealth.com	  Employee123!	    Submit requests, execute assigned tasks


📋 Quick Test Scenario
Login as Admin → Create a workflow
Login as Employee → Start a process
Login as Manager → Approve the task
Login as Finance → Final approval
Check process completion in dashboard

# I hope you like it
