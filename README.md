# Saga-Pattern-Choreography-Eventual-Consistency
The aim of this project is to process the  Eventual Consistency  by following the best practices with  Saga Pattern Choreography  on NETCore 

### Necessary For Docker
+ RabbitMQ : **docker run -d -p 15672:15672 -p 5672:5672 --name rabbitmqcontainer rabbitmq:3-management**
+ PostgreSQL : **docker run --name postgrescontainer -e POSTGRES_PASSWORD=yourpassword -d -p 5432:5432 postgres**

### Eventual Consistency - Saga Pattern
+ It is a transaction management pattern used in distributed systems and microservices architectures.
+ Other systems come into play in sequence based on the success status.
+ There is no atomic state.
+ In case of possible errors, the Compensable Transaction will be activated, and all performed operations will be rolled back.
    + This ensures data consistency.
+ What is the purpose of the Saga?
    + It aims to provide a model for managing long-lived and complex transactions in distributed systems and microservices.
    + It aims to make the system more modular and scalable.
+ There may be short delays between services.
+ It has two different approaches: Events/Choreography and Command/Orchestration.

#### Events/Choreography
+ Communication between microservices is done without a central control and communication point through events.
+ Message brokers are commonly used for inter-service communication.
+ The transaction starts with the first service, then it performs its service and sends a successful (later) or unsuccessful (before) event to the next service, and so on.
+ Each service is in a decision-making position itself.
+ It is usually preferred when the number of microservices is between 2 and 4.
+ Each service listens to the queue, performs the operation, and then provides information about success or failure as an event.
+ As a drawback, if the number of services increases, it becomes challenging to manage which service is listening to which event.

### Project Arch
![Project Arch](https://github.com/tahapek5454/Saga-Pattern-Choreography-Eventual-Consistency/blob/main/Docs/saga.png)
