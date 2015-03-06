# NServiceBus-RoutingInDataBase

The main part, where the tricks are done, is the DynamicRouting project, here we can find:

- DynamicRoutingBehaviour: there I inject a new step in the pipeline.  In that step addresses for a message type are searched and a round robin is made with them.

- DynamicRoutingSubscriptionStorage: A new subscription storage is implemented. In that storage subscribers are searched in database. A round robin is done there if several SubscriberEndpoint with the same name are found. Also a new PersistenceDefinition and a new Feature are implemented.

- DynamicRoutingConfiguration: NServiceBus checks if an endpoint exists in the configuration, for that every endpoint in the database is introduced in the configuration but anything is done with it after that.
