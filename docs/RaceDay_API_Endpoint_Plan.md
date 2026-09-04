API Planning 

The API will provide communication between the RaceDay application and the database. The API will contain endpoints for authentication, user profiles, events, event types, categories, enrolments and results. Each endpoint will specify the HTTP method, route, description, role required, request body and expected response. 

Authentication Endpoints 

Method 

 

Route 

 

Description 

 

Role Required 

 

POST 

 

/api/auth/register 

 

Register a new account 

Public 

POST 

 

/api/auth/login 

 

Log into the system 

 

Public 

 

  

  

User Profile Endpoints 

Method 

 

Route 

 

Description 

 

Role Required 

 

GET 

 

/api/users/me 

 

View own profile 

 

Organiser / Participant 

PUT 

 

/api/users/me 

 

Update own profile 

 

Organiser / Participant 

  

Event Endpoints 

Method 

 

Route 

 

Description 

 

Role Required 

 

GET 

 

/api/events 

 

View all events 

 

Organiser / Participant 

GET 

 

/api/events/{id} 

 

View one event 

 

Organiser / Participant 

POST 

 

/api/events 

 

Create an event 

 

Organiser 

 

PUT 

 

/api/events/{id} 

 

Update an event 

 

Organiser 

 

DELETE 

 

/api/events/{id} 

 

Delete an event 

 

Organiser 

 

  

Event Type Endpoint 

Method 

 

Route 

 

Description 

 

Role Required 

 

GET 

 

/api/eventtypes 

 

View event types 

 

Organiser / Participant 

  

Category Endpoints 

Method 

 

Route 

 

Description 

 

Role Required 

 

GET 

 

/api/events/{eventId}/categories 

View event categories 

Organiser / Participant 

POST 

 

/api/events/{eventId}/categories 

Create a category 

 

Organiser 

 

PUT 

 

/api/categories/{id} 

 

Update a category 

 

Organiser 

 

DELETE 

 

/api/categories/{id} 

 

Delete a category 

 

Organiser 

 

  

Enrolment Endpoints 

Method 

 

Route 

 

Description 

 

Role Required 

 

POST 

 

/api/events/{eventId}/enrolments 

Enroll in an event 

 

Participant 

 

GET 

 

/api/users/me/enrolments 

View own enrolments 

Participant 

 

GET 

 

/api/events/{eventId}/enrolments 

View event enrolments 

 

Organiser 

 

DELETE 

 

/api/enrolments/{id} 

 

Cancel an enrolment 

 

Participant 

 

  

  

  

  

  

  

Result Endpoints 

Method 

 

Route 

 

Description 

 

Role Required 

 

POST 

 

/api/enrolments/{enrolmentId}/result 

Add a result 

 

Organiser 

 

GET 

 

/api/users/me/results 

View own results 

 

Participant 

 

GET 

 

/api/events/{eventId}/results 

 

View event results 

 

Organiser / Participant 

 

   

Expected API Error Responses  

Status Code - 200  

Meaning - Request was successful  

 

Status Code - 201  

Meaning - Resource was successfully created  

 

Status Code - 204  

Meaning - Request was successful with no content returned  

 

Status Code - 400  

Meaning - Invalid request or information  

  

Status Code - 401  

Meaning - User is not authenticated  

  

Status Code - 403   

Meaning - User does not have permission  

 

Status Code - 404  

Meaning - Requested item was not found  

 

Status Code - 409  

Meaning - Conflict, such as duplicate enrolment  

  

Status Code - 500  

Meaning - Server error 

## Database Entities

The RaceDay database contains six main entities:

- Users
- Events
- EventTypes
- Categories
- Enrolments
- Results
 
